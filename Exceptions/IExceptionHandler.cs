using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 발생 할 수 있는 다양한 예외 상황에 맞는 Handler를 등록하여,
    /// 응용프로그램 코드의 문제를 처리 할 수 있도록 예외 처리 기능을 정의합니다.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// 메시지와 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외를 정의합니다.
        /// </summary>
        /// <param name="Message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        void Handle(string message, Exception e);

        /// <summary>
        /// 메시지와 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외를 정의합니다.
        /// </summary>
        /// <param name="Message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="Arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        void Handle(string message, Exception e, dynamic arguments);
    }
}
