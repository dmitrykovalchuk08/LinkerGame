using System;
using Configuration.DataStructures;

namespace Game.LevelGoals
{
    public class LevelGoal
    {
        public event Action EvConditionPassed;
        public event Action EvConditionFailed;

        public bool IsPassed { get; private set; }
        public LevelCondition Condition { get; private set; }
        public int Quantity { get; private set; }

        public LevelGoal(LevelCondition condition)
        {
            Condition = condition;
        }

        public bool ProcessAction(LevelAction action)
        {
            if (action.Type != Condition.ActionType)
            {
                return false;
            }

            switch (Condition.ActionType)
            {
                case ActionType.CollectPoints:
                {
                    if (Condition.BlockType != BlockType.Any &&
                        action.BlockType != Condition.BlockType)
                    {
                        return false;
                    }

                    Quantity += action.Quantity;
                    if (Condition.Quantity <= Quantity)
                    {
                        IsPassed = true;
                        EvConditionPassed?.Invoke();
                    }

                    return true;
                }
                case ActionType.MovesLimit:
                {
                    Quantity += action.Quantity;
                    if (Condition.Quantity <= Quantity)
                    {
                        EvConditionFailed?.Invoke();
                    }

                    return true;
                }
            }

            return false;
        }
    }
}