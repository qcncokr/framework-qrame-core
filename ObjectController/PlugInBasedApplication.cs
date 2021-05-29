using Qrame.CoreFX.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Qrame.CoreFX.ObjectController
{
    public class PlugInBasedApplication<T> : IPlugInBasedApplication
    {
        public string Name { get; private set; }

        public Dictionary<ModulePlugin, IApplicationPlugIn<T>> PlugIns { get; private set; }

        public string PlugInFolder { get; set; }

        public List<string> ExceptionPlugIns { get; private set; }

        public PlugInBasedApplication()
        {
            Initialize();
            PlugInFolder = PluginHelper.GetCurrentDirectory();
        }

        public PlugInBasedApplication(string plugInFolder)
        {
            Initialize();
            PlugInFolder = plugInFolder;
        }

        public void RefreshPlugIn()
        {
            ExceptionPlugIns = new List<string>();
            PlugIns = new Dictionary<ModulePlugin, IApplicationPlugIn<T>>();

            List<string> typeList = null;
            if (string.IsNullOrEmpty(PlugInFolder) || !Directory.Exists(PlugInFolder))
            {
                try
                {
                    Directory.CreateDirectory(PlugInFolder);
                }
                catch (Exception exception)
                {
                    throw new ApplicationException(string.Format("PlugInFolder must be a valid folder path - {0}", exception.ToString()));
                }
            }

            ExceptionPlugIns = new List<string>();
            var assemblyFiles = PluginHelper.FindAssemblyFiles(PlugInFolder);
            var plugInType = typeof(T);
            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    typeList = new List<string>();
                    FileInfo fileInfo = new FileInfo(assemblyFile);
                    string pluginFileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    if (fileVersionInfo.LegalTrademarks.IndexOf("PLUGINMODLUE") > -1)
                    {
                        var allTypes = assembly.GetTypes();
                        foreach (var type in allTypes)
                        {
                            if (plugInType.IsAssignableFrom(type) && type.IsClass && type.IsAbstract == false)
                            {
                                try
                                {
                                    string contract = assembly.GetStringEmbeddedResource(string.Format("{0}.Contract.json", pluginFileName));
                                    string pluginName = string.Concat(pluginFileName, "_", type.Name);

                                    ModulePlugin modulePlugin = new ModulePlugin()
                                    {
                                        PluginFileName = pluginFileName,
                                        PluginName = pluginName,
                                        LoadAssembly = assembly,
                                        AssemblyName = assembly.Location,
                                        TypeNames = typeList,
                                        Contract = contract
                                    };


                                    if (PlugIns.ContainsKey(modulePlugin) == false)
                                    {
                                        typeList.Add(type.FullName);
                                        PlugIns.Add(modulePlugin, new ApplicationPlugIn<T>(this, type));
                                    }
                                    else
                                    {
                                        ExceptionPlugIns.Add(string.Format("고유한 Plugin 키 필요 - {0}", pluginName));
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ExceptionPlugIns.Add(string.Format("{0}.{1} - {2}", fileInfo.Name, type.Name, exception.ToString()));
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionPlugIns.Add(string.Format("{0} - {1}", assemblyFile, exception.ToString()));
                }
            }
        }

        private void Initialize()
        {
            var plugInApplicationAttribute = PluginHelper.GetAttribute<PlugInApplicationAttribute>(GetType());
            Name = plugInApplicationAttribute == null ? GetType().Name : plugInApplicationAttribute.Name;
            PlugIns = new Dictionary<ModulePlugin, IApplicationPlugIn<T>>();
        }
    }

    public class ModulePlugin
    {
        public string PluginFileName { get; set; }
        public string PluginName { get; set; }
        public Assembly LoadAssembly { get; set; }
        public string AssemblyName { get; set; }
        public List<string> TypeNames { get; set; }
        public string Contract { get; set; }
    }
}
