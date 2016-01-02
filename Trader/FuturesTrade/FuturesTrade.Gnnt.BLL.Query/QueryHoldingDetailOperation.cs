using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryHoldingDetailOperation : QueryOperation
	{
		public delegate void HoldingDetailFillCallBack(DataTable holdingDetailDataTable);
		private bool QueryHoldingDetailFlag = true;
		private string holdingDetailSortFld = "CommodityID";
		private string holdingDetailSql = " 1=1 ";
		private string holdingDetailSort = " ASC ";
		private string[] sumNmaes = new string[]
		{
			"CommodityID",
			"TransactionsCode",
			"Cur_Open",
			"Margin",
			"GoodsQty"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private bool refreshHoldingDetailFlag = true;
		public QueryHoldingDetailOperation.HoldingDetailFillCallBack HoldingDetailFill;
		public QueryHoldingDetailOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryHoldingDetailInfoLoad()
		{
			if (this.QueryHoldingDetailFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				WaitCallback callBack = new WaitCallback(this.QueryHoldingDetailInfo);
				ThreadPool.QueueUserWorkItem(callBack);
				this.QueryHoldingDetailFlag = false;
			}
		}
		private void QueryHoldingDetailInfo(object _holdingDetailQueryRequestVO)
		{
			if (!this.refreshHoldingDetailFlag)
			{
				return;
			}
			this.refreshHoldingDetailFlag = false;
			HoldingDetailRequestVO holdingDetailQueryReqVO = this.FillHoldingDetailQueryReqVO();
			this.QueryHoldingDetailMemoryInfo(holdingDetailQueryReqVO);
			this.refreshHoldingDetailFlag = true;
		}
		private HoldingDetailRequestVO FillHoldingDetailQueryReqVO()
		{
			return new HoldingDetailRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryHoldingDetailMemoryInfo(HoldingDetailRequestVO holdingDetailQueryReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateQueryHoldingDetail().QueryHoldingDetailInfo(holdingDetailQueryReqVO);
			DataTable dataTable = base.GetDataTable(dataSet.Tables["HoldingDetail"], this.holdingDetailSql, this.holdingDetailSortFld + this.holdingDetailSort);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.HoldingDetailFill != null)
			{
				this.HoldingDetailFill(dataTable);
			}
		}
		public void HoldingDetailSort(string sortName)
		{
			this.holdingDetailSortFld = sortName;
			if (this.holdingDetailSort == " ASC ")
			{
				this.holdingDetailSort = " Desc ";
			}
			else
			{
				this.holdingDetailSort = " ASC ";
			}
			this.QueryHoldingDetailMemoryInfo(null);
		}
		public void HoldingDetailScreen(string sql)
		{
			this.holdingDetailSql = sql;
			this.QueryHoldingDetailMemoryInfo(null);
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryHoldingDetailFlag = flag;
		}
	}
}
