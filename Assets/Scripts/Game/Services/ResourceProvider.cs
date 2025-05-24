using System.Threading;
using Core.Controller.Components;
using Core.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public class ResourceProvider : IBundleProvider
    {
        public T Load<T>(IControllerResources resources, string path)
        where T : Object
        {
            var asset = Resources.Load<T>(path);
            
            resources.Add(new DisposableSource(() => Object.Destroy(asset)));

            return asset;
        }

        public async UniTask<T> LoadAsync<T>(IControllerResources resources, string path, CancellationToken token) where T : Object
        {
            var asset = await Resources.LoadAsync<T>(path).ToUniTask(cancellationToken: token);
            if (token.IsCancellationRequested)
            {
                return default;
            }
            
            resources.Add(new DisposableSource(() => Object.Destroy(asset)));

            return (T)asset;
        }
    }
}