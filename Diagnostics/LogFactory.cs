using System;
using Qrame.CoreFX.Configuration.Settings;
using Qrame.CoreFX.Patterns;
using Qrame.CoreFX.Diagnostics.Adapter;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 LogFactory를 이용 하여 데이터베이스, Adapter, ini 파일, 레지스트리를 데이터 소스로
    /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 동일 하게 제공 하도록 설계되었습니다.
    /// 
    /// 주요 기능으로 다음과 같습니다.
    /// </summary>
    public class LogFactory
    {
        /// <summary>
        /// 스레드에 안전한 싱글턴 인스턴스를 구현하기 위한 레퍼런스 클래스입니다.
        /// </summary>
        private static object syncObject = new object();

        /// <summary>
        /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현한 Adapter를 반환합니다.
        /// </summary>
        /// <param name="adapter">ILogAdapter 인터페이스를 구현하는 타입입니다.</param>
        /// <returns>ILogAdapter 인터페이스를 구현하는 타입의 인스턴스입니다.</returns>
        public static LogFactory Create(ILogAdapter adapter)
        {
            lock (syncObject)
            {
                if (Current == null)
                {
                    LogFactory factory = new LogFactory(adapter);
                    Current = factory;
                }

                return Current;
            }
        }

        /// <summary>
        /// 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현한 Adapter를 반환합니다.
        /// </summary>
        /// <param name="logType">로그 기능을 구현하기 위한 Adapter를 지정하는 열거자입니다.</param>
        /// <returns>ILogAdapter 인터페이스를 구현하는 타입의 인스턴스입니다.</returns>
        public static LogFactory Create(AdapterType logType)
        {
            ILogAdapter LogAdapter = null;

            switch (logType)
            {
                case AdapterType.Ini:
                    LogAdapter = new Ini();
                    break;
                case AdapterType.Registry:
                    LogAdapter = new Registry();
                    break;
                case AdapterType.EventLog:
                    LogAdapter = new EventLog();
                    break;
                case AdapterType.Database:
                    LogAdapter = new Database();
                    break;
                case AdapterType.TextFile:
                    LogAdapter = new TextFile();
                    break;
                default:
                    LogAdapter = new Ini();
                    break;
            }

            return Create(LogAdapter);
        }

        private static LogFactory currentFactory;

        /// <summary>
        /// 인스턴스 생성시, 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현한 Ini Adapter를 반환합니다.
        /// </summary>
        /// <returns>ILogAdapter 인터페이스를 구현하는 타입의 인스턴스입니다.</returns>
        public static LogFactory Create()
        {
            DiagnosticsSetting setting = Singleton<DiagnosticsSetting>.Instance;
            AdapterType adapter = (AdapterType)Enum.Parse(typeof(AdapterType), setting.DefaultAdapter);

            return Create(adapter);
        }

        /// <summary>
        /// AppDomain에 로드중인, IAdapter의 레퍼런스 타입을 포함 하는 LogFactory 인스턴스를 가져오거나, 설정합니다.
        /// </summary>
        public static LogFactory Current
        {
            get { return currentFactory; }
            set { currentFactory = value; }
        }

        /// <summary>
        /// AppDomain에 로드중인, ILogAdapter의 인터페이스를 구현하는 Adapter 인스턴스입니다.
        /// </summary>
        private ILogAdapter logAdapter = null;

        /// <summary>
        /// AppDomain에 로드중인, ILogAdapter의 인터페이스를 구현하는 Adapter 인스턴스를 가져오거나, 설정합니다.
        /// </summary>
        public ILogAdapter LogAdapter
        {
            get { return logAdapter; }
            set { logAdapter = value; }
        }

        /// <summary>
        /// 인스턴스 생성시, 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현한 Ini Adapter를 반환합니다.
        /// </summary>
        public LogFactory() 
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현한 지정된 Adapter를 반환합니다.
        /// </summary>
        public LogFactory(ILogAdapter adapter)
        {
            LogAdapter = adapter;
        }

        /// <summary>
        /// Adapter에서 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        public bool WriteEntry(LogEntry log)
        {
            return LogAdapter.WriteEntry(log);
        }

        /// <summary>
        /// Adapter에서 로그를 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>다양한 로그 정보를 포함하는 LogEntry 타입입니다.</returns>
        public LogEntry GetLogEntry(dynamic condition)
        {
            return LogAdapter.GetEntryValue(condition);
        }

        /// <summary>
        /// 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            if (logAdapter is Ini)
            {
                ((Ini)logAdapter).Clear();
            }

            return false;
        }

        /// <summary>
        /// Adapter에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool DeleteLog(dynamic condition)
        {
            return LogAdapter.DeleteLog(condition);
        }

        /// <summary>
        /// Adapter에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntryValue(dynamic condition)
        {
            return LogAdapter.GetEntryValue(condition);
        }

        /// <summary>
        /// Adapter에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries()
        {
            return LogAdapter.GetEntries();
        }

        /// <summary>
        /// Adapter에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate)
        {
            return LogAdapter.GetEntries(level, fromDate, toDate);
        }

        /// <summary>
        /// Adapter에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Clear(dynamic condition)
        {
            return LogAdapter.Clear(condition);
        }
    }
}
