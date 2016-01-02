﻿namespace FuturesTrade.Gnnt.UI.Modules.Query
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation;
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

    public class AllOrder : UserControl
    {
        private AllOrderItemInfo allOrderItemInfo = new AllOrderItemInfo();
        private ToolStripSeparator bindingNavigatorSeparator1;
        internal BindingNavigator bindNavAllOrder;
        private int bindNavHeight;
        private MyButton buttonAll;
        private MyButton buttonAllAllOrder;
        private MyButton buttonCancelAllOrder;
        private byte buttonFlag;
        private MyButton buttonSelAllOrder;
        private MyButton buttonVoid;
        internal MyCombobox comboB_SAllOrder;
        internal MyCombobox comboCommodityAllOrder;
        internal MyCombobox comboTrancAllOrder;
        private IContainer components;
        private string DeleteOrdersContent = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_DeleteOrdersContent");
        private string DeleteOrdersTitle = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_DeleteOrdersTitle");
        internal DataGridView dgAllOrder;
        private int dgAllOrderHeight;
        private FillAllTradeOrder fillAllTradeOrder;
        private GroupBox groupBoxAllOrder;
        private GroupBox groupBoxBSAllOrder;
        private bool isAllClicked = true;
        private bool isAllOrderHeaderLoad;
        private bool isFirstLoad = true;
        private Label labelB_SAllOrder;
        private Label labelCommodityAllOrder;
        private Label labelTrancAllOrder;
        private OperationManager operationManager = OperationManager.GetInstance();
        private string OrdersNumNonCancellation = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_OrdersNumNonCancellation");
        private int pageNum;
        private Panel panelLeft;
        private MyRadioButton radioAllAllOrder;
        private MyRadioButton radioCancelAllOrder;
        private string RevokeOrdersErrorMessege = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_RevokeOrdersErrorMessege");
        private DataGridViewCheckBoxColumn SelectFlagF2;
        private int Style;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tSBtnAllOrderFirst;
        private ToolStripButton tSBtnAllOrderGO;
        private ToolStripButton tSBtnAllOrderLast;
        private ToolStripButton tSBtnAllOrderNext;
        private ToolStripButton tSBtnAllOrderPrevious;
        private ToolStripLabel tSLblAllOrderNum;
        private ToolStripLabel tSLblAllOrderP;
        private ToolStripLabel tSLblAllOrderPage;
        private ToolStripTextBox tSTbxAllOrderCurPage;

        public AllOrder(int style)
        {
            this.InitializeComponent();
            this.Style = style;
            this.operationManager.queryAllOrderOperation.AllOrderFill = new QueryAllOrderOperation.AllOrderFillCallBack(this.AllTradeOrderInfoFill);
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.CreateHandle();
        }

        private void AllOrder_Load(object sender, EventArgs e)
        {
            this.SetControlText();
            this.ComboLoad();
        }

        private void AllOrder_SizeChanged(object sender, EventArgs e)
        {
            this.dgAllOrderHeight = this.dgAllOrder.Height;
            this.bindNavHeight = this.bindNavAllOrder.Height;
            this.buttonAll.Height = this.panelLeft.Height / 2;
            this.buttonVoid.Height = this.panelLeft.Height / 2;
        }

        private void AllOrderSetEnable(bool isEnable)
        {
            if (this.operationManager.queryAllOrderOperation.AllOrderCurrentPage == 1)
            {
                this.tSBtnAllOrderFirst.Enabled = !isEnable;
                this.tSBtnAllOrderPrevious.Enabled = !isEnable;
                this.tSBtnAllOrderNext.Enabled = isEnable;
                this.tSBtnAllOrderLast.Enabled = isEnable;
            }
            else if (this.operationManager.queryAllOrderOperation.AllOrderCurrentPage == this.operationManager.queryAllOrderOperation.AllOrderAllPage)
            {
                this.tSBtnAllOrderFirst.Enabled = isEnable;
                this.tSBtnAllOrderPrevious.Enabled = isEnable;
                this.tSBtnAllOrderNext.Enabled = !isEnable;
                this.tSBtnAllOrderLast.Enabled = !isEnable;
            }
            else
            {
                this.tSBtnAllOrderFirst.Enabled = isEnable;
                this.tSBtnAllOrderPrevious.Enabled = isEnable;
                this.tSBtnAllOrderNext.Enabled = isEnable;
                this.tSBtnAllOrderLast.Enabled = isEnable;
            }
        }

        private void AllTradeOrderInfoFill(DataTable dTable, bool isPaging)
        {
            try
            {
                this.fillAllTradeOrder = new FillAllTradeOrder(this.dsAllTradeFill);
                this.HandleCreated();
                base.Invoke(this.fillAllTradeOrder, new object[] { dTable, isPaging });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            this.buttonAll.Enabled = false;
            this.buttonAll.BackColor = Color.LightGray;
            this.buttonVoid.Enabled = true;
            this.buttonVoid.BackColor = Color.White;
            this.isAllClicked = true;
            this.QueryConditionChanged(sender, e);
        }

        private void buttonAllAllOrder_Click(object sender, EventArgs e)
        {
            if (this.buttonAllAllOrder.Text.Equals(this.operationManager.AllCheck))
            {
                for (int i = 0; i < this.dgAllOrder.Rows.Count; i++)
                {
                    if (this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
                    {
                        this.dgAllOrder.Rows[i].Cells[0].Value = true;
                    }
                    else
                    {
                        this.dgAllOrder.Rows[i].Cells[0].Value = false;
                    }
                }
                this.buttonAllAllOrder.Text = this.operationManager.AllNotCheck;
            }
            else
            {
                for (int j = 0; j < this.dgAllOrder.Rows.Count; j++)
                {
                    if (this.dgAllOrder.Rows[j].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[j].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
                    {
                        this.dgAllOrder.Rows[j].Cells[0].Value = false;
                    }
                    else
                    {
                        this.dgAllOrder.Rows[j].Cells[0].Value = false;
                    }
                }
                this.buttonAllAllOrder.Text = this.operationManager.AllCheck;
            }
        }

        private void buttonCancelAllOrder_Click(object sender, EventArgs e)
        {
            MessageForm form;
            List<string> orderNoList = new List<string>();
            string str = string.Empty;
            string str2 = string.Empty;
            for (int i = this.dgAllOrder.Rows.Count - 1; i >= 0; i--)
            {
                if ((this.dgAllOrder["SelectFlagF2", i].Value != null) && ((bool)this.dgAllOrder["SelectFlagF2", i].Value))
                {
                    if (this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
                    {
                        str = str + "--" + this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
                        string item = this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
                        orderNoList.Add(item);
                    }
                    else
                    {
                        str2 = str2 + "--" + this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
                    }
                }
            }
            if (!str2.Equals(""))
            {
                string message = string.Format(this.OrdersNumNonCancellation, str2);
                form = new MessageForm(this.RevokeOrdersErrorMessege, message, 1)
                {
                    Owner = base.ParentForm
                };
                form.ShowDialog();
                form.Dispose();
            }
            if (orderNoList.Count > 0)
            {
                form = new MessageForm(this.operationManager.RevokeOrders, string.Format(this.operationManager.RevokeOrdersMessge, str), 0)
                {
                    Owner = base.ParentForm
                };
                form.ShowDialog();
                form.Dispose();
                if (form.isOK)
                {
                    this.operationManager.revokeOrderOperation.RevokeOrderThread(orderNoList);
                }
            }
            else
            {
                form = new MessageForm(this.DeleteOrdersTitle, this.DeleteOrdersContent, 1)
                {
                    Owner = base.ParentForm
                };
                form.ShowDialog();
                form.Dispose();
            }
        }

        private void buttonSelAllOrder_Click(object sender, EventArgs e)
        {
            this.operationManager.queryAllOrderOperation.ButtonRefreshFlag = 1;
            this.operationManager.queryAllOrderOperation.QueryAllOrderInfoLoad();
            this.operationManager.IdleRefreshButton = 0;
        }

        private void buttonVoid_Click(object sender, EventArgs e)
        {
            this.buttonAll.Enabled = true;
            this.buttonAll.BackColor = Color.White;
            this.buttonVoid.Enabled = false;
            this.buttonVoid.BackColor = Color.LightGray;
            this.isAllClicked = false;
            this.QueryConditionChanged(sender, e);
        }

        private void ComboLoad()
        {
            this.comboB_SAllOrder.Items.Add(this.operationManager.StrAll);
            this.comboB_SAllOrder.Items.Add(this.operationManager.StrBuy);
            this.comboB_SAllOrder.Items.Add(this.operationManager.StrSale);
            this.comboB_SAllOrder.SelectedIndex = 0;
        }

        private void dgAllOrder_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex != -1) && (this.dgAllOrder.Rows[e.RowIndex].Cells[1].Value.ToString() != this.operationManager.Total))
            {
                if (this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
                {
                    MessageForm form = new MessageForm(this.operationManager.RevokeOrders, string.Format(this.operationManager.RevokeOrdersMessge, this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim()), 0);
                    form.ShowDialog();
                    form.Dispose();
                    if (form.isOK)
                    {
                        List<string> orderNoList = new List<string>();
                        string item = this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim();
                        orderNoList.Add(item);
                        this.operationManager.revokeOrderOperation.RevokeOrderThread(orderNoList);
                    }
                }
                else
                {
                    string message = string.Format(this.OrdersNumNonCancellation, this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim());
                    MessageForm form2 = new MessageForm(this.RevokeOrdersErrorMessege, message, 1);
                    form2.ShowDialog();
                    form2.Dispose();
                }
            }
        }

        private void dgAllOrder_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
                {
                    if (Global.StatusInfoFill != null)
                    {
                        Global.StatusInfoFill(this.operationManager.DoubleClickCancellation, Global.RightColor, true);
                    }
                }
                else
                {
                    string format = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_StateNonCancellation");
                    if (Global.StatusInfoFill != null)
                    {
                        Global.StatusInfoFill(string.Format(format, this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value), Global.RightColor, true);
                    }
                }
            }
        }

        private void dgAllOrder_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (Global.StatusInfoFill != null))
            {
                Global.StatusInfoFill("", Global.RightColor, true);
            }
        }

        private void dgAllOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                this.operationManager.queryAllOrderOperation.AllOrderDataGridViewSort(this.dgAllOrder.Columns[e.ColumnIndex].Name.ToString());
            }
        }

        private void dgAllOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.SetBackGround();
            Global.BSAlign(this.dgAllOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dsAllTradeFill(DataTable dt, bool isPaging)
        {
            this.dgAllOrder.DataSource = dt;
            this.SetDataGridViewHeader();
            this.SetBindNavLayOut(isPaging);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AllOrder));
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.groupBoxAllOrder = new GroupBox();
            this.bindNavAllOrder = new BindingNavigator(this.components);
            this.bindingNavigatorSeparator1 = new ToolStripSeparator();
            this.tSBtnAllOrderFirst = new ToolStripButton();
            this.tSBtnAllOrderPrevious = new ToolStripButton();
            this.tSLblAllOrderPage = new ToolStripLabel();
            this.tSBtnAllOrderNext = new ToolStripButton();
            this.tSBtnAllOrderLast = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tSLblAllOrderNum = new ToolStripLabel();
            this.tSTbxAllOrderCurPage = new ToolStripTextBox();
            this.tSLblAllOrderP = new ToolStripLabel();
            this.tSBtnAllOrderGO = new ToolStripButton();
            this.dgAllOrder = new DataGridView();
            this.SelectFlagF2 = new DataGridViewCheckBoxColumn();
            this.comboB_SAllOrder = new MyCombobox();
            this.labelB_SAllOrder = new Label();
            this.comboTrancAllOrder = new MyCombobox();
            this.comboCommodityAllOrder = new MyCombobox();
            this.labelTrancAllOrder = new Label();
            this.labelCommodityAllOrder = new Label();
            this.buttonCancelAllOrder = new MyButton();
            this.buttonAllAllOrder = new MyButton();
            this.buttonSelAllOrder = new MyButton();
            this.groupBoxBSAllOrder = new GroupBox();
            this.radioCancelAllOrder = new MyRadioButton();
            this.radioAllAllOrder = new MyRadioButton();
            this.panelLeft = new Panel();
            this.buttonVoid = new MyButton();
            this.buttonAll = new MyButton();
            this.groupBoxAllOrder.SuspendLayout();
            this.bindNavAllOrder.BeginInit();
            this.bindNavAllOrder.SuspendLayout();
            ((ISupportInitialize)this.dgAllOrder).BeginInit();
            this.groupBoxBSAllOrder.SuspendLayout();
            this.panelLeft.SuspendLayout();
            base.SuspendLayout();
            this.groupBoxAllOrder.Controls.Add(this.bindNavAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.dgAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.comboB_SAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.labelB_SAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.comboTrancAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.comboCommodityAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.labelTrancAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.labelCommodityAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.buttonCancelAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.buttonAllAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.buttonSelAllOrder);
            this.groupBoxAllOrder.Controls.Add(this.groupBoxBSAllOrder);
            this.groupBoxAllOrder.Dock = DockStyle.Fill;
            this.groupBoxAllOrder.Location = new Point(0x20, 0);
            this.groupBoxAllOrder.Margin = new Padding(0);
            this.groupBoxAllOrder.Name = "groupBoxAllOrder";
            this.groupBoxAllOrder.Padding = new Padding(3, 0, 3, 3);
            this.groupBoxAllOrder.Size = new Size(0x2b0, 200);
            this.groupBoxAllOrder.TabIndex = 0x16;
            this.groupBoxAllOrder.TabStop = false;
            this.groupBoxAllOrder.Text = "委托查询";
            this.groupBoxAllOrder.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxAllOrder.ForeColor = Color.FromArgb(235, 235, 235);
            this.bindNavAllOrder.AddNewItem = null;
            this.bindNavAllOrder.AutoSize = false;
            this.bindNavAllOrder.BackColor = Color.Gainsboro;
            this.bindNavAllOrder.CountItem = null;
            this.bindNavAllOrder.DeleteItem = null;
            this.bindNavAllOrder.Dock = DockStyle.Bottom;
            this.bindNavAllOrder.GripMargin = new Padding(0);
            this.bindNavAllOrder.Items.AddRange(new ToolStripItem[] { this.bindingNavigatorSeparator1, this.tSBtnAllOrderFirst, this.tSBtnAllOrderPrevious, this.tSLblAllOrderPage, this.tSBtnAllOrderNext, this.tSBtnAllOrderLast, this.toolStripSeparator1, this.tSLblAllOrderNum, this.tSTbxAllOrderCurPage, this.tSLblAllOrderP, this.tSBtnAllOrderGO });
            this.bindNavAllOrder.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.bindNavAllOrder.Location = new Point(3, 0xb1);
            this.bindNavAllOrder.MoveFirstItem = null;
            this.bindNavAllOrder.MoveLastItem = null;
            this.bindNavAllOrder.MoveNextItem = null;
            this.bindNavAllOrder.MovePreviousItem = null;
            this.bindNavAllOrder.Name = "bindNavAllOrder";
            this.bindNavAllOrder.Padding = new Padding(0);
            this.bindNavAllOrder.PositionItem = null;
            this.bindNavAllOrder.RenderMode = ToolStripRenderMode.System;
            this.bindNavAllOrder.Size = new Size(0x2aa, 20);
            this.bindNavAllOrder.TabIndex = 0x1b;
            this.bindNavAllOrder.Text = "bindingNavigator3";
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new Size(6, 0x17);
            this.tSBtnAllOrderFirst.AutoSize = false;
            this.tSBtnAllOrderFirst.BackgroundImage = (Image)manager.GetObject("tSBtnAllOrderFirst.BackgroundImage");
            this.tSBtnAllOrderFirst.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnAllOrderFirst.ImageTransparentColor = Color.Magenta;
            this.tSBtnAllOrderFirst.Name = "tSBtnAllOrderFirst";
            this.tSBtnAllOrderFirst.Size = new Size(0x10, 0x10);
            this.tSBtnAllOrderFirst.Click += new EventHandler(this.tSBtnAllOrderFirst_Click);
            this.tSBtnAllOrderPrevious.AutoSize = false;
            this.tSBtnAllOrderPrevious.BackgroundImage = (Image)manager.GetObject("tSBtnAllOrderPrevious.BackgroundImage");
            this.tSBtnAllOrderPrevious.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnAllOrderPrevious.Image = (Image)manager.GetObject("tSBtnAllOrderPrevious.Image");
            this.tSBtnAllOrderPrevious.ImageTransparentColor = Color.Magenta;
            this.tSBtnAllOrderPrevious.Name = "tSBtnAllOrderPrevious";
            this.tSBtnAllOrderPrevious.Size = new Size(0x10, 0x10);
            this.tSBtnAllOrderPrevious.Click += new EventHandler(this.tSBtnAllOrderPrevious_Click);
            this.tSLblAllOrderPage.AutoSize = false;
            this.tSLblAllOrderPage.Name = "tSLblAllOrderPage";
            this.tSLblAllOrderPage.Size = new Size(0x5f, 0x10);
            this.tSLblAllOrderPage.Text = "Page/Total";
            this.tSBtnAllOrderNext.AutoSize = false;
            this.tSBtnAllOrderNext.BackgroundImage = (Image)manager.GetObject("tSBtnAllOrderNext.BackgroundImage");
            this.tSBtnAllOrderNext.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnAllOrderNext.Image = (Image)manager.GetObject("tSBtnAllOrderNext.Image");
            this.tSBtnAllOrderNext.ImageTransparentColor = Color.Magenta;
            this.tSBtnAllOrderNext.Name = "tSBtnAllOrderNext";
            this.tSBtnAllOrderNext.Size = new Size(0x10, 0x10);
            this.tSBtnAllOrderNext.Click += new EventHandler(this.tSBtnAllOrderNext_Click);
            this.tSBtnAllOrderLast.AutoSize = false;
            this.tSBtnAllOrderLast.BackgroundImage = (Image)manager.GetObject("tSBtnAllOrderLast.BackgroundImage");
            this.tSBtnAllOrderLast.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnAllOrderLast.Image = (Image)manager.GetObject("tSBtnAllOrderLast.Image");
            this.tSBtnAllOrderLast.ImageTransparentColor = Color.Magenta;
            this.tSBtnAllOrderLast.Name = "tSBtnAllOrderLast";
            this.tSBtnAllOrderLast.Size = new Size(0x10, 0x10);
            this.tSBtnAllOrderLast.Click += new EventHandler(this.tSBtnAllOrderLast_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x17);
            this.tSLblAllOrderNum.AutoSize = false;
            this.tSLblAllOrderNum.Name = "tSLblAllOrderNum";
            this.tSLblAllOrderNum.Size = new Size(0x11, 0x10);
            this.tSLblAllOrderNum.Text = "第";
            this.tSTbxAllOrderCurPage.AutoSize = false;
            this.tSTbxAllOrderCurPage.BorderStyle = BorderStyle.None;
            this.tSTbxAllOrderCurPage.Name = "tSTbxAllOrderCurPage";
            this.tSTbxAllOrderCurPage.Size = new Size(50, 0x10);
            this.tSTbxAllOrderCurPage.TextBoxTextAlign = HorizontalAlignment.Center;
            this.tSLblAllOrderP.AutoSize = false;
            this.tSLblAllOrderP.Name = "tSLblAllOrderP";
            this.tSLblAllOrderP.Size = new Size(0x11, 0x10);
            this.tSLblAllOrderP.Text = "页";
            this.tSBtnAllOrderGO.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tSBtnAllOrderGO.Image = (Image)manager.GetObject("tSBtnAllOrderGO.Image");
            this.tSBtnAllOrderGO.ImageTransparentColor = Color.Magenta;
            this.tSBtnAllOrderGO.Name = "tSBtnAllOrderGO";
            this.tSBtnAllOrderGO.Size = new Size(0x1f, 0x15);
            this.tSBtnAllOrderGO.Text = "GO";
            this.tSBtnAllOrderGO.Click += new EventHandler(this.tSBtnAllOrderGO_Click);
            this.dgAllOrder.AllowUserToAddRows = false;
            this.dgAllOrder.AllowUserToDeleteRows = false;
            this.dgAllOrder.AllowUserToOrderColumns = true;
            this.dgAllOrder.AllowUserToResizeRows = false;
            this.dgAllOrder.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgAllOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgAllOrder.ColumnHeadersDefaultCellStyle = style;
            this.dgAllOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgAllOrder.Columns.AddRange(new DataGridViewColumn[] { this.SelectFlagF2 });
            style2.Alignment = DataGridViewContentAlignment.MiddleRight;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dgAllOrder.DefaultCellStyle = style2;
            this.dgAllOrder.Location = new Point(3, 0x29);
            this.dgAllOrder.Margin = new Padding(0);
            this.dgAllOrder.Name = "dgAllOrder";
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dgAllOrder.RowHeadersDefaultCellStyle = style3;
            this.dgAllOrder.RowHeadersVisible = false;
            this.dgAllOrder.RowTemplate.Height = 0x12;
            this.dgAllOrder.ScrollBars = ScrollBars.Vertical;
            this.dgAllOrder.Size = new Size(0x2aa, 0x9c);
            this.dgAllOrder.TabIndex = 0;
            this.dgAllOrder.TabStop = false;
            this.dgAllOrder.CellDoubleClick += new DataGridViewCellEventHandler(this.dgAllOrder_CellDoubleClick);
            this.dgAllOrder.CellMouseEnter += new DataGridViewCellEventHandler(this.dgAllOrder_CellMouseEnter);
            this.dgAllOrder.CellMouseLeave += new DataGridViewCellEventHandler(this.dgAllOrder_CellMouseLeave);
            this.dgAllOrder.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgAllOrder_ColumnHeaderMouseClick);
            this.dgAllOrder.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgAllOrder_DataBindingComplete);
            this.SelectFlagF2.HeaderText = "选择";
            this.SelectFlagF2.Name = "SelectFlagF2";
            this.comboB_SAllOrder.FormattingEnabled = true;
            this.comboB_SAllOrder.Location = new Point(0x153, 0x11);
            this.comboB_SAllOrder.Margin = new Padding(0);
            this.comboB_SAllOrder.Name = "comboB_SAllOrder";
            this.comboB_SAllOrder.Size = new Size(0x39, 20);
            this.comboB_SAllOrder.TabIndex = 0x19;
            this.comboB_SAllOrder.TabStop = false;
            this.comboB_SAllOrder.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.labelB_SAllOrder.AutoSize = true;
            this.labelB_SAllOrder.ImeMode = ImeMode.NoControl;
            this.labelB_SAllOrder.Location = new Point(0x131, 20);
            this.labelB_SAllOrder.Margin = new Padding(0);
            this.labelB_SAllOrder.Name = "labelB_SAllOrder";
            this.labelB_SAllOrder.Size = new Size(0x23, 12);
            this.labelB_SAllOrder.TabIndex = 0x18;
            this.labelB_SAllOrder.Text = "买/卖";
            this.comboTrancAllOrder.Location = new Point(0xd6, 0x10);
            this.comboTrancAllOrder.Margin = new Padding(0);
            this.comboTrancAllOrder.Name = "comboTrancAllOrder";
            this.comboTrancAllOrder.Size = new Size(0x58, 20);
            this.comboTrancAllOrder.TabIndex = 20;
            this.comboTrancAllOrder.TabStop = false;
            this.comboTrancAllOrder.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.comboCommodityAllOrder.Location = new Point(70, 0x10);
            this.comboCommodityAllOrder.Margin = new Padding(0);
            this.comboCommodityAllOrder.Name = "comboCommodityAllOrder";
            this.comboCommodityAllOrder.Size = new Size(80, 20);
            this.comboCommodityAllOrder.TabIndex = 0x13;
            this.comboCommodityAllOrder.TabStop = false;
            this.comboCommodityAllOrder.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
            this.labelTrancAllOrder.ImeMode = ImeMode.NoControl;
            this.labelTrancAllOrder.Location = new Point(150, 0x12);
            this.labelTrancAllOrder.Margin = new Padding(0);
            this.labelTrancAllOrder.Name = "labelTrancAllOrder";
            this.labelTrancAllOrder.Size = new Size(0x48, 0x10);
            this.labelTrancAllOrder.TabIndex = 0x12;
            this.labelTrancAllOrder.Text = "交易代码：";
            this.labelTrancAllOrder.TextAlign = ContentAlignment.MiddleCenter;
            this.labelCommodityAllOrder.ImeMode = ImeMode.NoControl;
            this.labelCommodityAllOrder.Location = new Point(3, 0x12);
            this.labelCommodityAllOrder.Margin = new Padding(0);
            this.labelCommodityAllOrder.Name = "labelCommodityAllOrder";
            this.labelCommodityAllOrder.Size = new Size(0x48, 0x10);
            this.labelCommodityAllOrder.TabIndex = 0x11;
            this.labelCommodityAllOrder.Text = "商品代码：";
            this.labelCommodityAllOrder.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonCancelAllOrder.ImeMode = ImeMode.NoControl;
            this.buttonCancelAllOrder.Location = new Point(0x278, 0x11);
            this.buttonCancelAllOrder.Margin = new Padding(0);
            this.buttonCancelAllOrder.Name = "buttonCancelAllOrder";
            this.buttonCancelAllOrder.Size = new Size(0x35, 20);
            this.buttonCancelAllOrder.TabIndex = 0x10;
            this.buttonCancelAllOrder.TabStop = false;
            this.buttonCancelAllOrder.Text = "撤单";
            this.buttonCancelAllOrder.UseVisualStyleBackColor = true;
            this.buttonCancelAllOrder.Click += new EventHandler(this.buttonCancelAllOrder_Click);
            this.buttonAllAllOrder.ImeMode = ImeMode.NoControl;
            this.buttonAllAllOrder.Location = new Point(0x242, 0x11);
            this.buttonAllAllOrder.Margin = new Padding(0);
            this.buttonAllAllOrder.Name = "buttonAllAllOrder";
            this.buttonAllAllOrder.Size = new Size(0x35, 20);
            this.buttonAllAllOrder.TabIndex = 15;
            this.buttonAllAllOrder.TabStop = false;
            this.buttonAllAllOrder.Text = "全选";
            this.buttonAllAllOrder.UseVisualStyleBackColor = true;
            this.buttonAllAllOrder.Click += new EventHandler(this.buttonAllAllOrder_Click);
            this.buttonSelAllOrder.ImeMode = ImeMode.NoControl;
            this.buttonSelAllOrder.Location = new Point(520, 0x11);
            this.buttonSelAllOrder.Margin = new Padding(0);
            this.buttonSelAllOrder.Name = "buttonSelAllOrder";
            this.buttonSelAllOrder.Size = new Size(0x39, 20);
            this.buttonSelAllOrder.TabIndex = 14;
            this.buttonSelAllOrder.TabStop = false;
            this.buttonSelAllOrder.Text = "刷新";
            this.buttonSelAllOrder.UseVisualStyleBackColor = true;
            this.buttonSelAllOrder.Click += new EventHandler(this.buttonSelAllOrder_Click);
            this.groupBoxBSAllOrder.Controls.Add(this.radioCancelAllOrder);
            this.groupBoxBSAllOrder.Controls.Add(this.radioAllAllOrder);
            this.groupBoxBSAllOrder.Location = new Point(0x18f, 9);
            this.groupBoxBSAllOrder.Margin = new Padding(0);
            this.groupBoxBSAllOrder.Name = "groupBoxBSAllOrder";
            this.groupBoxBSAllOrder.Padding = new Padding(0);
            this.groupBoxBSAllOrder.Size = new Size(0x76, 30);
            this.groupBoxBSAllOrder.TabIndex = 13;
            this.groupBoxBSAllOrder.TabStop = false;
            this.radioCancelAllOrder.AutoSize = true;
            this.radioCancelAllOrder.ImeMode = ImeMode.NoControl;
            this.radioCancelAllOrder.Location = new Point(0x3f, 11);
            this.radioCancelAllOrder.Margin = new Padding(0);
            this.radioCancelAllOrder.Name = "radioCancelAllOrder";
            this.radioCancelAllOrder.Size = new Size(0x2f, 0x10);
            this.radioCancelAllOrder.TabIndex = 1;
            this.radioCancelAllOrder.Text = "可撤";
            this.radioCancelAllOrder.UseVisualStyleBackColor = true;
            this.radioCancelAllOrder.CheckedChanged += new EventHandler(this.QueryConditionChanged);
            this.radioAllAllOrder.AutoSize = true;
            this.radioAllAllOrder.Checked = true;
            this.radioAllAllOrder.ImeMode = ImeMode.NoControl;
            this.radioAllAllOrder.Location = new Point(7, 10);
            this.radioAllAllOrder.Margin = new Padding(0);
            this.radioAllAllOrder.Name = "radioAllAllOrder";
            this.radioAllAllOrder.Size = new Size(0x2f, 0x10);
            this.radioAllAllOrder.TabIndex = 0;
            this.radioAllAllOrder.TabStop = true;
            this.radioAllAllOrder.Text = "全部";
            this.radioAllAllOrder.UseVisualStyleBackColor = true;
            this.radioAllAllOrder.CheckedChanged += new EventHandler(this.QueryConditionChanged);
            this.panelLeft.Controls.Add(this.buttonVoid);
            this.panelLeft.Controls.Add(this.buttonAll);
            this.panelLeft.Dock = DockStyle.Left;
            this.panelLeft.Location = new Point(0, 0);
            this.panelLeft.Margin = new Padding(0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new Size(0x20, 200);
            this.panelLeft.TabIndex = 30;
            //this.panelLeft.BackColor = Color.Black;
            this.buttonVoid.BackColor = Color.White;
            this.buttonVoid.Dock = DockStyle.Fill;
            this.buttonVoid.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.buttonVoid.ForeColor = SystemColors.ControlDarkDark;
            this.buttonVoid.Location = new Point(0, 0x62);
            this.buttonVoid.Margin = new Padding(0);
            this.buttonVoid.Name = "buttonVoid";
            this.buttonVoid.Size = new Size(0x20, 0x66);
            this.buttonVoid.TabIndex = 2;
            this.buttonVoid.TabStop = false;
            this.buttonVoid.Text = "可\r\n撤";
            this.buttonVoid.UseVisualStyleBackColor = false;
            this.buttonVoid.Click += new EventHandler(this.buttonVoid_Click);
            this.buttonAll.BackColor = Color.LightGray;
            this.buttonAll.Dock = DockStyle.Top;
            this.buttonAll.Enabled = false;
            this.buttonAll.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.buttonAll.ForeColor = SystemColors.ControlDarkDark;
            this.buttonAll.Location = new Point(0, 0);
            this.buttonAll.Margin = new Padding(0);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new Size(0x20, 0x62);
            this.buttonAll.TabIndex = 1;
            this.buttonAll.TabStop = false;
            this.buttonAll.Text = "全\r\n部";
            this.buttonAll.UseVisualStyleBackColor = false;
            this.buttonAll.Click += new EventHandler(this.buttonAll_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxAllOrder);
            base.Controls.Add(this.panelLeft);
            base.Margin = new Padding(0);
            base.Name = "AllOrder";
            base.Size = new Size(720, 200);
            base.Load += new EventHandler(this.AllOrder_Load);
            base.SizeChanged += new EventHandler(this.AllOrder_SizeChanged);
            this.groupBoxAllOrder.ResumeLayout(false);
            this.groupBoxAllOrder.PerformLayout();
            this.bindNavAllOrder.EndInit();
            this.bindNavAllOrder.ResumeLayout(false);
            this.bindNavAllOrder.PerformLayout();
            ((ISupportInitialize)this.dgAllOrder).EndInit();
            this.groupBoxBSAllOrder.ResumeLayout(false);
            this.groupBoxBSAllOrder.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private string OrderSql()
        {
            string str = " 1=1 ";
            string str2 = " and TransactionsCode<>'' ";
            string str3 = " and B_S<>''";
            if (this.comboCommodityAllOrder.SelectedIndex != 0)
            {
                str = str + " and CommodityID = '" + this.comboCommodityAllOrder.Text + "' ";
            }
            if (this.comboTrancAllOrder.SelectedIndex != 0)
            {
                str2 = " and TransactionsCode= '" + this.comboTrancAllOrder.Text + "' ";
            }
            else
            {
                str = str + str2;
            }
            if (this.Style == 1)
            {
                if (!this.isAllClicked)
                {
                    string str4 = str;
                    str = str4 + " and ( Status='" + Global.OrderStatusStrArr[1] + "' or Status='" + Global.OrderStatusStrArr[2] + "' ) ";
                }
            }
            else if (!this.radioAllAllOrder.Checked)
            {
                string str5 = str;
                str = str5 + " and ( Status='" + Global.OrderStatusStrArr[1] + "' or Status='" + Global.OrderStatusStrArr[2] + "' ) ";
            }
            if (this.comboB_SAllOrder.SelectedIndex == 1)
            {
                return (str + " and B_S='" + Global.BuySellStrArr[1] + "' ");
            }
            if (this.comboB_SAllOrder.SelectedIndex == 2)
            {
                return (str + " and B_S='" + Global.BuySellStrArr[2] + "' ");
            }
            return (str + str3);
        }

        private void QueryConditionChanged(object sender, EventArgs e)
        {
            if (!this.isFirstLoad)
            {
                if ((sender != null) && (sender is MyRadioButton))
                {
                    MyRadioButton button = (MyRadioButton)sender;
                    if (!button.Checked)
                    {
                        return;
                    }
                }
                string text = string.Empty;
                short selectedIndex = 0;
                string sql = this.OrderSql();
                short num2 = 0;
                if (this.comboCommodityAllOrder.SelectedIndex != 0)
                {
                    text = this.comboCommodityAllOrder.Text;
                }
                else
                {
                    text = string.Empty;
                }
                selectedIndex = (short)this.comboB_SAllOrder.SelectedIndex;
                if (this.Style == 1)
                {
                    if (this.isAllClicked)
                    {
                        num2 = 0;
                    }
                    else
                    {
                        num2 = 1;
                    }
                }
                else if (this.radioAllAllOrder.Checked)
                {
                    num2 = 0;
                }
                else
                {
                    num2 = 1;
                }
                this.operationManager.queryAllOrderOperation.ScreeningAllOrderData(text, selectedIndex, num2, sql);
            }
        }

        private void QueryPagingAllOrder()
        {
            this.operationManager.queryAllOrderOperation.QueryPageAllOrderData(this.buttonFlag, this.pageNum);
            this.AllOrderSetEnable(true);
            this.tSLblAllOrderPage.Text = this.operationManager.queryAllOrderOperation.AllOrderCurrentPage.ToString() + "/" + this.operationManager.queryAllOrderOperation.AllOrderAllPage.ToString();
        }

        private void SetBackGround()
        {
            try
            {
                for (int i = 0; i < this.dgAllOrder.Rows.Count; i++)
                {
                    string str = this.dgAllOrder.Rows[i].Cells["Status"].Value.ToString().Trim();
                    if (!str.Equals(Global.OrderStatusStrArr[1]) && !str.Equals(Global.OrderStatusStrArr[2]))
                    {
                        if (str == "")
                        {
                            if (i == (this.dgAllOrder.RowCount - 1))
                            {
                                this.dgAllOrder.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                                this.dgAllOrder.Rows[i].ReadOnly = true;
                            }
                            else
                            {
                                this.dgAllOrder.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                            }
                        }
                        else
                        {
                            this.dgAllOrder.Rows[i].DefaultCellStyle.BackColor = Color.Khaki;
                        }
                    }
                    else
                    {
                        this.dgAllOrder.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
                this.dgAllOrder.Columns["AutoID"].Visible = false;
            }
            catch (Exception)
            {
                Logger.wirte(MsgType.Error, "");
            }
        }

        private void SetBindNavLayOut(bool isShowPagingControl)
        {
            if (isShowPagingControl)
            {
                if (!this.bindNavAllOrder.Visible)
                {
                    this.bindNavAllOrder.Visible = true;
                    this.dgAllOrder.Height = this.dgAllOrderHeight - this.bindNavHeight;
                }
            }
            else if (this.bindNavAllOrder.Visible)
            {
                this.bindNavAllOrder.Visible = false;
                this.dgAllOrder.Height = this.dgAllOrderHeight;
            }
            this.tSLblAllOrderPage.Text = this.operationManager.queryAllOrderOperation.AllOrderCurrentPage.ToString() + "/" + this.operationManager.queryAllOrderOperation.AllOrderAllPage.ToString();
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboCommodityAllOrder.Items.Clear();
            this.comboCommodityAllOrder.Items.AddRange(commodityIDList.ToArray());
            this.comboCommodityAllOrder.SelectedIndex = 0;
            this.isFirstLoad = false;
        }

        private void SetControlText()
        {
            if (Global.CustomerCount < 2)
            {
                this.labelTrancAllOrder.Visible = false;
                this.comboTrancAllOrder.Visible = false;
                this.labelB_SAllOrder.Location = new Point(this.labelB_SAllOrder.Location.X - 150, this.labelB_SAllOrder.Location.Y);
                this.comboB_SAllOrder.Location = new Point(this.comboB_SAllOrder.Location.X - 150, this.comboB_SAllOrder.Location.Y);
                this.groupBoxBSAllOrder.Location = new Point(this.groupBoxBSAllOrder.Location.X - 150, this.groupBoxBSAllOrder.Location.Y);
                this.buttonSelAllOrder.Location = new Point(this.buttonSelAllOrder.Location.X - 150, this.buttonSelAllOrder.Location.Y);
                this.buttonAllAllOrder.Location = new Point(this.buttonAllAllOrder.Location.X - 150, this.buttonAllAllOrder.Location.Y);
                this.buttonCancelAllOrder.Location = new Point(this.buttonCancelAllOrder.Location.X - 150, this.buttonCancelAllOrder.Location.Y);
            }
            this.groupBoxAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF2");
            this.labelCommodityAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
            this.labelTrancAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
            this.radioAllAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_RadioAllF2");
            this.radioCancelAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_RadioCancelF2");
            this.buttonSelAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF2");
            this.buttonCancelAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonCancelF2");
            this.buttonSelAllOrder.TextAlign = ContentAlignment.TopCenter;
            this.buttonAllAllOrder.TextAlign = ContentAlignment.TopCenter;
            this.buttonCancelAllOrder.TextAlign = ContentAlignment.TopCenter;
            this.labelB_SAllOrder.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
            this.buttonAll.Text = Global.M_ResourceManager.GetString("TradeStr_All");
            this.buttonVoid.Text = Global.M_ResourceManager.GetString("TradeStr_RadioCancelF2");
            if (this.Style == 1)
            {
                this.buttonAll.Height = this.panelLeft.Height / 2;
                this.buttonVoid.Height = this.panelLeft.Height / 2;
                this.panelLeft.Visible = true;
                this.groupBoxBSAllOrder.Visible = false;
            }
            else
            {
                this.panelLeft.Visible = false;
                this.groupBoxBSAllOrder.Visible = true;
            }
            this.radioAllAllOrder.Checked = true;
            this.dgAllOrderHeight = this.dgAllOrder.Height;
            this.bindNavHeight = this.bindNavAllOrder.Height;
            this.buttonAll.Height = this.panelLeft.Height / 2;
            this.buttonVoid.Height = this.panelLeft.Height / 2;
            this.bindNavAllOrder.Visible = false;
        }

        private void SetDataGridViewHeader()
        {
            if (!this.isAllOrderHeaderLoad)
            {
                for (int i = 0; i < this.dgAllOrder.Columns.Count; i++)
                {
                    ColItemInfo info = (ColItemInfo)this.allOrderItemInfo.m_htItemInfo[this.dgAllOrder.Columns[i].Name];
                    if (info != null)
                    {
                        this.dgAllOrder.Columns[i].MinimumWidth = info.width;
                        this.dgAllOrder.Columns[i].FillWeight = info.width;
                        this.dgAllOrder.Columns[i].HeaderText = info.name;
                        this.dgAllOrder.Columns[i].DefaultCellStyle.Format = info.format;
                        this.dgAllOrder.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
                        if (info.sortID == 1)
                        {
                            this.dgAllOrder.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                        else
                        {
                            this.dgAllOrder.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (!this.allOrderItemInfo.m_strItems.Contains(this.dgAllOrder.Columns[i].Name))
                        {
                            this.dgAllOrder.Columns[i].Visible = false;
                        }
                        if (i == 0)
                        {
                            this.dgAllOrder.Columns[i].ReadOnly = false;
                        }
                        else
                        {
                            this.dgAllOrder.Columns[i].ReadOnly = true;
                        }
                    }
                }
                this.isAllOrderHeaderLoad = true;
            }
        }

        private void tSBtnAllOrderFirst_Click(object sender, EventArgs e)
        {
            this.buttonFlag = 0;
            this.QueryPagingAllOrder();
        }

        private void tSBtnAllOrderGO_Click(object sender, EventArgs e)
        {
            if (this.tSTbxAllOrderCurPage.Text.Trim().Length == 0)
            {
                MessageBox.Show(this.operationManager.InputPageNum, this.operationManager.Prompt, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.tSTbxAllOrderCurPage.Focus();
            }
            else
            {
                int num = int.Parse(this.tSTbxAllOrderCurPage.Text.Trim());
                if (num > 0)
                {
                    if (num != this.operationManager.queryAllOrderOperation.AllOrderCurrentPage)
                    {
                        if (num <= this.operationManager.queryAllOrderOperation.AllOrderAllPage)
                        {
                            this.buttonFlag = 4;
                            this.pageNum = num;
                            this.QueryPagingAllOrder();
                        }
                        else
                        {
                            MessageBox.Show(this.operationManager.InputPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            this.tSTbxAllOrderCurPage.Focus();
                            this.tSTbxAllOrderCurPage.SelectAll();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this.operationManager.InputPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.tSTbxAllOrderCurPage.Focus();
                    this.tSTbxAllOrderCurPage.SelectAll();
                }
            }
        }

        private void tSBtnAllOrderLast_Click(object sender, EventArgs e)
        {
            this.buttonFlag = 3;
            this.QueryPagingAllOrder();
        }

        private void tSBtnAllOrderNext_Click(object sender, EventArgs e)
        {
            this.buttonFlag = 2;
            this.QueryPagingAllOrder();
        }

        private void tSBtnAllOrderPrevious_Click(object sender, EventArgs e)
        {
            this.buttonFlag = 1;
            this.QueryPagingAllOrder();
        }

        private delegate void FillAllTradeOrder(DataTable dt, bool isShowPagingControl);
    }
}
