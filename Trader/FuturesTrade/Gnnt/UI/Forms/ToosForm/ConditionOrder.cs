namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
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

    public class ConditionOrder : Form
    {
        private MyButton btnCancel;
        private MyButton btnConmmit;
        private MyCombobox comboBoxCommodityID;
        private MyCombobox comboConTypeOrderS;
        private MyCombobox comboOperatorS;
        private IContainer components;
        private DateTimePicker dateTimePicker1;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        internal GroupBox groupBoxSimple;
        private static volatile ConditionOrder instance;
        private bool isClose;
        private Label labelBSS;
        private Label labelCommodityIDS;
        private Label labelConditionS;
        private Label labelLargestTN;
        private Label labelOLS;
        private Label labelPriceS;
        private Label labelQtyS;
        private Label labelTimeS;
        private NumericUpDown numericConPriceS;
        private NumericUpDown numericPriceS;
        private NumericUpDown numericQtyS;
        private OperationManager operationManager = OperationManager.GetInstance();
        private OrderMessageInfoCallBack OrderMessageInfo;
        private PromptLargestTradeNumCallBack PromptLargestTradeNum;
        private MyRadioButton radioS_B;
        private MyRadioButton radioS_L;
        private MyRadioButton radioS_O;
        private MyRadioButton radioS_S;

        public ConditionOrder()
        {
            this.InitializeComponent();
            this.operationManager.submitConOrderOperation.ConOrderMessage = new SubmitConOrderOperation.ConOrderMessageCallBack(this.OrderMessage);
            this.operationManager.conOrderOperation.setLargestTN = new ConOrderOperation.SetLargestTNCallBack(this.SetLargestTNInfo);
            Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnConmmit_Click(object sender, EventArgs e)
        {
            this.SubmintConOrder();
        }

        public void CloseForm()
        {
            this.isClose = true;
            base.Close();
            base.Dispose();
        }

        private void comboBoxCommodityID_KeyDown(object sender, KeyEventArgs e)
        {
            this.operationManager.conOrderOperation.ComboxKeyDown(e);
        }

        private void comboBoxCommodityID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.numericPriceS.Value = 0M;
            this.numericQtyS.Value = 0M;
            this.numericConPriceS.Value = 0M;
            int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboBoxCommodityID.Text);
            this.SetRadioEnable(currentTradeMode);
        }

        private void comboBoxCommodityID_TextChanged(object sender, EventArgs e)
        {
            if (this.comboBoxCommodityID.Text.StartsWith("Y"))
            {
                if (!this.radioS_B.Checked)
                {
                    this.radioS_B.Checked = true;
                }
                //if (!radioS_O.Checked)
                //{
                //    this.radioS_O.Checked = true;
                //}
               
            }
            decimal commoditySpread = this.operationManager.orderOperation.GetCommoditySpread(this.comboBoxCommodityID.Text);
            this.numericPriceS.Increment = commoditySpread;
            this.numericConPriceS.Increment = commoditySpread;
            int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
            this.numericPriceS.DecimalPlaces = decimalPlaces;
            this.numericConPriceS.DecimalPlaces = decimalPlaces;
        }

        private void ComboCommodityLoad()
        {
            this.comboBoxCommodityID.Items.Clear();
            try
            {
                if (OperationManager.GetInstance().myCommodityList.Count > 1)
                {
                    foreach (string str in OperationManager.GetInstance().myCommodityList)
                    {
                        if (str != OperationManager.GetInstance().StrAll)
                        {
                            this.comboBoxCommodityID.Items.Add(str);
                        }
                    }
                }
                else
                {
                    foreach (string str2 in OperationManager.GetInstance().commodityList)
                    {
                        if (str2 != OperationManager.GetInstance().StrAll)
                        {
                            this.comboBoxCommodityID.Items.Add(str2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "获取商品信息错误：" + exception.Message);
            }
            if (this.comboBoxCommodityID.Items.Count > 0)
            {
                this.comboBoxCommodityID.SelectedIndex = 0;
            }
        }

        private void ConditionOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void ConditionOrder_Load(object sender, EventArgs e)
        {
            this.ComboCommodityLoad();
            this.comboConTypeOrderS.SelectedIndex = 0;
            this.comboOperatorS.SelectedIndex = 0;
            ScaleForm.ScaleForms(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (instance != null)
            {
                instance = null;
            }
            Global.SetOrderInfo -= new Global.SetOrderInfoCallBack(this.SetOrderInfo);
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
            this.groupBoxSimple = new GroupBox();
            this.labelLargestTN = new Label();
            this.labelCommodityIDS = new Label();
            this.comboBoxCommodityID = new MyCombobox();
            this.dateTimePicker1 = new DateTimePicker();
            this.labelTimeS = new Label();
            this.labelQtyS = new Label();
            this.labelPriceS = new Label();
            this.numericQtyS = new NumericUpDown();
            this.numericPriceS = new NumericUpDown();
            this.groupBox3 = new GroupBox();
            this.radioS_S = new MyRadioButton();
            this.radioS_B = new MyRadioButton();
            this.labelOLS = new Label();
            this.labelBSS = new Label();
            this.numericConPriceS = new NumericUpDown();
            this.groupBox4 = new GroupBox();
            this.radioS_L = new MyRadioButton();
            this.radioS_O = new MyRadioButton();
            this.labelConditionS = new Label();
            this.btnCancel = new MyButton();
            this.btnConmmit = new MyButton();
            this.comboOperatorS = new MyCombobox();
            this.comboConTypeOrderS = new MyCombobox();
            this.groupBoxSimple.SuspendLayout();
            this.numericQtyS.BeginInit();
            this.numericPriceS.BeginInit();
            this.groupBox3.SuspendLayout();
            this.numericConPriceS.BeginInit();
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
            this.groupBoxSimple.Size = new Size(0x144, 0x115);
            this.groupBoxSimple.TabIndex = 40;
            this.groupBoxSimple.TabStop = false;
            this.groupBoxSimple.Text = "条件单设置";
            this.labelLargestTN.AutoSize = true;
            this.labelLargestTN.ImeMode = ImeMode.NoControl;
            this.labelLargestTN.Location = new Point(0xa1, 0xd3);
            this.labelLargestTN.Name = "labelLargestTN";
            this.labelLargestTN.Size = new Size(0x4d, 12);
            this.labelLargestTN.TabIndex = 0x1f;
            this.labelLargestTN.Text = "最大可交易量";
            this.labelLargestTN.Click += new EventHandler(this.labelLargestTN_Click);
            this.labelCommodityIDS.AutoSize = true;
            this.labelCommodityIDS.Location = new Point(10, 0x22);
            this.labelCommodityIDS.Name = "labelCommodityIDS";
            this.labelCommodityIDS.Size = new Size(0x29, 12);
            this.labelCommodityIDS.TabIndex = 0x1c;
            this.labelCommodityIDS.Text = "商品：";
            this.comboBoxCommodityID.FormattingEnabled = true;
            this.comboBoxCommodityID.Location = new Point(0x3b, 0x1f);
            this.comboBoxCommodityID.Name = "comboBoxCommodityID";
            this.comboBoxCommodityID.Size = new Size(0x51, 20);
            this.comboBoxCommodityID.TabIndex = 1;
            this.comboBoxCommodityID.SelectedIndexChanged += new EventHandler(this.comboBoxCommodityID_SelectedIndexChanged);
            this.comboBoxCommodityID.TextChanged += new EventHandler(this.comboBoxCommodityID_TextChanged);
            this.comboBoxCommodityID.KeyDown += new KeyEventHandler(this.comboBoxCommodityID_KeyDown);
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.DropDownAlign = LeftRightAlignment.Right;
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new Point(0xcb, 0x1f);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new Size(0x58, 0x15);
            this.dateTimePicker1.TabIndex = 2;
            this.labelTimeS.AutoSize = true;
            this.labelTimeS.Location = new Point(0x93, 0x24);
            this.labelTimeS.Name = "labelTimeS";
            this.labelTimeS.Size = new Size(0x35, 12);
            this.labelTimeS.TabIndex = 0x1a;
            this.labelTimeS.Text = "有效期：";
            this.labelQtyS.AutoSize = true;
            this.labelQtyS.Location = new Point(0xa1, 0xb8);
            this.labelQtyS.Name = "labelQtyS";
            this.labelQtyS.Size = new Size(0x29, 12);
            this.labelQtyS.TabIndex = 0x19;
            this.labelQtyS.Text = "数量：";
            this.labelPriceS.AutoSize = true;
            this.labelPriceS.Location = new Point(10, 0xb8);
            this.labelPriceS.Name = "labelPriceS";
            this.labelPriceS.Size = new Size(0x29, 12);
            this.labelPriceS.TabIndex = 0x18;
            this.labelPriceS.Text = "价格：";
            int[] bits = new int[4];
            bits[0] = 10;
            this.numericQtyS.Increment = new decimal(bits);
            this.numericQtyS.Location = new Point(210, 0xb3);
            int[] numArray2 = new int[4];
            numArray2[0] = 0xf423f;
            this.numericQtyS.Maximum = new decimal(numArray2);
            this.numericQtyS.Name = "numericQtyS";
            this.numericQtyS.Size = new Size(0x51, 0x15);
            this.numericQtyS.TabIndex = 9;
            this.numericQtyS.Enter += new EventHandler(this.numericQtyS_Enter);
            this.numericQtyS.KeyUp += new KeyEventHandler(this.numericQtyS_KeyUp);
            int[] numArray3 = new int[4];
            numArray3[0] = 10;
            this.numericPriceS.Increment = new decimal(numArray3);
            this.numericPriceS.Location = new Point(0x3b, 180);
            int[] numArray4 = new int[4];
            numArray4[0] = 0xf423f;
            this.numericPriceS.Maximum = new decimal(numArray4);
            this.numericPriceS.Name = "numericPriceS";
            this.numericPriceS.Size = new Size(0x51, 0x15);
            this.numericPriceS.TabIndex = 8;
            this.numericPriceS.Enter += new EventHandler(this.numericPriceS_Enter);
            this.numericPriceS.KeyUp += new KeyEventHandler(this.numericPriceS_KeyUp);
            this.groupBox3.BackColor = Color.Transparent;
            this.groupBox3.Controls.Add(this.radioS_S);
            this.groupBox3.Controls.Add(this.radioS_B);
            this.groupBox3.Location = new Point(0x3b, 0x5f);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x8b, 0x25);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.radioS_S.AutoSize = true;
            this.radioS_S.ForeColor = Color.Green;
            this.radioS_S.ImeMode = ImeMode.NoControl;
            this.radioS_S.Location = new Point(0x45, 14);
            this.radioS_S.Name = "radioS_S";
            this.radioS_S.Size = new Size(0x2f, 0x10);
            this.radioS_S.TabIndex = 2;
            this.radioS_S.Text = "卖出";
            this.radioS_S.CheckedChanged += new EventHandler(this.radioS_B_CheckedChanged);
            this.radioS_B.AutoSize = true;
            this.radioS_B.Checked = true;
            this.radioS_B.ForeColor = Color.Red;
            this.radioS_B.ImeMode = ImeMode.NoControl;
            this.radioS_B.Location = new Point(10, 14);
            this.radioS_B.Name = "radioS_B";
            this.radioS_B.Size = new Size(0x2f, 0x10);
            this.radioS_B.TabIndex = 1;
            this.radioS_B.TabStop = true;
            this.radioS_B.Text = "买入";
            this.radioS_B.CheckedChanged += new EventHandler(this.radioS_B_CheckedChanged);
            this.labelOLS.AutoSize = true;
            this.labelOLS.Location = new Point(10, 0x93);
            this.labelOLS.Name = "labelOLS";
            this.labelOLS.Size = new Size(0x29, 12);
            this.labelOLS.TabIndex = 0x13;
            this.labelOLS.Text = "开平：";
            this.labelBSS.AutoSize = true;
            this.labelBSS.Location = new Point(10, 0x6c);
            this.labelBSS.Name = "labelBSS";
            this.labelBSS.Size = new Size(0x29, 12);
            this.labelBSS.TabIndex = 0x12;
            this.labelBSS.Text = "买卖：";
            int[] numArray5 = new int[4];
            numArray5[0] = 10;
            this.numericConPriceS.Increment = new decimal(numArray5);
            this.numericConPriceS.Location = new Point(210, 0x43);
            int[] numArray6 = new int[4];
            numArray6[0] = 0xf423f;
            this.numericConPriceS.Maximum = new decimal(numArray6);
            this.numericConPriceS.Name = "numericConPriceS";
            this.numericConPriceS.Size = new Size(0x51, 0x15);
            this.numericConPriceS.TabIndex = 5;
            this.numericConPriceS.KeyUp += new KeyEventHandler(this.numericConPriceS_KeyUp);
            this.groupBox4.BackColor = Color.Transparent;
            this.groupBox4.Controls.Add(this.radioS_L);
            this.groupBox4.Controls.Add(this.radioS_O);
            this.groupBox4.Location = new Point(0x3b, 0x84);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x8b, 0x25);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.radioS_L.AutoSize = true;
            this.radioS_L.ForeColor = Color.Black;
            this.radioS_L.ImeMode = ImeMode.NoControl;
            this.radioS_L.Location = new Point(0x45, 14);
            this.radioS_L.Name = "radioS_L";
            this.radioS_L.Size = new Size(0x2f, 0x10);
            this.radioS_L.TabIndex = 2;
            this.radioS_L.CheckedChanged += new EventHandler(this.radioS_L_CheckedChanged);
            this.radioS_L.Text = "转让";
            this.radioS_O.AutoSize = true;
            this.radioS_O.Checked = true;
            this.radioS_O.ForeColor = Color.Black;
            this.radioS_O.ImeMode = ImeMode.NoControl;
            this.radioS_O.Location = new Point(10, 14);
            this.radioS_O.Name = "radioS_O";
            this.radioS_O.CheckedChanged += new EventHandler(this.radioS_L_CheckedChanged);
            this.radioS_O.Size = new Size(0x2f, 0x10);
            this.radioS_O.TabIndex = 1;
            this.radioS_O.TabStop = true;
            this.radioS_O.Text = "订立";
            this.labelConditionS.AutoSize = true;
            this.labelConditionS.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelConditionS.Location = new Point(10, 0x47);
            this.labelConditionS.Name = "labelConditionS";
            this.labelConditionS.Size = new Size(0x29, 12);
            this.labelConditionS.TabIndex = 4;
            this.labelConditionS.Text = "条件：";
            this.btnCancel.Location = new Point(200, 0xe9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x42, 0x17);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnConmmit.Location = new Point(0x3e, 0xe9);
            this.btnConmmit.Name = "btnConmmit";
            this.btnConmmit.Size = new Size(0x42, 0x17);
            this.btnConmmit.TabIndex = 10;
            this.btnConmmit.Text = "提交";
            this.btnConmmit.UseVisualStyleBackColor = true;
            this.btnConmmit.Click += new EventHandler(this.btnConmmit_Click);
            this.comboOperatorS.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboOperatorS.FormattingEnabled = true;
            this.comboOperatorS.Items.AddRange(new object[] { "<", ">", "=", "≤", "≥" });
            this.comboOperatorS.Location = new Point(0x8d, 0x43);
            this.comboOperatorS.Name = "comboOperatorS";
            this.comboOperatorS.Size = new Size(0x39, 20);
            this.comboOperatorS.TabIndex = 4;
            this.comboConTypeOrderS.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboConTypeOrderS.FormattingEnabled = true;
            this.comboConTypeOrderS.Items.AddRange(new object[] { "买1价", "卖1价", "最新价" });
            this.comboConTypeOrderS.Location = new Point(0x3b, 0x43);
            this.comboConTypeOrderS.Name = "comboConTypeOrderS";
            this.comboConTypeOrderS.Size = new Size(0x45, 20);
            this.comboConTypeOrderS.TabIndex = 3;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x144, 0x115);
            base.Controls.Add(this.groupBoxSimple);
            base.Name = "ConditionOrder";
            this.Text = "条件下单";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
            base.FormClosing += new FormClosingEventHandler(this.ConditionOrder_FormClosing);
            base.Load += new EventHandler(this.ConditionOrder_Load);
            this.groupBoxSimple.ResumeLayout(false);
            this.groupBoxSimple.PerformLayout();
            this.numericQtyS.EndInit();
            this.numericPriceS.EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.numericConPriceS.EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            base.ResumeLayout(false);
        }

        public static ConditionOrder Instance()
        {
            if (instance == null)
            {
                lock (typeof(ConditionOrder))
                {
                    if (instance == null)
                    {
                        instance = new ConditionOrder();
                    }
                }
            }
            return instance;
        }

        private void labelLargestTN_Click(object sender, EventArgs e)
        {
            this.numericQtyS.Value = Global.ToDecimal(this.operationManager.orderOperation.GetLargestTradeNum(this.labelLargestTN.Text).ToString());
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

        private void numericConPriceS_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
        }

        private void numericPriceS_Enter(object sender, EventArgs e)
        {
            if (this.numericPriceS.Value == 0M)
            {
                this.numericPriceS.Select(0, this.numericPriceS.Text.Length);
            }
            this.operationManager.orderOperation.GetCommoditySpread(this.comboBoxCommodityID.Text);
        }

        private void numericPriceS_KeyUp(object sender, KeyEventArgs e)
        {
            Global.PriceKeyUp(sender, e);
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
            Hashtable o = new Hashtable();
            o.Add("Commodity", this.comboBoxCommodityID.Text);
            o.Add("B_SType", num);
            o.Add("O_LType", num2);
            o.Add("numericPrice", Convert.ToDouble(this.numericPriceS.Value));
            o.Add("tbTranc_comboTranc", Global.UserID + "00");
            this.operationManager.conOrderOperation.GetNumericQtyThread(o);
        }

        private void numericQtyS_KeyUp(object sender, KeyEventArgs e)
        {
            Global.QtyKeyUp(sender, e);
        }

        private void OrderInfoMessage(long retCode, string retMessage)
        {
            this.comboBoxCommodityID.Focus();
            if (IniData.GetInstance().ClearData)
            {
                this.numericConPriceS.Value = 0M;
                this.numericPriceS.Value = 0M;
                this.numericQtyS.Text = "";
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
            else if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(retMessage, Global.ErrorColor, true);
            }
        }

        private void OrderMessage(long retCode, string retMessage)
        {
            try
            {
                this.OrderMessageInfo = new OrderMessageInfoCallBack(this.OrderInfoMessage);
                this.HandleCreated();
                base.Invoke(this.OrderMessageInfo, new object[] { retCode, retMessage });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }
        private void radioS_L_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboBoxCommodityID.Text.StartsWith("Y"))
            {
                if (this.radioS_O.Checked&&!this.radioS_B.Checked)
                {
                    this.radioS_B.Checked = true;
                }


                if (this.radioS_L.Checked&&!this.radioS_S.Checked)
                {
                    this.radioS_S.Checked = true;
                }
    
            }
        }
        private void radioS_B_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboBoxCommodityID.Text.StartsWith("Y"))
            {

                if (this.radioS_B.Checked&&!this.radioS_O.Checked)
                {
                    this.radioS_O.Checked = true;
                }
              

                if (this.radioS_S.Checked&&!this.radioS_L.Checked)
                {
                    this.radioS_L.Checked = true;
                }
          

              


            }

            int buysell = 0;
            if (this.radioS_S.Checked)
            {
                buysell = 1;
            }
            decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(buysell);
            this.numericPriceS.Value = bSPrice;
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboBoxCommodityID.Items.Clear();
            foreach (string str in commodityIDList)
            {
                if (str != this.operationManager.StrAll)
                {
                    this.comboBoxCommodityID.Items.Add(str);
                }
            }
            this.comboBoxCommodityID.SelectedIndex = 0;
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

        public void SetOrderInfo(string comboCommodityCode, double BuyPrice, double SellPrice)
        {
            string str = string.Empty;
            int index = comboCommodityCode.IndexOf("_");
            if (index != -1)
            {
                comboCommodityCode.Substring(0, index);
                str = comboCommodityCode.Substring(index + 1);
            }
            else
            {
                str = comboCommodityCode;
            }
            if (Global.MarketHT.Count == 1)
            {
                for (int i = 0; i < this.comboBoxCommodityID.Items.Count; i++)
                {
                    if (str.Equals(this.comboBoxCommodityID.Items[i].ToString()))
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
                }
            }
            else
            {
                for (int j = 0; j < this.comboBoxCommodityID.Items.Count; j++)
                {
                    if (str.Equals(this.comboBoxCommodityID.Items[j].ToString()))
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
                }
            }
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
                    if (!this.radioS_B.Checked)
                    {
                        this.radioS_L.Enabled = false;
                        this.radioS_O.Checked = true;
                        return;
                    }
                    this.radioS_O.Enabled = false;
                    this.radioS_L.Checked = true;
                    return;

                case 2:
                    if (!this.radioS_B.Checked)
                    {
                        this.radioS_O.Enabled = false;
                        this.radioS_L.Checked = true;
                        return;
                    }
                    this.radioS_L.Enabled = false;
                    this.radioS_O.Checked = true;
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
            }
        }

        private void SubmintConOrder()
        {
            SubmitConOrderInfo orderInfo = new SubmitConOrderInfo
            {
                commodityID = this.comboBoxCommodityID.Text,
                customerID = Global.UserID
            };
            if (this.radioS_S.Checked)
            {
                orderInfo.B_SType = 2;
            }
            if (this.radioS_L.Checked)
            {
                orderInfo.O_LType = 2;
            }
            orderInfo.datetime = this.dateTimePicker1.Text;
            orderInfo.conprice = Tools.StrToDouble(this.numericConPriceS.Value.ToString(), 0.0);
            orderInfo.price = Tools.StrToDouble(this.numericPriceS.Value.ToString(), 0.0);
            orderInfo.qty = Tools.StrToInt(this.numericQtyS.Value.ToString(), 0);
            if (this.comboConTypeOrderS.SelectedIndex == 0)
            {
                orderInfo.contype = 2;
            }
            else if (this.comboConTypeOrderS.SelectedIndex == 1)
            {
                orderInfo.contype = 3;
            }
            else if (this.comboConTypeOrderS.SelectedIndex == 2)
            {
                orderInfo.contype = 1;
            }
            if (this.comboOperatorS.SelectedIndex == 0)
            {
                orderInfo.conoperator = -1;
            }
            else if (this.comboOperatorS.SelectedIndex == 1)
            {
                orderInfo.conoperator = 1;
            }
            else if (this.comboOperatorS.SelectedIndex == 2)
            {
                orderInfo.conoperator = 0;
            }
            else if (this.comboOperatorS.SelectedIndex == 3)
            {
                orderInfo.conoperator = -2;
            }
            else if (this.comboOperatorS.SelectedIndex == 4)
            {
                orderInfo.conoperator = 2;
            }
            this.operationManager.submitConOrderOperation.ButtonConOrderComm(orderInfo);
        }

        private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);

        private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);
    }
}
