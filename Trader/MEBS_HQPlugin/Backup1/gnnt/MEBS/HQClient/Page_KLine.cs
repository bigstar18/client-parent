// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_KLine
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.HQThread;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class Page_KLine : Page_Main
  {
    private int rcQuoteWidth = 250;
    private int iProductType;
    internal Draw_KLine draw_KLine;
    private Draw_MinLine draw_MinLine;
    private Draw_Quote draw_Quote;
    private Rectangle rcKLine;
    private Rectangle rcQuote;
    private Draw_MinLine DM;
    public PluginInfo pluginInfo;
    public SetInfo setInfo;
    private ButtonUtils buttonUtils;
    private List<clickRect> listRect;
    public bool isDrawCursor;
    private int minX;
    private int minY;
    private ToolStripMenuItem menuZoomIn;
    private ToolStripMenuItem menuZoomOut;

    public Page_KLine(Rectangle _rc, HQForm hqForm)
      : base(_rc, hqForm)
    {
      try
      {
        this.pluginInfo = this.m_pluginInfo;
        this.setInfo = this.m_setInfo;
        this.buttonUtils = hqForm.CurHQClient.buttonUtils;
        if (this.m_hqClient.isNeedAskData)
          this.AskForDataOnce();
        this.m_hqClient.CurrentPage = 2;
        this.draw_KLine = new Draw_KLine(this);
        this.draw_MinLine = new Draw_MinLine(this.m_hqForm, false);
        this.DM = new Draw_MinLine(this.m_hqForm, true);
        this.MakeMenus();
        this.iProductType = this.m_hqClient.getProductType(this.m_hqClient.curCommodityInfo);
        this.m_hqClient.kLineUpDown = new HQClientMain.KLineUpDownCallBack(this.KLineUpDown);
        this.m_hqClient.createIndicator = new HQClientMain.CreateIndicatorCallback(this.draw_KLine.CreateIndicator);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    private void AskForDataOnce()
    {
      try
      {
        ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
        DateTime time = new DateTime();
        if (productData != null && productData.realData != null)
          time = productData.realData.time;
        SendThread.AskForRealQuote(this.m_hqClient.curCommodityInfo.marketID, this.m_hqClient.curCommodityInfo.commodityCode, time, this.m_hqClient.sendThread);
        this.AskForKLine();
        CMDMinVO cmdMinVo = new CMDMinVO();
        CommidityVO commidityVo = new CommidityVO();
        commidityVo.code = this.m_hqClient.curCommodityInfo.commodityCode;
        commidityVo.marketID = this.m_hqClient.curCommodityInfo.marketID;
        commidityVo.location = (byte) 1;
        cmdMinVo.mark = (byte) 1;
        cmdMinVo.type = (byte) 0;
        cmdMinVo.date = 0;
        cmdMinVo.time = 0;
        cmdMinVo.commidityList = new List<CommidityVO>(1)
        {
          commidityVo
        };
        this.m_hqClient.sendThread.AskForData((CMDVO) cmdMinVo);
        CMDBillByVersionVO cmdBillByVersionVo = new CMDBillByVersionVO();
        cmdBillByVersionVo.marketID = this.m_hqClient.curCommodityInfo.marketID;
        cmdBillByVersionVo.code = this.m_hqClient.curCommodityInfo.commodityCode;
        cmdBillByVersionVo.type = (byte) 2;
        cmdBillByVersionVo.time = 0L;
        cmdBillByVersionVo.ReservedField = string.Empty;
        if (productData == null || productData.aBill.Count == 0)
        {
          cmdBillByVersionVo.totalAmount = 0L;
        }
        else
        {
          BillDataVO billDataVo = (BillDataVO) productData.aBill[productData.aBill.Count - 1];
          cmdBillByVersionVo.totalAmount = billDataVo.totalAmount;
        }
        this.m_hqClient.sendThread.AskForData((CMDVO) cmdBillByVersionVo);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    protected override void AskForDataOnTimer()
    {
    }

    internal void AskForKLine()
    {
      ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
      if (productData != null)
      {
        if (1 == this.m_hqClient.m_iKLineCycle || 2 == this.m_hqClient.m_iKLineCycle || (3 == this.m_hqClient.m_iKLineCycle || 4 == this.m_hqClient.m_iKLineCycle) || 13 == this.m_hqClient.m_iKLineCycle)
        {
          if (productData.dayKLine != null)
            return;
        }
        else if (7 == this.m_hqClient.m_iKLineCycle)
        {
          if (productData.min5KLine != null)
            return;
        }
        else if (productData.min1KLine != null)
          return;
      }
      this.m_hqClient.httpThread.AskForData(new Packet_HttpRequest()
      {
        marketID = this.m_hqClient.curCommodityInfo.marketID,
        sCode = this.m_hqClient.curCommodityInfo.commodityCode,
        type = 1 == this.m_hqClient.m_iKLineCycle || 2 == this.m_hqClient.m_iKLineCycle || (3 == this.m_hqClient.m_iKLineCycle || 4 == this.m_hqClient.m_iKLineCycle) || 13 == this.m_hqClient.m_iKLineCycle ? (byte) 0 : (7 != this.m_hqClient.m_iKLineCycle ? (byte) 2 : (byte) 1)
      });
    }

    public override void Paint(Graphics g, int value)
    {
      try
      {
        ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
        Font font1 = new Font("楷体_GB2312", 26f, FontStyle.Bold);
        Font font2 = new Font("宋体", 12f, FontStyle.Regular);
        int height1 = this.m_rc.Height;
        int height2 = font1.Height;
        font1.Dispose();
        font2.Dispose();
        this.rcQuote = new Rectangle(this.m_rc.X + this.m_rc.Width - this.rcQuoteWidth, this.m_rc.Y, this.rcQuoteWidth, height1);
        this.draw_Quote = new Draw_Quote(this.m_hqClient);
        this.draw_Quote.Paint(g, this.rcQuote, productData, this.m_hqClient.curCommodityInfo, this.m_hqClient.iShowBuySellPrice, this.m_hqClient);
        this.rcKLine = new Rectangle(this.m_rc.X, this.m_rc.Y, this.m_rc.Width - this.rcQuoteWidth, this.m_rc.Height);
        if (this.m_hqForm.IsMultiCycle)
          this.paintMoreKLine(g, productData);
        else
          this.draw_KLine.Paint(g, this.rcKLine, productData, value);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    private void paintMoreKLine(Graphics g, ProductData stock)
    {
      this.m_hqClient.m_iKLineCycle = 5;
      this.AskForKLine();
      this.m_hqClient.m_iKLineCycle = 1;
      this.AskForKLine();
      int num1 = this.m_globalData.rectRol * this.m_globalData.rectCol;
      if (this.listRect == null)
        this.listRect = new List<clickRect>();
      if (this.listRect.Count != num1)
        this.listRect.Clear();
      int num2 = this.rcKLine.Width / this.m_globalData.rectCol;
      int num3 = this.rcKLine.Height / this.m_globalData.rectRol;
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_globalData.rectRol; ++index2)
      {
        for (int index3 = 0; index3 < this.m_globalData.rectCol; ++index3)
        {
          clickRect clickRect = new clickRect();
          Rectangle rc = new Rectangle();
          rc.X = this.rcKLine.X + index3 * num2;
          rc.Y = this.rcKLine.Y + index2 * num3;
          rc.Height = num3;
          rc.Width = num2;
          if (index1 == 0)
          {
            this.DM.Paint(g, rc, stock);
            clickRect.OwnRect = rc;
            clickRect.OwnMinKLine = this.DM;
          }
          else
          {
            Draw_KLine drawKline = new Draw_KLine(this);
            if (num1 == 2)
              this.m_hqClient.m_iKLineCycle = 1;
            else if (num1 == 4)
            {
              switch (index1)
              {
                case 1:
                  this.m_hqClient.m_iKLineCycle = 7;
                  break;
                case 2:
                  this.m_hqClient.m_iKLineCycle = 10;
                  break;
                case 3:
                  this.m_hqClient.m_iKLineCycle = 1;
                  break;
              }
            }
            else if (num1 == 6)
            {
              switch (index1)
              {
                case 1:
                  this.m_hqClient.m_iKLineCycle = 7;
                  break;
                case 2:
                  this.m_hqClient.m_iKLineCycle = 9;
                  break;
                case 3:
                  this.m_hqClient.m_iKLineCycle = 10;
                  break;
                case 4:
                  this.m_hqClient.m_iKLineCycle = 1;
                  break;
                case 5:
                  this.m_hqClient.m_iKLineCycle = 2;
                  break;
              }
            }
            else if (num1 == 9)
            {
              switch (index1)
              {
                case 1:
                  this.m_hqClient.m_iKLineCycle = 5;
                  break;
                case 2:
                  this.m_hqClient.m_iKLineCycle = 7;
                  break;
                case 3:
                  this.m_hqClient.m_iKLineCycle = 8;
                  break;
                case 4:
                  this.m_hqClient.m_iKLineCycle = 9;
                  break;
                case 5:
                  this.m_hqClient.m_iKLineCycle = 10;
                  break;
                case 6:
                  this.m_hqClient.m_iKLineCycle = 1;
                  break;
                case 7:
                  this.m_hqClient.m_iKLineCycle = 2;
                  break;
                case 8:
                  this.m_hqClient.m_iKLineCycle = 3;
                  break;
              }
            }
            clickRect.Cycle = this.m_hqClient.m_iKLineCycle;
            clickRect.OwnRect = rc;
            clickRect.OwnDrawKLine = drawKline;
            drawKline.Paint(g, rc, stock, 0);
          }
          if (this.listRect.Count != num1)
            this.listRect.Add(clickRect);
          else
            this.listRect[index1] = clickRect;
          g.DrawRectangle(new Pen(SetInfo.RHColor.clGrid, 1f), new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2));
          ++index1;
        }
      }
      if (this.isDrawCursor)
      {
        if (this.m_globalData.SelectNum == 1)
        {
          if (this.listRect[0].OwnMinKLine.m_iPos >= 0)
          {
            this.m_hqClient.m_iKLineCycle = this.listRect[0].Cycle;
            this.listRect[0].OwnMinKLine.DrawLabel(g);
          }
        }
        else if (this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine != null)
        {
          this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.m_iPos = this.m_globalData.newPos;
          this.m_hqClient.m_iKLineCycle = this.listRect[this.m_globalData.SelectNum - 1].Cycle;
          this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.DrawLabel(g);
        }
      }
      this.m_hqForm.EndPaint();
      for (int index2 = 1; index2 < num1; ++index2)
      {
        if (this.listRect[index2].OwnDrawKLine != null)
        {
          this.m_hqClient.m_iKLineCycle = this.listRect[index2].Cycle;
          this.listRect[index2].OwnDrawKLine.DrawCursor(-1);
        }
      }
      if (this.m_globalData.SelectNum == 1)
      {
        if (this.listRect[0].OwnMinKLine.m_iPos >= 0 && this.isDrawCursor)
        {
          this.listRect[0].OwnMinKLine.m_YSrcBmp = (Bitmap) null;
          this.m_hqClient.m_iKLineCycle = this.listRect[0].Cycle;
          this.listRect[0].OwnMinKLine.DrawCursor(-1);
        }
        else if (this.isDrawCursor)
        {
          int minLineIndexFromX = this.listRect[0].OwnMinKLine.GetMinLineIndexFromX(this.minX);
          this.listRect[0].OwnMinKLine.m_YSrcBmp = (Bitmap) null;
          this.m_hqClient.m_iKLineCycle = this.listRect[0].Cycle;
          this.listRect[0].OwnMinKLine.DrawCursor(minLineIndexFromX);
        }
      }
      if (this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine == null || !this.isDrawCursor)
        return;
      this.m_hqClient.m_iKLineCycle = this.listRect[this.m_globalData.SelectNum - 1].Cycle;
      this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.DrawCursor(this.m_globalData.newPos);
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      int x = e.X - this.m_hqForm.ScrollOffset.X;
      int y = e.Y - this.m_hqForm.ScrollOffset.Y;
      if (e.Button == MouseButtons.Left && this.draw_Quote != null && (y > this.rcQuote.Y + this.rcQuote.Height - this.draw_Quote.rcRightButton.Height && y < this.draw_Quote.rcRightButton.Y + this.draw_Quote.rcRightButton.Height))
        this.ClickButton(x, y);
      if (!this.m_hqForm.IsMultiCycle)
      {
        if (this.rcKLine.Contains(e.X, e.Y) && this.draw_KLine != null && e.Button == MouseButtons.Left)
        {
          this.isDrawCursor = !this.isDrawCursor;
          if (this.isDrawCursor)
            this.draw_KLine.m_iPos = this.draw_KLine.getClickPointX(e.X, e.Y);
          this.m_hqForm.Repaint();
        }
      }
      else if (this.rcKLine.Contains(e.X, e.Y) && this.listRect != null && (this.draw_KLine != null && e.Button == MouseButtons.Left))
      {
        for (int index = 0; index < this.listRect.Count; ++index)
        {
          if (this.listRect[index].OwnRect.Contains(e.X, e.Y))
          {
            this.m_globalData.SelectNum = index + 1;
            this.isDrawCursor = !this.isDrawCursor;
            if (index == 0)
            {
              this.minX = e.X;
              this.minY = e.Y;
              this.DM.MouseDoubleClick(e.X, e.Y);
            }
            if (!this.isDrawCursor)
              this.m_hqForm.Repaint();
            if (index != 0 && this.isDrawCursor)
            {
              this.listRect[index].OwnDrawKLine.MouseLeftClicked(e.X, e.Y);
              this.m_hqClient.m_iKLineCycle = this.listRect[index].Cycle;
              this.m_globalData.newPos = this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.getClickPointX(e.X, e.Y);
              this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.m_iPos = this.m_globalData.newPos;
              this.m_hqForm.Repaint();
            }
          }
        }
      }
      if (this.rcQuote.Contains(e.X, e.Y))
        this.draw_Quote.MouseLeftClick(e.X, e.Y, this.m_hqForm, this.m_hqClient);
      ((HQClientForm) this.m_hqForm).mainWindow.Focus();
    }

    private void ClickButton(int x, int y)
    {
      MyButton myButton = this.draw_Quote.rightbuttonGraph.MouseLeftClicked(x, y, this.buttonUtils.RightButtonList, false);
      if (myButton == null)
        return;
      this.buttonUtils.CuRightrButtonName = myButton.Name;
      this.m_hqForm.Repaint();
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.rcQuote.Contains(e.X, e.Y))
        this.draw_Quote.MouseDoubleClick(e.X, e.Y, this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo), this.m_hqForm, this.m_hqClient);
      if (!this.m_globalData.isClickMultiCycle || !this.rcKLine.Contains(e.X, e.Y) || this.draw_KLine == null)
        return;
      this.m_hqForm.IsMultiCycle = !this.m_hqForm.IsMultiCycle;
      if (!this.m_hqForm.IsMultiCycle)
      {
        if (this.m_globalData.SelectNum != 1)
        {
          this.m_hqClient.m_iKLineCycle = this.listRect[this.m_globalData.SelectNum - 1].Cycle;
        }
        else
        {
          this.m_hqForm.MainGraph = (Page_Main) new Page_MinLine(this.m_rc, this.m_hqForm);
          this.m_hqForm.IsMultiCommidity = false;
        }
      }
      this.m_hqForm.Repaint();
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.rcKLine.Contains(e.X, e.Y) && this.draw_KLine != null)
      {
        if (this.m_hqForm.M_Cursor == Cursors.Hand)
          this.m_hqForm.M_Cursor = Cursors.Default;
        bool flag = false;
        if (!this.m_hqForm.IsMultiCycle)
        {
          if (this.isDrawCursor)
            flag = this.draw_KLine.MouseDragged(e.X, e.Y);
        }
        else if (this.isDrawCursor)
        {
          for (int index = 0; index < this.listRect.Count; ++index)
          {
            if (this.listRect[index].OwnRect.Contains(e.X, e.Y))
            {
              if (index != 0)
                this.m_globalData.newPos = this.listRect[index].OwnDrawKLine.getClickPointX(e.X, e.Y);
              if (this.m_globalData.SelectNum != index + 1)
              {
                this.m_globalData.SelectNum = index + 1;
                this.m_hqForm.Repaint();
              }
              if (index != 0)
              {
                this.m_hqClient.m_iKLineCycle = this.listRect[index].Cycle;
                flag = this.listRect[index].OwnDrawKLine.MouseDragged(e.X, e.Y);
                break;
              }
              this.minX = e.X;
              this.minY = e.Y;
              this.m_hqClient.m_iKLineCycle = this.listRect[index].Cycle;
              flag = this.listRect[index].OwnMinKLine.MouseDragged(e.X, e.Y);
              break;
            }
          }
        }
        if (!flag)
          return;
        this.m_hqForm.Repaint();
      }
      else if (this.rcQuote.Contains(e.X, e.Y))
      {
        this.draw_Quote.MouseMove(e.X, e.Y, this.m_hqForm);
      }
      else
      {
        if (!(this.m_hqForm.M_Cursor == Cursors.Hand))
          return;
        this.m_hqForm.M_Cursor = Cursors.Default;
      }
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        bool flag = false;
        switch (e.KeyData)
        {
          case Keys.Prior:
            this.m_hqForm.ChangeStock(true);
            flag = true;
            break;
          case Keys.Next:
            this.m_hqForm.ChangeStock(false);
            flag = true;
            break;
          case Keys.F10:
            this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
            break;
          default:
            if (this.m_hqForm.IsMultiCycle)
            {
              if (this.m_globalData.SelectNum != 1)
              {
                flag = this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.KeyPressed(e);
                this.m_hqClient.m_iKLineCycle = this.listRect[this.m_globalData.SelectNum - 1].Cycle;
                this.m_globalData.newPos = this.listRect[this.m_globalData.SelectNum - 1].OwnDrawKLine.m_iPos;
                break;
              }
              break;
            }
            flag = this.draw_KLine.KeyPressed(e);
            break;
        }
        this.m_hqForm.IsNeedRepaint = flag;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Kline_Page_KeyDown异常：" + ex.Message);
      }
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_LineType"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KType"));
      toolStripMenuItem1.Name = "menuKType";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_KLine"));
      toolStripMenuItem2.Name = "KLine";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_USALine"));
      toolStripMenuItem3.Name = "USA";
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_PolyLine"));
      toolStripMenuItem4.Name = "Poly";
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Indicator"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Indicator"));
      toolStripMenuItem5.Name = "menuIndicator";
      ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_AnalysisCycle") + "  F8", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Cycle"));
      toolStripMenuItem6.Name = "menuCycle";
      ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_multiCycle"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Cycle"));
      toolStripMenuItem7.Name = "multiCycle";
      ToolStripMenuItem toolStripMenuItem8 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
      toolStripMenuItem8.Name = "cmd_60";
      ToolStripMenuItem toolStripMenuItem9 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
      toolStripMenuItem9.Name = "cmd_80";
      this.menuZoomIn = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ZooMIn"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_ZoomIn"));
      this.menuZoomIn.Name = "zoomin";
      this.menuZoomOut = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ZoomOut"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_ZoomOut"));
      this.menuZoomOut.Name = "zoomout";
      ToolStripMenuItem toolStripMenuItem10 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_PrevCommodity") + "  PageUp", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_PrevStock"));
      toolStripMenuItem10.Name = "prevstock";
      ToolStripMenuItem toolStripMenuItem11 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_NextCommodity") + "  PageDown", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_PostStock"));
      toolStripMenuItem11.Name = "poststock";
      ToolStripMenuItem toolStripMenuItem12 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
      toolStripMenuItem12.Name = "minline";
      ToolStripMenuItem toolStripMenuItem13 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_TradeList") + "  F1", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Bill"));
      toolStripMenuItem13.Name = "bill";
      ToolStripMenuItem toolStripMenuItem14 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_DayLine"));
      toolStripMenuItem14.Name = "day";
      ToolStripMenuItem toolStripMenuItem15 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_WeekLine"));
      toolStripMenuItem15.Name = "week";
      ToolStripMenuItem toolStripMenuItem16 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MonthLine"));
      toolStripMenuItem16.Name = "month";
      ToolStripMenuItem toolStripMenuItem17 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__5MinLine"));
      toolStripMenuItem17.Name = "min5";
      ToolStripMenuItem toolStripMenuItem18 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__15MinLine"));
      toolStripMenuItem18.Name = "min15";
      ToolStripMenuItem toolStripMenuItem19 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__30MinLine"));
      toolStripMenuItem19.Name = "min30";
      ToolStripMenuItem toolStripMenuItem20 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__60MinLine"));
      toolStripMenuItem20.Name = "min60";
      ToolStripMenuItem toolStripMenuItem21 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_QuarterLine"));
      toolStripMenuItem21.Name = "quarter";
      ToolStripMenuItem toolStripMenuItem22 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__1MinLine"));
      toolStripMenuItem22.Name = "min1";
      ToolStripMenuItem toolStripMenuItem23 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__3MinLine"));
      toolStripMenuItem23.Name = "min3";
      ToolStripMenuItem toolStripMenuItem24 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__2HrLine"));
      toolStripMenuItem24.Name = "min120";
      ToolStripMenuItem toolStripMenuItem25 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr__4HrLine"));
      toolStripMenuItem25.Name = "min240";
      ToolStripMenuItem toolStripMenuItem26 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_oneCycle"));
      toolStripMenuItem26.Name = "oneCycle";
      ToolStripMenuItem toolStripMenuItem27 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_twoCycle"));
      toolStripMenuItem27.Name = "twoCycle";
      ToolStripMenuItem toolStripMenuItem28 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_fourCycle"));
      toolStripMenuItem28.Name = "fourCycle";
      ToolStripMenuItem toolStripMenuItem29 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_sixCycle"));
      toolStripMenuItem29.Name = "sixCycle";
      ToolStripMenuItem toolStripMenuItem30 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_nineCycle"));
      toolStripMenuItem30.Name = "nineCycle";
      ToolStripMenuItem toolStripMenuItem31 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_CommodityInfo") + "  F10");
      toolStripMenuItem31.Name = "commodityInfo";
      ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem14);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem15);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem16);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem21);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem22);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem23);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem17);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem18);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem19);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem20);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem24);
      contextMenuStrip1.Items.Add((ToolStripItem) toolStripMenuItem25);
      contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem6.DropDown = (ToolStripDropDown) contextMenuStrip1;
      ContextMenuStrip contextMenuStrip2 = new ContextMenuStrip();
      contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem26);
      contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem27);
      contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem28);
      contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem29);
      contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem30);
      contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem7.DropDown = (ToolStripDropDown) contextMenuStrip2;
      int length = IndicatorBase.INDICATOR_NAME.GetLength(0);
      ContextMenuStrip contextMenuStrip3 = new ContextMenuStrip();
      for (int index = 0; index < length; ++index)
      {
        string str = IndicatorBase.INDICATOR_NAME[index, 0];
        string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_T_" + IndicatorBase.INDICATOR_NAME[index, 0]);
        ToolStripMenuItem toolStripMenuItem32 = new ToolStripMenuItem(str + "  " + @string);
        toolStripMenuItem32.Name = "Indicator_" + str;
        contextMenuStrip3.Items.Add((ToolStripItem) toolStripMenuItem32);
      }
      contextMenuStrip3.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem5.DropDown = (ToolStripDropDown) contextMenuStrip3;
      ContextMenuStrip contextMenuStrip4 = new ContextMenuStrip();
      contextMenuStrip4.Items.Add((ToolStripItem) toolStripMenuItem2);
      contextMenuStrip4.Items.Add((ToolStripItem) toolStripMenuItem3);
      contextMenuStrip4.Items.Add((ToolStripItem) toolStripMenuItem4);
      contextMenuStrip4.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem1.DropDown = (ToolStripDropDown) contextMenuStrip4;
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem5);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem6);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem7);
      this.contextMenuStrip.Items.Add("-");
      if (this.iProductType != 4)
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem12);
      if (this.iProductType != 2 && this.iProductType != 3 && this.iProductType != 4)
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem13);
      if (this.iProductType != 4)
        this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) this.menuZoomIn);
      this.contextMenuStrip.Items.Add((ToolStripItem) this.menuZoomOut);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem10);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem11);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem8);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem9);
      if (this.m_hqForm.isDisplayF10Menu())
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem31);
      this.AddCommonMenu();
    }

    protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        CommodityInfoF.CommodityInfoClose();
        string name = e.ClickedItem.Name;
        if (name.IndexOf("cmd_") >= 0)
          this.m_hqForm.UserCommand(name.Substring(4));
        if ("USA".Equals(name))
        {
          this.m_globalData.m_iCurKLineType = 1;
          this.draw_KLine.ChangeKLineType(1);
        }
        else if ("KLine".Equals(name))
        {
          this.m_globalData.m_iCurKLineType = 0;
          this.draw_KLine.ChangeKLineType(0);
        }
        else if ("Poly".Equals(name))
        {
          this.m_globalData.m_iCurKLineType = 2;
          this.draw_KLine.ChangeKLineType(2);
        }
        else if (name.Equals("zoomin"))
          this.zoomInKLineGraph();
        else if (name.Equals("zoomout"))
          this.zoomOutKLineGraph();
        else if (name.Equals("prevstock"))
          this.m_hqForm.ChangeStock(true);
        else if (name.Equals("poststock"))
          this.m_hqForm.ChangeStock(false);
        else if (name.Equals("day"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 1;
          this.AskForKLine();
        }
        else if (name.Equals("week"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 2;
          this.AskForKLine();
        }
        else if (name.Equals("month"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 3;
          this.AskForKLine();
        }
        else if (name.Equals("min5"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 7;
          this.AskForKLine();
        }
        else if (name.Equals("min15"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 8;
          this.m_hqClient.KLineValue = 15;
          this.AskForKLine();
        }
        else if (name.Equals("min30"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 9;
          this.m_hqClient.KLineValue = 30;
          this.AskForKLine();
        }
        else if (name.Equals("min60"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 10;
          this.m_hqClient.KLineValue = 60;
          this.AskForKLine();
        }
        else if (name.Equals("min1"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 5;
          this.m_hqClient.KLineValue = 1;
          this.AskForKLine();
        }
        else if (name.Equals("min3"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 6;
          this.m_hqClient.KLineValue = 3;
          this.AskForKLine();
        }
        else if (name.Equals("min120"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 11;
          this.m_hqClient.KLineValue = 120;
          this.AskForKLine();
        }
        else if (name.Equals("min240"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 12;
          this.m_hqClient.KLineValue = 240;
          this.AskForKLine();
        }
        else if (name.Equals("quarter"))
        {
          this.m_hqForm.IsMultiCycle = false;
          this.m_hqClient.m_iKLineCycle = 4;
          this.AskForKLine();
        }
        else if (name.Equals("minline"))
          this.m_hqForm.ShowPageMinLine();
        else if (name.Equals("bill"))
          this.m_hqForm.UserCommand("01");
        else if (name.StartsWith("Indicator_"))
        {
          this.m_hqClient.m_strIndicator = name.Substring(10);
          this.draw_KLine.CreateIndicator();
        }
        else if (name.Equals("commodityInfo"))
          this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
        else if (name.Equals("oneCycle"))
        {
          this.m_globalData.isClickMultiCycle = false;
          this.m_hqForm.IsMultiCycle = false;
          this.m_globalData.SelectNum = 1;
        }
        else if (name.Equals("twoCycle"))
        {
          this.m_globalData.isClickMultiCycle = true;
          this.m_hqForm.IsMultiCycle = true;
          this.m_globalData.SelectNum = 1;
          this.m_globalData.rectCol = 1;
          this.m_globalData.rectRol = 2;
        }
        else if (name.Equals("fourCycle"))
        {
          this.m_globalData.isClickMultiCycle = true;
          this.m_hqForm.IsMultiCycle = true;
          this.m_globalData.SelectNum = 1;
          this.m_globalData.rectCol = 2;
          this.m_globalData.rectRol = 2;
        }
        else if (name.Equals("sixCycle"))
        {
          this.m_globalData.isClickMultiCycle = true;
          this.m_hqForm.IsMultiCycle = true;
          this.m_globalData.SelectNum = 1;
          this.m_globalData.rectCol = 2;
          this.m_globalData.rectRol = 3;
        }
        else if (name.Equals("nineCycle"))
        {
          this.m_globalData.isClickMultiCycle = true;
          this.m_hqForm.IsMultiCycle = true;
          this.m_globalData.SelectNum = 1;
          this.m_globalData.rectCol = 3;
          this.m_globalData.rectRol = 3;
        }
        else
          this.m_hqForm.UserCommand(name);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Kline-contextMenu_ItemClicked异常：" + ex.Message);
      }
    }

    private void KLineUpDown(int iconFlag)
    {
      if (iconFlag == 1)
      {
        this.zoomInKLineGraph();
      }
      else
      {
        if (iconFlag != 2)
          return;
        this.zoomOutKLineGraph();
      }
    }

    public void setMenuEnable(string label, bool b)
    {
      if (label.Equals("zoomout"))
      {
        this.menuZoomOut.Enabled = b;
      }
      else
      {
        if (!label.Equals("zoomin"))
          return;
        this.menuZoomIn.Enabled = b;
      }
    }

    private void zoomInKLineGraph()
    {
      if (!this.draw_KLine.ChangeRatio(true))
        return;
      if (!this.draw_KLine.ChangeRatio(true))
        this.menuZoomIn.Enabled = false;
      else
        this.draw_KLine.ChangeRatio(false);
      if (this.menuZoomOut.Enabled)
        return;
      this.menuZoomOut.Enabled = true;
    }

    private void zoomOutKLineGraph()
    {
      if (!this.draw_KLine.ChangeRatio(false))
        return;
      if (!this.draw_KLine.ChangeRatio(false))
        this.menuZoomOut.Enabled = false;
      else
        this.draw_KLine.ChangeRatio(true);
      if (this.menuZoomIn.Enabled)
        return;
      this.menuZoomIn.Enabled = true;
    }

    public override void Dispose()
    {
      GC.Collect();
    }
  }
}
