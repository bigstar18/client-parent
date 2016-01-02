using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class AboutForm : Form
	{
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel;
		private LinkLabel linkCompany;
		private Label labelVersion;
		private Label labelCompanyName;
		private Label labelProductName;
		private Button okButton;
		private PictureBox pictureBox1;
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
			this.tableLayoutPanel = new TableLayoutPanel();
			this.linkCompany = new LinkLabel();
			this.labelVersion = new Label();
			this.labelCompanyName = new Label();
			this.labelProductName = new Label();
			this.okButton = new Button();
			this.pictureBox1 = new PictureBox();
			this.tableLayoutPanel.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 67f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67f));
			this.tableLayoutPanel.Controls.Add(this.linkCompany, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel.Dock = DockStyle.Fill;
			this.tableLayoutPanel.Location = new Point(3, 3);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 6;
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 26.81159f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.94203f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 12.31884f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.49275f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 9.42029f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20.28986f));
			this.tableLayoutPanel.Size = new Size(398, 136);
			this.tableLayoutPanel.TabIndex = 1;
			this.linkCompany.AutoSize = true;
			this.linkCompany.Location = new Point(73, 92);
			this.linkCompany.Margin = new Padding(6, 0, 3, 0);
			this.linkCompany.Name = "linkCompany";
			this.linkCompany.Size = new Size(137, 12);
			this.linkCompany.TabIndex = 25;
			this.linkCompany.TabStop = true;
			this.linkCompany.Text = "http://www.gnnt.com.cn";
			this.labelVersion.Dock = DockStyle.Fill;
			this.labelVersion.Location = new Point(73, 36);
			this.labelVersion.Margin = new Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new Size(0, 16);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new Size(322, 16);
			this.labelVersion.TabIndex = 0;
			this.labelVersion.Text = "版本";
			this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
			this.labelCompanyName.Dock = DockStyle.Fill;
			this.labelCompanyName.Location = new Point(73, 73);
			this.labelCompanyName.Margin = new Padding(6, 0, 3, 0);
			this.labelCompanyName.MaximumSize = new Size(0, 16);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new Size(322, 16);
			this.labelCompanyName.TabIndex = 22;
			this.labelCompanyName.Text = "技术支持";
			this.labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
			this.labelProductName.AutoSize = true;
			this.labelProductName.Location = new Point(73, 3);
			this.labelProductName.Margin = new Padding(6, 3, 3, 0);
			this.labelProductName.MaximumSize = new Size(0, 16);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new Size(53, 12);
			this.labelProductName.TabIndex = 19;
			this.labelProductName.Text = "产品名称";
			this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
			this.okButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.okButton.DialogResult = DialogResult.Cancel;
			this.okButton.Location = new Point(320, 113);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(75, 20);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "关闭(&O)";
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new Point(20, 3);
			this.pictureBox1.Margin = new Padding(20, 3, 0, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(32, 30);
			this.pictureBox1.TabIndex = 26;
			this.pictureBox1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(404, 142);
			base.Controls.Add(this.tableLayoutPanel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.Padding = new Padding(3);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "AboutForm";
			base.Load += new EventHandler(this.AboutForm_Load);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
		public AboutForm()
		{
			this.InitializeComponent();
		}
		private void AboutForm_Load(object sender, EventArgs e)
		{
			ScaleForm.ScaleForms(this);
		}
	}
}
