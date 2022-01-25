using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration.DataStructures;
using Game.Board.Implementations;
using Game.Board.Interfaces;
using Game.Level.Interfaces;
using Game.LevelGoals;
using Game.LevelGoals.Interfaces;
using PlayerInput;
using PlayerInput.Interfaces;
using Signal.DataStructures;
using Signal.Implementation;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Level.LevelController
{
    public class LevelController : ILevelController, IInputListener, ILevelGoalListener
    {
        private ILevelView levelView;
        private LevelConfiguration configuration;
        private TaskCompletionSource<bool> levelCompletionTask;
        private IPlayerInput input;
        private List<BoardCoordinates> chain;
        private BlockType selectedBlockType;
        private IBoardController boardController;
        private ILevelGoalChecker levelGoalChecker;
        private bool canHandleInput;
        private int movesMade;

        public Task Initialize(ILevelView view)
        {
            levelView = view;
            chain = new List<BoardCoordinates>();
            return Task.CompletedTask;
        }

        public async Task<Task<bool>> PlayLevel(LevelConfiguration config)
        {
            canHandleInput = false;
            levelCompletionTask = new TaskCompletionSource<bool>();
            boardController = new BoardController(config.ObjectProbabilities);
            try
            {
                configuration = config;
                movesMade = 0;
                boardController.GenerateBoard(configuration.BoardWidth, configuration.BoardHeight);
                await levelView.InitializeLevel(
                    configuration.BoardWidth,
                    configuration.BoardHeight,
                    boardController.Blocks);

                input = InputModule.GetInput();
                input.RegisterHandler(this);

                CreateLevelGoals(config);

                await CheckNoMoves();

                canHandleInput = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                levelCompletionTask.SetException(e);
            }

            return levelCompletionTask.Task;
        }

        public async Task ReceiveInput(BoardCoordinates coords, InputState state)
        {
            if (!canHandleInput)
            {
                return;
            }

            //check out of bounds
            if (state != InputState.Ended &&
                boardController.ValidatePosition(coords))
            {
                return;
            }

            switch (state)
            {
                case InputState.Started:
                    StartSelection(coords);
                    break;
                case InputState.InProgress:
                    TrySelectBlock(coords);
                    break;
                case InputState.Ended:
                    await OnEndSelection();
                    break;
            }
        }

        private void CreateLevelGoals(LevelConfiguration config)
        {
            levelGoalChecker = new LevelGoalChecker();
            levelGoalChecker.Initialize(config.WinConditions);
            levelGoalChecker.RegisterListener(this);
        }

        public void LevelWon()
        {
            levelCompletionTask.SetResult(true);
            ClearLevel();
        }

        public void LevelFailed()
        {
            levelCompletionTask.SetResult(false);
            ClearLevel();
        }

        public void LevelUpdated(LevelGoal goal)
        {
            StaticSignalBus.DispatchSignal(
                SignalType.LevelGoalUpdated,
                new LevelGoalUpdatedData(goal)
            );
        }

        private void ClearLevel()
        {
            input.UnregisterHandler(this);
            input = null;
            boardController.Dispose();
            boardController = null;
            configuration = null;
            levelCompletionTask = null;
            levelGoalChecker.UnregisterListener(this);
            levelView.ClearBoard();
        }

        private void StartSelection(BoardCoordinates coords)
        {
            chain.Clear();
            chain.Add(coords);
            selectedBlockType = boardController.Blocks[coords.X, coords.Y];
            levelView.SelectBlock(coords);
        }

        private void TrySelectBlock(BoardCoordinates coords)
        {
            var thisBlockType = boardController.Blocks[coords.X, coords.Y];
            if (thisBlockType != selectedBlockType)
            {
                return;
            }

            var last = chain[chain.Count - 1];
            var delta = coords - last;
            if (math.abs(delta.X) > 1 || math.abs(delta.Y) > 1)
            {
            }
            else if (chain.Count > 1 && chain[chain.Count - 2] == coords)
            {
                chain.RemoveAt(chain.Count - 1);
                levelView.DeselectBlock(last);
            }
            else
            {
                if (chain.Contains(coords))
                {
                    return;
                }

                chain.Add(coords);
                levelView.SelectBlock(coords);
            }
        }

        private async Task OnEndSelection()
        {
            canHandleInput = false;
            if (chain.Count < 3)
            {
                foreach (var coord in chain)
                {
                    levelView.DeselectBlock(coord);
                }
            }
            else
            {
                boardController.RemoveBlocks(chain);
                await levelView.RemoveBlocks(chain, boardController.Blocks);

                levelGoalChecker.HandleAction(new LevelAction(
                    ActionType.CollectPoints,
                    chain.Count,
                    selectedBlockType
                ));

                if (UpdateMovesValue())
                {
                    await CheckNoMoves();
                }
            }

            chain.Clear();
            selectedBlockType = BlockType.None;
            canHandleInput = true;
        }

        private bool UpdateMovesValue()
        {
            movesMade++;
            StaticSignalBus.DispatchSignal(
                SignalType.MovesUpdated,
                new MovesUpdatedData(configuration.Moves, movesMade)
            );

            if (movesMade >= configuration.Moves)
            {
                LevelFailed();
                return false;
            }

            return true;
        }

        private async Task CheckNoMoves()
        {
            var hasMoves = await boardController.CheckBoardForPossibleMoves();
            if (!hasMoves)
            {
                boardController.GenerateBoard(configuration.BoardWidth, configuration.BoardHeight);
                await levelView.DropBoard();
                await levelView.InitializeLevel(
                    configuration.BoardWidth,
                    configuration.BoardHeight,
                    boardController.Blocks);
            }
        }
    }
}