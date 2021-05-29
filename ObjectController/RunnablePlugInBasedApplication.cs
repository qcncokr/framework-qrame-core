using System;

namespace Qrame.CoreFX.ObjectController
{
    public abstract class RunnablePlugInBasedApplication<TPlugIn> : PlugInBasedApplication<TPlugIn> where TPlugIn : IRunnablePlugIn
    {
        public event EventHandler Starting;
        
        public event EventHandler Started;
        
        public event EventHandler Stopping;
        
        public event EventHandler Stopped;
        
        public void Start()
        {
            OnStarting();
            
            foreach (var plugIn in PlugIns)
            {
                plugIn.Value.PlugInProxy.Start();
            }

            OnStarted();
        }
        
        public void Stop()
        {
            OnStopping();

            foreach (var plugIn in PlugIns)
            {
                plugIn.Value.PlugInProxy.Stop();
            }

            OnStopped();
        }
        
        public void WaitToStop()
        {
            foreach (var plugIn in PlugIns)
            {
                plugIn.Value.PlugInProxy.WaitToStop();
            }

            OnWaitToStop();
        }
        
        protected virtual void OnStarting()
        {
            if (Starting != null)
            {
                Starting(this, new EventArgs());
            }
        }
        
        protected virtual void OnStarted()
        {
            if (Started != null)
            {
                Started(this, new EventArgs());
            }
        }
        
        protected virtual void OnStopping()
        {
            if (Stopping != null)
            {
                Stopping(this, new EventArgs());
            }
        }
        
        protected virtual void OnStopped()
        {
            if (Stopped != null)
            {
                Stopped(this, new EventArgs());
            }
        }
        
        protected virtual void OnWaitToStop()
        {
        }
    }
}
