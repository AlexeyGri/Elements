using System.Threading;
using Core.Controller.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public interface IBundleProvider
    {
        T LoadAsset<T>(string path) where T : Object;
        UniTask<T> LoadAssetAsync<T>(string path, CancellationToken token) where T : Object;
        T GetInstanceFromPool<T>(IControllerResources resources, string path) where T : Object;
        UniTask<T> GetInstanceFromPoolAsync<T>(IControllerResources resources, string path, CancellationToken token) where T : Object;
    }
}
