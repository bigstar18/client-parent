// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.SelectServer
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Configuration;
using Gnnt.MEBS.HQClient;
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
    private static volatile SelectServer instance = (SelectServer) null;
    public const int WM_SYSCOMMAND = 274;
    public const int SC_MOVE = 61456;
    public const int HTCAPTION = 2;
    private IContainer components;
    private Label lbProxy;
    private Label lbExit;
    private Label lbSetServer;
    private Label lbLogin;
    public ResourceManager resourceManager;
    public static Icon SystemIcon;

    public static event EventHandler selectServerFormLoad;

    public static event FormClosedEventHandler selectServerFormClosed;

    private SelectServer()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectServer));
      this.lbProxy = new Label();
      this.lbExit = new Label();
      this.lbSetServer = new Label();
      this.lbLogin = new Label();
      this.SuspendLayout();
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
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.AppWorkspace;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.BackgroundImageLayout = ImageLayout.Center;
      this.ClientSize = new Size(402, 271);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lbLogin);
      this.Controls.Add((Control) this.lbSetServer);
      this.Controls.Add((Control) this.lbExit);
      this.Controls.Add((Control) this.lbProxy);
      this.DoubleBuffered = true;
      this.ForeColor = SystemColors.ControlText;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = "SelectServer";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SelectServer";
      this.Load += new EventHandler(this.SelectServer_Load);
      this.FormClosed += new FormClosedEventHandler(this.SelectServer_FormClosed);
      this.MouseDown += new MouseEventHandler(this.SelectServer_MouseDown);
      this.ResumeLayout(false);
    }

    public static SelectServer GetInstance()
    {
      if (SelectServer.instance == null)
      {
        lock (typeof (SelectServer))
        {
          if (SelectServer.instance == null)
            SelectServer.instance = new SelectServer();
        }
      }
      return SelectServer.instance;
    }

    private void SelectServer_Load(object sender, EventArgs e)
    {
      if (this.SetLanguage("Chinese"))
      {
        BitmapRegion bitmapRegion = new BitmapRegion();
        BitmapRegion.CreateControlRegion((Control) this, new Bitmap((Image) this.resourceManager.GetObject("HQImg_main")));
        this.SetControlText();
        if (SelectServer.selectServerFormLoad == null)
          return;
        SelectServer.selectServerFormLoad((object) this, e);
      }
      else
      {
        int num = (int) MessageBox.Show("没有发现资源文件，系统将退出！", "查找资源文件", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
    }

    private bool SetLanguage(string language)
    {
      if (language.Equals("Chinese"))
      {
        if (!File.Exists("Gnnt.MEBS.ch.resources"))
          return false;
        this.resourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", (System.Type) null);
      }
      else
      {
        if (!File.Exists("Gnnt.en.resources"))
          return false;
        this.resourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.en", "", (System.Type) null);
      }
      return true;
    }

    private void SetControlText()
    {
      this.Text = "选择服务器";
      SelectServer.SystemIcon = (Icon) this.resourceManager.GetObject("Logo.ico");
      this.Icon = SelectServer.SystemIcon;
    }

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    private void SelectServer_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.WindowState == FormWindowState.Maximized || e.Clicks != 1)
        return;
      SelectServer.ReleaseCapture();
      SelectServer.SendMessage(this.Handle, 274, 61458, 0);
    }

    private void SelectServer_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (SelectServer.selectServerFormClosed != null)
        SelectServer.selectServerFormClosed((object) this, e);
      else
        Application.Exit();
      SelectServer.instance = (SelectServer) null;
    }

    private void lbProxy_Click(object sender, EventArgs e)
    {
      new SetProxyServer(this.resourceManager).Show();
    }

    private void lbExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void lbLogin_Click(object sender, EventArgs e)
    {
      PluginInfo _pluginInfo = new PluginInfo();
      _pluginInfo.HQResourceManager = this.resourceManager;
      Configuration configuration = new Configuration();
      _pluginInfo.HTConfig = configuration.getSection("HQSystems");
      _pluginInfo.IPAddress = "172.16.20.220";
      _pluginInfo.Port = 16906;
      _pluginInfo.HttpPort = 16907;
      new HQClientForm(new MainWindow(_pluginInfo)).Show();
      this.Hide();
    }

    private void lbSetServer_Click(object sender, EventArgs e)
    {
    }
  }
}
