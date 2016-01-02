using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.Threading;
using System.Windows.Forms;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class SubmitConOrderOperation : QueryOperation
	{
		public delegate void SetFocusCallBack(short flag);
		public delegate void ConOrderMessageCallBack(long retCode, string retMessage);
		private string CommodityID = Global.M_ResourceManager.GetString("TradeStr_ConditionCommodityID");
		private string ConditionType = Global.M_ResourceManager.GetString("TradeStr_ConditionType");
		private string ConditionSign = Global.M_ResourceManager.GetString("TradeStr_ConditionSign");
		private string ConditionPrice = Global.M_ResourceManager.GetString("TradeStr_ConditionPrice");
		private string MatureTime = Global.M_ResourceManager.GetString("TradeStr_MatureTime");
		private string TrustTypeId = Global.M_ResourceManager.GetString("TradeStr_TrustTypeId");
		private string TrustType = Global.M_ResourceManager.GetString("TradeStr_TrustType");
		private string TrustPrice = Global.M_ResourceManager.GetString("TradeStr_TrustPrice");
		private string TrustNum = Global.M_ResourceManager.GetString("TradeStr_TrustNum");
		private string SureOrder = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_SureSubmitTrust");
		private string OrderInfo = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_TrustMessage");
		public SubmitConOrderOperation.SetFocusCallBack SetFocus;
		public SubmitConOrderOperation.ConOrderMessageCallBack ConOrderMessage;
		public void ButtonConOrderComm(SubmitConOrderInfo orderInfo)
		{
			if (orderInfo != null)
			{
				if (!TradeDataInfo.CommodityHashtable.ContainsKey(orderInfo.commodityID))
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_NoExistGoodsId");
					if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
					{
						OperationManager.GetInstance().conOrderOperation.setLargestTN(@string, 1);
					}
					if (this.SetFocus != null)
					{
						this.SetFocus(0);
						return;
					}
				}
				else
				{
					CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[orderInfo.commodityID];
					if (orderInfo.price <= 0.0)
					{
						string string2 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_InputTrustPrice");
						if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
						{
							OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Format(string2, commodityInfo.SpreadUp, commodityInfo.SpreadDown), 1);
						}
						if (this.SetFocus != null)
						{
							this.SetFocus(1);
							return;
						}
					}
					else
					{
						if (orderInfo.conprice <= 0.0)
						{
							string string3 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_InputConditionPrice");
							if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
							{
								OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Format(string3, commodityInfo.SpreadUp, commodityInfo.SpreadDown), 1);
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
								string string4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorPrice");
								if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
								{
									OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Concat(new object[]
									{
										string4,
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
									string string5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
									if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
									{
										OperationManager.GetInstance().conOrderOperation.setLargestTN(string5, 1);
									}
									if (this.SetFocus != null)
									{
										this.SetFocus(2);
										return;
									}
								}
								else
								{
									if (Convert.ToDouble(orderInfo.qty) % Convert.ToDouble(commodityInfo.MinQty) != 0.0)
									{
										string string6 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsError");
										if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
										{
											OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Concat(new object[]
											{
												string6,
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
									else
									{
										this.SubmitConOrderInfo(orderInfo);
									}
								}
							}
						}
					}
				}
			}
		}
		private void SubmitConOrderInfo(SubmitConOrderInfo orderInfo)
		{
			ConditionOrderRequestVO conditionOrderRequestVO = new ConditionOrderRequestVO();
			conditionOrderRequestVO.UserID = Global.UserID;
			conditionOrderRequestVO.FirmID = Global.FirmID;
			conditionOrderRequestVO.Concommodity = orderInfo.commodityID;
			conditionOrderRequestVO.CommodityID = orderInfo.commodityID;
			conditionOrderRequestVO.BuySell = orderInfo.B_SType;
			conditionOrderRequestVO.SettleBasis = orderInfo.O_LType;
			conditionOrderRequestVO.Price = Convert.ToDouble(orderInfo.price);
			conditionOrderRequestVO.Quantity = (long)Convert.ToInt32(orderInfo.qty);
			conditionOrderRequestVO.TraderID = Global.UserID;
			conditionOrderRequestVO.ConPrice = Convert.ToDouble(orderInfo.conprice);
			conditionOrderRequestVO.ConExpire = orderInfo.datetime;
			conditionOrderRequestVO.Conoperator = orderInfo.conoperator;
			conditionOrderRequestVO.Contype = orderInfo.contype;
			if (IniData.GetInstance().ShowDialog)
			{
				string text = string.Empty;
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					this.CommodityID,
					":",
					conditionOrderRequestVO.Concommodity,
					"\r\n",
					this.ConditionType,
					":",
					Global.ConditionTypeStrArr[(int)conditionOrderRequestVO.Contype],
					"\r\n",
					this.ConditionSign,
					":",
					Global.ConditionOperatorStrArr[(int)(conditionOrderRequestVO.Conoperator + 2)],
					"\r\n",
					this.ConditionPrice,
					":",
					conditionOrderRequestVO.ConPrice,
					"\r\n",
					this.MatureTime,
					":",
					conditionOrderRequestVO.ConExpire,
					"\r\n\n********************\r\n",
					this.TrustTypeId,
					":",
					conditionOrderRequestVO.CommodityID,
					"\r\n",
					this.TrustType,
					":",
					Global.BuySellStrArr[(int)conditionOrderRequestVO.BuySell],
					Global.SettleBasisStrArr[(int)conditionOrderRequestVO.SettleBasis],
					"\r\n",
					this.TrustPrice,
					":",
					conditionOrderRequestVO.Price,
					"\r\n",
					this.TrustNum,
					":",
					conditionOrderRequestVO.Quantity,
					"\r\n********************\r\n",
					this.SureOrder
				});
				DialogResult dialogResult = MessageBox.Show(text, this.OrderInfo, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dialogResult == DialogResult.OK)
				{
					this.SubmitConOrderThread(conditionOrderRequestVO);
					return;
				}
			}
			else
			{
				this.SubmitConOrderThread(conditionOrderRequestVO);
			}
		}
		public void SubmitConOrderThread(object orderRequertVO)
		{
			WaitCallback callBack = new WaitCallback(this.SubmitConOrder);
			ThreadPool.QueueUserWorkItem(callBack, orderRequertVO);
		}
		public void SubmitConOrder(object _orderRequestVO)
		{
			ConditionOrderRequestVO conditionOrder = (ConditionOrderRequestVO)_orderRequestVO;
			ConditionOrderResponseVO conditionOrderResponseVO = this.serviceManage.CreateConditionOrder().SetConditionOrder(conditionOrder);
			if (this.RefreshCurrentTab != null && conditionOrderResponseVO.RetCode == 0L)
			{
				this.RefreshCurrentTab(2, true);
			}
			if (this.ConOrderMessage != null && conditionOrderResponseVO != null)
			{
				this.ConOrderMessage(conditionOrderResponseVO.RetCode, conditionOrderResponseVO.RetMessage);
			}
		}
	}
}
