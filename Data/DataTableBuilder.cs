using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Data
{
    /// <summary>
    /// DataTable 결과 셋을 간단하게 만들수 있는 헬퍼 클래스입니다.
    /// </summary>
    public class DataTableBuilder
    {
        /// <summary>
        /// 클래스 내에서 사용하게될 DataTable입니다.
        /// </summary>
        private DataTable resultTable = null;

        /// <summary>
        /// 인스턴스 생성시, 생성할 DataTable에 NewDataTable 이름을 지정합니다.
        /// </summary>
        public DataTableBuilder()
            : this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 생성할 DataTable에 지정한 이름을 지정합니다.
        /// </summary>
        /// <param name="tableName">생성할 DataTable 이름입니다.</param>
        public DataTableBuilder(string tableName)
        {
            resultTable = new DataTable(tableName);
        }

        /// <summary>
        /// 생성할 DataTable에 컬럼을 추가합니다.
        /// </summary>
        /// <param name="columnName">추가할 컬럼명입니다.</param>
        /// <param name="columnType">추가할 컬럼타입입니다.</param>
        public void AddColumn(string columnName, Type columnType)
        {
            DataColumn Column = new DataColumn();
            Column.DataType = columnType;
            Column.ColumnName = columnName;

            resultTable.Columns.Add(Column);
        }

        /// <summary>
        /// 생성할 DataTable에 새로운 로우를 추가합니다.
        /// </summary>
        public void NewRow()
        {
            DataRow rowItem = resultTable.NewRow();
            resultTable.Rows.Add(rowItem);
        }

        /// <summary>
        /// 생성할 DataTable에 값을 설정합니다.
        /// </summary>
        /// <param name="rowIndex">설정할 로우의 인덱스입니다.</param>
        /// <param name="columnName">설정할 컬럼명입니다.</param>
        /// <param name="value">설정할 값입니다.</param>
        public void SetValue(int rowIndex, string columnName, object value)
        {
            resultTable.Rows[rowIndex][columnName] = value;
        }

        /// <summary>
        /// 생성할 DataTable에  값을 설정합니다.
        /// </summary>
        /// <param name="rowIndex">설정할 로우의 인덱스입니다.</param>
        /// <param name="columnIndex">설정할 컬럼의 인덱스입니다.</param>
        /// <param name="value">설정할 값입니다.</param>
        public void SetValue(int rowIndex, int columnIndex, object value)
        {
            resultTable.Rows[rowIndex][columnIndex] = value;
        }

        /// <summary>
        /// 생성할 DataTable에서 값을 가져옵니다.
        /// </summary>
        /// <param name="rowIndex">가져올 로우의 인덱스입니다.</param>
        /// <param name="columnName">가져올 컬럼의 이름입니다.</param>
        /// <returns>값을 반환합니다.</returns>
        public object GetValue(int rowIndex, string columnName)
        {
            return resultTable.Rows[rowIndex][columnName];
        }

        /// <summary>
        /// 생성할 DataTable에서 값을 가져옵니다.
        /// </summary>
        /// <param name="rowIndex">가져올 로우의 인덱스입니다.</param>
        /// <param name="columnIndex">가져올 컬럼의 인덱스입니다.</param>
        /// <returns>값을 반환합니다.</returns>
        public object GetValue(int rowIndex, int columnIndex)
        {
            return resultTable.Rows[rowIndex][columnIndex];
        }

        /// <summary>
        /// DataTable을 가져옵니다.
        /// </summary>
        /// <returns>DataTable을 반환합니다.</returns>
        public DataTable GetDataTable()
        {
            return resultTable;
        }

        /// <summary>
        /// DataTable을 null로 초기화합니다.
        /// </summary>
        public void Clear()
        {
            resultTable = null;
        }

        public static DataSet DataReaderToDataSet(IDataReader reader, string prefix = "dataTable", int dataTableIndex = 0)
        {
            using (DataSet ds = new DataSet())
            {
                do
                {
                    using (DataTable dataTable = new DataTable())
                    using (DataTable schemaTable = reader.GetSchemaTable())
                    {
                        if (schemaTable == null)
                        {
                            continue;
                        }

                        DataRow row;

                        string columnName;
                        DataColumn column;
                        int count = schemaTable.Rows.Count;

                        for (int i = 0; i < count; i++)
                        {
                            row = schemaTable.Rows[i];
                            columnName = (string)row["ColumnName"];

                            column = new DataColumn(columnName, (Type)row["DataType"]);
                            dataTable.Columns.Add(column);
                        }

                        dataTable.TableName = prefix + dataTableIndex.ToString();
                        ds.Tables.Add(dataTable);

                        object[] values = new object[count];

                        try
                        {
                            dataTable.BeginLoadData();
                            while (reader.Read())
                            {
                                reader.GetValues(values);
                                dataTable.LoadDataRow(values, true);
                            }
                        }
                        finally
                        {
                            dataTable.EndLoadData();
                        }
                    }
                    dataTableIndex = dataTableIndex + 1;
                } while (reader.NextResult() == true);

                reader.Close();
                return ds;
            }
        }

        public static DataSet DataReaderToSchemeOnly(IDataReader reader, string prefix = "dataTable", int dataTableIndex = 0)
        {
            using (DataSet ds = new DataSet())
            {
                do
                {
                    using (DataTable schemaTable = reader.GetSchemaTable())
                    {
                        if (schemaTable == null)
                        {
                            continue;
                        }

                        DataTable addTable = schemaTable.Copy();
                        addTable.TableName = prefix + dataTableIndex.ToString();
                        ds.Tables.Add(addTable);
                    }
                    dataTableIndex = dataTableIndex + 1;
                } while (reader.NextResult() == true);

                reader.Close();
                return ds;
            }
        }
    }
}
