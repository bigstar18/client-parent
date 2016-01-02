namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Order;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class CrazyOrder : Form
    {
        private double bPrice;
        private MyButton btn_BL;
        private MyButton btn_BO;
        private MyButton btn_SL;
        private MyButton btn_SO;
        private bool buttonClick;
        private short BuySell;
        private MyCombobox comboCommodity_co;
        private MyCombobox comboMarKet_co;
        private MyCombobox comboTranc_co;
        private IContainer components;
        private bool ConnectHQ;
        private string curCommodityMode = "";
        private bool displayInfo = true;
        private string ErrorPriceMassage = "价格不能为0！！！";
        private GroupBox groupBoxCO;
        private static volatile CrazyOrder instance;
        private bool isBLFocused;
        private bool isBOFocused;
        private bool isBtnFocused;
        private bool isDirectfirm;
        private bool isMove;
        private bool isSLFocused;
        private bool isSOFocused;
        private Label labComCode_co;
        private Label labelLargestTN_co;
        private Label labelMarKet_co;
        private Label labPrice_co;
        private Label labQty_co;
        private Label labTrancCode_co;
        private NumericUpDown numericPrice_co;
        private NumericUpDown numericQty_co;
        private string NumIsNotZero = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
        private OrderMessageInfoCallBack OrderMessageInfo;
        private Panel panel_co;
        private short SettleBasis;
        private double sPrice;
        private SubmitOrderInfo submitOrderInfo;
        private TextBox tbTranc_co;
        private string TitleInfo = string.Empty;

        public CrazyOrder()
        {
            this.InitializeComponent();
            Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
            OperationManager.GetInstance().submitOrderOperation.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Combine(OperationManager.GetInstance().submitOrderOperation.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
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
            if (num > 0.0)
            {
                this.submitOrderInfo.price = num;
            }
            else
            {
                this.numericPrice_co.Focus();
                this.Text = this.TitleInfo + this.ErrorPriceMassage;
                return;
            }
            int num2 = Tools.StrToInt(this.numericQty_co.Text, 0);
            if (num2 > 0)
            {
                this.submitOrderInfo.qty = num2;
            }
            else
            {
                this.numericQty_co.Focus();
                this.Text = this.TitleInfo + this.NumIsNotZero;
                return;
            }
            OperationManager.GetInstance().orderOperation.orderType = OrderType.CrazyOrder;
            OperationManager.GetInstance().submitOrderOperation.SubmitOrderInfo(this.submitOrderInfo, 0);
        }

        private void comboCommodity_co_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.numericPrice_co.Value = 0M;
            this.numericQty_co.Value = 0M;
            this.numericPrice_co.Value = 0M;
            this.labelLargestTN_co.Text = "";
        }

        private void ComboCommodityLoad()
        {
            this.comboCommodity_co.Items.Clear();
            try
            {
                if (OperationManager.GetInstance().myCommodityList.Count > 1)
                {
                    foreach (string str in OperationManager.GetInstance().myCommodityList)
                    {
                        if (str != OperationManager.GetInstance().StrAll)
                        {
                            this.comboCommodity_co.Items.Add(str);
                        }
                    }
                }
                else
                {
                    foreach (string str2 in OperationManager.GetInstance().commodityList)
                    {
                        if (str2 != OperationManager.GetInstance().StrAll)
                        {
                            this.comboCommodity_co.Items.Add(str2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "获取商品信息错误：" + exception.Message);
            }
            if (this.comboCommodity_co.Items.Count > 0)
            {
                this.comboCommodity_co.SelectedIndex = 0;
            }
        }

        private void ComboMarKetLoad()
        {
            ArrayList list = new ArrayList();
            if (Global.MarketHT != null)
            {
                foreach (DictionaryEntry entry in Global.MarketHT)
                {
                    MarkeInfo info = (MarkeInfo)entry.Value;
                    if (info != null)
                    {
                        list.Add(new AddValue(info.ShortName, info.MarketID));
                    }
                }
                this.comboMarKet_co.DisplayMember = "Display";
                this.comboMarKet_co.ValueMember = "Value";
                this.comboMarKet_co.DataSource = null;
                this.comboMarKet_co.DataSource = list;
            }
            this.comboMarKet_co.SelectedIndex = 0;
            this.labelMarKet_co.Visible = true;
            this.comboMarKet_co.Visible = true;
            this.tbTranc_co.Visible = false;
            this.labTrancCode_co.Visible = false;
            this.comboTranc_co.Visible = false;
        }

        private void ComboTrancLoad()
        {
            this.comboTranc_co.Items.Clear();
            try
            {
                if (OperationManager.GetInstance().myTransactionsList.Count > 1)
                {
                    foreach (string str in OperationManager.GetInstance().myTransactionsList)
                    {
                        if (str != OperationManager.GetInstance().StrAll)
                        {
                            this.comboTranc_co.Items.Add(str.Substring(str.Length - 2));
                        }
                    }
                }
                else
                {
                    foreach (string str2 in OperationManager.GetInstance().transactionsList)
                    {
                        if (str2 != OperationManager.GetInstance().StrAll)
                        {
                            this.comboTranc_co.Items.Add(str2.Substring(str2.Length - 2));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.Message);
            }
            if (this.comboTranc_co.Items.Count > 0)
            {
                this.comboTranc_co.SelectedIndex = 0;
            }
        }

        private void CommdityInfo(CommodityInfo commodityInfo)
        {
            if (commodityInfo != null)
            {
                string str = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
                string str2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
                string text1 = string.Concat(new object[] { str2, commodityInfo.CommodityName, "  ", str, commodityInfo.SpreadDown, " – ", commodityInfo.SpreadUp });
                if (commodityInfo.Spread < 0.1)
                {
                    this.numericPrice_co.DecimalPlaces = 2;
                }
                else if (commodityInfo.Spread < 1.0)
                {
                    this.numericPrice_co.DecimalPlaces = 1;
                }
                else
                {
                    this.numericPrice_co.DecimalPlaces = 0;
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
                else if (commodityInfo.MinQty < 1.0)
                {
                    this.numericQty_co.DecimalPlaces = 1;
                }
                else
                {
                    this.numericQty_co.DecimalPlaces = 0;
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
            //string str = Global.M_ResourceManager.GetString("TradeStr_CrazyOrder_Str");
            this.Text = "下单助手";
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

        private void CrazyOrder_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.buttonClick)
            {
                this.buttonClick = false;
            }
            else
            {
                if (e.KeyValue == 13)
                {
                    if (this.comboTranc_co.Focused || this.comboMarKet_co.Focused)
                    {
                        this.comboCommodity_co.Focus();
                    }
                    else if (this.comboCommodity_co.Focused)
                    {
                        this.numericPrice_co.Focus();
                    }
                    else if (this.numericPrice_co.Focused)
                    {
                        this.numericQty_co.Focus();
                    }
                    else if (this.numericQty_co.Focused)
                    {
                        if (this.isBtnFocused)
                        {
                            if (this.isBOFocused && this.btn_BO.Enabled)
                            {
                                this.btn_BO.Focus();
                            }
                            else if (this.isBLFocused && this.btn_BL.Enabled)
                            {
                                this.btn_BL.Focus();
                            }
                            else if (this.isSOFocused && this.btn_SO.Enabled)
                            {
                                this.btn_SO.Focus();
                            }
                            else if (this.isSLFocused && this.btn_SL.Enabled)
                            {
                                this.btn_SL.Focus();
                            }
                        }
                        else if (this.btn_BO.Enabled)
                        {
                            this.btn_BO.Focus();
                        }
                        else if (this.btn_BL.Enabled)
                        {
                            this.btn_BL.Focus();
                        }
                        else if (this.btn_SO.Enabled)
                        {
                            this.btn_SO.Focus();
                        }
                        else if (this.btn_SL.Enabled)
                        {
                            this.btn_SL.Focus();
                        }
                    }
                    else if (this.comboMarKet_co.Visible)
                    {
                        this.comboMarKet_co.Focus();
                    }
                    else if (this.comboTranc_co.Visible)
                    {
                        this.comboTranc_co.Focus();
                    }
                    else
                    {
                        this.comboCommodity_co.Focus();
                    }
                }
                this.displayInfo = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (instance != null)
            {
                instance = null;
            }
            Global.SetOrderInfo -= new Global.SetOrderInfoCallBack(this.SetOrderInfo);
            OperationManager.GetInstance().submitOrderOperation.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Remove(OperationManager.GetInstance().submitOrderOperation.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetCom(object o)
        {
        }

        private void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void InitializeComponent()
        {
            this.groupBoxCO = new GroupBox();
            this.panel_co = new Panel();
            this.btn_SO = new MyButton();
            this.btn_SL = new MyButton();
            this.btn_BL = new MyButton();
            this.btn_BO = new MyButton();
            this.comboTranc_co = new MyCombobox();
            this.labQty_co = new Label();
            this.numericQty_co = new NumericUpDown();
            this.labelLargestTN_co = new Label();
            this.tbTranc_co = new TextBox();
            this.comboCommodity_co = new MyCombobox();
            this.numericPrice_co = new NumericUpDown();
            this.labPrice_co = new Label();
            this.labComCode_co = new Label();
            this.comboMarKet_co = new MyCombobox();
            this.labTrancCode_co = new Label();
            this.labelMarKet_co = new Label();
            this.groupBoxCO.SuspendLayout();
            this.panel_co.SuspendLayout();
            this.numericQty_co.BeginInit();
            this.numericPrice_co.BeginInit();
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
            this.groupBoxCO.Size = new Size(0xc2, 0xde);
            this.groupBoxCO.TabIndex = 8;
            this.groupBoxCO.TabStop = false;
            this.groupBoxCO.Text = "委托";
            this.panel_co.Controls.Add(this.btn_SO);
            this.panel_co.Controls.Add(this.btn_SL);
            this.panel_co.Controls.Add(this.btn_BL);
            this.panel_co.Controls.Add(this.btn_BO);
            this.panel_co.Location = new Point(11, 0x98);
            this.panel_co.Margin = new Padding(0);
            this.panel_co.Name = "panel_co";
            this.panel_co.Size = new Size(170, 0x3e);
            this.panel_co.TabIndex = 0x23;
            this.btn_SO.BackColor = Color.LightSteelBlue;
            this.btn_SO.FlatStyle = FlatStyle.Popup;
            this.btn_SO.ForeColor = Color.Green;
            this.btn_SO.Location = new Point(0x5d, 3);
            this.btn_SO.Name = "btn_SO";
            this.btn_SO.Size = new Size(0x4b, 0x17);
            this.btn_SO.TabIndex = 6;
            this.btn_SO.Text = "卖订立";
            this.btn_SO.UseVisualStyleBackColor = false;
            this.btn_SO.Click += new EventHandler(this.btn_SO_Click);
            this.btn_SL.BackColor = Color.LightSteelBlue;
            this.btn_SL.FlatStyle = FlatStyle.Popup;
            this.btn_SL.ForeColor = Color.Green;
            this.btn_SL.Location = new Point(0x5d, 0x23);
            this.btn_SL.Name = "btn_SL";
            this.btn_SL.Size = new Size(0x4b, 0x17);
            this.btn_SL.TabIndex = 7;
            this.btn_SL.Text = "卖转让";
            this.btn_SL.UseVisualStyleBackColor = false;
            this.btn_SL.Click += new EventHandler(this.btn_SL_Click);
            this.btn_BL.BackColor = Color.LightSteelBlue;
            this.btn_BL.FlatStyle = FlatStyle.Popup;
            this.btn_BL.ForeColor = Color.Red;
            this.btn_BL.Location = new Point(3, 0x23);
            this.btn_BL.Name = "btn_BL";
            this.btn_BL.Size = new Size(0x4b, 0x17);
            this.btn_BL.TabIndex = 5;
            this.btn_BL.Text = "买转让";
            this.btn_BL.UseVisualStyleBackColor = false;
            this.btn_BL.Click += new EventHandler(this.btn_BL_Click);
            this.btn_BO.BackColor = Color.LightSteelBlue;
            this.btn_BO.FlatStyle = FlatStyle.Popup;
            this.btn_BO.ForeColor = Color.Red;
            this.btn_BO.Location = new Point(3, 3);
            this.btn_BO.Name = "btn_BO";
            this.btn_BO.Size = new Size(0x4b, 0x17);
            this.btn_BO.TabIndex = 4;
            this.btn_BO.Text = "买订立";
            this.btn_BO.UseVisualStyleBackColor = false;
            this.btn_BO.Click += new EventHandler(this.btn_BO_Click);
            this.comboTranc_co.Location = new Point(0x80, 12);
            this.comboTranc_co.MaxLength = 2;
            this.comboTranc_co.Name = "comboTranc_co";
            this.comboTranc_co.Size = new Size(0x25, 20);
            this.comboTranc_co.TabIndex = 0;
            this.labQty_co.AutoSize = true;
            this.labQty_co.ImeMode = ImeMode.NoControl;
            this.labQty_co.Location = new Point(15, 0x69);
            this.labQty_co.Name = "labQty_co";
            this.labQty_co.Size = new Size(0x41, 12);
            this.labQty_co.TabIndex = 9;
            this.labQty_co.Text = "委托数量：";
            this.labQty_co.TextAlign = ContentAlignment.BottomLeft;
            this.numericQty_co.Location = new Point(0x58, 0x66);
            int[] bits = new int[4];
            bits[0] = 0xf423f;
            this.numericQty_co.Maximum = new decimal(bits);
            this.numericQty_co.Name = "numericQty_co";
            this.numericQty_co.Size = new Size(0x4e, 0x15);
            this.numericQty_co.TabIndex = 3;
            this.numericQty_co.Enter += new EventHandler(this.numericQty_co_Enter);
            this.numericQty_co.KeyUp += new KeyEventHandler(this.numericQty_co_KeyUp);
            this.labelLargestTN_co.AutoSize = true;
            this.labelLargestTN_co.ImeMode = ImeMode.NoControl;
            this.labelLargestTN_co.Location = new Point(15, 130);
            this.labelLargestTN_co.Name = "labelLargestTN_co";
            this.labelLargestTN_co.Size = new Size(0x4d, 12);
            this.labelLargestTN_co.TabIndex = 30;
            this.labelLargestTN_co.Text = "最大可交易量";
            this.labelLargestTN_co.Visible = false;
            this.tbTranc_co.BackColor = Color.White;
            this.tbTranc_co.Location = new Point(0x58, 12);
            this.tbTranc_co.Multiline = true;
            this.tbTranc_co.Name = "tbTranc_co";
            this.tbTranc_co.ReadOnly = true;
            this.tbTranc_co.Size = new Size(0x2a, 20);
            this.tbTranc_co.TabIndex = 0x22;
            this.tbTranc_co.TabStop = false;
            this.comboCommodity_co.Location = new Point(0x58, 40);
            this.comboCommodity_co.MaxLength = 6;
            this.comboCommodity_co.Name = "comboCommodity_co";
            this.comboCommodity_co.Size = new Size(0x4e, 20);
            this.comboCommodity_co.TabIndex = 1;
            this.comboCommodity_co.SelectedIndexChanged += new EventHandler(this.comboCommodity_co_SelectedIndexChanged);
            this.numericPrice_co.Location = new Point(0x58, 70);
            int[] numArray2 = new int[4];
            numArray2[0] = 0xf423f;
            this.numericPrice_co.Maximum = new decimal(numArray2);
            this.numericPrice_co.Name = "numericPrice_co";
            this.numericPrice_co.Size = new Size(0x4e, 0x15);
            this.numericPrice_co.TabIndex = 2;
            this.numericPrice_co.Enter += new EventHandler(this.numericPrice_co_Enter);
            this.numericPrice_co.KeyUp += new KeyEventHandler(this.numericPrice_co_KeyUp);
            this.labPrice_co.AutoSize = true;
            this.labPrice_co.Font = new Font("宋体", 9f);
            this.labPrice_co.ForeColor = SystemColors.ControlText;
            this.labPrice_co.ImeMode = ImeMode.NoControl;
            this.labPrice_co.Location = new Point(15, 0x4b);
            this.labPrice_co.Name = "labPrice_co";
            this.labPrice_co.Size = new Size(0x41, 12);
            this.labPrice_co.TabIndex = 8;
            this.labPrice_co.Text = "委托价格：";
            this.labPrice_co.TextAlign = ContentAlignment.BottomLeft;
            this.labComCode_co.AutoSize = true;
            this.labComCode_co.ImageAlign = ContentAlignment.MiddleLeft;
            this.labComCode_co.ImeMode = ImeMode.NoControl;
            this.labComCode_co.Location = new Point(15, 0x2d);
            this.labComCode_co.Name = "labComCode_co";
            this.labComCode_co.Size = new Size(0x41, 12);
            this.labComCode_co.TabIndex = 4;
            this.labComCode_co.Text = "商品代码：";
            this.labComCode_co.TextAlign = ContentAlignment.BottomLeft;
            this.comboMarKet_co.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboMarKet_co.Location = new Point(0x58, 12);
            this.comboMarKet_co.Name = "comboMarKet_co";
            this.comboMarKet_co.Size = new Size(0x4e, 20);
            this.comboMarKet_co.TabIndex = 30;
            this.comboMarKet_co.Visible = false;
            this.labTrancCode_co.AutoSize = true;
            this.labTrancCode_co.ImageAlign = ContentAlignment.MiddleLeft;
            this.labTrancCode_co.ImeMode = ImeMode.NoControl;
            this.labTrancCode_co.Location = new Point(15, 15);
            this.labTrancCode_co.Name = "labTrancCode_co";
            this.labTrancCode_co.Size = new Size(0x41, 12);
            this.labTrancCode_co.TabIndex = 2;
            this.labTrancCode_co.Text = "交易代码：";
            this.labTrancCode_co.TextAlign = ContentAlignment.BottomLeft;
            this.labelMarKet_co.AutoSize = true;
            this.labelMarKet_co.ImeMode = ImeMode.NoControl;
            this.labelMarKet_co.Location = new Point(0x15, 15);
            this.labelMarKet_co.Name = "labelMarKet_co";
            this.labelMarKet_co.Size = new Size(0x41, 12);
            this.labelMarKet_co.TabIndex = 0x1f;
            this.labelMarKet_co.Text = "市场标志：";
            this.labelMarKet_co.Visible = false;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0xc2, 0xde);
            base.Controls.Add(this.groupBoxCO);
            base.Name = "CrazyOrder";
            this.Text = "CrazyOrder";
            base.Load += new EventHandler(this.Crazy_Order_Load);
            base.KeyUp += new KeyEventHandler(this.CrazyOrder_KeyUp);
            this.groupBoxCO.ResumeLayout(false);
            this.groupBoxCO.PerformLayout();
            this.panel_co.ResumeLayout(false);
            this.numericQty_co.EndInit();
            this.numericPrice_co.EndInit();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.ResumeLayout(false);
        }

        public static CrazyOrder Instance()
        {
            if (instance == null)
            {
                lock (typeof(CrazyOrder))
                {
                    if (instance == null)
                    {
                        instance = new CrazyOrder();
                    }
                }
            }
            return instance;
        }

        private void labPrice_co_DoubleClick(object sender, EventArgs e)
        {
        }

        private void numericPrice_co_Enter(object sender, EventArgs e)
        {
            this.numericPrice_co.Select(0, this.numericPrice_co.Value.ToString().Length);
            WaitCallback callBack = new WaitCallback(this.GetCom);
            ThreadPool.QueueUserWorkItem(callBack, this.comboCommodity_co.Text);
        }

        private void numericPrice_co_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
        }

        private void numericQty_co_Enter(object sender, EventArgs e)
        {
            this.numericQty_co.Select(0, this.numericQty_co.Value.ToString().Length);
            WaitCallback callBack = new WaitCallback(this.GetCom);
            ThreadPool.QueueUserWorkItem(callBack, this.comboCommodity_co.Text);
        }

        private void numericQty_co_KeyUp(object sender, KeyEventArgs e)
        {
            Global.QtyKeyUp(sender, e);
        }

        private void Order(object _orderRequestVO)
        {
        }

        private void orderEnable(bool enable)
        {
            if (this.isBOFocused)
            {
                this.btn_BO.Enabled = enable;
            }
            else if (this.isBLFocused)
            {
                this.btn_BL.Enabled = enable;
            }
            else if (this.isSOFocused)
            {
                this.btn_SO.Enabled = enable;
            }
            else if (this.isSLFocused)
            {
                this.btn_SL.Enabled = enable;
            }
        }

        private void OrderInfoMessage(long retCode, string retMessage)
        {
            this.comboCommodity_co.Focus();
            if (IniData.GetInstance().ClearData)
            {
                if (this.numericPrice_co.Enabled)
                {
                    this.numericPrice_co.Value = 0M;
                }
                this.numericQty_co.Value = 0M;
            }
            if (retCode == 0L)
            {
                OperationManager.GetInstance().orderOperation.IsChangePrice = false;
                if (Global.StatusInfoFill != null)
                {
                    Global.StatusInfoFill(OperationManager.GetInstance().SussceOrder, Global.RightColor, true);
                }
            }
            else if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
            {
                MessageBox.Show(retMessage, OperationManager.GetInstance().ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(retMessage, Global.ErrorColor, true);
            }
        }

        private void OrderMessage(ResponseVO responseVO)
        {
        }

        private void OrderMessage(long retCode, string retMessage)
        {
            try
            {
                if (OperationManager.GetInstance().orderOperation.orderType == OrderType.CrazyOrder)
                {
                    this.OrderMessageInfo = new OrderMessageInfoCallBack(this.OrderInfoMessage);
                    this.HandleCreated();
                    base.Invoke(this.OrderMessageInfo, new object[] { retCode, retMessage });
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
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

        public void SetOrderInfo(string comboCommodityCode, double BuyPrice, double SellPrice)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            int index = comboCommodityCode.IndexOf("_");
            if (index != -1)
            {
                str = comboCommodityCode.Substring(0, index);
                str2 = comboCommodityCode.Substring(index + 1);
            }
            else
            {
                str2 = comboCommodityCode;
            }
            if (str2 != this.comboCommodity_co.Text)
            {
                this.ConnectHQ = true;
            }
            if (Global.MarketHT.Count == 1)
            {
                for (int i = 0; i < this.comboCommodity_co.Items.Count; i++)
                {
                    if (str2.Equals(this.comboCommodity_co.Items[i].ToString()))
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
                    AddValue value2 = (AddValue)this.comboMarKet_co.Items[j];
                    if (str.Equals(value2.Value))
                    {
                        this.comboMarKet_co.SelectedIndex = j;
                        break;
                    }
                }
                for (int k = 0; k < this.comboCommodity_co.Items.Count; k++)
                {
                    if (str2.Equals(this.comboCommodity_co.Items[k].ToString()))
                    {
                        this.comboCommodity_co.SelectedIndex = k;
                        break;
                    }
                }
            }
            this.numericQty_co.Focus();
        }

        private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);

        private delegate void ResponseVOCallback(ResponseVO resultMessage);
    }
}
