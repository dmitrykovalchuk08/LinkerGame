using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration.DataStructures;

namespace Game.Board.Interfaces
{
    public interface IBoardController: IDisposable
    {
        BlockType[,] Blocks { get; }

        void GenerateBoard(int boardWidth, int boardHeight);

        void RemoveBlocks(List<BoardCoordinates> chain);

        bool ValidatePosition(BoardCoordinates coords);

        Task<bool> CheckBoardForPossibleMoves();
    }
}