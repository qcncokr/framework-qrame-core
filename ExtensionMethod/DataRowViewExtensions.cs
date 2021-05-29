using System;
using System.Data;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for ADO.NET DataRowViewViews (DataView / DataTable / DataSet)
    /// </summary>
    public static class DataRowViewExtensions
    {

        /// <summary>
        /// Gets the record value casted to the specified data type or the data types default value.
        /// </summary>
        /// <typeparam name="T">The return data type</typeparam>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static T Get<T>(this DataRowView @this, string fieldName)
        {
            return @this.Get<T>(fieldName);
        }

        /// <summary>
        /// Gets the record value casted as byte array.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static byte[] GetBytes(this DataRowView @this, string fieldName)
        {
            return (@this[fieldName] as byte[]);
        }

        /// <summary>
        /// Gets the record value casted as string or null.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static string GetString(this DataRowView @this, string fieldName)
        {
            return @this.GetString(fieldName, null);
        }

        /// <summary>
        /// Gets the record value casted as string or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static string GetString(this DataRowView @this, string fieldName, string defaultValue)
        {
            var value = @this[fieldName];
            return (value is string ? (string)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as Guid or Guid.Empty.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static Guid GetGuid(this DataRowView @this, string fieldName)
        {
            var value = @this[fieldName];
            return (value is Guid ? (Guid)value : Guid.Empty);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or DateTime.MinValue.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string fieldName)
        {
            return @this.GetDateTime(fieldName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string fieldName, DateTime defaultValue)
        {
            var value = @this[fieldName];
            return (value is DateTime ? (DateTime)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string fieldName)
        {
            return new DateTimeOffset(@this.GetDateTime(fieldName), TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string fieldName, DateTimeOffset defaultValue)
        {
            var dt = @this.GetDateTime(fieldName);
            return (dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as int or 0.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static int GetInt32(this DataRowView @this, string fieldName)
        {
            return @this.GetInt32(fieldName, 0);
        }

        /// <summary>
        /// Gets the record value casted as int or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static int GetInt32(this DataRowView @this, string fieldName, int defaultValue)
        {
            var value = @this[fieldName];
            return (value is int ? (int)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as long or 0.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static long GetInt64(this DataRowView @this, string fieldName)
        {
            return @this.GetInt64(fieldName, 0);
        }

        /// <summary>
        /// Gets the record value casted as long or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static long GetInt64(this DataRowView @this, string fieldName, int defaultValue)
        {
            var value = @this[fieldName];
            return (value is long ? (long)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as decimal or 0.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static decimal GetDecimal(this DataRowView @this, string fieldName)
        {
            return @this.GetDecimal(fieldName, 0);
        }

        /// <summary>
        /// Gets the record value casted as decimal or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static decimal GetDecimal(this DataRowView @this, string fieldName, long defaultValue)
        {
            var value = @this[fieldName];
            return (value is decimal ? (decimal)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as bool or false.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static bool GetBoolean(this DataRowView @this, string fieldName)
        {
            return @this.GetBoolean(fieldName, false);
        }

        /// <summary>
        /// Gets the record value casted as bool or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static bool GetBoolean(this DataRowView @this, string fieldName, bool defaultValue)
        {
            var value = @this[fieldName];
            return (value is bool ? (bool)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value as Type class instance or null.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static Type GetType(this DataRowView @this, string fieldName)
        {
            return @this.GetType(fieldName, null);
        }

        /// <summary>
        /// Gets the record value as Type class instance or the specified default value.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static Type GetType(this DataRowView @this, string fieldName, Type defaultValue)
        {
            string classType = @this.GetString(fieldName);

            if (classType.Length > 0)
            {
                Type type = Type.GetType(classType);

                if (type != null)
                {
                    return type;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or null.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static object GetTypeInstance(this DataRowView @this, string fieldName)
        {
            return @this.GetTypeInstance(fieldName, null);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or the specified default type.
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static object GetTypeInstance(this DataRowView @this, string fieldName, Type defaultValue)
        {
            var type = @this.GetType(fieldName, defaultValue);
            return (type != null ? Activator.CreateInstance(type) : null);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or null.
        /// </summary>
        /// <typeparam name="T">The type to be casted to</typeparam>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstance<T>(this DataRowView @this, string fieldName) where T : class
        {
            return (@this.GetTypeInstance(fieldName, null) as T);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or the specified default type.
        /// </summary>
        /// <typeparam name="T">The type to be casted to</typeparam>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <param name="type">The type.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstanceSafe<T>(this DataRowView @this, string fieldName, Type type) where T : class
        {
            var instance = (@this.GetTypeInstance(fieldName, null) as T);
            return (instance ?? Activator.CreateInstance(type) as T);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or an instance from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to be casted to</typeparam>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstanceSafe<T>(this DataRowView @this, string fieldName) where T : class, new()
        {
            var instance = (@this.GetTypeInstance(fieldName, null) as T);
            return (instance ?? new T());
        }

        /// <summary>
        /// Determines whether the record value is DBNull.Value
        /// </summary>
        /// <param name="@this">The data @this.</param>
        /// <param name="fieldName">The name of the record fieldName.</param>
        /// <returns>
        /// 	<c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDBNull(this DataRowView @this, string fieldName)
        {
            var value = @this[fieldName];
            return (value == DBNull.Value);
        }
    }
}
