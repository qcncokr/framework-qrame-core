using System;
using System.Collections.Generic;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Number 데이터 타입을 대상으로 동작하는 확장 메서드 클래스입니다.
    /// 
    /// 주요 기능으로 다음과 같습니다.
    /// </summary>
    /// <code>
    /// Format for Number 레퍼런스
    /// String.Format("{0:00000}", 15);                        // "00015"
    /// String.Format("{0:00000}", -15);                       // "-00015"
    /// String.Format("{0,5}", 15);                            // "   15"
    /// String.Format("{0,-5}", 15);                           // "15   "
    /// String.Format("{0,5:000}", 15);                        // "  015"
    /// String.Format("{0,-5:000}", 15);                       // "015  "
    /// String.Format("{0:#;minus #}", 15);                    // "15"
    /// String.Format("{0:#;minus #}", -15);                   // "minus 15"
    /// String.Format("{0:#;minus #;zero}", 0);                // "zero"
    /// String.Format("{0:+### ### ### ###}", 447900123456);   // "+447 900 123 456"
    /// String.Format("{0:##-####-####}", 8958712551);         // "89-5871-2551"
    /// String.Format("{0:0.00}", 123.4567);                   // "123.46"
    /// String.Format("{0:0.00}", 123.4);                      // "123.40"
    /// String.Format("{0:0.00}", 123.0);                      // "123.00"
    /// String.Format("{0:0.##}", 123.4567);                   // "123.46"
    /// String.Format("{0:0.##}", 123.4);                      // "123.4"
    /// String.Format("{0:0.##}", 123.0);                      // "123"
    /// String.Format("{0:00.0}", 123.4567);                   // "123.5"
    /// String.Format("{0:00.0}", 23.4567);                    // "23.5"
    /// String.Format("{0:00.0}", 3.4567);                     // "03.5"
    /// String.Format("{0:00.0}", -3.4567);                    // "-03.5"
    /// String.Format("{0:0,0.0}", 12345.67);                  // "12,345.7"
    /// String.Format("{0:0,0}", 12345.67);                    // "12,346"
    /// String.Format("{0:0.0}", 0.0);                         // "0.0"
    /// String.Format("{0:0.#}", 0.0);                         // "0"
    /// String.Format("{0:#.0}", 0.0);                         // ".0"
    /// String.Format("{0:#.#}", 0.0);                         // ""
    /// String.Format("{0,10:0.0}", 123.4567);                 // "     123.5"
    /// String.Format("{0,-10:0.0}", 123.4567);                // "123.5     "
    /// String.Format("{0,10:0.0}", -123.4567);                // "    -123.5"
    /// String.Format("{0,-10:0.0}", -123.4567);               // "-123.5    "
    /// String.Format("{0:0.00;minus 0.00;zero}", 123.4567);   // "123.46"
    /// String.Format("{0:0.00;minus 0.00;zero}", -123.4567);  // "minus 123.46"
    /// String.Format("{0:0.00;minus 0.00;zero}", 0.0);        // "zero"
    /// String.Format("{0:my number is 0.0}", 12.3);           // "my number is 12.3"
    /// String.Format("{0:0aaa.bbb0}", 12.3);                  // "12aaa.bbb3"
    /// </code>
    public static class NumberExtensions
    {
        /// <summary>
        /// 16비트의 정수값을 바이트 정수값으로 변환합니다.
        /// </summary>
        /// <param name="@this">부호 없는 16비트 정수를 나타냅니다.</param>
        /// <returns>부호 없는 16비트 정수를 나타냅니다.</returns>
        public static UInt16 ReverseBytes(this UInt16 @this)
        {
            return (UInt16)((@this & 0xFFU) << 8 | (@this & 0xFF00U) >> 8);
        }

        /// <summary>
        /// 32비트의 정수값을 바이트 정수값으로 변환합니다.
        /// </summary>
        /// <param name="@this">부호 없는 32비트 정수를 나타냅니다.</param>
        /// <returns>부호 없는 32비트 정수를 나타냅니다.</returns>ㅕ
        public static UInt32 ReverseBytes(this UInt32 @this)
        {
            return (@this & 0x000000FFU) << 24 | (@this & 0x0000FF00U) << 8 |
                   (@this & 0x00FF0000U) >> 8 | (@this & 0xFF000000U) >> 24;
        }

        /// <summary>
        /// 64비트의 정수값을 바이트 정수값으로 변환합니다.
        /// </summary>
        /// <param name="@this">부호 없는 64비트 정수를 나타냅니다.</param>
        /// <returns>부호 없는 64비트 정수를 나타냅니다.</returns>
        public static UInt64 ReverseBytes(this UInt64 @this)
        {
            return (@this & 0x00000000000000FFUL) << 56 | (@this & 0x000000000000FF00UL) << 40 |
                   (@this & 0x0000000000FF0000UL) << 24 | (@this & 0x00000000FF000000UL) << 8 |
                   (@this & 0x000000FF00000000UL) >> 8 | (@this & 0x0000FF0000000000UL) >> 24 |
                   (@this & 0x00FF000000000000UL) >> 40 | (@this & 0xFF00000000000000UL) >> 56;
        }

        /// <summary>
        /// 현재 숫자값이 최소값, 최대값의 지정한 범위에 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="@this">지정한 범위에 포함되어 있는지 확인 할 숫자값입니다.</param>
        /// <param name="minimumValue">최소 범위값입니다.</param>
        /// <param name="maximumValue">최대 범위값입니다.</param>
        /// <returns>현재 숫자값이 지정한 범위에 포함되어 있으면 true를, 아니면 false를 반환합니다.
        /// </returns>
        /// <code>
        /// int @this = 5;
        /// if(@this.IsBetween(1, 10)) { 
        ///     // ... 
        /// }
        /// </code>
        public static bool IsBetween(this int @this, int minimumValue, int maximumValue)
        {
            return (minimumValue <= @this) && (maximumValue >= @this);
        }

        /// <summary>
        /// 현재 숫자값이 최소값, 최대값의 지정한 범위에 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="@this">지정한 범위에 포함되어 있는지 확인 할 숫자값입니다.</param>
        /// <param name="minimumValue">최소 범위값입니다.</param>
        /// <param name="maximumValue">최대 범위값입니다.</param>
        /// <returns>현재 숫자값이 지정한 범위에 포함되어 있으면 true를, 아니면 false를 반환합니다.
        /// </returns>
        /// <code>
        /// long @this = 5;
        /// if(@this.IsBetween(1, 10)) { 
        ///     // ... 
        /// }
        /// </code>
        public static bool IsBetween(this long @this, long minimumValue, long maximumValue)
        {
            return (minimumValue <= @this) && (maximumValue >= @this);
        }

        /// <summary>
        /// 현재 숫자값이 최소값, 최대값의 지정한 범위에 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="@this">지정한 범위에 포함되어 있는지 확인 할 숫자값입니다.</param>
        /// <param name="minimumValue">최소 범위값입니다.</param>
        /// <param name="maximumValue">최대 범위값입니다.</param>
        /// <returns>현재 숫자값이 지정한 범위에 포함되어 있으면 true를, 아니면 false를 반환합니다.
        /// </returns>
        /// <code>
        /// double @this = 5;
        /// if(@this.IsBetween(1, 10)) { 
        ///     // ... 
        /// }
        /// </code>
        public static bool IsBetween(this double @this, double minimumValue, double maximumValue)
        {
            return (minimumValue <= @this) && (maximumValue >= @this);
        }

        /// <summary>
        /// 현재 숫자값이 최소값, 최대값의 지정한 범위에 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="@this">지정한 범위에 포함되어 있는지 확인 할 숫자값입니다.</param>
        /// <param name="minimumValue">최소 범위값입니다.</param>
        /// <param name="maximumValue">최대 범위값입니다.</param>
        /// <returns>현재 숫자값이 지정한 범위에 포함되어 있으면 true를, 아니면 false를 반환합니다.
        /// </returns>
        /// <code>
        /// decimal @this = 5;
        /// if(@this.IsBetween(1, 10)) { 
        ///     // ... 
        /// }
        /// </code>
        public static bool IsBetween(this decimal @this, decimal minimumValue, decimal maximumValue)
        {
            return (minimumValue <= @this) && (maximumValue >= @this);
        }

        /// <summary>
        /// 현재 숫자값이 최소값, 최대값의 지정한 범위에 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="@this">지정한 범위에 포함되어 있는지 확인 할 숫자값입니다.</param>
        /// <param name="minimumValue">최소 범위값입니다.</param>
        /// <param name="maximumValue">최대 범위값입니다.</param>
        /// <returns>현재 숫자값이 지정한 범위에 포함되어 있으면 true를, 아니면 false를 반환합니다.
        /// </returns>
        /// <code>
        /// decimal @this = 5;
        /// if(@this.IsBetween(1, 10)) { 
        ///     // ... 
        /// }
        /// </code>
        public static bool IsBetween(this float @this, float minimumValue, float maximumValue)
        {
            return (minimumValue <= @this) && (maximumValue >= @this);
        }

        /// <summary>
        /// 현재 숫자값이 100%를 기준으로 totalValue값에 몇 %인지 계산합니다.
        /// </summary>
        /// <param name="@this">%를 계산할 숫자값입니다.</param>
        /// <param name="totalValue">현재 숫자값이 100%를 기준으로 몇 %인지 계산하기 위한 기준값입니다.</param>
        /// <returns>100%를 기준으로 몇 %인지 계산된 값입니다.</returns>
        public static int PercentageOf(this int @this, int totalValue)
        {
            return Convert.ToInt32(@this * 100 / totalValue);
        }

        /// <summary>
        /// 현재 숫자값이 100%를 기준으로 totalValue값에 몇 %인지 계산합니다.
        /// </summary>
        /// <param name="@this">%를 계산할 숫자값입니다.</param>
        /// <param name="totalValue">현재 숫자값이 100%를 기준으로 몇 %인지 계산하기 위한 기준값입니다.</param>
        /// <returns>100%를 기준으로 몇 %인지 계산된 값입니다.</returns>
        public static long PercentageOf(this long @this, long totalValue)
        {
            return Convert.ToInt64(@this * 100 / totalValue);
        }

        /// <summary>
        /// 현재의 수를 100%를 기준으로 몇%인지 계산
        /// </summary>
        /// <param name="@this">비교값</param>
        /// <param name="totalValue">총값</param>
        /// <returns>Percentage 값</returns>
        public static double PercentageOf(this double @this, double totalValue)
        {
            return Convert.ToDouble(@this * 100 / totalValue);
        }

        /// <summary>
        /// 현재 숫자값이 100%를 기준으로 totalValue값에 몇 %인지 계산합니다.
        /// </summary>
        /// <param name="@this">%를 계산할 숫자값입니다.</param>
        /// <param name="totalValue">현재 숫자값이 100%를 기준으로 몇 %인지 계산하기 위한 기준값입니다.</param>
        /// <returns>100%를 기준으로 몇 %인지 계산된 값입니다.</returns>
        public static decimal PercentageOf(this decimal @this, decimal totalValue)
        {
            return Convert.ToDecimal(@this * 100 / totalValue);
        }

        /// <summary>
        /// 현재 숫자값이 100%를 기준으로 totalValue값에 몇 %인지 계산합니다.
        /// </summary>
        /// <param name="@this">%를 계산할 숫자값입니다.</param>
        /// <param name="totalValue">현재 숫자값이 100%를 기준으로 몇 %인지 계산하기 위한 기준값입니다.</param>
        /// <returns>100%를 기준으로 몇 %인지 계산된 값입니다.</returns>
        public static float PercentageOf(this float @this, float totalValue)
        {
            return Convert.ToSingle(@this * 100 / totalValue);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this int @this)
        {
            return string.Format("{0:N0}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this long @this)
        {
            return string.Format("{0:N0}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this decimal @this)
        {
            return string.Format("{0:N0}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <param name="digits">소숫점 자리수에 해당하는 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this decimal @this, int digits)
        {
            return string.Format("{0:N" + digits.ToString() + "}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this double @this)
        {
            return string.Format("{0:N0}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <param name="digits">소숫점 자리수에 해당하는 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this double @this, int digits)
        {
            return string.Format("{0:N" + digits.ToString() + "}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this float @this)
        {
            return string.Format("{0:N0}", @this);
        }

        /// <summary>
        /// 현재 숫자값에 통화 서식을 적용하여 문자열로 반환합니다.
        /// </summary>
        /// <param name="@this">통화 서식을 적용하여 문자열로 반환 할 숫자값입니다.</param>
        /// <param name="digits">소숫점 자리수에 해당하는 숫자값입니다.</param>
        /// <returns>통화 서식이 적용된 문자열입니다.</returns>
        public static string ToCurrencyString(this float @this, int digits)
        {
            return string.Format("{0:N" + digits.ToString() + "}", @this);
        }
    }
}
