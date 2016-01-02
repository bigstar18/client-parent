using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class MessageForm : Form
	{
		public bool isOK;
		private IContainer components;
		private Button buttonOK;
		private Button buttonCancel;
		private Label textBoxMessage;
		private PictureBox pictureBoxOK;
		private Panel panel1;
		public Size formSize
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}
		public Size textSize
		{
			get
			{
				return this.textBoxMessage.Size;
			}
			set
			{
				this.textBoxMessage.Size = value;
			}
		}
		public MessageForm(string formName, string message, int formStyle)
		{
			try
			{
				this.InitializeComponent();
				this.Text = formName;
				this.buttonOK.Text = Global.m_PMESResourceManager.GetString("TradeStr_BUTTON_OK");
				this.buttonCancel.Text = Global.m_PMESResourceManager.GetString("TradeStr_BUTTON_CANCEL");
				this.textBoxMessage.Text = message;
				this.buttonOK.Focus();
				if (formStyle == 0)
				{
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_wenhao");
				}
				else if (formStyle == 1)
				{
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_gantanhao");
					this.buttonOK.Left = (base.Width - this.buttonOK.Width) / 2;
					this.buttonCancel.Visible = false;
				}
				base.Icon = Global.SystamIcon;
				this.Font = Global.GetIniFont();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public MessageForm(string formName, string message, int formStyle, StatusBarType statusBarType)
		{
			try
			{
				this.InitializeComponent();
				this.Text = formName;
				base.Icon = Global.SystamIcon;
				this.buttonOK.Text = Global.m_PMESResourceManager.GetString("TradeStr_BUTTON_OK");
				this.buttonCancel.Text = Global.m_PMESResourceManager.GetString("TradeStr_BUTTON_CANCEL");
				this.textBoxMessage.Text = message;
				this.buttonOK.Focus();
				if (formStyle == 0)
				{
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_wenhao");
				}
				else if (formStyle == 1)
				{
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_gantanhao");
					this.buttonOK.Left = (base.Width - this.buttonOK.Width) / 2;
					this.buttonCancel.Visible = false;
				}
				switch (statusBarType)
				{
				case StatusBarType.Message:
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("status_info.png");
					break;
				case StatusBarType.Warning:
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("status_warning.png");
					break;
				case StatusBarType.Error:
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("status_error.png");
					break;
				case StatusBarType.Success:
					this.pictureBoxOK.Image = (Image)Global.m_PMESResourceManager.GetObject("status_success.png");
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.isOK = true;
			base.Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isOK = false;
			base.Close();
		}
		private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
		{
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MessageForm));
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.textBoxMessage = new Label();
			this.pictureBoxOK = new PictureBox();
			this.panel1 = new Panel();
			((ISupportInitialize)this.pictureBoxOK).BeginInit();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.buttonOK.Location = new Point(49, 3);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(167, 3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.textBoxMessage.Location = new Point(49, 12);
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.Size = new Size(231, 70);
			this.textBoxMessage.TabIndex = 4;
			this.pictureBoxOK.Location = new Point(10, 12);
			this.pictureBoxOK.Name = "pictureBoxOK";
			this.pictureBoxOK.Size = new Size(32, 32);
			this.pictureBoxOK.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBoxOK.TabIndex = 5;
			this.pictureBoxOK.TabStop = false;
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Dock = DockStyle.Bottom;
			this.panel1.Location = new Point(0, 85);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(292, 31);
			this.panel1.TabIndex = 6;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(292, 116);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.textBoxMessage);
			base.Controls.Add(this.pictureBoxOK);
			this.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MessageForm";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "MessageForm";
			base.TopMost = true;
			base.FormClosing += new FormClosingEventHandler(this.MessageForm_FormClosing);
			((ISupportInitialize)this.pictureBoxOK).EndInit();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
