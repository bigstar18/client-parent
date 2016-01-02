using HttpTrade.Gnnt.ISSUE.Lib;
using PluginInterface;
using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using ToolsLibrary.XmlUtil;
using TPME.Log;
using TradeClientApp.Gnnt.ISSUE;
using TradeClientApp.Gnnt.ISSUE.Library;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace Gnnt.ISSUE.Module.TradePlugin
{
	public class TradePlugin : IPlugin
	{
		private class ScreenSize
		{
			public int width;
			public int hight;
		}
		private int myPluginNO = 2;
		private string myName = "ISSUE_Trade";
		private string myConfigFileName = "ISSUE_TradePlugin.xml";
		private string mySettingConfigName = "ISSUE_Trade.xml";
		private Hashtable myHashConfigSettings = new Hashtable();
		private string myDescription = "投资品交易系统插件";
		private string myAuthor = "薛计涛";
		private string myText = string.Empty;
		private bool myIsEnable = true;
		private bool myIsNeedLoad = true;
		private DisplayTypes myDisplayType = 4;
		private Form myPreLoginForm;
		private Form myLoginedForm;
		private string myIpAddress = string.Empty;
		private int myPort;
		private string myCommunicationUrl = string.Empty;
		private string strHttp = "http://";
		private IPluginHost myHost;
		private PluginConfigInfo pluginConfigInfo;
		private TradeLibrary tradeLibrary;
		private bool isLogin;
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
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			string text5 = string.Empty;
			XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("ConfigInfo");
			if (xmlElement.SelectSingleNode("Enable") != null)
			{
				text = xmlElement.SelectSingleNode("Enable").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("Text") != null)
			{
				text2 = xmlElement.SelectSingleNode("Text").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("Http") != null)
			{
				this.strHttp = xmlElement.SelectSingleNode("Http").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CommunicationUrl") != null)
			{
				text5 = xmlElement.SelectSingleNode("CommunicationUrl").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CurTelecomServer") != null)
			{
				text3 = xmlElement.SelectSingleNode("CurTelecomServer").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("CurNetcomServer") != null)
			{
				text4 = xmlElement.SelectSingleNode("CurNetcomServer").InnerText.Trim();
			}
			if (xmlElement.SelectSingleNode("DisplayTypes") != null)
			{
				this.myDisplayType = int.Parse(xmlElement.SelectSingleNode("DisplayTypes").InnerText.Trim());
			}
			this.myIsEnable = Tools.StrToBool(text);
			this.myText = text2;
			Hashtable section = this.myHost.get_ConfigurationInfo().getSection("Systems");
			int num;
			XmlNode xmlNode;
			if (Tools.StrToInt((string)section["CurServer"]) == 0)
			{
				num = Tools.StrToInt(text3);
				xmlNode = xmlElement.SelectSingleNode("AllTelecomServer");
			}
			else
			{
				num = Tools.StrToInt(text4);
				xmlNode = xmlElement.SelectSingleNode("AllNetcomServer");
			}
			if (num == -1)
			{
				Random random = new Random();
				num = random.Next(xmlNode.ChildNodes.Count);
			}
			XmlElement xmlElement2 = (XmlElement)xmlNode.ChildNodes[num];
			if (xmlElement2.SelectSingleNode("IPAddress") != null)
			{
				this.myIpAddress = xmlElement2.SelectSingleNode("IPAddress").InnerText.Trim();
			}
			if (xmlElement2.SelectSingleNode("Port") != null)
			{
				this.myPort = Tools.StrToInt(xmlElement2.SelectSingleNode("Port").InnerText.Trim());
			}
			this.myCommunicationUrl = string.Concat(new object[]
			{
				this.strHttp,
				this.myIpAddress,
				":",
				this.myPort,
				"/",
				text5
			});
			this.tradeLibrary = new TradeLibrary();
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
				info = "没有发现插件拥有者，登陆失败！";
			}
			else if (this.tradeLibrary == null)
			{
				info = "没有初始化通讯对象，登陆失败！";
			}
			else
			{
				try
				{
					LogonRequestVO logonRequestVO = new LogonRequestVO();
					logonRequestVO.set_UserID(this.myHost.get_SysLogonInfo().TraderID);
					logonRequestVO.set_Password(this.myHost.get_SysLogonInfo().Password);
					logonRequestVO.set_RegisterWord(this.myHost.get_SysLogonInfo().RegisterWord);
					logonRequestVO.set_VersionInfo(this.myHost.get_SysLogonInfo().VersionInfo);
					logonRequestVO.set_LogonType("pc");
					LogonResponseVO logonResponseVO = this.tradeLibrary.Logon(logonRequestVO);
					if (logonResponseVO != null && logonResponseVO.RetCode == 0L)
					{
						result = true;
						this.isLogin = true;
						this.myHost.get_SysLogonInfo().LastTime = logonResponseVO.get_LastTime();
						this.myHost.get_SysLogonInfo().LastIP = logonResponseVO.get_LastIP();
						this.myHost.get_SysLogonInfo().ChgPWD = logonResponseVO.get_ChgPWD();
					}
					else
					{
						info = "登陆失败！" + logonResponseVO.RetMessage;
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
					ResponseVO responseVO = this.tradeLibrary.ChangePwd(chgPwdRequestVO);
					if (responseVO.RetCode == 0L)
					{
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
					checkUserRequestVO.set_LogonType("pc");
					ResponseVO responseVO = this.tradeLibrary.CheckUser(checkUserRequestVO);
					if (responseVO != null && responseVO.RetCode == 0L)
					{
						result = true;
						this.isLogin = true;
						info = "通过身份验证！";
					}
					else
					{
						info = "没配权限或登录已超时！" + responseVO.RetMessage;
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
					}
					else
					{
						info = "注销失败！" + responseVO.RetMessage;
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
				info = "交易系统没有发现配置信息！";
				return null;
			}
			if (this.CheckUser(ref info))
			{
				return this.GetCurrentForm(ref info);
			}
			info = "没有权限登陆" + this.myText + "错误原因：" + info;
			return null;
		}
		private Form GetCurrentForm(ref string info)
		{
			if (this.myLoginedForm == null || this.myLoginedForm.IsDisposed)
			{
				Global.HTConfig = XmlParse.getSection(this.myHashConfigSettings, "Trade");
				Global.M_ResourceManager = this.myHost.get_ISSUE_ResourceManager();
				Global.UserID = this.myHost.get_SysLogonInfo().TraderID;
				Global.Password = this.myHost.get_SysLogonInfo().Password;
				Global.RegisterWord = this.myHost.get_SysLogonInfo().RegisterWord;
				Global.TradeLibrary = this.tradeLibrary;
				Global.ConfigPath = this.pluginConfigInfo.XmlPath;
				this.setScreenSize();
				try
				{
					MainForm mainForm = new MainForm();
					InterFace.TopLevel = false;
					mainForm.toolStrip.Items.RemoveAt(mainForm.toolStrip.Items.Count - 1);
					mainForm.add_MinLineEvent(new InterFace.CommodityInfoEventHander(this.mainForm_MinLineEvent));
					mainForm.add_KLineEvent(new InterFace.CommodityInfoEventHander(this.mainForm_KLineEvent));
					mainForm.add_LockFormEvent(new EventHandler(this.mainForm_LockFormEvent));
					this.myLoginedForm = mainForm;
				}
				catch (Exception)
				{
					info = "";
				}
			}
			return this.myLoginedForm;
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
						MainForm mainForm = (MainForm)this.myLoginedForm;
						mainForm.SetOrderInfo(text, Tools.StrToFloat(text2, 0f), Tools.StrToFloat(text3, 0f));
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
				mainForm.CloseMainForm();
			}
			if (this.myLoginedForm != null && !this.myLoginedForm.IsDisposed)
			{
				MainForm mainForm2 = (MainForm)this.myLoginedForm;
				mainForm2.CloseMainForm();
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
