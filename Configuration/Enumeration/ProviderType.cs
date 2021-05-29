

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// 응용 프로그램 항목을 저장할 방식을 지정합니다.
    /// </summary>
    public enum ProviderType
    {
        /// <summary>
        /// .NET 응용 프로그램 구성 파일입니다.
        /// </summary>
        Appication,
        /// <summary>
        /// Custom Xml 파일입니다.
        /// </summary>
        XmlFile,
        /// <summary>
        /// Database 입니다.
        /// </summary>
        Database
    }
}
