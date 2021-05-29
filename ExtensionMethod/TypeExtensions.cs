using System.Linq;
using System;
using System.Reflection;
using System.Globalization;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class TypeExtensions
    {
        public static string GetFriendlyName(this Type @this)
        {
            if (@this == null)
                throw new ArgumentNullException("@this", "@this은 null일 수 없습니다");

            if (!@this.IsGenericType || @this.IsGenericParameter)
                return @this.Name;

            const StringComparison sc = StringComparison.Ordinal;
            string name = @this.Name.Substring(0, @this.Name.IndexOf("`", sc));
            string[] arguments = @this.GetGenericArguments()
                .Select(arg => arg.GetFriendlyName())
                .ToArray();

            return String.Concat(name, '<', String.Join(", ", arguments), '>');
        }

        public static DataTable ToDataTable<T>(this T[] @this)
        {
            Type typeOf = typeof(T);

            PropertyInfo[] properties = typeOf.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = typeOf.GetFields(BindingFlags.Public | BindingFlags.Instance);

            DataTable result = new DataTable();

            foreach (PropertyInfo property in properties)
            {
                result.Columns.Add(property.Name, property.PropertyType);
            }

            foreach (FieldInfo field in fields)
            {
                result.Columns.Add(field.Name, field.FieldType);
            }

            foreach (T item in @this)
            {
                DataRow dr = result.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    dr[property.Name] = property.GetValue(item, null);
                }

                foreach (FieldInfo field in fields)
                {
                    dr[field.Name] = field.GetValue(item);
                }

                result.Rows.Add(dr);
            }

            return result;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable result = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                result.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                result.Rows.Add(values);
            }

            return result;
        }

        public static T CreateInstance<T>(this Type @this, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture)
        {
            return (T)Activator.CreateInstance(@this, bindingAttr, binder, args, culture);
        }

        public static T CreateInstance<T>(this Type @this, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture, Object[] activationAttributes)
        {
            return (T)Activator.CreateInstance(@this, bindingAttr, binder, args, culture, activationAttributes);
        }

        public static T CreateInstance<T>(this Type @this, Object[] args)
        {
            return (T)Activator.CreateInstance(@this, args);
        }

        public static T CreateInstance<T>(this Type @this, Object[] args, Object[] activationAttributes)
        {
            return (T)Activator.CreateInstance(@this, args, activationAttributes);
        }

        public static T CreateInstance<T>(this Type @this)
        {
            return (T)Activator.CreateInstance(@this);
        }

        public static T CreateInstance<T>(this Type @this, Boolean nonPublic)
        {
            return (T)Activator.CreateInstance(@this, nonPublic);
        }

        public static MethodInfo GetPublicInstanceMethod(this Type @this, string name, Type[] types)
        {
            return @this.GetMethod(name, BindingFlags.Instance | BindingFlags.Public, null, types, null);
        }

        public static TypeCode GetTypeCode(Type @this)
        {
            return Type.GetTypeCode(@this);
        }

        public static bool IsEnum(this Type @this)
        {
            return @this.IsEnum;
        }

        public static bool IsGenericType(this Type @this)
        {
            return @this.IsGenericType;
        }

        public static bool IsInterface(this Type @this)
        {
            return @this.IsInterface;
        }

        public static bool IsValueType(this Type @this)
        {
            return @this.IsValueType;
        }

        public static string Name(this Type @this)
        {
            return @this.Name;
        }
    }
}
