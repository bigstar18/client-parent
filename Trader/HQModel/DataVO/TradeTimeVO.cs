// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.TradeTimeVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;
using System.Text;
using TPME.Log;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class TradeTimeVO
  {
    public DateTime modifytime = new DateTime();
    public int orderID;
    public int beginDate;
    public int beginTime;
    public int endDate;
    public int endTime;
    public int tradeDate;
    public int status;

    public override string ToString()
    {
      string str = "\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("**TradeTimeVO**" + str);
      stringBuilder.Append("OrderID:" + (object) this.orderID + str);
      stringBuilder.Append("BeginTime:" + (object) this.beginTime + str);
      stringBuilder.Append("EndTime:" + (object) this.endTime + str);
      stringBuilder.Append("Status:" + (object) this.status + str);
      stringBuilder.Append("Modifytime:" + (object) this.modifytime + str);
      stringBuilder.Append(str);
      return stringBuilder.ToString();
    }

    public static string HHMMSSIntToString(int iTime)
    {
      string str1 = string.Empty;
      string str2 = Convert.ToString(iTime);
      while (str2.Length < 6)
        str2 = "0" + str2;
      return str2.Substring(0, 2) + ":" + str2.Substring(2, 2) + ":" + str2.Substring(4);
    }

    public static string HHMMIntToString(int iTime)
    {
      string str1 = string.Empty;
      string str2 = Convert.ToString(iTime);
      while (str2.Length < 4)
        str2 = "0" + str2;
      return str2.Substring(0, 2) + ":" + str2.Substring(2);
    }

    public static int TimeStringToInt(string strTime)
    {
      strTime = strTime.Replace(":", "");
      strTime = strTime.Replace("-", "");
      return Convert.ToInt32(strTime);
    }

    public static DateTime HHmmToDateTime(int yyyyMMdd, int HHmm)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = Convert.ToString(yyyyMMdd);
      while (str3.Length < 6)
        str3 = "0" + str3;
      string str4 = Convert.ToString(HHmm);
      while (str4.Length < 4)
        str4 = "0" + str4;
      return DateTime.ParseExact(str3 + str4, "yyyyMMddHHmm", (IFormatProvider) null);
    }

    public static DateTime HHmmssToDateTime(int yyyyMMdd, int HHmmss)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = Convert.ToString(yyyyMMdd);
      while (str3.Length < 6)
        str3 = "0" + str3;
      string str4 = Convert.ToString(HHmmss);
      while (str4.Length < 6)
        str4 = "0" + str4;
      if (yyyyMMdd != 0 || HHmmss != 0)
        return DateTime.ParseExact(str3 + str4, "yyyyMMddHHmmss", (IFormatProvider) null);
      Logger.wirte(MsgType.Error, "某个市场时间为零，或者其他地方时间为0，转换的时候报错！");
      return DateTime.Now;
    }

    public static int GetTotalMinute(TradeTimeVO[] timeRange)
    {
      int num = 0;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime = TradeTimeVO.HHmmToDateTime(timeRange[index].beginDate, timeRange[index].beginTime);
        TimeSpan timeSpan = TradeTimeVO.HHmmToDateTime(timeRange[index].endDate, timeRange[index].endTime).Subtract(dateTime);
        num += (int) timeSpan.TotalMinutes;
      }
      return num;
    }

    public static int GetTimeFromIndex(int iIndex, TradeTimeVO[] timeRange)
    {
      int num1 = iIndex + 1;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime = TradeTimeVO.HHmmToDateTime(timeRange[index].beginDate, timeRange[index].beginTime);
        int num2 = (int) TradeTimeVO.HHmmToDateTime(timeRange[index].endDate, timeRange[index].endTime).Subtract(dateTime).TotalMinutes;
        if (num2 >= num1)
          return TradeTimeVO.TimeStringToInt(dateTime.AddMinutes((double) num1).ToString("HH:mm"));
        num1 -= num2;
      }
      return -1;
    }

    public static DateTime GetDateTimeFromIndex(int iIndex, TradeTimeVO[] timeRange)
    {
      if (timeRange.Length <= 0)
        return new DateTime();
      DateTime dateTime1 = TradeTimeVO.HHmmToDateTime(timeRange[timeRange.Length - 1].endDate, timeRange[timeRange.Length - 1].endTime);
      int num1 = iIndex + 1;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime2 = TradeTimeVO.HHmmToDateTime(timeRange[index].beginDate, timeRange[index].beginTime);
        int num2 = (int) TradeTimeVO.HHmmToDateTime(timeRange[index].endDate, timeRange[index].endTime).Subtract(dateTime2).TotalMinutes;
        if (num2 >= num1)
          return dateTime2.AddMinutes((double) num1);
        num1 -= num2;
      }
      return dateTime1;
    }

    public static int GetIndexFromTime(int iDate, int iTime, TradeTimeVO[] timeRange)
    {
      return TradeTimeVO.GetIndexFromTime(TradeTimeVO.HHmmToDateTime(iDate, iTime), timeRange);
    }

    public static int GetIndexFromTime(DateTime dateTime, TradeTimeVO[] timeRange)
    {
      int num = -1;
      for (int index = 0; index < timeRange.Length; ++index)
      {
        DateTime dateTime1 = TradeTimeVO.HHmmToDateTime(timeRange[index].beginDate, timeRange[index].beginTime);
        DateTime dateTime2 = TradeTimeVO.HHmmToDateTime(timeRange[index].endDate, timeRange[index].endTime);
        if (dateTime1.Subtract(dateTime).TotalSeconds > 0.0)
          return num;
        if (dateTime2.CompareTo(dateTime) >= 0 && dateTime1.CompareTo(dateTime) <= 0)
        {
          num += (int) dateTime.Subtract(dateTime1).TotalMinutes;
          break;
        }
        num += (int) dateTime2.Subtract(dateTime1).TotalMinutes;
      }
      if (num < 0)
        num = 0;
      return num;
    }
  }
}
