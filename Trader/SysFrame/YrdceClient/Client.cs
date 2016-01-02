using DIYForm;
using PluginInterface;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation;
using YrdceClient.Yrdce.Common.Operation.MainOperation;
using YrdceClient.Yrdce.Common.Operation.Manager;
using YrdceClient.Yrdce.UI.Forms.ContainerManager;
using YrdceClient.Yrdce.UI.Forms.UserControls;
using YrdceClient.UI.Forms.PromptForms;     
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using System.Collections.Generic;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using YrdceClient.UI.Forms;
using System.IO;
using System.Text;
using System.Net;
//using FuturesTrade.Gnnt.UI.Modules.Status;
using CefSharp.WinForms;
using CefSharp;


namespace YrdceClient
{
    /// <summary>
    /// 主界面
    /// </summary>
	public class Client : MyForm
	{
       



        
       
		private IContainer components;
		private System.Windows.Forms.Timer zcjsTimer;
		private ToolTip toolTipText;
		private Panel panelMain;//主窗体主panel
		public OperationManager operationManager = OperationManager.GetInstance();
        private Hashtable htConfig;
        public MyTabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;

        //private StatusInfoBar statusInfo = new StatusInfoBar();

        private List<VistaButton> menuBtnList = new List<VistaButton>();
        private CefSharp.WinForms.WebView webBrowser1;
        //private Panel panelMenuParent;

        private Panel panelTop;
        private Panel panelTopMid;
        private Panel panelTopTime;
        private Splitter spTime;

        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timerLinkLabel;
        //private Panel panelBottom;
        //private VistaButton btnUser;
    
        //private VistaButton btnlogoff;
        public Button btnLoginAndLogoff;
        public Button btnChangePWD;
        private WebBrowser tempWeb;
   
        private Button btnPanelDown;
        private Button btnPanelUp;
        private Button btnGetIP;
        private PictureBox pictureLogo;
        private Label lblWeek;
        private PictureBox picTelephone;
        private PictureBox picNotice;
        public Panel panelTopInfo;
        private LinkLabel linkTest;
        private System.Windows.Forms.Timer timer1;
        private Button TopToLeft;
        public Button TopToRight;

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

            this.components = new System.ComponentModel.Container();
            this.zcjsTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTipText = new System.Windows.Forms.ToolTip(this.components);
            //this.panelMenuParent = new System.Windows.Forms.Panel();
            this.tabControl1 = new YrdceClient.MyTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new TabPage();

            this.webBrowser1 = new CefSharp.WinForms.WebView();
           
            this.tempWeb = new WebBrowser();
          
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelTopTime = new System.Windows.Forms.Panel();
            //this.panelBottom = new System.Windows.Forms.Panel();
            this.panelTopInfo = new Panel();
            this.btnPanelDown = new Button();
            this.btnPanelUp = new Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer();
            this.timerLinkLabel = new System.Windows.Forms.Timer(this.components);
            //this.btnlogoff = new VistaButton();
            this.btnLoginAndLogoff = new Button();
            this.btnChangePWD = new Button();
            //this.btnUser = new VistaButton();
            this.pictureLogo = new PictureBox();
            //this.panelUserMenu = new Panel();
            this.panelTopMid = new Panel();
            this.btnGetIP = new Button();
            this.lblWeek = new Label();
            this.picTelephone = new PictureBox();
            this.picNotice = new PictureBox();
            this.linkTest = new LinkLabel();
            this.TopToLeft = new Button();
            this.TopToRight = new Button();

            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
        
            this.panelTop.SuspendLayout();
            
            this.SuspendLayout();
            // 
            // zcjsTimer
            // 
            this.zcjsTimer.Enabled = true;
            // 
            // panelMenuParent
            // 
            //this.panelMenuParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            //this.panelMenuParent.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            //this.panelMenuParent.Location = new System.Drawing.Point(2, 62);
            //this.panelMenuParent.Name = "panelMenuParent";
            //this.panelMenuParent.Size = new System.Drawing.Size(122, 646);
            //this.panelMenuParent.TabIndex = 14;

            //spliLeft = new Splitter();
            //spliLeft.Width = 1;
            //spliLeft.Dock = DockStyle.Right;
            //panelMenuParent.Controls.Add(spliLeft);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
          
