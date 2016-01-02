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

    public class ConditionOrderQuery : UserControl
    {
        private BindingNavigator bindNavConOrder;
        private int bindNavHeight;
        private MyButton BtnRefresh;
        private MyButton btnRevoke;
        private CheckBox checkConditionAll;
        private MyCombobox comboBSQuery;
        internal MyCombobox comboCommodityQuery;
        private MyCombobox comboConTypeQuery;
        private MyCombobox comboOLQuery;
        private MyCombobox comboState;
        private IContainer components;
        private ConditionOrderItemInfo conditionOrderItemInfo = new ConditionOrderItemInfo();
        internal DataGridView dgAllConditionOrder;
        private int dgConditionOrderHeight;
        private FillConditionOrder fillConditionOrder;
        private GroupBox groupBoxConOrder;
        private bool isFirstLoad = true;
        private bool isHeaderLoad;
        private Label labBS2;
        private Label labComCode3;
        private Label labOL2;
        private Label labState;
        private Label labType2;
        private OperationManager operationManager = OperationManager.GetInstance();
        private DataGridViewCheckBoxColumn SelectAll;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton tSBtnConOrderFirst;
        private ToolStripButton tSBtnConOrderGO;
        private ToolStripButton tSBtnConOrderLast;
        private ToolStripButton tSBtnConOrderNext;
        private ToolStripLabel tSBtnConOrderNum;
        private ToolStripButton tSBtnConOrderPrevious;
        private ToolStripLabel tSLblConOrderP;
        private ToolStripLabel tSLblConOrderPage;
        private ToolStripTextBox tSTbxConOrderCurP;

        public ConditionOrderQuery()
        {
            this.InitializeComponent();
            this.operationManager.queryConOrderOperation.ConOrderFill = new QueryConOrderOperation.ConOrderFillCallBack(this.ConditionOrderInfoFill);
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.CreateHandle();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.operationManager.queryConOrderOperation.ButtonRefreshFlag = 1;
            this.operationManager.queryConOrderOperation.QueryConOrderInfoLoad();
            this.operationManager.IdleRefreshButton = 0;
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            MessageForm form;
            List<string> orderNoList = new List<string>();
            string str = string.Empty;
            string str2 = string.Empty;
            for (int i = this.dgAllConditionOrder.Rows.Count - 1; i >= 0; i--)
            {
                if ((this.dgAllConditionOrder["SelectAll", i].Value != null) && ((bool)this.dgAllConditionOrder["SelectAll", i].Value))
                {
                    if (this.dgAllConditionOrder.Rows[i].Cells["OrderState"].Value.Equals(Global.OrderTypeStrArr[1]))
                    {
                        string item = this.dgAllConditionOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
                        str = str + "-" + item;
                        orderNoList.Add(item);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + ",";
                        }
                        str2 = str2 + this.dgAllConditionOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
                    }
                }
            }
            if (!str2.Equals(""))
            {
                string format = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_ErrorOrderStr");
                string formName = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_ErrorOrderMessage");
                string message = string.Format(format, str2);
                Logger.wirte(MsgType.Error, "条件下单委托单号:【" + str2 + "】不可撤销");
                form = new MessageForm(formName, message, 1)
                {
                    Owner = base.ParentForm
                };
                form.ShowDialog();
                form.Dispose();
            }
            else if (orderNoList.Count > 0)
            {
                int count = orderNoList.Count;
                string str7 = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrders");
                string str8 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_RevokeOrdersMessage");
                form = new MessageForm(str7, string.Format(str8, count), 0)
                {
                    Owner = base.ParentForm
                };
                form.ShowDialog();
                form.Dispose();
                if (form.isOK)
                {
                    Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_DataSubmiting");
                    this.operationManager.revokeConOrderOperation.RevokeConOrderThread(orderNoList);
                    this.checkConditionAll.Checked = false;
                }
            }
            else
            {
                string str9 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_CancelMessageContent");
                form = new MessageForm(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_CancelMessageTitle"), str9, 1);
                WriteLog.WriteMsg("请至少选择一笔条件下单委托后撤单！");
                form.Owner = base.ParentForm;
                form.ShowDialog();
                form.Dispose();
            }
        }

        private void checkConditionAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkConditionAll.Checked)
            {
                for (int i = 0; i < this.dgAllConditionOrder.Rows.Count; i++)
                {
                    if (this.dgAllConditionOrder.Rows[i].Cells["OrderState"].Value.Equals(Global.OrderTypeStrArr[1]))
                    {
                        this.dgAllConditionOrder.Rows[i].Cells[0].Value = true;
                    }
                    else
                    {
                        this.dgAllConditionOrder.Rows[i].Cells[0].Value = false;
                    }
                }
            }
            else
            {
                for (int j = 0; j < this.dgAllConditionOrder.Rows.Count; j++)
                {
                    if (this.dgAllConditionOrder.Rows[j].Cells["OrderState"].Value.Equals(Global.OrderTypeStrArr[1]))
                    {
                        this.dgAllConditionOrder.Rows[j].Cells[0].Value = false;
                    }
                    else
                    {
                        this.dgAllConditionOrder.Rows[j].Cells[0].Value = false;
                    }
                }
            }
        }

        private void CommodityInfoLoad()
        {
        }

        private void ConditionOrder_Load(object sender, EventArgs e)
        {
            this.CommodityInfoLoad();
            this.SetControlText();
        }

        private void ConditionOrderInfoFill(DataTable dTable, bool isPaging)
        {
            this.fillConditionOrder = new FillConditionOrder(this.dsConOrderFill);
            this.HandleCreated();
            base.Invoke(this.fillConditionOrder, new object[] { dTable });
        }

        private void ConditionOrderQuery_SizeChanged(object sender, EventArgs e)
        {
            this.dgConditionOrderHeight = this.dgAllConditionOrder.Height;
            this.bindNavHeight = this.bindNavConOrder.Height;
        }

        private void ConditionOrderSetEnable(bool isEnable)
        {
            if (this.operationManager.queryConOrderOperation.ConOrderCurrentPage == this.operationManager.queryConOrderOperation.ConOrderAllPage)
            {
                this.tSBtnConOrderFirst.Enabled = isEnable;
                this.tSBtnConOrderPrevious.Enabled = isEnable;
                this.tSBtnConOrderNext.Enabled = !isEnable;
                this.tSBtnConOrderLast.Enabled = !isEnable;
            }
            else if (this.operationManager.queryConOrderOperation.ConOrderCurrentPage == 1)
            {
                this.tSBtnConOrderFirst.Enabled = !isEnable;
                this.tSBtnConOrderPrevious.Enabled = !isEnable;
                this.tSBtnConOrderNext.Enabled = isEnable;
                this.tSBtnConOrderLast.Enabled = isEnable;
            }
            else
            {
                this.tSBtnConOrderFirst.Enabled = isEnable;
                this.tSBtnConOrderPrevious.Enabled = isEnable;
                this.tSBtnConOrderNext.Enabled = isEnable;
                this.tSBtnConOrderLast.Enabled = isEnable;
            }
        }

        private void dgAllConditionOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                this.operationManager.queryConOrderOperation.ConOrderDataGridViewSort(this.dgAllConditionOrder.Columns[e.ColumnIndex].Name.ToString());
            }
        }

        private void dgAllConditionOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string str = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
            if ((this.dgAllConditionOrder.RowCount > 1) && (this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].Cells["commodityID"].Value.ToString().Trim() == str))
            {
                this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
                this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].ReadOnly = true;
            }
            try
            {
                this.dgAllConditionOrder.Columns["AutoID"].Visible = false;
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.Message);
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

        private void dsConOrderFill(DataTable dt)
        {
            this.dgAllConditionOrder.DataSource = dt;
            this.SetDataGridViewHeader();
            this.SetBindNavLayOut();
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ConditionOrderQuery));
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            this.groupBoxConOrder = new GroupBox();
            this.checkConditionAll = new CheckBox();
            this.BtnRefresh = new MyButton();
            this.btnRevoke = new MyButton();
            this.labComCode3 = new Label();
            this.labType2 = new Label();
            this.labState = new Label();
            this.labBS2 = new Label();
            this.comboBSQuery = new MyCombobox();
            this.labOL2 = new Label();
            this.comboCommodityQuery = new MyCombobox();
            this.comboOLQuery = new MyCombobox();
            this.comboState = new MyCombobox();
            this.comboConTypeQuery = new MyCombobox();
            this.bindNavConOrder = new BindingNavigator(this.components);
            this.tSBtnConOrderFirst = new ToolStripButton();
            this.tSBtnConOrderPrevious = new ToolStripButton();
            this.toolStripLabel1 = new ToolStripLabel();
            this.tSLblConOrderPage = new ToolStripLabel();
            this.tSBtnConOrderNext = new ToolStripButton();
            this.tSBtnConOrderLast = new ToolStripButton();
            this.tSBtnConOrderNum = new ToolStripLabel();
            this.tSTbxConOrderCurP = new ToolStripTextBox();
            this.tSLblConOrderP = new ToolStripLabel();
            this.tSBtnConOrderGO = new ToolStripButton();
            this.dgAllConditionOrder = new DataGridView();
            this.SelectAll = new DataGridViewCheckBoxColumn();
            this.groupBoxConOrder.SuspendLayout();
            this.bindNavConOrder.BeginInit();
            this.bindNavConOrder.SuspendLayout();
            ((ISupportInitialize)this.dgAllConditionOrder).BeginInit();
            base.SuspendLayout();
            this.groupBoxConOrder.Controls.Add(this.checkConditionAll);
            this.groupBoxConOrder.Controls.Add(this.BtnRefresh);
            this.groupBoxConOrder.Controls.Add(this.btnRevoke);
            this.groupBoxConOrder.Controls.Add(this.labComCode3);
            this.groupBoxConOrder.Controls.Add(this.labType2);
            this.groupBoxConOrder.Controls.Add(this.labState);
            this.groupBoxConOrder.Controls.Add(this.labBS2);
            this.groupBoxConOrder.Controls.Add(this.comboBSQuery);
            this.groupBoxConOrder.Controls.Add(this.labOL2);
            this.groupBoxConOrder.Controls.Add(this.comboCommodityQuery);
            this.groupBoxConOrder.Controls.Add(this.comboOLQuery);
            this.groupBoxConOrder.Controls.Add(this.comboState);
            this.groupBoxConOrder.Controls.Add(this.comboConTypeQuery);
            this.groupBoxConOrder.Controls.Add(this.bindNavConOrder);
            this.groupBoxConOrder.Controls.Add(this.dgAllConditionOrder);
            this.groupBoxConOrder.Dock = DockStyle.Fill;
            this.groupBoxConOrder.Font = new Font("宋体", 9f);
            this.groupBoxConOrder.Location = new Point(0, 0);
            this.groupBoxConOrder.Margin = new Padding(0);
            this.groupBoxConOrder.Name = "groupBoxConOrder";
            this.groupBoxConOrder.Size = new Size(820, 0x142);
            this.groupBoxConOrder.TabIndex = 5;
            this.groupBoxConOrder.TabStop = false;
            this.groupBoxConOrder.Text = "查询条件下单";
            this.groupBoxConOrder.BackColor = Color.FromArgb(235,235,235); ;
            //this.groupBoxConOrder.ForeColor = Color.Black;
            this.checkConditionAll.BackColor = Color.Transparent;
            this.checkConditionAll.ImeMode = ImeMode.NoControl;
            this.checkConditionAll.Location = new Point(12, 0x33);
            this.checkConditionAll.Name = "checkConditionAll";
            this.checkConditionAll.Size = new Size(14, 0x10);
            this.checkConditionAll.TabIndex = 0;
            this.checkConditionAll.UseVisualStyleBackColor = false;
            this.checkConditionAll.CheckedChanged += new EventHandler(this.checkConditionAll_CheckedChanged);
            this.BtnRefresh.ImeMode = ImeMode.NoControl;
            this.BtnRefresh.Location = new Point(0x2e8, 15);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new Size(50, 0x17);
            this.BtnRefresh.TabIndex = 0x25;
            this.BtnRefresh.Text = "刷新";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
            this.btnRevoke.ImeMode = ImeMode.NoControl;
            this.btnRevoke.Location = new Point(0x299, 15);
            this.btnRevoke.Name = "btnRevoke";
            this.btnRevoke.Size = new Size(0x4b, 0x17);
            this.btnRevoke.TabIndex = 0x12;
            this.btnRevoke.Text = "撤所选单";
            this.btnRevoke.UseVisualStyleBackColor = true;
            this.btnRevoke.Click += new EventHandler(this.btnRevoke_Click);
            this.labComCode3.AutoSize = true;
            this.labComCode3.ImeMode = ImeMode.NoControl;
            this.labComCode3.Location = new Point(8, 0x15);
            this.labComCode3.Name = "labComCode3";
            this.labComCode3.Size = new Size(0x35, 12);
            this.labComCode3.TabIndex = 0;
            this.labComCode3.Text = "品种代码";
            this.labType2.AutoSize = true;
            this.labType2.ImeMode = ImeMode.NoControl;
            this.labType2.Location = new Point(0x162, 0x15);
            this.labType2.Name = "labType2";
            this.labType2.Size = new Size(0x41, 12);
            this.labType2.TabIndex = 0;
            this.labType2.Text = "条件类型：";
            this.labState.AutoSize = true;
            this.labState.ImeMode = ImeMode.NoControl;
            this.labState.Location = new Point(0x1fd, 0x15);
            this.labState.Name = "labState";
            this.labState.Size = new Size(0x41, 12);
            this.labState.TabIndex = 0;
            this.labState.Text = "委托状态：";
            this.labBS2.AutoSize = true;
            this.labBS2.ImeMode = ImeMode.NoControl;
            this.labBS2.Location = new Point(150, 0x15);
            this.labBS2.Name = "labBS2";
            this.labBS2.Size = new Size(0x1d, 12);
            this.labBS2.TabIndex = 0;
            this.labBS2.Text = "买卖";
            this.comboBSQuery.FormattingEnabled = true;
            this.comboBSQuery.Items.AddRange(new object[] { "全部", "买入", "卖出" });
            this.comboBSQuery.Location = new Point(0xb8, 0x11);
            this.comboBSQuery.Name = "comboBSQuery";
            this.comboBSQuery.Size = new Size(0x3d, 20);
            this.comboBSQuery.TabIndex = 13;
            this.comboBSQuery.Text = "全部";
            this.comboBSQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.labOL2.AutoSize = true;
            this.labOL2.ImeMode = ImeMode.NoControl;
            this.labOL2.Location = new Point(250, 0x15);
            this.labOL2.Name = "labOL2";
            this.labOL2.Size = new Size(0x1d, 12);
            this.labOL2.TabIndex = 0;
            this.labOL2.Text = "订转";
            this.comboCommodityQuery.FormattingEnabled = true;
            this.comboCommodityQuery.Location = new Point(0x41, 0x11);
            this.comboCommodityQuery.Name = "comboCommodityQuery";
            this.comboCommodityQuery.Size = new Size(80, 20);
            this.comboCommodityQuery.TabIndex = 12;
            this.comboCommodityQuery.Text = "全部";
            this.comboCommodityQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comboOLQuery.FormattingEnabled = true;
            this.comboOLQuery.Items.AddRange(new object[] { "全部", "订立", "转让" });
            this.comboOLQuery.Location = new Point(0x11e, 0x11);
            this.comboOLQuery.Name = "comboOLQuery";
            this.comboOLQuery.Size = new Size(0x3d, 20);
            this.comboOLQuery.TabIndex = 14;
            this.comboOLQuery.Text = "全部";
            this.comboOLQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comboState.FormattingEnabled = true;
            this.comboState.Items.AddRange(new object[] { "全部", "未委托", "已过期", "委托成功", "委托失败", "已撤单" });
            this.comboState.Location = new Point(580, 0x11);
            this.comboState.Name = "comboState";
            this.comboState.Size = new Size(0x4e, 20);
            this.comboState.TabIndex = 0x10;
            this.comboState.Text = "全部";
            this.comboState.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comboConTypeQuery.FormattingEnabled = true;
            this.comboConTypeQuery.Items.AddRange(new object[] { "全部", "申买价", "申卖价", "最新价" });
            this.comboConTypeQuery.Location = new Point(0x1a9, 0x11);
            this.comboConTypeQuery.Name = "comboConTypeQuery";
            this.comboConTypeQuery.Size = new Size(0x4e, 20);
            this.comboConTypeQuery.TabIndex = 15;
            this.comboConTypeQuery.Text = "全部";
            this.comboConTypeQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.bindNavConOrder.AddNewItem = null;
            this.bindNavConOrder.BackColor = Color.Gainsboro;
            this.bindNavConOrder.CountItem = null;
            this.bindNavConOrder.DeleteItem = null;
            this.bindNavConOrder.Dock = DockStyle.Bottom;
            this.bindNavConOrder.Items.AddRange(new ToolStripItem[] { this.tSBtnConOrderFirst, this.tSBtnConOrderPrevious, this.toolStripLabel1, this.tSLblConOrderPage, this.tSBtnConOrderNext, this.tSBtnConOrderLast, this.tSBtnConOrderNum, this.tSTbxConOrderCurP, this.tSLblConOrderP, this.tSBtnConOrderGO });
            this.bindNavConOrder.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.bindNavConOrder.Location = new Point(3, 0x127);
            this.bindNavConOrder.MoveFirstItem = null;
            this.bindNavConOrder.MoveLastItem = null;
            this.bindNavConOrder.MoveNextItem = null;
            this.bindNavConOrder.MovePreviousItem = null;
            this.bindNavConOrder.Name = "bindNavConOrder";
            this.bindNavConOrder.PositionItem = null;
            this.bindNavConOrder.RenderMode = ToolStripRenderMode.System;
            this.bindNavConOrder.Size = new Size(0x32e, 0x18);
            this.bindNavConOrder.TabIndex = 0x26;
            this.bindNavConOrder.Text = "bindingNavigator1";
            this.tSBtnConOrderFirst.AutoSize = false;
            this.tSBtnConOrderFirst.BackgroundImage = (Image)manager.GetObject("tSBtnConOrderFirst.BackgroundImage");
            this.tSBtnConOrderFirst.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnConOrderFirst.Enabled = false;
            this.tSBtnConOrderFirst.ImageTransparentColor = Color.Magenta;
            this.tSBtnConOrderFirst.Name = "tSBtnConOrderFirst";
            this.tSBtnConOrderFirst.Size = new Size(0x10, 0x10);
            this.tSBtnConOrderFirst.ToolTipText = "第一页";
            this.tSBtnConOrderFirst.Click += new EventHandler(this.tSBtnConOrderFirst_Click);
            this.tSBtnConOrderPrevious.AutoSize = false;
            this.tSBtnConOrderPrevious.BackgroundImage = (Image)manager.GetObject("tSBtnConOrderPrevious.BackgroundImage");
            this.tSBtnConOrderPrevious.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnConOrderPrevious.Enabled = false;
            this.tSBtnConOrderPrevious.Image = (Image)manager.GetObject("tSBtnConOrderPrevious.Image");
            this.tSBtnConOrderPrevious.ImageTransparentColor = Color.Magenta;
            this.tSBtnConOrderPrevious.Name = "tSBtnConOrderPrevious";
            this.tSBtnConOrderPrevious.Size = new Size(0x10, 0x10);
            this.tSBtnConOrderPrevious.ToolTipText = "上一页";
            this.tSBtnConOrderPrevious.Click += new EventHandler(this.tSBtnConOrderPrevious_Click);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0, 0);
            this.tSLblConOrderPage.AutoSize = false;
            this.tSLblConOrderPage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSLblConOrderPage.ImageTransparentColor = Color.Magenta;
            this.tSLblConOrderPage.Name = "tSLblConOrderPage";
            this.tSLblConOrderPage.Size = new Size(0x5f, 0x10);
            this.tSLblConOrderPage.Text = "toolStripLabel2";
            this.tSBtnConOrderNext.AutoSize = false;
            this.tSBtnConOrderNext.BackColor = Color.Gainsboro;
            this.tSBtnConOrderNext.BackgroundImage = (Image)manager.GetObject("tSBtnConOrderNext.BackgroundImage");
            this.tSBtnConOrderNext.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnConOrderNext.ImageTransparentColor = Color.Magenta;
            this.tSBtnConOrderNext.Name = "tSBtnConOrderNext";
            this.tSBtnConOrderNext.Size = new Size(0x10, 0x10);
            this.tSBtnConOrderNext.ToolTipText = "下一页";
            this.tSBtnConOrderNext.Click += new EventHandler(this.tSBtnConOrderNext_Click);
            this.tSBtnConOrderLast.AutoSize = false;
            this.tSBtnConOrderLast.BackgroundImage = (Image)manager.GetObject("tSBtnConOrderLast.BackgroundImage");
            this.tSBtnConOrderLast.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnConOrderLast.ImageTransparentColor = Color.Magenta;
            this.tSBtnConOrderLast.Name = "tSBtnConOrderLast";
            this.tSBtnConOrderLast.Size = new Size(0x10, 0x10);
            this.tSBtnConOrderLast.Click += new EventHandler(this.tSBtnConOrderLast_Click);
            this.tSBtnConOrderNum.AutoSize = false;
            this.tSBtnConOrderNum.Name = "tSBtnConOrderNum";
            this.tSBtnConOrderNum.Size = new Size(0x11, 0x10);
            this.tSBtnConOrderNum.Text = "第";
            this.tSTbxConOrderCurP.AutoSize = false;
            this.tSTbxConOrderCurP.BackColor = Color.Snow;
            this.tSTbxConOrderCurP.BorderStyle = BorderStyle.None;
            this.tSTbxConOrderCurP.Name = "tSTbxConOrderCurP";
            this.tSTbxConOrderCurP.Size = new Size(50, 0x10);
            this.tSTbxConOrderCurP.TextBoxTextAlign = HorizontalAlignment.Center;
            this.tSLblConOrderP.AutoSize = false;
            this.tSLblConOrderP.Name = "tSLblConOrderP";
            this.tSLblConOrderP.Size = new Size(0x11, 0x10);
            this.tSLblConOrderP.Text = "页";
            this.tSBtnConOrderGO.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnConOrderGO.ImageTransparentColor = Color.Magenta;
            this.tSBtnConOrderGO.Name = "tSBtnConOrderGO";
            this.tSBtnConOrderGO.Size = new Size(0x1f, 0x15);
            this.tSBtnConOrderGO.Text = "GO";
            this.tSBtnConOrderGO.Click += new EventHandler(this.tSBtnConOrderGO_Click);
            this.dgAllConditionOrder.AllowUserToAddRows = false;
            this.dgAllConditionOrder.AllowUserToDeleteRows = false;
            this.dgAllConditionOrder.AllowUserToOrderColumns = true;
            this.dgAllConditionOrder.AllowUserToResizeRows = false;
            this.dgAllConditionOrder.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgAllConditionOrder.ColumnHeadersDefaultCellStyle = style;
            this.dgAllConditionOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgAllConditionOrder.Columns.AddRange(new DataGridViewColumn[] { this.SelectAll });
            style2.Alignment = DataGridViewContentAlignment.MiddleRight;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dgAllConditionOrder.DefaultCellStyle = style2;
            this.dgAllConditionOrder.Location = new Point(3, 0x2f);
            this.dgAllConditionOrder.Name = "dgAllConditionOrder";
            this.dgAllConditionOrder.RowHeadersVisible = false;
            this.dgAllConditionOrder.RowTemplate.Height = 20;
            this.dgAllConditionOrder.Size = new Size(0x32e, 0x10d);
            this.dgAllConditionOrder.TabIndex = 0x27;
            this.dgAllConditionOrder.TabStop = false;
            this.dgAllConditionOrder.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgAllConditionOrder_ColumnHeaderMouseClick);
            this.dgAllConditionOrder.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgAllConditionOrder_DataBindingComplete);
            this.SelectAll.HeaderText = "选择";
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Width = 70;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxConOrder);
            base.Name = "ConditionOrderQuery";
            base.Size = new Size(820, 0x142);
            base.Load += new EventHandler(this.ConditionOrder_Load);
            base.SizeChanged += new EventHandler(this.ConditionOrderQuery_SizeChanged);
            this.groupBoxConOrder.ResumeLayout(false);
            this.groupBoxConOrder.PerformLayout();
            this.bindNavConOrder.EndInit();
            this.bindNavConOrder.ResumeLayout(false);
            this.bindNavConOrder.PerformLayout();
            ((ISupportInitialize)this.dgAllConditionOrder).EndInit();
            base.ResumeLayout(false);
        }

        private string OrderSql()
        {
            string str = "1=1";
            if (this.comboCommodityQuery.SelectedIndex != 0)
            {
                str = str + " and commodityID = '" + this.comboCommodityQuery.Text + "'";
            }
            if (this.comboBSQuery.SelectedIndex != 0)
            {
                if (this.comboBSQuery.SelectedIndex == 1)
                {
                    str = str + " and B_S ='" + Global.BuySellStrArr[1] + "'";
                }
                else if (this.comboBSQuery.SelectedIndex == 2)
                {
                    str = str + " and B_S ='" + Global.BuySellStrArr[2] + "'";
                }
            }
            if (this.comboOLQuery.SelectedIndex != 0)
            {
                if (this.comboOLQuery.SelectedIndex == 1)
                {
                    str = str + " and O_L ='" + Global.SettleBasisStrArr[1] + "'";
                }
                else if (this.comboOLQuery.SelectedIndex == 2)
                {
                    str = str + " and O_L ='" + Global.SettleBasisStrArr[2] + "'";
                }
            }
            if (this.comboConTypeQuery.SelectedIndex != 0)
            {
                if (this.comboConTypeQuery.SelectedIndex == 1)
                {
                    str = str + " and CoditionType ='" + Global.ConditionTypeStrArr[2] + "'";
                }
                else if (this.comboConTypeQuery.SelectedIndex == 2)
                {
                    str = str + " and CoditionType ='" + Global.ConditionTypeStrArr[3] + "'";
                }
                else if (this.comboConTypeQuery.SelectedIndex == 3)
                {
                    str = str + " and CoditionType ='" + Global.ConditionTypeStrArr[1] + "'";
                }
            }
            if (this.comboState.SelectedIndex != 0)
            {
                if (this.comboState.SelectedIndex == 1)
                {
                    return (str + " and OrderState ='" + Global.OrderTypeStrArr[1] + "'");
                }
                if (this.comboState.SelectedIndex == 2)
                {
                    return (str + " and OrderState ='" + Global.OrderTypeStrArr[2] + "'");
                }
                if (this.comboState.SelectedIndex == 3)
                {
                    return (str + " and OrderState ='" + Global.OrderTypeStrArr[3] + "'");
                }
                if (this.comboState.SelectedIndex == 4)
                {
                    return (str + "and OrderState ='" + Global.OrderTypeStrArr[4] + "'");
                }
                if (this.comboState.SelectedIndex == 5)
                {
                    str = str + " and OrderState ='" + Global.OrderTypeStrArr[5] + "'";
                }
            }
            return str;
        }

        private void QueryConditionChanged(object sender, EventArgs e)
        {
            if (!this.isFirstLoad)
            {
                string str = string.Empty;
                short selectedIndex = 0;
                string text = "";
                short num2 = 0;
                short num3 = 0;
                string sql = this.OrderSql();
                if (this.comboCommodityQuery.SelectedIndex != 0)
                {
                    str = this.comboCommodityQuery.Text;
                }
                if (this.comboBSQuery.SelectedIndex != 0)
                {
                    selectedIndex = (short)this.comboBSQuery.SelectedIndex;
                }
                text = this.comboState.Text;
                if (this.comboConTypeQuery.SelectedIndex != 0)
                {
                    num3 = (short)this.comboConTypeQuery.SelectedIndex;
                }
                if ((sender != null) && (sender is MyRadioButton))
                {
                    MyRadioButton button = (MyRadioButton)sender;
                    if (!button.Checked)
                    {
                        return;
                    }
                }
                this.operationManager.queryConOrderOperation.ScreeningConOrderData(str, selectedIndex, text, num2, num3, sql);
            }
        }

        private void SetBindNavLayOut()
        {
            if (this.operationManager.queryConOrderOperation.IsShowPagingControl)
            {
                if (!this.bindNavConOrder.Visible)
                {
                    this.bindNavConOrder.Visible = true;
                    this.dgAllConditionOrder.Height = this.dgConditionOrderHeight - this.bindNavHeight;
                }
            }
            else if (this.bindNavConOrder.Visible)
            {
                this.bindNavConOrder.Visible = false;
                this.dgAllConditionOrder.Height = this.dgConditionOrderHeight;
            }
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboCommodityQuery.Items.Clear();
            this.comboCommodityQuery.Items.AddRange(commodityIDList.ToArray());
            this.comboCommodityQuery.SelectedIndex = 0;
            this.isFirstLoad = false;
        }

        private void SetControlText()
        {
            this.labComCode3.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_ConditionCode");
            this.labBS2.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_BS");
            this.labOL2.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_OG");
            this.labType2.Text = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_labType2");
            this.labState.Text = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_labState");
            this.btnRevoke.Text = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_btnRevoke");
            this.BtnRefresh.Text = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_BtnRefresh");
            this.comboBSQuery.Items.Clear();
            this.comboOLQuery.Items.Clear();
            this.comboConTypeQuery.Items.Clear();
            this.comboState.Items.Clear();
            string item = Global.M_ResourceManager.GetString("TradeStr_All");
            this.comboBSQuery.Text = item;
            this.comboOLQuery.Text = item;
            this.comboConTypeQuery.Text = item;
            this.comboState.Text = item;
            this.comboBSQuery.Items.Add(item);
            this.comboOLQuery.Items.Add(item);
            this.comboConTypeQuery.Items.Add(item);
            this.comboState.Items.Add(item);
            this.comboBSQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboBSOrderItems1"));
            this.comboBSQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboBSOrderItems2"));
            this.comboOLQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboOLOrderItems1"));
            this.comboOLQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboOLOrderItems2"));
            this.comboConTypeQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboConTypeOrderItems1"));
            this.comboConTypeQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboConTypeOrderItems2"));
            this.comboConTypeQuery.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboConTypeOrderItems3"));
            this.comboState.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboStateItem1"));
            this.comboState.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboStateItem2"));
            this.comboState.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboStateItem3"));
            this.comboState.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboStateItem4"));
            this.comboState.Items.Add(Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_comboStateItem5"));
            this.comboBSQuery.SelectedIndex = 0;
            this.comboOLQuery.SelectedIndex = 0;
            this.comboConTypeQuery.SelectedIndex = 0;
            this.comboState.SelectedIndex = 0;
            this.dgConditionOrderHeight = this.dgAllConditionOrder.Height;
            this.bindNavHeight = this.bindNavConOrder.Height;
            this.bindNavConOrder.Visible = false;
        }

        private void SetDataGridViewHeader()
        {
            if (!this.isHeaderLoad)
            {
                for (int i = 0; i < this.dgAllConditionOrder.Columns.Count; i++)
                {
                    ColItemInfo info = (ColItemInfo)this.conditionOrderItemInfo.m_htItemInfo[this.dgAllConditionOrder.Columns[i].Name];
                    if (info != null)
                    {
                        this.dgAllConditionOrder.Columns[i].MinimumWidth = info.width;
                        this.dgAllConditionOrder.Columns[i].FillWeight = info.width;
                        this.dgAllConditionOrder.Columns[i].HeaderText = info.name;
                        this.dgAllConditionOrder.Columns[i].DefaultCellStyle.Format = info.format;
                        this.dgAllConditionOrder.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
                        if (info.sortID == 1)
                        {
                            this.dgAllConditionOrder.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                        else
                        {
                            this.dgAllConditionOrder.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (!this.conditionOrderItemInfo.m_strItems.Contains(this.dgAllConditionOrder.Columns[i].Name))
                        {
                            this.dgAllConditionOrder.Columns[i].Visible = false;
                        }
                        if (i == 0)
                        {
                            this.dgAllConditionOrder.Columns[i].ReadOnly = false;
                        }
                        else
                        {
                            this.dgAllConditionOrder.Columns[i].ReadOnly = true;
                        }
                    }
                }
                this.isHeaderLoad = true;
            }
        }

        private void tSBtnConOrderFirst_Click(object sender, EventArgs e)
        {
            this.operationManager.queryConOrderOperation.QueryPageConOrderData(0, 0);
            this.ConditionOrderSetEnable(true);
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        private void tSBtnConOrderGO_Click(object sender, EventArgs e)
        {
            string text = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPageNum");
            string caption = Global.M_ResourceManager.GetString("TradeStr_MainForm_PageNumError");
            if (this.tSTbxConOrderCurP.Text.Trim().Length == 0)
            {
                string str3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPageNum");
                string str4 = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_ErrorInfo");
                MessageBox.Show(str3, str4, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.tSTbxConOrderCurP.Focus();
            }
            else
            {
                int num = int.Parse(this.tSTbxConOrderCurP.Text.Trim());
                if (num > 0)
                {
                    if (num == this.operationManager.queryConOrderOperation.ConOrderCurrentPage)
                    {
                        return;
                    }
                    if (num <= this.operationManager.queryConOrderOperation.ConOrderAllPage)
                    {
                        this.operationManager.queryConOrderOperation.QueryPageConOrderData(4, num);
                        this.ConditionOrderSetEnable(true);
                    }
                }
                else
                {
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.tSTbxConOrderCurP.Focus();
                    this.tSTbxConOrderCurP.SelectAll();
                }
            }
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        private void tSBtnConOrderLast_Click(object sender, EventArgs e)
        {
            this.operationManager.queryConOrderOperation.QueryPageConOrderData(3, 0);
            this.ConditionOrderSetEnable(true);
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        private void tSBtnConOrderNext_Click(object sender, EventArgs e)
        {
            this.operationManager.queryConOrderOperation.QueryPageConOrderData(2, 0);
            this.ConditionOrderSetEnable(true);
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        private void tSBtnConOrderPrevious_Click(object sender, EventArgs e)
        {
            this.operationManager.queryConOrderOperation.QueryPageConOrderData(1, 0);
            this.ConditionOrderSetEnable(true);
            this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
        }

        private delegate void FillConditionOrder(DataTable dt);
    }
}
