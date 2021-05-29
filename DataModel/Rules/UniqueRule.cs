using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 유일값 입력 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class UniqueRule : BusinessRule
    {
        /// <summary>
        /// 기본적인 유일값 입력 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        public UniqueRule(string propertyName)
            : base(propertyName)
        {
            ErrorMessage = propertyName + "은 유일한 값이어야 합니다.";
        }

        /// <summary>
        /// 기본적인 필수 유일값 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        public UniqueRule(string propertyName, string errorMessage)
            : base(propertyName)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 필수 유일값 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Validate(EntityObject businessObject)
        {
            try
            {
                int id = int.Parse(GetPropertyValue(businessObject).ToString());
                return id >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
