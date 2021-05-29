using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Xml;
using System.Globalization;

using Qrame.CoreFX.ExtensionMethod;
using Qrame.CoreFX.Cryptography;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// .NET 응용 프로그램 구성 제공자 기본 템플릿 클래스 입니다. 현재 클래스를 이용하거나, 참조하여 새로운 클래스를 개발합니다.
    /// </summary>
    /// <typeparam name="T">응용 프로그램 구성 파일에 지정될 제너릭 타입입니다.</typeparam>
    public class ConfigurationFile<T> : ConfigurationProviderBase<T> where T : ApplicationConfig, new()
    {
        /// <summary>
        /// 응용 프로그램 구성 파일명을 가져오거나 설정합니다.
        /// </summary>
        private string configurationFileName = "";

        /// <summary>
        /// 응용 프로그램 구성 항목의 섹션을 가져오거나 설정합니다.
        /// </summary>
        private string configurationSection = "";

        /// <summary>
        /// 고유한 Xml Node를 생성하기 위한 XmlNamespaceManager
        /// </summary>
        private XmlNamespaceManager xmlNamespaces = null;

        // 기본 Xml Namespace
        private string namespacePrefix = "rs:";

        /// <summary>
        /// 응용 프로그램 구성 파일명을 가져오거나 설정합니다.(공백일 경우 현재 응용 프로그램의 구성파일을 설정합니다.)
        /// </summary>
        public string ConfigurationFileName
        {
            get { return configurationFileName; }
            set { configurationFileName = value; }
        }

        /// <summary>
        /// 응용 프로그램 구성 항목의 섹션을 가져오거나 설정합니다.(공백일 경우 appSettings 구성섹션을 설정합니다.)
        /// </summary>
        public string ConfigurationSection
        {
            get { return configurationSection; }
            set { configurationSection = value; }
        }

        /// <summary>
        /// .NET 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입입니다. 기반의 제너릭 타입입니다.</typeparam>
        /// <returns>ApplicationConfig 타입입니다. 기반의 제너릭 타입입니다.</returns>
        public override T Read<T>()
        {
            T Config = Activator.CreateInstance(typeof(T), true) as T;

            if (!Read(Config))
            {
                return null;
            }

            return Config;
        }

        /// <summary>
        /// .NET 응용 프로그램 설정 파일에서 항목을 읽어와 ApplicationConfig 필드에 매핑합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 타입입니다.</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Read(ApplicationConfig config)
        {
            // 기본 구성 파일 경로가 아닌 사용자 경로의 구성 파일로 매핑
            if (ConfigurationFileName.Length > 0)
            {
                return Read(config, ConfigurationFileName);
            }

            var configration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationFileName = configration.FilePath;
            MemberInfo[] memberInfos = config.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);

            bool missingFields = false;

            string fieldsToEncrypt = "," + PropertiesToEncrypt.ToLower() + ",";

            // 필드와 프로퍼티를 추출하여 동일한 이름의 값을 매핑
            foreach (MemberInfo member in memberInfos)
            {
                string typeName = null;

                FieldInfo field = null;
                PropertyInfo property = null;
                Type fieldType = null;

                if (member.MemberType == MemberTypes.Field)
                {
                    field = (FieldInfo)member;
                    fieldType = field.FieldType;
                    typeName = fieldType.Name.ToLower();
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    property = (PropertyInfo)member;
                    fieldType = property.PropertyType;
                    typeName = fieldType.Name.ToLower();
                }
                else
                {
                    continue;
                }

                string fieldName = member.Name;

                if (fieldName == "ExceptionMessage" || fieldName == "Provider")
                {
                    continue;
                }

                string value = null;
                if (ConfigurationSection.Length == 0)
                {
                    value = ConfigurationManager.AppSettings[fieldName];
                }
                else
                {
                    NameValueCollection nameValues = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(ConfigurationSection);
                    if (nameValues != null)
                    {
                        value = nameValues[fieldName];
                    }
                }

                if (value == null)
                {
                    missingFields = true;
                    continue;
                }

                // 구성 항목중에 암호화되어 있는 필드에 값이 있으면 복호화를 하여 매핑
                if (value.Length > 0 && fieldsToEncrypt.Replace(" ", "").IndexOf("," + fieldName + ",") > -1)
                {
                    Decryptor cryptography = new Decryptor(Encryption.Des);
                    Encoding ascii = new ASCIIEncoding();
                    value = Convert.ToBase64String(cryptography.Decrypt(value.ToBytes(ascii), DecryptionKey.ToBytes(ascii)));
                }

                try
                {
                    // ApplicationConfig 타입의 필드에 구성 항목에서 얻은 데이터를 채운다
                    Reflector.SetPropertyEx(config, fieldName, Reflector.StringToTypedValue(value, fieldType, CultureInfo.InvariantCulture));
                }
                catch
                {
                }
            }

            if (missingFields == true)
            {
                Write(config);
            }

            return true;
        }

        /// <summary>
        /// 사용자 경로의 .NET 응용 프로그램 설정 파일에서 항목을 읽어와 ApplicationConfig 필드에 매핑합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 타입입니다.</param>
        /// <param name="fileName">.NET 응용 프로그램 구성 파일 경로</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        private new bool Read(ApplicationConfig config, string fileName)
        {
            MemberInfo[] memberInfos = config.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);

            bool missingFields = false;

            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(fileName);
            }
            catch
            {
                if (Write(config) == false)
                {
                    return false;
                }

                xmlDocument.Load(fileName);
            }

            GetXmlNamespaceInfo(xmlDocument);

            string configSection = ConfigurationSection;

            if (configSection == "")
            {
                configSection = "appSettings";
            }

            string fieldsToEncrypt = "," + PropertiesToEncrypt.ToLower() + ",";

            foreach (MemberInfo member in memberInfos)
            {
                FieldInfo field = null;
                PropertyInfo property = null;
                Type fieldType = null;
                string typeName = null;

                if (member.MemberType == MemberTypes.Field)
                {
                    field = (FieldInfo)member;
                    fieldType = field.FieldType;
                    typeName = field.FieldType.Name.ToLower();
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    property = (PropertyInfo)member;
                    fieldType = property.PropertyType;
                    typeName = property.PropertyType.Name.ToLower();
                }
                else
                {
                    continue;
                }

                string fieldName = member.Name;

                if (fieldName == "ExceptionMessage" || fieldName == "Provider")
                {
                    continue;
                }

                XmlNode section = xmlDocument.DocumentElement.SelectSingleNode(namespacePrefix + configSection, xmlNamespaces);
                if (section == null)
                {
                    section = CreateConfigSection(xmlDocument, ConfigurationSection);
                    xmlDocument.DocumentElement.AppendChild(section);
                }

                string value = GetNamedValueFromXml(xmlDocument, fieldName, configSection);
                if (value == null)
                {
                    missingFields = true;
                    continue;
                }

                fieldName = fieldName.ToLower();

                // 구성 항목중에 암호화되어 있는 필드에 값이 있으면 복호화를 하여 매핑
                if (value != "" && fieldsToEncrypt.Replace(" ", "").IndexOf("," + fieldName + ",") > -1)
                {
                    Decryptor cryptography = new Decryptor(Encryption.Des);
                    Encoding ascii = new ASCIIEncoding();
                    value = Convert.ToBase64String(cryptography.Decrypt(value.ToBytes(ascii), DecryptionKey.ToBytes(ascii)));
                }

                Reflector.SetPropertyEx(config, fieldName, Reflector.StringToTypedValue(value, fieldType, CultureInfo.InvariantCulture));
            }

            if (missingFields == true)
            {
                Write(config);
            }

            return true;
        }

        /// <summary>
        /// ApplicationConfig 타입입니다. 기준으로 응용 프로그램 항목을 작성합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 타입입니다.</param>
        /// <returns>응용 프로그램 설정 파일에 항목을 정상적으로 작성하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Write(ApplicationConfig config)
        {
            XmlDocument xmlDocument = new XmlDocument();

            if (File.Exists(ConfigurationFileName) == true)
            {
                xmlDocument.Load(ConfigurationFileName);
            }
            else
            {
                string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";
                xmlDocument.LoadXml(Xml);

                GetXmlNamespaceInfo(xmlDocument);
            }

            MemberInfo[] memberInfos = config.GetType().GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public);

            string fieldsToEncrypt = "," + PropertiesToEncrypt.ToLower() + ",";

            foreach (MemberInfo field in memberInfos)
            {
                string value = null;
                object rawValue = null;

                if (field.MemberType == MemberTypes.Field)
                {
                    rawValue = ((FieldInfo)field).GetValue(config);
                }
                else if (field.MemberType == MemberTypes.Property)
                {
                    rawValue = ((PropertyInfo)field).GetValue(config, null);
                }
                else
                {
                    continue;
                }

                if (field.Name == "ExceptionMessage" || field.Name == "Provider")
                {
                    continue;
                }

                value = Reflector.TypedValueToString(rawValue, CultureInfo.InvariantCulture);

                if (fieldsToEncrypt.Replace(" ", "").IndexOf("," + field.Name.ToLower() + ",") > -1)
                {
                    Decryptor cryptography = new Decryptor(Encryption.Des);
                    Encoding ascii = new ASCIIEncoding();
                    value = Convert.ToBase64String(cryptography.Decrypt(value.ToBytes(ascii), DecryptionKey.ToBytes(ascii)));
                }

                string configSection = "appSettings";
                if (ConfigurationSection.Length > 0)
                {
                    configSection = ConfigurationSection;
                }

                XmlNode xmlNode = CreateValue(xmlDocument, configSection, field.Name, value);

                string xml = xmlNode.OuterXml;
            }

            try
            {
                xmlDocument.Save(ConfigurationFileName);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// XmlDocument 타입에서 구성 키와 구성 섹션값으로 값을 조회 합니다.
        /// </summary>
        /// <param name="xmlDocument">XmlDocument 타입</param>
        /// <param name="keyName">구성 키</param>
        /// <param name="configSection">구성 섹션</param>
        /// <returns>string</returns>
        protected string GetNamedValueFromXml(XmlDocument xmlDocument, string keyName, string configSection)
        {
            XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode(namespacePrefix + configSection + @"/" + namespacePrefix + "add[@key='" + keyName + "']", xmlNamespaces);

            if (xmlNode == null)
            {
                xmlNode = xmlDocument.DocumentElement.SelectSingleNode("/" + configSection + "/add[@key='" + keyName + "']", xmlNamespaces);
            }

            if (xmlNode == null)
            {
                return null;
            }

            return xmlNode.Attributes["value"].Value;
        }

        /// <summary>
        /// Xml 문서의 접두어를 설정합니다.
        /// </summary>
        /// <param name="xmlDocument">XmlDocument 타입</param>
        protected void GetXmlNamespaceInfo(XmlDocument xmlDocument)
        {
            if (xmlDocument.DocumentElement.NamespaceURI == null || xmlDocument.DocumentElement.NamespaceURI == "")
            {
                xmlNamespaces = null;
                namespacePrefix = "";
            }
            else
            {
                if (xmlDocument.DocumentElement.Prefix == null || xmlDocument.DocumentElement.Prefix == "")
                {
                    namespacePrefix = "rs";
                }
                else
                {
                    namespacePrefix = xmlDocument.DocumentElement.Prefix;
                }

                xmlNamespaces = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNamespaces.AddNamespace(namespacePrefix, xmlDocument.DocumentElement.NamespaceURI);

                namespacePrefix += ":";
            }
        }

        /// <summary>
        /// 신규 항목 섹션을 기록 합니다.
        /// </summary>
        /// <param name="xmlDocument">XmlDocument 타입</param>
        /// <param name="configSection">구성 섹션</param>
        /// <returns>XML 문서의 단일 노드를 나타냅니다.</returns>
        private XmlNode CreateConfigSection(XmlDocument xmlDocument, string configSection)
        {
            XmlNode appSettingsNode = xmlDocument.CreateNode(XmlNodeType.Element, configSection, xmlDocument.DocumentElement.NamespaceURI);

            XmlNode parentNode = xmlDocument.DocumentElement.AppendChild(appSettingsNode);

            if (configSection != "appSettings")
            {
                XmlNode configSectionHeader = xmlDocument.DocumentElement.SelectSingleNode(namespacePrefix + "configSections", xmlNamespaces);

                if (configSectionHeader == null)
                {
                    XmlNode configSectionNode = xmlDocument.CreateNode(XmlNodeType.Element, "configSections", xmlDocument.DocumentElement.NamespaceURI);

                    configSectionHeader = xmlDocument.DocumentElement.InsertBefore(configSectionNode, xmlDocument.DocumentElement.ChildNodes[0]);
                }

                XmlNode sectionNode = configSectionHeader.SelectSingleNode(namespacePrefix + "section[@name='" + configSection + "']", xmlNamespaces);

                if (sectionNode == null)
                {
                    sectionNode = xmlDocument.CreateNode(XmlNodeType.Element, "section", null);

                    XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
                    xmlAttribute.Value = configSection;
                    XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("type");

                    // 기본 NameValueSectionHandler
                    xmlAttribute2.Value = "System.Configuration.NameValueSectionHandler, System,Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                  
                    XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("requirePermission");
                    xmlAttribute3.Value = "false";

                    sectionNode.Attributes.Append(xmlAttribute);
                    sectionNode.Attributes.Append(xmlAttribute3);
                    sectionNode.Attributes.Append(xmlAttribute2);

                    configSectionHeader.AppendChild(sectionNode);
                }
            }

            return parentNode;
        }

        /// <summary>
        /// 신규 항목을 기록 합니다.
        /// </summary>
        /// <param name="xmlDocument">XmlDocument 타입입니다.</param>
        /// <param name="configSection">구성 섹션입니다.</param>
        /// <param name="key">구성 키입니다.</param>
        /// <param name="value">구성 값입니다.</param>
        /// <returns>XML 문서의 단일 노드를 나타냅니다.</returns>
        private XmlNode CreateValue(XmlDocument xmlDocument, string configSection, string key, string value)
        {
            XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode(namespacePrefix + configSection + "/" + namespacePrefix + "add[@key='" + key + "']", xmlNamespaces);

            if (xmlNode == null)
            {
                xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "add", xmlDocument.DocumentElement.NamespaceURI);

                XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("key");
                xmlAttribute2.Value = key;
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("value");
                xmlAttribute.Value = value;

                xmlNode.Attributes.Append(xmlAttribute2);
                xmlNode.Attributes.Append(xmlAttribute);

                XmlNode parentNode = xmlDocument.DocumentElement.SelectSingleNode(namespacePrefix + configSection, xmlNamespaces);

                if (parentNode == null)
                {
                    parentNode = CreateConfigSection(xmlDocument, configSection);
                }

                parentNode.AppendChild(xmlNode);
            }
            else
            {
                xmlNode.Attributes.GetNamedItem("value").Value = value;
            }

            return xmlNode;
        }
    }
}
