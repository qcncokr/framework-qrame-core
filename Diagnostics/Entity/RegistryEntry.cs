
using Qrame.CoreFX;

namespace Qrame.CoreFX.Diagnostics.Entity
{
    /// <summary>
    /// Registry Adapter에서 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현하기 위한 엔터티 타입입니다.
    /// </summary>
    internal class RegistryEntry : BaseEntity
    {
        /// <summary>
        /// 레지스트리의 키입니다.
        /// </summary>
        private string keyName = "";

        /// <summary>
        /// 레지스트리의 키를 가져오거나, 설정합니다.
        /// </summary>
        public string KeyName
        {
            get { return keyName; }
            set { keyName = value; }
        }

        /// <summary>
        /// 레지스트리의 값입니다.
        /// </summary>
        private object keyValue = null;

        /// <summary>
        /// 레지스트리의 값을 가져오거나, 설정합니다.
        /// </summary>
        public object KeyValue
        {
            get { return keyValue; }
            set { keyValue = value; }
        }

        /// <summary>
        /// 레지스트리의 서브 섹션을 구성하는 로그 레벨 수준입니다.
        /// </summary>
        private EntryLevel entryLevel = EntryLevel.Exception;

        /// <summary>
        /// 레지스트리의 서브 섹션을 구성하는 로그 레벨 수준을 가져오거나, 설정합니다.
        /// </summary>
        public EntryLevel EntryLevel
        {
            get { return entryLevel; }
            set { entryLevel = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, 빈 문자열의 키, null값의 값, Exception의 로그 레벨 수준을 기본값으로 초기값을 설정합니다.
        /// </summary>
        public RegistryEntry()
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 지정된 문자열의 키, 값, 로그 레벨 수준을 초기값을 설정합니다.
        /// </summary>
        /// <param name="name">레지스트리의 섹션입니다.</param>
        /// <param name="value">레지스트리의 키입니다.</param>
        /// <param name="level"> 로그 값에 대한 레벨 수준을 표현하는 열거자입니다.</param>
        public RegistryEntry(string name, object value, EntryLevel level)
        {
            KeyName = name;
            KeyValue = value;
            EntryLevel = level;
        }
    }
}
