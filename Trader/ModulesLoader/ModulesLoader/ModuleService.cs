using System;
using System.Collections.Generic;
using System.Xml;
using TPME.Log;
namespace ModulesLoader
{
	public class ModuleService
	{
		private string configXml = "Modules.xml";
		private List<ModuleInfo> modules;
		private PluginService plugins;
		public List<ModuleInfo> Modules
		{
			get
			{
				return this.modules;
			}
			set
			{
				this.modules = value;
			}
		}
		public PluginService Plugins
		{
			get
			{
				return this.plugins;
			}
			set
			{
				this.plugins = value;
			}
		}
		public ModuleService()
		{
			this.modules = new List<ModuleInfo>();
			this.LoadModules();
			this.plugins = new PluginService(this.modules);
		}
		private void LoadModules()
		{
			try
			{
				XmlDocument xmlDocument = new XmlDataDocument();
				xmlDocument.Load(this.configXml);
				foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
				{
					if (xmlNode.Attributes["Name"] != null)
					{
						string value = xmlNode.Attributes["Name"].Value;
						string value2 = xmlNode.Attributes["Path"].Value;
						string value3 = xmlNode.Attributes["ConfigPath"].Value;
						string value4 = xmlNode.Attributes["ModuleNO"].Value;
						ModuleInfo moduleInfo = new ModuleInfo();
						moduleInfo.ModuleName = value;
						moduleInfo.ModulePath = value2;
						moduleInfo.ConfigPath = value3;
						moduleInfo.ModuleNo = value4;
						this.modules.Add(moduleInfo);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "加载模块异常：" + ex.Message);
			}
		}
		public void LoadPlugins()
		{
			foreach (ModuleInfo current in this.modules)
			{
				string modulePath = current.ModulePath;
				string configPath = current.ConfigPath;
				this.plugins.FindPlugins(modulePath, configPath);
			}
		}
	}
}
