using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation
{
	public class QueryTradeOperation : QueryOperation
	{
		public delegate void TradeFillCallBack(DataTable tradeDataTable, bool _isShowPagingControl);
		private bool QueryTradeFlag = true;
		private string tradeSortFld = "TradeNo";
		private string commodityID = string.Empty;
		private short buySellType;
		private short se_f;
		private int tradeCurrentPage = 1;
		private byte queryCurrentPageDataFlag;
		private int tradeAllNum;
		private int tradeAllPage;
		private string tradeSql = " 1=1 ";
		private string tradeSort = " DESC";
		private string[] sumNmaes = new string[]
		{
			"Time",
			"TransactionsCode",
			"Qty",
			"Liqpl"
		};
		private Hashtable sumNamesHashtable = new Hashtable();
		private bool isShowPagingControl;
		private bool isTradeNew;
		private bool refreshTradeFlag = true;
		private byte QueryCurrentDataFlag;
		private bool QueryPagingDataFirst = true;
		public QueryTradeOperation.TradeFillCallBack TradeFill;
		public int TradeCurrentPage
		{
			get
			{
				return this.tradeCurrentPage;
			}
			set
			{
				this.tradeCurrentPage = value;
			}
		}
		public int TradeAllPage
		{
			get
			{
				return this.tradeAllPage;
			}
			set
			{
				this.tradeAllPage = value;
			}
		}
		public QueryTradeOperation()
		{
			for (int i = 0; i < this.sumNmaes.Length; i++)
			{
				this.sumNamesHashtable.Add(this.sumNmaes[i], "");
			}
		}
		public void QueryTradeInfoLoad()
		{
			if (this.QueryTradeFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				if (this.isTradeNew)
				{
					this.QueryPagingTradeInfoThread(null);
				}
				else
				{
					this.QueryTradeInfoThread();
				}
				this.QueryTradeFlag = false;
			}
		}
		private void QueryTradeInfoThread()
		{
			WaitCallback callBack = new WaitCallback(this.QueryTradeInfo);
			ThreadPool.QueueUserWorkItem(callBack);
		}
		private void QueryTradeInfo(object showOrder)
		{
			if (!this.refreshTradeFlag)
			{
				return;
			}
			this.refreshTradeFlag = false;
			TradeQueryRequestVO tradeQueryReqVO = this.FillTradeQueryReqVO();
			this.QueryTradeMemoryInfo(tradeQueryReqVO);
			this.refreshTradeFlag = true;
		}
		private void QueryTradeMemoryInfo(TradeQueryRequestVO tradeQueryReqVO)
		{
			try
			{
				DataSet dataSet = this.serviceManage.CreateIQueryTrade(this.isTradeNew).QueryTradeDataSet(tradeQueryReqVO);
				this.tradeAllNum = dataSet.Tables["Trade"].Rows.Count;
				this.TradeSetPage();
				DataTable dataTable = base.GetDataTable(dataSet.Tables["Trade"], this.tradeSql, this.tradeSortFld + this.tradeSort, this.tradeCurrentPage);
				base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
				if (this.TradeFill != null)
				{
					this.TradeFill(dataTable, this.isShowPagingControl);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private TradeQueryRequestVO FillTradeQueryReqVO()
		{
			return new TradeQueryRequestVO
			{
				UserID = Global.UserID
			};
		}
		private void QueryPagingTradeInfoThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.QueryPingTradeInfo);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void QueryPingTradeInfo(object o)
		{
			if (!this.refreshTradeFlag)
			{
				return;
			}
			this.refreshTradeFlag = false;
			TradeQueryPagingRequestVO tradeQueryPagingRegVO = this.FillTradeQueryPagingReqVO(o);
			this.QueryPagingTradeMemoryInfo(tradeQueryPagingRegVO);
			this.refreshTradeFlag = true;
		}
		private void QueryPagingTradeMemoryInfo(TradeQueryPagingRequestVO tradeQueryPagingRegVO)
		{
			try
			{
				DataSet dataSet = this.serviceManage.CreateIQueryTrade(this.isTradeNew).QueryTradeDataSet(tradeQueryPagingRegVO);
				this.tradeAllNum = base.GetAllDataCount(dataSet.Tables["Trade"], this.sumNmaes);
				this.TradeSetPage();
				DataTable dataTable = base.GetDataTable(dataSet.Tables["Trade"], this.tradeSql, this.tradeSortFld + this.tradeSort, 1);
				if (this.TradeFill != null)
				{
					this.TradeFill(dataTable, this.isShowPagingControl);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private TradeQueryPagingRequestVO FillTradeQueryPagingReqVO(object o)
		{
			TradeQueryPagingRequestVO tradeQueryPagingRequestVO = new TradeQueryPagingRequestVO();
			tradeQueryPagingRequestVO.UserID = Global.UserID;
			if (o != null)
			{
				tradeQueryPagingRequestVO.CurrentPagNum = this.tradeCurrentPage;
			}
			tradeQueryPagingRequestVO.IsDesc = base.GetDescOrAsc(this.tradeSort);
			tradeQueryPagingRequestVO.RecordCount = Global.PagNum;
			tradeQueryPagingRequestVO.SortFld = this.tradeSortFld;
			if (tradeQueryPagingRequestVO.SortFld == "Price")
			{
				tradeQueryPagingRequestVO.SortFld = "TR_P";
			}
			tradeQueryPagingRequestVO.Pri = this.commodityID;
			tradeQueryPagingRequestVO.Type = this.buySellType;
			tradeQueryPagingRequestVO.Se_f = this.se_f;
			return tradeQueryPagingRequestVO;
		}
		private void TradeSetPage()
		{
			if (this.tradeAllNum <= Global.PagNum)
			{
				this.isShowPagingControl = false;
				return;
			}
			this.isShowPagingControl = true;
			if (this.tradeAllNum % Global.PagNum == 0)
			{
				this.tradeAllPage = this.tradeAllNum / Global.PagNum;
				return;
			}
			this.tradeAllPage = this.tradeAllNum / Global.PagNum + 1;
		}
		public void TradeDataGridViewSort(string tradeSortName)
		{
			this.tradeSortFld = tradeSortName;
			if (this.tradeSort == " ASC ")
			{
				this.tradeSort = " Desc ";
			}
			else
			{
				this.tradeSort = " ASC ";
			}
			if (this.isTradeNew)
			{
				this.QueryPagingTradeInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryTradeMemoryInfo(null);
		}
		public void QueryPageTradeData(byte buttonMark, int num)
		{
			switch (buttonMark)
			{
			case 0:
				this.tradeCurrentPage = 1;
				break;
			case 1:
				this.tradeCurrentPage--;
				if (this.tradeCurrentPage < 1)
				{
					this.tradeCurrentPage = 1;
				}
				break;
			case 2:
				this.tradeCurrentPage++;
				if (this.tradeCurrentPage > this.tradeAllPage)
				{
					this.tradeCurrentPage = this.tradeAllPage;
				}
				break;
			case 3:
				this.tradeCurrentPage = this.tradeAllPage;
				break;
			case 4:
				this.tradeCurrentPage = num;
				break;
			}
			if (this.isTradeNew)
			{
				this.QueryPagingTradeInfoThread(this.queryCurrentPageDataFlag);
				return;
			}
			this.QueryTradeMemoryInfo(null);
		}
		public void ScreeningTradeData(string _commodityID, short _buySellType, short _se_f, string sql)
		{
			this.commodityID = _commodityID;
			this.buySellType = _buySellType;
			this.se_f = _se_f;
			this.tradeSql = sql;
			this.tradeCurrentPage = 1;
			if (this.isTradeNew)
			{
				this.QueryPagingTradeInfoThread(null);
				return;
			}
			this.QueryTradeMemoryInfo(null);
		}
		public void SetTradeIsPaging(bool isPagingQuery)
		{
			this.isTradeNew = isPagingQuery;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryTradeFlag = flag;
		}
	}
}
