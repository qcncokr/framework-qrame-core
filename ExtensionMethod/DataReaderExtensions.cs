using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Dynamic;

namespace Qrame.CoreFX.ExtensionMethod
{
  /// <summary>
  /// Extension methods for all kind of ADO.NET DataReaders (SqlDataReader, OracleDataReader, ...)
  /// </summary>
  public static class DataReaderExtensions
  {

    /// <summary>
    /// Gets the record value casted to the specified data type or the data types default value.
    /// </summary>
    /// <typeparam name="T">The return data type</typeparam>
    /// <param name="@this">The data @this.</param>
    /// <param name="fieldName">The name of the record fieldName.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this IDataReader @this, string fieldName)
    {
      return (T)@this.GetValue(@this.GetOrdinal(fieldName));
    }

    public static T Get<T>(this IDataReader @this, int index)
    {
      return (T)@this.GetValue(index);
    }

    public static T GetValueAsOrDefault<T>(this IDataReader @this, string columnName, T defaultValue)
    {
      try
      {
        return (T)@this.GetValue(@this.GetOrdinal(columnName));
      }
      catch
      {
        return defaultValue;
      }
    }

    public static T GetValueAsOrDefault<T>(this IDataReader @this, string columnName)
    {
      try
      {
        return (T)@this.GetValue(@this.GetOrdinal(columnName));
      }
      catch
      {
        return default(T);
      }
    }

