using System;
using Qrame.CoreFX.ExtensionMethod;
using Qrame.CoreFX.Diagnostics;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// 예외 업무에 따라 프로젝트에서 새로운 예외 처리기를 만들어야 할 샘플 클래스입니다.
    /// </summary>
    /// <code>
    /// ExceptionFactory.Register("Exception", new ErrorException());
    /// ....
    /// ExceptionFactory.Handle("Error Message", new Exception());
    /// </code>
    public class ErrorException : ExceptionBase
    {
        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public ErrorException()
        {
        }

        /// <summary>
        /// 인스턴스 생성시, Exception Handler의 이름를 입력합니다.
        /// </summary>
        /// <param name="handlerName">Exception Handler의 이름를 입력합니다.</param>
        public ErrorException(string handlerName)
        {
            base.handlerName = handlerName;
        }

        /// <summary>
        /// 예외 처리 Handler에서 처리할 업무를 정의합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        protected override void InternalHandle(string message, Exception e, dynamic arguments)
        {
            LogFactory log = LogFactory.Create(AdapterType.TextFile);
            LogEntry entry = new LogEntry(e);

            entry.Level = EntryLevel.Exception;
            entry.Id = string.Concat(DateTime.Now.ToShortDateString(), "_", entry.Level.ToString());
            entry.Message = string.Concat(message, ", ", entry.Message);

            log.WriteEntry(entry);

            if (this.isExposeException == true)
            {
                throw e.GetOriginalException();
            }
        }
    }
}
