using Core.Controller;
using Game.Services;

namespace Game.Infra
{
    public class InitializeController : ControllerBase
    {
        private IBundleProvider _bundleProvider;
        
        public InitializeController(IBundleProvider bundleProvider)
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