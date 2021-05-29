using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Qrame.CoreFX.ExtensionMethod
{
	/// <summary>
	/// String 데이터 타입을 대상으로 동작하는 확장 메서드 클래스입니다.
	/// 
	/// 주요 기능으로 다음과 같습니다.
	/// </summary>
	public static class StringExtensions
	{
		public static string ToJoin<T>(this IEnumerable<T> @this, string separator)
		{
			return string.Join(separator, @this);
		}

		public static string ToJoin<T>(this IEnumerable<T> @this, char separator)
		{
			return string.Join(separator.ToString(), @this);
		}

		/// <summary>
		/// String을 바이트 배열로 변환 합니다.
		/// </summary>      
		public static byte[] ToByte(this string @this, Encoding encoding)
		{
			return encoding.GetBytes(@this);
		}

		/// <summary>
		/// uniqueSize에 지정된 길이에 맞는 유일 값을 반환합니다. uniqueSize가 작을 수록 중복 값을 생성할 수 있습니다.
		/// 권장 uniqueSize 값은 8 입니다. 최대 uniqueSize는 16자리입니다.
		/// </summary>
		/// <param name="uniqueSize">유일 값을 생성할 자리 수입니다.</param>
		/// <returns>uniqueSize에 지정된 길이에 맞는 유일 값입니다.</returns>
		public static string GenerateUniqueId(int uniqueSize)
		{
			string chars = "abcdefghijkmnopqrstuvwxyz1234567890";
			StringBuilder sb = new StringBuilder(uniqueSize);

			int count = 0;
			foreach (byte b in Guid.NewGuid().ToByteArray())
			{
				sb.Append(chars[b % (chars.Length - 1)]);

				count++;

				if (count >= uniqueSize)
				{
					return sb.ToString();
				}
			}

			return sb.ToString();
		}


		/// <summary>
		/// 8자리 길이에 맞는 유일 값을 반환합니다.
		/// </summary>
		/// <returns>8자리 길이에 맞는 유일 값입니다.</returns>
		public static string GenerateUniqueId()
		{
			return GenerateUniqueId(8);
		}

		/// <summary>
		/// Guid.NewGuid()의 결과를 long 타입으로 반환합니다.
		/// </summary>
		/// <returns>Guid의 정수값입니다.</returns>
		public static long GenerateUniqueNumericId()
		{
			byte[] bytes = Guid.NewGuid().ToByteArray();
			return BitConverter.ToInt64(bytes, 0);
		}

		/// <summary>
		/// System.String에서 시작 문자와 종료 문자 사이의 문자열을 반환합니다.
		/// </summary>
		/// <param name="@this">문자 사이의 문자열을 반환하기 위한 문자값입니다.</param>
		/// <param name="startChar">문자열을 자르기 위한 시작 문자값 입니다.</param>
		/// <param name="endChar">문자열을 자르기 위한 종료 문자값 입니다.</param>
		/// <returns>시작 문자와 종료 문자 사이의 문자열입니다. 시작 문자나 종료 문자가 검색 되지 않을 경우 <c>빈 값</c>을 반환합니다.</returns>
		public static string Between(this string @this, char startChar, char endChar)
		{
			string Result = "";
			int StartIndex = @this.IndexOf(startChar);

			if (StartIndex != -1)
			{
				++StartIndex;
				int EndIndex = @this.IndexOf(endChar, StartIndex);
				if (EndIndex != -1)
				{
					Result = @this.Substring(StartIndex, EndIndex - StartIndex);
				}
			}

			return Result;
		}

		/// <summary>
		/// System.String에서 지정한 문자를 포함하는 개수를 반환합니다.
		/// </summary>
		/// <param name="@this">지정한 문자를 포함하는 개수를 반환하기 위한 문자값입니다.</param>
		/// <param name="searchChar">포함하는 개수를 반환하기 위한 지정 문자입니다.</param>
		/// <returns>지정한 문자를 포함하는 개수입니다.</returns>
		public static int Count(this string @this, char searchChar)
		{
			int Result = 0;
			foreach (char CharValue in @this)
			{
				if (CharValue == searchChar)
				{
					++Result;
				}
			}

			return Result;
		}

		/// <summary>
		/// System.String에서 숫자형 데이터 타입으로 변환 할 수 있는지 확인합니다.
		/// </summary>
		/// <param name="@this">숫자형 데이터 타입으로 변환 할 수 있는지 확인하기 위한 문자값입니다.</param>
		/// <returns>숫자형 데이터 타입으로 변환 가능 여부입니다.</returns>
		public static bool IsNumeric(this string @this)
		{
			float output;
			return float.TryParse(@this, out output);
		}

		public static bool IsNullOrEmpty(this string @this)
		{
			return string.IsNullOrEmpty(@this);
		}

		/// <summary>
		/// 문자열의 지정된 두 인스턴스를 연결합니다.
		/// </summary>
		/// <param name="@this">연결할 문자열입니다.</param>
		/// <param name="concatValues">연결할 문자 배열입니다.</param>
		/// <returns>연결된 @this 및 concatValues을 반환합니다.</returns>
		public static string ConcatWith(this string @this, params string[] concatValues)
		{
			return string.Concat(@this, string.Concat(concatValues));
		}

		/// <summary>
		/// System.String에서 왼쪽에서 부터 지정된 길이 만큼 나누어 반환합니다.
		/// </summary>
		/// <param name="@this">왼쪽에서 부터 지정된 길이 만큼 나눌 문자열입니다.</param>
		/// <param name="length">문자열을 나눌 지정 길이 값입니다.</param>
		/// <returns>문자열을 왼쪽에서 부터 지정된 길이 만큼 나눈 문자열입니다.</returns>
		public static string Left(this string @this, int length)
		{
			return Left(@this, length, true);
		}

		/// <summary>
		/// System.String에서 지정된 폰트 이름과 폰트 사이즈를 기준으로 SizeF 구조체를 반환합니다.
		/// </summary>
		/// <param name="@this">가로 길이와 세로 길이값을 구할 문자열입니다.</param>
		/// <param name="fontName">시스템에 설치된 폰트 명입니다.</param>
		/// <param name="emSize">폰트의 크기입니다.</param>
		/// <returns>보통 사각형의 너비와 높이의 순서로 정렬된 부동 소수점 숫자 쌍을 저장합니다.</returns>
		public static SizeF TextWidth(this string @this, string fontName, float emSize)
		{
			using (Image DummyImage = new Bitmap(1, 1))
			{
				Graphics DummyGraphics = Graphics.FromImage(DummyImage);
				return DummyGraphics.MeasureString(@this, new Font(fontName, emSize));
			}
		}

		/// <summary>
		/// System.String에서 지정된 Encoding에 맞춰 왼쪽에서 부터 지정된 길이 만큼의 문자열을 반환합니다.
		/// </summary>
		/// <param name="@this">왼쪽에서 부터 지정된 길이 만큼 나눌 문자열입니다.</param>
		/// <param name="length">문자열을 나눌 지정 길이 값입니다.</param>
		/// <param name="isAscii">Ascii 코드의 길이로 계산할 지 여부입니다.</param>
		/// <returns>문자열을 왼쪽에서 부터 지정된 길이 만큼 나눈 문자열입니다.</returns>
		public static string Left(this string @this, int length, bool isAscii)
		{
			if (string.IsNullOrEmpty(@this) || length > @this.Length || length < 0)
			{
				return @this;
			}

			if (isAscii == false)
			{
				return @this.Substring(0, length);
			}
			else
			{
				Byte[] UTF8Bytes = Encoding.UTF8.GetBytes(@this);
				Byte[] ConvertBytes = Encoding.Convert(Encoding.UTF8, Encoding.Default, UTF8Bytes);

				if (ConvertBytes.Length < length)
				{
					return Encoding.Default.GetString(ConvertBytes);
				}
				else
				{
					return Encoding.Default.GetString(ConvertBytes, 0, length);
				}
			}
		}

		/// <summary>
		/// System.String에서 오른쪽에서 부터 지정된 길이 만큼 나누어 반환합니다.
		/// </summary>
		/// <param name="@this">오른쪽에서 부터 지정된 길이 만큼 나눌 문자열입니다.</param>
		/// <param name="length">문자열을 나눌 지정 길이 값입니다.</param>
		/// <returns>문자열을 오른쪽에서 부터 지정된 길이 만큼 나눈 문자열입니다.</returns>
		public static string Right(this string @this, int length)
		{
			if (string.IsNullOrEmpty(@this) || length > @this.Length || length < 0)
			{
				return @this;
			}

			return @this.Substring(@this.Length - length);
		}

		/// <summary>
		/// System.String에서 지정된 정규식이 지정된 입력 문자열에서 일치하는 항목을 찾을 것인지 여부를 나타냅니다.
		/// </summary>
		/// <param name="@this">일치 항목을 검색할 문자열입니다.</param>
		/// <param name="regexPattern">일치 항목을 찾을 정규식 패턴입니다.</param>
		/// <returns>정규식에서 일치하는 항목을 찾게 하려면 true이고, 그렇지 않으면 false입니다.</returns>
		public static bool IsMatch(this string @this, string regexPattern)
		{
			return (new Regex(regexPattern)).IsMatch(@this);
		}

		/// <summary>
		/// 지정된 유니코드 문자 배열의 요소로 구분된 이 인스턴스의 부분 문자열이 들어 있는 System.String 배열을 반환합니다.
		/// </summary>
		/// <param name="@this">구분 항목을 검색할 문자열입니다.</param>
		/// <param name="separator">이 인스턴스의 부분 문자열을 구분하는 유니코드 문자, 구분 기호를 포함하지 않는 빈 배열, 또는 null입니다.</param>
		/// <returns>해당 요소에 separator에 있는 하나 이상의 문자로 구분되는 이 인스턴스의 부분 문자열이 포함된 배열입니다.</returns>
		public static string[] Split(this string @this, string separator)
		{
			return @this.Split(separator.ToCharArray());
		}

		/// <summary>
		/// 정규식 패턴으로 Split을 수행하여 System.String 배열로 반환합니다, 정규식 옵션을 설정하는 데 사용하는 열거형 RegexOptions을 이용합니다.
		/// </summary>
		/// <param name="@this">구분 항목을 검색할 문자열입니다.</param>
		/// <param name="regexPattern">일치 항목을 찾을 정규식 패턴입니다.</param>
		/// <param name="patternOptions">정규식 옵션을 설정하는 데 사용하는 열거형 값입니다.</param>
		/// <returns>이 인스턴스의 부분 문자열이 포함된 배열입니다.</returns>
		public static string[] Split(this string @this, string regexPattern, RegexOptions patternOptions)
		{
			return Regex.Split(@this, regexPattern, patternOptions);
		}

		/// <summary>
		/// System.String의 모든 문자를 바이트 시퀀스로 인코딩합니다.
		/// </summary>
		/// <param name="@this">인코딩할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="encoding">문자 인코딩입니다.</param>
		/// <returns>바이트 시퀀스로 인코딩 값입니다.</returns>
		public static byte[] ToBytes(this string @this, Encoding encoding)
		{
			if (encoding == null)
			{
				encoding = Encoding.Default;
			}

			return encoding.GetBytes(@this);
		}

		/// <summary>
		/// System.String의 Hex 문자를 바이트 시퀀스로 인코딩합니다.
		/// </summary>
		/// <param name="hex">Hex 문자열입니다.</param>
		/// <returns>바이트 시퀀스 인코딩 값입니다.</returns>
		public static byte[] HexToBytes(this string hex)
		{
			byte[] bytes = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length / 2; i++)
			{
				string code = hex.Substring(i * 2, 2);
				bytes[i] = byte.Parse(code, System.Globalization.NumberStyles.HexNumber);
			}
			return bytes;
		}

		/// <summary>
		/// 바이트 시퀀스로 인코딩된 값을 문자열로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 시퀀스 인코딩 값입니다.</param>
		/// <returns>Hex 문자열입니다.</returns>
		public static string BytesToHex(this byte[] bytes)
		{
			StringBuilder hex = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				hex.AppendFormat("{0:X2}", bytes[i]);
			}
			return hex.ToString();
		}

		/// <summary>
		/// 지정된 유니코드 문자 배열의 요소로 구분된 이 인스턴스의 부분 문자열이 들어 있는 System.String Generic List를 반환합니다.
		/// </summary>
		/// <param name="@this">구분 항목을 검색할 문자열입니다.</param>
		/// <param name="separator">이 인스턴스의 부분 문자열을 구분하는 유니코드 문자, 구분 기호를 포함하지 않는 빈 배열, 또는 null입니다.</param>
		/// <returns>해당 요소에 separator에 있는 하나 이상의 문자로 구분되는 이 인스턴스의 부분 문자열이 포함된 System.String Generic List입니다.</returns>
		public static List<string> ToList(this string @this, string separator)
		{
			List<string> list = new List<string>();

			foreach (string e in @this.Split(separator.ToCharArray()))
			{
				list.Add(e.Trim());
			}

			return list;
		}

		/// <summary>
		/// 8비트 부호 없는 정수로 구성된 배열을 유니코드 문자의 UTF-8 인코딩을 이용하여, base-64 숫자로 인코딩된 해당하는 System.String 표현으로 변환합니다.
		/// </summary>
		/// <param name="@this">UTF-8 인코딩 System.String 표현으로 변환할 문자열입니다.</param>
		/// <returns>base 64 숫자의 System.String 표현입니다.</returns>
		public static string EncodeBase64(this string @this)
		{
			return Convert.ToBase64String(@this.ToBytes(Encoding.UTF8));
		}

		/// <summary>
		/// base-64 숫자로 인코딩된 해당하는 System.String 표현을 이용하여, 8비트 부호 없는 정수로 구성된 배열로 변환합니다.
		/// </summary>
		/// <param name="@this">8비트 부호 없는 정수로 구성된 배열로 변환할 문자열입니다.</param>
		/// <returns>일반 System.String 표현입니다.</returns>
		public static string DecodeBase64(this string @this)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(@this));
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 bool 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 bool 데이터 타입입니다.</returns>
		public static bool ParseBool(this string @this, bool defaultValue = false)
		{
			bool result = false;
			if (string.IsNullOrEmpty(@this) == true)
			{
				result = defaultValue;
			}
			else
			{
				result = (@this == "true" || @this == "True" || @this == "TRUE" || @this == "Y" || @this == "1");
			}

			return result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 DateTime 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 DateTime 데이터 타입입니다.</returns>
		public static DateTime? ParseDateTime(this string @this, DateTime? defaultValue = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None)
		{
			DateTime? result = null;

			Regex regex = new Regex(@"(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))");
			if (string.IsNullOrEmpty(@this) == true || regex.IsMatch(@this) == false)
			{
				result = defaultValue;
			}
			else
			{
				result = DateTime.Parse(@this, null, dateTimeStyles);
			}

			return result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 int 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 int 데이터 타입입니다.</returns>
		public static int ParseInt(this string @this, int defaultValue)
		{
			return ParseInt(@this, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 int 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <param name="numberFormat">형식 지정을 제어하는 개체를 검색하기 위한 인터페이스 구현입니다.</param>
		/// <returns>데이터 타입으로 변환된 int 데이터 타입입니다.</returns>
		public static int ParseInt(this string @this, int defaultValue, IFormatProvider numberFormat)
		{
			int Result = defaultValue;
			int.TryParse(@this, NumberStyles.Any, numberFormat, out Result);
			return Result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 long 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 long 데이터 타입입니다.</returns>
		public static long ParseLong(this string @this, long defaultValue)
		{
			return ParseLong(@this, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 long 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <param name="numberFormat">형식 지정을 제어하는 개체를 검색하기 위한 인터페이스 구현입니다.</param>
		/// <returns>데이터 타입으로 변환된 long 데이터 타입입니다.</returns>
		public static long ParseLong(this string @this, long defaultValue, IFormatProvider numberFormat)
		{
			long Result = defaultValue;
			long.TryParse(@this, NumberStyles.Any, numberFormat, out Result);
			return Result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 decimal 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 decimal 데이터 타입입니다.</returns>
		public static decimal ParseDecimal(this string @this, decimal defaultValue)
		{
			return ParseDecimal(@this, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 decimal 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <param name="numberFormat">형식 지정을 제어하는 개체를 검색하기 위한 인터페이스 구현입니다.</param>
		/// <returns>데이터 타입으로 변환된 decimal 데이터 타입입니다.</returns>
		public static decimal ParseDecimal(this string @this, decimal defaultValue, IFormatProvider numberFormat)
		{
			decimal Result = defaultValue;
			decimal.TryParse(@this, NumberStyles.Any, numberFormat, out Result);
			return Result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 double 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 double 데이터 타입입니다.</returns>
		public static double ParseDouble(this string @this, double defaultValue)
		{
			return ParseDouble(@this, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 double 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <param name="numberFormat">형식 지정을 제어하는 개체를 검색하기 위한 인터페이스 구현입니다.</param>
		/// <returns>데이터 타입으로 변환된 double 데이터 타입입니다.</returns>
		public static double ParseDouble(this string @this, double defaultValue, IFormatProvider numberFormat)
		{
			double Result = defaultValue;
			double.TryParse(@this, NumberStyles.Any, numberFormat, out Result);
			return Result;
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 float 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <returns>데이터 타입으로 변환된 float 데이터 타입입니다.</returns>
		public static float ParseFloat(this string @this, float defaultValue)
		{
			return ParseFloat(@this, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
		}

		/// <summary>
		/// 현재 System.String 표현을 해당하는 float 데이터 타입으로 변환합니다.
		/// </summary>
		/// <param name="@this">변환을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <param name="defaultValue">변환 작업 수행시 에러가 발생 할 경우 지정할 기본값입니다.</param>
		/// <param name="numberFormat">형식 지정을 제어하는 개체를 검색하기 위한 인터페이스 구현입니다.</param>
		/// <returns>데이터 타입으로 변환된 float 데이터 타입입니다.</returns>
		public static float ParseFloat(this string @this, float defaultValue, IFormatProvider numberFormat)
		{
			float Result = defaultValue;
			float.TryParse(@this, NumberStyles.Any, numberFormat, out Result);
			return Result;
		}

		/// <summary>
		/// 현재 System.String에서 숫자로 되어있는 단어를 추출합니다. 소수점 자리수가 제외 되니 정수형 데이터 타입에 적합합니다.
		/// </summary>
		/// <param name="@this">숫자추출을 수행할 문자가 들어 있는 System.String입니다.</param>
		/// <returns>숫자값으로 추출된 문자열입니다.</returns>
		public static string ToNumeric(this string @this)
		{
			if (string.IsNullOrEmpty(@this) == false)
			{
				char[] Result = new char[@this.Length];
				int i = 0;

				foreach (char Character in @this)
				{
					if (char.IsNumber(Character))
					{
						Result[i++] = Character;
					}
				}

				if (0 == i)
				{
					@this = "";
				}
				else if (Result.Length != i)
				{
					@this = new string(Result, 0, i);
				}
			}
			return @this;
		}

		/// <summary>
		/// 현재 System.String에서 지정된 단어 수를 반환합니다.
		/// </summary>
		/// <param name="@this">System.String입니다.</param>
		/// <param name="pattern">수를 셀 단어입니다.</param>
		/// <returns>int 데이터 타입을 나타냅니다.</returns>
		public static int ToCount(this string @this, string pattern)
		{
			int count = 0;
			int i = 0;
			while ((i = @this.IndexOf(pattern, i)) != -1)
			{
				i += pattern.Length;
				count++;
			}
			return count;
		}

		/// <summary>
		/// 현재 System.String을 bool 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">bool 타입으로 반환할 System.String입니다.</param>
		/// <returns>bool 데ㅕ이터 타입을 나타냅니다.</returns>
		public static bool ToBoolean(this string @this)
		{
			return Reflector.StringToTypedValue<bool>(@this);
		}

		/// <summary>
		/// 현재 System.String을 short 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">short 타입으로 반환할 System.String입니다.</param>
		/// <returns>short 데이터 타입을 나타냅니다.</returns>
		public static short ToShort(this string @this)
		{
			return Reflector.StringToTypedValue<short>(@this);
		}

		/// <summary>
		/// 현재 System.String을 int 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">int 타입으로 반환할 System.String입니다.</param>
		/// <returns>int 데이터 타입을 나타냅니다.</returns>
		public static int ToInt(this string @this)
		{
			return Reflector.StringToTypedValue<int>(@this);
		}

		/// <summary>
		/// 현재 System.String을 long 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">long 타입으로 반환할 System.String입니다.</param>
		/// <returns>long 데이터 타입을 나타냅니다.</returns>
		public static long ToLong(this string @this)
		{
			return Reflector.StringToTypedValue<long>(@this);
		}

		/// <summary>
		/// 현재 System.String을 decimal 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">decimal 타입으로 반환할 System.String입니다.</param>
		/// <returns>decimal 데이터 타입을 나타냅니다.</returns>
		public static decimal ToDecimal(this string @this)
		{
			return Reflector.StringToTypedValue<decimal>(@this);
		}

		/// <summary>
		/// 현재 System.String을 float 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">float 타입으로 반환할 System.String입니다.</param>
		/// <returns>float 데이터 타입을 나타냅니다.</returns>
		public static float ToFloat(this string @this)
		{
			return Reflector.StringToTypedValue<float>(@this);
		}

		/// <summary>
		/// 현재 System.String을 double 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">double 타입으로 반환할 System.String입니다.</param>
		/// <returns>double 데이터 타입을 나타냅니다.</returns>
		public static double ToDouble(this string @this)
		{
			return Reflector.StringToTypedValue<double>(@this);
		}

		/// <summary>
		/// 현재 System.String을 DateTime 타입으로 반환합니다.
		/// </summary>
		/// <param name="@this">DateTime 타입으로 반환할 System.String입니다.</param>
		/// <param name="dateFormat">변환된 DateTime에 적용할 형식 구조입니다.올바른 형식에는 "yyyy-MM-ddTHH:mm:sszzzzzz" 및 그 하위 집합이 포함됩니다.</param>
		/// <returns>DateTime 데이터 타입을 나타냅니다.</returns>
		public static DateTime ToDateTime(this string @this, string dateFormat)
		{
			return XmlConvert.ToDateTime(@this, dateFormat);
		}

		/// <summary>
		/// 지정된 입력 문자열 내에서 지정된 정규식과 일치하는 모든 문자열을 지정된 대체 문자열로 바꿉니다.지정한 옵션에 따라 일치 작업이 수정됩니다.
		/// </summary>
		/// <param name="@this">일치 항목을 검색할 문자열입니다.</param>
		/// <param name="regexPattern">일치 항목을 찾을 정규식 패턴입니다.</param>
		/// <param name="replaceValue">대체 문자열입니다.</param>
		/// <param name="ignoreCase">대/소문자를 구분하지 않고 일치 항목을 찾도록 지정합니다.</param>
		/// <returns>입력 문자열과 동일한 새 문자열입니다. 단, 대체 문자열은 일치하는 각 문자열을 대체합니다.</returns>
		public static string Replace(this string @this, string regexPattern, string replaceValue, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(@this) == true)
			{
				return @this;
			}

			if (ignoreCase == true)
			{
				return Regex.Replace(@this, regexPattern, replaceValue, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			}
			else
			{
				return Regex.Replace(@this, regexPattern, replaceValue, RegexOptions.Compiled);
			}
		}

		/// <summary>
		/// 현재 System.String을 첫번째단어를 대문자로 변환하여 반환합니다.
		/// </summary>
		/// <param name="@this">CamelCase를 적용하여 반환할 System.String입니다.</param>
		/// <returns>CamelCase가 적용된 문자열 을 나타냅니다.</returns>
		public static string ToCamelCase(this string @this)
		{
			return @this.Substring(0, 1).ToLower() + @this.Substring(1);
		}

		public static string Format(this string format, object[] args)
		{
			return string.Format(format, args);
		}

		public static Match Match(this string @this, string pattern)
		{
			return Regex.Match(@this, pattern);
		}

		public static Match Match(this string @this, string pattern, RegexOptions options)
		{
			return Regex.Match(@this, pattern, options);
		}

		public static MatchCollection Matches(this string @this, string pattern)
		{
			return Regex.Matches(@this, pattern);
		}

		public static MatchCollection Matches(this string @this, string pattern, RegexOptions options)
		{
			return Regex.Matches(@this, pattern, options);
		}

		public static string toSHA256Hash(this string value)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public static string EncryptAES(this string value, string key)
		{
			var aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = new byte[16];

			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(value);
					cs.Write(bytes, 0, bytes.Length);
				}

				return Convert.ToBase64String(ms.ToArray());
			}
		}

		public static string DecryptAES(this string value, string key)
		{
			var aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = new byte[16];

			var decrypt = aes.CreateDecryptor();
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
				{
					byte[] bytes = Convert.FromBase64String(value);
					cs.Write(bytes, 0, bytes.Length);
				}

				return Encoding.UTF8.GetString(ms.ToArray());
			}
		}

		public static string EncryptRSA(this string @this, string key)
		{
			var cspp = new CspParameters { KeyContainerName = key };
			var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
			byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), true);

			return BitConverter.ToString(bytes);
		}

		public static string DecryptRSA(this string @this, string key)
		{
			var cspp = new CspParameters { KeyContainerName = key };
			var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
			string[] decryptArray = @this.Split(new[] { "-" }, StringSplitOptions.None);
			byte[] decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));
			byte[] bytes = rsa.Decrypt(decryptByteArray, true);

			return Encoding.UTF8.GetString(bytes);
		}

		public static string Truncate(this string @this, int maxLength)
		{
			const string suffix = "...";

			if (@this == null || @this.Length <= maxLength)
			{
				return @this;
			}

			int strLength = maxLength - suffix.Length;
			return @this.Substring(0, strLength) + suffix;
		}

		public static string Truncate(this string @this, int maxLength, string suffix)
		{
			if (@this == null || @this.Length <= maxLength)
			{
				return @this;
			}

			int strLength = maxLength - suffix.Length;
			return @this.Substring(0, strLength) + suffix;
		}

		public static StringBuilder AppendIf<T>(this StringBuilder @this, Func<T, bool> predicate, params T[] values)
		{
			foreach (var item in values)
			{
				if (predicate(item))
				{
					@this.Append(item);
				}
			}

			return @this;
		}

		public static StringBuilder AppendJoin<T>(this StringBuilder @this, string separator, IEnumerable<T> values)
		{
			@this.Append(string.Join(separator, values));

			return @this;
		}

		public static StringBuilder AppendJoin<T>(this StringBuilder @this, string separator, params T[] values)
		{
			@this.Append(string.Join(separator, values));

			return @this;
		}

		public static StringBuilder AppendLineFormat(this StringBuilder @this, string format, params object[] args)
		{
			@this.AppendLine(string.Format(format, args));

			return @this;
		}

		public static StringBuilder AppendLineFormat(this StringBuilder @this, string format, List<IEnumerable<object>> args)
		{
			@this.AppendLine(string.Format(format, args));

			return @this;
		}

		public static StringBuilder AppendLineJoin<T>(this StringBuilder @this, string separator, IEnumerable<T> values)
		{
			@this.AppendLine(string.Join(separator, values));

			return @this;
		}

		public static StringBuilder AppendLineJoin(this StringBuilder @this, string separator, params object[] values)
		{
			@this.AppendLine(string.Join(separator, values));

			return @this;
		}

		public static string Substring(this StringBuilder @this, int startIndex)
		{
			return @this.ToString(startIndex, @this.Length - startIndex);
		}

		public static string Substring(this StringBuilder @this, int startIndex, int length)
		{
			return @this.ToString(startIndex, length);
		}

		public static T DeserializeBinary<T>(this string @this)
		{
			using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
			{
				var binaryRead = new BinaryFormatter();
				return (T)binaryRead.Deserialize(stream);
			}
		}

		public static T DeserializeBinary<T>(this string @this, Encoding encoding)
		{
			using (var stream = new MemoryStream(encoding.GetBytes(@this)))
			{
				var binaryRead = new BinaryFormatter();
				return (T)binaryRead.Deserialize(stream);
			}
		}

		public static T DeserializeJavaScript<T>(this string @this)
		{
			return JsonConvert.DeserializeObject<T>(@this);
		}

		public static T DeserializeJson<T>(this string @this)
		{
			var serializer = new DataContractJsonSerializer(typeof(T));

			using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
			{
				return (T)serializer.ReadObject(stream);
			}
		}

		public static T DeserializeJson<T>(this string @this, Encoding encoding)
		{
			var serializer = new DataContractJsonSerializer(typeof(T));

			using (var stream = new MemoryStream(encoding.GetBytes(@this)))
			{
				return (T)serializer.ReadObject(stream);
			}
		}

		public static T DeserializeXml<T>(this string @this)
		{
			var x = new XmlSerializer(typeof(T));
			var r = new StringReader(@this);

			return (T)x.Deserialize(r);
		}
	}
}
