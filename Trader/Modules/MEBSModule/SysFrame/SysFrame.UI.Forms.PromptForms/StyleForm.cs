using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.Manager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace SysFrame.UI.Forms.PromptForms
{
	public class StyleForm : Form
	{
		private IContainer components;
		private Button buttonOK;
		private Button buttonAuto;
		private PictureBox pictureBoxStyle;
		private Panel panelStyle;
		private RadioButton radioButtonT8;
		private RadioButton radioButtonT1;
		private RadioButton radioButtonAuto;
		private int styleNun;
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
			this.buttonAuto = new Button();
			this.pictureBoxStyle = new PictureBox();
			this.panelStyle = new Panel();
			this.radioButtonT8 = new RadioButton();
			this.radioButtonT1 = new RadioButton();
			this.radioButtonAuto = new RadioButton();
			((ISupportInitialize)this.pictureBoxStyle).BeginInit();
			this.panelStyle.SuspendLayout();
			base.SuspendLayout();
			this.buttonOK.Location = new Point(195, 473);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "确定";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonAuto.DialogResult = DialogResult.Cancel;
			this.buttonAuto.Location = new Point(508, 473);
			this.buttonAuto.Name = "buttonAuto";
			this.buttonAuto.Size = new Size(75, 23);
			this.buttonAuto.TabIndex = 1;
			this.buttonAuto.Text = "取消";
			this.buttonAuto.UseVisualStyleBackColor = true;
			this.pictureBoxStyle.Location = new Point(12, 12);
			this.pictureBoxStyle.Name = "pictureBoxStyle";
			this.pictureBoxStyle.Size = new Size(792, 390);
			this.pictureBoxStyle.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBoxStyle.TabIndex = 2;
			this.pictureBoxStyle.TabStop = false;
			this.panelStyle.Controls.Add(this.radioButtonT8);
			this.panelStyle.Controls.Add(this.radioButtonT1);
			this.panelStyle.Controls.Add(this.radioButtonAuto);
			this.panelStyle.Location = new Point(90, 418);
			this.panelStyle.Name = "panelStyle";
			this.panelStyle.Size = new Size(612, 33);
			this.panelStyle.TabIndex = 3;
			this.radioButtonT8.AutoSize = true;
			this.radioButtonT8.Location = new Point(477, 9);
			this.radioButtonT8.Name = "radioButtonT8";
			this.radioButtonT8.Size = new Size(59, 16);
			this.radioButtonT8.TabIndex = 2;
			this.radioButtonT8.TabStop = true;
			this.radioButtonT8.Text = "T8风格";
			this.radioButtonT8.UseVisualStyleBackColor = true;
			this.radioButtonT8.CheckedChanged += new EventHandler(this.radioButtonAuto_CheckedChanged);
			this.radioButtonT1.AutoSize = true;
			this.radioButtonT1.Location = new Point(271, 9);
			this.radioButtonT1.Name = "radioButtonT1";
			this.radioButtonT1.Size = new Size(59, 16);
			this.radioButtonT1.TabIndex = 1;
			this.radioButtonT1.TabStop = true;
			this.radioButtonT1.Text = "T1风格";
			this.radioButtonT1.UseVisualStyleBackColor = true;
			this.radioButtonT1.CheckedChanged += new EventHandler(this.radioButtonAuto_CheckedChanged);
			this.radioButtonAuto.AutoSize = true;
			this.radioButtonAuto.Location = new Point(30, 9);
			this.radioButtonAuto.Name = "radioButtonAuto";
			this.radioButtonAuto.Size = new Size(71, 16);
			this.radioButtonAuto.TabIndex = 0;
			this.radioButtonAuto.TabStop = true;
			this.radioButtonAuto.Text = "标准风格";
			this.radioButtonAuto.UseVisualStyleBackColor = true;
			this.radioButtonAuto.CheckedChanged += new EventHandler(this.radioButtonAuto_CheckedChanged);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.ControlLightLight;
			base.CancelButton = this.buttonAuto;
			base.ClientSize = new Size(816, 520);
			base.Controls.Add(this.panelStyle);
			base.Controls.Add(this.pictureBoxStyle);
			base.Controls.Add(this.buttonAuto);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "StyleForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "选择系统的风格";
			base.Load += new EventHandler(this.StyleForm_Load);
			((ISupportInitialize)this.pictureBoxStyle).EndInit();
			this.panelStyle.ResumeLayout(false);
			this.panelStyle.PerformLayout();
			base.ResumeLayout(false);
		}
		public StyleForm()
		{
			this.InitializeComponent();
		}
		private void StyleForm_Load(object sender, EventArgs e)
		{
			base.TopMost = true;
			base.TopMost = false;
			this.SetControlText();
			ScaleForm.ScaleForms(this);
		}
		private void SetControlText()
		{
			base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
			this.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("StyleTitle");
			this.radioButtonAuto.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("StyleRadioButtonAuto");
			this.radioButtonT1.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("StyleradioButtonT1");
			this.radioButtonT8.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("StyleradioButtonT8");
			this.buttonOK.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_OK");
			this.buttonAuto.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_CANCEL");
			int num = Tools.StrToInt((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["FormStyle"], 0);
			if (num == 0)
			{
				this.radioButtonAuto.Checked = true;
				return;
			}
			if (num == 1)
			{
				this.radioButtonT1.Checked = true;
				return;
			}
			if (num == 8)
			{
				this.radioButtonT8.Checked = true;
			}
		}
		private void radioButtonAuto_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonAuto.Checked)
			{
				this.pictureBoxStyle.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("T0");
			}
			if (this.radioButtonT1.Checked)
			{
				this.pictureBoxStyle.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("T1");
			}
			if (this.radioButtonT8.Checked)
			{
				this.pictureBoxStyle.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("T8");
			}
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (Tools.StrToBool((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["FirstRunApp"], false))
			{
				Global.Modules.get_Plugins().get_ConfigurationInfo().updateValue("Systems", "FirstRunApp", "false");
			}
			int num = Tools.StrToInt((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["FormStyle"], 0);
			if ((num != 0 && this.radioButtonAuto.Checked) || (num != 1 && this.radioButtonT1.Checked) || (num != 8 && this.radioButtonT8.Checked))
			{
				if (this.radioButtonAuto.Checked)
				{
					num = 0;
				}
				if (this.radioButtonT1.Checked)
				{
					num = 1;
				}
				if (this.radioButtonT8.Checked)
				{
					num = 8;
				}
				Global.Modules.get_Plugins().get_ConfigurationInfo().updateValue("Systems", "FormStyle", num.ToString());
				OperationManager.GetInstance().FormStyle = num;
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}
	}
}
