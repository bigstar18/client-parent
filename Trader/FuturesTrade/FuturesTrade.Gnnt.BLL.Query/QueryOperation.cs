using FuturesTrade.Gnnt.DBService.ServiceManager;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using ToolsLibrary.util;
using TPME.Log;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryOperation
	{
		public delegate void RefreshCurrentTabCallBack(int mark, bool flag);
		private string Total = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
		private string TotalNum = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_TotalNum");
		public int refreshTime = Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 20);
		public int ButtonRefreshFlag;
		public int IdleOnMoudel;
		public int refreshButtonTime = Tools.StrToInt((string)Global.HTConfig["MaxIdleRefreshButton"], 5);
		public int IdleRefreshButton;
		public ServiceManage serviceManage = ServiceManage.GetInstance();
		public QueryOperation.RefreshCurrentTabCallBack RefreshCurrentTab;
		public bool IsOutRefreshTime()
		{
			bool result = false;
			if (this.ButtonRefreshFlag == 1)
			{
				if (this.IdleRefreshButton > this.refreshButtonTime)
				{
					result = true;
				}
			}
			else
			{
				if (this.IdleOnMoudel > this.refreshTime)
				{
					result = true;
				}
			}
			return result;
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
			DataTable dataTable = defaultView.Table.Clone();
			dataTable.Clear();
			if (currentPageNum == 0)
			{
				currentPageNum = 1;
			}
			int num = (currentPageNum - 1) * Global.PagNum;
			while (num < currentPageNum * Global.PagNum && num < defaultView.Count)
			{
				if (flag)
				{
					if (num != defaultView.Count - 1)
					{
						dataTable.ImportRow(defaultView[num].Row);
					}
				}
				else
				{
					dataTable.ImportRow(defaultView[num].Row);
				}
				num++;
			}
			if (flag && dataTable.Rows.Count > 0)
			{
				dataTable.ImportRow(defaultView[defaultView.Count - 1].Row);
			}
			return dataTable;
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
			DataTable dataTable = defaultView.Table.Clone();
			dataTable.Clear();
			for (int i = 0; i < defaultView.Count; i++)
			{
				dataTable.ImportRow(defaultView[i].Row);
			}
			return dataTable;
		}
		public int GetAllDataCount(DataTable dtable, string[] sumNames)
		{
			int result = 0;
			try
			{
				int count = dtable.Rows.Count;
				string text = dtable.Rows[count - 1][sumNames[1]].ToString();
				text = text.Substring(1, text.Length - 2);
				result = Tools.StrToInt(text, 0);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
			return result;
		}
		public void DataViewAddQueryDgUnTradeSum(DataView dataView, string[] sumNames, Hashtable sumHashtable)
		{
			try
			{
				if (sumNames != null && sumNames.Length != 0)
				{
					if (dataView.Count > 1 && dataView[dataView.Count - 2].Row[sumNames[0]].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(dataView.Count - 2);
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
							int num = 0;
							double num2 = 0.0;
							for (int k = 0; k < dataView.Count; k++)
							{
								if (dataView[k].Row[sumNames[j]].GetType() == typeof(int))
								{
									num += Tools.StrToInt(dataView[k].Row[sumNames[j]].ToString(), 0);
								}
								else
								{
									if (dataView[k].Row[sumNames[j]].GetType() == typeof(double))
									{
										num2 += Tools.StrToDouble(dataView[k].Row[sumNames[j]].ToString(), 0.0);
									}
								}
							}
							if (num2 == 0.0)
							{
								sumHashtable[sumNames[j]] = num;
							}
							else
							{
								if (num == 0)
								{
									sumHashtable[sumNames[j]] = num2;
								}
							}
						}
						DataRowView dataRowView = dataView.AddNew();
						dataRowView[sumNames[0]] = this.Total;
						dataRowView[sumNames[1]] = string.Format(this.TotalNum, dataView.Count - 1);
						for (int l = 2; l < sumNames.Length; l++)
						{
							if (dataView[0].Row[sumNames[l]].GetType() == typeof(int))
							{
								dataRowView[sumNames[l]] = Tools.StrToInt(sumHashtable[sumNames[l]].ToString(), 0);
							}
							else
							{
								if (dataView[0].Row[sumNames[l]].GetType() == typeof(double))
								{
									dataRowView[sumNames[l]] = Tools.StrToDouble(sumHashtable[sumNames[l]].ToString(), 0.0);
								}
							}
						}
						dataRowView["AutoID"] = 100000;
						dataRowView.EndEdit();
						dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		public short GetDescOrAsc(string sortWay)
		{
			short result = 1;
			if (!string.IsNullOrEmpty(sortWay))
			{
				if (sortWay.ToUpper() == "DESC")
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
