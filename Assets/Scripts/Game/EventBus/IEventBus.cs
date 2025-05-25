using System;

namespace Game.EventBus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> callback) where T : IEvent;
        void Unsubscribe<T>(Action<T> callback) where T : IEvent;
        void Invoke<T>(T busEvent) where T : IEvent;
    }
}