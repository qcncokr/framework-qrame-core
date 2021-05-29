using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// 예외 업무에 따라 프로젝트에서 구현할 새로운 예외 처리기의 기반 기능을 정의합니다.
    /// </summary>
    public class ExceptionBase : IExceptionHandler
    {
        /// <summary>
        /// Exception Handler의 이름입니다.
        /// </summary>
        protected bool isExposeException = false;

        /// <summary>
        /// Exception Handler의 이름을 가져옵니다.
        /// </summary>
        public bool IsExposeException
        {
            get
            {
                return this.isExposeException;
            }
            set
            {
                this.isExposeException = value;
            }
        }

        /// <summary>
        /// Exception Handler의 이름입니다.
        /// </summary>
        protected string handlerName = "";

        /// <summary>
        /// Exception Handler의 이름을 가져옵니다.
        /// </summary>
        public string HandlerName
        {
            get
            {
                return this.handlerName;
            }
        }

        /// <summary>
        /// 예외 처리 Handler에서 처리할 업무를 정의합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        public virtual void Handle(string message, Exception e)
        {
            this.Handle(message, e, null);
        }

        /// <summary>
        /// 예외 처리 Handler에서 처리할 업무를 정의합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        public virtual void Handle(string message, Exception e, dynamic arguments)
        {
            this.InternalHandle(message, e, arguments);
        }

        /// <summary>
        /// 예외 처리 Handler에서 처리할 업무를 정의합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        protected virtual void InternalHandle(string message, Exception e, dynamic arguments)
        {
            throw new NotImplementedException(string.Format("{0} Exception Handler에서 InternalHandle 메서드를 구현하지 않았습니다.", handlerName));
        }

        /// <summary>
        /// 여러 스택 프레임의 정렬된 컬렉션에 해당하는 CallStack 추적을 나타냅니다.
        /// </summary>
        /// <returns>CallStack 추적을 기록한 문자열입니다.</returns>
        protected virtual string GetCallStackInfo()
        {
            StackTrace trace = new StackTrace(true);
            StringBuilder sb = new StringBuilder(512);

            foreach (StackFrame frame in trace.GetFrames())
            {
                sb.AppendLine(string.Concat(
                  "  File: ", frame.GetFileName(),
                  "  Line: ", frame.GetFileLineNumber(),
                  "  Col: ", frame.GetFileColumnNumber(),
                  "  Offset: ", frame.GetILOffset(),
                  "  Method: ", frame.GetMethod().Name));
            }

            return sb.ToString();
        }
    }
}
