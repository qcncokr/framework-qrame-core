using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Messages
{
    /// <summary>
    /// WCF 메시지 통신을 수행후 연결을 유지 할지, 종료 할지에 대한 열거자입니다.
    /// </summary>
    public enum ExecutionConnection
    {
        Open,
        Closed
    }
}
