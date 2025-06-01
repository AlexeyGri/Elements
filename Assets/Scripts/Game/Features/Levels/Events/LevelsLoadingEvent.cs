using Game.EventBus;
using Game.Features.Levels.Models;

namespace Game.Features.Levels.Events
{
    public class LevelsLoadingEvent : IEvent
    {
        public readonly LevelsLoadingResult Result;

        public LevelsLoadingEvent(LevelsLoadingResult result)
        {
            Result = result;
        }
    }
}