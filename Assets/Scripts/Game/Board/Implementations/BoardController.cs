using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration.DataStructures;
using Game.Board.Interfaces;
using Random = UnityEngine.Random;

namespace Game.Board.Implementations
{
    public class BoardController : IBoardController
    {
        public BlockType[,] Blocks { get; private set; }

        private List<ObjectAppearData> blockProbabilities;
        private float probabilitySum;

        public BoardController(List<ObjectAppearData> objectProbabilities)
        {
            blockProbabilities = objectProbabilities;
            probabilitySum = objectProbabilities.Sum(data => data.Probability);
        }

        public void GenerateBoard(int boardWidth, int boardHeight)
        {
            Blocks = new BlockType[boardWidth, boardHeight];
            for (var x = 0; x < boardWidth; x++)
            for (var y = 0; y < boardHeight; y++)
            {
                Blocks[x, y] = GenerateTile();
                //generates board with no moves
                // Blocks[x, y] = (BlockType) ((x + y * 2) % 5);
            }
        }

        public void RemoveBlocks(List<BoardCoordinates> chain)
        {
            foreach (var block in chain)
            {
                Blocks[block.X, block.Y] = BlockType.None;
            }

            var uniqueXPos = new HashSet<int>(chain.Select(ints => ints.X));

            foreach (var x in uniqueXPos)
            {
                var y = Blocks.GetLength(1) - 1;
                var targetY = -1;
                var toSpawn = 0;
                while (y >= 0)
                {
                    var tile = Blocks[x, y];
                    if (tile == BlockType.None)
                    {
                        toSpawn++;
                        if (targetY == -1)
                        {
                            targetY = y;
                        }
                    }
                    else if (targetY != -1)
                    {
                        Blocks[x, targetY] = tile;
                        Blocks[x, y] = BlockType.None;
                        targetY--;
                    }

                    y--;
                }

                for (var i = 0; i < toSpawn; i++)
                {
                    var tile = GenerateTile();
                    Blocks[x, targetY - i] = tile;
                }
            }
        }

        public bool ValidatePosition(BoardCoordinates coords)
        {
            return coords.X < 0 ||
                   coords.X >= Blocks.GetLength(0) ||
                   coords.Y < 0 ||
                   coords.Y >= Blocks.GetLength(1);
        }

        public void Dispose()
        {
            Blocks = null;
            blockProbabilities = null;
            probabilitySum = 0;
        }

        public async Task<bool> CheckBoardForPossibleMoves()
        {
            var xLen = Blocks.GetLength(0);
            var yLen = Blocks.GetLength(1);

            for (var x = 0; x < xLen; x++)
            for (var y = 0; y < yLen; y++)
            {
                if (NeighboursCheck(x, y, xLen, yLen))
                {
                    return true;
                }

                await Task.Yield();
            }

            return false;
        }

        private BlockType GenerateTile()
        {
            var random = Random.Range(0, probabilitySum);
            foreach (var t in blockProbabilities)
            {
                if (random < t.Probability)
                {
                    return t.type;
                }

                random -= t.Probability;
            }

            return BlockType.Any;
        }

        private bool NeighboursCheck(int x, int y, int xLen, int yLen)
        {
            var xLower = x - 1 < 0 ? 0 : x - 1;
            var xHigh = x + 1 >= xLen ? xLen - 1 : x + 1;
            var yLower = y - 1 < 0 ? 0 : y - 1;
            var yHigh = y + 1 >= yLen ? yLen - 1 : y + 1;

            var count = 0;
            for (var i = xLower; i <= xHigh; i++)
            for (var j = yLower; j <= yHigh; j++)
            {
                if (x == i && y == j)
                {
                    continue;
                }

                if (Blocks[x, y] == Blocks[i, j])
                {
                    count++;
                }

                if (count >= 2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}