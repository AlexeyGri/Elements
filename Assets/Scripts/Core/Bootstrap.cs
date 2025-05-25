using Core.Controller;
using Game.Infra;
using UnityEngine;
using Zenject;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        private RootController _root;
        
        [Inject]
        public void Construct(IControllerFactory controllerFactory)
        {
            _root = controllerFactory.CrateRoot<MyRootController>(Application.exitCancellationToken);
        }

        private void Start()
        {
            _root.Start();
        }

        private void OnDisable()
        {
            _root.Stop();
        }

        private void OnDestroy()
        {
            _root.Dispose();
        }
    }
}