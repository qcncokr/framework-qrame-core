using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Qrame.CoreFX.ExtensionMethod
{
  /// <summary>
  /// DateTime 클래스를 대상으로 동작하는 확장 메서드 클래스입니다.
  /// 
  /// 주요 기능으로 다음과 같습니다.
  /// </summary>
  /// <code>
  /// Custom DateTime Formatting 레퍼런스
  /// // create @this @this 2008-03-09 16:05:07.123
  /// DateTime @this = new DateTime(2008, 3, 9, 16, 5, 7, 123);
  /// 
  /// String.Format("{0:y yy yyy yyyy}", @this);  // "8 08 008 2008"   year
  /// String.Format("{0:M MM MMM MMMM}", @this);  // "3 03 Mar March"  month
  /// String.Format("{0:d dd ddd dddd}", @this);  // "9 09 Sun Sunday" day
  /// String.Format("{0:h hh H HH}",     @this);  // "4 04 16 16"      hour 12/24
  /// String.Format("{0:m mm}",          @this);  // "5 05"            minute
  /// String.Format("{0:s ss}",          @this);  // "7 07"            second
  /// String.Format("{0:f ff fff ffff}", @this);  // "1 12 123 1230"   sec.fraction
  /// String.Format("{0:F FF FFF FFFF}", @this);  // "1 12 123 123"    without zeroes
  /// String.Format("{0:t tt}",          @this);  // "P PM"            A.M. or P.M.
  /// String.Format("{0:z zz zzz}",      @this);  // "-6 -06 -06:00"   @this zone
  /// 
  /// // @this separator in german culture is "." (so "/" changes to ".")
  /// String.Format("{0:d/M/yyyy HH:mm:ss}", @this); // "9/3/2008 16:05:07" - english (en-US)
  /// String.Format("{0:d/M/yyyy HH:mm:ss}", @this); // "9.3.2008 16:05:07" - german (de-DE)
  ///          
  /// // month/day numbers without/with leading zeroes
  /// String.Format("{0:M/d/yyyy}", @this);            // "3/9/2008"
  /// String.Format("{0:MM/dd/yyyy}", @this);          // "03/09/2008"
  /// 
  /// // day/month names
  /// String.Format("{0:ddd, MMM d, yyyy}", @this);    // "Sun, Mar 9, 2008"
  /// String.Format("{0:dddd, MMMM d, yyyy}", @this);  // "Sunday, March 9, 2008"
  /// 
  /// // two/four digit year
  /// String.Format("{0:MM/dd/yy}", @this);            // "03/09/08"
  /// String.Format("{0:MM/dd/yyyy}", @this);          // "03/09/2008"
  /// 
  /// Specifier	DateTimeFormatInfo property	Pattern value (for en-US culture)
  /// t	    ShortTimePattern	                h:mm tt
  /// d	    ShortDatePattern	                M/d/yyyy
  /// T	    LongTimePattern	                    h:mm:ss tt
  /// D	    LongDatePattern	                    dddd, MMMM dd, yyyy
  /// f	    (combination of D and t)	        dddd, MMMM dd, yyyy h:mm tt
  /// F	    FullDateTimePattern	                dddd, MMMM dd, yyyy h:mm:ss tt
  /// g	    (combination of d and t)	        M/d/yyyy h:mm tt
  /// G	    (combination of d and T)	        M/d/yyyy h:mm:ss tt
  /// m, M	MonthDayPattern	                    MMMM dd
  /// y, Y	YearMonthPattern	                MMMM, yyyy
  /// r, R	RFC1123Pattern	                    ddd, dd MMM yyyy HH':'mm':'ss 'GMT' (*)
  /// s	    SortableDateTi­mePattern	            yyyy'-'MM'-'dd'T'HH':'mm':'ss (*)
  /// u	    UniversalSorta­bleDateTimePat­tern	yyyy'-'MM'-'dd HH':'mm':'ss'Z' (*)
  ///          
  /// String.Format("{0:t}", @this);  // "4:05 PM"                         ShortTime
  /// String.Format("{0:d}", @this);  // "3/9/2008"                        ShortDate
  /// String.Format("{0:T}", @this);  // "4:05:07 PM"                      LongTime
  /// String.Format("{0:D}", @this);  // "Sunday, March 09, 2008"          LongDate
  /// String.Format("{0:f}", @this);  // "Sunday, March 09, 2008 4:05 PM"  LongDate+ShortTime
  /// String.Format("{0:F}", @this);  // "Sunday, March 09, 2008 4:05:07 PM" FullDateTime
  /// String.Format("{0:g}", @this);  // "3/9/2008 4:05 PM"                ShortDate+ShortTime
  /// String.Format("{0:G}", @this);  // "3/9/2008 4:05:07 PM"             ShortDate+LongTime
  /// String.Format("{0:m}", @this);  // "March 09"                        MonthDay
  /// String.Format("{0:y}", @this);  // "March, 2008"                     YearMonth
  /// String.Format("{0:r}", @this);  // "Sun, 09 Mar 2008 16:05:07 GMT"   RFC1123
  /// String.Format("{0:s}", @this);  // "2008-03-09T16:05:07"             SortableDateTime
  /// String.Format("{0:u}", @this);  // "2008-03-09 16:05:07Z"            UniversalSortableDateTime
  /// </code>
  public static class DateTimeExtensions
  {
    ///<summary>
    ///	현재 시스템의 시간과 UTC (Universal Time Coordinated 협정 세계시(時))과의 차이를 TimeSpan값으로 조회합니다.
    ///</summary>
    public static TimeSpan UtcOffset
    {
      get { return DateTime.Now.Subtract(DateTime.UtcNow); }
    }

    public static string TimeZone
    {
      get { return _timeZone; }
      set
      {
        TimeZoneInstance = null;
        _timeZone = value;
      }
    }
    private static string _timeZone;

    private static TimeZoneInfo _timeZoneInstance;
    public static TimeZoneInfo TimeZoneInstance
    {
      get
      {
        if (_timeZoneInstance == null)
        {
          try
          {
            _timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
          }
          catch
          {
            TimeZone = "Korea Standard Time";
            _timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
          }
        }
        return _timeZoneInstance;
      }
      private set { _timeZoneInstance = value; }
    }

    public static DateTime GetStandardDateTime(this DateTime? @this)
    {
      TimeZoneInfo tzi = null;

      if (@this == null)
      {
        @this = DateTime.UtcNow;
      }

      return TimeZoneInfo.ConvertTimeFromUtc(@this.Value, TimeZoneInstance);
    }

    public static DateTime GetStandardDateTime(this DateTime? @this, TimeZoneInfo timeZoneInfo)
    {
      TimeZoneInfo tzi = null;

      if (@this == null)
      {
        @this = DateTime.UtcNow;
      }

      return TimeZoneInfo.ConvertTimeFromUtc(@this.Value, timeZoneInfo);
    }

    public static DateTime GetUtcDateTime(this DateTime? @this)
    {
      if (@this == null)
      {
        @this = DateTime.Now;
      }

      return TimeZoneInfo.ConvertTime(@this.Value, TimeZoneInstance).ToUniversalTime();
    }

    public static DateTime GetUtcDateTime(this DateTime? @this, TimeZoneInfo timeZoneInfo)
    {
      if (@this == null)
      {
        @this = DateTime.Now;
      }

      return TimeZoneInfo.ConvertTime(@this.Value, timeZoneInfo).ToUniversalTime();
    }

    public static DateTime AdjustTimeZoneOffset(this DateTime @this, TimeZoneInfo tzi = null)
    {
      if (tzi == null)
      {
        tzi = TimeZoneInstance;
      }

      var offset = tzi.GetUtcOffset(@this).TotalHours;
      var offset2 = TimeZoneInfo.Local.GetUtcOffset(@this).TotalHours;
      return @this.AddHours(offset2 - offset);
    }

    /// <summary>
    /// "d" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">ShortDate를 반환할 DateTime 타입입니다.</param>
    /// <returns>"d" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToShortDate(this DateTime @this)
    {
      return ToShortDate(@this, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// "d" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 ShortDate를 반환합니다.
    /// </summary>
    /// <param name="@this">ShortDate를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 클래스의 문자열입니다.</param>
    /// <returns>"d" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToShortDate(this DateTime @this, string culture)
    {
      return ToShortDate(@this, new CultureInfo(culture));
    }

    /// <summary>
    /// "d" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 ShortDate를 반환합니다.
    /// </summary>
    /// <param name="@this">ShortDate를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 타입입니다.</param>
    /// <returns>string</returns>
    public static string ToShortDate(this DateTime @this, CultureInfo culture)
    {
      if (culture == null)
      {
        culture = CultureInfo.CurrentCulture;
      }

      return @this.ToString(culture.DateTimeFormat.ShortDatePattern, culture);
    }

    /// <summary>
    /// "D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">LongDate를 반환할 DateTime 타입입니다.</param>
    /// <returns>"D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongDate(this DateTime @this)
    {
      return ToLongDate(@this, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// "D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 LongDate를 반환합니다.
    /// </summary>
    /// <param name="@this">LongDate를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 클래스의 문자열입니다.</param>
    /// <returns>"D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongDate(this DateTime @this, string culture)
    {
      return ToLongDate(@this, new CultureInfo(culture));
    }

    /// <summary>
    /// "D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 LongDate를 반환합니다.
    /// </summary>
    /// <param name="@this">LongDate를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 타입입니다.</param>
    /// <returns>"D" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongDate(this DateTime @this, CultureInfo culture)
    {
      if (culture == null)
      {
        culture = CultureInfo.CurrentCulture;
      }

      return @this.ToString(culture.DateTimeFormat.LongDatePattern, culture);
    }

    /// <summary>
    /// "t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">ShortTime를 반환할 DateTime 타입입니다.</param>
    /// <returns>"t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToShortTime(this DateTime @this)
    {
      return ToShortTime(@this, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// "t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 ShortTime를 반환합니다.
    /// </summary>
    /// <param name="@this">ShortTime를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 클래스의 문자열입니다.</param>
    /// <returns>"t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToShortTime(this DateTime @this, string culture)
    {
      return ToShortTime(@this, new CultureInfo(culture));
    }

    /// <summary>
    /// "t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 ShortTime를 반환합니다.
    /// </summary>
    /// <param name="@this">ShortTime를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 타입입니다.</param>
    /// <returns>"t" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToShortTime(this DateTime @this, CultureInfo culture)
    {
      if (culture == null)
      {
        culture = CultureInfo.CurrentCulture;
      }

      return @this.ToString(culture.DateTimeFormat.ShortTimePattern, culture);
    }

    /// <summary>
    /// "T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">LongTime를 반환할 DateTime 타입입니다.</param>
    /// <returns>"T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongTime(this DateTime @this)
    {
      return ToLongTime(@this, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// "T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 LongTime를 반환합니다.
    /// </summary>
    /// <param name="@this">LongTime를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 클래스의 문자열입니다.</param>
    /// <returns>"T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongTime(this DateTime @this, string culture)
    {
      return ToLongTime(@this, new CultureInfo(culture));
    }

    /// <summary>
    /// "T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴을 조회합니다. CultureInfo 문자열에 따라 LongTime를 반환합니다.
    /// </summary>
    /// <param name="@this">LongTime를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 타입입니다.</param>
    /// <returns>"T" 형식 패턴과 관련된 간단한 날짜 값의 형식 패턴입니다.</returns>
    public static string ToLongTime(this DateTime @this, CultureInfo culture)
    {
      if (culture == null)
      {
        culture = CultureInfo.CurrentCulture;
      }

      return @this.ToString(culture.DateTimeFormat.LongTimePattern, culture);
    }

    /// <summary>
    /// datePart 일자 구분값에 따라 현재 DateTime의 값과 매개 변수 DateTime의 값의 시간의 차이를 반환합니다.
    /// </summary>
    /// <param name="@this">DateTime의 값의 시간의 차이를 반환할 시작 DateTime 타입입니다.</param>
    /// <param name="datePart">DateTime간의 시간의 차이를 구할 때 기준이 되는 열거형입니다.</param>
    /// <param name="endDate">DateTime의 값의 시간의 차이를 반환할 종료 DateTime 타입입니다.</param>
    /// <returns></returns>
    public static double DateDiff(this DateTime @this, PartOfDateTime datePart, DateTime endDate)
    {
      double Result = 0;

      TimeSpan SubtractDateTime = new TimeSpan(endDate.Ticks - @this.Ticks);

      switch (datePart)
      {
        case PartOfDateTime.Year:
          Result = endDate.Year - @this.Year;
          break;

        case PartOfDateTime.Quarter:
          double AvgQuarterDays = 365 / 4;
          Result = Math.Floor(SubtractDateTime.TotalDays / AvgQuarterDays);
          break;

        case PartOfDateTime.Month:
          double AvgMonthDays = 365 / 12;
          Result = Math.Floor(SubtractDateTime.TotalDays / AvgMonthDays);
          break;

        case PartOfDateTime.Day:
          Result = SubtractDateTime.TotalDays;
          break;

        case PartOfDateTime.Week:
          Result = Math.Floor(SubtractDateTime.TotalDays / 7);
          break;

        case PartOfDateTime.Hour:
          Result = SubtractDateTime.TotalHours;
          break;

        case PartOfDateTime.Minute:
          Result = SubtractDateTime.TotalMinutes;
          break;

        case PartOfDateTime.Second:
          Result = SubtractDateTime.TotalSeconds;
          break;

        case PartOfDateTime.Millisecond:
          Result = SubtractDateTime.TotalMilliseconds;
          break;

        default:
          throw new ArgumentException("검증되지 않는 PartOfDateTime 값입니다");
      }

      return Result;
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 현재 월의 첫번째 일을 반환합니다.
    /// </summary>
    /// <param name="@this">현재 월의 첫번째 일을 반환할 DateTime 타입입니다.</param>
    /// <returns>현재 월의 첫번째 일을 포함하는 DateTime 타입입니다.</returns>
    public static DateTime FirstDayOfMonth(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, 1, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 현재 월의 마지막 일을 반환합니다.
    /// </summary>
    /// <param name="@this">현재 월의 마지막 일을 반환할 DateTime 타입입니다.</param>
    /// <returns>현재 월의 마지막 일을 포함하는 DateTime 타입입니다.</returns>
    public static DateTime LastDayOfMonth(this DateTime @this)
    {
      return FirstDayOfMonth(@this).AddMonths(1).AddDays(-1);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 현재 주의 첫번째 일을 반환합니다.
    /// </summary>
    /// <param name="@this">현재 주의 첫번째 일을 반환할 DateTime 타입입니다.</param>
    /// <returns>현재 주의 첫번째 일을 포함하는 DateTime 타입입니다.</returns>
    public static DateTime FirstDayOfWeek(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond).AddDays(-(int)@this.DayOfWeek);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 현재 주의 마지막 일을 반환합니다.
    /// </summary>
    /// <param name="@this">현재 주의 마지막 일을 반환할 DateTime 타입입니다.</param>
    /// <returns>현재 주의 마지막 일을 포함하는 DateTime 타입입니다.</returns>
    public static DateTime LastDayOfWeek(this DateTime @this)
    {
      return FirstDayOfWeek(@this).AddDays(6);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 시간(시, 분, 초)을 재 설정 합니다.
    /// </summary>
    /// <param name="@this">시간(시, 분, 초)을 재 설정할 DateTime 타입입니다.</param>
    /// <param name="hours">양수 또는 음수 시간 간격을 나타내는 시(Hour) 입니다.</param>
    /// <param name="minutes">양수 또는 음수 시간 간격을 나타내는 분(minutes) 입니다.</param>
    /// <param name="seconds">양수 또는 음수 시간 간격을 나타내는 초(seconds) 입니다.</param>
    /// <returns>시간(시, 분, 초)이 재 설정된 DateTime을 반환합니다.</returns>
    public static DateTime SetTime(this DateTime @this, int hours, int minutes, int seconds)
    {
      return @this.SetTime(new TimeSpan(hours, minutes, seconds));
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 시간(시, 분, 초)을 TimeSpan 타입으로 재 설정 합니다
    /// 양수 또는 음수 시간 간격을 나타내는 System.TimeSpan 개체입니다.
    /// </summary>
    /// <param name="@this">시간(시, 분, 초)을 재 설정할 DateTime 타입입니다.</param>
    /// <param name="currentTimeSpan">시간(시, 분, 초)을 재 설정할 TimeSpan 타입입니다.</param>
    /// <returns>시간(시, 분, 초)이 재 설정된 DateTime을 반환합니다.</returns>
    public static DateTime SetTime(this DateTime @this, TimeSpan currentTimeSpan)
    {
      return @this.Date.Add(currentTimeSpan);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 오늘 날짜인지 확인합니다.
    /// </summary>
    /// <param name="@this">오늘 날짜인지 확인할 DateTime 타입입니다.</param>
    /// <returns>현재 DateTime 타입이 오늘 날짜이면 true를, 아니면 false를 반환합니다.</returns>
    public static bool IsToday(this DateTime @this)
    {
      return (@this.Date == DateTime.Today);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 주말 인지 확인합니다.
    /// </summary>
    /// <param name="@this">주말인지 확인할 DateTime 타입입니다.</param>
    /// <returns>현재 DateTime 타입이 주말이면 true를, 아니면 false를 반환합니다.</returns>
    public static bool IsWeekend(this DateTime @this)
    {
      return (@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 현재 주의 지정된 요일을 반환합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">지정된 요일을 반환할 DateTime 타입입니다.</param>
    /// <param name="weekday">요일을 가리킬 DayOfWeek 열거형입니다.</param>
    /// <returns>현재 주의 지정된 요일을 반환합니다.</returns>
    public static DateTime GetWeekday(this DateTime @this, DayOfWeek weekday)
    {
      return @this.FirstDayOfWeek().GetNextWeekday(weekday);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 지정된 요일에 맞는 DateTime을 반환합니다.
    /// </summary>
    /// <param name="@this">지정된 요일을 반환할 DateTime 타입입니다.</param>
    /// <param name="weekday">요일을 가리킬 DayOfWeek 열거형입니다.</param>
    /// <returns>현재 주의 지정된 요일을 반환합니다.</returns>
    private static DateTime GetNextWeekday(this DateTime @this, DayOfWeek weekday)
    {
      while (@this.DayOfWeek != weekday)
      {
        @this = @this.AddDays(1);
      }

      return @this;
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 해당 연도의 몇번째 주인지를 반환합니다. 현재 시스템의 문화권을 기반으로 하는 CultureInfo를 기본으로 사용합니다.
    /// </summary>
    /// <param name="@this">해당 연도의 몇번째 주인지를 반환할 DateTime 타입입니다.</param>
    /// <returns>해당 연도의 몇번째 주인지를 반환합니다.</returns>
    public static int GetWeekOfYear(this DateTime @this)
    {
      return GetWeekOfYear(@this, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 해당 연도의 몇번째 주인지를 반환합니다.
    /// </summary>
    /// <param name="@this">해당 연도의 몇번째 주인지를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 클래스의 문자열입니다.</param>
    /// <returns>해당 연도의 몇번째 주인지를 반환합니다.</returns>
    public static int GetWeekOfYear(this DateTime @this, string culture)
    {
      return GetWeekOfYear(@this, new CultureInfo(culture));
    }

    /// <summary>
    /// 현재 DateTime 클래스에서 해당 연도의 몇번째 주인지를 반환합니다.
    /// </summary>
    /// <param name="@this">해당 연도의 몇번째 주인지를 반환할 DateTime 타입입니다.</param>
    /// <param name="culture">문화권을 기반으로 하는 CultureInfo 타입입니다.</param>
    /// <returns>해당 연도의 몇번째 주인지를 반환합니다.</returns>
    public static int GetWeekOfYear(this DateTime @this, CultureInfo culture)
    {
      if (culture == null)
      {
        culture = CultureInfo.CurrentCulture;
      }

      CalendarWeekRule weekRule = culture.DateTimeFormat.CalendarWeekRule;
      DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
      return culture.Calendar.GetWeekOfYear(@this, weekRule, firstDayOfWeek);
    }

    public static int Age(this DateTime @this)
    {
      if (DateTime.Today.Month < @this.Month ||
          DateTime.Today.Month == @this.Month &&
          DateTime.Today.Day < @this.Day)
      {
        return DateTime.Today.Year - @this.Year - 1;
      }
      return DateTime.Today.Year - @this.Year;
    }

    public static DateTime Ago(this TimeSpan @this)
    {
      return DateTime.Now.Subtract(@this);
    }

    public static DateTime FromNow(this TimeSpan @this)
    {
      return DateTime.Now.Add(@this);
    }

    public static DateTime UtcAgo(this TimeSpan @this)
    {
      return DateTime.UtcNow.Subtract(@this);
    }

    public static DateTime UtcFromNow(this TimeSpan @this)
    {
      return DateTime.UtcNow.Add(@this);
    }

    public static TimeSpan Elapsed(this DateTime @this)
    {
      return DateTime.Now - @this;
    }

    public static DateTime EndOfDay(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime EndOfMonth(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, 1).AddMonths(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime EndOfWeek(this DateTime @this, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
    {
      DateTime end = @this;
      DayOfWeek endDayOfWeek = startDayOfWeek - 1;
      if (endDayOfWeek < 0)
      {
        endDayOfWeek = DayOfWeek.Saturday;
      }

      if (end.DayOfWeek != endDayOfWeek)
      {
        if (endDayOfWeek < end.DayOfWeek)
        {
          end = end.AddDays(7 - (end.DayOfWeek - endDayOfWeek));
        }
        else
        {
          end = end.AddDays(endDayOfWeek - end.DayOfWeek);
        }
      }

      return new DateTime(end.Year, end.Month, end.Day, 23, 59, 59, 999);
    }

    public static DateTime EndOfYear(this DateTime @this)
    {
      return new DateTime(@this.Year, 1, 1).AddYears(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static bool IsAfternoon(this DateTime @this)
    {
      return @this.TimeOfDay >= new DateTime(2000, 1, 1, 12, 0, 0).TimeOfDay;
    }

    public static bool IsDateEqual(this DateTime @this, DateTime dateToCompare)
    {
      return (@this.Date == dateToCompare.Date);
    }

    public static bool IsMorning(this DateTime @this)
    {
      return @this.TimeOfDay < new DateTime(2000, 1, 1, 12, 0, 0).TimeOfDay;
    }

    public static bool IsFuture(this DateTime @this)
    {
      return @this > DateTime.Now;
    }

    public static bool IsPast(this DateTime @this)
    {
      return @this < DateTime.Now;
    }

    public static bool IsTimeEqual(this DateTime @this, DateTime timeToCompare)
    {
      return (@this.TimeOfDay == timeToCompare.TimeOfDay);
    }

    public static bool IsWeekDay(this DateTime @this)
    {
      return !(@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
    }

    public static bool IsWeekendDay(this DateTime @this)
    {
      return (@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
    }

    public static DateTime SetTime(this DateTime @this, int hour)
    {
      return SetTime(@this, hour, 0, 0, 0);
    }

    public static DateTime SetTime(this DateTime @this, int hour, int minute)
    {
      return SetTime(@this, hour, minute, 0, 0);
    }

    public static DateTime SetTime(this DateTime @this, int hour, int minute, int second, int millisecond)
    {
      return new DateTime(@this.Year, @this.Month, @this.Day, hour, minute, second, millisecond);
    }

    public static DateTime StartOfDay(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, @this.Day);
    }

    public static DateTime StartOfMonth(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, 1);
    }

    public static DateTime StartOfWeek(this DateTime @this, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
    {
      var start = new DateTime(@this.Year, @this.Month, @this.Day);

      if (start.DayOfWeek != startDayOfWeek)
      {
        int d = startDayOfWeek - start.DayOfWeek;
        if (startDayOfWeek <= start.DayOfWeek)
        {
          return start.AddDays(d);
        }
        return start.AddDays(-7 + d);
      }

      return start;
    }

    public static DateTime StartOfYear(this DateTime @this)
    {
      return new DateTime(@this.Year, 1, 1);
    }

    public static TimeSpan ToEpochTimeSpan(this DateTime @this)
    {
      return @this.Subtract(new DateTime(1970, 1, 1));
    }

    public static DateTime Yesterday(this DateTime @this)
    {
      return @this.AddDays(-1);
    }

    public static DateTime Tomorrow(this DateTime @this)
    {
      return @this.AddDays(1);
    }

    public static double ToUnixTimestamp(this DateTime @this)
    {
      var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      var diff = @this - origin;
      return Math.Floor(diff.TotalSeconds);
    }

    public static DateTime FromUnixTimestamp(this double @this)
    {
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      return epoch.AddSeconds(@this);
    }

    public static DateTime ToDayEnd(this DateTime @this)
    {
      return @this.Date.AddDays(1).AddMilliseconds(-1);
    }

    public static IEnumerable<DateTime> DaysOfMonth(int year, int month)
    {
      return Enumerable.Range(0, DateTime.DaysInMonth(year, month))
          .Select(day => new DateTime(year, month, day + 1));
    }

    public static int WeekDayInstanceOfMonth(this DateTime @this)
    {
      var y = 0;
      return DaysOfMonth(@this.Year, @this.Month)
          .Where(x => x.DayOfWeek.Equals(@this.DayOfWeek))
          .Select(x => new { n = ++y, @this = x })
          .Where(x => x.@this.Equals(new DateTime(@this.Year, @this.Month, @this.Day)))
          .Select(x => x.n).FirstOrDefault();
    }

    public static int TotalDaysInMonth(this DateTime @this)
    {
      return DaysOfMonth(@this.Year, @this.Month).Count();
    }

    public static DateTime ToDateTimeUnspecified(this DateTime @this)
    {
      if (@this.Kind == DateTimeKind.Unspecified)
      {
        return @this;
      }

      return new DateTime(@this.Year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, DateTimeKind.Unspecified);
    }

    public static DateTime TrimMilliseconds(this DateTime @this)
    {
      return new DateTime(@this.Year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Kind);
    }
  }

  /// <summary>
  /// DateTime간의 시간의 차이를 구할 때 기준이 되는 열거형입니다.
  /// </summary>
  public enum PartOfDateTime
  {
    /// <summary>
    /// 일자의 연도 구성 요소입니다.
    /// </summary>
    Year,
    /// <summary>
    /// 일자의 분기 구성 요소입니다.
    /// </summary>
    Quarter,
    /// <summary>
    /// 일자의 월 구성 요소입니다.
    /// </summary>
    Month,
    /// <summary>
    /// 일자의 주 구성 요소입니다.
    /// </summary>
    Week,
    /// <summary>
    /// 일자의 일 구성 요소입니다.
    /// </summary>
    Day,
    /// <summary>
    /// 일자의 시 구성 요소입니다.
    /// </summary>
    Hour,
    /// <summary>
    /// 일자의 분 구성 요소입니다.
    /// </summary>
    Minute,
    /// <summary>
    /// 일자의 초 구성 요소입니다.
    /// </summary>
    Second,
    /// <summary>
    /// 일자의 밀리 초 구성 요소입니다.
    /// </summary>
    Millisecond
  }
}
