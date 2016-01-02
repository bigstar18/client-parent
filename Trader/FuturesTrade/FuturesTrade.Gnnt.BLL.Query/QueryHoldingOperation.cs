using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryHoldingOperation : QueryOperation
	{
		public delegate void HoldingFillCallBack(DataTable holdingDataTable);
		private bool QueryHoldingFlag = true;
		private string holdingSortFld = "CommodityID";
		private string holdingSql = " 1=1 ";
		private string holdingSort = " ASC";
		private string[] sumNmaes = new string[]
		{
			"CommodityID",
			"TransactionsCode",
			"BuyHolding",
			"SellHolding",
			"GoodsQty",
			"Margin",
			"Floatpl"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private bool refreshHoldingFlag = true;
		public QueryHoldingOperation.HoldingFillCallBack HoldingFillEvent;
		public QueryHoldingOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryHoldingInfoLoad()
		{
			if (this.QueryHoldingFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				WaitCallback callBack = new WaitCallback(this.QueryHoldingInfo);
				ThreadPool.QueueUserWorkItem(callBack);
				this.QueryHoldingFlag = false;
			}
		}
		private void QueryHoldingInfo(object _holdingQueryRequestVO)
		{
			if (!this.refreshHoldingFlag)
			{
				return;
			}
			this.refreshHoldingFlag = false;
			HoldingQueryRequestVO holdingQueryReqVO = this.FillHoldingQueryReqVO();
			this.QueryHoldingMemoryInfo(holdingQueryReqVO);
			this.refreshHoldingFlag = true;
		}
		private HoldingQueryRequestVO FillHoldingQueryReqVO()
		{
			return new HoldingQueryRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryHoldingMemoryInfo(HoldingQueryRequestVO holdingQueryReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateQueryHolding().QueryHoldingInfo(holdingQueryReqVO);
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Holding"], this.holdingSql, this.holdingSortFld + this.holdingSort);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.HoldingFillEvent != null)
			{
				this.HoldingFillEvent(dataTable);
			}
		}
		public void HoldingSort(string sortName)
		{
			this.holdingSortFld = sortName;
			if (this.holdingSort == " ASC ")
			{
				this.holdingSort = " Desc ";
			}
			else
			{
				this.holdingSort = " ASC ";
			}
			this.QueryHoldingMemoryInfo(null);
		}
		public void HoldingScreen(string sql)
		{
			this.holdingSql = sql;
			this.QueryHoldingMemoryInfo(null);
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryHoldingFlag = flag;
		}
	}
}
