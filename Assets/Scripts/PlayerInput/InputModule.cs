using CoordinatesConvertor;
using PlayerInput.Interfaces;
using UnityEngine;

namespace PlayerInput
{
    public class InputModule
    {
        private static GameObject inputGO;
        private static IPlayerInput input;

        public static void GenerateInput(ICoordinatesProvider coordinatesProvider)
        {
            inputGO = new GameObject();

#if !UNITY_EDITOR && ( UNITY_ANDROID || UNITY_IPHONE)
            input = inputGO.AddComponent<PlayerInputTouch>();
#else
            input = inputGO.AddComponent<PlayerInputMouse>();
#endif
            input.SetUp(coordinatesProvider);
        }

        public static IPlayerInput GetInput()
        {
            return input;
        }
    }
}