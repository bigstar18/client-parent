using ModulesLoader;
using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.UI.Forms.PromptForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.Common.Operation.MainOperation
{
	public class StripButtonOperation
	{
		public delegate void RefreshPanelCallBack(PanelLoad panelLoad, FormandPlugin temp);
		public delegate void CreateLoginForm(IPlugin plugin);
		public delegate void LoadClickFormCallBack();
		public StripButtonOperation.RefreshPanelCallBack RefreshPanel;
		public StripButtonOperation.CreateLoginForm createLoginForm;
		private StripButtonOperation.LoadClickFormCallBack loadClickFormCallBack;
		private string Nodes = "nodes.xml";
		public bool isReloadPlugin;
		private bool isLogin;
		private bool isAgencyLogin;
		public string curLoginPluginName = string.Empty;
		private List<IPlugin> pluginsList;
		private ToolStrip ToolStripButtons;
		private ToolStripItem curToolStripItem;
		private ThreadStartAddNodes s;
		public Image menuClickBackImage;
		private List<FormandPlugin> FormandPluginList = new List<FormandPlugin>();
		public void StripButtonNodeLoad(ToolStrip toolStripButtons)
		{
			this.ToolStripButtons = toolStripButtons;
			this.s = new ThreadStartAddNodes(toolStripButtons.Items, this.Nodes);
			this.s.ToolStripButtonClick = new ThreadStartAddNodes.ToolsStripButtonClickCallBack(this.ToolStripButtonClickForm);
			new Thread(new ParameterizedThreadStart(this.s.LoadNode)).Start(this.isLogin);
		}
		private void ToolStripButtonClickForm()
		{
			this.loadClickFormCallBack = new StripButtonOperation.LoadClickFormCallBack(this.LoadCheckForm);
			this.ToolStripButtons.Invoke(this.loadClickFormCallBack);
		}
		private void LoadCheckForm()
		{
			this.ShowLoginedNodes();
			ToolStripItem toolStripItem = this.ToolStripButtons.Items[this.curLoginPluginName];
			if (toolStripItem != null)
			{
				this.menuClickBackImage = Image.FromFile("images\\menuclick.png");
				((ToolStripLabel)toolStripItem).ToolTipText = "true";
				toolStripItem.BackgroundImage = this.menuClickBackImage;
				ToolStripItemClickedEventArgs e = new ToolStripItemClickedEventArgs(toolStripItem);
				this.isReloadPlugin = true;
				this.s_ToolStripButtonClick(e);
			}
		}
		public void s_ToolStripButtonClick(ToolStripItemClickedEventArgs e)
		{
			if (e == null || e.ClickedItem == null)
			{
				return;
			}
			List<IPlugin> list = (List<IPlugin>)e.ClickedItem.Tag;
			if (list != null)
			{
				this.pluginsList = list;
			}
			if (e.ClickedItem.Name == "login" || (e.ClickedItem.Name == "OTC_AgencyTrade" && !this.isAgencyLogin))
			{
				if (this.pluginsList != null)
				{
					if (this.createLoginForm != null)
					{
						if (this.pluginsList.Count > 1)
						{
							this.createLoginForm(this.pluginsList[1]);
						}
						if (this.pluginsList.Count == 1)
						{
							this.createLoginForm(this.pluginsList[0]);
							return;
						}
					}
				}
				else if (this.createLoginForm != null)
				{
					this.createLoginForm(null);
					return;
				}
			}
			else
			{
				if (e.ClickedItem.Name.Equals("ChangePW"))
				{
					this.ChangePassWord();
					return;
				}
				if (e.ClickedItem.Name.Equals("logout"))
				{
					string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_SysForm_canceled");
					string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_SysForm_canceledContext");
					if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
					{
						this.Logout();
						return;
					}
				}
				else
				{
					this.FormandPluginList.Clear();
					string empty = string.Empty;
					for (int i = 0; i < this.pluginsList.Count; i++)
					{
						if (this.curToolStripItem == null || this.curToolStripItem != e.ClickedItem || this.isReloadPlugin)
						{
							IPlugin arg_19A_0 = this.pluginsList[i];
							this.displayPluginForm(this.pluginsList[i], ref empty);
						}
					}
					if (!this.isLogin || this.FormandPluginList.Count == this.pluginsList.Count)
					{
						if (this.FormandPluginList.Count <= 0)
						{
							return;
						}
						if (this.FormandPluginList[0].plugin.get_DisplayType() != 1)
						{
							this.RefreshPanel(PanelLoad.RemoveHQTrade, null);
						}
						using (List<FormandPlugin>.Enumerator enumerator = this.FormandPluginList.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								FormandPlugin current = enumerator.Current;
								if (current.plugin.get_DisplayType() == null)
								{
									if (this.RefreshPanel != null)
									{
										this.RefreshPanel(PanelLoad.HQPanelLoad, current);
									}
								}
								else if (current.plugin.get_DisplayType() == 4)
								{
									if (this.RefreshPanel != null)
									{
										this.RefreshPanel(PanelLoad.TradePanelLoad, current);
									}
								}
								else if (current.plugin.get_DisplayType() == 1 && current.form.Tag != null)
								{
									Process.Start(current.form.Tag.ToString());
								}
							}
							goto IL_316;
						}
					}
					if (this.curToolStripItem != e.ClickedItem)
					{
						string string3 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_SysForm_Error");
						if (empty.Length > 0)
						{
							MessageBox.Show(empty, string3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						return;
					}
					IL_316:
					this.curLoginPluginName = e.ClickedItem.Name;
					IPlugin plugin = this.pluginsList[0];
					if (!plugin.get_Name().Contains("HQ"))
					{
						OperationManager.GetInstance().changePassWordOperation.myPlugin = plugin;
					}
					else if (this.pluginsList.Count > 1)
					{
						OperationManager.GetInstance().changePassWordOperation.myPlugin = this.pluginsList[1];
					}
					if (plugin.get_DisplayType() == 1)
					{
						SysShareInfo.FromLogonType = "web";
					}
					else
					{
						SysShareInfo.FromLogonType = "pc";
					}
					this.curToolStripItem = e.ClickedItem;
				}
			}
		}
		public void displayPluginForm(IPlugin plugin, ref string info)
		{
			if (plugin == null)
			{
				return;
			}
			Form form = null;
			try
			{
				form = plugin.GetForm(this.isLogin, ref info);
			}
			catch (Exception)
			{
				info = "窗体初始化失败！！！";
			}
			if (form != null)
			{
				FormandPlugin formandPlugin = new FormandPlugin();
				formandPlugin.form = form;
				formandPlugin.plugin = plugin;
				this.FormandPluginList.Add(formandPlugin);
			}
		}
		private void ChangePassWord()
		{
			ChangePW changePW = new ChangePW(false);
			changePW.ShowDialog();
		}
		public void Logout()
		{
			if (this.RefreshPanel != null)
			{
				this.RefreshPanel(PanelLoad.RemoveHQTrade, null);
			}
			Global.Modules.get_Plugins().ClosePlugins();
			Global.Modules.LoadPlugins();
			Global.ModuleInfos.Clear();
			foreach (ModuleInfo current in Global.Modules.get_Modules())
			{
				Global.ModuleInfos.Add(current.get_ModuleNo(), current.get_ModuleName());
			}
			this.isLogin = false;
			this.isAgencyLogin = false;
			Global.Modules.get_Plugins().get_SysLogonInfo().RegisterWord = string.Empty;
			Global.Modules.get_Plugins().get_SysLogonInfo().Password = string.Empty;
			Global.Modules.get_Plugins().get_SysLogonInfo().TraderID = string.Empty;
			Global.Modules.get_Plugins().get_SysLogonInfo().VersionInfo = string.Empty;
			Global.htConfig = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems");
			OperationManager.GetInstance().sysFrameOperation.isFloatTrade = false;
			OperationManager.GetInstance().sysFrameOperation.isLockTrade = false;
			if (this.curLoginPluginName != "")
			{
				this.curLoginPluginName = "";
			}
			if (this.pluginsList != null)
			{
				this.pluginsList = null;
			}
			if (this.curToolStripItem != null)
			{
				this.curToolStripItem = null;
			}
			OperationManager.GetInstance().isLogin = false;
			OperationManager.GetInstance().PluginNameList.Clear();
			this.StripButtonNodeLoad(this.ToolStripButtons);
		}
		public void SetIsLogin(bool _isLogin, string _curLoginPluginName)
		{
			if (this.curLoginPluginName == "OTC_AgencyTrade")
			{
				this.isAgencyLogin = true;
			}
			else
			{
				this.isLogin = _isLogin;
			}
			if (this.s != null)
			{
				this.ToolStripButtonClickForm();
				if (!this.isAgencyLogin)
				{
					this.s.addNodeChild(null, this.ToolStripButtons.Items);
				}
				this.RemoveLoginButton();
			}
		}
		private void RemoveLoginButton()
		{
			ToolStripItem value = this.ToolStripButtons.Items["login"];
			this.ToolStripButtons.Items.Remove(value);
		}
		private void ShowLoginedNodes()
		{
			if (this.isLogin)
			{
				foreach (string current in this.s.NodeNameList)
				{
					ToolStripItem toolStripItem = this.ToolStripButtons.Items[current];
					if (current == "OTC_AgencyTrade")
					{
						if (Global.Modules.get_Plugins().get_IdentityType() == "1")
						{
							toolStripItem.Visible = true;
						}
					}
					else if (current == "OTC_BankInterface")
					{
						if (Global.Modules.get_Plugins().get_IdentityType() == "0")
						{
							toolStripItem.Visible = true;
						}
					}
					else
					{
						toolStripItem.Visible = true;
					}
				}
			}
		}
	}
}
