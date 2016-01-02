using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace TradeVersionInfo
{
	public class Form1 : Form
	{
		private IContainer components;
		private Button button1;
		private TextBox textBox1;
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
			this.button1 = new Button();
			this.textBox1 = new TextBox();
			base.SuspendLayout();
			this.button1.Location = new Point(78, 114);
			this.button1.Name = "button1";
			this.button1.Size = new Size(120, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "获取详细版本信息";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.textBox1.Location = new Point(78, 56);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(100, 21);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "F3.1.02";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(292, 266);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button1);
			base.Name = "Form1";
			this.Text = "Form1";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public Form1()
		{
			this.InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			string text = this.textBox1.Text;
			DetailVersion detailVersion = new DetailVersion();
			VersionInfo detailVersion2 = detailVersion.GetDetailVersion(text);
			MessageBox.Show("完整版本号：" + detailVersion2.FullVersion);
			MessageBox.Show("版本类型：" + detailVersion2.VersionType);
			MessageBox.Show("主板本号：" + detailVersion2.MajorVersion);
			MessageBox.Show("从版本号：" + detailVersion2.MinorVersion);
			MessageBox.Show("小版本号：" + detailVersion2.MicroVersion);
		}
	}
}
