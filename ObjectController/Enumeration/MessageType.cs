using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ObjectController
{
    /// <summary>
    ///  View 페이지를 랜더링하는 결과에 대한 열거자입니다.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 성공을 표현합니다.
        /// </summary>
        Success,

        /// <summary>
        /// 에러를 표현합니다.
        /// </summary>
        Error,

        /// <summary>
        /// 경고를 표현합니다.
        /// </summary>
        Warning
    }
}
