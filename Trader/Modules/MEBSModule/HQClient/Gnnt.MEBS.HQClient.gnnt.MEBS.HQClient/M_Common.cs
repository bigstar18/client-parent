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
			{
				return 0;
			}
			DateTime dateTime = TradeTimeVO.HHmmssToDateTime(yyyymmdd, hhmmss);
			int num = dateTime.Second;
			if (dateTime.Second > 0)
			{
				dateTime = dateTime.AddMinutes(1.0);
				dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
			}
			int num2 = TradeTimeVO.GetIndexFromTime(dateTime, timeRange);
			num2 *= 60 / iMinLineInterval;
			if (num == 0)
			{
				num = 60;
			}
			num2 += (num - 1) / iMinLineInterval;
			if (num2 < 0)
			{
				num2 = 0;
			}
			return num2;
		}
		public static int GetTimeIndexFromTime(int yyyymmdd, int hhmm, TradeTimeVO[] timeRange)
		{
			if (timeRange == null)
			{
				return 0;
			}
			return TradeTimeVO.GetIndexFromTime(yyyymmdd, hhmm, timeRange);
		}
		public static int GetTimeFromTimeIndex(int index, TradeTimeVO[] timeRange)
		{
			if (timeRange == null)
			{
				return 0;
			}
			return TradeTimeVO.GetTimeFromIndex(index, timeRange);
		}
		public static int GetTimeFromMinLineIndex(int index, TradeTimeVO[] timeRange, int iMinLineInterval)
		{
			if (timeRange == null)
			{
				return 0;
			}
			int num = 60 / iMinLineInterval;
			int num2 = TradeTimeVO.GetTimeFromIndex(index / num, timeRange);
			int num3 = (index % num + 1) % num * iMinLineInterval;
			if (num3 > 0)
			{
				if (num2 == 0)
				{
					num2 = 2400;
				}
				int num4 = num2 / 100 * 60 + num2 % 100 - 1;
				num2 = num4 / 60 * 100 + num4 % 60;
				return num2 * 100 + num3;
			}
			return num2 * 100 + num3;
		}
		public static TradeTimeVO[] getTimeRange(CommodityInfo commodityInfo, HQClientMain client)
		{
			if (client.TimeRange == null)
			{
				return new TradeTimeVO[0];
			}
			if (client.isIndex(commodityInfo))
			{
				return client.TimeRange;
			}
			CodeTable codeTable = (CodeTable)client.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
			if (codeTable == null)
			{
				return new TradeTimeVO[0];
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < codeTable.tradeSecNo.Length; i++)
			{
				for (int j = 0; j < client.TimeRange.Length; j++)
				{
					if (codeTable.tradeSecNo[i] == client.TimeRange[j].orderID)
					{
						arrayList.Add(client.TimeRange[j]);
						break;
					}
				}
			}
			TradeTimeVO[] array = new TradeTimeVO[arrayList.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = (TradeTimeVO)arrayList[k];
			}
			return array;
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
			for (int i = 0; i < timeRange.Length; i++)
			{
				DateTime value = TradeTimeVO.HHmmssToDateTime(timeRange[i].beginDate, timeRange[i].beginTime * 100);
				DateTime value2 = TradeTimeVO.HHmmssToDateTime(timeRange[i].endDate, timeRange[i].endTime * 100);
				if (dateTime.CompareTo(value) >= 0 && dateTime.CompareTo(value2) <= 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = timeRange.Length - 1; j >= 0; j--)
				{
					DateTime dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[j].endDate, timeRange[j].endTime * 100);
					if (dateTime.CompareTo(dateTime2) > 0)
					{
						dateTime = dateTime2;
						break;
					}
				}
			}
			return dateTime;
		}
		public static string FloatToString(double f, int iPrecision)
		{
			if (iPrecision == 0)
			{
				return f.ToString("0");
			}
			string text = "0.";
			for (int i = 0; i < iPrecision; i++)
			{
				text += "0";
			}
			return f.ToString(text);
		}
	}
}
