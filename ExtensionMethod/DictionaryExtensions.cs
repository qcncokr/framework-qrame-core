using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Qrame.CoreFX.ExtensionMethod
{
  /// <summary>
  /// 키/값 쌍의 제네릭이 아닌 컬렉션에 대한 확장 메서드 클래스입니다.
  /// </summary>
  public static class DictionaryExtensions
  {
    /// <summary>
    /// 컬렉션내에 지정된 섹션명에 키값이 포함되어 있는지 확인합니다.
    /// </summary>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="sectionName">컬렉션 섹션명입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <returns>포함되어 있으면 true를, 아니면 false를 반환합니다.</returns>
    public static bool Contains(this IDictionary @this, string sectionName, string key)
    {
      IDictionary newDictionary = @this.Section(sectionName);
      if (newDictionary == null)
      {
        return false;
      }
      return newDictionary.Contains(key);
    }

    /// <summary>
    /// 지정된 값이 제네릭 타입으로 변환이 가능한지 확인합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="input">타입 캐스팅을 수행하여 반환할 object값입니다.</param>
    /// <returns>제네릭 타입으로 변환이 가능하면 true를, 아니면 false를 반환합니다.</returns>
    public static T ConvertTo<T>(object input)
    {
      object obj2 = default(T);
      if (typeof(T) == typeof(int))
      {
        obj2 = Convert.ToInt32(input);
      }
      else if (typeof(T) == typeof(long))
      {
        obj2 = Convert.ToInt64(input);
      }
      else if (typeof(T) == typeof(string))
      {
        obj2 = Convert.ToString(input);
      }
      else if (typeof(T) == typeof(bool))
      {
        obj2 = Convert.ToBoolean(input);
      }
      else if (typeof(T) == typeof(double))
      {
        obj2 = Convert.ToDouble(input);
      }
      else if (typeof(T) == typeof(DateTime))
      {
        obj2 = Convert.ToDateTime(input);
      }
      else
      {
        obj2 = input;
      }
      return (T)obj2;
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 지정한 키값을 제네릭 타입으로 캐스팅하여 반환합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <returns>지정한 제네릭 타입으로 캐스팅된 값입니다.</returns>
    public static T Get<T>(this IDictionary @this, string key)
    {
      object input = @this[key];
      if (input == null)
      {
        return default(T);
      }
      return ConvertTo<T>(input);
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 지정한 키값을 제네릭 타입으로 캐스팅하여 반환합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="section">컬렉션 섹션명입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <returns>지정한 제네릭 타입으로 캐스팅된 값입니다.</returns>
    public static T Get<T>(this IDictionary @this, string section, string key)
    {
      object input = @this.Get(section, key);
      if (input == null)
      {
        return default(T);
      }
      return ConvertTo<T>(input);
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 지정한 키값을 제네릭 타입으로 캐스팅하여 반환합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="sectionName">컬렉션 섹션명입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <returns>지정한 제네릭 타입으로 캐스팅된 값입니다.</returns>
    public static object Get(this IDictionary @this, string sectionName, string key)
    {
      if (!@this.Contains(sectionName))
      {
        return null;
      }
      IDictionary newDictionary = @this[sectionName] as IDictionary;
      if (!newDictionary.Contains(key))
      {
        return null;
      }
      return newDictionary[key];
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 지정한 키값을 제네릭 타입으로 캐스팅하여 반환합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="sectionName">컬렉션 섹션명입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <param name="defaultValue">반환하는 도중 에러가 발생할경우 반환할 기본값입니다.</param>
    /// <returns>지정한 제네릭 타입으로 캐스팅된 값입니다.</returns>
    public static T Get<T>(this IDictionary @this, string section, string key, T defaultValue)
    {
      if (!@this.Contains(section, key))
      {
        return defaultValue;
      }
      return @this.Get<T>(section, key);
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 지정한 키값을 제네릭 타입으로 캐스팅하여 반환합니다.
    /// </summary>
    /// <typeparam name="T">데이터 타입을 표현하는 제네릭 타입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="key">컬렉션 키값입니다.</param>
    /// <param name="defaultValue">반환하는 도중 에러가 발생할경우 반환할 기본값입니다.</param>
    /// <returns>지정한 제네릭 타입으로 캐스팅된 값입니다.</returns>
    public static T GetOrDefault<T>(this IDictionary @this, string key, T defaultValue)
    {
      if (!@this.Contains(key))
      {
        return defaultValue;
      }
      return @this.Get<T>(key);
    }

    /// <summary>
    /// 키/값 쌍의 제네릭이 아닌 컬렉션에서 섹션명에 맞는 컬렉션을 반환합니다.
    /// </summary>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <param name="section">컬렉션 섹션명입니다.</param>
    /// <returns>키/값 쌍의 제네릭이 아닌 컬렉션입니다.</returns>
    public static IDictionary Section(this IDictionary @this, string section)
    {
      if (@this.Contains(section))
      {
        return (@this[section] as IDictionary);
      }
      return null;
    }

    /// <summary>
    /// 키의 해시 코드에 따라 구성되는 키/값 쌍의 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="T">제네릭 타입입니다.</typeparam>
    /// <typeparam name="V">제네릭 타입입니다.</typeparam>
    /// <param name="@this">키/값 쌍의 제네릭이 아닌 컬렉션입니다.</param>
    /// <returns>키의 해시 코드에 따라 구성되는 키/값 쌍의 컬렉션입니다.</returns>
    public static Hashtable ToHashTable<T, V>(this IDictionary<T, V> @this)
    {
      var table = new Hashtable();

      foreach (var item in @this)
        table.Add(item.Key, item.Value);

      return table;
    }

    public static DbParameter[] ToDbParameters(this IDictionary<string, object> @this, DbCommand command)
    {
      return @this.Select(x =>
      {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = x.Key;
        parameter.Value = x.Value;
        return parameter;
      }).ToArray();
    }

    public static DbParameter[] ToDbParameters(this IDictionary<string, object> @this, DbConnection connection)
    {
      DbCommand command = connection.CreateCommand();

      return @this.Select(x =>
      {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = x.Key;
        parameter.Value = x.Value;
        return parameter;
      }).ToArray();
    }
  }
}
