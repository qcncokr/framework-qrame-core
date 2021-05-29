using System;
using Qrame.CoreFX;
using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>    
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 응용 프로그램 구성 제공자의 기본 관리 기능을 제공 합니다. 
    /// ConfigurationProviderBase를 상속 하여, 업무에 필요한 다양한 데이터 소스의 응용 프로그램 구성 제공자를 만들수 있습니다.
    /// </summary>
    /// <typeparam name="T">응용 프로그램 구성 파일에 지정될 제너릭 타입입니다.</typeparam>
    public abstract class ConfigurationProviderBase<T> : IConfigurationProvider where T : ApplicationConfig, new()
    {
        /// <summary>
        /// 응용 프로그램 항목을 구현 중에 예외 발생시 메시지입니다.
        /// </summary>
        private string exceptionMessage = "";

        /// <summary>
        /// 응용 프로그램 항목을 구현 중에 예외 발생시 메시지를 가져오거나 설정합니다.
        /// </summary>
        public virtual string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// 콤마로 나열된 암호화가 필요한 항목 목록입니다.
        /// </summary>
        private string propertiesToEncrypt = "";

        /// <summary>
        /// 콤마로 나열된 암호화가 필요한 항목 목록이 있는 문자열을 가져오거나 설정합니다.
        /// </summary>
        public virtual string PropertiesToEncrypt
        {
            get { return propertiesToEncrypt; }
            set { propertiesToEncrypt = value; }
        }

        /// <summary>
        /// 암호화가 필요한 구성 항목에 적용할 기본값입니다.
        /// </summary>
        private string decryptionKey = "5tkatjd!";

        /// <summary>
        /// 암호화가 필요한 구성 항목에 적용할 키값을 가져오거나 설정합니다.
        /// </summary>
        public virtual string DecryptionKey
        {
            get { return decryptionKey; }
            set { decryptionKey = value; }
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자를 통해 새로운 제너릭 구성 인스턴스를 생성 합니다
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public abstract T Read<T>() where T : ApplicationConfig, new();

        /// <summary>
        /// 응용 프로그램 항목을 조회합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public abstract bool Read(ApplicationConfig config);

        /// <summary>
        /// 응용 프로그램 항목을 기록합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public abstract bool Write(ApplicationConfig config);

        /// <summary>
        /// .NET XML Serialization 문자열로 저장된 응용 프로그램 구성 제공자의 새로운 인스턴스를 가져옵니다.
        /// Serialization 양식은 WriteSerialize 메서드를 통해 만들어진 양식이어야 합니다
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <param name="xml">.NET XML Serialization 문서입니다.</param>
        /// <returns>ApplicationConfig 인스턴스 타입입니다.</returns>
        public virtual T Read<T>(string xml) where T : ApplicationConfig, new()
        {
            T result = null;

            try
            {
                result = Reflector.DeSerializeXml<T>(xml) as T;
            }
            catch (Exception exception)
            {
                SetError(exception);
                return null;
            }

            return result;
        }

        /// <summary>
        /// ApplicationConfig 클래스를 구현한 제공자의 타입과 .NET XML Serialization 문자열으로 저장된 응용 프로그램 구성 제공자의 인스턴스를 가져옵니다.
        /// Serialization 양식은 WriteSerialize 메서드를 통해 만들어진 양식이어야 합니다
        /// </summary>
        /// <param name="config">ApplicationConfig 타입 기반의 타입입니다.</param>
        /// <param name="fileName">.NET XML Serialization 문서입니다.</param>
        /// <returns>응용 프로그램 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Read(ApplicationConfig config, string fileName)
        {
            T newConfig = null;

            try
            {
                newConfig = Reflector.Deserialize<T>(fileName) as T;
            }
            catch (Exception exception)
            {
                SetError(exception);
                return false;
            }

            if (newConfig != null)
            {
                Reflector.CopyTo<ApplicationConfig>(newConfig, config, "Provider, ExceptionMessage");
                return true;
            }

            return false;
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자를 .NET XML Serialization 문자열으로 반환합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 타입 기반의 타입입니다.</param>
        /// <returns>.NET XML Serialization 문서입니다.</returns>
        public virtual string WriteSerialize(ApplicationConfig config)
        {
            string result = "";

            try
            {
                result = Reflector.Serialize<ApplicationConfig>(config);
            }
            catch (Exception exception)
            {
                SetError(exception);
                return "";
            }

            return result;
        }

        /// <summary>
        /// 응용 프로그램 항목을 구현 중에 예외 발생시 Exception 예외 메시지를 구성합니다.
        /// </summary>
        /// <param name="message">에러메세지</param>
        protected virtual void SetError(string message)
        {
            if (message.Length == 0)
            {
                ExceptionMessage = "";
                return;
            }

            ExceptionMessage = message;
        }

        /// <summary>
        /// 응용 프로그램 항목을 구현 중에 예외 발생시 Exception 예외 메시지를 구성합니다.
        /// </summary>
        /// <param name="e">Exception 타입</param>
        protected virtual void SetError(Exception e)
        {
            string message = e.Message;

            if (e.InnerException != null)
            {
                message += " " + e.GetOriginalMessage();
            }

            SetError(message);
        }

        /// <summary>
        /// 응용 프로그램 구성 제공자의 새로운 인스턴스를 생성합니다.
        /// </summary>
        /// <returns>ApplicationConfig 인스턴스 타입입니다.</returns>
        protected T CreateConfigurationInstance()
        {
            return Activator.CreateInstance(typeof(T)) as T;
        }
    }
}
