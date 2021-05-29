using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// XML 문서와 Json 문자열간의 변환 기능을 제공합니다. 
    /// </summary>
    public sealed class XmlToJson
    {
        /// <summary>
        /// XML 문서를 Json 문자열로 변환합니다.
        /// </summary>
        /// <param name="resultSetXML">FOR XML AUTO, ROOT('XXX') 구문을 적용한 SQL XML 결과입니다.</param>
        /// <param name="rootNodeName">최상위 JSON 객체명입니다.</param>
        /// <param name="elementNodeName">XML Row Node명입니다.</param>
        /// <returns>JSON 문자열ㅕ입니다.</returns>
        public static string JSONTransformer(string resultSetXML, string rootNodeName, string elementNodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            DataSet dataset = new DataSet();
            StringBuilder result = new StringBuilder();
            int recordCount = 0;
            int currentIndex = 0;

            XmlTextReader reader = new XmlTextReader(new StringReader(resultSetXML));
            XPathDocument xdoc = new XPathDocument(reader);
            XPathNavigator nav = xdoc.CreateNavigator();
            XPathNodeIterator iter = nav.Select(rootNodeName + "/" + elementNodeName);

            recordCount = iter.Count;
            currentIndex = 0;

            result.Append("{ \"recordcount\": \"" + recordCount.ToString() + "\", \"data\": [ ");

            while (iter.MoveNext())
            {
                XPathNavigator item = iter.Current;
                result.Append("{ ");

                if (item.HasAttributes)
                {
                    item.MoveToFirstAttribute();
                    string line = "";

                    do
                    {
                        string name = item.Name;
                        string value = item.Value;

                        line += "\"" + name + "\": \"" + value.Replace("\"", "\\\"") + "\", ";
                    } while (item.MoveToNextAttribute());

                    line = line.Substring(0, line.Length - 2);
                    result.Append(line);
                }

                result.Append(" }");
                if (currentIndex < recordCount - 1) result.Append(", ");
                currentIndex++;
            }

            result.Append(" ]}");

            reader.Close();
            return result.ToString();
        }
    }
}
