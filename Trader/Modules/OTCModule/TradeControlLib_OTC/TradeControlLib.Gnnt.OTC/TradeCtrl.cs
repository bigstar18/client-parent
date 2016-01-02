using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using TPME.Log;
namespace TradeControlLib.Gnnt.OTC
{
	public class TradeCtrl : UserControl
	{
		public delegate void TradeHanlder(TradeMenu _tradeMenu, string _commodityID, bool _buySell);
		public delegate bool ShowMenu(string name);
		private const double TradeNameXRatio = 0.38235294818878174;
		private const double TradeNameYRatio = 0.042016807943582535;
		private const double SellTextandImageXRatio = 0.0941176488995552;
		private const double BuyTextandImageXRatio = 0.56470588235294117;
		private const double NO1andNO2YRatio = 0.32773110270500183;
		private const double NO2andNO4XRatio = 0.20588235557079315;
		private const double NO1andNO3XRatio = 0.69411766529083252;
		private const double NO4andNO3YRatio = 0.5378151535987854;
		private const double NO5andNO6YRatio = 0.79831933975219727;
		private const double HightTextXRatio = 0.04117647185921669;
		private const double NO5XRatio = 0.20000000298023224;
		private const double LowTextXRatio = 0.51176470518112183;
		private const double NO6XRatio = 0.67058825492858887;
		private Image m_imgbg;
		private DrawProperties m_stDrawProperties = new DrawProperties();
		private double m_dSell;
		private double m_dBuy;
		private double m_curSell;
		private double m_curBuy;
		public ResourceManager m_rm;
		public static int m_iWidth = 170;
		public static int m_iHeight = 119;
		public Color m_crsText;
		public Image m_imgState;
		private Rectangle buyRect;
		private TradeMenu trademenu;
		public string CommodityID;
		private bool buySell;
		private int TradeNameX;
		private int TradeNameY;
		private int SellTextX;
		private int SellTextY;
		private int NO2X;
		private int NO2Y;
		private int LowandHightImg2X;
		private int LowandHightImg2Y;
		private int NO4X;
		private int NO4Y;
		private int HightTextX;
		private int HightTextY;
		private int NO5X;
		private int NO5Y;
		private int BuyTextX;
		private int BuyTextY;
		private int NO1X;
		private int NO1Y;
		private int LowandHightImg1X;
		private int LowandHightImg1Y;
		private int NO3X;
		private int NO3Y;
		private int LowTextX;
		private int LowTextY;
		private int NO6X;
		private int NO6Y;
		private IContainer components;
		private Label TradeName;
		private Label NO1;
		private Label LowandHightImg1;
		private Label NO2;
		private Label NO4;
		private Label NO5;
		private Label NO6;
		private Label NO3;
		private Label LowandHightImg2;
		private Label BuyText;
		private Label SellText;
		private Label HightText;
		private Label LowText;
		private ContextMenuStrip contextMenuStripHQCtrl;
		private ToolStripMenuItem TsMenuItemOpenPrice_S;
		private ToolStripMenuItem TsMenuItemClosePrice_S;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem TsMenuItemClosePrice_L;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem TSMenuItemCancel;
		public event TradeCtrl.TradeHanlder TradeRefreashed;
		public event TradeCtrl.ShowMenu _ShowMenu;
		public TradeCtrl()
		{
			try
			{
				this.InitializeComponent();
				this.m_rm = ResourceManager.CreateFileBasedResourceManager("PMES.cn", "", null);
				this.m_imgbg = (Image)this.m_rm.GetObject("bg.jpg");
				this.BackgroundImage = this.m_imgbg;
				base.SetBounds(0, 0, this.m_imgbg.Width, this.m_imgbg.Height);
				this.TradeCtrl_Resize(this, new EventArgs());
				Version version = Environment.Version;
				if (version.Major < 2)
				{
					base.SetStyle(ControlStyles.DoubleBuffer, true);
				}
				else
				{
					base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				}
				base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				base.SetStyle(ControlStyles.UserPaint, true);
				base.SetStyle(ControlStyles.ResizeRedraw, true);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetShowMenu(bool bshow)
		{
			try
			{
				if (bshow)
				{
					this.ContextMenuStrip = this.contextMenuStripHQCtrl;
				}
				else
				{
					this.ContextMenuStrip = null;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetBuyLable(string strval)
		{
			if (strval.Length == 0)
			{
				return;
			}
			this.BuyText.Text = strval;
		}
		public void SetSellLable(string strval)
		{
			if (strval.Length == 0)
			{
				return;
			}
			this.SellText.Text = strval;
		}
		public void SetLowLable(string strval)
		{
			if (strval.Length == 0)
			{
				return;
			}
			this.LowText.Text = strval;
		}
		public void SetHightLable(string strval)
		{
			if (strval.Length == 0)
			{
				return;
			}
			this.HightText.Text = strval;
		}
		public void SetTradeNameLable(string strval)
		{
			if (strval.Length == 0)
			{
				return;
			}
			this.TradeName.Text = strval;
		}
		public void InitTradeCtrl()
		{
			try
			{
				this.NO1.Text = (this.NO2.Text = (this.NO3.Text = (this.NO4.Text = (this.NO5.Text = (this.NO6.Text = "0.0")))));
				this.m_dSell = 0.0;
				this.m_dBuy = 0.0;
				this.NO5.Text = "50.0";
				this.NO6.Text = "50.0";
				this.BuyText.Text = this.m_rm.GetString("PMESStr_BUY_TEXT");
				this.SellText.Text = this.m_rm.GetString("PMESStr_SELL_TEXT");
				this.HightText.Text = this.m_rm.GetString("PMESStr_HIGHT_TEXT");
				this.LowText.Text = this.m_rm.GetString("PMESStr_LOW_TEXT");
				this.LowandHightImg1.Image = (Image)this.m_rm.GetObject("Low.png");
				this.LowandHightImg2.Image = (Image)this.m_rm.GetObject("Hight.png");
				this.TsMenuItemOpenPrice_S.Text = this.m_rm.GetString("PMESStr_MENU_OPENPRICE_S");
				this.TsMenuItemClosePrice_S.Text = this.m_rm.GetString("PMESStr_MENU_CLOSEPRICE_S");
				this.TsMenuItemClosePrice_L.Text = this.m_rm.GetString("PMESStr_MENU_LIMITCREATWAREHOUSE");
				this.TSMenuItemCancel.Text = this.m_rm.GetString("PMESStr_MENU_CANCEL");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TradeCtrl_Load(object sender, EventArgs e)
		{
			try
			{
				base.Size = new Size(base.Width * 2, base.Height * 2);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetProductsName(string strName)
		{
			this.TradeName.Text = strName;
		}
		public void SetCurrentPriceSell(string strPrice)
		{
			try
			{
				double y = 0.0;
				double x = 0.0;
				bool flag = true;
				this.m_stDrawProperties.m_strCurrentPriceSe = strPrice;
				try
				{
					if (this.NO2.Text == this.NO4.Text)
					{
						x = Convert.ToDouble(this.NO1.Text);
					}
					else
					{
						string[] array = this.NO2.Text.Split(new char[]
						{
							'.'
						});
						x = Convert.ToDouble(string.Format("{0}.{1}", array[0], this.NO4.Text));
					}
				}
				catch (Exception)
				{
					flag = false;
				}
				if (strPrice.CompareTo("--") == 0)
				{
					flag = false;
				}
				DrawImageState drawImageState;
				if (flag)
				{
					try
					{
						y = Convert.ToDouble(strPrice);
					}
					catch (OverflowException)
					{
					}
					drawImageState = this.CheckData(x, y);
					if (drawImageState != DrawImageState.DrawIS1 && drawImageState == DrawImageState.DrawIS2)
					{
					}
				}
				else
				{
					drawImageState = DrawImageState.DrawIS3;
				}
				if (drawImageState == DrawImageState.DrawIS2)
				{
					this.SetSellText(strPrice, Color.Green);
					this.m_crsText = Color.Green;
					this.SetImage(drawImageState);
				}
				else if (drawImageState == DrawImageState.DrawIS1)
				{
					this.SetSellText(strPrice, Color.Red);
					this.m_crsText = Color.Red;
					this.SetImage(drawImageState);
				}
				else
				{
					this.SetSellText(strPrice, Color.Black);
					this.m_crsText = Color.Black;
					this.SetImage(drawImageState);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetCurrentPriceBuy(string strPrice)
		{
			try
			{
				this.SetBuyText(strPrice, this.m_crsText);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetHightPrice(string strPrice)
		{
			if (strPrice.Length != 0)
			{
				this.UpDataHighttext(strPrice);
			}
		}
		public void SetLowPrice(string strPrice)
		{
			if (strPrice.Length != 0)
			{
				this.UpDataLowtext(strPrice);
			}
		}
		private DrawImageState CheckData(double x, double y)
		{
			if (x == y)
			{
				return DrawImageState.DrawIS3;
			}
			if (x <= y)
			{
				return DrawImageState.DrawIS1;
			}
			return DrawImageState.DrawIS2;
		}
		private void SetBuyText(string str, Color cr)
		{
			try
			{
				int num = str.IndexOf('.');
				if (str.CompareTo("--") == 0)
				{
					this.NO1.ForeColor = cr;
					this.NO1.Text = "--";
					this.NO3.ForeColor = cr;
					this.NO3.Text = "--";
				}
				else if (num > 0)
				{
					string[] array = str.Split(new char[]
					{
						'.'
					});
					this.NO1.Text = array[0] + ".";
					this.NO3.ForeColor = cr;
					this.NO3.Text = array[1].PadRight(2, '0');
				}
				else
				{
					this.NO1.Text = string.Format("{0}.", str);
					this.NO3.ForeColor = cr;
					this.NO3.Text = "00";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetSellText(string str, Color cr)
		{
			try
			{
				int num = str.IndexOf('.');
				if (str.CompareTo("--") == 0)
				{
					this.NO2.ForeColor = cr;
					this.NO2.Text = "--";
					this.NO4.ForeColor = cr;
					this.NO4.Text = "--";
				}
				else if (num > 0)
				{
					string[] array = str.Split(new char[]
					{
						'.'
					});
					this.NO2.Text = array[0] + ".";
					this.NO4.ForeColor = cr;
					this.NO4.Text = array[1].PadRight(2, '0');
				}
				else
				{
					this.NO2.Text = string.Format("{0}.", str);
					this.NO4.ForeColor = cr;
					this.NO4.Text = "00";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetImage(DrawImageState iStyle)
		{
			try
			{
				if (iStyle == DrawImageState.DrawIS1)
				{
					this.LowandHightImg1.Image = (Image)this.m_rm.GetObject("Hight.png");
					this.LowandHightImg2.Image = (Image)this.m_rm.GetObject("Hight.png");
					this.m_imgState = (Image)this.m_rm.GetObject("Hight.png");
				}
				else if (iStyle == DrawImageState.DrawIS2)
				{
					this.LowandHightImg1.Image = (Image)this.m_rm.GetObject("Low.png");
					this.LowandHightImg2.Image = (Image)this.m_rm.GetObject("Low.png");
					this.m_imgState = (Image)this.m_rm.GetObject("Low.png");
				}
				else
				{
					this.LowandHightImg1.Image = (Image)this.m_rm.GetObject("spjt.png");
					this.LowandHightImg2.Image = (Image)this.m_rm.GetObject("spjt.png");
					this.m_imgState = (Image)this.m_rm.GetObject("spjt.png");
				}
				this.LowandHightImg1.Invalidate();
				this.LowandHightImg2.Invalidate();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpDataLowtext(string strMin)
		{
			try
			{
				if (strMin.CompareTo("--") == 0)
				{
					this.NO6.ForeColor = Color.Black;
					this.NO6.Text = "--";
				}
				else
				{
					this.NO6.ForeColor = Color.Green;
					this.NO6.Text = strMin;
					this.NO6.Update();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpDataHighttext(string strMax)
		{
			try
			{
				if (strMax.CompareTo("--") == 0)
				{
					this.NO5.ForeColor = Color.Black;
					this.NO5.Text = "--";
				}
				else
				{
					this.NO5.ForeColor = Color.Red;
					this.NO5.Text = strMax;
					this.NO5.Update();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TradeCtrl_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				Point pt = new Point(0, 0);
				pt = new Point(this.TradeNameX, this.TradeNameY);
				TextRenderer.DrawText(e.Graphics, this.TradeName.Text, this.TradeName.Font, pt, Color.White, Color.Transparent);
				pt = new Point(this.SellTextX, this.SellTextY);
				TextRenderer.DrawText(e.Graphics, this.SellText.Text, this.SellText.Font, pt, this.SellText.ForeColor, Color.Transparent);
				pt = new Point(this.NO2X, this.NO2Y);
				TextRenderer.DrawText(e.Graphics, this.NO2.Text, this.NO2.Font, pt, this.NO2.ForeColor, Color.Transparent);
				Rectangle rect = new Rectangle(this.LowandHightImg2X, this.LowandHightImg2Y, this.LowandHightImg2.Image.Width, this.LowandHightImg2.Image.Height);
				e.Graphics.DrawImage(this.LowandHightImg2.Image, rect);
				pt = new Point(this.NO4X, this.NO4Y);
				TextRenderer.DrawText(e.Graphics, this.NO4.Text, this.NO4.Font, pt, this.NO4.ForeColor, Color.Transparent);
				pt = new Point(this.HightTextX, this.HightTextY);
				TextRenderer.DrawText(e.Graphics, this.HightText.Text, this.HightText.Font, pt, this.HightText.ForeColor, Color.Transparent);
				pt = new Point(this.NO5X, this.NO5Y);
				TextRenderer.DrawText(e.Graphics, this.NO5.Text, this.NO5.Font, pt, this.NO5.ForeColor, Color.Transparent);
				pt = new Point(this.BuyTextX, this.BuyTextY);
				TextRenderer.DrawText(e.Graphics, this.BuyText.Text, this.BuyText.Font, pt, this.BuyText.ForeColor, Color.Transparent);
				pt = new Point(this.NO1X, this.NO1Y);
				TextRenderer.DrawText(e.Graphics, this.NO1.Text, this.NO1.Font, pt, this.NO1.ForeColor, Color.Transparent);
				rect = new Rectangle(this.LowandHightImg1X, this.LowandHightImg1Y, this.LowandHightImg1.Image.Width, this.LowandHightImg1.Image.Height);
				e.Graphics.DrawImage(this.LowandHightImg1.Image, rect);
				pt = new Point(this.NO3X, this.NO3Y);
				TextRenderer.DrawText(e.Graphics, this.NO3.Text, this.NO3.Font, pt, this.NO3.ForeColor, Color.Transparent);
				pt = new Point(this.LowTextX, this.LowTextY);
				TextRenderer.DrawText(e.Graphics, this.LowText.Text, this.LowText.Font, pt, this.LowText.ForeColor, Color.Transparent);
				pt = new Point(this.NO6X, this.NO6Y);
				TextRenderer.DrawText(e.Graphics, this.NO6.Text, this.NO6.Font, pt, this.NO6.ForeColor, Color.Transparent);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TsMenuItemOpenPrice_S_Click(object sender, EventArgs e)
		{
			try
			{
				this.trademenu = TradeMenu.em_OpenPrice_S;
				this.TradeRefreashed(this.trademenu, this.CommodityID, this.buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TsMenuItemClosePrice_s_Click(object sender, EventArgs e)
		{
			try
			{
				this.trademenu = TradeMenu.em_ClosePrice_S;
				this.TradeRefreashed(this.trademenu, this.CommodityID, this.buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TsMenuItemClosePrice_L_Click(object sender, EventArgs e)
		{
			try
			{
				this.trademenu = TradeMenu.em_OpenPrice_L;
				this.TradeRefreashed(this.trademenu, this.CommodityID, this.buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void contextMenuStripHQCtrlEnable(bool Status)
		{
			try
			{
				this.TsMenuItemOpenPrice_S.Enabled = Status;
				this.TsMenuItemClosePrice_S.Enabled = Status;
				this.TsMenuItemClosePrice_L.Enabled = Status;
				this.TSMenuItemCancel.Enabled = Status;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void contextMenuStripHQCtrlEnable()
		{
			try
			{
				this.TsMenuItemClosePrice_L.Visible = false;
				this.toolStripSeparator1.Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TradeCtrl_Resize(object sender, EventArgs e)
		{
			try
			{
				this.TradeNameX = (int)(0.38235294818878174 * (double)base.Width);
				this.TradeNameY = (int)(0.042016807943582535 * (double)base.Height);
				this.SellTextX = (int)(0.0941176488995552 * (double)base.Width);
				this.SellTextY = (int)(0.32773110270500183 * (double)base.Height);
				this.NO2X = (int)(0.20588235557079315 * (double)base.Width);
				this.NO2Y = (int)(0.32773110270500183 * (double)base.Height);
				this.LowandHightImg2X = (int)(0.0941176488995552 * (double)base.Width);
				this.LowandHightImg2Y = (int)(0.5378151535987854 * (double)base.Height) - 2;
				this.NO4X = (int)(0.20588235557079315 * (double)base.Width);
				this.NO4Y = (int)(0.5378151535987854 * (double)base.Height);
				this.HightTextX = (int)(0.04117647185921669 * (double)base.Width);
				this.HightTextY = (int)(0.79831933975219727 * (double)base.Height);
				this.NO5X = (int)(0.20000000298023224 * (double)base.Width);
				this.NO5Y = (int)(0.79831933975219727 * (double)base.Height);
				this.BuyTextX = (int)(0.56470588235294117 * (double)base.Width);
				this.BuyTextY = (int)(0.32773110270500183 * (double)base.Height);
				this.NO1X = (int)(0.69411766529083252 * (double)base.Width);
				this.NO1Y = (int)(0.32773110270500183 * (double)base.Height);
				this.LowandHightImg1X = (int)(0.56470588235294117 * (double)base.Width);
				this.LowandHightImg1Y = (int)(0.5378151535987854 * (double)base.Height);
				this.NO3X = (int)(0.69411766529083252 * (double)base.Width);
				this.NO3Y = (int)(0.5378151535987854 * (double)base.Height);
				this.LowTextX = (int)(0.51176470518112183 * (double)base.Width);
				this.LowTextY = (int)(0.79831933975219727 * (double)base.Height);
				this.NO6X = (int)(0.67058825492858887 * (double)base.Width);
				this.NO6Y = (int)(0.79831933975219727 * (double)base.Height);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void contextMenuStripHQCtrl_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				if (this._ShowMenu != null)
				{
					this._ShowMenu("TradeCtrl");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TradeCtrl_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					this.buyRect.X = base.ClientRectangle.X;
					this.buyRect.Y = base.ClientRectangle.Y;
					this.buyRect.Height = base.ClientRectangle.Height;
					this.buyRect.Width = base.ClientRectangle.Width >> 1;
					Point pt = new Point(e.X, e.Y);
					this.buySell = this.buyRect.Contains(pt);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TradeCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					if (this._ShowMenu("TradeCtrl"))
					{
						this.buyRect.X = base.ClientRectangle.X;
						this.buyRect.Y = base.ClientRectangle.Y;
						this.buyRect.Height = base.ClientRectangle.Height;
						this.buyRect.Width = base.ClientRectangle.Width >> 1;
						this.buySell = this.buyRect.Contains(e.Location);
						this.TsMenuItemOpenPrice_S_Click(this, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
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
			this.TradeName = new Label();
			this.NO1 = new Label();
			this.LowandHightImg1 = new Label();
			this.NO2 = new Label();
			this.NO4 = new Label();
			this.NO5 = new Label();
			this.NO6 = new Label();
			this.NO3 = new Label();
			this.LowandHightImg2 = new Label();
			this.BuyText = new Label();
			this.SellText = new Label();
			this.HightText = new Label();
			this.LowText = new Label();
			this.contextMenuStripHQCtrl = new ContextMenuStrip(this.components);
			this.TsMenuItemOpenPrice_S = new ToolStripMenuItem();
			this.TsMenuItemClosePrice_S = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.TsMenuItemClosePrice_L = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.TSMenuItemCancel = new ToolStripMenuItem();
			this.contextMenuStripHQCtrl.SuspendLayout();
			base.SuspendLayout();
			this.TradeName.BackColor = Color.Transparent;
			this.TradeName.ForeColor = Color.Transparent;
			this.TradeName.Location = new Point(0, 0);
			this.TradeName.Name = "TradeName";
			this.TradeName.Size = new Size(0, 0);
			this.TradeName.TabIndex = 0;
			this.TradeName.Text = "label1";
			this.NO1.BackColor = Color.Transparent;
			this.NO1.Location = new Point(0, 0);
			this.NO1.Name = "NO1";
			this.NO1.Size = new Size(0, 0);
			this.NO1.TabIndex = 1;
			this.LowandHightImg1.BackColor = Color.Transparent;
			this.LowandHightImg1.Font = new Font("Arial Narrow", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LowandHightImg1.Location = new Point(0, 0);
			this.LowandHightImg1.Name = "LowandHightImg1";
			this.LowandHightImg1.Size = new Size(0, 0);
			this.LowandHightImg1.TabIndex = 2;
			this.NO2.BackColor = Color.Transparent;
			this.NO2.Location = new Point(0, 0);
			this.NO2.Name = "NO2";
			this.NO2.Size = new Size(0, 0);
			this.NO2.TabIndex = 3;
			this.NO4.BackColor = Color.Transparent;
			this.NO4.Location = new Point(0, 0);
			this.NO4.Name = "NO4";
			this.NO4.Size = new Size(0, 0);
			this.NO4.TabIndex = 4;
			this.NO5.BackColor = Color.Transparent;
			this.NO5.ForeColor = Color.Red;
			this.NO5.Location = new Point(0, 0);
			this.NO5.Name = "NO5";
			this.NO5.Size = new Size(0, 0);
			this.NO5.TabIndex = 5;
			this.NO6.BackColor = Color.Transparent;
			this.NO6.ForeColor = Color.FromArgb(0, 192, 0);
			this.NO6.Location = new Point(0, 0);
			this.NO6.Name = "NO6";
			this.NO6.Size = new Size(0, 0);
			this.NO6.TabIndex = 6;
			this.NO3.BackColor = Color.Transparent;
			this.NO3.Location = new Point(0, 0);
			this.NO3.Name = "NO3";
			this.NO3.Size = new Size(0, 0);
			this.NO3.TabIndex = 7;
			this.LowandHightImg2.BackColor = Color.Transparent;
			this.LowandHightImg2.Font = new Font("Arial Narrow", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LowandHightImg2.Location = new Point(0, 0);
			this.LowandHightImg2.Name = "LowandHightImg2";
			this.LowandHightImg2.Size = new Size(0, 0);
			this.LowandHightImg2.TabIndex = 8;
			this.BuyText.BackColor = Color.Transparent;
			this.BuyText.ForeColor = Color.Blue;
			this.BuyText.Location = new Point(0, 0);
			this.BuyText.Name = "BuyText";
			this.BuyText.Size = new Size(0, 0);
			this.BuyText.TabIndex = 9;
			this.SellText.BackColor = Color.Transparent;
			this.SellText.ForeColor = Color.Blue;
			this.SellText.Location = new Point(0, 0);
			this.SellText.Name = "SellText";
			this.SellText.Size = new Size(0, 0);
			this.SellText.TabIndex = 10;
			this.HightText.BackColor = Color.Transparent;
			this.HightText.Location = new Point(0, 0);
			this.HightText.Name = "HightText";
			this.HightText.Size = new Size(0, 0);
			this.HightText.TabIndex = 11;
			this.LowText.BackColor = Color.Transparent;
			this.LowText.Location = new Point(0, 0);
			this.LowText.Name = "LowText";
			this.LowText.Size = new Size(0, 0);
			this.LowText.TabIndex = 12;
			this.contextMenuStripHQCtrl.BackgroundImageLayout = ImageLayout.Center;
			this.contextMenuStripHQCtrl.Items.AddRange(new ToolStripItem[]
			{
				this.TsMenuItemOpenPrice_S,
				this.TsMenuItemClosePrice_S,
				this.toolStripSeparator1,
				this.TsMenuItemClosePrice_L,
				this.toolStripSeparator2,
				this.TSMenuItemCancel
			});
			this.contextMenuStripHQCtrl.Name = "contextMenuStrip1";
			this.contextMenuStripHQCtrl.Size = new Size(77, 104);
			this.contextMenuStripHQCtrl.Opening += new CancelEventHandler(this.contextMenuStripHQCtrl_Opening);
			this.TsMenuItemOpenPrice_S.Name = "TsMenuItemOpenPrice_S";
			this.TsMenuItemOpenPrice_S.Size = new Size(76, 22);
			this.TsMenuItemOpenPrice_S.Text = "1";
			this.TsMenuItemOpenPrice_S.Click += new EventHandler(this.TsMenuItemOpenPrice_S_Click);
			this.TsMenuItemClosePrice_S.Name = "TsMenuItemClosePrice_S";
			this.TsMenuItemClosePrice_S.Size = new Size(76, 22);
			this.TsMenuItemClosePrice_S.Text = "2";
			this.TsMenuItemClosePrice_S.Click += new EventHandler(this.TsMenuItemClosePrice_s_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(73, 6);
			this.TsMenuItemClosePrice_L.Name = "TsMenuItemClosePrice_L";
			this.TsMenuItemClosePrice_L.Size = new Size(76, 22);
			this.TsMenuItemClosePrice_L.Text = "3";
			this.TsMenuItemClosePrice_L.Click += new EventHandler(this.TsMenuItemClosePrice_L_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(73, 6);
			this.TSMenuItemCancel.Name = "TSMenuItemCancel";
			this.TSMenuItemCancel.Size = new Size(76, 22);
			this.TSMenuItemCancel.Text = "4";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Control;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			this.ContextMenuStrip = this.contextMenuStripHQCtrl;
			base.Controls.Add(this.LowText);
			base.Controls.Add(this.HightText);
			base.Controls.Add(this.SellText);
			base.Controls.Add(this.BuyText);
			base.Controls.Add(this.NO3);
			base.Controls.Add(this.NO6);
			base.Controls.Add(this.LowandHightImg2);
			base.Controls.Add(this.NO5);
			base.Controls.Add(this.TradeName);
			base.Controls.Add(this.NO4);
			base.Controls.Add(this.NO2);
			base.Controls.Add(this.NO1);
			base.Controls.Add(this.LowandHightImg1);
			base.Name = "TradeCtrl";
			base.Size = new Size(197, 139);
			base.Load += new EventHandler(this.TradeCtrl_Load);
			base.Paint += new PaintEventHandler(this.TradeCtrl_Paint);
			base.MouseDoubleClick += new MouseEventHandler(this.TradeCtrl_MouseDoubleClick);
			base.MouseDown += new MouseEventHandler(this.TradeCtrl_MouseDown);
			base.Resize += new EventHandler(this.TradeCtrl_Resize);
			this.contextMenuStripHQCtrl.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
