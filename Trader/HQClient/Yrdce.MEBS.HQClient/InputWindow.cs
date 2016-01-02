using DIYForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient
{
	public class InputWindow : MyForm
	{
		private IContainer components;
		private Label InputLabel;
		private TextBox InputBox;
		private Button ConfirmBtn;
		private Button CancelBtn;
		private int _KValue;
		private int _KType;
		public int KValue
		{
			get
			{
				if (this.InputBox.Text != "")
				{
					return Convert.ToInt32(this.InputBox.Text);
				}
				return 0;
			}
			set
			{
				this._KValue = value;
			}
		}
		public int KType
		{
			get
			{
				return this._KType;
			}
			set
			{
				this._KType = value;
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
			this.InputLabel = new Label();
			this.InputBox = new TextBox();
			this.ConfirmBtn = new Button();
			this.CancelBtn = new Button();
			base.SuspendLayout();
			this.InputLabel.AutoSize = true;
			this.InputLabel.BackColor = Color.Transparent;
			this.InputLabel.Location = new Point(39, 36);
			this.InputLabel.Name = "InputLabel";
			this.InputLabel.Size = new Size(71, 12);
			this.InputLabel.TabIndex = 6;
			this.InputLabel.Text = "请输入天数:";
			this.InputBox.Location = new Point(39, 51);
			this.InputBox.Name = "InputBox";
			this.InputBox.Size = new Size(170, 21);
			this.InputBox.TabIndex = 7;
			this.InputBox.KeyPress += new KeyPressEventHandler(this.InputBox_KeyPress);
			this.ConfirmBtn.BackColor = Color.White;
			this.ConfirmBtn.FlatStyle = FlatStyle.Popup;
			this.ConfirmBtn.Location = new Point(50, 75);
			this.ConfirmBtn.Name = "ConfirmBtn";
			this.ConfirmBtn.Size = new Size(47, 23);
			this.ConfirmBtn.TabIndex = 8;
			this.ConfirmBtn.Text = "确定";
			this.ConfirmBtn.UseVisualStyleBackColor = false;
			this.ConfirmBtn.Click += new EventHandler(this.ConfirmBtn_Click);
			this.CancelBtn.BackColor = Color.White;
			this.CancelBtn.DialogResult = DialogResult.Cancel;
			this.CancelBtn.FlatStyle = FlatStyle.Popup;
			this.CancelBtn.Location = new Point(145, 75);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new Size(47, 23);
			this.CancelBtn.TabIndex = 9;
			this.CancelBtn.Text = "取消";
			this.CancelBtn.UseVisualStyleBackColor = false;
			this.CancelBtn.Click += new EventHandler(this.CancelBtn_Click);
			base.AcceptButton = this.ConfirmBtn;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.CancelBtn;
			base.ClientSize = new Size(247, 110);
			base.Controls.Add(this.CancelBtn);
			base.Controls.Add(this.ConfirmBtn);
			base.Controls.Add(this.InputBox);
			base.Controls.Add(this.InputLabel);
			base.DIYMaximizeBox=false;
			base.DIYMinimizeBox=false;
			base.Name = "InputWindow";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "InputWindow";
			base.Controls.SetChildIndex(this.InputLabel, 0);
			base.Controls.SetChildIndex(this.InputBox, 0);
			base.Controls.SetChildIndex(this.ConfirmBtn, 0);
			base.Controls.SetChildIndex(this.CancelBtn, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public InputWindow(int type)
		{
			this.InitializeComponent();
			this.KType = type;
			if (this.KType == 1)
			{
				this.Text = "任意天技术分析";
				this.InputLabel.Text = "请输入天数：";
			}
			else
			{
				this.Text = "任意分钟技术分析";
				this.InputLabel.Text = "请输入分钟数：";
			}
			this.InputBox.Focus();
		}
		private void ConfirmBtn_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			base.Close();
		}
		private void CancelBtn_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
		}
		public static bool isNumberic(string _string)
		{
			if (string.IsNullOrEmpty(_string))
			{
				return false;
			}
			for (int i = 0; i < _string.Length; i++)
			{
				char c = _string[i];
				if (!char.IsDigit(c))
				{
					return false;
				}
			}
			return true;
		}
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				base.DialogResult = DialogResult.No;
				base.Close();
			}
			if (e.KeyData == Keys.Return)
			{
				base.DialogResult = DialogResult.Yes;
				base.Close();
			}
		}
		private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = (e.KeyChar < '0' || e.KeyChar > '9');
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
				return;
			}
			if (this.InputBox.Text.Length > 5)
			{
				e.Handled = true;
			}
		}
	}
}
