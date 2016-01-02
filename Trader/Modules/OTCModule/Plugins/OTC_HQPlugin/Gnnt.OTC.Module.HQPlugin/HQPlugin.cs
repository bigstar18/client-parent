using Gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using ToolsLibrary.XmlUtil;
using TPME.Log;
namespace Gnnt.OTC.Module.HQPlugin
{
	public class HQPlugin : IPlugin
	{
		private int myPluginNO = 3;
		private string myName = "OTC_HQSystem";
		private string myConfigFileName = "OTC_HQPlugin.xml";
		private string mySettingConfigName = "OTC_HQSystems.xml";
		private string myDescription = "报价行情系统插件";
		private string myAuthor = "薛计涛";
		private string myText = "行情系统";
		private Hashtable myHashConfigSettings = new Hashtable();
		private bool myIsEnable = true;
		private bool myIsNeedLoad;
		private DisplayTypes myDisplayType;
		private Form myPreLoginForm;
		private Form myLoginedForm;
		private string myIpAddress = string.Empty;
		private int myPort;
		private int myHttpPort;
		private string myCommunicationUrl = string.Empty;
		private int timeout;
		private IPluginHost myHost;
		private string strEnable = string.Empty;
		private string strIsNeedLogon = string.Empty;
		private string strText = string.Empty;
		private string strCurTelecomServer = string.Empty;
		private string strCurNetcomServer = string.Empty;
		private string strCommunicationUrl = string.Empty;
		private XmlNode xnAllServer;
		private int ServetType = -1;
		private int RandNum = -1;
		private List<int> NumList = new List<int>();
		private bool isconn;
		private int connServer = -1;
		private bool IsRandServer;
		private bool first = true;
		private PluginConfigInfo pluginConfigInfo;
		private string Fir_myIpAddress = string.Empty;
		private int Fir_myPort;
		private int Fir_myHttpPort;
		private PluginInfo pluginInfo = new PluginInfo();
		private bool textConn = true;
		private Socket socket;
		private IPEndPoint hostEP;
		private Thread timer;
		private Thread thread;
		public string Description
		{
			get
			{
				return this.myDescription;
			}
		}
		public string Author
		{
			get
			{
				return this.myAuthor;
			}
		}
		public IPluginHost Host
		{
			get
			{
				return this.myHost;
			}
			set
			{
				this.myHost = value;
			}
		}
		public int PluginNO
		{
			get
			{
				return this.myPluginNO;
			}
		}
		public string Name
		{
			get
			{
				return this.myName;
			}
		}
		public string ConfigFileName
		{
			get
			{
				return this.myConfigFileName;
			}
			set
			{
				this.myConfigFileName = value;
			}
		}
		public string Version
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString() + "[" + InterFace.get_AssemblyVersion() + "]";
			}
		}
		public string Text
		{
			get
			{
				return this.myText;
			}
		}
		public bool IsEnable
		{
			get
			{
				return this.myIsEnable;
			}
		}
		public bool IsNeedLoad
		{
			get
			{
				return this.myIsNeedLoad;
			}
		}
		public DisplayTypes DisplayType
		{
			get
			{
				return this.myDisplayType;
			}
		}
		public string IpAddress
		{
			get
			{
				return this.myIpAddress;
			}
		}
		public int Port
		{
			get
			{
				return this.myPort;
			}
		}
		public string CommunicationUrl
		{
			get
			{
				return this.myCommunicationUrl;
			}
		}
		public string SettingConfigName
		{
			get
			{
				return this.mySettingConfigName;
			}
			set
			{
				this.mySettingConfigName = value;
			}
		}
		public Hashtable HashConfigSettings
		{
			get
			{
				return this.myHashConfigSettings;
			}
			set
			{
				this.myHashConfigSettings = value;
			}
		}
		public void Initialize()
		{
			XmlDocument xmlDocument = null;
			if (this.myHost != null && this.myHost.get_HtConfigInfo() != null)
			{
				this.pluginConfigInfo = (PluginConfigInfo)this.myHost.get_HtConfigInfo()[this.myName];
				xmlDocument = this.pluginConfigInfo.XmlDoc;
			}
			if (xmlDocument == null)
			{
				this.myIsEnable = false;
				this.myIsNeedLoad = false;
				return;
			}
			XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("ConfigInfo");
			if (xmlElement.SelectSingleNode("Enable") != null)
			{
				this.strEnable = xmlElement.SelectSingleNode("Enable").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("IsNeedLogon") != null)
			{
				this.strIsNeedLogon = xmlElement.SelectSingleNode("IsNeedLogon").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("Text") != null)
			{
				this.strText = xmlElement.SelectSingleNode("Text").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CommunicationUrl") != null)
			{
				this.strCommunicationUrl = xmlElement.SelectSingleNode("CommunicationUrl").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CurTelecomServer") != null)
			{
				this.strCurTelecomServer = xmlElement.SelectSingleNode("CurTelecomServer").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CurNetcomServer") != null)
			{
				this.strCurNetcomServer = xmlElement.SelectSingleNode("CurNetcomServer").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("DisplayTypes") != null)
			{
				this.myDisplayType = int.Parse(xmlElement.SelectSingleNode("DisplayTypes").InnerText.Trim());
			}
			this.myIsEnable = Tools.StrToBool(this.strEnable);
			this.myIsNeedLoad = Tools.StrToBool(this.strIsNeedLogon);
			this.myText = this.strText;
			Hashtable section = this.myHost.get_ConfigurationInfo().getSection("Systems");
			this.timeout = Tools.StrToInt((string)section["TimeOut"], -1);
			if (this.timeout <= 0)
			{
				this.timeout = 300;
			}
			if (!this.isconn)
			{
				this.ServetType = Tools.StrToInt((string)section["CurServer"]);
			}
			else
			{
				this.ServetType = this.connServer;
			}
			int num;
			if (this.ServetType == 0)
			{
				this.connServer = 1;
				num = Tools.StrToInt(this.strCurTelecomServer);
				this.xnAllServer = xmlElement.SelectSingleNode("AllTelecomServer");
			}
			else
			{
				this.connServer = 0;
				num = Tools.StrToInt(this.strCurNetcomServer);
				this.xnAllServer = xmlElement.SelectSingleNode("AllNetcomServer");
			}
			if (num == -1)
			{
				this.IsRandServer = true;
				this.NumList.Clear();
				for (int i = 0; i < this.xnAllServer.ChildNodes.Count; i++)
				{
					Random random = new Random();
					this.RandNum = random.Next(this.xnAllServer.ChildNodes.Count);
					if (!this.NumList.Contains(this.RandNum))
					{
						this.NumList.Add(this.RandNum);
					}
					else
					{
						i--;
					}
				}
				num = this.NumList[0];
			}
			this.GetHQClientUrl(num);
			this.SetServer();
		}
		private void GetHQClientUrl(int curServer)
		{
			XmlElement xmlElement = (XmlElement)this.xnAllServer.ChildNodes[curServer];
			if (xmlElement.SelectSingleNode("IPAddress") != null)
			{
				this.myIpAddress = xmlElement.SelectSingleNode("IPAddress").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("Port") != null)
			{
				this.myPort = Tools.StrToInt(xmlElement.SelectSingleNode("Port").InnerText.Trim());
			}
			if (xmlElement.SelectSingleNode("HttpPort") != null)
			{
				this.myHttpPort = Tools.StrToInt(xmlElement.SelectSingleNode("HttpPort").InnerText.Trim());
			}
			if (this.first)
			{
				this.Fir_myIpAddress = this.myIpAddress;
				this.Fir_myPort = this.myPort;
				this.Fir_myHttpPort = this.myHttpPort;
				this.first = false;
			}
		}
		public bool Logon(ref string info)
		{
			info = "登录成功！";
			return true;
		}
		public bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info)
		{
			info = "此插件不支持修改密码！";
			return false;
		}
		public Form GetForm(bool isLoad, ref string info)
		{
			Form result;
			try
			{
				if (!isLoad)
				{
					if (this.myPreLoginForm == null || this.myPreLoginForm.IsDisposed)
					{
						if (this.IsRandServer)
						{
							for (int i = 0; i < this.NumList.Count; i++)
							{
								if (i >= 1)
								{
									this.GetHQClientUrl(this.NumList[i]);
									this.SetServer();
								}
								if (this.IsConnection())
								{
									break;
								}
							}
							if (!this.textConn)
							{
								if (this.ConnServer())
								{
									Configuration configuration = new Configuration();
									configuration.updateValue("Systems", "CurServer", this.ServetType.ToString());
								}
								else
								{
									this.SetServer();
								}
							}
						}
						MainWindow mainWindow = new MainWindow(this.pluginInfo);
						mainWindow.PreviewKeyDown += new PreviewKeyDownEventHandler(this.hqMainForm_PreviewKeyDown);
						mainWindow.MouseClick += new MouseEventHandler(this.hqClientForm_MouseClick);
						mainWindow.mainForm.add_MultiQuoteMouseEvent(new InterFace.CommodityInfoEventHander(this.hqMainForm_MultiQuoteMouseEvent));
						this.myPreLoginForm = mainWindow;
					}
					result = this.myPreLoginForm;
				}
				else
				{
					this.myLoginedForm = this.GetForm(false, ref info);
					result = this.myLoginedForm;
				}
			}
			catch (Exception ex)
			{
				info = this.myText + ":" + ex.Message;
				result = null;
			}
			return result;
		}
		private void hqMainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (this.myPreLoginForm != null && !this.myPreLoginForm.Focused)
			{
				this.myPreLoginForm.Focus();
			}
		}
		private void SetServer()
		{
			this.pluginInfo.HQResourceManager = this.Host.get_MEBS_ResourceManager();
			this.pluginInfo.HTConfig = XmlParse.getSection(this.myHashConfigSettings, "HQSystems");
			this.pluginInfo.IPAddress = this.myIpAddress;
			this.pluginInfo.Port = this.myPort;
			this.pluginInfo.HttpPort = this.myHttpPort;
			this.pluginInfo.ConfigPath = this.pluginConfigInfo.XmlPath;
		}
		private bool ConnServer()
		{
			bool result = false;
			this.isconn = true;
			this.IsRandServer = false;
			this.NumList.Clear();
			this.Initialize();
			if (!this.IsRandServer)
			{
				if (this.IsConnection())
				{
					result = true;
				}
			}
			else
			{
				for (int i = 0; i < this.NumList.Count; i++)
				{
					if (i >= 1)
					{
						this.GetHQClientUrl(this.NumList[i]);
						this.SetServer();
					}
					if (this.IsConnection())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		private bool IsConnection()
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(this.myIpAddress);
			IPAddress address = hostAddresses[0];
			this.hostEP = new IPEndPoint(address, this.myPort);
			this.socket = new Socket(this.hostEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				this.textConn = true;
				this.timer = new Thread(new ThreadStart(this.TimerOut));
				this.thread = new Thread(new ThreadStart(this.TextConnServer));
				this.thread.Start();
				this.timer.Start();
				while (this.timer.ThreadState != ThreadState.Stopped)
				{
				}
				if (this.thread.ThreadState != ThreadState.Stopped)
				{
					this.thread.Abort();
					this.textConn = false;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				this.textConn = false;
			}
			return this.textConn;
		}
		private void TextConnServer()
		{
			try
			{
				this.socket.Connect(this.hostEP);
				this.socket.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
		}
		private void TimerOut()
		{
			Thread.Sleep(this.timeout);
		}
		private void hqMainForm_MultiQuoteMouseEvent(object sender, InterFace.CommodityInfoEventArgs e)
		{
			ProductDataVO commodityInfo = e.get_CommodityInfo();
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "HQSelCommodity";
			pluginCommunicateInfo.InfoType = 0;
			using (StringWriter stringWriter = new StringWriter())
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteComment("商品信息");
				xmlTextWriter.WriteStartElement("GNNT");
				xmlTextWriter.WriteElementString("COMMODITYID", commodityInfo.marketID + "_" + commodityInfo.code);
				xmlTextWriter.WriteElementString("BUYPRICE", string.Concat(commodityInfo.buyPrice[0]));
				xmlTextWriter.WriteElementString("SELLPRICE", string.Concat(commodityInfo.sellPrice[0]));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndDocument();
				xmlTextWriter.Flush();
				xmlTextWriter.Close();
				pluginCommunicateInfo.InfoContent = stringWriter.ToString();
			}
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void hqClientForm_MouseClick(object sender, EventArgs e)
		{
			if (this.myPreLoginForm != null && !this.myPreLoginForm.Focused)
			{
				this.myPreLoginForm.Focus();
			}
		}
		public void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
		{
			if (pluginCommunicateInfo.InfoName.Equals("KLineEvent") && this.myLoginedForm != null)
			{
				MainWindow mainWindow = (MainWindow)this.myLoginedForm;
				CommodityInfo commodityInfo = CommodityInfo.DealCode(pluginCommunicateInfo.InfoContent);
				mainWindow.mainForm.ShowPageKLine(commodityInfo);
			}
			if (pluginCommunicateInfo.InfoName.Equals("MinLineEvent") && this.myLoginedForm != null)
			{
				MainWindow mainWindow2 = (MainWindow)this.myLoginedForm;
				CommodityInfo commodityInfo2 = CommodityInfo.DealCode(pluginCommunicateInfo.InfoContent);
				mainWindow2.mainForm.ShowPageMinLine(commodityInfo2);
			}
		}
		public void Dispose()
		{
			if (this.myPreLoginForm != null && !this.myPreLoginForm.IsDisposed)
			{
				this.myPreLoginForm.Close();
			}
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				this.myLoginedForm.Close();
			}
		}
		public bool AgencyLogon(ref string info)
		{
			return false;
		}
		public void CloseForm()
		{
		}
		public void SetAgencyLogoutEvent(EventAgencyLogOut _AgencyLogoutEvent)
		{
		}
		public void SetLockTree(EventLockTree _LogoutEvent)
		{
		}
		public void SetLogoutEvent(EventLogOut _LogoutEvent)
		{
		}
		public void SetMessageEvent(EventHandler _messageEvent)
		{
		}
		public void SetPlayMessage(EventPlayMessage _PlayMessage)
		{
		}
		public void SetProgressEvent(EventInitData _initDataMainForm)
		{
		}
		public void SetReLoad(EventReLoad _ReLoad)
		{
		}
		public void SetUnLock(bool _UnLock)
		{
		}
	}
}
