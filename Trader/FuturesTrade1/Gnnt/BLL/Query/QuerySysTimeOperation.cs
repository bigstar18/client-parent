namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QuerySysTimeOperation : QueryOperation
    {
        private int connectStatus;
        private int HQTimerRefreshCount;
        private double QuerySysTimeRefresh = 1300.0;
        private bool refreshTimeFlag = true;
        public SetConnectStatusCallBack SetConnectStatus;
        public ShowNotifierCallBack ShowNotifier;

        public event EventHandler ChangeServerEvent;

        private void CheckChangeTradeDay(string tradeDay)
        {
            if ((Global.curTradeDay != null) && (Global.curTradeDay.Length > 0))
            {
                Global.curTradeDay.Equals(tradeDay);
            }
        }

        private void CheckConnectStatus(long retCode, string retMessage)
        {
            bool flag = false;
            if (retCode == 0L)
            {
                if (this.connectStatus != 0)
                {
                    flag = true;
                }
                this.connectStatus = 0;
            }
            else
            {
                Logger.wirte(MsgType.Error, "主窗体获取服务器系统时间错误：" + retMessage);
                if (this.connectStatus == 0)
                {
                    flag = true;
                    if ((retMessage != null) && (retMessage.Length > 0))
                    {
                        this.connectStatus = 1;
                    }
                    else
                    {
                        this.connectStatus = 2;
                        if (this.ChangeServerEvent != null)
                        {
                            this.ChangeServerEvent(null, null);
                        }
                    }
                }
            }
            if ((this.SetConnectStatus != null) && flag)
            {
                this.SetConnectStatus(this.connectStatus);
            }
        }

        private void displayBroadcast(object obj)
        {
            List<Broadcast> list = (List<Broadcast>)obj;
            if (list != null)
            {
                int count = list.Count;
            }
        }

        private void displayTradeInfo(object obj)
        {
            string tradeInfo = (string)obj;
            if (!tradeInfo.Equals(""))
            {
                PlayWav.PlayWavResource("ring.wav", 0);
                if (IniData.GetInstance().SuccessShowDialog)
                {
                    if (this.ShowNotifier != null)
                    {
                        this.ShowNotifier(tradeInfo);
                    }
                }
                else
                {
                    tradeInfo = tradeInfo.Replace("\n", "\0");
                    if (Global.StatusInfoFill != null)
                    {
                        Global.StatusInfoFill(tradeInfo, Global.RightColor, true);
                    }
                }
            }
        }

        private void DisplayTradeMessage(short newTrade, List<TradeMessage> tradeMessageList)
        {
            if (newTrade == 1)
            {
                string str = string.Empty;
                if ((tradeMessageList != null) && (tradeMessageList.Count > 0))
                {
                    string str2 = Global.M_ResourceManager.GetString("TradeStr_MainnForm_TradeMessege1");
                    string str3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_TradeMessege2");
                    for (int i = 0; i < tradeMessageList.Count; i++)
                    {
                        if (tradeMessageList[i].CommodityID == "")
                        {
                            str = str + string.Format("{0}{1}{2}{3}\n", new object[] { tradeMessageList[i].OrderNO, str2, str3, tradeMessageList[i].CommodityID, tradeMessageList[i].TradeQuatity });
                        }
                        else
                        {
                            str = str + string.Format("{0}{1}({2}){3}{4}\n", new object[] { tradeMessageList[i].OrderNO, str2, tradeMessageList[i].CommodityID, str3, tradeMessageList[i].TradeQuatity });
                        }
                    }
                    if (base.RefreshCurrentTab != null)
                    {
                        base.RefreshCurrentTab(1, true);
                    }
                }
                this.displayTradeInfo(str);
            }
        }

        private void QuerySysTime(object obj)
        {
            if (this.refreshTimeFlag)
            {
                this.refreshTimeFlag = false;
                SysTimeQueryRequestVO req = new SysTimeQueryRequestVO
                {
                    UserID = Global.UserID
                };
                SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(req);
                this.CheckConnectStatus(sysTime.RetCode, sysTime.RetMessage);
                if (sysTime.RetCode == 0L)
                {
                    if (!sysTime.CurrentDate.Equals("") && !sysTime.CurrentTime.Equals(""))
                    {
                        this.CheckChangeTradeDay(sysTime.TradeDay);
                        Global.curTradeDay = sysTime.TradeDay;
                        this.SetCurrentDateTime(sysTime.CurrentDate, sysTime.CurrentTime);
                    }
                    if ((sysTime.RefreshMark != 0) && (base.RefreshCurrentTab != null))
                    {
                        base.RefreshCurrentTab(2, true);
                    }
                    this.DisplayTradeMessage(sysTime.NewTrade, sysTime.TradeMessageList);
                }
                this.refreshTimeFlag = true;
            }
        }

        public void QuerySysTimeThread()
        {
            WaitCallback callBack = new WaitCallback(this.QuerySysTime);
            ThreadPool.QueueUserWorkItem(callBack, null);
        }

        public void RefreshSysTime(int interval)
        {
            this.HQTimerRefreshCount++;
            if ((this.QuerySysTimeRefresh / ((double)interval)) <= this.HQTimerRefreshCount)
            {
                this.HQTimerRefreshCount = 0;
                this.QuerySysTimeThread();
            }
        }

        private void SetCurrentDateTime(string currentDate, string currentTime)
        {
            DateTime now = new DateTime();
            try
            {
                now = DateTime.Parse(currentDate + " " + currentTime);
            }
            catch
            {
                now = DateTime.Now;
            }
            Global.ServerTime = now;
        }

        public void SetSysTimeRefreshTime(bool type)
        {
            if (!type)
            {
                this.QuerySysTimeRefresh = 20000.0;
            }
            else
            {
                this.QuerySysTimeRefresh = Tools.StrToDouble((string)Global.HTConfig["HQRefreshTime"], 1.5) * 1000.0;
            }
        }

        public delegate void SetConnectStatusCallBack(int connectionStatus);

        public delegate void ShowNotifierCallBack(string tradeInfo);
    }
}
