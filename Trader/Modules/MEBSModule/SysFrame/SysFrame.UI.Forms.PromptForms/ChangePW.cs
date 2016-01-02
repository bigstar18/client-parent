using DIYForm;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation;
using SysFrame.Gnnt.Common.Operation.Manager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.UI.Forms.PromptForms
{
	public class ChangePW : MyForm
	{
		private IContainer components;
		private Label LabOldPW;
		private Label labNewPW;
		private Label labConfirmPW;
		private TextBox textOldPW;
		private TextBox textNewPW;
		private TextBox textConfirmPW;
		private Button buttonSubmit;
		private Button buttonCancel;
		private Label labelMessage;
		private bool myForceChangePassword;
		private bool clickExitButton = true;
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
			this.LabOldPW = new Label();
			this.labNewPW = new Label();
			this.labConfirmPW = new Label();
			this.textOldPW = new TextBox();
			this.textNewPW = new TextBox();
			this.textConfirmPW = new TextBox();
			this.buttonSubmit = new Button();
			this.buttonCancel = new Button();
			this.labelMessage = new Label();
			base.SuspendLayout();
			this.LabOldPW.AutoSize = true;
			this.LabOldPW.Location = new Point(71, 43);
			this.LabOldPW.Name = "LabOldPW";
			this.LabOldPW.Size = new Size(53, 12);
			this.LabOldPW.TabIndex = 0;
			this.LabOldPW.Text = "原密码：";
			this.labNewPW.AutoSize = true;
			this.labNewPW.Location = new Point(71, 91);
			this.labNewPW.Name = "labNewPW";
			this.labNewPW.Size = new Size(53, 12);
			this.labNewPW.TabIndex = 1;
			this.labNewPW.Text = "新密码：";
			this.labConfirmPW.AutoSize = true;
			this.labConfirmPW.Location = new Point(47, 139);
			this.labConfirmPW.Name = "labConfirmPW";
			this.labConfirmPW.Size = new Size(77, 12);
			this.labConfirmPW.TabIndex = 2;
			this.labConfirmPW.Text = "重复新密码：";
			this.textOldPW.Location = new Point(143, 40);
			this.textOldPW.Name = "textOldPW";
			this.textOldPW.PasswordChar = '*';
			this.textOldPW.Size = new Size(100, 21);
			this.textOldPW.TabIndex = 3;
			this.textNewPW.Location = new Point(143, 88);
			this.textNewPW.MaxLength = 32;
			this.textNewPW.Name = "textNewPW";
			this.textNewPW.PasswordChar = '*';
			this.textNewPW.Size = new Size(100, 21);
			this.textNewPW.TabIndex = 4;
			this.textNewPW.KeyPress += new KeyPressEventHandler(this.textNewPW_KeyPress);
			this.textNewPW.Leave += new EventHandler(this.textNewPW_Leave);
			this.textConfirmPW.Location = new Point(143, 136);
			this.textConfirmPW.MaxLength = 32;
			this.textConfirmPW.Name = "textConfirmPW";
			this.textConfirmPW.PasswordChar = '*';
			this.textConfirmPW.Size = new Size(100, 21);
			this.textConfirmPW.TabIndex = 5;
			this.buttonSubmit.Location = new Point(70, 178);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new Size(63, 23);
			this.buttonSubmit.TabIndex = 6;
			this.buttonSubmit.Text = "确  认";
			this.buttonSubmit.UseVisualStyleBackColor = true;
			this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(180, 178);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(63, 23);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "取  消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new Point(47, 115);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new Size(0, 12);
			this.labelMessage.TabIndex = 8;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(345, 223);
			base.Controls.Add(this.labelMessage);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonSubmit);
			base.Controls.Add(this.textConfirmPW);
			base.Controls.Add(this.textNewPW);
			base.Controls.Add(this.textOldPW);
			base.Controls.Add(this.labConfirmPW);
			base.Controls.Add(this.labNewPW);
			base.Controls.Add(this.LabOldPW);
			base.set_DIYMaximizeBox(false);
			base.set_DIYMinimizeBox(false);
			base.Name = "ChangePW";
			base.set_SetSize(false);
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "修改密码";
			base.TopMost = true;
			base.FormClosed += new FormClosedEventHandler(this.ChangePW_FormClosed);
			base.Load += new EventHandler(this.ChangePW_Load);
			base.Controls.SetChildIndex(this.LabOldPW, 0);
			base.Controls.SetChildIndex(this.labNewPW, 0);
			base.Controls.SetChildIndex(this.labConfirmPW, 0);
			base.Controls.SetChildIndex(this.textOldPW, 0);
			base.Controls.SetChildIndex(this.textNewPW, 0);
			base.Controls.SetChildIndex(this.textConfirmPW, 0);
			base.Controls.SetChildIndex(this.buttonSubmit, 0);
			base.Controls.SetChildIndex(this.buttonCancel, 0);
			base.Controls.SetChildIndex(this.labelMessage, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ChangePW(bool forceChangePassword)
		{
			this.InitializeComponent();
			this.myForceChangePassword = forceChangePassword;
			OperationManager.GetInstance().changePassWordOperation.SetFocus = new ChangePasswordOperation.SetFocusCallBack(this.SetFouce);
		}
		private void buttonSubmit_Click(object sender, EventArgs e)
		{
			ChangePassWordInfo changePassWordInfo = new ChangePassWordInfo();
			changePassWordInfo.oldPwd = this.textOldPW.Text;
			changePassWordInfo.newPwd = this.textNewPW.Text;
			changePassWordInfo.confirmPwd = this.textConfirmPW.Text;
			bool flag = OperationManager.GetInstance().changePassWordOperation.ChangePassword(changePassWordInfo);
			if (flag)
			{
				this.clickExitButton = false;
				base.Close();
			}
		}
		private void SetFouce(short flag)
		{
			if (flag == 0)
			{
				this.textOldPW.Focus();
				return;
			}
			if (flag == 1)
			{
				this.textNewPW.Focus();
				return;
			}
			if (flag == 2)
			{
				this.textConfirmPW.Focus();
			}
		}
		private static bool IsNumeric(string str)
		{
			if (str == null || str.Length == 0)
			{
				return false;
			}
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			byte[] array = bytes;
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				if (b < 48 || b > 57)
				{
					return false;
				}
			}
			return true;
		}
		private static bool IsAlphabet(string str)
		{
			if (str == null || str.Length == 0)
			{
				return false;
			}
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			byte[] array = bytes;
			int i = 0;
			while (i < array.Length)
			{
				byte b = array[i];
				bool result;
				if (b < 65 || b > 122)
				{
					result = false;
				}
				else
				{
					if (b <= 90 || b >= 97)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.clickExitButton = false;
			base.Close();
		}
		private void SetControlText()
		{
			this.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_Form");
			this.LabOldPW.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_labOldPwd");
			this.labNewPW.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_labNewPwd");
			this.labConfirmPW.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_labConfirmPwd");
			this.buttonSubmit.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_buttonSubmit");
			this.labelMessage.Visible = false;
			if (this.myForceChangePassword)
			{
				this.buttonSubmit.Location = new Point(120, 178);
				this.buttonCancel.Visible = false;
				return;
			}
			this.buttonCancel.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_buttonCancel");
		}
		private void SetSkin()
		{
			base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
			this.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_Skin1");
			this.BackgroundImageLayout = ImageLayout.Stretch;
			this.buttonSubmit.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_Skin1");
			this.buttonCancel.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_Skin1");
			this.LabOldPW.BackColor = Color.Transparent;
			this.labNewPW.BackColor = Color.Transparent;
			this.labConfirmPW.BackColor = Color.Transparent;
			this.labelMessage.BackColor = Color.Transparent;
		}
		private void textNewPW_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_OnlyNumLetter");
				MessageBox.Show(@string);
				e.Handled = true;
			}
		}
		private void ChangePW_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (this.clickExitButton && this.myForceChangePassword)
			{
				base.DialogResult = DialogResult.No;
			}
		}
		private void textNewPW_Leave(object sender, EventArgs e)
		{
			ChangePassWordInfo changePassWordInfo = new ChangePassWordInfo();
			changePassWordInfo.oldPwd = this.textOldPW.Text;
			changePassWordInfo.newPwd = this.textNewPW.Text;
			bool flag = OperationManager.GetInstance().changePassWordOperation.VerifyNewPassWord(changePassWordInfo);
			if (flag)
			{
				this.labelMessage.Visible = true;
				this.labelMessage.ForeColor = Color.Red;
				this.labelMessage.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_ChangePW_Prompt");
				return;
			}
			this.labelMessage.Visible = false;
		}
		private void ChangePW_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.SetSkin();
			ScaleForm.ScaleForms(this);
		}
	}
}
