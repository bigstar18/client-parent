using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TPME.Log;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class DataProcess
	{
		public Hashtable ht_TradeMode;
		public Hashtable ht_Variety;
		public Hashtable ht_CommodityInfo;
		public string ChangePassword(string textOldPwd, string textNewPwd, string textAfmPwd)
		{
			string text = string.Empty;
			if (!textOldPwd.Equals(Global.Password))
			{
				text += "密码修改信息：旧密码不正确！";
			}
			else if (!textNewPwd.Equals(textAfmPwd))
			{
				text += "密码修改信息：两次新密码输入不一致！";
			}
			else
			{
				ChgPwdRequestVO chgPwdRequestVO = new ChgPwdRequestVO();
				chgPwdRequestVO.UserID = Global.UserID;
				chgPwdRequestVO.OldPassword = textOldPwd;
				chgPwdRequestVO.NewPassword = textAfmPwd;
				ResponseVO responseVO = Global.TradeLibrary.ChangePwd(chgPwdRequestVO);
				if (responseVO.RetCode == 0L)
				{
					text += "密码修改信息：成功！";
					Global.Password = textAfmPwd;
				}
				else
				{
					Logger.wirte(1, "密码修改信息：失败！原因：" + responseVO.RetMessage);
					text = text + "密码修改信息：失败！原因：" + responseVO.RetMessage;
				}
			}
			return text;
		}
		public int GetCommodity()
		{
			this.ht_TradeMode = new Hashtable();
			this.ht_Variety = new Hashtable();
			this.ht_CommodityInfo = new Hashtable();
			string path = Global.ConfigPath + Global.CommCodeXml.Substring(0, Global.CommCodeXml.IndexOf(".")) + ".xsd";
			if (!File.Exists(Global.ConfigPath + Global.CommCodeXml) || !File.Exists(path))
			{
				throw new Exception("没有发现商品架构文件！");
			}
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
			DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
			CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
			commodityQueryRequestVO.UserID = Global.UserID;
			commodityQueryRequestVO.CommodityID = "";
			CommodityQueryResponseVO commodityQueryResponseVO = Global.TradeLibrary.CommodityQuery(commodityQueryRequestVO);
			List<CommodityInfo> commodityInfoList = commodityQueryResponseVO.CommodityInfoList;
			if (commodityQueryResponseVO.RetCode == 0L && commodityInfoList != null && commodityInfoList.Count > 0)
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
					"CommodityName",
					"MarKet"
				};
				string[] array = new string[5];
				for (int i = 0; i < commodityInfoList.Count; i++)
				{
					CommodityInfo commodityInfo = commodityInfoList[i];
					array[0] = "false";
					array[1] = i.ToString();
					array[2] = commodityInfo.CommodityID.Trim();
					array[3] = commodityInfo.CommodityName.Trim();
					array[4] = commodityInfo.MarketID;
					if (commodityInfo.OnMarket == 0)
					{
						xmlDataSet.WriteBoolXml(columns, array);
						if (commodityInfo.TradeMode.ToString().Trim().Length > 0)
						{
							this.ht_TradeMode.Add(commodityInfo.CommodityID.Trim(), commodityInfo.TradeMode.ToString());
							this.ht_Variety.Add(commodityInfo.CommodityID.Trim(), commodityInfo.VarietyID.ToString());
						}
					}
					if (commodityInfo.CommodityID != null && commodityInfo.CommodityID != "")
					{
						this.ht_CommodityInfo.Add(commodityInfo.CommodityID, commodityInfo.CommodityName);
					}
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
					xmlDataSet.UpdateXmlRow(columns2, columnValue, "commodityCode", array2[j]);
				}
				return 0;
			}
			if (commodityQueryResponseVO.RetCode == 0L)
			{
				return 1;
			}
			Logger.wirte(3, "商品查询错误：" + commodityQueryResponseVO.RetMessage);
			xmlDataSet.DeleteXmlAllRows();
			return 2;
		}
		public void GetFirmInfoList()
		{
			FirmInfoResponseVO firmInfo = Global.TradeLibrary.GetFirmInfo(Global.UserID);
			Global.FirmID = firmInfo.FirmID;
		}
		public ResponseVO Order(OrderRequestVO req)
		{
			return Global.TradeLibrary.Order(req);
		}
		public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
		{
			return Global.TradeLibrary.WithDrawOrder(req);
		}
		public DataSet QueryTradeOrderInfo(TradeQueryRequestVO tradeQueryRequestVO)
		{
			TradeQueryResponseVO tradeQueryResponseVO = Global.TradeLibrary.TradeQuery(tradeQueryRequestVO);
			List<TradeInfo> tradeInfoList = tradeQueryResponseVO.TradeInfoList;
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("OrderNo");
			DataColumn column3 = new DataColumn("Time");
			DataColumn column4 = new DataColumn("TransactionsCode");
			DataColumn column5 = new DataColumn("CommodityID");
			DataColumn column6 = new DataColumn("CommodityName");
			DataColumn column7 = new DataColumn("B_S");
			DataColumn column8 = new DataColumn("O_L");
			DataColumn column9 = new DataColumn("Price");
			DataColumn column10 = new DataColumn("Qty", typeof(int));
			DataColumn column11 = new DataColumn("LPrice");
			DataColumn column12 = new DataColumn("Comm");
			DataColumn column13 = new DataColumn("Market");
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
			dataTable.Columns.Add(column13);
			dataSet.Tables.Add(dataTable);
			if (tradeInfoList.Count == 0 && tradeQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "成交委托情况查询错误：" + tradeQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				TradeInfo tradeInfo = tradeInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["OrderNo"] = tradeInfo.OrderNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[tradeInfo.CommodityID];
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice.ToString("n");
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				string value = tradeInfo.TransferPrice.ToString("n");
				dataRow["LPrice"] = value;
				dataRow["Comm"] = tradeInfo.Comm.ToString("n");
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryTradeInfo(TradeQueryRequestVO tradeQueryRequestVO)
		{
			TradeQueryResponseVO tradeQueryResponseVO = Global.TradeLibrary.TradeQuery(tradeQueryRequestVO);
			List<TradeInfo> tradeInfoList = tradeQueryResponseVO.TradeInfoList;
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("CommodityName");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price");
			DataColumn column9 = new DataColumn("Qty", typeof(int));
			DataColumn column10 = new DataColumn("Liqpl");
			DataColumn column11 = new DataColumn("LPrice");
			DataColumn column12 = new DataColumn("Comm");
			DataColumn column13 = new DataColumn("Market");
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
			dataTable.Columns.Add(column13);
			dataSet.Tables.Add(dataTable);
			if (tradeInfoList.Count == 0 && tradeQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "成交情况查询错误：" + tradeQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				TradeInfo tradeInfo = tradeInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[tradeInfo.CommodityID];
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice.ToString("n");
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				dataRow["Liqpl"] = tradeInfo.TransferPL.ToString("n");
				string value = tradeInfo.TransferPrice.ToString("n");
				dataRow["LPrice"] = value;
				dataRow["Comm"] = tradeInfo.Comm.ToString("n");
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QuerySumTradeInfo(TradeSumQueryRequestVO tradeSumQueryRequestVO)
		{
			TradeSumQueryResponseVO tradeSumQueryResponseVO = Global.TradeLibrary.TradeSumQuery(tradeSumQueryRequestVO);
			List<TradeSumInfo> tradeSumInfoList = tradeSumQueryResponseVO.TradeSumInfoList;
			DataSet dataSet = new DataSet("tradeSumDataSet");
			DataTable dataTable = new DataTable("TradeSum");
			DataColumn column = new DataColumn("CommodityID");
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("BuyQty", typeof(long));
			DataColumn column4 = new DataColumn("BuyComm", typeof(double));
			DataColumn column5 = new DataColumn("SellQty", typeof(long));
			DataColumn column6 = new DataColumn("SellComm", typeof(double));
			DataColumn dataColumn = new DataColumn("TotalQty", typeof(long));
			DataColumn dataColumn2 = new DataColumn("TotalComm", typeof(double));
			DataColumn dataColumn3 = new DataColumn("IndeWareHouse", typeof(long));
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(dataColumn);
			dataTable.Columns.Add(dataColumn2);
			dataTable.Columns.Add(dataColumn3);
			dataColumn.Expression = "BuyQty + SellQty";
			dataColumn2.Expression = "BuyComm + SellComm";
			dataColumn3.Expression = "BuyQty - SellQty";
			dataSet.Tables.Add(dataTable);
			if (tradeSumInfoList.Count == 0 && tradeSumQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "成交情况查询错误：" + tradeSumQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < tradeSumInfoList.Count; i++)
			{
				TradeSumInfo tradeSumInfo = tradeSumInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["CommodityID"] = tradeSumInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[tradeSumInfo.CommodityID].ToString();
				dataRow["BuyQty"] = tradeSumInfo.BuyQty;
				dataRow["BuyComm"] = tradeSumInfo.BuyComm;
				dataRow["SellQty"] = tradeSumInfo.SellQty;
				dataRow["SellComm"] = tradeSumInfo.SellComm;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DateTime GetTradeTime()
		{
			DateTime result = default(DateTime);
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = Global.UserID;
			SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
			string str = string.Empty;
			string str2 = string.Empty;
			if (sysTime.RetCode == 0L)
			{
				if (sysTime.CurrentDate.Equals("") || sysTime.CurrentTime.Equals(""))
				{
					return result;
				}
				str = sysTime.CurrentDate;
				str2 = sysTime.CurrentTime;
				try
				{
					result = DateTime.Parse(str + " " + str2);
					return result;
				}
				catch
				{
					result = default(DateTime);
					return result;
				}
			}
			Logger.wirte(3, "获取服务器系统时间错误：" + sysTime.RetMessage);
			return result;
		}
		public DataSet QueryOrderInfo(OrderQueryRequestVO orderQueryRequestVO)
		{
			OrderQueryResponseVO orderQueryResponseVO = Global.TradeLibrary.OrderQuery(orderQueryRequestVO);
			List<OrderInfo> orderInfoList = orderQueryResponseVO.OrderInfoList;
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("CommodityName");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price");
			DataColumn column9 = new DataColumn("Qty");
			DataColumn column10 = new DataColumn("Balance");
			DataColumn column11 = new DataColumn("Status");
			DataColumn column12 = new DataColumn("Market");
			DataColumn column13 = new DataColumn("CBasis");
			DataColumn column14 = new DataColumn("BillTradeType");
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
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column14);
			dataSet.Tables.Add(dataTable);
			if (orderQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "委托查询错误：" + orderQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < orderInfoList.Count; i++)
			{
				OrderInfo orderInfo = orderInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["OrderNo"] = orderInfo.OrderNO;
				dataRow["Time"] = Global.toTime(orderInfo.Time);
				dataRow["TransactionsCode"] = orderInfo.CustomerID;
				dataRow["CommodityID"] = orderInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[orderInfo.CommodityID];
				dataRow["B_S"] = Global.BuySellStrArr[(int)orderInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)orderInfo.SettleBasis];
				dataRow["Price"] = orderInfo.OrderPrice.ToString("n");
				dataRow["Qty"] = orderInfo.OrderQuantity;
				dataRow["Balance"] = orderInfo.Balance;
				dataRow["Status"] = Global.OrderStatusStrArr[(int)orderInfo.State];
				dataRow["Market"] = orderInfo.MarketID;
				dataRow["CBasis"] = Global.CBasisStrArr[(int)orderInfo.CBasis];
				dataRow["BillTradeType"] = Global.BillTradeTypeStrArr[(int)orderInfo.BillTradeType];
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryTodayOrderInfo(OrderQueryRequestVO orderQueryRequestVO)
		{
			OrderQueryResponseVO orderQueryResponseVO = Global.TradeLibrary.AllOrderQuery(orderQueryRequestVO);
			List<OrderInfo> orderInfoList = orderQueryResponseVO.OrderInfoList;
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("CommodityName");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price");
			DataColumn column9 = new DataColumn("Qty");
			DataColumn column10 = new DataColumn("Balance");
			DataColumn column11 = new DataColumn("Status");
			DataColumn column12 = new DataColumn("Market");
			DataColumn column13 = new DataColumn("CBasis");
			DataColumn column14 = new DataColumn("BillTradeType");
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
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column14);
			dataSet.Tables.Add(dataTable);
			if (orderQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "当日委托指令浏览查询错误：" + orderQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < orderInfoList.Count; i++)
			{
				OrderInfo orderInfo = orderInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["OrderNo"] = orderInfo.OrderNO;
				dataRow["Time"] = Global.toTime(orderInfo.Time);
				dataRow["TransactionsCode"] = orderInfo.CustomerID;
				dataRow["CommodityID"] = orderInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[orderInfo.CommodityID];
				dataRow["B_S"] = Global.BuySellStrArr[(int)orderInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)orderInfo.SettleBasis];
				dataRow["Price"] = orderInfo.OrderPrice.ToString("n");
				dataRow["Qty"] = orderInfo.OrderQuantity;
				dataRow["Balance"] = orderInfo.Balance;
				dataRow["Status"] = Global.OrderStatusStrArr[(int)orderInfo.State];
				dataRow["Market"] = orderInfo.MarketID;
				dataRow["CBasis"] = Global.CBasisStrArr[(int)orderInfo.CBasis];
				dataRow["BillTradeType"] = Global.BillTradeTypeStrArr[(int)orderInfo.BillTradeType];
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryHoldingInfo(HoldingQueryRequestVO holdingQueryRequestVO)
		{
			HoldingQueryResponseVO holdingQueryResponseVO = Global.TradeLibrary.HoldingQuery(holdingQueryRequestVO);
			List<HoldingInfo> holdingInfoList = holdingQueryResponseVO.HoldingInfoList;
			DataSet dataSet = new DataSet("holdingDataSet");
			DataTable dataTable = new DataTable("Holding");
			DataColumn column = new DataColumn("CommodityID");
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("BuyHolding", typeof(int));
			DataColumn column5 = new DataColumn("BuyAvg");
			DataColumn column6 = new DataColumn("SellHolding", typeof(int));
			DataColumn column7 = new DataColumn("SellAvg", typeof(double));
			DataColumn column8 = new DataColumn("Margin");
			DataColumn column9 = new DataColumn("Floatpl", typeof(double));
			DataColumn column10 = new DataColumn("BuyVHolding", typeof(int));
			DataColumn column11 = new DataColumn("SellVHolding", typeof(int));
			DataColumn column12 = new DataColumn("GoodsQty", typeof(int));
			DataColumn column13 = new DataColumn("NewPriceLP");
			DataColumn column14 = new DataColumn("LPRatio");
			DataColumn column15 = new DataColumn("MarketValue");
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
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column14);
			dataTable.Columns.Add(column15);
			dataSet.Tables.Add(dataTable);
			if (holdingQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "持仓查询错误：" + holdingQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < holdingInfoList.Count; i++)
			{
				HoldingInfo holdingInfo = holdingInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["CommodityID"] = holdingInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[holdingInfo.CommodityID];
				dataRow["TransactionsCode"] = holdingInfo.CustomerID;
				dataRow["BuyHolding"] = holdingInfo.BuyHolding;
				dataRow["BuyAvg"] = holdingInfo.BuyAverage.ToString("n");
				dataRow["SellHolding"] = holdingInfo.SellHolding;
				dataRow["SellAvg"] = holdingInfo.SellAverage;
				dataRow["Margin"] = holdingInfo.Bail.ToString("n");
				dataRow["Floatpl"] = holdingInfo.FloatingLP;
				dataRow["BuyVHolding"] = holdingInfo.BuyVHolding;
				dataRow["SellVHolding"] = holdingInfo.SellVHolding;
				dataRow["GoodsQty"] = holdingInfo.GOQuantity;
				dataRow["NewPriceLP"] = holdingInfo.NewPriceLP.ToString("n");
				dataRow["LPRatio"] = Global.ToPercent(holdingInfo.LPRadio);
				dataRow["MarketValue"] = holdingInfo.MarketValue.ToString("n");
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryHoldingDetailInfo(HoldingDetailRequestVO HoldingDetailRequestVO)
		{
			HoldingDetailResponseVO holdingDetailResponseVO = Global.TradeLibrary.HoldPtByPrice(HoldingDetailRequestVO);
			List<HoldingDetailInfo> holdingDetailInfoList = holdingDetailResponseVO.HoldingDetailInfoList;
			DataSet dataSet = new DataSet("holdingDetailDataSet");
			DataTable dataTable = new DataTable("HoldingDetail");
			DataColumn column = new DataColumn("CommodityID");
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("B_S");
			DataColumn column5 = new DataColumn("Cur_Open", typeof(int));
			DataColumn column6 = new DataColumn("Price");
			DataColumn column7 = new DataColumn("Margin");
			DataColumn column8 = new DataColumn("GoodsQty", typeof(int));
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataSet.Tables.Add(dataTable);
			if (holdingDetailResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "订货明细查询错误：" + holdingDetailResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < holdingDetailInfoList.Count; i++)
			{
				HoldingDetailInfo holdingDetailInfo = holdingDetailInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["CommodityID"] = holdingDetailInfo.CommodityID;
				dataRow["CommodityName"] = this.ht_CommodityInfo[holdingDetailInfo.CommodityID];
				dataRow["TransactionsCode"] = holdingDetailInfo.CustomerID;
				dataRow["Price"] = holdingDetailInfo.Price.ToString("n");
				dataRow["B_S"] = Global.BuySellStrArr[(int)holdingDetailInfo.BuySell];
				dataRow["Margin"] = holdingDetailInfo.Bail.ToString("n");
				dataRow["Cur_Open"] = holdingDetailInfo.AmountOnOrder;
				dataRow["GoodsQty"] = holdingDetailInfo.GOQuantity;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public FirmInfoResponseVO QueryFundsInfo()
		{
			FirmInfoResponseVO firmInfo = Global.TradeLibrary.GetFirmInfo(Global.UserID);
			if (firmInfo.RetCode != 0L)
			{
				Logger.wirte(3, "会员信息查询错误：" + firmInfo.RetMessage);
			}
			return firmInfo;
		}
		public CommodityInfo QueryCommodityInfo(string MarKet, string CommodityID)
		{
			int num = CommodityID.IndexOf("_");
			if (num != -1)
			{
				MarKet = CommodityID.Substring(0, num);
				CommodityID = CommodityID.Substring(num + 1);
			}
			CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
			commodityQueryRequestVO.UserID = Global.UserID;
			commodityQueryRequestVO.MarketID = MarKet;
			commodityQueryRequestVO.CommodityID = CommodityID;
			CommodityQueryResponseVO commodityQueryResponseVO = Global.TradeLibrary.CommodityQuery(commodityQueryRequestVO);
			if (commodityQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "查询商品信息错误：" + commodityQueryResponseVO.RetMessage);
				return null;
			}
			List<CommodityInfo> commodityInfoList = commodityQueryResponseVO.CommodityInfoList;
			if (commodityInfoList.Count > 0)
			{
				return commodityInfoList[0];
			}
			return null;
		}
		public CommData QueryGNCommodityInfo(string MarketID, string CommodityID)
		{
			CommDataQueryRequestVO commDataQueryRequestVO = new CommDataQueryRequestVO();
			commDataQueryRequestVO.UserID = Global.UserID;
			commDataQueryRequestVO.MarketID = MarketID;
			commDataQueryRequestVO.CommodityID = CommodityID;
			CommDataQueryResponseVO commDataQueryResponseVO = Global.TradeLibrary.CommDataQuery(commDataQueryRequestVO);
			if (commDataQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "查询商品行情错误：" + commDataQueryResponseVO.RetMessage);
				return null;
			}
			List<CommData> commDataList = commDataQueryResponseVO.CommDataList;
			if (commDataList.Count > 0)
			{
				return commDataList[0];
			}
			return null;
		}
		public Hashtable QueryMarketInfo(MarketQueryRequestVO req)
		{
			Hashtable hashtable = new Hashtable();
			MarketQueryResponseVO marketQueryResponseVO = Global.TradeLibrary.MarketQuery(req);
			if (marketQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "市场详细信息查询错误：" + marketQueryResponseVO.RetMessage);
				return hashtable;
			}
			List<MarkeInfo> markeInfoList = marketQueryResponseVO.MarkeInfoList;
			for (int i = 0; i < markeInfoList.Count; i++)
			{
				hashtable.Add(markeInfoList[i].MarketID, markeInfoList[i]);
			}
			return hashtable;
		}
		public FirmbreedQueryResponseVO QueryFirmbreed()
		{
			FirmbreedQueryResponseVO firmbreedQueryResponseVO = Global.TradeLibrary.FirmbreedQuery(Global.UserID);
			if (firmbreedQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(3, "主持交易商品种信息查询错误：" + firmbreedQueryResponseVO.RetMessage);
			}
			return firmbreedQueryResponseVO;
		}
		public InvestorInfoResponseVO GetInvestorInfo()
		{
			InvestorInfoResponseVO investorInfo = Global.TradeLibrary.GetInvestorInfo(Global.UserID);
			if (investorInfo.RetCode != 0L)
			{
				Logger.wirte(3, "投资人信息查询错误：" + investorInfo.RetMessage);
			}
			return investorInfo;
		}
		public int CalculatLargestTradeNum(CommodityInfo commodityInfo, double price, short B_SType, short O_LType, int prevClear, string MarketID, string CustomerID)
		{
			double num = 0.0;
			if (O_LType == 1)
			{
				double num2 = 0.0;
				short num3 = 1;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				double num7 = 0.0;
				short num8 = 1;
				double num9 = 0.0;
				double num10 = 0.0;
				FirmInfoResponseVO firmInfoResponseVO = this.QueryFundsInfo();
				num2 = firmInfoResponseVO.UsableFund;
				num3 = commodityInfo.MarginType;
				num4 = commodityInfo.BMargin;
				num5 = commodityInfo.SMargin;
				num6 = commodityInfo.BMargin_g;
				num7 = commodityInfo.SMargin_g;
				num8 = commodityInfo.CommType;
				num10 = commodityInfo.CtrtSize;
				if (num2 <= 0.0)
				{
					return 0;
				}
				try
				{
					num9 = commodityInfo.BOpenComm;
				}
				catch
				{
					num9 = 0.0;
				}
				double num11 = 0.0;
				double num12 = 0.0;
				if (B_SType == 1 && price > 0.0 && num10 > 0.0 && (num4 > 0.0 || num4 == -1.0))
				{
					if (num3 == 2)
					{
						if (num4 == -1.0)
						{
							num11 = price * num10;
						}
						else
						{
							num11 = num4 - num6;
						}
					}
					else if (num4 == -1.0)
					{
						num11 = price * num10;
					}
					else
					{
						num11 = price * num10 * (num4 - num6) / 100.0;
					}
					if (num8 == 2)
					{
						num12 = num9;
					}
					else
					{
						num12 = price * num10 * num9 / 100.0;
					}
				}
				else if (B_SType == 2 && price > 0.0 && num10 > 0.0 && (num5 > 0.0 || num5 == -1.0))
				{
					if (num3 == 2)
					{
						if (num5 == -1.0)
						{
							num11 = price * num10;
						}
						else
						{
							num11 = num5 - num7;
						}
					}
					else if (num5 == -1.0)
					{
						num11 = price * num10;
					}
					else
					{
						num11 = price * num10 * (num5 - num7) / 100.0;
					}
					if (num8 == 2)
					{
						num12 = num9;
					}
					else
					{
						num12 = price * num10 * num9 / 100.0;
					}
				}
				if (num11 + num12 > 0.0)
				{
					num = num2 / (num11 + num12);
				}
			}
			else
			{
				HoldingQueryRequestVO holdingQueryRequestVO = new HoldingQueryRequestVO();
				holdingQueryRequestVO.UserID = Global.UserID;
				holdingQueryRequestVO.CommodityID = commodityInfo.CommodityID;
				HoldingQueryResponseVO holdingQueryResponseVO = Global.TradeLibrary.HoldingQuery(holdingQueryRequestVO);
				if (holdingQueryResponseVO.RetCode == 0L)
				{
					List<HoldingInfo> holdingInfoList = holdingQueryResponseVO.HoldingInfoList;
					int i = 0;
					while (i < holdingInfoList.Count)
					{
						HoldingInfo holdingInfo = holdingInfoList[i];
						if (holdingInfo.CustomerID.Equals(CustomerID))
						{
							if (B_SType == 1)
							{
								num = (double)holdingInfo.SellVHolding;
								break;
							}
							num = (double)holdingInfo.BuyVHolding;
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
			return (int)num;
		}
	}
}
