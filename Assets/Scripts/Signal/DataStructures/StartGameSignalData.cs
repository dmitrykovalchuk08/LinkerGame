
namespace Signal.DataStructures
{
    public class StartGameSignalData : SignalData
    {
        public int SelectedLevel;

        public StartGameSignalData(int selectedLevel)
        {
            SelectedLevel = selectedLevel;
            Key = SignalType.StartGame;
        }
    }
}