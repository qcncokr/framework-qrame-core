using System;
using System.Collections.Generic;

using Qrame.CoreFX;

namespace Qrame.CoreFX.Diagnostics.Entity
{
    /// <summary>
    /// Qrame.CoreFX.Diagnostics 어셈블리에서, 다양한 로그 정보를 포함하는 LogEntry 타입입니다.
    /// </summary>
    public class LogEntry : BaseEntity
    {
        /// <summary>
        /// 로그 ID값입니다.
        /// </summary>
        private string id = "";

        /// <summary>
        /// 로그 ID값을 가져오거나, 설정합니다.
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 로그를 기록한 시간입니다.
        /// </summary>
        private DateTime logDateTime = DateTime.Now;

        /// <summary>
        /// 로그를 기록한 시간을 가져오거나, 설정합니다.
        /// </summary>
        public DateTime LogDateTime
        {
            get { return logDateTime; }
            set { logDateTime = value; }
        }

        /// <summary>
        /// 로그 예외 메시지값입니다.
        /// </summary>
        private object message = "";

        /// <summary>
        /// 로그 예외 메시지값을 가져오거나, 설정합니다.
        /// </summary>
        public object Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 로그 값에 대한 레벨 수준입니다.
        /// </summary>
        private EntryLevel level = EntryLevel.Exception;

        /// <summary>
        /// 로그 값에 대한 레벨 수준을 가져오거나, 설정합니다.
        /// </summary>
        public EntryLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// 로그 예외 타입명입니다.
        /// </summary>
        private string exceptionType = "";

        /// <summary>
        /// 로그 예외 타입명을 가져오거나, 설정합니다.
        /// </summary>
        public string ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }

        /// <summary>
        /// 로그 추정 정보입니다.
        /// </summary>
        private string stackTrace = "";

        /// <summary>
        /// 로그 추정 정보를 가져오거나, 설정합니다.
        /// </summary>
        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, 응용 프로그램을 실행할 때 나타나는 오류 타입에서, 기본 Exception 예외 레벨, 예외 타입, 예외 메시지, 예외 추적 정보를 설정합니다.
        /// </summary>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        public LogEntry(Exception e)
        {
            Level = EntryLevel.Exception;
            ExceptionType = e.GetType().Name;
            Message = e.Message;
            StackTrace = (e.StackTrace != null && e.StackTrace.Length > 1490) ? e.StackTrace.Substring(0, 1500) : e.StackTrace;
        }
    }
}
