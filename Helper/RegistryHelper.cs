using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qrame.CoreFX.Cryptography;

namespace Qrame.CoreFX.Helper
{
    public static class RegistryHelper
    {
        private static readonly string InstallationRegistryPath = @"Software\QCN";

        public static DateTime? GetDateTimeValue(string valueName)
        {
            var longVal = GetInt64Value(valueName);
            if (longVal.HasValue)
            {
                return new DateTime(longVal.Value);
            }

            return null;
        }

        public static TimeSpan? GetTimeSpanValue(string valueName)
        {
            return GetTimeSpanValue(Registry.LocalMachine, valueName);
        }

        public static TimeSpan? GetTimeSpanValue(RegistryKey regHiveKey, string valueName)
        {
            var longVal = GetInt64Value(regHiveKey, valueName);
            if (longVal.HasValue)
            {
                return new TimeSpan(longVal.Value);
            }
            return null;
        }

        public static long? GetInt64Value(string valueName)
        {
            return GetInt64Value(Registry.LocalMachine, valueName);
        }

        public static long? GetInt64Value(RegistryKey regHiveKey, string valueName)
        {
            return GetObjectValue(regHiveKey, InstallationRegistryPath, valueName) as long?;
        }

        public static int? GetInt32Value(string valueName)
        {
            return GetInt32Value(Registry.LocalMachine, valueName);
        }

        public static int? GetInt32Value(RegistryKey regHiveKey, string valueName)
        {
            return GetObjectValue(regHiveKey, InstallationRegistryPath, valueName) as int?;
        }

        public static bool? GetBooleanValue(string valueName)
        {
            var longVal = GetInt64Value(valueName);
            if (longVal.HasValue)
            {
                return (longVal != 0);
            }
            return null;
        }

        public static string GetStringValue(string valueName)
        {
            return GetStringValue(InstallationRegistryPath, valueName);
        }

        public static string GetStringValue(string valueName, bool isEncrypt)
        {
            string Result = GetStringValue(InstallationRegistryPath, valueName);

            if (string.IsNullOrEmpty(Result) == true)
            {
                return Result;
            }

            if (isEncrypt == true)
            {
                Result = AES256Crypto.Decrypt(Result);
            }

            return Result;
        }

        public static string GetStringValue(string regKeyPath, string valueName)
        {
            return GetObjectValue(Registry.LocalMachine, regKeyPath, valueName) as string;
        }

        public static string GetStringValue(string regKeyPath, string valueName, bool isEncrypt)
        {
            string Result = GetObjectValue(Registry.LocalMachine, regKeyPath, valueName) as string;

            if (string.IsNullOrEmpty(Result) == true)
            {
                return Result;
            }

            if (isEncrypt == true)
            {
                Result = AES256Crypto.Decrypt(Result);
            }

            return Result;
        }

        private static object GetObjectValue(RegistryKey regHiveKey, string regKeyPath, string valueName)
        {
            try
            {
                using (var SubKey = regHiveKey.OpenSubKey(regKeyPath, false))
                {
                    if (SubKey == null)
                    {
                        return null;
                    }
                    return SubKey.GetValue(valueName);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool SetValue(string valueName, DateTime value)
        {
            return SetInt64Value(valueName, value.Ticks);
        }

        public static bool SetValue(string valueName, TimeSpan value)
        {
            return SetInt64Value(valueName, value.Ticks);
        }

        public static bool SetValue(string valueName, bool value)
        {
            return SetInt64Value(valueName, value ? 1L : 0L);
        }

        public static bool SetValue(string valueName, string value)
        {
            return SetValue(InstallationRegistryPath, valueName, value);
        }

        public static bool SetValue(string valueName, string value, bool isEncrypt)
        {
            return SetValue(InstallationRegistryPath, valueName, isEncrypt == true ? AES256Crypto.Encrypt(value) : value);
        }

        public static bool SetValue(string regKeyPath, string valueName, string value)
        {
            return SetObjectValue(regKeyPath, valueName, value, RegistryValueKind.String);
        }

        public static bool SetInt32Value(string valueName, int value)
        {
            return SetObjectValue(InstallationRegistryPath, valueName, value, RegistryValueKind.DWord);
        }

        public static bool SetInt64Value(string valueName, long value)
        {
            return SetObjectValue(InstallationRegistryPath, valueName, value, RegistryValueKind.QWord);
        }

        private static bool SetObjectValue(string regKeyPath, string valueName, object value, RegistryValueKind valueKind)
        {
            try
            {
                using (var SubKey = Registry.LocalMachine.CreateSubKey(regKeyPath))
                {
                    if (SubKey == null)
                    {
                        return false;
                    }
                    SubKey.SetValue(valueName, value, valueKind);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteValue(string valueName)
        {
            DeleteValue(InstallationRegistryPath, valueName);
        }

        public static void DeleteValue(string regKeyPath, string valueName)
        {
            try
            {
                using (var SubKey = Registry.LocalMachine.OpenSubKey(regKeyPath, true))
                {
                    if (SubKey == null)
                    {
                        return;
                    }
                    SubKey.DeleteValue(valueName, false);
                }
            }
            catch
            {
            }
        }
    }
}
