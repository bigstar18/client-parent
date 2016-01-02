namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryHoldingOperation : QueryOperation
    {
        public HoldingFillCallBack HoldingFillEvent;
        private string holdingSort = " ASC";
        private string holdingSortFld = "CommodityID";
        private string holdingSql = " 1=1 ";
        private bool QueryHoldingFlag = true;
        private bool refreshHoldingFlag = true;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "CommodityID", "TransactionsCode", "BuyHolding", "SellHolding", "GoodsQty", "Margin", "Floatpl" };

        public QueryHoldingOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private HoldingQueryRequestVO FillHoldingQueryReqVO()
        {
            return new HoldingQueryRequestVO { UserID = Global.UserID };
        }

        public void HoldingScreen(string sql)
        {
            this.holdingSql = sql;
            this.QueryHoldingMemoryInfo(null);
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

        private void QueryHoldingInfo(object _holdingQueryRequestVO)
        {
            if (this.refreshHoldingFlag)
            {
                this.refreshHoldingFlag = false;
                HoldingQueryRequestVO holdingQueryReqVO = this.FillHoldingQueryReqVO();
                this.QueryHoldingMemoryInfo(holdingQueryReqVO);
                this.refreshHoldingFlag = true;
            }
        }

        public void QueryHoldingInfoLoad()
        {
            if (this.QueryHoldingFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                WaitCallback callBack = new WaitCallback(this.QueryHoldingInfo);
                ThreadPool.QueueUserWorkItem(callBack);
                this.QueryHoldingFlag = false;
            }
        }

        private void QueryHoldingMemoryInfo(HoldingQueryRequestVO holdingQueryReqVO)
        {
            DataSet set = base.serviceManage.CreateQueryHolding().QueryHoldingInfo(holdingQueryReqVO);
            DataTable holdingDataTable = base.GetDataTable(set.Tables["Holding"], this.holdingSql, this.holdingSortFld + this.holdingSort);
            base.DataViewAddQueryDgUnTradeSum(holdingDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.HoldingFillEvent != null)
            {
                this.HoldingFillEvent(holdingDataTable);
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryHoldingFlag = flag;
        }

        public delegate void HoldingFillCallBack(DataTable holdingDataTable);
    }
}
