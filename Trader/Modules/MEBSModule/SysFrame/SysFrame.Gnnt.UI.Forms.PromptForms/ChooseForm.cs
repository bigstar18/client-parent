using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.UI.Forms.PromptForms
{
	public class ChooseForm : Form
	{
		public delegate void ShowMixForm();
		private IContainer components;
		private Button buttonOK;
		private Button buttonCancel;
		private Label label1;
		public ChooseForm.ShowMixForm showMixForm;
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
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.label1 = new Label();
			base.SuspendLayout();
			this.buttonOK.Location = new Point(66, 107);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "我已开户";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Location = new Point(202, 107);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.label1.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label1.ForeColor = Color.Red;
			this.label1.Location = new Point(32, 21);
			this.label1.Name = "label1";
			this.label1.Size = new Size(296, 70);
			this.label1.TabIndex = 1;
			this.label1.Text = "提示：没有开户或账户没有权限，如果您已开户，请点击“我已开户”进行账户绑定，如果没有开户请先开户！";
			this.label1.TextAlign = ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(358, 166);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Name = "ChooseForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "选择去向";
			base.Load += new EventHandler(this.ChooseForm_Load);
			base.ResumeLayout(false);
		}
		public ChooseForm()
		{
			this.InitializeComponent();
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.showMixForm != null)
			{
				this.showMixForm();
			}
			base.DialogResult = DialogResult.OK;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}
		private void ChooseForm_Load(object sender, EventArgs e)
		{
			ScaleForm.ScaleForms(this);
		}
	}
}
