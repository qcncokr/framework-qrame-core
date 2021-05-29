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
    public sealed class jqGridJson
    {
        /// <summary>
        /// 메모리 내의 데이터 캐시 데이터를 JSON 문서를 생성하기 위한 jqGridJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리 내의 데이터 캐시 데이터를 나타냅니다.</param>
        /// <returns>jqGridJsonData 컬렉션입니다.</returns>
        public static List<jqGridJsonData> ToJsonObject(string fieldID, DataSet source)
        {
            List<jqGridJsonData> jqGrids = new List<jqGridJsonData>();

            int iRow = 0;

            foreach (DataTable dataTable in source.Tables)
            {
                jqGridJsonData jqGrid = new jqGridJsonData();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    jqGridJsonData.Row row = new jqGridJsonData.Row();
                    row.id = fieldID + iRow.ToString();

                    foreach (object item in dataRow.ItemArray)
                    {
                        row.cell.Add(item.ToString());
                    }

                    jqGrid.rows.Add(row);
                    iRow++;
                }

                jqGrid.name = dataTable.TableName;
                jqGrid.page = 1;
                jqGrid.records = jqGrid.rows.Count;
                jqGrid.total = jqGrid.rows.Count;

                jqGrids.Add(jqGrid);
                iRow = 0;
            }

            return jqGrids;
        }

        /// <summary>
        /// 메모리에 있는 데이터로 구성된 하나의 테이블을 JSON 문서를 생성하기 위한 jqGridJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">메모리에 있는 데이터로 구성된 하나의 테이블을 나타냅니다.</param>
        /// <returns>jqGridJsonData 컬렉션입니다.</returns>
        public static jqGridJsonData ToJsonObject(string fieldID, DataTable source)
        {
            int iRow = 0;

            jqGridJsonData jqGrid = new jqGridJsonData();

            foreach (DataRow dataRow in source.Rows)
            {
                jqGridJsonData.Row row = new jqGridJsonData.Row();
                row.id = fieldID + iRow.ToString();

                foreach (object item in dataRow.ItemArray)
                {
                    row.cell.Add(item.ToString());
                }

                jqGrid.rows.Add(row);
                iRow++;
            }

            jqGrid.name = source.TableName;
            jqGrid.page = 1;
            jqGrid.records = jqGrid.rows.Count;
            jqGrid.total = jqGrid.rows.Count;

            return jqGrid;
        }

        /// <summary>
        /// 앞으로만 이동 가능한 스트림을 JSON 문서를 생성하기 위한 jqGridJsonData 컬렉션으로 반환합니다.
        /// </summary>
        /// <param name="fieldID">접두어 문자열입니다.</param>
        /// <param name="source">앞으로만 이동 가능한 스트림을 나타냅니다.</param>
        /// <returns>jqGridJsonData 컬렉션입니다.</returns>
        public static List<jqGridJsonData> ToJsonObject(string fieldID, IDataReader source)
        {
            List<jqGridJsonData> jqGrids = new List<jqGridJsonData>();

            bool NextResult = false;
            int results = 0;
            int iRow = 0;
            jqGridJsonData jqGrid = new jqGridJsonData();

            do
            {
                if (NextResult == true)
                {
                    jqGrids.Add(jqGrid);
                    jqGrid = new jqGridJsonData();
                    iRow = 0;
                    NextResult = false;
                }

                while (source.Read())
                {
                    jqGridJsonData.Row row = new jqGridJsonData.Row();
                    row.id = fieldID + iRow.ToString();

                    for (int i = 0; i < source.FieldCount; i++)
                    {
                        row.cell.Add(source[i].ToString());
                    }

                    jqGrid.rows.Add(row);
                    iRow++;
                }

                jqGrid.name = "source" + results.ToString();
                jqGrid.page = 1;
                jqGrid.records = jqGrid.rows.Count;
                jqGrid.total = jqGrid.rows.Count;

                jqGrids.Add(jqGrid);
                iRow = 0;
            } while (source.NextResult());

            return jqGrids;
        }
    }

    /// <summary>
    /// jqGrid 바인딩용 JSON 문서를 생성하기 위한 엔터티 클래스입니다.
    /// </summary>
    public class jqGridJsonData
    {
        /// <summary>
        /// jqGrid의 행을 가리키는 Row 클래스입니다.
        /// </summary>
        public class Row
        {
            /// <summary>
            /// jqGrid의 행의 식별자를 가져오거나 설정합니다.
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// jqGrid의 열의 데이터를 가져오거나 설정합니다.
            /// </summary>
            public List<string> cell { get; set; }

            /// <summary>
            ///  jqGrid의 열의 컬렉션을 초기화하는 기본 생성자입니다.
            /// </summary>
            public Row()
            {
                cell = new List<string>();
            }
        }

        /// <summary>
        /// jqGrid 데이터 바인딩용 name 속성을 가져오거나 설정합니다.
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// jqGrid 데이터 바인딩용 page 속성을 가져오거나 설정합니다.
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// jqGrid 데이터 바인딩용 total 속성을 가져오거나 설정합니다.
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// jqGrid 데이터 바인딩용 records 속성을 가져오거나 설정합니다.
        /// </summary>
        public int records { get; set; }

        /// <summary>
        /// jqGrid 데이터 바인딩용 rows 속성을 가져오거나 설정합니다.
        /// </summary>
        public List<Row> rows { get; set; }

        /// <summary>
        /// jqGrid 바인딩용 엔터티 클래스의 키와 값의 컬렉션을 초기화하는 기본 생성자입니다.
        /// </summary>
        public jqGridJsonData()
        {
            rows = new List<Row>();
        }
    }
}
