using SysFrame.Gnnt.Common.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame.UI.Forms.PromptForms
{
	public class MarketInfo : Form
	{
		private IContainer components;
		private WebBrowser webBrowser1;
		public static bool isLoadedForm;
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
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(644, 442);
			base.Controls.Add(this.webBrowser1);
			base.Name = "MarketInfo";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "市场公告";
			base.Load += new EventHandler(this.MarketInfo_Load);
			base.ResumeLayout(false);
		}
		public MarketInfo()
		{
			this.InitializeComponent();
		}
		private void MarketInfo_Load(object sender, EventArgs e)
		{
			MarketInfo.isLoadedForm = true;
			try
			{
				ScaleForm.ScaleForms(this);
				string text = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["MarketInfo"].ToString();
				if (!string.IsNullOrEmpty(text))
				{
					this.webBrowser1.Url = new Uri(text);
					base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
				}
				else
				{
					base.Close();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
				base.Close();
			}
		}
	}
}
