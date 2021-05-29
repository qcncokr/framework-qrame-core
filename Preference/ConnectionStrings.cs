using System.Configuration;

namespace Qrame.CoreFX.Preference
{
    /// <summary>
    /// 기본 응용 프로그램 구성파일의 ConnectionStrings 항목에 대한 접근을 제공합니다.
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// 현재 응용 프로그램의 기본 구성에 대한 지정된 구성 섹션의 값을 검색합니다.
        /// </summary>
        /// <param Name="connectionName">구성 섹션 이름입니다.</param>
        /// <returns>지정된 구성 섹션의 값이거나, 섹션이 없으면 null입니다.</returns>
        public string GetConnectionString(string connectionName)
        {
            return GetConnectionString(connectionName, null);
        }

        /// <summary>
        /// 현재 응용 프로그램의 기본 구성에 대한 지정된 구성 섹션의 값을 검색합니다. 섹션값이 없을 경우 지정된 기본값을 반환합니다.
        /// </summary>
        /// <param name="connectionName">구성 섹션 이름입니다.</param>
        /// <param name="defaultValue">섹션값이 없을 경우 지정된 기본값입니다.</param>
        /// <returns>지정된 구성 섹션의 값이거나, 섹션이 없으면 지정된 기본값을 반환합니다.</returns>
        public string GetConnectionString(string connectionName, string defaultValue)
        {
            ConnectionStringSettings Settings = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];

            if (Settings == null)
            {
                return defaultValue;
            }

            return Settings.ConnectionString;
        }

        /// <summary>
        /// 현재 응용 프로그램의 기본 구성에 대한 지정된 구성 섹션의 값을 검색합니다.
        /// </summary>
        /// <param Name="connectionName">구성 섹션 이름입니다.</param>
        /// <returns>지정된 구성 섹션의 값이거나, 섹션이 없으면 null입니다.</returns>
        public string this[string connectionName]
        {
            get { return GetConnectionString(connectionName); }
        }

        /// <summary>
        /// 현재 응용 프로그램의 기본 구성에 대한 지정된 구성 섹션의 값을 검색합니다. 섹션값이 없을 경우 지정된 기본값을 반환합니다.
        /// </summary>
        /// <param name="connectionName">구성 섹션 이름입니다.</param>
        /// <param name="defaultValue">섹션값이 없을 경우 지정된 기본값입니다.</param>
        /// <returns>지정된 구성 섹션의 값이거나, 섹션이 없으면 지정된 기본값을 반환합니다.</returns>
        public string this[string connectionName, string defaultValue]
        {
            get { return GetConnectionString(connectionName, defaultValue); }
        }
    }
}
