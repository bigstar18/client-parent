using ModulesLoader;
using PluginInterface;
using YrdceClient.Yrdce.Common.Library;
using System;
using System.Collections.Generic;
using System.Xml;
using TPME.Log;
namespace YrdceClient.Yrdce.Common.Operation
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
							AvailablePluginInfo availablePluginInfo = Global.Modules.Plugins.AvailablePlugins.Find(array[i]);
							if (availablePluginInfo == null)
							{
								Logger.wirte(MsgType.Error, "没有找到插件" + array[i]);
							}
							else if (!availablePluginInfo.Instance.IsEnable)
							{
								Logger.wirte(MsgType.Error, "插件不可用" + array[i]);
							}
							else
							{
								IPlugin instance = availablePluginInfo.Instance;
								if (instance.IsEnable && instance.Name == "MEBS_HQSystem")
								{
									this.MEBS_HQPlugin = instance;
								}
								if (instance.IsEnable && instance.Name == "ISSUE_HQSystem")
								{
									this.ISSUE_HQPlugin = instance;
								}
								if (instance.IsEnable && instance.Name == "OTC_HQSystem")
								{
									this.OTC_HQPlugin = instance;
								}
								if (instance.IsNeedLoad && instance.IsEnable && instance.Name != Global.ConsumerPluginName)
								{
									this.htPlugunsName.Add(instance.Text, instance.Name);
									if (this.myPlugin == null)
									{
										this.myPlugin = instance;
									}
								}
								if (instance.DisplayType == DisplayTypes.Dialog)
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
			Global.Modules.Plugins.ClosePlugins();
		}
	}
}
