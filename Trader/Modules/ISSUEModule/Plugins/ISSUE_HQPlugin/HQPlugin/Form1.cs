using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace HQPlugin
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
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
			this.Text = "Form1";
		}
	}
}
