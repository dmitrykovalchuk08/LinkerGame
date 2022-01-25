using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Configuration.DataStructures;
using ContentProvider;
using CoordinatesConvertor;
using Pool.Implementation;
using Pool.Interfaces;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class BoardVisual : MonoBehaviour
    {
        [SerializeField] private Transform boardBackgroundRoot;
        [SerializeField] private Transform boardObjectsRoot;
        [SerializeField] private Transform boardMask;
        private ICoordinatesProvider coordinatesProvider;
        private BackgroundTileView[,] boardBackground;
        private BlockView[,] boardBlocks;
        private IVisualContentProvider contentProvider;
        private GameVisualData visualData;
        private IObjectPool<BackgroundTileView> boardBackgroundPool;
        private BlocksPooledProvider blocksPooledProvider;
        private SynchronizationContext unitySynchronizationContext;

        public void Initialize(
            ICoordinatesProvider iCoordinatesProvider,
            IVisualContentProvider iContentProvider,
            GameVisualData gameVisualData)
        {
            unitySynchronizationContext = SynchronizationContext.Current;
            contentProvider = iContentProvider;
            coordinatesProvider = iCoordinatesProvider;
            visualData = gameVisualData;
            boardBackgroundPool = new GameObjectPool<BackgroundTileView>();
            ((GameObjectPool<BackgroundTileView>) boardBackgroundPool)
                .InitializePool(100, contentProvider.GetBackgroundTile);
            blocksPooledProvider = new BlocksPooledProvider(contentProvider);
            blocksPooledProvider.CreatePool(BlockType.Any, 50);
            blocksPooledProvider.CreatePool(BlockType.Blue, 50);
            blocksPooledProvider.CreatePool(BlockType.Green, 50);
            blocksPooledProvider.CreatePool(BlockType.Pink, 50);
            blocksPooledProvider.CreatePool(BlockType.Red, 50);
            blocksPooledProvider.CreatePool(BlockType.Yellow, 50);
        }

        public async Task BuildBoard(int sizeX, int sizeY)
        {
            boardBackground = new BackgroundTileView[sizeX, sizeY];
            boardBlocks = new BlockView[sizeX, sizeY];
            coordinatesProvider.UpdateOffsetForBoard(sizeX, sizeY);

            for (var x = 0; x < sizeX; x++)
            for (var y = 0; y < sizeY; y++)
            {
                var tile = await boardBackgroundPool.Get();
                tile.Setup(boardBackgroundRoot, coordinatesProvider, visualData);
                tile.SetPosition(new BoardCoordinates(x, y));
                boardBackground[x, y] = tile;
            }

            boardMask.transform.localScale = new Vector3(
                sizeX * visualData.CellSize,
                sizeY * visualData.CellSize,
                0
            );
        }

        public async Task FillBoard(BlockType[,] board)
        {
            var anims = new List<Task>();
            for (var x = 0; x < board.GetLength(0); x++)
            for (var y = 0; y < board.GetLength(1); y++)
            {
                var tile = await blocksPooledProvider.GetBoardObject(board[x, y]);
                tile.Setup(boardObjectsRoot, coordinatesProvider, visualData);
                tile.SetPosition(new BoardCoordinates(x, -1));
                boardBlocks[x, y] = tile;
                // this delays will create nice board fill
                // from left-to-right
                var delay =
                    visualData.BlockAnimationConfiguration.FallDelay *
                    (board.GetLength(1) - y + x);
                anims.Add(tile.AnimateToPosition(new BoardCoordinates(x, y), delay));
            }

            await Task.WhenAll(anims);
        }

        public async Task RemoveBlocks(List<BoardCoordinates> chain, BlockType[,] newBoard)
        {
            var awaiters = new List<Task>();
            foreach (var block in chain)
            {
                var tile = boardBlocks[block.X, block.Y];
                boardBlocks[block.X, block.Y] = null;
                var tcs = new TaskCompletionSource<bool>();
                awaiters.Add(tcs.Task);
                _ = tile.AnimateToDestroy()
                    .ContinueWith(task =>
                    {
                        try
                        {
                            unitySynchronizationContext.Post(state =>
                                {
                                    blocksPooledProvider.PutBack(tile);
                                    tcs.SetResult(true);
                                }
                                , this);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }

                        return task;
                    });
            }

            await Task.WhenAll(awaiters);
            awaiters.Clear();

            var uniqueXPos = new HashSet<int>(chain.Select(ints => ints.X));

            foreach (var x in uniqueXPos)
            {
                var toSpawn = 0;
                var y = boardBlocks.GetLength(1) - 1;
                var targetY = -1;
                while (y >= 0)
                {
                    var tile = boardBlocks[x, y];
                    if (tile == null)
                    {
                        toSpawn++;
                        if (targetY == -1)
                        {
                            targetY = y;
                        }
                    }
                    else if (targetY != -1)
                    {
                        boardBlocks[x, targetY] = tile;
                        boardBlocks[x, y] = null;
                        awaiters.Add(tile.AnimateToPosition(new BoardCoordinates(x, targetY)));
                        targetY--;
                    }

                    y--;
                }

                for (var i = 0; i < toSpawn; i++)
                {
                    var tileGO = await contentProvider.GetBoardObject(newBoard[x, targetY - i]);
                    var tile = tileGO.GetComponent<BlockView>();
                    tile.Setup(boardObjectsRoot, coordinatesProvider, visualData);
                    tile.SetPosition(new BoardCoordinates(x, -i - 1));
                    boardBlocks[x, targetY - i] = tile;
                    awaiters.Add(tile.AnimateToPosition(new BoardCoordinates(x, targetY - i)));
                }
            }

            await Task.WhenAll(awaiters);
        }

        public void SelectBlock(BoardCoordinates coordinates)
        {
            boardBlocks[coordinates.X, coordinates.Y].SetSelected(true);
        }

        public void DeselectBlock(BoardCoordinates coordinates)
        {
            boardBlocks[coordinates.X, coordinates.Y].SetSelected(false);
        }

        public async Task DropBoard()
        {
            var awaiters = new List<Task>();
            var fallToPos = boardBlocks.GetLength(1) + 1;
            foreach (var block in boardBlocks)
            {
                var tcs = new TaskCompletionSource<bool>();
                awaiters.Add(tcs.Task);

                _ = block.FallToDestroy(fallToPos)
                    .ContinueWith(task =>
                    {
                        try
                        {
                            unitySynchronizationContext.Post(state =>
                                {
                                    blocksPooledProvider.PutBack(block);
                                    tcs.SetResult(true);
                                }
                                , this);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return task;
                    });
            }

            await Task.WhenAll(awaiters);
            awaiters.Clear();
        }

        public void ClearBoard()
        {
            foreach (var bg in boardBackground)
            {
                boardBackgroundPool.Release(bg);
            }

            foreach (var bb in boardBlocks)
            {
                blocksPooledProvider.PutBack(bb);
            }

            boardBackground = null;
            boardBlocks = null;
        }
    }
}