using System.Collections.Generic;
using Signal.DataStructures;
using Signal.Interfaces;

namespace Signal.Implementation
{
    public static class StaticSignalBus
    {
        private static readonly Dictionary<SignalType, List<ISignalListener>> listeners =
            new Dictionary<SignalType, List<ISignalListener>>();

        /// <summary>
        /// subscribe given instance of ISignalListener to
        /// given event type 
        /// </summary>
        public static void Subscribe(SignalType key, ISignalListener listener)
        {
            if (!listeners.ContainsKey(key))
            {
                listeners.Add(key, new List<ISignalListener>()
                {
                    listener
                });
            }
            else
            {
                listeners[key].Add(listener);
            }
        }

        /// <summary>
        /// Unsubscribe given instance of ISignalListener from
        /// given event type 
        /// </summary>
        public static void Unsubscribe(SignalType key, ISignalListener listener)
        {
            if (listeners.ContainsKey(key))
            {
                listeners[key].Remove(listener);
            }
        }

        /// <summary>
        /// Unsubscribe given instance of ISignalListener from
        /// all events
        /// </summary>
        public static void UnSubscribeAll(ISignalListener listener)
        {
            foreach (var t in listeners.Values)
            {
                t.Remove(listener);
            }
        }

        /// <summary>
        /// dispatches signal of given type to listeners
        ///  with given data.
        /// </summary>
        public static void DispatchSignal(SignalType key, SignalData data)
        {
            if (listeners.ContainsKey(key))
            {
                foreach (var listener in listeners[key])
                {
                    listener?.HandleSignal(data);
                }
            }
        }
    }
}