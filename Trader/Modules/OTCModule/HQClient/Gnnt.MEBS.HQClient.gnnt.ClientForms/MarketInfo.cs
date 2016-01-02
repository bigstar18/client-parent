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
				ResourceManager resourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", null);
				base.Icon = (Icon)resourceManager.GetObject("Logo.ico");
			}
			catch (Exception)
			{
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
			this.webBrowser1 = new WebBrowser();
			base.SuspendLayout();
			this.webBrowser1.Dock = DockStyle.Fill;
			this.webBrowser1.Location = new Point(0, 0);
			this.webBrowser1.MinimumSize = new Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new Size(644, 442);
			this.webBrowser1.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(644, 442);
			base.Controls.Add(this.webBrowser1);
			base.Name = "MarketInfo";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "市场公告";
			base.TopMost = true;
			base.Load += new EventHandler(this.MarketInfo_Load);
			base.ResumeLayout(false);
		}
	}
}
