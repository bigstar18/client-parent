using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary;
using TPME.Log;
namespace FuturesTrade.Gnnt.UI.Containers
{
	public class LockControl : UserControl
	{
		public delegate void LockFormCallBack(bool type);
		private LocalHook hook;
		private OperationManager operationManager = OperationManager.GetInstance();
		public LockControl.LockFormCallBack lockFormCallBack;
		private IContainer components;
		private Panel panelLock;
		private PictureBox picSoftKey;
		private Label labelPwdInfo;
		private Button buttonUnLock;
		private Label labelPwd;
		private PasswordTextBox textBoxPwd;
		public LockControl()
		{
			this.InitializeComponent();
		}
		private void LockControl_Load(object sender, EventArgs e)
		{
			this.SetControlText();
		}
		private void SetControlText()
		{
			try
			{
				this.labelPwd.Text = Global.M_ResourceManager.GetString("TradeStr_MainForm_labelPwd");
				this.labelPwdInfo.Text = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPassword");
				this.buttonUnLock.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_UnlockButton");
				this.panelLock.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_panelLockImage");
				this.panelLock.BackgroundImageLayout = ImageLayout.Tile;
				this.panelLock.Left = (base.Parent.Width - this.panelLock.Width) / 2;
				this.panelLock.Top = (base.Parent.Height - this.panelLock.Height) / 2;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		public void LockSet(bool type)
		{
			if (base.Width - this.panelLock.Width > 0 && base.Height - this.panelLock.Height > 0)
			{
				this.panelLock.Location = new Point((base.Parent.Width - this.panelLock.Width) / 2, (base.Parent.Height - this.panelLock.Height) / 2);
			}
			if (!type)
			{
				this.BackColor = Color.Black;
			}
			else
			{
				this.BackColor = SystemColors.Control;
				Global.IdleStartTime = DateTime.Now;
			}
			this.operationManager.SetRefreshTime(type);
		}
		private void AddHook()
		{
			Global.IdleStartTime = DateTime.Now;
			this.hook = new LocalHook();
			this.hook.OnMouseActivity += new MouseEventHandler(this.hook_OnMouseActivity);
			this.hook.KeyDown += new KeyEventHandler(this.hook_KeyDown);
		}
		private void hook_OnMouseActivity(object sender, MouseEventArgs e)
		{
			Global.IdleStartTime = DateTime.Now;
		}
		private void hook_KeyDown(object sender, KeyEventArgs e)
		{
			Global.IdleStartTime = DateTime.Now;
		}
		private void buttonUnLock_Click(object sender, EventArgs e)
		{
			this.textBoxPwd.CheckPass = true;
			string text = this.textBoxPwd.Text;
			this.textBoxPwd.CheckPass = false;
			if (text.Equals(Global.Password))
			{
				this.textBoxPwd.CheckPass = true;
				this.textBoxPwd.Text = "";
				this.textBoxPwd.CheckPass = false;
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPassword");
				this.labelPwdInfo.Text = @string;
				if (this.lockFormCallBack != null)
				{
					this.lockFormCallBack(true);
				}
				return;
			}
			string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_PasswordError");
			this.labelPwdInfo.Text = string2;
			this.textBoxPwd.Focus();
			this.textBoxPwd.SelectAll();
		}
		private void textBoxPwd_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.buttonUnLock_Click(sender, e);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LockControl));
			this.panelLock = new Panel();
			this.picSoftKey = new PictureBox();
			this.labelPwdInfo = new Label();
			this.buttonUnLock = new Button();
			this.labelPwd = new Label();
			this.textBoxPwd = new PasswordTextBox();
			this.panelLock.SuspendLayout();
			((ISupportInitialize)this.picSoftKey).BeginInit();
			base.SuspendLayout();
			this.panelLock.BackgroundImage = (Image)componentResourceManager.GetObject("panelLock.BackgroundImage");
			this.panelLock.BorderStyle = BorderStyle.Fixed3D;
			this.panelLock.Controls.Add(this.picSoftKey);
			this.panelLock.Controls.Add(this.labelPwdInfo);
			this.panelLock.Controls.Add(this.buttonUnLock);
			this.panelLock.Controls.Add(this.labelPwd);
			this.panelLock.Controls.Add(this.textBoxPwd);
			this.panelLock.Location = new Point(125, 27);
			this.panelLock.Name = "panelLock";
			this.panelLock.Size = new Size(628, 200);
			this.panelLock.TabIndex = 8;
			this.picSoftKey.BackColor = Color.Transparent;
			this.picSoftKey.ImeMode = ImeMode.NoControl;
			this.picSoftKey.Location = new Point(284, 71);
			this.picSoftKey.Name = "picSoftKey";
			this.picSoftKey.Size = new Size(16, 16);
			this.picSoftKey.TabIndex = 4;
			this.picSoftKey.TabStop = false;
			this.labelPwdInfo.AutoSize = true;
			this.labelPwdInfo.BackColor = Color.Transparent;
			this.labelPwdInfo.ImeMode = ImeMode.NoControl;
			this.labelPwdInfo.Location = new Point(315, 72);
			this.labelPwdInfo.Name = "labelPwdInfo";
			this.labelPwdInfo.Size = new Size(89, 12);
			this.labelPwdInfo.TabIndex = 3;
			this.labelPwdInfo.Text = "请输入登录密码";
			this.buttonUnLock.BackColor = Color.Transparent;
			this.buttonUnLock.ForeColor = Color.Black;
			this.buttonUnLock.ImeMode = ImeMode.NoControl;
			this.buttonUnLock.Location = new Point(115, 108);
			this.buttonUnLock.Name = "buttonUnLock";
			this.buttonUnLock.Size = new Size(145, 26);
			this.buttonUnLock.TabIndex = 2;
			this.buttonUnLock.UseVisualStyleBackColor = false;
			this.buttonUnLock.Click += new EventHandler(this.buttonUnLock_Click);
			this.labelPwd.BackColor = Color.Transparent;
			this.labelPwd.ImeMode = ImeMode.NoControl;
			this.labelPwd.Location = new Point(54, 68);
			this.labelPwd.Name = "labelPwd";
			this.labelPwd.Size = new Size(55, 21);
			this.labelPwd.TabIndex = 1;
			this.labelPwd.Text = "密\u3000码：";
			this.labelPwd.TextAlign = ContentAlignment.MiddleCenter;
			this.textBoxPwd.CheckPass = false;
			this.textBoxPwd.Location = new Point(115, 68);
			this.textBoxPwd.Name = "textBoxPwd";
			this.textBoxPwd.PasswordChar = '*';
			this.textBoxPwd.Size = new Size(168, 21);
			this.textBoxPwd.TabIndex = 0;
			this.textBoxPwd.KeyUp += new KeyEventHandler(this.textBoxPwd_KeyUp);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panelLock);
			base.Name = "LockControl";
			base.Size = new Size(963, 260);
			base.Load += new EventHandler(this.LockControl_Load);
			this.panelLock.ResumeLayout(false);
			this.panelLock.PerformLayout();
			((ISupportInitialize)this.picSoftKey).EndInit();
			base.ResumeLayout(false);
		}
	}
}
