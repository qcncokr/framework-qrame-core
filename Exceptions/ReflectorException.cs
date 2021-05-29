using System;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// IndexedDictionary 컬렉션에서 발생하는 사소한 예외에 대한 클래스를 정의 합니다
    /// </summary>
    public class ReflectorException : BaseException
    {
        /// <summary>
        /// ReflectorException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public ReflectorException()
        {

        }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 ReflectorException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">지정된 오류 메시지입니다.</param>
        public ReflectorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 지정된 오류 메시지와 Exception 타입을 사용하여 ReflectorException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">지정된 오류 메시지입니다.</param>
        /// <param name="e">Exception 타입입니다.</param>
        public ReflectorException(string message, Exception e)
            : base(message, e)
        {
            WriteToLog(e.GetType().FullName, message);
        }
    }
}
