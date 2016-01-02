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

    public class Order : UserControl
    {
        private byte BtnFlag;
        private MyButton buttonAddPre;
        private bool buttonClick;
        public MyButton buttonOrder;
        private MyCombobox comboCommodity;
        private MyCombobox comboMarKet;
        private MyCombobox comboTranc;
        private IContainer components;
        private GroupBox gbCloseMode;
        private GroupBox groupBoxB_S;
        private GroupBox groupBoxO_L;
        private Panel pBoxOrder;
        private Label labComCode;
        private Label labelLargestTN;
        private Label labelLPrice;
        private Label labelMarKet;
        private Label labPrice;
        private Label labQty;
        private Label lblTitle;
        private Label labTrancCode;
        private NumericUpDown numericLPrice;
        private NumericUpDown numericPrice;
        private NumericUpDown numericQty;
        private string OneTwoInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OneTwoInfo");
        private OperationManager operationManager = OperationManager.GetInstance();
        private OrderMessageInfoCallBack OrderMessageInfo;
        private PromptLargestTradeNumCallBack PromptLargestTradeNum;
        private MyRadioButton radioB;
        private MyRadioButton radioL;
        private MyRadioButton radioO;
        private MyRadioButton radioS;
        private MyRadioButton rbCloseH;
        private MyRadioButton rbCloseT;
        private TextBox tbTranc;
        private UpdateNumericPriceCallBack UpdatePrice;

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

        private void buttonAddPre_Click(object sender, EventArgs e)
        {
            this.BtnFlag = 1;
            this.SubmintOrder();
        }

        private void buttonOrder_Click(object sender, EventArgs e)
        {
            this.BtnFlag = 0;
            this.SubmintOrder();
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
            this.numericPrice.Value = 0M;
            this.numericQty.Value = 0M;
            this.numericLPrice.Value = 0M;
            this.labelLargestTN.Text = "";
            this.operationManager.orderOperation.SetListBoxVisible(false);
            this.operationManager.orderOperation.ShowMinLine(this.comboCommodity.Text);
            int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
            this.SetRadioEnable(currentTradeMode);
        }

        private void comboCommodity_TextChanged(object sender, EventArgs e)
        {
            if (this.comboCommodity.Text.StartsWith("Y"))
            {
                this.radioB.Checked = true;
                this.radioO.Checked = true;
            }
            this.operationManager.orderOperation.IsChangePrice = false;
            this.operationManager.orderOperation.ComboxTextChanged(this.comboCommodity);
            decimal commoditySpread = this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
            this.numericPrice.Increment = commoditySpread;
            this.numericLPrice.Increment = commoditySpread;
            int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
            this.numericPrice.DecimalPlaces = decimalPlaces;
            this.numericLPrice.DecimalPlaces = decimalPlaces;
        }

        private void comboTranc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gbCloseMode_Enter(object sender, EventArgs e)
        {
            if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(string.Format(this.OneTwoInfo, Global.TimeFlagStrArr[0], Global.TimeFlagStrArr[1]), Global.RightColor, true);
            }
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
            this.pBoxOrder = new Panel();
            this.comboTranc = new MyCombobox();
            this.tbTranc = new TextBox();
            this.labelLargestTN = new Label();
            this.comboCommodity = new MyCombobox();
            this.buttonAddPre = new MyButton();
            this.buttonOrder = new MyButton();
            this.numericQty = new NumericUpDown();
            this.numericPrice = new NumericUpDown();
            this.labQty = new Label();
            this.labPrice = new Label();
            this.labComCode = new Label();
            this.groupBoxB_S = new GroupBox();
            this.radioS = new MyRadioButton();
            this.radioB = new MyRadioButton();
            this.gbCloseMode = new GroupBox();
            this.rbCloseH = new MyRadioButton();
            this.rbCloseT = new MyRadioButton();
            this.groupBoxO_L = new GroupBox();
            this.radioL = new MyRadioButton();
            this.radioO = new MyRadioButton();
            this.numericLPrice = new NumericUpDown();
            this.labelLPrice = new Label();
            this.comboMarKet = new MyCombobox();
            this.labTrancCode = new Label();
            this.labelMarKet = new Label();
            this.lblTitle = new Label();
            this.pBoxOrder.SuspendLayout();
            this.numericQty.BeginInit();
            this.numericPrice.BeginInit();
            this.groupBoxB_S.SuspendLayout();
            this.gbCloseMode.SuspendLayout();
            this.groupBoxO_L.SuspendLayout();
            this.numericLPrice.BeginInit();
            this.numericLPrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericPrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericQty.BorderStyle = System.Windows.Forms.BorderStyle.None;
            base.SuspendLayout();
            this.pBoxOrder.BackColor = Color.Transparent;
            this.pBoxOrder.BackgroundImageLayout = ImageLayout.Stretch;
            this.pBoxOrder.Controls.Add(this.comboTranc);
            this.pBoxOrder.Controls.Add(this.tbTranc);
            this.pBoxOrder.Controls.Add(this.labelLargestTN);
            this.pBoxOrder.Controls.Add(this.comboCommodity);
            this.pBoxOrder.Controls.Add(this.buttonAddPre);
            this.pBoxOrder.Controls.Add(this.buttonOrder);
            this.pBoxOrder.Controls.Add(this.numericQty);
            this.pBoxOrder.Controls.Add(this.numericPrice);
            this.pBoxOrder.Controls.Add(this.labQty);
            this.pBoxOrder.Controls.Add(this.labPrice);
            this.pBoxOrder.Controls.Add(this.labComCode);
            this.pBoxOrder.Controls.Add(this.groupBoxB_S);
            this.pBoxOrder.Controls.Add(this.gbCloseMode);
            this.pBoxOrder.Controls.Add(this.groupBoxO_L);
            this.pBoxOrder.Controls.Add(this.numericLPrice);
            this.pBoxOrder.Controls.Add(this.labelLPrice);
            this.pBoxOrder.Controls.Add(this.comboMarKet);
            this.pBoxOrder.Controls.Add(this.labTrancCode);
            this.pBoxOrder.Controls.Add(this.labelMarKet);
            this.pBoxOrder.Controls.Add(this.lblTitle);
            this.pBoxOrder.Dock = DockStyle.Fill;
            this.pBoxOrder.BackColor = Color.FromArgb(235, 235, 235);
            this.pBoxOrder.Location = new Point(0, 0);
            this.pBoxOrder.Margin = new Padding(0);
            this.pBoxOrder.Name = "pBoxOrder";
            this.pBoxOrder.Size = new Size(168, 240);
            this.pBoxOrder.TabIndex = 0;
            this.pBoxOrder.TabStop = false;
            this.pBoxOrder.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxOrder.Text = "委托";
            //this.groupBoxOrder.ForeColor = Color.FromArgb(235, 235, 235);
            this.lblTitle.Location = new Point(this.pBoxOrder.Left, this.pBoxOrder.Top);
            this.lblTitle.AutoSize = false;
            this.lblTitle.Height = 25;
            this.lblTitle.Width = this.pBoxOrder.Width;
            this.lblTitle.Text = "  委托";
            this.lblTitle.ForeColor = Color.Black;
            this.lblTitle.Font = new Font("微软雅黑", 12);
            this.lblTitle.BackColor = Color.FromArgb(239, 227, 199);
            this.comboTranc.Location = new Point(0x73, 12+15);
            this.comboTranc.MaxLength = 2;
            this.comboTranc.Name = "comboTranc";
            this.comboTranc.Size = new Size(0x25, 20);
            this.comboTranc.TabIndex = 0;
            this.comboTranc.TabStop = false;
            this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
            this.tbTranc.BackColor = Color.White;
            this.tbTranc.Location = new Point(0x4a, 12+15);
            this.tbTranc.Multiline = true;
            this.tbTranc.Name = "tbTranc";
            this.tbTranc.ReadOnly = true;
            this.tbTranc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbTranc.Size = new Size(0x2a, 20+15);
            this.tbTranc.TabIndex = 0x22;
            this.tbTranc.TabStop = false;
            this.labelLargestTN.AutoSize = true;
            this.labelLargestTN.ImeMode = ImeMode.NoControl;
            this.labelLargestTN.Location = new Point(7, 0x8a+15);
            this.labelLargestTN.Name = "labelLargestTN";
            this.labelLargestTN.Size = new Size(0x4d, 12);
            this.labelLargestTN.TabIndex = 30;
            this.labelLargestTN.Text = "最大可交易量";
            this.labelLargestTN.Click += new EventHandler(this.labelLargestTN_Click);
            this.comboCommodity.Location = new Point(0x4a, 0x22+15);
            this.comboCommodity.MaxLength = 6;
            this.comboCommodity.Name = "comboCommodity";
            this.comboCommodity.Size = new Size(0x4e, 20);
            this.comboCommodity.TabIndex = 1;
            this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
            this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
            this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
            this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
            //this.buttonAddPre.BackColor = Color.LightSteelBlue;
            this.buttonAddPre.FlatStyle = FlatStyle.Popup;
            this.buttonAddPre.ImeMode = ImeMode.NoControl;
            this.buttonAddPre.Location = new Point(0x56, 0xd1+10);
            this.buttonAddPre.Name = "buttonAddPre";
            this.buttonAddPre.Size = new Size(0x41, 0x15);
            this.buttonAddPre.TabIndex = 10;
            this.buttonAddPre.Text = "预埋委托";
            //this.buttonAddPre.ForeColor = Color.FromArgb(235, 235, 235);
            this.buttonAddPre.UseVisualStyleBackColor = false;
            this.buttonAddPre.Click += new EventHandler(this.buttonAddPre_Click);
            //this.buttonOrder.BackColor = Color.LightSteelBlue;
            this.buttonOrder.FlatStyle = FlatStyle.Popup;
            this.buttonOrder.ImeMode = ImeMode.NoControl;
            this.buttonOrder.Location = new Point(11, 0xd1+10);
            this.buttonOrder.Name = "buttonOrder";
            this.buttonOrder.Size = new Size(0x41, 0x15);
            this.buttonOrder.TabIndex = 9;
            this.buttonOrder.Text = "立即提交";
         
            this.buttonOrder.UseVisualStyleBackColor = false;
            this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
            this.numericQty.Location = new Point(0x4a, 0x9d+15);
            int[] bits = new int[4];
            bits[0] = 0xf423f;
            this.numericQty.Maximum = new decimal(bits);
            this.numericQty.Name = "numericQty";
            this.numericQty.Size = new Size(0x4e, 0x15);
            this.numericQty.TabIndex = 7;
            this.numericQty.Enter += new EventHandler(this.numericQty_Enter);
            this.numericQty.KeyUp += new KeyEventHandler(this.numericQty_KeyUp);
            int[] numArray2 = new int[4];
            numArray2[0] = 10;
            this.numericPrice.Increment = new decimal(numArray2);
            this.numericPrice.Location = new Point(0x4a, 0x6f+15);
            int[] numArray3 = new int[4];
            numArray3[0] = 0xf423f;
            this.numericPrice.Maximum = new decimal(numArray3);
            this.numericPrice.Name = "numericPrice";
            this.numericPrice.Size = new Size(0x4e, 0x15);
            this.numericPrice.TabIndex = 6;
            this.numericPrice.ValueChanged += new EventHandler(this.numericPrice_ValueChanged);
            this.numericPrice.Enter += new EventHandler(this.numericPrice_Enter);
            this.numericPrice.KeyUp += new KeyEventHandler(this.numericPrice_KeyUp);
            this.labQty.AutoSize = true;
            this.labQty.ImeMode = ImeMode.NoControl;
            this.labQty.Location = new Point(8, 0xa1+15);
            this.labQty.Name = "labQty";
            this.labQty.Size = new Size(0x41, 12);
            this.labQty.TabIndex = 9;
            this.labQty.Text = "委托数量：";
            //this.labQty.ForeColor = Color.FromArgb(235, 235, 235);
            this.labQty.TextAlign = ContentAlignment.BottomLeft;
            this.labPrice.AutoSize = true;
            this.labPrice.Font = new Font("宋体", 9f);
            this.labPrice.ForeColor = SystemColors.ControlText;
            this.labPrice.ImeMode = ImeMode.NoControl;
            this.labPrice.Location = new Point(8, 0x72+15);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(0x41, 12);
            this.labPrice.TabIndex = 8;
            this.labPrice.Text = "委托价格：";
            //this.labPrice.ForeColor = Color.FromArgb(235, 235, 235);
            this.labPrice.TextAlign = ContentAlignment.BottomLeft;
            this.labComCode.AutoSize = true;
            this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
            this.labComCode.ImeMode = ImeMode.NoControl;
            this.labComCode.Location = new Point(6, 0x26+15);
            this.labComCode.Margin = new Padding(0);
            this.labComCode.Name = "labComCode";
            this.labComCode.Size = new Size(0x41, 15);
            this.labComCode.TabIndex = 4;
            this.labComCode.Text = "商品代码：";
            //this.labComCode.ForeColor = Color.FromArgb(235, 235, 235);
            this.labComCode.TextAlign = ContentAlignment.BottomLeft;
            this.groupBoxB_S.BackColor = Color.Transparent;
            this.groupBoxB_S.Controls.Add(this.radioS);
            this.groupBoxB_S.Controls.Add(this.radioB);
            this.groupBoxB_S.Location = new Point(15, 0x33+15);
            this.groupBoxB_S.Margin = new Padding(0);
            this.groupBoxB_S.Name = "groupBoxB_S";
            this.groupBoxB_S.Padding = new Padding(0);
            this.groupBoxB_S.Size = new Size(0x7e, 0x20);
            this.groupBoxB_S.TabIndex = 2;
            this.groupBoxB_S.TabStop = false;
            this.radioS.AutoSize = true;
            this.radioS.ForeColor = Color.Green;
            this.radioS.ImeMode = ImeMode.NoControl;
            this.radioS.Location = new Point(0x48, 12);
            this.radioS.Name = "radioS";
            this.radioS.Size = new Size(0x2f, 0x10);
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
            this.radioB.Size = new Size(0x2f, 0x10);
            this.radioB.TabIndex = 2;
            this.radioB.TabStop = true;
            this.radioB.Text = "买入";
            this.radioB.CheckedChanged += new EventHandler(this.radioB_CheckedChanged);
            this.radioB.Enter += new EventHandler(this.radioB_Enter);
            this.gbCloseMode.BackColor = Color.Transparent;
            this.gbCloseMode.Controls.Add(this.rbCloseH);
            this.gbCloseMode.Controls.Add(this.rbCloseT);
            this.gbCloseMode.Location = new Point(7, 0xae+15);
            this.gbCloseMode.Margin = new Padding(0);
            this.gbCloseMode.Name = "gbCloseMode";
            this.gbCloseMode.Padding = new Padding(0);
            this.gbCloseMode.Size = new Size(0x93, 0x20);
            this.gbCloseMode.TabIndex = 8;
            this.gbCloseMode.TabStop = false;
            this.gbCloseMode.Visible = false;
            this.gbCloseMode.Enter += new EventHandler(this.gbCloseMode_Enter);
            this.rbCloseH.AutoSize = true;
            this.rbCloseH.Checked = true;
            this.rbCloseH.ImeMode = ImeMode.NoControl;
            this.rbCloseH.Location = new Point(0x4a, 11+15);
            this.rbCloseH.Name = "rbCloseH";
            this.rbCloseH.Size = new Size(0x47, 0x10);
            this.rbCloseH.TabIndex = 5;
            this.rbCloseH.TabStop = true;
            this.rbCloseH.Text = "转历史订";
            this.rbCloseT.AutoSize = true;
            this.rbCloseT.ImeMode = ImeMode.NoControl;
            this.rbCloseT.Location = new Point(3, 11+15);
            this.rbCloseT.Name = "rbCloseT";
            this.rbCloseT.Size = new Size(0x3b, 0x10);
            this.rbCloseT.TabIndex = 4;
            this.rbCloseT.Text = "转今订";
            this.groupBoxO_L.BackColor = Color.Transparent;
            this.groupBoxO_L.Controls.Add(this.radioL);
            this.groupBoxO_L.Controls.Add(this.radioO);
            this.groupBoxO_L.Location = new Point(15, 0x4d+15);
            this.groupBoxO_L.Name = "groupBoxO_L";
            this.groupBoxO_L.Size = new Size(0x7e, 0x20);
            this.groupBoxO_L.TabIndex = 3;
            this.groupBoxO_L.TabStop = false;
            this.radioL.AutoSize = true;
            this.radioL.ImeMode = ImeMode.NoControl;
            this.radioL.Location = new Point(0x48, 12);
            this.radioL.Name = "radioL";
            this.radioL.Size = new Size(0x2f, 0x10);
            this.radioL.TabIndex = 5;
            this.radioL.Text = "转让";
            //this.radioL.ForeColor = Color.FromArgb(235, 235, 235);
            this.radioL.CheckedChanged += new EventHandler(this.radioL_CheckedChanged);
            this.radioO.AutoSize = true;
            this.radioO.Checked = true;
            this.radioO.ImeMode = ImeMode.NoControl;
            this.radioO.Location = new Point(8, 12);
            this.radioO.Name = "radioO";
            this.radioO.Size = new Size(0x2f, 0x10);
            this.radioO.TabIndex = 4;
            this.radioO.TabStop = true;
            this.radioO.Text = "订立";
            //this.radioO.ForeColor = Color.FromArgb(235, 235, 235);
            this.radioO.CheckedChanged += new EventHandler(this.radioO_CheckedChanged);
            this.radioO.Enter += new EventHandler(this.radioO_Enter);
            this.numericLPrice.Location = new Point(0x4a, 0xb7+15);
            int[] numArray4 = new int[4];
            numArray4[0] = 0xf423f;
            this.numericLPrice.Maximum = new decimal(numArray4);
            this.numericLPrice.Name = "numericLPrice";
            this.numericLPrice.Size = new Size(0x4e, 0x15);
            this.numericLPrice.TabIndex = 8;
            this.numericLPrice.Visible = false;
            this.numericLPrice.KeyUp += new KeyEventHandler(this.numericLPrice_KeyUp);
            this.numericLPrice.MouseDown += new MouseEventHandler(this.numericLPrice_MouseDown);
            this.labelLPrice.AutoSize = true;
            this.labelLPrice.ImeMode = ImeMode.NoControl;
            this.labelLPrice.Location = new Point(9, 0xbb+15);
            this.labelLPrice.Name = "labelLPrice";
            this.labelLPrice.Size = new Size(0x41, 12);
            this.labelLPrice.TabIndex = 0x21;
            this.labelLPrice.Text = "指定价格：";
            this.labelLPrice.TextAlign = ContentAlignment.BottomLeft;
            this.labelLPrice.Visible = false;
            this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboMarKet.Location = new Point(0x4a, 12+15);
            this.comboMarKet.Name = "comboMarKet";
            this.comboMarKet.Size = new Size(0x4e, 20);
            this.comboMarKet.TabIndex = 30;
            this.comboMarKet.Visible = false;
            this.labTrancCode.AutoSize = true;
            this.labTrancCode.ImageAlign = ContentAlignment.MiddleLeft;
            this.labTrancCode.ImeMode = ImeMode.NoControl;
            this.labTrancCode.Location = new Point(6, 15+15);
            this.labTrancCode.Margin = new Padding(0);
            this.labTrancCode.Name = "labTrancCode";
            this.labTrancCode.Size = new Size(0x41, 12);
            this.labTrancCode.TabIndex = 2;
            this.labTrancCode.Text = "交易代码：";
            //this.labTrancCode.ForeColor = Color.FromArgb(235, 235, 235);
            this.labTrancCode.TextAlign = ContentAlignment.BottomLeft;
            this.labelMarKet.AutoSize = true;
            this.labelMarKet.ImeMode = ImeMode.NoControl;
            this.labelMarKet.Location = new Point(12, 15+15);
            this.labelMarKet.Name = "labelMarKet";
            this.labelMarKet.Size = new Size(0x41, 12);
            this.labelMarKet.TabIndex = 0x1f;
            this.labelMarKet.Text = "市场标志：";
            this.labelMarKet.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pBoxOrder);
            base.Name = "Order";
            base.Size = new Size(160, 240);
            base.Load += new EventHandler(this.Order_Load);
            this.pBoxOrder.ResumeLayout(false);
            this.pBoxOrder.PerformLayout();
            this.numericQty.EndInit();
            this.numericPrice.EndInit();
            this.groupBoxB_S.ResumeLayout(false);
            this.groupBoxB_S.PerformLayout();
            this.gbCloseMode.ResumeLayout(false);
            this.gbCloseMode.PerformLayout();
            this.groupBoxO_L.ResumeLayout(false);
            this.groupBoxO_L.PerformLayout();
            this.numericLPrice.EndInit();
            base.ResumeLayout(false);
        }

        private void labelLargestTN_Click(object sender, EventArgs e)
        {
            this.numericQty.Value = Global.ToDecimal(this.operationManager.orderOperation.GetLargestTradeNum(this.labelLargestTN.Text).ToString());
        }

        private void LargestTNInfo(string text, int colorFlag)
        {
            if (colorFlag == 0)
            {
                this.labelLargestTN.ForeColor = Global.LightColor;
            }
            else if (colorFlag == 1)
            {
                this.labelLargestTN.ForeColor = Global.ErrorColor;
            }
            this.labelLargestTN.Text = text;
        }

        private void numericLPrice_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
        }

        private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
        {
            this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
        }

        private void numericPrice_Enter(object sender, EventArgs e)
        {
            if (this.numericPrice.Value == 0M)
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
            int buysell = 0;
            if (this.radioS.Checked)
            {
                buysell = 1;
            }
            decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
            if (this.numericPrice.Value != bSPrice)
            {
                this.operationManager.orderOperation.IsChangePrice = true;
            }
            else
            {
                this.operationManager.orderOperation.IsChangePrice = false;
            }
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
            Hashtable o = new Hashtable();
            o.Add("Commodity", this.comboCommodity.Text);
            o.Add("B_SType", num);
            o.Add("O_LType", num2);
            o.Add("numericPrice", Convert.ToDouble(this.numericPrice.Value));
            o.Add("tbTranc_comboTranc", this.tbTranc.Text + this.comboTranc.Text);
            this.operationManager.orderOperation.GetNumericQtyThread(o);
        }

        private void numericQty_KeyUp(object sender, KeyEventArgs e)
        {
            Global.QtyKeyUp(sender, e);
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

        private void OrderInfoMessage(long retCode, string retMessage)
        {
            this.comboCommodity.Focus();
            if (IniData.GetInstance().ClearData)
            {
                if (this.numericPrice.Enabled)
                {
                    this.numericPrice.Value = 0M;
                }
                this.numericQty.Text = "";
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.buttonClick)
            {
                this.buttonClick = false;
                return false;
            }
            if ((keyData == Keys.D1) || (keyData == Keys.NumPad1))
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
            else if ((keyData == Keys.D2) || (keyData == Keys.NumPad2))
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
            if (!IniData.GetInstance().UpDownFocus)
            {
                return false;
            }
            if ((((keyData != Keys.Up) && (keyData != Keys.Down)) && ((keyData != Keys.Left) && (keyData != Keys.Right))) && (((keyData != Keys.Enter) || this.buttonOrder.Focused) || this.buttonAddPre.Focused))
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            this.SetFocus(keyData);
            return true;
        }

        private void radioB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboCommodity.Text.StartsWith("Y"))
            {

                if (this.radioB.Checked)
                {
                    this.radioO.Checked = true;
                }
                else
                {
                    this.radioO.Checked = false;
                }
            }

            this.operationManager.orderOperation.IsChangePrice = false;
            int buysell = 0;
            if (this.radioS.Checked)
            {
                buysell = 1;
            }
            decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
            this.numericPrice.Value = bSPrice;
        }

        private void radioB_Enter(object sender, EventArgs e)
        {
            string message = string.Format(this.OneTwoInfo, this.operationManager.StrBuy, this.operationManager.StrSale);
            if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(message, Global.RightColor, true);
            }
        }

        private void radioL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboCommodity.Text.StartsWith("Y"))
            {
                if (this.radioL.Checked)
                {
                    this.radioS.Checked = true;
                }
                else
                {
                    this.radioS.Checked = false;
                }
            }

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
                }
                this.labelLPrice.Visible = false;
                this.numericLPrice.Visible = false;
                this.gbCloseMode.Visible = false;
            }
        }

        private void radioO_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboCommodity.Text.StartsWith("Y"))
            {
                if (this.radioO.Checked)
                {
                    this.radioB.Checked = true;
                }
                else
                {
                    this.radioB.Checked = false;
                }
            }

            if (this.radioO.Checked)
            {
                this.labelLPrice.Visible = false;
                this.numericLPrice.Visible = false;
                this.gbCloseMode.Visible = false;
            }
        }

        private void radioO_Enter(object sender, EventArgs e)
        {
            if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(Global.M_ResourceManager.GetString("TradeStr_Tsxx"), Global.RightColor, true);
            }
        }

        private void radioS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboCommodity.Text.StartsWith("Y"))
            {
                if (this.radioS.Checked)
                {
                    this.radioL.Checked = true;
                }
                else
                {
                    this.radioL.Checked = false;
                }
            }

            int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
            this.SetRadioEnable(currentTradeMode);
        }

        private void radioS_Enter(object sender, EventArgs e)
        {
            string message = string.Format(this.OneTwoInfo, this.operationManager.StrBuy, this.operationManager.StrSale);
            if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(message, Global.RightColor, true);
            }
        }

        private void SetButtonOrderEnable(bool enable)
        {
            this.buttonOrder.Enabled = enable;
            this.buttonAddPre.Enabled = enable;
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

        private void SetCommodityID(string commodityID)
        {
            this.comboCommodity.Text = commodityID;
            int num = this.comboCommodity.FindStringExact(commodityID);
            if (num != -1)
            {
                this.comboCommodity.SelectedIndex = num;
            }
        }

        private bool SetCommoditySelectIndex(string marketID, string commodityID)
        {
            bool flag = false;
            if (Global.MarketHT.Count <= 1)
            {
                for (int k = 0; k < this.comboCommodity.Items.Count; k++)
                {
                    if (this.comboCommodity.Items[k].ToString() == commodityID)
                    {
                        this.comboCommodity.SelectedIndex = k;
                        return true;
                    }
                }
                return flag;
            }
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
                    return true;
                }
            }
            return flag;
        }

        private void SetControlText()
        {
            this.pBoxOrder.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder");
            this.labTrancCode.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
            this.labComCode.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
            this.labelMarKet.Text = Global.M_ResourceManager.GetString("TradeStr_LabelMarKet");
            this.radioB.Text = Global.M_ResourceManager.GetString("TradeStr_RadioB");
            this.radioS.Text = Global.M_ResourceManager.GetString("TradeStr_RadioS");
            this.radioO.Text = Global.M_ResourceManager.GetString("TradeStr_RadioO");
            this.radioL.Text = Global.M_ResourceManager.GetString("TradeStr_RadioL");
            this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice");
            this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
            if (Global.HTConfig.ContainsKey("DisplaySwitch") && Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
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
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "SetDoubleClickOrderInfo异常：" + exception.Message);
            }
        }

        private void SetFocus(Keys e)
        {
            if (e == Keys.Right)
            {
                if (this.radioB.Focused)
                {
                    this.radioS.Focus();
                }
                else if (this.radioO.Focused)
                {
                    this.radioL.Focus();
                }
                else if (this.rbCloseT.Focused)
                {
                    this.rbCloseH.Focus();
                }
                else if (this.buttonOrder.Focused)
                {
                    this.buttonAddPre.Focus();
                }
            }
            else if (e == Keys.Left)
            {
                if (this.radioS.Focused)
                {
                    this.radioB.Focus();
                }
                else if (this.radioL.Focused)
                {
                    this.radioO.Focus();
                }
                else if (this.rbCloseH.Focused)
                {
                    this.rbCloseT.Focus();
                }
                else if (this.buttonAddPre.Focused)
                {
                    this.buttonOrder.Focus();
                }
            }
            else if (e == Keys.Up)
            {
                if (this.comboCommodity.Focused)
                {
                    this.buttonOrder.Focus();
                }
                else if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
                {
                    if (this.numericLPrice.Visible)
                    {
                        this.numericLPrice.Focus();
                    }
                    else if (this.gbCloseMode.Visible)
                    {
                        if (this.rbCloseT.Checked)
                        {
                            this.rbCloseT.Focus();
                        }
                        else
                        {
                            this.rbCloseH.Focus();
                        }
                    }
                    else
                    {
                        this.numericQty.Focus();
                    }
                }
                else if ((this.numericLPrice.Focused || this.rbCloseT.Focused) || this.rbCloseH.Focused)
                {
                    this.numericQty.Focus();
                }
                else if (this.numericQty.Focused)
                {
                    this.numericPrice.Focus();
                }
                else if (this.numericPrice.Focused)
                {
                    if (this.radioO.Checked)
                    {
                        this.radioO.Focus();
                    }
                    else
                    {
                        this.radioL.Focus();
                    }
                }
                else if (this.radioO.Focused || this.radioL.Focused)
                {
                    if (this.radioB.Checked)
                    {
                        this.radioB.Focus();
                    }
                    else
                    {
                        this.radioS.Focus();
                    }
                }
                else if (this.radioB.Focused || this.radioS.Focused)
                {
                    this.comboCommodity.Focus();
                }
            }
            else if ((e == Keys.Down) || (e == Keys.Enter))
            {
                if (this.comboCommodity.Focused)
                {
                    if (this.radioB.Checked)
                    {
                        this.radioB.Focus();
                    }
                    else
                    {
                        this.radioS.Focus();
                    }
                }
                else if (this.radioB.Focused || this.radioS.Focused)
                {
                    if (this.radioO.Checked)
                    {
                        this.radioO.Focus();
                    }
                    else
                    {
                        this.radioL.Focus();
                    }
                }
                else if (this.radioO.Focused || this.radioL.Focused)
                {
                    this.numericPrice.Focus();
                }
                else if (this.numericPrice.Focused)
                {
                    this.numericQty.Focus();
                }
                else if (this.numericQty.Focused)
                {
                    if (this.numericLPrice.Visible)
                    {
                        this.numericLPrice.Focus();
                    }
                    else if (this.gbCloseMode.Visible)
                    {
                        if (this.rbCloseT.Checked)
                        {
                            this.rbCloseT.Focus();
                        }
                        else
                        {
                            this.rbCloseH.Focus();
                        }
                    }
                    else
                    {
                        this.buttonOrder.Focus();
                    }
                }
                else if ((this.numericLPrice.Focused || this.rbCloseT.Focused) || this.rbCloseH.Focused)
                {
                    this.buttonOrder.Focus();
                }
                else if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
                {
                    this.comboCommodity.Focus();
                }
            }
        }

        private void SetFouce(short flag)
        {
            this.labelLargestTN.Text = "";
            if (flag == 0)
            {
                this.comboCommodity.Focus();
            }
            else if (flag == 1)
            {
                this.numericPrice.Focus();
            }
            else if (flag == 2)
            {
                this.numericQty.Focus();
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
            if (this.radioB.Checked)
            {
                this.numericPrice.Value = (decimal)sPrice;
            }
            else if (this.radioS.Checked)
            {
                this.numericPrice.Value = (decimal)bPrice;
            }
        }

        private void SetOrderInfo(string commodityID, double buyPrice, double sellPrice)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            int index = commodityID.IndexOf("_");
            if (index != -1)
            {
                str = commodityID.Substring(0, index);
                str2 = commodityID.Substring(index + 1);
            }
            else
            {
                str2 = commodityID;
            }
            if (str2 != this.comboCommodity.Text)
            {
                this.operationManager.orderOperation.ConnectHQ = true;
            }
            if (Global.MarketHT.Count == 1)
            {
                for (int i = 0; i < this.comboCommodity.Items.Count; i++)
                {
                    if (str2.Equals(this.comboCommodity.Items[i].ToString()))
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
                }
            }
            else
            {
                for (int j = 0; j < this.comboMarKet.Items.Count; j++)
                {
                    AddValue value2 = (AddValue)this.comboMarKet.Items[j];
                    if (str.Equals(value2.Value))
                    {
                        this.comboMarKet.SelectedIndex = j;
                        break;
                    }
                }
                for (int k = 0; k < this.comboCommodity.Items.Count; k++)
                {
                    if (str2.Equals(this.comboCommodity.Items[k].ToString()))
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
                            this.numericPrice.Value = decimal.Parse(info);
                        }
                        catch (Exception)
                        {
                            this.numericPrice.Value = 0M;
                        }
                        return;

                    case 1:
                        try
                        {
                            this.numericQty.Value = decimal.Parse(info);
                        }
                        catch (Exception)
                        {
                            this.numericQty.Value = 0M;
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
                    this.radioO.Enabled = true;
                    this.radioL.Enabled = true;
                    this.radioS.Enabled = true;
                    this.radioB.Enabled = true;
                    return;

                case 1:
                    if (!this.radioB.Checked)
                    {
                        this.radioL.Enabled = false;
                        this.radioO.Checked = true;
                        return;
                    }
                    this.radioO.Enabled = false;
                    this.radioL.Checked = true;
                    return;

                case 2:
                    if (!this.radioB.Checked)
                    {
                        this.radioO.Enabled = false;
                        this.radioL.Checked = true;
                        return;
                    }
                    this.radioL.Enabled = false;
                    this.radioO.Checked = true;
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
            }
        }

        private void SetTransactionList(List<string> transactionList)
        {
            this.comboTranc.Items.Clear();
            if (transactionList.Contains(this.operationManager.StrAll))
            {
                transactionList.Remove(this.operationManager.StrAll);
            }
            foreach (string str in transactionList)
            {
                this.comboTranc.Items.Add(str.Substring(str.Length - 2));
            }
            if (this.comboTranc.Items.Count < 2)
            {
                this.comboTranc.Visible = false;
                this.tbTranc.Size = new Size(this.comboCommodity.Width, 20);
            }
            this.tbTranc.Text = Global.FirmID;
            this.comboTranc.SelectedIndex = 0;
        }

        private void SubmintOrder()
        {
            SubmitOrderInfo orderInfo = new SubmitOrderInfo
            {
                customerID = this.tbTranc.Text + this.comboTranc.Text,
                commodityID = this.comboCommodity.Text
            };
            if (this.radioS.Checked)
            {
                orderInfo.B_SType = 2;
            }
            if (this.radioL.Checked)
            {
                orderInfo.O_LType = 2;
            }
            orderInfo.price = Tools.StrToDouble(this.numericPrice.Value.ToString(), 0.0);
            orderInfo.qty = Tools.StrToInt(this.numericQty.Value.ToString(), 0);
            if (orderInfo.O_LType == 2)
            {
                if (IniData.GetInstance().CloseMode == 2)
                {
                    orderInfo.closeMode = 2;
                    if (this.rbCloseT.Checked)
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
