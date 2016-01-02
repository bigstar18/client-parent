using Gnnt.MixAccountPlugin;
using YrdceClient.Yrdce.Common.Operation.Manager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary;
namespace YrdceClient.Yrdce.UI.Forms.UserControls
{
	public class AccountInfoControl : UserControl
	{
		private OperationManager operationManager = OperationManager.GetInstance();
		private IContainer components;
		private GroupBox groupBoxAccountInfo;
		private RadioButton radioButton;
		private Label labelAccount;
		private PasswordTextBox textBoxPassWord;
		private TextBox textBoxAccount;
		private Label labelPassword;
		private Button buttonCheck;
		private Button buttonDel;
		public AccountInfoControl()
		{
			this.InitializeComponent();
		}
		private void buttonCheck_Click(object sender, EventArgs e)
		{
			UserInfo userInfo = new UserInfo();
			string text = this.textBoxAccount.Text;
			this.textBoxPassWord.CheckPass = true;
			string text2 = this.textBoxPassWord.Text;
			this.textBoxPassWord.CheckPass = false;
			userInfo.UserID=text;
			userInfo.PassWord=text2;
			bool flag = this.operationManager.bindAccountOperation.CheckAccountInfo(userInfo);
			if (flag)
			{
				this.buttonCheck.Enabled = false;
				this.textBoxAccount.Enabled = false;
				this.textBoxPassWord.Enabled = false;
				this.radioButton.Enabled = true;
				if (this.operationManager.bindAccountOperation.checkAccountCallBack != null)
				{
					this.operationManager.bindAccountOperation.checkAccountCallBack(true, userInfo);
				}
			}
		}
		private void buttonDel_Click(object sender, EventArgs e)
		{
			UserInfo userInfo = new UserInfo();
			string text = this.textBoxAccount.Text;
			string text2 = this.textBoxPassWord.Text;
			userInfo.UserID=text;
			userInfo.PassWord=text2;
			base.Parent.Controls.Remove(this);
			if (this.operationManager.bindAccountOperation.removeControlCallBack != null)
			{
				bool isChecked = false;
				if (this.radioButton.Checked)
				{
					isChecked = true;
				}
				this.operationManager.bindAccountOperation.removeControlCallBack(isChecked);
			}
			if (this.operationManager.bindAccountOperation.checkAccountCallBack != null && !this.buttonCheck.Enabled)
			{
				this.operationManager.bindAccountOperation.checkAccountCallBack(false, userInfo);
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
			this.groupBoxAccountInfo = new GroupBox();
			this.radioButton = new RadioButton();
			this.labelAccount = new Label();
			this.textBoxPassWord = new PasswordTextBox();
			this.textBoxAccount = new TextBox();
			this.labelPassword = new Label();
			this.buttonDel = new Button();
			this.buttonCheck = new Button();
			this.groupBoxAccountInfo.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxAccountInfo.Controls.Add(this.radioButton);
			this.groupBoxAccountInfo.Controls.Add(this.labelAccount);
			this.groupBoxAccountInfo.Controls.Add(this.textBoxPassWord);
			this.groupBoxAccountInfo.Controls.Add(this.textBoxAccount);
			this.groupBoxAccountInfo.Controls.Add(this.labelPassword);
			this.groupBoxAccountInfo.Controls.Add(this.buttonDel);
			this.groupBoxAccountInfo.Controls.Add(this.buttonCheck);
			this.groupBoxAccountInfo.Location = new Point(45, 10);
			this.groupBoxAccountInfo.Name = "groupBoxAccountInfo";
			this.groupBoxAccountInfo.Size = new Size(310, 100);
			this.groupBoxAccountInfo.TabIndex = 0;
			this.groupBoxAccountInfo.TabStop = false;
			this.groupBoxAccountInfo.Text = "账号信息";
			this.radioButton.AutoSize = true;
			this.radioButton.Enabled = false;
			this.radioButton.Location = new Point(19, 76);
			this.radioButton.Name = "radioButton";
			this.radioButton.Size = new Size(155, 16);
			this.radioButton.TabIndex = 4;
			this.radioButton.TabStop = true;
			this.radioButton.Text = "以此账号作为主登录账号";
			this.radioButton.UseVisualStyleBackColor = true;
			this.labelAccount.AutoSize = true;
			this.labelAccount.Location = new Point(17, 23);
			this.labelAccount.Name = "labelAccount";
			this.labelAccount.Size = new Size(53, 12);
			this.labelAccount.TabIndex = 1;
			this.labelAccount.Text = "用户名：";
			this.textBoxPassWord.CheckPass = false;
			this.textBoxPassWord.Location = new Point(76, 48);
			this.textBoxPassWord.Name = "textBoxPassWord";
			this.textBoxPassWord.Size = new Size(120, 21);
			this.textBoxPassWord.TabIndex = 1;
			this.textBoxPassWord.UseSystemPasswordChar = true;
			this.textBoxAccount.Location = new Point(76, 20);
			this.textBoxAccount.Name = "textBoxAccount";
			this.textBoxAccount.Size = new Size(120, 21);
			this.textBoxAccount.TabIndex = 0;
			this.labelPassword.AutoSize = true;
			this.labelPassword.Location = new Point(17, 51);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new Size(53, 12);
			this.labelPassword.TabIndex = 3;
			this.labelPassword.Text = "密  码：";
			this.buttonDel.Location = new Point(210, 51);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new Size(75, 23);
			this.buttonDel.TabIndex = 3;
			this.buttonDel.Text = "删除";
			this.buttonDel.UseVisualStyleBackColor = true;
			this.buttonDel.Click += new EventHandler(this.buttonDel_Click);
			this.buttonCheck.Location = new Point(210, 18);
			this.buttonCheck.Name = "buttonCheck";
			this.buttonCheck.Size = new Size(75, 23);
			this.buttonCheck.TabIndex = 2;
			this.buttonCheck.Text = "校验";
			this.buttonCheck.UseVisualStyleBackColor = true;
			this.buttonCheck.Click += new EventHandler(this.buttonCheck_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxAccountInfo);
			base.Name = "AccountInfoControl";
			base.Size = new Size(400, 120);
			this.groupBoxAccountInfo.ResumeLayout(false);
			this.groupBoxAccountInfo.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
