using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class MessageForm : Form
	{
		public bool isOK;
		private IContainer components;
		private Button buttonOK;
		private Button buttonCancel;
		private Label textBoxMessage;
		private PictureBox pictureBoxOK;
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
			this.InitializeComponent();
			this.Text = formName;
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.buttonOK.Text = Global.M_ResourceManager.GetString("TradeStr_BUTTON_OK");
			this.buttonCancel.Text = Global.M_ResourceManager.GetString("TradeStr_BUTTON_CANCEL");
			this.textBoxMessage.Text = message;
			this.buttonOK.Focus();
			if (formStyle == 0)
			{
				this.pictureBoxOK.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_wenhao");
				return;
			}
			if (formStyle == 1)
			{
				this.pictureBoxOK.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_gantanhao");
				this.buttonOK.Left = (base.Width - this.buttonOK.Width) / 2;
				this.buttonCancel.Visible = false;
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
		private void MessageForm_Load(object sender, EventArgs e)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MessageForm));
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.textBoxMessage = new Label();
			this.pictureBoxOK = new PictureBox();
			((ISupportInitialize)this.pictureBoxOK).BeginInit();
			base.SuspendLayout();
			this.buttonOK.Location = new Point(40, 66);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(153, 66);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.textBoxMessage.Location = new Point(37, 12);
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.Size = new Size(201, 48);
			this.textBoxMessage.TabIndex = 4;
			this.textBoxMessage.TextAlign = ContentAlignment.MiddleLeft;
			this.pictureBoxOK.Location = new Point(10, 12);
			this.pictureBoxOK.Name = "pictureBoxOK";
			this.pictureBoxOK.Size = new Size(20, 20);
			this.pictureBoxOK.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBoxOK.TabIndex = 5;
			this.pictureBoxOK.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(268, 109);
			base.Controls.Add(this.textBoxMessage);
			base.Controls.Add(this.pictureBoxOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MessageForm";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "MessageForm";
			base.TopMost = true;
			base.FormClosing += new FormClosingEventHandler(this.MessageForm_FormClosing);
			base.Load += new EventHandler(this.MessageForm_Load);
			((ISupportInitialize)this.pictureBoxOK).EndInit();
			base.ResumeLayout(false);
		}
	}
}
