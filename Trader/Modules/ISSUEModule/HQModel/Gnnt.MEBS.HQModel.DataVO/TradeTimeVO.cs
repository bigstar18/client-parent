using System;
using System.Text;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class TradeTimeVO
	{
		public int orderID;
		public int beginDate;
		public int beginTime;
		public int endDate;
		public int endTime;
		public int tradeDate;
		public int status;
		public DateTime modifytime = default(DateTime);
		public override string ToString()
		{
			string text = "\n";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("**TradeTimeVO**" + text);
			stringBuilder.Append("OrderID:" + this.orderID + text);
			stringBuilder.Append("BeginTime:" + this.beginTime + text);
			stringBuilder.Append("EndTime:" + this.endTime + text);
			stringBuilder.Append("Status:" + this.status + text);
			stringBuilder.Append("Modifytime:" + this.modifytime + text);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
		public static string HHMMSSIntToString(int iTime)
		{
			string text = string.Empty;
			text = Convert.ToString(iTime);
			while (text.Length < 6)
			{
				text = "0" + text;
			}
			return string.Concat(new string[]
			{
				text.Substring(0, 2),
				":",
				text.Substring(2, 2),
				":",
				text.Substring(4)
			});
		}
		public static string HHMMIntToString(int iTime)
		{
			string text = string.Empty;
			text = Convert.ToString(iTime);
			while (text.Length < 4)
			{
				text = "0" + text;
			}
			return text.Substring(0, 2) + ":" + text.Substring(2);
		}
		public static int TimeStringToInt(string strTime)
		{
			strTime = strTime.Replace(":", "");
			strTime = strTime.Replace("-", "");
			return Convert.ToInt32(strTime);
		}
		public static DateTime HHmmToDateTime(int yyyyMMdd, int HHmm)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			text = Convert.ToString(yyyyMMdd);
			while (text.Length < 6)
			{
				text = "0" + text;
			}
			text2 = Convert.ToString(HHmm);
			while (text2.Length < 4)
			{
				text2 = "0" + text2;
			}
			return DateTime.ParseExact(text + text2, "yyyyMMddHHmm", null);
		}
		public static DateTime HHmmssToDateTime(int yyyyMMdd, int HHmmss)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			text = Convert.ToString(yyyyMMdd);
			while (text.Length < 6)
			{
				text = "0" + text;
			}
			text2 = Convert.ToString(HHmmss);
			while (text2.Length < 6)
			{
				text2 = "0" + text2;
			}
			if (yyyyMMdd == 0 && HHmmss == 0)
			{
				Logger.wirte(3, "某个市场时间为零，或者其他地方时间为0，转换的时候报错！");
				return DateTime.Now;
			}
			return DateTime.ParseExact(text + text2, "yyyyMMddHHmmss", null);
		}
		public static int GetTotalMinute(TradeTimeVO[] timeRange)
		{
			int num = 0;
			for (int i = 0; i < timeRange.Length; i++)
			{
				DateTime value = TradeTimeVO.HHmmToDateTime(timeRange[i].beginDate, timeRange[i].beginTime);
				num += (int)TradeTimeVO.HHmmToDateTime(timeRange[i].endDate, timeRange[i].endTime).Subtract(value).TotalMinutes;
			}
			return num;
		}
		public static int GetTimeFromIndex(int iIndex, TradeTimeVO[] timeRange)
		{
			int num = iIndex + 1;
			for (int i = 0; i < timeRange.Length; i++)
			{
				DateTime value = TradeTimeVO.HHmmToDateTime(timeRange[i].beginDate, timeRange[i].beginTime);
				int num2 = (int)TradeTimeVO.HHmmToDateTime(timeRange[i].endDate, timeRange[i].endTime).Subtract(value).TotalMinutes;
				if (num2 >= num)
				{
					return TradeTimeVO.TimeStringToInt(value.AddMinutes((double)num).ToString("HH:mm"));
				}
				num -= num2;
			}
			return -1;
		}
		public static DateTime GetDateTimeFromIndex(int iIndex, TradeTimeVO[] timeRange)
		{
			if (timeRange.Length > 0)
			{
				DateTime result = TradeTimeVO.HHmmToDateTime(timeRange[timeRange.Length - 1].endDate, timeRange[timeRange.Length - 1].endTime);
				int num = iIndex + 1;
				for (int i = 0; i < timeRange.Length; i++)
				{
					DateTime value = TradeTimeVO.HHmmToDateTime(timeRange[i].beginDate, timeRange[i].beginTime);
					int num2 = (int)TradeTimeVO.HHmmToDateTime(timeRange[i].endDate, timeRange[i].endTime).Subtract(value).TotalMinutes;
					if (num2 >= num)
					{
						return value.AddMinutes((double)num);
					}
					num -= num2;
				}
				return result;
			}
			return default(DateTime);
		}
		public static int GetIndexFromTime(int iDate, int iTime, TradeTimeVO[] timeRange)
		{
			DateTime dateTime = TradeTimeVO.HHmmToDateTime(iDate, iTime);
			return TradeTimeVO.GetIndexFromTime(dateTime, timeRange);
		}
		public static int GetIndexFromTime(DateTime dateTime, TradeTimeVO[] timeRange)
		{
			int num = -1;
			for (int i = 0; i < timeRange.Length; i++)
			{
				DateTime value = TradeTimeVO.HHmmToDateTime(timeRange[i].beginDate, timeRange[i].beginTime);
				DateTime dateTime2 = TradeTimeVO.HHmmToDateTime(timeRange[i].endDate, timeRange[i].endTime);
				if (value.Subtract(dateTime).TotalSeconds > 0.0)
				{
					return num;
				}
				if (dateTime2.CompareTo(dateTime) >= 0 && value.CompareTo(dateTime) <= 0)
				{
					num += (int)dateTime.Subtract(value).TotalMinutes;
					break;
				}
				num += (int)dateTime2.Subtract(value).TotalMinutes;
			}
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
	}
}
