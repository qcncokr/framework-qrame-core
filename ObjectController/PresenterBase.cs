using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ObjectController
{
    /// <summary>
    /// ASP.NET Presenter를 구현하기 위한 베이스 기능을 제공합니다.
    /// </summary>
    /// <typeparam name="T">Presenter가를 표현하는 View를 가리키는 타입입니다.</typeparam>
    public abstract class PresenterBase<T>
        where T : IView
    {
        /// <summary>
        /// 화면에 대한 데이터바인딩 완료시 발생 되는 이벤트 핸들러입니다.
        /// </summary>
        public event EventHandler OnDataBindCompleted;

        /// <summary>
        /// Presenter가를 표현하는 View를 가리키는 타입입니다.
        /// </summary>
        protected T view;

        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public PresenterBase()
        {
        }

        /// <summary>
        /// View의 기본 동작을 지정하는 생성자입니다.
        /// </summary>
        /// <param name="view">Presenter가를 표현하는 View를 가리키는 타입입니다.</param>
        public PresenterBase(T view)
        {
            this.view = view;

            // Subscribe to Events
            SubscribeToEvents();
        }

        /// <summary>
        /// Presenter가를 표현하는 View를 지정합니다.
        /// </summary>
        /// <param name="view">Presenter가를 표현하는 View를 가리키는 타입입니다.</param>
        public void SetView(T view)
        {
            this.view = view;

            SubscribeToEvents();
        }

        /// <summary>
        /// View의 기본 이벤트 동작을 지정합니다.
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            view.Init += new EventHandler(OnInit);
            view.Load += new EventHandler(OnLoad);
        }

        /// <summary>
        /// 페이지 로드 이벤트에서 데이터 바인딩을 수행후 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</param>
        /// <param name="e">이벤트 데이터가 들어 있는 클래스에 대한 기본 클래스입니다.</param>
        protected virtual void OnLoad(object sender, EventArgs e)
        {
            if (!view.IsPostBack)
            {
                view.DataBind();

                if (OnDataBindCompleted != null)
                {
                    OnDataBindCompleted(sender, e);
                }
            }
        }

        /// <summary>
        /// 페이지 초기 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</param>
        /// <param name="e">이벤트 데이터가 들어 있는 클래스에 대한 기본 클래스입니다.</param>
        protected virtual void OnInit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// View 페이지에 메시지를 표현합니다.
        /// </summary>
        /// <param name="sender">모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</param>
        /// <param name="e">이벤트 데이터가 들어 있는 클래스에 대한 기본 클래스입니다.</param>
        protected virtual void OnDisplayMessage(object sender, MessageEventArgs e)
        {
        }

        /// <summary>
        /// Presenter에 표현되는 View 페이지의 컨트롤 목록을 가져옵니다.
        /// </summary>
        /// <returns>View 페이지의 컨트롤 목록입니다.</returns>
        protected virtual dynamic GetViewControls() { return null; }
    }
}
