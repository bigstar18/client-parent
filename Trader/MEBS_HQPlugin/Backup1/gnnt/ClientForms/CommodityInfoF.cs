// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.CommodityInfoF
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
  public class CommodityInfoF : Form
  {
    private IContainer components;
    private WebBrowser webBCommodityInfo;
    private static volatile CommodityInfoF instance;
    private string m_url;
    private string m_CommodityID;
    private HQForm m_hqForm;

    public string Url
    {
      get
      {
        return this.m_url;
      }
      set
      {
        this.m_url = value;
      }
    }

    public string CommodityID
    {
      get
      {
        return this.m_CommodityID;
      }
      set
      {
        this.m_CommodityID = value;
      }
    }

    private CommodityInfoF(HQForm hqForm)
    {
      this.InitializeComponent();
      this.m_hqForm = hqForm;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBCommodityInfo = new WebBrowser();
      this.SuspendLayout();
      this.webBCommodityInfo.Dock = DockStyle.Fill;
      this.webBCommodityInfo.Location = new Point(0, 0);
      this.webBCommodityInfo.MinimumSize = new Size(20, 20);
      this.webBCommodityInfo.Name = "webBCommodityInfo";
      this.webBCommodityInfo.Size = new Size(707, 601);
      this.webBCommodityInfo.TabIndex = 0;
      this.webBCommodityInfo.PreviewKeyDown += new PreviewKeyDownEventHandler(this.webBCommodityInfo_PreviewKeyDown);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(707, 601);
      this.Controls.Add((Control) this.webBCommodityInfo);
      this.Name = "CommodityInfo";
      this.Text = "CommodityInfo";
      this.Load += new EventHandler(this.CommodityInfo_Load);
      this.FormClosing += new FormClosingEventHandler(this.CommodityInfo_FormClosing);
      this.KeyDown += new KeyEventHandler(this.CommodityInfo_KeyDown);
      this.ResumeLayout(false);
    }

    public static CommodityInfoF GetInstance(HQForm hqForm)
    {
      if (CommodityInfoF.instance == null || CommodityInfoF.instance.IsDisposed)
      {
        lock (typeof (CommodityInfoF))
        {
          if (CommodityInfoF.instance != null)
          {
            if (!CommodityInfoF.instance.IsDisposed)
              goto label_6;
          }
          CommodityInfoF.instance = new CommodityInfoF(hqForm);
        }
      }
label_6:
      return CommodityInfoF.instance;
    }

    private void CommodityInfo_Load(object sender, EventArgs e)
    {
      this.Icon = SelectServer.SystemIcon;
    }

    public void setUri()
    {
      this.Text = this.m_CommodityID + " 详细信息";
      this.webBCommodityInfo.Url = new Uri(this.m_url + "?commodity_id=" + this.m_CommodityID);
    }

    private void CommodityInfo_FormClosing(object sender, FormClosingEventArgs e)
    {
      CommodityInfoF.instance = (CommodityInfoF) null;
    }

    private void webBCommodityInfo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (e.KeyData == Keys.Escape)
      {
        this.Close();
      }
      else
      {
        this.Close();
        this.m_hqForm.HQMainForm_KeyDown((object) this, new KeyEventArgs(e.KeyData));
      }
    }

    private void CommodityInfo_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        this.Close();
      }
      else
      {
        this.Close();
        this.m_hqForm.HQMainForm_KeyDown((object) this, e);
      }
    }

    public static void CommodityInfoClose()
    {
      if (CommodityInfoF.instance == null)
        return;
      CommodityInfoF.instance.Close();
    }
  }
}
