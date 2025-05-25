using Core.Controller;
using Game.Services;

namespace Game.Featuries.Background
{
    public class BackgroundController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;
        
        public BackgroundController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}