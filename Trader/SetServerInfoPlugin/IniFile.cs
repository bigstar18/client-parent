// Decompiled with JetBrains decompiler
// Type: SetServerInfoPlugin.IniFile
// Assembly: SetServerInfoPlugin, Version=3.0.8.0, Culture=neutral, PublicKeyToken=null
// MVID: E04F003E-2DD5-4E4F-8F62-E41AF4AB517D
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\SetServerInfoPlugin.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SetServerInfoPlugin
{
  public class IniFile
  {
    public string Path;

    public IniFile(string inipath)
    {
      this.Path = this.GetIniFullPath(inipath);
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public void IniWriteValue(string Section, string Key, string Value)
    {
      IniFile.WritePrivateProfileString(Section, Key, Value, this.Path);
    }

    public string IniReadValue(string Section, string Key)
    {
      StringBuilder retVal = new StringBuilder(1024);
      IniFile.GetPrivateProfileString(Section, Key, "", retVal, 1024, this.Path);
      return retVal.ToString();
    }

    public string GetIniFullPath(string inipath)
    {
      if (inipath.IndexOf(":") > 0)
        return inipath;
      return Environment.CurrentDirectory + "/" + inipath;
    }
  }
}
