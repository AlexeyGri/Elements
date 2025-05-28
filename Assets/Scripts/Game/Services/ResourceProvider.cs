using System.Threading;
using Core.Controller.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public class ResourceProvider : IBundleProvider
    {
        private readonly IPool _pool;

        public ResourceProvider(IPool pool)
        {
            _pool = pool;
        }

        public T LoadAsset<T>(string path) where T : Object
        {
            return Load<T>(path);
        }

        public async UniTask<T> LoadAssetAsync<T>(string path, CancellationToken token) where T : Object
        {
            var loadOperation = await LoaAsync<T>(path, token).SuppressCancellationThrow();
            if (loadOperation.IsCanceled)
            {
                return default;
            }

            return loadOperation.Result;
        }

        public (bool, T) TryGetInstanceFromPool<T>(IControllerResources resources, string path) where T : Object
        {
            if (_pool.TryGetInstance<T>(resources, path, out var instance))
            {
                return (true, instance.GetComponent<T>());
            }

            if (!_pool.TryGetAsset<T>(path, out var prefab) || prefab == null)
            {
                return (false, default);
            }

            return (true, Instantiate<T>(resources, prefab as GameObject, path));
        }

        private T Load<T>(string path) where T : Object
        {
            if (_pool.TryGetAsset<T>(path, out var asset) && asset != null)
            {
                return asset;
            }

            asset = Resources.Load<T>(path);
            _pool.AddAsset(path, asset);

            return asset;
        }

        private async UniTask<T> LoaAsync<T>(string path, CancellationToken token) where T : Object
        {
            if (_pool.TryGetAsset<T>(path, out var asset) && asset != null)
            {
                return asset;
            }

            asset = await Resources.LoadAsync<T>(path).ToUniTask(cancellationToken: token) as T;
            _pool.AddAsset(path, asset);

            return asset;
        }

        private T Instantiate<T>(IControllerResources resources, GameObject prefab, string key) where T : Object
        {
            var instance = Object.Instantiate(prefab);
            instance.SetActive(false);
            _pool.AddInstance(key, instance);

            _pool.TryGetInstance<T>(resources, key, out instance);
            return instance as T;
        }
    }
}