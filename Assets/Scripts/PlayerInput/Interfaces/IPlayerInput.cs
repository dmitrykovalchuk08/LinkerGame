using CoordinatesConvertor;

namespace PlayerInput.Interfaces
{
    public interface IPlayerInput
    {
        void SetUp(ICoordinatesProvider coordinatesProvider);
        void RegisterHandler(IInputListener receiver);
        void UnregisterHandler(IInputListener receiver);
    }
}