using Configuration.DataStructures;

namespace Game.LevelGoals
{
    public struct LevelAction
    {
        public readonly ActionType Type;
        public readonly int Quantity;
        public readonly BlockType BlockType;

        public LevelAction(ActionType type, int quantity, BlockType blockType)
        {
            Type = type;
            Quantity = quantity;
            BlockType = blockType;
        }
    }
}