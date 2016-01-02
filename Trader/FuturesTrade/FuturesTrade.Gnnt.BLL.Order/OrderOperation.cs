using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class OrderOperation : QueryOperation
	{
		public delegate void RefreshHQCallBack(string commodityID);
		public delegate void SetCommodityIDCallBack(string commodityID);
		public delegate void SetLargestTNCallBack(string text, int colorFlag);
		public delegate void SetButtonOrderEnableCallBack(bool enable);
		public delegate void UpdateNumericPriceCallBack(double bPrice, double sPrice);
		private string MessegeTransfer = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeTransfer");
		private string MessegeConclude = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeConclude");
		private string GoodsName = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
		private string PriceIn = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
		private string InputRightPrice = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPrice");
		private string NoSellPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoSellPositions");
		private string NoBuyPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoBuyPositions");
		private string InfoGoods = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoGoods");
		private ListBox lbmain;
		public bool ConnectHQ;
		public OrderType orderType;
		private double bPrice;
		private double sPrice;
		public bool IsChangePrice;
		public OrderOperation.RefreshHQCallBack refreshHQ;
		public OrderOperation.SetCommodityIDCallBack setCommodityID;
		public OrderOperation.SetLargestTNCallBack setLargestTN;
		public OrderOperation.SetLargestTNCallBack setLargestTN_S;
		public OrderOperation.SetButtonOrderEnableCallBack SetButtonOrderEnable;
		public OrderOperation.UpdateNumericPriceCallBack UpdateNumericPrice;
		public OrderOperation()
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
		public void ShowMinLine(string commodityID)
		{
			if (Tools.StrToBool((string)Global.HTConfig["AutoDisplayMinLine"]) && !Global.LoadFlag && !this.ConnectHQ)
			{
				Global.displayMinLine("", commodityID);
			}
			this.ConnectHQ = false;
		}
		public int GetCurrentTradeMode(string commodityesID)
		{
			int result = 0;
			if (TradeDataInfo.ht_TradeMode.Contains(commodityesID))
			{
				string a = TradeDataInfo.ht_TradeMode[commodityesID].ToString();
				bool flag = false;
				for (int i = 0; i < TradeDataInfo.FirmBreedInfoList.Count; i++)
				{
					if (TradeDataInfo.FirmBreedInfoList[i].VarietyID == TradeDataInfo.ht_Variety[commodityesID].ToString())
					{
						flag = true;
						break;
					}
					flag = false;
				}
				if ((a == "3" && flag) || (a == "4" && !flag))
				{
					result = 3;
				}
				else
				{
					if ((!flag && a == "3") || (a == "4" && flag))
					{
						result = 4;
					}
					else
					{
						if (a == "1")
						{
							result = 1;
						}
						else
						{
							if (a == "2")
							{
								result = 2;
							}
						}
					}
				}
			}
			return result;
		}
		public void ComboxTextChanged(ComboBox comboCommodity)
		{
			if (this.refreshHQ != null)
			{
				this.refreshHQ(comboCommodity.Text);
			}
			if (comboCommodity.Text == "")
			{
				this.lbmain.Visible = false;
				return;
			}
			if (!comboCommodity.Focused)
			{
				this.lbmain.Visible = false;
				return;
			}
			if (!comboCommodity.Parent.Controls.Contains(this.lbmain))
			{
				this.lbmain.Width = comboCommodity.Width;
				this.lbmain.Height = 80;
				this.lbmain.Parent = comboCommodity.Parent;
				this.lbmain.Top = comboCommodity.Top + comboCommodity.Height + 1;
				this.lbmain.Left = comboCommodity.Left;
				comboCommodity.Parent.Controls.Add(this.lbmain);
				this.lbmain.BringToFront();
			}
			int selectionStart = comboCommodity.SelectionStart;
			if (selectionStart > comboCommodity.Text.Length)
			{
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < comboCommodity.Items.Count; i++)
			{
				list.Add(comboCommodity.Items[i].ToString());
			}
			ArrayList arrayList = this.getfilllist(comboCommodity.Text.Substring(0, selectionStart), list);
			this.lbmain.Items.Clear();
			this.lbmain.Items.AddRange(arrayList.ToArray());
			if (this.lbmain.Items.Count > 0)
			{
				this.lbmain.SelectedIndex = 0;
			}
			if (!this.lbmain.Visible)
			{
				this.lbmain.Visible = true;
			}
		}
		private ArrayList getfilllist(string strvalue, List<string> commodityIDList)
		{
			ArrayList arrayList = new ArrayList();
			int count = commodityIDList.Count;
			int length = strvalue.Length;
			for (int i = 0; i < count; i++)
			{
				string text = commodityIDList[i];
				if (text.Length >= length && string.Compare(text.Substring(0, length), strvalue, true) == 0)
				{
					arrayList.Add(text);
				}
			}
			return arrayList;
		}
		public decimal GetCommoditySpread(string commodityID)
		{
			decimal result = 0m;
			try
			{
				CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[commodityID];
				if (commodityInfo != null)
				{
					string message = string.Concat(new object[]
					{
						this.GoodsName,
						commodityInfo.CommodityName,
						"  ",
						this.PriceIn,
						commodityInfo.SpreadDown,
						" – ",
						commodityInfo.SpreadUp
					});
					if (Global.StatusInfoFill != null)
					{
						Global.StatusInfoFill(message, Global.RightColor, true);
					}
					result = decimal.Parse(commodityInfo.Spread.ToString());
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + "    " + ex.Message);
			}
			return result;
		}
		public int GetDecimalPlaces(decimal spread)
		{
			int result = 0;
			try
			{
				string[] array = spread.ToString().Split(new char[]
				{
					'.'
				});
				if (array.Length == 1)
				{
					result = 0;
				}
				else
				{
					if (array.Length == 2)
					{
						if (array[1].Length == 1)
						{
							result = 1;
						}
						else
						{
							if (array[1].Length == 2)
							{
								result = 2;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + "    " + ex.Message);
			}
			return result;
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
			if (IniData.GetInstance().AutoPrice && !this.IsChangePrice)
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
			try
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
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "最大可订立量查询错误：" + ex.Message);
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
				List<HoldingInfo> holdingInfoList = TradeDataInfo.holdingInfoList;
				int i = 0;
				while (i < holdingInfoList.Count)
				{
					HoldingInfo holdingInfo = holdingInfoList[i];
					if (holdingInfo.CustomerID.Equals(CustomerID) && holdingInfo.CommodityID.Equals(commodityInfo.CommodityID))
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
					text = this.InputRightPrice;
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
					text = this.InputRightPrice;
				}
			}
			if (this.setLargestTN_S != null)
			{
				if (this.setLargestTN != null && B_SType == 1)
				{
					this.setLargestTN(text, colorFlag);
				}
				if (B_SType == 2)
				{
					this.setLargestTN_S(text, colorFlag);
				}
			}
			else
			{
				if (this.setLargestTN != null)
				{
					this.setLargestTN(text, colorFlag);
				}
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
		public void SetListBoxVisible(bool visible)
		{
			this.lbmain.Visible = visible;
		}
		public void UpdatePrice(double _bPrice, double _sPrice)
		{
			this.bPrice = _bPrice;
			this.sPrice = _sPrice;
			if (!this.IsChangePrice && this.UpdateNumericPrice != null)
			{
				this.UpdateNumericPrice(this.bPrice, this.sPrice);
			}
		}
		public decimal GetPriceWithTax(string commodityId, decimal price)
		{
			decimal result;
			try
			{
				CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[commodityId];
				decimal d = (decimal)commodityInfo.TaxRate;
				decimal num = price * ++d;
				result = num;
			}
			catch
			{
				result = price;
			}
			return result;
		}
	}
}
