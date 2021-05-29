using System;

namespace Qrame.CoreFX.ObjectController
{
    internal class PlugInApplication<T> : IPlugInApplication<T>
    {
        public T ApplicationProxy { get; set; }
        
        public string Name { get; set; }
    }
}
