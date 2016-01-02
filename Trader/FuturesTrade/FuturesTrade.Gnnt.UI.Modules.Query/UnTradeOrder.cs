using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation;
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
	public class UnTradeOrder : UserControl
	{
		private delegate void FillUnTradeOrder(DataTable dt, bool isShowPagingControl);
		private byte buttonFlag;
		private int pageNum;
		private bool isUnOrderHeaderLoad;
		private OrderItemInfo untradeOrderItemInfo = new OrderItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private int dgUnTradeHeight;
		private int bindNavHeight;
		private UnTradeOrder.FillUnTradeOrder fillUnTradeOrder;
		private IContainer components;
		private GroupBox groupBoxUnTrade;
		internal DataGridView dgUnTrade;
		internal BindingNavigator bindNavUnOrder;
		private ToolStripButton tSBtnUnOrderFirst;
		private ToolStripButton tSBtnUnOrderPrevious;
		private ToolStripLabel toolStripTextBox1;
		private ToolStripLabel tSLblUnOrderPageNum;
		private ToolStripButton tSBtnUnOrderNext;
		private ToolStripButton tSBtnUnOrderLast;
		private ToolStripLabel tSLblUnOrderNum;
		private ToolStripTextBox tSTbxUnOrderCurPage;
		private ToolStripLabel tSLblUnOrderP;
		private ToolStripButton tSBtnUnOrderGO;
		public UnTradeOrder()
		{
			this.InitializeComponent();
			this.operationManager.queryUnOrderOperation.UnOrderFill = new QueryUnOrderOperation.UnOrderFillCallBack(this.UnTradeOrderInfoFill);
			this.CreateHandle();
		}
		private void UnTradeOrderInfoFill(DataTable dTable, bool isPage)
		{
			try
			{
				this.fillUnTradeOrder = new UnTradeOrder.FillUnTradeOrder(this.dsUnTradeFill);
				this.HandleCreated();
				base.Invoke(this.fillUnTradeOrder, new object[]
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
		private void dsUnTradeFill(DataTable dt, bool isShowPagingControl)
		{
			this.dgUnTrade.DataSource = dt;
			this.SetDataGridViewHeader();
			this.SetBindNavLayOut(isShowPagingControl);
		}
		private void SetDataGridViewHeader()
		{
			if (!this.isUnOrderHeaderLoad)
			{
				for (int i = 0; i < this.dgUnTrade.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.untradeOrderItemInfo.m_htItemInfo[this.dgUnTrade.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgUnTrade.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgUnTrade.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgUnTrade.Columns[i].HeaderText = colItemInfo.name;
						this.dgUnTrade.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						this.dgUnTrade.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
						if (colItemInfo.sortID == 1)
						{
							this.dgUnTrade.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgUnTrade.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.untradeOrderItemInfo.m_strItems.Contains(this.dgUnTrade.Columns[i].Name))
						{
							this.dgUnTrade.Columns[i].Visible = false;
						}
						if (i == 0)
						{
							this.dgUnTrade.Columns[i].ReadOnly = false;
						}
						else
						{
							this.dgUnTrade.Columns[i].ReadOnly = true;
						}
					}
				}
				this.isUnOrderHeaderLoad = true;
			}
		}
		private void UnOrderSetEnable(bool isEnable)
		{
			if (this.operationManager.queryUnOrderOperation.UnOrderCurrentPage == 1)
			{
				this.tSBtnUnOrderFirst.Enabled = !isEnable;
				this.tSBtnUnOrderPrevious.Enabled = !isEnable;
				this.tSBtnUnOrderNext.Enabled = isEnable;
				this.tSBtnUnOrderLast.Enabled = isEnable;
				return;
			}
			if (this.operationManager.queryUnOrderOperation.UnOrderCurrentPage == this.operationManager.queryUnOrderOperation.UnOrderAllPage)
			{
				this.tSBtnUnOrderFirst.Enabled = isEnable;
				this.tSBtnUnOrderPrevious.Enabled = isEnable;
				this.tSBtnUnOrderNext.Enabled = !isEnable;
				this.tSBtnUnOrderLast.Enabled = !isEnable;
				return;
			}
			this.tSBtnUnOrderFirst.Enabled = isEnable;
			this.tSBtnUnOrderPrevious.Enabled = isEnable;
			this.tSBtnUnOrderNext.Enabled = isEnable;
			this.tSBtnUnOrderLast.Enabled = isEnable;
		}
		private void tSBtnUnOrderFirst_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 0;
			this.QueryPagingUnOrder();
		}
		private void tSBtnUnOrderPrevious_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 1;
			this.QueryPagingUnOrder();
		}
		private void tSBtnUnOrderNext_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 2;
			this.QueryPagingUnOrder();
		}
		private void tSBtnUnOrderLast_Click(object sender, EventArgs e)
		{
			this.buttonFlag = 3;
			this.QueryPagingUnOrder();
		}
		private void tSBtnUnOrderGO_Click(object sender, EventArgs e)
		{
			if (this.tSTbxUnOrderCurPage.Text.Trim().Length == 0)
			{
				MessageBox.Show(this.operationManager.InputPageNum, this.operationManager.Prompt, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.tSTbxUnOrderCurPage.Focus();
				return;
			}
			int num = int.Parse(this.tSTbxUnOrderCurPage.Text.Trim());
			if (num <= 0)
			{
				MessageBox.Show(this.operationManager.InputRightPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.tSTbxUnOrderCurPage.Focus();
				this.tSTbxUnOrderCurPage.SelectAll();
				return;
			}
			if (num == this.operationManager.queryUnOrderOperation.UnOrderCurrentPage)
			{
				return;
			}
			if (num <= this.operationManager.queryUnOrderOperation.UnOrderAllPage)
			{
				this.buttonFlag = 4;
				this.pageNum = num;
				this.QueryPagingUnOrder();
				return;
			}
			MessageBox.Show(this.operationManager.InputRightPageNum, this.operationManager.PageNumError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			this.tSTbxUnOrderCurPage.Focus();
			this.tSTbxUnOrderCurPage.SelectAll();
		}
		private void QueryPagingUnOrder()
		{
			this.operationManager.queryUnOrderOperation.QueryPageUnOrderData(this.buttonFlag, this.pageNum);
			this.UnOrderSetEnable(true);
			this.tSLblUnOrderPageNum.Text = this.operationManager.queryUnOrderOperation.UnOrderCurrentPage.ToString() + "/" + this.operationManager.queryUnOrderOperation.UnOrderAllPage.ToString();
		}
		private void SetBindNavLayOut(bool isShowPagingControl)
		{
			if (isShowPagingControl)
			{
				if (!this.bindNavUnOrder.Visible)
				{
					this.bindNavUnOrder.Visible = true;
					this.dgUnTrade.Height = this.dgUnTradeHeight - this.bindNavHeight;
				}
			}
			else
			{
				if (this.bindNavUnOrder.Visible)
				{
					this.bindNavUnOrder.Visible = false;
					this.dgUnTrade.Height = this.dgUnTradeHeight;
				}
			}
			this.tSLblUnOrderPageNum.Text = this.operationManager.queryUnOrderOperation.UnOrderCurrentPage.ToString() + "/" + this.operationManager.queryUnOrderOperation.UnOrderAllPage.ToString();
		}
		private void dgUnTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				List<string> list = new List<string>();
				string text = this.dgUnTrade.Rows[this.dgUnTrade.CurrentRow.Index].Cells["OrderNo"].Value.ToString().Trim();
				if (text == "-1")
				{
					return;
				}
				string message = string.Format(this.operationManager.RevokeOrdersMessge, text);
				MessageForm messageForm = new MessageForm(this.operationManager.RevokeOrders, message, 0);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					list.Add(text);
					this.operationManager.revokeOrderOperation.RevokeOrderThread(list);
				}
			}
		}
		private void dgUnTrade_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				if (this.dgUnTrade.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgUnTrade.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
				{
					if (Global.StatusInfoFill != null)
					{
						Global.StatusInfoFill(this.operationManager.DoubleClickCancellation, Global.RightColor, true);
						return;
					}
				}
				else
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_StateNonCancellation");
					if (Global.StatusInfoFill != null)
					{
						Global.StatusInfoFill(string.Format(@string, this.dgUnTrade.Rows[e.RowIndex].Cells["Status"].Value), Global.RightColor, true);
					}
				}
			}
		}
		private void dgUnTrade_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1 && Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill("", Global.RightColor, true);
			}
		}
		private void dgUnTrade_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (this.dgUnTrade.RowCount > 1 && this.dgUnTrade.Rows[this.dgUnTrade.RowCount - 1].Cells["Time"].Value.ToString().Trim() == "合计")
			{
				this.dgUnTrade.Rows[this.dgUnTrade.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgUnTrade.Rows[this.dgUnTrade.RowCount - 1].ReadOnly = true;
			}
			Global.BSAlign(this.dgUnTrade);
		}
		private void dgUnTrade_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex < 0)
			{
				return;
			}
			try
			{
				string unOrderSortName = this.dgUnTrade.Columns[e.ColumnIndex].Name.ToString();
				this.operationManager.queryUnOrderOperation.UnOrderDataGridViewSort(unOrderSortName);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void tSTbxUnOrderCurPage_KeyPress(object sender, KeyPressEventArgs e)
		{
			Global.TextBoxNumKeypress(e);
		}
		private void UnTradeOrder_Load(object sender, EventArgs e)
		{
			this.bindNavUnOrder.Visible = false;
		}
		private void UnTradeOrder_SizeChanged(object sender, EventArgs e)
		{
			this.dgUnTradeHeight = this.dgUnTrade.Height;
			this.bindNavHeight = this.bindNavUnOrder.Height;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UnTradeOrder));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			this.groupBoxUnTrade = new GroupBox();
			this.bindNavUnOrder = new BindingNavigator(this.components);
			this.tSBtnUnOrderFirst = new ToolStripButton();
			this.tSBtnUnOrderPrevious = new ToolStripButton();
			this.toolStripTextBox1 = new ToolStripLabel();
			this.tSLblUnOrderPageNum = new ToolStripLabel();
			this.tSBtnUnOrderNext = new ToolStripButton();
			this.tSBtnUnOrderLast = new ToolStripButton();
			this.tSLblUnOrderNum = new ToolStripLabel();
			this.tSTbxUnOrderCurPage = new ToolStripTextBox();
			this.tSLblUnOrderP = new ToolStripLabel();
			this.tSBtnUnOrderGO = new ToolStripButton();
			this.dgUnTrade = new DataGridView();
			this.groupBoxUnTrade.SuspendLayout();
			((ISupportInitialize)this.bindNavUnOrder).BeginInit();
			this.bindNavUnOrder.SuspendLayout();
			((ISupportInitialize)this.dgUnTrade).BeginInit();
			base.SuspendLayout();
			this.groupBoxUnTrade.Controls.Add(this.bindNavUnOrder);
			this.groupBoxUnTrade.Controls.Add(this.dgUnTrade);
			this.groupBoxUnTrade.Dock = DockStyle.Fill;
			this.groupBoxUnTrade.Location = new Point(0, 0);
			this.groupBoxUnTrade.Margin = new Padding(0);
			this.groupBoxUnTrade.Name = "groupBoxUnTrade";
			this.groupBoxUnTrade.Padding = new Padding(3, 0, 3, 3);
			this.groupBoxUnTrade.RightToLeft = RightToLeft.No;
			this.groupBoxUnTrade.Size = new Size(700, 180);
			this.groupBoxUnTrade.TabIndex = 0;
			this.groupBoxUnTrade.TabStop = false;
			this.groupBoxUnTrade.Text = "未成交委托";
			this.bindNavUnOrder.AddNewItem = null;
			this.bindNavUnOrder.AutoSize = false;
			this.bindNavUnOrder.BackColor = Color.Gainsboro;
			this.bindNavUnOrder.CountItem = null;
			this.bindNavUnOrder.DeleteItem = null;
			this.bindNavUnOrder.Dock = DockStyle.Bottom;
			this.bindNavUnOrder.Items.AddRange(new ToolStripItem[]
			{
				this.tSBtnUnOrderFirst,
				this.tSBtnUnOrderPrevious,
				this.toolStripTextBox1,
				this.tSLblUnOrderPageNum,
				this.tSBtnUnOrderNext,
				this.tSBtnUnOrderLast,
				this.tSLblUnOrderNum,
				this.tSTbxUnOrderCurPage,
				this.tSLblUnOrderP,
				this.tSBtnUnOrderGO
			});
			this.bindNavUnOrder.LayoutStyle = ToolStripLayoutStyle.Flow;
			this.bindNavUnOrder.Location = new Point(3, 157);
			this.bindNavUnOrder.MoveFirstItem = null;
			this.bindNavUnOrder.MoveLastItem = null;
			this.bindNavUnOrder.MoveNextItem = null;
			this.bindNavUnOrder.MovePreviousItem = null;
			this.bindNavUnOrder.Name = "bindNavUnOrder";
			this.bindNavUnOrder.PositionItem = null;
			this.bindNavUnOrder.RenderMode = ToolStripRenderMode.System;
			this.bindNavUnOrder.Size = new Size(694, 20);
			this.bindNavUnOrder.TabIndex = 3;
			this.bindNavUnOrder.Text = "bindingNavigator1";
			this.tSBtnUnOrderFirst.AutoSize = false;
			this.tSBtnUnOrderFirst.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnUnOrderFirst.BackgroundImage");
			this.tSBtnUnOrderFirst.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnUnOrderFirst.Enabled = false;
			this.tSBtnUnOrderFirst.ImageTransparentColor = Color.Magenta;
			this.tSBtnUnOrderFirst.Name = "tSBtnUnOrderFirst";
			this.tSBtnUnOrderFirst.Size = new Size(16, 16);
			this.tSBtnUnOrderFirst.ToolTipText = "第一页";
			this.tSBtnUnOrderFirst.Click += new EventHandler(this.tSBtnUnOrderFirst_Click);
			this.tSBtnUnOrderPrevious.AutoSize = false;
			this.tSBtnUnOrderPrevious.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnUnOrderPrevious.BackgroundImage");
			this.tSBtnUnOrderPrevious.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnUnOrderPrevious.Enabled = false;
			this.tSBtnUnOrderPrevious.Image = (Image)componentResourceManager.GetObject("tSBtnUnOrderPrevious.Image");
			this.tSBtnUnOrderPrevious.ImageTransparentColor = Color.Magenta;
			this.tSBtnUnOrderPrevious.Name = "tSBtnUnOrderPrevious";
			this.tSBtnUnOrderPrevious.Size = new Size(16, 16);
			this.tSBtnUnOrderPrevious.ToolTipText = "上一页";
			this.tSBtnUnOrderPrevious.Click += new EventHandler(this.tSBtnUnOrderPrevious_Click);
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new Size(0, 0);
			this.tSLblUnOrderPageNum.AutoSize = false;
			this.tSLblUnOrderPageNum.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSLblUnOrderPageNum.ImageTransparentColor = Color.Magenta;
			this.tSLblUnOrderPageNum.Name = "tSLblUnOrderPageNum";
			this.tSLblUnOrderPageNum.Size = new Size(95, 16);
			this.tSLblUnOrderPageNum.Text = "Page/Total";
			this.tSBtnUnOrderNext.AutoSize = false;
			this.tSBtnUnOrderNext.BackColor = Color.Gainsboro;
			this.tSBtnUnOrderNext.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnUnOrderNext.BackgroundImage");
			this.tSBtnUnOrderNext.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnUnOrderNext.ImageTransparentColor = Color.Magenta;
			this.tSBtnUnOrderNext.Name = "tSBtnUnOrderNext";
			this.tSBtnUnOrderNext.Size = new Size(16, 16);
			this.tSBtnUnOrderNext.ToolTipText = "下一页";
			this.tSBtnUnOrderNext.Click += new EventHandler(this.tSBtnUnOrderNext_Click);
			this.tSBtnUnOrderLast.AutoSize = false;
			this.tSBtnUnOrderLast.BackgroundImage = (Image)componentResourceManager.GetObject("tSBtnUnOrderLast.BackgroundImage");
			this.tSBtnUnOrderLast.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnUnOrderLast.ImageTransparentColor = Color.Magenta;
			this.tSBtnUnOrderLast.Name = "tSBtnUnOrderLast";
			this.tSBtnUnOrderLast.Size = new Size(16, 16);
			this.tSBtnUnOrderLast.Click += new EventHandler(this.tSBtnUnOrderLast_Click);
			this.tSLblUnOrderNum.AutoSize = false;
			this.tSLblUnOrderNum.Name = "tSLblUnOrderNum";
			this.tSLblUnOrderNum.Size = new Size(17, 16);
			this.tSLblUnOrderNum.Text = "第";
			this.tSTbxUnOrderCurPage.AcceptsReturn = true;
			this.tSTbxUnOrderCurPage.AutoSize = false;
			this.tSTbxUnOrderCurPage.BackColor = Color.Snow;
			this.tSTbxUnOrderCurPage.BorderStyle = BorderStyle.None;
			this.tSTbxUnOrderCurPage.Name = "tSTbxUnOrderCurPage";
			this.tSTbxUnOrderCurPage.Size = new Size(50, 16);
			this.tSTbxUnOrderCurPage.TextBoxTextAlign = HorizontalAlignment.Center;
			this.tSLblUnOrderP.AutoSize = false;
			this.tSLblUnOrderP.Name = "tSLblUnOrderP";
			this.tSLblUnOrderP.Size = new Size(17, 16);
			this.tSLblUnOrderP.Text = "页";
			this.tSBtnUnOrderGO.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.tSBtnUnOrderGO.ImageTransparentColor = Color.Magenta;
			this.tSBtnUnOrderGO.Name = "tSBtnUnOrderGO";
			this.tSBtnUnOrderGO.Size = new Size(23, 16);
			this.tSBtnUnOrderGO.Text = "GO";
			this.tSBtnUnOrderGO.Click += new EventHandler(this.tSBtnUnOrderGO_Click);
			this.dgUnTrade.AllowUserToAddRows = false;
			this.dgUnTrade.AllowUserToDeleteRows = false;
			this.dgUnTrade.AllowUserToOrderColumns = true;
			this.dgUnTrade.AllowUserToResizeRows = false;
			this.dgUnTrade.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dgUnTrade.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgUnTrade.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgUnTrade.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgUnTrade.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgUnTrade.Location = new Point(3, 14);
			this.dgUnTrade.Margin = new Padding(0);
			this.dgUnTrade.Name = "dgUnTrade";
			this.dgUnTrade.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgUnTrade.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgUnTrade.RowHeadersVisible = false;
			this.dgUnTrade.RowTemplate.Height = 16;
			this.dgUnTrade.ScrollBars = ScrollBars.Vertical;
			this.dgUnTrade.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgUnTrade.Size = new Size(694, 163);
			this.dgUnTrade.TabIndex = 1;
			this.dgUnTrade.TabStop = false;
			this.dgUnTrade.CellDoubleClick += new DataGridViewCellEventHandler(this.dgUnTrade_CellDoubleClick);
			this.dgUnTrade.CellMouseEnter += new DataGridViewCellEventHandler(this.dgUnTrade_CellMouseEnter);
			this.dgUnTrade.CellMouseLeave += new DataGridViewCellEventHandler(this.dgUnTrade_CellMouseLeave);
			this.dgUnTrade.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgUnTrade_ColumnHeaderMouseClick);
			this.dgUnTrade.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgUnTrade_DataBindingComplete);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxUnTrade);
			base.Name = "UnTradeOrder";
			base.Size = new Size(700, 180);
			base.Load += new EventHandler(this.UnTradeOrder_Load);
			base.SizeChanged += new EventHandler(this.UnTradeOrder_SizeChanged);
			this.groupBoxUnTrade.ResumeLayout(false);
			((ISupportInitialize)this.bindNavUnOrder).EndInit();
			this.bindNavUnOrder.ResumeLayout(false);
			this.bindNavUnOrder.PerformLayout();
			((ISupportInitialize)this.dgUnTrade).EndInit();
			base.ResumeLayout(false);
		}
	}
}
