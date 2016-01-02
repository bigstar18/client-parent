using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FrmObject.Properties;

namespace FrmObject
{
    public partial class FrmMain : Form
    {
        private Image imgTop;
        private Image imgLeft;
        private Image imgRight;
        private Image imgBottom;
        private Image imgRightTop;
        private Image imgLeftTop;
        private Image imgLeftBottom;
        private Image imgRightBottom;
        private Point mouseOffset;//记录鼠标按下的坐标
        private bool isMouseDown;//记录鼠标是否被按下
        private bool isMoveForm;//是否移动窗体
        private FrmMainMousePosition mousePosition;
        private static int frmLastLeft = 0;
        private static int frmLastTop = 0;
        private static int frmLastWidth = 0;
        private static int frmLastHeight = 0;
        private static int frmLeft;
        private static int frmTop;
        private static int frmWidth;
        private static int frmHeight;
        private Rectangle frmRectangle = default(Rectangle);
        private static FrmMain FocusForm = null;
        /// <summary>
        /// 鼠标移动到边框 改变鼠标样式 宽度
        /// </summary>
        private int formEdge=5;
        public int FormEdge
        {
            get { return formEdge; }
            set { formEdge = value; }
        }
        /// <summary>
        /// 设置窗口拉伸和移动的模式，0：适时拉伸移动 1：延时拉伸移动。
        /// </summary>
        private int formSizeMoveMode=1;
        public int FormSizeMoveMode
        {
            get { return formSizeMoveMode; }
            set { formSizeMoveMode = value; }
        }
        /// <summary>
        /// 窗体是否可以拉伸
        /// </summary>
        private bool isSetSize = true;
        public bool IsSetSize
        {
            get { return isSetSize; }
            set { isSetSize = value; }
        }
        
        
        private bool b_IsChangingSize;
        private Brush textColor = Brushes.White;

        public Brush TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public FrmMain()
        {
            
            InitializeComponent();
            InitImage();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DrawTop(e.Graphics);
            this.DrawLeft(e.Graphics);
            this.DrawRight(e.Graphics);
            this.DrawBottom(e.Graphics);
            this.DrawLeftTop(e.Graphics);
            this.DrawRightTop(e.Graphics);
            this.DrawLeftBottom(e.Graphics);
            this.DrawRightBottom(e.Graphics);
            int num = (this.imgTop.Height - 15) / 2;
            if (base.Icon != null)
            {
                //this.Icon = Resources.yrdce;
                e.Graphics.DrawIcon(base.Icon, new Rectangle(10, num, 16, 16));
            }
            Font font = new Font("微软雅黑", 9f, FontStyle.Regular);

            e.Graphics.DrawString(this.Text, font, textColor, 31f, (float)num);
        }
        private void InitImage()
        {
            imgTop = Resources.Top;
            imgLeft = Resources.Test;
            imgRight = Resources.Test;
            imgBottom = Resources.BU;
            imgRightTop = Resources.RIGHTTOP;
            imgLeftTop = Resources.LEFTTOP;
            imgLeftBottom = Resources.LeftBottom;
            imgRightBottom = Resources.RightBottom;
        }
        //画窗体顶端
        private void DrawTop(Graphics graph)
        {
            Brush brush = new TextureBrush(this.imgTop, new Rectangle(0, 0, this.imgTop.Width, this.imgTop.Height));
            graph.FillRectangle(brush, this.imgLeft.Width, 0, base.Width - this.imgLeft.Width - this.imgRight.Width, this.imgTop.Height);
            brush.Dispose();
        }
        //画左边框
        private void DrawLeft(Graphics graph)
        {
            Brush brush = new TextureBrush(this.imgLeft, new Rectangle(0, 0, this.imgLeft.Width, this.imgLeft.Height));
            graph.FillRectangle(brush, 0, this.imgLeftTop.Height, this.imgLeftTop.Width, base.Height - this.imgLeftBottom.Height - this.imgLeftTop.Height);
            brush.Dispose();
        }
        //画右边框
        private void DrawRight(Graphics graph)
        {
            TextureBrush textureBrush = new TextureBrush(this.imgRight, new Rectangle(0, 0, this.imgRight.Width, this.imgRight.Height));
            textureBrush.TranslateTransform((float)(base.Width - this.imgRight.Width), (float)this.imgRightTop.Height);
            graph.FillRectangle(textureBrush, base.Width - this.imgRight.Width, this.imgRightTop.Height, this.imgRightTop.Width, base.Height - this.imgRightBottom.Height - this.imgRightTop.Height);
            textureBrush.Dispose();
        }
        //画底部
        private void DrawBottom(Graphics graph)
        {
            TextureBrush textureBrush = new TextureBrush(this.imgBottom);
            textureBrush.TranslateTransform((float)this.imgLeftBottom.Width, (float)(base.Height - this.imgBottom.Height));
            graph.FillRectangle(textureBrush, this.imgLeftBottom.Width, base.Height - this.imgBottom.Height, base.Width - this.imgRightBottom.Width - this.imgLeftBottom.Width, this.imgBottom.Height);
            textureBrush.Dispose();
        }

