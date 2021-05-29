using System;
using Qrame.CoreFX;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// 응용 프로그램 구성 제공자에 필요한 추상 클래스로, 구성에 필요한 데이터 소스로는 업무에 따라 
    /// Web.Config, App.Config와 같은 닷넷 응용 프로그램 구성 파일이 되거나 별도의 커스텀 XML 문서, 또는 데이터베이스가 될 수 있습니다.
    /// </summary>
    public abstract class ApplicationConfig
    {
        /// <summary>
        /// Configuration Provider 인터페이스
        /// </summary>
        [NonSerialized]
        public IConfigurationProvider Provider = null;

        /// <summary>
        /// 응용 프로그램 항목을 제어 할 때 발생하는 에러 메시지를 가져오거나 설정합니다.
        /// </summary>
        [NonSerialized]
        public string ExceptionMessage = null;

        /// <summary>
        /// 기본 생성자를 이용하여 인스턴스를 만들게 되면 더미 역할을 수행하게 됩니다.
        /// </summary>         
        public ApplicationConfig()
        {
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자를 통해 항목을 기록합니다.
        /// </summary>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Write()
        {
            // 응용 프로그램 구성 제공자에 Write 메서드가 구현이 안되어 있을 경우, 제공자에서 설정한
            // 에러 메시지를 설정후 bool값을 반환 합니다
            if (Provider.Write(this) == false)
            {
                ExceptionMessage = Provider.ExceptionMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자를 .NET XML Serialization 문자열으로 반환합니다.
        /// </summary>
        /// <returns>.NET XML Serialization 문자열입니다.</returns>
        public virtual string WriteSerialize()
        {
            return Reflector.Serialize<ApplicationConfig>(this);
        }

        /// <summary>
        /// 제너릭 타입에 맞는 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public virtual T Read<T>() where T : ApplicationConfig, new()
        {
            dynamic instance = Provider.Read<T>();

            // 응용 프로그램 구성 제공자의 인스턴스가 null일 경우, 제공자에서 설정한 에러 메시지를 설정후 bool값을 반환 합니다.
            if (instance == null)
            {
                ExceptionMessage = Provider.ExceptionMessage;
                return null;
            }

            return instance;
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자를 통해 항목을 조회합니다.
        /// </summary>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Read()
        {
            // 응용 프로그램 구성 제공자에 Read 메서드가 구현이 안되어 있을 경우, 제공자에서 설정한 에러 메시지를 설정후 bool값을 반환 합니다.
            if (Provider.Read(this) == false)
            {
                ExceptionMessage = Provider.ExceptionMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// .NET XML Serialization 문자열으로 저장된 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// Serialization 양식은 WriteSerialize 메서드를 통해 만들어진 양식이어야 합니다
        /// </summary>
        /// <param name="xml">.NET XML Serialization 문자열입니다.</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Read(string xml)
        {
            dynamic instance = Reflector.DeSerializeXml<ApplicationConfig>(xml) as ApplicationConfig;

            if (instance == null)
            {
                return false;
            }
            else
            {
                Reflector.CopyTo<ApplicationConfig>(instance, this, "Provider, ExceptionMessage");
            }

            return true;
        }

        /// <summary>
        /// Configuration Provider 인터페이스를 구현한 제공자의 Read 메서드를 수행합니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <param name="provider">Configuration Provider 인터페이스</param>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public static T Read<T>(IConfigurationProvider provider) where T : ApplicationConfig, new()
        {
            return provider.Read<T>() as T;
        }

        /// <summary>
        /// Configuration Provider 인터페이스를 구현한 제공자의 Read 메서드와
        /// .NET XML Serialization 문자열으로 저장된 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// Serialization 양식은 WriteSerialize 메서드를 통해 만들어진 양식이어야 합니다
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <param name="xml">.NET XML Serialization 문자열입니다.</param>
        /// <param name="provider">응용 프로그램 구성 제공자의 인스턴스</param>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public static T Read<T>(string xml, IConfigurationProvider provider) where T : ApplicationConfig, new()
        {
            return Reflector.DeSerializeXml<T>(xml) as T;
        }

        /// <summary>
        /// Configuration Provider 인터페이스를 구현한 제공자의 Read 메서드와
        /// .NET XML Serialization 문자열으로 저장된 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// Serialization 양식은 WriteSerialize 메서드를 통해 만들어진 양식이어야 합니다
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <param name="xml">.NET XML Serialization 문자열입니다.</param>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public static T Read<T>(string xml) where T : ApplicationConfig, new()
        {
            return Read<T>(xml, null);
        }
    }
}
