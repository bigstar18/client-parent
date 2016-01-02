using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.Properties;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class HQClientForm : Form, HQForm
	{
		private IContainer components;
		private System.Windows.Forms.Timer timerExChange;
		public MainWindow mainWindow;
		public PluginInfo pluginInfo;
		public SetInfo setInfo;
		public ButtonUtils buttonUtils;
		public MultiQuoteData multiQuoteData = new MultiQuoteData();
		private Rectangle m_rcMain;
		public Rectangle m_rcBottom;
		private int bottomHeight = 20;
		public int iStyle = Settings.Default.iStyle;
		private Page_Main mainGraph;
		private bool isMultiCycle;
		private bool isMultiMarket;
		private Page_Bottom bottomGraph;
		private HQClientMain curHQClient;
		private bool addMarketName;
		public string marketInfoAddress;
		private bool m_bEndPaint;
		private Bitmap m_bmp;
		private Bitmap m_bmpBottom;
		private bool m_bEndBottomPaint;
		private Point scrollOffset = default(Point);
		public bool isUp;
		public bool isMouseWheel;
		private MouseEventArgs MouseWheelArg;
		private bool bNeedRepaint;
		public bool m_isMouseLeftButtonDown;
		public Point m_mouseBeforeMove = new Point(0, 0);
		public event EventHandler hqFormLoad;
		public event InterFace.CommodityInfoEventHander MultiQuoteMouseEvent;
		public event MouseEventHandler MainForm_MouseClick;
		public event KeyEventHandler MainForm_KeyDown;
		public event KeyPressEventHandler MainForm_KeyPress;
		public event MouseEventHandler MainForm_MouseMove;
		public event MouseEventHandler MainForm_MouseDoubleClick;
		public Page_Main MainGraph
		{
			get
			{
				return this.mainGraph;
			}
			set
			{
				this.mainGraph = value;
			}
		}
		public bool IsMultiCycle
		{
			get
			{
				return this.isMultiCycle;
			}
			set
			{
				this.isMultiCycle = value;
			}
		}
		public bool IsMultiCommidity
		{
			get
			{
				return this.isMultiMarket;
			}
			set
			{
				this.isMultiMarket = value;
			}
		}
		public Cursor M_Cursor
		{
			get
			{
				return this.Cursor;
			}
			set
			{
				this.Cursor = value;
			}
		}
		public Graphics M_Graphics
		{
			get
			{
				Graphics result;
				try
				{
					result = base.CreateGraphics();
				}
				catch (ObjectDisposedException ex)
				{
					Console.WriteLine("Caught: {0}", ex.Message);
					result = null;
				}
				return result;
			}
		}
		public HQClientMain CurHQClient
		{
			get
			{
				return this.curHQClient;
			}
			set
			{
				this.curHQClient = value;
			}
		}
		public bool AddMarketName
		{
			get
			{
				return this.addMarketName;
			}
			set
			{
				this.addMarketName = value;
			}
		}
		public bool IsEndPaint
		{
			get
			{
				return this.m_bEndPaint;
			}
			set
			{
				this.m_bEndPaint = value;
			}
		}
		public Point ScrollOffset
		{
			get
			{
				return this.scrollOffset;
			}
			set
			{
				this.scrollOffset = value;
			}
		}
		public bool IsNeedRepaint
		{
			get
			{
				return this.bNeedRepaint;
			}
			set
			{
				this.bNeedRepaint = value;
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
			this.timerExChange = new System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			this.timerExChange.Interval = 500;
			this.timerExChange.Tick += new EventHandler(this.timerExChange_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScroll = true;
			base.ClientSize = new Size(522, 382);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "HQClientForm";
			base.FormClosing += new FormClosingEventHandler(this.HQMainForm_FormClosing);
			base.FormClosed += new FormClosedEventHandler(this.HQMainForm_FormClosed);
			base.Load += new EventHandler(this.HQMainForm_Load);
			base.Scroll += new ScrollEventHandler(this.HQMainForm_Scroll);
			base.ClientSizeChanged += new EventHandler(this.HQMainForm_ClientSizeChanged);
			base.Paint += new PaintEventHandler(this.HQMainForm_Paint);
			base.KeyDown += new KeyEventHandler(this.HQMainForm_KeyDown);
			base.KeyPress += new KeyPressEventHandler(this.HQMainForm_KeyPress);
			base.MouseClick += new MouseEventHandler(this.HQMainForm_MouseClick);
			base.MouseDoubleClick += new MouseEventHandler(this.HQMainForm_MouseDoubleClick);
			base.MouseMove += new MouseEventHandler(this.HQMainForm_MouseMove);
			base.ResumeLayout(false);
            
		}
		public HQClientForm(MainWindow mainWin)
		{
           
			this.mainWindow = mainWin;
			this.InitializeComponent();
			base.MouseUp += new MouseEventHandler(this.HQClientForm_MouseUp);
			base.MouseDown += new MouseEventHandler(this.HQClientForm_MouseDown);
			base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.HQClientForm_PreviewKeyDown);
			base.LostFocus += new EventHandler(this.HQClientForm_LostFocus);
			this.pluginInfo = mainWin.pluginInfo;
			this.setInfo = mainWin.setInfo;
			this.buttonUtils = new ButtonUtils(this.pluginInfo, this.setInfo);
			this.curHQClient = new HQClientMain(this);
		}
		private void HQClientForm_LostFocus(object sender, EventArgs e)
		{
			if (Form.ActiveForm == this.mainWindow && this != null)
			{
				base.Focus();
			}
		}
		private void HQClientForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right)
			{
				this.KDown(new KeyEventArgs(e.KeyData));
			}
		}
		private void HQClientForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.HQMainForm_MouseDown(e.X, e.Y);
			}
		}
		private void HQClientForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.m_isMouseLeftButtonDown = false;
			}
		}
		public void MultiQuoteMouseLeftClick(object sender, InterFace.CommodityInfoEventArgs e)
		{
			if (this.MultiQuoteMouseEvent != null)
			{
				this.MultiQuoteMouseEvent(sender, e);
			}
		}
		public void KDown(KeyEventArgs e)
		{
			if (this.mainGraph != null)
			{
				this.HQMainForm_KeyDown(null, e);
			}
		}
		private void HQMainForm_Load(object sender, EventArgs e)
		{
            
			if (this.pluginInfo.HQResourceManager == null)
			{
				throw new Exception("没有初始化行情系统资源");
			}
			this.setInfo.ISDebug = Tools.StrToBool((string)this.pluginInfo.HTConfig["bDebug"], false);
			this.addMarketName = Tools.StrToBool((string)this.pluginInfo.HTConfig["addMarketName"], false);
			this.marketInfoAddress = (string)this.pluginInfo.HTConfig["MarketInfo"];
			this.curHQClient.init();
			this.curHQClient.m_commodityInfoAddress = (string)this.pluginInfo.HTConfig["CommodityInfoAddress"];
			this.SetGraphSize();
			this.BackColor = SetInfo.RHColor.clBackGround;
			try
			{
				this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
				this.bottomGraph = new Page_Bottom(this.m_rcBottom, this);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.ToString());
			}
			if (this.hqFormLoad != null)
			{
				this.hqFormLoad(this, e);
			}
			if (this.marketInfoAddress != null && this.marketInfoAddress.Length > 0)
			{
				MarketInfo marketInfo = new MarketInfo(new Uri(this.marketInfoAddress));
				marketInfo.Show();
			}
		}
		protected override void OnPaintBackground(PaintEventArgs paintg)
		{
		}
		private void PaintHQ(Graphics myG, int value)
		{
			Monitor.Enter(this);
			try
			{
				if (!this.DoubleBuffered)
				{
					base.SetStyle(ControlStyles.UserPaint, true);
					base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
					base.SetStyle(ControlStyles.DoubleBuffer, true);
					base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				}
				if (this.m_rcMain.Width != 0 && this.m_rcMain.Height != 0)
				{
					if (this.mainGraph != null && myG != null)
					{
						myG.Clear(SetInfo.RHColor.clBackGround);
						this.mainGraph.Paint(myG, value);
					}
					if (this.bottomGraph != null)
					{
						this.bottomGraph.Paint(myG);
					}
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}
		private void PaintHQ(int value)
		{
			Monitor.Enter(this);
			try
			{
				if (this.m_rcMain.Width <= 0 || this.m_rcMain.Height <= 0)
				{
					return;
				}
				this.m_bEndPaint = false;
				using (this.m_bmp = new Bitmap(this.m_rcMain.Width, this.m_rcMain.Height))
				{
					using (Graphics graphics = Graphics.FromImage(this.m_bmp))
					{
						if (this.mainGraph != null)
						{
							if (graphics != null)
							{
								graphics.Clear(SetInfo.RHColor.clBackGround);
								this.mainGraph.Paint(graphics, value);
							}
						}
						else
						{
							if (graphics != null)
							{
								graphics.Clear(SetInfo.RHColor.clBackGround);
							}
						}
						this.EndPaint();
						if (!MainWindow.isLoadItemButtom)
						{
							this.mainWindow.SetControl(false);
						}
						else
						{
							this.mainWindow.clearMenuItem();
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
			this.mainWindow.changeBtColor();
			this.mainWindow.ChangeKLineBtnColor();
		}
		public void ClearRect(Graphics g, float x, float y, float width, float height)
		{
			if (g != null)
			{
				SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clBackGround);
				g.FillRectangle(solidBrush, x, y, width, height);
				solidBrush.Dispose();
			}
		}
		public void EndPaint()
		{
			if (!this.m_bEndPaint && this.m_bmp != null)
			{
				Graphics graphics = base.CreateGraphics();
				this.TranslateTransform(graphics);
				graphics.DrawImage(this.m_bmp, 0, 0);
			}
			this.m_bEndPaint = true;
		}
		public void Repaint()
		{
			this.bNeedRepaint = false;
			this.PaintHQ(this.curHQClient.KLineValue);
		}
		public void RepaintMin()
		{
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			this.bottomGraph = new Page_Bottom(this.m_rcBottom, this);
		}
		public void ReQueryCurClient()
		{
			switch (this.curHQClient.CurrentPage)
			{
			case 0:
				this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
				break;
			case 1:
				this.mainGraph = new Page_MinLine(this.m_rcMain, this);
				return;
			case 2:
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				return;
			case 3:
				break;
			case 4:
				this.mainGraph = new Page_Bill(this.m_rcMain, this);
				return;
			case 5:
				this.mainGraph = new Page_MarketStatus(this.m_rcMain, this);
				return;
			default:
				return;
			}
		}
		public void RepaintBottom()
		{
			Monitor.Enter(this);
			try
			{
				if (this.m_rcBottom.Width != 0)
				{
					this.m_bEndBottomPaint = false;
					using (this.m_bmpBottom = new Bitmap(this.m_rcBottom.Width, this.m_rcBottom.Height))
					{
						using (Graphics graphics = Graphics.FromImage(this.m_bmpBottom))
						{
							if (this.bottomGraph != null)
							{
								if (graphics != null)
								{
									graphics.Clear(SetInfo.RHColor.clBackGround);
									this.bottomGraph.Paint(graphics);
								}
							}
							else
							{
								if (graphics != null)
								{
									graphics.Clear(SetInfo.RHColor.clBackGround);
								}
							}
							this.EndBottomPaint();
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}
		public void EndBottomPaint()
		{
			if (!this.m_bEndBottomPaint && this.m_bmpBottom != null)
			{
				Graphics graphics = base.CreateGraphics();
				this.TranslateTransform(graphics);
				graphics.DrawImage(this.m_bmpBottom, this.m_rcBottom.X, this.m_rcBottom.Y);
			}
			this.m_bEndBottomPaint = true;
		}
		public Graphics TranslateTransform(Graphics g)
		{
			Point autoScrollPosition = base.AutoScrollPosition;
			g.TranslateTransform((float)autoScrollPosition.X, (float)autoScrollPosition.Y);
			return g;
		}
		public void SavePic(string path)
		{
			this.m_bmp.Save(path);
		}
		public bool isDisplayF10Menu()
		{
			return this.curHQClient.m_commodityInfoAddress != null && this.curHQClient.m_commodityInfoAddress.Length > 0;
		}
		public void DisplayCommodityInfo(string sCode)
		{
			if (this.curHQClient.m_commodityInfoAddress != null && this.curHQClient.m_commodityInfoAddress.Length > 0)
			{
				CommodityInfoF instance = CommodityInfoF.GetInstance(this);
				instance.Url = this.curHQClient.m_commodityInfoAddress;
				instance.CommodityID = sCode;
				instance.setUri();
				instance.Dock = DockStyle.Fill;
				instance.FormBorderStyle = FormBorderStyle.None;
				instance.BringToFront();
				instance.TopLevel = false;
				base.Controls.Add(instance);
				instance.Show();
			}
		}
		public void QueryStock(CommodityInfo commodityInfo)
		{
			this.curHQClient.curCommodityInfo = commodityInfo;
			if (1 == this.curHQClient.CurrentPage)
			{
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 2;
				return;
			}
			if (2 == this.curHQClient.CurrentPage)
			{
				this.mainGraph = new Page_MinLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 1;
				return;
			}
			if (this.curHQClient.globalData.PrePage == 2)
			{
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 2;
				return;
			}
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			this.curHQClient.globalData.PrePage = 1;
		}
		public void ShowPageKLine(CommodityInfo commodityInfo)
		{
			this.curHQClient.curCommodityInfo = commodityInfo;
			this.mainGraph = new Page_KLine(this.m_rcMain, this);
			this.Repaint();
		}
		public void ShowPageMinLine(CommodityInfo commodityInfo)
		{
			this.curHQClient.curCommodityInfo = commodityInfo;
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			this.Repaint();
		}
		public void ShowPageMinLine()
		{
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			this.Repaint();
		}
		public void ShowPageKLine()
		{
			this.mainGraph = new Page_KLine(this.m_rcMain, this);
			this.Repaint();
		}
		public void ChangeStock(bool bUp)
		{
			if (this.multiQuoteData.MultiQuotePage == 1)
			{
				int num = -1;
				CommodityInfo commodityInfo = this.curHQClient.curCommodityInfo;
				for (int i = 0; i < this.multiQuoteData.MyCommodityList.Count; i++)
				{
					if (commodityInfo.Compare(CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[i].ToString())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					if (this.multiQuoteData.MyCommodityList.Count > 0)
					{
						commodityInfo = CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[0].ToString());
					}
				}
				else
				{
					if (bUp)
					{
						if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
						{
							num--;
							Logger.wirte(MsgType.Information, "Index -- 1 = " + num.ToString());
							if (num < 0)
							{
								num = this.multiQuoteData.MyCommodityList.Count - 1;
							}
						}
					}
					else
					{
						if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
						{
							num++;
							Logger.wirte(MsgType.Information, "Index ++ 1 = " + num.ToString());
							if (num >= this.multiQuoteData.MyCommodityList.Count)
							{
								num = 0;
							}
						}
					}
					commodityInfo = CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[num].ToString());
				}
				if (1 == this.curHQClient.CurrentPage)
				{
					this.ShowPageMinLine(commodityInfo);
					return;
				}
				if (2 == this.curHQClient.CurrentPage)
				{
					this.ShowPageKLine(commodityInfo);
					return;
				}
			}
			else
			{
				if (this.curHQClient.curCommodityInfo == null)
				{
					return;
				}
				int num2 = -1;
				for (int j = 0; j < this.multiQuoteData.m_curQuoteList.Length; j++)
				{
					if (this.curHQClient.curCommodityInfo.Compare(new CommodityInfo(this.multiQuoteData.m_curQuoteList[j].marketID, this.multiQuoteData.m_curQuoteList[j].code)))
					{
						num2 = j;
						break;
					}
				}
				if (num2 == -1)
				{
					if (this.multiQuoteData.m_curQuoteList.Length > 0)
					{
						this.curHQClient.curCommodityInfo = new CommodityInfo(this.multiQuoteData.m_curQuoteList[0].marketID, this.multiQuoteData.m_curQuoteList[0].code);
					}
				}
				else
				{
					if (bUp)
					{
						if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
						{
							num2--;
							Logger.wirte(MsgType.Information, "Index -- 2 = " + num2.ToString());
							if (num2 < 0)
							{
								num2 = this.multiQuoteData.m_curQuoteList.Length - 1;
							}
						}
					}
					else
					{
						if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
						{
							num2++;
							Logger.wirte(MsgType.Information, "Index ++ 2 = " + num2.ToString());
							if (num2 >= this.multiQuoteData.m_curQuoteList.Length)
							{
								num2 = 0;
							}
						}
					}
					this.curHQClient.curCommodityInfo = new CommodityInfo(this.multiQuoteData.m_curQuoteList[num2].marketID, this.multiQuoteData.m_curQuoteList[num2].code);
				}
				if (1 == this.curHQClient.CurrentPage)
				{
					this.mainGraph = new Page_MinLine(this.m_rcMain, this);
					return;
				}
				if (2 == this.curHQClient.CurrentPage)
				{
					this.mainGraph = new Page_KLine(this.m_rcMain, this);
				}
			}
		}
		private void SetGraphSize()
		{
			int width = base.ClientSize.Width;
			int height = base.ClientSize.Height;
			if (width == 0 && height == 0)
			{
				return;
			}
			if (width < base.AutoScrollMinSize.Width)
			{
				width = base.AutoScrollMinSize.Width;
			}
			if (height < base.AutoScrollMinSize.Height)
			{
				height = base.AutoScrollMinSize.Height;
			}
			this.m_rcMain = new Rectangle(new Point(0, 0), new Size(width, height));
			this.m_rcBottom = new Rectangle(new Point(0, 0), new Size(width, height));
			this.m_rcMain.Height = this.m_rcMain.Height - this.bottomHeight;
			this.m_rcBottom.Y = this.m_rcMain.Y + this.m_rcMain.Height;
			this.m_rcBottom.Height = this.bottomHeight;
			this.scrollOffset = base.AutoScrollPosition;
		}
		private void HQMainForm_ClientSizeChanged(object sender, EventArgs e)
		{
			this.SetGraphSize();
			if (this.mainGraph != null)
			{
				this.mainGraph.m_rc = this.m_rcMain;
				if (this.bottomGraph != null)
				{
					this.bottomGraph.rc = this.m_rcBottom;
				}
				base.Invalidate();
			}
		}
		private void HQMainForm_Scroll(object sender, ScrollEventArgs e)
		{
			this.scrollOffset = base.AutoScrollPosition;
		}
		private void HQMainForm_Paint(object sender, PaintEventArgs e)
		{
			this.Repaint();
			this.RepaintBottom();
		}
		private void HQMainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.setInfo.saveSetInfo("iStyle", this.iStyle.ToString());
			if (this.curHQClient != null)
			{
				this.curHQClient.Dispose();
			}
			if (this.mainGraph != null)
			{
				this.mainGraph.PageDispose();
			}
			this.buttonUtils.ButtonList.Clear();
			this.buttonUtils.RightButtonList.Clear();
		}
		private void HQMainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			SelectServer.GetInstance().Close();
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.timerExChange.Enabled = false;
			this.MouseWheelArg = e;
			this.timerExChange.Enabled = true;
			this.curHQClient.isNeedAskData = false;
			this.MouseWheelMethod(e);
		}
		private void MouseWheelMethodByTimer(MouseEventArgs e)
		{
			this.MouseWheelMethod(e);
		}
		public void MouseWheelMethod(MouseEventArgs e)
		{
			this.isMouseWheel = true;
			KeyEventArgs e2;
			if (e.Delta > 0)
			{
				e2 = new KeyEventArgs(Keys.Prior);
				this.isUp = true;
			}
			else
			{
				e2 = new KeyEventArgs(Keys.Next);
				this.isUp = false;
			}
			this.MainForm_KeyDown(this, e2);
			if (this.bNeedRepaint)
			{
				this.Repaint();
			}
		}
		public void HQMainForm_MouseClick(object sender, MouseEventArgs e)
		{
			this.isMouseWheel = false;
			int arg_0D_0 = e.X;
			int y = e.Y;
			if (y < this.m_rcMain.Y + this.m_rcMain.Height)
			{
				if (this.MainForm_MouseClick != null && e.Button == MouseButtons.Left)
				{
					this.MainForm_MouseClick(this, e);
					return;
				}
				if (e.Button == MouseButtons.Right)
				{
					if (this.curHQClient.CurrentPage == 0 && y > this.m_rcMain.Y + this.m_rcMain.Height - this.multiQuoteData.buttonHight)
					{
						this.MainForm_MouseClick(this, e);
						return;
					}
					this.MainForm_MouseClick(this, e);
					if (this.mainGraph != null)
					{
						this.mainGraph.contextMenuStrip.Show(this, e.X, e.Y);
					}
				}
			}
		}
		public void HQMainForm_KeyDown(object sender, KeyEventArgs e)
		{
			this.isMouseWheel = false;
			Keys keyData = e.KeyData;
			if (keyData != Keys.Escape)
			{
				switch (keyData)
				{
				case Keys.F1:
					if (this.curHQClient.CurrentPage == 1 || this.curHQClient.CurrentPage == 2)
					{
						this.UserCommand("01");
						goto IL_198;
					}
					goto IL_198;
				case Keys.F2:
					this.UserCommand("60");
					goto IL_198;
				case Keys.F3:
					if (this.curHQClient.indexMainCode.Length > 0)
					{
						this.UserCommand("INDEX_" + this.curHQClient.indexMainCode);
						goto IL_198;
					}
					goto IL_198;
				case Keys.F4:
					this.UserCommand("80");
					goto IL_198;
				case Keys.F5:
					this.UserCommand("05");
					goto IL_198;
				case Keys.F7:
					this.UserCommand("70");
					goto IL_198;
				}
				if (this.mainGraph != null && this.MainForm_KeyDown != null)
				{
					this.MainForm_KeyDown(this, e);
				}
			}
			else
			{
				if (this.mainGraph != null)
				{
					if (this.mainGraph != null && (this.curHQClient.CurrentPage == 1 || this.curHQClient.CurrentPage == 2))
					{
						if (this.MainForm_KeyDown != null)
						{
							this.MainForm_KeyDown(this, e);
						}
						if (!this.bNeedRepaint && this.curHQClient.CurrentPage != 0)
						{
							this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
							this.bNeedRepaint = true;
						}
					}
					else
					{
						if (this.curHQClient.CurrentPage != 0)
						{
							this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
							this.bNeedRepaint = true;
						}
					}
				}
			}
			IL_198:
			if (this.bNeedRepaint)
			{
				this.Repaint();
			}
		}
		public void HQMainForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.isMouseWheel = false;
			char keyChar = e.KeyChar;
			if (char.IsLetterOrDigit(keyChar))
			{
				InputDialog inputDialog = new InputDialog(keyChar, this);
				Rectangle bounds = base.Bounds;
				int left = bounds.Right - inputDialog.Width - 5;
				int top = bounds.Bottom - inputDialog.Height - this.m_rcBottom.Height - 5;
				inputDialog.Left = left;
				inputDialog.Top = top;
				if (inputDialog.ShowDialog() == DialogResult.OK)
				{
					string strCmd = inputDialog.strCmd;
					if (strCmd == null || strCmd.Length == 0)
					{
						return;
					}
					char c = strCmd[0];
					switch (c)
					{
					case 'A':
						this.UserCommand(strCmd.Substring(1));
						break;
					case 'B':
						break;
					case 'C':
						this.UserCommand(strCmd.Substring(0));
						break;
					default:
						if (c != 'P')
						{
							if (c == 'T')
							{
								this.curHQClient.m_strIndicator = strCmd.Substring(1);
								((Page_KLine)this.mainGraph).draw_KLine.CreateIndicator();
							}
						}
						else
						{
							if (this.curHQClient.CurrentPage == 2)
							{
								this.ShowPageKLine(CommodityInfo.DealCode(strCmd.Substring(1)));
							}
							else
							{
								this.ShowPageMinLine(CommodityInfo.DealCode(strCmd.Substring(1)));
							}
						}
						break;
					}
					this.bNeedRepaint = true;
					return;
				}
			}
			else
			{
				if (this.MainForm_KeyPress != null)
				{
					this.MainForm_KeyPress(this, e);
				}
			}
		}
		public void HQMainForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.MainForm_MouseMove != null)
			{
				this.MainForm_MouseMove(this, e);
			}
		}
		public void HQMainForm_MouseDown(int x, int y)
		{
			this.m_isMouseLeftButtonDown = true;
			this.m_mouseBeforeMove = new Point(x, y);
		}
		public void HQMainForm_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.MainForm_MouseDoubleClick != null)
			{
				this.MainForm_MouseDoubleClick(this, e);
			}
		}
		public void UserCommand(string sCmd)
		{
			this.bNeedRepaint = true;
			if (sCmd == null || sCmd.Length == 0)
			{
				return;
			}
			if (sCmd.StartsWith("INDEX_"))
			{
				CommodityInfo curCommodityInfo = CommodityInfo.DealCode(sCmd.Substring(6));
				this.curHQClient.curCommodityInfo = curCommodityInfo;
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				return;
			}
			if (sCmd.StartsWith("SERIES_"))
			{
				CommodityInfo curCommodityInfo2 = CommodityInfo.DealCode(sCmd.Substring(7));
				this.curHQClient.curCommodityInfo = curCommodityInfo2;
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				return;
			}
			if (sCmd.Equals("page_history"))
			{
				this.UserCommand("70");
				return;
			}
			if (sCmd.Equals("refreshBt"))
			{
				this.curHQClient.CurrentPage = this.curHQClient.CurrentPage;
				this.curHQClient.isChangePage = 0;
				this.curHQClient.m_hqForm.M_Graphics.FillRectangle(Brushes.Black, this.curHQClient.m_hqForm.MainGraph.m_rc);
				this.curHQClient.m_hqForm.Repaint();
				return;
			}
			if (!sCmd.StartsWith("Color"))
			{
				if (sCmd.StartsWith("userSet"))
				{
					UserSet userSet = new UserSet(this);
					userSet.ShowDialog();
				}
				int num;
				try
				{
					num = int.Parse(sCmd);
				}
				catch
				{
					return;
				}
				Logger.wirte(MsgType.Information, "sCmd =" + num);
				int num2 = num;
				if (num2 <= 5)
				{
					if (num2 != 1)
					{
						if (num2 == 5)
						{
							this.OnF5();
						}
					}
					else
					{
						this.mainGraph = new Page_Bill(this.m_rcMain, this);
					}
				}
				else
				{
					if (num2 != 60)
					{
						if (num2 != 70)
						{
							if (num2 == 80)
							{
								this.mainGraph = new Page_MarketStatus(this.m_rcMain, this);
							}
						}
						else
						{
							this.mainGraph = new Page_History(this.m_rcMain, this);
						}
					}
					else
					{
						this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
					}
				}
				this.mainWindow.changeBtColor();
				return;
			}
			sCmd = sCmd.Substring(5);
			try
			{
				this.iStyle = int.Parse(sCmd);
			}
			catch
			{
				return;
			}
			SetInfo.RHColor = new RHColor(this.iStyle);
		}
		public void OnF5()
		{
			if (this.curHQClient.curCommodityInfo == null)
			{
				return;
			}
			if (1 == this.curHQClient.CurrentPage)
			{
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 2;
				return;
			}
			if (2 == this.curHQClient.CurrentPage)
			{
				this.mainGraph = new Page_MinLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 1;
				return;
			}
			if (this.curHQClient.globalData.PrePage == 2)
			{
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				this.curHQClient.globalData.PrePage = 2;
				return;
			}
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			this.curHQClient.globalData.PrePage = 1;
		}
		public void DisplayHQSunForm(CommodityInfo commodityInfo)
		{
			HQSubForm hQSubForm = new HQSubForm(commodityInfo, this.pluginInfo, this.setInfo);
			if (base.MdiParent != null)
			{
				hQSubForm.MdiParent = base.MdiParent;
			}
			hQSubForm.Show();
		}
		public void ReMakeIndexMenu()
		{
			if (this.mainGraph != null)
			{
				MethodInvoker method = new MethodInvoker(this.mainGraph.AddIndexMenu);
				base.BeginInvoke(method);
			}
		}
		private void timerExChange_Tick(object sender, EventArgs e)
		{
			this.curHQClient.isNeedAskData = true;
			this.timerExChange.Enabled = false;
			if (this.curHQClient.CurrentPage == 2 || this.curHQClient.CurrentPage == 1)
			{
				this.MouseWheelMethodByTimer(this.MouseWheelArg);
			}
		}
	}
}
