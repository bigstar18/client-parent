// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.Service.HQServiceUtil
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;

namespace Gnnt.MEBS.HQModel.Service
{
  internal class HQServiceUtil
  {
    public static DateTime getDate(int date, int time)
    {
      DateTime dateTime = new DateTime();
      dateTime.AddMilliseconds(0.0);
      int num1 = date / 10000;
      int num2 = (date - num1 * 10000) / 100;
      int num3 = date - num1 * 10000 - num2 * 100;
      int num4 = time / 10000;
      int num5 = (time - num4 * 10000) / 100;
      int num6 = time - num4 * 10000 - num5 * 100;
      dateTime = dateTime.AddYears(num1 - 1);
      dateTime = dateTime.AddMonths(num2 - 1);
      dateTime = dateTime.AddDays((double) (num3 - 1));
      dateTime = dateTime.AddHours((double) num4);
      dateTime = dateTime.AddMinutes((double) num5);
      dateTime = dateTime.AddSeconds((double) num6);
      return dateTime;
    }
  }
}
