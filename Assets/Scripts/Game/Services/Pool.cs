using System.Collections.Generic;
using System.Linq;
using Core.Controller.Components;
using Core.Utility;
using UnityEngine;

namespace Game.Services
{
    public class Pool : IPool
    {
        private readonly Dictionary<string, Object> _assets;
        private readonly Dictionary<string, List<Object>> _instances;
        
        public void AddAsset<T>(string path, T asset) where T : Object
        {
            _assets.TryAdd(path, asset);
        }

        public bool TryGetAsset<T>(string path, out T asset) where T : Object
        {
            asset = default;

            if (_assets.TryGetValue(path, out var result))
            {
                asset = result as T;
                
                return true;
            }
            
            return false;
        }

        public void AddInstance<T>(string key, T instance) where T : Object
        {
            if (_instances.TryGetValue(key, out var instances))
            {
                instances.Add(instance);
            }
            else
            {
                _instances.Add(key, new List<Object> {instance});
            }
        }

        public bool TryGetInstance<T>(IControllerResources resources, string key, out GameObject instance) where T : Object
        {
            instance = default;

            if (!_instances.TryGetValue(key, out var instances))
            {
                return false;
            }
            
            var obj = instances.FirstOrDefault() as GameObject;
            if (obj == null)
            {
                return false;
            }
                
            instances.Remove(obj);
                
            resources.Add(new DisposableSource(() =>
            {
                obj.SetActive(false);
                instances.Add(obj);
            }));

            instance = obj;
            return true;
        }
    }
}