using Core.Controller;
using Core.Controller.Components;
using Core.Utility;
using UnityEngine;

namespace Core.Extensions
{
    public static class ControllerExtension
    {
        public static TPrefab Instantiate<TPrefab>(this ControllerBase _, IControllerResources resources, TPrefab prefab)
        where TPrefab : Object
        {
            var instance = Object.Instantiate(prefab);
            resources.Add(new DisposableSource(() => Object.Destroy(instance)));

            return instance;
        }
    }
}