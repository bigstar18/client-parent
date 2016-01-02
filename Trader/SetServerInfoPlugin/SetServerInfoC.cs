// Decompiled with JetBrains decompiler
// Type: SetServerInfoPlugin.SetServerInfoC
// Assembly: SetServerInfoPlugin, Version=3.0.8.0, Culture=neutral, PublicKeyToken=null
// MVID: E04F003E-2DD5-4E4F-8F62-E41AF4AB517D
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\SetServerInfoPlugin.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using ToolsLibrary.util;

namespace SetServerInfoPlugin
{
  public class SetServerInfoC : UserControl
  {
    private string strText = string.Empty;
    private string strCurTelecomServer = string.Empty;
    private string strCurNetcomServer = string.Empty;
    private ArrayList telecomServerList = new ArrayList();
    private ArrayList netcomServerList = new ArrayList();
    private int serverType;
    private string myXmlPath;
    private XmlDocument myXmlDoc;
    private int myCurServer;
    private bool isUpdateServer;
    private IContainer components;

    public string XmlPath
    {
      get
      {
        return this.myXmlPath;
      }
      set
      {
        this.myXmlPath = value;
      }
    }

    public XmlDocument XmlDoc
    {
      get
      {
        return this.myXmlDoc;
      }
      set
      {
        this.myXmlDoc = value;
      }
    }

    public int CurServer
    {
      get
      {
        return this.myCurServer;
      }
      set
      {
        this.myCurServer = value;
      }
    }

    public bool IsUpdateServer
    {
      get
      {
        return this.isUpdateServer;
      }
    }

    public event SetServerInfoC.dataChangeEventHander dataChange;

    public SetServerInfoC(int serverType)
    {
      this.InitializeComponent();
      this.serverType = serverType;
    }

    private void LoadData()
    {
      XmlNode xmlNode1 = this.myXmlDoc.SelectSingleNode("ConfigInfo");
      if (xmlNode1.SelectSingleNode("Text") != null)
        this.strText = xmlNode1.SelectSingleNode("Text").InnerText;
      if (xmlNode1.SelectSingleNode("CurTelecomServer") != null)
        this.strCurTelecomServer = xmlNode1.SelectSingleNode("CurTelecomServer").InnerText;
      if (xmlNode1.SelectSingleNode("CurNetcomServer") != null)
        this.strCurNetcomServer = xmlNode1.SelectSingleNode("CurNetcomServer").InnerText;
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode("AllTelecomServer");
      if (xmlNode2 != null)
      {
        foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
        {
          MoudelServerInfo moudelServerInfo = new MoudelServerInfo();
          XmlElement xmlElement = (XmlElement) xmlNode3;
          if (xmlElement.SelectSingleNode("ServerName") != null)
            moudelServerInfo.ServerName = xmlElement.SelectSingleNode("ServerName").InnerText;
          if (xmlElement.SelectSingleNode("IPAddress") != null)
            moudelServerInfo.IP_Address = xmlElement.SelectSingleNode("IPAddress").InnerText;
          if (xmlElement.SelectSingleNode("Port") != null)
            moudelServerInfo.Port = Tools.StrToInt(xmlElement.SelectSingleNode("Port").InnerText);
          if (xmlElement.SelectSingleNode("HttpPort") != null)
            moudelServerInfo.HttpPort = Tools.StrToInt(xmlElement.SelectSingleNode("HttpPort").InnerText);
          this.telecomServerList.Add((object) moudelServerInfo);
        }
      }
      XmlNode xmlNode4 = xmlNode1.SelectSingleNode("AllNetcomServer");
      if (xmlNode2 == null)
        return;
      foreach (XmlNode xmlNode3 in xmlNode4.ChildNodes)
      {
        MoudelServerInfo moudelServerInfo = new MoudelServerInfo();
        XmlElement xmlElement = (XmlElement) xmlNode3;
        if (xmlElement.SelectSingleNode("ServerName") != null)
          moudelServerInfo.ServerName = xmlElement.SelectSingleNode("ServerName").InnerText;
        if (xmlElement.SelectSingleNode("IPAddress") != null)
          moudelServerInfo.IP_Address = xmlElement.SelectSingleNode("IPAddress").InnerText;
        if (xmlElement.SelectSingleNode("Port") != null)
          moudelServerInfo.Port = Tools.StrToInt(xmlElement.SelectSingleNode("Port").InnerText);
        if (xmlElement.SelectSingleNode("HttpPort") != null)
          moudelServerInfo.HttpPort = Tools.StrToInt(xmlElement.SelectSingleNode("HttpPort").InnerText);
        this.netcomServerList.Add((object) moudelServerInfo);
      }
    }

