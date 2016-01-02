using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation
{
	public class QueryAllOrderOperation : QueryOperation
	{
		public delegate void AllOrderFillCallBack(DataTable allOrderDataTable, bool _isShowPagingControl);
		private bool QueryAllOrderFlag = true;
		private string allOrderSortFld = "OrderNo";
		private string commodityID = string.Empty;
		private short buySellType;
		private short orderStatue;
		private int allOrderCurrentPage = 1;
		private byte queryCurrentPageDataFlag;
		private int allOrderAllNum;
		private int allOrderAllPage;
		private string allOrderSql = " 1=1 ";
		private string allOrderSort = " DESC";
		private string[] sumNmaes = new string[]
		{
			"Time",
			"TransactionsCode",
			"Qty",
			"Balance"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private bool isShowPagingControl;
		private bool isOrderNew;
		private bool refreshAllOrderFlag = true;
		private byte QueryCurrentDataFlag;
		private bool QueryPagingDataFirst = true;
		public QueryAllOrderOperation.AllOrderFillCallBack AllOrderFill;
		public int AllOrderCurrentPage
		{
			get
			{
				return this.allOrderCurrentPage;
			}
			set
			{
				this.allOrderCurrentPage = value;
			}
		}
		public int AllOrderAllPage
		{
			get
			{
				return this.allOrderAllPage;
			}
			set
			{
				this.allOrderAllPage = value;
			}
		}
		public QueryAllOrderOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryAllOrderInfoLoad()
		{
			if (this.QueryAllOrderFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				if (this.isOrderNew)
				{
					this.QueryPagingAllOrderInfoThread(null);
				}
				else
				{
					this.QueryAllOrderInfoThread();
				}
				this.QueryAllOrderFlag = false;
			}
		}
		public void QueryAllOrderInfo()
		{
			if (this.isOrderNew)
			{
				this.QueryPagingOrderMemoryInfo(null);
				return;
			}
			this.QueryAllOrderMemoryInfo(null);
		}
		private void QueryAllOrderInfoThread()
		{
			WaitCallback callBack = new WaitCallback(this.QueryAllOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack);
		}
		private void QueryAllOrderInfo(object showOrder)
		{
			if (!this.refreshAllOrderFlag)
			{
				return;
			}
			this.refreshAllOrderFlag = false;
			OrderQueryRequestVO orderQueryReqVO = this.FillOrderQueryReqVO();
			this.QueryAllOrderMemoryInfo(orderQueryReqVO);
			this.refreshAllOrderFlag = true;
		}
		private void QueryAllOrderMemoryInfo(OrderQueryRequestVO orderQueryReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryAllOrderDataSet(orderQueryReqVO);
			this.allOrderAllNum = dataSet.Tables["Order"].Rows.Count;
			this.UnOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Order"], this.allOrderSql, this.allOrderSortFld + this.allOrderSort, this.allOrderCurrentPage);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.AllOrderFill != null)
			{
				this.AllOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private OrderQueryRequestVO FillOrderQueryReqVO()
		{
			return new OrderQueryRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryPagingAllOrderInfoThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.QueryPingOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void QueryPingOrderInfo(object o)
		{
			if (!this.refreshAllOrderFlag)
			{
				return;
			}
			this.refreshAllOrderFlag = false;
			OrderQueryPagingRequestVO orderQueryPagingRegVO = this.FillOrderQueryPagingReqVO(o);
			this.QueryPagingOrderMemoryInfo(orderQueryPagingRegVO);
			this.refreshAllOrderFlag = true;
		}
		private void QueryPagingOrderMemoryInfo(OrderQueryPagingRequestVO OrderQueryPagingRegVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryAllOrderDataSet(OrderQueryPagingRegVO);
			this.allOrderAllNum = base.GetAllDataCount(dataSet.Tables["Order"], this.sumNmaes);
			this.UnOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Order"], this.allOrderSql, this.allOrderSortFld + this.allOrderSort, 1);
			if (this.AllOrderFill != null)
			{
				this.AllOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private OrderQueryPagingRequestVO FillOrderQueryPagingReqVO(object o)
		{
			OrderQueryPagingRequestVO orderQueryPagingRequestVO = new OrderQueryPagingRequestVO();
			orderQueryPagingRequestVO.UserID = Global.UserID;
			if (o != null)
			{
				orderQueryPagingRequestVO.CurrentPagNum = this.allOrderCurrentPage;
			}
			orderQueryPagingRequestVO.IsQueryAll = 0;
			orderQueryPagingRequestVO.IsDesc = base.GetDescOrAsc(this.allOrderSort);
			orderQueryPagingRequestVO.RecordCount = Global.PagNum;
			orderQueryPagingRequestVO.SortFld = this.allOrderSortFld;
			if (orderQueryPagingRequestVO.SortFld == "Price")
			{
				orderQueryPagingRequestVO.SortFld = "OR_P";
			}
			orderQueryPagingRequestVO.Pri = this.commodityID;
			orderQueryPagingRequestVO.Type = this.buySellType;
			orderQueryPagingRequestVO.Sta = this.orderStatue;
			return orderQueryPagingRequestVO;
		}
		private void UnOrderSetPage()
		{
			if (this.allOrderAllNum <= Global.PagNum)
			{
				this.isShowPagingControl = false;
				return;
			}
			this.isShowPagingControl = true;
			if (this.allOrderAllNum % Global.PagNum == 0)
			{
				this.allOrderAllPage = this.allOrderAllNum / Global.PagNum;
				return;
			}
			this.allOrderAllPage = this.allOrderAllNum / Global.PagNum + 1;
		}
		public void AllOrderDataGridViewSort(string allOrderSortName)
		{
			this.allOrderSortFld = allOrderSortName;
			if (this.allOrderSort == " ASC ")
			{
				this.allOrderSort = " Desc ";
			}
			else
			{
				this.allOrderSort = " ASC ";
			}
			if (this.isOrderNew)
			{
				this.QueryPagingAllOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryAllOrderMemoryInfo(null);
		}
		public void QueryPageAllOrderData(byte buttonMark, int num)
		{
			switch (buttonMark)
			{
			case 0:
				this.allOrderCurrentPage = 1;
				break;
			case 1:
				this.allOrderCurrentPage--;
				if (this.allOrderCurrentPage < 1)
				{
					this.allOrderCurrentPage = 1;
				}
				break;
			case 2:
				this.allOrderCurrentPage++;
				if (this.allOrderCurrentPage > this.allOrderAllPage)
				{
					this.allOrderCurrentPage = this.allOrderAllPage;
				}
				break;
			case 3:
				this.allOrderCurrentPage = this.allOrderAllPage;
				break;
			case 4:
				this.allOrderCurrentPage = num;
				break;
			}
			if (this.isOrderNew)
			{
				this.QueryPagingAllOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryAllOrderMemoryInfo(null);
		}
		public void ScreeningAllOrderData(string _commodityID, short _buySellType, short _orderStatue, string sql)
		{
			this.commodityID = _commodityID;
			this.orderStatue = _orderStatue;
			this.buySellType = _buySellType;
			this.allOrderSql = sql;
			this.allOrderCurrentPage = 1;
			if (this.isOrderNew)
			{
				this.QueryPagingAllOrderInfoThread(null);
				return;
			}
			this.QueryAllOrderMemoryInfo(null);
		}
		public void SetAllOrderIsPaging(bool isPagingQuery)
		{
			this.isOrderNew = isPagingQuery;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryAllOrderFlag = flag;
		}
	}
}
