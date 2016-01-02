using FuturesTrade.Gnnt.Library;
using System;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryCommDataOperation : QueryOperation
	{
		public delegate void UpdateCommDataCallBack(CommData commData);
		public delegate void UpdatePriceCallBack(double bPrice, double sellPrice);
		public delegate void SetLockInfoCallBack(bool type);
		public QueryCommDataOperation.UpdateCommDataCallBack UpdateCommData;
		public QueryCommDataOperation.UpdatePriceCallBack UpdatePrice;
		public QueryCommDataOperation.SetLockInfoCallBack SetLockInfo;
		private bool refreshGNFlag = true;
		private string CurCommodityID = string.Empty;
		private double timerRefreshGN = 3000.0;
		private int timerRefreshCount;
		public void RefreshGNTime(int interval)
		{
			if (this.timerRefreshGN / (double)interval <= (double)this.timerRefreshCount)
			{
				this.RefreshGN(this.CurCommodityID);
				if (IniData.GetInstance().LockEnable && DateTime.Now.Subtract(Global.IdleStartTime).Minutes >= IniData.GetInstance().LockTime && this.SetLockInfo != null)
				{
					this.SetLockInfo(false);
				}
				this.timerRefreshCount = 0;
			}
			this.timerRefreshCount++;
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
			if (!this.refreshGNFlag)
			{
				return;
			}
			try
			{
				this.refreshGNFlag = false;
				CommData commData = this.serviceManage.CreateQueryCommData().QueryGNCommodityInfo(Global.MarketID, commodityID.ToString());
				if (this.UpdatePrice != null)
				{
					this.UpdatePrice(this.GetPrice(commData, 1), this.GetPrice(commData, 2));
				}
				if (this.UpdateCommData != null)
				{
					this.UpdateCommData(commData);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
			this.refreshGNFlag = true;
		}
		private double GetPrice(CommData commData, int buysell)
		{
			double result = 0.0;
			if (commData.PrevClear > 0.0)
			{
				result = commData.PrevClear;
			}
			if (commData.Last > 0.0)
			{
				result = commData.Last;
			}
			if (buysell == 1 && commData.Bid > 0.0)
			{
				result = commData.Bid;
			}
			if (buysell == 2 && commData.Offer > 0.0)
			{
				result = commData.Offer;
			}
			return result;
		}
		public void SetRefreshGNTime(bool type)
		{
			if (!type)
			{
				this.timerRefreshGN = 60000.0;
				return;
			}
			this.timerRefreshGN = Tools.StrToDouble((string)Global.HTConfig["SysTimeRefreshTime"], 1.3) * 1000.0;
		}
	}
}
