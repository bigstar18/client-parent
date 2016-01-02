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
	public class PWarehouseForm : Form
	{
		private delegate void SetTextCallback(string text);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private delegate void ResponseVOSCallback(ResponseVO resultMessage, ResponseVO resultMessageFSJC);
		private IContainer components;
		private GroupBox gbS;
		private TextBox textBoxSHQ;
		private GroupBox gb1;
		private Label labelQty;
		private Label labelDetail;
		private ComboBox comboBoxTradeType;
		private Label labelTradeType;
		private TextBox textBoxDetails;
		private CheckBox checkBoxDianCha;
		private CheckBox checkBoxConfirm;
		private CheckBox checkBoxFSJC;
		private NumericUpDown numericUpDownDianCha;
		private Button buttonSubmit;
		private Button buttonCancel;
		private GroupBox gbX;
		private CheckBox checkBoxZY;
		private CheckBox checkBoxZS;
		private TextBox textBoxZY;
		private TextBox textBoxZS;
		private Label labelZY;
		private Label labelZS;
		private NumericUpDown numericUpDownZY;
		private NumericUpDown numericUpDownZS;
		private TextBox textBoxXHQ;
		private ComboBox comboBoxValidTime;
		private Label labelPeriod;
		private Label labelBuySell;
		private RadioButton radioButtonSell;
		private RadioButton radioButtonBuy;
		private NumericUpDown numericUpDownQty;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ComboBox comboBoxCommodity;
		private Label labelCommodity;
		private NumericUpDown numericUpDownFSJC;
		private GroupBox gbMask;
		private LoadingCircle loadingCircle1;
		private Label labelMask;
		private ComboBox comboBoxTrader;
		private Label labelTrader;
		private Button buttonmax;
		private Label labelQtyScope;
		private string _OtherID = string.Empty;
		private bool _IsCloseFromTotalHolding;
		private bool _IsCloseButtonOKOrCancel;
		private WaitHandle[] waitHandles = new WaitHandle[]
		{
			new AutoResetEvent(true),
			new AutoResetEvent(true)
		};
		private bool _IsFormFirstLoad = true;
		private bool _IsCloseSpecificOrder;
		private Dictionary<string, CloseCommodityInfo> _CloseCommodityInfoList = new Dictionary<string, CloseCommodityInfo>();
		private TradeType _CloseTradeType = TradeType.ShiJiaDan;
		private TMainForm _ParentForm;
		private bool RefreshGNFlag = true;
		private string _CurrentCommodityId = string.Empty;
		private string _CurrentBuySell = BuySell.Buy.ToString("d");
		private int _MaxCloseQty;
		public static string[] TradeTypeStrArr = new string[]
		{
			"<未选>",
			"市价平仓单",
			Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "PF_TRADETYPESTRARR"))
		};
		private bool _FailShowDialog = IniData.GetInstance().FailShowDialog;
		private decimal QtyMaxValue = -1m;
		private decimal QtyMinValue = -1m;
		private decimal DianChaMaxValue = -1m;
		private decimal DianChaMinValue = -1m;
		private decimal FSJCMaxValue = -1m;
		private decimal FSJCMinValue = -1m;
		public string OtherID
		{
			get
			{
				return this._OtherID;
			}
			set
			{
				this._OtherID = value;
			}
		}
		public bool IsCloseFromTotalHolding
		{
			get
			{
				return this._IsCloseFromTotalHolding;
			}
			set
			{
				this._IsCloseFromTotalHolding = value;
			}
		}
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
		public bool IsCloseSpecificOrder
		{
			get
			{
				return this._IsCloseSpecificOrder;
			}
			set
			{
				this._IsCloseSpecificOrder = value;
			}
		}
		public Dictionary<string, CloseCommodityInfo> CloseCommodityInfoList
		{
			get
			{
				return this._CloseCommodityInfoList;
			}
			set
			{
				this._CloseCommodityInfoList = value;
			}
		}
		public TradeType CloseTradeType
		{
			get
			{
				return this._CloseTradeType;
			}
			set
			{
				this._CloseTradeType = value;
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
		public int MaxCloseQty
		{
			get
			{
				return this._MaxCloseQty;
			}
			set
			{
				this._MaxCloseQty = value;
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
			this.gbS = new GroupBox();
			this.comboBoxTrader = new ComboBox();
			this.labelTrader = new Label();
			this.numericUpDownFSJC = new NumericUpDown();
			this.checkBoxFSJC = new CheckBox();
			this.numericUpDownDianCha = new NumericUpDown();
			this.textBoxSHQ = new TextBox();
			this.checkBoxDianCha = new CheckBox();
			this.gbX = new GroupBox();
			this.comboBoxValidTime = new ComboBox();
			this.textBoxXHQ = new TextBox();
			this.labelPeriod = new Label();
			this.checkBoxZY = new CheckBox();
			this.checkBoxZS = new CheckBox();
			this.textBoxZY = new TextBox();
			this.textBoxZS = new TextBox();
			this.labelZY = new Label();
			this.labelZS = new Label();
			this.numericUpDownZY = new NumericUpDown();
			this.numericUpDownZS = new NumericUpDown();
			this.gb1 = new GroupBox();
			this.buttonmax = new Button();
			this.comboBoxCommodity = new ComboBox();
			this.labelCommodity = new Label();
			this.numericUpDownQty = new NumericUpDown();
			this.radioButtonSell = new RadioButton();
			this.radioButtonBuy = new RadioButton();
			this.labelBuySell = new Label();
			this.textBoxDetails = new TextBox();
			this.labelQty = new Label();
			this.labelDetail = new Label();
			this.comboBoxTradeType = new ComboBox();
			this.labelTradeType = new Label();
			this.checkBoxConfirm = new CheckBox();
			this.buttonSubmit = new Button();
			this.buttonCancel = new Button();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.gbMask = new GroupBox();
			this.labelMask = new Label();
			this.loadingCircle1 = new LoadingCircle();
			this.labelQtyScope = new Label();
			this.gbS.SuspendLayout();
			((ISupportInitialize)this.numericUpDownFSJC).BeginInit();
			((ISupportInitialize)this.numericUpDownDianCha).BeginInit();
			this.gbX.SuspendLayout();
			((ISupportInitialize)this.numericUpDownZY).BeginInit();
			((ISupportInitialize)this.numericUpDownZS).BeginInit();
			this.gb1.SuspendLayout();
			((ISupportInitialize)this.numericUpDownQty).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.gbMask.SuspendLayout();
			base.SuspendLayout();
			this.gbS.Controls.Add(this.comboBoxTrader);
			this.gbS.Controls.Add(this.labelTrader);
			this.gbS.Controls.Add(this.numericUpDownFSJC);
			this.gbS.Controls.Add(this.checkBoxFSJC);
			this.gbS.Controls.Add(this.numericUpDownDianCha);
			this.gbS.Controls.Add(this.textBoxSHQ);
			this.gbS.Controls.Add(this.checkBoxDianCha);
			this.gbS.Location = new Point(21, 168);
			this.gbS.Margin = new Padding(2);
			this.gbS.Name = "gbS";
			this.gbS.Padding = new Padding(2);
			this.gbS.Size = new Size(343, 208);
			this.gbS.TabIndex = 1;
			this.gbS.TabStop = false;
			this.comboBoxTrader.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTrader.Enabled = false;
			this.comboBoxTrader.Font = new Font("宋体", 9f);
			this.comboBoxTrader.FormattingEnabled = true;
			this.comboBoxTrader.Location = new Point(97, 168);
			this.comboBoxTrader.Margin = new Padding(2);
			this.comboBoxTrader.Name = "comboBoxTrader";
			this.comboBoxTrader.Size = new Size(224, 20);
			this.comboBoxTrader.TabIndex = 8;
			this.labelTrader.AutoEllipsis = true;
			this.labelTrader.AutoSize = true;
			this.labelTrader.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelTrader.Location = new Point(26, 171);
			this.labelTrader.Margin = new Padding(2, 0, 2, 0);
			this.labelTrader.Name = "labelTrader";
			this.labelTrader.Size = new Size(67, 14);
			this.labelTrader.TabIndex = 8;
			this.labelTrader.Text = "交 易 商";
			this.labelTrader.TextAlign = ContentAlignment.MiddleCenter;
			this.numericUpDownFSJC.Enabled = false;
			this.numericUpDownFSJC.Font = new Font("宋体", 10.5f);
			this.numericUpDownFSJC.Location = new Point(240, 126);
			NumericUpDown arg_4F9_0 = this.numericUpDownFSJC;
			int[] array = new int[4];
			array[0] = 1000000000;
			arg_4F9_0.Maximum = new decimal(array);
			this.numericUpDownFSJC.Name = "numericUpDownFSJC";
			this.numericUpDownFSJC.Size = new Size(80, 23);
			this.numericUpDownFSJC.TabIndex = 101;
			this.numericUpDownFSJC.TextChanged += new EventHandler(this.numericUpDownFSJC_TextChanged);
			this.numericUpDownFSJC.Leave += new EventHandler(this.numericUpDownFSJC_Leave);
			this.numericUpDownFSJC.Enter += new EventHandler(this.numericUpDownFSJC_Enter);
			this.checkBoxFSJC.AutoSize = true;
			this.checkBoxFSJC.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxFSJC.Location = new Point(28, 129);
			this.checkBoxFSJC.Margin = new Padding(2);
			this.checkBoxFSJC.Name = "checkBoxFSJC";
			this.checkBoxFSJC.Size = new Size(116, 18);
			this.checkBoxFSJC.TabIndex = 9;
			this.checkBoxFSJC.Text = "允许反手建仓";
			this.checkBoxFSJC.UseVisualStyleBackColor = true;
			this.checkBoxFSJC.CheckedChanged += new EventHandler(this.checkBoxFSJC_CheckedChanged);
			this.numericUpDownDianCha.Enabled = false;
			this.numericUpDownDianCha.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDownDianCha.Location = new Point(240, 83);
			NumericUpDown arg_68C_0 = this.numericUpDownDianCha;
			int[] array2 = new int[4];
			array2[0] = 1000000000;
			arg_68C_0.Maximum = new decimal(array2);
			this.numericUpDownDianCha.Name = "numericUpDownDianCha";
			this.numericUpDownDianCha.Size = new Size(80, 23);
			this.numericUpDownDianCha.TabIndex = 8;
			this.numericUpDownDianCha.TextChanged += new EventHandler(this.numericUpDownDianCha_TextChanged);
			this.numericUpDownDianCha.Leave += new EventHandler(this.numericUpDownDianCha_Leave);
			this.numericUpDownDianCha.Enter += new EventHandler(this.numericUpDownDianCha_Enter);
			this.textBoxSHQ.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxSHQ.Font = new Font("宋体", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxSHQ.ForeColor = Color.Blue;
			this.textBoxSHQ.Location = new Point(8, 25);
			this.textBoxSHQ.Margin = new Padding(2);
			this.textBoxSHQ.Name = "textBoxSHQ";
			this.textBoxSHQ.ReadOnly = true;
			this.textBoxSHQ.Size = new Size(330, 41);
			this.textBoxSHQ.TabIndex = 100;
			this.textBoxSHQ.TextAlign = HorizontalAlignment.Center;
			this.checkBoxDianCha.AutoSize = true;
			this.checkBoxDianCha.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxDianCha.Location = new Point(28, 87);
			this.checkBoxDianCha.Margin = new Padding(2);
			this.checkBoxDianCha.Name = "checkBoxDianCha";
			this.checkBoxDianCha.Size = new Size(206, 18);
			this.checkBoxDianCha.TabIndex = 7;
			this.checkBoxDianCha.Text = "允许成交价和报价最大点差";
			this.checkBoxDianCha.UseVisualStyleBackColor = true;
			this.checkBoxDianCha.CheckedChanged += new EventHandler(this.checkBoxDianCha_CheckedChanged);
			this.gbX.Controls.Add(this.comboBoxValidTime);
			this.gbX.Controls.Add(this.textBoxXHQ);
			this.gbX.Controls.Add(this.labelPeriod);
			this.gbX.Controls.Add(this.checkBoxZY);
			this.gbX.Controls.Add(this.checkBoxZS);
			this.gbX.Controls.Add(this.textBoxZY);
			this.gbX.Controls.Add(this.textBoxZS);
			this.gbX.Controls.Add(this.labelZY);
			this.gbX.Controls.Add(this.labelZS);
			this.gbX.Controls.Add(this.numericUpDownZY);
			this.gbX.Controls.Add(this.numericUpDownZS);
			this.gbX.Location = new Point(21, 168);
			this.gbX.Name = "gbX";
			this.gbX.Size = new Size(343, 208);
			this.gbX.TabIndex = 1;
			this.gbX.TabStop = false;
			this.comboBoxValidTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxValidTime.Font = new Font("宋体", 9f);
			this.comboBoxValidTime.FormattingEnabled = true;
			this.comboBoxValidTime.Items.AddRange(new object[]
			{
				"hhhh",
				"hhhh"
			});
			this.comboBoxValidTime.Location = new Point(97, 22);
			this.comboBoxValidTime.Name = "comboBoxValidTime";
			this.comboBoxValidTime.Size = new Size(129, 20);
			this.comboBoxValidTime.TabIndex = 21;
			this.textBoxXHQ.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxXHQ.Font = new Font("宋体", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxXHQ.ForeColor = Color.Blue;
			this.textBoxXHQ.Location = new Point(8, 55);
			this.textBoxXHQ.Margin = new Padding(2);
			this.textBoxXHQ.Name = "textBoxXHQ";
			this.textBoxXHQ.ReadOnly = true;
			this.textBoxXHQ.Size = new Size(330, 41);
			this.textBoxXHQ.TabIndex = 100;
			this.textBoxXHQ.TextAlign = HorizontalAlignment.Center;
			this.labelPeriod.AutoSize = true;
			this.labelPeriod.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelPeriod.Location = new Point(28, 27);
			this.labelPeriod.Name = "labelPeriod";
			this.labelPeriod.Size = new Size(52, 14);
			this.labelPeriod.TabIndex = 0;
			this.labelPeriod.Text = "有效期";
			this.checkBoxZY.AutoSize = true;
			this.checkBoxZY.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxZY.Location = new Point(26, 148);
			this.checkBoxZY.Name = "checkBoxZY";
			this.checkBoxZY.Size = new Size(56, 18);
			this.checkBoxZY.TabIndex = 25;
			this.checkBoxZY.Text = "止盈";
			this.checkBoxZY.UseVisualStyleBackColor = true;
			this.checkBoxZY.CheckedChanged += new EventHandler(this.checkBoxZY_CheckedChanged);
			this.checkBoxZS.AutoSize = true;
			this.checkBoxZS.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxZS.Location = new Point(26, 114);
			this.checkBoxZS.Name = "checkBoxZS";
			this.checkBoxZS.Size = new Size(56, 18);
			this.checkBoxZS.TabIndex = 23;
			this.checkBoxZS.Text = "止损";
			this.checkBoxZS.UseVisualStyleBackColor = true;
			this.checkBoxZS.CheckedChanged += new EventHandler(this.checkBoxZS_CheckedChanged);
			this.textBoxZY.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxZY.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxZY.ForeColor = Color.Blue;
			this.textBoxZY.Location = new Point(234, 142);
			this.textBoxZY.Name = "textBoxZY";
			this.textBoxZY.ReadOnly = true;
			this.textBoxZY.Size = new Size(87, 26);
			this.textBoxZY.TabIndex = 10;
			this.textBoxZY.TextAlign = HorizontalAlignment.Center;
			this.textBoxZS.BackColor = Color.FromArgb(174, 202, 238);
			this.textBoxZS.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.textBoxZS.ForeColor = Color.Blue;
			this.textBoxZS.Location = new Point(234, 109);
			this.textBoxZS.Name = "textBoxZS";
			this.textBoxZS.ReadOnly = true;
			this.textBoxZS.Size = new Size(87, 26);
			this.textBoxZS.TabIndex = 6;
			this.textBoxZS.TextAlign = HorizontalAlignment.Center;
			this.labelZY.AutoSize = true;
			this.labelZY.Font = new Font("宋体", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelZY.Location = new Point(199, 145);
			this.labelZY.Name = "labelZY";
			this.labelZY.Size = new Size(22, 21);
			this.labelZY.TabIndex = 9;
			this.labelZY.Text = ">";
			this.labelZS.AutoSize = true;
			this.labelZS.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelZS.Location = new Point(199, 112);
			this.labelZS.Name = "labelZS";
			this.labelZS.Size = new Size(20, 20);
			this.labelZS.TabIndex = 5;
			this.labelZS.Text = "<";
			this.numericUpDownZY.Enabled = false;
			this.numericUpDownZY.Font = new Font("宋体", 12f);
			this.numericUpDownZY.Location = new Point(97, 142);
			NumericUpDown arg_FBF_0 = this.numericUpDownZY;
			int[] array3 = new int[4];
			array3[0] = 100000000;
			arg_FBF_0.Maximum = new decimal(array3);
			this.numericUpDownZY.Name = "numericUpDownZY";
			this.numericUpDownZY.Size = new Size(87, 26);
			this.numericUpDownZY.TabIndex = 26;
			this.numericUpDownZY.TextChanged += new EventHandler(this.numericUpDownZY_TextChanged);
			this.numericUpDownZY.ValueChanged += new EventHandler(this.numericUpDownZY_ValueChanged);
			this.numericUpDownZS.Enabled = false;
			this.numericUpDownZS.Font = new Font("宋体", 12f);
			this.numericUpDownZS.Location = new Point(97, 109);
			NumericUpDown arg_107B_0 = this.numericUpDownZS;
			int[] array4 = new int[4];
			array4[0] = 100000000;
			arg_107B_0.Maximum = new decimal(array4);
			this.numericUpDownZS.Name = "numericUpDownZS";
			this.numericUpDownZS.Size = new Size(87, 26);
			this.numericUpDownZS.TabIndex = 24;
			this.numericUpDownZS.TextChanged += new EventHandler(this.numericUpDownZS_TextChanged);
			this.numericUpDownZS.ValueChanged += new EventHandler(this.numericUpDownZS_ValueChanged);
			this.gb1.BackColor = SystemColors.Control;
			this.gb1.Controls.Add(this.labelQtyScope);
			this.gb1.Controls.Add(this.buttonmax);
			this.gb1.Controls.Add(this.comboBoxCommodity);
			this.gb1.Controls.Add(this.labelCommodity);
			this.gb1.Controls.Add(this.numericUpDownQty);
			this.gb1.Controls.Add(this.radioButtonSell);
			this.gb1.Controls.Add(this.radioButtonBuy);
			this.gb1.Controls.Add(this.labelBuySell);
			this.gb1.Controls.Add(this.textBoxDetails);
			this.gb1.Controls.Add(this.labelQty);
			this.gb1.Controls.Add(this.labelDetail);
			this.gb1.Controls.Add(this.comboBoxTradeType);
			this.gb1.Controls.Add(this.labelTradeType);
			this.gb1.ForeColor = SystemColors.ControlText;
			this.gb1.Location = new Point(21, 11);
			this.gb1.Margin = new Padding(2);
			this.gb1.Name = "gb1";
			this.gb1.Padding = new Padding(2);
			this.gb1.Size = new Size(342, 155);
			this.gb1.TabIndex = 0;
			this.gb1.TabStop = false;
			this.buttonmax.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.buttonmax.Location = new Point(165, 95);
			this.buttonmax.Name = "buttonmax";
			this.buttonmax.Size = new Size(45, 21);
			this.buttonmax.TabIndex = 8;
			this.buttonmax.Text = "最大";
			this.buttonmax.UseVisualStyleBackColor = true;
			this.buttonmax.Click += new EventHandler(this.buttonmax_Click);
			this.comboBoxCommodity.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCommodity.Font = new Font("宋体", 9f);
			this.comboBoxCommodity.FormattingEnabled = true;
			this.comboBoxCommodity.Location = new Point(97, 43);
			this.comboBoxCommodity.Name = "comboBoxCommodity";
			this.comboBoxCommodity.Size = new Size(224, 20);
			this.comboBoxCommodity.TabIndex = 2;
			this.comboBoxCommodity.SelectedIndexChanged += new EventHandler(this.comboBoxCommodity_SelectedIndexChanged);
			this.labelCommodity.AutoSize = true;
			this.labelCommodity.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelCommodity.Location = new Point(40, 46);
			this.labelCommodity.Margin = new Padding(2, 0, 2, 0);
			this.labelCommodity.Name = "labelCommodity";
			this.labelCommodity.Size = new Size(37, 14);
			this.labelCommodity.TabIndex = 7;
			this.labelCommodity.Text = "商品";
			this.numericUpDownQty.Font = new Font("宋体", 9f);
			this.numericUpDownQty.Location = new Point(97, 95);
			NumericUpDown arg_14A1_0 = this.numericUpDownQty;
			int[] array5 = new int[4];
			array5[0] = 1000000000;
			arg_14A1_0.Maximum = new decimal(array5);
			this.numericUpDownQty.Name = "numericUpDownQty";
			this.numericUpDownQty.Size = new Size(62, 21);
			this.numericUpDownQty.TabIndex = 5;
			this.numericUpDownQty.TextChanged += new EventHandler(this.numericUpDownQty_TextChanged);
			this.numericUpDownQty.Leave += new EventHandler(this.numericUpDownQty_Leave);
			this.numericUpDownQty.Enter += new EventHandler(this.numericUpDownQty_Enter);
			this.radioButtonSell.AutoSize = true;
			this.radioButtonSell.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonSell.Location = new Point(171, 72);
			this.radioButtonSell.Name = "radioButtonSell";
			this.radioButtonSell.Size = new Size(55, 18);
			this.radioButtonSell.TabIndex = 4;
			this.radioButtonSell.Text = "卖出";
			this.radioButtonSell.UseVisualStyleBackColor = true;
			this.radioButtonSell.CheckedChanged += new EventHandler(this.radioButtonSell_CheckedChanged);
			this.radioButtonBuy.AutoSize = true;
			this.radioButtonBuy.Checked = true;
			this.radioButtonBuy.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.radioButtonBuy.Location = new Point(97, 72);
			this.radioButtonBuy.Name = "radioButtonBuy";
			this.radioButtonBuy.Size = new Size(55, 18);
			this.radioButtonBuy.TabIndex = 3;
			this.radioButtonBuy.TabStop = true;
			this.radioButtonBuy.Text = "买入";
			this.radioButtonBuy.UseVisualStyleBackColor = true;
			this.radioButtonBuy.CheckedChanged += new EventHandler(this.radioButtonBuy_CheckedChanged);
			this.labelBuySell.AutoSize = true;
			this.labelBuySell.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelBuySell.Location = new Point(40, 73);
			this.labelBuySell.Margin = new Padding(2, 0, 2, 0);
			this.labelBuySell.Name = "labelBuySell";
			this.labelBuySell.Size = new Size(37, 14);
			this.labelBuySell.TabIndex = 3;
			this.labelBuySell.Text = "方向";
			this.textBoxDetails.Font = new Font("宋体", 9f);
			this.textBoxDetails.Location = new Point(97, 123);
			this.textBoxDetails.Margin = new Padding(2);
			this.textBoxDetails.Name = "textBoxDetails";
			this.textBoxDetails.ReadOnly = true;
			this.textBoxDetails.Size = new Size(224, 21);
			this.textBoxDetails.TabIndex = 6;
			this.labelQty.AutoSize = true;
			this.labelQty.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelQty.Location = new Point(40, 99);
			this.labelQty.Margin = new Padding(2, 0, 2, 0);
			this.labelQty.Name = "labelQty";
			this.labelQty.Size = new Size(37, 14);
			this.labelQty.TabIndex = 0;
			this.labelQty.Text = "数量";
			this.labelDetail.AutoEllipsis = true;
			this.labelDetail.AutoSize = true;
			this.labelDetail.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelDetail.Location = new Point(40, 127);
			this.labelDetail.Margin = new Padding(2, 0, 2, 0);
			this.labelDetail.Name = "labelDetail";
			this.labelDetail.Size = new Size(37, 14);
			this.labelDetail.TabIndex = 0;
			this.labelDetail.Text = "明细";
			this.labelDetail.TextAlign = ContentAlignment.MiddleCenter;
			this.comboBoxTradeType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTradeType.Font = new Font("宋体", 9f);
			this.comboBoxTradeType.FormattingEnabled = true;
			this.comboBoxTradeType.Location = new Point(97, 16);
			this.comboBoxTradeType.Margin = new Padding(2);
			this.comboBoxTradeType.Name = "comboBoxTradeType";
			this.comboBoxTradeType.Size = new Size(224, 20);
			this.comboBoxTradeType.TabIndex = 1;
			this.comboBoxTradeType.SelectedIndexChanged += new EventHandler(this.comboBoxTradeType_SelectedIndexChanged);
			this.labelTradeType.AutoSize = true;
			this.labelTradeType.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.labelTradeType.Location = new Point(40, 20);
			this.labelTradeType.Margin = new Padding(2, 0, 2, 0);
			this.labelTradeType.Name = "labelTradeType";
			this.labelTradeType.Size = new Size(37, 14);
			this.labelTradeType.TabIndex = 0;
			this.labelTradeType.Text = "类型";
			this.checkBoxConfirm.AutoSize = true;
			this.checkBoxConfirm.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.checkBoxConfirm.Location = new Point(23, 381);
			this.checkBoxConfirm.Margin = new Padding(2);
			this.checkBoxConfirm.Name = "checkBoxConfirm";
			this.checkBoxConfirm.Size = new Size(101, 18);
			this.checkBoxConfirm.TabIndex = 200;
			this.checkBoxConfirm.Text = "下单前确认";
			this.checkBoxConfirm.UseVisualStyleBackColor = true;
			this.buttonSubmit.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.buttonSubmit.Location = new Point(89, 404);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new Size(75, 23);
			this.buttonSubmit.TabIndex = 201;
			this.buttonSubmit.Text = "确认";
			this.buttonSubmit.UseVisualStyleBackColor = true;
			this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
			this.buttonCancel.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.buttonCancel.Location = new Point(224, 404);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 202;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1
			});
			this.statusStrip1.Location = new Point(0, 432);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(384, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(56, 17);
			this.toolStripStatusLabel1.Text = "信息提示";
			this.gbMask.Controls.Add(this.labelMask);
			this.gbMask.Controls.Add(this.loadingCircle1);
			this.gbMask.Location = new Point(21, 168);
			this.gbMask.Margin = new Padding(2);
			this.gbMask.Name = "gbMask";
			this.gbMask.Padding = new Padding(2);
			this.gbMask.Size = new Size(343, 208);
			this.gbMask.TabIndex = 0;
			this.gbMask.TabStop = false;
			this.gbMask.Visible = false;
			this.labelMask.AutoSize = true;
			this.labelMask.Font = new Font("宋体", 9f);
			this.labelMask.Location = new Point(75, 84);
			this.labelMask.Name = "labelMask";
			this.labelMask.Size = new Size(137, 12);
			this.labelMask.TabIndex = 1;
			this.labelMask.Text = "正在处理，请稍候......";
			this.loadingCircle1.set_Active(false);
			this.loadingCircle1.set_Color(Color.DarkGray);
			this.loadingCircle1.set_InnerCircleRadius(5);
			this.loadingCircle1.Location = new Point(31, 72);
			this.loadingCircle1.Margin = new Padding(2);
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.set_NumberSpoke(12);
			this.loadingCircle1.set_OuterCircleRadius(11);
			this.loadingCircle1.set_RotationSpeed(100);
			this.loadingCircle1.Size = new Size(45, 35);
			this.loadingCircle1.set_SpokeThickness(2);
			this.loadingCircle1.set_StylePreset(0);
			this.loadingCircle1.TabIndex = 0;
			this.labelQtyScope.AutoSize = true;
			this.labelQtyScope.Font = new Font("宋体", 9.5f);
			this.labelQtyScope.Location = new Point(213, 99);
			this.labelQtyScope.Name = "labelQtyScope";
			this.labelQtyScope.Size = new Size(88, 13);
			this.labelQtyScope.TabIndex = 9;
			this.labelQtyScope.Text = "(范围：0-10)";
			this.labelQtyScope.TextAlign = ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new SizeF(5f, 10f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(384, 454);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonSubmit);
			base.Controls.Add(this.checkBoxConfirm);
			base.Controls.Add(this.gb1);
			base.Controls.Add(this.gbS);
			base.Controls.Add(this.gbMask);
			base.Controls.Add(this.gbX);
			this.Font = new Font("宋体", 7.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.Margin = new Padding(2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PWarehouseForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "平仓单";
			base.FormClosed += new FormClosedEventHandler(this.PWarehouseForm_FormClosed);
			base.KeyDown += new KeyEventHandler(this.PWarehouseForm_KeyDown);
			this.gbS.ResumeLayout(false);
			this.gbS.PerformLayout();
			((ISupportInitialize)this.numericUpDownFSJC).EndInit();
			((ISupportInitialize)this.numericUpDownDianCha).EndInit();
			this.gbX.ResumeLayout(false);
			this.gbX.PerformLayout();
			((ISupportInitialize)this.numericUpDownZY).EndInit();
			((ISupportInitialize)this.numericUpDownZS).EndInit();
			this.gb1.ResumeLayout(false);
			this.gb1.PerformLayout();
			((ISupportInitialize)this.numericUpDownQty).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.gbMask.ResumeLayout(false);
			this.gbMask.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
		public PWarehouseForm(TMainForm parent)
		{
			try
			{
				this.InitializeComponent();
				this._ParentForm = parent;
				base.Icon = Global.SystamIcon;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			try
			{
				base.OnLoad(e);
				this.BindControlData();
				this.SetControlValue();
				this._IsFormFirstLoad = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindControlData()
		{
			try
			{
				this.BindTradeType();
				this.BindCommodity();
				this.BindCustomerDianCha();
				this.BindValidTime();
				this.BindEspecialMember();
				this.BindTradeDetail();
				this.BindFSJCQty();
				this.BindTextBoxSHQ();
				this.BindOrderComfirm();
				this._ParentForm.HQRefreashed += new TMainForm.RefreshHQHanlder(this.BindTextBoxSHQ);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetControlValue()
		{
			try
			{
				this.SetComboBoxTradeType();
				this.SetGroupBox();
				this.SetSpecificOrder();
				this.SetBuySell();
				this.SetZSZY();
				this.SetZSZYSign();
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
					PWarehouseForm.SetTextCallback method = new PWarehouseForm.SetTextCallback(this.SetTextSHQ);
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
		private void SetTextXHQ(string text)
		{
			try
			{
				if (this.textBoxXHQ.InvokeRequired)
				{
					PWarehouseForm.SetTextCallback method = new PWarehouseForm.SetTextCallback(this.SetTextXHQ);
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
					this.textBoxXHQ.Text = text;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetTextXZS(string text)
		{
			try
			{
				if (this.textBoxZS.InvokeRequired)
				{
					PWarehouseForm.SetTextCallback method = new PWarehouseForm.SetTextCallback(this.SetTextXZS);
					if (this != null)
					{
						base.BeginInvoke(method, new object[]
						{
							text
						});
					}
				}
				else if (this.textBoxZS != null)
				{
					this.textBoxZS.Text = text;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetTextXZY(string text)
		{
			try
			{
				if (this.textBoxZY.InvokeRequired)
				{
					PWarehouseForm.SetTextCallback method = new PWarehouseForm.SetTextCallback(this.SetTextXZY);
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
					this.textBoxZY.Text = text;
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
					string currentBuySell = this._CurrentBuySell;
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
					if ((currentBuySell == BuySell.Buy.ToString("d") || currentBuySell == BuySell.Sell.ToString("d")) && dictionary != null && dictionary.ContainsKey(currentCommodityId))
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
									if (currentBuySell == BuySell.Buy.ToString("d"))
									{
										this.SetTextSHQ(commData.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										this.SetTextXHQ(commData.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										double num = commData.BuyPrice + commodityInfo.STOP_L_P * commodityInfo.Spread;
										double num2 = commData.BuyPrice - commodityInfo.STOP_P_P * commodityInfo.Spread;
										this.SetTextXZS(num.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										this.SetTextXZY(num2.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
									}
									else if (currentBuySell == BuySell.Sell.ToString("d"))
									{
										this.SetTextSHQ(commData.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										this.SetTextXHQ(commData.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										double num3 = commData.SellPrice - commodityInfo.STOP_L_P * commodityInfo.Spread;
										double num4 = commData.SellPrice + commodityInfo.STOP_P_P * commodityInfo.Spread;
										this.SetTextXZS(num3.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
										this.SetTextXZY(num4.ToString(string.Format("f{0}", minSpreadPriceCount.ToString())));
									}
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
								if (currentBuySell == BuySell.Buy.ToString("d"))
								{
									this.SetTextSHQ(commData2.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									this.SetTextXHQ(commData2.BuyPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									double num5 = commData2.BuyPrice + commodityInfo2.STOP_L_P * commodityInfo2.Spread;
									double num6 = commData2.BuyPrice - commodityInfo2.STOP_P_P * commodityInfo2.Spread;
									this.SetTextXZS(num5.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									this.SetTextXZY(num6.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
								}
								else if (currentBuySell == BuySell.Sell.ToString("d"))
								{
									this.SetTextSHQ(commData2.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									this.SetTextXHQ(commData2.SellPrice.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									double num7 = commData2.SellPrice - commodityInfo2.STOP_L_P * commodityInfo2.Spread;
									double num8 = commData2.SellPrice + commodityInfo2.STOP_P_P * commodityInfo2.Spread;
									this.SetTextXZS(num7.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
									this.SetTextXZY(num8.ToString(string.Format("f{0}", minSpreadPriceCount2.ToString())));
								}
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
					string pValue = PWarehouseForm.TradeTypeStrArr[num];
					string text = num.ToString();
					if (!this._IsCloseSpecificOrder)
					{
						if (text == TradeType.ShiJiaDan.ToString("d"))
						{
							CBListItem item = new CBListItem(text, pValue);
							this.comboBoxTradeType.Items.Add(item);
						}
					}
					else if (num != 2 || this._ParentForm.dataProcess.sIdentity != Identity.Member)
					{
						CBListItem item2 = new CBListItem(text, pValue);
						this.comboBoxTradeType.Items.Add(item2);
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
				this.BindQty();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindQty()
		{
			try
			{
				string currentBuySell = this._CurrentBuySell;
				decimal qtyMinValue = 0m;
				decimal num = 0m;
				if (this._CloseCommodityInfoList != null && this._CloseCommodityInfoList.ContainsKey(this._CurrentCommodityId))
				{
					if (currentBuySell == BuySell.Buy.ToString("d"))
					{
						num = Convert.ToDecimal(this._CloseCommodityInfoList[this._CurrentCommodityId].CloseMaxBuyQty);
					}
					else if (currentBuySell == BuySell.Sell.ToString("d"))
					{
						num = Convert.ToDecimal(this._CloseCommodityInfoList[this._CurrentCommodityId].CloseMaxSellQty);
					}
					if (num > 0m)
					{
						qtyMinValue = 1m;
					}
				}
				this.QtyMaxValue = num;
				this.QtyMinValue = qtyMinValue;
				this.numericUpDownQty.Value = this.QtyMaxValue;
				this.labelQtyScope.Text = string.Format("(可填范围：{0}-{1})", this.QtyMinValue, this.QtyMaxValue);
				if (this.MaxCloseQty != 0)
				{
					this.numericUpDownQty.Value = this.MaxCloseQty;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindTradeDetail()
		{
			try
			{
				this.labelDetail.Visible = (this._IsCloseSpecificOrder || this.IsCloseFromTotalHolding);
				this.textBoxDetails.Visible = (this._IsCloseSpecificOrder || this.IsCloseFromTotalHolding);
				this.comboBoxCommodity.Enabled = (!this._IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				this.radioButtonBuy.Enabled = (!this._IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				this.radioButtonSell.Enabled = (!this._IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				if (this.CloseTradeType == TradeType.XianJiaDan)
				{
					this.numericUpDownQty.Enabled = !this._IsCloseSpecificOrder;
				}
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
		private void BindFSJCQty()
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					this.FSJCMaxValue = Convert.ToDecimal(Global.AgencyCommodityData[this._CurrentCommodityId].P_MAX_H);
					this.FSJCMinValue = Convert.ToDecimal(Global.AgencyCommodityData[this._CurrentCommodityId].P_MIN_H);
				}
				else
				{
					this.FSJCMaxValue = Convert.ToDecimal(Global.CommodityData[this._CurrentCommodityId].P_MAX_H);
					this.FSJCMinValue = Convert.ToDecimal(Global.CommodityData[this._CurrentCommodityId].P_MIN_H);
				}
				this.numericUpDownFSJC.Value = this.FSJCMinValue;
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
				if (this._OtherID.Trim().Length > 0)
				{
					string text = this._OtherID.Trim();
					string pValue = string.Empty;
					if (Global.AllEspecialMemberList != null && Global.AllEspecialMemberList.ContainsKey(text))
					{
						pValue = ((EspecialMemberQuery)Global.AllEspecialMemberList[text]).EspecialMemberName;
					}
					else
					{
						pValue = text;
					}
					CBListItem item = new CBListItem(text, pValue);
					this.comboBoxTrader.Items.Add(item);
				}
				else
				{
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
								CBListItem item2 = new CBListItem(especialMemberID, especialMemberName);
								this.comboBoxTrader.Items.Add(item2);
							}
							goto IL_169;
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
						CBListItem item3 = new CBListItem(especialMemberID2, especialMemberName2);
						this.comboBoxTrader.Items.Add(item3);
					}
				}
				IL_169:
				this.comboBoxTrader.SelectedIndex = 0;
				if (this.comboBoxTrader.Items.Count == 1)
				{
					if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
					{
						this.comboBoxTrader.Visible = true;
						this.labelTrader.Visible = true;
					}
					else
					{
						this.comboBoxTrader.Visible = false;
						this.labelTrader.Visible = false;
					}
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
		public void BindOrderComfirm()
		{
			try
			{
				this.checkBoxConfirm.Checked = IniData.GetInstance().ShowDialog;
				try
				{
					IniFile iniFile = new IniFile(string.Format(Global.ConfigPath + "{0}Trade.ini", Global.UserID));
					this.checkBoxDianCha.Checked = bool.Parse(iniFile.IniReadValue("PWarehouseForm", "checkBoxDianCha"));
				}
				catch (Exception)
				{
					this.checkBoxDianCha.Checked = false;
				}
				this.numericUpDownDianCha.Enabled = this.checkBoxDianCha.Checked;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetZSZYSign()
		{
			try
			{
				if (this.radioButtonBuy.Checked)
				{
					this.labelZS.Text = ">";
					this.labelZY.Text = "<";
				}
				else if (this.radioButtonSell.Checked)
				{
					this.labelZS.Text = "<";
					this.labelZY.Text = ">";
				}
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
				this.comboBoxTradeType.SelectedIndex = Convert.ToInt32(this._CloseTradeType) - 1;
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
				this.radioButtonBuy.Checked = (this._CurrentBuySell == BuySell.Buy.ToString("d"));
				this.radioButtonSell.Checked = (this._CurrentBuySell == BuySell.Sell.ToString("d"));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetSpecificOrder()
		{
			try
			{
				if (this.IsCloseSpecificOrder)
				{
					if (this.CloseCommodityInfoList != null && this.CloseCommodityInfoList.ContainsKey(this._CurrentCommodityId))
					{
						CloseCommodityInfo closeCommodityInfo = this.CloseCommodityInfoList[this._CurrentCommodityId];
						if (closeCommodityInfo.OriginBuySell == BuySell.Buy.ToString("d"))
						{
							this._CurrentBuySell = BuySell.Sell.ToString("d");
						}
						else if (closeCommodityInfo.OriginBuySell == BuySell.Sell.ToString("d"))
						{
							this._CurrentBuySell = BuySell.Buy.ToString("d");
						}
						string text = string.Empty;
						if (this._ParentForm.dataProcess.IsAgency)
						{
							text = string.Format("{0} {1} {2}", closeCommodityInfo.HoldingID.ToString(), Global.BuySellStrArr[Convert.ToInt32(this._CurrentBuySell)], Global.AgencyCommodityData[this._CurrentCommodityId].CommodityName);
						}
						else
						{
							text = string.Format("{0} {1} {2}", closeCommodityInfo.HoldingID.ToString(), Global.BuySellStrArr[Convert.ToInt32(this._CurrentBuySell)], Global.CommodityData[this._CurrentCommodityId].CommodityName);
						}
						this.textBoxDetails.Text = text;
					}
				}
				else if (this.IsCloseFromTotalHolding && this.CloseCommodityInfoList != null && this.CloseCommodityInfoList.ContainsKey(this._CurrentCommodityId))
				{
					CloseCommodityInfo closeCommodityInfo2 = this.CloseCommodityInfoList[this._CurrentCommodityId];
					if (closeCommodityInfo2.OriginBuySell == BuySell.Buy.ToString("d"))
					{
						this._CurrentBuySell = BuySell.Sell.ToString("d");
					}
					else if (closeCommodityInfo2.OriginBuySell == BuySell.Sell.ToString("d"))
					{
						this._CurrentBuySell = BuySell.Buy.ToString("d");
					}
					string text2 = string.Empty;
					if (this._ParentForm.dataProcess.IsAgency)
					{
						text2 = string.Format("{0} {1} ", Global.BuySellStrArr[Convert.ToInt32(this._CurrentBuySell)], Global.AgencyCommodityData[this._CurrentCommodityId].CommodityName);
					}
					else
					{
						text2 = string.Format("{0} {1} ", Global.BuySellStrArr[Convert.ToInt32(this._CurrentBuySell)], Global.CommodityData[this._CurrentCommodityId].CommodityName);
					}
					this.textBoxDetails.Text = text2;
				}
				this.radioButtonBuy.Enabled = (!this.IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				this.radioButtonSell.Enabled = (!this.IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				this.comboBoxCommodity.Enabled = (!this.IsCloseSpecificOrder && !this.IsCloseFromTotalHolding);
				this.labelDetail.Visible = (this.IsCloseSpecificOrder || this.IsCloseFromTotalHolding);
				this.textBoxDetails.Visible = (this.IsCloseSpecificOrder || this.IsCloseFromTotalHolding);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetZSZY()
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
							decimal increment = Convert.ToDecimal(commodityInfo.Spread);
							int minSpreadPriceCount = BizController.GetMinSpreadPriceCount(commodityInfo);
							this.numericUpDownZS.Increment = increment;
							this.numericUpDownZY.Increment = increment;
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
						this.numericUpDownZS.Increment = increment2;
						this.numericUpDownZY.Increment = increment2;
						this.numericUpDownZS.DecimalPlaces = minSpreadPriceCount2;
						this.numericUpDownZY.DecimalPlaces = minSpreadPriceCount2;
					}
				}
				if (this.IsCloseSpecificOrder && this.CloseCommodityInfoList != null && this.CloseCommodityInfoList.ContainsKey(this._CurrentCommodityId))
				{
					CloseCommodityInfo closeCommodityInfo = this.CloseCommodityInfoList[this._CurrentCommodityId];
					this.numericUpDownZS.Text = closeCommodityInfo.ZS.ToString();
					this.numericUpDownZY.Text = closeCommodityInfo.ZY.ToString();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetGroupBox()
		{
			try
			{
				string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
				if (key == TradeType.ShiJiaDan.ToString("d"))
				{
					this.gbS.Visible = true;
					this.gbS.Enabled = true;
					this.gbX.Visible = false;
					this.gbX.Enabled = false;
				}
				else if (key == TradeType.XianJiaDan.ToString("d"))
				{
					this.gbS.Visible = false;
					this.gbS.Enabled = false;
					this.gbX.Visible = true;
					this.gbX.Enabled = true;
				}
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
				if (!this._IsFormFirstLoad)
				{
					this.SetGroupBox();
					ComboBox comboBox = (ComboBox)sender;
					string key = ((CBListItem)comboBox.SelectedItem).Key;
					if (key == TradeType.XianJiaDan.ToString("d"))
					{
						this.SetZSZY();
					}
					this.numericUpDownZS.Value = 0m;
					this.numericUpDownZY.Value = 0m;
					this.BindQty();
					if (this.IsCloseSpecificOrder)
					{
						this.numericUpDownQty.Enabled = (key == TradeType.ShiJiaDan.ToString("d"));
					}
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
				if (!this._IsFormFirstLoad)
				{
					ComboBox comboBox = (ComboBox)sender;
					this._CurrentCommodityId = ((CBListItem)comboBox.SelectedItem).Key;
					this.BindQty();
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
								this.numericUpDownZS.Increment = increment;
								this.numericUpDownZY.Increment = increment;
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
							this.numericUpDownZS.Increment = increment2;
							this.numericUpDownZY.Increment = increment2;
							this.numericUpDownZS.DecimalPlaces = minSpreadPriceCount2;
							this.numericUpDownZY.DecimalPlaces = minSpreadPriceCount2;
						}
					}
					this.numericUpDownZS.Value = 0m;
					this.numericUpDownZY.Value = 0m;
					this.SetZSZY();
					this.BindFSJCQty();
					this.BindCustomerDianCha();
				}
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
				if (!this._IsFormFirstLoad)
				{
					this._CurrentBuySell = this.GetBuySell();
					this.BindQty();
					this.BindTextBoxSHQ();
					if (this.radioButtonBuy.Checked)
					{
						this.labelZS.Text = ">";
						this.labelZY.Text = "<";
					}
				}
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
				if (!this._IsFormFirstLoad)
				{
					this._CurrentBuySell = this.GetBuySell();
					this.BindQty();
					this.BindTextBoxSHQ();
					if (this.radioButtonSell.Checked)
					{
						this.labelZS.Text = "<";
						this.labelZY.Text = ">";
					}
				}
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
				this.ActHelpMsg(string.Format("提示：可平仓数量最小{0}手、最大{1}手！", this.QtyMinValue.ToString("f0"), this.QtyMaxValue.ToString("f0")));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownQty_Leave(object sender, EventArgs e)
		{
			try
			{
				this.ResetHelpMsg();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void ActHelpMsg(string msg)
		{
			try
			{
				this.FillInfoText(msg, StatusBarType.Message, true);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void ResetHelpMsg()
		{
			try
			{
				this.FillInfoText(string.Format("提示", new object[0]), StatusBarType.Message, true);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillInfoText(string msg, StatusBarType msgType, bool isDisplay)
		{
			try
			{
				if (isDisplay)
				{
					this.toolStripStatusLabel1.Text = msg;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxDianCha_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownDianCha.Enabled = this.checkBoxDianCha.Checked;
				this.numericUpDownDianCha.Focus();
				this.numericUpDownDianCha.Select(0, 100);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxFSJC_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownFSJC.Enabled = this.checkBoxFSJC.Checked;
				this.comboBoxTrader.Enabled = this.checkBoxFSJC.Checked;
				this.numericUpDownFSJC.Focus();
				this.numericUpDownFSJC.Select(0, 100);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
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
		private void numericUpDownDianCha_Leave(object sender, EventArgs e)
		{
			try
			{
				this.ResetHelpMsg();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownFSJC_Enter(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownFSJC.Select(0, 100);
				this.ActHelpMsg(string.Format("提示：反手建仓数量最小{0}、最大{1}！", this.FSJCMinValue.ToString("f0"), this.FSJCMaxValue.ToString("f0")));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownFSJC_TextChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownFSJC_Leave(object sender, EventArgs e)
		{
			try
			{
				this.ResetHelpMsg();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxZS_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownZS.Enabled = this.checkBoxZS.Checked;
				this.numericUpDownZS.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void checkBoxZY_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.numericUpDownZY.Enabled = this.checkBoxZY.Checked;
				this.numericUpDownZY.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void numericUpDownZS_TextChanged(object sender, EventArgs e)
		{
			double num = 0.0;
			try
			{
				if (this.numericUpDownZS.Text.Trim().Length == 0)
				{
					return;
				}
				num = Convert.ToDouble(this.numericUpDownZS.Text);
			}
			catch (Exception ex)
			{
				MessageForm messageForm = new MessageForm("提示", "请输入数值类型", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
				Logger.wirte(1, ex.Message);
				this.numericUpDownZS.Text = "";
				return;
			}
			if (num > Convert.ToDouble(this.numericUpDownZS.Maximum) || num < Convert.ToDouble(this.numericUpDownZS.Minimum))
			{
				MessageForm messageForm = new MessageForm("提示", "数值超出范围！", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
			}
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
		private void numericUpDownZY_TextChanged(object sender, EventArgs e)
		{
			double num = 0.0;
			try
			{
				if (this.numericUpDownZY.Text.Trim().Length == 0)
				{
					return;
				}
				num = Convert.ToDouble(this.numericUpDownZY.Text);
			}
			catch (Exception ex)
			{
				MessageForm messageForm = new MessageForm("提示", "请输入数值类型", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
				this.numericUpDownZY.Text = "";
				Logger.wirte(1, ex.Message);
				return;
			}
			if (num > Convert.ToDouble(this.numericUpDownZY.Maximum) || num < Convert.ToDouble(this.numericUpDownZY.Minimum))
			{
				MessageForm messageForm = new MessageForm("提示", "数值超出范围！", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
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
		private void SetSubmitControls(bool isSubmit)
		{
			try
			{
				this.buttonSubmit.Enabled = !isSubmit;
				this.buttonCancel.Enabled = !isSubmit;
				this.RefreshGNFlag = !isSubmit;
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
				this.gbMask.Visible = isActive;
				this.loadingCircle1.set_Active(isActive);
				if (isActive)
				{
					this.gbS.Visible = !isActive;
					this.gbX.Visible = !isActive;
				}
				else
				{
					string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
					if (key == TradeType.ShiJiaDan.ToString("d"))
					{
						this.gbS.Visible = true;
						this.gbS.Enabled = true;
						this.gbX.Visible = false;
						this.gbX.Enabled = false;
					}
					else if (key == TradeType.XianJiaDan.ToString("d"))
					{
						this.gbS.Visible = false;
						this.gbS.Enabled = false;
						this.gbX.Visible = true;
						this.gbX.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private new void HandleCreated()
		{
			try
			{
				while (!base.IsHandleCreated)
				{
					Thread.Sleep(100);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void buttonSubmit_Click(object sender, EventArgs e)
		{
			try
			{
				this.SetSubmitControls(true);
				try
				{
					Convert.ToInt32(this.numericUpDownQty.Text);
				}
				catch (Exception)
				{
					this.ShowError("请填写正确交易手数！");
					this.numericUpDownQty.Focus();
					this.SetSubmitControls(false);
					return;
				}
				this.UpdateOrderConfirmSetting();
				string key = ((CBListItem)this.comboBoxTradeType.SelectedItem).Key;
				if (!this.DataCheckOn(key))
				{
					this.SetSubmitControls(false);
				}
				else
				{
					if (key == TradeType.ShiJiaDan.ToString("d"))
					{
						this.SubmitOrderS();
					}
					else if (key == TradeType.XianJiaDan.ToString("d"))
					{
						this.SubmitOrderX();
					}
					IniFile iniFile = new IniFile(string.Format(Global.ConfigPath + "{0}Trade.ini", Global.UserID));
					iniFile.IniWriteValue("PWarehouseForm", "checkBoxDianCha", this.checkBoxDianCha.Checked.ToString());
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillOrderRequestVOS(OrderRequestVO orderRequestVO, string commodityId, long qty, short buyOrSell, double currentPrice, short dianCha, short tradeType, bool isFSJC, string otherId, long fsjcQty)
		{
			try
			{
				orderRequestVO.UserID = Global.UserID;
				orderRequestVO.MarketID = string.Empty;
				orderRequestVO.BuySell = buyOrSell;
				orderRequestVO.CommodityID = commodityId;
				orderRequestVO.Price = currentPrice;
				orderRequestVO.Quantity = qty;
				orderRequestVO.SettleBasis = 2;
				orderRequestVO.DotDiff = dianCha;
				orderRequestVO.TradeType = tradeType;
				orderRequestVO.IsFSJC = isFSJC;
				orderRequestVO.OtherID = otherId;
				orderRequestVO.FSJCQuantity = fsjcQty;
				if (this.IsCloseSpecificOrder)
				{
					orderRequestVO.CloseMode = 2;
					orderRequestVO.HoldingID = this.CloseCommodityInfoList[commodityId].HoldingID;
				}
				else
				{
					orderRequestVO.CloseMode = 1;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillOrderRequestVOX(SetLossProfitRequestVO orderRequestVO, string commodityId, short buyOrSell, long holdingID, double stopLoss, double stopProfit)
		{
			try
			{
				orderRequestVO.BuySellType = buyOrSell.ToString();
				orderRequestVO.CommodityID = commodityId;
				orderRequestVO.HoldingID = holdingID;
				orderRequestVO.StopLoss = stopLoss;
				orderRequestVO.StopProfit = stopProfit;
				orderRequestVO.UserID = Global.UserID;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private OrderRequestVO FillOrderRequestVOSFSJC(OrderRequestVO orderRequestVO)
		{
			OrderRequestVO orderRequestVO2 = new OrderRequestVO();
			try
			{
				orderRequestVO2.UserID = orderRequestVO.UserID;
				orderRequestVO2.MarketID = orderRequestVO.MarketID;
				orderRequestVO2.BuySell = orderRequestVO.BuySell;
				orderRequestVO2.CommodityID = orderRequestVO.CommodityID;
				orderRequestVO2.Price = orderRequestVO.Price;
				orderRequestVO2.Quantity = orderRequestVO.FSJCQuantity;
				orderRequestVO2.SettleBasis = 1;
				orderRequestVO2.TradeType = 1;
				orderRequestVO2.OtherID = orderRequestVO.OtherID;
				orderRequestVO2.DotDiff = orderRequestVO.DotDiff;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return orderRequestVO2;
		}
		private void SubmitOrderS()
		{
			try
			{
				if (this.checkBoxFSJC.Checked)
				{
					try
					{
						Convert.ToInt32(this.numericUpDownFSJC.Text);
					}
					catch (Exception)
					{
						this.ShowError("请填写正确反手建仓数量！");
						this.SetSubmitControls(false);
						return;
					}
				}
				string key = ((CBListItem)this.comboBoxCommodity.SelectedItem).Key;
				short tradeType = Convert.ToInt16(((CBListItem)this.comboBoxTradeType.SelectedItem).Key);
				long num = 0L;
				long.TryParse(this.numericUpDownQty.Value.ToString(), out num);
				short buyOrSell = Convert.ToInt16(this.GetBuySell());
				double currentPrice = Convert.ToDouble(this.textBoxSHQ.Text);
				short dianCha = 0;
				if (this.checkBoxDianCha.Checked && !short.TryParse(this.numericUpDownDianCha.Text.ToString(), out dianCha))
				{
					this.ShowError("请输入正确的'允许成交价和报价的最大点差'！");
					this.SetSubmitControls(false);
				}
				else
				{
					bool @checked = this.checkBoxConfirm.Checked;
					bool checked2 = this.checkBoxFSJC.Checked;
					string key2 = ((CBListItem)this.comboBoxTrader.SelectedItem).Key;
					long num2 = 0L;
					long.TryParse(this.numericUpDownFSJC.Value.ToString(), out num2);
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
						if (!commodityInfo.B_L_P)
						{
							this.ShowError("没有买入市价平仓权限！");
							this.SetSubmitControls(false);
							return;
						}
					}
					else if (buyOrSell.ToString() == BuySell.Sell.ToString("d") && !commodityInfo.S_L_P)
					{
						this.ShowError("没有卖出市价平仓权限！");
						this.SetSubmitControls(false);
						return;
					}
					if (num <= 0L || num > this.QtyMaxValue)
					{
						this.ShowError("请输入正确的平仓数量！" + string.Format("可平仓数量最小{0}手、最大{1}手！", this.QtyMinValue.ToString("f0"), this.QtyMaxValue.ToString("f0")));
						this.numericUpDownQty.Focus();
						this.SetSubmitControls(false);
					}
					else
					{
						this.FillOrderRequestVOS(orderRequestVO, key, num, buyOrSell, currentPrice, dianCha, tradeType, checked2, key2, num2);
						if (@checked)
						{
							string message = string.Empty;
							if (this._ParentForm.dataProcess.IsAgency)
							{
								if (this.checkBoxFSJC.Checked)
								{
									message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n反手建仓数量：{6}\r\n\r\n确定下单吗？  ", new object[]
									{
										Global.AgencyCommodityData[orderRequestVO.CommodityID].CommodityName,
										orderRequestVO.CommodityID,
										orderRequestVO.Price,
										orderRequestVO.Quantity,
										Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
										Global.BuySellStrArr[(int)orderRequestVO.BuySell],
										num2
									});
								}
								else
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
							}
							else if (this.checkBoxFSJC.Checked)
							{
								message = string.Format("商品：{0}[{1}]\r\n商品价格：{2}   商品数量：{3}\r\n买卖方式：{4}{5}\r\n反手建仓数量：{6}\r\n\r\n确定下单吗？  ", new object[]
								{
									Global.CommodityData[orderRequestVO.CommodityID].CommodityName,
									orderRequestVO.CommodityID,
									orderRequestVO.Price,
									orderRequestVO.Quantity,
									Global.SettleBasisStrArr[(int)orderRequestVO.SettleBasis],
									Global.BuySellStrArr[(int)orderRequestVO.BuySell],
									num2
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
							Size textSize = new Size(231, 90);
							messageForm.textSize = textSize;
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
			}
			catch (Exception ex)
			{
				string.Format("错误：{0} 堆栈：{1}", ex.Message, ex.StackTrace);
				Logger.wirte(ex);
				this.ShowError(ex.Message);
			}
		}
		private void SubmitOrderX()
		{
			try
			{
				string key = ((CBListItem)this.comboBoxCommodity.SelectedItem).Key;
				Convert.ToInt16(((CBListItem)this.comboBoxTradeType.SelectedItem).Key);
				long num = 0L;
				long.TryParse(this.numericUpDownQty.Value.ToString(), out num);
				short num2 = Convert.ToInt16(this.GetBuySell());
				Convert.ToDouble(this.textBoxXHQ.Text);
				bool @checked = this.checkBoxConfirm.Checked;
				bool checked2 = this.checkBoxZS.Checked;
				bool checked3 = this.checkBoxZY.Checked;
				double num3 = 0.0;
				try
				{
					if (checked2)
					{
						num3 = Convert.ToDouble(this.numericUpDownZS.Text);
					}
				}
				catch
				{
					this.ShowError("请输入正确的止损价格！");
					this.SetSubmitControls(false);
					return;
				}
				double num4 = 0.0;
				try
				{
					if (checked3)
					{
						num4 = Convert.ToDouble(this.numericUpDownZY.Text);
					}
				}
				catch
				{
					this.ShowError("请输入正确的止盈价格！");
					this.SetSubmitControls(false);
					return;
				}
				double num5 = 0.0;
				double num6 = 0.0;
				if (checked2 && this.textBoxZS.Text.Trim().Length > 0)
				{
					num5 = Convert.ToDouble(this.textBoxZS.Text);
				}
				if (checked3 && this.textBoxZY.Text.Trim().Length > 0)
				{
					num6 = Convert.ToDouble(this.textBoxZY.Text);
				}
				long holdingID = this._CloseCommodityInfoList[key].HoldingID;
				CommodityInfo commodityInfo;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					commodityInfo = Global.AgencyCommodityData[key];
				}
				else
				{
					commodityInfo = Global.CommodityData[key];
				}
				SetLossProfitRequestVO setLossProfitRequestVO = new SetLossProfitRequestVO();
				if (num2.ToString() == BuySell.Buy.ToString("d"))
				{
					if (!commodityInfo.B_S_L && checked2)
					{
						this.ShowError("没有买入止损权限！");
						this.SetSubmitControls(false);
						return;
					}
					if (!commodityInfo.B_S_P && checked3)
					{
						this.ShowError("没有买入止盈权限！");
						this.SetSubmitControls(false);
						return;
					}
				}
				else if (num2.ToString() == BuySell.Sell.ToString("d"))
				{
					if (!commodityInfo.S_S_L && checked2)
					{
						this.ShowError("没有卖出止损权限！");
						this.SetSubmitControls(false);
						return;
					}
					if (!commodityInfo.S_S_P && checked3)
					{
						this.ShowError("没有卖出止盈权限！");
						this.SetSubmitControls(false);
						return;
					}
				}
				if (num <= 0L || num > this.QtyMaxValue)
				{
					this.ShowError("请输入正确的平仓数量！" + string.Format("可平仓数量最小{0}手、最大{1}手！", this.QtyMinValue.ToString("f0"), this.QtyMaxValue.ToString("f0")));
					this.numericUpDownQty.Focus();
					this.SetSubmitControls(false);
				}
				else if (!checked2 && !checked3)
				{
					this.ShowError("请设置止损止盈条件！");
					this.SetSubmitControls(false);
				}
				else if (checked2 && num3 == 0.0)
				{
					this.ShowError("请设置止损条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZS.Select(0, 100);
					this.numericUpDownZS.Focus();
				}
				else if (checked3 && num4 == 0.0)
				{
					this.ShowError("请设置止盈条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZY.Select(0, 100);
					this.numericUpDownZY.Focus();
				}
				else if (checked2 && ((this.labelZS.Text == ">" && num3 <= num5) || (this.labelZS.Text == "<" && num3 >= num5)))
				{
					this.ShowError("止损价格不符合条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZS.Select(0, 100);
					this.numericUpDownZS.Focus();
				}
				else if (checked3 && ((this.labelZY.Text == ">" && num4 <= num6) || (this.labelZY.Text == "<" && num4 >= num6)))
				{
					this.ShowError("止盈价格不符合条件！");
					this.SetSubmitControls(false);
					this.numericUpDownZY.Select(0, 100);
					this.numericUpDownZY.Focus();
				}
				else
				{
					this.FillOrderRequestVOX(setLossProfitRequestVO, key, num2, holdingID, num3, num4);
					if (@checked)
					{
						string text = checked2 ? string.Format("\r\n止损价：{0}", num3) : string.Empty;
						string text2 = checked3 ? string.Format("\r\n止盈价：{0}", num4) : string.Empty;
						string message = string.Empty;
						if (this._ParentForm.dataProcess.IsAgency)
						{
							message = string.Format("商品：{0}[{1}]\r\n单号：{2}\r\n 商品数量：{3}\r\n买卖方式：{4}{5}{6}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.AgencyCommodityData[setLossProfitRequestVO.CommodityID].CommodityName,
								setLossProfitRequestVO.CommodityID,
								holdingID,
								num,
								Global.BuySellStrArr[(int)num2],
								text,
								text2
							});
						}
						else
						{
							message = string.Format("商品：{0}[{1}]\r\n单号：{2}\r\n 商品数量：{3}\r\n买卖方式：{4}{5}{6}\r\n\r\n确定下单吗？  ", new object[]
							{
								Global.CommodityData[setLossProfitRequestVO.CommodityID].CommodityName,
								setLossProfitRequestVO.CommodityID,
								holdingID,
								num,
								Global.BuySellStrArr[(int)num2],
								text,
								text2
							});
						}
						MessageForm messageForm = new MessageForm("订单信息", message, 0, StatusBarType.Message);
						messageForm.Size = new Size(300, 180);
						messageForm.textSize = new Size(231, 100);
						messageForm.Owner = this;
						messageForm.ShowDialog();
						messageForm.Dispose();
						if (messageForm.isOK)
						{
							this.OrderX(setLossProfitRequestVO);
						}
						else
						{
							this.SetSubmitControls(false);
						}
					}
					else
					{
						this.OrderX(setLossProfitRequestVO);
					}
				}
			}
			catch (Exception ex)
			{
				string.Format("错误：{0} 堆栈：{1}", ex.Message, ex.StackTrace);
				Logger.wirte(ex);
				this.ShowError(ex.Message);
			}
		}
		private void Order(OrderRequestVO orderRequestVO)
		{
			try
			{
				this.ActiveSubmitMask(true);
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				threadPoolParameter.obj = orderRequestVO;
				threadPoolParameter.Semaphores = (AutoResetEvent)this.waitHandles[0];
				Logger.wirte(1, "下单线程提交，等待程序处理");
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
				OrderRequestVO orderRequestVO = (OrderRequestVO)threadPoolParameter.obj;
				ResponseVO responseVO = this._ParentForm.dataProcess.Order(orderRequestVO);
				ResponseVO responseVO2 = null;
				if (orderRequestVO.IsFSJC && responseVO.RetCode == 0L)
				{
					OrderRequestVO req = this.FillOrderRequestVOSFSJC(orderRequestVO);
					responseVO2 = this._ParentForm.dataProcess.Order(req);
				}
				PWarehouseForm.ResponseVOSCallback method = new PWarehouseForm.ResponseVOSCallback(this.OrderMessage);
				this.HandleCreated();
				base.BeginInvoke(method, new object[]
				{
					responseVO,
					responseVO2
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
		private void OrderMessage(ResponseVO responseVO, ResponseVO responseVOFSJC)
		{
			try
			{
				this.comboBoxTradeType.Focus();
				if (responseVO != null && responseVO.RetCode == 0L)
				{
					if (responseVOFSJC != null)
					{
						if (responseVOFSJC != null && responseVOFSJC.RetCode == 0L)
						{
							MessageForm messageForm = new MessageForm("提示", "操作成功！", 1, StatusBarType.Success);
							messageForm.Owner = this;
							messageForm.ShowDialog();
							messageForm.Dispose();
							this._IsCloseButtonOKOrCancel = true;
							base.Close();
						}
						else
						{
							string message = string.Format("{0} \r\n失败原因:{1}", "平仓成功，建仓失败!", responseVOFSJC.RetMessage);
							MessageForm messageForm2 = new MessageForm("错误", message, 1, StatusBarType.Error);
							messageForm2.Owner = this;
							messageForm2.ShowDialog();
							messageForm2.Dispose();
							this._IsCloseButtonOKOrCancel = true;
							base.Close();
						}
					}
					else
					{
						MessageForm messageForm3 = new MessageForm("提示", "操作成功！", 1, StatusBarType.Success);
						messageForm3.Owner = this;
						messageForm3.ShowDialog();
						messageForm3.Dispose();
						this._IsCloseButtonOKOrCancel = true;
						base.Close();
					}
				}
				else
				{
					SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
					sysTimeQueryRequestVO.UserID = Global.UserID;
					SysTimeQueryResponseVO sysTime = this._ParentForm.dataProcess.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
					if (sysTime.RetCode != 0L)
					{
						MessageForm messageForm4 = new MessageForm("错误", "您的网络状态异常!", 1, StatusBarType.Error);
						messageForm4.Owner = this;
						messageForm4.ShowDialog();
						messageForm4.Dispose();
						base.Close();
						return;
					}
					if (IniData.GetInstance().FailShowDialog)
					{
						MessageForm messageForm5 = new MessageForm("错误", responseVO.RetMessage, 1, StatusBarType.Error);
						messageForm5.Owner = this;
						messageForm5.ShowDialog();
						messageForm5.Dispose();
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
		private void OrderX(SetLossProfitRequestVO orderRequestVO)
		{
			try
			{
				this.ActiveSubmitMask(true);
				Logger.wirte(1, "下单线程提交，等待程序处理");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				threadPoolParameter.obj = orderRequestVO;
				threadPoolParameter.Semaphores = (AutoResetEvent)this.waitHandles[0];
				WaitCallback callBack = new WaitCallback(this.OrderX);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void OrderX(object _orderRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_orderRequestVO;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				SetLossProfitRequestVO lossProfit = (SetLossProfitRequestVO)threadPoolParameter.obj;
				ResponseVO responseVO = this._ParentForm.dataProcess.SetLossProfit(lossProfit);
				PWarehouseForm.ResponseVOCallback method = new PWarehouseForm.ResponseVOCallback(this.OrderMessageX);
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
		private void OrderMessageX(ResponseVO responseVO)
		{
			try
			{
				this.comboBoxTradeType.Focus();
				if (responseVO != null && responseVO.RetCode == 0L)
				{
					MessageForm messageForm = new MessageForm("提示", "操作成功！", 1, StatusBarType.Success);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					this._IsCloseButtonOKOrCancel = true;
					base.Close();
				}
				else if (IniData.GetInstance().FailShowDialog)
				{
					MessageForm messageForm2 = new MessageForm("错误", responseVO.RetMessage, 1, StatusBarType.Error);
					messageForm2.Owner = this;
					messageForm2.ShowDialog();
					messageForm2.Dispose();
				}
				else
				{
					this.FillInfoText(responseVO.RetMessage, StatusBarType.Error, true);
				}
				this.SetSubmitControls(false);
				this.ActiveSubmitMask(false);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private string GetBuySell()
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
		private void PWarehouseForm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				Keys keyCode = e.KeyCode;
				if (keyCode == Keys.Escape)
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
		private void PWarehouseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			WaitHandle[] array = this.waitHandles;
			for (int i = 0; i < array.Length; i++)
			{
				WaitHandle waitHandle = array[i];
				waitHandle.WaitOne();
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
			this.FillInfoText(string.Format(msg, new object[0]), StatusBarType.Error, true);
		}
		private bool DataCheckOn(string tradeType)
		{
			bool flag = true;
			if (tradeType == TradeType.ShiJiaDan.ToString("d"))
			{
				if (this.checkBoxDianCha.Checked)
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
					if (Convert.ToInt32(this.numericUpDownDianCha.Text) > this.DianChaMaxValue || Convert.ToInt32(this.numericUpDownDianCha.Text) < this.DianChaMinValue)
					{
						string msg = "请填写正确点差。" + string.Format("可设置点差范围最小{0}、最大{1}！", this.DianChaMinValue.ToString("f0"), this.DianChaMaxValue.ToString("f0"));
						flag = false;
						this.numericUpDownDianCha.Focus();
						this.ShowError(msg);
						return flag;
					}
				}
				if (!this.checkBoxFSJC.Checked)
				{
					return flag;
				}
				try
				{
					Convert.ToInt32(this.numericUpDownFSJC.Text);
				}
				catch (Exception)
				{
					string msg = "请输入正确的反手建仓数量！";
					flag = false;
					this.numericUpDownFSJC.Focus();
					this.ShowError(msg);
					bool result = flag;
					return result;
				}
				if (Convert.ToInt32(this.numericUpDownFSJC.Text) > this.FSJCMaxValue || Convert.ToInt32(this.numericUpDownFSJC.Text) < this.FSJCMinValue)
				{
					string msg = "请填写正确反手建仓数量。" + string.Format("可设置反手建仓范围最小{0}、最大{1}！", this.FSJCMinValue.ToString("f0"), this.FSJCMaxValue.ToString("f0"));
					flag = false;
					this.numericUpDownFSJC.Focus();
					this.ShowError(msg);
					return flag;
				}
				return flag;
			}
			return flag;
		}
	}
}
