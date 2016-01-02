// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.HQSubForm
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.Properties;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
  public class HQSubForm : Form, HQForm
  {
    private int iStyle = Settings.Default.iStyle;
    private Point scrollOffset = new Point();
    private IContainer components;
    public PluginInfo pluginInfo;
    public SetInfo setInfo;
    private bool isMultiCycle;
    private bool isMultiMarket;
    private Rectangle m_rcMain;
    private Bitmap m_bmp;
    private CommodityInfo m_commodity;
    private HQClientMain curHQClient;
    private int _KLineValue;
    public Page_Main mainGraph;
    private bool bNeedRepaint;
    private bool m_bEndPaint;
    private bool addMarketName;

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
        return this.CreateGraphics();
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

    public event MouseEventHandler MainForm_MouseClick;

    public event KeyEventHandler MainForm_KeyDown;

    public event KeyPressEventHandler MainForm_KeyPress;

    public event MouseEventHandler MainForm_MouseMove;

    public event MouseEventHandler MainForm_MouseDoubleClick;

    public HQSubForm(CommodityInfo commodity, PluginInfo _pluginInfo, SetInfo _setInfo)
    {
      this.InitializeComponent();
      this.pluginInfo = _pluginInfo;
      this.setInfo = _setInfo;
      this.m_commodity = commodity;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(561, 399);
      this.Name = "HQSubForm";
      this.Text = "HQSubForm";
      this.FormClosed += new FormClosedEventHandler(this.HQSubForm_FormClosed);
      this.MouseDoubleClick += new MouseEventHandler(this.HQMainForm_MouseDoubleClick);
      this.ClientSizeChanged += new EventHandler(this.HQSubForm_ClientSizeChanged);
      this.Paint += new PaintEventHandler(this.HQSubForm_Paint);
      this.MouseClick += new MouseEventHandler(this.HQMainForm_MouseClick);
      this.KeyPress += new KeyPressEventHandler(this.HQMainForm_KeyPress);
      this.FormClosing += new FormClosingEventHandler(this.HQSubForm_FormClosing);
      this.MouseMove += new MouseEventHandler(this.HQMainForm_MouseMove);
      this.KeyDown += new KeyEventHandler(this.HQMainForm_KeyDown);
      this.Scroll += new ScrollEventHandler(this.HQSubForm_Scroll);
      this.Load += new EventHandler(this.HQSubForm_Load);
      this.ResumeLayout(false);
    }

    protected override void OnPaintBackground(PaintEventArgs paintg)
    {
    }

    private void PaintHQ(Graphics myG, int value, int type)
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
        if (this.m_rcMain.Width == 0 || this.m_rcMain.Height == 0 || this.mainGraph == null)
          return;
        this.TranslateTransform(myG);
        if (myG == null)
          return;
        myG.Clear(SetInfo.RHColor.clBackGround);
        this.mainGraph.Paint(myG, value);
      }
    }

    private void PaintHQ(int value)
    {
      lock (this)
      {
        if (this.m_rcMain.Width == 0 || this.m_rcMain.Height == 0)
          return;
        this.m_bEndPaint = false;
        using (this.m_bmp = new Bitmap(this.m_rcMain.Width, this.m_rcMain.Height))
        {
          using (Graphics resource_0 = Graphics.FromImage((Image) this.m_bmp))
          {
            if (this.mainGraph != null)
            {
              this.TranslateTransform(resource_0);
              if (resource_0 != null)
              {
                resource_0.Clear(SetInfo.RHColor.clBackGround);
                this.mainGraph.Paint(resource_0, value);
              }
            }
            else if (resource_0 != null)
              resource_0.Clear(SetInfo.RHColor.clBackGround);
            this.EndPaint();
          }
        }
      }
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
        this.CreateGraphics().DrawImage((Image) this.m_bmp, 0, 0);
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
      Point autoScrollPosition = this.AutoScrollPosition;
      g.TranslateTransform((float) autoScrollPosition.X, (float) autoScrollPosition.Y);
      return g;
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
      if (this.curHQClient.multiQuoteData.MultiQuotePage == 1)
      {
        int num = -1;
        CommodityInfo commodityInfo = this.curHQClient.curCommodityInfo;
        for (int index = 0; index < this.curHQClient.multiQuoteData.MyCommodityList.Count; ++index)
        {
          if (commodityInfo.Compare(CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[index].ToString())))
          {
            num = index;
            break;
          }
        }
        if (num == -1)
        {
          if (this.curHQClient.multiQuoteData.MyCommodityList.Count > 0)
            commodityInfo = CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[0].ToString());
        }
        else
        {
          int index;
          if (bUp)
          {
            index = num - 1;
            if (index < 0)
              index = this.curHQClient.multiQuoteData.MyCommodityList.Count - 1;
          }
          else
          {
            index = num + 1;
            if (index >= this.curHQClient.multiQuoteData.MyCommodityList.Count)
              index = 0;
          }
          commodityInfo = CommodityInfo.DealCode(this.curHQClient.multiQuoteData.MyCommodityList[index].ToString());
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
        CodeTable codeTable = (CodeTable) this.curHQClient.m_htProduct[(object) (this.curHQClient.curCommodityInfo.marketID + this.curHQClient.curCommodityInfo.commodityCode)];
        ArrayList arrayList = this.curHQClient.m_codeList;
        if (codeTable != null && codeTable.status == 1)
          arrayList = this.curHQClient.hm_codeList;
        else if (codeTable != null)
          arrayList = this.curHQClient.nm_codeList;
        int num = -1;
        for (int index = 0; index < arrayList.Count; ++index)
        {
          if (this.curHQClient.curCommodityInfo.Compare(arrayList[index]))
          {
            num = index;
            break;
          }
        }
        if (num == -1)
        {
          if (arrayList.Count > 0)
            this.curHQClient.curCommodityInfo = (CommodityInfo) arrayList[0];
        }
        else
        {
          int index;
          if (bUp)
          {
            index = num - 1;
            if (index < 0)
              index = arrayList.Count - 1;
          }
          else
          {
            index = num + 1;
            if (index >= arrayList.Count)
              index = 0;
          }
          this.curHQClient.curCommodityInfo = (CommodityInfo) arrayList[index];
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
      }
    }

    private void OnF5()
    {
      if (this.curHQClient.curCommodityInfo == null)
        return;
      if (1 == this.curHQClient.CurrentPage)
        this.mainGraph = (Page_Main) new Page_KLine(this.m_rcMain, (HQForm) this);
      else
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
    }

    public void ReMakeIndexMenu()
    {
      if (this.mainGraph == null)
        return;
      this.BeginInvoke((Delegate) new MethodInvoker(this.mainGraph.AddIndexMenu));
    }

    private void HQMainForm_MouseClick(object sender, MouseEventArgs e)
    {
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
        if (this.curHQClient.CurrentPage == 0 && y > this.m_rcMain.Y + this.m_rcMain.Height - this.curHQClient.multiQuoteData.buttonHight)
        {
          this.MainForm_MouseClick((object) this, e);
        }
        else
        {
          if (this.mainGraph == null)
            return;
          this.mainGraph.contextMenuStrip.Show((Control) this, e.X, e.Y);
        }
      }
    }

    public void MultiQuoteMouseLeftClick(object sender, InterFace.CommodityInfoEventArgs e)
    {
    }

    public void HQMainForm_KeyDown(object sender, KeyEventArgs e)
    {
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

    private void HQMainForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      if (char.IsLetterOrDigit(keyChar))
      {
        InputDialog inputDialog = new InputDialog(keyChar, (HQForm) this);
        Rectangle bounds = this.Bounds;
        int num1 = bounds.Right - inputDialog.Width - 5;
        int num2 = bounds.Bottom - inputDialog.Height - 5;
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

    private void HQMainForm_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.MainForm_MouseMove == null)
        return;
      this.MainForm_MouseMove((object) this, e);
    }

    private void HQMainForm_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.MainForm_MouseDoubleClick == null)
        return;
      this.MainForm_MouseDoubleClick((object) this, e);
    }

    private void HQSubForm_Scroll(object sender, ScrollEventArgs e)
    {
      this.scrollOffset = this.AutoScrollPosition;
    }

    private void HQSubForm_Paint(object sender, PaintEventArgs e)
    {
      this.Repaint();
      this.RepaintBottom();
    }

    private void HQSubForm_Load(object sender, EventArgs e)
    {
      if (this.pluginInfo.HQResourceManager == null)
        throw new Exception("没有初始化行情系统资源");
      if (this.m_commodity == null)
        throw new Exception("没有初始化商品代码");
      bool.TryParse((string) this.pluginInfo.HTConfig[(object) "addMarketName"], out this.addMarketName);
      this.SetGraphSize();
      HQClientMain hqClientMain = new HQClientMain((HQForm) this);
      hqClientMain.init();
      this.curHQClient = hqClientMain;
      this.curHQClient.curCommodityInfo = this.m_commodity;
      try
      {
        this.mainGraph = (Page_Main) new Page_MinLine(this.m_rcMain, (HQForm) this);
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
        this.curHQClient.Dispose();
      if (this.mainGraph == null)
        return;
      this.mainGraph.PageDispose();
    }

    private void HQSubForm_ClientSizeChanged(object sender, EventArgs e)
    {
      this.SetGraphSize();
      if (this.mainGraph == null)
        return;
      this.mainGraph.m_rc = this.m_rcMain;
      this.Invalidate();
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
      this.scrollOffset = this.AutoScrollPosition;
    }
  }
}
