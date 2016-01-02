using ModulesLoader;
using PluginInterface;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.UI.Forms.PromptForms;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
namespace YrdceClient.Yrdce.Common.Operation.MainOperation
{
	public class YrdceClientOperation
	{
		public delegate void LockTradeFormEventCallBack(bool isLock);
		public delegate void FloatTradeFormEventCallBack(bool isFloat);
		public delegate void LogOutTradeFormEventCallBack();
		public YrdceClientOperation.LockTradeFormEventCallBack lockTradeFormEventCallBack;
		public YrdceClientOperation.FloatTradeFormEventCallBack floatTradeFormEventCallBack;
		public YrdceClientOperation.LogOutTradeFormEventCallBack logOutTradeFormEventCallBack;
		public bool isLockTrade;
		public bool isFloatTrade;
		public YrdceClientOperation()
		{
			Global.Modules.Plugins.PluginCommunicateInfoEvent += new PluginService.PluginCommunicateInfoEventHander(this.Plugins_PluginCommunicateInfoEvent);
		}
		public void AnalyticalParamenter(string[] args)
		{
			if (args != null && args.Length > 0)
			{
				string key = string.Empty;
				string value = string.Empty;
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i];
					int num = text.IndexOf("=");
					if (num != -1)
					{
						key = text.Substring(0, num);
						value = text.Substring(num + 1);
						if (!Global.htArgs.ContainsKey(key))
						{
							Global.htArgs.Add(key, value);
						}
					}
				}
			}
		}
		public void SetFormInfo(Form form)
		{
			Hashtable htArgs = Global.htArgs;
			if (htArgs != null && htArgs.ContainsKey("width"))
			{
				string s = htArgs["width"].ToString();
				try
				{
					form.Width = int.Parse(s);
				}
				catch (Exception)
				{
				}
			}
			if (htArgs != null && htArgs.ContainsKey("height"))
			{
				string s2 = htArgs["height"].ToString();
				try
				{
					form.Height = int.Parse(s2);
				}
				catch (Exception)
				{
				}
			}
			if (htArgs != null && htArgs.ContainsKey("x") && htArgs.ContainsKey("y"))
			{
				string s3 = htArgs["x"].ToString();
				string s4 = htArgs["y"].ToString();
				try
				{
					form.StartPosition = FormStartPosition.Manual;
					form.Location = new Point(int.Parse(s3), int.Parse(s4));
				}
				catch (Exception)
				{
				}
			}
			if (htArgs != null && htArgs.ContainsKey("Minimized"))
			{
				form.WindowState = FormWindowState.Minimized;
				return;
			}
			form.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
			form.WindowState = FormWindowState.Maximized;
		}
		private void Plugins_PluginCommunicateInfoEvent(object sender, PluginCommunicateInfoEventArgs e)
		{
			if (e.PluginCommunicateInfo.InfoType == PluginInfoType.StrType)
			{
				if (e.PluginCommunicateInfo.InfoName.Equals("LockTradeForm"))
				{
					if (this.lockTradeFormEventCallBack != null)
					{
						this.lockTradeFormEventCallBack(this.isLockTrade);
						return;
					}
				}
				else if (e.PluginCommunicateInfo.InfoName.Equals("FloatTradeForm"))
				{
					if (this.floatTradeFormEventCallBack != null)
					{
						this.floatTradeFormEventCallBack(this.isFloatTrade);
						return;
					}
				}
				else if (e.PluginCommunicateInfo.InfoName.Equals("LogOutTradeForm"))
				{
					if (this.logOutTradeFormEventCallBack != null)
					{
                        //宋
						this.logOutTradeFormEventCallBack();
						return;
					}
				}
				else if (e.PluginCommunicateInfo.InfoName.Equals("UpdateStyle"))
				{
					StyleForm styleForm = new StyleForm();
					if (DialogResult.OK == styleForm.ShowDialog())
					{
						MessageBox.Show("修改风格成功！下次进入系统时生效！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
		}
	}
}
