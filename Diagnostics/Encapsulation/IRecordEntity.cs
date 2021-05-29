using System;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 정의합니다.
    /// </summary>
    public interface IRecordEntity
    {
        /// <summary>
        /// 엔터티 클래스의 스냅샷을 가지고 있는 타이머의 식별자입니다.
        /// </summary>
        Guid RecordTimerID
        {
            get;
            set;
        }
    }
}