        //画左上角
        private void DrawLeftTop(Graphics graph)
        {
            Brush brush = new TextureBrush(this.imgLeftTop, new Rectangle(0, 0, this.imgLeftTop.Width, this.imgLeftTop.Height));
            graph.FillRectangle(brush, 0, 0, this.imgLeftTop.Width, this.imgLeftTop.Height);
            brush.Dispose();
        }
        //画右上角
        private void DrawRightTop(Graphics graph)
        {
            TextureBrush textureBrush = new TextureBrush(this.imgRightTop, new Rectangle(0, 0, this.imgRightTop.Width, this.imgRightTop.Height));
            textureBrush.TranslateTransform((float)(base.Width - this.imgRightTop.Width), 0f);
            graph.FillRectangle(textureBrush, base.Width - this.imgRightTop.Width, 0, this.imgRightTop.Width, this.imgRightTop.Height);
            textureBrush.Dispose();
        }
        //画左下角
        private void DrawLeftBottom(Graphics graph)
        {
            graph.DrawImage(this.imgLeftBottom, 0, base.Height - this.imgLeftBottom.Height, this.imgLeftBottom.Width, this.imgLeftBottom.Height);
        }
        //画右下角
        private void DrawRightBottom(Graphics graph)
        {
            graph.DrawImage(this.imgRightBottom, base.Width - this.imgRightBottom.Width, base.Height - this.imgRightBottom.Height, this.imgLeftBottom.Width, this.imgLeftBottom.Height);
        }
        //画容器
        private void DrawPanel(Graphics graph)
        {
            Brush brush = SystemBrushes.Control;
            if (this.BackgroundImage != null)
            {
                TextureBrush textureBrush = new TextureBrush(this.BackgroundImage, new Rectangle(0, 0, this.BackgroundImage.Width, this.BackgroundImage.Height));
                textureBrush.TranslateTransform((float)(base.Width - this.imgRight.Width), (float)this.imgRightTop.Height);
                brush = textureBrush;
            }
            graph.FillRectangle(brush, this.imgLeft.Width, this.imgLeftTop.Height, base.Width - this.imgLeft.Width - this.imgRight.Width, base.Height - this.imgLeftTop.Height - this.imgRightBottom.Height);
        }

        //更改鼠标样式
        private void ChangeMouseCursor()
        {
            Point point = base.PointToClient(Control.MousePosition);
            if (point.X > this.formEdge && point.X < base.Width - this.formEdge - this.picClose.Width - this.picMin.Width - this.picMax.Width && point.Y <= this.formEdge)
            {
                this.Cursor = Cursors.SizeNS;
                return;
            }
            if (point.X > this.formEdge && point.X < base.Width - this.formEdge && point.Y > base.Height - this.formEdge)
            {
                this.Cursor = Cursors.SizeNS;
                return;
            }
            if (point.X <= this.formEdge && point.Y > this.formEdge && point.Y < base.Height - this.formEdge)
            {
                this.Cursor = Cursors.SizeWE;
                return;
            }
            if (point.X >= base.Width - this.formEdge && point.Y > this.formEdge && point.Y < base.Height - this.formEdge)
            {
                this.Cursor = Cursors.SizeWE;
                return;
            }
            if (point.X <= this.formEdge && point.Y <= this.formEdge)
            {
                this.Cursor = Cursors.SizeNWSE;
                return;
            }
            if (point.X >= base.Width - this.formEdge && point.Y <= this.formEdge)
            {
                this.Cursor = Cursors.SizeNESW;
                return;
            }
            if (point.X <= this.formEdge && point.Y >= base.Height - this.formEdge)
            {
                this.Cursor = Cursors.SizeNESW;
                return;
            }
            if (point.X >= base.Width - this.formEdge && point.Y >= base.Height - this.formEdge)
            {
                this.Cursor = Cursors.SizeNWSE;
                return;
            }
            this.Cursor = Cursors.Default;
        }

