namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 성능 기록 정보에 대한 기본 인터페이스 정의입니다.
    /// </summary>
    public interface IPerformanceInfo
    {
        /// <summary>
        /// IPerformanceInfo에 등록된 카운터를 호출한 갯수를 가져옵니다.
        /// </summary>
        int MonitorCount
        {
            get;
        }

        /// <summary>
        /// 성능 카운터 범주의 이름을 가져오거나 설정합니다.
        /// </summary>
        string CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// IPerformanceInfo에 등록된 카운터의 원시 값 또는 계산되지 않은 값을 가져옵니다.
        /// </summary>
        void GenerateNextValues();
    }
}
