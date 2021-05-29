using System.Text;
using System.Runtime.InteropServices;

namespace Qrame.CoreFX.Helper
{
  public static class IniHelper
  {
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    
    public static string GetValue(string iniPath, string Section, string Key)
    {
      StringBuilder temp = new StringBuilder(255);
      int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
      return temp.ToString();
    }
    
    public static void SetValue(string iniPath, string Section, string Key, string Value)
    {
      WritePrivateProfileString(Section, Key, Value, iniPath);
    }
  }
}
