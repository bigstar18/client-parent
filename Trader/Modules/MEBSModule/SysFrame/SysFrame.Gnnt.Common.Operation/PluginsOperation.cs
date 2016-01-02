using ModulesLoader;
using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using System;
using System.Collections.Generic;
using System.Xml;
using TPME.Log;
namespace SysFrame.Gnnt.Common.Operation
{
	public class PluginsOperation
	{
		public delegate void SelectItemCallBack(IPlugin myPlugin);
		public IPlugin myPlugin;
		public IPlugin ServerInfoPlugin;
		public IPlugin MEBS_HQPlugin;
		public IPlugin ISSUE_HQPlugin;
		public IPlugin OTC_HQPlugin;
		public Dictionary<string, string> htPlugunsName = new Dictionary<string, string>();
		public PluginsOperation.SelectItemCallBack selectPlugin;
		public void PluginLoad()
		{
			if (this.myPlugin == null)
			{
				this.myPlugin = this.GetDfaultPlugin();
			}
			this.selectPlugin(this.myPlugin);
		}
		private IPlugin GetDfaultPlugin()
		{
			XmlDocument xmlDocument = new XmlDataDocument();
			xmlDocument.Load("nodes.xml");
			this.htPlugunsName.Clear();
			foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
			{
				if (xmlNode.Attributes["Plugins"] != null)
				{
					string value = xmlNode.Attributes["Plugins"].Value;
					if (value.Length > 0)
					{
						string[] array = value.Split(new char[]
						{
							';'
						});
						for (int i = 0; i < array.Length; i++)
						{
							AvailablePluginInfo availablePluginInfo = Global.Modules.get_Plugins().get_AvailablePlugins().Find(array[i]);
							if (availablePluginInfo == null)
							{
								Logger.wirte(3, "没有找到插件" + array[i]);
							}
							else if (!availablePluginInfo.get_Instance().get_IsEnable())
							{
								Logger.wirte(3, "插件不可用" + array[i]);
							}
							else
							{
								IPlugin instance = availablePluginInfo.get_Instance();
								if (instance.get_IsEnable() && instance.get_Name() == "MEBS_HQSystem")
								{
									this.MEBS_HQPlugin = instance;
								}
								if (instance.get_IsEnable() && instance.get_Name() == "ISSUE_HQSystem")
								{
									this.ISSUE_HQPlugin = instance;
								}
								if (instance.get_IsEnable() && instance.get_Name() == "OTC_HQSystem")
								{
									this.OTC_HQPlugin = instance;
								}
								if (instance.get_IsNeedLoad() && instance.get_IsEnable() && instance.get_Name() != Global.ConsumerPluginName)
								{
									this.htPlugunsName.Add(instance.get_Text(), instance.get_Name());
									if (this.myPlugin == null)
									{
										this.myPlugin = instance;
									}
								}
								if (instance.get_DisplayType() == 2)
								{
									this.ServerInfoPlugin = instance;
								}
							}
						}
					}
				}
			}
			return this.myPlugin;
		}
		public void ClosePlugins()
		{
			Global.Modules.get_Plugins().ClosePlugins();
		}
	}
}
