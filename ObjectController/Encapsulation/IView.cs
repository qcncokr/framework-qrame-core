using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ObjectController
{
    /// <summary>
    /// ASP.NET View 페이지에 대한 기본 인터페이스입니다.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 초기화 이벤트 핸들러입니다.
        /// </summary>
        event EventHandler Init;

        /// <summary>
        /// 화면 로드 이벤트 핸들러입니다.
        /// </summary>
        event EventHandler Load;

        /// <summary>
        /// PostBack 여부를 가져옵니다.
        /// </summary>
        bool IsPostBack { get; }

        /// <summary>
        /// 유효한 요청인지 여부를 가져옵니다.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// View 페이지에 대한 데이터 바인딩을 수행합니다.
        /// </summary>
        void DataBind();
    }
}
