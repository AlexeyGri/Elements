using Core.Controller;
using UnityEngine;

namespace Game
{
    public class RootController : ControllerBase
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose");
        }
    }
}