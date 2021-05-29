using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 길이 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class LengthRule : BusinessRule
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// 기본적인 길이 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="min">엔터티 타입 속성값에 대한의 최소한의 길이값입니다.</param>
        /// <param name="max">엔터티 타입 속성값에 대한의 최대한의 길이값입니다.</param>
        public LengthRule(string propertyName, int min, int max)
            : base(propertyName)
        {
            minLength = min;
            maxLength = max;

            ErrorMessage = string.Concat(propertyName, "의 값의 길이는 '", minLength, "'과 '", maxLength, "'의 사이 여야합니다.");
        }

        /// <summary>
        /// 기본적인 길이 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        /// <param name="min">엔터티 타입 속성값에 대한의 최소한의 길이값입니다.</param>
        /// <param name="max">엔터티 타입 속성값에 대한의 최대한의 길이값입니다.</param>
        public LengthRule(string propertyName, string errorMessage, int min, int max)
            : this(propertyName, min, max)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 길이 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Validate(EntityObject businessObject)
        {
            int length = GetPropertyValue(businessObject).ToString().Length;
            return length >= minLength && length <= maxLength;
        }
    }
}
