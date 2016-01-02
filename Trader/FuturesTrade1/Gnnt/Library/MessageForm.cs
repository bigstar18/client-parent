namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;

    public class MessageForm : Form
    {
        private MyButton buttonCancel;
        private MyButton buttonOK;
        private IContainer components;
        public bool isOK;
        private PictureBox pictureBoxOK;
        private Label textBoxMessage;

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
            }
            else if (formStyle == 1)
            {
                this.pictureBoxOK.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_gantanhao");
                this.buttonOK.Left = (base.Width - this.buttonOK.Width) / 2;
                this.buttonCancel.Visible = false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.isOK = false;
            base.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.isOK = true;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MessageForm));
            this.buttonOK = new MyButton();
            this.buttonCancel = new MyButton();
            this.textBoxMessage = new Label();
            this.pictureBoxOK = new PictureBox();
            ((ISupportInitialize)this.pictureBoxOK).BeginInit();
            base.SuspendLayout();
            this.buttonOK.Location = new Point(40, 80);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x99, 80);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.textBoxMessage.Location = new Point(0x25, 9);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new Size(0xc9, 0x3b);
            this.textBoxMessage.TabIndex = 4;
            this.textBoxMessage.TextAlign = ContentAlignment.MiddleLeft;
            this.pictureBoxOK.Location = new Point(10, 7);
            this.pictureBoxOK.Name = "pictureBoxOK";
            this.pictureBoxOK.Size = new Size(20, 0x15);
            this.pictureBoxOK.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxOK.TabIndex = 5;
            this.pictureBoxOK.TabStop = false;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x10c, 0x7b);
            base.Controls.Add(this.textBoxMessage);
            base.Controls.Add(this.pictureBoxOK);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Icon = (Icon)manager.GetObject("$this.Icon");
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

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            ScaleForm.ScaleForms(this);
        }

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
    }
}
