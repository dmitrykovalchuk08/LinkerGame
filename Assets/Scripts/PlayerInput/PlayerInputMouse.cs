using UnityEngine;

namespace PlayerInput
{
    public class PlayerInputMouse : PlayerInputBase
    {
        public void Update()
        {
            var worldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var coords = CoordinatesProvider.WorldCoordinatesToBoard(worldCoords);

            // if input ended - send it anyway. 
            if (Input.GetMouseButtonUp(0))
            {
                SendInput(coords, InputState.Ended);
                return;
            }

            if (coords == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                SendInput(coords, InputState.Started);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                SendInput(coords, InputState.InProgress);
                return;
            }
        }
    }
}