namespace FuturesTrade.Gnnt.UI.Modules.Query
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
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
    using TPME.Log;

    public class PreDelegate : UserControl
    {
        private MyButton buttonDel;
        public MyButton buttonOrder6;
        private MyButton buttonSel;
        private MyCombobox comCommodity;
        private IContainer components;
        internal MyCombobox comTranc;
        internal DataGridView dgPreDelegate;
        private FillPreOrder fillPreOrder;
        private GroupBox groupBoxF7;
        private string[] idColumns;
        private bool isPreDelegateHeaderLoad;
        private Label labelCommodityF6;
        internal Label labelTrancF6;
        private OperationManager operationManager = OperationManager.GetInstance();
        private PreOrderItemInfo preOrderItemInfo = new PreOrderItemInfo();
        private MyButton selAll;
        private DataGridViewCheckBoxColumn SelectFlag;

        public PreDelegate()
        {
            this.InitializeComponent();
            this.operationManager.queryPredelegateOperation.PreDelegateFill = new QueryPredelegateOperation.PreDelegateFillCallBack(this.PreOrderInfoFill);
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.CreateHandle();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            this.operationManager.queryPredelegateOperation.DeletePreDelegate(this.dgPreDelegate);
        }

        private void buttonOrder6_Click(object sender, EventArgs e)
        {
            this.operationManager.queryPredelegateOperation.PreDelegateOrder(this.dgPreDelegate);
        }

        private void Delegate_Load(object sender, EventArgs e)
        {
            this.SetControlText();
        }

        private void dgPreDelegate_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                string sortName = this.dgPreDelegate.Columns[e.ColumnIndex].Name.ToString();
                this.operationManager.queryPredelegateOperation.PredelegateSort(sortName);
            }
        }

        private void dgPreDelegate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((this.dgPreDelegate.RowCount > 1) && (this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].Cells["TransactionsCode"].Value.ToString().Trim() == "合计"))
            {
                this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
                this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].ReadOnly = true;
            }
            Global.BSAlign(this.dgPreDelegate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dsPreDelegateFill(DataTable dTable)
        {
            this.dgPreDelegate.DataSource = dTable;
            this.SetDataGridViewHeader();
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
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.groupBoxF7 = new GroupBox();
            this.dgPreDelegate = new DataGridView();
            this.SelectFlag = new DataGridViewCheckBoxColumn();
            this.buttonDel = new MyButton();
            this.buttonOrder6 = new MyButton();
            this.selAll = new MyButton();
            this.buttonSel = new MyButton();
            this.comTranc = new MyCombobox();
            this.comCommodity = new MyCombobox();
            this.labelTrancF6 = new Label();
            this.labelCommodityF6 = new Label();
            this.groupBoxF7.SuspendLayout();
            ((ISupportInitialize)this.dgPreDelegate).BeginInit();
            base.SuspendLayout();
            this.groupBoxF7.Controls.Add(this.dgPreDelegate);
            this.groupBoxF7.Controls.Add(this.buttonDel);
            this.groupBoxF7.Controls.Add(this.buttonOrder6);
            this.groupBoxF7.Controls.Add(this.selAll);
            this.groupBoxF7.Controls.Add(this.buttonSel);
            this.groupBoxF7.Controls.Add(this.comTranc);
            this.groupBoxF7.Controls.Add(this.comCommodity);
            this.groupBoxF7.Controls.Add(this.labelTrancF6);
            this.groupBoxF7.Controls.Add(this.labelCommodityF6);
            this.groupBoxF7.Dock = DockStyle.Fill;
            this.groupBoxF7.Location = new Point(0, 0);
            this.groupBoxF7.Margin = new Padding(0);
            this.groupBoxF7.Name = "groupBoxF7";
            this.groupBoxF7.Size = new Size(700, 200);
            this.groupBoxF7.TabIndex = 0x16;
            this.groupBoxF7.TabStop = false;
            this.groupBoxF7.Text = "预埋查询";
            this.groupBoxF7.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxF7.ForeColor = Color.FromArgb(235, 235, 235);
            this.dgPreDelegate.AllowUserToAddRows = false;
            this.dgPreDelegate.AllowUserToDeleteRows = false;
            this.dgPreDelegate.AllowUserToOrderColumns = true;
            this.dgPreDelegate.AllowUserToResizeRows = false;
            this.dgPreDelegate.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgPreDelegate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgPreDelegate.ColumnHeadersDefaultCellStyle = style;
            this.dgPreDelegate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgPreDelegate.Columns.AddRange(new DataGridViewColumn[] { this.SelectFlag });
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dgPreDelegate.DefaultCellStyle = style2;
            this.dgPreDelegate.Location = new Point(4, 0x29);
            this.dgPreDelegate.Name = "dgPreDelegate";
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dgPreDelegate.RowHeadersDefaultCellStyle = style3;
            this.dgPreDelegate.RowHeadersVisible = false;
            this.dgPreDelegate.RowTemplate.Height = 0x12;
            this.dgPreDelegate.Size = new Size(0x2b4, 0x9a);
            this.dgPreDelegate.TabIndex = 9;
            this.dgPreDelegate.TabStop = false;
            this.dgPreDelegate.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgPreDelegate_ColumnHeaderMouseClick);
            this.dgPreDelegate.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgPreDelegate_DataBindingComplete);
            this.SelectFlag.HeaderText = "";
            this.SelectFlag.Name = "SelectFlag";
            this.buttonDel.ImeMode = ImeMode.NoControl;
            this.buttonDel.Location = new Point(0x21b, 0x10);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new Size(0x35, 20);
            this.buttonDel.TabIndex = 8;
            this.buttonDel.Text = "删除";
            this.buttonDel.Click += new EventHandler(this.buttonDel_Click);
            this.buttonOrder6.ImeMode = ImeMode.NoControl;
            this.buttonOrder6.Location = new Point(0x1dd, 0x10);
            this.buttonOrder6.Name = "buttonOrder6";
            this.buttonOrder6.Size = new Size(0x35, 20);
            this.buttonOrder6.TabIndex = 7;
            this.buttonOrder6.Text = "委托";
            this.buttonOrder6.Click += new EventHandler(this.buttonOrder6_Click);
            this.selAll.ImeMode = ImeMode.NoControl;
            this.selAll.Location = new Point(0x1a2, 0x10);
            this.selAll.Name = "selAll";
            this.selAll.Size = new Size(0x35, 20);
            this.selAll.TabIndex = 6;
            this.selAll.Text = "全选";
            this.selAll.Click += new EventHandler(this.selAll_Click);
            this.buttonSel.ImeMode = ImeMode.NoControl;
            this.buttonSel.Location = new Point(0x164, 0x10);
            this.buttonSel.Name = "buttonSel";
            this.buttonSel.Size = new Size(0x35, 20);
            this.buttonSel.TabIndex = 5;
            this.buttonSel.Text = "查询";
            this.buttonSel.Click += new EventHandler(this.QueryConditionChanged);
            this.comTranc.Location = new Point(0xec, 15);
            this.comTranc.Name = "comTranc";
            this.comTranc.Size = new Size(0x58, 20);
            this.comTranc.TabIndex = 4;
            this.comTranc.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comCommodity.Location = new Point(0x4c, 15);
            this.comCommodity.Name = "comCommodity";
            this.comCommodity.Size = new Size(80, 20);
            this.comCommodity.TabIndex = 3;
            this.comCommodity.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.labelTrancF6.ImeMode = ImeMode.NoControl;
            this.labelTrancF6.Location = new Point(0xa4, 0x11);
            this.labelTrancF6.Name = "labelTrancF6";
            this.labelTrancF6.Size = new Size(0x48, 0x12);
            this.labelTrancF6.TabIndex = 2;
            this.labelTrancF6.Text = "交易代码：";
            this.labelTrancF6.TextAlign = ContentAlignment.MiddleCenter;
            this.labelCommodityF6.ImeMode = ImeMode.NoControl;
            this.labelCommodityF6.Location = new Point(4, 0x11);
            this.labelCommodityF6.Name = "labelCommodityF6";
            this.labelCommodityF6.Size = new Size(0x48, 0x10);
            this.labelCommodityF6.TabIndex = 1;
            this.labelCommodityF6.Text = "商品代码：";
            this.labelCommodityF6.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxF7);
            base.Name = "PreDelegate";
            base.Size = new Size(700, 200);
            base.Load += new EventHandler(this.Delegate_Load);
            this.groupBoxF7.ResumeLayout(false);
            ((ISupportInitialize)this.dgPreDelegate).EndInit();
            base.ResumeLayout(false);
        }

        private string PreDelegateSql()
        {
            string str = " 1=1 ";
            if (this.comTranc.Visible && (this.comTranc.SelectedIndex > 0))
            {
                str = str + " and TransactionsCode= '" + this.comTranc.Text + "'";
            }
            if (this.comCommodity.SelectedIndex != 0)
            {
                str = str + " and  CommodityCode = '" + this.comCommodity.Text + "'";
            }
            return str;
        }

        private void PreOrderInfoFill(DataTable dTable)
        {
            try
            {
                this.fillPreOrder = new FillPreOrder(this.dsPreDelegateFill);
                this.HandleCreated();
                base.Invoke(this.fillPreOrder, new object[] { dTable });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void QueryConditionChanged(object sender, EventArgs e)
        {
            string sql = this.PreDelegateSql();
            if ((sender != null) && (sender is MyRadioButton))
            {
                MyRadioButton button = (MyRadioButton)sender;
                if (!button.Checked)
                {
                    return;
                }
            }
            this.operationManager.queryPredelegateOperation.PreDelegateScreen(sql);
        }

        private void selAll_Click(object sender, EventArgs e)
        {
            if (this.selAll.Text.Equals(this.operationManager.AllCheck))
            {
                for (int i = 0; i < this.dgPreDelegate.Rows.Count; i++)
                {
                    if (!(this.dgPreDelegate.Rows[i].Cells["TransactionsCode"].Value.ToString() == this.operationManager.Total))
                    {
                        this.dgPreDelegate.Rows[i].Cells[0].Value = true;
                    }
                }
                this.selAll.Text = this.operationManager.AllNotCheck;
            }
            else
            {
                for (int j = 0; j < this.dgPreDelegate.Rows.Count; j++)
                {
                    this.dgPreDelegate.Rows[j].Cells[0].Value = false;
                }
                this.selAll.Text = this.operationManager.AllCheck;
            }
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comCommodity.Items.Clear();
            this.comCommodity.Items.AddRange(commodityIDList.ToArray());
            this.comCommodity.SelectedIndex = 0;
        }

        private void SetControlText()
        {
            this.groupBoxF7.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF7");
            this.labelCommodityF6.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
            this.labelTrancF6.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
            this.buttonSel.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSel");
            this.selAll.Text = Global.M_ResourceManager.GetString("TradeStr_SelAll");
            this.buttonOrder6.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonOrder6");
            this.buttonDel.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonDel");
            this.buttonSel.TextAlign = ContentAlignment.TopCenter;
            this.buttonOrder6.TextAlign = ContentAlignment.TopCenter;
            this.buttonDel.TextAlign = ContentAlignment.TopCenter;
            this.comTranc.Items.Add(this.operationManager.StrAll);
            this.comTranc.SelectedIndex = 0;
            if (Global.CustomerCount < 2)
            {
                this.labelTrancF6.Visible = false;
                this.comTranc.Visible = false;
            }
        }

        private void SetDataGridViewHeader()
        {
            if (!this.isPreDelegateHeaderLoad)
            {
                for (int i = 0; i < this.dgPreDelegate.Columns.Count; i++)
                {
                    ColItemInfo info = (ColItemInfo)this.preOrderItemInfo.m_htItemInfo[this.dgPreDelegate.Columns[i].Name];
                    if (info != null)
                    {
                        this.dgPreDelegate.Columns[i].MinimumWidth = info.width;
                        this.dgPreDelegate.Columns[i].FillWeight = info.width;
                        this.dgPreDelegate.Columns[i].HeaderText = info.name;
                        this.dgPreDelegate.Columns[i].DefaultCellStyle.Format = info.format;
                        this.dgPreDelegate.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
                        if (info.sortID == 1)
                        {
                            this.dgPreDelegate.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                        else
                        {
                            this.dgPreDelegate.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (!this.preOrderItemInfo.m_strItems.Contains(this.dgPreDelegate.Columns[i].Name + ";"))
                        {
                            this.dgPreDelegate.Columns[i].Visible = false;
                        }
                        if (i == 0)
                        {
                            this.dgPreDelegate.Columns[i].ReadOnly = false;
                        }
                        else
                        {
                            this.dgPreDelegate.Columns[i].ReadOnly = true;
                        }
                    }
                }
                this.isPreDelegateHeaderLoad = true;
            }
        }

        private delegate void FillPreOrder(DataTable dt);
    }
}
