using System;
using System.Data;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class DataSetExtensions
    {
        public static void SaveSchema(this DataSet @this, string schemaPath)
        {
            @this.WriteXmlSchema(schemaPath);
        }

        public static void SaveFile(this DataSet @this, string filePath)
        {
            @this.WriteXml(filePath);
        }

        public static void LoadSchema(this DataSet @this, string schemaPath)
        {
            @this.ReadXmlSchema(schemaPath);
        }

        public static void LoadFile(this DataSet @this, string filePath)
        {
            @this.ReadXml(filePath);
        }
    }
}
