using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX
{
    /// <summary>
    /// IDisposable 인터페이스를 구현하는 Dispose 패턴의 추상 클래스입니다.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool _disposed;
        private readonly static object disposalLock = new object();

        /// <summary>
        /// Dispose 여부를 조회합니다.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Dispose를 수행합니다.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 기본 소멸자입니다.
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose를 수행합니다.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            lock (disposalLock)
            {
                if (IsDisposed || !disposing)
                {
                    return;
                }

                DisposeResources();

                _disposed = true;
            }
        }

        /// <summary>
        /// Managed 리소스를 반환합니다.
        /// </summary>
        protected abstract void DisposeResources();

        /// <summary>
        /// Unmanaged 리소스를 반환합니다.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}
