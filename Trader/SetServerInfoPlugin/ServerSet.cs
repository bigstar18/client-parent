// Decompiled with JetBrains decompiler
// Type: SetServerInfoPlugin.ServerSet
// Assembly: SetServerInfoPlugin, Version=3.0.8.0, Culture=neutral, PublicKeyToken=null
// MVID: E04F003E-2DD5-4E4F-8F62-E41AF4AB517D
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\SetServerInfoPlugin.dll

using Org.Mentalis.Network.ProxySocket;
using PluginInterface;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;

namespace SetServerInfoPlugin
{
  public class ServerSet : Form
  {
    private IContainer components;
    private TabControl tabCSetServer;
    private Button btApply;
    private Button btConfirm;
    private Button btCancel;
    private RadioButton radioTelecom;
    private RadioButton radioNetcom;
    private Label label1;
    private Button btSetProxy;
    private IPluginHost myHost;
    private Hashtable htConfig;
    private bool updateNetUser;
    private bool isUpdate;
    private int curServer;

    public ServerSet(IPluginHost myHost)
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
      ComponentResourceManager resources = new ComponentResourceManager(typeof (ServerSet));
      this.tabCSetServer = new TabControl();
      this.btApply = new Button();
      this.btConfirm = new Button();
      this.btCancel = new Button();
      this.radioTelecom = new RadioButton();
      this.radioNetcom = new RadioButton();
      this.label1 = new Label();
      this.btSetProxy = new Button();
      this.SuspendLayout();
      this.tabCSetServer.Dock = DockStyle.Top;
      this.tabCSetServer.Location = new Point(0, 0);
      this.tabCSetServer.Name = "tabCSetServer";
      this.tabCSetServer.SelectedIndex = 0;
      this.tabCSetServer.Size = new Size(640, 295);
      this.tabCSetServer.TabIndex = 0;
      this.btApply.Enabled = false;
      this.btApply.Location = new Point(571, 301);
      this.btApply.Name = "btApply";
      this.btApply.Size = new Size(66, 23);
      this.btApply.TabIndex = 1;
      this.btApply.Text = "应用&A";
      this.btApply.UseVisualStyleBackColor = true;
      this.btApply.Click += new EventHandler(this.btApply_Click);
      this.btConfirm.Location = new Point(407, 301);
      this.btConfirm.Name = "btConfirm";
      this.btConfirm.Size = new Size(66, 23);
      this.btConfirm.TabIndex = 1;
      this.btConfirm.Text = "确定";
      this.btConfirm.UseVisualStyleBackColor = true;
      this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
      this.btCancel.Location = new Point(489, 301);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new Size(66, 23);
      this.btCancel.TabIndex = 1;
      this.btCancel.Text = "取消";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.radioTelecom.AutoSize = true;
      this.radioTelecom.Location = new Point(203, 304);
      this.radioTelecom.Name = "radioTelecom";
      this.radioTelecom.Size = new Size(71, 16);
      this.radioTelecom.TabIndex = 2;
      this.radioTelecom.TabStop = true;
      this.radioTelecom.Text = "电信用户";
      this.radioTelecom.UseVisualStyleBackColor = true;
      this.radioTelecom.CheckedChanged += new EventHandler(this.radioTelecom_CheckedChanged);
      this.radioNetcom.AutoSize = true;
      this.radioNetcom.Location = new Point(121, 304);
      this.radioNetcom.Name = "radioNetcom";
      this.radioNetcom.Size = new Size(71, 16);
      this.radioNetcom.TabIndex = 2;
      this.radioNetcom.TabStop = true;
      this.radioNetcom.Text = "联通用户";
      this.radioNetcom.UseVisualStyleBackColor = true;
      this.radioNetcom.CheckedChanged += new EventHandler(this.radioNetcom_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 306);
      this.label1.Name = "label1";
      this.label1.Size = new Size(101, 12);
      this.label1.TabIndex = 3;
      this.label1.Text = "请选择您所属网络";
      this.btSetProxy.Location = new Point(291, 301);
      this.btSetProxy.Name = "btSetProxy";
      this.btSetProxy.Size = new Size(103, 23);
      this.btSetProxy.TabIndex = 4;
      this.btSetProxy.Text = "设置代理服务器";
      this.btSetProxy.UseVisualStyleBackColor = true;
      this.btSetProxy.Click += new EventHandler(this.btSetProxy_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new Size(640, 328);
      this.Controls.Add((Control) this.btSetProxy);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.radioNetcom);
      this.Controls.Add((Control) this.radioTelecom);
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btConfirm);
      this.Controls.Add((Control) this.btApply);
      this.Controls.Add((Control) this.tabCSetServer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = (Icon) resources.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ServerSet";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "网络设置";
      this.Load += new EventHandler(this.ServerSet_Load);
      this.FormClosed += new FormClosedEventHandler(this.ServerSet_FormClosed);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void ServerSet_Load(object sender, EventArgs e)
    {
      this.htConfig = this.myHost.ConfigurationInfo.getSection("Systems");
      this.SetText();
      this.LoadNetUserInfo();
      this.radioNetcom.Text = ServerSetPlugin.strServerNameFir;
      this.radioTelecom.Text = ServerSetPlugin.strServerNameSec;
      this.LoadServer();
      this.btApply.Enabled = false;
    }

    private void SetText()
    {
      this.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_ServerSetTitle");
      this.label1.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_label1");
      this.btSetProxy.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btSetProxy");
      this.btConfirm.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btConfirm");
      this.btCancel.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btCancel");
      this.btApply.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btApply");
    }

    private void LoadNetUserInfo()
    {
      if (this.htConfig == null)
        return;
      try
      {
        this.curServer = int.Parse((string) this.htConfig[(object) "CurServer"]);
      }
      catch
      {
      }
      if (this.curServer == 0)
        this.radioTelecom.Checked = true;
      else
        this.radioNetcom.Checked = true;
    }

    private void updateNetUserInfo()
    {
      this.myHost.ConfigurationInfo.updateValue("Systems", "CurServer", string.Concat((object) this.curServer));
    }

    private void LoadServer()
    {
      this.populateTabControl(this.tabCSetServer);
    }

    private void populateTabControl(TabControl tabCSetServer)
    {
      foreach (DictionaryEntry dictionaryEntry in this.myHost.HtConfigInfo)
      {
        try
        {
          string str1 = (string) dictionaryEntry.Key;
          PluginConfigInfo pluginConfigInfo = (PluginConfigInfo) dictionaryEntry.Value;
          string str2 = (string) pluginConfigInfo.XmlPath;
          XmlDocument xmlDocument = (XmlDocument) pluginConfigInfo.XmlDoc;
          if (xmlDocument != null)
          {
            string s1 = string.Empty;
            string s2 = string.Empty;
            string str3 = string.Empty;
            XmlElement xmlElement = (XmlElement) xmlDocument.SelectSingleNode("ConfigInfo");
            if (xmlElement.SelectSingleNode("Enable") != null)
              s1 = xmlElement.SelectSingleNode("Enable").InnerText;
            if (xmlElement.SelectSingleNode("Text") != null)
              str3 = xmlElement.SelectSingleNode("Text").InnerText;
            if (xmlElement.SelectSingleNode("ServerType") != null)
              s2 = xmlElement.SelectSingleNode("ServerType").InnerText;
            if (Tools.StrToBool(s1))
            {
              int serverType = Tools.StrToInt(s2);
              switch (serverType)
              {
                case 0:
                case 1:
                  TabPage tabPage = new TabPage();
                  tabPage.Name = str1;
                  tabPage.Text = str3;
                  tabPage.AutoScroll = true;
                  SetServerInfoC setServerInfoC = new SetServerInfoC(serverType);
                  setServerInfoC.CurServer = this.curServer;
                  setServerInfoC.XmlPath = str2;
                  setServerInfoC.XmlDoc = xmlDocument;
                  setServerInfoC.Initialize();
                  setServerInfoC.Dock = DockStyle.Fill;
                  setServerInfoC.dataChange += new SetServerInfoC.dataChangeEventHander(this.c_dataChange);
                  tabPage.Controls.Add((Control) setServerInfoC);
                  tabCSetServer.Controls.Add((Control) tabPage);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        catch (Exception ex)
        {
          Logger.wirte(ex);
        }
      }
    }

    private void btApply_Click(object sender, EventArgs e)
    {
      try
      {
        this.tabCSetServer.Enabled = false;
        if (this.updateNetUser)
        {
          this.updateNetUserInfo();
          this.isUpdate = true;
        }
        for (int index = 0; index < this.tabCSetServer.TabPages.Count; ++index)
        {
          foreach (Control control in (ArrangedElementCollection) this.tabCSetServer.TabPages[index].Controls)
          {
            if (control is SetServerInfoC)
            {
              SetServerInfoC setServerInfoC = (SetServerInfoC) control;
              if (setServerInfoC.IsUpdateServer)
              {
                this.isUpdate = true;
                setServerInfoC.updateServerInfo();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.tabCSetServer.Enabled = true;
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

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void ServerSet_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
    }

    private void radioTelecom_CheckedChanged(object sender, EventArgs e)
    {
      this.updateNetUser = true;
      this.curServer = 0;
      for (int index = 0; index < this.tabCSetServer.TabPages.Count; ++index)
      {
        foreach (Control control1 in (ArrangedElementCollection) this.tabCSetServer.TabPages[index].Controls)
        {
          if (control1 is SetServerInfoC)
          {
            foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
            {
              if (control2 is GroupBox)
                control2.Enabled = control2.Name.Equals("CurTelecomServer");
            }
          }
        }
      }
      this.btApply.Enabled = true;
    }

    private void radioNetcom_CheckedChanged(object sender, EventArgs e)
    {
      this.updateNetUser = true;
      this.curServer = 1;
      for (int index = 0; index < this.tabCSetServer.TabPages.Count; ++index)
      {
        foreach (Control control1 in (ArrangedElementCollection) this.tabCSetServer.TabPages[index].Controls)
        {
          if (control1 is SetServerInfoC)
          {
            foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
            {
              if (control2 is GroupBox)
                control2.Enabled = control2.Name.Equals("CurNetcomServer");
            }
          }
        }
      }
      this.btApply.Enabled = true;
    }

    private void c_dataChange(object sender, EventArgs e)
    {
      this.btApply.Enabled = true;
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
