using System;

namespace Qrame.CoreFX.ObjectController
{
    [Serializable]
    public abstract class PlugIn<T> : MarshalByRefObject, IPlugIn
    {
        public IPlugInApplication<T> Application { get; internal set; }
        
        public string Name { get; private set; }
        
        protected PlugIn()
        {
            Application = new PlugInApplication<T>();
            
            var thisPlugInType = GetType();
            var plugInAttribute = PluginHelper.GetAttribute<PlugInAttribute>(thisPlugInType);
            Name = plugInAttribute == null ? thisPlugInType.Name : plugInAttribute.Name;
        }
    }
}