            this.tabControl1.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(8)))), ((int)(((byte)(45)))));
            this.tabControl1.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.tabControl1.ItemSize = new System.Drawing.Size(85, 25);
            this.tabControl1.Location = new System.Drawing.Point(2, 60);
            this.tabControl1.Multiline = true;
            this.tabControl1.MyBackColor = System.Drawing.Color.WhiteSmoke;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(912, 675);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelMain);
            this.tabPage1.Location = new System.Drawing.Point(29, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 647);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(3, 3);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(761, 641);
            this.panelMain.TabIndex = 11;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowser1);
            this.tabPage2.Location = new System.Drawing.Point(29, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 647);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(tempWeb);
            this.tabPage3.Location = new System.Drawing.Point(29, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(767, 647);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(761, 641);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.MenuHandler = new MYMenuHandler() ;
            //this.webBrowser1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.webBrowser1_MouseDown);
            //this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            //this.webBrowser1.ScriptErrorsSuppressed = true;
            
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.panelTopTime);
            this.panelTop.Controls.Add(this.panelTopMid);
            //this.panelTop.Controls.Add(this.panelUser);
            this.panelTop.Location = new System.Drawing.Point(2, 27);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(912, 40);
            this.panelTop.TabIndex = 15;
            //this.panelTop.BackgroundImage = TimeImage.PanelTop;
           
           
            // 
            // panelTopTime
            // 
            this.panelTopTime.BackColor = System.Drawing.Color.White;
            this.panelTopTime.Location = new System.Drawing.Point(0, 0);
            this.panelTopTime.Name = "panelTopTime";
            this.panelTopTime.Size = new System.Drawing.Size(122, 40);
            this.panelTopTime.TabIndex = 0;
            this.panelTopTime.BackgroundImage = TimeImage.PanelTop;
            this.panelTopTime.BackgroundImageLayout = ImageLayout.Stretch;
            this.panelTopTime.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTopTime_Paint);
            this.panelTopTime.Controls.Add(pictureLogo);
            spTime = new Splitter();
            spTime.Width = 1;
            spTime.Dock = DockStyle.Right;
            spTime.BackColor = System.Drawing.Color.FromArgb(206, 208, 213);
            panelTopTime.Controls.Add(spTime);
            panelTopTime.Controls.Add(lblWeek);
            // 
            // pictureLogo
            // 
            this.pictureLogo.Location = new System.Drawing.Point(5, 5);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.BackColor = Color.Transparent;
            this.pictureLogo.Size = new System.Drawing.Size(40, this.panelTopTime.Height-10);
            this.pictureLogo.TabIndex = 11;
            this.pictureLogo.TabStop = false;
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            //this.pictureLogo.BackColor = Color.Red;
            this.pictureLogo.Image = TimeImage.Toplogo1;
            // 
            // picTelephone
            // 
            //this.panelMenuParent.Controls.Add(picTelephone);
            this.picTelephone.Location = new System.Drawing.Point(5, 5);
            this.picTelephone.Name = "picTelephone";
            //this.picTelephone.Size = new System.Drawing.Size(this.panelMenuParent.Width,this.panelMenuParent.Height/5 );
            this.picTelephone.TabIndex = 11;
            this.picTelephone.TabStop = false;
            this.picTelephone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTelephone.Dock = DockStyle.Bottom;
            //this.pictureLogo.BackColor = Color.Red;
            this.picTelephone.Image = TimeImage.telephone;
            //
            //lblWeek
            //
        
            this.lblWeek.Location = new System.Drawing.Point(54, 4);
            this.lblWeek.Size = new Size(55,14);
            this.lblWeek.Text = DateTime.Now.ToString();
            this.lblWeek.Font = new Font("楷体",11);
            this.lblWeek.BackColor = Color.Transparent;
            
           //
            // panelTopMid
            //
            this.panelTopMid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left))));
            //this.panelTopMid.BackColor = Color.FromArgb(239, 227, 199);
            this.panelTopMid.Location = new System.Drawing.Point(this.panelTopTime.Right, 0);
            this.panelTopMid.BackgroundImage = TimeImage.PanelTop;
            this.panelTopMid.BackgroundImageLayout = ImageLayout.Stretch;
            //this.panelUserMenu.Dock = DockStyle.Right;
            this.panelTopMid.Name = "panelTopMid";

            this.panelTopMid.Size = new System.Drawing.Size(this.panelTop.Width - this.panelTopTime.Width-7, this.panelTop.Height);
            this.panelTopMid.TabIndex = 0;

            this.panelTopMid.Controls.Add(this.btnPanelUp);
            //this.panelTopMid.Controls.Add(this.picNotice);
            this.panelTopMid.Controls.Add(this.panelTopInfo); 
            this.panelTopMid.Controls.Add(this.btnLoginAndLogoff);
            this.panelTopMid.Controls.Add(this.btnChangePWD);
            this.panelTopMid.Controls.Add(this.TopToLeft);
            this.panelTopMid.Controls.Add(this.TopToRight);


            //
            //TopToLeft
            //
            TopToLeft.Location = new Point(0, 0);
            TopToLeft.Size = new System.Drawing.Size(10,this.panelTopMid.Height);
            //TopToLeft.BackColor = System.Drawing.Color.FromArgb(229, 161, 15);
            //TopToLeft.BackColor = Color.FromArgb(239, 227, 199);
            //TopToLeft.ForeColor = Color.FromArgb(199, 90, 27);
            this.TopToLeft.FlatStyle = FlatStyle.Flat;
            this.TopToLeft.FlatAppearance.BorderSize = 0;
            //this.TopToLeft.Text = "←";
            this.TopToLeft.Click += new System.EventHandler(this.TopToLeft_Click);
            this.TopToLeft.BackgroundImage = TimeImage.topToLeft;
            //
            //TopToRight
            //
            this.TopToRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Right))));
            
            TopToRight.Size = new System.Drawing.Size(10, this.panelTopMid.Height);
            //TopToRight.BackColor = System.Drawing.Color.FromArgb(229, 161, 15);
            //TopToRight.BackColor = Color.FromArgb(239, 227, 199);
            //TopToRight.ForeColor = Color.FromArgb(199, 90, 27);
            this.TopToRight.FlatStyle = FlatStyle.Flat;
            this.TopToRight.FlatAppearance.BorderSize = 0;
            //this.TopToRight.Text = "→";
            this.TopToRight.Click += new System.EventHandler(this.TopToRight_Click);
            this.TopToRight.BackgroundImage = TimeImage.topToRight;
            this.TopToRight.Visible = false;
            // 
            // btnPanelDown
            // 
            this.btnPanelDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top )
            )));
            this.btnPanelDown.Location = new Point(this.Width / 2 + 350, 28);
            this.btnPanelDown.Height =8;
            this.btnPanelDown.Width = 30;
            this.btnPanelDown.BackgroundImage = TimeImage.btnDown;
            this.btnPanelDown.BackgroundImageLayout = ImageLayout.Stretch;
            btnPanelDown.FlatAppearance.MouseOverBackColor = Color.Transparent ;//鼠标经过 
            btnPanelDown.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下 
            btnPanelDown.FlatStyle = FlatStyle.Flat;//样式 
            btnPanelDown.ForeColor = Color.Transparent;//前景 
            btnPanelDown.BackColor = Color.Transparent;//去背景 
            btnPanelDown.FlatAppearance.BorderSize = 0;//去边线 
            this.btnPanelDown.Visible = false;

            this.btnPanelDown.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnPanelUp
            // 
            this.btnPanelUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top)
            )));
            this.btnPanelUp.Location = new Point(this.Width / 2 +226, 31);
            this.btnPanelUp.Height = 8;
            this.btnPanelUp.Width = 30;

            this.btnPanelUp.BackgroundImage = TimeImage.btnUp;
            this.btnPanelUp.BackgroundImageLayout = ImageLayout.Stretch;
            btnPanelUp.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过 
            btnPanelUp.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下 
            btnPanelUp.FlatStyle = FlatStyle.Flat;//样式 
            btnPanelUp.ForeColor = Color.Transparent;//前景 
            btnPanelUp.BackColor = Color.Transparent;//去背景 
            btnPanelUp.FlatAppearance.BorderSize = 0;//去边线 
            this.btnPanelUp.Click += new System.EventHandler(this.btnUp_Click);
           
            // 
            // btnLoginAndLogoff
            // 
            this.btnLoginAndLogoff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right )
            )));
            this.btnLoginAndLogoff.Location = new Point(710, 8);
            //this.btnLoginAndLogoff.Dock = DockStyle.Right;
            this.btnLoginAndLogoff.BackColor = Color.Transparent;
            this.btnLoginAndLogoff.FlatStyle = FlatStyle.Flat;
            this.btnLoginAndLogoff.FlatAppearance.BorderSize = 0;
            this.btnLoginAndLogoff.Height = this.panelTop.Height - 15;
            this.btnLoginAndLogoff.Width = 60;
            this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogin;
            this.btnLoginAndLogoff.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnLoginAndLogoff.ForeColor = Color.Transparent;
            this.btnLoginAndLogoff.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过 
            this.btnLoginAndLogoff.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下 
            this.btnLoginAndLogoff.Font = new Font("楷体", 15);
            this.btnLoginAndLogoff.TextAlign = ContentAlignment.MiddleCenter;
            this.btnLoginAndLogoff.Click +=new System.EventHandler(btnLoginAndLogoff_Click);
            // 
            // btnChangePWD
            // 
            this.btnChangePWD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)
            )));
            this.btnChangePWD.Location = new Point(620, 8);
            //this.btnLoginAndLogoff.Dock = DockStyle.Right;
            this.btnChangePWD.BackColor = Color.Transparent;
            this.btnChangePWD.FlatStyle = FlatStyle.Flat;
            this.btnChangePWD.FlatAppearance.BorderSize = 0;
            this.btnChangePWD.Height = this.panelTop.Height - 15;
            this.btnChangePWD.Width = 80;
            //this.btnLoginAndLogoff.Dock = DockStyle.Top;
            this.btnLoginAndLogoff.Name = "ChangePW";
            this.btnChangePWD.BackgroundImage = TimeImage.picpwd2;
            this.btnChangePWD.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnChangePWD.ForeColor = Color.Transparent;
            this.btnChangePWD.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过 
            this.btnChangePWD.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下 
            this.btnChangePWD.Font = new Font("楷体", 15);
            this.btnChangePWD.TextAlign = ContentAlignment.MiddleCenter;
            this.btnChangePWD.Click += new System.EventHandler(btnChangePWD_Click);
            
            
            // 
            // panelTopInfo
            // 
            this.panelTopInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Right))));
            //this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(208)))), ((int)(((byte)(213)))));
            //this.panelTopInfo.BackColor = Color.Red;
            //this.panelTopInfo.Location = new System.Drawing.Point(710, 0);
            this.panelTopInfo.Name = "panelTopInfo";
            this.panelTopInfo.Size = new System.Drawing.Size(300, this.panelTopMid.Height);
            this.panelTopInfo.Controls.Add(linkTest);
            this.panelTopInfo.Controls.Add(picNotice);
            this.panelTopInfo.BackgroundImage = TimeImage.PanelTop;
            this.BackgroundImageLayout = ImageLayout.Stretch;
