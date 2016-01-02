using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace ToolsLibrary
{
	public class LoadingCircle : UserControl
	{
		public enum StylePresets
		{
			MacOSX,
			Firefox,
			IE7,
			Custom
		}
		private const double NumberOfDegreesInCircle = 360.0;
		private const double NumberOfDegreesInHalfCircle = 180.0;
		private const int DefaultInnerCircleRadius = 8;
		private const int DefaultOuterCircleRadius = 10;
		private const int DefaultNumberOfSpoke = 10;
		private const int DefaultSpokeThickness = 4;
		private const int MacOSXInnerCircleRadius = 5;
		private const int MacOSXOuterCircleRadius = 11;
		private const int MacOSXNumberOfSpoke = 12;
		private const int MacOSXSpokeThickness = 2;
		private const int FireFoxInnerCircleRadius = 6;
		private const int FireFoxOuterCircleRadius = 7;
		private const int FireFoxNumberOfSpoke = 9;
		private const int FireFoxSpokeThickness = 4;
		private const int IE7InnerCircleRadius = 8;
		private const int IE7OuterCircleRadius = 9;
		private const int IE7NumberOfSpoke = 24;
		private const int IE7SpokeThickness = 4;
		private readonly Color DefaultColor = Color.DarkGray;
		private Timer m_Timer;
		private bool m_IsTimerActive;
		private int m_NumberOfSpoke;
		private int m_SpokeThickness;
		private int m_ProgressValue;
		private int m_OuterCircleRadius;
		private int m_InnerCircleRadius;
		private PointF m_CenterPoint;
		private Color m_Color;
		private Color[] m_Colors;
		private double[] m_Angles;
		private LoadingCircle.StylePresets m_StylePreset;
		private IContainer components;
		[Category("LoadingCircle"), Description("获取和设置控件高亮色"), TypeConverter("System.Drawing.ColorConverter")]
		public Color Color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
				this.GenerateColorsPallet();
				base.Invalidate();
			}
		}
		[Category("LoadingCircle"), Description("获取和设置外围半径")]
		public int OuterCircleRadius
		{
			get
			{
				if (this.m_OuterCircleRadius == 0)
				{
					this.m_OuterCircleRadius = 10;
				}
				return this.m_OuterCircleRadius;
			}
			set
			{
				this.m_OuterCircleRadius = value;
				base.Invalidate();
			}
		}
		[Category("LoadingCircle"), Description("获取和设置内圆半径")]
		public int InnerCircleRadius
		{
			get
			{
				if (this.m_InnerCircleRadius == 0)
				{
					this.m_InnerCircleRadius = 8;
				}
				return this.m_InnerCircleRadius;
			}
			set
			{
				this.m_InnerCircleRadius = value;
				base.Invalidate();
			}
		}
		[Category("LoadingCircle"), Description("获取和设置辐条数量")]
		public int NumberSpoke
		{
			get
			{
				if (this.m_NumberOfSpoke == 0)
				{
					this.m_NumberOfSpoke = 10;
				}
				return this.m_NumberOfSpoke;
			}
			set
			{
				if (this.m_NumberOfSpoke != value && this.m_NumberOfSpoke > 0)
				{
					this.m_NumberOfSpoke = value;
					this.GenerateColorsPallet();
					this.GetSpokesAngles();
					base.Invalidate();
				}
			}
		}
		[Category("LoadingCircle"), Description("获取和设置一个布尔值，表示当前控件是否激活。")]
		public bool Active
		{
			get
			{
				return this.m_IsTimerActive;
			}
			set
			{
				this.m_IsTimerActive = value;
				this.ActiveTimer();
			}
		}
		[Category("LoadingCircle"), Description("获取和设置辐条粗细程度。")]
		public int SpokeThickness
		{
			get
			{
				if (this.m_SpokeThickness <= 0)
				{
					this.m_SpokeThickness = 4;
				}
				return this.m_SpokeThickness;
			}
			set
			{
				this.m_SpokeThickness = value;
				base.Invalidate();
			}
		}
		[Category("LoadingCircle"), Description("获取和设置旋转速度。")]
		public int RotationSpeed
		{
			get
			{
				return this.m_Timer.Interval;
			}
			set
			{
				if (value > 0)
				{
					this.m_Timer.Interval = value;
				}
			}
		}
		[Category("LoadingCircle"), DefaultValue(typeof(LoadingCircle.StylePresets), "Custom"), Description("快速设置预定义风格。")]
		public LoadingCircle.StylePresets StylePreset
		{
			get
			{
				return this.m_StylePreset;
			}
			set
			{
				this.m_StylePreset = value;
				switch (this.m_StylePreset)
				{
				case LoadingCircle.StylePresets.MacOSX:
					this.SetCircleAppearance(12, 2, 5, 11);
					return;
				case LoadingCircle.StylePresets.Firefox:
					this.SetCircleAppearance(9, 4, 6, 7);
					return;
				case LoadingCircle.StylePresets.IE7:
					this.SetCircleAppearance(24, 4, 8, 9);
					return;
				case LoadingCircle.StylePresets.Custom:
					this.SetCircleAppearance(10, 4, 8, 10);
					return;
				default:
					return;
				}
			}
		}
		public LoadingCircle()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.m_Color = this.DefaultColor;
			this.GenerateColorsPallet();
			this.GetSpokesAngles();
			this.GetControlCenterPoint();
			this.m_Timer = new Timer();
			this.m_Timer.Tick += new EventHandler(this.aTimer_Tick);
			this.ActiveTimer();
			base.Resize += new EventHandler(this.LoadingCircle_Resize);
		}
		private void LoadingCircle_Resize(object sender, EventArgs e)
		{
			this.GetControlCenterPoint();
		}
		private void aTimer_Tick(object sender, EventArgs e)
		{
			this.m_ProgressValue = ++this.m_ProgressValue % this.m_NumberOfSpoke;
			base.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.m_NumberOfSpoke > 0)
			{
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
				int num = this.m_ProgressValue;
				for (int i = 0; i < this.m_NumberOfSpoke; i++)
				{
					num %= this.m_NumberOfSpoke;
					this.DrawLine(e.Graphics, this.GetCoordinate(this.m_CenterPoint, this.m_InnerCircleRadius, this.m_Angles[num]), this.GetCoordinate(this.m_CenterPoint, this.m_OuterCircleRadius, this.m_Angles[num]), this.m_Colors[i], this.m_SpokeThickness);
					num++;
				}
			}
			base.OnPaint(e);
		}
		private Color Darken(Color _objColor, int _intPercent)
		{
			int r = (int)_objColor.R;
			int g = (int)_objColor.G;
			int b = (int)_objColor.B;
			return Color.FromArgb(_intPercent, Math.Min(r, 255), Math.Min(g, 255), Math.Min(b, 255));
		}
		private void GenerateColorsPallet()
		{
			this.m_Colors = this.GenerateColorsPallet(this.m_Color, this.Active, this.m_NumberOfSpoke);
		}
		private Color[] GenerateColorsPallet(Color _objColor, bool _blnShadeColor, int _intNbSpoke)
		{
			Color[] array = new Color[this.NumberSpoke];
			byte b = (byte)(255 / this.NumberSpoke);
			byte b2 = 0;
			for (int i = 0; i < this.NumberSpoke; i++)
			{
				if (_blnShadeColor)
				{
					if (i == 0 || i < this.NumberSpoke - _intNbSpoke)
					{
						array[i] = _objColor;
					}
					else
					{
						b2 += b;
						if (b2 > 255)
						{
							b2 = 255;
						}
						array[i] = this.Darken(_objColor, (int)b2);
					}
				}
				else
				{
					array[i] = _objColor;
				}
			}
			return array;
		}
		private void GetControlCenterPoint()
		{
			this.m_CenterPoint = this.GetControlCenterPoint(this);
		}
		private PointF GetControlCenterPoint(Control _objControl)
		{
			return new PointF((float)(_objControl.Width / 2), (float)(_objControl.Height / 2 - 1));
		}
		private void DrawLine(Graphics _objGraphics, PointF _objPointOne, PointF _objPointTwo, Color _objColor, int _intLineThickness)
		{
			using (Pen pen = new Pen(new SolidBrush(_objColor), (float)_intLineThickness))
			{
				pen.StartCap = LineCap.Round;
				pen.EndCap = LineCap.Round;
				_objGraphics.DrawLine(pen, _objPointOne, _objPointTwo);
			}
		}
		private PointF GetCoordinate(PointF _objCircleCenter, int _intRadius, double _dblAngle)
		{
			double num = 3.1415926535897931 * _dblAngle / 180.0;
			return new PointF(_objCircleCenter.X + (float)_intRadius * (float)Math.Cos(num), _objCircleCenter.Y + (float)_intRadius * (float)Math.Sin(num));
		}
		private void GetSpokesAngles()
		{
			this.m_Angles = this.GetSpokesAngles(this.NumberSpoke);
		}
		private double[] GetSpokesAngles(int _intNumberSpoke)
		{
			double[] array = new double[_intNumberSpoke];
			double num = 360.0 / (double)_intNumberSpoke;
			for (int i = 0; i < _intNumberSpoke; i++)
			{
				array[i] = ((i == 0) ? num : (array[i - 1] + num));
			}
			return array;
		}
		private void ActiveTimer()
		{
			if (this.m_IsTimerActive)
			{
				this.m_Timer.Start();
			}
			else
			{
				this.m_Timer.Stop();
				this.m_ProgressValue = 0;
			}
			this.GenerateColorsPallet();
			base.Invalidate();
		}
		public override Size GetPreferredSize(Size proposedSize)
		{
			proposedSize.Width = (this.m_OuterCircleRadius + this.m_SpokeThickness) * 2;
			return proposedSize;
		}
		public void SetCircleAppearance(int numberSpoke, int spokeThickness, int innerCircleRadius, int outerCircleRadius)
		{
			this.NumberSpoke = numberSpoke;
			this.SpokeThickness = spokeThickness;
			this.InnerCircleRadius = innerCircleRadius;
			this.OuterCircleRadius = outerCircleRadius;
			base.Invalidate();
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
		}
	}
}
