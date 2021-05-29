namespace Qrame.CoreFX.DataModel
{
    /// <summary>
    /// 엔터티 타입에서 유효성 검사를 수행할 기준 열거자입니다.
    /// </summary>
    public enum ValidationOperator
    {
        /// <summary>
        /// 같음을 표현합니다.
        /// </summary>
        Equal, 

        /// <summary>
        /// 같지 않음을 표현합니다.
        /// </summary>
        NotEqual, 

        /// <summary>
        /// 보다 높음을 표현합니다.
        /// </summary>
        GreaterThan, 

        /// <summary>
        /// 보다 높거나 같음을 표현합니다.
        /// </summary>
        GreaterThanEqual, 

        /// <summary>
        /// 보다 작음을 표현합니다.
        /// </summary>
        LessThan, 

        /// <summary>
        /// 보다 작거나 같음을 표현합니다.
        /// </summary>
        LessThanEqual
    }
}
