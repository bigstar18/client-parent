// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.HQClientForm
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.Properties;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
  public class HQClientForm : Form, HQForm
  {
    public MultiQuoteData multiQuoteData = new MultiQuoteData();
    private int bottomHeight = 20;
    public int iStyle = Settings.Default.iStyle;
    private Point scrollOffset = new Point();
    public Point m_mouseBeforeMove = new Point(0, 0);
    private IContainer components;
    private Timer timerExChange;
    public MainWindow mainWindow;
    public PluginInfo pluginInfo;
    public SetInfo setInfo;
    public ButtonUtils buttonUtils;
    private Rectangle m_rcMain;
    public Rectangle m_rcBottom;
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
    public bool isUp;
    public bool isMouseWheel;
    private MouseEventArgs MouseWheelArg;
    private bool bNeedRepaint;
    public bool m_isMouseLeftButtonDown;

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
        try
        {
          return this.CreateGraphics();
        }
        catch (ObjectDisposedException ex)
        {
          Console.WriteLine("Caught: {0}", (object) ex.Message);
          return (Graphics) null;
        }
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

    public event EventHandler hqFormLoad;

    public event InterFace.CommodityInfoEventHander MultiQuoteMouseEvent;

    public event MouseEventHandler MainForm_MouseClick;

    public event KeyEventHandler MainForm_KeyDown;

    public event KeyPressEventHandler MainForm_KeyPress;

    public event MouseEventHandler MainForm_MouseMove;

    public event MouseEventHandler MainForm_MouseDoubleClick;

    public HQClientForm(MainWindow mainWin)
    {
      this.mainWindow = mainWin;
      this.InitializeComponent();
      this.MouseUp += new MouseEventHandler(this.HQClientForm_MouseUp);
      this.MouseDown += new MouseEventHandler(this.HQClientForm_MouseDown);
      this.PreviewKeyDown += new PreviewKeyDownEventHandler(this.HQClientForm_PreviewKeyDown);
      this.LostFocus += new EventHandler(this.HQClientForm_LostFocus);
      this.pluginInfo = mainWin.pluginInfo;
      this.setInfo = mainWin.setInfo;
      this.buttonUtils = new ButtonUtils(this.pluginInfo, this.setInfo);
      this.curHQClient = new HQClientMain((HQForm) this);
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
      this.timerExChange = new Timer(this.components);
      this.SuspendLayout();
      this.timerExChange.Interval = 500;
      this.timerExChange.Tick += new EventHandler(this.timerExChange_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(522, 382);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = "HQClientForm";
      this.FormClosing += new FormClosingEventHandler(this.HQMainForm_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.HQMainForm_FormClosed);
      this.Load += new EventHandler(this.HQMainForm_Load);
      this.Scroll += new ScrollEventHandler(this.HQMainForm_Scroll);
      this.ClientSizeChanged += new EventHandler(this.HQMainForm_ClientSizeChanged);
      this.Paint += new PaintEventHandler(this.HQMainForm_Paint);
      this.KeyDown += new KeyEventHandler(this.HQMainForm_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.HQMainForm_KeyPress);
      this.MouseClick += new MouseEventHandler(this.HQMainForm_MouseClick);
      this.MouseDoubleClick += new MouseEventHandler(this.HQMainForm_MouseDoubleClick);
      this.MouseMove += new MouseEventHandler(this.HQMainForm_MouseMove);
      this.ResumeLayout(false);
    }

    private void HQClientForm_LostFocus(object sender, EventArgs e)
    {
      if (Form.ActiveForm != this.mainWindow || this == null)
        return;
      this.Focus();
    }

    private void HQClientForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (e.KeyData != Keys.Up && e.KeyData != Keys.Down && (e.KeyData != Keys.Left && e.KeyData != Keys.Right))
        return;
      this.KDown(new KeyEventArgs(e.KeyData));
    }

    private void HQClientForm_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.HQMainForm_MouseDown(e.X, e.Y);
    }

    private void HQClientForm_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.m_isMouseLeftButtonDown = false;
    }

    public void MultiQuoteMouseLeftClick(object sender, InterFace.CommodityInfoEventArgs e)
    {
      if (this.MultiQuoteMouseEvent == null)
        return;
      this.MultiQuoteMouseEvent(sender, e);
    }

    public void KDown(KeyEventArgs e)
    {
      if (this.mainGraph == null)
        return;
      this.HQMainForm_KeyDown((object) null, e);
    }

    private void HQMainForm_Load(object sender, EventArgs e)
    {
      if (this.pluginInfo.HQResourceManager == null)
        throw new Exception("没有初始化行情系统资源");
      this.setInfo.ISDebug = Tools.StrToBool((string) this.pluginInfo.HTConfig[(object) "bDebug"], false);
      this.addMarketName = Tools.StrToBool((string) this.pluginInfo.HTConfig[(object) "addMarketName"], false);
      this.marketInfoAddress = (string) this.pluginInfo.HTConfig[(object) "MarketInfo"];
      this.curHQClient.init();
      this.curHQClient.m_commodityInfoAddress = (string) this.pluginInfo.HTConfig[(object) "CommodityInfoAddress"];
      this.SetGraphSize();
      this.BackColor = SetInfo.RHColor.clBackGround;
      try
      {
        this.mainGraph = (Page_Main) new Page_MultiQuote(this.m_rcMain, (HQForm) this);
        this.bottomGraph = new Page_Bottom(this.m_rcBottom, (HQForm) this);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      if (this.hqFormLoad != null)
        this.hqFormLoad((object) this, e);
      if (this.marketInfoAddress == null || this.marketInfoAddress.Length <= 0)
        return;
      new MarketInfo(new Uri(this.marketInfoAddress)).Show();
    }

    protected override void OnPaintBackground(PaintEventArgs paintg)
    {
    }

    private void PaintHQ(Graphics myG, int value)
    {
      lock (this)
      {
        if (!this.DoubleBuffered)
        {
          this.SetStyle(ControlStyles.UserPaint, true);
          this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
          this.SetStyle(ControlStyles.DoubleBuffer, true);
          this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        if (this.m_rcMain.Width == 0 || this.m_rcMain.Height == 0)
          return;
        if (this.mainGraph != null && myG != null)
        {
          myG.Clear(SetInfo.RHColor.clBackGround);
          this.mainGraph.Paint(myG, value);
        }
        if (this.bottomGraph == null)
          return;
        this.bottomGraph.Paint(myG);
      }
    }

    private void PaintHQ(int value)
    {
      lock (this)
      {
        if (this.m_rcMain.Width <= 0 || this.m_rcMain.Height <= 0)
          return;
        this.m_bEndPaint = false;
        using (this.m_bmp = new Bitmap(this.m_rcMain.Width, this.m_rcMain.Height))
        {
          using (Graphics resource_0 = Graphics.FromImage((Image) this.m_bmp))
          {
            if (this.mainGraph != null)
            {
              if (resource_0 != null)
              {
                resource_0.Clear(SetInfo.RHColor.clBackGround);
                this.mainGraph.Paint(resource_0, value);
              }
            }
            else if (resource_0 != null)
              resource_0.Clear(SetInfo.RHColor.clBackGround);
            this.EndPaint();
            if (!MainWindow.isLoadItemButtom)
              this.mainWindow.SetControl(false);
            else
              this.mainWindow.clearMenuItem();
          }
        }
      }
      this.mainWindow.changeBtColor();
      this.mainWindow.ChangeKLineBtnColor();
    }

    public void ClearRect(Graphics g, float x, float y, float width, float height)
    {
      if (g == null)
        return;
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clBackGround);
      g.FillRectangle((Brush) solidBrush, x, y, width, height);
      solidBrush.Dispose();
    }

    public void EndPaint()
    {
      if (!this.m_bEndPaint && this.m_bmp != null)
      {
        Graphics graphics = this.CreateGraphics();
        this.TranslateTransform(graphics);
        graphics.DrawImage((Image) this.m_bmp, 0, 0);
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
      this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
      this.bottomGraph = new Page_Bottom(this.m_rcBottom, (HQForm) this);
    }

    public void ReQueryCurClient()
    {
      switch (this.curHQClient.CurrentPage)
      {
        case 0:
          this.mainGraph = (Page_Main) new Page_MultiQuote(this.m_rcMain, (HQForm) this);
          break;
        case 1:
          this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
          break;
        case 2:
          this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
          break;
        case 4:
          this.mainGraph = (Page_Main) new Page_Bill(this.m_rcMain, (HQForm) this);
          break;
        case 5:
          this.mainGraph = (Page_Main) new Page_MarketStatus(this.m_rcMain, (HQForm) this);
          break;
      }
    }

    public void RepaintBottom()
    {
      lock (this)
      {
        if (this.m_rcBottom.Width == 0)
          return;
        this.m_bEndBottomPaint = false;
        using (this.m_bmpBottom = new Bitmap(this.m_rcBottom.Width, this.m_rcBottom.Height))
        {
          using (Graphics resource_0 = Graphics.FromImage((Image) this.m_bmpBottom))
          {
            if (this.bottomGraph != null)
            {
              if (resource_0 != null)
              {
                resource_0.Clear(SetInfo.RHColor.clBackGround);
                this.bottomGraph.Paint(resource_0);
              }
            }
            else if (resource_0 != null)
              resource_0.Clear(SetInfo.RHColor.clBackGround);
            this.EndBottomPaint();
          }
        }
      }
    }

    public void EndBottomPaint()
    {
      if (!this.m_bEndBottomPaint && this.m_bmpBottom != null)
      {
        Graphics graphics = this.CreateGraphics();
        this.TranslateTransform(graphics);
        graphics.DrawImage((Image) this.m_bmpBottom, this.m_rcBottom.X, this.m_rcBottom.Y);
      }
      this.m_bEndBottomPaint = true;
    }

    public Graphics TranslateTransform(Graphics g)
    {
      Point autoScrollPosition = this.AutoScrollPosition;
      g.TranslateTransform((float) autoScrollPosition.X, (float) autoScrollPosition.Y);
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
      string str = sCode;
      if (this.curHQClient.m_commodityInfoAddress == null || this.curHQClient.m_commodityInfoAddress.Length <= 0)
        return;
      CommodityInfoF instance = CommodityInfoF.GetInstance((HQForm) this);
      instance.Url = this.curHQClient.m_commodityInfoAddress;
      instance.CommodityID = str;
      instance.setUri();
      instance.Dock = DockStyle.Fill;
      instance.FormBorderStyle = FormBorderStyle.None;
      instance.BringToFront();
      instance.TopLevel = false;
      this.Controls.Add((Control) instance);
      instance.Show();
    }

    public void QueryStock(CommodityInfo commodityInfo)
    {
      this.curHQClient.curCommodityInfo = commodityInfo;
      if (1 == this.curHQClient.CurrentPage)
      {
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 2;
      }
      else if (2 == this.curHQClient.CurrentPage)
      {
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 1;
      }
      else if (this.curHQClient.globalData.PrePage == 2)
      {
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 2;
      }
      else
      {
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 1;
      }
    }

    public void ShowPageKLine(CommodityInfo commodityInfo)
    {
      this.curHQClient.curCommodityInfo = commodityInfo;
      this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
      this.Repaint();
    }

    public void ShowPageMinLine(CommodityInfo commodityInfo)
    {
      this.curHQClient.curCommodityInfo = commodityInfo;
      this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
      this.Repaint();
    }

    public void ShowPageMinLine()
    {
      this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
      this.Repaint();
    }

    public void ShowPageKLine()
    {
      this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
      this.Repaint();
    }

    public void ChangeStock(bool bUp)
    {
      if (this.multiQuoteData.MultiQuotePage == 1)
      {
        int index1 = -1;
        CommodityInfo commodityInfo = this.curHQClient.curCommodityInfo;
        for (int index2 = 0; index2 < this.multiQuoteData.MyCommodityList.Count; ++index2)
        {
          if (commodityInfo.Compare(CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[index2].ToString())))
          {
            index1 = index2;
            break;
          }
        }
        if (index1 == -1)
        {
          if (this.multiQuoteData.MyCommodityList.Count > 0)
            commodityInfo = CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[0].ToString());
        }
        else
        {
          if (bUp)
          {
            if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
            {
              --index1;
              Logger.wirte(MsgType.Information, "Index -- 1 = " + index1.ToString());
              if (index1 < 0)
                index1 = this.multiQuoteData.MyCommodityList.Count - 1;
            }
          }
          else if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
          {
            ++index1;
            Logger.wirte(MsgType.Information, "Index ++ 1 = " + index1.ToString());
            if (index1 >= this.multiQuoteData.MyCommodityList.Count)
              index1 = 0;
          }
          commodityInfo = CommodityInfo.DealCode(this.multiQuoteData.MyCommodityList[index1].ToString());
        }
        if (1 == this.curHQClient.CurrentPage)
        {
          this.ShowPageMinLine(commodityInfo);
        }
        else
        {
          if (2 != this.curHQClient.CurrentPage)
            return;
          this.ShowPageKLine(commodityInfo);
        }
      }
      else
      {
        if (this.curHQClient.curCommodityInfo == null)
          return;
        int index1 = -1;
        for (int index2 = 0; index2 < this.multiQuoteData.m_curQuoteList.Length; ++index2)
        {
          if (this.curHQClient.curCommodityInfo.Compare(new CommodityInfo(this.multiQuoteData.m_curQuoteList[index2].marketID, this.multiQuoteData.m_curQuoteList[index2].code)))
          {
            index1 = index2;
            break;
          }
        }
        if (index1 == -1)
        {
          if (this.multiQuoteData.m_curQuoteList.Length > 0)
            this.curHQClient.curCommodityInfo = new CommodityInfo(this.multiQuoteData.m_curQuoteList[0].marketID, this.multiQuoteData.m_curQuoteList[0].code);
        }
        else
        {
          if (bUp)
          {
            if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
            {
              --index1;
              Logger.wirte(MsgType.Information, "Index -- 2 = " + index1.ToString());
              if (index1 < 0)
                index1 = this.multiQuoteData.m_curQuoteList.Length - 1;
            }
          }
          else if (!this.curHQClient.isNeedAskData || !this.isMouseWheel)
          {
            ++index1;
            Logger.wirte(MsgType.Information, "Index ++ 2 = " + index1.ToString());
            if (index1 >= this.multiQuoteData.m_curQuoteList.Length)
              index1 = 0;
          }
          this.curHQClient.curCommodityInfo = new CommodityInfo(this.multiQuoteData.m_curQuoteList[index1].marketID, this.multiQuoteData.m_curQuoteList[index1].code);
        }
        if (1 == this.curHQClient.CurrentPage)
        {
          this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
        }
        else
        {
          if (2 != this.curHQClient.CurrentPage)
            return;
          this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
        }
      }
    }

    private void SetGraphSize()
    {
      int width = this.ClientSize.Width;
      int height = this.ClientSize.Height;
      if (width == 0 && height == 0)
        return;
      if (width < this.AutoScrollMinSize.Width)
        width = this.AutoScrollMinSize.Width;
      if (height < this.AutoScrollMinSize.Height)
        height = this.AutoScrollMinSize.Height;
      this.m_rcMain = new Rectangle(new Point(0, 0), new Size(width, height));
      this.m_rcBottom = new Rectangle(new Point(0, 0), new Size(width, height));
      this.m_rcMain.Height -= this.bottomHeight;
      this.m_rcBottom.Y = this.m_rcMain.Y + this.m_rcMain.Height;
      this.m_rcBottom.Height = this.bottomHeight;
      this.scrollOffset = this.AutoScrollPosition;
    }

    private void HQMainForm_ClientSizeChanged(object sender, EventArgs e)
    {
      this.SetGraphSize();
      if (this.mainGraph == null)
        return;
      this.mainGraph.m_rc = this.m_rcMain;
      if (this.bottomGraph != null)
        this.bottomGraph.rc = this.m_rcBottom;
      this.Invalidate();
    }

    private void HQMainForm_Scroll(object sender, ScrollEventArgs e)
    {
      this.scrollOffset = this.AutoScrollPosition;
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
        this.curHQClient.Dispose();
      if (this.mainGraph != null)
        this.mainGraph.PageDispose();
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
      KeyEventArgs e1;
      if (e.Delta > 0)
      {
        e1 = new KeyEventArgs(Keys.Prior);
        this.isUp = true;
      }
      else
      {
        e1 = new KeyEventArgs(Keys.Next);
        this.isUp = false;
      }
      this.MainForm_KeyDown((object) this, e1);
      if (!this.bNeedRepaint)
        return;
      this.Repaint();
    }

    public void HQMainForm_MouseClick(object sender, MouseEventArgs e)
    {
      this.isMouseWheel = false;
      int x = e.X;
      int y = e.Y;
      if (y >= this.m_rcMain.Y + this.m_rcMain.Height)
        return;
      if (this.MainForm_MouseClick != null && e.Button == MouseButtons.Left)
      {
        this.MainForm_MouseClick((object) this, e);
      }
      else
      {
        if (e.Button != MouseButtons.Right)
          return;
        if (this.curHQClient.CurrentPage == 0 && y > this.m_rcMain.Y + this.m_rcMain.Height - this.multiQuoteData.buttonHight)
        {
          this.MainForm_MouseClick((object) this, e);
        }
        else
        {
          this.MainForm_MouseClick((object) this, e);
          if (this.mainGraph == null)
            return;
          this.mainGraph.contextMenuStrip.Show((Control) this, e.X, e.Y);
        }
      }
    }

    public void HQMainForm_KeyDown(object sender, KeyEventArgs e)
    {
      this.isMouseWheel = false;
      switch (e.KeyData)
      {
        case Keys.Escape:
          if (this.mainGraph != null)
          {
            if (this.mainGraph != null && (this.curHQClient.CurrentPage == 1 || this.curHQClient.CurrentPage == 2))
            {
              if (this.MainForm_KeyDown != null)
                this.MainForm_KeyDown((object) this, e);
              if (!this.bNeedRepaint && this.curHQClient.CurrentPage != 0)
              {
                this.mainGraph = (Page_Main) new Page_MultiQuote(this.m_rcMain, (HQForm) this);
                this.bNeedRepaint = true;
                break;
              }
              break;
            }
            if (this.curHQClient.CurrentPage != 0)
            {
              this.mainGraph = (Page_Main) new Page_MultiQuote(this.m_rcMain, (HQForm) this);
              this.bNeedRepaint = true;
              break;
            }
            break;
          }
          break;
        case Keys.F1:
          if (this.curHQClient.CurrentPage == 1 || this.curHQClient.CurrentPage == 2)
          {
            this.UserCommand("01");
            break;
          }
          break;
        case Keys.F2:
          this.UserCommand("60");
          break;
        case Keys.F3:
          if (this.curHQClient.indexMainCode.Length > 0)
          {
            this.UserCommand("INDEX_" + this.curHQClient.indexMainCode);
            break;
          }
          break;
        case Keys.F4:
          this.UserCommand("80");
          break;
        case Keys.F5:
          this.UserCommand("05");
          break;
        case Keys.F7:
          this.UserCommand("70");
          break;
        default:
          if (this.mainGraph != null && this.MainForm_KeyDown != null)
          {
            this.MainForm_KeyDown((object) this, e);
            break;
          }
          break;
      }
      if (!this.bNeedRepaint)
        return;
      this.Repaint();
    }

    public void HQMainForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.isMouseWheel = false;
      char keyChar = e.KeyChar;
      if (char.IsLetterOrDigit(keyChar))
      {
        InputDialog inputDialog = new InputDialog(keyChar, (HQForm) this);
        Rectangle bounds = this.Bounds;
        int num1 = bounds.Right - inputDialog.Width - 5;
        int num2 = bounds.Bottom - inputDialog.Height - this.m_rcBottom.Height - 5;
        inputDialog.Left = num1;
        inputDialog.Top = num2;
        if (inputDialog.ShowDialog() != DialogResult.OK)
          return;
        string str = inputDialog.strCmd;
        if (str == null || str.Length == 0)
          return;
        switch (str[0])
        {
          case 'A':
            this.UserCommand(str.Substring(1));
            break;
          case 'C':
            this.UserCommand(str.Substring(0));
            break;
          case 'P':
            if (this.curHQClient.CurrentPage == 2)
            {
              this.ShowPageKLine(CommodityInfo.DealCode(str.Substring(1)));
              break;
            }
            this.ShowPageMinLine(CommodityInfo.DealCode(str.Substring(1)));
            break;
          case 'T':
            this.curHQClient.m_strIndicator = str.Substring(1);
            ((Page_KLine) this.mainGraph).draw_KLine.CreateIndicator();
            break;
        }
        this.bNeedRepaint = true;
      }
      else
      {
        if (this.MainForm_KeyPress == null)
          return;
        this.MainForm_KeyPress((object) this, e);
      }
    }

    public void HQMainForm_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.MainForm_MouseMove == null)
        return;
      this.MainForm_MouseMove((object) this, e);
    }

    public void HQMainForm_MouseDown(int x, int y)
    {
      this.m_isMouseLeftButtonDown = true;
      this.m_mouseBeforeMove = new Point(x, y);
    }

    public void HQMainForm_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.MainForm_MouseDoubleClick == null)
        return;
      this.MainForm_MouseDoubleClick((object) this, e);
    }

    public void UserCommand(string sCmd)
    {
      this.bNeedRepaint = true;
      if (sCmd == null || sCmd.Length == 0)
        return;
      if (sCmd.StartsWith("INDEX_"))
      {
        this.curHQClient.curCommodityInfo = CommodityInfo.DealCode(sCmd.Substring(6));
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
      }
      else if (sCmd.StartsWith("SERIES_"))
      {
        this.curHQClient.curCommodityInfo = CommodityInfo.DealCode(sCmd.Substring(7));
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
      }
      else if (sCmd.Equals("page_history"))
        this.UserCommand("70");
      else if (sCmd.Equals("refreshBt"))
      {
        this.curHQClient.CurrentPage = this.curHQClient.CurrentPage;
        this.curHQClient.isChangePage = 0;
        this.curHQClient.m_hqForm.M_Graphics.FillRectangle(Brushes.Black, this.curHQClient.m_hqForm.MainGraph.m_rc);
        this.curHQClient.m_hqForm.Repaint();
      }
      else if (sCmd.StartsWith("Color"))
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
          int num1 = (int) new UserSet((HQForm) this).ShowDialog();
        }
        int num2;
        try
        {
          num2 = int.Parse(sCmd);
        }
        catch
        {
          return;
        }
        Logger.wirte(MsgType.Information, "sCmd =" + (object) num2);
        switch (num2)
        {
          case 60:
            this.mainGraph = (Page_Main) new Page_MultiQuote(this.m_rcMain, (HQForm) this);
            break;
          case 70:
            this.mainGraph = (Page_Main) new Page_History(this.m_rcMain, (HQForm) this);
            break;
          case 80:
            this.mainGraph = (Page_Main) new Page_MarketStatus(this.m_rcMain, (HQForm) this);
            break;
          case 1:
            this.mainGraph = (Page_Main) new Page_Bill(this.m_rcMain, (HQForm) this);
            break;
          case 5:
            this.OnF5();
            break;
        }
        this.mainWindow.changeBtColor();
      }
    }

    public void OnF5()
    {
      if (this.curHQClient.curCommodityInfo == null)
        return;
      if (1 == this.curHQClient.CurrentPage)
      {
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 2;
      }
      else if (2 == this.curHQClient.CurrentPage)
      {
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 1;
      }
      else if (this.curHQClient.globalData.PrePage == 2)
      {
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 2;
      }
      else
      {
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
        this.curHQClient.globalData.PrePage = 1;
      }
    }

    public void DisplayHQSunForm(CommodityInfo commodityInfo)
    {
      HQSubForm hqSubForm = new HQSubForm(commodityInfo, this.pluginInfo, this.setInfo);
      if (this.MdiParent != null)
        hqSubForm.MdiParent = this.MdiParent;
      hqSubForm.Show();
    }

    public void ReMakeIndexMenu()
    {
      if (this.mainGraph == null)
        return;
      this.BeginInvoke((Delegate) new MethodInvoker(this.mainGraph.AddIndexMenu));
    }

    private void timerExChange_Tick(object sender, EventArgs e)
    {
      this.curHQClient.isNeedAskData = true;
      this.timerExChange.Enabled = false;
      if (this.curHQClient.CurrentPage != 2 && this.curHQClient.CurrentPage != 1)
        return;
      this.MouseWheelMethodByTimer(this.MouseWheelArg);
    }
  }
}
