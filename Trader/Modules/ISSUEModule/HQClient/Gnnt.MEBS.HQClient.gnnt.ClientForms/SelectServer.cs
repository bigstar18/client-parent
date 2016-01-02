using Configuration;
using Gnnt.MEBS.HQClient.gnnt.BitmapControl;
using Gnnt.MEBS.HQModel;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class SelectServer : Form
	{
		public const int WM_SYSCOMMAND = 274;
		public const int SC_MOVE = 61456;
		public const int HTCAPTION = 2;
		private IContainer components;
		private Label lbProxy;
		private Label lbExit;
		private Label lbSetServer;
		private Label lbLogin;
		private static volatile SelectServer instance = null;
		public ResourceManager resourceManager;
		public static Icon SystemIcon;
		public static event EventHandler selectServerFormLoad;
		public static event FormClosedEventHandler selectServerFormClosed;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SelectServer));
			this.lbProxy = new Label();
			this.lbExit = new Label();
			this.lbSetServer = new Label();
			this.lbLogin = new Label();
			base.SuspendLayout();
			this.lbProxy.BackColor = Color.Transparent;
			this.lbProxy.Cursor = Cursors.Hand;
			this.lbProxy.Location = new Point(292, 118);
			this.lbProxy.Name = "lbProxy";
			this.lbProxy.Size = new Size(77, 24);
			this.lbProxy.TabIndex = 5;
			this.lbProxy.Click += new EventHandler(this.lbProxy_Click);
			this.lbExit.BackColor = Color.Transparent;
			this.lbExit.Cursor = Cursors.Hand;
			this.lbExit.Location = new Point(254, 156);
			this.lbExit.Name = "lbExit";
			this.lbExit.Size = new Size(62, 24);
			this.lbExit.TabIndex = 5;
			this.lbExit.Click += new EventHandler(this.lbExit_Click);
			this.lbSetServer.BackColor = Color.Transparent;
			this.lbSetServer.Cursor = Cursors.Hand;
			this.lbSetServer.Location = new Point(103, 183);
			this.lbSetServer.Name = "lbSetServer";
			this.lbSetServer.Size = new Size(62, 24);
			this.lbSetServer.TabIndex = 5;
			this.lbSetServer.Click += new EventHandler(this.lbSetServer_Click);
			this.lbLogin.BackColor = Color.Transparent;
			this.lbLogin.Cursor = Cursors.Hand;
			this.lbLogin.Location = new Point(172, 173);
			this.lbLogin.Name = "lbLogin";
			this.lbLogin.Size = new Size(62, 24);
			this.lbLogin.TabIndex = 5;
			this.lbLogin.Click += new EventHandler(this.lbLogin_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.AppWorkspace;
			this.BackgroundImage = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
			this.BackgroundImageLayout = ImageLayout.Center;
			base.ClientSize = new Size(402, 271);
			base.ControlBox = false;
			base.Controls.Add(this.lbLogin);
			base.Controls.Add(this.lbSetServer);
			base.Controls.Add(this.lbExit);
			base.Controls.Add(this.lbProxy);
			this.DoubleBuffered = true;
			this.ForeColor = SystemColors.ControlText;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "SelectServer";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "SelectServer";
			base.Load += new EventHandler(this.SelectServer_Load);
			base.FormClosed += new FormClosedEventHandler(this.SelectServer_FormClosed);
			base.MouseDown += new MouseEventHandler(this.SelectServer_MouseDown);
			base.ResumeLayout(false);
		}
		private SelectServer()
		{
			this.InitializeComponent();
		}
		public static SelectServer GetInstance()
		{
			if (SelectServer.instance == null)
			{
				lock (typeof(SelectServer))
				{
					if (SelectServer.instance == null)
					{
						SelectServer.instance = new SelectServer();
					}
				}
			}
			return SelectServer.instance;
		}
		private void SelectServer_Load(object sender, EventArgs e)
		{
			if (this.SetLanguage("Chinese"))
			{
				new BitmapRegion();
				Image image = (Image)this.resourceManager.GetObject("HQImg_main");
				Bitmap bitmap = new Bitmap((Bitmap)image);
				BitmapRegion.CreateControlRegion(this, bitmap);
				this.SetControlText();
				if (SelectServer.selectServerFormLoad != null)
				{
					SelectServer.selectServerFormLoad(this, e);
					return;
				}
			}
			else
			{
				MessageBox.Show("没有发现资源文件，系统将退出！", "查找资源文件", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
		}
		private bool SetLanguage(string language)
		{
			if (language.Equals("Chinese"))
			{
				if (!File.Exists("Gnnt.MEBS.ch.resources"))
				{
					return false;
				}
				this.resourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", null);
			}
			else
			{
				if (!File.Exists("Gnnt.en.resources"))
				{
					return false;
				}
				this.resourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.en", "", null);
			}
			return true;
		}
		private void SetControlText()
		{
			this.Text = "选择服务器";
			SelectServer.SystemIcon = (Icon)this.resourceManager.GetObject("Logo.ico");
			base.Icon = SelectServer.SystemIcon;
		}
		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
		private void SelectServer_MouseDown(object sender, MouseEventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized && e.Clicks == 1)
			{
				SelectServer.ReleaseCapture();
				SelectServer.SendMessage(base.Handle, 274, 61458, 0);
			}
		}
		private void SelectServer_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (SelectServer.selectServerFormClosed != null)
			{
				SelectServer.selectServerFormClosed(this, e);
			}
			else
			{
				Application.Exit();
			}
			SelectServer.instance = null;
		}
		private void lbProxy_Click(object sender, EventArgs e)
		{
			SetProxyServer setProxyServer = new SetProxyServer(this.resourceManager);
			setProxyServer.Show();
		}
		private void lbExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void lbLogin_Click(object sender, EventArgs e)
		{
			PluginInfo pluginInfo = new PluginInfo();
			pluginInfo.HQResourceManager = this.resourceManager;
			Configuration configuration = new Configuration();
			pluginInfo.HTConfig = configuration.getSection("HQSystems");
			pluginInfo.IPAddress = "172.16.20.220";
			pluginInfo.Port = 16906;
			pluginInfo.HttpPort = 16907;
			HQClientForm hQClientForm = new HQClientForm(new MainWindow(pluginInfo));
			hQClientForm.Show();
			base.Hide();
		}
		private void lbSetServer_Click(object sender, EventArgs e)
		{
		}
	}
}
