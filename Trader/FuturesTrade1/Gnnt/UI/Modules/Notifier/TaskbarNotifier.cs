namespace FuturesTrade.Gnnt.UI.Modules.Notifier
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class TaskbarNotifier : Form
    {
        public bool AutoHide = true;
        protected Bitmap BackgroundBitmap;
        protected bool bIsMouseDown;
        protected bool bIsMouseOverClose;
        protected bool bIsMouseOverContent;
        protected bool bIsMouseOverPopup;
        protected bool bIsMouseOverTitle;
        protected Bitmap CloseBitmap;
        protected Point CloseBitmapLocation;
        protected Size CloseBitmapSize;
        public bool CloseClickable = true;
        public bool ContentClickable = true;
        public Rectangle ContentRectangle;
        protected string contentText;
        public bool EnableSelectionRectangle = true;
        protected Color hoverContentColor = Color.FromArgb(0, 0, 0x66);
        protected Font hoverContentFont = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
        protected Color hoverTitleColor = Color.FromArgb(0xff, 0, 0);
        protected Font hoverTitleFont = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        protected int nHideEvents;
        protected int nIncrementHide;
        protected int nIncrementShow;
        protected Color normalContentColor = Color.FromArgb(0, 0, 0);
        protected Font normalContentFont = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
        protected Color normalTitleColor = Color.FromArgb(0xff, 0, 0);
        protected Font normalTitleFont = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        protected int nShowEvents;
        protected int nVisibleEvents;
        protected Rectangle RealContentRectangle;
        protected Rectangle RealTitleRectangle;
        protected TaskbarStates taskbarState;
        protected System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public bool TitleClickable;
        public Rectangle TitleRectangle;
        protected string titleText;
        protected Rectangle WorkAreaRectangle;

        public event EventHandler CloseClick;

        public event EventHandler ContentClick;

        public event EventHandler TitleClick;

        public TaskbarNotifier()
        {
            base.WindowState = FormWindowState.Minimized;
            base.Show();
            base.Hide();
            base.WindowState = FormWindowState.Normal;
            base.ShowInTaskbar = false;
            base.TopMost = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ControlBox = false;
            this.timer.Enabled = true;
            this.timer.Tick += new EventHandler(this.OnTimer);
        }

        protected Region BitmapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("Bitmap", "Bitmap cannot be null!");
            }
            int height = bitmap.Height;
            int width = bitmap.Width;
            GraphicsPath path = new GraphicsPath();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmap.GetPixel(j, i) != transparencyColor)
                    {
                        int x = j;
                        while ((j < width) && (bitmap.GetPixel(j, i) != transparencyColor))
                        {
                            j++;
                        }
                        path.AddRectangle(new Rectangle(x, i, j - x, 1));
                    }
                }
            }
            Region region = new Region(path);
            path.Dispose();
            return region;
        }

        protected void CalculateMouseRectangles()
        {
            Graphics graphics = base.CreateGraphics();
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.MeasureTrailingSpaces
            };
            SizeF ef = graphics.MeasureString(this.titleText, this.hoverTitleFont, this.TitleRectangle.Width, format);
            SizeF ef2 = graphics.MeasureString(this.contentText, this.hoverContentFont, this.ContentRectangle.Width, format);
            graphics.Dispose();
            this.RealTitleRectangle = new Rectangle(this.TitleRectangle.Left, this.TitleRectangle.Top, (int)ef.Width, (int)ef.Height);
            this.RealTitleRectangle.Inflate(0, 2);
            this.RealContentRectangle = new Rectangle(((this.ContentRectangle.Width - ((int)ef2.Width)) / 2) + this.ContentRectangle.Left, ((this.ContentRectangle.Height - ((int)ef2.Height)) / 2) + this.ContentRectangle.Top, (int)ef2.Width, (int)ef2.Height);
            this.RealContentRectangle.Inflate(0, 2);
        }

        protected void DrawCloseButton(Graphics grfx)
        {
            if (this.CloseBitmap != null)
            {
                Rectangle rectangle2;
                Rectangle destRect = new Rectangle(this.CloseBitmapLocation, this.CloseBitmapSize);
                if (this.bIsMouseOverClose)
                {
                    if (this.bIsMouseDown)
                    {
                        rectangle2 = new Rectangle(new Point(this.CloseBitmapSize.Width * 2, 0), this.CloseBitmapSize);
                    }
                    else
                    {
                        rectangle2 = new Rectangle(new Point(this.CloseBitmapSize.Width, 0), this.CloseBitmapSize);
                    }
                }
                else
                {
                    rectangle2 = new Rectangle(new Point(0, 0), this.CloseBitmapSize);
                }
                grfx.DrawImage(this.CloseBitmap, destRect, rectangle2, GraphicsUnit.Pixel);
            }
        }

        protected void DrawText(Graphics grfx)
        {
            if ((this.titleText != null) && (this.titleText.Length != 0))
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.NoWrap
                };
                if (this.bIsMouseOverTitle)
                {
                    grfx.DrawString(this.titleText, this.hoverTitleFont, new SolidBrush(this.hoverTitleColor), this.TitleRectangle, format);
                }
                else
                {
                    grfx.DrawString(this.titleText, this.normalTitleFont, new SolidBrush(this.normalTitleColor), this.TitleRectangle, format);
                }
            }
            if ((this.contentText != null) && (this.contentText.Length != 0))
            {
                StringFormat format2 = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.MeasureTrailingSpaces
                };
                if (this.bIsMouseOverContent)
                {
                    grfx.DrawString(this.contentText, this.hoverContentFont, new SolidBrush(this.hoverContentColor), this.ContentRectangle, format2);
                    if (this.EnableSelectionRectangle)
                    {
                        ControlPaint.DrawBorder3D(grfx, this.RealContentRectangle, Border3DStyle.Etched, Border3DSide.Bottom | Border3DSide.Right | Border3DSide.Top | Border3DSide.Left);
                    }
                }
                else
                {
                    grfx.DrawString(this.contentText, this.normalContentFont, new SolidBrush(this.normalContentColor), this.ContentRectangle, format2);
                }
            }
        }

        public void Hide()
        {
            if (this.taskbarState != TaskbarStates.hidden)
            {
                this.timer.Stop();
                this.taskbarState = TaskbarStates.hidden;
                base.Hide();
            }
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.ClientSize = new Size(0x124, 0x110);
            base.Name = "TaskbarNotifier";
            base.TopMost = true;
            base.ResumeLayout(false);
        }

        protected override void OnMouseDown(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);
            this.bIsMouseDown = true;
            if (this.bIsMouseOverClose)
            {
                this.Refresh();
            }
        }

        protected override void OnMouseEnter(EventArgs ea)
        {
            base.OnMouseEnter(ea);
            this.bIsMouseOverPopup = true;
            this.Refresh();
        }

        protected override void OnMouseLeave(EventArgs ea)
        {
            base.OnMouseLeave(ea);
            this.bIsMouseOverPopup = false;
            this.bIsMouseOverClose = false;
            this.bIsMouseOverTitle = false;
            this.bIsMouseOverContent = false;
            this.Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);
            bool flag = false;
            if ((((mea.X > this.CloseBitmapLocation.X) && (mea.X < (this.CloseBitmapLocation.X + this.CloseBitmapSize.Width))) && ((mea.Y > this.CloseBitmapLocation.Y) && (mea.Y < (this.CloseBitmapLocation.Y + this.CloseBitmapSize.Height)))) && this.CloseClickable)
            {
                if (!this.bIsMouseOverClose)
                {
                    this.bIsMouseOverClose = true;
                    this.bIsMouseOverTitle = false;
                    this.bIsMouseOverContent = false;
                    this.Cursor = Cursors.Hand;
                    flag = true;
                }
            }
            else if (this.RealContentRectangle.Contains(new Point(mea.X, mea.Y)) && this.ContentClickable)
            {
                if (!this.bIsMouseOverContent)
                {
                    this.bIsMouseOverClose = false;
                    this.bIsMouseOverTitle = false;
                    this.bIsMouseOverContent = true;
                    this.Cursor = Cursors.Hand;
                    flag = true;
                }
            }
            else if (this.RealTitleRectangle.Contains(new Point(mea.X, mea.Y)) && this.TitleClickable)
            {
                if (!this.bIsMouseOverTitle)
                {
                    this.bIsMouseOverClose = false;
                    this.bIsMouseOverTitle = true;
                    this.bIsMouseOverContent = false;
                    this.Cursor = Cursors.Hand;
                    flag = true;
                }
            }
            else
            {
                if ((this.bIsMouseOverClose || this.bIsMouseOverTitle) || this.bIsMouseOverContent)
                {
                    flag = true;
                }
                this.bIsMouseOverClose = false;
                this.bIsMouseOverTitle = false;
                this.bIsMouseOverContent = false;
                this.Cursor = Cursors.Default;
            }
            if (flag)
            {
                this.Refresh();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);
            this.bIsMouseDown = false;
            if (this.bIsMouseOverClose)
            {
                this.timer.Interval = this.nHideEvents;
                this.taskbarState = TaskbarStates.disappearing;
                this.timer.Start();
                if (this.CloseClick != null)
                {
                    this.CloseClick(this, new EventArgs());
                }
            }
            else if (this.bIsMouseOverTitle)
            {
                if (this.TitleClick != null)
                {
                    this.TitleClick(this, new EventArgs());
                }
            }
            else if (this.bIsMouseOverContent)
            {
                if (!this.AutoHide)
                {
                    this.timer.Interval = this.nHideEvents;
                    this.taskbarState = TaskbarStates.disappearing;
                    this.timer.Start();
                }
                if (this.ContentClick != null)
                {
                    this.ContentClick(this, new EventArgs());
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pea)
        {
            Graphics graphics = pea.Graphics;
            graphics.PageUnit = GraphicsUnit.Pixel;
            Bitmap image = new Bitmap(this.BackgroundBitmap.Width, this.BackgroundBitmap.Height);
            Graphics grfx = Graphics.FromImage(image);
            if (this.BackgroundBitmap != null)
            {
                grfx.DrawImage(this.BackgroundBitmap, 0, 0, this.BackgroundBitmap.Width, this.BackgroundBitmap.Height);
            }
            this.DrawCloseButton(grfx);
            this.DrawText(grfx);
            graphics.DrawImage(image, 0, 0);
        }

        protected void OnTimer(object obj, EventArgs ea)
        {
            switch (this.taskbarState)
            {
                case TaskbarStates.appearing:
                    base.FormBorderStyle = FormBorderStyle.None;
                    if (base.Height >= this.BackgroundBitmap.Height)
                    {
                        this.timer.Stop();
                        base.Height = this.BackgroundBitmap.Height;
                        this.timer.Interval = this.nVisibleEvents;
                        this.taskbarState = TaskbarStates.visible;
                        this.timer.Start();
                        return;
                    }
                    base.SetBounds(base.Left, base.Top - this.nIncrementShow, base.Width, base.Height + this.nIncrementShow);
                    return;

                case TaskbarStates.visible:
                    this.timer.Stop();
                    this.timer.Interval = this.nHideEvents;
                    this.taskbarState = TaskbarStates.disappearing;
                    if (!this.AutoHide)
                    {
                        break;
                    }
                    this.timer.Start();
                    return;

                case TaskbarStates.disappearing:
                    if (base.Top >= this.WorkAreaRectangle.Bottom)
                    {
                        this.Hide();
                        break;
                    }
                    base.SetBounds(base.Left, base.Top + this.nIncrementHide, base.Width, base.Height - this.nIncrementHide);
                    return;

                default:
                    return;
            }
        }

        public void SetBackgroundBitmap(Image image, Color transparencyColor)
        {
            this.BackgroundBitmap = new Bitmap(image);
            base.Width = this.BackgroundBitmap.Width;
            base.Height = this.BackgroundBitmap.Height;
            base.Region = this.BitmapToRegion(this.BackgroundBitmap, transparencyColor);
        }

        public void SetBackgroundBitmap(string strFilename, Color transparencyColor)
        {
            this.BackgroundBitmap = new Bitmap(strFilename);
            base.Width = this.BackgroundBitmap.Width;
            base.Height = this.BackgroundBitmap.Height;
            base.Region = this.BitmapToRegion(this.BackgroundBitmap, transparencyColor);
        }

        public void SetCloseBitmap(Image image, Color transparencyColor, Point position)
        {
            this.CloseBitmap = new Bitmap(image);
            this.CloseBitmap.MakeTransparent(transparencyColor);
            this.CloseBitmapSize = new Size(this.CloseBitmap.Width / 3, this.CloseBitmap.Height);
            this.CloseBitmapLocation = position;
        }

        public void SetCloseBitmap(string strFilename, Color transparencyColor, Point position)
        {
            this.CloseBitmap = new Bitmap(strFilename);
            this.CloseBitmap.MakeTransparent(transparencyColor);
            this.CloseBitmapSize = new Size(this.CloseBitmap.Width / 3, this.CloseBitmap.Height);
            this.CloseBitmapLocation = position;
        }

        public void Show(string strTitle, string strContent, int nTimeToShow, int nTimeToStay, int nTimeToHide)
        {
            int num;
            this.WorkAreaRectangle = Screen.GetWorkingArea(this.WorkAreaRectangle);
            this.titleText = strTitle;
            this.contentText = strContent;
            this.nVisibleEvents = nTimeToStay;
            this.CalculateMouseRectangles();
            if (nTimeToShow > 10)
            {
                num = Math.Min(nTimeToShow / 10, this.BackgroundBitmap.Height);
                this.nShowEvents = nTimeToShow / num;
                this.nIncrementShow = this.BackgroundBitmap.Height / num;
            }
            else
            {
                this.nShowEvents = 10;
                this.nIncrementShow = this.BackgroundBitmap.Height;
            }
            if (nTimeToHide > 10)
            {
                num = Math.Min(nTimeToHide / 10, this.BackgroundBitmap.Height);
                this.nHideEvents = nTimeToHide / num;
                this.nIncrementHide = this.BackgroundBitmap.Height / num;
            }
            else
            {
                this.nHideEvents = 10;
                this.nIncrementHide = this.BackgroundBitmap.Height;
            }
            switch (this.taskbarState)
            {
                case TaskbarStates.hidden:
                    this.taskbarState = TaskbarStates.appearing;
                    base.SetBounds(this.WorkAreaRectangle.Left + 0x11, this.WorkAreaRectangle.Bottom - 1, this.BackgroundBitmap.Width, 0);
                    this.timer.Interval = this.nShowEvents;
                    this.timer.Start();
                    base.Show();
                    return;

                case TaskbarStates.appearing:
                    this.Refresh();
                    return;

                case TaskbarStates.visible:
                    this.timer.Stop();
                    this.timer.Interval = this.nVisibleEvents;
                    this.timer.Start();
                    this.Refresh();
                    return;

                case TaskbarStates.disappearing:
                    this.timer.Stop();
                    this.taskbarState = TaskbarStates.visible;
                    base.SetBounds(this.WorkAreaRectangle.Left + 0x11, (this.WorkAreaRectangle.Bottom - this.BackgroundBitmap.Height) - 1, this.BackgroundBitmap.Width, this.BackgroundBitmap.Height);
                    this.timer.Interval = this.nVisibleEvents;
                    this.timer.Start();
                    this.Refresh();
                    return;
            }
        }

        public string ContentText
        {
            get
            {
                return this.contentText;
            }
            set
            {
                this.contentText = value;
                this.Refresh();
            }
        }

        public Color HoverContentColor
        {
            get
            {
                return this.hoverContentColor;
            }
            set
            {
                this.hoverContentColor = value;
                this.Refresh();
            }
        }

        public Font HoverContentFont
        {
            get
            {
                return this.hoverContentFont;
            }
            set
            {
                this.hoverContentFont = value;
                this.Refresh();
            }
        }

        public Color HoverTitleColor
        {
            get
            {
                return this.hoverTitleColor;
            }
            set
            {
                this.hoverTitleColor = value;
                this.Refresh();
            }
        }

        public Font HoverTitleFont
        {
            get
            {
                return this.hoverTitleFont;
            }
            set
            {
                this.hoverTitleFont = value;
                this.Refresh();
            }
        }

        public Color NormalContentColor
        {
            get
            {
                return this.normalContentColor;
            }
            set
            {
                this.normalContentColor = value;
                this.Refresh();
            }
        }

        public Font NormalContentFont
        {
            get
            {
                return this.normalContentFont;
            }
            set
            {
                this.normalContentFont = value;
                this.Refresh();
            }
        }

        public Color NormalTitleColor
        {
            get
            {
                return this.normalTitleColor;
            }
            set
            {
                this.normalTitleColor = value;
                this.Refresh();
            }
        }

        public Font NormalTitleFont
        {
            get
            {
                return this.normalTitleFont;
            }
            set
            {
                this.normalTitleFont = value;
                this.Refresh();
            }
        }

        public TaskbarStates TaskbarState
        {
            get
            {
                return this.taskbarState;
            }
        }

        public string TitleText
        {
            get
            {
                return this.titleText;
            }
            set
            {
                this.titleText = value;
                this.Refresh();
            }
        }

        public enum TaskbarStates
        {
            hidden,
            appearing,
            visible,
            disappearing
        }
    }
}
