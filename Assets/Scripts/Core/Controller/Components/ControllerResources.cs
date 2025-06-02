using System;
using System.Collections.Generic;
using Core.Utility;
using UnityEngine;

namespace Core.Controller.Components
{
    public class ControllerResources : IControllerResources
    {
        private readonly List<DisposableSource> _resources = new();

        public void Add(DisposableSource source)
        {
            _resources.Add(source);
        }

        internal void Clear()
        {
            foreach (var resource in _resources)
            {
                try
                {
                    resource.Dispose();
                }
                catch (MissingReferenceException _)
                {
                    // ignore)
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            
            _resources.Clear();
        }
    }
}