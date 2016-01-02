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
		public const int TYPE_CODELIST = 0;
		public const int TYPE_OTHER = 1;
		public const int TYPE_XMLClass = 2;
		private ManualResetEvent Event = new ManualResetEvent(true);
		private HQClientMain m_Client;
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
		public HttpThread(int type, HQClientMain Client)
		{
			this.threadName = "HTTP线程" + type;
			this.m_Client = Client;
			this.m_hqForm = this.m_Client.m_hqForm;
			this.pluginInfo = this.m_Client.pluginInfo;
			this.setInfo = this.m_Client.setInfo;
			this.buttonUtils = Client.buttonUtils;
			this.iType = type;
			if (this.iType == 1)
			{
				this.aPacket = ArrayList.Synchronized(new ArrayList());
			}
		}
		public void AskForData(Packet_HttpRequest packet)
		{
			Monitor.Enter(this);
			try
			{
				if (packet != null)
				{
					this.aPacket.Add(packet);
					int count = this.aPacket.Count;
					if (count > 1)
					{
						for (int i = 0; i < count - 1; i++)
						{
							this.aPacket.RemoveAt(i);
						}
					}
				}
				this.Event.Set();
			}
			finally
			{
				Monitor.Exit(this);
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
				else
				{
					if (2 == this.iType)
					{
						if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
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
			while (this.m_Client != null && !this.blnIsStopped && !flag)
			{
				this.mUnique.WaitOne();
				this.mUnique.ReleaseMutex();
				try
				{
					ProductInfoListVO productInfoList = RequestUtil.getProductInfoList(this.m_Client.strURLPath + "data/productinfo.dat");
					Logger.wirte(MsgType.Information, string.Concat(new object[]
					{
						"码表时间:",
						productInfoList.date,
						" ",
						productInfoList.time
					}));
					this.m_Client.m_iCodeDate = productInfoList.date;
					this.m_Client.m_iCodeTime = productInfoList.time;
					ProductInfoVO[] productInfos = productInfoList.productInfos;
					this.m_Client.m_codeList.Clear();
					this.m_Client.m_htProduct.Clear();
					for (int i = 0; i < productInfos.Length; i++)
					{
						CodeTable codeTable = new CodeTable();
						codeTable.marketID = productInfos[i].marketID;
						codeTable.sName = productInfos[i].name;
						string text = (string)this.pluginInfo.HTConfig["Market" + codeTable.marketID];
						if (text == null || text.Length == 0)
						{
							text = codeTable.marketID;
						}
						if (this.m_hqForm.AddMarketName)
						{
							codeTable.sName = text + codeTable.sName;
						}
						codeTable.sPinyin = productInfos[i].pyName;
						codeTable.status = productInfos[i].status;
						codeTable.tradeSecNo = productInfos[i].tradeSecNo;
						codeTable.mPrice = productInfos[i].mPrice;
						codeTable.fUnit = productInfos[i].fUnit;
						if (this.m_Client.m_htProduct != null && !this.m_Client.m_htProduct.ContainsKey(productInfos[i].marketID + productInfos[i].code))
						{
							this.m_Client.m_htProduct.Add(productInfos[i].marketID + productInfos[i].code, codeTable);
							CommodityInfo value = new CommodityInfo(productInfos[i].marketID, productInfos[i].code, productInfos[i].region, productInfos[i].industry);
							this.m_Client.m_codeList.Add(value);
							if (productInfos[i].status == 1)
							{
								this.m_Client.hm_codeList.Add(value);
							}
							else
							{
								this.m_Client.nm_codeList.Add(value);
							}
						}
						else
						{
							Logger.wirte(MsgType.Warning, "HTTP取码表出现重复数据，将出现Hashtable重复键加入！商品代码" + productInfos[i].code);
						}
						if (codeTable.status == 3 && this.m_Client.indexMainCode.Length == 0)
						{
							this.m_Client.indexMainCode = productInfos[i].marketID + "_" + productInfos[i].code;
						}
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
					catch (Exception ex2)
					{
						Logger.wirte(MsgType.Error, ex2.ToString());
					}
				}
			}
		}
		private void GetMarketList()
		{
			bool flag = false;
			while (this.m_Client != null && !this.blnIsStopped && !flag)
			{
				this.mUnique.WaitOne();
				this.mUnique.ReleaseMutex();
				try
				{
					MarketInfoListVO marketInfoList = RequestUtil.getMarketInfoList(this.m_Client.strURLPath + "data/marketinfo.dat");
					MarketInfoVO[] marketInfos = marketInfoList.marketInfos;
					for (int i = 0; i < marketInfos.Length; i++)
					{
						if (this.m_Client.m_htMarketData.ContainsKey(marketInfos[i].marketID))
						{
							((MarketDataVO)this.m_Client.m_htMarketData[marketInfos[i].marketID]).marketName = marketInfos[i].marketName;
						}
						else
						{
							MarketDataVO marketDataVO = new MarketDataVO();
							marketDataVO.marketID = marketInfos[i].marketID;
							marketDataVO.marketName = marketInfos[i].marketName;
							this.m_Client.m_htMarketData.Add(marketInfos[i].marketID, marketDataVO);
						}
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
					catch (Exception ex2)
					{
						Logger.wirte(MsgType.Error, ex2.ToString());
					}
				}
			}
		}
		private void GetXMLClass()
		{
			bool flag = false;
			while (this.m_Client != null && !this.blnIsStopped && !flag)
			{
				this.mUnique.WaitOne();
				this.mUnique.ReleaseMutex();
				try
				{
					MemoryStream inStream = new MemoryStream(RequestUtil.getRepoent(this.m_Client.strURLPath + "data/CreateXMLClass.xml"));
					this.m_Client.commodityClass = new CommodityClass(inStream);
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
					catch (Exception ex2)
					{
						Logger.wirte(MsgType.Error, ex2.ToString());
					}
				}
			}
		}
		public void RemakeButton()
		{
			try
			{
				while (this.m_hqForm.MainGraph == null)
				{
					Thread.Sleep(100);
				}
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					this.AddMarketButton();
				}
				else
				{
					this.AddClassButton();
				}
				this.buttonUtils.isTidyBtnFlag++;
				if (this.buttonUtils.isTidyBtnFlag == 2)
				{
					this.buttonUtils.TidyButtons(this.buttonUtils.ButtonList);
				}
				if (this.m_Client.CurrentPage == 0)
				{
					this.m_hqForm.Repaint();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "RemakeButton异常：" + ex.Message);
			}
		}
		public void AddMarketButton()
		{
			if (!this.isEndAddMarketBtn && this.m_Client.m_htMarketData != null)
			{
				bool selected = false;
				foreach (DictionaryEntry dictionaryEntry in this.m_Client.m_htMarketData)
				{
					MarketDataVO marketDataVO = (MarketDataVO)dictionaryEntry.Value;
					if (this.buttonUtils.InitialButtonName.StartsWith("Market"))
					{
						string text = this.buttonUtils.InitialButtonName.Substring(6);
						if (text.Equals(dictionaryEntry.Key.ToString()))
						{
							selected = true;
						}
					}
					this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count, new MyButton("Market" + dictionaryEntry.Key.ToString(), marketDataVO.marketName, selected), this.buttonUtils.ButtonList);
					selected = false;
				}
				this.isEndAddMarketBtn = true;
			}
		}
		public void AddClassButton()
		{
			if (!this.isEndAddClassBtn && this.m_Client.commodityClass != null)
			{
				for (int i = 0; i < this.m_Client.commodityClass.classList.Count; i++)
				{
					ClassVO classVO = (ClassVO)this.m_Client.commodityClass.classList[i];
					if (this.buttonUtils.InitialButtonName.Equals("C" + classVO.classID))
					{
						if (this.buttonUtils.ButtonList.Count > 0)
						{
							this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count - 1, new MyButton("C" + classVO.classID, classVO.name, true), this.buttonUtils.ButtonList);
						}
						else
						{
							this.buttonUtils.InsertButton(0, new MyButton("C" + classVO.classID, classVO.name, true), this.buttonUtils.ButtonList);
						}
					}
					else
					{
						if (this.buttonUtils.ButtonList.Count > 0)
						{
							this.buttonUtils.InsertButton(this.buttonUtils.ButtonList.Count - 1, new MyButton("C" + classVO.classID, classVO.name, false), this.buttonUtils.ButtonList);
						}
						else
						{
							this.buttonUtils.InsertButton(0, new MyButton("C" + classVO.classID, classVO.name, false), this.buttonUtils.ButtonList);
						}
					}
				}
				this.isEndAddClassBtn = true;
			}
		}
		private void GetHttpData()
		{
			while (this.m_Client != null && !this.blnIsStopped)
			{
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
					Packet_HttpRequest packet_HttpRequest = (Packet_HttpRequest)this.aPacket[count - 1];
					this.aPacket.RemoveAt(count - 1);
					switch (packet_HttpRequest.type)
					{
					case 0:
						this.GetDayLine(packet_HttpRequest);
						break;
					case 1:
						this.Get5MinLine(packet_HttpRequest);
						break;
					case 2:
						this.Get1MinLine(packet_HttpRequest);
						break;
					}
				}
				else
				{
					this.Event.Reset();
				}
			}
		}
		public static KLineData[] GetHistoryData(string url)
		{
			KLineData[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream(RequestUtil.getRepoent(url));
				GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
				BinaryReader binaryReader = new BinaryReader(gZipStream);
				InputStreamConvert inputStreamConvert = new InputStreamConvert(binaryReader);
				KLineData[] array = new KLineData[inputStreamConvert.ReadJavaInt()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new KLineData();
					int num = inputStreamConvert.ReadJavaInt();
					if (string.Concat(num).Length > 6)
					{
						array[i].date = 199700000000L + (long)num;
					}
					else
					{
						array[i].date = (long)(num + 19970000);
					}
					array[i].openPrice = inputStreamConvert.ReadJavaFloat();
					array[i].highPrice = inputStreamConvert.ReadJavaFloat();
					array[i].lowPrice = inputStreamConvert.ReadJavaFloat();
					array[i].closePrice = inputStreamConvert.ReadJavaFloat();
					array[i].balancePrice = inputStreamConvert.ReadJavaFloat();
					array[i].totalAmount = inputStreamConvert.ReadJavaLong();
					array[i].totalMoney = (double)inputStreamConvert.ReadJavaFloat();
					array[i].reserveCount = inputStreamConvert.ReadJavaInt();
				}
				binaryReader.Close();
				gZipStream.Close();
				memoryStream.Close();
				result = array;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "GetHistoryData异常：" + ex.Message);
				result = null;
			}
			return result;
		}
		public static KLineData[] GetLocalHistoryData(string url)
		{
			KLineData[] result;
			try
			{
				FileStream input = new FileStream(url, FileMode.Open);
				BinaryReader binaryReader = new BinaryReader(input);
				InputStreamConvert inputStreamConvert = new InputStreamConvert(binaryReader);
				KLineData[] array = new KLineData[inputStreamConvert.ReadJavaInt()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new KLineData();
					int num = inputStreamConvert.ReadJavaInt();
					if (string.Concat(num).Length > 6)
					{
						array[i].date = 199700000000L + (long)num;
					}
					else
					{
						array[i].date = (long)(num + 19970000);
					}
					array[i].openPrice = inputStreamConvert.ReadJavaFloat();
					array[i].highPrice = inputStreamConvert.ReadJavaFloat();
					array[i].lowPrice = inputStreamConvert.ReadJavaFloat();
					array[i].closePrice = inputStreamConvert.ReadJavaFloat();
					array[i].balancePrice = inputStreamConvert.ReadJavaFloat();
					array[i].totalAmount = inputStreamConvert.ReadJavaLong();
					array[i].totalMoney = (double)inputStreamConvert.ReadJavaFloat();
					array[i].reserveCount = inputStreamConvert.ReadJavaInt();
				}
				binaryReader.Close();
				result = array;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "GetLocalHistoryData异常：" + ex.Message);
				result = null;
			}
			return result;
		}
		private void GetDayLine(Packet_HttpRequest request)
		{
			try
			{
				bool flag = false;
				string text = string.Concat(new string[]
				{
					this.m_Client.strURLPath,
					"data/day/",
					request.marketID,
					request.sCode.Trim(),
					".day.zip"
				});
				string text2 = string.Concat(new string[]
				{
					this.pluginInfo.ConfigPath,
					"data/day/",
					request.marketID,
					"/",
					request.marketID,
					request.sCode.Trim(),
					".day.zip"
				});
				if (!Directory.Exists(text2.Remove(text2.LastIndexOf('/'))))
				{
					Directory.CreateDirectory(text2.Remove(text2.LastIndexOf('/')));
				}
				string path = text2.Remove(text2.LastIndexOf('.'));
				if (!File.Exists(path) || File.GetLastWriteTime(path).Day != DateTime.Now.Day || File.GetLastWriteTime(path).Month != DateTime.Now.Month)
				{
					Logger.wirte(MsgType.Information, "Get Day : " + text);
					if (this.DownloadFile(text, text2) && this.UnZipFile(text2))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(text2.Remove(text2.LastIndexOf('.')));
					ProductData productData = this.m_Client.GetProductData(request.marketID, request.sCode);
					if (productData == null)
					{
						if (this.m_Client.aProductData.Count > 50)
						{
							this.m_Client.aProductData.RemoveAt(50);
						}
						productData = new ProductData();
						productData.commodityInfo = new CommodityInfo(request.marketID, request.sCode);
						productData.dayKLine = localHistoryData;
						this.m_Client.aProductData.Insert(0, productData);
					}
					else
					{
						productData.dayKLine = localHistoryData;
					}
					if (localHistoryData.Length > 0 && 2 == this.m_Client.CurrentPage && this.m_Client.curCommodityInfo.marketID.Equals(request.marketID) && this.m_Client.curCommodityInfo.commodityCode.Equals(request.sCode))
					{
						this.m_hqForm.Repaint();
					}
				}
			}
			catch (UriFormatException ex)
			{
				Logger.wirte(MsgType.Error, "UriFormatException" + ex.ToString());
			}
			catch (IOException ex2)
			{
				Logger.wirte(MsgType.Error, "IOException" + ex2.ToString());
			}
			catch (Exception ex3)
			{
				Logger.wirte(MsgType.Error, "Exception" + ex3.ToString());
			}
		}
		private void Get5MinLine(Packet_HttpRequest request)
		{
			try
			{
				bool flag = false;
				string text = string.Concat(new string[]
				{
					this.m_Client.strURLPath,
					"data/5min/",
					request.marketID,
					request.sCode.Trim(),
					".5min.zip"
				});
				string text2 = string.Concat(new string[]
				{
					this.pluginInfo.ConfigPath,
					"data/5min/",
					request.marketID,
					"/",
					request.marketID,
					request.sCode.Trim(),
					".5min.zip"
				});
				if (!Directory.Exists(text2.Remove(text2.LastIndexOf('/'))))
				{
					Directory.CreateDirectory(text2.Remove(text2.LastIndexOf('/')));
				}
				string path = text2.Remove(text2.LastIndexOf('.'));
				if (!File.Exists(path) || File.GetLastWriteTime(path).Day != DateTime.Now.Day || File.GetLastWriteTime(path).Month != DateTime.Now.Month)
				{
					Logger.wirte(MsgType.Information, "Get 5Min : " + text);
					if (this.DownloadFile(text, text2) && this.UnZipFile(text2))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(text2.Remove(text2.LastIndexOf('.')));
					CommodityInfo commodityInfo = new CommodityInfo(request.marketID, request.sCode);
					ProductData productData = this.m_Client.GetProductData(commodityInfo);
					if (productData == null)
					{
						if (this.m_Client.aProductData.Count > 50)
						{
							this.m_Client.aProductData.RemoveAt(50);
						}
						productData = new ProductData();
						productData.commodityInfo = commodityInfo;
						productData.min5KLine = localHistoryData;
						this.m_Client.aProductData.Insert(0, productData);
					}
					else
					{
						productData.min5KLine = localHistoryData;
					}
					for (int i = 0; i < productData.min5KLine.Length; i++)
					{
						if (productData.min5KLine[i].balancePrice <= 0f)
						{
							if (productData.min5KLine[i].totalAmount > 0L)
							{
								productData.min5KLine[i].balancePrice = (float)(productData.min5KLine[i].totalMoney / (double)productData.min5KLine[i].totalAmount);
							}
							else
							{
								if (i > 0)
								{
									productData.min5KLine[i].balancePrice = productData.min5KLine[i - 1].balancePrice;
								}
								else
								{
									productData.min5KLine[i].balancePrice = productData.min5KLine[i].closePrice;
								}
							}
						}
					}
					if (localHistoryData.Length > 0 && 2 == this.m_Client.CurrentPage && this.m_Client.curCommodityInfo.Compare(commodityInfo))
					{
						this.m_hqForm.Repaint();
					}
				}
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
				string text = string.Concat(new string[]
				{
					this.m_Client.strURLPath,
					"data/min/",
					request.marketID,
					request.sCode.Trim(),
					".min.zip"
				});
				string text2 = string.Concat(new string[]
				{
					this.pluginInfo.ConfigPath,
					"data/min/",
					request.marketID,
					"/",
					request.marketID,
					request.sCode.Trim(),
					".min.zip"
				});
				if (!Directory.Exists(text2.Remove(text2.LastIndexOf('/'))))
				{
					Directory.CreateDirectory(text2.Remove(text2.LastIndexOf('/')));
				}
				string path = text2.Remove(text2.LastIndexOf('.'));
				if (!File.Exists(path) || File.GetLastWriteTime(path).Day != DateTime.Now.Day || File.GetLastWriteTime(path).Month != DateTime.Now.Month)
				{
					Logger.wirte(MsgType.Information, "Get 1Min : " + text);
					if (this.DownloadFile(text, text2) && this.UnZipFile(text2))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					KLineData[] localHistoryData = HttpThread.GetLocalHistoryData(text2.Remove(text2.LastIndexOf('.')));
					CommodityInfo commodityInfo = new CommodityInfo(request.marketID, request.sCode);
					ProductData productData = this.m_Client.GetProductData(commodityInfo);
					if (productData == null)
					{
						if (this.m_Client.aProductData.Count > 50)
						{
							this.m_Client.aProductData.RemoveAt(50);
						}
						productData = new ProductData();
						productData.commodityInfo = commodityInfo;
						productData.min1KLine = localHistoryData;
						this.m_Client.aProductData.Insert(0, productData);
					}
					else
					{
						productData.min1KLine = localHistoryData;
					}
					for (int i = 0; i < productData.min1KLine.Length; i++)
					{
						if (productData.min1KLine[i].balancePrice <= 0f)
						{
							if (productData.min1KLine[i].totalAmount > 0L)
							{
								productData.min1KLine[i].balancePrice = (float)(productData.min1KLine[i].totalMoney / (double)productData.min1KLine[i].totalAmount);
							}
							else
							{
								if (i > 0)
								{
									productData.min1KLine[i].balancePrice = productData.min1KLine[i - 1].balancePrice;
								}
								else
								{
									productData.min1KLine[i].balancePrice = productData.min1KLine[i].closePrice;
								}
							}
						}
					}
					if (localHistoryData.Length > 0 && 2 == this.m_Client.CurrentPage && this.m_Client.curCommodityInfo.Compare(commodityInfo))
					{
						this.m_hqForm.Repaint();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.ToString());
			}
		}
		public bool DownloadFile(string StrUrl, string StrFileName)
		{
			FileStream fileStream = null;
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(StrUrl);
				long contentLength = httpWebRequest.GetResponse().ContentLength;
				long num;
				if (File.Exists(StrFileName))
				{
					fileStream = File.OpenWrite(StrFileName);
					num = fileStream.Length;
					fileStream.Seek(num, SeekOrigin.Current);
				}
				else
				{
					fileStream = new FileStream(StrFileName, FileMode.Create);
					num = 0L;
				}
				if (num > 0L)
				{
					httpWebRequest.AddRange((int)num);
				}
				Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
				byte[] buffer = new byte[1024];
				int i = responseStream.Read(buffer, 0, 1024);
				while (i > 0)
				{
					fileStream.Write(buffer, 0, i);
					i = responseStream.Read(buffer, 0, 1024);
					long arg_A7_0 = fileStream.Length;
				}
				fileStream.Close();
				responseStream.Close();
				result = true;
			}
			catch (Exception ex)
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				Logger.wirte(MsgType.Error, "下载文件异常：" + ex.Message);
				result = false;
			}
			return result;
		}
		private bool UnZipFile(string fileName)
		{
			bool result;
			try
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					string path = fileName.Remove(fileName.LastIndexOf('.'));
					using (FileStream fileStream2 = File.Create(path))
					{
						using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
						{
							this.CopyTo(gZipStream, fileStream2);
							Logger.wirte(MsgType.Information, "解压文件：" + fileName + "成功。");
						}
					}
				}
				File.Delete(fileName);
				result = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "解压缩文件：" + fileName + "出错：" + ex.Message);
				result = false;
			}
			return result;
		}
		public void CopyTo(Stream basestream, Stream destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (!basestream.CanRead && !basestream.CanWrite)
			{
				throw new ObjectDisposedException(null, new Exception("输入流不可读写"));
			}
			if (!destination.CanRead && !destination.CanWrite)
			{
				throw new ObjectDisposedException("destination", new Exception("目标流不可读写"));
			}
			if (!basestream.CanRead)
			{
				throw new NotSupportedException("输入流不可读");
			}
			if (!destination.CanWrite)
			{
				throw new NotSupportedException("目标流不可写");
			}
			this.InternalCopyTo(basestream, destination, 4096);
		}
		private void InternalCopyTo(Stream basestream, Stream destination, int bufferSize)
		{
			byte[] array = new byte[bufferSize];
			int count;
			while ((count = basestream.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}
	}
}
