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
using FuturesTrade.Gnnt.UI.Modules.Tools;

using FuturesTrade.Gnnt.UI.Modules.Status;
using FuturesTrade.Gnnt.UI.Modules.Query;
using FuturesTrade.Gnnt.UI.Modules.Notifier;
using FuturesTrade.Gnnt.UI.Modules.HQ;
namespace FuturesTrade.Gnnt.UI.Modules.Order
{
	public class P1Order : UserControl
	{
		private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private string buyStr = string.Empty;
		private string sellStr = string.Empty;
		private OperationManager operationManager = OperationManager.GetInstance();
		private P1Order.PromptLargestTradeNumCallBack PromptLargestTradeNum;
		private P1Order.OrderMessageInfoCallBack OrderMessageInfo;
		private byte BtnFlag;
		private short bsType;
		private IContainer components;
		private Panel panelOrder;
		private ComboBox comboMarKet;
		private NumericUpDown numericLPrice;
		private Label labelLPrice;
		private CheckBox checkBoxPreDelegate;
		private Label label2;
		private Label label1;
		private ComboBox comboTranc;
		private Button buttonCondition;
		private Button buttonBuy;
		private Button buttonBigenStatue;
		private ComboBox comboCommodity;
		private Button buttonSell;
		private ComboBox comboBoxOG;
		private NumericUpDown numericQty;
		private Label labelLargestTN_B;
		private NumericUpDown numericPrice;
		private Label labelPrice;
		private ComboBox comboBoxPriceType;
		private Label labelQty;
		private Label labelLargestTN_S;
		public P1Order()
		{
			this.InitializeComponent();
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.operationManager.orderOperation.setCommodityID = new OrderOperation.SetCommodityIDCallBack(this.SetCommodityID);
			this.operationManager.orderOperation.setLargestTN = new OrderOperation.SetLargestTNCallBack(this.SetLargestTNInfoB);
			this.operationManager.orderOperation.setLargestTN_S = new OrderOperation.SetLargestTNCallBack(this.SetLargestTNInfoS);
			this.operationManager.submitOrderOperation.SetFocus = new SubmitOrderOperation.SetFocusCallBack(this.SetFouce);
			this.operationManager.submitOrderOperation.OrderMessage = new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage);
			this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
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
				this.comboBoxOG.Enabled = true;
				this.buttonBuy.Enabled = true;
				this.buttonSell.Enabled = true;
				return;
			case 1:
			case 2:
				break;
			case 3:
				this.buttonBuy.Enabled = true;
				this.buttonSell.Enabled = false;
				this.comboBoxOG.Enabled = false;
				this.comboBoxOG.SelectedIndex = 0;
				return;
			case 4:
				this.buttonBuy.Enabled = false;
				this.buttonSell.Enabled = true;
				this.comboBoxOG.Enabled = false;
				this.comboBoxOG.SelectedIndex = 0;
				break;
			default:
				return;
			}
		}
		private void comboCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numericPrice.Value = 0m;
			this.numericQty.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.labelLargestTN_B.Text = "";
			this.labelLargestTN_S.Text = "";
			this.operationManager.orderOperation.SetListBoxVisible(false);
			this.operationManager.orderOperation.ShowMinLine(this.comboCommodity.Text);
			int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
			this.SetRadioEnable(currentTradeMode);
		}
		private void comboCommodity_TextChanged(object sender, EventArgs e)
		{
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
		private void comboBoxOG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxOG.SelectedIndex == 3)
			{
				this.labelLPrice.Visible = true;
				this.numericLPrice.Visible = true;
				IniData.GetInstance().CloseMode = 3;
				return;
			}
			if (this.comboBoxOG.SelectedIndex == 2)
			{
				this.labelLPrice.Visible = false;
				this.numericLPrice.Visible = false;
				IniData.GetInstance().CloseMode = 2;
				return;
			}
			IniData.GetInstance().CloseMode = 1;
			this.labelLPrice.Visible = false;
			this.numericLPrice.Visible = false;
		}
		private void comboBoxPriceType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxPriceType.SelectedIndex == 1)
			{
				this.operationManager.orderOperation.IsChangePrice = false;
				this.numericPrice.Enabled = false;
				this.numericPrice.Value = 0m;
				this.buttonBuy.Text = this.buyStr;
				this.buttonSell.Text = this.sellStr;
				return;
			}
			this.numericPrice.Enabled = true;
			this.operationManager.orderOperation.IsChangePrice = true;
		}
		private void comboBoxPriceType_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
			{
				this.comboBoxPriceType.SelectedIndex = 0;
				return;
			}
			if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
			{
				this.comboBoxPriceType.SelectedIndex = 1;
			}
		}
		private void numericPrice_Enter(object sender, EventArgs e)
		{
			if (this.numericPrice.Value == 0m)
			{
				this.numericPrice.Select(0, this.numericPrice.Text.Length);
			}
			this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
		}
		private void numericPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericPrice_ValueChanged(object sender, EventArgs e)
		{
			this.buttonBuy.Text = this.numericPrice.Value.ToString() + this.buyStr;
			this.buttonSell.Text = this.numericPrice.Value.ToString() + this.sellStr;
		}
		private void numericQty_Enter(object sender, EventArgs e)
		{
			this.numericQty.Select(0, this.numericQty.Value.ToString().Length);
			this.labelLargestTN_B.ForeColor = Global.LightColor;
			this.labelLargestTN_S.ForeColor = Global.LightColor;
			short num = 1;
			short num2 = 2;
			short num3 = 1;
			if (this.comboBoxOG.SelectedIndex != 0)
			{
				num3 = 2;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Commodity", this.comboCommodity.Text);
			hashtable.Add("B_SType", num);
			hashtable.Add("O_LType", num3);
			if (this.numericPrice.Enabled)
			{
				hashtable.Add("numericPrice", Convert.ToDouble(this.numericPrice.Value));
			}
			else
			{
				hashtable.Add("numericPrice", Convert.ToDouble(this.operationManager.orderOperation.GetBSPrice((int)num)));
			}
			hashtable.Add("tbTranc_comboTranc", Global.UserID + "00");
			this.operationManager.orderOperation.GetNumericQtyThread(hashtable);
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("Commodity", this.comboCommodity.Text);
			hashtable2.Add("B_SType", num2);
			hashtable2.Add("O_LType", num3);
			if (this.numericPrice.Enabled)
			{
				hashtable2.Add("numericPrice", Convert.ToDouble(this.numericPrice.Value));
			}
			else
			{
				hashtable2.Add("numericPrice", Convert.ToDouble(this.operationManager.orderOperation.GetBSPrice((int)num2)));
			}
			hashtable2.Add("tbTranc_comboTranc", Global.UserID + "00");
			this.operationManager.orderOperation.GetNumericQtyThread(hashtable2);
		}
		private void numericQty_KeyUp(object sender, KeyEventArgs e)
		{
			Global.QtyKeyUp(sender, e);
		}
		private void numericLPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
		{
			this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
		}
		private void buttonBigenStatue_Click(object sender, EventArgs e)
		{
			this.comboBoxOG.SelectedIndex = 0;
			this.comboBoxPriceType.SelectedIndex = 0;
			this.numericQty.Text = "";
			this.numericPrice.Value = 0m;
			this.labelLargestTN_B.Text = "";
			this.labelLargestTN_S.Text = "";
		}
		private void buttonBuy_Click(object sender, EventArgs e)
		{
			this.bsType = 1;
			this.SubmintOrder();
		}
		private void buttonSell_Click(object sender, EventArgs e)
		{
			this.bsType = 2;
			this.SubmintOrder();
		}
		private void SubmintOrder()
		{
			SubmitOrderInfo submitOrderInfo = new SubmitOrderInfo();
			submitOrderInfo.customerID = Global.FirmID + Global.CustomerID;
			submitOrderInfo.commodityID = this.comboCommodity.Text;
			submitOrderInfo.B_SType = this.bsType;
			if (this.comboBoxOG.SelectedIndex != 0)
			{
				submitOrderInfo.O_LType = 2;
			}
            submitOrderInfo.price = ToolsLibrary.util.Tools.StrToDouble(this.numericPrice.Value.ToString(), 0.0);
			if (this.comboBoxPriceType.SelectedIndex == 1)
			{
                submitOrderInfo.price = ToolsLibrary.util.Tools.StrToDouble(this.operationManager.orderOperation.GetBSPrice((int)this.bsType).ToString(), 0.0);
			}
            submitOrderInfo.qty = ToolsLibrary.util.Tools.StrToInt(this.numericQty.Value.ToString(), 0);
			if (submitOrderInfo.O_LType == 2)
			{
				if (IniData.GetInstance().CloseMode == 2)
				{
					submitOrderInfo.closeMode = 2;
					if (this.comboBoxOG.SelectedIndex == 2)
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
			if (this.checkBoxPreDelegate.Checked)
			{
				this.BtnFlag = 1;
			}
			else
			{
				this.BtnFlag = 0;
			}
			OperationManager.GetInstance().orderOperation.orderType = OrderType.Order;
			this.operationManager.submitOrderOperation.ButtonOrderComm(submitOrderInfo, this.BtnFlag);
		}
		private void P1Order_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.ComboTrans();
			this.comboBoxPriceType.SelectedIndex = 0;
		}
		private void SetControlText()
		{
			this.labelLPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabelLPrice");
			this.labelPrice.Text = Global.M_ResourceManager.GetString("TradeStr_MainForm_Price");
			this.labelQty.Text = Global.M_ResourceManager.GetString("TradeStr_T1MainForm_labelQty");
			this.checkBoxPreDelegate.Text = Global.M_ResourceManager.GetString("TradeStr_T1MainForm_checkBoxPreDelegate");
			this.buttonBuy.Text = "\r\n\r\n" + Global.M_ResourceManager.GetString("TradeStr_RadioB");
			this.buttonSell.Text = "\r\n\r\n" + Global.M_ResourceManager.GetString("TradeStr_RadioS");
			this.buttonBigenStatue.Text = Global.M_ResourceManager.GetString("TradeStr_T1MainForm_Reset");
			this.buttonCondition.Text = Global.M_ResourceManager.GetString("TradeStr_T1MainForm_conditionOrder");
			this.buyStr = this.buttonBuy.Text;
			this.sellStr = this.buttonSell.Text;
		}
		private void ComboTrans()
		{
			this.comboBoxOG.Items.Clear();
			string @string = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1");
			string string2 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2");
			this.comboBoxOG.Items.Add(@string);
			this.comboBoxOG.Items.Add(string2);
			string string3 = Global.M_ResourceManager.GetString("TradeStr_TransferToday");
			this.comboBoxOG.Items.Add(string3);
			string string4 = Global.M_ResourceManager.GetString("Global_CloseModeStrArr2");
			this.comboBoxOG.Items.Add(string4);
			this.comboBoxOG.SelectedIndex = 0;
		}
		private void SetButtonOrderEnable(bool enable)
		{
			this.buttonBuy.Enabled = enable;
			this.buttonSell.Enabled = enable;
			this.buttonCondition.Enabled = enable;
		}
		private void SetLargestTNInfoB(string text, int colorFlag)
		{
			try
			{
				this.PromptLargestTradeNum = new P1Order.PromptLargestTradeNumCallBack(this.LargestTNInfoB);
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
		private void SetLargestTNInfoS(string text, int colorFlag)
		{
			try
			{
				this.PromptLargestTradeNum = new P1Order.PromptLargestTradeNumCallBack(this.LargestTNInfoS);
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
		private void LargestTNInfoB(string text, int colorFlag)
		{
			if (colorFlag == 0)
			{
				this.labelLargestTN_B.ForeColor = Global.LightColor;
			}
			else
			{
				if (colorFlag == 1)
				{
					this.labelLargestTN_B.ForeColor = Global.ErrorColor;
				}
			}
			this.labelLargestTN_B.Text = this.operationManager.StrBuy + text;
		}
		private void LargestTNInfoS(string text, int colorFlag)
		{
			if (colorFlag == 0)
			{
				this.labelLargestTN_S.ForeColor = Global.LightColor;
			}
			else
			{
				if (colorFlag == 1)
				{
					this.labelLargestTN_S.ForeColor = Global.ErrorColor;
				}
			}
			this.labelLargestTN_S.Text = this.operationManager.StrSale + text;
		}
		private void SetFouce(short flag)
		{
			this.labelLargestTN_B.Text = "";
			this.labelLargestTN_S.Text = "";
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
					this.OrderMessageInfo = new P1Order.OrderMessageInfoCallBack(this.OrderInfoMessage);
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
		private void SetCommodityID(string commodityID)
		{
			this.comboCommodity.Text = commodityID;
			int num = this.comboCommodity.FindStringExact(commodityID);
			if (num != -1)
			{
				this.comboCommodity.SelectedIndex = num;
			}
		}
		private void buttonCondition_Click(object sender, EventArgs e)
		{
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		public void SetOrderInfo(string CommodityCode, double BuyPrice, double SellPrice)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = CommodityCode.IndexOf("_");
			if (num != -1)
			{
				text = CommodityCode.Substring(0, num);
				text2 = CommodityCode.Substring(num + 1);
			}
			else
			{
				text2 = CommodityCode;
			}
			if (text2 != this.comboCommodity.Text)
			{
				this.operationManager.orderOperation.ConnectHQ = true;
			}
			if (Global.MarketHT.Count == 1)
			{
				for (int i = 0; i < this.comboCommodity.Items.Count; i++)
				{
					if (text2.Equals(this.comboCommodity.Items[i].ToString()))
					{
						this.comboCommodity.SelectedIndex = i;
						return;
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
			for (int k = 0; k < this.comboCommodity.Items.Count; k++)
			{
				if (text2.Equals(this.comboCommodity.Items[k].ToString()))
				{
					this.comboCommodity.SelectedIndex = k;
					return;
				}
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
				if (ordertype == 0)
				{
					this.comboBoxOG.SelectedIndex = 0;
				}
				else
				{
					this.comboBoxOG.SelectedIndex = 1;
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
			this.panelOrder = new Panel();
			this.comboMarKet = new ComboBox();
			this.numericLPrice = new NumericUpDown();
			this.labelLPrice = new Label();
			this.checkBoxPreDelegate = new CheckBox();
			this.label2 = new Label();
			this.label1 = new Label();
			this.comboTranc = new ComboBox();
			this.buttonCondition = new Button();
			this.buttonBuy = new Button();
			this.buttonBigenStatue = new Button();
			this.comboCommodity = new ComboBox();
			this.buttonSell = new Button();
			this.comboBoxOG = new ComboBox();
			this.numericQty = new NumericUpDown();
			this.labelLargestTN_B = new Label();
			this.numericPrice = new NumericUpDown();
			this.labelPrice = new Label();
			this.comboBoxPriceType = new ComboBox();
			this.labelQty = new Label();
			this.labelLargestTN_S = new Label();
			this.panelOrder.SuspendLayout();
			((ISupportInitialize)this.numericLPrice).BeginInit();
			((ISupportInitialize)this.numericQty).BeginInit();
			((ISupportInitialize)this.numericPrice).BeginInit();
			base.SuspendLayout();
			this.panelOrder.BorderStyle = BorderStyle.FixedSingle;
			this.panelOrder.Controls.Add(this.comboMarKet);
			this.panelOrder.Controls.Add(this.numericLPrice);
			this.panelOrder.Controls.Add(this.labelLPrice);
			this.panelOrder.Controls.Add(this.checkBoxPreDelegate);
			this.panelOrder.Controls.Add(this.label2);
			this.panelOrder.Controls.Add(this.label1);
			this.panelOrder.Controls.Add(this.comboTranc);
			this.panelOrder.Controls.Add(this.buttonCondition);
			this.panelOrder.Controls.Add(this.buttonBuy);
			this.panelOrder.Controls.Add(this.buttonBigenStatue);
			this.panelOrder.Controls.Add(this.comboCommodity);
			this.panelOrder.Controls.Add(this.buttonSell);
			this.panelOrder.Controls.Add(this.comboBoxOG);
			this.panelOrder.Controls.Add(this.numericQty);
			this.panelOrder.Controls.Add(this.labelLargestTN_S);
			this.panelOrder.Controls.Add(this.labelLargestTN_B);
			this.panelOrder.Controls.Add(this.numericPrice);
			this.panelOrder.Controls.Add(this.labelPrice);
			this.panelOrder.Controls.Add(this.comboBoxPriceType);
			this.panelOrder.Controls.Add(this.labelQty);
			this.panelOrder.Dock = DockStyle.Fill;
			this.panelOrder.Location = new Point(0, 0);
			this.panelOrder.Margin = new Padding(1);
			this.panelOrder.Name = "panelOrder";
			this.panelOrder.Size = new Size(270, 250);
			this.panelOrder.TabIndex = 1;
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet.Location = new Point(179, 107);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(78, 20);
			this.comboMarKet.TabIndex = 36;
			this.comboMarKet.Visible = false;
			this.numericLPrice.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericLPrice.Location = new Point(67, 74);
			NumericUpDown arg_3F2_0 = this.numericLPrice;
			int[] array = new int[4];
			array[0] = 999999;
			arg_3F2_0.Maximum = new decimal(array);
			this.numericLPrice.Name = "numericLPrice";
			this.numericLPrice.Size = new Size(56, 21);
			this.numericLPrice.TabIndex = 5;
			this.numericLPrice.Visible = false;
			this.numericLPrice.KeyUp += new KeyEventHandler(this.numericLPrice_KeyUp);
			this.numericLPrice.MouseDown += new MouseEventHandler(this.numericLPrice_MouseDown);
			this.labelLPrice.AutoSize = true;
			this.labelLPrice.Font = new Font("宋体", 9f);
			this.labelLPrice.ImeMode = ImeMode.NoControl;
			this.labelLPrice.Location = new Point(4, 78);
			this.labelLPrice.Name = "labelLPrice";
			this.labelLPrice.Size = new Size(59, 12);
			this.labelLPrice.TabIndex = 35;
			this.labelLPrice.Text = "指定价格:";
			this.labelLPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labelLPrice.Visible = false;
			this.checkBoxPreDelegate.AutoSize = true;
			this.checkBoxPreDelegate.Location = new Point(9, 160);
			this.checkBoxPreDelegate.Name = "checkBoxPreDelegate";
			this.checkBoxPreDelegate.Size = new Size(60, 16);
			this.checkBoxPreDelegate.TabIndex = 9;
			this.checkBoxPreDelegate.Text = "预埋单";
			this.checkBoxPreDelegate.UseVisualStyleBackColor = true;
			this.label2.BackColor = Color.Green;
			this.label2.Font = new Font("宋体", 9f);
			this.label2.ForeColor = Color.Green;
			this.label2.Location = new Point(188, 191);
			this.label2.Name = "label2";
			this.label2.Size = new Size(60, 1);
			this.label2.TabIndex = 15;
			this.label2.Text = "_________";
			this.label1.BackColor = Color.OrangeRed;
			this.label1.Font = new Font("宋体", 9f);
			this.label1.ForeColor = Color.OrangeRed;
			this.label1.Location = new Point(87, 191);
			this.label1.Name = "label1";
			this.label1.Size = new Size(60, 1);
			this.label1.TabIndex = 14;
			this.label1.Text = "_________";
			this.comboTranc.Location = new Point(218, 107);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(37, 20);
			this.comboTranc.TabIndex = 13;
			this.comboTranc.Visible = false;
			this.buttonCondition.Location = new Point(8, 208);
			this.buttonCondition.Name = "buttonCondition";
			this.buttonCondition.Size = new Size(60, 26);
			this.buttonCondition.TabIndex = 10;
			this.buttonCondition.Text = "条件单";
			this.buttonCondition.UseVisualStyleBackColor = true;
			this.buttonCondition.Visible = false;
			this.buttonCondition.Click += new EventHandler(this.buttonCondition_Click);
			this.buttonBuy.Cursor = Cursors.Default;
			this.buttonBuy.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.buttonBuy.ForeColor = Color.OrangeRed;
			this.buttonBuy.Location = new Point(73, 159);
			this.buttonBuy.Name = "buttonBuy";
			this.buttonBuy.Size = new Size(89, 76);
			this.buttonBuy.TabIndex = 7;
			this.buttonBuy.Text = "\r\n\r\n买入";
			this.buttonBuy.UseVisualStyleBackColor = true;
			this.buttonBuy.Click += new EventHandler(this.buttonBuy_Click);
			this.buttonBigenStatue.Location = new Point(8, 179);
			this.buttonBigenStatue.Name = "buttonBigenStatue";
			this.buttonBigenStatue.Size = new Size(60, 26);
			this.buttonBigenStatue.TabIndex = 9;
			this.buttonBigenStatue.Text = "复位";
			this.buttonBigenStatue.UseVisualStyleBackColor = true;
			this.buttonBigenStatue.Click += new EventHandler(this.buttonBigenStatue_Click);
			this.comboCommodity.FormattingEnabled = true;
			this.comboCommodity.Location = new Point(9, 9);
			this.comboCommodity.Name = "comboCommodity";
			this.comboCommodity.Size = new Size(108, 20);
			this.comboCommodity.TabIndex = 0;
			this.comboCommodity.Text = "HJ 黄金";
			this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
			this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
			this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
			this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
			this.buttonSell.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.buttonSell.ForeColor = Color.Green;
			this.buttonSell.Location = new Point(172, 159);
			this.buttonSell.Name = "buttonSell";
			this.buttonSell.Size = new Size(89, 76);
			this.buttonSell.TabIndex = 8;
			this.buttonSell.Text = "\r\n\r\n卖出";
			this.buttonSell.UseVisualStyleBackColor = true;
			this.buttonSell.Click += new EventHandler(this.buttonSell_Click);
			this.comboBoxOG.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxOG.FormattingEnabled = true;
			this.comboBoxOG.Items.AddRange(new object[]
			{
				"订立",
				"转今",
				"转让",
				"按价格转让"
			});
			this.comboBoxOG.Location = new Point(176, 9);
			this.comboBoxOG.Name = "comboBoxOG";
			this.comboBoxOG.Size = new Size(86, 20);
			this.comboBoxOG.TabIndex = 1;
			this.comboBoxOG.SelectedIndexChanged += new EventHandler(this.comboBoxOG_SelectedIndexChanged);
			this.numericQty.Location = new Point(176, 74);
			NumericUpDown arg_AFD_0 = this.numericQty;
			int[] array2 = new int[4];
			array2[0] = 99999;
			arg_AFD_0.Maximum = new decimal(array2);
			this.numericQty.Name = "numericQty";
			this.numericQty.Size = new Size(86, 21);
			this.numericQty.TabIndex = 4;
			this.numericQty.Enter += new EventHandler(this.numericQty_Enter);
			this.numericQty.KeyUp += new KeyEventHandler(this.numericQty_KeyUp);
			this.labelLargestTN_B.AutoSize = true;
			this.labelLargestTN_B.Location = new Point(14, 109);
			this.labelLargestTN_B.Name = "labelLargestTN_B";
			this.labelLargestTN_B.Size = new Size(101, 12);
			this.labelLargestTN_B.TabIndex = 8;
			this.labelLargestTN_B.Text = "买参考可订立量：";
			this.numericPrice.Location = new Point(176, 40);
			NumericUpDown arg_BF2_0 = this.numericPrice;
			int[] array3 = new int[4];
			array3[0] = 99999;
			arg_BF2_0.Maximum = new decimal(array3);
			this.numericPrice.Name = "numericPrice";
			this.numericPrice.Size = new Size(86, 21);
			this.numericPrice.TabIndex = 3;
			this.numericPrice.ValueChanged += new EventHandler(this.numericPrice_ValueChanged);
			this.numericPrice.Enter += new EventHandler(this.numericPrice_Enter);
			this.numericPrice.KeyUp += new KeyEventHandler(this.numericPrice_KeyUp);
			this.labelPrice.AutoSize = true;
			this.labelPrice.Location = new Point(129, 44);
			this.labelPrice.Name = "labelPrice";
			this.labelPrice.Size = new Size(41, 12);
			this.labelPrice.TabIndex = 7;
			this.labelPrice.Text = "价格：";
			this.comboBoxPriceType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxPriceType.FormattingEnabled = true;
			this.comboBoxPriceType.Items.AddRange(new object[]
			{
				"1.限价",
				"2.市价"
			});
			this.comboBoxPriceType.Location = new Point(9, 40);
			this.comboBoxPriceType.Name = "comboBoxPriceType";
			this.comboBoxPriceType.Size = new Size(108, 20);
			this.comboBoxPriceType.TabIndex = 2;
			this.comboBoxPriceType.SelectedIndexChanged += new EventHandler(this.comboBoxPriceType_SelectedIndexChanged);
			this.comboBoxPriceType.KeyUp += new KeyEventHandler(this.comboBoxPriceType_KeyUp);
			this.labelQty.AutoSize = true;
			this.labelQty.Location = new Point(129, 79);
			this.labelQty.Name = "labelQty";
			this.labelQty.Size = new Size(41, 12);
			this.labelQty.TabIndex = 6;
			this.labelQty.Text = "数量：";
			this.labelLargestTN_S.AutoSize = true;
			this.labelLargestTN_S.Location = new Point(14, 133);
			this.labelLargestTN_S.Name = "labelLargestTN_S";
			this.labelLargestTN_S.Size = new Size(101, 12);
			this.labelLargestTN_S.TabIndex = 8;
			this.labelLargestTN_S.Text = "卖参考可订立量：";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panelOrder);
			base.Name = "P1Order";
			base.Size = new Size(270, 250);
			base.Load += new EventHandler(this.P1Order_Load);
			this.panelOrder.ResumeLayout(false);
			this.panelOrder.PerformLayout();
			((ISupportInitialize)this.numericLPrice).EndInit();
			((ISupportInitialize)this.numericQty).EndInit();
			((ISupportInitialize)this.numericPrice).EndInit();
			base.ResumeLayout(false);
		}
	}
}
