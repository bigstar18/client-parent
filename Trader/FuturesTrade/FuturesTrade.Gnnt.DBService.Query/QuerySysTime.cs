using FuturesTrade.Gnnt.Library;
using System;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QuerySysTime
	{
		public DateTime GetServerTime()
		{
			DateTime result = default(DateTime);
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = Global.UserID;
			SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
			string str = string.Empty;
			string str2 = string.Empty;
			if (sysTime.RetCode == 0L)
			{
				if (sysTime.CurrentDate.Equals("") || sysTime.CurrentTime.Equals(""))
				{
					return result;
				}
				str = sysTime.CurrentDate;
				str2 = sysTime.CurrentTime;
				try
				{
					result = DateTime.Parse(str + " " + str2);
					return result;
				}
				catch
				{
					result = default(DateTime);
					return result;
				}
			}
			Logger.wirte(MsgType.Error, "获取服务器系统时间错误：" + sysTime.RetMessage);
			return result;
		}
	}
}
