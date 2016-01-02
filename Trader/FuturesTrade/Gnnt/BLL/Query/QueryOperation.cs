namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.DBService.ServiceManager;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using ToolsLibrary.util;
    using TPME.Log;

    public class QueryOperation
    {
        public int ButtonRefreshFlag;
        public int IdleOnMoudel;
        public int IdleRefreshButton;
        public int refreshButtonTime = Tools.StrToInt((string)Global.HTConfig["MaxIdleRefreshButton"], 5);
        public RefreshCurrentTabCallBack RefreshCurrentTab;
        public int refreshTime = Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 20);
        public ServiceManage serviceManage = ServiceManage.GetInstance();
        private string Total = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
        private string TotalNum = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_TotalNum");

        public void DataViewAddQueryDgUnTradeSum(DataView dataView, string[] sumNames, Hashtable sumHashtable)
        {
            try
            {
                if ((sumNames != null) && (sumNames.Length != 0))
                {
                    if ((dataView.Count > 1) && (dataView[dataView.Count - 2].Row[sumNames[0]].ToString() == this.Total))
                    {
                        dataView.AllowDelete = true;
                        dataView.Delete((int)(dataView.Count - 2));
                    }
                    else
                    {
                        for (int i = 0; i < dataView.Count; i++)
                        {
                            if (dataView[i].Row[sumNames[0]].ToString() == this.Total)
                            {
                                dataView.AllowDelete = true;
                                dataView.Delete(i);
                            }
                        }
                    }
                    if (dataView.Count > 1)
                    {
                        dataView.AllowNew = true;
                        if (!dataView.Table.Columns.Contains("AutoID"))
                        {
                            dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
                        }
                        for (int j = 2; j < sumNames.Length; j++)
                        {
                            int num3 = 0;
                            double num4 = 0.0;
                            for (int m = 0; m < dataView.Count; m++)
                            {
                                if (dataView[m].Row[sumNames[j]].GetType() == typeof(int))
                                {
                                    num3 += Tools.StrToInt(dataView[m].Row[sumNames[j]].ToString(), 0);
                                }
                                else if (dataView[m].Row[sumNames[j]].GetType() == typeof(double))
                                {
                                    num4 += Tools.StrToInt(dataView[m].Row[sumNames[j]].ToString(), 0);
                                }
                            }
                            if (num4 == 0.0)
                            {
                                sumHashtable[sumNames[j]] = num3;
                            }
                            else if (num3 == 0)
                            {
                                sumHashtable[sumNames[j]] = num4;
                            }
                        }
                        DataRowView view = dataView.AddNew();
                        view[sumNames[0]] = this.Total;
                        view[sumNames[1]] = string.Format(this.TotalNum, dataView.Count - 1);
                        for (int k = 2; k < sumNames.Length; k++)
                        {
                            if (dataView[0].Row[sumNames[k]].GetType() == typeof(int))
                            {
                                view[sumNames[k]] = Tools.StrToInt(sumHashtable[sumNames[k]].ToString(), 0);
                            }
                            else if (dataView[0].Row[sumNames[k]].GetType() == typeof(double))
                            {
                                view[sumNames[k]] = Tools.StrToDouble(sumHashtable[sumNames[k]].ToString(), 0.0);
                            }
                        }
                        view["AutoID"] = 0x186a0;
                        view.EndEdit();
                        dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        public int GetAllDataCount(DataTable dtable, string[] sumNames)
        {
            int num = 0;
            try
            {
                int count = dtable.Rows.Count;
                string str = dtable.Rows[count - 1][sumNames[1]].ToString();
                num = Tools.StrToInt(str.Substring(1, str.Length - 2), 0);
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
            return num;
        }

        public DataTable GetDataTable(DataTable dtable, string sql, string sort)
        {
            DataView defaultView = dtable.DefaultView;
            try
            {
                defaultView.RowFilter = sql + " OR AutoID = '100000'";
                defaultView.Sort = " AutoID ASC, " + sort;
            }
            catch (Exception)
            {
                defaultView.RowFilter = sql;
                defaultView.Sort = sort;
            }
            DataTable table = defaultView.Table.Clone();
            table.Clear();
            for (int i = 0; i < defaultView.Count; i++)
            {
                table.ImportRow(defaultView[i].Row);
            }
            return table;
        }

        public DataTable GetDataTable(DataTable dtable, string sql, string sort, int currentPageNum)
        {
            DataView defaultView = dtable.DefaultView;
            bool flag = false;
            try
            {
                defaultView.RowFilter = sql + " OR AutoID = '100000'";
                defaultView.Sort = " AutoID ASC, " + sort;
                flag = true;
            }
            catch (Exception)
            {
                defaultView.RowFilter = sql;
                defaultView.Sort = sort;
                flag = false;
            }
            DataTable table = defaultView.Table.Clone();
            table.Clear();
            if (currentPageNum == 0)
            {
                currentPageNum = 1;
            }
            for (int i = (currentPageNum - 1) * Global.PagNum; i < (currentPageNum * Global.PagNum); i++)
            {
                if (i >= defaultView.Count)
                {
                    break;
                }
                if (flag)
                {
                    if (i != (defaultView.Count - 1))
                    {
                        table.ImportRow(defaultView[i].Row);
                    }
                }
                else
                {
                    table.ImportRow(defaultView[i].Row);
                }
            }
            if (flag && (table.Rows.Count > 0))
            {
                table.ImportRow(defaultView[defaultView.Count - 1].Row);
            }
            return table;
        }

        public short GetDescOrAsc(string sortWay)
        {
            short num = 1;
            if (string.IsNullOrEmpty(sortWay))
            {
                return num;
            }
            if (sortWay.ToUpper() == "DESC")
            {
                return 1;
            }
            return 0;
        }

        public bool IsOutRefreshTime()
        {
            bool flag = false;
            if (this.ButtonRefreshFlag == 1)
            {
                if (this.IdleRefreshButton > this.refreshButtonTime)
                {
                    flag = true;
                }
                return flag;
            }
            if (this.IdleOnMoudel > this.refreshTime)
            {
                flag = true;
            }
            return flag;
        }

        public delegate void RefreshCurrentTabCallBack(int mark, bool flag);
    }
}
