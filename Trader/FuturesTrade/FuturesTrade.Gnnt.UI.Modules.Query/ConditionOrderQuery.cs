using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
namespace FuturesTrade.Gnnt.UI.Modules.Query
{
	public class ConditionOrderQuery : UserControl
	{
		private delegate void FillConditionOrder(DataTable dt);
		private IContainer components;
		private GroupBox groupBoxConOrder;
		private CheckBox checkConditionAll;
		private Button BtnRefresh;
		private Button btnRevoke;
		private Label labComCode3;
		private Label labType2;
		private Label labState;
		private Label labBS2;
		private ComboBox comboBSQuery;
		private Label labOL2;
		internal ComboBox comboCommodityQuery;
		private ComboBox comboOLQuery;
		private ComboBox comboState;
		private ComboBox comboConTypeQuery;
		internal DataGridView dgAllConditionOrder;
		private BindingNavigator bindNavConOrder;
		private ToolStripButton tSBtnConOrderFirst;
		private ToolStripButton tSBtnConOrderPrevious;
		private ToolStripLabel toolStripLabel1;
		private ToolStripLabel tSLblConOrderPage;
		private ToolStripButton tSBtnConOrderNext;
		private ToolStripButton tSBtnConOrderLast;
		private ToolStripLabel tSBtnConOrderNum;
		private ToolStripTextBox tSTbxConOrderCurP;
		private ToolStripLabel tSLblConOrderP;
		private ToolStripButton tSBtnConOrderGO;
		private DataGridViewCheckBoxColumn SelectAll;
		private int dgConditionOrderHeight;
		private int bindNavHeight;
		private bool isHeaderLoad;
		private bool isFirstLoad = true;
		private ConditionOrderItemInfo conditionOrderItemInfo = new ConditionOrderItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private ConditionOrderQuery.FillConditionOrder fillConditionOrder;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ConditionOrderQuery));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.groupBoxConOrder = new GroupBox();
			this.checkConditionAll = new CheckBox();
			this.BtnRefresh = new Button();
			this.btnRevoke = new Button();
			this.labComCode3 = new Label();
			this.labType2 = new Label();
			this.labState = new Label();
			this.labBS2 = new Label();
			this.comboBSQuery = new ComboBox();
			this.labOL2 = new Label();
			this.comboCommodityQuery = new ComboBox();
			this.comboOLQuery = new ComboBox();
			this.comboState = new ComboBox();
			this.comboConTypeQuery = new ComboBox();
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
			((ISupportInitialize)this.bindNavConOrder).BeginInit();
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
			this.groupBoxConOrder.Size = new Size(820, 322);
			this.groupBoxConOrder.TabIndex = 5;
			this.groupBoxConOrder.TabStop = false;
			this.groupBoxConOrder.Text = "查询条件下单";
			this.checkConditionAll.BackColor = Color.Transparent;
			this.checkConditionAll.ImeMode = ImeMode.NoControl;
			this.checkConditionAll.Location = new Point(12, 51);
			this.checkConditionAll.Name = "checkConditionAll";
			this.checkConditionAll.Size = new Size(14, 16);
			this.checkConditionAll.TabIndex = 0;
			this.checkConditionAll.UseVisualStyleBackColor = false;
			this.checkConditionAll.CheckedChanged += new EventHandler(this.checkConditionAll_CheckedChanged);
			this.BtnRefresh.ImeMode = ImeMode.NoControl;
			this.BtnRefresh.Location = new Point(744, 15);
			this.BtnRefresh.Name = "BtnRefresh";
			this.BtnRefresh.Size = new Size(50, 23);
			this.BtnRefresh.TabIndex = 37;
			this.BtnRefresh.Text = "刷新";
			this.BtnRefresh.UseVisualStyleBackColor = true;
			this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
			this.btnRevoke.ImeMode = ImeMode.NoControl;
			this.btnRevoke.Location = new Point(665, 15);
			this.btnRevoke.Name = "btnRevoke";
			this.btnRevoke.Size = new Size(75, 23);
			this.btnRevoke.TabIndex = 18;
			this.btnRevoke.Text = "撤所选单";
			this.btnRevoke.UseVisualStyleBackColor = true;
			this.btnRevoke.Click += new EventHandler(this.btnRevoke_Click);
			this.labComCode3.AutoSize = true;
			this.labComCode3.ImeMode = ImeMode.NoControl;
			this.labComCode3.Location = new Point(8, 21);
			this.labComCode3.Name = "labComCode3";
			this.labComCode3.Size = new Size(53, 12);
			this.labComCode3.TabIndex = 0;
			this.labComCode3.Text = "品种代码";
			this.labType2.AutoSize = true;
			this.labType2.ImeMode = ImeMode.NoControl;
			this.labType2.Location = new Point(354, 21);
			this.labType2.Name = "labType2";
			this.labType2.Size = new Size(65, 12);
			this.labType2.TabIndex = 0;
			this.labType2.Text = "条件类型：";
			this.labState.AutoSize = true;
			this.labState.ImeMode = ImeMode.NoControl;
			this.labState.Location = new Point(509, 21);
			this.labState.Name = "labState";
			this.labState.Size = new Size(65, 12);
			this.labState.TabIndex = 0;
			this.labState.Text = "委托状态：";
			this.labBS2.AutoSize = true;
			this.labBS2.ImeMode = ImeMode.NoControl;
			this.labBS2.Location = new Point(150, 21);
			this.labBS2.Name = "labBS2";
			this.labBS2.Size = new Size(29, 12);
			this.labBS2.TabIndex = 0;
			this.labBS2.Text = "买卖";
			this.comboBSQuery.FormattingEnabled = true;
			this.comboBSQuery.Items.AddRange(new object[]
			{
				"全部",
				"买入",
				"卖出"
			});
			this.comboBSQuery.Location = new Point(184, 17);
			this.comboBSQuery.Name = "comboBSQuery";
			this.comboBSQuery.Size = new Size(61, 20);
			this.comboBSQuery.TabIndex = 13;
			this.comboBSQuery.Text = "全部";
			this.comboBSQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.labOL2.AutoSize = true;
			this.labOL2.ImeMode = ImeMode.NoControl;
			this.labOL2.Location = new Point(250, 21);
			this.labOL2.Name = "labOL2";
			this.labOL2.Size = new Size(29, 12);
			this.labOL2.TabIndex = 0;
			this.labOL2.Text = "订转";
			this.comboCommodityQuery.FormattingEnabled = true;
			this.comboCommodityQuery.Location = new Point(65, 17);
			this.comboCommodityQuery.Name = "comboCommodityQuery";
			this.comboCommodityQuery.Size = new Size(80, 20);
			this.comboCommodityQuery.TabIndex = 12;
			this.comboCommodityQuery.Text = "全部";
			this.comboCommodityQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.comboOLQuery.FormattingEnabled = true;
			this.comboOLQuery.Items.AddRange(new object[]
			{
				"全部",
				"订立",
				"转让"
			});
			this.comboOLQuery.Location = new Point(286, 17);
			this.comboOLQuery.Name = "comboOLQuery";
			this.comboOLQuery.Size = new Size(61, 20);
			this.comboOLQuery.TabIndex = 14;
			this.comboOLQuery.Text = "全部";
			this.comboOLQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.comboState.FormattingEnabled = true;
			this.comboState.Items.AddRange(new object[]
			{
				"全部",
				"未委托",
				"已过期",
				"委托成功",
				"委托失败",
				"已撤单"
			});
			this.comboState.Location = new Point(580, 17);
			this.comboState.Name = "comboState";
			this.comboState.Size = new Size(78, 20);
			this.comboState.TabIndex = 16;
			this.comboState.Text = "全部";
			this.comboState.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.comboConTypeQuery.FormattingEnabled = true;
			this.comboConTypeQuery.Items.AddRange(new object[]
			{
				"全部",
				"申买价",
				"申卖价",
				"最新价"
			});
			this.comboConTypeQuery.Location = new Point(425, 17);
			this.comboConTypeQuery.Name = "comboConTypeQuery";
			this.comboConTypeQuery.Size = new Size(78, 20);
			this.comboConTypeQuery.TabIndex = 15;
			this.comboConTypeQuery.Text = "全部";
			this.comboConTypeQuery.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.bindNavConOrder.AddNewItem = null;
			this.bindNavConOrder.BackColor = Color.Gainsboro;
			this.bindNavConOrder.CountItem = null;
			this.bindNavConOrder.DeleteItem = null;
			this.bindNavConOrder.Dock = DockStyle.Bottom;
			this.bindNavConOrder.Items.AddRange(new ToolStripItem[]
			{
				this.tSBtnConOrderFirst,
				this.tSBtnConOrderPrevious,
				this.toolStripLabel1,
				this.tSLblConOrderPage,
				this.tSBtnConOrderNext,
				this.tSBtnConOrderLast,
				this.tSBtnConOrderNum,
				this.tSTbxConOrderCurP,
				this.tSLblConOrderP,
				this.tSBtnConOrderGO
			});
			this.bindNavConOrder.LayoutStyle = ToolStripLayoutStyle.Flow;
			this.bindNavConOrder.Location = new Point(3, 295);
			this.bindNavConOrder.MoveFirstItem = null;
			this.bindNavConOrder.MoveLastItem = null;
			this.bindNavConOrder.MoveNextItem = null;
			this.bindNavConOrder.MovePreviousItem = null;
			this.bindNavConOrder.Name = "bindNavConOrder";
			this.bindNavConOrder.PositionItem = null;
			this.bindNavConOrder.RenderMode = ToolStripRenderMode.System;
			this.bindNavConOrder.Size = new Size(814, 24);
			this.bindNavConOrder.TabIndex = 38;
			this.bindNavConOrder.Text = "bindingNavigator1";
			this.tSBtnConOrderFirst.AutoSize = false;
			this.tSBtnConOrderFirst.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnConOrderFirst.BackgroundImage");
			this.tSBtnConOrderFirst.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnConOrderFirst.Enabled = false;
			this.tSBtnConOrderFirst.ImageTransparentColor = Color.Magenta;
			this.tSBtnConOrderFirst.Name = "tSBtnConOrderFirst";
			this.tSBtnConOrderFirst.Size = new Size(16, 16);
			this.tSBtnConOrderFirst.ToolTipText = "第一页";
			this.tSBtnConOrderFirst.Click += new EventHandler(this.tSBtnConOrderFirst_Click);
			this.tSBtnConOrderPrevious.AutoSize = false;
			this.tSBtnConOrderPrevious.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnConOrderPrevious.BackgroundImage");
			this.tSBtnConOrderPrevious.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnConOrderPrevious.Enabled = false;
			this.tSBtnConOrderPrevious.Image = (Image)componentResourceManager.GetObject("tSBtnConOrderPrevious.Image");
			this.tSBtnConOrderPrevious.ImageTransparentColor = Color.Magenta;
			this.tSBtnConOrderPrevious.Name = "tSBtnConOrderPrevious";
			this.tSBtnConOrderPrevious.Size = new Size(16, 16);
			this.tSBtnConOrderPrevious.ToolTipText = "上一页";
			this.tSBtnConOrderPrevious.Click += new EventHandler(this.tSBtnConOrderPrevious_Click);
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new Size(0, 0);
			this.tSLblConOrderPage.AutoSize = false;
			this.tSLblConOrderPage.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSLblConOrderPage.ImageTransparentColor = Color.Magenta;
			this.tSLblConOrderPage.Name = "tSLblConOrderPage";
			this.tSLblConOrderPage.Size = new Size(95, 16);
			this.tSLblConOrderPage.Text = "toolStripLabel2";
			this.tSBtnConOrderNext.AutoSize = false;
			this.tSBtnConOrderNext.BackColor = Color.Gainsboro;
			this.tSBtnConOrderNext.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnConOrderNext.BackgroundImage");
			this.tSBtnConOrderNext.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnConOrderNext.ImageTransparentColor = Color.Magenta;
			this.tSBtnConOrderNext.Name = "tSBtnConOrderNext";
			this.tSBtnConOrderNext.Size = new Size(16, 16);
			this.tSBtnConOrderNext.ToolTipText = "下一页";
			this.tSBtnConOrderNext.Click += new EventHandler(this.tSBtnConOrderNext_Click);
			this.tSBtnConOrderLast.AutoSize = false;
			this.tSBtnConOrderLast.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnConOrderLast.BackgroundImage");
			this.tSBtnConOrderLast.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnConOrderLast.ImageTransparentColor = Color.Magenta;
			this.tSBtnConOrderLast.Name = "tSBtnConOrderLast";
			this.tSBtnConOrderLast.Size = new Size(16, 16);
			this.tSBtnConOrderLast.Click += new EventHandler(this.tSBtnConOrderLast_Click);
			this.tSBtnConOrderNum.AutoSize = false;
			this.tSBtnConOrderNum.Name = "tSBtnConOrderNum";
			this.tSBtnConOrderNum.Size = new Size(17, 16);
			this.tSBtnConOrderNum.Text = "第";
			this.tSTbxConOrderCurP.AutoSize = false;
			this.tSTbxConOrderCurP.BackColor = Color.Snow;
			this.tSTbxConOrderCurP.BorderStyle = BorderStyle.None;
			this.tSTbxConOrderCurP.Name = "tSTbxConOrderCurP";
			this.tSTbxConOrderCurP.Size = new Size(50, 16);
			this.tSTbxConOrderCurP.TextBoxTextAlign = HorizontalAlignment.Center;
			this.tSLblConOrderP.AutoSize = false;
			this.tSLblConOrderP.Name = "tSLblConOrderP";
			this.tSLblConOrderP.Size = new Size(17, 16);
			this.tSLblConOrderP.Text = "页";
			this.tSBtnConOrderGO.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnConOrderGO.ImageTransparentColor = Color.Magenta;
			this.tSBtnConOrderGO.Name = "tSBtnConOrderGO";
			this.tSBtnConOrderGO.Size = new Size(31, 21);
			this.tSBtnConOrderGO.Text = "GO";
			this.tSBtnConOrderGO.Click += new EventHandler(this.tSBtnConOrderGO_Click);
			this.dgAllConditionOrder.AllowUserToAddRows = false;
			this.dgAllConditionOrder.AllowUserToDeleteRows = false;
			this.dgAllConditionOrder.AllowUserToOrderColumns = true;
			this.dgAllConditionOrder.AllowUserToResizeRows = false;
			this.dgAllConditionOrder.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgAllConditionOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgAllConditionOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgAllConditionOrder.Columns.AddRange(new DataGridViewColumn[]
			{
				this.SelectAll
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgAllConditionOrder.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgAllConditionOrder.Location = new Point(3, 47);
			this.dgAllConditionOrder.Name = "dgAllConditionOrder";
			this.dgAllConditionOrder.RowHeadersVisible = false;
			this.dgAllConditionOrder.RowTemplate.Height = 20;
			this.dgAllConditionOrder.Size = new Size(814, 269);
			this.dgAllConditionOrder.TabIndex = 39;
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
			base.Size = new Size(820, 322);
			base.Load += new EventHandler(this.ConditionOrder_Load);
			base.SizeChanged += new EventHandler(this.ConditionOrderQuery_SizeChanged);
			this.groupBoxConOrder.ResumeLayout(false);
			this.groupBoxConOrder.PerformLayout();
			((ISupportInitialize)this.bindNavConOrder).EndInit();
			this.bindNavConOrder.ResumeLayout(false);
			this.bindNavConOrder.PerformLayout();
			((ISupportInitialize)this.dgAllConditionOrder).EndInit();
			base.ResumeLayout(false);
		}
		public ConditionOrderQuery()
		{
			this.InitializeComponent();
			this.operationManager.queryConOrderOperation.ConOrderFill = new QueryConOrderOperation.ConOrderFillCallBack(this.ConditionOrderInfoFill);
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.CreateHandle();
		}
		private void ConditionOrder_Load(object sender, EventArgs e)
		{
			this.CommodityInfoLoad();
			this.SetControlText();
		}
		private void CommodityInfoLoad()
		{
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
			string @string = Global.M_ResourceManager.GetString("TradeStr_All");
			this.comboBSQuery.Text = @string;
			this.comboOLQuery.Text = @string;
			this.comboConTypeQuery.Text = @string;
			this.comboState.Text = @string;
			this.comboBSQuery.Items.Add(@string);
			this.comboOLQuery.Items.Add(@string);
			this.comboConTypeQuery.Items.Add(@string);
			this.comboState.Items.Add(@string);
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
		private void ConditionOrderInfoFill(DataTable dTable, bool isPaging)
		{
			this.fillConditionOrder = new ConditionOrderQuery.FillConditionOrder(this.dsConOrderFill);
			this.HandleCreated();
			base.Invoke(this.fillConditionOrder, new object[]
			{
				dTable
			});
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void dsConOrderFill(DataTable dt)
		{
			this.dgAllConditionOrder.DataSource = dt;
			this.SetDataGridViewHeader();
			this.SetBindNavLayOut();
		}
		private void SetDataGridViewHeader()
		{
			if (!this.isHeaderLoad)
			{
				for (int i = 0; i < this.dgAllConditionOrder.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.conditionOrderItemInfo.m_htItemInfo[this.dgAllConditionOrder.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgAllConditionOrder.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgAllConditionOrder.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgAllConditionOrder.Columns[i].HeaderText = colItemInfo.name;
						this.dgAllConditionOrder.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						this.dgAllConditionOrder.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
						if (colItemInfo.sortID == 1)
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
			else
			{
				if (this.bindNavConOrder.Visible)
				{
					this.bindNavConOrder.Visible = false;
					this.dgAllConditionOrder.Height = this.dgConditionOrderHeight;
				}
			}
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void ConditionOrderSetEnable(bool isEnable)
		{
			if (this.operationManager.queryConOrderOperation.ConOrderCurrentPage == this.operationManager.queryConOrderOperation.ConOrderAllPage)
			{
				this.tSBtnConOrderFirst.Enabled = isEnable;
				this.tSBtnConOrderPrevious.Enabled = isEnable;
				this.tSBtnConOrderNext.Enabled = !isEnable;
				this.tSBtnConOrderLast.Enabled = !isEnable;
				return;
			}
			if (this.operationManager.queryConOrderOperation.ConOrderCurrentPage == 1)
			{
				this.tSBtnConOrderFirst.Enabled = !isEnable;
				this.tSBtnConOrderPrevious.Enabled = !isEnable;
				this.tSBtnConOrderNext.Enabled = isEnable;
				this.tSBtnConOrderLast.Enabled = isEnable;
				return;
			}
			this.tSBtnConOrderFirst.Enabled = isEnable;
			this.tSBtnConOrderPrevious.Enabled = isEnable;
			this.tSBtnConOrderNext.Enabled = isEnable;
			this.tSBtnConOrderLast.Enabled = isEnable;
		}
		private void tSBtnConOrderFirst_Click(object sender, EventArgs e)
		{
			this.operationManager.queryConOrderOperation.QueryPageConOrderData(0, 0);
			this.ConditionOrderSetEnable(true);
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void tSBtnConOrderPrevious_Click(object sender, EventArgs e)
		{
			this.operationManager.queryConOrderOperation.QueryPageConOrderData(1, 0);
			this.ConditionOrderSetEnable(true);
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void tSBtnConOrderNext_Click(object sender, EventArgs e)
		{
			this.operationManager.queryConOrderOperation.QueryPageConOrderData(2, 0);
			this.ConditionOrderSetEnable(true);
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void tSBtnConOrderLast_Click(object sender, EventArgs e)
		{
			this.operationManager.queryConOrderOperation.QueryPageConOrderData(3, 0);
			this.ConditionOrderSetEnable(true);
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void tSBtnConOrderGO_Click(object sender, EventArgs e)
		{
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPageNum");
			string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_PageNumError");
			if (this.tSTbxConOrderCurP.Text.Trim().Length == 0)
			{
				string string3 = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPageNum");
				string string4 = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_ErrorInfo");
				MessageBox.Show(string3, string4, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					MessageBox.Show(@string, string2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					this.tSTbxConOrderCurP.Focus();
					this.tSTbxConOrderCurP.SelectAll();
				}
			}
			this.tSLblConOrderPage.Text = this.operationManager.queryConOrderOperation.ConOrderCurrentPage.ToString() + "/" + this.operationManager.queryConOrderOperation.ConOrderAllPage.ToString();
		}
		private void dgAllConditionOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				return;
			}
			this.operationManager.queryConOrderOperation.ConOrderDataGridViewSort(this.dgAllConditionOrder.Columns[e.ColumnIndex].Name.ToString());
		}
		private void QueryConditionChanged(object sender, EventArgs e)
		{
			if (!this.isFirstLoad)
			{
				string commodityID = string.Empty;
				short buySellType = 0;
				short settleBasis = 0;
				short conditionType = 0;
				string sql = this.OrderSql();
				if (this.comboCommodityQuery.SelectedIndex != 0)
				{
					commodityID = this.comboCommodityQuery.Text;
				}
				if (this.comboBSQuery.SelectedIndex != 0)
				{
					buySellType = (short)this.comboBSQuery.SelectedIndex;
				}
				string text = this.comboState.Text;
				if (this.comboConTypeQuery.SelectedIndex != 0)
				{
					conditionType = (short)this.comboConTypeQuery.SelectedIndex;
				}
				if (sender != null && sender is RadioButton)
				{
					RadioButton radioButton = (RadioButton)sender;
					if (!radioButton.Checked)
					{
						return;
					}
				}
				this.operationManager.queryConOrderOperation.ScreeningConOrderData(commodityID, buySellType, text, settleBasis, conditionType, sql);
			}
		}
		private string OrderSql()
		{
			string text = "1=1";
			if (this.comboCommodityQuery.SelectedIndex != 0)
			{
				text = text + " and commodityID = '" + this.comboCommodityQuery.Text + "'";
			}
			if (this.comboBSQuery.SelectedIndex != 0)
			{
				if (this.comboBSQuery.SelectedIndex == 1)
				{
					text = text + " and B_S ='" + Global.BuySellStrArr[1] + "'";
				}
				else
				{
					if (this.comboBSQuery.SelectedIndex == 2)
					{
						text = text + " and B_S ='" + Global.BuySellStrArr[2] + "'";
					}
				}
			}
			if (this.comboOLQuery.SelectedIndex != 0)
			{
				if (this.comboOLQuery.SelectedIndex == 1)
				{
					text = text + " and O_L ='" + Global.SettleBasisStrArr[1] + "'";
				}
				else
				{
					if (this.comboOLQuery.SelectedIndex == 2)
					{
						text = text + " and O_L ='" + Global.SettleBasisStrArr[2] + "'";
					}
				}
			}
			if (this.comboConTypeQuery.SelectedIndex != 0)
			{
				if (this.comboConTypeQuery.SelectedIndex == 1)
				{
					text = text + " and CoditionType ='" + Global.ConditionTypeStrArr[2] + "'";
				}
				else
				{
					if (this.comboConTypeQuery.SelectedIndex == 2)
					{
						text = text + " and CoditionType ='" + Global.ConditionTypeStrArr[3] + "'";
					}
					else
					{
						if (this.comboConTypeQuery.SelectedIndex == 3)
						{
							text = text + " and CoditionType ='" + Global.ConditionTypeStrArr[1] + "'";
						}
					}
				}
			}
			if (this.comboState.SelectedIndex != 0)
			{
				if (this.comboState.SelectedIndex == 1)
				{
					text = text + " and OrderState ='" + Global.OrderTypeStrArr[1] + "'";
				}
				else
				{
					if (this.comboState.SelectedIndex == 2)
					{
						text = text + " and OrderState ='" + Global.OrderTypeStrArr[2] + "'";
					}
					else
					{
						if (this.comboState.SelectedIndex == 3)
						{
							text = text + " and OrderState ='" + Global.OrderTypeStrArr[3] + "'";
						}
						else
						{
							if (this.comboState.SelectedIndex == 4)
							{
								text = text + "and OrderState ='" + Global.OrderTypeStrArr[4] + "'";
							}
							else
							{
								if (this.comboState.SelectedIndex == 5)
								{
									text = text + " and OrderState ='" + Global.OrderTypeStrArr[5] + "'";
								}
							}
						}
					}
				}
			}
			return text;
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
				return;
			}
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
		private void dgAllConditionOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
			if (this.dgAllConditionOrder.RowCount > 1 && this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].Cells["commodityID"].Value.ToString().Trim() == @string)
			{
				this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgAllConditionOrder.Rows[this.dgAllConditionOrder.RowCount - 1].ReadOnly = true;
			}
			try
			{
				this.dgAllConditionOrder.Columns["AutoID"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
		}
		public void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboCommodityQuery.Items.Clear();
			this.comboCommodityQuery.Items.AddRange(commodityIDList.ToArray());
			this.comboCommodityQuery.SelectedIndex = 0;
			this.isFirstLoad = false;
		}
		private void ConditionOrderQuery_SizeChanged(object sender, EventArgs e)
		{
			this.dgConditionOrderHeight = this.dgAllConditionOrder.Height;
			this.bindNavHeight = this.bindNavConOrder.Height;
		}
		private void BtnRefresh_Click(object sender, EventArgs e)
		{
			this.operationManager.queryConOrderOperation.ButtonRefreshFlag = 1;
			this.operationManager.queryConOrderOperation.QueryConOrderInfoLoad();
			this.operationManager.IdleRefreshButton = 0;
		}
		private void btnRevoke_Click(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			string str = string.Empty;
			string text = string.Empty;
			for (int i = this.dgAllConditionOrder.Rows.Count - 1; i >= 0; i--)
			{
				if (this.dgAllConditionOrder["SelectAll", i].Value != null && (bool)this.dgAllConditionOrder["SelectAll", i].Value)
				{
					if (this.dgAllConditionOrder.Rows[i].Cells["OrderState"].Value.Equals(Global.OrderTypeStrArr[1]))
					{
						string text2 = this.dgAllConditionOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
						str = str + "-" + text2;
						list.Add(text2);
					}
					else
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += this.dgAllConditionOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
					}
				}
			}
			if (!text.Equals(""))
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_ErrorOrderStr");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_ErrorOrderMessage");
				string message = string.Format(@string, text);
				Logger.wirte(MsgType.Error, "条件下单委托单号:【" + text + "】不可撤销");
				MessageForm messageForm = new MessageForm(string2, message, 1);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				return;
			}
			if (list.Count > 0)
			{
				int count = list.Count;
				string string3 = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrders");
				string string4 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_RevokeOrdersMessage");
				MessageForm messageForm = new MessageForm(string3, string.Format(string4, count), 0);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_DataSubmiting");
					this.operationManager.revokeConOrderOperation.RevokeConOrderThread(list);
					this.checkConditionAll.Checked = false;
					return;
				}
			}
			else
			{
				string string5 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_CancelMessageContent");
				string string6 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_CancelMessageTitle");
				MessageForm messageForm = new MessageForm(string6, string5, 1);
				WriteLog.WriteMsg("请至少选择一笔条件下单委托后撤单！");
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
	}
}
