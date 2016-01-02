using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace SysFrame.Gnnt.UI.Forms.UserControls
{
	public class ProgressControl : UserControl
	{
		private IContainer components;
		private ProgressBar LoginProgress;
		private Label LogonInfo;
		public ProgressControl()
		{
			this.InitializeComponent();
		}
		public void SetProgressCtrlInfo(string str, int val)
		{
			if (this.LoginProgress.Value + val > 100)
			{
				this.LoginProgress.Value = 100;
			}
			else
			{
				this.LoginProgress.Value += val;
			}
			this.LogonInfo.Text = str;
		}
		public void InitProgressCtrl()
		{
			this.LoginProgress.Value = 0;
			this.LogonInfo.Text = "";
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
			this.LoginProgress = new ProgressBar();
			this.LogonInfo = new Label();
			base.SuspendLayout();
			this.LoginProgress.Dock = DockStyle.Bottom;
			this.LoginProgress.Location = new Point(0, 20);
			this.LoginProgress.Name = "LoginProgress";
			this.LoginProgress.Size = new Size(384, 17);
			this.LoginProgress.Style = ProgressBarStyle.Continuous;
			this.LoginProgress.TabIndex = 1;
			this.LoginProgress.Tag = "";
			this.LogonInfo.BackColor = Color.Transparent;
			this.LogonInfo.Dock = DockStyle.Top;
			this.LogonInfo.Font = new Font("宋体", 10f);
			this.LogonInfo.ForeColor = Color.White;
			this.LogonInfo.Location = new Point(0, 0);
			this.LogonInfo.Name = "LogonInfo";
			this.LogonInfo.Size = new Size(384, 17);
			this.LogonInfo.TabIndex = 2;
			this.LogonInfo.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.Transparent;
			base.Controls.Add(this.LogonInfo);
			base.Controls.Add(this.LoginProgress);
			base.Name = "ProgressControl";
			base.Size = new Size(384, 37);
			base.ResumeLayout(false);
		}
	}
}
