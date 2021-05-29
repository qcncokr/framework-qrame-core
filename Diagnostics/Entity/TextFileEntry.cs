
using Qrame.CoreFX;

namespace Qrame.CoreFX.Diagnostics.Entity
{
    /// <summary>
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현하기 위한 엔터티 타입입니다.
    /// </summary>
    internal class TextFileEntry : BaseEntity
    {
        /// <summary>
        /// Text 파일의 내용입니다.
        /// </summary>
        private string message = "";

        /// <summary>
        /// Text 파일의 섹션을 가져오거나, 설정합니다.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        
        /// <summary>
        /// 인스턴스 생성시, 빈 문자열의 값으로 초기값을 설정합니다.
        /// </summary>
        public TextFileEntry()
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 지정된 빈 문자열의 값으로 초기값을 설정합니다.
        /// </summary>
        /// <param name="Message">Text 파일의 내용입니다.</param>
        public TextFileEntry(string textMessage)
        {
            message = textMessage;
        }
    }
}
