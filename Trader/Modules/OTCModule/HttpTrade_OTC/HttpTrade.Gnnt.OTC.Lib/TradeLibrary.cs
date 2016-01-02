using HttpTrade.Gnnt.OTC.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
using TradeInterface.Gnnt.OTC.Interface;
namespace HttpTrade.Gnnt.OTC.Lib
{
	public class TradeLibrary : ITradeLibrary
	{
		private const int maxTradeCount = 500;
		private const int maxOrderCount = 500;
		private int protocolID = 1;
		public static bool isWriteLog;
		private string myIpAddress = string.Empty;
		private int myPort;
		private int mySafePort;
		private string myCommunicationUrl = string.Empty;
		private string mySafeCommunicationUrl = string.Empty;
		private string buffPath = string.Empty;
		private static volatile TradeLibrary InstanceTradeLibrary;
		private HTTPCommunicate com;
		private string tradebuffer = "trade.buf";
		private string orderbuffer = "order.buf";
		private string ordertimebuffer = "ordertime.buf";
		private bool isFirstCheckUser = true;
		private long lastTradeId;
		private long agencyLastTradeId;
		private List<M_TradeInfo> tradeListBuffer = new List<M_TradeInfo>();
		private bool isWriteFile;
		private string curTradeDay = string.Empty;
		private long lastID;
		private long tradeTotal = -1L;
		private long ltradeTotal;
		private List<M_OrderInfo> orderListBuffer = new List<M_OrderInfo>();
		private long weekOrderUpdateTime;
		private long agencyWeekOrderUpdateTime;
		private List<M_OrderInfo> weekOrderListBuffer = new List<M_OrderInfo>();
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
		public int SafePort
		{
			get
			{
				return this.mySafePort;
			}
			set
			{
				this.mySafePort = value;
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
		public string SafeCommunicationUrl
		{
			get
			{
				return this.mySafeCommunicationUrl;
			}
			set
			{
				this.mySafeCommunicationUrl = value;
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
		public static TradeLibrary GetInstance()
		{
			if (TradeLibrary.InstanceTradeLibrary == null)
			{
				lock (typeof(TradeLibrary))
				{
					if (TradeLibrary.InstanceTradeLibrary == null)
					{
						try
						{
							TradeLibrary.InstanceTradeLibrary = new TradeLibrary();
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
				}
			}
			return TradeLibrary.InstanceTradeLibrary;
		}
		public void Initialize()
		{
			try
			{
				this.com = new HTTPCommunicate(this.myCommunicationUrl, this.SafeCommunicationUrl);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
		public void Dispose()
		{
			try
			{
				this.orderListBuffer.Clear();
				this.weekOrderListBuffer.Clear();
				this.tradeListBuffer.Clear();
				this.weekOrderUpdateTime = 0L;
				this.lastTradeId = 0L;
				this.tradeTotal = -1L;
				this.isWriteFile = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
		private void recordKey(string username, string key)
		{
			FileStream fileStream = null;
			BinaryWriter binaryWriter = null;
			try
			{
				fileStream = new FileStream(username + ".key", FileMode.Create);
				binaryWriter = new BinaryWriter(fileStream, Encoding.BigEndianUnicode);
				binaryWriter.Write(key);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				if (binaryWriter != null)
				{
					binaryWriter.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
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
			if (logonReqVO.RegisterWord == null || logonReqVO.RegisterWord.Length == 0)
			{
				logonReqVO.RegisterWord = this.readKey(logonReqVO.UserID);
			}
			try
			{
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
						logonResponseVO.Identity = logonRepVO.Result.Identity;
						logonResponseVO.Name = logonRepVO.Result.Name;
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
						Logger.wirte(3, string.Format("retCodeNo [{0}]:{1} ", logonRepVO.Result.RetCode.ToString(), logonRepVO.Result.RetMessage.ToString()));
						logonResponseVO.RetCode = -1L;
						logonResponseVO.RetMessage = logonRepVO.Result.RetMessage;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
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
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryReader.Close();
				fileStream.Close();
			}
		}
		public void ReadWeekOrderInfo(List<M_OrderInfo> list)
		{
			FileStream fileStream = new FileStream(this.orderbuffer, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			try
			{
				while (true)
				{
					M_OrderInfo m_OrderInfo = new M_OrderInfo();
					BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
					FieldInfo[] fields = m_OrderInfo.GetType().GetFields(bindingAttr);
					FieldInfo[] array = fields;
					for (int i = 0; i < array.Length; i++)
					{
						FieldInfo fieldInfo = array[i];
						if (fieldInfo.FieldType.Name.Equals("String"))
						{
							string arg_5A_0 = fieldInfo.Name;
							fieldInfo.SetValue(m_OrderInfo, this.decode(binaryReader.ReadString()));
						}
					}
					list.Add(m_OrderInfo);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryReader.Close();
				fileStream.Close();
			}
		}
		public void ReadOrderTimeInfo()
		{
			FileStream fileStream = new FileStream(this.ordertimebuffer, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			try
			{
				this.weekOrderUpdateTime = binaryReader.ReadInt64();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryReader.Close();
				fileStream.Close();
			}
		}
		public void WriteInfo(List<M_TradeInfo> list)
		{
			FileStream fileStream = null;
			BinaryWriter binaryWriter = null;
			string path = this.tradebuffer;
			try
			{
				if (File.Exists(path))
				{
					fileStream = new FileStream(path, FileMode.Append);
				}
				else
				{
					fileStream = new FileStream(path, FileMode.Create);
				}
				binaryWriter = new BinaryWriter(fileStream);
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
						current.CommodityID,
						current.TradePrice.ToString(),
						current.TradeQuantity.ToString(),
						current.TransferPL.ToString(),
						current.Comm.ToString(),
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
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryWriter.Close();
				fileStream.Close();
			}
		}
		public void WriteWeekOrderInfo(List<M_OrderInfo> list, string userID, string agencyNo)
		{
			FileStream fileStream = null;
			BinaryWriter binaryWriter = null;
			try
			{
				fileStream = new FileStream(userID + agencyNo + this.orderbuffer, FileMode.Create);
				binaryWriter = new BinaryWriter(fileStream);
				foreach (M_OrderInfo current in list)
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
						current.CommodityID.ToString(),
						current.OrderPrice.ToString(),
						current.OrderQuantity.ToString(),
						current.WithDrawTime.ToString()
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
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryWriter.Close();
				fileStream.Close();
			}
		}
		public void WriteOrderTimeInfo(long updateTime, string userID, string agencyNo)
		{
			FileStream fileStream = null;
			BinaryWriter binaryWriter = null;
			try
			{
				fileStream = new FileStream(userID + agencyNo + this.ordertimebuffer, FileMode.Create);
				binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(updateTime);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			finally
			{
				binaryWriter.Close();
				fileStream.Close();
			}
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
			WaitCallback callBack = new WaitCallback(this.LogoffVO);
			ThreadPool.QueueUserWorkItem(callBack, userID);
			return result;
		}
		private void LogoffVO(object userID)
		{
			try
			{
				LogoffReqVO logoffReqVO = new LogoffReqVO();
				logoffReqVO.UserID = userID.ToString();
				logoffReqVO.SessionID = SysShareInfo.sessionID;
				LogoffRepVO arg_2E_0 = (LogoffRepVO)this.com.commuteBYVO(logoffReqVO);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
		public ResponseVO CheckUser(CheckUserRequestVO requestVO)
		{
			ResponseVO responseVO = null;
			try
			{
				CheckUserReqVO checkUserReqVO = new CheckUserReqVO();
				checkUserReqVO.UserID = requestVO.UserID;
				checkUserReqVO.SessionID = SysShareInfo.sessionID;
				checkUserReqVO.ModuleID = SysShareInfo.moduleID;
				checkUserReqVO.FromLogonType = SysShareInfo.FromLogonType;
				checkUserReqVO.LogonType = requestVO.LogonType;
				CheckUserRepVO checkUserRepVO = (CheckUserRepVO)this.com.commuteBYVO(checkUserReqVO);
				if (checkUserRepVO != null)
				{
					responseVO = new ResponseVO();
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
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
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
			try
			{
				ChgPwdReqVO chgPwdReqVO = new ChgPwdReqVO();
				chgPwdReqVO.UserID = req.UserID;
				chgPwdReqVO.OldPassword = req.OldPassword;
				chgPwdReqVO.NewPassword = req.NewPassword;
				chgPwdReqVO.SessionID = SysShareInfo.sessionID;
				chgPwdReqVO.PasswordType = req.PasswordType;
				ChgPwdRepVO chgPwdRepVO = (ChgPwdRepVO)this.com.commuteBYVO(chgPwdReqVO);
				if (chgPwdRepVO != null)
				{
					responseVO.RetCode = chgPwdRepVO.Result.RetCode;
					responseVO.RetMessage = chgPwdRepVO.Result.RetMessage;
				}
				else
				{
					responseVO.RetMessage = "操作超时，请重试！";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public FirmInfoResponseVO GetFirmInfo(FirmInfoRequestVO req)
		{
			FirmInfoResponseVO firmInfoResponseVO = new FirmInfoResponseVO();
			FirmInfoReqVO firmInfoReqVO = new FirmInfoReqVO();
			try
			{
				firmInfoReqVO.UserID = req.UserID;
				firmInfoReqVO.SessionID = SysShareInfo.sessionID;
				firmInfoReqVO.AgencyNo = req.AgencyNo;
				firmInfoReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
						firmInfoResponseVO.InitFund = firmInfo.InitFund;
						firmInfoResponseVO.YesterdayBail = firmInfo.YesterdayBail;
						firmInfoResponseVO.YesterdayFL = firmInfo.YesterdayFL;
						firmInfoResponseVO.CurrentBail = firmInfo.CurrentBail;
						firmInfoResponseVO.CurrentFL = firmInfo.CurrentFL;
						firmInfoResponseVO.OrderFrozenFund = firmInfo.OrderFrozenFund;
						firmInfoResponseVO.OrderFrozenMargin = firmInfo.OrderFrozenMargin;
						firmInfoResponseVO.OrderFrozenFee = firmInfo.OrderFrozenFee;
						firmInfoResponseVO.OtherFrozenFund = firmInfo.OtherFrozenFund;
						firmInfoResponseVO.RealFund = firmInfo.RealFund;
						firmInfoResponseVO.Fee = firmInfo.Fee;
						firmInfoResponseVO.TransferPL = firmInfo.TransferPL;
						firmInfoResponseVO.UsingFund = firmInfo.UsingFund;
						firmInfoResponseVO.CurrentRight = firmInfo.CurrentRight;
						firmInfoResponseVO.InOutFund = firmInfo.InOutFund;
						firmInfoResponseVO.HoldingPL = firmInfo.HoldingPL;
						firmInfoResponseVO.OtherChange = firmInfo.OtherChange;
						firmInfoResponseVO.ImpawnFund = firmInfo.ImpawnFund;
						firmInfoResponseVO.ClearDelay = firmInfo.ClearDelay;
						firmInfoResponseVO.CStatus = firmInfo.CStatus;
						firmInfoResponseVO.FundRisk = firmInfo.FundRisk;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return firmInfoResponseVO;
		}
		public ResponseVO Order(OrderRequestVO req)
		{
			ResponseVO result = null;
			try
			{
				switch (req.TradeType)
				{
				case 1:
					result = this.OrderS(req);
					break;
				case 2:
					result = this.OrderX(req);
					break;
				default:
					result = null;
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return result;
		}
		public ResponseVO OrderS(OrderRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			OrderSReqVO orderSReqVO = new OrderSReqVO();
			try
			{
				orderSReqVO.UserID = req.UserID;
				orderSReqVO.BuySell = req.BuySell;
				orderSReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				orderSReqVO.Price = req.Price;
				orderSReqVO.Quantity = req.Quantity;
				orderSReqVO.SettleBasis = req.SettleBasis;
				orderSReqVO.CloseMode = req.CloseMode;
				orderSReqVO.DotDiff = req.DotDiff;
				orderSReqVO.HoldingID = req.HoldingID;
				orderSReqVO.OtherID = req.OtherID;
				orderSReqVO.StopLoss = req.StopLoss;
				orderSReqVO.StopProfit = req.StopProfit;
				orderSReqVO.SessionID = SysShareInfo.sessionID;
				orderSReqVO.AgencyNo = req.AgencyNo;
				orderSReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				OrderSRepVO orderSRepVO = (OrderSRepVO)this.com.commuteBYVO(orderSReqVO);
				if (orderSRepVO != null)
				{
					responseVO.RetCode = orderSRepVO.Result.RetCode;
					responseVO.RetMessage = orderSRepVO.Result.RetMessage;
				}
				else
				{
					responseVO.RetMessage = "操作超时，请重试！";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public ResponseVO OrderX(OrderRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			OrderXReqVO orderXReqVO = new OrderXReqVO();
			try
			{
				orderXReqVO.UserID = req.UserID;
				orderXReqVO.BuySell = req.BuySell;
				orderXReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				orderXReqVO.Price = req.Price;
				orderXReqVO.Quantity = req.Quantity;
				orderXReqVO.ValidityType = req.ValidityType;
				orderXReqVO.OtherID = req.OtherID;
				orderXReqVO.StopLoss = req.StopLoss;
				orderXReqVO.StopProfit = req.StopProfit;
				orderXReqVO.SessionID = SysShareInfo.sessionID;
				orderXReqVO.AgencyNo = req.AgencyNo;
				orderXReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				OrderXRepVO orderXRepVO = (OrderXRepVO)this.com.commuteBYVO(orderXReqVO);
				if (orderXRepVO != null)
				{
					responseVO.RetCode = orderXRepVO.Result.RetCode;
					responseVO.RetMessage = orderXRepVO.Result.RetMessage;
				}
				else
				{
					responseVO.RetMessage = "操作超时，请重试！";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			WithDrawOrderReqVO withDrawOrderReqVO = new WithDrawOrderReqVO();
			try
			{
				withDrawOrderReqVO.UserID = req.UserID;
				withDrawOrderReqVO.OrderNo = req.OrderNo;
				withDrawOrderReqVO.SessionID = SysShareInfo.sessionID;
				withDrawOrderReqVO.AgencyNo = req.AgencyNo;
				withDrawOrderReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				WithDrawOrderRepVO withDrawOrderRepVO = (WithDrawOrderRepVO)this.com.commuteBYVO(withDrawOrderReqVO);
				if (withDrawOrderRepVO != null)
				{
					responseVO.RetCode = withDrawOrderRepVO.Result.RetCode;
					responseVO.RetMessage = withDrawOrderRepVO.Result.RetMessage;
				}
				else
				{
					responseVO.RetMessage = "操作超时，请重试！";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public TradeQueryResponseVO TradeQuery(TradeQueryRequestVO req)
		{
			TradeQueryResponseVO tradeQueryResponseVO = new TradeQueryResponseVO();
			TradeQueryReqVO tradeQueryReqVO = new TradeQueryReqVO();
			try
			{
				bool flag = req.AgencyNo.Trim().Length != 0;
				tradeQueryReqVO.UserID = req.UserID;
				tradeQueryReqVO.MarketID = req.MarketID;
				if (flag)
				{
					tradeQueryReqVO.LastTradeID = this.agencyLastTradeId;
				}
				else
				{
					tradeQueryReqVO.LastTradeID = this.lastTradeId;
				}
				tradeQueryReqVO.AgencyNo = req.AgencyNo;
				tradeQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
								tradeInfo.HoldingNO = m_TradeInfo.HoldingNO;
								tradeInfo.TradeTime = m_TradeInfo.TradeTime;
								tradeInfo.BuySell = m_TradeInfo.BuySell;
								tradeInfo.SettleBasis = m_TradeInfo.SettleBasis;
								tradeInfo.TraderID = m_TradeInfo.TraderID;
								tradeInfo.FirmID = m_TradeInfo.FirmID;
								MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeInfo.CommodityID);
								tradeInfo.MarketID = marketAndeComm.marketID;
								tradeInfo.CommodityID = marketAndeComm.commodityID;
								tradeInfo.TradePrice = m_TradeInfo.TradePrice;
								tradeInfo.OpenPrice = m_TradeInfo.OpenPrice;
								tradeInfo.TradeQuantity = m_TradeInfo.TradeQuantity;
								tradeInfo.TransferPL = m_TradeInfo.TransferPL;
								tradeInfo.Comm = m_TradeInfo.Comm;
								tradeInfo.TradeType = m_TradeInfo.TradeType;
								tradeInfo.HoldingPrice = m_TradeInfo.HoldingPrice;
								tradeInfo.OrderTime = m_TradeInfo.OrderTime;
								tradeInfo.OtherID = m_TradeInfo.OtherID;
								tradeQueryResponseVO.TradeInfoList.Add(tradeInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return tradeQueryResponseVO;
		}
		public SysTimeQueryResponseVO GetSysTime(SysTimeQueryRequestVO req)
		{
			SysTimeQueryResponseVO sysTimeQueryResponseVO = new SysTimeQueryResponseVO();
			SysTimeQueryReqVO sysTimeQueryReqVO = new SysTimeQueryReqVO();
			try
			{
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
				sysTimeQueryReqVO.AgencyNo = req.AgencyNo;
				sysTimeQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
						sysTimeQueryResponseVO.LastID = this.lastID;
					}
					sysTimeQueryResponseVO.UpdateData = sysTimeQueryRepVO.Result.UpdateData;
					sysTimeQueryResponseVO.SystemStatus = sysTimeQueryRepVO.Result.SystemStatus;
					sysTimeQueryResponseVO.NewTrade = sysTimeQueryRepVO.Result.NewTrade;
					sysTimeQueryResponseVO.TradeTotal = sysTimeQueryRepVO.Result.TradeTotal;
					sysTimeQueryResponseVO.TradeDay = sysTimeQueryRepVO.Result.TradeDay;
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
							for (int i = 0; i < m_TradeMessage.Count; i++)
							{
								TradeMessage tradeMessage = new TradeMessage();
								tradeMessage.OrderNO = m_TradeMessage[i].OrderNO;
								MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_TradeMessage[i].CommodityID);
								tradeMessage.MarketID = marketAndeComm.marketID;
								tradeMessage.CommodityID = marketAndeComm.commodityID;
								tradeMessage.TradeQuatity = m_TradeMessage[i].TradeQuatity;
								tradeMessage.BuySell = m_TradeMessage[i].BuySell;
								tradeMessage.SettleBasis = m_TradeMessage[i].SettleBasis;
								tradeMessage.TradeType = m_TradeMessage[i].TradeType;
								sysTimeQueryResponseVO.TradeMessageList.Add(tradeMessage);
							}
						}
					}
					if (this.curTradeDay != null && this.curTradeDay.Length > 0 && !this.curTradeDay.Equals(sysTimeQueryRepVO.Result.TradeDay))
					{
						FileInfo fileInfo = new FileInfo(this.tradebuffer);
						fileInfo.Delete();
						this.Dispose();
					}
					this.curTradeDay = sysTimeQueryRepVO.Result.TradeDay;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return sysTimeQueryResponseVO;
		}
		public OrderQueryResponseVO OrderQuery(OrderQueryRequestVO req)
		{
			OrderQueryResponseVO orderQueryResponseVO = new OrderQueryResponseVO();
			OrderQueryReqVO orderQueryReqVO = new OrderQueryReqVO();
			try
			{
				orderQueryReqVO.UserID = req.UserID;
				orderQueryReqVO.BuySell = req.BuySell;
				orderQueryReqVO.OrderNO = req.OrderNO;
				orderQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				orderQueryReqVO.StartNum = (long)req.StartNum;
				orderQueryReqVO.RecordCount = (long)req.RecordCount;
				orderQueryReqVO.SessionID = SysShareInfo.sessionID;
				orderQueryReqVO.UpdateTime = req.UpdateTime;
				orderQueryReqVO.SortField = req.SortField;
				orderQueryReqVO.IsDesc = req.IsDesc;
				orderQueryReqVO.AgencyNo = req.AgencyNo;
				orderQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				OrderQueryRepVO orderQueryRepVO = (OrderQueryRepVO)this.com.commuteBYVO(orderQueryReqVO);
				if (orderQueryRepVO != null)
				{
					orderQueryResponseVO.RetCode = orderQueryRepVO.Result.RetCode;
					orderQueryResponseVO.RetMessage = orderQueryRepVO.Result.RetMessage;
					orderQueryResponseVO.TotalRecord = orderQueryRepVO.Result.TotalRecord;
					List<M_OrderInfo> oderInfoList = orderQueryRepVO.ResultList.OderInfoList;
					int count = this.orderListBuffer.Count;
					if (oderInfoList != null)
					{
						for (int i = 0; i < oderInfoList.Count; i++)
						{
							bool flag = false;
							for (int j = 0; j < count; j++)
							{
								if (oderInfoList[i].OrderNO == this.orderListBuffer[j].OrderNO)
								{
									if (oderInfoList[i].State == 1 || oderInfoList[i].State == 2)
									{
										this.orderListBuffer[j] = oderInfoList[i];
									}
									else
									{
										this.orderListBuffer.RemoveAt(j);
									}
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.orderListBuffer.Add(oderInfoList[i]);
							}
						}
					}
					if (this.orderListBuffer != null && this.orderListBuffer.Count > 0)
					{
						if (orderQueryResponseVO.OrderInfoList == null)
						{
							orderQueryResponseVO.OrderInfoList = new List<OrderInfo>();
						}
						for (int k = 0; k < this.orderListBuffer.Count; k++)
						{
							M_OrderInfo m_OrderInfo = this.orderListBuffer[k];
							OrderInfo orderInfo = new OrderInfo();
							orderInfo.OrderNO = m_OrderInfo.OrderNO;
							orderInfo.Time = m_OrderInfo.Time;
							orderInfo.State = m_OrderInfo.State;
							orderInfo.BuySell = m_OrderInfo.BuySell;
							orderInfo.SettleBasis = m_OrderInfo.SettleBasis;
							orderInfo.TraderID = m_OrderInfo.TraderID;
							orderInfo.FirmID = m_OrderInfo.FirmID;
							MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_OrderInfo.CommodityID);
							orderInfo.MarketID = marketAndeComm.marketID;
							orderInfo.CommodityID = marketAndeComm.commodityID;
							orderInfo.OrderPrice = m_OrderInfo.OrderPrice;
							orderInfo.OrderQuantity = m_OrderInfo.OrderQuantity;
							orderInfo.WithDrawTime = m_OrderInfo.WithDrawTime;
							orderInfo.StopLoss = m_OrderInfo.StopLoss;
							orderInfo.StopProfit = m_OrderInfo.StopProfit;
							orderInfo.OrderType = m_OrderInfo.OrderType;
							orderInfo.OrderFirmID = m_OrderInfo.OrderFirmID;
							orderInfo.HoldingNO = m_OrderInfo.HoldingNO;
							orderInfo.FrozenMargin = m_OrderInfo.FrozenMargin;
							orderInfo.FrozenFee = m_OrderInfo.FrozenFee;
							orderQueryResponseVO.OrderInfoList.Add(orderInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return orderQueryResponseVO;
		}
		public OrderQueryResponseVO AllOrderQuery(OrderQueryRequestVO req)
		{
			OrderQueryResponseVO orderQueryResponseVO = new OrderQueryResponseVO();
			OrderQueryReqVO orderQueryReqVO = new OrderQueryReqVO();
			try
			{
				bool flag = req.AgencyNo.Trim().Length != 0;
				orderQueryReqVO.UserID = req.UserID;
				orderQueryReqVO.BuySell = req.BuySell;
				orderQueryReqVO.OrderNO = req.OrderNO;
				orderQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				orderQueryReqVO.StartNum = (long)req.StartNum;
				orderQueryReqVO.RecordCount = (long)req.RecordCount;
				orderQueryReqVO.SessionID = SysShareInfo.sessionID;
				if (flag)
				{
					orderQueryReqVO.UpdateTime = this.agencyWeekOrderUpdateTime;
				}
				else
				{
					orderQueryReqVO.UpdateTime = this.weekOrderUpdateTime;
				}
				orderQueryReqVO.SortField = req.SortField;
				orderQueryReqVO.IsDesc = req.IsDesc;
				orderQueryReqVO.AgencyNo = req.AgencyNo;
				orderQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				OrderQueryRepVO orderQueryRepVO = (OrderQueryRepVO)this.com.commuteBYVO(orderQueryReqVO);
				if (orderQueryRepVO != null)
				{
					orderQueryResponseVO.RetCode = orderQueryRepVO.Result.RetCode;
					orderQueryResponseVO.RetMessage = orderQueryRepVO.Result.RetMessage;
					orderQueryResponseVO.TotalRecord = orderQueryRepVO.Result.TotalRecord;
					if (flag)
					{
						this.agencyWeekOrderUpdateTime = orderQueryRepVO.Result.UpdateTime;
					}
					else
					{
						this.weekOrderUpdateTime = orderQueryRepVO.Result.UpdateTime;
					}
					List<M_OrderInfo> oderInfoList = orderQueryRepVO.ResultList.OderInfoList;
					int count = this.weekOrderListBuffer.Count;
					if (oderInfoList != null)
					{
						for (int i = 0; i < oderInfoList.Count; i++)
						{
							bool flag2 = false;
							for (int j = 0; j < count; j++)
							{
								if (oderInfoList[i].OrderNO == this.weekOrderListBuffer[j].OrderNO)
								{
									this.weekOrderListBuffer[j] = oderInfoList[i];
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								this.weekOrderListBuffer.Add(oderInfoList[i]);
							}
						}
					}
					if (this.weekOrderListBuffer != null && this.weekOrderListBuffer.Count > 0)
					{
						if (this.weekOrderListBuffer.Count > 500)
						{
							this.WriteWeekOrderInfo(this.weekOrderListBuffer, req.UserID, req.AgencyNo);
							this.WriteOrderTimeInfo(this.weekOrderUpdateTime, req.UserID, req.AgencyNo);
						}
						if (orderQueryResponseVO.OrderInfoList == null)
						{
							orderQueryResponseVO.OrderInfoList = new List<OrderInfo>();
						}
						for (int k = 0; k < this.weekOrderListBuffer.Count; k++)
						{
							M_OrderInfo m_OrderInfo = this.weekOrderListBuffer[k];
							OrderInfo orderInfo = new OrderInfo();
							orderInfo.OrderNO = m_OrderInfo.OrderNO;
							orderInfo.Time = m_OrderInfo.Time;
							orderInfo.State = m_OrderInfo.State;
							orderInfo.BuySell = m_OrderInfo.BuySell;
							orderInfo.SettleBasis = m_OrderInfo.SettleBasis;
							orderInfo.TraderID = m_OrderInfo.TraderID;
							orderInfo.FirmID = m_OrderInfo.FirmID;
							MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_OrderInfo.CommodityID);
							orderInfo.MarketID = marketAndeComm.marketID;
							orderInfo.CommodityID = marketAndeComm.commodityID;
							orderInfo.OrderPrice = m_OrderInfo.OrderPrice;
							orderInfo.OrderQuantity = m_OrderInfo.OrderQuantity;
							orderInfo.WithDrawTime = m_OrderInfo.WithDrawTime;
							orderInfo.StopLoss = m_OrderInfo.StopLoss;
							orderInfo.StopProfit = m_OrderInfo.StopProfit;
							orderInfo.OrderType = m_OrderInfo.OrderType;
							orderInfo.OrderFirmID = m_OrderInfo.OrderFirmID;
							orderInfo.AgentID = m_OrderInfo.AgentID;
							orderInfo.HoldingNO = m_OrderInfo.HoldingNO;
							orderInfo.FrozenMargin = m_OrderInfo.FrozenMargin;
							orderInfo.FrozenFee = m_OrderInfo.FrozenFee;
							orderQueryResponseVO.OrderInfoList.Add(orderInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return orderQueryResponseVO;
		}
		public HoldingDetailResponseVO HoldPtByPrice(HoldingDetailRequestVO req)
		{
			HoldingDetailResponseVO holdingDetailResponseVO = new HoldingDetailResponseVO();
			HoldingDetailReqVO holdingDetailReqVO = new HoldingDetailReqVO();
			try
			{
				holdingDetailReqVO.UserID = req.UserID;
				holdingDetailReqVO.MarketID = req.MarketID;
				holdingDetailReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				holdingDetailReqVO.StartNum = (long)req.StartNum;
				holdingDetailReqVO.RecordCount = (long)req.RecordCount;
				holdingDetailReqVO.SessionID = SysShareInfo.sessionID;
				holdingDetailReqVO.SortField = req.SortField;
				holdingDetailReqVO.IsDesc = req.IsDesc;
				holdingDetailReqVO.AgencyNo = req.AgencyNo;
				holdingDetailReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
							if (m_HoldingDetailInfo.HoldingQuantity > 0L)
							{
								HoldingDetailInfo holdingDetailInfo = new HoldingDetailInfo();
								MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_HoldingDetailInfo.CommodityID);
								holdingDetailInfo.HoldingID = m_HoldingDetailInfo.HoldingID;
								holdingDetailInfo.CommodityID = marketAndeComm.commodityID;
								holdingDetailInfo.BuySell = m_HoldingDetailInfo.BuySell;
								holdingDetailInfo.HoldingQuantity = m_HoldingDetailInfo.HoldingQuantity;
								holdingDetailInfo.OpenQuantity = m_HoldingDetailInfo.OpenQuantity;
								holdingDetailInfo.OpenPrice = m_HoldingDetailInfo.Price;
								holdingDetailInfo.HoldPrice = m_HoldingDetailInfo.HoldingPrice;
								holdingDetailInfo.OrderTime = m_HoldingDetailInfo.OrderTime;
								holdingDetailInfo.Bail = m_HoldingDetailInfo.Bail;
								holdingDetailInfo.TotalFloatingPrice = m_HoldingDetailInfo.FloatProfit;
								holdingDetailInfo.CommPrice = m_HoldingDetailInfo.Comm;
								holdingDetailInfo.StopLoss = m_HoldingDetailInfo.StopLoss;
								holdingDetailInfo.StopProfit = m_HoldingDetailInfo.StopProfit;
								holdingDetailInfo.OtherID = m_HoldingDetailInfo.OtherID;
								holdingDetailInfo.AgentID = m_HoldingDetailInfo.AgentID;
								holdingDetailResponseVO.HoldingDetailInfoList.Add(holdingDetailInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return holdingDetailResponseVO;
		}
		public HoldingQueryResponseVO HoldingQuery(HoldingQueryRequestVO req)
		{
			HoldingQueryResponseVO holdingQueryResponseVO = new HoldingQueryResponseVO();
			HoldingQueryReqVO holdingQueryReqVO = new HoldingQueryReqVO();
			try
			{
				holdingQueryReqVO.UserID = req.UserID;
				holdingQueryReqVO.MarketID = req.MarketID;
				holdingQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				holdingQueryReqVO.StartNum = (long)req.StartNum;
				holdingQueryReqVO.RecordCount = (long)req.RecordCount;
				holdingQueryReqVO.SessionID = SysShareInfo.sessionID;
				holdingQueryReqVO.AgencyNo = req.AgencyNo;
				holdingQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
							if (m_HoldingInfo.Qty > 0L)
							{
								HoldingInfo holdingInfo = new HoldingInfo();
								MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_HoldingInfo.CommodityID);
								holdingInfo.MarketID = marketAndeComm.marketID;
								holdingInfo.CommodityID = marketAndeComm.commodityID;
								holdingInfo.FloatingLP = m_HoldingInfo.FloatingLP;
								holdingInfo.Bail = m_HoldingInfo.Bail;
								holdingInfo.CommPrice = m_HoldingInfo.Comm;
								holdingInfo.TradeType = m_HoldingInfo.TradeType;
								holdingInfo.Qty = m_HoldingInfo.Qty;
								holdingInfo.OpenAveragePrice = m_HoldingInfo.OpenAveragePrice;
								holdingInfo.HoldingAveragePrice = m_HoldingInfo.HoldingAveragePrice;
								holdingInfo.FreezeQty = m_HoldingInfo.FreezeQty;
								holdingQueryResponseVO.HoldingInfoList.Add(holdingInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return holdingQueryResponseVO;
		}
		public CommDataQueryResponseVO CommDataQuery(CommDataQueryRequestVO req)
		{
			CommDataQueryResponseVO commDataQueryResponseVO = new CommDataQueryResponseVO();
			try
			{
				if (req.Identity == Identity.Member.ToString("d"))
				{
					CommDataQueryMemberReqVO commDataQueryMemberReqVO = new CommDataQueryMemberReqVO();
					commDataQueryMemberReqVO.UserID = req.UserID;
					commDataQueryMemberReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
					commDataQueryMemberReqVO.SessionID = SysShareInfo.sessionID;
					CommDataQueryMemberRepVO commDataQueryMemberRepVO = (CommDataQueryMemberRepVO)this.com.commuteBYVO(commDataQueryMemberReqVO);
					if (commDataQueryMemberRepVO != null)
					{
						commDataQueryResponseVO.RetCode = commDataQueryMemberRepVO.Result.RetCode;
						commDataQueryResponseVO.RetMessage = commDataQueryMemberRepVO.Result.RetMessage;
						List<M_CommDataMember> commDataList = commDataQueryMemberRepVO.ResultList.CommDataList;
						if (commDataList != null && commDataList.Count > 0)
						{
							if (commDataQueryResponseVO.CommDataList == null)
							{
								commDataQueryResponseVO.CommDataList = new Dictionary<string, CommData>();
							}
							for (int i = 0; i < commDataList.Count; i++)
							{
								M_CommDataMember m_CommDataMember = commDataList[i];
								CommData commData = new CommData();
								MarketAndeComm marketAndeComm = this.GetMarketAndeComm(m_CommDataMember.CommodityID);
								commData.MarketID = marketAndeComm.marketID;
								commData.CommodityID = m_CommDataMember.CommodityID;
								commData.High = m_CommDataMember.High;
								commData.Low = m_CommDataMember.Low;
								commData.BuyPrice = m_CommDataMember.BasePrice + m_CommDataMember.SMemberBuyDianCha;
								commData.SellPrice = m_CommDataMember.BasePrice - m_CommDataMember.SMemberSellDianCha;
								commData.CustomerBuyPrice = m_CommDataMember.BasePrice + m_CommDataMember.MemberBuyDianCha;
								commData.CustomerSellPrice = m_CommDataMember.BasePrice - m_CommDataMember.MemberSellDianCha;
								commData.UpdateTime = m_CommDataMember.UpdateTime;
								commDataQueryResponseVO.CommDataList.Add(m_CommDataMember.CommodityID, commData);
							}
						}
					}
				}
				else
				{
					CommDataQueryReqVO commDataQueryReqVO = new CommDataQueryReqVO();
					commDataQueryReqVO.UserID = req.UserID;
					commDataQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
					commDataQueryReqVO.SessionID = SysShareInfo.sessionID;
					commDataQueryReqVO.AgencyNo = req.AgencyNo;
					commDataQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
					CommDataQueryRepVO commDataQueryRepVO = (CommDataQueryRepVO)this.com.commuteBYVO(commDataQueryReqVO);
					if (commDataQueryRepVO != null)
					{
						commDataQueryResponseVO.RetCode = commDataQueryRepVO.Result.RetCode;
						commDataQueryResponseVO.RetMessage = commDataQueryRepVO.Result.RetMessage;
						List<M_CommData> commDataList2 = commDataQueryRepVO.ResultList.CommDataList;
						if (commDataList2 != null && commDataList2.Count > 0)
						{
							if (commDataQueryResponseVO.CommDataList == null)
							{
								commDataQueryResponseVO.CommDataList = new Dictionary<string, CommData>();
							}
							for (int j = 0; j < commDataList2.Count; j++)
							{
								M_CommData m_CommData = commDataList2[j];
								CommData commData2 = new CommData();
								MarketAndeComm marketAndeComm2 = this.GetMarketAndeComm(m_CommData.CommodityID);
								commData2.MarketID = marketAndeComm2.marketID;
								commData2.CommodityID = m_CommData.CommodityID;
								commData2.High = m_CommData.High;
								commData2.Low = m_CommData.Low;
								commData2.BuyPrice = m_CommData.BuyPrice;
								commData2.SellPrice = m_CommData.SellPrice;
								commData2.UpdateTime = m_CommData.UpdateTime;
								commDataQueryResponseVO.CommDataList.Add(m_CommData.CommodityID, commData2);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return commDataQueryResponseVO;
		}
		public CommodityQueryResponseVO CommodityQuery(CommodityQueryRequestVO req)
		{
			CommodityQueryResponseVO commodityQueryResponseVO = new CommodityQueryResponseVO();
			CommodityQueryReqVO commodityQueryReqVO = new CommodityQueryReqVO();
			try
			{
				commodityQueryReqVO.UserID = req.UserID;
				commodityQueryReqVO.CommodityID = this.GetCommodityID(req.MarketID, req.CommodityID);
				commodityQueryReqVO.SessionID = SysShareInfo.sessionID;
				commodityQueryReqVO.AgencyNo = req.AgencyNo;
				commodityQueryReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
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
							commodityQueryResponseVO.CommodityInfoList = new Dictionary<string, CommodityInfo>();
						}
						for (int i = 0; i < commodityList.Count; i++)
						{
							M_CommodityInfo m_CommodityInfo = commodityList[i];
							CommodityInfo commodityInfo = new CommodityInfo();
							commodityInfo.MarketID = m_CommodityInfo.MarketID;
							commodityInfo.CommodityID = m_CommodityInfo.CommodityID;
							commodityInfo.CommodityName = m_CommodityInfo.CommodityName;
							commodityInfo.Status = m_CommodityInfo.Status;
							commodityInfo.CtrtSize = m_CommodityInfo.CtrtSize;
							commodityInfo.Spread = m_CommodityInfo.Spread;
							commodityInfo.SpreadUp = m_CommodityInfo.SpreadUp;
							commodityInfo.SpreadDown = m_CommodityInfo.SpreadDown;
							commodityInfo.MarginType = m_CommodityInfo.MarginType;
							commodityInfo.MarginValue = m_CommodityInfo.MarginValue;
							commodityInfo.PrevClear = m_CommodityInfo.PrevClear;
							commodityInfo.CommType = m_CommodityInfo.CommType;
							commodityInfo.DeliveryCommType = m_CommodityInfo.DeliveryCommType;
							commodityInfo.DeliveryBComm = m_CommodityInfo.DeliveryBComm;
							commodityInfo.DeliverySComm = m_CommodityInfo.DeliverySComm;
							commodityInfo.VarietyID = m_CommodityInfo.VarietyID;
							commodityInfo.TradeMode = m_CommodityInfo.TradeMode;
							commodityInfo.DeferType = m_CommodityInfo.DeferAmountType;
							commodityInfo.P_MIN_H = m_CommodityInfo.P__MIN__H;
							commodityInfo.P_MAX_H = m_CommodityInfo.P__M__H;
							commodityInfo.MaxHolding = m_CommodityInfo.MaxHolding;
							commodityInfo.W_D_T_P = m_CommodityInfo.W__D__T__P;
							commodityInfo.W_D_S_L_P = m_CommodityInfo.W__D__S__L__P;
							commodityInfo.W_D_S_P_P = m_CommodityInfo.W__D__S__P__P;
							commodityInfo.B_O_P = m_CommodityInfo.B__O__P;
							commodityInfo.B_L_P = m_CommodityInfo.B__L__P;
							commodityInfo.B_X_O_P = m_CommodityInfo.B__X__O__P;
							commodityInfo.B_S_L = m_CommodityInfo.B__S__L;
							commodityInfo.B_S_P = m_CommodityInfo.B__S__P;
							commodityInfo.S_O_P = m_CommodityInfo.S__O__P;
							commodityInfo.S_L_P = m_CommodityInfo.S__L__P;
							commodityInfo.S_X_O_P = m_CommodityInfo.S__X__O__P;
							commodityInfo.S_S_L = m_CommodityInfo.S__S__L;
							commodityInfo.S_S_P = m_CommodityInfo.S__S__P;
							commodityInfo.B_P_D_D = m_CommodityInfo.B__P__D__D;
							commodityInfo.S_P_D_D = m_CommodityInfo.S__P__D__D;
							commodityInfo.X_O_B_D_D = m_CommodityInfo.X__O__B__D__D;
							commodityInfo.X_O_S_D_D = m_CommodityInfo.X__O__S__D__D;
							commodityInfo.U_O_D_D_MIN = m_CommodityInfo.U__O__D__D__MIN;
							commodityInfo.U_O_D_D_MAX = m_CommodityInfo.U__O__D__D__MAX;
							commodityInfo.U_O_D_D_DF = m_CommodityInfo.U__O__D__D__DF;
							commodityInfo.OrderNum = m_CommodityInfo.OrderNum;
							commodityInfo.B_J_H = m_CommodityInfo.B__J__H;
							commodityInfo.STOP_L_P = m_CommodityInfo.STOP__L__P;
							commodityInfo.STOP_P_P = m_CommodityInfo.STOP__P__P;
							commodityInfo.FeeType = m_CommodityInfo.FEE__T;
							commodityInfo.FeeValue = m_CommodityInfo.FEE__V;
							commodityInfo.CommodityUnit = m_CommodityInfo.CommodityUnit;
							if (m_CommodityInfo.YanQiList != null && m_CommodityInfo.YanQiList.M_YanQiList != null && m_CommodityInfo.YanQiList.M_YanQiList.Count > 0)
							{
								List<M_YanQi> m_YanQiList = m_CommodityInfo.YanQiList.M_YanQiList;
								if (commodityInfo.YanQiFeeList == null)
								{
									commodityInfo.YanQiFeeList = new List<YanQiFee>();
								}
								foreach (M_YanQi current in m_YanQiList)
								{
									YanQiFee yanQiFee = new YanQiFee();
									yanQiFee.StepValue = current.StepValue;
									yanQiFee.StepLow = current.StepLow;
									yanQiFee.YanQiValue = current.YanValue;
									commodityInfo.YanQiFeeList.Add(yanQiFee);
								}
							}
							commodityQueryResponseVO.CommodityInfoList.Add(m_CommodityInfo.CommodityID, commodityInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return commodityQueryResponseVO;
		}
		public MarketQueryResponseVO MarketQuery(MarketQueryRequestVO req)
		{
			MarketQueryResponseVO marketQueryResponseVO = new MarketQueryResponseVO();
			MarketQueryReqVO marketQueryReqVO = new MarketQueryReqVO();
			try
			{
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
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return marketQueryResponseVO;
		}
		public FirmbreedQueryResponseVO FirmbreedQuery(string userID)
		{
			FirmbreedQueryResponseVO firmbreedQueryResponseVO = new FirmbreedQueryResponseVO();
			FirmBreedReqVO firmBreedReqVO = new FirmBreedReqVO();
			try
			{
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
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return firmbreedQueryResponseVO;
		}
		private string GetCommodityID(string marketID, string commodityID)
		{
			return commodityID;
		}
		private MarketAndeComm GetMarketAndeComm(string commodityID)
		{
			MarketAndeComm marketAndeComm = new MarketAndeComm();
			try
			{
				if (commodityID != null)
				{
					marketAndeComm.marketID = "0";
					marketAndeComm.commodityID = commodityID;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return marketAndeComm;
		}
		public ResponseVO SetLossProfit(SetLossProfitRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			SetLossProfitReqVO setLossProfitReqVO = new SetLossProfitReqVO();
			try
			{
				setLossProfitReqVO.UserID = req.UserID;
				setLossProfitReqVO.SessionID = SysShareInfo.sessionID;
				setLossProfitReqVO.StopLoss = req.StopLoss;
				setLossProfitReqVO.StopProfit = req.StopProfit;
				setLossProfitReqVO.Holding_ID = req.HoldingID;
				setLossProfitReqVO.BuySellType = req.BuySellType;
				setLossProfitReqVO.CommodityID = req.CommodityID;
				setLossProfitReqVO.AgencyNo = req.AgencyNo;
				setLossProfitReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				SetLossProfitRepVO setLossProfitRepVO = (SetLossProfitRepVO)this.com.commuteBYVO(setLossProfitReqVO);
				if (setLossProfitRepVO != null)
				{
					responseVO.RetCode = setLossProfitRepVO.Result.RetCode;
					responseVO.RetMessage = setLossProfitRepVO.Result.RetMessage;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public ResponseVO WithdrawLossProfit(WithdrawLossProfitRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			WithdrawLossProfitReqVO withdrawLossProfitReqVO = new WithdrawLossProfitReqVO();
			try
			{
				withdrawLossProfitReqVO.UserID = req.UserID;
				withdrawLossProfitReqVO.SessionID = SysShareInfo.sessionID;
				withdrawLossProfitReqVO.WithdrawType = req.Type;
				withdrawLossProfitReqVO.Holding_ID = req.HoldingID;
				withdrawLossProfitReqVO.CommodityID = req.CommodityID;
				withdrawLossProfitReqVO.AgencyNo = req.AgencyNo;
				withdrawLossProfitReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				WithdrawLossProfitRepVO withdrawLossProfitRepVO = (WithdrawLossProfitRepVO)this.com.commuteBYVO(withdrawLossProfitReqVO);
				if (withdrawLossProfitRepVO != null)
				{
					responseVO.RetCode = withdrawLossProfitRepVO.Result.RetCode;
					responseVO.RetMessage = withdrawLossProfitRepVO.Result.RetMessage;
				}
				else
				{
					responseVO.RetMessage = "操作超时，请重试！";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
		public FirmFundsInfoResponseVO GetFirmFundsInfo(string userID)
		{
			FirmFundsInfoResponseVO firmFundsInfoResponseVO = new FirmFundsInfoResponseVO();
			FirmFundsInfoReqVO firmFundsInfoReqVO = new FirmFundsInfoReqVO();
			try
			{
				firmFundsInfoReqVO.UserID = userID;
				firmFundsInfoReqVO.SessionID = SysShareInfo.sessionID;
				FirmFundsInfoRepVO firmFundsInfoRepVO = (FirmFundsInfoRepVO)this.com.commuteBYVO(firmFundsInfoReqVO);
				if (firmFundsInfoRepVO != null)
				{
					firmFundsInfoResponseVO.RetCode = firmFundsInfoRepVO.Result.RetCode;
					firmFundsInfoResponseVO.RetMessage = firmFundsInfoRepVO.Result.RetMessage;
					List<M_FirmFundsInfo> firmFundsInfoList = firmFundsInfoRepVO.ResultList.FirmFundsInfoList;
					if (firmFundsInfoList != null && firmFundsInfoList.Count > 0)
					{
						M_FirmFundsInfo m_FirmFundsInfo = firmFundsInfoList[0];
						firmFundsInfoResponseVO.MemberPureFloating = m_FirmFundsInfo.PureFloatProfit;
						firmFundsInfoResponseVO.CustomerTradeFloating = m_FirmFundsInfo.TradeFloatProfit;
						firmFundsInfoResponseVO.CustomerCloseProfit = m_FirmFundsInfo.CustomerCloseProfit;
						firmFundsInfoResponseVO.DuiChongFloating = m_FirmFundsInfo.DuiChongFloatProfit;
						firmFundsInfoResponseVO.RiskMargin = m_FirmFundsInfo.RiskMargin;
						firmFundsInfoResponseVO.MemberJingTouCun = m_FirmFundsInfo.JingTouCun;
						firmFundsInfoResponseVO.JingTouCunAlertLimit = m_FirmFundsInfo.JingTouCunAlertLimit;
						firmFundsInfoResponseVO.MemberFundAlertLimit = m_FirmFundsInfo.MemberFundAlertLimit;
						firmFundsInfoResponseVO.CustomerFundAlertLimit = m_FirmFundsInfo.CustomerFundAlertLimit;
						firmFundsInfoResponseVO.MemberFreezeLimit = m_FirmFundsInfo.MemberFreezeLimit;
						firmFundsInfoResponseVO.JingTouCunAlertLimit = m_FirmFundsInfo.JingTouCunAlertLimit;
						firmFundsInfoResponseVO.JingTouCunMaxAlertLimit = m_FirmFundsInfo.JingTouCunMaxAlertLimit;
						firmFundsInfoResponseVO.Status = m_FirmFundsInfo.Status;
						firmFundsInfoResponseVO.MemberType = m_FirmFundsInfo.MemberType;
						firmFundsInfoResponseVO.MinRiskFund = m_FirmFundsInfo.MinRiskFund;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return firmFundsInfoResponseVO;
		}
		public CustomerOrderQueryResponseVO GetCustomerOrderQuery(CustomerOrderQueryRequestVO req)
		{
			CustomerOrderQueryResponseVO customerOrderQueryResponseVO = new CustomerOrderQueryResponseVO();
			CustomerOrderQueryReqVO customerOrderQueryReqVO = new CustomerOrderQueryReqVO();
			try
			{
				customerOrderQueryReqVO.UserID = req.UserID;
				customerOrderQueryReqVO.CommodityID = req.CommodityID;
				customerOrderQueryReqVO.SessionID = SysShareInfo.sessionID;
				CustomerOrderQueryRepVO customerOrderQueryRepVO = (CustomerOrderQueryRepVO)this.com.commuteBYVO(customerOrderQueryReqVO);
				if (customerOrderQueryRepVO != null)
				{
					customerOrderQueryResponseVO.RetCode = customerOrderQueryRepVO.Result.RetCode;
					customerOrderQueryResponseVO.RetMessage = customerOrderQueryRepVO.Result.RetMessage;
					List<M_CustomerOrderQuery> customerOrderQueryList = customerOrderQueryRepVO.ResultList.CustomerOrderQueryList;
					if (customerOrderQueryList != null && customerOrderQueryList.Count > 0)
					{
						foreach (M_CustomerOrderQuery current in customerOrderQueryList)
						{
							CustomerOrderQuery customerOrderQuery = new CustomerOrderQuery();
							customerOrderQuery.CommodityID = current.CommodityID;
							customerOrderQuery.BuyAveragePrice = current.BuyAveragePrice;
							customerOrderQuery.BuyHoldingAmount = current.BuyHoldingAmount;
							customerOrderQuery.BuyQuantity = current.BuyQuantity;
							customerOrderQuery.BuyFloat = current.BuyFloat;
							customerOrderQuery.SellAveragePrice = current.SellAveragePrice;
							customerOrderQuery.SellHoldingAmount = current.SellHoldingAmount;
							customerOrderQuery.SellFloat = current.SellFloat;
							customerOrderQuery.SellQuantity = current.SellQuantity;
							customerOrderQuery.JingTouCun = current.JingTouCun;
							customerOrderQuery.Float = current.Float;
							customerOrderQueryResponseVO.CustomerOrderQueryList.Add(customerOrderQuery);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return customerOrderQueryResponseVO;
		}
		public EspecialMemberQueryResponseVO GetEspecialMemberQuery(string userID, bool isGetDefault)
		{
			EspecialMemberQueryResponseVO especialMemberQueryResponseVO = new EspecialMemberQueryResponseVO();
			EspecialMemberQueryReqVO especialMemberQueryReqVO = new EspecialMemberQueryReqVO();
			try
			{
				especialMemberQueryReqVO.UserID = userID;
				especialMemberQueryReqVO.SessionID = SysShareInfo.sessionID;
				especialMemberQueryReqVO.IsGetDefault = (isGetDefault ? Convert.ToInt16(1) : Convert.ToInt16(0));
				EspecialMemberQueryRepVO especialMemberQueryRepVO = (EspecialMemberQueryRepVO)this.com.commuteBYVO(especialMemberQueryReqVO);
				if (especialMemberQueryRepVO != null)
				{
					especialMemberQueryResponseVO.RetCode = especialMemberQueryRepVO.Result.RetCode;
					especialMemberQueryResponseVO.RetMessage = especialMemberQueryRepVO.Result.RetMessage;
					List<M_EspecialMemberQuery> especialMemberQueryList = especialMemberQueryRepVO.ResultList.EspecialMemberQueryList;
					if (especialMemberQueryList != null && especialMemberQueryList.Count > 0)
					{
						if (especialMemberQueryResponseVO.EspecialMemberQueryList == null)
						{
							especialMemberQueryResponseVO.EspecialMemberQueryList = new List<EspecialMemberQuery>();
						}
						foreach (M_EspecialMemberQuery current in especialMemberQueryList)
						{
							EspecialMemberQuery especialMemberQuery = new EspecialMemberQuery();
							especialMemberQuery.EspecialMemberID = current.EspecialMemberID;
							especialMemberQuery.EspecialMemberName = current.EspecialMemberName;
							especialMemberQueryResponseVO.EspecialMemberQueryList.Add(especialMemberQuery);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return especialMemberQueryResponseVO;
		}
		public FirmHoldSumResponseVO GetFirmHoldSumQuery(string userID)
		{
			FirmHoldSumResponseVO firmHoldSumResponseVO = new FirmHoldSumResponseVO();
			FirmHoldSumReqVO firmHoldSumReqVO = new FirmHoldSumReqVO();
			try
			{
				firmHoldSumReqVO.UserID = userID;
				firmHoldSumReqVO.SessionID = SysShareInfo.sessionID;
				FirmHoldSumRepVO firmHoldSumRepVO = (FirmHoldSumRepVO)this.com.commuteBYVO(firmHoldSumReqVO);
				if (firmHoldSumRepVO != null)
				{
					firmHoldSumResponseVO.RetCode = firmHoldSumRepVO.Result.RetCode;
					firmHoldSumResponseVO.RetMessage = firmHoldSumRepVO.Result.RetMessage;
					List<M_FirmHoldSum> firmHoldSumList = firmHoldSumRepVO.ResultList.FirmHoldSumList;
					if (firmHoldSumList != null && firmHoldSumList.Count > 0)
					{
						foreach (M_FirmHoldSum current in firmHoldSumList)
						{
							FirmHoldSumQuery firmHoldSumQuery = new FirmHoldSumQuery();
							firmHoldSumQuery.CommodityID = current.CommodityID;
							firmHoldSumQuery.CustomerJingTouCun = current.CustomerJingTouCun;
							firmHoldSumQuery.CustomerTradeFloating = current.CustomerTradeFloating;
							firmHoldSumQuery.DuiChongFloating = current.DuiChongFloating;
							firmHoldSumQuery.DuiChongJingTouCun = current.DuiChongJingTouCun;
							firmHoldSumQuery.HoldingNetFloating = current.HoldingNetFloating;
							firmHoldSumQuery.MaxHolding = current.MaxHolding;
							firmHoldSumQuery.MemberJingTouCun = current.MemberJingTouCun;
							firmHoldSumResponseVO.FirmHoldSumQueryList.Add(firmHoldSumQuery);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return firmHoldSumResponseVO;
		}
		public ResponseVO AgencyLogon(AgencyLogonRequestVO req)
		{
			ResponseVO responseVO = new ResponseVO();
			AgencyLogonReqVO agencyLogonReqVO = new AgencyLogonReqVO();
			try
			{
				agencyLogonReqVO.UserID = req.UserID;
				agencyLogonReqVO.SessionID = SysShareInfo.sessionID;
				agencyLogonReqVO.AgencyNo = req.AgencyNo;
				agencyLogonReqVO.AgencyPhonePassword = req.AgencyPhonePassword;
				AgencyLogonRepVO agencyLogonRepVO = (AgencyLogonRepVO)this.com.commuteBYVO(agencyLogonReqVO);
				if (agencyLogonRepVO != null)
				{
					responseVO.RetCode = agencyLogonRepVO.Result.RetCode;
					responseVO.RetMessage = agencyLogonRepVO.Result.RetMessage;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return responseVO;
		}
	}
}
