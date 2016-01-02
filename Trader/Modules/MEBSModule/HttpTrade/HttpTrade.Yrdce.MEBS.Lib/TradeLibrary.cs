using HttpTrade.Gnnt.MEBS.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
using TradeInterface.Gnnt.Interface;
namespace HttpTrade.Gnnt.MEBS.Lib
{
	public class TradeLibrary : ITradeLibrary
	{
		private const int maxTradeCount = 500;
		private const int maxOrderCount = 500;
		private bool isConsumerPlugin;
		private int protocolID = 1;
		public static bool isWriteLog;
		private string myIpAddress = string.Empty;
		private int myPort;
		private string myCommunicationUrl = string.Empty;
		private string buffPath = string.Empty;
		private HTTPCommunicate com;
		private string tradebuffer = "tradebuffer.buf";
		private string orderbuffer = "orderbuffer.buf";
		private string ordertimebuffer = "ordertimebuffer.buf";
		private object lockObject = new object();
		private bool isFirstCheckUser = true;
		private object weekOrderBufferLock = new object();
		private long lastTradeId;
		private long lastPagingTradeId;
		private List<M_TradeInfo> tradeListBuffer = new List<M_TradeInfo>();
		private bool isWriteFile;
		private bool IsSaveLastID;
		private string curTradeDay = string.Empty;
		private long lastID;
		private long tradeTotal = -1L;
		private long ltradeTotal;
		private List<M_OrderInfo> orderListBuffer = new List<M_OrderInfo>();
		private long weekOrderUpdateTime;
		private long weekOrderPagingUpdateTime;
		private long weekConditionOrderUpdateTime;
		private bool weekOrderQueryFirst = true;
		public bool IsConsumerPlugin
		{
			get
			{
				return this.isConsumerPlugin;
			}
			set
			{
				this.isConsumerPlugin = value;
			}
		}
		public int ProtocolID
		{
			get
			{
				return this.protocolID;
			}
			set
			{
				this.protocolID = value;
			}
		}
		public bool IsWriteLog
		{
			get
			{
				return TradeLibrary.isWriteLog;
			}
			set
			{
				TradeLibrary.isWriteLog = value;
			}
		}
		public string IpAddress
		{
			get
			{
				return this.myIpAddress;
			}
			set
			{
				this.myIpAddress = value;
			}
		}
		public int Port
		{
			get
			{
				return this.myPort;
			}
			set
			{
				this.myPort = value;
			}
		}
		public string CommunicationUrl
		{
			get
			{
				return this.myCommunicationUrl;
			}
			set
			{
				this.myCommunicationUrl = value;
			}
		}
		public string BuffPath
		{
			get
			{
				return this.buffPath;
			}
			set
			{
				this.buffPath = value;
			}
		}
		public void Initialize()
		{
			this.com = new HTTPCommunicate(this.myCommunicationUrl);
		}
		public void Dispose()
		{
			this.orderListBuffer.Clear();
			this.tradeListBuffer.Clear();
			this.weekOrderUpdateTime = 0L;
			this.weekOrderPagingUpdateTime = 0L;
			this.weekConditionOrderUpdateTime = 0L;
			this.lastTradeId = 0L;
			this.lastPagingTradeId = 0L;
			this.tradeTotal = -1L;
			this.isWriteFile = false;
		}
		private void recordKey(string username, string key)
		{
			FileStream fileStream = new FileStream(username + ".key", FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.BigEndianUnicode);
			binaryWriter.Write(key);
			binaryWriter.Close();
			fileStream.Close();
		}
		private string readKey(string username)
		{
			string result = string.Empty;
			if (File.Exists(username + ".key"))
			{
				FileStream fileStream = new FileStream(username + ".key", FileMode.Open);
				BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.BigEndianUnicode);
				try
				{
					result = binaryReader.ReadString();
				}
				catch
				{
					result = "";
				}
				binaryReader.Close();
				fileStream.Close();
			}
			return result;
		}
		public LogonResponseVO Logon(LogonRequestVO req)
		{
			LogonResponseVO logonResponseVO = new LogonResponseVO();
			LogonReqVO logonReqVO = new LogonReqVO();
			logonReqVO.UserID = req.UserID;
			logonReqVO.Password = req.Password;
			logonReqVO.RegisterWord = req.RegisterWord;
			logonReqVO.VersionInfo = req.VersionInfo;
			logonReqVO.LoginMark = req.LoginMark;
			logonReqVO.LogonType = req.LogonType;
			string la = SysLanguage.language.ToString();
			logonReqVO.La = la;
			if (logonReqVO.RegisterWord == null || logonReqVO.RegisterWord.Length == 0)
			{
				logonReqVO.RegisterWord = this.readKey(logonReqVO.UserID);
			}
			LogonRepVO logonRepVO = (LogonRepVO)this.com.commuteBYVO(logonReqVO, false);
			if (logonRepVO != null)
			{
				if (logonRepVO.Result.RetCode > 0L)
				{
					SysShareInfo.sessionID = logonRepVO.Result.RetCode;
					SysShareInfo.moduleID = logonRepVO.Result.ModuleID;
					logonResponseVO.RetCode = 0L;
					logonResponseVO.RetMessage = logonRepVO.Result.RetMessage;
					logonResponseVO.LastIP = logonRepVO.Result.LastIP;
					logonResponseVO.ChgPWD = logonRepVO.Result.ChgPWD;
					logonResponseVO.LastTime = logonRepVO.Result.LastTime;
					logonResponseVO.RandomKey = logonRepVO.Result.RandomKey;
					logonResponseVO.UserID = logonRepVO.Result.UserID;
					if (logonResponseVO.RandomKey != null && logonResponseVO.RandomKey.Length > 0)
					{
						this.recordKey(logonReqVO.UserID, logonResponseVO.RandomKey);
					}
				}
				else
				{
					logonResponseVO.RetCode = -1L;
					logonResponseVO.RetMessage = logonRepVO.Result.RetMessage;
				}
			}
			return logonResponseVO;
		}
		public void ReadInfo(List<M_TradeInfo> list)
		{
			FileStream fileStream = new FileStream(this.tradebuffer, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			try
			{
				while (true)
				{
					M_TradeInfo m_TradeInfo = new M_TradeInfo();
					BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
					FieldInfo[] fields = m_TradeInfo.GetType().GetFields(bindingAttr);
					FieldInfo[] array = fields;
					for (int i = 0; i < array.Length; i++)
					{
						FieldInfo fieldInfo = array[i];
						if (fieldInfo.FieldType.Name.Equals("String"))
						{
							string arg_5A_0 = fieldInfo.Name;
							fieldInfo.SetValue(m_TradeInfo, this.decode(binaryReader.ReadString()));
						}
					}
					list.Add(m_TradeInfo);
				}
			}
			catch (Exception)
			{
				Console.WriteLine("\n\n读取结束！");
			}
			binaryReader.Close();
			fileStream.Close();
		}
		public void ReadWeekOrderInfo(List<WeekOrderInfo> list)
		{
			FileStream fileStream = new FileStream(this.orderbuffer, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			try
			{
				while (true)
				{
					WeekOrderInfo weekOrderInfo = new WeekOrderInfo();
					BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
					FieldInfo[] fields = weekOrderInfo.GetType().GetFields(bindingAttr);
					FieldInfo[] array = fields;
					for (int i = 0; i < array.Length; i++)
					{
						FieldInfo fieldInfo = array[i];
						if (fieldInfo.FieldType.Name.Equals("String"))
						{
							string arg_5A_0 = fieldInfo.Name;
							fieldInfo.SetValue(weekOrderInfo, this.decode(binaryReader.ReadString()));
						}
					}
					list.Add(weekOrderInfo);
				}
			}
			catch (Exception)
			{
				Console.WriteLine("\n\n读取结束！");
			}
			binaryReader.Close();
			fileStream.Close();
		}
		public void ReadOrderTimeInfo()
		{
			FileStream fileStream = new FileStream(this.ordertimebuffer, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			try
			{
				this.weekOrderUpdateTime = binaryReader.ReadInt64();
			}
			catch (Exception)
			{
				Console.WriteLine("\n\n读取结束！");
			}
			binaryReader.Close();
			fileStream.Close();
		}
		public void WriteInfo(List<M_TradeInfo> list)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.WriteTradeBuffer));
			thread.Start(list);
		}
		private void WriteTradeBuffer(object o)
		{
			lock (this.lockObject)
			{
				try
				{
					List<M_TradeInfo> list = (List<M_TradeInfo>)o;
					FileStream fileStream = null;
					if (File.Exists(this.tradebuffer))
					{
						fileStream = new FileStream(this.tradebuffer, FileMode.Append);
					}
					else
					{
						fileStream = new FileStream(this.tradebuffer, FileMode.Create);
					}
					BinaryWriter binaryWriter = new BinaryWriter(fileStream);
					foreach (M_TradeInfo current in list)
					{
						string[] array = new string[]
						{
							current.TradeNO.ToString(),
							current.OrderNO.ToString(),
							current.TradeTime,
							current.BuySell.ToString(),
							current.SettleBasis.ToString(),
							current.TraderID,
							current.FirmID,
							current.CustomerID,
							current.CommodityID,
							current.TradePrice.ToString(),
							current.TradeQuantity.ToString(),
							current.TransferPrice.ToString(),
							current.TransferPL.ToString(),
							current.Comm.ToString(),
							current.STradeNO.ToString(),
							current.ATradeNO.ToString(),
							current.TradeType.ToString()
						};
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != null)
							{
								binaryWriter.Write(this.encode(array[i]));
							}
							else
							{
								binaryWriter.Write("");
							}
						}
						binaryWriter.Flush();
					}
					binaryWriter.Close();
					fileStream.Close();
				}
				catch (Exception ex)
				{
					Logger.wirte(MsgType.Error, "WriteInfo():" + ex.Message);
				}
			}
		}
		public void WriteWeekOrderInfo(List<WeekOrderInfo> list)
		{
			FileStream fileStream = null;
			fileStream = new FileStream(this.orderbuffer, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			foreach (WeekOrderInfo current in list)
			{
				string[] array = new string[]
				{
					current.OrderNO.ToString(),
					current.Time.ToString(),
					current.State.ToString(),
					current.BuySell.ToString(),
					current.SettleBasis.ToString(),
					current.TraderID.ToString(),
					current.FirmID.ToString(),
					current.CustomerID.ToString(),
					current.CommodityID.ToString(),
					current.OrderPrice.ToString(),
					current.OrderQuantity.ToString(),
					current.Balance.ToString(),
					current.LPrice.ToString(),
					current.WithDrawTime.ToString(),
					current.CBasis.ToString(),
					current.BillTradeType.ToString()
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						binaryWriter.Write(this.encode(array[i]));
					}
					else
					{
						binaryWriter.Write("");
					}
				}
				binaryWriter.Flush();
			}
			binaryWriter.Close();
			fileStream.Close();
		}
		public void WriteOrderTimeInfo(long updateTime)
		{
			FileStream fileStream = new FileStream(this.ordertimebuffer, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(updateTime);
			binaryWriter.Close();
			fileStream.Close();
		}
		public string encode(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += (char)((int)(str[i] + '\f') - i * 2);
			}
			return text;
		}
		public string decode(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += (char)((int)(str[i] - '\f') + i * 2);
			}
			return text;
		}
		public ResponseVO Logoff(string userID)
		{
			ResponseVO result = new ResponseVO();
			WaitCallback callBack = new WaitCallback(this.LogoffReqVO);
			ThreadPool.QueueUserWorkItem(callBack, userID);
			return result;
		}
		private void LogoffReqVO(object userID)
		{
			LogoffReqVO logoffReqVO = new LogoffReqVO();
			logoffReqVO.UserID = userID.ToString();
			logoffReqVO.SessionID = SysShareInfo.sessionID;
			LogoffRepVO logoffRepVO = (LogoffRepVO)this.com.commuteBYVO(logoffReqVO, false);
			if (logoffRepVO != null)
			{
				this.com.cookies = new Dictionary<string, Cookie>();
			}
		}
		public ResponseVO CheckUser(CheckUserRequestVO requestVO)
		{
			ResponseVO responseVO = new ResponseVO();
			CheckUserReqVO checkUserReqVO = new CheckUserReqVO();
			checkUserReqVO.UserID = requestVO.UserID;
			checkUserReqVO.SessionID = SysShareInfo.sessionID;
			checkUserReqVO.ModuleID = SysShareInfo.moduleID;
			checkUserReqVO.FromLogonType = SysShareInfo.FromLogonType;
			checkUserReqVO.LogonType = requestVO.LogonType;
			CheckUserRepVO checkUserRepVO = (CheckUserRepVO)this.com.commuteBYVO(checkUserReqVO, this.isConsumerPlugin);
			if (checkUserRepVO != null)
			{
				responseVO.RetCode = checkUserRepVO.Result.RetCode;
				responseVO.RetMessage = checkUserRepVO.Result.RetMessage;
				if (responseVO.RetCode == 0L)
				{
					SysShareInfo.moduleID = checkUserRepVO.Result.ModuleID;
					if (this.isFirstCheckUser)
					{
						this.ReadTradeBuff(requestVO.UserID);
						this.isFirstCheckUser = false;
					}
				}
			}
			return responseVO;
		}
		private void ReadTradeBuff(string userID)
		{
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = userID;
			this.IsSaveLastID = false;
			SysTimeQueryResponseVO sysTime = this.GetSysTime(sysTimeQueryRequestVO);
			string tradeDay = sysTime.TradeDay;
			this.tradebuffer = string.Concat(new string[]
			{
				this.buffPath,
				"\\",
				tradeDay,
				userID,
				this.tradebuffer
			});
			FileInfo fileInfo = new FileInfo(this.tradebuffer);
			if (fileInfo.Exists)
			{
				this.isWriteFile = true;
				this.ReadInfo(this.tradeListBuffer);
				if (this.tradeListBuffer != null && this.tradeListBuffer.Count > 0)
				{
					this.lastTradeId = this.tradeListBuffer[this.tradeListBuffer.Count - 1].TradeNO;
				}
			}
			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" + this.buffPath, "*" + userID + "tradebuffer.buf", SearchOption.TopDirectoryOnly);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string fileName = array[i];
				FileInfo fileInfo2 = new FileInfo(fileName);
				if (fileInfo2.Name != this.tradebuffer.Substring(this.tradebuffer.LastIndexOf("\\") + 1))
				{
					fileInfo2.Delete();
				}
			}
		}
		public ResponseVO ChangePwd(ChgPwdRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			ChgPwdReqVO chgPwdReqVO = new ChgPwdReqVO();
			chgPwdReqVO.UserID = req.UserID;
			chgPwdReqVO.OldPassword = req.OldPassword;
			chgPwdReqVO.NewPassword = req.NewPassword;
			chgPwdReqVO.SessionID = SysShareInfo.sessionID;
			ChgPwdRepVO chgPwdRepVO = (ChgPwdRepVO)this.com.commuteBYVO(chgPwdReqVO, false);
			if (chgPwdRepVO != null)
			{
				responseVO.RetCode = chgPwdRepVO.Result.RetCode;
				responseVO.RetMessage = chgPwdRepVO.Result.RetMessage;
			}
			return responseVO;
		}
		public FirmInfoResponseVO GetFirmInfo(string userID)
		{
			FirmInfoResponseVO firmInfoResponseVO = new FirmInfoResponseVO();
			FirmInfoReqVO firmInfoReqVO = new FirmInfoReqVO();
			firmInfoReqVO.UserID = userID;
			firmInfoReqVO.SessionID = SysShareInfo.sessionID;
			FirmInfoRepVO firmInfoRepVO = (FirmInfoRepVO)this.com.commuteBYVO(firmInfoReqVO, false);
			if (firmInfoRepVO != null)
			{
				firmInfoResponseVO.RetCode = firmInfoRepVO.Result.RetCode;
				firmInfoResponseVO.RetMessage = firmInfoRepVO.Result.RetMessage;
				List<FirmInfo> firmInfoList = firmInfoRepVO.ResultList.FirmInfoList;
				if (firmInfoList != null && firmInfoList.Count > 0)
				{
					FirmInfo firmInfo = firmInfoList[0];
					firmInfoResponseVO.FirmID = firmInfo.FirmID;
					firmInfoResponseVO.FirmName = firmInfo.FirmName;
					firmInfoResponseVO.InitFund = firmInfo.InitFund;
					firmInfoResponseVO.YesterdayBail = firmInfo.YesterdayBail;
					firmInfoResponseVO.YesterdayFL = firmInfo.YesterdayFL;
					firmInfoResponseVO.CurrentBail = firmInfo.CurrentBail;
					firmInfoResponseVO.CurrentFL = firmInfo.CurrentFL;
					firmInfoResponseVO.OrderFrozenFund = firmInfo.OrderFrozenFund;
					firmInfoResponseVO.OtherFrozenFund = firmInfo.OtherFrozenFund;
					firmInfoResponseVO.RealFund = firmInfo.RealFund;
					firmInfoResponseVO.Fee = firmInfo.Fee;
					firmInfoResponseVO.TransferPL = firmInfo.TransferPL;
					if (firmInfo.CodeList.M_CodeList != null && firmInfo.CodeList.M_CodeList.Count > 0)
					{
						List<HttpTrade.Gnnt.MEBS.VO.Code> m_CodeList = firmInfo.CodeList.M_CodeList;
						if (firmInfoResponseVO.CDS == null)
						{
							firmInfoResponseVO.CDS = new List<TradeInterface.Gnnt.DataVO.Code>();
						}
						for (int i = 0; i < m_CodeList.Count; i++)
						{
							TradeInterface.Gnnt.DataVO.Code code = new TradeInterface.Gnnt.DataVO.Code();
							code.CD = m_CodeList[i].M_Code;
							firmInfoResponseVO.CDS.Add(code);
						}
					}
					firmInfoResponseVO.CurrentRight = firmInfo.CurrentRight;
					firmInfoResponseVO.InOutFund = firmInfo.InOutFund;
					firmInfoResponseVO.HoldingPL = firmInfo.HoldingPL;
					firmInfoResponseVO.OtherChange = firmInfo.OtherChange;
					firmInfoResponseVO.ImpawnFund = firmInfo.ImpawnFund;
					firmInfoResponseVO.Status = firmInfo.Status;
					firmInfoResponseVO.AMHolding = firmInfo.AMHolding;
					firmInfoResponseVO.CurMHolding = firmInfo.CurMHolding;
					firmInfoResponseVO.CurrentOpen = firmInfo.CurrentOpen;
					firmInfoResponseVO.MinFund = firmInfo.MinFund;
					firmInfoResponseVO.SFund = firmInfo.SFund;
					firmInfoResponseVO.CustomerID = firmInfo.CustomerID;
					firmInfoResponseVO.CustomerName = firmInfo.CustomerName;
				}
			}
			return firmInfoResponseVO;
		}
		public ResponseVO Order(OrderRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			OrderReqVO orderReqVO = new OrderReqVO();
			orderReqVO.UserID = req.UserID;
			orderReqVO.CustomerID = req.CustomerID;
			orderReqVO.BuySell = req.BuySell;
			orderReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			orderReqVO.Price = req.Price;
			orderReqVO.Quantity = req.Quantity;
			orderReqVO.SettleBasis = req.SettleBasis;
			orderReqVO.CloseMode = req.CloseMode;
			orderReqVO.TimeFlag = req.TimeFlag;
			orderReqVO.LPrice = req.LPrice;
			orderReqVO.SessionID = SysShareInfo.sessionID;
			orderReqVO.BillType = req.BillType;
			OrderRepVO orderRepVO = (OrderRepVO)this.com.commuteBYVO(orderReqVO, false);
			if (orderRepVO != null)
			{
				try
				{
					responseVO.RetCode = orderRepVO.Result.RetCode;
					responseVO.RetMessage = orderRepVO.Result.RetMessage;
					if (responseVO.RetCode == 0L && orderRepVO.Result.OrderNo > 0L)
					{
						responseVO.OrderNo = orderRepVO.Result.OrderNo;
						responseVO.orderTime = orderRepVO.Result.Time;
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(MsgType.Error, "Order()方法出错，错误信息：" + ex.Message + "    错误位置：" + ex.StackTrace);
				}
			}
			return responseVO;
		}
		public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			WithDrawOrderReqVO withDrawOrderReqVO = new WithDrawOrderReqVO();
			withDrawOrderReqVO.UserID = req.UserID;
			withDrawOrderReqVO.OrderNo = req.OrderNo;
			withDrawOrderReqVO.SessionID = SysShareInfo.sessionID;
			WithDrawOrderRepVO withDrawOrderRepVO = (WithDrawOrderRepVO)this.com.commuteBYVO(withDrawOrderReqVO, false);
			if (withDrawOrderRepVO != null)
			{
				responseVO.RetCode = withDrawOrderRepVO.Result.RetCode;
				responseVO.RetMessage = withDrawOrderRepVO.Result.RetMessage;
			}
			return responseVO;
		}
		public TradeQueryResponseVO TradeQuery(TradeQueryRequestVO req)
		{
			TradeQueryResponseVO tradeQueryResponseVO = new TradeQueryResponseVO();
			TradeQueryReqVO tradeQueryReqVO = new TradeQueryReqVO();
			tradeQueryReqVO.UserID = req.UserID;
			tradeQueryReqVO.MarketID = req.MarketID;
			tradeQueryReqVO.LastTradeID = this.lastTradeId;
			tradeQueryReqVO.SessionID = SysShareInfo.sessionID;
			TradeQueryRepVO tradeQueryRepVO = (TradeQueryRepVO)this.com.commuteBYVO(tradeQueryReqVO, false);
			if (tradeQueryRepVO != null)
			{
				tradeQueryResponseVO.RetCode = tradeQueryRepVO.Result.RetCode;
				tradeQueryResponseVO.RetMessage = tradeQueryRepVO.Result.RetMessage;
				tradeQueryResponseVO.TotalRecord = tradeQueryRepVO.Result.TotalRecord;
				if (tradeQueryRepVO.ResultList.TradeInfoList != null)
				{
					if (this.tradeListBuffer.Count > 0)
					{
						while (tradeQueryRepVO.ResultList.TradeInfoList.Count > 0 && tradeQueryRepVO.ResultList.TradeInfoList[0].TradeNO <= this.tradeListBuffer[this.tradeListBuffer.Count - 1].TradeNO)
						{
							tradeQueryRepVO.ResultList.TradeInfoList.RemoveAt(0);
						}
					}
					if (this.isWriteFile)
					{
						this.WriteInfo(tradeQueryRepVO.ResultList.TradeInfoList);
					}
					this.tradeListBuffer.AddRange(tradeQueryRepVO.ResultList.TradeInfoList);
					if (this.tradeListBuffer.Count >= 500 && !this.isWriteFile)
					{
						this.WriteInfo(this.tradeListBuffer);
						this.isWriteFile = true;
					}
				}
				List<M_TradeInfo> list = this.tradeListBuffer;
				if (list != null && list.Count > 0)
				{
					if (tradeQueryResponseVO.TradeInfoList == null)
					{
						tradeQueryResponseVO.TradeInfoList = new List<TradeInfo>();
					}
					for (int i = 0; i < list.Count; i++)
					{
						M_TradeInfo m_TradeInfo = list[i];
						if (this.lastPagingTradeId == 0L && m_TradeInfo.TradeNO > this.lastTradeId)
						{
							this.lastTradeId = m_TradeInfo.TradeNO;
						}
						if (req.MarketID == null || req.MarketID.Length <= 0 || this.GetMarketAndeComm(m_TradeInfo.CommodityID).marketID.Equals(req.MarketID))
						{
							TradeInfo tradeInfo = new TradeInfo();
							tradeInfo.TradeNO = m_TradeInfo.TradeNO;
							tradeInfo.OrderNO = m_TradeInfo.OrderNO;
							tradeInfo.TradeTime = m_TradeInfo.TradeTime;
							tradeInfo.BuySell = m_TradeInfo.BuySell;
							tradeInfo.SettleBasis = m_TradeInfo.SettleBasis;
							tradeInfo.TraderID = m_TradeInfo.TraderID;
							tradeInfo.FirmID = m_TradeInfo.FirmID;
							tradeInfo.CustomerID = m_TradeInfo.CustomerID;
							MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeInfo.CommodityID);
							tradeInfo.MarketID = marketAndeComm.marketID;
							tradeInfo.CommodityID = marketAndeComm.commodityID;
							tradeInfo.TradePrice = m_TradeInfo.TradePrice;
							tradeInfo.TradeQuantity = m_TradeInfo.TradeQuantity;
							tradeInfo.TransferPrice = m_TradeInfo.TransferPrice;
							tradeInfo.TransferPL = m_TradeInfo.TransferPL;
							tradeInfo.Comm = m_TradeInfo.Comm;
							tradeInfo.STradeNO = m_TradeInfo.STradeNO;
							tradeInfo.ATradeNO = m_TradeInfo.ATradeNO;
							tradeInfo.TradeType = m_TradeInfo.TradeType;
							tradeQueryResponseVO.TradeInfoList.Add(tradeInfo);
						}
					}
				}
			}
			return tradeQueryResponseVO;
		}
		public SysTimeQueryResponseVO GetSysTime(SysTimeQueryRequestVO req)
		{
			SysTimeQueryResponseVO sysTimeQueryResponseVO = new SysTimeQueryResponseVO();
			SysTimeQueryReqVO sysTimeQueryReqVO = new SysTimeQueryReqVO();
			sysTimeQueryReqVO.UserID = req.UserID;
			sysTimeQueryReqVO.LastID = this.lastID;
			if (this.tradeTotal == -1L)
			{
				this.tradeTotal = 0L;
				sysTimeQueryReqVO.TradeCount = this.tradeTotal;
				sysTimeQueryReqVO.CurLogon = 1L;
			}
			else
			{
				sysTimeQueryReqVO.TradeCount = this.tradeTotal;
				sysTimeQueryReqVO.CurLogon = 0L;
			}
			sysTimeQueryReqVO.SessionID = SysShareInfo.sessionID;
			sysTimeQueryReqVO.UpdateTime = this.weekConditionOrderUpdateTime;
			SysTimeQueryRepVO sysTimeQueryRepVO = (SysTimeQueryRepVO)this.com.commuteBYVO(sysTimeQueryReqVO, false);
			if (sysTimeQueryRepVO != null)
			{
				sysTimeQueryResponseVO.RetCode = sysTimeQueryRepVO.Result.RetCode;
				sysTimeQueryResponseVO.RetMessage = sysTimeQueryRepVO.Result.RetMessage;
				sysTimeQueryResponseVO.CurrentTime = sysTimeQueryRepVO.Result.CurrentTime;
				sysTimeQueryResponseVO.CurrentDate = sysTimeQueryRepVO.Result.CurrentDate;
				sysTimeQueryResponseVO.MicroSecond = sysTimeQueryRepVO.Result.MicroSecond;
				sysTimeQueryResponseVO.TradeDay = sysTimeQueryRepVO.Result.TradeDay;
				sysTimeQueryResponseVO.RefreshMark = sysTimeQueryRepVO.Result.RefreshMark;
				if (sysTimeQueryRepVO.Result.LastID != -1L && this.IsSaveLastID)
				{
					this.lastID = sysTimeQueryRepVO.Result.LastID;
				}
				this.IsSaveLastID = true;
				if (sysTimeQueryRepVO.Result.BroadcastList.M_Broadcast != null && sysTimeQueryRepVO.Result.BroadcastList.M_Broadcast.Count > 0)
				{
					List<M_Broadcast> m_Broadcast = sysTimeQueryRepVO.Result.BroadcastList.M_Broadcast;
					if (sysTimeQueryResponseVO.BroadcastList == null)
					{
						sysTimeQueryResponseVO.BroadcastList = new List<Broadcast>();
					}
					for (int i = 0; i < m_Broadcast.Count; i++)
					{
						Broadcast broadcast = new Broadcast();
						broadcast.content = m_Broadcast[i].BCContent;
						sysTimeQueryResponseVO.BroadcastList.Add(broadcast);
					}
				}
				sysTimeQueryResponseVO.NewTrade = sysTimeQueryRepVO.Result.NewTrade;
				sysTimeQueryResponseVO.TradeTotal = sysTimeQueryRepVO.Result.TradeTotal;
				if (sysTimeQueryResponseVO.TradeTotal != -1L)
				{
					this.tradeTotal = sysTimeQueryResponseVO.TradeTotal;
				}
				if (sysTimeQueryResponseVO.NewTrade == 1 && this.tradeTotal != this.ltradeTotal)
				{
					this.ltradeTotal = this.tradeTotal;
					if (sysTimeQueryRepVO.Result.TradeMessageList.M_TradeMessage != null && sysTimeQueryRepVO.Result.TradeMessageList.M_TradeMessage.Count > 0)
					{
						List<M_TradeMessage> m_TradeMessage = sysTimeQueryRepVO.Result.TradeMessageList.M_TradeMessage;
						if (sysTimeQueryResponseVO.TradeMessageList == null)
						{
							sysTimeQueryResponseVO.TradeMessageList = new List<TradeMessage>();
						}
						for (int j = 0; j < m_TradeMessage.Count; j++)
						{
							TradeMessage tradeMessage = new TradeMessage();
							tradeMessage.OrderNO = m_TradeMessage[j].OrderNO;
							MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeMessage[j].CommodityID);
							tradeMessage.MarketID = marketAndeComm.marketID;
							tradeMessage.CommodityID = marketAndeComm.commodityID;
							tradeMessage.TradeQuatity = m_TradeMessage[j].TradeQuatity;
							sysTimeQueryResponseVO.TradeMessageList.Add(tradeMessage);
						}
					}
				}
				if (this.curTradeDay != null && this.curTradeDay.Length > 0 && !this.curTradeDay.Equals(sysTimeQueryRepVO.Result.TradeDay))
				{
					FileInfo fileInfo = new FileInfo(this.tradebuffer);
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
					}
					this.Dispose();
				}
				this.curTradeDay = sysTimeQueryRepVO.Result.TradeDay;
			}
			return sysTimeQueryResponseVO;
		}
		public OrderQueryResponseVO OrderQuery(OrderQueryRequestVO req)
		{
			OrderQueryResponseVO orderQueryResponseVO = this.AllOrderQuery(req);
			orderQueryResponseVO.OrderInfoList = orderQueryResponseVO.OrderInfoList.FindAll((OrderInfo o) => o.State == 1 || o.State == 2);
			return orderQueryResponseVO;
		}
		public OrderQueryResponseVO OrderQuery()
		{
			OrderQueryResponseVO orderQueryResponseVO = this.AllOrderQuery();
			orderQueryResponseVO.OrderInfoList = orderQueryResponseVO.OrderInfoList.FindAll((OrderInfo o) => o.State == 1 || o.State == 2);
			return orderQueryResponseVO;
		}
		public OrderQueryResponseVO AllOrderQuery(OrderQueryRequestVO req)
		{
			OrderQueryResponseVO orderQueryResponseVO = new OrderQueryResponseVO();
			WeekOrderQueryReqVO weekOrderQueryReqVO = new WeekOrderQueryReqVO();
			weekOrderQueryReqVO.UserID = req.UserID;
			weekOrderQueryReqVO.MarketID = req.MarketID;
			weekOrderQueryReqVO.BuySell = req.BuySell;
			weekOrderQueryReqVO.OrderNO = req.OrderNO;
			weekOrderQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			weekOrderQueryReqVO.StartNum = req.StartNum;
			weekOrderQueryReqVO.RecordCount = req.RecordCount;
			weekOrderQueryReqVO.SessionID = SysShareInfo.sessionID;
			weekOrderQueryReqVO.UpdateTime = this.weekOrderUpdateTime;
			WeekOrderQueryRepVO weekOrderQueryRepVO = (WeekOrderQueryRepVO)this.com.commuteBYVO(weekOrderQueryReqVO, false);
			if (weekOrderQueryRepVO != null)
			{
				orderQueryResponseVO.RetCode = weekOrderQueryRepVO.Result.RetCode;
				orderQueryResponseVO.RetMessage = weekOrderQueryRepVO.Result.RetMessage;
				orderQueryResponseVO.TotalRecord = weekOrderQueryRepVO.Result.TotalRecord;
				this.weekOrderUpdateTime = weekOrderQueryRepVO.Result.UpdateTime;
				List<WeekOrderInfo> weekOrderList = weekOrderQueryRepVO.ResultList.WeekOrderList;
				if (weekOrderList != null && weekOrderList.Count > 0)
				{
					if (orderQueryResponseVO.OrderInfoList == null)
					{
						orderQueryResponseVO.OrderInfoList = new List<OrderInfo>();
					}
					for (int i = 0; i < weekOrderList.Count; i++)
					{
						WeekOrderInfo weekOrderInfo = weekOrderList[i];
						OrderInfo orderInfo = new OrderInfo();
						orderInfo.OrderNO = weekOrderInfo.OrderNO;
						orderInfo.Time = weekOrderInfo.Time;
						orderInfo.State = weekOrderInfo.State;
						orderInfo.BuySell = weekOrderInfo.BuySell;
						orderInfo.SettleBasis = weekOrderInfo.SettleBasis;
						orderInfo.TraderID = weekOrderInfo.TraderID;
						orderInfo.FirmID = weekOrderInfo.FirmID;
						orderInfo.CustomerID = weekOrderInfo.CustomerID;
						orderInfo.CBasis = 3;
						if (weekOrderInfo.CBasis >= 0)
						{
							orderInfo.CBasis = weekOrderInfo.CBasis;
						}
						orderInfo.BillTradeType = 0;
						if (weekOrderInfo.BillTradeType > 0)
						{
							orderInfo.BillTradeType = weekOrderInfo.BillTradeType;
						}
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(weekOrderInfo.CommodityID);
						orderInfo.MarketID = marketAndeComm.marketID;
						orderInfo.CommodityID = marketAndeComm.commodityID;
						orderInfo.OrderPrice = weekOrderInfo.OrderPrice;
						orderInfo.OrderQuantity = weekOrderInfo.OrderQuantity;
						orderInfo.Balance = weekOrderInfo.Balance;
						orderInfo.LPrice = weekOrderInfo.LPrice;
						orderInfo.WithDrawTime = weekOrderInfo.WithDrawTime;
						orderQueryResponseVO.OrderInfoList.Add(orderInfo);
					}
				}
			}
			return orderQueryResponseVO;
		}
		public OrderQueryResponseVO AllOrderQuery()
		{
			return new OrderQueryResponseVO();
		}
		public HoldingDetailResponseVO HoldPtByPrice(HoldingDetailRequestVO req)
		{
			HoldingDetailResponseVO holdingDetailResponseVO = new HoldingDetailResponseVO();
			HoldingDetailReqVO holdingDetailReqVO = new HoldingDetailReqVO();
			holdingDetailReqVO.UserID = req.UserID;
			holdingDetailReqVO.MarketID = req.MarketID;
			holdingDetailReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			holdingDetailReqVO.StartNum = req.StartNum;
			holdingDetailReqVO.RecordCount = req.RecordCount;
			holdingDetailReqVO.SessionID = SysShareInfo.sessionID;
			HoldingDetailRepVO holdingDetailRepVO = (HoldingDetailRepVO)this.com.commuteBYVO(holdingDetailReqVO, false);
			if (holdingDetailRepVO != null)
			{
				holdingDetailResponseVO.RetCode = holdingDetailRepVO.Result.RetCode;
				holdingDetailResponseVO.RetMessage = holdingDetailRepVO.Result.RetMessage;
				holdingDetailResponseVO.TotalRecord = holdingDetailRepVO.Result.TotalRecord;
				List<M_HoldingDetailInfo> holdingDetailInfoList = holdingDetailRepVO.ResultList.HoldingDetailInfoList;
				if (holdingDetailInfoList != null && holdingDetailInfoList.Count > 0)
				{
					if (holdingDetailResponseVO.HoldingDetailInfoList == null)
					{
						holdingDetailResponseVO.HoldingDetailInfoList = new List<HoldingDetailInfo>();
					}
					for (int i = 0; i < holdingDetailInfoList.Count; i++)
					{
						M_HoldingDetailInfo m_HoldingDetailInfo = holdingDetailInfoList[i];
						HoldingDetailInfo holdingDetailInfo = new HoldingDetailInfo();
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_HoldingDetailInfo.CommodityID);
						holdingDetailInfo.CommodityID = marketAndeComm.commodityID;
						holdingDetailInfo.CustomerID = m_HoldingDetailInfo.CustomerID;
						holdingDetailInfo.BuySell = m_HoldingDetailInfo.BuySell;
						holdingDetailInfo.AmountOnOrder = m_HoldingDetailInfo.AmountOnOrder;
						holdingDetailInfo.Price = m_HoldingDetailInfo.Price;
						holdingDetailInfo.GOQuantity = m_HoldingDetailInfo.GOQuantity;
						holdingDetailInfo.Bail = m_HoldingDetailInfo.Bail;
						holdingDetailInfo.DeadLine = m_HoldingDetailInfo.DeadLine;
						holdingDetailInfo.RemainDay = m_HoldingDetailInfo.RemainDay;
						holdingDetailInfo.holddate = m_HoldingDetailInfo.holddate;
						holdingDetailResponseVO.HoldingDetailInfoList.Add(holdingDetailInfo);
					}
				}
			}
			return holdingDetailResponseVO;
		}
		public HoldingQueryResponseVO HoldingQuery(HoldingQueryRequestVO req)
		{
			HoldingQueryResponseVO holdingQueryResponseVO = new HoldingQueryResponseVO();
			HoldingQueryReqVO holdingQueryReqVO = new HoldingQueryReqVO();
			holdingQueryReqVO.UserID = req.UserID;
			holdingQueryReqVO.MarketID = req.MarketID;
			holdingQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			holdingQueryReqVO.StartNum = req.StartNum;
			holdingQueryReqVO.RecordCount = req.RecordCount;
			holdingQueryReqVO.SessionID = SysShareInfo.sessionID;
			HoldingQueryRepVO holdingQueryRepVO = (HoldingQueryRepVO)this.com.commuteBYVO(holdingQueryReqVO, false);
			if (holdingQueryRepVO != null)
			{
				holdingQueryResponseVO.RetCode = holdingQueryRepVO.Result.RetCode;
				holdingQueryResponseVO.RetMessage = holdingQueryRepVO.Result.RetMessage;
				holdingQueryResponseVO.TotalRecord = holdingQueryRepVO.Result.TotalRecord;
				List<M_HoldingInfo> holdingInfoList = holdingQueryRepVO.ResultList.HoldingInfoList;
				if (holdingInfoList != null && holdingInfoList.Count > 0)
				{
					if (holdingQueryResponseVO.HoldingInfoList == null)
					{
						holdingQueryResponseVO.HoldingInfoList = new List<HoldingInfo>();
					}
					for (int i = 0; i < holdingInfoList.Count; i++)
					{
						M_HoldingInfo m_HoldingInfo = holdingInfoList[i];
						HoldingInfo holdingInfo = new HoldingInfo();
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_HoldingInfo.CommodityID);
						holdingInfo.MarketID = marketAndeComm.marketID;
						holdingInfo.CommodityID = marketAndeComm.commodityID;
						holdingInfo.CustomerID = m_HoldingInfo.CustomerID;
						holdingInfo.BuyHolding = m_HoldingInfo.BuyHolding;
						holdingInfo.SellHolding = m_HoldingInfo.SellHolding;
						holdingInfo.BuyVHolding = m_HoldingInfo.BuyVHolding;
						holdingInfo.SellVHolding = m_HoldingInfo.SellVHolding;
						holdingInfo.BuyAverage = m_HoldingInfo.BuyAverage;
						holdingInfo.SellAverage = m_HoldingInfo.SellAverage;
						holdingInfo.GOQuantity = m_HoldingInfo.GOQuantity;
						holdingInfo.FloatingLP = m_HoldingInfo.FloatingLP;
						holdingInfo.Bail = m_HoldingInfo.Bail;
						holdingInfo.NewPriceLP = m_HoldingInfo.NewPriceLP;
						holdingQueryResponseVO.HoldingInfoList.Add(holdingInfo);
					}
				}
			}
			return holdingQueryResponseVO;
		}
		public CommDataQueryResponseVO CommDataQuery(CommDataQueryRequestVO req)
		{
			CommDataQueryResponseVO commDataQueryResponseVO = new CommDataQueryResponseVO();
			CommDataQueryReqVO commDataQueryReqVO = new CommDataQueryReqVO();
			commDataQueryReqVO.UserID = req.UserID;
			commDataQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			commDataQueryReqVO.SessionID = SysShareInfo.sessionID;
			CommDataQueryRepVO commDataQueryRepVO = (CommDataQueryRepVO)this.com.commuteBYVO(commDataQueryReqVO, false);
			if (commDataQueryRepVO != null)
			{
				commDataQueryResponseVO.RetCode = commDataQueryRepVO.Result.RetCode;
				commDataQueryResponseVO.RetMessage = commDataQueryRepVO.Result.RetMessage;
				List<M_CommData> commDataList = commDataQueryRepVO.ResultList.CommDataList;
				if (commDataList != null && commDataList.Count > 0)
				{
					if (commDataQueryResponseVO.CommDataList == null)
					{
						commDataQueryResponseVO.CommDataList = new List<CommData>();
					}
					for (int i = 0; i < commDataList.Count; i++)
					{
						M_CommData m_CommData = commDataList[i];
						CommData commData = new CommData();
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_CommData.CommodityID);
						commData.MarketID = marketAndeComm.marketID;
						commData.CommodityID = marketAndeComm.commodityID;
						commData.CommodityName = m_CommData.CommodityName;
						commData.DeliveryDate = m_CommData.DeliveryDate;
						commData.PrevClear = m_CommData.PrevClear;
						commData.Bid = m_CommData.Bid;
						commData.BidVol = m_CommData.BidVol;
						commData.Offer = m_CommData.Offer;
						commData.OfferVol = m_CommData.OfferVol;
						commData.High = m_CommData.High;
						commData.Low = m_CommData.Low;
						commData.Last = m_CommData.Last;
						commData.Avg = m_CommData.Avg;
						commData.Change = m_CommData.Change;
						commData.VolToday = m_CommData.VolToday;
						commData.TTOpen = m_CommData.TTOpen;
						commDataQueryResponseVO.CommDataList.Add(commData);
					}
				}
			}
			return commDataQueryResponseVO;
		}
		public CommodityQueryResponseVO CommodityQuery(CommodityQueryRequestVO req)
		{
			CommodityQueryResponseVO commodityQueryResponseVO = new CommodityQueryResponseVO();
			CommodityQueryReqVO commodityQueryReqVO = new CommodityQueryReqVO();
			commodityQueryReqVO.UserID = req.UserID;
			commodityQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			commodityQueryReqVO.SessionID = SysShareInfo.sessionID;
			CommodityQueryRepVO commodityQueryRepVO = (CommodityQueryRepVO)this.com.commuteBYVO(commodityQueryReqVO, false);
			if (commodityQueryRepVO != null)
			{
				commodityQueryResponseVO.RetCode = commodityQueryRepVO.Result.RetCode;
				commodityQueryResponseVO.RetMessage = commodityQueryRepVO.Result.RetMessage;
				List<M_CommodityInfo> commodityList = commodityQueryRepVO.ResultList.CommodityList;
				if (commodityList != null && commodityList.Count > 0)
				{
					if (commodityQueryResponseVO.CommodityInfoList == null)
					{
						commodityQueryResponseVO.CommodityInfoList = new List<CommodityInfo>();
					}
					for (int i = 0; i < commodityList.Count; i++)
					{
						M_CommodityInfo m_CommodityInfo = commodityList[i];
						CommodityInfo commodityInfo = new CommodityInfo();
						commodityInfo.MarketID = m_CommodityInfo.MarketID;
						commodityInfo.CommodityID = m_CommodityInfo.CommodityID;
						commodityInfo.CommodityName = m_CommodityInfo.CommodityName;
						commodityInfo.DeliveryDate = m_CommodityInfo.DeliveryDate;
						commodityInfo.Status = m_CommodityInfo.Status;
						commodityInfo.CtrtSize = m_CommodityInfo.CtrtSize;
						commodityInfo.Spread = m_CommodityInfo.Spread;
						commodityInfo.SpreadUp = m_CommodityInfo.SpreadUp;
						commodityInfo.SpreadDown = m_CommodityInfo.SpreadDown;
						commodityInfo.MarginType = m_CommodityInfo.MarginType;
						commodityInfo.BMargin = m_CommodityInfo.BMargin;
						commodityInfo.SMargin = m_CommodityInfo.SMargin;
						commodityInfo.BMargin_g = m_CommodityInfo.BMargin_g;
						commodityInfo.SMargin_g = m_CommodityInfo.SMargin_g;
						commodityInfo.PrevClear = m_CommodityInfo.PrevClear;
						commodityInfo.CommType = m_CommodityInfo.CommType;
						commodityInfo.BOpenComm = m_CommodityInfo.BOpenComm;
						commodityInfo.SOpenComm = m_CommodityInfo.SOpenComm;
						commodityInfo.BTHHComm = m_CommodityInfo.BTHHComm;
						commodityInfo.STHHComm = m_CommodityInfo.STHHComm;
						commodityInfo.BTTHComm = m_CommodityInfo.BTTHComm;
						commodityInfo.STTHComm = m_CommodityInfo.STTHComm;
						commodityInfo.BFTComm = m_CommodityInfo.BFTComm;
						commodityInfo.SFTComm = m_CommodityInfo.SFTComm;
						commodityInfo.DeliveryCommType = m_CommodityInfo.DeliveryCommType;
						commodityInfo.DeliveryBComm = m_CommodityInfo.DeliveryBComm;
						commodityInfo.DeliverySComm = m_CommodityInfo.DeliverySComm;
						commodityInfo.VarietyID = m_CommodityInfo.VarietyID;
						commodityInfo.TradeMode = m_CommodityInfo.TradeMode;
						commodityInfo.MinQty = m_CommodityInfo.MinQty;
						commodityInfo.MaxHoldDays = m_CommodityInfo.MaxHoldDays;
						commodityInfo.TaxRate = m_CommodityInfo.TaxRate;
						commodityQueryResponseVO.CommodityInfoList.Add(commodityInfo);
					}
				}
			}
			return commodityQueryResponseVO;
		}
		public MarketQueryResponseVO MarketQuery(MarketQueryRequestVO req)
		{
			MarketQueryResponseVO marketQueryResponseVO = new MarketQueryResponseVO();
			MarketQueryReqVO marketQueryReqVO = new MarketQueryReqVO();
			marketQueryReqVO.UserID = req.UserID;
			marketQueryReqVO.MarketID = req.MarketID;
			marketQueryReqVO.SessionID = SysShareInfo.sessionID;
			MarketQueryRepVO marketQueryRepVO = (MarketQueryRepVO)this.com.commuteBYVO(marketQueryReqVO, false);
			if (marketQueryRepVO != null)
			{
				marketQueryResponseVO.RetCode = marketQueryRepVO.Result.RetCode;
				marketQueryResponseVO.RetMessage = marketQueryRepVO.Result.RetMessage;
				List<M_MarkeInfo> markeInfoList = marketQueryRepVO.ResultList.MarkeInfoList;
				if (markeInfoList != null && markeInfoList.Count > 0)
				{
					if (marketQueryResponseVO.MarkeInfoList == null)
					{
						marketQueryResponseVO.MarkeInfoList = new List<MarkeInfo>();
					}
					for (int i = 0; i < markeInfoList.Count; i++)
					{
						M_MarkeInfo m_MarkeInfo = markeInfoList[i];
						MarkeInfo markeInfo = new MarkeInfo();
						markeInfo.MarketID = m_MarkeInfo.MarketID;
						markeInfo.MarketName = m_MarkeInfo.MarketName;
						markeInfo.Status = m_MarkeInfo.Status;
						markeInfo.FirmID = m_MarkeInfo.FirmID;
						markeInfo.MarginType = m_MarkeInfo.MarginType;
						markeInfo.ShortName = m_MarkeInfo.ShortName;
						marketQueryResponseVO.MarkeInfoList.Add(markeInfo);
					}
				}
			}
			return marketQueryResponseVO;
		}
		public FirmbreedQueryResponseVO FirmbreedQuery(string userID)
		{
			FirmbreedQueryResponseVO firmbreedQueryResponseVO = new FirmbreedQueryResponseVO();
			FirmBreedReqVO firmBreedReqVO = new FirmBreedReqVO();
			firmBreedReqVO.UserID = userID;
			firmBreedReqVO.SessionID = SysShareInfo.sessionID;
			FirmBreedRepVO firmBreedRepVO = (FirmBreedRepVO)this.com.commuteBYVO(firmBreedReqVO, false);
			if (firmBreedRepVO != null)
			{
				firmbreedQueryResponseVO.RetCode = firmBreedRepVO.Result.RetCode;
				firmbreedQueryResponseVO.RetMessage = firmBreedRepVO.Result.RetMessage;
				List<F_FirmBreedInfo> firmBreedInfoList = firmBreedRepVO.ResultList.FirmBreedInfoList;
				if (firmBreedInfoList != null && firmBreedInfoList.Count > 0)
				{
					if (firmbreedQueryResponseVO.FirmBreedInfoList == null)
					{
						firmbreedQueryResponseVO.FirmBreedInfoList = new List<FirmBreedInfo>();
					}
					for (int i = 0; i < firmBreedInfoList.Count; i++)
					{
						F_FirmBreedInfo f_FirmBreedInfo = firmBreedInfoList[i];
						FirmBreedInfo firmBreedInfo = new FirmBreedInfo();
						firmBreedInfo.VarietyID = f_FirmBreedInfo.VarietytID;
						firmbreedQueryResponseVO.FirmBreedInfoList.Add(firmBreedInfo);
					}
				}
			}
			return firmbreedQueryResponseVO;
		}
		public QuerydateqtyResponseVO Querydateqty(QuerydateqtyRequestVO req)
		{
			QuerydateqtyResponseVO querydateqtyResponseVO = new QuerydateqtyResponseVO();
			QuerydateqtyReqVO querydateqtyReqVO = new QuerydateqtyReqVO();
			querydateqtyReqVO.UserID = req.UserID;
			querydateqtyReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			querydateqtyReqVO.SessionID = SysShareInfo.sessionID;
			querydateqtyReqVO.QueryName = req.QueryName;
			if (this.lastTradeId != 0L)
			{
				querydateqtyReqVO.Parameter = "TR_N:" + this.lastTradeId.ToString();
			}
			QuerydateqtyRepVO querydateqtyRepVO = (QuerydateqtyRepVO)this.com.commuteBYVO(querydateqtyReqVO, false);
			if (querydateqtyRepVO != null)
			{
				querydateqtyResponseVO.RetCode = querydateqtyRepVO.Result.RetCode;
				querydateqtyResponseVO.RetMessage = querydateqtyRepVO.Result.RetMessage;
				querydateqtyResponseVO.TotalRecord = querydateqtyRepVO.Result.TotalRecord;
				List<ResultListREC> resultListRec = querydateqtyRepVO.ResultList.ResultListRec;
				if (querydateqtyResponseVO.totalRowList == null)
				{
					querydateqtyResponseVO.totalRowList = new List<TotalRow>();
				}
				if (resultListRec != null && resultListRec.Count > 0)
				{
					for (int i = 0; i < resultListRec.Count; i++)
					{
						T_TotalRow totalRowList = resultListRec[i].TotalRowList;
						TotalRow totalRow = new TotalRow();
						totalRow.ResponseName = totalRowList.ResponseName;
						totalRow.TotalNum = totalRowList.TotalNum;
						totalRow.TotalQty = totalRowList.TotalQty;
						totalRow.UnTradeQty = totalRowList.UnTradeQty;
						totalRow.TotalLiqpl = totalRowList.TotalLiqpl;
						totalRow.TotalComm = totalRowList.TotalComm;
						querydateqtyResponseVO.totalRowList.Add(totalRow);
					}
				}
			}
			return querydateqtyResponseVO;
		}
		public OrderQueryPagingResponseVO AllOrderQueryPaging(OrderQueryPagingRequestVO req)
		{
			OrderQueryPagingResponseVO orderQueryPagingResponseVO = new OrderQueryPagingResponseVO();
			WeekOrderPagingQueryReqVO weekOrderPagingQueryReqVO = new WeekOrderPagingQueryReqVO();
			weekOrderPagingQueryReqVO.UserID = req.UserID;
			weekOrderPagingQueryReqVO.MarketID = req.MarketID;
			weekOrderPagingQueryReqVO.BuySell = req.BuySell;
			weekOrderPagingQueryReqVO.OrderNO = req.OrderNO;
			weekOrderPagingQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			weekOrderPagingQueryReqVO.StartNum = req.StartNum;
			weekOrderPagingQueryReqVO.RecordCount = req.RecordCount;
			weekOrderPagingQueryReqVO.SortFld = this.GetSortFld(req.SortFld);
			weekOrderPagingQueryReqVO.IsDesc = req.IsDesc;
			weekOrderPagingQueryReqVO.CurrentPagNum = req.CurrentPagNum;
			weekOrderPagingQueryReqVO.IsQueryAll = req.IsQueryAll;
			weekOrderPagingQueryReqVO.SessionID = SysShareInfo.sessionID;
			if (req.CurrentPagNum == 0)
			{
				weekOrderPagingQueryReqVO.UpdateTime = this.weekOrderPagingUpdateTime;
				if (req.IsQueryAll == 0 && this.weekOrderQueryFirst)
				{
					weekOrderPagingQueryReqVO.UpdateTime = 0L;
					this.weekOrderQueryFirst = false;
				}
			}
			if (weekOrderPagingQueryReqVO.UpdateTime == 0L && weekOrderPagingQueryReqVO.CurrentPagNum == 0)
			{
				weekOrderPagingQueryReqVO.CurrentPagNum = 1;
			}
			weekOrderPagingQueryReqVO.Pri = req.Pri;
			weekOrderPagingQueryReqVO.Type = req.Type;
			weekOrderPagingQueryReqVO.Sta = req.Sta;
			WeekOrderPagingQueryRepVO weekOrderPagingQueryRepVO = (WeekOrderPagingQueryRepVO)this.com.commuteBYVO(weekOrderPagingQueryReqVO, false);
			if (weekOrderPagingQueryRepVO != null)
			{
				orderQueryPagingResponseVO.RetCode = weekOrderPagingQueryRepVO.Result.RetCode;
				orderQueryPagingResponseVO.RetMessage = weekOrderPagingQueryRepVO.Result.RetMessage;
				orderQueryPagingResponseVO.TotalRecord = weekOrderPagingQueryRepVO.Result.TotalRecord;
				orderQueryPagingResponseVO.TotalRow.TotalNum = weekOrderPagingQueryRepVO.Result.TotalRow.TotalNum;
				orderQueryPagingResponseVO.TotalRow.TotalQty = weekOrderPagingQueryRepVO.Result.TotalRow.TotalQty;
				orderQueryPagingResponseVO.TotalRow.UnTradeQty = weekOrderPagingQueryRepVO.Result.TotalRow.UnTradeQty;
				if (req.CurrentPagNum == 0)
				{
					this.weekOrderPagingUpdateTime = weekOrderPagingQueryRepVO.Result.UpdateTime;
				}
				List<WeekOrderInfo> weekOrderList = weekOrderPagingQueryRepVO.ResultList.WeekOrderList;
				if (weekOrderList != null && weekOrderList.Count > 0)
				{
					if (orderQueryPagingResponseVO.OrderInfoList == null)
					{
						orderQueryPagingResponseVO.OrderInfoList = new List<OrderInfo>();
					}
					for (int i = 0; i < weekOrderList.Count; i++)
					{
						WeekOrderInfo weekOrderInfo = weekOrderList[i];
						OrderInfo orderInfo = new OrderInfo();
						orderInfo.OrderNO = weekOrderInfo.OrderNO;
						orderInfo.Time = weekOrderInfo.Time;
						orderInfo.State = weekOrderInfo.State;
						orderInfo.BuySell = weekOrderInfo.BuySell;
						orderInfo.SettleBasis = weekOrderInfo.SettleBasis;
						orderInfo.TraderID = weekOrderInfo.TraderID;
						orderInfo.FirmID = weekOrderInfo.FirmID;
						orderInfo.CustomerID = weekOrderInfo.CustomerID;
						orderInfo.CBasis = 3;
						if (weekOrderInfo.CBasis >= 0)
						{
							orderInfo.CBasis = weekOrderInfo.CBasis;
						}
						orderInfo.BillTradeType = 0;
						if (weekOrderInfo.BillTradeType > 0)
						{
							orderInfo.BillTradeType = weekOrderInfo.BillTradeType;
						}
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(weekOrderInfo.CommodityID);
						orderInfo.MarketID = marketAndeComm.marketID;
						orderInfo.CommodityID = marketAndeComm.commodityID;
						orderInfo.OrderPrice = weekOrderInfo.OrderPrice;
						orderInfo.OrderQuantity = weekOrderInfo.OrderQuantity;
						orderInfo.Balance = weekOrderInfo.Balance;
						orderInfo.LPrice = weekOrderInfo.LPrice;
						orderInfo.WithDrawTime = weekOrderInfo.WithDrawTime;
						orderQueryPagingResponseVO.OrderInfoList.Add(orderInfo);
					}
				}
			}
			return orderQueryPagingResponseVO;
		}
		public string GetSortFld(string sortFld)
		{
			string text = sortFld;
			string key;
			switch (key = text)
			{
			case "OrderNo":
				text = "OR_N";
				break;
			case "TradeNo":
				text = "TR_N";
				break;
			case "Time":
				text = "TIME";
				break;
			case "TransactionsCode":
				text = "CU_I";
				break;
			case "CommodityID":
				text = "PRI";
				break;
			case "B_S":
				text = "TYPE";
				break;
			case "O_L":
				text = "SE_F";
				break;
			case "LPrice":
				text = "OR_P";
				break;
			case "Qty":
				text = "QTY";
				break;
			case "Balance":
				text = "BAL";
				break;
			case "Status":
				text = "STA";
				break;
			case "Liqpl":
				text = "LIQPL";
				break;
			case "Comm":
				text = "COMM";
				break;
			}
			return text;
		}
		private string GetConditionSoftFld(string sortFld)
		{
			string text = sortFld;
			string key;
			switch (key = text)
			{
			case "OrderNo":
				text = "CO_N";
				break;
			case "CommodityID":
				text = "PRI";
				break;
			case "Price":
				text = "CO_P";
				break;
			case "B_S":
				text = "TYPE";
				break;
			case "O_L":
				text = "SE_F";
				break;
			case "ConditionCommodityID":
				text = "CO_PRI";
				break;
			case "Qty":
				text = "QTY";
				break;
			case "ConditionPrice":
				text = "CO_PRICE";
				break;
			case "OrderState":
				text = "STA";
				break;
			case "PrepareTime":
				text = "CO_PT";
				break;
			case "MatureTime":
				text = "CO_MT";
				break;
			case "OrderTime":
				text = "CO_OT";
				break;
			case "CoditionType":
				text = "CO_T";
				break;
			case "ConditionSign":
				text = "CO_S";
				break;
			}
			return text;
		}
		public TradeQueryPagingResponseVO TradeQueryPaging(TradeQueryPagingRequestVO req)
		{
			FileInfo fileInfo = new FileInfo(this.tradebuffer);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
				this.lastPagingTradeId = 0L;
				this.tradeListBuffer.Clear();
			}
			this.tradeListBuffer.Clear();
			TradeQueryPagingResponseVO tradeQueryPagingResponseVO = new TradeQueryPagingResponseVO();
			TradePagingQueryReqVO tradePagingQueryReqVO = new TradePagingQueryReqVO();
			tradePagingQueryReqVO.UserID = req.UserID;
			tradePagingQueryReqVO.MarketID = req.MarketID;
			if (req.CurrentPagNum == 0)
			{
				tradePagingQueryReqVO.LastTradeID = this.lastPagingTradeId;
			}
			tradePagingQueryReqVO.SessionID = SysShareInfo.sessionID;
			tradePagingQueryReqVO.RecordCount = req.RecordCount;
			tradePagingQueryReqVO.SortFld = this.GetSortFld(req.SortFld);
			tradePagingQueryReqVO.IsDesc = req.IsDesc;
			tradePagingQueryReqVO.CurrentPageNum = req.CurrentPagNum;
			if (this.lastPagingTradeId == 0L && tradePagingQueryReqVO.CurrentPageNum == 0)
			{
				tradePagingQueryReqVO.CurrentPageNum = 1;
			}
			tradePagingQueryReqVO.Pri = req.Pri;
			tradePagingQueryReqVO.Type = req.Type;
			tradePagingQueryReqVO.Se_t = req.Se_f;
			TradePagingQueryRepVO tradePagingQueryRepVO = (TradePagingQueryRepVO)this.com.commuteBYVO(tradePagingQueryReqVO, false);
			if (tradePagingQueryRepVO != null)
			{
				tradeQueryPagingResponseVO.RetCode = tradePagingQueryRepVO.Result.RetCode;
				tradeQueryPagingResponseVO.RetMessage = tradePagingQueryRepVO.Result.RetMessage;
				tradeQueryPagingResponseVO.TotalRecord = tradePagingQueryRepVO.Result.TotalRecord;
				tradeQueryPagingResponseVO.TradeTotalRow.TotalNum = tradePagingQueryRepVO.Result.TotalRow.TotalNum;
				tradeQueryPagingResponseVO.TradeTotalRow.TotalQty = tradePagingQueryRepVO.Result.TotalRow.TotalQty;
				tradeQueryPagingResponseVO.TradeTotalRow.TotalLiqpl = tradePagingQueryRepVO.Result.TotalRow.TotalLiqpl;
				tradeQueryPagingResponseVO.TradeTotalRow.TotalComm = tradePagingQueryRepVO.Result.TotalRow.TotalComm;
				if (tradePagingQueryRepVO.ResultList.TradeInfoList != null && tradePagingQueryRepVO.ResultList.TradeInfoList.Count > 0)
				{
					this.tradeListBuffer.AddRange(tradePagingQueryRepVO.ResultList.TradeInfoList);
				}
				if (this.tradeListBuffer != null && this.tradeListBuffer.Count > 0)
				{
					if (tradeQueryPagingResponseVO.TradeInfoList == null)
					{
						tradeQueryPagingResponseVO.TradeInfoList = new List<TradeInfo>();
					}
					for (int i = 0; i < this.tradeListBuffer.Count; i++)
					{
						M_TradeInfo m_TradeInfo = this.tradeListBuffer[i];
						if (this.lastPagingTradeId < m_TradeInfo.TradeNO)
						{
							this.lastPagingTradeId = m_TradeInfo.TradeNO;
						}
						if (req.MarketID == null || req.MarketID.Length <= 0 || this.GetMarketAndeComm(m_TradeInfo.CommodityID).marketID.Equals(req.MarketID))
						{
							TradeInfo tradeInfo = new TradeInfo();
							tradeInfo.TradeNO = m_TradeInfo.TradeNO;
							tradeInfo.OrderNO = m_TradeInfo.OrderNO;
							tradeInfo.TradeTime = m_TradeInfo.TradeTime;
							tradeInfo.BuySell = m_TradeInfo.BuySell;
							tradeInfo.SettleBasis = m_TradeInfo.SettleBasis;
							tradeInfo.TraderID = m_TradeInfo.TraderID;
							tradeInfo.FirmID = m_TradeInfo.FirmID;
							tradeInfo.CustomerID = m_TradeInfo.CustomerID;
							MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeInfo.CommodityID);
							tradeInfo.MarketID = marketAndeComm.marketID;
							tradeInfo.CommodityID = marketAndeComm.commodityID;
							tradeInfo.TradePrice = m_TradeInfo.TradePrice;
							tradeInfo.TradeQuantity = m_TradeInfo.TradeQuantity;
							tradeInfo.TransferPrice = m_TradeInfo.TransferPrice;
							tradeInfo.TransferPL = m_TradeInfo.TransferPL;
							tradeInfo.Comm = m_TradeInfo.Comm;
							tradeInfo.STradeNO = m_TradeInfo.STradeNO;
							tradeInfo.ATradeNO = m_TradeInfo.ATradeNO;
							tradeInfo.TradeType = m_TradeInfo.TradeType;
							tradeQueryPagingResponseVO.TradeInfoList.Add(tradeInfo);
						}
					}
				}
			}
			return tradeQueryPagingResponseVO;
		}
		public VerifyVersionResponseVO VerifyVersion(VerifyVersionRequestVO req)
		{
			VerifyVersionResponseVO verifyVersionResponseVO = new VerifyVersionResponseVO();
			VerifyVersionReqVO verifyVersionReqVO = new VerifyVersionReqVO();
			verifyVersionReqVO.UserID = req.UserID;
			verifyVersionReqVO.SessionID = SysShareInfo.sessionID;
			verifyVersionReqVO.ModuleID = SysShareInfo.moduleID;
			verifyVersionReqVO.ClientVersion = req.ClientVersion;
			VerifyVersionRepVO verifyVersionRepVO = (VerifyVersionRepVO)this.com.commuteBYVO(verifyVersionReqVO, false);
			if (verifyVersionRepVO != null)
			{
				verifyVersionResponseVO.RetCode = verifyVersionRepVO.Result.RetCode;
				verifyVersionResponseVO.RetMessage = verifyVersionRepVO.Result.RetMessage;
				verifyVersionResponseVO.ModuleID = verifyVersionRepVO.Result.ModuleID;
				verifyVersionResponseVO.ServerVersion = verifyVersionRepVO.Result.ServerVersion;
			}
			return verifyVersionResponseVO;
		}
		public ConditionOrderResponseVO ConditionOrder(ConditionOrderRequestVO req)
		{
			ConditionOrderResponseVO conditionOrderResponseVO = new ConditionOrderResponseVO();
			ConditionOrderReqVO conditionOrderReqVO = new ConditionOrderReqVO();
			conditionOrderReqVO.UserID = req.UserID;
			conditionOrderReqVO.SessionID = SysShareInfo.sessionID;
			conditionOrderReqVO.BillType = req.BillType;
			conditionOrderReqVO.Buy_Sell = req.BuySell;
			conditionOrderReqVO.CloseMode = req.CloseMode;
			conditionOrderReqVO.ConcommodityID = req.Concommodity;
			conditionOrderReqVO.ConPrice = req.ConPrice;
			conditionOrderReqVO.ConexPire = req.ConExpire;
			conditionOrderReqVO.ConOperator = req.Conoperator;
			conditionOrderReqVO.CustomerID = req.CustomerID;
			conditionOrderReqVO.LPrice = req.LPrice;
			conditionOrderReqVO.ConmmodityID = this.GetCommodityID("", req.CommodityID);
			conditionOrderReqVO.Price = req.Price;
			conditionOrderReqVO.ConType = req.Contype;
			conditionOrderReqVO.Quantity = req.Quantity;
			conditionOrderReqVO.SettleBasis = req.SettleBasis;
			conditionOrderReqVO.So = req.SO;
			conditionOrderReqVO.TimeFlag = req.TimeFlag;
			conditionOrderReqVO.FirmID = req.FirmID;
			conditionOrderReqVO.TraderID = req.TraderID;
			ConditionOrderRepVO conditionOrderRepVO = (ConditionOrderRepVO)this.com.commuteBYVO(conditionOrderReqVO, false);
			if (conditionOrderRepVO != null)
			{
				conditionOrderResponseVO.RetCode = conditionOrderRepVO.Result.RetCode;
				conditionOrderResponseVO.RetMessage = conditionOrderRepVO.Result.RetMessage;
			}
			return conditionOrderResponseVO;
		}
		public ConditionQueryResponseVO ConditionQuery(ConditionQueryRequestVO req)
		{
			ConditionQueryResponseVO conditionQueryResponseVO = new ConditionQueryResponseVO();
			ConditionQueryReqVO conditionQueryReqVO = new ConditionQueryReqVO();
			conditionQueryReqVO.BuySell = req.BuySell;
			conditionQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
			conditionQueryReqVO.ConditionType = req.ConditionType;
			conditionQueryReqVO.CustomerID = req.CustomerID;
			conditionQueryReqVO.isDesc = req.ISDesc;
			conditionQueryReqVO.MarketID = req.MarketID;
			conditionQueryReqVO.OrderNO = req.OrderNO;
			conditionQueryReqVO.OrderStatus = req.OrderStatus;
			conditionQueryReqVO.RecordCount = req.RecordCount;
			conditionQueryReqVO.SessionID = SysShareInfo.sessionID;
			conditionQueryReqVO.SettleBasis = req.SettleBasis;
			conditionQueryReqVO.SortField = this.GetConditionSoftFld(req.SortField);
			conditionQueryReqVO.StartNum = req.StartNum;
			if (req.PageNumber == 0)
			{
				if (this.weekConditionOrderUpdateTime != 0L)
				{
					conditionQueryReqVO.UpdateTime = this.weekConditionOrderUpdateTime;
				}
				else
				{
					conditionQueryReqVO.UpdateTime = req.UpdateTime;
				}
			}
			else
			{
				conditionQueryReqVO.UpdateTime = 1L;
			}
			if (req.PageNumber == 0)
			{
				conditionQueryReqVO.PageNumber = 1;
			}
			else
			{
				conditionQueryReqVO.PageNumber = req.PageNumber;
			}
			conditionQueryReqVO.Userid = req.UserID;
			ConditionQueryRepVO conditionQueryRepVO = (ConditionQueryRepVO)this.com.commuteBYVO(conditionQueryReqVO, false);
			if (conditionQueryRepVO != null)
			{
				conditionQueryResponseVO.RetCode = conditionQueryRepVO.Result.RetCode;
				conditionQueryResponseVO.RetMessage = conditionQueryRepVO.Result.RetMessage;
				conditionQueryResponseVO.UpdateTime = conditionQueryRepVO.Result.UpdateTime;
				conditionQueryResponseVO.TotalInfo.TotalNum = conditionQueryRepVO.Result.TotalRow.TotalNum;
				conditionQueryResponseVO.TotalInfo.TotalQty = conditionQueryRepVO.Result.TotalRow.TotalQty;
				this.weekConditionOrderUpdateTime = conditionQueryRepVO.Result.UpdateTime;
				List<Condition_Info> condition_InfoList = conditionQueryRepVO.ResultList.Condition_InfoList;
				if (conditionQueryResponseVO.ConditionQueryInfoList == null)
				{
					conditionQueryResponseVO.ConditionQueryInfoList = new List<ConditionQueryOrderInfo>();
				}
				if (condition_InfoList != null && condition_InfoList.Count > 0)
				{
					for (int i = 0; i < condition_InfoList.Count; i++)
					{
						Condition_Info condition_Info = condition_InfoList[i];
						ConditionQueryOrderInfo conditionQueryOrderInfo = new ConditionQueryOrderInfo();
						conditionQueryOrderInfo.BillTradeType = condition_Info.BillTradeType;
						conditionQueryOrderInfo.BuySell_Type = condition_Info.BuySell;
						conditionQueryOrderInfo.CFlag = condition_Info.CFlag;
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(condition_Info.CommodityID);
						conditionQueryOrderInfo.CommodityID = marketAndeComm.commodityID;
						conditionQueryOrderInfo.Condition_CommodityID = condition_Info.Codition_CommodityID;
						conditionQueryOrderInfo.ConditionOperator = condition_Info.ConOperator;
						conditionQueryOrderInfo.ConditionPrice = condition_Info.ConditionPrice;
						conditionQueryOrderInfo.ConditionType = condition_Info.ConditionType;
						conditionQueryOrderInfo.CustomerID = condition_Info.CustomerID;
						conditionQueryOrderInfo.FirmID = condition_Info.FirmID;
						conditionQueryOrderInfo.LPrice = condition_Info.LPrice;
						conditionQueryOrderInfo.MatureTime = condition_Info.MatureTime;
						conditionQueryOrderInfo.OrderNO = condition_Info.OrderNO;
						conditionQueryOrderInfo.OrderPrice = condition_Info.OrderPrice;
						conditionQueryOrderInfo.OrderQuantity = condition_Info.OrderQuantity;
						conditionQueryOrderInfo.OrderStare = condition_Info.OrderState;
						conditionQueryOrderInfo.OrderTime = condition_Info.OrderTime;
						conditionQueryOrderInfo.PrePareTime = condition_Info.PrePareTime;
						conditionQueryOrderInfo.RevokeTime = condition_Info.RevokeTime;
						conditionQueryOrderInfo.SessionID = condition_Info.SessionID;
						conditionQueryOrderInfo.SettleBasis = condition_Info.SettleBasis;
						conditionQueryOrderInfo.Surplus = condition_Info.Surplus;
						conditionQueryOrderInfo.TraderID = condition_Info.TraderID;
						conditionQueryResponseVO.ConditionQueryInfoList.Add(conditionQueryOrderInfo);
					}
				}
			}
			return conditionQueryResponseVO;
		}
		public ConditionRevokeResponseVO ConditionRevoke(ConditionRevokeRequestVO req)
		{
			ConditionRevokeResponseVO conditionRevokeResponseVO = new ConditionRevokeResponseVO();
			ConditionRevokeReqVO conditionRevokeReqVO = new ConditionRevokeReqVO();
			conditionRevokeReqVO.ConditionOrderNo = req.ConditionOrderNo;
			conditionRevokeReqVO.CustomerID = req.CustomerID;
			conditionRevokeReqVO.SessionID = SysShareInfo.sessionID;
			conditionRevokeReqVO.UserID = req.UserID;
			ConditionRevokeRepVO conditionRevokeRepVO = (ConditionRevokeRepVO)this.com.commuteBYVO(conditionRevokeReqVO, false);
			if (conditionRevokeRepVO != null)
			{
				conditionRevokeResponseVO.RetCode = conditionRevokeRepVO.Result.RetCode;
				conditionRevokeResponseVO.RetMessage = conditionRevokeRepVO.Result.RetMessage;
			}
			return conditionRevokeResponseVO;
		}
		private string GetCommodityID(string marketID, string commodityID)
		{
			string result = string.Empty;
			if (marketID == null || marketID.Length != 2)
			{
				marketID = "99";
			}
			if (commodityID != null && commodityID.Length > 0)
			{
				result = marketID + commodityID;
			}
			return result;
		}
		private MarketAndeComm GetMarketAndeComm(string commodityID)
		{
			MarketAndeComm marketAndeComm = new MarketAndeComm();
			if (commodityID != null && commodityID.Length > 2)
			{
				marketAndeComm.marketID = commodityID.Substring(0, 2);
				marketAndeComm.commodityID = commodityID.Substring(2);
			}
			return marketAndeComm;
		}
		public ResponseVO ChgMappingPwd(ChgMappingPwdRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			ChgMappingPwdReqVO chgMappingPwdReqVO = new ChgMappingPwdReqVO();
			chgMappingPwdReqVO.UserID = req.UserID;
			chgMappingPwdReqVO.ModuleID = req.ModuleID;
			chgMappingPwdReqVO.OldPassword = req.OldPassword;
			chgMappingPwdReqVO.NewPassword = req.NewPassword;
			chgMappingPwdReqVO.SessionID = SysShareInfo.sessionID;
			ChgMappingPwdRepVO chgMappingPwdRepVO = (ChgMappingPwdRepVO)this.com.commuteBYVO(chgMappingPwdReqVO, false);
			if (chgMappingPwdRepVO != null)
			{
				responseVO.RetCode = chgMappingPwdRepVO.Result.RetCode;
				responseVO.RetMessage = chgMappingPwdRepVO.Result.RetMessage;
			}
			return responseVO;
		}
		public GetMappingUserResponseVO GetMappingUser(GetMappingUserRequestVO req)
		{
			GetMappingUserResponseVO getMappingUserResponseVO = new GetMappingUserResponseVO();
			GetMappingUserReqVO getMappingUserReqVO = new GetMappingUserReqVO();
			getMappingUserReqVO.UserID = req.UserID;
			getMappingUserReqVO.ModuleID = req.ModuleID;
			getMappingUserReqVO.Password = req.Password;
			GetMappingUserRepVO getMappingUserRepVO = (GetMappingUserRepVO)this.com.commuteBYVO(getMappingUserReqVO, false);
			if (getMappingUserRepVO != null)
			{
				getMappingUserResponseVO.RetCode = getMappingUserRepVO.Result.RetCode;
				getMappingUserResponseVO.RetMessage = getMappingUserRepVO.Result.RetMessage;
				getMappingUserResponseVO.MappingUser = getMappingUserRepVO.Result.MUser;
				List<MappingUserInfo> mappingUser_InfoList = getMappingUserRepVO.ResultList.MappingUser_InfoList;
				if (getMappingUserResponseVO.MappingUser_InfoList == null)
				{
					getMappingUserResponseVO.MappingUser_InfoList = new List<MappingUser_Info>();
				}
				if (mappingUser_InfoList != null && mappingUser_InfoList.Count > 0)
				{
					for (int i = 0; i < mappingUser_InfoList.Count; i++)
					{
						MappingUserInfo mappingUserInfo = mappingUser_InfoList[i];
						MappingUser_Info mappingUser_Info = new MappingUser_Info();
						mappingUser_Info.ModuleID = mappingUserInfo.MdID;
						mappingUser_Info.MappingUserID = mappingUserInfo.MpUser;
						getMappingUserResponseVO.MappingUser_InfoList.Add(mappingUser_Info);
					}
				}
			}
			return getMappingUserResponseVO;
		}
		public ResponseVO MixUser(MixUserRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			MixUserReqVO mixUserReqVO = new MixUserReqVO();
			mixUserReqVO.UserID = req.UserID;
			mixUserReqVO.ModuleID = req.ModuleID;
			mixUserReqVO.MappingType = (long)req.MappingType;
			mixUserReqVO.MmUser = req.MMappingUserID;
			mixUserReqVO.SessionID = SysShareInfo.sessionID;
			mixUserReqVO.RequestList = new AccountInfoList();
			mixUserReqVO.RequestList.AccountList = new List<Account_Info>();
			foreach (AccountInfo current in req.AccountInfoList)
			{
				Account_Info account_Info = new Account_Info();
				account_Info.MwUser = current.MWUserID;
				account_Info.Password = current.Password;
				mixUserReqVO.RequestList.AccountList.Add(account_Info);
			}
			MixUserRepVO mixUserRepVO = (MixUserRepVO)this.com.commuteBYVO(mixUserReqVO, false);
			if (mixUserRepVO != null)
			{
				responseVO.RetCode = mixUserRepVO.Result.RetCode;
				responseVO.RetMessage = mixUserRepVO.Result.RetMessage;
			}
			return responseVO;
		}
		public ResponseVO CheckMappingUser(CheckMappingUserRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			CheckMappingUserReqVO checkMappingUserReqVO = new CheckMappingUserReqVO();
			checkMappingUserReqVO.UserID = req.UserID;
			checkMappingUserReqVO.ModuleID = req.ModuleID;
			checkMappingUserReqVO.Password = req.Password;
			CheckMappingUserRepVO checkMappingUserRepVO = (CheckMappingUserRepVO)this.com.commuteBYVO(checkMappingUserReqVO, false);
			if (checkMappingUserRepVO != null)
			{
				responseVO.RetCode = checkMappingUserRepVO.Result.RetCode;
				responseVO.RetMessage = checkMappingUserRepVO.Result.RetMessage;
			}
			return responseVO;
		}
	}
}
