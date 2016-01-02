// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.SetProxyServer
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Org.Mentalis.Network.ProxySocket
{
  public class SetProxyServer : Form
  {
    private IContainer components;
    private GroupBox gbProxy;
    private TextBox tbPassword;
    private TextBox tbProxyPort;
    private TextBox tbUserName;
    private TextBox tbProxyIP;
    private Label label4;
    private Label lbPwd;
    private Label lbUserName;
    private Label lbProxyIP;
    private RadioButton rbHttp;
    private RadioButton rbSock5;
    private RadioButton rbSock4;
    private CheckBox cbProxyEnable;
    private Button btCancel;
    private Button btConfirm;
    private Button btTestProxy;
    private IniFile iniFile;
    private ResourceManager SysResourceManager;

    public SetProxyServer(ResourceManager resource)
    {
      this.InitializeComponent();
      this.SysResourceManager = resource;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gbProxy = new GroupBox();
      this.btCancel = new Button();
      this.btConfirm = new Button();
      this.btTestProxy = new Button();
      this.tbPassword = new TextBox();
      this.tbProxyPort = new TextBox();
      this.tbUserName = new TextBox();
      this.tbProxyIP = new TextBox();
      this.label4 = new Label();
      this.lbPwd = new Label();
      this.lbUserName = new Label();
      this.lbProxyIP = new Label();
      this.rbHttp = new RadioButton();
      this.rbSock5 = new RadioButton();
      this.rbSock4 = new RadioButton();
      this.cbProxyEnable = new CheckBox();
      this.gbProxy.SuspendLayout();
      this.SuspendLayout();
      this.gbProxy.Controls.Add((Control) this.btCancel);
      this.gbProxy.Controls.Add((Control) this.btConfirm);
      this.gbProxy.Controls.Add((Control) this.btTestProxy);
      this.gbProxy.Controls.Add((Control) this.tbPassword);
      this.gbProxy.Controls.Add((Control) this.tbProxyPort);
      this.gbProxy.Controls.Add((Control) this.tbUserName);
      this.gbProxy.Controls.Add((Control) this.tbProxyIP);
      this.gbProxy.Controls.Add((Control) this.label4);
      this.gbProxy.Controls.Add((Control) this.lbPwd);
      this.gbProxy.Controls.Add((Control) this.lbUserName);
      this.gbProxy.Controls.Add((Control) this.lbProxyIP);
      this.gbProxy.Controls.Add((Control) this.rbHttp);
      this.gbProxy.Controls.Add((Control) this.rbSock5);
      this.gbProxy.Controls.Add((Control) this.rbSock4);
      this.gbProxy.Controls.Add((Control) this.cbProxyEnable);
      this.gbProxy.Dock = DockStyle.Fill;
      this.gbProxy.Location = new Point(0, 0);
      this.gbProxy.Name = "gbProxy";
      this.gbProxy.Size = new Size(392, 189);
      this.gbProxy.TabIndex = 0;
      this.gbProxy.TabStop = false;
      this.gbProxy.Text = "代理";
      this.btCancel.Location = new Point(209, 157);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new Size(70, 23);
      this.btCancel.TabIndex = 9;
      this.btCancel.Text = "取 消";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.btConfirm.Location = new Point(121, 157);
      this.btConfirm.Name = "btConfirm";
      this.btConfirm.Size = new Size(70, 23);
      this.btConfirm.TabIndex = 9;
      this.btConfirm.Text = "确 定";
      this.btConfirm.UseVisualStyleBackColor = true;
      this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
      this.btTestProxy.Location = new Point(302, 129);
      this.btTestProxy.Name = "btTestProxy";
      this.btTestProxy.Size = new Size(62, 21);
      this.btTestProxy.TabIndex = 9;
      this.btTestProxy.Text = "测试代理";
      this.btTestProxy.UseVisualStyleBackColor = true;
      this.btTestProxy.Click += new EventHandler(this.btTestProxy_Click);
      this.tbPassword.Location = new Point(272, 93);
      this.tbPassword.Name = "tbPassword";
      this.tbPassword.PasswordChar = '*';
      this.tbPassword.Size = new Size(94, 21);
      this.tbPassword.TabIndex = 8;
      this.tbProxyPort.Location = new Point(313, 56);
      this.tbProxyPort.Name = "tbProxyPort";
      this.tbProxyPort.Size = new Size(51, 21);
      this.tbProxyPort.TabIndex = 8;
      this.tbUserName.Location = new Point(96, 93);
      this.tbUserName.Name = "tbUserName";
      this.tbUserName.Size = new Size(95, 21);
      this.tbUserName.TabIndex = 8;
      this.tbProxyIP.Location = new Point(96, 56);
      this.tbProxyIP.Name = "tbProxyIP";
      this.tbProxyIP.Size = new Size(193, 21);
      this.tbProxyIP.TabIndex = 8;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.label4.Location = new Point(294, 57);
      this.label4.Name = "label4";
      this.label4.Size = new Size(15, 14);
      this.label4.TabIndex = 7;
      this.label4.Text = ":";
      this.lbPwd.AutoSize = true;
      this.lbPwd.Location = new Point(207, 98);
      this.lbPwd.Name = "lbPwd";
      this.lbPwd.Size = new Size(59, 12);
      this.lbPwd.TabIndex = 6;
      this.lbPwd.Text = "验证密码:";
      this.lbUserName.AutoSize = true;
      this.lbUserName.Location = new Point(29, 98);
      this.lbUserName.Name = "lbUserName";
      this.lbUserName.Size = new Size(59, 12);
      this.lbUserName.TabIndex = 5;
      this.lbUserName.Text = "验证用户:";
      this.lbProxyIP.AutoSize = true;
      this.lbProxyIP.Location = new Point(29, 59);
      this.lbProxyIP.Name = "lbProxyIP";
      this.lbProxyIP.Size = new Size(59, 12);
      this.lbProxyIP.TabIndex = 4;
      this.lbProxyIP.Text = "代理地址:";
      this.rbHttp.AutoSize = true;
      this.rbHttp.Location = new Point(297, 20);
      this.rbHttp.Name = "rbHttp";
      this.rbHttp.Size = new Size(71, 16);
      this.rbHttp.TabIndex = 3;
      this.rbHttp.TabStop = true;
      this.rbHttp.Text = "HTTP协议";
      this.rbHttp.UseVisualStyleBackColor = true;
      this.rbSock5.AutoSize = true;
      this.rbSock5.Location = new Point(197, 20);
      this.rbSock5.Name = "rbSock5";
      this.rbSock5.Size = new Size(77, 16);
      this.rbSock5.TabIndex = 2;
      this.rbSock5.TabStop = true;
      this.rbSock5.Text = "SOCK5协议";
      this.rbSock5.UseVisualStyleBackColor = true;
      this.rbSock4.AutoSize = true;
      this.rbSock4.Location = new Point(96, 20);
      this.rbSock4.Name = "rbSock4";
      this.rbSock4.Size = new Size(77, 16);
      this.rbSock4.TabIndex = 1;
      this.rbSock4.TabStop = true;
      this.rbSock4.Text = "SOCK4协议";
      this.rbSock4.UseVisualStyleBackColor = true;
      this.cbProxyEnable.AutoSize = true;
      this.cbProxyEnable.Location = new Point(12, 20);
      this.cbProxyEnable.Name = "cbProxyEnable";
      this.cbProxyEnable.Size = new Size(72, 16);
      this.cbProxyEnable.TabIndex = 0;
      this.cbProxyEnable.Text = "使用代理";
      this.cbProxyEnable.UseVisualStyleBackColor = true;
      this.cbProxyEnable.CheckedChanged += new EventHandler(this.cbProxyEnable_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(392, 189);
      
      this.Controls.Add((Control) this.gbProxy);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SetProxyServer";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SetProxyServer";
      this.Load += new EventHandler(this.SetProxyServer_Load);
      this.gbProxy.ResumeLayout(false);
      this.gbProxy.PerformLayout();
      this.ResumeLayout(false);
    }

    private void SetProxyServer_Load(object sender, EventArgs e)
    {
      if (System.IO.File.Exists("Proxy.ini"))
      {
        try
        {
          this.iniFile = new IniFile("Proxy.ini");
          bool flag = bool.Parse(this.iniFile.IniReadValue("ProxyServer", "Enable"));
          int num1 = int.Parse(this.iniFile.IniReadValue("ProxyServer", "Type"));
          string str1 = this.iniFile.IniReadValue("ProxyServer", "ProxyIP");
          int num2 = int.Parse(this.iniFile.IniReadValue("ProxyServer", "ProxyPort"));
          string str2 = this.iniFile.IniReadValue("ProxyServer", "UserName");
          string str3 = this.iniFile.IniReadValue("ProxyServer", "Password");
          if (flag)
          {
            this.cbProxyEnable.Checked = true;
          }
          else
          {
            this.cbProxyEnable.Checked = false;
            this.tbProxyIP.Enabled = false;
            this.tbProxyPort.Enabled = false;
            this.tbUserName.Enabled = false;
            this.tbPassword.Enabled = false;
            this.lbProxyIP.Enabled = false;
            this.lbUserName.Enabled = false;
            this.lbPwd.Enabled = false;
            this.rbSock4.Enabled = false;
            this.rbSock5.Enabled = false;
            this.rbHttp.Enabled = false;
            this.btTestProxy.Enabled = false;
          }
          this.tbProxyIP.Text = str1;
          this.tbProxyPort.Text = num2.ToString();
          this.tbUserName.Text = str2;
          this.tbPassword.Text = str3;
          if (num1 == 0)
            this.rbSock4.Checked = true;
          else if (num1 == 1)
            this.rbSock5.Checked = true;
          else if (num1 == 2)
            this.rbHttp.Checked = true;
        }
        catch
        {
          this.cbProxyEnable.Checked = false;
          this.tbProxyIP.Enabled = false;
          this.tbProxyPort.Enabled = false;
          this.tbUserName.Enabled = false;
          this.tbPassword.Enabled = false;
          this.lbProxyIP.Enabled = false;
          this.lbUserName.Enabled = false;
          this.lbPwd.Enabled = false;
          this.rbSock4.Enabled = false;
          this.rbSock5.Enabled = false;
          this.rbHttp.Enabled = false;
          this.btTestProxy.Enabled = false;
        }
      }
      this.SetControlText();
    }

    private void SetControlText()
    {
      this.gbProxy.Text = this.SysResourceManager.GetString("ProxyStr_gbProxy");
      this.cbProxyEnable.Text = this.SysResourceManager.GetString("ProxyStr_cbProxyEnable");
      this.rbSock4.Text = this.SysResourceManager.GetString("ProxyStr_rbSock4");
      this.rbSock5.Text = this.SysResourceManager.GetString("ProxyStr_rbSock5");
      this.rbHttp.Text = this.SysResourceManager.GetString("ProxyStr_rbHttp");
      this.lbProxyIP.Text = this.SysResourceManager.GetString("ProxyStr_lbProxyIP");
      this.lbUserName.Text = this.SysResourceManager.GetString("ProxyStr_lbUserName");
      this.lbPwd.Text = this.SysResourceManager.GetString("ProxyStr_lbPwd");
      this.btTestProxy.Text = this.SysResourceManager.GetString("ProxyStr_btTestProxy");
      this.btConfirm.Text = this.SysResourceManager.GetString("ProxyStr_btConfirm");
      this.btCancel.Text = this.SysResourceManager.GetString("ProxyStr_btCancel");
    }

    private void btConfirm_Click(object sender, EventArgs e)
    {
      bool @checked = this.cbProxyEnable.Checked;
      int num1 = -1;
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      if (this.iniFile == null)
        this.iniFile = new IniFile("Proxy.ini");
      if (@checked)
      {
        if (!this.rbSock4.Checked && !this.rbSock5.Checked && !this.rbHttp.Checked)
        {
          int num2 = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_SelectServerType"));
          return;
        }
        if (this.rbSock4.Checked)
          num1 = 0;
        else if (this.rbSock5.Checked)
          num1 = 1;
        else if (this.rbHttp.Checked)
          num1 = 2;
        int num3;
        try
        {
          num3 = int.Parse(this.tbProxyPort.Text);
          if (num3 > 0)
          {
            if (num3 <= (int) ushort.MaxValue)
              goto label_15;
          }
          int num2 = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorPort"));
          return;
        }
        catch
        {
          int num2 = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorPort"));
          return;
        }
label_15:
        string text1 = this.tbProxyIP.Text;
        if (text1.Length < 5)
        {
          int num2 = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorIP"));
          return;
        }
        string text2 = this.tbUserName.Text;
        string text3 = this.tbPassword.Text;
        this.iniFile.IniWriteValue("ProxyServer", "Enable", "True");
        this.iniFile.IniWriteValue("ProxyServer", "Type", num1.ToString());
        this.iniFile.IniWriteValue("ProxyServer", "ProxyIP", text1);
        this.iniFile.IniWriteValue("ProxyServer", "ProxyPort", num3.ToString());
        this.iniFile.IniWriteValue("ProxyServer", "UserName", text2);
        this.iniFile.IniWriteValue("ProxyServer", "Password", text3);
      }
      else
        this.iniFile.IniWriteValue("ProxyServer", "Enable", "False");
      this.Close();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btTestProxy_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(this.testProxyServer)).Start();
    }

    private void testProxyServer()
    {
      SetProxyServer.BoolDelegate boolDelegate = new SetProxyServer.BoolDelegate(this.gbProxyEnabled);
      try
      {
        string str = string.Empty;
        if (!this.rbSock4.Checked)
        {
          if (!this.rbSock5.Checked)
          {
            if (!this.rbHttp.Checked)
            {
              int num = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_SelectServerType"));
              return;
            }
          }
        }
        int port;
        try
        {
          port = int.Parse(this.tbProxyPort.Text);
          if (port > 0)
          {
            if (port <= (int) ushort.MaxValue)
              goto label_9;
          }
          int num = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorPort"));
          return;
        }
        catch
        {
          int num = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorPort"));
          return;
        }
label_9:
        string text = this.tbProxyIP.Text;
        if (text.Length < 5)
        {
          int num = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_ErrorIP"));
          return;
        }
        this.Invoke((Delegate) boolDelegate, (object) false);
        ProxySocket proxySocket = new ProxySocket();
        proxySocket.ProxyEndPoint = new IPEndPoint(IPAddress.Parse(text), port);
        if (this.rbSock4.Checked)
        {
          proxySocket.ProxyUser = this.tbUserName.Text;
          proxySocket.ProxyType = ProxyTypes.Socks4;
        }
        else if (this.rbSock5.Checked)
        {
          proxySocket.ProxyUser = this.tbUserName.Text;
          proxySocket.ProxyPass = this.tbPassword.Text;
          proxySocket.ProxyType = ProxyTypes.Socks5;
        }
        else if (this.rbHttp.Checked)
        {
          proxySocket.ProxyUser = this.tbUserName.Text;
          proxySocket.ProxyPass = this.tbPassword.Text;
          proxySocket.ProxyType = ProxyTypes.Http;
        }
        proxySocket.Connect((EndPoint) new IPEndPoint(Dns.GetHostAddresses("www.baidu.com")[0], 80));
        proxySocket.Send(Encoding.ASCII.GetBytes("GET / HTTP/1.0\r\nHost: www.baidu.com\r\n\r\n"));
        byte[] numArray = new byte[1024];
        for (int count = proxySocket.Receive(numArray); count > 0; count = proxySocket.Receive(numArray))
          Console.Write(Encoding.ASCII.GetString(numArray, 0, count));
        int num1 = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_TestSuccess"));
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        int num = (int) MessageBox.Show(this.SysResourceManager.GetString("ProxyStr_TestFails"));
      }
      this.Invoke((Delegate) boolDelegate, (object) true);
    }

    private void gbProxyEnabled(bool enable)
    {
      this.gbProxy.Enabled = enable;
    }

    private void cbProxyEnable_CheckedChanged(object sender, EventArgs e)
    {
      this.cbProxyEnable.Checked = this.cbProxyEnable.Checked;
      this.tbProxyIP.Enabled = this.cbProxyEnable.Checked;
      this.tbProxyPort.Enabled = this.cbProxyEnable.Checked;
      this.tbUserName.Enabled = this.cbProxyEnable.Checked;
      this.tbPassword.Enabled = this.cbProxyEnable.Checked;
      this.lbProxyIP.Enabled = this.cbProxyEnable.Checked;
      this.lbUserName.Enabled = this.cbProxyEnable.Checked;
      this.lbPwd.Enabled = this.cbProxyEnable.Checked;
      this.rbSock4.Enabled = this.cbProxyEnable.Checked;
      this.rbSock5.Enabled = this.cbProxyEnable.Checked;
      this.rbHttp.Enabled = this.cbProxyEnable.Checked;
      this.btTestProxy.Enabled = this.cbProxyEnable.Checked;
    }

    private delegate void BoolDelegate(bool b);
  }
}
