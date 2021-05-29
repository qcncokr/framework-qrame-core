
namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 로그 값에 대한 레벨 수준을 표현하는 열거자입니다.
    /// </summary>
    public enum EntryLevel
    {      
        /// <summary>
        /// 응용 프로그램에 심각한 오류가 발생 했습니다.
        /// </summary>          
        Error,
        /// <summary>
        /// 예측 가능한 문제가 생길 수 있습니다.
        /// </summary>
        Warning,
        /// <summary>
        /// 예외 정보 입니다.
        /// </summary>
        Exception,
        /// <summary>
        /// 메시지를 로그로 기록합니다.
        /// </summary>
        Message,
        /// <summary>
        /// 여러 가지 종류의 주제에 대한 로그로 기록합니다.
        /// </summary>
        Miscellaneous,
        /// <summary>
        /// 기타 메시지를 로그로 기록합니다. 로그 조회시 전체 조회 옵션으로 사용합니다.
        /// </summary>
        All
    }
}
