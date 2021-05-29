using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Qrame.CoreFX.ObjectController
{
    internal static class PluginHelper
    {
        public static T GetAttribute<T>(MemberInfo memberInfo) where T : class
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), true);
            if (attributes.Length <= 0)
            {
                return null;
            }

            return attributes[0] as T;
        }
        
        public static List<string> FindAssemblyFiles(string plugInFolder)
        {
            var assemblyFilePaths = new List<string>();
            assemblyFilePaths.AddRange(Directory.GetFiles(plugInFolder, "*.exe", SearchOption.AllDirectories));
            assemblyFilePaths.AddRange(Directory.GetFiles(plugInFolder, "*.dll", SearchOption.AllDirectories));
            return assemblyFilePaths;
        }
        
        public static string GetCurrentDirectory()
        {
            try
            {
                return (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.FullName;
            }
            catch (Exception)
            {
                return Directory.GetCurrentDirectory();
            }
        }
    }
}
