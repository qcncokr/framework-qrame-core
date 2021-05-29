using System;

namespace Qrame.CoreFX.ObjectController
{
    public abstract class RunnablePlugIn<TApp> : PlugIn<TApp>, IRunnablePlugIn
    {
        public event EventHandler Started;
        
        public event EventHandler Stopped;
        
        public void Start()
        {
            OnStart();
        }
        
        public void Stop()
        {
            OnStop();
        }
        
        public void WaitToStop()
        {
            OnWaitToStop();
        }
        
        protected virtual void OnStart()
        {
            if (Started != null)
            {
                Started(this, new EventArgs());
            }
        }
        
        protected virtual void OnStop()
        {
            if (Started != null)
            {
                Stopped(this, new EventArgs());
            }
        }
        
        protected virtual void OnWaitToStop()
        {
        }
    }
}
