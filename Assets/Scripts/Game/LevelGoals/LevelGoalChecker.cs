using System.Collections.Generic;
using System.Linq;
using Configuration.DataStructures;
using Game.LevelGoals.Interfaces;

namespace Game.LevelGoals
{
    public class LevelGoalChecker : ILevelGoalChecker
    {
        private List<LevelGoal> winConditions;
        private List<ILevelGoalListener> listeners;

        public void Initialize(List<LevelCondition> win)
        {
            listeners = new List<ILevelGoalListener>();
            winConditions = new List<LevelGoal>();
            foreach (var levelCondition in win)
            {
                var goal = new LevelGoal(levelCondition);
                winConditions.Add(goal);
                goal.EvConditionPassed += OnConditionPassed;
            }
        }

        public void HandleAction(LevelAction action)
        {
            CheckWinConditions(action);
        }

        public void RegisterListener(ILevelGoalListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(ILevelGoalListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        private void OnConditionPassed()
        {
            if (winConditions.Any(condition => !condition.IsPassed))
            {
                return;
            }

            for (var i = 0; i < listeners.Count; i++)
            {
                listeners[i]?.LevelWon();
            }
        }

        private void CheckWinConditions(LevelAction action)
        {
            foreach (var condition in winConditions)
            {
                if (!condition.ProcessAction(action)) continue;
                for (var i = 0; i < listeners.Count; i++)
                {
                    listeners[i]?.LevelUpdated(condition);
                }
            }
        }
    }
}