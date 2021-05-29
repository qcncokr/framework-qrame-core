using System;
using System.Data;
using System.Configuration;

using System.Runtime.Serialization;

namespace Qrame.CoreFX.Messages
{
    /// <summary>
    /// WCF 요청에 대한 기본적인 응답 여부에 대한 성공여부를 표현하는 열거자입니다.
    /// </summary>
    [DataContract(Namespace = "http://www.tempuri.com/types/")]
    public enum AcknowledgeType
    {
        /// <summary>
        /// WCF 요청에 대한 응답이 실패을 표현합니다.
        /// </summary>
        [EnumMember]
        Failure = 0,

        /// <summary>
        /// WCF 요청에 대한 응답이 성공을 표현합니다.
        /// </summary>
        [EnumMember]
        Success = 1
    }
}
