
namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 컴퓨터의 레지스트리에 데이터를 기록 할 때, 적용하는 옵션 열거자입니다.
    /// </summary>
    public enum RegistryKind
    {
        /// <summary>
        /// 모든 형태의 이진 데이터입니다. RegistryValueKind.Binary에 대응합니다.
        /// </summary>
        Binary,

        /// <summary>
        /// 32비트 이진수입니다. RegistryValueKind.DWord에 대응합니다.
        /// </summary>
        DWord,

        /// <summary>
        /// 값이 검색될 때 확장되는 %PATH%와 같은 환경 변수에 대한 확장되지 않는 참조가 포함된 null로 끝나는 문자열입니다. RegistryValueKind.ExpandString에 대응합니다.
        /// </summary>
        ExpandString,

        /// <summary>
        /// 두 null 문자로 끝나는, null로 끝나는 문자열의 배열입니다. RegistryValueKind.MultiString에 대응합니다.
        /// </summary>
        MultiString,

        /// <summary>
        /// 지원되지 않는 레지스트리 데이터 형식입니다. RegistryValueKind.None에 대응합니다.
        /// </summary>
        None,

        /// <summary>
        /// 64비트 이진수입니다. RegistryValueKind.QWord에 대응합니다.
        /// </summary>
        QWord,

        /// <summary>
        /// null로 끝나는 문자열입니다. RegistryValueKind.String에 대응합니다.
        /// </summary>
        String
    }
}
