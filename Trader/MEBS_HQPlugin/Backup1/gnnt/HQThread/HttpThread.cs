// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.HQThread.HttpThread
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
  public class HttpThread : MYThread
  {
    private ManualResetEvent Event = new ManualResetEvent(true);
    public const int TYPE_CODELIST = 0;
    public const int TYPE_OTHER = 1;
    public const int TYPE_XMLClass = 2;
    private HQClientMain m_client;
    private HQForm m_hqForm;
    private int iType;
    private ArrayList aPacket;
    public bool isEndAddClassBtn;
    public bool isEndAddMarketBtn;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ButtonUtils buttonUtils;
    private int p;
    private HQClientMain hQClientMain;

    public HttpThread(int type, HQClientMain client)
    {
      this.threadName = "HTTP线程" + (object) type;
      this.m_client = client;
      this.m_hqForm = this.m_client.m_hqForm;
      this.pluginInfo = this.m_client.pluginInfo;
      this.setInfo = this.m_client.setInfo;
      this.buttonUtils = client.buttonUtils;
      this.iType = type;
      if (this.iType != 1)
        return;
      this.aPacket = ArrayList.Synchronized(new ArrayList());
    }

    public void AskForData(Packet_HttpRequest packet)
    {
      lock (this)
      {
        if (packet != null)
        {
          this.aPacket.Add((object) packet);
          int local_0 = this.aPacket.Count;
          if (local_0 > 1)
          {
            for (int local_1 = 0; local_1 < local_0 - 1; ++local_1)
              this.aPacket.RemoveAt(local_1);
          }
        }
        this.Event.Set();
      }
    }

    protected override void run()
    {
      try
      {
        if (this.iType == 0)
        {
          this.GetCodeList();
          Logger.wirte(MsgType.Information, "**********" + this.threadName + "取码表结束！！！**********");
        }
        else if (2 == this.iType)
        {
          if (Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
          {
            this.GetMarketList();
            Logger.wirte(MsgType.Information, "**********" + this.threadName + "取市场列表结束！！！**********");
          }
          else
          {
            this.GetXMLClass();
            Logger.wirte(MsgType.Information, "**********" + this.threadName + "取商品分类信息结束！！！**********");
          }
        }
        else
        {
          this.GetHttpData();
          Logger.wirte(MsgType.Information, "**********" + this.threadName + "结束！！！**********");
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "HttpThread-run异常：" + ex.Message);
      }
    }

    protected override void disposeThread()
    {
      this.Event.Set();
    }

    public void GetCodeList()
    {
      bool flag = false;
      while (this.m_client != null && !this.blnIsStopped && !flag)
      {
        this.mUnique.WaitOne();
        this.mUnique.ReleaseMutex();
        try
        {
          ProductInfoListVO productInfoList = RequestUtil.getProductInfoList(this.m_client.strURLPath + "data/productinfo.dat");
          Logger.wirte(MsgType.Information, string.Concat(new object[4]
          {
            (object) "码表时间:",
            (object) productInfoList.date,
            (object) " ",
            (object) productInfoList.time
          }));
          this.m_client.m_iCodeDate = productInfoList.date;
          this.m_client.m_iCodeTime = productInfoList.time;
          ProductInfoVO[] productInfoVoArray = productInfoList.productInfos;
          this.m_client.m_codeList.Clear();
          this.m_client.m_htProduct.Clear();
          for (int index = 0; index < productInfoVoArray.Length; ++index)
          {
            CodeTable codeTable = new CodeTable();
            codeTable.marketID = productInfoVoArray[index].marketID;
            codeTable.sName = productInfoVoArray[index].name;
            string str = (string) this.pluginInfo.HTConfig[(object) ("Market" + codeTable.marketID)];
            if (str == null || str.Length == 0)
              str = codeTable.marketID;
            if (this.m_hqForm.AddMarketName)
              codeTable.sName = str + codeTable.sName;
            codeTable.sPinyin = productInfoVoArray[index].pyName;
            codeTable.status = productInfoVoArray[index].status;
            codeTable.tradeSecNo = productInfoVoArray[index].tradeSecNo;
            codeTable.mPrice = productInfoVoArray[index].mPrice;
            codeTable.fUnit = productInfoVoArray[index].fUnit;
            if (this.m_client.m_htProduct != null && !this.m_client.m_htProduct.ContainsKey((object) (productInfoVoArray[index].marketID + productInfoVoArray[index].code)))
            {
              this.m_client.m_htProduct.Add((object) (productInfoVoArray[index].marketID + productInfoVoArray[index].code), (object) codeTable);
              CommodityInfo commodityInfo = new CommodityInfo(productInfoVoArray[index].marketID, productInfoVoArray[index].code, productInfoVoArray[index].region, productInfoVoArray[index].industry);
              this.m_client.m_codeList.Add((object) commodityInfo);
              if (productInfoVoArray[index].status == 1)
                this.m_client.hm_codeList.Add((object) commodityInfo);
              else
                this.m_client.nm_codeList.Add((object) commodityInfo);
            }
            else
              Logger.wirte(MsgType.Warning, "HTTP取码表出现重复数据，将出现Hashtable重复键加入！商品代码" + productInfoVoArray[index].code);
            if (codeTable.status == 3 && this.m_client.indexMainCode.Length == 0)
              this.m_client.indexMainCode = productInfoVoArray[index].marketID + "_" + productInfoVoArray[index].code;
          }
          flag = true;
          this.m_hqForm.ReMakeIndexMenu();
          this.m_hqForm.Repaint();
        }
        catch (Exception ex)
        {
          Logger.wirte(MsgType.Error, ex.ToString());
        }
        if (!flag)
        {
          try
          {
            Thread.Sleep(1000);
          }
          catch (Exception ex)
          {
            Logger.wirte(MsgType.Error, ex.ToString());
          }
        }
      }
    }

    private void GetMarketList()
    {
      bool flag = false;
      while (this.m_client != null && !this.blnIsStopped && !flag)
      {
        this.mUnique.WaitOne();
        this.mUnique.ReleaseMutex();
        try
        {
          MarketInfoVO[] marketInfoVoArray = RequestUtil.getMarketInfoList(this.m_client.strURLPath + "data/marketinfo.dat").marketInfos;
          for (int index = 0; index < marketInfoVoArray.Length; ++index)
          {
            if (this.m_client.m_htMarketData.ContainsKey((object) marketInfoVoArray[index].marketID))
              ((MarketDataVO) this.m_client.m_htMarketData[(object) marketInfoVoArray[index].marketID]).marketName = marketInfoVoArray[index].marketName;
            else
              this.m_client.m_htMarketData.Add((object) marketInfoVoArray[index].marketID, (object) new MarketDataVO()
              {
                marketID = marketInfoVoArray[index].marketID,
                marketName = marketInfoVoArray[index].marketName
              });
          }
          flag = true;
          this.RemakeButton();
        }
        catch (Exception ex)
        {
          Logger.wirte(MsgType.Error, ex.ToString());
        }
        if (!flag)
        {
          try
          {
            Thread.Sleep(1000);
          }
          catch (Exception ex)
          {
            Logger.wirte(MsgType.Error, ex.ToString());
          }
        }
      }
    }

    private void GetXMLClass()
    {
      bool flag = false;
      while (this.m_client != null && !this.blnIsStopped && !flag)
      {
        this.mUnique.WaitOne();
        this.mUnique.ReleaseMutex();
        try
        {
          this.m_client.commodityClass = new CommodityClass((Stream) new MemoryStream(RequestUtil.getRepoent(this.m_client.strURLPath + "data/CreateXMLClass.xml")));
          flag = true;
          this.RemakeButton();
        }
        catch (Exception ex)
        {
          Logger.wirte(MsgType.Error, "下载市场分类XML文件" + ex.Message);
          this.blnIsStopped = true;
        }
        if (!flag)
        {
          try
          {
            Thread.Sleep(1000);
          }
          catch (Exception ex)
          {
            Logger.wirte(MsgType.Error, ex.ToString());
          }
        }
      }
    }

    public void RemakeButton()
    {
      try
      {
        while (this.m_hqForm.MainGraph == null)
          Thread.Sleep(100);
        if (Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
          this.AddMarketButton();
        else
          this.AddClassButton();
        ++this.buttonUtils.isTidyBtnFlag;
        if (this.buttonUtils.isTidyBtnFlag == 2)
          this.buttonUtils.TidyButtons(this.buttonUtils.ButtonList);
        if (this.m_client.CurrentPage != 0)
          return;
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "RemakeButton异常：" + ex.Message);
      }
    }

    public void AddMarketButton()
    {
      if (this.isEndAddMarketBtn || this.m_client.m_htMarketData == null)
        return;
      bool Selected = false;
      foreach (DictionaryEntry dictionaryEntry in this.m_client.m_htMarketData)
      {
        MarketDataVO marketDataVo = (MarketDataVO) dictionaryEntry.Value;
        if (this.buttonUtils.InitialButtonName.StartsWith("Market") && this.buttonUtils.InitialButtonName.Substring(6).Equals(dictionaryEntry.Key.ToString()))
          Selected = true;
        this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count, new MyButton("Market" + dictionaryEntry.Key.ToString(), marketDataVo.marketName, Selected), this.buttonUtils.ButtonList);
        Selected = false;
      }
      this.isEndAddMarketBtn = true;
    }

    public void AddClassButton()
    {
      if (this.isEndAddClassBtn || this.m_client.commodityClass == null)
        return;
      for (int index = 0; index < this.m_client.commodityClass.classList.Count; ++index)
      {
        ClassVO classVo = (ClassVO) this.m_client.commodityClass.classList[index];
        if (this.buttonUtils.InitialButtonName.Equals("C" + classVo.classID))
        {
          if (this.buttonUtils.ButtonList.Count > 0)
            this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count - 1, new MyButton("C" + classVo.classID, classVo.name, true), this.buttonUtils.ButtonList);
          else
            this.buttonUtils.InsertButton(0, new MyButton("C" + classVo.classID, classVo.name, true), this.buttonUtils.ButtonList);
        }
        else if (this.buttonUtils.ButtonList.Count > 0)
          this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count - 1, new MyButton("C" + classVo.classID, classVo.name, false), this.buttonUtils.ButtonList);
        else
          this.buttonUtils.InsertButton(0, new MyButton("C" + classVo.classID, classVo.name, false), this.buttonUtils.ButtonList);
      }
      this.isEndAddClassBtn = true;
    }

    private void GetHttpData()
    {
      while (this.m_client != null)
      {
        if (this.blnIsStopped)
          break;
        try
        {
          Thread.Sleep(300);
        }
        catch (Exception ex)
        {
          Logger.wirte(MsgType.Error, ex.ToString());
        }
        this.mUnique.WaitOne();
        this.mUnique.ReleaseMutex();
        this.Event.WaitOne();
        int count = this.aPacket.Count;
        if (count > 0)
        {
          Packet_HttpRequest request = (Packet_HttpRequest) this.aPacket[count - 1];
          this.aPacket.RemoveAt(count - 1);
          switch (request.type)
          {
            case (byte) 0:
              this.GetDayLine(request);
              continue;
            case (byte) 1:
              this.Get5MinLine(request);
              continue;
            case (byte) 2:
              this.Get1MinLine(request);
              continue;
            default:
              continue;
          }
        }
        else
          this.Event.Reset();
      }
    }

    public static KLineData[] GetHistoryData(string url)
    {
      try
      {
        MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
        GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress, true);
        BinaryReader input = new BinaryReader((Stream) gzipStream);
        InputStreamConvert inputStreamConvert = new InputStreamConvert(input);
        KLineData[] klineDataArray = new KLineData[inputStreamConvert.ReadJavaInt()];
        for (int index = 0; index < klineDataArray.Length; ++index)
        {
          klineDataArray[index] = new KLineData();
          int num = inputStreamConvert.ReadJavaInt();
          klineDataArray[index].date = string.Concat((object) num).Length <= 6 ? (long) (num + 19970000) : 199700000000L + (long) num;
          klineDataArray[index].openPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].highPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].lowPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].closePrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].balancePrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].totalAmount = inputStreamConvert.ReadJavaLong();
          klineDataArray[index].totalMoney = (double) inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].reserveCount = inputStreamConvert.ReadJavaInt();
        }
        input.Close();
        gzipStream.Close();
        memoryStream.Close();
        return klineDataArray;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "GetHistoryData异常：" + ex.Message);
        return (KLineData[]) null;
      }
    }

    public static KLineData[] GetLocalHistoryData(string url)
    {
      try
      {
        BinaryReader input = new BinaryReader((Stream) new FileStream(url, FileMode.Open));
        InputStreamConvert inputStreamConvert = new InputStreamConvert(input);
        KLineData[] klineDataArray = new KLineData[inputStreamConvert.ReadJavaInt()];
        for (int index = 0; index < klineDataArray.Length; ++index)
        {
          klineDataArray[index] = new KLineData();
          int num = inputStreamConvert.ReadJavaInt();
          klineDataArray[index].date = string.Concat((object) num).Length <= 6 ? (long) (num + 19970000) : 199700000000L + (long) num;
          klineDataArray[index].openPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].highPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].lowPrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].closePrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].balancePrice = inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].totalAmount = inputStreamConvert.ReadJavaLong();
          klineDataArray[index].totalMoney = (double) inputStreamConvert.ReadJavaFloat();
          klineDataArray[index].reserveCount = inputStreamConvert.ReadJavaInt();
        }
        input.Close();
        return klineDataArray;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "GetLocalHistoryData异常：" + ex.Message);
        return (KLineData[]) null;
      }
    }

    private void GetDayLine(Packet_HttpRequest request)
    {
      try
      {
        bool flag = false;
        string StrUrl = this.m_client.strURLPath + "data/day/" + request.marketID + request.sCode.Trim() + ".day.zip";
        string str = this.pluginInfo.ConfigPath + "data/day/" + request.marketID + "/" + request.marketID + request.sCode.Trim() + ".day.zip";
        if (!Directory.Exists(str.Remove(str.LastIndexOf('/'))))
          Directory.CreateDirectory(str.Remove(str.LastIndexOf('/')));
        string path = str.Remove(str.LastIndexOf('.'));
        if (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTime(path).Day != DateTime.Now.Day || System.IO.File.GetLastWriteTime(path).Month != DateTime.Now.Month)
        {
          Logger.wirte(MsgType.Information, "Get Day : " + StrUrl);
          if (this.DownloadFile(StrUrl, str) && this.UnZipFile(str))
            flag = true;
        }
        else
          flag = true;
        if (!flag)
          return;
        KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(str.Remove(str.LastIndexOf('.')));
        ProductData productData = this.m_client.GetProductData(request.marketID, request.sCode);
        if (productData == null)
        {
          if (this.m_client.aProductData.Count > 50)
            this.m_client.aProductData.RemoveAt(50);
          this.m_client.aProductData.Insert(0, (object) new ProductData()
          {
            commodityInfo = new CommodityInfo(request.marketID, request.sCode),
            dayKLine = localHistoryData
          });
        }
        else
          productData.dayKLine = localHistoryData;
        if (localHistoryData.Length <= 0 || 2 != this.m_client.CurrentPage || (!this.m_client.curCommodityInfo.marketID.Equals(request.marketID) || !this.m_client.curCommodityInfo.commodityCode.Equals(request.sCode)))
          return;
        this.m_hqForm.Repaint();
      }
      catch (UriFormatException ex)
      {
        Logger.wirte(MsgType.Error, "UriFormatException" + ex.ToString());
      }
      catch (IOException ex)
      {
        Logger.wirte(MsgType.Error, "IOException" + ex.ToString());
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Exception" + ex.ToString());
      }
    }

    private void Get5MinLine(Packet_HttpRequest request)
    {
      try
      {
        bool flag = false;
        string StrUrl = this.m_client.strURLPath + "data/5min/" + request.marketID + request.sCode.Trim() + ".5min.zip";
        string str = this.pluginInfo.ConfigPath + "data/5min/" + request.marketID + "/" + request.marketID + request.sCode.Trim() + ".5min.zip";
        if (!Directory.Exists(str.Remove(str.LastIndexOf('/'))))
          Directory.CreateDirectory(str.Remove(str.LastIndexOf('/')));
        string path = str.Remove(str.LastIndexOf('.'));
        if (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTime(path).Day != DateTime.Now.Day || System.IO.File.GetLastWriteTime(path).Month != DateTime.Now.Month)
        {
          Logger.wirte(MsgType.Information, "Get 5Min : " + StrUrl);
          if (this.DownloadFile(StrUrl, str) && this.UnZipFile(str))
            flag = true;
        }
        else
          flag = true;
        if (!flag)
          return;
        KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(str.Remove(str.LastIndexOf('.')));
        CommodityInfo commodityInfo = new CommodityInfo(request.marketID, request.sCode);
        ProductData productData = this.m_client.GetProductData(commodityInfo);
        if (productData == null)
        {
          if (this.m_client.aProductData.Count > 50)
            this.m_client.aProductData.RemoveAt(50);
          productData = new ProductData();
          productData.commodityInfo = commodityInfo;
          productData.min5KLine = localHistoryData;
          this.m_client.aProductData.Insert(0, (object) productData);
        }
        else
          productData.min5KLine = localHistoryData;
        for (int index = 0; index < productData.min5KLine.Length; ++index)
        {
          if ((double) productData.min5KLine[index].balancePrice <= 0.0)
            productData.min5KLine[index].balancePrice = productData.min5KLine[index].totalAmount <= 0L ? (index <= 0 ? productData.min5KLine[index].closePrice : productData.min5KLine[index - 1].balancePrice) : (float) productData.min5KLine[index].totalMoney / (float) productData.min5KLine[index].totalAmount;
        }
        if (localHistoryData.Length <= 0 || 2 != this.m_client.CurrentPage || !this.m_client.curCommodityInfo.Compare(commodityInfo))
          return;
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
    }

    private void Get1MinLine(Packet_HttpRequest request)
    {
      try
      {
        bool flag = false;
        string StrUrl = this.m_client.strURLPath + "data/min/" + request.marketID + request.sCode.Trim() + ".min.zip";
        string str = this.pluginInfo.ConfigPath + "data/min/" + request.marketID + "/" + request.marketID + request.sCode.Trim() + ".min.zip";
        if (!Directory.Exists(str.Remove(str.LastIndexOf('/'))))
          Directory.CreateDirectory(str.Remove(str.LastIndexOf('/')));
        string path = str.Remove(str.LastIndexOf('.'));
        if (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTime(path).Day != DateTime.Now.Day || System.IO.File.GetLastWriteTime(path).Month != DateTime.Now.Month)
        {
          Logger.wirte(MsgType.Information, "Get 1Min : " + StrUrl);
          if (this.DownloadFile(StrUrl, str) && this.UnZipFile(str))
            flag = true;
        }
        else
          flag = true;
        if (!flag)
          return;
        KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(str.Remove(str.LastIndexOf('.')));
        CommodityInfo commodityInfo = new CommodityInfo(request.marketID, request.sCode);
        ProductData productData = this.m_client.GetProductData(commodityInfo);
        if (productData == null)
        {
          if (this.m_client.aProductData.Count > 50)
            this.m_client.aProductData.RemoveAt(50);
          productData = new ProductData();
          productData.commodityInfo = commodityInfo;
          productData.min1KLine = localHistoryData;
          this.m_client.aProductData.Insert(0, (object) productData);
        }
        else
          productData.min1KLine = localHistoryData;
        for (int index = 0; index < productData.min1KLine.Length; ++index)
        {
          if ((double) productData.min1KLine[index].balancePrice <= 0.0)
            productData.min1KLine[index].balancePrice = productData.min1KLine[index].totalAmount <= 0L ? (index <= 0 ? productData.min1KLine[index].closePrice : productData.min1KLine[index - 1].balancePrice) : (float) productData.min1KLine[index].totalMoney / (float) productData.min1KLine[index].totalAmount;
        }
        if (localHistoryData.Length <= 0 || 2 != this.m_client.CurrentPage || !this.m_client.curCommodityInfo.Compare(commodityInfo))
          return;
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
    }

    public bool DownloadFile(string StrUrl, string StrFileName)
    {
      FileStream fileStream = (FileStream) null;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(StrUrl);
        long contentLength = httpWebRequest.GetResponse().ContentLength;
        long offset;
        if (System.IO.File.Exists(StrFileName))
        {
          fileStream = System.IO.File.OpenWrite(StrFileName);
          offset = fileStream.Length;
          fileStream.Seek(offset, SeekOrigin.Current);
        }
        else
        {
          fileStream = new FileStream(StrFileName, FileMode.Create);
          offset = 0L;
        }
        if (offset > 0L)
          httpWebRequest.AddRange((int) offset);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        byte[] buffer = new byte[1024];
        int count = responseStream.Read(buffer, 0, 1024);
        while (count > 0)
        {
          fileStream.Write(buffer, 0, count);
          count = responseStream.Read(buffer, 0, 1024);
          long length = fileStream.Length;
        }
        fileStream.Close();
        responseStream.Close();
        return true;
      }
      catch (Exception ex)
      {
        if (fileStream != null)
          fileStream.Close();
        Logger.wirte(MsgType.Error, "下载文件异常：" + ex.Message);
        return false;
      }
    }

    private bool UnZipFile(string fileName)
    {
      try
      {
        using (FileStream fileStream1 = new FileStream(fileName, FileMode.Open))
        {
          using (FileStream fileStream2 = System.IO.File.Create(fileName.Remove(fileName.LastIndexOf('.'))))
          {
            using (GZipStream gzipStream = new GZipStream((Stream) fileStream1, CompressionMode.Decompress))
            {
              this.CopyTo((Stream) gzipStream, (Stream) fileStream2);
              Logger.wirte(MsgType.Information, "解压文件：" + fileName + "成功。");
            }
          }
        }
        System.IO.File.Delete(fileName);
        return true;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "解压缩文件：" + fileName + "出错：" + ex.Message);
        return false;
      }
    }

    public void CopyTo(Stream basestream, Stream destination)
    {
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (!basestream.CanRead && !basestream.CanWrite)
        throw new ObjectDisposedException((string) null, new Exception("输入流不可读写"));
      if (!destination.CanRead && !destination.CanWrite)
        throw new ObjectDisposedException("destination", new Exception("目标流不可读写"));
      if (!basestream.CanRead)
        throw new NotSupportedException("输入流不可读");
      if (!destination.CanWrite)
        throw new NotSupportedException("目标流不可写");
      this.InternalCopyTo(basestream, destination, 4096);
    }

    private void InternalCopyTo(Stream basestream, Stream destination, int bufferSize)
    {
      byte[] buffer = new byte[bufferSize];
      int count;
      while ((count = basestream.Read(buffer, 0, buffer.Length)) != 0)
        destination.Write(buffer, 0, count);
    }
  }
}
