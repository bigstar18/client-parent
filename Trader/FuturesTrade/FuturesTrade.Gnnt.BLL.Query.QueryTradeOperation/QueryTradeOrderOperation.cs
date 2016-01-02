using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation
{
	public class QueryTradeOrderOperation : QueryOperation
	{
		public delegate void TradeOrderFillCallBack(DataTable tradeOrderDataTable, bool _isShowPagingControl);
		private bool QueryTradeOrderFlag = true;
		private string tradeOrderSortFld = "TradeNo";
		private int tradeOrderCurrentPage = 1;
		private int tradeOrderAllNum;
		private int tradeOrderAllPage;
		private string tradeOrderSql = " 1=1 ";
		private string tradeOrderSort = " DESC";
		private string[] sumNmaes = new string[]
		{
			"Time",
			"TransactionsCode",
			"Qty"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private bool isShowPagingControl;
		private bool isTradeOrderNew;
		private bool refreshTradeOrderFlag = true;
		private byte queryCurrentPageDataFlag;
		private bool QueryPagingDataFirst = true;
		public QueryTradeOrderOperation.TradeOrderFillCallBack TradeOrderFill;
		public int TradeOrderCurrentPage
		{
			get
			{
				return this.tradeOrderCurrentPage;
			}
			set
			{
				this.tradeOrderCurrentPage = value;
			}
		}
		public int TradeOrderAllPage
		{
			get
			{
				return this.tradeOrderAllPage;
			}
			set
			{
				this.tradeOrderAllPage = value;
			}
		}
		public QueryTradeOrderOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryTradeOrderInfoLoad()
		{
			if (this.QueryTradeOrderFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				if (this.isTradeOrderNew)
				{
					this.QueryPagingTradeOrderInfoThread(null);
				}
				else
				{
					this.QueryTradeOrderInfoThread();
				}
				this.QueryTradeOrderFlag = false;
			}
		}
		private void QueryTradeOrderInfoThread()
		{
			WaitCallback callBack = new WaitCallback(this.QueryTradeOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack);
		}
		private void QueryTradeOrderInfo(object o)
		{
			if (!this.refreshTradeOrderFlag)
			{
				return;
			}
			this.refreshTradeOrderFlag = false;
			TradeQueryRequestVO tradeOrderQueryReqVO = this.FillTradeOrderQueryReqVO();
			this.QueryTradeOrderMemoryInfo(tradeOrderQueryReqVO);
			this.refreshTradeOrderFlag = true;
		}
		private TradeQueryRequestVO FillTradeOrderQueryReqVO()
		{
			return new TradeQueryRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryTradeOrderMemoryInfo(TradeQueryRequestVO tradeOrderQueryReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryTrade(this.isTradeOrderNew).QueryTradeOrderDataSet(tradeOrderQueryReqVO);
			this.tradeOrderAllNum = dataSet.Tables["Trade"].Rows.Count;
			this.TradeOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Trade"], this.tradeOrderSql, this.tradeOrderSortFld + this.tradeOrderSort, this.tradeOrderCurrentPage);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.TradeOrderFill != null)
			{
				this.TradeOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private void QueryPagingTradeOrderInfoThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.QueryPingTradeOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void QueryPingTradeOrderInfo(object o)
		{
			if (!this.refreshTradeOrderFlag)
			{
				return;
			}
			this.refreshTradeOrderFlag = false;
			TradeQueryPagingRequestVO tradeOrderQueryPagingRegVO = this.FillTradeOrderQueryPagingReqVO(o);
			this.QueryPagingTradeOrderMemoryInfo(tradeOrderQueryPagingRegVO);
			this.refreshTradeOrderFlag = true;
		}
		private TradeQueryPagingRequestVO FillTradeOrderQueryPagingReqVO(object o)
		{
			TradeQueryPagingRequestVO tradeQueryPagingRequestVO = new TradeQueryPagingRequestVO();
			tradeQueryPagingRequestVO.UserID = Global.UserID;
			if (o != null)
			{
				tradeQueryPagingRequestVO.CurrentPagNum = this.tradeOrderCurrentPage;
			}
			tradeQueryPagingRequestVO.IsDesc = base.GetDescOrAsc(this.tradeOrderSort);
			tradeQueryPagingRequestVO.RecordCount = Global.PagNum;
			tradeQueryPagingRequestVO.SortFld = this.tradeOrderSortFld;
			if (tradeQueryPagingRequestVO.SortFld == "Price")
			{
				tradeQueryPagingRequestVO.SortFld = "TR_P";
			}
			return tradeQueryPagingRequestVO;
		}
		private void QueryPagingTradeOrderMemoryInfo(TradeQueryPagingRequestVO tradeOrderQueryPagingRegVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryTrade(this.isTradeOrderNew).QueryTradeOrderDataSet(tradeOrderQueryPagingRegVO);
			this.tradeOrderAllNum = base.GetAllDataCount(dataSet.Tables["Trade"], this.sumNmaes);
			this.TradeOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Trade"], this.tradeOrderSql, this.tradeOrderSortFld + this.tradeOrderSort, 1);
			if (this.TradeOrderFill != null)
			{
				this.TradeOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private void TradeOrderSetPage()
		{
			if (this.tradeOrderAllNum <= Global.PagNum)
			{
				this.isShowPagingControl = false;
				return;
			}
			this.isShowPagingControl = true;
			if (this.tradeOrderAllNum % Global.PagNum == 0)
			{
				this.tradeOrderAllPage = this.tradeOrderAllNum / Global.PagNum;
				return;
			}
			this.tradeOrderAllPage = this.tradeOrderAllNum / Global.PagNum + 1;
		}
		public void TradeOrderDataGridViewSort(string tradeOrderSortName)
		{
			this.tradeOrderSortFld = tradeOrderSortName;
			if (this.tradeOrderSort == " ASC ")
			{
				this.tradeOrderSort = " Desc ";
			}
			else
			{
				this.tradeOrderSort = " ASC ";
			}
			if (this.isTradeOrderNew)
			{
				this.QueryPagingTradeOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryTradeOrderMemoryInfo(null);
		}
		public void QueryPageTradeOrderData(byte buttonMark, int num)
		{
			switch (buttonMark)
			{
			case 0:
				this.tradeOrderCurrentPage = 1;
				break;
			case 1:
				this.tradeOrderCurrentPage--;
				if (this.tradeOrderCurrentPage < 1)
				{
					this.tradeOrderCurrentPage = 1;
				}
				break;
			case 2:
				this.tradeOrderCurrentPage++;
				if (this.tradeOrderCurrentPage > this.tradeOrderAllPage)
				{
					this.tradeOrderCurrentPage = this.tradeOrderAllPage;
				}
				break;
			case 3:
				this.tradeOrderCurrentPage = this.tradeOrderAllPage;
				break;
			case 4:
				this.tradeOrderCurrentPage = num;
				break;
			}
			if (this.isTradeOrderNew)
			{
				this.QueryPagingTradeOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryTradeOrderMemoryInfo(null);
		}
		public void SetTradeOrderIsPaging(bool isPagingQuery)
		{
			this.isTradeOrderNew = isPagingQuery;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryTradeOrderFlag = flag;
		}
	}
}
