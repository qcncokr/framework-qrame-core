using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics.Adapter
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 컴퓨터의 특정 경로에 TextFile 파일을 생성 하여, 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현합니다.
    /// </summary>
    public class TextFile : ILogAdapter
    {
        /// <summary>
        /// Text 파일의 경로입니다.
        /// </summary>
        private string textDirectoryPath = string.Empty;

        /// <summary>
        /// Text 파일의 경로를 가져오거나, 설정합니다.
        /// </summary>
        public string TextDirectoryPath
        {
            get { return textDirectoryPath; }
            set { textDirectoryPath = value; }
        }

        /// <summary>
        /// Text 파일에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지 입니다.
        /// </summary>
        private string exceptionMessage = "";

        /// <summary>
        /// Text 파일에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지를 가져오거나, 설정합니다.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, Text 파일의 경로를 현재 응용 프로그램 도메인의 시작 경로로 설정합니다.
        /// </summary>
        public TextFile()
        {
            textDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "Diagnostics";
        }

        /// <summary>
        /// 인스턴스 생성시, 지정된 경로로 Text 파일의 경로를 설정합니다.
        /// </summary>
        /// <param name="directoryPath"></param>
        public TextFile(string directoryPath)
        {
            textDirectoryPath = directoryPath;
        }

        /// <summary>
        /// Text 파일에 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool WriteEntry(LogEntry log)
        {
            bool result = false;

            try
            {
                string logDirectory = Path.Combine(textDirectoryPath, log.Level.ToString());
                if (Directory.Exists(logDirectory) == false)
                {
                    Directory.CreateDirectory(logDirectory);
                }

                using (StreamWriter fileWriter = new StreamWriter(Path.Combine(logDirectory + "\\" + log.Id + ".txt"), true, Encoding.UTF8))
                {
                    fileWriter.WriteLine(DateTime.Now.ToString() + " - " + (string)log.Message);
                }

                result = true;
            }
            catch (Exception e)
            {
                result = false;
                exceptionMessage = string.Concat("TextFile 파일에 기록하지 못했습니다. 다음 메시지를 확인하세요.", Environment.NewLine, e.Message);
            }

            return result;
        }

        /// <summary>
        /// (지원안함) Text 파일에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool DeleteLog(dynamic condition)
        {
            throw new NotSupportedException("TextAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// Text 파일에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntryValue(dynamic condition)
        {
            throw new NotSupportedException("TextAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// (지원안함) Text 파일에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries()
        {
            throw new NotSupportedException("TextAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// (지원안함) Text 파일에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate)
        {
            throw new NotSupportedException("TextAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// Text 파일을 삭제합니다.
        /// </summary>
        /// <returns>Text 파일을 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        internal bool Clear()
        {
            bool result = false;
            try
            {
                if (Directory.Exists(textDirectoryPath) == true)
                {
                    Directory.Delete(textDirectoryPath);
                    result = true;
                }
            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat(string.Format("{0} 폴더를 삭제 하지 못했습니다. 다음 메시지를 확인하세요.", textDirectoryPath), Environment.NewLine, e.Message);
            }

            return result;
        }

        /// <summary>
        /// (지원안함) Text 파일에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Clear(dynamic condition)
        {
            throw new NotSupportedException("TextAdapter는 해당 메서드를 지원하지 않습니다.");
        }
    }
}
