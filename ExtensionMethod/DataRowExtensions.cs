using System;
using System.Data;
using System.Reflection;
using Qrame.CoreFX;
using Qrame.CoreFX.ExpandObjects.DataObject;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// ADO.NET DataRow 클래스를 대상으로 동작하는 확장 메서드 클래스입니다.
    /// 
    /// 주요 기능으로 다음과 같습니다.
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// DataRow 데이터 타입의 값을 초기화합니다. 
        /// 이 행에 스키마가 있는 DataTable이 null일 경우, 아무런 작업을 수행하지 않습니다.
        /// </summary>
        /// <param name="@this">값을 초기화할 DataRow 타입 입니다.</param>
        public static void Initialize(this DataRow @this)
        {
            if (@this.Table == null)
            {
                return;
            }

            DataColumnCollection dataColumns = @this.Table.Columns;

            for (int i = 0; i < dataColumns.Count; i++)
            {
                if (@this.IsNull(i) == false)
                {
                    continue;
                }

                string rowType = dataColumns[i].DataType.Name;

                switch (rowType)
                {
                    case "String":
                        @this[i] = "";
                        break;
                    case "Int":
                        @this[i] = 0;
                        break;
                    case "Byte":
                        @this[i] = 0;
                        break;
                    case "Decimal":
                        @this[i] = 0.00M;
                        break;
                    case "Double":
                        @this[i] = 0.00;
                        break;
                    case "Boolean":
                        @this[i] = false;
                        break;
                    case "DateTime":
                        @this[i] = DateTime.MinValue;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// @this DataRow의 데이터를 @this DataRow의 컬럼명으로 복사합니다.
        /// 데이터를 복사하는 과정 중에 예외 발생시, 예외를 무시하고 다음 복사를 진행합니다.
        /// 이 행에 스키마가 있는 DataTable이 null일 경우, 아무런 작업을 수행하지 않습니다.
        /// </summary>
        /// <param name="@this">복사 원본 DataRow 타입입니다.</param>
        /// <param name="@this">복사할 대상 DataRow 타입입니다.</param>
        /// <returns>DataRow 데이터 복사가 정상적으로 완료되면 true를, 복사 도중 예외를 만나면 false를 반환합니다.</returns>
        public static bool ForceCopyDataRow(this DataRow @this, DataRow target)
        {
            bool result = true;
            if (@this.Table == null || target.Table == null)
            {
                return false;
            }

            DataColumnCollection dataColumns = target.Table.Columns;

            for (int i = 0; i < dataColumns.Count; i++)
            {
                try
                {
                    target[i] = @this[dataColumns[i].ColumnName];
                }
                catch 
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// @this DataRow의 데이터를 @this DataRow의 컬럼명으로 복사합니다.
        /// 이 행에 스키마가 있는 DataTable이 null일 경우, 아무런 작업을 수행하지 않습니다.
        /// </summary>
        /// <param name="@this">복사 원본 DataRow 타입입니다.</param>
        /// <param name="@this">복사할 대상 DataRow 타입입니다.</param>
        public static void CopyDataRow(this DataRow @this, DataRow target)
        {
            if (@this.Table == null || target.Table == null)
            {
                return;
            }

            DataColumnCollection dataColumns = target.Table.Columns;

            for (int i = 0; i < dataColumns.Count; i++)
            {
                try
                {
                    target[i] = @this[dataColumns[i].ColumnName];
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// @this DataRow의 데이터를 TargetObject의 필드 또는 속성명으로 비교하여 데이터를 설정합니다.
        /// 이 행에 스키마가 있는 DataTable이 null일 경우, 아무런 작업을 수행하지 않습니다.
        /// </summary>
        /// <param name="@this">복사 원본 DataRow 타입입니다.</param>
        /// <param name="target">복사할 대상 object 타입입니다.</param>
        public static void CopyObjectFromDataRow(this DataRow @this, object target)
        {
            if (@this.Table == null)
            {
                return;
            }

            try
            {
                MemberInfo[] memberInfos = target.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, Reflector.memberAccess, null, null);

                foreach (MemberInfo memberInfo in memberInfos)
                {
                    string propertyName = memberInfo.Name;

                    if (@this.Table.Columns.Contains(propertyName) == false)
                    {
                        continue;
                    }

                    switch (memberInfo.MemberType)
                    {
                        case MemberTypes.All:
                            break;
                        case MemberTypes.Constructor:
                            break;
                        case MemberTypes.Custom:
                            break;
                        case MemberTypes.Event:
                            break;
                        case MemberTypes.Field:
                            Reflector.SetField(target, propertyName, @this[propertyName]);
                            break;
                        case MemberTypes.Method:
                            break;
                        case MemberTypes.NestedType:
                            break;
                        case MemberTypes.Property:
                            Reflector.SetProperty(target, propertyName, @this[propertyName]);
                            break;
                        case MemberTypes.TypeInfo:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// @this DataRow의 데이터를 TargetObject의 필드 또는 속성명으로 비교하여 데이터를 설정합니다.
        /// 이 행에 스키마가 있는 DataTable이 null일 경우, 아무런 작업을 수행하지 않습니다.
        /// 데이터를 복사하는 과정 중에 예외 발생시, 예외를 무시하고 다음 복사를 진행합니다.
        /// </summary>
        /// <param name="@this">복사 원본 DataRow 타입입니다.</param>
        /// <param name="target">복사할 대상 object 타입입니다.</param>
        /// <returns>DataRow 데이터 복사가 정상적으로 완료되면 true를, 복사 도중 예외를 만나면 false를 반환합니다.</returns>
        public static bool CopyObjectToDataRow(this DataRow @this, object target)
        {
            bool result = true;

            if (@this.Table == null)
            {
                return false;
            }

            try
            {
                MemberInfo[] memberInfos = target.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, Reflector.memberAccess, null, null);

                foreach (MemberInfo memberInfo in memberInfos)
                {
                    string propertyName = memberInfo.Name;

                    if (@this.Table.Columns.Contains(propertyName) == false)
                    {
                        continue;
                    }

                    switch (memberInfo.MemberType)
                    {
                        case MemberTypes.All:
                            break;
                        case MemberTypes.Constructor:
                            break;
                        case MemberTypes.Custom:
                            break;
                        case MemberTypes.Event:
                            break;
                        case MemberTypes.Field:
                            Reflector.SetField(target, propertyName, @this[propertyName]);
                            break;
                        case MemberTypes.Method:
                            break;
                        case MemberTypes.NestedType:
                            break;
                        case MemberTypes.Property:
                            Reflector.SetProperty(target, propertyName, @this[propertyName]);
                            break;
                        case MemberTypes.TypeInfo:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 byte[]로 변환하여 가져옵니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 byte[]로 가져옵니다. byte[] 변환이 되지 않으면 null을 반환합니다.</returns>
        public static byte[] GetBytes(this DataRow @this, string fieldName)
        {
            return (@this[fieldName] as byte[]);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 string로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 null을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 string로 가져옵니다.string로 변환이 되지 않으면 null을 반환합니다.</returns>
        public static string GetString(this DataRow @this, string fieldName)
        {
            return @this.GetString(fieldName, null);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 string로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없거나 예외가 발생 할 경우 defaultValue을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">string로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 string로 가져옵니다. string로 변환이 되지 않으면 defaultValue을 반환합니다.</returns>
        public static string GetString(this DataRow @this, string fieldName, string defaultValue)
        {
            var value = @this[fieldName];
            return (value is string ? (string)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 Guid로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 빈 Guid를 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 Guid로 가져옵니다. Guid로 변환이 되지 않으면 빈 Guid를 반환합니다.</returns>
        public static Guid GetGuid(this DataRow @this, string fieldName)
        {
            var value = @this[fieldName];
            return (value is Guid ? (Guid)value : Guid.Empty);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 DateTime으로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 DateTime의 최소값을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 DateTime으로 가져옵니다. DateTime으로 변환이 되지 않으면 빈 DateTime의 최소값을 반환합니다.</returns>
        public static DateTime GetDateTime(this DataRow @this, string fieldName)
        {
            return @this.GetDateTime(fieldName, DateTime.MinValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 DateTime으로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 DateTime의 최소값을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">DateTime으로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 DateTime으로 가져옵니다. DateTime으로 변환이 되지 않으면 defaultValue의 최소값을 반환합니다.</returns>
        public static DateTime GetDateTime(this DataRow @this, string fieldName, DateTime defaultValue)
        {
            var value = @this[fieldName];
            return (value is DateTime ? (DateTime)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 int로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 0을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 int로 가져옵니다. int로 변환이 되지 않으면 0을 반환합니다.</returns>
        public static int GetInt32(this DataRow @this, string fieldName)
        {
            return @this.GetInt32(fieldName, 0);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 int로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없거나 예외가 발생 할 경우 defaultValue을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">int로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 int로 가져옵니다. int로 변환이 되지 않으면 defaultValue을 반환합니다.</returns>
        public static int GetInt32(this DataRow @this, string fieldName, int defaultValue)
        {
            var value = @this[fieldName];
            return (value is int ? (int)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 long로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 0을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 long로 가져옵니다. long로 변환이 되지 않으면 0을 반환합니다.</returns>
        public static long GetInt64(this DataRow @this, string fieldName)
        {
            return @this.GetInt64(fieldName, 0);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 long로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없거나 예외가 발생 할 경우 defaultValue을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">long로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 long로 가져옵니다. long로 변환이 되지 않으면 defaultValue을 반환합니다.</returns>
        public static long GetInt64(this DataRow @this, string fieldName, int defaultValue)
        {
            var value = @this[fieldName];
            return (value is long ? (long)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 decimal로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 0을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 decimal로 가져옵니다. decimal로 변환이 되지 않으면 0을 반환합니다.</returns>
        public static decimal GetDecimal(this DataRow @this, string fieldName)
        {
            return @this.GetDecimal(fieldName, 0);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 decimal로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없거나 예외가 발생 할 경우 defaultValue을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">decimal로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 decimal로 가져옵니다. decimal로 변환이 되지 않으면 defaultValue을 반환합니다.</returns>
        public static decimal GetDecimal(this DataRow @this, string fieldName, long defaultValue)
        {
            var value = @this[fieldName];
            return (value is decimal ? (decimal)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 bool로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없을 경우 false를 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터를 bool로 가져옵니다. bool로 변환이 되지 않으면 false를 반환합니다.</returns>
        public static bool GetBoolean(this DataRow @this, string fieldName)
        {
            return @this.GetBoolean(fieldName, false);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터를 bool로 변환하여 가져옵니다.
        /// 지정된 이름을 가진 열이 없거나 예외가 발생 할 경우 defaultValue을 반환합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 가져올 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <param name="defaultValue">bool로 변환시 예외가 발생 하면, 반환 할 기본값입니다.</param>
        /// <returns>열에 저장된 데이터를 bool로 가져옵니다. bool로 변환이 되지 않으면 defaultValue을 반환합니다.</returns>
        public static bool GetBoolean(this DataRow @this, string fieldName, bool defaultValue)
        {
            var value = @this[fieldName];
            return (value is bool ? (bool)value : defaultValue);
        }

        /// <summary>
        /// @this DataRow에서 지정된 이름을 가진 열에 저장된 데이터가 null인지 확인합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터가 null인지 확인 할 DataRow 타입입니다.</param>
        /// <param name="fieldName">열에 저장된 데이터를 가져올 필드명입니다.</param>
        /// <returns>열에 저장된 데이터가 null이면 true를, 아니면 false를 반환합니다.</returns>
        public static bool IsDBNull(this DataRow @this, string fieldName)
        {
            var value = @this[fieldName];
            return (value == DBNull.Value);
        }

        /// <summary>
        /// @this DataRow에 데이터를 문자열 배열 순으로 변경합니다. (주의)이 행에 스키마가 신뢰할 수 있을 경우에만 사용해야 합니다.
        /// </summary>
        /// <param name="@this">열에 저장된 데이터를 변경할 DataRow 타입입니다.</param>
        /// <param name="values">DataRow에 데이터를 복사 할 문자열 배열입니다.</param>
        /// <returns></returns>
        public static DataRow AddDataRow(this DataRow @this, params string[] values)
        {
            int i = 0;
            foreach (string value in values)
            {
                @this[i] = value;
                i++;
            }

            return @this;
        }

        public static dynamic AsDynamic(this DataRow @this)
        {
            return new DynamicDataRow(@this);
        }
    }
}
