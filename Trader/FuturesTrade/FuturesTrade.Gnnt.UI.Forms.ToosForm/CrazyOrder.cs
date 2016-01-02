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
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class CrazyOrder : Form
	{
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private IContainer components;
		private GroupBox groupBoxCO;
		private Panel panel_co;
		private Button btn_SO;
		private Button btn_SL;
		private Button btn_BL;
		private Button btn_BO;
		private ComboBox comboTranc_co;
		private Label labQty_co;
		private NumericUpDown numericQty_co;
		private Label labelLargestTN_co;
		private TextBox tbTranc_co;
		private ComboBox comboCommodity_co;
		private NumericUpDown numericPrice_co;
		private Label labPrice_co;
		private Label labComCode_co;
		private ComboBox comboMarKet_co;
		private Label labTrancCode_co;
		private Label labelMarKet_co;
		private string NumIsNotZero = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
		private string ErrorPriceMassage = "价格不能为0！！！";
		private SubmitOrderInfo submitOrderInfo;
		private bool ConnectHQ;
		private bool displayInfo = true;
		private bool isMove;
		private double sPrice;
		private double bPrice;
		private string TitleInfo = string.Empty;
		private short BuySell;
		private short SettleBasis;
		private bool buttonClick;
		private bool isBtnFocused;
		private bool isBOFocused;
		private bool isBLFocused;
		private bool isSOFocused;
		private bool isSLFocused;
		private bool isDirectfirm;
		private string curCommodityMode = "";
		private CrazyOrder.OrderMessageInfoCallBack OrderMessageInfo;
		private static volatile CrazyOrder instance;
		protected override void Dispose(bool disposing)
		{
			if (CrazyOrder.instance != null)
			{
				CrazyOrder.instance = null;
			}
			Global.SetOrderInfo -= new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			SubmitOrderOperation expr_2C = OperationManager.GetInstance().submitOrderOperation;
			expr_2C.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Remove(expr_2C.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.groupBoxCO = new GroupBox();
			this.panel_co = new Panel();
			this.btn_SO = new Button();
			this.btn_SL = new Button();
			this.btn_BL = new Button();
			this.btn_BO = new Button();
			this.comboTranc_co = new ComboBox();
			this.labQty_co = new Label();
			this.numericQty_co = new NumericUpDown();
			this.labelLargestTN_co = new Label();
			this.tbTranc_co = new TextBox();
			this.comboCommodity_co = new ComboBox();
			this.numericPrice_co = new NumericUpDown();
			this.labPrice_co = new Label();
			this.labComCode_co = new Label();
			this.comboMarKet_co = new ComboBox();
			this.labTrancCode_co = new Label();
			this.labelMarKet_co = new Label();
			this.groupBoxCO.SuspendLayout();
			this.panel_co.SuspendLayout();
			((ISupportInitialize)this.numericQty_co).BeginInit();
			((ISupportInitialize)this.numericPrice_co).BeginInit();
			base.SuspendLayout();
			this.groupBoxCO.BackColor = Color.Transparent;
			this.groupBoxCO.BackgroundImageLayout = ImageLayout.Stretch;
			this.groupBoxCO.Controls.Add(this.panel_co);
			this.groupBoxCO.Controls.Add(this.comboTranc_co);
			this.groupBoxCO.Controls.Add(this.labQty_co);
			this.groupBoxCO.Controls.Add(this.numericQty_co);
			this.groupBoxCO.Controls.Add(this.labelLargestTN_co);
			this.groupBoxCO.Controls.Add(this.tbTranc_co);
			this.groupBoxCO.Controls.Add(this.comboCommodity_co);
			this.groupBoxCO.Controls.Add(this.numericPrice_co);
			this.groupBoxCO.Controls.Add(this.labPrice_co);
			this.groupBoxCO.Controls.Add(this.labComCode_co);
			this.groupBoxCO.Controls.Add(this.comboMarKet_co);
			this.groupBoxCO.Controls.Add(this.labTrancCode_co);
			this.groupBoxCO.Controls.Add(this.labelMarKet_co);
			this.groupBoxCO.Dock = DockStyle.Fill;
			this.groupBoxCO.Location = new Point(0, 0);
			this.groupBoxCO.Name = "groupBoxCO";
			this.groupBoxCO.Size = new Size(194, 222);
			this.groupBoxCO.TabIndex = 8;
			this.groupBoxCO.TabStop = false;
			this.groupBoxCO.Text = "委托";
			this.panel_co.Controls.Add(this.btn_SO);
			this.panel_co.Controls.Add(this.btn_SL);
			this.panel_co.Controls.Add(this.btn_BL);
			this.panel_co.Controls.Add(this.btn_BO);
			this.panel_co.Location = new Point(11, 152);
			this.panel_co.Margin = new Padding(0);
			this.panel_co.Name = "panel_co";
			this.panel_co.Size = new Size(170, 62);
			this.panel_co.TabIndex = 35;
			this.btn_SO.BackColor = Color.LightSteelBlue;
			this.btn_SO.FlatStyle = FlatStyle.Popup;
			this.btn_SO.ForeColor = Color.Green;
			this.btn_SO.Location = new Point(93, 3);
			this.btn_SO.Name = "btn_SO";
			this.btn_SO.Size = new Size(75, 23);
			this.btn_SO.TabIndex = 6;
			this.btn_SO.Text = "卖订立";
			this.btn_SO.UseVisualStyleBackColor = false;
			this.btn_SO.Click += new EventHandler(this.btn_SO_Click);
			this.btn_SL.BackColor = Color.LightSteelBlue;
			this.btn_SL.FlatStyle = FlatStyle.Popup;
			this.btn_SL.ForeColor = Color.Green;
			this.btn_SL.Location = new Point(93, 35);
			this.btn_SL.Name = "btn_SL";
			this.btn_SL.Size = new Size(75, 23);
			this.btn_SL.TabIndex = 7;
			this.btn_SL.Text = "卖转让";
			this.btn_SL.UseVisualStyleBackColor = false;
			this.btn_SL.Click += new EventHandler(this.btn_SL_Click);
			this.btn_BL.BackColor = Color.LightSteelBlue;
			this.btn_BL.FlatStyle = FlatStyle.Popup;
			this.btn_BL.ForeColor = Color.Red;
			this.btn_BL.Location = new Point(3, 35);
			this.btn_BL.Name = "btn_BL";
			this.btn_BL.Size = new Size(75, 23);
			this.btn_BL.TabIndex = 5;
			this.btn_BL.Text = "买转让";
			this.btn_BL.UseVisualStyleBackColor = false;
			this.btn_BL.Click += new EventHandler(this.btn_BL_Click);
			this.btn_BO.BackColor = Color.LightSteelBlue;
			this.btn_BO.FlatStyle = FlatStyle.Popup;
			this.btn_BO.ForeColor = Color.Red;
			this.btn_BO.Location = new Point(3, 3);
			this.btn_BO.Name = "btn_BO";
			this.btn_BO.Size = new Size(75, 23);
			this.btn_BO.TabIndex = 4;
			this.btn_BO.Text = "买订立";
			this.btn_BO.UseVisualStyleBackColor = false;
			this.btn_BO.Click += new EventHandler(this.btn_BO_Click);
			this.comboTranc_co.Location = new Point(128, 12);
			this.comboTranc_co.MaxLength = 2;
			this.comboTranc_co.Name = "comboTranc_co";
			this.comboTranc_co.Size = new Size(37, 20);
			this.comboTranc_co.TabIndex = 0;
			this.labQty_co.AutoSize = true;
			this.labQty_co.ImeMode = ImeMode.NoControl;
			this.labQty_co.Location = new Point(15, 105);
			this.labQty_co.Name = "labQty_co";
			this.labQty_co.Size = new Size(65, 12);
			this.labQty_co.TabIndex = 9;
			this.labQty_co.Text = "委托数量：";
			this.labQty_co.TextAlign = ContentAlignment.BottomLeft;
			this.numericQty_co.Location = new Point(88, 102);
			NumericUpDown arg_6DD_0 = this.numericQty_co;
			int[] array = new int[4];
			array[0] = 999999;
			arg_6DD_0.Maximum = new decimal(array);
			this.numericQty_co.Name = "numericQty_co";
			this.numericQty_co.Size = new Size(78, 21);
			this.numericQty_co.TabIndex = 3;
			this.numericQty_co.Enter += new EventHandler(this.numericQty_co_Enter);
			this.numericQty_co.KeyUp += new KeyEventHandler(this.numericQty_co_KeyUp);
			this.labelLargestTN_co.AutoSize = true;
			this.labelLargestTN_co.ImeMode = ImeMode.NoControl;
			this.labelLargestTN_co.Location = new Point(15, 130);
			this.labelLargestTN_co.Name = "labelLargestTN_co";
			this.labelLargestTN_co.Size = new Size(77, 12);
			this.labelLargestTN_co.TabIndex = 30;
			this.labelLargestTN_co.Text = "最大可交易量";
			this.labelLargestTN_co.Visible = false;
			this.tbTranc_co.BackColor = Color.White;
			this.tbTranc_co.Location = new Point(88, 12);
			this.tbTranc_co.Multiline = true;
			this.tbTranc_co.Name = "tbTranc_co";
			this.tbTranc_co.ReadOnly = true;
			this.tbTranc_co.Size = new Size(42, 20);
			this.tbTranc_co.TabIndex = 34;
			this.tbTranc_co.TabStop = false;
			this.comboCommodity_co.Location = new Point(88, 40);
			this.comboCommodity_co.MaxLength = 6;
			this.comboCommodity_co.Name = "comboCommodity_co";
			this.comboCommodity_co.Size = new Size(78, 20);
			this.comboCommodity_co.TabIndex = 1;
			this.comboCommodity_co.SelectedIndexChanged += new EventHandler(this.comboCommodity_co_SelectedIndexChanged);
			this.comboCommodity_co.TextChanged += new EventHandler(this.comboCommodity_co_TextChanged);
			this.numericPrice_co.Location = new Point(88, 70);
			NumericUpDown arg_8E2_0 = this.numericPrice_co;
			int[] array2 = new int[4];
			array2[0] = 999999;
			arg_8E2_0.Maximum = new decimal(array2);
			this.numericPrice_co.Name = "numericPrice_co";
			this.numericPrice_co.Size = new Size(78, 21);
			this.numericPrice_co.TabIndex = 2;
			this.numericPrice_co.Enter += new EventHandler(this.numericPrice_co_Enter);
			this.numericPrice_co.KeyUp += new KeyEventHandler(this.numericPrice_co_KeyUp);
			this.labPrice_co.AutoSize = true;
			this.labPrice_co.Font = new Font("宋体", 9f);
			this.labPrice_co.ForeColor = SystemColors.ControlText;
			this.labPrice_co.ImeMode = ImeMode.NoControl;
			this.labPrice_co.Location = new Point(15, 75);
			this.labPrice_co.Name = "labPrice_co";
			this.labPrice_co.Size = new Size(65, 12);
			this.labPrice_co.TabIndex = 8;
			this.labPrice_co.Text = "委托价格：";
			this.labPrice_co.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode_co.AutoSize = true;
			this.labComCode_co.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode_co.ImeMode = ImeMode.NoControl;
			this.labComCode_co.Location = new Point(15, 45);
			this.labComCode_co.Name = "labComCode_co";
			this.labComCode_co.Size = new Size(65, 12);
			this.labComCode_co.TabIndex = 4;
			this.labComCode_co.Text = "商品代码：";
			this.labComCode_co.TextAlign = ContentAlignment.BottomLeft;
			this.comboMarKet_co.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet_co.Location = new Point(88, 12);
			this.comboMarKet_co.Name = "comboMarKet_co";
			this.comboMarKet_co.Size = new Size(78, 20);
			this.comboMarKet_co.TabIndex = 30;
			this.comboMarKet_co.Visible = false;
			this.labTrancCode_co.AutoSize = true;
			this.labTrancCode_co.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTrancCode_co.ImeMode = ImeMode.NoControl;
			this.labTrancCode_co.Location = new Point(15, 15);
			this.labTrancCode_co.Name = "labTrancCode_co";
			this.labTrancCode_co.Size = new Size(65, 12);
			this.labTrancCode_co.TabIndex = 2;
			this.labTrancCode_co.Text = "交易代码：";
			this.labTrancCode_co.TextAlign = ContentAlignment.BottomLeft;
			this.labelMarKet_co.AutoSize = true;
			this.labelMarKet_co.ImeMode = ImeMode.NoControl;
			this.labelMarKet_co.Location = new Point(21, 15);
			this.labelMarKet_co.Name = "labelMarKet_co";
			this.labelMarKet_co.Size = new Size(65, 12);
			this.labelMarKet_co.TabIndex = 31;
			this.labelMarKet_co.Text = "市场标志：";
			this.labelMarKet_co.Visible = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(194, 222);
			base.Controls.Add(this.groupBoxCO);
			base.Name = "CrazyOrder";
			this.Text = "CrazyOrder";
			base.Load += new EventHandler(this.Crazy_Order_Load);
			base.KeyUp += new KeyEventHandler(this.CrazyOrder_KeyUp);
			this.groupBoxCO.ResumeLayout(false);
			this.groupBoxCO.PerformLayout();
			this.panel_co.ResumeLayout(false);
			((ISupportInitialize)this.numericQty_co).EndInit();
			((ISupportInitialize)this.numericPrice_co).EndInit();
			base.ResumeLayout(false);
		}
		public CrazyOrder()
		{
			this.InitializeComponent();
			Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			SubmitOrderOperation expr_64 = OperationManager.GetInstance().submitOrderOperation;
			expr_64.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Combine(expr_64.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
		}
		public static CrazyOrder Instance()
		{
			if (CrazyOrder.instance == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(CrazyOrder));
				try
				{
					if (CrazyOrder.instance == null)
					{
						CrazyOrder.instance = new CrazyOrder();
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return CrazyOrder.instance;
		}
		private void Crazy_Order_Load(object sender, EventArgs e)
		{
			this.groupBoxCO.BackColor = Color.Transparent;
			this.groupBoxCO.BackgroundImage = ControlLayout.SkinImage;
			this.groupBoxCO.BackgroundImageLayout = ImageLayout.Stretch;
			foreach (Control control in this.groupBoxCO.Controls)
			{
				if (control is Label)
				{
					control.BackColor = Color.Transparent;
				}
			}
			this.SetControlText();
			this.TitleInfo = Global.UserID + "---";
			string @string = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_Str");
			this.Text = @string;
			this.ComboCommodityLoad();
			if (Global.MarketHT.Count > 1)
			{
				this.ComboMarKetLoad();
			}
			else
			{
				this.tbTranc_co.Text = Global.FirmID;
				this.ComboTrancLoad();
			}
			this.comboCommodity_co.Focus();
			ScaleForm.ScaleForms(this);
		}
		private void SetControlText()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.groupBoxCO.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder");
			this.labTrancCode_co.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode_co.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelMarKet_co.Text = Global.M_ResourceManager.GetString("TradeStr_LabelMarKet");
			this.labTrancCode_co.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode_co.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labPrice_co.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice");
			this.labQty_co.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
			this.btn_BL.Text = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_btn_BL");
			this.btn_BO.Text = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_btn_BO");
			this.btn_SL.Text = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_btn_SL");
			this.btn_SO.Text = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_btn_SO");
			if (Global.HTConfig.ContainsKey("DisplaySwitch") && Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
			{
				if (IniData.GetInstance().AutoPrice)
				{
					this.labPrice_co.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice1");
				}
				else
				{
					this.labPrice_co.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice2");
				}
				this.labPrice_co.Font = new Font("宋体", 10.5f, FontStyle.Bold);
				this.labPrice_co.ForeColor = Color.DarkOrange;
			}
			this.BackgroundImage = ControlLayout.SkinImage;
			if (Global.CustomerCount < 2)
			{
				this.comboTranc_co.Visible = false;
				this.tbTranc_co.Size = new Size(70, 20);
			}
		}
		private void ComboTrancLoad()
		{
			this.comboTranc_co.Items.Clear();
			try
			{
				if (OperationManager.GetInstance().myTransactionsList.Count > 1)
				{
					using (List<string>.Enumerator enumerator = OperationManager.GetInstance().myTransactionsList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (current != OperationManager.GetInstance().StrAll)
							{
								this.comboTranc_co.Items.Add(current.Substring(current.Length - 2));
							}
						}
						goto IL_EB;
					}
				}
				foreach (string current2 in OperationManager.GetInstance().transactionsList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.comboTranc_co.Items.Add(current2.Substring(current2.Length - 2));
					}
				}
				IL_EB:;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
			if (this.comboTranc_co.Items.Count > 0)
			{
				this.comboTranc_co.SelectedIndex = 0;
			}
		}
		private void ComboCommodityLoad()
		{
			this.comboCommodity_co.Items.Clear();
			try
			{
				if (OperationManager.GetInstance().myCommodityList.Count > 1)
				{
					using (List<string>.Enumerator enumerator = OperationManager.GetInstance().myCommodityList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (current != OperationManager.GetInstance().StrAll)
							{
								this.comboCommodity_co.Items.Add(current);
							}
						}
						goto IL_D1;
					}
				}
				foreach (string current2 in OperationManager.GetInstance().commodityList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.comboCommodity_co.Items.Add(current2);
					}
				}
				IL_D1:;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "获取商品信息错误：" + ex.Message);
			}
			if (this.comboCommodity_co.Items.Count > 0)
			{
				this.comboCommodity_co.SelectedIndex = 0;
			}
		}
		private void ComboMarKetLoad()
		{
			ArrayList arrayList = new ArrayList();
			if (Global.MarketHT != null)
			{
				foreach (DictionaryEntry dictionaryEntry in Global.MarketHT)
				{
					MarkeInfo markeInfo = (MarkeInfo)dictionaryEntry.Value;
					if (markeInfo != null)
					{
						arrayList.Add(new AddValue(markeInfo.ShortName, markeInfo.MarketID));
					}
				}
				this.comboMarKet_co.DisplayMember = "Display";
				this.comboMarKet_co.ValueMember = "Value";
				this.comboMarKet_co.DataSource = null;
				this.comboMarKet_co.DataSource = arrayList;
			}
			this.comboMarKet_co.SelectedIndex = 0;
			this.labelMarKet_co.Visible = true;
			this.comboMarKet_co.Visible = true;
			this.tbTranc_co.Visible = false;
			this.labTrancCode_co.Visible = false;
			this.comboTranc_co.Visible = false;
		}
		private void ButOrder()
		{
			if (this.submitOrderInfo == null)
			{
				this.submitOrderInfo = new SubmitOrderInfo();
			}
			this.submitOrderInfo.commodityID = this.comboCommodity_co.Text.Trim();
			this.submitOrderInfo.customerID = this.tbTranc_co.Text + this.comboTranc_co.Text;
			this.submitOrderInfo.B_SType = this.BuySell;
			this.submitOrderInfo.O_LType = this.SettleBasis;
			this.submitOrderInfo.closeMode = 1;
			double num = Tools.StrToDouble(this.numericPrice_co.Text, -1.0);
			if (num <= 0.0)
			{
				this.numericPrice_co.Focus();
				this.Text = this.TitleInfo + this.ErrorPriceMassage;
				return;
			}
			this.submitOrderInfo.price = num;
			int num2 = Tools.StrToInt(this.numericQty_co.Text, 0);
			if (num2 > 0)
			{
				this.submitOrderInfo.qty = num2;
				OperationManager.GetInstance().orderOperation.orderType = OrderType.CrazyOrder;
				OperationManager.GetInstance().submitOrderOperation.SubmitOrderInfo(this.submitOrderInfo, 0);
				return;
			}
			this.numericQty_co.Focus();
			this.Text = this.TitleInfo + this.NumIsNotZero;
		}
		private void orderEnable(bool enable)
		{
			if (this.isBOFocused)
			{
				this.btn_BO.Enabled = enable;
				return;
			}
			if (this.isBLFocused)
			{
				this.btn_BL.Enabled = enable;
				return;
			}
			if (this.isSOFocused)
			{
				this.btn_SO.Enabled = enable;
				return;
			}
			if (this.isSLFocused)
			{
				this.btn_SL.Enabled = enable;
			}
		}
		private void Order(object _orderRequestVO)
		{
		}
		private void OrderMessage(ResponseVO responseVO)
		{
		}
		public void SetOrderInfo(string comboCommodityCode, double BuyPrice, double SellPrice)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = comboCommodityCode.IndexOf("_");
			if (num != -1)
			{
				text = comboCommodityCode.Substring(0, num);
				text2 = comboCommodityCode.Substring(num + 1);
			}
			else
			{
				text2 = comboCommodityCode;
			}
			if (text2 != this.comboCommodity_co.Text)
			{
				this.ConnectHQ = true;
			}
			if (Global.MarketHT.Count == 1)
			{
				for (int i = 0; i < this.comboCommodity_co.Items.Count; i++)
				{
					if (text2.Equals(this.comboCommodity_co.Items[i].ToString()))
					{
						this.comboCommodity_co.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.comboMarKet_co.Items.Count; j++)
				{
					AddValue addValue = (AddValue)this.comboMarKet_co.Items[j];
					if (text.Equals(addValue.Value))
					{
						this.comboMarKet_co.SelectedIndex = j;
						break;
					}
				}
				for (int k = 0; k < this.comboCommodity_co.Items.Count; k++)
				{
					if (text2.Equals(this.comboCommodity_co.Items[k].ToString()))
					{
						this.comboCommodity_co.SelectedIndex = k;
						break;
					}
				}
			}
			this.numericQty_co.Focus();
		}
		private void btn_BO_Click(object sender, EventArgs e)
		{
			this.buttonClick = true;
			this.BuySell = 1;
			this.SettleBasis = 1;
			this.ButOrder();
			this.isBtnFocused = true;
			this.isBOFocused = true;
			this.isBLFocused = false;
			this.isSOFocused = false;
			this.isSLFocused = false;
		}
		private void btn_SO_Click(object sender, EventArgs e)
		{
			this.buttonClick = true;
			this.BuySell = 2;
			this.SettleBasis = 1;
			this.ButOrder();
			this.isBtnFocused = true;
			this.isBOFocused = false;
			this.isBLFocused = false;
			this.isSOFocused = true;
			this.isSLFocused = false;
		}
		private void btn_BL_Click(object sender, EventArgs e)
		{
			this.buttonClick = true;
			this.BuySell = 1;
			this.SettleBasis = 2;
			this.ButOrder();
			this.isBtnFocused = true;
			this.isBOFocused = false;
			this.isBLFocused = true;
			this.isSOFocused = false;
			this.isSLFocused = false;
		}
		private void btn_SL_Click(object sender, EventArgs e)
		{
			this.buttonClick = true;
			this.BuySell = 2;
			this.SettleBasis = 2;
			this.ButOrder();
			this.isBtnFocused = true;
			this.isBOFocused = false;
			this.isBLFocused = false;
			this.isSOFocused = false;
			this.isSLFocused = true;
		}
		private void CrazyOrder_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.buttonClick)
			{
				this.buttonClick = false;
				return;
			}
			if (e.KeyValue == 13)
			{
				if (this.comboTranc_co.Focused || this.comboMarKet_co.Focused)
				{
					this.comboCommodity_co.Focus();
				}
				else
				{
					if (this.comboCommodity_co.Focused)
					{
						this.numericPrice_co.Focus();
					}
					else
					{
						if (this.numericPrice_co.Focused)
						{
							this.numericQty_co.Focus();
						}
						else
						{
							if (this.numericQty_co.Focused)
							{
								if (this.isBtnFocused)
								{
									if (this.isBOFocused && this.btn_BO.Enabled)
									{
										this.btn_BO.Focus();
									}
									else
									{
										if (this.isBLFocused && this.btn_BL.Enabled)
										{
											this.btn_BL.Focus();
										}
										else
										{
											if (this.isSOFocused && this.btn_SO.Enabled)
											{
												this.btn_SO.Focus();
											}
											else
											{
												if (this.isSLFocused && this.btn_SL.Enabled)
												{
													this.btn_SL.Focus();
												}
											}
										}
									}
								}
								else
								{
									if (this.btn_BO.Enabled)
									{
										this.btn_BO.Focus();
									}
									else
									{
										if (this.btn_BL.Enabled)
										{
											this.btn_BL.Focus();
										}
										else
										{
											if (this.btn_SO.Enabled)
											{
												this.btn_SO.Focus();
											}
											else
											{
												if (this.btn_SL.Enabled)
												{
													this.btn_SL.Focus();
												}
											}
										}
									}
								}
							}
							else
							{
								if (this.comboMarKet_co.Visible)
								{
									this.comboMarKet_co.Focus();
								}
								else
								{
									if (this.comboTranc_co.Visible)
									{
										this.comboTranc_co.Focus();
									}
									else
									{
										this.comboCommodity_co.Focus();
									}
								}
							}
						}
					}
				}
			}
			this.displayInfo = true;
		}
		private void numericPrice_co_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericQty_co_KeyUp(object sender, KeyEventArgs e)
		{
			Global.QtyKeyUp(sender, e);
		}
		private void numericPrice_co_Enter(object sender, EventArgs e)
		{
			this.numericPrice_co.Select(0, this.numericPrice_co.Value.ToString().Length);
			WaitCallback callBack = new WaitCallback(this.GetCom);
			ThreadPool.QueueUserWorkItem(callBack, this.comboCommodity_co.Text);
		}
		private void numericQty_co_Enter(object sender, EventArgs e)
		{
			this.numericQty_co.Select(0, this.numericQty_co.Value.ToString().Length);
			WaitCallback callBack = new WaitCallback(this.GetCom);
			ThreadPool.QueueUserWorkItem(callBack, this.comboCommodity_co.Text);
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void GetCom(object o)
		{
		}
		private void CommdityInfo(CommodityInfo commodityInfo)
		{
			if (commodityInfo != null)
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
				string.Concat(new object[]
				{
					string2,
					commodityInfo.CommodityName,
					"  ",
					@string,
					commodityInfo.SpreadDown,
					" – ",
					commodityInfo.SpreadUp
				});
				if (commodityInfo.Spread < 0.1)
				{
					this.numericPrice_co.DecimalPlaces = 2;
				}
				else
				{
					if (commodityInfo.Spread < 1.0)
					{
						this.numericPrice_co.DecimalPlaces = 1;
					}
					else
					{
						this.numericPrice_co.DecimalPlaces = 0;
					}
				}
				try
				{
					this.numericPrice_co.Increment = (decimal)commodityInfo.Spread;
				}
				catch (Exception)
				{
				}
				if (commodityInfo.MinQty < 0.1)
				{
					this.numericQty_co.DecimalPlaces = 2;
				}
				else
				{
					if (commodityInfo.MinQty < 1.0)
					{
						this.numericQty_co.DecimalPlaces = 1;
					}
					else
					{
						this.numericQty_co.DecimalPlaces = 0;
					}
				}
				try
				{
					this.numericQty_co.Increment = (decimal)commodityInfo.MinQty;
				}
				catch (Exception)
				{
				}
			}
		}
		private void labPrice_co_DoubleClick(object sender, EventArgs e)
		{
		}
		private void comboCommodity_co_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numericPrice_co.Value = 0m;
			this.numericQty_co.Value = 0m;
			this.numericPrice_co.Value = 0m;
			this.labelLargestTN_co.Text = "";
		}
		private void OrderMessage(long retCode, string retMessage)
		{
			try
			{
				if (OperationManager.GetInstance().orderOperation.orderType == OrderType.CrazyOrder)
				{
					this.OrderMessageInfo = new CrazyOrder.OrderMessageInfoCallBack(this.OrderInfoMessage);
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
			this.comboCommodity_co.Focus();
			if (IniData.GetInstance().ClearData)
			{
				if (this.numericPrice_co.Enabled)
				{
					this.numericPrice_co.Value = 0m;
				}
				this.numericQty_co.Value = 0m;
			}
			if (retCode == 0L)
			{
				OperationManager.GetInstance().orderOperation.IsChangePrice = false;
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(OperationManager.GetInstance().SussceOrder, Global.RightColor, true);
					return;
				}
			}
			else
			{
				if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
				{
					MessageBox.Show(retMessage, OperationManager.GetInstance().ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(retMessage, Global.ErrorColor, true);
				}
			}
		}
		private void comboCommodity_co_TextChanged(object sender, EventArgs e)
		{
			decimal commoditySpread = OperationManager.GetInstance().orderOperation.GetCommoditySpread(this.comboCommodity_co.Text);
			this.numericPrice_co.Increment = commoditySpread;
			this.numericPrice_co.Increment = commoditySpread;
			int decimalPlaces = OperationManager.GetInstance().orderOperation.GetDecimalPlaces(commoditySpread);
			this.numericPrice_co.DecimalPlaces = decimalPlaces;
			this.numericPrice_co.DecimalPlaces = decimalPlaces;
		}
	}
}
