using System;
using System.Collections.Generic;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// 예외 업무에 따라 프로젝트에서 구현한 예외 처리기를 관리하는 클래스입니다.
    /// </summary>
    /// <code>
    /// ExceptionFactory.Register("", new DefaultExceptionHandler());
    /// ....
    /// ExceptionFactory.Handle("Error Message", new Exception());
    /// </code>
    public class ExceptionFactory
    {
        /// <summary>
        /// Handler들을 관리할 컬렉션입니다.
        /// </summary>
        private static IDictionary<string, IExceptionHandler> exceptionHandlers = new Dictionary<string, IExceptionHandler>();

        /// <summary>
        /// 기본 예외 Handler의 레퍼런스입니다.
        /// </summary>
        private static IExceptionHandler handler;

        /// <summary>
        /// 스레드에 안전한 싱글턴 인스턴스를 구현하기 위한 레퍼런스 클래스입니다.
        /// </summary>
        private static object syncObject = new object();
        
        /// <summary>
        /// 인스턴스 생성시, 마지막으로 등록된 Handler의 레퍼런스를 컬렉션에 추가합니다.
        /// </summary>
        static ExceptionFactory()
        {
            exceptionHandlers[""] = handler;
        }

        /// <summary>
        /// Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        public static void Handle(string message, Exception e)
        {
            InternalHandle(message, e, null, null);
        }

        /// <summary>
        /// Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="Arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        public static void Handle(string message, Exception e, dynamic Arguments)
        {
            InternalHandle(message, e, null, Arguments);
        }

        /// <summary>
        /// Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="handlerKey">Handler의 컬렉션 키입니다.</param>
        public static void Handle(string message, Exception e, string handlerKey)
        {
            InternalHandle(message, e, handlerKey, null);
        }

        /// <summary>
        /// Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="handlerKey">Handler의 컬렉션 키입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        public static void Handle(string message, Exception e, string handlerKey, dynamic arguments)
        {
            InternalHandle(message, e, handlerKey, arguments);
        }

        /// <summary>
        /// ExceptionFactory에 등록된 모든 Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        public static void DoHandle(string message, Exception e)
        {
            DoHandle(message, e, null);
        }

        /// <summary>
        /// ExceptionFactory에 등록된 모든 Handler에게 응용 프로그램을 실행할 때 나타나는 오류를 가지고 예외 처리를 호출합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        public static void DoHandle(string message, Exception e, dynamic arguments)
        {
            foreach (KeyValuePair<string, IExceptionHandler> Handler in exceptionHandlers)
            {
                ((IExceptionHandler)Handler.Value).Handle(message, e, arguments);
            }
        }
        
        /// <summary>
        /// Handler들을 관리할 컬렉션에 추가하지 않고, 기본 예외 Handler의 레퍼런스를 설정합니다.
        /// </summary>
        /// <param name="exceptionProvider">예외 처리 Handler입니다.</param>
        public static void Init(IExceptionHandler exceptionProvider)
        {
            lock (syncObject)
            {
                handler = exceptionProvider;
            }
        }

        /// <summary>
        /// 예외 처리 Handler에서 처리할 업무를 정의합니다.
        /// </summary>
        /// <param name="message">예외 발생시 Handler에서 처리할 예외 메시지입니다.</param>
        /// <param name="e">응용 프로그램을 실행할 때 나타나는 오류 타입입니다.</param>
        /// <param name="handlerKey">Handler의 컬렉션 키입니다.</param>
        /// <param name="arguments">예외 발생시 Handler에서 처리할 추가 정보입니다.</param>
        private static void InternalHandle(string message, Exception e, string handlerKey, dynamic arguments)
        {
            if (handlerKey == null)
            {
                handler.Handle(message, e, arguments);
            }
            else
            {
                if (exceptionHandlers.ContainsKey(handlerKey) == false)
                {
                    throw new ArgumentException(string.Format("{0}키는 ExceptionFactory에 등록 되지 않은 예외 처리 Handler입니다.", handlerKey));
                }

                exceptionHandlers[handlerKey].Handle(message, e, arguments);
            }
        }

        /// <summary>
        /// ExceptionFactory에 응용프로그램 코드의 문제를 처리 할 수 있도록 예외 처리 Handler를 등록합니다.
        /// </summary>
        /// <param name="handlerKey">Handler의 컬렉션 키입니다.</param>
        /// <param name="exceptionProvider">예외 처리 Handler입니다.</param>
        public static void Register(string handlerKey, IExceptionHandler exceptionProvider)
        {
            lock (syncObject)
            {
                handler = exceptionProvider;

                if (exceptionHandlers.ContainsKey(handlerKey) == false)
                {
                    exceptionHandlers[handlerKey] = exceptionProvider;
                }
            }
        }
    }
}
