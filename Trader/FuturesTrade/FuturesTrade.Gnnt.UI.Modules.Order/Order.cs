using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Order;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using FuturesTrade.Gnnt.UI.Modules;
namespace FuturesTrade.Gnnt.UI.Modules.Order
{
	public class Order : UserControl
	{
		private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private delegate void UpdateNumericPriceCallBack(double bPrice, double sPrice);
		private string OneTwoInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OneTwoInfo");
		private OperationManager operationManager = OperationManager.GetInstance();
		private Order.PromptLargestTradeNumCallBack PromptLargestTradeNum;
		private Order.OrderMessageInfoCallBack OrderMessageInfo;
		private Order.UpdateNumericPriceCallBack UpdatePrice;
		private byte BtnFlag;
		private bool buttonClick;
		private IContainer components;
		private GroupBox groupBoxOrder;
		private ComboBox comboTranc;
		private TextBox tbTranc;
		private Label labelLargestTN;
		private ComboBox comboCommodity;
		private Button buttonAddPre;
		public Button buttonOrder;
		private NumericUpDown numericQty;
		private NumericUpDown numericPrice;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private GroupBox groupBoxB_S;
		private RadioButton radioS;
		private RadioButton radioB;
		private GroupBox gbCloseMode;
		private RadioButton rbCloseH;
		private RadioButton rbCloseT;
		private GroupBox groupBoxO_L;
		private RadioButton radioL;
		private RadioButton radioO;
		private NumericUpDown numericLPrice;
		private Label labelLPrice;
		private ComboBox comboMarKet;
		private Label labTrancCode;
		private Label labelMarKet;
		private Label lblPriceWithTax;
		private Label lblTax;
		public Order()
		{
			this.InitializeComponent();
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.operationManager.SetTransactionEvent += new OperationManager.SetTransactionCallBack(this.SetTransactionList);
			this.operationManager.orderOperation.UpdateNumericPrice = new OrderOperation.UpdateNumericPriceCallBack(this.UpdateNumericPrice);
			this.operationManager.TransferInfo = new OperationManager.TransferInfoCallBack(this.SetPriceQty);
			Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			Global.SetCommoditySelectIndex = new Global.SetCommoditySelectIndexCallBack(this.SetCommoditySelectIndex);
			Global.SetDoubleClickOrderInfo = new Global.SetDoubleClickOrderInfoCallBack(this.SetDoubleClickOrderInfo);
			this.CreateHandle();
		}
		private void SetRadioEnable(int currentTradeMode)
		{
			switch (currentTradeMode)
			{
			case 0:
				this.radioO.Enabled = true;
				this.radioL.Enabled = true;
				this.radioS.Enabled = true;
				this.radioB.Enabled = true;
				return;
			case 1:
				if (this.radioB.Checked)
				{
					this.radioO.Enabled = false;
					this.radioL.Checked = true;
					return;
				}
				this.radioL.Enabled = false;
				this.radioO.Checked = true;
				return;
			case 2:
				if (this.radioB.Checked)
				{
					this.radioL.Enabled = false;
					this.radioO.Checked = true;
					return;
				}
				this.radioO.Enabled = false;
				this.radioL.Checked = true;
				return;
			case 3:
				this.radioS.Enabled = false;
				this.radioB.Checked = true;
				this.radioL.Enabled = false;
				this.radioO.Checked = true;
				return;
			case 4:
				this.radioB.Enabled = false;
				this.radioS.Checked = true;
				this.radioL.Enabled = false;
				this.radioO.Checked = true;
				return;
			default:
				return;
			}
		}
		private void comboCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numericPrice.Value = 0m;
			this.numericQty.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.labelLargestTN.Text = "";
			this.operationManager.orderOperation.SetListBoxVisible(false);
			this.operationManager.orderOperation.ShowMinLine(this.comboCommodity.Text);
			int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
			this.SetRadioEnable(currentTradeMode);
		}
		private void comboCommodity_TextChanged(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.IsChangePrice = false;
			this.operationManager.orderOperation.ComboxTextChanged(this.comboCommodity);
			decimal commoditySpread = this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
			this.numericPrice.Increment = commoditySpread;
			this.numericLPrice.Increment = commoditySpread;
			int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
			this.numericPrice.DecimalPlaces = decimalPlaces;
			this.numericLPrice.DecimalPlaces = decimalPlaces;
		}
		private void comboCommodity_KeyDown(object sender, KeyEventArgs e)
		{
			this.operationManager.orderOperation.ComboxKeyDown(e);
		}
		private void comboCommodity_DropDown(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.SetListBoxVisible(false);
		}
		private void radioB_Enter(object sender, EventArgs e)
		{
			string message = string.Format(this.OneTwoInfo, this.operationManager.StrBuy, this.operationManager.StrSale);
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(message, Global.RightColor, true);
			}
		}
		private void radioB_CheckedChanged(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.IsChangePrice = false;
			int buysell = 0;
			if (this.radioS.Checked)
			{
				buysell = 1;
			}
			decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
			this.numericPrice.Value = bSPrice;
		}
		private void radioS_Enter(object sender, EventArgs e)
		{
			string message = string.Format(this.OneTwoInfo, this.operationManager.StrBuy, this.operationManager.StrSale);
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(message, Global.RightColor, true);
			}
		}
		private void radioS_CheckedChanged(object sender, EventArgs e)
		{
			int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
			this.SetRadioEnable(currentTradeMode);
		}
		private void radioO_Enter(object sender, EventArgs e)
		{
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(Global.M_ResourceManager.GetString("TradeStr_Tsxx"), Global.RightColor, true);
			}
		}
		private void radioO_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioO.Checked)
			{
				this.labelLPrice.Visible = false;
				this.numericLPrice.Visible = false;
				this.gbCloseMode.Visible = false;
			}
		}
		private void radioL_CheckedChanged(object sender, EventArgs e)
		{
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(Global.M_ResourceManager.GetString("TradeStr_Tsxx"), Global.RightColor, true);
			}
			if (this.radioL.Checked)
			{
				switch (IniData.GetInstance().CloseMode)
				{
				case 1:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = false;
					return;
				case 2:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = true;
					return;
				case 3:
					this.labelLPrice.Visible = true;
					this.numericLPrice.Visible = true;
					this.gbCloseMode.Visible = false;
					return;
				default:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = false;
					break;
				}
			}
		}
		private void numericPrice_Enter(object sender, EventArgs e)
		{
			this.numericPrice.Select(0, this.numericPrice.Text.Length);
			this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
		}
		private void numericPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericQty_Enter(object sender, EventArgs e)
		{
			this.numericQty.Select(0, this.numericQty.Value.ToString().Length);
			this.labelLargestTN.ForeColor = Global.LightColor;
			short num = 1;
			if (this.radioS.Checked)
			{
				num = 2;
			}
			short num2 = 1;
			if (this.radioL.Checked)
			{
				num2 = 2;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Commodity", this.comboCommodity.Text);
			hashtable.Add("B_SType", num);
			hashtable.Add("O_LType", num2);
			hashtable.Add("numericPrice", Convert.ToDouble(this.numericPrice.Value));
			hashtable.Add("tbTranc_comboTranc", this.tbTranc.Text + this.comboTranc.Text);
			this.operationManager.orderOperation.GetNumericQtyThread(hashtable);
		}
		private void numericQty_KeyUp(object sender, KeyEventArgs e)
		{
			Global.QtyKeyUp(sender, e);
		}
		private void gbCloseMode_Enter(object sender, EventArgs e)
		{
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(string.Format(this.OneTwoInfo, Global.TimeFlagStrArr[0], Global.TimeFlagStrArr[1]), Global.RightColor, true);
			}
		}
		private void numericLPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
		{
			this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
		}
		private void labelLargestTN_Click(object sender, EventArgs e)
		{
			long largestTradeNum = this.operationManager.orderOperation.GetLargestTradeNum(this.labelLargestTN.Text);
			this.numericQty.Value = Global.ToDecimal(largestTradeNum.ToString());
		}
		private void comboTranc_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			this.BtnFlag = 0;
			this.SubmintOrder();
		}
		private void buttonAddPre_Click(object sender, EventArgs e)
		{
			this.BtnFlag = 1;
			this.SubmintOrder();
		}
		private void SubmintOrder()
		{
			SubmitOrderInfo submitOrderInfo = new SubmitOrderInfo();
			submitOrderInfo.customerID = this.tbTranc.Text + this.comboTranc.Text;
			submitOrderInfo.commodityID = this.comboCommodity.Text;
			if (this.radioS.Checked)
			{
				submitOrderInfo.B_SType = 2;
			}
			if (this.radioL.Checked)
			{
				submitOrderInfo.O_LType = 2;
			}
            submitOrderInfo.price = ToolsLibrary.util.Tools.StrToDouble(this.numericPrice.Value.ToString(), 0.0);
            submitOrderInfo.qty = ToolsLibrary.util.Tools.StrToInt(this.numericQty.Value.ToString(), 0);
			if (submitOrderInfo.O_LType == 2)
			{
				if (IniData.GetInstance().CloseMode == 2)
				{
					submitOrderInfo.closeMode = 2;
					if (this.rbCloseT.Checked)
					{
						submitOrderInfo.timeFlag = 1;
					}
					else
					{
						submitOrderInfo.closeMode = 1;
					}
				}
				else
				{
					if (IniData.GetInstance().CloseMode == 3)
					{
						submitOrderInfo.closeMode = 3;
                        submitOrderInfo.lPrice = ToolsLibrary.util.Tools.StrToDouble(this.numericLPrice.Value.ToString(), 0.0);
					}
					else
					{
						submitOrderInfo.closeMode = 1;
					}
				}
			}
			OperationManager.GetInstance().orderOperation.orderType = OrderType.Order;
			this.operationManager.submitOrderOperation.ButtonOrderComm(submitOrderInfo, this.BtnFlag);
		}
		private void SetCommodityID(string commodityID)
		{
			this.comboCommodity.Text = commodityID;
			int num = this.comboCommodity.FindStringExact(commodityID);
			if (num != -1)
			{
				this.comboCommodity.SelectedIndex = num;
			}
		}
		private void SetLargestTNInfo(string text, int colorFlag)
		{
			try
			{
				this.PromptLargestTradeNum = new Order.PromptLargestTradeNumCallBack(this.LargestTNInfo);
				this.HandleCreated();
				base.Invoke(this.PromptLargestTradeNum, new object[]
				{
					text,
					colorFlag
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void LargestTNInfo(string text, int colorFlag)
		{
			if (colorFlag == 0)
			{
				this.labelLargestTN.ForeColor = Global.LightColor;
			}
			else
			{
				if (colorFlag == 1)
				{
					this.labelLargestTN.ForeColor = Global.ErrorColor;
				}
			}
			this.labelLargestTN.Text = text;
		}
		private void SetFouce(short flag)
		{
			this.labelLargestTN.Text = "";
			if (flag == 0)
			{
				this.comboCommodity.Focus();
				return;
			}
			if (flag == 1)
			{
				this.numericPrice.Focus();
				return;
			}
			if (flag == 2)
			{
				this.numericQty.Focus();
			}
		}
		private void OrderMessage(long retCode, string retMessage)
		{
			try
			{
				if (OperationManager.GetInstance().orderOperation.orderType == OrderType.Order)
				{
					this.OrderMessageInfo = new Order.OrderMessageInfoCallBack(this.OrderInfoMessage);
					this.HandleCreated();
					base.Invoke(this.OrderMessageInfo, new object[]
					{
						retCode,
						retMessage
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void OrderInfoMessage(long retCode, string retMessage)
		{
			this.comboCommodity.Focus();
			if (IniData.GetInstance().ClearData)
			{
				if (this.numericPrice.Enabled)
				{
					this.numericPrice.Value = 0m;
				}
				this.numericQty.Text = "";
				this.numericLPrice.Value = 0m;
			}
			if (retCode == 0L)
			{
				this.operationManager.orderOperation.IsChangePrice = false;
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(this.operationManager.SussceOrder, Global.RightColor, true);
					return;
				}
			}
			else
			{
				if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
				{
					MessageBox.Show(retMessage, this.operationManager.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (this.operationManager.orderOperation.setLargestTN != null)
				{
					this.operationManager.orderOperation.setLargestTN(retMessage, 1);
				}
			}
		}
		private void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboCommodity.Items.Clear();
			foreach (string current in commodityIDList)
			{
				if (current != this.operationManager.StrAll)
				{
					this.comboCommodity.Items.Add(current);
				}
			}
			this.comboCommodity.SelectedIndex = 0;
			this.comboCommodity.Focus();
		}
		private void SetTransactionList(List<string> transactionList)
		{
			this.comboTranc.Items.Clear();
			if (transactionList.Contains(this.operationManager.StrAll))
			{
				transactionList.Remove(this.operationManager.StrAll);
			}
			foreach (string current in transactionList)
			{
				this.comboTranc.Items.Add(current.Substring(current.Length - 2));
			}
			if (this.comboTranc.Items.Count < 2)
			{
				this.comboTranc.Visible = false;
				this.tbTranc.Size = new Size(this.comboCommodity.Width, 20);
			}
			this.tbTranc.Text = Global.FirmID;
			this.comboTranc.SelectedIndex = 0;
		}
		private void Order_Load(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.setCommodityID = new OrderOperation.SetCommodityIDCallBack(this.SetCommodityID);
			this.operationManager.orderOperation.setLargestTN = new OrderOperation.SetLargestTNCallBack(this.SetLargestTNInfo);
			this.operationManager.submitOrderOperation.SetFocus = new SubmitOrderOperation.SetFocusCallBack(this.SetFouce);
			this.operationManager.submitOrderOperation.OrderMessage = new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage);
			this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
			this.tbTranc.Text = Global.FirmID;
			this.SetControlText();
		}
		private void SetControlText()
		{
			this.groupBoxOrder.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder");
			this.labTrancCode.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelMarKet.Text = Global.M_ResourceManager.GetString("TradeStr_LabelMarKet");
			this.radioB.Text = Global.M_ResourceManager.GetString("TradeStr_RadioB");
			this.radioS.Text = Global.M_ResourceManager.GetString("TradeStr_RadioS");
			this.radioO.Text = Global.M_ResourceManager.GetString("TradeStr_RadioO");
			this.radioL.Text = Global.M_ResourceManager.GetString("TradeStr_RadioL");
			this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice");
			this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
            if (Global.HTConfig.ContainsKey("DisplaySwitch") && ToolsLibrary.util.Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
			{
				if (IniData.GetInstance().AutoPrice)
				{
					this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice1");
					this.numericPrice.Enabled = false;
				}
				else
				{
					this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice2");
					this.numericPrice.Enabled = true;
				}
			}
			this.rbCloseT.Text = Global.TimeFlagStrArr[0];
			this.rbCloseH.Text = Global.TimeFlagStrArr[1];
			this.buttonOrder.TextAlign = ContentAlignment.TopCenter;
			this.buttonAddPre.TextAlign = ContentAlignment.TopCenter;
			this.buttonOrder.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonOrder");
			this.buttonAddPre.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonAddPre");
		}
		private void SetButtonOrderEnable(bool enable)
		{
			this.buttonOrder.Enabled = enable;
			this.buttonAddPre.Enabled = enable;
		}
		private void SetFocus(Keys e)
		{
			if (e == Keys.Right)
			{
				if (this.radioB.Focused)
				{
					this.radioS.Focus();
					return;
				}
				if (this.radioO.Focused)
				{
					this.radioL.Focus();
					return;
				}
				if (this.rbCloseT.Focused)
				{
					this.rbCloseH.Focus();
					return;
				}
				if (this.buttonOrder.Focused)
				{
					this.buttonAddPre.Focus();
					return;
				}
				if (this.numericPrice.Focused)
				{
					this.SetPosition(this.numericPrice, Keys.Right);
					return;
				}
			}
			else
			{
				if (e == Keys.Left)
				{
					if (this.radioS.Focused)
					{
						this.radioB.Focus();
						return;
					}
					if (this.radioL.Focused)
					{
						this.radioO.Focus();
						return;
					}
					if (this.rbCloseH.Focused)
					{
						this.rbCloseT.Focus();
						return;
					}
					if (this.buttonAddPre.Focused)
					{
						this.buttonOrder.Focus();
						return;
					}
					if (this.numericPrice.Focused)
					{
						this.SetPosition(this.numericPrice, Keys.Left);
						return;
					}
				}
				else
				{
					if (e == Keys.Up)
					{
						if (this.comboCommodity.Focused)
						{
							this.buttonOrder.Focus();
							return;
						}
						if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
						{
							if (this.numericLPrice.Visible)
							{
								this.numericLPrice.Focus();
								return;
							}
							if (!this.gbCloseMode.Visible)
							{
								this.numericQty.Focus();
								return;
							}
							if (this.rbCloseT.Checked)
							{
								this.rbCloseT.Focus();
								return;
							}
							this.rbCloseH.Focus();
							return;
						}
						else
						{
							if (this.numericLPrice.Focused || this.rbCloseT.Focused || this.rbCloseH.Focused)
							{
								this.numericQty.Focus();
								return;
							}
							if (this.numericQty.Focused)
							{
								this.numericPrice.Focus();
								return;
							}
							if (this.numericPrice.Focused)
							{
								if (this.radioO.Checked)
								{
									this.radioO.Focus();
									return;
								}
								this.radioL.Focus();
								return;
							}
							else
							{
								if (this.radioO.Focused || this.radioL.Focused)
								{
									if (this.radioB.Checked)
									{
										this.radioB.Focus();
										return;
									}
									this.radioS.Focus();
									return;
								}
								else
								{
									if (this.radioB.Focused || this.radioS.Focused)
									{
										this.comboCommodity.Focus();
										return;
									}
								}
							}
						}
					}
					else
					{
						if (e == Keys.Down || e == Keys.Return)
						{
							if (this.comboCommodity.Focused)
							{
								if (this.radioB.Checked)
								{
									this.radioB.Focus();
									return;
								}
								this.radioS.Focus();
								return;
							}
							else
							{
								if (this.radioB.Focused || this.radioS.Focused)
								{
									if (this.radioO.Checked)
									{
										this.radioO.Focus();
										return;
									}
									this.radioL.Focus();
									return;
								}
								else
								{
									if (this.radioO.Focused || this.radioL.Focused)
									{
										this.numericPrice.Focus();
										return;
									}
									if (this.numericPrice.Focused)
									{
										this.numericQty.Focus();
										return;
									}
									if (this.numericQty.Focused)
									{
										if (this.numericLPrice.Visible)
										{
											this.numericLPrice.Focus();
											return;
										}
										if (!this.gbCloseMode.Visible)
										{
											this.buttonOrder.Focus();
											return;
										}
										if (this.rbCloseT.Checked)
										{
											this.rbCloseT.Focus();
											return;
										}
										this.rbCloseH.Focus();
										return;
									}
									else
									{
										if (this.numericLPrice.Focused || this.rbCloseT.Focused || this.rbCloseH.Focused)
										{
											this.buttonOrder.Focus();
											return;
										}
										if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
										{
											this.comboCommodity.Focus();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private void UpdateNumericPrice(double bPrice, double sPrice)
		{
			try
			{
				this.UpdatePrice = new Order.UpdateNumericPriceCallBack(this.SetNumericPrice);
				this.HandleCreated();
				base.Invoke(this.UpdatePrice, new object[]
				{
					bPrice,
					sPrice
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void SetNumericPrice(double bPrice, double sPrice)
		{
			if (this.radioB.Checked)
			{
				this.numericPrice.Value = (decimal)sPrice;
				return;
			}
			if (this.radioS.Checked)
			{
				this.numericPrice.Value = (decimal)bPrice;
			}
		}
		private void SetPriceQty(string info, byte priceQtyFlag)
		{
			if (!string.IsNullOrEmpty(info))
			{
				switch (priceQtyFlag)
				{
				case 0:
					try
					{
						this.numericPrice.Value = decimal.Parse(info);
						return;
					}
					catch (Exception)
					{
						this.numericPrice.Value = 0m;
						return;
					}
					break;
				case 1:
					break;
				default:
					return;
				}
				try
				{
					this.numericQty.Value = decimal.Parse(info);
				}
				catch (Exception)
				{
					this.numericQty.Value = 0m;
				}
			}
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void SetOrderInfo(string commodityID, double buyPrice, double sellPrice)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = commodityID.IndexOf("_");
			if (num != -1)
			{
				text = commodityID.Substring(0, num);
				text2 = commodityID.Substring(num + 1);
			}
			else
			{
				text2 = commodityID;
			}
			if (text2 != this.comboCommodity.Text)
			{
				this.operationManager.orderOperation.ConnectHQ = true;
			}
			if (Global.MarketHT.Count == 1)
			{
				int i = 0;
				while (i < this.comboCommodity.Items.Count)
				{
					if (text2.Equals(this.comboCommodity.Items[i].ToString()))
					{
						this.comboCommodity.SelectedIndex = i;
						if (this.radioB.Checked)
						{
							this.numericPrice.Value = (decimal)sellPrice;
							return;
						}
						this.numericPrice.Value = (decimal)buyPrice;
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			for (int j = 0; j < this.comboMarKet.Items.Count; j++)
			{
				AddValue addValue = (AddValue)this.comboMarKet.Items[j];
				if (text.Equals(addValue.Value))
				{
					this.comboMarKet.SelectedIndex = j;
					break;
				}
			}
			int k = 0;
			while (k < this.comboCommodity.Items.Count)
			{
				if (text2.Equals(this.comboCommodity.Items[k].ToString()))
				{
					this.comboCommodity.SelectedIndex = k;
					if (this.radioB.Checked)
					{
						this.numericPrice.Value = (decimal)sellPrice;
						return;
					}
					this.numericPrice.Value = (decimal)buyPrice;
					return;
				}
				else
				{
					k++;
				}
			}
		}
		private void numericPrice_ValueChanged(object sender, EventArgs e)
		{
			int buysell = 0;
			if (this.radioS.Checked)
			{
				buysell = 1;
			}
			decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
			this.lblPriceWithTax.Text = this.operationManager.orderOperation.GetPriceWithTax(this.comboCommodity.Text, this.numericPrice.Value).ToString();
			if (this.numericPrice.Value != bSPrice)
			{
				this.operationManager.orderOperation.IsChangePrice = true;
				return;
			}
			this.operationManager.orderOperation.IsChangePrice = false;
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (this.buttonClick)
			{
				this.buttonClick = false;
				return false;
			}
			if (keyData == Keys.D1 || keyData == Keys.NumPad1)
			{
				if (this.radioB.Focused || this.radioS.Focused)
				{
					this.radioB.Checked = true;
					this.radioB.Focus();
				}
				if (this.radioO.Focused || this.radioL.Focused)
				{
					this.radioO.Checked = true;
					this.radioO.Focus();
				}
				if (this.rbCloseT.Focused || this.rbCloseH.Focused)
				{
					this.rbCloseT.Checked = true;
					this.rbCloseT.Focus();
				}
			}
			else
			{
				if (keyData == Keys.D2 || keyData == Keys.NumPad2)
				{
					if (this.radioB.Focused || this.radioS.Focused)
					{
						this.radioS.Checked = true;
						this.radioS.Focus();
					}
					if (this.radioO.Focused || this.radioL.Focused)
					{
						this.radioL.Checked = true;
						this.radioL.Focus();
					}
					if (this.rbCloseT.Focused || this.rbCloseH.Focused)
					{
						this.rbCloseH.Checked = true;
						this.rbCloseH.Focus();
					}
				}
			}
			if (!IniData.GetInstance().UpDownFocus)
			{
				return false;
			}
			if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right || (keyData == Keys.Return && !this.buttonOrder.Focused && !this.buttonAddPre.Focused))
			{
				this.SetFocus(keyData);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void SetPosition(NumericUpDown NudPriceB, Keys keyData)
		{
			try
			{
				if (NudPriceB != null)
				{
					TextBox textBox = (TextBox)NudPriceB.Controls[1];
					int num;
					if (textBox.SelectedText == NudPriceB.Text)
					{
						num = NudPriceB.Text.Length;
					}
					else
					{
						num = textBox.SelectionStart;
					}
					if (keyData == Keys.Right)
					{
						num++;
						if (num > NudPriceB.Text.Length)
						{
							num = NudPriceB.Text.Length;
						}
					}
					else
					{
						if (keyData == Keys.Left)
						{
							num--;
							if (num < 0)
							{
								num = 0;
							}
						}
					}
					NudPriceB.Select(num, 0);
				}
			}
			catch (Exception)
			{
			}
		}
		private bool SetCommoditySelectIndex(string marketID, string commodityID)
		{
			bool result = false;
			if (Global.MarketHT.Count > 1)
			{
				for (int i = 0; i < this.comboMarKet.Items.Count; i++)
				{
					if (this.comboMarKet.Items[i].ToString() == marketID)
					{
						this.comboMarKet.SelectedIndex = i;
						break;
					}
				}
				for (int j = 0; j < this.comboCommodity.Items.Count; j++)
				{
					if (this.comboCommodity.Items[j].ToString() == commodityID)
					{
						this.comboCommodity.SelectedIndex = j;
						result = true;
						break;
					}
				}
			}
			else
			{
				for (int k = 0; k < this.comboCommodity.Items.Count; k++)
				{
					if (this.comboCommodity.Items[k].ToString() == commodityID)
					{
						this.comboCommodity.SelectedIndex = k;
						result = true;
						break;
					}
				}
			}
			return result;
		}
		private void SetDoubleClickOrderInfo(double price, double Lprice, int qty, short buysell, short ordertype)
		{
			try
			{
				if (buysell == 0)
				{
					this.radioB.Checked = true;
				}
				else
				{
					this.radioS.Checked = true;
				}
				if (ordertype == 0)
				{
					this.radioO.Checked = true;
				}
				else
				{
					this.radioL.Checked = true;
				}
				if (Lprice != 0.0)
				{
					this.numericLPrice.Value = decimal.Parse(Lprice.ToString());
				}
				if (price != 0.0)
				{
					this.numericPrice.Value = decimal.Parse(price.ToString());
				}
				this.numericQty.Value = qty;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "SetDoubleClickOrderInfo异常：" + ex.Message);
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.groupBoxOrder = new GroupBox();
			this.lblPriceWithTax = new Label();
			this.lblTax = new Label();
			this.comboTranc = new ComboBox();
			this.tbTranc = new TextBox();
			this.labelLargestTN = new Label();
			this.comboCommodity = new ComboBox();
			this.buttonAddPre = new Button();
			this.buttonOrder = new Button();
			this.numericQty = new NumericUpDown();
			this.numericPrice = new NumericUpDown();
			this.labQty = new Label();
			this.labPrice = new Label();
			this.labComCode = new Label();
			this.groupBoxB_S = new GroupBox();
			this.radioS = new RadioButton();
			this.radioB = new RadioButton();
			this.gbCloseMode = new GroupBox();
			this.rbCloseH = new RadioButton();
			this.rbCloseT = new RadioButton();
			this.groupBoxO_L = new GroupBox();
			this.radioL = new RadioButton();
			this.radioO = new RadioButton();
			this.numericLPrice = new NumericUpDown();
			this.labelLPrice = new Label();
			this.comboMarKet = new ComboBox();
			this.labTrancCode = new Label();
			this.labelMarKet = new Label();
			this.groupBoxOrder.SuspendLayout();
			((ISupportInitialize)this.numericQty).BeginInit();
			((ISupportInitialize)this.numericPrice).BeginInit();
			this.groupBoxB_S.SuspendLayout();
			this.gbCloseMode.SuspendLayout();
			this.groupBoxO_L.SuspendLayout();
			((ISupportInitialize)this.numericLPrice).BeginInit();
			base.SuspendLayout();
			this.groupBoxOrder.BackColor = Color.Transparent;
			this.groupBoxOrder.BackgroundImageLayout = ImageLayout.Stretch;
			this.groupBoxOrder.Controls.Add(this.lblPriceWithTax);
			this.groupBoxOrder.Controls.Add(this.lblTax);
			this.groupBoxOrder.Controls.Add(this.comboTranc);
			this.groupBoxOrder.Controls.Add(this.tbTranc);
			this.groupBoxOrder.Controls.Add(this.labelLargestTN);
			this.groupBoxOrder.Controls.Add(this.comboCommodity);
			this.groupBoxOrder.Controls.Add(this.buttonAddPre);
			this.groupBoxOrder.Controls.Add(this.buttonOrder);
			this.groupBoxOrder.Controls.Add(this.numericQty);
			this.groupBoxOrder.Controls.Add(this.numericPrice);
			this.groupBoxOrder.Controls.Add(this.labQty);
			this.groupBoxOrder.Controls.Add(this.labPrice);
			this.groupBoxOrder.Controls.Add(this.labComCode);
			this.groupBoxOrder.Controls.Add(this.groupBoxB_S);
			this.groupBoxOrder.Controls.Add(this.gbCloseMode);
			this.groupBoxOrder.Controls.Add(this.groupBoxO_L);
			this.groupBoxOrder.Controls.Add(this.numericLPrice);
			this.groupBoxOrder.Controls.Add(this.labelLPrice);
			this.groupBoxOrder.Controls.Add(this.comboMarKet);
			this.groupBoxOrder.Controls.Add(this.labTrancCode);
			this.groupBoxOrder.Controls.Add(this.labelMarKet);
			this.groupBoxOrder.Dock = DockStyle.Fill;
			this.groupBoxOrder.Location = new Point(0, 0);
			this.groupBoxOrder.Margin = new Padding(0);
			this.groupBoxOrder.Name = "groupBoxOrder";
			this.groupBoxOrder.Size = new Size(163, 264);
			this.groupBoxOrder.TabIndex = 0;
			this.groupBoxOrder.TabStop = false;
			this.groupBoxOrder.Text = "委托";
			this.lblPriceWithTax.AutoSize = true;
			this.lblPriceWithTax.Location = new Point(74, 139);
			this.lblPriceWithTax.Name = "lblPriceWithTax";
			this.lblPriceWithTax.Size = new Size(0, 12);
			this.lblPriceWithTax.TabIndex = 36;
			this.lblTax.AutoSize = true;
			this.lblTax.ForeColor = Color.Red;
			this.lblTax.Location = new Point(8, 139);
			this.lblTax.Name = "lblTax";
			this.lblTax.Size = new Size(65, 12);
			this.lblTax.TabIndex = 35;
			this.lblTax.Text = "含税价格：";
			this.comboTranc.Location = new Point(115, 12);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(37, 20);
			this.comboTranc.TabIndex = 0;
			this.comboTranc.TabStop = false;
			this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
			this.tbTranc.BackColor = Color.White;
			this.tbTranc.Location = new Point(74, 12);
			this.tbTranc.Multiline = true;
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.Size = new Size(42, 20);
			this.tbTranc.TabIndex = 34;
			this.tbTranc.TabStop = false;
			this.labelLargestTN.AutoSize = true;
			this.labelLargestTN.ImeMode = ImeMode.NoControl;
			this.labelLargestTN.Location = new Point(7, 162);
			this.labelLargestTN.Name = "labelLargestTN";
			this.labelLargestTN.Size = new Size(77, 12);
			this.labelLargestTN.TabIndex = 30;
			this.labelLargestTN.Text = "最大可交易量";
			this.labelLargestTN.Click += new EventHandler(this.labelLargestTN_Click);
			this.comboCommodity.Location = new Point(74, 34);
			this.comboCommodity.MaxLength = 6;
			this.comboCommodity.Name = "comboCommodity";
			this.comboCommodity.Size = new Size(78, 20);
			this.comboCommodity.TabIndex = 1;
			this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
			this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
			this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
			this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
			this.buttonAddPre.BackColor = Color.LightSteelBlue;
			this.buttonAddPre.FlatStyle = FlatStyle.Popup;
			this.buttonAddPre.ImeMode = ImeMode.NoControl;
			this.buttonAddPre.Location = new Point(86, 233);
			this.buttonAddPre.Name = "buttonAddPre";
			this.buttonAddPre.Size = new Size(65, 21);
			this.buttonAddPre.TabIndex = 10;
			this.buttonAddPre.Text = "预埋委托";
			this.buttonAddPre.UseVisualStyleBackColor = false;
			this.buttonAddPre.Click += new EventHandler(this.buttonAddPre_Click);
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			this.buttonOrder.FlatStyle = FlatStyle.Popup;
			this.buttonOrder.ImeMode = ImeMode.NoControl;
			this.buttonOrder.Location = new Point(11, 233);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(65, 21);
			this.buttonOrder.TabIndex = 9;
			this.buttonOrder.Text = "立即提交";
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.numericQty.Location = new Point(74, 181);
			NumericUpDown arg_84E_0 = this.numericQty;
			int[] array = new int[4];
			array[0] = 999999;
			arg_84E_0.Maximum = new decimal(array);
			this.numericQty.Name = "numericQty";
			this.numericQty.Size = new Size(78, 21);
			this.numericQty.TabIndex = 7;
			this.numericQty.Enter += new EventHandler(this.numericQty_Enter);
			this.numericQty.KeyUp += new KeyEventHandler(this.numericQty_KeyUp);
			NumericUpDown arg_8C9_0 = this.numericPrice;
			int[] array2 = new int[4];
			array2[0] = 10;
			arg_8C9_0.Increment = new decimal(array2);
			this.numericPrice.Location = new Point(74, 111);
			NumericUpDown arg_8FD_0 = this.numericPrice;
			int[] array3 = new int[4];
			array3[0] = 999999;
			arg_8FD_0.Maximum = new decimal(array3);
			this.numericPrice.Name = "numericPrice";
			this.numericPrice.Size = new Size(78, 21);
			this.numericPrice.TabIndex = 6;
			this.numericPrice.ValueChanged += new EventHandler(this.numericPrice_ValueChanged);
			this.numericPrice.Enter += new EventHandler(this.numericPrice_Enter);
			this.numericPrice.KeyUp += new KeyEventHandler(this.numericPrice_KeyUp);
			this.labQty.AutoSize = true;
			this.labQty.ImeMode = ImeMode.NoControl;
			this.labQty.Location = new Point(8, 185);
			this.labQty.Name = "labQty";
			this.labQty.Size = new Size(65, 12);
			this.labQty.TabIndex = 9;
			this.labQty.Text = "委托数量：";
			this.labQty.TextAlign = ContentAlignment.BottomLeft;
			this.labPrice.AutoSize = true;
			this.labPrice.Font = new Font("宋体", 9f);
			this.labPrice.ForeColor = SystemColors.ControlText;
			this.labPrice.ImeMode = ImeMode.NoControl;
			this.labPrice.Location = new Point(8, 114);
			this.labPrice.Name = "labPrice";
			this.labPrice.Size = new Size(65, 12);
			this.labPrice.TabIndex = 8;
			this.labPrice.Text = "委托价格：";
			this.labPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode.AutoSize = true;
			this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode.ImeMode = ImeMode.NoControl;
			this.labComCode.Location = new Point(6, 38);
			this.labComCode.Margin = new Padding(0);
			this.labComCode.Name = "labComCode";
			this.labComCode.Size = new Size(65, 12);
			this.labComCode.TabIndex = 4;
			this.labComCode.Text = "商品代码：";
			this.labComCode.TextAlign = ContentAlignment.BottomLeft;
			this.groupBoxB_S.BackColor = Color.Transparent;
			this.groupBoxB_S.Controls.Add(this.radioS);
			this.groupBoxB_S.Controls.Add(this.radioB);
			this.groupBoxB_S.Location = new Point(15, 51);
			this.groupBoxB_S.Margin = new Padding(0);
			this.groupBoxB_S.Name = "groupBoxB_S";
			this.groupBoxB_S.Padding = new Padding(0);
			this.groupBoxB_S.Size = new Size(126, 32);
			this.groupBoxB_S.TabIndex = 2;
			this.groupBoxB_S.TabStop = false;
			this.radioS.AutoSize = true;
			this.radioS.ForeColor = Color.Green;
			this.radioS.ImeMode = ImeMode.NoControl;
			this.radioS.Location = new Point(72, 12);
			this.radioS.Name = "radioS";
			this.radioS.Size = new Size(47, 16);
			this.radioS.TabIndex = 3;
			this.radioS.Text = "卖出";
			this.radioS.CheckedChanged += new EventHandler(this.radioS_CheckedChanged);
			this.radioS.Enter += new EventHandler(this.radioS_Enter);
			this.radioB.AutoSize = true;
			this.radioB.Checked = true;
			this.radioB.ForeColor = Color.Red;
			this.radioB.ImeMode = ImeMode.NoControl;
			this.radioB.Location = new Point(9, 12);
			this.radioB.Name = "radioB";
			this.radioB.Size = new Size(47, 16);
			this.radioB.TabIndex = 2;
			this.radioB.TabStop = true;
			this.radioB.Text = "买入";
			this.radioB.CheckedChanged += new EventHandler(this.radioB_CheckedChanged);
			this.radioB.Enter += new EventHandler(this.radioB_Enter);
			this.gbCloseMode.BackColor = Color.Transparent;
			this.gbCloseMode.Controls.Add(this.rbCloseH);
			this.gbCloseMode.Controls.Add(this.rbCloseT);
			this.gbCloseMode.Location = new Point(7, 198);
			this.gbCloseMode.Margin = new Padding(0);
			this.gbCloseMode.Name = "gbCloseMode";
			this.gbCloseMode.Padding = new Padding(0);
			this.gbCloseMode.Size = new Size(147, 32);
			this.gbCloseMode.TabIndex = 8;
			this.gbCloseMode.TabStop = false;
			this.gbCloseMode.Visible = false;
			this.gbCloseMode.Enter += new EventHandler(this.gbCloseMode_Enter);
			this.rbCloseH.AutoSize = true;
			this.rbCloseH.Checked = true;
			this.rbCloseH.ImeMode = ImeMode.NoControl;
			this.rbCloseH.Location = new Point(74, 11);
			this.rbCloseH.Name = "rbCloseH";
			this.rbCloseH.Size = new Size(71, 16);
			this.rbCloseH.TabIndex = 5;
			this.rbCloseH.TabStop = true;
			this.rbCloseH.Text = "转历史订";
			this.rbCloseT.AutoSize = true;
			this.rbCloseT.ImeMode = ImeMode.NoControl;
			this.rbCloseT.Location = new Point(3, 11);
			this.rbCloseT.Name = "rbCloseT";
			this.rbCloseT.Size = new Size(59, 16);
			this.rbCloseT.TabIndex = 4;
			this.rbCloseT.Text = "转今订";
			this.groupBoxO_L.BackColor = Color.Transparent;
			this.groupBoxO_L.Controls.Add(this.radioL);
			this.groupBoxO_L.Controls.Add(this.radioO);
			this.groupBoxO_L.Location = new Point(15, 77);
			this.groupBoxO_L.Name = "groupBoxO_L";
			this.groupBoxO_L.Size = new Size(126, 32);
			this.groupBoxO_L.TabIndex = 3;
			this.groupBoxO_L.TabStop = false;
			this.radioL.AutoSize = true;
			this.radioL.ImeMode = ImeMode.NoControl;
			this.radioL.Location = new Point(72, 12);
			this.radioL.Name = "radioL";
			this.radioL.Size = new Size(47, 16);
			this.radioL.TabIndex = 5;
			this.radioL.Text = "转让";
			this.radioL.CheckedChanged += new EventHandler(this.radioL_CheckedChanged);
			this.radioO.AutoSize = true;
			this.radioO.Checked = true;
			this.radioO.ImeMode = ImeMode.NoControl;
			this.radioO.Location = new Point(8, 12);
			this.radioO.Name = "radioO";
			this.radioO.Size = new Size(47, 16);
			this.radioO.TabIndex = 4;
			this.radioO.TabStop = true;
			this.radioO.Text = "订立";
			this.radioO.CheckedChanged += new EventHandler(this.radioO_CheckedChanged);
			this.radioO.Enter += new EventHandler(this.radioO_Enter);
			this.numericLPrice.Location = new Point(74, 207);
			NumericUpDown arg_1105_0 = this.numericLPrice;
			int[] array4 = new int[4];
			array4[0] = 999999;
			arg_1105_0.Maximum = new decimal(array4);
			this.numericLPrice.Name = "numericLPrice";
			this.numericLPrice.Size = new Size(78, 21);
			this.numericLPrice.TabIndex = 8;
			this.numericLPrice.Visible = false;
			this.numericLPrice.KeyUp += new KeyEventHandler(this.numericLPrice_KeyUp);
			this.numericLPrice.MouseDown += new MouseEventHandler(this.numericLPrice_MouseDown);
			this.labelLPrice.AutoSize = true;
			this.labelLPrice.ImeMode = ImeMode.NoControl;
			this.labelLPrice.Location = new Point(9, 211);
			this.labelLPrice.Name = "labelLPrice";
			this.labelLPrice.Size = new Size(65, 12);
			this.labelLPrice.TabIndex = 33;
			this.labelLPrice.Text = "指定价格：";
			this.labelLPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labelLPrice.Visible = false;
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet.Location = new Point(74, 12);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(78, 20);
			this.comboMarKet.TabIndex = 30;
			this.comboMarKet.Visible = false;
			this.labTrancCode.AutoSize = true;
			this.labTrancCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTrancCode.ImeMode = ImeMode.NoControl;
			this.labTrancCode.Location = new Point(6, 15);
			this.labTrancCode.Margin = new Padding(0);
			this.labTrancCode.Name = "labTrancCode";
			this.labTrancCode.Size = new Size(65, 12);
			this.labTrancCode.TabIndex = 2;
			this.labTrancCode.Text = "交易代码：";
			this.labTrancCode.TextAlign = ContentAlignment.BottomLeft;
			this.labelMarKet.AutoSize = true;
			this.labelMarKet.ImeMode = ImeMode.NoControl;
			this.labelMarKet.Location = new Point(12, 15);
			this.labelMarKet.Name = "labelMarKet";
			this.labelMarKet.Size = new Size(65, 12);
			this.labelMarKet.TabIndex = 31;
			this.labelMarKet.Text = "市场标志：";
			this.labelMarKet.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxOrder);
			base.Name = "Order";
			base.Size = new Size(163, 264);
			base.Load += new EventHandler(this.Order_Load);
			this.groupBoxOrder.ResumeLayout(false);
			this.groupBoxOrder.PerformLayout();
			((ISupportInitialize)this.numericQty).EndInit();
			((ISupportInitialize)this.numericPrice).EndInit();
			this.groupBoxB_S.ResumeLayout(false);
			this.groupBoxB_S.PerformLayout();
			this.gbCloseMode.ResumeLayout(false);
			this.gbCloseMode.PerformLayout();
			this.groupBoxO_L.ResumeLayout(false);
			this.groupBoxO_L.PerformLayout();
			((ISupportInitialize)this.numericLPrice).EndInit();
			base.ResumeLayout(false);
		}
	}
}
