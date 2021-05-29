using System;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 닷넷 응용 프로그램을 호스팅하고 있는 시스템의 정보를 저장할 엔터티 클래스입니다.
    /// </summary>
    public class EnvironmentInfo
    {
        /// <summary>
        /// 시스템 정보의 스냅샷을 가지고 있는 타이머의 식별자입니다.
        /// </summary>
        internal Guid RecordTimerID;

        /// <summary>
        /// 시스템의 정보를 저장할 EnvironmentInfo클래스의 인스턴스가 생성 시간입니다.
        /// </summary>
        public readonly DateTime CreateDataTime = DateTime.Now;

        /// <summary>
        /// 이 로컬 컴퓨터의 NetBIOS 이름을 가져옵니다.
        /// </summary>
        public readonly string MachineName = Environment.MachineName;

        /// <summary>
        /// Windows 운영 체제에 현재 로그온한 사용자의 이름을 가져옵니다.
        /// </summary>
        public readonly string UserName = Environment.UserName;

        /// <summary>
        /// 운영 체제에 현재 설치된 플랫폼 식별자, 버전 및 서비스 팩의 연결된 문자열 표현을 가져옵니다.
        /// </summary>
        public readonly string OSVersion = Environment.OSVersion.VersionString;

        /// <summary>
        /// 운영 체제의 버전에 대한 문자열 표현을 가져옵니다.
        /// </summary>
        public readonly string EnvironmentVersion = string.Concat(Environment.Version.Major, ".", Environment.Version.Minor, ".", Environment.Version.Build, ".", Environment.Version.Revision);

        /// <summary>
        /// 프로세스 컨텍스트에 매핑되는 실제 메모리의 크기를 가져옵니다.
        /// </summary>
        public readonly long WorkingSet = Environment.WorkingSet;

        /// <summary>
        /// 운영 체제 페이지 파일의 메모리 양을 가져옵니다.
        /// </summary>
        public readonly int SystemPageSize = Environment.SystemPageSize;

        /// <summary>
        /// 현재 컴퓨터의 프로세서 수를 가져옵니다.
        /// </summary>
        public readonly int ProcessorCount = Environment.ProcessorCount;

        /// <summary>
        /// 시스템 시작 이후 경과 시간(밀리초)을 가져옵니다.
        /// </summary>
        public readonly int TickCount = Environment.TickCount;
    }
}
