using System;

namespace Qrame.CoreFX.ObjectController
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlugInApplicationAttribute : Attribute
    {
        public string Name { get; set; }
        
        public PlugInApplicationAttribute(string name)
        {
            Name = name;
        }
    }
}
