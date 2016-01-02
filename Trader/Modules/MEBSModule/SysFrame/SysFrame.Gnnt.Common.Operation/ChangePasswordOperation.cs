using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.UI.Forms.PromptForms;
using System;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.Common.Operation
{
	public class ChangePasswordOperation
	{
		public delegate void SetFocusCallBack(short flag);
		public bool isChangeFirstPwd = true;
		public IPlugin myPlugin;
		public ChangePasswordOperation.SetFocusCallBack SetFocus;
		public bool ShowChangePasswordForm(IPlugin plugin)
		{
			this.myPlugin = plugin;
			ChangePW changePW = new ChangePW(Tools.StrToBool((string)Global.htConfig["ForceChangePassword"], false));
			changePW.ShowDialog();
			bool result;
			if (changePW.DialogResult == DialogResult.No)
			{
				this.myPlugin.Dispose();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		public bool ChangePassword(ChangePassWordInfo changepwdInfo)
		{
			bool result = false;
			string text = string.Empty;
			if (changepwdInfo.oldPwd.Length == 0)
			{
				string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_ModifyResults");
				string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_OldPWEmpty");
				MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				this.SetFocus(0);
			}
			else if (changepwdInfo.newPwd.Length == 0)
			{
				string string3 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_ModifyResults");
				string string4 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_NewPWEmpty");
				MessageBox.Show(string4, string3, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				this.SetFocus(1);
			}
			else if (changepwdInfo.confirmPwd.Length == 0)
			{
				string string5 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_ModifyResults");
				string string6 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_RepeatPWEmpty");
				MessageBox.Show(string6, string5, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				this.SetFocus(2);
			}
			else if (!changepwdInfo.oldPwd.Equals(Global.Modules.get_Plugins().get_SysLogonInfo().Password))
			{
				string string7 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_OldPWError");
				text += string7;
			}
			else if (!changepwdInfo.newPwd.Equals(changepwdInfo.confirmPwd))
			{
				string string8 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_PWDifferError");
				text += string8;
			}
			else if (changepwdInfo.newPwd.Equals(changepwdInfo.oldPwd))
			{
				string string9 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_PWSameError");
				text += string9;
			}
			else if (Tools.StrToBool((string)Global.htConfig["IsModifyPassword"], false))
			{
				if (changepwdInfo.newPwd.Length < 8)
				{
					string string10 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_PWBuildError");
					text += string10;
				}
				else
				{
					bool flag = this.myPlugin.chgPWD(1, changepwdInfo.newPwd, changepwdInfo.oldPwd, ref text);
					if (flag)
					{
						Global.Modules.get_Plugins().get_SysLogonInfo().Password = changepwdInfo.newPwd;
						result = true;
					}
				}
			}
			else
			{
				bool flag2 = this.myPlugin.chgPWD(1, changepwdInfo.newPwd, changepwdInfo.oldPwd, ref text);
				if (flag2)
				{
					Global.Modules.get_Plugins().get_SysLogonInfo().Password = changepwdInfo.newPwd;
					result = true;
				}
			}
			if (text != "")
			{
				string string11 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_ModifyResults");
				MessageBox.Show(text, string11, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			return result;
		}
		public bool VerifyNewPassWord(ChangePassWordInfo changepwdInfo)
		{
			return changepwdInfo.newPwd.Equals(changepwdInfo.oldPwd);
		}
	}
}
