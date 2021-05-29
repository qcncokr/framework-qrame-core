using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// form 바인딩용 JSON 문서를 생성하는 클래스 라이브러리입니다.
    /// </summary>
    public sealed class FormJson
    {
        /// <summary>
        /// 메모리 내의 데이터 캐시 데이터를 JSON 문서를 생성하기 위한 FormJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리 내의 데이터 캐시 데이터를 나타냅니다.</param>
        /// <returns>FormJsonData 컬렉션입니다.</returns>
        public static List<FormJsonData> ToJsonObject(string fieldID, DataSet source)
        {
            List<FormJsonData> formControls = new List<FormJsonData>();

            int colIndex = 0;

            foreach (DataTable dataTable in source.Tables)
            {
                FormJsonData formData = new FormJsonData();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    colIndex = 0;
                    foreach (object item in dataRow.ItemArray)
                    {
                        if (formData.Value.ContainsKey(dataTable.Columns[colIndex].ColumnName) == false)
                        {
                            formData.Value.Add(dataTable.Columns[colIndex].ColumnName, item.ToString());
                        }
                        colIndex++;
                    }
                }

                formData.FieldID = dataTable.TableName;

                formControls.Add(formData);
            }

            return formControls;
        }

        /// <summary>
        /// 메모리에 있는 데이터로 구성된 하나의 테이블을 JSON 문서를 생성하기 위한 FormJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리에 있는 데이터로 구성된 하나의 테이블을 나타냅니다.</param>
        /// <returns>FormJsonData 컬렉션입니다.</returns>
        public static FormJsonData ToJsonObject(string fieldID, DataTable source)
        {
            int colIndex = 0;

            FormJsonData formData = new FormJsonData();

            foreach (DataRow dataRow in source.Rows)
            {
                colIndex = 0;
                foreach (object item in dataRow.ItemArray)
                {
                    if (formData.Value.ContainsKey(source.Columns[colIndex].ColumnName) == false)
                    {
                        formData.Value.Add(source.Columns[colIndex].ColumnName, item.ToString());
                    }
                    colIndex++;
                }
            }

            formData.FieldID = fieldID;

            return formData;
        }

        /// <summary>
        /// 앞으로만 이동 가능한 스트림을 JSON 문서를 생성하기 위한 FormJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">앞으로만 이동 가능한 스트림을 나타냅니다.</param>
        /// <returns>FormJsonData 컬렉션입니다.</returns>
        public static List<FormJsonData> ToJsonObject(string fieldID, IDataReader source)
        {
            List<FormJsonData> formControls = new List<FormJsonData>();

            bool NextResult = false;
            int results = 0;
            FormJsonData formData = new FormJsonData();

            do
            {
                if (NextResult == true)
                {
                    formControls.Add(formData);
                    formData = new FormJsonData();
                    NextResult = false;
                }

                while (source.Read())
                {
                    for (int i = 0; i < source.FieldCount; i++)
                    {
                        if (formData.Value.ContainsKey(source.GetName(i)) == false)
                        {
                            formData.Value.Add(source.GetName(i), source[i].ToString());
                        }
                    }
                }

                formData.FieldID = "source" + results.ToString();

                formControls.Add(formData);
            } while (source.NextResult());

            return formControls;
        }
    }

    /// <summary>
    /// form 바인딩용 JSON 문서를 생성하기 위한 엔터티 클래스입니다.
    /// </summary>
    public class FormJsonData
    {
        /// <summary>
        /// 키 값입니다.
        /// </summary>
        public string FieldID { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 나타냅니다.
        /// </summary>
        public Dictionary<string, string> Value { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 초기화하는 기본 생성자입니다.
        /// </summary>
        public FormJsonData()
        {
            Value = new Dictionary<string, string>();
        }
    }
}
