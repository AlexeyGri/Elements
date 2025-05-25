using Core.Controller;
using Game.Services;

namespace Game.Features.UI
{
    public class InterfaceController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;

        public InterfaceController(IBundleProvider bundleProvider)
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