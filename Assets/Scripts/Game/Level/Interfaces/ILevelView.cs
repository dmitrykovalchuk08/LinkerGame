using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration.DataStructures;
using ContentProvider;
using CoordinatesConvertor;

namespace Game.Level.Interfaces
{
    public interface ILevelView
    {
        void Initialize(
            ICoordinatesProvider coordProvider,
            IVisualContentProvider contentProvider,
            GameVisualData visualConfig);

        Task InitializeLevel(int x, int y, BlockType[,] board);
        void SelectBlock(BoardCoordinates coord);
        void DeselectBlock(BoardCoordinates coord);
        Task RemoveBlocks(List<BoardCoordinates> chain, BlockType[,] newBoard);
        Task DropBoard();
        void ClearBoard();
    }
}