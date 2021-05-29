using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// handsontable 바인딩용 JSON 문서를 생성하는 클래스 라이브러리입니다.
    /// </summary>
    public sealed class GridJson
    {
        /// <summary>
        /// 메모리에 있는 데이터로 구성된 하나의 테이블을 JSON 문서를 생성하기 위한 GridJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리에 있는 데이터로 구성된 하나의 테이블을 나타냅니다.</param>
        /// <returns>GridJsonData 컬렉션입니다.</returns>
        public static GridJsonData ToJsonObject(string fieldID, DataTable source)
        {
            GridJsonData result = new GridJsonData();
            result.FieldID = fieldID;

            Dictionary<string, object> childRow;
            foreach (DataRow dataRow in source.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in source.Columns)
                {
                    childRow.Add(col.ColumnName, dataRow[col]);
                }
                result.Value.Add(childRow);
            }

            return result;
        }
    }

    /// <summary>
    /// handsontable 바인딩용 JSON 문서를 생성하기 위한 엔터티 클래스입니다.
    /// </summary>
    public class GridJsonData
    {
        /// <summary>
        /// 키 값입니다.
        /// </summary>
        public string FieldID { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 나타냅니다.
        /// </summary>
        public List<Dictionary<string, object>> Value { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 초기화하는 기본 생성자입니다.
        /// </summary>
        public GridJsonData()
        {
            Value = new List<Dictionary<string, object>>();
        }
    }
}
