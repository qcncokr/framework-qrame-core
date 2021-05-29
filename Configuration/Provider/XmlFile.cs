using System;
using Qrame.CoreFX;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// .NET 응용 프로그램 구성 제공자 기본 템플릿 클래스 입니다. 현재 클래스를 이용하거나, 참조하여 새로운 클래스를 개발합니다.
    /// </summary>
    /// <typeparam name="T">XML 파일에 응용 프로그램 구성값이 지정될 제너릭 타입입니다.</typeparam>
    public class XmlFile<T> : ConfigurationProviderBase<T> where T : ApplicationConfig, new()
    {
        private string xmlFileName = "";

        /// <summary>
        /// 구성 파일명을 가져오거나 설정합니다.(공백일 경우 현재 응용 프로그램의 구성파일을 설정합니다.)
        /// </summary>
        public string XmlFileName
        {
            get { return xmlFileName; }
            set { xmlFileName = value; }
        }

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        public override T Read<T>()
        {
            var newConfig = Reflector.Deserialize<T>(XmlFileName) as T;

            return newConfig;
        }

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Read(ApplicationConfig config)
        {
            var newConfig = Reflector.Deserialize<T>(XmlFileName) as T;
            if (newConfig == null)
            {
                if (Write(config))
                {
                    return true;
                }

                return false;
            }

            Reflector.CopyTo(newConfig, config, "Provider, ExceptionMessage");

            return true;
        }

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 기록합니다.
        /// </summary>
        /// <param name="config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 구성 항목이 정상적으로 만들어지면 true를, 아니면 false를 반환합니다.</returns>
        public override bool Write(ApplicationConfig config)
        {
            try
            {
                Reflector.Serialize(config, XmlFileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
