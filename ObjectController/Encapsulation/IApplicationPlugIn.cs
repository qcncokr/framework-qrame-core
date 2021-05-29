namespace Qrame.CoreFX.ObjectController
{
    public interface IApplicationPlugIn<out TPlugIn>
    {
        string Name { get; }
        TPlugIn PlugInProxy { get; }
    }
}
