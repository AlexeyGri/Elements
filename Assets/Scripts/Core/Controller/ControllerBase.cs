using System.Collections.Generic;
using System.Threading;
using Core.Controller.Components;

namespace Core.Controller
{
    public abstract class ControllerBase
    {
        private readonly List<ControllerBase> _children = new();
        private readonly ControllerResources _resources = new();

        private ControllerStates _state = ControllerStates.Created;

        protected CancellationTokenSource _cancellationTokenSource;

        protected ControllerBase Parent { get; private set; }
        protected CancellationToken Token => _cancellationTokenSource.Token;

        protected IControllerResources ControllerResources => _resources;

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract void OnDispose();

        public void Start()
        {
            if (_state is ControllerStates.Running or ControllerStates.Disposed)
            {
                return;
            }
            
            OnStart();
            _state = ControllerStates.Running;
        }

        public void Stop()
        {
            if (_state != ControllerStates.Running)
            {
                return;
            }
            
            foreach (var child in _children)
            {
                child.Stop();
            }

            OnStop();
            _state = ControllerStates.Stopped;
        }

        public void Dispose()
        {
            if (_state == ControllerStates.Disposed)
            {
                return;
            }
            
            _cancellationTokenSource.Cancel();
            
            foreach (var child in _children)
            {
                child.Dispose();
            }
            
            OnDispose();

            _children.Clear();
            _resources.Clear();
            
            _state = ControllerStates.Disposed;
        }

        protected void AddController(ControllerBase controller)
        {
            _children.Add(controller);
            controller.Parent = this;
            controller._cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Token);
            
            controller.Start();
        }

        protected void RemoveController(ControllerBase controller, bool withDispose = true)
        {
            if (controller == null)
            {
                return;
            }

            if (controller._state == ControllerStates.Running)
            {
                controller.Stop();
            }

            if (withDispose && controller._state is ControllerStates.Stopped or ControllerStates.Created)
            {
                controller.Dispose();
            }

            _children.Remove(controller);
            controller.Parent = null;
        }
    }

    public abstract class RootController : ControllerBase
    {
        public void SetCancellationToken(CancellationToken token)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        }
    }
    
    internal enum ControllerStates
    {
        Created,
        Running,
        Stopped,
        Disposed,
    }
}
