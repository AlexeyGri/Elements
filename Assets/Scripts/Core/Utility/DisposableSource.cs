using System;

namespace Core.Utility
{
    public class DisposableSource : IDisposable
    {
        private readonly Action _dispose;
        
        public DisposableSource(Action dispose)
        {
            _dispose = dispose;
        }
        
        public void Dispose()
        {
            _dispose.Invoke();
        }
    }
}