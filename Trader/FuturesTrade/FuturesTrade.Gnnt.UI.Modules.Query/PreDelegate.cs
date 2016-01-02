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
	public class PreDelegate : UserControl
	{
		private delegate void FillPreOrder(DataTable dt);
		private IContainer components;
		private GroupBox groupBoxF7;
		internal DataGridView dgPreDelegate;
		private DataGridViewCheckBoxColumn SelectFlag;
		private Button buttonDel;
		public Button buttonOrder6;
		private Button selAll;
		private Button buttonSel;
		internal ComboBox comTranc;
		private ComboBox comCommodity;
		internal Label labelTrancF6;
		private Label labelCommodityF6;
		private bool isPreDelegateHeaderLoad;
		private string[] idColumns;
		private PreOrderItemInfo preOrderItemInfo = new PreOrderItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private PreDelegate.FillPreOrder fillPreOrder;
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
			this.groupBoxF7 = new GroupBox();
			this.dgPreDelegate = new DataGridView();
			this.SelectFlag = new DataGridViewCheckBoxColumn();
			this.buttonDel = new Button();
			this.buttonOrder6 = new Button();
			this.selAll = new Button();
			this.buttonSel = new Button();
			this.comTranc = new ComboBox();
			this.comCommodity = new ComboBox();
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
			this.groupBoxF7.TabIndex = 22;
			this.groupBoxF7.TabStop = false;
			this.groupBoxF7.Text = "预埋查询";
			this.dgPreDelegate.AllowUserToAddRows = false;
			this.dgPreDelegate.AllowUserToDeleteRows = false;
			this.dgPreDelegate.AllowUserToOrderColumns = true;
			this.dgPreDelegate.AllowUserToResizeRows = false;
			this.dgPreDelegate.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dgPreDelegate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgPreDelegate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgPreDelegate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgPreDelegate.Columns.AddRange(new DataGridViewColumn[]
			{
				this.SelectFlag
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgPreDelegate.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgPreDelegate.Location = new Point(4, 41);
			this.dgPreDelegate.Name = "dgPreDelegate";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgPreDelegate.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgPreDelegate.RowHeadersVisible = false;
			this.dgPreDelegate.RowTemplate.Height = 18;
			this.dgPreDelegate.Size = new Size(692, 154);
			this.dgPreDelegate.TabIndex = 9;
			this.dgPreDelegate.TabStop = false;
			this.dgPreDelegate.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgPreDelegate_ColumnHeaderMouseClick);
			this.dgPreDelegate.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgPreDelegate_DataBindingComplete);
			this.SelectFlag.HeaderText = "";
			this.SelectFlag.Name = "SelectFlag";
			this.buttonDel.ImeMode = ImeMode.NoControl;
			this.buttonDel.Location = new Point(539, 16);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new Size(53, 20);
			this.buttonDel.TabIndex = 8;
			this.buttonDel.Text = "删除";
			this.buttonDel.Click += new EventHandler(this.buttonDel_Click);
			this.buttonOrder6.ImeMode = ImeMode.NoControl;
			this.buttonOrder6.Location = new Point(477, 16);
			this.buttonOrder6.Name = "buttonOrder6";
			this.buttonOrder6.Size = new Size(53, 20);
			this.buttonOrder6.TabIndex = 7;
			this.buttonOrder6.Text = "委托";
			this.buttonOrder6.Click += new EventHandler(this.buttonOrder6_Click);
			this.selAll.ImeMode = ImeMode.NoControl;
			this.selAll.Location = new Point(418, 16);
			this.selAll.Name = "selAll";
			this.selAll.Size = new Size(53, 20);
			this.selAll.TabIndex = 6;
			this.selAll.Text = "全选";
			this.selAll.Click += new EventHandler(this.selAll_Click);
			this.buttonSel.ImeMode = ImeMode.NoControl;
			this.buttonSel.Location = new Point(356, 16);
			this.buttonSel.Name = "buttonSel";
			this.buttonSel.Size = new Size(53, 20);
			this.buttonSel.TabIndex = 5;
			this.buttonSel.Text = "查询";
			this.buttonSel.Click += new EventHandler(this.QueryConditionChanged);
			this.comTranc.Location = new Point(236, 15);
			this.comTranc.Name = "comTranc";
			this.comTranc.Size = new Size(88, 20);
			this.comTranc.TabIndex = 4;
			this.comTranc.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.comCommodity.Location = new Point(76, 15);
			this.comCommodity.Name = "comCommodity";
			this.comCommodity.Size = new Size(80, 20);
			this.comCommodity.TabIndex = 3;
			this.comCommodity.SelectedIndexChanged += new EventHandler(this.QueryConditionChanged);
			this.labelTrancF6.ImeMode = ImeMode.NoControl;
			this.labelTrancF6.Location = new Point(164, 17);
			this.labelTrancF6.Name = "labelTrancF6";
			this.labelTrancF6.Size = new Size(72, 18);
			this.labelTrancF6.TabIndex = 2;
			this.labelTrancF6.Text = "交易代码：";
			this.labelTrancF6.TextAlign = ContentAlignment.MiddleCenter;
			this.labelCommodityF6.ImeMode = ImeMode.NoControl;
			this.labelCommodityF6.Location = new Point(4, 17);
			this.labelCommodityF6.Name = "labelCommodityF6";
			this.labelCommodityF6.Size = new Size(72, 16);
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
		public PreDelegate()
		{
			this.InitializeComponent();
			this.operationManager.queryPredelegateOperation.PreDelegateFill = new QueryPredelegateOperation.PreDelegateFillCallBack(this.PreOrderInfoFill);
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.CreateHandle();
		}
		private void PreOrderInfoFill(DataTable dTable)
		{
			try
			{
				this.fillPreOrder = new PreDelegate.FillPreOrder(this.dsPreDelegateFill);
				this.HandleCreated();
				base.Invoke(this.fillPreOrder, new object[]
				{
					dTable
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
		private void dsPreDelegateFill(DataTable dTable)
		{
			this.dgPreDelegate.DataSource = dTable;
			this.SetDataGridViewHeader();
		}
		private void SetDataGridViewHeader()
		{
			if (!this.isPreDelegateHeaderLoad)
			{
				for (int i = 0; i < this.dgPreDelegate.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.preOrderItemInfo.m_htItemInfo[this.dgPreDelegate.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgPreDelegate.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgPreDelegate.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgPreDelegate.Columns[i].HeaderText = colItemInfo.name;
						this.dgPreDelegate.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						this.dgPreDelegate.Columns[i].DefaultCellStyle.FormatProvider = Global.MyNumberFormatInfo;
						if (colItemInfo.sortID == 1)
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
		private void QueryConditionChanged(object sender, EventArgs e)
		{
			string sql = this.PreDelegateSql();
			if (sender != null && sender is RadioButton)
			{
				RadioButton radioButton = (RadioButton)sender;
				if (!radioButton.Checked)
				{
					return;
				}
			}
			this.operationManager.queryPredelegateOperation.PreDelegateScreen(sql);
		}
		private string PreDelegateSql()
		{
			string text = " 1=1 ";
			if (this.comTranc.Visible && this.comTranc.SelectedIndex > 0)
			{
				text = text + " and TransactionsCode= '" + this.comTranc.Text + "'";
			}
			if (this.comCommodity.SelectedIndex != 0)
			{
				text = text + " and  CommodityCode = '" + this.comCommodity.Text + "'";
			}
			return text;
		}
		private void dgPreDelegate_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				return;
			}
			string sortName = this.dgPreDelegate.Columns[e.ColumnIndex].Name.ToString();
			this.operationManager.queryPredelegateOperation.PredelegateSort(sortName);
		}
		private void dgPreDelegate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (this.dgPreDelegate.RowCount > 1 && this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].Cells["TransactionsCode"].Value.ToString().Trim() == "合计")
			{
				this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgPreDelegate.Rows[this.dgPreDelegate.RowCount - 1].ReadOnly = true;
			}
			Global.BSAlign(this.dgPreDelegate);
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
				return;
			}
			for (int j = 0; j < this.dgPreDelegate.Rows.Count; j++)
			{
				this.dgPreDelegate.Rows[j].Cells[0].Value = false;
			}
			this.selAll.Text = this.operationManager.AllCheck;
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
		public void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comCommodity.Items.Clear();
			this.comCommodity.Items.AddRange(commodityIDList.ToArray());
			this.comCommodity.SelectedIndex = 0;
		}
	}
}
