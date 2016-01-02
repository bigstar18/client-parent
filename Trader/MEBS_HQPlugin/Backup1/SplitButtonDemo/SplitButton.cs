// Decompiled with JetBrains decompiler
// Type: SplitButtonDemo.SplitButton
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SplitButtonDemo
{
  public class SplitButton : Button
  {
    private bool _CalculateSplitRect = true;
    private bool _FillSplitHeight = true;
    private IContainer components;
    private bool _DoubleClickedEnabled;
    private bool _AlwaysDropDown;
    private bool _AlwaysHoverChange;
    private int _SplitHeight;
    private int _SplitWidth;
    private string _NormalImage;
    private string _HoverImage;
    private string _ClickedImage;
    private string _DisabledImage;
    private string _FocusedImage;
    private ImageList _DefaultSplitImages;

    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Indicates whether the double click event is raised on the SplitButton")]
    public bool DoubleClickedEnabled
    {
      get
      {
        return this._DoubleClickedEnabled;
      }
      set
      {
        this._DoubleClickedEnabled = value;
      }
    }

    [DefaultValue(false)]
    [Description("Indicates whether the SplitButton always shows the drop down menu even if the button part of the SplitButton is clicked.")]
    [Category("Split Button")]
    public bool AlwaysDropDown
    {
      get
      {
        return this._AlwaysDropDown;
      }
      set
      {
        this._AlwaysDropDown = value;
      }
    }

    [Category("Split Button")]
    [DefaultValue(false)]
    [Description("Indicates whether the SplitButton always shows the Hover image status in the split part even if the button part of the SplitButton is hovered.")]
    public bool AlwaysHoverChange
    {
      get
      {
        return this._AlwaysHoverChange;
      }
      set
      {
        this._AlwaysHoverChange = value;
      }
    }

    [Category("Split Button")]
    [Description("Indicates whether the split rectange must be calculated (basing on Split image size)")]
    [DefaultValue(true)]
    public bool CalculateSplitRect
    {
      get
      {
        return this._CalculateSplitRect;
      }
      set
      {
        bool flag = this._CalculateSplitRect;
        this._CalculateSplitRect = value;
        if (flag == this._CalculateSplitRect || this._SplitWidth <= 0 || this._SplitHeight <= 0)
          return;
        this.InitDefaultSplitImages(true);
      }
    }

    [DefaultValue(true)]
    [Description("Indicates whether the split height must be filled to the button height even if the split image height is lower.")]
    [Category("Split Button")]
    public bool FillSplitHeight
    {
      get
      {
        return this._FillSplitHeight;
      }
      set
      {
        this._FillSplitHeight = value;
      }
    }

    [Category("Split Button")]
    [Description("The split height (ignored if CalculateSplitRect is setted to true).")]
    [DefaultValue(0)]
    public int SplitHeight
    {
      get
      {
        return this._SplitHeight;
      }
      set
      {
        this._SplitHeight = value;
        if (this._CalculateSplitRect || this._SplitWidth <= 0 || this._SplitHeight <= 0)
          return;
        this.InitDefaultSplitImages(true);
      }
    }

    [DefaultValue(0)]
    [Category("Split Button")]
    [Description("The split width (ignored if CalculateSplitRect is setted to true).")]
    public int SplitWidth
    {
      get
      {
        return this._SplitWidth;
      }
      set
      {
        this._SplitWidth = value;
        if (this._CalculateSplitRect || this._SplitWidth <= 0 || this._SplitHeight <= 0)
          return;
        this.InitDefaultSplitImages(true);
      }
    }

    [DefaultValue("")]
    [Description("The Normal status image name in the ImageList.")]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [TypeConverter(typeof (ImageKeyConverter))]
    [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [Category("Split Button Images")]
    public string NormalImage
    {
      get
      {
        return this._NormalImage;
      }
      set
      {
        this._NormalImage = value;
      }
    }

    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [TypeConverter(typeof (ImageKeyConverter))]
    [Description("The Hover status image name in the ImageList.")]
    [Category("Split Button Images")]
    public string HoverImage
    {
      get
      {
        return this._HoverImage;
      }
      set
      {
        this._HoverImage = value;
      }
    }

    [Localizable(true)]
    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [Category("Split Button Images")]
    [RefreshProperties(RefreshProperties.Repaint)]
    [TypeConverter(typeof (ImageKeyConverter))]
    [Description("The Clicked status image name in the ImageList.")]
    public string ClickedImage
    {
      get
      {
        return this._ClickedImage;
      }
      set
      {
        this._ClickedImage = value;
      }
    }

    [RefreshProperties(RefreshProperties.Repaint)]
    [TypeConverter(typeof (ImageKeyConverter))]
    [Description("The Disabled status image name in the ImageList.")]
    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [Localizable(true)]
    [Category("Split Button Images")]
    public string DisabledImage
    {
      get
      {
        return this._DisabledImage;
      }
      set
      {
        this._DisabledImage = value;
      }
    }

    [Category("Split Button Images")]
    [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [TypeConverter(typeof (ImageKeyConverter))]
    [DefaultValue("")]
    [Description("The Focused status image name in the ImageList.")]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]
    public string FocusedImage
    {
      get
      {
        return this._FocusedImage;
      }
      set
      {
        this._FocusedImage = value;
      }
    }

    [Description("Occurs when the button part of the SplitButton is clicked.")]
    [Browsable(true)]
    [Category("Action")]
    public event EventHandler ButtonClick;

    [Browsable(true)]
    [Description("Occurs when the button part of the SplitButton is clicked.")]
    [Category("Action")]
    public event EventHandler ButtonDoubleClick;

    public SplitButton()
    {
      this.InitializeComponent();
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
    }

    protected override void OnCreateControl()
    {
      this.InitDefaultSplitImages();
      if (this.ImageList == null)
        this.ImageList = this._DefaultSplitImages;
      if (this.Enabled)
        this.SetSplit(this._NormalImage);
      else
        this.SetSplit(this._DisabledImage);
      base.OnCreateControl();
    }

    private void InitDefaultSplitImages()
    {
      this.InitDefaultSplitImages(false);
    }

    private void InitDefaultSplitImages(bool refresh)
    {
      if (string.IsNullOrEmpty(this._NormalImage))
        this._NormalImage = "Normal";
      if (string.IsNullOrEmpty(this._HoverImage))
        this._HoverImage = "Hover";
      if (string.IsNullOrEmpty(this._ClickedImage))
        this._ClickedImage = "Clicked";
      if (string.IsNullOrEmpty(this._DisabledImage))
        this._DisabledImage = "Disabled";
      if (string.IsNullOrEmpty(this._FocusedImage))
        this._FocusedImage = "Focused";
      if (this._DefaultSplitImages == null)
        this._DefaultSplitImages = new ImageList();
      if (this._DefaultSplitImages.Images.Count != 0 && !refresh)
        return;
      if (this._DefaultSplitImages.Images.Count > 0)
        this._DefaultSplitImages.Images.Clear();
      try
      {
        int width = this._CalculateSplitRect || this._SplitWidth <= 0 ? 18 : this._SplitWidth;
        int num1 = (this.CalculateSplitRect || this.SplitHeight <= 0 ? this.Height : this.SplitHeight) - 8;
        this._DefaultSplitImages.ImageSize = new Size(width, num1);
        int num2 = width / 2;
        int x = num2 + num2 % 2;
        int num3 = num1 / 2;
        Pen pen = new Pen(this.ForeColor, 1f);
        SolidBrush solidBrush = new SolidBrush(this.ForeColor);
        Bitmap bitmap1 = new Bitmap(width, num1);
        Graphics graphics1 = Graphics.FromImage((Image) bitmap1);
        graphics1.CompositingQuality = CompositingQuality.HighQuality;
        graphics1.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num1 - 2));
        graphics1.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num1));
        graphics1.FillPolygon((Brush) solidBrush, new Point[3]
        {
          new Point(x - 2, num3 - 1),
          new Point(x + 3, num3 - 1),
          new Point(x, num3 + 2)
        });
        graphics1.Dispose();
        Bitmap bitmap2 = new Bitmap(width, num1);
        Graphics graphics2 = Graphics.FromImage((Image) bitmap2);
        graphics2.CompositingQuality = CompositingQuality.HighQuality;
        graphics2.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num1 - 2));
        graphics2.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num1));
        graphics2.FillPolygon((Brush) solidBrush, new Point[3]
        {
          new Point(x - 3, num3 - 2),
          new Point(x + 4, num3 - 2),
          new Point(x, num3 + 2)
        });
        graphics2.Dispose();
        Bitmap bitmap3 = new Bitmap(width, num1);
        Graphics graphics3 = Graphics.FromImage((Image) bitmap3);
        graphics3.CompositingQuality = CompositingQuality.HighQuality;
        graphics3.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num1 - 2));
        graphics3.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num1));
        graphics3.FillPolygon((Brush) solidBrush, new Point[3]
        {
          new Point(x - 2, num3 - 1),
          new Point(x + 3, num3 - 1),
          new Point(x, num3 + 2)
        });
        graphics3.Dispose();
        Bitmap bitmap4 = new Bitmap(width, num1);
        Graphics graphics4 = Graphics.FromImage((Image) bitmap4);
        graphics4.CompositingQuality = CompositingQuality.HighQuality;
        graphics4.DrawLine(SystemPens.GrayText, new Point(1, 1), new Point(1, num1 - 2));
        graphics4.FillPolygon((Brush) new SolidBrush(SystemColors.GrayText), new Point[3]
        {
          new Point(x - 2, num3 - 1),
          new Point(x + 3, num3 - 1),
          new Point(x, num3 + 2)
        });
        graphics4.Dispose();
        Bitmap bitmap5 = new Bitmap(width, num1);
        Graphics graphics5 = Graphics.FromImage((Image) bitmap5);
        graphics5.CompositingQuality = CompositingQuality.HighQuality;
        graphics5.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num1 - 2));
        graphics5.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num1));
        graphics5.FillPolygon((Brush) solidBrush, new Point[3]
        {
          new Point(x - 2, num3 - 1),
          new Point(x + 3, num3 - 1),
          new Point(x, num3 + 2)
        });
        graphics5.Dispose();
        pen.Dispose();
        solidBrush.Dispose();
        this._DefaultSplitImages.Images.Add(this._NormalImage, (Image) bitmap1);
        this._DefaultSplitImages.Images.Add(this._HoverImage, (Image) bitmap2);
        this._DefaultSplitImages.Images.Add(this._ClickedImage, (Image) bitmap3);
        this._DefaultSplitImages.Images.Add(this._DisabledImage, (Image) bitmap4);
        this._DefaultSplitImages.Images.Add(this._FocusedImage, (Image) bitmap5);
      }
      catch
      {
      }
    }

    protected override void OnMouseMove(MouseEventArgs mevent)
    {
      if (this._AlwaysDropDown || this._AlwaysHoverChange || this.MouseInSplit())
      {
        if (this.Enabled)
          this.SetSplit(this._HoverImage);
      }
      else if (this.Enabled)
        this.SetSplit(this._NormalImage);
      base.OnMouseMove(mevent);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      if (this.Enabled)
        this.SetSplit(this._NormalImage);
      base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
      if (this._AlwaysDropDown || this.MouseInSplit())
      {
        if (this.Enabled)
        {
          this.SetSplit(this._ClickedImage);
          if (this.ContextMenuStrip != null && this.ContextMenuStrip.Items.Count > 0)
            this.ContextMenuStrip.Show((Control) this, new Point(0, this.Height));
        }
      }
      else if (this.Enabled)
        this.SetSplit(this._NormalImage);
      base.OnMouseDown(mevent);
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
      if (this._AlwaysDropDown || this._AlwaysHoverChange || this.MouseInSplit())
      {
        if (this.Enabled)
          this.SetSplit(this._HoverImage);
      }
      else if (this.Enabled)
        this.SetSplit(this._NormalImage);
      base.OnMouseUp(mevent);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
      if (!this.Enabled)
        this.SetSplit(this._DisabledImage);
      else if (this.MouseInSplit())
        this.SetSplit(this._HoverImage);
      else
        this.SetSplit(this._NormalImage);
      base.OnEnabledChanged(e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
      if (this.Enabled)
        this.SetSplit(this._FocusedImage);
      base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
      if (this.Enabled)
        this.SetSplit(this._NormalImage);
      base.OnLostFocus(e);
    }

    protected override void OnClick(EventArgs e)
    {
      base.OnClick(e);
      if (this.MouseInSplit() || this._AlwaysDropDown || this.ButtonClick == null)
        return;
      this.ButtonClick((object) this, e);
    }

    protected override void OnDoubleClick(EventArgs e)
    {
      if (!this._DoubleClickedEnabled)
        return;
      base.OnDoubleClick(e);
      if (this.MouseInSplit() || this._AlwaysDropDown || this.ButtonClick == null)
        return;
      this.ButtonDoubleClick((object) this, e);
    }

    private void SetSplit(string imageName)
    {
      if (imageName == null || this.ImageList == null || !this.ImageList.Images.ContainsKey(imageName))
        return;
      this.ImageKey = imageName;
    }

    public bool MouseInSplit()
    {
      return this.PointInSplit(this.PointToClient(Control.MousePosition));
    }

    public bool PointInSplit(Point pt)
    {
      Rectangle imageRect = this.GetImageRect(this._NormalImage);
      if (!this._CalculateSplitRect)
      {
        imageRect.Width = this._SplitWidth;
        imageRect.Height = this._SplitHeight;
      }
      return imageRect.Contains(pt);
    }

    public Rectangle GetImageRect(string imageKey)
    {
      Image image = this.GetImage(imageKey);
      if (image == null)
        return Rectangle.Empty;
      int x = 0;
      int y = 0;
      int width = image.Width + 1;
      int height = image.Height + 1;
      if (width > this.Width)
        width = this.Width;
      if (height > this.Width)
        height = this.Width;
      switch (this.ImageAlign)
      {
        case ContentAlignment.BottomCenter:
          x = (this.Width - width) / 2;
          y = this.Height - height;
          if ((this.Width - width) % 2 > 0)
          {
            ++x;
            break;
          }
          break;
        case ContentAlignment.BottomRight:
          x = this.Width - width;
          y = this.Height - height;
          break;
        case ContentAlignment.MiddleRight:
          x = this.Width - width;
          y = (this.Height - height) / 2;
          if ((this.Height - height) % 2 > 0)
          {
            ++y;
            break;
          }
          break;
        case ContentAlignment.BottomLeft:
          x = 0;
          y = this.Height - height;
          if ((this.Height - height) % 2 > 0)
          {
            ++y;
            break;
          }
          break;
        case ContentAlignment.TopLeft:
          x = 0;
          y = 0;
          break;
        case ContentAlignment.TopCenter:
          x = (this.Width - width) / 2;
          y = 0;
          if ((this.Width - width) % 2 > 0)
          {
            ++x;
            break;
          }
          break;
        case ContentAlignment.TopRight:
          x = this.Width - width;
          y = 0;
          break;
        case ContentAlignment.MiddleLeft:
          x = 0;
          y = (this.Height - height) / 2;
          if ((this.Height - height) % 2 > 0)
          {
            ++y;
            break;
          }
          break;
        case ContentAlignment.MiddleCenter:
          x = (this.Width - width) / 2;
          y = (this.Height - height) / 2;
          if ((this.Width - width) % 2 > 0)
            ++x;
          if ((this.Height - height) % 2 > 0)
          {
            ++y;
            break;
          }
          break;
      }
      if (this._FillSplitHeight && height < this.Height)
        height = this.Height;
      if (x > 0)
        --x;
      if (y > 0)
        --y;
      return new Rectangle(x, y, width, height);
    }

    private Image GetImage(string imageName)
    {
      if (this.ImageList != null && this.ImageList.Images.ContainsKey(imageName))
        return this.ImageList.Images[imageName];
      return (Image) null;
    }
  }
}
