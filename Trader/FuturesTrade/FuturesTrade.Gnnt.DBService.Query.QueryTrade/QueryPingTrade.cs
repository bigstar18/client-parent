using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
	internal class QueryPingTrade : IQueryTrade
	{
		private List<TradeInfo> tradeListBufferF2 = new List<TradeInfo>();
		private List<TradeInfo> tradeListBufferF4 = new List<TradeInfo>();
		private int tradeorderNum;
		private int tradeorderQty;
		private double tradeorderComm;
		private int tradeorderCurPageNum;
		private int tradeAllNum;
		private double tradeLiqpl;
		private int tradeQty;
		private double tradeComm;
		private int tradeCurPageNum;
		private object tradeLock = new object();
		private void UpdateTradeInfo(List<TradeInfo> tradeInfoList)
		{
			object obj;
			Monitor.Enter(obj = this.tradeLock);
			try
			{
				if (this.tradeListBufferF2.Count == 0)
				{
					this.tradeListBufferF2.AddRange(tradeInfoList);
				}
				else
				{
					if (this.tradeListBufferF4.Count == 0)
					{
						this.tradeListBufferF4.AddRange(tradeInfoList);
					}
					else
					{
						this.ReplaceTradeInfo(tradeInfoList);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		private void ReplaceTradeInfo(List<TradeInfo> tradeInfoList)
		{
			int count = this.tradeListBufferF2.Count;
			int count2 = this.tradeListBufferF4.Count;
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (tradeInfoList[i].OrderNO == this.tradeListBufferF2[j].OrderNO)
					{
						this.tradeListBufferF2[j] = tradeInfoList[i];
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (this.tradeorderCurPageNum <= 1)
					{
						this.tradeListBufferF2.Add(tradeInfoList[i]);
					}
				}
				else
				{
					flag = false;
				}
				for (int k = 0; k < count2; k++)
				{
					if (tradeInfoList[i].OrderNO == this.tradeListBufferF4[k].OrderNO)
					{
						this.tradeListBufferF4[k] = tradeInfoList[i];
						flag = true;
						break;
					}
				}
				if (!flag && this.tradeCurPageNum <= 1)
				{
					this.tradeListBufferF4.Add(tradeInfoList[i]);
				}
			}
		}
		public DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO)
		{
			TradeQueryPagingResponseVO tradeQueryPagingResponseVO = this.QueryPagingTrade(queryTradeOrderInfoReqVO, 0);
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("OrderNo", typeof(long));
			DataColumn column3 = new DataColumn("Time");
			DataColumn column4 = new DataColumn("TransactionsCode");
			DataColumn column5 = new DataColumn("CommodityID");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price", typeof(double));
			DataColumn column9 = new DataColumn("Qty", typeof(int));
			DataColumn column10 = new DataColumn("LPrice", typeof(double));
			DataColumn column11 = new DataColumn("Comm", typeof(double));
			DataColumn column12 = new DataColumn("Market");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column10);
			dataTable.Columns.Add(column11);
			dataTable.Columns.Add(column12);
			dataTable.Columns.Add(new DataColumn("AutoID", typeof(int)));
			dataTable.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			dataSet.Tables.Add(dataTable);
			if (this.tradeListBufferF2.Count == 0 && tradeQueryPagingResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "成交情况查询错误：" + tradeQueryPagingResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < this.tradeListBufferF2.Count; i++)
			{
				TradeInfo tradeInfo = this.tradeListBufferF2[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["OrderNo"] = tradeInfo.OrderNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice;
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				dataRow["LPrice"] = tradeInfo.TransferPrice;
				dataRow["Comm"] = tradeInfo.Comm;
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["Time"] = "合计";
			dataRow2["TransactionsCode"] = "共" + this.tradeorderNum.ToString() + "条";
			dataRow2["Qty"] = this.tradeorderQty;
			dataRow2["Comm"] = this.tradeorderComm;
			dataRow2["AutoID"] = 100000;
			dataTable.Rows.Add(dataRow2);
			return dataSet;
		}
		public DataSet QueryTradeDataSet(object queryTradeInfoReqVO)
		{
			TradeQueryPagingResponseVO tradeQueryPagingResponseVO = this.QueryPagingTrade(queryTradeInfoReqVO, 1);
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("B_S");
			DataColumn column6 = new DataColumn("O_L");
			DataColumn column7 = new DataColumn("Price", typeof(double));
			DataColumn column8 = new DataColumn("Qty", typeof(int));
			DataColumn column9 = new DataColumn("Liqpl", typeof(double));
			DataColumn column10 = new DataColumn("LPrice", typeof(double));
			DataColumn column11 = new DataColumn("Comm", typeof(double));
			DataColumn column12 = new DataColumn("Market");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column10);
			dataTable.Columns.Add(column11);
			dataTable.Columns.Add(column12);
			dataTable.Columns.Add(new DataColumn("AutoID", typeof(int)));
			dataTable.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			dataSet.Tables.Add(dataTable);
			if (this.tradeListBufferF4.Count == 0 && tradeQueryPagingResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "成交情况查询错误：" + tradeQueryPagingResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < this.tradeListBufferF4.Count; i++)
			{
				TradeInfo tradeInfo = this.tradeListBufferF4[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice;
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				dataRow["Liqpl"] = tradeInfo.TransferPL;
				dataRow["LPrice"] = tradeInfo.TransferPrice;
				dataRow["Comm"] = tradeInfo.Comm;
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["Time"] = "合计";
			dataRow2["TransactionsCode"] = "共" + this.tradeAllNum.ToString() + "条";
			dataRow2["Qty"] = this.tradeQty;
			dataRow2["Liqpl"] = this.tradeLiqpl;
			dataRow2["Comm"] = this.tradeComm;
			dataRow2["AutoID"] = 100000;
			dataTable.Rows.Add(dataRow2);
			return dataSet;
		}
		private TradeQueryPagingResponseVO QueryPagingTrade(object queryTradeInfoReqVO, byte tradeFlag)
		{
			TradeQueryPagingRequestVO tradeQueryPagingRequestVO = (TradeQueryPagingRequestVO)queryTradeInfoReqVO;
			TradeQueryPagingResponseVO tradeQueryPagingResponseVO = null;
			if (tradeQueryPagingRequestVO != null)
			{
				if (tradeQueryPagingRequestVO.CurrentPagNum > 0)
				{
					if (tradeFlag == 0)
					{
						this.tradeorderCurPageNum = tradeQueryPagingRequestVO.CurrentPagNum;
						this.tradeListBufferF2.Clear();
					}
					else
					{
						if (tradeFlag == 1)
						{
							this.tradeCurPageNum = tradeQueryPagingRequestVO.CurrentPagNum;
							this.tradeListBufferF4.Clear();
						}
					}
				}
				tradeQueryPagingResponseVO = Global.TradeLibrary.TradeQueryPaging(tradeQueryPagingRequestVO);
				this.UpdateTradeInfo(tradeQueryPagingResponseVO.TradeInfoList);
				if (tradeFlag == 0)
				{
					this.tradeorderNum = tradeQueryPagingResponseVO.TradeTotalRow.TotalNum;
					this.tradeorderQty = tradeQueryPagingResponseVO.TradeTotalRow.TotalQty;
					this.tradeorderComm = tradeQueryPagingResponseVO.TradeTotalRow.TotalComm;
				}
				else
				{
					if (tradeFlag == 1)
					{
						this.tradeAllNum = tradeQueryPagingResponseVO.TradeTotalRow.TotalNum;
						this.tradeLiqpl = tradeQueryPagingResponseVO.TradeTotalRow.TotalLiqpl;
						this.tradeQty = tradeQueryPagingResponseVO.TradeTotalRow.TotalQty;
						this.tradeComm = tradeQueryPagingResponseVO.TradeTotalRow.TotalComm;
					}
				}
			}
			return tradeQueryPagingResponseVO;
		}
	}
}
