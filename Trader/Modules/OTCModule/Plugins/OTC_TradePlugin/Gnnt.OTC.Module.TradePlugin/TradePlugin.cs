using HttpTrade.Gnnt.OTC.Lib;
using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using ToolsLibrary.XmlUtil;
using TPME.Log;
using TradeClientApp.Gnnt.OTC;
using TradeClientApp.Gnnt.OTC.Library;
using TradeClientApp.Gnnt.OTC.ToolsBar;
using TradeInterface.Gnnt.OTC.DataVO;
namespace Gnnt.OTC.Module.TradePlugin
{
	public class TradePlugin : IPlugin
	{
		public delegate void InitDataMainForm(string str, int val);
		private class ScreenSize
		{
			public int width;
			public int hight;
		}
		private int myPluginNO = 2;
		private string myName = "OTC_Trade";
		private string myConfigFileName = "OTC_TradePlugin.xml";
		private string mySettingConfigName = "OTC_Trade.xml";
		private string myDescription = "天津贵金属交易所";
		private string myAuthor = "";
		private string myText = string.Empty;
		private Hashtable myHashConfigSettings = new Hashtable();
		public TMainForm mainForm = null;
		private EventHandler MessageEvent = null;
		private bool myIsEnable = true;
		private bool myIsNeedLoad = true;
		private DisplayTypes myDisplayType = 4;
		private Form myPreLoginForm = null;
		private Form myLoginedForm = null;
		private string myIpAddress = string.Empty;
		private int myPort = 0;
		private int mySafePort = 0;
		private string myCommunicationUrl = string.Empty;
		private string mySafeCommunicationUrl = string.Empty;
		private string strHttp = "http://";
		private IPluginHost myHost = null;
		private XmlNode xnAllServer = null;
		private int ServetType = -1;
		private int RandNum = -1;
		private List<int> NumList;
		private bool isconn = false;
		private int connServer = -1;
		private bool IsRandServer = false;
		private bool first = true;
		private TradeLibrary tradeLibrary = null;
		private string strEnable = string.Empty;
		private string strIsNeedLogon = string.Empty;
		private string strText = string.Empty;
		private string strCurTelecomServer = string.Empty;
		private string strCurNetcomServer = string.Empty;
		private string strCommunicationUrl = string.Empty;
		private string strRunMode = 0.ToString("d");
		private PluginConfigInfo pluginConfigInfo;
		private string Fir_myIpAddress = string.Empty;
		private int Fir_myPort = 0;
		private int Fir_mySafePort = 0;
		private bool isLogin = false;
		private int iIdentity = 0;
		private bool textConn = true;
		private Socket socket = null;
		private IPEndPoint hostEP;
		private Thread timer;
		private Thread thread;
		private int timeout = 0;
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
		public int SafePort
		{
			get
			{
				return this.mySafePort;
			}
		}
		public string SafeCommunicationUrl
		{
			get
			{
				return this.mySafeCommunicationUrl;
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
		public void SetUnLock(bool _UnLock)
		{
			if (this.mainForm != null)
			{
				this.mainForm.SetUnLock(_UnLock);
			}
		}
		public void SetLogoutEvent(EventLogOut _LogoutEvent)
		{
			if (this.mainForm != null)
			{
				if (_LogoutEvent != null)
				{
					this.mainForm.add_LogOutEvent(new TMainForm.LogOut(_LogoutEvent.Invoke));
				}
			}
		}
		public void SetPlayMessage(EventPlayMessage _PlayMessage)
		{
			if (this.mainForm != null)
			{
				if (_PlayMessage != null)
				{
					TMainForm expr_1E = this.mainForm;
					expr_1E.PlayMessageEvent = (TMainForm.EventPlayMessage)Delegate.Combine(expr_1E.PlayMessageEvent, new TMainForm.EventPlayMessage(_PlayMessage.Invoke));
				}
			}
		}
		public void CloseForm()
		{
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				TMainForm tMainForm = (TMainForm)this.myLoginedForm;
				tMainForm.CloseMainForm();
				tMainForm.Dispose();
			}
		}
		public void SetAgencyLogoutEvent(EventAgencyLogOut _AgencyLogoutEvent)
		{
		}
		public void SetLockTree(EventLockTree _LogoutEvent)
		{
			if (this.mainForm != null)
			{
				if (_LogoutEvent != null)
				{
					this.mainForm.add_LockTreeEvent(new TMainForm.LockTree(_LogoutEvent.Invoke));
				}
			}
		}
		public void SetReLoad(EventReLoad _ReLoad)
		{
			if (this.mainForm != null)
			{
				if (_ReLoad != null)
				{
					this.mainForm.add_ReLoad(new TMainForm.ReLoadHanlder(_ReLoad.Invoke));
				}
			}
		}
		public void Initialize()
		{
			this.NumList = new List<int>();
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
			}
			else
			{
				this.strEnable = string.Empty;
				this.strIsNeedLogon = string.Empty;
				this.strText = string.Empty;
				this.strCurTelecomServer = string.Empty;
				this.strCurNetcomServer = string.Empty;
				this.strCommunicationUrl = string.Empty;
				this.strRunMode = 0.ToString("d");
				Hashtable section = this.myHost.get_ConfigurationInfo().getSection("Systems");
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
					for (int i = 0; i < this.xnAllServer.ChildNodes.Count; i++)
					{
						this.NumList.Add(i);
					}
				}
				this.GetCommunicationUrl(num, true);
				if (this.first)
				{
					this.ChangeServer();
				}
			}
		}
		private void GetCommunicationUrl(int curServer, bool iniFrist)
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
			if (xmlElement.SelectSingleNode("SafePort") != null)
			{
				this.mySafePort = Tools.StrToInt(xmlElement.SelectSingleNode("SafePort").InnerText.Trim());
			}
			else
			{
				this.mySafePort = this.myPort;
			}
			if (this.first && iniFrist)
			{
				this.Fir_myIpAddress = this.myIpAddress;
				this.Fir_myPort = this.myPort;
				this.Fir_mySafePort = this.mySafePort;
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
			this.mySafeCommunicationUrl = string.Concat(new object[]
			{
				this.strHttp.Replace("http://", "https://"),
				this.myIpAddress,
				":",
				this.mySafePort,
				"/",
				this.strCommunicationUrl
			});
			if (this.strRunMode == 1.ToString("d"))
			{
				this.mySafeCommunicationUrl = this.myCommunicationUrl;
			}
			this.myHost.set_TradeUrl(this.myCommunicationUrl);
			this.myHost.set_TradeSafeUrl(this.mySafeCommunicationUrl);
			if ((this.first && iniFrist) || this.tradeLibrary == null)
			{
				this.tradeLibrary = TradeLibrary.GetInstance();
				this.first = false;
			}
			this.tradeLibrary.set_CommunicationUrl(this.myCommunicationUrl);
			this.tradeLibrary.set_SafeCommunicationUrl(this.mySafeCommunicationUrl);
			this.tradeLibrary.set_BuffPath(this.pluginConfigInfo.XmlPath);
			this.tradeLibrary.Initialize();
		}
		public bool AgencyLogon(ref string info)
		{
			info = "登录成功！";
			return true;
		}
		public bool Logon(ref string info)
		{
			this.ChangeServer();
			bool result = false;
			if (this.myHost == null)
			{
				info = "没有发现插件拥有者，登录失败！";
			}
			else if (this.tradeLibrary == null)
			{
				info = "没有初始化通讯对象，登录失败！";
			}
			else
			{
				try
				{
					this.SendRequest(ref info, ref result);
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		private void SendRequest(ref string info, ref bool result)
		{
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
			logonRequestVO.set_Password(this.myHost.get_SysLogonInfo().Password);
			logonRequestVO.set_RegisterWord(this.myHost.get_SysLogonInfo().RegisterWord);
			logonRequestVO.set_VersionInfo(this.myHost.get_SysLogonInfo().VersionInfo);
			if (this.myDisplayType == 1)
			{
				logonRequestVO.set_LogonType("web");
			}
			else
			{
				logonRequestVO.set_LogonType("pc");
			}
			LogonResponseVO logonResponseVO = this.tradeLibrary.Logon(logonRequestVO);
			if (logonResponseVO != null && logonResponseVO.RetCode == 0L)
			{
				this.myHost.get_SysLogonInfo().LastTime = logonResponseVO.get_LastTime();
				this.myHost.get_SysLogonInfo().LastIP = logonResponseVO.get_LastIP();
				this.myHost.get_SysLogonInfo().ChgPWD = logonResponseVO.get_ChgPWD();
				this.iIdentity = Convert.ToInt32(logonResponseVO.get_Identity());
				this.myHost.set_IdentityType(logonResponseVO.get_Identity());
				result = true;
				this.isLogin = true;
			}
			else
			{
				info = "登录失败！" + logonResponseVO.RetMessage;
			}
		}
		private void ChangeServer()
		{
			bool flag = false;
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i], false);
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
				}
				else
				{
					this.myIpAddress = this.Fir_myIpAddress;
					this.myPort = this.Fir_myPort;
					this.mySafePort = this.Fir_mySafePort;
				}
			}
		}
		private bool ConnServer()
		{
			bool flag = false;
			this.isconn = true;
			this.IsRandServer = false;
			this.NumList.Clear();
			this.Initialize();
			bool result;
			if (!this.IsRandServer)
			{
				if (this.IsConnection())
				{
					result = true;
					return result;
				}
			}
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i], false);
				}
				if (this.IsConnection())
				{
					flag = true;
					break;
				}
			}
			result = flag;
			return result;
		}
		private void TimerOut()
		{
			Thread.Sleep(this.timeout);
		}
		private void TextConnServer()
		{
			try
			{
				this.socket.Connect(this.hostEP);
				this.socket.Close();
			}
			catch (Exception var_0_23)
			{
				this.textConn = false;
			}
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
		public bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				info = "没有发现插件拥有者，验证失败！";
			}
			else if (this.tradeLibrary == null)
			{
				info = "没有初始化通讯对象，验证失败！";
			}
			else
			{
				try
				{
					ChgPwdRequestVO chgPwdRequestVO = new ChgPwdRequestVO();
					chgPwdRequestVO.UserID = this.myHost.get_SysLogonInfo().TraderID;
					chgPwdRequestVO.OldPassword = oldPWD;
					chgPwdRequestVO.NewPassword = newPWD;
					if (chgpwd == 0)
					{
						chgPwdRequestVO.PasswordType = "1";
					}
					else
					{
						chgPwdRequestVO.PasswordType = "0";
					}
					ResponseVO responseVO = this.tradeLibrary.ChangePwd(chgPwdRequestVO);
					if (responseVO.RetCode == 0L)
					{
						if (chgpwd == 1)
						{
							Global.Password = newPWD;
						}
						result = true;
						info = "密码修改信息：成功！";
					}
					else
					{
						info = "密码修改信息：失败！原因：" + responseVO.RetMessage;
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
				info = "没有发现插件拥有者，验证失败！";
			}
			else if (this.tradeLibrary == null)
			{
				info = "没有初始化通讯对象，验证失败！";
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
				catch (Exception ex)
				{
					info = ex.Message;
					Logger.wirte(3, ex.Message);
				}
			}
			return result;
		}
		private bool Logoff(ref string info)
		{
			bool result = false;
			if (this.myHost == null)
			{
				info = "没有发现插件拥有者，注销失败！";
			}
			else if (this.tradeLibrary == null)
			{
				info = "没有初始化通讯对象，退出失败！";
			}
			else
			{
				try
				{
					ResponseVO responseVO = this.tradeLibrary.Logoff(this.myHost.get_SysLogonInfo().TraderID);
					if (responseVO != null && responseVO.RetCode == 0L)
					{
						result = true;
						info = "注销成功！";
						Logger.wirte(1, "TradePlugin 注销成功");
					}
					else
					{
						info = "注销失败！" + responseVO.RetMessage;
					}
					if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
					{
						TMainForm tMainForm = (TMainForm)this.myLoginedForm;
						tMainForm.CloseMainForm();
						tMainForm.Dispose();
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
			this.ChangeServer();
			Form result;
			if (!isLoad)
			{
				result = this.myPreLoginForm;
			}
			else if (XmlParse.getSection(this.myHashConfigSettings, "Trade") == null)
			{
				info = "交易系统没有发现配置信息！";
				result = null;
			}
			else if (this.CheckUser(ref info))
			{
				result = this.GetCurrentForm();
			}
			else
			{
				info = "没有权限登录" + this.myText + "错误原因：" + info;
				result = null;
			}
			return result;
		}
		private Form GetCurrentForm()
		{
			if (this.myLoginedForm == null || this.myLoginedForm.IsDisposed)
			{
				Global.HTConfig = XmlParse.getSection(this.myHashConfigSettings, "Trade");
				Global.M_ResourceManager = this.myHost.get_MEBS_ResourceManager();
				Global.m_PMESResourceManager = this.myHost.get_OTC_ResourceManager();
				Global.UserID = this.myHost.get_SysLogonInfo().TraderID;
				Global.Password = this.myHost.get_SysLogonInfo().Password;
				Global.RegisterWord = this.myHost.get_SysLogonInfo().RegisterWord;
				Global.SystamIcon = this.myHost.get_SystemIcon();
				Global.SystamText = this.myHost.get_SystemTitle();
				Global.SystamVersion = this.myHost.get_SystemVersion();
				if (this.pluginConfigInfo != null)
				{
					Global.ConfigPath = this.pluginConfigInfo.XmlPath;
				}
				this.setScreenSize();
				try
				{
					this.mainForm = new TMainForm(this.tradeLibrary, this.myHost.get_IdentityType());
					ToolsBarControl toolsBarControl = new ToolsBarControl(false, this.myHost.get_IdentityType());
					List<Control> list = new List<Control>();
					list.Add(toolsBarControl);
					list.Add(this.mainForm.HQ_DataGrid);
					this.mainForm.Tag = list;
					InterFace.TopLevel = false;
					this.mainForm.add_ChangeServerEvent(new EventHandler(this.mainForm_ChangeServerEvent));
					this.mainForm.callbackFirmInfoDataGrid = new TMainForm.CallbackFirmInfoDataGrid(toolsBarControl.LoadFirmInfoDataGrid);
					this.mainForm.updateFirmInfo = new TMainForm.UpdateFirmInfoCallBack(toolsBarControl.CalculateFirmInfo);
					this.mainForm.SetToolsBarEnable = new TMainForm.SetToolsBarEnableCallBack(toolsBarControl.SetToolsBarEnable);
					toolsBarControl.toolsBarButtonClick = new ToolsBarControl.ToolsBarButtonCallBack(this.mainForm.ToolsBarButtonClick);
					this.myLoginedForm = this.mainForm;
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
			return this.myLoginedForm;
		}
		private void mainForm_ChangeServerEvent(object sender, EventArgs e)
		{
			this.ChangeServer();
		}
		private void mainForm_LockFormEvent(object sender, EventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "LockTradeForm";
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = "Lock";
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void mainForm_KLineEvent(object sender, InterFace.CommodityInfoEventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = e.get_EventInfo();
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = e.get_CommodityCode();
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void mainForm_MinLineEvent(object sender, InterFace.CommodityInfoEventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = e.get_EventInfo();
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = e.get_CommodityCode();
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private TradePlugin.ScreenSize getScreenSize()
		{
			Rectangle workingArea = Screen.GetWorkingArea(new Form1());
			int width = workingArea.Width;
			int height = workingArea.Height;
			return new TradePlugin.ScreenSize
			{
				width = width,
				hight = height
			};
		}
		private void setSize(TradePlugin.ScreenSize screenSize)
		{
			if (screenSize.width == 800)
			{
				MessageBox.Show("不建议使用800*600分辨率", "提示");
			}
			Global.screenWidth = screenSize.width;
			Global.screenHight = screenSize.hight;
		}
		private void setScreenSize()
		{
			TradePlugin.ScreenSize size = new TradePlugin.ScreenSize();
			size = this.getScreenSize();
			this.setSize(size);
		}
		public void SetMessageEvent(EventHandler _messageEvent)
		{
			if (this.mainForm != null)
			{
				if (_messageEvent != null)
				{
					this.mainForm.SetMessageEvent(_messageEvent);
				}
			}
		}
		public void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
		{
			if (pluginCommunicateInfo.InfoName.Equals("HQSelCommodity"))
			{
				if (pluginCommunicateInfo.InfoType == 0)
				{
					try
					{
						string text = string.Empty;
						string text2 = string.Empty;
						string text3 = string.Empty;
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(pluginCommunicateInfo.InfoContent);
						if (xmlDocument.SelectSingleNode("/GNNT/COMMODITYID") != null)
						{
							text = xmlDocument.SelectSingleNode("/GNNT/COMMODITYID").InnerText;
						}
						if (xmlDocument.SelectSingleNode("/GNNT/BUYPRICE") != null)
						{
							text2 = xmlDocument.SelectSingleNode("/GNNT/BUYPRICE").InnerText;
						}
						if (xmlDocument.SelectSingleNode("/GNNT/SELLPRICE") != null)
						{
							text3 = xmlDocument.SelectSingleNode("/GNNT/SELLPRICE").InnerText;
						}
						if (this.myLoginedForm != null)
						{
							TMainForm tMainForm = (TMainForm)this.myLoginedForm;
						}
					}
					catch (Exception var_5_EA)
					{
					}
				}
			}
		}
		public void SetProgressEvent(EventInitData _initDataMainForm)
		{
			if (this.mainForm != null)
			{
				if (_initDataMainForm != null)
				{
					this.mainForm.add_InitData(new TMainForm.InitDataMainForm(_initDataMainForm.Invoke));
				}
				this.mainForm.Initdata();
			}
		}
		public void Dispose()
		{
			if (this.tradeLibrary != null)
			{
				this.tradeLibrary.Dispose();
			}
			if (this.myPreLoginForm != null && !this.myPreLoginForm.IsDisposed)
			{
				TMainForm tMainForm = (TMainForm)this.myPreLoginForm;
				tMainForm.CloseMainForm();
			}
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				TMainForm tMainForm = (TMainForm)this.myLoginedForm;
				tMainForm.CloseMainForm();
			}
			if (this.isLogin)
			{
				string empty = string.Empty;
				if (!this.Logoff(ref empty))
				{
					Logger.wirte(2, this.myText + "注销用户时发生错误，错误信息：" + empty);
				}
			}
		}
	}
}
