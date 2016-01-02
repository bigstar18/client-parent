// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.RequestUtil
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.OutInfo;
using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class RequestUtil
  {
    public const byte CMD_HEARTBEAT = (byte) 0;
    public const byte CMD_LOGON = (byte) 1;
    public const byte CMD_VERIFYVERSION = (byte) 2;
    public const byte CMD_CODELIST = (byte) 3;
    public const byte CMD_QUOTE = (byte) 4;
    public const byte CMD_CLASS_SORT = (byte) 5;
    public const byte CMD_MIN_DATA = (byte) 6;
    public const byte CMD_BILL_DATA = (byte) 7;
    public const byte CMD_TRADETIME = (byte) 8;
    public const byte CMD_DATE = (byte) 9;
    public const byte CMD_MARKET_SORT = (byte) 10;
    public const byte CMD_BYCONDITION = (byte) 11;
    public const byte CMD_MIN_LINE_INTERVAL = (byte) 12;
    public const byte CMD_SETCURPAGE = (byte) 13;
    public const byte CMD_UPDATETRADETIME = (byte) 14;
    public const byte CMD_INDUSTRYDATA = (byte) 15;
    public const byte CMD_REGIONATA = (byte) 16;
    public const byte CMD_MARKETINFOLIST = (byte) 17;

    public static void SendRequest(CMDVO cmd, Socket socket)
    {
      try
      {
        if (socket == null || !socket.Connected)
          throw new Exception("没有建立连接，请检查套接字是否为空或者套接字是否连接成功！！！");
        NetworkStream networkStream = new NetworkStream(socket);
        OutputStreamConvert outputStreamConvert = new OutputStreamConvert(new BinaryWriter((Stream) new BufferedStream((Stream) networkStream)));
        switch (cmd.cmd)
        {
          case (byte) 0:
            outputStreamConvert.WriteJavaByte((byte) 0);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 1:
            CMDLogonVO cmdLogonVo = (CMDLogonVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 1);
            outputStreamConvert.WriteJavaUTF(cmdLogonVo.name);
            outputStreamConvert.WriteJavaUTF(cmdLogonVo.password);
            outputStreamConvert.WriteJavaUTF(cmdLogonVo.key);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 2:
            outputStreamConvert.WriteJavaByte((byte) 2);
            outputStreamConvert.WriteJavaUTF(Application.ProductVersion);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 3:
            CMDProductInfoVO cmdProductInfoVo = (CMDProductInfoVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 3);
            outputStreamConvert.WriteJavaInt(cmdProductInfoVo.date);
            outputStreamConvert.WriteJavaInt(cmdProductInfoVo.time);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 4:
            CMDQuoteVO cmdQuoteVo = (CMDQuoteVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 4);
            outputStreamConvert.WriteJavaByte(cmdQuoteVo.isAll);
            outputStreamConvert.WriteJavaInt(cmdQuoteVo.codeList.GetLength(0));
            for (int index = 0; index < cmdQuoteVo.codeList.GetLength(0); ++index)
            {
              outputStreamConvert.WriteJavaUTF(cmdQuoteVo.codeList[index, 0]);
              outputStreamConvert.WriteJavaUTF(cmdQuoteVo.codeList[index, 1]);
              outputStreamConvert.WriteJavaUTF(cmdQuoteVo.codeList[index, 2]);
            }
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 5:
            CMDSortVO cmdSortVo = (CMDSortVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 5);
            outputStreamConvert.WriteJavaByte(cmdSortVo.sortBy);
            outputStreamConvert.WriteJavaByte(cmdSortVo.isDescend);
            outputStreamConvert.WriteJavaInt(cmdSortVo.start);
            outputStreamConvert.WriteJavaInt(cmdSortVo.end);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 6:
            CMDMinVO cmdMinVo = (CMDMinVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 6);
            outputStreamConvert.WriteJavaByte((byte) cmdMinVo.commidityList.Count);
            outputStreamConvert.WriteJavaByte(cmdMinVo.mark);
            for (int index = 0; index < cmdMinVo.commidityList.Count; ++index)
            {
              outputStreamConvert.WriteJavaUTF(cmdMinVo.commidityList[index].code);
              outputStreamConvert.WriteJavaUTF(cmdMinVo.commidityList[index].marketID);
              outputStreamConvert.WriteJavaByte(cmdMinVo.commidityList[index].location);
            }
            outputStreamConvert.WriteJavaByte(cmdMinVo.type);
            outputStreamConvert.WriteJavaInt(cmdMinVo.date);
            outputStreamConvert.WriteJavaInt(cmdMinVo.time);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 7:
            CMDBillByVersionVO cmdBillByVersionVo = (CMDBillByVersionVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 7);
            outputStreamConvert.WriteJavaUTF(cmdBillByVersionVo.marketID);
            outputStreamConvert.WriteJavaUTF(cmdBillByVersionVo.code);
            outputStreamConvert.WriteJavaByte(cmdBillByVersionVo.type);
            outputStreamConvert.WriteJavaLong(cmdBillByVersionVo.totalAmount);
            outputStreamConvert.WriteJavaLong(cmdBillByVersionVo.time);
            outputStreamConvert.WriteJavaUTF(cmdBillByVersionVo.ReservedField);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 8:
            outputStreamConvert.WriteJavaByte((byte) 8);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 9:
            outputStreamConvert.WriteJavaByte((byte) 9);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 10:
            CMDMarketSortVO cmdMarketSortVo = (CMDMarketSortVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 10);
            outputStreamConvert.WriteJavaInt(cmdMarketSortVo.num);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 11:
            CMDByConditionVO cmdByConditionVo = (CMDByConditionVO) cmd;
            outputStreamConvert.WriteJavaByte((byte) 11);
            outputStreamConvert.WriteJavaInt(cmdByConditionVo.type);
            outputStreamConvert.WriteJavaLong(cmdByConditionVo.value);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 12:
            outputStreamConvert.WriteJavaByte((byte) 12);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
          case (byte) 13:
            CMDSetCurPage cmdSetCurPage = (CMDSetCurPage) cmd;
            outputStreamConvert.WriteJavaByte((byte) 13);
            outputStreamConvert.WriteJavaInt(cmdSetCurPage.curPage);
            networkStream.Flush();
            outputStreamConvert.Flush();
            outputStreamConvert.Close();
            break;
        }
      }
      catch (IOException ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
    }

    public static byte[] getRepoent(string url)
    {
      //HttpSocket httpSocket = (HttpSocket) null;
      Stream input = (Stream) null;
      BinaryReader binaryReader = (BinaryReader) null;
      MemoryStream memoryStream = (MemoryStream) null;
      try
      {
        WebClient webClient = new WebClient();
        IniFile iniFile = new IniFile("Proxy.ini");
        try
        {
          if (iniFile.IniReadValue("ProxyServer", "Enable") != "True")
            webClient.Proxy = (IWebProxy) null;
        }
        catch
        {
          webClient.Proxy = (IWebProxy) null;
        }
        input = webClient.OpenRead(url);
        binaryReader = new BinaryReader(input);
        memoryStream = new MemoryStream();
        byte[] buffer = new byte[1];
        while (binaryReader.Read(buffer, 0, buffer.Length) > 0)
          memoryStream.Write(buffer, 0, buffer.Length);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.Message);
      }
      if (input != null)
        input.Flush();
      if (memoryStream != null)
      {
        memoryStream.Flush();
        memoryStream.Close();
      }
      if (binaryReader != null)
        binaryReader.Close();
      if (input != null)
        input.Close();
      //if (httpSocket != null)
      //  httpSocket.close();
      return memoryStream.GetBuffer();
    }

    public static ProductInfoListVO getProductInfoList(string url)
    {
      MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
      GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress, true);
      BinaryReader input1 = new BinaryReader((Stream) gzipStream);
      InputStreamConvert input2 = new InputStreamConvert(input1);
      int num = (int) input2.ReadJavaByte();
      ProductInfoListVO productInfoListVo = CMDProductInfoVO.getObj(input2);
      input1.Close();
      gzipStream.Close();
      memoryStream.Close();
      return productInfoListVo;
    }

    public static MarketInfoListVO getMarketInfoList(string url)
    {
      MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
      GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress, true);
      BinaryReader input1 = new BinaryReader((Stream) gzipStream);
      InputStreamConvert input2 = new InputStreamConvert(input1);
      int num = (int) input2.ReadJavaByte();
      MarketInfoListVO marketInfoListVo = CMDMarketInfoVO.getObj(input2);
      input1.Close();
      gzipStream.Close();
      memoryStream.Close();
      return marketInfoListVo;
    }
  }
}
