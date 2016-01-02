namespace FuturesTrade.Gnnt.UI.Modules.Status
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using FuturesTrade.Gnnt.UI.Modules.Notifier;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class StatusInfoBar : UserControl
    {
        private IContainer components;
        private int connectStatus;
        private FillInfoTextCallBack fillInfoText;
        private ToolStripStatusLabel info;
        private TaskbarNotifier Notifier = new TaskbarNotifier();
        private OperationManager operationManager = OperationManager.GetInstance();
        private SetConnectStatusCallBack SetConnectStatus;
        private ShowNotifierCallBack ShowNotifier;
        private ToolStripStatusLabel status;
        private StatusStrip statusInfo;
        private System.Windows.Forms.Timer Systimer;
        private ToolStripStatusLabel time;
        private System.Windows.Forms.Timer TradeTime;
        private ToolStripStatusLabel user;

        public StatusInfoBar()
        {
            this.InitializeComponent();
            this.CreateHandle();
            Global.StatusInfoFill = new Global.StatusInfoFillCallBack(this.FillInfoText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillInfoText(string infoMessage, Color color, bool display)
        {
            try
            {
                this.fillInfoText = new FillInfoTextCallBack(this.InfoText);
                this.HandleCreated();
                base.Invoke(this.fillInfoText, new object[] { infoMessage, color, display });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        public void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void InfoText(string infoMessage, Color color, bool display)
        {
            if (display)
            {
                this.info.ForeColor = color;
                string str = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoMessegePresentation");
                this.info.Text = str + infoMessage;
            }
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
            this.statusInfo.Items.AddRange(new ToolStripItem[] { this.info, this.user, this.status, this.time });
            this.statusInfo.Location = new Point(0, 0);
            this.statusInfo.Name = "statusInfo";
            this.statusInfo.Size = new Size(0x375, 0x17);
            this.statusInfo.TabIndex = 6;
            this.statusInfo.Text = "statusStrip1";
            this.statusInfo.BackColor = Color.FromArgb(235,235,235);
            //this.statusInfo.Visible = false;
            //隐藏
            this.info.AutoSize = false;
            this.info.ForeColor = SystemColors.ControlText;
            this.info.Margin = new Padding(1);
            this.info.Name = "info";
            this.info.Size = new Size(590, 0x15);
            this.info.Text = "信息提示";
            this.info.TextAlign = ContentAlignment.MiddleLeft;
            this.user.AutoSize = false;
            this.user.Margin = new Padding(1);
            this.user.Name = "user";
            this.user.Size = new Size(150, 0x15);
            this.user.Text = "登录用户：";
            this.user.TextAlign = ContentAlignment.MiddleLeft;
            this.status.AutoSize = false;
            this.status.BackColor = Color.Lime;
            this.status.DoubleClickEnabled = true;
            this.status.Margin = new Padding(1);
            this.status.Name = "status";
            this.status.Size = new Size(40, 0x15);
            this.status.Text = "连接";
            this.status.DoubleClick += new EventHandler(this.status_DoubleClick);
            this.time.AutoSize = false;
            this.time.Margin = new Padding(1);
            this.time.Name = "time";
            this.time.Size = new Size(80, 0x15);
            this.time.Text = "HH:MM:SS";
            this.Systimer.Tick += new EventHandler(this.Systimer_Tick);
            this.TradeTime.Tick += new EventHandler(this.TradeTime_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.Controls.Add(this.statusInfo);
            base.Name = "StatusInfoBar";
            base.Size = new Size(0x375, 0x17);
            base.Load += new EventHandler(this.StatusInfoBar_Load);
            base.SizeChanged += new EventHandler(this.StatusInfoBar_SizeChanged);
            this.statusInfo.ResumeLayout(false);
            this.statusInfo.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
            //this.Visible = false;
            /////
        }

        private void InitNotifier()
        {
            Image original = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoPic");
            Image image2 = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoClose");
            if ((original != null) && (image2 != null))
            {
                Bitmap image = new Bitmap(original);
                Bitmap bitmap2 = new Bitmap(image2);
                this.Notifier.SetBackgroundBitmap(image, Color.FromArgb(0, 0, 0xff));
                this.Notifier.SetCloseBitmap(bitmap2, Color.FromArgb(0, 0, 0xff), new Point(170, 0));
                this.Notifier.TitleRectangle = new Rectangle(0x19, 0, 0x55, 0x16);
                this.Notifier.ContentRectangle = new Rectangle(30, 30, 150, 0x34);
                this.Notifier.AutoHide = true;
            }
        }

        private void NotifierMessage(string tradeInfo)
        {
            try
            {
                this.ShowNotifier = new ShowNotifierCallBack(this.ShowNotifierMessage);
                this.HandleCreated();
                base.Invoke(this.ShowNotifier, new object[] { tradeInfo });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.Message);
            }
        }

        private bool Relogon(ref string info)
        {
            bool flag = false;
            LogonRequestVO req = new LogonRequestVO
            {
                UserID = Global.UserID,
                Password = Global.Password,
                RegisterWord = Global.RegisterWord
            };
            LogonResponseVO evo = Global.TradeLibrary.Logon(req);
            if (evo.RetCode == 0L)
            {
                flag = true;
                string str = Global.M_ResourceManager.GetString("TradeStr_MainForm_LastLoginTime");
                string str2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_LastLoginIP");
                info = str + evo.LastTime + "\r\n" + str2 + evo.LastIP;
            }
            return flag;
        }

        private void SetStatus(int connectStatus)
        {
            try
            {
                this.SetConnectStatus = new SetConnectStatusCallBack(this.Status);
                this.HandleCreated();
                base.Invoke(this.SetConnectStatus, new object[] { connectStatus });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void ShowNotifierMessage(string tradeInfo)
        {
            string strTitle = Global.M_ResourceManager.GetString("TradeStr_MainForm_tredeInfoHead");
            this.Notifier.Show(strTitle, tradeInfo, 500, 0x1388, 500);
        }

        private void Status(int _connectStatus)
        {
            string str = Global.M_ResourceManager.GetString("TradeStr_MainForm_connectStatus1");
            string str2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_connectStatus2");
            this.connectStatus = _connectStatus;
            if (this.connectStatus == 0)
            {
                this.status.BackColor = Color.Lime;
                this.status.Text = str2;
                this.operationManager.SetAllQueryFlag(true);
                this.operationManager.orderOperation.SetButtonOrderEnable(true);
        
            }
            else if (this.connectStatus == 1)
            {
                this.status.BackColor = Color.Red;
                this.status.Text = str;
                this.operationManager.SetAllQueryFlag(false);
                this.operationManager.orderOperation.SetButtonOrderEnable(false);
                this.status_DoubleClick(null, null);
            }
            else if (this.connectStatus == 2)
            {
                this.status.BackColor = Color.Red;
                this.status.Text = str;
                this.operationManager.SetAllQueryFlag(false);
                this.operationManager.orderOperation.SetButtonOrderEnable(true);
            }
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
                string caption = Global.M_ResourceManager.GetString("TradeStr_MainForm_DisconnectedInfo");
                string text = Global.M_ResourceManager.GetString("TradeStr_MainForm_DisconnectedMessege");
                string str3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_Relogin");
                string str4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_Cancel");
                string str5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_LogOffMessage");
                string str6 = Global.M_ResourceManager.GetString("TradeStr_MainForm_OK");
                if (Tools.StrToBool(Global.HTConfig["IsEnableRelogin"].ToString(), false))
                {
                    if (MessageBoxEx.Show(text, caption, MessageBoxButtons.OK, new string[] { str3 + "(&O)", str4 + "(&C)" }, MessageBoxIcon.Asterisk) == DialogResult.OK)
                    {
                        string info = string.Empty;
                        if (!this.Relogon(ref info))
                        {
                            this.FillInfoText(info, Global.ErrorColor, true);
                        }
                        else
                        {
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
                            string strTitle = Global.M_ResourceManager.GetString("TradeStr_MainForm_ReloginSuccess");
                            this.Notifier.Show(strTitle, info, 200, 0x1770, 500);
                            this.FillInfoText(strTitle, Global.RightColor, true);
                        }
                    }
                }
                else if (MessageBoxEx.Show(str5, caption, MessageBoxButtons.OK, new string[] { str6 + "(&O)", str4 + "(&C)" }, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    Global.Logout(null, null);
                }
            }
        }

        private void StatusInfoBar_Load(object sender, EventArgs e)
        {
            this.user.Text = this.user.Text + Global.UserID;
            this.operationManager.querySysTimeOperation.SetConnectStatus = new QuerySysTimeOperation.SetConnectStatusCallBack(this.SetStatus);
            this.operationManager.querySysTimeOperation.ShowNotifier = new QuerySysTimeOperation.ShowNotifierCallBack(this.NotifierMessage);
            this.Systimer.Enabled = true;
            this.TradeTime.Interval = 0x3e8;
            this.TradeTime.Enabled = true;
            this.InitNotifier();
        }

        private void StatusInfoBar_SizeChanged(object sender, EventArgs e)
        {
            this.info.Width = (((base.Width - this.user.Width) - this.status.Width) - this.time.Width) - 20;
        }

        private void Systimer_Tick(object sender, EventArgs e)
        {
            this.operationManager.querySysTimeOperation.RefreshSysTime(this.Systimer.Interval);
        }

        private void TradeTime_Tick(object sender, EventArgs e)
        {
            this.operationManager.IdleOnMoudel++;
            this.operationManager.IdleRefreshButton++;
            this.operationManager.SetIdleOnMoudel();
            TimeSpan span = new TimeSpan(0, 0, 1);
            Global.ServerTime += span;
            this.time.Text = Global.ServerTime.ToLongTimeString();
        }

        private delegate void FillInfoTextCallBack(string infoMessage, Color color, bool display);

        private delegate void SetConnectStatusCallBack(int connectStatus);

        private delegate void ShowNotifierCallBack(string tradeInfo);
    }
}
