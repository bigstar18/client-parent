using HttpTrade.Gnnt.OTC.Lib;
using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
namespace Gnnt.OTC.Module.BankInterFacePlugin
{
	public class BankInterfacePlugin : IPlugin
	{
		private int myPluginNO = 6;
		private string myName = "OTC_BankInterface";
		private string myConfigFileName = "OTC_BankInterface.xml";
		private string myDescription = "出入金管理插件";
		private string myAuthor = "";
		private string myText = string.Empty;
		private bool isIdentity;
		private string mySettingConfigName = "";
		private Hashtable myHashConfigSettings = new Hashtable();
		private bool myIsEnable = true;
		private bool myIsNeedLoad = true;
		private DisplayTypes myDisplayType;
		private Form myPreLoginForm;
		private Form myLoginedForm;
		private string myIpAddress = string.Empty;
		private int myPort;
		private string myCommunicationUrl = string.Empty;
		private string strHttp = "http://";
		private string PreLoginUrl = "/money/main_1.jsp";
		private string LoginedUrl = "/money/main.jsp";
		private IPluginHost myHost;
		private XmlNode xnAllServer;
		private int ServetType = -1;
		private int RandNum = -1;
		private List<int> NumList;
		private bool isconn;
		private int connServer = -1;
		private bool IsRandServer;
		private bool first = true;
		private TradeLibrary tradeLibrary;
		private string strEnable = string.Empty;
		private string strIsNeedLogon = string.Empty;
		private string strText = string.Empty;
		private string strCurTelecomServer = string.Empty;
		private string strCurNetcomServer = string.Empty;
		private string strCommunicationUrl = string.Empty;
		private string strRunMode = 0.ToString("d");
		private bool isLogin;
		private string Fir_myIpAddress = string.Empty;
		private int Fir_myPort;
		private bool textConn = true;
		private Socket socket;
		private IPEndPoint hostEP;
		private Thread timer;
		private Thread thread;
		private int timeout;
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
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
		public void CloseForm()
		{
		}
		public void SetMessageEvent(EventHandler _messageEvent)
		{
		}
		public void SetUnLock(bool _UnLock)
		{
		}
		public void SetLogoutEvent(EventLogOut _LogoutEvent)
		{
		}
		public void SetReLoad(EventReLoad _ReLoad)
		{
		}
		public void SetLockTree(EventLockTree _LogoutEvent)
		{
		}
		public void SetPlayMessage(EventPlayMessage _PlayMessage)
		{
		}
		public void SetAgencyLogoutEvent(EventAgencyLogOut _AgencyLogoutEvent)
		{
		}
		public void SetProgressEvent(EventInitData _initDataMainForm)
		{
		}
		public void Initialize()
		{
			try
			{
				XmlDocument xmlDocument = null;
				if (this.myHost != null && this.myHost.get_HtConfigInfo() != null)
				{
					PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)this.myHost.get_HtConfigInfo()[this.myName];
					xmlDocument = pluginConfigInfo.XmlDoc;
				}
				else
				{
					Logger.wirte(3, "myHost为空或者是 myHost.HtConfigInfo为空。");
				}
				if (xmlDocument == null)
				{
					this.myIsEnable = false;
					this.myIsNeedLoad = false;
					Logger.wirte(3, "没有配置信息 。");
				}
				else
				{
					this.strEnable = string.Empty;
					this.strIsNeedLogon = string.Empty;
					this.strText = string.Empty;
					this.myHost.get_ConfigurationInfo().getSection("Systems");
					XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("ConfigInfo");
					if (xmlElement.SelectSingleNode("Enable") != null)
					{
						this.strEnable = xmlElement.SelectSingleNode("Enable").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement.SelectSingleNode("IsNeedLogon") != null)
					{
						this.strIsNeedLogon = xmlElement.SelectSingleNode("IsNeedLogon").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement.SelectSingleNode("Text") != null)
					{
						this.strText = xmlElement.SelectSingleNode("Text").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					this.myIsEnable = Tools.StrToBool(this.strEnable);
					this.myIsNeedLoad = Tools.StrToBool(this.strIsNeedLogon);
					this.myText = this.strText;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				throw;
			}
		}
		private void Initialize(string name)
		{
			try
			{
				this.NumList = new List<int>();
				XmlDocument xmlDocument = null;
				if (this.myHost != null && this.myHost.get_HtConfigInfo() != null)
				{
					PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)this.myHost.get_HtConfigInfo()[this.myName];
					xmlDocument = pluginConfigInfo.XmlDoc;
				}
				else
				{
					Logger.wirte(3, "myHost为空或者是 myHost.HtConfigInfo为空。");
				}
				if (xmlDocument == null)
				{
					this.myIsEnable = false;
					this.myIsNeedLoad = false;
					Logger.wirte(3, "没有配置信息 。");
				}
				else
				{
					this.strCurTelecomServer = string.Empty;
					this.strCurNetcomServer = string.Empty;
					this.strCommunicationUrl = string.Empty;
					Hashtable section = this.myHost.get_ConfigurationInfo().getSection("Systems");
					XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("ConfigInfo");
					XmlElement xmlElement2 = (XmlElement)xmlElement.SelectSingleNode(name);
					if (xmlElement2.SelectSingleNode("Http") != null)
					{
						this.strHttp = xmlElement2.SelectSingleNode("Http").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("CommunicationUrl") != null)
					{
						this.strCommunicationUrl = xmlElement2.SelectSingleNode("CommunicationUrl").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("PreLoginUrl") != null)
					{
						this.PreLoginUrl = xmlElement2.SelectSingleNode("PreLoginUrl").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("LoginedUrl") != null)
					{
						this.LoginedUrl = xmlElement2.SelectSingleNode("LoginedUrl").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("CurTelecomServer") != null)
					{
						this.strCurTelecomServer = xmlElement2.SelectSingleNode("CurTelecomServer").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("CurNetcomServer") != null)
					{
						this.strCurNetcomServer = xmlElement2.SelectSingleNode("CurNetcomServer").InnerText.Trim();
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
					if (xmlElement2.SelectSingleNode("DisplayTypes") != null)
					{
						this.myDisplayType = int.Parse(xmlElement2.SelectSingleNode("DisplayTypes").InnerText.Trim());
					}
					else
					{
						Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
					}
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
						this.xnAllServer = xmlElement2.SelectSingleNode("AllTelecomServer");
					}
					else
					{
						this.connServer = 0;
						num = Tools.StrToInt(this.strCurNetcomServer);
						this.xnAllServer = xmlElement2.SelectSingleNode("AllNetcomServer");
					}
					if (num == -1)
					{
						this.IsRandServer = true;
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
					else
					{
						this.NumList.Clear();
						for (int j = 0; j < this.xnAllServer.ChildNodes.Count; j++)
						{
							this.NumList.Add(j);
						}
					}
					this.GetCommunicationUrl(num);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				throw;
			}
		}
		private void GetCommunicationUrl(int curServer)
		{
			XmlElement xmlElement = (XmlElement)this.xnAllServer.ChildNodes[curServer];
			if (xmlElement.SelectSingleNode("IPAddress") != null)
			{
				this.myIpAddress = xmlElement.SelectSingleNode("IPAddress").InnerText.Trim();
			}
			else
			{
				Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
			}
			if (xmlElement.SelectSingleNode("Port") != null)
			{
				this.myPort = Tools.StrToInt(xmlElement.SelectSingleNode("Port").InnerText.Trim());
			}
			else
			{
				Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
			}
			string text = string.Empty;
			if (xmlElement.SelectSingleNode("CommonPort") != null)
			{
				text = xmlElement.SelectSingleNode("CommonPort").InnerText.Trim();
			}
			else
			{
				Logger.wirte(3, "出入金管理插件xml文档格式不对 。");
			}
			if (this.first)
			{
				this.Fir_myIpAddress = this.myIpAddress;
				this.Fir_myPort = this.myPort;
				this.first = false;
			}
			this.myCommunicationUrl = string.Concat(new object[]
			{
				this.strHttp,
				this.myIpAddress,
				":",
				this.myPort,
				"/",
				this.strCommunicationUrl
			});
			if (this.strRunMode == 1.ToString("d") && text.Length > 0)
			{
				this.myCommunicationUrl = string.Concat(new string[]
				{
					this.strHttp.Replace("https://", "http://"),
					this.myIpAddress,
					":",
					text,
					"/",
					this.strCommunicationUrl
				});
			}
			this.tradeLibrary = new TradeLibrary();
			this.tradeLibrary.set_CommunicationUrl(this.myCommunicationUrl);
			this.tradeLibrary.Initialize();
			this.isIdentity = true;
		}
		public bool AgencyLogon(ref string info)
		{
			info = "登录成功！";
			return true;
		}
		public bool Logon(ref string info)
		{
			bool result = false;
			try
			{
				this.ChangeServer();
				TradeLibrary instance = TradeLibrary.GetInstance();
				if (this.myHost.get_TradeUrl().Length != 0)
				{
					instance.set_CommunicationUrl(this.myHost.get_TradeUrl());
				}
				instance.Initialize();
				if (this.myHost == null)
				{
					info = "没有发现插件拥有者，登录失败！";
					Logger.wirte(3, info);
				}
				else if (instance == null)
				{
					info = "没有初始化通讯对象，登录失败！";
					Logger.wirte(3, info);
				}
				else
				{
					LogonRequestVO logonRequestVO = new LogonRequestVO();
					logonRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
					logonRequestVO.set_Password(this.myHost.get_SysLogonInfo().Password);
					if (this.myDisplayType == 1)
					{
						logonRequestVO.set_LogonType("web");
					}
					else
					{
						logonRequestVO.set_LogonType("pc");
					}
					LogonResponseVO logonResponseVO = instance.Logon(logonRequestVO);
					if (logonResponseVO != null)
					{
						if (logonResponseVO.RetCode == 0L)
						{
							result = true;
							this.isLogin = true;
							this.myHost.get_SysLogonInfo().LastTime = logonResponseVO.get_LastTime();
							this.myHost.get_SysLogonInfo().LastIP = logonResponseVO.get_LastIP();
							this.myHost.get_SysLogonInfo().ChgPWD = logonResponseVO.get_ChgPWD();
							info = "登录成功！";
							this.myHost.set_IdentityType(logonResponseVO.get_Identity());
						}
						else
						{
							info = logonResponseVO.RetMessage;
						}
					}
					else
					{
						info = "没有返回包，登录失败！";
						Logger.wirte(3, info);
					}
				}
			}
			catch (Exception ex)
			{
				info = ex.Message;
				Logger.wirte(3, info);
				throw;
			}
			return result;
		}
		public bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info)
		{
			return true;
		}
		private void ChangeServer()
		{
			bool flag = false;
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i]);
				}
				if (this.IsConnection())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (this.ConnServer())
				{
					this.myHost.get_ConfigurationInfo().updateValue("Systems", "CurServer", this.ServetType.ToString());
					return;
				}
				this.myIpAddress = this.Fir_myIpAddress;
				this.myPort = this.Fir_myPort;
			}
		}
		private bool ConnServer()
		{
			bool result = false;
			this.isconn = true;
			this.IsRandServer = false;
			this.NumList.Clear();
			this.Initialize();
			if (!this.IsRandServer && this.IsConnection())
			{
				return true;
			}
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i]);
				}
				if (this.IsConnection())
				{
					result = true;
					break;
				}
			}
			return result;
		}
		private void TimerOut()
		{
			Thread.Sleep(this.timeout);
		}
		private void TextConnServer()
		{
			this.socket.Connect(this.hostEP);
			this.socket.Close();
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
				Logger.wirte(ex);
				this.textConn = false;
			}
			return this.textConn;
		}
		private bool CheckUser(ref string info)
		{
			bool result = false;
			try
			{
				if (this.myHost == null)
				{
					info = "没有发现插件拥有者，验证失败！";
					Logger.wirte(3, info);
				}
				else if (this.tradeLibrary == null)
				{
					info = "没有初始化通讯对象，验证失败！";
					Logger.wirte(3, info);
				}
				else
				{
					CheckUserRequestVO checkUserRequestVO = new CheckUserRequestVO();
					checkUserRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
					if (this.myDisplayType == 1)
					{
						checkUserRequestVO.set_LogonType("web");
					}
					else
					{
						checkUserRequestVO.set_LogonType("pc");
					}
					ResponseVO responseVO = this.tradeLibrary.CheckUser(checkUserRequestVO);
					if (responseVO != null)
					{
						if (responseVO.RetCode == 0L)
						{
							result = true;
							this.isLogin = true;
							info = "通过身份验证！";
						}
						else
						{
							info = string.Format("身份验证失败！{0}({1})", responseVO.RetMessage, responseVO.RetCode);
							Logger.wirte(3, info);
						}
					}
					else
					{
						info = "身份验证无响应，请重试！";
						Logger.wirte(3, info + "responseVO为null");
					}
				}
			}
			catch (Exception ex)
			{
				info = ex.Message;
				Logger.wirte(3, info);
			}
			return result;
		}
		private bool Logoff(ref string info)
		{
			bool result = false;
			try
			{
				if (this.myHost == null)
				{
					info = "没有发现插件拥有者，注销失败！";
					Logger.wirte(3, info);
				}
				else if (this.tradeLibrary == null)
				{
					info = "没有初始化通讯对象，注销失败！";
					Logger.wirte(3, info);
				}
				else
				{
					ResponseVO responseVO = this.tradeLibrary.Logoff(this.myHost.get_SysLogonInfo().TraderID);
					if (responseVO != null && responseVO.RetCode == 0L)
					{
						result = true;
						info = "注销成功！";
						Logger.wirte(1, "BankInterFacePlugin 注销成功");
					}
					else
					{
						info = "注销失败！" + responseVO.RetMessage;
						Logger.wirte(2, info);
					}
				}
			}
			catch (Exception ex)
			{
				info = ex.Message;
				Logger.wirte(3, info);
				throw;
			}
			this.isIdentity = false;
			return result;
		}
		public Form GetForm(bool isLoad, ref string info)
		{
			if (!this.isIdentity)
			{
				if (Convert.ToInt32(this.myHost.get_IdentityType()) == Convert.ToInt32(0))
				{
					this.Initialize(0.ToString());
				}
				else
				{
					this.Initialize(1.ToString());
				}
			}
			this.ChangeServer();
			if (!isLoad)
			{
				if (this.myPreLoginForm == null || this.myPreLoginForm.IsDisposed)
				{
					this.myPreLoginForm = new BrowseForm(new Uri(string.Concat(new object[]
					{
						this.strHttp,
						this.myIpAddress,
						":",
						this.myPort,
						this.PreLoginUrl
					})));
				}
				return this.myPreLoginForm;
			}
			if (this.CheckUser(ref info))
			{
				return this.GetCurrentForm(ref info);
			}
			return null;
		}
		private Form GetCurrentForm(ref string info)
		{
			string text = string.Empty;
			if (this.myLoginedForm == null || this.myLoginedForm.IsDisposed)
			{
				if (this.myDisplayType == 1)
				{
					text = "?LogonType=web&FromLogonType=" + SysShareInfo.FromLogonType;
					this.myLoginedForm.Tag = string.Concat(new object[]
					{
						this.strHttp,
						this.myIpAddress,
						":",
						this.myPort,
						this.LoginedUrl,
						text
					});
				}
				else
				{
					text = "?LogonType=pc&FromLogonType=" + SysShareInfo.FromLogonType;
					this.myLoginedForm = new BrowseForm(new Uri(string.Concat(new object[]
					{
						this.strHttp,
						this.myIpAddress,
						":",
						this.myPort,
						this.LoginedUrl,
						text
					})));
				}
			}
			return this.myLoginedForm;
		}
		public void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
		{
		}
		public void Dispose()
		{
			if (this.tradeLibrary != null)
			{
				this.tradeLibrary.Dispose();
			}
			if (this.myPreLoginForm != null && !this.myPreLoginForm.IsDisposed)
			{
				this.myPreLoginForm.Close();
			}
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				this.myLoginedForm.Close();
			}
			if (this.isLogin)
			{
				string empty = string.Empty;
				if (!this.Logoff(ref empty))
				{
					Logger.wirte(3, this.myText + "注销用户时发生错误，错误信息：" + empty);
				}
			}
		}
	}
}
