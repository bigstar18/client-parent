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

    public class HoldingDetail : UserControl
    {
        private MyButton buttonSelF4;
        internal MyCombobox comboB_SF5;
        internal MyCombobox comboCommodityF4;
        internal MyCombobox comboTrancF4;
        private IContainer components;
        internal DataGridView dgHoldingDetail;
        private FillHoldingDetail fillHoldingDetail;
        private GroupBox groupBoxF4;
        private Panel groupBoxF4_1;
        private HoldingDetailItemInfo holdingDetailItemInfo = new HoldingDetailItemInfo();
        private bool isFirstLoad = true;
        private bool isHoldingDetailHeaderLoad;
        internal Label labelB_SF5;
        private Label labelCommodityF4;
        internal Label labelTrancF4;
        private OperationManager operationManager = OperationManager.GetInstance();
        internal MyRadioButton radioHdCollect;
        internal MyRadioButton radioHdDetail;
        private int Style;

        public HoldingDetail(int style)
        {
            this.InitializeComponent();
            this.Style = style;
            this.operationManager.queryHoldingDatailOperation.HoldingDetailFill = new QueryHoldingDetailOperation.HoldingDetailFillCallBack(this.HoldingDetailInfoFill);
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.operationManager.ShowHoldingDetail = new OperationManager.ShowHoldingControlCallBack(this.SetVisible);
            this.CreateHandle();
        }

        private void buttonSelF4_Click(object sender, EventArgs e)
        {
            this.operationManager.queryHoldingDatailOperation.ButtonRefreshFlag = 1;
            this.operationManager.queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
            this.operationManager.IdleRefreshButton = 0;
        }

        private void dgHoldingDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = false;
            if (e.RowIndex != -1)
            {
                if (this.dgHoldingDetail.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == this.operationManager.Total)
                {
                    Global.StatusInfoFill("", Global.RightColor, true);
                }
                string marketID = this.dgHoldingDetail["Market", e.RowIndex].Value.ToString().Trim();
                string commodityID = this.dgHoldingDetail["CommodityID", e.RowIndex].Value.ToString().Trim();
                if (Global.SetCommoditySelectIndex != null)
                {
                    flag = Global.SetCommoditySelectIndex(marketID, commodityID);
                }
                if (flag)
                {
                    if ((e.ColumnIndex == this.dgHoldingDetail.Columns["Price"].Index) || (e.ColumnIndex == this.dgHoldingDetail.Columns["B_S"].Index))
                    {
                        ServiceManage.GetInstance().CreateQueryCommData().QueryGNCommodityInfo(marketID, commodityID);
                        short buysell = 0;
                        short ordertype = 0;
                        double price = 0.0;
                        double lprice = 0.0;
                        int qty = 0;
                        string str3 = Global.M_ResourceManager.GetString("TradeStr_RadioB");
                        string str4 = Global.M_ResourceManager.GetString("TradeStr_RadioS");
                        if (this.dgHoldingDetail["B_S", e.RowIndex].Value.Equals(str3))
                        {
                            buysell = 1;
                        }
                        else if (this.dgHoldingDetail["B_S", e.RowIndex].Value.Equals(str4))
                        {
                            buysell = 0;
                        }
                        ordertype = 1;
                        lprice = Tools.StrToDouble(this.dgHoldingDetail["Price", e.RowIndex].Value.ToString(), 0.0);
                        qty = Tools.StrToInt(this.dgHoldingDetail["Cur_Open", e.RowIndex].Value.ToString(), 0);
                        Global.M_ResourceManager.GetString("TradeStr_MainFormF5_QuickOrderFailed");
                        if (Global.SetDoubleClickOrderInfo != null)
                        {
                            Global.SetDoubleClickOrderInfo(price, lprice, qty, buysell, ordertype);
                        }
                    }
                }
                else
                {
                    string message = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_NotExistGoods");
                    Global.StatusInfoFill(message, Global.ErrorColor, true);
                }
            }
        }

        private void dgHoldingDetail_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                this.operationManager.queryHoldingDatailOperation.HoldingDetailSort(this.dgHoldingDetail.Columns[e.ColumnIndex].Name.ToString());
            }
        }

        private void dgHoldingDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((this.dgHoldingDetail.RowCount > 1) && (this.dgHoldingDetail.Rows[this.dgHoldingDetail.RowCount - 1].Cells["CommodityID"].Value.ToString().Trim() == "合计"))
            {
                this.dgHoldingDetail.Rows[this.dgHoldingDetail.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
                this.dgHoldingDetail.Rows[this.dgHoldingDetail.RowCount - 1].ReadOnly = true;
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

        private void dsHoldingDetailFill(DataTable dt)
        {
            this.dgHoldingDetail.DataSource = dt;
            this.SetDataGridViewHeader();
        }

        public void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void HoldingDetail_Load(object sender, EventArgs e)
        {
            if (this.Style == 1)
            {
                this.groupBoxF4_1.Visible = false;
            }
            else
            {
                this.groupBoxF4_1.Visible = true;
            }
            this.SetControlText();
            this.QueryHoldingDetailInfoLoad();
        }

        private void HoldingDetailInfoFill(DataTable dTable)
        {
            try
            {
                this.fillHoldingDetail = new FillHoldingDetail(this.dsHoldingDetailFill);
                this.HandleCreated();
                base.Invoke(this.fillHoldingDetail, new object[] { dTable });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private string HoldingDetailSql()
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

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.groupBoxF4 = new GroupBox();
            this.groupBoxF4_1 = new Panel();
            this.radioHdCollect = new MyRadioButton();
            this.radioHdDetail = new MyRadioButton();
            this.dgHoldingDetail = new DataGridView();
            this.labelB_SF5 = new Label();
            this.comboB_SF5 = new MyCombobox();
            this.comboTrancF4 = new MyCombobox();
            this.comboCommodityF4 = new MyCombobox();
            this.labelTrancF4 = new Label();
            this.labelCommodityF4 = new Label();
            this.buttonSelF4 = new MyButton();
            this.groupBoxF4.SuspendLayout();
            this.groupBoxF4_1.SuspendLayout();
            ((ISupportInitialize)this.dgHoldingDetail).BeginInit();
            base.SuspendLayout();
            this.groupBoxF4.Controls.Add(this.groupBoxF4_1);
            this.groupBoxF4.Controls.Add(this.dgHoldingDetail);
            this.groupBoxF4.Controls.Add(this.labelB_SF5);
            this.groupBoxF4.Controls.Add(this.comboB_SF5);
            this.groupBoxF4.Controls.Add(this.comboTrancF4);
            this.groupBoxF4.Controls.Add(this.comboCommodityF4);
            this.groupBoxF4.Controls.Add(this.labelTrancF4);
            this.groupBoxF4.Controls.Add(this.labelCommodityF4);
            this.groupBoxF4.Controls.Add(this.buttonSelF4);
            this.groupBoxF4.Dock = DockStyle.Fill;
            this.groupBoxF4.Location = new Point(0, 0);
            this.groupBoxF4.Name = "groupBoxF4";
            this.groupBoxF4.Size = new Size(0x29d, 200);
            this.groupBoxF4.TabIndex = 0x16;
            this.groupBoxF4.TabStop = false;
            this.groupBoxF4.Text = "订货明细";
            this.groupBoxF4_1.Controls.Add(this.radioHdCollect);
            this.groupBoxF4_1.Controls.Add(this.radioHdDetail);
            this.groupBoxF4_1.Location = new Point(0x1bd, 12);
            this.groupBoxF4_1.Name = "groupBoxF4_1";
            this.groupBoxF4_1.Size = new Size(0x9a, 0x19);
            this.groupBoxF4_1.TabIndex = 0x1c;
            this.radioHdCollect.AutoSize = true;
            this.radioHdCollect.ImeMode = ImeMode.NoControl;
            this.radioHdCollect.Location = new Point(3, 4);
            this.radioHdCollect.Name = "radioHdCollect";
            this.radioHdCollect.Size = new Size(0x47, 0x10);
            this.radioHdCollect.TabIndex = 0x19;
            this.radioHdCollect.Text = "订货汇总";
            this.radioHdCollect.UseVisualStyleBackColor = true;
            this.radioHdCollect.Click += new EventHandler(this.radioHdCollect_Click);
            this.radioHdDetail.AutoSize = true;
            this.radioHdDetail.Checked = true;
            this.radioHdDetail.ImeMode = ImeMode.NoControl;
            this.radioHdDetail.Location = new Point(80, 4);
            this.radioHdDetail.Name = "radioHdDetail";
            this.radioHdDetail.Size = new Size(0x47, 0x10);
            this.radioHdDetail.TabIndex = 0x1a;
            this.radioHdDetail.TabStop = true;
            this.radioHdDetail.Text = "订货明细";
            this.radioHdDetail.UseVisualStyleBackColor = true;
            this.dgHoldingDetail.AllowUserToAddRows = false;
            this.dgHoldingDetail.AllowUserToDeleteRows = false;
            this.dgHoldingDetail.AllowUserToOrderColumns = true;
            this.dgHoldingDetail.AllowUserToResizeRows = false;
            this.dgHoldingDetail.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgHoldingDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgHoldingDetail.ColumnHeadersDefaultCellStyle = style;
            this.dgHoldingDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dgHoldingDetail.DefaultCellStyle = style2;
            this.dgHoldingDetail.Location = new Point(4, 0x29);
            this.dgHoldingDetail.Name = "dgHoldingDetail";
            this.dgHoldingDetail.ReadOnly = true;
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dgHoldingDetail.RowHeadersDefaultCellStyle = style3;
            this.dgHoldingDetail.RowHeadersVisible = false;
            this.dgHoldingDetail.RowTemplate.Height = 20;
            this.dgHoldingDetail.Size = new Size(660, 0x9a);
            this.dgHoldingDetail.TabIndex = 0x16;
            this.dgHoldingDetail.TabStop = false;
            this.dgHoldingDetail.CellDoubleClick += new DataGridViewCellEventHandler(this.dgHoldingDetail_CellDoubleClick);
            this.dgHoldingDetail.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgHoldingDetail_ColumnHeaderMouseClick);
            this.dgHoldingDetail.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgHoldingDetail_DataBindingComplete);
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
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxF4);
            base.Name = "HoldingDetail";
            base.Size = new Size(0x29d, 200);
            base.Load += new EventHandler(this.HoldingDetail_Load);
            this.groupBoxF4.ResumeLayout(false);
            this.groupBoxF4.PerformLayout();
            this.groupBoxF4_1.ResumeLayout(false);
            this.groupBoxF4_1.PerformLayout();
            ((ISupportInitialize)this.dgHoldingDetail).EndInit();
            base.ResumeLayout(false);
        }

        private void QueryConditionChanged(object sender, EventArgs e)
        {
            if (!this.isFirstLoad)
            {
                string sql = this.HoldingDetailSql();
                this.operationManager.queryHoldingDatailOperation.HoldingDetailScreen(sql);
            }
        }

        public void QueryHoldingDetailInfoLoad()
        {
            this.comboB_SF5.Items.Clear();
            this.comboB_SF5.Items.Add(this.operationManager.StrAll);
            this.comboB_SF5.Items.Add(this.operationManager.StrBuy);
            this.comboB_SF5.Items.Add(this.operationManager.StrSale);
            this.comboB_SF5.SelectedIndex = 0;
            if (Global.CustomerCount < 2)
            {
                this.comboTrancF4.Visible = false;
                this.labelTrancF4.Visible = false;
                this.labelB_SF5.Location = new Point(this.labelB_SF5.Location.X - 150, this.labelB_SF5.Location.Y);
                this.comboB_SF5.Location = new Point(this.comboB_SF5.Location.X - 150, this.comboB_SF5.Location.Y);
            }
        }

        private void radioHdCollect_Click(object sender, EventArgs e)
        {
            this.radioHdDetail.Checked = true;
            base.Visible = false;
            this.operationManager.ShowHolding(0);
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
            this.groupBoxF4.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF4");
            this.labelCommodityF4.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
            this.labelTrancF4.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
            this.buttonSelF4.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF4");
            this.labelB_SF5.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
            this.buttonSelF4.TextAlign = ContentAlignment.TopCenter;
        }

        private void SetDataGridViewHeader()
        {
            if (!this.isHoldingDetailHeaderLoad)
            {
                for (int i = 0; i < this.dgHoldingDetail.Columns.Count; i++)
                {
                    ColItemInfo info = (ColItemInfo)this.holdingDetailItemInfo.m_htItemInfo[this.dgHoldingDetail.Columns[i].Name];
                    if (info != null)
                    {
                        this.dgHoldingDetail.Columns[i].MinimumWidth = info.width;
                        this.dgHoldingDetail.Columns[i].FillWeight = info.width;
                        this.dgHoldingDetail.Columns[i].HeaderText = info.name;
                        this.dgHoldingDetail.Columns[i].DefaultCellStyle.Format = info.format;
                        this.dgHoldingDetail.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
                        if (info.sortID == 1)
                        {
                            this.dgHoldingDetail.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                        else
                        {
                            this.dgHoldingDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (!this.holdingDetailItemInfo.m_strItems.Contains(this.dgHoldingDetail.Columns[i].Name))
                        {
                            this.dgHoldingDetail.Columns[i].Visible = false;
                        }
                        if (i == 0)
                        {
                            this.dgHoldingDetail.Columns[i].ReadOnly = false;
                        }
                        else
                        {
                            this.dgHoldingDetail.Columns[i].ReadOnly = true;
                        }
                    }
                }
                this.isHoldingDetailHeaderLoad = true;
            }
        }

        private void SetVisible()
        {
            base.Visible = true;
        }

        private delegate void FillHoldingDetail(DataTable dt);
    }
}
