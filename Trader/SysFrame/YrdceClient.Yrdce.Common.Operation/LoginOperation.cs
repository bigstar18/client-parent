using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation.Manager;
using System;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace YrdceClient.Yrdce.Common.Operation
{
	public class LoginOperation
	{
		public delegate bool ShowCommitFormCallBack();
		public delegate bool ShowBalanceFormCallBack();
		public delegate void BooleanCallback(bool enable, string info);
		public delegate void ShowClient();
		public LoginOperation.ShowCommitFormCallBack ShowCommitForm;
		public LoginOperation.ShowBalanceFormCallBack ShowBalanceForm;
		public LoginOperation.BooleanCallback enableControls;
		public LoginOperation.ShowClient showClient;
		public void Connect(object obj)
		{
			LogonOperationInfo logonOperationInfo = (LogonOperationInfo)obj;
			if (OperationManager.GetInstance().stripButtonOperation.curLoginPluginName == "OTC_AgencyTrade")
			{
				Global.Modules.Plugins.SysLogonInfo.AgencyNo = logonOperationInfo.username;
				Global.Modules.Plugins.SysLogonInfo.AgencyPhonePassword = logonOperationInfo.password;
			}
			Global.Modules.Plugins.SysLogonInfo.TraderID = logonOperationInfo.username;
			Global.Modules.Plugins.SysLogonInfo.Password = logonOperationInfo.password;
			Global.Modules.Plugins.SysLogonInfo.VersionInfo = (string)Global.htConfig["Version"];
			Global.Modules.Plugins.SysLogonInfo.str = logonOperationInfo.loginmark.ToString();
			string text = string.Empty;
			bool flag = false;
			int num = Tools.StrToInt((string)Global.htConfig["LoadMode"], 0);
			if (num == 1 || num == 2)
			{
				string text2 = logonOperationInfo.axkey.VerifyUser(Tools.StrToShort((string)Global.htConfig["MarketID"]), logonOperationInfo.username);
				if (text2.Equals("-10"))
				{
					string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_readKeyError");
					text = @string;
				}
				else if (text2.Equals("-1"))
				{
					if (num == 2)
					{
						string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_insertKey");
						text = string2;
					}
					else
					{
						flag = logonOperationInfo.myPlugin.Logon(ref text);
					}
				}
				else if (text2.Equals("-2"))
				{
					string string3 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_KeyTypeError");
					text = string3;
				}
				else if (text2.Equals("-3"))
				{
					string string4 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_KeyAndIdNotMatch");
					text = string4;
				}
				else if (text2.Equals("-4"))
				{
					string string5 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_KeyBroken");
					text = string5;
				}
				else
				{
					Global.Modules.Plugins.SysLogonInfo.RegisterWord = text2;
					flag = logonOperationInfo.myPlugin.Logon(ref text);
				}
			}
			else if (OperationManager.GetInstance().stripButtonOperation.curLoginPluginName == "OTC_AgencyTrade")
			{
				flag = logonOperationInfo.myPlugin.AgencyLogon(ref text);
			}
			else
			{
				flag = logonOperationInfo.myPlugin.Logon(ref text);
			}
			Logger.wirte(MsgType.Information, string.Concat(new object[]
			{
				"登陆返回信息：",
				flag,
				"  ",
				text
			}));
			if (!flag)
			{
				if (text == "")
				{
					string string6 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_LoginFail");
					text = string6;
				}
				string string7 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_UnableConnect");
                if (text=="登陆失败！")
                {
                    DialogResult r = MessageBox.Show(text+"请检查网络设置后重新登陆！", string7, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.enableControls(true, text);
                    if(r == System.Windows.Forms.DialogResult.OK)
                    {
                       System.Environment.Exit(0);
                    }
                    
                    return;
                }
                else
                {
                   MessageBox.Show(text, string7, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.enableControls(true, text);
                   return;
                }
			}
			OperationManager.GetInstance().verifyCodeOparation.VerifyPassWord(Global.Modules.Plugins.SysLogonInfo.Password);
			if (this.ShowCommitForm != null && !this.ShowCommitForm())
			{
				return;
			}
			if (this.ShowBalanceForm != null && !this.ShowBalanceForm())
			{
				return;
			}
			if (this.showClient != null)
			{
				//登录
                this.showClient();
			}
		}
	}
}
