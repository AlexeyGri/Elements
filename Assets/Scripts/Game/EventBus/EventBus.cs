using System;
using System.Collections.Generic;

namespace Game.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, List<object>> _eventCallbacks = new();

        public void Subscribe<T>(Action<T> callback) where T : IEvent
        {
            var key = GetKey<T>();
            if (_eventCallbacks.TryGetValue(key, out var eventCallbacks))
            {
                eventCallbacks.Add(callback);
            }
            else
            {
                _eventCallbacks.Add(key, new List<object> { callback });
            }
        }

        public void Unsubscribe<T>(Action<T> callback) where T : IEvent
        {
            if (_eventCallbacks.TryGetValue(GetKey<T>(), out var eventCallbacks))
            {
                eventCallbacks.Remove(callback);
            }
        }

        public void Invoke<T>(T busEvent) where T : IEvent
        {
            if (_eventCallbacks.TryGetValue(GetKey<T>(), out var eventCallbacks))
            {
                for (var i = 0; i < eventCallbacks.Count; i++)
                {
                    var callback = eventCallbacks[i] as Action<T>;
                    callback?.Invoke(busEvent);
                }
            }
        }

        private static string GetKey<T>()
        {
            return typeof(T).Name;
        }
    }
}