    public static T GetValueAsOrDefault<T>(this IDataReader @this, int index, T defaultValue)
    {
      try
      {
        return (T)@this.GetValue(index);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static T GetValueAsOrDefault<T>(this IDataReader @this, int index)
    {
      try
      {
        return (T)@this.GetValue(index);
      }
      catch
      {
        return default(T);
      }
    }

    /// <summary>
    /// Gets the record value casted as byte array.
    /// </summary>
    /// <param name="@this">The data @this.</param>
    /// <param name="fieldName">The name of the record fieldName.</param>
    /// <returns>The record value</returns>
    public static byte[] GetBytes(this IDataReader @this, string fieldName)
    {
      return (@this[fieldName] as byte[]);
    }

    /// <summary>
    /// Gets the record value casted as string or null.
    /// </summary>
    /// <param name="@this">The data @this.</param>
    /// <param name="fieldName">The name of the record fieldName.</param>
    /// <returns>The record value</returns>
    public static string GetString(this IDataReader @this, string fieldName)
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
    public static string GetString(this IDataReader @this, string fieldName, string defaultValue)
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
    public static Guid GetGuid(this IDataReader @this, string fieldName)
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
    public static DateTime GetDateTime(this IDataReader @this, string fieldName)
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
    public static DateTime GetDateTime(this IDataReader @this, string fieldName, DateTime defaultValue)
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
    public static DateTimeOffset GetDateTimeOffset(this IDataReader @this, string fieldName)
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
    public static DateTimeOffset GetDateTimeOffset(this IDataReader @this, string fieldName, DateTimeOffset defaultValue)
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
    public static int GetInt32(this IDataReader @this, string fieldName)
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
    public static int GetInt32(this IDataReader @this, string fieldName, int defaultValue)
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
    public static long GetInt64(this IDataReader @this, string fieldName)
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
    public static long GetInt64(this IDataReader @this, string fieldName, int defaultValue)
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
    public static decimal GetDecimal(this IDataReader @this, string fieldName)
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
    public static decimal GetDecimal(this IDataReader @this, string fieldName, long defaultValue)
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
    public static bool GetBoolean(this IDataReader @this, string fieldName)
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
    public static bool GetBoolean(this IDataReader @this, string fieldName, bool defaultValue)
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
    public static Type GetType(this IDataReader @this, string fieldName)
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
    public static Type GetType(this IDataReader @this, string fieldName, Type defaultValue)
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
    public static object GetTypeInstance(this IDataReader @this, string fieldName)
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
    public static object GetTypeInstance(this IDataReader @this, string fieldName, Type defaultValue)
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
    public static T GetTypeInstance<T>(this IDataReader @this, string fieldName) where T : class
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
    public static T GetTypeInstanceSafe<T>(this IDataReader @this, string fieldName, Type type) where T : class
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
    public static T GetTypeInstanceSafe<T>(this IDataReader @this, string fieldName) where T : class, new()
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
    public static bool IsDBNull(this IDataReader @this, string fieldName)
    {
      return @this.IsDBNull(@this.GetOrdinal(fieldName));
    }

    /// <summary>
    /// Reads all all records from a data @this and performs an action for each.
    /// </summary>
    /// <param name="@this">The data @this.</param>
    /// <param name="action">The action to be performed.</param>
    /// <returns>
    /// The count of actions that were performed.
    /// </returns>
    public static int ReadAll(this IDataReader @this, Action<IDataReader> action)
    {
      var count = 0;
      while (@this.Read())
      {
        action(@this);
        count++;
      }
      return count;
    }

    /// <summary>
    /// Populates the properties of an object from a single DataReader row using
    /// Reflection by matching the public properties of the object with
    /// the DataReader fields to read from by exact name. Unmatched properties
    /// are left unchanged.
    /// 
    /// You need to pass in a data @this located on the active row you want
    /// to serialize.
    /// 
    /// This routine works best for matching pure data entities and should
    /// be used only in low volume environments where retrieval speed is not
    /// critical due to its use of Reflection.
    /// </summary>
    /// <param name="@this">Instance of the DataReader to read data from. Should be located on the correct record (Read() should have been called on it before calling this method)</param>
    /// <param name="instance">Instance of the object to populate properties on</param>
    public static void ToObject(this IDataReader @this, List<string> columnNames, object instance)
    {
      for (int i = 0; i < columnNames.Count; i++)
      {
        object value = @this.GetValue(i);
        if (value != DBNull.Value)
        {
          PropertyInfo propertyInfo = instance.GetType().GetProperty(columnNames[i]);

          if (propertyInfo != null)
          {
            propertyInfo.SetValue(instance, value, null);
          }
        }
      }

      // PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
      // foreach (PropertyInfo property in properties)
      // {
      //     if (!property.CanRead || !property.CanWrite)
      //     {
      //         continue;
      //     }
      // 
      //     string Name = property.Name;
      // 
      //     object value = null;
      // 
      //     try
      //     {
      //         if (@this.ColumnExists(Name) == false)
      //         {
      //             continue;
      //         }
      // 
      //         value = @this[Name];
      // 
      //         if (value is DBNull)
      //         {
      //             value = null;
      //         }
      //     }
      //     catch
      //     {
      //         continue;
      //     }
      // 
      //     property.SetValue(instance, value, null);
      // }
    }

    public static bool ColumnExists(this IDataReader reader, string columnName)
    {
      for (int i = 0; i < reader.FieldCount; i++)
      {
        if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Populates the properties of an object from a single DataReader row using
    /// Reflection by matching the public properties of the object with
    /// the DataReader fields to read from by exact name. Unmatched properties
    /// are left unchanged.
    /// 
    /// You need to pass in a data @this located on the active row you want
    /// to serialize.
    /// 
    /// This routine works best for matching pure data entities and should
    /// be used only in low volume environments where retrieval speed is not
    /// critical due to its use of Reflection.
    /// </summary>
    /// <param name="@this">Instance of the DataReader to read data from. Should be located on the correct record (Read() should have been called on it before calling this method)</param>
    /// <param name="instance">Instance of the object to populate properties on</param>
    /// <param name="fieldsToSkip">A comma delimited list of object properties that should not be updated</param>
    public static void ToObject(this IDataReader @this, object instance, string fieldsToSkip)
    {
      if (fieldsToSkip.Length == 0)
      {
        fieldsToSkip = "";
      }
      else
      {
        fieldsToSkip = "," + fieldsToSkip + ",";
      }

      fieldsToSkip = fieldsToSkip.ToLower();

      if (@this.IsClosed)
      {
        throw new InvalidOperationException("DataReader passed to DataReaderToObject cannot be closed");
      }

      PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (PropertyInfo property in properties)
      {
        if (!property.CanRead || !property.CanWrite)
        {
          continue;
        }

        string Name = property.Name;

        if (fieldsToSkip.Contains("," + Name.ToLower() + ","))
        {
          continue;
        }

        object value = null;

        try
        {
          value = @this[Name];

          if (value is DBNull)
          {
            value = null;
          }
        }
        catch
        {
          continue;
        }

        property.SetValue(instance, value, null);
      }
    }

    /// <summary>
    /// Creates a list of a given type from all the rows in a DataReader.
    /// 
    /// Note this method uses Reflection so this isn't a high performance
    /// operation, but it can be useful for generic data @this to entity
    /// conversions on the fly and with anonymous types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="@this"></param>
    /// <param name="fieldsToSkip"></param>
    /// <returns></returns>
    public static List<T> ToObjectList<T>(this IDataReader @this)
    {
      if (@this == null)
      {
        return null;
      }

      List<T> items = new List<T>();

      List<string> columnNames = new List<string>();
      for (int i = 0; i < @this.FieldCount; i++)
      {
        columnNames.Add(@this.GetName(i));
      }

      while (@this.Read())
      {
        var instance = Activator.CreateInstance<T>();
        ToObject(@this, columnNames, instance);

        items.Add(instance);
      }

      return items;
    }

    public static List<T> ToObjectList<T>(this IDataReader @this, string fieldsToSkip)
    {
      if (@this == null)
      {
        return null;
      }

      List<T> items = new List<T>();

      while (@this.Read())
      {
        var instance = Activator.CreateInstance<T>();
        ToObject(@this, instance, fieldsToSkip);

        items.Add(instance);
      }

      return items;
    }

    public static bool ContainsColumn(this IDataReader @this, int columnIndex)
    {
      try
      {
        return @this.FieldCount > columnIndex;
      }
      catch (Exception)
      {
        try
        {
          return @this[columnIndex] != null;
        }
        catch (Exception)
        {
          return false;
        }
      }
    }

    public static bool ContainsColumn(this IDataReader @this, string columnName)
    {
      try
      {
        return @this.GetOrdinal(columnName) != -1;
      }
      catch (Exception)
      {
        try
        {
          return @this[columnName] != null;
        }
        catch (Exception)
        {
          return false;
        }
      }
    }

    public static IDataReader ForEach(this IDataReader @this, Action<IDataReader> action)
    {
      while (@this.Read())
      {
        action(@this);
      }

      return @this;
    }

    public static IEnumerable<string> GetColumnNames(this IDataRecord @this)
    {
      return Enumerable.Range(0, @this.FieldCount)
          .Select(@this.GetName)
          .ToList();
    }

    public static DataTable ToDataTable(this IDataReader @this)
    {
      var dt = new DataTable();
      dt.Load(@this);
      return dt;
    }

    public static IEnumerable<T> ToEntities<T>(this IDataReader @this) where T : new()
    {
      Type type = typeof(T);
      PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

      var list = new List<T>();

      var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
          .Select(@this.GetName));

      while (@this.Read())
      {
        var entity = new T();

        foreach (PropertyInfo property in properties)
        {
          if (hash.Contains(property.Name))
          {
            Type valueType = property.PropertyType;
            property.SetValue(entity, @this[property.Name].To(valueType), null);
          }
        }

        foreach (FieldInfo field in fields)
        {
          if (hash.Contains(field.Name))
          {
            Type valueType = field.FieldType;
            field.SetValue(entity, @this[field.Name].To(valueType));
          }
        }

        list.Add(entity);
      }

      return list;
    }

    public static T ToEntity<T>(this IDataReader @this) where T : new()
    {
      Type type = typeof(T);
      PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

      var entity = new T();

      var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
          .Select(@this.GetName));

      foreach (PropertyInfo property in properties)
      {
        if (hash.Contains(property.Name))
        {
          Type valueType = property.PropertyType;
          property.SetValue(entity, @this[property.Name].To(valueType), null);
        }
      }

      foreach (FieldInfo field in fields)
      {
        if (hash.Contains(field.Name))
        {
          Type valueType = field.FieldType;
          field.SetValue(entity, @this[field.Name].To(valueType));
        }
      }

      return entity;
    }

    public static dynamic ToExpandoObject(this IDataReader @this)
    {
      Dictionary<int, KeyValuePair<int, string>> columnNames = Enumerable.Range(0, @this.FieldCount)
          .Select(x => new KeyValuePair<int, string>(x, @this.GetName(x)))
          .ToDictionary(pair => pair.Key);

      dynamic entity = new ExpandoObject();
      var expandoDict = (IDictionary<string, object>)entity;

      Enumerable.Range(0, @this.FieldCount)
          .ToList()
          .ForEach(x => expandoDict.Add(columnNames[x].Value, @this[x]));

      return entity;
    }

    public static IEnumerable<dynamic> ToExpandoObjects(this IDataReader @this)
    {
      Dictionary<int, KeyValuePair<int, string>> columnNames = Enumerable.Range(0, @this.FieldCount)
          .Select(x => new KeyValuePair<int, string>(x, @this.GetName(x)))
          .ToDictionary(pair => pair.Key);

      var list = new List<dynamic>();

      while (@this.Read())
      {
        dynamic entity = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>)entity;

        Enumerable.Range(0, @this.FieldCount)
            .ToList()
            .ForEach(x => expandoDict.Add(columnNames[x].Value, @this[x]));

        list.Add(entity);
      }

      return list;
    }
  }
}
