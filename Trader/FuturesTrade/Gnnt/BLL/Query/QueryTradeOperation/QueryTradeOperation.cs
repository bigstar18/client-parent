namespace FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryTradeOperation : QueryOperation
    {
        private short buySellType;
        private string commodityID = string.Empty;
        private bool isShowPagingControl;
        private bool isTradeNew;
        private byte QueryCurrentDataFlag;
        private byte queryCurrentPageDataFlag;
        private bool QueryPagingDataFirst = true;
        private bool QueryTradeFlag = true;
        private bool refreshTradeFlag = true;
        private short se_f;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "Time", "TransactionsCode", "Qty", "Liqpl" };
        private int tradeAllNum;
        private int tradeAllPage;
        private int tradeCurrentPage = 1;
        public TradeFillCallBack TradeFill;
        private string tradeSort = " DESC";
        private string tradeSortFld = "TradeNo";
        private string tradeSql = " 1=1 ";

        public QueryTradeOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private TradeQueryPagingRequestVO FillTradeQueryPagingReqVO(object o)
        {
            TradeQueryPagingRequestVO tvo = new TradeQueryPagingRequestVO
            {
                UserID = Global.UserID
            };
            if (o != null)
            {
                tvo.CurrentPagNum = this.tradeCurrentPage;
            }
            tvo.IsDesc = base.GetDescOrAsc(this.tradeSort);
            tvo.RecordCount = Global.PagNum;
            tvo.SortFld = this.tradeSortFld;
            if (tvo.SortFld == "Price")
            {
                tvo.SortFld = "TR_P";
            }
            tvo.Pri = this.commodityID;
            tvo.Type = this.buySellType;
            tvo.Se_f = this.se_f;
            return tvo;
        }

        private TradeQueryRequestVO FillTradeQueryReqVO()
        {
            return new TradeQueryRequestVO { UserID = Global.UserID };
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
            }
            else
            {
                this.QueryTradeMemoryInfo(null);
            }
        }

        private void QueryPagingTradeInfoThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.QueryPingTradeInfo);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void QueryPagingTradeMemoryInfo(TradeQueryPagingRequestVO tradeQueryPagingRegVO)
        {
            try
            {
                DataSet set = base.serviceManage.CreateIQueryTrade(this.isTradeNew).QueryTradeDataSet(tradeQueryPagingRegVO);
                this.tradeAllNum = base.GetAllDataCount(set.Tables["Trade"], this.sumNmaes);
                this.TradeSetPage();
                DataTable tradeDataTable = base.GetDataTable(set.Tables["Trade"], this.tradeSql, this.tradeSortFld + this.tradeSort, 1);
                if (this.TradeFill != null)
                {
                    this.TradeFill(tradeDataTable, this.isShowPagingControl);
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void QueryPingTradeInfo(object o)
        {
            if (this.refreshTradeFlag)
            {
                this.refreshTradeFlag = false;
                TradeQueryPagingRequestVO tradeQueryPagingRegVO = this.FillTradeQueryPagingReqVO(o);
                this.QueryPagingTradeMemoryInfo(tradeQueryPagingRegVO);
                this.refreshTradeFlag = true;
            }
        }

        private void QueryTradeInfo(object showOrder)
        {
            if (this.refreshTradeFlag)
            {
                this.refreshTradeFlag = false;
                TradeQueryRequestVO tradeQueryReqVO = this.FillTradeQueryReqVO();
                this.QueryTradeMemoryInfo(tradeQueryReqVO);
                this.refreshTradeFlag = true;
            }
        }

        public void QueryTradeInfoLoad()
        {
            if (this.QueryTradeFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
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

        private void QueryTradeMemoryInfo(TradeQueryRequestVO tradeQueryReqVO)
        {
            try
            {
                DataSet set = base.serviceManage.CreateIQueryTrade(this.isTradeNew).QueryTradeDataSet(tradeQueryReqVO);
                this.tradeAllNum = set.Tables["Trade"].Rows.Count;
                this.TradeSetPage();
                DataTable tradeDataTable = base.GetDataTable(set.Tables["Trade"], this.tradeSql, this.tradeSortFld + this.tradeSort, this.tradeCurrentPage);
                base.DataViewAddQueryDgUnTradeSum(tradeDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
                if (this.TradeFill != null)
                {
                    this.TradeFill(tradeDataTable, this.isShowPagingControl);
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
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
            }
            else
            {
                this.QueryTradeMemoryInfo(null);
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryTradeFlag = flag;
        }

        public void SetTradeIsPaging(bool isPagingQuery)
        {
            this.isTradeNew = isPagingQuery;
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
            }
            else
            {
                this.QueryTradeMemoryInfo(null);
            }
        }

        private void TradeSetPage()
        {
            if (this.tradeAllNum > Global.PagNum)
            {
                this.isShowPagingControl = true;
                if ((this.tradeAllNum % Global.PagNum) == 0)
                {
                    this.tradeAllPage = this.tradeAllNum / Global.PagNum;
                }
                else
                {
                    this.tradeAllPage = (this.tradeAllNum / Global.PagNum) + 1;
                }
            }
            else
            {
                this.isShowPagingControl = false;
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

        public delegate void TradeFillCallBack(DataTable tradeDataTable, bool _isShowPagingControl);
    }
}
