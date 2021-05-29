
namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 로그 기능을 구현하기 위한 Adapter를 지정하는 열거자입니다.
    /// </summary>
    public enum AdapterType
    {
        /// <summary>
        /// TextFile 파일에 기록합니다.
        /// </summary>
        TextFile,
        /// <summary>
        /// Ini 파일에 기록합니다.
        /// </summary>
        Ini,
        /// <summary>
        /// 레지스트리에 기록합니다.
        /// </summary>
        Registry,
        /// <summary>
        /// 이벤트 로그에 기록합니다.
        /// </summary>
        EventLog,
        /// <summary>
        /// 데이터베이스에 기록합니다.
        /// </summary>
        Database
    }
}
