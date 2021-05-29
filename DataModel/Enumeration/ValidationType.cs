
namespace Qrame.CoreFX.DataModel
{
    /// <summary>
    /// 엔터티 타입에서 유효성 검사 방식에 대한 타입 기준 열거자입니다.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// 문자열입니다.
        /// </summary>
        String, 

        /// <summary>
        /// 정수입니다.
        /// </summary>
        Integer,

        /// <summary>
        /// double형 입니다.
        /// </summary>
        Double, 

        /// <summary>
        /// decimal형 입니다.
        /// </summary>
        Decimal,

        /// <summary>
        /// 일자입니다.
        /// </summary>
        Date
    }
}