    public void Initialize()
    {
      this.LoadData();
      int x1 = 20;
      int num1 = 5;
      GroupBox groupBox1 = new GroupBox();
      groupBox1.Text = ServerSetPlugin.strServerNameSec;
      groupBox1.Size = new Size(this.Width - 16, this.telecomServerList.Count * 30 + 80);
      groupBox1.Dock = DockStyle.Top;
      groupBox1.Name = "CurTelecomServer";
      this.Controls.Add((Control) groupBox1);
      if (this.myCurServer != 0)
        groupBox1.Enabled = false;
      int y1 = num1 + 30;
      RadioButton radioButton1 = new RadioButton();
      string string1 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_AutoSelect");
      radioButton1.Text = string1;
      radioButton1.AutoSize = true;
      radioButton1.Location = new Point(x1, y1);
      radioButton1.Name = "rbServer-1";
      if (Tools.StrToInt(this.strCurTelecomServer) == -1)
        radioButton1.Checked = true;
      radioButton1.Click += new EventHandler(this.hqControl_Click);
      groupBox1.Controls.Add((Control) radioButton1);
      int num2;
      for (int index = 0; index < this.telecomServerList.Count; ++index)
      {
        int x2 = 20;
        int y2 = (index + 2) * 30 + 5;
        MoudelServerInfo moudelServerInfo = (MoudelServerInfo) this.telecomServerList[index];
        RadioButton radioButton2 = new RadioButton();
        radioButton2.Text = moudelServerInfo.ServerName;
        radioButton2.AutoSize = true;
        radioButton2.Location = new Point(x2, y2);
        radioButton2.Name = "rbServer" + (object) index;
        if (Tools.StrToInt(this.strCurTelecomServer) == index)
          radioButton2.Checked = true;
        radioButton2.Click += new EventHandler(this.hqControl_Click);
        groupBox1.Controls.Add((Control) radioButton2);
        int x3 = x2 + radioButton2.Width;
        Label label1 = new Label();
        label1.Location = new Point(x3, y2);
        string string2 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_IpAddress");
        label1.Text = string2 + " ";
        label1.Name = "lbIP" + moudelServerInfo.ServerName;
        label1.AutoSize = true;
        groupBox1.Controls.Add((Control) label1);
        int x4 = x3 + label1.Width;
        TextBox textBox1 = new TextBox();
        textBox1.Location = new Point(x4, y2);
        textBox1.Text = moudelServerInfo.IP_Address;
        textBox1.Name = "tbIP" + moudelServerInfo.ServerName;
        textBox1.TextChanged += new EventHandler(this.hqControl_Click);
        groupBox1.Controls.Add((Control) textBox1);
        int x5 = x4 + textBox1.Width;
        Label label2 = new Label();
        label2.Location = new Point(x5, y2);
        string string3 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_StrPort");
        label2.Text = " " + string3 + " ";
        label2.Name = "lbPort" + moudelServerInfo.ServerName;
        label2.AutoSize = true;
        groupBox1.Controls.Add((Control) label2);
        int x6 = x5 + label2.Width;
        TextBox textBox2 = new TextBox();
        textBox2.Location = new Point(x6, y2);
        textBox2.MaxLength = 10;
        textBox2.Text = moudelServerInfo.Port.ToString();
        textBox2.Name = "tbPort" + moudelServerInfo.ServerName;
        textBox2.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
        textBox2.TextChanged += new EventHandler(this.hqControl_Click);
        groupBox1.Controls.Add((Control) textBox2);
        int x7 = x6 + textBox2.Width;
        if (this.serverType == 1)
        {
          Label label3 = new Label();
          label3.Location = new Point(x7, y2);
          string string4 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_HttpPort");
          label3.Text = string4 + " ";
          label3.Name = "lbHttpPort" + moudelServerInfo.ServerName;
          label3.AutoSize = true;
          groupBox1.Controls.Add((Control) label3);
          int x8 = x7 + label3.Width;
          TextBox textBox3 = new TextBox();
          textBox3.Location = new Point(x8, y2);
          textBox3.MaxLength = 10;
          textBox3.Text = moudelServerInfo.HttpPort.ToString();
          textBox3.Name = "tbHttpPort" + moudelServerInfo.ServerName;
          textBox3.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
          textBox3.TextChanged += new EventHandler(this.hqControl_Click);
          groupBox1.Controls.Add((Control) textBox3);
          num2 = x8 + textBox3.Width;
          if (!ServerSetPlugin.displayIP)
          {
            label3.Visible = false;
            textBox3.Visible = false;
          }
        }
        if (!ServerSetPlugin.displayIP)
        {
          label1.Visible = false;
          textBox1.Visible = false;
          label2.Visible = false;
          textBox2.Visible = false;
        }
      }
      int x9 = 20;
      int num3 = 5;
      GroupBox groupBox2 = new GroupBox();
      groupBox2.Text = ServerSetPlugin.strServerNameFir;
      groupBox2.Size = new Size(this.Width - 16, this.netcomServerList.Count * 30 + 80);
      groupBox2.Dock = DockStyle.Top;
      groupBox2.Name = "CurNetcomServer";
      this.Controls.Add((Control) groupBox2);
      if (this.myCurServer != 1)
        groupBox2.Enabled = false;
      int y3 = num3 + 30;
      RadioButton radioButton3 = new RadioButton();
      radioButton3.Text = string1;
      radioButton3.AutoSize = true;
      radioButton3.Location = new Point(x9, y3);
      radioButton3.Name = "rbServer-1";
      if (Tools.StrToInt(this.strCurNetcomServer) == -1)
        radioButton3.Checked = true;
      radioButton3.Click += new EventHandler(this.hqControl_Click);
      groupBox2.Controls.Add((Control) radioButton3);
      for (int index = 0; index < this.netcomServerList.Count; ++index)
      {
        int x2 = 20;
        int y2 = (index + 2) * 30 + 5;
        MoudelServerInfo moudelServerInfo = (MoudelServerInfo) this.netcomServerList[index];
        RadioButton radioButton2 = new RadioButton();
        radioButton2.Text = moudelServerInfo.ServerName;
        radioButton2.AutoSize = true;
        radioButton2.Location = new Point(x2, y2);
        radioButton2.Name = "rbServer" + (object) index;
        if (Tools.StrToInt(this.strCurNetcomServer) == index)
          radioButton2.Checked = true;
        radioButton2.Click += new EventHandler(this.hqControl_Click);
        groupBox2.Controls.Add((Control) radioButton2);
        int x3 = x2 + radioButton2.Width;
        Label label1 = new Label();
        label1.Location = new Point(x3, y2);
        string string2 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_IpAddress");
        label1.Text = string2 + " ";
        label1.Name = "lbIP" + moudelServerInfo.ServerName;
        label1.AutoSize = true;
        groupBox2.Controls.Add((Control) label1);
        int x4 = x3 + label1.Width;
        TextBox textBox1 = new TextBox();
        textBox1.Location = new Point(x4, y2);
        textBox1.Text = moudelServerInfo.IP_Address;
        textBox1.Name = "tbIP" + moudelServerInfo.ServerName;
        textBox1.TextChanged += new EventHandler(this.hqControl_Click);
        groupBox2.Controls.Add((Control) textBox1);
        int x5 = x4 + textBox1.Width;
        Label label2 = new Label();
        label2.Location = new Point(x5, y2);
        string string3 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_StrPort");
        label2.Text = " " + string3 + " ";
        label2.Name = "lbPort" + moudelServerInfo.ServerName;
        label2.AutoSize = true;
        groupBox2.Controls.Add((Control) label2);
        int x6 = x5 + label2.Width;
        TextBox textBox2 = new TextBox();
        textBox2.Location = new Point(x6, y2);
        textBox2.MaxLength = 10;
        textBox2.Text = moudelServerInfo.Port.ToString();
        textBox2.Name = "tbPort" + moudelServerInfo.ServerName;
        textBox2.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
        textBox2.TextChanged += new EventHandler(this.hqControl_Click);
        groupBox2.Controls.Add((Control) textBox2);
        int x7 = x6 + textBox2.Width;
        if (this.serverType == 1)
        {
          Label label3 = new Label();
          label3.Location = new Point(x7, y2);
          string string4 = ServerSetPlugin.SysResourceManager.GetString("PluginStr_HttpPort");
          label3.Text = string4;
          label3.Name = "lbHttpPort" + moudelServerInfo.ServerName;
          label3.AutoSize = true;
          groupBox2.Controls.Add((Control) label3);
          int x8 = x7 + label3.Width;
          TextBox textBox3 = new TextBox();
          textBox3.Location = new Point(x8, y2);
          textBox3.MaxLength = 10;
          textBox3.Text = moudelServerInfo.HttpPort.ToString();
          textBox3.Name = "tbHttpPort" + moudelServerInfo.ServerName;
          textBox3.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
          textBox3.TextChanged += new EventHandler(this.hqControl_Click);
          groupBox2.Controls.Add((Control) textBox3);
          num2 = x8 + textBox3.Width;
          if (!ServerSetPlugin.displayIP)
          {
            label3.Visible = false;
            textBox3.Visible = false;
          }
        }
        if (!ServerSetPlugin.displayIP)
        {
          label1.Visible = false;
          textBox1.Visible = false;
          label2.Visible = false;
          textBox2.Visible = false;
        }
      }
    }

