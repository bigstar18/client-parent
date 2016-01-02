// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.TestDraw
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient
{
  public class TestDraw : Form
  {
    private Point ptOriginal = new Point();
    private Point ptLast = new Point();
    private bool flag;
    private Brush brush;
    private bool bHaveMouse;
    private IContainer components;
    private Timer timer1;
    private Button button1;
    private Label label1;

    public TestDraw()
    {
      this.InitializeComponent();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      DateTime now = DateTime.Now;
      Bitmap bitmap = new Bitmap(600, 600);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.Clear(Color.Black);
      if (this.flag)
      {
        this.brush = (Brush) new LinearGradientBrush(new PointF(0.0f, 0.0f), new PointF(700f, 300f), Color.Red, Color.Blue);
        this.flag = false;
      }
      else
      {
        this.brush = (Brush) new LinearGradientBrush(new PointF(0.0f, 0.0f), new PointF(700f, 300f), Color.Blue, Color.Red);
        this.flag = true;
      }
      for (int index1 = 0; index1 < 60; ++index1)
      {
        for (int index2 = 0; index2 < 60; ++index2)
          graphics.FillEllipse(this.brush, index2 * 10, index1 * 10, 10, 10);
      }
      this.CreateGraphics().DrawImage((Image) bitmap, 0, 0);
      this.label1.Text = "速度：" + ((float) (1000 / (DateTime.Now - now).Milliseconds)).ToString() + "帧/秒";
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
      Rectangle rectangle = new Rectangle();
      p1 = this.PointToScreen(p1);
      p2 = this.PointToScreen(p2);
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
        Point point = new Point(e.X, e.Y);
        this.MyDrawReversibleRectangle(this.ptOriginal, this.ptLast);
      }
      this.ptLast.X = -1;
      this.ptLast.Y = -1;
      this.ptOriginal.X = -1;
      this.ptOriginal.Y = -1;
    }

    public void MyMouseMove(object sender, MouseEventArgs e)
    {
      Point p2 = new Point(e.X, e.Y);
      if (!this.bHaveMouse)
        return;
      if (this.ptLast.X != -1)
        this.MyDrawReversibleRectangle(this.ptOriginal, this.ptLast);
      this.ptLast = p2;
      this.MyDrawReversibleRectangle(this.ptOriginal, p2);
    }

    protected override void OnLoad(EventArgs e)
    {
      this.MouseDown += new MouseEventHandler(this.MyMouseDown);
      this.MouseUp += new MouseEventHandler(this.MyMouseUp);
      this.MouseMove += new MouseEventHandler(this.MyMouseMove);
      this.bHaveMouse = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.timer1 = new Timer(this.components);
      this.button1 = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
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
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(715, 421);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button1);
      this.Name = "TestDraw";
      this.Text = "TestDraw";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
