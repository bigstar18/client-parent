using YrdceClient.Yrdce.Common.Library;
using YrdceClient.UI.Forms.PromptForms;
using System;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace YrdceClient.Yrdce.Common.Operation
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
				string uriString = (string)Global.htConfig["ReportUrl"] + "&firmid=" + Global.Modules.Plugins.SysLogonInfo.TraderID;
				BalanceForm balanceForm = new BalanceForm();
				balanceForm.Url = new Uri(uriString);
				balanceForm.ShowDialog();
				result = (balanceForm.DialogResult != DialogResult.No);
			}
			return result;
		}
	}
}
