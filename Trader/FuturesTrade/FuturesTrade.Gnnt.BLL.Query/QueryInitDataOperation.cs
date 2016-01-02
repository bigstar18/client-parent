using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QueryInitDataOperation : QueryOperation
	{
		public delegate void IsPagingQueryCallBack(bool _isOrderNew, bool _isTradeNew);
		public delegate void FundsFillCallBack(FirmInfoResponseVO firmInfoResponseVO);
		public delegate void ComboCommodityLoadCallBack();
		public delegate void TransactionLoadCallBack();
		public Hashtable ht_TradeMode;
		public Hashtable ht_Variety;
		public QueryInitDataOperation.IsPagingQueryCallBack isPagingQueryCallBack;
		public QueryInitDataOperation.FundsFillCallBack FundsFill;
		public QueryInitDataOperation.ComboCommodityLoadCallBack ComboCommodityLoad;
		public QueryInitDataOperation.TransactionLoadCallBack TransactionLoad;
		private bool QueryFundsFlag = true;
		private bool refreshFirmInfoFlag = true;
		public void InitData()
		{
			this.GetCommodity();
			this.GetFirmInfoList();
			this.serviceManage.CreateQueryInitData().QueryMarketInfo();
			this.serviceManage.CreateQueryInitData().QueryFirmbreed();
			this.QueryDataQty();
		}
		public void GetCommodity()
		{
			this.ht_TradeMode = new Hashtable();
			this.ht_Variety = new Hashtable();
			string path = Global.ConfigPath + Global.CommCodeXml.Substring(0, Global.CommCodeXml.IndexOf(".")) + ".xsd";
			if (!File.Exists(Global.ConfigPath + Global.CommCodeXml) || !File.Exists(path))
			{
				throw new Exception("没有发现商品架构文件！");
			}
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
			DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
			this.serviceManage.CreateQueryInitData().QueryCommodity("", "");
			Hashtable commodityHashtable = TradeDataInfo.CommodityHashtable;
			if (commodityHashtable != null && commodityHashtable.Count > 0)
			{
				string text = string.Empty;
				foreach (DataRow dataRow in dataSetByXml.Tables[0].Rows)
				{
					if ((bool)dataRow["Flag"])
					{
						text = text + dataRow["commodityCode"] + "-";
					}
				}
				xmlDataSet.DeleteXmlAllRows();
				string[] columns = new string[]
				{
					"Flag",
					"ID",
					"CommodityCode",
					"MarKet"
				};
				string[] array = new string[4];
				int num = 0;
				foreach (DictionaryEntry dictionaryEntry in commodityHashtable)
				{
					CommodityInfo commodityInfo = (CommodityInfo)dictionaryEntry.Value;
					array[0] = "false";
					array[1] = num.ToString();
					array[2] = commodityInfo.CommodityID.Trim();
					array[3] = commodityInfo.MarketID;
					xmlDataSet.WriteBoolXml(columns, array);
					if (commodityInfo.TradeMode.ToString().Trim().Length > 0)
					{
						this.ht_TradeMode.Add(commodityInfo.CommodityID.Trim(), commodityInfo.TradeMode.ToString());
						this.ht_Variety.Add(commodityInfo.CommodityID.Trim(), commodityInfo.VarietyID.ToString());
					}
					num++;
				}
				string[] columns2 = new string[]
				{
					"Flag"
				};
				string[] columnValue = new string[]
				{
					"true"
				};
				string[] array2 = text.Split(new char[]
				{
					'-'
				});
				for (int i = 0; i < array2.Length - 1; i++)
				{
					xmlDataSet.UpdateXmlRow(columns2, columnValue, "commodityCode", array2[i]);
				}
			}
			else
			{
				xmlDataSet.DeleteXmlAllRows();
			}
			if (this.ComboCommodityLoad != null)
			{
				this.ComboCommodityLoad();
			}
		}
		public void GetFirmInfoList()
		{
			string path = Global.ConfigPath + Global.TrancCodeXml.Substring(0, Global.TrancCodeXml.IndexOf(".")) + ".xsd";
			if (!File.Exists(Global.ConfigPath + Global.TrancCodeXml) || !File.Exists(path))
			{
				throw new Exception("没有发现交易员架构文件！");
			}
			FirmInfoResponseVO firmInfoResponseVO = this.serviceManage.CreateQueryInitData().QueryFundsInfo();
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml);
			DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
			Global.FirmID = firmInfoResponseVO.FirmID;
			Global.CustomerCount = firmInfoResponseVO.CDS.Count;
			if (firmInfoResponseVO.CDS.Count > 0)
			{
				string text = string.Empty;
				foreach (DataRow dataRow in dataSetByXml.Tables[0].Rows)
				{
					if ((bool)dataRow["Flag"])
					{
						text = text + dataRow["TransactionsCode"] + "-";
					}
				}
				xmlDataSet.DeleteXmlAllRows();
				string[] columns = new string[]
				{
					"Flag",
					"ID",
					"TransactionsCode"
				};
				string[] array = new string[3];
				for (int i = 0; i < firmInfoResponseVO.CDS.Count; i++)
				{
					array[0] = "false";
					array[1] = i.ToString();
					Code code = firmInfoResponseVO.CDS[i];
					array[2] = firmInfoResponseVO.FirmID + code.CD;
					xmlDataSet.WriteBoolXml(columns, array);
				}
				string[] columns2 = new string[]
				{
					"Flag"
				};
				string[] columnValue = new string[]
				{
					"true"
				};
				string[] array2 = text.Split(new char[]
				{
					'-'
				});
				for (int j = 0; j < array2.Length - 1; j++)
				{
					xmlDataSet.UpdateXmlRow(columns2, columnValue, "TransactionsCode", array2[j]);
				}
			}
			else
			{
				xmlDataSet.DeleteXmlAllRows();
				string[] columns3 = new string[]
				{
					"Flag",
					"ID",
					"TransactionsCode"
				};
				xmlDataSet.WriteBoolXml(columns3, new string[]
				{
					"false",
					"0",
					firmInfoResponseVO.FirmID
				});
			}
			if (this.TransactionLoad != null)
			{
				this.TransactionLoad();
			}
		}
		public void QueryFirmInfoThread()
		{
			if (this.QueryFundsFlag || base.IsOutRefreshTime())
			{
				this.ButtonRefreshFlag = 0;
				WaitCallback callBack = new WaitCallback(this.QueryFirmInfo);
				ThreadPool.QueueUserWorkItem(callBack);
				this.QueryFundsFlag = false;
			}
		}
		private void QueryFirmInfo(object o)
		{
			if (!this.refreshFirmInfoFlag)
			{
				return;
			}
			this.refreshFirmInfoFlag = false;
			FirmInfoResponseVO firmInfoResponseVO = this.serviceManage.CreateQueryInitData().QueryFundsInfo();
			if (this.FundsFill != null)
			{
				this.FundsFill(firmInfoResponseVO);
			}
			this.refreshFirmInfoFlag = true;
		}
		public void SetQueryUnOrderFlag(bool flag)
		{
			this.QueryFundsFlag = flag;
		}
		public void QueryDataQty()
		{
			List<TotalRow> list = this.serviceManage.CreateQueryInitData().Querydateqty();
			bool isOrderNew = false;
			bool isTradeNew = false;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					TotalRow totalRow = list[i];
					if (totalRow.ResponseName == "my_order_pagingquery" || totalRow.ResponseName == "my_weekorder_pagingquery")
					{
						if (totalRow.TotalNum > Global.MaxCount)
						{
							isOrderNew = true;
						}
					}
					else
					{
						if (totalRow.ResponseName == "tradepagingquery" && totalRow.TotalNum > Global.MaxCount)
						{
							isTradeNew = true;
						}
					}
				}
			}
			if (this.isPagingQueryCallBack != null)
			{
				this.isPagingQueryCallBack(isOrderNew, isTradeNew);
			}
		}
	}
}
