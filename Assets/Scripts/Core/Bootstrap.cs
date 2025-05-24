using System;
using Core.Controller;
using Game;
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
            _root = controllerFactory.CrateController<RootController>();
        }

        private void Start()
        {
            _root.Start();
        }

        private void OnDestroy()
        {
            _root.Stop();
            _root.Dispose();
        }
    }
}