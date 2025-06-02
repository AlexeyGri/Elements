using Core.Controller;
using Core.Controller.Components;
using Core.Utility;
using UnityEngine;

namespace Core.Extensions
{
    public static class ControllerExtension
    {
        public static TPrefab Instantiate<TPrefab>(this ControllerBase _, IControllerResources resources, TPrefab prefab)
        where TPrefab : MonoBehaviour
        {
            var instance = Object.Instantiate(prefab);
            
            resources.Add(new DisposableSource(() => Destroy(instance)));

            return instance;

            void Destroy(TPrefab instance)
            {
                if (instance.gameObject == null)
                {
                    return;
                }

                Object.Destroy(instance.gameObject);
            }
        }
    }
}