using DIYForm;
using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation;
using SysFrame.Gnnt.Common.Operation.MainOperation;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.Gnnt.UI.Forms.ContainerManager;
using SysFrame.UI.Forms.PromptForms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame
{
	public class Frame : MyForm
	{
		private OperationManager operationManager = OperationManager.GetInstance();
		private Hashtable htConfig;
		private int sleepTime = 1000;
		private IContainer components;
		private Timer zcjsTimer;
		private ToolTip toolTipText;
		private Panel panelMain;
		public Frame(string[] args)
		{
			this.InitializeComponent();
			this.ControlLoad(args);
		}
		private void ControlLoad(string[] args)
		{
			try
			{
				this.operationManager.sysFrameOperation.AnalyticalParamenter(args);
				this.operationManager.sysFrameOperation.SetFormInfo(this);
				ContainerManage containerManage = new ContainerManage(this.panelMain);
				containerManage.ControlLayOut();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
			}
		}
		private void Frame_Shown(object sender, EventArgs e)
		{
			if (!MarketInfo.isLoadedForm)
			{
				MarketInfo marketInfo = new MarketInfo();
				marketInfo.Show();
			}
		}
		private void Frame_Load(object sender, EventArgs e)
		{
			this.htConfig = Global.htConfig;
			if (this.SetControlText())
			{
				OperationManager.GetInstance().updateOperation.StartCheckUpdate(this);
			}
			else
			{
				string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_SelectResource");
				string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_NoResource");
				MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
			this.operationManager.stripButtonOperation.createLoginForm = new StripButtonOperation.CreateLoginForm(this.CreateLoginForm);
		}
		private void CreateLoginForm(IPlugin plugin)
		{
			ProgramOperation.CreateLogin(plugin, true);
		}
		private bool SetControlText()
		{
			bool result;
			try
			{
				ScaleForm.ScaleForms(this);
				base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
				if (this.htConfig != null)
				{
					string arg = string.Empty;
					string text = string.Empty;
					if (this.htConfig.Contains("Title"))
					{
						arg = this.htConfig["Title"].ToString();
					}
					if (this.htConfig.Contains("Version"))
					{
						text = this.htConfig["Version"].ToString();
					}
					if (string.IsNullOrEmpty(text))
					{
						this.Text = string.Format("{0}", arg);
					}
					else
					{
						this.Text = string.Format("{0}({1})", arg, text);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
				result = false;
			}
			return result;
		}
		private void Frame_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.operationManager.updateOperation.updateColse)
			{
				TimerForm timerForm = new TimerForm();
				timerForm.ShowDialog();
				if (!timerForm.Result)
				{
					e.Cancel = true;
					return;
				}
				Global.Modules.get_Plugins().ClosePlugins();
			}
		}
		private void Frame_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				Logger.UnInitLogger();
				base.Dispose();
				Application.ExitThread();
				Application.Exit();
				Process.GetCurrentProcess().Kill();
			}
			catch (Exception)
			{
				base.Dispose();
				Application.ExitThread();
				Application.Exit();
				Process.GetCurrentProcess().Kill();
			}
		}
		private void Frame_SizeChanged(object sender, EventArgs e)
		{
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			return keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Right || keyData == Keys.Left || base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 74)
			{
				base.DefWndProc(ref m);
				return;
			}
			Type type = default(CopyDataStruct).GetType();
			CopyDataStruct copyDataStruct = (CopyDataStruct)m.GetLParam(type);
			if (this.operationManager.isLogin && Global.Modules.get_Plugins().get_SysLogonInfo().TraderID.Equals(copyDataStruct.str))
			{
				m.Result = (IntPtr)1;
				return;
			}
			m.Result = (IntPtr)0;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.zcjsTimer = new Timer(this.components);
			this.toolTipText = new ToolTip(this.components);
			this.panelMain = new Panel();
			base.SuspendLayout();
			this.zcjsTimer.Enabled = true;
			this.panelMain.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.panelMain.Location = new Point(6, 27);
			this.panelMain.Margin = new Padding(0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new Size(897, 715);
			this.panelMain.TabIndex = 11;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(910, 750);
			base.ControlBox = false;
			base.Controls.Add(this.panelMain);
			base.KeyPreview = true;
			base.Name = "Frame";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "金网统一交易软件";
			base.FormClosing += new FormClosingEventHandler(this.Frame_FormClosing);
			base.FormClosed += new FormClosedEventHandler(this.Frame_FormClosed);
			base.Load += new EventHandler(this.Frame_Load);
			base.Shown += new EventHandler(this.Frame_Shown);
			base.SizeChanged += new EventHandler(this.Frame_SizeChanged);
			base.Controls.SetChildIndex(this.panelMain, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
