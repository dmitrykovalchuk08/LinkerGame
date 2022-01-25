using System.Collections.Generic;
using Configuration.DataStructures;
using Game.LevelGoals.Interfaces;

namespace Game.LevelGoals
{
    public interface ILevelGoalChecker
    {
        void Initialize(List<LevelCondition> win);
        void HandleAction(LevelAction action);

        void RegisterListener(ILevelGoalListener listener);
        void UnregisterListener(ILevelGoalListener listener);
    }
}