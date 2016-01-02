using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient
{
	public class TestDraw : Form
	{
		private IContainer components;
		private Timer timer1;
		private Button button1;
		private Label label1;
		private bool flag;
		private Brush brush;
		private bool bHaveMouse;
		private Point ptOriginal = default(Point);
		private Point ptLast = default(Point);
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
			this.timer1 = new Timer(this.components);
			this.button1 = new Button();
			this.label1 = new Label();
			base.SuspendLayout();
			this.timer1.Interval = 10;
			this.timer1.Tick += new EventHandler(this.timer1_Tick);
			this.button1.Location = new Point(651, 72);
			this.button1.Name = "button1";
			this.button1.Size = new Size(58, 26);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(639, 40);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(715, 421);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Name = "TestDraw";
			this.Text = "TestDraw";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public TestDraw()
		{
			this.InitializeComponent();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			Bitmap image = new Bitmap(600, 600);
			Graphics graphics = Graphics.FromImage(image);
			graphics.Clear(Color.Black);
			if (this.flag)
			{
				this.brush = new LinearGradientBrush(new PointF(0f, 0f), new PointF(700f, 300f), Color.Red, Color.Blue);
				this.flag = false;
			}
			else
			{
				this.brush = new LinearGradientBrush(new PointF(0f, 0f), new PointF(700f, 300f), Color.Blue, Color.Red);
				this.flag = true;
			}
			for (int i = 0; i < 60; i++)
			{
				for (int j = 0; j < 60; j++)
				{
					graphics.FillEllipse(this.brush, j * 10, i * 10, 10, 10);
				}
			}
			base.CreateGraphics().DrawImage(image, 0, 0);
			DateTime now2 = DateTime.Now;
			float num = (float)(1000 / (now2 - now).Milliseconds);
			this.label1.Text = "速度：" + num.ToString() + "帧/秒";
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.timer1.Enabled = true;
		}
		private void button2_Click(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
		}
		public void MyMouseDown(object sender, MouseEventArgs e)
		{
			this.bHaveMouse = true;
			this.ptOriginal.X = e.X;
			this.ptOriginal.Y = e.Y;
			this.ptLast.X = -1;
			this.ptLast.Y = -1;
		}
		private void MyDrawReversibleRectangle(Point p1, Point p2)
		{
			Rectangle rectangle = default(Rectangle);
			p1 = base.PointToScreen(p1);
			p2 = base.PointToScreen(p2);
			if (p1.X < p2.X)
			{
				rectangle.X = p1.X;
				rectangle.Width = p2.X - p1.X;
			}
			else
			{
				rectangle.X = p2.X;
				rectangle.Width = p1.X - p2.X;
			}
			if (p1.Y < p2.Y)
			{
				rectangle.Y = p1.Y;
				rectangle.Height = p2.Y - p1.Y;
			}
			else
			{
				rectangle.Y = p2.Y;
				rectangle.Height = p1.Y - p2.Y;
			}
			ControlPaint.DrawReversibleFrame(rectangle, Color.Red, FrameStyle.Dashed);
		}
		public void MyMouseUp(object sender, MouseEventArgs e)
		{
			this.bHaveMouse = false;
			if (this.ptLast.X != -1)
			{
				new Point(e.X, e.Y);
				this.MyDrawReversibleRectangle(this.ptOriginal, this.ptLast);
			}
			this.ptLast.X = -1;
			this.ptLast.Y = -1;
			this.ptOriginal.X = -1;
			this.ptOriginal.Y = -1;
		}
		public void MyMouseMove(object sender, MouseEventArgs e)
		{
			Point p = new Point(e.X, e.Y);
			if (this.bHaveMouse)
			{
				if (this.ptLast.X != -1)
				{
					this.MyDrawReversibleRectangle(this.ptOriginal, this.ptLast);
				}
				this.ptLast = p;
				this.MyDrawReversibleRectangle(this.ptOriginal, p);
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.MouseDown += new MouseEventHandler(this.MyMouseDown);
			base.MouseUp += new MouseEventHandler(this.MyMouseUp);
			base.MouseMove += new MouseEventHandler(this.MyMouseMove);
			this.bHaveMouse = false;
		}
	}
}
