using Org.Mentalis.Network.ProxySocket;
using PluginInterface;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
namespace SetServerInfoPlugin
{
	public class ServerSet : Form
	{
		private IContainer components;
		private TabControl tabCSetServer;
		private Button btApply;
		private Button btConfirm;
		private Button btCancel;
		private RadioButton radioTelecom;
		private RadioButton radioNetcom;
		private Label label1;
		private Button btSetProxy;
		private IPluginHost myHost;
		private Hashtable htConfig;
		private bool updateNetUser;
		private bool isUpdate;
		private int curServer;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ServerSet));
			this.tabCSetServer = new TabControl();
			this.btApply = new Button();
			this.btConfirm = new Button();
			this.btCancel = new Button();
			this.radioTelecom = new RadioButton();
			this.radioNetcom = new RadioButton();
			this.label1 = new Label();
			this.btSetProxy = new Button();
			base.SuspendLayout();
			this.tabCSetServer.Dock = DockStyle.Top;
			this.tabCSetServer.Location = new Point(0, 0);
			this.tabCSetServer.Name = "tabCSetServer";
			this.tabCSetServer.SelectedIndex = 0;
			this.tabCSetServer.Size = new Size(640, 295);
			this.tabCSetServer.TabIndex = 0;
			this.btApply.Enabled = false;
			this.btApply.Location = new Point(571, 301);
			this.btApply.Name = "btApply";
			this.btApply.Size = new Size(66, 23);
			this.btApply.TabIndex = 1;
			this.btApply.Text = "应用&A";
			this.btApply.UseVisualStyleBackColor = true;
			this.btApply.Click += new EventHandler(this.btApply_Click);
			this.btConfirm.Location = new Point(407, 301);
			this.btConfirm.Name = "btConfirm";
			this.btConfirm.Size = new Size(66, 23);
			this.btConfirm.TabIndex = 1;
			this.btConfirm.Text = "确定";
			this.btConfirm.UseVisualStyleBackColor = true;
			this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
			this.btCancel.Location = new Point(489, 301);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new Size(66, 23);
			this.btCancel.TabIndex = 1;
			this.btCancel.Text = "取消";
			this.btCancel.UseVisualStyleBackColor = true;
			this.btCancel.Click += new EventHandler(this.btCancel_Click);
			this.radioTelecom.AutoSize = true;
			this.radioTelecom.Location = new Point(203, 304);
			this.radioTelecom.Name = "radioTelecom";
			this.radioTelecom.Size = new Size(71, 16);
			this.radioTelecom.TabIndex = 2;
			this.radioTelecom.TabStop = true;
			this.radioTelecom.Text = "电信用户";
			this.radioTelecom.UseVisualStyleBackColor = true;
			this.radioTelecom.CheckedChanged += new EventHandler(this.radioTelecom_CheckedChanged);
			this.radioNetcom.AutoSize = true;
			this.radioNetcom.Location = new Point(121, 304);
			this.radioNetcom.Name = "radioNetcom";
			this.radioNetcom.Size = new Size(71, 16);
			this.radioNetcom.TabIndex = 2;
			this.radioNetcom.TabStop = true;
			this.radioNetcom.Text = "联通用户";
			this.radioNetcom.UseVisualStyleBackColor = true;
			this.radioNetcom.CheckedChanged += new EventHandler(this.radioNetcom_CheckedChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 306);
			this.label1.Name = "label1";
			this.label1.Size = new Size(101, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "请选择您所属网络";
			this.btSetProxy.Location = new Point(291, 301);
			this.btSetProxy.Name = "btSetProxy";
			this.btSetProxy.Size = new Size(103, 23);
			this.btSetProxy.TabIndex = 4;
			this.btSetProxy.Text = "设置代理服务器";
			this.btSetProxy.UseVisualStyleBackColor = true;
			this.btSetProxy.Click += new EventHandler(this.btSetProxy_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(640, 328);
			base.Controls.Add(this.btSetProxy);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.radioNetcom);
			base.Controls.Add(this.radioTelecom);
			base.Controls.Add(this.btCancel);
			base.Controls.Add(this.btConfirm);
			base.Controls.Add(this.btApply);
			base.Controls.Add(this.tabCSetServer);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ServerSet";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "网络设置";
			base.Load += new EventHandler(this.ServerSet_Load);
			base.FormClosed += new FormClosedEventHandler(this.ServerSet_FormClosed);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ServerSet(IPluginHost myHost)
		{
			this.InitializeComponent();
			this.myHost = myHost;
		}
		private void ServerSet_Load(object sender, EventArgs e)
		{
			this.htConfig = this.myHost.get_ConfigurationInfo().getSection("Systems");
			this.SetText();
			this.LoadNetUserInfo();
			this.radioNetcom.Text = ServerSetPlugin.strServerNameFir;
			this.radioTelecom.Text = ServerSetPlugin.strServerNameSec;
			this.LoadServer();
			this.btApply.Enabled = false;
		}
		private void SetText()
		{
			this.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_ServerSetTitle");
			this.label1.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_label1");
			this.btSetProxy.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btSetProxy");
			this.btConfirm.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btConfirm");
			this.btCancel.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btCancel");
			this.btApply.Text = ServerSetPlugin.SysResourceManager.GetString("PluginStr_btApply");
		}
		private void LoadNetUserInfo()
		{
			if (this.htConfig == null)
			{
				return;
			}
			try
			{
				this.curServer = int.Parse((string)this.htConfig["CurServer"]);
			}
			catch
			{
			}
			int num = this.curServer;
			if (num == 0)
			{
				this.radioTelecom.Checked = true;
				return;
			}
			this.radioNetcom.Checked = true;
		}
		private void updateNetUserInfo()
		{
			this.myHost.get_ConfigurationInfo().updateValue("Systems", "CurServer", string.Concat(this.curServer));
		}
		private void LoadServer()
		{
			this.populateTabControl(this.tabCSetServer);
		}
		private void populateTabControl(TabControl tabCSetServer)
		{
			foreach (DictionaryEntry dictionaryEntry in this.myHost.get_HtConfigInfo())
			{
				try
				{
					string name = (string)dictionaryEntry.Key;
					PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)dictionaryEntry.Value;
					string xmlPath = pluginConfigInfo.XmlPath;
					XmlDocument xmlDoc = pluginConfigInfo.XmlDoc;
					if (xmlDoc != null)
					{
						string text = string.Empty;
						string text2 = string.Empty;
						string text3 = string.Empty;
						XmlElement xmlElement = (XmlElement)xmlDoc.SelectSingleNode("ConfigInfo");
						if (xmlElement.SelectSingleNode("Enable") != null)
						{
							text = xmlElement.SelectSingleNode("Enable").InnerText;
						}
						if (xmlElement.SelectSingleNode("Text") != null)
						{
							text3 = xmlElement.SelectSingleNode("Text").InnerText;
						}
						if (xmlElement.SelectSingleNode("ServerType") != null)
						{
							text2 = xmlElement.SelectSingleNode("ServerType").InnerText;
						}
						if (Tools.StrToBool(text))
						{
							int num = Tools.StrToInt(text2);
							if (num == 0 || num == 1)
							{
								TabPage tabPage = new TabPage();
								tabPage.Name = name;
								tabPage.Text = text3;
								tabPage.AutoScroll = true;
								SetServerInfoC setServerInfoC = new SetServerInfoC(num);
								setServerInfoC.CurServer = this.curServer;
								setServerInfoC.XmlPath = xmlPath;
								setServerInfoC.XmlDoc = xmlDoc;
								setServerInfoC.Initialize();
								setServerInfoC.Dock = DockStyle.Fill;
								setServerInfoC.dataChange += new SetServerInfoC.dataChangeEventHander(this.c_dataChange);
								tabPage.Controls.Add(setServerInfoC);
								tabCSetServer.Controls.Add(tabPage);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
		}
		private void btApply_Click(object sender, EventArgs e)
		{
			try
			{
				this.tabCSetServer.Enabled = false;
				if (this.updateNetUser)
				{
					this.updateNetUserInfo();
					this.isUpdate = true;
				}
				for (int i = 0; i < this.tabCSetServer.TabPages.Count; i++)
				{
					foreach (Control control in this.tabCSetServer.TabPages[i].Controls)
					{
						if (control is SetServerInfoC)
						{
							SetServerInfoC setServerInfoC = (SetServerInfoC)control;
							if (setServerInfoC.IsUpdateServer)
							{
								this.isUpdate = true;
								setServerInfoC.updateServerInfo();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.tabCSetServer.Enabled = true;
			this.btApply.Enabled = false;
		}
		private void btConfirm_Click(object sender, EventArgs e)
		{
			if (this.btApply.Enabled)
			{
				try
				{
					this.btApply_Click(null, null);
					base.Close();
					goto IL_32;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					goto IL_32;
				}
			}
			base.Close();
			IL_32:
			if (this.isUpdate)
			{
				base.DialogResult = DialogResult.OK;
			}
		}
		private void btCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}
		private void ServerSet_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Dispose();
		}
		private void radioTelecom_CheckedChanged(object sender, EventArgs e)
		{
			this.updateNetUser = true;
			this.curServer = 0;
			for (int i = 0; i < this.tabCSetServer.TabPages.Count; i++)
			{
				TabPage tabPage = this.tabCSetServer.TabPages[i];
				foreach (Control control in tabPage.Controls)
				{
					if (control is SetServerInfoC)
					{
						foreach (Control control2 in control.Controls)
						{
							if (control2 is GroupBox)
							{
								if (control2.Name.Equals("CurTelecomServer"))
								{
									control2.Enabled = true;
								}
								else
								{
									control2.Enabled = false;
								}
							}
						}
					}
				}
			}
			this.btApply.Enabled = true;
		}
		private void radioNetcom_CheckedChanged(object sender, EventArgs e)
		{
			this.updateNetUser = true;
			this.curServer = 1;
			for (int i = 0; i < this.tabCSetServer.TabPages.Count; i++)
			{
				TabPage tabPage = this.tabCSetServer.TabPages[i];
				foreach (Control control in tabPage.Controls)
				{
					if (control is SetServerInfoC)
					{
						foreach (Control control2 in control.Controls)
						{
							if (control2 is GroupBox)
							{
								if (control2.Name.Equals("CurNetcomServer"))
								{
									control2.Enabled = true;
								}
								else
								{
									control2.Enabled = false;
								}
							}
						}
					}
				}
			}
			this.btApply.Enabled = true;
		}
		private void c_dataChange(object sender, EventArgs e)
		{
			this.btApply.Enabled = true;
		}
		private void btSetProxy_Click(object sender, EventArgs e)
		{
			string text = string.Empty;
			if (File.Exists("Proxy.ini"))
			{
				StreamReader streamReader = new StreamReader("Proxy.ini");
				text = streamReader.ReadToEnd();
				streamReader.Close();
			}
			SetProxyServer setProxyServer = new SetProxyServer(ServerSetPlugin.SysResourceManager);
			setProxyServer.ShowDialog();
			string value = string.Empty;
			if (File.Exists("Proxy.ini"))
			{
				StreamReader streamReader2 = new StreamReader("Proxy.ini");
				value = streamReader2.ReadToEnd();
				streamReader2.Close();
			}
			if (!text.Equals(value))
			{
				this.isUpdate = true;
			}
		}
	}
}
