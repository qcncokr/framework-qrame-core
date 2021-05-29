using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 정규식 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class RegexRule : BusinessRule
    {
        protected string Pattern { get; set; }

        /// <summary>
        /// 기본적인 정규식 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="min">엔터티 타입 속성값에 대한의 최소한의 길이값입니다.</param>
        public RegexRule(string propertyName, string pattern)
            : base(propertyName)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// 기본적인 정규식 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        /// <param name="pattern">정규식을 표현하는 문자열입니다.</param>
        public RegexRule(string propertyName, string errorMessage, string pattern)
            : this(propertyName, pattern)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 정규식 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Validate(EntityObject businessObject)
        {
            return Regex.Match(GetPropertyValue(businessObject).ToString(), Pattern).Success;
        }
    }
}
