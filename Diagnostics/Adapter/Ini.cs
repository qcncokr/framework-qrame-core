using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics.Adapter
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 컴퓨터의 특정 경로에 ini 파일을 생성 하여, 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현합니다.
    /// </summary>
    public class Ini : ILogAdapter
    {
        /// <summary>
        /// ini 파일의 경로입니다.
        /// </summary>
        private string iniFilePath = string.Empty;

        /// <summary>
        /// ini 파일의 경로를 가져오거나, 설정합니다.
        /// </summary>
        public string IniFilePath
        {
            get { return iniFilePath; }
            set { iniFilePath = value; }
        }

        /// <summary>
        /// ini 파일에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지 입니다.
        /// </summary>
        private string exceptionMessage = "";

        /// <summary>
        /// ini 파일에서 로그를 가져오거나, 설정할 때 발생하는 마지막 예외 메시지를 가져오거나, 설정합니다.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, ini 파일의 경로를 현재 응용 프로그램 도메인의 시작 경로로 설정합니다.
        /// </summary>
        public Ini()
        {
            iniFilePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName, ".ini");
        }

        /// <summary>
        /// ini 파일에 섹션과 키로 검색하여 값을 저장합니다.
        /// </summary>
        /// <param name="lpAppName">섹션명입니다.</param>
        /// <param name="lpKeyName">키값명입니다.</param>
        /// <param name="lpString">저장 할 문자열입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns>값을 얻은 성공 여부입니다.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// ini 파일에 섹션을 저장합니다.
        /// </summary>
        /// <param name="lpAppName">섹션명입니다.</param>
        /// <param name="lpString">키=값으로 되어 있는 문자열 데이터입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        /// <summary>
        /// ini 파일에 섹션과 키로 검색하여 값을 Integer형으로 읽어 옵니다.
        /// </summary>
        /// <param name="lpAppName">섹션명입니다.</param>
        /// <param name="lpKeyName">키값명입니다.</param>
        /// <param name="nDefalut">기본값입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns>입력된 값입니다. 만약 해당 키로 검색 실패시 기본 값으로 대체 됩니다.</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefalut, string lpFileName);

        /// <summary>
        /// ini 파일에 섹션과 키로 검색하여 값을 문자열형으로 읽어 옵니다.
        /// </summary>
        /// <param name="lpAppName">섹션명입니다.</param>
        /// <param name="lpKeyName">키값명입니다.</param>
        /// <param name="lpDefault">기본 문자열입니다.</param>
        /// <param name="lpReturnedString">가져온 문자열입니다.</param>
        /// <param name="nSize">문자열 버퍼의 크기입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns>가져온 문자열 크기입니다.</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        /// <summary>
        /// ini 파일에 섹션으로 검색하여 키와 값을 Pair형태로 가져옵니다.
        /// </summary>
        /// <param name="lpAppName">섹션명입니다.</param>
        /// <param name="lpPairVaules">Pair한 키와 값을 담을 배열입니다.</param>
        /// <param name="nSize">배열의 크기입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns>읽어온 바이트 수 입니다.</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpPairVaules, int nSize, string lpFileName);

        /// <summary>
        /// ini 파일의 섹션을 가져옵니다.
        /// </summary>
        /// <param name="lpSections">섹션의 리스트를 직렬화하여 담을 배열입니다.</param>
        /// <param name="nSize">배열의 크기입니다.</param>
        /// <param name="lpFileName">파일 이름입니다.</param>
        /// <returns>읽어온 바이트 수 입니다.</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSectionNames(byte[] lpSections, int nSize, string lpFileName);

        /// <summary>
        /// 인스턴스 생성시, 지정된 경로로 ini 파일의 경로를 설정합니다.
        /// </summary>
        /// <param name="filePath"></param>
        public Ini(string filePath)
        {
            iniFilePath = filePath;
        }

        /// <summary>
        /// ini 파일에서 값을 조회합니다.
        /// </summary>
        /// <param name="section">ini 파일의 섹션입니다.</param>
        /// <param name="keyName">ini 파일의 키입니다.</param>
        /// <returns>읽어온 값 입니다.</returns>
        public string GetValue(string section, string keyName)
        {
            StringBuilder Builder = new StringBuilder(255);
            GetPrivateProfileString(section, keyName, "", Builder, 255, iniFilePath);
            return Builder.ToString();
        }

        /// <summary>
        /// ini 파일에서 값을 조회합니다.
        /// </summary>
        /// <param name="section">ini 파일의 섹션입니다.</param>
        /// <param name="keyName">ini 파일의 키입니다.</param>
        /// <param name="defaultValue">읽어온 값이 없을 경우 지정한 값을 반환합니다.</param>
        /// <returns>읽어온 값 입니다. 읽어온 값이 없을 경우 DefaultValue을 반환합니다.</returns>
        public string GetValue(string section, string keyName, string defaultValue)
        {
            StringBuilder Builder = new StringBuilder(255);

            int i = GetPrivateProfileString(section, keyName, "", Builder, 255, iniFilePath);

            if (Builder.ToString().Length < 1)
            {
                return defaultValue;
            }
            else
            {
                return Builder.ToString();
            }
        }

        /// <summary>
        /// ini 파일에서 값을 기록합니다.
        /// </summary>
        /// <param name="section">ini 파일의 섹션입니다.</param>
        /// <param name="keyName">ini 파일의 키입니다.</param>
        /// <param name="value">ini 파일에 기록할 값입니다.</param>
        public void SetValue(string section, string keyName, string value)
        {
            WritePrivateProfileString(section, keyName, value, iniFilePath);
        }

        /// <summary>
        /// ini 파일에 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool WriteEntry(LogEntry log)
        {
            bool result = false;

            try
            {
                SetValue(log.Level.ToString(), log.Id, (string)log.Message);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                exceptionMessage = string.Concat("Ini 파일에 기록하지 못했습니다. 다음 메시지를 확인하세요.", Environment.NewLine, e.Message);
            }

            return result;
        }

        /// <summary>
        /// (지원안함) ini 파일에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool DeleteLog(dynamic condition)
        {
            throw new NotSupportedException("IniAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// ini 파일에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntryValue(dynamic condition)
        {
            IniEntry logCondition = null;
            if (condition is IniEntry)
            {
                logCondition = condition;
            }
            else
            {
                throw new ArgumentException("GetEntryValue 메서드의 파라메터는 IniEntry 타입이어야 합니다.");
            }

            string result = null;

            try
            {
                result = GetValue(logCondition.KeyValue, logCondition.Section, logCondition.DefaultValue);

            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat("Ini 파일에서 값을 가져오지 못했습니다. 다음 메시지를 확인하세요.", Environment.NewLine, e.Message);
            }

            return result;
        }

        /// <summary>
        /// (지원안함) ini 파일에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries()
        {
            throw new NotSupportedException("IniAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// (지원안함) ini 파일에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate)
        {
            throw new NotSupportedException("IniAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// (지원안함) ini 파일을 삭제합니다.
        /// </summary>
        /// <returns>ini 파일을 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        internal bool Clear()
        {
            throw new NotSupportedException("IniAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// ini 파일에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Clear(dynamic condition)
        {
            bool result = false;
            try
            {
                if (File.Exists(iniFilePath) == true)
                {
                    File.Delete(iniFilePath);
                    result = true;
                }
            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat("Ini 파일을 삭제 하지 못했습니다. 다음 메시지를 확인하세요.", Environment.NewLine, e.Message);
            }

            return result;           
        }
    }
}
