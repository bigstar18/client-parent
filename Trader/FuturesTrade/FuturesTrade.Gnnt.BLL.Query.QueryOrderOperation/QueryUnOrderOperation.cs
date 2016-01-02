using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation
{
	public class QueryUnOrderOperation : QueryOperation
	{
		public delegate void UnOrderFillCallBack(DataTable unOrderDataTable, bool _isShowPagingControl);
		private bool QueryUnOrderFlag = true;
		private string unOrderSortFld = "OrderNo";
		private int unOrderCurrentPage = 1;
		private int unOrderAllNum;
		private int unOrderAllPage;
		private string unOrderSql = " 1=1 ";
		private string unOrderSort = " DESC";
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
		private bool refreshUnOrderFlag = true;
		private byte queryCurrentPageDataFlag;
		private bool QueryPagingDataFirst = true;
		public QueryUnOrderOperation.UnOrderFillCallBack UnOrderFill;
		public int UnOrderCurrentPage
		{
			get
			{
				return this.unOrderCurrentPage;
			}
			set
			{
				this.unOrderCurrentPage = value;
			}
		}
		public int UnOrderAllPage
		{
			get
			{
				return this.unOrderAllPage;
			}
			set
			{
				this.unOrderAllPage = value;
			}
		}
		public QueryUnOrderOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryUnOrderInfoLoad()
		{
			if (this.QueryUnOrderFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				if (this.isOrderNew)
				{
					this.QueryPagingUnOrderInfoThread(null);
				}
				else
				{
					this.QueryUnOrderInfoThread();
				}
				this.QueryUnOrderFlag = false;
			}
		}
		public void QueryUnOrderInfo()
		{
			if (this.isOrderNew)
			{
				this.QueryPagingUnOrderMemoryInfo(null);
				return;
			}
			this.QueryUnOrderMemoryInfo(null);
		}
		private void QueryUnOrderInfoThread()
		{
			WaitCallback callBack = new WaitCallback(this.QueryUnOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack);
		}
		private void QueryUnOrderInfo(object o)
		{
			if (!this.refreshUnOrderFlag)
			{
				return;
			}
			this.refreshUnOrderFlag = false;
			OrderQueryRequestVO orderQueryReqVO = this.FillOrderQueryReqVO();
			this.QueryUnOrderMemoryInfo(orderQueryReqVO);
			this.refreshUnOrderFlag = true;
		}
		private OrderQueryRequestVO FillOrderQueryReqVO()
		{
			return new OrderQueryRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryUnOrderMemoryInfo(OrderQueryRequestVO orderQueryReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryUnOrderDataSet(orderQueryReqVO);
			this.unOrderAllNum = dataSet.Tables["Order"].Rows.Count;
			this.UnOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Order"], this.unOrderSql, this.unOrderSortFld + this.unOrderSort, this.unOrderCurrentPage);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.UnOrderFill != null)
			{
				this.UnOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private void QueryPagingUnOrderInfoThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.QueryPingOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void QueryPingOrderInfo(object o)
		{
			if (!this.refreshUnOrderFlag)
			{
				return;
			}
			this.refreshUnOrderFlag = false;
			OrderQueryPagingRequestVO orderQueryPagingRegVO = this.FillOrderQueryPagingReqVO(o);
			this.QueryPagingUnOrderMemoryInfo(orderQueryPagingRegVO);
			this.refreshUnOrderFlag = true;
		}
		private OrderQueryPagingRequestVO FillOrderQueryPagingReqVO(object o)
		{
			OrderQueryPagingRequestVO orderQueryPagingRequestVO = new OrderQueryPagingRequestVO();
			orderQueryPagingRequestVO.UserID = Global.UserID;
			if (o != null)
			{
				orderQueryPagingRequestVO.CurrentPagNum = this.unOrderCurrentPage;
			}
			orderQueryPagingRequestVO.IsQueryAll = 1;
			orderQueryPagingRequestVO.IsDesc = base.GetDescOrAsc(this.unOrderSort);
			orderQueryPagingRequestVO.RecordCount = Global.PagNum;
			orderQueryPagingRequestVO.SortFld = this.unOrderSortFld;
			if (orderQueryPagingRequestVO.SortFld == "Price")
			{
				orderQueryPagingRequestVO.SortFld = "OR_P";
			}
			return orderQueryPagingRequestVO;
		}
		private void QueryPagingUnOrderMemoryInfo(OrderQueryPagingRequestVO orderQueryPagingRegVO)
		{
			DataSet dataSet = this.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryUnOrderDataSet(orderQueryPagingRegVO);
			this.unOrderAllNum = base.GetAllDataCount(dataSet.Tables["Order"], this.sumNmaes);
			this.UnOrderSetPage();
			if (this.UnOrderFill != null)
			{
				this.UnOrderFill(dataSet.Tables["Order"], this.isShowPagingControl);
			}
		}
		private void UnOrderSetPage()
		{
			if (this.unOrderAllNum <= Global.PagNum)
			{
				this.isShowPagingControl = false;
				return;
			}
			this.isShowPagingControl = true;
			if (this.unOrderAllNum % Global.PagNum == 0)
			{
				this.unOrderAllPage = this.unOrderAllNum / Global.PagNum;
				return;
			}
			this.unOrderAllPage = this.unOrderAllNum / Global.PagNum + 1;
		}
		public void UnOrderDataGridViewSort(string unOrderSortName)
		{
			this.unOrderSortFld = unOrderSortName;
			if (this.unOrderSort == " ASC ")
			{
				this.unOrderSort = " Desc ";
			}
			else
			{
				this.unOrderSort = " ASC ";
			}
			if (this.isOrderNew)
			{
				this.QueryPagingUnOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryUnOrderMemoryInfo(null);
		}
		public void QueryPageUnOrderData(byte buttonMark, int num)
		{
			switch (buttonMark)
			{
			case 0:
				this.unOrderCurrentPage = 1;
				break;
			case 1:
				this.unOrderCurrentPage--;
				if (this.unOrderCurrentPage < 1)
				{
					this.unOrderCurrentPage = 1;
				}
				break;
			case 2:
				this.unOrderCurrentPage++;
				if (this.unOrderCurrentPage > this.unOrderAllPage)
				{
					this.unOrderCurrentPage = this.unOrderAllPage;
				}
				break;
			case 3:
				this.unOrderCurrentPage = this.unOrderAllPage;
				break;
			case 4:
				this.unOrderCurrentPage = num;
				break;
			}
			if (this.isOrderNew)
			{
				this.QueryPagingUnOrderInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryUnOrderMemoryInfo(null);
		}
		public void SetUnOrderIsPaging(bool isPagingQuery)
		{
			this.isOrderNew = isPagingQuery;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryUnOrderFlag = flag;
		}
	}
}
