using Qrame.CoreFX.Collections;
using System.Collections;

namespace Qrame.CoreFX.Preference
{
    /// <summary>
    /// 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스 구현입니다.
    /// </summary>
    public class SettingsSourceAppSettings : ISettingsSource
    {
        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public SettingsSourceAppSettings()
        {
        }

        /// <summary>
        /// ISetting 인터페이스를 구현한 타입을 반환합니다.
        /// </summary>
        /// <param name="fieldName">설정값을 조회 하기 위한 필드명입니다.</param>
        /// <returns>ISetting</returns>
        public ISetting Get(string fieldName)
        {
            return new SettingsNameValuePair()
            {
                Name = fieldName,
                Value = System.Configuration.ConfigurationManager.AppSettings[fieldName]
            };
        }

        /// <summary>
        /// AppSettings의 열거형을 반환합니다.
        /// </summary>
        /// <returns>IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return System.Configuration.ConfigurationManager.AppSettings.GetEnumerator();
        }
    }
}
