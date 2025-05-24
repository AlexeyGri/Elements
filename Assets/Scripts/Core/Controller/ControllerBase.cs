using System.Collections.Generic;

namespace Core.Controller
{
    public abstract class ControllerBase
    {
        private readonly List<ControllerBase> _children = new();
        private ControllerStates _state = ControllerStates.Created;

        public ControllerBase Parent { get; private set; }
        public ControllerResources Resources { get; private set; } = new();

        public abstract void OnStart();
        public abstract void OnStop();
        public abstract void OnDispose();

        private void Start()
        {
            if (_state is ControllerStates.Running or ControllerStates.Disposed)
            {
                return;
            }
            
            OnStart();
            _state = ControllerStates.Running;
        }

        private void Stop()
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
        }

        private void Dispose()
        {
            OnDispose();
            
            foreach (var child in _children)
            {
                child.Dispose();
            }

            _children.Clear();
            
            Resources.Clear();

            _state = ControllerStates.Disposed;
        }

        public void AddController(ControllerBase controller)
        {
            _children.Add(controller);
            controller.Parent = this;
            
            controller.Start();
        }

        public void RemoveController(ControllerBase controller, bool withDispose = true)
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
    
    internal enum ControllerStates
    {
        Created,
        Running,
        Stopped,
        Disposed,
    }
}
