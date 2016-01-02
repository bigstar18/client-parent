namespace FuturesTrade.Gnnt.UI.Modules.Order
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Order;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;
    using TPME.Log;

    public class T8Order : UserControl
    {
        private byte BtnFlag;
        private bool buttonClick;
        private MyButton buttonSubmit;
        private int clickNum = 1;
        private MyCombobox comboBoxBuyOrSall;
        private MyCombobox comboBoxTransfer;
        private MyCombobox comboCommodity;
        private IContainer components;
        private bool isFirstLoad = true;
        private Label label_BS;
        private Label label_OG;
        private Label label_Price;
        private Label label_Qty;
        private Label labelAnswer;
        private Label labelCanTransfer;
        private Label labelLPrice;
        private Label labelPingZhong;
        private NumericUpDown numericLPrice;
        private NumericUpDown numericUpDownNum;
        private NumericUpDown numericUpDownPrice;
        private OperationManager operationManager = OperationManager.GetInstance();
        private OrderMessageInfoCallBack OrderMessageInfo;
        private Panel panelAnswer;
        private Panel panelOrder;
        private Panel panelPicBt;
        private Panel panelTop;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PromptLargestTradeNumCallBack PromptLargestTradeNum;
        private int startKeyNum = 1;
        private TextBox textBoxCanNum;
        private TextBox textBoxInfo;
        private UpdateNumericPriceCallBack UpdatePrice;

        public T8Order()
        {
            this.InitializeComponent();
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.operationManager.orderOperation.UpdateNumericPrice = new OrderOperation.UpdateNumericPriceCallBack(this.UpdateNumericPrice);
            this.operationManager.orderOperation.setLargestTN = new OrderOperation.SetLargestTNCallBack(this.SetLargestTNInfo);
            this.operationManager.submitOrderOperation.SetFocus = new SubmitOrderOperation.SetFocusCallBack(this.SetFouce);
            this.operationManager.submitOrderOperation.OrderMessage = new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage);
            this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
            this.operationManager.TransferInfo = new OperationManager.TransferInfoCallBack(this.SetPriceQty);
            Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
            Global.SetCommoditySelectIndex = new Global.SetCommoditySelectIndexCallBack(this.SetCommoditySelectIndex);
            Global.SetDoubleClickOrderInfo = new Global.SetDoubleClickOrderInfoCallBack(this.SetDoubleClickOrderInfo);
            base.CreateControl();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            this.buttonClick = true;
            SubmitOrderInfo orderInfo = new SubmitOrderInfo
            {
                customerID = Global.FirmID + Global.CustomerID,
                commodityID = this.comboCommodity.Text
            };
            orderInfo.B_SType = Tools.StrToShort((this.comboBoxBuyOrSall.SelectedIndex + 1).ToString());
            if (this.comboBoxTransfer.SelectedIndex != 0)
            {
                orderInfo.O_LType = 2;
            }
            orderInfo.price = Tools.StrToDouble(this.numericUpDownPrice.Value.ToString(), 0.0);
            orderInfo.qty = Tools.StrToInt(this.numericUpDownNum.Value.ToString(), 0);
            if (orderInfo.O_LType == 2)
            {
                if (IniData.GetInstance().CloseMode == 2)
                {
                    orderInfo.closeMode = 2;
                    if (this.comboBoxTransfer.SelectedIndex == 2)
                    {
                        orderInfo.timeFlag = 1;
                    }
                    else
                    {
                        orderInfo.closeMode = 1;
                    }
                }
                else if (IniData.GetInstance().CloseMode == 3)
                {
                    orderInfo.closeMode = 3;
                    orderInfo.lPrice = Tools.StrToDouble(this.numericLPrice.Value.ToString(), 0.0);
                }
                else
                {
                    orderInfo.closeMode = 1;
                }
            }
            OperationManager.GetInstance().orderOperation.orderType = OrderType.Order;
            this.operationManager.submitOrderOperation.ButtonOrderComm(orderInfo, this.BtnFlag);
        }

        private void ChangeComboSelectIndex(Keys keyData, MyCombobox combo)
        {
            int selectedIndex = combo.SelectedIndex;
            if (keyData == Keys.Up)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
            }
            else if (keyData == Keys.Down)
            {
                selectedIndex++;
                if (selectedIndex > (combo.Items.Count - 1))
                {
                    selectedIndex = combo.Items.Count - 1;
                }
            }
            combo.SelectedIndex = selectedIndex;
        }

        private void comboBoxBuyOrSall_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.operationManager.t8OrderOperation.ComboDrawItem(this.comboBoxBuyOrSall, e);
        }

        private void comboBoxBuyOrSall_Enter(object sender, EventArgs e)
        {
            this.operationManager.t8OrderOperation.KeyTip(1, this.startKeyNum);
        }

        private void comboBoxBuyOrSall_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isFirstLoad)
            {
                this.operationManager.orderOperation.IsChangePrice = false;
                int selectedIndex = this.comboBoxBuyOrSall.SelectedIndex;
                decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(selectedIndex);
                this.numericUpDownPrice.Value = bSPrice;
                this.numericUpDownNum_Enter(null, null);
            }
        }

        private void comboBoxBuyOrSall_TextChanged(object sender, EventArgs e)
        {
            this.operationManager.t8OrderOperation.ChangeComboForColor(this.comboBoxBuyOrSall);
        }

        private void comboBoxTransfer_Enter(object sender, EventArgs e)
        {
            this.operationManager.t8OrderOperation.KeyTip(2, this.startKeyNum);
        }

        private void comboBoxTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }
            string str = Global.M_ResourceManager.GetString("HQStr_Dingli");
            if ((this.comboBoxTransfer.SelectedItem != null) && this.comboBoxTransfer.SelectedItem.ToString().Contains(str))
            {
                string str2 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanAmount");
                this.labelCanTransfer.Text = str2;
                this.comboBoxTransfer.SelectedIndex = 0;
                this.labelLPrice.Visible = false;
                this.numericLPrice.Visible = false;
            }
            else
            {
                string str3 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanGrantAmount");
                this.labelCanTransfer.Text = str3;
                if (this.comboBoxTransfer.SelectedIndex == 1)
                {
                    IniData.GetInstance().CloseMode = 1;
                }
                else if (this.comboBoxTransfer.SelectedIndex == 2)
                {
                    IniData.GetInstance().CloseMode = 2;
                }
                else
                {
                    IniData.GetInstance().CloseMode = 3;
                }
                switch (IniData.GetInstance().CloseMode)
                {
                    case 1:
                        this.labelLPrice.Visible = false;
                        this.numericLPrice.Visible = false;
                        goto Label_0169;

                    case 2:
                        this.labelLPrice.Visible = false;
                        this.numericLPrice.Visible = false;
                        goto Label_0169;

                    case 3:
                        this.labelLPrice.Visible = true;
                        this.numericLPrice.Visible = true;
                        goto Label_0169;
                }
                this.labelLPrice.Visible = false;
                this.numericLPrice.Visible = false;
            }
            Label_0169:
            this.numericUpDownNum_Enter(null, null);
        }

        private void comboCommodity_DropDown(object sender, EventArgs e)
        {
            this.operationManager.orderOperation.SetListBoxVisible(false);
        }

        private void comboCommodity_KeyDown(object sender, KeyEventArgs e)
        {
            this.operationManager.orderOperation.ComboxKeyDown(e);
        }

        private void comboCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IniData.GetInstance().AutoPrice)
            {
                this.numericUpDownPrice.Value = 0M;
            }
            this.numericUpDownNum.Value = 0M;
            this.numericLPrice.Value = 0M;
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
            this.numericUpDownPrice.Increment = commoditySpread;
            this.numericLPrice.Increment = commoditySpread;
            int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
            this.numericUpDownPrice.DecimalPlaces = decimalPlaces;
            this.numericLPrice.DecimalPlaces = decimalPlaces;
        }

        private void ComboLoad()
        {
            this.comboBoxBuyOrSall.Items.Clear();
            this.comboBoxBuyOrSall.Items.Add(this.startKeyNum + "." + this.operationManager.StrBuy);
            this.comboBoxBuyOrSall.Items.Add((this.startKeyNum + 1) + "." + this.operationManager.StrSale);
            this.comboBoxBuyOrSall.SelectedIndex = 0;
        }

        private void ComboTrans()
        {
            this.comboBoxTransfer.Items.Clear();
            string str = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1");
            string str2 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2");
            this.comboBoxTransfer.Items.Add(this.startKeyNum + "." + str);
            this.comboBoxTransfer.Items.Add((this.startKeyNum + 1) + "." + str2);
            string str3 = Global.M_ResourceManager.GetString("TradeStr_TransferToday");
            this.comboBoxTransfer.Items.Add((this.startKeyNum + 2) + "." + str3);
            string str4 = Global.M_ResourceManager.GetString("Global_CloseModeStrArr2");
            this.comboBoxTransfer.Items.Add((this.startKeyNum + 3) + "." + str4);
            this.comboBoxTransfer.SelectedIndex = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.panelPicBt = new Panel();
            this.numericLPrice = new NumericUpDown();
            this.labelLPrice = new Label();
            this.pictureBox3 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.pictureBox1 = new PictureBox();
            this.panelOrder = new Panel();
            this.textBoxCanNum = new TextBox();
            this.labelCanTransfer = new Label();
            this.comboCommodity = new MyCombobox();
            this.numericUpDownNum = new NumericUpDown();
            this.numericUpDownPrice = new NumericUpDown();
            this.label_OG = new Label();
            this.label_BS = new Label();
            this.comboBoxTransfer = new MyCombobox();
            this.comboBoxBuyOrSall = new MyCombobox();
            this.buttonSubmit = new MyButton();
            this.label_Qty = new Label();
            this.label_Price = new Label();
            this.labelPingZhong = new Label();
            this.panelAnswer = new Panel();
            this.labelAnswer = new Label();
            this.textBoxInfo = new TextBox();
            this.panelTop.SuspendLayout();
            this.panelPicBt.SuspendLayout();
            this.numericLPrice.BeginInit();
            ((ISupportInitialize)this.pictureBox3).BeginInit();
            ((ISupportInitialize)this.pictureBox2).BeginInit();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.panelOrder.SuspendLayout();
            this.numericUpDownNum.BeginInit();
            this.numericUpDownPrice.BeginInit();
            this.panelAnswer.SuspendLayout();
            base.SuspendLayout();
            this.panelTop.BorderStyle = BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.panelPicBt);
            this.panelTop.Controls.Add(this.panelOrder);
            this.panelTop.Controls.Add(this.panelAnswer);
            this.panelTop.Dock = DockStyle.Fill;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Margin = new Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(950, 0x54);
            this.panelTop.TabIndex = 0x10;
            this.panelPicBt.BorderStyle = BorderStyle.Fixed3D;
            this.panelPicBt.Controls.Add(this.numericLPrice);
            this.panelPicBt.Controls.Add(this.labelLPrice);
            this.panelPicBt.Controls.Add(this.pictureBox3);
            this.panelPicBt.Controls.Add(this.pictureBox2);
            this.panelPicBt.Controls.Add(this.pictureBox1);
            this.panelPicBt.Dock = DockStyle.Fill;
            this.panelPicBt.ForeColor = SystemColors.ControlText;
            this.panelPicBt.Location = new Point(0, 0x38);
            this.panelPicBt.Name = "panelPicBt";
            this.panelPicBt.Size = new Size(0x3b4, 0x1a);
            this.panelPicBt.TabIndex = 4;
            this.numericLPrice.Font = new Font("宋体", 9f);
            this.numericLPrice.Location = new Point(0x1d0, 1);
            int[] bits = new int[4];
            bits[0] = 0xf423f;
            this.numericLPrice.Maximum = new decimal(bits);
            this.numericLPrice.Name = "numericLPrice";
            this.numericLPrice.Size = new Size(0x51, 0x15);
            this.numericLPrice.TabIndex = 0x26;
            this.numericLPrice.Visible = false;
            this.labelLPrice.AutoSize = true;
            this.labelLPrice.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.labelLPrice.ImeMode = ImeMode.NoControl;
            this.labelLPrice.Location = new Point(0x17f, 4);
            this.labelLPrice.Name = "labelLPrice";
            this.labelLPrice.Size = new Size(0x4d, 14);
            this.labelLPrice.TabIndex = 0x27;
            this.labelLPrice.Text = "指定价格：";
            this.labelLPrice.TextAlign = ContentAlignment.BottomLeft;
            this.labelLPrice.Visible = false;
            this.pictureBox3.ImeMode = ImeMode.NoControl;
            this.pictureBox3.Location = new Point(0xc7, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Size(100, 0x16);
            this.pictureBox3.TabIndex = 0x25;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Tag = "市价委托";
            this.pictureBox3.Click += new EventHandler(this.pictureBox1_Click);
            this.pictureBox3.Paint += new PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox2.ImeMode = ImeMode.NoControl;
            this.pictureBox2.Location = new Point(0x65, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(100, 0x16);
            this.pictureBox2.TabIndex = 0x23;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "预备指令";
            this.pictureBox2.Click += new EventHandler(this.pictureBox1_Click);
            this.pictureBox2.Paint += new PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox1.ImeMode = ImeMode.NoControl;
            this.pictureBox1.Location = new Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(100, 0x16);
            this.pictureBox1.TabIndex = 0x22;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "正常委托";
            this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new PaintEventHandler(this.pictureBox_Paint);
            this.panelOrder.BorderStyle = BorderStyle.Fixed3D;
            this.panelOrder.Controls.Add(this.textBoxCanNum);
            this.panelOrder.Controls.Add(this.labelCanTransfer);
            this.panelOrder.Controls.Add(this.comboCommodity);
            this.panelOrder.Controls.Add(this.numericUpDownNum);
            this.panelOrder.Controls.Add(this.numericUpDownPrice);
            this.panelOrder.Controls.Add(this.label_OG);
            this.panelOrder.Controls.Add(this.label_BS);
            this.panelOrder.Controls.Add(this.comboBoxTransfer);
            this.panelOrder.Controls.Add(this.comboBoxBuyOrSall);
            this.panelOrder.Controls.Add(this.buttonSubmit);
            this.panelOrder.Controls.Add(this.label_Qty);
            this.panelOrder.Controls.Add(this.label_Price);
            this.panelOrder.Controls.Add(this.labelPingZhong);
            this.panelOrder.Dock = DockStyle.Top;
            this.panelOrder.Location = new Point(0, 0x1a);
            this.panelOrder.Margin = new Padding(0);
            this.panelOrder.Name = "panelOrder";
            this.panelOrder.Size = new Size(0x3b4, 30);
            this.panelOrder.TabIndex = 0;
            this.textBoxCanNum.Enabled = false;
            this.textBoxCanNum.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxCanNum.ForeColor = SystemColors.ControlText;
            this.textBoxCanNum.Location = new Point(0x26d, 2);
            this.textBoxCanNum.Name = "textBoxCanNum";
            this.textBoxCanNum.ReadOnly = true;
            this.textBoxCanNum.Size = new Size(90, 0x17);
            this.textBoxCanNum.TabIndex = 0x18;
            this.textBoxCanNum.Text = "0";
            this.labelCanTransfer.AutoSize = true;
            this.labelCanTransfer.BackColor = Color.Transparent;
            this.labelCanTransfer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.labelCanTransfer.ImeMode = ImeMode.NoControl;
            this.labelCanTransfer.Location = new Point(550, 6);
            this.labelCanTransfer.Name = "labelCanTransfer";
            this.labelCanTransfer.Size = new Size(0x3f, 14);
            this.labelCanTransfer.TabIndex = 0x17;
            this.labelCanTransfer.Text = "可转让量";
            this.labelCanTransfer.TextAlign = ContentAlignment.MiddleLeft;
            this.comboCommodity.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboCommodity.ItemHeight = 14;
            this.comboCommodity.Location = new Point(0x29, 2);
            this.comboCommodity.MaxLength = 6;
            this.comboCommodity.Name = "comboCommodity";
            this.comboCommodity.Size = new Size(90, 0x16);
            this.comboCommodity.TabIndex = 1;
            this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
            this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
            this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
            this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
            this.numericUpDownNum.BackColor = SystemColors.Window;
            this.numericUpDownNum.Font = new Font("宋体", 10.5f);
            this.numericUpDownNum.Location = new Point(0x2f4, 2);
            this.numericUpDownNum.Margin = new Padding(0);
            int[] numArray2 = new int[4];
            numArray2[0] = 0xf423f;
            this.numericUpDownNum.Maximum = new decimal(numArray2);
            this.numericUpDownNum.Name = "numericUpDownNum";
            this.numericUpDownNum.Size = new Size(90, 0x17);
            this.numericUpDownNum.TabIndex = 14;
            this.numericUpDownNum.Enter += new EventHandler(this.numericUpDownNum_Enter);
            this.numericUpDownNum.KeyUp += new KeyEventHandler(this.numericUpDownNum_KeyUp);
            this.numericUpDownPrice.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            int[] numArray3 = new int[4];
            numArray3[0] = 10;
            this.numericUpDownPrice.Increment = new decimal(numArray3);
            this.numericUpDownPrice.Location = new Point(0x1cf, 2);
            int[] numArray4 = new int[4];
            numArray4[0] = 0xf423f;
            this.numericUpDownPrice.Maximum = new decimal(numArray4);
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new Size(0x51, 0x17);
            this.numericUpDownPrice.TabIndex = 13;
            this.numericUpDownPrice.ValueChanged += new EventHandler(this.numericUpDownPrice_ValueChanged);
            this.numericUpDownPrice.Enter += new EventHandler(this.numericUpDownPrice_Enter);
            this.numericUpDownPrice.KeyUp += new KeyEventHandler(this.numericUpDownPrice_KeyUp);
            this.label_OG.AutoSize = true;
            this.label_OG.BackColor = Color.Transparent;
            this.label_OG.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label_OG.ImageAlign = ContentAlignment.MiddleLeft;
            this.label_OG.ImeMode = ImeMode.NoControl;
            this.label_OG.Location = new Point(0x108, 6);
            this.label_OG.Name = "label_OG";
            this.label_OG.Size = new Size(0x23, 14);
            this.label_OG.TabIndex = 20;
            this.label_OG.Text = "订转";
            this.label_OG.TextAlign = ContentAlignment.MiddleLeft;
            this.label_BS.AutoSize = true;
            this.label_BS.BackColor = Color.Transparent;
            this.label_BS.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label_BS.ImageAlign = ContentAlignment.MiddleLeft;
            this.label_BS.ImeMode = ImeMode.NoControl;
            this.label_BS.Location = new Point(0x88, 6);
            this.label_BS.Margin = new Padding(0);
            this.label_BS.Name = "label_BS";
            this.label_BS.Size = new Size(0x23, 14);
            this.label_BS.TabIndex = 0x13;
            this.label_BS.Text = "买卖";
            this.label_BS.TextAlign = ContentAlignment.MiddleLeft;
            this.comboBoxTransfer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboBoxTransfer.FormattingEnabled = true;
            this.comboBoxTransfer.Items.AddRange(new object[] { "1.订立", "2.转让", "3.转今", "4.按价格转让" });
            this.comboBoxTransfer.Location = new Point(0x131, 2);
            this.comboBoxTransfer.Margin = new Padding(0);
            this.comboBoxTransfer.Name = "comboBoxTransfer";
            this.comboBoxTransfer.Size = new Size(110, 0x16);
            this.comboBoxTransfer.TabIndex = 12;
            this.comboBoxTransfer.SelectedIndexChanged += new EventHandler(this.comboBoxTransfer_SelectedIndexChanged);
            this.comboBoxTransfer.Enter += new EventHandler(this.comboBoxTransfer_Enter);
            this.comboBoxBuyOrSall.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboBoxBuyOrSall.FormattingEnabled = true;
            this.comboBoxBuyOrSall.Items.AddRange(new object[] { "1.卖出", "2.买入" });
            this.comboBoxBuyOrSall.Location = new Point(0xb3, 2);
            this.comboBoxBuyOrSall.Name = "comboBoxBuyOrSall";
            this.comboBoxBuyOrSall.Size = new Size(0x51, 0x16);
            this.comboBoxBuyOrSall.TabIndex = 11;
            this.comboBoxBuyOrSall.DrawItem += new DrawItemEventHandler(this.comboBoxBuyOrSall_DrawItem);
            this.comboBoxBuyOrSall.SelectedIndexChanged += new EventHandler(this.comboBoxBuyOrSall_SelectedIndexChanged);
            this.comboBoxBuyOrSall.TextChanged += new EventHandler(this.comboBoxBuyOrSall_TextChanged);
            this.comboBoxBuyOrSall.Enter += new EventHandler(this.comboBoxBuyOrSall_Enter);
            this.buttonSubmit.BackColor = Color.LightSteelBlue;
            this.buttonSubmit.FlatStyle = FlatStyle.Popup;
            this.buttonSubmit.Font = new Font("宋体", 10.5f);
            this.buttonSubmit.ImeMode = ImeMode.NoControl;
            this.buttonSubmit.Location = new Point(0x353, 1);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new Size(80, 0x18);
            this.buttonSubmit.TabIndex = 20;
            this.buttonSubmit.Text = "正常委托";
            this.buttonSubmit.UseVisualStyleBackColor = false;
            this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
            this.label_Qty.AutoSize = true;
            this.label_Qty.BackColor = Color.Transparent;
            this.label_Qty.Font = new Font("宋体", 10.5f);
            this.label_Qty.ImeMode = ImeMode.NoControl;
            this.label_Qty.Location = new Point(0x2cc, 6);
            this.label_Qty.Name = "label_Qty";
            this.label_Qty.Size = new Size(0x23, 14);
            this.label_Qty.TabIndex = 0x11;
            this.label_Qty.Text = "数量";
            this.label_Qty.TextAlign = ContentAlignment.MiddleLeft;
            this.label_Price.AutoSize = true;
            this.label_Price.BackColor = Color.Transparent;
            this.label_Price.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label_Price.ImeMode = ImeMode.NoControl;
            this.label_Price.Location = new Point(420, 6);
            this.label_Price.Name = "label_Price";
            this.label_Price.Size = new Size(0x23, 14);
            this.label_Price.TabIndex = 0x10;
            this.label_Price.Text = "价格";
            this.label_Price.TextAlign = ContentAlignment.MiddleLeft;
            this.labelPingZhong.AutoSize = true;
            this.labelPingZhong.BackColor = Color.Transparent;
            this.labelPingZhong.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.labelPingZhong.ImageAlign = ContentAlignment.MiddleLeft;
            this.labelPingZhong.ImeMode = ImeMode.NoControl;
            this.labelPingZhong.Location = new Point(3, 6);
            this.labelPingZhong.Name = "labelPingZhong";
            this.labelPingZhong.Size = new Size(0x23, 14);
            this.labelPingZhong.TabIndex = 12;
            this.labelPingZhong.Text = "品种";
            this.labelPingZhong.TextAlign = ContentAlignment.MiddleLeft;
            this.panelAnswer.BorderStyle = BorderStyle.Fixed3D;
            this.panelAnswer.Controls.Add(this.labelAnswer);
            this.panelAnswer.Controls.Add(this.textBoxInfo);
            this.panelAnswer.Dock = DockStyle.Top;
            this.panelAnswer.Location = new Point(0, 0);
            this.panelAnswer.Name = "panelAnswer";
            this.panelAnswer.Size = new Size(0x3b4, 0x1a);
            this.panelAnswer.TabIndex = 5;
            this.labelAnswer.AutoSize = true;
            this.labelAnswer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.labelAnswer.ImeMode = ImeMode.NoControl;
            this.labelAnswer.Location = new Point(3, 5);
            this.labelAnswer.Name = "labelAnswer";
            this.labelAnswer.Size = new Size(0x23, 14);
            this.labelAnswer.TabIndex = 1;
            this.labelAnswer.Text = "应答";
            this.textBoxInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxInfo.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxInfo.ForeColor = Color.Red;
            this.textBoxInfo.Location = new Point(40, 1);
            this.textBoxInfo.Margin = new Padding(0);
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new Size(0x38c, 0x17);
            this.textBoxInfo.TabIndex = 2;
            this.textBoxInfo.TabStop = false;
            this.textBoxInfo.Text = "10:57:02 当前系统正常!";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.panelTop);
            base.Margin = new Padding(0);
            base.Name = "T8Order";
            base.Size = new Size(950, 0x54);
            base.Load += new EventHandler(this.T8Order_Load);
            this.panelTop.ResumeLayout(false);
            this.panelPicBt.ResumeLayout(false);
            this.panelPicBt.PerformLayout();
            this.numericLPrice.EndInit();
            ((ISupportInitialize)this.pictureBox3).EndInit();
            ((ISupportInitialize)this.pictureBox2).EndInit();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.panelOrder.ResumeLayout(false);
            this.panelOrder.PerformLayout();
            this.numericUpDownNum.EndInit();
            this.numericUpDownPrice.EndInit();
            this.panelAnswer.ResumeLayout(false);
            this.panelAnswer.PerformLayout();
            base.ResumeLayout(false);
        }

        private void keyNum(int num)
        {
            if (Tools.StrToBool((string)Global.HTConfig["UseZeroKey"], false))
            {
                num++;
            }
            switch (num)
            {
                case 1:
                    if (this.comboBoxBuyOrSall.Focused)
                    {
                        this.comboBoxBuyOrSall.SelectedIndex = 0;
                    }
                    if (!this.comboBoxTransfer.Focused)
                    {
                        break;
                    }
                    this.comboBoxTransfer.SelectedIndex = 0;
                    return;

                case 2:
                    if (this.comboBoxBuyOrSall.Focused)
                    {
                        this.comboBoxBuyOrSall.SelectedIndex = 1;
                    }
                    if (!this.comboBoxTransfer.Focused)
                    {
                        break;
                    }
                    this.comboBoxTransfer.SelectedIndex = 1;
                    return;

                case 3:
                    if (!this.comboBoxTransfer.Focused || (this.comboBoxTransfer.Items.Count <= 2))
                    {
                        break;
                    }
                    this.comboBoxTransfer.SelectedIndex = 2;
                    return;

                case 4:
                    if (this.comboBoxTransfer.Focused && (this.comboBoxTransfer.Items.Count > 2))
                    {
                        this.comboBoxTransfer.SelectedIndex = 3;
                    }
                    break;

                default:
                    return;
            }
        }

        private void LargestTNInfo(string text, int colorFlag)
        {
            if (colorFlag == 0)
            {
                this.textBoxInfo.ForeColor = Global.LightColor;
            }
            else if (colorFlag == 1)
            {
                this.textBoxInfo.ForeColor = Global.ErrorColor;
            }
            this.textBoxInfo.Text = Global.ServerTime.ToLongTimeString() + "  " + text;
            int index = text.IndexOf("：");
            if (index > 0)
            {
                this.textBoxCanNum.Text = text.Substring(index + 1);
            }
            else
            {
                this.textBoxCanNum.Text = "0";
            }
        }

        private void numericLPrice_Enter(object sender, EventArgs e)
        {
            this.numericLPrice.Select(0, this.numericLPrice.Text.Length);
        }

        private void numericLPrice_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
        }

        private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
        {
            this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
        }

        private void numericUpDownNum_Enter(object sender, EventArgs e)
        {
            this.numericUpDownNum.Select(0, this.numericUpDownNum.Value.ToString().Length);
            short num = 2;
            if (this.comboBoxBuyOrSall.SelectedIndex == 0)
            {
                num = 1;
            }
            short num2 = 1;
            if (this.comboBoxTransfer.SelectedIndex != 0)
            {
                num2 = 2;
            }
            Hashtable o = new Hashtable();
            o.Add("Commodity", this.comboCommodity.Text);
            o.Add("B_SType", num);
            o.Add("O_LType", num2);
            o.Add("numericPrice", Convert.ToDouble(this.numericUpDownPrice.Value));
            o.Add("tbTranc_comboTranc", Global.FirmID + Global.CustomerID);
            this.operationManager.orderOperation.GetNumericQtyThread(o);
        }

        private void numericUpDownNum_KeyUp(object sender, KeyEventArgs e)
        {
            Global.QtyKeyUp(sender, e);
        }

        private void numericUpDownPrice_Enter(object sender, EventArgs e)
        {
            if (this.numericUpDownPrice.Value == 0M)
            {
                this.numericUpDownPrice.Select(0, this.numericUpDownPrice.Text.Length);
            }
            this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
        }

        private void numericUpDownPrice_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
        }

        private void numericUpDownPrice_ValueChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.comboBoxBuyOrSall.SelectedIndex;
            decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(selectedIndex);
            if (this.numericUpDownPrice.Value != bSPrice)
            {
                this.operationManager.orderOperation.IsChangePrice = true;
            }
            else
            {
                this.operationManager.orderOperation.IsChangePrice = false;
            }
        }

        private void OrderInfoMessage(long retCode, string retMessage)
        {
            this.comboCommodity.Focus();
            if (IniData.GetInstance().ClearData)
            {
                if (this.numericUpDownPrice.Enabled)
                {
                    this.numericUpDownPrice.Value = 0M;
                }
                this.numericUpDownNum.Text = "";
                this.numericLPrice.Value = 0M;
            }
            if (retCode == 0L)
            {
                this.operationManager.orderOperation.IsChangePrice = false;
                if (Global.StatusInfoFill != null)
                {
                    Global.StatusInfoFill(this.operationManager.SussceOrder, Global.RightColor, true);
                }
            }
            else if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
            {
                MessageBox.Show(retMessage, this.operationManager.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.operationManager.orderOperation.setLargestTN != null)
            {
                this.operationManager.orderOperation.setLargestTN(retMessage, 1);
            }
        }

        private void OrderMessage(long retCode, string retMessage)
        {
            try
            {
                if (OperationManager.GetInstance().orderOperation.orderType == OrderType.Order)
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

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox label = sender as PictureBox;
            this.operationManager.t8OrderOperation.ChangeBorder(e, label, this.clickNum);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            this.clickNum = this.operationManager.t8OrderOperation.PictureBoxClick(pb, this.panelPicBt, this.clickNum);
            if (this.clickNum == 2)
            {
                this.BtnFlag = 1;
            }
            else
            {
                this.BtnFlag = 0;
            }
            this.buttonSubmit.Text = pb.Tag.ToString();
            if (this.clickNum == 4)
            {
                this.numericUpDownPrice.Enabled = false;
            }
            else
            {
                this.numericUpDownPrice.Enabled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.buttonClick)
            {
                this.buttonClick = false;
                return false;
            }
            if (!IniData.GetInstance().UpDownFocus)
            {
                return false;
            }
            if (((keyData == Keys.Up) || (keyData == Keys.Down)) || ((keyData == Keys.Left) || (keyData == Keys.Right)))
            {
                this.SetFouces(keyData);
                return true;
            }
            if (!this.comboBoxBuyOrSall.Focused && !this.comboBoxTransfer.Focused)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            if ((keyData == Keys.D0) || (keyData == Keys.NumPad0))
            {
                this.keyNum(0);
            }
            else if ((keyData == Keys.D1) || (keyData == Keys.NumPad1))
            {
                this.keyNum(1);
            }
            else if ((keyData == Keys.D2) || (keyData == Keys.NumPad2))
            {
                this.keyNum(2);
            }
            else if ((keyData == Keys.D3) || (keyData == Keys.NumPad3))
            {
                this.keyNum(3);
            }
            else if ((keyData == Keys.D4) || (keyData == Keys.NumPad4))
            {
                this.keyNum(4);
            }
            else if ((keyData == Keys.D5) || (keyData == Keys.NumPad5))
            {
                this.keyNum(5);
            }
            return true;
        }

        private void SetButtonOrderEnable(bool enable)
        {
            this.buttonSubmit.Enabled = enable;
        }

        private void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboCommodity.Items.Clear();
            foreach (string str in commodityIDList)
            {
                if (str != this.operationManager.StrAll)
                {
                    this.comboCommodity.Items.Add(str);
                }
            }
            this.comboCommodity.SelectedIndex = 0;
            this.comboCommodity.Focus();
        }

        private bool SetCommoditySelectIndex(string marketID, string commodityID)
        {
            for (int i = 0; i < this.comboCommodity.Items.Count; i++)
            {
                if (this.comboCommodity.Items[i].ToString() == commodityID)
                {
                    this.comboCommodity.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void SetControlText()
        {
            this.numericUpDownNum.Text = "";
            this.comboBoxBuyOrSall.DrawMode = DrawMode.OwnerDrawVariable;
            this.labelAnswer.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Answer");
            this.labelPingZhong.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Variety");
            this.label_BS.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_BS");
            this.label_OG.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_OG");
            this.label_Price.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Price");
            this.labelCanTransfer.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanGrantAmount");
            this.label_Qty.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Qty");
            this.buttonSubmit.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-Normal");
            this.labelLPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabelLPrice");
            this.pictureBox1.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-Normal");
            this.pictureBox2.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_embed");
            this.pictureBox3.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-market");
        }

        private void SetDoubleClickOrderInfo(double price, double Lprice, int qty, short buysell, short ordertype)
        {
            try
            {
                if (buysell == 0)
                {
                    this.comboBoxBuyOrSall.SelectedIndex = 0;
                }
                else
                {
                    this.comboBoxBuyOrSall.SelectedIndex = 1;
                }
                if (ordertype == 0)
                {
                    this.comboBoxTransfer.SelectedIndex = 0;
                }
                else
                {
                    this.comboBoxTransfer.SelectedIndex = 1;
                }
                if (this.numericLPrice.Visible)
                {
                    this.numericLPrice.Value = decimal.Parse(Lprice.ToString());
                }
                if (price != 0.0)
                {
                    this.numericUpDownPrice.Value = decimal.Parse(price.ToString());
                }
                this.numericUpDownNum.Value = qty;
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "SetDoubleClickOrderInfo异常：" + exception.Message);
            }
        }

        private void SetFouce(short flag)
        {
            if (flag == 0)
            {
                this.comboCommodity.Focus();
            }
            else if (flag == 1)
            {
                this.numericUpDownPrice.Focus();
            }
            else if (flag == 2)
            {
                this.numericUpDownNum.Focus();
            }
        }

        private void SetFouces(Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                if (this.comboCommodity.Focused)
                {
                    this.buttonSubmit.Focus();
                }
                else if (this.comboBoxBuyOrSall.Focused)
                {
                    if (IniData.GetInstance().LimitFocus && ((this.clickNum == 1) || (this.clickNum == 4)))
                    {
                        this.buttonSubmit.Focus();
                    }
                    else
                    {
                        this.comboCommodity.Focus();
                    }
                }
                else if (this.comboBoxTransfer.Focused)
                {
                    this.comboBoxBuyOrSall.Focus();
                }
                else if (this.numericUpDownPrice.Focused)
                {
                    this.comboBoxTransfer.Focus();
                }
                else if (this.numericUpDownNum.Focused)
                {
                    if (this.numericUpDownPrice.Enabled)
                    {
                        this.numericUpDownPrice.Focus();
                    }
                    else
                    {
                        this.comboBoxTransfer.Focus();
                    }
                }
                else if (this.buttonSubmit.Focused)
                {
                    if (this.numericLPrice.Visible)
                    {
                        this.numericLPrice.Focus();
                    }
                    else
                    {
                        this.numericUpDownNum.Focus();
                    }
                }
                else if (this.numericLPrice.Focused)
                {
                    this.numericUpDownNum.Focus();
                }
            }
            else if (keyData == Keys.Right)
            {
                if (this.comboCommodity.Focused)
                {
                    this.comboBoxBuyOrSall.Focus();
                }
                else if (this.comboBoxBuyOrSall.Focused)
                {
                    this.comboBoxTransfer.Focus();
                }
                else if (this.comboBoxTransfer.Focused)
                {
                    if (this.numericUpDownPrice.Enabled)
                    {
                        this.numericUpDownPrice.Focus();
                    }
                    else
                    {
                        this.numericUpDownNum.Focus();
                    }
                }
                else if (this.numericUpDownPrice.Focused)
                {
                    this.numericUpDownNum.Focus();
                }
                else if (this.numericUpDownNum.Focused)
                {
                    if (this.numericLPrice.Visible)
                    {
                        this.numericLPrice.Focus();
                    }
                    else
                    {
                        this.buttonSubmit.Focus();
                    }
                }
                else if (this.numericLPrice.Focused)
                {
                    this.buttonSubmit.Focus();
                }
                else if (this.buttonSubmit.Focused)
                {
                    if (IniData.GetInstance().LimitFocus && ((this.clickNum == 1) || (this.clickNum == 4)))
                    {
                        this.comboBoxBuyOrSall.Focus();
                    }
                    else
                    {
                        this.comboCommodity.Focus();
                    }
                }
            }
            else if (this.comboCommodity.Focused)
            {
                this.ChangeComboSelectIndex(keyData, this.comboCommodity);
            }
            else if (this.comboBoxBuyOrSall.Focused)
            {
                this.ChangeComboSelectIndex(keyData, this.comboBoxBuyOrSall);
            }
            else if (this.comboBoxTransfer.Focused)
            {
                this.ChangeComboSelectIndex(keyData, this.comboBoxTransfer);
            }
        }

        private void SetLargestTNInfo(string text, int colorFlag)
        {
            try
            {
                this.PromptLargestTradeNum = new PromptLargestTradeNumCallBack(this.LargestTNInfo);
                this.HandleCreated();
                base.Invoke(this.PromptLargestTradeNum, new object[] { text, colorFlag });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void SetNumericPrice(double bPrice, double sPrice)
        {
            if (this.comboBoxBuyOrSall.SelectedIndex == 0)
            {
                this.numericUpDownPrice.Value = (decimal)sPrice;
            }
            else if (this.comboBoxBuyOrSall.SelectedIndex == 1)
            {
                this.numericUpDownPrice.Value = (decimal)bPrice;
            }
            this.numericUpDownNum_Enter(null, null);
        }

        private void SetOrderInfo(string commodityID, double buyPrice, double sellPrice)
        {
            string str = string.Empty;
            int index = commodityID.IndexOf("_");
            if (index != -1)
            {
                commodityID.Substring(0, index);
                str = commodityID.Substring(index + 1);
            }
            else
            {
                str = commodityID;
            }
            if (str != this.comboCommodity.Text)
            {
                this.operationManager.orderOperation.ConnectHQ = true;
            }
            if (Global.MarketHT.Count == 1)
            {
                for (int i = 0; i < this.comboCommodity.Items.Count; i++)
                {
                    if (str.Equals(this.comboCommodity.Items[i].ToString()))
                    {
                        this.comboCommodity.SelectedIndex = i;
                        if (this.comboBoxBuyOrSall.SelectedIndex == 0)
                        {
                            this.numericUpDownPrice.Value = (decimal)sellPrice;
                            return;
                        }
                        this.numericUpDownPrice.Value = (decimal)buyPrice;
                        return;
                    }
                }
            }
            else
            {
                for (int j = 0; j < this.comboCommodity.Items.Count; j++)
                {
                    if (str.Equals(this.comboCommodity.Items[j].ToString()))
                    {
                        this.comboCommodity.SelectedIndex = j;
                        if (this.comboBoxBuyOrSall.SelectedIndex == 0)
                        {
                            this.numericUpDownPrice.Value = (decimal)sellPrice;
                            return;
                        }
                        this.numericUpDownPrice.Value = (decimal)buyPrice;
                        return;
                    }
                }
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
                            this.numericUpDownPrice.Value = decimal.Parse(info);
                        }
                        catch (Exception)
                        {
                            this.numericUpDownPrice.Value = 0M;
                        }
                        return;

                    case 1:
                        try
                        {
                            this.numericUpDownNum.Value = decimal.Parse(info);
                        }
                        catch (Exception)
                        {
                            this.numericUpDownNum.Value = 0M;
                        }
                        return;
                }
            }
        }

        private void SetRadioEnable(int currentTradeMode)
        {
            switch (currentTradeMode)
            {
                case 0:
                    this.comboBoxBuyOrSall.Enabled = true;
                    this.comboBoxTransfer.Enabled = true;
                    return;

                case 1:
                    {
                        this.comboBoxBuyOrSall.Enabled = true;
                        this.comboBoxTransfer.Enabled = false;
                        if (this.comboBoxBuyOrSall.SelectedIndex != 0)
                        {
                            if (this.comboBoxBuyOrSall.SelectedIndex != 1)
                            {
                                break;
                            }
                            this.comboBoxTransfer.SelectedIndex = 0;
                            return;
                        }
                        int closeMode = IniData.GetInstance().CloseMode;
                        if (closeMode != 1)
                        {
                            if (closeMode == 2)
                            {
                                this.comboBoxTransfer.SelectedIndex = 2;
                                return;
                            }
                            if (closeMode != 3)
                            {
                                break;
                            }
                            this.comboBoxTransfer.SelectedIndex = 3;
                            return;
                        }
                        this.comboBoxTransfer.SelectedIndex = 1;
                        return;
                    }
                case 2:
                    this.comboBoxBuyOrSall.Enabled = true;
                    this.comboBoxTransfer.Enabled = false;
                    if (this.comboBoxBuyOrSall.SelectedIndex != 0)
                    {
                        if (this.comboBoxBuyOrSall.SelectedIndex != 1)
                        {
                            break;
                        }
                        int num2 = IniData.GetInstance().CloseMode;
                        switch (num2)
                        {
                            case 1:
                                this.comboBoxTransfer.SelectedIndex = 1;
                                return;

                            case 2:
                                this.comboBoxTransfer.SelectedIndex = 2;
                                return;
                        }
                        if (num2 != 3)
                        {
                            break;
                        }
                        this.comboBoxTransfer.SelectedIndex = 3;
                        return;
                    }
                    this.comboBoxTransfer.SelectedIndex = 0;
                    return;

                case 3:
                    this.comboBoxBuyOrSall.Enabled = false;
                    this.comboBoxBuyOrSall.SelectedIndex = 0;
                    this.comboBoxTransfer.Enabled = false;
                    this.comboBoxTransfer.SelectedIndex = 0;
                    return;

                case 4:
                    this.comboBoxBuyOrSall.Enabled = false;
                    this.comboBoxBuyOrSall.SelectedIndex = 1;
                    this.comboBoxTransfer.Enabled = false;
                    this.comboBoxTransfer.SelectedIndex = 0;
                    break;

                default:
                    return;
            }
        }

        private void T8Order_Load(object sender, EventArgs e)
        {
            if (Tools.StrToBool((string)Global.HTConfig["UseZeroKey"], false))
            {
                this.startKeyNum = 0;
            }
            this.SetControlText();
            this.ComboLoad();
            this.ComboTrans();
            this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
            this.isFirstLoad = false;
        }

        private void UpdateNumericPrice(double bPrice, double sPrice)
        {
            try
            {
                this.UpdatePrice = new UpdateNumericPriceCallBack(this.SetNumericPrice);
                this.HandleCreated();
                base.Invoke(this.UpdatePrice, new object[] { bPrice, sPrice });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);

        private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);

        private delegate void UpdateNumericPriceCallBack(double bPrice, double sPrice);
    }
}
