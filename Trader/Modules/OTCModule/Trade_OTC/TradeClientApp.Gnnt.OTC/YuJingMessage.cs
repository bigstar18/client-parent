using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TPME.Log;
using TradeClientApp.Gnnt.OTC.Library;
namespace TradeClientApp.Gnnt.OTC
{
	public class YuJingMessage : Form
	{
		private const uint SC_MOVE = 61456u;
		private const uint MF_BYCOMMAND = 0u;
		private DataTable _dtmessage;
		private CreateXmlFile createxml;
		private string YJMessageFileName = Global.ConfigPath + "yjmessage" + Global.UserID + ".xml";
		private IContainer components;
		private DataGridView dataGridYJMessage;
		public YuJingMessage()
		{
			this.InitializeComponent();
			base.Icon = Global.SystamIcon;
		}
		private void YuJingMessage_FormClosed(object sender, FormClosedEventArgs e)
		{
			Global.YuJingFlag = false;
		}
		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hwnd, bool bRevert);
		[DllImport("user32.dll")]
		private static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);
		private void YuJingMessage_Load(object sender, EventArgs e)
		{
			IntPtr systemMenu = YuJingMessage.GetSystemMenu(base.Handle, false);
			if (systemMenu != IntPtr.Zero)
			{
				YuJingMessage.DeleteMenu(systemMenu, 61456u, 0u);
			}
			Global.YuJingFlag = true;
			this.BindData();
			this.SetColumnText();
		}
		public void BindData()
		{
			try
			{
				Logger.wirte(1, "YuJingBind线程启动1");
				if (this._dtmessage == null)
				{
					this._dtmessage = new DataTable();
				}
				if (this.createxml == null)
				{
					this.createxml = new CreateXmlFile();
				}
				this._dtmessage.Clear();
				this.createxml.CreateFile(this.YJMessageFileName);
				this._dtmessage = this.createxml.GetDataByXml("", "");
				Logger.wirte(1, "YuJingBind线程启动2");
				this.dataGridYJMessage.DataSource = this._dtmessage.DefaultView;
				this._dtmessage.DefaultView.Sort = "CFSJ desc";
				base.TopMost = true;
				Logger.wirte(1, "YuJingBind线程启动3");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "YuJingBind线程完成");
		}
		public void SetColumnText()
		{
			try
			{
				this.dataGridYJMessage.Columns["ID"].HeaderText = "预警序号";
				this.dataGridYJMessage.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				this.dataGridYJMessage.Columns["YJLX"].HeaderText = "预警类型";
				this.dataGridYJMessage.Columns["YJLX"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dataGridYJMessage.Columns["YJX"].Visible = false;
				this.dataGridYJMessage.Columns["YJTJ"].HeaderText = "预警条件";
				this.dataGridYJMessage.Columns["YJTJ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dataGridYJMessage.Columns["DQZ"].HeaderText = "当前值";
				this.dataGridYJMessage.Columns["DQZ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dataGridYJMessage.Columns["CFSJ"].HeaderText = "触发时间";
				this.dataGridYJMessage.Columns["CFSJ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
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
			this.dataGridYJMessage = new DataGridView();
			((ISupportInitialize)this.dataGridYJMessage).BeginInit();
			base.SuspendLayout();
			this.dataGridYJMessage.AllowUserToAddRows = false;
			this.dataGridYJMessage.AllowUserToDeleteRows = false;
			this.dataGridYJMessage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridYJMessage.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridYJMessage.BackgroundColor = SystemColors.ButtonFace;
			this.dataGridYJMessage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridYJMessage.Location = new Point(-1, 1);
			this.dataGridYJMessage.MultiSelect = false;
			this.dataGridYJMessage.Name = "dataGridYJMessage";
			this.dataGridYJMessage.ReadOnly = true;
			this.dataGridYJMessage.RowHeadersVisible = false;
			this.dataGridYJMessage.RowTemplate.Height = 23;
			this.dataGridYJMessage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridYJMessage.Size = new Size(466, 312);
			this.dataGridYJMessage.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.ButtonFace;
			base.ClientSize = new Size(465, 317);
			base.Controls.Add(this.dataGridYJMessage);
			this.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			this.MaximumSize = new Size(475, 349);
			base.MinimizeBox = false;
			this.MinimumSize = new Size(475, 349);
			base.Name = "YuJingMessage";
			base.ShowIcon = false;
			base.StartPosition = FormStartPosition.Manual;
			this.Text = "预警提示";
			base.Load += new EventHandler(this.YuJingMessage_Load);
			base.FormClosed += new FormClosedEventHandler(this.YuJingMessage_FormClosed);
			((ISupportInitialize)this.dataGridYJMessage).EndInit();
			base.ResumeLayout(false);
		}
	}
}
