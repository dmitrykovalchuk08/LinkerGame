using System;
using System.Collections.Generic;
using Configuration.DataStructures;
using CoordinatesConvertor;
using PlayerInput.Interfaces;
using UnityEngine;

namespace PlayerInput
{
    public abstract class PlayerInputBase : MonoBehaviour, IPlayerInput, IDisposable
    {
        protected ICoordinatesProvider CoordinatesProvider;
        private List<IInputListener> receivers;
        private BoardCoordinates lastSentCoordinates;

        public void SetUp(ICoordinatesProvider coordinatesProvider)
        {
            CoordinatesProvider = coordinatesProvider;
            receivers = new List<IInputListener>();
        }

        public void RegisterHandler(IInputListener receiver)
        {
            receivers.Add(receiver);
        }

        public void UnregisterHandler(IInputListener receiver)
        {
            receivers.Remove(receiver);
        }

        protected void SendInput(BoardCoordinates coords, InputState state)
        {
            switch (state)
            {
                case InputState.Started:
                    Send(coords, state);
                    break;
                case InputState.InProgress:
                    if (coords != lastSentCoordinates)
                    {
                        Send(coords, state);
                    }

                    break;
                case InputState.Ended:
                    Send(coords, state);
                    break;
            }
        }

        private void Send(BoardCoordinates coords, InputState state)
        {
            lastSentCoordinates = coords;
            for (var i = 0; i < receivers.Count; i++)
            {
                var receiver = receivers[i];
                try
                {
                    receiver?.ReceiveInput(coords, state);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error when sending input data to {receiver?.GetType()} \n {e}");
                }
            }
        }

        public void Dispose()
        {
            receivers?.Clear();
        }
    }
}