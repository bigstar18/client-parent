using SysFrame.Gnnt.Common.Library;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.UI.Forms.PromptForms
{
	public class CommitForm : Form
	{
		private Hashtable htConfig;
		private int CommitType;
		private IContainer components;
		private Label labCommitment;
		private Button buttonUnAgree;
		private Button buttonAgree;
		private GroupBox groupBox1;
		private GroupBox gbMain;
		private WebBrowser webBrowser;
		private CheckBox chbCommit;
		public CommitForm()
		{
			this.InitializeComponent();
		}
		private void CommitForm_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			ScaleForm.ScaleForms(this);
		}
		private void SetControlText()
		{
			base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
			this.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_COMMIT_CAPTION");
			this.buttonAgree.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_AGREE");
			this.buttonUnAgree.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_UNAGREE");
			this.labCommitment.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LAB_COMMITMENT");
			this.webBrowser.Visible = true;
			string text = string.Empty;
			this.htConfig = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems");
			if (this.htConfig != null)
			{
				text = (string)this.htConfig["AddressLicense"];
			}
			if (text.Length > 0)
			{
				this.webBrowser.Url = new Uri(text);
			}
			else
			{
				string text2 = string.Empty;
				string path = "license.txt";
				if (!File.Exists(path))
				{
					text2 = "暂无协议";
				}
				else
				{
					StreamReader streamReader = new StreamReader(path, Encoding.Default);
					string str;
					while ((str = streamReader.ReadLine()) != null)
					{
						text2 += str;
					}
					streamReader.Close();
				}
				this.webBrowser.DocumentText = text2;
				this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
			}
			this.CommitType = Tools.StrToInt((string)this.htConfig["CommitType"], 0);
			if (this.CommitType == 0)
			{
				this.chbCommit.Text = "今日不再提示";
			}
			else
			{
				this.chbCommit.Text = "以后不再提示";
			}
			if (!Tools.StrToBool((string)this.htConfig["IsDisplayChbCommit"], false))
			{
				this.chbCommit.Visible = false;
			}
		}
		private void CloseForm(bool enable)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
			base.Dispose();
		}
		private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			e.Cancel = true;
		}
		private void buttonUnAgree_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
		}
		private void buttonAgree_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			if (this.chbCommit.Checked)
			{
				if (this.CommitType == 0)
				{
					Global.Modules.get_Plugins().get_ConfigurationInfo().updateValue("Systems", "IsDisplayCommitday", DateTime.Now.Date.ToShortDateString());
					return;
				}
				Global.Modules.get_Plugins().get_ConfigurationInfo().updateValue("Systems", "IsDisplayCommit", "false");
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
			this.labCommitment = new Label();
			this.buttonUnAgree = new Button();
			this.buttonAgree = new Button();
			this.groupBox1 = new GroupBox();
			this.webBrowser = new WebBrowser();
			this.gbMain = new GroupBox();
			this.chbCommit = new CheckBox();
			this.groupBox1.SuspendLayout();
			this.gbMain.SuspendLayout();
			base.SuspendLayout();
			this.labCommitment.BackColor = SystemColors.Control;
			this.labCommitment.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labCommitment.Location = new Point(56, 24);
			this.labCommitment.Name = "labCommitment";
			this.labCommitment.Size = new Size(464, 24);
			this.labCommitment.TabIndex = 7;
			this.labCommitment.Text = "交易商承诺";
			this.labCommitment.TextAlign = ContentAlignment.MiddleCenter;
			this.buttonUnAgree.DialogResult = DialogResult.Cancel;
			this.buttonUnAgree.Location = new Point(343, 277);
			this.buttonUnAgree.Name = "buttonUnAgree";
			this.buttonUnAgree.Size = new Size(112, 24);
			this.buttonUnAgree.TabIndex = 2;
			this.buttonUnAgree.Text = "不 同 意";
			this.buttonUnAgree.Click += new EventHandler(this.buttonUnAgree_Click);
			this.buttonAgree.Location = new Point(169, 277);
			this.buttonAgree.Name = "buttonAgree";
			this.buttonAgree.Size = new Size(112, 24);
			this.buttonAgree.TabIndex = 1;
			this.buttonAgree.Text = "同   意";
			this.buttonAgree.Click += new EventHandler(this.buttonAgree_Click);
			this.groupBox1.Controls.Add(this.webBrowser);
			this.groupBox1.Location = new Point(18, 51);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(544, 216);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.webBrowser.Location = new Point(6, 11);
			this.webBrowser.MinimumSize = new Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new Size(532, 201);
			this.webBrowser.TabIndex = 0;
			this.webBrowser.TabStop = false;
			this.webBrowser.Url = new Uri("", UriKind.Relative);
			this.webBrowser.Visible = false;
			this.gbMain.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.gbMain.Controls.Add(this.chbCommit);
			this.gbMain.Controls.Add(this.labCommitment);
			this.gbMain.Location = new Point(-1, -9);
			this.gbMain.Name = "gbMain";
			this.gbMain.Size = new Size(582, 326);
			this.gbMain.TabIndex = 8;
			this.gbMain.TabStop = false;
			this.chbCommit.AutoSize = true;
			this.chbCommit.Location = new Point(34, 289);
			this.chbCommit.Name = "chbCommit";
			this.chbCommit.Size = new Size(96, 16);
			this.chbCommit.TabIndex = 3;
			this.chbCommit.Text = "今日不再提示";
			this.chbCommit.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(580, 314);
			base.ControlBox = false;
			base.Controls.Add(this.buttonAgree);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonUnAgree);
			base.Controls.Add(this.gbMain);
			base.MaximizeBox = false;
			base.Name = "CommitForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "CommitForm";
			base.Load += new EventHandler(this.CommitForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.gbMain.ResumeLayout(false);
			this.gbMain.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
