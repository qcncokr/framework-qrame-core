using System;

namespace Qrame.CoreFX.ObjectController
{
    public class ApplicationPlugIn<T> : IApplicationPlugIn<T>
    {
        public string Name { get; private set; }
        
        public T PlugInProxy { get; private set; }

        public ApplicationPlugIn(IPlugInBasedApplication plugInApplication, Type plugInType)
        {
            PlugInProxy = (T)Activator.CreateInstance(plugInType);

            var plugInObjectType = PlugInProxy.GetType();

            var applicationProperty = plugInObjectType.GetProperty("Application");
            var applicationPropertyValue = applicationProperty.GetValue(PlugInProxy, null);
            var applicationPropertyType = applicationPropertyValue.GetType();

            applicationPropertyType.GetProperty("Name").SetValue(applicationPropertyValue, plugInApplication.Name, null);
            applicationPropertyType.GetProperty("ApplicationProxy").SetValue(applicationPropertyValue, plugInApplication, null);

            Name = ((IPlugIn) PlugInProxy).Name;
        }
    }
}
