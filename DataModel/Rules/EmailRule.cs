using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 이메일 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class EmailRule : RegexRule
    {
        /// <summary>
        /// 기본적인 이메일 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        public EmailRule(string propertyName) :
            base(propertyName, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
            ErrorMessage = propertyName + "은 유효하지 않은 이메일 주소입니다.";
        }

        /// <summary>
        /// 기본적인 이메일 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        public EmailRule(string propertyName, string errorMessage) :
            this(propertyName)
        {
            ErrorMessage = errorMessage;
        }
    }
}
