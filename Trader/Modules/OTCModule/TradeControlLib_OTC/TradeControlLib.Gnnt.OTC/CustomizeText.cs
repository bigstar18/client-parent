using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace TradeControlLib.Gnnt.OTC
{
	public class CustomizeText : TextBox
	{
		private const int NULL_BRUSH = 5;
		private const int TRANSPARENT = 1;
		private const int WM_ERASEBKGND = 20;
		private const int WM_KEYDOWN = 256;
		private IContainer components;
		private TextBox textBox1;
		private DrawState m_emds;
		private Color m_crBackColor;
		private Color m_crBackColor1;
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
			this.textBox1 = new TextBox();
			base.SuspendLayout();
			this.textBox1.BackColor = Color.White;
			this.textBox1.Location = new Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(100, 21);
			this.textBox1.TabIndex = 0;
			base.ResumeLayout(false);
		}
		[DllImport("gdi32")]
		private static extern IntPtr GetStockObject(int fnobject);
		[DllImport("gdi32")]
		private static extern int SetBkMode(IntPtr hdc, int bkMode);
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 20)
			{
				m.Result = CustomizeText.GetStockObject(5);
				this.OnEraseBkgnd(Graphics.FromHdc(m.WParam));
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}
		protected void OnEraseBkgnd(Graphics gs)
		{
			if (base.Bounds.Width <= 0)
			{
				return;
			}
			if (Convert.ToBoolean(DrawState.Drawbg3 & this.m_emds))
			{
				int num = base.Bounds.Width >> 1;
				new Rectangle(base.Bounds.X, base.Bounds.Y, num, base.Bounds.Height);
				SolidBrush brush = new SolidBrush(this.m_crBackColor1);
				gs.FillRectangle(brush, base.Bounds);
				Rectangle rect = new Rectangle(base.Bounds.X + num, base.Bounds.Y, base.Bounds.Width, base.Bounds.Height);
				brush = new SolidBrush(this.m_crBackColor);
				gs.FillRectangle(brush, rect);
				this.BackColor = Color.ForestGreen;
				return;
			}
			SolidBrush brush2 = new SolidBrush(this.m_crBackColor1);
			gs.FillRectangle(brush2, base.Bounds);
		}
		public CustomizeText()
		{
			this.m_emds = DrawState.Drawbg3;
			this.InitializeComponent();
			this.m_crBackColor = Color.FromArgb(243, 200, 199);
			this.m_crBackColor1 = Color.FromArgb(174, 202, 238);
		}
		public void SetDrawState(DrawState dsval)
		{
			this.m_emds = dsval;
		}
		public void SetBackColor2(Color crval)
		{
			this.m_crBackColor = crval;
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}
	}
}
