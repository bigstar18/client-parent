using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using FuturesTrade.Gnnt.UI.Modules.Notifier;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Modules.Status
{
	public class StatusInfoBar : UserControl
	{
		private delegate void FillInfoTextCallBack(string infoMessage, Color color, bool display);
		private delegate void SetConnectStatusCallBack(int connectStatus);
		private delegate void ShowNotifierCallBack(string tradeInfo);
		private TaskbarNotifier Notifier = new TaskbarNotifier();
		private OperationManager operationManager = OperationManager.GetInstance();
		private StatusInfoBar.FillInfoTextCallBack fillInfoText;
		private StatusInfoBar.SetConnectStatusCallBack SetConnectStatus;
		private StatusInfoBar.ShowNotifierCallBack ShowNotifier;
		private int connectStatus;
		private IContainer components;
		private StatusStrip statusInfo;
		private ToolStripStatusLabel info;
		private ToolStripStatusLabel user;
		private ToolStripStatusLabel status;
		private ToolStripStatusLabel time;
		private System.Windows.Forms.Timer Systimer;
		private System.Windows.Forms.Timer TradeTime;
		public StatusInfoBar()
		{
			this.InitializeComponent();
			this.CreateHandle();
			Global.StatusInfoFill = new Global.StatusInfoFillCallBack(this.FillInfoText);
		}
		private void FillInfoText(string infoMessage, Color color, bool display)
		{
			try
			{
				this.fillInfoText = new StatusInfoBar.FillInfoTextCallBack(this.InfoText);
				this.HandleCreated();
				base.Invoke(this.fillInfoText, new object[]
				{
					infoMessage,
					color,
					display
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void InfoText(string infoMessage, Color color, bool display)
		{
			if (display)
			{
				this.info.ForeColor = color;
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoMessegePresentation");
				this.info.Text = @string + infoMessage;
			}
		}
		private void SetStatus(int connectStatus)
		{
			try
			{
				this.SetConnectStatus = new StatusInfoBar.SetConnectStatusCallBack(this.Status);
				this.HandleCreated();
				base.Invoke(this.SetConnectStatus, new object[]
				{
					connectStatus
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void Status(int _connectStatus)
		{
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_connectStatus1");
			string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_connectStatus2");
			this.connectStatus = _connectStatus;
			if (this.connectStatus == 0)
			{
				this.status.BackColor = Color.Lime;
				this.status.Text = string2;
				this.operationManager.SetAllQueryFlag(true);
				this.operationManager.orderOperation.SetButtonOrderEnable(true);
				return;
			}
			if (this.connectStatus == 1)
			{
				this.status.BackColor = Color.Red;
				this.status.Text = @string;
				this.operationManager.SetAllQueryFlag(false);
				this.operationManager.orderOperation.SetButtonOrderEnable(false);
				this.status_DoubleClick(null, null);
				return;
			}
			if (this.connectStatus == 2)
			{
				this.status.BackColor = Color.Red;
				this.status.Text = @string;
				this.operationManager.SetAllQueryFlag(false);
				this.operationManager.orderOperation.SetButtonOrderEnable(true);
			}
		}
		private void StatusInfoBar_SizeChanged(object sender, EventArgs e)
		{
			this.info.Width = base.Width - this.user.Width - this.status.Width - this.time.Width - 20;
		}
		private void StatusInfoBar_Load(object sender, EventArgs e)
		{
			ToolStripStatusLabel expr_06 = this.user;
			expr_06.Text += Global.UserID;
			this.operationManager.querySysTimeOperation.SetConnectStatus = new QuerySysTimeOperation.SetConnectStatusCallBack(this.SetStatus);
			this.operationManager.querySysTimeOperation.ShowNotifier = new QuerySysTimeOperation.ShowNotifierCallBack(this.NotifierMessage);
			this.Systimer.Enabled = true;
			this.TradeTime.Interval = 1000;
			this.TradeTime.Enabled = true;
			this.InitNotifier();
		}
		private void InitNotifier()
		{
			Image image = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoPic");
			Image image2 = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoClose");
			if (image != null && image2 != null)
			{
				Bitmap image3 = new Bitmap(image);
				Bitmap image4 = new Bitmap(image2);
				this.Notifier.SetBackgroundBitmap(image3, Color.FromArgb(0, 0, 255));
				this.Notifier.SetCloseBitmap(image4, Color.FromArgb(0, 0, 255), new Point(170, 0));
				this.Notifier.TitleRectangle = new Rectangle(25, 0, 85, 22);
				this.Notifier.ContentRectangle = new Rectangle(30, 30, 150, 52);
				this.Notifier.AutoHide = true;
			}
		}
		private void NotifierMessage(string tradeInfo)
		{
			try
			{
				this.ShowNotifier = new StatusInfoBar.ShowNotifierCallBack(this.ShowNotifierMessage);
				this.HandleCreated();
				base.Invoke(this.ShowNotifier, new object[]
				{
					tradeInfo
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
		}
		private void ShowNotifierMessage(string tradeInfo)
		{
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_tredeInfoHead");
			this.Notifier.Show(@string, tradeInfo, 500, 5000, 500);
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void TradeTime_Tick(object sender, EventArgs e)
		{
			this.operationManager.IdleOnMoudel++;
			this.operationManager.IdleRefreshButton++;
			this.operationManager.SetIdleOnMoudel();
			TimeSpan t = new TimeSpan(0, 0, 1);
			Global.ServerTime += t;
			this.time.Text = Global.ServerTime.ToLongTimeString();
		}
		private void Systimer_Tick(object sender, EventArgs e)
		{
			this.operationManager.querySysTimeOperation.RefreshSysTime(this.Systimer.Interval);
		}
		private void status_DoubleClick(object sender, EventArgs e)
		{
			if (this.connectStatus == 1)
			{
				this.Systimer.Enabled = false;
				if (this.operationManager.SetHQTimerEnable != null)
				{
					this.operationManager.SetHQTimerEnable(false);
				}
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_DisconnectedInfo");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_DisconnectedMessege");
				string string3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_Relogin");
				string string4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_Cancel");
				string string5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_LogOffMessage");
				string string6 = Global.M_ResourceManager.GetString("TradeStr_MainForm_OK");
                bool flag = ToolsLibrary.util.Tools.StrToBool(Global.HTConfig["IsEnableRelogin"].ToString(), false);
				if (flag)
				{
					DialogResult dialogResult = MessageBoxEx.Show(string2, @string, MessageBoxButtons.OKCancel, new string[]
					{
						string3 + "(&O)",
						string4 + "(&C)"
					}, MessageBoxIcon.Asterisk);
					if (dialogResult == DialogResult.OK)
					{
						string empty = string.Empty;
						if (!this.Relogon(ref empty))
						{
							this.FillInfoText(empty, Global.ErrorColor, true);
							return;
						}
						this.Systimer.Enabled = true;
						if (this.operationManager.SetHQTimerEnable != null)
						{
							this.operationManager.SetHQTimerEnable(true);
						}
						if (this.connectStatus != 0)
						{
							this.connectStatus = 0;
							this.SetStatus(this.connectStatus);
						}
						string string7 = Global.M_ResourceManager.GetString("TradeStr_MainForm_ReloginSuccess");
						this.Notifier.Show(string7, empty, 200, 6000, 500);
						this.FillInfoText(string7, Global.RightColor, true);
						return;
					}
				}
				else
				{
					DialogResult dialogResult2 = MessageBoxEx.Show(string5, @string, MessageBoxButtons.OK, new string[]
					{
						string6 + "(&O)",
						string4 + "(&C)"
					}, MessageBoxIcon.Asterisk);
					if (dialogResult2 == DialogResult.OK)
					{
						Global.Logout(null, null);
					}
				}
			}
		}
		private bool Relogon(ref string info)
		{
			bool result = false;
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.UserID = Global.UserID;
			logonRequestVO.Password = Global.Password;
			logonRequestVO.RegisterWord = Global.RegisterWord;
			LogonResponseVO logonResponseVO = Global.TradeLibrary.Logon(logonRequestVO);
			if (logonResponseVO.RetCode == 0L)
			{
				result = true;
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_LastLoginTime");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_LastLoginIP");
				info = string.Concat(new string[]
				{
					@string,
					logonResponseVO.LastTime,
					"\r\n",
					string2,
					logonResponseVO.LastIP
				});
			}
			return result;
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
			this.statusInfo = new StatusStrip();
			this.info = new ToolStripStatusLabel();
			this.user = new ToolStripStatusLabel();
			this.status = new ToolStripStatusLabel();
			this.time = new ToolStripStatusLabel();
			this.Systimer = new System.Windows.Forms.Timer(this.components);
			this.TradeTime = new System.Windows.Forms.Timer(this.components);
			this.statusInfo.SuspendLayout();
			base.SuspendLayout();
			this.statusInfo.Dock = DockStyle.Fill;
			this.statusInfo.Items.AddRange(new ToolStripItem[]
			{
				this.info,
				this.user,
				this.status,
				this.time
			});
			this.statusInfo.Location = new Point(0, 0);
			this.statusInfo.Name = "statusInfo";
			this.statusInfo.Size = new Size(885, 23);
			this.statusInfo.TabIndex = 6;
			this.statusInfo.Text = "statusStrip1";
			this.info.AutoSize = false;
			this.info.ForeColor = SystemColors.ControlText;
			this.info.Margin = new Padding(1);
			this.info.Name = "info";
			this.info.Size = new Size(590, 21);
			this.info.Text = "信息提示";
			this.info.TextAlign = ContentAlignment.MiddleLeft;
			this.user.AutoSize = false;
			this.user.Margin = new Padding(1);
			this.user.Name = "user";
			this.user.Size = new Size(150, 21);
			this.user.Text = "登录用户：";
			this.user.TextAlign = ContentAlignment.MiddleLeft;
			this.status.AutoSize = false;
			this.status.BackColor = Color.Lime;
			this.status.DoubleClickEnabled = true;
			this.status.Margin = new Padding(1);
			this.status.Name = "status";
			this.status.Size = new Size(40, 21);
			this.status.Text = "连接";
			this.status.DoubleClick += new EventHandler(this.status_DoubleClick);
			this.time.AutoSize = false;
			this.time.Margin = new Padding(1);
			this.time.Name = "time";
			this.time.Size = new Size(80, 21);
			this.time.Text = "HH:MM:SS";
			this.Systimer.Tick += new EventHandler(this.Systimer_Tick);
			this.TradeTime.Tick += new EventHandler(this.TradeTime_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScroll = true;
			base.Controls.Add(this.statusInfo);
			base.Name = "StatusInfoBar";
			base.Size = new Size(885, 23);
			base.Load += new EventHandler(this.StatusInfoBar_Load);
			base.SizeChanged += new EventHandler(this.StatusInfoBar_SizeChanged);
			this.statusInfo.ResumeLayout(false);
			this.statusInfo.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
