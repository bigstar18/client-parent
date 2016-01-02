using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class MarketForm : Form
	{
		private IContainer components;
		internal Panel MainPanel;
		internal Label labelName;
		internal Label labelId;
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
			this.MainPanel = new Panel();
			this.labelName = new Label();
			this.labelId = new Label();
			this.MainPanel.SuspendLayout();
			base.SuspendLayout();
			this.MainPanel.AutoScroll = true;
			this.MainPanel.AutoScrollMinSize = new Size(220, 300);
			this.MainPanel.BackColor = Color.Black;
			this.MainPanel.Controls.Add(this.labelName);
			this.MainPanel.Controls.Add(this.labelId);
			this.MainPanel.Dock = DockStyle.Fill;
			this.MainPanel.ForeColor = Color.White;
			this.MainPanel.Location = new Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new Size(230, 300);
			this.MainPanel.TabIndex = 0;
			this.labelName.AutoSize = true;
			this.labelName.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelName.ForeColor = Color.Yellow;
			this.labelName.Location = new Point(110, 10);
			this.labelName.Name = "label2";
			this.labelName.Size = new Size(76, 16);
			this.labelName.TabIndex = 2;
			this.labelName.Text = "市场名称";
			this.labelId.AutoSize = true;
			this.labelId.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelId.ForeColor = Color.Yellow;
			this.labelId.Location = new Point(10, 10);
			this.labelId.Name = "label1";
			this.labelId.Size = new Size(76, 16);
			this.labelId.TabIndex = 1;
			this.labelId.Text = "市场编号";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(230, 300);
			base.Controls.Add(this.MainPanel);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "MarketForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "所有市场信息";
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			base.ResumeLayout(false);
		}
		public MarketForm()
		{
			this.InitializeComponent();
		}
	}
}
