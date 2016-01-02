using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace ToolsLibrary
{
	public class FrmWaiting : Form
	{
		private IContainer components;
		private LoadingCircle loadingCircle1;
		private Label label1;
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
			this.label1 = new Label();
			this.loadingCircle1 = new LoadingCircle();
			base.SuspendLayout();
			this.label1.Dock = DockStyle.Top;
			this.label1.ForeColor = Color.BurlyWood;
			this.label1.Location = new Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(138, 25);
			this.label1.TabIndex = 1;
			this.label1.Text = "系统载入中......";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.loadingCircle1.Active = false;
			this.loadingCircle1.Color = Color.DarkOrange;
			this.loadingCircle1.Dock = DockStyle.Bottom;
			this.loadingCircle1.InnerCircleRadius = 6;
			this.loadingCircle1.Location = new Point(0, 28);
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.NumberSpoke = 9;
			this.loadingCircle1.OuterCircleRadius = 7;
			this.loadingCircle1.RotationSpeed = 200;
			this.loadingCircle1.Size = new Size(138, 83);
			this.loadingCircle1.SpokeThickness = 4;
			this.loadingCircle1.StylePreset = LoadingCircle.StylePresets.Firefox;
			this.loadingCircle1.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.DimGray;
			base.ClientSize = new Size(138, 111);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.loadingCircle1);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "FrmWaiting";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "FrmWaiting";
			base.TopMost = true;
			base.Load += new EventHandler(this.FrmWaiting_Load);
			base.ResumeLayout(false);
		}
		public FrmWaiting()
		{
			this.InitializeComponent();
		}
		private void FrmWaiting_Load(object sender, EventArgs e)
		{
			this.loadingCircle1.Active = true;
			this.loadingCircle1.InnerCircleRadius = 15;
			this.loadingCircle1.NumberSpoke = 15;
			this.loadingCircle1.OuterCircleRadius = 20;
		}
	}
}
