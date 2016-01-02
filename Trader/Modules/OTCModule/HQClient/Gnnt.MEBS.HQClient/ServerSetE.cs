using DIYForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
namespace Gnnt.MEBS.HQClient
{
	public class ServerSetE : MyForm
	{
		private XmlElement xnConfigInfo;
		private XmlDocument xmlDoc;
		private Program.SET SET = new Program.SET();
		private bool isUpdate;
		private int curServer;
		private IContainer components;
		private GroupBox groupBoxServer;
		private Button btConfirm;
		private Button btCancel;
		private Button btApply;
		private RadioButton radioNet;
		private RadioButton radioTel;
		public ServerSetE()
		{
			this.InitializeComponent();
		}
		private void ServerSetE_Load(object sender, EventArgs e)
		{
			this.xmlDoc = new XmlDocument();
			this.xmlDoc.Load(this.SET.myConfigFileName);
			this.xnConfigInfo = (XmlElement)this.xmlDoc.SelectSingleNode("ConfigInfo");
			this.LoadNetUserInfo();
			this.btApply.Enabled = false;
			this.isUpdate = false;
		}
		private void LoadNetUserInfo()
		{
			if (this.xnConfigInfo == null)
			{
				return;
			}
			try
			{
				this.curServer = int.Parse(this.xnConfigInfo.SelectSingleNode("ServerType").InnerText, NumberStyles.None);
			}
			catch
			{
			}
			int num = this.curServer;
			if (num == 0)
			{
				this.radioTel.Checked = true;
				return;
			}
			this.radioNet.Checked = true;
		}
		private void radioTel_CheckedChanged(object sender, EventArgs e)
		{
			this.btApply.Enabled = true;
			this.isUpdate = true;
		}
		private void btCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}
		private void ServerSetE_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Dispose();
		}
		private void btApply_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.isUpdate)
				{
					this.updateNetUserInfo();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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
		private void updateNetUserInfo()
		{
			if (this.radioNet.Checked)
			{
				this.curServer = 1;
			}
			else
			{
				this.curServer = 0;
			}
			this.xnConfigInfo.SelectSingleNode("ServerType").InnerText = this.curServer.ToString();
			this.xmlDoc.Save(this.SET.myConfigFileName);
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
			this.groupBoxServer = new GroupBox();
			this.radioNet = new RadioButton();
			this.radioTel = new RadioButton();
			this.btConfirm = new Button();
			this.btCancel = new Button();
			this.btApply = new Button();
			this.groupBoxServer.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxServer.BackColor = Color.Transparent;
			this.groupBoxServer.Controls.Add(this.radioNet);
			this.groupBoxServer.Controls.Add(this.radioTel);
			this.groupBoxServer.FlatStyle = FlatStyle.Popup;
			this.groupBoxServer.Location = new Point(19, 37);
			this.groupBoxServer.Name = "groupBoxServer";
			this.groupBoxServer.Size = new Size(301, 116);
			this.groupBoxServer.TabIndex = 0;
			this.groupBoxServer.TabStop = false;
			this.groupBoxServer.Text = "服务器选择";
			this.radioNet.AutoSize = true;
			this.radioNet.Location = new Point(86, 88);
			this.radioNet.Name = "radioNet";
			this.radioNet.Size = new Size(83, 16);
			this.radioNet.TabIndex = 1;
			this.radioNet.TabStop = true;
			this.radioNet.Text = "联通服务器";
			this.radioNet.UseVisualStyleBackColor = true;
			this.radioNet.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
			this.radioTel.AutoSize = true;
			this.radioTel.Location = new Point(86, 48);
			this.radioTel.Name = "radioTel";
			this.radioTel.Size = new Size(83, 16);
			this.radioTel.TabIndex = 0;
			this.radioTel.TabStop = true;
			this.radioTel.Text = "电信服务器";
			this.radioTel.UseVisualStyleBackColor = true;
			this.radioTel.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
			this.btConfirm.Location = new Point(27, 173);
			this.btConfirm.Name = "btConfirm";
			this.btConfirm.Size = new Size(75, 23);
			this.btConfirm.TabIndex = 1;
			this.btConfirm.Text = "确定";
			this.btConfirm.UseVisualStyleBackColor = true;
			this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
			this.btCancel.Location = new Point(120, 173);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new Size(75, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "取消";
			this.btCancel.UseVisualStyleBackColor = true;
			this.btCancel.Click += new EventHandler(this.btCancel_Click);
			this.btApply.Location = new Point(211, 173);
			this.btApply.Name = "btApply";
			this.btApply.Size = new Size(75, 23);
			this.btApply.TabIndex = 3;
			this.btApply.Text = "应用";
			this.btApply.UseVisualStyleBackColor = true;
			this.btApply.Click += new EventHandler(this.btApply_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(347, 208);
			base.Controls.Add(this.btApply);
			base.Controls.Add(this.btCancel);
			base.Controls.Add(this.btConfirm);
			base.Controls.Add(this.groupBoxServer);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ServerSetE";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "网络设置";
			base.FormClosed += new FormClosedEventHandler(this.ServerSetE_FormClosed);
			base.Load += new EventHandler(this.ServerSetE_Load);
			base.Controls.SetChildIndex(this.groupBoxServer, 0);
			base.Controls.SetChildIndex(this.btConfirm, 0);
			base.Controls.SetChildIndex(this.btCancel, 0);
			base.Controls.SetChildIndex(this.btApply, 0);
			this.groupBoxServer.ResumeLayout(false);
			this.groupBoxServer.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
