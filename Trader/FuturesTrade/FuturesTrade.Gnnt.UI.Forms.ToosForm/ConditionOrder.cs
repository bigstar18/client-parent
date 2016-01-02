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
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class ConditionOrder : Form
	{
		private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private OperationManager operationManager = OperationManager.GetInstance();
		private ConditionOrder.PromptLargestTradeNumCallBack PromptLargestTradeNum;
		private ConditionOrder.OrderMessageInfoCallBack OrderMessageInfo;
		private static volatile ConditionOrder instance;
		private bool isClose;
		private IContainer components;
		internal GroupBox groupBoxSimple;
		private Label labelCommodityIDS;
		private ComboBox comboBoxCommodityID;
		private DateTimePicker dateTimePicker1;
		private Label labelTimeS;
		private Label labelQtyS;
		private Label labelPriceS;
		private NumericUpDown numericQtyS;
		private NumericUpDown numericPriceS;
		private GroupBox groupBox3;
		private RadioButton radioS_S;
		private RadioButton radioS_B;
		private Label labelOLS;
		private Label labelBSS;
		private NumericUpDown numericConPriceS;
		private GroupBox groupBox4;
		private RadioButton radioS_L;
		private RadioButton radioS_O;
		private Label labelConditionS;
		private Button btnCancel;
		private Button btnConmmit;
		private ComboBox comboOperatorS;
		private ComboBox comboConTypeOrderS;
		private Label labelLargestTN;
		public ConditionOrder()
		{
			this.InitializeComponent();
			this.operationManager.submitConOrderOperation.ConOrderMessage = new SubmitConOrderOperation.ConOrderMessageCallBack(this.OrderMessage);
			this.operationManager.conOrderOperation.setLargestTN = new ConOrderOperation.SetLargestTNCallBack(this.SetLargestTNInfo);
			Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
		}
		public static ConditionOrder Instance()
		{
			if (ConditionOrder.instance == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(ConditionOrder));
				try
				{
					if (ConditionOrder.instance == null)
					{
						ConditionOrder.instance = new ConditionOrder();
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return ConditionOrder.instance;
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void numericConPriceS_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericPriceS_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericQtyS_KeyUp(object sender, KeyEventArgs e)
		{
			Global.QtyKeyUp(sender, e);
		}
		public void SetOrderInfo(string comboCommodityCode, double BuyPrice, double SellPrice)
		{
			string arg_05_0 = string.Empty;
			string text = string.Empty;
			int num = comboCommodityCode.IndexOf("_");
			if (num != -1)
			{
				comboCommodityCode.Substring(0, num);
				text = comboCommodityCode.Substring(num + 1);
			}
			else
			{
				text = comboCommodityCode;
			}
			if (Global.MarketHT.Count == 1)
			{
				int i = 0;
				while (i < this.comboBoxCommodityID.Items.Count)
				{
					if (text.Equals(this.comboBoxCommodityID.Items[i].ToString()))
					{
						this.comboBoxCommodityID.SelectedIndex = i;
						if (this.radioS_B.Checked)
						{
							this.numericPriceS.Value = (decimal)SellPrice;
							return;
						}
						this.numericPriceS.Value = (decimal)BuyPrice;
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			int j = 0;
			while (j < this.comboBoxCommodityID.Items.Count)
			{
				if (text.Equals(this.comboBoxCommodityID.Items[j].ToString()))
				{
					this.comboBoxCommodityID.SelectedIndex = j;
					if (this.radioS_B.Checked)
					{
						this.numericPriceS.Value = (decimal)SellPrice;
						return;
					}
					this.numericPriceS.Value = (decimal)BuyPrice;
					return;
				}
				else
				{
					j++;
				}
			}
		}
		public void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboBoxCommodityID.Items.Clear();
			foreach (string current in commodityIDList)
			{
				if (current != this.operationManager.StrAll)
				{
					this.comboBoxCommodityID.Items.Add(current);
				}
			}
			this.comboBoxCommodityID.SelectedIndex = 0;
		}
		private void ConditionOrder_FormClosing(object sender, FormClosingEventArgs e)
		{
		}
		public void CloseForm()
		{
			this.isClose = true;
			base.Close();
			base.Dispose();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void btnConmmit_Click(object sender, EventArgs e)
		{
			this.SubmintConOrder();
		}
		private void SubmintConOrder()
		{
			SubmitConOrderInfo submitConOrderInfo = new SubmitConOrderInfo();
			submitConOrderInfo.commodityID = this.comboBoxCommodityID.Text;
			submitConOrderInfo.customerID = Global.UserID;
			if (this.radioS_S.Checked)
			{
				submitConOrderInfo.B_SType = 2;
			}
			if (this.radioS_L.Checked)
			{
				submitConOrderInfo.O_LType = 2;
			}
			submitConOrderInfo.datetime = this.dateTimePicker1.Text;
			submitConOrderInfo.conprice = Tools.StrToDouble(this.numericConPriceS.Value.ToString(), 0.0);
			submitConOrderInfo.price = Tools.StrToDouble(this.numericPriceS.Value.ToString(), 0.0);
			submitConOrderInfo.qty = Tools.StrToInt(this.numericQtyS.Value.ToString(), 0);
			if (this.comboConTypeOrderS.SelectedIndex == 0)
			{
				submitConOrderInfo.contype = 2;
			}
			else
			{
				if (this.comboConTypeOrderS.SelectedIndex == 1)
				{
					submitConOrderInfo.contype = 3;
				}
				else
				{
					if (this.comboConTypeOrderS.SelectedIndex == 2)
					{
						submitConOrderInfo.contype = 1;
					}
				}
			}
			if (this.comboOperatorS.SelectedIndex == 0)
			{
				submitConOrderInfo.conoperator = -1;
			}
			else
			{
				if (this.comboOperatorS.SelectedIndex == 1)
				{
					submitConOrderInfo.conoperator = 1;
				}
				else
				{
					if (this.comboOperatorS.SelectedIndex == 2)
					{
						submitConOrderInfo.conoperator = 0;
					}
					else
					{
						if (this.comboOperatorS.SelectedIndex == 3)
						{
							submitConOrderInfo.conoperator = -2;
						}
						else
						{
							if (this.comboOperatorS.SelectedIndex == 4)
							{
								submitConOrderInfo.conoperator = 2;
							}
						}
					}
				}
			}
			this.operationManager.submitConOrderOperation.ButtonConOrderComm(submitConOrderInfo);
		}
		private void comboBoxCommodityID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numericPriceS.Value = 0m;
			this.numericQtyS.Value = 0m;
			this.numericConPriceS.Value = 0m;
			int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboBoxCommodityID.Text);
			this.SetRadioEnable(currentTradeMode);
		}
		private void comboBoxCommodityID_TextChanged(object sender, EventArgs e)
		{
			decimal commoditySpread = this.operationManager.orderOperation.GetCommoditySpread(this.comboBoxCommodityID.Text);
			this.numericPriceS.Increment = commoditySpread;
			this.numericConPriceS.Increment = commoditySpread;
			int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
			this.numericPriceS.DecimalPlaces = decimalPlaces;
			this.numericConPriceS.DecimalPlaces = decimalPlaces;
		}
		private void SetRadioEnable(int currentTradeMode)
		{
			switch (currentTradeMode)
			{
			case 0:
				this.radioS_O.Enabled = true;
				this.radioS_L.Enabled = true;
				this.radioS_S.Enabled = true;
				this.radioS_B.Enabled = true;
				return;
			case 1:
				if (this.radioS_B.Checked)
				{
					this.radioS_O.Enabled = false;
					this.radioS_L.Checked = true;
					return;
				}
				this.radioS_L.Enabled = false;
				this.radioS_O.Checked = true;
				return;
			case 2:
				if (this.radioS_B.Checked)
				{
					this.radioS_L.Enabled = false;
					this.radioS_O.Checked = true;
					return;
				}
				this.radioS_O.Enabled = false;
				this.radioS_L.Checked = true;
				return;
			case 3:
				this.radioS_S.Enabled = false;
				this.radioS_B.Checked = true;
				this.radioS_L.Enabled = false;
				this.radioS_O.Checked = true;
				return;
			case 4:
				this.radioS_B.Enabled = false;
				this.radioS_S.Checked = true;
				this.radioS_L.Enabled = false;
				this.radioS_O.Checked = true;
				return;
			default:
				return;
			}
		}
		private void comboBoxCommodityID_KeyDown(object sender, KeyEventArgs e)
		{
			this.operationManager.conOrderOperation.ComboxKeyDown(e);
		}
		private void radioS_B_CheckedChanged(object sender, EventArgs e)
		{
			int buysell = 0;
			if (this.radioS_S.Checked)
			{
				buysell = 1;
			}
			decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
			this.numericPriceS.Value = bSPrice;
		}
		private void numericQtyS_Enter(object sender, EventArgs e)
		{
			this.numericQtyS.Select(0, this.numericQtyS.Value.ToString().Length);
			this.labelLargestTN.ForeColor = Global.LightColor;
			short num = 1;
			if (this.radioS_S.Checked)
			{
				num = 2;
			}
			short num2 = 1;
			if (this.radioS_L.Checked)
			{
				num2 = 2;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Commodity", this.comboBoxCommodityID.Text);
			hashtable.Add("B_SType", num);
			hashtable.Add("O_LType", num2);
			hashtable.Add("numericPrice", Convert.ToDouble(this.numericPriceS.Value));
			hashtable.Add("tbTranc_comboTranc", Global.UserID + "00");
			this.operationManager.conOrderOperation.GetNumericQtyThread(hashtable);
		}
		private void SetLargestTNInfo(string text, int colorFlag)
		{
			try
			{
				this.PromptLargestTradeNum = new ConditionOrder.PromptLargestTradeNumCallBack(this.LargestTNInfo);
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
		private void labelLargestTN_Click(object sender, EventArgs e)
		{
			long largestTradeNum = this.operationManager.orderOperation.GetLargestTradeNum(this.labelLargestTN.Text);
			this.numericQtyS.Value = Global.ToDecimal(largestTradeNum.ToString());
		}
		private void ConditionOrder_Load(object sender, EventArgs e)
		{
			this.ComboCommodityLoad();
			this.comboConTypeOrderS.SelectedIndex = 0;
			this.comboOperatorS.SelectedIndex = 0;
			ScaleForm.ScaleForms(this);
		}
		private void ComboCommodityLoad()
		{
			this.comboBoxCommodityID.Items.Clear();
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
								this.comboBoxCommodityID.Items.Add(current);
							}
						}
						goto IL_D1;
					}
				}
				foreach (string current2 in OperationManager.GetInstance().commodityList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.comboBoxCommodityID.Items.Add(current2);
					}
				}
				IL_D1:;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "获取商品信息错误：" + ex.Message);
			}
			if (this.comboBoxCommodityID.Items.Count > 0)
			{
				this.comboBoxCommodityID.SelectedIndex = 0;
			}
		}
		private void OrderMessage(long retCode, string retMessage)
		{
			try
			{
				this.OrderMessageInfo = new ConditionOrder.OrderMessageInfoCallBack(this.OrderInfoMessage);
				this.HandleCreated();
				base.Invoke(this.OrderMessageInfo, new object[]
				{
					retCode,
					retMessage
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void OrderInfoMessage(long retCode, string retMessage)
		{
			this.comboBoxCommodityID.Focus();
			if (IniData.GetInstance().ClearData)
			{
				this.numericConPriceS.Value = 0m;
				this.numericPriceS.Value = 0m;
				this.numericQtyS.Text = "";
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
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(retMessage, Global.ErrorColor, true);
				}
			}
		}
		private void numericPriceS_Enter(object sender, EventArgs e)
		{
			if (this.numericPriceS.Value == 0m)
			{
				this.numericPriceS.Select(0, this.numericPriceS.Text.Length);
			}
			this.operationManager.orderOperation.GetCommoditySpread(this.comboBoxCommodityID.Text);
		}
		protected override void Dispose(bool disposing)
		{
			if (ConditionOrder.instance != null)
			{
				ConditionOrder.instance = null;
			}
			Global.SetOrderInfo -= new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.groupBoxSimple = new GroupBox();
			this.labelLargestTN = new Label();
			this.labelCommodityIDS = new Label();
			this.comboBoxCommodityID = new ComboBox();
			this.dateTimePicker1 = new DateTimePicker();
			this.labelTimeS = new Label();
			this.labelQtyS = new Label();
			this.labelPriceS = new Label();
			this.numericQtyS = new NumericUpDown();
			this.numericPriceS = new NumericUpDown();
			this.groupBox3 = new GroupBox();
			this.radioS_S = new RadioButton();
			this.radioS_B = new RadioButton();
			this.labelOLS = new Label();
			this.labelBSS = new Label();
			this.numericConPriceS = new NumericUpDown();
			this.groupBox4 = new GroupBox();
			this.radioS_L = new RadioButton();
			this.radioS_O = new RadioButton();
			this.labelConditionS = new Label();
			this.btnCancel = new Button();
			this.btnConmmit = new Button();
			this.comboOperatorS = new ComboBox();
			this.comboConTypeOrderS = new ComboBox();
			this.groupBoxSimple.SuspendLayout();
			((ISupportInitialize)this.numericQtyS).BeginInit();
			((ISupportInitialize)this.numericPriceS).BeginInit();
			this.groupBox3.SuspendLayout();
			((ISupportInitialize)this.numericConPriceS).BeginInit();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxSimple.Controls.Add(this.labelLargestTN);
			this.groupBoxSimple.Controls.Add(this.labelCommodityIDS);
			this.groupBoxSimple.Controls.Add(this.comboBoxCommodityID);
			this.groupBoxSimple.Controls.Add(this.dateTimePicker1);
			this.groupBoxSimple.Controls.Add(this.labelTimeS);
			this.groupBoxSimple.Controls.Add(this.labelQtyS);
			this.groupBoxSimple.Controls.Add(this.labelPriceS);
			this.groupBoxSimple.Controls.Add(this.numericQtyS);
			this.groupBoxSimple.Controls.Add(this.numericPriceS);
			this.groupBoxSimple.Controls.Add(this.groupBox3);
			this.groupBoxSimple.Controls.Add(this.labelOLS);
			this.groupBoxSimple.Controls.Add(this.labelBSS);
			this.groupBoxSimple.Controls.Add(this.numericConPriceS);
			this.groupBoxSimple.Controls.Add(this.groupBox4);
			this.groupBoxSimple.Controls.Add(this.labelConditionS);
			this.groupBoxSimple.Controls.Add(this.btnCancel);
			this.groupBoxSimple.Controls.Add(this.btnConmmit);
			this.groupBoxSimple.Controls.Add(this.comboOperatorS);
			this.groupBoxSimple.Controls.Add(this.comboConTypeOrderS);
			this.groupBoxSimple.Dock = DockStyle.Fill;
			this.groupBoxSimple.Font = new Font("宋体", 9f);
			this.groupBoxSimple.Location = new Point(0, 0);
			this.groupBoxSimple.Margin = new Padding(0);
			this.groupBoxSimple.Name = "groupBoxSimple";
			this.groupBoxSimple.Padding = new Padding(0);
			this.groupBoxSimple.Size = new Size(324, 277);
			this.groupBoxSimple.TabIndex = 40;
			this.groupBoxSimple.TabStop = false;
			this.groupBoxSimple.Text = "条件单设置";
			this.labelLargestTN.AutoSize = true;
			this.labelLargestTN.ImeMode = ImeMode.NoControl;
			this.labelLargestTN.Location = new Point(161, 211);
			this.labelLargestTN.Name = "labelLargestTN";
			this.labelLargestTN.Size = new Size(77, 12);
			this.labelLargestTN.TabIndex = 31;
			this.labelLargestTN.Text = "最大可交易量";
			this.labelLargestTN.Click += new EventHandler(this.labelLargestTN_Click);
			this.labelCommodityIDS.AutoSize = true;
			this.labelCommodityIDS.Location = new Point(10, 34);
			this.labelCommodityIDS.Name = "labelCommodityIDS";
			this.labelCommodityIDS.Size = new Size(41, 12);
			this.labelCommodityIDS.TabIndex = 28;
			this.labelCommodityIDS.Text = "商品：";
			this.comboBoxCommodityID.FormattingEnabled = true;
			this.comboBoxCommodityID.Location = new Point(59, 31);
			this.comboBoxCommodityID.Name = "comboBoxCommodityID";
			this.comboBoxCommodityID.Size = new Size(81, 20);
			this.comboBoxCommodityID.TabIndex = 1;
			this.comboBoxCommodityID.SelectedIndexChanged += new EventHandler(this.comboBoxCommodityID_SelectedIndexChanged);
			this.comboBoxCommodityID.TextChanged += new EventHandler(this.comboBoxCommodityID_TextChanged);
			this.comboBoxCommodityID.KeyDown += new KeyEventHandler(this.comboBoxCommodityID_KeyDown);
			this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
			this.dateTimePicker1.DropDownAlign = LeftRightAlignment.Right;
			this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new Point(203, 31);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new Size(88, 21);
			this.dateTimePicker1.TabIndex = 2;
			this.labelTimeS.AutoSize = true;
			this.labelTimeS.Location = new Point(147, 36);
			this.labelTimeS.Name = "labelTimeS";
			this.labelTimeS.Size = new Size(53, 12);
			this.labelTimeS.TabIndex = 26;
			this.labelTimeS.Text = "有效期：";
			this.labelQtyS.AutoSize = true;
			this.labelQtyS.Location = new Point(161, 184);
			this.labelQtyS.Name = "labelQtyS";
			this.labelQtyS.Size = new Size(41, 12);
			this.labelQtyS.TabIndex = 25;
			this.labelQtyS.Text = "数量：";
			this.labelPriceS.AutoSize = true;
			this.labelPriceS.Location = new Point(10, 184);
			this.labelPriceS.Name = "labelPriceS";
			this.labelPriceS.Size = new Size(41, 12);
			this.labelPriceS.TabIndex = 24;
			this.labelPriceS.Text = "价格：";
			NumericUpDown arg_6D5_0 = this.numericQtyS;
			int[] array = new int[4];
			array[0] = 10;
			arg_6D5_0.Increment = new decimal(array);
			this.numericQtyS.Location = new Point(210, 179);
			NumericUpDown arg_70F_0 = this.numericQtyS;
			int[] array2 = new int[4];
			array2[0] = 999999;
			arg_70F_0.Maximum = new decimal(array2);
			this.numericQtyS.Name = "numericQtyS";
			this.numericQtyS.Size = new Size(81, 21);
			this.numericQtyS.TabIndex = 9;
			this.numericQtyS.Enter += new EventHandler(this.numericQtyS_Enter);
			this.numericQtyS.KeyUp += new KeyEventHandler(this.numericQtyS_KeyUp);
			NumericUpDown arg_78B_0 = this.numericPriceS;
			int[] array3 = new int[4];
			array3[0] = 10;
			arg_78B_0.Increment = new decimal(array3);
			this.numericPriceS.Location = new Point(59, 180);
			NumericUpDown arg_7C2_0 = this.numericPriceS;
			int[] array4 = new int[4];
			array4[0] = 999999;
			arg_7C2_0.Maximum = new decimal(array4);
			this.numericPriceS.Name = "numericPriceS";
			this.numericPriceS.Size = new Size(81, 21);
			this.numericPriceS.TabIndex = 8;
			this.numericPriceS.Enter += new EventHandler(this.numericPriceS_Enter);
			this.numericPriceS.KeyUp += new KeyEventHandler(this.numericPriceS_KeyUp);
			this.groupBox3.BackColor = Color.Transparent;
			this.groupBox3.Controls.Add(this.radioS_S);
			this.groupBox3.Controls.Add(this.radioS_B);
			this.groupBox3.Location = new Point(59, 95);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(139, 37);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.radioS_S.AutoSize = true;
			this.radioS_S.ForeColor = Color.Green;
			this.radioS_S.ImeMode = ImeMode.NoControl;
			this.radioS_S.Location = new Point(69, 14);
			this.radioS_S.Name = "radioS_S";
			this.radioS_S.Size = new Size(47, 16);
			this.radioS_S.TabIndex = 2;
			this.radioS_S.Text = "卖出";
			this.radioS_S.CheckedChanged += new EventHandler(this.radioS_B_CheckedChanged);
			this.radioS_B.AutoSize = true;
			this.radioS_B.Checked = true;
			this.radioS_B.ForeColor = Color.Red;
			this.radioS_B.ImeMode = ImeMode.NoControl;
			this.radioS_B.Location = new Point(10, 14);
			this.radioS_B.Name = "radioS_B";
			this.radioS_B.Size = new Size(47, 16);
			this.radioS_B.TabIndex = 1;
			this.radioS_B.TabStop = true;
			this.radioS_B.Text = "买入";
			this.radioS_B.CheckedChanged += new EventHandler(this.radioS_B_CheckedChanged);
			this.labelOLS.AutoSize = true;
			this.labelOLS.Location = new Point(10, 147);
			this.labelOLS.Name = "labelOLS";
			this.labelOLS.Size = new Size(41, 12);
			this.labelOLS.TabIndex = 19;
			this.labelOLS.Text = "开平：";
			this.labelBSS.AutoSize = true;
			this.labelBSS.Location = new Point(10, 108);
			this.labelBSS.Name = "labelBSS";
			this.labelBSS.Size = new Size(41, 12);
			this.labelBSS.TabIndex = 18;
			this.labelBSS.Text = "买卖：";
			NumericUpDown arg_AD2_0 = this.numericConPriceS;
			int[] array5 = new int[4];
			array5[0] = 10;
			arg_AD2_0.Increment = new decimal(array5);
			this.numericConPriceS.Location = new Point(210, 67);
			NumericUpDown arg_B0C_0 = this.numericConPriceS;
			int[] array6 = new int[4];
			array6[0] = 999999;
			arg_B0C_0.Maximum = new decimal(array6);
			this.numericConPriceS.Name = "numericConPriceS";
			this.numericConPriceS.Size = new Size(81, 21);
			this.numericConPriceS.TabIndex = 5;
			this.numericConPriceS.KeyUp += new KeyEventHandler(this.numericConPriceS_KeyUp);
			this.groupBox4.BackColor = Color.Transparent;
			this.groupBox4.Controls.Add(this.radioS_L);
			this.groupBox4.Controls.Add(this.radioS_O);
			this.groupBox4.Location = new Point(59, 132);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(139, 37);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.radioS_L.AutoSize = true;
			this.radioS_L.ForeColor = Color.Black;
			this.radioS_L.ImeMode = ImeMode.NoControl;
			this.radioS_L.Location = new Point(69, 14);
			this.radioS_L.Name = "radioS_L";
			this.radioS_L.Size = new Size(47, 16);
			this.radioS_L.TabIndex = 2;
			this.radioS_L.Text = "转让";
			this.radioS_O.AutoSize = true;
			this.radioS_O.Checked = true;
			this.radioS_O.ForeColor = Color.Black;
			this.radioS_O.ImeMode = ImeMode.NoControl;
			this.radioS_O.Location = new Point(10, 14);
			this.radioS_O.Name = "radioS_O";
			this.radioS_O.Size = new Size(47, 16);
			this.radioS_O.TabIndex = 1;
			this.radioS_O.TabStop = true;
			this.radioS_O.Text = "订立";
			this.labelConditionS.AutoSize = true;
			this.labelConditionS.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.labelConditionS.Location = new Point(10, 71);
			this.labelConditionS.Name = "labelConditionS";
			this.labelConditionS.Size = new Size(41, 12);
			this.labelConditionS.TabIndex = 4;
			this.labelConditionS.Text = "条件：";
			this.btnCancel.Location = new Point(200, 233);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(66, 23);
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.btnConmmit.Location = new Point(62, 233);
			this.btnConmmit.Name = "btnConmmit";
			this.btnConmmit.Size = new Size(66, 23);
			this.btnConmmit.TabIndex = 10;
			this.btnConmmit.Text = "提交";
			this.btnConmmit.UseVisualStyleBackColor = true;
			this.btnConmmit.Click += new EventHandler(this.btnConmmit_Click);
			this.comboOperatorS.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboOperatorS.FormattingEnabled = true;
			this.comboOperatorS.Items.AddRange(new object[]
			{
				"<",
				">",
				"=",
				"≤",
				"≥"
			});
			this.comboOperatorS.Location = new Point(141, 67);
			this.comboOperatorS.Name = "comboOperatorS";
			this.comboOperatorS.Size = new Size(57, 20);
			this.comboOperatorS.TabIndex = 4;
			this.comboConTypeOrderS.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboConTypeOrderS.FormattingEnabled = true;
			this.comboConTypeOrderS.Items.AddRange(new object[]
			{
				"买1价",
				"卖1价",
				"最新价"
			});
			this.comboConTypeOrderS.Location = new Point(59, 67);
			this.comboConTypeOrderS.Name = "comboConTypeOrderS";
			this.comboConTypeOrderS.Size = new Size(69, 20);
			this.comboConTypeOrderS.TabIndex = 3;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(324, 277);
			base.Controls.Add(this.groupBoxSimple);
			base.Name = "ConditionOrder";
			this.Text = "条件下单";
			base.FormClosing += new FormClosingEventHandler(this.ConditionOrder_FormClosing);
			base.Load += new EventHandler(this.ConditionOrder_Load);
			this.groupBoxSimple.ResumeLayout(false);
			this.groupBoxSimple.PerformLayout();
			((ISupportInitialize)this.numericQtyS).EndInit();
			((ISupportInitialize)this.numericPriceS).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((ISupportInitialize)this.numericConPriceS).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
