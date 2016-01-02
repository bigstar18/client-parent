// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.HQThread.ReceiveThread
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
  public class ReceiveThread : MYThread
  {
    private string marketName = string.Empty;
    private string fristMarketCode = string.Empty;
    private bool onceReceiveTime = true;
    private bool isConnection;
    private HQClientMain m_client;
    private HQForm m_hqForm;
    private bool AddButtonFlag;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ButtonUtils buttonUtils;
    private MultiQuoteData multiQuoteData;
    private DateTime UpdateTradeEndTime;
    private bool isUpdateTradeTime;
    private bool isAddRightBtn;
    private HQClientMain hQClientMain;

    public ReceiveThread(HQClientMain client)
    {
      this.m_client = client;
      this.m_hqForm = this.m_client.m_hqForm;
      this.pluginInfo = client.pluginInfo;
      this.setInfo = client.setInfo;
      this.buttonUtils = client.buttonUtils;
      this.multiQuoteData = client.multiQuoteData;
      this.threadName = "接收线程";
    }

    protected override void run()
    {
      try
      {
        BinaryReader input = (BinaryReader) null;
        InputStreamConvert inputStreamConvert = (InputStreamConvert) null;
        while (this.m_client != null)
        {
          if (!this.blnIsStopped)
          {
            this.mUnique.WaitOne();
            this.mUnique.ReleaseMutex();
            if (this.m_client.m_socket != null)
            {
              if (this.m_client.m_socket.Connected)
              {
                try
                {
                  if (inputStreamConvert == null || input.BaseStream == null)
                  {
                    input = new BinaryReader((Stream) new NetworkStream(this.m_client.m_socket));
                    inputStreamConvert = new InputStreamConvert(input);
                  }
                  byte num = inputStreamConvert.ReadJavaByte();
                  switch (num)
                  {
                    case (byte) 0:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd:",
                        (object) num,
                        (object) "心跳数据"
                      }));
                      goto label_45;
                    case (byte) 1:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd:",
                        (object) num,
                        (object) "登陆"
                      }));
                      this.ReceiveLogon(inputStreamConvert);
                      goto label_45;
                    case (byte) 2:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd:",
                        (object) num,
                        (object) "服务器端版本号"
                      }));
                      this.m_client.isResendVersion = true;
                      this.m_client.m_Version = inputStreamConvert.ReadJavaUTF();
                      this.GetVersionInfo(this.m_client.m_Version);
                      goto label_45;
                    case (byte) 3:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 更新码表"
                      }));
                      this.ReceiveCodeTable(inputStreamConvert);
                      goto label_45;
                    case (byte) 4:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 个股行情"
                      }));
                      this.ReceiveStockQuote(inputStreamConvert);
                      goto label_45;
                    case (byte) 5:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 报价排名"
                      }));
                      this.ReceiveQuoteList(inputStreamConvert);
                      goto label_45;
                    case (byte) 6:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 分时数据"
                      }));
                      this.ReceiveMinLineData(inputStreamConvert);
                      goto label_45;
                    case (byte) 7:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 成交明细"
                      }));
                      this.ReceiveBillDataByV(inputStreamConvert);
                      goto label_45;
                    case (byte) 8:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 交易节时间"
                      }));
                      this.ReceiveTradeTime(inputStreamConvert);
                      if (!this.AddButtonFlag)
                      {
                        this.RemakeButton();
                        goto label_45;
                      }
                      else
                        goto label_45;
                    case (byte) 9:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 市场日期"
                      }));
                      this.ReceiveMarketDate(inputStreamConvert);
                      goto label_45;
                    case (byte) 10:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 综合排名"
                      }));
                      this.ReceiveMarketSort(inputStreamConvert);
                      goto label_45;
                    case (byte) 12:
                      this.m_client.m_iMinLineInterval = inputStreamConvert.ReadJavaInt();
                      Logger.wirte(MsgType.Information, this.marketName + (object) "Receive cmd: " + (string) (object) num + " 分时间隔:" + (string) (object) this.m_client.m_iMinLineInterval);
                      if (this.m_client.m_iMinLineInterval > 0)
                      {
                        if (this.m_client.m_iMinLineInterval <= 60)
                          goto label_45;
                      }
                      this.m_client.m_iMinLineInterval = 60;
                      goto label_45;
                    case (byte) 14:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 切换交易日清空数据"
                      }));
                      this.ReceiveUpdateTradeTime(inputStreamConvert);
                      goto label_45;
                    case (byte) 15:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 行业数据"
                      }));
                      this.ReceiveIndustryData(inputStreamConvert);
                      goto label_45;
                    case (byte) 16:
                      Logger.wirte(MsgType.Information, string.Concat(new object[4]
                      {
                        (object) this.marketName,
                        (object) "Receive cmd: ",
                        (object) num,
                        (object) " 地域数据"
                      }));
                      this.ReceiveRegionData(inputStreamConvert);
                      goto label_45;
                    default:
                      goto label_45;
                  }
                }
                catch (EndOfStreamException ex)
                {
                  Logger.wirte(MsgType.Error, this.marketName + "接收线程发生EndOfStreamException异常：" + ex.ToString());
                  if (this.m_client != null)
                  {
                    if (this.m_client.m_socket != null)
                      this.m_client.m_socket.Close();
                    input = (BinaryReader) null;
                    inputStreamConvert = (InputStreamConvert) null;
                    this.m_client.sendThread.AskForData((CMDVO) null);
                    goto label_45;
                  }
                  else
                    goto label_45;
                }
                catch (IOException ex)
                {
                  Logger.wirte(MsgType.Error, this.marketName + "接收线程发生IOException异常：" + ex.ToString());
                  if (this.m_client != null)
                  {
                    if (this.m_client.m_socket != null)
                      this.m_client.m_socket.Close();
                    input = (BinaryReader) null;
                    inputStreamConvert = (InputStreamConvert) null;
                    this.m_client.sendThread.AskForData((CMDVO) null);
                    goto label_45;
                  }
                  else
                    goto label_45;
                }
                catch (InvalidDataException ex)
                {
                  Logger.wirte(MsgType.Error, this.marketName + "接收线程发生InvalidDataException异常：" + ex.ToString());
                  if (this.m_client != null)
                  {
                    if (this.m_client.m_socket != null)
                      this.m_client.m_socket.Close();
                    input = (BinaryReader) null;
                    inputStreamConvert = (InputStreamConvert) null;
                    this.m_client.sendThread.AskForData((CMDVO) null);
                    goto label_45;
                  }
                  else
                    goto label_45;
                }
                catch (ThreadAbortException ex)
                {
                  Logger.wirte(MsgType.Error, this.marketName + "接收线程发生ThreadAbortException异常：" + ex.ToString());
                  if (this.m_client != null)
                  {
                    if (this.m_client.m_socket != null)
                      this.m_client.m_socket.Close();
                    input = (BinaryReader) null;
                    inputStreamConvert = (InputStreamConvert) null;
                    goto label_45;
                  }
                  else
                    goto label_45;
                }
                catch (Exception ex)
                {
                  Logger.wirte(MsgType.Error, this.marketName + "接收线程发生Exception异常：" + ex.ToString());
                  if (this.m_client != null)
                  {
                    if (this.m_client.m_socket != null)
                      this.m_client.m_socket.Close();
                    input = (BinaryReader) null;
                    inputStreamConvert = (InputStreamConvert) null;
                    this.m_client.sendThread.AskForData((CMDVO) null);
                    goto label_45;
                  }
                  else
                    goto label_45;
                }
              }
            }
            input = (BinaryReader) null;
            inputStreamConvert = (InputStreamConvert) null;
            this.m_client.sendThread.AskForData((CMDVO) null);
            Thread.Sleep(500);
