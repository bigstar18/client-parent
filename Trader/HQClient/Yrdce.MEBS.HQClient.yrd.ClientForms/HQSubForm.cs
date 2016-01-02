using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.Properties;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class HQSubForm : Form, HQForm
	{
		private IContainer components;
		public PluginInfo pluginInfo;
		public SetInfo setInfo;
		private bool isMultiCycle;
		private bool isMultiMarket;
		private Rectangle m_rcMain;
		private int iStyle = Settings.Default.iStyle;
		private Bitmap m_bmp;
		private CommodityInfo m_commodity;
		private HQClientMain curHQClient;
		private int _KLineValue;
		public Page_Main mainGraph;
		private bool bNeedRepaint;
		private bool m_bEndPaint;
		private Point scrollOffset = default(Point);
		private bool addMarketName;
		public event MouseEventHandler MainForm_MouseClick;
		public event KeyEventHandler MainForm_KeyDown;
		public event KeyPressEventHandler MainForm_KeyPress;
		public event MouseEventHandler MainForm_MouseMove;
		public event MouseEventHandler MainForm_MouseDoubleClick;
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
		public int KLineValue
		{
			get
			{
				return this._KLineValue;
			}
			set
			{
				this._KLineValue = value;
			}
		}
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
		public Graphics M_Graphics
		{
			get
			{
				return base.CreateGraphics();
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(561, 399);
			base.Name = "HQSubForm";
			this.Text = "HQSubForm";
			base.FormClosed += new FormClosedEventHandler(this.HQSubForm_FormClosed);
			base.MouseDoubleClick += new MouseEventHandler(this.HQMainForm_MouseDoubleClick);
			base.ClientSizeChanged += new EventHandler(this.HQSubForm_ClientSizeChanged);
			base.Paint += new PaintEventHandler(this.HQSubForm_Paint);
			base.MouseClick += new MouseEventHandler(this.HQMainForm_MouseClick);
			base.KeyPress += new KeyPressEventHandler(this.HQMainForm_KeyPress);
			base.FormClosing += new FormClosingEventHandler(this.HQSubForm_FormClosing);
			base.MouseMove += new MouseEventHandler(this.HQMainForm_MouseMove);
			base.KeyDown += new KeyEventHandler(this.HQMainForm_KeyDown);
			base.Scroll += new ScrollEventHandler(this.HQSubForm_Scroll);
			base.Load += new EventHandler(this.HQSubForm_Load);
			base.ResumeLayout(false);
		}
		public HQSubForm(CommodityInfo commodity, PluginInfo _pluginInfo, SetInfo _setInfo)
		{
			this.InitializeComponent();
			this.pluginInfo = _pluginInfo;
			this.setInfo = _setInfo;
			this.m_commodity = commodity;
		}
		protected override void OnPaintBackground(PaintEventArgs paintg)
		{
		}
		private void PaintHQ(Graphics myG, int value, int type)
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
					if (this.mainGraph != null)
					{
						this.TranslateTransform(myG);
						if (myG != null)
						{
							myG.Clear(SetInfo.RHColor.clBackGround);
							this.mainGraph.Paint(myG, value);
						}
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
				if (this.m_rcMain.Width != 0 && this.m_rcMain.Height != 0)
				{
					this.m_bEndPaint = false;
					using (this.m_bmp = new Bitmap(this.m_rcMain.Width, this.m_rcMain.Height))
					{
						using (Graphics graphics = Graphics.FromImage(this.m_bmp))
						{
							if (this.mainGraph != null)
							{
								this.TranslateTransform(graphics);
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
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
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
				base.CreateGraphics().DrawImage(this.m_bmp, 0, 0);
			}
			this.m_bEndPaint = true;
		}
		public void Repaint()
		{
			this.bNeedRepaint = false;
			this.PaintHQ(this.KLineValue);
		}
		public void RepaintMin()
		{
		}
		public void ReQueryCurClient()
		{
		}
		public void RepaintBottom()
		{
		}
		public void EndBottomPaint()
		{
		}
		public Graphics TranslateTransform(Graphics g)
		{
			Point autoScrollPosition = base.AutoScrollPosition;
			g.TranslateTransform((float)autoScrollPosition.X, (float)autoScrollPosition.Y);
			return g;
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
			if (this.curHQClient.multiQuoteData.MultiQuotePage == 1)
			{
				int num = -1;
				CommodityInfo commodityInfo = this.curHQClient.curCommodityInfo;
				for (int i = 0; i < this.curHQClient.multiQuoteData.MyCommodityList.Count; i++)
				{
					if (commodityInfo.Compare(CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[i].ToString())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					if (this.curHQClient.multiQuoteData.MyCommodityList.Count > 0)
					{
						commodityInfo = CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[0].ToString());
					}
				}
				else
				{
					if (bUp)
					{
						num--;
						if (num < 0)
						{
							num = this.curHQClient.multiQuoteData.MyCommodityList.Count - 1;
						}
					}
					else
					{
						num++;
						if (num >= this.curHQClient.multiQuoteData.MyCommodityList.Count)
						{
							num = 0;
						}
					}
					commodityInfo = CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[num].ToString());
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
				CodeTable codeTable = (CodeTable)this.curHQClient.m_htProduct[this.curHQClient.curCommodityInfo.marketID + this.curHQClient.curCommodityInfo.commodityCode];
				ArrayList arrayList = this.curHQClient.m_codeList;
				if (codeTable != null && codeTable.status == 1)
				{
					arrayList = this.curHQClient.hm_codeList;
				}
				else
				{
					if (codeTable != null)
					{
						arrayList = this.curHQClient.nm_codeList;
					}
				}
				int num2 = -1;
				for (int j = 0; j < arrayList.Count; j++)
				{
					if (this.curHQClient.curCommodityInfo.Compare(arrayList[j]))
					{
						num2 = j;
						break;
					}
				}
				if (num2 == -1)
				{
					if (arrayList.Count > 0)
					{
						this.curHQClient.curCommodityInfo = (CommodityInfo)arrayList[0];
					}
				}
				else
				{
					if (bUp)
					{
						num2--;
						if (num2 < 0)
						{
							num2 = arrayList.Count - 1;
						}
					}
					else
					{
						num2++;
						if (num2 >= arrayList.Count)
						{
							num2 = 0;
						}
					}
					this.curHQClient.curCommodityInfo = (CommodityInfo)arrayList[num2];
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
			if (sCmd.StartsWith("Color"))
			{
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
			else
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
					if (num2 == 1)
					{
						this.mainGraph = new Page_Bill(this.m_rcMain, this);
						return;
					}
					if (num2 != 5)
					{
						return;
					}
					this.OnF5();
					return;
				}
				else
				{
					if (num2 == 60)
					{
						this.mainGraph = new Page_MultiQuote(this.m_rcMain, this);
						return;
					}
					if (num2 == 70)
					{
						this.mainGraph = new Page_History(this.m_rcMain, this);
						return;
					}
					if (num2 != 80)
					{
						return;
					}
					this.mainGraph = new Page_MarketStatus(this.m_rcMain, this);
					return;
				}
			}
		}
		private void OnF5()
		{
			if (this.curHQClient.curCommodityInfo == null)
			{
				return;
			}
			if (1 == this.curHQClient.CurrentPage)
			{
				this.mainGraph = new Page_KLine(this.m_rcMain, this);
				return;
			}
			this.mainGraph = new Page_MinLine(this.m_rcMain, this);
		}
		public void ReMakeIndexMenu()
		{
			if (this.mainGraph != null)
			{
				MethodInvoker method = new MethodInvoker(this.mainGraph.AddIndexMenu);
				base.BeginInvoke(method);
			}
		}
		private void HQMainForm_MouseClick(object sender, MouseEventArgs e)
		{
			int arg_06_0 = e.X;
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
					if (this.curHQClient.CurrentPage == 0 && y > this.m_rcMain.Y + this.m_rcMain.Height - this.curHQClient.multiQuoteData.buttonHight)
					{
						this.MainForm_MouseClick(this, e);
						return;
					}
					if (this.mainGraph != null)
					{
						this.mainGraph.contextMenuStrip.Show(this, e.X, e.Y);
					}
				}
			}
		}
		public void MultiQuoteMouseLeftClick(object sender, InterFace.CommodityInfoEventArgs e)
		{
		}
		public void HQMainForm_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keyData = e.KeyData;
			if (keyData != Keys.Escape)
			{
				switch (keyData)
				{
				case Keys.F1:
					if (this.curHQClient.CurrentPage == 1 || this.curHQClient.CurrentPage == 2)
					{
						this.UserCommand("01");
						goto IL_191;
					}
					goto IL_191;
				case Keys.F2:
					this.UserCommand("60");
					goto IL_191;
				case Keys.F3:
					if (this.curHQClient.indexMainCode.Length > 0)
					{
						this.UserCommand("INDEX_" + this.curHQClient.indexMainCode);
						goto IL_191;
					}
					goto IL_191;
				case Keys.F4:
					this.UserCommand("80");
					goto IL_191;
				case Keys.F5:
					this.UserCommand("05");
					goto IL_191;
				case Keys.F7:
					this.UserCommand("70");
					goto IL_191;
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
			IL_191:
			if (this.bNeedRepaint)
			{
				this.Repaint();
			}
		}
		private void HQMainForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (char.IsLetterOrDigit(keyChar))
			{
				InputDialog inputDialog = new InputDialog(keyChar, this);
				Rectangle bounds = base.Bounds;
				int left = bounds.Right - inputDialog.Width - 5;
				int top = bounds.Bottom - inputDialog.Height - 5;
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
		private void HQMainForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.MainForm_MouseMove != null)
			{
				this.MainForm_MouseMove(this, e);
			}
		}
		private void HQMainForm_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.MainForm_MouseDoubleClick != null)
			{
				this.MainForm_MouseDoubleClick(this, e);
			}
		}
		private void HQSubForm_Scroll(object sender, ScrollEventArgs e)
		{
			this.scrollOffset = base.AutoScrollPosition;
		}
		private void HQSubForm_Paint(object sender, PaintEventArgs e)
		{
			this.Repaint();
			this.RepaintBottom();
		}
		private void HQSubForm_Load(object sender, EventArgs e)
		{
			if (this.pluginInfo.HQResourceManager == null)
			{
				throw new Exception("没有初始化行情系统资源");
			}
			if (this.m_commodity == null)
			{
				throw new Exception("没有初始化商品代码");
			}
			bool.TryParse((string)this.pluginInfo.HTConfig["addMarketName"], out this.addMarketName);
			this.SetGraphSize();
			HQClientMain hQClientMain = new HQClientMain(this);
			hQClientMain.init();
			this.curHQClient = hQClientMain;
			this.curHQClient.curCommodityInfo = this.m_commodity;
			try
			{
				this.mainGraph = new Page_MinLine(this.m_rcMain, this);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.ToString());
			}
			this.BackColor = SetInfo.RHColor.clBackGround;
		}
		private void HQSubForm_FormClosed(object sender, FormClosedEventArgs e)
		{
		}
		private void HQSubForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.iStyle = this.iStyle;
			Settings.Default.Save();
			if (this.curHQClient != null)
			{
				this.curHQClient.Dispose();
			}
			if (this.mainGraph != null)
			{
				this.mainGraph.PageDispose();
			}
		}
		private void HQSubForm_ClientSizeChanged(object sender, EventArgs e)
		{
			this.SetGraphSize();
			if (this.mainGraph != null)
			{
				this.mainGraph.m_rc = this.m_rcMain;
				base.Invalidate();
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
			this.scrollOffset = base.AutoScrollPosition;
		}
	}
}
