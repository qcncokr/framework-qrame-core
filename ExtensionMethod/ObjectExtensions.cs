using System.IO;
using System.Reflection;
using System.Drawing;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Qrame.CoreFX.ExtensionMethod
{
	public static class ObjectExtensions
	{
		public static T Chain<T>(this T @this, Action<T> action)
		{
			action(@this);

			return @this;
		}

		public static T DeepClone<T>(this T @this)
		{
			IFormatter formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, @this);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}

		public static T ShallowCopy<T>(this T @this)
		{
			MethodInfo method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
			return (T)method.Invoke(@this, null);
		}

		public static string ToStringSafe(this object @this)
		{
			return @this == null ? "" : @this.ToString();
		}

		public static object To(this object @this, Type type)
		{
			if (@this != null)
			{
				Type targetType = type;

				if (@this.GetType() == targetType)
				{
					return @this;
				}

				TypeConverter converter = TypeDescriptor.GetConverter(@this);
				if (converter != null)
				{
					if (converter.CanConvertTo(targetType))
					{
						return converter.ConvertTo(@this, targetType);
					}
				}

				converter = TypeDescriptor.GetConverter(targetType);
				if (converter != null)
				{
					if (converter.CanConvertFrom(@this.GetType()))
					{
						return converter.ConvertFrom(@this);
					}
				}

				if (@this == DBNull.Value)
				{
					return null;
				}
			}

			return @this;
		}

		public static T To<T>(this object @this)
		{
			if (@this != null)
			{
				Type targetType = typeof(T);

				if (@this.GetType() == targetType)
				{
					return (T)@this;
				}

				TypeConverter converter = TypeDescriptor.GetConverter(@this);
				if (converter != null)
				{
					if (converter.CanConvertTo(targetType))
					{
						return (T)converter.ConvertTo(@this, targetType);
					}
				}

				converter = TypeDescriptor.GetConverter(targetType);
				if (converter != null)
				{
					if (converter.CanConvertFrom(@this.GetType()))
					{
						return (T)converter.ConvertFrom(@this);
					}
				}

				if (@this == DBNull.Value)
				{
					return (T)(object)null;
				}
			}

			return (T)@this;
		}

		public static bool IsAssignableFrom<T>(this object @this)
		{
			Type type = @this.GetType();
			return type.IsAssignableFrom(typeof(T));
		}

		public static bool IsAssignableFrom(this object @this, Type targetType)
		{
			Type type = @this.GetType();
			return type.IsAssignableFrom(targetType);
		}

		public static T As<T>(this object @this)
		{
			return (T)@this;
		}

		public static T AsOrDefault<T>(this object @this)
		{
			try
			{
				return (T)@this;
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public static T AsOrDefault<T>(this object @this, T defaultValue)
		{
			try
			{
				return (T)@this;
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		public static T AsOrDefault<T>(this object @this, Func<T> defaultValueFactory)
		{
			try
			{
				return (T)@this;
			}
			catch (Exception)
			{
				return defaultValueFactory();
			}
		}

		public static T AsOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
		{
			try
			{
				return (T)@this;
			}
			catch (Exception)
			{
				return defaultValueFactory(@this);
			}
		}

		public static string SerializeBinary<T>(this T @this)
		{
			var binaryWrite = new BinaryFormatter();

			using (var memoryStream = new MemoryStream())
			{
				binaryWrite.Serialize(memoryStream, @this);
				return Encoding.Default.GetString(memoryStream.ToArray());
			}
		}

		public static string SerializeBinary<T>(this T @this, Encoding encoding)
		{
			var binaryWrite = new BinaryFormatter();

			using (var memoryStream = new MemoryStream())
			{
				binaryWrite.Serialize(memoryStream, @this);
				return encoding.GetString(memoryStream.ToArray());
			}
		}

		public static string SerializeJavaScript<T>(this T @this)
		{
			return JsonConvert.SerializeObject(@this);
		}

		public static string SerializeJson<T>(this T @this)
		{
			var serializer = new DataContractJsonSerializer(typeof(T));

			using (var memoryStream = new MemoryStream())
			{
				serializer.WriteObject(memoryStream, @this);
				return Encoding.Default.GetString(memoryStream.ToArray());
			}
		}

		public static string SerializeJson<T>(this T @this, Encoding encoding)
		{
			var serializer = new DataContractJsonSerializer(typeof(T));

			using (var memoryStream = new MemoryStream())
			{
				serializer.WriteObject(memoryStream, @this);
				return encoding.GetString(memoryStream.ToArray());
			}
		}

		public static string SerializeXml(this object @this)
		{
			var xmlSerializer = new XmlSerializer(@this.GetType());

			using (var stringWriter = new StringWriter())
			{
				xmlSerializer.Serialize(stringWriter, @this);
				using (var streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
	}
}
