using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary;
using ToolsLibrary.util;
using TPME.Log;
using TradeClientApp.Gnnt.OTC.Library;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC
{
	public class NewOrdersform : Form
	{
		private delegate void SetTextCallback(string text);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private const int WM_KEYDOWN = 256;
		private IContainer components;
		private Label labelTradeType;
		private ComboBox comboBoxCommodity;
		private Label labelCommodity;
		private Label labelQty;
		private Label labelTrader;
		private ComboBox comboBoxTrader;
		private GroupBox groupBox1;
		private GroupBox groupBoxShiJia;
		private TextBox textBoxSHQ;
		private RadioButton radioButtonSell;
		private RadioButton radioButtonBuy;
		private Button buttonCancel;
		private CheckBox checkBoxDianCha;
		private CheckBox checkBoxConfirm;
		private GroupBox groupBoxXianJia;
		private NumericUpDown numericUpDownXPrice;
		private Label labelXPrice;
		private ComboBox comboBoxValidTime;
		private Label labelValidTime;
		private RadioButton radioButtonSellX;
		private RadioButton radioButtonBuyX;
		private Label labelZY;
		private Label labelZS;
		private NumericUpDown numericUpDownZY;
		private NumericUpDown numericUpDownZS;
		private TextBox textBoxZY;
		private TextBox textBoxZS;
		private TextBox textBoxXBuyHQ;
		private TextBox textBoxXSellHQ;
		private CheckBox checkBoxZY;
		private CheckBox checkBoxZS;
		private NumericUpDown numericUpDownDianCha;
		private ComboBox comboBoxTradeType;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabelInfo;
		private Button buttonOrder;
		private NumericUpDown numericUpDownQty;
		private GroupBox groupBoxSubmitMask;
		private Label label1;
		private LoadingCircle loadingCircle1;
		private Button buttonmax;
		private Label labelQtyScope;
		private bool _IsCloseButtonOKOrCancel;
		public DateTime SubmitDateTime;
		private TradeType _OpenTradeType = TradeType.ShiJiaDan;
		private TMainForm _ParentForm;
		private bool RefreshGNFlag = true;
		private string _CurrentCommodityId = string.Empty;
		private string _CurrentBuySell = BuySell.Buy.ToString("d");
		public static string[] TradeTypeStrArr = new string[]
		{
			"<未选>",
			"市价建仓单",
			Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "MENU_LIMITCREATWAREHOUSE"))
		};
		private WaitHandle waitHandles = new AutoResetEvent(true);
		private bool _FailShowDialog = IniData.GetInstance().FailShowDialog;
		private decimal QtyMaxValue = -1m;
		private decimal QtyMinValue = -1m;
		private decimal DianChaMaxValue = -1m;
		private decimal DianChaMinValue = -1m;
		public bool IsCloseButtonOKOrCancel
		{
			get
			{
				return this._IsCloseButtonOKOrCancel;
			}
			set
			{
				this._IsCloseButtonOKOrCancel = value;
			}
		}
		public TradeType OpenTradeType
		{
			get
			{
				return this._OpenTradeType;
			}
			set
			{
				this._OpenTradeType = value;
			}
		}
		public string CurrentCommodityId
		{
			get
			{
				return this._CurrentCommodityId;
			}
			set
			{
				this._CurrentCommodityId = value;
			}
		}
		public string CurrentBuySell
		{
			get
			{
				return this._CurrentBuySell;
			}
			set
			{
				this._CurrentBuySell = value;
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
			this.labelTradeType = new Label();
			this.comboBoxTradeType = new ComboBox();
			this.comboBoxCommodity = new ComboBox();
			this.labelCommodity = new Label();
			this.labelQty = new Label();
			this.labelTrader = new Label();
			this.comboBoxTrader = new ComboBox();
			this.groupBox1 = new GroupBox();
			this.buttonmax = new Button();
			this.numericUpDownQty = new NumericUpDown();
			this.groupBoxXianJia = new GroupBox();
			this.checkBoxZY = new CheckBox();
			this.textBoxXBuyHQ = new TextBox();
			this.checkBoxZS = new CheckBox();
			this.textBoxZY = new TextBox();
			this.textBoxXSellHQ = new TextBox();
			this.textBoxZS = new TextBox();
			this.numericUpDownXPrice = new NumericUpDown();
			this.labelZY = new Label();
			this.labelXPrice = new Label();
			this.labelZS = new Label();
			this.comboBoxValidTime = new ComboBox();
			this.numericUpDownZY = new NumericUpDown();
			this.labelValidTime = new Label();
			this.numericUpDownZS = new NumericUpDown();
			this.radioButtonSellX = new RadioButton();
			this.radioButtonBuyX = new RadioButton();
			this.groupBoxSubmitMask = new GroupBox();
			this.label1 = new Label();
			this.loadingCircle1 = new LoadingCircle();
			this.groupBoxShiJia = new GroupBox();
			this.numericUpDownDianCha = new NumericUpDown();
			this.checkBoxDianCha = new CheckBox();
			this.textBoxSHQ = new TextBox();
			this.radioButtonSell = new RadioButton();
			this.radioButtonBuy = new RadioButton();
			this.buttonCancel = new Button();
			this.checkBoxConfirm = new CheckBox();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabelInfo = new ToolStripStatusLabel();
			this.buttonOrder = new Button();
			this.labelQtyScope = new Label();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.numericUpDownQty).BeginInit();
			this.groupBoxXianJia.SuspendLayout();
			((ISupportInitialize)this.numericUpDownXPrice).BeginInit();
			((ISupportInitialize)this.numericUpDownZY).BeginInit();
			((ISupportInitialize)this.numericUpDownZS).BeginInit();
			this.groupBoxSubmitMask.SuspendLayout();
			this.groupBoxShiJia.SuspendLayout();
			((ISupportInitialize)this.numericUpDownDianCha).BeginInit();
			this.statusStrip1.SuspendLayout();
			base.SuspendLayout();
			this.labelTradeType.AutoSize = true;
			this.labelTradeType.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelTradeType.Location = new Point(27, 22);
			this.labelTradeType.Name = "labelTradeType";
			this.labelTradeType.Size = new Size(67, 14);
			this.labelTradeType.TabIndex = 0;
			this.labelTradeType.Text = "交易类型";
			this.comboBoxTradeType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTradeType.FormattingEnabled = true;
			this.comboBoxTradeType.Location = new Point(115, 19);
			this.comboBoxTradeType.Name = "comboBoxTradeType";
			this.comboBoxTradeType.Size = new Size(223, 20);
			this.comboBoxTradeType.TabIndex = 0;
			this.comboBoxTradeType.SelectedIndexChanged += new EventHandler(this.comboBoxTradeType_SelectedIndexChanged);
			this.comboBoxCommodity.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCommodity.FormattingEnabled = true;
			this.comboBoxCommodity.Location = new Point(115, 51);
			this.comboBoxCommodity.Name = "comboBoxCommodity";
			this.comboBoxCommodity.Size = new Size(223, 20);
			this.comboBoxCommodity.TabIndex = 1;
			this.comboBoxCommodity.SelectedIndexChanged += new EventHandler(this.comboBoxCommodity_SelectedIndexChanged);
			this.labelCommodity.AutoEllipsis = true;
			this.labelCommodity.AutoSize = true;
			this.labelCommodity.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelCommodity.Location = new Point(27, 55);
			this.labelCommodity.Name = "labelCommodity";
			this.labelCommodity.Size = new Size(67, 14);
			this.labelCommodity.TabIndex = 0;
			this.labelCommodity.Text = "商\u3000\u3000品";
			this.labelCommodity.TextAlign = ContentAlignment.MiddleCenter;
			this.labelQty.AutoSize = true;
			this.labelQty.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelQty.Location = new Point(27, 88);
			this.labelQty.Name = "labelQty";
			this.labelQty.Size = new Size(67, 14);
			this.labelQty.TabIndex = 0;
			this.labelQty.Text = "手\u3000\u3000数";
			this.labelTrader.AutoSize = true;
			this.labelTrader.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelTrader.Location = new Point(27, 119);
			this.labelTrader.Name = "labelTrader";
			this.labelTrader.Size = new Size(67, 14);
			this.labelTrader.TabIndex = 0;
			this.labelTrader.Text = "交 易 商";
			this.comboBoxTrader.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTrader.FormattingEnabled = true;
			this.comboBoxTrader.Location = new Point(115, 115);
			this.comboBoxTrader.Name = "comboBoxTrader";
			this.comboBoxTrader.Size = new Size(223, 20);
			this.comboBoxTrader.TabIndex = 3;
			this.groupBox1.BackColor = SystemColors.Control;
			this.groupBox1.Controls.Add(this.labelQtyScope);
			this.groupBox1.Controls.Add(this.buttonmax);
			this.groupBox1.Controls.Add(this.numericUpDownQty);
			this.groupBox1.Controls.Add(this.comboBoxTrader);
			this.groupBox1.Controls.Add(this.labelTrader);
			this.groupBox1.Controls.Add(this.labelQty);
			this.groupBox1.Controls.Add(this.labelCommodity);
			this.groupBox1.Controls.Add(this.comboBoxCommodity);
			this.groupBox1.Controls.Add(this.comboBoxTradeType);
			this.groupBox1.Controls.Add(this.labelTradeType);
			this.groupBox1.ForeColor = SystemColors.ControlText;
			this.groupBox1.Location = new Point(12, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(373, 151);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.buttonmax.Location = new Point(183, 83);
			this.buttonmax.Name = "buttonmax";
			this.buttonmax.Size = new Size(45, 21);
			this.buttonmax.TabIndex = 4;
			this.buttonmax.Text = "最大";
			this.buttonmax.UseVisualStyleBackColor = true;
			this.buttonmax.Click += new EventHandler(this.buttonmax_Click);
			this.numericUpDownQty.Location = new Point(115, 83);
			NumericUpDown arg_7A5_0 = this.numericUpDownQty;
			int[] array = new int[4];
			array[0] = 1000000000;
			arg_7A5_0.Maximum = new decimal(array);
			this.numericUpDownQty.Name = "numericUpDownQty";
			this.numericUpDownQty.Size = new Size(62, 21);
			this.numericUpDownQty.TabIndex = 2;
			this.numericUpDownQty.TextChanged += new EventHandler(this.numericUpDownQty_TextChanged);
			this.numericUpDownQty.Leave += new EventHandler(this.numericUpDownQty_Leave);
			this.numericUpDownQty.Enter += new EventHandler(this.numericUpDownQty_Enter);
			this.groupBoxXianJia.Controls.Add(this.checkBoxZY);
			this.groupBoxXianJia.Controls.Add(this.textBoxXBuyHQ);
			this.groupBoxXianJia.Controls.Add(this.checkBoxZS);
			this.groupBoxXianJia.Controls.Add(this.textBoxZY);
			this.groupBoxXianJia.Controls.Add(this.textBoxXSellHQ);
			this.groupBoxXianJia.Controls.Add(this.textBoxZS);
			this.groupBoxXianJia.Controls.Add(this.numericUpDownXPrice);
			this.groupBoxXianJia.Controls.Add(this.labelZY);
			this.groupBoxXianJia.Controls.Add(this.labelXPrice);
			this.groupBoxXianJia.Controls.Add(this.labelZS);
			this.groupBoxXianJia.Controls.Add(this.comboBoxValidTime);
			this.groupBoxXianJia.Controls.Add(this.numericUpDownZY);
			this.groupBoxXianJia.Controls.Add(this.labelValidTime);
			this.groupBoxXianJia.Controls.Add(this.numericUpDownZS);
			this.groupBoxXianJia.Controls.Add(this.radioButtonSellX);
			this.groupBoxXianJia.Controls.Add(this.radioButtonBuyX);
			this.groupBoxXianJia.Location = new Point(12, 167);
			this.groupBoxXianJia.Name = "groupBoxXianJia";
			this.groupBoxXianJia.Size = new Size(373, 241);
			this.groupBoxXianJia.TabIndex = 1;
			this.groupBoxXianJia.TabStop = false;
			this.groupBoxXianJia.Visible = false;
			this.checkBoxZY.AutoSize = true;
			this.checkBoxZY.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxZY.Location = new Point(24, 201);
			this.checkBoxZY.Name = "checkBoxZY";
			this.checkBoxZY.Size = new Size(56, 18);
			this.checkBoxZY.TabIndex = 7;
			this.checkBoxZY.Text = "止盈";
			this.checkBoxZY.UseVisualStyleBackColor = true;
			this.checkBoxZY.CheckedChanged += new EventHandler(this.checkBoxZY_CheckedChanged);
			this.textBoxXBuyHQ.BackColor = Color.FromArgb(243, 200, 199);
			this.textBoxXBuyHQ.Font = new Font("宋体", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBoxXBuyHQ.ForeColor = Color.Red;
			this.textBoxXBuyHQ.Location = new Point(185, 57);
			this.textBoxXBuyHQ.Name = "textBoxXBuyHQ";
			this.textBoxXBuyHQ.ReadOnly = true;
			this.textBoxXBuyHQ.Size = new Size(170, 41);
			this.textBoxXBuyHQ.TabIndex = 11;
			this.textBoxXBuyHQ.TabStop = false;
			this.textBoxXBuyHQ.TextAlign = HorizontalAlignment.Center;
			this.checkBoxZS.AutoSize = true;
			this.checkBoxZS.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxZS.Location = new Point(24, 168);
			this.checkBoxZS.Name = "checkBoxZS";
			this.checkBoxZS.Size = new Size(56, 18);
			this.checkBoxZS.TabIndex = 5;
			this.checkBoxZS.Text = "止损";
			this.checkBoxZS.UseVisualStyleBackColor = true;
			this.checkBoxZS.CheckedChanged += new EventHandler(this.checkBoxZS_CheckedChanged);
			this.textBoxZY.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxZY.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxZY.ForeColor = Color.Blue;
			this.textBoxZY.Location = new Point(243, 198);
			this.textBoxZY.Name = "textBoxZY";
			this.textBoxZY.ReadOnly = true;
			this.textBoxZY.Size = new Size(112, 26);
			this.textBoxZY.TabIndex = 10;
			this.textBoxZY.TabStop = false;
			this.textBoxZY.TextAlign = HorizontalAlignment.Center;
			this.textBoxXSellHQ.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxXSellHQ.Font = new Font("宋体", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBoxXSellHQ.ForeColor = Color.Blue;
			this.textBoxXSellHQ.Location = new Point(16, 57);
			this.textBoxXSellHQ.Name = "textBoxXSellHQ";
			this.textBoxXSellHQ.ReadOnly = true;
			this.textBoxXSellHQ.Size = new Size(170, 41);
			this.textBoxXSellHQ.TabIndex = 10;
			this.textBoxXSellHQ.TabStop = false;
			this.textBoxXSellHQ.TextAlign = HorizontalAlignment.Center;
			this.textBoxZS.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxZS.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxZS.ForeColor = Color.Blue;
			this.textBoxZS.Location = new Point(243, 163);
			this.textBoxZS.Name = "textBoxZS";
			this.textBoxZS.ReadOnly = true;
			this.textBoxZS.Size = new Size(112, 26);
			this.textBoxZS.TabIndex = 10;
			this.textBoxZS.TabStop = false;
			this.textBoxZS.TextAlign = HorizontalAlignment.Center;
			this.numericUpDownXPrice.DecimalPlaces = 2;
			this.numericUpDownXPrice.Font = new Font("宋体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDownXPrice.Increment = new decimal(new int[]
			{
				1,
				0,
				0,
				131072
			});
			this.numericUpDownXPrice.Location = new Point(73, 109);
			NumericUpDown arg_EAB_0 = this.numericUpDownXPrice;
			int[] array2 = new int[4];
			array2[0] = 100000000;
			arg_EAB_0.Maximum = new decimal(array2);
			this.numericUpDownXPrice.Name = "numericUpDownXPrice";
			this.numericUpDownXPrice.Size = new Size(282, 44);
			this.numericUpDownXPrice.TabIndex = 4;
			this.numericUpDownXPrice.TextAlign = HorizontalAlignment.Right;
			this.numericUpDownXPrice.TextChanged += new EventHandler(this.numericUpDownXPrice_TextChanged);
			this.numericUpDownXPrice.ValueChanged += new EventHandler(this.numericUpDownXPrice_ValueChanged);
			this.numericUpDownXPrice.Enter += new EventHandler(this.numericUpDownXPrice_Enter);
			this.labelZY.AutoSize = true;
			this.labelZY.Font = new Font("宋体", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelZY.Location = new Point(210, 201);
			this.labelZY.Name = "labelZY";
			this.labelZY.Size = new Size(22, 21);
			this.labelZY.TabIndex = 1;
			this.labelZY.Text = ">";
			this.labelXPrice.AutoSize = true;
			this.labelXPrice.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelXPrice.Location = new Point(30, 125);
			this.labelXPrice.Name = "labelXPrice";
			this.labelXPrice.Size = new Size(37, 14);
			this.labelXPrice.TabIndex = 7;
			this.labelXPrice.Text = "价格";
			this.labelZS.AutoSize = true;
			this.labelZS.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelZS.Location = new Point(210, 166);
			this.labelZS.Name = "labelZS";
			this.labelZS.Size = new Size(20, 20);
			this.labelZS.TabIndex = 1;
			this.labelZS.Text = "<";
			this.comboBoxValidTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxValidTime.FormattingEnabled = true;
			this.comboBoxValidTime.Location = new Point(237, 25);
			this.comboBoxValidTime.Name = "comboBoxValidTime";
			this.comboBoxValidTime.Size = new Size(99, 20);
			this.comboBoxValidTime.TabIndex = 3;
			this.numericUpDownZY.DecimalPlaces = 2;
			this.numericUpDownZY.Enabled = false;
			this.numericUpDownZY.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.numericUpDownZY.Increment = new decimal(new int[]
			{
				1,
				0,
				0,
				131072
			});
			this.numericUpDownZY.Location = new Point(86, 198);
			NumericUpDown arg_11B4_0 = this.numericUpDownZY;
			int[] array3 = new int[4];
			array3[0] = 100000000;
			arg_11B4_0.Maximum = new decimal(array3);
			this.numericUpDownZY.Name = "numericUpDownZY";
			this.numericUpDownZY.Size = new Size(111, 26);
			this.numericUpDownZY.TabIndex = 8;
			this.numericUpDownZY.TextAlign = HorizontalAlignment.Right;
			this.numericUpDownZY.TextChanged += new EventHandler(this.numericUpDownZY_TextChanged);
			this.numericUpDownZY.ValueChanged += new EventHandler(this.numericUpDownZY_ValueChanged);
			this.numericUpDownZY.Enter += new EventHandler(this.numericUpDownZY_Enter);
			this.labelValidTime.AutoSize = true;
			this.labelValidTime.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelValidTime.Location = new Point(176, 29);
			this.labelValidTime.Name = "labelValidTime";
			this.labelValidTime.Size = new Size(52, 14);
			this.labelValidTime.TabIndex = 4;
			this.labelValidTime.Text = "有效期";
			this.numericUpDownZS.DecimalPlaces = 2;
			this.numericUpDownZS.Enabled = false;
			this.numericUpDownZS.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.numericUpDownZS.Increment = new decimal(new int[]
			{
				1,
				0,
				0,
				131072
			});
			this.numericUpDownZS.Location = new Point(86, 163);
			NumericUpDown arg_1354_0 = this.numericUpDownZS;
			int[] array4 = new int[4];
			array4[0] = 10000000;
			arg_1354_0.Maximum = new decimal(array4);
			this.numericUpDownZS.Name = "numericUpDownZS";
			this.numericUpDownZS.Size = new Size(111, 26);
			this.numericUpDownZS.TabIndex = 6;
			this.numericUpDownZS.TextAlign = HorizontalAlignment.Right;
			this.numericUpDownZS.TextChanged += new EventHandler(this.numericUpDownZS_TextChanged);
			this.numericUpDownZS.ValueChanged += new EventHandler(this.numericUpDownZS_ValueChanged);
			this.numericUpDownZS.Enter += new EventHandler(this.numericUpDownZS_Enter);
			this.radioButtonSellX.AutoSize = true;
			this.radioButtonSellX.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonSellX.Location = new Point(102, 27);
			this.radioButtonSellX.Name = "radioButtonSellX";
			this.radioButtonSellX.Size = new Size(40, 18);
			this.radioButtonSellX.TabIndex = 2;
			this.radioButtonSellX.TabStop = true;
			this.radioButtonSellX.Text = "卖";
			this.radioButtonSellX.UseVisualStyleBackColor = true;
			this.radioButtonSellX.CheckedChanged += new EventHandler(this.radioButtonSellX_CheckedChanged);
			this.radioButtonBuyX.AutoSize = true;
			this.radioButtonBuyX.Checked = true;
			this.radioButtonBuyX.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonBuyX.Location = new Point(40, 27);
			this.radioButtonBuyX.Name = "radioButtonBuyX";
			this.radioButtonBuyX.Size = new Size(40, 18);
			this.radioButtonBuyX.TabIndex = 1;
			this.radioButtonBuyX.TabStop = true;
			this.radioButtonBuyX.Text = "买";
			this.radioButtonBuyX.UseVisualStyleBackColor = true;
			this.radioButtonBuyX.CheckedChanged += new EventHandler(this.radioButtonBuyX_CheckedChanged);
			this.groupBoxSubmitMask.Controls.Add(this.label1);
			this.groupBoxSubmitMask.Controls.Add(this.loadingCircle1);
			this.groupBoxSubmitMask.Location = new Point(12, 167);
			this.groupBoxSubmitMask.Name = "groupBoxSubmitMask";
			this.groupBoxSubmitMask.Size = new Size(373, 241);
			this.groupBoxSubmitMask.TabIndex = 90;
			this.groupBoxSubmitMask.TabStop = false;
			this.groupBoxSubmitMask.Visible = false;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(71, 107);
			this.label1.Name = "label1";
			this.label1.Size = new Size(137, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "正在处理，请稍候......";
			this.loadingCircle1.set_Active(false);
			this.loadingCircle1.set_Color(Color.DarkGray);
			this.loadingCircle1.set_InnerCircleRadius(5);
			this.loadingCircle1.Location = new Point(24, 89);
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.set_NumberSpoke(12);
			this.loadingCircle1.set_OuterCircleRadius(11);
			this.loadingCircle1.set_RotationSpeed(100);
			this.loadingCircle1.Size = new Size(51, 49);
			this.loadingCircle1.set_SpokeThickness(2);
			this.loadingCircle1.set_StylePreset(0);
			this.loadingCircle1.TabIndex = 0;
			this.groupBoxShiJia.BackColor = SystemColors.Control;
			this.groupBoxShiJia.Controls.Add(this.numericUpDownDianCha);
			this.groupBoxShiJia.Controls.Add(this.checkBoxDianCha);
			this.groupBoxShiJia.Controls.Add(this.textBoxSHQ);
			this.groupBoxShiJia.Controls.Add(this.radioButtonSell);
			this.groupBoxShiJia.Controls.Add(this.radioButtonBuy);
			this.groupBoxShiJia.Location = new Point(12, 167);
			this.groupBoxShiJia.Name = "groupBoxShiJia";
			this.groupBoxShiJia.Size = new Size(373, 241);
			this.groupBoxShiJia.TabIndex = 1;
			this.groupBoxShiJia.TabStop = false;
			this.numericUpDownDianCha.Location = new Point(210, 165);
			NumericUpDown arg_17F5_0 = this.numericUpDownDianCha;
			int[] array5 = new int[4];
			array5[0] = 1000000000;
			arg_17F5_0.Maximum = new decimal(array5);
			this.numericUpDownDianCha.Name = "numericUpDownDianCha";
			this.numericUpDownDianCha.Size = new Size(83, 21);
			this.numericUpDownDianCha.TabIndex = 4;
			this.numericUpDownDianCha.TextChanged += new EventHandler(this.numericUpDownDianCha_TextChanged);
			this.numericUpDownDianCha.Enter += new EventHandler(this.numericUpDownDianCha_Enter);
			this.checkBoxDianCha.Checked = true;
			this.checkBoxDianCha.CheckState = CheckState.Checked;
			this.checkBoxDianCha.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxDianCha.Location = new Point(40, 122);
			this.checkBoxDianCha.Name = "checkBoxDianCha";
			this.checkBoxDianCha.Size = new Size(234, 23);
			this.checkBoxDianCha.TabIndex = 3;
			this.checkBoxDianCha.Text = "允许成交价和报价的最大点差";
			this.checkBoxDianCha.UseVisualStyleBackColor = true;
			this.checkBoxDianCha.Click += new EventHandler(this.checkBoxDianCha_Click);
			this.textBoxSHQ.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxSHQ.Font = new Font("宋体", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxSHQ.Location = new Point(16, 57);
			this.textBoxSHQ.Name = "textBoxSHQ";
			this.textBoxSHQ.ReadOnly = true;
			this.textBoxSHQ.Size = new Size(339, 41);
			this.textBoxSHQ.TabIndex = 2;
			this.textBoxSHQ.TabStop = false;
			this.textBoxSHQ.TextAlign = HorizontalAlignment.Center;
			this.radioButtonSell.AutoSize = true;
			this.radioButtonSell.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonSell.Location = new Point(102, 27);
			this.radioButtonSell.Name = "radioButtonSell";
			this.radioButtonSell.Size = new Size(40, 18);
			this.radioButtonSell.TabIndex = 1;
			this.radioButtonSell.TabStop = true;
			this.radioButtonSell.Text = "卖";
			this.radioButtonSell.UseVisualStyleBackColor = true;
			this.radioButtonSell.CheckedChanged += new EventHandler(this.radioButtonSell_CheckedChanged);
			this.radioButtonBuy.AutoSize = true;
			this.radioButtonBuy.Checked = true;
			this.radioButtonBuy.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonBuy.Location = new Point(40, 27);
			this.radioButtonBuy.Name = "radioButtonBuy";
			this.radioButtonBuy.Size = new Size(40, 18);
			this.radioButtonBuy.TabIndex = 0;
			this.radioButtonBuy.TabStop = true;
			this.radioButtonBuy.Text = "买";
			this.radioButtonBuy.UseVisualStyleBackColor = true;
			this.radioButtonBuy.CheckedChanged += new EventHandler(this.radioButtonBuy_CheckedChanged);
			this.buttonCancel.Location = new Point(228, 439);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 102;
			this.buttonCancel.TabStop = false;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.checkBoxConfirm.AutoSize = true;
			this.checkBoxConfirm.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxConfirm.Location = new Point(16, 415);
			this.checkBoxConfirm.Name = "checkBoxConfirm";
			this.checkBoxConfirm.Size = new Size(101, 18);
			this.checkBoxConfirm.TabIndex = 100;
			this.checkBoxConfirm.Text = "下单前确认";
			this.checkBoxConfirm.UseVisualStyleBackColor = true;
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabelInfo
			});
			this.statusStrip1.Location = new Point(0, 470);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(398, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabelInfo.ImageAlign = ContentAlignment.MiddleLeft;
			this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
			this.toolStripStatusLabelInfo.Size = new Size(56, 17);
			this.toolStripStatusLabelInfo.Text = "信息提示";
			this.toolStripStatusLabelInfo.TextAlign = ContentAlignment.MiddleLeft;
			this.buttonOrder.Location = new Point(96, 439);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(75, 23);
			this.buttonOrder.TabIndex = 101;
			this.buttonOrder.Text = "确定";
			this.buttonOrder.UseVisualStyleBackColor = true;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.labelQtyScope.AutoSize = true;
			this.labelQtyScope.Location = new Point(231, 87);
			this.labelQtyScope.Name = "labelQtyScope";
			this.labelQtyScope.Size = new Size(77, 12);
			this.labelQtyScope.TabIndex = 5;
			this.labelQtyScope.Text = "(范围：0-10)";
			this.labelQtyScope.TextAlign = ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(398, 492);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.checkBoxConfirm);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOrder);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBoxShiJia);
			base.Controls.Add(this.groupBoxXianJia);
			base.Controls.Add(this.groupBoxSubmitMask);
			this.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "NewOrdersform";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "建仓单";
			base.FormClosed += new FormClosedEventHandler(this.NewOrdersform_FormClosed);
			base.KeyDown += new KeyEventHandler(this.NewOrdersform_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.numericUpDownQty).EndInit();
			this.groupBoxXianJia.ResumeLayout(false);
			this.groupBoxXianJia.PerformLayout();
			((ISupportInitialize)this.numericUpDownXPrice).EndInit();
			((ISupportInitialize)this.numericUpDownZY).EndInit();
			((ISupportInitialize)this.numericUpDownZS).EndInit();
			this.groupBoxSubmitMask.ResumeLayout(false);
			this.groupBoxSubmitMask.PerformLayout();
			this.groupBoxShiJia.ResumeLayout(false);
			this.groupBoxShiJia.PerformLayout();
			((ISupportInitialize)this.numericUpDownDianCha).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
		protected override void WndProc(ref Message m)
		{
			int arg_0C_0 = m.Msg;
			base.WndProc(ref m);
		}
		public NewOrdersform(TMainForm parent)
		{
			this.InitializeComponent();
			this._ParentForm = parent;
			base.Icon = Global.SystamIcon;
			try
			{
				if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
				{
					List<EspecialMemberQuery> especialMemberList = this._ParentForm.dataProcess.GetEspecialMemberList();
					if (especialMemberList != null && especialMemberList.Count > 0)
					{
						Global.EspecialMemberList = especialMemberList;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.BindControlData();
			this.SetControl();
		}
		private void BindControlData()
		{
			try
			{
				this.BindTradeType();
				this.BindCommodity();
				this.BindEspecialMember();
				this.BindCustomerDianCha();
				this.BindValidTime();
				this.BindTextBoxSHQ();
				this.BindOrderComfirm();
				this._ParentForm.HQRefreashed += new TMainForm.RefreshHQHanlder(this.BindTextBoxSHQ);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetControl()
		{
			try
			{
				this.SetComboBoxTradeType();
				this.SetBuySell();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetTextSHQ(string text)
		{
			try
			{
				if (this.textBoxSHQ.InvokeRequired)
				{
					NewOrdersform.SetTextCallback method = new NewOrdersform.SetTextCallback(this.SetTextSHQ);
					if (this != null)
					{
						base.BeginInvoke(method, new object[]
						{
							text
						});
					}
				}
				else
				{
					this.textBoxSHQ.Text = text;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetTextXBHQ(string text)
		{
			try
			{
				if (this.textBoxXBuyHQ.InvokeRequired)
				{
					NewOrdersform.SetTextCallback method = new NewOrdersform.SetTextCallback(this.SetTextXBHQ);
					if (this != null)
					{
						base.BeginInvoke(method, new object[]
						{
							text
						});
					}
				}
				else
				{
					this.textBoxXBuyHQ.Text = text;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetTextXSHQ(string text)
		{
			try
			{
				if (this.textBoxXSellHQ.InvokeRequired)
				{
					NewOrdersform.SetTextCallback method = new NewOrdersform.SetTextCallback(this.SetTextXSHQ);
					if (this != null)
					{
						base.BeginInvoke(method, new object[]
						{
							text
						});
					}
				}
				else
				{
					this.textBoxXSellHQ.Text = text;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindTextBoxSHQ()
		{
			try
			{
				if (this.RefreshGNFlag)
				{
					this.RefreshGNFlag = false;
					string shiJiaBuySell = this.GetShiJiaBuySell();
					string currentCommodityId = this._CurrentCommodityId;
					Dictionary<string, CommData> dictionary = null;
					if (this._ParentForm.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData == null)
							{
								this.RefreshGNFlag = true;
								return;
							}
							dictionary = Global.gAgencyHQCommData;
							goto IL_96;
						}
					}
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData == null)
						{
							this.RefreshGNFlag = true;
							return;
						}
						dictionary = Global.gHQCommData;
					}
					IL_96:
					if ((shiJiaBuySell == BuySell.Buy.ToString("d") || shiJiaBuySell == BuySell.Sell.ToString("d")) && dictionary != null && dictionary.ContainsKey(currentCommodityId))
					{
						if (this._ParentForm.dataProcess.IsAgency)
						{
							if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(currentCommodityId))
							{
								CommodityInfo commodityInfo = Global.AgencyCommodityData[currentCommodityId];
								if (commodityInfo != null)
								{
									int minSpreadPriceCount = BizController.GetMinSpreadPriceCount(commodityInfo);
									CommData commData = dictionary[currentCommodityId];
									if (shiJiaBuySell == BuySell.Buy.ToString("d"))
									{
										this.SetTextSHQ(commData.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
									}
									else if (shiJiaBuySell == BuySell.Sell.ToString("d"))
									{
										this.SetTextSHQ(commData.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
									}
									double xBPrice = this.GetXBPrice(commodityInfo, commData.BuyPrice);
									double xSPrice = this.GetXSPrice(commodityInfo, commData.SellPrice);
									this.SetTextXBHQ(xBPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
									this.SetTextXSHQ(xSPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
								}
							}
						}
						else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(currentCommodityId))
						{
							CommodityInfo commodityInfo2 = Global.CommodityData[currentCommodityId];
							if (commodityInfo2 != null)
							{
								int minSpreadPriceCount2 = BizController.GetMinSpreadPriceCount(commodityInfo2);
								CommData commData2 = dictionary[currentCommodityId];
								if (shiJiaBuySell == BuySell.Buy.ToString("d"))
								{
									this.SetTextSHQ(commData2.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
								}
								else if (shiJiaBuySell == BuySell.Sell.ToString("d"))
								{
									this.SetTextSHQ(commData2.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
								}
								double xBPrice2 = this.GetXBPrice(commodityInfo2, commData2.BuyPrice);
								double xSPrice2 = this.GetXSPrice(commodityInfo2, commData2.SellPrice);
								this.SetTextXBHQ(xBPrice2.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
								this.SetTextXSHQ(xSPrice2.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
							}
						}
					}
					this.RefreshGNFlag = true;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindTradeType()
		{
			try
			{
				this.comboBoxTradeType.Items.Clear();
				foreach (int num in Enum.GetValues(typeof(TradeType)))
				{
					string pValue = NewOrdersform.TradeTypeStrArr[num];
					string pKey = num.ToString();
					if (num != 2 || this._ParentForm.dataProcess.sIdentity != Identity.Member)
					{
						CBListItem item = new CBListItem(pKey, pValue);
						this.comboBoxTradeType.Items.Add(item);
					}
				}
				this.comboBoxTradeType.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindCommodity()
		{
			try
			{
				this.comboBoxCommodity.Items.Clear();
				int selectedIndex = 0;
				int num = 0;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							string commodityName = current.Value.CommodityName;
							string commodityID = current.Value.CommodityID;
							CBListItem item = new CBListItem(commodityID, commodityName);
							this.comboBoxCommodity.Items.Add(item);
							if (commodityID == this._CurrentCommodityId)
							{
								selectedIndex = num;
							}
							num++;
						}
						goto IL_124;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					string commodityName2 = current2.Value.CommodityName;
					string commodityID2 = current2.Value.CommodityID;
					CBListItem item2 = new CBListItem(commodityID2, commodityName2);
					this.comboBoxCommodity.Items.Add(item2);
					if (commodityID2 == this._CurrentCommodityId)
					{
						selectedIndex = num;
					}
					num++;
				}
				IL_124:
				this.comboBoxCommodity.SelectedIndex = selectedIndex;
				this._CurrentCommodityId = ((CBListItem)this.comboBoxCommodity.SelectedItem).Key;
				this.BindQty(this._CurrentCommodityId);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindQty(string commodityId)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					this.QtyMaxValue = Convert.ToDecimal(Global.AgencyCommodityData[commodityId].P_MAX_H);
					this.QtyMinValue = Convert.ToDecimal(Global.AgencyCommodityData[commodityId].P_MIN_H);
					this.numericUpDownQty.Value = this.QtyMinValue;
				}
				else
				{
					this.QtyMaxValue = Convert.ToDecimal(Global.CommodityData[commodityId].P_MAX_H);
					this.QtyMinValue = Convert.ToDecimal(Global.CommodityData[commodityId].P_MIN_H);
					this.numericUpDownQty.Value = this.QtyMinValue;
				}
				this.labelQtyScope.Text = string.Format("(可填范围：{0}-{1})", this.QtyMinValue, this.QtyMaxValue);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindEspecialMember()
		{
			try
			{
				this.comboBoxTrader.Items.Clear();
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyEspecialMemberList == null)
					{
						return;
					}
					using (List<EspecialMemberQuery>.Enumerator enumerator = Global.AgencyEspecialMemberList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							EspecialMemberQuery current = enumerator.Current;
							string especialMemberName = current.EspecialMemberName;
							string especialMemberID = current.EspecialMemberID;
							CBListItem item = new CBListItem(especialMemberID, especialMemberName + "(" + especialMemberID + ")");
							this.comboBoxTrader.Items.Add(item);
						}
						goto IL_115;
					}
				}
				if (Global.EspecialMemberList == null)
				{
					return;
				}
				foreach (EspecialMemberQuery current2 in Global.EspecialMemberList)
				{
					string especialMemberName2 = current2.EspecialMemberName;
					string especialMemberID2 = current2.EspecialMemberID;
					CBListItem item2 = new CBListItem(especialMemberID2, especialMemberName2 + "(" + especialMemberID2 + ")");
					this.comboBoxTrader.Items.Add(item2);
				}
				IL_115:
				this.comboBoxTrader.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindCustomerDianCha()
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					this.DianChaMaxValue = Convert.ToDecimal(Global.AgencyCommodityData[this._CurrentCommodityId].U_O_D_D_MAX);
					this.DianChaMinValue = Convert.ToDecimal(Global.AgencyCommodityData[this._CurrentCommodityId].U_O_D_D_MIN);
					this.numericUpDownDianCha.Value = Convert.ToDecimal(Global.AgencyCommodityData[this._CurrentCommodityId].U_O_D_D_DF);
				}
				else
				{
					this.DianChaMaxValue = Convert.ToDecimal(Global.CommodityData[this._CurrentCommodityId].U_O_D_D_MAX);
					this.DianChaMinValue = Convert.ToDecimal(Global.CommodityData[this._CurrentCommodityId].U_O_D_D_MIN);
					this.numericUpDownDianCha.Value = Convert.ToDecimal(Global.CommodityData[this._CurrentCommodityId].U_O_D_D_DF);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindValidTime()
		{
			try
			{
				this.comboBoxValidTime.Items.Clear();
				CBListItem item = new CBListItem("0", "当日有效");
				this.comboBoxValidTime.Items.Add(item);
				this.comboBoxValidTime.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindZSZY()
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						int minSpreadPriceCount = BizController.GetMinSpreadPriceCount(commodityInfo);
						string text = this.GetXianJiaBuySell().Trim();
						if (commodityInfo != null)
						{
							if (this.checkBoxZS.Checked && this.numericUpDownXPrice.Value > 0m && text.Length > 0)
							{
								double num;
								if (text == BuySell.Buy.ToString("d"))
								{
									num = Convert.ToDouble(this.numericUpDownXPrice.Value) - commodityInfo.STOP_L_P * commodityInfo.Spread - commodityInfo.B_P_D_D * commodityInfo.Spread;
								}
								else
								{
									num = Convert.ToDouble(this.numericUpDownXPrice.Value) + commodityInfo.STOP_L_P * commodityInfo.Spread + commodityInfo.B_P_D_D * commodityInfo.Spread;
								}
								if (num >= 0.0)
								{
									this.textBoxZS.Text = num.ToString(string.Format("f{0}", minSpreadPriceCount.ToString()));
								}
								else if (minSpreadPriceCount > 0)
								{
									this.textBoxZS.Text = "0.".PadRight(minSpreadPriceCount + 2, '0');
								}
								else
								{
									this.textBoxZS.Text = "0";
								}
							}
							else
							{
								this.textBoxZS.Text = string.Empty;
							}
							if (this.checkBoxZY.Checked && this.numericUpDownXPrice.Value > 0m && text.Length > 0)
							{
								double num2;
								if (text == BuySell.Buy.ToString("d"))
								{
									num2 = Convert.ToDouble(this.numericUpDownXPrice.Value) + commodityInfo.STOP_P_P * commodityInfo.Spread - commodityInfo.B_P_D_D * commodityInfo.Spread;
								}
								else
								{
									num2 = Convert.ToDouble(this.numericUpDownXPrice.Value) - commodityInfo.STOP_P_P * commodityInfo.Spread + commodityInfo.B_P_D_D * commodityInfo.Spread;
								}
								if (num2 >= 0.0)
								{
									this.textBoxZY.Text = num2.ToString(string.Format("f{0}", minSpreadPriceCount.ToString()));
								}
								else if (minSpreadPriceCount > 0)
								{
									this.textBoxZY.Text = "0.".PadRight(minSpreadPriceCount + 2, '0');
								}
								else
								{
									this.textBoxZY.Text = "0";
								}
							}
							else
							{
								this.textBoxZY.Text = string.Empty;
							}
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					int minSpreadPriceCount2 = BizController.GetMinSpreadPriceCount(commodityInfo2);
					string text2 = this.GetXianJiaBuySell().Trim();
					if (commodityInfo2 != null)
					{
						if (this.checkBoxZS.Checked && this.numericUpDownXPrice.Value > 0m && text2.Length > 0)
						{
							double num3;
							if (text2 == BuySell.Buy.ToString("d"))
							{
								num3 = Convert.ToDouble(this.numericUpDownXPrice.Value) - commodityInfo2.STOP_L_P * commodityInfo2.Spread - commodityInfo2.B_P_D_D * commodityInfo2.Spread;
							}
							else
							{
								num3 = Convert.ToDouble(this.numericUpDownXPrice.Value) + commodityInfo2.STOP_L_P * commodityInfo2.Spread + commodityInfo2.B_P_D_D * commodityInfo2.Spread;
							}
							if (num3 >= 0.0)
							{
								this.textBoxZS.Text = num3.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString()));
							}
							else if (minSpreadPriceCount2 > 0)
							{
								this.textBoxZS.Text = "0.".PadRight(minSpreadPriceCount2 + 2, '0');
							}
							else
							{
								this.textBoxZS.Text = "0";
							}
						}
						else
						{
							this.textBoxZS.Text = string.Empty;
						}
						if (this.checkBoxZY.Checked && this.numericUpDownXPrice.Value > 0m && text2.Length > 0)
						{
							double num4;
							if (text2 == BuySell.Buy.ToString("d"))
							{
								num4 = Convert.ToDouble(this.numericUpDownXPrice.Value) + commodityInfo2.STOP_P_P * commodityInfo2.Spread - commodityInfo2.B_P_D_D * commodityInfo2.Spread;
							}
							else
							{
								num4 = Convert.ToDouble(this.numericUpDownXPrice.Value) - commodityInfo2.STOP_P_P * commodityInfo2.Spread + commodityInfo2.B_P_D_D * commodityInfo2.Spread;
							}
							if (num4 >= 0.0)
							{
								this.textBoxZY.Text = num4.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString()));
							}
							else if (minSpreadPriceCount2 > 0)
							{
								this.textBoxZY.Text = "0.".PadRight(minSpreadPriceCount2 + 2, '0');
							}
							else
							{
								this.textBoxZY.Text = "0";
							}
						}
						else
						{
							this.textBoxZY.Text = string.Empty;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void BindNumericZS()
		{
			if (this.textBoxZS.Text.Trim().Length > 0)
			{
				try
				{
					decimal num = Convert.ToDecimal(this.textBoxZS.Text);
					if (this.labelZS.Text == ">")
					{
						this.numericUpDownZS.Minimum = num;
						this.numericUpDownZS.Maximum = 100000000m;
					}
					else if (this.labelZS.Text == "<")
					{
						this.numericUpDownZS.Maximum = num;
						this.numericUpDownZS.Minimum = 0m;
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
		}
		public void BindNumericZY()
		{
			if (this.textBoxZY.Text.Trim().Length > 0)
			{
				try
				{
					decimal num = Convert.ToDecimal(this.textBoxZY.Text);
					if (this.labelZY.Text == ">")
					{
						this.numericUpDownZY.Minimum = num;
						this.numericUpDownZY.Maximum = 100000000m;
					}
					else if (this.labelZY.Text == "<")
					{
						this.numericUpDownZY.Maximum = num;
						this.numericUpDownZY.Minimum = 0m;
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
		}
		public void BindOrderComfirm()
		{
			try
			{
				this.checkBoxConfirm.Checked = IniData.GetInstance().ShowDialog;
				try
				{
					IniFile iniFile = new IniFile(string.Format(Global.ConfigPath + "{0}Trade.ini", Global.UserID));
					this.checkBoxDianCha.Checked = bool.Parse(iniFile.IniReadValue("NewOrdersForm", "checkBoxDianCha"));
				}
				catch (Exception)
				{
					this.checkBoxDianCha.Checked = true;
				}
				this.numericUpDownDianCha.Enabled = this.checkBoxDianCha.Checked;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetComboBoxTradeType()
		{
			try
			{
				this.comboBoxTradeType.SelectedIndex = Convert.ToInt32(this._OpenTradeType) - 1;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetBuySell()
		{
			try
			{
				string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
				if (key == TradeType.ShiJiaDan.ToString("d"))
				{
					this.radioButtonBuy.Checked = (this._CurrentBuySell == BuySell.Buy.ToString("d"));
					this.radioButtonSell.Checked = (this._CurrentBuySell == BuySell.Sell.ToString("d"));
				}
				else if (key == TradeType.XianJiaDan.ToString("d"))
				{
					this.radioButtonBuyX.Checked = (this._CurrentBuySell == BuySell.Buy.ToString("d"));
					this.radioButtonSellX.Checked = (this._CurrentBuySell == BuySell.Sell.ToString("d"));
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillOrderRequestVOS(OrderRequestVO orderRequestVO, string commodityId, long qty, short buyOrSell, double currentPrice, short dianCha, short tradeType, string otherId)
		{
			try
			{
				orderRequestVO.UserID = Global.UserID;
				orderRequestVO.MarketID = string.Empty;
				orderRequestVO.BuySell = buyOrSell;
				orderRequestVO.CommodityID = commodityId;
				orderRequestVO.Price = currentPrice;
				orderRequestVO.Quantity = qty;
				orderRequestVO.SettleBasis = 1;
				orderRequestVO.DotDiff = dianCha;
				orderRequestVO.TradeType = tradeType;
				orderRequestVO.OtherID = otherId;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SubmitOrderS()
		{
			try
			{
				string key = ((CBListItem)this.comboBoxCommodity.SelectedItem).Key;
				short tradeType = Convert.ToInt16(((CBListItem)this.comboBoxTradeType.SelectedItem).Key);
				long num = 0L;
				long.TryParse(this.numericUpDownQty.Value.ToString(), out num);
				short buyOrSell = Convert.ToInt16(this.GetShiJiaBuySell());
				double currentPrice = 0.0;
				try
				{
					currentPrice = Convert.ToDouble(this.textBoxSHQ.Text);
				}
				catch
				{
					this.ShowError("获取行情报价错误！");
					this.SetSubmitControls(false);
					return;
				}
				short dianCha = 0;
				if (this.checkBoxDianCha.Checked)
				{
					short.TryParse(this.numericUpDownDianCha.Text.ToString(), out dianCha);
				}
				bool @checked = this.checkBoxConfirm.Checked;
				string text = string.Empty;
				if (this.comboBoxTrader.SelectedItem != null)
				{
					text = ((CBListItem)this.comboBoxTrader.SelectedItem).Key;
				}
				CommodityInfo commodityInfo;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					commodityInfo = Global.AgencyCommodityData[key];
				}
				else
				{
					commodityInfo = Global.CommodityData[key];
				}
				OrderRequestVO orderRequestVO = new OrderRequestVO();
				if (buyOrSell.ToString() == BuySell.Buy.ToString("d"))
				{
					if (!commodityInfo.B_O_P)
					{
						this.ShowError("没有买入市价建仓权限！");
						this.SetSubmitControls(false);
						return;
					}
				}
				else if (buyOrSell.ToString() == BuySell.Sell.ToString("d") && !commodityInfo.S_O_P)
				{
					this.ShowError("没有卖出市价建仓权限！");
					this.SetSubmitControls(false);
					return;
				}
				if (num <= 0L || num > commodityInfo.P_MAX_H)
				{
					this.ShowError(string.Format("交易数量必须在{0}至{1}之间！", commodityInfo.P_MIN_H.ToString(), commodityInfo.P_MAX_H.ToString()));
					this.SetSubmitControls(false);
					this.numericUpDownQty.Select(0, 100);
					this.numericUpDownQty.Focus();
				}
				else if (text.Length == 0)
				{
					if (this._ParentForm.dataProcess.sIdentity == Identity.Client)
					{
						this.ShowError("无法获取综合会员！");
					}
					else if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
					{
						this.ShowError("无可用特别会员！");
					}
					this.SetSubmitControls(false);
				}
				else
				{
					this.FillOrderRequestVOS(orderRequestVO, key, num, buyOrSell, currentPrice, dianCha, tradeType, text);
					if (@checked)
					{
						string message;
						if (this._ParentForm.dataProcess.IsAgency)
						{
							message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.AgencyCommodityData[orderRequestVO.CommodityID].CommodityName,
								orderRequestVO.CommodityID,
								orderRequestVO.Price,
								orderRequestVO.Quantity,
								Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
								Global.BuySellStrArr[(int)orderRequestVO.BuySell]
							});
						}
						else
						{
							message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.CommodityData[orderRequestVO.CommodityID].CommodityName,
								orderRequestVO.CommodityID,
								orderRequestVO.Price,
								orderRequestVO.Quantity,
								Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
								Global.BuySellStrArr[(int)orderRequestVO.BuySell]
							});
						}
						MessageForm messageForm = new MessageForm("订单信息", message, 0, StatusBarType.Message);
						messageForm.Owner = this;
						messageForm.ShowDialog();
						messageForm.Dispose();
						if (messageForm.isOK)
						{
							this.Order(orderRequestVO);
						}
						else
						{
							this.SetSubmitControls(false);
						}
					}
					else
					{
						this.Order(orderRequestVO);
					}
				}
			}
			catch (Exception ex)
			{
				string text2 = string.Format("错误：{0} 堆栈：{1}", ex.Message, ex.StackTrace);
				Logger.wirte(3, text2);
				this.ShowError(ex.Message);
			}
		}
		private void Order(OrderRequestVO orderRequestVO)
		{
			try
			{
				this.ActiveSubmitMask(true);
				Logger.wirte(1, "下单线程提交，等待程序处理");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				threadPoolParameter.obj = orderRequestVO;
				threadPoolParameter.Semaphores = (AutoResetEvent)this.waitHandles;
				WaitCallback callBack = new WaitCallback(this.Order);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void Order(object _orderRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_orderRequestVO;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				OrderRequestVO req = (OrderRequestVO)threadPoolParameter.obj;
				ResponseVO responseVO = this._ParentForm.dataProcess.Order(req);
				NewOrdersform.ResponseVOCallback method = new NewOrdersform.ResponseVOCallback(this.OrderMessage);
				this.HandleCreated();
				base.BeginInvoke(method, new object[]
				{
					responseVO
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
			}
		}
		private void OrderMessage(ResponseVO responseVO)
		{
			try
			{
				this.comboBoxTradeType.Focus();
				if (responseVO.RetCode == 0L)
				{
					MessageForm messageForm = new MessageForm("提示", "下单成功！", 1, StatusBarType.Success);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					this._IsCloseButtonOKOrCancel = true;
					base.Close();
				}
				else
				{
					SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
					sysTimeQueryRequestVO.UserID = Global.UserID;
					SysTimeQueryResponseVO sysTime = this._ParentForm.dataProcess.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
					if (sysTime.RetCode != 0L)
					{
						MessageForm messageForm2 = new MessageForm("错误", "您的网络状态异常!", 1, StatusBarType.Error);
						messageForm2.Owner = this;
						messageForm2.ShowDialog();
						messageForm2.Dispose();
						base.Close();
						return;
					}
					if (IniData.GetInstance().FailShowDialog)
					{
						MessageForm messageForm3 = new MessageForm("错误", responseVO.RetMessage, 1, StatusBarType.Error);
						messageForm3.Owner = this;
						messageForm3.ShowDialog();
						messageForm3.Dispose();
					}
					else
					{
						this.FillInfoText(responseVO.RetMessage, StatusBarType.Error, true);
					}
				}
				this.SetSubmitControls(false);
				this.ActiveSubmitMask(false);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillOrderRequestVOX(OrderRequestVO orderRequestVO, string commodityId, long qty, short buyOrSell, double currentPrice, short tradeType, double zs, double zy, string validType, string otherId)
		{
			try
			{
				orderRequestVO.UserID = Global.UserID;
				orderRequestVO.MarketID = string.Empty;
				orderRequestVO.BuySell = buyOrSell;
				orderRequestVO.CommodityID = commodityId;
				orderRequestVO.Price = currentPrice;
				orderRequestVO.Quantity = qty;
				orderRequestVO.SettleBasis = 1;
				orderRequestVO.TradeType = tradeType;
				orderRequestVO.StopLoss = zs;
				orderRequestVO.StopProfit = zy;
				orderRequestVO.ValidityType = validType;
				orderRequestVO.OtherID = otherId;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SubmitOrderX()
		{
			try
			{
				this.toolStripStatusLabelInfo.Text = string.Empty;
				string key = ((CBListItem)this.comboBoxCommodity.SelectedItem).Key;
				short tradeType = Convert.ToInt16(((CBListItem)this.comboBoxTradeType.SelectedItem).Key);
				long num = 0L;
				long.TryParse(this.numericUpDownQty.Value.ToString(), out num);
				short buyOrSell = Convert.ToInt16(this.GetXianJiaBuySell());
				double num2 = 0.0;
				double.TryParse(this.numericUpDownXPrice.Text, out num2);
				bool @checked = this.checkBoxConfirm.Checked;
				string key2 = ((CBListItem)this.comboBoxValidTime.SelectedItem).Key;
				double num3 = Convert.ToDouble(this.textBoxXBuyHQ.Text);
				double num4 = Convert.ToDouble(this.textBoxXSellHQ.Text);
				bool checked2 = this.checkBoxZS.Checked;
				bool checked3 = this.checkBoxZY.Checked;
				double num5 = 0.0;
				try
				{
					if (checked2)
					{
						num5 = Convert.ToDouble(this.numericUpDownZS.Text);
					}
				}
				catch
				{
					this.ShowError("请输入正确的止损价格！");
					this.SetSubmitControls(false);
					return;
				}
				double num6 = 0.0;
				try
				{
					if (checked3)
					{
						num6 = Convert.ToDouble(this.numericUpDownZY.Text);
					}
				}
				catch
				{
					this.ShowError("请输入正确的止盈价格！");
					this.SetSubmitControls(false);
					return;
				}
				double num7 = 0.0;
				double num8 = 0.0;
				if (checked2 && this.textBoxZS.Text.Trim().Length > 0)
				{
					num7 = Convert.ToDouble(this.textBoxZS.Text);
				}
				if (checked3 && this.textBoxZY.Text.Trim().Length > 0)
				{
					num8 = Convert.ToDouble(this.textBoxZY.Text);
				}
				string key3 = ((CBListItem)this.comboBoxTrader.SelectedItem).Key;
				CommodityInfo commodityInfo;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					commodityInfo = Global.AgencyCommodityData[key];
				}
				else
				{
					commodityInfo = Global.CommodityData[key];
				}
				OrderRequestVO orderRequestVO = new OrderRequestVO();
				if (buyOrSell.ToString() == BuySell.Buy.ToString("d"))
				{
					if (!commodityInfo.B_X_O_P)
					{
						this.ShowError(string.Format("没有买入{0}权限！", Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "NO_PW_SHOWERROR"))));
						this.SetSubmitControls(false);
						return;
					}
				}
				else if (buyOrSell.ToString() == BuySell.Sell.ToString("d") && !commodityInfo.S_X_O_P)
				{
					this.ShowError(string.Format("没有卖出{0}权限！", Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "NO_PW_SHOWERROR"))));
					this.SetSubmitControls(false);
					return;
				}
				if (num <= 0L || num > commodityInfo.P_MAX_H)
				{
					this.ShowError(string.Format("交易数量必须在{0}至{1}之间！", commodityInfo.P_MIN_H.ToString(), commodityInfo.P_MAX_H.ToString()));
					this.SetSubmitControls(false);
					this.numericUpDownQty.Select(0, 100);
					this.numericUpDownQty.Focus();
				}
				else if (num2 == 0.0 || (num2 > num4 && num2 < num3))
				{
					this.ShowError("价格不符合条件！");
					this.SetSubmitControls(false);
					this.numericUpDownXPrice.Select(0, 100);
					this.numericUpDownXPrice.Focus();
				}
				else if (checked2 && ((this.labelZS.Text == ">" && num5 <= num7) || (this.labelZS.Text == "<" && num5 >= num7)))
				{
					this.ShowError("止损价格不符合条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZS.Select(0, 100);
					this.numericUpDownZS.Focus();
				}
				else if (checked3 && ((this.labelZY.Text == ">" && num6 <= num8) || (this.labelZY.Text == "<" && num6 >= num8)))
				{
					this.ShowError("止盈价格不符合条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZY.Select(0, 100);
					this.numericUpDownZY.Focus();
				}
				else
				{
					this.FillOrderRequestVOX(orderRequestVO, key, num, buyOrSell, num2, tradeType, num5, num6, key2, key3);
					if (@checked)
					{
						string message;
						if (this._ParentForm.dataProcess.IsAgency)
						{
							message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.AgencyCommodityData[orderRequestVO.CommodityID].CommodityName,
								orderRequestVO.CommodityID,
								orderRequestVO.Price,
								orderRequestVO.Quantity,
								Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
								Global.BuySellStrArr[(int)orderRequestVO.BuySell]
							});
						}
						else
						{
							message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.CommodityData[orderRequestVO.CommodityID].CommodityName,
								orderRequestVO.CommodityID,
								orderRequestVO.Price,
								orderRequestVO.Quantity,
								Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
								Global.BuySellStrArr[(int)orderRequestVO.BuySell]
							});
						}
						MessageForm messageForm = new MessageForm("订单信息", message, 0, StatusBarType.Message);
						messageForm.Owner = this;
						messageForm.ShowDialog();
						messageForm.Dispose();
						if (messageForm.isOK)
						{
							this.Order(orderRequestVO);
						}
						else
						{
							this.SetSubmitControls(false);
						}
					}
					else
					{
						this.Order(orderRequestVO);
					}
				}
			}
			catch (Exception ex)
			{
				string text = string.Format("错误：{0} 堆栈：{1}", ex.Message, ex.StackTrace);
				Logger.wirte(3, text);
				this.ShowError(ex.Message);
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			try
			{
				this.SetSubmitControls(true);
				this.SubmitDateTime = DateTime.Now;
				string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
				if (!this.DataCheckOn(key))
				{
					this.SetSubmitControls(false);
				}
				else
				{
					this.UpdateOrderConfirmSetting();
					if (key == TradeType.ShiJiaDan.ToString("d"))
					{
						this.SubmitOrderS();
					}
					else if (key == TradeType.XianJiaDan.ToString("d"))
					{
						this.SubmitOrderX();
					}
					IniFile iniFile = new IniFile(string.Format(Global.ConfigPath + "{0}Trade.ini", Global.UserID));
					iniFile.IniWriteValue("NewOrdersForm", "checkBoxDianCha", this.checkBoxDianCha.Checked.ToString());
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void comboBoxCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				ComboBox comboBox = (ComboBox)sender;
				this._CurrentCommodityId = ((CBListItem)comboBox.SelectedItem).Key;
				this.BindQty(this._CurrentCommodityId);
				this.BindTextBoxSHQ();
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						if (commodityInfo != null)
						{
							decimal increment = Convert.ToDecimal(commodityInfo.Spread);
							int minSpreadPriceCount = BizController.GetMinSpreadPriceCount(commodityInfo);
							this.numericUpDownXPrice.Increment = increment;
							this.numericUpDownZS.Increment = increment;
							this.numericUpDownZY.Increment = increment;
							this.numericUpDownXPrice.DecimalPlaces = minSpreadPriceCount;
							this.numericUpDownZS.DecimalPlaces = minSpreadPriceCount;
							this.numericUpDownZY.DecimalPlaces = minSpreadPriceCount;
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					if (commodityInfo2 != null)
					{
						decimal increment2 = Convert.ToDecimal(commodityInfo2.Spread);
						int minSpreadPriceCount2 = BizController.GetMinSpreadPriceCount(commodityInfo2);
						this.numericUpDownXPrice.Increment = increment2;
						this.numericUpDownZS.Increment = increment2;
						this.numericUpDownZY.Increment = increment2;
						this.numericUpDownXPrice.DecimalPlaces = minSpreadPriceCount2;
						this.numericUpDownZS.DecimalPlaces = minSpreadPriceCount2;
						this.numericUpDownZY.DecimalPlaces = minSpreadPriceCount2;
					}
				}
				this.numericUpDownXPrice.Value = 0m;
				this.numericUpDownZS.Value = 0m;
				this.numericUpDownZY.Value = 0m;
				this.BindCustomerDianCha();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxDianCha_Click(object sender, EventArgs e)
		{
			this.numericUpDownDianCha.Enabled = this.checkBoxDianCha.Checked;
		}
		private void numericUpDownDianCha_Enter(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownDianCha.Select(0, 100);
				this.ActHelpMsg(string.Format("提示：可设置点差范围最小{0}、最大{1}！", this.DianChaMinValue.ToString("f0"), this.DianChaMaxValue.ToString("f0")));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void radioButtonBuy_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.BindTextBoxSHQ();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void radioButtonSell_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.BindTextBoxSHQ();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this._ParentForm.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.BindTextBoxSHQ);
				this._IsCloseButtonOKOrCancel = false;
				base.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void comboBoxTradeType_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.SetBuySell();
				ComboBox comboBox = (ComboBox)sender;
				string key = ((CBListItem)comboBox.SelectedItem).Key;
				if (key == TradeType.ShiJiaDan.ToString("d"))
				{
					this.groupBoxShiJia.Visible = true;
					this.groupBoxShiJia.Enabled = true;
					this.groupBoxXianJia.Visible = false;
					this.groupBoxXianJia.Enabled = false;
				}
				else if (key == TradeType.XianJiaDan.ToString("d"))
				{
					this.groupBoxShiJia.Visible = false;
					this.groupBoxShiJia.Enabled = false;
					this.groupBoxXianJia.Visible = true;
					this.groupBoxXianJia.Enabled = true;
				}
				this.numericUpDownXPrice.Value = 0m;
				this.numericUpDownZS.Value = 0m;
				this.numericUpDownZY.Value = 0m;
				this.ResetHelpMsg();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownQty_Enter(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownQty.Select(0, 100);
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						if (commodityInfo != null)
						{
							this.ActHelpMsg(string.Format("提示：交易手数{0}至{1}！", commodityInfo.P_MIN_H.ToString(), commodityInfo.P_MAX_H.ToString()));
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					if (commodityInfo2 != null)
					{
						this.ActHelpMsg(string.Format("提示：交易手数{0}至{1}！", commodityInfo2.P_MIN_H.ToString(), commodityInfo2.P_MAX_H.ToString()));
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxZY_CheckedChanged(object sender, EventArgs e)
		{
			this.numericUpDownZY.Enabled = this.checkBoxZY.Checked;
			this.numericUpDownZY.Focus();
			this.BindZSZY();
		}
		private void checkBoxZS_CheckedChanged(object sender, EventArgs e)
		{
			this.numericUpDownZS.Enabled = this.checkBoxZS.Checked;
			this.numericUpDownZS.Focus();
			this.BindZSZY();
		}
		private void numericUpDownXPrice_TextChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownXPrice_Enter(object sender, EventArgs e)
		{
			this.numericUpDownXPrice.Select(0, 100);
		}
		private void numericUpDownZS_TextChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownZS_Enter(object sender, EventArgs e)
		{
			this.numericUpDownZS.Select(0, 100);
		}
		private void numericUpDownZY_TextChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownZY_Enter(object sender, EventArgs e)
		{
			this.numericUpDownZY.Select(0, 100);
		}
		private void numericUpDownXPrice_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						if (commodityInfo != null)
						{
							decimal d = Convert.ToDecimal(commodityInfo.Spread);
							if (this.numericUpDownXPrice.Value == 0m || this.numericUpDownXPrice.Value == d)
							{
								string xianJiaBuySell = this.GetXianJiaBuySell();
								if (xianJiaBuySell == BuySell.Buy.ToString("d"))
								{
									this.numericUpDownXPrice.Value = Convert.ToDecimal(this.textBoxXBuyHQ.Text);
								}
								else if (xianJiaBuySell == BuySell.Sell.ToString("d"))
								{
									this.numericUpDownXPrice.Value = Convert.ToDecimal(this.textBoxXSellHQ.Text);
								}
							}
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					if (commodityInfo2 != null)
					{
						decimal d2 = Convert.ToDecimal(commodityInfo2.Spread);
						if (this.numericUpDownXPrice.Value == 0m || this.numericUpDownXPrice.Value == d2)
						{
							string xianJiaBuySell2 = this.GetXianJiaBuySell();
							if (xianJiaBuySell2 == BuySell.Buy.ToString("d"))
							{
								this.numericUpDownXPrice.Value = Convert.ToDecimal(this.textBoxXBuyHQ.Text);
							}
							else if (xianJiaBuySell2 == BuySell.Sell.ToString("d"))
							{
								this.numericUpDownXPrice.Value = Convert.ToDecimal(this.textBoxXSellHQ.Text);
							}
						}
					}
				}
				this.BindZSZY();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void NewOrdersform_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				Keys keyCode = e.KeyCode;
				if (keyCode != Keys.Return && keyCode == Keys.Escape)
				{
					this._ParentForm.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.BindTextBoxSHQ);
					this._IsCloseButtonOKOrCancel = false;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void radioButtonBuyX_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.radioButtonBuyX.Checked)
				{
					this.labelZS.Text = "<";
					this.labelZY.Text = ">";
				}
				this.BindZSZY();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void radioButtonSellX_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.radioButtonSellX.Checked)
				{
					this.labelZS.Text = ">";
					this.labelZY.Text = "<";
				}
				this.BindZSZY();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownQty_Leave(object sender, EventArgs e)
		{
			this.ResetHelpMsg();
		}
		private void numericUpDownZS_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						if (commodityInfo != null)
						{
							decimal d = Convert.ToDecimal(commodityInfo.Spread);
							if ((this.numericUpDownZS.Value == 0m || this.numericUpDownZS.Value == d) && this.textBoxZS.Text.Trim().Length > 0)
							{
								this.numericUpDownZS.Value = Convert.ToDecimal(this.textBoxZS.Text);
							}
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					if (commodityInfo2 != null)
					{
						decimal d2 = Convert.ToDecimal(commodityInfo2.Spread);
						if ((this.numericUpDownZS.Value == 0m || this.numericUpDownZS.Value == d2) && this.textBoxZS.Text.Trim().Length > 0)
						{
							this.numericUpDownZS.Value = Convert.ToDecimal(this.textBoxZS.Text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownZY_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(this._CurrentCommodityId))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[this._CurrentCommodityId];
						if (commodityInfo != null)
						{
							decimal d = Convert.ToDecimal(commodityInfo.Spread);
							if ((this.numericUpDownZY.Value == 0m || this.numericUpDownZY.Value == d) && this.textBoxZY.Text.Trim().Length > 0)
							{
								this.numericUpDownZY.Value = Convert.ToDecimal(this.textBoxZY.Text);
							}
						}
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(this._CurrentCommodityId))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[this._CurrentCommodityId];
					if (commodityInfo2 != null)
					{
						decimal d2 = Convert.ToDecimal(commodityInfo2.Spread);
						if ((this.numericUpDownZY.Value == 0m || this.numericUpDownZY.Value == d2) && this.textBoxZY.Text.Trim().Length > 0)
						{
							this.numericUpDownZY.Value = Convert.ToDecimal(this.textBoxZY.Text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private double GetXBPrice(CommodityInfo commInfo, double buyPriceHQ)
		{
			return buyPriceHQ + commInfo.X_O_B_D_D * commInfo.Spread;
		}
		private double GetXSPrice(CommodityInfo commInfo, double sellPriceHQ)
		{
			return sellPriceHQ - commInfo.X_O_S_D_D * commInfo.Spread;
		}
		private string GetShiJiaBuySell()
		{
			string result = string.Empty;
			try
			{
				if (this.radioButtonBuy.Checked)
				{
					result = BuySell.Buy.ToString("d");
				}
				else if (this.radioButtonSell.Checked)
				{
					result = BuySell.Sell.ToString("d");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return result;
		}
		private string GetXianJiaBuySell()
		{
			string result = string.Empty;
			try
			{
				if (this.radioButtonBuyX.Checked)
				{
					result = BuySell.Buy.ToString("d");
				}
				else if (this.radioButtonSellX.Checked)
				{
					result = BuySell.Sell.ToString("d");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return result;
		}
		private void FillInfoText(string msg, StatusBarType msgType, bool isDisplay)
		{
			try
			{
				if (isDisplay)
				{
					this.toolStripStatusLabelInfo.Text = msg;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void ActHelpMsg(string msg)
		{
			this.FillInfoText(msg, StatusBarType.Message, true);
		}
		private void ResetHelpMsg()
		{
			this.FillInfoText(string.Format("提示", new object[0]), StatusBarType.Message, true);
		}
		private void SetSubmitControls(bool isSubmit)
		{
			try
			{
				this.buttonOrder.Enabled = !isSubmit;
				this.buttonCancel.Enabled = !isSubmit;
				this.RefreshGNFlag = !isSubmit;
				if (this.RefreshGNFlag)
				{
					this.BindTextBoxSHQ();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void ActiveSubmitMask(bool isActive)
		{
			try
			{
				this.groupBoxSubmitMask.Visible = isActive;
				this.loadingCircle1.set_Active(isActive);
				if (isActive)
				{
					this.groupBoxShiJia.Visible = false;
					this.groupBoxXianJia.Visible = false;
				}
				else
				{
					string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
					if (key == TradeType.ShiJiaDan.ToString("d"))
					{
						this.groupBoxShiJia.Visible = true;
						this.groupBoxShiJia.Enabled = true;
						this.groupBoxXianJia.Visible = false;
						this.groupBoxXianJia.Enabled = false;
					}
					else if (key == TradeType.XianJiaDan.ToString("d"))
					{
						this.groupBoxShiJia.Visible = false;
						this.groupBoxShiJia.Enabled = false;
						this.groupBoxXianJia.Visible = true;
						this.groupBoxXianJia.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdateOrderConfirmSetting()
		{
			try
			{
				if (this.checkBoxConfirm.Checked != IniData.GetInstance().ShowDialog)
				{
					IniData.GetInstance().ShowDialog = this.checkBoxConfirm.Checked;
					IniFile iniFile = new IniFile(Global.ConfigPath + Global.UserID + "Trade.ini");
					iniFile.IniWriteValue("Set", "ShowDialog", IniData.GetInstance().ShowDialog.ToString());
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownDianCha_TextChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownQty_TextChanged(object sender, EventArgs e)
		{
		}
		private void NewOrdersform_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.waitHandles.WaitOne();
		}
		private void buttonmax_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownQty.Text = this.QtyMaxValue.ToString();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void ShowError(string msg)
		{
			if (this._FailShowDialog)
			{
				MessageForm messageForm = new MessageForm("提示", msg, 1, StatusBarType.Warning);
				messageForm.ShowDialog();
				return;
			}
			this.ActHelpMsg(msg);
		}
		private bool DataCheckOn(string tradeType)
		{
			bool flag = true;
			try
			{
				Convert.ToInt32(this.numericUpDownQty.Text);
			}
			catch (Exception)
			{
				string msg = "请填写正确交易手数";
				flag = false;
				this.numericUpDownQty.Focus();
				this.ShowError(msg);
				bool result = flag;
				return result;
			}
			if (Convert.ToInt32(this.numericUpDownQty.Text) > this.QtyMaxValue)
			{
				string msg = "请输入正确的交易手数" + string.Format("交易手数最小{0}手、最大{1}手！", this.QtyMinValue.ToString("f0"), this.QtyMaxValue.ToString("f0"));
				flag = false;
				this.numericUpDownQty.Focus();
				this.ShowError(msg);
				return flag;
			}
			if (Convert.ToInt32(this.numericUpDownQty.Text) < this.QtyMinValue)
			{
				string msg = "请输入正确的交易手数" + string.Format("交易手数最小{0}手、最大{1}手！", this.QtyMinValue.ToString("f0"), this.QtyMaxValue.ToString("f0"));
				flag = false;
				this.numericUpDownQty.Focus();
				this.ShowError(msg);
				return flag;
			}
			if (tradeType == TradeType.ShiJiaDan.ToString("d") && this.checkBoxDianCha.Checked)
			{
				try
				{
					Convert.ToInt32(this.numericUpDownDianCha.Text);
				}
				catch (Exception)
				{
					string msg = "请输入正确的'允许成交价和报价的最大点差'！";
					flag = false;
					this.numericUpDownDianCha.Focus();
					this.ShowError(msg);
					bool result = flag;
					return result;
				}
				if (Convert.ToInt32(this.numericUpDownDianCha.Text) > this.DianChaMaxValue)
				{
					string msg = "请填写正确点差。" + string.Format("可设置点差范围最小{0}、最大{1}！", this.DianChaMinValue.ToString("f0"), this.DianChaMaxValue.ToString("f0"));
					flag = false;
					this.numericUpDownDianCha.Focus();
					this.ShowError(msg);
					return flag;
				}
				if (Convert.ToInt32(this.numericUpDownDianCha.Text) < this.DianChaMinValue)
				{
					string msg = "请填写正确点差。" + string.Format("可设置点差范围最小{0}、最大{1}！", this.DianChaMinValue.ToString("f0"), this.DianChaMaxValue.ToString("f0"));
					flag = false;
					this.numericUpDownDianCha.Focus();
					this.ShowError(msg);
					return flag;
				}
			}
			return flag;
		}
	}
}