;

            // 
            // linkTest
            // 
            this.linkTest.AutoSize = true;
            this.linkTest.Location = new System.Drawing.Point(this.picNotice.Right, 10);
            this.linkTest.Name = "linkTest";
            this.linkTest.Size = new System.Drawing.Size(87, 15);
            this.linkTest.BackColor = Color.Transparent;
            this.linkTest.TabIndex = 13;
            this.linkTest.TabStop = true;
            this.linkTest.Text = "长三角商品所欢迎您！";
            this.linkTest.Font = new Font("微软雅黑",12);
            this.linkTest.LinkColor = Color.Black;
            this.linkTest.LinkBehavior = LinkBehavior.NeverUnderline;
            this.linkTest.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTest_LinkClicked);
            // 
            // 
            // picNotice
            // 
            this.picNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Right)));
            this.picNotice.Location = new System.Drawing.Point(0,10);
            this.picNotice.Name = "picNotice";
            this.picNotice.Size = new System.Drawing.Size(this.panelTopInfo.Height-18, this.panelTopInfo.Height-18);
            this.picNotice.TabIndex = 11;
            this.picNotice.TabStop = false;
            //this.picNotice.Dock = DockStyle.Left;
            this.picNotice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picNotice.BackColor = Color.Transparent;
            //this.pictureLogo.BackColor = Color.Red;
            this.picNotice.Image = TimeImage.Notice;
            this.picNotice.Click += new System.EventHandler(this.btnGetIP_Click);
            this.picNotice.BringToFront();
            // 
            // btnGetIP
            // 
            this.btnGetIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)
            )));
            this.btnGetIP.Location = new Point(854, 0);
            //this.btnGetIP.Height = this.panelBottom.Height;
            this.btnGetIP.Width = 50;
            //this.btnGetIP.Dock = DockStyle.Left;
            this.btnGetIP.Text = "状态";
            
            this.btnGetIP.ForeColor = Color.White;
            this.btnGetIP.Font = new Font("楷体", 12);
            this.btnGetIP.TextAlign = ContentAlignment.MiddleCenter;
            this.btnGetIP.BackColor = Color.Red;
            
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.timer1.Interval = 1000;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer3_Tick);
            this.timer1.Interval = 100;
            // 
            // timerLinkLabel
            // 
            this.timerLinkLabel.Enabled = true;
            this.timerLinkLabel.Tick += new System.EventHandler(this.timer2_Tick);
            this.timerLinkLabel.Interval = 100;
            // 
            // Client
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(915, 736);
            this.ControlBox = false;
            ////this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.btnPanelDown);
            
            //this.Controls.Add(this.panelMenuParent);
            this.Controls.Add(this.tabControl1);
            //this.Controls.Add(this.panelUserMenu);
            this.KeyPreview = true;
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "长三角统一交易软件";
            this.TextColor =   Brushes.White;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
            this.Load += new System.EventHandler(this.Client_Load);
            this.Shown += new System.EventHandler(this.Client_Shown);
            this.SizeChanged += new System.EventHandler(this.Client_SizeChanged);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.panelTop, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.Opacity = 0;
       
            for (int i = 0; i < 10; i++)
            {
                image[i] = (Image)TimeImage.ResourceManager.GetObject(i.ToString());
            }

            //添加按钮
            tempButtonQuotation = CreateMenu(120, new Point(10, 0), this.panelTopMid.Height, this.panelTopMid, xmlReader.GetTabButtonText(0));
            tempButton = CreateMenu(120, new Point(this.tempButtonQuotation.Right + 5, 0), this.panelTopMid.Height, this.panelTopMid, xmlReader.GetTabButtonText(4));       
            tempButton2 = CreateMenu(120, new Point(this.tempButton.Right + 5, 0), this.panelTopMid.Height, this.panelTopMid, xmlReader.GetTabButtonText(2));
            tempButton3 = CreateMenu(120, new Point(this.tempButton2.Right + 5, 0), this.panelTopMid.Height, this.panelTopMid, xmlReader.GetTabButtonText(1));
            tempButton4 = CreateMenu(120, new Point(this.tempButton3.Right + 5, 0), this.panelTopMid.Height, this.panelTopMid, xmlReader.GetTabButtonText(3));
		}

      
        private Image[] image = new Bitmap[10];
        public VistaButton tempButtonQuotation;
        public VistaButton tempButton;
        public VistaButton tempButton2;
        public VistaButton tempButton3;
        public VistaButton tempButton4;

        public string Week()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            return week;
        }


        
		public Client(string[] args)
		{
			this.InitializeComponent();
			this.ControlLoad(args);
            tempMin = DateTime.Now.Minute % 10;
		}
        public   ContainerManage containerManage;
		private void ControlLoad(string[] args)
		{
			try
			{
                this.operationManager.YrdceClientOperation.AnalyticalParamenter(args);
                this.operationManager.YrdceClientOperation.SetFormInfo(this);
                containerManage = new ContainerManage(panelMain);
                
                containerManage.ControlLayOut();
               
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void Client_Shown(object sender, EventArgs e)
		{
			if (!MarketInfo.isLoadedForm)
			{
				MarketInfo marketInfo = new MarketInfo();
                marketInfo.Show();
			}
		}

        /// <summary>
        /// 生成左侧菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private VistaButton CreateMenu(int width, Point location, int btnHeight, Control control, string tabButtonTest)
        {

            VistaButton tabButton = new VistaButton();
            Panel contentPanel = new Panel();
            //MenuControls menuControls = new MenuControls();
            tabButton.ButtonColor = Color.Transparent;
            tabButton.BaseColor = Color.White;
            tabButton.Location = location;
            tabButton.Width = width;
            tabButton.CornerRadius = 0;
            tabButton.Height = btnHeight;

            tabButton.Click += new EventHandler(tabbutton_Click);



            tabButton.ButtonText = tabButtonTest;
            tabButton.Tag = contentPanel;
            tabButton.BackColor = System.Drawing.Color.FromArgb(229, 161, 15);
            tabButton.FlatStyle = FlatStyle.Flat;
            tabButton.FlatAppearance.BorderSize = 0;
            tabButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 227, 199);
            tabButton.ForeColor = Color.FromArgb(199, 90, 27);
            tabButton.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(161)))), ((int)(((byte)(15)))));
            tabButton.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

            tabButton.Font = new Font("微软雅黑", 12);
            //contentPanel.BackColor = Color.White;
            //contentPanel.Top = tabButton.Bottom;
            //contentPanel.Width = width;
            //contentPanel.Height = 0;
            //contentPanel.Left = tabButton.Left;
            this.menuBtnList.Add(tabButton);
            //control.Controls.Add(contentPanel);
            control.Controls.Add(tabButton);



            return tabButton;
        }
        
        
        
       



        ParameterizedThreadStart ts;
        Thread th;


        /// <summary>
        /// 左上角时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private int tempMin;
        private void panelTopTime_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int hh = DateTime.Now.Hour;                       //取得小时数字  
            int hh1 = hh / 10;
            int hh2 = hh % 10;
         
            int mm = DateTime.Now.Minute;                      //取得分钟数字  
            int mm1 = mm / 10;
            int mm2 = mm % 10;
            g.DrawImage(image[hh1], 60, 24, 6, 10);
            g.DrawImage(image[hh2], 70, 24, 6, 10);
            g.DrawImage(TimeImage.colon, 78, 24, 6, 10);
            g.DrawImage(image[mm1], 86, 24, 6, 10);
            g.DrawImage(image[mm2], 96, 24, 6, 10);

        }


        #region 定时出现
        System.Timers.Timer thead = new System.Timers.Timer(3000); //设置时间间隔为2秒


        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.operationManager.isLogin == false)
            {
                this.Opacity = 1;
            }
            
        }
        #endregion 定时出现
    
        string timeNow;
        
		private void Client_Load(object sender, EventArgs e)
		{

                this.tempButtonQuotation.Location = new Point(10, 0);
                this.tempButton.Visible = true;
                this.tempButton2.Visible =true;
                this.tempButton3.Visible = true;

            SetButton(this.btnLoginAndLogoff);
            SetButton(this.btnChangePWD);
            if (this.operationManager.isLogin  == true)
            {

                this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogoff;
                this.btnChangePWD.Visible = true;
                this.panelTopInfo.Location = new System.Drawing.Point(this.btnChangePWD.Left - this.panelTopInfo.Width - 5, 0);
                TopToRight.Location = new Point(this.btnChangePWD.Left - this.panelTopInfo.Width - 15, 0);
           
            }
            else
            {
                this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogin;
                this.btnChangePWD.Visible = false;
                this.panelTopInfo.Location = new System.Drawing.Point(this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 5, 0);
                TopToRight.Location = new Point(this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 15, 0);
            }
            this.lblWeek.Text = Week();
            timeNow =  DateTime.Now.ToString();
            thead.Enabled = true; //是否触发Elapsed事件
            thead.Start();
            thead.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            thead.AutoReset = false; 
			this.htConfig = Global.htConfig;
            //tab隐藏标题
            //this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            this.tabControl1.SizeMode = TabSizeMode.Fixed;  
            this.tabControl1.ItemSize   =   new   Size(0,   1);
            this.btnLoginAndLogoff.BringToFront();
            this.btnPanelDown.BringToFront();
            this.btnChangePWD.BringToFront();         
            if (this.SetControlText())
			{
				OperationManager.GetInstance().updateOperation.StartCheckUpdate(this);
			}
			else
			{
				string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_SelectResource");
				string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_NoResource");
				MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
			this.operationManager.stripButtonOperation.createLoginForm = new StripButtonOperation.CreateLoginForm(this.CreateLoginForm);
            
            
		}



        #region 不动
        private void CreateLoginForm(IPlugin plugin)
		{
			ProgramOperation.CreateLogin(plugin, true);
            
		}
        string text;
        private bool SetControlText()
		{
			bool result;
			try
			{
				ScaleForm.ScaleForms(this);
				base.Icon = Global.Modules.Plugins.SystemIcon;
				if (this.htConfig != null)
				{
					string arg = string.Empty;
					text = string.Empty;
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
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
				result = false;
			}
			return result;
		}
		private void Client_FormClosing(object sender, FormClosingEventArgs e)
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
				Global.Modules.Plugins.ClosePlugins();
			}
		}
		private void Client_FormClosed(object sender, FormClosedEventArgs e)
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
		private void Client_SizeChanged(object sender, EventArgs e)
		{
            if (tempButtonQuotation.Left > panelTopInfo.Left)
            {
                tempButtonQuotation.Visible = false;
                
            }
            else 
            {
                tempButtonQuotation.Visible = true;
            }
            if (tempButton.Left > panelTopInfo.Left)
            {
               
                tempButton.Visible = false;
                
            }
            else
            {
                tempButton.Visible = true;
            }
            if (tempButton2.Left > panelTopInfo.Left)
            {
                tempButton2.Visible = false;
                
            }
            else
            {
                tempButton2.Visible = true;
            }
            if (tempButton3.Left > panelTopInfo.Left)
            {
                tempButton3.Visible = false;

            }
            else
            {
                tempButton3.Visible = true;
            }
            if (tempButton4.Left > this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 5)
            {
         
               tempButton4.Visible = false;
            }
            else
            {
    
                tempButton4.Visible = true;
                
            }
            if (this.panelTopInfo.Left>this.tempButton4.Width*4+30)
            {
                this.tempButtonQuotation.Location = new Point(10, 0);
                this.tempButton.Location = new Point(this.tempButtonQuotation.Right + 5, 0);
                this.tempButton2.Location = new Point(this.tempButton.Right + 5, 0);
                this.tempButton3.Location = new Point(this.tempButton2.Right + 5, 0);
                this.tempButton4.Location = new Point(this.tempButton3.Right + 5, 0);
               
            }
            if (this.tempButtonQuotation.Location.X < 0)
            {
                this.TopToRight.Visible = true;
            }
            else
            {
                this.TopToRight.Visible = false;
            }
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
			if (this.operationManager.isLogin && Global.Modules.Plugins.SysLogonInfo.TraderID.Equals(copyDataStruct.str))
			{
				m.Result = (IntPtr)1;
              
				return;
			}
			m.Result = (IntPtr)0;
		}
        #endregion 不动









        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    if (lastBtn !=null)
        //    {
        //        lastBtn.BackColor = Color.White;
        //    }
        //    btn.BackColor = Color.FromArgb(206, 208, 213);
        //    lastBtn = btn;
        //    this.tabControl1.SelectTab(0);
        //    this.panelTopMid.Controls.Clear();
        //    this.operationManager.stripButtonOperation.button_Click("MEBS_Trade");         
        //}


        private void btnUp_Click(object sender, EventArgs e)
        {
            if (this.panelTop.Visible == true)
            {
                this.panelTop.Visible = false;   
                this.btnPanelDown.Visible = true;
                this.btnPanelUp.Visible = false;
                this.tabControl1.Location = new System.Drawing.Point(2, 30);
                this.tabControl1.Height += 30;
            
            }
            else
            {
                this.panelTop.Visible = true;
                this.btnPanelDown.Visible = false;
                this.btnPanelUp.Visible = true;
                this.tabControl1.Location = new System.Drawing.Point(2, 60);
                this.tabControl1.Height -= 30;
              
            }
            
        }
       private string webPageType;


       private VistaButton lastBtn = null;
        private void tabbutton_Click(object sender, EventArgs e)
        {      
            VistaButton btn = (VistaButton)sender;
            webPageType = btn.ButtonText;
            if (this.operationManager.isLogin ==true)
            {
                btn.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(161)))), ((int)(((byte)(15)))));
                btn.Enabled = false;
                //this.panelTopMid.Controls.Clear();

                if (this.lastBtn != null)
                {
                    lastBtn.Enabled = true;
                    this.lastBtn.GlowColor = Color.White;
                }
                this.lastBtn = btn;
            }  
            if (btn.ButtonText == "发售交易")
                {
                    this.tabControl1.SelectTab(0);
                    this.operationManager.stripButtonOperation.button_Click("MEBS_Trade");
                    return;
                }
            string tempUrl;
            if (this.operationManager.isLogin == true)
            {
               
                if (this.tempWeb.Url == null)
                {                 
                    #region 访问第一层页面
                    this.operationManager.stripButtonOperation.button_Click("MEBS_Consumer");
                    tempUrl = this.operationManager.stripButtonOperation.WebUrl;
                    this.tempWeb.Url = new Uri(tempUrl);
                    this.tempWeb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
                    #endregion 访问第一层页面
                }
                else 
                {
                   
                    showSecondUrl();
                }
            }
            else 
            {
              DialogResult dr =  MessageBox.Show("您尚未登录，请登录后操作" ,"系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    this.CreateLoginForm(null);
                    if (this.operationManager.isLogin == true)
                    {
                        this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogoff;
                        //this.panelTopInfo.Location = new System.Drawing.Point(930, 0);
                        this.panelTopInfo.Location = new System.Drawing.Point(this.btnChangePWD.Left - this.panelTopInfo.Width - 5, 0);
                        this.TopToRight.Location = new Point(this.btnChangePWD.Left - this.panelTopInfo.Width - 15, 0);
                        this.btnChangePWD.Visible = true;
                        this.tabControl1.SelectTab(0);

                    }
                }
            }
        }
        
        private void wb_DocumentCompleted(object sender, EventArgs e)
        {
            showSecondUrl();
        }

        private void showSecondUrl()
        {
            
            string fristUrl = "";
            if (webPageType == "资金管理")
            {
                fristUrl = "http://61.147.181.4:10002/finance_front/front/frame/mainframe.jsp?";
            }
            else if (webPageType == "发售业务")
            {
                fristUrl = "http://61.147.181.4:10002/ipo_front/front/frame/mainframe.jsp?";
            }
            else if (webPageType == "仓单业务")
            {
                fristUrl = "http://61.147.181.4:10002/bill_front/front/frame/mainframe.jsp?";
            }
            else if (webPageType == "银行接口")
            {
                fristUrl = "http://61.147.181.4:10002/bank_front/front/frame/mainframe.jsp?";
            }


            string[] myurl = Regex.Split(this.operationManager.stripButtonOperation.MyUrl[1], "pc");
            string x = fristUrl + myurl[0] + "web" + myurl[1];
            //if (this.webBrowser1.Url.ToString() == x)
            //    return;
            this.webBrowser1.Address= x;
            this.tabControl1.SelectTab(1);
            this.webBrowser1.Load(x);
        }

        private void btnChangePWD_Click(object sender ,EventArgs e)
        {
            this.operationManager.stripButtonOperation.button_Click("ChangePW");
        }

        //移动大按钮
        private void TopToLeft_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.tempButton4.Location.X) > 20 && this.panelTopInfo.Left<this.tempButton4.Right)
            {
                this.TopToRight.Visible = true;
                this.tempButton.Location = new Point(this.tempButton.Location.X - this.tempButtonQuotation.Width - 5, 0);
                this.tempButton2.Location = new Point(this.tempButton2.Location.X - this.tempButtonQuotation.Width - 5, 0);
                this.tempButton3.Location = new Point(this.tempButton3.Location.X - this.tempButtonQuotation.Width - 5, 0);
                this.tempButton4.Location = new Point(this.tempButton4.Location.X - this.tempButtonQuotation.Width - 5, 0);
                this.tempButtonQuotation.Location = new Point(this.tempButtonQuotation.Location.X - this.tempButtonQuotation.Width - 5, 0);
                if (this.panelTopInfo.Left > this.tempButton4.Left)
                {
                    tempButton4.Visible = true;
                }
                if (this.panelTopInfo.Left>this.tempButton3.Left)
                {
                    tempButton3.Visible = true;
                }
                if (this.panelTopInfo.Left > this.tempButton2.Left)
                {
                    tempButton2.Visible = true;
                }
                if (this.panelTopInfo.Left > this.tempButton.Left)
                {
                    tempButton.Visible = true;
                }
                if (this.panelTopInfo.Left > this.tempButtonQuotation.Left)
                {
                    tempButtonQuotation.Visible = true;
                }
            }
            
        }
        private void TopToRight_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.tempButtonQuotation.Location.X) < 0)
            {
                this.tempButton.Location = new Point(this.tempButton.Location.X + this.tempButtonQuotation.Width + 5, 0);
                this.tempButton2.Location = new Point(this.tempButton2.Location.X + this.tempButtonQuotation.Width + 5, 0);
                this.tempButton3.Location = new Point(this.tempButton3.Location.X + this.tempButtonQuotation.Width + 5, 0);
                this.tempButton4.Location = new Point(this.tempButton4.Location.X + this.tempButtonQuotation.Width + 5, 0);
                this.tempButtonQuotation.Location = new Point(this.tempButtonQuotation.Location.X + this.tempButtonQuotation.Width + 5, 0);
            }
            
            if (tempButtonQuotation.Left > panelTopInfo.Left)
            {
                tempButtonQuotation.Visible = false;

            }
            if (tempButton.Left > panelTopInfo.Left)
            {
                tempButton.Visible = false;

            }
            if (tempButton2.Left > panelTopInfo.Left)
            {
                tempButton2.Visible = false;

            }
            if (tempButton3.Left > panelTopInfo.Left)
            {
                tempButton3.Visible = false;

            }
            if (tempButton4.Left > this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 5)
            {

                tempButton4.Visible = false;
            }
            if (Convert.ToInt32(this.tempButtonQuotation.Location.X) >= 0)
                this.TopToRight.Visible = false;
        }


        private void btnLoginAndLogoff_Click(object sender, EventArgs e)
        {
            
            //this.tabControl1.SelectTab(1);
            if (this.operationManager.isLogin == false)
            {
                this.CreateLoginForm(null);
                if (this.operationManager.isLogin == true)
                {
                    this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogoff;
                    //this.panelTopInfo.Location = new System.Drawing.Point(930, 0);
                    this.panelTopInfo.Location = new System.Drawing.Point(this.btnChangePWD.Left-this.panelTopInfo.Width-5, 0);
                    TopToRight.Location = new Point(this.btnChangePWD.Left - this.panelTopInfo.Width - 15, 0);
                    this.btnChangePWD.Visible = true;
                    this.tabControl1.SelectTab(0);
                }
            }
            else if (this.operationManager.isLogin == true)
            {
                this.operationManager.stripButtonOperation.button_Click("logout");
                if (this.operationManager.isLogin == false)
                {
                    this.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogin;
                    //this.panelTopInfo.Location = new System.Drawing.Point(1020, 0);
                    this.panelTopInfo.Location = new System.Drawing.Point(this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 5, 0);
                    TopToRight.Location = new Point(this.btnLoginAndLogoff.Left - this.panelTopInfo.Width - 15, 0);
                    this.btnChangePWD.Visible = false;
                }
            }
        
        }
        XmlReader xmlReader = new XmlReader();
        private void btnUser_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectTab(1);
        }

        private void btnUserTest_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            MessageBox.Show(btn.Name);
            //MessageBox.Show(xmlReader.GetName(1));//
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

            //this.operationManager.stripButtonOperation.button_Click(sender, e);
            //this.webBrowser1.Url = new Uri("https://www.baidu.com/");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            linkTest.Left -= 2;//设置label1左边缘与其容器的工作区左边缘之间的距离
            if (linkTest.Right < 0)//当label1右边缘与其容器的工作区左边缘之间的距离小于0时
            {
                linkTest.Left = this.panelTopInfo.Width;//设置label1左边缘与其容器的工作区左边缘之间的距离为该窗体的宽度
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int min2 = DateTime.Now.Minute % 10;
            if (min2!=tempMin)
            {
                this.panelTopTime.Invalidate();
                this.tempMin = min2;
            }        
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.operationManager.stripButtonOperation.MyPerformStep == 100)
            {
                this.Opacity = 1;
                timer2.Enabled = false;
            }
            


        }
       
        //public void BtnThrid_Click(object sender, EventArgs e)
        //{

        //    Button btn = (Button)sender;
        //    tagUrl = btn.Tag.ToString();
        //    btnNumber = Int32.Parse(btn.Name);
        //    this.tabControl1.SelectTab(1);
        //    if (lastThrid != null)
        //    {
        //        lastThrid.BackColor = Color.FromArgb(231, 215, 174);
        //        //lastThrid.ForeColor = Color.FromArgb(235, 235, 235);
        //    }
        //    btn.BackColor = Color.White;
        //    btn.ForeColor = Color.Black;
        //    lastThrid = btn;
        //    if (this.operationManager.isLogin == true)
        //    {
        //        string y = tagUrl;
        //        webBrowser1.Address = y;
        //    }
        //    else 
        //    {

        //        DialogResult dr =  MessageBox.Show("您尚未登录，请登录后再操作！", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //        if (dr == DialogResult.OK)
        //        {
        //            //用户选择确认的操作
        //            MessageBox.Show("您选择的是【确认】");
        //        }
        //        this.CreateLoginForm(null);
        //        this.tabControl1.SelectTab(0);
        //    }
            
        //}
       
        //查看IPaddress
        public void btnGetIP_Click(object sender, EventArgs e)
        {
            StateForm stateForm = new StateForm();
            stateForm.lblClientVersion.Text = text;
            stateForm.lblTime.Text = timeNow;
            if (this.operationManager.stripButtonOperation.HQIPaddress != null)
            {
                stateForm.lblHQAddress.Text = this.operationManager.stripButtonOperation.HQIPaddress;
            }
            if (this.operationManager.isLogin == true)
            {
                if (this.operationManager.stripButtonOperation.TradeIPaddress != null)
                {
                    stateForm.lblTradeAddress.Text = this.operationManager.stripButtonOperation.TradeIPaddress;

                }
                if (this.operationManager.stripButtonOperation.CounmerIPadress != null)
                {
                    stateForm.lblCounmerAddress.Text = this.operationManager.stripButtonOperation.CounmerIPadress;
                }
            }
            else 
            {
                stateForm.lblTradeAddress.Text = "尚未连接";
                stateForm.lblCounmerAddress.Text = "尚未连接";
            }
            stateForm.ShowDialog();
        }



        private void linkTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.webBrowser1.Address = "http://www.yrdce.cn/";
            this.tabControl1.SelectTab(1);
            this.webBrowser1.Load("http://www.yrdce.cn/");
        }


        private void SetButton(Button button)
        {
            MethodInfo methodinfo = button.GetType().GetMethod("SetStyle",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            methodinfo.Invoke(button,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,null,new object[] {ControlStyles.Selectable,false},Application.CurrentCulture);
        }
        
        

        //网页禁止右键
        internal class MYMenuHandler : IMenuHandler
        {
        
                public bool OnBeforeMenu(IWebBrowser browser) { return true; }
         }

        
    }


  

        


}

