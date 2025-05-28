using Game.EventBus;
using Game.Features.Levels.Models;

namespace Game.Features.Levels.Events
{
    public class LevelFinishedEvent : IEvent
    {
        public readonly LevelResults Result;

        public LevelFinishedEvent(LevelResults result)
        {
            Result = result;
        }
    }
}