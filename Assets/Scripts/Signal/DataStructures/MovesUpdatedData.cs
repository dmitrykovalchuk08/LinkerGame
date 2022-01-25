
namespace Signal.DataStructures
{
    public class MovesUpdatedData : SignalData
    {
        public int MovesValue;
        public int MovesLimit;

        public MovesUpdatedData(int movesLimit, int movesValue)
        {
            MovesLimit = movesLimit;
            MovesValue = movesValue;
            Key = SignalType.MovesUpdated;
        }
    }
}