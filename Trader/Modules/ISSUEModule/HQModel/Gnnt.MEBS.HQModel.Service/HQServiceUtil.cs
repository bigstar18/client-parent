using System;
namespace Gnnt.MEBS.HQModel.Service
{
	internal class HQServiceUtil
	{
		public static DateTime getDate(int date, int time)
		{
			DateTime dateTime = default(DateTime);
			dateTime.AddMilliseconds(0.0);
			int num = date / 10000;
			int num2 = (date - num * 10000) / 100;
			int num3 = date - num * 10000 - num2 * 100;
			int num4 = time / 10000;
			int num5 = (time - num4 * 10000) / 100;
			int num6 = time - num4 * 10000 - num5 * 100;
			return dateTime.AddYears(num - 1).AddMonths(num2 - 1).AddDays((double)(num3 - 1)).AddHours((double)num4).AddMinutes((double)num5).AddSeconds((double)num6);
		}
	}
}
