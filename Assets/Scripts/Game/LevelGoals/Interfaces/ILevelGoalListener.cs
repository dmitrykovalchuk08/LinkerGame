namespace Game.LevelGoals.Interfaces
{
    public interface ILevelGoalListener
    {
        void LevelWon();
        void LevelFailed();
        void LevelUpdated(LevelGoal goal);
    }
}