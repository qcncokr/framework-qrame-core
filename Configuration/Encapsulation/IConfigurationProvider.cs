using System;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// Configuration Provider 인터페이스는 다양한 데이터 소스로 구성된 요소에서 동일한 방법으로
    /// 항목을 읽거나 쓰기 위한 매커니즘을 제공 합니다.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// 응용 프로그램 항목을 제어 할 때 발생하는 에러 메시지를 가져오거나 설정합니다.
        /// </summary>
        string ExceptionMessage { get; set; }

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        T Read<T>() where T : ApplicationConfig, new();

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <typeparam name="T">ApplicationConfig 타입 기반의 제너릭 타입입니다.</typeparam>
        /// <param name="Xml">.NET XML Serialization 문자열입니다.</param>
        /// <returns>ApplicationConfig 타입 기반의 제너릭 타입입니다.</returns>
        T Read<T>(string Xml) where T : ApplicationConfig, new();

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        bool Read(ApplicationConfig config);

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 조회합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <param name="Xml">.NET XML Serialization 문자열입니다.</param>
        /// <returns>응용 프로그램 설정 파일에서 항목을 정상적으로 읽으면 true를, 아니면 false를 반환합니다.</returns>
        bool Read(ApplicationConfig config, string xml);

        /// <summary>
        /// 응용 프로그램 구성 Provider에서 항목을 기록합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <returns>응용 프로그램 구성 항목이 정상적으로 만들어지면 true를, 아니면 false를 반환합니다.</returns>
        bool Write(ApplicationConfig config);

        /// <summary>
        /// 응용 프로그램 구성 Provider을 .NET XML Serialization 문자열로 변환합니다.
        /// </summary>
        /// <param name="Config">ApplicationConfig 인스턴스</param>
        /// <returns>.NET XML Serialization 문자열입니다.</returns>
        string WriteSerialize(ApplicationConfig config);
    }
}
