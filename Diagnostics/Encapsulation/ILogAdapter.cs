using System;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 다양한 데이터 소스를 사용하는 Adapter들을 통해 
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 정의합니다.
    /// </summary>
    public interface ILogAdapter
    {
        /// <summary>
        /// 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지를 가져옵니다.
        /// </summary>
        string ExceptionMessage
        {
            get;
        }

        /// <summary>
        /// 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        bool WriteEntry(LogEntry log);

        /// <summary>
        /// Adapter에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        bool DeleteLog(dynamic condition);

        /// <summary>
        /// Adapter에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        dynamic GetEntryValue(dynamic condition);

        /// <summary>
        /// Adapter에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        dynamic GetEntries();

        /// <summary>
        /// Adapter에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Adapter에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        bool Clear(dynamic condition);
    }
}
