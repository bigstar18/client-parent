using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeClientApp.Gnnt.ISSUE.Library;
namespace TradeClientApp.Gnnt.ISSUE
{
	internal class AboutForm : Form
	{
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel;
		private Button okButton;
		private LinkLabel linkCompany;
		private Label labelVersion;
		private Label labelCompanyName;
		private Label labelProductName;
		private PictureBox pictureBox1;
		public string AssemblyTitle
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (customAttributes.Length > 0)
				{
					AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
					if (assemblyTitleAttribute.Title != "")
					{
						return assemblyTitleAttribute.Title;
					}
				}
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}
		public string AssemblyDescription
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
			}
		}
		public string AssemblyProduct
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)customAttributes[0]).Product;
			}
		}
		public string AssemblyCopyright
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
			}
		}
		public string AssemblyCompany
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
			}
		}
		public AboutForm()
		{
			this.InitializeComponent();
			this.Text = string.Format("关于 {0}", "");
			this.labelProductName.Text = (string)Global.HTConfig["Title"];
			this.labelVersion.Text = string.Format("版本 {0}", this.VersionFormat(this.AssemblyVersion));
			this.pictureBox1.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_gnnt_logo");
			this.labelCompanyName.Text = this.AssemblyCompany;
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
		}
		public string VersionFormat(string version)
		{
			string[] array = version.Split(new char[]
			{
				'.'
			});
			version = array[0] + "." + array[1];
			for (int i = 2; i < array.Length; i++)
			{
				if (array[i].Length < 2)
				{
					array[i] = "0" + array[i];
				}
				version = version + "." + array[i];
			}
			return version;
		}
		private void okButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void linkCompany_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("IEXPLORE.EXE", "http://www.gnnt.com.cn");
		}
		private void AboutForm_Load(object sender, EventArgs e)
		{
			ScaleForm.ScaleForms(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AboutForm));
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
			this.tableLayoutPanel.Location = new Point(9, 8);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 6;
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 26.81159f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.94203f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 12.31884f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.49275f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 9.42029f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20.28986f));
			this.tableLayoutPanel.Size = new Size(382, 138);
			this.tableLayoutPanel.TabIndex = 0;
			this.linkCompany.AutoSize = true;
			this.linkCompany.Location = new Point(73, 96);
			this.linkCompany.Margin = new Padding(6, 0, 3, 0);
			this.linkCompany.Name = "linkCompany";
			this.linkCompany.Size = new Size(137, 12);
			this.linkCompany.TabIndex = 25;
			this.linkCompany.TabStop = true;
			this.linkCompany.Text = "http://www.gnnt.com.cn";
			this.linkCompany.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkCompany_LinkClicked);
			this.labelVersion.Dock = DockStyle.Fill;
			this.labelVersion.Location = new Point(73, 37);
			this.labelVersion.Margin = new Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new Size(0, 16);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new Size(306, 16);
			this.labelVersion.TabIndex = 0;
			this.labelVersion.Text = "版本";
			this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
			this.labelCompanyName.Dock = DockStyle.Fill;
			this.labelCompanyName.Location = new Point(73, 76);
			this.labelCompanyName.Margin = new Padding(6, 0, 3, 0);
			this.labelCompanyName.MaximumSize = new Size(0, 16);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new Size(306, 16);
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
			this.okButton.Location = new Point(304, 115);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(75, 20);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "关闭(&O)";
			this.okButton.Click += new EventHandler(this.okButton_Click);
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new Point(20, 3);
			this.pictureBox1.Margin = new Padding(20, 3, 0, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(32, 30);
			this.pictureBox1.TabIndex = 26;
			this.pictureBox1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(400, 154);
			base.Controls.Add(this.tableLayoutPanel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.Padding = new Padding(9, 8, 9, 8);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "关于";
			base.TopMost = true;
			base.Load += new EventHandler(this.AboutForm_Load);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
