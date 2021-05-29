using Qrame.CoreFX.Collections;

namespace Qrame.CoreFX.Preference
{
    /// <summary>
    /// 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스 구현입니다.
    /// </summary>
    public class SettingsNameValuePair : ISetting
    {
        /// <summary>
        /// 필드명을 가져오거나 설정합니다.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 필드값을 가져오거나 설정합니다.
        /// </summary>
        public object Value
        {
            get;
            set;
        }
    }
}
