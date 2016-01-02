using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Gnnt.ISSUE.Module.TradePlugin
{
	public class Form1 : Form
	{
		private IContainer components;
		public Form1()
		{
			this.InitializeComponent();
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(366, 273);
			base.Name = "Form1";
			this.Text = "Form1";
			base.ResumeLayout(false);
		}
	}
}
