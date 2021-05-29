
namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// 컴퓨터의 레지스트리 시작 경로를 가리키는 열거자입니다.
    /// </summary>
    public enum RegistryClass
    {
        /// <summary>
        /// 컴퓨터의 HKEY_CLASSES_ROOT 경로입니다.
        /// </summary>
        ClassesRoot,
        /// <summary>
        /// 컴퓨터의 HKEY_CURRENT_CONFIG 경로입니다.
        /// </summary>
        CurrentConfig,
        /// <summary>
        /// 컴퓨터의 HKEY_CURRENT_USER 경로입니다.
        /// </summary>
        CurrentUser,
        /// <summary>
        /// 컴퓨터의 HKEY_LOCAL_MACHINE 경로입니다.
        /// </summary>
        LocalMachine,
        /// <summary>
        /// 컴퓨터의 HKEY_PERFORMANCE_DATA 경로입니다.
        /// </summary>
        PerformanceData,
        /// <summary>
        /// 컴퓨터의 HKEY_USERS 경로입니다.
        /// </summary>
        Users
    }
}
