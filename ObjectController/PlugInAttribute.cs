using System;

namespace Qrame.CoreFX.ObjectController
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlugInAttribute : Attribute
    {
        public string Name { get; set; }
        
        public PlugInAttribute(string name)
        {
            Name = name;
        }
    }
}
