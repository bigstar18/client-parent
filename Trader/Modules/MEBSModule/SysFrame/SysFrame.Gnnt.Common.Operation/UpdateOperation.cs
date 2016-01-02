using AppUpdate;
using SysFrame.Gnnt.Common.Library;
using System;
using System.Threading;
using System.Windows.Forms;
namespace SysFrame.Gnnt.Common.Operation
{
	public class UpdateOperation
	{
		public delegate void CloseFormCallBack();
		private string UpdateList = "UpdateList.xml";
		public bool updateColse;
		private bool bUpdate = true;
		private Thread startCheckUpdate;
		public UpdateOperation.CloseFormCallBack CloseForm;
		public void StartCheckUpdate(object obj)
		{
			if (!this.bUpdate)
			{
				return;
			}
			this.bUpdate = false;
			this.startCheckUpdate = new Thread(new ParameterizedThreadStart(this.CheckUpdate));
			this.startCheckUpdate.Start(obj);
		}
		private void CheckUpdate(object obj)
		{
			Form form = (Form)obj;
			CheckForUpdate checkForUpdate = new CheckForUpdate(this.UpdateList);
			if (checkForUpdate.StartCheck())
			{
				if (checkForUpdate.UpdateLevel == 1)
				{
					checkForUpdate.StartUpdate();
					this.updateColse = true;
					MethodInvoker method = new MethodInvoker(form.Close);
					if (form != null)
					{
						form.BeginInvoke(method);
						return;
					}
					Application.Exit();
					return;
				}
				else
				{
					string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UpdateTitle");
					string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UpdateContext");
					if (MessageBox.Show(Form.ActiveForm, string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
					{
						checkForUpdate.StartUpdate();
						this.updateColse = true;
						MethodInvoker method2 = new MethodInvoker(form.Close);
						if (form != null)
						{
							form.BeginInvoke(method2);
							return;
						}
						Application.Exit();
					}
				}
			}
		}
	}
}
