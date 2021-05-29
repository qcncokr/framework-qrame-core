namespace Qrame.CoreFX.ObjectController
{
    public interface IPlugInApplication<out T>
    {
        T ApplicationProxy { get; }
        string Name { get; }
    }
}
