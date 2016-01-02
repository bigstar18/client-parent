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
		private bool isConnection;
		private HQClientMain m_client;
		private HQForm m_hqForm;
		private string marketName = string.Empty;
		private bool AddButtonFlag;
		private string fristMarketCode = string.Empty;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private ButtonUtils buttonUtils;
		private MultiQuoteData multiQuoteData;
		private bool onceReceiveTime = true;
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
				BinaryReader binaryReader = null;
				InputStreamConvert inputStreamConvert = null;
				while (this.m_client != null && !this.blnIsStopped)
				{
					this.mUnique.WaitOne();
					this.mUnique.ReleaseMutex();
					if (this.m_client.m_socket == null || !this.m_client.m_socket.Connected)
					{
						binaryReader = null;
						inputStreamConvert = null;
						this.m_client.sendThread.AskForData(null);
						Thread.Sleep(500);
					}
					else
					{
						try
						{
							if (inputStreamConvert == null || binaryReader.BaseStream == null)
							{
								binaryReader = new BinaryReader(new NetworkStream(this.m_client.m_socket));
								inputStreamConvert = new InputStreamConvert(binaryReader);
							}
							byte b = inputStreamConvert.ReadJavaByte();
							switch (b)
							{
							case 0:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd:",
									b,
									"心跳数据"
								}));
								break;
							case 1:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd:",
									b,
									"登陆"
								}));
								this.ReceiveLogon(inputStreamConvert);
								break;
							case 2:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd:",
									b,
									"服务器端版本号"
								}));
								this.m_client.isResendVersion = true;
								this.m_client.m_Version = inputStreamConvert.ReadJavaUTF();
								this.GetVersionInfo(this.m_client.m_Version);
								break;
							case 3:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 更新码表"
								}));
								this.ReceiveCodeTable(inputStreamConvert);
								break;
							case 4:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 个股行情"
								}));
								this.ReceiveStockQuote(inputStreamConvert);
								break;
							case 5:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 报价排名"
								}));
								this.ReceiveQuoteList(inputStreamConvert);
								break;
							case 6:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 分时数据"
								}));
								this.ReceiveMinLineData(inputStreamConvert);
								break;
							case 7:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 成交明细"
								}));
								this.ReceiveBillDataByV(inputStreamConvert);
								break;
							case 8:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 交易节时间"
								}));
								this.ReceiveTradeTime(inputStreamConvert);
								if (!this.AddButtonFlag)
								{
									this.RemakeButton();
								}
								break;
							case 9:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 市场日期"
								}));
								this.ReceiveMarketDate(inputStreamConvert);
								break;
							case 10:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 综合排名"
								}));
								this.ReceiveMarketSort(inputStreamConvert);
								break;
							case 12:
								this.m_client.m_iMinLineInterval = inputStreamConvert.ReadJavaInt();
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 分时间隔:",
									this.m_client.m_iMinLineInterval
								}));
								if (this.m_client.m_iMinLineInterval <= 0 || this.m_client.m_iMinLineInterval > 60)
								{
									this.m_client.m_iMinLineInterval = 60;
								}
								break;
							case 14:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 切换交易日清空数据"
								}));
								this.ReceiveUpdateTradeTime(inputStreamConvert);
								break;
							case 15:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 行业数据"
								}));
								this.ReceiveIndustryData(inputStreamConvert);
								break;
							case 16:
								Logger.wirte(1, string.Concat(new object[]
								{
									this.marketName,
									"Receive cmd: ",
									b,
									" 地域数据"
								}));
								this.ReceiveRegionData(inputStreamConvert);
								break;
							}
						}
						catch (EndOfStreamException ex)
						{
							Logger.wirte(3, this.marketName + "接收线程发生EndOfStreamException异常：" + ex.ToString());
							if (this.m_client != null)
							{
								if (this.m_client.m_socket != null)
								{
									this.m_client.m_socket.Close();
								}
								binaryReader = null;
								inputStreamConvert = null;
								this.m_client.sendThread.AskForData(null);
							}
						}
						catch (IOException ex2)
						{
							Logger.wirte(3, this.marketName + "接收线程发生IOException异常：" + ex2.ToString());
							if (this.m_client != null)
							{
								if (this.m_client.m_socket != null)
								{
									this.m_client.m_socket.Close();
								}
								binaryReader = null;
								inputStreamConvert = null;
								this.m_client.sendThread.AskForData(null);
							}
						}
						catch (InvalidDataException ex3)
						{
							Logger.wirte(3, this.marketName + "接收线程发生InvalidDataException异常：" + ex3.ToString());
							if (this.m_client != null)
							{
								if (this.m_client.m_socket != null)
								{
									this.m_client.m_socket.Close();
								}
								binaryReader = null;
								inputStreamConvert = null;
								CMDVO packet = null;
								this.m_client.sendThread.AskForData(packet);
							}
						}
						catch (ThreadAbortException ex4)
						{
							Logger.wirte(3, this.marketName + "接收线程发生ThreadAbortException异常：" + ex4.ToString());
							if (this.m_client != null)
							{
								if (this.m_client.m_socket != null)
								{
									this.m_client.m_socket.Close();
								}
								binaryReader = null;
								inputStreamConvert = null;
							}
						}
						catch (Exception ex5)
						{
							Logger.wirte(3, this.marketName + "接收线程发生Exception异常：" + ex5.ToString());
							if (this.m_client != null)
							{
								if (this.m_client.m_socket != null)
								{
									this.m_client.m_socket.Close();
								}
								binaryReader = null;
								inputStreamConvert = null;
								this.m_client.sendThread.AskForData(null);
							}
						}
					}
					this.ReData();
				}
			}
			catch (ThreadAbortException ex6)
			{
				Logger.wirte(3, this.marketName + (string)ex6.ExceptionState + "》》》》》》》》》》》》》》");
			}
			catch (Exception ex7)
			{
				Logger.wirte(3, this.marketName + "接收线程发生异常：" + ex7.ToString());
			}
			Logger.wirte(1, this.marketName + "**********" + this.threadName + "结束！！！**********");
		}
		private void ReceiveUpdateTradeTime(InputStreamConvert m_reader)
		{
			this.isUpdateTradeTime = true;
			string marketID = m_reader.ReadJavaUTF();
			this.ClearDataPlus(marketID);
			this.UpdateTradeEndTime = DateTime.Now;
		}
		private void ReData()
		{
			if (this.isUpdateTradeTime && (DateTime.Now - this.UpdateTradeEndTime).TotalSeconds >= 2.0)
			{
				this.m_client.httpThread.GetCodeList();
				this.m_hqForm.ReQueryCurClient();
				this.UpdateTradeEndTime = DateTime.Now;
				this.isUpdateTradeTime = false;
			}
		}
		private void ClearLocalKLineDatas(string marketID)
		{
			string path = this.pluginInfo.ConfigPath + "data/day/" + marketID;
			string path2 = this.pluginInfo.ConfigPath + "data/min/" + marketID;
			string path3 = this.pluginInfo.ConfigPath + "data/5min/" + marketID;
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
			if (Directory.Exists(path2))
			{
				Directory.Delete(path2, true);
			}
			if (Directory.Exists(path3))
			{
				Directory.Delete(path3, true);
			}
		}
		private void ReceiveRegionData(InputStreamConvert m_reader)
		{
			int num = m_reader.ReadJavaInt();
			this.m_client.m_htRegion.Clear();
			for (int i = 0; i < num; i++)
			{
				this.m_client.m_htRegion.Add(m_reader.ReadJavaUTF(), m_reader.ReadJavaUTF().Trim());
			}
		}
		private void ReceiveIndustryData(InputStreamConvert m_reader)
		{
			int num = m_reader.ReadJavaInt();
			this.m_client.m_htIndustry.Clear();
			for (int i = 0; i < num; i++)
			{
				this.m_client.m_htIndustry.Add(m_reader.ReadJavaUTF(), m_reader.ReadJavaUTF());
			}
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
			ArrayList arrayList = new ArrayList();
			foreach (CommodityInfo commodityInfo in this.m_client.m_codeList)
			{
				if (commodityInfo.marketID != marketID)
				{
					arrayList.Add(commodityInfo);
				}
			}
			this.m_client.m_codeList = arrayList;
			Hashtable hashtable = new Hashtable();
			foreach (string key in this.m_client.m_htProduct.Keys)
			{
				if (((CodeTable)this.m_client.m_htProduct[key]).marketID != marketID)
				{
					hashtable.Add(key, (CodeTable)this.m_client.m_htProduct[key]);
				}
			}
			this.m_client.m_htProduct = hashtable;
			ArrayList arrayList2 = new ArrayList();
			foreach (ProductData productData in this.m_client.aProductData)
			{
				if (productData.commodityInfo.marketID != marketID)
				{
					arrayList2.Add(productData);
				}
			}
			this.m_client.aProductData = arrayList2;
			ArrayList arrayList3 = new ArrayList();
			for (int i = 0; i < this.m_client.m_quoteList.Length; i++)
			{
				ProductDataVO productDataVO = this.m_client.m_quoteList[i];
				if (productDataVO.marketID != marketID)
				{
					arrayList3.Add(productDataVO);
				}
			}
			this.m_client.m_quoteList = new ProductDataVO[arrayList3.Count];
			for (int j = 0; j < arrayList3.Count; j++)
			{
				this.m_client.m_quoteList[j] = (ProductDataVO)arrayList3[j];
			}
			this.m_client.m_iCodeDate = 0;
			this.m_client.m_iCodeTime = 0;
		}
		private void ReceiveMarketDate(InputStreamConvert m_reader)
		{
			string text = m_reader.ReadJavaUTF();
			int num = m_reader.ReadJavaInt();
			int num2 = m_reader.ReadJavaInt();
			if (this.onceReceiveTime)
			{
				this.fristMarketCode = text;
				this.onceReceiveTime = false;
			}
			if (this.m_client.m_htMarketData.ContainsKey(text))
			{
				MarketDataVO marketDataVO = (MarketDataVO)this.m_client.m_htMarketData[text];
				marketDataVO.date = num;
				marketDataVO.time = num2;
			}
			else
			{
				MarketDataVO marketDataVO2 = new MarketDataVO();
				marketDataVO2.marketID = text;
				marketDataVO2.date = num;
				marketDataVO2.time = num2;
				this.m_client.m_htMarketData.Add(text, marketDataVO2);
			}
			int iDate = this.m_client.m_iDate;
			int iTime = this.m_client.m_iTime;
			if (this.setInfo.CurTimeMarketId == text)
			{
				this.m_client.m_iTime = num2;
				if (this.m_client.m_iDate == 0 || num != iDate)
				{
					this.m_client.m_iDate = num;
				}
			}
			else if ((this.setInfo.CurTimeMarketId == "" || this.setInfo.CurTimeMarketId == null) && (text == "00" || (!this.m_client.m_htMarketData.ContainsKey("00") && text == this.fristMarketCode)))
			{
				this.m_client.m_iTime = num2;
				if (this.m_client.m_iDate == 0 || num != iDate)
				{
					this.m_client.m_iDate = num;
				}
			}
			if (!Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
			{
				this.m_client.m_iTime = num2;
				if (this.m_client.m_iDate == 0)
				{
					this.m_client.m_iDate = num;
				}
			}
			Logger.wirte(1, string.Concat(new object[]
			{
				this.marketName,
				"市场日期为:",
				this.m_client.m_iDate,
				"市场时间为： ",
				num2
			}));
			if (iDate != this.m_client.m_iDate || iTime != this.m_client.m_iTime)
			{
				this.m_hqForm.Repaint();
				this.m_hqForm.RepaintBottom();
			}
		}
		private void ReceiveTradeTime(InputStreamConvert m_reader)
		{
			Hashtable obj = CMDTradeTimeVO.getObj(m_reader);
			foreach (string key in obj.Keys)
			{
				if (this.m_client.m_htMarketData.ContainsKey(key))
				{
					((MarketDataVO)this.m_client.m_htMarketData[key]).m_timeRange = ((MarketDataVO)obj[key]).m_timeRange;
				}
				else
				{
					this.m_client.m_htMarketData.Add(key, obj[key]);
				}
			}
		}
		protected override void disposeThread()
		{
			try
			{
				if (this.m_client != null && this.m_client.m_socket != null)
				{
					this.m_client.m_socket.Close();
					this.m_client.m_socket = null;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
			}
			base.Abort("**********强行中断" + this.threadName + "！！！**********");
		}
		private void ReceiveQuoteList(InputStreamConvert reader)
		{
			bool flag = false;
			ProductDataVO[] obj = CMDQuoteListVO.getObj(reader);
			Logger.wirte(1, this.marketName + "本次更新数据条数=" + obj.Length);
			ProductDataVO[] array = obj;
			for (int i = 0; i < array.Length; i++)
			{
				ProductDataVO productDataVO = array[i];
				foreach (CommodityInfo commodityInfo in this.m_client.m_codeList)
				{
					if (productDataVO.code == commodityInfo.commodityCode)
					{
						productDataVO.industry = commodityInfo.industry;
						productDataVO.region = commodityInfo.region;
					}
				}
			}
			if (this.m_client.m_quoteList.Length == 0)
			{
				flag = true;
				this.m_client.m_quoteList = obj;
			}
			else
			{
				for (int j = 0; j < obj.Length; j++)
				{
					obj[j].datahighlightTime = this.multiQuoteData.HighlightTime;
					bool flag2 = false;
					for (int k = 0; k < this.m_client.m_quoteList.Length; k++)
					{
						if (this.m_client.m_quoteList[k].code.Equals(obj[j].code) && this.m_client.m_quoteList[k].marketID.Equals(obj[j].marketID))
						{
							this.m_client.m_quoteList[k] = obj[j];
							flag2 = true;
							break;
						}
					}
					for (int l = 0; l < this.multiQuoteData.m_curQuoteList.Length; l++)
					{
						if (this.multiQuoteData.m_curQuoteList[l].code.Equals(obj[j].code) && this.multiQuoteData.m_curQuoteList[l].marketID.Equals(obj[j].marketID))
						{
							flag = true;
							break;
						}
					}
					if (!flag2)
					{
						ProductDataVO[] array2 = new ProductDataVO[this.m_client.m_quoteList.Length + 1];
						for (int m = 0; m < this.m_client.m_quoteList.Length; m++)
						{
							array2[m] = this.m_client.m_quoteList[m];
						}
						array2[array2.Length - 1] = obj[j];
						this.m_client.m_quoteList = array2;
					}
					if (obj.Length > 0 && this.m_client.m_bShowIndexAtBottom == 1 && this.m_client.indexMainCode.Length > 0 && string.Compare(obj[j].marketID + "_" + obj[j].code, this.m_client.indexMainCode, true) == 0)
					{
						ProductData productData = this.m_client.GetProductData(this.m_client.curCommodityInfo);
						if (productData == null)
						{
							if (this.m_client.aProductData.Count > 50)
							{
								this.m_client.aProductData.RemoveAt(50);
							}
							productData = new ProductData();
							productData.commodityInfo = new CommodityInfo(obj[j].marketID, obj[j].code);
							productData.realData = obj[j];
							this.m_client.aProductData.Insert(0, productData);
						}
						else
						{
							productData.realData = obj[j];
						}
						this.m_hqForm.RepaintBottom();
					}
				}
			}
			if (this.m_client.CurrentPage != 0)
			{
				return;
			}
			if (this.m_client.m_bShowIndexAtBottom == 1)
			{
				this.m_hqForm.RepaintBottom();
			}
			if (flag)
			{
				this.m_hqForm.Repaint();
			}
		}
		private void ReceiveMinLineData(InputStreamConvert reader)
		{
			string text = reader.ReadJavaUTF();
			string text2 = reader.ReadJavaUTF();
			reader.ReadJavaByte();
			reader.ReadJavaInt();
			reader.ReadJavaInt();
			MinDataVO[] obj = CMDMinVO.getObj(reader);
			Logger.wirte(1, "分时数据条数======" + obj.Length.ToString());
			ProductData productData = this.m_client.GetProductData(text, text2);
			if (productData == null)
			{
				if (this.m_client.aProductData.Count > 50)
				{
					this.m_client.aProductData.RemoveAt(50);
				}
				productData = new ProductData();
				productData.commodityInfo = new CommodityInfo(text, text2);
				this.m_client.aProductData.Insert(0, productData);
			}
			lock (productData)
			{
				productData.aMinLine = ArrayList.Synchronized(new ArrayList());
				int num = 0;
				for (int i = 0; i < obj.Length; i++)
				{
					TradeTimeVO[] timeRange = ((MarketDataVO)this.m_client.m_htMarketData[text]).m_timeRange;
					int minLineIndexFromTime = M_Common.GetMinLineIndexFromTime(obj[i].date, obj[i].time, timeRange, this.m_client.m_iMinLineInterval);
					for (int j = num; j < minLineIndexFromTime; j++)
					{
						MinDataVO minDataVO = new MinDataVO();
						if (j > 0)
						{
							minDataVO.curPrice = ((MinDataVO)productData.aMinLine[j - 1]).curPrice;
							minDataVO.totalAmount = ((MinDataVO)productData.aMinLine[j - 1]).totalAmount;
							minDataVO.totalMoney = ((MinDataVO)productData.aMinLine[j - 1]).totalMoney;
							minDataVO.averPrice = ((MinDataVO)productData.aMinLine[j - 1]).averPrice;
							minDataVO.reserveCount = ((MinDataVO)productData.aMinLine[j - 1]).reserveCount;
						}
						else if (productData.realData != null)
						{
							minDataVO.curPrice = productData.realData.yesterBalancePrice;
							minDataVO.averPrice = productData.realData.yesterBalancePrice;
						}
						productData.aMinLine.Add(minDataVO);
					}
					if (minLineIndexFromTime >= productData.aMinLine.Count - 1)
					{
						MinDataVO minDataVO2;
						if (minLineIndexFromTime == productData.aMinLine.Count - 1)
						{
							minDataVO2 = (MinDataVO)productData.aMinLine[productData.aMinLine.Count - 1];
						}
						else
						{
							minDataVO2 = new MinDataVO();
							productData.aMinLine.Add(minDataVO2);
						}
						minDataVO2.curPrice = obj[i].curPrice;
						minDataVO2.totalAmount = obj[i].totalAmount;
						minDataVO2.reserveCount = obj[i].reserveCount;
						minDataVO2.averPrice = obj[i].averPrice;
						num = minLineIndexFromTime + 1;
					}
				}
			}
			if ((2 == this.m_client.CurrentPage || 1 == this.m_client.CurrentPage) && (this.m_hqForm.IsMultiCommidity || (string.Compare(this.m_client.curCommodityInfo.marketID, productData.commodityInfo.marketID) == 0 && string.Compare(this.m_client.curCommodityInfo.commodityCode, productData.commodityInfo.commodityCode) == 0)))
			{
				Logger.wirte(1, "重绘==============");
				this.m_hqForm.Repaint();
			}
		}
		private void ReceiveBillData(InputStreamConvert reader)
		{
			string text = reader.ReadJavaUTF();
			string text2 = reader.ReadJavaUTF();
			byte b = reader.ReadJavaByte();
			reader.ReadJavaInt();
			int time = reader.ReadJavaInt();
			BillDataVO[] obj = CMDBillVO.getObj(reader, ref this.isConnection);
			ProductData productData = this.m_client.GetProductData(text, text2);
			if (productData == null)
			{
				if (this.m_client.aProductData.Count > 50)
				{
					this.m_client.aProductData.RemoveAt(50);
				}
				productData = new ProductData();
				productData.commodityInfo = new CommodityInfo(text, text2);
				this.m_client.aProductData.Insert(0, productData);
			}
			switch (b)
			{
			case 0:
				this.MakeLastBills(productData, time, obj);
				return;
			case 1:
				productData.lastBill = ArrayList.Synchronized(new ArrayList());
				for (int i = 0; i < obj.Length; i++)
				{
					productData.lastBill.Add(obj[i]);
				}
				if (obj.Length != 0 && 1 == this.m_client.CurrentPage && (this.m_hqForm.IsMultiCommidity || (string.Compare(this.m_client.curCommodityInfo.marketID, text) == 0 && string.Compare(this.m_client.curCommodityInfo.commodityCode, text2) == 0)))
				{
					this.m_hqForm.Repaint();
					return;
				}
				break;
			case 2:
				this.MakeLastBills(productData, time, obj);
				break;
			default:
				return;
			}
		}
		private void ReceiveBillDataByV(InputStreamConvert reader)
		{
			string text = reader.ReadJavaUTF();
			string text2 = reader.ReadJavaUTF();
			byte b = reader.ReadJavaByte();
			reader.ReadJavaLong();
			long num = reader.ReadJavaLong();
			reader.ReadJavaUTF();
			BillDataVO[] obj = CMDBillVO.getObj(reader, ref this.isConnection);
			ProductData productData = this.m_client.GetProductData(text, text2);
			if (productData == null)
			{
				if (this.m_client.aProductData.Count > 50)
				{
					this.m_client.aProductData.RemoveAt(50);
				}
				productData = new ProductData();
				productData.commodityInfo = new CommodityInfo(text, text2);
				this.m_client.aProductData.Insert(0, productData);
			}
			switch (b)
			{
			case 0:
				this.MakeLastBills(productData, (int)num, obj);
				return;
			case 1:
				this.m_client.isChangePage = 0;
				productData.lastBill = ArrayList.Synchronized(new ArrayList());
				for (int i = 0; i < obj.Length; i++)
				{
					productData.lastBill.Add(obj[i]);
				}
				if (obj.Length != 0 && 1 == this.m_client.CurrentPage && (this.m_hqForm.IsMultiCommidity || (string.Compare(this.m_client.curCommodityInfo.marketID, text) == 0 && string.Compare(this.m_client.curCommodityInfo.commodityCode, text2) == 0)))
				{
					this.m_hqForm.Repaint();
					return;
				}
				break;
			case 2:
				this.MakeLastBills(productData, (int)num, obj);
				break;
			default:
				return;
			}
		}
		private void MakeLastBills(ProductData stock, int time, BillDataVO[] values)
		{
			if (values.Length <= 0)
			{
				this.m_client.isChangePage = 0;
				return;
			}
			if (time == 0)
			{
				Logger.wirte(1, "time=======" + time.ToString() + "     " + values.Length.ToString());
				this.m_client.isChangePage = 0;
				int i = 0;
				while (i < values.Length)
				{
					if (stock.aBill.Count <= 0)
					{
						goto IL_A1;
					}
					BillDataVO billDataVO = (BillDataVO)stock.aBill[stock.aBill.Count - 1];
					if (billDataVO != null && values[i] != null && billDataVO.totalAmount < values[i].totalAmount)
					{
						goto IL_A1;
					}
					IL_129:
					i++;
					continue;
					IL_A1:
					stock.aBill.Add(values[i]);
					if (this.m_client.CurrentPage == 2)
					{
						if (stock.lastBill.Count > 0)
						{
							BillDataVO billDataVO2 = (BillDataVO)stock.lastBill[stock.lastBill.Count - 1];
							if (billDataVO2.date > values[0].date || (billDataVO2.date == values[0].date && billDataVO2.totalAmount >= values[i].totalAmount))
							{
								goto IL_129;
							}
						}
						stock.lastBill.Add(values[i]);
						goto IL_129;
					}
					goto IL_129;
				}
				if (4 == this.m_client.CurrentPage || (2 == this.m_client.CurrentPage && (7 == this.m_client.m_iKLineCycle || 8 == this.m_client.m_iKLineCycle || 9 == this.m_client.m_iKLineCycle || 10 == this.m_client.m_iKLineCycle || 5 == this.m_client.m_iKLineCycle || 6 == this.m_client.m_iKLineCycle || 14 == this.m_client.m_iKLineCycle)))
				{
					this.m_hqForm.Repaint();
				}
				return;
			}
			if (this.m_client.isChangePage == 1)
			{
				return;
			}
			if (stock.aBill == null)
			{
				stock.aBill = ArrayList.Synchronized(new ArrayList());
			}
			bool flag = false;
			bool flag2 = false;
			switch (this.m_client.CurrentPage)
			{
			case 1:
				if (stock.lastBill.Count > 0)
				{
					BillDataVO billDataVO3 = (BillDataVO)stock.lastBill[stock.lastBill.Count - 1];
					if (billDataVO3.date > values[0].date || (billDataVO3.date == values[0].date && billDataVO3.totalAmount >= values[0].totalAmount))
					{
						return;
					}
				}
				break;
			case 2:
				if (stock.aBill.Count > 0)
				{
					BillDataVO billDataVO4 = (BillDataVO)stock.aBill[stock.aBill.Count - 1];
					if (billDataVO4.date > values[0].date || (billDataVO4.date == values[0].date && billDataVO4.totalAmount >= values[0].totalAmount))
					{
						flag = true;
					}
				}
				if (stock.lastBill.Count > 0)
				{
					BillDataVO billDataVO5 = (BillDataVO)stock.lastBill[stock.lastBill.Count - 1];
					if (billDataVO5.date > values[0].date || (billDataVO5.date == values[0].date && billDataVO5.totalAmount >= values[0].totalAmount))
					{
						flag2 = true;
					}
				}
				if (flag && flag2)
				{
					return;
				}
				break;
			case 4:
				if (stock.aBill.Count > 0)
				{
					BillDataVO billDataVO6 = (BillDataVO)stock.aBill[stock.aBill.Count - 1];
					if (billDataVO6.date > values[0].date || (billDataVO6.date == values[0].date && billDataVO6.totalAmount >= values[0].totalAmount))
					{
						return;
					}
				}
				break;
			}
			for (int j = 0; j < values.Length; j++)
			{
				if (this.m_client.CurrentPage == 1)
				{
					stock.lastBill.Add(values[j]);
				}
				else if (this.m_client.CurrentPage == 2)
				{
					if (!flag2)
					{
						stock.lastBill.Add(values[j]);
					}
					if (!flag)
					{
						if (values[j] == null && stock.aBill.Count > 1)
						{
							stock.aBill.RemoveAt(stock.aBill.Count - 1);
							break;
						}
						stock.aBill.Add(values[j]);
					}
				}
				else if (this.m_client.CurrentPage == 4)
				{
					if (values[j] == null && stock.aBill.Count > 1)
					{
						stock.aBill.RemoveAt(stock.aBill.Count - 1);
						break;
					}
					stock.aBill.Add(values[j]);
				}
			}
			if (this.m_client.CurrentPage == 4)
			{
				this.m_hqForm.Repaint();
				return;
			}
			if (2 == this.m_client.CurrentPage || 1 == this.m_client.CurrentPage)
			{
				if (stock.aMinLine == null)
				{
					stock.aMinLine = ArrayList.Synchronized(new ArrayList());
				}
				for (int k = 0; k < values.Length; k++)
				{
					TradeTimeVO[] timeRange = ((MarketDataVO)this.m_client.m_htMarketData[stock.commodityInfo.marketID]).m_timeRange;
					int minLineIndexFromTime = M_Common.GetMinLineIndexFromTime(values[k].date, values[k].time, timeRange, this.m_client.m_iMinLineInterval);
					if (minLineIndexFromTime < stock.aMinLine.Count - 1)
					{
						break;
					}
					MinDataVO minDataVO = null;
					if (minLineIndexFromTime == stock.aMinLine.Count - 1)
					{
						minDataVO = (MinDataVO)stock.aMinLine[minLineIndexFromTime];
					}
					else
					{
						for (int l = stock.aMinLine.Count; l <= minLineIndexFromTime; l++)
						{
							minDataVO = new MinDataVO();
							if (l > 0)
							{
								minDataVO.curPrice = ((MinDataVO)stock.aMinLine[l - 1]).curPrice;
								minDataVO.totalAmount = ((MinDataVO)stock.aMinLine[l - 1]).totalAmount;
								minDataVO.totalMoney = ((MinDataVO)stock.aMinLine[l - 1]).totalMoney;
								minDataVO.reserveCount = ((MinDataVO)stock.aMinLine[l - 1]).reserveCount;
								minDataVO.averPrice = ((MinDataVO)stock.aMinLine[l - 1]).averPrice;
							}
							stock.aMinLine.Add(minDataVO);
						}
					}
					minDataVO.curPrice = values[k].curPrice;
					minDataVO.totalAmount = values[k].totalAmount;
					minDataVO.totalMoney = values[k].totalMoney;
					minDataVO.reserveCount = values[k].reserveCount;
					minDataVO.averPrice = values[k].balancePrice;
				}
				if (values[values.Length - 1].date > this.m_client.m_iDate)
				{
					this.m_client.m_iDate = values[values.Length - 1].date;
				}
				if (values[values.Length - 1].date >= this.m_client.m_iDate && values[values.Length - 1].time > this.m_client.m_iTime)
				{
					this.m_client.m_iTime = values[values.Length - 1].time;
				}
				if ((2 == this.m_client.CurrentPage || 1 == this.m_client.CurrentPage || 4 == this.m_client.CurrentPage) && (this.m_hqForm.IsMultiCommidity || (string.Compare(this.m_client.curCommodityInfo.marketID, stock.commodityInfo.marketID) == 0 && string.Compare(this.m_client.curCommodityInfo.commodityCode, stock.commodityInfo.commodityCode) == 0)))
				{
					this.m_hqForm.Repaint();
				}
			}
		}
		private void ReceiveMarketSort(InputStreamConvert reader)
		{
			int iCount = reader.ReadJavaInt();
			MarketStatusVO[] obj = CMDMarketSortVO.getObj(reader);
			if (5 != this.m_client.CurrentPage)
			{
				return;
			}
			Page_MarketStatus page_MarketStatus = (Page_MarketStatus)this.m_hqForm.MainGraph;
			page_MarketStatus.packetInfo = new Packet_MarketStatus();
			page_MarketStatus.packetInfo.iCount = iCount;
			page_MarketStatus.statusData = obj;
			this.m_hqForm.Repaint();
		}
		private void ReceiveClassSort(InputStreamConvert reader)
		{
			byte b = reader.ReadJavaByte();
			reader.ReadJavaByte();
			int num = reader.ReadJavaInt();
			int num2 = reader.ReadJavaInt();
			Logger.wirte(1, string.Concat(new object[]
			{
				" sortBy:",
				b,
				" totalCount:",
				num,
				" start:",
				num2
			}));
			CMDSortVO.getObj(reader);
		}
		private void ReceiveStockQuote(InputStreamConvert reader)
		{
			ProductDataVO[] obj = CMDQuoteVO.getObj(reader);
			for (int i = 0; i < obj.Length; i++)
			{
				string marketID = obj[i].marketID;
				string code = obj[i].code;
				ProductData productData = this.m_client.GetProductData(marketID, code);
				if (productData == null)
				{
					if (this.m_client.aProductData.Count > 50)
					{
						this.m_client.aProductData.RemoveAt(50);
					}
					productData = new ProductData();
					productData.commodityInfo = new CommodityInfo(marketID, code);
					productData.realData = obj[i];
					this.m_client.aProductData.Insert(0, productData);
				}
				else
				{
					productData.realData = obj[i];
				}
			}
			if (obj.Length > 0 && (2 == this.m_client.CurrentPage || 1 == this.m_client.CurrentPage))
			{
				this.m_hqForm.Repaint();
			}
			if (obj.Length > 0 && this.m_client.m_bShowIndexAtBottom == 1)
			{
				this.m_hqForm.RepaintBottom();
			}
		}
		public void ReceiveCodeTable(InputStreamConvert reader)
		{
			ProductInfoListVO obj = CMDProductInfoVO.getObj(reader);
			for (int i = 0; i < obj.productInfos.Length; i++)
			{
				CommodityInfo commodityInfo = new CommodityInfo(obj.productInfos[i].marketID, obj.productInfos[i].code, obj.productInfos[i].region, obj.productInfos[i].industry);
				bool flag = false;
				for (int j = 0; j < this.m_client.m_codeList.Count; j++)
				{
					if (commodityInfo.Compare(this.m_client.m_codeList[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.m_client.m_codeList.Add(commodityInfo);
					if (obj.productInfos[i].status == 1)
					{
						this.m_client.hm_codeList.Add(commodityInfo);
					}
					else
					{
						this.m_client.nm_codeList.Add(commodityInfo);
					}
				}
				CodeTable codeTable = new CodeTable();
				codeTable.marketID = obj.productInfos[i].marketID;
				codeTable.sName = obj.productInfos[i].name;
				if (this.m_hqForm.AddMarketName)
				{
					codeTable.sName = this.marketName + codeTable.sName;
				}
				codeTable.sPinyin = obj.productInfos[i].pyName;
				codeTable.status = obj.productInfos[i].status;
				codeTable.fUnit = obj.productInfos[i].fUnit;
				codeTable.tradeSecNo = obj.productInfos[i].tradeSecNo;
				codeTable.mPrice = obj.productInfos[i].mPrice;
				this.m_client.m_htProduct[obj.productInfos[i].marketID + obj.productInfos[i].code] = codeTable;
				this.m_client.m_iCodeDate = obj.date;
				this.m_client.m_iCodeTime = obj.time;
			}
		}
		public void ReceiveMarketTable(InputStreamConvert reader)
		{
		}
		public void RemakeButton()
		{
			while (this.m_hqForm.MainGraph == null)
			{
				Thread.Sleep(100);
			}
			this.AddMarketButton();
			if (!this.isAddRightBtn)
			{
				this.AddRightButton();
				this.isAddRightBtn = true;
			}
			this.buttonUtils.isTidyBtnFlag++;
			if (this.buttonUtils.isTidyBtnFlag == 2)
			{
				this.buttonUtils.TidyButtons(this.buttonUtils.ButtonList);
			}
			if (this.m_client.CurrentPage == 0)
			{
				this.m_hqForm.Repaint();
			}
		}
		public void AddMarketButton()
		{
			if (this.m_client.m_htMarketData != null)
			{
				this.AddButtonFlag = true;
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					this.buttonUtils.InsertButton(0, new MyButton("AllMarket", "所有市场商品", this.buttonUtils.InitialButtonName == "AllMarket"), this.buttonUtils.ButtonList);
				}
				else if (this.m_client.m_htMarketData != null)
				{
					IDictionaryEnumerator enumerator = this.m_client.m_htMarketData.GetEnumerator();
					try
					{
						if (enumerator.MoveNext())
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
							MarketDataVO arg_B9_0 = (MarketDataVO)dictionaryEntry.Value;
							if (this.buttonUtils.InitialButtonName.StartsWith("Market"))
							{
								this.buttonUtils.InitialButtonName.Substring(6);
							}
							MyButton myButton = new MyButton("Market" + dictionaryEntry.Key.ToString(), "所有商品", true);
							if (this.buttonUtils.ButtonList.Count == 0)
							{
								this.buttonUtils.InsertButton(0, myButton, this.buttonUtils.ButtonList);
							}
							else if (((MyButton)this.buttonUtils.ButtonList[0]).Name != myButton.Name)
							{
								this.buttonUtils.InsertButton(0, myButton, this.buttonUtils.ButtonList);
							}
						}
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
				bool selected = false;
				if (this.buttonUtils.InitialButtonName.Equals("MyCommodity"))
				{
					selected = true;
				}
				this.buttonUtils.InsertButton(1, new MyButton("MyCommodity", this.pluginInfo.HQResourceManager.GetString("HQStr_MyCommodity"), selected), this.buttonUtils.ButtonList);
			}
		}
		public void AddRightButton()
		{
			string rightButtonItems = this.setInfo.RightButtonItems;
			string[] array = rightButtonItems.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				if (array2[0].Equals("CareFul"))
				{
					this.buttonUtils.InsertButton(i, new MyButton("X_Btn", array2[1], true), this.buttonUtils.RightButtonList);
				}
				else if (array2[0].Equals("Plate"))
				{
					this.buttonUtils.InsertButton(i, new MyButton("P_Btn", array2[1], false), this.buttonUtils.RightButtonList);
				}
				else if (array2[0].Equals("Map"))
				{
					this.buttonUtils.InsertButton(i, new MyButton("T_Btn", array2[1], false), this.buttonUtils.RightButtonList);
				}
			}
		}
		public void GetVersionInfo(string version)
		{
			if (version.Length > 0)
			{
				try
				{
					VersionInfo.First = version.Substring(0, 1);
					string[] array = version.Split(new char[]
					{
						'.'
					});
					VersionInfo.MainVersion = array[0].Substring(1, array[0].Length - VersionInfo.First.Length);
					VersionInfo.SecondVersion = array[1];
					VersionInfo.ThirdVersion = array[2];
				}
				catch (Exception ex)
				{
					Logger.wirte(3, "解析版本号出错：" + ex.Message);
				}
			}
		}
		private void ReceiveLogon(InputStreamConvert reader)
		{
			new LogonVO();
			CMDLogonVO.getObj(reader);
		}
	}
}
