using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ObjectController
{
    /// <summary>
    /// View 페이지를 랜더링하는 이벤트 데이터가 들어 있는 클래스입니다.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// View 페이지를 랜더링하는 결과에 대한 열거자를 가져오거나 설정합니다.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// View 페이지를 랜더링하는 결과에 대한 추가 메시지를 가져오거나 설정합니다.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// View 페이지를 랜더링하는 결과에 대한 예외정보를 가져오거나 설정합니다.
        /// </summary>
        public Exception CurrrentException { get; set; }

        /// <summary>
        /// View 페이지를 랜더링하는 결과에 대한 기록여부를 가져오거나 설정합니다.
        /// </summary>
        public bool IsLog { get; set; }
    }
}