label_45:
            this.ReData();
          }
          else
            break;
        }
      }
      catch (ThreadAbortException ex)
      {
        Logger.wirte(MsgType.Error, this.marketName + (string) ex.ExceptionState + "》》》》》》》》》》》》》》");
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, this.marketName + "接收线程发生异常：" + ex.ToString());
      }
      Logger.wirte(MsgType.Information, this.marketName + "**********" + this.threadName + "结束！！！**********");
    }

    private void ReceiveUpdateTradeTime(InputStreamConvert m_reader)
    {
      this.isUpdateTradeTime = true;
      this.ClearDataPlus(m_reader.ReadJavaUTF());
      this.UpdateTradeEndTime = DateTime.Now;
    }

    private void ReData()
    {
      if (!this.isUpdateTradeTime || (DateTime.Now - this.UpdateTradeEndTime).TotalSeconds < 2.0)
        return;
      this.m_client.httpThread.GetCodeList();
      this.m_hqForm.ReQueryCurClient();
      this.UpdateTradeEndTime = DateTime.Now;
      this.isUpdateTradeTime = false;
    }

    private void ClearLocalKLineDatas(string marketID)
    {
      string path1 = this.pluginInfo.ConfigPath + "data/day/" + marketID;
      string path2 = this.pluginInfo.ConfigPath + "data/min/" + marketID;
      string path3 = this.pluginInfo.ConfigPath + "data/5min/" + marketID;
      if (Directory.Exists(path1))
        Directory.Delete(path1, true);
      if (Directory.Exists(path2))
        Directory.Delete(path2, true);
      if (!Directory.Exists(path3))
        return;
      Directory.Delete(path3, true);
    }

    private void ReceiveRegionData(InputStreamConvert m_reader)
    {
      int num = m_reader.ReadJavaInt();
      this.m_client.m_htRegion.Clear();
      for (int index = 0; index < num; ++index)
        this.m_client.m_htRegion.Add((object) m_reader.ReadJavaUTF(), (object) m_reader.ReadJavaUTF().Trim());
    }

    private void ReceiveIndustryData(InputStreamConvert m_reader)
    {
      int num = m_reader.ReadJavaInt();
      this.m_client.m_htIndustry.Clear();
      for (int index = 0; index < num; ++index)
        this.m_client.m_htIndustry.Add((object) m_reader.ReadJavaUTF(), (object) m_reader.ReadJavaUTF());
    }

    private void ClearData()
    {
      this.m_client.m_codeList.Clear();
      this.m_client.m_htProduct.Clear();
      this.m_client.aProductData.Clear();
      this.m_client.m_quoteList = new ProductDataVO[0];
      this.m_client.m_iCodeDate = 0;
      this.m_client.m_iCodeTime = 0;
    }

    private void ClearDataPlus(string marketID)
    {
      ArrayList arrayList1 = new ArrayList();
      foreach (CommodityInfo commodityInfo in this.m_client.m_codeList)
      {
        if (commodityInfo.marketID != marketID)
          arrayList1.Add((object) commodityInfo);
      }
      this.m_client.m_codeList = arrayList1;
      Hashtable hashtable = new Hashtable();
      foreach (string str in (IEnumerable) this.m_client.m_htProduct.Keys)
      {
        if (((CodeTable) this.m_client.m_htProduct[(object) str]).marketID != marketID)
          hashtable.Add((object) str, (object) (CodeTable) this.m_client.m_htProduct[(object) str]);
      }
      this.m_client.m_htProduct = hashtable;
      ArrayList arrayList2 = new ArrayList();
      foreach (ProductData productData in this.m_client.aProductData)
      {
        if (productData.commodityInfo.marketID != marketID)
          arrayList2.Add((object) productData);
      }
      this.m_client.aProductData = arrayList2;
      ArrayList arrayList3 = new ArrayList();
      for (int index = 0; index < this.m_client.m_quoteList.Length; ++index)
      {
        ProductDataVO productDataVo = this.m_client.m_quoteList[index];
        if (productDataVo.marketID != marketID)
          arrayList3.Add((object) productDataVo);
      }
      this.m_client.m_quoteList = new ProductDataVO[arrayList3.Count];
      for (int index = 0; index < arrayList3.Count; ++index)
        this.m_client.m_quoteList[index] = (ProductDataVO) arrayList3[index];
      this.m_client.m_iCodeDate = 0;
      this.m_client.m_iCodeTime = 0;
    }

    private void ReceiveMarketDate(InputStreamConvert m_reader)
    {
      string str = m_reader.ReadJavaUTF();
      int num1 = m_reader.ReadJavaInt();
      int num2 = m_reader.ReadJavaInt();
      if (this.onceReceiveTime)
      {
        this.fristMarketCode = str;
        this.onceReceiveTime = false;
      }
      if (this.m_client.m_htMarketData.ContainsKey((object) str))
      {
        MarketDataVO marketDataVo = (MarketDataVO) this.m_client.m_htMarketData[(object) str];
        marketDataVo.date = num1;
        marketDataVo.time = num2;
      }
      else
        this.m_client.m_htMarketData.Add((object) str, (object) new MarketDataVO()
        {
          marketID = str,
          date = num1,
          time = num2
        });
      int num3 = this.m_client.m_iDate;
      int num4 = this.m_client.m_iTime;
      if (this.setInfo.CurTimeMarketId == str)
      {
        this.m_client.m_iTime = num2;
        if (this.m_client.m_iDate == 0 || num1 != num3)
          this.m_client.m_iDate = num1;
      }
      else if ((this.setInfo.CurTimeMarketId == "" || this.setInfo.CurTimeMarketId == null) && (str == "00" || !this.m_client.m_htMarketData.ContainsKey((object) "00") && str == this.fristMarketCode))
      {
        this.m_client.m_iTime = num2;
        if (this.m_client.m_iDate == 0 || num1 != num3)
          this.m_client.m_iDate = num1;
      }
      if (!Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
      {
        this.m_client.m_iTime = num2;
        if (this.m_client.m_iDate == 0)
          this.m_client.m_iDate = num1;
      }
      Logger.wirte(MsgType.Information, this.marketName + (object) "市场日期为:" + (string) (object) this.m_client.m_iDate + "市场时间为： " + (string) (object) num2);
      if (num3 == this.m_client.m_iDate && num4 == this.m_client.m_iTime)
        return;
      this.m_hqForm.Repaint();
      this.m_hqForm.RepaintBottom();
    }

    private void ReceiveTradeTime(InputStreamConvert m_reader)
    {
      Hashtable hashtable = CMDTradeTimeVO.getObj(m_reader);
      foreach (string str in (IEnumerable) hashtable.Keys)
      {
        if (this.m_client.m_htMarketData.ContainsKey((object) str))
          ((MarketDataVO) this.m_client.m_htMarketData[(object) str]).m_timeRange = ((MarketDataVO) hashtable[(object) str]).m_timeRange;
        else
          this.m_client.m_htMarketData.Add((object) str, hashtable[(object) str]);
      }
    }

    protected override void disposeThread()
    {
      try
      {
        if (this.m_client != null)
        {
          if (this.m_client.m_socket != null)
          {
            this.m_client.m_socket.Close();
            this.m_client.m_socket = (Socket) null;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      this.Abort("**********强行中断" + this.threadName + "！！！**********");
    }

    private void ReceiveQuoteList(InputStreamConvert reader)
    {
      bool flag1 = false;
      ProductDataVO[] productDataVoArray1 = CMDQuoteListVO.getObj(reader);
      Logger.wirte(MsgType.Information, this.marketName + (object) "本次更新数据条数=" + (string) (object) productDataVoArray1.Length);
      foreach (ProductDataVO productDataVo in productDataVoArray1)
      {
        foreach (CommodityInfo commodityInfo in this.m_client.m_codeList)
        {
          if (productDataVo.code == commodityInfo.commodityCode)
          {
            productDataVo.industry = commodityInfo.industry;
            productDataVo.region = commodityInfo.region;
          }
        }
      }
      if (this.m_client.m_quoteList.Length == 0)
      {
        flag1 = true;
        this.m_client.m_quoteList = productDataVoArray1;
      }
      else
      {
        for (int index1 = 0; index1 < productDataVoArray1.Length; ++index1)
        {
          productDataVoArray1[index1].datahighlightTime = this.multiQuoteData.HighlightTime;
          bool flag2 = false;
          for (int index2 = 0; index2 < this.m_client.m_quoteList.Length; ++index2)
          {
            if (this.m_client.m_quoteList[index2].code.Equals(productDataVoArray1[index1].code) && this.m_client.m_quoteList[index2].marketID.Equals(productDataVoArray1[index1].marketID))
            {
              this.m_client.m_quoteList[index2] = productDataVoArray1[index1];
              flag2 = true;
              break;
            }
          }
          for (int index2 = 0; index2 < this.multiQuoteData.m_curQuoteList.Length; ++index2)
          {
            if (this.multiQuoteData.m_curQuoteList[index2].code.Equals(productDataVoArray1[index1].code) && this.multiQuoteData.m_curQuoteList[index2].marketID.Equals(productDataVoArray1[index1].marketID))
            {
              flag1 = true;
              break;
            }
          }
          if (!flag2)
          {
            ProductDataVO[] productDataVoArray2 = new ProductDataVO[this.m_client.m_quoteList.Length + 1];
            for (int index2 = 0; index2 < this.m_client.m_quoteList.Length; ++index2)
              productDataVoArray2[index2] = this.m_client.m_quoteList[index2];
            productDataVoArray2[productDataVoArray2.Length - 1] = productDataVoArray1[index1];
            this.m_client.m_quoteList = productDataVoArray2;
          }
          if (productDataVoArray1.Length > 0 && this.m_client.m_bShowIndexAtBottom == 1 && (this.m_client.indexMainCode.Length > 0 && string.Compare(productDataVoArray1[index1].marketID + "_" + productDataVoArray1[index1].code, this.m_client.indexMainCode, true) == 0))
          {
            ProductData productData = this.m_client.GetProductData(this.m_client.curCommodityInfo);
            if (productData == null)
            {
              if (this.m_client.aProductData.Count > 50)
                this.m_client.aProductData.RemoveAt(50);
              this.m_client.aProductData.Insert(0, (object) new ProductData()
              {
                commodityInfo = new CommodityInfo(productDataVoArray1[index1].marketID, productDataVoArray1[index1].code),
                realData = productDataVoArray1[index1]
              });
            }
            else
              productData.realData = productDataVoArray1[index1];
            this.m_hqForm.RepaintBottom();
          }
        }
      }
      if (this.m_client.CurrentPage != 0)
        return;
      if (this.m_client.m_bShowIndexAtBottom == 1)
        this.m_hqForm.RepaintBottom();
      if (!flag1)
        return;
      this.m_hqForm.Repaint();
    }

    private void ReceiveMinLineData(InputStreamConvert reader)
    {
      string str1 = reader.ReadJavaUTF();
      string str2 = reader.ReadJavaUTF();
      int num = (int) reader.ReadJavaByte();
      reader.ReadJavaInt();
      reader.ReadJavaInt();
      MinDataVO[] minDataVoArray = CMDMinVO.getObj(reader);
      Logger.wirte(MsgType.Information, "分时数据条数======" + minDataVoArray.Length.ToString());
      ProductData productData = this.m_client.GetProductData(str1, str2);
      if (productData == null)
      {
        if (this.m_client.aProductData.Count > 50)
          this.m_client.aProductData.RemoveAt(50);
        productData = new ProductData();
        productData.commodityInfo = new CommodityInfo(str1, str2);
        this.m_client.aProductData.Insert(0, (object) productData);
      }
      lock (productData)
      {
        productData.aMinLine = ArrayList.Synchronized(new ArrayList());
        int local_4 = 0;
        for (int local_5 = 0; local_5 < minDataVoArray.Length; ++local_5)
        {
          TradeTimeVO[] local_6 = ((MarketDataVO) this.m_client.m_htMarketData[(object) str1]).m_timeRange;
          int local_7 = M_Common.GetMinLineIndexFromTime(minDataVoArray[local_5].date, minDataVoArray[local_5].time, local_6, this.m_client.m_iMinLineInterval);
          for (int local_8 = local_4; local_8 < local_7; ++local_8)
          {
            MinDataVO local_9 = new MinDataVO();
            if (local_8 > 0)
            {
              local_9.curPrice = ((MinDataVO) productData.aMinLine[local_8 - 1]).curPrice;
              local_9.totalAmount = ((MinDataVO) productData.aMinLine[local_8 - 1]).totalAmount;
              local_9.totalMoney = ((MinDataVO) productData.aMinLine[local_8 - 1]).totalMoney;
              local_9.averPrice = ((MinDataVO) productData.aMinLine[local_8 - 1]).averPrice;
              local_9.reserveCount = ((MinDataVO) productData.aMinLine[local_8 - 1]).reserveCount;
            }
            else if (productData.realData != null)
            {
              local_9.curPrice = productData.realData.yesterBalancePrice;
              local_9.averPrice = productData.realData.yesterBalancePrice;
            }
            productData.aMinLine.Add((object) local_9);
          }
          if (local_7 >= productData.aMinLine.Count - 1)
          {
            MinDataVO local_10_1;
            if (local_7 == productData.aMinLine.Count - 1)
            {
              local_10_1 = (MinDataVO) productData.aMinLine[productData.aMinLine.Count - 1];
            }
            else
            {
              local_10_1 = new MinDataVO();
              productData.aMinLine.Add((object) local_10_1);
            }
            local_10_1.curPrice = minDataVoArray[local_5].curPrice;
            local_10_1.totalAmount = minDataVoArray[local_5].totalAmount;
            local_10_1.reserveCount = minDataVoArray[local_5].reserveCount;
            local_10_1.averPrice = minDataVoArray[local_5].averPrice;
            local_4 = local_7 + 1;
          }
        }
      }
      if (2 != this.m_client.CurrentPage && 1 != this.m_client.CurrentPage || !this.m_hqForm.IsMultiCommidity && (string.Compare(this.m_client.curCommodityInfo.marketID, productData.commodityInfo.marketID) != 0 || string.Compare(this.m_client.curCommodityInfo.commodityCode, productData.commodityInfo.commodityCode) != 0))
        return;
      Logger.wirte(MsgType.Information, "重绘==============");
      this.m_hqForm.Repaint();
    }

    private void ReceiveBillData(InputStreamConvert reader)
    {
      string str1 = reader.ReadJavaUTF();
      string str2 = reader.ReadJavaUTF();
      byte num = reader.ReadJavaByte();
      reader.ReadJavaInt();
      int time = reader.ReadJavaInt();
      BillDataVO[] values = CMDBillVO.getObj(reader, ref this.isConnection);
      ProductData stock = this.m_client.GetProductData(str1, str2);
      if (stock == null)
      {
        if (this.m_client.aProductData.Count > 50)
          this.m_client.aProductData.RemoveAt(50);
        stock = new ProductData();
        stock.commodityInfo = new CommodityInfo(str1, str2);
        this.m_client.aProductData.Insert(0, (object) stock);
      }
      switch (num)
      {
        case (byte) 0:
          this.MakeLastBills(stock, time, values);
          break;
        case (byte) 1:
          stock.lastBill = ArrayList.Synchronized(new ArrayList());
          for (int index = 0; index < values.Length; ++index)
            stock.lastBill.Add((object) values[index]);
          if (values.Length == 0 || 1 != this.m_client.CurrentPage || !this.m_hqForm.IsMultiCommidity && (string.Compare(this.m_client.curCommodityInfo.marketID, str1) != 0 || string.Compare(this.m_client.curCommodityInfo.commodityCode, str2) != 0))
            break;
          this.m_hqForm.Repaint();
          break;
        case (byte) 2:
          this.MakeLastBills(stock, time, values);
          break;
      }
    }

    private void ReceiveBillDataByV(InputStreamConvert reader)
    {
      string str1 = reader.ReadJavaUTF();
      string str2 = reader.ReadJavaUTF();
      byte num1 = reader.ReadJavaByte();
      reader.ReadJavaLong();
      long num2 = reader.ReadJavaLong();
      reader.ReadJavaUTF();
      BillDataVO[] values = CMDBillVO.getObj(reader, ref this.isConnection);
      ProductData stock = this.m_client.GetProductData(str1, str2);
      if (stock == null)
      {
        if (this.m_client.aProductData.Count > 50)
          this.m_client.aProductData.RemoveAt(50);
        stock = new ProductData();
        stock.commodityInfo = new CommodityInfo(str1, str2);
        this.m_client.aProductData.Insert(0, (object) stock);
      }
      switch (num1)
      {
        case (byte) 0:
          this.MakeLastBills(stock, (int) num2, values);
          break;
        case (byte) 1:
          this.m_client.isChangePage = 0;
          stock.lastBill = ArrayList.Synchronized(new ArrayList());
          for (int index = 0; index < values.Length; ++index)
            stock.lastBill.Add((object) values[index]);
          if (values.Length == 0 || 1 != this.m_client.CurrentPage || !this.m_hqForm.IsMultiCommidity && (string.Compare(this.m_client.curCommodityInfo.marketID, str1) != 0 || string.Compare(this.m_client.curCommodityInfo.commodityCode, str2) != 0))
            break;
          this.m_hqForm.Repaint();
          break;
        case (byte) 2:
          this.MakeLastBills(stock, (int) num2, values);
          break;
      }
    }

    private void MakeLastBills(ProductData stock, int time, BillDataVO[] values)
    {
      if (values.Length <= 0)
        this.m_client.isChangePage = 0;
      else if (time == 0)
      {
        Logger.wirte(MsgType.Information, "time=======" + time.ToString() + "     " + values.Length.ToString());
        this.m_client.isChangePage = 0;
        for (int index = 0; index < values.Length; ++index)
        {
          if (stock.aBill.Count > 0)
          {
            BillDataVO billDataVo = (BillDataVO) stock.aBill[stock.aBill.Count - 1];
            if (billDataVo == null || values[index] == null || billDataVo.totalAmount >= values[index].totalAmount)
              continue;
          }
          stock.aBill.Add((object) values[index]);
          if (this.m_client.CurrentPage == 2)
          {
            if (stock.lastBill.Count > 0)
            {
              BillDataVO billDataVo = (BillDataVO) stock.lastBill[stock.lastBill.Count - 1];
              if (billDataVo.date > values[0].date || billDataVo.date == values[0].date && billDataVo.totalAmount >= values[index].totalAmount)
                continue;
            }
            stock.lastBill.Add((object) values[index]);
          }
        }
        if (4 != this.m_client.CurrentPage && (2 != this.m_client.CurrentPage || 7 != this.m_client.m_iKLineCycle && 8 != this.m_client.m_iKLineCycle && (9 != this.m_client.m_iKLineCycle && 10 != this.m_client.m_iKLineCycle) && (5 != this.m_client.m_iKLineCycle && 6 != this.m_client.m_iKLineCycle && 14 != this.m_client.m_iKLineCycle)))
          return;
        this.m_hqForm.Repaint();
      }
      else
      {
        if (this.m_client.isChangePage == 1)
          return;
        if (stock.aBill == null)
          stock.aBill = ArrayList.Synchronized(new ArrayList());
        bool flag1 = false;
        bool flag2 = false;
        switch (this.m_client.CurrentPage)
        {
          case 1:
            if (stock.lastBill.Count > 0)
            {
              BillDataVO billDataVo = (BillDataVO) stock.lastBill[stock.lastBill.Count - 1];
              if (billDataVo.date > values[0].date || billDataVo.date == values[0].date && billDataVo.totalAmount >= values[0].totalAmount)
                return;
              break;
            }
            break;
          case 2:
            if (stock.aBill.Count > 0)
            {
              BillDataVO billDataVo = (BillDataVO) stock.aBill[stock.aBill.Count - 1];
              if (billDataVo.date > values[0].date || billDataVo.date == values[0].date && billDataVo.totalAmount >= values[0].totalAmount)
                flag1 = true;
            }
            if (stock.lastBill.Count > 0)
            {
              BillDataVO billDataVo = (BillDataVO) stock.lastBill[stock.lastBill.Count - 1];
              if (billDataVo.date > values[0].date || billDataVo.date == values[0].date && billDataVo.totalAmount >= values[0].totalAmount)
                flag2 = true;
            }
            if (flag1 && flag2)
              return;
            break;
          case 4:
            if (stock.aBill.Count > 0)
            {
              BillDataVO billDataVo = (BillDataVO) stock.aBill[stock.aBill.Count - 1];
              if (billDataVo.date > values[0].date || billDataVo.date == values[0].date && billDataVo.totalAmount >= values[0].totalAmount)
                return;
              break;
            }
            break;
        }
        for (int index = 0; index < values.Length; ++index)
        {
          if (this.m_client.CurrentPage == 1)
            stock.lastBill.Add((object) values[index]);
          else if (this.m_client.CurrentPage == 2)
          {
            if (!flag2)
              stock.lastBill.Add((object) values[index]);
            if (!flag1)
            {
              if (values[index] == null && stock.aBill.Count > 1)
              {
                stock.aBill.RemoveAt(stock.aBill.Count - 1);
                break;
              }
              stock.aBill.Add((object) values[index]);
            }
          }
          else if (this.m_client.CurrentPage == 4)
          {
            if (values[index] == null && stock.aBill.Count > 1)
            {
              stock.aBill.RemoveAt(stock.aBill.Count - 1);
              break;
            }
            stock.aBill.Add((object) values[index]);
          }
        }
        if (this.m_client.CurrentPage == 4)
        {
          this.m_hqForm.Repaint();
        }
        else
        {
          if (2 != this.m_client.CurrentPage && 1 != this.m_client.CurrentPage)
            return;
          if (stock.aMinLine == null)
            stock.aMinLine = ArrayList.Synchronized(new ArrayList());
          for (int index = 0; index < values.Length; ++index)
          {
            TradeTimeVO[] timeRange = ((MarketDataVO) this.m_client.m_htMarketData[(object) stock.commodityInfo.marketID]).m_timeRange;
            int lineIndexFromTime = M_Common.GetMinLineIndexFromTime(values[index].date, values[index].time, timeRange, this.m_client.m_iMinLineInterval);
            if (lineIndexFromTime >= stock.aMinLine.Count - 1)
            {
              MinDataVO minDataVo = (MinDataVO) null;
              if (lineIndexFromTime == stock.aMinLine.Count - 1)
              {
                minDataVo = (MinDataVO) stock.aMinLine[lineIndexFromTime];
              }
              else
              {
                for (int count = stock.aMinLine.Count; count <= lineIndexFromTime; ++count)
                {
                  minDataVo = new MinDataVO();
                  if (count > 0)
                  {
                    minDataVo.curPrice = ((MinDataVO) stock.aMinLine[count - 1]).curPrice;
                    minDataVo.totalAmount = ((MinDataVO) stock.aMinLine[count - 1]).totalAmount;
                    minDataVo.totalMoney = ((MinDataVO) stock.aMinLine[count - 1]).totalMoney;
                    minDataVo.reserveCount = ((MinDataVO) stock.aMinLine[count - 1]).reserveCount;
                    minDataVo.averPrice = ((MinDataVO) stock.aMinLine[count - 1]).averPrice;
                  }
                  stock.aMinLine.Add((object) minDataVo);
                }
              }
              minDataVo.curPrice = values[index].curPrice;
              minDataVo.totalAmount = values[index].totalAmount;
              minDataVo.totalMoney = values[index].totalMoney;
              minDataVo.reserveCount = values[index].reserveCount;
              minDataVo.averPrice = values[index].balancePrice;
            }
            else
              break;
          }
          if (values[values.Length - 1].date > this.m_client.m_iDate)
            this.m_client.m_iDate = values[values.Length - 1].date;
          if (values[values.Length - 1].date >= this.m_client.m_iDate && values[values.Length - 1].time > this.m_client.m_iTime)
            this.m_client.m_iTime = values[values.Length - 1].time;
          if (2 != this.m_client.CurrentPage && 1 != this.m_client.CurrentPage && 4 != this.m_client.CurrentPage || !this.m_hqForm.IsMultiCommidity && (string.Compare(this.m_client.curCommodityInfo.marketID, stock.commodityInfo.marketID) != 0 || string.Compare(this.m_client.curCommodityInfo.commodityCode, stock.commodityInfo.commodityCode) != 0))
            return;
          this.m_hqForm.Repaint();
        }
      }
    }

    private void ReceiveMarketSort(InputStreamConvert reader)
    {
      int num = reader.ReadJavaInt();
      MarketStatusVO[] marketStatusVoArray = CMDMarketSortVO.getObj(reader);
      if (5 != this.m_client.CurrentPage)
        return;
      Page_MarketStatus pageMarketStatus = (Page_MarketStatus) this.m_hqForm.MainGraph;
      pageMarketStatus.packetInfo = new Packet_MarketStatus();
      pageMarketStatus.packetInfo.iCount = num;
      pageMarketStatus.statusData = marketStatusVoArray;
      this.m_hqForm.Repaint();
    }

    private void ReceiveClassSort(InputStreamConvert reader)
    {
      byte num1 = reader.ReadJavaByte();
      int num2 = (int) reader.ReadJavaByte();
      int num3 = reader.ReadJavaInt();
      int num4 = reader.ReadJavaInt();
      Logger.wirte(MsgType.Information, " sortBy:" + (object) num1 + " totalCount:" + (string) (object) num3 + " start:" + (string) (object) num4);
      CMDSortVO.getObj(reader);
    }

    private void ReceiveStockQuote(InputStreamConvert reader)
    {
      ProductDataVO[] productDataVoArray = CMDQuoteVO.getObj(reader);
      for (int index = 0; index < productDataVoArray.Length; ++index)
      {
        string str1 = productDataVoArray[index].marketID;
        string str2 = productDataVoArray[index].code;
        ProductData productData = this.m_client.GetProductData(str1, str2);
        if (productData == null)
        {
          if (this.m_client.aProductData.Count > 50)
            this.m_client.aProductData.RemoveAt(50);
          this.m_client.aProductData.Insert(0, (object) new ProductData()
          {
            commodityInfo = new CommodityInfo(str1, str2),
            realData = productDataVoArray[index]
          });
        }
        else
          productData.realData = productDataVoArray[index];
      }
      if (productDataVoArray.Length > 0 && (2 == this.m_client.CurrentPage || 1 == this.m_client.CurrentPage))
        this.m_hqForm.Repaint();
      if (productDataVoArray.Length <= 0 || this.m_client.m_bShowIndexAtBottom != 1)
        return;
      this.m_hqForm.RepaintBottom();
    }

    public void ReceiveCodeTable(InputStreamConvert reader)
    {
      ProductInfoListVO productInfoListVo = CMDProductInfoVO.getObj(reader);
      for (int index1 = 0; index1 < productInfoListVo.productInfos.Length; ++index1)
      {
        CommodityInfo commodityInfo = new CommodityInfo(productInfoListVo.productInfos[index1].marketID, productInfoListVo.productInfos[index1].code, productInfoListVo.productInfos[index1].region, productInfoListVo.productInfos[index1].industry);
        bool flag = false;
        for (int index2 = 0; index2 < this.m_client.m_codeList.Count; ++index2)
        {
          if (commodityInfo.Compare(this.m_client.m_codeList[index2]))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.m_client.m_codeList.Add((object) commodityInfo);
          if (productInfoListVo.productInfos[index1].status == 1)
            this.m_client.hm_codeList.Add((object) commodityInfo);
          else
            this.m_client.nm_codeList.Add((object) commodityInfo);
        }
        CodeTable codeTable = new CodeTable();
        codeTable.marketID = productInfoListVo.productInfos[index1].marketID;
        codeTable.sName = productInfoListVo.productInfos[index1].name;
        if (this.m_hqForm.AddMarketName)
          codeTable.sName = this.marketName + codeTable.sName;
        codeTable.sPinyin = productInfoListVo.productInfos[index1].pyName;
        codeTable.status = productInfoListVo.productInfos[index1].status;
        codeTable.fUnit = productInfoListVo.productInfos[index1].fUnit;
        codeTable.tradeSecNo = productInfoListVo.productInfos[index1].tradeSecNo;
        codeTable.mPrice = productInfoListVo.productInfos[index1].mPrice;
        this.m_client.m_htProduct[(object) (productInfoListVo.productInfos[index1].marketID + productInfoListVo.productInfos[index1].code)] = (object) codeTable;
        this.m_client.m_iCodeDate = productInfoListVo.date;
        this.m_client.m_iCodeTime = productInfoListVo.time;
      }
    }

    public void ReceiveMarketTable(InputStreamConvert reader)
    {
    }

    public void RemakeButton()
    {
      while (this.m_hqForm.MainGraph == null)
        Thread.Sleep(100);
      this.AddMarketButton();
      if (!this.isAddRightBtn)
      {
        this.AddRightButton();
        this.isAddRightBtn = true;
      }
      ++this.buttonUtils.isTidyBtnFlag;
      if (this.buttonUtils.isTidyBtnFlag == 2)
        this.buttonUtils.TidyButtons(this.buttonUtils.ButtonList);
      if (this.m_client.CurrentPage != 0)
        return;
      this.m_hqForm.Repaint();
    }

    public void AddMarketButton()
    {
      if (this.m_client.m_htMarketData == null)
        return;
      this.AddButtonFlag = true;
      if (Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
        this.buttonUtils.InsertButton(0, new MyButton("AllMarket", "所有市场商品", this.buttonUtils.InitialButtonName == "AllMarket"), this.buttonUtils.ButtonList);
      else if (this.m_client.m_htMarketData != null)
      {
        IDictionaryEnumerator enumerator = this.m_client.m_htMarketData.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            DictionaryEntry dictionaryEntry = (DictionaryEntry) enumerator.Current;
            MarketDataVO marketDataVo = (MarketDataVO) dictionaryEntry.Value;
            if (this.buttonUtils.InitialButtonName.StartsWith("Market"))
              this.buttonUtils.InitialButtonName.Substring(6);
            MyButton myButton = new MyButton("Market" + dictionaryEntry.Key.ToString(), "所有商品", true);
            if (this.buttonUtils.ButtonList.Count == 0)
              this.buttonUtils.InsertButton(0, myButton, this.buttonUtils.ButtonList);
            else if (((MyButton) this.buttonUtils.ButtonList[0]).Name != myButton.Name)
              this.buttonUtils.InsertButton(0, myButton, this.buttonUtils.ButtonList);
          }
        }
        finally
        {
          IDisposable disposable = enumerator as IDisposable;
          if (disposable != null)
            disposable.Dispose();
        }
      }
      bool Selected = false;
      if (this.buttonUtils.InitialButtonName.Equals("MyCommodity"))
        Selected = true;
      this.buttonUtils.InsertButton(1, new MyButton("MyCommodity", this.pluginInfo.HQResourceManager.GetString("HQStr_MyCommodity"), Selected), this.buttonUtils.ButtonList);
    }

    public void AddRightButton()
    {
      string[] strArray1 = this.setInfo.RightButtonItems.Split(';');
      for (int index = 0; index < strArray1.Length - 1; ++index)
      {
        string[] strArray2 = strArray1[index].Split(':');
        if (strArray2[0].Equals("CareFul"))
          this.buttonUtils.InsertButton(index, new MyButton("X_Btn", strArray2[1], true), this.buttonUtils.RightButtonList);
        else if (strArray2[0].Equals("Plate"))
          this.buttonUtils.InsertButton(index, new MyButton("P_Btn", strArray2[1], false), this.buttonUtils.RightButtonList);
        else if (strArray2[0].Equals("Map"))
          this.buttonUtils.InsertButton(index, new MyButton("T_Btn", strArray2[1], false), this.buttonUtils.RightButtonList);
      }
    }

    public void GetVersionInfo(string version)
    {
      if (version.Length <= 0)
        return;
      try
      {
        VersionInfo.First = version.Substring(0, 1);
        string[] strArray = version.Split('.');
        VersionInfo.MainVersion = strArray[0].Substring(1, strArray[0].Length - VersionInfo.First.Length);
        VersionInfo.SecondVersion = strArray[1];
        VersionInfo.ThirdVersion = strArray[2];
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "解析版本号出错：" + ex.Message);
      }
    }

    private void ReceiveLogon(InputStreamConvert reader)
    {
      LogonVO logonVo = new LogonVO();
      CMDLogonVO.getObj(reader);
    }
  }
}
