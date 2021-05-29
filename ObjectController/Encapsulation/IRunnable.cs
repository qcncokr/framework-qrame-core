namespace Qrame.CoreFX.ObjectController
{
    public interface IRunnable
    {
        void Start();
        void Stop();
        void WaitToStop();
    }
}