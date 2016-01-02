using System;
using ToolsLibrary.XmlUtil;
namespace TradeVersionInfo
{
	public class DetailVersion
	{
		public VersionInfo GetDetailVersion(string version)
		{
			VersionInfo versionInfo = new VersionInfo();
			if (version != null && version != "")
			{
				try
				{
					string[] array = version.Split(new char[]
					{
						'.'
					});
					if (array.Length < 3)
					{
                       
						WriteLog.WriteMsg("版本信息不正确。");
					}
					else if (array[0].ToString() == "" || array[1].ToString() == "" || array[2].ToString() == "")
					{
						versionInfo.VersionType = "";
						versionInfo.MajorVersion = "";
						versionInfo.MinorVersion = "";
						versionInfo.MicroVersion = "";
					}
					else
					{
						string text = array[0].ToString();
						string minorVersion = array[1].ToString();
						string microVersion = array[2].ToString();
						if (char.IsUpper(text[0]))
						{
							versionInfo.VersionType = text[0].ToString();
							versionInfo.MajorVersion = text.Remove(0, 1);
						}
						else
						{
							versionInfo.VersionType = "F";
							versionInfo.MajorVersion = text;
						}
						versionInfo.MinorVersion = minorVersion;
						versionInfo.MicroVersion = microVersion;
					}
					goto IL_197;
				}
				catch (Exception ex)
				{
					versionInfo.VersionType = "";
					versionInfo.MajorVersion = "";
					versionInfo.MinorVersion = "";
					versionInfo.MicroVersion = "";
					WriteLog.WriteMsg("获取详细版本信息发生错误：" + ex.Message);
					goto IL_197;
				}
			}
			versionInfo.VersionType = "";
			versionInfo.MajorVersion = "";
			versionInfo.MinorVersion = "";
			versionInfo.MicroVersion = "";
			IL_197:
			versionInfo.FullVersion = version;
			return versionInfo;
		}
	}
}
