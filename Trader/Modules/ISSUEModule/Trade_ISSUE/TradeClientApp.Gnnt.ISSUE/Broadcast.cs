using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeClientApp.Gnnt.ISSUE.Library;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class Broadcast : Form
	{
		public const int WM_SYSCOMMAND = 274;
		public const int SC_MOVE = 61456;
		public const int HTCAPTION = 2;
		private int state;
		private int height;
		private string staticSysInfo = "系统广播消息：   ";
		private string sysInfo = "系统广播消息：   ";
		private ArrayList boradList = new ArrayList();
		private IContainer components;
		private RichTextBox BroadcastText;
		private Label labInfo;
		private Button butClose;
		private Button butModel;
		private Timer timer1;
		private SplitContainer splitContainer1;
		private Panel panelCover;
		public Broadcast()
		{
			this.InitializeComponent();
		}
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
		private void Broadcast_Load(object sender, EventArgs e)
		{
			this.height = base.Height;
			this.timer1.Enabled = true;
			base.Height = this.splitContainer1.Panel1.Height;
			this.splitContainer1.Panel2Collapsed = false;
			this.splitContainer1.Panel1.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_Title");
			this.butClose.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_close");
			this.butModel.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_max");
			this.state = 1;
			ScaleForm.ScaleForms(this);
		}
		public void broadCastBuffer(object obj)
		{
			List<TradeInterface.Gnnt.ISSUE.DataVO.Broadcast> list = (List<TradeInterface.Gnnt.ISSUE.DataVO.Broadcast>)obj;
			for (int i = 0; i < list.Count; i++)
			{
				this.boradList.Add(list[i].content);
			}
			this.showBroadcast();
		}
		private void showBroadcast()
		{
			int num = 1;
			this.BroadcastText.Clear();
			this.BroadcastText.ReadOnly = true;
			this.sysInfo = this.staticSysInfo;
			for (int i = this.boradList.Count - 1; i >= 0; i--)
			{
				string text = this.boradList[i].ToString();
				text = text.Replace("\r\n", "");
				object obj = this.sysInfo;
				this.sysInfo = string.Concat(new object[]
				{
					obj,
					"            『",
					num,
					":",
					text,
					"』"
				});
				this.BroadcastText.AppendText(string.Concat(new object[]
				{
					num,
					":",
					this.boradList[i],
					"\n\r\n"
				}));
				this.BroadcastText.Focus();
				this.BroadcastText.SelectionStart = 0;
				this.BroadcastText.ScrollToCaret();
				num++;
				if (base.Visible)
				{
					Global.broadcastFlag = false;
				}
				else
				{
					Global.broadcastFlag = true;
					if (Tools.StrToBool((string)Global.HTConfig["DisplayBroadcast"], false))
					{
						base.Show();
					}
				}
			}
			if (this.state == 0)
			{
				this.labInfo.Text = this.staticSysInfo;
				return;
			}
			if (this.state == 1)
			{
				this.labInfo.Text = this.sysInfo;
			}
		}
		private void butClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}
		private void butModel_Click(object sender, EventArgs e)
		{
			if (this.state == 0)
			{
				this.butModel.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_max");
				this.labInfo.Text = this.sysInfo;
				this.panelCover.BorderStyle = BorderStyle.Fixed3D;
				this.panelCover.BackColor = Color.Black;
				this.labInfo.BackColor = Color.Black;
				this.labInfo.ForeColor = Color.White;
				this.timer1.Enabled = true;
				base.Height = this.splitContainer1.Panel1.Height;
				this.splitContainer1.Panel2Collapsed = true;
				this.state = 1;
				return;
			}
			if (this.state == 1)
			{
				this.butModel.BackgroundImage = (this.butModel.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_reset"));
				this.panelCover.BorderStyle = BorderStyle.None;
				this.panelCover.BackColor = Color.Transparent;
				this.labInfo.BackColor = Color.Transparent;
				FontStyle fontStyle = this.labInfo.Font.Style;
				fontStyle |= FontStyle.Bold;
				this.labInfo.Font = new Font(this.labInfo.Font, fontStyle);
				this.labInfo.ForeColor = Color.Blue;
				this.labInfo.Left = 0;
				base.Height = this.height;
				this.timer1.Enabled = false;
				this.labInfo.Text = "系统广播消息";
				this.splitContainer1.Panel2Collapsed = false;
				this.state = 0;
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.labInfo.Left = this.labInfo.Left - 5;
			if (this.labInfo.Left <= -this.labInfo.Width)
			{
				this.labInfo.Left = this.panelCover.Width;
				this.labInfo.Text = this.sysInfo;
			}
		}
		private void panelCover_MouseDown(object sender, MouseEventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized && e.Clicks == 1)
			{
				Broadcast.ReleaseCapture();
				Broadcast.SendMessage(base.Handle, 274, 61458, 0);
			}
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
			this.BroadcastText = new RichTextBox();
			this.labInfo = new Label();
			this.butClose = new Button();
			this.butModel = new Button();
			this.timer1 = new Timer(this.components);
			this.splitContainer1 = new SplitContainer();
			this.panelCover = new Panel();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panelCover.SuspendLayout();
			base.SuspendLayout();
			this.BroadcastText.BackColor = Color.Gainsboro;
			this.BroadcastText.Dock = DockStyle.Fill;
			this.BroadcastText.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.BroadcastText.Location = new Point(0, 0);
			this.BroadcastText.Name = "BroadcastText";
			this.BroadcastText.Size = new Size(650, 472);
			this.BroadcastText.TabIndex = 1;
			this.BroadcastText.Text = "";
			this.BroadcastText.MouseDown += new MouseEventHandler(this.panelCover_MouseDown);
			this.labInfo.AutoSize = true;
			this.labInfo.BackColor = SystemColors.ControlText;
			this.labInfo.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labInfo.ForeColor = Color.White;
			this.labInfo.Location = new Point(3, 4);
			this.labInfo.Name = "labInfo";
			this.labInfo.Size = new Size(57, 12);
			this.labInfo.TabIndex = 2;
			this.labInfo.Text = "系统消息";
			this.labInfo.TextAlign = ContentAlignment.MiddleLeft;
			this.butClose.Location = new Point(617, 4);
			this.butClose.Name = "butClose";
			this.butClose.Size = new Size(25, 18);
			this.butClose.TabIndex = 3;
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new EventHandler(this.butClose_Click);
			this.butModel.Location = new Point(591, 4);
			this.butModel.Name = "butModel";
			this.butModel.Size = new Size(25, 18);
			this.butModel.TabIndex = 4;
			this.butModel.UseVisualStyleBackColor = true;
			this.butModel.Click += new EventHandler(this.butModel_Click);
			this.timer1.Interval = 200;
			this.timer1.Tick += new EventHandler(this.timer1_Tick);
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.BackColor = SystemColors.Control;
			this.splitContainer1.Panel1.Controls.Add(this.butClose);
			this.splitContainer1.Panel1.Controls.Add(this.panelCover);
			this.splitContainer1.Panel1.Controls.Add(this.butModel);
			this.splitContainer1.Panel1.MouseDown += new MouseEventHandler(this.panelCover_MouseDown);
			this.splitContainer1.Panel2.Controls.Add(this.BroadcastText);
			this.splitContainer1.Size = new Size(650, 500);
			this.splitContainer1.SplitterDistance = 27;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 5;
			this.panelCover.BackColor = Color.Black;
			this.panelCover.BorderStyle = BorderStyle.Fixed3D;
			this.panelCover.Controls.Add(this.labInfo);
			this.panelCover.Location = new Point(2, 2);
			this.panelCover.Name = "panelCover";
			this.panelCover.Size = new Size(586, 23);
			this.panelCover.TabIndex = 3;
			this.panelCover.MouseDown += new MouseEventHandler(this.panelCover_MouseDown);
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(650, 500);
			base.Controls.Add(this.splitContainer1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.MaximizeBox = false;
			base.Name = "Broadcast";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "消息";
			base.TopMost = true;
			base.Load += new EventHandler(this.Broadcast_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.panelCover.ResumeLayout(false);
			this.panelCover.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
