using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeClientApp.Gnnt.ISSUE.Library;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class FundsTransfer : Form
	{
		private string m_url = string.Empty;
		private IContainer components;
		private WebBrowser webBFundsTransfer;
		public FundsTransfer(string url)
		{
			this.InitializeComponent();
			this.m_url = url;
		}
		private void FundsTransfer_Load(object sender, EventArgs e)
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			Uri url = new Uri(this.m_url + "?uid=" + Global.UserID);
			this.webBFundsTransfer.Url = url;
			ScaleForm.ScaleForms(this);
		}
		private void FundsTransfer_FormClosing(object sender, FormClosingEventArgs e)
		{
			Global.HtForm.Remove("fundsTransfer");
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
			this.webBFundsTransfer = new WebBrowser();
			base.SuspendLayout();
			this.webBFundsTransfer.Dock = DockStyle.Fill;
			this.webBFundsTransfer.Location = new Point(0, 0);
			this.webBFundsTransfer.MinimumSize = new Size(20, 20);
			this.webBFundsTransfer.Name = "webBFundsTransfer";
			this.webBFundsTransfer.Size = new Size(679, 387);
			this.webBFundsTransfer.TabIndex = 0;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(679, 387);
			base.Controls.Add(this.webBFundsTransfer);
			base.Name = "FundsTransfer";
			base.TopMost = true;
			base.FormClosing += new FormClosingEventHandler(this.FundsTransfer_FormClosing);
			base.Load += new EventHandler(this.FundsTransfer_Load);
			base.ResumeLayout(false);
		}
	}
}
