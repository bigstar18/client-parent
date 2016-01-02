using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeClientApp.Gnnt.ISSUE.Library;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class ConditionOrder : Form
	{
		private IContainer components;
		private WebBrowser ConditionOrderWB;
		private string url = string.Empty;
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
			this.ConditionOrderWB = new WebBrowser();
			base.SuspendLayout();
			this.ConditionOrderWB.Dock = DockStyle.Fill;
			this.ConditionOrderWB.Location = new Point(0, 0);
			this.ConditionOrderWB.MinimumSize = new Size(20, 20);
			this.ConditionOrderWB.Name = "ConditionOrderWB";
			this.ConditionOrderWB.Size = new Size(843, 487);
			this.ConditionOrderWB.TabIndex = 0;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(843, 487);
			base.Controls.Add(this.ConditionOrderWB);
			base.Name = "ConditionOrder";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "条件下单";
			base.FormClosing += new FormClosingEventHandler(this.ConditionOrder_FormClosing);
			base.Load += new EventHandler(this.ConditionOrder_Load);
			base.ResumeLayout(false);
		}
		public ConditionOrder(string url)
		{
			this.InitializeComponent();
			this.url = url;
		}
		private void ConditionOrder_Load(object sender, EventArgs e)
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			Uri uri = new Uri(this.url);
			this.ConditionOrderWB.Url = uri;
			ScaleForm.ScaleForms(this);
		}
		private void ConditionOrder_FormClosing(object sender, FormClosingEventArgs e)
		{
			Global.HTConfig.Remove("conditionOrder");
		}
	}
}