    private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.Text.Length != 0 && textBox.SelectedText.Length != textBox.Text.Length || (int) e.KeyChar != 48)
        return;
      int num = (int) MessageBox.Show(ServerSetPlugin.SysResourceManager.GetString("PluginStr_PortNoZero"));
      e.Handled = true;
    }

    private void hqControl_Click(object sender, EventArgs e)
    {
      this.isUpdateServer = true;
      if (this.dataChange == null)
        return;
      this.dataChange((object) this, e);
    }

    public void updateServerInfo()
    {
      foreach (Control control1 in (ArrangedElementCollection) this.Controls)
      {
        if (control1 is GroupBox)
        {
          foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (control2 is RadioButton)
            {
              if (((RadioButton) control2).Checked)
                this.UpdateConfigInfo(control1.Name, control2.Name.Substring(8));
            }
            else if (control2.Name.StartsWith("tbIP") && control2 is TextBox)
            {
              TextBox textBox = (TextBox) control2;
              if (textBox.Text.Length == 0)
                throw new Exception("插件:" + this.strText + " 服务器：" + control2.Name.Substring(4) + "  IP地址为空");
              this.UpdateConfigInfo(control2.Name.Substring(4), "IPAddress", textBox.Text);
            }
            else if (control2.Name.StartsWith("tbPort") && control2 is TextBox)
            {
              TextBox textBox = (TextBox) control2;
              if (textBox.Text.Length == 0)
                throw new Exception("插件:" + this.strText + " 服务器：" + control2.Name.Substring(6) + "  端口为空");
              this.UpdateConfigInfo(control2.Name.Substring(6), "Port", textBox.Text);
            }
            else if (control2.Name.StartsWith("tbHttpPort") && control2 is TextBox)
            {
              TextBox textBox = (TextBox) control2;
              if (textBox.Text.Length == 0)
                throw new Exception("插件:" + this.strText + " 服务器：" + control2.Name.Substring(10) + "  Http端口为空");
              this.UpdateConfigInfo(control2.Name.Substring(10), "HttpPort", textBox.Text);
            }
          }
        }
      }
    }

    public bool UpdateConfigInfo(string key, string value)
    {
      bool flag = false;
      if (key == null || key.Length == 0 || (value == null || value.Length == 0))
        return false;
      XmlNode xmlNode = this.myXmlDoc.SelectSingleNode("ConfigInfo");
      if (xmlNode.SelectSingleNode(key) != null)
      {
        xmlNode.SelectSingleNode(key).InnerText = value.Trim();
        this.myXmlDoc.Save(this.myXmlPath + this.myXmlDoc.BaseURI.Substring(this.myXmlDoc.BaseURI.LastIndexOf("/")));
        flag = true;
      }
      return flag;
    }

    public bool UpdateConfigInfo(string serverName, string key, string value)
    {
      bool flag = false;
      if (key == null || key.Length == 0 || (value == null || value.Length == 0))
        return false;
      XmlNode xmlNode1 = this.myXmlDoc.SelectSingleNode("ConfigInfo");
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode("AllTelecomServer");
      if (xmlNode2 != null)
      {
        foreach (XmlElement xmlElement in xmlNode2.ChildNodes)
        {
          if (xmlElement.SelectSingleNode("ServerName") != null && xmlElement.SelectSingleNode("ServerName").InnerText.Equals(serverName) && xmlElement.SelectSingleNode(key) != null)
          {
            xmlElement.SelectSingleNode(key).InnerText = value.Trim();
            this.myXmlDoc.Save(this.myXmlPath + this.myXmlDoc.BaseURI.Substring(this.myXmlDoc.BaseURI.LastIndexOf("/")));
            flag = true;
            return flag;
          }
        }
      }
      XmlNode xmlNode3 = xmlNode1.SelectSingleNode("AllNetcomServer");
      if (xmlNode3 != null)
      {
        foreach (XmlElement xmlElement in xmlNode3.ChildNodes)
        {
          if (xmlElement.SelectSingleNode("ServerName") != null && xmlElement.SelectSingleNode("ServerName").InnerText.Equals(serverName) && xmlElement.SelectSingleNode(key) != null)
          {
            xmlElement.SelectSingleNode(key).InnerText = value.Trim();
            this.myXmlDoc.Save(this.myXmlPath + this.myXmlDoc.BaseURI.Substring(this.myXmlDoc.BaseURI.LastIndexOf("/")));
            flag = true;
            return flag;
          }
        }
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = "SetServerInfoC";
      this.Size = new Size(664, 325);
      this.ResumeLayout(false);
    }

    public delegate void dataChangeEventHander(object sender, EventArgs e);
  }
}
