using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.HQThread;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.OutInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class HQClientMain : IDisposable
	{
		public delegate void KLineUpDownCallBack(int iconFlag);
		public delegate void SetPictureEnableCallback(bool enable);
		public delegate void CreateIndicatorCallback();
		public const int PAGE_MULTIQUOTE = 0;
		public const int PAGE_MINLINE = 1;
		public const int PAGE_KLINE = 2;
		public const int PAGE_BILL = 4;
		public const int PAGE_MARKETSTATUS = 5;
		public const int PAGE_HISTORY = 6;
		public HQForm m_hqForm;
		public PluginInfo pluginInfo;
		public SetInfo setInfo;
		public ButtonUtils buttonUtils;
		public MultiQuoteData multiQuoteData;
		public GlobalData globalData = new GlobalData();
		public Hashtable m_htMarketData = new Hashtable();
		public Hashtable m_htRegion = new Hashtable();
		public Hashtable m_htIndustry = new Hashtable();
		public int m_iDate;
		public int m_iTime;
		public string m_Version = string.Empty;
		public int connectionCount;
		public int maxConnectionCount = 10;
		public int isChangePage;
		public int oldPage = -1;
		private int iCurrentPage = -1;
		public bool isFirstVersion = true;
		public CommodityInfo curCommodityInfo;
		public string m_strIndicator = "ORDER";
		public int iShowBuySellPrice = 3;
		public int m_iKLineCycle = 1;
		public int m_bShowIndexKLine;
		public int m_bShowIndexAtBottom = 1;
		public string indexMainCode = "";
		public int m_iMinLineInterval = 60;
		public bool isResendVersion;
		public int m_iCodeDate;
		public int m_iCodeTime;
		public ArrayList m_codeList = ArrayList.Synchronized(new ArrayList());
		public ArrayList hm_codeList = ArrayList.Synchronized(new ArrayList());
		public ArrayList nm_codeList = ArrayList.Synchronized(new ArrayList());
		public Hashtable m_htProduct = new Hashtable();
		public ArrayList aProductData;
		public ProductDataVO[] m_quoteList = new ProductDataVO[0];
		public Hashtable m_MarketDate = new Hashtable();
		private int m_iPrecision;
		public int m_iPrecisionIndex;
		private int m_iPrecisionDecimal = 2;
		public string m_commodityInfoAddress = string.Empty;
		public bool isClickMultiMarket;
		public List<ProductData> listData = new List<ProductData>();
		public List<clickRect> listRectInfo;
		public int rectRol = 1;
		public int rectCol = 1;
		public bool Connected;
		public string strSocketIP;
		public int iSocketPort;
		public string strURLPath;
		public Socket m_socket;
		public SendThread sendThread;
		public ReceiveThread receiveThread;
		public HttpThread httpThread;
		public MyCommodity myCommodity;
		public CommodityClass commodityClass;
		private int _KLineValue;
		public HQClientMain.KLineUpDownCallBack kLineUpDown;
		public HQClientMain.SetPictureEnableCallback setPictureEnable;
		public HQClientMain.CreateIndicatorCallback createIndicator;
		public string strAllItemName;
		public Hashtable m_htItemInfo;
		public string strDefaultItem;
		public Hashtable stockM_htItemInfo;
		public string[] stockM_strItems;
		public string ABVOLS;
		public bool isNeedAskData = true;
		public int CurrentPage
		{
			get
			{
				return this.iCurrentPage;
			}
			set
			{
				if (this.iCurrentPage != value)
				{
					this.oldPage = this.iCurrentPage;
				}
				this.isChangePage = 1;
				this.iCurrentPage = value;
				if (this.iCurrentPage == 2)
				{
					if (this.setPictureEnable != null)
					{
						this.setPictureEnable(true);
					}
				}
				else
				{
					if (this.setPictureEnable != null)
					{
						this.setPictureEnable(false);
					}
				}
				if (this.sendThread != null)
				{
					if (this.iCurrentPage != 4)
					{
						this.connectionCount = 0;
					}
					CMDSetCurPage cMDSetCurPage = new CMDSetCurPage();
					cMDSetCurPage.curPage = this.iCurrentPage;
					this.sendThread.AskForData(cMDSetCurPage);
				}
			}
		}
		public TradeTimeVO[] TimeRange
		{
			get
			{
				if (this.curCommodityInfo == null || this.curCommodityInfo.marketID == null || this.curCommodityInfo.marketID.Length == 0)
				{
					return null;
				}
				if (this.m_htMarketData == null || this.m_htMarketData[this.curCommodityInfo.marketID] == null)
				{
					return null;
				}
				return ((MarketDataVO)this.m_htMarketData[this.curCommodityInfo.marketID]).m_timeRange;
			}
		}
		public int KLineValue
		{
			get
			{
				return this._KLineValue;
			}
			set
			{
				this._KLineValue = value;
			}
		}
		public HQClientMain(HQForm hqForm)
		{
			this.m_hqForm = hqForm;
			HQClientForm hQClientForm = hqForm as HQClientForm;
			this.pluginInfo = hQClientForm.pluginInfo;
			this.setInfo = hQClientForm.setInfo;
			this.setInfo.init(this.pluginInfo.ConfigPath);
			this.buttonUtils = hQClientForm.buttonUtils;
			this.multiQuoteData = hQClientForm.multiQuoteData;
           
		}
		public void init()
		{
			this.aProductData = ArrayList.Synchronized(new ArrayList());
			this.myCommodity = new MyCommodity(this.pluginInfo.ConfigPath + "MyCommodity.xml");
			this.iShowBuySellPrice = this.setInfo.ShowBuySellPrice;
			this.initAllItem();
			this.initStockMarketInfo();
			this.jbInit();
		}
		private void initStockMarketInfo()
		{
			this.stockM_htItemInfo = new Hashtable();
			this.stockM_htItemInfo.Add("Newly", this.pluginInfo.HQResourceManager.GetString("HQStr_Newly"));
			this.stockM_htItemInfo.Add("ChangeValue", this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"));
			this.stockM_htItemInfo.Add("Open", this.pluginInfo.HQResourceManager.GetString("HQStr_Open"));
			this.stockM_htItemInfo.Add("ChangeRate", this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeRate"));
			this.stockM_htItemInfo.Add("High", this.pluginInfo.HQResourceManager.GetString("HQStr_High"));
			this.stockM_htItemInfo.Add("CurVol", this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"));
			this.stockM_htItemInfo.Add("Low", this.pluginInfo.HQResourceManager.GetString("HQStr_Low"));
			this.stockM_htItemInfo.Add("TotalVolume", this.pluginInfo.HQResourceManager.GetString("HQStr_TotalVolume"));
			this.stockM_htItemInfo.Add("VolRate", this.pluginInfo.HQResourceManager.GetString("HQStr_VolRate"));
			this.stockM_htItemInfo.Add("Balance", this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"));
			this.stockM_htItemInfo.Add("PreBalance", this.pluginInfo.HQResourceManager.GetString("HQStr_PreBalance1"));
			this.stockM_htItemInfo.Add("Order", this.pluginInfo.HQResourceManager.GetString("HQStr_Order1"));
			this.stockM_htItemInfo.Add("OrderChange", this.pluginInfo.HQResourceManager.GetString("HQStr_OrderChange"));
			this.stockM_htItemInfo.Add("cje", "");
			this.stockM_htItemInfo.Add("hsl", "");
			this.stockM_htItemInfo.Add("zs", "");
			string stockMarketName = this.setInfo.StockMarketName;
			string[] array = stockMarketName.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				if (array2.Length == 2 && array2[1].Length > 0)
				{
					this.stockM_htItemInfo[array2[0]] = array2[1];
				}
			}
			string stockMarketItems = this.setInfo.StockMarketItems;
			this.stockM_strItems = stockMarketItems.Split(new char[]
			{
				';'
			});
			this.ABVOLS = this.setInfo.ABVOLS;
		}
		private void initAllItem()
		{
			this.strAllItemName = "No;MarketName;Name;Code;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;UpRate;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;TotalMoney;AmountRate;ConsignRate;Unit;Industry;Region;";
			this.strDefaultItem = "No;MarketName;Name;Code;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;Industry;Region;";
			if (!Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
			{
				this.strAllItemName = this.strAllItemName.Replace("Industry;", "");
				this.strAllItemName = this.strAllItemName.Replace("Region;", "");
				this.strDefaultItem = this.strDefaultItem.Replace("Industry;", "");
				this.strDefaultItem = this.strDefaultItem.Replace("Region;", "");
				this.strAllItemName = this.strAllItemName.Replace("MarketName;", "");
				this.strDefaultItem = this.strDefaultItem.Replace("MarketName;", "");
			}
			this.strDefaultItem = this.strDefaultItem.Replace("Unit;", "");
			this.strAllItemName = this.strAllItemName.Replace("Unit;", "");
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("No", new MultiQuoteItemInfo("", 20, -1));
			this.m_htItemInfo.Add("MarketName", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_MarketName"), 80, -1));
			this.m_htItemInfo.Add("Name", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Name"), 80, -1));
			this.m_htItemInfo.Add("Code", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Code"), 64, 0));
			this.m_htItemInfo.Add("CurPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Newly"), 60, 1));
			this.m_htItemInfo.Add("CurAmount", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"), 60, -1));
			this.m_htItemInfo.Add("SellPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_SellPrice"), 60, -1));
			this.m_htItemInfo.Add("SellAmount", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_SellVol"), 48, -1));
			this.m_htItemInfo.Add("BuyPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_BuyPrice"), 60, -1));
			this.m_htItemInfo.Add("BuyAmount", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_BuyVol"), 48, -1));
			this.m_htItemInfo.Add("TotalAmount", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), 75, 9));
			this.m_htItemInfo.Add("UpValue", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"), 40, 2));
			this.m_htItemInfo.Add("UpRate", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeRate"), 65, 3));
			this.m_htItemInfo.Add("ReverseCount", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), 70, -1));
			this.m_htItemInfo.Add("Balance", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), 60, -1));
			this.m_htItemInfo.Add("OpenPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Open"), 55, -1));
			this.m_htItemInfo.Add("HighPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_High"), 55, -1));
			this.m_htItemInfo.Add("LowPrice", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Low"), 55, -1));
			this.m_htItemInfo.Add("Industry", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Industry"), 55, -1));
			this.m_htItemInfo.Add("Region", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Region"), 55, -1));
			this.m_htItemInfo.Add("YesterBalance", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_PreBalance"), 60, -1));
			this.m_htItemInfo.Add("TotalMoney", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Money"), 80, 6));
			this.m_htItemInfo.Add("AmountRate", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_VolRate"), 50, 5));
			this.m_htItemInfo.Add("ConsignRate", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), 50, 7));
			this.m_htItemInfo.Add("Unit", new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Unit"), 40, -1));
		}
		private void jbInit()
		{
			this.strURLPath = string.Concat(new object[]
			{
				"http://",
				this.pluginInfo.IPAddress,
				":",
				this.pluginInfo.HttpPort,
				"/hqApplet/"
			});
			this.strSocketIP = this.pluginInfo.IPAddress;
			this.iSocketPort = this.pluginInfo.Port;
          
			new HttpThread(0, this).Start();
			new HttpThread(2, this).Start();
			this.httpThread = new HttpThread(1, this);
			this.sendThread = new SendThread(this);
			this.receiveThread = new ReceiveThread(this);
			try
			{
				this.httpThread.Start();
				this.sendThread.Start();
				this.receiveThread.Start();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.ToString());
			}
		}
		public bool IsIncludeCurrentComm()
		{
			bool result = false;
			try
			{
				ProductDataVO[] quoteList = this.m_quoteList;
				for (int i = 0; i < quoteList.Length; i++)
				{
					ProductDataVO productDataVO = quoteList[i];
					if (productDataVO.code == this.m_hqForm.CurHQClient.curCommodityInfo.commodityCode)
					{
						result = true;
						break;
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
		public ProductData GetProductData(CommodityInfo commodityInfo)
		{
			for (int i = 0; i < this.aProductData.Count; i++)
			{
				ProductData productData = (ProductData)this.aProductData[i];
				if (productData.commodityInfo.Compare(commodityInfo))
				{
					return (ProductData)this.aProductData[i];
				}
			}
			return null;
		}
		public ProductData GetProductData(string marketID, string code)
		{
			CommodityInfo commodityInfo = new CommodityInfo(marketID, code);
			return this.GetProductData(commodityInfo);
		}
		public int getProductType(CommodityInfo commodityInfo)
		{
			if (commodityInfo == null)
			{
				return 0;
			}
			CodeTable codeTable = (CodeTable)this.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
			if (codeTable == null)
			{
				return -1;
			}
			return codeTable.status;
		}
		internal int GetPrecision(CommodityInfo commodityInfo)
		{
			int result = 0;
			try
			{
				CodeTable codeTable = (CodeTable)this.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
				if (codeTable != null)
				{
					float mPrice = codeTable.mPrice;
					string[] array = mPrice.ToString().Split(new char[]
					{
						'.'
					});
					if (array == null || array.Length == 1)
					{
						result = 0;
					}
					else
					{
						result = array[1].Length;
					}
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("GetPrecision" + ex.Message);
			}
			return result;
		}
		public bool isIndex(CommodityInfo commodityInfo)
		{
			int productType = this.getProductType(commodityInfo);
			return productType == 2 || productType == 3;
		}
		public void Exit()
		{
			if (this.httpThread != null)
			{
				this.httpThread.Dispose();
			}
			if (this.receiveThread != null)
			{
				this.receiveThread.Dispose();
			}
			if (this.sendThread != null)
			{
				this.sendThread.Dispose();
			}
			if (this.m_socket != null && this.m_socket.Connected)
			{
				this.m_socket.Close();
			}
		}
		public void Dispose()
		{
			this.Exit();
			GC.SuppressFinalize(this);
		}
        public void GetIP()
        {
            string MyIPAdress = strSocketIP + "[" + iSocketPort + "]";
            if (!File.Exists("./HQIPAddress.txt"))
            {
                FileStream fs1 = new FileStream("./HQIPAddress.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write(MyIPAdress);//开始写入值
                sw.Close();
                fs1.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter("./HQIPAddress.txt");
                sw.Write(MyIPAdress);
                sw.Close();
            }
        }
	}
}
