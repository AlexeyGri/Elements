using System.Threading;
using Core.Controller.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public interface IBundleProvider
    {
        T Load<T>(IControllerResources resources, string path) where T : Object;
        UniTask<T> LoadAsync<T>(IControllerResources resources, string path, CancellationToken token) where T : Object;
    }
}
