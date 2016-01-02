using DIYForm;
using Gnnt.MEBS.HQModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient
{
	public class About : MyForm
	{
		private IContainer components;
		private Label labCompony;
		private Button btnOK;
		private WebBrowser webBrowser;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
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
			this.labCompony = new Label();
			this.btnOK = new Button();
			this.webBrowser = new WebBrowser();
			base.SuspendLayout();
			this.labCompony.Anchor = AnchorStyles.Top;
			this.labCompony.AutoSize = true;
			this.labCompony.BackColor = Color.Transparent;
			this.labCompony.Font = new Font("宋体", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labCompony.ForeColor = Color.DarkRed;
			this.labCompony.Location = new Point(57, 50);
			this.labCompony.Name = "labCompony";
			this.labCompony.Size = new Size(209, 19);
			this.labCompony.TabIndex = 6;
			this.labCompony.Text = "金网安泰统一交易系统";
			this.btnOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnOK.BackColor = Color.Transparent;
			this.btnOK.DialogResult = DialogResult.Cancel;
			this.btnOK.FlatStyle = FlatStyle.Popup;
			this.btnOK.Location = new Point(215, 235);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(73, 29);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new EventHandler(this.button1_Click);
			this.webBrowser.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.webBrowser.Location = new Point(19, 82);
			this.webBrowser.MinimumSize = new Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new Size(286, 138);
			this.webBrowser.TabIndex = 17;
			this.webBrowser.TabStop = false;
			this.webBrowser.Url = new Uri("", UriKind.Relative);
			this.webBrowser.Visible = false;
			base.AcceptButton = this.btnOK;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnOK;
			base.ClientSize = new Size(319, 284);
			base.Controls.Add(this.webBrowser);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.labCompony);
			base.Name = "About";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "关于";
			base.Load += new EventHandler(this.About_Load);
			base.Controls.SetChildIndex(this.labCompony, 0);
			base.Controls.SetChildIndex(this.btnOK, 0);
			base.Controls.SetChildIndex(this.webBrowser, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public About(MainWindow mainWindow)
		{
			this.InitializeComponent();
			this.pluginInfo = mainWindow.pluginInfo;
			this.setInfo = mainWindow.setInfo;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void SetControlText()
		{
			this.webBrowser.Visible = true;
			base.Icon = (Icon)this.pluginInfo.HQResourceManager.GetObject("Logo.ico");
			this.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_AboutTitle");
			this.labCompony.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_labCompony");
			this.btnOK.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btnOK");
			string text = this.pluginInfo.HTConfig["AddressAbout"].ToString();
			if (text.Length > 0)
			{
				this.webBrowser.Url = new Uri(text);
				return;
			}
			string text2 = string.Empty;
			string path = "about.txt";
			if (!File.Exists(path))
			{
				text2 = "暂无关于";
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
		private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			e.Cancel = true;
		}
		private void About_Load(object sender, EventArgs e)
		{
			this.SetControlText();
		}
	}
}
