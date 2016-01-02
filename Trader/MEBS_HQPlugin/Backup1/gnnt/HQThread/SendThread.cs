// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.HQThread.SendThread
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel.DataVO;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
  public class SendThread : MYThread
  {
    private ManualResetEvent Event = new ManualResetEvent(true);
    private ArrayList aPacket = ArrayList.Synchronized(new ArrayList());
    private bool isFirstCon = true;
    protected DateTime m_calCodeTable;
    protected DateTime m_calIndex;
    private HQClientMain m_client;
    private DateTime datetimeBill;
    private DateTime datetimeClassSort;
    private DateTime datetimeQuote;
    private DateTime datetimeMinData;
    private DateTime datetimeMarketSort;
    private ProxySocket proxySocket;

    public SendThread(HQClientMain client)
    {
      this.m_client = client;
      this.threadName = "发送线程";
    }

    public void AskForData(CMDVO packet)
    {
      lock (this)
      {
        if (packet != null)
        {
          switch (packet.getCmd())
          {
            case (byte) 4:
              if ((DateTime.Now - this.datetimeQuote).TotalMilliseconds >= 100.0)
              {
                this.aPacket.Add((object) packet);
                this.datetimeQuote = DateTime.Now;
                break;
              }
              break;
            case (byte) 5:
              if ((DateTime.Now - this.datetimeClassSort).TotalMilliseconds >= 100.0)
              {
                this.aPacket.Add((object) packet);
                this.datetimeClassSort = DateTime.Now;
                Logger.wirte(MsgType.Information, string.Concat(new object[4]
                {
                  (object) "---------cmd:",
                  (object) packet.getCmd(),
                  (object) "------",
                  (object) this.datetimeClassSort.ToString("o")
                }));
                break;
              }
              break;
            case (byte) 6:
              if ((DateTime.Now - this.datetimeMinData).TotalMilliseconds >= 100.0)
              {
                this.aPacket.Add((object) packet);
                this.datetimeMinData = DateTime.Now;
                break;
              }
              break;
            case (byte) 7:
              if ((DateTime.Now - this.datetimeBill).TotalMilliseconds >= 100.0)
              {
                this.aPacket.Add((object) packet);
                this.datetimeBill = DateTime.Now;
                Logger.wirte(MsgType.Information, "------------" + (object) packet.getCmd() + this.datetimeBill.ToLongTimeString());
                break;
              }
              break;
            case (byte) 10:
              if ((DateTime.Now - this.datetimeMarketSort).TotalMilliseconds >= 100.0)
              {
                this.aPacket.Add((object) packet);
                this.datetimeMarketSort = DateTime.Now;
                break;
              }
              break;
            default:
              this.aPacket.Add((object) packet);
              break;
          }
          int local_0 = this.aPacket.Count;
          int local_1 = 5;
          if (local_0 > local_1)
          {
            for (int local_2 = 0; local_2 < local_0 - local_1; ++local_2)
            {
              Logger.wirte(MsgType.Information, "AskForDataRemove:" + (object) ((CMDVO) this.aPacket[0]).getCmd());
              this.aPacket.RemoveAt(0);
            }
          }
        }
        this.Event.Set();
      }
    }

    protected override void run()
    {
      try
      {
        while (this.m_client != null && !this.blnIsStopped)
        {
          this.Event.WaitOne();
          this.mUnique.WaitOne();
          this.mUnique.ReleaseMutex();
          if (this.m_client.m_socket == null || !this.m_client.m_socket.Connected)
          {
            if (!this.ConnectToServer(7168))
              Thread.Sleep(5000);
          }
          else
          {
            Thread.Sleep(100);
            TimeSpan timeSpan = new TimeSpan();
            if (this.m_client.m_bShowIndexAtBottom == 1 && this.m_client.indexMainCode.Length > 0)
            {
              timeSpan = DateTime.Now.Subtract(this.m_calIndex);
              if (this.m_calIndex.Year == 1 || timeSpan.Seconds >= 10)
              {
                this.AskForIndex();
                this.m_calIndex = DateTime.Now;
              }
            }
            timeSpan = DateTime.Now.Subtract(this.m_calCodeTable);
            if (timeSpan.Minutes >= 1)
            {
              this.AskForDateAndCodeTable();
              this.m_calCodeTable = DateTime.Now;
            }
            if (this.aPacket.Count > 0)
            {
              CMDVO cmd = (CMDVO) this.aPacket[0];
              this.aPacket.RemoveAt(0);
              try
              {
                switch (cmd.getCmd())
                {
                  case (byte) 4:
                    Logger.wirte(MsgType.Information, "Send cmd:" + (object) cmd.getCmd() + "-" + ((CMDQuoteVO) cmd).codeList[0, 0] + "-" + ((CMDQuoteVO) cmd).codeList[0, 1]);
                    break;
                  case (byte) 5:
                    CMDSortVO cmdSortVo = (CMDSortVO) cmd;
                    Logger.wirte(MsgType.Information, "Send cmd:" + (object) cmd.getCmd() + " sort by " + (string) (object) cmdSortVo.sortBy + " start:" + (string) (object) cmdSortVo.start + " end:" + (string) (object) cmdSortVo.end);
                    break;
                  default:
                    Logger.wirte(MsgType.Information, "Send cmd:" + (object) cmd.getCmd());
                    break;
                }
                RequestUtil.SendRequest(cmd, this.m_client.m_socket);
              }
              catch (Exception ex)
              {
                Logger.wirte(MsgType.Error, ex.ToString());
                this.m_client.m_socket = (Socket) null;
              }
            }
            else
              this.Event.Reset();
          }
        }
        Logger.wirte(MsgType.Information, "**********" + this.threadName + "结束！！！**********");
      }
      catch (ThreadAbortException ex)
      {
        Logger.wirte(MsgType.Error, (string) ex.ExceptionState);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "发送线程run方法异常：" + ex.Message);
      }
    }

    protected override void disposeThread()
    {
      this.Event.Set();
      try
      {
        if (this.proxySocket != null)
        {
          this.proxySocket.Close();
          this.proxySocket = (ProxySocket) null;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      this.Abort("**********强行中断" + this.threadName + "！！！**********");
    }

    public bool ConnectToServer(int receiveBufferSize)
    {
      HQForm hqForm = this.m_client.m_hqForm;
      this.m_client.Connected = false;
      hqForm.RepaintBottom();
      try
      {
        IPEndPoint ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(this.m_client.strSocketIP)[0], this.m_client.iSocketPort);
        this.proxySocket = new ProxySocket();
        if (receiveBufferSize > 0)
          this.proxySocket.ReceiveBufferSize = receiveBufferSize;
        this.m_client.m_socket = this.proxySocket.GetSocket((EndPoint) ipEndPoint);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "ConnectToServer:" + ex.ToString());
        return false;
      }
      Logger.wirte(MsgType.Information, "**************连接成功**************");
      this.m_client.Connected = true;
      if (!this.isFirstCon)
        hqForm.ReQueryCurClient();
      this.isFirstCon = false;
      hqForm.RepaintBottom();
      if (this.m_client.m_socket == null)
        Logger.wirte(MsgType.Information, "Socket is null");
      this.m_calCodeTable = DateTime.Now;
      return true;
    }

    private void AskForDateAndCodeTable()
    {
      try
      {
        RequestUtil.SendRequest((CMDVO) new CMDDateVO(), this.m_client.m_socket);
        RequestUtil.SendRequest((CMDVO) new CMDProductInfoVO()
        {
          date = this.m_client.m_iCodeDate,
          time = this.m_client.m_iCodeTime
        }, this.m_client.m_socket);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
        this.m_client.m_socket = (Socket) null;
      }
    }

    public void AskForServerVersion()
    {
      try
      {
        RequestUtil.SendRequest((CMDVO) new CMDVersionVO(), this.m_client.m_socket);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
        this.m_client.m_socket = (Socket) null;
      }
    }

    public void AskForIndex()
    {
      if (this.m_client.indexMainCode.Length == 0)
        return;
      CommodityInfo commodityInfo = CommodityInfo.DealCode(this.m_client.indexMainCode);
      ProductData productData = this.m_client.GetProductData(commodityInfo);
      DateTime dateTime = new DateTime();
      if (productData != null && productData.realData != null)
        dateTime = productData.realData.time;
      CMDQuoteVO cmdQuoteVo = new CMDQuoteVO();
      cmdQuoteVo.codeList = new string[1, 3];
      cmdQuoteVo.codeList[0, 0] = commodityInfo.commodityCode;
      cmdQuoteVo.codeList[0, 2] = commodityInfo.marketID;
      cmdQuoteVo.isAll = (byte) 1;
      if (dateTime.Year != 1)
      {
        int num = dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second;
        cmdQuoteVo.codeList[0, 1] = num.ToString();
      }
      else
        cmdQuoteVo.codeList[0, 1] = "0";
      try
      {
        RequestUtil.SendRequest((CMDVO) cmdQuoteVo, this.m_client.m_socket);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
        this.m_client.m_socket = (Socket) null;
      }
    }

    public static void AskForRealQuote(string marketID, string code, DateTime time, SendThread sendThread)
    {
      CMDQuoteVO cmdQuoteVo = new CMDQuoteVO();
      cmdQuoteVo.codeList = new string[1, 3];
      cmdQuoteVo.codeList[0, 0] = code;
      cmdQuoteVo.codeList[0, 2] = marketID;
      cmdQuoteVo.isAll = (byte) 1;
      if (time.Year != 1)
      {
        long num = (long) ((time.Year * 10000 + time.Month * 100 + time.Day) * 10000 + (time.Hour * 10000 + time.Minute * 100 + time.Second));
        cmdQuoteVo.codeList[0, 1] = num.ToString();
      }
      else
        cmdQuoteVo.codeList[0, 1] = "0";
      sendThread.AskForData((CMDVO) cmdQuoteVo);
    }
  }
}
