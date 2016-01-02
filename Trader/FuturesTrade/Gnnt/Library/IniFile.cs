namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IniFile
    {
        public string Path;

        public IniFile(string inipath)
        {
            this.Path = this.GetIniFullPath(inipath);
        }

        public string GetIniFullPath(string inipath)
        {
            if (inipath.IndexOf(":") > 0)
            {
                return inipath;
            }
            return (Environment.CurrentDirectory + "/" + inipath);
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder retVal = new StringBuilder(0x400);
            GetPrivateProfileString(Section, Key, "", retVal, 0x400, this.Path);
            return retVal.ToString();
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    }
}
