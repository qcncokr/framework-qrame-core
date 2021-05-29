namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스 정의입니다.
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// 필드명을 가져오거나 설정합니다.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 필드값을 가져오거나 설정합니다.
        /// </summary>
        object Value
        {
            get;
            set;
        }
    }
}
