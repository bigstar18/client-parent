using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.UI.Forms.PromptForms;
using System;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.Common.Operation
{
	public class DisplayFormsOperation
	{
		public void displayStyle()
		{
			bool flag = Tools.StrToBool((string)Global.htConfig["FirstRunApp"], false);
			if (flag)
			{
				StyleForm styleForm = new StyleForm();
				styleForm.ShowDialog();
			}
		}
		public bool displayCommit()
		{
			bool result = true;
			bool flag = Tools.StrToBool((string)Global.htConfig["IsDisplayCommit"], false);
			bool flag2 = Tools.StrToBool((string)Global.htConfig["FirstLogon"], true);
			string text = (string)Global.htConfig["IsDisplayCommitday"];
			if (flag && flag2 && !text.Equals(DateTime.Now.Date.ToShortDateString()))
			{
				CommitForm commitForm = new CommitForm();
				commitForm.ShowDialog();
				result = (commitForm.DialogResult != DialogResult.No);
			}
			return result;
		}
		public bool displayBalance()
		{
			bool result = true;
			bool flag = Tools.StrToBool((string)Global.htConfig["IsDisplayBalance"], false);
			if (flag)
			{
				string uriString = (string)Global.htConfig["ReportUrl"] + "&firmid=" + Global.Modules.get_Plugins().get_SysLogonInfo().TraderID;
				BalanceForm balanceForm = new BalanceForm();
				balanceForm.Url = new Uri(uriString);
				balanceForm.ShowDialog();
				result = (balanceForm.DialogResult != DialogResult.No);
			}
			return result;
		}
		public bool displayChangePwd(IPlugin myPlugin)
		{
			bool flag = false;
			try
			{
				if (Global.Modules.get_Plugins().get_SysLogonInfo().ChgPWD.Equals("1"))
				{
					string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UpdatePasswordTitle");
					string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UpdatePasswordContext");
					MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					flag = OperationManager.GetInstance().changePassWordOperation.ShowChangePasswordForm(myPlugin);
					if (!flag)
					{
						string string3 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UnableConnect");
						string string4 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_NoUpdatePassword");
						MessageBox.Show(string4, string3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}
	}
}
