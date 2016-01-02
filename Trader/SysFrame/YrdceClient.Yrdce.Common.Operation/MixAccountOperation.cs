using Gnnt.MixAccountPlugin;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation.Manager;

using System;
using System.Windows.Forms;
namespace YrdceClient.Yrdce.Common.Operation
{
	public class MixAccountOperation
	{
		public delegate void RemoveControlCallBack(bool isChecked);
		public delegate void CheckAccountCallBack(bool isChecked, UserInfo userinfo);
		public MixAccountOperation.RemoveControlCallBack removeControlCallBack;
		public MixAccountOperation.CheckAccountCallBack checkAccountCallBack;
		
        //public void ShowChooseForm()
        //{
        //    this.chooseForm.ShowDialog();
        //}
        //public void ShowMixAccountInfoForm()
        //{
        //    this.chooseForm.Hide();
        //    MixAccountForm mixAccountForm = new MixAccountForm();
        //    mixAccountForm.ShowDialog();
        //}
		public bool GetMappingAccountInfo(UserInfo userInfo)
		{
			bool result = false;
			string text = "";
			MixAccountInfo mappingUser = OperationManager.GetInstance().mixAccountPlugin.GetMappingUser(userInfo, ref text);
			if (mappingUser == null)
			{
				result = false;
			}
			else
			{
				result = true;
				Global.MainAccount = mappingUser.MainAccount;
				foreach (UserInfo current in mappingUser.ChildAccounts)
				{
					if (current != null)
					{
						Global.ChildAccounts.Add(current.ModuleID, current.UserID);
					}
				}
			}
			return result;
		}
		public bool CheckAccountInfo(UserInfo userInfo)
		{
			string text = "";
			bool flag = OperationManager.GetInstance().mixAccountPlugin.CheckMappingUser(userInfo, ref text);
			if (!flag)
			{
				string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_Error");
				if (text.Length > 0)
				{
					MessageBox.Show(text, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			return flag;
		}
		public bool MixAccount(MixAccountInfo mixInfo)
		{
			string text = "";
			bool flag = OperationManager.GetInstance().mixAccountPlugin.MixUser(mixInfo, ref text);
			if (!flag)
			{
				string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_Error");
				if (text.Length > 0)
				{
					MessageBox.Show(text, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			return flag;
		}
	}
}
