namespace FuturesTrade.Gnnt.UI.Modules.Query
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.DBService.ServiceManager;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class HoldingCollect : UserControl
    {
        private MyButton buttonHolding;
        private MyButton buttonSelF4;
        internal MyCombobox comboB_SF5;
        internal MyCombobox comboCommodityF4;
        internal MyCombobox comboTrancF4;
        private IContainer components;
        internal DataGridView dgHoldingCollect;
        private FillHolding fillHolding;
        private Panel groupBoxF4_1;
        private GroupBox groupBoxHoldingCollect;
        private HoldingItemInfo holdingItemInfo = new HoldingItemInfo();
        private bool isFirstLoad = true;
        private bool isHoldingDetailDataLoad;
        private bool isHoldingHeaderLoad;
        internal Label labelB_SF5;
        private Label labelCommodityF4;
        internal Label labelTrancF4;
        private OperationManager operationManager = OperationManager.GetInstance();
        internal MyRadioButton radioHdCollect;
        internal MyRadioButton radioHdDetail;
        private int Style;

        public HoldingCollect(int style)
        {
            this.InitializeComponent();
            this.Style = style;
            this.operationManager.queryHoldingOperation.HoldingFillEvent = new QueryHoldingOperation.HoldingFillCallBack(this.HoldingInfoFill);
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.operationManager.ShowHoldingCollect = new OperationManager.ShowHoldingControlCallBack(this.SetVisible);
            this.CreateHandle();
        }

        private void buttonSelF4_Click(object sender, EventArgs e)
        {
            this.operationManager.queryHoldingOperation.ButtonRefreshFlag = 1;
            this.operationManager.queryHoldingOperation.QueryHoldingInfoLoad();
            this.operationManager.IdleRefreshButton = 0;
        }

        private void ComboLoad()
        {
            this.comboB_SF5.Items.Add(this.operationManager.StrAll);
            this.comboB_SF5.Items.Add(this.operationManager.StrBuy);
            this.comboB_SF5.Items.Add(this.operationManager.StrSale);
            this.comboB_SF5.SelectedIndex = 0;
            this.labelB_SF5.Visible = false;
            this.comboB_SF5.Visible = false;
            if (Global.CustomerCount < 2)
            {
                this.comboTrancF4.Visible = false;
                this.labelTrancF4.Visible = false;
                this.labelB_SF5.Location = new Point(this.labelB_SF5.Location.X - 150, this.labelB_SF5.Location.Y);
                this.comboB_SF5.Location = new Point(this.comboB_SF5.Location.X - 150, this.comboB_SF5.Location.Y);
            }
        }

        private void dgHoldingCollect_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                if ((e.ColumnIndex == 2) || (e.ColumnIndex == 3))
                {
                    string message = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessage1");
                    Global.StatusInfoFill(message, Global.RightColor, true);
                }
                else if ((e.ColumnIndex == 4) || (e.ColumnIndex == 5))
                {
                    string str2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessage2");
                    Global.StatusInfoFill(str2, Global.RightColor, true);
                }
                if ((Tools.StrToBool((string)Global.HTConfig["AutoDisplayMinLine"], false) && (e.RowIndex > -1)) && (e.RowIndex < (this.dgHoldingCollect.RowCount - 1)))
                {
                    Global.displayMinLine(this.dgHoldingCollect["Market", e.RowIndex].Value.ToString().Trim(), this.dgHoldingCollect[0, e.RowIndex].Value.ToString().Trim());
                }
            }
        }

        private void dgHoldingCollect_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = false;
            if (e.RowIndex != -1)
            {
                if (this.dgHoldingCollect.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == this.operationManager.Total)
                {
                    Global.StatusInfoFill("", Global.RightColor, true);
                }
                string marketID = this.dgHoldingCollect["Market", e.RowIndex].Value.ToString().Trim();
                string commodityID = this.dgHoldingCollect["CommodityID", e.RowIndex].Value.ToString().Trim();
                if (Global.SetCommoditySelectIndex != null)
                {
                    flag = Global.SetCommoditySelectIndex(marketID, commodityID);
                }
                if (flag)
                {
                    Global.M_ResourceManager.GetString("TradeStr_MainFormF5_QuickOrderFailed");
                    CommData data = ServiceManage.GetInstance().CreateQueryCommData().QueryGNCommodityInfo(marketID, commodityID);
                    short buysell = 0;
                    short ordertype = 0;
                    double price = 0.0;
                    double lprice = 0.0;
                    int qty = 0;
                    if (((e.ColumnIndex == this.dgHoldingCollect.Columns["BuyHolding"].Index) || (e.ColumnIndex == this.dgHoldingCollect.Columns["BuyVHolding"].Index)) || (e.ColumnIndex == this.dgHoldingCollect.Columns["BuyAvg"].Index))
                    {
                        buysell = 1;
                        ordertype = 1;
                        price = data.Bid;
                        qty = Tools.StrToInt(this.dgHoldingCollect["BuyVHolding", e.RowIndex].Value.ToString(), 0);
                    }
                    else if (((e.ColumnIndex == this.dgHoldingCollect.Columns["SellHolding"].Index) || (e.ColumnIndex == this.dgHoldingCollect.Columns["SellAvg"].Index)) || (e.ColumnIndex == this.dgHoldingCollect.Columns["SellVHolding"].Index))
                    {
                        buysell = 0;
                        ordertype = 1;
                        price = data.Offer;
                        qty = Tools.StrToInt(this.dgHoldingCollect["SellVHolding", e.RowIndex].Value.ToString(), 0);
                    }
                    if (Global.SetDoubleClickOrderInfo != null)
                    {
                        Global.SetDoubleClickOrderInfo(price, lprice, qty, buysell, ordertype);
                    }
                }
                else
                {
                    string message = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_NotExistGoods");
                    Global.StatusInfoFill(message, Global.ErrorColor, true);
                }
            }
        }

        private void dgHoldingCollect_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == 2) || (e.ColumnIndex == 3))
            {
                string str = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessageMaxBuy");
                DataGridViewCell cell = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = str;
            }
            else if ((e.ColumnIndex == 4) || (e.ColumnIndex == 5))
            {
                string str2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessageMinSale");
                DataGridViewCell cell2 = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell2.ToolTipText = str2;
            }
        }

        private void dgHoldingCollect_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                if (this.radioHdCollect.Checked)
                {
                    this.operationManager.queryHoldingOperation.HoldingSort(this.dgHoldingCollect.Columns[e.ColumnIndex].Name.ToString());
                }
                else if (this.radioHdDetail.Checked)
                {
                    this.operationManager.queryHoldingDatailOperation.HoldingDetailSort(this.dgHoldingCollect.Columns[e.ColumnIndex].Name.ToString());
                }
            }
        }

        private void dgHoldingCollect_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((this.dgHoldingCollect.RowCount > 1) && (this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].Cells["CommodityID"].Value.ToString().Trim() == "合计"))
            {
                this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
                this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].ReadOnly = true;
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

        private void dsHoldingFill(DataTable dt)
        {
            this.dgHoldingCollect.DataSource = dt;
            this.SetDataGridViewHeader();
        }

        public void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void HoldingCollect_Load(object sender, EventArgs e)
        {
            if (this.Style == 1)
            {
                this.buttonHolding.Visible = true;
                this.groupBoxF4_1.Visible = false;
            }
            else
            {
                this.buttonHolding.Visible = false;
                this.groupBoxF4_1.Visible = true;
            }
            this.SetControlText();
            this.ComboLoad();
        }

        private void HoldingInfoFill(DataTable dTable)
        {
            try
            {
                this.fillHolding = new FillHolding(this.dsHoldingFill);
                this.HandleCreated();
                base.Invoke(this.fillHolding, new object[] { dTable });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.groupBoxHoldingCollect = new GroupBox();
            this.groupBoxF4_1 = new Panel();
            this.radioHdCollect = new MyRadioButton();
            this.radioHdDetail = new MyRadioButton();
            this.labelB_SF5 = new Label();
            this.comboB_SF5 = new MyCombobox();
            this.dgHoldingCollect = new DataGridView();
            this.comboTrancF4 = new MyCombobox();
            this.comboCommodityF4 = new MyCombobox();
            this.labelTrancF4 = new Label();
            this.labelCommodityF4 = new Label();
            this.buttonSelF4 = new MyButton();
            this.buttonHolding = new MyButton();
            this.groupBoxHoldingCollect.SuspendLayout();
            this.groupBoxF4_1.SuspendLayout();
            ((ISupportInitialize)this.dgHoldingCollect).BeginInit();
            base.SuspendLayout();
            this.groupBoxHoldingCollect.Controls.Add(this.groupBoxF4_1);
            this.groupBoxHoldingCollect.Controls.Add(this.labelB_SF5);
            this.groupBoxHoldingCollect.Controls.Add(this.comboB_SF5);
            this.groupBoxHoldingCollect.Controls.Add(this.dgHoldingCollect);
            this.groupBoxHoldingCollect.Controls.Add(this.comboTrancF4);
            this.groupBoxHoldingCollect.Controls.Add(this.comboCommodityF4);
            this.groupBoxHoldingCollect.Controls.Add(this.labelTrancF4);
            this.groupBoxHoldingCollect.Controls.Add(this.labelCommodityF4);
            this.groupBoxHoldingCollect.Controls.Add(this.buttonSelF4);
            this.groupBoxHoldingCollect.Dock = DockStyle.Fill;
            this.groupBoxHoldingCollect.Location = new Point(0x1f, 0);
            this.groupBoxHoldingCollect.Name = "groupBoxHoldingCollect";
            this.groupBoxHoldingCollect.Size = new Size(0x29d, 200);
            this.groupBoxHoldingCollect.TabIndex = 0x16;
            this.groupBoxHoldingCollect.TabStop = false;
            this.groupBoxHoldingCollect.Text = "订货汇总";
            this.groupBoxHoldingCollect.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxHoldingCollect.ForeColor = Color.FromArgb(235, 235, 235);
            this.groupBoxF4_1.Controls.Add(this.radioHdCollect);
            this.groupBoxF4_1.Controls.Add(this.radioHdDetail);
            this.groupBoxF4_1.Location = new Point(0x1bd, 12);
            this.groupBoxF4_1.Name = "groupBoxF4_1";
            this.groupBoxF4_1.Size = new Size(0x9a, 0x19);
            this.groupBoxF4_1.TabIndex = 0x1b;
            this.radioHdCollect.AutoSize = true;
            this.radioHdCollect.Checked = true;
            this.radioHdCollect.ImeMode = ImeMode.NoControl;
            this.radioHdCollect.Location = new Point(3, 4);
            this.radioHdCollect.Name = "radioHdCollect";
            this.radioHdCollect.Size = new Size(0x47, 0x10);
            this.radioHdCollect.TabIndex = 0x19;
            this.radioHdCollect.TabStop = true;
            this.radioHdCollect.Text = "订货汇总";
            this.radioHdCollect.UseVisualStyleBackColor = true;
            this.radioHdDetail.AutoSize = true;
            this.radioHdDetail.ImeMode = ImeMode.NoControl;
            this.radioHdDetail.Location = new Point(80, 4);
            this.radioHdDetail.Name = "radioHdDetail";
            this.radioHdDetail.Size = new Size(0x47, 0x10);
            this.radioHdDetail.TabIndex = 0x1a;
            this.radioHdDetail.Text = "订货明细";
            this.radioHdDetail.UseVisualStyleBackColor = true;
            this.radioHdDetail.Click += new EventHandler(this.radioHdDetail_Click);
            this.labelB_SF5.AutoSize = true;
            this.labelB_SF5.ImeMode = ImeMode.NoControl;
            this.labelB_SF5.Location = new Point(330, 0x13);
            this.labelB_SF5.Name = "labelB_SF5";
            this.labelB_SF5.Size = new Size(0x23, 12);
            this.labelB_SF5.TabIndex = 0x18;
            this.labelB_SF5.Text = "买/卖";
            this.comboB_SF5.FormattingEnabled = true;
            this.comboB_SF5.Location = new Point(0x173, 14);
            this.comboB_SF5.Name = "comboB_SF5";
            this.comboB_SF5.Size = new Size(0x35, 20);
            this.comboB_SF5.TabIndex = 0x17;
            this.comboB_SF5.TabStop = false;
            this.comboB_SF5.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.dgHoldingCollect.AllowUserToAddRows = false;
            this.dgHoldingCollect.AllowUserToDeleteRows = false;
            this.dgHoldingCollect.AllowUserToOrderColumns = true;
            this.dgHoldingCollect.AllowUserToResizeRows = false;
            this.dgHoldingCollect.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgHoldingCollect.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgHoldingCollect.ColumnHeadersDefaultCellStyle = style;
            this.dgHoldingCollect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dgHoldingCollect.DefaultCellStyle = style2;
            this.dgHoldingCollect.Location = new Point(4, 40);
            this.dgHoldingCollect.Name = "dgHoldingCollect";
            this.dgHoldingCollect.ReadOnly = true;
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dgHoldingCollect.RowHeadersDefaultCellStyle = style3;
            this.dgHoldingCollect.RowHeadersVisible = false;
            this.dgHoldingCollect.RowTemplate.Height = 20;
            this.dgHoldingCollect.ScrollBars = ScrollBars.Vertical;
            this.dgHoldingCollect.Size = new Size(0x296, 0x9b);
            this.dgHoldingCollect.TabIndex = 0x16;
            this.dgHoldingCollect.TabStop = false;
            this.dgHoldingCollect.CellClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellClick);
            this.dgHoldingCollect.CellDoubleClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellDoubleClick);
            this.dgHoldingCollect.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgHoldingCollect_CellFormatting);
            this.dgHoldingCollect.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgHoldingCollect_ColumnHeaderMouseClick);
            this.dgHoldingCollect.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgHoldingCollect_DataBindingComplete);
            this.comboTrancF4.Location = new Point(0xec, 15);
            this.comboTrancF4.Name = "comboTrancF4";
            this.comboTrancF4.Size = new Size(0x58, 20);
            this.comboTrancF4.TabIndex = 20;
            this.comboTrancF4.TabStop = false;
            this.comboTrancF4.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comboCommodityF4.Location = new Point(0x4c, 15);
            this.comboCommodityF4.Name = "comboCommodityF4";
            this.comboCommodityF4.Size = new Size(80, 20);
            this.comboCommodityF4.TabIndex = 0x13;
            this.comboCommodityF4.TabStop = false;
            this.comboCommodityF4.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.labelTrancF4.ImeMode = ImeMode.NoControl;
            this.labelTrancF4.Location = new Point(0xa4, 0x11);
            this.labelTrancF4.Name = "labelTrancF4";
            this.labelTrancF4.Size = new Size(0x48, 0x10);
            this.labelTrancF4.TabIndex = 0x12;
            this.labelTrancF4.Text = "交易代码：";
            this.labelTrancF4.TextAlign = ContentAlignment.MiddleCenter;
            this.labelCommodityF4.ImeMode = ImeMode.NoControl;
            this.labelCommodityF4.Location = new Point(4, 0x11);
            this.labelCommodityF4.Name = "labelCommodityF4";
            this.labelCommodityF4.Size = new Size(0x48, 0x10);
            this.labelCommodityF4.TabIndex = 0x11;
            this.labelCommodityF4.Text = "商品代码：";
            this.labelCommodityF4.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonSelF4.ImeMode = ImeMode.NoControl;
            this.buttonSelF4.Location = new Point(0x25c, 0x10);
            this.buttonSelF4.Name = "buttonSelF4";
            this.buttonSelF4.Size = new Size(0x39, 20);
            this.buttonSelF4.TabIndex = 14;
            this.buttonSelF4.TabStop = false;
            this.buttonSelF4.Text = "刷新";
            this.buttonSelF4.UseVisualStyleBackColor = true;
            this.buttonSelF4.Click += new EventHandler(this.buttonSelF4_Click);
            this.buttonHolding.BackColor = Color.White;
            this.buttonHolding.Dock = DockStyle.Left;
            this.buttonHolding.Enabled = false;
            this.buttonHolding.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.buttonHolding.ForeColor = SystemColors.ControlDarkDark;
            this.buttonHolding.Location = new Point(0, 0);
            this.buttonHolding.Name = "buttonHolding";
            this.buttonHolding.Size = new Size(0x1f, 200);
            this.buttonHolding.TabIndex = 0x17;
            this.buttonHolding.TabStop = false;
            this.buttonHolding.Text = "持\r\n仓";
            this.buttonHolding.UseVisualStyleBackColor = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxHoldingCollect);
            base.Controls.Add(this.buttonHolding);
            base.Name = "HoldingCollect";
            base.Size = new Size(700, 200);
            base.Load += new EventHandler(this.HoldingCollect_Load);
            this.groupBoxHoldingCollect.ResumeLayout(false);
            this.groupBoxHoldingCollect.PerformLayout();
            this.groupBoxF4_1.ResumeLayout(false);
            this.groupBoxF4_1.PerformLayout();
            ((ISupportInitialize)this.dgHoldingCollect).EndInit();
            base.ResumeLayout(false);
        }

        private string OrderSql()
        {
            string str = " 1=1 ";
            if (this.comboCommodityF4.SelectedIndex != 0)
            {
                str = str + " and CommodityID = '" + this.comboCommodityF4.Text + "'";
            }
            if (this.comboB_SF5.SelectedIndex == 1)
            {
                return (str + " and B_S='" + Global.BuySellStrArr[1] + "' ");
            }
            if (this.comboB_SF5.SelectedIndex == 2)
            {
                str = str + " and B_S='" + Global.BuySellStrArr[2] + "' ";
            }
            return str;
        }

        private void QueryConditionChanged(object sender, EventArgs e)
        {
            if (!this.isFirstLoad)
            {
                string sql = this.OrderSql();
                this.operationManager.queryHoldingOperation.HoldingScreen(sql);
            }
        }

        private void radioHdDetail_Click(object sender, EventArgs e)
        {
            this.radioHdCollect.Checked = true;
            base.Visible = false;
            this.operationManager.ShowHolding(1);
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboCommodityF4.Items.Clear();
            this.comboCommodityF4.Items.AddRange(commodityIDList.ToArray());
            this.comboCommodityF4.SelectedIndex = 0;
            this.isFirstLoad = false;
        }

        private void SetControlText()
        {
            this.groupBoxHoldingCollect.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF4");
            this.labelCommodityF4.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
            this.labelTrancF4.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
            this.buttonSelF4.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF4");
            this.buttonSelF4.TextAlign = ContentAlignment.TopCenter;
            this.labelB_SF5.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
            this.radioHdCollect.Text = Global.M_ResourceManager.GetString("TradeStr_RadioHdCollect");
            this.radioHdDetail.Text = Global.M_ResourceManager.GetString("TradeStr_RadioHdDetail");
            this.buttonSelF4.TextAlign = ContentAlignment.TopCenter;
        }

        private void SetDataGridViewHeader()
        {
            if (!this.isHoldingHeaderLoad)
            {
                for (int i = 0; i < this.dgHoldingCollect.Columns.Count; i++)
                {
                    ColItemInfo info = (ColItemInfo)this.holdingItemInfo.m_htItemInfo[this.dgHoldingCollect.Columns[i].Name];
                    if (info != null)
                    {
                        this.dgHoldingCollect.Columns[i].MinimumWidth = info.width;
                        this.dgHoldingCollect.Columns[i].FillWeight = info.width;
                        this.dgHoldingCollect.Columns[i].HeaderText = info.name;
                        this.dgHoldingCollect.Columns[i].DefaultCellStyle.Format = info.format;
                        this.dgHoldingCollect.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
                        if (info.sortID == 1)
                        {
                            this.dgHoldingCollect.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                        else
                        {
                            this.dgHoldingCollect.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (!this.holdingItemInfo.m_strItems.Contains(this.dgHoldingCollect.Columns[i].Name))
                        {
                            this.dgHoldingCollect.Columns[i].Visible = false;
                        }
                        if (i == 0)
                        {
                            this.dgHoldingCollect.Columns[i].ReadOnly = false;
                        }
                        else
                        {
                            this.dgHoldingCollect.Columns[i].ReadOnly = true;
                        }
                    }
                }
                this.isHoldingHeaderLoad = true;
            }
        }

        private void SetVisible()
        {
            base.Visible = true;
        }

        private delegate void FillHolding(DataTable dt);
    }
}
