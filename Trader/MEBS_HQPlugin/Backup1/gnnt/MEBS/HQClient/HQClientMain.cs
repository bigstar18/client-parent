// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.HQClientMain
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.HQThread;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.OutInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class HQClientMain : IDisposable
  {
    public GlobalData globalData = new GlobalData();
    public Hashtable m_htMarketData = new Hashtable();
    public Hashtable m_htRegion = new Hashtable();
    public Hashtable m_htIndustry = new Hashtable();
    public string m_Version = string.Empty;
    public int maxConnectionCount = 10;
    public int oldPage = -1;
    private int iCurrentPage = -1;
    public bool isFirstVersion = true;
    public string m_strIndicator = "ORDER";
    public int iShowBuySellPrice = 3;
    public int m_iKLineCycle = 1;
    public int m_bShowIndexAtBottom = 1;
    public string indexMainCode = "";
    public int m_iMinLineInterval = 60;
    public ArrayList m_codeList = ArrayList.Synchronized(new ArrayList());
    public ArrayList hm_codeList = ArrayList.Synchronized(new ArrayList());
    public ArrayList nm_codeList = ArrayList.Synchronized(new ArrayList());
    public Hashtable m_htProduct = new Hashtable();
    public ProductDataVO[] m_quoteList = new ProductDataVO[0];
    public Hashtable m_MarketDate = new Hashtable();
    private int m_iPrecisionDecimal = 2;
    public string m_commodityInfoAddress = string.Empty;
    public List<ProductData> listData = new List<ProductData>();
    public int rectRol = 1;
    public int rectCol = 1;
    public bool isNeedAskData = true;
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
    public int m_iDate;
    public int m_iTime;
    public int connectionCount;
    public int isChangePage;
    public CommodityInfo curCommodityInfo;
    public int m_bShowIndexKLine;
    public bool isResendVersion;
    public int m_iCodeDate;
    public int m_iCodeTime;
    public ArrayList aProductData;
    private int m_iPrecision;
    public int m_iPrecisionIndex;
    public bool isClickMultiMarket;
    public List<clickRect> listRectInfo;
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

    public int CurrentPage
    {
      get
      {
        return this.iCurrentPage;
      }
      set
      {
        if (this.iCurrentPage != value)
          this.oldPage = this.iCurrentPage;
        this.isChangePage = 1;
        this.iCurrentPage = value;
        if (this.iCurrentPage == 2)
        {
          if (this.setPictureEnable != null)
            this.setPictureEnable(true);
        }
        else if (this.setPictureEnable != null)
          this.setPictureEnable(false);
        if (this.sendThread == null)
          return;
        if (this.iCurrentPage != 4)
          this.connectionCount = 0;
        this.sendThread.AskForData((CMDVO) new CMDSetCurPage()
        {
          curPage = this.iCurrentPage
        });
      }
    }

    public TradeTimeVO[] TimeRange
    {
      get
      {
        if (this.curCommodityInfo == null || this.curCommodityInfo.marketID == null || this.curCommodityInfo.marketID.Length == 0)
          return (TradeTimeVO[]) null;
        if (this.m_htMarketData == null || this.m_htMarketData[(object) this.curCommodityInfo.marketID] == null)
          return (TradeTimeVO[]) null;
        return ((MarketDataVO) this.m_htMarketData[(object) this.curCommodityInfo.marketID]).m_timeRange;
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
      HQClientForm hqClientForm = hqForm as HQClientForm;
      this.pluginInfo = hqClientForm.pluginInfo;
      this.setInfo = hqClientForm.setInfo;
      this.setInfo.init(this.pluginInfo.ConfigPath);
      this.buttonUtils = hqClientForm.buttonUtils;
      this.multiQuoteData = hqClientForm.multiQuoteData;
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
      this.stockM_htItemInfo.Add((object) "Newly", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_Newly"));
      this.stockM_htItemInfo.Add((object) "ChangeValue", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"));
      this.stockM_htItemInfo.Add((object) "Open", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_Open"));
      this.stockM_htItemInfo.Add((object) "ChangeRate", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeRate"));
      this.stockM_htItemInfo.Add((object) "High", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_High"));
      this.stockM_htItemInfo.Add((object) "CurVol", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"));
      this.stockM_htItemInfo.Add((object) "Low", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_Low"));
      this.stockM_htItemInfo.Add((object) "TotalVolume", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_TotalVolume"));
      this.stockM_htItemInfo.Add((object) "VolRate", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_VolRate"));
      this.stockM_htItemInfo.Add((object) "Balance", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"));
      this.stockM_htItemInfo.Add((object) "PreBalance", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_PreBalance1"));
      this.stockM_htItemInfo.Add((object) "Order", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_Order1"));
      this.stockM_htItemInfo.Add((object) "OrderChange", (object) this.pluginInfo.HQResourceManager.GetString("HQStr_OrderChange"));
      this.stockM_htItemInfo.Add((object) "cje", (object) "");
      this.stockM_htItemInfo.Add((object) "hsl", (object) "");
      this.stockM_htItemInfo.Add((object) "zs", (object) "");
      string str1 = this.setInfo.StockMarketName;
      char[] chArray1 = new char[1]
      {
        ';'
      };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]
        {
          ':'
        };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 2 && strArray[1].Length > 0)
          this.stockM_htItemInfo[(object) strArray[0]] = (object) strArray[1];
      }
      this.stockM_strItems = this.setInfo.StockMarketItems.Split(';');
      this.ABVOLS = this.setInfo.ABVOLS;
    }

    private void initAllItem()
    {
      this.strAllItemName = "No;MarketName;Name;Code;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;UpRate;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;TotalMoney;AmountRate;ConsignRate;Unit;Industry;Region;";
      this.strDefaultItem = "No;MarketName;Name;Code;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;Industry;Region;";
      if (!Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
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
      this.m_htItemInfo.Add((object) "No", (object) new MultiQuoteItemInfo("", 20, -1));
      this.m_htItemInfo.Add((object) "MarketName", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_MarketName"), 80, -1));
      this.m_htItemInfo.Add((object) "Name", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Name"), 80, -1));
      this.m_htItemInfo.Add((object) "Code", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Code"), 64, 0));
      this.m_htItemInfo.Add((object) "CurPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Newly"), 60, 1));
      this.m_htItemInfo.Add((object) "CurAmount", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"), 60, -1));
      this.m_htItemInfo.Add((object) "SellPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_SellPrice"), 60, -1));
      this.m_htItemInfo.Add((object) "SellAmount", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_SellVol"), 48, -1));
      this.m_htItemInfo.Add((object) "BuyPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_BuyPrice"), 60, -1));
      this.m_htItemInfo.Add((object) "BuyAmount", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_BuyVol"), 48, -1));
      this.m_htItemInfo.Add((object) "TotalAmount", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), 75, 9));
      this.m_htItemInfo.Add((object) "UpValue", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"), 40, 2));
      this.m_htItemInfo.Add((object) "UpRate", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeRate"), 65, 3));
      this.m_htItemInfo.Add((object) "ReverseCount", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), 70, -1));
      this.m_htItemInfo.Add((object) "Balance", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), 60, -1));
      this.m_htItemInfo.Add((object) "OpenPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Open"), 55, -1));
      this.m_htItemInfo.Add((object) "HighPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_High"), 55, -1));
      this.m_htItemInfo.Add((object) "LowPrice", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Low"), 55, -1));
      this.m_htItemInfo.Add((object) "Industry", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Industry"), 55, -1));
      this.m_htItemInfo.Add((object) "Region", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Region"), 55, -1));
      this.m_htItemInfo.Add((object) "YesterBalance", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_PreBalance"), 60, -1));
      this.m_htItemInfo.Add((object) "TotalMoney", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Money"), 80, 6));
      this.m_htItemInfo.Add((object) "AmountRate", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_VolRate"), 50, 5));
      this.m_htItemInfo.Add((object) "ConsignRate", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), 50, 7));
      this.m_htItemInfo.Add((object) "Unit", (object) new MultiQuoteItemInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Unit"), 40, -1));
    }

    private void jbInit()
    {
      this.strURLPath = "http://" + (object) this.pluginInfo.IPAddress + ":" + (string) (object) this.pluginInfo.HttpPort + "/hqApplet/";
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
      bool flag = false;
      try
      {
        foreach (ProductDataVO productDataVo in this.m_quoteList)
        {
          if (productDataVo.code == this.m_hqForm.CurHQClient.curCommodityInfo.commodityCode)
          {
            flag = true;
            break;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return flag;
    }

    public ProductData GetProductData(CommodityInfo commodityInfo)
    {
      for (int index = 0; index < this.aProductData.Count; ++index)
      {
        if (((ProductData) this.aProductData[index]).commodityInfo.Compare(commodityInfo))
          return (ProductData) this.aProductData[index];
      }
      return (ProductData) null;
    }

    public ProductData GetProductData(string marketID, string code)
    {
      return this.GetProductData(new CommodityInfo(marketID, code));
    }

    public int getProductType(CommodityInfo commodityInfo)
    {
      if (commodityInfo == null)
        return 0;
      CodeTable codeTable = (CodeTable) this.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
      if (codeTable == null)
        return -1;
      return codeTable.status;
    }

    internal int GetPrecision(CommodityInfo commodityInfo)
    {
      int num = 0;
      try
      {
        CodeTable codeTable = (CodeTable) this.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
        if (codeTable != null)
        {
          string[] strArray = codeTable.mPrice.ToString().Split('.');
          num = strArray == null || strArray.Length == 1 ? 0 : strArray[1].Length;
        }
      }
      catch (Exception ex)
      {
        WriteLog.WriteMsg("GetPrecision" + ex.Message);
      }
      return num;
    }

    public bool isIndex(CommodityInfo commodityInfo)
    {
      int productType = this.getProductType(commodityInfo);
      if (productType != 2)
        return productType == 3;
      return true;
    }

    public void Exit()
    {
      if (this.httpThread != null)
        this.httpThread.Dispose();
      if (this.receiveThread != null)
        this.receiveThread.Dispose();
      if (this.sendThread != null)
        this.sendThread.Dispose();
      if (this.m_socket == null || !this.m_socket.Connected)
        return;
      this.m_socket.Close();
    }

    public void Dispose()
    {
      this.Exit();
      GC.SuppressFinalize((object) this);
    }

    public delegate void KLineUpDownCallBack(int iconFlag);

    public delegate void SetPictureEnableCallback(bool enable);

    public delegate void CreateIndicatorCallback();
  }
}
