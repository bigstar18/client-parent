using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeClientApp.Gnnt.ISSUE.Library;
using TradeClientApp.Gnnt.ISSUE.Notifier;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class MainForm : Form
	{
		private delegate void DateSetArrCallback(DataSet orderDataSet, DataSet tradeOrderDataSet);
		private delegate void DateSetCallback(DataSet DataSet);
		private delegate void BoolCallback(bool result);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private delegate void ResponseVOArrCallback(ResponseVO[] responseVOArr);
		private delegate void StringObjCallback(object _commodityItem);
		private delegate void PreOrder_CommdityInfoCallback(CommodityInfo commodityInfo);
		private delegate void Order_CommdityInfoCallback(CommodityInfo commodityInfo);
		private delegate void CommdityInfoCallback(CommodityInfo commodityInfo);
		private delegate void NumericQtyInfoCallback(CommodityInfo commodityInfo, long TradeNum, long ltimes);
		private delegate void RFGetCommoityCallback(CommodityInfo commodityInfo);
		private MainForm.DateSetCallback dgHoldingFill;
		private MainForm.DateSetCallback dgHoldingDetailFill;
		private bool IsFTime;
		private MainForm.DateSetCallback dgTradeFill;
		private MainForm.DateSetCallback dgTradeSumFill;
		private bool refreshTradeFlag = true;
		private bool refreshTradeSumFlag = true;
		private bool isFTradeSum = true;
		private MainForm.DateSetCallback dgAllOrderFill;
		private OrderItemInfo orderItemInfo;
		private TradeOrderItemInfo tradeOrderItemInfo;
		private AllOrderItemInfo allOrderItemInfo;
		private TradeItemInfo tradeItemInfo;
		private TradeSumItemInfo tradeSumItemInfo;
		private HoldingItemInfo holdingItemInfo;
		private HoldingDetailItemInfo holdingDetailItemInfo;
		private FundsItemInfo fundsItemInfo;
		private InvestorItemInfo investorItemInfo;
		private CommodityItemInfo commodityItemInfo;
		private PreOrderItemInfo preOrderItemInfo;
		private MainForm.DateSetArrCallback dgOrderFill;
		private bool refreshOTFlag = true;
		internal bool refreshFlag;
		private IContainer components;
		private System.Windows.Forms.Timer tradeTime;
		private ComboBox comboBox7;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn47;
		private DataGridViewTextBoxColumn Column9;
		private DataGridViewTextBoxColumn Column10;
		private GroupBox groupBoxOrder;
		private ComboBox comboCommodity;
		private NumericUpDown numericQty;
		private NumericUpDown numericPrice;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private ComboBox comboTranc;
		private Label labTrancCode;
		private GroupBox groupBoxB_S;
		private RadioButton radioS;
		private RadioButton radioB;
		private GroupBox groupBoxO_L;
		private RadioButton radioL;
		private RadioButton radioO;
		private Button buttonAddPre;
		private StatusStrip statusInfo;
		private ToolStripStatusLabel info;
		private ToolStripStatusLabel user;
		private ToolStripStatusLabel status;
		private ToolStripStatusLabel time;
		private ContextMenuStrip MenuRefresh;
		private ToolStripMenuItem ToolStripMenuItem;
		private TabPage TabPageF8;
		private GroupBox groupBoxF7;
		private DataGridView dgPreDelegate;
		private DataGridViewCheckBoxColumn SelectFlag;
		private Button buttonDel;
		private Button selAll;
		private Button buttonSel;
		private ComboBox comTranc;
		private ComboBox comCommodity;
		private Label labelTrancF6;
		private Label labelCommodityF6;
		private TabPage TabPageF6;
		private GroupBox groupBoxMoney;
		private ListView lstVFunds;
		private TabPage TabPageF5;
		private GroupBox groupBoxF4;
		private DataGridView dgHoldingCollect;
		private ComboBox comboTrancF4;
		private ComboBox comboCommodityF4;
		private Label labelTrancF4;
		private Label labelCommodityF4;
		private Button buttonSelF4;
		private TabPage TabPageF4;
		private GroupBox groupBoxF3;
		private ComboBox comboB_S;
		private Label labelB_S;
		private DataGridView dgTrade;
		private ComboBox comboTrancF3;
		private ComboBox comboCommodityF3;
		private Label labelTrancF3;
		private Label labelCommodityF3;
		private Button buttonSelF3;
		private GroupBox groupBoxF3_1;
		private RadioButton radioLF3;
		private RadioButton radioAllF3;
		private TabPage TabPageF3;
		private GroupBox groupBoxF2;
		private ComboBox comboTrancF2;
		private ComboBox comboCommodityF2;
		private Label labelTrancF2;
		private Label labelCommodityF2;
		private Button buttonCancelF2;
		private Button buttonAllF2;
		private Button buttonSelF2;
		private GroupBox groupBoxF2_1;
		private RadioButton radioCancelF2;
		private RadioButton radioAllF2;
		private TabPage TabPageF2;
		private GroupBox groupBoxTrade;
		private GroupBox groupBoxUnTrade;
		private TabControl tabMain;
		private Button buttonSelFundsF4;
		private System.Windows.Forms.Timer timerLock;
		private Panel panelLock;
		private Button buttonUnLock;
		private Label labelPwd;
		private TextBox textBoxPwd;
		private Label labelPwdInfo;
		private GroupBox groupBoxGNCommodit;
		private Label labelGNInfo;
		private Label labelBP2;
		private Label labelSP2;
		private Label labelBV3;
		private Label labelSV1;
		private Label labelLimitUpV;
		private Label labelLimitUp;
		private Label labelBP3;
		private Label labelSP1;
		private Label labelB3;
		private Label labelS1;
		private Label labelBV1;
		private Label labelSV3;
		private Label labelLimitDown;
		private Label labelLast;
		private Label labelBP1;
		private Label labelSP3;
		private Label labelB1;
		private Label labelBV2;
		private Label labelS3;
		private Label labelLastP;
		private Label labelSV2;
		private Label labelB2;
		private Label labelCount;
		private Label labelS2;
		private ComboBox comboMarKet;
		private Label labelMarKet;
		private ToolStripButton toolStripButtonLock;
		private ToolStripButton toolStripButtonHelp;
		private ToolStripButton toolStripButtonAbout;
		private ToolStripButton toolStripButtonExit;
		private ToolTip toolTip;
		private HelpProvider helpProvider;
		private Label MessageInfo;
		private Label labelLargestTN;
		private NumericUpDown numericLPrice;
		private Label labelLPrice;
		private ToolStripButton toolStripButtonMsg;
		private SplitContainer splitOrder;
		private Label labelSpread;
		private Button butMinLine;
		private Button butKLine;
		public ToolStrip toolStrip;
		private Button buttonFundsTransfer;
		private SplitContainer m_splitContainer;
		private ToolStripButton toolStripButtonSet;
		private ToolStripButton toolStripButtonOrder;
		private TextBox tbTranc;
		private RadioButton radioOF3;
		private DataGridView dgUnTrade;
		private DataGridView dgTradeOrder;
		private ComboBox comboB_SF3;
		private Label labelB_SF3;
		private DataGridView dgAllOrder;
		private DataGridViewCheckBoxColumn SelectFlagF2;
		private GroupBox gbCloseMode;
		private RadioButton rbCloseH;
		private RadioButton rbCloseT;
		public Button buttonOrder;
		public Button buttonOrder6;
		private Button butConditionOrder;
		private ToolStripButton toolStripButtonBill;
		private Label labelB_SF5;
		private ComboBox comboB_SF5;
		private Panel groupBoxF4_1;
		private RadioButton radioHdCollect;
		private RadioButton radioHdDetail;
		private DataGridView dgHoldingDetail;
		private TabPage TabPageF9;
		private GroupBox groupBoxInvestor;
		private ListView listVInvestor;
		private Label labelLimitDownV;
		private Label labelCountV;
		private Label labelPrevClearV;
		private Panel panel1;
		private Panel SplitterPanel;
		private System.Windows.Forms.Timer timerSysTime;
		private RadioButton radioTradeSum;
		private RadioButton radioTradeDetail;
		private DataGridView dgTradeSum;
		private bool LoadFlag = true;
		private bool DelegateFlag = true;
		private bool QueryOrderInfoFlag = true;
		private bool QueryTradeInfoFlag = true;
		private bool QueryHoldingInfoFlag = true;
		private bool QueryFundsInfoFlag = true;
		private bool QueryCommodityInfoFlag = true;
		private bool PreDelegateFlag = true;
		private bool InvestorFlag = true;
		private bool ConnectHQ;
		private FirmbreedQueryResponseVO BreedRep = new FirmbreedQueryResponseVO();
		private bool isDirectfirm;
		private int IdleOnMoudel;
		private int IdleRefreshButton;
		private DateTime IdleStartTime = default(DateTime);
		private double sPrice;
		private double bPrice;
		private bool displayInfo = true;
		private bool refreshTimeFlag = true;
		private bool refreshGNFlag = true;
		private Broadcast broadcast;
		private int broadcastNum = 1;
		private int broadcastCount;
		public string marketID = string.Empty;
		public string queryMarketID = string.Empty;
		private int MeInfoNum;
		private string messageInfomation = string.Empty;
		private MainForm.PreOrder_CommdityInfoCallback preorder_commodityInfo;
		private MainForm.Order_CommdityInfoCallback butOrderComm;
		private MainForm.CommdityInfoCallback commdityInfo;
		private MainForm.NumericQtyInfoCallback numericQtyInfo;
		private MainForm.RFGetCommoityCallback rfgetcommodity;
		private decimal screenWidthFl = 1m;
		internal DataProcess dataProcess = new DataProcess();
		private ControlLayout controlLayout = new ControlLayout();
		public XmlDataSet XmlCommodity;
		internal DataSet dsCommodity;
		internal XmlDataSet XmlTransactions;
		internal DataSet dsTransactions;
		private int connectStatus;
		private ListBox lbmain;
		private TaskbarNotifier Notifier = new TaskbarNotifier();
		private bool buttonClick;
		private double oldPrice;
		private double newPrice;
		private long querytimes;
		private BillOrder billOrder;
		private FormOrder formOrder;
		public string currentCommodity = string.Empty;
		private MainForm.StringObjCallback fillGNText;
		private string curCommodityMode = "";
		private double timerLockRefresh = 5000.0;
		private int timerLockCount;
		private double sysTimeRefresh = 5000.0;
		private int sysTimeCount;
		private LocalHook hook;
		private bool isAutoPrice;
		private ConditionOrder conditionOrder;
		private MainForm.StringObjCallback lstVInvestorFill;
		private static XmlDataSet XmlPreDelegate;
		private static DataSet dsPreDelegate;
		private string[] idColumns;
		private string m_order = " ASC ";
		private MainForm.StringObjCallback lstVCommodityFill;
		private Hashtable htCommodity = Hashtable.Synchronized(new Hashtable());
		private MainForm.StringObjCallback lstVFundsFill;
		private FundsTransfer fundsTransfer;
		public static event EventHandler mainFormLoad;
		public event InterFace.CommodityInfoEventHander MinLineEvent;
		public event InterFace.CommodityInfoEventHander KLineEvent;
		public event EventHandler LockFormEvent;
		private void QueryHoldingInfoLoad()
		{
			this.IsFTime = true;
			this.radioHdCollect.Checked = true;
			this.radioHdDetail.Checked = false;
			this.dgHoldingCollect.Visible = true;
			this.dgHoldingDetail.Visible = false;
			HoldingQueryRequestVO holdingQueryRequestVO = new HoldingQueryRequestVO();
			holdingQueryRequestVO.UserID = Global.UserID;
			this.dgHoldingFill = new MainForm.DateSetCallback(this.DgHoldingFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryHoldingInfo);
			ThreadPool.QueueUserWorkItem(callBack, holdingQueryRequestVO);
		}
		private void QueryHoldingInfo(object _holdingQueryRequestVO)
		{
			HoldingQueryRequestVO holdingQueryRequestVO = (HoldingQueryRequestVO)_holdingQueryRequestVO;
			DataSet dataSet = this.dataProcess.QueryHoldingInfo(holdingQueryRequestVO);
			this.HandleCreated();
			base.Invoke(this.dgHoldingFill, new object[]
			{
				dataSet
			});
		}
		private void QueryHoldingDetailInfoLoad()
		{
			this.comboB_SF5.Items.Clear();
			this.comboB_SF5.Items.Add("全部");
			this.comboB_SF5.SelectedIndex = 0;
			this.comboB_SF5.Items.Add("买入");
			this.comboB_SF5.Items.Add("卖出");
			HoldingDetailRequestVO holdingDetailRequestVO = new HoldingDetailRequestVO();
			holdingDetailRequestVO.UserID = Global.UserID;
			this.dgHoldingDetailFill = new MainForm.DateSetCallback(this.DgHoldDetailFill);
			WaitCallback callBack = new WaitCallback(this.QueryHoldingDetailInfo);
			this.EnableControls(false, "数据查询中");
			ThreadPool.QueueUserWorkItem(callBack, holdingDetailRequestVO);
		}
		private void QueryHoldingDetailInfo(object _holdingDetailRequestVO)
		{
			HoldingDetailRequestVO holdingDetailRequestVO = (HoldingDetailRequestVO)_holdingDetailRequestVO;
			DataSet dataSet = this.dataProcess.QueryHoldingDetailInfo(holdingDetailRequestVO);
			this.HandleCreated();
			base.Invoke(this.dgHoldingDetailFill, new object[]
			{
				dataSet
			});
		}
		private void DgHoldingFill(DataSet holdingDataSet)
		{
			DataView dataView = new DataView(holdingDataSet.Tables["Holding"]);
			string text = " 1=1 ";
			if (this.comboCommodityF4.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF4.Text + "'";
			}
			if (this.comboTrancF4.SelectedIndex != 0)
			{
				text = text + " and TransactionsCode= '" + this.comboTrancF4.Text + "'";
			}
			dataView.RowFilter = text;
			this.dgHoldingCollect.DataSource = dataView;
			this.DataViewAddHoldingSum(dataView);
			for (int i = 0; i < this.dgHoldingCollect.Columns.Count; i++)
			{
				ColItemInfo colItemInfo = (ColItemInfo)this.holdingItemInfo.m_htItemInfo[this.dgHoldingCollect.Columns[i].Name];
				if (colItemInfo != null)
				{
					this.dgHoldingCollect.Columns[i].HeaderText = colItemInfo.name;
					this.dgHoldingCollect.Columns[i].MinimumWidth = colItemInfo.width;
					this.dgHoldingCollect.Columns[i].FillWeight = (float)colItemInfo.width;
					this.dgHoldingCollect.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
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
				}
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void DgHoldDetailFill(DataSet holdingDetailDataSet)
		{
			DataView dataView = new DataView(holdingDetailDataSet.Tables["HoldingDetail"]);
			string text = " 1=1 ";
			if (this.comboCommodityF4.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF4.Text + "'";
			}
			if (this.comboTrancF4.SelectedIndex != 0)
			{
				text = text + " and TransactionsCode= '" + this.comboTrancF4.Text + "'";
			}
			if (this.comboB_SF5.SelectedIndex == 1)
			{
				text = text + " and B_S = '" + Global.BuySellStrArr[1] + "' ";
			}
			else if (this.comboB_SF5.SelectedIndex == 2)
			{
				text = text + " and B_S = '" + Global.BuySellStrArr[2] + "' ";
			}
			dataView.RowFilter = text;
			this.dgHoldingDetail.DataSource = dataView;
			this.DataViewAddDetailHoldingSum(dataView);
			for (int i = 0; i < this.dgHoldingDetail.Columns.Count; i++)
			{
				ColItemInfo colItemInfo = (ColItemInfo)this.holdingDetailItemInfo.m_htItemInfo[this.dgHoldingDetail.Columns[i].Name];
				if (colItemInfo != null)
				{
					this.dgHoldingDetail.Columns[i].HeaderText = colItemInfo.name;
					this.dgHoldingDetail.Columns[i].MinimumWidth = colItemInfo.width;
					this.dgHoldingDetail.Columns[i].FillWeight = (float)colItemInfo.width;
					this.dgHoldingDetail.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
					if (colItemInfo.sortID == 1)
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
				}
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void QueryF4()
		{
			string text = " 1=1 ";
			string str = " and CommodityID <> '合计' ";
			if (this.comboCommodityF4.SelectedIndex != 0)
			{
				str = " and CommodityID = '" + this.comboCommodityF4.Text + "'";
			}
			text += str;
			if (this.comboTrancF4.SelectedIndex != 0)
			{
				text = text + " and TransactionsCode= '" + this.comboTrancF4.Text + "'";
			}
			DataView dataView = (DataView)this.dgHoldingCollect.DataSource;
			if (dataView == null)
			{
				return;
			}
			if (dataView.Count > 1)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			dataView.RowFilter = text;
			this.DataViewAddHoldingSum(dataView);
		}
		private void QueryDetailF4()
		{
			string text = " 1=1 ";
			if (this.comboCommodityF4.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF4.Text + "'";
			}
			if (this.comboTrancF4.SelectedIndex != 0)
			{
				text = text + " and TransactionsCode= '" + this.comboTrancF4.Text + "'";
			}
			if (this.comboB_SF5.SelectedIndex == 1)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
			}
			else if (this.comboB_SF5.SelectedIndex == 2)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
			}
			DataView dataView = (DataView)this.dgHoldingDetail.DataSource;
			if (dataView != null)
			{
				dataView.RowFilter = text;
				this.DataViewAddDetailHoldingSum(dataView);
			}
		}
		private void comboCommodityF4_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.LoadFlag && !this.QueryHoldingInfoFlag)
			{
				if (this.radioHdCollect.Checked)
				{
					this.QueryF4();
					return;
				}
				if (this.radioHdDetail.Checked)
				{
					this.QueryDetailF4();
				}
			}
		}
		private void DtatViewIsVisible_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.LoadFlag && !this.QueryHoldingInfoFlag)
			{
				if (this.radioHdCollect.Checked)
				{
					this.comboCommodityF4.SelectedIndex = 0;
					this.QueryF4();
					this.dgHoldingCollect.Visible = true;
					this.dgHoldingDetail.Visible = false;
					this.labelB_SF5.Visible = false;
					this.comboB_SF5.Visible = false;
					return;
				}
				if (this.radioHdDetail.Checked)
				{
					if (this.IsFTime)
					{
						this.QueryHoldingDetailInfoLoad();
						this.IsFTime = false;
					}
					else
					{
						this.QueryDetailF4();
					}
					this.dgHoldingCollect.Visible = false;
					this.dgHoldingDetail.Visible = true;
				}
			}
		}
		private void DataViewAddHoldingSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityID"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityID"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					dataView[j].Row["AutoID"] = 1;
					num += int.Parse(dataView[j].Row["BuyHolding"].ToString());
					num2 += double.Parse(dataView[j].Row["Margin"].ToString());
					num3 += double.Parse(dataView[j].Row["NewPriceLP"].ToString());
					num4 += double.Parse(dataView[j].Row["MarketValue"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityID"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["BuyHolding"] = num;
				dataRowView["Margin"] = num2.ToString("n");
				dataRowView["NewPriceLP"] = num3.ToString("n");
				dataRowView["MarketValue"] = num4.ToString("n");
				dataRowView["AutoID"] = 100;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				dataView.Sort = " AutoID ASC,  TransactionsCode ASC,  CommodityID ASC ";
				this.HjF5(this.dgHoldingCollect);
			}
		}
		private void DataViewAddDetailHoldingSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityID"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityID"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				int num2 = 0;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					dataView[j].Row["AutoID"] = 1;
					num += int.Parse(dataView[j].Row["GoodsQty"].ToString());
					num2 += int.Parse(dataView[j].Row["Cur_Open"].ToString());
					num3 += double.Parse(dataView[j].Row["Margin"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityID"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["Cur_Open"] = num2;
				dataRowView["GoodsQty"] = num;
				dataRowView["Margin"] = num3.ToString("n");
				dataRowView["AutoID"] = 100;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				dataView.Sort = " AutoID ASC,  TransactionsCode ASC,  CommodityID ASC ";
				this.HjF5(this.dgHoldingDetail);
			}
		}
		private void dgHoldingCollect_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex > 0)
			{
				if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
				{
					this.FillInfoText("双击可以市场最高买价产生该商品的卖出信息", Global.RightColor, this.displayInfo);
					return;
				}
				if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
				{
					this.FillInfoText("双击可以市场最低卖价产生该商品的卖出信息", Global.RightColor, this.displayInfo);
				}
			}
		}
		private void dgHoldingCollect_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
			{
				DataGridViewCell dataGridViewCell = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
				dataGridViewCell.ToolTipText = "双击以市场最高买价产生该商品的卖出委托";
				return;
			}
			if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
			{
				DataGridViewCell dataGridViewCell2 = this.dgHoldingCollect.Rows[e.RowIndex].Cells[e.ColumnIndex];
				dataGridViewCell2.ToolTipText = "双击以市场最低卖价产生商品的卖出委托";
			}
		}
		private void dgHoldingDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
			{
				DataGridViewCell dataGridViewCell = this.dgHoldingDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (this.dgHoldingDetail.Rows[e.RowIndex].Cells["B_S"].Value.ToString() == "买入")
				{
					dataGridViewCell.ToolTipText = "双击以市场最高买价产生该商品的卖出委托";
					return;
				}
				if (this.dgHoldingDetail.Rows[e.RowIndex].Cells["B_S"].Value.ToString() == "卖出")
				{
					dataGridViewCell.ToolTipText = "双击以市场最低卖价产生该商品的卖出委托";
				}
			}
		}
		private void dgHoldingDetail_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && (e.ColumnIndex == 3 || e.ColumnIndex == 4))
			{
				if (this.dgHoldingDetail.Rows[e.RowIndex].Cells["B_S"].Value.ToString() == "买入")
				{
					this.FillInfoText("双击可以市场最高买价产生该商品的卖出信息", Global.RightColor, this.displayInfo);
					return;
				}
				if (this.dgHoldingDetail.Rows[e.RowIndex].Cells["B_S"].Value.ToString() == "卖出")
				{
					this.FillInfoText("双击可以市场最低卖价产生该商品的卖出信息", Global.RightColor, this.displayInfo);
				}
			}
		}
		private bool setValue(DataGridView dgHoldingDetail, DataGridViewCellEventArgs e)
		{
			bool result = false;
			if (e.RowIndex != -1)
			{
				if (dgHoldingDetail.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == "合计")
				{
					this.FillInfoText("", Global.RightColor, this.displayInfo);
				}
				if (Global.MarketHT.Count > 1)
				{
					for (int i = 0; i < this.comboMarKet.Items.Count; i++)
					{
						AddValue addValue = (AddValue)this.comboMarKet.Items[i];
						if (dgHoldingDetail["Market", e.RowIndex].Value.ToString().Trim().Equals(addValue.Value))
						{
							this.comboMarKet.SelectedIndex = i;
							break;
						}
					}
					for (int j = 0; j < this.comboCommodity.Items.Count; j++)
					{
						if (dgHoldingDetail["CommodityID", e.RowIndex].Value.ToString().Trim().Equals(this.GetCommodityID(this.comboCommodity.Items[j].ToString())))
						{
							this.comboCommodity.SelectedIndex = j;
							result = true;
							break;
						}
					}
				}
				else
				{
					for (int k = 0; k < this.comboCommodity.Items.Count; k++)
					{
						string commodityID = this.GetCommodityID(this.comboCommodity.Items[k].ToString().Trim());
						if (dgHoldingDetail["CommodityID", e.RowIndex].Value.ToString().Trim().Equals(commodityID))
						{
							this.comboCommodity.SelectedIndex = k;
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}
		private void dgHoldingCollect_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.setValue(this.dgHoldingCollect, e))
			{
				CommData commData = this.dataProcess.QueryGNCommodityInfo(this.marketID, this.currentCommodity);
				if (e.ColumnIndex == this.dgHoldingCollect.Columns["BuyHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["BuyVHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["BuyAvg"].Index)
				{
					this.radioS.Checked = true;
					this.radioL.Checked = true;
					this.numericPrice.Value = this.ToDecimal(string.Concat(commData.Bid));
					this.numericQty.Value = this.ToDecimal(this.dgHoldingCollect["BuyVHolding", e.RowIndex].Value.ToString());
					return;
				}
				if (e.ColumnIndex == this.dgHoldingCollect.Columns["SellHolding"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["SellAvg"].Index || e.ColumnIndex == this.dgHoldingCollect.Columns["SellVHolding"].Index)
				{
					this.radioB.Checked = true;
					this.radioL.Checked = true;
					this.numericPrice.Value = this.ToDecimal(string.Concat(commData.Offer));
					this.numericQty.Value = this.ToDecimal(this.dgHoldingCollect["SellVHolding", e.RowIndex].Value.ToString());
					return;
				}
			}
			else
			{
				this.FillInfoText("商品下拉框中不存在该商品，请在设置中添加！", Global.ErrorColor, this.displayInfo);
			}
		}
		private void dgHoldingDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.setValue(this.dgHoldingDetail, e))
			{
				this.dataProcess.QueryGNCommodityInfo(this.marketID, this.currentCommodity);
				if (e.ColumnIndex == this.dgHoldingDetail.Columns["Price"].Index || e.ColumnIndex == this.dgHoldingDetail.Columns["B_S"].Index)
				{
					if (this.dgHoldingDetail["B_S", e.RowIndex].Value.Equals("买入"))
					{
						this.radioS.Checked = true;
					}
					else if (this.dgHoldingDetail["B_S", e.RowIndex].Value.Equals("卖出"))
					{
						this.radioB.Checked = true;
					}
					this.radioL.Checked = true;
					if (this.labelLPrice.Visible && this.numericLPrice.Visible)
					{
						this.numericLPrice.Value = decimal.Parse(this.dgHoldingDetail["Price", e.RowIndex].Value.ToString());
					}
					this.numericQty.Value = this.ToDecimal(this.dgHoldingDetail["Cur_Open", e.RowIndex].Value.ToString());
					return;
				}
			}
			else
			{
				this.FillInfoText("商品下拉框中不存在该商品，请在设置中添加！", Global.ErrorColor, this.displayInfo);
			}
		}
		private void dgHoldingCollect_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF5(this.dgHoldingCollect);
		}
		private void dgHoldingDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF5(this.dgHoldingDetail);
		}
		private void HjF5(DataGridView dg)
		{
			if (dg.RowCount > 1 && dg.Rows[dg.RowCount - 1].Cells[0].Value.ToString().Trim() == "合计")
			{
				dg.Rows[dg.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				dg.Rows[dg.RowCount - 1].ReadOnly = true;
			}
			else
			{
				for (int i = 0; i < dg.RowCount; i++)
				{
					string a = dg.Rows[i].Cells[0].Value.ToString().Trim();
					if (a == "合计")
					{
						dg.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
						dg.Rows[i].ReadOnly = true;
					}
					else
					{
						dg.Rows[i].DefaultCellStyle.BackColor = Color.White;
					}
				}
			}
			try
			{
				dg.Columns["AutoID"].Visible = false;
			}
			catch (Exception)
			{
			}
		}
		private void dgHoldingCollect_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			this.columnHeaderClickSort(this.dgHoldingCollect, e);
			this.HjF5(this.dgHoldingCollect);
		}
		private void dgHoldingDetail_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			this.columnHeaderClickSort(this.dgHoldingDetail, e);
		}
		private void columnHeaderClickSort(DataGridView dg, DataGridViewCellMouseEventArgs e)
		{
			DataView dataView = (DataView)dg.DataSource;
			try
			{
				dataView.Sort = " AutoID ASC, " + dg.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				dataView.Sort = " " + dg.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
		}
		private void QueryTradeInfoLoad()
		{
			this.isFTradeSum = true;
			if (this.radioTradeDetail.Checked)
			{
				this.dgTrade.Visible = true;
				this.dgTradeSum.Visible = false;
				this.labelB_S.Visible = true;
				this.comboB_S.Visible = true;
				this.TradeQueryThread();
				return;
			}
			if (this.radioTradeSum.Checked)
			{
				this.dgTrade.Visible = false;
				this.dgTradeSum.Visible = true;
				this.labelB_S.Visible = false;
				this.comboB_S.Visible = false;
				this.TradeSumQueryThread();
			}
		}
		private void TradeQueryThread()
		{
			TradeQueryRequestVO tradeQueryRequestVO = new TradeQueryRequestVO();
			tradeQueryRequestVO.UserID = Global.UserID;
			this.dgTradeFill = new MainForm.DateSetCallback(this.DgTradeFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryTradeInfo);
			ThreadPool.QueueUserWorkItem(callBack, tradeQueryRequestVO);
		}
		private void TradeSumQueryThread()
		{
			TradeSumQueryRequestVO tradeSumQueryRequestVO = new TradeSumQueryRequestVO();
			tradeSumQueryRequestVO.UserID = Global.UserID;
			this.dgTradeSumFill = new MainForm.DateSetCallback(this.DgTradeSumFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryTradeSumInfo);
			ThreadPool.QueueUserWorkItem(callBack, tradeSumQueryRequestVO);
		}
		private void QueryTradeInfo(object _tradeQueryRequestVO)
		{
			try
			{
				if (this.refreshTradeFlag)
				{
					this.refreshTradeFlag = false;
					TradeQueryRequestVO tradeQueryRequestVO = (TradeQueryRequestVO)_tradeQueryRequestVO;
					DataSet dataSet = this.dataProcess.QueryTradeInfo(tradeQueryRequestVO);
					this.HandleCreated();
					base.Invoke(this.dgTradeFill, new object[]
					{
						dataSet
					});
					this.refreshTradeFlag = true;
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("QueryTradeInfo异常：" + ex.Message);
			}
		}
		private void QueryTradeSumInfo(object _TradeQuerySumRequestVO)
		{
			try
			{
				if (this.refreshTradeSumFlag)
				{
					this.refreshTradeSumFlag = false;
					TradeSumQueryRequestVO tradeSumQueryRequestVO = (TradeSumQueryRequestVO)_TradeQuerySumRequestVO;
					DataSet dataSet = this.dataProcess.QuerySumTradeInfo(tradeSumQueryRequestVO);
					this.HandleCreated();
					base.Invoke(this.dgTradeSumFill, new object[]
					{
						dataSet
					});
					this.refreshTradeSumFlag = true;
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("QueryTradeSumInfo异常：" + ex.Message);
			}
		}
		private void DgTradeFill(DataSet tradeDataSet)
		{
			if (tradeDataSet != null)
			{
				DataView dataView = new DataView(tradeDataSet.Tables["Trade"]);
				string text = " 1=1 ";
				if (this.comboCommodityF3.SelectedIndex != 0)
				{
					text = text + " and CommodityID = '" + this.comboCommodityF3.Text + "' ";
				}
				if (this.comboTrancF3.SelectedIndex != 0)
				{
					text = text + " and TransactionsCode= '" + this.comboTrancF3.Text + "' ";
				}
				if (this.comboB_S.SelectedIndex == 1)
				{
					text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
				}
				else if (this.comboB_S.SelectedIndex == 2)
				{
					text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
				}
				if (this.radioOF3.Checked)
				{
					text = text + " and O_L='" + Global.SettleBasisStrArr[1] + "' ";
				}
				else if (this.radioLF3.Checked)
				{
					text = text + " and O_L='" + Global.SettleBasisStrArr[2] + "' ";
				}
				dataView.RowFilter = text;
				this.DataViewAddSum(dataView);
				this.dgTrade.DataSource = dataView;
				dataView.Sort = " TradeNo desc ";
				this.dgTrade.Sort(this.dgTrade.Columns[0], ListSortDirection.Descending);
				for (int i = 0; i < this.dgTrade.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.tradeItemInfo.m_htItemInfo[this.dgTrade.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgTrade.Columns[i].HeaderText = colItemInfo.name;
						this.dgTrade.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgTrade.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgTrade.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						if (colItemInfo.sortID == 1)
						{
							this.dgTrade.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgTrade.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.tradeItemInfo.m_strItems.Contains(this.dgTrade.Columns[i].Name + ";"))
						{
							this.dgTrade.Columns[i].Visible = false;
						}
					}
				}
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void DgTradeSumFill(DataSet tradeSumDataSet)
		{
			if (tradeSumDataSet != null)
			{
				DataView dataView = new DataView(tradeSumDataSet.Tables["TradeSum"]);
				string text = " 1=1 ";
				if (this.comboCommodityF3.SelectedIndex != 0)
				{
					text = text + " and CommodityID = '" + this.comboCommodityF3.Text + "' ";
				}
				dataView.RowFilter = text;
				dataView.Sort = " CommodityID desc ";
				this.dgTradeSum.DataSource = dataView;
				for (int i = 0; i < this.dgTradeSum.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.tradeSumItemInfo.m_htItemInfo[this.dgTradeSum.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgTradeSum.Columns[i].HeaderText = colItemInfo.name;
						this.dgTradeSum.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgTradeSum.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgTradeSum.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						if (colItemInfo.sortID == 1)
						{
							this.dgTradeSum.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgTradeSum.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.tradeSumItemInfo.m_strItems.Contains(this.dgTradeSum.Columns[i].Name + ";"))
						{
							this.dgTradeSum.Columns[i].Visible = false;
						}
					}
				}
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void QueryF3()
		{
			DataView dataView = (DataView)this.dgTrade.DataSource;
			if (dataView == null)
			{
				return;
			}
			string text = " 1=1 ";
			if (this.comboCommodityF3.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF3.Text + "' ";
			}
			string str = " and TransactionsCode<>'' ";
			string str2 = "and B_S<>''";
			if (this.comboTrancF3.SelectedIndex != 0)
			{
				str = " and TransactionsCode= '" + this.comboTrancF3.Text + "' ";
			}
			else
			{
				text += str;
			}
			if (this.comboB_S.SelectedIndex == 1)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
			}
			else if (this.comboB_S.SelectedIndex == 2)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
			}
			else
			{
				text += str2;
			}
			if (this.radioOF3.Checked)
			{
				text = text + " and O_L='" + Global.SettleBasisStrArr[1] + "' ";
			}
			else if (this.radioLF3.Checked)
			{
				text = text + " and O_L='" + Global.SettleBasisStrArr[2] + "' ";
			}
			dataView.RowFilter = text;
			this.DataViewAddSum(dataView);
		}
		private void QuerySumF3()
		{
			DataView dataView = (DataView)this.dgTradeSum.DataSource;
			if (dataView == null)
			{
				return;
			}
			string text = " 1=1 ";
			if (this.comboCommodityF3.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF3.Text + "' ";
			}
			dataView.RowFilter = text;
		}
		private void comboCommodityF3_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is RadioButton)
			{
				RadioButton radioButton = (RadioButton)sender;
				if (!radioButton.Checked)
				{
					return;
				}
			}
			if (this.radioTradeDetail.Checked)
			{
				if (!this.LoadFlag && !this.QueryTradeInfoFlag)
				{
					this.QueryF3();
					return;
				}
			}
			else if (this.radioTradeSum.Checked && !this.LoadFlag && !this.QueryTradeInfoFlag)
			{
				this.QuerySumF3();
			}
		}
		private void DataViewAddSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["Time"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["Time"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (int)dataView[j].Row["Qty"];
					num2 += double.Parse(dataView[j].Row["Liqpl"].ToString());
					num3 += double.Parse(dataView[j].Row["Comm"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["Time"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["Qty"] = num;
				dataRowView["Liqpl"] = num2.ToString("n");
				dataRowView["Comm"] = num3.ToString("n");
				dataRowView["AutoID"] = 1000000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				this.HjF4();
			}
		}
		private void dgTrade_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF4();
		}
		private void HjF4()
		{
			if (this.dgTrade.RowCount > 1 && this.dgTrade.Rows[this.dgTrade.RowCount - 1].Cells[1].Value.ToString().Trim() == "合计")
			{
				this.dgTrade.Rows[this.dgTrade.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				this.dgTrade.Rows[this.dgTrade.RowCount - 1].ReadOnly = true;
			}
			try
			{
				this.dgTrade.Columns["AutoID"].Visible = false;
			}
			catch (Exception)
			{
			}
		}
		private void dgTrade_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgTrade.DataSource;
			DataView defaultView = dataTable.DefaultView;
			try
			{
				defaultView.Sort = " AutoID ASC, " + this.dgTrade.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				defaultView.Sort = " " + this.dgTrade.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
		}
		private void radioTradeDetail_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.LoadFlag && !this.QueryTradeInfoFlag)
			{
				if (this.radioTradeDetail.Checked)
				{
					if (this.isFTradeSum)
					{
						this.TradeQueryThread();
						this.isFTradeSum = false;
					}
					else
					{
						this.QueryF3();
					}
					this.dgTrade.Visible = true;
					this.dgTradeSum.Visible = false;
					this.labelB_S.Visible = true;
					this.comboB_S.Visible = true;
					return;
				}
				if (this.radioTradeSum.Checked)
				{
					if (this.isFTradeSum)
					{
						this.TradeSumQueryThread();
						this.isFTradeSum = false;
					}
					else
					{
						this.QuerySumF3();
					}
					this.dgTrade.Visible = false;
					this.dgTradeSum.Visible = true;
					this.labelB_S.Visible = false;
					this.comboB_S.Visible = false;
				}
			}
		}
		private void QueryOrderInfoLoad()
		{
			this.comboB_SF3.Items.Clear();
			this.comboB_SF3.Items.Add("全部");
			this.comboB_SF3.Items.Add("买入");
			this.comboB_SF3.Items.Add("卖出");
			this.comboB_SF3.SelectedIndex = 0;
			this.radioAllF2.Checked = true;
			OrderQueryRequestVO orderQueryRequestVO = new OrderQueryRequestVO();
			orderQueryRequestVO.UserID = Global.UserID;
			this.dgAllOrderFill = new MainForm.DateSetCallback(this.DgAllOrderFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryAllOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, orderQueryRequestVO);
		}
		private void QueryAllOrderInfo(object _orderQueryRequestVO)
		{
			OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)_orderQueryRequestVO;
			DataSet dataSet = new DataSet();
			dataSet = this.dataProcess.QueryTodayOrderInfo(orderQueryRequestVO);
			this.HandleCreated();
			base.Invoke(this.dgAllOrderFill, new object[]
			{
				dataSet
			});
		}
		private void DgAllOrderFill(DataSet allOrderDataSet)
		{
			if (allOrderDataSet != null)
			{
				DataView dataView = new DataView(allOrderDataSet.Tables["Order"]);
				string text = " 1=1 ";
				if (this.comboCommodityF2.SelectedIndex != 0)
				{
					text = text + " and CommodityID = '" + this.comboCommodityF2.Text + "' ";
				}
				if (this.comboTrancF2.SelectedIndex != 0)
				{
					text = text + " and TransactionsCode= '" + this.comboTrancF2.Text + "' ";
				}
				if (!this.radioAllF2.Checked)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						" and Status='",
						Global.OrderStatusStrArr[1],
						"' or Status='",
						Global.OrderStatusStrArr[2],
						"'  "
					});
				}
				if (this.comboB_SF3.SelectedIndex == 1)
				{
					text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
				}
				else if (this.comboB_SF3.SelectedIndex == 2)
				{
					text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
				}
				dataView.RowFilter = text;
				this.DataViewAddQueryF3Sum(dataView);
				this.dgAllOrder.DataSource = dataView;
				dataView.Sort = " OrderNo desc ";
				for (int i = 0; i < this.dgAllOrder.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.allOrderItemInfo.m_htItemInfo[this.dgAllOrder.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgAllOrder.Columns[i].HeaderText = colItemInfo.name;
						this.dgAllOrder.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgAllOrder.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgAllOrder.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						if (colItemInfo.sortID == 1)
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
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void QueryF2()
		{
			DataView dataView = (DataView)this.dgAllOrder.DataSource;
			if (dataView == null)
			{
				return;
			}
			string text = " 1=1 ";
			string str = " and TransactionsCode<>'' ";
			string str2 = " and B_S<>''";
			if (this.comboCommodityF2.SelectedIndex != 0)
			{
				text = text + " and CommodityID = '" + this.comboCommodityF2.Text + "' ";
			}
			if (this.comboTrancF2.SelectedIndex != 0)
			{
				str = " and TransactionsCode= '" + this.comboTrancF2.Text + "' ";
			}
			else
			{
				text += str;
			}
			if (!this.radioAllF2.Checked)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" and ( Status='",
					Global.OrderStatusStrArr[1],
					"' or Status='",
					Global.OrderStatusStrArr[2],
					"' ) "
				});
			}
			if (this.comboB_SF3.SelectedIndex == 1)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[1] + "' ";
			}
			else if (this.comboB_SF3.SelectedIndex == 2)
			{
				text = text + " and B_S='" + Global.BuySellStrArr[2] + "' ";
			}
			else
			{
				text += str2;
			}
			dataView.RowFilter = text;
			this.DataViewAddQueryF3Sum(dataView);
		}
		private void comboCommodityF2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is RadioButton)
			{
				RadioButton radioButton = (RadioButton)sender;
				if (!radioButton.Checked)
				{
					return;
				}
			}
			if (!this.LoadFlag && !this.QueryOrderInfoFlag)
			{
				this.QueryF2();
			}
		}
		private void buttonAllF2_Click(object sender, EventArgs e)
		{
			if (this.buttonAllF2.Text.Equals("全选"))
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
				this.buttonAllF2.Text = "全不选";
				return;
			}
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
			this.buttonAllF2.Text = "全选";
		}
		private void buttonCancelF2_Click(object sender, EventArgs e)
		{
			ArrayList arrayList = new ArrayList();
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = this.dgAllOrder.Rows.Count - 1; i >= 0; i--)
			{
				if (this.dgAllOrder["SelectFlagF2", i].Value != null && (bool)this.dgAllOrder["SelectFlagF2", i].Value)
				{
					if (this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[i].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
					{
						text = text + "--" + this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
						arrayList.Add(new WithDrawOrderRequestVO
						{
							UserID = Global.UserID,
							OrderNo = Tools.StrToLong(this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim())
						});
					}
					else
					{
						text2 = text2 + "--" + this.dgAllOrder.Rows[i].Cells["OrderNo"].Value.ToString().Trim();
					}
				}
			}
			if (!text2.Equals(""))
			{
				string message = "委托单号:【" + text2 + "】不属于可撤单";
				MessageForm messageForm = new MessageForm("撤单操作错误提示", message, 1);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
			if (arrayList.Count > 0)
			{
				MessageForm messageForm = new MessageForm("撤单操作", "确定要撤消委托单号为:【" + text + "】的委托吗？", 0);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					this.EnableControls(false, "撤单数据提交中");
					for (int j = 0; j < arrayList.Count; j++)
					{
						WithDrawOrderRequestVO state = (WithDrawOrderRequestVO)arrayList[j];
						WaitCallback callBack = new WaitCallback(this.WithDrawOrder);
						ThreadPool.QueueUserWorkItem(callBack, state);
					}
					return;
				}
			}
			else
			{
				MessageForm messageForm = new MessageForm("删除委托结果", "请选择您要撤的委托单后，点击撤单！", 1);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
		private void dgAllOrder_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				if (this.dgAllOrder.Rows[e.RowIndex].Cells[1].Value.ToString() == "合计")
				{
					return;
				}
				if (this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
				{
					MessageForm messageForm = new MessageForm("撤单操作", "确定要撤消委托单号为【" + this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim() + "】的委托单吗?", 0);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					if (messageForm.isOK)
					{
						WithDrawOrderRequestVO withDrawOrderRequestVO = new WithDrawOrderRequestVO();
						withDrawOrderRequestVO.UserID = Global.UserID;
						withDrawOrderRequestVO.OrderNo = Tools.StrToLong(this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim());
						this.EnableControls(false, "撤单数据提交中");
						WaitCallback callBack = new WaitCallback(this.WithDrawOrder);
						ThreadPool.QueueUserWorkItem(callBack, withDrawOrderRequestVO);
						return;
					}
				}
				else
				{
					string message = "委托单号:" + this.dgAllOrder.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString().Trim() + "不属于可撤单";
					MessageForm messageForm2 = new MessageForm("撤单操作错误提示", message, 1);
					messageForm2.Owner = this;
					messageForm2.ShowDialog();
					messageForm2.Dispose();
				}
			}
		}
		private void dgAllOrder_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				if (this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[1]) || this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value.Equals(Global.OrderStatusStrArr[2]))
				{
					this.FillInfoText("双击可撤单！", Global.RightColor, this.displayInfo);
					return;
				}
				this.FillInfoText(this.dgAllOrder.Rows[e.RowIndex].Cells["Status"].Value + "状态，不可撤单！", Global.RightColor, this.displayInfo);
			}
		}
		private void dgAllOrder_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				this.FillInfoText("", Global.RightColor, this.displayInfo);
			}
		}
		private void dgAllOrder_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
		}
		private void dgAllOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF3();
		}
		private void DataViewAddQueryF3Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["Time"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["Time"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				int num2 = 0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += int.Parse(dataView[j].Row["Qty"].ToString());
					num2 += int.Parse(dataView[j].Row["Balance"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["Time"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["Qty"] = num;
				dataRowView["Balance"] = num2;
				dataRowView["AutoID"] = 1000000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				this.HjF3();
			}
		}
		private void HjF3()
		{
			for (int i = 0; i < this.dgAllOrder.Rows.Count; i++)
			{
				string text = this.dgAllOrder.Rows[i].Cells["Status"].Value.ToString().Trim();
				if (!text.Equals(Global.OrderStatusStrArr[1]) && !text.Equals(Global.OrderStatusStrArr[2]))
				{
					if (text == "")
					{
						this.dgAllOrder.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
						this.dgAllOrder.Rows[i].ReadOnly = true;
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
			try
			{
				this.dgAllOrder.Columns["AutoID"].Visible = false;
			}
			catch (Exception)
			{
			}
		}
		private void dgAllOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				return;
			}
			DataTable dataTable = (DataTable)this.dgAllOrder.DataSource;
			DataView defaultView = dataTable.DefaultView;
			try
			{
				defaultView.Sort = " AutoID ASC, " + this.dgAllOrder.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				defaultView.Sort = " " + this.dgAllOrder.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
		}
		private void InitFieldInfo()
		{
			this.orderItemInfo = new OrderItemInfo();
			this.tradeOrderItemInfo = new TradeOrderItemInfo();
			this.allOrderItemInfo = new AllOrderItemInfo();
			this.tradeItemInfo = new TradeItemInfo();
			this.tradeSumItemInfo = new TradeSumItemInfo();
			this.holdingItemInfo = new HoldingItemInfo();
			this.holdingDetailItemInfo = new HoldingDetailItemInfo();
			this.fundsItemInfo = new FundsItemInfo();
			this.investorItemInfo = new InvestorItemInfo();
			this.commodityItemInfo = new CommodityItemInfo();
			this.preOrderItemInfo = new PreOrderItemInfo();
		}
		internal void DelegateRefresh()
		{
			if (IniData.GetInstance().AutoRefresh)
			{
				if (this.tabMain.SelectedIndex != 0)
				{
					this.DelegateFlag = true;
					this.QueryOrderInfoFlag = true;
					this.QueryTradeInfoFlag = true;
					this.QueryHoldingInfoFlag = true;
					this.QueryFundsInfoFlag = true;
					this.tabMain_SelectedIndexChanged(this, null);
				}
				else
				{
					this.QueryOrderInfoFlag = true;
					this.QueryTradeInfoFlag = true;
					this.QueryHoldingInfoFlag = true;
					this.QueryFundsInfoFlag = true;
					OrderQueryRequestVO orderQueryRequestVO = new OrderQueryRequestVO();
					orderQueryRequestVO.UserID = Global.UserID;
					this.dgOrderFill = new MainForm.DateSetArrCallback(this.DgOrderFill);
					WaitCallback callBack = new WaitCallback(this.QueryOrderInfo);
					ThreadPool.QueueUserWorkItem(callBack, orderQueryRequestVO);
				}
			}
			this.refreshFlag = false;
		}
		private void DelegateLoad()
		{
			OrderQueryRequestVO orderQueryRequestVO = new OrderQueryRequestVO();
			orderQueryRequestVO.UserID = Global.UserID;
			this.dgOrderFill = new MainForm.DateSetArrCallback(this.DgOrderFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, orderQueryRequestVO);
		}
		private void QueryOrderInfo(object _orderQueryRequestVO)
		{
			if (!this.refreshOTFlag)
			{
				return;
			}
			this.refreshOTFlag = false;
			OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)_orderQueryRequestVO;
			DataSet dataSet = this.dataProcess.QueryOrderInfo(orderQueryRequestVO);
			TradeQueryRequestVO tradeQueryRequestVO = new TradeQueryRequestVO();
			tradeQueryRequestVO.UserID = Global.UserID;
			tradeQueryRequestVO.MarketID = orderQueryRequestVO.MarketID;
			DataSet dataSet2 = this.dataProcess.QueryTradeOrderInfo(tradeQueryRequestVO);
			this.HandleCreated();
			base.Invoke(this.dgOrderFill, new object[]
			{
				dataSet,
				dataSet2
			});
			this.refreshOTFlag = true;
		}
		private void DgOrderFill(DataSet orderDataSet, DataSet tradeOrderDataSet)
		{
			if (orderDataSet != null && tradeOrderDataSet != null)
			{
				DataView dataView = new DataView(orderDataSet.Tables["Order"]);
				dataView.RowFilter = string.Concat(new string[]
				{
					"Status='",
					Global.OrderStatusStrArr[1],
					"' or Status='",
					Global.OrderStatusStrArr[2],
					"' "
				});
				this.DataViewAddQueryDgUnTradeSum(dataView);
				this.dgUnTrade.DataSource = dataView.Table;
				dataView.Table.DefaultView.Sort = "  OrderNo desc ";
				DataView dataView2 = new DataView(tradeOrderDataSet.Tables["Trade"]);
				this.DataViewAddQueryF2Sum(dataView2);
				this.dgTradeOrder.DataSource = dataView2.Table;
				dataView2.Table.DefaultView.Sort = " TradeNo desc ";
				for (int i = 0; i < this.dgTradeOrder.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.tradeOrderItemInfo.m_htItemInfo[this.dgTradeOrder.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgTradeOrder.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgTradeOrder.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgTradeOrder.Columns[i].HeaderText = colItemInfo.name;
						this.dgTradeOrder.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						if (colItemInfo.sortID == 1)
						{
							this.dgTradeOrder.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgTradeOrder.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.tradeOrderItemInfo.m_strItems.Contains(this.dgTradeOrder.Columns[i].Name + ";"))
						{
							this.dgTradeOrder.Columns[i].Visible = false;
						}
					}
				}
				for (int j = 0; j < this.dgUnTrade.Columns.Count; j++)
				{
					ColItemInfo colItemInfo2 = (ColItemInfo)this.orderItemInfo.m_htItemInfo[this.dgUnTrade.Columns[j].Name];
					if (colItemInfo2 != null)
					{
						this.dgUnTrade.Columns[j].MinimumWidth = colItemInfo2.width;
						this.dgUnTrade.Columns[j].FillWeight = (float)colItemInfo2.width;
						this.dgUnTrade.Columns[j].HeaderText = colItemInfo2.name;
						this.dgUnTrade.Columns[j].DefaultCellStyle.Format = colItemInfo2.format;
						if (colItemInfo2.sortID == 1)
						{
							this.dgUnTrade.Columns[j].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgUnTrade.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.orderItemInfo.m_strItems.Contains(this.dgUnTrade.Columns[j].Name))
						{
							this.dgUnTrade.Columns[j].Visible = false;
						}
					}
				}
			}
			this.EnableControls(true, "数据查询完毕");
		}
		protected void dgUnTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				WithDrawOrderRequestVO withDrawOrderRequestVO = new WithDrawOrderRequestVO();
				withDrawOrderRequestVO.UserID = Global.UserID;
				withDrawOrderRequestVO.OrderNo = Tools.StrToLong(this.dgUnTrade.Rows[this.dgUnTrade.CurrentRow.Index].Cells["OrderNo"].Value.ToString().Trim());
				if (withDrawOrderRequestVO.OrderNo == -1L)
				{
					return;
				}
				string message = "确定要撤消委托单号为【" + withDrawOrderRequestVO.OrderNo + "】的委托单吗？";
				MessageForm messageForm = new MessageForm("撤单操作", message, 0);
				messageForm.textSize = new Size(230, 48);
				messageForm.Owner = this;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					this.EnableControls(false, "撤单数据提交中");
					WaitCallback callBack = new WaitCallback(this.WithDrawOrder);
					ThreadPool.QueueUserWorkItem(callBack, withDrawOrderRequestVO);
				}
			}
		}
		private void WithDrawOrder(object _withDrawOrderRequestVO)
		{
			WithDrawOrderRequestVO req = (WithDrawOrderRequestVO)_withDrawOrderRequestVO;
			ResponseVO responseVO = this.dataProcess.WithDrawOrder(req);
			this.HandleCreated();
			MainForm.ResponseVOCallback method = new MainForm.ResponseVOCallback(this.WithDrawMessage);
			base.Invoke(method, new object[]
			{
				responseVO
			});
		}
		private void WithDrawMessage(ResponseVO responseVO)
		{
			if (responseVO.RetCode == 0L)
			{
				if (this.tabMain.SelectedIndex == 0)
				{
					this.DelegateLoad();
					this.QueryOrderInfoFlag = true;
				}
				else
				{
					this.QueryOrderInfoLoad();
					this.DelegateFlag = true;
				}
				this.FillInfoText("撤单成功！", Global.RightColor, this.displayInfo);
			}
			else
			{
				this.FillInfoText(responseVO.RetMessage, Global.ErrorColor, this.displayInfo);
			}
			this.EnableControls(true, "撤单数据提交完毕");
		}
		protected void dgUnTrade_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				this.FillInfoText("双击可撤单！", Global.RightColor, this.displayInfo);
			}
		}
		protected void dgUnTrade_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				this.FillInfoText("", Global.RightColor, this.displayInfo);
			}
		}
		private void dgUnTrade_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF2(this.dgUnTrade);
		}
		private void dgTradeOrder_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.HjF2(this.dgTradeOrder);
		}
		private void DataViewAddQueryDgUnTradeSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["Time"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["Time"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				int num2 = 0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += int.Parse(dataView[j].Row["Qty"].ToString());
					num2 += int.Parse(dataView[j].Row["Balance"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["Time"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["Qty"] = num;
				dataRowView["Balance"] = num2;
				dataRowView["AutoID"] = 100000;
				dataRowView.EndEdit();
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				this.HjF2(this.dgUnTrade);
			}
		}
		private void DataViewAddQueryF2Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["OrderNo"].ToString() == "合计")
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["OrderNo"].ToString() == "合计")
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				int num = 0;
				double num2 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (int)dataView[j].Row["Qty"];
					num2 += double.Parse(dataView[j].Row["Comm"].ToString());
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["OrderNo"] = "合计";
				dataRowView["TransactionsCode"] = "共" + (dataView.Count - 1) + "条";
				dataRowView["Qty"] = num;
				dataRowView["Comm"] = num2.ToString("n");
				dataRowView["AutoID"] = 1000000;
				dataRowView.EndEdit();
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
				this.HjF2(this.dgTradeOrder);
			}
		}
		private void HjF2(DataGridView dg)
		{
			if (dg.RowCount > 1 && dg.Rows[dg.RowCount - 1].Cells[1].Value.ToString().Trim() == "合计")
			{
				dg.Rows[dg.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;
				dg.Rows[dg.RowCount - 1].ReadOnly = true;
			}
		}
		private void dgTradeOrder_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void dgUnTrade_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgUnTrade.DataSource;
			DataView defaultView = dataTable.DefaultView;
			try
			{
				defaultView.Sort = " AutoID ASC, " + this.dgUnTrade.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				defaultView.Sort = " " + this.dgUnTrade.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
		}
		private void dgTradeOrder_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgTradeOrder.DataSource;
			DataView defaultView = dataTable.DefaultView;
			try
			{
				defaultView.Sort = " AutoID ASC, " + this.dgTradeOrder.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				defaultView.Sort = " " + this.dgTradeOrder.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
			this.splitOrder = new SplitContainer();
			this.groupBoxUnTrade = new GroupBox();
			this.butConditionOrder = new Button();
			this.dgUnTrade = new DataGridView();
			this.MenuRefresh = new ContextMenuStrip(this.components);
			this.ToolStripMenuItem = new ToolStripMenuItem();
			this.groupBoxTrade = new GroupBox();
			this.dgTradeOrder = new DataGridView();
			this.m_splitContainer = new SplitContainer();
			this.toolStrip = new ToolStrip();
			this.toolStripButtonBill = new ToolStripButton();
			this.toolStripButtonOrder = new ToolStripButton();
			this.toolStripButtonSet = new ToolStripButton();
			this.toolStripButtonMsg = new ToolStripButton();
			this.toolStripButtonLock = new ToolStripButton();
			this.toolStripButtonHelp = new ToolStripButton();
			this.toolStripButtonAbout = new ToolStripButton();
			this.toolStripButtonExit = new ToolStripButton();
			this.tabMain = new TabControl();
			this.TabPageF2 = new TabPage();
			this.TabPageF3 = new TabPage();
			this.groupBoxF2 = new GroupBox();
			this.dgAllOrder = new DataGridView();
			this.SelectFlagF2 = new DataGridViewCheckBoxColumn();
			this.comboB_SF3 = new ComboBox();
			this.labelB_SF3 = new Label();
			this.comboTrancF2 = new ComboBox();
			this.comboCommodityF2 = new ComboBox();
			this.labelTrancF2 = new Label();
			this.labelCommodityF2 = new Label();
			this.buttonCancelF2 = new Button();
			this.buttonAllF2 = new Button();
			this.buttonSelF2 = new Button();
			this.groupBoxF2_1 = new GroupBox();
			this.radioCancelF2 = new RadioButton();
			this.radioAllF2 = new RadioButton();
			this.TabPageF4 = new TabPage();
			this.groupBoxF3 = new GroupBox();
			this.dgTradeSum = new DataGridView();
			this.comboB_S = new ComboBox();
			this.labelB_S = new Label();
			this.dgTrade = new DataGridView();
			this.comboTrancF3 = new ComboBox();
			this.comboCommodityF3 = new ComboBox();
			this.labelTrancF3 = new Label();
			this.labelCommodityF3 = new Label();
			this.buttonSelF3 = new Button();
			this.groupBoxF3_1 = new GroupBox();
			this.radioTradeSum = new RadioButton();
			this.radioTradeDetail = new RadioButton();
			this.radioOF3 = new RadioButton();
			this.radioLF3 = new RadioButton();
			this.radioAllF3 = new RadioButton();
			this.TabPageF5 = new TabPage();
			this.groupBoxF4 = new GroupBox();
			this.dgHoldingDetail = new DataGridView();
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
			this.TabPageF6 = new TabPage();
			this.groupBoxMoney = new GroupBox();
			this.buttonFundsTransfer = new Button();
			this.buttonSelFundsF4 = new Button();
			this.lstVFunds = new ListView();
			this.TabPageF8 = new TabPage();
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
			this.TabPageF9 = new TabPage();
			this.groupBoxInvestor = new GroupBox();
			this.listVInvestor = new ListView();
			this.groupBoxGNCommodit = new GroupBox();
			this.panel1 = new Panel();
			this.butMinLine = new Button();
			this.butKLine = new Button();
			this.labelSpread = new Label();
			this.labelGNInfo = new Label();
			this.labelBP2 = new Label();
			this.labelSP2 = new Label();
			this.labelBV3 = new Label();
			this.labelSV1 = new Label();
			this.labelLimitDownV = new Label();
			this.labelPrevClearV = new Label();
			this.labelLimitUpV = new Label();
			this.labelLimitUp = new Label();
			this.labelBP3 = new Label();
			this.labelSP1 = new Label();
			this.labelB3 = new Label();
			this.labelS1 = new Label();
			this.labelBV1 = new Label();
			this.labelSV3 = new Label();
			this.labelLimitDown = new Label();
			this.labelLast = new Label();
			this.labelBP1 = new Label();
			this.labelSP3 = new Label();
			this.labelB1 = new Label();
			this.labelBV2 = new Label();
			this.labelS3 = new Label();
			this.labelCountV = new Label();
			this.labelLastP = new Label();
			this.labelSV2 = new Label();
			this.labelB2 = new Label();
			this.labelCount = new Label();
			this.labelS2 = new Label();
			this.groupBoxOrder = new GroupBox();
			this.comboTranc = new ComboBox();
			this.tbTranc = new TextBox();
			this.comboCommodity = new ComboBox();
			this.labelLargestTN = new Label();
			this.buttonAddPre = new Button();
			this.buttonOrder = new Button();
			this.numericQty = new NumericUpDown();
			this.numericPrice = new NumericUpDown();
			this.labQty = new Label();
			this.labPrice = new Label();
			this.labComCode = new Label();
			this.groupBoxB_S = new GroupBox();
			this.radioS = new RadioButton();
			this.radioB = new RadioButton();
			this.gbCloseMode = new GroupBox();
			this.rbCloseH = new RadioButton();
			this.rbCloseT = new RadioButton();
			this.groupBoxO_L = new GroupBox();
			this.radioL = new RadioButton();
			this.radioO = new RadioButton();
			this.numericLPrice = new NumericUpDown();
			this.labelLPrice = new Label();
			this.comboMarKet = new ComboBox();
			this.labTrancCode = new Label();
			this.labelMarKet = new Label();
			this.tradeTime = new System.Windows.Forms.Timer(this.components);
			this.comboBox7 = new ComboBox();
			this.dataGridViewTextBoxColumn46 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn47 = new DataGridViewTextBoxColumn();
			this.Column9 = new DataGridViewTextBoxColumn();
			this.Column10 = new DataGridViewTextBoxColumn();
			this.statusInfo = new StatusStrip();
			this.info = new ToolStripStatusLabel();
			this.user = new ToolStripStatusLabel();
			this.status = new ToolStripStatusLabel();
			this.time = new ToolStripStatusLabel();
			this.MessageInfo = new Label();
			this.timerLock = new System.Windows.Forms.Timer(this.components);
			this.toolTip = new ToolTip(this.components);
			this.SplitterPanel = new Panel();
			this.helpProvider = new HelpProvider();
			this.buttonUnLock = new Button();
			this.panelLock = new Panel();
			this.labelPwdInfo = new Label();
			this.labelPwd = new Label();
			this.textBoxPwd = new TextBox();
			this.timerSysTime = new System.Windows.Forms.Timer(this.components);
			this.splitOrder.Panel1.SuspendLayout();
			this.splitOrder.Panel2.SuspendLayout();
			this.splitOrder.SuspendLayout();
			this.groupBoxUnTrade.SuspendLayout();
			((ISupportInitialize)this.dgUnTrade).BeginInit();
			this.MenuRefresh.SuspendLayout();
			this.groupBoxTrade.SuspendLayout();
			((ISupportInitialize)this.dgTradeOrder).BeginInit();
			this.m_splitContainer.Panel1.SuspendLayout();
			this.m_splitContainer.Panel2.SuspendLayout();
			this.m_splitContainer.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.TabPageF2.SuspendLayout();
			this.TabPageF3.SuspendLayout();
			this.groupBoxF2.SuspendLayout();
			((ISupportInitialize)this.dgAllOrder).BeginInit();
			this.groupBoxF2_1.SuspendLayout();
			this.TabPageF4.SuspendLayout();
			this.groupBoxF3.SuspendLayout();
			((ISupportInitialize)this.dgTradeSum).BeginInit();
			((ISupportInitialize)this.dgTrade).BeginInit();
			this.groupBoxF3_1.SuspendLayout();
			this.TabPageF5.SuspendLayout();
			this.groupBoxF4.SuspendLayout();
			((ISupportInitialize)this.dgHoldingDetail).BeginInit();
			this.groupBoxF4_1.SuspendLayout();
			((ISupportInitialize)this.dgHoldingCollect).BeginInit();
			this.TabPageF6.SuspendLayout();
			this.groupBoxMoney.SuspendLayout();
			this.TabPageF8.SuspendLayout();
			this.groupBoxF7.SuspendLayout();
			((ISupportInitialize)this.dgPreDelegate).BeginInit();
			this.TabPageF9.SuspendLayout();
			this.groupBoxInvestor.SuspendLayout();
			this.groupBoxGNCommodit.SuspendLayout();
			this.groupBoxOrder.SuspendLayout();
			((ISupportInitialize)this.numericQty).BeginInit();
			((ISupportInitialize)this.numericPrice).BeginInit();
			this.groupBoxB_S.SuspendLayout();
			this.gbCloseMode.SuspendLayout();
			this.groupBoxO_L.SuspendLayout();
			((ISupportInitialize)this.numericLPrice).BeginInit();
			this.statusInfo.SuspendLayout();
			this.panelLock.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.splitOrder, "splitOrder");
			this.splitOrder.FixedPanel = FixedPanel.Panel1;
			this.splitOrder.Name = "splitOrder";
			componentResourceManager.ApplyResources(this.splitOrder.Panel1, "splitOrder.Panel1");
			this.splitOrder.Panel1.Controls.Add(this.groupBoxUnTrade);
			this.helpProvider.SetShowHelp(this.splitOrder.Panel1, (bool)componentResourceManager.GetObject("splitOrder.Panel1.ShowHelp"));
			componentResourceManager.ApplyResources(this.splitOrder.Panel2, "splitOrder.Panel2");
			this.splitOrder.Panel2.Controls.Add(this.groupBoxTrade);
			this.helpProvider.SetShowHelp(this.splitOrder.Panel2, (bool)componentResourceManager.GetObject("splitOrder.Panel2.ShowHelp"));
			this.helpProvider.SetShowHelp(this.splitOrder, (bool)componentResourceManager.GetObject("splitOrder.ShowHelp"));
			this.groupBoxUnTrade.Controls.Add(this.butConditionOrder);
			this.groupBoxUnTrade.Controls.Add(this.dgUnTrade);
			componentResourceManager.ApplyResources(this.groupBoxUnTrade, "groupBoxUnTrade");
			this.helpProvider.SetHelpKeyword(this.groupBoxUnTrade, componentResourceManager.GetString("groupBoxUnTrade.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.groupBoxUnTrade, (HelpNavigator)componentResourceManager.GetObject("groupBoxUnTrade.HelpNavigator"));
			this.helpProvider.SetHelpString(this.groupBoxUnTrade, componentResourceManager.GetString("groupBoxUnTrade.HelpString"));
			this.groupBoxUnTrade.Name = "groupBoxUnTrade";
			this.helpProvider.SetShowHelp(this.groupBoxUnTrade, (bool)componentResourceManager.GetObject("groupBoxUnTrade.ShowHelp"));
			this.groupBoxUnTrade.TabStop = false;
			this.butConditionOrder.BackColor = Color.FromArgb(233, 233, 233);
			this.butConditionOrder.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.butConditionOrder, "butConditionOrder");
			this.butConditionOrder.Name = "butConditionOrder";
			this.helpProvider.SetShowHelp(this.butConditionOrder, (bool)componentResourceManager.GetObject("butConditionOrder.ShowHelp"));
			this.butConditionOrder.UseVisualStyleBackColor = false;
			this.butConditionOrder.Click += new EventHandler(this.butConditionOrder_Click);
			this.dgUnTrade.AllowUserToAddRows = false;
			this.dgUnTrade.AllowUserToDeleteRows = false;
			this.dgUnTrade.AllowUserToResizeRows = false;
			this.dgUnTrade.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgUnTrade.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgUnTrade.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgUnTrade.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgUnTrade.ContextMenuStrip = this.MenuRefresh;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgUnTrade.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dgUnTrade, "dgUnTrade");
			this.helpProvider.SetHelpKeyword(this.dgUnTrade, componentResourceManager.GetString("dgUnTrade.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgUnTrade, (HelpNavigator)componentResourceManager.GetObject("dgUnTrade.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgUnTrade, componentResourceManager.GetString("dgUnTrade.HelpString"));
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
			this.dgUnTrade.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.helpProvider.SetShowHelp(this.dgUnTrade, (bool)componentResourceManager.GetObject("dgUnTrade.ShowHelp"));
			this.dgUnTrade.TabStop = false;
			this.dgUnTrade.CellClick += new DataGridViewCellEventHandler(this.dgUnTrade_CellClick);
			this.dgUnTrade.CellDoubleClick += new DataGridViewCellEventHandler(this.dgUnTrade_CellDoubleClick);
			this.dgUnTrade.CellMouseEnter += new DataGridViewCellEventHandler(this.dgUnTrade_CellMouseEnter);
			this.dgUnTrade.CellMouseLeave += new DataGridViewCellEventHandler(this.dgUnTrade_CellMouseLeave);
			this.dgUnTrade.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgUnTrade_ColumnHeaderMouseClick);
			this.dgUnTrade.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgUnTrade_DataBindingComplete);
			this.MenuRefresh.Items.AddRange(new ToolStripItem[]
			{
				this.ToolStripMenuItem
			});
			this.MenuRefresh.Name = "MenuRefresh";
			this.helpProvider.SetShowHelp(this.MenuRefresh, (bool)componentResourceManager.GetObject("MenuRefresh.ShowHelp"));
			componentResourceManager.ApplyResources(this.MenuRefresh, "MenuRefresh");
			this.ToolStripMenuItem.Name = "ToolStripMenuItem";
			componentResourceManager.ApplyResources(this.ToolStripMenuItem, "ToolStripMenuItem");
			this.ToolStripMenuItem.Click += new EventHandler(this.ToolStripMenuItem_Click);
			this.groupBoxTrade.Controls.Add(this.dgTradeOrder);
			componentResourceManager.ApplyResources(this.groupBoxTrade, "groupBoxTrade");
			this.helpProvider.SetHelpKeyword(this.groupBoxTrade, componentResourceManager.GetString("groupBoxTrade.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.groupBoxTrade, (HelpNavigator)componentResourceManager.GetObject("groupBoxTrade.HelpNavigator"));
			this.helpProvider.SetHelpString(this.groupBoxTrade, componentResourceManager.GetString("groupBoxTrade.HelpString"));
			this.groupBoxTrade.Name = "groupBoxTrade";
			this.helpProvider.SetShowHelp(this.groupBoxTrade, (bool)componentResourceManager.GetObject("groupBoxTrade.ShowHelp"));
			this.groupBoxTrade.TabStop = false;
			this.dgTradeOrder.AllowUserToAddRows = false;
			this.dgTradeOrder.AllowUserToDeleteRows = false;
			this.dgTradeOrder.AllowUserToResizeRows = false;
			this.dgTradeOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgTradeOrder.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = SystemColors.Control;
			dataGridViewCellStyle4.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
			this.dgTradeOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgTradeOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgTradeOrder.ContextMenuStrip = this.MenuRefresh;
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = SystemColors.Window;
			dataGridViewCellStyle5.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
			this.dgTradeOrder.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dgTradeOrder, "dgTradeOrder");
			this.helpProvider.SetHelpKeyword(this.dgTradeOrder, componentResourceManager.GetString("dgTradeOrder.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgTradeOrder, (HelpNavigator)componentResourceManager.GetObject("dgTradeOrder.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgTradeOrder, componentResourceManager.GetString("dgTradeOrder.HelpString"));
			this.dgTradeOrder.Name = "dgTradeOrder";
			this.dgTradeOrder.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = SystemColors.Control;
			dataGridViewCellStyle6.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
			this.dgTradeOrder.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgTradeOrder.RowHeadersVisible = false;
			this.dgTradeOrder.RowTemplate.Height = 16;
			this.dgTradeOrder.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.helpProvider.SetShowHelp(this.dgTradeOrder, (bool)componentResourceManager.GetObject("dgTradeOrder.ShowHelp"));
			this.dgTradeOrder.TabStop = false;
			this.dgTradeOrder.CellClick += new DataGridViewCellEventHandler(this.dgTradeOrder_CellClick);
			this.dgTradeOrder.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgTradeOrder_ColumnHeaderMouseClick);
			this.dgTradeOrder.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgTradeOrder_DataBindingComplete);
			componentResourceManager.ApplyResources(this.m_splitContainer, "m_splitContainer");
			this.m_splitContainer.FixedPanel = FixedPanel.Panel2;
			this.m_splitContainer.Name = "m_splitContainer";
			componentResourceManager.ApplyResources(this.m_splitContainer.Panel1, "m_splitContainer.Panel1");
			this.m_splitContainer.Panel1.Controls.Add(this.toolStrip);
			this.m_splitContainer.Panel1.Controls.Add(this.tabMain);
			componentResourceManager.ApplyResources(this.m_splitContainer.Panel2, "m_splitContainer.Panel2");
			this.m_splitContainer.Panel2.Controls.Add(this.groupBoxGNCommodit);
			this.m_splitContainer.Panel2.Controls.Add(this.groupBoxOrder);
			this.m_splitContainer.SplitterMoved += new SplitterEventHandler(this.m_splitContainer_SplitterMoved);
			componentResourceManager.ApplyResources(this.toolStrip, "toolStrip");
			this.toolStrip.BackColor = SystemColors.Control;
			this.toolStrip.GripMargin = new Padding(0);
			this.helpProvider.SetHelpKeyword(this.toolStrip, componentResourceManager.GetString("toolStrip.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.toolStrip, (HelpNavigator)componentResourceManager.GetObject("toolStrip.HelpNavigator"));
			this.helpProvider.SetHelpString(this.toolStrip, componentResourceManager.GetString("toolStrip.HelpString"));
			this.toolStrip.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripButtonBill,
				this.toolStripButtonOrder,
				this.toolStripButtonSet,
				this.toolStripButtonMsg,
				this.toolStripButtonLock,
				this.toolStripButtonHelp,
				this.toolStripButtonAbout,
				this.toolStripButtonExit
			});
			this.toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
			this.toolStrip.Name = "toolStrip";
			this.helpProvider.SetShowHelp(this.toolStrip, (bool)componentResourceManager.GetObject("toolStrip.ShowHelp"));
			this.toolStripButtonBill.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonBill, "toolStripButtonBill");
			this.toolStripButtonBill.Name = "toolStripButtonBill";
			this.toolStripButtonBill.Click += new EventHandler(this.toolStripButtonBill_Click);
			this.toolStripButtonOrder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonOrder, "toolStripButtonOrder");
			this.toolStripButtonOrder.Name = "toolStripButtonOrder";
			this.toolStripButtonOrder.Click += new EventHandler(this.toolStripButtonOrder_Click);
			this.toolStripButtonSet.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonSet, "toolStripButtonSet");
			this.toolStripButtonSet.Name = "toolStripButtonSet";
			this.toolStripButtonSet.Click += new EventHandler(this.toolStripButtonSet_Click);
			this.toolStripButtonMsg.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonMsg, "toolStripButtonMsg");
			this.toolStripButtonMsg.Name = "toolStripButtonMsg";
			this.toolStripButtonMsg.Click += new EventHandler(this.toolStripButtonMsg_Click);
			this.toolStripButtonLock.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonLock, "toolStripButtonLock");
			this.toolStripButtonLock.Name = "toolStripButtonLock";
			this.toolStripButtonLock.Click += new EventHandler(this.toolStripButtonLock_Click);
			this.toolStripButtonHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonHelp, "toolStripButtonHelp");
			this.toolStripButtonHelp.Name = "toolStripButtonHelp";
			this.toolStripButtonHelp.Click += new EventHandler(this.toolStripButtonHelp_Click);
			this.toolStripButtonAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonAbout, "toolStripButtonAbout");
			this.toolStripButtonAbout.Name = "toolStripButtonAbout";
			this.toolStripButtonAbout.Click += new EventHandler(this.toolStripButtonAbout_Click);
			this.toolStripButtonExit.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonExit, "toolStripButtonExit");
			this.toolStripButtonExit.Name = "toolStripButtonExit";
			this.toolStripButtonExit.Click += new EventHandler(this.toolStripButtonExit_Click);
			this.tabMain.Controls.Add(this.TabPageF2);
			this.tabMain.Controls.Add(this.TabPageF3);
			this.tabMain.Controls.Add(this.TabPageF4);
			this.tabMain.Controls.Add(this.TabPageF5);
			this.tabMain.Controls.Add(this.TabPageF6);
			this.tabMain.Controls.Add(this.TabPageF8);
			this.tabMain.Controls.Add(this.TabPageF9);
			componentResourceManager.ApplyResources(this.tabMain, "tabMain");
			this.helpProvider.SetHelpKeyword(this.tabMain, componentResourceManager.GetString("tabMain.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.tabMain, (HelpNavigator)componentResourceManager.GetObject("tabMain.HelpNavigator"));
			this.helpProvider.SetHelpString(this.tabMain, componentResourceManager.GetString("tabMain.HelpString"));
			this.tabMain.Multiline = true;
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.helpProvider.SetShowHelp(this.tabMain, (bool)componentResourceManager.GetObject("tabMain.ShowHelp"));
			this.tabMain.TabStop = false;
			this.tabMain.DrawItem += new DrawItemEventHandler(this.tabMain_DrawItem);
			this.tabMain.SelectedIndexChanged += new EventHandler(this.tabMain_SelectedIndexChanged);
			this.tabMain.Selecting += new TabControlCancelEventHandler(this.tabMain_Selecting);
			componentResourceManager.ApplyResources(this.TabPageF2, "TabPageF2");
			this.TabPageF2.Controls.Add(this.splitOrder);
			this.helpProvider.SetHelpKeyword(this.TabPageF2, componentResourceManager.GetString("TabPageF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF2, (HelpNavigator)componentResourceManager.GetObject("TabPageF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF2, componentResourceManager.GetString("TabPageF2.HelpString"));
			this.TabPageF2.Name = "TabPageF2";
			this.helpProvider.SetShowHelp(this.TabPageF2, (bool)componentResourceManager.GetObject("TabPageF2.ShowHelp"));
			this.TabPageF2.UseVisualStyleBackColor = true;
			this.TabPageF3.Controls.Add(this.groupBoxF2);
			this.helpProvider.SetHelpKeyword(this.TabPageF3, componentResourceManager.GetString("TabPageF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF3, (HelpNavigator)componentResourceManager.GetObject("TabPageF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF3, componentResourceManager.GetString("TabPageF3.HelpString"));
			componentResourceManager.ApplyResources(this.TabPageF3, "TabPageF3");
			this.TabPageF3.Name = "TabPageF3";
			this.helpProvider.SetShowHelp(this.TabPageF3, (bool)componentResourceManager.GetObject("TabPageF3.ShowHelp"));
			this.TabPageF3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxF2, "groupBoxF2");
			this.groupBoxF2.Controls.Add(this.dgAllOrder);
			this.groupBoxF2.Controls.Add(this.comboB_SF3);
			this.groupBoxF2.Controls.Add(this.labelB_SF3);
			this.groupBoxF2.Controls.Add(this.comboTrancF2);
			this.groupBoxF2.Controls.Add(this.comboCommodityF2);
			this.groupBoxF2.Controls.Add(this.labelTrancF2);
			this.groupBoxF2.Controls.Add(this.labelCommodityF2);
			this.groupBoxF2.Controls.Add(this.buttonCancelF2);
			this.groupBoxF2.Controls.Add(this.buttonAllF2);
			this.groupBoxF2.Controls.Add(this.buttonSelF2);
			this.groupBoxF2.Controls.Add(this.groupBoxF2_1);
			this.helpProvider.SetHelpKeyword(this.groupBoxF2, componentResourceManager.GetString("groupBoxF2.HelpKeyword"));
			this.helpProvider.SetHelpString(this.groupBoxF2, componentResourceManager.GetString("groupBoxF2.HelpString"));
			this.groupBoxF2.Name = "groupBoxF2";
			this.helpProvider.SetShowHelp(this.groupBoxF2, (bool)componentResourceManager.GetObject("groupBoxF2.ShowHelp"));
			this.groupBoxF2.TabStop = false;
			this.dgAllOrder.AllowUserToAddRows = false;
			this.dgAllOrder.AllowUserToDeleteRows = false;
			this.dgAllOrder.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgAllOrder, "dgAllOrder");
			this.dgAllOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = SystemColors.Control;
			dataGridViewCellStyle7.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
			this.dgAllOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgAllOrder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgAllOrder.Columns.AddRange(new DataGridViewColumn[]
			{
				this.SelectFlagF2
			});
			this.dgAllOrder.ContextMenuStrip = this.MenuRefresh;
			dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = SystemColors.Window;
			dataGridViewCellStyle8.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
			this.dgAllOrder.DefaultCellStyle = dataGridViewCellStyle8;
			this.helpProvider.SetHelpKeyword(this.dgAllOrder, componentResourceManager.GetString("dgAllOrder.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgAllOrder, (HelpNavigator)componentResourceManager.GetObject("dgAllOrder.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgAllOrder, componentResourceManager.GetString("dgAllOrder.HelpString"));
			this.dgAllOrder.Name = "dgAllOrder";
			dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = SystemColors.Control;
			dataGridViewCellStyle9.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
			this.dgAllOrder.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dgAllOrder.RowHeadersVisible = false;
			this.dgAllOrder.RowTemplate.Height = 18;
			this.helpProvider.SetShowHelp(this.dgAllOrder, (bool)componentResourceManager.GetObject("dgAllOrder.ShowHelp"));
			this.dgAllOrder.TabStop = false;
			this.dgAllOrder.CellClick += new DataGridViewCellEventHandler(this.dgAllOrder_CellClick);
			this.dgAllOrder.CellDoubleClick += new DataGridViewCellEventHandler(this.dgAllOrder_CellDoubleClick);
			this.dgAllOrder.CellMouseEnter += new DataGridViewCellEventHandler(this.dgAllOrder_CellMouseEnter);
			this.dgAllOrder.CellMouseLeave += new DataGridViewCellEventHandler(this.dgAllOrder_CellMouseLeave);
			this.dgAllOrder.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgAllOrder_ColumnHeaderMouseClick);
			this.dgAllOrder.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgAllOrder_DataBindingComplete);
			componentResourceManager.ApplyResources(this.SelectFlagF2, "SelectFlagF2");
			this.SelectFlagF2.Name = "SelectFlagF2";
			this.comboB_SF3.FormattingEnabled = true;
			this.helpProvider.SetHelpKeyword(this.comboB_SF3, componentResourceManager.GetString("comboB_SF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboB_SF3, (HelpNavigator)componentResourceManager.GetObject("comboB_SF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboB_SF3, componentResourceManager.GetString("comboB_SF3.HelpString"));
			componentResourceManager.ApplyResources(this.comboB_SF3, "comboB_SF3");
			this.comboB_SF3.Name = "comboB_SF3";
			this.helpProvider.SetShowHelp(this.comboB_SF3, (bool)componentResourceManager.GetObject("comboB_SF3.ShowHelp"));
			this.comboB_SF3.SelectedIndexChanged += new EventHandler(this.comboCommodityF2_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelB_SF3, "labelB_SF3");
			this.labelB_SF3.Name = "labelB_SF3";
			this.helpProvider.SetShowHelp(this.labelB_SF3, (bool)componentResourceManager.GetObject("labelB_SF3.ShowHelp"));
			this.helpProvider.SetHelpKeyword(this.comboTrancF2, componentResourceManager.GetString("comboTrancF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboTrancF2, (HelpNavigator)componentResourceManager.GetObject("comboTrancF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboTrancF2, componentResourceManager.GetString("comboTrancF2.HelpString"));
			componentResourceManager.ApplyResources(this.comboTrancF2, "comboTrancF2");
			this.comboTrancF2.Name = "comboTrancF2";
			this.helpProvider.SetShowHelp(this.comboTrancF2, (bool)componentResourceManager.GetObject("comboTrancF2.ShowHelp"));
			this.comboTrancF2.SelectedIndexChanged += new EventHandler(this.comboCommodityF2_SelectedIndexChanged);
			this.helpProvider.SetHelpKeyword(this.comboCommodityF2, componentResourceManager.GetString("comboCommodityF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboCommodityF2, (HelpNavigator)componentResourceManager.GetObject("comboCommodityF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboCommodityF2, componentResourceManager.GetString("comboCommodityF2.HelpString"));
			componentResourceManager.ApplyResources(this.comboCommodityF2, "comboCommodityF2");
			this.comboCommodityF2.Name = "comboCommodityF2";
			this.helpProvider.SetShowHelp(this.comboCommodityF2, (bool)componentResourceManager.GetObject("comboCommodityF2.ShowHelp"));
			this.comboCommodityF2.SelectedIndexChanged += new EventHandler(this.comboCommodityF2_SelectedIndexChanged);
			this.helpProvider.SetHelpKeyword(this.labelTrancF2, componentResourceManager.GetString("labelTrancF2.HelpKeyword"));
			this.helpProvider.SetHelpString(this.labelTrancF2, componentResourceManager.GetString("labelTrancF2.HelpString"));
			componentResourceManager.ApplyResources(this.labelTrancF2, "labelTrancF2");
			this.labelTrancF2.Name = "labelTrancF2";
			this.helpProvider.SetShowHelp(this.labelTrancF2, (bool)componentResourceManager.GetObject("labelTrancF2.ShowHelp"));
			this.helpProvider.SetHelpKeyword(this.labelCommodityF2, componentResourceManager.GetString("labelCommodityF2.HelpKeyword"));
			this.helpProvider.SetHelpString(this.labelCommodityF2, componentResourceManager.GetString("labelCommodityF2.HelpString"));
			componentResourceManager.ApplyResources(this.labelCommodityF2, "labelCommodityF2");
			this.labelCommodityF2.Name = "labelCommodityF2";
			this.helpProvider.SetShowHelp(this.labelCommodityF2, (bool)componentResourceManager.GetObject("labelCommodityF2.ShowHelp"));
			this.helpProvider.SetHelpKeyword(this.buttonCancelF2, componentResourceManager.GetString("buttonCancelF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonCancelF2, (HelpNavigator)componentResourceManager.GetObject("buttonCancelF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonCancelF2, componentResourceManager.GetString("buttonCancelF2.HelpString"));
			componentResourceManager.ApplyResources(this.buttonCancelF2, "buttonCancelF2");
			this.buttonCancelF2.Name = "buttonCancelF2";
			this.helpProvider.SetShowHelp(this.buttonCancelF2, (bool)componentResourceManager.GetObject("buttonCancelF2.ShowHelp"));
			this.buttonCancelF2.UseVisualStyleBackColor = true;
			this.buttonCancelF2.Click += new EventHandler(this.buttonCancelF2_Click);
			this.helpProvider.SetHelpKeyword(this.buttonAllF2, componentResourceManager.GetString("buttonAllF2.HelpKeyword"));
			this.helpProvider.SetHelpString(this.buttonAllF2, componentResourceManager.GetString("buttonAllF2.HelpString"));
			componentResourceManager.ApplyResources(this.buttonAllF2, "buttonAllF2");
			this.buttonAllF2.Name = "buttonAllF2";
			this.helpProvider.SetShowHelp(this.buttonAllF2, (bool)componentResourceManager.GetObject("buttonAllF2.ShowHelp"));
			this.buttonAllF2.UseVisualStyleBackColor = true;
			this.buttonAllF2.Click += new EventHandler(this.buttonAllF2_Click);
			this.helpProvider.SetHelpKeyword(this.buttonSelF2, componentResourceManager.GetString("buttonSelF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonSelF2, (HelpNavigator)componentResourceManager.GetObject("buttonSelF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonSelF2, componentResourceManager.GetString("buttonSelF2.HelpString"));
			componentResourceManager.ApplyResources(this.buttonSelF2, "buttonSelF2");
			this.buttonSelF2.Name = "buttonSelF2";
			this.helpProvider.SetShowHelp(this.buttonSelF2, (bool)componentResourceManager.GetObject("buttonSelF2.ShowHelp"));
			this.buttonSelF2.UseVisualStyleBackColor = true;
			this.buttonSelF2.Click += new EventHandler(this.ToolStripMenuItem_Click);
			this.groupBoxF2_1.Controls.Add(this.radioCancelF2);
			this.groupBoxF2_1.Controls.Add(this.radioAllF2);
			this.helpProvider.SetHelpKeyword(this.groupBoxF2_1, componentResourceManager.GetString("groupBoxF2_1.HelpKeyword"));
			this.helpProvider.SetHelpString(this.groupBoxF2_1, componentResourceManager.GetString("groupBoxF2_1.HelpString"));
			componentResourceManager.ApplyResources(this.groupBoxF2_1, "groupBoxF2_1");
			this.groupBoxF2_1.Name = "groupBoxF2_1";
			this.helpProvider.SetShowHelp(this.groupBoxF2_1, (bool)componentResourceManager.GetObject("groupBoxF2_1.ShowHelp"));
			this.groupBoxF2_1.TabStop = false;
			componentResourceManager.ApplyResources(this.radioCancelF2, "radioCancelF2");
			this.helpProvider.SetHelpKeyword(this.radioCancelF2, componentResourceManager.GetString("radioCancelF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioCancelF2, (HelpNavigator)componentResourceManager.GetObject("radioCancelF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioCancelF2, componentResourceManager.GetString("radioCancelF2.HelpString"));
			this.radioCancelF2.Name = "radioCancelF2";
			this.helpProvider.SetShowHelp(this.radioCancelF2, (bool)componentResourceManager.GetObject("radioCancelF2.ShowHelp"));
			this.radioCancelF2.UseVisualStyleBackColor = true;
			this.radioCancelF2.CheckedChanged += new EventHandler(this.comboCommodityF2_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.radioAllF2, "radioAllF2");
			this.radioAllF2.Checked = true;
			this.helpProvider.SetHelpKeyword(this.radioAllF2, componentResourceManager.GetString("radioAllF2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioAllF2, (HelpNavigator)componentResourceManager.GetObject("radioAllF2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioAllF2, componentResourceManager.GetString("radioAllF2.HelpString"));
			this.radioAllF2.Name = "radioAllF2";
			this.helpProvider.SetShowHelp(this.radioAllF2, (bool)componentResourceManager.GetObject("radioAllF2.ShowHelp"));
			this.radioAllF2.TabStop = true;
			this.radioAllF2.UseVisualStyleBackColor = true;
			this.radioAllF2.CheckedChanged += new EventHandler(this.comboCommodityF2_SelectedIndexChanged);
			this.TabPageF4.Controls.Add(this.groupBoxF3);
			this.helpProvider.SetHelpKeyword(this.TabPageF4, componentResourceManager.GetString("TabPageF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF4, (HelpNavigator)componentResourceManager.GetObject("TabPageF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF4, componentResourceManager.GetString("TabPageF4.HelpString"));
			componentResourceManager.ApplyResources(this.TabPageF4, "TabPageF4");
			this.TabPageF4.Name = "TabPageF4";
			this.helpProvider.SetShowHelp(this.TabPageF4, (bool)componentResourceManager.GetObject("TabPageF4.ShowHelp"));
			this.TabPageF4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxF3, "groupBoxF3");
			this.groupBoxF3.Controls.Add(this.dgTradeSum);
			this.groupBoxF3.Controls.Add(this.comboB_S);
			this.groupBoxF3.Controls.Add(this.labelB_S);
			this.groupBoxF3.Controls.Add(this.dgTrade);
			this.groupBoxF3.Controls.Add(this.comboTrancF3);
			this.groupBoxF3.Controls.Add(this.comboCommodityF3);
			this.groupBoxF3.Controls.Add(this.labelTrancF3);
			this.groupBoxF3.Controls.Add(this.labelCommodityF3);
			this.groupBoxF3.Controls.Add(this.buttonSelF3);
			this.groupBoxF3.Controls.Add(this.groupBoxF3_1);
			this.helpProvider.SetHelpKeyword(this.groupBoxF3, componentResourceManager.GetString("groupBoxF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.groupBoxF3, (HelpNavigator)componentResourceManager.GetObject("groupBoxF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.groupBoxF3, componentResourceManager.GetString("groupBoxF3.HelpString"));
			this.groupBoxF3.Name = "groupBoxF3";
			this.helpProvider.SetShowHelp(this.groupBoxF3, (bool)componentResourceManager.GetObject("groupBoxF3.ShowHelp"));
			this.groupBoxF3.TabStop = false;
			this.dgTradeSum.AllowUserToAddRows = false;
			this.dgTradeSum.AllowUserToDeleteRows = false;
			this.dgTradeSum.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgTradeSum, "dgTradeSum");
			this.dgTradeSum.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgTradeSum.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgTradeSum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgTradeSum.ContextMenuStrip = this.MenuRefresh;
			this.helpProvider.SetHelpNavigator(this.dgTradeSum, (HelpNavigator)componentResourceManager.GetObject("dgTradeSum.HelpNavigator"));
			this.dgTradeSum.Name = "dgTradeSum";
			this.dgTradeSum.ReadOnly = true;
			this.dgTradeSum.RowHeadersVisible = false;
			this.dgTradeSum.RowTemplate.Height = 16;
			this.helpProvider.SetShowHelp(this.dgTradeSum, (bool)componentResourceManager.GetObject("dgTradeSum.ShowHelp"));
			this.dgTradeSum.TabStop = false;
			this.comboB_S.FormattingEnabled = true;
			this.helpProvider.SetHelpKeyword(this.comboB_S, componentResourceManager.GetString("comboB_S.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboB_S, (HelpNavigator)componentResourceManager.GetObject("comboB_S.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboB_S, componentResourceManager.GetString("comboB_S.HelpString"));
			componentResourceManager.ApplyResources(this.comboB_S, "comboB_S");
			this.comboB_S.Name = "comboB_S";
			this.helpProvider.SetShowHelp(this.comboB_S, (bool)componentResourceManager.GetObject("comboB_S.ShowHelp"));
			this.comboB_S.SelectedIndexChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelB_S, "labelB_S");
			this.labelB_S.Name = "labelB_S";
			this.helpProvider.SetShowHelp(this.labelB_S, (bool)componentResourceManager.GetObject("labelB_S.ShowHelp"));
			this.dgTrade.AllowUserToAddRows = false;
			this.dgTrade.AllowUserToDeleteRows = false;
			this.dgTrade.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgTrade, "dgTrade");
			this.dgTrade.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgTrade.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgTrade.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgTrade.ContextMenuStrip = this.MenuRefresh;
			this.helpProvider.SetHelpKeyword(this.dgTrade, componentResourceManager.GetString("dgTrade.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgTrade, (HelpNavigator)componentResourceManager.GetObject("dgTrade.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgTrade, componentResourceManager.GetString("dgTrade.HelpString"));
			this.dgTrade.Name = "dgTrade";
			this.dgTrade.ReadOnly = true;
			this.dgTrade.RowHeadersVisible = false;
			this.dgTrade.RowTemplate.Height = 16;
			this.helpProvider.SetShowHelp(this.dgTrade, (bool)componentResourceManager.GetObject("dgTrade.ShowHelp"));
			this.dgTrade.TabStop = false;
			this.dgTrade.CellClick += new DataGridViewCellEventHandler(this.dgTrade_CellClick);
			this.dgTrade.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgTrade_ColumnHeaderMouseClick);
			this.dgTrade.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgTrade_DataBindingComplete);
			this.helpProvider.SetHelpKeyword(this.comboTrancF3, componentResourceManager.GetString("comboTrancF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboTrancF3, (HelpNavigator)componentResourceManager.GetObject("comboTrancF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboTrancF3, componentResourceManager.GetString("comboTrancF3.HelpString"));
			componentResourceManager.ApplyResources(this.comboTrancF3, "comboTrancF3");
			this.comboTrancF3.Name = "comboTrancF3";
			this.helpProvider.SetShowHelp(this.comboTrancF3, (bool)componentResourceManager.GetObject("comboTrancF3.ShowHelp"));
			this.comboTrancF3.SelectedIndexChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			this.helpProvider.SetHelpKeyword(this.comboCommodityF3, componentResourceManager.GetString("comboCommodityF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboCommodityF3, (HelpNavigator)componentResourceManager.GetObject("comboCommodityF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboCommodityF3, componentResourceManager.GetString("comboCommodityF3.HelpString"));
			componentResourceManager.ApplyResources(this.comboCommodityF3, "comboCommodityF3");
			this.comboCommodityF3.Name = "comboCommodityF3";
			this.helpProvider.SetShowHelp(this.comboCommodityF3, (bool)componentResourceManager.GetObject("comboCommodityF3.ShowHelp"));
			this.comboCommodityF3.SelectedIndexChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelTrancF3, "labelTrancF3");
			this.labelTrancF3.Name = "labelTrancF3";
			this.helpProvider.SetShowHelp(this.labelTrancF3, (bool)componentResourceManager.GetObject("labelTrancF3.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelCommodityF3, "labelCommodityF3");
			this.labelCommodityF3.Name = "labelCommodityF3";
			this.helpProvider.SetShowHelp(this.labelCommodityF3, (bool)componentResourceManager.GetObject("labelCommodityF3.ShowHelp"));
			this.helpProvider.SetHelpKeyword(this.buttonSelF3, componentResourceManager.GetString("buttonSelF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonSelF3, (HelpNavigator)componentResourceManager.GetObject("buttonSelF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonSelF3, componentResourceManager.GetString("buttonSelF3.HelpString"));
			componentResourceManager.ApplyResources(this.buttonSelF3, "buttonSelF3");
			this.buttonSelF3.Name = "buttonSelF3";
			this.helpProvider.SetShowHelp(this.buttonSelF3, (bool)componentResourceManager.GetObject("buttonSelF3.ShowHelp"));
			this.buttonSelF3.UseVisualStyleBackColor = true;
			this.buttonSelF3.Click += new EventHandler(this.ToolStripMenuItem_Click);
			this.groupBoxF3_1.Controls.Add(this.radioTradeSum);
			this.groupBoxF3_1.Controls.Add(this.radioTradeDetail);
			this.groupBoxF3_1.Controls.Add(this.radioOF3);
			this.groupBoxF3_1.Controls.Add(this.radioLF3);
			this.groupBoxF3_1.Controls.Add(this.radioAllF3);
			componentResourceManager.ApplyResources(this.groupBoxF3_1, "groupBoxF3_1");
			this.groupBoxF3_1.Name = "groupBoxF3_1";
			this.helpProvider.SetShowHelp(this.groupBoxF3_1, (bool)componentResourceManager.GetObject("groupBoxF3_1.ShowHelp"));
			this.groupBoxF3_1.TabStop = false;
			componentResourceManager.ApplyResources(this.radioTradeSum, "radioTradeSum");
			this.radioTradeSum.Name = "radioTradeSum";
			this.radioTradeSum.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioTradeDetail, "radioTradeDetail");
			this.radioTradeDetail.Checked = true;
			this.radioTradeDetail.Name = "radioTradeDetail";
			this.radioTradeDetail.TabStop = true;
			this.radioTradeDetail.UseVisualStyleBackColor = true;
			this.radioTradeDetail.CheckedChanged += new EventHandler(this.radioTradeDetail_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioOF3, "radioOF3");
			this.helpProvider.SetHelpKeyword(this.radioOF3, componentResourceManager.GetString("radioOF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioOF3, (HelpNavigator)componentResourceManager.GetObject("radioOF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioOF3, componentResourceManager.GetString("radioOF3.HelpString"));
			this.radioOF3.Name = "radioOF3";
			this.helpProvider.SetShowHelp(this.radioOF3, (bool)componentResourceManager.GetObject("radioOF3.ShowHelp"));
			this.radioOF3.UseVisualStyleBackColor = true;
			this.radioOF3.CheckedChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.radioLF3, "radioLF3");
			this.helpProvider.SetHelpKeyword(this.radioLF3, componentResourceManager.GetString("radioLF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioLF3, (HelpNavigator)componentResourceManager.GetObject("radioLF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioLF3, componentResourceManager.GetString("radioLF3.HelpString"));
			this.radioLF3.Name = "radioLF3";
			this.helpProvider.SetShowHelp(this.radioLF3, (bool)componentResourceManager.GetObject("radioLF3.ShowHelp"));
			this.radioLF3.UseVisualStyleBackColor = true;
			this.radioLF3.CheckedChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.radioAllF3, "radioAllF3");
			this.helpProvider.SetHelpKeyword(this.radioAllF3, componentResourceManager.GetString("radioAllF3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioAllF3, (HelpNavigator)componentResourceManager.GetObject("radioAllF3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioAllF3, componentResourceManager.GetString("radioAllF3.HelpString"));
			this.radioAllF3.Name = "radioAllF3";
			this.helpProvider.SetShowHelp(this.radioAllF3, (bool)componentResourceManager.GetObject("radioAllF3.ShowHelp"));
			this.radioAllF3.UseVisualStyleBackColor = true;
			this.radioAllF3.CheckedChanged += new EventHandler(this.comboCommodityF3_SelectedIndexChanged);
			this.TabPageF5.Controls.Add(this.groupBoxF4);
			this.helpProvider.SetHelpKeyword(this.TabPageF5, componentResourceManager.GetString("TabPageF5.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF5, (HelpNavigator)componentResourceManager.GetObject("TabPageF5.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF5, componentResourceManager.GetString("TabPageF5.HelpString"));
			componentResourceManager.ApplyResources(this.TabPageF5, "TabPageF5");
			this.TabPageF5.Name = "TabPageF5";
			this.helpProvider.SetShowHelp(this.TabPageF5, (bool)componentResourceManager.GetObject("TabPageF5.ShowHelp"));
			this.TabPageF5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxF4, "groupBoxF4");
			this.groupBoxF4.Controls.Add(this.dgHoldingDetail);
			this.groupBoxF4.Controls.Add(this.groupBoxF4_1);
			this.groupBoxF4.Controls.Add(this.labelB_SF5);
			this.groupBoxF4.Controls.Add(this.comboB_SF5);
			this.groupBoxF4.Controls.Add(this.dgHoldingCollect);
			this.groupBoxF4.Controls.Add(this.comboTrancF4);
			this.groupBoxF4.Controls.Add(this.comboCommodityF4);
			this.groupBoxF4.Controls.Add(this.labelTrancF4);
			this.groupBoxF4.Controls.Add(this.labelCommodityF4);
			this.groupBoxF4.Controls.Add(this.buttonSelF4);
			this.helpProvider.SetHelpKeyword(this.groupBoxF4, componentResourceManager.GetString("groupBoxF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.groupBoxF4, (HelpNavigator)componentResourceManager.GetObject("groupBoxF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.groupBoxF4, componentResourceManager.GetString("groupBoxF4.HelpString"));
			this.groupBoxF4.Name = "groupBoxF4";
			this.helpProvider.SetShowHelp(this.groupBoxF4, (bool)componentResourceManager.GetObject("groupBoxF4.ShowHelp"));
			this.groupBoxF4.TabStop = false;
			this.dgHoldingDetail.AllowUserToAddRows = false;
			this.dgHoldingDetail.AllowUserToDeleteRows = false;
			this.dgHoldingDetail.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgHoldingDetail, "dgHoldingDetail");
			this.dgHoldingDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgHoldingDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgHoldingDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgHoldingDetail.ContextMenuStrip = this.MenuRefresh;
			this.helpProvider.SetHelpKeyword(this.dgHoldingDetail, componentResourceManager.GetString("dgHoldingDetail.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgHoldingDetail, (HelpNavigator)componentResourceManager.GetObject("dgHoldingDetail.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgHoldingDetail, componentResourceManager.GetString("dgHoldingDetail.HelpString"));
			this.dgHoldingDetail.Name = "dgHoldingDetail";
			this.dgHoldingDetail.ReadOnly = true;
			this.dgHoldingDetail.RowHeadersVisible = false;
			this.dgHoldingDetail.RowTemplate.Height = 23;
			this.helpProvider.SetShowHelp(this.dgHoldingDetail, (bool)componentResourceManager.GetObject("dgHoldingDetail.ShowHelp"));
			this.dgHoldingDetail.TabStop = false;
			this.dgHoldingDetail.CellClick += new DataGridViewCellEventHandler(this.dgHoldingDetail_CellClick);
			this.dgHoldingDetail.CellDoubleClick += new DataGridViewCellEventHandler(this.dgHoldingDetail_CellDoubleClick);
			this.dgHoldingDetail.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgHoldingDetail_CellFormatting);
			this.dgHoldingDetail.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgHoldingDetail_ColumnHeaderMouseClick);
			this.dgHoldingDetail.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgHoldingDetail_DataBindingComplete);
			this.groupBoxF4_1.Controls.Add(this.radioHdCollect);
			this.groupBoxF4_1.Controls.Add(this.radioHdDetail);
			componentResourceManager.ApplyResources(this.groupBoxF4_1, "groupBoxF4_1");
			this.groupBoxF4_1.Name = "groupBoxF4_1";
			componentResourceManager.ApplyResources(this.radioHdCollect, "radioHdCollect");
			this.radioHdCollect.Checked = true;
			this.radioHdCollect.Name = "radioHdCollect";
			this.radioHdCollect.TabStop = true;
			this.radioHdCollect.UseVisualStyleBackColor = true;
			this.radioHdCollect.CheckedChanged += new EventHandler(this.DtatViewIsVisible_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.radioHdDetail, "radioHdDetail");
			this.radioHdDetail.Name = "radioHdDetail";
			this.radioHdDetail.UseVisualStyleBackColor = true;
			this.radioHdDetail.CheckedChanged += new EventHandler(this.DtatViewIsVisible_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelB_SF5, "labelB_SF5");
			this.labelB_SF5.Name = "labelB_SF5";
			this.comboB_SF5.FormattingEnabled = true;
			this.helpProvider.SetHelpKeyword(this.comboB_SF5, componentResourceManager.GetString("comboB_SF5.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboB_SF5, (HelpNavigator)componentResourceManager.GetObject("comboB_SF5.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboB_SF5, componentResourceManager.GetString("comboB_SF5.HelpString"));
			componentResourceManager.ApplyResources(this.comboB_SF5, "comboB_SF5");
			this.comboB_SF5.Name = "comboB_SF5";
			this.helpProvider.SetShowHelp(this.comboB_SF5, (bool)componentResourceManager.GetObject("comboB_SF5.ShowHelp"));
			this.comboB_SF5.SelectedIndexChanged += new EventHandler(this.comboCommodityF4_SelectedIndexChanged);
			this.dgHoldingCollect.AllowUserToAddRows = false;
			this.dgHoldingCollect.AllowUserToDeleteRows = false;
			this.dgHoldingCollect.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgHoldingCollect, "dgHoldingCollect");
			this.dgHoldingCollect.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgHoldingCollect.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgHoldingCollect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgHoldingCollect.ContextMenuStrip = this.MenuRefresh;
			this.helpProvider.SetHelpKeyword(this.dgHoldingCollect, componentResourceManager.GetString("dgHoldingCollect.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgHoldingCollect, (HelpNavigator)componentResourceManager.GetObject("dgHoldingCollect.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgHoldingCollect, componentResourceManager.GetString("dgHoldingCollect.HelpString"));
			this.dgHoldingCollect.Name = "dgHoldingCollect";
			this.dgHoldingCollect.ReadOnly = true;
			this.dgHoldingCollect.RowHeadersVisible = false;
			this.dgHoldingCollect.RowTemplate.Height = 16;
			this.helpProvider.SetShowHelp(this.dgHoldingCollect, (bool)componentResourceManager.GetObject("dgHoldingCollect.ShowHelp"));
			this.dgHoldingCollect.TabStop = false;
			this.dgHoldingCollect.CellClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellClick);
			this.dgHoldingCollect.CellDoubleClick += new DataGridViewCellEventHandler(this.dgHoldingCollect_CellDoubleClick);
			this.dgHoldingCollect.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgHoldingCollect_CellFormatting);
			this.dgHoldingCollect.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgHoldingCollect_ColumnHeaderMouseClick);
			this.helpProvider.SetHelpKeyword(this.comboTrancF4, componentResourceManager.GetString("comboTrancF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboTrancF4, (HelpNavigator)componentResourceManager.GetObject("comboTrancF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboTrancF4, componentResourceManager.GetString("comboTrancF4.HelpString"));
			componentResourceManager.ApplyResources(this.comboTrancF4, "comboTrancF4");
			this.comboTrancF4.Name = "comboTrancF4";
			this.helpProvider.SetShowHelp(this.comboTrancF4, (bool)componentResourceManager.GetObject("comboTrancF4.ShowHelp"));
			this.comboTrancF4.SelectedIndexChanged += new EventHandler(this.comboCommodityF4_SelectedIndexChanged);
			this.helpProvider.SetHelpKeyword(this.comboCommodityF4, componentResourceManager.GetString("comboCommodityF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboCommodityF4, (HelpNavigator)componentResourceManager.GetObject("comboCommodityF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboCommodityF4, componentResourceManager.GetString("comboCommodityF4.HelpString"));
			componentResourceManager.ApplyResources(this.comboCommodityF4, "comboCommodityF4");
			this.comboCommodityF4.Name = "comboCommodityF4";
			this.helpProvider.SetShowHelp(this.comboCommodityF4, (bool)componentResourceManager.GetObject("comboCommodityF4.ShowHelp"));
			this.comboCommodityF4.SelectedIndexChanged += new EventHandler(this.comboCommodityF4_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelTrancF4, "labelTrancF4");
			this.labelTrancF4.Name = "labelTrancF4";
			this.helpProvider.SetShowHelp(this.labelTrancF4, (bool)componentResourceManager.GetObject("labelTrancF4.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelCommodityF4, "labelCommodityF4");
			this.labelCommodityF4.Name = "labelCommodityF4";
			this.helpProvider.SetShowHelp(this.labelCommodityF4, (bool)componentResourceManager.GetObject("labelCommodityF4.ShowHelp"));
			this.helpProvider.SetHelpKeyword(this.buttonSelF4, componentResourceManager.GetString("buttonSelF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonSelF4, (HelpNavigator)componentResourceManager.GetObject("buttonSelF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonSelF4, componentResourceManager.GetString("buttonSelF4.HelpString"));
			componentResourceManager.ApplyResources(this.buttonSelF4, "buttonSelF4");
			this.buttonSelF4.Name = "buttonSelF4";
			this.helpProvider.SetShowHelp(this.buttonSelF4, (bool)componentResourceManager.GetObject("buttonSelF4.ShowHelp"));
			this.buttonSelF4.UseVisualStyleBackColor = true;
			this.buttonSelF4.Click += new EventHandler(this.ToolStripMenuItem_Click);
			this.TabPageF6.Controls.Add(this.groupBoxMoney);
			this.helpProvider.SetHelpKeyword(this.TabPageF6, componentResourceManager.GetString("TabPageF6.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF6, (HelpNavigator)componentResourceManager.GetObject("TabPageF6.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF6, componentResourceManager.GetString("TabPageF6.HelpString"));
			componentResourceManager.ApplyResources(this.TabPageF6, "TabPageF6");
			this.TabPageF6.Name = "TabPageF6";
			this.helpProvider.SetShowHelp(this.TabPageF6, (bool)componentResourceManager.GetObject("TabPageF6.ShowHelp"));
			this.TabPageF6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxMoney, "groupBoxMoney");
			this.groupBoxMoney.Controls.Add(this.buttonFundsTransfer);
			this.groupBoxMoney.Controls.Add(this.buttonSelFundsF4);
			this.groupBoxMoney.Controls.Add(this.lstVFunds);
			this.groupBoxMoney.Name = "groupBoxMoney";
			this.helpProvider.SetShowHelp(this.groupBoxMoney, (bool)componentResourceManager.GetObject("groupBoxMoney.ShowHelp"));
			this.groupBoxMoney.TabStop = false;
			this.buttonFundsTransfer.BackColor = SystemColors.ControlDark;
			componentResourceManager.ApplyResources(this.buttonFundsTransfer, "buttonFundsTransfer");
			this.buttonFundsTransfer.ForeColor = Color.Maroon;
			this.buttonFundsTransfer.Name = "buttonFundsTransfer";
			this.buttonFundsTransfer.UseVisualStyleBackColor = false;
			this.buttonFundsTransfer.Click += new EventHandler(this.buttonFundsTransfer_Click);
			this.helpProvider.SetHelpKeyword(this.buttonSelFundsF4, componentResourceManager.GetString("buttonSelFundsF4.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonSelFundsF4, (HelpNavigator)componentResourceManager.GetObject("buttonSelFundsF4.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonSelFundsF4, componentResourceManager.GetString("buttonSelFundsF4.HelpString"));
			componentResourceManager.ApplyResources(this.buttonSelFundsF4, "buttonSelFundsF4");
			this.buttonSelFundsF4.Name = "buttonSelFundsF4";
			this.helpProvider.SetShowHelp(this.buttonSelFundsF4, (bool)componentResourceManager.GetObject("buttonSelFundsF4.ShowHelp"));
			this.buttonSelFundsF4.UseVisualStyleBackColor = true;
			this.buttonSelFundsF4.Click += new EventHandler(this.ToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.lstVFunds, "lstVFunds");
			this.lstVFunds.ContextMenuStrip = this.MenuRefresh;
			this.lstVFunds.FullRowSelect = true;
			this.lstVFunds.GridLines = true;
			this.lstVFunds.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.helpProvider.SetHelpKeyword(this.lstVFunds, componentResourceManager.GetString("lstVFunds.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.lstVFunds, (HelpNavigator)componentResourceManager.GetObject("lstVFunds.HelpNavigator"));
			this.helpProvider.SetHelpString(this.lstVFunds, componentResourceManager.GetString("lstVFunds.HelpString"));
			this.lstVFunds.Name = "lstVFunds";
			this.helpProvider.SetShowHelp(this.lstVFunds, (bool)componentResourceManager.GetObject("lstVFunds.ShowHelp"));
			this.lstVFunds.TabStop = false;
			this.lstVFunds.UseCompatibleStateImageBehavior = false;
			this.lstVFunds.View = View.Details;
			this.TabPageF8.Controls.Add(this.groupBoxF7);
			this.helpProvider.SetHelpKeyword(this.TabPageF8, componentResourceManager.GetString("TabPageF8.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.TabPageF8, (HelpNavigator)componentResourceManager.GetObject("TabPageF8.HelpNavigator"));
			this.helpProvider.SetHelpString(this.TabPageF8, componentResourceManager.GetString("TabPageF8.HelpString"));
			componentResourceManager.ApplyResources(this.TabPageF8, "TabPageF8");
			this.TabPageF8.Name = "TabPageF8";
			this.helpProvider.SetShowHelp(this.TabPageF8, (bool)componentResourceManager.GetObject("TabPageF8.ShowHelp"));
			this.TabPageF8.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxF7, "groupBoxF7");
			this.groupBoxF7.Controls.Add(this.dgPreDelegate);
			this.groupBoxF7.Controls.Add(this.buttonDel);
			this.groupBoxF7.Controls.Add(this.buttonOrder6);
			this.groupBoxF7.Controls.Add(this.selAll);
			this.groupBoxF7.Controls.Add(this.buttonSel);
			this.groupBoxF7.Controls.Add(this.comTranc);
			this.groupBoxF7.Controls.Add(this.comCommodity);
			this.groupBoxF7.Controls.Add(this.labelTrancF6);
			this.groupBoxF7.Controls.Add(this.labelCommodityF6);
			this.groupBoxF7.Name = "groupBoxF7";
			this.helpProvider.SetShowHelp(this.groupBoxF7, (bool)componentResourceManager.GetObject("groupBoxF7.ShowHelp"));
			this.groupBoxF7.TabStop = false;
			this.dgPreDelegate.AllowUserToAddRows = false;
			this.dgPreDelegate.AllowUserToDeleteRows = false;
			this.dgPreDelegate.AllowUserToResizeRows = false;
			componentResourceManager.ApplyResources(this.dgPreDelegate, "dgPreDelegate");
			this.dgPreDelegate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = SystemColors.Control;
			dataGridViewCellStyle10.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
			this.dgPreDelegate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.dgPreDelegate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgPreDelegate.Columns.AddRange(new DataGridViewColumn[]
			{
				this.SelectFlag
			});
			dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = SystemColors.Window;
			dataGridViewCellStyle11.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
			this.dgPreDelegate.DefaultCellStyle = dataGridViewCellStyle11;
			this.helpProvider.SetHelpKeyword(this.dgPreDelegate, componentResourceManager.GetString("dgPreDelegate.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.dgPreDelegate, (HelpNavigator)componentResourceManager.GetObject("dgPreDelegate.HelpNavigator"));
			this.helpProvider.SetHelpString(this.dgPreDelegate, componentResourceManager.GetString("dgPreDelegate.HelpString"));
			this.dgPreDelegate.Name = "dgPreDelegate";
			dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = SystemColors.Control;
			dataGridViewCellStyle12.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
			this.dgPreDelegate.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.dgPreDelegate.RowHeadersVisible = false;
			this.dgPreDelegate.RowTemplate.Height = 18;
			this.helpProvider.SetShowHelp(this.dgPreDelegate, (bool)componentResourceManager.GetObject("dgPreDelegate.ShowHelp"));
			this.dgPreDelegate.TabStop = false;
			this.dgPreDelegate.CellClick += new DataGridViewCellEventHandler(this.dgPreDelegate_CellClick);
			this.dgPreDelegate.CellDoubleClick += new DataGridViewCellEventHandler(this.dgPreDelegate_CellDoubleClick);
			this.dgPreDelegate.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgPreDelegate_ColumnHeaderMouseClick);
			this.dgPreDelegate.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgPreDelegate_DataBindingComplete);
			componentResourceManager.ApplyResources(this.SelectFlag, "SelectFlag");
			this.SelectFlag.Name = "SelectFlag";
			this.helpProvider.SetHelpKeyword(this.buttonDel, componentResourceManager.GetString("buttonDel.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonDel, (HelpNavigator)componentResourceManager.GetObject("buttonDel.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonDel, componentResourceManager.GetString("buttonDel.HelpString"));
			componentResourceManager.ApplyResources(this.buttonDel, "buttonDel");
			this.buttonDel.Name = "buttonDel";
			this.helpProvider.SetShowHelp(this.buttonDel, (bool)componentResourceManager.GetObject("buttonDel.ShowHelp"));
			this.buttonDel.Click += new EventHandler(this.buttonDel_Click);
			this.helpProvider.SetHelpKeyword(this.buttonOrder6, componentResourceManager.GetString("buttonOrder6.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonOrder6, (HelpNavigator)componentResourceManager.GetObject("buttonOrder6.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonOrder6, componentResourceManager.GetString("buttonOrder6.HelpString"));
			componentResourceManager.ApplyResources(this.buttonOrder6, "buttonOrder6");
			this.buttonOrder6.Name = "buttonOrder6";
			this.helpProvider.SetShowHelp(this.buttonOrder6, (bool)componentResourceManager.GetObject("buttonOrder6.ShowHelp"));
			this.buttonOrder6.Click += new EventHandler(this.buttonOrder6_Click);
			this.helpProvider.SetHelpKeyword(this.selAll, componentResourceManager.GetString("selAll.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.selAll, (HelpNavigator)componentResourceManager.GetObject("selAll.HelpNavigator"));
			this.helpProvider.SetHelpString(this.selAll, componentResourceManager.GetString("selAll.HelpString"));
			componentResourceManager.ApplyResources(this.selAll, "selAll");
			this.selAll.Name = "selAll";
			this.helpProvider.SetShowHelp(this.selAll, (bool)componentResourceManager.GetObject("selAll.ShowHelp"));
			this.selAll.Click += new EventHandler(this.selAll_Click);
			this.helpProvider.SetHelpKeyword(this.buttonSel, componentResourceManager.GetString("buttonSel.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonSel, (HelpNavigator)componentResourceManager.GetObject("buttonSel.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonSel, componentResourceManager.GetString("buttonSel.HelpString"));
			componentResourceManager.ApplyResources(this.buttonSel, "buttonSel");
			this.buttonSel.Name = "buttonSel";
			this.helpProvider.SetShowHelp(this.buttonSel, (bool)componentResourceManager.GetObject("buttonSel.ShowHelp"));
			this.buttonSel.Click += new EventHandler(this.buttonSel_Click);
			this.helpProvider.SetHelpKeyword(this.comTranc, componentResourceManager.GetString("comTranc.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comTranc, (HelpNavigator)componentResourceManager.GetObject("comTranc.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comTranc, componentResourceManager.GetString("comTranc.HelpString"));
			componentResourceManager.ApplyResources(this.comTranc, "comTranc");
			this.comTranc.Name = "comTranc";
			this.helpProvider.SetShowHelp(this.comTranc, (bool)componentResourceManager.GetObject("comTranc.ShowHelp"));
			this.comTranc.SelectedIndexChanged += new EventHandler(this.comTranc_SelectedIndexChanged);
			this.helpProvider.SetHelpKeyword(this.comCommodity, componentResourceManager.GetString("comCommodity.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comCommodity, (HelpNavigator)componentResourceManager.GetObject("comCommodity.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comCommodity, componentResourceManager.GetString("comCommodity.HelpString"));
			componentResourceManager.ApplyResources(this.comCommodity, "comCommodity");
			this.comCommodity.Name = "comCommodity";
			this.helpProvider.SetShowHelp(this.comCommodity, (bool)componentResourceManager.GetObject("comCommodity.ShowHelp"));
			this.comCommodity.SelectedIndexChanged += new EventHandler(this.comCommodity_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelTrancF6, "labelTrancF6");
			this.labelTrancF6.Name = "labelTrancF6";
			this.helpProvider.SetShowHelp(this.labelTrancF6, (bool)componentResourceManager.GetObject("labelTrancF6.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelCommodityF6, "labelCommodityF6");
			this.labelCommodityF6.Name = "labelCommodityF6";
			this.helpProvider.SetShowHelp(this.labelCommodityF6, (bool)componentResourceManager.GetObject("labelCommodityF6.ShowHelp"));
			this.TabPageF9.Controls.Add(this.groupBoxInvestor);
			componentResourceManager.ApplyResources(this.TabPageF9, "TabPageF9");
			this.TabPageF9.Name = "TabPageF9";
			this.TabPageF9.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBoxInvestor, "groupBoxInvestor");
			this.groupBoxInvestor.Controls.Add(this.listVInvestor);
			this.groupBoxInvestor.Name = "groupBoxInvestor";
			this.helpProvider.SetShowHelp(this.groupBoxInvestor, (bool)componentResourceManager.GetObject("groupBoxInvestor.ShowHelp"));
			this.groupBoxInvestor.TabStop = false;
			componentResourceManager.ApplyResources(this.listVInvestor, "listVInvestor");
			this.listVInvestor.ContextMenuStrip = this.MenuRefresh;
			this.listVInvestor.FullRowSelect = true;
			this.listVInvestor.GridLines = true;
			this.listVInvestor.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.helpProvider.SetHelpKeyword(this.listVInvestor, componentResourceManager.GetString("listVInvestor.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.listVInvestor, (HelpNavigator)componentResourceManager.GetObject("listVInvestor.HelpNavigator"));
			this.helpProvider.SetHelpString(this.listVInvestor, componentResourceManager.GetString("listVInvestor.HelpString"));
			this.listVInvestor.Name = "listVInvestor";
			this.helpProvider.SetShowHelp(this.listVInvestor, (bool)componentResourceManager.GetObject("listVInvestor.ShowHelp"));
			this.listVInvestor.TabStop = false;
			this.listVInvestor.UseCompatibleStateImageBehavior = false;
			this.listVInvestor.View = View.Details;
			this.groupBoxGNCommodit.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.groupBoxGNCommodit, "groupBoxGNCommodit");
			this.groupBoxGNCommodit.CausesValidation = false;
			this.groupBoxGNCommodit.Controls.Add(this.panel1);
			this.groupBoxGNCommodit.Controls.Add(this.butMinLine);
			this.groupBoxGNCommodit.Controls.Add(this.butKLine);
			this.groupBoxGNCommodit.Controls.Add(this.labelSpread);
			this.groupBoxGNCommodit.Controls.Add(this.labelGNInfo);
			this.groupBoxGNCommodit.Controls.Add(this.labelBP2);
			this.groupBoxGNCommodit.Controls.Add(this.labelSP2);
			this.groupBoxGNCommodit.Controls.Add(this.labelBV3);
			this.groupBoxGNCommodit.Controls.Add(this.labelSV1);
			this.groupBoxGNCommodit.Controls.Add(this.labelLimitDownV);
			this.groupBoxGNCommodit.Controls.Add(this.labelPrevClearV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLimitUpV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLimitUp);
			this.groupBoxGNCommodit.Controls.Add(this.labelBP3);
			this.groupBoxGNCommodit.Controls.Add(this.labelSP1);
			this.groupBoxGNCommodit.Controls.Add(this.labelB3);
			this.groupBoxGNCommodit.Controls.Add(this.labelS1);
			this.groupBoxGNCommodit.Controls.Add(this.labelBV1);
			this.groupBoxGNCommodit.Controls.Add(this.labelSV3);
			this.groupBoxGNCommodit.Controls.Add(this.labelLimitDown);
			this.groupBoxGNCommodit.Controls.Add(this.labelLast);
			this.groupBoxGNCommodit.Controls.Add(this.labelBP1);
			this.groupBoxGNCommodit.Controls.Add(this.labelSP3);
			this.groupBoxGNCommodit.Controls.Add(this.labelB1);
			this.groupBoxGNCommodit.Controls.Add(this.labelBV2);
			this.groupBoxGNCommodit.Controls.Add(this.labelS3);
			this.groupBoxGNCommodit.Controls.Add(this.labelCountV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLastP);
			this.groupBoxGNCommodit.Controls.Add(this.labelSV2);
			this.groupBoxGNCommodit.Controls.Add(this.labelB2);
			this.groupBoxGNCommodit.Controls.Add(this.labelCount);
			this.groupBoxGNCommodit.Controls.Add(this.labelS2);
			this.helpProvider.SetHelpKeyword(this.groupBoxGNCommodit, componentResourceManager.GetString("groupBoxGNCommodit.HelpKeyword"));
			this.helpProvider.SetHelpString(this.groupBoxGNCommodit, componentResourceManager.GetString("groupBoxGNCommodit.HelpString"));
			this.groupBoxGNCommodit.Name = "groupBoxGNCommodit";
			this.helpProvider.SetShowHelp(this.groupBoxGNCommodit, (bool)componentResourceManager.GetObject("groupBoxGNCommodit.ShowHelp"));
			this.groupBoxGNCommodit.TabStop = false;
			this.panel1.BackColor = Color.White;
			this.panel1.ForeColor = Color.White;
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.butMinLine.BackColor = Color.LightSteelBlue;
			componentResourceManager.ApplyResources(this.butMinLine, "butMinLine");
			this.butMinLine.Name = "butMinLine";
			this.butMinLine.TabStop = false;
			this.butMinLine.UseVisualStyleBackColor = false;
			this.butMinLine.Click += new EventHandler(this.butMinLine_Click);
			this.butKLine.BackColor = Color.LightSteelBlue;
			componentResourceManager.ApplyResources(this.butKLine, "butKLine");
			this.butKLine.Name = "butKLine";
			this.butKLine.TabStop = false;
			this.butKLine.UseVisualStyleBackColor = false;
			this.butKLine.Click += new EventHandler(this.butKLine_Click);
			componentResourceManager.ApplyResources(this.labelSpread, "labelSpread");
			this.labelSpread.ForeColor = Color.DodgerBlue;
			this.labelSpread.Name = "labelSpread";
			this.helpProvider.SetShowHelp(this.labelSpread, (bool)componentResourceManager.GetObject("labelSpread.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelGNInfo, "labelGNInfo");
			this.labelGNInfo.ForeColor = Color.Blue;
			this.labelGNInfo.Name = "labelGNInfo";
			this.helpProvider.SetShowHelp(this.labelGNInfo, (bool)componentResourceManager.GetObject("labelGNInfo.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelBP2, "labelBP2");
			this.labelBP2.Cursor = Cursors.Hand;
			this.labelBP2.ForeColor = SystemColors.ControlText;
			this.helpProvider.SetHelpKeyword(this.labelBP2, componentResourceManager.GetString("labelBP2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBP2, (HelpNavigator)componentResourceManager.GetObject("labelBP2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBP2, componentResourceManager.GetString("labelBP2.HelpString"));
			this.labelBP2.Name = "labelBP2";
			this.helpProvider.SetShowHelp(this.labelBP2, (bool)componentResourceManager.GetObject("labelBP2.ShowHelp"));
			this.toolTip.SetToolTip(this.labelBP2, componentResourceManager.GetString("labelBP2.ToolTip"));
			this.labelBP2.Click += new EventHandler(this.labelBP2_Click);
			componentResourceManager.ApplyResources(this.labelSP2, "labelSP2");
			this.labelSP2.Cursor = Cursors.Hand;
			this.helpProvider.SetHelpKeyword(this.labelSP2, componentResourceManager.GetString("labelSP2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSP2, (HelpNavigator)componentResourceManager.GetObject("labelSP2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSP2, componentResourceManager.GetString("labelSP2.HelpString"));
			this.labelSP2.Name = "labelSP2";
			this.helpProvider.SetShowHelp(this.labelSP2, (bool)componentResourceManager.GetObject("labelSP2.ShowHelp"));
			this.labelSP2.Click += new EventHandler(this.labelSP2_Click);
			componentResourceManager.ApplyResources(this.labelBV3, "labelBV3");
			this.helpProvider.SetHelpKeyword(this.labelBV3, componentResourceManager.GetString("labelBV3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBV3, (HelpNavigator)componentResourceManager.GetObject("labelBV3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBV3, componentResourceManager.GetString("labelBV3.HelpString"));
			this.labelBV3.Name = "labelBV3";
			this.helpProvider.SetShowHelp(this.labelBV3, (bool)componentResourceManager.GetObject("labelBV3.ShowHelp"));
			this.toolTip.SetToolTip(this.labelBV3, componentResourceManager.GetString("labelBV3.ToolTip"));
			this.labelBV3.Click += new EventHandler(this.labelOfferVolV_Click);
			componentResourceManager.ApplyResources(this.labelSV1, "labelSV1");
			this.helpProvider.SetHelpKeyword(this.labelSV1, componentResourceManager.GetString("labelSV1.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSV1, (HelpNavigator)componentResourceManager.GetObject("labelSV1.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSV1, componentResourceManager.GetString("labelSV1.HelpString"));
			this.labelSV1.Name = "labelSV1";
			this.helpProvider.SetShowHelp(this.labelSV1, (bool)componentResourceManager.GetObject("labelSV1.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelLimitDownV, "labelLimitDownV");
			this.labelLimitDownV.Cursor = Cursors.Hand;
			this.labelLimitDownV.Name = "labelLimitDownV";
			this.helpProvider.SetShowHelp(this.labelLimitDownV, (bool)componentResourceManager.GetObject("labelLimitDownV.ShowHelp"));
			this.labelLimitDownV.Click += new EventHandler(this.labelLimitDownV_Click);
			componentResourceManager.ApplyResources(this.labelPrevClearV, "labelPrevClearV");
			this.labelPrevClearV.Name = "labelPrevClearV";
			this.helpProvider.SetShowHelp(this.labelPrevClearV, (bool)componentResourceManager.GetObject("labelPrevClearV.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelLimitUpV, "labelLimitUpV");
			this.labelLimitUpV.Cursor = Cursors.Hand;
			this.labelLimitUpV.Name = "labelLimitUpV";
			this.helpProvider.SetShowHelp(this.labelLimitUpV, (bool)componentResourceManager.GetObject("labelLimitUpV.ShowHelp"));
			this.labelLimitUpV.Click += new EventHandler(this.labelLimitUpV_Click);
			componentResourceManager.ApplyResources(this.labelLimitUp, "labelLimitUp");
			this.labelLimitUp.Name = "labelLimitUp";
			this.helpProvider.SetShowHelp(this.labelLimitUp, (bool)componentResourceManager.GetObject("labelLimitUp.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelBP3, "labelBP3");
			this.labelBP3.Cursor = Cursors.Hand;
			this.helpProvider.SetHelpKeyword(this.labelBP3, componentResourceManager.GetString("labelBP3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBP3, (HelpNavigator)componentResourceManager.GetObject("labelBP3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBP3, componentResourceManager.GetString("labelBP3.HelpString"));
			this.labelBP3.Name = "labelBP3";
			this.helpProvider.SetShowHelp(this.labelBP3, (bool)componentResourceManager.GetObject("labelBP3.ShowHelp"));
			this.toolTip.SetToolTip(this.labelBP3, componentResourceManager.GetString("labelBP3.ToolTip"));
			this.labelBP3.Click += new EventHandler(this.labelBP3_Click);
			componentResourceManager.ApplyResources(this.labelSP1, "labelSP1");
			this.labelSP1.Cursor = Cursors.Hand;
			this.helpProvider.SetHelpKeyword(this.labelSP1, componentResourceManager.GetString("labelSP1.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSP1, (HelpNavigator)componentResourceManager.GetObject("labelSP1.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSP1, componentResourceManager.GetString("labelSP1.HelpString"));
			this.labelSP1.Name = "labelSP1";
			this.helpProvider.SetShowHelp(this.labelSP1, (bool)componentResourceManager.GetObject("labelSP1.ShowHelp"));
			this.labelSP1.Click += new EventHandler(this.labelSP1_Click);
			componentResourceManager.ApplyResources(this.labelB3, "labelB3");
			this.labelB3.Name = "labelB3";
			this.helpProvider.SetShowHelp(this.labelB3, (bool)componentResourceManager.GetObject("labelB3.ShowHelp"));
			this.toolTip.SetToolTip(this.labelB3, componentResourceManager.GetString("labelB3.ToolTip"));
			componentResourceManager.ApplyResources(this.labelS1, "labelS1");
			this.labelS1.Name = "labelS1";
			this.helpProvider.SetShowHelp(this.labelS1, (bool)componentResourceManager.GetObject("labelS1.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelBV1, "labelBV1");
			this.helpProvider.SetHelpKeyword(this.labelBV1, componentResourceManager.GetString("labelBV1.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBV1, (HelpNavigator)componentResourceManager.GetObject("labelBV1.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBV1, componentResourceManager.GetString("labelBV1.HelpString"));
			this.labelBV1.Name = "labelBV1";
			this.helpProvider.SetShowHelp(this.labelBV1, (bool)componentResourceManager.GetObject("labelBV1.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelSV3, "labelSV3");
			this.helpProvider.SetHelpKeyword(this.labelSV3, componentResourceManager.GetString("labelSV3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSV3, (HelpNavigator)componentResourceManager.GetObject("labelSV3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSV3, componentResourceManager.GetString("labelSV3.HelpString"));
			this.labelSV3.Name = "labelSV3";
			this.helpProvider.SetShowHelp(this.labelSV3, (bool)componentResourceManager.GetObject("labelSV3.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelLimitDown, "labelLimitDown");
			this.labelLimitDown.Name = "labelLimitDown";
			this.helpProvider.SetShowHelp(this.labelLimitDown, (bool)componentResourceManager.GetObject("labelLimitDown.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelLast, "labelLast");
			this.labelLast.Name = "labelLast";
			this.helpProvider.SetShowHelp(this.labelLast, (bool)componentResourceManager.GetObject("labelLast.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelBP1, "labelBP1");
			this.labelBP1.Cursor = Cursors.Hand;
			this.helpProvider.SetHelpKeyword(this.labelBP1, componentResourceManager.GetString("labelBP1.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBP1, (HelpNavigator)componentResourceManager.GetObject("labelBP1.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBP1, componentResourceManager.GetString("labelBP1.HelpString"));
			this.labelBP1.Name = "labelBP1";
			this.helpProvider.SetShowHelp(this.labelBP1, (bool)componentResourceManager.GetObject("labelBP1.ShowHelp"));
			this.labelBP1.Click += new EventHandler(this.labelBP1_Click);
			componentResourceManager.ApplyResources(this.labelSP3, "labelSP3");
			this.labelSP3.Cursor = Cursors.Hand;
			this.helpProvider.SetHelpKeyword(this.labelSP3, componentResourceManager.GetString("labelSP3.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSP3, (HelpNavigator)componentResourceManager.GetObject("labelSP3.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSP3, componentResourceManager.GetString("labelSP3.HelpString"));
			this.labelSP3.Name = "labelSP3";
			this.helpProvider.SetShowHelp(this.labelSP3, (bool)componentResourceManager.GetObject("labelSP3.ShowHelp"));
			this.toolTip.SetToolTip(this.labelSP3, componentResourceManager.GetString("labelSP3.ToolTip"));
			this.labelSP3.Click += new EventHandler(this.labelSP3_Click);
			componentResourceManager.ApplyResources(this.labelB1, "labelB1");
			this.labelB1.Name = "labelB1";
			this.helpProvider.SetShowHelp(this.labelB1, (bool)componentResourceManager.GetObject("labelB1.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelBV2, "labelBV2");
			this.helpProvider.SetHelpKeyword(this.labelBV2, componentResourceManager.GetString("labelBV2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelBV2, (HelpNavigator)componentResourceManager.GetObject("labelBV2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelBV2, componentResourceManager.GetString("labelBV2.HelpString"));
			this.labelBV2.Name = "labelBV2";
			this.helpProvider.SetShowHelp(this.labelBV2, (bool)componentResourceManager.GetObject("labelBV2.ShowHelp"));
			this.toolTip.SetToolTip(this.labelBV2, componentResourceManager.GetString("labelBV2.ToolTip"));
			this.labelBV2.Click += new EventHandler(this.labelBidVolV_Click);
			componentResourceManager.ApplyResources(this.labelS3, "labelS3");
			this.labelS3.Name = "labelS3";
			this.helpProvider.SetShowHelp(this.labelS3, (bool)componentResourceManager.GetObject("labelS3.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelCountV, "labelCountV");
			this.labelCountV.Name = "labelCountV";
			this.helpProvider.SetShowHelp(this.labelCountV, (bool)componentResourceManager.GetObject("labelCountV.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelLastP, "labelLastP");
			this.labelLastP.Name = "labelLastP";
			this.helpProvider.SetShowHelp(this.labelLastP, (bool)componentResourceManager.GetObject("labelLastP.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelSV2, "labelSV2");
			this.helpProvider.SetHelpKeyword(this.labelSV2, componentResourceManager.GetString("labelSV2.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.labelSV2, (HelpNavigator)componentResourceManager.GetObject("labelSV2.HelpNavigator"));
			this.helpProvider.SetHelpString(this.labelSV2, componentResourceManager.GetString("labelSV2.HelpString"));
			this.labelSV2.Name = "labelSV2";
			this.helpProvider.SetShowHelp(this.labelSV2, (bool)componentResourceManager.GetObject("labelSV2.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelB2, "labelB2");
			this.labelB2.Name = "labelB2";
			this.helpProvider.SetShowHelp(this.labelB2, (bool)componentResourceManager.GetObject("labelB2.ShowHelp"));
			this.toolTip.SetToolTip(this.labelB2, componentResourceManager.GetString("labelB2.ToolTip"));
			componentResourceManager.ApplyResources(this.labelCount, "labelCount");
			this.labelCount.Name = "labelCount";
			this.helpProvider.SetShowHelp(this.labelCount, (bool)componentResourceManager.GetObject("labelCount.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelS2, "labelS2");
			this.labelS2.Name = "labelS2";
			this.helpProvider.SetShowHelp(this.labelS2, (bool)componentResourceManager.GetObject("labelS2.ShowHelp"));
			this.groupBoxOrder.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.groupBoxOrder, "groupBoxOrder");
			this.groupBoxOrder.Controls.Add(this.comboTranc);
			this.groupBoxOrder.Controls.Add(this.tbTranc);
			this.groupBoxOrder.Controls.Add(this.comboCommodity);
			this.groupBoxOrder.Controls.Add(this.labelLargestTN);
			this.groupBoxOrder.Controls.Add(this.buttonAddPre);
			this.groupBoxOrder.Controls.Add(this.buttonOrder);
			this.groupBoxOrder.Controls.Add(this.numericQty);
			this.groupBoxOrder.Controls.Add(this.numericPrice);
			this.groupBoxOrder.Controls.Add(this.labQty);
			this.groupBoxOrder.Controls.Add(this.labPrice);
			this.groupBoxOrder.Controls.Add(this.labComCode);
			this.groupBoxOrder.Controls.Add(this.groupBoxB_S);
			this.groupBoxOrder.Controls.Add(this.gbCloseMode);
			this.groupBoxOrder.Controls.Add(this.groupBoxO_L);
			this.groupBoxOrder.Controls.Add(this.numericLPrice);
			this.groupBoxOrder.Controls.Add(this.labelLPrice);
			this.groupBoxOrder.Controls.Add(this.comboMarKet);
			this.groupBoxOrder.Controls.Add(this.labTrancCode);
			this.groupBoxOrder.Controls.Add(this.labelMarKet);
			this.helpProvider.SetHelpKeyword(this.groupBoxOrder, componentResourceManager.GetString("groupBoxOrder.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.groupBoxOrder, (HelpNavigator)componentResourceManager.GetObject("groupBoxOrder.HelpNavigator"));
			this.helpProvider.SetHelpString(this.groupBoxOrder, componentResourceManager.GetString("groupBoxOrder.HelpString"));
			this.groupBoxOrder.Name = "groupBoxOrder";
			this.helpProvider.SetShowHelp(this.groupBoxOrder, (bool)componentResourceManager.GetObject("groupBoxOrder.ShowHelp"));
			this.groupBoxOrder.TabStop = false;
			this.helpProvider.SetHelpKeyword(this.comboTranc, componentResourceManager.GetString("comboTranc.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboTranc, (HelpNavigator)componentResourceManager.GetObject("comboTranc.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboTranc, componentResourceManager.GetString("comboTranc.HelpString"));
			componentResourceManager.ApplyResources(this.comboTranc, "comboTranc");
			this.comboTranc.Name = "comboTranc";
			this.helpProvider.SetShowHelp(this.comboTranc, (bool)componentResourceManager.GetObject("comboTranc.ShowHelp"));
			this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
			this.tbTranc.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbTranc, "tbTranc");
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.TabStop = false;
			this.helpProvider.SetHelpKeyword(this.comboCommodity, componentResourceManager.GetString("comboCommodity.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboCommodity, (HelpNavigator)componentResourceManager.GetObject("comboCommodity.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboCommodity, componentResourceManager.GetString("comboCommodity.HelpString"));
			componentResourceManager.ApplyResources(this.comboCommodity, "comboCommodity");
			this.comboCommodity.Name = "comboCommodity";
			this.helpProvider.SetShowHelp(this.comboCommodity, (bool)componentResourceManager.GetObject("comboCommodity.ShowHelp"));
			this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
			this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
			this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
			this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
			this.comboCommodity.KeyPress += new KeyPressEventHandler(this.comboCommodity_KeyPress);
			this.comboCommodity.Leave += new EventHandler(this.comboCommodity_Leave);
			componentResourceManager.ApplyResources(this.labelLargestTN, "labelLargestTN");
			this.labelLargestTN.Name = "labelLargestTN";
			this.helpProvider.SetShowHelp(this.labelLargestTN, (bool)componentResourceManager.GetObject("labelLargestTN.ShowHelp"));
			this.toolTip.SetToolTip(this.labelLargestTN, componentResourceManager.GetString("labelLargestTN.ToolTip"));
			this.labelLargestTN.Click += new EventHandler(this.labelLargestTN_Click);
			this.buttonAddPre.BackColor = Color.LightSteelBlue;
			componentResourceManager.ApplyResources(this.buttonAddPre, "buttonAddPre");
			this.helpProvider.SetHelpKeyword(this.buttonAddPre, componentResourceManager.GetString("buttonAddPre.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonAddPre, (HelpNavigator)componentResourceManager.GetObject("buttonAddPre.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonAddPre, componentResourceManager.GetString("buttonAddPre.HelpString"));
			this.buttonAddPre.Name = "buttonAddPre";
			this.helpProvider.SetShowHelp(this.buttonAddPre, (bool)componentResourceManager.GetObject("buttonAddPre.ShowHelp"));
			this.buttonAddPre.UseVisualStyleBackColor = false;
			this.buttonAddPre.Click += new EventHandler(this.buttonAddPre_Click);
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			componentResourceManager.ApplyResources(this.buttonOrder, "buttonOrder");
			this.helpProvider.SetHelpKeyword(this.buttonOrder, componentResourceManager.GetString("buttonOrder.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.buttonOrder, (HelpNavigator)componentResourceManager.GetObject("buttonOrder.HelpNavigator"));
			this.helpProvider.SetHelpString(this.buttonOrder, componentResourceManager.GetString("buttonOrder.HelpString"));
			this.buttonOrder.Name = "buttonOrder";
			this.helpProvider.SetShowHelp(this.buttonOrder, (bool)componentResourceManager.GetObject("buttonOrder.ShowHelp"));
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.helpProvider.SetHelpKeyword(this.numericQty, componentResourceManager.GetString("numericQty.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.numericQty, (HelpNavigator)componentResourceManager.GetObject("numericQty.HelpNavigator"));
			this.helpProvider.SetHelpString(this.numericQty, componentResourceManager.GetString("numericQty.HelpString"));
			componentResourceManager.ApplyResources(this.numericQty, "numericQty");
			NumericUpDown arg_6446_0 = this.numericQty;
			int[] array = new int[4];
			array[0] = 99999;
			arg_6446_0.Maximum = new decimal(array);
			this.numericQty.Name = "numericQty";
			this.helpProvider.SetShowHelp(this.numericQty, (bool)componentResourceManager.GetObject("numericQty.ShowHelp"));
			this.numericQty.Click += new EventHandler(this.numericQty_Click);
			this.numericQty.Enter += new EventHandler(this.numericQty_Enter);
			this.numericQty.KeyPress += new KeyPressEventHandler(this.numericQty_KeyPress);
			this.numericQty.KeyUp += new KeyEventHandler(this.numericQty_KeyUp);
			this.helpProvider.SetHelpKeyword(this.numericPrice, componentResourceManager.GetString("numericPrice.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.numericPrice, (HelpNavigator)componentResourceManager.GetObject("numericPrice.HelpNavigator"));
			this.helpProvider.SetHelpString(this.numericPrice, componentResourceManager.GetString("numericPrice.HelpString"));
			NumericUpDown arg_654C_0 = this.numericPrice;
			int[] array2 = new int[4];
			array2[0] = 10;
			arg_654C_0.Increment = new decimal(array2);
			componentResourceManager.ApplyResources(this.numericPrice, "numericPrice");
			NumericUpDown arg_6580_0 = this.numericPrice;
			int[] array3 = new int[4];
			array3[0] = 999999;
			arg_6580_0.Maximum = new decimal(array3);
			this.numericPrice.Name = "numericPrice";
			this.helpProvider.SetShowHelp(this.numericPrice, (bool)componentResourceManager.GetObject("numericPrice.ShowHelp"));
			this.numericPrice.ValueChanged += new EventHandler(this.numericPrice_ValueChanged);
			this.numericPrice.Click += new EventHandler(this.numericPrice_Click);
			this.numericPrice.Enter += new EventHandler(this.numericPrice_Enter);
			this.numericPrice.KeyPress += new KeyPressEventHandler(this.numericPrice_KeyPress);
			this.numericPrice.KeyUp += new KeyEventHandler(this.numericPrice_KeyUp);
			componentResourceManager.ApplyResources(this.labQty, "labQty");
			this.labQty.Name = "labQty";
			this.helpProvider.SetShowHelp(this.labQty, (bool)componentResourceManager.GetObject("labQty.ShowHelp"));
			componentResourceManager.ApplyResources(this.labPrice, "labPrice");
			this.labPrice.ForeColor = SystemColors.ControlText;
			this.labPrice.Name = "labPrice";
			this.helpProvider.SetShowHelp(this.labPrice, (bool)componentResourceManager.GetObject("labPrice.ShowHelp"));
			this.toolTip.SetToolTip(this.labPrice, componentResourceManager.GetString("labPrice.ToolTip"));
			this.labPrice.DoubleClick += new EventHandler(this.labPrice_DoubleClick);
			componentResourceManager.ApplyResources(this.labComCode, "labComCode");
			this.labComCode.Name = "labComCode";
			this.helpProvider.SetShowHelp(this.labComCode, (bool)componentResourceManager.GetObject("labComCode.ShowHelp"));
			this.groupBoxB_S.BackColor = SystemColors.Control;
			this.groupBoxB_S.Controls.Add(this.radioS);
			this.groupBoxB_S.Controls.Add(this.radioB);
			componentResourceManager.ApplyResources(this.groupBoxB_S, "groupBoxB_S");
			this.groupBoxB_S.Name = "groupBoxB_S";
			this.helpProvider.SetShowHelp(this.groupBoxB_S, (bool)componentResourceManager.GetObject("groupBoxB_S.ShowHelp"));
			this.groupBoxB_S.TabStop = false;
			this.groupBoxB_S.Enter += new EventHandler(this.groupBox3_Enter);
			this.groupBoxB_S.MouseHover += new EventHandler(this.groupBox3_MouseHover);
			componentResourceManager.ApplyResources(this.radioS, "radioS");
			this.radioS.ForeColor = Color.Green;
			this.helpProvider.SetHelpKeyword(this.radioS, componentResourceManager.GetString("radioS.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioS, (HelpNavigator)componentResourceManager.GetObject("radioS.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioS, componentResourceManager.GetString("radioS.HelpString"));
			this.radioS.Name = "radioS";
			this.helpProvider.SetShowHelp(this.radioS, (bool)componentResourceManager.GetObject("radioS.ShowHelp"));
			this.radioS.CheckedChanged += new EventHandler(this.radioS_CheckedChanged);
			this.radioS.Enter += new EventHandler(this.radioS_Enter);
			this.radioS.MouseHover += new EventHandler(this.radioS_MouseHover);
			componentResourceManager.ApplyResources(this.radioB, "radioB");
			this.radioB.Checked = true;
			this.radioB.ForeColor = Color.Red;
			this.helpProvider.SetHelpKeyword(this.radioB, componentResourceManager.GetString("radioB.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioB, (HelpNavigator)componentResourceManager.GetObject("radioB.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioB, componentResourceManager.GetString("radioB.HelpString"));
			this.radioB.Name = "radioB";
			this.helpProvider.SetShowHelp(this.radioB, (bool)componentResourceManager.GetObject("radioB.ShowHelp"));
			this.radioB.TabStop = true;
			this.radioB.CheckedChanged += new EventHandler(this.radioB_CheckedChanged);
			this.radioB.Enter += new EventHandler(this.radioB_Enter);
			this.radioB.MouseHover += new EventHandler(this.radioB_MouseHover);
			this.gbCloseMode.BackColor = SystemColors.Control;
			this.gbCloseMode.Controls.Add(this.rbCloseH);
			this.gbCloseMode.Controls.Add(this.rbCloseT);
			componentResourceManager.ApplyResources(this.gbCloseMode, "gbCloseMode");
			this.gbCloseMode.Name = "gbCloseMode";
			this.helpProvider.SetShowHelp(this.gbCloseMode, (bool)componentResourceManager.GetObject("gbCloseMode.ShowHelp"));
			this.gbCloseMode.TabStop = false;
			this.gbCloseMode.Enter += new EventHandler(this.gbCloseMode_Enter);
			this.gbCloseMode.MouseHover += new EventHandler(this.gbCloseMode_MouseHover);
			componentResourceManager.ApplyResources(this.rbCloseH, "rbCloseH");
			this.rbCloseH.Checked = true;
			this.helpProvider.SetHelpKeyword(this.rbCloseH, componentResourceManager.GetString("rbCloseH.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.rbCloseH, (HelpNavigator)componentResourceManager.GetObject("rbCloseH.HelpNavigator"));
			this.helpProvider.SetHelpString(this.rbCloseH, componentResourceManager.GetString("rbCloseH.HelpString"));
			this.rbCloseH.Name = "rbCloseH";
			this.helpProvider.SetShowHelp(this.rbCloseH, (bool)componentResourceManager.GetObject("rbCloseH.ShowHelp"));
			this.rbCloseH.TabStop = true;
			this.rbCloseH.Enter += new EventHandler(this.gbCloseMode_Enter);
			this.rbCloseH.MouseHover += new EventHandler(this.gbCloseMode_MouseHover);
			componentResourceManager.ApplyResources(this.rbCloseT, "rbCloseT");
			this.helpProvider.SetHelpKeyword(this.rbCloseT, componentResourceManager.GetString("rbCloseT.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.rbCloseT, (HelpNavigator)componentResourceManager.GetObject("rbCloseT.HelpNavigator"));
			this.helpProvider.SetHelpString(this.rbCloseT, componentResourceManager.GetString("rbCloseT.HelpString"));
			this.rbCloseT.Name = "rbCloseT";
			this.helpProvider.SetShowHelp(this.rbCloseT, (bool)componentResourceManager.GetObject("rbCloseT.ShowHelp"));
			this.rbCloseT.Enter += new EventHandler(this.gbCloseMode_Enter);
			this.rbCloseT.MouseHover += new EventHandler(this.gbCloseMode_MouseHover);
			this.groupBoxO_L.BackColor = SystemColors.Control;
			this.groupBoxO_L.Controls.Add(this.radioL);
			this.groupBoxO_L.Controls.Add(this.radioO);
			componentResourceManager.ApplyResources(this.groupBoxO_L, "groupBoxO_L");
			this.groupBoxO_L.Name = "groupBoxO_L";
			this.helpProvider.SetShowHelp(this.groupBoxO_L, (bool)componentResourceManager.GetObject("groupBoxO_L.ShowHelp"));
			this.groupBoxO_L.TabStop = false;
			this.groupBoxO_L.Enter += new EventHandler(this.groupBox4_Enter);
			this.groupBoxO_L.MouseHover += new EventHandler(this.groupBox4_MouseHover);
			componentResourceManager.ApplyResources(this.radioL, "radioL");
			this.helpProvider.SetHelpKeyword(this.radioL, componentResourceManager.GetString("radioL.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioL, (HelpNavigator)componentResourceManager.GetObject("radioL.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioL, componentResourceManager.GetString("radioL.HelpString"));
			this.radioL.Name = "radioL";
			this.helpProvider.SetShowHelp(this.radioL, (bool)componentResourceManager.GetObject("radioL.ShowHelp"));
			this.radioL.CheckedChanged += new EventHandler(this.radioL_CheckedChanged);
			this.radioL.MouseHover += new EventHandler(this.radioL_MouseHover);
			componentResourceManager.ApplyResources(this.radioO, "radioO");
			this.radioO.Checked = true;
			this.helpProvider.SetHelpKeyword(this.radioO, componentResourceManager.GetString("radioO.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.radioO, (HelpNavigator)componentResourceManager.GetObject("radioO.HelpNavigator"));
			this.helpProvider.SetHelpString(this.radioO, componentResourceManager.GetString("radioO.HelpString"));
			this.radioO.Name = "radioO";
			this.helpProvider.SetShowHelp(this.radioO, (bool)componentResourceManager.GetObject("radioO.ShowHelp"));
			this.radioO.TabStop = true;
			this.radioO.CheckedChanged += new EventHandler(this.radioO_CheckedChanged);
			this.radioO.Enter += new EventHandler(this.radioO_Enter);
			this.radioO.MouseHover += new EventHandler(this.radioO_MouseHover);
			componentResourceManager.ApplyResources(this.numericLPrice, "numericLPrice");
			NumericUpDown arg_6EEC_0 = this.numericLPrice;
			int[] array4 = new int[4];
			array4[0] = 999999;
			arg_6EEC_0.Maximum = new decimal(array4);
			this.numericLPrice.Name = "numericLPrice";
			this.helpProvider.SetShowHelp(this.numericLPrice, (bool)componentResourceManager.GetObject("numericLPrice.ShowHelp"));
			this.numericLPrice.MouseDown += new MouseEventHandler(this.numericLPrice_MouseDown);
			componentResourceManager.ApplyResources(this.labelLPrice, "labelLPrice");
			this.labelLPrice.Name = "labelLPrice";
			this.helpProvider.SetShowHelp(this.labelLPrice, (bool)componentResourceManager.GetObject("labelLPrice.ShowHelp"));
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.helpProvider.SetHelpKeyword(this.comboMarKet, componentResourceManager.GetString("comboMarKet.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.comboMarKet, (HelpNavigator)componentResourceManager.GetObject("comboMarKet.HelpNavigator"));
			this.helpProvider.SetHelpString(this.comboMarKet, componentResourceManager.GetString("comboMarKet.HelpString"));
			componentResourceManager.ApplyResources(this.comboMarKet, "comboMarKet");
			this.comboMarKet.Name = "comboMarKet";
			this.helpProvider.SetShowHelp(this.comboMarKet, (bool)componentResourceManager.GetObject("comboMarKet.ShowHelp"));
			this.comboMarKet.SelectedIndexChanged += new EventHandler(this.comboMarKet_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labTrancCode, "labTrancCode");
			this.labTrancCode.Name = "labTrancCode";
			this.helpProvider.SetShowHelp(this.labTrancCode, (bool)componentResourceManager.GetObject("labTrancCode.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelMarKet, "labelMarKet");
			this.labelMarKet.Name = "labelMarKet";
			this.helpProvider.SetShowHelp(this.labelMarKet, (bool)componentResourceManager.GetObject("labelMarKet.ShowHelp"));
			this.tradeTime.Enabled = true;
			this.tradeTime.Interval = 1000;
			this.tradeTime.Tick += new EventHandler(this.tradeTime_Tick);
			componentResourceManager.ApplyResources(this.comboBox7, "comboBox7");
			this.comboBox7.Name = "comboBox7";
			this.helpProvider.SetShowHelp(this.comboBox7, (bool)componentResourceManager.GetObject("comboBox7.ShowHelp"));
			this.dataGridViewTextBoxColumn46.Name = "dataGridViewTextBoxColumn46";
			this.dataGridViewTextBoxColumn47.Name = "dataGridViewTextBoxColumn47";
			this.Column9.Name = "Column9";
			this.Column10.Name = "Column10";
			this.statusInfo.Items.AddRange(new ToolStripItem[]
			{
				this.info,
				this.user,
				this.status,
				this.time
			});
			componentResourceManager.ApplyResources(this.statusInfo, "statusInfo");
			this.statusInfo.Name = "statusInfo";
			this.helpProvider.SetShowHelp(this.statusInfo, (bool)componentResourceManager.GetObject("statusInfo.ShowHelp"));
			this.statusInfo.SizeChanged += new EventHandler(this.statusInfo_SizeChanged);
			componentResourceManager.ApplyResources(this.info, "info");
			this.info.ForeColor = SystemColors.ControlText;
			this.info.Name = "info";
			componentResourceManager.ApplyResources(this.user, "user");
			this.user.Name = "user";
			componentResourceManager.ApplyResources(this.status, "status");
			this.status.BackColor = Color.Lime;
			this.status.DoubleClickEnabled = true;
			this.status.Name = "status";
			this.status.DoubleClick += new EventHandler(this.status_DoubleClick);
			componentResourceManager.ApplyResources(this.time, "time");
			this.time.Name = "time";
			this.MessageInfo.BackColor = Color.FromArgb(255, 255, 192);
			componentResourceManager.ApplyResources(this.MessageInfo, "MessageInfo");
			this.MessageInfo.Name = "MessageInfo";
			this.helpProvider.SetShowHelp(this.MessageInfo, (bool)componentResourceManager.GetObject("MessageInfo.ShowHelp"));
			this.timerLock.Tick += new EventHandler(this.timerLock_Tick);
			this.SplitterPanel.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.SplitterPanel, "SplitterPanel");
			this.SplitterPanel.BorderStyle = BorderStyle.FixedSingle;
			this.SplitterPanel.Name = "SplitterPanel";
			this.toolTip.SetToolTip(this.SplitterPanel, componentResourceManager.GetString("SplitterPanel.ToolTip"));
			this.SplitterPanel.Click += new EventHandler(this.lbSplitterHide_Click);
			componentResourceManager.ApplyResources(this.helpProvider, "helpProvider");
			this.buttonUnLock.BackColor = Color.Transparent;
			this.buttonUnLock.ForeColor = Color.Black;
			componentResourceManager.ApplyResources(this.buttonUnLock, "buttonUnLock");
			this.buttonUnLock.Name = "buttonUnLock";
			this.helpProvider.SetShowHelp(this.buttonUnLock, (bool)componentResourceManager.GetObject("buttonUnLock.ShowHelp"));
			this.buttonUnLock.UseVisualStyleBackColor = false;
			this.buttonUnLock.Click += new EventHandler(this.buttonUnlock_Click);
			componentResourceManager.ApplyResources(this.panelLock, "panelLock");
			this.panelLock.BorderStyle = BorderStyle.Fixed3D;
			this.panelLock.Controls.Add(this.labelPwdInfo);
			this.panelLock.Controls.Add(this.buttonUnLock);
			this.panelLock.Controls.Add(this.labelPwd);
			this.panelLock.Controls.Add(this.textBoxPwd);
			this.panelLock.Name = "panelLock";
			this.helpProvider.SetShowHelp(this.panelLock, (bool)componentResourceManager.GetObject("panelLock.ShowHelp"));
			componentResourceManager.ApplyResources(this.labelPwdInfo, "labelPwdInfo");
			this.labelPwdInfo.BackColor = Color.Transparent;
			this.labelPwdInfo.Name = "labelPwdInfo";
			this.helpProvider.SetShowHelp(this.labelPwdInfo, (bool)componentResourceManager.GetObject("labelPwdInfo.ShowHelp"));
			this.labelPwd.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.labelPwd, "labelPwd");
			this.labelPwd.Name = "labelPwd";
			this.helpProvider.SetShowHelp(this.labelPwd, (bool)componentResourceManager.GetObject("labelPwd.ShowHelp"));
			componentResourceManager.ApplyResources(this.textBoxPwd, "textBoxPwd");
			this.textBoxPwd.Name = "textBoxPwd";
			this.helpProvider.SetShowHelp(this.textBoxPwd, (bool)componentResourceManager.GetObject("textBoxPwd.ShowHelp"));
			this.timerSysTime.Tick += new EventHandler(this.timerSysTime_Tick);
			base.AcceptButton = this.buttonUnLock;
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.m_splitContainer);
			base.Controls.Add(this.statusInfo);
			base.Controls.Add(this.SplitterPanel);
			base.Controls.Add(this.panelLock);
			base.Controls.Add(this.MessageInfo);
			base.KeyPreview = true;
			base.Name = "MainForm";
			this.helpProvider.SetShowHelp(this, (bool)componentResourceManager.GetObject("$this.ShowHelp"));
			base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
			base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
			base.Load += new EventHandler(this.MainForm_Load);
			base.Shown += new EventHandler(this.MainForm_Shown);
			base.KeyPress += new KeyPressEventHandler(this.MainForm_KeyPress);
			base.KeyUp += new KeyEventHandler(this.MainForm_KeyUp);
			this.splitOrder.Panel1.ResumeLayout(false);
			this.splitOrder.Panel2.ResumeLayout(false);
			this.splitOrder.ResumeLayout(false);
			this.groupBoxUnTrade.ResumeLayout(false);
			((ISupportInitialize)this.dgUnTrade).EndInit();
			this.MenuRefresh.ResumeLayout(false);
			this.groupBoxTrade.ResumeLayout(false);
			((ISupportInitialize)this.dgTradeOrder).EndInit();
			this.m_splitContainer.Panel1.ResumeLayout(false);
			this.m_splitContainer.Panel1.PerformLayout();
			this.m_splitContainer.Panel2.ResumeLayout(false);
			this.m_splitContainer.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.TabPageF2.ResumeLayout(false);
			this.TabPageF3.ResumeLayout(false);
			this.groupBoxF2.ResumeLayout(false);
			this.groupBoxF2.PerformLayout();
			((ISupportInitialize)this.dgAllOrder).EndInit();
			this.groupBoxF2_1.ResumeLayout(false);
			this.groupBoxF2_1.PerformLayout();
			this.TabPageF4.ResumeLayout(false);
			this.groupBoxF3.ResumeLayout(false);
			this.groupBoxF3.PerformLayout();
			((ISupportInitialize)this.dgTradeSum).EndInit();
			((ISupportInitialize)this.dgTrade).EndInit();
			this.groupBoxF3_1.ResumeLayout(false);
			this.groupBoxF3_1.PerformLayout();
			this.TabPageF5.ResumeLayout(false);
			this.groupBoxF4.ResumeLayout(false);
			this.groupBoxF4.PerformLayout();
			((ISupportInitialize)this.dgHoldingDetail).EndInit();
			this.groupBoxF4_1.ResumeLayout(false);
			this.groupBoxF4_1.PerformLayout();
			((ISupportInitialize)this.dgHoldingCollect).EndInit();
			this.TabPageF6.ResumeLayout(false);
			this.TabPageF6.PerformLayout();
			this.groupBoxMoney.ResumeLayout(false);
			this.TabPageF8.ResumeLayout(false);
			this.groupBoxF7.ResumeLayout(false);
			((ISupportInitialize)this.dgPreDelegate).EndInit();
			this.TabPageF9.ResumeLayout(false);
			this.TabPageF9.PerformLayout();
			this.groupBoxInvestor.ResumeLayout(false);
			this.groupBoxGNCommodit.ResumeLayout(false);
			this.groupBoxGNCommodit.PerformLayout();
			this.groupBoxOrder.ResumeLayout(false);
			this.groupBoxOrder.PerformLayout();
			((ISupportInitialize)this.numericQty).EndInit();
			((ISupportInitialize)this.numericPrice).EndInit();
			this.groupBoxB_S.ResumeLayout(false);
			this.groupBoxB_S.PerformLayout();
			this.gbCloseMode.ResumeLayout(false);
			this.gbCloseMode.PerformLayout();
			this.groupBoxO_L.ResumeLayout(false);
			this.groupBoxO_L.PerformLayout();
			((ISupportInitialize)this.numericLPrice).EndInit();
			this.statusInfo.ResumeLayout(false);
			this.statusInfo.PerformLayout();
			this.panelLock.ResumeLayout(false);
			this.panelLock.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		private void QuerySysTime()
		{
			WaitCallback callBack = new WaitCallback(this.refreshTime);
			ThreadPool.QueueUserWorkItem(callBack, null);
		}
		private void refreshTime(object obj)
		{
			if (!this.refreshTimeFlag)
			{
				return;
			}
			this.refreshTimeFlag = false;
			DateTime serverTime = default(DateTime);
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = Global.UserID;
			SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
			string str = string.Empty;
			string str2 = string.Empty;
			if (sysTime.RetCode == 0L)
			{
				if (this.connectStatus != 0)
				{
					this.connectStatus = 0;
					WaitCallback waitCallback = new WaitCallback(this.SetStatus);
					this.HandleCreated();
					Delegate arg_7F_1 = waitCallback;
					object[] args = new object[1];
					base.Invoke(arg_7F_1, args);
				}
				if (!sysTime.CurrentDate.Equals("") && !sysTime.CurrentTime.Equals(""))
				{
					str = sysTime.CurrentDate;
					str2 = sysTime.CurrentTime;
					try
					{
						serverTime = DateTime.Parse(str + " " + str2);
					}
					catch
					{
						serverTime = DateTime.Now;
					}
					Global.ServerTime = serverTime;
				}
				MainForm.StringObjCallback method = new MainForm.StringObjCallback(this.displayBroadcast);
				this.HandleCreated();
				base.Invoke(method, new object[]
				{
					sysTime.BroadcastList
				});
				if (sysTime.NewTrade == 1)
				{
					string text = string.Empty;
					if (sysTime.TradeMessageList != null && sysTime.TradeMessageList.Count > 0)
					{
						for (int i = 0; i < sysTime.TradeMessageList.Count; i++)
						{
							if (sysTime.TradeMessageList[i].CommodityID == "")
							{
								text += string.Format("{0}号委托成交{2}\n", sysTime.TradeMessageList[i].OrderNO, sysTime.TradeMessageList[i].CommodityID, sysTime.TradeMessageList[i].TradeQuatity);
							}
							else
							{
								text += string.Format("{0}号委托({1})成交{2}\n", sysTime.TradeMessageList[i].OrderNO, sysTime.TradeMessageList[i].CommodityID, sysTime.TradeMessageList[i].TradeQuatity);
							}
						}
						this.refreshFlag = true;
					}
					MainForm.StringObjCallback method2 = new MainForm.StringObjCallback(this.displayTradeInfo);
					base.Invoke(method2, new object[]
					{
						text
					});
				}
			}
			else
			{
				Logger.wirte(3, "主窗体获取服务器系统时间错误：" + sysTime.RetMessage);
				if (this.connectStatus == 0)
				{
					if (sysTime.RetMessage != null && sysTime.RetMessage.Length > 0)
					{
						this.connectStatus = 1;
					}
					else
					{
						this.connectStatus = 2;
					}
					WaitCallback waitCallback2 = new WaitCallback(this.SetStatus);
					this.HandleCreated();
					Delegate arg_2B7_1 = waitCallback2;
					object[] args2 = new object[1];
					base.Invoke(arg_2B7_1, args2);
				}
			}
			this.refreshTimeFlag = true;
		}
		private void displayBroadcast(object obj)
		{
			List<TradeInterface.Gnnt.ISSUE.DataVO.Broadcast> list = (List<TradeInterface.Gnnt.ISSUE.DataVO.Broadcast>)obj;
			if (list != null && list.Count > 0)
			{
				this.broadcast.broadCastBuffer(list);
			}
		}
		private void displayTradeInfo(object obj)
		{
			string text = (string)obj;
			if (!text.Equals(""))
			{
				PlayWav.PlayWavResource("ring.wav", 0);
				if (IniData.GetInstance().SuccessShowDialog)
				{
					this.Notifier.Show("成交回报", text, 500, 2000, 500);
					return;
				}
				text = text.Replace("\n", "\0");
				this.FillInfoText(text, Global.RightColor, this.displayInfo);
			}
		}
		private void SetStatus(object o)
		{
			if (this.connectStatus == 2)
			{
				this.status.BackColor = Color.Red;
				this.status.Text = "断开";
				this.DelegateFlag = false;
				this.QueryOrderInfoFlag = false;
				this.QueryTradeInfoFlag = false;
				this.QueryHoldingInfoFlag = false;
				this.QueryFundsInfoFlag = false;
				this.MenuRefresh.Enabled = false;
				this.EnableControls(false, "设置连接状态中");
				return;
			}
			if (this.connectStatus == 1)
			{
				this.status.BackColor = Color.Red;
				this.status.Text = "断开";
				this.DelegateFlag = false;
				this.QueryOrderInfoFlag = false;
				this.QueryTradeInfoFlag = false;
				this.QueryHoldingInfoFlag = false;
				this.QueryFundsInfoFlag = false;
				this.MenuRefresh.Enabled = false;
				this.buttonOrder.Enabled = false;
				this.buttonOrder6.Enabled = false;
				this.toolStripButtonOrder.Enabled = false;
				this.toolStripButtonBill.Enabled = false;
				return;
			}
			if (this.connectStatus == 0)
			{
				this.status.BackColor = Color.Lime;
				this.status.Text = "连接";
				this.DelegateFlag = true;
				this.QueryOrderInfoFlag = true;
				this.QueryTradeInfoFlag = true;
				this.QueryHoldingInfoFlag = true;
				this.QueryFundsInfoFlag = true;
				this.MenuRefresh.Enabled = true;
				this.EnableControls(true, "设置连接状态中");
			}
		}
		public MainForm()
		{
			this.InitializeComponent();
			this.lbmain = new ListBox();
			this.lbmain.Click += new EventHandler(this.lbmain_click);
			this.lbmain.KeyDown += new KeyEventHandler(this.lbmain_keydown);
			this.lbmain.Visible = false;
			int commodity = this.dataProcess.GetCommodity();
			if (commodity == 1)
			{
				MessageBox.Show("对不起,您的交易权限有误！系统将退出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
				return;
			}
			if (commodity == 2)
			{
				MessageBox.Show("对不起,现在没有可交易商品，无法进行交易！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
				return;
			}
			this.dataProcess.GetFirmInfoList();
			MarketQueryRequestVO marketQueryRequestVO = new MarketQueryRequestVO();
			marketQueryRequestVO.UserID = Global.UserID;
			Global.MarketHT = this.dataProcess.QueryMarketInfo(marketQueryRequestVO);
			if (Global.MarketHT != null && Global.MarketHT.Count == 1)
			{
				foreach (DictionaryEntry dictionaryEntry in Global.MarketHT)
				{
					MarkeInfo markeInfo = (MarkeInfo)dictionaryEntry.Value;
					if (markeInfo != null)
					{
						this.marketID = markeInfo.MarketID;
					}
				}
			}
			this.QuerySysTime();
			this.BreedRep = this.dataProcess.QueryFirmbreed();
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.PreDelegateXml);
			xmlDataSet.WriteNewXml(Global.ConfigPath + Global.PreDelegateXml, Global.ConfigPath + Global.UserID + Global.PreDelegateXml);
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.InitControlLayout();
			this.SetFormSize();
			this.InitFieldInfo();
			this.XmlCommodity = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
			this.dsCommodity = this.XmlCommodity.GetDataSetByXml();
			this.XmlTransactions = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml);
			this.dsTransactions = this.XmlTransactions.GetDataSetByXml();
			if (Global.MarketHT.Count > 1)
			{
				this.ComboMarKetLoad();
			}
			this.ComboCommodityLoad();
			string commodityID = this.GetCommodityID(this.comboCommodity.Text);
			if (this.dataProcess.ht_TradeMode.Count != 0 && this.dataProcess.ht_TradeMode[commodityID].ToString() != "0")
			{
				this.setRadioEnable(commodityID);
			}
			this.ComboBuysellLoad();
			this.ComboTrancLoad();
			this.DelegateLoad();
			this.StartLock();
			this.LoadFlag = false;
			MainForm.XmlPreDelegate = new XmlDataSet(Global.ConfigPath + Global.UserID + Global.PreDelegateXml);
			MainForm.dsPreDelegate = MainForm.XmlPreDelegate.GetDataSetByXml();
			this.broadcast = new Broadcast();
			this.toolStripButtonAbout.Visible = InterFace.TopLevel;
			this.butMinLine.Visible = !InterFace.TopLevel;
			this.butKLine.Visible = !InterFace.TopLevel;
			if (MainForm.mainFormLoad != null)
			{
				MainForm.mainFormLoad(this, e);
			}
			ScaleForm.ScaleForms(this);
		}
		private void ComboBuysellLoad()
		{
			this.comboB_S.Items.Clear();
			this.comboB_S.Items.Add("全部");
			this.comboB_S.Items.Add("买入");
			this.comboB_S.Items.Add("卖出");
			this.comboB_S.SelectedIndex = 0;
		}
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			MessageForm messageForm = new MessageForm("退出信息提示", "确定要退出交易客户端吗？", 0);
			messageForm.Owner = this;
			messageForm.ShowDialog();
			messageForm.Dispose();
			if (!messageForm.isOK)
			{
				e.Cancel = true;
			}
		}
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.CloseMainForm();
		}
		public void CloseMainForm()
		{
			base.Dispose();
			this.broadcast.Dispose();
			this.broadcast.Close();
			if (this.fundsTransfer != null)
			{
				this.fundsTransfer.Dispose();
				this.fundsTransfer.Close();
				Global.HtForm.Remove("fundsTransfer");
			}
			if (this.formOrder != null)
			{
				this.formOrder.Dispose();
				this.formOrder.Close();
			}
			if (this.billOrder != null)
			{
				this.billOrder.Dispose();
				this.billOrder.Close();
			}
			if (this.conditionOrder != null)
			{
				this.conditionOrder.Dispose();
				this.conditionOrder.Close();
				Global.HTConfig.Remove("conditionOrder");
			}
			if (this.hook != null)
			{
				this.hook.Stop();
			}
		}
		private void SetControlText()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			if (!File.Exists(this.helpProvider.HelpNamespace))
			{
				this.toolStrip.Items.Remove(this.toolStripButtonHelp);
			}
			if (Global.HTConfig.ContainsKey("BillType"))
			{
				string text = (string)Global.HTConfig["BillType"];
				if (text == null || text.Length == 0 || text == "0")
				{
					this.toolStrip.Items.Remove(this.toolStripButtonBill);
				}
			}
			if (Global.HTConfig.ContainsKey("ConditionOrder"))
			{
				string text2 = (string)Global.HTConfig["ConditionOrder"];
				if (text2 == null || text2.ToLower() != "true")
				{
					this.butConditionOrder.Visible = false;
				}
			}
			this.buttonUnLock.BackgroundImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_UnlockButton");
			this.TabPageF2.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF2");
			this.TabPageF3.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF3");
			this.TabPageF4.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF4");
			this.TabPageF5.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF5");
			this.TabPageF6.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF6");
			this.TabPageF8.Text = Global.M_ResourceManager.GetString("TradeStr_TabPageF8");
			this.groupBoxUnTrade.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxUnTrade");
			this.groupBoxTrade.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxTrade");
			this.groupBoxF2.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF2");
			this.labelCommodityF2.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelTrancF2.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.radioAllF2.Text = Global.M_ResourceManager.GetString("TradeStr_RadioAllF2");
			this.radioCancelF2.Text = Global.M_ResourceManager.GetString("TradeStr_RadioCancelF2");
			this.buttonSelF2.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF2");
			this.buttonAllF2.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonAllF2");
			this.buttonCancelF2.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonCancelF2");
			this.groupBoxF3.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF3");
			this.labelCommodityF3.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelTrancF3.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labelB_S.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
			this.labelB_SF3.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
			this.radioAllF3.Text = Global.M_ResourceManager.GetString("TradeStr_RadioAllF3");
			this.radioOF3.Text = Global.M_ResourceManager.GetString("TradeStr_radioOF3");
			this.radioLF3.Text = Global.M_ResourceManager.GetString("TradeStr_RadioLF3");
			this.buttonSelF3.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF3");
			this.groupBoxF4.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF4");
			this.labelCommodityF4.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelTrancF4.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.buttonSelF4.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelF4");
			this.labelB_SF5.Text = Global.M_ResourceManager.GetString("TradeStr_LabelB_S");
			this.radioHdCollect.Text = Global.M_ResourceManager.GetString("TradeStr_RadioHdCollect");
			this.radioHdDetail.Text = Global.M_ResourceManager.GetString("TradeStr_RadioHdDetail");
			this.groupBoxMoney.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxMoney");
			this.buttonFundsTransfer.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonFundsTransfer");
			this.buttonSelFundsF4.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSelFundsF4");
			this.groupBoxF7.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxF7");
			this.labelCommodityF6.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelTrancF6.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.buttonSel.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonSel");
			this.selAll.Text = Global.M_ResourceManager.GetString("TradeStr_SelAll");
			this.buttonOrder6.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonOrder6");
			this.buttonDel.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonDel");
			this.groupBoxOrder.Text = Global.M_ResourceManager.GetString("TradeStr_GroupBoxOrder");
			this.labTrancCode.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.labelMarKet.Text = Global.M_ResourceManager.GetString("TradeStr_LabelMarKet");
			this.radioB.Text = Global.M_ResourceManager.GetString("TradeStr_RadioB");
			this.radioS.Text = Global.M_ResourceManager.GetString("TradeStr_RadioS");
			this.radioO.Text = Global.M_ResourceManager.GetString("TradeStr_RadioO");
			this.radioL.Text = Global.M_ResourceManager.GetString("TradeStr_RadioL");
			this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice");
			this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
			if (Global.HTConfig.ContainsKey("DisplaySwitch") && Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
			{
				if (IniData.GetInstance().AutoAddBSPQ)
				{
					this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice1");
				}
				else
				{
					this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice2");
				}
				this.labPrice.Font = new Font("宋体", 10.5f, FontStyle.Bold);
				this.labPrice.ForeColor = Color.DarkOrange;
			}
			this.buttonOrder.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonOrder");
			this.buttonAddPre.Text = Global.M_ResourceManager.GetString("TradeStr_ButtonAddPre");
			this.butKLine.Text = Global.M_ResourceManager.GetString("TradeStr_ButKLine");
			this.butMinLine.Text = Global.M_ResourceManager.GetString("TradeStr_ButMinLine");
			this.toolStripButtonOrder.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonOrder");
			this.toolStripButtonSet.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonSet");
			this.toolStripButtonMsg.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonMsg");
			this.toolStripButtonLock.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonLock");
			this.toolStripButtonHelp.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonHelp");
			this.toolStripButtonAbout.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonAbout");
			this.toolStripButtonExit.ToolTipText = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonExit");
			this.rbCloseT.Text = Global.TimeFlagStrArr[0];
			this.rbCloseH.Text = Global.TimeFlagStrArr[1];
			this.user.Text = Global.M_ResourceManager.GetString("TradeStr_user");
			this.radioTradeDetail.Checked = true;
			this.Text = (string)Global.HTConfig["Title"];
			ToolStripStatusLabel expr_869 = this.user;
			expr_869.Text += Global.UserID;
			this.tbTranc.Text = Global.FirmID;
			this.MessageInfo.Visible = false;
			this.MessageInfo.Size = new Size(200, 35);
			this.MessageInfo.Left = (base.Width - this.MessageInfo.Width - 300) / 2;
			this.MessageInfo.Top = (base.Height - this.MessageInfo.Height - 100) / 2;
			this.panelLock.Left = (base.Width - this.panelLock.Width) / 2;
			this.panelLock.Top = (base.Height - this.panelLock.Height) / 2;
			this.panelLock.Visible = false;
			this.comboTranc.Focus();
			this.labelSpread.Text = "";
			this.labelLargestTN.Text = "";
			if ((string)Global.HTConfig["AddressTransfer"] == "")
			{
				this.buttonFundsTransfer.Visible = false;
			}
			if (Global.CustomerCount < 2)
			{
				this.comboTranc.Visible = false;
				this.tbTranc.Size = new Size(this.comboCommodity.Width, 20);
				this.labelTrancF2.Visible = false;
				this.comboTrancF2.Visible = false;
				this.labelTrancF3.Visible = false;
				this.comboTrancF3.Visible = false;
				this.labelTrancF4.Visible = false;
				this.comboTrancF4.Visible = false;
				this.labelTrancF6.Visible = false;
				this.comTranc.Visible = false;
				this.labelB_SF3.Location = new Point(this.labelB_SF3.Location.X - 150, this.labelB_SF3.Location.Y);
				this.comboB_SF3.Location = new Point(this.comboB_SF3.Location.X - 150, this.comboB_SF3.Location.Y);
				this.labelB_SF5.Location = new Point(this.labelB_SF5.Location.X - 150, this.labelB_SF5.Location.Y);
				this.comboB_SF5.Location = new Point(this.comboB_SF5.Location.X - 150, this.comboB_SF5.Location.Y);
				this.groupBoxF2_1.Location = new Point(this.groupBoxF2_1.Location.X - 150, this.groupBoxF2_1.Location.Y);
				this.buttonSelF2.Location = new Point(this.buttonSelF2.Location.X - 150, this.buttonSelF2.Location.Y);
				this.buttonAllF2.Location = new Point(this.buttonAllF2.Location.X - 150, this.buttonAllF2.Location.Y);
				this.buttonCancelF2.Location = new Point(this.buttonCancelF2.Location.X - 150, this.buttonCancelF2.Location.Y);
				this.labelB_S.Location = new Point(this.labelB_S.Location.X - 169, this.labelB_S.Location.Y);
				this.comboB_S.Location = new Point(this.comboB_S.Location.X - 169, this.comboB_S.Location.Y);
				this.groupBoxF3_1.Location = new Point(this.groupBoxF3_1.Location.X - 169, this.groupBoxF3_1.Location.Y);
				this.buttonSelF3.Location = new Point(this.buttonSelF3.Location.X - 169, this.buttonSelF3.Location.Y);
			}
			Image image = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoPic");
			Image image2 = (Image)Global.M_ResourceManager.GetObject("TradeImg_InfoClose");
			if (image != null && image2 != null)
			{
				Bitmap image3 = new Bitmap(image);
				Bitmap image4 = new Bitmap(image2);
				this.Notifier.SetBackgroundBitmap(image3, Color.FromArgb(0, 0, 255));
				this.Notifier.SetCloseBitmap(image4, Color.FromArgb(0, 0, 255), new Point(155, 88));
				this.Notifier.TitleRectangle = new Rectangle(70, 100, 85, 22);
				this.Notifier.ContentRectangle = new Rectangle(20, 120, 200, 80);
				this.Notifier.AutoHide = true;
			}
			if (Global.HTConfig.ContainsKey("AutoScrollMinSize"))
			{
				string text3 = string.Empty;
				text3 = Global.HTConfig["AutoScrollMinSize"].ToString().Trim();
				int width = Tools.StrToInt(text3, 695);
				this.m_splitContainer.Panel1.AutoScrollMinSize = new Size(width, this.m_splitContainer.Panel1.AutoScrollMinSize.Height);
			}
			this.toolStrip.Items.Remove(this.toolStripButtonMsg);
		}
		private void InitControlLayout()
		{
			this.controlLayout.groupBoxOrder_Width = this.groupBoxOrder.Width;
			this.controlLayout.comboMarKet_Width = this.comboMarKet.Width;
			this.controlLayout.comboTranc_Width = this.comboTranc.Width;
			this.controlLayout.comboCommodity_Width = this.comboCommodity.Width;
			this.controlLayout.groupBoxB_S_Width = this.groupBoxB_S.Width;
			this.controlLayout.radioS_Left = this.radioS.Left;
			this.controlLayout.groupBoxO_L_Width = this.groupBoxO_L.Width;
			this.controlLayout.radioL_Left = this.radioL.Left;
			this.controlLayout.numericPrice_Width = this.numericPrice.Width;
			this.controlLayout.numericQty_Width = this.numericQty.Width;
			this.controlLayout.numericLPrice_Width = this.numericLPrice.Width;
			this.controlLayout.buttonAddPre_Left = this.buttonAddPre.Left;
			this.controlLayout.groupBoxCloseMode_Width = this.gbCloseMode.Width;
			this.controlLayout.radioCloseH_Left = this.rbCloseH.Left;
			this.controlLayout.groupBoxGNCommodit_Left = this.groupBoxGNCommodit.Left;
			this.controlLayout.groupBoxGNCommodit_Width = this.groupBoxGNCommodit.Width;
			this.controlLayout.butMinLine_Left = this.butMinLine.Left;
			ControlLayout.SkinImage = (Image)Global.M_ResourceManager.GetObject("TradeImg_Skin1");
			this.controlLayout.dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.controlLayout.dataGridViewCellStyle.BackColor = SystemColors.Window;
			this.controlLayout.dataGridViewCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.controlLayout.dataGridViewCellStyle.ForeColor = Color.Blue;
			this.controlLayout.dataGridViewCellStyle.SelectionBackColor = Color.LightSkyBlue;
			this.controlLayout.dataGridViewCellStyle.SelectionForeColor = SystemColors.ControlText;
			this.controlLayout.dataGridViewCellStyle.WrapMode = DataGridViewTriState.False;
			this.controlLayout.columnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			this.controlLayout.columnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;
			this.controlLayout.columnHeadersDefaultCellStyle.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.controlLayout.columnHeadersDefaultCellStyle.ForeColor = SystemColors.ActiveCaptionText;
			this.controlLayout.columnHeadersDefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
			this.controlLayout.columnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
			this.controlLayout.columnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
		}
		private void SetFormSize()
		{
			base.Size = new Size(Global.screenWidth, Global.screenHight / 3 + 46);
			base.Location = new Point(0, Global.screenHight - Global.screenHight / 3 - 46);
			this.groupBoxOrder.Height = base.Height;
			this.groupBoxGNCommodit.Height = base.Height;
		}
		private void frmRRegister_Resize()
		{
			if (Convert.ToDecimal(Global.screenWidth) / Convert.ToDecimal(800) != 1m)
			{
				if (Convert.ToDecimal(Global.screenWidth) / Convert.ToDecimal(1024) != 1m)
				{
					this.screenWidthFl = Convert.ToDecimal(Global.screenWidth) / Convert.ToDecimal(1024);
					this.tabMain.Width = Global.screenWidth - this.groupBoxOrder.Width - this.groupBoxGNCommodit.Width - 15;
					this.info.Width = this.statusInfo.Width - this.user.Width - this.status.Width - this.time.Width - 22;
				}
				return;
			}
			decimal d = Convert.ToDecimal(this.tabMain.Width) / Convert.ToDecimal(Global.screenWidth - this.groupBoxOrder.Width);
			this.tabMain.Width = Global.screenWidth - this.groupBoxOrder.Width - this.groupBoxGNCommodit.Width;
			this.info.Width = this.tabMain.Width - 5;
			this.groupBoxOrder.Location = new Point(this.tabMain.Width, 1);
			foreach (Control control in this.TabPageF6.Controls)
			{
				control.Left = Convert.ToInt32(control.Left * d);
				control.Width = Convert.ToInt32(control.Width * d);
			}
		}
		private void writeXsd()
		{
			this.XmlCommodity.WriteXmlSchema();
			this.XmlTransactions.WriteXmlSchema();
			MainForm.XmlPreDelegate.WriteXmlSchema();
		}
		private void EnableControls(bool enable, string messageInfo)
		{
			this.messageInfomation = messageInfo;
			this.MessageInfo.Text = messageInfo;
			this.MessageInfo.BringToFront();
			this.MessageInfo.Visible = !enable;
		}
		private void FillInfoText(string infoMessage, Color color, bool display)
		{
			if (display)
			{
				this.info.ForeColor = color;
				this.info.Text = "信息提示：" + infoMessage;
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.buttonClick)
			{
				this.buttonClick = false;
				return;
			}
			if (e.KeyCode == Keys.F2)
			{
				this.tabMain.SelectedIndex = 0;
			}
			else if (e.KeyCode == Keys.F3)
			{
				this.tabMain.SelectedIndex = 1;
			}
			else if (e.KeyCode == Keys.F4)
			{
				this.tabMain.SelectedIndex = 2;
			}
			else if (e.KeyCode == Keys.F5)
			{
				this.tabMain.SelectedIndex = 3;
			}
			else if (e.KeyCode == Keys.F6)
			{
				this.tabMain.SelectedIndex = 4;
			}
			else if (e.KeyCode == Keys.F7)
			{
				this.tabMain.SelectedIndex = 5;
			}
			else if (e.KeyCode == Keys.F8)
			{
				this.tabMain.SelectedIndex = 6;
			}
			else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
			{
				if (this.radioB.Focused || this.radioS.Focused)
				{
					this.radioB.Checked = true;
				}
				if (this.radioO.Focused || this.radioL.Focused)
				{
					this.radioO.Checked = true;
				}
				if (this.rbCloseT.Focused || this.rbCloseH.Focused)
				{
					this.rbCloseT.Checked = true;
				}
			}
			else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
			{
				if (this.radioB.Focused || this.radioS.Focused)
				{
					this.radioS.Checked = true;
				}
				if (this.radioO.Focused || this.radioL.Focused)
				{
					this.radioL.Checked = true;
				}
				if (this.rbCloseT.Focused || this.rbCloseH.Focused)
				{
					this.rbCloseH.Checked = true;
				}
			}
			else if (e.KeyValue == 13)
			{
				if (this.comboCommodity.Focused)
				{
					this.radioB.Focus();
				}
				else if (this.radioB.Focused || this.radioS.Focused)
				{
					this.numericPrice.Focus();
				}
				else if (this.numericPrice.Focused)
				{
					this.numericQty.Focus();
				}
				else if (this.numericQty.Focused)
				{
					this.buttonOrder.Focus();
				}
				else if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
				{
					this.comboCommodity.Focus();
				}
			}
			this.displayInfo = true;
		}
		private void SetFocus(Keys e)
		{
			if (e == Keys.Right)
			{
				if (this.radioB.Focused)
				{
					this.radioS.Focus();
					return;
				}
				if (this.buttonOrder.Focused)
				{
					this.buttonAddPre.Focus();
					return;
				}
			}
			else if (e == Keys.Left)
			{
				if (this.radioS.Focused)
				{
					this.radioB.Focus();
					return;
				}
				if (this.buttonAddPre.Focused)
				{
					this.buttonOrder.Focus();
					return;
				}
			}
			else if (e == Keys.Up)
			{
				if (this.comboCommodity.Focused)
				{
					this.buttonOrder.Focus();
					return;
				}
				if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
				{
					this.numericQty.Focus();
					return;
				}
				if (this.numericQty.Focused)
				{
					this.numericPrice.Focus();
					return;
				}
				if (this.numericPrice.Focused)
				{
					this.radioB.Focus();
					return;
				}
				if (this.radioB.Focused || this.radioS.Focused)
				{
					this.comboCommodity.Focus();
					return;
				}
			}
			else if (e == Keys.Down)
			{
				if (this.comboCommodity.Focused)
				{
					this.radioB.Focus();
					return;
				}
				if (this.radioB.Focused || this.radioS.Focused)
				{
					this.numericPrice.Focus();
					return;
				}
				if (this.numericPrice.Focused)
				{
					this.numericQty.Focus();
					return;
				}
				if (this.numericQty.Focused)
				{
					this.buttonOrder.Focus();
					return;
				}
				if (this.buttonOrder.Focused || this.buttonAddPre.Focused)
				{
					this.comboCommodity.Focus();
				}
			}
		}
		private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (this.Cursor == Cursors.WaitCursor)
			{
				e.Cancel = true;
			}
		}
		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				switch (this.tabMain.SelectedIndex)
				{
				case 0:
					if (this.DelegateFlag || this.IdleOnMoudel > Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.DelegateLoad();
						this.DelegateFlag = false;
					}
					break;
				case 1:
					if (this.QueryOrderInfoFlag || this.IdleOnMoudel > Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.QueryOrderInfoLoad();
						this.QueryOrderInfoFlag = false;
					}
					break;
				case 2:
					if (this.QueryTradeInfoFlag || this.IdleOnMoudel > Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.QueryTradeInfoLoad();
						this.QueryTradeInfoFlag = false;
					}
					break;
				case 3:
					if (this.QueryHoldingInfoFlag || this.IdleOnMoudel > Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.QueryHoldingInfoLoad();
						this.QueryHoldingInfoFlag = false;
					}
					break;
				case 4:
					if (this.QueryFundsInfoFlag || this.IdleOnMoudel > Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.QueryFundsInfoLoad();
						this.QueryFundsInfoFlag = false;
					}
					break;
				case 5:
					if (this.PreDelegateFlag)
					{
						this.PreDelegateLoad();
						this.PreDelegateFlag = false;
					}
					break;
				case 6:
					if (this.InvestorFlag)
					{
						this.InvestorInfoLoad();
						this.InvestorFlag = false;
					}
					break;
				}
				this.IdleOnMoudel = 0;
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("tabMain_SelectedIndexChanged异常：" + ex.Message);
			}
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			if (this.numericPrice.Text.Length > 0 && this.numericQty.Text.Length > 0)
			{
				this.buttonOrder.Enabled = false;
				this.buttonOrder6.Enabled = false;
				this.toolStripButtonOrder.Enabled = false;
				this.toolStripButtonBill.Enabled = false;
				this.buttonClick = true;
				WaitCallback callBack = new WaitCallback(this.ThreadO_CommodityInfo);
				ThreadPool.QueueUserWorkItem(callBack, this.GetCommodityID(this.comboCommodity.Text));
				return;
			}
			if (this.numericPrice.Text.Length <= 0)
			{
				this.labelLargestTN.ForeColor = Global.ErrorColor;
				this.labelLargestTN.Text = "请输入正确的价格区间";
				this.numericPrice.Focus();
				return;
			}
			MessageBox.Show("请输入数量！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			this.numericQty.Focus();
		}
		private void ThreadO_CommodityInfo(object o)
		{
			CommodityInfo commodityInfo = this.GetCommodityInfo((string)o);
			if (commodityInfo != null)
			{
				this.butOrderComm = new MainForm.Order_CommdityInfoCallback(this.ButOrderComm);
				base.Invoke(this.butOrderComm, new object[]
				{
					commodityInfo
				});
				return;
			}
			this.butOrderComm = new MainForm.Order_CommdityInfoCallback(this.ButOrderComm);
			base.Invoke(this.butOrderComm, new object[]
			{
				new CommodityInfo()
			});
		}
		private void ButOrderComm(CommodityInfo commodityInfo)
		{
			if (commodityInfo == null)
			{
				MessageBox.Show("输入的商品不存在！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.FillInfoText("输入的商品不存在！", Global.ErrorColor, true);
				this.displayInfo = false;
				this.buttonOrder.Enabled = true;
				this.buttonOrder6.Enabled = true;
				this.toolStripButtonOrder.Enabled = true;
				this.toolStripButtonBill.Enabled = true;
				this.comboCommodity.Focus();
				return;
			}
			if (!this.CheckCommodity(this.GetCommodityID(this.comboCommodity.Text)))
			{
				this.FillInfoText("输入的商品不存在！", Global.ErrorColor, true);
				this.displayInfo = false;
				this.buttonOrder.Enabled = true;
				this.buttonOrder6.Enabled = true;
				this.toolStripButtonOrder.Enabled = true;
				this.toolStripButtonBill.Enabled = true;
				this.comboCommodity.Focus();
				return;
			}
			if (Convert.ToInt64(this.numericPrice.Value * 100000m) % Convert.ToInt64((decimal)commodityInfo.Spread * 100000m) != 0L && !this.numericLPrice.Visible)
			{
				this.FillInfoText("价格不符合要求！商品价格最小变动价位为【" + commodityInfo.Spread + "】", Global.ErrorColor, true);
				this.displayInfo = false;
				this.buttonOrder.Enabled = true;
				this.buttonOrder6.Enabled = true;
				this.toolStripButtonOrder.Enabled = true;
				this.toolStripButtonBill.Enabled = true;
				this.numericPrice.Focus();
				return;
			}
			if (this.numericQty.Value <= 0m)
			{
				this.FillInfoText("数量不能为０！", Global.ErrorColor, true);
				this.displayInfo = false;
				this.buttonOrder.Enabled = true;
				this.buttonOrder6.Enabled = true;
				this.toolStripButtonOrder.Enabled = true;
				this.toolStripButtonBill.Enabled = true;
				this.numericQty.Focus();
				return;
			}
			if (Convert.ToInt32(this.numericQty.Value) % Convert.ToInt32(commodityInfo.MinQty) != 0)
			{
				MessageBox.Show("数量不符合要求！商品最小变动数量为【" + commodityInfo.MinQty + "】");
				this.FillInfoText("数量不符合要求！商品最小变动数量为【" + commodityInfo.MinQty + "】", Global.ErrorColor, true);
				this.displayInfo = false;
				this.buttonOrder.Enabled = true;
				this.buttonOrder6.Enabled = true;
				this.toolStripButtonOrder.Enabled = true;
				this.toolStripButtonBill.Enabled = true;
				this.numericQty.Focus();
				return;
			}
			OrderRequestVO orderRequestVO = new OrderRequestVO();
			orderRequestVO.UserID = Global.UserID;
			orderRequestVO.CustomerID = this.tbTranc.Text + this.comboTranc.Text;
			orderRequestVO.MarketID = this.marketID;
			if (this.radioB.Checked)
			{
				orderRequestVO.BuySell = 1;
			}
			else if (this.radioS.Checked)
			{
				orderRequestVO.BuySell = 2;
			}
			orderRequestVO.CommodityID = this.GetCommodityID(this.comboCommodity.Text);
			orderRequestVO.Price = Convert.ToDouble(Math.Round(this.numericPrice.Value, this.numericPrice.DecimalPlaces));
			orderRequestVO.Quantity = (long)Convert.ToInt32(this.numericQty.Value);
			if (this.radioO.Checked)
			{
				orderRequestVO.SettleBasis = 1;
			}
			else if (this.radioL.Checked)
			{
				orderRequestVO.SettleBasis = 2;
				if (IniData.GetInstance().CloseMode == 2)
				{
					orderRequestVO.CloseMode = 2;
					if (this.rbCloseT.Checked)
					{
						orderRequestVO.TimeFlag = 1;
					}
					else
					{
						orderRequestVO.CloseMode = 1;
					}
				}
				else if (IniData.GetInstance().CloseMode == 3)
				{
					orderRequestVO.CloseMode = 3;
					orderRequestVO.LPrice = Convert.ToDouble(this.numericLPrice.Value);
				}
				else
				{
					orderRequestVO.CloseMode = 1;
				}
			}
			orderRequestVO.BillType = 0;
			string text = string.Empty;
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"商品代码：",
				orderRequestVO.CommodityID,
				"\r\n商品价格：",
				orderRequestVO.Price,
				"   商品数量:",
				orderRequestVO.Quantity,
				"\r\n买卖方式：",
				Global.BuySellStrArr[(int)orderRequestVO.BuySell],
				"   \r\n\u3000\u3000\u3000确定下委托单吗？"
			});
			if (!IniData.GetInstance().ShowDialog)
			{
				this.Order(orderRequestVO);
				return;
			}
			MessageForm messageForm = new MessageForm("委托单信息", text, 0);
			if (this.radioB.Checked)
			{
				messageForm.ForeColor = Color.Red;
			}
			else
			{
				messageForm.ForeColor = Color.Green;
			}
			messageForm.Owner = this;
			messageForm.ShowDialog();
			messageForm.Dispose();
			if (messageForm.isOK)
			{
				this.Order(orderRequestVO);
				return;
			}
			this.buttonOrder.Enabled = true;
			this.buttonOrder6.Enabled = true;
			this.toolStripButtonOrder.Enabled = true;
			this.toolStripButtonBill.Enabled = true;
		}
		private void Order(OrderRequestVO orderRequestVO)
		{
			Logger.wirte(1, "下单线程提交，等待程序处理");
			WaitCallback callBack = new WaitCallback(this.Order);
			ThreadPool.QueueUserWorkItem(callBack, orderRequestVO);
		}
		private void Order(object _orderRequestVO)
		{
			OrderRequestVO req = (OrderRequestVO)_orderRequestVO;
			ResponseVO responseVO = this.dataProcess.Order(req);
			MainForm.ResponseVOCallback method = new MainForm.ResponseVOCallback(this.OrderMessage);
			this.HandleCreated();
			base.Invoke(method, new object[]
			{
				responseVO
			});
		}
		private void OrderMessage(ResponseVO responseVO)
		{
			if (this.labPrice.Text != Global.M_ResourceManager.GetString("TradeStr_LabPrice1"))
			{
				this.numericPrice.Value = 0m;
			}
			this.numericQty.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.labelLargestTN.Text = "";
			this.buttonOrder.Enabled = true;
			this.buttonOrder6.Enabled = true;
			this.toolStripButtonOrder.Enabled = true;
			this.toolStripButtonBill.Enabled = true;
			if (this.comboTranc.Visible)
			{
				this.comboTranc.Focus();
			}
			else
			{
				this.comboCommodity.Focus();
			}
			if (responseVO.RetCode == 0L)
			{
				this.FillInfoText("成功委托", Global.RightColor, this.displayInfo);
				this.refreshFlag = true;
				return;
			}
			if (IniData.GetInstance().FailShowDialog)
			{
				MessageBox.Show(responseVO.RetMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			this.FillInfoText(responseVO.RetMessage, Global.ErrorColor, this.displayInfo);
		}
		private void buttonAddPre_Click(object sender, EventArgs e)
		{
			if (this.numericPrice.Text.Length > 0 && this.numericQty.Text.Length > 0)
			{
				this.buttonClick = true;
				WaitCallback callBack = new WaitCallback(this.ThreadPO_CommodityInfo);
				ThreadPool.QueueUserWorkItem(callBack, this.GetCommodityID(this.comboCommodity.Text));
				return;
			}
			if (this.numericPrice.Text.Length <= 0)
			{
				this.labelLargestTN.ForeColor = Global.ErrorColor;
				this.labelLargestTN.Text = "请输入正确的价格区间";
				this.numericPrice.Focus();
				return;
			}
			MessageBox.Show("请输入数量！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			this.numericQty.Focus();
		}
		private void ThreadPO_CommodityInfo(object o)
		{
			CommodityInfo commodityInfo = this.GetCommodityInfo((string)o);
			if (commodityInfo != null)
			{
				this.preorder_commodityInfo = new MainForm.PreOrder_CommdityInfoCallback(this.ThPreOrder);
				this.HandleCreated();
				base.Invoke(this.preorder_commodityInfo, new object[]
				{
					commodityInfo
				});
				return;
			}
			this.preorder_commodityInfo = new MainForm.PreOrder_CommdityInfoCallback(this.ThPreOrder);
			this.HandleCreated();
			base.Invoke(this.preorder_commodityInfo, new object[]
			{
				new CommodityInfo()
			});
		}
		private void ThPreOrder(CommodityInfo commodityInfo)
		{
			if (commodityInfo != null)
			{
				if (this.numericPrice.Value > (decimal)commodityInfo.SpreadUp || this.numericPrice.Value < (decimal)commodityInfo.SpreadDown)
				{
					this.FillInfoText(string.Concat(new object[]
					{
						"价格不符合要求！应在",
						commodityInfo.SpreadDown,
						"与",
						commodityInfo.SpreadUp,
						"之间！"
					}), Global.ErrorColor, true);
					this.displayInfo = false;
					this.numericPrice.Focus();
					return;
				}
				if (Convert.ToInt64(this.numericPrice.Value * 100000m) % Convert.ToInt64((decimal)commodityInfo.Spread * 100000m) != 0L)
				{
					this.FillInfoText("价格不符合要求！商品价格最小变动价位为【" + commodityInfo.Spread + "】", Global.ErrorColor, true);
					this.displayInfo = false;
					this.numericPrice.Focus();
					return;
				}
				if (this.numericQty.Value <= 0m)
				{
					this.FillInfoText("数量不能为０！", Global.ErrorColor, true);
					this.displayInfo = false;
					this.numericQty.Focus();
					return;
				}
				if (!this.CheckCommodity(this.GetCommodityID(this.comboCommodity.Text)))
				{
					this.FillInfoText("输入的商品不存在！", Global.ErrorColor, true);
					this.displayInfo = false;
					this.comboCommodity.Focus();
					return;
				}
				if (Convert.ToInt32(this.numericQty.Value) % Convert.ToInt32(commodityInfo.MinQty) != 0)
				{
					MessageBox.Show("数量不符合要求！商品最小变动数量为【" + commodityInfo.MinQty + "】");
					this.FillInfoText("最小数量变动为:" + commodityInfo.MinQty + "的整数倍", Global.ErrorColor, this.displayInfo);
					this.numericQty.Focus();
					return;
				}
				string[] columns = new string[]
				{
					"ID",
					"TransactionsCode",
					"commodityCode",
					"CommodityName",
					"B_S",
					"O_L",
					"price",
					"qty",
					"MarKet",
					"LPrice",
					"TodayPosition",
					"CloseMode",
					"TimeFlag"
				};
				int num = int.Parse(MainForm.dsPreDelegate.Tables[1].Rows[0][0].ToString()) + 1;
				MainForm.XmlPreDelegate.UpdateXmlCounter(num);
				string text = string.Concat(num);
				string text2 = this.tbTranc.Text + this.comboTranc.Text;
				string commodityID = this.GetCommodityID(this.comboCommodity.Text);
				string text3 = this.dataProcess.ht_CommodityInfo[commodityID].ToString();
				short num2;
				if (this.radioB.Checked)
				{
					num2 = 1;
				}
				else
				{
					num2 = 2;
				}
				short num3;
				if (this.radioO.Checked)
				{
					num3 = 1;
				}
				else
				{
					num3 = 2;
				}
				string text4 = Global.BuilderString(Math.Round(this.numericPrice.Value, this.numericPrice.DecimalPlaces).ToString(Global.formatMoney));
				string text5 = this.numericQty.Value.ToString();
				string text6 = this.numericLPrice.Value.ToString();
				string text7 = string.Empty;
				string text8 = string.Empty;
				string text9 = string.Empty;
				if (this.radioL.Checked)
				{
					if (IniData.GetInstance().CloseMode == 2)
					{
						text8 = "2";
						if (this.rbCloseT.Checked)
						{
							text9 = "1";
							text7 = Global.TimeFlagStrArr[0];
						}
						else
						{
							text7 = Global.TimeFlagStrArr[1];
							text8 = "1";
						}
					}
					else if (IniData.GetInstance().CloseMode == 3)
					{
						text8 = "3";
						text7 = Global.CloseModeStrArr[2];
					}
					else
					{
						text8 = "1";
						text7 = Global.CloseModeStrArr[0];
					}
				}
				string[] columnValue = new string[]
				{
					text,
					text2,
					commodityID,
					text3,
					Global.BuySellStrArr[(int)num2],
					Global.SettleBasisStrArr[(int)num3],
					text4,
					text5,
					this.marketID,
					text6,
					text7,
					text8,
					text9
				};
				string text10 = string.Empty;
				string text11 = text10;
				text10 = string.Concat(new string[]
				{
					text11,
					"商品代码：",
					this.GetCommodityID(this.comboCommodity.Text),
					"\r\n商品价格：",
					this.numericPrice.Text,
					"   商品数量:",
					this.numericQty.Text,
					"\r\n买卖方式：",
					Global.BuySellStrArr[(int)num2],
					"\r\n\u3000\u3000\u3000确定下预埋委托单吗？"
				});
				if (!IniData.GetInstance().ShowDialog)
				{
					this.AddEmbeddedOrder(columns, columnValue);
					return;
				}
				MessageForm messageForm = new MessageForm("委托单信息", text10, 0);
				if (this.radioB.Checked)
				{
					messageForm.ForeColor = Color.Red;
				}
				else
				{
					messageForm.ForeColor = Color.Green;
				}
				messageForm.Owner = this;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					this.AddEmbeddedOrder(columns, columnValue);
					return;
				}
			}
			else
			{
				MessageBox.Show("输入的商品不存在！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.FillInfoText("输入的商品不存在！", Global.ErrorColor, true);
				this.displayInfo = false;
				this.comboCommodity.Focus();
			}
		}
		private void AddEmbeddedOrder(string[] Columns, string[] ColumnValue)
		{
			string text = MainForm.XmlPreDelegate.WriteXmlByDataSet(Columns, ColumnValue);
			if (text.Equals("true"))
			{
				this.FillInfoText("添加预埋委托成功！", Global.RightColor, this.displayInfo);
			}
			else
			{
				this.FillInfoText(text, Global.ErrorColor, this.displayInfo);
			}
			this.numericPrice.Value = 0m;
			this.numericQty.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.dgPreDelegateRefresh();
			if (this.comboTranc.Visible)
			{
				this.comboTranc.Focus();
				return;
			}
			this.comboCommodity.Focus();
		}
		private void comboTranc_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}
		private void tradeTime_Tick(object sender, EventArgs e)
		{
			this.IdleOnMoudel++;
			this.IdleRefreshButton++;
			if (Global.broadcastFlag)
			{
				if (this.broadcastCount % 2 == 0)
				{
					if (!Tools.StrToBool((string)Global.HTConfig["DisplayBroadcast"], false))
					{
						this.toolStripButtonMsg.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_UBCGlobal");
					}
				}
				else
				{
					this.toolStripButtonMsg.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_userbroadcast");
				}
				this.broadcastCount++;
			}
			TimeSpan t = new TimeSpan(0, 0, 1);
			Global.ServerTime += t;
			this.time.Text = Global.ServerTime.ToLongTimeString();
			if (this.MessageInfo.Visible)
			{
				this.MeInfoNum++;
				if (this.MeInfoNum % 8 != 0)
				{
					Label expr_F1 = this.MessageInfo;
					expr_F1.Text += ".";
					return;
				}
				this.MessageInfo.Text = this.messageInfomation;
				this.MeInfoNum = 0;
			}
		}
		private void ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.IdleRefreshButton > Tools.StrToInt((string)Global.HTConfig["MaxIdleRefreshButton"], 5) || e == null)
				{
					switch (this.tabMain.SelectedIndex)
					{
					case 0:
						this.DelegateLoad();
						this.DelegateFlag = false;
						break;
					case 1:
						this.QueryOrderInfoLoad();
						this.QueryOrderInfoFlag = false;
						break;
					case 2:
						this.QueryTradeInfoLoad();
						this.QueryTradeInfoFlag = false;
						break;
					case 3:
						this.QueryHoldingInfoLoad();
						this.QueryHoldingInfoFlag = false;
						break;
					case 4:
						this.QueryFundsInfoLoad();
						this.QueryFundsInfoFlag = false;
						break;
					case 5:
						if (this.PreDelegateFlag)
						{
							this.PreDelegateLoad();
							this.PreDelegateFlag = false;
						}
						break;
					case 6:
						if (this.InvestorFlag)
						{
							this.InvestorInfoLoad();
							this.InvestorFlag = false;
						}
						break;
					}
					this.IdleRefreshButton = 0;
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("ToolStripMenuItem_Click异常：" + ex.Message);
			}
		}
		private void Thread_RFGetCommoity(object o)
		{
			CommodityInfo commodityInfo = this.dataProcess.QueryCommodityInfo(this.marketID, (string)o);
			this.rfgetcommodity = new MainForm.RFGetCommoityCallback(this.th_RFGetCommoity);
			base.Invoke(this.rfgetcommodity, new object[]
			{
				commodityInfo
			});
		}
		private void th_RFGetCommoity(CommodityInfo commodityInfo)
		{
		}
		private void numericPrice_ValueChanged(object sender, EventArgs e)
		{
		}
		private void numericPrice_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numericPrice.DecimalPlaces == 0)
			{
				if (this.numericPrice.Value.ToString().Length > 6)
				{
					this.numericPrice.Value = this.numericPrice.Maximum;
					e.Handled = true;
				}
				if (e.KeyCode == Keys.Decimal)
				{
					this.numericPrice.Select(this.numericPrice.Value.ToString().Length, 0);
					e.Handled = true;
				}
				if (e.KeyValue % 48 == this.numericPrice.Value)
				{
					this.numericPrice.Select(this.numericPrice.Value.ToString().Length, 0);
				}
			}
		}
		private void numericQty_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numericQty.Value.ToString().Length >= 6)
			{
				this.numericQty.Value = this.numericQty.Maximum;
				e.Handled = true;
			}
			if (e.KeyValue % 48 == this.numericQty.Value)
			{
				this.numericQty.Select(this.numericQty.Value.ToString().Length, 0);
			}
		}
		private void numericQty_Enter(object sender, EventArgs e)
		{
			try
			{
				this.querytimes += 1L;
			}
			catch (Exception)
			{
				this.querytimes = 0L;
			}
			this.numericQty.Select(0, this.numericQty.Value.ToString().Length);
			this.labelLargestTN.ForeColor = Global.LightColor;
			short num = 1;
			if (this.radioS.Checked)
			{
				num = 2;
			}
			short num2 = 1;
			if (this.radioL.Checked)
			{
				num2 = 2;
				this.labelLargestTN.Text = "参考可转让量：--";
			}
			else
			{
				this.labelLargestTN.Text = "参考可订立量：--";
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Commodity", this.GetCommodityID(this.comboCommodity.Text));
			hashtable.Add("B_SType", num);
			hashtable.Add("O_LType", num2);
			hashtable.Add("numericPrice", Convert.ToDouble(this.numericPrice.Value));
			hashtable.Add("labelPrevClearV", Convert.ToInt32(this.ToDecimal(this.labelPrevClearV.Text)));
			hashtable.Add("tbTranc_comboTranc", this.tbTranc.Text + this.comboTranc.Text);
			hashtable.Add("Times", this.querytimes);
			WaitCallback callBack = new WaitCallback(this.Qty);
			ThreadPool.QueueUserWorkItem(callBack, hashtable);
		}
		private void Qty(object o)
		{
			int num = 0;
			Hashtable hashtable = (Hashtable)o;
			long num2 = (long)hashtable["Times"];
			CommodityInfo commodityInfo = this.GetCommodityInfo((string)hashtable["Commodity"]);
			if (commodityInfo == null)
			{
				return;
			}
			if ((Convert.ToDouble(this.numericPrice.Value) <= commodityInfo.SpreadUp && Convert.ToDouble(this.numericPrice.Value) >= commodityInfo.SpreadDown) || this.numericLPrice.Visible)
			{
				num = this.dataProcess.CalculatLargestTradeNum(commodityInfo, (double)hashtable["numericPrice"], (short)hashtable["B_SType"], (short)hashtable["O_LType"], (int)hashtable["labelPrevClearV"], this.marketID, (string)hashtable["tbTranc_comboTranc"]);
			}
			this.numericQtyInfo = new MainForm.NumericQtyInfoCallback(this.NumericQtyInfo);
			this.HandleCreated();
			base.Invoke(this.numericQtyInfo, new object[]
			{
				commodityInfo,
				num,
				num2
			});
		}
		private void NumericQtyInfo(CommodityInfo commodityInfo, long TradeNum, long ltimes)
		{
			if (commodityInfo != null)
			{
				if (this.radioL.Checked)
				{
					if (this.numericPrice.Text.Length > 0)
					{
						if ((Convert.ToDouble(Math.Round(this.numericPrice.Value, 2)) <= commodityInfo.SpreadUp && Convert.ToDouble(Math.Round(this.numericPrice.Value, 2)) >= commodityInfo.SpreadDown) || this.numericLPrice.Visible)
						{
							if (ltimes == this.querytimes)
							{
								this.labelLargestTN.ForeColor = Global.LightColor;
								this.labelLargestTN.Text = "参考可卖量：" + TradeNum.ToString();
							}
						}
						else if (this.labPrice.Text != Global.M_ResourceManager.GetString("TradeStr_LabPrice1") && !this.numericLPrice.Visible)
						{
							this.labelLargestTN.ForeColor = Global.ErrorColor;
							this.labelLargestTN.Text = "请输入正确的价格区间";
							this.numericPrice.Focus();
						}
					}
					else
					{
						this.labelLargestTN.ForeColor = Global.ErrorColor;
						this.labelLargestTN.Text = "请输入价格";
						this.numericPrice.Focus();
					}
				}
				else if (this.numericPrice.Text.Length > 0)
				{
					if (Convert.ToDouble(Math.Round(this.numericPrice.Value, 2)) <= commodityInfo.SpreadUp && Convert.ToDouble(Math.Round(this.numericPrice.Value, 2)) >= commodityInfo.SpreadDown)
					{
						this.labelLargestTN.ForeColor = Global.LightColor;
						if (ltimes == this.querytimes)
						{
							this.labelLargestTN.Text = "参考可买量：" + TradeNum.ToString();
						}
					}
					else
					{
						this.labelLargestTN.ForeColor = Global.ErrorColor;
						this.labelLargestTN.Text = "请输入正确的价格区间";
						this.numericPrice.Focus();
					}
				}
				else
				{
					this.labelLargestTN.ForeColor = Global.ErrorColor;
					this.labelLargestTN.Text = "请输入正确的价格区间";
					this.numericPrice.Focus();
				}
				if (commodityInfo.MinQty != 0.0)
				{
					string infoMessage = string.Concat(new object[]
					{
						"商品名称：",
						commodityInfo.CommodityName,
						"   商品最小变动数量为【",
						commodityInfo.MinQty,
						"】"
					});
					this.FillInfoText(infoMessage, Global.RightColor, this.displayInfo);
				}
			}
		}
		private void radioB_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isAutoPrice)
			{
				if (this.radioB.Checked)
				{
					this.numericPrice.Value = (decimal)this.sPrice;
				}
				else
				{
					this.numericPrice.Value = (decimal)this.bPrice;
				}
				this.isAutoPrice = false;
			}
			if (!IniData.GetInstance().SetDoubleClick && this.labPrice.Text == Global.M_ResourceManager.GetString("TradeStr_LabPrice2"))
			{
				return;
			}
			if (IniData.GetInstance().AutoAddBSPQ)
			{
				if (this.radioB.Checked)
				{
					this.numericPrice.Value = (decimal)this.sPrice;
					return;
				}
				this.numericPrice.Value = (decimal)this.bPrice;
			}
		}
		private void comboMarKet_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.marketID = this.GetCurrentMarKetID();
			if (!this.LoadFlag)
			{
				this.comboCommodityRefresh();
				this.DelegateRefresh();
			}
		}
		internal string GetCurrentMarKetID()
		{
			string result = string.Empty;
			if (!this.comboMarKet.Text.Equals("全部") && Global.MarketHT.Count > 1)
			{
				result = this.comboMarKet.SelectedValue.ToString();
			}
			return result;
		}
		private bool CheckCommodity(string commodityCode)
		{
			bool result = false;
			if (Global.MarketHT.Count > 1)
			{
				IEnumerator enumerator = this.dsCommodity.Tables[0].Rows.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DataRow dataRow = (DataRow)enumerator.Current;
						if (dataRow["commodityCode"].ToString().Equals(commodityCode) && dataRow["MarKet"].ToString().Equals(this.marketID))
						{
							bool result2 = true;
							return result2;
						}
					}
					return result;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			foreach (DataRow dataRow2 in this.dsCommodity.Tables[0].Rows)
			{
				if (dataRow2["commodityCode"].ToString().Equals(commodityCode))
				{
					bool result2 = true;
					return result2;
				}
			}
			return result;
		}
		internal CommodityInfo GetCommodityInfo(string commodityID)
		{
			CommodityInfo commodityInfo = new CommodityInfo();
			if (!this.htCommodity.ContainsKey(commodityID) || this.htCommodity[commodityID] == null)
			{
				commodityInfo = this.dataProcess.QueryCommodityInfo(this.marketID, commodityID);
				if (commodityInfo == null || this.htCommodity.ContainsKey(commodityID))
				{
					return commodityInfo;
				}
				try
				{
					this.htCommodity.Add(commodityID, commodityInfo);
					return commodityInfo;
				}
				catch (ArgumentException)
				{
					return commodityInfo;
				}
				catch (Exception)
				{
					return commodityInfo;
				}
			}
			commodityInfo = (CommodityInfo)this.htCommodity[commodityID];
			return commodityInfo;
		}
		private void ComboMarKetLoad()
		{
			ArrayList arrayList = new ArrayList();
			if (Global.MarketHT != null)
			{
				foreach (DictionaryEntry dictionaryEntry in Global.MarketHT)
				{
					MarkeInfo markeInfo = (MarkeInfo)dictionaryEntry.Value;
					if (markeInfo != null)
					{
						arrayList.Add(new AddValue(markeInfo.ShortName, markeInfo.MarketID));
					}
				}
				this.comboMarKet.DisplayMember = "Display";
				this.comboMarKet.ValueMember = "Value";
				this.comboMarKet.DataSource = null;
				this.comboMarKet.DataSource = arrayList;
			}
			this.comboMarKet.SelectedIndex = 0;
			this.labTrancCode.Visible = false;
			this.tbTranc.Visible = false;
			this.comboTranc.Visible = false;
			this.labelTrancF2.Visible = false;
			this.comboTrancF2.Visible = false;
			this.labelTrancF3.Visible = false;
			this.comboTrancF3.Visible = false;
			this.labelTrancF4.Visible = false;
			this.comboTrancF4.Visible = false;
			this.labelTrancF6.Visible = false;
			this.comTranc.Visible = false;
			this.labelMarKet.Visible = true;
			this.comboMarKet.Visible = true;
		}
		private void ComboCommodityLoad()
		{
			int num = 0;
			this.comboCommodityF2.Items.Add("全部");
			this.comboCommodityF3.Items.Add("全部");
			this.comboCommodityF4.Items.Add("全部");
			foreach (DataRow dataRow in this.dsCommodity.Tables[0].Rows)
			{
				if (Global.MarketHT.Count > 1)
				{
					if ((bool)dataRow["Flag"])
					{
						if (this.queryMarketID != null && this.queryMarketID.Length > 0)
						{
							if (dataRow["MarKet"].ToString().Equals(this.queryMarketID))
							{
								this.comboCommodityF2.Items.Add(dataRow["commodityCode"].ToString());
								this.comboCommodityF3.Items.Add(dataRow["commodityCode"].ToString());
								this.comboCommodityF4.Items.Add(dataRow["commodityCode"].ToString());
								this.comboCommodity.Items.Add(dataRow["commodityCode"].ToString() + " " + dataRow["CommodityName"].ToString());
								num++;
							}
						}
						else
						{
							this.comboCommodityF2.Items.Add(dataRow["commodityCode"].ToString());
							this.comboCommodityF3.Items.Add(dataRow["commodityCode"].ToString());
							this.comboCommodityF4.Items.Add(dataRow["commodityCode"].ToString());
							if (dataRow["MarKet"].ToString().Equals(this.marketID))
							{
								this.comboCommodity.Items.Add(dataRow["commodityCode"].ToString() + " " + dataRow["CommodityName"].ToString());
								num++;
							}
						}
					}
				}
				else if ((bool)dataRow["Flag"])
				{
					this.comboCommodityF2.Items.Add(dataRow["commodityCode"].ToString());
					this.comboCommodityF3.Items.Add(dataRow["commodityCode"].ToString());
					this.comboCommodityF4.Items.Add(dataRow["commodityCode"].ToString());
					this.comboCommodity.Items.Add(dataRow["commodityCode"].ToString() + " " + dataRow["CommodityName"].ToString());
					num++;
				}
			}
			if (num == 0)
			{
				foreach (DataRow dataRow2 in this.dsCommodity.Tables[0].Rows)
				{
					if (Global.MarketHT.Count > 1)
					{
						if (this.queryMarketID != null && this.queryMarketID.Length > 0)
						{
							if (dataRow2["MarKet"].ToString().Equals(this.queryMarketID))
							{
								this.comboCommodityF2.Items.Add(dataRow2["commodityCode"].ToString());
								this.comboCommodityF3.Items.Add(dataRow2["commodityCode"].ToString());
								this.comboCommodityF4.Items.Add(dataRow2["commodityCode"].ToString());
								this.comboCommodity.Items.Add(dataRow2["commodityCode"].ToString() + " " + dataRow2["CommodityName"].ToString());
								num++;
							}
						}
						else
						{
							this.comboCommodityF2.Items.Add(dataRow2["commodityCode"].ToString());
							this.comboCommodityF3.Items.Add(dataRow2["commodityCode"].ToString());
							this.comboCommodityF4.Items.Add(dataRow2["commodityCode"].ToString());
							if (dataRow2["MarKet"].ToString().Equals(this.marketID))
							{
								this.comboCommodity.Items.Add(dataRow2["commodityCode"].ToString() + " " + dataRow2["CommodityName"].ToString());
								num++;
							}
						}
					}
					else
					{
						this.comboCommodityF2.Items.Add(dataRow2["commodityCode"].ToString());
						this.comboCommodityF3.Items.Add(dataRow2["commodityCode"].ToString());
						this.comboCommodityF4.Items.Add(dataRow2["commodityCode"].ToString());
						this.comboCommodity.Items.Add(dataRow2["commodityCode"].ToString() + " " + dataRow2["CommodityName"].ToString());
						num++;
					}
				}
			}
			if (this.comboCommodityF2.Items.Count > 0)
			{
				this.comboCommodityF2.SelectedIndex = 0;
			}
			if (this.comboCommodityF3.Items.Count > 0)
			{
				this.comboCommodityF3.SelectedIndex = 0;
			}
			if (this.comboCommodityF4.Items.Count > 0)
			{
				this.comboCommodityF4.SelectedIndex = 0;
			}
			if (this.comboCommodity.Items.Count > 0)
			{
				this.currentCommodity = this.GetCommodityID(this.comboCommodity.Items[0].ToString());
				this.comboCommodity.SelectedIndex = 0;
			}
		}
		private void ComboTrancLoad()
		{
			this.comboTrancF2.Items.Add("全部");
			this.comboTrancF3.Items.Add("全部");
			this.comboTrancF4.Items.Add("全部");
			this.comboTranc.Items.Add("00");
			this.comboTrancF2.Items.Add("00");
			this.comboTrancF3.Items.Add("00");
			this.comboTrancF4.Items.Add("00");
			if (this.comboTranc.Items.Count > 0)
			{
				this.comboTranc.SelectedIndex = 0;
			}
			this.comboTrancF2.SelectedIndex = 0;
			this.comboTrancF3.SelectedIndex = 0;
			this.comboTrancF4.SelectedIndex = 0;
		}
		internal void comboCommodityRefresh()
		{
			this.comboCommodity.Items.Clear();
			this.comboCommodity.Text = "";
			this.comboCommodityF2.Items.Clear();
			this.comboCommodityF3.Items.Clear();
			this.comboCommodityF4.Items.Clear();
			this.dsCommodity = this.XmlCommodity.GetDataSetByXml();
			this.ComboCommodityLoad();
		}
		internal void comboTrancRefresh()
		{
			this.comboTranc.Items.Clear();
			this.comboTrancF2.Items.Clear();
			this.comboTrancF3.Items.Clear();
			this.comboTrancF4.Items.Clear();
			this.dsTransactions = this.XmlTransactions.GetDataSetByXml();
			this.ComboTrancLoad();
		}
		private void toolStripButtonBill_Click(object sender, EventArgs e)
		{
			this.billOrder = BillOrder.Instance();
			this.billOrder.TopMost = true;
			this.billOrder.m_MainForm = this;
			this.billOrder.Show();
		}
		private void toolStripButtonOrder_Click(object sender, EventArgs e)
		{
			this.formOrder = FormOrder.Instance();
			this.formOrder.TopMost = true;
			this.formOrder.m_MainForm = this;
			this.formOrder.Show();
		}
		private void toolStripButtonSet_Click(object sender, EventArgs e)
		{
			new UserSet(this)
			{
				TopMost = true
			}.ShowDialog();
		}
		private void toolStripButtonLock_Click(object sender, EventArgs e)
		{
			if (this.LockFormEvent != null)
			{
				this.LockFormEvent(sender, e);
			}
			this.LockSet(false);
		}
		private void toolStripButtonExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void toolStripButtonAbout_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
		}
		private void toolStripButtonHelp_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, this.helpProvider.HelpNamespace);
		}
		private void toolStripButtonMsg_Click(object sender, EventArgs e)
		{
			Global.broadcastFlag = false;
			this.toolStripButtonMsg.Image = (Image)Global.M_ResourceManager.GetObject("TradeImg_userbroadcast");
			this.broadcast.Show();
		}
		private void statusInfo_SizeChanged(object sender, EventArgs e)
		{
			this.info.Width = this.statusInfo.Width - this.user.Width - this.status.Width - this.time.Width - 22;
		}
		private void groupBox3_Enter(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void groupBox3_MouseHover(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void radioB_Enter(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void radioS_Enter(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void radioB_MouseHover(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void radioS_MouseHover(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为买入，按“2”表示为卖出", Global.RightColor, this.displayInfo);
		}
		private void groupBox4_Enter(object sender, EventArgs e)
		{
		}
		private void groupBox4_MouseHover(object sender, EventArgs e)
		{
		}
		private void gbCloseMode_Enter(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为" + Global.TimeFlagStrArr[0] + "，按“2”表示为" + Global.TimeFlagStrArr[1], Global.RightColor, this.displayInfo);
		}
		private void gbCloseMode_MouseHover(object sender, EventArgs e)
		{
			this.FillInfoText("按“1”表示为" + Global.TimeFlagStrArr[0] + "，按“2”表示为" + Global.TimeFlagStrArr[1], Global.RightColor, this.displayInfo);
		}
		private void radioO_Enter(object sender, EventArgs e)
		{
		}
		public void radioL_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioL.Checked)
			{
				switch (IniData.GetInstance().CloseMode)
				{
				case 1:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = false;
					return;
				case 2:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = true;
					return;
				case 3:
					this.labelLPrice.Visible = true;
					this.numericLPrice.Visible = true;
					this.gbCloseMode.Visible = false;
					return;
				default:
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
					this.gbCloseMode.Visible = false;
					break;
				}
			}
		}
		private void radioO_MouseHover(object sender, EventArgs e)
		{
		}
		private void radioL_MouseHover(object sender, EventArgs e)
		{
		}
		private void numericPrice_Enter(object sender, EventArgs e)
		{
			if (this.numericPrice.Value == 0m)
			{
				this.numericPrice.Select(0, this.numericPrice.Text.Length);
			}
			WaitCallback callBack = new WaitCallback(this.GetCom);
			ThreadPool.QueueUserWorkItem(callBack, this.GetCommodityID(this.comboCommodity.Text));
		}
		private void GetCom(object o)
		{
			CommodityInfo commodityInfo = this.GetCommodityInfo((string)o);
			this.commdityInfo = new MainForm.CommdityInfoCallback(this.CommdityInfo);
			this.HandleCreated();
			base.Invoke(this.commdityInfo, new object[]
			{
				commodityInfo
			});
		}
		private void CommdityInfo(CommodityInfo commodityInfo)
		{
			if (commodityInfo != null)
			{
				string infoMessage = string.Concat(new object[]
				{
					"商品名称:",
					commodityInfo.CommodityName,
					"  价格区间:",
					commodityInfo.SpreadDown,
					" – ",
					commodityInfo.SpreadUp
				});
				this.FillInfoText(infoMessage, Global.RightColor, this.displayInfo);
				if (commodityInfo.Spread < 0.1)
				{
					this.numericPrice.DecimalPlaces = 2;
					this.numericLPrice.DecimalPlaces = 2;
				}
				else if (commodityInfo.Spread < 1.0)
				{
					this.numericPrice.DecimalPlaces = 1;
					this.numericLPrice.DecimalPlaces = 1;
				}
				else
				{
					this.numericPrice.DecimalPlaces = 0;
					this.numericLPrice.DecimalPlaces = 0;
				}
				this.numericPrice.Increment = (decimal)commodityInfo.Spread;
				this.numericQty.Increment = (decimal)commodityInfo.MinQty;
				this.labelSpread.Text = string.Concat(new object[]
				{
					" 价格：",
					commodityInfo.SpreadDown,
					"–",
					commodityInfo.SpreadUp
				});
			}
		}
		private void status_DoubleClick(object sender, EventArgs e)
		{
			if (this.connectStatus == 1)
			{
				this.timerLock.Enabled = false;
				this.timerSysTime.Enabled = false;
				Configuration configuration = new Configuration();
				Hashtable section = configuration.getSection("Systems");
				string text = section["Title"].ToString();
				DialogResult dialogResult = MessageBoxEx.Show("连接中断，请检查网络状态，重新登录！", text ?? "", MessageBoxButtons.OKCancel, new string[]
				{
					"重新登录(&O)",
					"取消(&C)"
				}, MessageBoxIcon.Asterisk);
				if (dialogResult == DialogResult.OK)
				{
					LogonRequestVO logonRequestVO = new LogonRequestVO();
					logonRequestVO.UserID = Global.UserID;
					logonRequestVO.Password = Global.Password;
					logonRequestVO.RegisterWord = Global.RegisterWord;
					LogonResponseVO logonResponseVO = Global.TradeLibrary.Logon(logonRequestVO);
					if (logonResponseVO.RetCode != 0L)
					{
						this.FillInfoText(logonResponseVO.RetMessage, Global.ErrorColor, this.displayInfo);
						return;
					}
					this.buttonOrder.Enabled = true;
					this.buttonOrder6.Enabled = true;
					this.toolStripButtonOrder.Enabled = true;
					this.toolStripButtonBill.Enabled = true;
					if (!this.timerLock.Enabled)
					{
						this.timerLock.Enabled = true;
					}
					if (!this.timerSysTime.Enabled)
					{
						this.timerSysTime.Enabled = true;
					}
					if (!this.MenuRefresh.Enabled)
					{
						this.connectStatus = 0;
						this.SetStatus(null);
					}
					this.Notifier.Show("重新登录成功", "上次登录时间：" + logonResponseVO.LastTime + "\r\n上次登录IP：" + logonResponseVO.LastIP, 200, 6000, 500);
					this.FillInfoText("重新登录成功！", Global.RightColor, this.displayInfo);
				}
			}
		}
		private void refreshGN()
		{
			WaitCallback callBack = new WaitCallback(this.refreshGNCommodity);
			ThreadPool.QueueUserWorkItem(callBack, this.marketID);
		}
		private void refreshGNCommodity(object market)
		{
			if (!this.refreshGNFlag)
			{
				return;
			}
			this.refreshGNFlag = false;
			CommData commData = this.dataProcess.QueryGNCommodityInfo((string)market, this.currentCommodity);
			this.fillGNText = new MainForm.StringObjCallback(this.fillGNInfo);
			this.HandleCreated();
			if (commData != null)
			{
				base.Invoke(this.fillGNText, new object[]
				{
					commData
				});
			}
			this.refreshGNFlag = true;
		}
		private void fillGNInfo(object _commData)
		{
			CommData commData = (CommData)_commData;
			string text = commData.CommodityName.Trim();
			CommodityInfo commodityInfo = this.GetCommodityInfo(this.currentCommodity);
			if (text.Length < 2)
			{
				text = commodityInfo.CommodityName;
			}
			this.labelGNInfo.Text = commData.CommodityID.Trim() + "(" + text + ")";
			int len = 0;
			if (commodityInfo.Spread < 1.0)
			{
				len = 2;
			}
			this.labelSP3.ForeColor = this.ColorSet(commData.SPrice3, commData.PrevClear);
			this.labelSP3.Text = this.ToString(commData.SPrice3, len);
			this.labelSP2.ForeColor = this.ColorSet(commData.SPrice2, commData.PrevClear);
			this.labelSP2.Text = this.ToString(commData.SPrice2, len);
			this.labelSP1.ForeColor = this.ColorSet(commData.SPrice1, commData.PrevClear);
			this.labelSP1.Text = this.ToString(commData.SPrice1, len);
			this.labelSV3.ForeColor = this.ColorSet(commData.SValue3, commData.PrevClear);
			this.labelSV3.Text = commData.SValue3.ToString();
			this.labelSV2.ForeColor = this.ColorSet(commData.SValue2, commData.PrevClear);
			this.labelSV2.Text = commData.SValue2.ToString();
			this.labelSV1.ForeColor = this.ColorSet(commData.SValue1, commData.PrevClear);
			this.labelSV1.Text = commData.SValue1.ToString();
			this.labelBP1.ForeColor = this.ColorSet(commData.BPrice1, commData.PrevClear);
			this.labelBP1.Text = this.ToString(commData.BPrice1, len);
			this.labelBP2.ForeColor = this.ColorSet(commData.BPrice2, commData.PrevClear);
			this.labelBP2.Text = this.ToString(commData.BPrice2, len);
			this.labelBP3.ForeColor = this.ColorSet(commData.BPrice3, commData.PrevClear);
			this.labelBP3.Text = this.ToString(commData.BPrice3, len);
			this.labelBV1.ForeColor = this.ColorSet(commData.BValue1, commData.PrevClear);
			this.labelBV1.Text = commData.BValue1.ToString();
			this.labelBV2.ForeColor = this.ColorSet(commData.BValue2, commData.PrevClear);
			this.labelBV2.Text = commData.BValue2.ToString();
			this.labelBV3.ForeColor = this.ColorSet(commData.BValue3, commData.PrevClear);
			this.labelBV3.Text = commData.BValue3.ToString();
			this.bPrice = commData.BPrice1;
			this.sPrice = commData.SPrice1;
			this.labelBV3.ForeColor = this.ColorSet(commData.Last, commData.PrevClear);
			this.labelLastP.Text = this.ToString(commData.Last, len);
			this.labelCountV.Text = commData.Count.ToString();
			this.labelLimitUpV.ForeColor = this.ColorSet(commodityInfo.SpreadUp, commData.PrevClear);
			this.labelLimitUpV.Text = commodityInfo.SpreadUp.ToString();
			this.labelLimitDownV.ForeColor = this.ColorSet(commodityInfo.SpreadDown, commData.PrevClear);
			this.labelLimitDownV.Text = commodityInfo.SpreadDown.ToString();
			this.labelPrevClearV.Text = this.ToString(commData.PrevClear, len);
			this.radioB_CheckedChanged(null, null);
		}
		private void comboCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numericPrice.Value = 0m;
			this.numericQty.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.labelLargestTN.Text = "";
			this.lbmain.Visible = false;
			if (Tools.StrToBool((string)Global.HTConfig["AutoDisplayMinLine"]) && !this.LoadFlag && !this.ConnectHQ)
			{
				this.displayMinLine("", this.GetCommodityID(this.comboCommodity.Text));
			}
			this.ConnectHQ = false;
			string commodityID = this.GetCommodityID(this.comboCommodity.Text);
			if (this.dataProcess.ht_TradeMode.Count != 0 && this.dataProcess.ht_TradeMode[commodityID].ToString() == "0")
			{
				this.radioO.Enabled = true;
				this.radioL.Enabled = true;
				this.radioS.Enabled = true;
				this.radioB.Enabled = true;
				return;
			}
			this.setRadioEnable(commodityID);
		}
		private string GetCommodityID(string text)
		{
			string result = string.Empty;
			try
			{
				if (text != null)
				{
					string[] array = text.Split(new char[]
					{
						' '
					});
					if (array != null && array.Length > 0)
					{
						result = array[0];
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message + ex.StackTrace);
			}
			return result;
		}
		private void setRadioEnable(string commodityesID)
		{
			this.curCommodityMode = "0";
			this.radioO.Enabled = true;
			this.radioL.Enabled = true;
			this.radioS.Enabled = true;
			this.radioB.Enabled = true;
			for (int i = 0; i < this.BreedRep.FirmBreedInfoList.Count; i++)
			{
				if (this.BreedRep.FirmBreedInfoList[i].VarietyID == this.dataProcess.ht_Variety[commodityesID].ToString())
				{
					this.isDirectfirm = true;
					break;
				}
			}
			if (this.curCommodityMode == "0")
			{
				if (this.radioB.Checked)
				{
					this.radioO.Checked = true;
				}
				else
				{
					this.radioL.Checked = true;
				}
			}
			if ((this.curCommodityMode == "3" && this.isDirectfirm) || (this.curCommodityMode == "4" && !this.isDirectfirm))
			{
				this.radioS.Enabled = false;
				this.radioB.Checked = true;
				this.radioL.Enabled = false;
				this.radioO.Checked = true;
				return;
			}
			if ((!this.isDirectfirm && this.curCommodityMode == "3") || (this.curCommodityMode == "4" && this.isDirectfirm))
			{
				this.radioB.Enabled = false;
				this.radioS.Checked = true;
				this.radioL.Enabled = false;
				this.radioO.Checked = true;
				return;
			}
			if (this.isDirectfirm)
			{
				return;
			}
			if (!(this.curCommodityMode == "1"))
			{
				if (this.curCommodityMode == "2")
				{
					if (this.radioB.Checked)
					{
						this.radioL.Enabled = false;
						this.radioO.Checked = true;
						return;
					}
					this.radioO.Enabled = false;
					this.radioL.Checked = true;
				}
				return;
			}
			if (this.radioB.Checked)
			{
				this.radioO.Enabled = false;
				this.radioL.Checked = true;
				return;
			}
			this.radioL.Enabled = false;
			this.radioO.Checked = true;
		}
		private Color ColorSet(double value, double PrevClear)
		{
			Color result;
			if (value > PrevClear)
			{
				result = Global.HighColor;
			}
			else if (value < PrevClear)
			{
				result = Global.LowColor;
			}
			else
			{
				result = Global.EqualsColor;
			}
			return result;
		}
		private string ToString(double value)
		{
			string result = string.Empty;
			if (value != 0.0)
			{
				result = value.ToString();
			}
			else
			{
				result = "-";
			}
			return result;
		}
		private string ToString(double value, int len)
		{
			string result = string.Empty;
			if (value != 0.0)
			{
				result = value.ToString("f" + len);
			}
			else
			{
				result = "-";
			}
			return result;
		}
		private decimal ToDecimal(string value)
		{
			decimal result;
			if (value.Equals("-"))
			{
				result = 0m;
			}
			else
			{
				try
				{
					result = decimal.Parse(value);
				}
				catch
				{
					result = 0m;
				}
			}
			return result;
		}
		private void labelBidV_Click(object sender, EventArgs e)
		{
			this.numericPrice.Value = this.ToDecimal(this.labelBP2.Text);
			this.labelBP3.BorderStyle = BorderStyle.None;
			this.labelBV2.BorderStyle = BorderStyle.None;
			this.labelBV3.BorderStyle = BorderStyle.None;
			this.labelBP2.BorderStyle = BorderStyle.FixedSingle;
		}
		private void labelBidVolV_Click(object sender, EventArgs e)
		{
			this.numericQty.Value = this.ToDecimal(this.labelBV2.Text);
			this.labelBP2.BorderStyle = BorderStyle.None;
			this.labelBP3.BorderStyle = BorderStyle.None;
			this.labelBV3.BorderStyle = BorderStyle.None;
			this.labelBV2.BorderStyle = BorderStyle.FixedSingle;
		}
		private void labelBidV_DoubleClick(object sender, EventArgs e)
		{
			if (this.numericQty.Value > 0m)
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP2.Text);
				this.buttonOrder_Click(this, null);
			}
		}
		private void labelOfferV_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP3.Text);
				this.labelBP2.BorderStyle = BorderStyle.None;
				this.labelBV2.BorderStyle = BorderStyle.None;
				this.labelBV3.BorderStyle = BorderStyle.None;
				this.labelBP3.BorderStyle = BorderStyle.FixedSingle;
			}
			catch
			{
				this.FillInfoText("价格不合法", Global.ErrorColor, this.displayInfo);
			}
		}
		private void labelOfferVolV_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericQty.Value = this.ToDecimal(this.labelBV3.Text);
				this.labelBP2.BorderStyle = BorderStyle.None;
				this.labelBV2.BorderStyle = BorderStyle.None;
				this.labelBP3.BorderStyle = BorderStyle.None;
				this.labelBV3.BorderStyle = BorderStyle.FixedSingle;
			}
			catch
			{
				this.FillInfoText("数量不合法", Global.ErrorColor, this.displayInfo);
			}
		}
		private void labelLastV_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelSP3.Text);
				Graphics graphics = this.labelSP3.CreateGraphics();
				ControlPaint.DrawBorder(graphics, this.labelSP3.ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
			}
			catch
			{
				this.FillInfoText("价格不合法", Global.ErrorColor, this.displayInfo);
			}
		}
		private void labelLastV_DoubleClick(object sender, EventArgs e)
		{
			if (this.numericQty.Value > 0m)
			{
				this.numericPrice.Value = this.ToDecimal(this.labelSP3.Text);
				this.buttonOrder_Click(this, null);
			}
		}
		private void labelOfferV_DoubleClick(object sender, EventArgs e)
		{
			if (this.numericQty.Value > 0m)
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP3.Text);
				this.buttonOrder_Click(this, null);
			}
		}
		private void labelLargestTN_Click(object sender, EventArgs e)
		{
			int num = this.labelLargestTN.Text.IndexOf("：");
			if (num != -1)
			{
				try
				{
					this.numericQty.Value = this.ToDecimal(this.labelLargestTN.Text.Substring(num + 1));
				}
				catch
				{
					this.FillInfoText("最大可交易量错误：数量不合法", Global.ErrorColor, this.displayInfo);
				}
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Control activeControl = base.ActiveControl;
			int num = 0;
			while (num < 3 && activeControl is SplitContainer)
			{
				SplitContainer splitContainer = (SplitContainer)activeControl;
				activeControl = splitContainer.ActiveControl;
				num++;
			}
			if (activeControl is DataGridView || (keyData != Keys.Up && keyData != Keys.Down && keyData != Keys.Left && keyData != Keys.Right) || this.lbmain.Visible)
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
			if ((keyData == Keys.Up || keyData == Keys.Down) && !this.comboTranc.Focused && !this.comboMarKet.Focused && !this.comboCommodity.Focused && !this.radioB.Focused && !this.radioS.Focused && !this.radioO.Focused && !this.radioL.Focused && !this.numericPrice.Focused && !this.numericQty.Focused && !this.numericLPrice.Focused && !this.rbCloseT.Focused && !this.rbCloseH.Focused && !this.buttonOrder.Focused && !this.buttonAddPre.Focused)
			{
				if (this.comboMarKet.Visible)
				{
					this.comboMarKet.Focus();
				}
				else if (this.comboTranc.Visible)
				{
					this.comboTranc.Focus();
				}
				else
				{
					this.comboCommodity.Focus();
				}
				return true;
			}
			this.SetFocus(keyData);
			return true;
		}
		private void StartLock()
		{
			this.timerLock.Enabled = true;
			this.timerLockRefresh = Tools.StrToDouble((string)Global.HTConfig["HqRefreshTime"], 3.0) * 1000.0;
			this.timerSysTime.Enabled = true;
			this.sysTimeRefresh = Tools.StrToDouble((string)Global.HTConfig["SysTimeRefreshTime"], 3.0) * 1000.0;
			this.IdleStartTime = DateTime.Now;
			this.AddHook();
		}
		private void timerLock_Tick(object sender, EventArgs e)
		{
			if (this.timerLockRefresh / (double)this.timerLock.Interval <= (double)this.timerLockCount)
			{
				this.refreshGN();
				this.status_DoubleClick(this, null);
				if (IniData.GetInstance().LockEnable && DateTime.Now.Subtract(this.IdleStartTime).Minutes >= IniData.GetInstance().LockTime)
				{
					this.LockSet(false);
				}
				this.timerLockCount = 0;
			}
			this.timerLockCount++;
		}
		private void timerSysTime_Tick(object sender, EventArgs e)
		{
			if (this.sysTimeRefresh / (double)this.timerSysTime.Interval <= (double)this.sysTimeCount)
			{
				this.QuerySysTime();
				this.sysTimeCount = 0;
			}
			if (this.refreshFlag)
			{
				this.DelegateRefresh();
			}
			this.sysTimeCount++;
		}
		private void AddHook()
		{
			this.IdleStartTime = DateTime.Now;
			this.hook = new LocalHook();
			this.hook.OnMouseActivity += new MouseEventHandler(this.hook_OnMouseActivity);
			this.hook.KeyDown += new KeyEventHandler(this.hook_KeyDown);
		}
		private void hook_OnMouseActivity(object sender, MouseEventArgs e)
		{
			this.IdleStartTime = DateTime.Now;
		}
		private void hook_KeyDown(object sender, KeyEventArgs e)
		{
			this.IdleStartTime = DateTime.Now;
		}
		public void LockSet(bool type)
		{
			this.m_splitContainer.Visible = type;
			this.statusInfo.Visible = type;
			this.panelLock.Visible = !type;
			if (!type)
			{
				this.BackColor = Color.Black;
				this.timerLockRefresh = 20000.0;
				this.sysTimeRefresh = 20000.0;
			}
			else
			{
				this.BackColor = SystemColors.Control;
				this.IdleStartTime = DateTime.Now;
				this.timerLockRefresh = Tools.StrToDouble((string)Global.HTConfig["HqRefreshTime"], 3.0) * 1000.0;
				this.sysTimeRefresh = Tools.StrToDouble((string)Global.HTConfig["SysTimeRefreshTime"], 3.0) * 1000.0;
			}
			if (this.formOrder != null && !type)
			{
				this.formOrder.Close();
			}
			if (this.billOrder != null && !type)
			{
				this.billOrder.Close();
			}
			if (this.conditionOrder != null && !type)
			{
				this.conditionOrder.Close();
			}
		}
		private void buttonUnlock_Click(object sender, EventArgs e)
		{
			if (this.textBoxPwd.Text.Equals(Global.Password))
			{
				this.LockSet(true);
				this.textBoxPwd.Text = "";
				this.labelPwdInfo.Text = "请输入登录密码！";
				return;
			}
			this.labelPwdInfo.Text = "密码不正确！";
		}
		private void butKLine_Click(object sender, EventArgs e)
		{
			string text = this.GetCommodityID(this.comboCommodity.Text);
			if (Global.MarketHT.Count > 1)
			{
				text = this.marketID + "_" + text;
			}
			InterFace.CommodityInfoEventArgs e2 = new InterFace.CommodityInfoEventArgs(text, "KLineEvent");
			if (this.KLineEvent != null)
			{
				this.KLineEvent(this, e2);
			}
		}
		private void butMinLine_Click(object sender, EventArgs e)
		{
			string text = this.GetCommodityID(this.comboCommodity.Text);
			if (Global.MarketHT.Count > 1)
			{
				text = this.marketID + "_" + text;
			}
			InterFace.CommodityInfoEventArgs e2 = new InterFace.CommodityInfoEventArgs(text, "MinLineEvent");
			if (this.MinLineEvent != null)
			{
				this.MinLineEvent(this, e2);
			}
		}
		private void displayMinLine(string market, string commodityCode)
		{
			for (int i = 0; i < this.comboCommodity.Items.Count; i++)
			{
				if (commodityCode.Equals(this.GetCommodityID(this.comboCommodity.Items[i].ToString())))
				{
					this.comboCommodity.SelectedIndex = i;
					break;
				}
			}
			if (Global.MarketHT.Count > 1)
			{
				commodityCode = market + "_" + commodityCode;
			}
			InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityCode, "MinLineEvent");
			if (this.MinLineEvent != null)
			{
				this.MinLineEvent(this, e);
			}
		}
		public void SetOrderInfo(string CommodityCode, float BuyPrice, float SellPrice)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = CommodityCode.IndexOf("_");
			if (num != -1)
			{
				text = CommodityCode.Substring(0, num);
				text2 = CommodityCode.Substring(num + 1);
			}
			else
			{
				text2 = CommodityCode;
			}
			if (text2 != this.GetCommodityID(this.comboCommodity.Text))
			{
				this.ConnectHQ = true;
			}
			if (this.formOrder != null)
			{
				this.formOrder.SetOrderInfo(text, text2, BuyPrice, SellPrice);
			}
			if (Global.MarketHT.Count == 1)
			{
				int i = 0;
				while (i < this.comboCommodity.Items.Count)
				{
					if (text2.Equals(this.GetCommodityID(this.comboCommodity.Items[i].ToString())))
					{
						this.comboCommodity.SelectedIndex = i;
						if (this.radioB.Checked)
						{
							this.numericPrice.Value = (decimal)SellPrice;
							break;
						}
						this.numericPrice.Value = (decimal)BuyPrice;
						break;
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.comboMarKet.Items.Count; j++)
				{
					AddValue addValue = (AddValue)this.comboMarKet.Items[j];
					if (text.Equals(addValue.Value))
					{
						this.comboMarKet.SelectedIndex = j;
						break;
					}
				}
				int k = 0;
				while (k < this.comboCommodity.Items.Count)
				{
					if (text2.Equals(this.GetCommodityID(this.comboCommodity.Items[k].ToString())))
					{
						this.comboCommodity.SelectedIndex = k;
						if (this.radioB.Checked)
						{
							this.numericPrice.Value = (decimal)SellPrice;
							break;
						}
						this.numericPrice.Value = (decimal)BuyPrice;
						break;
					}
					else
					{
						k++;
					}
				}
			}
			this.refreshGN();
		}
		private void dgUnTrade_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void dgAllOrder_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void dgTrade_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void dgPreDelegate_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			Control activeControl = base.ActiveControl;
			if (activeControl is SplitContainer)
			{
				SplitContainer splitContainer = (SplitContainer)activeControl;
				activeControl = splitContainer.ActiveControl;
			}
			if (activeControl is ComboBox)
			{
				ComboBox comboBox = (ComboBox)activeControl;
				if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					comboBox.DropDownStyle = ComboBoxStyle.DropDown;
				}
				if (comboBox != this.comboCommodity)
				{
					this.AutoComplete(comboBox, e, true);
				}
			}
		}
		public void AutoComplete(ComboBox cb, KeyPressEventArgs e, bool blnLimitToList)
		{
			string text;
			if (e.KeyChar == '\b')
			{
				if (cb.SelectionStart < 1)
				{
					if (blnLimitToList)
					{
						e.Handled = blnLimitToList;
						return;
					}
					cb.Text = "";
					return;
				}
				else if (cb.SelectionLength == 0)
				{
					text = cb.Text.Substring(0, cb.Text.Length - 1);
				}
				else
				{
					text = cb.Text.Substring(0, cb.SelectionStart - 1);
				}
			}
			else if (cb.SelectionLength == 0)
			{
				text = cb.Text + e.KeyChar;
			}
			else
			{
				text = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
			}
			int num = cb.FindString(text);
			if (num == cb.SelectedIndex)
			{
				cb.SelectionStart = text.Length;
				cb.SelectionLength = cb.Text.Length - cb.SelectionStart;
				e.Handled = true;
				return;
			}
			if (num != -1)
			{
				cb.SelectedText = "";
				cb.SelectedIndex = num;
				cb.SelectionStart = text.Length;
				cb.SelectionLength = cb.Text.Length - cb.SelectionStart;
				e.Handled = true;
				return;
			}
			e.Handled = blnLimitToList;
		}
		private ArrayList getfilllist(string strvalue)
		{
			ArrayList arrayList = new ArrayList();
			int count = this.comboCommodity.Items.Count;
			int length = strvalue.Length;
			for (int i = 0; i < count; i++)
			{
				string text = this.comboCommodity.Items[i].ToString();
				if (text.Length >= length && string.Compare(text.Substring(0, length), strvalue, true) == 0)
				{
					arrayList.Add(text);
				}
			}
			return arrayList;
		}
		private void lbmain_click(object sender, EventArgs e)
		{
			if (this.lbmain.SelectedItems.Count == 0)
			{
				return;
			}
			string text = this.lbmain.SelectedItem.ToString();
			this.comboCommodity.Text = text;
			int num = this.comboCommodity.FindStringExact(text);
			if (num != -1)
			{
				this.comboCommodity.SelectedIndex = num;
			}
			this.lbmain.Visible = false;
		}
		private void lbmain_keydown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left || e.KeyCode == Keys.Prior)
			{
				if (this.lbmain.SelectedIndex > 0)
				{
					this.lbmain.SelectedIndex = this.lbmain.SelectedIndex - 1;
					return;
				}
			}
			else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next) && this.lbmain.SelectedIndex < this.lbmain.Items.Count - 1)
			{
				this.lbmain.SelectedIndex = this.lbmain.SelectedIndex + 1;
			}
		}
		private void comboCommodity_TextChanged(object sender, EventArgs e)
		{
			this.currentCommodity = this.GetCommodityID(this.comboCommodity.Text);
			this.numericPrice_Enter(null, null);
			this.isAutoPrice = true;
			this.refreshGN();
			if (this.comboCommodity.Text == "")
			{
				this.lbmain.Visible = false;
				return;
			}
			if (!this.comboCommodity.Focused)
			{
				this.lbmain.Visible = false;
				return;
			}
			if (!this.comboCommodity.Parent.Controls.Contains(this.lbmain))
			{
				this.lbmain.Width = this.comboCommodity.Width;
				this.lbmain.Height = 80;
				this.lbmain.Parent = this.comboCommodity.Parent;
				this.lbmain.Top = this.comboCommodity.Top + this.comboCommodity.Height + 1;
				this.lbmain.Left = this.comboCommodity.Left;
				this.comboCommodity.Parent.Controls.Add(this.lbmain);
				this.lbmain.BringToFront();
			}
			int selectionStart = this.comboCommodity.SelectionStart;
			if (selectionStart > this.comboCommodity.Text.Length)
			{
				return;
			}
			ArrayList arrayList = this.getfilllist(this.comboCommodity.Text.Substring(0, selectionStart));
			this.lbmain.Items.Clear();
			this.lbmain.Items.AddRange(arrayList.ToArray());
			if (this.lbmain.Items.Count > 0)
			{
				this.lbmain.SelectedIndex = 0;
			}
			if (!this.lbmain.Visible)
			{
				this.lbmain.Visible = true;
			}
		}
		private void comboCommodity_Leave(object sender, EventArgs e)
		{
			if (!this.lbmain.Focused)
			{
				this.lbmain.Visible = false;
			}
		}
		private void comboCommodity_DropDown(object sender, EventArgs e)
		{
			this.lbmain.Visible = false;
		}
		private void comboCommodity_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.lbmain.Visible)
			{
				if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next || e.KeyCode == Keys.Prior)
				{
					this.lbmain_keydown(this.lbmain, e);
					e.Handled = true;
					return;
				}
				if (e.KeyCode == Keys.Return)
				{
					this.lbmain_click(this.lbmain, e);
					e.Handled = true;
				}
			}
		}
		private void SetSkin()
		{
			this.groupBoxOrder.BackColor = Color.Transparent;
			this.groupBoxOrder.BackgroundImage = ControlLayout.SkinImage;
			this.groupBoxOrder.BackgroundImageLayout = ImageLayout.Stretch;
			this.groupBoxB_S.BackColor = Color.Transparent;
			this.radioB.BackColor = Color.Transparent;
			this.radioS.BackColor = Color.Transparent;
			this.groupBoxO_L.BackColor = Color.Transparent;
			this.radioO.BackColor = Color.Transparent;
			this.radioL.BackColor = Color.Transparent;
			this.gbCloseMode.BackColor = Color.Transparent;
			this.rbCloseT.BackColor = Color.Transparent;
			this.rbCloseH.BackColor = Color.Transparent;
			foreach (Control control in this.groupBoxOrder.Controls)
			{
				if (control is Label)
				{
					control.BackColor = Color.Transparent;
				}
			}
			this.groupBoxGNCommodit.BackColor = Color.Transparent;
			this.groupBoxGNCommodit.BackgroundImage = ControlLayout.SkinImage;
			this.groupBoxGNCommodit.BackgroundImageLayout = ImageLayout.Stretch;
			foreach (Control control2 in this.groupBoxGNCommodit.Controls)
			{
				if (control2 is Label)
				{
					control2.BackColor = Color.Transparent;
				}
			}
			if (this.tabMain.DrawMode == TabDrawMode.Normal)
			{
				this.tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
			}
			for (int i = 0; i < this.tabMain.TabCount; i++)
			{
				foreach (Control control3 in this.tabMain.TabPages[i].Controls)
				{
					this.tabMain.TabPages[i].BackColor = Color.Transparent;
					this.tabMain.TabPages[i].BackgroundImage = ControlLayout.SkinImage;
					this.tabMain.TabPages[i].BackgroundImageLayout = ImageLayout.Stretch;
					if (control3 is GroupBox)
					{
						control3.BackColor = Color.Transparent;
					}
				}
			}
			this.dgUnTrade.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgTradeOrder.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgAllOrder.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgTrade.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgTradeSum.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgHoldingCollect.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgHoldingDetail.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgPreDelegate.DefaultCellStyle = this.controlLayout.dataGridViewCellStyle;
			this.dgUnTrade.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgTradeOrder.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgAllOrder.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgTrade.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgTradeSum.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgHoldingCollect.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgHoldingDetail.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgPreDelegate.ColumnHeadersDefaultCellStyle = this.controlLayout.columnHeadersDefaultCellStyle;
			this.dgUnTrade.BackgroundColor = Color.Gainsboro;
			this.dgTradeOrder.BackgroundColor = Color.Gainsboro;
			this.dgAllOrder.BackgroundColor = Color.Gainsboro;
			this.dgTrade.BackgroundColor = Color.Gainsboro;
			this.dgTradeSum.BackgroundColor = Color.Gainsboro;
			this.dgHoldingCollect.BackgroundColor = Color.Gainsboro;
			this.dgHoldingDetail.BackgroundColor = Color.Gainsboro;
			this.dgPreDelegate.BackgroundColor = Color.Gainsboro;
		}
		private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
		{
			for (int i = 0; i < this.tabMain.TabCount; i++)
			{
				this.DrawTab(e.Graphics, this.tabMain.TabPages[i], i);
			}
		}
		internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
		{
			Rectangle tabRect = this.tabMain.GetTabRect(nIndex);
			RectangleF layoutRectangle = this.tabMain.GetTabRect(nIndex);
			bool flag = this.tabMain.SelectedIndex == nIndex;
			Point[] array = new Point[7];
			if (this.tabMain.Alignment == TabAlignment.Top)
			{
				array[0] = new Point(tabRect.Left, tabRect.Bottom);
				array[1] = new Point(tabRect.Left, tabRect.Top + 3);
				array[2] = new Point(tabRect.Left + 3, tabRect.Top);
				array[3] = new Point(tabRect.Right - 3, tabRect.Top);
				array[4] = new Point(tabRect.Right, tabRect.Top + 3);
				array[5] = new Point(tabRect.Right, tabRect.Bottom);
				array[6] = new Point(tabRect.Left, tabRect.Bottom);
			}
			else
			{
				array[0] = new Point(tabRect.Left, tabRect.Top);
				array[1] = new Point(tabRect.Right, tabRect.Top);
				array[2] = new Point(tabRect.Right, tabRect.Bottom - 3);
				array[3] = new Point(tabRect.Right - 3, tabRect.Bottom);
				array[4] = new Point(tabRect.Left + 3, tabRect.Bottom);
				array[5] = new Point(tabRect.Left, tabRect.Bottom - 3);
				array[6] = new Point(tabRect.Left, tabRect.Top);
			}
			Brush brush = new TextureBrush(ControlLayout.SkinImage);
			g.FillPolygon(brush, array);
			g.DrawPolygon(SystemPens.ButtonHighlight, array);
			if (flag)
			{
				Pen pen = new Pen(tabPage.BackColor);
				switch (this.tabMain.Alignment)
				{
				case TabAlignment.Top:
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Bottom, tabRect.Right - 1, tabRect.Bottom);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Bottom + 1, tabRect.Right - 1, tabRect.Bottom + 1);
					break;
				case TabAlignment.Bottom:
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top, tabRect.Right - 1, tabRect.Top);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top - 1, tabRect.Right - 1, tabRect.Top - 1);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top - 2, tabRect.Right - 1, tabRect.Top - 2);
					break;
				}
				pen.Dispose();
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			brush = new SolidBrush(tabPage.ForeColor);
			g.DrawString(tabPage.Text, this.Font, brush, layoutRectangle, stringFormat);
			brush.Dispose();
		}
		private void m_splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			int num = this.m_splitContainer.Panel2.Width - this.m_splitContainer.Panel2.AutoScrollMinSize.Width;
			if (num == 0)
			{
				return;
			}
			int num2;
			int num3;
			int num4;
			int num5;
			if (num < 0)
			{
				num2 = 0;
				num3 = 0;
				num4 = 0;
				num5 = 0;
			}
			else if (0 < num && num <= 30)
			{
				num2 = num;
				num3 = num;
				num4 = 0;
				num5 = 0;
			}
			else if (num > 30 && num <= 50)
			{
				num2 = num;
				num3 = 30;
				num4 = 0;
				num5 = 0;
			}
			else if (num > 50 && num <= 120)
			{
				num2 = 50;
				num3 = 30;
				num4 = num - 50;
				num5 = num - 50;
			}
			else
			{
				num2 = 50;
				num3 = 30;
				num4 = 70;
				num5 = num - 50;
			}
			if (this.controlLayout.groupBoxOrder_Width == 0)
			{
				return;
			}
			this.groupBoxOrder.Width = this.controlLayout.groupBoxOrder_Width + num2;
			this.comboMarKet.Width = this.controlLayout.comboMarKet_Width + num3;
			this.comboCommodity.Width = this.controlLayout.comboCommodity_Width + num3;
			this.comboTranc.Width = this.controlLayout.comboTranc_Width + num3;
			if (Global.CustomerCount < 2)
			{
				this.tbTranc.Width = this.comboCommodity.Width;
			}
			else
			{
				this.tbTranc.Width = this.controlLayout.comboTranc_Width + num3;
			}
			this.groupBoxB_S.Width = this.controlLayout.groupBoxB_S_Width + num3;
			this.radioS.Left = this.controlLayout.radioS_Left + num3;
			this.groupBoxO_L.Width = this.controlLayout.groupBoxO_L_Width + num3;
			this.radioL.Left = this.controlLayout.radioL_Left + num3;
			this.numericPrice.Width = this.controlLayout.numericPrice_Width + num3;
			this.numericQty.Width = this.controlLayout.numericQty_Width + num3;
			this.numericLPrice.Width = this.controlLayout.numericLPrice_Width + num3;
			this.buttonAddPre.Left = this.controlLayout.buttonAddPre_Left + num3;
			this.gbCloseMode.Width = this.controlLayout.groupBoxCloseMode_Width + num3;
			this.rbCloseH.Left = this.controlLayout.radioCloseH_Left + num3;
			this.groupBoxGNCommodit.Width = this.controlLayout.groupBoxGNCommodit_Width + num5;
			this.groupBoxGNCommodit.Left = this.controlLayout.groupBoxGNCommodit_Left + num2;
			this.butMinLine.Left = this.controlLayout.butMinLine_Left + num4;
		}
		private void MainForm_Shown(object sender, EventArgs e)
		{
		}
		private void radioO_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioO.Checked)
			{
				this.labelLPrice.Visible = false;
				this.numericLPrice.Visible = false;
				this.gbCloseMode.Visible = false;
			}
		}
		private void labPrice_DoubleClick(object sender, EventArgs e)
		{
			if (!IniData.GetInstance().SetDoubleClick)
			{
				return;
			}
			if (IniData.GetInstance().AutoAddBSPQ)
			{
				this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice2");
			}
			else
			{
				this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice1");
			}
			IniData.GetInstance().AutoAddBSPQ = !IniData.GetInstance().AutoAddBSPQ;
			IniFile iniFile = new IniFile(Global.ConfigPath + "ISSUE_Trade.ini");
			iniFile.IniWriteValue("Set", "AutoAddBSPQ", IniData.GetInstance().AutoAddBSPQ.ToString());
		}
		private void butConditionOrder_Click(object sender, EventArgs e)
		{
			if (Global.HTConfig.ContainsKey("conditionOrder") && Global.HTConfig["conditionOrder"] != null)
			{
				this.conditionOrder = (ConditionOrder)Global.HTConfig["conditionOrder"];
				this.conditionOrder.WindowState = FormWindowState.Normal;
				this.conditionOrder.Activate();
				return;
			}
			string text = "/conditionOrder/conditionOrder.do?method=conditionOrderQuery&SendStatus=0";
			text = Global.TradeLibrary.CommunicationUrl.Replace("\\httpXmlServlet", text);
			this.conditionOrder = new ConditionOrder(text);
			this.conditionOrder.Show();
			Global.HTConfig.Add("conditionOrder", this.conditionOrder);
		}
		private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
		{
			this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
		}
		private void radioS_CheckedChanged(object sender, EventArgs e)
		{
			this.setRadioEnable(this.GetCommodityID(this.comboCommodity.Text.ToString()));
		}
		private void numericPrice_Click(object sender, EventArgs e)
		{
			this.numericPrice.Select(0, this.numericPrice.Text.Length);
		}
		private void numericQty_Click(object sender, EventArgs e)
		{
			this.numericQty.Select(0, this.numericQty.Text.Length);
		}
		private void comboCommodity_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.ChangeKeyChar(e.KeyChar);
			}
		}
		private void numericPrice_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b' && e.KeyChar != '.');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.ChangeKeyChar(e.KeyChar);
			}
		}
		private void numericQty_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.ChangeKeyChar(e.KeyChar);
			}
		}
		public char ChangeKeyChar(char c)
		{
			switch (c)
			{
			case '０':
				return '0';
			case '１':
				return '1';
			case '２':
				return '2';
			case '３':
				return '3';
			case '４':
				return '4';
			case '５':
				return '5';
			case '６':
				return '6';
			case '７':
				return '7';
			case '８':
				return '8';
			case '９':
				return '9';
			default:
				return '9';
			}
		}
		private void lbSplitterHide_Click(object sender, EventArgs e)
		{
			if (this.LockFormEvent != null)
			{
				this.LockFormEvent(sender, e);
			}
		}
		private void labelSP1_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelSP1.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "卖一价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelSP2_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelSP2.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "卖二价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelSP3_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelSP3.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "卖三价传入出错:" + ex.Message + ex.StackTrace);
			}
		}
		private void labelBP1_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP1.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "买一价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelBP2_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP2.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "买二价穿入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelBP3_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelBP3.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "买三价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelLimitUpV_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelLimitUpV.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "涨停价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void labelLimitDownV_Click(object sender, EventArgs e)
		{
			try
			{
				this.numericPrice.Value = this.ToDecimal(this.labelLimitDownV.Text);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "跌停价传入出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void InvestorInfoLoad()
		{
			this.lstVInvestorFill = new MainForm.StringObjCallback(this.lstInvestorFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryInvestorInfo);
			ThreadPool.QueueUserWorkItem(callBack, null);
		}
		private void QueryInvestorInfo(object userid)
		{
			InvestorInfoResponseVO investorInfo = this.dataProcess.GetInvestorInfo();
			this.HandleCreated();
			base.Invoke(this.lstVInvestorFill, new object[]
			{
				investorInfo
			});
		}
		private void lstInvestorFill(object _investorinfoResponseVO)
		{
			InvestorInfoResponseVO investorInfoResponseVO = (InvestorInfoResponseVO)_investorinfoResponseVO;
			string text = string.Empty;
			text = investorInfoResponseVO.Account;
			this.listVInvestor.Clear();
			try
			{
				string[] array = this.investorItemInfo.m_strItems.Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					this.listVInvestor.Columns.Add("项目", 140, HorizontalAlignment.Left);
					this.listVInvestor.Columns.Add("项目值", 190, HorizontalAlignment.Right);
					this.groupBoxInvestor.Width = 354 * (i + 1);
					string[] array2 = array[i].Split(new char[]
					{
						';'
					});
					for (int j = 0; j < array2.Length; j++)
					{
						ListViewItem listViewItem = null;
						if (j < this.listVInvestor.Items.Count)
						{
							listViewItem = this.listVInvestor.Items[j];
						}
						if (array2[j].Equals("Account"))
						{
							ColItemInfo colItemInfo = (ColItemInfo)this.investorItemInfo.m_htItemInfo["Account"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo.name);
								this.listVInvestor.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo.name);
							}
							listViewItem.SubItems.Add(text);
						}
						if (array2[j].Equals("Name"))
						{
							ColItemInfo colItemInfo2 = (ColItemInfo)this.investorItemInfo.m_htItemInfo["Name"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo2.name);
								this.listVInvestor.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo2.name);
							}
							listViewItem.SubItems.Add(investorInfoResponseVO.Name);
						}
						if (array2[j].Equals("Bank"))
						{
							ColItemInfo colItemInfo3 = (ColItemInfo)this.investorItemInfo.m_htItemInfo["Bank"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo3.name);
								this.listVInvestor.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo3.name);
							}
							listViewItem.SubItems.Add(investorInfoResponseVO.Bank);
						}
						if (array2[j].Equals("Phone"))
						{
							ColItemInfo colItemInfo4 = (ColItemInfo)this.investorItemInfo.m_htItemInfo["Phone"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo4.name);
								this.listVInvestor.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo4.name);
							}
							listViewItem.SubItems.Add(investorInfoResponseVO.Phone);
						}
						if (array2[j].Equals("IDNum"))
						{
							ColItemInfo colItemInfo5 = (ColItemInfo)this.investorItemInfo.m_htItemInfo["IDNum"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo5.name);
								this.listVInvestor.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo5.name);
							}
							listViewItem.SubItems.Add(investorInfoResponseVO.IDNum);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "查询投资人信息错误：" + ex.Message);
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void PreDelegateLoad()
		{
			try
			{
				this.dgPreDelegateRefresh();
				for (int i = 0; i < this.dgPreDelegate.Columns.Count; i++)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.preOrderItemInfo.m_htItemInfo[this.dgPreDelegate.Columns[i].Name];
					if (colItemInfo != null)
					{
						this.dgPreDelegate.Columns[i].HeaderText = colItemInfo.name;
						this.dgPreDelegate.Columns[i].MinimumWidth = colItemInfo.width;
						this.dgPreDelegate.Columns[i].FillWeight = (float)colItemInfo.width;
						this.dgPreDelegate.Columns[i].DefaultCellStyle.Format = colItemInfo.format;
						if (colItemInfo.sortID == 1)
						{
							this.dgPreDelegate.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
						}
						else
						{
							this.dgPreDelegate.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						if (!this.preOrderItemInfo.m_strItems.Contains(this.dgPreDelegate.Columns[i].Name))
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
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
			}
		}
		private void comboLoad()
		{
			this.comCommodity.Items.Clear();
			this.comTranc.Items.Clear();
			this.comCommodity.Items.Add("全部");
			this.comTranc.Items.Add("全部");
			this.comCommodity.SelectedIndex = 0;
			this.comTranc.SelectedIndex = 0;
			foreach (DataRow dataRow in MainForm.dsPreDelegate.Tables[0].Rows)
			{
				bool flag = true;
				bool flag2 = true;
				for (int i = 0; i < this.comCommodity.Items.Count; i++)
				{
					if (Global.MarketHT.Count > 1 && this.queryMarketID != null && this.queryMarketID.Length > 0 && !dataRow["MarKet"].ToString().Equals(this.queryMarketID))
					{
						flag = false;
						break;
					}
					if (dataRow["commodityCode"].ToString().Equals(this.comCommodity.Items[i].ToString()))
					{
						flag = false;
					}
				}
				for (int j = 0; j < this.comTranc.Items.Count; j++)
				{
					if (dataRow["TransactionsCode"].ToString().Equals(this.comTranc.Items[j].ToString()))
					{
						flag2 = false;
					}
				}
				if (flag)
				{
					this.comCommodity.Items.Add(dataRow["commodityCode"].ToString());
				}
				if (flag2)
				{
					this.comTranc.Items.Add(dataRow["TransactionsCode"].ToString());
				}
			}
		}
		private void dgPreDelegateRefresh()
		{
			MainForm.dsPreDelegate = MainForm.XmlPreDelegate.GetDataSetByXml();
			if (Global.MarketHT.Count > 1)
			{
				string text = string.Empty;
				if (this.queryMarketID != null && this.queryMarketID.Length > 0)
				{
					text = text + " MarKet= '" + this.queryMarketID + "'";
				}
				DataTable dataSource = MainForm.XmlPreDelegate.GetDataViewByXml(text, " ID asc").ToTable();
				this.dgPreDelegate.DataSource = dataSource;
			}
			else
			{
				this.dgPreDelegate.DataSource = MainForm.dsPreDelegate.Tables[0];
			}
			this.comboLoad();
			this.DataViewAddQueryF8Sum(this.dgPreDelegate);
		}
		private void selAll_Click(object sender, EventArgs e)
		{
			if (this.selAll.Text.Equals("全选"))
			{
				for (int i = 0; i < this.dgPreDelegate.Rows.Count; i++)
				{
					if (!(this.dgPreDelegate.Rows[i].Cells["TransactionsCode"].Value.ToString() == "合计"))
					{
						this.dgPreDelegate.Rows[i].Cells[0].Value = true;
					}
				}
				this.selAll.Text = "全不选";
				return;
			}
			for (int j = 0; j < this.dgPreDelegate.Rows.Count; j++)
			{
				this.dgPreDelegate.Rows[j].Cells[0].Value = false;
			}
			this.selAll.Text = "全选";
		}
		private void comCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void comTranc_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void buttonSel_Click(object sender, EventArgs e)
		{
			this.QueryF8();
		}
		private void QueryF8()
		{
			try
			{
				string text = " 1=1 ";
				if (Global.MarketHT.Count > 1 && this.queryMarketID != null && this.queryMarketID.Length > 0)
				{
					text = text + " and MarKet= '" + this.queryMarketID + "'";
				}
				if (this.comTranc.SelectedIndex != 0)
				{
					text = text + " and TransactionsCode= '" + this.comTranc.Text + "'";
				}
				if (this.comCommodity.SelectedIndex != 0)
				{
					text = text + " and  commodityCode = '" + this.comCommodity.Text + "'";
				}
				this.dgPreDelegate.DataSource = MainForm.XmlPreDelegate.GetDataViewByXml(text, " ID asc").ToTable();
				this.DataViewAddQueryF8Sum(this.dgPreDelegate);
				this.selAll.Text = "全选";
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "查询预埋委托信息错误：" + ex.Message);
			}
		}
		private void buttonDel_Click(object sender, EventArgs e)
		{
			this.DeletePreDelegate();
		}
		private void DeletePreDelegate()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = this.dgPreDelegate.Rows.Count - 1; i >= 0; i--)
			{
				if (this.dgPreDelegate["SelectFlag", i].Value != null && (bool)this.dgPreDelegate["SelectFlag", i].Value)
				{
					text = text + this.dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
				}
			}
			if (!text.Equals(""))
			{
				MessageForm messageForm = new MessageForm("删除预埋委托信息提示", "确定要删除选定的预埋委托单吗？", 0);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					text = text.Remove(text.Length - 1);
					string[] columnValue = text.Split(new char[]
					{
						'_'
					});
					string text3 = MainForm.XmlPreDelegate.DeleteXmlRows("id", columnValue);
					if (text3.Equals("true"))
					{
						text2 = "删除成功！";
					}
					else
					{
						text2 = "删除失败！";
					}
					this.dgPreDelegateRefresh();
				}
			}
			else
			{
				text2 = "请至少选择一条记录！";
			}
			if (!text2.Equals(""))
			{
				MessageForm messageForm = new MessageForm("删除预埋委托结果", text2, 1);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
		private void buttonOrder6_Click(object sender, EventArgs e)
		{
			ArrayList arrayList = new ArrayList();
			string arg_0B_0 = string.Empty;
			string text = string.Empty;
			for (int i = 0; i <= this.dgPreDelegate.Rows.Count - 1; i++)
			{
				if (this.dgPreDelegate["SelectFlag", i].Value != null && (bool)this.dgPreDelegate["SelectFlag", i].Value)
				{
					arrayList.Add(new OrderRequestVO
					{
						UserID = Global.UserID,
						CustomerID = this.dgPreDelegate["TransactionsCode", i].Value.ToString(),
						BuySell = Global.StrToShort(Global.BuySellStrArr, this.dgPreDelegate["B_S", i].Value.ToString()),
						MarketID = this.dgPreDelegate["MarKet", i].Value.ToString(),
						CommodityID = this.dgPreDelegate["commodityCode", i].Value.ToString(),
						Price = Tools.StrToDouble(this.dgPreDelegate["price", i].Value.ToString()),
						Quantity = Tools.StrToLong(this.dgPreDelegate["qty", i].Value.ToString()),
						SettleBasis = Global.StrToShort(Global.SettleBasisStrArr, this.dgPreDelegate["O_L", i].Value.ToString()),
						LPrice = Tools.StrToDouble(this.dgPreDelegate["LPrice", i].Value.ToString()),
						CloseMode = Tools.StrToShort(this.dgPreDelegate["CloseMode", i].Value.ToString()),
						TimeFlag = Tools.StrToShort(this.dgPreDelegate["TimeFlag", i].Value.ToString()),
						BillType = 0
					});
					text = text + this.dgPreDelegate.Rows[i].Cells[1].Value.ToString() + "_";
				}
			}
			if (!text.Equals(""))
			{
				MessageForm messageForm = new MessageForm("委托操作", "确定提交所选预埋委托吗？", 0);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					this.EnableControls(false, "数据提交中");
					this.buttonOrder6.Enabled = false;
					text = text.Remove(text.Length - 1);
					this.idColumns = text.Split(new char[]
					{
						'_'
					});
					WaitCallback callBack = new WaitCallback(this.Order6);
					ThreadPool.QueueUserWorkItem(callBack, arrayList);
					return;
				}
			}
			else
			{
				MessageForm messageForm = new MessageForm("委托", "请至少选择一条记录！", 1);
				messageForm.Owner = base.ParentForm;
				messageForm.ShowDialog();
				messageForm.Dispose();
			}
		}
		private void Order6(object _orderArr)
		{
			ArrayList arrayList = (ArrayList)_orderArr;
			ResponseVO[] array = new ResponseVO[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				OrderRequestVO req = (OrderRequestVO)arrayList[i];
				array[i] = this.dataProcess.Order(req);
			}
			if (arrayList.Count > 0)
			{
				MainForm.ResponseVOArrCallback method = new MainForm.ResponseVOArrCallback(this.OrderMessage6);
				this.HandleCreated();
				base.Invoke(method, new object[]
				{
					array
				});
			}
		}
		private void OrderMessage6(ResponseVO[] responseVOArr)
		{
			bool flag = true;
			string text = string.Empty;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < responseVOArr.Length; i++)
			{
				ResponseVO responseVO = responseVOArr[i];
				if (responseVO.RetCode != 0L)
				{
					flag = false;
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"[预埋委托单",
						this.idColumns[i],
						"：",
						responseVO.RetMessage.Trim(),
						"]"
					});
				}
				else
				{
					arrayList.Add(this.idColumns[i]);
				}
			}
			if (!flag)
			{
				this.FillInfoText(text, Global.ErrorColor, this.displayInfo);
			}
			this.EnableControls(true, "数据提交完毕");
			this.buttonOrder6.Enabled = true;
			this.idColumns = null;
			if (arrayList.Count > 0)
			{
				MainForm.XmlPreDelegate.DeleteXmlRows("id", (string[])arrayList.ToArray(typeof(string)));
				this.dgPreDelegateRefresh();
				this.refreshFlag = true;
			}
		}
		private void dgPreDelegate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			for (int i = 0; i < this.dgPreDelegate.Rows.Count; i++)
			{
				if (this.dgPreDelegate.Rows[i].Cells["TransactionsCode"].Value.ToString().Trim() == "合计")
				{
					this.dgPreDelegate.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
					this.dgPreDelegate.Rows[i].ReadOnly = true;
				}
			}
		}
		private void DataViewAddQueryF8Sum(DataGridView dataGridView)
		{
			if (dataGridView.DataSource == null)
			{
				return;
			}
			DataTable dataTable = (DataTable)dataGridView.DataSource;
			if (dataTable.Rows.Count < 2)
			{
				return;
			}
			dataTable.Columns.Add("AutoID");
			int num = 0;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				num += int.Parse(dataRow["Qty"].ToString());
				dataRow["AutoId"] = "1";
			}
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["TransactionsCode"] = "合计";
			dataRow2["CommodityCode"] = "共" + dataTable.Rows.Count + "条";
			dataRow2["Qty"] = num;
			dataRow2["AutoID"] = "2";
			dataTable.Rows.Add(dataRow2);
			this.dgPreDelegate.Columns["AutoId"].Visible = false;
		}
		private void dgPreDelegate_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				return;
			}
			DataTable dataTable = (DataTable)this.dgPreDelegate.DataSource;
			try
			{
				dataTable.DefaultView.Sort = " AutoID ASC, " + this.dgPreDelegate.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				dataTable.DefaultView.Sort = " " + this.dgPreDelegate.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.m_order = " Desc ";
				}
				else
				{
					this.m_order = " ASC ";
				}
			}
		}
		private void dgPreDelegate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1 && this.dgPreDelegate.Rows[e.RowIndex].Cells["TransactionsCode"].Value.ToString() != "合计")
			{
				string text = string.Empty;
				string text2 = string.Empty;
				text = this.dgPreDelegate.Rows[e.RowIndex].Cells[1].Value.ToString();
				if (!text.Equals(""))
				{
					MessageForm messageForm = new MessageForm("删除预埋委托信息提示", "确定要删除选定的预埋委托单吗？", 0);
					messageForm.Owner = base.ParentForm;
					messageForm.ShowDialog();
					messageForm.Dispose();
					if (messageForm.isOK)
					{
						string[] columnValue = new string[]
						{
							text
						};
						string text3 = MainForm.XmlPreDelegate.DeleteXmlRows("id", columnValue);
						if (text3.Equals("true"))
						{
							text2 = "删除成功！";
						}
						else
						{
							text2 = "删除失败！";
						}
						this.dgPreDelegateRefresh();
					}
				}
				else
				{
					text2 = "请至少选择一条记录！";
				}
				if (!text2.Equals(""))
				{
					MessageForm messageForm = new MessageForm("删除预埋委托结果", text2, 1);
					messageForm.Owner = base.ParentForm;
					messageForm.ShowDialog();
					messageForm.Dispose();
				}
			}
		}
		private void QueryFundsInfoLoad()
		{
			this.lstVFundsFill = new MainForm.StringObjCallback(this.LstVFundsFill);
			this.EnableControls(false, "数据查询中");
			WaitCallback callBack = new WaitCallback(this.QueryFundsInfo);
			ThreadPool.QueueUserWorkItem(callBack, null);
		}
		private void QueryFundsInfo(object _commodityID)
		{
			FirmInfoResponseVO firmInfoResponseVO = this.dataProcess.QueryFundsInfo();
			this.HandleCreated();
			base.Invoke(this.lstVFundsFill, new object[]
			{
				firmInfoResponseVO
			});
		}
		private void LstVFundsFill(object _firmInfoResponseVO)
		{
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(1, 15);
			this.lstVFunds.SmallImageList = imageList;
			FirmInfoResponseVO firmInfoResponseVO = (FirmInfoResponseVO)_firmInfoResponseVO;
			string text = firmInfoResponseVO.FirmID + "(" + firmInfoResponseVO.FirmName.Trim() + ")";
			this.lstVFunds.Clear();
			try
			{
				string[] array = this.fundsItemInfo.m_strItems.Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					this.lstVFunds.Columns.Add("项目", 140, HorizontalAlignment.Left);
					this.lstVFunds.Columns.Add("项目值", 190, HorizontalAlignment.Right);
					this.groupBoxMoney.Width = 354 * (i + 1);
					string[] array2 = array[i].Split(new char[]
					{
						';'
					});
					for (int j = 0; j < array2.Length; j++)
					{
						ListViewItem listViewItem = null;
						if (j < this.lstVFunds.Items.Count)
						{
							listViewItem = this.lstVFunds.Items[j];
						}
						if (array2[j].Equals("null"))
						{
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem("");
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add("");
							}
							listViewItem.SubItems.Add("");
						}
						if (array2[j].Equals("FirmID"))
						{
							ColItemInfo colItemInfo = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FirmID"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo.name);
							}
							listViewItem.SubItems.Add(text);
						}
						if (array2[j].Equals("FirmTpye"))
						{
							ColItemInfo colItemInfo2 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FirmTpye"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo2.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo2.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.FirmType.ToString(colItemInfo2.format));
						}
						if (array2[j].Equals("InitFund"))
						{
							ColItemInfo colItemInfo3 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["InitFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo3.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo3.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.InitFund.ToString(colItemInfo3.format));
						}
						if (array2[j].Equals("InFund"))
						{
							ColItemInfo colItemInfo4 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["InFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo4.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo4.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.InFund.ToString(colItemInfo4.format));
						}
						if (array2[j].Equals("OutFund"))
						{
							ColItemInfo colItemInfo5 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OutFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo5.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo5.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OutFund.ToString(colItemInfo5.format));
						}
						if (array2[j].Equals("HKSell"))
						{
							ColItemInfo colItemInfo6 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["HKSell"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo6.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo6.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.HKSell.ToString(colItemInfo6.format));
						}
						if (array2[j].Equals("HKSellMinus"))
						{
							ColItemInfo colItemInfo7 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["HKSellMinus"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo7.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo7.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.HKSell.ToString(colItemInfo7.format));
						}
						if (array2[j].Equals("HKBuy"))
						{
							ColItemInfo colItemInfo8 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["HKBuy"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo8.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo8.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.HKBuy.ToString(colItemInfo8.format));
						}
						if (array2[j].Equals("CurFreezeFund"))
						{
							ColItemInfo colItemInfo9 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["CurFreezeFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo9.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo9.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurFreezeFund.ToString(colItemInfo9.format));
						}
						if (array2[j].Equals("CurFreezeFundMinus"))
						{
							ColItemInfo colItemInfo10 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["CurFreezeFundMinus"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo10.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo10.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurFreezeFund.ToString(colItemInfo10.format));
						}
						if (array2[j].Equals("CurUnfreezeFund"))
						{
							ColItemInfo colItemInfo11 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["CurUnfreezeFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo11.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo11.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurUnfreezeFund.ToString(colItemInfo11.format));
						}
						if (array2[j].Equals("IssuanceFee"))
						{
							ColItemInfo colItemInfo12 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["IssuanceFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo12.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo12.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.IssuanceFee.ToString(colItemInfo12.format));
						}
						if (array2[j].Equals("SGFreezeFund"))
						{
							ColItemInfo colItemInfo13 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["SGFreezeFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo13.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo13.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.SGFreezeFund.ToString(colItemInfo13.format));
						}
						if (array2[j].Equals("OrderFrozenFund"))
						{
							ColItemInfo colItemInfo14 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OrderFrozenFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo14.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo14.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OrderFrozenFund.ToString(colItemInfo14.format));
						}
						if (array2[j].Equals("OrderFrozenFundMinus"))
						{
							ColItemInfo colItemInfo15 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OrderFrozenFundMinus"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo15.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo15.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OrderFrozenFund.ToString(colItemInfo15.format));
						}
						if (array2[j].Equals("OtherFrozenFund"))
						{
							ColItemInfo colItemInfo16 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OtherFrozenFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo16.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo16.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OtherFrozenFund.ToString(colItemInfo16.format));
						}
						if (array2[j].Equals("Fee"))
						{
							ColItemInfo colItemInfo17 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Fee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo17.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo17.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.Fee.ToString(colItemInfo17.format));
						}
						if (array2[j].Equals("WareHouseRegFee"))
						{
							ColItemInfo colItemInfo18 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["WareHouseRegFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo18.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo18.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.WareHouseRegFee.ToString(colItemInfo18.format));
						}
						if (array2[j].Equals("WareHouseCancelFee"))
						{
							ColItemInfo colItemInfo19 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["WareHouseCancelFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo19.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo19.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.WareHouseCancelFee.ToString(colItemInfo19.format));
						}
						if (array2[j].Equals("TransferFee"))
						{
							ColItemInfo colItemInfo20 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["TransferFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo20.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo20.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.TransferFee.ToString(colItemInfo20.format));
						}
						if (array2[j].Equals("DistributionFee"))
						{
							ColItemInfo colItemInfo21 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["DistributionFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo21.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo21.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.DistributionFee.ToString(colItemInfo21.format));
						}
						if (array2[j].Equals("OtherFee"))
						{
							ColItemInfo colItemInfo22 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OtherFee"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo22.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo22.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OtherFee.ToString(colItemInfo22.format));
						}
						if (array2[j].Equals("OtherChange"))
						{
							ColItemInfo colItemInfo23 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["OtherChange"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo23.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo23.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OtherChange.ToString(colItemInfo23.format));
						}
						if (array2[j].Equals("MarketValue"))
						{
							ColItemInfo colItemInfo24 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["MarketValue"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo24.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo24.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.MarketValue.ToString(colItemInfo24.format));
						}
						if (array2[j].Equals("UsableFund"))
						{
							ColItemInfo colItemInfo25 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["UsableFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo25.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo25.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.UsableFund.ToString(colItemInfo25.format));
						}
						if (array2[j].Equals("UsableFundAdd"))
						{
							ColItemInfo colItemInfo26 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["UsableFundAdd"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo26.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo26.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.UsableFund.ToString(colItemInfo26.format));
						}
						if (array2[j].Equals("DesirableFund"))
						{
							ColItemInfo colItemInfo27 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["DesirableFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo27.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo27.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.DesirableFund.ToString(colItemInfo27.format));
						}
						if (array2[j].Equals("CurrentRight"))
						{
							ColItemInfo colItemInfo28 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["CurrentRight"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo28.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo28.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurrentRight.ToString(colItemInfo28.format));
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.EnableControls(true, "数据查询完毕");
		}
		private void buttonFundsTransfer_Click(object sender, EventArgs e)
		{
			if (Global.HtForm["fundsTransfer"] == null)
			{
				this.fundsTransfer = new FundsTransfer((string)Global.HTConfig["AddressTransfer"]);
				this.fundsTransfer.Show();
				Global.HtForm.Add("fundsTransfer", this.fundsTransfer);
				return;
			}
			this.fundsTransfer = (FundsTransfer)Global.HtForm["fundsTransfer"];
			this.fundsTransfer.WindowState = FormWindowState.Normal;
			this.fundsTransfer.Activate();
		}
	}
}
