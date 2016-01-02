using FuturesTrade.Gnnt.Library;
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
namespace Gnnt.MixAccountPlugin
{
	public class MixAccountPlugin : IPlugin
	{
		private int myPluginNO = 12;
		private string myName = "MixAccount";
		private string myConfigFileName = "MixAccountPlugin.xml";
		private string mySettingConfigName = "";
		private Hashtable myHashConfigSettings = new Hashtable();
		private string myDescription = "合并账户插件";
		private string myAuthor = " ";
		private string myText = string.Empty;
		private bool myIsEnable = true;
		private bool myIsNeedLoad;
        private DisplayTypes myDisplayType;
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
		public TradeLibrary tradeLibrary;
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
				return Assembly.GetExecutingAssembly().GetName().Version.ToString() + "[" + InterFace.AssemblyVersion + "]";
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
			this.SysResourceManager = this.myHost.MEBS_ResourceManager;
			XmlDocument xmlDocument = null;
			if (this.myHost != null && this.myHost.HtConfigInfo != null)
			{
				this.pluginConfigInfo = (PluginConfigInfo)this.myHost.HtConfigInfo[this.myName];
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
				this.myDisplayType = (DisplayTypes)int.Parse(xmlElement.SelectSingleNode("DisplayTypes").InnerText.Trim());
			}
			this.myIsEnable = Tools.StrToBool(this.strEnable);
			this.myText = this.strText;
			Hashtable section = this.myHost.ConfigurationInfo.getSection("Systems");
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
			this.tradeLibrary.CommunicationUrl=this.myCommunicationUrl;
			this.tradeLibrary.IsWriteLog=Global.IsWriteLog;
			this.tradeLibrary.Initialize();
		}
		public bool Logon(ref string info)
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
					this.myHost.ConfigurationInfo.updateValue("Systems", "CurServer", this.ServetType.ToString());
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
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
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
					ChgMappingPwdRequestVO chgMappingPwdRequestVO = new ChgMappingPwdRequestVO();
					chgMappingPwdRequestVO.UserID=this.myHost.SysLogonInfo.TraderID;
					chgMappingPwdRequestVO.OldPassword=oldPWD;
					chgMappingPwdRequestVO.NewPassword=newPWD;
					ResponseVO responseVO = this.tradeLibrary.ChgMappingPwd(chgMappingPwdRequestVO);
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
		public bool MixUser(MixAccountInfo mixAccountInfo, ref string info)
		{
			bool result = false;
			MixUserRequestVO mixUserRequestVO = new MixUserRequestVO();
			mixUserRequestVO.MMappingUserID=mixAccountInfo.MainAccount;
			mixUserRequestVO.UserID=mixAccountInfo.UserID;
			mixUserRequestVO.MappingType=mixAccountInfo.MapType;
			mixUserRequestVO.ModuleID=mixAccountInfo.ModuleID;
			foreach (UserInfo current in mixAccountInfo.ChildAccounts)
			{
				AccountInfo accountInfo = new AccountInfo();
				accountInfo.MWUserID=current.UserID;
				accountInfo.Password=current.PassWord;
				mixUserRequestVO.AccountInfoList.Add(accountInfo);
			}
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
					ResponseVO responseVO = this.tradeLibrary.MixUser(mixUserRequestVO);
					if (responseVO.RetCode == 0L)
					{
						result = true;
						string text = "绑定成功";
						info = text;
					}
					else
					{
						string str = "绑定失败";
						info = str + responseVO.RetMessage;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return result;
		}
		public MixAccountInfo GetMappingUser(UserInfo userInfo, ref string info)
		{
			GetMappingUserRequestVO getMappingUserRequestVO = new GetMappingUserRequestVO();
			getMappingUserRequestVO.UserID=userInfo.UserID;
			getMappingUserRequestVO.Password=userInfo.PassWord;
			getMappingUserRequestVO.ModuleID=userInfo.ModuleID;
			MixAccountInfo mixAccountInfo = new MixAccountInfo();
			GetMappingUserResponseVO getMappingUserResponseVO = new GetMappingUserResponseVO();
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
					getMappingUserResponseVO = this.tradeLibrary.GetMappingUser(getMappingUserRequestVO);
					if (getMappingUserResponseVO.RetCode == 0L)
					{
						mixAccountInfo.MainAccount = getMappingUserResponseVO.MappingUser;
						foreach (MappingUser_Info current in getMappingUserResponseVO.MappingUser_InfoList)
						{
							UserInfo userInfo2 = new UserInfo();
							userInfo2.UserID = current.MappingUserID;
							userInfo2.ModuleID = current.ModuleID;
							mixAccountInfo.ChildAccounts.Add(userInfo2);
						}
						string text = "获取成功";
						info = text;
					}
					else
					{
						mixAccountInfo = null;
						string str = "获取失败";
						info = str + getMappingUserResponseVO.RetMessage;
					}
				}
				catch (Exception ex)
				{
					info = ex.Message;
				}
			}
			return mixAccountInfo;
		}
		public bool CheckMappingUser(UserInfo userInfo, ref string info)
		{
			CheckMappingUserRequestVO checkMappingUserRequestVO = new CheckMappingUserRequestVO();
			checkMappingUserRequestVO.UserID=userInfo.UserID;
			checkMappingUserRequestVO.Password=userInfo.PassWord;
			checkMappingUserRequestVO.ModuleID=userInfo.ModuleID;
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
					ResponseVO responseVO = this.tradeLibrary.CheckMappingUser(checkMappingUserRequestVO);
					if (responseVO.RetCode == 0L)
					{
						result = true;
						string text = "校验成功";
						info = text;
					}
					else
					{
						string str = "校验失败";
						info = str + responseVO.RetMessage;
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
			return true;
		}
		private bool Logoff(ref string info)
		{
			return true;
		}
		public Form GetForm(bool isLoad, ref string info)
		{
			return null;
		}
		private void mainForm_UpdateStyleEvent(object sender, EventArgs e)
		{
		}
		private void mainForm_FloatFormEvent(object sender, EventArgs e)
		{
		}
		private void mainForm_CloseFormEvent(object sender, EventArgs e)
		{
		}
		private void mainForm_ChangeServerEvent(object sender, EventArgs e)
		{
		}
		private void mainForm_LockFormEvent(object sender, EventArgs e)
		{
		}
		private void mainForm_KLineEvent(object sender, InterFace.CommodityInfoEventArgs e)
		{
		}
		private void mainForm_MinLineEvent(object sender, InterFace.CommodityInfoEventArgs e)
		{
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
