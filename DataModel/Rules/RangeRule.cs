using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 범위 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class RangeRule : BusinessRule
    {
        private ValidationType validationType { get; set; }
        private ValidationOperator validationOperator { get; set; }

        private object minValue { get; set; }
        private object maxValue { get; set; }

        /// <summary>
        /// 기본적인 범위 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="min">엔터티 타입 속성값에 대한의 최소한의 길이값입니다.</param>
        /// <param name="max">엔터티 타입 속성값에 대한의 최대한의 길이값입니다.</param>
        /// <param name="compareOperator">엔터티 타입에서 유효성 검사를 수행할 기준 열거자입니다.</param>
        /// <param name="validation">엔터티 타입에서 유효성 검사 방식에 대한 타입 기준 열거자입니다.</param>
        public RangeRule(string propertyName, object min, object max, ValidationOperator compareOperator, ValidationType validation)
            : base(propertyName)
        {
            minValue = min;
            maxValue = max;

            validationOperator = compareOperator;
            validationType = validation;

            ErrorMessage = string.Concat(propertyName, "의 값은 '", min, "'과 '", max, "'의 사이 여야합니다.");
        }

        /// <summary>
        /// 기본적인 범위 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        /// <param name="min">엔터티 타입 속성값에 대한의 최소한의 길이값입니다.</param>
        /// <param name="max">엔터티 타입 속성값에 대한의 최대한의 길이값입니다.</param>
        /// <param name="compareOperator">엔터티 타입에서 유효성 검사를 수행할 기준 열거자입니다.</param>
        /// <param name="validation">엔터티 타입에서 유효성 검사 방식에 대한 타입 기준 열거자입니다.</param>
        public RangeRule(string propertyName, string errorMessage, object min, object max, ValidationOperator compareOperator, ValidationType validation)
            : this(propertyName, min, max, compareOperator, validation)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 범위 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Validate(EntityObject businessObject)
        {
            try
            {
                string value = GetPropertyValue(businessObject).ToString();

                switch (validationType)
                {
                    case ValidationType.Integer:

                        int imin = int.Parse(minValue.ToString());
                        int imax = int.Parse(maxValue.ToString());
                        int ival = int.Parse(value);

                        return (ival >= imin && ival <= imax);

                    case ValidationType.Double:
                        double dmin = double.Parse(minValue.ToString());
                        double dmax = double.Parse(maxValue.ToString());
                        double dval = double.Parse(value);

                        return (dval >= dmin && dval <= dmax);

                    case ValidationType.Decimal:
                        decimal cmin = decimal.Parse(minValue.ToString());
                        decimal cmax = decimal.Parse(maxValue.ToString());
                        decimal cval = decimal.Parse(value);

                        return (cval >= cmin && cval <= cmax);

                    case ValidationType.Date:
                        DateTime tmin = DateTime.Parse(minValue.ToString());
                        DateTime tmax = DateTime.Parse(maxValue.ToString());
                        DateTime tval = DateTime.Parse(value);

                        return (tval >= tmin && tval <= tmax);

                    case ValidationType.String:

                        string smin = minValue.ToString();
                        string smax = maxValue.ToString();

                        int result1 = string.Compare(smin, value);
                        int result2 = string.Compare(value, smax);

                        return result1 >= 0 && result2 <= 0;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
