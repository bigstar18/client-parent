using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolStripRender.Properties;
namespace ToolStripRender
{
	public class Form1 : Form
	{
		private IContainer components;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripSeparator toolStripSeparator12;
		private ToolStripButton toolStripButton2;
		private ToolStripDropDownButton toolStripDropDownButton1;
		private ToolStripMenuItem toolStripMenuItem47;
		private ToolStripMenuItem toolStripMenuItem48;
		private ToolStripSeparator toolStripSeparator13;
		private ToolStripMenuItem toolStripMenuItem49;
		private ToolStripSplitButton toolStripSplitButton1;
		private ToolStripMenuItem toolStripMenuItem50;
		private ToolStripMenuItem toolStripMenuItem51;
		private ToolStripSeparator toolStripSeparator14;
		private ToolStripMenuItem toolStripMenuItem52;
		private ToolStripButton toolStripButton3;
		public Form1()
		{
			this.InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
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
			this.toolStrip1 = new ToolStrip();
			this.toolStripButton1 = new ToolStripButton();
			this.toolStripSeparator12 = new ToolStripSeparator();
			this.toolStripButton2 = new ToolStripButton();
			this.toolStripDropDownButton1 = new ToolStripDropDownButton();
			this.toolStripMenuItem47 = new ToolStripMenuItem();
			this.toolStripMenuItem48 = new ToolStripMenuItem();
			this.toolStripSeparator13 = new ToolStripSeparator();
			this.toolStripMenuItem49 = new ToolStripMenuItem();
			this.toolStripSplitButton1 = new ToolStripSplitButton();
			this.toolStripMenuItem50 = new ToolStripMenuItem();
			this.toolStripMenuItem51 = new ToolStripMenuItem();
			this.toolStripSeparator14 = new ToolStripSeparator();
			this.toolStripMenuItem52 = new ToolStripMenuItem();
			this.toolStripButton3 = new ToolStripButton();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripButton1,
				this.toolStripSeparator12,
				this.toolStripButton2,
				this.toolStripDropDownButton1,
				this.toolStripSplitButton1,
				this.toolStripButton3
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(284, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			this.toolStripButton1.CheckOnClick = true;
			this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = Resources.NewFolderHS;
			this.toolStripButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new Size(6, 25);
			this.toolStripButton2.Image = Resources.OpenHS;
			this.toolStripButton2.ImageTransparentColor = Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new Size(76, 22);
			this.toolStripButton2.Text = "打开文件";
			this.toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItem47,
				this.toolStripMenuItem48,
				this.toolStripSeparator13,
				this.toolStripMenuItem49
			});
			this.toolStripDropDownButton1.Image = Resources.NewDocumentHS;
			this.toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new Size(29, 22);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			this.toolStripMenuItem47.Name = "toolStripMenuItem47";
			this.toolStripMenuItem47.Size = new Size(199, 22);
			this.toolStripMenuItem47.Text = "toolStripMenuItem47";
			this.toolStripMenuItem48.Name = "toolStripMenuItem48";
			this.toolStripMenuItem48.Size = new Size(199, 22);
			this.toolStripMenuItem48.Text = "toolStripMenuItem48";
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new Size(196, 6);
			this.toolStripMenuItem49.Name = "toolStripMenuItem49";
			this.toolStripMenuItem49.Size = new Size(199, 22);
			this.toolStripMenuItem49.Text = "toolStripMenuItem49";
			this.toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItem50,
				this.toolStripMenuItem51,
				this.toolStripSeparator14,
				this.toolStripMenuItem52
			});
			this.toolStripSplitButton1.Image = Resources.PrintHS;
			this.toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new Size(32, 22);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			this.toolStripMenuItem50.Name = "toolStripMenuItem50";
			this.toolStripMenuItem50.Size = new Size(199, 22);
			this.toolStripMenuItem50.Text = "toolStripMenuItem50";
			this.toolStripMenuItem51.Name = "toolStripMenuItem51";
			this.toolStripMenuItem51.Size = new Size(199, 22);
			this.toolStripMenuItem51.Text = "toolStripMenuItem51";
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new Size(196, 6);
			this.toolStripMenuItem52.Name = "toolStripMenuItem52";
			this.toolStripMenuItem52.Size = new Size(199, 22);
			this.toolStripMenuItem52.Text = "toolStripMenuItem52";
			this.toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = Resources.AlignTableCellMiddleLeftJustHS;
			this.toolStripButton3.ImageTransparentColor = Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 262);
			base.Controls.Add(this.toolStrip1);
			base.Name = "Form1";
			this.Text = "Form1";
			base.Load += new EventHandler(this.Form1_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
