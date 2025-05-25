using Core.Controller.Components;
using UnityEngine;

namespace Game.Services
{
    public interface IPool
    {
        void AddAsset<T>(string path, T asset) where T : Object;
        bool TryGetAsset<T>(string path, out T asset) where T : Object;

        void AddInstance<T>(string key, T instance) where T : Object;
        bool TryGetInstance<T>(IControllerResources resources, string key, out GameObject instance) where T : Object;
    }
}