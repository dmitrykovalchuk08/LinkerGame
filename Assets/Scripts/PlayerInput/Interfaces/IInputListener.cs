using System.Threading.Tasks;
using Configuration.DataStructures;

namespace PlayerInput.Interfaces
{
    public interface IInputListener
    {
        Task ReceiveInput(BoardCoordinates coords, InputState state);
    }
}