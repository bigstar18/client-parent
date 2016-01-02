using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryPredelegateOperation : QueryOperation
	{
		public delegate void PreDelegateFillCallBack(DataTable preDelegateDataTable);
		public delegate void SetMaxIDCallBack(int maxid);
		private bool QueryPreDelegateFlag = true;
		private string preDetegateSortFld = "ID";
		private string preDelegateSql = " 1=1 ";
		private string preDelegateSort = " DESC";
		private string[] sumNmaes = new string[]
		{
			"TransactionsCode",
			"CommodityCode",
			"Qty"
		};
		private string[] ids;
		private int maxID;
		private Hashtable sumNamesHashtable = new Hashtable();
		private XmlDataSet XmlPreDelegate;
		private DataSet dsPreDelegate;
		private bool isLoadPreDelegate = true;
		public QueryPredelegateOperation.PreDelegateFillCallBack PreDelegateFill;
		public QueryPredelegateOperation.SetMaxIDCallBack setMaxID;
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
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.PreDelegateXml);
			xmlDataSet.WriteNewXml(Global.ConfigPath + Global.PreDelegateXml, Global.ConfigPath + Global.UserID + Global.PreDelegateXml);
		}
		public void QueryPreDelegateLoad()
		{
			WaitCallback callBack = new WaitCallback(this.QueryPreDelegate);
			ThreadPool.QueueUserWorkItem(callBack);
		}
		private void QueryPreDelegate(object _preRequest)
		{
			if (!this.QueryPreDelegateFlag)
			{
				return;
			}
			this.QueryPreDelegateFlag = false;
			this.isLoadPreDelegate = true;
			this.QueryPreDelegateInfo();
			this.QueryPreDelegateFlag = true;
		}
		private void QueryPreDelegateInfo()
		{
			try
			{
				DataTable preDelegateDataTable = this.GetPreDelegateDataTable();
				if (preDelegateDataTable != null)
				{
					DataTable dataTable = base.GetDataTable(preDelegateDataTable, this.preDelegateSql, this.preDetegateSortFld + this.preDelegateSort);
					base.DataViewAddQueryDgUnTradeSum(dataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
					if (this.PreDelegateFill != null)
					{
						this.PreDelegateFill(dataTable);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private DataTable GetPreDelegateDataTable()
		{
			DataTable result;
			try
			{
				if (this.isLoadPreDelegate)
				{
					this.dsPreDelegate = this.XmlPreDelegate.GetDataSetByXml();
				}
				DataTable dataTable = null;
				DataTable dataTable2 = null;
				if (this.dsPreDelegate != null && this.dsPreDelegate.Tables != null && this.dsPreDelegate.Tables[0] != null)
				{
					dataTable = this.dsPreDelegate.Tables[0];
					if (this.dsPreDelegate.Tables[1] != null)
					{
						dataTable2 = this.dsPreDelegate.Tables[1];
					}
				}
				if (dataTable2 != null)
				{
					this.maxID = int.Parse(this.dsPreDelegate.Tables[1].Rows[0][0].ToString()) + 1;
					this.setMaxID(this.maxID);
				}
				result = dataTable;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
				result = null;
			}
			return result;
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
		public void PreDelegateScreen(string sql)
		{
			this.preDelegateSql = sql;
			this.isLoadPreDelegate = false;
			this.QueryPreDelegateInfo();
		}
		public void DeletePreDelegate(DataGridView dgPreDelegate)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = dgPreDelegate.Rows.Count - 1; i >= 0; i--)
			{
				if (dgPreDelegate["SelectFlag", i].Value != null && (bool)dgPreDelegate["SelectFlag", i].Value)
				{
					text = text + dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
				}
			}
			if (!text.Equals(""))
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderInfoTitle");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderInfoContent");
				MessageForm messageForm = new MessageForm(@string, string2, 0);
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					text = text.Remove(text.Length - 1);
					string[] idColumns = text.Split(new char[]
					{
						'_'
					});
					string text3 = this.PreDelegateDatasDelete(idColumns);
					if (text3.Equals("true"))
					{
						string string3 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteSuccess");
						text2 = string3;
					}
					else
					{
						string string4 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteFail");
						text2 = string4;
					}
					this.QueryPreDelegateLoad();
				}
			}
			else
			{
				string string5 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_MustSelectOneRecord");
				text2 = string5;
			}
			if (!text2.Equals(""))
			{
				string string6 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DeleteOrderResult");
				MessageForm messageForm = new MessageForm(string6, text2, 1);
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
		private string PreDelegateDatasDelete(string[] idColumns)
		{
			return this.XmlPreDelegate.DeleteXmlRows("id", idColumns);
		}
		public void PreDelegateOrder(DataGridView dgPreDelegate)
		{
			ArrayList arrayList = new ArrayList();
			string arg_0B_0 = string.Empty;
			string text = string.Empty;
			for (int i = 0; i <= dgPreDelegate.Rows.Count - 1; i++)
			{
				if (dgPreDelegate["SelectFlag", i].Value != null && (bool)dgPreDelegate["SelectFlag", i].Value)
				{
					arrayList.Add(new OrderRequestVO
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
					});
					text = text + dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
				}
			}
			if (!text.Equals(""))
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EntrustOperationTitle");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EntrustOperationContent");
				MessageForm messageForm = new MessageForm(@string, string2, 0);
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					Global.M_ResourceManager.GetString("TradeStr_MainFormF8_DataSubmiting");
					text = text.Remove(text.Length - 1);
					this.ids = text.Split(new char[]
					{
						'_'
					});
					this.OrderPreDelegateThread(arrayList);
					return;
				}
			}
			else
			{
				string string3 = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_MustSelectOneRecord");
				string string4 = Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder");
				MessageForm messageForm = new MessageForm(string4, string3, 1);
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
		public void OrderPreDelegateThread(object _orderArr)
		{
			WaitCallback callBack = new WaitCallback(this.OrderPreDelegate);
			ThreadPool.QueueUserWorkItem(callBack, _orderArr);
		}
		private void OrderPreDelegate(object _orderArr)
		{
			ArrayList arrayList = (ArrayList)_orderArr;
			ResponseVO[] array = new ResponseVO[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				OrderRequestVO req = (OrderRequestVO)arrayList[i];
				array[i] = this.serviceManage.CreateEntrustOrder().Order(req);
			}
			if (arrayList.Count > 0)
			{
				this.OrderMessage(array, this.ids);
			}
		}
		private void OrderMessage(ResponseVO[] responseVOArr, string[] idColumns)
		{
			bool flag = true;
			string text = string.Empty;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < responseVOArr.Length; i++)
			{
				ResponseVO responseVO = responseVOArr[i];
				if (responseVO.RetCode != 0L)
				{
					flag = false;
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF8_EmbeddedOrder");
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"[",
						@string,
						idColumns[i],
						"：",
						responseVO.RetMessage.Trim(),
						"]"
					});
				}
				else
				{
					arrayList.Add(idColumns[i]);
				}
			}
			if (!flag)
			{
				Global.StatusInfoFill(text, Global.ErrorColor, true);
			}
			idColumns = null;
			if (arrayList.Count > 0)
			{
				Global.orderCount += arrayList.Count;
				this.XmlPreDelegate.DeleteXmlRows("id", (string[])arrayList.ToArray(typeof(string)));
				this.QueryPreDelegate(null);
			}
		}
		public string SubmitPredelegateInfo(string[] Columns, string[] ColumnValue)
		{
			string text = this.XmlPreDelegate.WriteXmlByDataSet(Columns, ColumnValue);
			if (text.Equals("true"))
			{
				this.XmlPreDelegate.UpdateXmlCounter(this.maxID);
			}
			return text;
		}
	}
}
