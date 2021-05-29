using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// DataSet 바인딩용 JSON 문서를 생성하는 클래스 라이브러리입니다.
    /// </summary>
    public sealed class DataTableJson
    {
        /// <summary>
        /// 메모리에 있는 데이터로 구성된 하나의 테이블을 JSON 문서를 생성하기 위한 GridJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리에 있는 데이터로 구성된 하나의 테이블을 나타냅니다.</param>
        /// <returns>GridJsonData 컬렉션입니다.</returns>
        public static DataTableJsonData ToJsonObject(string fieldID, DataTable source)
        {
            DataTableJsonData result = new DataTableJsonData();
            result.FieldID = fieldID;
            result.Value = source;

            return result;
        }
    }

    /// <summary>
    /// DataSet 바인딩용 JSON 문서를 생성하기 위한 엔터티 클래스입니다.
    /// </summary>
    public class DataTableJsonData
    {
        /// <summary>
        /// 키 값입니다.
        /// </summary>
        public string FieldID { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 나타냅니다.
        /// </summary>
        public DataTable Value { get; set; }

        /// <summary>
        /// 문자열 기반의 키와 값의 컬렉션을 초기화하는 기본 생성자입니다.
        /// </summary>
        public DataTableJsonData()
        {
            Value = new DataTable();
        }
    }
}
