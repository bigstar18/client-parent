using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using ToolsLibrary.util;
using ToolsLibrary.XmlUtil;
using TPME.Log;
namespace ModulesLoader
{
	public class PluginService : IPluginHost
	{
		public delegate void PluginCommunicateInfoEventHander(object sender, PluginCommunicateInfoEventArgs e);
		private Configuration myConfigInfo;
		private Hashtable myHtConfigInfo = new Hashtable();
		private ResourceManager _Mebs_ResourceManager;
		private ResourceManager _Otc_ResourceManager;
		private ResourceManager _Issue_ResourceManager;
		private Image mySystemImage;
		private Icon mySystemIcon;
		private string myTradeUrl = string.Empty;
		private string myTradeSafeUrl = string.Empty;
		private string mySystemText = string.Empty;
		private string mySystemVersion = string.Empty;
		private string myIdentity = string.Empty;
		private Hashtable nodesHashtable = new Hashtable();
		private LogonInfo myLogonInfo = new LogonInfo();
		private AvailablePlugins colAvailablePlugins = new AvailablePlugins();
		private Hashtable loginPluginsHashtable = new Hashtable();
		public event PluginService.PluginCommunicateInfoEventHander PluginCommunicateInfoEvent;
		public Hashtable HtConfigInfo
		{
			get
			{
				return this.myHtConfigInfo;
			}
		}
		public LogonInfo SysLogonInfo
		{
			get
			{
				return this.myLogonInfo;
			}
			set
			{
				this.myLogonInfo = value;
			}
		}
		public AvailablePlugins AvailablePlugins
		{
			get
			{
				return this.colAvailablePlugins;
			}
			set
			{
				this.colAvailablePlugins = value;
			}
		}
		public Hashtable LoginPluginsHashtable
		{
			get
			{
				return this.loginPluginsHashtable;
			}
			set
			{
				this.loginPluginsHashtable = value;
			}
		}
		public Icon SystemIcon
		{
			get
			{
				return this.mySystemIcon;
			}
			set
			{
				this.mySystemIcon = value;
			}
		}
		public Image SystemImage
		{
			get
			{
				return this.mySystemImage;
			}
			set
			{
				this.mySystemImage = value;
			}
		}
		public string IdentityType
		{
			get
			{
				return this.myIdentity;
			}
			set
			{
				this.myIdentity = value;
			}
		}
		public ResourceManager ISSUE_ResourceManager
		{
			get
			{
				return this._Issue_ResourceManager;
			}
		}
		public ResourceManager MEBS_ResourceManager
		{
			get
			{
				return this._Mebs_ResourceManager;
			}
		}
		public ResourceManager OTC_ResourceManager
		{
			get
			{
				return this._Otc_ResourceManager;
			}
		}
		public Configuration ConfigurationInfo
		{
			get
			{
				return this.myConfigInfo;
			}
		}
		public string SystemTitle
		{
			get
			{
				return this.mySystemText;
			}
			set
			{
				this.mySystemText = value;
			}
		}
		public string SystemVersion
		{
			get
			{
				return this.mySystemVersion;
			}
			set
			{
				this.mySystemVersion = value;
			}
		}
		public string TradeUrl
		{
			get
			{
				return this.myTradeUrl;
			}
			set
			{
				this.myTradeUrl = value;
			}
		}
		public string TradeSafeUrl
		{
			get
			{
				return this.myTradeSafeUrl;
			}
			set
			{
				this.myTradeSafeUrl = value;
			}
		}
		public PluginService(List<ModuleInfo> moduleInfoList)
		{
			try
			{
				this.myConfigInfo = new Configuration();
				if (this.myConfigInfo.getSection("Systems").Contains("SysLanguage"))
				{
					SysLanguage.language = Tools.StrToShort(this.myConfigInfo.getSection("Systems")["SysLanguage"].ToString(), 0);
				}
				else
				{
					SysLanguage.language = 0;
				}
				if (!this.SetControlText(SysLanguage.language, moduleInfoList))
				{
					throw new Exception("PluginServices 初始化资源文件信息发生异常！");
				}
				this.mySystemImage = (Image)this._Mebs_ResourceManager.GetObject("logo_32.png");
				this.mySystemIcon = (Icon)this._Mebs_ResourceManager.GetObject("Logo.ico");
				this.LoadNodesXML();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
				this.myConfigInfo = null;
			}
		}
		private void LoadNodesXML()
		{
			XmlDocument xmlDocument = new XmlDataDocument();
			xmlDocument.Load("nodes.xml");
			foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
			{
				if (xmlNode.Attributes == null)
				{
					break;
				}
				string key = string.Empty;
				string value = string.Empty;
				if (xmlNode.Attributes["Name"] != null)
				{
					key = xmlNode.Attributes["Name"].Value;
				}
				if (xmlNode.Attributes["Text"] != null)
				{
					value = xmlNode.Attributes["Text"].Value;
				}
				this.nodesHashtable.Add(key, value);
			}
		}
		private bool SetControlText(short language, List<ModuleInfo> moduleInfoList)
		{
			try
			{
				foreach (ModuleInfo current in moduleInfoList)
				{
					string moduleName = current.ModuleName;
					string configPath = current.ConfigPath;
					string path = Environment.CurrentDirectory + "\\" + configPath + "\\";
					if (language == 0)
					{
						string fillName = "Yrdce." + moduleName + ".ch.resources";
						if (!this.LoadResource(moduleName, path, fillName))
						{
							bool result = false;
							return result;
						}
					}
					else
					{
						string fillName2 = "Yrdce." + moduleName + ".en.resources";
						if (!this.LoadResource(moduleName, path, fillName2))
						{
							bool result = false;
							return result;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
				bool result = false;
				return result;
			}
			return true;
		}
		private bool LoadResource(string name, string path, string fillName)
		{
			if (!File.Exists(path + fillName))
			{
				return false;
			}
			if (name == "MEBS")
			{
				this._Mebs_ResourceManager = ResourceManager.CreateFileBasedResourceManager(path + fillName.Replace(".resources", ""), "", null);
			}
			else if (name == "OTC")
			{
				this._Otc_ResourceManager = ResourceManager.CreateFileBasedResourceManager(path + fillName.Replace(".resources", ""), "", null);
			}
			else if (name == "ISSUE")
			{
				this._Issue_ResourceManager = ResourceManager.CreateFileBasedResourceManager(path + fillName.Replace(".resources", ""), "", null);
			}
			return true;
		}
		public void FindPlugins(string Path, string configPath)
		{
			try
			{
				string[] files = Directory.GetFiles(Path);
				for (int i = 0; i < files.Length; i++)
				{
					string fileName = files[i];
					FileInfo fileInfo = new FileInfo(fileName);
					if (fileInfo.Extension.Equals(".dll"))
					{
						this.AddPlugin(fileName, configPath);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "加载插件异常：" + ex.Message);
			}
		}
		public void ClosePlugins()
		{
			try
			{
				foreach (AvailablePluginInfo availablePluginInfo in this.colAvailablePlugins)
				{
					if (availablePluginInfo.Instance != null)
					{
						availablePluginInfo.Instance.Dispose();
						availablePluginInfo.Instance = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "释放插件异常：" + ex.Message);
			}
			finally
			{
				this.colAvailablePlugins.Clear();
				this.myHtConfigInfo.Clear();
				this.loginPluginsHashtable.Clear();
			}
		}
		private void AddPlugin(string FileName, string configPath)
		{
			Assembly assembly = Assembly.LoadFrom(FileName);
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (type.IsPublic && !type.IsAbstract)
				{
					Type @interface = type.GetInterface("PluginInterface.IPlugin", true);
					if (@interface != null)
					{
						AvailablePluginInfo availablePluginInfo = new AvailablePluginInfo();
						availablePluginInfo.AssemblyPath = FileName;
						availablePluginInfo.Instance = (IPlugin)Activator.CreateInstance(assembly.GetType(type.ToString()));
						bool flag = false;
						if (availablePluginInfo.Instance.Name != null && availablePluginInfo.Instance.Name.Length > 0 && availablePluginInfo.Instance.ConfigFileName != null && availablePluginInfo.Instance.ConfigFileName.Length > 0)
						{
							try
							{
                                //song
								XmlDocument xmlDocument = new XmlDocument();
								availablePluginInfo.Instance.ConfigFileName = configPath + "\\" + availablePluginInfo.Instance.ConfigFileName;
								xmlDocument.Load(availablePluginInfo.Instance.ConfigFileName);
								PluginConfigInfo pluginConfigInfo = new PluginConfigInfo();
								pluginConfigInfo.XmlPath = configPath + "\\";
								pluginConfigInfo.XmlDoc = xmlDocument;
								if (!this.myHtConfigInfo.Contains(availablePluginInfo.Instance.Name))
								{
									this.myHtConfigInfo.Add(availablePluginInfo.Instance.Name, pluginConfigInfo);
								}
							}
							catch (Exception ex)
							{
								flag = true;
								Logger.wirte(MsgType.Error, "加载插件" + availablePluginInfo.Instance.Name + "配置信息出错，出错信息" + ex.Message);
							}
						}
						if (availablePluginInfo.Instance.Name != null && availablePluginInfo.Instance.Name.Length > 0 && availablePluginInfo.Instance.SettingConfigName != null && availablePluginInfo.Instance.SettingConfigName.Length > 0)
						{
							try
							{
								string str = configPath + "\\" + availablePluginInfo.Instance.SettingConfigName;
								availablePluginInfo.Instance.HashConfigSettings = XmlParse.ParseXml(Environment.CurrentDirectory + "/" + str);
							}
							catch (Exception ex2)
							{
								Logger.wirte(MsgType.Error, "加载插件" + availablePluginInfo.Instance.Name + "配置信息出错，出错信息" + ex2.Message);
							}
						}
						if (!flag)
						{
							availablePluginInfo.Instance.Host = this;
							availablePluginInfo.Instance.Initialize();
							this.colAvailablePlugins.Add(availablePluginInfo);
							if (availablePluginInfo.Instance.IsNeedLoad && this.nodesHashtable.ContainsKey(availablePluginInfo.Instance.Name) && !this.loginPluginsHashtable.Contains(this.nodesHashtable[availablePluginInfo.Instance.Name]))
							{
								this.loginPluginsHashtable.Add(this.nodesHashtable[availablePluginInfo.Instance.Name], availablePluginInfo.Instance.Name);
							}
						}
						availablePluginInfo = null;
					}
				}
			}
			assembly = null;
		}
		public void Feedback(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
		{
			try
			{
				if (this.PluginCommunicateInfoEvent != null)
				{
					PluginCommunicateInfoEventArgs e = new PluginCommunicateInfoEventArgs(pluginCommunicateInfo);
					this.PluginCommunicateInfoEvent(this, e);
				}
				foreach (AvailablePluginInfo availablePluginInfo in this.AvailablePlugins)
				{
					if (availablePluginInfo.Instance != null && availablePluginInfo.Instance.IsEnable)
					{
						availablePluginInfo.Instance.AcceptInfo(pluginCommunicateInfo, Plugin);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "接受来自插件的消息Feedback异常：" + ex.Message);
			}
		}
	}
}
