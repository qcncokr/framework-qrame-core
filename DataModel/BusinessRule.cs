using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel
{
    /// <summary>
    /// 엔터티 타입에 부여할 기본적인 업무 규칙을 표현하는 추상 클래스입니다.
    /// </summary>
    public abstract class BusinessRule
    {
        /// <summary>
        /// 속성에 대한 이름을 가져오거나, 설정합니다.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 유효성 검사 실패에 대한 내용을 가져오거나, 설정합니다.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 기본적인 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        public BusinessRule(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = propertyName + "은 유효하지 않습니다.";
        }

        /// <summary>
        /// 기본적인 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        public BusinessRule(string propertyName, string errorMessage)
            : this(propertyName)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public abstract bool Validate(EntityObject businessObject);

        /// <summary>
        /// 엔터티 타입의 속성값을 가져옵니다.
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        protected object GetPropertyValue(EntityObject businessObject)
        {
            return businessObject.GetType().GetProperty(PropertyName).GetValue(businessObject, null);
        }
    }
}
