using Gnnt.MEBS.HQModel.OutInfo;
using Gnnt.MEBS.HQModel.Service.IO;
using Http;
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
		public const byte CMD_HEARTBEAT = 0;
		public const byte CMD_LOGON = 1;
		public const byte CMD_VERIFYVERSION = 2;
		public const byte CMD_CODELIST = 3;
		public const byte CMD_QUOTE = 4;
		public const byte CMD_CLASS_SORT = 5;
		public const byte CMD_MIN_DATA = 6;
		public const byte CMD_BILL_DATA = 7;
		public const byte CMD_TRADETIME = 8;
		public const byte CMD_DATE = 9;
		public const byte CMD_MARKET_SORT = 10;
		public const byte CMD_BYCONDITION = 11;
		public const byte CMD_MIN_LINE_INTERVAL = 12;
		public const byte CMD_SETCURPAGE = 13;
		public const byte CMD_UPDATETRADETIME = 14;
		public const byte CMD_INDUSTRYDATA = 15;
		public const byte CMD_REGIONATA = 16;
		public const byte CMD_MARKETINFOLIST = 17;
		public static void SendRequest(CMDVO cmd, Socket socket)
		{
			try
			{
				if (socket == null || !socket.Connected)
				{
					throw new Exception("没有建立连接，请检查套接字是否为空或者套接字是否连接成功！！！");
				}
				NetworkStream networkStream = new NetworkStream(socket);
				BinaryWriter outer = new BinaryWriter(new BufferedStream(networkStream));
				OutputStreamConvert outputStreamConvert = new OutputStreamConvert(outer);
				switch (cmd.cmd)
				{
				case 0:
					outputStreamConvert.WriteJavaByte(0);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				case 1:
				{
					CMDLogonVO cMDLogonVO = (CMDLogonVO)cmd;
					outputStreamConvert.WriteJavaByte(1);
					outputStreamConvert.WriteJavaUTF(cMDLogonVO.name);
					outputStreamConvert.WriteJavaUTF(cMDLogonVO.password);
					outputStreamConvert.WriteJavaUTF(cMDLogonVO.key);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 2:
					outputStreamConvert.WriteJavaByte(2);
					outputStreamConvert.WriteJavaUTF(Application.ProductVersion);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				case 3:
				{
					CMDProductInfoVO cMDProductInfoVO = (CMDProductInfoVO)cmd;
					outputStreamConvert.WriteJavaByte(3);
					outputStreamConvert.WriteJavaInt(cMDProductInfoVO.date);
					outputStreamConvert.WriteJavaInt(cMDProductInfoVO.time);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 4:
				{
					CMDQuoteVO cMDQuoteVO = (CMDQuoteVO)cmd;
					outputStreamConvert.WriteJavaByte(4);
					outputStreamConvert.WriteJavaByte(cMDQuoteVO.isAll);
					outputStreamConvert.WriteJavaInt(cMDQuoteVO.codeList.GetLength(0));
					for (int i = 0; i < cMDQuoteVO.codeList.GetLength(0); i++)
					{
						outputStreamConvert.WriteJavaUTF(cMDQuoteVO.codeList[i, 0]);
						outputStreamConvert.WriteJavaUTF(cMDQuoteVO.codeList[i, 1]);
						outputStreamConvert.WriteJavaUTF(cMDQuoteVO.codeList[i, 2]);
					}
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 5:
				{
					CMDSortVO cMDSortVO = (CMDSortVO)cmd;
					outputStreamConvert.WriteJavaByte(5);
					outputStreamConvert.WriteJavaByte(cMDSortVO.sortBy);
					outputStreamConvert.WriteJavaByte(cMDSortVO.isDescend);
					outputStreamConvert.WriteJavaInt(cMDSortVO.start);
					outputStreamConvert.WriteJavaInt(cMDSortVO.end);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 6:
				{
					CMDMinVO cMDMinVO = (CMDMinVO)cmd;
					outputStreamConvert.WriteJavaByte(6);
					outputStreamConvert.WriteJavaByte((byte)cMDMinVO.commidityList.Count);
					outputStreamConvert.WriteJavaByte(cMDMinVO.mark);
					for (int j = 0; j < cMDMinVO.commidityList.Count; j++)
					{
						outputStreamConvert.WriteJavaUTF(cMDMinVO.commidityList[j].code);
						outputStreamConvert.WriteJavaUTF(cMDMinVO.commidityList[j].marketID);
						outputStreamConvert.WriteJavaByte(cMDMinVO.commidityList[j].location);
					}
					outputStreamConvert.WriteJavaByte(cMDMinVO.type);
					outputStreamConvert.WriteJavaInt(cMDMinVO.date);
					outputStreamConvert.WriteJavaInt(cMDMinVO.time);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 7:
				{
					CMDBillByVersionVO cMDBillByVersionVO = (CMDBillByVersionVO)cmd;
					outputStreamConvert.WriteJavaByte(7);
					outputStreamConvert.WriteJavaUTF(cMDBillByVersionVO.marketID);
					outputStreamConvert.WriteJavaUTF(cMDBillByVersionVO.code);
					outputStreamConvert.WriteJavaByte(cMDBillByVersionVO.type);
					outputStreamConvert.WriteJavaLong(cMDBillByVersionVO.totalAmount);
					outputStreamConvert.WriteJavaLong(cMDBillByVersionVO.time);
					outputStreamConvert.WriteJavaUTF(cMDBillByVersionVO.ReservedField);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 8:
					outputStreamConvert.WriteJavaByte(8);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				case 9:
					outputStreamConvert.WriteJavaByte(9);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				case 10:
				{
					CMDMarketSortVO cMDMarketSortVO = (CMDMarketSortVO)cmd;
					outputStreamConvert.WriteJavaByte(10);
					outputStreamConvert.WriteJavaInt(cMDMarketSortVO.num);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 11:
				{
					CMDByConditionVO cMDByConditionVO = (CMDByConditionVO)cmd;
					outputStreamConvert.WriteJavaByte(11);
					outputStreamConvert.WriteJavaInt(cMDByConditionVO.type);
					outputStreamConvert.WriteJavaLong(cMDByConditionVO.value);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				case 12:
					outputStreamConvert.WriteJavaByte(12);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				case 13:
				{
					CMDSetCurPage cMDSetCurPage = (CMDSetCurPage)cmd;
					outputStreamConvert.WriteJavaByte(13);
					outputStreamConvert.WriteJavaInt(cMDSetCurPage.curPage);
					networkStream.Flush();
					outputStreamConvert.Flush();
					outputStreamConvert.Close();
					break;
				}
				}
			}
			catch (IOException ex)
			{
				Logger.wirte(3, ex.ToString());
			}
		}
		public static byte[] getRepoent(string url)
		{
			HttpSocket httpSocket = null;
			Stream stream = null;
			BinaryReader binaryReader = null;
			MemoryStream memoryStream = null;
			try
			{
				WebClient webClient = new WebClient();
				IniFile iniFile = new IniFile("Proxy.ini");
				try
				{
					if (iniFile.IniReadValue("ProxyServer", "Enable") != "True")
					{
						webClient.Proxy = null;
					}
				}
				catch
				{
					webClient.Proxy = null;
				}
				stream = webClient.OpenRead(url);
				binaryReader = new BinaryReader(stream);
				memoryStream = new MemoryStream();
				byte[] array = new byte[1];
				while (binaryReader.Read(array, 0, array.Length) > 0)
				{
					memoryStream.Write(array, 0, array.Length);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
			if (stream != null)
			{
				stream.Flush();
			}
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
			if (binaryReader != null)
			{
				binaryReader.Close();
			}
			if (stream != null)
			{
				stream.Close();
			}
			if (httpSocket != null)
			{
				httpSocket.close();
			}
			return memoryStream.GetBuffer();
		}
		public static ProductInfoListVO getProductInfoList(string url)
		{
			MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
			GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
			BinaryReader binaryReader = new BinaryReader(gZipStream);
			InputStreamConvert inputStreamConvert = new InputStreamConvert(binaryReader);
			inputStreamConvert.ReadJavaByte();
			ProductInfoListVO obj = CMDProductInfoVO.getObj(inputStreamConvert);
			binaryReader.Close();
			gZipStream.Close();
			memoryStream.Close();
			return obj;
		}
		public static MarketInfoListVO getMarketInfoList(string url)
		{
			MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
			GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
			BinaryReader binaryReader = new BinaryReader(gZipStream);
			InputStreamConvert inputStreamConvert = new InputStreamConvert(binaryReader);
			inputStreamConvert.ReadJavaByte();
			MarketInfoListVO obj = CMDMarketInfoVO.getObj(inputStreamConvert);
			binaryReader.Close();
			gZipStream.Close();
			memoryStream.Close();
			return obj;
		}
	}
}
