// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_MultiQuote
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_MultiQuote : Page_Main
  {
    private Font fontTitle = new Font("宋体", 12f, FontStyle.Regular);
    private Font font = new Font("宋体", 12f, FontStyle.Regular);
    private string strSortItem = "Code";
    private bool bCanMove = true;
    private int iEnd = 30;
    private int xOnceMove = 3;
    public int xScrollWidth = 100;
    private int scrollButtonSize = 15;
    private int xScrollBarWidth = 50;
    private double yOnceMove = 10.0;
    private Color colorBlack = Color.FromArgb(80, 80, 80);
    private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
    private int dataGap = 20;
    private int needShowCol = 4;
    private int lbLocationY = 35;
    private int lbHeight = 25;
    private const int TITLE_GAP = 2;
    private int iStockRows;
    private int iStockCols;
    private int fontHeight;
    private byte sortBy;
    private byte isDescend;
    private Rectangle rcButton;
    private Rectangle rcData;
    private Page_Button buttonGraph;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ButtonUtils buttonUtils;
    public string[] m_strItems;
    private int iDynamicIndex;
    private int m_iStaticIndex;
    private Rectangle xScrollRect;
    private Rectangle xScrollBarRect;
    private int xChange;
    private int xMaxChaneg;
    private Rectangle yScrollRect;
    private Rectangle yScrollBarRect;
    private int yScrollBarHeight;
    private int yMaxChaneg;
    private int i;
    private bool m_bShowSortTag;
    private int scrolXOrY;
    private ToolStripMenuItem DelUserCommodity;
    private ToolStripMenuItem AddUserCommodity;

    public Page_MultiQuote(Rectangle _rc, HQForm hqForm)
      : base(_rc, hqForm)
    {
      try
      {
        this.pluginInfo = this.m_pluginInfo;
        this.setInfo = this.m_setInfo;
        this.buttonUtils = hqForm.CurHQClient.buttonUtils;
        if (Tools.StrToBool((string) this.m_pluginInfo.HTConfig[(object) "FontB"], false))
          this.font = this.fontTitle;
        this.fontHeight = this.font.Height;
        this.AskForQuoteList();
        this.m_hqClient.CurrentPage = 0;
        this.initStockFieldInfo();
        this.buttonGraph = new Page_Button(this.rcButton, this.m_hqForm, this.buttonUtils);
        this.MakeMenus();
        new Thread(new ThreadStart(this.checkCommCurprice)).Start();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote异常：" + ex.Message);
      }
    }

    private void setSortStockRequestPacket()
    {
      CMDSortVO cmdSortVo = new CMDSortVO();
      cmdSortVo.isDescend = this.isDescend;
      cmdSortVo.sortBy = this.sortBy;
      if (this.m_multiQuoteData.iStart == this.iEnd)
        this.iEnd = this.m_multiQuoteData.iStart + 1;
      cmdSortVo.start = this.m_multiQuoteData.iStart;
      cmdSortVo.end = this.iEnd;
      Logger.wirte(MsgType.Information, string.Concat(new object[4]
      {
        (object) "取报价排名Start = ",
        (object) this.m_multiQuoteData.iStart,
        (object) "  End = ",
        (object) this.iEnd
      }));
      this.m_hqClient.sendThread.AskForData((CMDVO) cmdSortVo);
    }

    protected override void AskForDataOnTimer()
    {
      int num = this.m_hqClient.m_bShowIndexAtBottom;
    }

    private void AskForQuoteList()
    {
    }

    public void TransferCommodityInfo()
    {
      try
      {
        if (this.m_multiQuoteData.iHighlightIndex <= 0 || this.m_multiQuoteData.iHighlightIndex > this.m_multiQuoteData.m_curQuoteList.Length)
          return;
        this.m_hqForm.MultiQuoteMouseLeftClick((object) this, new InterFace.CommodityInfoEventArgs((ProductDataVO) this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].Clone()));
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace);
      }
    }

    private void checkCommCurprice()
    {
      while (!this.stopFlag)
      {
        Thread.Sleep(1000);
        if (this.m_hqClient.CurrentPage != 0)
          break;
        lock (this.m_multiQuoteData.m_curQuoteList)
        {
          if (this.m_multiQuoteData.m_curQuoteList.Length != 0)
          {
            int local_0 = this.iStockRows;
            if (this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart < this.iStockRows)
              local_0 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
            for (int local_1 = 0; local_1 < local_0; ++local_1)
            {
              int local_2 = this.m_multiQuoteData.iStart + local_1;
              if (local_2 >= this.m_multiQuoteData.m_curQuoteList.Length)
                local_2 = this.m_multiQuoteData.m_curQuoteList.Length - 1;
              ProductDataVO local_3 = this.m_multiQuoteData.m_curQuoteList[local_2];
              if (local_3.datahighlightTime == 0 && local_3.isDraw)
              {
                GDIDraw.XorRectangle(this.m_hqForm.M_Graphics, local_3.curPriceRectangle, SetInfo.RHColor.clPriceChange, this.m_hqForm.ScrollOffset);
                local_3.isDraw = false;
              }
              else if (local_3.datahighlightTime == this.m_multiQuoteData.HighlightTime && !local_3.isDraw)
              {
                GDIDraw.XorRectangle(this.m_hqForm.M_Graphics, local_3.curPriceRectangle, SetInfo.RHColor.clPriceChange, this.m_hqForm.ScrollOffset);
                local_3.isDraw = true;
              }
              if (local_3.datahighlightTime > 0)
                --local_3.datahighlightTime;
            }
          }
        }
      }
    }

    private void initStockFieldInfo()
    {
      string str1 = this.setInfo.MultiQuoteName;
      char[] chArray1 = new char[1]
      {
        ';'
      };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]
        {
          ':'
        };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 2 && strArray[1].Length > 0)
        {
          MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) strArray[0]];
          if (multiQuoteItemInfo != null)
            multiQuoteItemInfo.name = strArray[1];
        }
      }
      if (!Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
      {
        this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Industry;", "");
        this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Region;", "");
        this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("MarketName;", "");
      }
      this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Unit;", "");
      string str3 = this.setInfo.MultiQuoteItems;
      if (str3.Length == 0)
        str3 = this.m_hqClient.strAllItemName;
      this.m_strItems = str3.Split(';');
      this.m_iStaticIndex = this.setInfo.MultiQuoteStaticIndex;
      this.iDynamicIndex = this.m_iStaticIndex + 1;
    }

    private void calculateRowsOfPage()
    {
      this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
      this.iStockRows = (this.rcData.Height - this.fontTitle.Height) / (this.fontHeight + 2);
      if (this.iStockRows < 1)
        this.iStockRows = 25;
      this.iEnd = this.m_multiQuoteData.iStart + this.iStockRows - 1;
      if (this.m_multiQuoteData.iHighlightIndex <= this.iStockRows)
        return;
      this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
    }

    private void calculateColsOfPage()
    {
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < this.m_strItems.Length; ++index)
      {
        MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[index]];
        if ((index <= this.m_iStaticIndex || index >= this.iDynamicIndex) && multiQuoteItemInfo != null)
        {
          num2 += multiQuoteItemInfo.width;
          if (this.rcData.Width > num2)
            ++num1;
          else
            break;
        }
      }
      this.iStockCols = num1 - 1;
    }

    private void paintXScroll(Graphics g)
    {
      this.xMaxChaneg = this.xOnceMove * (this.m_strItems.Length - this.needShowCol - 1);
      this.xScrollWidth = this.xOnceMove * (this.m_strItems.Length - this.needShowCol - 1) + this.xScrollBarWidth + 2 * this.scrollButtonSize;
      this.xScrollRect = new Rectangle(this.rcButton.X + this.rcButton.Width - this.xScrollWidth, this.rcButton.Y - 1, this.xScrollWidth - 1, this.scrollButtonSize);
      this.xScrollBarRect = new Rectangle(this.xScrollRect.X + this.scrollButtonSize + this.xChange, this.xScrollRect.Y, this.xScrollBarWidth, this.xScrollRect.Height);
      g.FillRectangle((Brush) this.m_Brush, this.xScrollRect);
      g.FillRectangle((Brush) new SolidBrush(this.colorBlack), this.xScrollBarRect);
    }

    private void paintYScroll(Graphics g)
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length <= this.iStockRows)
      {
        Rectangle rectangle = new Rectangle();
        this.yScrollBarRect = rectangle;
        this.yScrollRect = this.yScrollBarRect = rectangle;
      }
      else
      {
        int num = (int) (this.yOnceMove * (double) this.m_multiQuoteData.iStart);
        if (this.m_multiQuoteData.yChange > num + 20 || this.m_multiQuoteData.yChange < num - 20)
          this.m_multiQuoteData.yChange = num;
        this.yScrollRect = new Rectangle(this.rcData.X + this.rcData.Width, this.rcData.Y + this.fontHeight + 1, this.scrollButtonSize - 1, this.rcData.Height - this.fontHeight - 3);
        this.yScrollBarHeight = (this.yScrollRect.Height - 2 * this.scrollButtonSize) * this.iStockRows / this.m_multiQuoteData.m_curQuoteList.Length;
        this.yScrollBarRect = new Rectangle(this.yScrollRect.X, this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange, this.scrollButtonSize, this.yScrollBarHeight);
        this.yOnceMove = (double) Convert.ToSingle(this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarRect.Height) / (double) (this.m_multiQuoteData.m_curQuoteList.Length - this.iStockRows);
        this.yMaxChaneg = this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarRect.Height;
        if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
        {
          this.m_multiQuoteData.yChange = this.yMaxChaneg;
          this.yScrollBarRect = new Rectangle(this.yScrollRect.X, this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange, this.scrollButtonSize, this.yScrollBarHeight);
        }
        g.FillRectangle((Brush) this.m_Brush, this.yScrollRect);
        g.FillRectangle((Brush) new SolidBrush(this.colorBlack), this.yScrollBarRect);
      }
    }

    private void rePaintXScrollBar()
    {
      if (!this.m_hqForm.IsEndPaint)
        return;
      if (this.xChange < 0)
        this.xChange = 0;
      if (this.xChange > this.xMaxChaneg)
        this.xChange = this.xMaxChaneg;
      this.xScrollBarRect.X = this.xScrollRect.X + this.scrollButtonSize + this.xChange;
      using (Graphics mGraphics = this.m_hqForm.M_Graphics)
      {
        this.m_hqForm.TranslateTransform(mGraphics);
        using (Bitmap bitmap = new Bitmap(this.xScrollRect.Width, this.xScrollRect.Height))
        {
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            graphics.FillRectangle((Brush) new SolidBrush(this.colorBlack), this.xScrollBarRect);
            mGraphics.DrawImage((Image) bitmap, this.xScrollRect.X, this.xScrollRect.Y);
          }
        }
      }
    }

    private void rePaintYScrollBar()
    {
      if (!this.m_hqForm.IsEndPaint)
        return;
      if (this.m_multiQuoteData.yChange < 0)
        this.m_multiQuoteData.yChange = 0;
      if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
        this.m_multiQuoteData.yChange = this.yMaxChaneg;
      this.yScrollBarRect.Y = this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange;
      using (Graphics mGraphics = this.m_hqForm.M_Graphics)
      {
        this.m_hqForm.TranslateTransform(mGraphics);
        using (Bitmap bitmap = new Bitmap(this.yScrollRect.Width, this.yScrollRect.Height))
        {
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            graphics.FillRectangle((Brush) new SolidBrush(this.colorBlack), this.yScrollBarRect);
            mGraphics.DrawImage((Image) bitmap, this.yScrollRect.X, this.yScrollRect.Y);
          }
        }
      }
    }

    private void paintScrollBack(Graphics g)
    {
      this.m_Brush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
      if (this.xScrollRect.Location.X + this.xScrollRect.Width != this.m_rc.Width)
        this.paintXScroll(g);
      g.DrawString("<", this.font, (Brush) new SolidBrush(Color.Yellow), (PointF) this.xScrollRect.Location);
      g.DrawString(">", this.font, (Brush) new SolidBrush(Color.Yellow), (PointF) new Point(this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize, this.xScrollRect.Y));
      if (this.m_multiQuoteData.m_curQuoteList.Length <= this.iStockRows)
        return;
      Font font = new Font("宋体", 8f, FontStyle.Regular);
      this.rcData.Width = this.rcData.Width - this.scrollButtonSize;
      this.paintYScroll(g);
      g.DrawString("∧", font, (Brush) new SolidBrush(Color.Yellow), (PointF) this.yScrollRect.Location);
      g.DrawString("∨", font, (Brush) new SolidBrush(Color.Yellow), (PointF) new Point(this.yScrollRect.X, this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize));
    }

    public override void Paint(Graphics g, int v)
    {
      try
      {
        this.rcData = this.m_rc;
        this.rcButton = this.m_rc;
        this.rcData.Height -= this.m_multiQuoteData.buttonHight;
        this.rcButton.Y = this.rcData.Y + this.rcData.Height;
        this.rcButton.Height = this.m_multiQuoteData.buttonHight;
        this.buttonGraph.rc = this.rcButton;
        ++this.i;
        if (this.buttonUtils.ButtonList != null && this.buttonUtils.ButtonList.Count > 0)
          this.buttonGraph.Paint(g, this.buttonUtils.ButtonList, true);
        this.calculateRowsOfPage();
        this.calculateColsOfPage();
        this.paintTitleItems(g);
        this.paintQuoteData(g);
        this.paintGrid(g);
        this.paintScrollBack(g);
        this.m_hqForm.EndPaint();
        this.paintHighlightBar();
        this.paintXScroll(g);
        this.paintYScroll(g);
        this.bCanMove = true;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    private void paintGrid(Graphics g)
    {
      Pen pen = new Pen((Brush) new SolidBrush(SetInfo.RHColor.clGrid), 1f);
      g.DrawRectangle(pen, this.m_rc.X, this.m_rc.Y, this.rcData.Width - 1, this.m_rc.Height - 1);
      g.DrawLine(pen, this.rcData.X, this.rcData.Y + this.fontTitle.Height, this.rcData.X + this.rcData.Width, this.rcData.Y + this.fontTitle.Height);
    }

    private void paintTitleItems(Graphics g)
    {
      int x = this.rcData.X;
      int num1 = this.rcData.Y + 1;
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
      g.FillRectangle((Brush) solidBrush, new Rectangle(this.rcData.X, this.rcData.Y, this.rcData.Width, this.fontTitle.Height));
      for (int index = 0; index < this.m_strItems.Length; ++index)
      {
        if (index <= this.m_iStaticIndex || index >= this.iDynamicIndex)
        {
          MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[index]];
          if (multiQuoteItemInfo != null)
          {
            if (index > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1) && index != this.m_strItems.Length)
            {
              g.DrawString(">>", this.font, (Brush) solidBrush, (float) x, (float) num1);
              break;
            }
            string str = multiQuoteItemInfo.name;
            if (this.m_bShowSortTag && this.strSortItem.Equals(this.m_strItems[index]))
            {
              if ((int) this.isDescend == 1)
              {
                solidBrush.Color = SetInfo.RHColor.clIncrease;
                str += "↓";
              }
              else
              {
                solidBrush.Color = SetInfo.RHColor.clDecrease;
                str += "↑";
              }
            }
            else
              solidBrush.Color = SetInfo.RHColor.clItem;
            int num2 = (int) g.MeasureString(str, this.fontTitle).Width;
            if (multiQuoteItemInfo.width < num2)
              multiQuoteItemInfo.width = num2 + 10;
            x += multiQuoteItemInfo.width;
            g.DrawString(str, this.fontTitle, (Brush) solidBrush, (float) (x - num2), (float) num1);
            if (x > this.rcData.X + this.rcData.Width)
              break;
          }
        }
      }
      solidBrush.Dispose();
    }

    public bool zoom(string strText, Graphics g, MultiQuoteItemInfo info, int y)
    {
      int num = (int) g.MeasureString(strText, this.font).Width;
      if (info.width - num >= this.dataGap)
        return false;
      info.width += num - info.width + this.dataGap;
      this.m_Brush.Color = SetInfo.RHColor.clBackGround;
      g.FillRectangle((Brush) this.m_Brush, this.rcData.X, this.rcData.Y, this.rcData.Width, y - this.rcData.Y);
      this.paintTitleItems(g);
      this.paintQuoteData(g);
      return true;
    }

    private void paintQuoteData(Graphics g)
    {
      lock (this.m_multiQuoteData.m_curQuoteList)
      {
        if (this.buttonUtils.CurButtonName == "AllMarket")
        {
          ArrayList local_0 = new ArrayList();
          for (int local_1 = 0; local_1 < this.m_hqClient.m_quoteList.Length; ++local_1)
          {
            ProductDataVO local_2 = this.m_hqClient.m_quoteList[local_1];
            local_0.Add((object) local_2);
          }
          this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_0.ToArray(typeof (ProductDataVO));
          this.m_multiQuoteData.MultiQuotePage = 0;
        }
        else if (this.buttonUtils.CurButtonName.StartsWith("Market"))
        {
          string local_3 = this.buttonUtils.CurButtonName.Substring(6);
          ArrayList local_4 = new ArrayList();
          for (int local_5 = 0; local_5 < this.m_hqClient.m_quoteList.Length; ++local_5)
          {
            if (this.m_hqClient.m_quoteList[local_5].marketID.Equals(local_3))
            {
              ProductDataVO local_6 = this.m_hqClient.m_quoteList[local_5];
              local_4.Add((object) local_6);
            }
          }
          this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_4.ToArray(typeof (ProductDataVO));
          this.m_multiQuoteData.MultiQuotePage = 0;
        }
        else if (this.buttonUtils.CurButtonName.Equals("MyCommodity"))
        {
          ArrayList local_7 = new ArrayList();
          this.m_multiQuoteData.MyCommodityList.Clear();
          ArrayList local_8 = this.m_hqClient.myCommodity.GetMyCommodity();
          for (int local_9 = 0; local_9 < local_8.Count; ++local_9)
          {
            CommodityInfo local_10 = CommodityInfo.DealCode(local_8[local_9].ToString());
            for (int local_11 = 0; local_11 < this.m_hqClient.m_quoteList.Length; ++local_11)
            {
              if (this.m_hqClient.m_quoteList[local_11].marketID.Equals(local_10.marketID) && this.m_hqClient.m_quoteList[local_11].code.Equals(local_10.commodityCode))
              {
                ProductDataVO local_12 = this.m_hqClient.m_quoteList[local_11];
                local_7.Add((object) local_12);
                this.m_multiQuoteData.MyCommodityList.Add((object) local_8[local_9].ToString());
                break;
              }
            }
          }
          this.m_multiQuoteData.MultiQuotePage = 1;
          this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_7.ToArray(typeof (ProductDataVO));
        }
        else if (this.buttonUtils.CurButtonName.StartsWith("MAC"))
        {
          if (this.m_hqClient.commodityClass == null)
            return;
          string local_13 = this.buttonUtils.CurButtonName.Substring(3);
          string local_14 = string.Empty;
          string local_15 = string.Empty;
          int local_16 = local_13.IndexOf("_");
          string local_14_1;
          if (local_16 != -1)
          {
            local_14_1 = local_13.Substring(0, local_16);
            local_15 = local_13.Substring(local_16 + 1);
          }
          else
            local_14_1 = local_13;
          if (local_14_1 != null && local_14_1.Length > 0)
          {
            ArrayList local_17 = new ArrayList();
            for (int local_18 = 0; local_18 < this.m_hqClient.m_quoteList.Length; ++local_18)
            {
              if (this.m_hqClient.m_quoteList[local_18].marketID.Equals(local_14_1))
              {
                ProductDataVO local_19 = this.m_hqClient.m_quoteList[local_18];
                local_17.Add((object) local_19);
              }
            }
            this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_17.ToArray(typeof (ProductDataVO));
          }
          if (this.m_multiQuoteData.m_curQuoteList.Length > 0 && local_15 != null && local_15.Length > 0)
          {
            if (this.m_hqClient.commodityClass == null)
              return;
            ArrayList local_20 = (ArrayList) this.m_hqClient.commodityClass.htCommodityByClass[(object) local_15];
            if (local_20 == null)
              return;
            ArrayList local_21 = new ArrayList();
            for (int local_22 = 0; local_22 < this.m_multiQuoteData.m_curQuoteList.Length; ++local_22)
            {
              for (int local_23 = 0; local_23 < local_20.Count; ++local_23)
              {
                CommodityClassVO local_24 = (CommodityClassVO) local_20[local_23];
                if (this.m_multiQuoteData.m_curQuoteList[local_22].code.Equals(local_24.commodityID))
                {
                  local_21.Add((object) this.m_multiQuoteData.m_curQuoteList[local_22]);
                  break;
                }
              }
            }
            this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_21.ToArray(typeof (ProductDataVO));
          }
          this.m_multiQuoteData.MultiQuotePage = 0;
        }
        else if (this.buttonUtils.CurButtonName.StartsWith("C"))
        {
          string local_25 = this.buttonUtils.CurButtonName.Substring(1);
          if (local_25 != null && local_25.Length > 0)
          {
            if (this.m_hqClient.commodityClass == null)
              return;
            ArrayList local_26 = (ArrayList) this.m_hqClient.commodityClass.htCommodityByClass[(object) local_25];
            if (local_26 == null)
              return;
            ArrayList local_27 = new ArrayList();
            for (int local_28 = 0; local_28 < this.m_hqClient.m_quoteList.Length; ++local_28)
            {
              for (int local_29 = 0; local_29 < local_26.Count; ++local_29)
              {
                CommodityClassVO local_30 = (CommodityClassVO) local_26[local_29];
                if (this.m_hqClient.m_quoteList[local_28].marketID.Equals(local_30.market) && this.m_hqClient.m_quoteList[local_28].code.Equals(local_30.commodityID))
                {
                  local_27.Add((object) this.m_hqClient.m_quoteList[local_28]);
                  break;
                }
              }
            }
            this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_27.ToArray(typeof (ProductDataVO));
            this.m_multiQuoteData.MultiQuotePage = 0;
          }
        }
        else if (this.buttonUtils.CurButtonName.StartsWith("Select"))
        {
          ArrayList local_31 = new ArrayList();
          for (int local_32 = 0; local_32 < this.m_hqClient.m_quoteList.Length; ++local_32)
          {
            ProductDataVO local_33 = this.m_hqClient.m_quoteList[local_32];
            if (MainWindow.selectIndexHY != -1 && MainWindow.selectIndexDQ != -1)
            {
              if (MainWindow.hangYeStrings[MainWindow.selectIndexHY] == local_33.industry && MainWindow.diQuStrings[MainWindow.selectIndexDQ] == local_33.region)
                local_31.Add((object) local_33);
            }
            else if (MainWindow.selectIndexHY == -1 && MainWindow.selectIndexDQ == -1)
              local_31.Add((object) local_33);
            else if (MainWindow.selectIndexHY == -1)
            {
              if (MainWindow.diQuStrings[MainWindow.selectIndexDQ] == local_33.region)
                local_31.Add((object) local_33);
            }
            else if (MainWindow.selectIndexDQ == -1 && MainWindow.hangYeStrings[MainWindow.selectIndexHY] == local_33.industry)
              local_31.Add((object) local_33);
          }
          this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[]) local_31.ToArray(typeof (ProductDataVO));
        }
        if (this.m_multiQuoteData.m_curQuoteList.Length == 0)
          return;
        this.sortItems();
        int local_34 = 2;
        int local_35 = this.rcData.X;
        int local_36 = this.rcData.Y + this.fontHeight + 2;
        int local_37 = this.iStockRows;
        if (this.m_multiQuoteData.m_curQuoteList.Length < this.iStockRows)
          this.m_multiQuoteData.iStart = 0;
        if (this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart < this.iStockRows)
          local_37 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
        SolidBrush local_38 = new SolidBrush(SetInfo.RHColor.clNumber);
        for (int local_39 = 0; local_39 < local_37; ++local_39)
        {
          int local_40 = this.m_multiQuoteData.iStart + local_39;
          ProductDataVO local_41 = this.m_multiQuoteData.m_curQuoteList[local_40];
          if (local_41.code == null)
          {
            Logger.wirte(MsgType.Error, "Code = null");
          }
          else
          {
            CodeTable local_42 = (CodeTable) this.m_hqClient.m_htProduct[(object) (local_41.marketID + local_41.code)];
            string local_43 = "-";
            if (local_42 != null)
            {
              if (local_42.sName != null)
                local_43 = local_42.sName;
              else
                Logger.wirte(MsgType.Information, "stockTable.sName = null");
            }
            else
              Logger.wirte(MsgType.Information, " stockTable = null ");
            int local_45 = this.m_hqClient.GetPrecision(new CommodityInfo(local_41.marketID, local_41.code));
            float local_46 = local_41.yesterBalancePrice;
            int local_48 = 0;
            for (int local_49 = 0; local_49 < this.m_strItems.Length; ++local_49)
            {
              if (local_49 <= this.m_iStaticIndex || local_49 >= this.iDynamicIndex)
              {
                if (local_49 <= this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
                {
                  MultiQuoteItemInfo local_50 = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[local_49]];
                  if (local_50 != null)
                  {
                    local_35 += local_50.width;
                    SizeF local_60;
                    if (this.m_strItems[local_49].Equals("No"))
                    {
                      string local_47 = Convert.ToString(local_40 + 1);
                      local_38.Color = SetInfo.RHColor.clNumber;
                      local_60 = g.MeasureString(local_47, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_47, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("Name"))
                    {
                      string local_47_1 = local_43;
                      local_38.Color = SetInfo.RHColor.clProductName;
                      local_60 = g.MeasureString(local_47_1, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_47_1, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("Code"))
                    {
                      string local_47_2 = local_41.code;
                      local_38.Color = SetInfo.RHColor.clProductName;
                      local_60 = g.MeasureString(local_47_2, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_47_2, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("CurPrice"))
                    {
                      Rectangle local_51 = new Rectangle(local_35 - local_50.width, local_36, local_50.width, this.fontHeight);
                      local_41.curPriceRectangle = local_51;
                      local_41.isDraw = false;
                      if (this.zoom(local_41.curPrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.curPrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("Balance"))
                    {
                      if (this.zoom(local_41.balancePrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.balancePrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("UpValue"))
                    {
                      float local_52 = (double) local_46 == 0.0 || (double) local_41.curPrice == 0.0 ? 0.0f : local_41.curPrice - local_46;
                      if (this.zoom(local_52.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_52, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("UpRate"))
                    {
                      float local_53 = (double) local_46 <= 0.0 || (double) local_41.curPrice <= 0.0 ? 0.0f : (float) (((double) local_41.curPrice - (double) local_46) / (double) local_46 * 100.0);
                      if (this.zoom(local_53.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_53, "", this.m_strItems[local_49], 2, 0.0f, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("YesterBalance"))
                    {
                      if (this.zoom(local_41.yesterBalancePrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.yesterBalancePrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("OpenPrice"))
                    {
                      if (this.zoom(local_41.openPrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.openPrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("BuyPrice"))
                    {
                      if (this.zoom(local_41.buyPrice[0].ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.buyPrice[0], "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("SellPrice"))
                    {
                      if (this.zoom(local_41.sellPrice[0].ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.sellPrice[0], "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("BuyAmount"))
                    {
                      if (this.zoom(local_41.buyAmount[0].ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.buyAmount[0], "", this.m_strItems[local_49], 0, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("SellAmount"))
                    {
                      if (this.zoom(local_41.sellAmount[0].ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.sellAmount[0], "", this.m_strItems[local_49], 0, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("HighPrice"))
                    {
                      if (this.zoom(local_41.highPrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.highPrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("LowPrice"))
                    {
                      if (this.zoom(local_41.lowPrice.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.lowPrice, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("TotalAmount"))
                    {
                      if (this.zoom(local_41.totalAmount.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.totalAmount, "", this.m_strItems[local_49], 0, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("TotalMoney"))
                    {
                      if (this.zoom(local_41.totalMoney.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, local_41.totalMoney, "", this.m_strItems[local_49], local_45, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("ReverseCount"))
                    {
                      if (this.zoom(local_41.reserveCount.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.reserveCount, "", this.m_strItems[local_49], 0, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("CurAmount"))
                    {
                      if (this.zoom(local_41.curAmount.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.curAmount, "", this.m_strItems[local_49], 0, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("AmountRate"))
                    {
                      if (this.zoom(local_41.amountRate.ToString("0.00"), g, local_50, local_36))
                        return;
                      this.paintNumber(g, local_38, (double) local_41.amountRate, "", this.m_strItems[local_49], 2, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("ConsignRate"))
                    {
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      this.paintNumber(g, local_38, (double) local_41.consignRate, "", this.m_strItems[local_49], 2, local_46, local_35, local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("Region"))
                    {
                      local_38.Color = Color.White;
                      string local_54 = "—";
                      if (this.m_hqClient.m_htRegion.Count != 0 && local_41.region != null && this.m_hqClient.m_htRegion[(object) local_41.region] != null)
                        local_54 = this.m_hqClient.m_htRegion[(object) local_41.region].ToString();
                      else if (local_41.region != null)
                        local_54 = local_41.region;
                      local_60 = g.MeasureString(local_54, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_54, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("Industry"))
                    {
                      local_38.Color = Color.White;
                      string local_55 = "—";
                      if (this.m_hqClient.m_htIndustry.Count != 0 && local_41.industry != null && this.m_hqClient.m_htIndustry[(object) local_41.industry] != null)
                        local_55 = this.m_hqClient.m_htIndustry[(object) local_41.industry].ToString();
                      else if (local_41.industry != null)
                        local_55 = local_41.industry;
                      local_60 = g.MeasureString(local_55, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_55, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    else if (this.m_strItems[local_49].Equals("MarketName"))
                    {
                      string local_56 = "--";
                      if (this.m_hqClient.m_htMarketData != null)
                      {
                        foreach (DictionaryEntry item_0 in this.m_hqClient.m_htMarketData)
                        {
                          MarketDataVO local_58 = (MarketDataVO) item_0.Value;
                          if (local_58.marketID == local_41.marketID)
                            local_56 = local_58.marketName;
                        }
                      }
                      local_38.Color = SetInfo.RHColor.clProductName;
                      local_60 = g.MeasureString(local_56, this.font);
                      local_48 = (int) local_60.Width;
                      if (local_50.width - local_48 < this.dataGap)
                      {
                        local_50.width += local_48 - local_50.width + this.dataGap;
                        local_38.Color = SetInfo.RHColor.clBackGround;
                        g.FillRectangle((Brush) local_38, this.rcData.X, this.rcData.Y, this.rcData.Width, local_36 - this.rcData.Y);
                        this.paintTitleItems(g);
                        this.paintQuoteData(g);
                        return;
                      }
                      g.DrawString(local_56, this.font, (Brush) local_38, (float) (local_35 - local_48), (float) local_36);
                    }
                    if (local_35 > this.rcData.X + this.rcData.Width)
                      break;
                  }
                }
                else
                  break;
              }
            }
            local_35 = this.rcData.X;
            local_36 += this.fontHeight + local_34;
          }
        }
        local_38.Dispose();
      }
    }

    private void paintHighlightBar()
    {
      if (!this.m_hqForm.IsEndPaint)
        return;
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0 && this.m_multiQuoteData.iHighlightIndex > this.m_multiQuoteData.m_curQuoteList.Length)
      {
        this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length;
        if (this.m_multiQuoteData.iHighlightIndex < 0)
          this.m_multiQuoteData.iHighlightIndex = 1;
      }
      else if (this.m_multiQuoteData.m_curQuoteList.Length == 0)
        this.m_multiQuoteData.iHighlightIndex = 1;
      GDIDraw.XorRectangle(this.m_hqForm.M_Graphics, new Rectangle(this.rcData.X, this.rcData.Y + (this.m_multiQuoteData.iHighlightIndex - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
      try
      {
        if (this.m_multiQuoteData.m_curQuoteList.Length <= 0)
          return;
        this.m_hqClient.curCommodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].code);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.Message);
      }
    }

    private void repaintHighlightBar(int iNewPos)
    {
      if (!this.m_hqForm.IsEndPaint)
        return;
      Graphics mGraphics = this.m_hqForm.M_Graphics;
      int y1 = this.rcData.Y + (this.m_multiQuoteData.iHighlightIndex - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1;
      int y2 = this.rcData.Y + (iNewPos - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1;
      GDIDraw.XorRectangle(mGraphics, new Rectangle(this.rcData.X, y1, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
      GDIDraw.XorRectangle(mGraphics, new Rectangle(this.rcData.X, y2, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
      this.m_multiQuoteData.iHighlightIndex = iNewPos;
      try
      {
        if (this.m_multiQuoteData.m_curQuoteList.Length <= 0)
          return;
        this.m_hqClient.curCommodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].code);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.Message);
      }
    }

    private void paintNumber(Graphics g, SolidBrush m_Brush, double num, string strSuffix, string itemName, int iPrecision, float close, int x, int y)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (itemName.Equals("TotalAmount") || itemName.Equals("CurAmount") || (itemName.Equals("BuyAmount") || itemName.Equals("SellAmount")) || itemName.Equals("AmountRate"))
        m_Brush.Color = SetInfo.RHColor.clVolume;
      else if (itemName.Equals("ReverseCount"))
        m_Brush.Color = SetInfo.RHColor.clReserve;
      else if (itemName.Equals("TotalMoney"))
        m_Brush.Color = SetInfo.RHColor.clNumber;
      else if (itemName.Equals("ConsignRate"))
        m_Brush.Color = SetInfo.RHColor.clNumber;
      else if (itemName.Equals("YesterBalance"))
        m_Brush.Color = SetInfo.RHColor.clEqual;
      else if (itemName.Equals("UpValue"))
      {
        if (num > 0.0)
        {
          stringBuilder.Append("+");
          m_Brush.Color = SetInfo.RHColor.clIncrease;
        }
        else
          m_Brush.Color = num != 0.0 ? SetInfo.RHColor.clDecrease : SetInfo.RHColor.clEqual;
      }
      else
        m_Brush.Color = num <= (double) close ? (num == (double) close || num == 0.0 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
      if (itemName.Equals("UpRate"))
      {
        if (num == -100.0 || num == 0.0)
        {
          stringBuilder.Append("—");
        }
        else
        {
          stringBuilder.Append(M_Common.FloatToString(num, iPrecision));
          stringBuilder.Append("%");
        }
      }
      else if (num == 0.0)
        stringBuilder.Append("—");
      else
        stringBuilder.Append(M_Common.FloatToString(num, iPrecision));
      stringBuilder.Append(strSuffix);
      int num1 = (int) g.MeasureString(stringBuilder.ToString(), this.font).Width;
      double num2 = (double) g.MeasureString("代码", this.font).Width;
      g.DrawString(stringBuilder.ToString(), this.font, (Brush) m_Brush, (float) (x - num1), (float) y);
    }

    public void sortItems()
    {
      Arrays.sort(this.m_multiQuoteData.m_curQuoteList, this.strSortItem, (string) this.pluginInfo.HTConfig[(object) "SortRules00"]);
      if ((int) this.isDescend != 0)
        return;
      int length = this.m_multiQuoteData.m_curQuoteList.Length;
      int num = length / 2;
      for (int index = 0; index < num; ++index)
      {
        ProductDataVO productDataVo = this.m_multiQuoteData.m_curQuoteList[index];
        this.m_multiQuoteData.m_curQuoteList[index] = this.m_multiQuoteData.m_curQuoteList[length - index - 1];
        this.m_multiQuoteData.m_curQuoteList[length - index - 1] = productDataVo;
      }
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        int x1 = e.X - this.m_hqForm.ScrollOffset.X;
        int y = e.Y - this.m_hqForm.ScrollOffset.Y;
        if (e.Button == MouseButtons.Left)
        {
          bool flag = false;
          if (this.xScrollRect.Contains(x1, y) && !this.xScrollBarRect.Contains(x1, y))
          {
            if (x1 < this.xScrollRect.X + this.scrollButtonSize)
              flag = this.Key_LEFT_Pressed(1);
            else if (x1 > this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize)
            {
              flag = this.Key_RIGHT_Pressed(1);
            }
            else
            {
              int num = x1 - this.xScrollRect.X - this.scrollButtonSize;
              this.xChange = num >= this.xScrollBarWidth / 2 ? (num <= this.xScrollRect.Width - 2 * this.scrollButtonSize - this.xScrollBarWidth / 2 ? num - this.xScrollBarWidth / 2 : this.xMaxChaneg) : 0;
              flag = true;
              this.iDynamicIndex = this.xChange / this.xOnceMove + this.needShowCol;
            }
          }
          else if (this.yScrollRect.Contains(x1, y) && !this.yScrollBarRect.Contains(x1, y))
          {
            if (y < this.yScrollRect.Y + this.scrollButtonSize)
              flag = this.Key_UP_Pressed();
            else if (y > this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize)
            {
              flag = this.Key_DOWN_Pressed();
            }
            else
            {
              int num = y - this.yScrollRect.Y - this.scrollButtonSize;
              if (num < this.yScrollBarHeight / 2)
                this.m_multiQuoteData.yChange = 0;
              else if (num > this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarHeight / 2)
                this.m_multiQuoteData.yChange = this.yMaxChaneg;
              else
                this.m_multiQuoteData.yChange = num - this.yScrollBarHeight / 2;
              flag = true;
              this.m_multiQuoteData.iStart = (int) ((double) this.m_multiQuoteData.yChange / this.yOnceMove);
            }
          }
          if (flag)
            this.m_hqForm.Repaint();
          if (y > this.fontHeight && y < this.rcData.Y + this.rcData.Height)
          {
            this.selectProduct(x1, y);
            this.TransferCommodityInfo();
          }
          else if (y > this.rcData.Y + this.rcData.Height && y < this.rcButton.Y + this.rcButton.Height)
          {
            this.ClickButton(x1, y);
          }
          else
          {
            int x2 = this.rcData.X;
            for (int index = 0; index < this.m_strItems.Length; ++index)
            {
              if (index <= this.m_iStaticIndex || index >= this.iDynamicIndex)
              {
                MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[index]];
                if (multiQuoteItemInfo != null)
                {
                  if (index > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
                    return;
                  if (x1 > x2 && x1 < x2 + multiQuoteItemInfo.width)
                  {
                    this.changeSortField(this.m_strItems[index]);
                    break;
                  }
                  x2 += multiQuoteItemInfo.width;
                  if (x2 > this.rcData.X + this.rcData.Width)
                    break;
                }
              }
            }
          }
        }
        else if (e.Button == MouseButtons.Right)
        {
          if (y > this.rcData.Y + this.rcData.Height && y < this.rcButton.Y + this.rcButton.Height)
            this.ClickButtonRight(x1, y);
          if (y > this.fontHeight && y < this.rcData.Y + this.rcData.Height)
            this.selectProduct(x1, y);
        }
        ((HQClientForm) this.m_hqForm).mainWindow.Focus();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-Page_MouseClick异常：" + ex.Message);
      }
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      try
      {
        int num1 = e.X - this.m_hqForm.ScrollOffset.X;
        int num2 = e.Y - this.m_hqForm.ScrollOffset.Y;
        if (e.Button != MouseButtons.Left || this.m_multiQuoteData.m_curQuoteList.Length <= 0)
          return;
        int x = this.rcData.X;
        int num3 = this.rcData.Y + this.fontTitle.Height;
        int num4 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
        if (num4 > this.iStockRows)
          num4 = this.iStockRows;
        for (int index = 0; index < num4; ++index)
        {
          if (num1 > x && num1 < x + this.m_rc.Width - this.yScrollRect.Width && (num2 > num3 && num2 < num3 + this.fontHeight))
          {
            this.m_hqForm.QueryStock(new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + index].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + index].code));
            this.m_hqForm.Repaint();
            break;
          }
          num3 += this.fontHeight + 2;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-Page_MouseDoubleClick异常：" + ex.Message);
      }
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
      try
      {
        int num1 = e.X - this.m_hqForm.ScrollOffset.X;
        int num2 = e.Y - this.m_hqForm.ScrollOffset.Y;
        HQClientForm hqClientForm = (HQClientForm) this.m_hqForm;
        if (hqClientForm.m_isMouseLeftButtonDown)
        {
          if (this.xScrollBarRect.Contains(hqClientForm.m_mouseBeforeMove))
          {
            this.scrolXOrY = 1;
            if (this.xScrollBarRect.X + this.xScrollBarRect.Width > this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize)
              this.xChange = this.xMaxChaneg;
            else if (this.xScrollBarRect.X < this.xScrollRect.X + this.scrollButtonSize)
              this.xChange = 0;
            if (e.Location.X - hqClientForm.m_mouseBeforeMove.X != 0)
            {
              this.xChange += e.Location.X - hqClientForm.m_mouseBeforeMove.X;
              if (this.xChange < 0)
                this.xChange = 0;
              this.rePaintXScrollBar();
            }
            if ((Decimal) this.iDynamicIndex != Math.Ceiling((Decimal) this.xChange / (Decimal) this.xOnceMove + (Decimal) this.needShowCol))
            {
              this.iDynamicIndex = !(Math.Ceiling((Decimal) this.xChange / (Decimal) this.xOnceMove + (Decimal) this.needShowCol) <= (Decimal) (this.m_iStaticIndex + 1)) ? this.xChange / this.xOnceMove + this.needShowCol : this.m_iStaticIndex + 1;
              this.m_hqForm.Repaint();
            }
          }
          else if (this.yScrollBarRect.Contains(hqClientForm.m_mouseBeforeMove))
          {
            this.scrolXOrY = 2;
            if (this.yScrollBarRect.Y + this.yScrollBarRect.Height > this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize)
              this.m_multiQuoteData.yChange = this.yMaxChaneg;
            else if (this.yScrollBarRect.Y < this.yScrollRect.Y + this.scrollButtonSize)
              this.m_multiQuoteData.yChange = 0;
            if (e.Location.Y - hqClientForm.m_mouseBeforeMove.Y != 0)
            {
              this.m_multiQuoteData.yChange += e.Location.Y - hqClientForm.m_mouseBeforeMove.Y;
              hqClientForm.m_mouseBeforeMove.Y = e.Location.Y;
              this.rePaintYScrollBar();
            }
            if ((double) this.m_multiQuoteData.iStart != (double) this.m_multiQuoteData.yChange / this.yOnceMove)
            {
              this.m_multiQuoteData.iStart = (int) ((double) this.m_multiQuoteData.yChange / this.yOnceMove);
              if (this.m_multiQuoteData.iStart <= 0)
                this.m_multiQuoteData.iStart = 0;
              this.m_hqForm.Repaint();
            }
          }
          if (this.scrolXOrY == 1)
            hqClientForm.m_mouseBeforeMove.X = e.Location.X;
          else if (this.scrolXOrY == 2)
            hqClientForm.m_mouseBeforeMove.Y = e.Location.Y;
        }
        if (num2 <= 0 || num2 >= this.fontHeight)
        {
          this.m_hqForm.M_Cursor = Cursors.Default;
        }
        else
        {
          int x = this.rcData.X;
          for (int index = 0; index < this.m_strItems.Length; ++index)
          {
            if (index <= this.m_iStaticIndex || index >= this.iDynamicIndex)
            {
              MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[index]];
              if (multiQuoteItemInfo != null)
              {
                if (index > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
                  return;
                if (num1 > x && num1 < x + multiQuoteItemInfo.width && multiQuoteItemInfo.sortID == -1)
                {
                  this.m_hqForm.M_Cursor = Cursors.Default;
                  return;
                }
                if (num1 > x && num1 < x + multiQuoteItemInfo.width && multiQuoteItemInfo.sortID != -1)
                {
                  this.m_hqForm.M_Cursor = Cursors.Hand;
                  return;
                }
                x += multiQuoteItemInfo.width;
                if (x > this.rcData.X + this.rcData.Width)
                  break;
              }
            }
          }
          this.m_hqForm.M_Cursor = Cursors.Default;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-Page_MouseMove异常：" + ex.Message);
      }
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        bool flag = false;
        switch (e.KeyData)
        {
          case Keys.Return:
            flag = this.Key_ENTER_Pressed();
            break;
          case Keys.Prior:
            flag = this.Key_PAGEUP_Pressed();
            break;
          case Keys.Next:
            flag = this.Key_PAGEDOWN_Pressed();
            break;
          case Keys.Left:
            flag = this.Key_LEFT_Pressed(1);
            break;
          case Keys.Up:
            flag = this.Key_UP_Pressed();
            break;
          case Keys.Right:
            flag = this.Key_RIGHT_Pressed(1);
            break;
          case Keys.Down:
            flag = this.Key_DOWN_Pressed();
            break;
          case Keys.F10:
            flag = this.Key_F10_Pressed();
            break;
        }
        this.m_hqForm.IsNeedRepaint = flag;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-Page_KeyDown异常：" + ex.Message);
      }
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_SortBy"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_StockRank"));
      toolStripMenuItem1.Name = "SortBy";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
      toolStripMenuItem2.Name = "cmd_80";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
      toolStripMenuItem3.Name = "minline";
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
      toolStripMenuItem4.Name = "kline";
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_CommodityInfo") + "  F10");
      toolStripMenuItem5.Name = "commodityInfo";
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      for (int index = 0; index < this.m_strItems.Length; ++index)
      {
        MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) this.m_strItems[index]];
        if (multiQuoteItemInfo != null && multiQuoteItemInfo.sortID != -1)
        {
          ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(multiQuoteItemInfo.name);
          toolStripMenuItem6.Name = "Sort_" + this.m_strItems[index];
          contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem6);
        }
      }
      contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem1.DropDown = (ToolStripDropDown) contextMenuStrip;
      this.AddUserCommodity = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_AddUserCommodity"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_AddCustom"));
      this.AddUserCommodity.Name = "AddUserCommodity";
      this.AddUserCommodity.Visible = !this.buttonUtils.CurButtonName.Equals("MyCommodity");
      this.DelUserCommodity = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_DelUserCommodity"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_DelCustom"));
      this.DelUserCommodity.Name = "DelUserCommodity";
      this.DelUserCommodity.Visible = this.buttonUtils.CurButtonName.Equals("MyCommodity");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem3);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem4);
      if (this.m_hqForm.isDisplayF10Menu())
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem5);
      this.contextMenuStrip.Items.Add((ToolStripItem) this.AddUserCommodity);
      this.contextMenuStrip.Items.Add((ToolStripItem) this.DelUserCommodity);
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
        else if (name.IndexOf("Sort_") >= 0)
          this.changeSortField(name.Substring(5));
        else if (name.Equals("minline"))
        {
          if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
          {
            ProductDataVO productDataVo = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
            this.m_hqForm.ShowPageMinLine(new CommodityInfo(productDataVo.marketID, productDataVo.code));
          }
        }
        else if (name.Equals("kline"))
        {
          if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
          {
            ProductDataVO productDataVo = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
            this.m_hqForm.ShowPageKLine(new CommodityInfo(productDataVo.marketID, productDataVo.code));
          }
        }
        else if (name.Equals("AddUserCommodity"))
        {
          if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
          {
            ProductDataVO productDataVo = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
            this.m_hqClient.myCommodity.AddMyCommodity(productDataVo.marketID + "_" + productDataVo.code);
          }
        }
        else if (name.Equals("DelUserCommodity"))
        {
          if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
          {
            ProductDataVO productDataVo = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
            string commodityCode = productDataVo.marketID + "_" + productDataVo.code;
            if (this.m_hqClient != null)
              this.m_hqClient.myCommodity.DelMyCommodity(commodityCode);
          }
        }
        else if (name.Equals("commodityInfo"))
          this.Key_F10_Pressed();
        else
          this.m_hqForm.UserCommand(name);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-contextMenu_ItemClicked异常：" + ex.Message);
      }
    }

    private void selectProduct(int x, int y)
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length <= 0)
        return;
      int x1 = this.rcData.X;
      int num1 = this.rcData.Y + this.fontTitle.Height + 2;
      int num2 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
      if (num2 > this.iStockRows)
        num2 = this.iStockRows;
      for (int iNewPos = 1; iNewPos < num2 + 1; ++iNewPos)
      {
        if (x > x1 && x < x1 + this.rcData.Width && (y > num1 && y < num1 + this.fontHeight + 2))
        {
          if (iNewPos == this.m_multiQuoteData.iHighlightIndex)
            break;
          this.repaintHighlightBar(iNewPos);
          break;
        }
        num1 += this.fontHeight + 2;
      }
    }

    private void ClickButton(int x, int y)
    {
      try
      {
        MyButton myButton1 = this.buttonGraph.MouseLeftClicked(x, y, this.buttonUtils.ButtonList, true);
        if (myButton1 != null && !myButton1.Name.StartsWith("More"))
        {
          this.buttonUtils.CurButtonName = myButton1.Name;
          this.m_multiQuoteData.iStart = 0;
          this.AddUserCommodity.Visible = !this.buttonUtils.CurButtonName.Equals("MyCommodity");
          this.DelUserCommodity.Visible = this.buttonUtils.CurButtonName.Equals("MyCommodity");
          this.m_hqForm.Repaint();
        }
        MyButton myButton2 = this.buttonGraph.MouseRightClicked(x, y, this.buttonUtils.ButtonList);
        if (myButton2 == null || !myButton2.Name.StartsWith("More") || (this.m_hqClient == null || this.m_hqClient.m_htMarketData == null) || this.m_hqClient.m_htMarketData.Count <= 0)
          return;
        this.CreatMarketListMenu(this.buttonUtils.ButtonList, x, y);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-ClickButton异常：" + ex.Message);
      }
    }

    private void ClickButtonRight(int x, int y)
    {
      try
      {
        MyButton myButton = this.buttonGraph.MouseRightClicked(x, y, this.buttonUtils.ButtonList);
        if (myButton == null)
          return;
        string str = myButton.Name;
        if (this.m_hqClient.m_htMarketData.Count <= 1 || !str.StartsWith("Market"))
          return;
        string marketID = str.Substring(6);
        if (this.m_hqClient == null || this.m_hqClient.commodityClass == null || this.m_hqClient.commodityClass.classList.Count <= 0)
          return;
        this.CreatClassListMenu(marketID, this.m_hqClient.commodityClass.classList, x, y);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-ClickButtonRight异常：" + ex.Message);
      }
    }

    private void changeSortField(string strSortItem)
    {
      MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo) this.m_hqClient.m_htItemInfo[(object) strSortItem];
      if (multiQuoteItemInfo == null || multiQuoteItemInfo.sortID == -1)
        return;
      this.m_bShowSortTag = true;
      if (this.strSortItem.Equals(strSortItem))
      {
        this.isDescend = (int) this.isDescend == 1 ? (byte) 0 : (byte) 1;
        this.m_multiQuoteData.iStart = 0;
      }
      else
      {
        this.isDescend = (byte) 0;
        this.strSortItem = strSortItem;
      }
      this.sortItems();
      this.m_hqForm.Repaint();
      this.AskForDataOnTimer();
    }

    private bool Key_LEFT_Pressed(int leftColNum = 1)
    {
      if (this.iDynamicIndex == this.m_iStaticIndex + 1)
        return false;
      this.iDynamicIndex = this.iDynamicIndex - leftColNum;
      this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
      return true;
    }

    private bool Key_RIGHT_Pressed(int rightColNum = 1)
    {
      bool flag = false;
      if (this.iDynamicIndex < this.m_strItems.Length - 1)
      {
        this.iDynamicIndex = this.iDynamicIndex + rightColNum;
        this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
        flag = true;
      }
      return flag;
    }

    private bool Key_DOWN_Pressed()
    {
      if (!this.bCanMove || this.m_multiQuoteData.m_curQuoteList.Length <= 0 || this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
        return false;
      if (this.m_multiQuoteData.iHighlightIndex < this.iStockRows)
      {
        this.repaintHighlightBar(this.m_multiQuoteData.iHighlightIndex + 1);
        return false;
      }
      this.m_multiQuoteData.iStart += this.iStockRows - 1 > 0 ? this.iStockRows : 1;
      this.m_multiQuoteData.iHighlightIndex = 1;
      this.m_multiQuoteData.yChange = (int) (this.yOnceMove * (double) this.m_multiQuoteData.iStart);
      if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
        this.m_multiQuoteData.yChange = this.yMaxChaneg;
      return true;
    }

    private bool Key_UP_Pressed()
    {
      if (!this.bCanMove || this.m_multiQuoteData.m_curQuoteList.Length <= 0)
        return false;
      if (this.m_multiQuoteData.iHighlightIndex > 1)
        this.repaintHighlightBar(this.m_multiQuoteData.iHighlightIndex - 1);
      else if (this.m_multiQuoteData.iStart > 0)
      {
        this.m_multiQuoteData.iStart -= this.iStockRows - 1 > 0 ? this.iStockRows : 1;
        if (this.m_multiQuoteData.iStart < 0)
          this.m_multiQuoteData.iStart = 0;
        this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
        if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
          this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length;
        this.m_multiQuoteData.yChange = (int) (this.yOnceMove * (double) this.m_multiQuoteData.iStart);
        return true;
      }
      return false;
    }

    private bool Key_PAGEUP_Pressed()
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        if (this.m_multiQuoteData.iStart > 0)
        {
          this.m_multiQuoteData.iStart -= this.iStockRows - 1 > 0 ? this.iStockRows - 1 : 1;
          if (this.m_multiQuoteData.iStart < 0)
            this.m_multiQuoteData.iStart = 0;
          this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
          if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
            this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length - 1;
          this.m_multiQuoteData.yChange = (int) (this.yOnceMove * (double) this.m_multiQuoteData.iStart);
          return true;
        }
        this.repaintHighlightBar(1);
      }
      return false;
    }

    public bool YScrollUp(int UpDataRowNum = 1)
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        if (this.m_multiQuoteData.iStart > 0)
        {
          this.m_multiQuoteData.iStart -= this.iStockRows - 1 > 0 ? this.iStockRows - 1 : 1;
          if (this.m_multiQuoteData.iStart < 0)
            this.m_multiQuoteData.iStart = 0;
          this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
          if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
            this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length - 1;
          return true;
        }
        this.repaintHighlightBar(1);
      }
      return false;
    }

    private bool Key_PAGEDOWN_Pressed()
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        if (this.m_multiQuoteData.iStart + this.iStockRows < this.m_multiQuoteData.m_curQuoteList.Length)
        {
          this.m_multiQuoteData.iStart += this.iStockRows - 1 > 0 ? this.iStockRows - 1 : 1;
          this.m_multiQuoteData.iHighlightIndex = 1;
          this.m_multiQuoteData.yChange = (int) (this.yOnceMove * (double) this.m_multiQuoteData.iStart);
          if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
            this.m_multiQuoteData.yChange = this.yMaxChaneg;
          return true;
        }
        this.repaintHighlightBar(this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart);
      }
      return false;
    }

    private bool Key_F10_Pressed()
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        int index = this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1;
        if (index >= 0 && index <= this.m_multiQuoteData.m_curQuoteList.Length - 1)
          this.m_hqForm.DisplayCommodityInfo(this.m_multiQuoteData.m_curQuoteList[index].code);
      }
      return false;
    }

    private bool Key_ENTER_Pressed()
    {
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        int index = this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1;
        if (index >= 0 && index <= this.m_multiQuoteData.m_curQuoteList.Length - 1)
        {
          this.m_hqForm.QueryStock(new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[index].marketID, this.m_multiQuoteData.m_curQuoteList[index].code));
          return true;
        }
      }
      return false;
    }

    private void CreatClassListMenu(string marketID, ArrayList classList, int x, int y)
    {
      try
      {
        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        contextMenuStrip1.BackColor = Color.Black;
        contextMenuStrip1.ForeColor = Color.White;
        contextMenuStrip1.ShowImageMargin = false;
        contextMenuStrip1.ShowCheckMargin = false;
        ContextMenuStrip contextMenuStrip2 = contextMenuStrip1;
        for (int index = 0; index < classList.Count; ++index)
        {
          ClassVO classVo = (ClassVO) classList[index];
          ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(classVo.name);
          toolStripMenuItem.Name = "MAC" + marketID + "_" + classVo.classID;
          contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem);
        }
        if (contextMenuStrip2.Items.Count > 0)
        {
          ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_AllCommodity"));
          toolStripMenuItem.Name = "MAC" + marketID;
          contextMenuStrip2.Items.Insert(0, (ToolStripItem) toolStripMenuItem);
        }
        contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(this.classMenuStrip_ItemClicked);
        contextMenuStrip2.Show((Control) this.m_hqForm, new Point(x, y), ToolStripDropDownDirection.AboveRight);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-CreatClassListMenu异常：" + ex.Message);
      }
    }

    private void CreatMarketListMenu(ArrayList marketList, int x, int y)
    {
      try
      {
        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        contextMenuStrip1.BackColor = Color.Black;
        contextMenuStrip1.ForeColor = Color.White;
        contextMenuStrip1.ShowImageMargin = false;
        contextMenuStrip1.ShowCheckMargin = false;
        ContextMenuStrip contextMenuStrip2 = contextMenuStrip1;
        for (int index = this.setInfo.ShowMarketBtnCount + 1; index < marketList.Count; ++index)
        {
          MyButton myButton = (MyButton) marketList[index];
          ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(myButton.Text);
          toolStripMenuItem.Name = myButton.Name;
          contextMenuStrip2.Items.Add((ToolStripItem) toolStripMenuItem);
        }
        contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(this.marketMenuStrip_ItemClicked);
        contextMenuStrip2.Show((Control) this.m_hqForm, new Point(x, y), ToolStripDropDownDirection.AboveRight);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-CreatMarketListMenu异常：" + ex.Message);
      }
    }

    private void marketMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        string name = e.ClickedItem.Name;
        if (name.StartsWith("All"))
        {
          MarketForm marketForm = new MarketForm();
          int num1 = 0;
          foreach (DictionaryEntry dictionaryEntry in this.m_hqClient.m_htMarketData)
          {
            MarketDataVO marketDataVo = (MarketDataVO) dictionaryEntry.Value;
            Label label1 = new Label();
            label1.Parent = (Control) marketForm.MainPanel;
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 35 + num1 * 25);
            label1.Font = new Font("宋体", 12f, FontStyle.Regular);
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label1.Text = marketDataVo.marketID;
            Label label2 = new Label();
            label2.Tag = (object) ("Market" + marketDataVo.marketID);
            label2.Parent = (Control) marketForm.MainPanel;
            label2.ForeColor = Color.White;
            label2.Location = new Point(110, this.lbLocationY + num1 * this.lbHeight);
            label2.Font = new Font("宋体", 12f, FontStyle.Underline);
            label2.Cursor = Cursors.Hand;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Text = marketDataVo.marketName;
            label2.Click += new EventHandler(this.lbValue_Click);
            ++num1;
          }
          int num2 = (int) marketForm.ShowDialog();
        }
        else
        {
          this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
          this.buttonUtils.CurButtonName = name;
          this.m_multiQuoteData.iStart = 0;
        }
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-marketMenuStrip_ItemClicked异常：" + ex.Message);
      }
    }

    private void lbValue_Click(object sender, EventArgs e)
    {
      try
      {
        Label label = sender as Label;
        string text = label.Text;
        MarketForm marketForm = label.Parent.Parent as MarketForm;
        string str = label.Tag.ToString();
        if (str.StartsWith("Market"))
        {
          this.buttonUtils.selectTemp = (label.Location.Y - this.lbLocationY) / this.lbHeight + 1;
          if (this.buttonUtils.selectTemp > this.setInfo.ShowMarketBtnCount - 2)
            this.buttonUtils.selectTemp = this.setInfo.ShowMarketBtnCount;
          this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
          this.buttonUtils.CurButtonName = str;
          this.m_multiQuoteData.iStart = 0;
        }
        this.m_hqForm.Repaint();
        marketForm.Close();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-lbValue_Click异常：" + ex.Message);
      }
    }

    private void classMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        string name = e.ClickedItem.Name;
        if (name.StartsWith("MAC"))
        {
          this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
          this.buttonUtils.CurButtonName = name;
          this.m_multiQuoteData.iStart = 0;
        }
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MultiQuote-classMenuStrip_ItemClicked异常：" + ex.Message);
      }
    }

    public override void Dispose()
    {
      this.setInfo.saveSetInfo("CurButtonName", this.buttonUtils.CurButtonName);
      this.setInfo.lastSave();
      GC.Collect();
    }
  }
}
