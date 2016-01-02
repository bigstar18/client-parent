using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.DBService.ServiceManager;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class ConOrderOperation : QueryOperation
	{
		public delegate void SetCommodityIDCallBack(string commodityID);
		public delegate void SetLargestTNCallBack(string text, int colorFlag);
		private double bPrice;
		private double sPrice;
		private string MessegeTransfer = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeTransfer");
		private string MessegeConclude = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeConclude");
		private string GoodsName = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
		private string PriceIn = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
		private string NoSellPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoSellPositions");
		private string NoBuyPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoBuyPositions");
		private string InfoGoods = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoGoods");
		private ListBox lbmain;
		public ConOrderOperation.SetCommodityIDCallBack setCommodityID;
		public ConOrderOperation.SetLargestTNCallBack setLargestTN;
		public new ServiceManage serviceManage = ServiceManage.GetInstance();
		public ConOrderOperation()
		{
			this.lbmain = new ListBox();
			this.lbmain.Click += new EventHandler(this.lbmain_click);
			this.lbmain.KeyDown += new KeyEventHandler(this.lbmain_keydown);
			this.lbmain.Visible = false;
		}
		private void lbmain_click(object sender, EventArgs e)
		{
			if (this.lbmain.SelectedItems.Count == 0)
			{
				return;
			}
			string commodityID = this.lbmain.SelectedItem.ToString();
			if (this.setCommodityID != null)
			{
				this.setCommodityID(commodityID);
			}
			this.lbmain.Visible = false;
		}
		private void lbmain_keydown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left || e.KeyCode == Keys.Prior)
			{
				if (this.lbmain.SelectedIndex > 0)
				{
					this.lbmain.SelectedIndex = this.lbmain.SelectedIndex - 1;
					return;
				}
			}
			else
			{
				if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next) && this.lbmain.SelectedIndex < this.lbmain.Items.Count - 1)
				{
					this.lbmain.SelectedIndex = this.lbmain.SelectedIndex + 1;
				}
			}
		}
		public void ComboxKeyDown(KeyEventArgs e)
		{
			if (this.lbmain.Visible)
			{
				if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next || e.KeyCode == Keys.Prior)
				{
					this.lbmain_keydown(this.lbmain, e);
					e.Handled = true;
					return;
				}
				if (e.KeyCode == Keys.Return)
				{
					this.lbmain_click(this.lbmain, e);
					e.Handled = true;
				}
			}
		}
		public decimal GetBSPrice(int buysell)
		{
			decimal result = 0m;
			if (IniData.GetInstance().AutoPrice)
			{
				if (buysell == 0)
				{
					result = (decimal)this.sPrice;
				}
				else
				{
					result = (decimal)this.bPrice;
				}
			}
			return result;
		}
		public void GetNumericQtyThread(object o)
		{
			WaitCallback callBack = new WaitCallback(this.Qty);
			ThreadPool.QueueUserWorkItem(callBack, o);
		}
		private void Qty(object o)
		{
			Hashtable hashtable = (Hashtable)o;
			CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[(string)hashtable["Commodity"]];
			double price = (double)hashtable["numericPrice"];
			short b_SType = (short)hashtable["B_SType"];
			short o_LType = (short)hashtable["O_LType"];
			string customerID = (string)hashtable["tbTranc_comboTranc"];
			if (commodityInfo != null)
			{
				long tradeNum = this.CalculatLargestTradeNum(commodityInfo, price, b_SType, o_LType, customerID);
				this.NumericQtyInfo(tradeNum, commodityInfo, price, b_SType, o_LType);
			}
		}
		private long CalculatLargestTradeNum(CommodityInfo commodityInfo, double price, short B_SType, short O_LType, string CustomerID)
		{
			double num = 0.0;
			if (O_LType == 1)
			{
				FirmInfoResponseVO firmInfoResponseVO = this.serviceManage.CreateQueryInitData().QueryFundsInfo();
				double num2 = firmInfoResponseVO.RealFund + firmInfoResponseVO.ImpawnFund;
				short marginType = commodityInfo.MarginType;
				double bMargin = commodityInfo.BMargin;
				double sMargin = commodityInfo.SMargin;
				double bMargin_g = commodityInfo.BMargin_g;
				double sMargin_g = commodityInfo.SMargin_g;
				short commType = commodityInfo.CommType;
				double bOpenComm = commodityInfo.BOpenComm;
				double sOpenComm = commodityInfo.SOpenComm;
				double ctrtSize = commodityInfo.CtrtSize;
				if (num2 <= 0.0)
				{
					return 0L;
				}
				double num3 = 0.0;
				double num4 = 0.0;
				if (B_SType == 1)
				{
					if (marginType == 2)
					{
						num3 = bMargin - bMargin_g;
					}
					else
					{
						if (price > 0.0 && ctrtSize > 0.0)
						{
							num3 = price * ctrtSize * (bMargin - bMargin_g) / 100.0;
						}
					}
					if (commType == 2)
					{
						num4 = bOpenComm;
					}
					else
					{
						num4 = bOpenComm * price * ctrtSize / 100.0;
					}
				}
				else
				{
					if (B_SType == 2)
					{
						if (marginType == 2)
						{
							num3 = sMargin - sMargin_g;
						}
						else
						{
							if (price > 0.0 && ctrtSize > 0.0)
							{
								num3 = price * ctrtSize * (sMargin - sMargin_g) / 100.0;
							}
						}
						if (commType == 2)
						{
							num4 = sOpenComm;
						}
						else
						{
							num4 = sOpenComm * price * ctrtSize / 100.0;
						}
					}
				}
				if (num3 + num4 > 0.0)
				{
					num = num2 / (num3 + num4);
				}
				else
				{
					num = 99999.0;
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
			return (long)num;
		}
		private void NumericQtyInfo(long TradeNum, CommodityInfo commodityInfo, double price, short B_SType, short O_LType)
		{
			string text = string.Empty;
			int colorFlag;
			if (O_LType == 2)
			{
				if (price > 0.0 && price <= commodityInfo.SpreadUp && price >= commodityInfo.SpreadDown)
				{
					if (B_SType == 1)
					{
						if (TradeNum == 0L)
						{
							colorFlag = 1;
							text = this.NoSellPositions;
						}
						else
						{
							colorFlag = 0;
							text = this.MessegeTransfer + TradeNum;
						}
					}
					else
					{
						if (TradeNum == 0L)
						{
							colorFlag = 1;
							text = this.NoBuyPositions;
						}
						else
						{
							colorFlag = 0;
							text = this.MessegeTransfer + TradeNum;
						}
					}
				}
				else
				{
					colorFlag = 1;
				}
			}
			else
			{
				if (price > 0.0 && price <= commodityInfo.SpreadUp && price >= commodityInfo.SpreadDown)
				{
					colorFlag = 0;
					text = Global.M_ResourceManager.GetString("TradeStr_Ckkdll") + "：" + TradeNum;
				}
				else
				{
					colorFlag = 1;
				}
			}
			if (this.setLargestTN != null)
			{
				this.setLargestTN(text, colorFlag);
			}
			if (commodityInfo.MinQty != 0.0)
			{
				string message = string.Format(this.InfoGoods, commodityInfo.CommodityName, commodityInfo.MinQty);
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(message, Global.RightColor, true);
				}
			}
		}
		public long GetLargestTradeNum(string largestInfo)
		{
			long result = 0L;
			if (largestInfo != null && largestInfo.Length > 0)
			{
				int num = largestInfo.IndexOf("：");
				if (num != -1)
				{
					try
					{
						result = Tools.StrToLong(largestInfo.Substring(num + 1), 0L);
					}
					catch
					{
						result = 0L;
					}
				}
			}
			return result;
		}
	}
}
