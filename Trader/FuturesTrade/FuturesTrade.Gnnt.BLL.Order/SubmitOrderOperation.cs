using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.Drawing;
using System.Threading;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class SubmitOrderOperation : QueryOperation
	{
		public delegate void SetFocusCallBack(short flag);
		public delegate void OrderMessageCallBack(long retCode, string retMessage);
		public delegate string SubmitPredelegateCallBack(string[] Columns, string[] ColumnValue);
		private string idMax = "1";
		private string GoodsId = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsId");
		private string GoodsPrice = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsPrice");
		private string GoodsNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsNum");
		private string GoodsSaleType = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsSaleType");
		private string SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureOrder");
		private string OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OrderInfo");
		public SubmitOrderOperation.SetFocusCallBack SetFocus;
		public SubmitOrderOperation.OrderMessageCallBack OrderMessage;
		public SubmitOrderOperation.SubmitPredelegateCallBack SubmitPredelegateInfo;
		public void ButtonOrderComm(SubmitOrderInfo orderInfo, byte btnFlag)
		{
			if (TradeDataInfo.CommodityHashtable.ContainsKey(orderInfo.commodityID))
			{
				CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[orderInfo.commodityID];
				if (orderInfo.price > commodityInfo.SpreadUp || orderInfo.price < commodityInfo.SpreadDown)
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceErrorMessege");
					if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
					{
						OperationManager.GetInstance().orderOperation.setLargestTN(string.Format(@string, commodityInfo.SpreadUp, commodityInfo.SpreadDown), 1);
					}
					if (this.SetFocus != null)
					{
						this.SetFocus(1);
						return;
					}
				}
				else
				{
					if (Convert.ToInt64(orderInfo.price * 100000.0) % Convert.ToInt64((decimal)commodityInfo.Spread * 100000m) != 0L)
					{
						string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorPrice");
						if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
						{
							OperationManager.GetInstance().orderOperation.setLargestTN(string.Concat(new object[]
							{
								string2,
								"【",
								commodityInfo.Spread,
								"】"
							}), 1);
						}
						if (this.SetFocus != null)
						{
							this.SetFocus(1);
							return;
						}
					}
					else
					{
						if (orderInfo.qty <= 0)
						{
							string string3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
							if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
							{
								OperationManager.GetInstance().orderOperation.setLargestTN(string3, 1);
							}
							if (this.SetFocus != null)
							{
								this.SetFocus(2);
								return;
							}
						}
						else
						{
							if (Convert.ToDouble(orderInfo.qty) % Convert.ToDouble(commodityInfo.MinQty) == 0.0)
							{
								this.SubmitOrderInfo(orderInfo, btnFlag);
								return;
							}
							string string4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsError");
							if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
							{
								OperationManager.GetInstance().orderOperation.setLargestTN(string.Concat(new object[]
								{
									string4,
									"【",
									commodityInfo.MinQty,
									"】"
								}), 1);
							}
							if (this.SetFocus != null)
							{
								this.SetFocus(2);
								return;
							}
						}
					}
				}
			}
			else
			{
				string string5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoExistInputGoods");
				if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
				{
					OperationManager.GetInstance().orderOperation.setLargestTN(string5, 1);
				}
				if (this.SetFocus != null)
				{
					this.SetFocus(0);
				}
			}
		}
		public void SubmitOrderInfo(SubmitOrderInfo orderInfo, byte btnFlag)
		{
			OrderRequestVO orderRequestVO = new OrderRequestVO();
			orderRequestVO.UserID = Global.UserID;
			orderRequestVO.CustomerID = orderInfo.customerID;
			orderRequestVO.MarketID = Global.MarketID;
			orderRequestVO.BuySell = orderInfo.B_SType;
			orderRequestVO.CommodityID = orderInfo.commodityID;
			orderRequestVO.Price = orderInfo.price;
			orderRequestVO.Quantity = (long)orderInfo.qty;
			orderRequestVO.SettleBasis = orderInfo.O_LType;
			orderRequestVO.CloseMode = orderInfo.closeMode;
			orderRequestVO.TimeFlag = orderInfo.timeFlag;
			orderRequestVO.LPrice = orderInfo.lPrice;
			orderRequestVO.BillType = orderInfo.billType;
			if (IniData.GetInstance().ShowDialog)
			{
				string text = string.Empty;
				if (btnFlag == 0)
				{
					this.SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureOrder");
					this.OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OrderInfo");
				}
				else
				{
					if (btnFlag == 1)
					{
						this.SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureEmbedOrder");
						this.OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_EmbedOrderInfo");
					}
				}
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					this.GoodsId,
					orderRequestVO.CommodityID,
					"\r\n",
					this.GoodsPrice,
					orderRequestVO.Price,
					"   ",
					this.GoodsNum,
					orderRequestVO.Quantity,
					"\r\n",
					this.GoodsSaleType,
					Global.BuySellStrArr[(int)orderRequestVO.BuySell],
					"   ",
					Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
					"\r\n\u3000\u3000\u3000",
					this.SureOrder
				});
				MessageForm messageForm = new MessageForm(this.OrderInfo, text, 0);
				if (orderInfo.B_SType == 1)
				{
					messageForm.ForeColor = Color.Red;
				}
				else
				{
					messageForm.ForeColor = Color.Green;
				}
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					if (btnFlag == 0)
					{
						this.SubmitOrderThread(orderRequestVO);
						return;
					}
					if (btnFlag == 1)
					{
						this.SubmitPredelegate(orderRequestVO);
						return;
					}
				}
			}
			else
			{
				if (btnFlag == 0)
				{
					this.SubmitOrderThread(orderRequestVO);
					return;
				}
				if (btnFlag == 1)
				{
					this.SubmitPredelegate(orderRequestVO);
				}
			}
		}
		public void SubmitOrderThread(object orderRequertVO)
		{
			WaitCallback callBack = new WaitCallback(this.SubmitOrder);
			ThreadPool.QueueUserWorkItem(callBack, orderRequertVO);
		}
		public void SubmitOrder(object _orderRequestVO)
		{
			OrderRequestVO req = (OrderRequestVO)_orderRequestVO;
			ResponseVO responseVO = this.serviceManage.CreateEntrustOrder().Order(req);
			if (this.RefreshCurrentTab != null && responseVO.RetCode == 0L)
			{
				this.RefreshCurrentTab(0, true);
			}
			if (this.OrderMessage != null && responseVO != null)
			{
				this.OrderMessage(responseVO.RetCode, responseVO.RetMessage);
			}
		}
		private void SubmitPredelegate(OrderRequestVO orderRequertVO)
		{
			string[] columns = new string[]
			{
				"ID",
				"TransactionsCode",
				"commodityCode",
				"B_S",
				"O_L",
				"price",
				"qty",
				"MarKet",
				"LPrice",
				"TodayPosition",
				"CloseMode",
				"TimeFlag"
			};
			string text = string.Empty;
			if (orderRequertVO.SettleBasis == 2)
			{
				if (orderRequertVO.CloseMode == 2)
				{
					if (orderRequertVO.TimeFlag == 1)
					{
						text = Global.TimeFlagStrArr[0];
					}
					else
					{
						text = Global.TimeFlagStrArr[1];
					}
				}
				else
				{
					if (orderRequertVO.CloseMode == 3)
					{
						text = Global.CloseModeStrArr[2];
					}
					else
					{
						text = Global.CloseModeStrArr[0];
					}
				}
			}
			string[] columnValue = new string[]
			{
				this.idMax,
				orderRequertVO.CustomerID,
				orderRequertVO.CommodityID,
				Global.BuySellStrArr[(int)orderRequertVO.BuySell],
				Global.SettleBasisStrArr[(int)orderRequertVO.SettleBasis],
				orderRequertVO.Price.ToString(),
				orderRequertVO.Quantity.ToString(),
				orderRequertVO.MarketID,
				orderRequertVO.LPrice.ToString(),
				text,
				orderRequertVO.CloseMode.ToString(),
				orderRequertVO.TimeFlag.ToString()
			};
			string text2 = string.Empty;
			if (this.SubmitPredelegateInfo != null)
			{
				text2 = this.SubmitPredelegateInfo(columns, columnValue);
			}
			if (this.RefreshCurrentTab != null && text2.Equals("true"))
			{
				this.RefreshCurrentTab(3, true);
			}
			if (this.SetFocus != null)
			{
				this.SetFocus(0);
			}
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_AddEmbeddedOrderSuccess");
			if (text2.Equals("true"))
			{
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(@string, Global.RightColor, true);
					return;
				}
			}
			else
			{
				if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
				{
					OperationManager.GetInstance().orderOperation.setLargestTN(text2, 1);
				}
			}
		}
		public void SetMaxID(int maxID)
		{
			this.idMax = maxID.ToString();
		}
	}
}
