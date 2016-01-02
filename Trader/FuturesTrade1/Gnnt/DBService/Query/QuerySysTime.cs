namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QuerySysTime
    {
        public DateTime GetServerTime()
        {
            DateTime time = new DateTime();
            SysTimeQueryRequestVO req = new SysTimeQueryRequestVO
            {
                UserID = Global.UserID
            };
            SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(req);
            string currentDate = string.Empty;
            string currentTime = string.Empty;
            if (sysTime.RetCode == 0L)
            {
                if (sysTime.CurrentDate.Equals("") || sysTime.CurrentTime.Equals(""))
                {
                    return time;
                }
                currentDate = sysTime.CurrentDate;
                currentTime = sysTime.CurrentTime;
                try
                {
                    return DateTime.Parse(currentDate + " " + currentTime);
                }
                catch
                {
                    return new DateTime();
                }
            }
            Logger.wirte(MsgType.Error, "获取服务器系统时间错误：" + sysTime.RetMessage);
            return time;
        }
    }
}
