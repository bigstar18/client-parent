using DIYForm.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace DIYForm
{
	public class MyForm : Form
	{
		private IContainer components;
		private Timer timerBackGround;
		private PictureBox pictureBoxMax;
		private PictureBox pictureBoxMin;
		private PictureBox pictureBoxClose;
		private static MyForm FocusForm = null;
		private Image m_LeftTop;
		private Image m_Top;
		private Image m_RightTop;
		private Image m_Left;
		private Image m_Right;
		private Image m_LeftBottom;
		private Image m_Bottom;
		private Image m_RightBottom;
		private int m_FormSizeMoveMode = 1;
		private bool m_MaximizeBox = true;
		private bool m_MinimizeBox = true;
		private bool m_CloseBox = true;
		private bool isSetSize = true;
		private int m_FormEdge = 5;
		private Point m_MouseOffset;
		private bool b_IsChangingSize;
		private bool b_IsMouseDown;
		private bool b_IsMoveForm;
		private MyFormMousePosition m_MousePosition;
		private static int frmLastLeft = 0;
		private static int frmLastTop = 0;
		private static int frmLastWidth = 0;
		private static int frmLastHeight = 0;
		private static int frmLeft;
		private static int frmTop;
		private static int frmWidth;
		private static int frmHeight;
		private Rectangle frmRectangle = default(Rectangle);
		[Category("DIYForm窗体属性"), Description("设定窗体拉伸以及移动模式，0：适时拉伸移动 1：延时拉伸移动。")]
		public int FormSizeMoveMode
		{
			get
			{
				return this.m_FormSizeMoveMode;
			}
			set
			{
				this.m_FormSizeMoveMode = value;
			}
		}
		[Category("DIYForm窗体属性"), Description("确定窗体标题栏右上角是否有最大化框。")]
		public bool DIYMaximizeBox
		{
			get
			{
				return this.m_MaximizeBox;
			}
			set
			{
				this.m_MaximizeBox = value;
				base.Invalidate();
			}
		}
		[Category("DIYForm窗体属性"), Description("确定窗体标题栏右上角是否有最小化框。")]
		public bool DIYMinimizeBox
		{
			get
			{
				return this.m_MinimizeBox;
			}
			set
			{
				this.m_MinimizeBox = value;
				base.Invalidate();
			}
		}
		[Category("DIYForm窗体属性"), Description("确定窗体标题栏右上角是否有关闭框。")]
		public bool DIYCloseBox
		{
			get
			{
				return this.m_CloseBox;
			}
			set
			{
				this.m_CloseBox = value;
				base.Invalidate();
			}
		}
		[Category("DIYForm窗体属性"), Description("窗体是否可以拉伸。")]
		public bool SetSize
		{
			get
			{
				return this.isSetSize;
			}
			set
			{
				this.isSetSize = value;
			}
		}
		[Category("DIYForm窗体属性"), Description("设定边框的实际宽度，鼠标根据该宽度判断何时可以变换形状。")]
		public int FormEdge
		{
			get
			{
				return this.m_FormEdge;
			}
			set
			{
				this.m_FormEdge = value;
			}
		}
        private Brush textColor = Brushes.Black;

        public Brush TextColor
        {
            get { return textColor; }
            set { textColor = value; }
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MyForm));
			this.timerBackGround = new Timer(this.components);
			this.pictureBoxMax = new PictureBox();
			this.pictureBoxMin = new PictureBox();
			this.pictureBoxClose = new PictureBox();
			((ISupportInitialize)this.pictureBoxMax).BeginInit();
			((ISupportInitialize)this.pictureBoxMin).BeginInit();
			((ISupportInitialize)this.pictureBoxClose).BeginInit();
			base.SuspendLayout();
			this.timerBackGround.Enabled = true;
			this.timerBackGround.Interval = 1000;
			this.timerBackGround.Tick += new EventHandler(this.timerBackGround_Tick);
			this.pictureBoxMax.BackgroundImageLayout = ImageLayout.Stretch;
			this.pictureBoxMax.Image = Resources.Max;
			this.pictureBoxMax.Location = new Point(220,1);
			this.pictureBoxMax.Name = "pictureBoxMax";
			this.pictureBoxMax.Size = new Size(26, 26);
            this.pictureBoxMax.BackColor = Color.Transparent;
            this.pictureBoxMax.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxMax.TabIndex = 5;
			this.pictureBoxMax.TabStop = false;
			this.pictureBoxMax.MouseLeave += new EventHandler(this.pictureBoxMax_MouseLeave);
			this.pictureBoxMax.Click += new EventHandler(this.pictureBoxMax_Click);
			this.pictureBoxMax.MouseEnter += new EventHandler(this.pictureBoxMax_MouseEnter);
			this.pictureBoxMin.BackgroundImageLayout = ImageLayout.Stretch;
			this.pictureBoxMin.Image = Resources.Min;
			this.pictureBoxMin.Location = new Point(196, 1);
			this.pictureBoxMin.Name = "pictureBoxMin";
			this.pictureBoxMin.Size = new Size(26, 26);
			this.pictureBoxMin.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBoxMin.TabIndex = 4;
            this.pictureBoxMin.BackColor = Color.Transparent;
			this.pictureBoxMin.TabStop = false;
			this.pictureBoxMin.MouseLeave += new EventHandler(this.pictureBoxMin_MouseLeave);
			this.pictureBoxMin.Click += new EventHandler(this.pictureBoxMin_Click);
			this.pictureBoxMin.MouseEnter += new EventHandler(this.pictureBoxMin_MouseEnter);
			this.pictureBoxClose.BackgroundImageLayout = ImageLayout.Stretch;
			this.pictureBoxClose.Image = Resources.Close;
			this.pictureBoxClose.Location = new Point(243,1);
			this.pictureBoxClose.Name = "pictureBoxClose";
			this.pictureBoxClose.Size = new Size(26, 26);
			this.pictureBoxClose.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBoxClose.TabIndex = 3;
			this.pictureBoxClose.TabStop = false;
            this.pictureBoxClose.BackColor = Color.Transparent;
			this.pictureBoxClose.MouseLeave += new EventHandler(this.pictureBoxClose_MouseLeave);
			this.pictureBoxClose.Click += new EventHandler(this.pictureBoxClose_Click);
			this.pictureBoxClose.MouseEnter += new EventHandler(this.pictureBoxClose_MouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(292, 273);
            base.Controls.Add(this.pictureBoxMax);
			base.Controls.Add(this.pictureBoxMin);
			base.Controls.Add(this.pictureBoxClose);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "MyForm";
			this.Text = "MyForm";
			base.Load += new EventHandler(this.MyFormForm_Load);
			base.MouseUp += new MouseEventHandler(this.MyFormForm_MouseUp);
			base.MouseDoubleClick += new MouseEventHandler(this.MyFormForm_MouseDoubleClick);
			base.SizeChanged += new EventHandler(this.MyForm_SizeChanged);
			base.MouseDown += new MouseEventHandler(this.MyFormForm_MouseDown);
			base.MouseLeave += new EventHandler(this.MyForm_MouseLeave);
			base.MouseMove += new MouseEventHandler(this.MyFormForm_MouseMove);
			((ISupportInitialize)this.pictureBoxMax).EndInit();
			((ISupportInitialize)this.pictureBoxMin).EndInit();
			((ISupportInitialize)this.pictureBoxClose).EndInit();
			base.ResumeLayout(false);
            base.PerformLayout();
        }
		public MyForm()
		{
			this.InitializeComponent();
			MyForm.FocusForm = this;
			this.InitImages();
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}
		private void InitImages()
		{
			this.m_Top = Resources.Top;
			this.m_LeftTop = Resources.LEFTTOP;
			this.m_RightTop = Resources.RIGHTTOP;
			this.m_Left = Resources.LEFT;
			this.m_Right = Resources.RIGHT;
			this.m_LeftBottom = Resources.LeftBottom;
			this.m_Bottom = Resources.BOTTOM;
			this.m_RightBottom = Resources.RightBottom;
		}
		private void DrawLeftTop(Graphics graph)
		{
			Brush brush = new TextureBrush(this.m_LeftTop, new Rectangle(0, 0, this.m_LeftTop.Width, this.m_LeftTop.Height));
			graph.FillRectangle(brush, 0, 0, this.m_LeftTop.Width, this.m_LeftTop.Height);
			brush.Dispose();
		}
		private void DrawTop(Graphics graph)
		{
			Brush brush = new TextureBrush(this.m_Top, new Rectangle(0, 0, this.m_Top.Width, this.m_Top.Height));
			graph.FillRectangle(brush, this.m_Left.Width, 0, base.Width - this.m_Left.Width - this.m_Right.Width, this.m_Top.Height);
			brush.Dispose();
		}
		private void DrawRightTop(Graphics graph)
		{
			TextureBrush textureBrush = new TextureBrush(this.m_RightTop, new Rectangle(0, 0, this.m_RightTop.Width, this.m_RightTop.Height));
			textureBrush.TranslateTransform((float)(base.Width - this.m_RightTop.Width), 0f);
			graph.FillRectangle(textureBrush, base.Width - this.m_RightTop.Width, 0, this.m_RightTop.Width, this.m_RightTop.Height);
			textureBrush.Dispose();
		}
		private void DrawLeft(Graphics graph)
		{
			Brush brush = new TextureBrush(this.m_Left, new Rectangle(0, 0, this.m_Left.Width, this.m_Left.Height));
			graph.FillRectangle(brush, 0, this.m_LeftTop.Height, this.m_LeftTop.Width, base.Height - this.m_LeftBottom.Height - this.m_LeftTop.Height);
			brush.Dispose();
		}
		private void DrawRight(Graphics graph)
		{
			TextureBrush textureBrush = new TextureBrush(this.m_Right, new Rectangle(0, 0, this.m_Right.Width, this.m_Right.Height));
			textureBrush.TranslateTransform((float)(base.Width - this.m_Right.Width), (float)this.m_RightTop.Height);
			graph.FillRectangle(textureBrush, base.Width - this.m_Right.Width, this.m_RightTop.Height, this.m_RightTop.Width, base.Height - this.m_RightBottom.Height - this.m_RightTop.Height);
			textureBrush.Dispose();
		}
		private void DrawLeftBottom(Graphics graph)
		{
			graph.DrawImage(this.m_LeftBottom, 0, base.Height - this.m_LeftBottom.Height, this.m_LeftBottom.Width, this.m_LeftBottom.Height);
		}
		private void DrawBottom(Graphics graph)
		{
			TextureBrush textureBrush = new TextureBrush(this.m_Bottom);
			textureBrush.TranslateTransform((float)this.m_LeftBottom.Width, (float)(base.Height - this.m_Bottom.Height));
			graph.FillRectangle(textureBrush, this.m_LeftBottom.Width, base.Height - this.m_Bottom.Height, base.Width - this.m_RightBottom.Width - this.m_LeftBottom.Width, this.m_Bottom.Height);
			textureBrush.Dispose();
		}
		private void DrawRightBottom(Graphics graph)
		{
			graph.DrawImage(this.m_RightBottom, base.Width - this.m_RightBottom.Width, base.Height - this.m_RightBottom.Height, this.m_LeftBottom.Width, this.m_LeftBottom.Height);
		}
		private void DrawPanel(Graphics graph)
		{
			Brush brush = SystemBrushes.Control;
			if (this.BackgroundImage != null)
			{
				TextureBrush textureBrush = new TextureBrush(this.BackgroundImage, new Rectangle(0, 0, this.BackgroundImage.Width, this.BackgroundImage.Height));
				textureBrush.TranslateTransform((float)(base.Width - this.m_Right.Width), (float)this.m_RightTop.Height);
				brush = textureBrush;
			}
			graph.FillRectangle(brush, this.m_Left.Width, this.m_LeftTop.Height, base.Width - this.m_Left.Width - this.m_Right.Width, base.Height - this.m_LeftTop.Height - this.m_RightBottom.Height);
		}
		private void DrawStateBoxPosition()
		{
			this.pictureBoxClose.Left = base.Width - this.pictureBoxClose.Width - this.m_Right.Width;
			this.pictureBoxMax.Left = base.Width - this.m_Right.Width - this.pictureBoxClose.Width - this.pictureBoxMax.Width;
			this.pictureBoxMin.Left = base.Width - this.m_Right.Width - this.pictureBoxClose.Width - this.pictureBoxMax.Width - this.pictureBoxMin.Width;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.pictureBoxMax.Visible = this.m_MaximizeBox;
			this.pictureBoxMin.Visible = this.m_MinimizeBox;
			this.pictureBoxClose.Visible = this.m_CloseBox;
			this.DrawTop(e.Graphics);
			this.DrawLeftTop(e.Graphics);
			this.DrawRightTop(e.Graphics);
            this.DrawLeft(e.Graphics);
            this.DrawRight(e.Graphics);
			this.DrawLeftBottom(e.Graphics);
			this.DrawBottom(e.Graphics);
			this.DrawRightBottom(e.Graphics);
			this.DrawPanel(e.Graphics);
			this.DrawStateBoxPosition();
			int num = 5;
			if (base.Icon != null)
			{
				//e.Graphics.DrawIcon(base.Icon, new Rectangle(10, num, 16, 16));
			}
			Font font = new Font("新宋体", 11f, FontStyle.Regular);
          
			e.Graphics.DrawString(this.Text, font,textColor, 5f, (float)num);
		}
		private void ChangeMouseCursor()
		{
			Point point = base.PointToClient(Control.MousePosition);
			if (point.X > this.m_FormEdge && point.X < base.Width - this.m_FormEdge - this.pictureBoxClose.Width - this.pictureBoxMin.Width - this.pictureBoxMax.Width && point.Y <= this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNS;
				return;
			}
			if (point.X > this.m_FormEdge && point.X < base.Width - this.m_FormEdge && point.Y > base.Height - this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNS;
				return;
			}
			if (point.X <= this.m_FormEdge && point.Y > this.m_FormEdge && point.Y < base.Height - this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeWE;
				return;
			}
			if (point.X >= base.Width - this.m_FormEdge && point.Y > this.m_FormEdge && point.Y < base.Height - this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeWE;
				return;
			}
			if (point.X <= this.m_FormEdge && point.Y <= this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNWSE;
				return;
			}
			if (point.X >= base.Width - this.m_FormEdge && point.Y <= this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNESW;
				return;
			}
			if (point.X <= this.m_FormEdge && point.Y >= base.Height - this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNESW;
				return;
			}
			if (point.X >= base.Width - this.m_FormEdge && point.Y >= base.Height - this.m_FormEdge)
			{
				this.Cursor = Cursors.SizeNWSE;
				return;
			}
			this.Cursor = Cursors.Default;
		}
		private void ChangeFormSize()
		{
			Point point = base.PointToClient(Control.MousePosition);
			if (point.X > this.m_FormEdge && point.X < base.Width - this.m_FormEdge && point.Y <= this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.TOP;
			}
			else if (point.X > this.m_FormEdge && point.X < base.Width - this.m_FormEdge && point.Y > base.Height - this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.BOTTOM;
			}
			else if (point.X <= this.m_FormEdge && point.Y > this.m_FormEdge && point.Y < base.Height - this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.LEFT;
			}
			else if (point.X >= base.Width - this.m_FormEdge && point.Y > this.m_FormEdge && point.Y < base.Height - this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.RIGHT;
			}
			else if (point.X <= this.m_FormEdge && point.Y <= this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.LEFTTOP;
			}
			else if (point.X >= base.Width - this.m_FormEdge && point.Y <= this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.RIGHTTOP;
			}
			else if (point.X <= this.m_FormEdge && point.Y >= base.Height - this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.LEFTBOTTOM;
			}
			else if (point.X >= base.Width - this.m_FormEdge && point.Y >= base.Height - this.m_FormEdge && this.b_IsMouseDown && this.m_MousePosition == MyFormMousePosition.NORMAL)
			{
				this.m_MousePosition = MyFormMousePosition.RIGHTBOTTOM;
			}
			if (this.b_IsMouseDown)
			{
				if (this.m_FormSizeMoveMode == 0)
				{
					this.RealTimeChgSize();
					return;
				}
				if (this.m_FormSizeMoveMode == 1)
				{
					this.LapseTimeChgSize();
				}
			}
		}
		private void RealTimeChgSize()
		{
			if (this.m_MousePosition != MyFormMousePosition.NORMAL)
			{
				switch (this.m_MousePosition)
				{
				case MyFormMousePosition.LEFTTOP:
				{
					int right = base.Right;
					int bottom = base.Bottom;
					base.Top = Control.MousePosition.Y;
					base.Height = bottom - base.Top;
					base.Left = Control.MousePosition.X;
					base.Width = right - base.Left;
					break;
				}
				case MyFormMousePosition.TOP:
				{
					int bottom = base.Bottom;
					base.Top = Control.MousePosition.Y;
					base.Height = bottom - base.Top;
					break;
				}
				case MyFormMousePosition.RIGHTTOP:
				{
					int bottom = base.Bottom;
					base.Top = Control.MousePosition.Y;
					base.Height = bottom - base.Top;
					base.Width = Control.MousePosition.X - base.Left;
					break;
				}
				case MyFormMousePosition.LEFT:
				{
					int right = base.Right;
					base.Left = Control.MousePosition.X;
					base.Width = right - base.Left;
					break;
				}
				case MyFormMousePosition.RIGHT:
					base.Width = Control.MousePosition.X - base.Left;
					break;
				case MyFormMousePosition.LEFTBOTTOM:
				{
					int right = base.Right;
					base.Height = Control.MousePosition.Y - base.Top;
					base.Left = Control.MousePosition.X;
					base.Width = right - base.Left;
					break;
				}
				case MyFormMousePosition.RIGHTBOTTOM:
					base.Height = Control.MousePosition.Y - base.Top;
					base.Width = Control.MousePosition.X - base.Left;
					break;
				case MyFormMousePosition.BOTTOM:
					base.Height = Control.MousePosition.Y - base.Top;
					break;
				}
				base.Invalidate(false);
				this.b_IsChangingSize = true;
			}
		}
		private void LapseTimeChgSize()
		{
			int left = base.Left;
			int top = base.Top;
			int num = Control.MousePosition.X - base.Location.X;
			int num2 = Control.MousePosition.Y - base.Location.Y;
			if (this.m_MousePosition != MyFormMousePosition.NORMAL)
			{
				switch (this.m_MousePosition)
				{
				case MyFormMousePosition.LEFTTOP:
					MyForm.frmTop = top + num2;
					MyForm.frmLeft = left + num;
					MyForm.frmWidth = base.Width - num;
					MyForm.frmHeight = base.Height - num2;
					break;
				case MyFormMousePosition.TOP:
					MyForm.frmTop = top + num2;
					MyForm.frmLeft = left;
					MyForm.frmWidth = base.Width;
					MyForm.frmHeight = base.Height - num2;
					break;
				case MyFormMousePosition.RIGHTTOP:
					MyForm.frmTop = top + num2;
					MyForm.frmLeft = left;
					MyForm.frmWidth = num;
					MyForm.frmHeight = base.Height - num2;
					break;
				case MyFormMousePosition.LEFT:
					MyForm.frmTop = top;
					MyForm.frmLeft = left + num;
					MyForm.frmHeight = base.Height;
					MyForm.frmWidth = base.Width - num;
					break;
				case MyFormMousePosition.RIGHT:
					MyForm.frmTop = top;
					MyForm.frmLeft = left;
					MyForm.frmHeight = base.Height;
					MyForm.frmWidth = num;
					break;
				case MyFormMousePosition.LEFTBOTTOM:
					MyForm.frmTop = top;
					MyForm.frmLeft = left + num;
					MyForm.frmWidth = base.Width - num;
					MyForm.frmHeight = num2;
					break;
				case MyFormMousePosition.RIGHTBOTTOM:
					MyForm.frmTop = top;
					MyForm.frmLeft = left;
					MyForm.frmWidth = num;
					MyForm.frmHeight = num2;
					break;
				case MyFormMousePosition.BOTTOM:
					MyForm.frmTop = top;
					MyForm.frmLeft = left;
					MyForm.frmWidth = base.Width;
					MyForm.frmHeight = num2;
					break;
				default:
					MyForm.frmTop = top;
					MyForm.frmLeft = left;
					MyForm.frmWidth = num;
					MyForm.frmHeight = num2;
					break;
				}
				if (MyForm.frmLastLeft == 0)
				{
					MyForm.frmLastLeft = MyForm.frmLeft;
				}
				if (MyForm.frmLastTop == 0)
				{
					MyForm.frmLastTop = MyForm.frmTop;
				}
				if (MyForm.frmLastWidth == 0)
				{
					MyForm.frmLastWidth = MyForm.frmWidth;
				}
				if (MyForm.frmLastHeight == 0)
				{
					MyForm.frmLastHeight = MyForm.frmHeight;
				}
				if (this.b_IsChangingSize)
				{
					this.frmRectangle.Location = new Point(MyForm.frmLastLeft, MyForm.frmLastTop);
					this.frmRectangle.Size = new Size(MyForm.frmLastWidth, MyForm.frmLastHeight);
				}
				if (this.b_IsChangingSize)
				{
                    ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Thick);
				}
				this.b_IsChangingSize = true;
				MyForm.frmLastLeft = MyForm.frmLeft;
				MyForm.frmLastTop = MyForm.frmTop;
				MyForm.frmLastWidth = MyForm.frmWidth;
				MyForm.frmLastHeight = MyForm.frmHeight;
				this.frmRectangle.Location = new Point(MyForm.frmLeft, MyForm.frmTop);
				this.frmRectangle.Size = new Size(MyForm.frmWidth, MyForm.frmHeight);
                ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Thick);
			}
		}
		private void ChangeFormStyle()
		{
			if (base.WindowState == FormWindowState.Maximized)
			{
				base.WindowState = FormWindowState.Normal;
				this.pictureBoxMax.Image = Resources.Max;
			}
			else
			{
				this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
				base.WindowState = FormWindowState.Maximized;
				this.pictureBoxMax.Image = Resources.max3;
			}
			base.Invalidate(false);
		}
		private void MyFormForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized)
			{
				if (!this.b_IsChangingSize && ((e.Button == MouseButtons.Left && e.Y > this.m_FormEdge && e.Y < this.m_Top.Height) || this.b_IsMoveForm))
				{
					if (this.m_FormSizeMoveMode == 0)
					{
						Point location = default(Point);
						location = Control.MousePosition;
						location.Offset(this.m_MouseOffset.X, this.m_MouseOffset.Y);
						base.Location = location;
						this.b_IsMoveForm = true;
					}
					else
					{
						CommonClass.ReleaseCapture();
						CommonClass.SendMessage(base.Handle, 274, 61458, 0);
						this.MyFormForm_MouseUp(null, null);
					}
				}
				if (!this.b_IsMoveForm && this.isSetSize)
				{
					this.ChangeMouseCursor();
					this.ChangeFormSize();
				}
			}
		}
		private void MyFormForm_MouseDown(object sender, MouseEventArgs e)
		{
			this.m_MouseOffset = new Point(-e.X, -e.Y);
			this.b_IsMouseDown = true;
		}
		private void MyFormForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.b_IsChangingSize && this.m_FormSizeMoveMode == 1)
			{
				this.frmRectangle.Location = new Point(MyForm.frmLeft, MyForm.frmTop);
				this.frmRectangle.Size = new Size(MyForm.frmWidth, MyForm.frmHeight);
                ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Thick);
				base.Left = MyForm.frmLeft;
				base.Top = MyForm.frmTop;
				base.Width = MyForm.frmWidth;
				base.Height = MyForm.frmHeight;
			}
			this.b_IsChangingSize = false;
			this.b_IsMouseDown = false;
			this.b_IsMoveForm = false;
			this.m_MousePosition = MyFormMousePosition.NORMAL;
		}
		private void MyFormForm_Load(object sender, EventArgs e)
		{
			CommonClass.SetTaskMenu(this);
		}
		private void MyFormForm_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Point point = default(Point);
			if (base.PointToClient(Control.MousePosition).Y <= this.m_Top.Height)
			{
				this.ChangeFormStyle();
			}
		}
		private void pictureBoxClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void pictureBoxMax_Click(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Maximized)
			{
				base.WindowState = FormWindowState.Normal;
			}
			else
			{
				this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
				base.WindowState = FormWindowState.Maximized;
			}
			base.Invalidate(false);
		}
		private void pictureBoxMin_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}
		private void pictureBoxMin_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBoxMin.Image = Resources.Min1;
		}
		private void pictureBoxMin_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBoxMin.Image = Resources.Min;
		}
		private void pictureBoxMax_MouseEnter(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Normal)
			{
				this.pictureBoxMax.Image = Resources.Max1;
				return;
			}
			this.pictureBoxMax.Image = Resources.Max2;
		}
		private void pictureBoxMax_MouseLeave(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Normal)
			{
				this.pictureBoxMax.Image = Resources.Max;
				return;
			}
			this.pictureBoxMax.Image = Resources.max3;
		}
		private void pictureBoxClose_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBoxClose.Image = Resources.Close1;
		}
		private void pictureBoxClose_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBoxClose.Image = Resources.Close;
		}
		private void EnableAllControls()
		{
			if (this == null || base.Controls == null)
			{
				return;
			}
			foreach (Control control in base.Controls)
			{
				control.Enabled = true;
			}
		}
		private void DisableAllControls()
		{
			foreach (Control control in base.Controls)
			{
				control.Enabled = false;
			}
		}
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			MyForm.FocusForm = null;
			this.timerBackGround.Start();
		}
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			MyForm.FocusForm = this;
			this.timerBackGround.Stop();
			base.Invalidate();
		}
		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			base.Invalidate();
		}
		private void timerBackGround_Tick(object sender, EventArgs e)
		{
			if (this != MyForm.FocusForm)
			{
				base.Invalidate(false);
			}
		}
		private void MyForm_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}
		private void MyForm_SizeChanged(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Normal)
			{
				this.pictureBoxMax.Image = Resources.Max;
				return;
			}
			this.pictureBoxMax.Image = Resources.max3;
		}
	}
}
