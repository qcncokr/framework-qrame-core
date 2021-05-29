using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Qrame.CoreFX.DataModel
{
    /// <summary>
    /// 엔터티 타입에 유효성 검사에 대한 베이스 기능을 제공합니다.
    /// </summary>
    public abstract class EntityObject : BaseEntity
    {
        /// <summary>
        /// 기본 버전 정보입니다.
        /// </summary>
        protected static readonly string versionDefault = "NotSet";

        private List<BusinessRule> businessRules = new List<BusinessRule>();
        private List<string> validationErrors = new List<string>();

        /// <summary>
        /// 엔터티 타입에 부여할 기본적인 업무 규칙을 표현하는 추상 클래스 컬렉션을 가져오거나 설정합니다.
        /// </summary>
        public List<BusinessRule> BusinessRules
        {
            get { return businessRules; }
            set { businessRules = value; }
        }

        /// <summary>
        /// 유효성 검사에서 발생한 에러 메시지 문자열 목록을 가져옵니다.
        /// </summary>
        public List<string> ValidationErrors
        {
            get { return validationErrors; }
        }

        /// <summary>
        /// 엔터티 타입에 부여할 유효성 검사 업무 규칙을 추가합니다.
        /// </summary>
        /// <param name="rule">엔터티 타입에 부여할 기본적인 업무 규칙을 표현하는 추상 클래스입니다.</param>
        protected void AddRule(BusinessRule rule)
        {
            businessRules.Add(rule);
        }

        /// <summary>
        /// 엔터티 타입에 부여할 유효성 검사 업무 규칙을 삭제합니다.
        /// </summary>
        /// <param name="index">업무 규칙을 삭제할 컬렉션의 인덱스입니다.</param>
        protected void RemoveAtRule(int index)
        {
            businessRules.RemoveAt(index);
        }

        /// <summary>
        /// 엔터티 타입에 부여된 모든 유효성 규칙을 검사합니다.
        /// </summary>
        /// <returns>모든 유효성 검사에 통과하면 true를, 아니면 false를 반환합니다.</returns>
        public bool Validate()
        {
            bool IsValid = true;

            validationErrors.Clear();

            foreach (BusinessRule Rule in businessRules)
            {
                if (Rule.Validate(this) == false)
                {
                    IsValid = false;
                    validationErrors.Add(Rule.ErrorMessage);
                }
            }
            return IsValid;
        }
    }
}
