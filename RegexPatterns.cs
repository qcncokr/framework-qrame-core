using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Qrame.CoreFX.Data
{
    /// <summary>    
    /// 정규식 샘플입니다. 
    /// 레퍼런스
    /// 정규식 예제 - http://msdn.microsoft.com/ko-kr/library/kweb790z(v=VS.100).aspx 
    /// The 30 Minute Regex Tutorial - http://www.codeproject.com/KB/dotnet/regextutorial.aspx
    /// </summary>
    public sealed class RegexPatterns
    {
        /// <summary>
        /// 영문 대문자와 소문자를 필터링합니다.
        /// </summary>
        public const string Alpha = "^[a-zA-Z]*$";

        /// <summary>
        /// 영문 소문자를 필터링합니다.
        /// </summary>
        public const string AlphaLowerCase = "^[a-z]*$";

        /// <summary>
        /// 영문 대문자를 필터링합니다.
        /// </summary>
        public const string AlphaUpperCase = "^[A-Z]*$";

        /// <summary>
        /// 영문 대, 소문자와 숫자를 필터링합니다.
        /// </summary>
        public const string AlphaNumeric = "^[a-zA-Z0-9]*$";

        /// <summary>
        /// 이메일 주소를 필터링합니다.
        /// </summary>
        public const string Email = "^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";

        /// <summary>
        /// 숫자를 필터링합니다.
        /// </summary>
        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";

        /// <summary>
        /// 주민번호를 필터링합니다.
        /// </summary>
        public const string SocialID = @"\d{6}[-]?\d{7}";

        /// <summary>
        /// Url 주소를 필터링합니다.
        /// </summary>
        public const string Url = @"^^(ht|f)tp(value?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
    }
}
