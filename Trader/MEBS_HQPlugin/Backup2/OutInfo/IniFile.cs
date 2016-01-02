// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.OutInfo.IniFile
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Gnnt.MEBS.HQModel.OutInfo
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
      StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
      IniFile.GetPrivateProfileString(Section, Key, "", retVal, (int) byte.MaxValue, this.Path);
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
