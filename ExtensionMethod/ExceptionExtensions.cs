using System;
using System.Collections.Generic;
using System.Linq;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Exception 클래스를 대상으로 동작하는 확장 메서드 클래스입니다.
    /// 
    /// 주요 기능으로 다음과 같습니다.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 현재 예외를 발생시키는 최상위 예외 정보를 가져옵니다.
        /// </summary>
        /// <param name="@this">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        /// <returns>현재 예외를 발생시키는 최상위 Exception 타입입니다.</returns>
        public static Exception GetOriginalException(this Exception @this)
        {
            if (@this.InnerException == null)
            {
                return @this;
            }

            return @this.InnerException.GetOriginalException();
        }

        /// <summary>
        /// 현재 예외를 발생시키는 최상위 예외 메시지를 가져옵니다.
        /// </summary>
        /// <param name="@this">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        /// <returns>현재 예외를 발생시키는 최상위 메시지입니다.</returns>
        public static string GetOriginalMessage(this Exception @this)
        {
            if (@this.InnerException == null)
            {
                return @this.Message;
            }
            else
            {
                return @this.InnerException.GetOriginalMessage();
            }
        }

        /// <summary>
        /// 현재 예외를 발생시키는 모든 예외 메시지를 가져옵니다.
        /// </summary>
        /// <param name="@this">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        /// <returns>현재 예외를 발생시키는 모든 메시지 컬렉션입니다.</returns>
        public static IEnumerable<string> GetAllMessages(this Exception @this)
        {
            return @this != null ? new List<string>(@this.InnerException.GetAllMessages()) { @this.Message } : Enumerable.Empty<string>();
        }

        /// <summary>
        /// 현재 예외를 발생시키는 최상위 호출 스택을 가져옵니다.
        /// </summary>
        /// <param name="@this">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        /// <returns>현재 예외를 발생시키는 최상위 호출 스택입니다.</returns>
        public static string GetOriginalStackTrace(this Exception @this)
        {
            if (@this.InnerException == null)
            {
                return @this.StackTrace;
            }
            else
            {
                return @this.InnerException.GetOriginalStackTrace();
            }
        }

        /// <summary>
        /// 현재 예외를 발생시키는 최상위 응용 프로그램 또는 개체의 이름을 가져옵니다.
        /// </summary>
        /// <param name="@this">응용 프로그램을 실행할 때 나타나는 오류를 나타냅니다.</param>
        /// <returns>현재 예외를 발생시키는 최상위 응용 프로그램 또는 개체입니다.</returns>
        public static string GetOriginalSource(this Exception @this)
        {
            if (@this.InnerException == null)
            {
                return @this.Source;
            }
            else
            {
                return @this.InnerException.GetOriginalSource();
            }
        }
    }
}
