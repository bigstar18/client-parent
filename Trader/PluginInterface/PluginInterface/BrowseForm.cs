using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace PluginInterface
{
	public class BrowseForm : Form
	{
		private IContainer components;
		private WebBrowser webBrowser1;
		public BrowseForm(Uri url)
		{
			this.InitializeComponent();
			this.webBrowser1.Url = url;
			base.Tag = url.ToString();
		}
		public void seturi(Uri url)
		{
			this.webBrowser1.Url = url;
			base.Tag = url.ToString();
		}
		public void WebRefresh()
		{
			this.webBrowser1.Refresh();
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
			this.webBrowser1.Size = new Size(556, 402);
			this.webBrowser1.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(556, 402);
			base.Controls.Add(this.webBrowser1);
			base.Name = "BrowseForm";
			this.Text = "BrowseForm";
			base.ResumeLayout(false);
		}
	}
}
