// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.MarketInfo
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
  public class MarketInfo : Form
  {
    private Uri uri;
    private IContainer components;
    private WebBrowser webBrowser1;

    public MarketInfo(Uri url)
    {
      this.InitializeComponent();
      this.uri = url;
    }

    private void MarketInfo_Load(object sender, EventArgs e)
    {
      this.webBrowser1.Url = this.uri;
      try
      {
        this.Icon = (Icon) ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", (System.Type) null).GetObject("Logo.ico");
      }
      catch (Exception ex)
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowser1 = new WebBrowser();
      this.SuspendLayout();
      this.webBrowser1.Dock = DockStyle.Fill;
      this.webBrowser1.Location = new Point(0, 0);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(644, 442);
      this.webBrowser1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(644, 442);
      this.Controls.Add((Control) this.webBrowser1);
      this.Name = "MarketInfo";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "市场公告";
      this.TopMost = true;
      this.Load += new EventHandler(this.MarketInfo_Load);
      this.ResumeLayout(false);
    }
  }
}
