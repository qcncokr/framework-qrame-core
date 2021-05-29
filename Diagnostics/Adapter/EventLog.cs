using System;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics.Adapter
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 컴퓨터의 이벤트 로그에 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현합니다.
    /// </summary>
    public class EventLog : ILogAdapter
    {
        /// <summary>
        /// 이벤트 로그에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지 입니다.
        /// </summary>
        private string exceptionMessage = "";

        /// <summary>
        /// 이벤트 로그에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지를 가져오거나, 설정합니다.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// 이벤트 로그에 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool WriteEntry(LogEntry log)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 이벤트 로그에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool DeleteLog(dynamic condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 이벤트 로그에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntryValue(dynamic condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 이벤트 로그에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 이벤트 로그에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 이벤트 로그에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Clear(dynamic condition)
        {
            throw new NotImplementedException();
        }
    }
}
