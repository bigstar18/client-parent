namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryHoldingDetailOperation : QueryOperation
    {
        public HoldingDetailFillCallBack HoldingDetailFill;
        private string holdingDetailSort = " ASC ";
        private string holdingDetailSortFld = "CommodityID";
        private string holdingDetailSql = " 1=1 ";
        private bool QueryHoldingDetailFlag = true;
        private bool refreshHoldingDetailFlag = true;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "CommodityID", "TransactionsCode", "Cur_Open", "Margin", "GoodsQty" };

        public QueryHoldingDetailOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private HoldingDetailRequestVO FillHoldingDetailQueryReqVO()
        {
            return new HoldingDetailRequestVO { UserID = Global.UserID };
        }

        public void HoldingDetailScreen(string sql)
        {
            this.holdingDetailSql = sql;
            this.QueryHoldingDetailMemoryInfo(null);
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

        private void QueryHoldingDetailInfo(object _holdingDetailQueryRequestVO)
        {
            if (this.refreshHoldingDetailFlag)
            {
                this.refreshHoldingDetailFlag = false;
                HoldingDetailRequestVO holdingDetailQueryReqVO = this.FillHoldingDetailQueryReqVO();
                this.QueryHoldingDetailMemoryInfo(holdingDetailQueryReqVO);
                this.refreshHoldingDetailFlag = true;
            }
        }

        public void QueryHoldingDetailInfoLoad()
        {
            if (this.QueryHoldingDetailFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                WaitCallback callBack = new WaitCallback(this.QueryHoldingDetailInfo);
                ThreadPool.QueueUserWorkItem(callBack);
                this.QueryHoldingDetailFlag = false;
            }
        }

        private void QueryHoldingDetailMemoryInfo(HoldingDetailRequestVO holdingDetailQueryReqVO)
        {
            DataSet set = base.serviceManage.CreateQueryHoldingDetail().QueryHoldingDetailInfo(holdingDetailQueryReqVO);
            DataTable holdingDetailDataTable = base.GetDataTable(set.Tables["HoldingDetail"], this.holdingDetailSql, this.holdingDetailSortFld + this.holdingDetailSort);
            base.DataViewAddQueryDgUnTradeSum(holdingDetailDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.HoldingDetailFill != null)
            {
                this.HoldingDetailFill(holdingDetailDataTable);
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryHoldingDetailFlag = flag;
        }

        public delegate void HoldingDetailFillCallBack(DataTable holdingDetailDataTable);
    }
}
