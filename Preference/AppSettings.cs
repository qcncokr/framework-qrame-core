using Qrame.CoreFX.Collections;

namespace Qrame.CoreFX.Preference
{
    /// <summary>
    /// 기본 응용 프로그램 구성파일의 AppSettings 항목에 접근을 제공합니다.
    /// </summary>
    public class AppSettings : SettingsCollection
    {
        /// <summary>
        /// 기본 생성자 호출시 구성 파일에서 설정값을 조회하는 기능을 상속받습니다.
        /// </summary>
        public AppSettings()
            : base(new SettingsSourceAppSettings())
        {
        }
    }
}
