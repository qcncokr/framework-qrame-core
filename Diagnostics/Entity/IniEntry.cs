using Qrame.CoreFX;

namespace Qrame.CoreFX.Diagnostics.Entity
{
    /// <summary>
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현하기 위한 엔터티 타입입니다.
    /// </summary>
    internal class IniEntry : BaseEntity
    {
        /// <summary>
        /// ini 파일의 섹션입니다.
        /// </summary>
        private string section = "";

        /// <summary>
        /// ini 파일의 섹션을 가져오거나, 설정합니다.
        /// </summary>
        public string Section
        {
            get { return section; }
            set { section = value; }
        }

        /// <summary>
        /// ini 파일의 키입니다.
        /// </summary>
        private string keyValue = null;

        /// <summary>
        /// ini 파일의 키를 가져오거나, 설정합니다.
        /// </summary>
        public string KeyValue
        {
            get { return keyValue; }
            set { keyValue = value; }
        }

        /// <summary>
        /// ini 파일의 값입니다.
        /// </summary>
        private string defaultValue = "";

        /// <summary>
        /// ini 파일의 값을 가져오거나, 설정합니다.
        /// </summary>
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, 빈 문자열의 섹션, null값의 키, 빈 문자열의 기본값으로 초기값을 설정합니다.
        /// </summary>
        public IniEntry()
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 지정된 섹션, 키, 기본값으로 초기값을 설정합니다.
        /// </summary>
        /// <param name="section">ini 파일의 섹션입니다.</param>
        /// <param name="keyValue">ini 파일의 키입니다.</param>
        /// <param name="defaultValue">ini 파일에 기록할 값입니다.</param>
        public IniEntry(string section, string keyValue, string defaultValue)
        {
            Section = section;
            KeyValue = keyValue;
            DefaultValue = defaultValue;
        }
    }
}
