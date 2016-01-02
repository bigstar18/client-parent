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
		private IContainer components;
		private bool _DoubleClickedEnabled;
		private bool _AlwaysDropDown;
		private bool _AlwaysHoverChange;
		private bool _CalculateSplitRect = true;
		private bool _FillSplitHeight = true;
		private int _SplitHeight;
		private int _SplitWidth;
		private string _NormalImage;
		private string _HoverImage;
		private string _ClickedImage;
		private string _DisabledImage;
		private string _FocusedImage;
		private ImageList _DefaultSplitImages;
		[Browsable(true), Category("Action"), Description("Occurs when the button part of the SplitButton is clicked.")]
		public event EventHandler ButtonClick;
		[Browsable(true), Category("Action"), Description("Occurs when the button part of the SplitButton is clicked.")]
		public event EventHandler ButtonDoubleClick;
		[Category("Behavior"), DefaultValue(false), Description("Indicates whether the double click event is raised on the SplitButton")]
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
		[Category("Split Button"), DefaultValue(false), Description("Indicates whether the SplitButton always shows the drop down menu even if the button part of the SplitButton is clicked.")]
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
		[Category("Split Button"), DefaultValue(false), Description("Indicates whether the SplitButton always shows the Hover image status in the split part even if the button part of the SplitButton is hovered.")]
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
		[Category("Split Button"), DefaultValue(true), Description("Indicates whether the split rectange must be calculated (basing on Split image size)")]
		public bool CalculateSplitRect
		{
			get
			{
				return this._CalculateSplitRect;
			}
			set
			{
				bool calculateSplitRect = this._CalculateSplitRect;
				this._CalculateSplitRect = value;
				if (calculateSplitRect != this._CalculateSplitRect && this._SplitWidth > 0 && this._SplitHeight > 0)
				{
					this.InitDefaultSplitImages(true);
				}
			}
		}
		[Category("Split Button"), DefaultValue(true), Description("Indicates whether the split height must be filled to the button height even if the split image height is lower.")]
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
		[Category("Split Button"), DefaultValue(0), Description("The split height (ignored if CalculateSplitRect is setted to true).")]
		public int SplitHeight
		{
			get
			{
				return this._SplitHeight;
			}
			set
			{
				this._SplitHeight = value;
				if (!this._CalculateSplitRect && this._SplitWidth > 0 && this._SplitHeight > 0)
				{
					this.InitDefaultSplitImages(true);
				}
			}
		}
		[Category("Split Button"), DefaultValue(0), Description("The split width (ignored if CalculateSplitRect is setted to true).")]
		public int SplitWidth
		{
			get
			{
				return this._SplitWidth;
			}
			set
			{
				this._SplitWidth = value;
				if (!this._CalculateSplitRect && this._SplitWidth > 0 && this._SplitHeight > 0)
				{
					this.InitDefaultSplitImages(true);
				}
			}
		}
		[Category("Split Button Images"), DefaultValue(""), Description("The Normal status image name in the ImageList."), Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true), RefreshProperties(RefreshProperties.Repaint), TypeConverter(typeof(ImageKeyConverter))]
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
		[Category("Split Button Images"), DefaultValue(""), Description("The Hover status image name in the ImageList."), Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true), RefreshProperties(RefreshProperties.Repaint), TypeConverter(typeof(ImageKeyConverter))]
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
		[Category("Split Button Images"), DefaultValue(""), Description("The Clicked status image name in the ImageList."), Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true), RefreshProperties(RefreshProperties.Repaint), TypeConverter(typeof(ImageKeyConverter))]
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
		[Category("Split Button Images"), DefaultValue(""), Description("The Disabled status image name in the ImageList."), Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true), RefreshProperties(RefreshProperties.Repaint), TypeConverter(typeof(ImageKeyConverter))]
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
		[Category("Split Button Images"), DefaultValue(""), Description("The Focused status image name in the ImageList."), Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true), RefreshProperties(RefreshProperties.Repaint), TypeConverter(typeof(ImageKeyConverter))]
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
		}
		public SplitButton()
		{
			this.InitializeComponent();
		}
		protected override void OnCreateControl()
		{
			this.InitDefaultSplitImages();
			if (base.ImageList == null)
			{
				base.ImageList = this._DefaultSplitImages;
			}
			if (base.Enabled)
			{
				this.SetSplit(this._NormalImage);
			}
			else
			{
				this.SetSplit(this._DisabledImage);
			}
			base.OnCreateControl();
		}
		private void InitDefaultSplitImages()
		{
			this.InitDefaultSplitImages(false);
		}
		private void InitDefaultSplitImages(bool refresh)
		{
			if (string.IsNullOrEmpty(this._NormalImage))
			{
				this._NormalImage = "Normal";
			}
			if (string.IsNullOrEmpty(this._HoverImage))
			{
				this._HoverImage = "Hover";
			}
			if (string.IsNullOrEmpty(this._ClickedImage))
			{
				this._ClickedImage = "Clicked";
			}
			if (string.IsNullOrEmpty(this._DisabledImage))
			{
				this._DisabledImage = "Disabled";
			}
			if (string.IsNullOrEmpty(this._FocusedImage))
			{
				this._FocusedImage = "Focused";
			}
			if (this._DefaultSplitImages == null)
			{
				this._DefaultSplitImages = new ImageList();
			}
			if (this._DefaultSplitImages.Images.Count == 0 || refresh)
			{
				if (this._DefaultSplitImages.Images.Count > 0)
				{
					this._DefaultSplitImages.Images.Clear();
				}
				try
				{
					int num;
					if (!this._CalculateSplitRect && this._SplitWidth > 0)
					{
						num = this._SplitWidth;
					}
					else
					{
						num = 18;
					}
					int num2;
					if (!this.CalculateSplitRect && this.SplitHeight > 0)
					{
						num2 = this.SplitHeight;
					}
					else
					{
						num2 = base.Height;
					}
					num2 -= 8;
					this._DefaultSplitImages.ImageSize = new Size(num, num2);
					int num3 = num / 2;
					num3 += num3 % 2;
					int num4 = num2 / 2;
					Pen pen = new Pen(this.ForeColor, 1f);
					SolidBrush solidBrush = new SolidBrush(this.ForeColor);
					Bitmap image = new Bitmap(num, num2);
					Graphics graphics = Graphics.FromImage(image);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num2 - 2));
					graphics.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num2));
					graphics.FillPolygon(solidBrush, new Point[]
					{
						new Point(num3 - 2, num4 - 1),
						new Point(num3 + 3, num4 - 1),
						new Point(num3, num4 + 2)
					});
					graphics.Dispose();
					Bitmap image2 = new Bitmap(num, num2);
					graphics = Graphics.FromImage(image2);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num2 - 2));
					graphics.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num2));
					graphics.FillPolygon(solidBrush, new Point[]
					{
						new Point(num3 - 3, num4 - 2),
						new Point(num3 + 4, num4 - 2),
						new Point(num3, num4 + 2)
					});
					graphics.Dispose();
					Bitmap image3 = new Bitmap(num, num2);
					graphics = Graphics.FromImage(image3);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num2 - 2));
					graphics.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num2));
					graphics.FillPolygon(solidBrush, new Point[]
					{
						new Point(num3 - 2, num4 - 1),
						new Point(num3 + 3, num4 - 1),
						new Point(num3, num4 + 2)
					});
					graphics.Dispose();
					Bitmap image4 = new Bitmap(num, num2);
					graphics = Graphics.FromImage(image4);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.DrawLine(SystemPens.GrayText, new Point(1, 1), new Point(1, num2 - 2));
					graphics.FillPolygon(new SolidBrush(SystemColors.GrayText), new Point[]
					{
						new Point(num3 - 2, num4 - 1),
						new Point(num3 + 3, num4 - 1),
						new Point(num3, num4 + 2)
					});
					graphics.Dispose();
					Bitmap image5 = new Bitmap(num, num2);
					graphics = Graphics.FromImage(image5);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, num2 - 2));
					graphics.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, num2));
					graphics.FillPolygon(solidBrush, new Point[]
					{
						new Point(num3 - 2, num4 - 1),
						new Point(num3 + 3, num4 - 1),
						new Point(num3, num4 + 2)
					});
					graphics.Dispose();
					pen.Dispose();
					solidBrush.Dispose();
					this._DefaultSplitImages.Images.Add(this._NormalImage, image);
					this._DefaultSplitImages.Images.Add(this._HoverImage, image2);
					this._DefaultSplitImages.Images.Add(this._ClickedImage, image3);
					this._DefaultSplitImages.Images.Add(this._DisabledImage, image4);
					this._DefaultSplitImages.Images.Add(this._FocusedImage, image5);
				}
				catch
				{
				}
			}
		}
		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			if (this._AlwaysDropDown || this._AlwaysHoverChange || this.MouseInSplit())
			{
				if (base.Enabled)
				{
					this.SetSplit(this._HoverImage);
				}
			}
			else
			{
				if (base.Enabled)
				{
					this.SetSplit(this._NormalImage);
				}
			}
			base.OnMouseMove(mevent);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if (base.Enabled)
			{
				this.SetSplit(this._NormalImage);
			}
			base.OnMouseLeave(e);
		}
		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			if (this._AlwaysDropDown || this.MouseInSplit())
			{
				if (base.Enabled)
				{
					this.SetSplit(this._ClickedImage);
					if (this.ContextMenuStrip != null && this.ContextMenuStrip.Items.Count > 0)
					{
						this.ContextMenuStrip.Show(this, new Point(0, base.Height));
					}
				}
			}
			else
			{
				if (base.Enabled)
				{
					this.SetSplit(this._NormalImage);
				}
			}
			base.OnMouseDown(mevent);
		}
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (this._AlwaysDropDown || this._AlwaysHoverChange || this.MouseInSplit())
			{
				if (base.Enabled)
				{
					this.SetSplit(this._HoverImage);
				}
			}
			else
			{
				if (base.Enabled)
				{
					this.SetSplit(this._NormalImage);
				}
			}
			base.OnMouseUp(mevent);
		}
		protected override void OnEnabledChanged(EventArgs e)
		{
			if (!base.Enabled)
			{
				this.SetSplit(this._DisabledImage);
			}
			else
			{
				if (this.MouseInSplit())
				{
					this.SetSplit(this._HoverImage);
				}
				else
				{
					this.SetSplit(this._NormalImage);
				}
			}
			base.OnEnabledChanged(e);
		}
		protected override void OnGotFocus(EventArgs e)
		{
			if (base.Enabled)
			{
				this.SetSplit(this._FocusedImage);
			}
			base.OnGotFocus(e);
		}
		protected override void OnLostFocus(EventArgs e)
		{
			if (base.Enabled)
			{
				this.SetSplit(this._NormalImage);
			}
			base.OnLostFocus(e);
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (!this.MouseInSplit() && !this._AlwaysDropDown && this.ButtonClick != null)
			{
				this.ButtonClick(this, e);
			}
		}
		protected override void OnDoubleClick(EventArgs e)
		{
			if (this._DoubleClickedEnabled)
			{
				base.OnDoubleClick(e);
				if (!this.MouseInSplit() && !this._AlwaysDropDown && this.ButtonClick != null)
				{
					this.ButtonDoubleClick(this, e);
				}
			}
		}
		private void SetSplit(string imageName)
		{
			if (imageName != null && base.ImageList != null && base.ImageList.Images.ContainsKey(imageName))
			{
				base.ImageKey = imageName;
			}
		}
		public bool MouseInSplit()
		{
			return this.PointInSplit(base.PointToClient(Control.MousePosition));
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
			if (image != null)
			{
				int num = 0;
				int num2 = 0;
				int num3 = image.Width + 1;
				int num4 = image.Height + 1;
				if (num3 > base.Width)
				{
					num3 = base.Width;
				}
				if (num4 > base.Width)
				{
					num4 = base.Width;
				}
				ContentAlignment imageAlign = base.ImageAlign;
				if (imageAlign <= ContentAlignment.MiddleCenter)
				{
					switch (imageAlign)
					{
					case ContentAlignment.TopLeft:
						num = 0;
						num2 = 0;
						break;
					case ContentAlignment.TopCenter:
						num = (base.Width - num3) / 2;
						num2 = 0;
						if ((base.Width - num3) % 2 > 0)
						{
							num++;
						}
						break;
					case (ContentAlignment)3:
						break;
					case ContentAlignment.TopRight:
						num = base.Width - num3;
						num2 = 0;
						break;
					default:
						if (imageAlign != ContentAlignment.MiddleLeft)
						{
							if (imageAlign == ContentAlignment.MiddleCenter)
							{
								num = (base.Width - num3) / 2;
								num2 = (base.Height - num4) / 2;
								if ((base.Width - num3) % 2 > 0)
								{
									num++;
								}
								if ((base.Height - num4) % 2 > 0)
								{
									num2++;
								}
							}
						}
						else
						{
							num = 0;
							num2 = (base.Height - num4) / 2;
							if ((base.Height - num4) % 2 > 0)
							{
								num2++;
							}
						}
						break;
					}
				}
				else
				{
					if (imageAlign <= ContentAlignment.BottomLeft)
					{
						if (imageAlign != ContentAlignment.MiddleRight)
						{
							if (imageAlign == ContentAlignment.BottomLeft)
							{
								num = 0;
								num2 = base.Height - num4;
								if ((base.Height - num4) % 2 > 0)
								{
									num2++;
								}
							}
						}
						else
						{
							num = base.Width - num3;
							num2 = (base.Height - num4) / 2;
							if ((base.Height - num4) % 2 > 0)
							{
								num2++;
							}
						}
					}
					else
					{
						if (imageAlign != ContentAlignment.BottomCenter)
						{
							if (imageAlign == ContentAlignment.BottomRight)
							{
								num = base.Width - num3;
								num2 = base.Height - num4;
							}
						}
						else
						{
							num = (base.Width - num3) / 2;
							num2 = base.Height - num4;
							if ((base.Width - num3) % 2 > 0)
							{
								num++;
							}
						}
					}
				}
				if (this._FillSplitHeight && num4 < base.Height)
				{
					num4 = base.Height;
				}
				if (num > 0)
				{
					num--;
				}
				if (num2 > 0)
				{
					num2--;
				}
				return new Rectangle(num, num2, num3, num4);
			}
			return Rectangle.Empty;
		}
		private Image GetImage(string imageName)
		{
			if (base.ImageList != null && base.ImageList.Images.ContainsKey(imageName))
			{
				return base.ImageList.Images[imageName];
			}
			return null;
		}
	}
}
