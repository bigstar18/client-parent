namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryConOrderOperation : QueryOperation
    {
        private short buySellType;
        private string commodityID = string.Empty;
        private short ConditionType;
        private int conOrderAllNum;
        private int conOrderAllPage;
        private int conOrderCurrentPage = 1;
        public ConOrderFillCallBack ConOrderFill;
        private string conOrderSort = " DESC";
        private string conOrderSortFld = "OrderNo";
        private string conOrderSql = " 1=1 ";
        private string conOrderStatue = "0";
        private Hashtable conOrderStatueHashtable = new Hashtable();
        private bool isConOrderNew;
        private bool isShowPagingControl;
        private bool QueryConOrderFlag = true;
        private byte QueryCurrentDataFlag;
        private byte queryCurrentPageDataFlag;
        private bool QueryPagingDataFirst = true;
        private bool refreshConOrderFlag = true;
        private short SettleBasis;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "CommodityID", "B_S", "Qty" };

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

        private void ConOrderSetPage()
        {
            if (this.conOrderAllNum > Global.PagNum)
            {
                this.isShowPagingControl = true;
                if ((this.conOrderAllNum % Global.PagNum) == 0)
                {
                    this.conOrderAllPage = this.conOrderAllNum / Global.PagNum;
                }
                else
                {
                    this.conOrderAllPage = (this.conOrderAllNum / Global.PagNum) + 1;
                }
            }
            else
            {
                this.isShowPagingControl = false;
            }
        }

        private ConditionQueryRequestVO FillConOrderQueryReqVO(object o)
        {
            ConditionQueryRequestVO tvo = new ConditionQueryRequestVO
            {
                UserID = Global.UserID,
                CommodityID = this.commodityID,
                BuySell = this.buySellType,
                ConditionType = this.ConditionType,
                ISDesc = base.GetDescOrAsc(this.conOrderSort),
                OrderStatus = this.conOrderStatue,
                RecordCount = Global.PagNum,
                SettleBasis = this.SettleBasis,
                SortField = this.conOrderSortFld
            };
            if (o != null)
            {
                tvo.PageNumber = this.conOrderCurrentPage;
            }
            if (this.isConOrderNew)
            {
                tvo.UpdateTime = 1L;
                return tvo;
            }
            tvo.UpdateTime = 0L;
            return tvo;
        }

        private void QueryConOrderInfo(object o)
        {
            if (this.refreshConOrderFlag)
            {
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
        }

        public void QueryConOrderInfoLoad()
        {
            if (this.QueryConOrderFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                this.QueryConOrderInfoThread(null);
                this.QueryConOrderFlag = false;
            }
        }

        private void QueryConOrderInfoThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.QueryConOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void QueryConOrderMemoryInfo(ConditionQueryRequestVO queryConOrderReqVO)
        {
            DataSet set = base.serviceManage.CreateQueryConOrder().QueryConditionOrderInfo(queryConOrderReqVO);
            this.conOrderAllNum = set.Tables["Corder"].Rows.Count;
            this.ConOrderSetPage();
            DataTable conOrderDataTable = base.GetDataTable(set.Tables["Corder"], this.conOrderSql, this.conOrderSortFld + this.conOrderSort, this.conOrderCurrentPage);
            base.DataViewAddQueryDgUnTradeSum(conOrderDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.ConOrderFill != null)
            {
                this.ConOrderFill(conOrderDataTable, this.isShowPagingControl);
            }
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

        private void QueryPagingConOrderMemoryInfo(ConditionQueryRequestVO queryConOrderReqVO)
        {
            DataSet set = null;
            this.conOrderAllNum = base.GetAllDataCount(set.Tables["Corder"], this.sumNmaes);
            this.ConOrderSetPage();
            DataTable conOrderDataTable = base.GetDataTable(set.Tables["Corder"], this.conOrderSql, this.conOrderSortFld + this.conOrderSort, 1);
            base.DataViewAddQueryDgUnTradeSum(conOrderDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.ConOrderFill != null)
            {
                this.ConOrderFill(conOrderDataTable, this.isShowPagingControl);
            }
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
            }
            else
            {
                this.QueryConOrderMemoryInfo(null);
            }
        }

        public void SetConOrderIsPaging(bool isPagingQuery)
        {
            this.isConOrderNew = isPagingQuery;
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryConOrderFlag = flag;
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

        public delegate void ConOrderFillCallBack(DataTable conOrderDataTable, bool _isShowPagingControl);
    }
}
