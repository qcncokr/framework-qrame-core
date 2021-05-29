using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// jqGrid 바인딩용 JSON 문서를 생성하는 클래스 라이브러리입니다.
    /// </summary>
    public sealed class ChartGridJson
    {
        /// <summary>
        /// 메모리에 있는 데이터로 구성된 하나의 테이블을 JSON 문서를 생성하기 위한 ChartJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="source">메모리에 있는 데이터로 구성된 하나의 테이블을 나타냅니다.</param>
        /// <returns>ChartJsonData 컬렉션입니다.</returns>
        public static ChartJsonData ToJsonObject(string fieldID, DataTable source)
        {
            ChartJsonData result = new ChartJsonData();
            result.FieldID = fieldID;

            foreach (DataRow dataRow in source.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row["name"] = dataRow[0].ToString();
                row["data"] = new List<object>();

                int count = dataRow.ItemArray.Length;
                for (int i = 1; i < count; i++)
                {
                    ((List<object>)row["data"]).Add(dataRow[i]);
                }

                result.Value.Add(row);
            }

            return result;
        }
    }

    /// <summary>
    /// highcharts 바인딩용 JSON 문서를 생성하기 위한 엔터티 클래스입니다.
    /// </summary>
    public class ChartJsonData
    {
        /// <summary>
        /// 키 값입니다.
        /// </summary>
        public string FieldID { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 나타냅니다.
        /// </summary>
        public List<object> Value { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 초기화하는 기본 생성자입니다.
        /// </summary>
        public ChartJsonData()
        {
            Value = new List<object>();
        }
    }
}
