using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
using TradeInterface.Gnnt.OTC.Interface;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class DataProcess
	{
		private Identity _sIdentity;
		private ITradeLibrary _TradeLibrary;
		public Hashtable ht_TradeMode;
		public Hashtable ht_Variety;
		public bool IsAgency;
		public Identity sIdentity
		{
			get
			{
				return this._sIdentity;
			}
			set
			{
				this._sIdentity = value;
			}
		}
		public ITradeLibrary TradeLibrary
		{
			get
			{
				return this._TradeLibrary;
			}
			set
			{
				this._TradeLibrary = value;
			}
		}
		public string ChangePassword(string textOldPwd, string textNewPwd, string textAfmPwd)
		{
			string text = string.Empty;
			try
			{
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
					ResponseVO responseVO = this._TradeLibrary.ChangePwd(chgPwdRequestVO);
					if (responseVO.RetCode == 0L)
					{
						text += "密码修改信息：成功！";
						Global.Password = textAfmPwd;
					}
					else
					{
						Logger.wirte(2, "密码修改信息：失败！原因：" + responseVO.RetMessage);
						text = text + "密码修改信息：失败！原因：" + responseVO.RetMessage;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return text;
		}
		public int GetCommodity()
		{
			int result;
			try
			{
				this.ht_TradeMode = new Hashtable();
				this.ht_Variety = new Hashtable();
				string path = Global.CommCodeXml.Substring(0, Global.CommCodeXml.IndexOf(".")) + ".xsd";
				if (!File.Exists(Global.CommCodeXml) || !File.Exists(path))
				{
					throw new Exception("没有发现商品架构文件！");
				}
				XmlDataSet xmlDataSet = new XmlDataSet(Global.CommCodeXml);
				DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
				CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
				commodityQueryRequestVO.UserID = Global.UserID;
				commodityQueryRequestVO.CommodityID = "";
				if (this.IsAgency)
				{
					commodityQueryRequestVO.AgencyNo = Global.AgencyNo;
					commodityQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					commodityQueryRequestVO.AgencyNo = string.Empty;
					commodityQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				CommodityQueryResponseVO commodityQueryResponseVO = this._TradeLibrary.CommodityQuery(commodityQueryRequestVO);
				Dictionary<string, CommodityInfo> commodityInfoList = commodityQueryResponseVO.CommodityInfoList;
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
						"MarKet"
					};
					string[] array = new string[4];
					int num = 0;
					IEnumerator enumerator2 = commodityInfoList.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current != null)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator2.Current;
							CommodityInfo commodityInfo = (CommodityInfo)dictionaryEntry.Value;
							array[0] = "false";
							array[1] = num.ToString();
							array[2] = commodityInfo.CommodityID.Trim();
							array[3] = commodityInfo.MarketID;
							xmlDataSet.WriteBoolXml(columns, array);
							if (commodityInfo.TradeMode.Equals(0) || commodityInfo.TradeMode.Equals(1) || commodityInfo.TradeMode.Equals(2))
							{
								this.ht_TradeMode.Add(commodityInfo.CommodityID.Trim(), commodityInfo.TradeMode.ToString());
								this.ht_Variety.Add(commodityInfo.CommodityID.Trim(), commodityInfo.VarietyID.ToString());
							}
							num++;
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
					for (int i = 0; i < array2.Length - 1; i++)
					{
						xmlDataSet.UpdateXmlRow(columns2, columnValue, "commodityCode", array2[i]);
					}
					result = 0;
				}
				else if (commodityQueryResponseVO.RetCode != 0L)
				{
					result = 1;
				}
				else
				{
					Logger.wirte(3, "商品查询错误：" + commodityQueryResponseVO.RetMessage);
					xmlDataSet.DeleteXmlAllRows();
					result = 2;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = 1;
			}
			return result;
		}
		public DataSet GetCommodityHQInfo()
		{
			Dictionary<string, CommodityInfo> dictionary;
			if (this.IsAgency)
			{
				dictionary = Global.AgencyCommodityData;
			}
			else
			{
				dictionary = Global.CommodityData;
			}
			Dictionary<string, CommData> dictionary2 = null;
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("HQ");
			DataColumn column = new DataColumn("ColImage", typeof(Image));
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("CommodityID");
			DataColumn column4 = new DataColumn("SellPrice");
			DataColumn column5 = new DataColumn("BuyPrice");
			DataColumn column6 = new DataColumn("HightPrice");
			DataColumn column7 = new DataColumn("LowPrice");
			DataColumn column8 = new DataColumn("PriceTime");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column3);
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData == null || Global.AgencyHQCommData.Count == 0)
						{
							foreach (KeyValuePair<string, CommodityInfo> current in dictionary)
							{
								DataRow dataRow = dataTable.NewRow();
								dataRow["CommodityName"] = current.Value.CommodityName;
								dataRow["CommodityID"] = current.Value.CommodityID;
								dataRow["SellPrice"] = " --";
								dataRow["BuyPrice"] = "--";
								dataRow["HightPrice"] = "--";
								dataRow["LowPrice"] = "--";
								dataRow["PriceTime"] = "--";
								dataTable.Rows.Add(dataRow);
							}
							DataSet result = dataSet;
							return result;
						}
						lock (Global.AgencyHQCommDataLock)
						{
							dictionary2 = Global.gAgencyHQCommData;
						}
						goto IL_356;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData == null || Global.HQCommData.Count == 0)
					{
						foreach (KeyValuePair<string, CommodityInfo> current2 in dictionary)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["CommodityName"] = current2.Value.CommodityName;
							dataRow["CommodityID"] = current2.Value.CommodityID;
							dataRow["SellPrice"] = " --";
							dataRow["BuyPrice"] = "--";
							dataRow["HightPrice"] = "--";
							dataRow["LowPrice"] = "--";
							dataRow["PriceTime"] = "--";
							dataTable.Rows.Add(dataRow);
						}
						DataSet result = dataSet;
						return result;
					}
					lock (Global.HQCommDataLock)
					{
						dictionary2 = Global.gHQCommData;
					}
				}
				IL_356:
				if (dictionary2 == null)
				{
					Logger.wirte(3, "数据为空：");
					DataSet result = dataSet;
					return result;
				}
				if (dictionary == null)
				{
					Logger.wirte(3, "商品信息数据为空：");
					DataSet result = dataSet;
					return result;
				}
				if (dictionary.Count == 0)
				{
					Logger.wirte(2, "数据为空：");
					DataSet result = dataSet;
					return result;
				}
				foreach (KeyValuePair<string, CommodityInfo> current3 in dictionary)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["CommodityName"] = current3.Value.CommodityName;
					dataRow["CommodityID"] = current3.Value.CommodityID;
					if (dictionary2.ContainsKey(current3.Value.CommodityID))
					{
						dataRow["SellPrice"] = dictionary2[current3.Value.CommodityID].SellPrice.ToString();
						dataRow["BuyPrice"] = dictionary2[current3.Value.CommodityID].BuyPrice.ToString();
						dataRow["HightPrice"] = dictionary2[current3.Value.CommodityID].High.ToString();
						dataRow["LowPrice"] = dictionary2[current3.Value.CommodityID].Low.ToString();
						dataRow["PriceTime"] = dictionary2[current3.Value.CommodityID].UpdateTime.ToString();
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet GetAccountInfo()
		{
			FirmFundsInfoResponseVO firmFundsInfoResponseVO = null;
			DataSet dataSet = new DataSet("FirmInfoDataSet");
			DataTable dataTable = new DataTable("tFirmInfo");
			DataColumn column = new DataColumn("ProjectL");
			DataColumn column2 = new DataColumn("ProjectValL");
			DataColumn column3 = new DataColumn("ProjectR");
			DataColumn column4 = new DataColumn("ProjectValR");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataSet.Tables.Add(dataTable);
			try
			{
				FirmInfoRequestVO firmInfoRequestVO = new FirmInfoRequestVO();
				if (this.IsAgency)
				{
					firmInfoRequestVO.AgencyNo = Global.AgencyNo;
					firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					firmInfoRequestVO.AgencyNo = string.Empty;
					firmInfoRequestVO.AgencyPhonePassword = string.Empty;
				}
				firmInfoRequestVO.UserID = Global.UserID;
				FirmInfoResponseVO firmInfo = this._TradeLibrary.GetFirmInfo(firmInfoRequestVO);
				if (this._sIdentity == Identity.Member)
				{
					firmFundsInfoResponseVO = this._TradeLibrary.GetFirmFundsInfo(Global.UserID);
				}
				DataSet result;
				if (firmInfo.RetCode == 0L)
				{
					if (this.IsAgency)
					{
						lock (Global.AgencyFirmInfoDataLock)
						{
							Global.AgencyFirmInfoData = (FirmInfoResponseVO)firmInfo.Clone();
							goto IL_167;
						}
					}
					lock (Global.FirmInfoDataLock)
					{
						Global.FirmInfoData = (FirmInfoResponseVO)firmInfo.Clone();
						goto IL_167;
					}
					goto IL_15F;
					IL_167:
					if (this._sIdentity == Identity.Client)
					{
						if (firmInfo == null || firmInfo.RetCode != 0L)
						{
							Logger.wirte(2, "资金数据为空");
							result = dataSet;
							return result;
						}
					}
					else if (this._sIdentity == Identity.Member && (firmInfo == null || firmInfo.RetCode != 0L || firmFundsInfoResponseVO == null || firmFundsInfoResponseVO.RetCode != 0L))
					{
						Logger.wirte(2, "资金数据为空");
						result = dataSet;
						return result;
					}
					if (this._sIdentity == Identity.Client)
					{
						double balance = BizController.CalculateBalance(firmInfo.InitFund, firmInfo.InOutFund, firmInfo.Fee, firmInfo.YesterdayBail, firmInfo.TransferPL);
						DataRow dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FIRMNAME");
						dataRow["ProjectValL"] = firmInfo.FirmName;
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FIRMUSERID");
						if (this.IsAgency)
						{
							dataRow["ProjectValR"] = Global.AgencyNo;
						}
						else
						{
							dataRow["ProjectValR"] = Global.UserID;
						}
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						double num = BizController.CalculateInitRight(firmInfo.InitFund, firmInfo.YesterdayBail);
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_INITRIGHT");
						dataRow["ProjectValL"] = num.ToString("n2");
						double initFund = BizController.CalculateInitFund(balance, firmInfo.CurrentFL);
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_WORTH");
						dataRow["ProjectValR"] = initFund.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_INOUTFUND");
						dataRow["ProjectValL"] = firmInfo.InOutFund.ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_CURRENTFL");
						dataRow["ProjectValR"] = firmInfo.CurrentFL.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_TRANSFERPL");
						dataRow["ProjectValL"] = firmInfo.TransferPL.ToString("n2");
						double num2 = BizController.CalculateRealFund(initFund, firmInfo.CurrentBail, firmInfo.OrderFrozenFund, firmInfo.UsingFund);
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_REALFUND");
						dataRow["ProjectValR"] = num2.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FEE");
						dataRow["ProjectValL"] = (-firmInfo.Fee).ToString("n2");
						double currentBail = BizController.CalculateHoldingFund(firmInfo.CurrentBail, firmInfo.OrderFrozenFund, firmInfo.OtherFrozenFund);
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_HOLDINGBAIL");
						dataRow["ProjectValR"] = currentBail.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_CLEARDELAY");
						dataRow["ProjectValL"] = firmInfo.ClearDelay.ToString("n2");
						double orderFrozenMargin = firmInfo.OrderFrozenMargin;
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_ORDERFROZENFUND");
						dataRow["ProjectValR"] = orderFrozenMargin.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FUNDRISK");
						double num3 = BizController.CalculateFundRisk(initFund, currentBail);
						dataRow["ProjectValL"] = ((num3 < 0.0) ? "0.00%" : num3.ToString("p2"));
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_ORDERFROZEN");
						dataRow["ProjectValR"] = firmInfo.OrderFrozenFee.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_CSTATUS");
						string cStatus;
						if ((cStatus = firmInfo.CStatus) != null)
						{
							if (!(cStatus == "U"))
							{
								if (!(cStatus == "N"))
								{
									if (!(cStatus == "F"))
									{
										if (!(cStatus == "D"))
										{
											if (cStatus == "C")
											{
												dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 5);
											}
										}
										else
										{
											dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 4);
										}
									}
									else
									{
										dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 3);
									}
								}
								else
								{
									dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 2);
								}
							}
							else
							{
								dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 1);
							}
						}
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_USINGFUND");
						dataRow["ProjectValR"] = firmInfo.UsingFund.ToString("n2");
						dataTable.Rows.Add(dataRow);
					}
					else if (this._sIdentity == Identity.Member)
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FIRMNAME");
						dataRow["ProjectValL"] = firmInfo.FirmName;
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FIRMUSERID");
						dataRow["ProjectValR"] = Global.UserID;
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						double num4 = BizController.CalculateInitRight(firmInfo.InitFund, firmInfo.YesterdayBail);
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_INITRIGHT");
						dataRow["ProjectValL"] = num4.ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_REALFUND");
						dataRow["ProjectValR"] = firmFundsInfoResponseVO.RiskMargin.ToString("n2");
						double arg_85B_0 = firmFundsInfoResponseVO.RiskMargin;
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_INOUTFUND");
						dataRow["ProjectValL"] = firmInfo.InOutFund.ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_M_HOLD_NET_FLOAT");
						dataRow["ProjectValR"] = firmFundsInfoResponseVO.MemberPureFloating.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_TRANSFERPL_M");
						dataRow["ProjectValL"] = firmInfo.TransferPL.ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_C_TRADE_FLOAT");
						dataRow["ProjectValR"] = firmFundsInfoResponseVO.CustomerTradeFloating.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FEE_M");
						dataRow["ProjectValL"] = (-firmInfo.Fee).ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_CURRENTFL_M");
						dataRow["ProjectValR"] = firmFundsInfoResponseVO.DuiChongFloating.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_TRANSFERPL_C");
						dataRow["ProjectValL"] = firmFundsInfoResponseVO.CustomerCloseProfit.ToString("n2");
						double num5 = BizController.CalculateHoldingFund(firmInfo.CurrentBail, firmInfo.OrderFrozenFund, firmInfo.OtherFrozenFund);
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_HOLDINGBAIL");
						dataRow["ProjectValR"] = num5.ToString("n2");
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_USINGFUND");
						dataRow["ProjectValL"] = firmInfo.UsingFund.ToString("n2");
						dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_FUNDRISK");
						dataRow["ProjectValR"] = ((firmInfo.FundRisk < 0.0) ? "0.00%" : firmInfo.FundRisk.ToString("p2"));
						dataTable.Rows.Add(dataRow);
						dataRow = dataTable.NewRow();
						dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_AI_CSTATUS");
						string cStatus2;
						if ((cStatus2 = firmInfo.CStatus) != null)
						{
							if (!(cStatus2 == "U"))
							{
								if (!(cStatus2 == "N"))
								{
									if (!(cStatus2 == "F"))
									{
										if (!(cStatus2 == "D"))
										{
											if (cStatus2 == "C")
											{
												dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 5);
											}
										}
										else
										{
											dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 4);
										}
									}
									else
									{
										dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 3);
									}
								}
								else
								{
									dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 2);
								}
							}
							else
							{
								dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FIRMFUNDSSTATUS", 1);
							}
						}
						dataRow["ProjectR"] = string.Empty;
						dataRow["ProjectValR"] = string.Empty;
						dataTable.Rows.Add(dataRow);
					}
					return dataSet;
				}
				IL_15F:
				result = dataSet;
				return result;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet GetFirmHoldSumQuery(string userID)
		{
			Hashtable hashtable = new Hashtable();
			DataSet dataSet = null;
			try
			{
				dataSet = new DataSet("FirmHoldSumQueryDataSet");
				DataTable dataTable = new DataTable("tFirmHoldSumQuery");
				DataColumn column = new DataColumn("CommodityID");
				DataColumn column2 = new DataColumn("CommodityName");
				DataColumn column3 = new DataColumn("MaxHolding");
				DataColumn column4 = new DataColumn("MemberJingTouCun");
				DataColumn column5 = new DataColumn("CustomerJingTouCun");
				DataColumn column6 = new DataColumn("DuiChongJingTouCun");
				DataColumn column7 = new DataColumn("HoldingNetFloating");
				DataColumn column8 = new DataColumn("CustomerTradeFloating");
				DataColumn column9 = new DataColumn("DuiChongFloating");
				dataTable.Columns.Add(column);
				dataTable.Columns.Add(column2);
				dataTable.Columns.Add(column3);
				dataTable.Columns.Add(column4);
				dataTable.Columns.Add(column5);
				dataTable.Columns.Add(column6);
				dataTable.Columns.Add(column7);
				dataTable.Columns.Add(column8);
				dataTable.Columns.Add(column9);
				dataSet.Tables.Add(dataTable);
				if (userID.Length == 0)
				{
					DataSet result = dataSet;
					return result;
				}
				FirmHoldSumResponseVO firmHoldSumQuery = this._TradeLibrary.GetFirmHoldSumQuery(userID);
				if (firmHoldSumQuery.FirmHoldSumQueryList != null && firmHoldSumQuery.RetCode == 0L)
				{
					foreach (FirmHoldSumQuery current in firmHoldSumQuery.FirmHoldSumQueryList)
					{
						if (!hashtable.ContainsKey(current.CommodityID))
						{
							hashtable.Add(current.CommodityID, current);
						}
					}
				}
				if (Global.CommodityData == null)
				{
					Logger.wirte(2, "Global.CommodityData数据为空");
					DataSet result = dataSet;
					return result;
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["CommodityName"] = current2.Value.CommodityName;
					dataRow["CommodityID"] = current2.Value.CommodityID;
					if (hashtable.ContainsKey(current2.Value.CommodityID))
					{
						FirmHoldSumQuery firmHoldSumQuery2 = (FirmHoldSumQuery)hashtable[current2.Value.CommodityID];
						dataRow["MaxHolding"] = firmHoldSumQuery2.MaxHolding;
						dataRow["MemberJingTouCun"] = firmHoldSumQuery2.MemberJingTouCun;
						dataRow["CustomerJingTouCun"] = firmHoldSumQuery2.CustomerJingTouCun;
						dataRow["DuiChongJingTouCun"] = firmHoldSumQuery2.DuiChongJingTouCun;
						dataRow["HoldingNetFloating"] = firmHoldSumQuery2.HoldingNetFloating.ToString("n2");
						dataRow["CustomerTradeFloating"] = firmHoldSumQuery2.CustomerTradeFloating.ToString("n2");
						dataRow["DuiChongFloating"] = firmHoldSumQuery2.DuiChongFloating.ToString("n2");
					}
					else
					{
						dataRow["MaxHolding"] = "--";
						dataRow["MemberJingTouCun"] = "--";
						dataRow["CustomerJingTouCun"] = "--";
						dataRow["DuiChongJingTouCun"] = "--";
						dataRow["HoldingNetFloating"] = "--";
						dataRow["CustomerTradeFloating"] = "--";
						dataRow["DuiChongFloating"] = "--";
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return dataSet;
		}
		public DataSet GetCommodityInfo(string CommodityID)
		{
			DataSet dataSet = new DataSet("CommodityInfoDataSet");
			DataTable dataTable = new DataTable("tCommodityInfo");
			DataColumn column = new DataColumn("ProjectL");
			DataColumn column2 = new DataColumn("ProjectValL");
			DataColumn column3 = new DataColumn("ProjectR");
			DataColumn column4 = new DataColumn("ProjectValR");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataSet.Tables.Add(dataTable);
			try
			{
				CommodityInfo commodityInfo;
				if (this.IsAgency)
				{
					commodityInfo = Global.AgencyCommodityData[CommodityID];
				}
				else
				{
					commodityInfo = Global.CommodityData[CommodityID];
				}
				if (commodityInfo == null)
				{
					Logger.wirte(2, "数据为空：");
					DataSet result = dataSet;
					return result;
				}
				DataRow dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_COMMODITYID");
				dataRow["ProjectValL"] = commodityInfo.CommodityID;
				dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_COMMODITYNAME");
				dataRow["ProjectValR"] = commodityInfo.CommodityName;
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_STATUS");
				dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("COMMODITYSTATUS", (int)commodityInfo.Status);
				dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_CTRTSIZE");
				dataRow["ProjectValR"] = commodityInfo.CtrtSize.ToString("f2") + commodityInfo.CommodityUnit;
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_P_MIN_H");
				dataRow["ProjectValL"] = commodityInfo.P_MIN_H.ToString();
				dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_P_MAX_H");
				dataRow["ProjectValR"] = commodityInfo.P_MAX_H.ToString();
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_MAXHOLDING");
				dataRow["ProjectValL"] = ((commodityInfo.MaxHolding == -1L) ? "无限制" : commodityInfo.MaxHolding.ToString());
				dataRow["ProjectR"] = string.Empty;
				dataRow["ProjectValR"] = string.Empty;
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_FEEALGR");
				dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FEEALGR", Convert.ToInt32(commodityInfo.CommType));
				dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_FEEVALUE");
				dataRow["ProjectValR"] = ((!Global.DoubleIsZero(commodityInfo.FeeValue)) ? commodityInfo.FeeValue.ToString("f15").TrimEnd(new char[]
				{
					'0'
				}).TrimEnd(new char[]
				{
					'.'
				}) : "0.00");
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_FEEMODE");
				dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FEEMODE", Convert.ToInt32(commodityInfo.FeeType));
				dataRow["ProjectR"] = string.Empty;
				dataRow["ProjectValR"] = string.Empty;
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_MARGINTYPE");
				dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FEEALGR", Convert.ToInt32(commodityInfo.MarginType));
				dataRow["ProjectR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_MARGINVALUE");
				dataRow["ProjectValR"] = ((!Global.DoubleIsZero(commodityInfo.MarginValue)) ? commodityInfo.MarginValue.ToString("f15").TrimEnd(new char[]
				{
					'0'
				}).TrimEnd(new char[]
				{
					'.'
				}) : "0.00");
				dataTable.Rows.Add(dataRow);
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_YANQITYPE");
				dataRow["ProjectValL"] = Global.GetEnumtoResourcesString("FEEALGR", Convert.ToInt32(commodityInfo.DeferType));
				dataRow["ProjectR"] = "";
				dataRow["ProjectValR"] = "";
				dataTable.Rows.Add(dataRow);
				if (commodityInfo.YanQiFeeList == null)
				{
					DataSet result = dataSet;
					return result;
				}
				int arg_539_0 = commodityInfo.YanQiFeeList.Count;
				dataRow = dataTable.NewRow();
				dataRow["ProjectL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_DEFERTYPE");
				dataRow["ProjectValL"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_STEPVALUE");
				dataRow["ProjectValR"] = Global.m_PMESResourceManager.GetString("PMESStr_CI_YANQIVALUE");
				dataTable.Rows.Add(dataRow);
				int count = commodityInfo.YanQiFeeList.Count;
				for (int i = 0; i < count; i++)
				{
					dataRow = dataTable.NewRow();
					dataRow["ProjectValL"] = string.Format("{0}至{1}天", commodityInfo.YanQiFeeList[i].StepLow.ToString(), commodityInfo.YanQiFeeList[i].StepValue.ToString());
					dataRow["ProjectValR"] = ((!Global.DoubleIsZero(commodityInfo.YanQiFeeList[i].YanQiValue)) ? commodityInfo.YanQiFeeList[i].YanQiValue.ToString("f15").TrimEnd(new char[]
					{
						'0'
					}).TrimEnd(new char[]
					{
						'.'
					}) : "0.00");
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet GetFirmInfo()
		{
			DataSet dataSet = new DataSet("FirmInfoDataSet");
			DataTable dataTable = new DataTable("tFirmInfo");
			DataColumn column = new DataColumn("FirmName");
			DataColumn column2 = new DataColumn("InitFund", typeof(double));
			DataColumn column3 = new DataColumn("CurrentRight", typeof(double));
			DataColumn column4 = new DataColumn("CurrentFL", typeof(double));
			DataColumn column5 = new DataColumn("RealFund", typeof(double));
			DataColumn column6 = new DataColumn("CurrentBail", typeof(double));
			DataColumn column7 = new DataColumn("OrderFrozenFund", typeof(double));
			DataColumn column8 = new DataColumn("OrderFrozenFee", typeof(double));
			DataColumn column9 = new DataColumn("FundRisk");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataSet.Tables.Add(dataTable);
			try
			{
				FirmInfoRequestVO firmInfoRequestVO = new FirmInfoRequestVO();
				firmInfoRequestVO.AgencyNo = Global.AgencyNo;
				firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				firmInfoRequestVO.UserID = Global.UserID;
				if (this.IsAgency)
				{
					firmInfoRequestVO.AgencyNo = Global.AgencyNo;
					firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					firmInfoRequestVO.AgencyNo = string.Empty;
					firmInfoRequestVO.AgencyPhonePassword = string.Empty;
				}
				FirmInfoResponseVO firmInfo = this._TradeLibrary.GetFirmInfo(firmInfoRequestVO);
				if (firmInfo == null || firmInfo.RetCode != 0L)
				{
					DataSet result = dataSet;
					return result;
				}
				if (this.IsAgency)
				{
					lock (Global.AgencyFirmInfoDataLock)
					{
						Global.AgencyFirmInfoData = (FirmInfoResponseVO)firmInfo.Clone();
						goto IL_228;
					}
				}
				lock (Global.FirmInfoDataLock)
				{
					Global.FirmInfoData = (FirmInfoResponseVO)firmInfo.Clone();
				}
				IL_228:
				DataRow dataRow = dataTable.NewRow();
				double num = BizController.CalculateBalance(firmInfo.InitFund, firmInfo.InOutFund, firmInfo.Fee, firmInfo.YesterdayBail, firmInfo.TransferPL);
				double num2 = BizController.CalculateInitFund(num, firmInfo.CurrentFL);
				double num3 = BizController.CalculateHoldingFund(firmInfo.CurrentBail, firmInfo.OrderFrozenFund, firmInfo.OtherFrozenFund);
				dataRow["FirmName"] = firmInfo.FirmName;
				dataRow["InitFund"] = num2;
				dataRow["CurrentRight"] = num;
				dataRow["CurrentFL"] = firmInfo.CurrentFL;
				dataRow["RealFund"] = firmInfo.RealFund;
				dataRow["CurrentBail"] = num3;
				dataRow["OrderFrozenFund"] = firmInfo.OrderFrozenFund;
				dataRow["OrderFrozenFee"] = firmInfo.OrderFrozenFee;
				dataRow["FundRisk"] = BizController.CalculateFundRisk(num2, num3).ToString("p2");
				dataTable.Rows.Add(dataRow);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public void GetFirmInfoList()
		{
			try
			{
				string path = Global.TrancCodeXml.Substring(0, Global.TrancCodeXml.IndexOf(".")) + ".xsd";
				if (!File.Exists(Global.TrancCodeXml) || !File.Exists(path))
				{
					throw new Exception("没有发现交易员架构文件！");
				}
				FirmInfoRequestVO firmInfoRequestVO = new FirmInfoRequestVO();
				firmInfoRequestVO.AgencyNo = Global.AgencyNo;
				firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				firmInfoRequestVO.UserID = Global.UserID;
				if (this.IsAgency)
				{
					firmInfoRequestVO.AgencyNo = Global.AgencyNo;
					firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					firmInfoRequestVO.AgencyNo = string.Empty;
					firmInfoRequestVO.AgencyPhonePassword = string.Empty;
				}
				FirmInfoResponseVO firmInfo = this._TradeLibrary.GetFirmInfo(firmInfoRequestVO);
				XmlDataSet xmlDataSet = new XmlDataSet(Global.TrancCodeXml);
				DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
				Global.FirmID = firmInfo.FirmID;
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
				array[0] = "false";
				xmlDataSet.WriteBoolXml(columns, array);
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
					xmlDataSet.UpdateXmlRow(columns2, columnValue, "TransactionsCode", array2[i]);
				}
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
					firmInfo.FirmID
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public DataSet GetCustomerOrder(CustomerOrderQueryRequestVO _customerOrderQueryRequestVO)
		{
			DataSet dataSet = null;
			try
			{
				Hashtable hashtable = new Hashtable();
				CustomerOrderQueryResponseVO customerOrderQuery = this._TradeLibrary.GetCustomerOrderQuery(_customerOrderQueryRequestVO);
				dataSet = new DataSet("CODataSet");
				DataTable dataTable = new DataTable("CODetatable");
				DataColumn column = new DataColumn("Commodity");
				DataColumn column2 = new DataColumn("BuyAveragePrice", typeof(string));
				DataColumn column3 = new DataColumn("BuyHoldingAmount", typeof(string));
				DataColumn column4 = new DataColumn("BuyQuantity", typeof(string));
				DataColumn column5 = new DataColumn("BuyFloat", typeof(string));
				DataColumn column6 = new DataColumn("SellAveragePrice", typeof(string));
				DataColumn column7 = new DataColumn("SellHoldingAmount", typeof(string));
				DataColumn column8 = new DataColumn("SellQuantity", typeof(string));
				DataColumn column9 = new DataColumn("SellFloat", typeof(string));
				DataColumn column10 = new DataColumn("JingTouCun", typeof(string));
				DataColumn column11 = new DataColumn("Float", typeof(string));
				DataColumn column12 = new DataColumn("CommodityID");
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
				dataSet.Tables.Add(dataTable);
				if (customerOrderQuery.CustomerOrderQueryList != null && customerOrderQuery.RetCode == 0L)
				{
					foreach (CustomerOrderQuery current in customerOrderQuery.CustomerOrderQueryList)
					{
						if (!hashtable.ContainsKey(current.CommodityID))
						{
							hashtable.Add(current.CommodityID, current);
						}
					}
				}
				if (Global.CommodityData == null)
				{
					Logger.wirte(2, "Global.CommodityData数据为空");
					return dataSet;
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["Commodity"] = current2.Value.CommodityName;
					dataRow["CommodityID"] = current2.Value.CommodityID;
					if (hashtable.ContainsKey(current2.Value.CommodityID))
					{
						CustomerOrderQuery customerOrderQuery2 = (CustomerOrderQuery)hashtable[current2.Value.CommodityID];
						dataRow["BuyAveragePrice"] = customerOrderQuery2.BuyAveragePrice.ToString("f2");
						dataRow["BuyHoldingAmount"] = customerOrderQuery2.BuyHoldingAmount.ToString("n2");
						dataRow["BuyQuantity"] = customerOrderQuery2.BuyQuantity;
						dataRow["BuyFloat"] = customerOrderQuery2.BuyFloat.ToString("n2");
						dataRow["SellAveragePrice"] = customerOrderQuery2.SellAveragePrice.ToString("f2");
						dataRow["SellHoldingAmount"] = customerOrderQuery2.SellHoldingAmount.ToString("n2");
						dataRow["SellQuantity"] = customerOrderQuery2.SellQuantity;
						dataRow["SellFloat"] = customerOrderQuery2.SellFloat.ToString("n2");
						dataRow["JingTouCun"] = customerOrderQuery2.JingTouCun;
						dataRow["Float"] = customerOrderQuery2.Float.ToString("n2");
					}
					else
					{
						dataRow["BuyAveragePrice"] = "--";
						dataRow["BuyHoldingAmount"] = "--";
						dataRow["BuyQuantity"] = "--";
						dataRow["BuyFloat"] = "--";
						dataRow["SellAveragePrice"] = "--";
						dataRow["SellHoldingAmount"] = "--";
						dataRow["SellQuantity"] = "--";
						dataRow["SellFloat"] = "--";
						dataRow["JingTouCun"] = "--";
						dataRow["Float"] = "--";
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return dataSet;
		}
		public ResponseVO SetLossProfit(SetLossProfitRequestVO req)
		{
			if (this.IsAgency)
			{
				req.AgencyNo = Global.AgencyNo;
				req.AgencyPhonePassword = Global.AgencyPhonePassword;
			}
			else
			{
				req.AgencyNo = string.Empty;
				req.AgencyPhonePassword = string.Empty;
			}
			return this._TradeLibrary.SetLossProfit(req);
		}
		public ResponseVO Order(OrderRequestVO req)
		{
			if (this.IsAgency)
			{
				req.AgencyNo = Global.AgencyNo;
				req.AgencyPhonePassword = Global.AgencyPhonePassword;
			}
			else
			{
				req.AgencyNo = string.Empty;
				req.AgencyPhonePassword = string.Empty;
			}
			return this._TradeLibrary.Order(req);
		}
		public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
		{
			if (this.IsAgency)
			{
				req.AgencyNo = Global.AgencyNo;
				req.AgencyPhonePassword = Global.AgencyPhonePassword;
			}
			else
			{
				req.AgencyNo = string.Empty;
				req.AgencyPhonePassword = string.Empty;
			}
			return this._TradeLibrary.WithDrawOrder(req);
		}
		public DataSet QueryTradeOrderInfo(TradeQueryRequestVO tradeQueryRequestVO)
		{
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("OrderNo");
			DataColumn column3 = new DataColumn("Time");
			DataColumn column4 = new DataColumn("TransactionsCode");
			DataColumn column5 = new DataColumn("CommodityID");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price", typeof(double));
			DataColumn column9 = new DataColumn("Qty", typeof(int));
			DataColumn column10 = new DataColumn("LPrice");
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
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					tradeQueryRequestVO.AgencyNo = Global.AgencyNo;
					tradeQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					tradeQueryRequestVO.AgencyNo = string.Empty;
					tradeQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				TradeQueryResponseVO tradeQueryResponseVO = this._TradeLibrary.TradeQuery(tradeQueryRequestVO);
				if (tradeQueryResponseVO != null)
				{
					Logger.wirte(2, "成交委托情况查询错误：数据为空");
					DataSet result = dataSet;
					return result;
				}
				List<TradeInfo> tradeInfoList = tradeQueryResponseVO.TradeInfoList;
				if (tradeInfoList.Count == 0 && tradeQueryResponseVO.RetCode != 0L)
				{
					DataSet result = dataSet;
					return result;
				}
				for (int i = 0; i < tradeInfoList.Count; i++)
				{
					TradeInfo tradeInfo = tradeInfoList[i];
					DataRow dataRow = dataTable.NewRow();
					dataRow["TradeNo"] = tradeInfo.TradeNO;
					dataRow["OrderNo"] = tradeInfo.OrderNO;
					dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
					dataRow["CommodityID"] = tradeInfo.CommodityID;
					dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
					dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
					dataRow["Price"] = tradeInfo.TradePrice;
					dataRow["Qty"] = tradeInfo.TradeQuantity;
					string empty = string.Empty;
					dataRow["LPrice"] = empty;
					dataRow["Comm"] = tradeInfo.Comm;
					dataRow["Market"] = tradeInfo.MarketID;
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet QueryTradeInfo(TradeQueryRequestVO tradeQueryRequestVO)
		{
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("StradeNo", typeof(long));
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("BuySell");
			DataColumn column4 = new DataColumn("Quantity", typeof(long));
			DataColumn column5 = new DataColumn("OpenpRice", typeof(double));
			DataColumn column6 = new DataColumn("HoldPrice", typeof(double));
			DataColumn column7 = new DataColumn("ClosePrice", typeof(double));
			DataColumn column8 = new DataColumn("TransferPL", typeof(double));
			DataColumn column9 = new DataColumn("Comm", typeof(double));
			DataColumn column10 = new DataColumn("OrderNo", typeof(long));
			DataColumn column11 = new DataColumn("HoldingNo", typeof(long));
			DataColumn column12 = new DataColumn("SettleBasis");
			DataColumn column13 = new DataColumn("OtherID");
			DataColumn column14 = new DataColumn("TradeType");
			DataColumn column15 = new DataColumn("TradeOperateType");
			DataColumn column16 = new DataColumn("OrderTime");
			DataColumn column17 = new DataColumn("TradeTime");
			DataColumn column18 = new DataColumn("BuySellVal");
			DataColumn column19 = new DataColumn("SettleBasisVal");
			DataColumn column20 = new DataColumn("TradeTypeVal");
			DataColumn column21 = new DataColumn("CommodityID");
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
			dataTable.Columns.Add(column16);
			dataTable.Columns.Add(column17);
			dataTable.Columns.Add(column18);
			dataTable.Columns.Add(column19);
			dataTable.Columns.Add(column20);
			dataTable.Columns.Add(column21);
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					tradeQueryRequestVO.AgencyNo = Global.AgencyNo;
					tradeQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					tradeQueryRequestVO.AgencyNo = string.Empty;
					tradeQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				TradeQueryResponseVO tradeQueryResponseVO = this._TradeLibrary.TradeQuery(tradeQueryRequestVO);
				if (tradeQueryResponseVO == null)
				{
					Logger.wirte(2, "成交情况查询错误：数据为空");
					DataSet result = dataSet;
					return result;
				}
				List<TradeInfo> tradeInfoList = tradeQueryResponseVO.TradeInfoList;
				if (tradeInfoList.Count == 0 && tradeQueryResponseVO.RetCode != 0L)
				{
					DataSet result = dataSet;
					return result;
				}
				for (int i = 0; i < tradeInfoList.Count; i++)
				{
					TradeInfo tradeInfo = tradeInfoList[i];
					DataRow dataRow = dataTable.NewRow();
					dataRow["OrderNo"] = tradeInfo.OrderNO;
					if (this.IsAgency)
					{
						if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(tradeInfo.CommodityID))
						{
							dataRow["CommodityName"] = Global.AgencyCommodityData[tradeInfo.CommodityID].CommodityName;
							BizController.GetMinSpreadPriceCount(Global.AgencyCommodityData[tradeInfo.CommodityID]);
						}
					}
					else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(tradeInfo.CommodityID))
					{
						dataRow["CommodityName"] = Global.CommodityData[tradeInfo.CommodityID].CommodityName;
						BizController.GetMinSpreadPriceCount(Global.CommodityData[tradeInfo.CommodityID]);
					}
					dataRow["BuySell"] = Global.GetEnumtoResourcesString("BUYSELL", (int)tradeInfo.BuySell);
					dataRow["Quantity"] = tradeInfo.TradeQuantity.ToString();
					dataRow["OpenpRice"] = tradeInfo.OpenPrice;
					dataRow["HoldPrice"] = tradeInfo.HoldingPrice;
					dataRow["ClosePrice"] = tradeInfo.TradePrice;
					dataRow["TransferPL"] = tradeInfo.TransferPL;
					dataRow["Comm"] = -tradeInfo.Comm;
					dataRow["SettleBasis"] = Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeInfo.SettleBasis);
					dataRow["StradeNo"] = tradeInfo.TradeNO;
					dataRow["HoldingNo"] = tradeInfo.HoldingNO;
					dataRow["OrderTime"] = tradeInfo.OrderTime;
					dataRow["TradeTime"] = tradeInfo.TradeTime;
					dataRow["BuySellVal"] = tradeInfo.BuySell.ToString();
					dataRow["SettleBasisVal"] = tradeInfo.SettleBasis.ToString();
					dataRow["CommodityID"] = tradeInfo.CommodityID.ToString();
					dataRow["OtherID"] = tradeInfo.OtherID;
					dataRow["TradeType"] = Global.GetEnumtoResourcesString("TRADEDEALTYPE", (int)tradeInfo.TradeType);
					dataRow["TradeOperateType"] = Global.GetEnumtoResourcesString("TRADEOPERATETYPE", (int)tradeInfo.TradeType);
					dataRow["TradeTypeVal"] = tradeInfo.TradeType.ToString();
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DateTime GetTradeTime()
		{
			DateTime result = default(DateTime);
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = Global.UserID;
			try
			{
				if (this.IsAgency)
				{
					sysTimeQueryRequestVO.AgencyNo = Global.AgencyNo;
					sysTimeQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					sysTimeQueryRequestVO.AgencyNo = string.Empty;
					sysTimeQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				SysTimeQueryResponseVO sysTime = this._TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
				string str = string.Empty;
				string str2 = string.Empty;
				if (sysTime.RetCode == 0L)
				{
					if (sysTime.CurrentDate.Equals("") || sysTime.CurrentTime.Equals(""))
					{
						goto IL_DC;
					}
					str = sysTime.CurrentDate;
					str2 = sysTime.CurrentTime;
					try
					{
						result = DateTime.Parse(str + " " + str2);
						goto IL_DC;
					}
					catch
					{
						result = default(DateTime);
						goto IL_DC;
					}
				}
				Logger.wirte(2, "获取服务器系统时间错误：" + sysTime.RetMessage);
				IL_DC:;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return result;
			}
			return result;
		}
		public DataSet QueryOrderInfo(OrderQueryRequestVO orderQueryRequestVO, SystemStatus systemStatus)
		{
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("CommodityID");
			DataColumn column4 = new DataColumn("SellBuy");
			DataColumn column5 = new DataColumn("TradeQuantity", typeof(long));
			DataColumn column6 = new DataColumn("OrderPrice", typeof(double));
			DataColumn column7 = new DataColumn("StopLossShow");
			DataColumn column8 = new DataColumn("StopLoss", typeof(double));
			DataColumn column9 = new DataColumn("StopProfit", typeof(double));
			DataColumn column10 = new DataColumn("StopProfitShow");
			DataColumn column11 = new DataColumn("HoldingNo", typeof(string));
			DataColumn column12 = new DataColumn("FrozenMargin", typeof(double));
			DataColumn column13 = new DataColumn("FrozenFee", typeof(double));
			DataColumn column14 = new DataColumn("Time");
			DataColumn column15 = new DataColumn("OrderInfoStateVal");
			DataColumn column16 = new DataColumn("OrderTypeVal");
			DataColumn column17 = new DataColumn("OrderInfoState");
			DataColumn column18 = new DataColumn("OrderType");
			DataColumn column19 = new DataColumn("SettleBasis");
			DataColumn column20 = new DataColumn("BuySellVal");
			DataColumn column21 = new DataColumn("SettleBasisVal");
			DataColumn column22 = new DataColumn("AgentID");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column10);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column12);
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column17);
			dataTable.Columns.Add(column18);
			dataTable.Columns.Add(column19);
			dataTable.Columns.Add(column11);
			dataTable.Columns.Add(column14);
			dataTable.Columns.Add(column22);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column15);
			dataTable.Columns.Add(column16);
			dataTable.Columns.Add(column20);
			dataTable.Columns.Add(column21);
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					orderQueryRequestVO.AgencyNo = Global.AgencyNo;
					orderQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					orderQueryRequestVO.AgencyNo = string.Empty;
					orderQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				OrderQueryResponseVO orderQueryResponseVO = this._TradeLibrary.AllOrderQuery(orderQueryRequestVO);
				if (orderQueryResponseVO == null)
				{
					Logger.wirte(2, "委托查询错误：数据为空");
					DataSet result = dataSet;
					return result;
				}
				if (orderQueryResponseVO.RetCode != 0L)
				{
					DataSet result = dataSet;
					return result;
				}
				List<OrderInfo> orderInfoList = orderQueryResponseVO.OrderInfoList;
				if (this.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						DataSet result = dataSet;
						return result;
					}
				}
				else if (Global.CommodityData == null)
				{
					DataSet result = dataSet;
					return result;
				}
				if (orderInfoList.Count == 0)
				{
					DataSet result = dataSet;
					return result;
				}
				for (int i = 0; i < orderInfoList.Count; i++)
				{
					OrderInfo orderInfo = orderInfoList[i];
					DataRow dataRow = dataTable.NewRow();
					dataRow["OrderNo"] = orderInfo.OrderNO;
					if (this.IsAgency)
					{
						if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(orderInfo.CommodityID))
						{
							dataRow["CommodityName"] = Global.AgencyCommodityData[orderInfo.CommodityID].CommodityName.ToString();
						}
						BizController.GetMinSpreadPriceCount(Global.AgencyCommodityData[orderInfo.CommodityID]);
					}
					else
					{
						if (Global.CommodityData != null && Global.CommodityData.ContainsKey(orderInfo.CommodityID))
						{
							dataRow["CommodityName"] = Global.CommodityData[orderInfo.CommodityID].CommodityName.ToString();
						}
						BizController.GetMinSpreadPriceCount(Global.CommodityData[orderInfo.CommodityID]);
					}
					dataRow["OrderPrice"] = orderInfo.OrderPrice;
					dataRow["StopLoss"] = orderInfo.StopLoss;
					dataRow["StopLossShow"] = ((orderInfo.StopLoss > 0.0) ? orderInfo.StopLoss.ToString("f2") : "--");
					dataRow["StopProfit"] = orderInfo.StopProfit;
					dataRow["StopProfitShow"] = ((orderInfo.StopProfit > 0.0) ? orderInfo.StopProfit.ToString("f2") : "--");
					dataRow["CommodityID"] = orderInfo.CommodityID;
					dataRow["SellBuy"] = Global.GetEnumtoResourcesString("BUYSELL", (int)orderInfo.BuySell);
					dataRow["TradeQuantity"] = orderInfo.OrderQuantity;
					dataRow["OrderInfoState"] = Global.GetEnumtoResourcesString("ORDERINFOSTATE", (int)orderInfo.State);
					dataRow["OrderType"] = Global.GetEnumtoResourcesString("TRADETYPE", (int)orderInfo.OrderType);
					dataRow["HoldingNo"] = ((orderInfo.HoldingNO <= 0L) ? string.Empty : orderInfo.HoldingNO.ToString());
					dataRow["Time"] = "当日有效";
					dataRow["OrderInfoStateVal"] = orderInfo.State.ToString();
					dataRow["OrderTypeVal"] = orderInfo.OrderType.ToString();
					dataRow["AgentID"] = orderInfo.AgentID.ToString();
					dataRow["BuySellVal"] = orderInfo.BuySell;
					dataRow["SettleBasisVal"] = orderInfo.SettleBasis.ToString();
					dataRow["SettleBasis"] = Global.GetEnumtoResourcesString("SETTLEBASIS", (int)orderInfo.SettleBasis);
					dataRow["FrozenMargin"] = orderInfo.FrozenMargin;
					dataRow["FrozenFee"] = orderInfo.FrozenFee;
					if (systemStatus == SystemStatus.SettlementComplete && orderInfo.State == 1)
					{
						dataRow["OrderInfoStateVal"] = OrderInfoState.HasCancellation.ToString("d");
						dataRow["OrderInfoState"] = Global.GetEnumtoResourcesString("ORDERINFOSTATE", 3);
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet QueryTodayOrderInfo(OrderQueryRequestVO orderQueryRequestVO)
		{
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("B_S");
			DataColumn column6 = new DataColumn("O_L");
			DataColumn column7 = new DataColumn("Price", typeof(double));
			DataColumn column8 = new DataColumn("Qty");
			DataColumn column9 = new DataColumn("Balance");
			DataColumn column10 = new DataColumn("Status");
			DataColumn column11 = new DataColumn("Market");
			DataColumn column12 = new DataColumn("CBasis");
			DataColumn column13 = new DataColumn("BillTradeType");
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
			DataSet result;
			try
			{
				result = dataSet;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				result = dataSet;
			}
			return result;
		}
		public DataSet QueryHoldingInfo(HoldingQueryRequestVO holdingQueryRequestVO, SystemStatus _currentSystemStatus)
		{
			DataSet dataSet = new DataSet("holdingDataSet");
			DataTable dataTable = new DataTable("Holding");
			DataColumn column = new DataColumn("CommodityName");
			DataColumn column2 = new DataColumn("CommodityID");
			DataColumn column3 = new DataColumn("BuySell");
			DataColumn column4 = new DataColumn("BuySellText");
			DataColumn column5 = new DataColumn("Qty", typeof(long));
			DataColumn column6 = new DataColumn("OpenAveragePrice", typeof(double));
			DataColumn column7 = new DataColumn("HoldingAveragePrice", typeof(double));
			DataColumn column8 = new DataColumn("ClosePrice", typeof(double));
			DataColumn column9 = new DataColumn("FloatingLP", typeof(double));
			DataColumn column10 = new DataColumn("CommPrice", typeof(double));
			DataColumn column11 = new DataColumn("Bail", typeof(double));
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
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					holdingQueryRequestVO.AgencyNo = Global.AgencyNo;
					holdingQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					holdingQueryRequestVO.AgencyNo = string.Empty;
					holdingQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				HoldingQueryResponseVO holdingQueryResponseVO = this._TradeLibrary.HoldingQuery(holdingQueryRequestVO);
				if (holdingQueryResponseVO == null || holdingQueryResponseVO.HoldingInfoList == null)
				{
					Logger.wirte(2, "持仓汇总查询错误:数据为空");
					DataSet result = dataSet;
					return result;
				}
				if (holdingQueryResponseVO.RetCode != 0L)
				{
					if (holdingQueryResponseVO.RetCode != -202L)
					{
						Logger.wirte(2, string.Format("持仓查询错误：{0}({1})", holdingQueryResponseVO.RetMessage, holdingQueryResponseVO.RetCode.ToString()));
					}
					DataSet result = dataSet;
					return result;
				}
				if (this.IsAgency)
				{
					Global.AgencyHoldingInfoList = holdingQueryResponseVO.HoldingInfoList;
					if (Global.AgencyHoldingInfoList.Count == 0)
					{
						DataSet result = dataSet;
						return result;
					}
					int i = 0;
					while (i < Global.AgencyHoldingInfoList.Count)
					{
						HoldingInfo holdingInfo = Global.AgencyHoldingInfoList[i];
						DataRow dataRow = dataTable.NewRow();
						if (this.IsAgency)
						{
							if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(holdingInfo.CommodityID))
							{
								dataRow["CommodityName"] = Global.AgencyCommodityData[holdingInfo.CommodityID].CommodityName;
							}
						}
						else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(holdingInfo.CommodityID))
						{
							dataRow["CommodityName"] = Global.CommodityData[holdingInfo.CommodityID].CommodityName;
						}
						dataRow["CommodityID"] = holdingInfo.CommodityID;
						dataRow["BuySell"] = holdingInfo.TradeType.ToString();
						dataRow["BuySellText"] = Global.GetEnumtoResourcesString("BUYSELL", (int)holdingInfo.TradeType);
						dataRow["Qty"] = holdingInfo.Qty;
						dataRow["OpenAveragePrice"] = holdingInfo.OpenAveragePrice;
						dataRow["HoldingAveragePrice"] = holdingInfo.HoldingAveragePrice;
						Dictionary<string, CommData> dictionary = null;
						if (this.IsAgency)
						{
							lock (Global.AgencyHQCommDataLock)
							{
								if (Global.AgencyHQCommData != null)
								{
									dictionary = Global.gAgencyHQCommData;
								}
								goto IL_3D0;
							}
							goto IL_3AB;
						}
						goto IL_3AB;
						IL_3D0:
						if (_currentSystemStatus != SystemStatus.SettlementComplete)
						{
							if (dictionary != null && dictionary.ContainsKey(holdingInfo.CommodityID))
							{
								if (holdingInfo.TradeType == 2)
								{
									dataRow["ClosePrice"] = dictionary[holdingInfo.CommodityID].BuyPrice;
								}
								else if (holdingInfo.TradeType == 1)
								{
									dataRow["ClosePrice"] = dictionary[holdingInfo.CommodityID].SellPrice;
								}
							}
							else
							{
								dataRow["ClosePrice"] = 0;
							}
						}
						else
						{
							dataRow["ClosePrice"] = 0;
						}
						dataRow["FloatingLP"] = holdingInfo.FloatingLP;
						dataRow["CommPrice"] = holdingInfo.CommPrice;
						dataRow["Bail"] = holdingInfo.Bail;
						dataTable.Rows.Add(dataRow);
						i++;
						continue;
						IL_3AB:
						lock (Global.HQCommDataLock)
						{
							if (Global.HQCommData != null)
							{
								dictionary = Global.gHQCommData;
							}
						}
						goto IL_3D0;
					}
				}
				else
				{
					Global.HoldingInfoList = holdingQueryResponseVO.HoldingInfoList;
					if (Global.HoldingInfoList.Count == 0)
					{
						DataSet result = dataSet;
						return result;
					}
					int j = 0;
					while (j < Global.HoldingInfoList.Count)
					{
						HoldingInfo holdingInfo2 = Global.HoldingInfoList[j];
						DataRow dataRow = dataTable.NewRow();
						if (this.IsAgency)
						{
							if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(holdingInfo2.CommodityID))
							{
								dataRow["CommodityName"] = Global.AgencyCommodityData[holdingInfo2.CommodityID].CommodityName;
							}
						}
						else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(holdingInfo2.CommodityID))
						{
							dataRow["CommodityName"] = Global.CommodityData[holdingInfo2.CommodityID].CommodityName;
						}
						dataRow["CommodityID"] = holdingInfo2.CommodityID;
						dataRow["BuySell"] = holdingInfo2.TradeType.ToString();
						dataRow["BuySellText"] = Global.GetEnumtoResourcesString("BUYSELL", (int)holdingInfo2.TradeType);
						dataRow["Qty"] = holdingInfo2.Qty;
						dataRow["OpenAveragePrice"] = holdingInfo2.OpenAveragePrice;
						dataRow["HoldingAveragePrice"] = holdingInfo2.HoldingAveragePrice;
						Dictionary<string, CommData> dictionary2 = null;
						if (this.IsAgency)
						{
							lock (Global.AgencyHQCommDataLock)
							{
								if (Global.AgencyHQCommData != null)
								{
									dictionary2 = Global.gAgencyHQCommData;
								}
								goto IL_687;
							}
							goto IL_662;
						}
						goto IL_662;
						IL_687:
						if (dictionary2 != null && dictionary2.ContainsKey(holdingInfo2.CommodityID))
						{
							dataRow["ClosePrice"] = dictionary2[holdingInfo2.CommodityID].SellPrice;
						}
						dataRow["FloatingLP"] = holdingInfo2.FloatingLP;
						dataRow["CommPrice"] = holdingInfo2.CommPrice;
						dataRow["Bail"] = holdingInfo2.Bail;
						dataTable.Rows.Add(dataRow);
						j++;
						continue;
						IL_662:
						lock (Global.HQCommDataLock)
						{
							if (Global.HQCommData != null)
							{
								dictionary2 = Global.gHQCommData;
							}
						}
						goto IL_687;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public DataSet QueryHoldingDetailInfo(HoldingDetailRequestVO _HoldingDetailRequestVO, SystemStatus _currentSystemStatus)
		{
			DataSet dataSet = new DataSet("HDIDataSet");
			DataTable dataTable = new DataTable("HDIDetatable");
			DataColumn column = new DataColumn("HoldingID", typeof(long));
			DataColumn column2 = new DataColumn("CommodityName");
			DataColumn column3 = new DataColumn("BuySellText");
			DataColumn column4 = new DataColumn("OpenQuantity", typeof(long));
			DataColumn column5 = new DataColumn("HoldingQuantity", typeof(long));
			DataColumn column6 = new DataColumn("UnitQty", typeof(long));
			DataColumn column7 = new DataColumn("OpenPrice", typeof(double));
			DataColumn column8 = new DataColumn("HoldPrice", typeof(double));
			DataColumn column9 = new DataColumn("ClosePrice", typeof(double));
			DataColumn column10 = new DataColumn("StopLoss", typeof(double));
			DataColumn column11 = new DataColumn("StopLossShow");
			DataColumn column12 = new DataColumn("StopProfit", typeof(double));
			DataColumn column13 = new DataColumn("StopProfitShow");
			DataColumn dataColumn = new DataColumn("HoldingFloat", typeof(double));
			DataColumn dataColumn2 = new DataColumn("FloatingPrice", typeof(double));
			DataColumn dataColumn3 = new DataColumn("TotalFloat", typeof(double));
			DataColumn column14 = new DataColumn("CommPrice", typeof(double));
			DataColumn column15 = new DataColumn("Bail", typeof(double));
			DataColumn column16 = new DataColumn("OtherID");
			DataColumn column17 = new DataColumn("OrderTime");
			DataColumn column18 = new DataColumn("CommodityID");
			DataColumn column19 = new DataColumn("BuySell");
			DataColumn column20 = new DataColumn("AgentID");
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
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column12);
			dataTable.Columns.Add(dataColumn);
			dataTable.Columns.Add(dataColumn2);
			dataTable.Columns.Add(dataColumn3);
			dataTable.Columns.Add(column14);
			dataTable.Columns.Add(column15);
			dataTable.Columns.Add(column16);
			dataTable.Columns.Add(column17);
			dataTable.Columns.Add(column20);
			dataTable.Columns.Add(column18);
			dataTable.Columns.Add(column19);
			if (_currentSystemStatus != SystemStatus.SettlementComplete)
			{
				dataColumn2.Expression = "iif(BuySell=1,(ClosePrice -HoldPrice)*HoldingQuantity*UnitQty,-(ClosePrice -HoldPrice)*HoldingQuantity*UnitQty)";
				dataColumn.Expression = "iif(BuySell=1,(HoldPrice -OpenPrice)*HoldingQuantity*UnitQty,-(HoldPrice -OpenPrice)*HoldingQuantity*UnitQty)";
				dataColumn3.Expression = "FloatingPrice+HoldingFloat";
			}
			dataSet.Tables.Add(dataTable);
			try
			{
				if (this.IsAgency)
				{
					_HoldingDetailRequestVO.AgencyNo = Global.AgencyNo;
					_HoldingDetailRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					_HoldingDetailRequestVO.AgencyNo = string.Empty;
					_HoldingDetailRequestVO.AgencyPhonePassword = string.Empty;
				}
				HoldingDetailResponseVO holdingDetailResponseVO = this._TradeLibrary.HoldPtByPrice(_HoldingDetailRequestVO);
				if (holdingDetailResponseVO == null)
				{
					Logger.wirte(2, "持仓明细查询错误：数据为空");
				}
				List<HoldingDetailInfo> list = null;
				if (holdingDetailResponseVO.RetCode != 0L)
				{
					DataSet result = dataSet;
					return result;
				}
				list = holdingDetailResponseVO.HoldingDetailInfoList;
				if (this.IsAgency)
				{
					if (Global.AgencyCommodityData == null || Global.AgencyCommodityData.Count == 0)
					{
						Logger.wirte(2, "订货明细查询错误：" + holdingDetailResponseVO.RetMessage);
						DataSet result = dataSet;
						return result;
					}
				}
				else if (Global.CommodityData == null || Global.CommodityData.Count == 0)
				{
					Logger.wirte(2, "订货明细查询错误：" + holdingDetailResponseVO.RetMessage);
					DataSet result = dataSet;
					return result;
				}
				int i = 0;
				while (i < list.Count)
				{
					HoldingDetailInfo holdingDetailInfo = list[i];
					DataRow dataRow = dataTable.NewRow();
					dataRow["HoldingID"] = holdingDetailInfo.HoldingID;
					if (this.IsAgency)
					{
						if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(holdingDetailInfo.CommodityID))
						{
							dataRow["CommodityName"] = Global.AgencyCommodityData[holdingDetailInfo.CommodityID].CommodityName.ToString();
							dataRow["UnitQty"] = Global.AgencyCommodityData[holdingDetailInfo.CommodityID].CtrtSize;
						}
						else
						{
							dataRow["CommodityName"] = "--";
							dataRow["UnitQty"] = 0;
						}
					}
					else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(holdingDetailInfo.CommodityID))
					{
						dataRow["CommodityName"] = Global.CommodityData[holdingDetailInfo.CommodityID].CommodityName.ToString();
						dataRow["UnitQty"] = Global.CommodityData[holdingDetailInfo.CommodityID].CtrtSize;
					}
					else
					{
						dataRow["CommodityName"] = "--";
						dataRow["UnitQty"] = 0;
					}
					dataRow["BuySellText"] = Global.GetEnumtoResourcesString("BUYSELL", (int)holdingDetailInfo.BuySell);
					dataRow["OpenQuantity"] = holdingDetailInfo.OpenQuantity;
					dataRow["HoldingQuantity"] = holdingDetailInfo.HoldingQuantity;
					dataRow["OpenPrice"] = holdingDetailInfo.OpenPrice;
					dataRow["HoldPrice"] = holdingDetailInfo.HoldPrice;
					Dictionary<string, CommData> dictionary = null;
					if (this.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData != null)
							{
								dictionary = Global.gAgencyHQCommData;
							}
							goto IL_623;
						}
						goto IL_5FE;
					}
					goto IL_5FE;
					IL_623:
					if (_currentSystemStatus != SystemStatus.SettlementComplete)
					{
						if (dictionary != null && dictionary.ContainsKey(holdingDetailInfo.CommodityID))
						{
							if (holdingDetailInfo.BuySell == 2)
							{
								dataRow["ClosePrice"] = dictionary[holdingDetailInfo.CommodityID].BuyPrice;
							}
							else if (holdingDetailInfo.BuySell == 1)
							{
								dataRow["ClosePrice"] = dictionary[holdingDetailInfo.CommodityID].SellPrice;
							}
						}
						else
						{
							dataRow["ClosePrice"] = 0;
						}
					}
					else
					{
						dataRow["ClosePrice"] = 0;
					}
					dataRow["StopLossShow"] = ((holdingDetailInfo.StopLoss > 0.0) ? holdingDetailInfo.StopLoss.ToString("f2") : "--");
					dataRow["StopLoss"] = holdingDetailInfo.StopLoss;
					dataRow["StopProfitShow"] = ((holdingDetailInfo.StopProfit > 0.0) ? holdingDetailInfo.StopProfit.ToString("f2") : "--");
					dataRow["StopProfit"] = holdingDetailInfo.StopProfit;
					if (_currentSystemStatus == SystemStatus.SettlementComplete)
					{
						dataRow["FloatingPrice"] = holdingDetailInfo.TotalFloatingPrice;
					}
					else
					{
						dataRow["FloatingPrice"] = 0;
					}
					dataRow["CommPrice"] = ((holdingDetailInfo.CommPrice >= 0.0) ? holdingDetailInfo.CommPrice : 0.0);
					dataRow["Bail"] = ((holdingDetailInfo.Bail >= 0.0) ? holdingDetailInfo.Bail : 0.0);
					dataRow["OrderTime"] = holdingDetailInfo.OrderTime.ToString();
					dataRow["OtherID"] = holdingDetailInfo.OtherID.ToString();
					dataRow["AgentID"] = holdingDetailInfo.AgentID.ToString();
					dataRow["CommodityID"] = holdingDetailInfo.CommodityID.ToString();
					dataRow["BuySell"] = holdingDetailInfo.BuySell.ToString();
					dataTable.Rows.Add(dataRow);
					i++;
					continue;
					IL_5FE:
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData != null)
						{
							dictionary = Global.gHQCommData;
						}
					}
					goto IL_623;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				DataSet result = dataSet;
				return result;
			}
			return dataSet;
		}
		public FirmInfoResponseVO QueryFundsInfo()
		{
			FirmInfoResponseVO firmInfoResponseVO = null;
			try
			{
				FirmInfoRequestVO firmInfoRequestVO = new FirmInfoRequestVO();
				firmInfoRequestVO.AgencyNo = Global.AgencyNo;
				firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				firmInfoRequestVO.UserID = Global.UserID;
				if (this.IsAgency)
				{
					firmInfoRequestVO.AgencyNo = Global.AgencyNo;
					firmInfoRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					firmInfoRequestVO.AgencyNo = string.Empty;
					firmInfoRequestVO.AgencyPhonePassword = string.Empty;
				}
				firmInfoResponseVO = this._TradeLibrary.GetFirmInfo(firmInfoRequestVO);
				if (firmInfoResponseVO == null)
				{
					Logger.wirte(2, "会员信息查询错误：数据为空");
					FirmInfoResponseVO result = null;
					return result;
				}
				if (firmInfoResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "会员信息查询错误：" + firmInfoResponseVO.RetMessage);
					FirmInfoResponseVO result = null;
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				FirmInfoResponseVO result = null;
				return result;
			}
			return firmInfoResponseVO;
		}
		public FirmFundsInfoResponseVO QueryFirmFundsInfo()
		{
			FirmFundsInfoResponseVO firmFundsInfoResponseVO = null;
			try
			{
				firmFundsInfoResponseVO = this._TradeLibrary.GetFirmFundsInfo(Global.UserID);
				if (firmFundsInfoResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "会员资金信息查询错误：" + firmFundsInfoResponseVO.RetMessage);
					FirmFundsInfoResponseVO result = null;
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				FirmFundsInfoResponseVO result = null;
				return result;
			}
			return firmFundsInfoResponseVO;
		}
		public CommodityInfo QueryCommodityInfo(string MarKet, string CommodityID)
		{
			int num = CommodityID.IndexOf("_");
			CommodityInfo result;
			try
			{
				if (num != -1)
				{
					MarKet = CommodityID.Substring(0, num);
					CommodityID = CommodityID.Substring(num + 1);
				}
				CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
				commodityQueryRequestVO.UserID = Global.UserID;
				commodityQueryRequestVO.MarketID = MarKet;
				commodityQueryRequestVO.CommodityID = CommodityID;
				if (this.IsAgency)
				{
					commodityQueryRequestVO.AgencyNo = Global.AgencyNo;
					commodityQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					commodityQueryRequestVO.AgencyNo = string.Empty;
					commodityQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				CommodityQueryResponseVO commodityQueryResponseVO = this._TradeLibrary.CommodityQuery(commodityQueryRequestVO);
				if (commodityQueryResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "查询商品信息错误：" + commodityQueryResponseVO.RetMessage);
					result = null;
				}
				else
				{
					Dictionary<string, CommodityInfo> commodityInfoList = commodityQueryResponseVO.CommodityInfoList;
					if (commodityInfoList.Count > 0 && commodityInfoList.ContainsKey(CommodityID))
					{
						result = commodityInfoList[CommodityID];
					}
					else
					{
						Logger.wirte(2, "查询商品信息错误：" + commodityQueryResponseVO.RetMessage);
						result = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = null;
			}
			return result;
		}
		public Dictionary<string, CommodityInfo> QueryAllCommodityInfo(string MarKet)
		{
			CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
			CommodityQueryResponseVO commodityQueryResponseVO = null;
			try
			{
				commodityQueryRequestVO.UserID = Global.UserID;
				commodityQueryRequestVO.MarketID = MarKet;
				commodityQueryRequestVO.CommodityID = string.Empty;
				if (this.IsAgency)
				{
					commodityQueryRequestVO.AgencyNo = Global.AgencyNo;
					commodityQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					commodityQueryRequestVO.AgencyNo = string.Empty;
					commodityQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				commodityQueryResponseVO = this._TradeLibrary.CommodityQuery(commodityQueryRequestVO);
				if (commodityQueryResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "查询商品信息错误：" + commodityQueryResponseVO.RetMessage);
					Dictionary<string, CommodityInfo> result = null;
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				Dictionary<string, CommodityInfo> result = null;
				return result;
			}
			return commodityQueryResponseVO.CommodityInfoList;
		}
		public CommData QueryGNCommodityInfo(string MarketID, string CommodityID)
		{
			CommData result;
			try
			{
				CommDataQueryRequestVO commDataQueryRequestVO = new CommDataQueryRequestVO();
				commDataQueryRequestVO.UserID = Global.UserID;
				commDataQueryRequestVO.MarketID = MarketID;
				commDataQueryRequestVO.CommodityID = CommodityID;
				if (this.IsAgency)
				{
					commDataQueryRequestVO.AgencyNo = Global.AgencyNo;
					commDataQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					commDataQueryRequestVO.AgencyNo = string.Empty;
					commDataQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				CommDataQueryResponseVO commDataQueryResponseVO = this._TradeLibrary.CommDataQuery(commDataQueryRequestVO);
				if (commDataQueryResponseVO.RetCode != 0L)
				{
					Logger.wirte(3, "查询商品行情错误：" + commDataQueryResponseVO.RetMessage);
					result = null;
				}
				else
				{
					Dictionary<string, CommData> commDataList = commDataQueryResponseVO.CommDataList;
					if (commDataList.ContainsKey(CommodityID))
					{
						result = commDataList[CommodityID];
					}
					else
					{
						result = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = null;
			}
			return result;
		}
		public Dictionary<string, CommData> QueryAllGNCommodityInfo(string marketID)
		{
			CommDataQueryResponseVO commDataQueryResponseVO = null;
			try
			{
				CommDataQueryRequestVO commDataQueryRequestVO = new CommDataQueryRequestVO();
				commDataQueryRequestVO.UserID = Global.UserID;
				commDataQueryRequestVO.MarketID = marketID;
				commDataQueryRequestVO.CommodityID = string.Empty;
				commDataQueryRequestVO.Identity = this._sIdentity.ToString("d");
				if (this.IsAgency)
				{
					commDataQueryRequestVO.AgencyNo = Global.AgencyNo;
					commDataQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					commDataQueryRequestVO.AgencyNo = string.Empty;
					commDataQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				commDataQueryResponseVO = this._TradeLibrary.CommDataQuery(commDataQueryRequestVO);
				if (commDataQueryResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "查询商品行情错误：" + commDataQueryResponseVO.RetMessage);
					Dictionary<string, CommData> result = null;
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				Dictionary<string, CommData> result = null;
				return result;
			}
			return commDataQueryResponseVO.CommDataList;
		}
		public List<EspecialMemberQuery> GetEspecialMemberList()
		{
			EspecialMemberQueryResponseVO especialMemberQueryResponseVO = null;
			try
			{
				especialMemberQueryResponseVO = this._TradeLibrary.GetEspecialMemberQuery(Global.UserID, true);
				if (especialMemberQueryResponseVO.RetCode != 0L)
				{
					Logger.wirte(2, "查询交易对手错误：" + especialMemberQueryResponseVO.RetMessage);
					List<EspecialMemberQuery> result = null;
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				List<EspecialMemberQuery> result = null;
				return result;
			}
			return especialMemberQueryResponseVO.EspecialMemberQueryList;
		}
		public Hashtable GetAllEspecialMemberList()
		{
			Hashtable hashtable = null;
			try
			{
				EspecialMemberQueryResponseVO especialMemberQuery = this._TradeLibrary.GetEspecialMemberQuery(Global.UserID, false);
				if (especialMemberQuery.RetCode != 0L)
				{
					Logger.wirte(2, "查询交易对手错误：" + especialMemberQuery.RetMessage);
					Hashtable result = null;
					return result;
				}
				hashtable = new Hashtable();
				foreach (EspecialMemberQuery current in especialMemberQuery.EspecialMemberQueryList)
				{
					if (!hashtable.ContainsKey(current.EspecialMemberID))
					{
						hashtable.Add(current.EspecialMemberID, current);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				Hashtable result = null;
				return result;
			}
			return hashtable;
		}
		public List<EspecialMemberQuery> GetAgencyEspecialMemberList(string userId)
		{
			List<EspecialMemberQuery> list = null;
			try
			{
				list = new List<EspecialMemberQuery>();
				EspecialMemberQuery especialMemberQuery = new EspecialMemberQuery();
				FirmInfoRequestVO firmInfoRequestVO = new FirmInfoRequestVO();
				firmInfoRequestVO.UserID = userId;
				FirmInfoResponseVO firmInfo = this._TradeLibrary.GetFirmInfo(firmInfoRequestVO);
				if (firmInfo != null && firmInfo.RetCode == 0L)
				{
					especialMemberQuery.EspecialMemberName = firmInfo.FirmName;
					especialMemberQuery.EspecialMemberID = firmInfo.FirmID;
				}
				else
				{
					Logger.wirte(2, "获取电话下单交易对手账户名称异常userId：" + userId);
				}
				list.Add(especialMemberQuery);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return null;
			}
			return list;
		}
		public Hashtable QueryMarketInfo(MarketQueryRequestVO req)
		{
			return new Hashtable();
		}
		public int CalculatLargestTradeNum(CommodityInfo commodityInfo, double price, short B_SType, short O_LType, int prevClear, string MarketID, string CustomerID)
		{
			double num = 0.0;
			try
			{
				if (O_LType == 1)
				{
					double num2 = 0.0;
					double num3 = 0.0;
					double num4 = 0.0;
					double num5 = 0.0;
					double num6 = 0.0;
					FirmInfoResponseVO firmInfoResponseVO = this.QueryFundsInfo();
					double num7 = firmInfoResponseVO.RealFund + firmInfoResponseVO.ImpawnFund;
					short marginType = commodityInfo.MarginType;
					short commType = commodityInfo.CommType;
					double ctrtSize = commodityInfo.CtrtSize;
					if (num7 <= 0.0)
					{
						int result = 0;
						return result;
					}
					double num8 = 0.0;
					double num9 = 0.0;
					if (B_SType == 1 && price > 0.0 && ctrtSize > 0.0 && (num2 > 0.0 || num2 == -1.0))
					{
						if (marginType == 2)
						{
							if (num2 == -1.0)
							{
								num8 = price * ctrtSize;
							}
							else
							{
								num8 = num2 - num4;
							}
						}
						else if (num2 == -1.0)
						{
							num8 = price * ctrtSize;
						}
						else
						{
							num8 = price * ctrtSize * (num2 - num4) / 100.0;
						}
						if (commType == 2)
						{
							num9 = num6;
						}
						else
						{
							num9 = price * ctrtSize * num6 / 100.0;
						}
					}
					else if (B_SType == 2 && price > 0.0 && ctrtSize > 0.0 && (num3 > 0.0 || num3 == -1.0))
					{
						if (marginType == 2)
						{
							if (num3 == -1.0)
							{
								num8 = price * ctrtSize;
							}
							else
							{
								num8 = num3 - num5;
							}
						}
						else if (num3 == -1.0)
						{
							num8 = price * ctrtSize;
						}
						else
						{
							num8 = price * ctrtSize * (num3 - num5) / 100.0;
						}
						if (commType == 2)
						{
							num9 = num6;
						}
						else
						{
							num9 = price * ctrtSize * num6 / 100.0;
						}
					}
					if (num8 + num9 > 0.0)
					{
						num = num7 / (num8 + num9);
					}
				}
				else
				{
					HoldingQueryRequestVO holdingQueryRequestVO = new HoldingQueryRequestVO();
					holdingQueryRequestVO.UserID = Global.UserID;
					holdingQueryRequestVO.CommodityID = commodityInfo.CommodityID;
					if (this.IsAgency)
					{
						holdingQueryRequestVO.AgencyNo = Global.AgencyNo;
						holdingQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
					}
					else
					{
						holdingQueryRequestVO.AgencyNo = string.Empty;
						holdingQueryRequestVO.AgencyPhonePassword = string.Empty;
					}
					HoldingQueryResponseVO holdingQueryResponseVO = this._TradeLibrary.HoldingQuery(holdingQueryRequestVO);
					if (holdingQueryResponseVO.RetCode == 0L)
					{
						List<HoldingInfo> holdingInfoList = holdingQueryResponseVO.HoldingInfoList;
						int num10 = 0;
						if (num10 < holdingInfoList.Count)
						{
							HoldingInfo arg_2C7_0 = holdingInfoList[num10];
							if (B_SType == 1)
							{
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				int result = (int)num;
				return result;
			}
			return (int)num;
		}
	}
}
