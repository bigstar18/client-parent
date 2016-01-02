using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
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
			this.webBCommodityInfo = new WebBrowser();
			base.SuspendLayout();
			this.webBCommodityInfo.Dock = DockStyle.Fill;
			this.webBCommodityInfo.Location = new Point(0, 0);
			this.webBCommodityInfo.MinimumSize = new Size(20, 20);
			this.webBCommodityInfo.Name = "webBCommodityInfo";
			this.webBCommodityInfo.Size = new Size(707, 601);
			this.webBCommodityInfo.TabIndex = 0;
			this.webBCommodityInfo.PreviewKeyDown += new PreviewKeyDownEventHandler(this.webBCommodityInfo_PreviewKeyDown);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(707, 601);
			base.Controls.Add(this.webBCommodityInfo);
			base.Name = "CommodityInfo";
			this.Text = "CommodityInfo";
			base.Load += new EventHandler(this.CommodityInfo_Load);
			base.FormClosing += new FormClosingEventHandler(this.CommodityInfo_FormClosing);
			base.KeyDown += new KeyEventHandler(this.CommodityInfo_KeyDown);
			base.ResumeLayout(false);
		}
		public static CommodityInfoF GetInstance(HQForm hqForm)
		{
			if (CommodityInfoF.instance == null || CommodityInfoF.instance.IsDisposed)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(CommodityInfoF));
				try
				{
					if (CommodityInfoF.instance == null || CommodityInfoF.instance.IsDisposed)
					{
						CommodityInfoF.instance = new CommodityInfoF(hqForm);
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return CommodityInfoF.instance;
		}
		private CommodityInfoF(HQForm hqForm)
		{
			this.InitializeComponent();
			this.m_hqForm = hqForm;
		}
		private void CommodityInfo_Load(object sender, EventArgs e)
		{
			base.Icon = SelectServer.SystemIcon;
		}
		public void setUri()
		{
			this.Text = this.m_CommodityID + " 详细信息";
			this.webBCommodityInfo.Url = new Uri(this.m_url + "?commodity_id=" + this.m_CommodityID);
		}
		private void CommodityInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			CommodityInfoF.instance = null;
		}
		private void webBCommodityInfo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				base.Close();
				return;
			}
			base.Close();
			KeyEventArgs e2 = new KeyEventArgs(e.KeyData);
			this.m_hqForm.HQMainForm_KeyDown(this, e2);
		}
		private void CommodityInfo_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
				return;
			}
			base.Close();
			this.m_hqForm.HQMainForm_KeyDown(this, e);
		}
		public static void CommodityInfoClose()
		{
			if (CommodityInfoF.instance != null)
			{
				CommodityInfoF.instance.Close();
			}
		}
	}
}
