namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryCommDataOperation : QueryOperation
    {
        private string CurCommodityID = string.Empty;
        private bool refreshGNFlag = true;
        public SetLockInfoCallBack SetLockInfo;
        private int timerRefreshCount;
        private double timerRefreshGN = 3000.0;
        public UpdateCommDataCallBack UpdateCommData;
        public UpdatePriceCallBack UpdatePrice;

        private double GetPrice(CommData commData, int buysell)
        {
            double prevClear = 0.0;
            if (commData.PrevClear > 0.0)
            {
                prevClear = commData.PrevClear;
            }
            if (commData.Last > 0.0)
            {
                prevClear = commData.Last;
            }
            if ((buysell == 1) && (commData.Bid > 0.0))
            {
                prevClear = commData.Bid;
            }
            if ((buysell == 2) && (commData.Offer > 0.0))
            {
                prevClear = commData.Offer;
            }
            return prevClear;
        }

        public void RefreshGN(string commodityID)
        {
            if (!string.IsNullOrEmpty(commodityID))
            {
                this.CurCommodityID = commodityID;
                WaitCallback callBack = new WaitCallback(this.RefreshGNCommodity);
                ThreadPool.QueueUserWorkItem(callBack, commodityID);
            }
        }

        private void RefreshGNCommodity(object commodityID)
        {
            if (this.refreshGNFlag)
            {
                try
                {
                    this.refreshGNFlag = false;
                    CommData commData = base.serviceManage.CreateQueryCommData().QueryGNCommodityInfo(Global.MarketID, commodityID.ToString());
                    if (this.UpdatePrice != null)
                    {
                        this.UpdatePrice(this.GetPrice(commData, 1), this.GetPrice(commData, 2));
                    }
                    if (this.UpdateCommData != null)
                    {
                        this.UpdateCommData(commData);
                    }
                }
                catch (Exception exception)
                {
                    Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
                }
                this.refreshGNFlag = true;
            }
        }

        public void RefreshGNTime(int interval)
        {
            if ((this.timerRefreshGN / ((double)interval)) <= this.timerRefreshCount)
            {
                this.RefreshGN(this.CurCommodityID);
                if ((IniData.GetInstance().LockEnable && (DateTime.Now.Subtract(Global.IdleStartTime).Minutes >= IniData.GetInstance().LockTime)) && (this.SetLockInfo != null))
                {
                    this.SetLockInfo(false);
                }
                this.timerRefreshCount = 0;
            }
            this.timerRefreshCount++;
        }

        public void SetRefreshGNTime(bool type)
        {
            if (!type)
            {
                this.timerRefreshGN = 60000.0;
            }
            else
            {
                this.timerRefreshGN = Tools.StrToDouble((string)Global.HTConfig["SysTimeRefreshTime"], 1.3) * 1000.0;
            }
        }

        public delegate void SetLockInfoCallBack(bool type);

        public delegate void UpdateCommDataCallBack(CommData commData);

        public delegate void UpdatePriceCallBack(double bPrice, double sellPrice);
    }
}
