// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.ServerSet
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using DIYForm;
using Gnnt.MEBS.HQModel;
using Org.Mentalis.Network.ProxySocket;
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

namespace Gnnt.MEBS.HQClient
{
  public class ServerSet : MyForm
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
    public PluginInfo pluginInfo;
    public SetInfo setInfo;
    private Hashtable htConfig;
    private XmlElement xnConfigInfo;
    private XmlDocument xmlDoc;
    private bool updateNetUser;
    private bool isUpdate;
    private Program.SET SET;
    private int curServer;

    public ServerSet(MainWindow mainWindow)
    {
      this.\u002Ector();
      this.InitializeComponent();
      this.pluginInfo = mainWindow.pluginInfo;
      this.setInfo = mainWindow.setInfo;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ServerSet));
      this.tabCSetServer = new TabControl();
      this.btApply = new Button();
      this.btConfirm = new Button();
      this.btCancel = new Button();
      this.radioTelecom = new RadioButton();
      this.radioNetcom = new RadioButton();
      this.label1 = new Label();
      this.btSetProxy = new Button();
      ((Control) this).SuspendLayout();
      this.tabCSetServer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabCSetServer.Location = new Point(7, 27);
      this.tabCSetServer.Name = "tabCSetServer";
      this.tabCSetServer.SelectedIndex = 0;
      this.tabCSetServer.Size = new Size(633, 300);
      this.tabCSetServer.TabIndex = 0;
      this.btApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btApply.BackColor = Color.Transparent;
      this.btApply.Enabled = false;
      this.btApply.Location = new Point(568, 327);
      this.btApply.Name = "btApply";
      this.btApply.Size = new Size(66, 23);
      this.btApply.TabIndex = 1;
      this.btApply.Text = "应用&A";
      this.btApply.UseVisualStyleBackColor = false;
      this.btApply.Click += new EventHandler(this.btApply_Click);
      this.btConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btConfirm.BackColor = Color.Transparent;
      this.btConfirm.Location = new Point(407, 327);
      this.btConfirm.Name = "btConfirm";
      this.btConfirm.Size = new Size(66, 23);
      this.btConfirm.TabIndex = 1;
      this.btConfirm.Text = "确定";
      this.btConfirm.UseVisualStyleBackColor = false;
      this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
      this.btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btCancel.BackColor = Color.Transparent;
      this.btCancel.Location = new Point(489, 327);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new Size(66, 23);
      this.btCancel.TabIndex = 1;
      this.btCancel.Text = "取消";
      this.btCancel.UseVisualStyleBackColor = false;
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.radioTelecom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.radioTelecom.AutoSize = true;
      this.radioTelecom.BackColor = Color.Transparent;
      this.radioTelecom.Location = new Point(203, 330);
      this.radioTelecom.Name = "radioTelecom";
      this.radioTelecom.Size = new Size(71, 16);
      this.radioTelecom.TabIndex = 2;
      this.radioTelecom.TabStop = true;
      this.radioTelecom.Text = "电信用户";
      this.radioTelecom.UseVisualStyleBackColor = false;
      this.radioTelecom.CheckedChanged += new EventHandler(this.radioTelecom_CheckedChanged);
      this.radioNetcom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.radioNetcom.AutoSize = true;
      this.radioNetcom.BackColor = Color.Transparent;
      this.radioNetcom.Location = new Point(121, 330);
      this.radioNetcom.Name = "radioNetcom";
      this.radioNetcom.Size = new Size(71, 16);
      this.radioNetcom.TabIndex = 2;
      this.radioNetcom.TabStop = true;
      this.radioNetcom.Text = "联通用户";
      this.radioNetcom.UseVisualStyleBackColor = false;
      this.radioNetcom.CheckedChanged += new EventHandler(this.radioNetcom_CheckedChanged);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(12, 332);
      this.label1.Name = "label1";
      this.label1.Size = new Size(101, 12);
      this.label1.TabIndex = 3;
      this.label1.Text = "请选择您所属网络";
      this.btSetProxy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btSetProxy.BackColor = Color.Transparent;
      this.btSetProxy.Location = new Point(291, 327);
      this.btSetProxy.Name = "btSetProxy";
      this.btSetProxy.Size = new Size(103, 23);
      this.btSetProxy.TabIndex = 4;
      this.btSetProxy.Text = "设置代理服务器";
      this.btSetProxy.UseVisualStyleBackColor = false;
      this.btSetProxy.Click += new EventHandler(this.btSetProxy_Click);
      ((ContainerControl) this).AutoScaleDimensions = new SizeF(6f, 12f);
      ((ContainerControl) this).AutoScaleMode = AutoScaleMode.Font;
      ((Form) this).ClientSize = new Size(644, 358);
      ((Control) this).Controls.Add((Control) this.btSetProxy);
      ((Control) this).Controls.Add((Control) this.label1);
      ((Control) this).Controls.Add((Control) this.radioNetcom);
      ((Control) this).Controls.Add((Control) this.radioTelecom);
      ((Control) this).Controls.Add((Control) this.btCancel);
      ((Control) this).Controls.Add((Control) this.btConfirm);
      ((Control) this).Controls.Add((Control) this.btApply);
      ((Control) this).Controls.Add((Control) this.tabCSetServer);
      ((Form) this).Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      ((Form) this).MaximizeBox = false;
      ((Form) this).MinimizeBox = false;
      ((Control) this).Name = "ServerSet";
      ((Form) this).StartPosition = FormStartPosition.CenterScreen;
      ((Control) this).Text = "网络设置";
      ((Form) this).FormClosed += new FormClosedEventHandler(this.ServerSet_FormClosed);
      ((Form) this).Load += new EventHandler(this.ServerSet_Load);
      ((Control) this).Controls.SetChildIndex((Control) this.tabCSetServer, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btApply, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btConfirm, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btCancel, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.radioTelecom, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.radioNetcom, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.label1, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btSetProxy, 0);
      ((Control) this).ResumeLayout(false);
      ((Control) this).PerformLayout();
    }

    private void ServerSet_Load(object sender, EventArgs e)
    {
      this.htConfig = this.pluginInfo.HTConfig;
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.Load(this.SET.myConfigFileName);
      this.xnConfigInfo = (XmlElement) this.xmlDoc.SelectSingleNode("ConfigInfo");
      this.LoadNetUserInfo();
      this.LoadServer();
      this.btApply.Enabled = false;
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
      this.xnConfigInfo.SelectSingleNode("ServerType").InnerText = this.curServer.ToString();
      this.xmlDoc.Save(this.SET.myConfigFileName);
    }

    private void LoadServer()
    {
      this.populateTabControl(this.tabCSetServer);
    }

    private void populateTabControl(TabControl tabCSetServer)
    {
      try
      {
        if (this.xmlDoc == null)
          return;
        string s1 = string.Empty;
        string s2 = string.Empty;
        string str = string.Empty;
        XmlElement xmlElement = (XmlElement) this.xmlDoc.SelectSingleNode("ConfigInfo");
        if (xmlElement.SelectSingleNode("Enable") != null)
          s1 = xmlElement.SelectSingleNode("Enable").InnerText;
        if (xmlElement.SelectSingleNode("Text") != null)
          str = xmlElement.SelectSingleNode("Text").InnerText;
        if (xmlElement.SelectSingleNode("ServerType") != null)
          s2 = xmlElement.SelectSingleNode("ServerType").InnerText;
        if (!Tools.StrToBool(s1))
          return;
        int serverType = Tools.StrToInt(s2);
        switch (serverType)
        {
          case 0:
          case 1:
            TabPage tabPage = new TabPage();
            tabPage.Name = xmlElement.SelectSingleNode("Text").InnerText;
            tabPage.Text = str;
            tabPage.AutoScroll = true;
            SetServerInfoC setServerInfoC = new SetServerInfoC(serverType);
            setServerInfoC.CurServer = this.curServer;
            setServerInfoC.XmlPath = this.SET.myConfigFileName;
            setServerInfoC.XmlDoc = this.xmlDoc;
            setServerInfoC.Initialize();
            setServerInfoC.Dock = DockStyle.Fill;
            setServerInfoC.dataChange += new SetServerInfoC.dataChangeEventHander(this.c_dataChange);
            tabPage.Controls.Add((Control) setServerInfoC);
            tabCSetServer.Controls.Add((Control) tabPage);
            break;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.Message);
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
          ((Form) this).Close();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message);
        }
      }
      else
        ((Form) this).Close();
      if (!this.isUpdate)
        return;
      ((Form) this).DialogResult = DialogResult.OK;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      ((Form) this).DialogResult = DialogResult.Cancel;
      ((Form) this).Close();
    }

    private void ServerSet_FormClosed(object sender, FormClosedEventArgs e)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (((Component) this).Dispose());
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
      int num = (int) new SetProxyServer(this.pluginInfo.HQResourceManager).ShowDialog();
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
