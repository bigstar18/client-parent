using DIYForm;
using Gnnt.MEBS.HQModel;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient
{
	public class SysMessage : MyForm
	{
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private Hashtable htConfig;
		private IContainer components;
		private Label labCompony;
		private Button btnOK;
		private WebBrowser webBrowser;
		public SysMessage(MainWindow mainWindow)
		{
			this.InitializeComponent();
			this.pluginInfo = mainWindow.pluginInfo;
			this.setInfo = mainWindow.setInfo;
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void SysMessage_Load(object sender, EventArgs e)
		{
			this.SetControlText();
		}
		private void SetControlText()
		{
			this.webBrowser.Visible = true;
			base.Icon = (Icon)this.pluginInfo.HQResourceManager.GetObject("Logo.ico");
			this.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_SysMessageTitle");
			this.labCompony.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_labCompony");
			this.btnOK.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btnOK");
			string text = this.pluginInfo.HTConfig["AddressLicense"].ToString();
			if (text.Length > 0)
			{
				this.webBrowser.Url = new Uri(text);
				return;
			}
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
		private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			e.Cancel = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysMessage));
			this.labCompony = new Label();
			this.btnOK = new Button();
			this.webBrowser = new WebBrowser();
			base.SuspendLayout();
			this.labCompony.Anchor = AnchorStyles.Top;
			this.labCompony.AutoSize = true;
			this.labCompony.BackColor = Color.Transparent;
			this.labCompony.Font = new Font("宋体", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labCompony.ForeColor = Color.DarkRed;
			this.labCompony.Location = new Point(187, 38);
			this.labCompony.Name = "labCompony";
			this.labCompony.Size = new Size(169, 19);
			this.labCompony.TabIndex = 6;
			this.labCompony.Text = "金网安泰统一系统";
			this.btnOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnOK.BackColor = Color.Transparent;
			this.btnOK.DialogResult = DialogResult.Cancel;
			this.btnOK.FlatStyle = FlatStyle.Popup;
			this.btnOK.Location = new Point(235, 277);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(73, 29);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new EventHandler(this.btnOK_Click);
			this.webBrowser.Location = new Point(27, 74);
			this.webBrowser.MinimumSize = new Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new Size(509, 191);
			this.webBrowser.TabIndex = 16;
			this.webBrowser.TabStop = false;
			this.webBrowser.Url = new Uri("", UriKind.Relative);
			this.webBrowser.Visible = false;
			this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
			base.AcceptButton = this.btnOK;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnOK;
			base.ClientSize = new Size(545, 320);
			base.Controls.Add(this.webBrowser);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.labCompony);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "SysMessage";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "系统消息";
			base.Load += new EventHandler(this.SysMessage_Load);
			base.Controls.SetChildIndex(this.labCompony, 0);
			base.Controls.SetChildIndex(this.btnOK, 0);
			base.Controls.SetChildIndex(this.webBrowser, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
