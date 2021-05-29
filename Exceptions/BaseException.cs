using System;
using System.IO;
using System.Text;

namespace Qrame.CoreFX.Exceptions
{
    /// <summary>
    /// 닷넷 응용 프로그램에서 발생하는 사소한 예외에 대한 기본 클래스를 정의 합니다
    /// </summary>
    public class BaseException : ApplicationException
    {
        /// <summary>
        /// BaseException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public BaseException()
        {
        }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 BaseException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">지정된 오류 메시지입니다.</param>
        public BaseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 지정된 오류 메시지와 Exception 타입을 사용하여 BaseException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">지정된 오류 메시지입니다.</param>
        /// <param name="e">Exception 타입입니다.</param>
        public BaseException(string message, Exception e)
            : base(message, e)
        {
            WriteToLog(e.GetType().FullName, message);
        }

        /// <summary>
        /// Database 작업 수행중 에러 발생시 로그 파일에 기록 합니다.
        /// </summary>
        /// <param name="fileName">에러 메시지를 기록할 파일 경로입니다.</param>
        /// <param name="message">Database 에러 메시지입니다.</param>
        protected void WriteToLog(string fileName, string message)
        {
            string logDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (Directory.Exists(logDirectory) == false)
            {
                Directory.CreateDirectory(logDirectory);
            }

            using (StreamWriter fileWriter = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), true, Encoding.UTF8))
            {
                fileWriter.WriteLine(DateTime.Now.ToString() + " - " + message);
            }
        }
    }
}
