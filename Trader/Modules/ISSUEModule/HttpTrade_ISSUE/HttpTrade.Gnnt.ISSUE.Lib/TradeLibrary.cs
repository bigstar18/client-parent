using HttpTrade.Gnnt.ISSUE.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeInterface.Gnnt.ISSUE.DataVO;
using TradeInterface.Gnnt.ISSUE.Interface;
namespace HttpTrade.Gnnt.ISSUE.Lib
{
	public class TradeLibrary : ITradeLibrary
	{
		private const int maxTradeCount = 500;
		private const int maxOrderCount = 500;
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
		private bool isFirstCheckUser = true;
		private long lastTradeId;
		private List<M_TradeInfo> tradeListBuffer = new List<M_TradeInfo>();
		private bool isWriteFile;
		private string curTradeDay = string.Empty;
		private long lastID;
		private long tradeTotal = -1L;
		private long ltradeTotal;
		private List<M_OrderInfo> orderListBuffer = new List<M_OrderInfo>();
		private long weekOrderUpdateTime;
		private List<WeekOrderInfo> weekOrderListBuffer = new List<WeekOrderInfo>();
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
			this.weekOrderListBuffer.Clear();
			this.tradeListBuffer.Clear();
			this.weekOrderUpdateTime = 0L;
			this.lastTradeId = 0L;
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
		public int passwordScore(string password)
		{
			int num = 0;
			Regex regex = new Regex("[a-z]");
			Regex regex2 = new Regex("[A-Z]");
			Regex regex3 = new Regex("\\d+");
			Regex regex4 = new Regex("(\\d\\D*\\d\\D*\\d)");
			Regex regex5 = new Regex("[!,@#$%^&*?_~]");
			Regex regex6 = new Regex("([!,@#$%^&*?_~].*[!,@#$%^&*?_~])");
			Regex regex7 = new Regex("\\d");
			Regex regex8 = new Regex("\\D");
			if (regex.IsMatch(password))
			{
				num++;
			}
			if (regex2.IsMatch(password))
			{
				num += 5;
			}
			if (regex3.IsMatch(password))
			{
				num++;
			}
			if (regex4.IsMatch(password))
			{
				num += 5;
			}
			if (regex5.IsMatch(password))
			{
				num += 5;
			}
			if (regex6.IsMatch(password))
			{
				num += 5;
			}
			if (regex.IsMatch(password) && regex2.IsMatch(password))
			{
				num += 2;
			}
			if (regex7.IsMatch(password) && regex8.IsMatch(password))
			{
				num += 2;
			}
			if (regex.IsMatch(password) && regex2.IsMatch(password) && regex7.IsMatch(password) && regex5.IsMatch(password))
			{
				num += 2;
			}
			return num;
		}
		public LogonResponseVO Logon(LogonRequestVO req)
		{
			LogonResponseVO logonResponseVO = new LogonResponseVO();
			LogonReqVO logonReqVO = new LogonReqVO();
			logonReqVO.UserID = req.UserID;
			logonReqVO.Password = req.Password;
			logonReqVO.RegisterWord = req.RegisterWord;
			logonReqVO.VersionInfo = req.VersionInfo;
			logonReqVO.LogonType = req.LogonType;
			if (logonReqVO.RegisterWord == null || logonReqVO.RegisterWord.Length == 0)
			{
				logonReqVO.RegisterWord = this.readKey(logonReqVO.UserID);
			}
			LogonRepVO logonRepVO = (LogonRepVO)this.com.commuteBYVO(logonReqVO);
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
					if (logonResponseVO.RandomKey != null && logonResponseVO.RandomKey.Length > 0)
					{
						this.recordKey(logonReqVO.UserID, logonResponseVO.RandomKey);
					}
					if (logonReqVO.Password.Length < 8 && this.passwordScore(logonReqVO.Password) <= 1)
					{
						MessageBox.Show("您的交易密码过于简单，为了您的交易安全，请及时修改！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			ResponseVO responseVO = new ResponseVO();
			LogoffReqVO logoffReqVO = new LogoffReqVO();
			logoffReqVO.UserID = userID;
			logoffReqVO.SessionID = SysShareInfo.sessionID;
			LogoffRepVO logoffRepVO = (LogoffRepVO)this.com.commuteBYVO(logoffReqVO);
			if (logoffRepVO != null)
			{
				responseVO.RetCode = logoffRepVO.Result.RetCode;
				responseVO.RetMessage = logoffRepVO.Result.RetMessage;
			}
			return responseVO;
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
			CheckUserRepVO checkUserRepVO = (CheckUserRepVO)this.com.commuteBYVO(checkUserReqVO);
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
			SysTimeQueryResponseVO sysTime = this.GetSysTime(new SysTimeQueryRequestVO
			{
				UserID = userID
			});
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
			ChgPwdRepVO chgPwdRepVO = (ChgPwdRepVO)this.com.commuteBYVO(chgPwdReqVO);
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
			FirmInfoRepVO firmInfoRepVO = (FirmInfoRepVO)this.com.commuteBYVO(firmInfoReqVO);
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
					firmInfoResponseVO.FirmType = firmInfo.FirmTpye;
					firmInfoResponseVO.InitFund = firmInfo.InitFund;
					firmInfoResponseVO.InFund = firmInfo.InFund;
					firmInfoResponseVO.OutFund = firmInfo.OutFund;
					firmInfoResponseVO.HKSell = firmInfo.HKSell;
					firmInfoResponseVO.HKBuy = firmInfo.HKBuy;
					firmInfoResponseVO.CurFreezeFund = firmInfo.CurFreezeFund;
					firmInfoResponseVO.CurUnfreezeFund = firmInfo.CurUnfreezeFund;
					firmInfoResponseVO.IssuanceFee = firmInfo.IssuanceFee;
					firmInfoResponseVO.SGFreezeFund = firmInfo.SGFreezeFund;
					firmInfoResponseVO.OrderFrozenFund = firmInfo.OrderFrozenFund;
					firmInfoResponseVO.OtherFrozenFund = firmInfo.OtherFrozenFund;
					firmInfoResponseVO.Fee = firmInfo.Fee;
					firmInfoResponseVO.WareHouseRegFee = firmInfo.WareHouseRegFee;
					firmInfoResponseVO.WareHouseCancelFee = firmInfo.WareHouseCancelFee;
					firmInfoResponseVO.TransferFee = firmInfo.TransferFee;
					firmInfoResponseVO.DistributionFee = firmInfo.DistributionFee;
					firmInfoResponseVO.OtherFee = firmInfo.OtherFee;
					firmInfoResponseVO.OtherChange = firmInfo.OtherChange;
					firmInfoResponseVO.MarketValue = firmInfo.MarketValue;
					firmInfoResponseVO.UsableFund = firmInfo.UsableFund;
					firmInfoResponseVO.DesirableFund = firmInfo.DesirableFund;
					firmInfoResponseVO.CurrentRight = firmInfo.CurrentRight;
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
			OrderRepVO orderRepVO = (OrderRepVO)this.com.commuteBYVO(orderReqVO);
			if (orderRepVO != null)
			{
				responseVO.RetCode = orderRepVO.Result.RetCode;
				responseVO.RetMessage = orderRepVO.Result.RetMessage;
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
			WithDrawOrderRepVO withDrawOrderRepVO = (WithDrawOrderRepVO)this.com.commuteBYVO(withDrawOrderReqVO);
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
			TradeQueryRepVO tradeQueryRepVO = (TradeQueryRepVO)this.com.commuteBYVO(tradeQueryReqVO);
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
				if (this.tradeListBuffer != null && this.tradeListBuffer.Count > 0)
				{
					if (tradeQueryResponseVO.TradeInfoList == null)
					{
						tradeQueryResponseVO.TradeInfoList = new List<TradeInfo>();
					}
					for (int i = 0; i < this.tradeListBuffer.Count; i++)
					{
						M_TradeInfo m_TradeInfo = this.tradeListBuffer[i];
						if (i == this.tradeListBuffer.Count - 1)
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
		public TradeSumQueryResponseVO TradeSumQuery(TradeSumQueryRequestVO req)
		{
			TradeSumQueryResponseVO tradeSumQueryResponseVO = new TradeSumQueryResponseVO();
			TradeSumQueryReqVO tradeSumQueryReqVO = new TradeSumQueryReqVO();
			tradeSumQueryReqVO.UserID = req.UserID;
			tradeSumQueryReqVO.SessionID = SysShareInfo.sessionID;
			TradeSumQueryRepVO tradeSumQueryRepVO = (TradeSumQueryRepVO)this.com.commuteBYVO(tradeSumQueryReqVO);
			if (tradeSumQueryRepVO != null)
			{
				tradeSumQueryResponseVO.RetCode = tradeSumQueryRepVO.Result.RetCode;
				tradeSumQueryResponseVO.RetMessage = tradeSumQueryRepVO.Result.RetMessage;
				if (tradeSumQueryRepVO.ResultList.TradeSumInfoList != null && tradeSumQueryRepVO.ResultList.TradeSumInfoList.Count > 0)
				{
					if (tradeSumQueryResponseVO.TradeSumInfoList == null)
					{
						tradeSumQueryResponseVO.TradeSumInfoList = new List<TradeSumInfo>();
					}
					for (int i = 0; i < tradeSumQueryRepVO.ResultList.TradeSumInfoList.Count; i++)
					{
						M_TradeSumInfo m_TradeSumInfo = tradeSumQueryRepVO.ResultList.TradeSumInfoList[i];
						TradeSumInfo tradeSumInfo = new TradeSumInfo();
						MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeSumInfo.CommodityID);
						tradeSumInfo.CommodityID = marketAndeComm.commodityID;
						tradeSumInfo.BuyQty = m_TradeSumInfo.BuyQty;
						tradeSumInfo.BuyComm = m_TradeSumInfo.BuyComm;
						tradeSumInfo.SellQty = m_TradeSumInfo.SellQty;
						tradeSumInfo.SellComm = m_TradeSumInfo.SellComm;
						tradeSumQueryResponseVO.TradeSumInfoList.Add(tradeSumInfo);
					}
				}
			}
			return tradeSumQueryResponseVO;
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
			SysTimeQueryRepVO sysTimeQueryRepVO = (SysTimeQueryRepVO)this.com.commuteBYVO(sysTimeQueryReqVO);
			if (sysTimeQueryRepVO != null)
			{
				sysTimeQueryResponseVO.RetCode = sysTimeQueryRepVO.Result.RetCode;
				sysTimeQueryResponseVO.RetMessage = sysTimeQueryRepVO.Result.RetMessage;
				sysTimeQueryResponseVO.CurrentTime = sysTimeQueryRepVO.Result.CurrentTime;
				sysTimeQueryResponseVO.CurrentDate = sysTimeQueryRepVO.Result.CurrentDate;
				sysTimeQueryResponseVO.MicroSecond = sysTimeQueryRepVO.Result.MicroSecond;
				if (sysTimeQueryRepVO.Result.LastID != -1L)
				{
					this.lastID = sysTimeQueryRepVO.Result.LastID;
				}
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
			WeekOrderQueryRepVO weekOrderQueryRepVO = (WeekOrderQueryRepVO)this.com.commuteBYVO(weekOrderQueryReqVO);
			if (weekOrderQueryRepVO != null)
			{
				orderQueryResponseVO.RetCode = weekOrderQueryRepVO.Result.RetCode;
				orderQueryResponseVO.RetMessage = weekOrderQueryRepVO.Result.RetMessage;
				orderQueryResponseVO.TotalRecord = weekOrderQueryRepVO.Result.TotalRecord;
				this.weekOrderUpdateTime = weekOrderQueryRepVO.Result.UpdateTime;
				List<WeekOrderInfo> weekOrderList = weekOrderQueryRepVO.ResultList.WeekOrderList;
				int count = this.weekOrderListBuffer.Count;
				if (weekOrderList != null)
				{
					for (int i = 0; i < weekOrderList.Count; i++)
					{
						bool flag = false;
						for (int j = 0; j < count; j++)
						{
							if (weekOrderList[i].OrderNO == this.weekOrderListBuffer[j].OrderNO)
							{
								this.weekOrderListBuffer[j] = weekOrderList[i];
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.weekOrderListBuffer.Add(weekOrderList[i]);
						}
					}
				}
				if (this.weekOrderListBuffer != null && this.weekOrderListBuffer.Count > 0)
				{
					if (orderQueryResponseVO.OrderInfoList == null)
					{
						orderQueryResponseVO.OrderInfoList = new List<OrderInfo>();
					}
					for (int k = 0; k < this.weekOrderListBuffer.Count; k++)
					{
						WeekOrderInfo weekOrderInfo = this.weekOrderListBuffer[k];
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
			HoldingDetailRepVO holdingDetailRepVO = (HoldingDetailRepVO)this.com.commuteBYVO(holdingDetailReqVO);
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
			HoldingQueryRepVO holdingQueryRepVO = (HoldingQueryRepVO)this.com.commuteBYVO(holdingQueryReqVO);
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
						holdingInfo.LPRadio = m_HoldingInfo.LPRadio;
						holdingInfo.MarketValue = m_HoldingInfo.MarketValue;
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
			CommDataQueryRepVO commDataQueryRepVO = (CommDataQueryRepVO)this.com.commuteBYVO(commDataQueryReqVO);
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
						commData.BPrice1 = m_CommData.BPrice1;
						commData.BPrice2 = m_CommData.BPrice2;
						commData.BPrice3 = m_CommData.BPrice3;
						commData.SPrice1 = m_CommData.SPrice1;
						commData.SPrice2 = m_CommData.SPrice2;
						commData.SPrice3 = m_CommData.SPrice3;
						commData.BValue1 = m_CommData.BValue1;
						commData.BValue2 = m_CommData.BValue2;
						commData.BValue3 = m_CommData.BValue3;
						commData.SValue1 = m_CommData.SValue1;
						commData.SValue2 = m_CommData.SValue2;
						commData.SValue3 = m_CommData.SValue3;
						commData.Count = m_CommData.Count;
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
			CommodityQueryRepVO commodityQueryRepVO = (CommodityQueryRepVO)this.com.commuteBYVO(commodityQueryReqVO);
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
						commodityInfo.OnMarket = m_CommodityInfo.OnMarket;
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
			MarketQueryRepVO marketQueryRepVO = (MarketQueryRepVO)this.com.commuteBYVO(marketQueryReqVO);
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
			FirmBreedRepVO firmBreedRepVO = (FirmBreedRepVO)this.com.commuteBYVO(firmBreedReqVO);
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
		public InvestorInfoResponseVO GetInvestorInfo(string userID)
		{
			InvestorInfoResponseVO investorInfoResponseVO = new InvestorInfoResponseVO();
			InvestorInfoReqVO investorInfoReqVO = new InvestorInfoReqVO();
			investorInfoReqVO.UserID = userID;
			investorInfoReqVO.SessionID = SysShareInfo.sessionID;
			InvestorInfoRepVO investorInfoRepVO = (InvestorInfoRepVO)this.com.commuteBYVO(investorInfoReqVO);
			if (investorInfoRepVO != null)
			{
				investorInfoResponseVO.RetCode = investorInfoRepVO.Result.RetCode;
				investorInfoResponseVO.RetMessage = investorInfoRepVO.Result.RetMessage;
				investorInfoResponseVO.Account = investorInfoRepVO.Result.Account;
				investorInfoResponseVO.Bank = investorInfoRepVO.Result.Bank;
				investorInfoResponseVO.IDNum = investorInfoRepVO.Result.IDNum;
				investorInfoResponseVO.Name = investorInfoRepVO.Result.Name;
				investorInfoResponseVO.Phone = investorInfoRepVO.Result.Phone;
			}
			return investorInfoResponseVO;
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
	}
}
