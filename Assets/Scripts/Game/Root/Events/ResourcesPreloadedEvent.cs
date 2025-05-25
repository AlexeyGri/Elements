using Game.EventBus;

namespace Game.Infra.Events
{
    public class ResourcesPreloadedEvent : IEvent
    {
        public readonly bool IsPreloaded;
        
        public ResourcesPreloadedEvent(bool isPreloaded)
        {
            IsPreloaded = isPreloaded;
        }
    }
}