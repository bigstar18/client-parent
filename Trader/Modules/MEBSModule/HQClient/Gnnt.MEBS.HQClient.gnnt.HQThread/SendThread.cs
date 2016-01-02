using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel.DataVO;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections;
using System.Net;
using System.Threading;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
	public class SendThread : MYThread
	{
		private ManualResetEvent Event = new ManualResetEvent(true);
		private ArrayList aPacket = ArrayList.Synchronized(new ArrayList());
		protected DateTime m_calCodeTable;
		protected DateTime m_calIndex;
		private HQClientMain m_client;
		private DateTime datetimeBill;
		private DateTime datetimeClassSort;
		private DateTime datetimeQuote;
		private DateTime datetimeMinData;
		private DateTime datetimeMarketSort;
		private bool isFirstCon = true;
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
					case 4:
						if ((DateTime.Now - this.datetimeQuote).TotalMilliseconds >= 100.0)
						{
							this.aPacket.Add(packet);
							this.datetimeQuote = DateTime.Now;
							goto IL_206;
						}
						goto IL_206;
					case 5:
						if ((DateTime.Now - this.datetimeClassSort).TotalMilliseconds >= 100.0)
						{
							this.aPacket.Add(packet);
							this.datetimeClassSort = DateTime.Now;
							Logger.wirte(1, string.Concat(new object[]
							{
								"---------cmd:",
								packet.getCmd(),
								"------",
								this.datetimeClassSort.ToString("o")
							}));
							goto IL_206;
						}
						goto IL_206;
					case 6:
						if ((DateTime.Now - this.datetimeMinData).TotalMilliseconds >= 100.0)
						{
							this.aPacket.Add(packet);
							this.datetimeMinData = DateTime.Now;
							goto IL_206;
						}
						goto IL_206;
					case 7:
						if ((DateTime.Now - this.datetimeBill).TotalMilliseconds >= 100.0)
						{
							this.aPacket.Add(packet);
							this.datetimeBill = DateTime.Now;
							Logger.wirte(1, "------------" + packet.getCmd() + this.datetimeBill.ToLongTimeString());
							goto IL_206;
						}
						goto IL_206;
					case 10:
						if ((DateTime.Now - this.datetimeMarketSort).TotalMilliseconds >= 100.0)
						{
							this.aPacket.Add(packet);
							this.datetimeMarketSort = DateTime.Now;
							goto IL_206;
						}
						goto IL_206;
					}
					this.aPacket.Add(packet);
					IL_206:
					int count = this.aPacket.Count;
					int num = 5;
					if (count > num)
					{
						for (int i = 0; i < count - num; i++)
						{
							CMDVO cMDVO = (CMDVO)this.aPacket[0];
							Logger.wirte(1, "AskForDataRemove:" + cMDVO.getCmd());
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
						{
							Thread.Sleep(5000);
						}
					}
					else
					{
						Thread.Sleep(100);
						TimeSpan timeSpan = default(TimeSpan);
						if (this.m_client.m_bShowIndexAtBottom == 1 && this.m_client.indexMainCode.Length > 0)
						{
							timeSpan = DateTime.Now.Subtract(this.m_calIndex);
							if (this.m_calIndex.Year == 1 || timeSpan.Seconds >= 10)
							{
								this.AskForIndex();
								this.m_calIndex = DateTime.Now;
							}
						}
						if (DateTime.Now.Subtract(this.m_calCodeTable).Minutes >= 1)
						{
							this.AskForDateAndCodeTable();
							this.m_calCodeTable = DateTime.Now;
						}
						int count = this.aPacket.Count;
						if (count > 0)
						{
							CMDVO cMDVO = (CMDVO)this.aPacket[0];
							this.aPacket.RemoveAt(0);
							try
							{
								switch (cMDVO.getCmd())
								{
								case 4:
									Logger.wirte(1, string.Concat(new object[]
									{
										"Send cmd:",
										cMDVO.getCmd(),
										"-",
										((CMDQuoteVO)cMDVO).codeList[0, 0],
										"-",
										((CMDQuoteVO)cMDVO).codeList[0, 1]
									}));
									break;
								case 5:
								{
									CMDSortVO cMDSortVO = (CMDSortVO)cMDVO;
									Logger.wirte(1, string.Concat(new object[]
									{
										"Send cmd:",
										cMDVO.getCmd(),
										" sort by ",
										cMDSortVO.sortBy,
										" start:",
										cMDSortVO.start,
										" end:",
										cMDSortVO.end
									}));
									break;
								}
								default:
									Logger.wirte(1, "Send cmd:" + cMDVO.getCmd());
									break;
								}
								RequestUtil.SendRequest(cMDVO, this.m_client.m_socket);
								continue;
							}
							catch (Exception ex)
							{
								Logger.wirte(3, ex.ToString());
								this.m_client.m_socket = null;
								continue;
							}
						}
						this.Event.Reset();
					}
				}
				Logger.wirte(1, "**********" + this.threadName + "结束！！！**********");
			}
			catch (ThreadAbortException ex2)
			{
				Logger.wirte(3, (string)ex2.ExceptionState);
			}
			catch (Exception ex3)
			{
				Logger.wirte(3, "发送线程run方法异常：" + ex3.Message);
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
					this.proxySocket = null;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
			}
			base.Abort("**********强行中断" + this.threadName + "！！！**********");
		}
		public bool ConnectToServer(int receiveBufferSize)
		{
			HQForm hqForm = this.m_client.m_hqForm;
			this.m_client.Connected = false;
			hqForm.RepaintBottom();
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(this.m_client.strSocketIP);
				IPAddress address = hostAddresses[0];
				IPEndPoint iPEndPoint = new IPEndPoint(address, this.m_client.iSocketPort);
				this.proxySocket = new ProxySocket();
				if (receiveBufferSize > 0)
				{
					this.proxySocket.ReceiveBufferSize = receiveBufferSize;
				}
				this.m_client.m_socket = this.proxySocket.GetSocket(iPEndPoint);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "ConnectToServer:" + ex.ToString());
				return false;
			}
			Logger.wirte(1, "**************连接成功**************");
			this.m_client.Connected = true;
			if (!this.isFirstCon)
			{
				hqForm.ReQueryCurClient();
			}
			this.isFirstCon = false;
			hqForm.RepaintBottom();
			if (this.m_client.m_socket == null)
			{
				Logger.wirte(1, "Socket is null");
			}
			this.m_calCodeTable = DateTime.Now;
			return true;
		}
		private void AskForDateAndCodeTable()
		{
			try
			{
				CMDDateVO cmd = new CMDDateVO();
				RequestUtil.SendRequest(cmd, this.m_client.m_socket);
				RequestUtil.SendRequest(new CMDProductInfoVO
				{
					date = this.m_client.m_iCodeDate,
					time = this.m_client.m_iCodeTime
				}, this.m_client.m_socket);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
				this.m_client.m_socket = null;
			}
		}
		public void AskForServerVersion()
		{
			try
			{
				CMDVersionVO cmd = new CMDVersionVO();
				RequestUtil.SendRequest(cmd, this.m_client.m_socket);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
				this.m_client.m_socket = null;
			}
		}
		public void AskForIndex()
		{
			if (this.m_client.indexMainCode.Length == 0)
			{
				return;
			}
			CommodityInfo commodityInfo = CommodityInfo.DealCode(this.m_client.indexMainCode);
			ProductData productData = this.m_client.GetProductData(commodityInfo);
			DateTime dateTime = default(DateTime);
			if (productData != null && productData.realData != null)
			{
				dateTime = productData.realData.time;
			}
			CMDQuoteVO cMDQuoteVO = new CMDQuoteVO();
			cMDQuoteVO.codeList = new string[1, 3];
			cMDQuoteVO.codeList[0, 0] = commodityInfo.commodityCode;
			cMDQuoteVO.codeList[0, 2] = commodityInfo.marketID;
			cMDQuoteVO.isAll = 1;
			if (dateTime.Year != 1)
			{
				int num = dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second;
				cMDQuoteVO.codeList[0, 1] = num.ToString();
			}
			else
			{
				cMDQuoteVO.codeList[0, 1] = "0";
			}
			try
			{
				RequestUtil.SendRequest(cMDQuoteVO, this.m_client.m_socket);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
				this.m_client.m_socket = null;
			}
		}
		public static void AskForRealQuote(string marketID, string code, DateTime time, SendThread sendThread)
		{
			CMDQuoteVO cMDQuoteVO = new CMDQuoteVO();
			cMDQuoteVO.codeList = new string[1, 3];
			cMDQuoteVO.codeList[0, 0] = code;
			cMDQuoteVO.codeList[0, 2] = marketID;
			cMDQuoteVO.isAll = 1;
			if (time.Year != 1)
			{
				int num = time.Year * 10000 + time.Month * 100 + time.Day;
				int num2 = time.Hour * 10000 + time.Minute * 100 + time.Second;
				long num3 = (long)(num * 10000 + num2);
				cMDQuoteVO.codeList[0, 1] = num3.ToString();
			}
			else
			{
				cMDQuoteVO.codeList[0, 1] = "0";
			}
			sendThread.AskForData(cMDQuoteVO);
		}
	}
}
