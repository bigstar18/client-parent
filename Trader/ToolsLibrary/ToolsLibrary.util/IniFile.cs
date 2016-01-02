using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TPME.Log;
namespace ToolsLibrary.util
{
	public class IniFile
	{
		public string Path;
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
		public IniFile(string inipath)
		{
			try
			{
				if (File.Exists("Trade.ini") && !File.Exists(inipath))
				{
					File.Copy("Trade.ini", inipath);
				}
				this.Path = this.GetIniFullPath(inipath);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message.ToString());
			}
		}
		public void IniWriteValue(string Section, string Key, string Value)
		{
			IniFile.WritePrivateProfileString(Section, Key, Value, this.Path);
		}
		public string IniReadValue(string Section, string Key)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				IniFile.GetPrivateProfileString(Section, Key, "", stringBuilder, 1024, this.Path);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message.ToString());
			}
			return stringBuilder.ToString();
		}
		public string GetIniFullPath(string inipath)
		{
			if (inipath.IndexOf(":") > 0)
			{
				return inipath;
			}
			return Environment.CurrentDirectory + "/" + inipath;
		}
	}
}
