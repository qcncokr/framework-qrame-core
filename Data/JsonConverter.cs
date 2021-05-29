using System;
using System.Data;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;

using Newtonsoft.Json;

using Qrame.CoreFX.Exceptions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// 닷넷 타입과 Json 문자열간의 변환 기능을 제공합니다. 
    /// </summary>
    public sealed class JsonConverter
    {
        /// <summary>
        /// 닷넷 타입을 Json 문자열로 변환합니다.
        /// </summary>
        /// <param name="serializeObject">Json 문자열로 Serialize할 Object입니다.</param>
        /// <returns>Json 문자열 결과를 반환합니다.</returns>
        public static string Serialize(object serializeObject)
        {
            return Serialize(serializeObject, true);
        }

        /// <summary>
        /// 닷넷 타입을 Json 문자열로 변환합니다.
        /// </summary>
        /// <param name="serializeObject">Json 문자열로 Serialize할 Object입니다.</param>
        /// <param name="isStringNullValueEmpty">DataSet, DataTable내의 String 타입의 값이 Null일 경우 빈 값으로 변환할지 여부입니다.</param>
        /// <returns>Json 문자열 결과를 반환합니다.</returns>
        public static string Serialize(object serializeObject, bool isStringNullValueEmpty)
        {
            string jsonString = "{}";
            try
            {
                JsonSerializer jsonSerializer = JsonSerializer.Create(null);

                if (isStringNullValueEmpty == true)
                {
                    jsonSerializer.Converters.Add(new DataTableConverter());
                    jsonSerializer.Converters.Add(new DataSetConverter());
                }
                jsonSerializer.Converters.Add(new IsoDateTimeConverter());

                jsonSerializer.NullValueHandling = NullValueHandling.Include;
                jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                StringBuilder stringBuilder = new StringBuilder(1024);
                StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Newtonsoft.Json.Formatting.None;
                    jsonSerializer.Serialize(jsonTextWriter, serializeObject);
                }
                jsonString = stringWriter.ToString();
            }
            catch (Exception e)
            {
                ExceptionFactory.Register("DataException", new ErrorException());
                ExceptionFactory.Handle("Json Serialize 중에 에러가 발생 했습니다.", e);
            }

            return jsonString;
        }

        /// <summary>
        /// 닷넷 타입을 Json 문자열로 변환합니다.
        /// </summary>
        /// <param name="serializeObject">Json 문자열로 Serialize할 Object입니다.</param>
        /// <returns>Json 문자열 결과를 반환합니다.</returns>
        public static string Serialize<T>(T serializeObject)
        {
            return Serialize(serializeObject, true);
        }

        /// <summary>
        /// 닷넷 타입을 Json 문자열로 변환합니다.
        /// </summary>
        /// <param name="serializeObject">Json 문자열로 Serialize할 Object입니다.</param>
        /// <param name="isStringNullValueEmpty">DataSet, DataTable내의 String 타입의 값이 Null일 경우 빈 값으로 변환할지 여부입니다.</param>
        /// <returns>Json 문자열 결과를 반환합니다.</returns>
        public static string Serialize<T>(T serializeObject, bool isStringNullValueEmpty)
        {
            string jsonString = "{}";
            try
            {
                JsonSerializer jsonSerializer = JsonSerializer.Create(null);

                if (isStringNullValueEmpty == true)
                {
                    jsonSerializer.Converters.Add(new DataTableConverter());
                    jsonSerializer.Converters.Add(new DataSetConverter());
                }
                jsonSerializer.Converters.Add(new IsoDateTimeConverter());

                jsonSerializer.NullValueHandling = NullValueHandling.Include;
                jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                StringBuilder stringBuilder = new StringBuilder(1024);
                StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Newtonsoft.Json.Formatting.None;
                    jsonSerializer.Serialize(jsonTextWriter, serializeObject);
                }
                jsonString = stringWriter.ToString();
            }
            catch (Exception e)
            {
                ExceptionFactory.Register("DataException", new ErrorException());
                ExceptionFactory.Handle("Json Serialize 중에 에러가 발생 했습니다.", e);
            }

            return jsonString;
        }

        /// <summary>
        /// Json 문자열을 닷넷 타입으로 변환합니다.
        /// </summary>
        /// <param name="jsonString">Json 문자열 결과를 반환할 out 변수입니다.</param>
        /// <returns>런타임에 포함될 작업을 포함합니다.</returns>
        public static T Deserialize<T>(string jsonString) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString) as T;
            }
            catch (Exception e)
            {
                ExceptionFactory.Register("DataException", new ErrorException());
                ExceptionFactory.Handle("Json Deserialize 중에 에러가 발생 했습니다. Json 문자열과 AnonymousType의 타입을 검사해야합니다.", e);
            }

            return default(T);
        }

        /// <summary>
        /// Json 문자열을 닷넷 타입으로 변환합니다.
        /// </summary>
        /// <param name="jsonString">Json 문자열 결과를 반환할 out 변수입니다.</param>
        /// <param name="anonymousType">Json 문자열로 Serialize할 AnonymousType입니다.</param>
        /// <returns>런타임에 포함될 작업을 포함합니다.</returns>
        public static T Deserialize<T>(string jsonString, T anonymousType)
        {
            try
            {
                return JsonConvert.DeserializeAnonymousType(jsonString, anonymousType);
            }
            catch (Exception e)
            {
                ExceptionFactory.Register("DataException", new ErrorException());
                ExceptionFactory.Handle("Json Deserialize 중에 에러가 발생 했습니다. Json 문자열과 AnonymousType의 타입을 검사해야합니다.", e);
            }

            return default(T);
        }

        /// <summary>
        /// Json 문자열을 XmlDocument 타입으로 변환합니다.
        /// </summary>
        /// <param name="jsonString">Json 문자열 결과를 반환할 out 변수입니다.</param>
        /// <returns>XML 문서를 나타냅니다.</returns>
        public static XmlDocument Deserialize(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeXmlNode(jsonString);
            }
            catch (Exception e)
            {
                ExceptionFactory.Register("DataException", new ErrorException());
                ExceptionFactory.Handle("Json Deserialize 중에 에러가 발생 했습니다. Json 문자열과 AnonymousType의 타입을 검사해야합니다.", e);
            }

            return null;
        }
    }

    /// <summary>
    /// <see cref="DataTable"/> 객체를 JSON 문자열로 변환합니다.
    /// </summary>
    public class DataTableConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// JSON 문자열을 생성합니다.
        /// </summary>
        /// <param name="writer">JsonWriter 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="value">JsonSerializer 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataTable dataTable = (DataTable)value;
            DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
            writer.WriteStartArray();
            foreach (DataRow row in dataTable.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (serializer.NullValueHandling != NullValueHandling.Ignore || row[column] != null && row[column] != DBNull.Value)
                    {
                        writer.WritePropertyName(column.ColumnName);
                        serializer.Serialize(writer, (column.DataType == typeof(String) && row[column].GetType().Name == "DBNull") ? "" : row[column]);
                    }
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// DataTable의 인스턴스로부터 할당될 수 있는지 여부를 확인합니다.
        /// </summary>
        /// <param name="valueType">DataTable클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <returns>할당될 수 있으면 true를 아니면 false를 반환합니다.</returns>
        public override bool CanConvert(Type valueType)
        {
            return valueType == typeof(DataTable);
        }

        /// <summary>
        /// [사용안함] JsonConverter의 ReadJson 메서드를 재정의합니다.
        /// </summary>
        /// <param name="reader">JsonReader 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="objectType">DataRow 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="existingValue">기본 클래스입니다.</param>
        /// <param name="serializer">JsonSerializer 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <returns>기본 클래스입니다.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// <see cref="DataSet"/> 객체를 JSON 문자열로 변환합니다.
    /// </summary>
    public class DataSetConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// JSON 문자열을 생성합니다.
        /// </summary>
        /// <param name="writer">JsonWriter 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="value">JsonSerializer 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataSet dataSet = (DataSet)value;
            DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
            DataTableConverter dataTableConverter = new DataTableConverter();
            writer.WriteStartObject();
            foreach (DataTable table in dataSet.Tables)
            {
                writer.WritePropertyName((table.TableName));
                dataTableConverter.WriteJson(writer, table, serializer);
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// DataSet의 인스턴스로부터 할당될 수 있는지 여부를 확인합니다.
        /// </summary>
        /// <param name="valueType">DataRow클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <returns>할당될 수 있으면 true를 아니면 false를 반환합니다.</returns>
        public override bool CanConvert(Type valueType)
        {
            return valueType == typeof(DataSet);
        }

        /// <summary>
        /// [사용안함] JsonConverter의 ReadJson 메서드를 재정의합니다.
        /// </summary>
        /// <param name="reader">JsonReader 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="objectType">DataRow 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="existingValue">기본 클래스입니다.</param>
        /// <param name="serializer">JsonSerializer 클래스 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <returns>기본 클래스입니다.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
