using FuturesTrade.Gnnt.Library;
using FuturesTrade.Gnnt.UI.Forms;
using HttpTrade.Gnnt.MEBS.Lib;
using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using ToolsLibrary.XmlUtil;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace Gnnt.MEBS.Module.TradePlugin
{
	public class TradePlugin : IPlugin
	{
		private class ScreenSize
		{
			public int width;
			public int hight;
		}
		private int myPluginNO = 2;
		private string myName = "MEBS_Trade";
		private string myConfigFileName = "MEBS_TradePlugin.xml";
		private string mySettingConfigName = "MEBS_Trade.xml";
		private Hashtable myHashConfigSettings = new Hashtable();
		private string myDescription = "远期交易系统插件";
		private string myAuthor = "薛计涛";
		private string myText = string.Empty;
		private bool IsShowCondition;
		private bool myIsEnable = true;
		private bool myIsNeedLoad = true;
		private DisplayTypes myDisplayType = 4;
		private Form myPreLoginForm;
		private Form myLoginedForm;
		private string myIpAddress = string.Empty;
		private int myPort;
		private string myCommunicationUrl = string.Empty;
		private int timeout;
		private string strHttp = "http://";
		private IPluginHost myHost;
		private string strEnable = string.Empty;
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
		private PluginConfigInfo pluginConfigInfo;
		private ResourceManager SysResourceManager;
		private string Fir_myIpAddress = string.Empty;
		private int Fir_myPort;
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
		public void Initialize()
		{
			this.SysResourceManager = this.myHost.get_MEBS_ResourceManager();
			XmlDocument xmlDocument = null;
			if (this.myHost != null && this.myHost.get_HtConfigInfo() != null)
			{
				this.pluginConfigInfo = (PluginConfigInfo)this.myHost.get_HtConfigInfo()[this.myName];
				xmlDocument = this.pluginConfigInfo.XmlDoc;
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
			else
			{
				this.NumList.Clear();
				for (int j = 0; j < this.xnAllServer.ChildNodes.Count; j++)
				{
					this.NumList.Add(j);
				}
			}
			this.GetCommunicationUrl(num, false);
		}
		private void GetCommunicationUrl(int curServer, bool iniFrist = false)
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
			if (this.first && iniFrist)
			{
				this.Fir_myIpAddress = this.myIpAddress;
				this.Fir_myPort = this.myPort;
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
			if ((this.first && iniFrist) || this.tradeLibrary == null)
			{
				this.tradeLibrary = new TradeLibrary();
				this.first = false;
			}
			this.tradeLibrary.set_CommunicationUrl(this.myCommunicationUrl);
			this.tradeLibrary.set_IsWriteLog(Global.IsWriteLog);
			this.tradeLibrary.set_BuffPath(this.pluginConfigInfo.XmlPath);
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
				try
				{
					if (!this.IsRandServer)
					{
						result = this.SendRequest(ref info, result);
					}
					else
					{
						for (int i = 0; i < this.NumList.Count; i++)
						{
							if (i >= 1)
							{
								this.GetCommunicationUrl(this.NumList[i], false);
							}
							if (this.IsConnection())
							{
								result = this.SendRequest(ref info, result);
								break;
							}
						}
						if (!this.textConn)
						{
							if (this.ConnServer())
							{
								result = this.SendRequest(ref info, result);
							}
							else
							{
								this.myIpAddress = this.Fir_myIpAddress;
								this.myPort = this.Fir_myPort;
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
								this.tradeLibrary.set_CommunicationUrl(this.myCommunicationUrl);
								this.tradeLibrary.set_IsWriteLog(Global.IsWriteLog);
								this.tradeLibrary.Initialize();
								result = this.SendRequest(ref info, result);
							}
						}
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		private bool SendRequest(ref string info, bool result)
		{
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
			logonRequestVO.set_Password(this.myHost.get_SysLogonInfo().Password);
			logonRequestVO.set_RegisterWord(this.myHost.get_SysLogonInfo().RegisterWord);
			logonRequestVO.set_VersionInfo(this.myHost.get_SysLogonInfo().VersionInfo);
			logonRequestVO.set_LoginMark(this.myHost.get_SysLogonInfo().str);
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
				int num = 1;
				if (num == 2)
				{
					this.IsShowCondition = true;
				}
				else if (num == 3)
				{
					info = num.ToString();
				}
				result = true;
				this.isLogin = true;
				this.myHost.get_SysLogonInfo().LastTime = logonResponseVO.get_LastTime();
				this.myHost.get_SysLogonInfo().LastIP = logonResponseVO.get_LastIP();
				this.myHost.get_SysLogonInfo().ChgPWD = logonResponseVO.get_ChgPWD();
				if (logonResponseVO.get_UserID().Length > 0)
				{
					this.myHost.get_SysLogonInfo().TraderID = logonResponseVO.get_UserID();
				}
				Configuration configuration = new Configuration();
				configuration.updateValue("Systems", "CurServer", this.ServetType.ToString());
			}
			else
			{
				string @string = this.SysResourceManager.GetString("PluginStr_LoginFails");
				info = @string + logonResponseVO.RetMessage;
			}
			return result;
		}
		private void ChangeServer()
		{
			bool flag = false;
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i], true);
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
				result = true;
			}
			for (int i = 0; i < this.NumList.Count; i++)
			{
				if (i >= 1)
				{
					this.GetCommunicationUrl(this.NumList[i], true);
				}
				if (this.IsConnection())
				{
					result = true;
					break;
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
				Logger.wirte(3, ex.StackTrace + ex.Message);
				this.textConn = false;
			}
			return this.textConn;
		}
		private void TextConnServer()
		{
			this.socket.Connect(this.hostEP);
			this.socket.Close();
		}
		private void TimerOut()
		{
			Thread.Sleep(this.timeout);
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
						Global.Password = newPWD;
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
			if (!isLoad)
			{
				return this.myPreLoginForm;
			}
			if (XmlParse.getSection(this.myHashConfigSettings, "Trade") == null)
			{
				this.SysResourceManager.GetString("PluginStr_NoConfigeInfo");
				info = "";
				return null;
			}
			if (this.CheckUser(ref info))
			{
				return this.GetCurrentForm(ref info);
			}
			string @string = this.SysResourceManager.GetString("PluginStr_NoPermissionLogin");
			string string2 = this.SysResourceManager.GetString("PluginStr_ErrorReason");
			info = @string + this.myText + string2 + info;
			return null;
		}
		private Form GetCurrentForm(ref string info)
		{
			if (this.myLoginedForm == null || this.myLoginedForm.IsDisposed)
			{
				Global.HTConfig = XmlParse.getSection(this.myHashConfigSettings, "Trade");
				Global.M_ResourceManager = this.SysResourceManager;
				Global.UserID = this.myHost.get_SysLogonInfo().TraderID;
				Global.Password = this.myHost.get_SysLogonInfo().Password;
				Global.RegisterWord = this.myHost.get_SysLogonInfo().RegisterWord;
				Global.TradeLibrary = this.tradeLibrary;
				Global.ConfigPath = this.pluginConfigInfo.XmlPath;
				this.setScreenSize();
				try
				{
					int num = Tools.StrToInt(this.myHost.get_ConfigurationInfo().getSection("Systems")["FormStyle"].ToString());
					MainForm mainForm = new MainForm(num);
					InterFace.TopLevel = false;
					Global.add_MinLineEvent(new InterFace.CommodityInfoEventHander(this.mainForm_MinLineEvent));
					Global.add_KLineEvent(new InterFace.CommodityInfoEventHander(this.mainForm_KLineEvent));
					Global.add_LockFormEvent(new EventHandler(this.mainForm_LockFormEvent));
					Global.add_FloatFormEvent(new EventHandler(this.mainForm_FloatFormEvent));
					Global.add_LogoutEvent(new EventHandler(this.mainForm_LogOutFormEvent));
					Global.add_UpdateStyleEvent(new EventHandler(this.mainForm_UpdateStyleEvent));
					Global.add_ChangeServerEvent(new EventHandler(this.mainForm_ChangeServerEvent));
					this.myLoginedForm = mainForm;
				}
				catch (Exception)
				{
					info = "";
				}
			}
			return this.myLoginedForm;
		}
		private void mainForm_UpdateStyleEvent(object sender, EventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "UpdateStyle";
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = "Style";
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void mainForm_FloatFormEvent(object sender, EventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "FloatTradeForm";
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = "Float";
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void mainForm_LogOutFormEvent(object sender, EventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "LogOutTradeForm";
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = "LogOut";
			this.myHost.Feedback(pluginCommunicateInfo, this);
		}
		private void mainForm_CloseFormEvent(object sender, EventArgs e)
		{
			PluginCommunicateInfo pluginCommunicateInfo = new PluginCommunicateInfo();
			pluginCommunicateInfo.InfoName = "CloseTradeForm";
			pluginCommunicateInfo.InfoType = 1;
			pluginCommunicateInfo.InfoContent = "Close";
			this.myHost.Feedback(pluginCommunicateInfo, this);
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
				string @string = this.SysResourceManager.GetString("PluginStr_Tip");
				string string2 = this.SysResourceManager.GetString("PluginStr_TipContext");
				MessageBox.Show(string2, @string);
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
		public void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
		{
			if (pluginCommunicateInfo.InfoName.Equals("HQSelCommodity") && pluginCommunicateInfo.InfoType == null)
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
						Global.ReceivedOrderInfo(text, Tools.StrToDouble(text2, 0.0), Tools.StrToDouble(text3, 0.0));
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(3, this.Text + "接受消息，发生异常，异常内容：" + ex.ToString());
				}
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
				MainForm mainForm = (MainForm)this.myPreLoginForm;
				mainForm.Close();
			}
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				MainForm mainForm2 = (MainForm)this.myLoginedForm;
				mainForm2.Close();
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
