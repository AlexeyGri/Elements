using System;
using System.Collections.Generic;
using System.Linq;
using Core.Controller.Components;
using Core.Utility;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Services
{
    public class Pool : IPool, IDisposable
    {
        private readonly Dictionary<string, Object> _assets = new();
        private readonly Dictionary<string, List<Object>> _instances = new();
        
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

        public bool TryGetInstance<T>(IControllerResources resources, string key, out T instance) where T : Object
        {
            instance = default;

            if (!_instances.TryGetValue(key, out var instances))
            {
                return false;
            }
            
            var obj = instances.FirstOrDefault();
            if (obj == null)
            {
                return false;
            }
                
            instances.Remove(obj);
                
            resources.Add(new DisposableSource(() =>
            {
                obj.GameObject().SetActive(false);
                instances.Add(obj);
            }));

            instance = obj as T;
            return true;
        }

        public void Dispose()
        {
            _assets.Clear();

            foreach (var instance in _instances.Values.SelectMany(instances => instances))
            {
                Object.Destroy(instance);
            }
            
            _instances.Clear();
        }
    }
}