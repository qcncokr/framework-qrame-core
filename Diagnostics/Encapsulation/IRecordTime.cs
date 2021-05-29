using System.Collections.Generic;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 정의합니다.
    /// </summary>
    public interface IRecordTime
    {
        /// <summary>
        /// ProcessTimerRecorder의 기록을 저장합니다.
        /// </summary>
        bool Save(Dictionary<long, dynamic> customEntitys, Dictionary<long, EnvironmentInfo> environmentInfos, Dictionary<long, SqlServerStatisticsInfo> statisticsInfos);
    }
}
