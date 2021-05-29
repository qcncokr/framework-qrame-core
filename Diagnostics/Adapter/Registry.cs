using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Qrame.CoreFX.Diagnostics.Entity;

namespace Qrame.CoreFX.Diagnostics.Adapter
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 컴퓨터의 레지스트리에 응용프로그램 코드의 실행을 추적 할 수 있도록 로그 기능을 구현합니다.
    /// </summary>
    public class Registry : ILogAdapter
    {
        /// <summary>
        /// 인스턴스 생성시, 컴퓨터의 HKEY_CLASSES_ROOT 경로로, 레지스트리의 시작 경로를 설정합니다.
        /// </summary>
        public Registry()
        {
        }

        /// <summary>
        /// 인스턴스 생성시, 지정된 경로로 레지스트리의 시작 경로를 설정합니다.
        /// </summary>
        /// <param name="defaultClass"></param>
        public Registry(RegistryClass defaultClass)
        {
            registryClass = defaultClass;
        }

        /// <summary>
        /// 컴퓨터의 레지스트리 시작 경로입니다.
        /// </summary>
        private RegistryClass registryClass = RegistryClass.ClassesRoot;

        /// <summary>
        /// 컴퓨터의 레지스트리 시작 경로를 가져오거나, 설정합니다.
        /// </summary>
        public RegistryClass Class
        {
            get { return registryClass; }
        }

        /// <summary>
        /// 레지스트리 데이터를 기록 할 때, 적용하는 옵션입니다.
        /// </summary>
        private RegistryKind valueKind = RegistryKind.String;

        /// <summary>
        /// 레지스트리 데이터를 기록 할 때, 적용하는 옵션을 가져오거나, 설정합니다.
        /// </summary>
        public RegistryKind ValueKind
        {
            get { return valueKind; }
            set { valueKind = value; }
        }

        /// <summary>
        /// 마지막 레지스트리 작업 경로입니다.
        /// </summary>
        private string registryPath = "";
        
        /// <summary>
        /// 레지스트리 데이터를 조회하거나 가져올 때, 발생하는 예외 정보입니다.
        /// </summary>
        private string exceptionMessage = "";

        /// <summary>
        /// 레지스트리 데이터를 조회하거나 가져올 때, 발생하는 예외 정보를 가져옵니다.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }

        /// <summary>
        /// 인스턴스시 설정된 레지스트리 데이터를 기록 할 때, 적용하는 옵션을 RegistryValueKind로 반환합니다.
        /// </summary>
        /// <returns>레지스트리에 값을 저장할 때 사용할 데이터 형식을 지정하거나 레지스트리 값의 데이터 형식을 나타냅니다.</returns>
        private RegistryValueKind ToRegistryValueKind()
        {
            RegistryValueKind result = RegistryValueKind.String;

            switch (valueKind)
            {
                case RegistryKind.Binary:
                    result = RegistryValueKind.Binary;
                    break;
                case RegistryKind.DWord:
                    result = RegistryValueKind.DWord;
                    break;
                case RegistryKind.ExpandString:
                    result = RegistryValueKind.ExpandString;
                    break;
                case RegistryKind.MultiString:
                    result = RegistryValueKind.MultiString;
                    break;
                case RegistryKind.None:
                    result = RegistryValueKind.None;
                    break;
                case RegistryKind.QWord:
                    result = RegistryValueKind.QWord;
                    break;
                case RegistryKind.String:
                    result = RegistryValueKind.String;
                    break;
                default:
                    result = RegistryValueKind.String;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 인스턴스시 설정된 컴퓨터의 시작 경로 옵션을 RegistryKey로 반환합니다.
        /// </summary>
        /// <returns>Windows 레지스트리의 키 수준 노드를 나타냅니다.</returns>
        private RegistryKey ToRegistryKey()
        {
            RegistryKey result = Microsoft.Win32.Registry.LocalMachine;

            switch (registryClass)
            {
                case RegistryClass.ClassesRoot:
                    result = Microsoft.Win32.Registry.LocalMachine;
                    break;
                case RegistryClass.CurrentConfig:
                    result = Microsoft.Win32.Registry.CurrentConfig;
                    break;
                case RegistryClass.CurrentUser:
                    result = Microsoft.Win32.Registry.CurrentUser;
                    break;
                case RegistryClass.LocalMachine:
                    result = Microsoft.Win32.Registry.LocalMachine;
                    break;
                case RegistryClass.PerformanceData:
                    result = Microsoft.Win32.Registry.PerformanceData;
                    break;
                case RegistryClass.Users:
                    result = Microsoft.Win32.Registry.Users;
                    break;
                default:
                    result = Microsoft.Win32.Registry.LocalMachine;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 로그를 기록합니다.
        /// </summary>
        /// <param name="log">다양한 로그 정보를 포함하는 LogEntry 타입입니다.</param>
        /// <returns>LogEntry 타입을 이용하여 로그를 정상적으로 기록하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool WriteEntry(LogEntry log)
        {
            bool result = false;

            if (string.IsNullOrEmpty(registryPath) == true)
            {
                throw new NotSupportedException("Registry 경로가 입력 되지 않았습니다.");
            }

            RegistryKey registryRoot = ToRegistryKey();
            RegistryKey registrySub = registryRoot.OpenSubKey(registryPath, true);
            RegistryKey registryEntryLevel = null;

            try
            {
                if (registrySub == null)
                {
                    registrySub = registryRoot.CreateSubKey(registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }

                registryEntryLevel = registrySub.CreateSubKey(log.Level.ToString(), RegistryKeyPermissionCheck.ReadWriteSubTree);

                registrySub.SetValue(log.Id, log.Message, ToRegistryValueKind());
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                exceptionMessage = string.Concat("입력하신 Registry 경로에 맞는 SubKey를 찾거나, 만들지 못했습니다. 경로를 확인 해야합니다.", Environment.NewLine, e.Message);
            }
            finally
            {
                registryEntryLevel.Close();
                registrySub.Close();
                registryRoot.Close();
            }

            return result;
        }

        /// <summary>
        /// 레지스트리에서 조건식을 분석하여 로그를 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool DeleteLog(dynamic condition)
        {
            RegistryEntry logCondition = null;
            if (condition is RegistryEntry)
            {
                logCondition = condition;
            }
            else
            {
                throw new ArgumentException("DeleteLog 메서드의 파라메터는 RegistryEntry 타입이어야 합니다.");
            }

            bool result = false;

            if (string.IsNullOrEmpty(registryPath) == true)
            {
                throw new NotSupportedException("Registry 경로가 입력 되지 않았습니다.");
            }

            RegistryKey registryRoot = ToRegistryKey();
            RegistryKey registrySub = registryRoot.OpenSubKey(registryPath, true);
            RegistryKey registryEntryLevel = null;

            try
            {
                registryEntryLevel = registrySub.OpenSubKey(logCondition.EntryLevel.ToString(), RegistryKeyPermissionCheck.ReadWriteSubTree);

                registrySub.DeleteValue(logCondition.KeyName, false);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                exceptionMessage = string.Concat("입력하신 Registry 경로에 맞는 SubKey를 찾을 수 없습니다. 경로를 확인 해야합니다.", Environment.NewLine, e.Message);
            }
            finally
            {
                registryEntryLevel.Close();
                registrySub.Close();
                registryRoot.Close();
            }

            return result;
        }

        /// <summary>
        /// 레지스트리에서 조건식을 분석하여 로그 항목값을 가져옵니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntryValue(dynamic condition)
        {
            RegistryEntry logCondition = null;
            if (condition is RegistryEntry)
            {
                logCondition = condition;
            }
            else
            {
                throw new ArgumentException("GetEntryValue 메서드의 파라메터는 RegistryEntry 타입이어야 합니다.");
            }

            object result = null;

            if (string.IsNullOrEmpty(registryPath) == true)
            {
                throw new NotSupportedException("Registry 경로가 입력 되지 않았습니다.");
            }

            RegistryKey registryRoot = ToRegistryKey();
            RegistryKey registrySub = registryRoot.OpenSubKey(registryPath, true);
            RegistryKey registryEntryLevel = null;

            try
            {
                registryEntryLevel = registrySub.OpenSubKey(logCondition.EntryLevel.ToString(), RegistryKeyPermissionCheck.ReadWriteSubTree);

                result = registrySub.GetValue(logCondition.KeyName, null, RegistryValueOptions.None);
            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat("입력하신 Registry 경로에 맞는 SubKey를 찾을 수 없습니다. 경로를 확인 해야합니다.", Environment.NewLine, e.Message);
            }
            finally
            {
                registryEntryLevel.Close();
                registrySub.Close();
                registryRoot.Close();
            }

            return result;
        }

        /// <summary>
        /// 레지스트리에서 전체 로그 항목값을 가져옵니다.
        /// </summary>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries()
        {
            List<RegistryEntry> result = new List<RegistryEntry>();

            if (string.IsNullOrEmpty(registryPath) == true)
            {
                throw new NotSupportedException("Registry 경로가 입력 되지 않았습니다.");
            }

            RegistryKey registryRoot = ToRegistryKey();
            RegistryKey registrySub = registryRoot.OpenSubKey(registryPath, true);
            RegistryKey registryEntryLevel = null;

            try
            {
                RegistryEntry Entry = null;

                foreach (string keyName in registrySub.GetSubKeyNames())
                {
                    registryEntryLevel = registrySub.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    foreach (string valueName in  registryEntryLevel.GetValueNames())
                    {
                        Entry = new RegistryEntry()
                        {
                            KeyName = valueName,
                            KeyValue = registryEntryLevel.GetValue(valueName, null, RegistryValueOptions.None),
                            EntryLevel = (EntryLevel)Enum.Parse(typeof(EntryLevel), keyName)
                        };

                        result.Add(Entry);                        
                    }
                }
            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat("입력하신 Registry 경로에 맞는 SubKey를 찾을 수 없습니다. 경로를 확인 해야합니다.", Environment.NewLine, e.Message);
            }
            finally
            {
                registryEntryLevel.Close();
                registrySub.Close();
                registryRoot.Close();
            }

            return result;
        }

        /// <summary>
        /// 레지스트리에서 로그 항목값에 대한 레벨 수준, 일자 기간항목값을 기준으로 런타임에 확인될 작업이 포함된 개체를 가져옵니다.
        /// </summary>
        /// <param name="level">로그 항목값에 대한 레벨 수준입니다.</param>
        /// <param name="fromDate">로그 항목값이 기록된 시작일자 범위 항목값입니다.</param>
        /// <param name="toDate">로그 항목값이 기록된 완료일자 범위 항목값입니다.</param>
        /// <returns>런타임에 확인될 작업이 포함된 개체입니다.</returns>
        public virtual dynamic GetEntries(EntryLevel level, DateTime fromDate, DateTime toDate)
        {
            throw new NotSupportedException("RegistryAdapter는 해당 메서드를 지원하지 않습니다.");
        }

        /// <summary>
        /// 레지스트리에서 전체 로그 항목값을 삭제합니다.
        /// </summary>
        /// <param name="condition">런타임에 확인될 작업이 포함된 개체입니다.</param>
        /// <returns>로그를 정상적으로 삭제하면 true를, 아니면 false를 반환합니다.</returns>
        public virtual bool Clear(dynamic condition)
        {
            bool result = false;

            if (string.IsNullOrEmpty(registryPath) == true)
            {
                throw new NotSupportedException("Registry 경로가 입력 되지 않았습니다.");
            }

            RegistryKey registryRoot = ToRegistryKey();
            RegistryKey registrySub = registryRoot.OpenSubKey(registryPath, true);

            try
            {
                foreach (string KeyName in registrySub.GetSubKeyNames())
                {
                    registrySub.DeleteSubKeyTree(KeyName, true);
                }

                result = true;
            }
            catch (Exception e)
            {
                exceptionMessage = string.Concat("지정된 하위 키와 자식 하위 키를 재귀적으로 삭제하는 과정에서 예외가 발생했습니다.", Environment.NewLine, e.Message);
            }
            finally
            {
                registrySub.Close();
                registryRoot.Close();
            }

            return result;
        }
    }
}
