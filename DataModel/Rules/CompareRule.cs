using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.DataModel.Rules
{
    /// <summary>
    /// 엔터티 데이터에 대한 비교 유효성 검사 규칙을 제공합니다.
    /// </summary>
    public class CompareRule : BusinessRule
    {
        private string otherPropertyName { get; set; }
        private ValidationType validationType { get; set; }
        private ValidationOperator validationOperator { get; set; }

        /// <summary>
        /// 기본적인 비교 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="compareName">비교 규칙에 대한 이름입니다.</param>
        /// <param name="compareOperator">엔터티 타입에서 유효성 검사를 수행할 기준 열거자입니다.</param>
        /// <param name="validation">엔터티 타입에서 유효성 검사 방식에 대한 타입 기준 열거자입니다.</param>
        public CompareRule(string propertyName, string compareName, ValidationOperator compareOperator, ValidationType validation)
            : base(propertyName)
        {

            otherPropertyName = compareName;
            validationOperator = compareOperator;
            validationType = validation;

            ErrorMessage = string.Concat(propertyName, "과 ", compareName, "은 ", validationOperator.ToString(), " 비교 식에 적합하지않습니다.");
        }

        /// <summary>
        /// 기본적인 비교 업무 규칙을 지정합니다.
        /// </summary>
        /// <param name="propertyName">엔터티 타입 속성에 대한 이름입니다.</param>
        /// <param name="compareName">비교 규칙에 대한 이름입니다.</param>
        /// <param name="errorMessage">엔터티 타입 유효성 검사 실패에 대한 내용입니다.</param>
        /// <param name="compareOperator">엔터티 타입에서 유효성 검사를 수행할 기준 열거자입니다.</param>
        /// <param name="validation">엔터티 타입에서 유효성 검사 방식에 대한 타입 기준 열거자입니다.</param>
        public CompareRule(string propertyName, string compareName, string errorMessage, ValidationOperator compareOperator, ValidationType validation)
            : this(propertyName, compareName, compareOperator, validation)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 엔터티 타입에 대한 비교 유효성 검사를 수행합니다
        /// </summary>
        /// <param name="businessObject">EntityObject 타입의 인스턴스입니다.</param>
        /// <returns>유효성 검사 결과를 반환합니다. 유효하면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Validate(EntityObject businessObject)
        {
            try
            {
                string propertyValue1 = businessObject.GetType().GetProperty(PropertyName).GetValue(businessObject, null).ToString();
                string propertyValue2 = businessObject.GetType().GetProperty(otherPropertyName).GetValue(businessObject, null).ToString();

                switch (validationType)
                {
                    case ValidationType.Integer:

                        int integerValue1 = int.Parse(propertyValue1);
                        int integerValue2 = int.Parse(propertyValue2);

                        switch (validationOperator)
                        {
                            case ValidationOperator.Equal: return integerValue1 == integerValue2;
                            case ValidationOperator.NotEqual: return integerValue1 != integerValue2;
                            case ValidationOperator.GreaterThan: return integerValue1 > integerValue2;
                            case ValidationOperator.GreaterThanEqual: return integerValue1 >= integerValue2;
                            case ValidationOperator.LessThan: return integerValue1 < integerValue2;
                            case ValidationOperator.LessThanEqual: return integerValue1 <= integerValue2;
                        }
                        break;

                    case ValidationType.Double:

                        double doubleValue1 = double.Parse(propertyValue1);
                        double doubleValue2 = double.Parse(propertyValue2);

                        switch (validationOperator)
                        {
                            case ValidationOperator.Equal: return doubleValue1 == doubleValue2;
                            case ValidationOperator.NotEqual: return doubleValue1 != doubleValue2;
                            case ValidationOperator.GreaterThan: return doubleValue1 > doubleValue2;
                            case ValidationOperator.GreaterThanEqual: return doubleValue1 >= doubleValue2;
                            case ValidationOperator.LessThan: return doubleValue1 < doubleValue2;
                            case ValidationOperator.LessThanEqual: return doubleValue1 <= doubleValue2;
                        }
                        break;

                    case ValidationType.Decimal:

                        decimal decimalValue1 = decimal.Parse(propertyValue1);
                        decimal decimalValue2 = decimal.Parse(propertyValue2);

                        switch (validationOperator)
                        {
                            case ValidationOperator.Equal: return decimalValue1 == decimalValue2;
                            case ValidationOperator.NotEqual: return decimalValue1 != decimalValue2;
                            case ValidationOperator.GreaterThan: return decimalValue1 > decimalValue2;
                            case ValidationOperator.GreaterThanEqual: return decimalValue1 >= decimalValue2;
                            case ValidationOperator.LessThan: return decimalValue1 < decimalValue2;
                            case ValidationOperator.LessThanEqual: return decimalValue1 <= decimalValue2;
                        }
                        break;

                    case ValidationType.Date:

                        DateTime dateValue1 = DateTime.Parse(propertyValue1);
                        DateTime dateValue2 = DateTime.Parse(propertyValue2);

                        switch (validationOperator)
                        {
                            case ValidationOperator.Equal: return dateValue1 == dateValue2;
                            case ValidationOperator.NotEqual: return dateValue1 != dateValue2;
                            case ValidationOperator.GreaterThan: return dateValue1 > dateValue2;
                            case ValidationOperator.GreaterThanEqual: return dateValue1 >= dateValue2;
                            case ValidationOperator.LessThan: return dateValue1 < dateValue2;
                            case ValidationOperator.LessThanEqual: return dateValue1 <= dateValue2;
                        }
                        break;

                    case ValidationType.String:

                        int result = string.Compare(propertyValue1, propertyValue2, StringComparison.CurrentCulture);

                        switch (validationOperator)
                        {
                            case ValidationOperator.Equal: return result == 0;
                            case ValidationOperator.NotEqual: return result != 0;
                            case ValidationOperator.GreaterThan: return result > 0;
                            case ValidationOperator.GreaterThanEqual: return result >= 0;
                            case ValidationOperator.LessThan: return result < 0;
                            case ValidationOperator.LessThanEqual: return result <= 0;
                        }
                        break;

                }
                return false;
            }
            catch { return false; }
        }
    }
}
