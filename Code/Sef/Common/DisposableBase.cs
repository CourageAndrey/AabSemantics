using System;

namespace Sef.Common
{
    public abstract class DisposableBase : IDisposable
    {
        private bool disposed;

        public void Dispose()
        {
            clear();
            GC.SuppressFinalize(this);
        }

        ~DisposableBase()
        {
            clear();
        }

        private void clear()
        {
            if (!disposed)
            {
                Free();
                disposed = true;
            }
        }

        protected abstract void Free();
    }
}
