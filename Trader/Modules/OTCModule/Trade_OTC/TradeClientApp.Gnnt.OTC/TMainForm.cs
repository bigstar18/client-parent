using PluginInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using ToolsLibrary.Notifier;
using ToolsLibrary.util;
using TPME.Log;
using TradeClientApp.Gnnt.OTC.Library;
using TradeClientApp.Gnnt.OTC.ToolsBar;
using TradeControlLib.Gnnt.OTC;
using TradeInterface.Gnnt.OTC.DataVO;
using TradeInterface.Gnnt.OTC.Interface;
namespace TradeClientApp.Gnnt.OTC
{
	public class TMainForm : Form
	{
		private delegate void CallbackFirmFundsF10DataGrid(DataSet dt);
		private delegate void CallbackUpdateMemberFundPrice();
		private delegate void CallbackYuJing();
		private delegate void CallbackYuJingInfo();
		private delegate void CallbackCommodityInfoF8DataGrid(DataSet dataTable);
		private delegate void CallbackFirmInfoF7DataGrid(DataSet dataTable);
		private delegate void CallbackHoldingInfoF6DataGrid(DataSet dataTable);
		private delegate void InvokeFillHoldingInfoFloatingPriceEvent();
		private delegate void CallbackHoldingF5DataGrid(DataSet dataTable);
		private delegate void CallbackUpDataHoldingDetailInfoF5HQ();
		public delegate void SetToolsBarEnableCallBack(bool enable);
		private delegate void UpdataTimerInfo(string Text);
		private delegate void UpdataCurrentSystemStatus(string Text);
		private delegate void Checkstatus(object sender, EventArgs e);
		public delegate void EventPlayMessage();
		private delegate void LockTmain(bool Type);
		private delegate void FillHQDataGrid(object dataTable);
		private delegate void UpdataEvent();
		private delegate void UpdataHDIEvent(bool bset);
		private delegate void FillHQCtrl(object obj);
		public delegate void RefreshHQHanlder();
		public delegate void InitDataMainForm(string str, int val);
		public delegate void EventAgencyLogOut();
		public delegate void InitTradeCtrlMenuEnabled(bool flag);
		private delegate void Lock(bool bLock);
		public delegate void ReLoadHanlder();
		private delegate void StringObjCallback(object _commodityItem);
		public delegate void LogOut();
		public delegate void LockTree(bool Lock);
		private delegate void CallbackTradeInfoF4DataGrid(DataSet dataTable);
		private delegate void CallbackOrderInfoF3DataGrid(DataSet dataTable);
		private delegate void ResponseVOWithdrawOrderCallback(ResponseVO resultMessage);
		private delegate void CallbackHoldingDataGrid(DataSet dataTable);
		private delegate void CallbackOrderInfoDataGrid(DataSet dataTable);
		public delegate void CallbackFirmInfoDataGrid(DataSet dataTable);
		public delegate void UpdateFirmInfoCallBack(double floatingPT);
		private delegate void FillFirmInfoCallBack(DataSet firmInfo);
		private delegate void CallbackUpDataFirmInfo(double dataTable);
		private delegate void CallbackUpDataFirmInfoF7(double dataTable);
		private delegate void CallbackUpDataHoldingDetailInfoHQ();
		private delegate void ResponseVOWithdrawLossProfitCallback(ResponseVO resultMessage);
		private delegate void CallbackCustomerOrderF10DataGrid(DataSet dataTable);
		private delegate void CallbackUpdateCustomerOrderF10HQ();
		public const int SND_FILENAME = 131072;
		public const int SND_ASYNC = 1;
		private const int SPI_GETWORKAREA = 48;
		private const int TradeCtrl_m_iHeight = 119;
		private const int TradeCtrl_m_iWidth = 170;
		private const double textBoxPwdXRatio = 0.18668831168831168;
		private const double textBoxPwdYRatio = 0.3505154550075531;
		private const double textBoxPwdWidthRatio = 0.27228525280952454;
		private const double textBoxPwdHeightRatio = 0.10824742168188095;
		private const double buttonUnLockXRatio = 0.18638573586940765;
		private const double buttonUnLockYRatio = 0.55154639482498169;
		private const double buttonUnLockWidthRatio = 0.235008105635643;
		private const double buttonUnLockHeightRatio = 0.1340206116437912;
		private const double labelPwdX = 0.0892857164144516;
		private const double labelPwdY = 0.35023042559623718;
		private const double labelPwdInfoX = 0.47889611124992371;
		private const double labelPwdInfoY = 0.35944700241088867;
		private IContainer components;
		private SplitContainer splitContainer1;
		private SplitContainer splitContainerHQ;
		private Label HqTitle;
		private StatusStrip statusInfo;
		private ToolStripStatusLabel info;
		private ToolStripStatusLabel user;
		private ToolStripStatusLabel status;
		private ToolStripStatusLabel time;
		private TabControl tabTMain;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TabPage tabPage3;
		private TabPage tabPage4;
		private TabPage tabPage5;
		private TabPage tabPage6;
		private TabPage tabPage7;
		private TabPage tabPage8;
		private TabPage tabPage9;
		private HelpProvider helpProvider;
		private GroupBox gbHoldingDetailInfo;
		private GroupBox gbFirmInfo;
		private GroupBox gbOrderInfo;
		private DataGridView dgvHoldingDetailInfo;
		private DataGridView dgvOrderInfo;
		private GroupBox gbOrderInfoF3;
		private DataGridView dataGridView2;
		private DataGridView dataGridView1;
		private GroupBox gbTradeInfo;
		private DataGridView dvgTradeInfo;
		private DataGridView dataGridView3;
		private GroupBox gbHoldingDetailInfoF5;
		private GroupBox gbHoldingInfoF6;
		private DataGridView dgvHoldingInfoF6;
		private Label MessageInfo;
		private GroupBox gbqFirmInfo;
		private DataGridView dgvqFirmInfo;
		private DataGridView dgvFirmInfo;
		private ContextMenuStrip contextMenuStripHoldingDetail;
		private ToolStripMenuItem toolStripMenuItemSP;
		private ToolStripMenuItem toolStripMenuItemXP;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem toolStripMenuItemStopLoss;
		private ToolStripMenuItem toolStripMenuItemStopProfit;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem toolStripMenuItemCancel;
		private ToolStripMenuItem toolStripMenuItemRefresh;
		private ToolStripSeparator toolStripSeparator4;
		private SplitContainer splitContainer2;
		private SplitContainer splitContainer3;
		private GroupBox gbCommodityInfo;
		private DataGridView dgvCommodityInfo;
		private Label LableCommodity;
		private ComboBox cbCommodityInfo;
		private ContextMenuStrip contextMenuStripHQ;
		private ToolStripMenuItem toolStripMenuItemSO;
		private ToolStripMenuItem toolStripMenuItemSC;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripMenuItem toolStripMenuItemXO;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripMenuItem toolStripMenuItemHQCancel;
		private ContextMenuStrip contextMenuStripXJ;
		private ToolStripMenuItem toolStripMenuItemWithdrawOrder;
		private ToolStripSeparator toolStripSeparator7;
		private ToolStripMenuItem toolStripMenuItemXJCancel;
		private ToolStripMenuItem toolStripMenuItemXJRefresh;
		private ToolStripSeparator toolStripSeparator8;
		private DataGridView dgvOrderInfoF3;
		private DataGridView dgvHoldingDetailInfoF5;
		private Label lbBuySellF4;
		private Label lbSettleBasisF4F4;
		private Label lbCommodityF4;
		private Button btSelectF4;
		private ComboBox cbBuySellF4;
		private ComboBox cbCommodityF4;
		private ComboBox cbSettleBasisF4;
		private Label lbBuySell;
		private Label lbStatic;
		private Label lbOrderInfoType;
		private Label lbCommodity;
		private Button Selectbt;
		private ComboBox cbBuySell;
		private ComboBox cbOrderInfoType;
		private ComboBox cbCommodity;
		private ComboBox cbStatic;
		private GroupBox groupBox1;
		private DataGridView dvgYJInfoF9;
		private ContextMenuStrip contextMenuStripF9YJ;
		private ToolStripMenuItem NEWToolStripMenuItemYJF9;
		private ToolStripMenuItem MODToolStripMenuItemYJF9;
		private ToolStripMenuItem DELToolStripMenuItemYJF9;
		private ToolStripMenuItem ONOFFToolStripMenuItemYJF9;
		private ToolStripSeparator toolStripMenuItem2;
		private Panel panelLock;
		private Label labelPwdInfo;
		private Button buttonUnLock;
		private Label labelPwd;
		private TextBox textBoxPwd;
		private ContextMenuStrip contextMenuStripFirmInfo;
		private ToolStripMenuItem toolStripMenuItemFirmInfoRefresh;
		private GroupBox gbCustomerOrderF10;
		private DataGridView dgvCustomerOrderF10;
		private SplitContainer splitContainer4;
		private SplitContainer splitContainer5;
		private SplitContainer splitContainer6;
		private ToolStripSeparator toolStripMenuItem1;
		private ContextMenuStrip contextMenuStripF7;
		private ToolStripMenuItem toolStripMenuItemF7Refresh;
		private ContextMenuStrip contextMenuStripF6;
		private ToolStripMenuItem toolStripMenuItemF6Refresh;
		private ToolStripMenuItem toolStripMenuItemF6SP;
		private ToolStripSeparator toolStripMenuItem3;
		private SplitContainer splitContainerAll;
		private Button btReset;
		private Button butReset;
		private Label lbSettleBasis;
		private ComboBox cbSettleBasis;
		private ToolStripStatusLabel toolStripSystemStatus;
		private Panel panel1;
		private Panel panel2;
		private ToolStripStatusLabel toolStripStatusEnvironment;
		public DataGridView HQ_DataGrid;
		private TabPage tabPage10;
		private GroupBox groupBox2;
		private DataGridView dgvCustomerOrderF10_2;
		private TMainForm.CallbackFirmFundsF10DataGrid callbackFirmFundsF10DataGrid;
		private TMainForm.CallbackUpdateMemberFundPrice callbackUpdateMemberFundPrice;
		private DataTable ZiJindt = new DataTable();
		private YuJingMessage yujingmessage;
		private TMainForm.CallbackYuJing callbackyujing;
		private DataTable dtyj = new DataTable();
		private static XmlDataSet _XmlYJView;
		private static CreateXmlFile _CreateXml;
		private int YuJingCs;
		private string _YuJingMessage;
		private DataRow _YuJingCurrentRow;
		private int _YuJingNum = 1;
		private string _ServerTime = "";
		private string _DateTime = "";
		private int _YuJingHH = 1;
		private string _dqz = "";
		private string YJFileName = Global.ConfigPath + "yj" + Global.UserID + ".xml";
		private string YJMessageFileName = Global.ConfigPath + "yjmessage" + Global.UserID + ".xml";
		private TMainForm.CallbackCommodityInfoF8DataGrid callFillCommodityInfoF8DataGrid;
		private Dictionary<string, string> m_Commoditydata;
		private TMainForm.CallbackFirmInfoF7DataGrid callFillFirmInfoF7DataGrid;
		private TMainForm.CallbackHoldingInfoF6DataGrid callbackHoldingInfoF6DataGrid;
		private int _ChiCangNum = -1;
		private int _ChiCangNum2 = -1;
		private TMainForm.CallbackHoldingF5DataGrid callbackHoldingF5DataGrid;
		private TMainForm.CallbackUpDataHoldingDetailInfoF5HQ callbackUpDataHoldingDetailInfoF5HQ;
		private object LockDIDataTableF5 = new object();
		private DataSet _HDIDataTableF5;
		private SortOrder _LossSortOrderF5 = SortOrder.Ascending;
		private SortOrder _ProfitSortOrderF5 = SortOrder.Ascending;
		public TMainForm.SetToolsBarEnableCallBack SetToolsBarEnable;
		private Process m_pCalc;
		private DateTime IdleStartTime = DateTime.Now;
		private System.Timers.Timer timerLock = new System.Timers.Timer();
		private System.Timers.Timer timerHQ = new System.Timers.Timer();
		public string marketID = string.Empty;
		private Dictionary<string, TradeCtrl> m_MyTradeCtrl = new Dictionary<string, TradeCtrl>();
		private int m_iTradeCtrl = 10;
		private int m_iTradeCtrlx;
		private int m_iTradeCtrly;
		public TMainForm.EventPlayMessage PlayMessageEvent;
		private bool refreshTimeFlag = true;
		private bool refreshGNFlag = true;
		private int refreshHoldingDetailInfo = -1;
		private int MeInfoNum;
		private string messageInfomation = string.Empty;
		private Mutex mutex = new Mutex();
		private int _HoldingDetailContextMenuRowIndex = -1;
		private int _HQGridContextMenuRowIndex = -1;
		private int _HQGridContextMenuColumnIndex = -1;
		private int _XJGridContextMenuRowIndex = -1;
		private int _YJGridContextMenuRowIndex = -1;
		private IdentityStatus _IdentityStatus;
		private int _HoldingDetailMenuEnabled = -1;
		private int _XJGridMenuEnabled = -1;
		private bool Connect = true;
		private bool F2Flag = true;
		private bool F3Flag = true;
		private bool F4Flag = true;
		private bool F5Flag = true;
		private bool F6Flag = true;
		private bool F7Flag = true;
		private bool F8Flag = true;
		private bool F10Flag = true;
		private int IdleOnMoudel;
		private int IdleRefreshButton;
		public TMainForm.EventAgencyLogOut AgencyLogOut;
		private bool blogout;
		private bool firstRefreshTime;
		private object lockColor = new object();
		private bool UpDataHoldingDetailInfoflag;
		private bool UpDataHoldingDetailInfoF5flag;
		private bool FirmInfoflag;
		private bool FillHoldingInfoFloatingPriceflag;
		private bool UpdateMemberFundPriceflag;
		private bool FirmInfoF7flag;
		private bool UpdateCustomerOrderF10flag;
		private bool UpdateOrderInfoDataGridflag;
		private bool WithdrawLossProfitflag;
		private bool FillOrderInfoDataGridF3flag;
		private bool WithdrawOrderXflag;
		private bool QueryCommodityInfoF8flag;
		private bool FillTradeInfoDataGridF4flag;
		private bool YuJingMessageflag;
		private SystemStatus _CurrentSystemStatus;
		private object _CurrentSystemStatusObject = new object();
		internal bool refreshFlag;
		private NewOrdersform Ordersform;
		private PWarehouseForm pWarehouseForm;
		private UserSet userSet;
		private TMainYJSZ FromYJSZ;
		private AboutForm m_fAbout = new AboutForm();
		private bool systemTime = true;
		private TMainForm.Lock tlock;
		private bool displayInfo = true;
		private string strcurTradeDay = string.Empty;
		private TaskbarNotifier MessageNotifier = new TaskbarNotifier();
		private long LastID;
		public Font SysFont;
		private ConnectStatus connectStatus;
		internal DataProcess dataProcess = new DataProcess();
		private object floatingPriceTotalLock = new object();
		private double floatingPT;
		private double customerFloatingPT;
		private double _MemberMinRiskFund = 5000000.0;
		private EventHandler messageevent;
		private int timerMember;
		private int IntervaltimerMember;
		private bool Member;
		private Dictionary<string, WaitHandle> DictionarySemaphore = new Dictionary<string, WaitHandle>();
		private int iSelecttab;
		private object iSelecttablock = new object();
		private string IniFileName = string.Empty;
		private bool isFormShown;
		private int stoptime;
		private int timerLockRefresh = 1000;
		private int timerLockCount;
		private object LocktimerLockCount = new object();
		private int timerQuerySys = 1000;
		private int QuerySysCount;
		private object LockQuerySysCount = new object();
		private LocalHook hookKey;
		private LocalHook hook;
		private bool firstLockSet = true;
		private bool lockset = true;
		private bool bCloseForm;
		private bool PanelSize = true;
		private TMainForm.CallbackTradeInfoF4DataGrid callFillTradeInfoF4DataGrid;
		private TMainForm.CallbackOrderInfoF3DataGrid callFillOrderInfoF3DataGrid;
		private Hashtable totalF3ht = new Hashtable();
		private SortOrder _LossSortOrderOIF3 = SortOrder.Ascending;
		private SortOrder _ProfitSortOrderOIF3 = SortOrder.Ascending;
		private SortOrder sortOrder = SortOrder.Ascending;
		private string m_order = " Desc ";
		private TMainForm.CallbackHoldingDataGrid callFillHoldingDataGrid;
		private TMainForm.CallbackOrderInfoDataGrid callFillOrderInfoDataGrid;
		public TMainForm.CallbackFirmInfoDataGrid callbackFirmInfoDataGrid;
		public TMainForm.UpdateFirmInfoCallBack updateFirmInfo;
		private TMainForm.FillFirmInfoCallBack fillFirmInfoCallBack;
		private TMainForm.CallbackUpDataFirmInfo callbackUpDataFirmInfo;
		private TMainForm.CallbackUpDataFirmInfoF7 callbackUpDataFirmInfoF7;
		private DataTable _orderDataTable;
		private TMainForm.CallbackUpDataHoldingDetailInfoHQ callbackUpDataHoldingDetailInfoHQ;
		private string Total = Global.m_PMESResourceManager.GetString("PMESStr_TOTAL");
		private object LockDIDataTable = new object();
		private DataSet _HDIDataTable;
		private SortOrder _LossSortOrder = SortOrder.Ascending;
		private SortOrder _ProfitSortOrder = SortOrder.Ascending;
		private SortOrder _LossSortOrderOI = SortOrder.Ascending;
		private SortOrder _ProfitSortOrderOI = SortOrder.Ascending;
		private TMainForm.CallbackCustomerOrderF10DataGrid callbackCustomerOrderF10DataGrid;
		private TMainForm.CallbackUpdateCustomerOrderF10HQ callbackUpdateCustomerOrderF10HQ;
		private object LockDIDataTableF10 = new object();
		private DataSet _HDIDataTableF10;
		private Hashtable _CustomerOrderHashtable = new Hashtable();
		public event TMainForm.RefreshHQHanlder HQRefreashed;
		public event TMainForm.InitDataMainForm InitData;
		public event TMainForm.InitTradeCtrlMenuEnabled InitMenuEnabled;
		public event TMainForm.ReLoadHanlder ReLoad;
		public event EventHandler ChangeServerEvent;
		public event TMainForm.LogOut LogOutEvent;
		public event TMainForm.LockTree LockTreeEvent;
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TMainForm));
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle29 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle30 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle31 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle32 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle33 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle34 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle35 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle36 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle37 = new DataGridViewCellStyle();
			this.splitContainer1 = new SplitContainer();
			this.splitContainerHQ = new SplitContainer();
			this.HQ_DataGrid = new DataGridView();
			this.HqTitle = new Label();
			this.panelLock = new Panel();
			this.labelPwdInfo = new Label();
			this.buttonUnLock = new Button();
			this.labelPwd = new Label();
			this.textBoxPwd = new TextBox();
			this.tabTMain = new TabControl();
			this.tabPage1 = new TabPage();
			this.splitContainer2 = new SplitContainer();
			this.gbHoldingDetailInfo = new GroupBox();
			this.dgvHoldingDetailInfo = new DataGridView();
			this.splitContainer3 = new SplitContainer();
			this.splitContainer5 = new SplitContainer();
			this.gbOrderInfo = new GroupBox();
			this.dgvOrderInfo = new DataGridView();
			this.gbFirmInfo = new GroupBox();
			this.dgvFirmInfo = new DataGridView();
			this.splitContainer6 = new SplitContainer();
			this.gbCustomerOrderF10 = new GroupBox();
			this.dgvCustomerOrderF10 = new DataGridView();
			this.tabPage2 = new TabPage();
			this.gbOrderInfoF3 = new GroupBox();
			this.panel1 = new Panel();
			this.cbSettleBasis = new ComboBox();
			this.lbSettleBasis = new Label();
			this.btReset = new Button();
			this.lbBuySell = new Label();
			this.lbStatic = new Label();
			this.lbOrderInfoType = new Label();
			this.lbCommodity = new Label();
			this.Selectbt = new Button();
			this.cbBuySell = new ComboBox();
			this.cbOrderInfoType = new ComboBox();
			this.cbCommodity = new ComboBox();
			this.cbStatic = new ComboBox();
			this.dgvOrderInfoF3 = new DataGridView();
			this.dataGridView2 = new DataGridView();
			this.dataGridView1 = new DataGridView();
			this.tabPage3 = new TabPage();
			this.gbTradeInfo = new GroupBox();
			this.panel2 = new Panel();
			this.butReset = new Button();
			this.lbBuySellF4 = new Label();
			this.lbSettleBasisF4F4 = new Label();
			this.lbCommodityF4 = new Label();
			this.btSelectF4 = new Button();
			this.cbBuySellF4 = new ComboBox();
			this.cbCommodityF4 = new ComboBox();
			this.cbSettleBasisF4 = new ComboBox();
			this.dvgTradeInfo = new DataGridView();
			this.tabPage4 = new TabPage();
			this.gbHoldingDetailInfoF5 = new GroupBox();
			this.dgvHoldingDetailInfoF5 = new DataGridView();
			this.tabPage5 = new TabPage();
			this.gbHoldingInfoF6 = new GroupBox();
			this.dgvHoldingInfoF6 = new DataGridView();
			this.tabPage6 = new TabPage();
			this.gbqFirmInfo = new GroupBox();
			this.dgvqFirmInfo = new DataGridView();
			this.tabPage7 = new TabPage();
			this.gbCommodityInfo = new GroupBox();
			this.cbCommodityInfo = new ComboBox();
			this.LableCommodity = new Label();
			this.dgvCommodityInfo = new DataGridView();
			this.tabPage8 = new TabPage();
			this.groupBox1 = new GroupBox();
			this.dvgYJInfoF9 = new DataGridView();
			this.tabPage10 = new TabPage();
			this.groupBox2 = new GroupBox();
			this.dgvCustomerOrderF10_2 = new DataGridView();
			this.contextMenuStripFirmInfo = new ContextMenuStrip(this.components);
			this.toolStripMenuItemFirmInfoRefresh = new ToolStripMenuItem();
			this.MessageInfo = new Label();
			this.tabPage9 = new TabPage();
			this.splitContainer4 = new SplitContainer();
			this.contextMenuStripF9YJ = new ContextMenuStrip(this.components);
			this.NEWToolStripMenuItemYJF9 = new ToolStripMenuItem();
			this.toolStripMenuItem2 = new ToolStripSeparator();
			this.MODToolStripMenuItemYJF9 = new ToolStripMenuItem();
			this.DELToolStripMenuItemYJF9 = new ToolStripMenuItem();
			this.toolStripMenuItem1 = new ToolStripSeparator();
			this.ONOFFToolStripMenuItemYJF9 = new ToolStripMenuItem();
			this.contextMenuStripHoldingDetail = new ContextMenuStrip(this.components);
			this.toolStripMenuItemSP = new ToolStripMenuItem();
			this.toolStripMenuItemXP = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.toolStripMenuItemStopLoss = new ToolStripMenuItem();
			this.toolStripMenuItemStopProfit = new ToolStripMenuItem();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.toolStripMenuItemRefresh = new ToolStripMenuItem();
			this.toolStripSeparator4 = new ToolStripSeparator();
			this.toolStripMenuItemCancel = new ToolStripMenuItem();
			this.helpProvider = new HelpProvider();
			this.statusInfo = new StatusStrip();
			this.info = new ToolStripStatusLabel();
			this.toolStripSystemStatus = new ToolStripStatusLabel();
			this.toolStripStatusEnvironment = new ToolStripStatusLabel();
			this.user = new ToolStripStatusLabel();
			this.status = new ToolStripStatusLabel();
			this.time = new ToolStripStatusLabel();
			this.dataGridView3 = new DataGridView();
			this.contextMenuStripHQ = new ContextMenuStrip(this.components);
			this.toolStripMenuItemSO = new ToolStripMenuItem();
			this.toolStripMenuItemSC = new ToolStripMenuItem();
			this.toolStripSeparator5 = new ToolStripSeparator();
			this.toolStripMenuItemXO = new ToolStripMenuItem();
			this.toolStripSeparator6 = new ToolStripSeparator();
			this.toolStripMenuItemHQCancel = new ToolStripMenuItem();
			this.contextMenuStripXJ = new ContextMenuStrip(this.components);
			this.toolStripMenuItemWithdrawOrder = new ToolStripMenuItem();
			this.toolStripSeparator7 = new ToolStripSeparator();
			this.toolStripMenuItemXJRefresh = new ToolStripMenuItem();
			this.toolStripSeparator8 = new ToolStripSeparator();
			this.toolStripMenuItemXJCancel = new ToolStripMenuItem();
			this.contextMenuStripF7 = new ContextMenuStrip(this.components);
			this.toolStripMenuItemF7Refresh = new ToolStripMenuItem();
			this.contextMenuStripF6 = new ContextMenuStrip(this.components);
			this.toolStripMenuItemF6SP = new ToolStripMenuItem();
			this.toolStripMenuItem3 = new ToolStripSeparator();
			this.toolStripMenuItemF6Refresh = new ToolStripMenuItem();
			this.splitContainerAll = new SplitContainer();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainerHQ.Panel1.SuspendLayout();
			this.splitContainerHQ.SuspendLayout();
			((ISupportInitialize)this.HQ_DataGrid).BeginInit();
			this.panelLock.SuspendLayout();
			this.tabTMain.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.gbHoldingDetailInfo.SuspendLayout();
			((ISupportInitialize)this.dgvHoldingDetailInfo).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.splitContainer5.Panel1.SuspendLayout();
			this.splitContainer5.Panel2.SuspendLayout();
			this.splitContainer5.SuspendLayout();
			this.gbOrderInfo.SuspendLayout();
			((ISupportInitialize)this.dgvOrderInfo).BeginInit();
			this.gbFirmInfo.SuspendLayout();
			((ISupportInitialize)this.dgvFirmInfo).BeginInit();
			this.splitContainer6.Panel1.SuspendLayout();
			this.splitContainer6.SuspendLayout();
			this.gbCustomerOrderF10.SuspendLayout();
			((ISupportInitialize)this.dgvCustomerOrderF10).BeginInit();
			this.tabPage2.SuspendLayout();
			this.gbOrderInfoF3.SuspendLayout();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.dgvOrderInfoF3).BeginInit();
			((ISupportInitialize)this.dataGridView2).BeginInit();
			((ISupportInitialize)this.dataGridView1).BeginInit();
			this.tabPage3.SuspendLayout();
			this.gbTradeInfo.SuspendLayout();
			this.panel2.SuspendLayout();
			((ISupportInitialize)this.dvgTradeInfo).BeginInit();
			this.tabPage4.SuspendLayout();
			this.gbHoldingDetailInfoF5.SuspendLayout();
			((ISupportInitialize)this.dgvHoldingDetailInfoF5).BeginInit();
			this.tabPage5.SuspendLayout();
			this.gbHoldingInfoF6.SuspendLayout();
			((ISupportInitialize)this.dgvHoldingInfoF6).BeginInit();
			this.tabPage6.SuspendLayout();
			this.gbqFirmInfo.SuspendLayout();
			((ISupportInitialize)this.dgvqFirmInfo).BeginInit();
			this.tabPage7.SuspendLayout();
			this.gbCommodityInfo.SuspendLayout();
			((ISupportInitialize)this.dgvCommodityInfo).BeginInit();
			this.tabPage8.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.dvgYJInfoF9).BeginInit();
			this.tabPage10.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.dgvCustomerOrderF10_2).BeginInit();
			this.contextMenuStripFirmInfo.SuspendLayout();
			this.tabPage9.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			this.contextMenuStripF9YJ.SuspendLayout();
			this.contextMenuStripHoldingDetail.SuspendLayout();
			this.statusInfo.SuspendLayout();
			((ISupportInitialize)this.dataGridView3).BeginInit();
			this.contextMenuStripHQ.SuspendLayout();
			this.contextMenuStripXJ.SuspendLayout();
			this.contextMenuStripF7.SuspendLayout();
			this.contextMenuStripF6.SuspendLayout();
			this.splitContainerAll.Panel1.SuspendLayout();
			this.splitContainerAll.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.BackColor = SystemColors.Control;
			this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Margin = new Padding(4);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.splitContainerHQ);
			this.splitContainer1.Panel1.RightToLeft = RightToLeft.No;
			this.splitContainer1.Panel2.BackColor = SystemColors.Control;
			this.splitContainer1.Panel2.Controls.Add(this.panelLock);
			this.splitContainer1.Panel2.Controls.Add(this.tabTMain);
			this.splitContainer1.Panel2.RightToLeft = RightToLeft.No;
			this.splitContainer1.Size = new Size(1020, 720);
			this.splitContainer1.SplitterDistance = 198;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 3;
			this.splitContainerHQ.BorderStyle = BorderStyle.FixedSingle;
			this.splitContainerHQ.Dock = DockStyle.Fill;
			this.splitContainerHQ.Location = new Point(0, 0);
			this.splitContainerHQ.Margin = new Padding(4);
			this.splitContainerHQ.Name = "splitContainerHQ";
			this.splitContainerHQ.Panel1.BackColor = SystemColors.ButtonFace;
			this.splitContainerHQ.Panel1.Controls.Add(this.HQ_DataGrid);
			this.splitContainerHQ.Panel1.Controls.Add(this.HqTitle);
			this.splitContainerHQ.Panel2.AutoScroll = true;
			this.splitContainerHQ.Panel2.BackColor = Color.White;
			this.splitContainerHQ.Panel2.Resize += new EventHandler(this.splitContainerHQ_Panel2_Resize);
			this.splitContainerHQ.Size = new Size(1020, 198);
			this.splitContainerHQ.SplitterDistance = 539;
			this.splitContainerHQ.SplitterWidth = 6;
			this.splitContainerHQ.TabIndex = 0;
			this.HQ_DataGrid.AllowUserToAddRows = false;
			this.HQ_DataGrid.AllowUserToDeleteRows = false;
			this.HQ_DataGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = Color.White;
			this.HQ_DataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.HQ_DataGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.HQ_DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.HQ_DataGrid.BackgroundColor = Color.White;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Control;
			dataGridViewCellStyle2.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			this.HQ_DataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.HQ_DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.HQ_DataGrid.Location = new Point(8, 32);
			this.HQ_DataGrid.Margin = new Padding(4);
			this.HQ_DataGrid.MultiSelect = false;
			this.HQ_DataGrid.Name = "HQ_DataGrid";
			this.HQ_DataGrid.ReadOnly = true;
			this.HQ_DataGrid.RowHeadersVisible = false;
			this.HQ_DataGrid.RowHeadersWidth = 23;
			this.HQ_DataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			this.HQ_DataGrid.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.HQ_DataGrid.RowTemplate.Height = 30;
			this.HQ_DataGrid.RowTemplate.Resizable = DataGridViewTriState.False;
			this.HQ_DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.HQ_DataGrid.Size = new Size(525, 157);
			this.HQ_DataGrid.TabIndex = 2;
			this.HQ_DataGrid.CellMouseClick += new DataGridViewCellMouseEventHandler(this.HQ_DataGrid_CellMouseClick);
			this.HQ_DataGrid.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.HQ_DataGrid_CellMouseDoubleClick);
			this.HQ_DataGrid.MouseClick += new MouseEventHandler(this.HQ_DataGrid_MouseClick);
			this.HqTitle.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.HqTitle.AutoSize = true;
			this.HqTitle.BackColor = SystemColors.ButtonFace;
			this.HqTitle.Location = new Point(4, 6);
			this.HqTitle.Margin = new Padding(4, 0, 4, 0);
			this.HqTitle.Name = "HqTitle";
			this.HqTitle.Size = new Size(49, 13);
			this.HqTitle.TabIndex = 1;
			this.HqTitle.Text = "label1";
			this.panelLock.BackColor = Color.White;
			this.panelLock.BackgroundImage = (Image)componentResourceManager.GetObject("panelLock.BackgroundImage");
			this.panelLock.BorderStyle = BorderStyle.FixedSingle;
			this.panelLock.Controls.Add(this.labelPwdInfo);
			this.panelLock.Controls.Add(this.buttonUnLock);
			this.panelLock.Controls.Add(this.labelPwd);
			this.panelLock.Controls.Add(this.textBoxPwd);
			this.panelLock.Location = new Point(254, 178);
			this.panelLock.Margin = new Padding(4);
			this.panelLock.Name = "panelLock";
			this.helpProvider.SetShowHelp(this.panelLock, true);
			this.panelLock.Size = new Size(923, 299);
			this.panelLock.TabIndex = 8;
			this.panelLock.Visible = false;
			this.labelPwdInfo.AutoSize = true;
			this.labelPwdInfo.BackColor = Color.Transparent;
			this.labelPwdInfo.ImeMode = ImeMode.NoControl;
			this.labelPwdInfo.Location = new Point(345, 81);
			this.labelPwdInfo.Margin = new Padding(4, 0, 4, 0);
			this.labelPwdInfo.Name = "labelPwdInfo";
			this.helpProvider.SetShowHelp(this.labelPwdInfo, true);
			this.labelPwdInfo.Size = new Size(98, 13);
			this.labelPwdInfo.TabIndex = 3;
			this.labelPwdInfo.Text = "请输入登录密码";
			this.buttonUnLock.BackColor = Color.Transparent;
			this.buttonUnLock.ForeColor = Color.Black;
			this.buttonUnLock.ImeMode = ImeMode.NoControl;
			this.buttonUnLock.Location = new Point(192, 133);
			this.buttonUnLock.Margin = new Padding(4);
			this.buttonUnLock.Name = "buttonUnLock";
			this.helpProvider.SetShowHelp(this.buttonUnLock, true);
			this.buttonUnLock.Size = new Size(145, 25);
			this.buttonUnLock.TabIndex = 2;
			this.buttonUnLock.UseVisualStyleBackColor = false;
			this.buttonUnLock.Click += new EventHandler(this.buttonUnLock_Click_1);
			this.labelPwd.AutoSize = true;
			this.labelPwd.BackColor = Color.White;
			this.labelPwd.ImeMode = ImeMode.NoControl;
			this.labelPwd.Location = new Point(107, 81);
			this.labelPwd.Margin = new Padding(4, 0, 4, 0);
			this.labelPwd.Name = "labelPwd";
			this.helpProvider.SetShowHelp(this.labelPwd, true);
			this.labelPwd.Size = new Size(59, 13);
			this.labelPwd.TabIndex = 1;
			this.labelPwd.Text = "密\u3000码：";
			this.labelPwd.TextAlign = ContentAlignment.MiddleCenter;
			this.textBoxPwd.Location = new Point(192, 79);
			this.textBoxPwd.Margin = new Padding(4);
			this.textBoxPwd.Name = "textBoxPwd";
			this.textBoxPwd.PasswordChar = '*';
			this.helpProvider.SetShowHelp(this.textBoxPwd, true);
			this.textBoxPwd.Size = new Size(145, 22);
			this.textBoxPwd.TabIndex = 0;
			this.tabTMain.Controls.Add(this.tabPage1);
			this.tabTMain.Controls.Add(this.tabPage2);
			this.tabTMain.Controls.Add(this.tabPage3);
			this.tabTMain.Controls.Add(this.tabPage4);
			this.tabTMain.Controls.Add(this.tabPage5);
			this.tabTMain.Controls.Add(this.tabPage6);
			this.tabTMain.Controls.Add(this.tabPage7);
			this.tabTMain.Controls.Add(this.tabPage8);
			this.tabTMain.Controls.Add(this.tabPage10);
			this.tabTMain.Dock = DockStyle.Fill;
			this.tabTMain.Location = new Point(0, 0);
			this.tabTMain.Margin = new Padding(4);
			this.tabTMain.Name = "tabTMain";
			this.tabTMain.SelectedIndex = 0;
			this.tabTMain.Size = new Size(1018, 514);
			this.tabTMain.TabIndex = 0;
			this.tabTMain.SelectedIndexChanged += new EventHandler(this.tabTMain_SelectedIndexChanged);
			this.tabPage1.BackColor = SystemColors.Control;
			this.tabPage1.Controls.Add(this.splitContainer2);
			this.tabPage1.Location = new Point(4, 23);
			this.tabPage1.Margin = new Padding(4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(4);
			this.tabPage1.Size = new Size(1010, 487);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "F2";
			this.splitContainer2.BackColor = SystemColors.Control;
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.Location = new Point(4, 4);
			this.splitContainer2.Margin = new Padding(4);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = Orientation.Horizontal;
			this.splitContainer2.Panel1.Controls.Add(this.gbHoldingDetailInfo);
			this.splitContainer2.Panel1.ForeColor = Color.Black;
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new Size(1002, 479);
			this.splitContainer2.SplitterDistance = 278;
			this.splitContainer2.SplitterWidth = 6;
			this.splitContainer2.TabIndex = 3;
			this.gbHoldingDetailInfo.BackColor = SystemColors.Control;
			this.gbHoldingDetailInfo.Controls.Add(this.dgvHoldingDetailInfo);
			this.gbHoldingDetailInfo.Dock = DockStyle.Fill;
			this.gbHoldingDetailInfo.Location = new Point(0, 0);
			this.gbHoldingDetailInfo.Margin = new Padding(4);
			this.gbHoldingDetailInfo.Name = "gbHoldingDetailInfo";
			this.gbHoldingDetailInfo.Padding = new Padding(4);
			this.gbHoldingDetailInfo.Size = new Size(1002, 278);
			this.gbHoldingDetailInfo.TabIndex = 0;
			this.gbHoldingDetailInfo.TabStop = false;
			this.gbHoldingDetailInfo.Text = "持仓明细";
			this.dgvHoldingDetailInfo.AllowUserToAddRows = false;
			this.dgvHoldingDetailInfo.AllowUserToDeleteRows = false;
			this.dgvHoldingDetailInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle4.BackColor = Color.White;
			this.dgvHoldingDetailInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvHoldingDetailInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvHoldingDetailInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvHoldingDetailInfo.BackgroundColor = Color.White;
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = SystemColors.Control;
			dataGridViewCellStyle5.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			this.dgvHoldingDetailInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvHoldingDetailInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvHoldingDetailInfo.Dock = DockStyle.Fill;
			this.dgvHoldingDetailInfo.Location = new Point(4, 19);
			this.dgvHoldingDetailInfo.Margin = new Padding(4);
			this.dgvHoldingDetailInfo.MultiSelect = false;
			this.dgvHoldingDetailInfo.Name = "dgvHoldingDetailInfo";
			this.dgvHoldingDetailInfo.ReadOnly = true;
			this.dgvHoldingDetailInfo.RowHeadersVisible = false;
			this.dgvHoldingDetailInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle6.BackColor = Color.White;
			dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
			this.dgvHoldingDetailInfo.RowsDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvHoldingDetailInfo.RowTemplate.Height = 16;
			this.dgvHoldingDetailInfo.RowTemplate.Resizable = DataGridViewTriState.False;
			this.dgvHoldingDetailInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvHoldingDetailInfo.Size = new Size(994, 255);
			this.dgvHoldingDetailInfo.TabIndex = 1;
			this.dgvHoldingDetailInfo.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvHoldingDetailInfo_CellFormatting);
			this.dgvHoldingDetailInfo.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfo_CellMouseClick);
			this.dgvHoldingDetailInfo.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfo_CellMouseDoubleClick);
			this.dgvHoldingDetailInfo.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfo_ColumnHeaderMouseClick);
			this.dgvHoldingDetailInfo.SortCompare += new DataGridViewSortCompareEventHandler(this.dgvHoldingDetailInfo_SortCompare);
			this.dgvHoldingDetailInfo.Sorted += new EventHandler(this.dgvHoldingDetailInfo_Sorted);
			this.dgvHoldingDetailInfo.MouseClick += new MouseEventHandler(this.dgvHoldingDetailInfo_MouseClick);
			this.splitContainer3.BackColor = SystemColors.Control;
			this.splitContainer3.Dock = DockStyle.Fill;
			this.splitContainer3.Location = new Point(0, 0);
			this.splitContainer3.Margin = new Padding(4);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = Orientation.Horizontal;
			this.splitContainer3.Panel1.Controls.Add(this.splitContainer5);
			this.splitContainer3.Panel2.BackColor = SystemColors.Control;
			this.splitContainer3.Panel2.Controls.Add(this.splitContainer6);
			this.splitContainer3.Size = new Size(1002, 195);
			this.splitContainer3.SplitterDistance = 104;
			this.splitContainer3.SplitterWidth = 6;
			this.splitContainer3.TabIndex = 0;
			this.splitContainer5.Dock = DockStyle.Fill;
			this.splitContainer5.Location = new Point(0, 0);
			this.splitContainer5.Margin = new Padding(4);
			this.splitContainer5.Name = "splitContainer5";
			this.splitContainer5.Orientation = Orientation.Horizontal;
			this.splitContainer5.Panel1.Controls.Add(this.gbOrderInfo);
			this.splitContainer5.Panel2.Controls.Add(this.gbFirmInfo);
			this.splitContainer5.Size = new Size(1002, 104);
			this.splitContainer5.SplitterDistance = 40;
			this.splitContainer5.SplitterWidth = 6;
			this.splitContainer5.TabIndex = 2;
			this.gbOrderInfo.BackColor = SystemColors.Control;
			this.gbOrderInfo.Controls.Add(this.dgvOrderInfo);
			this.gbOrderInfo.Dock = DockStyle.Fill;
			this.gbOrderInfo.Location = new Point(0, 0);
			this.gbOrderInfo.Margin = new Padding(4);
			this.gbOrderInfo.Name = "gbOrderInfo";
			this.gbOrderInfo.Padding = new Padding(4);
			this.gbOrderInfo.Size = new Size(1002, 40);
			this.gbOrderInfo.TabIndex = 1;
			this.gbOrderInfo.TabStop = false;
			this.dgvOrderInfo.AllowUserToAddRows = false;
			this.dgvOrderInfo.AllowUserToDeleteRows = false;
			this.dgvOrderInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle7.BackColor = Color.White;
			this.dgvOrderInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvOrderInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvOrderInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvOrderInfo.BackgroundColor = Color.White;
			this.dgvOrderInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvOrderInfo.Dock = DockStyle.Fill;
			this.dgvOrderInfo.Location = new Point(4, 19);
			this.dgvOrderInfo.Margin = new Padding(4);
			this.dgvOrderInfo.MultiSelect = false;
			this.dgvOrderInfo.Name = "dgvOrderInfo";
			this.dgvOrderInfo.ReadOnly = true;
			this.dgvOrderInfo.RowHeadersVisible = false;
			this.dgvOrderInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle8.BackColor = Color.White;
			dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
			this.dgvOrderInfo.RowsDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvOrderInfo.RowTemplate.Height = 16;
			this.dgvOrderInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvOrderInfo.Size = new Size(994, 17);
			this.dgvOrderInfo.TabIndex = 2;
			this.dgvOrderInfo.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvOrderInfo_CellMouseClick);
			this.dgvOrderInfo.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvOrderInfo_ColumnHeaderMouseClick);
			this.dgvOrderInfo.MouseClick += new MouseEventHandler(this.dgvOrderInfo_MouseClick);
			this.gbFirmInfo.BackColor = SystemColors.Control;
			this.gbFirmInfo.Controls.Add(this.dgvFirmInfo);
			this.gbFirmInfo.Dock = DockStyle.Fill;
			this.gbFirmInfo.Location = new Point(0, 0);
			this.gbFirmInfo.Margin = new Padding(4);
			this.gbFirmInfo.Name = "gbFirmInfo";
			this.gbFirmInfo.Padding = new Padding(4);
			this.gbFirmInfo.Size = new Size(1002, 58);
			this.gbFirmInfo.TabIndex = 2;
			this.gbFirmInfo.TabStop = false;
			this.gbFirmInfo.Text = "账户信息";
			this.dgvFirmInfo.AllowUserToAddRows = false;
			this.dgvFirmInfo.AllowUserToDeleteRows = false;
			this.dgvFirmInfo.AllowUserToResizeColumns = false;
			this.dgvFirmInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle9.BackColor = Color.White;
			this.dgvFirmInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
			this.dgvFirmInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvFirmInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvFirmInfo.BackgroundColor = Color.White;
			this.dgvFirmInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvFirmInfo.Dock = DockStyle.Fill;
			this.dgvFirmInfo.Location = new Point(4, 19);
			this.dgvFirmInfo.Margin = new Padding(4);
			this.dgvFirmInfo.MultiSelect = false;
			this.dgvFirmInfo.Name = "dgvFirmInfo";
			this.dgvFirmInfo.ReadOnly = true;
			this.dgvFirmInfo.RowHeadersVisible = false;
			this.dgvFirmInfo.RowHeadersWidth = 23;
			this.dgvFirmInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle10.BackColor = Color.White;
			dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
			this.dgvFirmInfo.RowsDefaultCellStyle = dataGridViewCellStyle10;
			this.dgvFirmInfo.RowTemplate.Height = 16;
			this.dgvFirmInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvFirmInfo.Size = new Size(994, 35);
			this.dgvFirmInfo.TabIndex = 3;
			this.dgvFirmInfo.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvFirmInfo_CellMouseClick);
			this.dgvFirmInfo.MouseClick += new MouseEventHandler(this.dgvFirmInfo_MouseClick);
			this.splitContainer6.Dock = DockStyle.Fill;
			this.splitContainer6.Location = new Point(0, 0);
			this.splitContainer6.Margin = new Padding(4);
			this.splitContainer6.Name = "splitContainer6";
			this.splitContainer6.Orientation = Orientation.Horizontal;
			this.splitContainer6.Panel1.Controls.Add(this.gbCustomerOrderF10);
			this.splitContainer6.Size = new Size(1002, 85);
			this.splitContainer6.SplitterDistance = 28;
			this.splitContainer6.SplitterWidth = 6;
			this.splitContainer6.TabIndex = 2;
			this.gbCustomerOrderF10.BackColor = SystemColors.Control;
			this.gbCustomerOrderF10.Controls.Add(this.dgvCustomerOrderF10);
			this.gbCustomerOrderF10.Dock = DockStyle.Fill;
			this.gbCustomerOrderF10.Location = new Point(0, 0);
			this.gbCustomerOrderF10.Margin = new Padding(4);
			this.gbCustomerOrderF10.Name = "gbCustomerOrderF10";
			this.gbCustomerOrderF10.Padding = new Padding(4);
			this.gbCustomerOrderF10.Size = new Size(1002, 28);
			this.gbCustomerOrderF10.TabIndex = 1;
			this.gbCustomerOrderF10.TabStop = false;
			this.gbCustomerOrderF10.Text = "持仓明细";
			this.dgvCustomerOrderF10.AllowUserToAddRows = false;
			this.dgvCustomerOrderF10.AllowUserToDeleteRows = false;
			this.dgvCustomerOrderF10.AllowUserToResizeRows = false;
			dataGridViewCellStyle11.BackColor = Color.White;
			this.dgvCustomerOrderF10.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
			this.dgvCustomerOrderF10.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvCustomerOrderF10.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvCustomerOrderF10.BackgroundColor = Color.White;
			dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = SystemColors.Control;
			dataGridViewCellStyle12.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
			this.dgvCustomerOrderF10.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.dgvCustomerOrderF10.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle13.BackColor = SystemColors.Window;
			dataGridViewCellStyle13.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle13.ForeColor = Color.Black;
			dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
			this.dgvCustomerOrderF10.DefaultCellStyle = dataGridViewCellStyle13;
			this.dgvCustomerOrderF10.Dock = DockStyle.Fill;
			this.dgvCustomerOrderF10.Location = new Point(4, 19);
			this.dgvCustomerOrderF10.Margin = new Padding(4);
			this.dgvCustomerOrderF10.MultiSelect = false;
			this.dgvCustomerOrderF10.Name = "dgvCustomerOrderF10";
			this.dgvCustomerOrderF10.ReadOnly = true;
			this.dgvCustomerOrderF10.RowHeadersVisible = false;
			this.dgvCustomerOrderF10.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle14.BackColor = Color.White;
			dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
			this.dgvCustomerOrderF10.RowsDefaultCellStyle = dataGridViewCellStyle14;
			this.dgvCustomerOrderF10.RowTemplate.Height = 16;
			this.dgvCustomerOrderF10.RowTemplate.Resizable = DataGridViewTriState.False;
			this.dgvCustomerOrderF10.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvCustomerOrderF10.Size = new Size(994, 5);
			this.dgvCustomerOrderF10.TabIndex = 1;
			this.dgvCustomerOrderF10.MouseClick += new MouseEventHandler(this.dgvCustomerOrderF10_MouseClick);
			this.tabPage2.BackColor = SystemColors.Control;
			this.tabPage2.Controls.Add(this.gbOrderInfoF3);
			this.tabPage2.Controls.Add(this.dataGridView2);
			this.tabPage2.Controls.Add(this.dataGridView1);
			this.tabPage2.Location = new Point(4, 23);
			this.tabPage2.Margin = new Padding(4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(4);
			this.tabPage2.Size = new Size(1010, 487);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "F3";
			this.gbOrderInfoF3.BackColor = SystemColors.Control;
			this.gbOrderInfoF3.Controls.Add(this.panel1);
			this.gbOrderInfoF3.Controls.Add(this.dgvOrderInfoF3);
			this.gbOrderInfoF3.Dock = DockStyle.Fill;
			this.gbOrderInfoF3.Location = new Point(4, 4);
			this.gbOrderInfoF3.Margin = new Padding(4);
			this.gbOrderInfoF3.Name = "gbOrderInfoF3";
			this.gbOrderInfoF3.Padding = new Padding(4);
			this.gbOrderInfoF3.Size = new Size(1002, 479);
			this.gbOrderInfoF3.TabIndex = 5;
			this.gbOrderInfoF3.TabStop = false;
			this.gbOrderInfoF3.Text = "委托查询";
			this.gbOrderInfoF3.SizeChanged += new EventHandler(this.gbOrderInfoF3_SizeChanged);
			this.panel1.AutoScroll = true;
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.cbSettleBasis);
			this.panel1.Controls.Add(this.lbSettleBasis);
			this.panel1.Controls.Add(this.btReset);
			this.panel1.Controls.Add(this.lbBuySell);
			this.panel1.Controls.Add(this.lbStatic);
			this.panel1.Controls.Add(this.lbOrderInfoType);
			this.panel1.Controls.Add(this.lbCommodity);
			this.panel1.Controls.Add(this.Selectbt);
			this.panel1.Controls.Add(this.cbBuySell);
			this.panel1.Controls.Add(this.cbOrderInfoType);
			this.panel1.Controls.Add(this.cbCommodity);
			this.panel1.Controls.Add(this.cbStatic);
			this.panel1.Dock = DockStyle.Top;
			this.panel1.Location = new Point(4, 19);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(994, 57);
			this.panel1.TabIndex = 16;
			this.cbSettleBasis.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbSettleBasis.FormattingEnabled = true;
			this.cbSettleBasis.Location = new Point(259, 32);
			this.cbSettleBasis.Name = "cbSettleBasis";
			this.cbSettleBasis.Size = new Size(102, 21);
			this.cbSettleBasis.TabIndex = 15;
			this.lbSettleBasis.AutoSize = true;
			this.lbSettleBasis.Location = new Point(177, 37);
			this.lbSettleBasis.Name = "lbSettleBasis";
			this.lbSettleBasis.Size = new Size(66, 13);
			this.lbSettleBasis.TabIndex = 14;
			this.lbSettleBasis.Text = "建仓/平仓";
			this.btReset.AutoSize = true;
			this.btReset.Location = new Point(639, 1);
			this.btReset.Name = "btReset";
			this.btReset.Size = new Size(61, 23);
			this.btReset.TabIndex = 13;
			this.btReset.Text = "重置";
			this.btReset.UseVisualStyleBackColor = true;
			this.btReset.Click += new EventHandler(this.btReset_Click);
			this.lbBuySell.AutoSize = true;
			this.lbBuySell.Location = new Point(1, 37);
			this.lbBuySell.Margin = new Padding(4, 0, 4, 0);
			this.lbBuySell.Name = "lbBuySell";
			this.lbBuySell.Size = new Size(40, 13);
			this.lbBuySell.TabIndex = 12;
			this.lbBuySell.Text = "买/卖";
			this.lbStatic.AutoSize = true;
			this.lbStatic.Location = new Point(374, 8);
			this.lbStatic.Margin = new Padding(4, 0, 4, 0);
			this.lbStatic.Name = "lbStatic";
			this.lbStatic.Size = new Size(72, 13);
			this.lbStatic.TabIndex = 11;
			this.lbStatic.Text = "委托单状态";
			this.lbOrderInfoType.AutoSize = true;
			this.lbOrderInfoType.Location = new Point(177, 8);
			this.lbOrderInfoType.Margin = new Padding(4, 0, 4, 0);
			this.lbOrderInfoType.Name = "lbOrderInfoType";
			this.lbOrderInfoType.Size = new Size(72, 13);
			this.lbOrderInfoType.TabIndex = 10;
			this.lbOrderInfoType.Text = "委托单类型";
			this.lbCommodity.AutoSize = true;
			this.lbCommodity.Location = new Point(6, 8);
			this.lbCommodity.Margin = new Padding(4, 0, 4, 0);
			this.lbCommodity.Name = "lbCommodity";
			this.lbCommodity.Size = new Size(33, 13);
			this.lbCommodity.TabIndex = 9;
			this.lbCommodity.Text = "商品";
			this.Selectbt.AutoSize = true;
			this.Selectbt.Location = new Point(569, 1);
			this.Selectbt.Margin = new Padding(4);
			this.Selectbt.Name = "Selectbt";
			this.Selectbt.Size = new Size(61, 23);
			this.Selectbt.TabIndex = 8;
			this.Selectbt.Text = "查询";
			this.Selectbt.UseVisualStyleBackColor = true;
			this.Selectbt.Click += new EventHandler(this.Selectbt_Click);
			this.cbBuySell.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbBuySell.FormattingEnabled = true;
			this.cbBuySell.Location = new Point(52, 32);
			this.cbBuySell.Margin = new Padding(4);
			this.cbBuySell.Name = "cbBuySell";
			this.cbBuySell.Size = new Size(101, 21);
			this.cbBuySell.TabIndex = 7;
			this.cbOrderInfoType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbOrderInfoType.FormattingEnabled = true;
			this.cbOrderInfoType.Location = new Point(259, 3);
			this.cbOrderInfoType.Margin = new Padding(4);
			this.cbOrderInfoType.Name = "cbOrderInfoType";
			this.cbOrderInfoType.Size = new Size(102, 21);
			this.cbOrderInfoType.TabIndex = 6;
			this.cbCommodity.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbCommodity.FormattingEnabled = true;
			this.cbCommodity.Location = new Point(52, 3);
			this.cbCommodity.Margin = new Padding(4);
			this.cbCommodity.Name = "cbCommodity";
			this.cbCommodity.Size = new Size(101, 21);
			this.cbCommodity.TabIndex = 5;
			this.cbStatic.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbStatic.FormattingEnabled = true;
			this.cbStatic.Location = new Point(453, 3);
			this.cbStatic.Margin = new Padding(4);
			this.cbStatic.Name = "cbStatic";
			this.cbStatic.Size = new Size(98, 21);
			this.cbStatic.TabIndex = 4;
			this.dgvOrderInfoF3.AllowUserToAddRows = false;
			this.dgvOrderInfoF3.AllowUserToDeleteRows = false;
			this.dgvOrderInfoF3.AllowUserToResizeColumns = false;
			this.dgvOrderInfoF3.AllowUserToResizeRows = false;
			dataGridViewCellStyle15.BackColor = Color.White;
			this.dgvOrderInfoF3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
			this.dgvOrderInfoF3.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.dgvOrderInfoF3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvOrderInfoF3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvOrderInfoF3.BackgroundColor = Color.White;
			dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = SystemColors.Control;
			dataGridViewCellStyle16.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle16.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle16.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle16.SelectionForeColor = SystemColors.HighlightText;
			this.dgvOrderInfoF3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
			this.dgvOrderInfoF3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvOrderInfoF3.Location = new Point(4, 80);
			this.dgvOrderInfoF3.Margin = new Padding(4);
			this.dgvOrderInfoF3.MultiSelect = false;
			this.dgvOrderInfoF3.Name = "dgvOrderInfoF3";
			this.dgvOrderInfoF3.ReadOnly = true;
			this.dgvOrderInfoF3.RowHeadersVisible = false;
			this.dgvOrderInfoF3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle17.BackColor = Color.White;
			dataGridViewCellStyle17.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle17.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle17.SelectionForeColor = SystemColors.HighlightText;
			this.dgvOrderInfoF3.RowsDefaultCellStyle = dataGridViewCellStyle17;
			this.dgvOrderInfoF3.RowTemplate.Height = 16;
			this.dgvOrderInfoF3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvOrderInfoF3.Size = new Size(994, 344);
			this.dgvOrderInfoF3.TabIndex = 3;
			this.dgvOrderInfoF3.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvOrderInfoF3_CellMouseClick);
			this.dgvOrderInfoF3.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvOrderInfoF3_ColumnHeaderMouseClick);
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.AllowUserToResizeColumns = false;
			this.dataGridView2.AllowUserToResizeRows = false;
			dataGridViewCellStyle18.BackColor = SystemColors.ActiveCaptionText;
			this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle18;
			this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
			this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Dock = DockStyle.Fill;
			this.dataGridView2.Location = new Point(4, 4);
			this.dataGridView2.Margin = new Padding(4);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.RowHeadersVisible = false;
			this.dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView2.RowTemplate.Height = 23;
			this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView2.Size = new Size(1002, 479);
			this.dataGridView2.TabIndex = 4;
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			dataGridViewCellStyle19.BackColor = SystemColors.ActiveCaptionText;
			this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
			this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
			this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = DockStyle.Fill;
			this.dataGridView1.Location = new Point(4, 4);
			this.dataGridView1.Margin = new Padding(4);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new Size(1002, 479);
			this.dataGridView1.TabIndex = 3;
			this.tabPage3.BackColor = SystemColors.Control;
			this.tabPage3.Controls.Add(this.gbTradeInfo);
			this.tabPage3.Location = new Point(4, 23);
			this.tabPage3.Margin = new Padding(4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(4);
			this.tabPage3.Size = new Size(1010, 487);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "F4";
			this.gbTradeInfo.BackColor = SystemColors.Control;
			this.gbTradeInfo.Controls.Add(this.panel2);
			this.gbTradeInfo.Controls.Add(this.dvgTradeInfo);
			this.gbTradeInfo.Dock = DockStyle.Fill;
			this.gbTradeInfo.Location = new Point(4, 4);
			this.gbTradeInfo.Margin = new Padding(4);
			this.gbTradeInfo.Name = "gbTradeInfo";
			this.gbTradeInfo.Padding = new Padding(4);
			this.gbTradeInfo.Size = new Size(1002, 479);
			this.gbTradeInfo.TabIndex = 1;
			this.gbTradeInfo.TabStop = false;
			this.gbTradeInfo.Text = "成交查询";
			this.gbTradeInfo.SizeChanged += new EventHandler(this.gbTradeInfo_SizeChanged);
			this.panel2.AutoScroll = true;
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this.butReset);
			this.panel2.Controls.Add(this.lbBuySellF4);
			this.panel2.Controls.Add(this.lbSettleBasisF4F4);
			this.panel2.Controls.Add(this.lbCommodityF4);
			this.panel2.Controls.Add(this.btSelectF4);
			this.panel2.Controls.Add(this.cbBuySellF4);
			this.panel2.Controls.Add(this.cbCommodityF4);
			this.panel2.Controls.Add(this.cbSettleBasisF4);
			this.panel2.Dock = DockStyle.Top;
			this.panel2.Location = new Point(4, 19);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(994, 29);
			this.panel2.TabIndex = 21;
			this.panel2.SizeChanged += new EventHandler(this.panel2_SizeChanged);
			this.butReset.AutoSize = true;
			this.butReset.Location = new Point(627, 2);
			this.butReset.Name = "butReset";
			this.butReset.Size = new Size(61, 23);
			this.butReset.TabIndex = 20;
			this.butReset.Text = "重置";
			this.butReset.UseVisualStyleBackColor = true;
			this.butReset.Click += new EventHandler(this.butReset_Click);
			this.lbBuySellF4.AutoSize = true;
			this.lbBuySellF4.Location = new Point(369, 8);
			this.lbBuySellF4.Margin = new Padding(4, 0, 4, 0);
			this.lbBuySellF4.Name = "lbBuySellF4";
			this.lbBuySellF4.Size = new Size(40, 13);
			this.lbBuySellF4.TabIndex = 19;
			this.lbBuySellF4.Text = "买/卖";
			this.lbSettleBasisF4F4.AutoSize = true;
			this.lbSettleBasisF4F4.Location = new Point(179, 8);
			this.lbSettleBasisF4F4.Margin = new Padding(4, 0, 4, 0);
			this.lbSettleBasisF4F4.Name = "lbSettleBasisF4F4";
			this.lbSettleBasisF4F4.Size = new Size(66, 13);
			this.lbSettleBasisF4F4.TabIndex = 18;
			this.lbSettleBasisF4F4.Text = "建仓/平仓";
			this.lbCommodityF4.AutoSize = true;
			this.lbCommodityF4.Location = new Point(21, 8);
			this.lbCommodityF4.Margin = new Padding(4, 0, 4, 0);
			this.lbCommodityF4.Name = "lbCommodityF4";
			this.lbCommodityF4.Size = new Size(33, 13);
			this.lbCommodityF4.TabIndex = 17;
			this.lbCommodityF4.Text = "商品";
			this.btSelectF4.AutoSize = true;
			this.btSelectF4.Location = new Point(559, 2);
			this.btSelectF4.Margin = new Padding(4);
			this.btSelectF4.Name = "btSelectF4";
			this.btSelectF4.Size = new Size(61, 23);
			this.btSelectF4.TabIndex = 16;
			this.btSelectF4.Text = "查询";
			this.btSelectF4.UseVisualStyleBackColor = true;
			this.btSelectF4.Click += new EventHandler(this.btSelectF4_Click);
			this.cbBuySellF4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbBuySellF4.FormattingEnabled = true;
			this.cbBuySellF4.Location = new Point(419, 3);
			this.cbBuySellF4.Margin = new Padding(4);
			this.cbBuySellF4.Name = "cbBuySellF4";
			this.cbBuySellF4.Size = new Size(100, 21);
			this.cbBuySellF4.TabIndex = 15;
			this.cbCommodityF4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbCommodityF4.FormattingEnabled = true;
			this.cbCommodityF4.Location = new Point(64, 3);
			this.cbCommodityF4.Margin = new Padding(4);
			this.cbCommodityF4.Name = "cbCommodityF4";
			this.cbCommodityF4.Size = new Size(99, 21);
			this.cbCommodityF4.TabIndex = 14;
			this.cbSettleBasisF4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbSettleBasisF4.FormattingEnabled = true;
			this.cbSettleBasisF4.Location = new Point(252, 3);
			this.cbSettleBasisF4.Margin = new Padding(4);
			this.cbSettleBasisF4.Name = "cbSettleBasisF4";
			this.cbSettleBasisF4.Size = new Size(101, 21);
			this.cbSettleBasisF4.TabIndex = 13;
			this.dvgTradeInfo.AllowUserToAddRows = false;
			this.dvgTradeInfo.AllowUserToDeleteRows = false;
			this.dvgTradeInfo.AllowUserToResizeColumns = false;
			this.dvgTradeInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle20.BackColor = Color.White;
			this.dvgTradeInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle20;
			this.dvgTradeInfo.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.dvgTradeInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dvgTradeInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dvgTradeInfo.BackgroundColor = SystemColors.ButtonHighlight;
			this.dvgTradeInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle21.BackColor = SystemColors.Window;
			dataGridViewCellStyle21.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle21.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle21.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle21.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle21.WrapMode = DataGridViewTriState.False;
			this.dvgTradeInfo.DefaultCellStyle = dataGridViewCellStyle21;
			this.dvgTradeInfo.Location = new Point(4, 49);
			this.dvgTradeInfo.Margin = new Padding(4);
			this.dvgTradeInfo.MultiSelect = false;
			this.dvgTradeInfo.Name = "dvgTradeInfo";
			this.dvgTradeInfo.ReadOnly = true;
			this.dvgTradeInfo.RowHeadersVisible = false;
			this.dvgTradeInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dvgTradeInfo.RowTemplate.Height = 16;
			this.dvgTradeInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dvgTradeInfo.Size = new Size(994, 375);
			this.dvgTradeInfo.TabIndex = 1;
			this.dvgTradeInfo.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dvgTradeInfo_CellFormatting);
			this.dvgTradeInfo.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dvgTradeInfo_ColumnHeaderMouseClick);
			this.tabPage4.BackColor = SystemColors.Control;
			this.tabPage4.Controls.Add(this.gbHoldingDetailInfoF5);
			this.tabPage4.Location = new Point(4, 23);
			this.tabPage4.Margin = new Padding(4);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new Padding(4);
			this.tabPage4.Size = new Size(1010, 487);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "F5";
			this.gbHoldingDetailInfoF5.BackColor = SystemColors.Control;
			this.gbHoldingDetailInfoF5.Controls.Add(this.dgvHoldingDetailInfoF5);
			this.gbHoldingDetailInfoF5.Dock = DockStyle.Fill;
			this.gbHoldingDetailInfoF5.Location = new Point(4, 4);
			this.gbHoldingDetailInfoF5.Margin = new Padding(4);
			this.gbHoldingDetailInfoF5.Name = "gbHoldingDetailInfoF5";
			this.gbHoldingDetailInfoF5.Padding = new Padding(4);
			this.gbHoldingDetailInfoF5.Size = new Size(1002, 479);
			this.gbHoldingDetailInfoF5.TabIndex = 1;
			this.gbHoldingDetailInfoF5.TabStop = false;
			this.gbHoldingDetailInfoF5.Text = "持仓明细";
			this.dgvHoldingDetailInfoF5.AllowUserToAddRows = false;
			this.dgvHoldingDetailInfoF5.AllowUserToDeleteRows = false;
			this.dgvHoldingDetailInfoF5.AllowUserToResizeRows = false;
			dataGridViewCellStyle22.BackColor = Color.White;
			this.dgvHoldingDetailInfoF5.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle22;
			this.dgvHoldingDetailInfoF5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvHoldingDetailInfoF5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvHoldingDetailInfoF5.BackgroundColor = Color.White;
			dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle23.BackColor = SystemColors.Control;
			dataGridViewCellStyle23.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle23.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle23.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle23.SelectionForeColor = SystemColors.HighlightText;
			this.dgvHoldingDetailInfoF5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle23;
			this.dgvHoldingDetailInfoF5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvHoldingDetailInfoF5.Dock = DockStyle.Fill;
			this.dgvHoldingDetailInfoF5.Location = new Point(4, 19);
			this.dgvHoldingDetailInfoF5.Margin = new Padding(4);
			this.dgvHoldingDetailInfoF5.MultiSelect = false;
			this.dgvHoldingDetailInfoF5.Name = "dgvHoldingDetailInfoF5";
			this.dgvHoldingDetailInfoF5.ReadOnly = true;
			this.dgvHoldingDetailInfoF5.RowHeadersVisible = false;
			this.dgvHoldingDetailInfoF5.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle24.BackColor = Color.White;
			dataGridViewCellStyle24.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle24.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle24.SelectionForeColor = SystemColors.HighlightText;
			this.dgvHoldingDetailInfoF5.RowsDefaultCellStyle = dataGridViewCellStyle24;
			this.dgvHoldingDetailInfoF5.RowTemplate.Height = 16;
			this.dgvHoldingDetailInfoF5.RowTemplate.Resizable = DataGridViewTriState.False;
			this.dgvHoldingDetailInfoF5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvHoldingDetailInfoF5.Size = new Size(994, 456);
			this.dgvHoldingDetailInfoF5.TabIndex = 2;
			this.dgvHoldingDetailInfoF5.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvHoldingDetailInfoF5_CellFormatting);
			this.dgvHoldingDetailInfoF5.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfoF5_CellMouseClick);
			this.dgvHoldingDetailInfoF5.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfo_CellMouseDoubleClick);
			this.dgvHoldingDetailInfoF5.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingDetailInfoF5_ColumnHeaderMouseClick);
			this.dgvHoldingDetailInfoF5.MouseClick += new MouseEventHandler(this.dgvHoldingDetailInfoF5_MouseClick);
			this.tabPage5.BackColor = SystemColors.Control;
			this.tabPage5.Controls.Add(this.gbHoldingInfoF6);
			this.tabPage5.Location = new Point(4, 22);
			this.tabPage5.Margin = new Padding(4);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new Padding(4);
			this.tabPage5.Size = new Size(1010, 488);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "F6";
			this.gbHoldingInfoF6.BackColor = SystemColors.Control;
			this.gbHoldingInfoF6.Controls.Add(this.dgvHoldingInfoF6);
			this.gbHoldingInfoF6.Dock = DockStyle.Fill;
			this.gbHoldingInfoF6.Location = new Point(4, 4);
			this.gbHoldingInfoF6.Margin = new Padding(4);
			this.gbHoldingInfoF6.Name = "gbHoldingInfoF6";
			this.gbHoldingInfoF6.Padding = new Padding(4);
			this.gbHoldingInfoF6.Size = new Size(1002, 480);
			this.gbHoldingInfoF6.TabIndex = 2;
			this.gbHoldingInfoF6.TabStop = false;
			this.gbHoldingInfoF6.Text = "持仓汇总";
			this.dgvHoldingInfoF6.AllowUserToAddRows = false;
			this.dgvHoldingInfoF6.AllowUserToDeleteRows = false;
			this.dgvHoldingInfoF6.AllowUserToResizeColumns = false;
			this.dgvHoldingInfoF6.AllowUserToResizeRows = false;
			dataGridViewCellStyle25.BackColor = Color.White;
			this.dgvHoldingInfoF6.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle25;
			this.dgvHoldingInfoF6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvHoldingInfoF6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvHoldingInfoF6.BackgroundColor = SystemColors.ButtonHighlight;
			this.dgvHoldingInfoF6.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvHoldingInfoF6.Dock = DockStyle.Fill;
			this.dgvHoldingInfoF6.Location = new Point(4, 19);
			this.dgvHoldingInfoF6.Margin = new Padding(4);
			this.dgvHoldingInfoF6.MultiSelect = false;
			this.dgvHoldingInfoF6.Name = "dgvHoldingInfoF6";
			this.dgvHoldingInfoF6.ReadOnly = true;
			this.dgvHoldingInfoF6.RowHeadersVisible = false;
			this.dgvHoldingInfoF6.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvHoldingInfoF6.RowTemplate.Height = 16;
			this.dgvHoldingInfoF6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvHoldingInfoF6.Size = new Size(994, 457);
			this.dgvHoldingInfoF6.TabIndex = 1;
			this.dgvHoldingInfoF6.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingInfoF6_CellMouseClick);
			this.dgvHoldingInfoF6.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvHoldingInfoF6_CellMouseDoubleClick);
			this.dgvHoldingInfoF6.MouseClick += new MouseEventHandler(this.dgvHoldingInfoF6_MouseClick);
			this.tabPage6.BackColor = SystemColors.Control;
			this.tabPage6.Controls.Add(this.gbqFirmInfo);
			this.tabPage6.Location = new Point(4, 22);
			this.tabPage6.Margin = new Padding(4);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new Padding(4);
			this.tabPage6.Size = new Size(1010, 488);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "F7";
			this.gbqFirmInfo.BackColor = SystemColors.Control;
			this.gbqFirmInfo.Controls.Add(this.dgvqFirmInfo);
			this.gbqFirmInfo.Dock = DockStyle.Fill;
			this.gbqFirmInfo.Location = new Point(4, 4);
			this.gbqFirmInfo.Margin = new Padding(4);
			this.gbqFirmInfo.Name = "gbqFirmInfo";
			this.gbqFirmInfo.Padding = new Padding(4);
			this.gbqFirmInfo.Size = new Size(1002, 480);
			this.gbqFirmInfo.TabIndex = 2;
			this.gbqFirmInfo.TabStop = false;
			this.gbqFirmInfo.Text = "账户信息";
			this.dgvqFirmInfo.AllowUserToAddRows = false;
			this.dgvqFirmInfo.AllowUserToDeleteRows = false;
			this.dgvqFirmInfo.AllowUserToResizeColumns = false;
			this.dgvqFirmInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle26.BackColor = Color.White;
			this.dgvqFirmInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle26;
			this.dgvqFirmInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvqFirmInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvqFirmInfo.BackgroundColor = SystemColors.ButtonHighlight;
			dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle27.BackColor = SystemColors.Control;
			dataGridViewCellStyle27.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle27.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle27.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle27.SelectionForeColor = SystemColors.HighlightText;
			this.dgvqFirmInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle27;
			this.dgvqFirmInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvqFirmInfo.Dock = DockStyle.Fill;
			this.dgvqFirmInfo.Location = new Point(4, 19);
			this.dgvqFirmInfo.Margin = new Padding(4);
			this.dgvqFirmInfo.MultiSelect = false;
			this.dgvqFirmInfo.Name = "dgvqFirmInfo";
			this.dgvqFirmInfo.ReadOnly = true;
			this.dgvqFirmInfo.RowHeadersVisible = false;
			this.dgvqFirmInfo.RowHeadersWidth = 23;
			this.dgvqFirmInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvqFirmInfo.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.dgvqFirmInfo.RowTemplate.Height = 16;
			this.dgvqFirmInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvqFirmInfo.Size = new Size(994, 457);
			this.dgvqFirmInfo.TabIndex = 2;
			this.dgvqFirmInfo.MouseClick += new MouseEventHandler(this.dgvqFirmInfo_MouseClick);
			this.tabPage7.Controls.Add(this.gbCommodityInfo);
			this.tabPage7.Location = new Point(4, 22);
			this.tabPage7.Margin = new Padding(4);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new Padding(4);
			this.tabPage7.Size = new Size(1010, 488);
			this.tabPage7.TabIndex = 6;
			this.tabPage7.Text = "F8";
			this.tabPage7.UseVisualStyleBackColor = true;
			this.gbCommodityInfo.BackColor = SystemColors.Control;
			this.gbCommodityInfo.Controls.Add(this.cbCommodityInfo);
			this.gbCommodityInfo.Controls.Add(this.LableCommodity);
			this.gbCommodityInfo.Controls.Add(this.dgvCommodityInfo);
			this.gbCommodityInfo.Dock = DockStyle.Fill;
			this.gbCommodityInfo.Location = new Point(4, 4);
			this.gbCommodityInfo.Margin = new Padding(4);
			this.gbCommodityInfo.Name = "gbCommodityInfo";
			this.gbCommodityInfo.Padding = new Padding(4);
			this.gbCommodityInfo.Size = new Size(1002, 480);
			this.gbCommodityInfo.TabIndex = 6;
			this.gbCommodityInfo.TabStop = false;
			this.gbCommodityInfo.Text = "商品信息";
			this.gbCommodityInfo.SizeChanged += new EventHandler(this.gbCommodityInfo_SizeChanged);
			this.cbCommodityInfo.FormattingEnabled = true;
			this.cbCommodityInfo.Location = new Point(94, 23);
			this.cbCommodityInfo.Margin = new Padding(4);
			this.cbCommodityInfo.Name = "cbCommodityInfo";
			this.cbCommodityInfo.Size = new Size(120, 21);
			this.cbCommodityInfo.TabIndex = 4;
			this.cbCommodityInfo.SelectedIndexChanged += new EventHandler(this.cbCommodityInfo_SelectedIndexChanged_1);
			this.LableCommodity.AutoSize = true;
			this.LableCommodity.Location = new Point(18, 27);
			this.LableCommodity.Margin = new Padding(4, 0, 4, 0);
			this.LableCommodity.Name = "LableCommodity";
			this.LableCommodity.Size = new Size(72, 13);
			this.LableCommodity.TabIndex = 3;
			this.LableCommodity.Text = "请选择商品";
			this.dgvCommodityInfo.AllowUserToAddRows = false;
			this.dgvCommodityInfo.AllowUserToDeleteRows = false;
			this.dgvCommodityInfo.AllowUserToResizeColumns = false;
			this.dgvCommodityInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle28.BackColor = Color.White;
			this.dgvCommodityInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle28;
			this.dgvCommodityInfo.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dgvCommodityInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvCommodityInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			this.dgvCommodityInfo.BackgroundColor = SystemColors.ButtonHighlight;
			this.dgvCommodityInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCommodityInfo.Location = new Point(4, 53);
			this.dgvCommodityInfo.Margin = new Padding(4);
			this.dgvCommodityInfo.MultiSelect = false;
			this.dgvCommodityInfo.Name = "dgvCommodityInfo";
			this.dgvCommodityInfo.ReadOnly = true;
			this.dgvCommodityInfo.RowHeadersVisible = false;
			this.dgvCommodityInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle29.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.dgvCommodityInfo.RowsDefaultCellStyle = dataGridViewCellStyle29;
			this.dgvCommodityInfo.RowTemplate.Height = 16;
			this.dgvCommodityInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvCommodityInfo.Size = new Size(992, 421);
			this.dgvCommodityInfo.TabIndex = 2;
			this.tabPage8.BackColor = SystemColors.Control;
			this.tabPage8.Controls.Add(this.groupBox1);
			this.tabPage8.Location = new Point(4, 22);
			this.tabPage8.Margin = new Padding(4);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Padding = new Padding(4);
			this.tabPage8.Size = new Size(1010, 488);
			this.tabPage8.TabIndex = 7;
			this.tabPage8.Text = "F9";
			this.groupBox1.Controls.Add(this.dvgYJInfoF9);
			this.groupBox1.Dock = DockStyle.Fill;
			this.groupBox1.Location = new Point(4, 4);
			this.groupBox1.Margin = new Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new Padding(4);
			this.groupBox1.Size = new Size(1002, 480);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "预警信息";
			this.dvgYJInfoF9.AllowUserToAddRows = false;
			this.dvgYJInfoF9.AllowUserToDeleteRows = false;
			this.dvgYJInfoF9.AllowUserToResizeRows = false;
			this.dvgYJInfoF9.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dvgYJInfoF9.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dvgYJInfoF9.BackgroundColor = Color.White;
			dataGridViewCellStyle30.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle30.BackColor = SystemColors.Control;
			dataGridViewCellStyle30.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle30.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle30.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle30.SelectionForeColor = SystemColors.HighlightText;
			this.dvgYJInfoF9.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle30;
			this.dvgYJInfoF9.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dvgYJInfoF9.Dock = DockStyle.Fill;
			this.dvgYJInfoF9.Location = new Point(4, 19);
			this.dvgYJInfoF9.Margin = new Padding(4);
			this.dvgYJInfoF9.MultiSelect = false;
			this.dvgYJInfoF9.Name = "dvgYJInfoF9";
			this.dvgYJInfoF9.ReadOnly = true;
			this.dvgYJInfoF9.RowHeadersVisible = false;
			this.dvgYJInfoF9.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dvgYJInfoF9.RowTemplate.Height = 16;
			this.dvgYJInfoF9.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dvgYJInfoF9.Size = new Size(994, 457);
			this.dvgYJInfoF9.TabIndex = 0;
			this.dvgYJInfoF9.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dvgYJInfoF9_CellMouseClick);
			this.dvgYJInfoF9.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dvgYJInfoF9_CellMouseDoubleClick);
			this.dvgYJInfoF9.MouseClick += new MouseEventHandler(this.dvgYJInfoF9_MouseClick);
			this.tabPage10.Controls.Add(this.groupBox2);
			this.tabPage10.Location = new Point(4, 22);
			this.tabPage10.Name = "tabPage10";
			this.tabPage10.Size = new Size(1010, 488);
			this.tabPage10.TabIndex = 8;
			this.tabPage10.Text = "F10资金信息";
			this.tabPage10.UseVisualStyleBackColor = true;
			this.groupBox2.BackColor = SystemColors.Control;
			this.groupBox2.Controls.Add(this.dgvCustomerOrderF10_2);
			this.groupBox2.Dock = DockStyle.Fill;
			this.groupBox2.Location = new Point(0, 0);
			this.groupBox2.Margin = new Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new Padding(4);
			this.groupBox2.Size = new Size(1010, 488);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.dgvCustomerOrderF10_2.AllowUserToAddRows = false;
			this.dgvCustomerOrderF10_2.AllowUserToDeleteRows = false;
			this.dgvCustomerOrderF10_2.AllowUserToResizeRows = false;
			dataGridViewCellStyle31.BackColor = Color.White;
			this.dgvCustomerOrderF10_2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle31;
			this.dgvCustomerOrderF10_2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvCustomerOrderF10_2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvCustomerOrderF10_2.BackgroundColor = Color.White;
			dataGridViewCellStyle32.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle32.BackColor = SystemColors.Control;
			dataGridViewCellStyle32.Font = new Font("宋体", 9.5f);
			dataGridViewCellStyle32.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle32.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle32.SelectionForeColor = SystemColors.HighlightText;
			this.dgvCustomerOrderF10_2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle32;
			this.dgvCustomerOrderF10_2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCustomerOrderF10_2.Dock = DockStyle.Fill;
			this.dgvCustomerOrderF10_2.Location = new Point(4, 19);
			this.dgvCustomerOrderF10_2.Margin = new Padding(4);
			this.dgvCustomerOrderF10_2.MultiSelect = false;
			this.dgvCustomerOrderF10_2.Name = "dgvCustomerOrderF10_2";
			this.dgvCustomerOrderF10_2.ReadOnly = true;
			this.dgvCustomerOrderF10_2.RowHeadersVisible = false;
			this.dgvCustomerOrderF10_2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle33.BackColor = Color.White;
			dataGridViewCellStyle33.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle33.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle33.SelectionForeColor = SystemColors.HighlightText;
			this.dgvCustomerOrderF10_2.RowsDefaultCellStyle = dataGridViewCellStyle33;
			this.dgvCustomerOrderF10_2.RowTemplate.Height = 16;
			this.dgvCustomerOrderF10_2.RowTemplate.Resizable = DataGridViewTriState.False;
			this.dgvCustomerOrderF10_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvCustomerOrderF10_2.Size = new Size(1002, 465);
			this.dgvCustomerOrderF10_2.TabIndex = 0;
			this.dgvCustomerOrderF10_2.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvCustomerOrderF10_2_CellFormatting);
			this.dgvCustomerOrderF10_2.MouseClick += new MouseEventHandler(this.dgvCustomerOrderF10_2_MouseClick);
			this.contextMenuStripFirmInfo.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemFirmInfoRefresh
			});
			this.contextMenuStripFirmInfo.Name = "contextMenuStripFirmInfo";
			this.contextMenuStripFirmInfo.Size = new Size(101, 26);
			this.contextMenuStripFirmInfo.Opening += new CancelEventHandler(this.contextMenuStripFirmInfo_Opening);
			this.toolStripMenuItemFirmInfoRefresh.Name = "toolStripMenuItemFirmInfoRefresh";
			this.toolStripMenuItemFirmInfoRefresh.Size = new Size(100, 22);
			this.toolStripMenuItemFirmInfoRefresh.Text = "刷新";
			this.toolStripMenuItemFirmInfoRefresh.Click += new EventHandler(this.toolStripMenuItemFirmInfoRefresh_Click);
			this.MessageInfo.BackColor = Color.FromArgb(255, 255, 192);
			this.MessageInfo.ImeMode = ImeMode.NoControl;
			this.MessageInfo.Location = new Point(420, 66);
			this.MessageInfo.Margin = new Padding(4, 0, 4, 0);
			this.MessageInfo.Name = "MessageInfo";
			this.helpProvider.SetShowHelp(this.MessageInfo, true);
			this.MessageInfo.Size = new Size(202, 39);
			this.MessageInfo.TabIndex = 14;
			this.MessageInfo.Text = "MessageInfo";
			this.MessageInfo.TextAlign = ContentAlignment.MiddleCenter;
			this.tabPage9.BackColor = SystemColors.Control;
			this.tabPage9.Controls.Add(this.splitContainer4);
			this.tabPage9.Location = new Point(4, 22);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Padding = new Padding(3);
			this.tabPage9.Size = new Size(953, 319);
			this.tabPage9.TabIndex = 8;
			this.tabPage9.Text = "F10";
			this.splitContainer4.Dock = DockStyle.Fill;
			this.splitContainer4.Location = new Point(3, 3);
			this.splitContainer4.Name = "splitContainer4";
			this.splitContainer4.Orientation = Orientation.Horizontal;
			this.splitContainer4.Size = new Size(947, 313);
			this.splitContainer4.SplitterDistance = 150;
			this.splitContainer4.TabIndex = 2;
			this.contextMenuStripF9YJ.Items.AddRange(new ToolStripItem[]
			{
				this.NEWToolStripMenuItemYJF9,
				this.toolStripMenuItem2,
				this.MODToolStripMenuItemYJF9,
				this.DELToolStripMenuItemYJF9,
				this.toolStripMenuItem1,
				this.ONOFFToolStripMenuItemYJF9
			});
			this.contextMenuStripF9YJ.Name = "contextMenuStripF9YJ";
			this.contextMenuStripF9YJ.Size = new Size(130, 104);
			this.contextMenuStripF9YJ.Closed += new ToolStripDropDownClosedEventHandler(this.contextMenuStripF9YJ_Closed);
			this.NEWToolStripMenuItemYJF9.Name = "NEWToolStripMenuItemYJF9";
			this.NEWToolStripMenuItemYJF9.Size = new Size(129, 22);
			this.NEWToolStripMenuItemYJF9.Text = "新建";
			this.NEWToolStripMenuItemYJF9.Click += new EventHandler(this.NEWToolStripMenuItemYJF9_Click_1);
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new Size(126, 6);
			this.MODToolStripMenuItemYJF9.Name = "MODToolStripMenuItemYJF9";
			this.MODToolStripMenuItemYJF9.Size = new Size(129, 22);
			this.MODToolStripMenuItemYJF9.Text = "修改";
			this.MODToolStripMenuItemYJF9.Click += new EventHandler(this.MODToolStripMenuItemYJF9_Click);
			this.DELToolStripMenuItemYJF9.Name = "DELToolStripMenuItemYJF9";
			this.DELToolStripMenuItemYJF9.Size = new Size(129, 22);
			this.DELToolStripMenuItemYJF9.Text = "删除";
			this.DELToolStripMenuItemYJF9.Click += new EventHandler(this.DELToolStripMenuItemYJF9_Click);
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new Size(126, 6);
			this.ONOFFToolStripMenuItemYJF9.Name = "ONOFFToolStripMenuItemYJF9";
			this.ONOFFToolStripMenuItemYJF9.Size = new Size(129, 22);
			this.ONOFFToolStripMenuItemYJF9.Text = "启动/关闭";
			this.ONOFFToolStripMenuItemYJF9.Click += new EventHandler(this.ONOFFToolStripMenuItemYJF9_Click);
			this.contextMenuStripHoldingDetail.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemSP,
				this.toolStripMenuItemXP,
				this.toolStripSeparator2,
				this.toolStripMenuItemStopLoss,
				this.toolStripMenuItemStopProfit,
				this.toolStripSeparator3,
				this.toolStripMenuItemRefresh,
				this.toolStripSeparator4,
				this.toolStripMenuItemCancel
			});
			this.contextMenuStripHoldingDetail.Name = "contextMenuStripHoldingDetail";
			this.contextMenuStripHoldingDetail.Size = new Size(137, 154);
			this.contextMenuStripHoldingDetail.Closed += new ToolStripDropDownClosedEventHandler(this.contextMenuStripHoldingDetail_Closed);
			this.toolStripMenuItemSP.Name = "toolStripMenuItemSP";
			this.toolStripMenuItemSP.Size = new Size(136, 22);
			this.toolStripMenuItemSP.Text = "市价平仓单";
			this.toolStripMenuItemSP.Click += new EventHandler(this.toolStripMenuItemSP_Click);
			this.toolStripMenuItemXP.Name = "toolStripMenuItemXP";
			this.toolStripMenuItemXP.Size = new Size(136, 22);
			this.toolStripMenuItemXP.Click += new EventHandler(this.toolStripMenuItemXP_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(133, 6);
			this.toolStripMenuItemStopLoss.Name = "toolStripMenuItemStopLoss";
			this.toolStripMenuItemStopLoss.Size = new Size(136, 22);
			this.toolStripMenuItemStopLoss.Text = "撤销止损单";
			this.toolStripMenuItemStopLoss.Click += new EventHandler(this.toolStripMenuItemStopLoss_Click);
			this.toolStripMenuItemStopProfit.Name = "toolStripMenuItemStopProfit";
			this.toolStripMenuItemStopProfit.Size = new Size(136, 22);
			this.toolStripMenuItemStopProfit.Text = "撤销止盈单";
			this.toolStripMenuItemStopProfit.Click += new EventHandler(this.toolStripMenuItemStopProfit_Click);
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new Size(133, 6);
			this.toolStripMenuItemRefresh.Name = "toolStripMenuItemRefresh";
			this.toolStripMenuItemRefresh.Size = new Size(136, 22);
			this.toolStripMenuItemRefresh.Text = "刷新";
			this.toolStripMenuItemRefresh.Click += new EventHandler(this.toolStripMenuItemRefresh_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new Size(133, 6);
			this.toolStripMenuItemCancel.Name = "toolStripMenuItemCancel";
			this.toolStripMenuItemCancel.Size = new Size(136, 22);
			this.toolStripMenuItemCancel.Text = "取消";
			this.toolStripMenuItemCancel.Click += new EventHandler(this.toolStripMenuItemCancel_Click);
			this.helpProvider.HelpNamespace = "Help.chm";
			this.statusInfo.Items.AddRange(new ToolStripItem[]
			{
				this.info,
				this.toolStripSystemStatus,
				this.toolStripStatusEnvironment,
				this.user,
				this.status,
				this.time
			});
			this.statusInfo.Location = new Point(0, 720);
			this.statusInfo.Name = "statusInfo";
			this.statusInfo.Padding = new Padding(2, 0, 21, 0);
			this.helpProvider.SetShowHelp(this.statusInfo, true);
			this.statusInfo.Size = new Size(1020, 22);
			this.statusInfo.TabIndex = 6;
			this.statusInfo.Text = "statusStrip1";
			this.statusInfo.Resize += new EventHandler(this.statusInfo_Resize);
			this.info.AutoSize = false;
			this.info.ForeColor = SystemColors.ControlText;
			this.info.Name = "info";
			this.info.Size = new Size(100, 17);
			this.info.Text = "信息提示";
			this.info.TextAlign = ContentAlignment.MiddleLeft;
			this.toolStripSystemStatus.AutoSize = false;
			this.toolStripSystemStatus.Name = "toolStripSystemStatus";
			this.toolStripSystemStatus.Size = new Size(125, 17);
			this.toolStripSystemStatus.Text = "系统状态：初始化完成";
			this.toolStripStatusEnvironment.Name = "toolStripStatusEnvironment";
			this.toolStripStatusEnvironment.Size = new Size(116, 17);
			this.toolStripStatusEnvironment.Text = "交易环境：模拟环境";
			this.user.AutoSize = false;
			this.user.Name = "user";
			this.user.Size = new Size(200, 17);
			this.user.Text = "登录用户：";
			this.status.AutoSize = false;
			this.status.BackColor = Color.Lime;
			this.status.DoubleClickEnabled = true;
			this.status.Name = "status";
			this.status.Size = new Size(35, 17);
			this.status.Text = "连接";
			this.time.AutoSize = false;
			this.time.Name = "time";
			this.time.Size = new Size(140, 17);
			this.time.Text = "HH:MM:SS";
			this.dataGridView3.AllowUserToAddRows = false;
			this.dataGridView3.AllowUserToDeleteRows = false;
			this.dataGridView3.AllowUserToResizeColumns = false;
			this.dataGridView3.AllowUserToResizeRows = false;
			dataGridViewCellStyle34.BackColor = SystemColors.ActiveCaptionText;
			this.dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle34;
			this.dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
			dataGridViewCellStyle35.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle35.BackColor = SystemColors.Control;
			dataGridViewCellStyle35.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle35.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle35.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle35.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle35.WrapMode = DataGridViewTriState.True;
			this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle35;
			this.dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle36.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle36.BackColor = SystemColors.Window;
			dataGridViewCellStyle36.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle36.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle36.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle36.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle36.WrapMode = DataGridViewTriState.False;
			this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle36;
			this.dataGridView3.Dock = DockStyle.Fill;
			this.dataGridView3.Location = new Point(3, 17);
			this.dataGridView3.Name = "dataGridView3";
			dataGridViewCellStyle37.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle37.BackColor = SystemColors.Control;
			dataGridViewCellStyle37.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle37.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle37.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle37.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle37.WrapMode = DataGridViewTriState.True;
			this.dataGridView3.RowHeadersDefaultCellStyle = dataGridViewCellStyle37;
			this.dataGridView3.RowHeadersVisible = false;
			this.dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView3.RowTemplate.Height = 23;
			this.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView3.Size = new Size(951, 95);
			this.dataGridView3.TabIndex = 1;
			this.contextMenuStripHQ.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemSO,
				this.toolStripMenuItemSC,
				this.toolStripSeparator5,
				this.toolStripMenuItemXO,
				this.toolStripSeparator6,
				this.toolStripMenuItemHQCancel
			});
			this.contextMenuStripHQ.Name = "contextMenuStripHQ";
			this.contextMenuStripHQ.Size = new Size(137, 104);
			this.toolStripMenuItemSO.Name = "toolStripMenuItemSO";
			this.toolStripMenuItemSO.Size = new Size(136, 22);
			this.toolStripMenuItemSO.Text = "市价建仓单";
			this.toolStripMenuItemSO.Click += new EventHandler(this.toolStripMenuItemSO_Click);
			this.toolStripMenuItemSC.Name = "toolStripMenuItemSC";
			this.toolStripMenuItemSC.Size = new Size(136, 22);
			this.toolStripMenuItemSC.Text = "市价平仓单";
			this.toolStripMenuItemSC.Click += new EventHandler(this.toolStripMenuItemSC_Click);
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new Size(133, 6);
			this.toolStripMenuItemXO.Name = "toolStripMenuItemXO";
			this.toolStripMenuItemXO.Size = new Size(136, 22);
			this.toolStripMenuItemXO.Click += new EventHandler(this.toolStripMenuItemXO_Click);
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new Size(133, 6);
			this.toolStripMenuItemHQCancel.Name = "toolStripMenuItemHQCancel";
			this.toolStripMenuItemHQCancel.Size = new Size(136, 22);
			this.toolStripMenuItemHQCancel.Text = "取消";
			this.toolStripMenuItemHQCancel.Click += new EventHandler(this.toolStripMenuItemHQCancel_Click);
			this.contextMenuStripXJ.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemWithdrawOrder,
				this.toolStripSeparator7,
				this.toolStripMenuItemXJRefresh,
				this.toolStripSeparator8,
				this.toolStripMenuItemXJCancel
			});
			this.contextMenuStripXJ.Name = "contextMenuStripXJ";
			this.contextMenuStripXJ.Size = new Size(101, 82);
			this.contextMenuStripXJ.Closed += new ToolStripDropDownClosedEventHandler(this.contextMenuStripXJ_Closed);
			this.toolStripMenuItemWithdrawOrder.Name = "toolStripMenuItemWithdrawOrder";
			this.toolStripMenuItemWithdrawOrder.Size = new Size(100, 22);
			this.toolStripMenuItemWithdrawOrder.Click += new EventHandler(this.toolStripMenuItemWithdrawOrder_Click);
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new Size(97, 6);
			this.toolStripMenuItemXJRefresh.Name = "toolStripMenuItemXJRefresh";
			this.toolStripMenuItemXJRefresh.Size = new Size(100, 22);
			this.toolStripMenuItemXJRefresh.Text = "刷新";
			this.toolStripMenuItemXJRefresh.Click += new EventHandler(this.toolStripMenuItemXJRefresh_Click);
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new Size(97, 6);
			this.toolStripMenuItemXJCancel.Name = "toolStripMenuItemXJCancel";
			this.toolStripMenuItemXJCancel.Size = new Size(100, 22);
			this.toolStripMenuItemXJCancel.Text = "取消";
			this.toolStripMenuItemXJCancel.Click += new EventHandler(this.toolStripMenuItemXJCancel_Click);
			this.contextMenuStripF7.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemF7Refresh
			});
			this.contextMenuStripF7.Name = "contextMenuStripF7";
			this.contextMenuStripF7.Size = new Size(101, 26);
			this.toolStripMenuItemF7Refresh.Name = "toolStripMenuItemF7Refresh";
			this.toolStripMenuItemF7Refresh.Size = new Size(100, 22);
			this.toolStripMenuItemF7Refresh.Text = "刷新";
			this.toolStripMenuItemF7Refresh.Click += new EventHandler(this.toolStripMenuItemF7Refresh_Click);
			this.contextMenuStripF6.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemF6SP,
				this.toolStripMenuItem3,
				this.toolStripMenuItemF6Refresh
			});
			this.contextMenuStripF6.Name = "contextMenuStripF6";
			this.contextMenuStripF6.Size = new Size(137, 54);
			this.contextMenuStripF6.Closed += new ToolStripDropDownClosedEventHandler(this.contextMenuStripF6_Closed);
			this.toolStripMenuItemF6SP.Name = "toolStripMenuItemF6SP";
			this.toolStripMenuItemF6SP.Size = new Size(136, 22);
			this.toolStripMenuItemF6SP.Text = "市价平仓单";
			this.toolStripMenuItemF6SP.Click += new EventHandler(this.toolStripMenuItemF6SP_Click);
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new Size(133, 6);
			this.toolStripMenuItemF6Refresh.Name = "toolStripMenuItemF6Refresh";
			this.toolStripMenuItemF6Refresh.Size = new Size(136, 22);
			this.toolStripMenuItemF6Refresh.Text = "刷新";
			this.toolStripMenuItemF6Refresh.Click += new EventHandler(this.toolStripMenuItemF6Refresh_Click);
			this.splitContainerAll.BorderStyle = BorderStyle.FixedSingle;
			this.splitContainerAll.Dock = DockStyle.Fill;
			this.splitContainerAll.Location = new Point(0, 0);
			this.splitContainerAll.Margin = new Padding(4);
			this.splitContainerAll.Name = "splitContainerAll";
			this.splitContainerAll.Panel1.Controls.Add(this.splitContainer1);
			this.splitContainerAll.Panel2.AutoScroll = true;
			this.splitContainerAll.Panel2.BackColor = Color.White;
			this.splitContainerAll.Panel2.Resize += new EventHandler(this.splitContainerAll_Panel2_Resize);
			this.splitContainerAll.Panel2Collapsed = true;
			this.splitContainerAll.Size = new Size(1020, 720);
			this.splitContainerAll.SplitterDistance = 995;
			this.splitContainerAll.SplitterWidth = 6;
			this.splitContainerAll.TabIndex = 7;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(1020, 742);
			base.Controls.Add(this.MessageInfo);
			base.Controls.Add(this.splitContainerAll);
			base.Controls.Add(this.statusInfo);
			this.Font = new Font("宋体", 9.5f);
			base.KeyPreview = true;
			base.Margin = new Padding(4);
			base.Name = "TMainForm";
			this.Text = "TMainForm";
			base.Load += new EventHandler(this.TMainForm_Load);
			base.Shown += new EventHandler(this.TMainForm_Shown);
			base.VisibleChanged += new EventHandler(this.TMainForm_VisibleChanged);
			base.Paint += new PaintEventHandler(this.TMainForm_Paint);
			base.KeyUp += new KeyEventHandler(this.TMainForm_KeyUp);
			base.Resize += new EventHandler(this.TMainForm_Resize);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainerHQ.Panel1.ResumeLayout(false);
			this.splitContainerHQ.Panel1.PerformLayout();
			this.splitContainerHQ.ResumeLayout(false);
			((ISupportInitialize)this.HQ_DataGrid).EndInit();
			this.panelLock.ResumeLayout(false);
			this.panelLock.PerformLayout();
			this.tabTMain.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.gbHoldingDetailInfo.ResumeLayout(false);
			((ISupportInitialize)this.dgvHoldingDetailInfo).EndInit();
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.splitContainer5.Panel1.ResumeLayout(false);
			this.splitContainer5.Panel2.ResumeLayout(false);
			this.splitContainer5.ResumeLayout(false);
			this.gbOrderInfo.ResumeLayout(false);
			((ISupportInitialize)this.dgvOrderInfo).EndInit();
			this.gbFirmInfo.ResumeLayout(false);
			((ISupportInitialize)this.dgvFirmInfo).EndInit();
			this.splitContainer6.Panel1.ResumeLayout(false);
			this.splitContainer6.ResumeLayout(false);
			this.gbCustomerOrderF10.ResumeLayout(false);
			((ISupportInitialize)this.dgvCustomerOrderF10).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.gbOrderInfoF3.ResumeLayout(false);
			this.gbOrderInfoF3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((ISupportInitialize)this.dgvOrderInfoF3).EndInit();
			((ISupportInitialize)this.dataGridView2).EndInit();
			((ISupportInitialize)this.dataGridView1).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.gbTradeInfo.ResumeLayout(false);
			this.gbTradeInfo.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((ISupportInitialize)this.dvgTradeInfo).EndInit();
			this.tabPage4.ResumeLayout(false);
			this.gbHoldingDetailInfoF5.ResumeLayout(false);
			((ISupportInitialize)this.dgvHoldingDetailInfoF5).EndInit();
			this.tabPage5.ResumeLayout(false);
			this.gbHoldingInfoF6.ResumeLayout(false);
			((ISupportInitialize)this.dgvHoldingInfoF6).EndInit();
			this.tabPage6.ResumeLayout(false);
			this.gbqFirmInfo.ResumeLayout(false);
			((ISupportInitialize)this.dgvqFirmInfo).EndInit();
			this.tabPage7.ResumeLayout(false);
			this.gbCommodityInfo.ResumeLayout(false);
			this.gbCommodityInfo.PerformLayout();
			((ISupportInitialize)this.dgvCommodityInfo).EndInit();
			this.tabPage8.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.dvgYJInfoF9).EndInit();
			this.tabPage10.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.dgvCustomerOrderF10_2).EndInit();
			this.contextMenuStripFirmInfo.ResumeLayout(false);
			this.tabPage9.ResumeLayout(false);
			this.splitContainer4.ResumeLayout(false);
			this.contextMenuStripF9YJ.ResumeLayout(false);
			this.contextMenuStripHoldingDetail.ResumeLayout(false);
			this.statusInfo.ResumeLayout(false);
			this.statusInfo.PerformLayout();
			((ISupportInitialize)this.dataGridView3).EndInit();
			this.contextMenuStripHQ.ResumeLayout(false);
			this.contextMenuStripXJ.ResumeLayout(false);
			this.contextMenuStripF7.ResumeLayout(false);
			this.contextMenuStripF6.ResumeLayout(false);
			this.splitContainerAll.Panel1.ResumeLayout(false);
			this.splitContainerAll.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		private void DelegateLoadCOF10_2()
		{
			try
			{
				if (!this.UpdateMemberFundPriceflag)
				{
					this.UpdateMemberFundPriceflag = true;
					new FirmFundsInfoResponseVO();
					this.callbackFirmFundsF10DataGrid = new TMainForm.CallbackFirmFundsF10DataGrid(this.FillFirmFundsDataGridF10);
					ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
					if (this.DictionarySemaphore.ContainsKey("DelegateLoadCOF10_2"))
					{
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCOF10_2"];
						threadPoolParameter.obj = null;
					}
					else
					{
						this.DictionarySemaphore.Add("DelegateLoadCOF10_2", new AutoResetEvent(true));
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCOF10_2"];
						threadPoolParameter.obj = null;
					}
					WaitCallback callBack = new WaitCallback(this.QueryFirmFundsF10);
					ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryFirmFundsF10(object _FirmFunds)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (_FirmFunds != null)
				{
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_FirmFunds;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					DataSet firmHoldSumQuery = this.dataProcess.GetFirmHoldSumQuery(Global.UserID);
					this.HandleCreated();
					base.BeginInvoke(this.callbackFirmFundsF10DataGrid, new object[]
					{
						firmHoldSumQuery
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.UpdateMemberFundPriceflag = false;
			}
		}
		private void FillFirmFundsDataGridF10(DataSet dt)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvCustomerOrderF10_2.SelectedCells.Count != 0)
				{
					num = this.dgvCustomerOrderF10_2.SelectedCells[0].RowIndex;
				}
				if (this.dgvCustomerOrderF10_2.SortedColumn != null)
				{
					text = this.dgvCustomerOrderF10_2.SortedColumn.Name;
					SortOrder sortOrder = this.dgvCustomerOrderF10_2.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				Logger.wirte(1, "FillFirmFundsDataGridF10线程启动 1");
				if (dt != null)
				{
					DataView dataView = new DataView(dt.Tables["tFirmHoldSumQuery"]);
					lock (this.dgvCustomerOrderF10_2)
					{
						this.dgvCustomerOrderF10_2.DataSource = dataView.Table;
					}
					Logger.wirte(1, "FillFirmFundsDataGridF10线程 2");
				}
				this.SetColumnText();
				Logger.wirte(1, "FillFirmFundsDataGridF10线程 3");
				this.groupBox2.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "GB_FIRMFUNDS"));
				if (this.dgvCustomerOrderF10_2.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvCustomerOrderF10_2.Sort(this.dgvCustomerOrderF10_2.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvCustomerOrderF10_2.Rows.Count)
					{
						this.dgvCustomerOrderF10_2.CurrentCell = this.dgvCustomerOrderF10_2.Rows[num].Cells[1];
					}
					else
					{
						this.dgvCustomerOrderF10_2.ClearSelection();
					}
				}
				Logger.wirte(1, "FillFirmFundsDataGridF10线程 4");
				this.UpdateMemberFundPriceflag = false;
				SystemStatus currentSystemStatus;
				lock (this._CurrentSystemStatusObject)
				{
					currentSystemStatus = this._CurrentSystemStatus;
				}
				if (currentSystemStatus != SystemStatus.SettlementComplete)
				{
					this.UpdateMemberFundPrice();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "FillFirmFundsDataGridF10线程结束");
		}
		private void InvokeUpdateMemberFundPrice()
		{
			try
			{
				if (!this.UpdateMemberFundPriceflag)
				{
					this.UpdateMemberFundPriceflag = true;
					DataTable dataTable = null;
					lock (this.LockDIDataTable)
					{
						dataTable = this._HDIDataTable.Tables["HDIDetatable"].Copy();
					}
					if (dataTable == null)
					{
						this.UpdateMemberFundPriceflag = false;
					}
					else
					{
						Hashtable hashtable = null;
						lock (this._CustomerOrderHashtable)
						{
							hashtable = (Hashtable)this._CustomerOrderHashtable.Clone();
						}
						int i = 0;
						while (i < this.dgvCustomerOrderF10_2.RowCount)
						{
							string text = this.dgvCustomerOrderF10_2.Rows[i].Cells["CommodityID"].Value.ToString();
							double num = 0.0;
							try
							{
								Convert.ToInt64(this.dgvCustomerOrderF10_2.Rows[i].Cells["CustomerJingTouCun"].Value);
							}
							catch
							{
								goto IL_3AD;
							}
							goto Block_8;
							IL_3AD:
							i++;
							continue;
							Block_8:
							try
							{
								num = Convert.ToDouble(this.dgvCustomerOrderF10_2.Rows[i].Cells["CustomerTradeFloating"].Value);
							}
							catch
							{
							}
							try
							{
								Convert.ToInt64(this.dgvCustomerOrderF10_2.Rows[i].Cells["DuiChongJingTouCun"].Value);
							}
							catch
							{
							}
							double num2;
							try
							{
								num2 = Convert.ToDouble(this.dgvCustomerOrderF10_2.Rows[i].Cells["DuiChongFloating"].Value);
							}
							catch
							{
							}
							try
							{
								Convert.ToInt64(this.dgvCustomerOrderF10_2.Rows[i].Cells["MemberJingTouCun"].Value);
							}
							catch
							{
							}
							double num3;
							try
							{
								num3 = Convert.ToDouble(this.dgvCustomerOrderF10_2.Rows[i].Cells["HoldingNetFloating"].Value);
							}
							catch
							{
							}
							string text2 = string.Format(" CommodityID='{0}' and BuySell='{1}' ", text, BuySell.Buy.ToString("d"));
							string text3 = string.Format(" CommodityID='{0}' and BuySell='{1}' ", text, BuySell.Sell.ToString("d"));
							double num4 = 0.0;
							double num5 = 0.0;
							long num6 = 0L;
							long num7 = 0L;
							if (dataTable.Select(text2).Length > 0)
							{
								double.TryParse(dataTable.Compute("Sum(FloatingPrice)", text2).ToString(), out num4);
								long.TryParse(dataTable.Compute("Sum(HoldingQuantity)", text2).ToString(), out num6);
							}
							if (dataTable.Select(text3).Length > 0)
							{
								double.TryParse(dataTable.Compute("Sum(FloatingPrice)", text3).ToString(), out num5);
								long.TryParse(dataTable.Compute("Sum(HoldingQuantity)", text3).ToString(), out num7);
							}
							num2 = num5 + num4;
							if (hashtable.ContainsKey(text))
							{
								num = -Convert.ToDouble(hashtable[text]);
							}
							num3 = num2 + num;
							new DataGridViewCellStyle();
							lock (this.dgvCustomerOrderF10_2)
							{
								this.dgvCustomerOrderF10_2.Rows[i].Cells["CustomerTradeFloating"].Value = num.ToString("n2");
								this.dgvCustomerOrderF10_2.Rows[i].Cells["DuiChongFloating"].Value = num2.ToString("n2");
								this.dgvCustomerOrderF10_2.Rows[i].Cells["HoldingNetFloating"].Value = num3.ToString("n2");
							}
							goto IL_3AD;
						}
						this.UpdateMemberFundPriceflag = false;
					}
				}
			}
			catch (Exception ex)
			{
				this.UpdateMemberFundPriceflag = false;
				Logger.wirte(ex);
			}
			finally
			{
				this.UpdateMemberFundPriceflag = false;
			}
		}
		private Color GetColumnColor(object number)
		{
			Color result;
			try
			{
				if (Convert.ToDouble(number) > 0.0)
				{
					result = Color.Red;
				}
				else if (Convert.ToDouble(number) < 0.0)
				{
					result = Color.Green;
				}
				else
				{
					result = Color.Black;
				}
			}
			catch (Exception)
			{
				result = Color.Black;
			}
			return result;
		}
		private void UpdateMemberFundPrice()
		{
			this.callbackUpdateMemberFundPrice = new TMainForm.CallbackUpdateMemberFundPrice(this.InvokeUpdateMemberFundPrice);
			this.HandleCreated();
			base.BeginInvoke(this.callbackUpdateMemberFundPrice, new object[0]);
		}
		private void SetColumnText()
		{
			try
			{
				this.dgvCustomerOrderF10_2.Columns["CommodityID"].HeaderText = "1111";
				this.dgvCustomerOrderF10_2.Columns["CommodityID"].Visible = false;
				this.dgvCustomerOrderF10_2.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_COMMODITYNAME"));
				this.dgvCustomerOrderF10_2.Columns["CommodityName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvCustomerOrderF10_2.Columns["MaxHolding"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_MAX_HOLD"));
				this.dgvCustomerOrderF10_2.Columns["MaxHolding"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["MemberJingTouCun"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_MEMBERJINGTOUCUN"));
				this.dgvCustomerOrderF10_2.Columns["MemberJingTouCun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["CustomerJingTouCun"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_CUSTOMERJINGTOUCUN"));
				this.dgvCustomerOrderF10_2.Columns["CustomerJingTouCun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["DuiChongJingTouCun"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_DUICHONGJINGTOUCUN"));
				this.dgvCustomerOrderF10_2.Columns["DuiChongJingTouCun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["HoldingNetFloating"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_HOLDINGNETFLOATING"));
				this.dgvCustomerOrderF10_2.Columns["HoldingNetFloating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["HoldingNetFloating"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10_2.Columns["CustomerTradeFloating"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_CUSTOMERTRADEFLOATING"));
				this.dgvCustomerOrderF10_2.Columns["CustomerTradeFloating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["CustomerTradeFloating"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10_2.Columns["DuiChongFloating"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FH_DUICHONGFLOATING"));
				this.dgvCustomerOrderF10_2.Columns["DuiChongFloating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10_2.Columns["DuiChongFloating"].DefaultCellStyle.Format = "n2";
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvCustomerOrderF10_2_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					this.dgvCustomerOrderF10_2.ClearSelection();
					Point position = this.dgvCustomerOrderF10_2.PointToClient(Cursor.Position);
					this.SetMenuDisenable("contextMenuStripF7");
					this.contextMenuStripF7.Show(this.dgvCustomerOrderF10_2, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvCustomerOrderF10_2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				DataGridView dataGridView = (DataGridView)sender;
				object arg_0D_0 = e.Value;
				lock (this.dgvCustomerOrderF10_2)
				{
					if (dataGridView != null && dataGridView.RowCount > 0)
					{
						if (dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("CustomerJingTouCun") || dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("DuiChongJingTouCun") || dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("CustomerTradeFloating") || dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("DuiChongFloating") || dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("HoldingNetFloating"))
						{
							e.CellStyle.ForeColor = this.GetColumnColor(e.Value);
						}
						else if (dataGridView.Columns[e.ColumnIndex].DataPropertyName.Equals("MemberJingTouCun"))
						{
							e.CellStyle.ForeColor = this.GetColumnColor(e.Value);
							e.CellStyle.BackColor = Color.Yellow;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		[DllImport("winmm.dll")]
		public static extern bool PlaySound(string pszSound, int hmod, int fdwSound);
		private void DelegateLoadCIF9()
		{
			if (this.dtyj.Columns.Count == 0)
			{
				this.dataviewcolset();
			}
			this.getdataview();
		}
		private void dataviewcolset()
		{
			try
			{
				DataColumn column = new DataColumn("HH");
				DataColumn column2 = new DataColumn("ID", typeof(int));
				DataColumn column3 = new DataColumn("YJLX", typeof(string));
				DataColumn column4 = new DataColumn("ZJZH", typeof(string));
				DataColumn column5 = new DataColumn("YJX");
				DataColumn column6 = new DataColumn("YJTJ", typeof(string));
				DataColumn column7 = new DataColumn("YJFS", typeof(string));
				DataColumn column8 = new DataColumn("ZJCFSJ", typeof(string));
				DataColumn column9 = new DataColumn("YJYXSD", typeof(string));
				DataColumn column10 = new DataColumn("SFYX", typeof(string));
				DataColumn column11 = new DataColumn("XDSJD", typeof(string));
				DataColumn column12 = new DataColumn("CFCS", typeof(string));
				DataColumn column13 = new DataColumn("SFYXVALUE", typeof(string));
				this.dtyj.Columns.Add(column);
				this.dtyj.Columns.Add(column2);
				this.dtyj.Columns.Add(column3);
				this.dtyj.Columns.Add(column4);
				this.dtyj.Columns.Add(column5);
				this.dtyj.Columns.Add(column6);
				this.dtyj.Columns.Add(column7);
				this.dtyj.Columns.Add(column8);
				this.dtyj.Columns.Add(column9);
				this.dtyj.Columns.Add(column10);
				this.dtyj.Columns.Add(column11);
				this.dtyj.Columns.Add(column12);
				this.dtyj.Columns.Add(column13);
				this.dvgYJInfoF9.DataSource = this.dtyj;
				DataGridViewCellStyle defaultCellStyle = this.dvgYJInfoF9.Columns["HH"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["HH"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["HH"].HeaderText = "序号";
				this.dvgYJInfoF9.Columns["HH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				this.dvgYJInfoF9.Columns["ID"].Visible = false;
				defaultCellStyle = this.dvgYJInfoF9.Columns["YJLX"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["YJLX"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["YJLX"].HeaderText = "预警类型";
				this.dvgYJInfoF9.Columns["YJLX"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["ZJZH"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["ZJZH"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["ZJZH"].HeaderText = "资金账号";
				this.dvgYJInfoF9.Columns["ZJZH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				defaultCellStyle = this.dvgYJInfoF9.Columns["YJX"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["YJX"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["YJX"].HeaderText = "预警项";
				this.dvgYJInfoF9.Columns["YJX"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["YJTJ"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["YJTJ"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["YJTJ"].HeaderText = "预警条件";
				this.dvgYJInfoF9.Columns["YJTJ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["YJFS"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["YJFS"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["YJFS"].HeaderText = "预警方式";
				this.dvgYJInfoF9.Columns["YJFS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["ZJCFSJ"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["ZJCFSJ"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["ZJCFSJ"].HeaderText = "最近触发时间";
				this.dvgYJInfoF9.Columns["ZJCFSJ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["YJYXSD"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["YJYXSD"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["YJYXSD"].HeaderText = "预警有效时段";
				this.dvgYJInfoF9.Columns["YJYXSD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				defaultCellStyle = this.dvgYJInfoF9.Columns["SFYX"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["SFYX"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["SFYX"].HeaderText = "是否有效";
				this.dvgYJInfoF9.Columns["SFYX"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				defaultCellStyle = this.dvgYJInfoF9.Columns["XDSJD"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["XDSJD"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["XDSJD"].HeaderText = "限定时间段";
				this.dvgYJInfoF9.Columns["XDSJD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				defaultCellStyle = this.dvgYJInfoF9.Columns["CFCS"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgYJInfoF9.Columns["CFCS"].DefaultCellStyle = defaultCellStyle;
				this.dvgYJInfoF9.Columns["CFCS"].HeaderText = "重复次数";
				this.dvgYJInfoF9.Columns["CFCS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
				this.dvgYJInfoF9.Columns["SFYXVALUE"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void getdataview()
		{
			try
			{
				TMainForm._CreateXml = new CreateXmlFile();
				TMainForm._CreateXml.CreateFile(this.YJFileName);
				TMainForm._XmlYJView = new XmlDataSet(this.YJFileName);
				DataTable dataTable = new DataTable();
				this.dtyj.Clear();
				dataTable = TMainForm._XmlYJView.GetDataSetByXml().Tables[0];
				int count = dataTable.Rows.Count;
				if (this.dataProcess.IsAgency)
				{
					int i = 0;
					while (i < count)
					{
						if (!(dataTable.Rows[i]["YJLX"].ToString() == "0"))
						{
							goto IL_DD;
						}
						if (Global.AgencyCommodityData != null)
						{
							if (Global.AgencyCommodityData.ContainsKey(dataTable.Rows[i]["YJX"].ToString()))
							{
								goto IL_DD;
							}
							TMainForm._XmlYJView.DeleteXmlRowByIndex(i);
						}
						IL_57A:
						i++;
						continue;
						IL_DD:
						DataRow dataRow = this.dtyj.NewRow();
						dataRow["HH"] = (i + 1).ToString();
						dataRow["ID"] = dataTable.Rows[i]["ID"].ToString();
						dataRow["YJLX"] = this.getcolview("YJLX", dataTable.Rows[i]["YJLX"].ToString());
						dataRow["ZJZH"] = "贵金属账户";
						if (this.dataProcess.IsAgency)
						{
							if (dataTable.Rows[i]["YJLX"].ToString() == "0")
							{
								dataRow["YJX"] = Global.AgencyCommodityData[dataTable.Rows[i]["YJX"].ToString().Trim()].CommodityName.ToString();
								dataRow["YJTJ"] = this.getcolview("YJTJ", dataTable.Rows[i]["YJTJ"].ToString()) + dataTable.Rows[i]["YJFZ"].ToString();
							}
							else
							{
								dataRow["YJX"] = this.getcolview("YJX", Convert.ToInt16(dataTable.Rows[i]["YJX"]).ToString());
								dataRow["YJTJ"] = this.getcolview("YJTJ", ((int)(Convert.ToInt16(dataTable.Rows[i]["YJTJ"]) + 6)).ToString()) + dataTable.Rows[i]["YJFZ"].ToString();
							}
						}
						else if (dataTable.Rows[i]["YJLX"].ToString() == "0")
						{
							dataRow["YJX"] = Global.AgencyCommodityData[dataTable.Rows[i]["YJX"].ToString().Trim()].CommodityName.ToString();
							dataRow["YJTJ"] = this.getcolview("YJTJ", dataTable.Rows[i]["YJTJ"].ToString()) + dataTable.Rows[i]["YJFZ"].ToString();
						}
						else
						{
							dataRow["YJX"] = this.getcolview("YJX", Convert.ToInt16(dataTable.Rows[i]["YJX"]).ToString());
							dataRow["YJTJ"] = this.getcolview("YJTJ", ((int)(Convert.ToInt16(dataTable.Rows[i]["YJTJ"]) + 6)).ToString()) + dataTable.Rows[i]["YJFZ"].ToString();
						}
						dataRow["YJFS"] = this.getcolview("YJFS", dataTable.Rows[i]["YJFS"].ToString());
						dataRow["YJYXSD"] = dataTable.Rows[i]["YJYXSD"].ToString();
						dataRow["SFYXVALUE"] = dataTable.Rows[i]["SFYX"].ToString();
						if (dataTable.Rows[i]["SFYX"].ToString() == "Y")
						{
							dataRow["SFYX"] = "是";
						}
						else
						{
							dataRow["SFYX"] = "否";
						}
						dataRow["XDSJD"] = this.getcolview("XDSJD", dataTable.Rows[i]["XDSJD"].ToString());
						dataRow["CFCS"] = dataTable.Rows[i]["CFCS"].ToString();
						dataRow["ZJCFSJ"] = dataTable.Rows[i]["ZJCFSJ"].ToString();
						this.dtyj.Rows.Add(dataRow);
						goto IL_57A;
					}
				}
				else
				{
					int j = 0;
					while (j < count)
					{
						if (!(dataTable.Rows[j]["YJLX"].ToString() == "0"))
						{
							goto IL_5FE;
						}
						if (Global.CommodityData != null)
						{
							if (Global.CommodityData.ContainsKey(dataTable.Rows[j]["YJX"].ToString()))
							{
								goto IL_5FE;
							}
							TMainForm._XmlYJView.DeleteXmlRowByIndex(j);
						}
						IL_AB3:
						j++;
						continue;
						IL_5FE:
						DataRow dataRow = this.dtyj.NewRow();
						dataRow["HH"] = (j + 1).ToString();
						dataRow["ID"] = dataTable.Rows[j]["ID"].ToString();
						dataRow["YJLX"] = this.getcolview("YJLX", dataTable.Rows[j]["YJLX"].ToString());
						dataRow["ZJZH"] = "贵金属账户";
						if (this.dataProcess.IsAgency)
						{
							if (dataTable.Rows[j]["YJLX"].ToString() == "0")
							{
								dataRow["YJX"] = Global.CommodityData[dataTable.Rows[j]["YJX"].ToString().Trim()].CommodityName.ToString();
								dataRow["YJTJ"] = this.getcolview("YJTJ", dataTable.Rows[j]["YJTJ"].ToString()) + dataTable.Rows[j]["YJFZ"].ToString();
							}
							else
							{
								dataRow["YJX"] = this.getcolview("YJX", Convert.ToInt16(dataTable.Rows[j]["YJX"]).ToString());
								dataRow["YJTJ"] = this.getcolview("YJTJ", ((int)(Convert.ToInt16(dataTable.Rows[j]["YJTJ"]) + 6)).ToString()) + dataTable.Rows[j]["YJFZ"].ToString();
							}
						}
						else if (dataTable.Rows[j]["YJLX"].ToString() == "0")
						{
							dataRow["YJX"] = Global.CommodityData[dataTable.Rows[j]["YJX"].ToString().Trim()].CommodityName.ToString();
							dataRow["YJTJ"] = this.getcolview("YJTJ", dataTable.Rows[j]["YJTJ"].ToString()) + dataTable.Rows[j]["YJFZ"].ToString();
						}
						else
						{
							dataRow["YJX"] = this.getcolview("YJX", Convert.ToInt16(dataTable.Rows[j]["YJX"]).ToString());
							dataRow["YJTJ"] = this.getcolview("YJTJ", ((int)(Convert.ToInt16(dataTable.Rows[j]["YJTJ"]) + 6)).ToString()) + dataTable.Rows[j]["YJFZ"].ToString();
						}
						dataRow["YJFS"] = this.getcolview("YJFS", dataTable.Rows[j]["YJFS"].ToString());
						dataRow["YJYXSD"] = dataTable.Rows[j]["YJYXSD"].ToString();
						dataRow["SFYXVALUE"] = dataTable.Rows[j]["SFYX"].ToString();
						if (dataTable.Rows[j]["SFYX"].ToString() == "Y")
						{
							dataRow["SFYX"] = "是";
						}
						else
						{
							dataRow["SFYX"] = "否";
						}
						dataRow["XDSJD"] = this.getcolview("XDSJD", dataTable.Rows[j]["XDSJD"].ToString());
						dataRow["CFCS"] = dataTable.Rows[j]["CFCS"].ToString();
						dataRow["ZJCFSJ"] = dataTable.Rows[j]["ZJCFSJ"].ToString();
						this.dtyj.Rows.Add(dataRow);
						goto IL_AB3;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private string getcolview(string colint, string colvalue)
		{
			string result = "";
			string[] array = new string[0];
			try
			{
				if (colint != null)
				{
					if (!(colint == "YJLX"))
					{
						if (!(colint == "YJX"))
						{
							if (!(colint == "YJTJ"))
							{
								if (!(colint == "YJFS"))
								{
									if (!(colint == "XDSJD"))
									{
										if (colint == "CFCS")
										{
											array = new string[]
											{
												"1",
												"3",
												"5",
												"10",
												"50",
												"1000"
											};
											result = array[(int)Convert.ToInt16(colvalue)];
										}
									}
									else
									{
										array = new string[]
										{
											"10sec",
											"30sec",
											"60sec",
											"3min",
											"5min",
											"15min",
											"30min",
											"1hour"
										};
										result = array[(int)Convert.ToInt16(colvalue)];
									}
								}
								else
								{
									if (colvalue.Substring(0, 1) == "0")
									{
										result = "弹出窗口";
									}
									if (colvalue.Substring(1, 1) == "0" && colvalue.Substring(0, 1) == "0")
									{
										result = "弹出窗口/发出声音";
									}
									else if (colvalue.Substring(1, 1) == "0")
									{
										result = "发出声音";
									}
								}
							}
							else
							{
								array = new string[]
								{
									"买价>",
									"买价=",
									"买价<",
									"卖价>",
									"卖价=",
									"卖价<",
									">",
									"=",
									"<"
								};
								result = array[(int)Convert.ToInt16(colvalue)];
							}
						}
						else
						{
							if (this.dataProcess.sIdentity == Identity.Member)
							{
								array = new string[]
								{
									"风险值",
									"可用保证金",
									"会员持有净浮动盈亏"
								};
							}
							else
							{
								array = new string[]
								{
									"风险值",
									"当前权益",
									"可用保证金",
									"总浮动动盈亏"
								};
							}
							result = array[(int)Convert.ToInt16(colvalue)];
						}
					}
					else
					{
						if (this.dataProcess.sIdentity == Identity.Member)
						{
							array = new string[]
							{
								"行情预警",
								"风险预警",
								"可用保证金预警",
								"会员持有净浮动盈亏"
							};
						}
						else
						{
							array = new string[]
							{
								"行情预警",
								"风险预警",
								"当前权益预警",
								"保证金预警",
								"总浮动盈亏预警"
							};
						}
						result = array[(int)Convert.ToInt16(colvalue)];
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return result;
		}
		private void DelDataYJ()
		{
			try
			{
				MessageForm messageForm = new MessageForm("提示信息", "您确定要删除当前设置吗", 0, StatusBarType.Message);
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (messageForm.isOK)
				{
					if (!TMainForm._XmlYJView.DeleteXmlRowByIndex(this._YuJingNum))
					{
						MessageForm messageForm2 = new MessageForm("提示信息", "删除数据失败", 1, StatusBarType.Error);
						messageForm2.ShowDialog();
						messageForm2.Dispose();
					}
					this.DelegateLoadCIF9();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UPDateYJ()
		{
			this.FromYJSZ = new TMainYJSZ(this);
			this.FromYJSZ.TYJSZHH = (int)Convert.ToInt16(this.dvgYJInfoF9.Rows[this._YuJingNum].Cells["ID"].Value);
			this.FromYJSZ.TYJSZFLAG = true;
			this.FromYJSZ.ShowDialog();
			this.DelegateLoadCIF9();
			this.dvgYJInfoF9.Rows[this._YuJingNum].Selected = true;
		}
		private void OnOffYJ()
		{
			try
			{
				string[] columns = new string[]
				{
					"SFYX"
				};
				string[] columnValue = new string[0];
				if (this.dvgYJInfoF9.Rows[this._YuJingNum].Cells["SFYXVALUE"].Value.ToString() == "Y")
				{
					columnValue = new string[]
					{
						"N"
					};
				}
				else
				{
					columnValue = new string[]
					{
						"Y"
					};
				}
				if (TMainForm._XmlYJView.UpdateXmlRow(columns, columnValue, "ID", this.dvgYJInfoF9.Rows[this._YuJingNum].Cells["ID"].Value.ToString()))
				{
					this.DelegateLoadCIF9();
				}
				else
				{
					MessageForm messageForm = new MessageForm("提示信息", "更新预警信息失败", 1, StatusBarType.Error);
					messageForm.ShowDialog();
					messageForm.Dispose();
				}
				this.dvgYJInfoF9.Rows[this._YuJingNum].Selected = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dvgYJInfoF9_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
			{
				this.dvgYJInfoF9.ClearSelection();
				this.dvgYJInfoF9.Rows[e.RowIndex].Selected = true;
				this.MODToolStripMenuItemYJF9.Enabled = true;
				this.DELToolStripMenuItemYJF9.Enabled = true;
				this.ONOFFToolStripMenuItemYJF9.Enabled = true;
				this._YJGridContextMenuRowIndex = e.RowIndex;
				this._YuJingNum = e.RowIndex;
				Point position = this.dvgYJInfoF9.PointToClient(Cursor.Position);
				this.contextMenuStripF9YJ.Show(this.dvgYJInfoF9, position);
			}
		}
		private void dvgYJInfoF9_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.dvgYJInfoF9.ClearSelection();
				this.MODToolStripMenuItemYJF9.Enabled = (this._YJGridContextMenuRowIndex >= 0);
				this.DELToolStripMenuItemYJF9.Enabled = (this._YJGridContextMenuRowIndex >= 0);
				this.ONOFFToolStripMenuItemYJF9.Enabled = (this._YJGridContextMenuRowIndex >= 0);
				Point position = this.dvgYJInfoF9.PointToClient(Cursor.Position);
				this.contextMenuStripF9YJ.Show(this.dvgYJInfoF9, position);
			}
		}
		private void contextMenuStripF9YJ_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this._YJGridContextMenuRowIndex = -1;
		}
		private void YuJingRealNotifier()
		{
			try
			{
				if (this.dvgYJInfoF9.Rows.Count != 0)
				{
					if (this.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData == null)
							{
								return;
							}
						}
						if (Global.AgencyCommodityData == null)
						{
							return;
						}
					}
					else
					{
						lock (Global.HQCommDataLock)
						{
							if (Global.HQCommData == null)
							{
								return;
							}
						}
						if (this.dataProcess.IsAgency)
						{
							if (Global.AgencyCommodityData == null)
							{
								return;
							}
						}
						else if (Global.CommodityData == null)
						{
							return;
						}
					}
					this._ServerTime = Global.ServerTime.ToLongTimeString();
					if (this._ServerTime.Length == 7)
					{
						this._ServerTime = '0' + this._ServerTime;
					}
					this._DateTime = Global.ServerTime.ToShortDateString() + " " + this._ServerTime;
					DataTable dataTable = TMainForm._XmlYJView.GetDataViewByXml("", "ID").ToTable();
					int num = 0;
					TMainForm._CreateXml = new CreateXmlFile();
					TMainForm._CreateXml.CreateFile(this.YJFileName);
					TMainForm._XmlYJView = new XmlDataSet(this.YJFileName);
					Dictionary<string, CommData> dictionary = null;
					if (this.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData != null)
							{
								dictionary = Global.gAgencyHQCommData;
							}
							goto IL_18E;
						}
					}
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData != null)
						{
							dictionary = Global.gHQCommData;
						}
					}
					IL_18E:
					foreach (DataRow dataRow in dataTable.Rows)
					{
						DateTime t = Convert.ToDateTime(this._ServerTime);
						DateTime t2 = Convert.ToDateTime(dataRow["YJYXSD"].ToString().Substring(0, 8));
						DateTime t3 = Convert.ToDateTime(dataRow["YJYXSD"].ToString().Substring(10, 8));
						if (DateTime.Compare(t, t2) >= 0 && DateTime.Compare(t, t3) <= 0 && dataRow["SFYX"].ToString() == "Y")
						{
							this._YuJingCurrentRow = dataRow;
							string text = dataRow["YJFZ"].ToString();
							string a;
							if ((a = dataRow["YJLX"].ToString()) != null)
							{
								if (!(a == "0"))
								{
									if (!(a == "1"))
									{
										if (!(a == "2"))
										{
											if (!(a == "3"))
											{
												if (a == "4")
												{
													this.YuJingTypeOfOther("CurrentFL", text, "浮动盈亏", num);
												}
											}
											else if (this.dataProcess.sIdentity == Identity.Member)
											{
												this.YuJingTypeOfOtherMember(2, 3, text, num);
											}
											else
											{
												this.YuJingTypeOfOther("RealFund", text, "可用保证金", num);
											}
										}
										else if (this.dataProcess.sIdentity == Identity.Member)
										{
											this.YuJingTypeOfOtherMember(1, 3, text, num);
										}
										else
										{
											this.YuJingTypeOfOther("InitFund", text, "当前权益", num);
										}
									}
									else if (this.dataProcess.sIdentity == Identity.Member)
									{
										this.YuJingTypeOfOtherMember(6, 3, text, num);
									}
									else
									{
										this.YuJingTypeOfOther("FundRisk", text, "风险", num);
									}
								}
								else
								{
									if (this.dataProcess.IsAgency)
									{
										if (Global.AgencyCommodityData == null || !Global.AgencyCommodityData.ContainsKey(dataRow["YJX"].ToString()))
										{
											continue;
										}
										if (dictionary == null)
										{
											continue;
										}
									}
									else if (Global.CommodityData == null || !Global.CommodityData.ContainsKey(dataRow["YJX"].ToString()) || dictionary == null)
									{
										continue;
									}
									string a2;
									if ((a2 = dataRow["YJTJ"].ToString()) != null)
									{
										if (!(a2 == "0"))
										{
											if (!(a2 == "1"))
											{
												if (!(a2 == "2"))
												{
													if (!(a2 == "3"))
													{
														if (!(a2 == "4"))
														{
															if (a2 == "5")
															{
																if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].SellPrice.ToString()) < Convert.ToDecimal(text))
																{
																	this._dqz = dictionary[dataRow["YJX"].ToString()].SellPrice.ToString();
																	if (this.dataProcess.IsAgency)
																	{
																		this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价小于" + text.ToString();
																	}
																	else
																	{
																		this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价小于" + text.ToString();
																	}
																	this.YuJingCfcs(num);
																}
															}
														}
														else if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].SellPrice.ToString()) == Convert.ToDecimal(text))
														{
															this._dqz = dictionary[dataRow["YJX"].ToString()].SellPrice.ToString();
															if (this.dataProcess.IsAgency)
															{
																this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价等于" + text.ToString();
															}
															else
															{
																this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价等于" + text.ToString();
															}
															this.YuJingCfcs(num);
														}
													}
													else if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].SellPrice.ToString()) > Convert.ToDecimal(text))
													{
														this._dqz = dictionary[dataRow["YJX"].ToString()].SellPrice.ToString();
														if (this.dataProcess.IsAgency)
														{
															this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价大于" + text.ToString();
														}
														else
														{
															this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "卖价大于" + text.ToString();
														}
														this.YuJingCfcs(num);
													}
												}
												else if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString()) < Convert.ToDecimal(text))
												{
													this._dqz = dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString();
													if (this.dataProcess.IsAgency)
													{
														this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "买价小于" + text.ToString();
													}
													else
													{
														this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "买价小于" + text.ToString();
													}
													this.YuJingCfcs(num);
												}
											}
											else if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString()) == Convert.ToDecimal(text))
											{
												this._dqz = dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString();
												if (this.dataProcess.IsAgency)
												{
													this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "买价等于" + text.ToString();
												}
												else
												{
													this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "买价等于" + text.ToString();
												}
												this.YuJingCfcs(num);
											}
										}
										else if (Convert.ToDecimal(dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString()) > Convert.ToDecimal(text))
										{
											this._dqz = dictionary[dataRow["YJX"].ToString()].BuyPrice.ToString();
											if (this.dataProcess.IsAgency)
											{
												this._YuJingMessage = Global.AgencyCommodityData[dataRow["YJX"].ToString()].CommodityName + "买价大于" + text.ToString();
											}
											else
											{
												this._YuJingMessage = Global.CommodityData[dataRow["YJX"].ToString()].CommodityName + "买价大于" + text.ToString();
											}
											this.YuJingCfcs(num);
										}
									}
								}
							}
						}
						num++;
						if (num >= this.dvgYJInfoF9.Rows.Count)
						{
							this._YuJingHH = 1;
						}
						else
						{
							this._YuJingHH = num + 1;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingNotifier()
		{
			try
			{
				if (this != null)
				{
					Environment.CurrentDirectory = Global.CurrentDirectory;
					this.HandleCreated();
					TMainForm.CallbackYuJingInfo method = new TMainForm.CallbackYuJingInfo(this.YuJingRealNotifier);
					base.BeginInvoke(method, new object[0]);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingBind()
		{
			if (this.yujingmessage == null)
			{
				this.yujingmessage = new YuJingMessage();
			}
			this.yujingmessage.BindData();
		}
		private void CallbackYuJingMessage(object state)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.YuJingMessageflag)
				{
					this.YuJingMessageflag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)state;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					this.HandleCreated();
					base.BeginInvoke(this.callbackyujing, new object[0]);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.YuJingMessageflag = false;
			}
		}
		[DllImport("user32.dll")]
		public static extern int MoveWindow(int hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);
		[DllImport("user32.dll ")]
		public static extern int SystemParametersInfo(int uAction, int uParam, ref Rectangle rc, int fuWinIni);
		private void YuJingShowMessage()
		{
			try
			{
				if (this._YuJingCurrentRow["YJFS"].ToString().Substring(0, 1) == "0")
				{
					this.WriteMessage();
					if (!Global.YuJingFlag)
					{
						this.yujingmessage = new YuJingMessage();
						this.yujingmessage.Font = this.SysFont;
						Rectangle rectangle = default(Rectangle);
						TMainForm.SystemParametersInfo(48, 0, ref rectangle, 0);
						Rectangle bounds = this.yujingmessage.Bounds;
						int x = rectangle.Width - bounds.Width;
						int y = rectangle.Height - bounds.Height;
						this.yujingmessage.Show();
						TMainForm.MoveWindow(this.yujingmessage.Handle.ToInt32(), x, y, this.yujingmessage.Width, this.yujingmessage.Height, 1);
					}
					else
					{
						this.callbackyujing = new TMainForm.CallbackYuJing(this.YuJingBind);
						ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
						if (this.DictionarySemaphore.ContainsKey("YuJingShowMessage"))
						{
							threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["YuJingShowMessage"];
							threadPoolParameter.obj = null;
						}
						else
						{
							this.DictionarySemaphore.Add("YuJingShowMessage", new AutoResetEvent(true));
							threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["YuJingShowMessage"];
							threadPoolParameter.obj = null;
						}
						WaitCallback callBack = new WaitCallback(this.CallbackYuJingMessage);
						ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
					}
				}
				if (this._YuJingCurrentRow["YJFS"].ToString().Substring(1, 1) == "0")
				{
					TMainForm.PlaySound(this._YuJingCurrentRow["SYDZ"].ToString(), 0, 131073);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingUpdateZJCFSJ(int YuJingHH)
		{
			try
			{
				this.dvgYJInfoF9.Rows[YuJingHH].Cells["ZJCFSJ"].Value = this._DateTime;
				string[] columns = new string[]
				{
					"ZJCFSJ"
				};
				string[] columnValue = new string[]
				{
					this._DateTime
				};
				if (!TMainForm._XmlYJView.UpdateXmlRow(columns, columnValue, "ID", this._YuJingCurrentRow["ID"].ToString()))
				{
					MessageForm messageForm = new MessageForm("提示信息", "更新预警信息最近触发时间失败", 1, StatusBarType.Error);
					messageForm.ShowDialog();
					messageForm.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingCfcs(int YuJingHH)
		{
			try
			{
				string value = this._YuJingCurrentRow["CFCS"].ToString();
				string[] array = new string[]
				{
					"10",
					"30",
					"60",
					"180",
					"300",
					"900",
					"1800",
					"3600"
				};
				string value2 = array[(int)Convert.ToInt16(this._YuJingCurrentRow["XDSJD"])].ToString();
				string text = this._YuJingCurrentRow["ZJCFSJ"].ToString();
				if (text == "")
				{
					this.YuJingCs++;
					this.YuJingUpdateZJCFSJ(YuJingHH);
					this.YuJingShowMessage();
				}
				else
				{
					string value3 = Convert.ToDateTime(text).AddSeconds((double)Convert.ToInt16(value2)).TimeOfDay.ToString().Substring(0, 8);
					if (DateTime.Compare(Convert.ToDateTime(this._ServerTime), Convert.ToDateTime(value3)) >= 0)
					{
						this.YuJingCs = 0;
						this.YuJingCs++;
						this.YuJingUpdateZJCFSJ(YuJingHH);
						this.YuJingShowMessage();
					}
					else if (this.YuJingCs < (int)Convert.ToInt16(value))
					{
						this.YuJingCs++;
						this.YuJingUpdateZJCFSJ(YuJingHH);
						this.YuJingShowMessage();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingTypeOfOther(string dgvFirmInfoCol, string YuJingFZ, string ShowText, int YuJingHH)
		{
			try
			{
				if (this.dgvFirmInfo.RowCount != 0)
				{
					string text = this.dgvFirmInfo.Rows[0].Cells[dgvFirmInfoCol].Value.ToString();
					if (dgvFirmInfoCol == "FundRisk")
					{
						text = text.Substring(0, text.IndexOf("%"));
					}
					string a;
					if ((a = this._YuJingCurrentRow["YJTJ"].ToString()) != null)
					{
						if (!(a == "0"))
						{
							if (!(a == "1"))
							{
								if (a == "2")
								{
									if (Convert.ToDecimal(YuJingFZ) > Convert.ToDecimal(text))
									{
										this._dqz = text;
										this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "小于" + YuJingFZ.ToString();
										this.YuJingCfcs(YuJingHH);
									}
								}
							}
							else if (YuJingFZ.Equals(text))
							{
								this._dqz = text;
								this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "等于" + YuJingFZ.ToString();
								this.YuJingCfcs(YuJingHH);
							}
						}
						else if (Convert.ToDecimal(YuJingFZ) < Convert.ToDecimal(text))
						{
							this._dqz = text;
							this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "大于" + YuJingFZ.ToString();
							this.YuJingCfcs(YuJingHH);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void YuJingTypeOfOtherMember(int rownum, int colnum, string YuJingFZ, int YuJingHH)
		{
			try
			{
				if (this.dgvqFirmInfo.RowCount != 0)
				{
					string text = this.dgvqFirmInfo.Rows[rownum].Cells[colnum].Value.ToString();
					if (text != null)
					{
						if (text.IndexOf("%") > 0)
						{
							text = text.Substring(0, text.IndexOf("%"));
						}
						string a;
						if ((a = this._YuJingCurrentRow["YJTJ"].ToString()) != null)
						{
							if (!(a == "0"))
							{
								if (!(a == "1"))
								{
									if (a == "2")
									{
										if (Convert.ToDecimal(YuJingFZ) > Convert.ToDecimal(text))
										{
											this._dqz = text;
											this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "小于" + YuJingFZ.ToString();
											this.YuJingCfcs(YuJingHH);
										}
									}
								}
								else if (YuJingFZ.Equals(text))
								{
									this._dqz = text;
									this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "等于" + YuJingFZ.ToString();
									this.YuJingCfcs(YuJingHH);
								}
							}
							else if (Convert.ToDecimal(YuJingFZ) < Convert.ToDecimal(text))
							{
								this._dqz = text;
								this._YuJingMessage = this.dvgYJInfoF9.Rows[YuJingHH].Cells["YJX"].Value.ToString() + "大于" + YuJingFZ.ToString();
								this.YuJingCfcs(YuJingHH);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void WriteMessage()
		{
			try
			{
				string[] columns = new string[]
				{
					"ID",
					"YJLX",
					"YJX",
					"YJTJ",
					"DQZ",
					"CFSJ"
				};
				string[] columnValue = new string[]
				{
					this._YuJingHH.ToString(),
					(string)this.dvgYJInfoF9.Rows[this._YuJingHH - 1].Cells["YJLX"].Value,
					"",
					this._YuJingMessage,
					this._dqz,
					this._DateTime
				};
				new CreateXmlFile();
				TMainForm._CreateXml.CreateFile(this.YJMessageFileName);
				TMainForm._CreateXml.WriteXmlData(columns, columnValue);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dvgYJInfoF9_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			this._YuJingNum = e.RowIndex;
			this.UPDateYJ();
		}
		private void SetCIF8DataGridColText()
		{
			try
			{
				this.dgvCommodityInfo.Columns["ProjectL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECT"));
				this.dgvCommodityInfo.Columns["ProjectL"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvCommodityInfo.Columns["ProjectValL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECTVAL"));
				this.dgvCommodityInfo.Columns["ProjectValL"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvCommodityInfo.Columns["ProjectR"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECT"));
				this.dgvCommodityInfo.Columns["ProjectR"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvCommodityInfo.Columns["ProjectValR"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECTVAL"));
				this.dgvCommodityInfo.Columns["ProjectValR"].SortMode = DataGridViewColumnSortMode.NotSortable;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void InitCommodityInfoF8()
		{
			try
			{
				this.m_Commoditydata = new Dictionary<string, string>();
				this.gbCommodityInfo.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CI_GB_COMMODITYINFO"));
				this.LableCommodity.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "LC_LABLECOMMODITY"));
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						return;
					}
				}
				else if (Global.CommodityData == null)
				{
					return;
				}
				if (this.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							this.cbCommodityInfo.Items.Add(current.Value.CommodityName);
							this.m_Commoditydata[current.Value.CommodityName] = current.Value.CommodityID.ToString();
						}
						goto IL_179;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					this.cbCommodityInfo.Items.Add(current2.Value.CommodityName);
					this.m_Commoditydata[current2.Value.CommodityName] = current2.Value.CommodityID.ToString();
				}
				IL_179:
				this.cbCommodityInfo.Text = this.cbCommodityInfo.Items[0].ToString();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadCIF8()
		{
			try
			{
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						return;
					}
				}
				else if (Global.CommodityData == null)
				{
					return;
				}
				this.callFillCommodityInfoF8DataGrid = new TMainForm.CallbackCommodityInfoF8DataGrid(this.FillCommodityInfoDataGridF8);
				this.EnableControls(false, "数据查询中");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadCIF8"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCIF8"];
					threadPoolParameter.obj = this.m_Commoditydata[(string)this.cbCommodityInfo.SelectedItem];
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadCIF8", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCIF8"];
					threadPoolParameter.obj = this.m_Commoditydata[(string)this.cbCommodityInfo.SelectedItem];
				}
				WaitCallback callBack = new WaitCallback(this.QueryCommodityInfoF8);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryCommodityInfoF8(object _CommodityID)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.QueryCommodityInfoF8flag)
				{
					this.QueryCommodityInfoF8flag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_CommodityID;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					DataSet commodityInfo = this.dataProcess.GetCommodityInfo(threadPoolParameter.obj.ToString());
					this.HandleCreated();
					base.BeginInvoke(this.callFillCommodityInfoF8DataGrid, new object[]
					{
						commodityInfo
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.QueryCommodityInfoF8flag = false;
			}
		}
		private void FillCommodityInfoDataGridF8(DataSet CommodityInfo)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvCommodityInfo.SelectedCells.Count != 0)
				{
					num = this.dgvCommodityInfo.SelectedCells[0].RowIndex;
				}
				if (this.dgvCommodityInfo.SortedColumn != null)
				{
					text = this.dgvCommodityInfo.SortedColumn.Name;
					SortOrder sortOrder = this.dgvCommodityInfo.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				Logger.wirte(1, "FillCommodityInfoDataGridF8线程启动1");
				if (CommodityInfo != null)
				{
					DataView dataView = new DataView(CommodityInfo.Tables["tCommodityInfo"]);
					this.dgvCommodityInfo.DataSource = dataView.Table;
					Logger.wirte(1, "FillCommodityInfoDataGridF8线程2");
				}
				this.SetCIF8DataGridColText();
				Logger.wirte(1, "FillCommodityInfoDataGridF8线程3");
				if (this.dgvCommodityInfo.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvCommodityInfo.Sort(this.dgvCommodityInfo.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvCommodityInfo.Rows.Count)
					{
						this.dgvCommodityInfo.CurrentCell = this.dgvCommodityInfo.Rows[num].Cells[0];
					}
					else
					{
						this.dgvCommodityInfo.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, "数据查询完毕");
			Logger.wirte(1, "FillCommodityInfoDataGridF8线程完成");
		}
		private void SetFIF7DataGridColText()
		{
			try
			{
				this.dgvqFirmInfo.Columns["ProjectL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECT"));
				this.dgvqFirmInfo.Columns["ProjectL"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvqFirmInfo.Columns["ProjectL"].Width = 140;
				this.dgvqFirmInfo.Columns["ProjectValL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECTVAL"));
				this.dgvqFirmInfo.Columns["ProjectValL"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvqFirmInfo.Columns["ProjectValL"].Width = 140;
				this.dgvqFirmInfo.Columns["ProjectR"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECT"));
				this.dgvqFirmInfo.Columns["ProjectR"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvqFirmInfo.Columns["ProjectR"].Width = 140;
				this.dgvqFirmInfo.Columns["ProjectValR"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_PROJECTVAL"));
				this.dgvqFirmInfo.Columns["ProjectValR"].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvqFirmInfo.Columns["ProjectValR"].Width = 140;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadFIF7()
		{
			try
			{
				this.callFillFirmInfoF7DataGrid = new TMainForm.CallbackFirmInfoF7DataGrid(this.FillFirmInfoDataGridF7);
				if (!this.Member)
				{
					this.EnableControls(false, "数据查询中");
				}
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadFIF7"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadFIF7"];
					threadPoolParameter.obj = null;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadFIF7", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadFIF7"];
					threadPoolParameter.obj = null;
				}
				WaitCallback callBack = new WaitCallback(this.QueryFirmInfoF7);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryFirmInfoF7(object _FirmInforQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.FirmInfoF7flag)
				{
					this.FirmInfoF7flag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_FirmInforQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					DataSet accountInfo = this.dataProcess.GetAccountInfo();
					this.HandleCreated();
					base.BeginInvoke(this.callFillFirmInfoF7DataGrid, new object[]
					{
						accountInfo
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.FirmInfoF7flag = false;
			}
		}
		private void FillFirmInfoDataGridF7(DataSet FirmInfo)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvqFirmInfo.SelectedCells.Count != 0)
				{
					num = this.dgvqFirmInfo.SelectedCells[0].RowIndex;
				}
				if (this.dgvqFirmInfo.SortedColumn != null)
				{
					text = this.dgvqFirmInfo.SortedColumn.Name;
					SortOrder sortOrder = this.dgvqFirmInfo.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				Logger.wirte(1, "FillFirmInfoDataGridF7线程1");
				if (FirmInfo != null)
				{
					DataView dataView = new DataView(FirmInfo.Tables["tFirmInfo"]);
					this.dgvqFirmInfo.DataSource = dataView.Table;
					Logger.wirte(1, "FillFirmInfoDataGridF7线程2");
				}
				this.SetFIF7DataGridColText();
				Logger.wirte(1, "FillFirmInfoDataGridF7线程3");
				SystemStatus currentSystemStatus;
				lock (this._CurrentSystemStatusObject)
				{
					currentSystemStatus = this._CurrentSystemStatus;
				}
				if (currentSystemStatus != SystemStatus.SettlementComplete)
				{
					this.CalculateFirmInfoF7();
				}
				Logger.wirte(1, "FillFirmInfoDataGridF7线程4");
				if (this.dgvqFirmInfo.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvqFirmInfo.Sort(this.dgvqFirmInfo.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvqFirmInfo.Rows.Count)
					{
						this.dgvqFirmInfo.CurrentCell = this.dgvqFirmInfo.Rows[num].Cells[0];
					}
					else
					{
						this.dgvqFirmInfo.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			if (!this.Member)
			{
				this.EnableControls(true, "数据查询完毕");
			}
			Logger.wirte(1, "FillFirmInfoDataGridF7线程完成");
		}
		private void CalculateFirmInfoF7()
		{
			try
			{
				double num;
				lock (this.floatingPriceTotalLock)
				{
					num = this.floatingPT;
				}
				this.callbackUpDataFirmInfoF7 = new TMainForm.CallbackUpDataFirmInfoF7(this.UpDataFirmInfoF7);
				this.HandleCreated();
				base.BeginInvoke(this.callbackUpDataFirmInfoF7, new object[]
				{
					num
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void UpDataFirmInfoF7(double floatingPriceTotal)
		{
			try
			{
				FirmInfoResponseVO firmInfoResponseVO = null;
				if (this.FirmInfoF7flag)
				{
					return;
				}
				this.FirmInfoF7flag = true;
				if (this.dgvqFirmInfo.Rows.Count == 0)
				{
					this.FirmInfoF7flag = false;
					return;
				}
				int arg_3F_0 = this.dgvqFirmInfo.RowCount;
				if (this.dataProcess.IsAgency)
				{
					lock (Global.AgencyFirmInfoDataLock)
					{
						if (Global.AgencyFirmInfoData == null)
						{
							return;
						}
						firmInfoResponseVO = (FirmInfoResponseVO)Global.AgencyFirmInfoData.Clone();
						goto IL_BD;
					}
				}
				lock (Global.FirmInfoDataLock)
				{
					if (Global.FirmInfoData == null)
					{
						return;
					}
					firmInfoResponseVO = (FirmInfoResponseVO)Global.FirmInfoData.Clone();
				}
				IL_BD:
				if (firmInfoResponseVO != null)
				{
					DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
					if (this.dataProcess.sIdentity == Identity.Client)
					{
						double balance = BizController.CalculateBalance(firmInfoResponseVO.InitFund, firmInfoResponseVO.InOutFund, firmInfoResponseVO.Fee, firmInfoResponseVO.YesterdayBail, firmInfoResponseVO.TransferPL);
						double initFund = BizController.CalculateInitFund(balance, floatingPriceTotal);
						this.dgvqFirmInfo.Rows[1].Cells["ProjectValR"].Value = initFund.ToString("n2");
						if (Convert.ToDouble(floatingPriceTotal) > 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Red;
						}
						else if (Convert.ToDouble(floatingPriceTotal) == 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Black;
						}
						else
						{
							dataGridViewCellStyle.ForeColor = Color.Green;
						}
						this.dgvqFirmInfo.Rows[2].Cells["ProjectValR"].Style = dataGridViewCellStyle;
						this.dgvqFirmInfo.Rows[2].Cells["ProjectValR"].Value = floatingPriceTotal.ToString("n2");
						this.dgvqFirmInfo.Rows[3].Cells["ProjectValR"].Value = BizController.CalculateRealFund(initFund, firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.UsingFund).ToString("n2");
						double currentBail = BizController.CalculateHoldingFund(firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.OtherFrozenFund);
						this.dgvqFirmInfo.Rows[4].Cells["ProjectValR"].Value = currentBail.ToString("n2");
						double num = BizController.CalculateFundRisk(initFund, currentBail);
						this.dgvqFirmInfo.Rows[6].Cells["ProjectValL"].Value = ((num < 0.0) ? "0.00%" : num.ToString("p2"));
					}
					else if (this.dataProcess.sIdentity == Identity.Member)
					{
						double num2 = -this.customerFloatingPT;
						double num3 = floatingPriceTotal;
						double num4 = num2 + num3;
						double num5 = Convert.ToDouble(this.dgvqFirmInfo.Rows[1].Cells["ProjectValL"].Value);
						double num6 = Convert.ToDouble(this.dgvqFirmInfo.Rows[2].Cells["ProjectValL"].Value);
						double num7 = Convert.ToDouble(this.dgvqFirmInfo.Rows[3].Cells["ProjectValL"].Value);
						double num8 = Convert.ToDouble(this.dgvqFirmInfo.Rows[4].Cells["ProjectValL"].Value);
						double num9 = Convert.ToDouble(this.dgvqFirmInfo.Rows[5].Cells["ProjectValL"].Value);
						double num10 = Convert.ToDouble(this.dgvqFirmInfo.Rows[5].Cells["ProjectValR"].Value);
						double num11 = Convert.ToDouble(this.dgvqFirmInfo.Rows[6].Cells["ProjectValL"].Value);
						double memberMinRiskFund = this._MemberMinRiskFund;
						double num12 = (num5 >= memberMinRiskFund) ? num5 : memberMinRiskFund;
						double num13 = num5 + num6 + num7 + num8 + num9 + num4;
						double num14 = num13 / num12;
						double num15 = num13 - num10 - num11;
						dataGridViewCellStyle = new DataGridViewCellStyle();
						if (Convert.ToDouble(num4) > 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Red;
						}
						else if (Convert.ToDouble(num4) < 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Green;
						}
						else
						{
							dataGridViewCellStyle.ForeColor = Color.Black;
						}
						this.dgvqFirmInfo.Rows[2].Cells["ProjectValR"].Style = dataGridViewCellStyle;
						this.dgvqFirmInfo.Rows[2].Cells["ProjectValR"].Value = num4.ToString("n2");
						dataGridViewCellStyle = new DataGridViewCellStyle();
						if (Convert.ToDouble(num2) > 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Red;
						}
						else if (Convert.ToDouble(num2) < 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Green;
						}
						else
						{
							dataGridViewCellStyle.ForeColor = Color.Black;
						}
						this.dgvqFirmInfo.Rows[3].Cells["ProjectValR"].Style = dataGridViewCellStyle;
						this.dgvqFirmInfo.Rows[3].Cells["ProjectValR"].Value = num2.ToString("n2");
						dataGridViewCellStyle = new DataGridViewCellStyle();
						if (Convert.ToDouble(num3) > 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Red;
						}
						else if (Convert.ToDouble(num3) < 0.0)
						{
							dataGridViewCellStyle.ForeColor = Color.Green;
						}
						else
						{
							dataGridViewCellStyle.ForeColor = Color.Black;
						}
						this.dgvqFirmInfo.Rows[4].Cells["ProjectValR"].Style = dataGridViewCellStyle;
						this.dgvqFirmInfo.Rows[4].Cells["ProjectValR"].Value = num3.ToString("n2");
						this.dgvqFirmInfo.Rows[6].Cells["ProjectValR"].Value = ((num14 < 0.0) ? "0.00%" : num14.ToString("p2"));
						this.dgvqFirmInfo.Rows[1].Cells["ProjectValR"].Value = num15.ToString("n2");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.FirmInfoF7flag = false;
		}
		private void dgvqFirmInfo_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					this.dgvqFirmInfo.ClearSelection();
					Point position = this.dgvqFirmInfo.PointToClient(Cursor.Position);
					this.SetMenuDisenable("contextMenuStripF7");
					this.contextMenuStripF7.Show(this.dgvqFirmInfo, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemF7Refresh_Click(object sender, EventArgs e)
		{
			if (this.tabTMain.SelectedIndex != 5)
			{
				if (this.tabTMain.SelectedIndex == 0)
				{
					if (this.F10Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
					{
						this.UpDataTabCtrl();
						return;
					}
					MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm.ShowDialog();
				}
				return;
			}
			if (this.F7Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
			{
				this.UpDataTabCtrl();
				return;
			}
			MessageForm messageForm2 = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
			messageForm2.ShowDialog();
		}
		private void SetHIF6DataGridColText()
		{
			try
			{
				this.dgvHoldingInfoF6.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvHoldingInfoF6.Columns["CommodityName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingInfoF6.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["BuySellText"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_BUYSELL");
				this.dgvHoldingInfoF6.Columns["BuySellText"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingInfoF6.Columns["BuySellText"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["Qty"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_QUANTITY");
				this.dgvHoldingInfoF6.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["Qty"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["OpenAveragePrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_OPENPRICEAVERAGE");
				this.dgvHoldingInfoF6.Columns["OpenAveragePrice"].DefaultCellStyle.Format = "f2";
				this.dgvHoldingInfoF6.Columns["OpenAveragePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["OpenAveragePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["HoldingAveragePrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_HOLDINGAVERAGE");
				this.dgvHoldingInfoF6.Columns["HoldingAveragePrice"].DefaultCellStyle.Format = "f2";
				this.dgvHoldingInfoF6.Columns["HoldingAveragePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["HoldingAveragePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_CLOSEPRICE");
				this.dgvHoldingInfoF6.Columns["ClosePrice"].DefaultCellStyle.Format = "f2";
				this.dgvHoldingInfoF6.Columns["ClosePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["ClosePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["FloatingLP"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_FLOATINGLP");
				this.dgvHoldingInfoF6.Columns["FloatingLP"].DefaultCellStyle.Format = "n2";
				this.dgvHoldingInfoF6.Columns["FloatingLP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["FloatingLP"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["CommPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_COMMPRICE");
				this.dgvHoldingInfoF6.Columns["CommPrice"].DefaultCellStyle.Format = "f2";
				this.dgvHoldingInfoF6.Columns["CommPrice"].Visible = false;
				this.dgvHoldingInfoF6.Columns["CommPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["Bail"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HI_MAR");
				this.dgvHoldingInfoF6.Columns["Bail"].DefaultCellStyle.Format = "n2";
				this.dgvHoldingInfoF6.Columns["Bail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingInfoF6.Columns["Bail"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingInfoF6.Columns["CommodityID"].HeaderText = "CommodityID";
				this.dgvHoldingInfoF6.Columns["CommodityID"].Visible = false;
				this.dgvHoldingInfoF6.Columns["BuySell"].HeaderText = "BuySell";
				this.dgvHoldingInfoF6.Columns["BuySell"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadHIF6()
		{
			try
			{
				HoldingQueryRequestVO holdingQueryRequestVO = new HoldingQueryRequestVO();
				holdingQueryRequestVO.UserID = Global.UserID;
				this.callbackHoldingInfoF6DataGrid = new TMainForm.CallbackHoldingInfoF6DataGrid(this.FillHoldingInfoDataGridF6);
				this.EnableControls(false, "数据查询中");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadHIF6"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHIF6"];
					threadPoolParameter.obj = holdingQueryRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadHIF6", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHIF6"];
					threadPoolParameter.obj = holdingQueryRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.QueryHoldingInfoF6);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryHoldingInfoF6(object _holdingQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.FillHoldingInfoFloatingPriceflag)
				{
					this.FillHoldingInfoFloatingPriceflag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_holdingQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					HoldingQueryRequestVO holdingQueryRequestVO = (HoldingQueryRequestVO)threadPoolParameter.obj;
					DataSet dataSet = this.dataProcess.QueryHoldingInfo(holdingQueryRequestVO, this._CurrentSystemStatus);
					this.HandleCreated();
					base.BeginInvoke(this.callbackHoldingInfoF6DataGrid, new object[]
					{
						dataSet
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.FillHoldingInfoFloatingPriceflag = false;
			}
		}
		private void FillHoldingInfoDataGridF6(DataSet HoldingDetailInfo)
		{
			try
			{
				string arg_05_0 = string.Empty;
				if (this.dgvHoldingInfoF6.SelectedCells.Count != 0)
				{
					int arg_30_0 = this.dgvHoldingInfoF6.SelectedCells[0].RowIndex;
				}
				if (this.dgvHoldingInfoF6.SortedColumn != null)
				{
					string arg_4E_0 = this.dgvHoldingInfoF6.SortedColumn.Name;
					SortOrder sortOrder = this.dgvHoldingInfoF6.SortOrder;
				}
				Logger.wirte(1, "FillHoldingInfoDataGridF6线程启动1");
				if (HoldingDetailInfo != null && HoldingDetailInfo.Tables.Contains("Holding"))
				{
					DataTable dataTable = this.DataTableSort(HoldingDetailInfo.Tables["Holding"], " 1=1 ", "CommodityName", "Desc");
					this.DataViewAddQueryF6Sum(dataTable.DefaultView);
					this.dgvHoldingInfoF6.DataSource = dataTable.DefaultView;
					Logger.wirte(1, "FillHoldingInfoDataGridF6线程2");
				}
				this.SetHIF6DataGridColText();
				Logger.wirte(1, "FillHoldingInfoDataGridF6线程3");
				this.gbHoldingInfoF6.Text = Global.m_PMESResourceManager.GetString("PMESStr_HI_QB_TEXT");
				this.FillHoldingInfoFloatingPriceflag = false;
				this.FillHoldingInfoFloatingPrice();
				Logger.wirte(1, "FillHoldingInfoDataGridF6线程4");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, "");
			Logger.wirte(1, "FillHoldingInfoDataGridF6线程完成");
		}
		private void DataViewAddQueryF6Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				double num2 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["Qty"];
					num2 += (double)dataView[j].Row["Bail"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["BuySellText"] = string.Format(text, dataView.Count - 1);
				dataRowView["Qty"] = num;
				dataRowView["Bail"] = num2;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
		}
		private void InvokeFillHoldingInfoFloatingPrice()
		{
			try
			{
				if (!this.FillHoldingInfoFloatingPriceflag)
				{
					this.FillHoldingInfoFloatingPriceflag = true;
					DataTable dataTable = null;
					lock (this.LockDIDataTable)
					{
						dataTable = this._HDIDataTable.Tables["HDIDetatable"].Copy();
					}
					if (dataTable == null)
					{
						this.FillHoldingInfoFloatingPriceflag = false;
					}
					else if (dataTable.Rows.Count == 0)
					{
						this.FillHoldingInfoFloatingPriceflag = false;
					}
					else
					{
						int i = 0;
						while (i < this.dgvHoldingInfoF6.RowCount)
						{
							string text = this.dgvHoldingInfoF6.Rows[i].Cells["CommodityID"].Value.ToString();
							string text2 = this.dgvHoldingInfoF6.Rows[i].Cells["BuySell"].Value.ToString();
							double num = 0.0;
							string text3 = string.Format(" CommodityID='{0}' and BuySell='{1}' ", text, text2);
							if (dataTable.Select(text3).Length > 0)
							{
								double.TryParse(dataTable.Compute("Sum(FloatingPrice)", text3).ToString(), out num);
							}
							DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
							if (Convert.ToDouble(num) > 0.0)
							{
								dataGridViewCellStyle.ForeColor = Color.Red;
							}
							else if (Convert.ToDouble(num) == 0.0)
							{
								dataGridViewCellStyle.ForeColor = Color.Black;
							}
							else
							{
								dataGridViewCellStyle.ForeColor = Color.Green;
							}
							this.dgvHoldingInfoF6.Rows[i].Cells["FloatingLP"].Style = dataGridViewCellStyle;
							if (this.dgvHoldingInfoF6.Rows[i].Cells["CommodityName"].Value.ToString() != this.Total)
							{
								this.dgvHoldingInfoF6.Rows[i].Cells["FloatingLP"].Value = num;
							}
							Dictionary<string, CommData> dictionary = null;
							if (this.dataProcess.IsAgency)
							{
								lock (Global.AgencyHQCommDataLock)
								{
									if (Global.AgencyHQCommData != null)
									{
										dictionary = Global.gAgencyHQCommData;
									}
									goto IL_245;
								}
								goto IL_220;
							}
							goto IL_220;
							IL_245:
							if (this._CurrentSystemStatus != SystemStatus.SettlementComplete)
							{
								if (text2 == BuySell.Buy.ToString("d"))
								{
									if (dictionary != null && dictionary.ContainsKey(text))
									{
										this.dgvHoldingInfoF6.Rows[i].Cells["ClosePrice"].Value = dictionary[text].SellPrice.ToString();
									}
								}
								else if (text2 == BuySell.Sell.ToString("d") && dictionary != null && dictionary.ContainsKey(text))
								{
									this.dgvHoldingInfoF6.Rows[i].Cells["ClosePrice"].Value = dictionary[text].BuyPrice.ToString();
								}
							}
							else
							{
								this.dgvHoldingInfoF6.Rows[i].Cells["ClosePrice"].Value = 0;
							}
							i++;
							continue;
							IL_220:
							lock (Global.HQCommDataLock)
							{
								if (Global.HQCommData != null)
								{
									dictionary = Global.gHQCommData;
								}
							}
							goto IL_245;
						}
						this.FillHoldingInfoFloatingPriceflag = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				this.FillHoldingInfoFloatingPriceflag = false;
			}
		}
		private void FillHoldingInfoFloatingPrice()
		{
			TMainForm.InvokeFillHoldingInfoFloatingPriceEvent method = new TMainForm.InvokeFillHoldingInfoFloatingPriceEvent(this.InvokeFillHoldingInfoFloatingPrice);
			this.HandleCreated();
			base.BeginInvoke(method, new object[0]);
		}
		private void dgvHoldingInfoF6_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					this.dgvHoldingInfoF6.ClearSelection();
					this.SetMenuDisenable("contextMenuStripF6");
					this.toolStripMenuItemF6SP.Enabled = (this._ChiCangNum >= 0);
					Point position = this.dgvHoldingInfoF6.PointToClient(Cursor.Position);
					this.contextMenuStripF6.Show(this.dgvHoldingInfoF6, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemF6Refresh_Click(object sender, EventArgs e)
		{
			if (this.F6Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
			{
				this.UpDataTabCtrl();
				return;
			}
			MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
			messageForm.ShowDialog();
		}
		private void toolStripMenuItemF6SP_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow = this.dgvHoldingInfoF6.Rows[this._ChiCangNum2];
				string text = dataGridViewRow.Cells["BuySell"].Value.ToString();
				int num = Convert.ToInt32(dataGridViewRow.Cells["Qty"].Value);
				string text2 = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				PWarehouseForm pWarehouseForm = new PWarehouseForm(this);
				pWarehouseForm.CloseTradeType = TradeType.ShiJiaDan;
				pWarehouseForm.CurrentCommodityId = text2;
				pWarehouseForm.IsCloseSpecificOrder = false;
				pWarehouseForm.IsCloseFromTotalHolding = true;
				CloseCommodityInfo closeCommodityInfo = new CloseCommodityInfo();
				closeCommodityInfo.CommodityID = text2;
				if (text == BuySell.Buy.ToString("d"))
				{
					closeCommodityInfo.CloseMaxSellQty = (long)num;
					pWarehouseForm.CurrentBuySell = BuySell.Sell.ToString("d");
				}
				else if (text == BuySell.Sell.ToString("d"))
				{
					closeCommodityInfo.CloseMaxBuyQty = (long)num;
					pWarehouseForm.CurrentBuySell = BuySell.Buy.ToString("d");
				}
				closeCommodityInfo.OriginBuySell = text;
				pWarehouseForm.CloseCommodityInfoList.Add(closeCommodityInfo.CommodityID, closeCommodityInfo);
				pWarehouseForm.ShowDialog();
				if (pWarehouseForm.IsCloseButtonOKOrCancel)
				{
					this.DelegateRefresh();
					this.UpDataTabCtrl();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingInfoF6_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.dgvHoldingInfoF6.ClearSelection();
					this.dgvHoldingInfoF6.Rows[e.RowIndex].Selected = true;
					this.toolStripMenuItemF6SP.Enabled = true;
					this._ChiCangNum = e.RowIndex;
					this._ChiCangNum2 = e.RowIndex;
					Point position = this.dgvHoldingInfoF6.PointToClient(Cursor.Position);
					this.SetMenuDisenable("contextMenuStripF6");
					this.contextMenuStripF6.Show(this.dgvHoldingInfoF6, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void reDraw()
		{
			base.Invalidate();
		}
		private void contextMenuStripF6_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this._ChiCangNum = -1;
		}
		private void dgvHoldingInfoF6_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
				{
					if (this.identitystatus())
					{
						this._ChiCangNum2 = e.RowIndex;
						this.toolStripMenuItemF6SP_Click(sender, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingInfoF6_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataView dataView = (DataView)this.dgvHoldingInfoF6.DataSource;
			try
			{
				dataView.Sort = " AutoID ASC, " + this.dgvHoldingInfoF6.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				dataView.Sort = " " + this.dgvHoldingInfoF6.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.dgvHoldingInfoF6.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
					this.m_order = " Desc ";
				}
				else
				{
					this.dgvHoldingInfoF6.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
					this.m_order = " ASC ";
				}
			}
		}
		private void SetHDIF5DataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["HoldingID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfoF5.Columns["HoldingID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["HoldingID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGIDID");
				this.dgvHoldingDetailInfoF5.Columns["HoldingID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["CommodityName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfoF5.Columns["CommodityName"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvHoldingDetailInfoF5.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["BuySellText"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfoF5.Columns["BuySellText"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["BuySellText"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_SELLBUY"));
				this.dgvHoldingDetailInfoF5.Columns["BuySellText"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["OpenQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["OpenQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["OpenQuantity"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OPENQUANTITY");
				this.dgvHoldingDetailInfoF5.Columns["OpenQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["HoldingQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["HoldingQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["HoldingQuantity"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGQUANTITY");
				this.dgvHoldingDetailInfoF5.Columns["HoldingQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["OpenPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["OpenPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["OpenPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OPENPRICE");
				this.dgvHoldingDetailInfoF5.Columns["OpenPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["HoldPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["HoldPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["HoldPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDPRICE");
				this.dgvHoldingDetailInfoF5.Columns["HoldPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["ClosePrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["ClosePrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_CLOSEPRICE");
				this.dgvHoldingDetailInfoF5.Columns["ClosePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["StopLossShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["StopLossShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["StopLossShow"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPLOSS");
				this.dgvHoldingDetailInfoF5.Columns["StopLossShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfoF5.Columns["StopLossShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["StopLoss"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["StopLoss"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["StopLoss"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPLOSS");
				this.dgvHoldingDetailInfoF5.Columns["StopLoss"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["StopProfit"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["StopProfit"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["StopProfit"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPPROFIT");
				this.dgvHoldingDetailInfoF5.Columns["StopProfit"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["StopProfitShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["StopProfitShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["StopProfitShow"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPPROFIT");
				this.dgvHoldingDetailInfoF5.Columns["StopProfitShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfoF5.Columns["StopProfitShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["HoldingFloat"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfoF5.Columns["HoldingFloat"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["HoldingFloat"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGFLOAT");
				this.dgvHoldingDetailInfoF5.Columns["HoldingFloat"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["FloatingPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfoF5.Columns["FloatingPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["FloatingPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_FLOATINGPRICE");
				this.dgvHoldingDetailInfoF5.Columns["FloatingPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["TotalFloat"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfoF5.Columns["TotalFloat"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["TotalFloat"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_TOTALFLOAT");
				this.dgvHoldingDetailInfoF5.Columns["TotalFloat"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["CommPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfoF5.Columns["CommPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["CommPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_COMMPRICE");
				this.dgvHoldingDetailInfoF5.Columns["CommPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfoF5.Columns["CommPrice"].Visible = false;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["OrderTime"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["OrderTime"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["OrderTime"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_ORDERTIME");
				this.dgvHoldingDetailInfoF5.Columns["OrderTime"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["Bail"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfoF5.Columns["Bail"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["Bail"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_MAR");
				this.dgvHoldingDetailInfoF5.Columns["Bail"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["OtherID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["OtherID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["OtherID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OTHERID");
				this.dgvHoldingDetailInfoF5.Columns["OtherID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfoF5.Columns["OtherID"].Visible = (this.dataProcess.sIdentity == Identity.Member);
				defaultCellStyle = this.dgvHoldingDetailInfoF5.Columns["AgentID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfoF5.Columns["AgentID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfoF5.Columns["AgentID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_AGENTID");
				this.dgvHoldingDetailInfoF5.Columns["AgentID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfoF5.Columns["AgentID"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				this.dgvHoldingDetailInfoF5.Columns["CommodityID"].HeaderText = "";
				this.dgvHoldingDetailInfoF5.Columns["CommodityID"].Visible = false;
				this.dgvHoldingDetailInfoF5.Columns["BuySell"].HeaderText = "";
				this.dgvHoldingDetailInfoF5.Columns["BuySell"].Visible = false;
				this.dgvHoldingDetailInfoF5.Columns["StopProfit"].Visible = false;
				this.dgvHoldingDetailInfoF5.Columns["StopLoss"].Visible = false;
				this.dgvHoldingDetailInfoF5.Columns["UnitQty"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadHDIF5()
		{
			try
			{
				HoldingDetailRequestVO holdingDetailRequestVO = new HoldingDetailRequestVO();
				holdingDetailRequestVO.UserID = Global.UserID;
				this.callbackHoldingF5DataGrid = new TMainForm.CallbackHoldingF5DataGrid(this.FillHoldingDataGridF5);
				this.EnableControls(false, "数据查询中");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadHDIF5"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHDIF5"];
					threadPoolParameter.obj = holdingDetailRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadHDIF5", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHDIF5"];
					threadPoolParameter.obj = holdingDetailRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.QueryHoldingF5);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryHoldingF5(object _holdingrQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.UpDataHoldingDetailInfoF5flag)
				{
					this.UpDataHoldingDetailInfoF5flag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_holdingrQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					HoldingDetailRequestVO holdingDetailRequestVO = (HoldingDetailRequestVO)threadPoolParameter.obj;
					SystemStatus currentSystemStatus;
					lock (this._CurrentSystemStatusObject)
					{
						currentSystemStatus = this._CurrentSystemStatus;
					}
					lock (this.LockDIDataTableF5)
					{
						this._HDIDataTableF5 = this.dataProcess.QueryHoldingDetailInfo(holdingDetailRequestVO, currentSystemStatus);
					}
					this.HandleCreated();
					base.BeginInvoke(this.callbackHoldingF5DataGrid, new object[]
					{
						this._HDIDataTableF5
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.UpDataHoldingDetailInfoF5flag = false;
			}
		}
		private void InvokeUpDataHoldingDetailInfoF5HQ()
		{
			try
			{
				if (!this.UpDataHoldingDetailInfoF5flag)
				{
					this.UpDataHoldingDetailInfoF5flag = true;
					this.UpDataHoldingDetailInfoF5();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				this.UpDataHoldingDetailInfoF5flag = false;
			}
		}
		private void UpDataHoldingDetailInfoF5HQ()
		{
			this.callbackUpDataHoldingDetailInfoF5HQ = new TMainForm.CallbackUpDataHoldingDetailInfoF5HQ(this.InvokeUpDataHoldingDetailInfoF5HQ);
			this.HandleCreated();
			base.BeginInvoke(this.callbackUpDataHoldingDetailInfoF5HQ, new object[0]);
		}
		private void UpDataHoldingDetailInfoF5()
		{
			try
			{
				lock (this.iSelecttablock)
				{
				}
				int arg_28_0 = this.dgvHoldingDetailInfoF5.Rows.Count;
				Dictionary<string, CommData> dictionary = null;
				if (this.dataProcess.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData != null)
						{
							dictionary = Global.gAgencyHQCommData;
							goto IL_8E;
						}
						return;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData == null)
					{
						return;
					}
					dictionary = Global.gHQCommData;
				}
				IL_8E:
				lock (this.LockDIDataTableF5)
				{
					if (this._HDIDataTableF5 != null && this._HDIDataTableF5.Tables[0] != null)
					{
						foreach (DataRow dataRow in this._HDIDataTableF5.Tables[0].Rows)
						{
							string a = dataRow["BuySell"].ToString();
							string key = dataRow["CommodityID"].ToString();
							if (a == BuySell.Buy.ToString("d"))
							{
								if (dictionary != null && dictionary.ContainsKey(key))
								{
									double sellPrice = dictionary[key].SellPrice;
									dataRow["ClosePrice"] = sellPrice;
								}
							}
							else if (a == BuySell.Sell.ToString("d") && dictionary != null && dictionary.ContainsKey(key))
							{
								dataRow["ClosePrice"] = dictionary[key].BuyPrice;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillHoldingDataGridF5(DataSet HoldingDetailInfo)
		{
			try
			{
				string arg_05_0 = string.Empty;
				if (this.dgvHoldingDetailInfoF5.SelectedCells.Count != 0)
				{
					int arg_30_0 = this.dgvHoldingDetailInfoF5.SelectedCells[0].RowIndex;
				}
				if (this.dgvHoldingDetailInfoF5.SortedColumn != null)
				{
					string arg_4E_0 = this.dgvHoldingDetailInfoF5.SortedColumn.Name;
					SortOrder sortOrder = this.dgvHoldingDetailInfoF5.SortOrder;
				}
				Logger.wirte(1, "FillHoldingDataGridF5线程启动1");
				if (HoldingDetailInfo != null)
				{
					DataTable dataTable = HoldingDetailInfo.Tables["HDIDetatable"];
					SystemStatus currentSystemStatus;
					lock (this._CurrentSystemStatusObject)
					{
						currentSystemStatus = this._CurrentSystemStatus;
					}
					if (currentSystemStatus != SystemStatus.SettlementComplete)
					{
						this.UpDataHoldingDetailInfoF5();
					}
					this.DataViewAddQueryF5Sum(dataTable.DefaultView);
					this.dgvHoldingDetailInfoF5.DataSource = dataTable.DefaultView;
					dataTable.DefaultView.Sort = "HoldingID desc ";
					Logger.wirte(1, "FillHoldingDataGridF5线程2");
				}
				Logger.wirte(1, "FillHoldingDataGridF5线程3");
				this.SetHDIF5DataGridColText();
				Logger.wirte(1, "FillHoldingDataGridF5线程4");
				this.gbHoldingDetailInfoF5.Text = Global.m_PMESResourceManager.GetString("PMESStr_GB_HDI");
				Logger.wirte(1, "FillHoldingDataGridF5线程5");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "FillHoldingDataGridF5线程完成");
			this.EnableControls(true, "");
		}
		private void DataViewAddQueryF5Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				long num2 = 0L;
				double num3 = 0.0;
				double num4 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["OpenQuantity"];
					num2 += (long)dataView[j].Row["HoldingQuantity"];
					num3 += (double)dataView[j].Row["CommPrice"];
					num4 += (double)dataView[j].Row["Bail"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["BuySellText"] = string.Format(text, dataView.Count - 1);
				dataRowView["OpenQuantity"] = num;
				dataRowView["HoldingQuantity"] = num2;
				dataRowView["CommPrice"] = num3;
				dataRowView["Bail"] = num4;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
		}
		private void dgvHoldingDetailInfoF5_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.dgvHoldingDetailInfoF5.ClearSelection();
					this.dgvHoldingDetailInfoF5.Rows[e.RowIndex].Selected = true;
					this._HoldingDetailContextMenuRowIndex = e.RowIndex;
					string text = this.dgvHoldingDetailInfoF5.Rows[e.RowIndex].Cells["StopLoss"].Value.ToString().Trim();
					string text2 = this.dgvHoldingDetailInfoF5.Rows[e.RowIndex].Cells["StopProfit"].Value.ToString().Trim();
					if (this.SetMenuDisenable("contextMenuStripHoldingDetail"))
					{
						this.toolStripMenuItemStopLoss.Enabled = (text.Length > 0 && Convert.ToDouble(text) != 0.0);
						this.toolStripMenuItemStopProfit.Enabled = (text2.Length > 0 && Convert.ToDouble(text2) != 0.0);
					}
					Point position = this.dgvHoldingDetailInfoF5.PointToClient(Cursor.Position);
					this.contextMenuStripHoldingDetail.Show(this.dgvHoldingDetailInfoF5, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingDetailInfoF5_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DataView dataView = (DataView)this.dgvHoldingDetailInfoF5.DataSource;
				try
				{
					dataView.Sort = " AutoID ASC, " + this.dgvHoldingDetailInfoF5.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				catch (Exception)
				{
					dataView.Sort = " " + this.dgvHoldingDetailInfoF5.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				finally
				{
					if (this.m_order == " ASC ")
					{
						this.dgvHoldingDetailInfoF5.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						this.m_order = " Desc ";
					}
					else
					{
						this.dgvHoldingDetailInfoF5.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
						this.m_order = " ASC ";
					}
				}
			}
		}
		private void dgvHoldingDetailInfoF5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			lock (this.LockDIDataTableF5)
			{
				if ((e.ColumnIndex == this.dgvHoldingDetailInfoF5.Columns["FloatingPrice"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total) || (e.ColumnIndex == this.dgvHoldingDetailInfoF5.Columns["HoldingFloat"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total) || (e.ColumnIndex == this.dgvHoldingDetailInfoF5.Columns["TotalFloat"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total))
				{
					double num = Convert.ToDouble(e.Value);
					if (num > 0.0)
					{
						e.CellStyle.ForeColor = Color.Red;
					}
					else if (num == 0.0)
					{
						e.CellStyle.ForeColor = Color.Black;
					}
					else
					{
						e.CellStyle.ForeColor = Color.Green;
					}
				}
			}
		}
		private void dgvHoldingDetailInfoF5_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.dgvHoldingDetailInfoF5.ClearSelection();
				this.SetMenuDisenable("contextMenuStripHoldingDetail");
				this.toolStripMenuItemSP.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemXP.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemStopLoss.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemStopProfit.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				Point position = this.dgvHoldingDetailInfoF5.PointToClient(Cursor.Position);
				this.contextMenuStripHoldingDetail.Show(this.dgvHoldingDetailInfoF5, position);
			}
		}
		public TMainForm(ITradeLibrary tradeLibrary, string sIdentity)
		{
			try
			{
				this.InitDictionarySemaphore();
				this.InitializeComponent();
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.FillTradeCtrl);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.UpDataCommodityInfoCtrl);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.UpDataHoldingDetailInfoHQ);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.UpDataHoldingDetailInfoF5HQ);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.FillHoldingInfoFloatingPrice);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.UpdateMemberFundPrice);
				this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.YuJingNotifier);
				Identity identity = (Identity)Enum.Parse(typeof(Identity), sIdentity);
				if (identity == Identity.Member)
				{
					this.HQRefreashed += new TMainForm.RefreshHQHanlder(this.UpdateCustomerOrderF10HQ);
				}
				this.IniFileName = Global.ConfigPath + Global.UserID + "Trade.ini";
				IniFile iniFile = new IniFile(this.IniFileName);
				try
				{
					this.LastID = long.Parse(iniFile.IniReadValue("SystemMessage", "LastIdControl"));
				}
				catch (Exception)
				{
					Logger.wirte(1, "系统公告读取失败！有可能是第一次安装");
					this.LastID = 0L;
				}
				Image image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoPic");
				Image image2 = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoClose");
				lock (this._CurrentSystemStatusObject)
				{
					this.toolStripSystemStatus.Text = string.Format("系统状态：{0}", Global.GetEnumtoResourcesString("SYSTEMSTATUS", (int)this._CurrentSystemStatus));
				}
				this.timerLock.Interval = 1000.0;
				this.timerLock.Enabled = true;
				this.timerLock.Elapsed += new ElapsedEventHandler(this.timerLock_Tick);
				this.timerHQ.Interval = 100.0;
				this.timerHQ.Enabled = true;
				this.timerHQ.Elapsed += new ElapsedEventHandler(this.timerHQ_Tick);
				if (image != null && image2 != null)
				{
					Bitmap bitmap = new Bitmap(image);
					Bitmap bitmap2 = new Bitmap(image2);
					this.MessageNotifier.SetBackgroundBitmap(bitmap, Color.FromArgb(0, 0, 255));
					this.MessageNotifier.SetCloseBitmap(bitmap2, Color.FromArgb(0, 0, 255), new Point(180, 0));
					this.MessageNotifier.TitleRectangle = new Rectangle(25, 0, 85, 22);
					this.MessageNotifier.ContentRectangle = new Rectangle(30, 30, 150, 52);
					this.MessageNotifier.AutoHide = true;
				}
				if (tradeLibrary != null)
				{
					this.dataProcess.TradeLibrary = tradeLibrary;
					this.dataProcess.sIdentity = identity;
				}
				else
				{
					Logger.wirte(3, "tradeLibrary 为Null");
				}
				this.timerHQ.Stop();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public TMainForm(bool Agency, ITradeLibrary tradeLibrary, string sIdentity) : this(tradeLibrary, sIdentity)
		{
			this.dataProcess.IsAgency = Agency;
			if (this.dataProcess.IsAgency)
			{
				Logger.wirte(4, "电话下单TMainForm()成功");
				return;
			}
			Logger.wirte(1, "电话下单TMainForm()失败");
		}
		private void InitTaskbarNotifier()
		{
		}
		private void InitDictionarySemaphore()
		{
			this.DictionarySemaphore.Add("QuerySysTime", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadCOF10_2", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadCOF10", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadHDI", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadOI", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadFI", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("WithdrawLossProfit", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadOIF3", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("WithdrawOrderX", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadOIF4", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadHDIF5", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadHIF6", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadFIF7", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("DelegateLoadCIF8", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("YuJingShowMessage", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("UpDataCommodityInfoCtrl", new AutoResetEvent(true));
			this.DictionarySemaphore.Add("FillTradeCtrl", new AutoResetEvent(true));
		}
		private void InitPanelLock()
		{
			try
			{
				this.panelLock.Width = this.panelLock.BackgroundImage.Width;
				this.panelLock.Height = this.panelLock.BackgroundImage.Height;
				this.buttonUnLock.BackgroundImage = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_UnlockButton");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void Initdata()
		{
			try
			{
				if (this.InitData == null)
				{
					Logger.wirte(3, "无法显示更新数据");
					if (this.dataProcess.IsAgency)
					{
						Global.AgencyCommodityData = this.dataProcess.QueryAllCommodityInfo(this.marketID);
					}
					else
					{
						Global.CommodityData = this.dataProcess.QueryAllCommodityInfo(this.marketID);
					}
					if (this.dataProcess.IsAgency)
					{
						Global.AgencyEspecialMemberList = this.dataProcess.GetAgencyEspecialMemberList(Global.UserID);
					}
					else
					{
						Global.EspecialMemberList = this.dataProcess.GetEspecialMemberList();
						Global.AllEspecialMemberList = this.dataProcess.GetAllEspecialMemberList();
					}
					HoldingDetailRequestVO holdingDetailRequestVO = new HoldingDetailRequestVO();
					holdingDetailRequestVO.UserID = Global.UserID;
					this.dataProcess.TradeLibrary.HoldPtByPrice(holdingDetailRequestVO);
				}
				else
				{
					this.InitData("初始化", 0);
					this.InitData("正在加载商品数据", 5);
					this.timerQuerySys = Tools.StrToInt((string)Global.HTConfig["MaxIdleRefreshButton"], 3) * 1000;
					if (this.dataProcess.IsAgency)
					{
						Global.AgencyCommodityData = this.dataProcess.QueryAllCommodityInfo(this.marketID);
					}
					else
					{
						Global.CommodityData = this.dataProcess.QueryAllCommodityInfo(this.marketID);
					}
					this.InitData("正在加载行情数据", 5);
					if (this.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							Global.AgencyHQCommData = this.dataProcess.QueryAllGNCommodityInfo(this.marketID);
							goto IL_1B8;
						}
					}
					lock (Global.HQCommDataLock)
					{
						Global.HQCommData = this.dataProcess.QueryAllGNCommodityInfo(this.marketID);
					}
					IL_1B8:
					this.InitData("正在加载交易对手数据", 5);
					if (this.dataProcess.IsAgency)
					{
						Global.AgencyEspecialMemberList = this.dataProcess.GetAgencyEspecialMemberList(Global.UserID);
					}
					else
					{
						Global.EspecialMemberList = this.dataProcess.GetEspecialMemberList();
						Global.AllEspecialMemberList = this.dataProcess.GetAllEspecialMemberList();
					}
					this.InitData("正在加载持仓信息数据", 5);
					this.DelegateLoadHDI(false);
					this.F2Flag = false;
					this.InitData("正在加载账户信息数据", 5);
					this.DelegateLoadFI(false);
					if (this.dataProcess.sIdentity == Identity.Member)
					{
						FirmFundsInfoResponseVO firmFundsInfo = this.dataProcess.TradeLibrary.GetFirmFundsInfo(Global.UserID);
						this._MemberMinRiskFund = firmFundsInfo.MinRiskFund;
						MemberStatus sMemberStatus = (MemberStatus)Enum.Parse(typeof(MemberStatus), firmFundsInfo.Status);
						Global.sMemberStatus = sMemberStatus;
						this.timerMember = Tools.StrToInt((string)Global.HTConfig["MemberRefreshtime"], 3);
						this.Member = true;
						Global.sMemberType = MemberType.C;
						if (firmFundsInfo.MemberType != null)
						{
							string a;
							if ((a = firmFundsInfo.MemberType.ToUpper()) != null)
							{
								if (a == "C")
								{
									Global.sMemberType = MemberType.C;
									goto IL_33C;
								}
								if (a == "B")
								{
									Global.sMemberType = MemberType.B;
									goto IL_33C;
								}
							}
							Global.sMemberType = MemberType.C;
						}
					}
					else if (this.dataProcess.IsAgency)
					{
						this.Member = false;
						Global.sAgencyMemberType = MemberType.C;
					}
					else
					{
						this.Member = false;
						Global.sMemberType = MemberType.C;
					}
					IL_33C:
					this.InitData("正在加载委托单信息数据", 10);
					this.DelegateLoadOI(false);
					this.DelegateLoadOIF3();
					this.InitData("正在加载成交单信息", 5);
					this.InitComboxLableF4();
					this.DelegateLoadOIF4();
					this.InitData("正在加载持仓明细数据", 5);
					this.DelegateLoadHDIF5();
					this.InitData("正在加载f6持仓汇总数据", 5);
					this.DelegateLoadHIF6();
					this.InitData("正在加载账户信息数据", 5);
					this.DelegateLoadFIF7();
					this.InitData("正在加载商品详细信息数据", 5);
					this.InitCommodityInfoF8();
					this.DelegateLoadCIF8();
					this.InitData("正在加载预警信息数据", 5);
					this.DelegateLoadCIF9();
					if (this.dataProcess.sIdentity == Identity.Member)
					{
						this.InitData("正在加载客户下单情况数据", 5);
						this.InitCustomerOrderF10();
						this.DelegateLoadCOF10();
						this.InitData("正在加载会员资金信息数据", 5);
						this.DelegateLoadCOF10_2();
					}
					this.F3Flag = false;
					this.F4Flag = false;
					this.F5Flag = false;
					this.F6Flag = false;
					this.F7Flag = false;
					this.F8Flag = false;
					this.F10Flag = false;
					this.InitData("加载完成", 100);
					this.timerHQ.Start();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void InitToolBarText()
		{
			try
			{
				if (this.Total == null || this.Total.Length == 0)
				{
					this.Total = "合计";
				}
				this.tabPage1.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F2_TAB_TEXT"));
				this.tabPage2.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F3_TAB_TEXT"));
				this.tabPage3.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F4_TAB_TEXT"));
				this.tabPage4.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F5_TAB_TEXT"));
				this.tabPage5.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F6_TAB_TEXT"));
				this.tabPage6.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F7_TAB_TEXT"));
				this.tabPage7.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F8_TAB_TEXT"));
				this.tabPage8.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F9_TAB_TEXT"));
				this.tabPage9.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "F10_TAB_TEXT"));
				this.MessageInfo.Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void AddTradeCtrl(string strName, int x, int y, string CommodityID)
		{
			try
			{
				TradeCtrl tradeCtrl = new TradeCtrl();
				tradeCtrl.m_rm = Global.m_PMESResourceManager;
				tradeCtrl.Parent = this.splitContainerHQ.Panel2;
				this.m_MyTradeCtrl.Add(CommodityID, tradeCtrl);
				this.m_MyTradeCtrl[CommodityID].InitTradeCtrl();
				this.m_MyTradeCtrl[CommodityID].SetTradeNameLable(strName);
				this.m_MyTradeCtrl[CommodityID].CommodityID = CommodityID;
				this.m_MyTradeCtrl[CommodityID].TradeRefreashed += new TradeCtrl.TradeHanlder(this.TradeCtrlEvent);
				this.m_MyTradeCtrl[CommodityID]._ShowMenu += new TradeCtrl.ShowMenu(this.SetMenuDisenable);
				this.m_MyTradeCtrl[CommodityID].Invalidate();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void InitMenuText()
		{
			try
			{
				this.toolStripMenuItemXP.Text = Global.m_PMESResourceManager.GetString("PMESStr_PF_TRADETYPESTRARR");
				this.gbOrderInfo.Text = Global.m_PMESResourceManager.GetString("PMESStr_TM_MESSAGEFORM");
				this.toolStripMenuItemXO.Text = Global.m_PMESResourceManager.GetString("PMESStr_TM_XO_TEXT");
				this.toolStripMenuItemWithdrawOrder.Text = string.Format("撤销{0}", Global.m_PMESResourceManager.GetString("PMESStr_TM_MESSAGEFORM"));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void ToolsBarButtonClick(ToolsBarButton toolsBarButton)
		{
			switch (toolsBarButton)
			{
			case ToolsBarButton.CreateWareHoust:
				this.tbCreatWareHouse_Click(null, null);
				return;
			case ToolsBarButton.PWareHouse:
				this.tbPWareHouse_Click(null, null);
				return;
			case ToolsBarButton.Alarm:
				this.MenuAlarm_Click(null, null);
				return;
			case ToolsBarButton.SetUp:
				this.tbSetUp_Click(null, null);
				return;
			case ToolsBarButton.Lock:
				this.tbLock_Click(null, null);
				return;
			case ToolsBarButton.Calc:
				this.tbCalc_Click(null, null);
				return;
			case ToolsBarButton.logOut:
				this.tblogOut_Click(null, null);
				return;
			case ToolsBarButton.Help:
				this.tbHelp_Click(null, null);
				return;
			default:
				return;
			}
		}
		private void tbCreatWareHouse_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.identitystatus())
				{
					this.Ordersform = new NewOrdersform(this);
					this.Ordersform.ShowDialog();
					if (this.Ordersform.IsCloseButtonOKOrCancel)
					{
						this.DelegateRefresh();
						this.DelegateLoadHIF6();
						this.UpDataTabCtrl();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void tbPWareHouse_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.identitystatus())
				{
					if (this.dataProcess.IsAgency)
					{
						if (Global.AgencyCommodityData == null)
						{
							return;
						}
					}
					else if (Global.CommodityData == null)
					{
						return;
					}
					PWarehouseForm pWarehouseForm = new PWarehouseForm(this);
					pWarehouseForm.CloseTradeType = TradeType.ShiJiaDan;
					pWarehouseForm.CurrentBuySell = BuySell.Buy.ToString("d");
					if (this.dataProcess.IsAgency)
					{
						using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, CommodityInfo> current = enumerator.Current;
								CloseCommodityInfo closeCommodityInfo = new CloseCommodityInfo();
								closeCommodityInfo.CommodityID = current.Key;
								if (this.dataProcess.IsAgency)
								{
									if (Global.AgencyHoldingInfoList == null || Global.AgencyHoldingInfoList.Count == 0)
									{
										continue;
									}
									using (List<HoldingInfo>.Enumerator enumerator2 = Global.AgencyHoldingInfoList.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											HoldingInfo current2 = enumerator2.Current;
											if (current2.CommodityID.Equals(closeCommodityInfo.CommodityID))
											{
												if (current2.TradeType == 1)
												{
													closeCommodityInfo.CloseMaxSellQty = Convert.ToInt64(current2.Qty);
												}
												else if (current2.TradeType == 2)
												{
													closeCommodityInfo.CloseMaxBuyQty = Convert.ToInt64(current2.Qty);
												}
											}
										}
										goto IL_1CD;
									}
								}
								if (Global.HoldingInfoList == null || Global.HoldingInfoList.Count == 0)
								{
									continue;
								}
								foreach (HoldingInfo current3 in Global.HoldingInfoList)
								{
									if (current3.CommodityID.Equals(closeCommodityInfo.CommodityID))
									{
										if (current3.TradeType == 1)
										{
											closeCommodityInfo.CloseMaxSellQty = Convert.ToInt64(current3.Qty);
										}
										else if (current3.TradeType == 2)
										{
											closeCommodityInfo.CloseMaxBuyQty = Convert.ToInt64(current3.Qty);
										}
									}
								}
								IL_1CD:
								pWarehouseForm.CloseCommodityInfoList.Add(current.Key, closeCommodityInfo);
							}
							goto IL_3A5;
						}
					}
					foreach (KeyValuePair<string, CommodityInfo> current4 in Global.CommodityData)
					{
						CloseCommodityInfo closeCommodityInfo2 = new CloseCommodityInfo();
						closeCommodityInfo2.CommodityID = current4.Key;
						if (this.dataProcess.IsAgency)
						{
							if (Global.AgencyHoldingInfoList == null || Global.AgencyHoldingInfoList.Count == 0)
							{
								continue;
							}
							using (List<HoldingInfo>.Enumerator enumerator5 = Global.AgencyHoldingInfoList.GetEnumerator())
							{
								while (enumerator5.MoveNext())
								{
									HoldingInfo current5 = enumerator5.Current;
									if (current5.CommodityID.Equals(closeCommodityInfo2.CommodityID))
									{
										if (current5.TradeType == 1)
										{
											closeCommodityInfo2.CloseMaxSellQty = Convert.ToInt64(current5.Qty);
										}
										else if (current5.TradeType == 2)
										{
											closeCommodityInfo2.CloseMaxBuyQty = Convert.ToInt64(current5.Qty);
										}
									}
								}
								goto IL_375;
							}
						}
						if (Global.HoldingInfoList == null || Global.HoldingInfoList.Count == 0)
						{
							continue;
						}
						foreach (HoldingInfo current6 in Global.HoldingInfoList)
						{
							if (current6.CommodityID.Equals(closeCommodityInfo2.CommodityID))
							{
								if (current6.TradeType == 1)
								{
									closeCommodityInfo2.CloseMaxSellQty = Convert.ToInt64(current6.Qty);
								}
								else if (current6.TradeType == 2)
								{
									closeCommodityInfo2.CloseMaxBuyQty = Convert.ToInt64(current6.Qty);
								}
							}
						}
						IL_375:
						pWarehouseForm.CloseCommodityInfoList.Add(current4.Key, closeCommodityInfo2);
					}
					IL_3A5:
					pWarehouseForm.ShowDialog();
					if (pWarehouseForm.IsCloseButtonOKOrCancel)
					{
						this.DelegateRefresh();
						this.DelegateLoadHIF6();
						this.UpDataTabCtrl();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void tbNotice_Click(object sender, EventArgs e)
		{
			Global.broadcastFlag = false;
		}
		private void tbAlarm_Click(object sender, EventArgs e)
		{
			this.tabTMain.SelectedIndex = 7;
		}
		private void MenuAlarm_Click(object sender, EventArgs e)
		{
			this.tabTMain.SelectedIndex = 7;
			this.NEWToolStripMenuItemYJF9_Click_1(sender, e);
		}
		private void tbSetUp_Click(object sender, EventArgs e)
		{
			this.userSet = new UserSet(this);
			this.userSet.TopMost = true;
			this.userSet.ShowDialog();
		}
		private void tbLock_Click(object sender, EventArgs e)
		{
			this.LockSet(false);
		}
		private void tbCalc_Click(object sender, EventArgs e)
		{
			if (this.m_pCalc == null)
			{
				this.m_pCalc = Process.Start("Calc.exe");
				return;
			}
			try
			{
				this.m_pCalc.Kill();
			}
			catch (Exception)
			{
				this.m_pCalc = Process.Start("Calc.exe");
			}
		}
		private void tbHelp_Click(object sender, EventArgs e)
		{
		}
		private void MenuLimitCreatWarehouse_Click(object sender, EventArgs e)
		{
		}
		private void MenuAbout_Click(object sender, EventArgs e)
		{
			if (this.m_fAbout != null)
			{
				this.m_fAbout.ShowDialog();
			}
		}
		private void InitDataGridTitle()
		{
			this.HqTitle.Text = Global.m_PMESResourceManager.GetString("PMESStr_HQ_QUOTE_GB_TEXT");
			this.gbHoldingDetailInfo.Text = Global.m_PMESResourceManager.GetString("PMESStr_GB_HDI");
			this.gbOrderInfo.Text = Global.m_PMESResourceManager.GetString("PMESStr_GB_OI");
			this.gbFirmInfo.Text = Global.m_PMESResourceManager.GetString("PMESStr_FI_GB_FIRMINFO_TEXT");
		}
		private void SetHQDataGridColText()
		{
			this.HQ_DataGrid.ReadOnly = true;
			this.HQ_DataGrid.AutoGenerateColumns = false;
			this.HQ_DataGrid.Columns["ColImage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.HQ_DataGrid.Columns["ColImage"].HeaderText = Global.m_PMESResourceManager.GetString("");
			this.HQ_DataGrid.Columns["CommodityName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.HQ_DataGrid.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_GOODS_NAME");
			DataGridViewCellStyle defaultCellStyle = this.HQ_DataGrid.Columns["SellPrice"].DefaultCellStyle;
			defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.HQ_DataGrid.Columns["SellPrice"].DefaultCellStyle.ApplyStyle(defaultCellStyle);
			this.HQ_DataGrid.Columns["SellPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_SELL_PRICE");
			defaultCellStyle = this.HQ_DataGrid.Columns["BuyPrice"].DefaultCellStyle;
			defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.HQ_DataGrid.Columns["BuyPrice"].DefaultCellStyle = defaultCellStyle;
			this.HQ_DataGrid.Columns["BuyPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_BUY_PRICE");
			defaultCellStyle = this.HQ_DataGrid.Columns["HightPrice"].DefaultCellStyle;
			defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.HQ_DataGrid.Columns["HightPrice"].DefaultCellStyle = defaultCellStyle;
			this.HQ_DataGrid.Columns["HightPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_HIGHT_PRICE");
			defaultCellStyle = this.HQ_DataGrid.Columns["LowPrice"].DefaultCellStyle;
			defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.HQ_DataGrid.Columns["LowPrice"].DefaultCellStyle = defaultCellStyle;
			this.HQ_DataGrid.Columns["LowPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_LOW_PRICE");
			defaultCellStyle = this.HQ_DataGrid.Columns["PriceTime"].DefaultCellStyle;
			defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			this.HQ_DataGrid.Columns["PriceTime"].DefaultCellStyle = defaultCellStyle;
			this.HQ_DataGrid.Columns["PriceTime"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HQ_DGV_PRICE_TIME");
			this.HQ_DataGrid.Columns["CommodityID"].HeaderText = "CommodityID";
			this.HQ_DataGrid.Columns["CommodityID"].Visible = false;
		}
		public void InitCommodityInfo()
		{
			try
			{
				Dictionary<string, CommodityInfo> dictionary;
				if (this.dataProcess.IsAgency)
				{
					dictionary = Global.AgencyCommodityData;
				}
				else
				{
					dictionary = Global.CommodityData;
				}
				int num = 0;
				if (dictionary != null)
				{
					int num2 = 0;
					foreach (KeyValuePair<string, CommodityInfo> current in dictionary)
					{
						Rectangle rectangle = new Rectangle(this.m_iTradeCtrl * 2 * (num + 1) + TradeCtrl.m_iWidth * num, this.m_iTradeCtrl * 2 * (num2 + 1) + num2 * TradeCtrl.m_iHeight, TradeCtrl.m_iWidth, TradeCtrl.m_iHeight);
						this.AddTradeCtrl(current.Value.CommodityName, rectangle.X, rectangle.Y, current.Value.CommodityID);
					}
					this.MoveTradeCtrl(this.splitContainerHQ.Panel2);
					this.RefreshGN();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpDataCommodityInfoCtrl()
		{
			try
			{
				DataSet commodityHQInfo = this.dataProcess.GetCommodityHQInfo();
				this.HandleCreated();
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("UpDataCommodityInfoCtrl"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["UpDataCommodityInfoCtrl"];
					threadPoolParameter.obj = commodityHQInfo;
				}
				else
				{
					this.DictionarySemaphore.Add("UpDataCommodityInfoCtrl", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["UpDataCommodityInfoCtrl"];
					threadPoolParameter.obj = commodityHQInfo;
				}
				TMainForm.FillHQDataGrid method = new TMainForm.FillHQDataGrid(this.FillDataGrid);
				base.BeginInvoke(method, new object[]
				{
					threadPoolParameter
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillTradeCtrl()
		{
			try
			{
				this.dataProcess.GetCommodityHQInfo();
				this.HandleCreated();
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("FillTradeCtrl"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["FillTradeCtrl"];
					threadPoolParameter.obj = null;
				}
				else
				{
					this.DictionarySemaphore.Add("FillTradeCtrl", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["FillTradeCtrl"];
					threadPoolParameter.obj = null;
				}
				TMainForm.FillHQCtrl method = new TMainForm.FillHQCtrl(this.UpdataTradeCtrl);
				base.BeginInvoke(method, new object[]
				{
					threadPoolParameter
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdataTradeCtrl(object obj)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)obj;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						return;
					}
				}
				else if (Global.CommodityData == null)
				{
					return;
				}
				this.HandleCreated();
				Dictionary<string, CommData> dictionary = null;
				if (this.dataProcess.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData == null)
						{
							dictionary = null;
						}
						else
						{
							dictionary = Global.gAgencyHQCommData;
						}
						goto IL_A1;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData == null)
					{
						dictionary = null;
					}
					else
					{
						dictionary = Global.gHQCommData;
					}
				}
				IL_A1:
				if (this.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							if (dictionary == null || !dictionary.ContainsKey(current.Key))
							{
								this.m_MyTradeCtrl[current.Value.CommodityID].SetHightPrice("--");
								this.m_MyTradeCtrl[current.Value.CommodityID].SetLowPrice("--");
								this.m_MyTradeCtrl[current.Value.CommodityID].SetCurrentPriceSell("--");
								this.m_MyTradeCtrl[current.Value.CommodityID].SetCurrentPriceBuy("--");
								this.m_MyTradeCtrl[current.Value.CommodityID].Invalidate();
							}
							else
							{
								int minSpreadPriceCount = BizController.GetMinSpreadPriceCount(Global.AgencyCommodityData[current.Key]);
								this.m_MyTradeCtrl[current.Value.CommodityID].SetHightPrice(dictionary[current.Key].High.ToString(string.Format("f{0}", minSpreadPriceCount)));
								this.m_MyTradeCtrl[current.Value.CommodityID].SetLowPrice(dictionary[current.Key].Low.ToString(string.Format("f{0}", minSpreadPriceCount)));
								string currentPriceBuy = dictionary[current.Key].BuyPrice.ToString();
								string currentPriceSell = dictionary[current.Key].SellPrice.ToString();
								this.m_MyTradeCtrl[current.Value.CommodityID].SetCurrentPriceSell(currentPriceSell);
								this.m_MyTradeCtrl[current.Value.CommodityID].SetCurrentPriceBuy(currentPriceBuy);
								this.m_MyTradeCtrl[current.Value.CommodityID].Invalidate();
							}
						}
						goto IL_4E7;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					if (dictionary == null || !dictionary.ContainsKey(current2.Key))
					{
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetHightPrice("--");
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetLowPrice("--");
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetCurrentPriceSell("--");
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetCurrentPriceBuy("--");
						this.m_MyTradeCtrl[current2.Value.CommodityID].Invalidate();
					}
					else
					{
						int minSpreadPriceCount2 = BizController.GetMinSpreadPriceCount(Global.CommodityData[current2.Key]);
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetHightPrice(dictionary[current2.Key].High.ToString(string.Format("f{0}", minSpreadPriceCount2)));
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetLowPrice(dictionary[current2.Key].Low.ToString(string.Format("f{0}", minSpreadPriceCount2)));
						string currentPriceBuy2 = dictionary[current2.Key].BuyPrice.ToString();
						string currentPriceSell2 = dictionary[current2.Key].SellPrice.ToString();
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetCurrentPriceSell(currentPriceSell2);
						this.m_MyTradeCtrl[current2.Value.CommodityID].SetCurrentPriceBuy(currentPriceBuy2);
						this.m_MyTradeCtrl[current2.Value.CommodityID].Invalidate();
					}
				}
				IL_4E7:;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
			}
		}
		private void TradeCtrlEvent(TradeMenu _tradeMenu, string _commodityID, bool _buySell)
		{
			try
			{
				switch (_tradeMenu)
				{
				case TradeMenu.em_OpenPrice_S:
					this.Ordersform = new NewOrdersform(this);
					this.Ordersform.CurrentBuySell = (_buySell ? BuySell.Sell.ToString("d") : BuySell.Buy.ToString("d"));
					this.Ordersform.CurrentCommodityId = _commodityID;
					this.Ordersform.OpenTradeType = TradeType.ShiJiaDan;
					this.Ordersform.ShowDialog();
					if (this.Ordersform.IsCloseButtonOKOrCancel)
					{
						this.DelegateRefresh();
						this.DelegateLoadHIF6();
						this.UpDataTabCtrl();
					}
					break;
				case TradeMenu.em_ClosePrice_S:
					if (this.dataProcess.IsAgency)
					{
						if (Global.AgencyCommodityData == null)
						{
							break;
						}
					}
					else if (Global.CommodityData == null)
					{
						break;
					}
					this.pWarehouseForm = new PWarehouseForm(this);
					this.pWarehouseForm.CloseTradeType = TradeType.ShiJiaDan;
					this.pWarehouseForm.CurrentCommodityId = _commodityID;
					this.pWarehouseForm.IsCloseSpecificOrder = false;
					this.pWarehouseForm.CurrentBuySell = (_buySell ? BuySell.Sell.ToString("d") : BuySell.Buy.ToString("d"));
					if (this.dataProcess.IsAgency)
					{
						using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, CommodityInfo> current = enumerator.Current;
								CloseCommodityInfo closeCommodityInfo = new CloseCommodityInfo();
								closeCommodityInfo.CommodityID = current.Key;
								foreach (DataGridViewRow dataGridViewRow in (IEnumerable)this.dgvHoldingInfoF6.Rows)
								{
									if (dataGridViewRow.Cells["CommodityID"].Value.ToString().Equals(closeCommodityInfo.CommodityID))
									{
										if (dataGridViewRow.Cells["BuySell"].Value.ToString().Equals(BuySell.Buy.ToString("d")))
										{
											closeCommodityInfo.CloseMaxSellQty = Convert.ToInt64(dataGridViewRow.Cells["Qty"].Value);
										}
										else if (dataGridViewRow.Cells["BuySell"].Value.ToString().Equals(BuySell.Sell.ToString("d")))
										{
											closeCommodityInfo.CloseMaxBuyQty = Convert.ToInt64(dataGridViewRow.Cells["Qty"].Value);
										}
									}
								}
								this.pWarehouseForm.CloseCommodityInfoList.Add(current.Key, closeCommodityInfo);
							}
							goto IL_4B7;
						}
					}
					foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
					{
						CloseCommodityInfo closeCommodityInfo2 = new CloseCommodityInfo();
						closeCommodityInfo2.CommodityID = current2.Key;
						foreach (DataGridViewRow dataGridViewRow2 in (IEnumerable)this.dgvHoldingInfoF6.Rows)
						{
							if (dataGridViewRow2.Cells["CommodityID"].Value.ToString().Equals(closeCommodityInfo2.CommodityID))
							{
								if (dataGridViewRow2.Cells["BuySell"].Value.ToString().Equals(BuySell.Buy.ToString("d")))
								{
									closeCommodityInfo2.CloseMaxSellQty = Convert.ToInt64(dataGridViewRow2.Cells["Qty"].Value);
								}
								else if (dataGridViewRow2.Cells["BuySell"].Value.ToString().Equals(BuySell.Sell.ToString("d")))
								{
									closeCommodityInfo2.CloseMaxBuyQty = Convert.ToInt64(dataGridViewRow2.Cells["Qty"].Value);
								}
							}
						}
						this.pWarehouseForm.CloseCommodityInfoList.Add(current2.Key, closeCommodityInfo2);
					}
					IL_4B7:
					this.pWarehouseForm.ShowDialog();
					if (this.pWarehouseForm.IsCloseButtonOKOrCancel)
					{
						this.DelegateRefresh();
						this.DelegateLoadHIF6();
						this.UpDataTabCtrl();
					}
					break;
				case TradeMenu.em_OpenPrice_L:
					this.Ordersform = new NewOrdersform(this);
					this.Ordersform.CurrentBuySell = (_buySell ? BuySell.Sell.ToString("d") : BuySell.Buy.ToString("d"));
					this.Ordersform.CurrentCommodityId = _commodityID;
					this.Ordersform.OpenTradeType = TradeType.XianJiaDan;
					this.Ordersform.ShowDialog();
					if (this.Ordersform.IsCloseButtonOKOrCancel)
					{
						this.DelegateRefresh();
						this.DelegateLoadHIF6();
						this.UpDataTabCtrl();
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void FillDataGrid(object dataTable)
		{
			AutoResetEvent autoResetEvent = null;
			DataSet dataSet = null;
			try
			{
				if (this.HQ_DataGrid.RowCount == 0)
				{
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)dataTable;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					dataSet = (DataSet)threadPoolParameter.obj;
					DataView dataView = new DataView(dataSet.Tables["HQ"]);
					this.HQ_DataGrid.DataSource = dataView.Table;
					this.HQ_DataGrid.ClearSelection();
					this.SetHQDataGridColText();
					if (this.HQ_DataGrid.Rows.Count != 0)
					{
						for (int i = 0; i < this.HQ_DataGrid.Rows.Count; i++)
						{
							string key = (string)this.HQ_DataGrid.Rows[i].Cells["CommodityID"].Value;
							if (this.m_MyTradeCtrl.ContainsKey(key))
							{
								TradeCtrl tradeCtrl = this.m_MyTradeCtrl[key];
								tradeCtrl.m_rm = Global.m_PMESResourceManager;
								DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
								dataGridViewCellStyle.ForeColor = tradeCtrl.m_crsText;
								this.HQ_DataGrid.Rows[i].Cells["SellPrice"].Style = dataGridViewCellStyle;
								this.HQ_DataGrid.Rows[i].Cells["BuyPrice"].Style = dataGridViewCellStyle;
								this.HQ_DataGrid.Rows[i].Cells["ColImage"].Value = tradeCtrl.m_imgState;
							}
						}
					}
					if (this.HQ_DataGrid.Columns.Count > 0)
					{
						for (int j = 0; j < this.HQ_DataGrid.Columns.Count; j++)
						{
							if (j > 3)
							{
								this.HQ_DataGrid.Columns[j].Visible = false;
							}
						}
					}
				}
				else
				{
					this.UPDataHQDataGrid(dataSet);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
			}
		}
		private void UPDataHQDataGrid(DataSet dataTable)
		{
			try
			{
				Dictionary<string, CommData> dictionary = null;
				Dictionary<string, CommodityInfo> dictionary2 = null;
				if (this.HQ_DataGrid != null)
				{
					if (this.HQ_DataGrid.RowCount != 0)
					{
						if (this.dataProcess.IsAgency)
						{
							lock (Global.AgencyHQCommDataLock)
							{
								if (Global.AgencyHQCommData == null)
								{
									dictionary = null;
								}
								else
								{
									dictionary = Global.gAgencyHQCommData;
								}
							}
							lock (Global.AgencyCommodityData)
							{
								if (Global.AgencyCommodityData == null)
								{
									dictionary2 = null;
								}
								else
								{
									dictionary2 = Global.AgencyCommodityData;
								}
								goto IL_D0;
							}
						}
						lock (Global.HQCommDataLock)
						{
							if (Global.HQCommData == null)
							{
								dictionary = null;
							}
							else
							{
								dictionary = Global.gHQCommData;
							}
						}
						lock (Global.CommodityData)
						{
							if (Global.CommodityData == null)
							{
								dictionary2 = null;
							}
							else
							{
								dictionary2 = Global.CommodityData;
							}
						}
						IL_D0:
						if (dictionary != null)
						{
							int rowCount = this.HQ_DataGrid.RowCount;
							for (int i = 0; i < rowCount; i++)
							{
								string key = this.HQ_DataGrid.Rows[i].Cells["CommodityID"].Value.ToString();
								if (dictionary.ContainsKey(key))
								{
									int num = 2;
									if (dictionary2 != null && dictionary2.ContainsKey(key))
									{
										num = BizController.GetMinSpreadPriceCount(dictionary2[key]);
									}
									this.HQ_DataGrid.Rows[i].Cells["SellPrice"].Value = dictionary[key].SellPrice.ToString(string.Format("F{0}", num));
									this.HQ_DataGrid.Rows[i].Cells["BuyPrice"].Value = dictionary[key].BuyPrice.ToString(string.Format("F{0}", num));
									this.HQ_DataGrid.Rows[i].Cells["HightPrice"].Value = dictionary[key].High.ToString(string.Format("F{0}", num));
									this.HQ_DataGrid.Rows[i].Cells["LowPrice"].Value = dictionary[key].Low.ToString(string.Format("F{0}", num));
									this.HQ_DataGrid.Rows[i].Cells["PriceTime"].Value = dictionary[key].UpdateTime.ToString();
									string key2 = (string)this.HQ_DataGrid.Rows[i].Cells["CommodityID"].Value;
									if (this.m_MyTradeCtrl.ContainsKey(key2))
									{
										TradeCtrl tradeCtrl = this.m_MyTradeCtrl[key2];
										tradeCtrl.m_rm = Global.m_PMESResourceManager;
										DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
										dataGridViewCellStyle.ForeColor = tradeCtrl.m_crsText;
										this.HQ_DataGrid.Rows[i].Cells["SellPrice"].Style = dataGridViewCellStyle;
										this.HQ_DataGrid.Rows[i].Cells["BuyPrice"].Style = dataGridViewCellStyle;
										this.HQ_DataGrid.Rows[i].Cells["ColImage"].Value = tradeCtrl.m_imgState;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.splitContainer1.Panel1Collapsed = true;
				this.SysFont = Global.GetIniFont();
				float size = this.SysFont.Size;
				float num = 9f / size;
				TradeCtrl.m_iHeight = (int)(119f / num);
				TradeCtrl.m_iWidth = (int)(170f / num);
				this.splitContainer3.Panel1Collapsed = (this.dataProcess.sIdentity == Identity.Member);
				this.splitContainer3.Panel2Collapsed = (this.dataProcess.sIdentity == Identity.Client);
				this.splitContainer5.Panel2Collapsed = true;
				this.splitContainer6.Panel2Collapsed = true;
				if (this.dataProcess.sIdentity == Identity.Client)
				{
					this.tabTMain.TabPages.Remove(this.tabPage10);
				}
				this.InitTaskbarNotifier();
				this.InitToolBarText();
				this.InitMenuText();
				this.InitDataGridTitle();
				this.InitCommodityInfo();
				this.InitComboxLable();
				this.InitPanelLock();
				this.user.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "SS_USERNAME")) + Global.UserID.ToString();
				this.FillTradeCtrl();
				this.UpDataCommodityInfoCtrl();
				this.QuerySysTime();
				this.StartLock();
				this.Font = this.SysFont;
				this.ReLayoutControl();
				this.AgencyLoadForm();
				this.UpdataSystemEnvironment();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void AgencyLoadForm()
		{
			try
			{
				if (this.dataProcess.IsAgency)
				{
					this.tabTMain.Controls.Remove(this.tabPage8);
					this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.YuJingNotifier);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void EnableControls(bool enable, string messageInfo)
		{
			try
			{
				this.messageInfomation = messageInfo;
				this.MessageInfo.Text = messageInfo;
				this.MessageInfo.BringToFront();
				this.MessageInfo.Visible = !enable;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void timerHQ_Tick(object sender, EventArgs e)
		{
			try
			{
				this.RefreshGN();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void RefreshGN()
		{
			try
			{
				this.RefreshGNCommodity(this.marketID);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void RefreshGNCommodity(object market)
		{
			try
			{
				if (this.refreshGNFlag)
				{
					this.refreshGNFlag = false;
					int num = 0;
					lock (this.LocktimerLockCount)
					{
						num = this.timerLockCount;
					}
					if ((double)this.timerLockRefresh / this.timerHQ.Interval <= (double)num)
					{
						if (this.refreshHoldingDetailInfo == 0 && this.iSelecttab != 0)
						{
							TMainForm.UpdataHDIEvent method = new TMainForm.UpdataHDIEvent(this.DelegateLoadHDI);
							this.HandleCreated();
							base.BeginInvoke(method, new object[]
							{
								true
							});
						}
						lock (this.LocktimerLockCount)
						{
							this.timerLockCount = 0;
						}
						this.HandleCreated();
						TMainForm.Checkstatus checkstatus = new TMainForm.Checkstatus(this.status_DoubleClick);
						Delegate arg_D1_1 = checkstatus;
						object[] array = new object[2];
						array[0] = this;
						base.BeginInvoke(arg_D1_1, array);
						if (this.Connect)
						{
							if (this.dataProcess.IsAgency)
							{
								Dictionary<string, CommData> dictionary = this.dataProcess.QueryAllGNCommodityInfo((string)market);
								if (dictionary != null)
								{
									Global.AgencyHQCommData = dictionary;
								}
								else
								{
									Global.AgencyHQCommData = null;
								}
							}
							else
							{
								Dictionary<string, CommData> dictionary2 = this.dataProcess.QueryAllGNCommodityInfo((string)market);
								if (dictionary2 != null)
								{
									Global.HQCommData = dictionary2;
								}
								else
								{
									Global.HQCommData = null;
								}
							}
						}
						if (this.HQRefreashed != null)
						{
							this.HQRefreashed();
						}
					}
					int num2 = 0;
					lock (this.LockQuerySysCount)
					{
						num2 = this.QuerySysCount;
					}
					if ((double)this.timerQuerySys / this.timerHQ.Interval <= (double)num2)
					{
						this.QuerySysTime();
						lock (this.LockQuerySysCount)
						{
							this.QuerySysCount = 0;
							goto IL_1D0;
						}
					}
					lock (this.LockQuerySysCount)
					{
						this.QuerySysCount++;
					}
					IL_1D0:
					lock (this.LocktimerLockCount)
					{
						this.timerLockCount++;
					}
					this.refreshGNFlag = true;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				this.refreshGNFlag = true;
			}
		}
		private void QuerySysTime()
		{
			try
			{
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("QuerySysTime"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["QuerySysTime"];
					threadPoolParameter.obj = null;
				}
				else
				{
					this.DictionarySemaphore.Add("QuerySysTime", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["QuerySysTime"];
					threadPoolParameter.obj = null;
				}
				WaitCallback callBack = new WaitCallback(this.refreshTime);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetStatus(object o)
		{
			try
			{
				if (this.connectStatus == ConnectStatus.Disconnect)
				{
					this.status.BackColor = Color.Red;
					this.status.Text = "断开";
					this.F2Flag = false;
					this.F3Flag = false;
					this.F4Flag = false;
					this.F5Flag = false;
					this.F6Flag = false;
					this.F7Flag = false;
					this.F8Flag = false;
					this.F10Flag = false;
					this.Connect = false;
					this.EnableControls(false, "设置连接状态中");
				}
				else if (this.connectStatus == ConnectStatus.QZDisconnect || this.connectStatus == ConnectStatus.TimeOut)
				{
					this.status.BackColor = Color.Red;
					this.status.Text = "下线";
					this.F2Flag = false;
					this.F3Flag = false;
					this.F4Flag = false;
					this.F5Flag = false;
					this.F6Flag = false;
					this.F7Flag = false;
					this.F8Flag = false;
					this.F10Flag = false;
					this.Connect = false;
					this.timerHQ.Stop();
					this.timerLock.Stop();
					if (this.connectStatus == ConnectStatus.QZDisconnect)
					{
						Logger.wirte(2, "定时器停止， QZDisconnect");
					}
					else if (this.connectStatus == ConnectStatus.TimeOut)
					{
						Logger.wirte(2, "定时器停止， TimeOut");
					}
					this.status_DoubleClick(this, null);
				}
				else if (this.connectStatus == ConnectStatus.Connect)
				{
					this.status.BackColor = Color.Lime;
					this.status.Text = "连接";
					this.F2Flag = true;
					this.F3Flag = true;
					this.F4Flag = true;
					this.F5Flag = true;
					this.F6Flag = true;
					this.F7Flag = true;
					this.F8Flag = true;
					this.F10Flag = true;
					this.Connect = true;
					this.EnableControls(true, "设置连接状态中");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void refreshTime(object obj)
		{
			if (!this.refreshTimeFlag)
			{
				return;
			}
			this.refreshTimeFlag = false;
			this.stoptime++;
			AutoResetEvent autoResetEvent = null;
			try
			{
				Logger.wirte(1, "refreshTime线程启动 1");
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)obj;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				DateTime serverTime = default(DateTime);
				SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
				sysTimeQueryRequestVO.UserID = Global.UserID;
				if (this.dataProcess.IsAgency)
				{
					sysTimeQueryRequestVO.AgencyNo = Global.AgencyNo;
					sysTimeQueryRequestVO.AgencyPhonePassword = Global.AgencyPhonePassword;
				}
				else
				{
					sysTimeQueryRequestVO.AgencyNo = string.Empty;
					sysTimeQueryRequestVO.AgencyPhonePassword = string.Empty;
				}
				SysTimeQueryResponseVO sysTime = this.dataProcess.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
				Logger.wirte(1, "refreshTime线程 2");
				string str = string.Empty;
				string str2 = string.Empty;
				if (sysTime.RetCode == 0L)
				{
					if (this.connectStatus != ConnectStatus.Connect)
					{
						this.connectStatus = ConnectStatus.Connect;
						Logger.wirte(1, "refreshTime线程 2");
						WaitCallback waitCallback = new WaitCallback(this.SetStatus);
						Logger.wirte(1, "refreshTime线程 3");
						this.HandleCreated();
						Delegate arg_114_1 = waitCallback;
						object[] args = new object[1];
						base.BeginInvoke(arg_114_1, args);
						Logger.wirte(1, "refreshTime线程 4");
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
						Logger.wirte(1, "refreshTime线程 5");
						if (this.strcurTradeDay != null && this.strcurTradeDay.Length > 0 && !this.strcurTradeDay.Equals(sysTime.TradeDay))
						{
							Logger.wirte(1, "refreshTime线程 6");
							this.ReLoad();
							Logger.wirte(1, "refreshTime线程 7");
						}
						this.strcurTradeDay = sysTime.TradeDay;
					}
					if (!this.dataProcess.IsAgency && sysTime.LastID != -1L && this.LastID < sysTime.LastID)
					{
						this.LastID = sysTime.LastID;
						Logger.wirte(1, "refreshTime线程 8");
						TMainForm.StringObjCallback method = new TMainForm.StringObjCallback(this.displayMessage);
						this.HandleCreated();
						base.BeginInvoke(method, new object[]
						{
							sysTime.LastID
						});
						this.HandleCreated();
						if (this.PlayMessageEvent != null)
						{
							base.BeginInvoke(this.PlayMessageEvent);
						}
						Logger.wirte(1, "refreshTime线程 9");
						IniFile iniFile = new IniFile(this.IniFileName);
						try
						{
							iniFile.IniWriteValue("SystemMessage", "LastIdControl", this.LastID.ToString());
						}
						catch (Exception ex)
						{
							Logger.wirte(3, "系统公告写入失败！" + ex.ToString());
						}
					}
					if (sysTime.NewTrade == 1 && this.firstRefreshTime)
					{
						if (sysTime.TradeMessageList != null && sysTime.TradeMessageList.Count > 0)
						{
							Hashtable hashtable = new Hashtable();
							int num = sysTime.TradeMessageList.Count - 1;
							for (int i = num; i >= 0; i--)
							{
								TradeMessage tradeMessage = sysTime.TradeMessageList[i];
								if (tradeMessage.OrderNO != 0L)
								{
									if (!hashtable.ContainsKey(tradeMessage.OrderNO))
									{
										Logger.wirte(1, "refreshTime线程 10");
										TradeMessage tradeMessage2 = new TradeMessage();
										tradeMessage2.CommodityID = tradeMessage.CommodityID;
										tradeMessage2.MarketID = tradeMessage.MarketID;
										tradeMessage2.OrderNO = tradeMessage.OrderNO;
										tradeMessage2.TradeQuatity = tradeMessage.TradeQuatity;
										tradeMessage2.SettleBasis = tradeMessage.SettleBasis;
										tradeMessage2.BuySell = tradeMessage.BuySell;
										tradeMessage2.TradeType = tradeMessage.TradeType;
										hashtable.Add(tradeMessage2.OrderNO, tradeMessage2);
										Logger.wirte(1, "refreshTime线程 11");
									}
									else
									{
										TradeMessage tradeMessage3 = (TradeMessage)hashtable[tradeMessage.OrderNO];
										tradeMessage3.TradeQuatity += tradeMessage.TradeQuatity;
										Logger.wirte(1, "refreshTime线程 12");
										sysTime.TradeMessageList.RemoveAt(i);
									}
								}
							}
							foreach (TradeMessage current in sysTime.TradeMessageList)
							{
								StringBuilder stringBuilder = new StringBuilder(256);
								TradeMessage tradeMessage4;
								if (hashtable.ContainsKey(current.OrderNO))
								{
									tradeMessage4 = (TradeMessage)hashtable[current.OrderNO];
								}
								else
								{
									tradeMessage4 = current;
								}
								if (this.dataProcess.IsAgency)
								{
									if (tradeMessage4.CommodityID.Trim().Length > 0 && Global.AgencyCommodityData.ContainsKey(tradeMessage4.CommodityID))
									{
										if (tradeMessage4.TradeType == 1 || tradeMessage4.TradeType == 2 || tradeMessage4.TradeType == 6 || tradeMessage4.TradeType == 7)
										{
											stringBuilder.AppendFormat("客户{5}\n\n{0}号委托成交\n{1} {2}{3}{4}手\n", new object[]
											{
												tradeMessage4.OrderNO,
												Global.AgencyCommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
												Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
												Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
												tradeMessage4.TradeQuatity,
												Global.AgencyNo
											});
										}
										else if (tradeMessage4.TradeType == 3)
										{
											stringBuilder.AppendFormat("客户{5}\n\n{0}成交\n{1} {2}{3}{4}手\n", new object[]
											{
												Global.GetEnumtoResourcesString("TRADEOPERATETYPE", (int)tradeMessage4.TradeType),
												Global.AgencyCommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
												Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
												Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
												tradeMessage4.TradeQuatity,
												Global.AgencyNo
											});
										}
										else
										{
											stringBuilder.AppendFormat("客户{5}\n\n{0}成交\n{1} {2}{3}{4}手\n", new object[]
											{
												Global.GetEnumtoResourcesString("TRADEDEALTYPE", (int)tradeMessage4.TradeType),
												Global.AgencyCommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
												Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
												Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
												tradeMessage4.TradeQuatity,
												Global.AgencyNo
											});
										}
									}
									else if (tradeMessage4.TradeType == 1 || tradeMessage4.TradeType == 2 || tradeMessage4.TradeType == 6 || tradeMessage4.TradeType == 7)
									{
										stringBuilder.AppendFormat("客户{4}\n\n{0}号委托成交{1}{2}{3}手\n", new object[]
										{
											tradeMessage4.OrderNO,
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity,
											Global.AgencyNo
										});
									}
									else if (tradeMessage4.TradeType == 3)
									{
										stringBuilder.AppendFormat("客户{4}\n\n{0}成交{1}{2}{3}手\n", new object[]
										{
											Global.GetEnumtoResourcesString("TRADEOPERATETYPE", (int)tradeMessage4.TradeType),
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity,
											Global.AgencyNo
										});
									}
									else
									{
										stringBuilder.AppendFormat("客户{4}\n\n{0}成交{1}{2}{3}手\n", new object[]
										{
											Global.GetEnumtoResourcesString("TRADEDEALTYPE", (int)tradeMessage4.TradeType),
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity,
											Global.AgencyNo
										});
									}
								}
								else if (tradeMessage4.CommodityID.Trim().Length > 0 && Global.CommodityData.ContainsKey(tradeMessage4.CommodityID))
								{
									if (tradeMessage4.TradeType == 1 || tradeMessage4.TradeType == 2 || tradeMessage4.TradeType == 6 || tradeMessage4.TradeType == 7)
									{
										stringBuilder.AppendFormat("{0}号委托成交\n{1} {2}{3}{4}手\n", new object[]
										{
											tradeMessage4.OrderNO,
											Global.CommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity
										});
									}
									else if (tradeMessage4.TradeType == 3)
									{
										stringBuilder.AppendFormat("{0}成交\n{1} {2}{3}{4}手\n", new object[]
										{
											Global.GetEnumtoResourcesString("TRADEOPERATETYPE", (int)tradeMessage4.TradeType),
											Global.CommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity
										});
									}
									else
									{
										stringBuilder.AppendFormat("{0}成交\n{1} {2}{3}{4}手\n", new object[]
										{
											Global.GetEnumtoResourcesString("TRADEDEALTYPE", (int)tradeMessage4.TradeType),
											Global.CommodityData[tradeMessage4.CommodityID.ToString().Trim()].CommodityName.ToString(),
											Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
											Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
											tradeMessage4.TradeQuatity
										});
									}
								}
								else if (tradeMessage4.TradeType == 1 || tradeMessage4.TradeType == 2 || tradeMessage4.TradeType == 6 || tradeMessage4.TradeType == 7)
								{
									stringBuilder.AppendFormat("{0}号委托成交{1}{2}{3}手\n", new object[]
									{
										tradeMessage4.OrderNO,
										Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
										Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
										tradeMessage4.TradeQuatity
									});
								}
								else if (tradeMessage4.TradeType == 3)
								{
									stringBuilder.AppendFormat("{0}成交{1}{2}{3}手\n", new object[]
									{
										Global.GetEnumtoResourcesString("TRADEOPERATETYPE", (int)tradeMessage4.TradeType),
										Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
										Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
										tradeMessage4.TradeQuatity
									});
								}
								else
								{
									stringBuilder.AppendFormat("{0}成交{1}{2}{3}手\n", new object[]
									{
										Global.GetEnumtoResourcesString("TRADEDEALTYPE", (int)tradeMessage4.TradeType),
										Global.GetEnumtoResourcesString("SETTLEBASIS", (int)tradeMessage4.SettleBasis),
										Global.GetEnumtoResourcesString("BUYSELL", (int)tradeMessage4.BuySell),
										tradeMessage4.TradeQuatity
									});
								}
								Logger.wirte(1, "refreshTime线程 13");
								TMainForm.StringObjCallback method2 = new TMainForm.StringObjCallback(this.displayTradeInfo);
								this.refreshHoldingDetailInfo = 0;
								base.BeginInvoke(method2, new object[]
								{
									stringBuilder.ToString()
								});
								Logger.wirte(1, "refreshTime线程 14");
							}
						}
						this.refreshFlag = true;
						TMainForm.UpdataEvent method3 = new TMainForm.UpdataEvent(this.UpDataTabCtrl);
						this.DelegateLoadHIF6();
						this.HandleCreated();
						base.BeginInvoke(method3, new object[0]);
						Logger.wirte(1, "refreshTime线程 15");
					}
					SystemStatus currentSystemStatus;
					lock (this._CurrentSystemStatusObject)
					{
						currentSystemStatus = this._CurrentSystemStatus;
					}
					if ((sysTime.SystemStatus == 7 || sysTime.SystemStatus == 2) && (short)currentSystemStatus != sysTime.SystemStatus)
					{
						lock (this._CurrentSystemStatusObject)
						{
							this._CurrentSystemStatus = (SystemStatus)Enum.Parse(typeof(SystemStatus), sysTime.SystemStatus.ToString());
							goto IL_DF6;
						}
					}
					if (sysTime.SystemStatus == 3 && (short)currentSystemStatus != sysTime.SystemStatus)
					{
						this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.UpDataHoldingDetailInfoHQ);
						this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.UpDataHoldingDetailInfoF5HQ);
						this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.FillHoldingInfoFloatingPrice);
						this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.UpdateCustomerOrderF10HQ);
						this.HQRefreashed -= new TMainForm.RefreshHQHanlder(this.UpdateMemberFundPrice);
						lock (this._CurrentSystemStatusObject)
						{
							this._CurrentSystemStatus = (SystemStatus)Enum.Parse(typeof(SystemStatus), sysTime.SystemStatus.ToString());
						}
						TMainForm.UpdataEvent method4 = new TMainForm.UpdataEvent(this.UpDataTabCtrl);
						this.HandleCreated();
						base.BeginInvoke(method4, new object[0]);
					}
					IL_DF6:
					if (sysTime.SystemStatus != 5)
					{
						TMainForm.UpdataCurrentSystemStatus method5 = new TMainForm.UpdataCurrentSystemStatus(this.UpdataSystemStatus);
						this.HandleCreated();
						if (sysTime.SystemStatus == 3)
						{
							base.BeginInvoke(method5, new object[]
							{
								string.Format("系统状态：{0}", Global.GetEnumtoResourcesString("SYSTEMSTATUS", 3))
							});
						}
						else
						{
							base.BeginInvoke(method5, new object[]
							{
								string.Format("系统状态：{0}", Global.GetEnumtoResourcesString("SYSTEMSTATUS", 7))
							});
						}
					}
					else
					{
						this.HandleCreated();
						TMainForm.UpdataCurrentSystemStatus method6 = new TMainForm.UpdataCurrentSystemStatus(this.UpdataSystemStatus);
						base.BeginInvoke(method6, new object[]
						{
							string.Empty
						});
					}
				}
				else
				{
					Logger.wirte(2, string.Format("主窗体获取服务器系统时间错误：{0} {1}", sysTime.RetCode, sysTime.RetMessage));
					if (sysTime != null && sysTime.RetCode == -201L)
					{
						Logger.wirte(2, "ConnectStatus.QZDisconnect");
						this.connectStatus = ConnectStatus.QZDisconnect;
					}
					else if (sysTime != null && sysTime.RetCode == -2001L)
					{
						Logger.wirte(2, "ConnectStatus.TimeOut");
						this.connectStatus = ConnectStatus.TimeOut;
					}
					else
					{
						Logger.wirte(2, "ConnectStatus.Disconnect");
						this.connectStatus = ConnectStatus.Disconnect;
						if (this.ChangeServerEvent != null)
						{
							this.ChangeServerEvent(null, null);
						}
					}
					Logger.wirte(1, "refreshTime线程 16");
					WaitCallback waitCallback2 = new WaitCallback(this.SetStatus);
					this.HandleCreated();
					Delegate arg_F7C_1 = waitCallback2;
					object[] args2 = new object[1];
					base.BeginInvoke(arg_F7C_1, args2);
					Logger.wirte(1, "refreshTime线程 17");
				}
				this.refreshTimeFlag = true;
			}
			catch (Exception ex2)
			{
				Logger.wirte(ex2);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.refreshTimeFlag = true;
				this.firstRefreshTime = true;
			}
			Logger.wirte(1, "refreshTime线程结束 ");
		}
		internal void DelegateRefresh()
		{
			if (IniData.GetInstance().AutoRefresh)
			{
				this.F2Flag = true;
				this.F3Flag = true;
				this.F4Flag = true;
				this.F5Flag = true;
				this.F6Flag = true;
				this.F7Flag = true;
				this.F8Flag = true;
				this.F10Flag = true;
			}
			this.refreshFlag = false;
		}
		private void status_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (this.connectStatus == ConnectStatus.QZDisconnect || this.connectStatus == ConnectStatus.TimeOut)
				{
					if (!this.blogout)
					{
						this.blogout = true;
						Configuration configuration = new Configuration();
						Hashtable section = configuration.getSection("Systems");
						string text = section["Title"].ToString();
						if (this.dataProcess.IsAgency)
						{
							DialogResult dialogResult;
							if (this.connectStatus == ConnectStatus.QZDisconnect)
							{
								Logger.wirte(2, "当前客户身份不合法，请注销该客户后重新登录！");
								dialogResult = MessageBoxEx.Show("当前客户身份不合法，请注销该客户后重新登录！", text ?? "", MessageBoxButtons.OK, new string[]
								{
									"退出(&C)"
								}, MessageBoxIcon.Asterisk);
							}
							else
							{
								Logger.wirte(2, "当前客户身份已过期，请注销该客户后重新登录！");
								dialogResult = MessageBoxEx.Show("当前客户身份已过期，请注销该客户后重新登录！", text ?? "", MessageBoxButtons.OK, new string[]
								{
									"退出(&C)"
								}, MessageBoxIcon.Asterisk);
							}
							if (dialogResult == DialogResult.OK)
							{
								this.AgencyLogOff();
							}
						}
						else
						{
							DialogResult dialogResult;
							if (this.connectStatus == ConnectStatus.QZDisconnect)
							{
								dialogResult = MessageBoxEx.Show("您的账号在另一地点登录或被管理员强制下线！", text ?? "", MessageBoxButtons.OKCancel, new string[]
								{
									"重新登录(&O)",
									"退出(&C)"
								}, MessageBoxIcon.Asterisk);
							}
							else
							{
								Logger.wirte(2, "您的身份已过期，请重新登录！");
								dialogResult = MessageBoxEx.Show("您的身份已过期，请重新登录！", text ?? "", MessageBoxButtons.OKCancel, new string[]
								{
									"重新登录(&O)",
									"退出(&C)"
								}, MessageBoxIcon.Asterisk);
							}
							if (dialogResult == DialogResult.OK)
							{
								LogonRequestVO logonRequestVO = new LogonRequestVO();
								logonRequestVO.UserID = Global.UserID;
								logonRequestVO.Password = Global.Password;
								logonRequestVO.RegisterWord = Global.RegisterWord;
								LogonResponseVO logonResponseVO = this.dataProcess.TradeLibrary.Logon(logonRequestVO);
								if (logonResponseVO.RetCode != 0L)
								{
									MessageBoxEx.Show(string.Format("重新登录错误:{0}[{1}]", logonResponseVO.RetMessage, logonResponseVO.RetCode), "错误", MessageBoxButtons.OK, new string[]
									{
										"确定(&O)"
									}, MessageBoxIcon.Hand);
									if (this.LogOutEvent != null)
									{
										this.LogOutEvent();
									}
								}
								else
								{
									WaitCallback waitCallback = new WaitCallback(this.LinkRetry);
									this.HandleCreated();
									Delegate arg_22E_1 = waitCallback;
									object[] args = new object[1];
									base.BeginInvoke(arg_22E_1, args);
									this.connectStatus = ConnectStatus.Connect;
									this.SetStatus(null);
									this.UpDataHoldingDetailInfoflag = false;
									this.UpDataHoldingDetailInfoF5flag = false;
									this.FirmInfoflag = false;
									this.FillHoldingInfoFloatingPriceflag = false;
									this.UpdateMemberFundPriceflag = false;
									this.FirmInfoF7flag = false;
									this.UpdateCustomerOrderF10flag = false;
									this.UpdateOrderInfoDataGridflag = false;
									this.WithdrawLossProfitflag = false;
									this.FillOrderInfoDataGridF3flag = false;
									this.WithdrawOrderXflag = false;
									this.QueryCommodityInfoF8flag = false;
									this.FillTradeInfoDataGridF4flag = false;
									this.YuJingMessageflag = false;
									this.refreshFlag = false;
									this.refreshGNFlag = true;
									this.refreshTimeFlag = true;
									this.timerHQ.Start();
									this.timerLock.Start();
									TMainForm.UpdataEvent method = new TMainForm.UpdataEvent(this.UpDataTabCtrl);
									this.HandleCreated();
									base.BeginInvoke(method, new object[0]);
								}
							}
							else if (this.LogOutEvent != null)
							{
								this.LogOutEvent();
							}
						}
					}
				}
				else
				{
					this.blogout = false;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				this.blogout = false;
			}
		}
		private void FillInfoText(string infoMessage, Color color, bool display)
		{
			try
			{
				if (display)
				{
					this.info.ForeColor = color;
					this.info.Text = "信息提示：" + infoMessage;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void LinkRetry(object o)
		{
			try
			{
				this.FillInfoText("重新登录成功！", Global.RightColor, this.displayInfo);
				if (!this.timerLock.Enabled)
				{
					this.timerLock.Enabled = true;
				}
				if (this.Connect)
				{
					this.connectStatus = ConnectStatus.Connect;
					this.SetStatus(null);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void displayMessage(object obj)
		{
			try
			{
				int floatAlertWindowStayTime = IniData.GetInstance().FloatAlertWindowStayTime;
				TaskbarNotifier taskbarNotifier = new TaskbarNotifier();
				Image image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoPic");
				Image image2 = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoClose");
				if (image != null && image2 != null)
				{
					Bitmap bitmap = new Bitmap(image);
					Bitmap bitmap2 = new Bitmap(image2);
					taskbarNotifier.SetBackgroundBitmap(bitmap, Color.FromArgb(0, 0, 255));
					taskbarNotifier.SetCloseBitmap(bitmap2, Color.FromArgb(0, 0, 255), new Point(180, 0));
					taskbarNotifier.TitleRectangle = new Rectangle(25, 0, 85, 22);
					taskbarNotifier.ContentRectangle = new Rectangle(30, 30, 150, 52);
					taskbarNotifier.AutoHide = true;
				}
				if (this.messageevent != null)
				{
					taskbarNotifier.SetContentClickEvent(this.messageevent);
				}
				taskbarNotifier.Show("系统公告", "您有新消息单击查看", 500, floatAlertWindowStayTime * 1000, 500);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void displayTradeInfo(object obj)
		{
			try
			{
				string text = (string)obj;
				if (!text.Equals(""))
				{
					PlayWav.PlayWavResource("ring.wav", 0);
					if (IniData.GetInstance().SuccessShowDialog)
					{
						int floatAlertWindowStayTime = IniData.GetInstance().FloatAlertWindowStayTime;
						TaskbarNotifier taskbarNotifier = new TaskbarNotifier();
						Image image = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoPic");
						Image image2 = (Image)Global.m_PMESResourceManager.GetObject("TradeImg_InfoClose");
						if (image != null && image2 != null)
						{
							Bitmap bitmap = new Bitmap(image);
							Bitmap bitmap2 = new Bitmap(image2);
							taskbarNotifier.SetBackgroundBitmap(bitmap, Color.FromArgb(0, 0, 255));
							taskbarNotifier.SetCloseBitmap(bitmap2, Color.FromArgb(0, 0, 255), new Point(180, 0));
							taskbarNotifier.TitleRectangle = new Rectangle(25, 0, 85, 22);
							taskbarNotifier.ContentRectangle = new Rectangle(30, 30, 150, 52);
							taskbarNotifier.AutoHide = true;
						}
						taskbarNotifier.Show("成交回报", text, 500, floatAlertWindowStayTime * 1000, 500);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void SetMessageEvent(EventHandler _messageEvent)
		{
			try
			{
				if (this.MessageNotifier != null && _messageEvent != null)
				{
					this.messageevent = _messageEvent;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void timerLock_Tick(object sender, EventArgs e)
		{
			try
			{
				this.IdleOnMoudel++;
				this.IdleRefreshButton++;
				this.IntervaltimerMember++;
				if (this.IntervaltimerMember >= this.timerMember && this.Member)
				{
					this.updataMember();
					this.IntervaltimerMember = 0;
				}
				if (IniData.GetInstance().LockEnable)
				{
					TimeSpan t = DateTime.Now.Subtract(this.IdleStartTime);
					if (t.Minutes >= IniData.GetInstance().LockTime && !this.dataProcess.IsAgency)
					{
						this.HandleCreated();
						TMainForm.LockTmain method = new TMainForm.LockTmain(this.LockSet);
						base.BeginInvoke(method, new object[]
						{
							false
						});
					}
					this.status_DoubleClick(this, null);
					t = new TimeSpan(0, 0, 1);
					Global.ServerTime += t;
					this.HandleCreated();
					TMainForm.UpdataTimerInfo method2 = new TMainForm.UpdataTimerInfo(this.UpdataTimetext);
					base.BeginInvoke(method2, new object[]
					{
						Global.ServerTime.ToString("yyyy-MM-dd HH:mm:ss")
					});
				}
				if (this.refreshFlag)
				{
					this.DelegateRefresh();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdataSystemStatus(string Text)
		{
			try
			{
				this.toolStripSystemStatus.Text = Text;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdataSystemEnvironment()
		{
			string text = string.Empty;
			Configuration configuration = new Configuration();
			Hashtable section = configuration.getSection("Systems");
			if (section != null)
			{
				string text2 = section["CurEnvironmentKey"].ToString();
				string[] array = section["Envrionment"].ToString().Split(new char[]
				{
					'|'
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text3 = array2[i];
					string value = text3.Split(new char[]
					{
						':'
					})[1];
					if (text2.Equals(value))
					{
						text = string.Format("{0}", text3.Split(new char[]
						{
							':'
						})[0]);
						break;
					}
				}
			}
			this.HandleCreated();
			TMainForm.UpdataCurrentSystemStatus method = new TMainForm.UpdataCurrentSystemStatus(this.InvokeUpdataSystemEnvironment);
			base.BeginInvoke(method, new object[]
			{
				text
			});
		}
		private void InvokeUpdataSystemEnvironment(string Text)
		{
			this.toolStripStatusEnvironment.Text = Text;
		}
		private void UpdataTimetext(string Text)
		{
			this.time.Text = Text;
		}
		private void StartHook()
		{
			try
			{
				this.hookKey = new LocalHook();
				this.hookKey.KeyDown += new KeyEventHandler(this.TMainForm_KeyUp);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void AddHook()
		{
			try
			{
				this.IdleStartTime = DateTime.Now;
				this.hook = new LocalHook();
				this.hook.OnMouseActivity += new MouseEventHandler(this.hook_OnMouseActivity);
				this.hook.KeyUp += new KeyEventHandler(this.hook_KeyUp);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void hook_OnMouseActivity(object sender, MouseEventArgs e)
		{
			this.IdleStartTime = DateTime.Now;
		}
		private void hook_KeyUp(object sender, KeyEventArgs e)
		{
			this.TMainForm_KeyUp(sender, e);
			this.IdleStartTime = DateTime.Now;
		}
		public void SetUnLock(bool _UnLock)
		{
			this.InvokeSetUnLock(_UnLock);
		}
		private void InvokeSetUnLock(bool _lock)
		{
			this.HandleCreated();
			TMainForm.LockTmain method = new TMainForm.LockTmain(this.LockSet);
			base.BeginInvoke(method, new object[]
			{
				!_lock
			});
		}
		private void StartLock()
		{
			try
			{
				this.timerLock.Enabled = true;
				this.timerLockRefresh = Convert.ToInt32(Tools.StrToDouble((string)Global.HTConfig["HqRefreshTime"], 1.0) * 1000.0);
				this.IdleStartTime = DateTime.Now;
				this.AddHook();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void LockSet(bool type)
		{
			try
			{
				if (this.firstLockSet != type)
				{
					this.tabTMain.Visible = type;
					this.statusInfo.Visible = type;
					this.panelLock.Visible = !type;
					if (this.SetToolsBarEnable != null)
					{
						this.SetToolsBarEnable(type);
					}
					this.statusInfo.Visible = type;
					this.contextMenuStripHoldingDetail.Visible = type;
					this.contextMenuStripF7.Visible = type;
					this.contextMenuStripHQ.Visible = type;
					this.lockset = type;
					this.contextMenuStripXJ.Visible = type;
					this.contextMenuStripF9YJ.Visible = type;
					this.contextMenuStripFirmInfo.Visible = type;
					this.contextMenuStripHoldingDetail.Hide();
					this.contextMenuStripHQ.Hide();
					this.contextMenuStripXJ.Hide();
					this.contextMenuStripF9YJ.Hide();
					this.contextMenuStripFirmInfo.Hide();
					this.contextMenuStripF7.Hide();
					this.CloseMyForm();
					if (this.LockTreeEvent != null)
					{
						this.LockTreeEvent(type);
					}
					if (!type)
					{
						try
						{
							this.m_pCalc.Kill();
						}
						catch (Exception)
						{
							this.m_pCalc = null;
						}
					}
					foreach (KeyValuePair<string, TradeCtrl> current in this.m_MyTradeCtrl)
					{
						current.Value.SetShowMenu(type);
					}
					if (!type)
					{
						this.BackColor = Color.Black;
					}
					else
					{
						this.BackColor = SystemColors.Control;
						this.IdleStartTime = DateTime.Now;
						this.timerLockRefresh = Tools.StrToInt((string)Global.HTConfig["HqRefreshTime"], 3) * 1000;
					}
				}
				this.firstLockSet = type;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainForm_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.F2)
				{
					this.tabTMain.SelectedIndex = 0;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 0;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F3)
				{
					this.tabTMain.SelectedIndex = 1;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 1;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F4)
				{
					this.tabTMain.SelectedIndex = 2;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 2;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F5)
				{
					this.tabTMain.SelectedIndex = 3;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 3;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F6)
				{
					this.tabTMain.SelectedIndex = 4;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 4;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F7)
				{
					this.tabTMain.SelectedIndex = 5;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 5;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F8)
				{
					this.tabTMain.SelectedIndex = 6;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 6;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F9)
				{
					this.tabTMain.SelectedIndex = 7;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 7;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.F10)
				{
					this.tabTMain.SelectedIndex = 8;
					lock (this.iSelecttablock)
					{
						this.iSelecttab = 8;
						goto IL_20B;
					}
				}
				if (e.KeyCode == Keys.Return && this.panelLock.Visible)
				{
					this.buttonUnLock_Click_1(this, null);
				}
				IL_20B:
				this.displayInfo = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void KeyF10()
		{
			try
			{
				this.tabTMain.SelectedIndex = 8;
				lock (this.iSelecttablock)
				{
					this.iSelecttab = 8;
				}
				this.displayInfo = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void HQ_DataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
		}
		private void HQ_DataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
		}
		private void UpDataTabCtrl()
		{
			try
			{
				int selectedIndex = this.tabTMain.SelectedIndex;
				if (this.Connect)
				{
					switch (selectedIndex)
					{
					case 0:
						if (this.F2Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadHDI(true);
							if (this.dataProcess.sIdentity == Identity.Client)
							{
								this.DelegateLoadOI(true);
								this.DelegateLoadFI(true);
								this.F2Flag = false;
							}
							else if (this.dataProcess.sIdentity == Identity.Member)
							{
								this.DelegateLoadCOF10();
								this.F2Flag = false;
							}
						}
						break;
					case 1:
						if (this.F3Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadOIF3();
							this.F3Flag = false;
						}
						break;
					case 2:
						if (this.F4Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadOIF4();
							this.F4Flag = false;
						}
						break;
					case 3:
						if (this.F5Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadHDIF5();
							this.F5Flag = false;
						}
						break;
					case 4:
						if (this.F6Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadHIF6();
							this.F6Flag = false;
						}
						break;
					case 5:
						if (this.F7Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadFIF7();
							this.F7Flag = false;
						}
						break;
					case 6:
						if (this.F8Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadCIF8();
							this.F8Flag = false;
						}
						break;
					case 7:
						this.DelegateLoadCIF9();
						break;
					case 8:
						if (this.F10Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.DelegateLoadCOF10_2();
							this.F10Flag = false;
						}
						break;
					}
					this.IdleOnMoudel = 0;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void tabTMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				lock (this.iSelecttablock)
				{
					this.iSelecttab = this.tabTMain.SelectedIndex;
				}
				this.UpDataTabCtrl();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void HQ_DataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.HQ_DataGrid.ClearSelection();
					this.HQ_DataGrid.Rows[e.RowIndex].Selected = true;
					string text = "-1";
					if (e.ColumnIndex == 2)
					{
						text = BuySell.Sell.ToString("d");
					}
					else if (e.ColumnIndex == 3)
					{
						text = BuySell.Buy.ToString("d");
					}
					this._HQGridContextMenuRowIndex = e.RowIndex;
					this._HQGridContextMenuColumnIndex = e.ColumnIndex;
					if (this.SetMenuDisenable("contextMenuStripHQ"))
					{
						this.toolStripMenuItemSO.Enabled = !text.Equals("-1");
						this.toolStripMenuItemSC.Enabled = !text.Equals("-1");
						this.toolStripMenuItemXO.Enabled = !text.Equals("-1");
					}
					Point position = this.HQ_DataGrid.PointToClient(Cursor.Position);
					if (this.lockset)
					{
						this.contextMenuStripHQ.Show(this.HQ_DataGrid, position);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemSO_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow = this.HQ_DataGrid.Rows[this._HQGridContextMenuRowIndex];
				bool buySell = true;
				if (this._HQGridContextMenuColumnIndex == 2)
				{
					buySell = true;
				}
				else if (this._HQGridContextMenuColumnIndex == 3)
				{
					buySell = false;
				}
				string commodityID = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				this.TradeCtrlEvent(TradeMenu.em_OpenPrice_S, commodityID, buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemSC_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow = this.HQ_DataGrid.Rows[this._HQGridContextMenuRowIndex];
				bool buySell = true;
				if (this._HQGridContextMenuColumnIndex == 2)
				{
					buySell = true;
				}
				else if (this._HQGridContextMenuColumnIndex == 3)
				{
					buySell = false;
				}
				string commodityID = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				this.TradeCtrlEvent(TradeMenu.em_ClosePrice_S, commodityID, buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemXO_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow = this.HQ_DataGrid.Rows[this._HQGridContextMenuRowIndex];
				bool buySell = true;
				if (this._HQGridContextMenuColumnIndex == 2)
				{
					buySell = true;
				}
				else if (this._HQGridContextMenuColumnIndex == 3)
				{
					buySell = false;
				}
				string commodityID = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				this.TradeCtrlEvent(TradeMenu.em_OpenPrice_L, commodityID, buySell);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemHQCancel_Click(object sender, EventArgs e)
		{
			this.contextMenuStripHQ.Close();
		}
		private void dgvOrderInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.dgvOrderInfo.ClearSelection();
					this.dgvOrderInfo.Rows[e.RowIndex].Selected = true;
					this._XJGridContextMenuRowIndex = e.RowIndex;
					this._XJGridMenuEnabled = e.RowIndex;
					Point position = this.dgvOrderInfo.PointToClient(Cursor.Position);
					this.SetMenuDisenable("contextMenuStripXJ");
					this.contextMenuStripXJ.Show(this.dgvOrderInfo, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.CloseMainForm();
		}
		public void CloseMyForm()
		{
			if (this.Ordersform != null)
			{
				this.Ordersform.Close();
			}
			if (this.pWarehouseForm != null)
			{
				this.pWarehouseForm.Close();
			}
			if (this.userSet != null)
			{
				this.userSet.Close();
			}
			if (this.FromYJSZ != null)
			{
				this.FromYJSZ.Close();
			}
			if (this.yujingmessage != null)
			{
				this.yujingmessage.Close();
			}
			this.m_fAbout.Close();
		}
		private void DisposeMyForm()
		{
			if (this.Ordersform != null)
			{
				this.Ordersform.Close();
				this.Ordersform.Dispose();
				this.Ordersform = null;
			}
			if (this.pWarehouseForm != null)
			{
				this.pWarehouseForm.Close();
				this.pWarehouseForm.Dispose();
				this.pWarehouseForm = null;
			}
			if (this.userSet != null)
			{
				this.userSet.Close();
				this.userSet.Dispose();
				this.userSet = null;
			}
			if (this.FromYJSZ != null)
			{
				this.FromYJSZ.Close();
				this.FromYJSZ.Dispose();
				this.FromYJSZ = null;
			}
			if (this.m_fAbout != null)
			{
				this.m_fAbout.Close();
				this.m_fAbout.Dispose();
				this.m_fAbout = null;
			}
			if (this.yujingmessage != null)
			{
				this.yujingmessage.Close();
				this.yujingmessage.Dispose();
				this.yujingmessage = null;
			}
		}
		public void CloseMainForm()
		{
			try
			{
				this.bCloseForm = true;
				this.timerLock.Stop();
				this.timerLock.Dispose();
				this.timerLock = null;
				this.timerHQ.Stop();
				this.timerHQ.Dispose();
				Logger.wirte(2, "定时器停止， 窗体关闭");
				this.timerHQ = null;
				this.helpProvider.Dispose();
				this.helpProvider = null;
				this.DictionarySemaphore.Clear();
				Logger.wirte(1, "DictionarySemaphore.Clear();结束");
				this.DisposeMyForm();
				Logger.wirte(1, "DisposeMyForm;结束");
				if (this.hook != null)
				{
					this.hook.Stop();
				}
				Logger.wirte(1, "hook.Stop();结束");
				base.Dispose();
				Logger.wirte(1, "this.Dispose();结束");
				IniData.DisposeInstance();
				Logger.wirte(1, "radeClientApp.Library.IniData.DisposeInstance();结束");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			try
			{
				this.m_pCalc.Kill();
			}
			catch (Exception)
			{
				this.m_pCalc = null;
			}
		}
		private void cbCommodityInfo_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			try
			{
				if (this.F8Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
				{
					if (this.iSelecttab == 6)
					{
						this.UpDataTabCtrl();
					}
				}
				else
				{
					MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void NEWToolStripMenuItemYJF9_Click_1(object sender, EventArgs e)
		{
			try
			{
				this.FromYJSZ = new TMainYJSZ(this);
				this.FromYJSZ.ShowDialog();
				this.DelegateLoadCIF9();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainForm_Resize(object sender, EventArgs e)
		{
			int num = this.panelLock.Width >> 1;
			int num2 = this.panelLock.Height >> 1;
			int num3 = this.splitContainer1.Panel2.Width >> 1;
			int num4 = this.splitContainer1.Panel2.Height >> 1;
			int x = num3 - num;
			int y = num4 - num2;
			this.panelLock.SetBounds(x, y, this.panelLock.Width, this.panelLock.Height);
			Size clientSize = base.ClientSize;
			int num5 = clientSize.Width >> 1;
			int num6 = clientSize.Height >> 1;
			int num7 = this.MessageInfo.Width >> 1;
			int num8 = this.MessageInfo.Height >> 1;
			x = num5 - num7 + base.Bounds.Left;
			y = num6 - num8 + base.Bounds.Top;
			this.MessageInfo.SetBounds(x, y, this.MessageInfo.Width, this.MessageInfo.Height);
		}
		private void statusInfo_Resize(object sender, EventArgs e)
		{
			this.info.Width = this.statusInfo.Width - this.user.Width - this.status.Width - this.time.Width - this.toolStripSystemStatus.Width - 22 - this.toolStripStatusEnvironment.Width;
		}
		private void MODToolStripMenuItemYJF9_Click(object sender, EventArgs e)
		{
			this.UPDateYJ();
		}
		private void DELToolStripMenuItemYJF9_Click(object sender, EventArgs e)
		{
			this.DelDataYJ();
		}
		private void ONOFFToolStripMenuItemYJF9_Click(object sender, EventArgs e)
		{
			this.OnOffYJ();
		}
		private void buttonUnLock_Click_1(object sender, EventArgs e)
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
		private bool identitystatus()
		{
			bool result;
			try
			{
				if (this.panelLock.Visible)
				{
					result = false;
				}
				else
				{
					if (this._IdentityStatus == null)
					{
						this._IdentityStatus = new IdentityStatus(this.dataProcess.IsAgency);
					}
					string message = "";
					if (!this._IdentityStatus.IdentityStatusToEnableMenu(this.connectStatus, out message))
					{
						MessageForm messageForm = new MessageForm("错误", message, 1, StatusBarType.Error);
						messageForm.Owner = this;
						messageForm.ShowDialog();
						messageForm.Dispose();
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = false;
			}
			return result;
		}
		public void TradeCtrlMenuEnabled()
		{
		}
		private bool SetMenuDisenable(string menuname)
		{
			bool flag = false;
			try
			{
				if (this._IdentityStatus == null)
				{
					this._IdentityStatus = new IdentityStatus(this.dataProcess.IsAgency);
				}
				this.MemberMenuControl(menuname);
				string text = "";
				flag = this._IdentityStatus.IdentityStatusToEnableMenu(this.connectStatus, out text);
				if (this.panelLock.Visible)
				{
					flag = false;
				}
				switch (menuname)
				{
				case "contextMenuStripHoldingDetail":
					this.toolStripMenuItemSP.Enabled = flag;
					this.toolStripMenuItemXP.Enabled = flag;
					this.toolStripMenuItemStopLoss.Enabled = flag;
					this.toolStripMenuItemStopProfit.Enabled = flag;
					this.toolStripMenuItemRefresh.Enabled = flag;
					break;
				case "contextMenuStripXJ":
					this.toolStripMenuItemWithdrawOrder.Enabled = flag;
					this.toolStripMenuItemXJRefresh.Enabled = flag;
					break;
				case "contextMenuStripHQ":
					this.toolStripMenuItemSO.Enabled = flag;
					this.toolStripMenuItemSC.Enabled = flag;
					this.toolStripMenuItemXO.Enabled = flag;
					break;
				case "contextMenuStripF6":
					this.toolStripMenuItemF6SP.Enabled = flag;
					this.toolStripMenuItemF6Refresh.Enabled = flag;
					break;
				case "contextMenuStripFirmInfo":
					this.toolStripMenuItemFirmInfoRefresh.Enabled = flag;
					break;
				case "TradeCtrl":
					foreach (KeyValuePair<string, TradeCtrl> current in this.m_MyTradeCtrl)
					{
						current.Value.contextMenuStripHQCtrlEnable(flag);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return flag;
		}
		private void MemberMenuControl(string MenuName)
		{
			try
			{
				if (this.dataProcess.sIdentity == Identity.Member && MenuName != null)
				{
					if (!(MenuName == "contextMenuStripHoldingDetail"))
					{
						if (!(MenuName == "contextMenuStripHQ"))
						{
							if (!(MenuName == "contextMenuStripXJ"))
							{
								if (MenuName == "TradeCtrl")
								{
									foreach (KeyValuePair<string, TradeCtrl> current in this.m_MyTradeCtrl)
									{
										current.Value.contextMenuStripHQCtrlEnable();
									}
								}
							}
							else
							{
								this.toolStripMenuItemWithdrawOrder.Visible = false;
								this.toolStripSeparator7.Visible = false;
							}
						}
						else
						{
							this.toolStripMenuItemXO.Visible = false;
							this.toolStripSeparator5.Visible = false;
						}
					}
					else
					{
						this.toolStripMenuItemXP.Visible = false;
						this.toolStripMenuItemStopLoss.Visible = false;
						this.toolStripMenuItemStopProfit.Visible = false;
						this.toolStripSeparator2.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.splitContainerAll.Panel2Collapsed = true;
				this.splitContainerHQ.Panel2Collapsed = false;
				foreach (KeyValuePair<string, TradeCtrl> current in this.m_MyTradeCtrl)
				{
					current.Value.Parent = this.splitContainerHQ.Panel2;
				}
				this.MoveTradeCtrl(this.splitContainerHQ.Panel2);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.splitContainerAll.Panel2Collapsed = false;
				this.splitContainerHQ.Panel2Collapsed = true;
				int num = 0;
				foreach (KeyValuePair<string, TradeCtrl> current in this.m_MyTradeCtrl)
				{
					current.Value.Parent = this.splitContainerAll.Panel2;
					num = current.Value.Width;
				}
				if (num == 0)
				{
					num = 170;
				}
				this.splitContainerAll.SplitterDistance = base.Width - num - 60;
				this.MoveTradeCtrl(this.splitContainerAll.Panel2);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private bool SubmitConfirm(string message)
		{
			bool flag = false;
			try
			{
				if (!IniData.GetInstance().ShowDialog)
				{
					flag = true;
					bool result = flag;
					return result;
				}
				MessageForm messageForm = new MessageForm("订单信息", message, 0, StatusBarType.Message);
				messageForm.Owner = this;
				messageForm.ShowDialog();
				messageForm.Dispose();
				if (!messageForm.isOK)
				{
					bool result = flag;
					return result;
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return flag;
		}
		private void updataMember()
		{
			try
			{
				int num = 0;
				lock (this.iSelecttablock)
				{
					num = this.iSelecttab;
				}
				if (num == 0)
				{
					this.DelegateLoadCOF10();
					this.DelegateLoadCOF10_2();
					this.F10Flag = false;
				}
				else if (num == 5)
				{
					this.DelegateLoadCOF10();
					this.DelegateLoadCOF10_2();
					this.F10Flag = false;
					this.DelegateLoadFIF7();
					this.F7Flag = false;
				}
				else if (num == 8)
				{
					this.DelegateLoadCOF10_2();
					this.F10Flag = false;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainForm_Shown(object sender, EventArgs e)
		{
			this.isFormShown = true;
		}
		private void TMainForm_VisibleChanged(object sender, EventArgs e)
		{
			if (this.isFormShown && base.Visible && !this.bCloseForm)
			{
				this.UpDataTabCtrl();
			}
		}
		private void HQ_DataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if ((e.ColumnIndex == 2 || e.ColumnIndex == 3) && e.Button == MouseButtons.Left && e.RowIndex >= 0)
				{
					if (this.identitystatus())
					{
						this._HQGridContextMenuRowIndex = e.RowIndex;
						this._HQGridContextMenuColumnIndex = e.ColumnIndex;
						this.toolStripMenuItemSO_Click(sender, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingDetailInfo_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
		}
		private void dgvHoldingDetailInfo_Sorted(object sender, EventArgs e)
		{
		}
		private void HQ_DataGrid_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.HQ_DataGrid.ClearSelection();
			}
		}
		private void AgencyLogOff()
		{
			this.CloseMainForm();
			if (this.AgencyLogOut != null)
			{
				this.AgencyLogOut();
			}
		}
		private void tblogOut_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("您确定要注销当前客户吗？", "电话下单注销", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
			{
				this.AgencyLogOff();
			}
		}
		private void MoveTradeCtrl(Panel panel)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			panel.VerticalScroll.Value = 0;
			foreach (Control control in panel.Controls)
			{
				Rectangle rectangle = new Rectangle(this.m_iTradeCtrl * 2 * (num + 1) + TradeCtrl.m_iWidth * num, this.m_iTradeCtrl * 2 * (num2 + 1) + num2 * TradeCtrl.m_iHeight, TradeCtrl.m_iWidth, TradeCtrl.m_iHeight);
				if (panel.Bounds.Width >= rectangle.Right)
				{
					num++;
					control.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
				}
				else
				{
					if (num3 != 0 && num != 0)
					{
						num2++;
					}
					num = 0;
					control.SetBounds(this.m_iTradeCtrl * 2 * (num + 1) + TradeCtrl.m_iWidth * num, this.m_iTradeCtrl * 2 * (num2 + 1) + num2 * TradeCtrl.m_iHeight, rectangle.Width, rectangle.Height);
					num++;
				}
				num3++;
			}
		}
		private void splitContainerHQ_Panel2_Resize(object sender, EventArgs e)
		{
			this.MoveTradeCtrl(this.splitContainerHQ.Panel2);
		}
		private void splitContainerAll_Panel2_Resize(object sender, EventArgs e)
		{
			this.MoveTradeCtrl(this.splitContainerAll.Panel2);
		}
		private void TMainForm_Paint(object sender, PaintEventArgs e)
		{
		}
		private void panel2_SizeChanged(object sender, EventArgs e)
		{
		}
		private void ReLayoutControl()
		{
			this.ReLayoutF3();
			this.ReLayoutF4();
			this.ReLayoutF8();
			this.ChangeControlFont();
		}
		private void ChangeControlFont()
		{
		}
		private void ReLayoutF3()
		{
			int num = 4;
			int num2 = 8;
			int num3 = 2;
			this.cbCommodity.Location = new Point(this.lbBuySell.Location.X + this.lbBuySell.Width + num, this.cbCommodity.Location.Y);
			this.cbBuySell.Location = new Point(this.lbBuySell.Location.X + this.lbBuySell.Width + num, this.cbBuySell.Location.Y);
			this.lbOrderInfoType.Location = new Point(this.cbCommodity.Location.X + this.cbCommodity.Width + num2, this.lbOrderInfoType.Location.Y);
			this.lbSettleBasis.Location = new Point(this.cbCommodity.Location.X + this.cbCommodity.Width + num2, this.lbSettleBasis.Location.Y);
			this.cbOrderInfoType.Location = new Point(this.lbOrderInfoType.Location.X + this.lbOrderInfoType.Width + num, this.cbOrderInfoType.Location.Y);
			this.cbSettleBasis.Location = new Point(this.lbOrderInfoType.Location.X + this.lbOrderInfoType.Width + num, this.cbSettleBasis.Location.Y);
			this.lbStatic.Location = new Point(this.cbOrderInfoType.Location.X + this.cbOrderInfoType.Width + num2, this.lbStatic.Location.Y);
			this.cbStatic.Location = new Point(this.lbStatic.Location.X + this.lbStatic.Width + num, this.cbStatic.Location.Y);
			this.Selectbt.Location = new Point(this.cbStatic.Location.X + this.cbStatic.Width + num2, this.Selectbt.Location.Y);
			this.btReset.Location = new Point(this.Selectbt.Location.X + this.Selectbt.Width + num2, this.btReset.Location.Y);
			this.panel1.Height = this.cbCommodity.Height * 2 + num3 * 3;
			this.dgvOrderInfoF3.Location = new Point(this.dgvOrderInfoF3.Location.X, this.panel1.Location.Y + this.panel1.Height + num3);
			this.dgvOrderInfoF3.Height = this.gbOrderInfoF3.Location.Y + this.gbOrderInfoF3.Height - this.dgvOrderInfoF3.Location.Y - this.gbOrderInfoF3.Padding.Bottom - this.dgvOrderInfoF3.Margin.Bottom;
		}
		private void ReLayoutF4()
		{
			int num = 4;
			int num2 = 8;
			int num3 = 2;
			this.cbCommodityF4.Location = new Point(this.lbCommodityF4.Location.X + this.lbCommodityF4.Width + num, this.cbCommodityF4.Location.Y);
			this.lbSettleBasisF4F4.Location = new Point(this.cbCommodityF4.Location.X + this.cbCommodityF4.Width + num2, this.lbSettleBasisF4F4.Location.Y);
			this.cbSettleBasisF4.Location = new Point(this.lbSettleBasisF4F4.Location.X + this.lbSettleBasisF4F4.Width + num, this.cbSettleBasisF4.Location.Y);
			this.lbBuySellF4.Location = new Point(this.cbSettleBasisF4.Location.X + this.cbSettleBasisF4.Width + num2, this.lbBuySellF4.Location.Y);
			this.cbBuySellF4.Location = new Point(this.lbBuySellF4.Location.X + this.lbBuySellF4.Width + num, this.cbBuySellF4.Location.Y);
			this.btSelectF4.Location = new Point(this.cbBuySellF4.Location.X + this.cbBuySellF4.Width + num2, this.btSelectF4.Location.Y);
			this.butReset.Location = new Point(this.btSelectF4.Location.X + this.btSelectF4.Width + num2, this.butReset.Location.Y);
			this.panel2.Height = this.cbCommodityF4.Height + num3 * 2;
			this.dvgTradeInfo.Location = new Point(this.dvgTradeInfo.Location.X, this.panel2.Location.Y + this.panel2.Height + num3);
			this.dvgTradeInfo.Height = this.gbTradeInfo.Location.Y + this.gbTradeInfo.Height - this.dvgTradeInfo.Location.Y - this.gbTradeInfo.Padding.Bottom - this.dvgTradeInfo.Margin.Bottom;
		}
		private void ReLayoutF8()
		{
			int num = 4;
			int num2 = 2;
			this.cbCommodityInfo.Location = new Point(this.LableCommodity.Location.X + this.LableCommodity.Width + num, this.cbCommodityInfo.Location.Y);
			this.dgvCommodityInfo.Location = new Point(this.dgvCommodityInfo.Location.X, this.cbCommodityInfo.Location.Y + this.cbCommodityInfo.Height + num2);
			this.dgvCommodityInfo.Height = this.gbCommodityInfo.Location.Y + this.gbCommodityInfo.Height - this.dgvCommodityInfo.Location.Y - this.gbCommodityInfo.Padding.Bottom - this.gbCommodityInfo.Margin.Bottom;
		}
		private void gbOrderInfoF3_SizeChanged(object sender, EventArgs e)
		{
			this.ReLayoutF3();
		}
		private void gbTradeInfo_SizeChanged(object sender, EventArgs e)
		{
			this.ReLayoutF4();
		}
		private void gbCommodityInfo_SizeChanged(object sender, EventArgs e)
		{
			this.ReLayoutF8();
		}
		private void SetTIF4DataGridColText()
		{
			try
			{
				this.dvgTradeInfo.Columns["StradeNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_STRADENO"));
				this.dvgTradeInfo.Columns["StradeNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["StradeNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dvgTradeInfo.Columns["CommodityName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["BuySell"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_BUYSELL"));
				this.dvgTradeInfo.Columns["BuySell"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["BuySell"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["Quantity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_QUANTITY"));
				this.dvgTradeInfo.Columns["Quantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["OpenpRice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENPRICE"));
				this.dvgTradeInfo.Columns["OpenpRice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["HoldPrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_HOLDPRICE"));
				this.dvgTradeInfo.Columns["HoldPrice"].DefaultCellStyle.Format = "f2";
				this.dvgTradeInfo.Columns["HoldPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_CLOSEPRICE"));
				this.dvgTradeInfo.Columns["ClosePrice"].DefaultCellStyle.Format = "f2";
				this.dvgTradeInfo.Columns["ClosePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["TransferPL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRANSFERPL"));
				this.dvgTradeInfo.Columns["TransferPL"].DefaultCellStyle.Format = "n2";
				this.dvgTradeInfo.Columns["TransferPL"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["Comm"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_COMM"));
				this.dvgTradeInfo.Columns["Comm"].DefaultCellStyle.Format = "f2";
				this.dvgTradeInfo.Columns["Comm"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_ORDERNO"));
				this.dvgTradeInfo.Columns["OrderNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["OrderNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["HoldingNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_HOLDINGNO"));
				this.dvgTradeInfo.Columns["HoldingNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["HoldingNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["SettleBasis"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_SETTLEBASIS"));
				this.dvgTradeInfo.Columns["SettleBasis"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["SettleBasis"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["OtherID"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "HDI_OTHERID"));
				this.dvgTradeInfo.Columns["OtherID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["OtherID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["TradeType"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRADEDEALTYPE"));
				this.dvgTradeInfo.Columns["TradeType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["TradeType"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["TradeOperateType"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRADEOPERATETYPE"));
				this.dvgTradeInfo.Columns["TradeOperateType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dvgTradeInfo.Columns["TradeOperateType"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["OrderTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_ORDERTIME"));
				this.dvgTradeInfo.Columns["OrderTime"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["TradeTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRADETIME"));
				this.dvgTradeInfo.Columns["TradeTime"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dvgTradeInfo.Columns["BuySellVal"].HeaderText = "BuySellval";
				this.dvgTradeInfo.Columns["BuySellVal"].Visible = false;
				this.dvgTradeInfo.Columns["SettleBasisVal"].HeaderText = "SettleBasisVal";
				this.dvgTradeInfo.Columns["SettleBasisVal"].Visible = false;
				this.dvgTradeInfo.Columns["CommodityID"].HeaderText = "CommodityID";
				this.dvgTradeInfo.Columns["CommodityID"].Visible = false;
				this.dvgTradeInfo.Columns["TradeTypeVal"].HeaderText = "TradeTypeVal";
				this.dvgTradeInfo.Columns["TradeTypeVal"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadOIF4()
		{
			try
			{
				TradeQueryRequestVO tradeQueryRequestVO = new TradeQueryRequestVO();
				tradeQueryRequestVO.UserID = Global.UserID;
				this.callFillTradeInfoF4DataGrid = new TMainForm.CallbackTradeInfoF4DataGrid(this.FillTradeInfoDataGridF4);
				this.EnableControls(false, "数据查询中");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadOIF4"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOIF4"];
					threadPoolParameter.obj = tradeQueryRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadOIF4", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOIF4"];
					threadPoolParameter.obj = tradeQueryRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.QueryTradeInfoF4);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryTradeInfoF4(object _TradeQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.FillTradeInfoDataGridF4flag)
				{
					this.FillTradeInfoDataGridF4flag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_TradeQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					TradeQueryRequestVO tradeQueryRequestVO = (TradeQueryRequestVO)threadPoolParameter.obj;
					DataSet dataSet = this.dataProcess.QueryTradeInfo(tradeQueryRequestVO);
					this.HandleCreated();
					base.BeginInvoke(this.callFillTradeInfoF4DataGrid, new object[]
					{
						dataSet
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.FillTradeInfoDataGridF4flag = false;
			}
		}
		private void InitComboxLableF4()
		{
			try
			{
				this.BindCommodityF4();
				this.BindSettleBasisF4();
				this.BindBuySellF4();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillTradeInfoDataGridF4(DataSet TradeInfoDataView)
		{
			try
			{
				Logger.wirte(1, "FillTradeInfoDataGridF4线程启动1");
				string text = " 1=1 ";
				if (this.cbCommodityF4.SelectedIndex > 0)
				{
					CBListItem cBListItem = (CBListItem)this.cbCommodityF4.Items[this.cbCommodityF4.SelectedIndex];
					text = text + " and CommodityID = '" + cBListItem.Key + "'";
				}
				if (this.cbSettleBasisF4.SelectedIndex >= 0)
				{
					CBListItem cBListItem2 = (CBListItem)this.cbSettleBasisF4.Items[this.cbSettleBasisF4.SelectedIndex];
					if (cBListItem2.Key != "-2")
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							" and SettleBasisVal = '",
							Convert.ToInt32(cBListItem2.Key),
							"'"
						});
					}
				}
				if (this.cbBuySellF4.SelectedIndex > 0)
				{
					CBListItem cBListItem3 = (CBListItem)this.cbBuySellF4.Items[this.cbBuySellF4.SelectedIndex];
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						" and BuySellVal ='",
						Convert.ToInt32(cBListItem3.Key),
						"' "
					});
				}
				if (TradeInfoDataView != null)
				{
					DataTable dataTable = this.DataTableSort(TradeInfoDataView.Tables["Trade"], text, "TradeTime", "Desc");
					this.DataViewAddQueryF4Sum(dataTable.DefaultView);
					this.dvgTradeInfo.DataSource = dataTable.DefaultView;
				}
				Logger.wirte(1, "FillTradeInfoDataGridF4线程2");
				this.gbTradeInfo.Text = Global.m_PMESResourceManager.GetString("PMESStr_TI_GB_TRADEINFO");
				this.SetTIF4DataGridColText();
				Logger.wirte(1, "FillTradeInfoDataGridF4线程3");
				this.SetDataGridColumnBySettleBasis();
				Logger.wirte(1, "FillTradeInfoDataGridF4线程4");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, string.Empty);
			this.btSelectF4.Enabled = true;
			Logger.wirte(1, "FillTradeInfoDataGridF4线程完成");
		}
		private void DataViewAddQueryF4Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				double num2 = 0.0;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["Quantity"];
					num2 += (double)dataView[j].Row["TransferPL"];
					num3 += (double)dataView[j].Row["Comm"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["BuySell"] = string.Format(text, dataView.Count - 1);
				dataRowView["Quantity"] = num;
				dataRowView["TransferPL"] = num2;
				dataRowView["Comm"] = num3;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
		}
		private void BindCommodityF4()
		{
			try
			{
				this.cbCommodityF4.Items.Clear();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						return;
					}
				}
				else if (Global.CommodityData == null)
				{
					return;
				}
				CBListItem item;
				if (this.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							string commodityName = current.Value.CommodityName;
							string commodityID = current.Value.CommodityID;
							item = new CBListItem(commodityID, commodityName);
							this.cbCommodityF4.Items.Add(item);
						}
						goto IL_109;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					string commodityName = current2.Value.CommodityName;
					string commodityID = current2.Value.CommodityID;
					item = new CBListItem(commodityID, commodityName);
					this.cbCommodityF4.Items.Add(item);
				}
				IL_109:
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbCommodityF4.Items.Insert(0, item);
				this.cbCommodityF4.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindSettleBasisF4()
		{
			try
			{
				this.cbSettleBasisF4.Items.Clear();
				CBListItem item;
				foreach (int num in Enum.GetValues(typeof(SettleBasis)))
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("SETTLEBASIS", num);
					string pKey = string.Format("{0}", num);
					item = new CBListItem(pKey, enumtoResourcesString);
					this.cbSettleBasisF4.Items.Add(item);
				}
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbSettleBasisF4.Items.Insert(0, item);
				this.cbSettleBasisF4.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindBuySellF4()
		{
			try
			{
				this.cbBuySellF4.Items.Clear();
				CBListItem item;
				foreach (int num in Enum.GetValues(typeof(BuySell)))
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("BUYSELL", num);
					string pKey = string.Format("{0}", num);
					item = new CBListItem(pKey, enumtoResourcesString);
					this.cbBuySellF4.Items.Add(item);
				}
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbBuySellF4.Items.Insert(0, item);
				this.cbBuySellF4.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetDataGridColumnBySettleBasis()
		{
			try
			{
				string key = ((CBListItem)this.cbSettleBasisF4.SelectedItem).Key;
				if (key == SettleBasis.Open.ToString("d"))
				{
					this.dvgTradeInfo.Columns["OrderTime"].Visible = false;
					this.dvgTradeInfo.Columns["TradeTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENTIME"));
					this.dvgTradeInfo.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_ORDERNO"));
					this.dvgTradeInfo.Columns["HoldPrice"].Visible = false;
					this.dvgTradeInfo.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENPRICE"));
					this.dvgTradeInfo.Columns["TransferPL"].Visible = false;
					this.dvgTradeInfo.Columns["OpenpRice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENPRICE"));
					this.dvgTradeInfo.Columns["OpenpRice"].Visible = false;
				}
				else if (key == SettleBasis.Close.ToString("d"))
				{
					this.dvgTradeInfo.Columns["OrderTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENTIME"));
					this.dvgTradeInfo.Columns["OrderTime"].Visible = true;
					this.dvgTradeInfo.Columns["TradeTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_CLOSETIME"));
					this.dvgTradeInfo.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_ORDERNO"));
					this.dvgTradeInfo.Columns["HoldPrice"].Visible = true;
					this.dvgTradeInfo.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_CLOSEPRICE"));
					this.dvgTradeInfo.Columns["TransferPL"].Visible = true;
					this.dvgTradeInfo.Columns["OpenpRice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENPRICE"));
					this.dvgTradeInfo.Columns["OpenpRice"].Visible = true;
				}
				else
				{
					this.dvgTradeInfo.Columns["OrderTime"].Visible = false;
					this.dvgTradeInfo.Columns["TradeTime"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRADETIME"));
					this.dvgTradeInfo.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_ORDERNO"));
					this.dvgTradeInfo.Columns["HoldPrice"].Visible = false;
					this.dvgTradeInfo.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_TRADEDEALPRICE"));
					this.dvgTradeInfo.Columns["TransferPL"].Visible = true;
					this.dvgTradeInfo.Columns["OpenpRice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_OPENPRICE"));
					this.dvgTradeInfo.Columns["OpenpRice"].Visible = false;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void btSelectF4_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.F4Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
				{
					this.btSelectF4.Enabled = false;
					this.UpDataTabCtrl();
				}
				else
				{
					MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void butReset_Click(object sender, EventArgs e)
		{
			this.cbCommodityF4.SelectedIndex = 0;
			this.cbBuySellF4.SelectedIndex = 0;
			this.cbSettleBasisF4.SelectedIndex = 0;
		}
		private void dvgTradeInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == this.dvgTradeInfo.Columns["TransferPL"].Index)
			{
				if (Convert.ToDecimal(e.Value) > 0m)
				{
					e.CellStyle.ForeColor = Color.Red;
					return;
				}
				if (Convert.ToDecimal(e.Value) < 0m)
				{
					e.CellStyle.ForeColor = Color.Green;
					return;
				}
				e.CellStyle.ForeColor = Color.Black;
			}
		}
		private void dvgTradeInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataView dataView = (DataView)this.dvgTradeInfo.DataSource;
			try
			{
				dataView.Sort = " AutoID ASC, " + this.dvgTradeInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			catch (Exception)
			{
				dataView.Sort = " " + this.dvgTradeInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
			}
			finally
			{
				if (this.m_order == " ASC ")
				{
					this.dvgTradeInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
					this.m_order = " Desc ";
				}
				else
				{
					this.dvgTradeInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
					this.m_order = " ASC ";
				}
			}
		}
		private void SetOIF3DataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvOrderInfoF3.Columns["OrderNo"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["OrderNo"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERNO"));
				this.dgvOrderInfoF3.Columns["OrderNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["CommodityName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["CommodityName"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvOrderInfoF3.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["SellBuy"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["SellBuy"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["SellBuy"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_SELLBUY"));
				this.dgvOrderInfoF3.Columns["SellBuy"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["TradeQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfoF3.Columns["TradeQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["TradeQuantity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TRADEQUANTITY"));
				this.dgvOrderInfoF3.Columns["TradeQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["OrderPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfoF3.Columns["OrderPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["OrderPrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERPRICE"));
				this.dgvOrderInfoF3.Columns["OrderPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["StopLoss"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfoF3.Columns["StopLoss"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["StopLoss"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPLOSS"));
				this.dgvOrderInfoF3.Columns["StopLoss"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["StopLossShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfoF3.Columns["StopLossShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["StopLossShow"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPLOSS"));
				this.dgvOrderInfoF3.Columns["StopLossShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvOrderInfoF3.Columns["StopLossShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				defaultCellStyle = this.dgvOrderInfoF3.Columns["StopProfit"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfoF3.Columns["StopProfit"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["StopProfit"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPPROFIT"));
				this.dgvOrderInfoF3.Columns["StopProfit"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["StopProfitShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfoF3.Columns["StopProfitShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["StopProfitShow"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPPROFIT"));
				this.dgvOrderInfoF3.Columns["StopProfitShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvOrderInfoF3.Columns["StopProfitShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				defaultCellStyle = this.dgvOrderInfoF3.Columns["FrozenMargin"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvOrderInfoF3.Columns["FrozenMargin"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["FrozenMargin"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FROZENFUND"));
				this.dgvOrderInfoF3.Columns["FrozenMargin"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["FrozenFee"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvOrderInfoF3.Columns["FrozenFee"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["FrozenFee"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "AI_ORDERFROZEN"));
				this.dgvOrderInfoF3.Columns["FrozenFee"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["OrderInfoState"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["OrderInfoState"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERINFOSTATE"));
				this.dgvOrderInfoF3.Columns["OrderInfoState"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["OrderType"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["OrderType"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TRADETYPE"));
				this.dgvOrderInfoF3.Columns["OrderType"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["SettleBasis"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["SettleBasis"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["SettleBasis"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "TI_SETTLEBASIS"));
				this.dgvOrderInfoF3.Columns["SettleBasis"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["HoldingNo"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["HoldingNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_CLOSEHOLDINGNO"));
				this.dgvOrderInfoF3.Columns["HoldingNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["HoldingNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvOrderInfoF3.Columns["HoldingNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["Time"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfoF3.Columns["Time"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["Time"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TIME"));
				this.dgvOrderInfoF3.Columns["Time"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfoF3.Columns["AgentID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfoF3.Columns["AgentID"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfoF3.Columns["AgentID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_AGENTID");
				this.dgvOrderInfoF3.Columns["AgentID"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				this.dgvOrderInfoF3.Columns["AgentID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvOrderInfoF3.Columns["CommodityID"].HeaderText = "CommodityID";
				this.dgvOrderInfoF3.Columns["CommodityID"].Visible = false;
				this.dgvOrderInfoF3.Columns["OrderInfoStateVal"].HeaderText = "State";
				this.dgvOrderInfoF3.Columns["OrderInfoStateVal"].Visible = false;
				this.dgvOrderInfoF3.Columns["OrderTypeVal"].HeaderText = "OrderInfoTypeVal";
				this.dgvOrderInfoF3.Columns["OrderTypeVal"].Visible = false;
				this.dgvOrderInfoF3.Columns["BuySellVal"].HeaderText = "BuySellVal";
				this.dgvOrderInfoF3.Columns["BuySellVal"].Visible = false;
				this.dgvOrderInfoF3.Columns["StopLoss"].Visible = false;
				this.dgvOrderInfoF3.Columns["StopProfit"].Visible = false;
				this.dgvOrderInfoF3.Columns["SettleBasisVal"].HeaderText = "SettleBasisVal";
				this.dgvOrderInfoF3.Columns["SettleBasisVal"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadOIF3()
		{
			try
			{
				OrderQueryRequestVO orderQueryRequestVO = new OrderQueryRequestVO();
				orderQueryRequestVO.UserID = Global.UserID;
				this.callFillOrderInfoF3DataGrid = new TMainForm.CallbackOrderInfoF3DataGrid(this.FillOrderInfoDataGridF3);
				this.EnableControls(false, "数据查询中");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadOIF3"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOIF3"];
					threadPoolParameter.obj = orderQueryRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadOIF3", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOIF3"];
					threadPoolParameter.obj = orderQueryRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.QueryOrderInfoF3);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryOrderInfoF3(object _orderQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.FillOrderInfoDataGridF3flag)
				{
					this.FillOrderInfoDataGridF3flag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_orderQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)threadPoolParameter.obj;
					DataSet dataSet = this.dataProcess.QueryOrderInfo(orderQueryRequestVO, this._CurrentSystemStatus);
					this.HandleCreated();
					base.BeginInvoke(this.callFillOrderInfoF3DataGrid, new object[]
					{
						dataSet
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.FillOrderInfoDataGridF3flag = false;
			}
		}
		private void FillOrderInfoDataGridF3(DataSet OrderInfoDataView)
		{
			try
			{
				Logger.wirte(1, "FillOrderInfoDataGridF3线程启动1");
				string text = " 1=1 ";
				if (this.cbCommodity.SelectedIndex > 0)
				{
					CBListItem cBListItem = (CBListItem)this.cbCommodity.Items[this.cbCommodity.SelectedIndex];
					text = text + " and CommodityID = '" + cBListItem.Key + "'";
				}
				if (this.cbOrderInfoType.SelectedIndex > 0)
				{
					CBListItem cBListItem2 = (CBListItem)this.cbOrderInfoType.Items[this.cbOrderInfoType.SelectedIndex];
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						" and OrderTypeVal = '",
						Convert.ToInt32(cBListItem2.Key),
						"'"
					});
				}
				if (this.cbStatic.SelectedIndex > 0)
				{
					CBListItem cBListItem3 = (CBListItem)this.cbStatic.Items[this.cbStatic.SelectedIndex];
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						" and OrderInfoStateVal ='",
						Convert.ToInt32(cBListItem3.Key),
						"' "
					});
				}
				if (this.cbBuySell.SelectedIndex > 0)
				{
					CBListItem cBListItem4 = (CBListItem)this.cbBuySell.Items[this.cbBuySell.SelectedIndex];
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						" and BuySellVal ='",
						Convert.ToInt32(cBListItem4.Key),
						"' "
					});
				}
				if (this.cbSettleBasis.SelectedIndex > 0)
				{
					CBListItem cBListItem5 = (CBListItem)this.cbSettleBasis.Items[this.cbSettleBasis.SelectedIndex];
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						" and SettleBasisVal = '",
						Convert.ToInt32(cBListItem5.Key),
						"'"
					});
				}
				if (OrderInfoDataView != null)
				{
					DataTable dataTable = this.DataTableSort(OrderInfoDataView.Tables["Order"], text, "OrderNo", "Desc");
					this.DataViewAddQueryF3Sum(dataTable.DefaultView);
					this.dgvOrderInfoF3.DataSource = dataTable.DefaultView;
				}
				Logger.wirte(1, "FillOrderInfoDataGridF3线程2");
				this.gbOrderInfoF3.Text = Global.m_PMESResourceManager.GetString("PMESStr_GB_TI");
				this.SetOIF3DataGridColText();
				Logger.wirte(1, "FillOrderInfoDataGridF3线程3");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, "");
			this.Selectbt.Enabled = true;
			Logger.wirte(1, "FillOrderInfoDataGridF3线程完成");
		}
		private DataTable DataTableSort(DataTable dtable, string sql, string columnName, string sortFld)
		{
			DataRow[] array;
			try
			{
				array = dtable.Select(sql + " or AutoID='100000' ", " AutoID ASC, " + columnName + " " + sortFld);
			}
			catch (Exception)
			{
				array = dtable.Select(sql, columnName + " " + sortFld);
			}
			DataTable dataTable = dtable.Clone();
			dataTable.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			return dataTable;
		}
		private DataView DataViewAddQueryF3Sum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				double num2 = 0.0;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["TradeQuantity"];
					num2 += (double)dataView[j].Row["FrozenMargin"];
					num3 += (double)dataView[j].Row["FrozenFee"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["SellBuy"] = string.Format(text, dataView.Count - 1);
				dataRowView["TradeQuantity"] = num;
				dataRowView["FrozenMargin"] = num2;
				dataRowView["FrozenFee"] = num3;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
			return dataView;
		}
		private void toolStripMenuItemWithdrawOrder_Click(object sender, EventArgs e)
		{
			if (!this.SubmitConfirm(string.Format("确定撤销{0}吗？", Global.m_PMESResourceManager.GetString("PMESStr_TM_MESSAGEFORM"))))
			{
				return;
			}
			try
			{
				DataGridViewRow dataGridViewRow = this.dgvOrderInfo.Rows[this._XJGridContextMenuRowIndex];
				long orderNo = Convert.ToInt64(dataGridViewRow.Cells["OrderNo"].Value);
				string key = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				string arg = dataGridViewRow.Cells["CommodityName"].Value.ToString();
				WithDrawOrderRequestVO withDrawOrderRequestVO = new WithDrawOrderRequestVO();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(key))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[key];
						if (!commodityInfo.W_D_T_P)
						{
							MessageForm messageForm = new MessageForm("错误", string.Format("没有撤销{0}权限", Global.m_PMESResourceManager.GetString("PMESStr_TM_MESSAGEFORM")), 1, StatusBarType.Error);
							messageForm.Owner = this;
							messageForm.ShowDialog();
							messageForm.Dispose();
						}
						else
						{
							this.FillWithdrawOrderXRequestVO(withDrawOrderRequestVO, orderNo);
							this.WithdrawOrderX(withDrawOrderRequestVO);
						}
					}
					else
					{
						MessageForm messageForm2 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
						messageForm2.Owner = this;
						messageForm2.ShowDialog();
						messageForm2.Dispose();
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(key))
				{
					CommodityInfo commodityInfo2 = Global.CommodityData[key];
					if (!commodityInfo2.W_D_T_P)
					{
						MessageForm messageForm3 = new MessageForm("错误", string.Format("没有撤销{0}权限", Global.m_PMESResourceManager.GetString("PMESStr_TM_MESSAGEFORM")), 1, StatusBarType.Error);
						messageForm3.Owner = this;
						messageForm3.ShowDialog();
						messageForm3.Dispose();
					}
					else
					{
						this.FillWithdrawOrderXRequestVO(withDrawOrderRequestVO, orderNo);
						this.WithdrawOrderX(withDrawOrderRequestVO);
					}
				}
				else
				{
					MessageForm messageForm4 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
					messageForm4.Owner = this;
					messageForm4.ShowDialog();
					messageForm4.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void WithdrawOrderX(WithDrawOrderRequestVO withDrawOrderRequestVO)
		{
			try
			{
				Logger.wirte(1, "下单线程提交，等待程序处理");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("WithdrawOrderX"))
				{
					if (this.DictionarySemaphore.ContainsKey("WithdrawOrderX"))
					{
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["WithdrawOrderX"];
					}
					threadPoolParameter.obj = withDrawOrderRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("WithdrawOrderX", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["WithdrawOrderX"];
					threadPoolParameter.obj = withDrawOrderRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.WithdrawOrderX);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void WithdrawOrderX(object _withDrawOrderRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.WithdrawOrderXflag)
				{
					this.WithdrawOrderXflag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_withDrawOrderRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					WithDrawOrderRequestVO req = (WithDrawOrderRequestVO)threadPoolParameter.obj;
					ResponseVO responseVO = this.dataProcess.TradeLibrary.WithDrawOrder(req);
					TMainForm.ResponseVOWithdrawOrderCallback method = new TMainForm.ResponseVOWithdrawOrderCallback(this.WithdrawOrderMessage);
					this.HandleCreated();
					base.BeginInvoke(method, new object[]
					{
						responseVO
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.WithdrawOrderXflag = false;
			}
		}
		private void WithdrawOrderMessage(ResponseVO responseVO)
		{
			try
			{
				Logger.wirte(1, "WithdrawOrderMessage线程启动1");
				if (responseVO != null && responseVO.RetCode == 0L)
				{
					MessageForm messageForm = new MessageForm("提示", "操作成功！", 1, StatusBarType.Success);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					Logger.wirte(1, "WithdrawOrderMessage线程2");
					this.DelegateLoadOI(true);
					this.DelegateLoadFI(true);
					Logger.wirte(1, "WithdrawOrderMessage线程3");
				}
				else if (IniData.GetInstance().FailShowDialog)
				{
					Logger.wirte(1, "WithdrawOrderMessage线程2");
					MessageForm messageForm2 = new MessageForm("错误", responseVO.RetMessage, 1, StatusBarType.Error);
					messageForm2.Owner = this;
					messageForm2.ShowDialog();
					messageForm2.Dispose();
					Logger.wirte(1, "WithdrawOrderMessage线程3");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "WithdrawOrderMessage线程完成");
		}
		private void FillWithdrawOrderXRequestVO(WithDrawOrderRequestVO req, long orderNo)
		{
			req.OrderNo = orderNo;
			req.UserID = Global.UserID;
		}
		private void toolStripMenuItemXJCancel_Click(object sender, EventArgs e)
		{
			this.contextMenuStripXJ.Close();
		}
		private void toolStripMenuItemXJRefresh_Click(object sender, EventArgs e)
		{
			if (this.F2Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
			{
				this.UpDataTabCtrl();
				return;
			}
			MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
			messageForm.ShowDialog();
		}
		private void dgvOrderInfoF3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
		}
		private void InitComboxLable()
		{
			try
			{
				this.BindCommodity();
				this.BindTradeType();
				this.BindOrderInfoState();
				this.BindBuySell();
				this.BindSettleBasis();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindCommodity()
		{
			try
			{
				this.cbCommodity.Items.Clear();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData == null)
					{
						return;
					}
				}
				else if (Global.CommodityData == null)
				{
					return;
				}
				CBListItem item;
				if (this.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							string commodityName = current.Value.CommodityName;
							string commodityID = current.Value.CommodityID;
							item = new CBListItem(commodityID, commodityName);
							this.cbCommodity.Items.Add(item);
						}
						goto IL_109;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					string commodityName = current2.Value.CommodityName;
					string commodityID = current2.Value.CommodityID;
					item = new CBListItem(commodityID, commodityName);
					this.cbCommodity.Items.Add(item);
				}
				IL_109:
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbCommodity.Items.Insert(0, item);
				this.cbCommodity.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindTradeType()
		{
			try
			{
				this.cbOrderInfoType.Items.Clear();
				if (this.dataProcess.sIdentity == Identity.Member)
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("TRADETYPE", Convert.ToInt32(TradeType.ShiJiaDan));
					string pKey = string.Format("{0}", Convert.ToInt32(TradeType.ShiJiaDan));
					CBListItem item = new CBListItem(pKey, enumtoResourcesString);
					this.cbOrderInfoType.Items.Add(item);
				}
				else
				{
					CBListItem item;
					foreach (int num in Enum.GetValues(typeof(TradeType)))
					{
						string enumtoResourcesString = Global.GetEnumtoResourcesString("TRADETYPE", num);
						string pKey = string.Format("{0}", num);
						item = new CBListItem(pKey, enumtoResourcesString);
						this.cbOrderInfoType.Items.Add(item);
					}
					string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
					item = new CBListItem("-2", @string);
					this.cbOrderInfoType.Items.Insert(0, item);
				}
				this.cbOrderInfoType.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindOrderInfoState()
		{
			try
			{
				this.cbStatic.Items.Clear();
				CBListItem item;
				foreach (int num in Enum.GetValues(typeof(OrderInfoState)))
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("ORDERINFOSTATE", num);
					string pKey = string.Format("{0}", num);
					item = new CBListItem(pKey, enumtoResourcesString);
					this.cbStatic.Items.Add(item);
				}
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbStatic.Items.Insert(0, item);
				this.cbStatic.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindBuySell()
		{
			try
			{
				this.cbBuySell.Items.Clear();
				CBListItem item;
				foreach (int num in Enum.GetValues(typeof(BuySell)))
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("BUYSELL", num);
					string pKey = string.Format("{0}", num);
					item = new CBListItem(pKey, enumtoResourcesString);
					this.cbBuySell.Items.Add(item);
				}
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbBuySell.Items.Insert(0, item);
				this.cbBuySell.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void BindSettleBasis()
		{
			try
			{
				this.cbSettleBasis.Items.Clear();
				CBListItem item;
				foreach (int num in Enum.GetValues(typeof(SettleBasis)))
				{
					string enumtoResourcesString = Global.GetEnumtoResourcesString("SETTLEBASIS", num);
					string pKey = string.Format("{0}", num);
					item = new CBListItem(pKey, enumtoResourcesString);
					this.cbSettleBasis.Items.Add(item);
				}
				string @string = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "DL_SHOWALL"));
				item = new CBListItem("-2", @string);
				this.cbSettleBasis.Items.Insert(0, item);
				this.cbSettleBasis.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void Selectbt_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.F3Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
				{
					this.Selectbt.Enabled = false;
					this.UpDataTabCtrl();
				}
				else
				{
					MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void btReset_Click(object sender, EventArgs e)
		{
			this.cbCommodity.SelectedIndex = 0;
			this.cbOrderInfoType.SelectedIndex = 0;
			this.cbBuySell.SelectedIndex = 0;
			this.cbStatic.SelectedIndex = 0;
			this.cbSettleBasis.SelectedIndex = 0;
		}
		private void dgvOrderInfoF3_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DataView dataView = (DataView)this.dgvOrderInfoF3.DataSource;
				try
				{
					dataView.Sort = " AutoID ASC, " + this.dgvOrderInfoF3.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				catch (Exception)
				{
					dataView.Sort = " " + this.dgvOrderInfoF3.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				finally
				{
					if (this.m_order == " ASC ")
					{
						this.dgvOrderInfoF3.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						this.m_order = " Desc ";
					}
					else
					{
						this.dgvOrderInfoF3.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
						this.m_order = " ASC ";
					}
				}
			}
		}
		private void SetHDIDataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvHoldingDetailInfo.Columns["HoldingID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfo.Columns["HoldingID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["HoldingID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGIDID");
				this.dgvHoldingDetailInfo.Columns["HoldingID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["CommodityName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfo.Columns["CommodityName"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvHoldingDetailInfo.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["BuySellText"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvHoldingDetailInfo.Columns["BuySellText"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["BuySellText"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_SELLBUY"));
				this.dgvHoldingDetailInfo.Columns["BuySellText"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["OpenQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["OpenQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["OpenQuantity"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OPENQUANTITY");
				this.dgvHoldingDetailInfo.Columns["OpenQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["HoldingQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["HoldingQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["HoldingQuantity"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGQUANTITY");
				this.dgvHoldingDetailInfo.Columns["HoldingQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["OpenPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["OpenPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["OpenPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OPENPRICE");
				this.dgvHoldingDetailInfo.Columns["OpenPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["HoldPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["HoldPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["HoldPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDPRICE");
				this.dgvHoldingDetailInfo.Columns["HoldPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["ClosePrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["ClosePrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["ClosePrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_CLOSEPRICE");
				this.dgvHoldingDetailInfo.Columns["ClosePrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["StopLossShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["StopLossShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["StopLossShow"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPLOSS");
				this.dgvHoldingDetailInfo.Columns["StopLossShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfo.Columns["StopLossShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["StopLoss"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["StopLoss"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["StopLoss"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPLOSS");
				this.dgvHoldingDetailInfo.Columns["StopLoss"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["StopProfit"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["StopProfit"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["StopProfit"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPPROFIT");
				this.dgvHoldingDetailInfo.Columns["StopProfit"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["StopProfitShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["StopProfitShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["StopProfitShow"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_STOPPROFIT");
				this.dgvHoldingDetailInfo.Columns["StopProfitShow"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				this.dgvHoldingDetailInfo.Columns["StopProfitShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["HoldingFloat"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfo.Columns["HoldingFloat"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["HoldingFloat"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_HOLDINGFLOAT");
				this.dgvHoldingDetailInfo.Columns["HoldingFloat"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["FloatingPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfo.Columns["FloatingPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["FloatingPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_FLOATINGPRICE");
				this.dgvHoldingDetailInfo.Columns["FloatingPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["TotalFloat"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfo.Columns["TotalFloat"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["TotalFloat"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_TOTALFLOAT");
				this.dgvHoldingDetailInfo.Columns["TotalFloat"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["CommPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvHoldingDetailInfo.Columns["CommPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["CommPrice"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_COMMPRICE");
				this.dgvHoldingDetailInfo.Columns["CommPrice"].Visible = false;
				this.dgvHoldingDetailInfo.Columns["CommPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["OrderTime"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["OrderTime"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["OrderTime"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_ORDERTIME");
				this.dgvHoldingDetailInfo.Columns["OrderTime"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["Bail"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvHoldingDetailInfo.Columns["Bail"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["Bail"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_MAR");
				this.dgvHoldingDetailInfo.Columns["Bail"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["OtherID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["OtherID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["OtherID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_OTHERID");
				this.dgvHoldingDetailInfo.Columns["OtherID"].Visible = (this.dataProcess.sIdentity == Identity.Member);
				this.dgvHoldingDetailInfo.Columns["OtherID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvHoldingDetailInfo.Columns["AgentID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvHoldingDetailInfo.Columns["AgentID"].DefaultCellStyle = defaultCellStyle;
				this.dgvHoldingDetailInfo.Columns["AgentID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_AGENTID");
				this.dgvHoldingDetailInfo.Columns["AgentID"].Visible = (this.dataProcess.sIdentity == Identity.Client);
				this.dgvHoldingDetailInfo.Columns["AgentID"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvHoldingDetailInfo.Columns["CommodityID"].HeaderText = "";
				this.dgvHoldingDetailInfo.Columns["CommodityID"].Visible = false;
				this.dgvHoldingDetailInfo.Columns["BuySell"].HeaderText = "";
				this.dgvHoldingDetailInfo.Columns["BuySell"].Visible = false;
				this.dgvHoldingDetailInfo.Columns["StopProfit"].Visible = false;
				this.dgvHoldingDetailInfo.Columns["StopLoss"].Visible = false;
				this.dgvHoldingDetailInfo.Columns["UnitQty"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetOIDataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvOrderInfo.Columns["OrderNo"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["OrderNo"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["OrderNo"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERNO"));
				this.dgvOrderInfo.Columns["OrderNo"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["CommodityName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["CommodityName"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["CommodityName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvOrderInfo.Columns["CommodityName"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["SellBuy"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["SellBuy"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["SellBuy"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_SELLBUY"));
				this.dgvOrderInfo.Columns["SellBuy"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["TradeQuantity"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfo.Columns["TradeQuantity"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["TradeQuantity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TRADEQUANTITY"));
				this.dgvOrderInfo.Columns["TradeQuantity"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["OrderPrice"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfo.Columns["OrderPrice"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["OrderPrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERPRICE"));
				this.dgvOrderInfo.Columns["OrderPrice"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["StopLoss"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfo.Columns["StopLoss"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["StopLoss"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPLOSS"));
				this.dgvOrderInfo.Columns["StopLoss"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["StopLossShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfo.Columns["StopLossShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["StopLossShow"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPLOSS"));
				this.dgvOrderInfo.Columns["StopLossShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["StopProfit"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "f2";
				this.dgvOrderInfo.Columns["StopProfit"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["StopProfit"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPPROFIT"));
				this.dgvOrderInfo.Columns["StopProfit"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["StopProfitShow"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfo.Columns["StopProfitShow"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["StopProfitShow"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_STOPPROFIT"));
				this.dgvOrderInfo.Columns["StopProfitShow"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["FrozenMargin"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvOrderInfo.Columns["FrozenMargin"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["FrozenMargin"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FROZENFUND"));
				this.dgvOrderInfo.Columns["FrozenMargin"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["FrozenFee"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvOrderInfo.Columns["FrozenFee"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["FrozenFee"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "AI_ORDERFROZEN"));
				this.dgvOrderInfo.Columns["FrozenFee"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["Time"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["Time"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["Time"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TIME"));
				this.dgvOrderInfo.Columns["Time"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["OrderInfoState"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["OrderInfoState"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_ORDERINFOSTATE"));
				this.dgvOrderInfo.Columns["OrderInfoState"].SortMode = DataGridViewColumnSortMode.Programmatic;
				defaultCellStyle = this.dgvOrderInfo.Columns["OrderType"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvOrderInfo.Columns["OrderType"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "OI_TRADETYPE"));
				this.dgvOrderInfo.Columns["OrderType"].SortMode = DataGridViewColumnSortMode.Programmatic;
				this.dgvOrderInfo.Columns["CommodityID"].HeaderText = "CommodityID";
				this.dgvOrderInfo.Columns["CommodityID"].Visible = false;
				this.dgvOrderInfo.Columns["OrderInfoStateVal"].HeaderText = "State";
				this.dgvOrderInfo.Columns["OrderInfoStateVal"].Visible = false;
				this.dgvOrderInfo.Columns["OrderTypeVal"].HeaderText = "OrderInfoTypeVal";
				this.dgvOrderInfo.Columns["OrderTypeVal"].Visible = false;
				this.dgvOrderInfo.Columns["BuySellVal"].HeaderText = "BuySellVal";
				this.dgvOrderInfo.Columns["BuySellVal"].Visible = false;
				this.dgvOrderInfo.Columns["StopProfit"].Visible = false;
				this.dgvOrderInfo.Columns["StopLoss"].Visible = false;
				this.dgvOrderInfo.Columns["SettleBasisVal"].Visible = false;
				this.dgvOrderInfo.Columns["SettleBasis"].Visible = false;
				this.dgvOrderInfo.Columns["HoldingNo"].Visible = false;
				defaultCellStyle = this.dgvOrderInfo.Columns["AgentID"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvOrderInfo.Columns["AgentID"].DefaultCellStyle = defaultCellStyle;
				this.dgvOrderInfo.Columns["AgentID"].HeaderText = Global.m_PMESResourceManager.GetString("PMESStr_HDI_AGENTID");
				this.dgvOrderInfo.Columns["AgentID"].Visible = (this.dataProcess.sIdentity == Identity.Client);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void SetFIDataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvFirmInfo.Columns["FirmName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvFirmInfo.Columns["FirmName"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["FirmName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_ACCFUND"));
				this.dgvFirmInfo.Columns["FirmName"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["InitFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["InitFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["InitFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_TOTALFUNDS"));
				this.dgvFirmInfo.Columns["InitFund"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentRight"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentRight"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentRight"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_BALANCE"));
				this.dgvFirmInfo.Columns["CurrentRight"].Width = 100;
				this.dgvFirmInfo.Columns["CurrentRight"].Visible = false;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentFL"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentFL"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentFL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FLOATINGLP"));
				this.dgvFirmInfo.Columns["CurrentFL"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["RealFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["RealFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["RealFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_CURRENTBAIL"));
				this.dgvFirmInfo.Columns["RealFund"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentBail"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentBail"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentBail"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_USEDMARGIN"));
				this.dgvFirmInfo.Columns["CurrentBail"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["OrderFrozenFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["OrderFrozenFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["OrderFrozenFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FROZENFUND"));
				this.dgvFirmInfo.Columns["OrderFrozenFund"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["OrderFrozenFee"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["OrderFrozenFee"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["OrderFrozenFee"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "AI_ORDERFROZEN"));
				this.dgvFirmInfo.Columns["OrderFrozenFee"].Width = 100;
				defaultCellStyle = this.dgvFirmInfo.Columns["FundRisk"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvFirmInfo.Columns["FundRisk"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["FundRisk"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_RISK"));
				this.dgvFirmInfo.Columns["FundRisk"].Width = 100;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadHDI(bool showEnableControls)
		{
			try
			{
				if (!this.UpDataHoldingDetailInfoflag)
				{
					this.UpDataHoldingDetailInfoflag = true;
					HoldingDetailRequestVO holdingDetailRequestVO = new HoldingDetailRequestVO();
					holdingDetailRequestVO.UserID = Global.UserID;
					this.callFillHoldingDataGrid = new TMainForm.CallbackHoldingDataGrid(this.FillHoldingDataGrid);
					if (showEnableControls)
					{
						this.EnableControls(false, "数据查询中");
					}
					ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
					if (this.DictionarySemaphore.ContainsKey("DelegateLoadHDI"))
					{
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHDI"];
						threadPoolParameter.obj = holdingDetailRequestVO;
					}
					else
					{
						this.DictionarySemaphore.Add("DelegateLoadHDI", new AutoResetEvent(true));
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadHDI"];
						threadPoolParameter.obj = holdingDetailRequestVO;
					}
					WaitCallback callBack = new WaitCallback(this.QueryHolding);
					ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadOI(bool showEnableControls)
		{
			try
			{
				if (!this.UpdateOrderInfoDataGridflag)
				{
					this.UpdateOrderInfoDataGridflag = true;
					OrderQueryRequestVO orderQueryRequestVO = new OrderQueryRequestVO();
					orderQueryRequestVO.UserID = Global.UserID;
					this.callFillOrderInfoDataGrid = new TMainForm.CallbackOrderInfoDataGrid(this.FillOrderInfoDataGrid);
					if (showEnableControls)
					{
						this.EnableControls(false, "数据查询中");
					}
					ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
					if (this.DictionarySemaphore.ContainsKey("DelegateLoadOI"))
					{
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOI"];
						threadPoolParameter.obj = orderQueryRequestVO;
					}
					else
					{
						this.DictionarySemaphore.Add("DelegateLoadOI", new AutoResetEvent(true));
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadOI"];
						threadPoolParameter.obj = orderQueryRequestVO;
					}
					WaitCallback callBack = new WaitCallback(this.QueryOrderInfo);
					ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadFI(bool showEnableControls)
		{
			try
			{
				if (showEnableControls)
				{
					this.EnableControls(false, "数据查询中");
				}
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("DelegateLoadFI"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadFI"];
					threadPoolParameter.obj = null;
				}
				else
				{
					this.DictionarySemaphore.Add("DelegateLoadFI", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadFI"];
					threadPoolParameter.obj = null;
				}
				WaitCallback callBack = new WaitCallback(this.QueryFirmInfo);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryFirmInfo(object _FirmInforQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.FirmInfoflag)
				{
					this.FirmInfoflag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_FirmInforQueryRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					DataSet firmInfo = this.dataProcess.GetFirmInfo();
					if (this.callbackFirmInfoDataGrid != null)
					{
						this.callbackFirmInfoDataGrid(firmInfo);
					}
					this.HandleCreated();
					this.fillFirmInfoCallBack = new TMainForm.FillFirmInfoCallBack(this.FillFirmInfoDataGrid);
					base.BeginInvoke(this.fillFirmInfoCallBack, new object[]
					{
						firmInfo
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.FirmInfoflag = false;
			}
		}
		private void FillFirmInfoDataGrid(DataSet FirmInfo)
		{
			try
			{
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动1");
				if (FirmInfo != null)
				{
					DataView dataView = new DataView(FirmInfo.Tables["tFirmInfo"]);
					this.dgvFirmInfo.DataSource = dataView.Table;
				}
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动2");
				this.dgvFirmInfo.ClearSelection();
				this.SetFIDataGridColText();
				bool flag = true;
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动3");
				if (this.dataProcess.IsAgency)
				{
					lock (Global.AgencyFirmInfoDataLock)
					{
						flag = (Global.AgencyFirmInfoData == null);
						goto IL_B1;
					}
				}
				lock (Global.FirmInfoDataLock)
				{
					flag = (Global.FirmInfoData == null);
				}
				IL_B1:
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动4");
				if (flag)
				{
					this.EnableControls(true, "数据查询完毕");
					this.info.ForeColor = Color.Red;
					this.info.Text = "帐户信息异常";
					MessageForm messageForm = new MessageForm("帐户信息异常", "帐户信息异常", 1);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					Logger.wirte(1, "FillFirmInfoDataGrid线程启动5");
					return;
				}
				this.info.ForeColor = Color.Black;
				this.info.Text = "信息提示";
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动5");
				this.FirmInfoflag = false;
				SystemStatus currentSystemStatus;
				lock (this._CurrentSystemStatusObject)
				{
					currentSystemStatus = this._CurrentSystemStatus;
				}
				if (currentSystemStatus != SystemStatus.SettlementComplete)
				{
					this.CalculateFirmInfo();
					if (this.updateFirmInfo != null)
					{
						this.updateFirmInfo(this.floatingPT);
					}
				}
				this.dgvFirmInfo.ClearSelection();
				Logger.wirte(1, "FillFirmInfoDataGrid线程启动6");
				this.EnableControls(true, "数据查询完毕");
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "FillFirmInfoDataGrid线程启动完成");
		}
		private void QueryHolding(object _orderQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_orderQueryRequestVO;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				HoldingDetailRequestVO holdingDetailRequestVO = (HoldingDetailRequestVO)threadPoolParameter.obj;
				lock (this._CurrentSystemStatusObject)
				{
				}
				lock (this.LockDIDataTable)
				{
					this._HDIDataTable = this.dataProcess.QueryHoldingDetailInfo(holdingDetailRequestVO, this._CurrentSystemStatus);
					Logger.wirte(1, this._HDIDataTable.Tables[0].Rows.Count.ToString());
				}
				this.HandleCreated();
				base.BeginInvoke(this.callFillHoldingDataGrid, new object[]
				{
					this._HDIDataTable
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.UpDataHoldingDetailInfoflag = false;
				if (this.refreshHoldingDetailInfo == 0)
				{
					this.refreshHoldingDetailInfo = 1;
				}
			}
		}
		private void FillHoldingDataGrid(DataSet HoldingDetailInfo)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvHoldingDetailInfo.SelectedCells.Count != 0)
				{
					num = this.dgvHoldingDetailInfo.SelectedCells[0].RowIndex;
				}
				if (this.dgvHoldingDetailInfo.SortedColumn != null)
				{
					text = this.dgvHoldingDetailInfo.SortedColumn.Name;
					SortOrder sortOrder = this.dgvHoldingDetailInfo.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				Logger.wirte(1, "FillHoldingDataGrid线程启动 1");
				if (HoldingDetailInfo != null)
				{
					DataTable dataTable = HoldingDetailInfo.Tables["HDIDetatable"];
					Logger.wirte(1, "FillHoldingDataGrid线程2");
					SystemStatus currentSystemStatus;
					lock (this._CurrentSystemStatusObject)
					{
						currentSystemStatus = this._CurrentSystemStatus;
					}
					if (currentSystemStatus != SystemStatus.SettlementComplete)
					{
						this.UpDataHoldingDetailInfo();
					}
					this.DataViewAddQueryF2HoldingSum(dataTable.DefaultView);
					this.dgvHoldingDetailInfo.DataSource = dataTable.DefaultView;
					this._orderDataTable = dataTable;
					dataTable.DefaultView.Sort = "HoldingID desc ";
					Logger.wirte(1, "FillHoldingDataGrid线程3");
				}
				this.SetHDIDataGridColText();
				Logger.wirte(1, "FillHoldingDataGrid线程4");
				Logger.wirte(1, "FillHoldingDataGrid线程5");
				if (this.dgvHoldingDetailInfo.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvHoldingDetailInfo.Sort(this.dgvHoldingDetailInfo.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvHoldingDetailInfo.Rows.Count)
					{
						this.dgvHoldingDetailInfo.CurrentCell = this.dgvHoldingDetailInfo.Rows[num].Cells[0];
					}
					else
					{
						this.dgvHoldingDetailInfo.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, "");
			Logger.wirte(1, "FillHoldingDataGrid线程完成");
		}
		private void DataViewAddQueryF2HoldingSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				long num2 = 0L;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["OpenQuantity"];
					num2 += (long)dataView[j].Row["HoldingQuantity"];
					num3 += (double)dataView[j].Row["Bail"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["BuySellText"] = string.Format(text, dataView.Count - 1);
				dataRowView["OpenQuantity"] = num;
				dataRowView["HoldingQuantity"] = num2;
				dataRowView["Bail"] = num3;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
		}
		private void QueryOrderInfo(object _orderQueryRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_orderQueryRequestVO;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)threadPoolParameter.obj;
				DataSet dataSet = this.dataProcess.QueryOrderInfo(orderQueryRequestVO, this._CurrentSystemStatus);
				this.HandleCreated();
				base.BeginInvoke(this.callFillOrderInfoDataGrid, new object[]
				{
					dataSet
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.UpdateOrderInfoDataGridflag = false;
			}
		}
		private void InvokeUpDataHoldingDetailInfoHQ()
		{
			try
			{
				this.UpDataHoldingDetailInfo();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpDataHoldingDetailInfoHQ()
		{
			this.callbackUpDataHoldingDetailInfoHQ = new TMainForm.CallbackUpDataHoldingDetailInfoHQ(this.InvokeUpDataHoldingDetailInfoHQ);
			this.HandleCreated();
			base.BeginInvoke(this.callbackUpDataHoldingDetailInfoHQ, new object[0]);
		}
		private void UpDataHoldingDetailInfo()
		{
			try
			{
				int num = 0;
				lock (this.iSelecttablock)
				{
					num = this.iSelecttab;
				}
				if (this.dgvHoldingDetailInfo.Rows.Count == 0)
				{
					lock (this.floatingPriceTotalLock)
					{
						this.floatingPT = 0.0;
					}
					if (this.dataProcess.sIdentity == Identity.Client && this.updateFirmInfo != null)
					{
						this.updateFirmInfo(this.floatingPT);
					}
					if (num == 0)
					{
						this.CalculateFirmInfo();
					}
					else if (num == 5)
					{
						this.CalculateFirmInfoF7();
					}
				}
				else
				{
					if (this.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData == null)
							{
								lock (this.floatingPriceTotalLock)
								{
									this.floatingPT = 0.0;
								}
								this.CalculateFirmInfo();
								if (this.updateFirmInfo != null)
								{
									this.updateFirmInfo(this.floatingPT);
								}
								this.CalculateFirmInfoF7();
								return;
							}
							goto IL_185;
						}
					}
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData == null)
						{
							lock (this.floatingPriceTotalLock)
							{
								this.floatingPT = 0.0;
							}
							this.CalculateFirmInfo();
							if (this.updateFirmInfo != null)
							{
								this.updateFirmInfo(this.floatingPT);
							}
							this.CalculateFirmInfoF7();
							return;
						}
					}
					IL_185:
					int arg_195_0 = this.dgvHoldingDetailInfo.Rows.Count;
					Dictionary<string, CommData> dictionary = null;
					if (this.dataProcess.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData != null)
							{
								dictionary = Global.gAgencyHQCommData;
							}
							goto IL_1ED;
						}
					}
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData != null)
						{
							dictionary = Global.gHQCommData;
						}
					}
					IL_1ED:
					lock (this.LockDIDataTable)
					{
						foreach (DataRow dataRow in this._HDIDataTable.Tables[0].Rows)
						{
							string a = dataRow["BuySell"].ToString();
							string key = dataRow["CommodityID"].ToString();
							if (a == BuySell.Buy.ToString("d"))
							{
								if (dictionary != null && dictionary.ContainsKey(key))
								{
									double sellPrice = dictionary[key].SellPrice;
									dataRow["ClosePrice"] = sellPrice;
								}
							}
							else if (a == BuySell.Sell.ToString("d") && dictionary != null && dictionary.ContainsKey(key))
							{
								dataRow["ClosePrice"] = dictionary[key].BuyPrice;
							}
						}
					}
					lock (this.floatingPriceTotalLock)
					{
						if (this._HDIDataTable != null && this._HDIDataTable.Tables.Count > 0)
						{
							try
							{
								this.floatingPT = Convert.ToDouble(this._HDIDataTable.Tables[0].Compute("Sum(FloatingPrice)", ""));
							}
							catch (Exception ex)
							{
								Logger.wirte(3, string.Format("Sum(FloatingPrice) Error. Current floatingPT : {0}, Error Message:{1}", this.floatingPT, ex.Message + ex.StackTrace));
							}
						}
					}
					if (this.dataProcess.sIdentity == Identity.Client && this.updateFirmInfo != null)
					{
						this.updateFirmInfo(this.floatingPT);
					}
					if (num == 0)
					{
						this.CalculateFirmInfo();
					}
					else if (num == 5)
					{
						this.CalculateFirmInfoF7();
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.wirte(ex2);
			}
		}
		private void CalculateFirmInfo()
		{
			try
			{
				double num;
				lock (this.floatingPriceTotalLock)
				{
					num = this.floatingPT;
				}
				this.callbackUpDataFirmInfo = new TMainForm.CallbackUpDataFirmInfo(this.UpDataFirmInfo);
				this.HandleCreated();
				base.BeginInvoke(this.callbackUpDataFirmInfo, new object[]
				{
					num
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpDataFirmInfo(double floatingPriceTotal)
		{
			try
			{
				FirmInfoResponseVO firmInfoResponseVO = null;
				if (!this.FirmInfoflag)
				{
					this.FirmInfoflag = true;
					if (this.dgvFirmInfo == null || this.dgvFirmInfo.Rows.Count == 0)
					{
						this.FirmInfoflag = false;
					}
					else
					{
						int rowCount = this.dgvFirmInfo.RowCount;
						if (this.dataProcess.IsAgency)
						{
							lock (Global.AgencyFirmInfoDataLock)
							{
								if (Global.AgencyFirmInfoData == null)
								{
									return;
								}
								firmInfoResponseVO = (FirmInfoResponseVO)Global.AgencyFirmInfoData.Clone();
								goto IL_C5;
							}
						}
						lock (Global.FirmInfoDataLock)
						{
							if (Global.FirmInfoData == null)
							{
								return;
							}
							firmInfoResponseVO = (FirmInfoResponseVO)Global.FirmInfoData.Clone();
						}
						IL_C5:
						for (int i = 0; i < rowCount; i++)
						{
							if (firmInfoResponseVO != null)
							{
								double balance = BizController.CalculateBalance(firmInfoResponseVO.InitFund, firmInfoResponseVO.InOutFund, firmInfoResponseVO.Fee, firmInfoResponseVO.YesterdayBail, firmInfoResponseVO.TransferPL);
								this.dgvFirmInfo.Rows[i].Cells["CurrentRight"].Value = balance.ToString("f2");
								DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
								if (Convert.ToDouble(floatingPriceTotal) > 0.0)
								{
									dataGridViewCellStyle.ForeColor = Color.Red;
								}
								else if (Convert.ToDouble(floatingPriceTotal) == 0.0)
								{
									dataGridViewCellStyle.ForeColor = Color.Black;
								}
								else
								{
									dataGridViewCellStyle.ForeColor = Color.Green;
								}
								this.dgvFirmInfo.Rows[i].Cells["CurrentFL"].Style = dataGridViewCellStyle;
								this.dgvFirmInfo.Rows[i].Cells["CurrentFL"].Value = floatingPriceTotal.ToString("f2");
								double initFund = BizController.CalculateInitFund(balance, floatingPriceTotal);
								this.dgvFirmInfo.Rows[i].Cells["InitFund"].Value = initFund.ToString("f2");
								double currentBail = BizController.CalculateHoldingFund(firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.OtherFrozenFund);
								this.dgvFirmInfo.Rows[i].Cells["CurrentBail"].Value = currentBail.ToString("f2");
								double orderFrozenMargin = firmInfoResponseVO.OrderFrozenMargin;
								this.dgvFirmInfo.Rows[i].Cells["OrderFrozenFund"].Value = orderFrozenMargin.ToString("f2");
								this.dgvFirmInfo.Rows[i].Cells["RealFund"].Value = BizController.CalculateRealFund(initFund, firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.UsingFund).ToString("f2");
								if (this.dataProcess.sIdentity == Identity.Client)
								{
									this.dgvFirmInfo.Rows[i].Cells["FundRisk"].Value = BizController.CalculateFundRisk(initFund, currentBail).ToString("p2");
								}
								else if (this.dataProcess.sIdentity == Identity.Member)
								{
									this.dgvFirmInfo.Rows[i].Cells["FundRisk"].Value = firmInfoResponseVO.FundRisk.ToString("p2");
								}
							}
						}
						this.FirmInfoflag = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void FillOrderInfoDataGrid(DataSet OrderInfoDataView)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvOrderInfo.SelectedCells.Count != 0)
				{
					num = this.dgvOrderInfo.SelectedCells[0].RowIndex;
				}
				if (this.dgvOrderInfo.SortedColumn != null)
				{
					text = this.dgvOrderInfo.SortedColumn.Name;
					SortOrder sortOrder = this.dgvOrderInfo.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				if (OrderInfoDataView != null)
				{
					Logger.wirte(1, "FillOrderInfoDataGrid线程启动 1");
					if (this.dgvOrderInfo.SelectedCells.Count != 0)
					{
						num = this.dgvOrderInfo.SelectedCells[0].RowIndex;
					}
					if (this.dgvOrderInfo.SortedColumn != null)
					{
						text = this.dgvOrderInfo.SortedColumn.Name;
						SortOrder sortOrder = this.dgvOrderInfo.SortOrder;
						if (sortOrder == SortOrder.Ascending)
						{
							direction = ListSortDirection.Ascending;
						}
						else
						{
							direction = ListSortDirection.Descending;
						}
					}
					string sql = string.Concat(new string[]
					{
						"  OrderInfoStateVal='",
						OrderInfoState.HasCommissioned.ToString("d"),
						"' and OrderTypeVal='",
						TradeType.XianJiaDan.ToString("d"),
						"' and SettleBasisVal='",
						SettleBasis.Open.ToString("d"),
						"'"
					});
					DataTable dataTable = this.DataTableSort(OrderInfoDataView.Tables["Order"], sql, "OrderNo", "Desc");
					this.DataViewAddQueryF2OrderSum(dataTable.DefaultView);
					Logger.wirte(1, "FillOrderInfoDataGrid线程2");
					this.dgvOrderInfo.DataSource = dataTable.DefaultView;
					dataTable.DefaultView.AllowDelete = true;
					Logger.wirte(1, "FillOrderInfoDataGrid线程3");
				}
				this.SetOIDataGridColText();
				Logger.wirte(1, "FillOrderInfoDataGrid线程4");
				if (this.dgvOrderInfo.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvOrderInfo.Sort(this.dgvOrderInfo.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvOrderInfo.Rows.Count)
					{
						this.dgvOrderInfo.CurrentCell = this.dgvOrderInfo.Rows[num].Cells[0];
					}
					else
					{
						this.dgvOrderInfo.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			this.EnableControls(true, "");
			Logger.wirte(1, "FillOrderInfoDataGrid线程5");
		}
		private void DataViewAddQueryF2OrderSum(DataView dataView)
		{
			if (dataView.Count > 1 && dataView[dataView.Count - 1].Row["CommodityName"].ToString() == this.Total)
			{
				dataView.AllowDelete = true;
				dataView.Delete(dataView.Count - 1);
			}
			else
			{
				for (int i = 0; i < dataView.Count; i++)
				{
					if (dataView[i].Row["CommodityName"].ToString() == this.Total)
					{
						dataView.AllowDelete = true;
						dataView.Delete(i);
					}
				}
			}
			if (dataView.Count > 1)
			{
				dataView.AllowNew = true;
				long num = 0L;
				double num2 = 0.0;
				double num3 = 0.0;
				if (!dataView.Table.Columns.Contains("AutoID"))
				{
					dataView.Table.Columns.Add(new DataColumn("AutoID", typeof(int)));
				}
				for (int j = 0; j < dataView.Count; j++)
				{
					num += (long)dataView[j].Row["TradeQuantity"];
					num2 += (double)dataView[j].Row["FrozenMargin"];
					num3 += (double)dataView[j].Row["FrozenFee"];
				}
				string text = Global.m_PMESResourceManager.GetString("PMESStr_TOTALNUM");
				if (text == null || text.Length == 0)
				{
					text = "共{0}条";
				}
				DataRowView dataRowView = dataView.AddNew();
				dataRowView["CommodityName"] = this.Total;
				dataRowView["SellBuy"] = string.Format(text, dataView.Count - 1);
				dataRowView["TradeQuantity"] = num;
				dataRowView["FrozenMargin"] = num2;
				dataRowView["FrozenFee"] = num3;
				dataRowView["AutoID"] = 100000;
				dataView.Table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			}
		}
		private void toolStripMenuItemFirmInfoRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.F2Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
				{
					this.UpDataTabCtrl();
				}
				else
				{
					MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingDetailInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.dgvHoldingDetailInfo.ClearSelection();
					this.dgvHoldingDetailInfo.Rows[e.RowIndex].Selected = true;
					this._HoldingDetailContextMenuRowIndex = e.RowIndex;
					this._HoldingDetailMenuEnabled = e.RowIndex;
					string text = this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["StopLoss"].Value.ToString().Trim();
					string text2 = this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["StopProfit"].Value.ToString().Trim();
					if (this.SetMenuDisenable("contextMenuStripHoldingDetail"))
					{
						this.toolStripMenuItemStopLoss.Enabled = (text.Length > 0 && Convert.ToDouble(text) != 0.0);
						this.toolStripMenuItemStopProfit.Enabled = (text2.Length > 0 && Convert.ToDouble(text2) != 0.0);
					}
					Point position = this.dgvHoldingDetailInfo.PointToClient(Cursor.Position);
					this.contextMenuStripHoldingDetail.Show(this.dgvHoldingDetailInfo, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemSP_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow;
				if (this.tabTMain.SelectedIndex == 0)
				{
					dataGridViewRow = this.dgvHoldingDetailInfo.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				else
				{
					dataGridViewRow = this.dgvHoldingDetailInfoF5.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				long holdingID = Convert.ToInt64(dataGridViewRow.Cells["HoldingID"].Value);
				string text = dataGridViewRow.Cells["BuySell"].Value.ToString();
				int num = Convert.ToInt32(dataGridViewRow.Cells["HoldingQuantity"].Value);
				string text2 = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				string otherID = dataGridViewRow.Cells["OtherID"].Value.ToString();
				this.pWarehouseForm = new PWarehouseForm(this);
				this.pWarehouseForm.CloseTradeType = TradeType.ShiJiaDan;
				this.pWarehouseForm.CurrentCommodityId = text2;
				if (this.dataProcess.sIdentity == Identity.Member)
				{
					this.pWarehouseForm.OtherID = otherID;
				}
				this.pWarehouseForm.IsCloseSpecificOrder = true;
				CloseCommodityInfo closeCommodityInfo = new CloseCommodityInfo();
				closeCommodityInfo.CommodityID = text2;
				closeCommodityInfo.HoldingID = holdingID;
				if (text == BuySell.Buy.ToString("d"))
				{
					closeCommodityInfo.CloseMaxSellQty = (long)num;
					this.pWarehouseForm.CurrentBuySell = BuySell.Sell.ToString("d");
				}
				else if (text == BuySell.Sell.ToString("d"))
				{
					closeCommodityInfo.CloseMaxBuyQty = (long)num;
					this.pWarehouseForm.CurrentBuySell = BuySell.Buy.ToString("d");
				}
				closeCommodityInfo.OriginBuySell = text;
				this.pWarehouseForm.CloseCommodityInfoList.Add(closeCommodityInfo.CommodityID, closeCommodityInfo);
				this.pWarehouseForm.ShowDialog();
				if (this.pWarehouseForm.IsCloseButtonOKOrCancel)
				{
					this.DelegateRefresh();
					this.DelegateLoadHIF6();
					this.UpDataTabCtrl();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemXP_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewRow dataGridViewRow;
				if (this.tabTMain.SelectedIndex == 0)
				{
					dataGridViewRow = this.dgvHoldingDetailInfo.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				else
				{
					dataGridViewRow = this.dgvHoldingDetailInfoF5.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				long holdingID = Convert.ToInt64(dataGridViewRow.Cells["HoldingID"].Value);
				string text = dataGridViewRow.Cells["BuySell"].Value.ToString();
				int num = Convert.ToInt32(dataGridViewRow.Cells["HoldingQuantity"].Value);
				string text2 = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				double zS = 0.0;
				double zY = 0.0;
				double.TryParse(dataGridViewRow.Cells["StopLoss"].Value.ToString(), out zS);
				double.TryParse(dataGridViewRow.Cells["StopProfit"].Value.ToString(), out zY);
				this.pWarehouseForm = new PWarehouseForm(this);
				this.pWarehouseForm.CloseTradeType = TradeType.XianJiaDan;
				this.pWarehouseForm.MaxCloseQty = num;
				this.pWarehouseForm.CurrentCommodityId = text2;
				this.pWarehouseForm.IsCloseSpecificOrder = true;
				CloseCommodityInfo closeCommodityInfo = new CloseCommodityInfo();
				closeCommodityInfo.CommodityID = text2;
				closeCommodityInfo.HoldingID = holdingID;
				if (text == BuySell.Buy.ToString("d"))
				{
					closeCommodityInfo.CloseMaxSellQty = (long)num;
					this.pWarehouseForm.CurrentBuySell = BuySell.Sell.ToString("d");
				}
				else if (text == BuySell.Sell.ToString("d"))
				{
					closeCommodityInfo.CloseMaxBuyQty = (long)num;
					this.pWarehouseForm.CurrentBuySell = BuySell.Buy.ToString("d");
				}
				closeCommodityInfo.OriginBuySell = text;
				closeCommodityInfo.ZS = zS;
				closeCommodityInfo.ZY = zY;
				this.pWarehouseForm.CloseCommodityInfoList.Add(closeCommodityInfo.CommodityID, closeCommodityInfo);
				this.pWarehouseForm.ShowDialog();
				if (this.pWarehouseForm.IsCloseButtonOKOrCancel)
				{
					this.DelegateRefresh();
					this.DelegateLoadHIF6();
					this.UpDataTabCtrl();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				int selectedIndex = this.tabTMain.SelectedIndex;
				int num = selectedIndex;
				if (num != 0)
				{
					if (num == 3)
					{
						if (this.F5Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
						{
							this.UpDataTabCtrl();
						}
						else
						{
							MessageForm messageForm = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
							messageForm.ShowDialog();
						}
					}
				}
				else if (this.F2Flag || this.IdleOnMoudel >= Tools.StrToInt((string)Global.HTConfig["MaxIdleOnMoudel"], 5))
				{
					this.UpDataTabCtrl();
				}
				else
				{
					MessageForm messageForm2 = new MessageForm("提示", "刷新频率过高请稍候重试！", 1, StatusBarType.Warning);
					messageForm2.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemStopLoss_Click(object sender, EventArgs e)
		{
			if (!this.SubmitConfirm("确定撤销止损单吗？"))
			{
				return;
			}
			try
			{
				DataGridViewRow dataGridViewRow;
				if (this.tabTMain.SelectedIndex == 0)
				{
					dataGridViewRow = this.dgvHoldingDetailInfo.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				else
				{
					dataGridViewRow = this.dgvHoldingDetailInfoF5.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				long holdingID = Convert.ToInt64(dataGridViewRow.Cells["HoldingID"].Value);
				dataGridViewRow.Cells["BuySell"].Value.ToString();
				string text = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				string arg = dataGridViewRow.Cells["CommodityName"].Value.ToString();
				WithdrawLossProfitRequestVO withdrawLossProfitRequestVO = new WithdrawLossProfitRequestVO();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(text))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[text];
						if (!commodityInfo.W_D_S_L_P)
						{
							MessageForm messageForm = new MessageForm("错误", "没有撤销止损权限", 1, StatusBarType.Error);
							messageForm.Owner = this;
							messageForm.ShowDialog();
							messageForm.Dispose();
						}
						else
						{
							this.FillWithdrawLossProfitRequestVO(withdrawLossProfitRequestVO, holdingID, text, WithdrawLossProfitType.WithdrawLoss);
							this.WithdrawLossProfit(withdrawLossProfitRequestVO);
						}
					}
					else
					{
						MessageForm messageForm2 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
						messageForm2.Owner = this;
						messageForm2.ShowDialog();
						messageForm2.Dispose();
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(text))
				{
					CommodityInfo commodityInfo = Global.CommodityData[text];
					if (!commodityInfo.W_D_S_L_P)
					{
						MessageForm messageForm3 = new MessageForm("错误", "没有撤销止损权限", 1, StatusBarType.Error);
						messageForm3.Owner = this;
						messageForm3.ShowDialog();
						messageForm3.Dispose();
					}
					else
					{
						this.FillWithdrawLossProfitRequestVO(withdrawLossProfitRequestVO, holdingID, text, WithdrawLossProfitType.WithdrawLoss);
						this.WithdrawLossProfit(withdrawLossProfitRequestVO);
					}
				}
				else
				{
					MessageForm messageForm4 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
					messageForm4.Owner = this;
					messageForm4.ShowDialog();
					messageForm4.Dispose();
				}
			}
			catch (Exception ex)
			{
				MessageForm messageForm5 = new MessageForm("错误", ex.Message, 1, StatusBarType.Error);
				messageForm5.Owner = this;
				messageForm5.ShowDialog();
				messageForm5.Dispose();
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemStopProfit_Click(object sender, EventArgs e)
		{
			if (!this.SubmitConfirm("确定撤销止盈单吗？"))
			{
				return;
			}
			try
			{
				DataGridViewRow dataGridViewRow;
				if (this.tabTMain.SelectedIndex == 0)
				{
					dataGridViewRow = this.dgvHoldingDetailInfo.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				else
				{
					dataGridViewRow = this.dgvHoldingDetailInfoF5.Rows[this._HoldingDetailContextMenuRowIndex];
				}
				long holdingID = Convert.ToInt64(dataGridViewRow.Cells["HoldingID"].Value);
				dataGridViewRow.Cells["BuySell"].Value.ToString();
				string text = dataGridViewRow.Cells["CommodityID"].Value.ToString();
				string arg = dataGridViewRow.Cells["CommodityName"].Value.ToString();
				WithdrawLossProfitRequestVO withdrawLossProfitRequestVO = new WithdrawLossProfitRequestVO();
				if (this.dataProcess.IsAgency)
				{
					if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(text))
					{
						CommodityInfo commodityInfo = Global.AgencyCommodityData[text];
						if (!commodityInfo.W_D_S_P_P)
						{
							MessageForm messageForm = new MessageForm("错误", "没有撤销止盈权限", 1, StatusBarType.Error);
							messageForm.Owner = this;
							messageForm.ShowDialog();
							messageForm.Dispose();
						}
						else
						{
							this.FillWithdrawLossProfitRequestVO(withdrawLossProfitRequestVO, holdingID, text, WithdrawLossProfitType.WithdrawProfit);
							this.WithdrawLossProfit(withdrawLossProfitRequestVO);
						}
					}
					else
					{
						MessageForm messageForm2 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
						messageForm2.Owner = this;
						messageForm2.ShowDialog();
						messageForm2.Dispose();
					}
				}
				else if (Global.CommodityData != null && Global.CommodityData.ContainsKey(text))
				{
					CommodityInfo commodityInfo = Global.CommodityData[text];
					if (!commodityInfo.W_D_S_P_P)
					{
						MessageForm messageForm3 = new MessageForm("错误", "没有撤销止盈权限", 1, StatusBarType.Error);
						messageForm3.Owner = this;
						messageForm3.ShowDialog();
						messageForm3.Dispose();
					}
					else
					{
						this.FillWithdrawLossProfitRequestVO(withdrawLossProfitRequestVO, holdingID, text, WithdrawLossProfitType.WithdrawProfit);
						this.WithdrawLossProfit(withdrawLossProfitRequestVO);
					}
				}
				else
				{
					MessageForm messageForm4 = new MessageForm("错误", string.Format("无法找到商品\"{0}\"", arg), 1, StatusBarType.Error);
					messageForm4.Owner = this;
					messageForm4.ShowDialog();
					messageForm4.Dispose();
				}
			}
			catch (Exception ex)
			{
				MessageForm messageForm5 = new MessageForm("错误", ex.Message, 1, StatusBarType.Error);
				Logger.wirte(ex);
				messageForm5.Owner = this;
				messageForm5.ShowDialog();
				messageForm5.Dispose();
			}
		}
		private void WithdrawLossProfit(WithdrawLossProfitRequestVO withdrawLossProfitRequestVO)
		{
			try
			{
				Logger.wirte(1, "下单线程提交，等待程序处理");
				ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
				if (this.DictionarySemaphore.ContainsKey("WithdrawLossProfit"))
				{
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["WithdrawLossProfit"];
					threadPoolParameter.obj = withdrawLossProfitRequestVO;
				}
				else
				{
					this.DictionarySemaphore.Add("WithdrawLossProfit", new AutoResetEvent(true));
					threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["WithdrawLossProfit"];
					threadPoolParameter.obj = withdrawLossProfitRequestVO;
				}
				WaitCallback callBack = new WaitCallback(this.WithdrawLossProfit);
				ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void WithdrawLossProfit(object _withdrawLossProfitRequestVO)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				if (!this.WithdrawLossProfitflag)
				{
					this.WithdrawLossProfitflag = true;
					ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_withdrawLossProfitRequestVO;
					autoResetEvent = threadPoolParameter.Semaphores;
					autoResetEvent.Reset();
					WithdrawLossProfitRequestVO req = (WithdrawLossProfitRequestVO)threadPoolParameter.obj;
					ResponseVO responseVO = this.dataProcess.TradeLibrary.WithdrawLossProfit(req);
					TMainForm.ResponseVOWithdrawLossProfitCallback method = new TMainForm.ResponseVOWithdrawLossProfitCallback(this.WithdrawMessage);
					this.HandleCreated();
					base.BeginInvoke(method, new object[]
					{
						responseVO
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.WithdrawLossProfitflag = false;
			}
		}
		private void WithdrawMessage(ResponseVO responseVO)
		{
			try
			{
				if (responseVO != null && responseVO.RetCode == 0L)
				{
					Logger.wirte(1, "WithdrawMessage线程启动1");
					MessageForm messageForm = new MessageForm("提示", "操作成功！", 1, StatusBarType.Success);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					Logger.wirte(1, "WithdrawMessage线程2");
					if (this.tabTMain.SelectedIndex == 0)
					{
						this.DelegateLoadHDI(true);
						Logger.wirte(1, "WithdrawMessage线程3");
					}
					else if (this.tabTMain.SelectedIndex == 3)
					{
						this.DelegateLoadHDIF5();
						Logger.wirte(1, "WithdrawMessage线程3");
					}
				}
				else if (IniData.GetInstance().FailShowDialog)
				{
					MessageForm messageForm2 = new MessageForm("错误", responseVO.RetMessage, 1, StatusBarType.Error);
					messageForm2.Owner = this;
					messageForm2.ShowDialog();
					messageForm2.Dispose();
					Logger.wirte(1, "WithdrawMessage线程4");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "WithdrawMessage线程完成");
		}
		private void FillWithdrawLossProfitRequestVO(WithdrawLossProfitRequestVO req, long holdingID, string commodityID, WithdrawLossProfitType withdrawLossProfitType)
		{
			try
			{
				req.CommodityID = commodityID;
				req.HoldingID = holdingID;
				req.Type = (short)withdrawLossProfitType;
				req.UserID = Global.UserID;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void toolStripMenuItemCancel_Click(object sender, EventArgs e)
		{
			this.contextMenuStripHoldingDetail.Close();
		}
		private void dgvHoldingDetailInfo_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.dgvHoldingDetailInfo.ClearSelection();
				this.SetMenuDisenable("contextMenuStripHoldingDetail");
				this.toolStripMenuItemSP.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemXP.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemStopLoss.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				this.toolStripMenuItemStopProfit.Enabled = (this._HoldingDetailMenuEnabled >= 0);
				Point position = this.dgvHoldingDetailInfo.PointToClient(Cursor.Position);
				this.contextMenuStripHoldingDetail.Show(this.dgvHoldingDetailInfo, position);
			}
		}
		private void contextMenuStripHoldingDetail_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this._HoldingDetailMenuEnabled = -1;
		}
		private void dgvOrderInfo_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.dgvOrderInfo.ClearSelection();
				this.SetMenuDisenable("contextMenuStripXJ");
				this.toolStripMenuItemWithdrawOrder.Enabled = (this._XJGridMenuEnabled >= 0);
				Point position = this.dgvOrderInfo.PointToClient(Cursor.Position);
				this.contextMenuStripXJ.Show(this.dgvOrderInfo, position);
			}
		}
		private void contextMenuStripXJ_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this._XJGridMenuEnabled = -1;
		}
		private double CalculateTotalFrozenFund()
		{
			double num = 0.0;
			try
			{
				if (this.dgvFirmInfo.RowCount <= 0)
				{
					double result = num;
					return result;
				}
				if (this.dgvOrderInfo.RowCount <= 0)
				{
					double result = num;
					return result;
				}
				if (this.dataProcess.IsAgency)
				{
					IEnumerator enumerator = ((IEnumerable)this.dgvOrderInfo.Rows).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataGridViewRow dataGridViewRow = (DataGridViewRow)enumerator.Current;
							string key = dataGridViewRow.Cells["CommodityID"].Value.ToString();
							if (Global.AgencyCommodityData != null && Global.AgencyCommodityData.ContainsKey(key))
							{
								CommodityInfo commodityInfo = Global.AgencyCommodityData[key];
								double holdingQuantity = Convert.ToDouble(dataGridViewRow.Cells["TradeQuantity"].Value);
								double openPrice = Convert.ToDouble(dataGridViewRow.Cells["OrderPrice"].Value);
								num += BizController.CalculateFrozenFund(holdingQuantity, openPrice, commodityInfo.CtrtSize, commodityInfo.MarginValue);
							}
						}
						goto IL_1F7;
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
				foreach (DataGridViewRow dataGridViewRow2 in (IEnumerable)this.dgvOrderInfo.Rows)
				{
					string key2 = dataGridViewRow2.Cells["CommodityID"].Value.ToString();
					if (Global.CommodityData != null && Global.CommodityData.ContainsKey(key2))
					{
						CommodityInfo commodityInfo2 = Global.CommodityData[key2];
						double holdingQuantity2 = Convert.ToDouble(dataGridViewRow2.Cells["TradeQuantity"].Value);
						double openPrice2 = Convert.ToDouble(dataGridViewRow2.Cells["OrderPrice"].Value);
						num += BizController.CalculateFrozenFund(holdingQuantity2, openPrice2, commodityInfo2.CtrtSize, commodityInfo2.MarginValue);
					}
				}
				IL_1F7:;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return num;
		}
		private void contextMenuStripFirmInfo_Opening(object sender, CancelEventArgs e)
		{
			this.SetMenuDisenable("contextMenuStripFirmInfo");
		}
		private void dgvHoldingDetailInfo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
				{
					if (this.identitystatus())
					{
						this._HoldingDetailContextMenuRowIndex = e.RowIndex;
						this.toolStripMenuItemSP_Click(sender, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvHoldingDetailInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DataView dataView = (DataView)this.dgvHoldingDetailInfo.DataSource;
				try
				{
					dataView.Sort = " AutoID ASC, " + this.dgvHoldingDetailInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				catch (Exception)
				{
					dataView.Sort = " " + this.dgvHoldingDetailInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				finally
				{
					if (this.m_order == " ASC ")
					{
						this.dgvHoldingDetailInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						this.m_order = " Desc ";
					}
					else
					{
						this.dgvHoldingDetailInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
						this.m_order = " ASC ";
					}
				}
			}
		}
		private void dgvOrderInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DataView dataView = (DataView)this.dgvOrderInfo.DataSource;
				try
				{
					dataView.Sort = " AutoID ASC, " + this.dgvOrderInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				catch (Exception)
				{
					dataView.Sort = " " + this.dgvOrderInfo.Columns[e.ColumnIndex].Name.ToString() + this.m_order;
				}
				finally
				{
					if (this.m_order == " ASC ")
					{
						this.dgvOrderInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						this.m_order = " Desc ";
					}
					else
					{
						this.dgvOrderInfo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
						this.m_order = " ASC ";
					}
				}
			}
		}
		private void dgvHoldingDetailInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			lock (this.LockDIDataTable)
			{
				if ((e.ColumnIndex == this.dgvHoldingDetailInfo.Columns["FloatingPrice"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total) || (e.ColumnIndex == this.dgvHoldingDetailInfo.Columns["HoldingFloat"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total) || (e.ColumnIndex == this.dgvHoldingDetailInfo.Columns["TotalFloat"].Index && this.dgvHoldingDetailInfo.Rows[e.RowIndex].Cells["CommodityName"].Value.ToString() != this.Total))
				{
					double num = Convert.ToDouble(e.Value);
					if (num > 0.0)
					{
						e.CellStyle.ForeColor = Color.Red;
					}
					else if (num == 0.0)
					{
						e.CellStyle.ForeColor = Color.Black;
					}
					else
					{
						e.CellStyle.ForeColor = Color.Green;
					}
				}
			}
		}
		private void dgvFirmInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
				{
					this.dgvFirmInfo.Rows[e.RowIndex].Selected = true;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void dgvFirmInfo_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.dgvFirmInfo.ClearSelection();
				Point position = this.dgvFirmInfo.PointToClient(Cursor.Position);
				this.contextMenuStripFirmInfo.Show(this.dgvFirmInfo, position);
			}
		}
		private void SetCOF10DataGridColText()
		{
			try
			{
				this.dgvCustomerOrderF10.Columns["Commodity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_COMMODITYNAME"));
				this.dgvCustomerOrderF10.Columns["Commodity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvCustomerOrderF10.Columns["BuyAveragePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_BUYAVERAGEPRICE"));
				this.dgvCustomerOrderF10.Columns["BuyAveragePrice"].DefaultCellStyle.Format = "f2";
				this.dgvCustomerOrderF10.Columns["BuyAveragePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["BuyHoldingAmount"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_BUYHOLDINGAMOUNT"));
				this.dgvCustomerOrderF10.Columns["BuyHoldingAmount"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10.Columns["BuyHoldingAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["BuyQuantity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_BUYQUANTITY"));
				this.dgvCustomerOrderF10.Columns["BuyQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["BuyFloat"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_BUYFLOAT"));
				this.dgvCustomerOrderF10.Columns["BuyFloat"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10.Columns["BuyFloat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["SellAveragePrice"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_SELLAVERAGEPRICE"));
				this.dgvCustomerOrderF10.Columns["SellAveragePrice"].DefaultCellStyle.Format = "f2";
				this.dgvCustomerOrderF10.Columns["SellAveragePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["SellHoldingAmount"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_SELLHOLDINGAMOUNT"));
				this.dgvCustomerOrderF10.Columns["SellHoldingAmount"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10.Columns["SellHoldingAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["SellQuantity"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_SELLQUANTITY"));
				this.dgvCustomerOrderF10.Columns["SellQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["SellFloat"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_SELLFLOAT"));
				this.dgvCustomerOrderF10.Columns["SellFloat"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10.Columns["SellFloat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["JingTouCun"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_JINGTOUCUN"));
				this.dgvCustomerOrderF10.Columns["JingTouCun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["Float"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "CO_FLOAT"));
				this.dgvCustomerOrderF10.Columns["Float"].DefaultCellStyle.Format = "n2";
				this.dgvCustomerOrderF10.Columns["Float"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				this.dgvCustomerOrderF10.Columns["CommodityID"].HeaderText = "CommodityID";
				this.dgvCustomerOrderF10.Columns["CommodityID"].Visible = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void InitCustomerOrderF10()
		{
			try
			{
				this.gbCustomerOrderF10.Text = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "GB_CUSTOMERORDER"));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void DelegateLoadCOF10()
		{
			try
			{
				if (!this.UpdateCustomerOrderF10flag)
				{
					this.UpdateCustomerOrderF10flag = true;
					CustomerOrderQueryRequestVO customerOrderQueryRequestVO = new CustomerOrderQueryRequestVO();
					customerOrderQueryRequestVO.UserID = Global.UserID;
					this.callbackCustomerOrderF10DataGrid = new TMainForm.CallbackCustomerOrderF10DataGrid(this.FillCustomerOrderDataGridF10);
					ThreadPoolParameter threadPoolParameter = new ThreadPoolParameter();
					if (this.DictionarySemaphore.ContainsKey("DelegateLoadCOF10"))
					{
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCOF10"];
						threadPoolParameter.obj = customerOrderQueryRequestVO;
					}
					else
					{
						this.DictionarySemaphore.Add("DelegateLoadCOF10", new AutoResetEvent(true));
						threadPoolParameter.Semaphores = (AutoResetEvent)this.DictionarySemaphore["DelegateLoadCOF10"];
						threadPoolParameter.obj = customerOrderQueryRequestVO;
					}
					WaitCallback callBack = new WaitCallback(this.QueryCustomerOrderF10);
					ThreadPool.QueueUserWorkItem(callBack, threadPoolParameter);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void QueryCustomerOrderF10(object _CustomerOrder)
		{
			AutoResetEvent autoResetEvent = null;
			try
			{
				ThreadPoolParameter threadPoolParameter = (ThreadPoolParameter)_CustomerOrder;
				autoResetEvent = threadPoolParameter.Semaphores;
				autoResetEvent.Reset();
				CustomerOrderQueryRequestVO customerOrderQueryRequestVO = (CustomerOrderQueryRequestVO)threadPoolParameter.obj;
				lock (this.LockDIDataTableF10)
				{
					this._HDIDataTableF10 = this.dataProcess.GetCustomerOrder(customerOrderQueryRequestVO);
				}
				this.HandleCreated();
				base.BeginInvoke(this.callbackCustomerOrderF10DataGrid, new object[]
				{
					this._HDIDataTableF10
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			finally
			{
				if (autoResetEvent != null)
				{
					autoResetEvent.Set();
				}
				this.UpdateCustomerOrderF10flag = false;
			}
		}
		private void FillCustomerOrderDataGridF10(DataSet CODataSet)
		{
			try
			{
				int num = -1;
				string text = string.Empty;
				ListSortDirection direction = ListSortDirection.Ascending;
				if (this.dgvCustomerOrderF10.SelectedCells.Count != 0)
				{
					num = this.dgvCustomerOrderF10.SelectedCells[0].RowIndex;
				}
				if (this.dgvCustomerOrderF10.SortedColumn != null)
				{
					text = this.dgvCustomerOrderF10.SortedColumn.Name;
					SortOrder sortOrder = this.dgvCustomerOrderF10.SortOrder;
					if (sortOrder == SortOrder.Ascending)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						direction = ListSortDirection.Descending;
					}
				}
				Logger.wirte(1, "FillCustomerOrderDataGridF10线程启动 1");
				if (CODataSet != null)
				{
					DataView dataView = new DataView(CODataSet.Tables["CODetatable"]);
					SystemStatus currentSystemStatus;
					lock (this._CurrentSystemStatusObject)
					{
						currentSystemStatus = this._CurrentSystemStatus;
					}
					if (currentSystemStatus != SystemStatus.SettlementComplete)
					{
						this.UpdateCustomerOrderF10();
					}
					Logger.wirte(1, "FillCustomerOrderDataGridF10线程2");
					this.dgvCustomerOrderF10.DataSource = dataView.Table;
				}
				Logger.wirte(1, "FillCustomerOrderDataGridF10线程3");
				this.SetCOF10DataGridColText();
				Logger.wirte(1, "FillCustomerOrderDataGridF10线程4");
				if (this.dgvCustomerOrderF10.Rows.Count != 0)
				{
					if (text != string.Empty)
					{
						this.dgvCustomerOrderF10.Sort(this.dgvCustomerOrderF10.Columns[text], direction);
					}
					if (num > -1 && num < this.dgvCustomerOrderF10.Rows.Count)
					{
						this.dgvCustomerOrderF10.CurrentCell = this.dgvCustomerOrderF10.Rows[num].Cells[0];
					}
					else
					{
						this.dgvCustomerOrderF10.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			Logger.wirte(1, "FillCustomerOrderDataGridF10线程完成");
		}
		private void dgvCustomerOrderF10_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					this.dgvCustomerOrderF10.ClearSelection();
					Point position = this.dgvCustomerOrderF10.PointToClient(Cursor.Position);
					this.SetMenuDisenable("contextMenuStripF7");
					this.contextMenuStripF7.Show(this.dgvCustomerOrderF10, position);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdateCustomerOrderF10HQ()
		{
			this.callbackUpdateCustomerOrderF10HQ = new TMainForm.CallbackUpdateCustomerOrderF10HQ(this.InvokeUpdateCustomerOrderF10HQ);
			this.HandleCreated();
			base.BeginInvoke(this.callbackUpdateCustomerOrderF10HQ, new object[0]);
		}
		private void InvokeUpdateCustomerOrderF10HQ()
		{
			try
			{
				this.UpdateCustomerOrderF10();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void UpdateCustomerOrderF10()
		{
			try
			{
				Dictionary<string, CommData> dictionary = null;
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData == null)
					{
						return;
					}
					dictionary = Global.gHQCommData;
				}
				lock (this.LockDIDataTableF10)
				{
					if (this._HDIDataTableF10 != null && this._HDIDataTableF10.Tables[0] != null)
					{
						lock (this._CustomerOrderHashtable)
						{
							if (this._CustomerOrderHashtable == null)
							{
								this._CustomerOrderHashtable = new Hashtable();
							}
							else
							{
								this._CustomerOrderHashtable.Clear();
							}
							double num = 0.0;
							foreach (DataRow dataRow in this._HDIDataTableF10.Tables[0].Rows)
							{
								string key = dataRow["CommodityID"].ToString();
								string s = dataRow["BuyHoldingAmount"].ToString();
								string s2 = dataRow["BuyQuantity"].ToString();
								string s3 = dataRow["SellHoldingAmount"].ToString();
								string s4 = dataRow["SellQuantity"].ToString();
								double num2 = 0.0;
								long num3 = 0L;
								double num4 = 0.0;
								long num5 = 0L;
								bool flag = double.TryParse(s, out num2);
								bool flag2 = long.TryParse(s2, out num3);
								double.TryParse(s3, out num4);
								long.TryParse(s4, out num5);
								if (dictionary != null && dictionary.ContainsKey(key) && Global.CommodityData != null && Global.CommodityData.ContainsKey(key))
								{
									double customerSellPrice = dictionary[key].CustomerSellPrice;
									double customerBuyPrice = dictionary[key].CustomerBuyPrice;
									if (flag && flag2)
									{
										double ctrtSize = Global.CommodityData[key].CtrtSize;
										double num6 = customerSellPrice * (double)num3 * ctrtSize - num2;
										double num7 = num4 - customerBuyPrice * (double)num5 * ctrtSize;
										double num8 = num6 + num7;
										dataRow["BuyFloat"] = num6.ToString("n2");
										dataRow["SellFloat"] = num7.ToString("n2");
										dataRow["Float"] = num8.ToString("n2");
										num += num8;
										if (!this._CustomerOrderHashtable.ContainsKey(key))
										{
											this._CustomerOrderHashtable.Add(key, num8);
										}
									}
								}
							}
							this.customerFloatingPT = num;
						}
					}
				}
				this.UpdateMemberFundPrice();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
	}
}
