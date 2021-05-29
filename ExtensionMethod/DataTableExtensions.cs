using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Qrame.CoreFX.ExtensionMethod
{
	public static class DataTableExtensions
	{
		/// <summary>
		/// 데이터테이블을 컬럼을 추가합니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="columnName">추가할 컬럼명입니다.</param>
		/// <param name="columnType">추가할 컬럼타입입니다.</param>
		public static void AddColumn(this DataTable @this, string columnName, Type columnType)
		{
			@this.Columns.Add(new DataColumn() { DataType = columnType, ColumnName = columnName });
		}

		/// <summary>
		/// 데이터테이블을 컬럼을 삭제합니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="columnName">추가할 컬럼명입니다.</param>
		/// <param name="columnType">추가할 컬럼타입입니다.</param>
		public static void RemoveColumn(this DataTable @this, string columnName)
		{
			@this.Columns.Remove(columnName);
		}

		/// <summary>
		/// 새로운 로우를 추가합니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		public static void NewRow(this DataTable @this)
		{
			@this.Rows.Add(@this.NewRow());
		}

		/// <summary>
		/// 데이터테이블에 값을 설정합니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="rowIndex">설정할 로우의 인덱스입니다.</param>
		/// <param name="columnName">설정할 컬럼명입니다.</param>
		/// <param name="value">설정할 값입니다.</param>
		public static void SetValue(this DataTable @this, int rowIndex, string columnName, object value)
		{
			@this.Rows[rowIndex][columnName] = value;
		}

		/// <summary>
		/// 데이터테이블에 값을 설정합니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="rowIndex">설정할 로우의 인덱스입니다.</param>
		/// <param name="columnIndex">설정할 컬럼의 인덱스입니다.</param>
		/// <param name="value">설정할 값입니다.</param>
		public static void SetValue(this DataTable @this, int rowIndex, int columnIndex, object value)
		{
			@this.Rows[rowIndex][columnIndex] = value;
		}

		/// <summary>
		/// 데이터테이블에서 값을 가져옵니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="rowIndex">가져올 로우의 인덱스입니다.</param>
		/// <param name="columnName">가져올 컬럼의 이름입니다.</param>
		/// <returns>값을 반환합니다.</returns>
		public static object GetValue(this DataTable @this, int rowIndex, string columnName)
		{
			return @this.Rows[rowIndex][columnName];
		}

		/// <summary>
		/// 데이터테이블에서 값을 가져옵니다.
		/// </summary>
		/// <param name="@this">DataTable</param>
		/// <param name="rowIndex">가져올 로우의 인덱스입니다.</param>
		/// <param name="columnIndex">가져올 컬럼의 인덱스입니다.</param>
		/// <returns>값을 반환합니다.</returns>
		public static object GetValue(this DataTable @this, int rowIndex, int columnIndex)
		{
			return @this.Rows[rowIndex][columnIndex];
		}

		/// <summary>
		/// Linq 쿼리 결과 데이터를 DataTable로 반환합니다
		/// </summary>
		/// <typeparam name="T">제네릭 타입입니다</typeparam>
		/// <param name="source">열거형 데이터 정보입니다</param>
		/// <returns></returns>
		public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
		{
			return new ObjectShredder<T>().Shred(source, null, null);
		}

		public static DataTable CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
		{
			return new ObjectShredder<T>().Shred(source, table, options);
		}
	}

	internal class ObjectShredder<T>
	{
		private FieldInfo[] _fi;
		private PropertyInfo[] _pi;
		private Dictionary<string, int> _ordinalMap;
		private Type _type;

		public ObjectShredder()
		{
			_type = typeof(T);
			_fi = _type.GetFields();
			_pi = _type.GetProperties();
			_ordinalMap = new Dictionary<string, int>();
		}

		public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
		{
			if (typeof(T).IsPrimitive)
			{
				return ShredPrimitive(source, table, options);
			}

			table = table == null ? new DataTable(typeof(T).Name) : table;
			table = ExtendTable(table, typeof(T));
			table.BeginLoadData();
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				while (e.MoveNext())
				{
					if (options != null)
					{
						table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
					}
					else
					{
						table.LoadDataRow(ShredObject(table, e.Current), true);
					}
				}
			}
			table.EndLoadData();
			return table;
		}

		public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
		{
			table = table == null ? new DataTable(typeof(T).Name) : table;

			if (!table.Columns.Contains("Value"))
			{
				table.Columns.Add("Value", typeof(T));
			}

			table.BeginLoadData();
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				Object[] values = new object[table.Columns.Count];
				while (e.MoveNext())
				{
					values[table.Columns["Value"].Ordinal] = e.Current;

					if (options != null)
					{
						table.LoadDataRow(values, (LoadOption)options);
					}
					else
					{
						table.LoadDataRow(values, true);
					}
				}
			}
			table.EndLoadData();
			return table;
		}

		public object[] ShredObject(DataTable table, T instance)
		{

			FieldInfo[] fi = _fi;
			PropertyInfo[] pi = _pi;

			if (instance.GetType() != typeof(T))
			{
				ExtendTable(table, instance.GetType());
				fi = instance.GetType().GetFields();
				pi = instance.GetType().GetProperties();
			}

			Object[] values = new object[table.Columns.Count];
			foreach (FieldInfo f in fi)
			{
				values[_ordinalMap[f.Name]] = f.GetValue(instance);
			}

			foreach (PropertyInfo p in pi)
			{
				values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
			}

			return values;
		}

		public DataTable ExtendTable(DataTable table, Type type)
		{
			foreach (FieldInfo f in type.GetFields())
			{
				if (!_ordinalMap.ContainsKey(f.Name))
				{
					DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name] : table.Columns.Add(f.Name, f.FieldType);
					_ordinalMap.Add(f.Name, dc.Ordinal);
				}
			}
			foreach (PropertyInfo p in type.GetProperties())
			{
				if (!_ordinalMap.ContainsKey(p.Name))
				{
					DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name] : table.Columns.Add(p.Name, p.PropertyType);
					_ordinalMap.Add(p.Name, dc.Ordinal);
				}
			}

			return table;
		}
	}
}
