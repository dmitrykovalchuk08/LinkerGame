using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration.DataStructures;
using ContentProvider;
using CoordinatesConvertor;
using Game.Level.Interfaces;
using LineDrawer.Interfaces;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class LevelView : MonoBehaviour, ILevelView
    {
        [SerializeField] private BoardVisual boardVisual;
        [SerializeField] private AbstractLineDrawer lineDrawer;

        private ICoordinatesProvider coordinatesProvider;

        public void Initialize(
            ICoordinatesProvider coordProvider,
            IVisualContentProvider contentProvider,
            GameVisualData visualConfig)
        {
            coordinatesProvider = coordProvider;
            boardVisual.Initialize(coordProvider, contentProvider, visualConfig);
        }

        public async Task InitializeLevel(int x, int y, BlockType[,] board)
        {
            await boardVisual.BuildBoard(x, y);
            await boardVisual.FillBoard(board);
        }

        public void SelectBlock(BoardCoordinates coord)
        {
            lineDrawer.DrawLineToPosition(coordinatesProvider.BoardCoordinatesToWorld(coord));
            boardVisual.SelectBlock(coord);
        }

        public void DeselectBlock(BoardCoordinates coord)
        {
            boardVisual.DeselectBlock(coord);
            lineDrawer.RemoveLastLine();
        }

        public async Task RemoveBlocks(List<BoardCoordinates> chain, BlockType[,] newBoard)
        {
            lineDrawer.ClearAll();
            await boardVisual.RemoveBlocks(chain, newBoard);
        }

        public async Task DropBoard()
        {
            lineDrawer.RemoveLastLine();
            await boardVisual.DropBoard();
        }

        public void ClearBoard()
        {
            lineDrawer.ClearAll();
            boardVisual.ClearBoard();
        }
    }
}