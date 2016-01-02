namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryPredelegateOperation : QueryOperation
    {
        private DataSet dsPreDelegate;
        private string[] ids;
        private bool isLoadPreDelegate = true;
        private int maxID;
        public PreDelegateFillCallBack PreDelegateFill;
        private string preDelegateSort = " DESC";
        private string preDelegateSql = " 1=1 ";
        private string preDetegateSortFld = "ID";
        private bool QueryPreDelegateFlag = true;
        public SetMaxIDCallBack setMaxID;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "TransactionsCode", "CommodityCode", "Qty" };
        private XmlDataSet XmlPreDelegate;

        public QueryPredelegateOperation()
        {
            this.CreateUserPredelegate();
            this.XmlPreDelegate = new XmlDataSet(Global.ConfigPath + Global.UserID + Global.PreDelegateXml);
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private void CreateUserPredelegate()
        {
            new XmlDataSet(Global.ConfigPath + Global.PreDelegateXml).WriteNewXml(Global.ConfigPath + Global.PreDelegateXml, Global.ConfigPath + Global.UserID + Global.PreDelegateXml);
        }

        public void DeletePreDelegate(DataGridView dgPreDelegate)
        {
            MessageForm form;
            string str2 = string.Empty;
            string message = string.Empty;
            for (int i = dgPreDelegate.Rows.Count - 1; i >= 0; i--)
            {
                if ((dgPreDelegate["SelectFlag", i].Value != null) && ((bool)dgPreDelegate["SelectFlag", i].Value))
                {
                    str2 = str2 + dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
                }
            }
            if (!str2.Equals(""))
            {
                string formName = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderInfoTitle");
                string str5 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderInfoContent");
                form = new MessageForm(formName, str5, 0);
                form.ShowDialog();
                form.Dispose();
                if (form.isOK)
                {
                    string[] idColumns = str2.Remove(str2.Length - 1).Split(new char[] { '_' });
                    if (this.PreDelegateDatasDelete(idColumns).Equals("true"))
                    {
                        message = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteSuccess");
                    }
                    else
                    {
                        message = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteFail");
                    }
                    this.QueryPreDelegateLoad();
                }
            }
            else
            {
                message = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_MustSelectOneRecord");
            }
            if (!message.Equals(""))
            {
                form = new MessageForm(Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderResult"), message, 1);
                form.ShowDialog();
                form.Dispose();
            }
        }

        private DataTable GetPreDelegateDataTable()
        {
            try
            {
                if (this.isLoadPreDelegate)
                {
                    this.dsPreDelegate = this.XmlPreDelegate.GetDataSetByXml();
                }
                DataTable table = null;
                DataTable table2 = null;
                if (((this.dsPreDelegate != null) && (this.dsPreDelegate.Tables != null)) && (this.dsPreDelegate.Tables[0] != null))
                {
                    table = this.dsPreDelegate.Tables[0];
                    if (this.dsPreDelegate.Tables[1] != null)
                    {
                        table2 = this.dsPreDelegate.Tables[1];
                    }
                }
                if (table2 != null)
                {
                    this.maxID = int.Parse(this.dsPreDelegate.Tables[1].Rows[0][0].ToString()) + 1;
                    this.setMaxID(this.maxID);
                }
                return table;
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
                return null;
            }
        }

        private void OrderMessage(ResponseVO[] responseVOArr, string[] idColumns)
        {
            bool flag = true;
            string message = string.Empty;
            ArrayList list = new ArrayList();
            for (int i = 0; i < responseVOArr.Length; i++)
            {
                ResponseVO evo = responseVOArr[i];
                if (evo.RetCode != 0L)
                {
                    flag = false;
                    string str2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EmbeddedOrder");
                    string str3 = message;
                    message = str3 + "[" + str2 + idColumns[i] + "：" + evo.RetMessage.Trim() + "]";
                }
                else
                {
                    list.Add(idColumns[i]);
                }
            }
            if (!flag)
            {
                Global.StatusInfoFill(message, Global.ErrorColor, true);
            }
            idColumns = null;
            if (list.Count > 0)
            {
                Global.orderCount += list.Count;
                this.XmlPreDelegate.DeleteXmlRows("id", (string[])list.ToArray(typeof(string)));
                this.QueryPreDelegate(null);
            }
        }

        private void OrderPreDelegate(object _orderArr)
        {
            ArrayList list = (ArrayList)_orderArr;
            ResponseVO[] responseVOArr = new ResponseVO[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                OrderRequestVO req = (OrderRequestVO)list[i];
                responseVOArr[i] = base.serviceManage.CreateEntrustOrder().Order(req);
            }
            if (list.Count > 0)
            {
                this.OrderMessage(responseVOArr, this.ids);
            }
        }

        public void OrderPreDelegateThread(object _orderArr)
        {
            WaitCallback callBack = new WaitCallback(this.OrderPreDelegate);
            ThreadPool.QueueUserWorkItem(callBack, _orderArr);
        }

        private string PreDelegateDatasDelete(string[] idColumns)
        {
            return this.XmlPreDelegate.DeleteXmlRows("id", idColumns);
        }

        public void PreDelegateOrder(DataGridView dgPreDelegate)
        {
            MessageForm form;
            ArrayList list = new ArrayList();
            string str = string.Empty;
            for (int i = 0; i <= (dgPreDelegate.Rows.Count - 1); i++)
            {
                if ((dgPreDelegate["SelectFlag", i].Value != null) && ((bool)dgPreDelegate["SelectFlag", i].Value))
                {
                    OrderRequestVO tvo = new OrderRequestVO
                    {
                        UserID = Global.UserID,
                        CustomerID = dgPreDelegate["TransactionsCode", i].Value.ToString(),
                        BuySell = Global.StrToShort(Global.BuySellStrArr, dgPreDelegate["B_S", i].Value.ToString()),
                        MarketID = dgPreDelegate["MarKet", i].Value.ToString(),
                        CommodityID = dgPreDelegate["commodityCode", i].Value.ToString(),
                        Price = Tools.StrToDouble(dgPreDelegate["price", i].Value.ToString()),
                        Quantity = Tools.StrToLong(dgPreDelegate["qty", i].Value.ToString()),
                        SettleBasis = Global.StrToShort(Global.SettleBasisStrArr, dgPreDelegate["O_L", i].Value.ToString()),
                        LPrice = Tools.StrToDouble(dgPreDelegate["LPrice", i].Value.ToString()),
                        CloseMode = Tools.StrToShort(dgPreDelegate["CloseMode", i].Value.ToString()),
                        TimeFlag = Tools.StrToShort(dgPreDelegate["TimeFlag", i].Value.ToString()),
                        BillType = 0
                    };
                    list.Add(tvo);
                    str = str + dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
                }
            }
            if (!str.Equals(""))
            {
                string formName = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EntrustOperationTitle");
                string message = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EntrustOperationContent");
                form = new MessageForm(formName, message, 0);
                form.ShowDialog();
                form.Dispose();
                if (form.isOK)
                {
                    Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DataSubmiting");
                    this.ids = str.Remove(str.Length - 1).Split(new char[] { '_' });
                    this.OrderPreDelegateThread(list);
                }
            }
            else
            {
                string str4 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_MustSelectOneRecord");
                form = new MessageForm(Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder"), str4, 1);
                form.ShowDialog();
                form.Dispose();
            }
        }

        public void PreDelegateScreen(string sql)
        {
            this.preDelegateSql = sql;
            this.isLoadPreDelegate = false;
            this.QueryPreDelegateInfo();
        }

        public void PredelegateSort(string sortName)
        {
            this.preDetegateSortFld = sortName;
            if (this.preDelegateSort == " ASC ")
            {
                this.preDelegateSort = " Desc ";
            }
            else
            {
                this.preDelegateSort = " ASC ";
            }
            this.isLoadPreDelegate = false;
            this.QueryPreDelegateInfo();
        }

        private void QueryPreDelegate(object _preRequest)
        {
            if (this.QueryPreDelegateFlag)
            {
                this.QueryPreDelegateFlag = false;
                this.isLoadPreDelegate = true;
                this.QueryPreDelegateInfo();
                this.QueryPreDelegateFlag = true;
            }
        }

        private void QueryPreDelegateInfo()
        {
            try
            {
                DataTable preDelegateDataTable = this.GetPreDelegateDataTable();
                if (preDelegateDataTable != null)
                {
                    DataTable table2 = base.GetDataTable(preDelegateDataTable, this.preDelegateSql, this.preDetegateSortFld + this.preDelegateSort);
                    base.DataViewAddQueryDgUnTradeSum(table2.DefaultView, this.sumNmaes, this.sumNamesHashtable);
                    if (this.PreDelegateFill != null)
                    {
                        this.PreDelegateFill(table2);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        public void QueryPreDelegateLoad()
        {
            WaitCallback callBack = new WaitCallback(this.QueryPreDelegate);
            ThreadPool.QueueUserWorkItem(callBack);
        }

        public string SubmitPredelegateInfo(string[] Columns, string[] ColumnValue)
        {
            string str = this.XmlPreDelegate.WriteXmlByDataSet(Columns, ColumnValue);
            if (str.Equals("true"))
            {
                this.XmlPreDelegate.UpdateXmlCounter(this.maxID);
            }
            return str;
        }

        public delegate void PreDelegateFillCallBack(DataTable preDelegateDataTable);

        public delegate void SetMaxIDCallBack(int maxid);
    }
}
