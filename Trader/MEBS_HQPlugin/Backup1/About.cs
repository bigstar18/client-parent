// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.About
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using DIYForm;
using Gnnt.MEBS.HQModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient
{
  public class About : MyForm
  {
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private IContainer components;
    private Label labCompony;
    private Button btnOK;
    private WebBrowser webBrowser;

    public About(MainWindow mainWindow)
    {
      this.\u002Ector();
      this.InitializeComponent();
      this.pluginInfo = mainWindow.pluginInfo;
      this.setInfo = mainWindow.setInfo;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      ((Form) this).Close();
    }

    private void SetControlText()
    {
      this.webBrowser.Visible = true;
      ((Form) this).Icon = (Icon) this.pluginInfo.HQResourceManager.GetObject("Logo.ico");
      ((Control) this).Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_AboutTitle");
      this.labCompony.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_labCompony");
      this.btnOK.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btnOK");
      string uriString = this.pluginInfo.HTConfig[(object) "AddressAbout"].ToString();
      if (uriString.Length > 0)
      {
        this.webBrowser.Url = new Uri(uriString);
      }
      else
      {
        string str1 = string.Empty;
        string path = "about.txt";
        if (!File.Exists(path))
        {
          str1 = "暂无关于";
        }
        else
        {
          StreamReader streamReader = new StreamReader(path, Encoding.Default);
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
            str1 += str2;
          streamReader.Close();
        }
        this.webBrowser.DocumentText = str1;
        this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
      }
    }

    private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
      e.Cancel = true;
    }

    private void About_Load(object sender, EventArgs e)
    {
      this.SetControlText();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labCompony = new Label();
      this.btnOK = new Button();
      this.webBrowser = new WebBrowser();
      ((Control) this).SuspendLayout();
      this.labCompony.Anchor = AnchorStyles.Top;
      this.labCompony.AutoSize = true;
      this.labCompony.BackColor = Color.Transparent;
      this.labCompony.Font = new Font("宋体", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 134);
      this.labCompony.ForeColor = Color.DarkRed;
      this.labCompony.Location = new Point(57, 50);
      this.labCompony.Name = "labCompony";
      this.labCompony.Size = new Size(209, 19);
      this.labCompony.TabIndex = 6;
      this.labCompony.Text = "金网安泰统一交易系统";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.BackColor = Color.Transparent;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.FlatStyle = FlatStyle.Popup;
      this.btnOK.Location = new Point(215, 235);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(73, 29);
      this.btnOK.TabIndex = 14;
      this.btnOK.Text = "确定";
      this.btnOK.UseVisualStyleBackColor = false;
      this.btnOK.Click += new EventHandler(this.button1_Click);
      this.webBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.webBrowser.Location = new Point(19, 82);
      this.webBrowser.MinimumSize = new Size(20, 20);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.Size = new Size(286, 138);
      this.webBrowser.TabIndex = 17;
      this.webBrowser.TabStop = false;
      this.webBrowser.Url = new Uri("", UriKind.Relative);
      this.webBrowser.Visible = false;
      ((Form) this).AcceptButton = (IButtonControl) this.btnOK;
      ((ContainerControl) this).AutoScaleDimensions = new SizeF(6f, 12f);
      ((ContainerControl) this).AutoScaleMode = AutoScaleMode.Font;
      ((Form) this).CancelButton = (IButtonControl) this.btnOK;
      ((Form) this).ClientSize = new Size(319, 284);
      ((Control) this).Controls.Add((Control) this.webBrowser);
      ((Control) this).Controls.Add((Control) this.btnOK);
      ((Control) this).Controls.Add((Control) this.labCompony);
      ((Control) this).Name = "About";
      ((Form) this).StartPosition = FormStartPosition.CenterScreen;
      ((Control) this).Text = "关于";
      ((Form) this).Load += new EventHandler(this.About_Load);
      ((Control) this).Controls.SetChildIndex((Control) this.labCompony, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btnOK, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.webBrowser, 0);
      ((Control) this).ResumeLayout(false);
      ((Control) this).PerformLayout();
    }
  }
}
