using Gnnt.MixAccountPlugin;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.Gnnt.UI.Forms.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.UI.Forms.PromptForms
{
	public class MixAccountForm : Form
	{
		private OperationManager operationManager = OperationManager.GetInstance();
		private List<RadioButton> CheckRadioButtons = new List<RadioButton>();
		private int GapHeight;
		private int AccountInfoCount;
		private int CheckedCount;
		private List<UserInfo> UserInfoList = new List<UserInfo>();
		private string MainAccount = string.Empty;
		private int CanBindCount = 3;
		private IContainer components;
		private Button buttonOK;
		private Button buttonCancel;
		private GroupBox groupBoxAccountInfo;
		private Label labelAccount1;
		private TextBox textBoxAccount1;
		private RadioButton radioButtonMainAccount;
		private Button btnAddAccount;
		private Panel panelTop;
		private Panel panelBottom;
		private RadioButton radioButtonMixFunds;
		private RadioButton radioButtonMixLogin;
		public MixAccountForm()
		{
			this.InitializeComponent();
			this.operationManager.bindAccountOperation.removeControlCallBack = new MixAccountOperation.RemoveControlCallBack(this.RemoveControl);
			this.operationManager.bindAccountOperation.checkAccountCallBack = new MixAccountOperation.CheckAccountCallBack(this.CheckAccount);
		}
		private void BindAccountForm_Load(object sender, EventArgs e)
		{
			this.radioButtonMainAccount.CheckedChanged += new EventHandler(this.CheckedChanged);
			this.CheckRadioButtons.Add(this.radioButtonMainAccount);
			this.radioButtonMainAccount.Checked = true;
			ScaleForm.ScaleForms(this);
		}
		private void InitRadioButtons()
		{
			foreach (Control control in base.Controls)
			{
				if (control.Controls.Count > 0)
				{
					Control control2 = control.Controls[0];
					if (control2 is GroupBox)
					{
						foreach (Control control3 in control2.Controls)
						{
							if (control3 is RadioButton)
							{
								RadioButton radioButton = (RadioButton)control3;
								radioButton.CheckedChanged += new EventHandler(this.CheckedChanged);
								if (!this.CheckRadioButtons.Contains(radioButton))
								{
									this.CheckRadioButtons.Add(radioButton);
								}
							}
						}
					}
				}
			}
		}
		private void CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			if (!radioButton.ContainsFocus)
			{
				return;
			}
			foreach (RadioButton current in this.CheckRadioButtons)
			{
				if (current != radioButton)
				{
					current.Checked = false;
				}
			}
			GroupBox groupBox = (GroupBox)radioButton.Parent;
			if (groupBox != null)
			{
				foreach (Control control in groupBox.Controls)
				{
					if (control is TextBox)
					{
						TextBox textBox = (TextBox)control;
						this.MainAccount = textBox.Text;
					}
				}
			}
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			MixAccountInfo mixAccountInfo = new MixAccountInfo();
			mixAccountInfo.set_MainAccount(this.MainAccount);
			mixAccountInfo.set_ChildAccounts(this.UserInfoList);
			int mapType = 0;
			if (this.radioButtonMixLogin.Checked)
			{
				mapType = 1;
			}
			mixAccountInfo.set_MapType(mapType);
			mixAccountInfo.set_UserID(Global.Modules.get_Plugins().get_SysLogonInfo().TraderID);
			mixAccountInfo.set_ModuleID("");
			if (this.operationManager.bindAccountOperation.MixAccount(mixAccountInfo))
			{
				base.DialogResult = DialogResult.OK;
				return;
			}
			MessageBox.Show("绑定失败！");
		}
		private void btnAddAccount_Click(object sender, EventArgs e)
		{
			if (this.AccountInfoCount <= this.CanBindCount)
			{
				AccountInfoControl accountInfoControl = new AccountInfoControl();
				this.GapHeight = accountInfoControl.Height;
				accountInfoControl.Dock = DockStyle.Top;
				base.Controls.Add(accountInfoControl);
				base.Controls.Remove(this.panelTop);
				base.Controls.Add(this.panelTop);
				this.AccountInfoCount++;
				this.buttonOK.Enabled = false;
				this.InitRadioButtons();
				if (this.AccountInfoCount == this.CanBindCount)
				{
					this.btnAddAccount.Enabled = false;
				}
			}
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void RemoveControl(bool isCheckedRemove)
		{
			this.AccountInfoCount--;
			if (this.AccountInfoCount <= this.CanBindCount)
			{
				this.btnAddAccount.Enabled = true;
			}
			base.Height -= this.GapHeight;
			if (isCheckedRemove)
			{
				this.CheckRadioButtons[0].Checked = true;
			}
			if (this.CheckedCount >= this.AccountInfoCount && this.AccountInfoCount != 0)
			{
				this.buttonOK.Enabled = true;
				return;
			}
			this.buttonOK.Enabled = false;
		}
		private void CheckAccount(bool isChecked, UserInfo userinfo)
		{
			if (isChecked)
			{
				this.CheckedCount++;
				if (!this.UserInfoList.Contains(userinfo))
				{
					this.UserInfoList.Add(userinfo);
				}
			}
			else
			{
				this.CheckedCount--;
				if (this.UserInfoList.Contains(userinfo))
				{
					this.UserInfoList.Remove(userinfo);
				}
			}
			if (this.CheckedCount >= this.AccountInfoCount && this.AccountInfoCount != 0)
			{
				this.buttonOK.Enabled = true;
				return;
			}
			this.buttonOK.Enabled = false;
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
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBoxAccountInfo = new GroupBox();
			this.labelAccount1 = new Label();
			this.radioButtonMainAccount = new RadioButton();
			this.textBoxAccount1 = new TextBox();
			this.btnAddAccount = new Button();
			this.panelTop = new Panel();
			this.radioButtonMixFunds = new RadioButton();
			this.radioButtonMixLogin = new RadioButton();
			this.panelBottom = new Panel();
			this.groupBoxAccountInfo.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.panelBottom.SuspendLayout();
			base.SuspendLayout();
			this.buttonOK.Enabled = false;
			this.buttonOK.Location = new Point(85, 18);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "绑定";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Location = new Point(237, 18);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.groupBoxAccountInfo.Controls.Add(this.labelAccount1);
			this.groupBoxAccountInfo.Controls.Add(this.radioButtonMainAccount);
			this.groupBoxAccountInfo.Controls.Add(this.textBoxAccount1);
			this.groupBoxAccountInfo.Location = new Point(39, 40);
			this.groupBoxAccountInfo.Name = "groupBoxAccountInfo";
			this.groupBoxAccountInfo.Size = new Size(230, 71);
			this.groupBoxAccountInfo.TabIndex = 6;
			this.groupBoxAccountInfo.TabStop = false;
			this.groupBoxAccountInfo.Text = "当前账号信息";
			this.labelAccount1.AutoSize = true;
			this.labelAccount1.Location = new Point(17, 26);
			this.labelAccount1.Name = "labelAccount1";
			this.labelAccount1.Size = new Size(53, 12);
			this.labelAccount1.TabIndex = 2;
			this.labelAccount1.Text = "用户名：";
			this.radioButtonMainAccount.AutoSize = true;
			this.radioButtonMainAccount.Location = new Point(21, 49);
			this.radioButtonMainAccount.Name = "radioButtonMainAccount";
			this.radioButtonMainAccount.Size = new Size(155, 16);
			this.radioButtonMainAccount.TabIndex = 3;
			this.radioButtonMainAccount.Text = "以此账号作为主登录账号";
			this.radioButtonMainAccount.UseVisualStyleBackColor = true;
			this.textBoxAccount1.Location = new Point(76, 23);
			this.textBoxAccount1.Name = "textBoxAccount1";
			this.textBoxAccount1.Size = new Size(110, 21);
			this.textBoxAccount1.TabIndex = 2;
			this.btnAddAccount.Location = new Point(275, 66);
			this.btnAddAccount.Name = "btnAddAccount";
			this.btnAddAccount.Size = new Size(95, 23);
			this.btnAddAccount.TabIndex = 4;
			this.btnAddAccount.Text = "新增合并账号";
			this.btnAddAccount.UseVisualStyleBackColor = true;
			this.btnAddAccount.Click += new EventHandler(this.btnAddAccount_Click);
			this.panelTop.Controls.Add(this.radioButtonMixFunds);
			this.panelTop.Controls.Add(this.radioButtonMixLogin);
			this.panelTop.Controls.Add(this.groupBoxAccountInfo);
			this.panelTop.Controls.Add(this.btnAddAccount);
			this.panelTop.Dock = DockStyle.Top;
			this.panelTop.Location = new Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new Size(400, 120);
			this.panelTop.TabIndex = 7;
			this.radioButtonMixFunds.AutoSize = true;
			this.radioButtonMixFunds.Location = new Point(203, 11);
			this.radioButtonMixFunds.Name = "radioButtonMixFunds";
			this.radioButtonMixFunds.Size = new Size(131, 16);
			this.radioButtonMixFunds.TabIndex = 1;
			this.radioButtonMixFunds.Text = "合并交易商资金账户";
			this.radioButtonMixFunds.UseVisualStyleBackColor = true;
			this.radioButtonMixLogin.AutoSize = true;
			this.radioButtonMixLogin.Checked = true;
			this.radioButtonMixLogin.Location = new Point(40, 11);
			this.radioButtonMixLogin.Name = "radioButtonMixLogin";
			this.radioButtonMixLogin.Size = new Size(131, 16);
			this.radioButtonMixLogin.TabIndex = 0;
			this.radioButtonMixLogin.TabStop = true;
			this.radioButtonMixLogin.Text = "合并交易员登陆账户";
			this.radioButtonMixLogin.UseVisualStyleBackColor = true;
			this.panelBottom.Controls.Add(this.buttonOK);
			this.panelBottom.Controls.Add(this.buttonCancel);
			this.panelBottom.Dock = DockStyle.Bottom;
			this.panelBottom.Location = new Point(0, 120);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new Size(400, 53);
			this.panelBottom.TabIndex = 8;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.ClientSize = new Size(400, 173);
			base.Controls.Add(this.panelBottom);
			base.Controls.Add(this.panelTop);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Name = "MixAccountForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "账号绑定";
			base.Load += new EventHandler(this.BindAccountForm_Load);
			this.groupBoxAccountInfo.ResumeLayout(false);
			this.groupBoxAccountInfo.PerformLayout();
			this.panelTop.ResumeLayout(false);
			this.panelTop.PerformLayout();
			this.panelBottom.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
