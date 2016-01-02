// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Draw_KLine
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.OutInfo;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class Draw_KLine
  {
    private int cache_m_VirtualRatio = 15;
    private Rectangle[] m_rcPane = new Rectangle[3];
    private IndicatorBase[] m_indicator = new IndicatorBase[3];
    private IndicatorPos m_pos = new IndicatorPos();
    public int m_iPos = -1;
    public const int CYCLE_DAY = 1;
    public const int CYCLE_WEEK = 2;
    public const int CYCLE_MONTH = 3;
    public const int CYCLE_QUARTER = 4;
    public const int CYCLE_MIN1 = 5;
    public const int CYCLE_MIN3 = 6;
    public const int CYCLE_MIN5 = 7;
    public const int CYCLE_MIN15 = 8;
    public const int CYCLE_MIN30 = 9;
    public const int CYCLE_MIN60 = 10;
    public const int CYCLE_HOUR2 = 11;
    public const int CYCLE_HOUR4 = 12;
    public const int CYCLE_ANYDAY = 13;
    public const int CYCLE_ANYMIN = 14;
    private Page_KLine parent;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ProductData m_product;
    private KLineData[] m_kData;
    private Rectangle m_rcLabel;
    private int m_iPrecision;
    private DateTime dateTimeRange;

    public Draw_KLine(Page_KLine _parent)
    {
      this.m_pos.m_VirtualRatio = this.cache_m_VirtualRatio;
      this.parent = _parent;
      this.pluginInfo = this.parent.pluginInfo;
      this.setInfo = this.parent.setInfo;
      int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
      this.m_indicator[0] = !this.parent.m_hqClient.isIndex(this.parent.m_hqClient.curCommodityInfo) || this.parent.m_hqClient.m_bShowIndexKLine != 0 ? (IndicatorBase) new MA(this.m_pos, _parent.m_globalData.m_iCurKLineType, precision, this.parent.m_hqForm) : (IndicatorBase) new MA(this.m_pos, 2, precision, this.parent.m_hqForm);
      this.m_indicator[1] = (IndicatorBase) new VOL(this.m_pos, 0);
      this.CreateIndicator();
    }

    internal void Paint(Graphics g, Rectangle rc, ProductData product, int value)
    {
      try
      {
        this.m_product = product;
        if (product != null)
          this.m_iPrecision = this.parent.m_hqClient.GetPrecision(this.m_product.commodityInfo);
        this.MakeCycleData(value);
        this.GetScreen(g, rc);
        this.DrawCycle(g);
        if (this.m_rcPane[0].Width < 0)
          return;
        this.DrawTimeCoordinate(g);
        for (int index = 0; index < 3; ++index)
        {
          if (this.m_indicator[index] != null)
            this.m_indicator[index].Paint(g, this.m_rcPane[index], this.m_kData);
          else
            Logger.wirte(MsgType.Warning, "m_indicator[" + (object) index + "]为空！");
        }
        if (this.parent.m_hqForm.IsMultiCycle)
          return;
        if (this.parent.isDrawCursor)
          this.DrawLabel(g);
        this.parent.m_hqForm.EndPaint();
        if (!this.parent.isDrawCursor)
          return;
        this.DrawCursor(-1);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Draw_KLine-Paint异常：" + ex.StackTrace + ex.Message);
      }
    }

    private void GetScreen(Graphics g, Rectangle rc)
    {
      Font font = new Font("宋体", 10f, FontStyle.Regular);
      int height = font.Height;
      int x = rc.X + 4 * height;
      int width = rc.Width - 4 * height - 2;
      this.m_rcPane[0] = new Rectangle(x, rc.Y, width, (rc.Height - height) / 2);
      this.m_rcPane[1] = new Rectangle(x, rc.Y + this.m_rcPane[0].Height, width, (rc.Height - height) / 4);
      this.m_rcPane[2] = new Rectangle(x, this.m_rcPane[1].Y + this.m_rcPane[1].Height, width, (rc.Height - height) / 4);
      Pen pen = new Pen(SetInfo.RHColor.clGrid);
      g.DrawRectangle(pen, this.m_rcPane[0].X, this.m_rcPane[0].Y, width, rc.Height - height);
      g.DrawLine(pen, this.m_rcPane[1].X, this.m_rcPane[1].Y, this.m_rcPane[1].X + width, this.m_rcPane[1].Y);
      g.DrawLine(pen, this.m_rcPane[2].X, this.m_rcPane[2].Y, this.m_rcPane[1].X + width, this.m_rcPane[2].Y);
      pen.Dispose();
      int num = -1;
      if (this.m_iPos != -1)
        num = this.m_pos.m_Begin + this.m_iPos;
      if (this.m_kData != null)
        this.m_pos.GetScreen(this.m_rcPane[0].Width, this.m_kData.Length);
      else
        this.m_pos.GetScreen(this.m_rcPane[0].Width, 0);
      if (this.m_iPos != -1)
        this.m_iPos = num < this.m_pos.m_Begin || num > this.m_pos.m_End ? -1 : num - this.m_pos.m_Begin;
      this.m_rcLabel = this.parent.m_hqClient.m_iKLineCycle >= 5 && this.parent.m_hqClient.m_iKLineCycle <= 12 || this.parent.m_hqClient.m_iKLineCycle == 14 ? new Rectangle(1, height, x - 1 - rc.X, height * 22) : new Rectangle(1, height, x - 1 - rc.X, height * 21);
      font.Dispose();
    }

    private void DrawTimeCoordinate(Graphics g)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      Font font = new Font("宋体", 10f, FontStyle.Regular);
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
      Pen pen = new Pen(SetInfo.RHColor.clGrid);
      Rectangle rectangle = new Rectangle(this.m_rcPane[2].X, this.m_rcPane[2].Y + this.m_rcPane[2].Height, this.m_rcPane[2].Width, font.Height);
      int num1;
      switch (this.parent.m_hqClient.m_iKLineCycle)
      {
        case 1:
        case 2:
        case 13:
          num1 = (int) g.MeasureString("2004-10-10", font).Width;
          break;
        case 3:
        case 4:
          num1 = (int) g.MeasureString("2004-10", font).Width;
          break;
        default:
          num1 = (int) g.MeasureString("10-30 09:40", font).Width;
          break;
      }
      int num2 = (int) ((double) num1 * 1.5 / (double) this.m_pos.m_Ratio) + 1;
      int num3 = this.m_pos.m_End - this.m_pos.m_Begin + 1;
      int num4 = (this.m_pos.m_End - this.m_pos.m_Begin) / num2;
      int y = rectangle.Y;
      int num5 = (int) ((double) rectangle.X + (double) this.m_pos.m_Ratio / 2.0);
      g.DrawLine(pen, num5, rectangle.Y, num5, rectangle.Y + 5);
      string s1 = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[this.m_pos.m_Begin].date);
      g.DrawString(s1, font, (Brush) solidBrush, (float) num5, (float) y);
      for (int index = 1; index < num4; ++index)
      {
        int num6 = (int) ((double) rectangle.X + (double) (index * num2) * (double) this.m_pos.m_Ratio + (double) this.m_pos.m_Ratio / 2.0);
        g.DrawLine(pen, num6, rectangle.Y, num6, rectangle.Y + 5);
        string s2 = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[index * num2 + this.m_pos.m_Begin].date);
        int num7 = num6 - num1 / 2;
        g.DrawString(s2, font, (Brush) solidBrush, (float) num7, (float) y);
      }
      if (num4 > 0)
      {
        int num6 = rectangle.X + (int) ((double) num3 * (double) this.m_pos.m_Ratio - (double) this.m_pos.m_Ratio / 2.0);
        g.DrawLine(pen, num6, rectangle.Y, num6, rectangle.Y + 5);
        string s2 = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[this.m_pos.m_End].date);
        if (num4 > 1 || num6 + num1 > rectangle.X + rectangle.Width)
          num6 -= num1;
        g.DrawString(s2, font, (Brush) solidBrush, (float) num6, (float) y);
      }
      pen.Dispose();
      solidBrush.Dispose();
      font.Dispose();
    }

    private string TimeToString(int iCycle, long date)
    {
      string str1;
      switch (iCycle)
      {
        case 1:
        case 2:
        case 13:
          str1 = Convert.ToString(date);
          if (str1.Length >= 8)
          {
            str1 = str1.Substring(0, 4) + "-" + str1.Substring(4, 2) + "-" + str1.Substring(6, 2);
            break;
          }
          break;
        case 3:
        case 4:
          str1 = Convert.ToString(date / 100L);
          if (str1.Length >= 6)
          {
            str1 = str1.Substring(0, 4) + "-" + str1.Substring(4, 2);
            break;
          }
          break;
        default:
          string str2 = Convert.ToString(date);
          if (str2.Length >= 12)
          {
            string str3 = str2.Substring(4);
            str2 = str3.Substring(0, 2) + "-" + str3.Substring(2, 2) + " " + str3.Substring(4, 2) + ":" + str3.Substring(6, 2);
          }
          return str2;
      }
      return str1;
    }

    private void MakeCycleData(int value)
    {
      if (this.m_product == null)
        return;
      if (1 == this.parent.m_hqClient.m_iKLineCycle || 2 == this.parent.m_hqClient.m_iKLineCycle || (3 == this.parent.m_hqClient.m_iKLineCycle || 4 == this.parent.m_hqClient.m_iKLineCycle) || 13 == this.parent.m_hqClient.m_iKLineCycle)
        this.MakeTodayDayLine();
      else if (7 == this.parent.m_hqClient.m_iKLineCycle)
        this.MakeToday5MinLine();
      else
        this.MakeToday1MinLine();
      if (this.m_kData == null)
        return;
      switch (this.parent.m_hqClient.m_iKLineCycle)
      {
        case 2:
          this.MakeWeek();
          break;
        case 3:
          this.MakeMonth();
          break;
        case 4:
          this.MakeQUARTER();
          break;
        case 6:
          this.MakeMinCycle(3);
          break;
        case 8:
          this.MakeMinCycle(15);
          break;
        case 9:
          this.MakeMinCycle(30);
          break;
        case 10:
          this.MakeMinCycle(60);
          break;
        case 11:
          this.MakeMinCycle(120);
          break;
        case 12:
          this.MakeMinCycle(240);
          break;
        case 13:
          if (value == 0)
            value = 1;
          this.MakeDay(value);
          break;
        case 14:
          if (value == 0)
            value = 1;
          this.MakeMinCycle(value);
          break;
      }
    }

    private void MakeTodayDayLine()
    {
      if (this.m_product.realData == null || (double) this.m_product.realData.curPrice < 1.0 / 1000.0)
      {
        this.m_kData = this.m_product.dayKLine;
      }
      else
      {
        int index1 = this.m_product.dayKLine != null ? this.m_product.dayKLine.Length : 0;
        if (this.parent.m_hqClient.TimeRange == null)
          return;
        if (index1 > 0 && this.m_product.dayKLine[index1 - 1].date > (long) this.parent.m_hqClient.TimeRange[0].tradeDate)
        {
          this.m_kData = this.m_product.dayKLine;
        }
        else
        {
          if (index1 > 0 && this.m_product.dayKLine[index1 - 1].date == (long) this.parent.m_hqClient.TimeRange[0].tradeDate)
          {
            if (this.m_product.realData.totalAmount <= 0L)
            {
              this.m_kData = this.m_product.dayKLine;
              return;
            }
            --index1;
          }
          this.m_kData = new KLineData[index1 + 1];
          for (int index2 = 0; index2 < index1; ++index2)
            this.m_kData[index2] = this.m_product.dayKLine[index2];
          this.m_kData[index1] = new KLineData();
          this.m_kData[index1].date = (long) this.parent.m_hqClient.TimeRange[0].tradeDate;
          this.m_kData[index1].openPrice = this.m_product.realData.openPrice;
          this.m_kData[index1].highPrice = this.m_product.realData.highPrice;
          this.m_kData[index1].lowPrice = this.m_product.realData.lowPrice;
          this.m_kData[index1].closePrice = this.m_product.realData.curPrice;
          this.m_kData[index1].balancePrice = this.m_product.realData.balancePrice;
          this.m_kData[index1].totalAmount = this.m_product.realData.totalAmount;
          this.m_kData[index1].totalMoney = this.m_product.realData.totalMoney;
          this.m_kData[index1].reserveCount = this.m_product.realData.reserveCount;
        }
      }
    }

    private void MakeToday1MinLine()
    {
      if (this.m_product.realData == null)
      {
        CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
        if (codeTable == null || codeTable.status != 1 && codeTable.status != 6)
          return;
        this.m_kData = this.m_product.min1KLine;
      }
      else
      {
        KLineData[] klineDataArray = this.get1MinKLine(this.m_product.commodityInfo, this.m_product.aBill, this.m_product.realData.yesterBalancePrice);
        if (this.m_product.min1KLine == null)
          this.m_kData = klineDataArray;
        else if (klineDataArray.Length == 0)
        {
          this.m_kData = this.m_product.min1KLine;
        }
        else
        {
          int index1;
          for (index1 = this.m_product.min1KLine.Length - 1; index1 >= 0; --index1)
          {
            if (this.parent.m_hqClient.TimeRange.Length > 0)
            {
              long num = (long) this.parent.m_hqClient.TimeRange[0].beginDate * 10000L + (long) this.parent.m_hqClient.TimeRange[0].beginTime;
              if (this.m_product.min1KLine[index1].date < num)
                break;
            }
          }
          int num1 = index1 + 1;
          this.m_kData = new KLineData[num1 + klineDataArray.Length];
          for (int index2 = 0; index2 < num1; ++index2)
            this.m_kData[index2] = this.m_product.min1KLine[index2];
          for (int index2 = 0; index2 < klineDataArray.Length; ++index2)
          {
            if (klineDataArray[index2].reserveCount == 0 && klineDataArray[index2].totalAmount == 0L)
            {
              if (index2 == 0)
              {
                if (num1 > 0)
                  klineDataArray[index2].reserveCount = this.m_kData[num1 - 1].reserveCount;
              }
              else
                klineDataArray[index2].reserveCount = klineDataArray[index2 - 1].reserveCount;
            }
            this.m_kData[index2 + num1] = klineDataArray[index2];
          }
        }
      }
    }

    private KLineData[] get1MinKLine(CommodityInfo commodityInfo, ArrayList aBillData, float fPreClosePrice)
    {
      if (aBillData == null)
        return new KLineData[0];
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
      TradeTimeVO[] timeRange = M_Common.getTimeRange(commodityInfo, this.parent.m_hqClient);
      ArrayList arrayList = new ArrayList();
      KLineData klineData1 = (KLineData) null;
      DateTime dateTime = new DateTime();
      float num1 = 0.0f;
      long num2 = 0L;
      for (int index = 0; index < aBillData.Count; ++index)
      {
        BillDataVO billDataVo = (BillDataVO) aBillData[index];
        if (billDataVo != null)
        {
          if ((double) billDataVo.curPrice > 0.0)
          {
            DateTime dateTimePlus = this.GetDateTimePlus((long) billDataVo.date * 10000L + (long) (billDataVo.time / 100), 1, false);
            if (dateTimePlus != dateTime)
            {
              if (klineData1 != null)
              {
                if (klineData1.totalAmount > 0L)
                  klineData1.balancePrice = codeTable == null ? (float) klineData1.totalMoney / (float) klineData1.totalAmount : (float) klineData1.totalMoney / (float) klineData1.totalAmount / codeTable.fUnit;
                arrayList.Add((object) klineData1);
              }
              klineData1 = new KLineData();
              int num3 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("yyyy-MM-dd"));
              int num4 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("HH:mm:ss"));
              klineData1.date = (long) num3 * 10000L + (long) (num4 / 100);
              klineData1.openPrice = billDataVo.curPrice;
              klineData1.highPrice = billDataVo.curPrice;
              klineData1.lowPrice = billDataVo.curPrice;
              klineData1.closePrice = billDataVo.curPrice;
              klineData1.balancePrice = billDataVo.balancePrice;
              klineData1.reserveCount = billDataVo.reserveCount;
              klineData1.totalAmount += billDataVo.totalAmount - num2;
              klineData1.totalMoney += billDataVo.totalMoney - (double) num1;
              dateTime = dateTimePlus;
            }
            else
            {
              if ((double) billDataVo.curPrice > (double) klineData1.highPrice)
                klineData1.highPrice = billDataVo.curPrice;
              if ((double) billDataVo.curPrice < (double) klineData1.lowPrice)
                klineData1.lowPrice = billDataVo.curPrice;
              klineData1.closePrice = billDataVo.curPrice;
              klineData1.balancePrice = billDataVo.balancePrice;
              klineData1.reserveCount = billDataVo.reserveCount;
              klineData1.totalAmount += billDataVo.totalAmount - num2;
              klineData1.totalMoney += billDataVo.totalMoney - (double) num1;
            }
            num2 = billDataVo.totalAmount;
            num1 = (float) billDataVo.totalMoney;
          }
        }
        else
          break;
      }
      if (klineData1 != null)
      {
        if (klineData1.totalAmount > 0L)
          klineData1.balancePrice = codeTable == null ? (float) klineData1.totalMoney / (float) klineData1.totalAmount : (float) klineData1.totalMoney / (float) klineData1.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData1);
      }
      KLineData[] klineDataArray = new KLineData[0];
      if (arrayList.Count > 0)
      {
        int iTime = this.parent.m_hqClient.m_iTime / 100;
        if (this.parent.m_hqClient.m_iTime % 100 > 0)
        {
          int num3 = iTime / 100 * 60 + iTime % 100 + 1;
          iTime = (num3 / 60 * 100 + num3 % 60) % 2400;
        }
        int length = TradeTimeVO.GetIndexFromTime(this.parent.m_hqClient.m_iDate, iTime, timeRange) + 1;
        if (length == -1)
          length = 0;
        klineDataArray = new KLineData[length];
        int index = 0;
        for (int iIndex = 0; iIndex < length; ++iIndex)
        {
          DateTime dateTimeFromIndex = TradeTimeVO.GetDateTimeFromIndex(iIndex, timeRange);
          int num3 = TradeTimeVO.TimeStringToInt(dateTimeFromIndex.ToString("HH:mm"));
          long num4 = (long) (dateTimeFromIndex.Year * 10000 + dateTimeFromIndex.Month * 100 + dateTimeFromIndex.Day) * 10000L + (long) num3;
          if (index < arrayList.Count)
          {
            KLineData klineData2 = (KLineData) arrayList[index];
            if (num4 == klineData2.date)
            {
              klineDataArray[iIndex] = klineData2;
              ++index;
              continue;
            }
          }
          klineDataArray[iIndex] = new KLineData();
          klineDataArray[iIndex].date = num4;
          if (iIndex == 0)
          {
            klineDataArray[iIndex].balancePrice = fPreClosePrice;
            klineDataArray[iIndex].openPrice = klineDataArray[iIndex].highPrice = klineDataArray[iIndex].lowPrice = klineDataArray[iIndex].closePrice = fPreClosePrice;
            klineDataArray[iIndex].reserveCount = 0;
            klineDataArray[iIndex].totalAmount = 0L;
            klineDataArray[iIndex].totalMoney = 0.0;
          }
          else
          {
            klineDataArray[iIndex].balancePrice = klineDataArray[iIndex - 1].balancePrice;
            klineDataArray[iIndex].openPrice = klineDataArray[iIndex].highPrice = klineDataArray[iIndex].lowPrice = klineDataArray[iIndex].closePrice = klineDataArray[iIndex - 1].closePrice;
            klineDataArray[iIndex].reserveCount = klineDataArray[iIndex - 1].reserveCount;
            klineDataArray[iIndex].totalAmount = 0L;
            klineDataArray[iIndex].totalMoney = 0.0;
          }
        }
      }
      return klineDataArray;
    }

    private void MakeToday5MinLine()
    {
      if (this.m_product.realData == null)
      {
        CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
        if (codeTable == null || codeTable.status != 1 && codeTable.status != 6)
          return;
        this.m_kData = this.m_product.min5KLine;
      }
      else
      {
        KLineData[] klineDataArray = this.get5MinKLine(this.m_product.commodityInfo, this.m_product.aBill, this.m_product.realData.yesterBalancePrice);
        if (this.m_product.min5KLine == null)
          this.m_kData = klineDataArray;
        else if (klineDataArray.Length == 0)
        {
          this.m_kData = this.m_product.min5KLine;
        }
        else
        {
          int index1;
          for (index1 = this.m_product.min5KLine.Length - 1; index1 >= 0; --index1)
          {
            if (this.parent.m_hqClient.TimeRange.Length > 0)
            {
              long num = (long) this.parent.m_hqClient.TimeRange[0].beginDate * 10000L + (long) this.parent.m_hqClient.TimeRange[0].beginTime;
              if (this.m_product.min5KLine[index1].date < num)
                break;
            }
          }
          int num1 = index1 + 1;
          this.m_kData = new KLineData[num1 + klineDataArray.Length];
          for (int index2 = 0; index2 < num1; ++index2)
            this.m_kData[index2] = this.m_product.min5KLine[index2];
          for (int index2 = 0; index2 < klineDataArray.Length; ++index2)
          {
            if (klineDataArray[index2].reserveCount == 0 && klineDataArray[index2].totalAmount == 0L)
            {
              if (index2 == 0)
              {
                if (num1 > 0)
                  klineDataArray[index2].reserveCount = this.m_kData[num1 - 1].reserveCount;
              }
              else
                klineDataArray[index2].reserveCount = klineDataArray[index2 - 1].reserveCount;
            }
            this.m_kData[index2 + num1] = klineDataArray[index2];
          }
        }
      }
    }

    private KLineData[] get5MinKLine(CommodityInfo commodityInfo, ArrayList aBillData, float fPreClosePrice)
    {
      if (aBillData == null)
        return new KLineData[0];
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
      TradeTimeVO[] timeRange = M_Common.getTimeRange(commodityInfo, this.parent.m_hqClient);
      ArrayList arrayList = new ArrayList();
      KLineData klineData1 = (KLineData) null;
      DateTime dateTime = new DateTime();
      float num1 = 0.0f;
      long num2 = 0L;
      for (int index = 0; index < aBillData.Count; ++index)
      {
        BillDataVO billDataVo = (BillDataVO) aBillData[index];
        if (billDataVo != null)
        {
          if ((double) billDataVo.curPrice > 0.0)
          {
            DateTime dateTimePlus = this.GetDateTimePlus((long) billDataVo.date * 10000L + (long) (billDataVo.time / 100), 5, false);
            if (dateTimePlus != dateTime)
            {
              if (klineData1 != null)
              {
                if (klineData1.totalAmount > 0L)
                  klineData1.balancePrice = codeTable == null ? (float) klineData1.totalMoney / (float) klineData1.totalAmount : (float) klineData1.totalMoney / (float) klineData1.totalAmount / codeTable.fUnit;
                arrayList.Add((object) klineData1);
              }
              klineData1 = new KLineData();
              int num3 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("yyyy-MM-dd"));
              int num4 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("HH:mm:ss"));
              klineData1.date = (long) num3 * 10000L + (long) (num4 / 100);
              klineData1.openPrice = billDataVo.curPrice;
              klineData1.highPrice = billDataVo.curPrice;
              klineData1.lowPrice = billDataVo.curPrice;
              klineData1.closePrice = billDataVo.curPrice;
              klineData1.balancePrice = billDataVo.balancePrice;
              klineData1.reserveCount = billDataVo.reserveCount;
              klineData1.totalAmount += billDataVo.totalAmount - num2;
              klineData1.totalMoney += billDataVo.totalMoney - (double) num1;
              dateTime = dateTimePlus;
            }
            else
            {
              if ((double) billDataVo.curPrice > (double) klineData1.highPrice)
                klineData1.highPrice = billDataVo.curPrice;
              if ((double) billDataVo.curPrice < (double) klineData1.lowPrice)
                klineData1.lowPrice = billDataVo.curPrice;
              klineData1.closePrice = billDataVo.curPrice;
              klineData1.balancePrice = billDataVo.balancePrice;
              klineData1.reserveCount = billDataVo.reserveCount;
              klineData1.totalAmount += billDataVo.totalAmount - num2;
              klineData1.totalMoney += billDataVo.totalMoney - (double) num1;
            }
            num2 = billDataVo.totalAmount;
            num1 = (float) billDataVo.totalMoney;
          }
        }
        else
          break;
      }
      if (klineData1 != null)
      {
        if (klineData1.totalAmount > 0L)
          klineData1.balancePrice = codeTable == null ? (float) klineData1.totalMoney / (float) klineData1.totalAmount : (float) klineData1.totalMoney / (float) klineData1.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData1);
      }
      KLineData[] klineDataArray = new KLineData[0];
      if (arrayList.Count > 0)
      {
        int iTime = this.parent.m_hqClient.m_iTime / 100;
        if (this.parent.m_hqClient.m_iTime % 100 > 0)
        {
          int num3 = iTime / 100 * 60 + iTime % 100 + 1;
          iTime = (num3 / 60 * 100 + num3 % 60) % 2400;
        }
        int indexFromTime = TradeTimeVO.GetIndexFromTime(this.parent.m_hqClient.m_iDate, iTime, timeRange);
        int length = indexFromTime / 5;
        if (indexFromTime % 5 >= 0)
          ++length;
        klineDataArray = new KLineData[length];
        int index1 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
          DateTime dateTimeFromIndex = TradeTimeVO.GetDateTimeFromIndex(index2 * 5 + 4, timeRange);
          int num3 = TradeTimeVO.TimeStringToInt(dateTimeFromIndex.ToString("HH:mm"));
          long num4 = (long) (dateTimeFromIndex.Year * 10000 + dateTimeFromIndex.Month * 100 + dateTimeFromIndex.Day) * 10000L + (long) num3;
          if (index1 < arrayList.Count)
          {
            KLineData klineData2 = (KLineData) arrayList[index1];
            if (num4 == klineData2.date)
            {
              klineDataArray[index2] = klineData2;
              ++index1;
              continue;
            }
          }
          klineDataArray[index2] = new KLineData();
          klineDataArray[index2].date = num4;
          if (index2 == 0)
          {
            klineDataArray[index2].balancePrice = fPreClosePrice;
            klineDataArray[index2].openPrice = klineDataArray[index2].highPrice = klineDataArray[index2].lowPrice = klineDataArray[index2].closePrice = fPreClosePrice;
            klineDataArray[index2].reserveCount = 0;
            klineDataArray[index2].totalAmount = 0L;
            klineDataArray[index2].totalMoney = 0.0;
          }
          else
          {
            klineDataArray[index2].balancePrice = klineDataArray[index2 - 1].balancePrice;
            klineDataArray[index2].openPrice = klineDataArray[index2].highPrice = klineDataArray[index2].lowPrice = klineDataArray[index2].closePrice = klineDataArray[index2 - 1].closePrice;
            klineDataArray[index2].reserveCount = klineDataArray[index2 - 1].reserveCount;
            klineDataArray[index2].totalAmount = 0L;
            klineDataArray[index2].totalMoney = 0.0;
          }
        }
      }
      return klineDataArray;
    }

    private void MakeWeek()
    {
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
      ArrayList arrayList = new ArrayList();
      KLineData klineData = (KLineData) null;
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        bool flag;
        if (klineData != null)
        {
          DateTime dateTime1 = new DateTime((int) klineData.date / 10000, (int) klineData.date / 100 % 100, (int) klineData.date % 100);
          int num1 = 0;
          if (dateTime1.DayOfWeek == DayOfWeek.Monday)
            num1 = 2;
          else if (dateTime1.DayOfWeek == DayOfWeek.Tuesday)
            num1 = 3;
          else if (dateTime1.DayOfWeek == DayOfWeek.Wednesday)
            num1 = 4;
          else if (dateTime1.DayOfWeek == DayOfWeek.Thursday)
            num1 = 5;
          else if (dateTime1.DayOfWeek == DayOfWeek.Friday)
            num1 = 6;
          else if (dateTime1.DayOfWeek == DayOfWeek.Saturday)
            num1 = 7;
          else if (dateTime1.DayOfWeek == DayOfWeek.Sunday)
            num1 = 1;
          DateTime dateTime2 = new DateTime((int) this.m_kData[index].date / 10000, (int) this.m_kData[index].date / 100 % 100, (int) this.m_kData[index].date % 100);
          int num2 = 0;
          if (dateTime2.DayOfWeek == DayOfWeek.Sunday)
            num2 = 1;
          else if (dateTime2.DayOfWeek == DayOfWeek.Monday)
            num2 = 2;
          else if (dateTime2.DayOfWeek == DayOfWeek.Tuesday)
            num2 = 3;
          else if (dateTime2.DayOfWeek == DayOfWeek.Wednesday)
            num2 = 4;
          else if (dateTime2.DayOfWeek == DayOfWeek.Thursday)
            num2 = 5;
          else if (dateTime2.DayOfWeek == DayOfWeek.Friday)
            num2 = 6;
          else if (dateTime2.DayOfWeek == DayOfWeek.Saturday)
            num2 = 7;
          if (num1 >= num2)
          {
            flag = true;
          }
          else
          {
            dateTime1 = dateTime1.AddDays(7.0);
            dateTime1.CompareTo(dateTime2);
            flag = dateTime1.CompareTo(dateTime2) < 0;
          }
        }
        else
          flag = true;
        if (flag)
        {
          if (klineData != null)
          {
            if (klineData.totalAmount > 0L)
              klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
            arrayList.Add((object) klineData);
          }
          klineData = new KLineData();
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.date = this.m_kData[index].date;
          klineData.highPrice = this.m_kData[index].highPrice;
          klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.openPrice = this.m_kData[index].openPrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount = this.m_kData[index].totalAmount;
          klineData.totalMoney = this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
        }
        else
        {
          klineData.date = this.m_kData[index].date;
          if ((double) this.m_kData[index].highPrice > (double) klineData.highPrice)
            klineData.highPrice = this.m_kData[index].highPrice;
          if ((double) this.m_kData[index].lowPrice < (double) klineData.lowPrice)
            klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount += this.m_kData[index].totalAmount;
          klineData.totalMoney += this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
        }
      }
      if (klineData != null)
      {
        if (klineData.totalAmount > 0L)
          klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData);
      }
      this.m_kData = new KLineData[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        this.m_kData[index] = (KLineData) arrayList[index];
    }

    private void MakeMonth()
    {
      ArrayList arrayList = new ArrayList();
      KLineData klineData = (KLineData) null;
      int num1 = -1;
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        int num2 = (int) this.m_kData[index].date / 100;
        if (num2 != num1)
        {
          if (klineData != null)
          {
            if (klineData.totalAmount > 0L)
              klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
            arrayList.Add((object) klineData);
          }
          klineData = new KLineData();
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.highPrice = this.m_kData[index].highPrice;
          klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.openPrice = this.m_kData[index].openPrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount = this.m_kData[index].totalAmount;
          klineData.totalMoney = this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
          klineData.date = (long) (num2 * 100);
          num1 = num2;
        }
        else
        {
          if ((double) this.m_kData[index].highPrice > (double) klineData.highPrice)
            klineData.highPrice = this.m_kData[index].highPrice;
          if ((double) this.m_kData[index].lowPrice < (double) klineData.lowPrice)
            klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount += this.m_kData[index].totalAmount;
          klineData.totalMoney += this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
        }
      }
      if (klineData != null)
      {
        if (klineData.totalAmount > 0L)
          klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData);
      }
      this.m_kData = new KLineData[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        this.m_kData[index] = (KLineData) arrayList[index];
    }

    private void MakeQUARTER()
    {
      ArrayList arrayList = new ArrayList();
      KLineData klineData = (KLineData) null;
      int num1 = -1;
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        int num2 = (int) this.m_kData[index].date / 100;
        if (num2 > num1)
        {
          if (klineData != null)
          {
            if (klineData.totalAmount > 0L)
              klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
            arrayList.Add((object) klineData);
          }
          klineData = new KLineData();
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.highPrice = this.m_kData[index].highPrice;
          klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.openPrice = this.m_kData[index].openPrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount = this.m_kData[index].totalAmount;
          klineData.totalMoney = this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
          klineData.date = (long) (num2 * 100);
          num1 = num2 + 3;
        }
        else
        {
          if ((double) this.m_kData[index].highPrice > (double) klineData.highPrice)
            klineData.highPrice = this.m_kData[index].highPrice;
          if ((double) this.m_kData[index].lowPrice < (double) klineData.lowPrice)
            klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount += this.m_kData[index].totalAmount;
          klineData.totalMoney += this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
        }
      }
      if (klineData != null)
      {
        if (klineData.totalAmount > 0L)
          klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData);
      }
      this.m_kData = new KLineData[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        this.m_kData[index] = (KLineData) arrayList[index];
    }

    private void MakeMinCycle(int iMin)
    {
      try
      {
        int index1 = iMin - 1;
        this.dateTimeRange = new DateTime();
        ArrayList arrayList = new ArrayList();
        KLineData klineData = (KLineData) null;
        long num1 = -1L;
        CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
        for (int index2 = 0; index2 < this.m_kData.Length; ++index2)
        {
          long num2;
          if (this.m_kData.Length <= iMin)
            num2 = this.m_kData[this.m_kData.Length - 1].date;
          else if (index1 > this.m_kData.Length - 1)
          {
            long num3 = this.m_kData[index1 - (iMin - 1)].date;
            DateTime dateTime = TradeTimeVO.HHmmToDateTime((int) (num3 / 10000L), (int) (num3 % 10000L));
            dateTime = dateTime.AddMinutes((double) iMin);
            dateTime = dateTime.AddMinutes(-1.0);
            dateTime = dateTime.AddSeconds((double) -dateTime.Second);
            num2 = (long) TradeTimeVO.TimeStringToInt(dateTime.ToString("yyyy-MM-dd")) * 10000L + (long) (TradeTimeVO.TimeStringToInt(dateTime.ToString("HH:mm:ss")) / 100);
          }
          else
            num2 = this.m_kData[index1].date;
          if (num2 > num1)
          {
            if (klineData != null)
            {
              if (klineData.totalAmount > 0L)
                klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
              arrayList.Add((object) klineData);
            }
            klineData = new KLineData();
            klineData.closePrice = this.m_kData[index2].closePrice;
            klineData.highPrice = this.m_kData[index2].highPrice;
            klineData.lowPrice = this.m_kData[index2].lowPrice;
            klineData.openPrice = this.m_kData[index2].openPrice;
            klineData.balancePrice = this.m_kData[index2].balancePrice;
            klineData.totalAmount = this.m_kData[index2].totalAmount;
            klineData.totalMoney = this.m_kData[index2].totalMoney;
            klineData.reserveCount = this.m_kData[index2].reserveCount;
            klineData.date = num2;
            num1 = num2;
          }
          else
          {
            if ((double) this.m_kData[index2].highPrice > (double) klineData.highPrice)
              klineData.highPrice = this.m_kData[index2].highPrice;
            if ((double) this.m_kData[index2].lowPrice < (double) klineData.lowPrice)
              klineData.lowPrice = this.m_kData[index2].lowPrice;
            klineData.closePrice = this.m_kData[index2].closePrice;
            klineData.balancePrice = this.m_kData[index2].balancePrice;
            klineData.totalAmount += this.m_kData[index2].totalAmount;
            klineData.totalMoney += this.m_kData[index2].totalMoney;
            klineData.reserveCount = this.m_kData[index2].reserveCount;
          }
          if (index1 - index2 == 0)
            index1 += iMin;
        }
        if (klineData != null)
        {
          if (klineData.totalAmount > 0L)
            klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
          arrayList.Add((object) klineData);
        }
        this.m_kData = new KLineData[arrayList.Count];
        for (int index2 = 0; index2 < arrayList.Count; ++index2)
          this.m_kData[index2] = (KLineData) arrayList[index2];
      }
      catch (Exception ex)
      {
        WriteLog.WriteMsg("MakeMinCycle异常" + ex.Message);
      }
    }

    private void Make5MinCycle(int iMin)
    {
      this.dateTimeRange = new DateTime();
      ArrayList arrayList = new ArrayList();
      KLineData klineData = (KLineData) null;
      long num = -1L;
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        long currentDateTime = this.GetCurrentDateTime(this.m_kData[index].date, iMin);
        if (currentDateTime != num)
        {
          if (klineData != null)
          {
            if (klineData.totalAmount > 0L)
              klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
            arrayList.Add((object) klineData);
          }
          klineData = new KLineData();
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.highPrice = this.m_kData[index].highPrice;
          klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.openPrice = this.m_kData[index].openPrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount = this.m_kData[index].totalAmount;
          klineData.totalMoney = this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
          klineData.date = currentDateTime;
          num = currentDateTime;
        }
        else
        {
          if ((double) this.m_kData[index].highPrice > (double) klineData.highPrice)
            klineData.highPrice = this.m_kData[index].highPrice;
          if ((double) this.m_kData[index].lowPrice < (double) klineData.lowPrice)
            klineData.lowPrice = this.m_kData[index].lowPrice;
          klineData.closePrice = this.m_kData[index].closePrice;
          klineData.balancePrice = this.m_kData[index].balancePrice;
          klineData.totalAmount += this.m_kData[index].totalAmount;
          klineData.totalMoney += this.m_kData[index].totalMoney;
          klineData.reserveCount = this.m_kData[index].reserveCount;
        }
      }
      if (klineData != null)
      {
        if (klineData.totalAmount > 0L)
          klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
        arrayList.Add((object) klineData);
      }
      this.m_kData = new KLineData[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        this.m_kData[index] = (KLineData) arrayList[index];
      if (iMin != 60)
        return;
      this.m_product.hrKLine = this.m_kData;
    }

    private void MakeDay(int iday)
    {
      int index1 = iday - 1;
      ArrayList arrayList = new ArrayList();
      KLineData klineData = (KLineData) null;
      long num1 = -1L;
      CodeTable codeTable = (CodeTable) this.parent.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
      try
      {
        for (int index2 = 0; index2 < this.m_kData.Length; ++index2)
        {
          long num2 = this.m_kData.Length > iday ? (index1 <= this.m_kData.Length - 1 ? this.m_kData[index1].date : this.m_kData[this.m_kData.Length - 1].date) : this.m_kData[this.m_kData.Length - 1].date;
          if (num2 > num1)
          {
            if (klineData != null)
            {
              if (klineData.totalAmount > 0L)
                klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
              arrayList.Add((object) klineData);
            }
            klineData = new KLineData();
            klineData.closePrice = this.m_kData[index2].closePrice;
            klineData.highPrice = this.m_kData[index2].highPrice;
            klineData.lowPrice = this.m_kData[index2].lowPrice;
            klineData.openPrice = this.m_kData[index2].openPrice;
            klineData.balancePrice = this.m_kData[index2].balancePrice;
            klineData.totalAmount = this.m_kData[index2].totalAmount;
            klineData.totalMoney = this.m_kData[index2].totalMoney;
            klineData.reserveCount = this.m_kData[index2].reserveCount;
            klineData.date = num2;
            num1 = num2;
          }
          else
          {
            if ((double) this.m_kData[index2].highPrice > (double) klineData.highPrice)
              klineData.highPrice = this.m_kData[index2].highPrice;
            if ((double) this.m_kData[index2].lowPrice < (double) klineData.lowPrice)
              klineData.lowPrice = this.m_kData[index2].lowPrice;
            klineData.closePrice = this.m_kData[index2].closePrice;
            klineData.balancePrice = this.m_kData[index2].balancePrice;
            klineData.totalAmount += this.m_kData[index2].totalAmount;
            klineData.totalMoney += this.m_kData[index2].totalMoney;
            klineData.reserveCount = this.m_kData[index2].reserveCount;
          }
          if (index1 - index2 == 0)
            index1 += iday;
        }
        if (klineData != null)
        {
          if (klineData.totalAmount > 0L)
            klineData.balancePrice = codeTable == null ? (float) klineData.totalMoney / (float) klineData.totalAmount : (float) klineData.totalMoney / (float) klineData.totalAmount / codeTable.fUnit;
          arrayList.Add((object) klineData);
        }
        this.m_kData = new KLineData[arrayList.Count];
        for (int index2 = 0; index2 < arrayList.Count; ++index2)
          this.m_kData[index2] = (KLineData) arrayList[index2];
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "MakeDay异常" + ex.Message);
      }
    }

    private long GetCurrentDateTime(long date, int iMin)
    {
      return Convert.ToInt64(this.GetDateTimePlus(date, iMin, true).ToString("yyyyMMddHHmm"));
    }

    private DateTime GetDateTime(long date, int iMin, bool isHistoryData)
    {
      DateTime dateTime1 = TradeTimeVO.HHmmToDateTime((int) (date / 10000L), (int) (date % 10000L));
      int num1 = 0;
      TradeTimeVO[] timeRange = M_Common.getTimeRange(this.m_product.commodityInfo, this.parent.m_hqClient);
      if (timeRange.Length > 0)
        num1 = timeRange[0].beginTime / 100;
      int num2 = -1;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[index].beginDate, timeRange[index].beginTime * 100);
        DateTime dateTime3 = TradeTimeVO.HHmmssToDateTime(timeRange[index].endDate, timeRange[index].endTime * 100);
        if (index == 0 && dateTime1.CompareTo(dateTime2) == -1)
        {
          int num3 = (int) date / 10000;
          if (!isHistoryData)
          {
            num2 = 4;
            dateTime1 = dateTime2;
            break;
          }
          num2 = 3;
          break;
        }
        if (dateTime1.CompareTo(dateTime2) >= 0 && dateTime1.CompareTo(dateTime3) <= 0)
        {
          num2 = dateTime1.CompareTo(dateTime2) != 0 ? (dateTime1.CompareTo(dateTime3) != 0 ? 0 : 2) : 1;
          break;
        }
      }
      if (num2 == -1)
      {
        for (int index = timeRange.Length - 1; index >= 0; --index)
        {
          DateTime dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[index].endDate, timeRange[index].endTime * 100);
          if (dateTime1.CompareTo(dateTime2) > 0)
          {
            num2 = 2;
            dateTime1 = dateTime2;
            break;
          }
        }
      }
      if (num2 == 0 || num2 == 1)
      {
        if (iMin == 5)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
        else if (iMin > 60)
        {
          int num3 = iMin / 60;
          int num4 = dateTime1.Hour;
          if (num4 == 22)
            num4 = 22;
          if (dateTime1.Minute == 0)
          {
            if ((num4 - num1) % num3 != 0)
            {
              dateTime1 = dateTime1.AddMinutes((double) iMin);
              dateTime1 = dateTime1.AddHours((double) -((num4 - num1) % num3));
              dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
              dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
            }
          }
          else
          {
            dateTime1 = dateTime1.AddMinutes((double) iMin);
            dateTime1 = dateTime1.AddHours((double) -((num4 - num1) % num3));
            dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
            dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
          }
        }
        else if (dateTime1.Minute % iMin > 0)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
      }
      else if (num2 == 2)
      {
        if (dateTime1.Minute % iMin > 0)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
      }
      else if (num2 == 3)
      {
        if (dateTime1.Minute % iMin > 0)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
      }
      else if (num2 == 4)
      {
        dateTime1 = dateTime1.AddMinutes((double) iMin);
        dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
        dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
      }
      return dateTime1;
    }

    private void DrawCycle(Graphics g)
    {
      string str = "";
      switch (this.parent.m_hqClient.m_iKLineCycle)
      {
        case 1:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr_DayLine");
          break;
        case 2:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr_WeekLine");
          break;
        case 3:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr_MonthLine");
          break;
        case 4:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr_QuarterLine");
          break;
        case 5:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__1MinLine");
          break;
        case 6:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__3MinLine");
          break;
        case 7:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinLine");
          break;
        case 8:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__15MinLine");
          break;
        case 9:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__30MinLine");
          break;
        case 10:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__60MinLine");
          break;
        case 11:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__2HrLine");
          break;
        case 12:
          str = this.pluginInfo.HQResourceManager.GetString("HQStr__4HrLine");
          break;
        case 13:
          str = this.parent.m_hqClient.KLineValue.ToString() + this.pluginInfo.HQResourceManager.GetString("HQStr_AnyDayLine");
          break;
        case 14:
          str = this.parent.m_hqClient.KLineValue.ToString() + this.pluginInfo.HQResourceManager.GetString("HQStr_AnyMinLine");
          break;
      }
      Font font = new Font("宋体", 10f, FontStyle.Regular);
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
      int num1 = this.m_rcPane[0].X - (int) g.MeasureString(str, font).Width - 1;
      int num2 = this.m_rcPane[0].Y + 2;
      g.DrawString(str, font, (Brush) solidBrush, (float) num1, (float) num2);
      solidBrush.Dispose();
      font.Dispose();
    }

    private void DrawLabel()
    {
      using (Graphics mGraphics = this.parent.m_hqForm.M_Graphics)
      {
        this.parent.m_hqForm.TranslateTransform(mGraphics);
        this.DrawLabel(mGraphics);
      }
    }

    public void DrawCursor(int iNewPos)
    {
      using (Graphics mGraphics = this.parent.m_hqForm.M_Graphics)
      {
        if (!this.parent.m_hqForm.IsEndPaint)
          return;
        if (this.m_iPos != -1)
        {
          int num = this.m_rcPane[0].X + (int) ((double) this.m_pos.m_Ratio / 2.0 + (double) this.m_iPos * (double) this.m_pos.m_Ratio);
          GDIDraw.XorLine(mGraphics, num, this.m_rcPane[0].Y + 1, num, this.m_rcPane[2].Y + this.m_rcPane[2].Height - 1, SetInfo.RHColor.clCursor, this.parent.m_hqForm.ScrollOffset);
          this.m_indicator[0].DrawCursor(mGraphics, this.m_iPos);
        }
        if (iNewPos == -1)
          return;
        this.m_iPos = iNewPos;
        int num1 = this.m_rcPane[0].X + (int) ((double) this.m_pos.m_Ratio / 2.0 + (double) this.m_iPos * (double) this.m_pos.m_Ratio);
        GDIDraw.XorLine(mGraphics, num1, this.m_rcPane[0].Y + 1, num1, this.m_rcPane[2].Y + this.m_rcPane[2].Height - 1, SetInfo.RHColor.clCursor, this.parent.m_hqForm.ScrollOffset);
        this.m_indicator[0].DrawCursor(mGraphics, this.m_iPos);
      }
    }

    public void DrawLabel(Graphics g)
    {
      if (this.m_kData == null || this.m_kData.Length == 0 || g == null)
        return;
      int iIndex = this.m_iPos >= 0 ? this.m_pos.m_Begin + this.m_iPos : this.m_kData.Length - 1;
      for (int index = 0; index < 3; ++index)
      {
        if (this.m_indicator[index] != null)
          this.m_indicator[index].DrawTitle(g, iIndex);
      }
      if (this.m_iPos < 0)
        return;
      Rectangle rectangle = new Rectangle(this.m_rcLabel.X - 1, this.m_rcLabel.Y - 1, this.m_rcLabel.Width + 1, this.m_rcLabel.Height + 1);
      using (Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height))
      {
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        {
          graphics.Clear(SetInfo.RHColor.clBackGround);
          SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
          Pen pen = new Pen(SetInfo.RHColor.clNumber);
          graphics.DrawRectangle(pen, 0, 0, this.m_rcLabel.Width, this.m_rcLabel.Height);
          Font font1 = new Font("宋体", 10f, FontStyle.Regular);
          int num1 = 1;
          int num2 = 1;
          solidBrush.Color = SetInfo.RHColor.clItem;
          graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Date"), font1, (Brush) solidBrush, (float) num1, (float) num2);
          int num3 = num2 + font1.Height;
          if (iIndex >= this.m_kData.Length)
            return;
          string str1 = Convert.ToString(this.m_kData[iIndex].date);
          switch (this.parent.m_hqClient.m_iKLineCycle)
          {
            case 1:
            case 2:
            case 13:
              int num4 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str1, font1).Width - 1;
              solidBrush.Color = SetInfo.RHColor.clEqual;
              graphics.DrawString(str1, font1, (Brush) solidBrush, (float) num4, (float) num3);
              if (this.parent.m_hqClient.m_iKLineCycle >= 5 && this.parent.m_hqClient.m_iKLineCycle <= 12 || this.parent.m_hqClient.m_iKLineCycle == 14)
              {
                string str2 = Convert.ToString(this.m_kData[iIndex].date);
                int num5;
                if (str2.Length > 8)
                {
                  str2 = str2.Substring(8, 2) + ":" + str2.Substring(10);
                  num3 += font1.Height;
                  num5 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str2, font1).Width - 1;
                }
                else
                {
                  num3 += font1.Height;
                  num5 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str2, font1).Width - 1;
                }
                graphics.DrawString(str2, font1, (Brush) solidBrush, (float) num5, (float) num3);
              }
              int num6 = rectangle.X + 1;
              int num7 = num3 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Open"), font1, (Brush) solidBrush, (float) num6, (float) num7);
              float num8 = iIndex <= 0 ? this.m_kData[iIndex].openPrice : this.m_kData[iIndex - 1].balancePrice;
              int num9 = num7 + font1.Height;
              string str3 = M_Common.FloatToString((double) this.m_kData[iIndex].openPrice, this.m_iPrecision);
              solidBrush.Color = (double) this.m_kData[iIndex].openPrice <= (double) num8 ? ((double) this.m_kData[iIndex].openPrice >= (double) num8 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num10 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str3, font1).Width - 1;
              graphics.DrawString(str3, font1, (Brush) solidBrush, (float) num10, (float) num9);
              int num11 = rectangle.X + 1;
              int num12 = num9 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_High"), font1, (Brush) solidBrush, (float) num11, (float) num12);
              int num13 = num12 + font1.Height;
              string str4 = M_Common.FloatToString((double) this.m_kData[iIndex].highPrice, this.m_iPrecision);
              solidBrush.Color = (double) this.m_kData[iIndex].highPrice <= (double) num8 ? ((double) this.m_kData[iIndex].highPrice >= (double) num8 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num14 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str4, font1).Width - 1;
              graphics.DrawString(str4, font1, (Brush) solidBrush, (float) num14, (float) num13);
              int num15 = rectangle.X + 1;
              int num16 = num13 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Low"), font1, (Brush) solidBrush, (float) num15, (float) num16);
              int num17 = num16 + font1.Height;
              string str5 = M_Common.FloatToString((double) this.m_kData[iIndex].lowPrice, this.m_iPrecision);
              solidBrush.Color = (double) this.m_kData[iIndex].lowPrice <= (double) num8 ? ((double) this.m_kData[iIndex].lowPrice >= (double) num8 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num18 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str5, font1).Width - 1;
              graphics.DrawString(str5, font1, (Brush) solidBrush, (float) num18, (float) num17);
              int num19 = rectangle.X + 1;
              int num20 = num17 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Close"), font1, (Brush) solidBrush, (float) num19, (float) num20);
              int num21 = num20 + font1.Height;
              string str6 = M_Common.FloatToString((double) this.m_kData[iIndex].closePrice, this.m_iPrecision);
              solidBrush.Color = (double) this.m_kData[iIndex].closePrice <= (double) num8 ? ((double) this.m_kData[iIndex].closePrice >= (double) num8 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num22 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str6, font1).Width - 1;
              graphics.DrawString(str6, font1, (Brush) solidBrush, (float) num22, (float) num21);
              bool flag = true;
              if (this.parent.m_hqClient.m_iKLineCycle != 1 && (this.parent.m_hqClient.getProductType(this.m_product.commodityInfo) == 2 || this.parent.m_hqClient.getProductType(this.m_product.commodityInfo) == 3))
                flag = false;
              int num23 = rectangle.X + 1;
              int num24 = num21 + font1.Height;
              if (flag)
              {
                solidBrush.Color = SetInfo.RHColor.clItem;
                graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), font1, (Brush) solidBrush, (float) num23, (float) num24);
              }
              int num25 = num24 + font1.Height;
              if (flag)
              {
                str6 = M_Common.FloatToString((double) this.m_kData[iIndex].balancePrice, this.m_iPrecision);
                solidBrush.Color = (double) this.m_kData[iIndex].balancePrice <= (double) num8 ? ((double) this.m_kData[iIndex].balancePrice >= (double) num8 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
                int num5 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str6, font1).Width - 1;
                graphics.DrawString(str6, font1, (Brush) solidBrush, (float) num5, (float) num25);
              }
              if (iIndex > 0 && this.m_kData[iIndex - 1] != null)
              {
                int num5 = rectangle.X + 1;
                int num26 = num25 + font1.Height;
                solidBrush.Color = SetInfo.RHColor.clItem;
                graphics.DrawString("涨幅", font1, (Brush) solidBrush, (float) num5, (float) num26);
                string str2 = "";
                if (this.setInfo.AmountIncrease == "0" || this.setInfo.AmountIncrease == "2")
                {
                  num26 += font1.Height;
                  str6 = M_Common.FloatToString((double) this.m_kData[iIndex].closePrice - (double) this.m_kData[iIndex - 1].balancePrice, this.m_iPrecision);
                  if ((double) this.m_kData[iIndex - 1].balancePrice > 0.0)
                    str2 = ((this.m_kData[iIndex].closePrice - this.m_kData[iIndex - 1].balancePrice) / this.m_kData[iIndex - 1].balancePrice).ToString("P2");
                  solidBrush.Color = (double) this.m_kData[iIndex].balancePrice - (double) this.m_kData[iIndex - 1].balancePrice <= 0.0 ? ((double) this.m_kData[iIndex].balancePrice - (double) this.m_kData[iIndex - 1].balancePrice >= 0.0 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
                }
                else if (this.setInfo.AmountIncrease == "1")
                {
                  num26 += font1.Height;
                  str6 = M_Common.FloatToString((double) this.m_kData[iIndex].closePrice - (double) this.m_kData[iIndex - 1].closePrice, this.m_iPrecision);
                  if ((double) this.m_kData[iIndex - 1].balancePrice > 0.0)
                    str2 = ((this.m_kData[iIndex].closePrice - this.m_kData[iIndex - 1].closePrice) / this.m_kData[iIndex - 1].closePrice).ToString("P2");
                  solidBrush.Color = (double) this.m_kData[iIndex].closePrice - (double) this.m_kData[iIndex - 1].closePrice <= 0.0 ? ((double) this.m_kData[iIndex].closePrice - (double) this.m_kData[iIndex - 1].closePrice >= 0.0 ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
                }
                int num27 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str6, font1).Width - 1;
                graphics.DrawString(str6, font1, (Brush) solidBrush, (float) num27, (float) num26);
                num25 = num26 + font1.Height;
                int num28 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str2, font1).Width - 1;
                graphics.DrawString(str2, font1, (Brush) solidBrush, (float) num28, (float) num25);
              }
              int num29 = rectangle.X + 1;
              int num30 = num25 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), font1, (Brush) solidBrush, (float) num29, (float) num30);
              int num31 = num30 + font1.Height;
              string str7 = Convert.ToString(this.m_kData[iIndex].totalAmount);
              solidBrush.Color = SetInfo.RHColor.clVolume;
              int num32 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str7, font1).Width - 1;
              graphics.DrawString(str7, font1, (Brush) solidBrush, (float) num32, (float) num31);
              int num33 = rectangle.X + 1;
              int num34 = num31 + font1.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Money"), font1, (Brush) solidBrush, (float) num33, (float) num34);
              int num35 = num34 + font1.Height;
              string str8 = Convert.ToString((long) this.m_kData[iIndex].totalMoney);
              solidBrush.Color = SetInfo.RHColor.clVolume;
              Font font2 = font1;
              int num36 = (int) font1.Size;
              for (; (double) g.MeasureString(str8, font1).Width > (double) rectangle.Width; font1 = new Font("宋体", (float) num36, FontStyle.Regular))
                --num36;
              int num37 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str8, font1).Width - 1;
              graphics.DrawString(str8, font1, (Brush) solidBrush, (float) num37, (float) num35);
              Font font3 = font2;
              int num38 = rectangle.X + 1;
              int num39 = num35 + font3.Height;
              solidBrush.Color = SetInfo.RHColor.clItem;
              graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), font3, (Brush) solidBrush, (float) num38, (float) num39);
              int num40 = num39 + font3.Height;
              string str9 = Convert.ToString(this.m_kData[iIndex].reserveCount);
              solidBrush.Color = SetInfo.RHColor.clReserve;
              int num41 = rectangle.X + rectangle.Width - (int) graphics.MeasureString(str9, font3).Width - 1;
              graphics.DrawString(str9, font3, (Brush) solidBrush, (float) num41, (float) num40);
              font3.Dispose();
              pen.Dispose();
              solidBrush.Dispose();
              g.DrawImage((Image) bitmap, rectangle.X, rectangle.Y);
              break;
            case 3:
            case 4:
              if (str1.Length >= 6)
              {
                str1 = str1.Substring(0, 6);
                goto case 1;
              }
              else
                goto case 1;
            default:
              if (str1.Length >= 4)
              {
                str1 = str1.Substring(0, 8);
                goto case 1;
              }
              else
                goto case 1;
          }
        }
      }
    }

    public void CreateIndicator()
    {
      int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
      if (this.parent.m_hqClient.m_strIndicator.Equals("ASI"))
        this.m_indicator[2] = (IndicatorBase) new ASI(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("BIAS"))
        this.m_indicator[2] = (IndicatorBase) new BIAS(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("BRAR"))
        this.m_indicator[2] = (IndicatorBase) new BRAR(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("BOLL"))
        this.m_indicator[2] = (IndicatorBase) new BOLL(this.m_pos, precision, this.parent.m_hqForm);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("CCI"))
        this.m_indicator[2] = (IndicatorBase) new CCI(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("CR"))
        this.m_indicator[2] = (IndicatorBase) new CR(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("DMA"))
        this.m_indicator[2] = (IndicatorBase) new DMA(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("DMI"))
        this.m_indicator[2] = (IndicatorBase) new DMI(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("EMV"))
        this.m_indicator[2] = (IndicatorBase) new EMV(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("EXPMA"))
        this.m_indicator[2] = (IndicatorBase) new EXPMA(this.m_pos, precision, this.parent.m_hqForm);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("KDJ"))
        this.m_indicator[2] = (IndicatorBase) new KDJ(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("MACD"))
        this.m_indicator[2] = (IndicatorBase) new MACD(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("MIKE"))
        this.m_indicator[2] = (IndicatorBase) new MIKE(this.m_pos, precision, this.parent.m_hqForm);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("OBV"))
        this.m_indicator[2] = (IndicatorBase) new OBV(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("ORDER"))
        this.m_indicator[2] = (IndicatorBase) new Reserve(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("PSY"))
        this.m_indicator[2] = (IndicatorBase) new PSY(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("ROC"))
        this.m_indicator[2] = (IndicatorBase) new ROC(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("RSI"))
        this.m_indicator[2] = (IndicatorBase) new RSI(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("SAR"))
        this.m_indicator[2] = (IndicatorBase) new SAR(this.m_pos, precision, this.parent.m_hqForm);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("TRIX"))
        this.m_indicator[2] = (IndicatorBase) new TRIX(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("VR"))
        this.m_indicator[2] = (IndicatorBase) new VR(this.m_pos, precision);
      else if (this.parent.m_hqClient.m_strIndicator.Equals("W%R"))
      {
        this.m_indicator[2] = (IndicatorBase) new W_R(this.m_pos, precision);
      }
      else
      {
        if (!this.parent.m_hqClient.m_strIndicator.Equals("WVAD"))
          return;
        this.m_indicator[2] = (IndicatorBase) new WVAD(this.m_pos, precision);
      }
    }

    internal bool MouseLeftClicked(int x, int y)
    {
      if (!this.parent.m_hqForm.IsEndPaint || x < this.m_rcPane[0].X || x > this.m_rcPane[0].X + this.m_rcPane[0].Width)
        return false;
      int iNewPos = (int) ((double) (x - this.m_rcPane[0].X) / (double) this.m_pos.m_Ratio);
      if (iNewPos != this.m_iPos && iNewPos >= 0 && iNewPos <= this.m_pos.m_End - this.m_pos.m_Begin)
      {
        this.DrawCursor(iNewPos);
        this.DrawLabel();
      }
      if (this.parent.m_hqForm.IsMultiCycle)
      {
        int clickPointX = this.getClickPointX(x, y);
        if (clickPointX != this.m_iPos)
        {
          this.DrawCursor(clickPointX);
          this.DrawLabel();
        }
      }
      return false;
    }

    public int getClickPointX(int x, int y)
    {
      if (!this.parent.m_hqForm.IsEndPaint || x < this.m_rcPane[0].X || x > this.m_rcPane[0].X + this.m_rcPane[0].Width)
        return -1;
      int num = (int) ((double) (x - this.m_rcPane[0].X) / (double) this.m_pos.m_Ratio);
      if (num <= this.m_pos.m_End - this.m_pos.m_Begin)
        return num;
      return this.m_pos.m_End - this.m_pos.m_Begin;
    }

    internal bool MouseDragged(int x, int y)
    {
      return this.MouseLeftClicked(x, y);
    }

    internal bool KeyPressed(KeyEventArgs e)
    {
      bool flag = false;
      if (!this.parent.isDrawCursor)
      {
        this.parent.isDrawCursor = true;
        this.m_iPos = -1;
      }
      if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Left)
      {
        this.m_pos.m_EndPos += this.m_pos.m_ScreenCount;
        this.m_iPos -= this.m_pos.m_ScreenCount;
        flag = true;
      }
      else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Left)
      {
        if (this.m_pos.m_Begin > 0)
        {
          ++this.m_pos.m_EndPos;
          --this.m_iPos;
          flag = true;
        }
      }
      else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Right)
      {
        if (this.m_pos.m_Begin < this.m_pos.m_MaxPos)
        {
          --this.m_pos.m_EndPos;
          ++this.m_iPos;
          flag = true;
        }
      }
      else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Right)
      {
        this.m_pos.m_EndPos -= this.m_pos.m_ScreenCount;
        this.m_iPos += this.m_pos.m_ScreenCount;
        flag = true;
      }
      if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
      {
        this.parent.isDrawCursor = false;
        this.m_iPos = -1;
      }
      switch (e.KeyData)
      {
        case Keys.Escape:
          if (this.m_iPos != -1)
          {
            this.DrawCursor(-1);
            this.m_iPos = -1;
            flag = true;
            break;
          }
          break;
        case Keys.End:
          this.ChangeIndicator(true);
          flag = true;
          break;
        case Keys.Home:
          this.ChangeIndicator(false);
          flag = true;
          break;
        case Keys.Left:
          flag = this.ChangePos(true);
          break;
        case Keys.Up:
          flag = this.ChangeRatio(true);
          if (flag)
          {
            if (!this.ChangeRatio(true))
              this.parent.setMenuEnable("zoomin", false);
            else
              this.ChangeRatio(false);
            this.parent.setMenuEnable("zoomout", true);
            break;
          }
          break;
        case Keys.Right:
          flag = this.ChangePos(false);
          break;
        case Keys.Down:
          flag = this.ChangeRatio(false);
          if (flag)
          {
            if (!this.ChangeRatio(false))
              this.parent.setMenuEnable("zoomout", false);
            else
              this.ChangeRatio(true);
            this.parent.setMenuEnable("zoomin", true);
            break;
          }
          break;
        case Keys.F8:
          this.ChangeCycle();
          flag = true;
          break;
      }
      return flag;
    }

    private void ChangeCycle()
    {
      if (this.parent.m_hqClient.m_iKLineCycle >= 12)
        this.parent.m_hqClient.m_iKLineCycle = 1;
      else
        ++this.parent.m_hqClient.m_iKLineCycle;
      this.m_iPos = -1;
      this.m_pos.m_EndPos = 0;
      this.m_pos.m_MaxPos = 0;
      this.m_pos.m_End = 0;
      this.m_pos.m_Begin = 0;
      this.parent.AskForKLine();
    }

    public void ChangeKLineType(int iType)
    {
      int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
      this.m_indicator[0] = (IndicatorBase) new MA(this.m_pos, iType, precision, this.parent.m_hqForm);
    }

    private bool ChangePos(bool bIsLeft)
    {
      bool flag = false;
      if (!this.parent.m_hqForm.IsEndPaint || this.m_kData == null || this.m_kData.Length == 0)
        return flag;
      if (this.m_iPos == -1)
      {
        if (bIsLeft)
          this.DrawCursor(this.m_pos.m_End - this.m_pos.m_Begin);
        else
          this.DrawCursor(0);
        this.DrawLabel();
      }
      else if (bIsLeft)
      {
        if (this.m_iPos == 0)
        {
          if (this.m_pos.m_Begin <= 0)
            return false;
          ++this.m_pos.m_EndPos;
          flag = true;
        }
        else
        {
          this.DrawCursor(this.m_iPos - 1);
          this.DrawLabel();
        }
      }
      else if (this.m_iPos == this.m_pos.m_End - this.m_pos.m_Begin)
      {
        if (this.m_pos.m_Begin >= this.m_pos.m_MaxPos)
          return false;
        flag = true;
        --this.m_pos.m_EndPos;
      }
      else
      {
        this.DrawCursor(this.m_iPos + 1);
        this.DrawLabel();
      }
      return flag;
    }

    public bool ChangeRatio(bool b)
    {
      if (b)
      {
        if (this.m_pos.m_VirtualRatio >= 60)
          return false;
        this.m_pos.m_VirtualRatio = this.m_pos.m_VirtualRatio + 1;
      }
      else
      {
        if (this.m_pos.m_VirtualRatio <= 10)
          return false;
        this.m_pos.m_VirtualRatio = this.m_pos.m_VirtualRatio - 1;
      }
      this.cache_m_VirtualRatio = this.m_pos.m_VirtualRatio;
      return true;
    }

    private void ChangeIndicator(bool bDown)
    {
      int num = -1;
      for (int index = 0; index < IndicatorBase.INDICATOR_NAME.GetLength(0); ++index)
      {
        if (IndicatorBase.INDICATOR_NAME[index, 0].Equals(this.parent.m_hqClient.m_strIndicator))
        {
          num = index;
          break;
        }
      }
      int index1 = !bDown ? (num >= 1 ? (num - 1) % IndicatorBase.INDICATOR_NAME.Length : IndicatorBase.INDICATOR_NAME.GetLength(0) - 1) : (num + 1) % IndicatorBase.INDICATOR_NAME.GetLength(0);
      this.parent.m_hqClient.m_strIndicator = IndicatorBase.INDICATOR_NAME[index1, 0];
      this.CreateIndicator();
    }

    private DateTime GetDateTimePlus(long date, int iMin, bool isHistoryData)
    {
      DateTime dateTime1 = TradeTimeVO.HHmmToDateTime((int) (date / 10000L), (int) (date % 10000L));
      int num1 = 0;
      TradeTimeVO[] timeRange = M_Common.getTimeRange(this.m_product.commodityInfo, this.parent.m_hqClient);
      if (timeRange.Length > 0)
        num1 = timeRange[0].beginTime / 100;
      int index1 = 1;
      int num2 = -1;
      DateTime dateTime2 = new DateTime();
      DateTime dateTime3 = new DateTime();
      TimeSpan timeSpan1 = TimeSpan.Zero;
      for (int index2 = 0; index2 < timeRange.Length; ++index2)
      {
        DateTime dateTime4 = TradeTimeVO.HHmmssToDateTime(timeRange[index2].beginDate, timeRange[index2].beginTime * 100);
        DateTime dateTime5 = TradeTimeVO.HHmmssToDateTime(timeRange[index2].endDate, timeRange[index2].endTime * 100);
        if (index2 == 0 && dateTime1.CompareTo(dateTime4) == -1)
        {
          int num3 = (int) date / 10000;
          if (!isHistoryData)
          {
            num2 = 4;
            dateTime1 = dateTime4;
            break;
          }
          num2 = 3;
          break;
        }
        if (dateTime1.CompareTo(dateTime4) >= 0 && dateTime1.CompareTo(dateTime5) <= 0)
        {
          index1 = index2;
          num2 = dateTime1.CompareTo(dateTime4) != 0 ? (dateTime1.CompareTo(dateTime5) != 0 ? 0 : 2) : 1;
          break;
        }
      }
      if (num2 == -1)
      {
        for (int index2 = timeRange.Length - 1; index2 >= 0; --index2)
        {
          DateTime dateTime4 = TradeTimeVO.HHmmssToDateTime(timeRange[index2].endDate, timeRange[index2].endTime * 100);
          if (dateTime1.CompareTo(dateTime4) > 0)
          {
            num2 = 2;
            dateTime1 = dateTime4;
            break;
          }
        }
      }
      if (num2 == 0 || num2 == 1)
      {
        if (iMin == 5 || iMin == 1 || iMin == 3)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
        else
        {
          if (iMin > 60)
          {
            if (dateTime1.CompareTo(this.dateTimeRange) < 0)
            {
              dateTime1 = this.dateTimeRange;
              return dateTime1;
            }
            int num3 = iMin / 60;
            int num4 = dateTime1.Hour + num3;
            if (dateTime1.Minute == 0)
            {
              if ((num4 - num1) % num3 != 0)
              {
                dateTime1 = dateTime1.AddMinutes((double) iMin);
                dateTime1 = dateTime1.AddHours((double) -((num4 - num1) % num3));
                dateTime1 = iMin % 60 != 0 ? (iMin % 60 != 1 ? dateTime1.AddMinutes((double) -(dateTime1.Minute % (iMin % 60))) : dateTime1.AddMinutes(-1.0)) : dateTime1.AddMinutes(0.0);
                dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
              }
            }
            else
            {
              dateTime1 = dateTime1.AddMinutes((double) iMin);
              dateTime1 = dateTime1.AddHours((double) -((num4 - num1) % num3));
              dateTime1 = iMin % 60 != 0 ? (iMin % 60 != 1 ? dateTime1.AddMinutes((double) -(dateTime1.Minute % (iMin % 60))) : dateTime1.AddMinutes(-1.0)) : dateTime1.AddMinutes((double) -dateTime1.Minute);
              dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
            }
          }
          else if (dateTime1.Minute % iMin > 0)
          {
            dateTime1 = dateTime1.AddMinutes((double) iMin);
            dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
            dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
          }
          TradeTimeVO.HHmmssToDateTime(timeRange[index1].beginDate, timeRange[index1].beginTime * 100);
          DateTime dateTime4 = TradeTimeVO.HHmmssToDateTime(timeRange[index1].endDate, timeRange[index1].endTime * 100);
          if (dateTime1.CompareTo(dateTime4) > 0)
          {
            if (index1 + 1 < timeRange.Length)
            {
              DateTime dateTime5 = TradeTimeVO.HHmmssToDateTime(timeRange[index1 + 1].beginDate, timeRange[index1 + 1].beginTime * 100);
              TradeTimeVO.HHmmssToDateTime(timeRange[index1 + 1].endDate, timeRange[index1 + 1].endTime * 100);
              TimeSpan timeSpan2 = dateTime1 - dateTime4;
              dateTime1 = dateTime5 + timeSpan2;
              this.dateTimeRange = dateTime1;
            }
            else
            {
              dateTime1 = dateTime4;
              return dateTime1;
            }
          }
        }
      }
      else if (num2 == 2)
      {
        if (dateTime1.CompareTo(this.dateTimeRange) < 0)
        {
          dateTime1 = this.dateTimeRange;
          return dateTime1;
        }
        if (dateTime1.Minute % iMin > 0)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
      }
      else if (num2 == 3)
      {
        if (dateTime1.Minute % iMin > 0)
        {
          dateTime1 = dateTime1.AddMinutes((double) iMin);
          dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
          dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
        }
      }
      else if (num2 == 4)
      {
        dateTime1 = dateTime1.AddMinutes((double) iMin);
        dateTime1 = dateTime1.AddMinutes((double) -(dateTime1.Minute % iMin));
        dateTime1 = dateTime1.AddSeconds((double) -dateTime1.Second);
      }
      return dateTime1;
    }
  }
}
