using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation;
using FuturesTrade.Gnnt.Library;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
namespace FuturesTrade.Gnnt.UI.Modules.Query
{
	public class TradeOrder : UserControl
	{
		private delegate void FillTradeOrderInfo(DataTable dt, bool isShowPagingControl);
		private byte buttonFlag;
		private int pageNum;
		private bool isTradeOrderHeaderLoad;
		private TradeOrderItemInfo tradeOrderItemInfo = new TradeOrderItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private int dgTradeOrderHeight;
		private int bindNavHeight;
		private TradeOrder.FillTradeOrderInfo fillTradeOrder;
		private IContainer components;
		internal GroupBox groupBoxTradeOrder;
		internal DataGridView dgTradeOrder;
		internal BindingNavigator bindNavTradeOrder;
		private ToolStripButton tSBtnTradeOrderFirst;
		private ToolStripButton tSBtnTradeOrderPrevious;
		private ToolStripLabel tSLblTradeOrderPage;
		private ToolStripButton tSBtnTradeOrderNext;
		private ToolStripButton tSBtnTradeOrderLast;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripLabel tSLblTradeOrderNum;
		private ToolStripTextBox tSTbxTradeOrderCurPage;
		private ToolStripLabel tSLblTradeOrderP;
		private ToolStripButton tSBtnTradeOrderGO;
		public TradeOrder()
		{
			this.InitializeComponent();
			this.operationManager.queryTradeOrderOperation.TradeOrderFill = new QueryTradeOrderOperation.TradeOrderFillCallBack(this.TradeOrderInfoFill);
			this.CreateHandle();
		}
		private void TradeOrderInfoFill(DataTable dTable, bool isPage)
		{
			try
			{
				this.fillTradeOrder = new TradeOrder.FillTradeOrderInfo(this.dsTradeOrderFill);
				this.HandleCreated();
				base.Invoke(this.fillTradeOrder, new object[]
				{
					dTable,
					isPage
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void dsTradeOrderFill(DataTable dt, bool isShowPagingControl)
		{
			this.dgTradeOrder.DataSource = dt;
			this.SetDataGridViewHeader();
			this.SetBindNavLayOut(isShowPagingControl);
		}
		private void SetDataGridViewHeader()
		{
			if (!this.isTradeOrderHeaderLoad)
			{
				for (int i = 0; i < this.dgTradeOrder.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.tradeOrderItemInfo.m_htItemInfo[this.dgTradeOrder.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgTradeOrder.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgTradeOrder.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgTradeOrder.Columns[i].HeaderText = colItemInfo.name;
						this.dgTradeOrder.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						this.dgTradeOrder.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
						if (colItemInfo.sortID == 1)
						{
							this.dgTradeOrder.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgTradeOrder.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.tradeOrderItemInfo.m_strItems.Contains(this.dgTradeOrder.Columns[i].Name))
						{
							this.dgTradeOrder.Columns[i].Visible = false;
						}
						if (i == 0)
						{
							this.dgTradeOrder.Columns[i].ReadOnly = false;
						}
						else
						{
							this.dgTradeOrder.Columns[i].ReadOnly = true;
						}
					}
				}
				this.isTradeOrderHeaderLoad = true;
			}
		}
		private void SetBindNavLayOut(bool isShowPagingControl)
		{
			if (isShowPagingControl)
			{
				if (!this.bindNavTradeOrder.Visible)
				{
					this.bindNavTradeOrder.Visible = true;
					this.dgTradeOrder.Height = this.dgTradeOrderHeight - this.bindNavHeight;
				}
			}
			else
			{
				if (this.bindNavTradeOrder.Visible)
				{
					this.bindNavTradeOrder.Visible = false;
					this.dgTradeOrder.Height = this.dgTradeOrderHeight;
				}
			}
			this.tSLblTradeOrderPage.Text = this.operationManager.queryTradeOrderOperation.TradeOrderCurrentPage.ToString() + "/" + this.operationManager.queryTradeOrderOperation.TradeOrderAllPage.ToString();
		}
		private void TradeOrderSetEnable(bool isEnable)
		{
			if (this.operationManager.queryTradeOrderOperation.TradeOrderCurrentPage == 1)
			{
				this.tSBtnTradeOrderFirst.Enabled = !isEnable;
				this.tSBtnTradeOrderPrevious.Enabled = !isEnable;
				this.tSBtnTradeOrderNext.Enabled = isEnable;
				this.tSBtnTradeOrderLast.Enabled = isEnable;
				return;
			}
			if (this.operationManager.queryTradeOrderOperation.TradeOrderCurrentPage == this.operationManager.queryTradeOrderOperation.TradeOrderAllPage)
			{
				this.tSBtnTradeOrderFirst.Enabled = isEnable;
				this.tSBtnTradeOrderPrevious.Enabled = isEnable;
				this.tSBtnTradeOrderNext.Enabled = !isEnable;
				this.tSBtnTradeOrderLast.Enabled = !isEnable;
				return;
			}
			this.tSBtnTradeOrderFirst.Enabled = isEnable;
			this.tSBtnTradeOrderPrevious.Enabled = isEnable;
			this.tSBtnTradeOrderNext.Enabled = isEnable;
			this.tSBtnTradeOrderLast.Enabled = isEnable;
		}
		private void tSBtnTradeOrderFirst_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 0;
			this.QueryPagingTradeOrder();
		}
		private void tSBtnTradeOrderPrevious_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 1;
			this.QueryPagingTradeOrder();
		}
		private void tSBtnTradeOrderNext_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 2;
			this.QueryPagingTradeOrder();
		}
		private void tSBtnTradeOrderLast_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 3;
			this.QueryPagingTradeOrder();
		}
		private void tSBtnTradeOrderGO_Click(object sender, EventArgs e)
		{
			if (this.tSTbxTradeOrderCurPage.Text.Trim().Length == 0)
			{
				MessageBox.Show(this.operationManager.InputPageNum, this.operationManager.Prompt, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.tSTbxTradeOrderCurPage.Focus();
				return;
			}
			int num = int.Parse(this.tSTbxTradeOrderCurPage.Text.Trim());
			if (num <= 0)
			{
				MessageBox.Show(this.operationManager.InputRightPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.tSTbxTradeOrderCurPage.Focus();
				this.tSTbxTradeOrderCurPage.SelectAll();
				return;
			}
			if (num == this.operationManager.queryTradeOrderOperation.TradeOrderCurrentPage)
			{
				return;
			}
			if (num <= this.operationManager.queryTradeOrderOperation.TradeOrderAllPage)
			{
				this.buttonFlag = 4;
				this.pageNum = num;
				this.QueryPagingTradeOrder();
				return;
			}
			MessageBox.Show(this.operationManager.InputRightPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			this.tSTbxTradeOrderCurPage.Focus();
			this.tSTbxTradeOrderCurPage.SelectAll();
		}
		private void QueryPagingTradeOrder()
		{
			this.operationManager.queryTradeOrderOperation.QueryPageTradeOrderData(this.buttonFlag, this.pageNum);
			this.TradeOrderSetEnable(true);
			this.tSLblTradeOrderPage.Text = this.operationManager.queryTradeOrderOperation.TradeOrderCurrentPage.ToString() + "/" + this.operationManager.queryTradeOrderOperation.TradeOrderAllPage.ToString();
		}
		private void dgTradeOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			string tradeOrderSortName = this.dgTradeOrder.Columns[e.ColumnIndex].Name.ToString();
			this.operationManager.queryTradeOrderOperation.TradeOrderDataGridViewSort(tradeOrderSortName);
		}
		private void dgTradeOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (this.dgTradeOrder.RowCount > 1 && this.dgTradeOrder.Rows[this.dgTradeOrder.RowCount - 1].Cells["Time"].Value.ToString().Trim() == "合计")
			{
				this.dgTradeOrder.Rows[this.dgTradeOrder.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgTradeOrder.Rows[this.dgTradeOrder.RowCount - 1].ReadOnly = true;
			}
			Global.BSAlign(this.dgTradeOrder);
		}
		private void TradeOrder_SizeChanged(object sender, EventArgs e)
		{
			this.dgTradeOrderHeight = this.dgTradeOrder.Height;
			this.bindNavHeight = this.bindNavTradeOrder.Height;
		}
		private void TradeOrder_Load(object sender, EventArgs e)
		{
			this.bindNavTradeOrder.Visible = false;
		}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TradeOrder));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			this.groupBoxTradeOrder = new GroupBox();
			this.bindNavTradeOrder = new BindingNavigator(this.components);
			this.tSBtnTradeOrderFirst = new ToolStripButton();
			this.tSBtnTradeOrderPrevious = new ToolStripButton();
			this.tSLblTradeOrderPage = new ToolStripLabel();
			this.tSBtnTradeOrderNext = new ToolStripButton();
			this.tSBtnTradeOrderLast = new ToolStripButton();
			this.toolStripSeparator4 = new ToolStripSeparator();
			this.tSLblTradeOrderNum = new ToolStripLabel();
			this.tSTbxTradeOrderCurPage = new ToolStripTextBox();
			this.tSLblTradeOrderP = new ToolStripLabel();
			this.tSBtnTradeOrderGO = new ToolStripButton();
			this.dgTradeOrder = new DataGridView();
			this.groupBoxTradeOrder.SuspendLayout();
			((ISupportInitialize)this.bindNavTradeOrder).BeginInit();
			this.bindNavTradeOrder.SuspendLayout();
			((ISupportInitialize)this.dgTradeOrder).BeginInit();
			base.SuspendLayout();
			this.groupBoxTradeOrder.BackColor = Color.Transparent;
			this.groupBoxTradeOrder.Controls.Add(this.bindNavTradeOrder);
			this.groupBoxTradeOrder.Controls.Add(this.dgTradeOrder);
			this.groupBoxTradeOrder.Dock = DockStyle.Fill;
			this.groupBoxTradeOrder.Location = new Point(0, 0);
			this.groupBoxTradeOrder.Margin = new Padding(0);
			this.groupBoxTradeOrder.Name = "groupBoxTradeOrder";
			this.groupBoxTradeOrder.Padding = new Padding(3, 0, 3, 3);
			this.groupBoxTradeOrder.Size = new Size(700, 180);
			this.groupBoxTradeOrder.TabIndex = 0;
			this.groupBoxTradeOrder.TabStop = false;
			this.groupBoxTradeOrder.Text = "已成交委托";
			this.bindNavTradeOrder.AddNewItem = null;
			this.bindNavTradeOrder.AutoSize = false;
			this.bindNavTradeOrder.BackColor = Color.Gainsboro;
			this.bindNavTradeOrder.CountItem = null;
			this.bindNavTradeOrder.DeleteItem = null;
			this.bindNavTradeOrder.Dock = DockStyle.Bottom;
			this.bindNavTradeOrder.Items.AddRange(new ToolStripItem[]
			{
				this.tSBtnTradeOrderFirst,
				this.tSBtnTradeOrderPrevious,
				this.tSLblTradeOrderPage,
				this.tSBtnTradeOrderNext,
				this.tSBtnTradeOrderLast,
				this.toolStripSeparator4,
				this.tSLblTradeOrderNum,
				this.tSTbxTradeOrderCurPage,
				this.tSLblTradeOrderP,
				this.tSBtnTradeOrderGO
			});
			this.bindNavTradeOrder.LayoutStyle = ToolStripLayoutStyle.Flow;
			this.bindNavTradeOrder.Location = new Point(3, 157);
			this.bindNavTradeOrder.MoveFirstItem = null;
			this.bindNavTradeOrder.MoveLastItem = null;
			this.bindNavTradeOrder.MoveNextItem = null;
			this.bindNavTradeOrder.MovePreviousItem = null;
			this.bindNavTradeOrder.Name = "bindNavTradeOrder";
			this.bindNavTradeOrder.PositionItem = null;
			this.bindNavTradeOrder.RenderMode = ToolStripRenderMode.System;
			this.bindNavTradeOrder.Size = new Size(694, 20);
			this.bindNavTradeOrder.TabIndex = 16;
			this.bindNavTradeOrder.Text = "bindingNavigator2";
			this.tSBtnTradeOrderFirst.AutoSize = false;
			this.tSBtnTradeOrderFirst.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnTradeOrderFirst.BackgroundImage");
			this.tSBtnTradeOrderFirst.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnTradeOrderFirst.Enabled = false;
			this.tSBtnTradeOrderFirst.ImageTransparentColor = Color.Magenta;
			this.tSBtnTradeOrderFirst.Name = "tSBtnTradeOrderFirst";
			this.tSBtnTradeOrderFirst.Size = new Size(16, 16);
			this.tSBtnTradeOrderFirst.ToolTipText = "第一页";
			this.tSBtnTradeOrderFirst.Click += new EventHandler(this.tSBtnTradeOrderFirst_Click);
			this.tSBtnTradeOrderPrevious.AutoSize = false;
			this.tSBtnTradeOrderPrevious.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnTradeOrderPrevious.BackgroundImage");
			this.tSBtnTradeOrderPrevious.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnTradeOrderPrevious.Enabled = false;
			this.tSBtnTradeOrderPrevious.ImageTransparentColor = Color.Magenta;
			this.tSBtnTradeOrderPrevious.Name = "tSBtnTradeOrderPrevious";
			this.tSBtnTradeOrderPrevious.Size = new Size(16, 16);
			this.tSBtnTradeOrderPrevious.ToolTipText = "上一页";
			this.tSBtnTradeOrderPrevious.Click += new EventHandler(this.tSBtnTradeOrderPrevious_Click);
			this.tSLblTradeOrderPage.AutoSize = false;
			this.tSLblTradeOrderPage.Name = "tSLblTradeOrderPage";
			this.tSLblTradeOrderPage.Size = new Size(95, 16);
			this.tSLblTradeOrderPage.Text = "Page/Total";
			this.tSBtnTradeOrderNext.AutoSize = false;
			this.tSBtnTradeOrderNext.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnTradeOrderNext.BackgroundImage");
			this.tSBtnTradeOrderNext.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnTradeOrderNext.ImageTransparentColor = Color.Magenta;
			this.tSBtnTradeOrderNext.Name = "tSBtnTradeOrderNext";
			this.tSBtnTradeOrderNext.Size = new Size(16, 16);
			this.tSBtnTradeOrderNext.ToolTipText = "下一页";
			this.tSBtnTradeOrderNext.Click += new EventHandler(this.tSBtnTradeOrderNext_Click);
			this.tSBtnTradeOrderLast.AutoSize = false;
			this.tSBtnTradeOrderLast.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnTradeOrderLast.BackgroundImage");
			this.tSBtnTradeOrderLast.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnTradeOrderLast.ImageTransparentColor = Color.Magenta;
			this.tSBtnTradeOrderLast.Name = "tSBtnTradeOrderLast";
			this.tSBtnTradeOrderLast.Size = new Size(16, 16);
			this.tSBtnTradeOrderLast.ToolTipText = "最后页";
			this.tSBtnTradeOrderLast.Click += new EventHandler(this.tSBtnTradeOrderLast_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new Size(6, 23);
			this.tSLblTradeOrderNum.AutoSize = false;
			this.tSLblTradeOrderNum.Name = "tSLblTradeOrderNum";
			this.tSLblTradeOrderNum.Size = new Size(17, 16);
			this.tSLblTradeOrderNum.Text = "第";
			this.tSTbxTradeOrderCurPage.AutoSize = false;
			this.tSTbxTradeOrderCurPage.BackColor = Color.Snow;
			this.tSTbxTradeOrderCurPage.BorderStyle = BorderStyle.None;
			this.tSTbxTradeOrderCurPage.ForeColor = Color.Black;
			this.tSTbxTradeOrderCurPage.Name = "tSTbxTradeOrderCurPage";
			this.tSTbxTradeOrderCurPage.Size = new Size(50, 16);
			this.tSTbxTradeOrderCurPage.TextBoxTextAlign = HorizontalAlignment.Center;
			this.tSLblTradeOrderP.AutoSize = false;
			this.tSLblTradeOrderP.Name = "tSLblTradeOrderP";
			this.tSLblTradeOrderP.Size = new Size(17, 16);
			this.tSLblTradeOrderP.Text = "页";
			this.tSBtnTradeOrderGO.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnTradeOrderGO.ImageTransparentColor = Color.Magenta;
			this.tSBtnTradeOrderGO.Name = "tSBtnTradeOrderGO";
			this.tSBtnTradeOrderGO.Size = new Size(23, 16);
			this.tSBtnTradeOrderGO.Text = "GO";
			this.tSBtnTradeOrderGO.Click += new EventHandler(this.tSBtnTradeOrderGO_Click);
			this.dgTradeOrder.AllowUserToAddRows = false;
			this.dgTradeOrder.AllowUserToDeleteRows = false;
			this.dgTradeOrder.AllowUserToOrderColumns = true;
			this.dgTradeOrder.AllowUserToResizeRows = false;
			this.dgTradeOrder.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dgTradeOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgTradeOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgTradeOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgTradeOrder.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgTradeOrder.Location = new Point(3, 14);
			this.dgTradeOrder.Margin = new Padding(0);
			this.dgTradeOrder.Name = "dgTradeOrder";
			this.dgTradeOrder.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgTradeOrder.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgTradeOrder.RowHeadersVisible = false;
			this.dgTradeOrder.RowTemplate.Height = 16;
			this.dgTradeOrder.ScrollBars = ScrollBars.Vertical;
			this.dgTradeOrder.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgTradeOrder.Size = new Size(694, 163);
			this.dgTradeOrder.TabIndex = 2;
			this.dgTradeOrder.TabStop = false;
			this.dgTradeOrder.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgTradeOrder_ColumnHeaderMouseClick);
			this.dgTradeOrder.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgTradeOrder_DataBindingComplete);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxTradeOrder);
			base.Name = "TradeOrder";
			base.Size = new Size(700, 180);
			base.Load += new EventHandler(this.TradeOrder_Load);
			base.SizeChanged += new EventHandler(this.TradeOrder_SizeChanged);
			this.groupBoxTradeOrder.ResumeLayout(false);
			((ISupportInitialize)this.bindNavTradeOrder).EndInit();
			this.bindNavTradeOrder.ResumeLayout(false);
			this.bindNavTradeOrder.PerformLayout();
			((ISupportInitialize)this.dgTradeOrder).EndInit();
			base.ResumeLayout(false);
		}
	}
}
