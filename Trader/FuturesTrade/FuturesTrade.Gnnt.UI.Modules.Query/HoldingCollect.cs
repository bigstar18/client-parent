using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.DBService.ServiceManager;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
using FuturesTrade.Gnnt.UI.Modules.Tools;
namespace FuturesTrade.Gnnt.UI.Modules.Query
{
	public class HoldingCollect : UserControl
	{
		private delegate void FillHolding(DataTable dt);
		private IContainer components;
		private GroupBox groupBoxHoldingCollect;
		private Panel groupBoxF4_1;
		internal RadioButton radioHdCollect;
		internal RadioButton radioHdDetail;
		internal Label labelB_SF5;
		internal ComboBox comboB_SF5;
		internal DataGridView dgHoldingCollect;
		internal ComboBox comboTrancF4;
		internal ComboBox comboCommodityF4;
		internal Label labelTrancF4;
		private Label labelCommodityF4;
		private Button buttonSelF4;
		private Button buttonHolding;
		private int Style;
		private bool isHoldingHeaderLoad;
		private bool isHoldingDetailDataLoad;
		private bool isFirstLoad = true;
		private HoldingItemInfo holdingItemInfo = new HoldingItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private HoldingCollect.FillHolding fillHolding;
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			this.groupBoxHoldingCollect = new GroupBox();
			this.groupBoxF4_1 = new Panel();
			this.radioHdCollect = new RadioButton();
			this.radioHdDetail = new RadioButton();
			this.labelB_SF5 = new Label();
			this.comboB_SF5 = new ComboBox();
			this.dgHoldingCollect = new DataGridView();
			this.comboTrancF4 = new ComboBox();
			this.comboCommodityF4 = new ComboBox();
			this.labelTrancF4 = new Label();
			this.labelCommodityF4 = new Label();
			this.buttonSelF4 = new Button();
			this.buttonHolding = new Button();
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
			this.groupBoxHoldingCollect.Location = new Point(31, 0);
			this.groupBoxHoldingCollect.Name = "groupBoxHoldingCollect";
			this.groupBoxHoldingCollect.Size = new Size(669, 200);
			this.groupBoxHoldingCollect.TabIndex = 22;
			this.groupBoxHoldingCollect.TabStop = false;
			this.groupBoxHoldingCollect.Text = "订货汇总";
			this.groupBoxF4_1.Controls.Add(this.radioHdCollect);
			this.groupBoxF4_1.Controls.Add(this.radioHdDetail);
			this.groupBoxF4_1.Location = new Point(445, 12);
			this.groupBoxF4_1.Name = "groupBoxF4_1";
			this.groupBoxF4_1.Size = new Size(154, 25);
			this.groupBoxF4_1.TabIndex = 27;
			this.radioHdCollect.AutoSize = true;
			this.radioHdCollect.Checked = true;
			this.radioHdCollect.ImeMode = ImeMode.NoControl;
			this.radioHdCollect.Location = new Point(3, 4);
			this.radioHdCollect.Name = "radioHdCollect";
			this.radioHdCollect.Size = new Size(71, 16);
			this.radioHdCollect.TabIndex = 25;
			this.radioHdCollect.TabStop = true;
			this.radioHdCollect.Text = "订货汇总";
			this.radioHdCollect.UseVisualStyleBackColor = true;
			this.radioHdDetail.AutoSize = true;
			this.radioHdDetail.ImeMode = ImeMode.NoControl;
			this.radioHdDetail.Location = new Point(80, 4);
			this.radioHdDetail.Name = "radioHdDetail";
			this.radioHdDetail.Size = new Size(71, 16);
			this.radioHdDetail.TabIndex = 26;
			this.radioHdDetail.Text = "订货明细";
			this.radioHdDetail.UseVisualStyleBackColor = true;
			this.radioHdDetail.Click += new EventHandler(this.radioHdDetail_Click);
			this.labelB_SF5.AutoSize = true;
			this.labelB_SF5.ImeMode = ImeMode.NoControl;
			this.labelB_SF5.Location = new Point(330, 19);
			this.labelB_SF5.Name = "labelB_SF5";
			this.labelB_SF5.Size = new Size(35, 12);
			this.labelB_SF5.TabIndex = 24;
			this.labelB_SF5.Text = "买/卖";
			this.comboB_SF5.FormattingEnabled = true;
			this.comboB_SF5.Location = new Point(371, 14);
			this.comboB_SF5.Name = "comboB_SF5";
			this.comboB_SF5.Size = new Size(53, 20);
			this.comboB_SF5.TabIndex = 23;
			this.comboB_SF5.TabStop = false;
			this.comboB_SF5.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.dgHoldingCollect.AllowUserToAddRows = false;
			this.dgHoldingCollect.AllowUserToDeleteRows = false;
			this.dgHoldingCollect.AllowUserToOrderColumns = true;
			this.dgHoldingCollect.AllowUserToResizeRows = false;
			this.dgHoldingCollect.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dgHoldingCollect.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgHoldingCollect.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgHoldingCollect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgHoldingCollect.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgHoldingCollect.Location = new Point(4, 40);
			this.dgHoldingCollect.Name = "dgHoldingCollect";
			this.dgHoldingCollect.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgHoldingCollect.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgHoldingCollect.RowHeadersVisible = false;
			this.dgHoldingCollect.RowTemplate.Height = 20;
			this.dgHoldingCollect.ScrollBars = ScrollBars.Vertical;
			this.dgHoldingCollect.Size = new Size(662, 155);
			this.dgHoldingCollect.TabIndex = 22;
			this.dgHoldingCollect.TabStop = false;
			this.dgHoldingCollect.CellClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellClick);
			this.dgHoldingCollect.CellDoubleClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellDoubleClick);
			this.dgHoldingCollect.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgHoldingCollect_CellFormatting);
			this.dgHoldingCollect.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgHoldingCollect_ColumnHeaderMouseClick);
			this.dgHoldingCollect.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgHoldingCollect_DataBindingComplete);
			this.comboTrancF4.Location = new Point(236, 15);
			this.comboTrancF4.Name = "comboTrancF4";
			this.comboTrancF4.Size = new Size(88, 20);
			this.comboTrancF4.TabIndex = 20;
			this.comboTrancF4.TabStop = false;
			this.comboTrancF4.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.comboCommodityF4.Location = new Point(76, 15);
			this.comboCommodityF4.Name = "comboCommodityF4";
			this.comboCommodityF4.Size = new Size(80, 20);
			this.comboCommodityF4.TabIndex = 19;
			this.comboCommodityF4.TabStop = false;
			this.comboCommodityF4.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.labelTrancF4.ImeMode = ImeMode.NoControl;
			this.labelTrancF4.Location = new Point(164, 17);
			this.labelTrancF4.Name = "labelTrancF4";
			this.labelTrancF4.Size = new Size(72, 16);
			this.labelTrancF4.TabIndex = 18;
			this.labelTrancF4.Text = "交易代码：";
			this.labelTrancF4.TextAlign = ContentAlignment.MiddleCenter;
			this.labelCommodityF4.ImeMode = ImeMode.NoControl;
			this.labelCommodityF4.Location = new Point(4, 17);
			this.labelCommodityF4.Name = "labelCommodityF4";
			this.labelCommodityF4.Size = new Size(72, 16);
			this.labelCommodityF4.TabIndex = 17;
			this.labelCommodityF4.Text = "商品代码：";
			this.labelCommodityF4.TextAlign = ContentAlignment.MiddleCenter;
			this.buttonSelF4.ImeMode = ImeMode.NoControl;
			this.buttonSelF4.Location = new Point(604, 16);
			this.buttonSelF4.Name = "buttonSelF4";
			this.buttonSelF4.Size = new Size(57, 20);
			this.buttonSelF4.TabIndex = 14;
			this.buttonSelF4.TabStop = false;
			this.buttonSelF4.Text = "刷新";
			this.buttonSelF4.UseVisualStyleBackColor = true;
			this.buttonSelF4.Click += new EventHandler(this.buttonSelF4_Click);
			this.buttonHolding.BackColor = Color.White;
			this.buttonHolding.Dock = DockStyle.Left;
			this.buttonHolding.Enabled = false;
			this.buttonHolding.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.buttonHolding.ForeColor = SystemColors.ControlDarkDark;
			this.buttonHolding.Location = new Point(0, 0);
			this.buttonHolding.Name = "buttonHolding";
			this.buttonHolding.Size = new Size(31, 200);
			this.buttonHolding.TabIndex = 23;
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
		public HoldingCollect(int style)
		{
			this.InitializeComponent();
			this.Style = style;
			this.operationManager.queryHoldingOperation.HoldingFillEvent = new QueryHoldingOperation.HoldingFillCallBack(this.HoldingInfoFill);
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.operationManager.ShowHoldingCollect = new OperationManager.ShowHoldingControlCallBack(this.SetVisible);
			this.CreateHandle();
		}
		private void HoldingInfoFill(DataTable dTable)
		{
			try
			{
				this.fillHolding = new HoldingCollect.FillHolding(this.dsHoldingFill);
				this.HandleCreated();
				base.Invoke(this.fillHolding, new object[]
				{
					dTable
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void dsHoldingFill(DataTable dt)
		{
			this.dgHoldingCollect.DataSource = dt;
			this.SetDataGridViewHeader();
		}
		private void SetDataGridViewHeader()
		{
			if (!this.isHoldingHeaderLoad)
			{
				for (int i = 0; i < this.dgHoldingCollect.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.holdingItemInfo.m_htItemInfo[this.dgHoldingCollect.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgHoldingCollect.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgHoldingCollect.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgHoldingCollect.Columns[i].HeaderText = colItemInfo.name;
						this.dgHoldingCollect.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						this.dgHoldingCollect.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
						if (colItemInfo.sortID == 1)
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
		private void QueryConditionChanged(object sender, EventArgs e)
		{
			if (!this.isFirstLoad)
			{
				string sql = this.OrderSql();
				this.operationManager.queryHoldingOperation.HoldingScreen(sql);
			}
		}
		private string OrderSql()
		{
			string text = " 1=1 ";
			if (this.comboCommodityF4.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF4.Text + "'";
			}
			if (this.comboB_SF5.SelectedIndex == 1)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
			}
			else
			{
				if (this.comboB_SF5.SelectedIndex == 2)
				{
					text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
				}
			}
			return text;
		}
		private void dgHoldingCollect_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex > 0)
			{
				if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessage1");
					Global.StatusInfoFill(@string, Global.RightColor, true);
				}
				else
				{
					if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
					{
						string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessage2");
						Global.StatusInfoFill(string2, Global.RightColor, true);
					}
				}
                if (ToolsLibrary.util.Tools.StrToBool((string)Global.HTConfig["AutoDisplayMinLine"], false) && e.RowIndex > -1 && e.RowIndex < this.dgHoldingCollect.RowCount - 1)
				{
					Global.displayMinLine(this.dgHoldingCollect["Market", e.RowIndex].Value.ToString().Trim(), this.dgHoldingCollect[0, e.RowIndex].Value.ToString().Trim());
				}
			}
		}
		private void dgHoldingCollect_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessageMaxBuy");
				DataGridViewCell dataGridViewCell = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
				dataGridViewCell.ToolTipText = @string;
				return;
			}
			if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
			{
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_DoubleClickInfoMessageMinSale");
				DataGridViewCell dataGridViewCell2 = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
				dataGridViewCell2.ToolTipText = string2;
			}
		}
		private void dgHoldingCollect_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				return;
			}
			if (this.radioHdCollect.Checked)
			{
				this.operationManager.queryHoldingOperation.HoldingSort(this.dgHoldingCollect.Columns[e.ColumnIndex].Name.ToString());
				return;
			}
			if (this.radioHdDetail.Checked)
			{
				this.operationManager.queryHoldingDatailOperation.HoldingDetailSort(this.dgHoldingCollect.Columns[e.ColumnIndex].Name.ToString());
			}
		}
		private void dgHoldingCollect_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (this.dgHoldingCollect.RowCount > 1 && this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].Cells["CommodityID"].Value.ToString().Trim() == "合计")
			{
				this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgHoldingCollect.Rows[this.dgHoldingCollect.RowCount - 1].ReadOnly = true;
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
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
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
		public void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboCommodityF4.Items.Clear();
			this.comboCommodityF4.Items.AddRange(commodityIDList.ToArray());
			this.comboCommodityF4.SelectedIndex = 0;
			this.isFirstLoad = false;
		}
		private void buttonSelF4_Click(object sender, EventArgs e)
		{
			this.operationManager.queryHoldingOperation.ButtonRefreshFlag = 1;
			this.operationManager.queryHoldingOperation.QueryHoldingInfoLoad();
			this.operationManager.IdleRefreshButton = 0;
		}
		private void radioHdDetail_Click(object sender, EventArgs e)
		{
			this.radioHdCollect.Checked = true;
			base.Visible = false;
			this.operationManager.ShowHolding(1);
		}
		private void SetVisible()
		{
			base.Visible = true;
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
					CommData commData = ServiceManage.GetInstance().CreateQueryCommData().QueryGNCommodityInfo(marketID, commodityID);
					short buysell = 0;
					short ordertype = 0;
					double price = 0.0;
					double lprice = 0.0;
					int qty = 0;
					if (e.ColumnIndex == this.dgHoldingCollect.Columns["BuyHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["BuyVHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["BuyAvg"].Index)
					{
						buysell = 1;
						ordertype = 1;
						price = commData.Bid;
                        qty = ToolsLibrary.util.Tools.StrToInt(this.dgHoldingCollect["BuyVHolding", e.RowIndex].Value.ToString(), 0);
					}
					else
					{
						if (e.ColumnIndex == this.dgHoldingCollect.Columns["SellHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["SellAvg"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["SellVHolding"].Index)
						{
							buysell = 0;
							ordertype = 1;
							price = commData.Offer;
                            qty = ToolsLibrary.util.Tools.StrToInt(this.dgHoldingCollect["SellVHolding", e.RowIndex].Value.ToString(), 0);
						}
					}
					if (Global.SetDoubleClickOrderInfo != null)
					{
						Global.SetDoubleClickOrderInfo(price, lprice, qty, buysell, ordertype);
						return;
					}
				}
				else
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF5_NotExistGoods");
					Global.StatusInfoFill(@string, Global.ErrorColor, true);
				}
			}
		}
	}
}
