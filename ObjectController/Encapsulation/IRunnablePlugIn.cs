using System;

namespace Qrame.CoreFX.ObjectController
{
    public interface IRunnablePlugIn: IRunnable
    {
        event EventHandler Started;
        event EventHandler Stopped;
    }
}
