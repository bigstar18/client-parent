using HttpTrade.Gnnt.MEBS.Lib;
using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace ConsumerPlugin
{
	public class ConsumerPlugin : IPlugin
	{
		private int myPluginNO = 3;
		private string myName = "MEBS_Consumer";
		private string myConfigFileName = "MEBS_Consumer.xml";
		private string myDescription = "客户平台插件";
		private string myAuthor = "薛计涛";
		private string myText = string.Empty;
		private string mySettingConfigName = "";
		private Hashtable myHashConfigSettings = new Hashtable();
		private bool myIsEnable = true;
		private bool myIsNeedLoad;
		private DisplayTypes myDisplayType;
		private Form myPreLoginForm;
		private Form myLoginedForm = new Form();
		private string myIpAddress = string.Empty;
		private int myPort;
		private string myCommunicationUrl = string.Empty;
		private int timeout;
		private string strHttp = "http://";
		private string PreLoginUrl = "/tradeweb/message/navigator.jsp";
		private string LoginedUrl = "/tradeweb/message/navigator.jsp";
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
		private TradeLibrary tradeLibrary;
		private ResourceManager SysResourceManager;
		private string Fir_myIpAddress = string.Empty;
		private string Fir_myPort = string.Empty;
		private bool isLogin;
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
		public void Initialize()
		{
			this.SysResourceManager = this.myHost.get_MEBS_ResourceManager();
			XmlDocument xmlDocument = null;
			if (this.myHost != null && this.myHost.get_HtConfigInfo() != null)
			{
				PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)this.myHost.get_HtConfigInfo()[this.myName];
				xmlDocument = pluginConfigInfo.XmlDoc;
			}
			if (xmlDocument == null)
			{
				this.myIsEnable = false;
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
			if (xmlElement.SelectSingleNode("Http") != null)
			{
				this.strHttp = xmlElement.SelectSingleNode("Http").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CommunicationUrl") != null)
			{
				this.strCommunicationUrl = xmlElement.SelectSingleNode("CommunicationUrl").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("PreLoginUrl") != null)
			{
				this.PreLoginUrl = xmlElement.SelectSingleNode("PreLoginUrl").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("LoginedUrl") != null)
			{
				this.LoginedUrl = xmlElement.SelectSingleNode("LoginedUrl").InnerText.Trim();
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
			this.GetCommunicationUrl(num);
		}
		private void GetCommunicationUrl(int curServer)
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
			if (this.first)
			{
				this.Fir_myIpAddress = this.myIpAddress;
				this.Fir_myPort = this.myPort.ToString();
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
			this.tradeLibrary = new TradeLibrary();
			this.tradeLibrary.set_IsConsumerPlugin(true);
			this.tradeLibrary.set_CommunicationUrl(this.myCommunicationUrl);
			this.tradeLibrary.Initialize();
		}
		public bool Logon(ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				string @string = this.SysResourceManager.GetString("PluginStr_NoFoundPluginOwners");
				info = @string;
			}
			else if (this.tradeLibrary == null)
			{
				string string2 = this.SysResourceManager.GetString("PluginStr_NoInitialize");
				info = string2;
			}
			else
			{
				LogonRequestVO logonRequestVO = new LogonRequestVO();
				logonRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
				logonRequestVO.set_Password(this.myHost.get_SysLogonInfo().Password);
				logonRequestVO.set_RegisterWord(this.myHost.get_SysLogonInfo().RegisterWord);
				if (this.myDisplayType == 1)
				{
					logonRequestVO.set_LogonType("web");
				}
				else
				{
					logonRequestVO.set_LogonType("pc");
				}
				try
				{
					LogonResponseVO logonResponseVO = this.tradeLibrary.Logon(logonRequestVO);
					if (logonResponseVO != null)
					{
						if (logonResponseVO.RetCode == 0L)
						{
							result = true;
							this.isLogin = true;
							this.myHost.get_SysLogonInfo().LastTime = logonResponseVO.get_LastTime();
							this.myHost.get_SysLogonInfo().LastIP = logonResponseVO.get_LastIP();
							this.myHost.get_SysLogonInfo().ChgPWD = logonResponseVO.get_ChgPWD();
							string string3 = this.SysResourceManager.GetString("PluginStr_LoginSuccess");
							info = string3;
						}
						else
						{
							info = logonResponseVO.RetMessage;
						}
					}
					else
					{
						string string4 = this.SysResourceManager.GetString("PluginStr_NoResultBag");
						info = string4;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		public bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				string @string = this.SysResourceManager.GetString("PluginStr_NoOwnersValidationFails");
				info = @string;
			}
			else if (this.tradeLibrary == null)
			{
				string string2 = this.SysResourceManager.GetString("PluginStr_NoObjectValidationFails");
				info = string2;
			}
			else
			{
				try
				{
					ChgPwdRequestVO chgPwdRequestVO = new ChgPwdRequestVO();
					chgPwdRequestVO.UserID = this.myHost.get_SysLogonInfo().TraderID;
					chgPwdRequestVO.OldPassword = oldPWD;
					chgPwdRequestVO.NewPassword = newPWD;
					ResponseVO responseVO = this.tradeLibrary.ChangePwd(chgPwdRequestVO);
					if (responseVO.RetCode == 0L)
					{
						result = true;
						string string3 = this.SysResourceManager.GetString("PluginStr_PwdSuccess");
						info = string3;
					}
					else
					{
						string string4 = this.SysResourceManager.GetString("PluginStr_PwdFails");
						info = string4 + responseVO.RetMessage;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		private bool CheckUser(ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				string @string = this.SysResourceManager.GetString("PluginStr_NoOwnersValidationFails");
				info = @string;
			}
			else if (this.tradeLibrary == null)
			{
				string string2 = this.SysResourceManager.GetString("PluginStr_NoObjectValidationFails");
				info = string2;
			}
			else
			{
				try
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
					if (responseVO != null && responseVO.RetCode == 0L)
					{
						result = true;
						this.isLogin = true;
						string string3 = this.SysResourceManager.GetString("PluginStr_Authenticated");
						info = string3;
					}
					else
					{
						string string4 = this.SysResourceManager.GetString("PluginStr_AuthenticationFailed");
						info = string4 + responseVO.RetMessage;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		private bool Logoff(ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				string @string = this.SysResourceManager.GetString("PluginStr_NoOwnerLogoutFails");
				info = @string;
			}
			else if (this.tradeLibrary == null)
			{
				string string2 = this.SysResourceManager.GetString("PluginStr_NoObjetLogoutFails");
				info = string2;
			}
			else
			{
				try
				{
					ResponseVO responseVO = this.tradeLibrary.Logoff(this.myHost.get_SysLogonInfo().TraderID);
					if (responseVO != null && responseVO.RetCode == 0L)
					{
						result = true;
						string string3 = this.SysResourceManager.GetString("PluginStr_LogoutSuccess");
						info = string3;
					}
					else
					{
						string string4 = this.SysResourceManager.GetString("PluginStr_LogoutFails");
						info = string4 + responseVO.RetMessage;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		public Form GetForm(bool isLoad, ref string info)
		{
			string text = string.Empty;
			if (!isLoad)
			{
				if (this.myPreLoginForm == null || this.myPreLoginForm.IsDisposed)
				{
					if (!this.IsRandServer)
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
					else
					{
						for (int i = 0; i < this.NumList.Count; i++)
						{
							if (i >= 1)
							{
								this.GetCommunicationUrl(this.NumList[i]);
							}
							if (this.IsConnection())
							{
								this.myPreLoginForm = new BrowseForm(new Uri(string.Concat(new object[]
								{
									this.strHttp,
									this.myIpAddress,
									":",
									this.myPort,
									this.PreLoginUrl
								})));
								break;
							}
						}
						if (this.myPreLoginForm == null)
						{
							if (this.ConnServer())
							{
								Configuration configuration = new Configuration();
								configuration.updateValue("Systems", "CurServer", this.ServetType.ToString());
								this.myPreLoginForm = new BrowseForm(new Uri(string.Concat(new object[]
								{
									this.strHttp,
									this.myIpAddress,
									":",
									this.myPort,
									this.PreLoginUrl
								})));
							}
							else
							{
								this.myPreLoginForm = new BrowseForm(new Uri(string.Concat(new string[]
								{
									this.strHttp,
									this.Fir_myIpAddress,
									":",
									this.Fir_myPort,
									this.PreLoginUrl
								})));
							}
						}
					}
				}
				return this.myPreLoginForm;
			}
			if (this.CheckUser(ref info))
			{
				if (this.myDisplayType == 1)
				{
					text = string.Concat(new object[]
					{
						"?LogonType=web&FromLogonType=",
						SysShareInfo.FromLogonType,
						"&sessionID=",
						SysShareInfo.sessionID,
						"&FromModuleID=",
						SysShareInfo.moduleID,
						"&userID=",
						this.myHost.get_SysLogonInfo().TraderID
					});
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
					text = string.Concat(new object[]
					{
						"?LogonType=web&FromLogonType=",
						SysShareInfo.FromLogonType,
						"&sessionID=",
						SysShareInfo.sessionID,
						"&FromModuleID=",
						SysShareInfo.moduleID,
						"&userID=",
						this.myHost.get_SysLogonInfo().TraderID
					});
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
				return this.myLoginedForm;
			}
			string @string = this.SysResourceManager.GetString("PluginStr_NoPermissionLogin");
			string string2 = this.SysResourceManager.GetString("PluginStr_ErrorReason");
			info = @string + this.myText + string2 + info;
			return null;
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
						this.GetCommunicationUrl(this.NumList[i]);
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
				Logger.wirte(ex);
				this.textConn = false;
			}
			return this.textConn;
		}
		public void TextConnServer()
		{
			try
			{
				this.socket.Connect(this.hostEP);
				this.socket.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void TimerOut()
		{
			Thread.Sleep(this.timeout);
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
				string arg_60_0 = string.Empty;
			}
		}
		public bool AgencyLogon(ref string info)
		{
			return true;
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
