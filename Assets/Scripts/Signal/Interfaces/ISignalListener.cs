using Signal.DataStructures;

namespace Signal.Interfaces
{
    public interface ISignalListener
    {
        void HandleSignal(SignalData data);
    }

    
}