// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.M_Common
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class M_Common
  {
    public const int TYPE_INVALID = -1;
    public const int TYPE_COMMON = 0;
    public const int TYPE_CANCEL = 1;
    public const int TYPE_INDEX = 2;
    public const int TYPE_INDEX_MAIN = 3;
    public const int TYPE_SERIES = 4;
    public const int TYPE_PAUSE = 5;
    public const int TYPE_FINISHIED = 6;
    public const int TYPE_DECIMAL = 10;
    public const int PRODUCT_CACHENUM = 50;

    public static int GetMinLineIndexFromTime(int yyyymmdd, int hhmmss, TradeTimeVO[] timeRange, int iMinLineInterval)
    {
      if (timeRange == null)
        return 0;
      DateTime dateTime = TradeTimeVO.HHmmssToDateTime(yyyymmdd, hhmmss);
      int num1 = dateTime.Second;
      if (dateTime.Second > 0)
      {
        dateTime = dateTime.AddMinutes(1.0);
        dateTime = dateTime.AddSeconds((double) -dateTime.Second);
      }
      int num2 = TradeTimeVO.GetIndexFromTime(dateTime, timeRange) * (60 / iMinLineInterval);
      if (num1 == 0)
        num1 = 60;
      int num3 = num2 + (num1 - 1) / iMinLineInterval;
      if (num3 < 0)
        num3 = 0;
      return num3;
    }

    public static int GetTimeIndexFromTime(int yyyymmdd, int hhmm, TradeTimeVO[] timeRange)
    {
      if (timeRange == null)
        return 0;
      return TradeTimeVO.GetIndexFromTime(yyyymmdd, hhmm, timeRange);
    }

    public static int GetTimeFromTimeIndex(int index, TradeTimeVO[] timeRange)
    {
      if (timeRange == null)
        return 0;
      return TradeTimeVO.GetTimeFromIndex(index, timeRange);
    }

    public static int GetTimeFromMinLineIndex(int index, TradeTimeVO[] timeRange, int iMinLineInterval)
    {
      if (timeRange == null)
        return 0;
      int num1 = 60 / iMinLineInterval;
      int num2 = TradeTimeVO.GetTimeFromIndex(index / num1, timeRange);
      int num3 = (index % num1 + 1) % num1 * iMinLineInterval;
      if (num3 <= 0)
        return num2 * 100 + num3;
      if (num2 == 0)
        num2 = 2400;
      int num4 = num2 / 100 * 60 + num2 % 100 - 1;
      return (num4 / 60 * 100 + num4 % 60) * 100 + num3;
    }

    public static TradeTimeVO[] getTimeRange(CommodityInfo commodityInfo, HQClientMain client)
    {
      if (client.TimeRange == null)
        return new TradeTimeVO[0];
      if (client.isIndex(commodityInfo))
        return client.TimeRange;
      CodeTable codeTable = (CodeTable) client.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
      if (codeTable == null)
        return new TradeTimeVO[0];
      ArrayList arrayList = new ArrayList();
      for (int index1 = 0; index1 < codeTable.tradeSecNo.Length; ++index1)
      {
        for (int index2 = 0; index2 < client.TimeRange.Length; ++index2)
        {
          if (codeTable.tradeSecNo[index1] == client.TimeRange[index2].orderID)
          {
            arrayList.Add((object) client.TimeRange[index2]);
            break;
          }
        }
      }
      TradeTimeVO[] tradeTimeVoArray = new TradeTimeVO[arrayList.Count];
      for (int index = 0; index < tradeTimeVoArray.Length; ++index)
        tradeTimeVoArray[index] = (TradeTimeVO) arrayList[index];
      return tradeTimeVoArray;
    }

    public static void DrawDotLine(Graphics g, Color color, int x1, int y1, int x2, int y2)
    {
      Pen pen = new Pen(color);
      pen.DashStyle = DashStyle.Dash;
      g.DrawLine(pen, x1, y1, x2, y2);
      pen.Dispose();
    }

    public static DateTime CheckDateTime(DateTime dateTime, TradeTimeVO[] timeRange, int iMin)
    {
      bool flag = false;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime1 = TradeTimeVO.HHmmssToDateTime(timeRange[index].beginDate, timeRange[index].beginTime * 100);
        DateTime dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[index].endDate, timeRange[index].endTime * 100);
        if (dateTime.CompareTo(dateTime1) >= 0 && dateTime.CompareTo(dateTime2) <= 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        for (int index = timeRange.Length - 1; index >= 0; --index)
        {
          DateTime dateTime1 = TradeTimeVO.HHmmssToDateTime(timeRange[index].endDate, timeRange[index].endTime * 100);
          if (dateTime.CompareTo(dateTime1) > 0)
          {
            dateTime = dateTime1;
            break;
          }
        }
      }
      return dateTime;
    }

    public static string FloatToString(double f, int iPrecision)
    {
      if (iPrecision == 0)
        return f.ToString("0");
      string format = "0.";
      for (int index = 0; index < iPrecision; ++index)
        format += "0";
      return f.ToString(format);
    }
  }
}
