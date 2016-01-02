using YrdceClient.Yrdce.Common.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace YrdceClient.UI.Forms.PromptForms
{
	public class BalanceForm : Form
	{
		private IContainer components;
		private Button btnYes;
		private WebBrowser reportBrowser;
		private Button btnNo;
		public Uri Url
		{
			get
			{
				return this.reportBrowser.Url;
			}
			set
			{
				this.reportBrowser.Url = value;
			}
		}
		public BalanceForm()
		{
			this.InitializeComponent();
		}
		private void btnYes_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			base.Close();
		}
		private void btnNo_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
		}
		private void BalanceForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.SetControlText();
				ScaleForm.ScaleForms(this);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, base.Name + ex.ToString());
			}
		}
		private void SetControlText()
		{
			this.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_BalanceForm_Form");
			this.btnNo.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_BalanceForm_btnNo");
			this.btnYes.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_BalanceForm_btnYes");
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
			this.btnYes = new Button();
			this.reportBrowser = new WebBrowser();
			this.btnNo = new Button();
			base.SuspendLayout();
			this.btnYes.Location = new Point(63, 467);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new Size(100, 23);
			this.btnYes.TabIndex = 0;
			this.btnYes.Text = "同意";
			this.btnYes.UseVisualStyleBackColor = true;
			this.btnYes.Click += new EventHandler(this.btnYes_Click);
			this.reportBrowser.Location = new Point(0, 0);
			this.reportBrowser.MinimumSize = new Size(20, 20);
			this.reportBrowser.Name = "reportBrowser";
			this.reportBrowser.Size = new Size(485, 454);
			this.reportBrowser.TabIndex = 2;
			this.reportBrowser.TabStop = false;
			this.btnNo.Location = new Point(284, 467);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new Size(100, 23);
			this.btnNo.TabIndex = 1;
			this.btnNo.Text = "不同意";
			this.btnNo.UseVisualStyleBackColor = true;
			this.btnNo.Click += new EventHandler(this.btnNo_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			base.ClientSize = new Size(486, 496);
			base.ControlBox = false;
			base.Controls.Add(this.reportBrowser);
			base.Controls.Add(this.btnNo);
			base.Controls.Add(this.btnYes);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "BalanceForm";
			base.ShowIcon = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "资金结算表";
			base.Load += new EventHandler(this.BalanceForm_Load);
			base.ResumeLayout(false);
		}
	}
}