        //延时移动和缩放
        private void LapseTimeChgSize()
        {
            int left = this.Left;
            int top = this.Top;
            int num = Control.MousePosition.X - this.Location.X;
            int num2 = Control.MousePosition.Y - this.Location.Y;
            if (this.mousePosition != FrmMainMousePosition.NORMAL)
            {
                switch (this.mousePosition)
                {
                    case FrmMainMousePosition.LEFTTOP:
                        FrmMain.frmTop = top + num2;
                        FrmMain.frmLeft = left + num;
                        FrmMain.frmWidth = base.Width - num;
                        FrmMain.frmHeight = base.Height - num2;
                        break;
                    case FrmMainMousePosition.TOP:
                        FrmMain.frmTop = top + num2;
                        FrmMain.frmLeft = left;
                        FrmMain.frmWidth = base.Width;
                        FrmMain.frmHeight = base.Height - num2;
                        break;
                    case FrmMainMousePosition.RIGHTTOP:
                        FrmMain.frmTop = top + num2;
                        FrmMain.frmLeft = left;
                        FrmMain.frmWidth = num;
                        FrmMain.frmHeight = base.Height - num2;
                        break;
                    case FrmMainMousePosition.LEFT:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left + num;
                        FrmMain.frmHeight = base.Height;
                        FrmMain.frmWidth = base.Width - num;
                        break;
                    case FrmMainMousePosition.RIGHT:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left;
                        FrmMain.frmHeight = base.Height;
                        FrmMain.frmWidth = num;
                        break;
                    case FrmMainMousePosition.LEFTBOTTOM:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left + num;
                        FrmMain.frmWidth = base.Width - num;
                        FrmMain.frmHeight = num2;
                        break;
                    case FrmMainMousePosition.RIGHTBOTTOM:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left;
                        FrmMain.frmWidth = num;
                        FrmMain.frmHeight = num2;
                        break;
                    case FrmMainMousePosition.BOTTOM:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left;
                        FrmMain.frmWidth = base.Width;
                        FrmMain.frmHeight = num2;
                        break;
                    default:
                        FrmMain.frmTop = top;
                        FrmMain.frmLeft = left;
                        FrmMain.frmWidth = num;
                        FrmMain.frmHeight = num2;
                        break;
                }
                if (FrmMain.frmLastLeft == 0)
                {
                    FrmMain.frmLastLeft = FrmMain.frmLeft;
                }
                if (FrmMain.frmLastTop == 0)
                {
                    FrmMain.frmLastTop = FrmMain.frmTop;
                }
                if (FrmMain.frmLastWidth == 0)
                {
                    FrmMain.frmLastWidth = FrmMain.frmWidth;
                }
                if (FrmMain.frmLastHeight == 0)
                {
                    FrmMain.frmLastHeight = FrmMain.frmHeight;
                }
                if (this.b_IsChangingSize)
                {
                   this.frmRectangle.Location = new Point(this.Left, this.Top);
                    this.frmRectangle.Size = new Size(FrmMain.frmLastWidth, FrmMain.frmLastHeight);
                }
                if (this.b_IsChangingSize)
                {
                    ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Dashed);
                }
                this.b_IsChangingSize = true;
                FrmMain.frmLastLeft = FrmMain.frmLeft;
                FrmMain.frmLastTop = FrmMain.frmTop;
                FrmMain.frmLastWidth = FrmMain.frmWidth;
                FrmMain.frmLastHeight = FrmMain.frmHeight;
                this.frmRectangle.Location = new Point(this.Left, this.Top);
                this.frmRectangle.Size = new Size(FrmMain.frmWidth, FrmMain.frmHeight);
                ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Dashed);
            }
        }
        //更改窗体大小
        //更改窗体大小
        private void ChangeFormSize()
        {
            Point point = base.PointToClient(Control.MousePosition);
            if (point.X > this.formEdge && point.X < base.Width - this.formEdge && point.Y <= this.formEdge && this.isMouseDown && this.mousePosition ==FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.TOP;
            }
            else if (point.X > this.formEdge && point.X < base.Width - this.formEdge && point.Y > base.Height - this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.BOTTOM;
            }
            else if (point.X <= this.formEdge && point.Y > this.formEdge && point.Y < base.Height - this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.LEFT;
            }
            else if (point.X >= base.Width - this.formEdge && point.Y > this.formEdge && point.Y < base.Height - this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.RIGHT;
            }
            else if (point.X <= this.formEdge && point.Y <= this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.LEFTTOP;
            }
            else if (point.X >= base.Width - this.formEdge && point.Y <= this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.RIGHTTOP;
            }
            else if (point.X <= this.formEdge && point.Y >= base.Height - this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.LEFTBOTTOM;
            }
            else if (point.X >= base.Width - this.formEdge && point.Y >= base.Height - this.formEdge && this.isMouseDown && this.mousePosition == FrmMainMousePosition.NORMAL)
            {
                this.mousePosition = FrmMainMousePosition.RIGHTBOTTOM;
            }
            if (this.isMouseDown)
            {
                if (this.formSizeMoveMode == 0)
                {
                    this.RealTimeChgSize();
                    return;
                }
                if (this.formSizeMoveMode == 1)
                {
                    this.LapseTimeChgSize();
                }
            }
        }
        private void RealTimeChgSize()
        {
            if (this.mousePosition != FrmMainMousePosition.NORMAL)
            {
                switch (this.mousePosition)
                {
                    case FrmMainMousePosition.LEFTTOP:
                        {
                            int right = base.Right;
                            int bottom = base.Bottom;
                            base.Top = Control.MousePosition.Y;
                            base.Height = bottom - base.Top;
                            base.Left = Control.MousePosition.X;
                            base.Width = right - base.Left;
                            break;
                        }
                    case FrmMainMousePosition.TOP:
                        {
                            int bottom = base.Bottom;
                            base.Top = Control.MousePosition.Y;
                            base.Height = bottom - base.Top;
                            break;
                        }
                    case FrmMainMousePosition.RIGHTTOP:
                        {
                            int bottom = base.Bottom;
                            base.Top = Control.MousePosition.Y;
                            base.Height = bottom - base.Top;
                            base.Width = Control.MousePosition.X - base.Left;
                            break;
                        }
                    case FrmMainMousePosition.LEFT:
                        {
                            int right = base.Right;
                            base.Left = Control.MousePosition.X;
                            base.Width = right - base.Left;
                            break;
                        }
                    case FrmMainMousePosition.RIGHT:
                        base.Width = Control.MousePosition.X - base.Left;
                        break;
                    case FrmMainMousePosition.LEFTBOTTOM:
                        {
                            int right = base.Right;
                            base.Height = Control.MousePosition.Y - base.Top;
                            base.Left = Control.MousePosition.X;
                            base.Width = right - base.Left;
                            break;
                        }
                    case FrmMainMousePosition.RIGHTBOTTOM:
                        base.Height = Control.MousePosition.Y - base.Top;
                        base.Width = Control.MousePosition.X - base.Left;
                        break;
                    case FrmMainMousePosition.BOTTOM:
                        base.Height = Control.MousePosition.Y - base.Top;
                        break;
                }
                base.Invalidate(false);
                this.b_IsChangingSize = true;
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            FrmMain.FocusForm = null;
            this.timerBackGround.Start();
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            FrmMain.FocusForm = this;
            this.timerBackGround.Stop();
            base.Invalidate();
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            base.Invalidate();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picMax_Click(object sender, EventArgs e)
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

        private void picMin_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (base.WindowState != FormWindowState.Maximized)
            {
                if (!this.b_IsChangingSize && ((e.Button == MouseButtons.Left && e.Y > this.formEdge && e.Y < this.imgTop.Height) || this.isMoveForm))
                {
                    if (this.formSizeMoveMode == 0)
                    {
                        Point location = default(Point);
                        location = Control.MousePosition;
                        location.Offset(this.mouseOffset.X, this.mouseOffset.Y);
                        base.Location = location;
                        this.isMoveForm = true;
                    }
                    else
                    {
                        CommonClass.ReleaseCapture();
                        CommonClass.SendMessage(base.Handle, 274, 61458, 0);
                        this.FrmMain_MouseUp(null, null);
                    }
                }
                if (!this.isMoveForm && this.isSetSize)
                {
                    this.ChangeMouseCursor();
                    this.ChangeFormSize();
                }
            }
        }

        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseOffset = new Point(-e.X, -e.Y);
            this.isMouseDown = true;
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {

            if (this.b_IsChangingSize && this.formSizeMoveMode == 1)
            {
                this.frmRectangle.Location = new Point(this.Left, this.Top);
                this.frmRectangle.Size = new Size(FrmMain.frmWidth, FrmMain.frmHeight);
                ControlPaint.DrawReversibleFrame(this.frmRectangle, Color.Empty, FrameStyle.Dashed);
                this.Left = FrmMain.frmLeft;
                this.Top = FrmMain.frmTop;
                this.Width = FrmMain.frmWidth;
                this.Height = FrmMain.frmHeight;
            }
            this.b_IsChangingSize = false;
            this.isMouseDown = false;
            this.isMoveForm = false;
            this.mousePosition = FrmMainMousePosition.NORMAL;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            CommonClass.SetTaskMenu(this);
        }


    }
}
