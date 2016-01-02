using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryConOrderOperation : QueryOperation
	{
		public delegate void ConOrderFillCallBack(DataTable conOrderDataTable, bool _isShowPagingControl);
		private bool QueryConOrderFlag = true;
		private string conOrderSortFld = "OrderNo";
		private string commodityID = string.Empty;
		private short buySellType;
		private short SettleBasis;
		private short ConditionType;
		private string conOrderStatue = "0";
		private int conOrderCurrentPage = 1;
		private byte queryCurrentPageDataFlag;
		private int conOrderAllNum;
		private int conOrderAllPage;
		private string conOrderSql = " 1=1 ";
		private string conOrderSort = " DESC";
		private string[] sumNmaes = new string[]
		{
			"CommodityID",
			"B_S",
			"Qty"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private Hashtable conOrderStatueHashtable = new Hashtable();
		private bool isShowPagingControl;
		private bool isConOrderNew;
		private bool refreshConOrderFlag = true;
		private byte QueryCurrentDataFlag;
		private bool QueryPagingDataFirst = true;
		public QueryConOrderOperation.ConOrderFillCallBack ConOrderFill;
		public int ConOrderCurrentPage
		{
			get
			{
				return this.conOrderCurrentPage;
			}
			set
			{
				this.conOrderCurrentPage = value;
			}
		}
		public int ConOrderAllPage
		{
			get
			{
				return this.conOrderAllPage;
			}
			set
			{
				this.conOrderAllPage = value;
			}
		}
		public bool IsShowPagingControl
		{
			get
			{
				return this.isShowPagingControl;
			}
			set
			{
				this.isShowPagingControl = value;
			}
		}
		public QueryConOrderOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
			this.conOrderStatueHashtable.Add("全部", "0");
			this.conOrderStatueHashtable.Add("未委托", "01");
			this.conOrderStatueHashtable.Add("已过期", "02");
			this.conOrderStatueHashtable.Add("委托成功", "11");
			this.conOrderStatueHashtable.Add("委托失败", "12");
			this.conOrderStatueHashtable.Add("已撤单", "2");
		}
		public void QueryConOrderInfoLoad()
		{
			if (this.QueryConOrderFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				this.QueryConOrderInfoThread(null);
				this.QueryConOrderFlag = false;
			}
		}
		private void QueryConOrderInfoThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.QueryConOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void QueryConOrderInfo(object o)
		{
			if (!this.refreshConOrderFlag)
			{
				return;
			}
			this.refreshConOrderFlag = false;
			ConditionQueryRequestVO queryConOrderReqVO = this.FillConOrderQueryReqVO(o);
			if (this.isConOrderNew)
			{
				this.QueryPagingConOrderMemoryInfo(queryConOrderReqVO);
			}
			else
			{
				this.QueryConOrderMemoryInfo(queryConOrderReqVO);
			}
			this.refreshConOrderFlag = true;
		}
		private void QueryConOrderMemoryInfo(ConditionQueryRequestVO queryConOrderReqVO)
		{
			DataSet dataSet = this.serviceManage.CreateQueryConOrder().QueryConditionOrderInfo(queryConOrderReqVO);
			this.conOrderAllNum = dataSet.Tables["Corder"].Rows.Count;
			this.ConOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Corder"], this.conOrderSql, this.conOrderSortFld + this.conOrderSort, this.conOrderCurrentPage);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.ConOrderFill != null)
			{
				this.ConOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private void QueryPagingConOrderMemoryInfo(ConditionQueryRequestVO queryConOrderReqVO)
		{
			DataSet dataSet = null;
			this.conOrderAllNum = base.GetAllDataCount(dataSet.Tables["Corder"], this.sumNmaes);
			this.ConOrderSetPage();
			DataTable dataTable = base.GetDataTable(dataSet.Tables["Corder"], this.conOrderSql, this.conOrderSortFld + this.conOrderSort, 1);
			base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
			if (this.ConOrderFill != null)
			{
				this.ConOrderFill(dataTable, this.isShowPagingControl);
			}
		}
		private ConditionQueryRequestVO FillConOrderQueryReqVO(object o)
		{
			ConditionQueryRequestVO conditionQueryRequestVO = new ConditionQueryRequestVO();
			conditionQueryRequestVO.UserID = Global.UserID;
			conditionQueryRequestVO.CommodityID = this.commodityID;
			conditionQueryRequestVO.BuySell = this.buySellType;
			conditionQueryRequestVO.ConditionType = this.ConditionType;
			conditionQueryRequestVO.ISDesc = base.GetDescOrAsc(this.conOrderSort);
			conditionQueryRequestVO.OrderStatus = this.conOrderStatue;
			conditionQueryRequestVO.RecordCount = Global.PagNum;
			conditionQueryRequestVO.SettleBasis = this.SettleBasis;
			conditionQueryRequestVO.SortField = this.conOrderSortFld;
			if (o != null)
			{
				conditionQueryRequestVO.PageNumber = this.conOrderCurrentPage;
			}
			if (this.isConOrderNew)
			{
				conditionQueryRequestVO.UpdateTime = 1L;
			}
			else
			{
				conditionQueryRequestVO.UpdateTime = 0L;
			}
			return conditionQueryRequestVO;
		}
		private void ConOrderSetPage()
		{
			if (this.conOrderAllNum <= Global.PagNum)
			{
				this.isShowPagingControl = false;
				return;
			}
			this.isShowPagingControl = true;
			if (this.conOrderAllNum % Global.PagNum == 0)
			{
				this.conOrderAllPage = this.conOrderAllNum / Global.PagNum;
				return;
			}
			this.conOrderAllPage = this.conOrderAllNum / Global.PagNum + 1;
		}
		public void ConOrderDataGridViewSort(string conOrderSortName)
		{
			this.conOrderSortFld = conOrderSortName;
			if (this.conOrderSort == " ASC ")
			{
				this.conOrderSort = " Desc ";
			}
			else
			{
				this.conOrderSort = " ASC ";
			}
			this.QueryConOrderInfo(null);
		}
		public void QueryPageConOrderData(byte buttonMark, int num)
		{
			switch (buttonMark)
			{
			case 0:
				this.conOrderCurrentPage = 1;
				break;
			case 1:
				this.conOrderCurrentPage--;
				if (this.conOrderCurrentPage < 1)
				{
					this.conOrderCurrentPage = 1;
				}
				break;
			case 2:
				this.conOrderCurrentPage++;
				if (this.conOrderCurrentPage > this.conOrderAllPage)
				{
					this.conOrderCurrentPage = this.conOrderAllPage;
				}
				break;
			case 3:
				this.conOrderCurrentPage = this.conOrderAllPage;
				break;
			case 4:
				this.conOrderCurrentPage = num;
				break;
			}
			this.QueryConOrderInfo(this.queryCurrentPageDataFlag);
		}
		public void ScreeningConOrderData(string _commodityID, short _buySellType, string _orderStatue, short _settleBasis, short _conditionType, string sql)
		{
			this.commodityID = _commodityID;
			this.conOrderStatue = this.conOrderStatueHashtable[_orderStatue].ToString();
			this.buySellType = _buySellType;
			this.SettleBasis = _settleBasis;
			this.ConditionType = _conditionType;
			this.conOrderSql = sql;
			this.conOrderCurrentPage = 1;
			if (this.isConOrderNew)
			{
				this.QueryConOrderInfoThread(null);
				return;
			}
			this.QueryConOrderMemoryInfo(null);
		}
		public void SetConOrderIsPaging(bool isPagingQuery)
		{
			this.isConOrderNew = isPagingQuery;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryConOrderFlag = flag;
		}
	}
}
