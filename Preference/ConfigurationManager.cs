using Qrame.CoreFX.Patterns;

namespace Qrame.CoreFX.Preference
{
    /// <summary>
    /// System.Configuration.ConfigurationManager의 partial 클래스로 기본 응용 프로그램 구성파일의 AppSettings, ConnectionStrings
    /// 항목에 단일 인스턴스 접근을 제공합니다.
    /// </summary>
    public static partial class ConfigurationManager
    {
        /// <summary>
        /// AppSettings 구성 항목의 접근 타입을 조회합니다.
        /// </summary>
        public static AppSettings AppSettings
        {
            get
            {
                return Singleton<AppSettings>.Instance;
            }
        }

        /// <summary>
        /// ConnectionStrings 구성 항목의 접근 타입을 조회합니다.
        /// </summary>
        public static ConnectionStrings ConnectionStrings
        {
            get
            {
                return Singleton<ConnectionStrings>.Instance;
            }
        }
    }
}
