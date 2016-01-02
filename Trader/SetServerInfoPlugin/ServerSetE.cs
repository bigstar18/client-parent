﻿// Decompiled with JetBrains decompiler
// Type: SetServerInfoPlugin.ServerSetE
// Assembly: SetServerInfoPlugin, Version=3.0.8.0, Culture=neutral, PublicKeyToken=null
// MVID: E04F003E-2DD5-4E4F-8F62-E41AF4AB517D
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\SetServerInfoPlugin.dll

using Org.Mentalis.Network.ProxySocket;
using PluginInterface;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SetServerInfoPlugin
{
  public class ServerSetE : Form
  {
    private IContainer components;
    private GroupBox groupBoxServer;
    private Button btConfirm;
    private Button btCancel;
    private Button btApply;
    private RadioButton radioNet;
    private RadioButton radioTel;
    private Button btSetProxy;
    private IPluginHost myHost;
    private Hashtable htConfig;
    private bool isUpdate;
    private int curServer;

    public ServerSetE(IPluginHost myHost)
    {
      this.InitializeComponent();
      this.myHost = myHost;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBoxServer = new GroupBox();
      this.radioNet = new RadioButton();
      this.radioTel = new RadioButton();
      this.btConfirm = new Button();
      this.btCancel = new Button();
      this.btApply = new Button();
      this.btSetProxy = new Button();
      this.groupBoxServer.SuspendLayout();
      this.SuspendLayout();
      this.groupBoxServer.Controls.Add((Control) this.radioNet);
      this.groupBoxServer.Controls.Add((Control) this.radioTel);
      this.groupBoxServer.FlatStyle = FlatStyle.Popup;
      this.groupBoxServer.Location = new Point(12, 12);
      this.groupBoxServer.Name = "groupBoxServer";
      this.groupBoxServer.Size = new Size(347, 108);
      this.groupBoxServer.TabIndex = 0;
      this.groupBoxServer.TabStop = false;
      this.groupBoxServer.Text = "服务器选择";
      this.radioNet.AutoSize = true;
      this.radioNet.Location = new Point(85, 55);
      this.radioNet.Name = "radioNet";
      this.radioNet.Size = new Size(83, 16);
      this.radioNet.TabIndex = 1;
      this.radioNet.TabStop = true;
      this.radioNet.Text = "联通服务器";
      this.radioNet.UseVisualStyleBackColor = true;
      this.radioNet.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
      this.radioTel.AutoSize = true;
      this.radioTel.Location = new Point(206, 55);
      this.radioTel.Name = "radioTel";
      this.radioTel.Size = new Size(83, 16);
      this.radioTel.TabIndex = 0;
      this.radioTel.TabStop = true;
      this.radioTel.Text = "电信服务器";
      this.radioTel.UseVisualStyleBackColor = true;
      this.radioTel.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
      this.btConfirm.Location = new Point(12, 146);
      this.btConfirm.Name = "btConfirm";
      this.btConfirm.Size = new Size(75, 23);
      this.btConfirm.TabIndex = 1;
      this.btConfirm.Text = "确定";
      this.btConfirm.UseVisualStyleBackColor = true;
      this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
      this.btCancel.Location = new Point(97, 146);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new Size(75, 23);
      this.btCancel.TabIndex = 2;
      this.btCancel.Text = "取消";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.btApply.Location = new Point(187, 146);
      this.btApply.Name = "btApply";
      this.btApply.Size = new Size(75, 23);
      this.btApply.TabIndex = 3;
      this.btApply.Text = "应用";
      this.btApply.UseVisualStyleBackColor = true;
      this.btApply.Click += new EventHandler(this.btApply_Click);
      this.btSetProxy.Location = new Point(277, 146);
      this.btSetProxy.Name = "btSetProxy";
      this.btSetProxy.Size = new Size(103, 23);
      this.btSetProxy.TabIndex = 5;
      this.btSetProxy.Text = "设置代理服务器";
      this.btSetProxy.UseVisualStyleBackColor = true;
      this.btSetProxy.Click += new EventHandler(this.btSetProxy_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new Size(391, 201);
      this.Controls.Add((Control) this.btSetProxy);
      this.Controls.Add((Control) this.btApply);
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btConfirm);
      this.Controls.Add((Control) this.groupBoxServer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ServerSetE";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "网络设置";
      this.FormClosed += new FormClosedEventHandler(this.ServerSetE_FormClosed);
      this.Load += new EventHandler(this.ServerSetE_Load);
      this.groupBoxServer.ResumeLayout(false);
      this.groupBoxServer.PerformLayout();
      this.ResumeLayout(false);
    }

    private void ServerSetE_Load(object sender, EventArgs e)
    {
      this.Icon = new ServerSet(this.myHost).Icon;
      this.htConfig = this.myHost.ConfigurationInfo.getSection("Systems");
      this.LoadNetUserInfo();
      this.btApply.Enabled = false;
      this.isUpdate = false;
      this.SetText();
    }

    private void SetText()
    {
      this.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_ServerSetTitle");
      this.radioNet.Text = ServerSetPlugin.strServerNameFir;
      this.radioTel.Text = ServerSetPlugin.strServerNameSec;
      this.btConfirm.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btConfirm");
      this.btCancel.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btCancel");
      this.btApply.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btApply");
      this.btSetProxy.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btSetProxy");
      this.groupBoxServer.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_groupBoxServer");
    }

    private void LoadNetUserInfo()
    {
      if (this.htConfig == null)
        return;
      try
      {
        this.curServer = int.Parse((string) this.htConfig[(object) "CurServer"], NumberStyles.None);
      }
      catch
      {
      }
      if (this.curServer == 0)
        this.radioTel.Checked = true;
      else
        this.radioNet.Checked = true;
    }

    private void radioTel_CheckedChanged(object sender, EventArgs e)
    {
      this.btApply.Enabled = true;
      this.isUpdate = true;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void ServerSetE_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
    }

    private void btApply_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.isUpdate)
          this.updateNetUserInfo();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.btApply.Enabled = false;
    }

    private void btConfirm_Click(object sender, EventArgs e)
    {
      if (this.btApply.Enabled)
      {
        try
        {
          this.btApply_Click((object) null, (EventArgs) null);
          this.Close();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message);
        }
      }
      else
        this.Close();
      if (!this.isUpdate)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void updateNetUserInfo()
    {
      this.curServer = !this.radioNet.Checked ? 0 : 1;
      this.myHost.ConfigurationInfo.updateValue("Systems", "CurServer", string.Concat((object) this.curServer));
    }

    private void btSetProxy_Click(object sender, EventArgs e)
    {
      string str1 = string.Empty;
      if (File.Exists("Proxy.ini"))
      {
        StreamReader streamReader = new StreamReader("Proxy.ini");
        str1 = streamReader.ReadToEnd();
        streamReader.Close();
      }
      int num = (int) new SetProxyServer(ServerSetPlugin.SysResourceManager).ShowDialog();
      string str2 = string.Empty;
      if (File.Exists("Proxy.ini"))
      {
        StreamReader streamReader = new StreamReader("Proxy.ini");
        str2 = streamReader.ReadToEnd();
        streamReader.Close();
      }
      if (str1.Equals(str2))
        return;
      this.isUpdate = true;
    }
  }
}
