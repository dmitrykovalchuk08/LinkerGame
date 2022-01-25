using Game.LevelGoals;

namespace Signal.DataStructures
{
    public class LevelGoalUpdatedData : SignalData
    {
        public LevelGoal LevelGoal;

        public LevelGoalUpdatedData(LevelGoal goal)
        {
            Key = SignalType.LevelGoalUpdated;
            LevelGoal = goal;
        }
    }
}