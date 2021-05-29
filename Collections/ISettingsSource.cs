using System.Collections;

namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스 정의입니다.
    /// </summary>
    public interface ISettingsSource : IEnumerable
    {
        /// <summary>
        /// ISetting 인터페이스를 구현한 타입을 반환합니다.
        /// </summary>
        /// <param name="fieldName">설정값을 조회 하기 위한 필드명입니다.</param>
        /// <returns>ISetting</returns>
        ISetting Get(string fieldName);
    }
}
