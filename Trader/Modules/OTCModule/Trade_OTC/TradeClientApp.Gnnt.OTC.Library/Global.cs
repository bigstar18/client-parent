using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using ToolsLibrary.util;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class Global
	{
		public static bool IsWriteLog = false;
		public static Hashtable HTConfig = null;
		public static ResourceManager M_ResourceManager = null;
		public static ResourceManager m_PMESResourceManager = null;
		public static string UserID = string.Empty;
		public static string Password = string.Empty;
		public static string AgencyNo = string.Empty;
		public static string AgencyPhonePassword = string.Empty;
		public static string RegisterWord = string.Empty;
		public static string FirmID = string.Empty;
		public static int CustomerCount = 0;
		public static Hashtable MarketHT;
		public static string ConfigPath = string.Empty;
		public static string CommCodeXml = "CommodityCode.xml";
		public static string TrancCodeXml = "TransactionsCode.xml";
		public static string PreDelegateXml = "PreDelegate.xml";
		public static DateTime ServerTime;
		public static Color ErrorColor = Color.Red;
		public static Color RightColor = Color.Black;
		public static Color HighColor = Color.Red;
		public static Color LowColor = Color.Green;
		public static Color EqualsColor = Color.Black;
		public static Color LightColor = Color.LimeGreen;
		public static bool broadcastFlag = false;
		public static Hashtable HtForm = new Hashtable();
		public static int screenWidth = 1024;
		public static int screenHight = 760;
		public static string[] FirmStatusStrArr = new string[]
		{
			"正常",
			"禁止交易",
			"退市"
		};
		public static string[] BuySellStrArr = new string[]
		{
			"",
			"买入",
			"卖出"
		};
		public static string[] SettleBasisStrArr = new string[]
		{
			"",
			"建仓",
			"平仓"
		};
		public static string[] CloseModeStrArr = new string[]
		{
			"不指定转让",
			"指定时间转让",
			"指定价格转让"
		};
		public static string[] TimeFlagStrArr = new string[]
		{
			"转让今日",
			"转让历史"
		};
		public static string[] OrderStatusStrArr = new string[]
		{
			"",
			"已委托",
			"部分成交",
			"全部成交",
			"正在撤单",
			"全部撤单",
			"部分成交后撤单",
			"撤单委托成功",
			"撤单委托失败"
		};
		public static string[] TradeTypeStrArr = new string[]
		{
			"",
			"正常交易",
			"代理系统强平",
			"交易市场强平",
			"委托交易",
			"无委托成交",
			"协议平仓"
		};
		public static string[] CBasisStrArr = new string[]
		{
			"正常转让",
			"代为转让",
			"强行转让",
			""
		};
		public static string[] BillTradeTypeStrArr = new string[]
		{
			"",
			"卖仓单",
			"抵顶转让"
		};
		public static Dictionary<string, CommData> _AgencyHQCommData = null;
		public static Dictionary<string, CommData> _HQCommData = null;
		public static Dictionary<string, CommodityInfo> AgencyCommodityData = null;
		public static Dictionary<string, CommodityInfo> CommodityData = null;
		public static List<EspecialMemberQuery> AgencyEspecialMemberList = null;
		public static List<EspecialMemberQuery> EspecialMemberList = null;
		public static Hashtable AllEspecialMemberList = null;
		public static List<OrderInfo> AgencyOrderInfoList = null;
		public static List<OrderInfo> OrderInfoList = null;
		public static FirmInfoResponseVO AgencyFirmInfoData = null;
		public static FirmInfoResponseVO FirmInfoData = null;
		public static List<HoldingInfo> AgencyHoldingInfoList = null;
		public static List<HoldingInfo> HoldingInfoList = null;
		public static List<CustomerOrderQuery> CustomerOrderQueryList = null;
		public static List<CustomerOrderQuery> AgencyCustomerOrderQueryList = null;
		public static int TradeTimeOut = 20;
		public static Icon SystamIcon = null;
		public static string SystamText = string.Empty;
		public static string SystamVersion = string.Empty;
		public static MemberStatus sMemberStatus;
		public static MemberType sMemberType;
		public static MemberType sAgencyMemberType;
		public static object HQCommDataLock = new object();
		public static object AgencyHQCommDataLock = new object();
		public static object AgencyFirmInfoDataLock = new object();
		public static object FirmInfoDataLock = new object();
		public static string formatMoney = "C";
		public static bool YuJingFlag = false;
		public static string CurrentDirectory = Directory.GetCurrentDirectory();
		public static Dictionary<string, CommData> AgencyHQCommData
		{
			get
			{
				return Global._AgencyHQCommData;
			}
			set
			{
				Global._AgencyHQCommData = value;
			}
		}
		public static Dictionary<string, CommData> gAgencyHQCommData
		{
			get
			{
				Dictionary<string, CommData> result;
				lock (Global.AgencyHQCommDataLock)
				{
					if (Global._AgencyHQCommData == null)
					{
						result = Global._AgencyHQCommData;
						return result;
					}
				}
				lock (Global.AgencyHQCommDataLock)
				{
					Dictionary<string, CommData> dictionary = new Dictionary<string, CommData>();
					foreach (KeyValuePair<string, CommData> current in Global._AgencyHQCommData)
					{
						dictionary.Add(current.Key, (CommData)current.Value.Clone());
					}
					result = dictionary;
				}
				return result;
			}
		}
		public static Dictionary<string, CommData> HQCommData
		{
			get
			{
				return Global._HQCommData;
			}
			set
			{
				Global._HQCommData = value;
			}
		}
		public static Dictionary<string, CommData> gHQCommData
		{
			get
			{
				Dictionary<string, CommData> result;
				lock (Global.HQCommDataLock)
				{
					if (Global._HQCommData == null)
					{
						result = Global._HQCommData;
						return result;
					}
				}
				lock (Global.HQCommDataLock)
				{
					Dictionary<string, CommData> dictionary = new Dictionary<string, CommData>();
					foreach (KeyValuePair<string, CommData> current in Global._HQCommData)
					{
						dictionary.Add(current.Key, (CommData)current.Value.Clone());
					}
					result = dictionary;
				}
				return result;
			}
		}
		public static Font GetIniFont()
		{
			Font result;
			try
			{
				float emSize = 10.5f;
				IniFile iniFile = new IniFile(string.Format(Global.ConfigPath + "{0}Trade.ini", Global.UserID));
				string familyName = iniFile.IniReadValue("Font", "FontFamily");
				try
				{
					emSize = float.Parse(iniFile.IniReadValue("Font", "Size"));
				}
				catch
				{
					emSize = 10.5f;
				}
				int style;
				try
				{
					style = int.Parse(iniFile.IniReadValue("Font", "FontStyle"));
				}
				catch
				{
					style = 0;
				}
				Font font = new Font(familyName, emSize, (FontStyle)style);
				result = font;
			}
			catch (Exception)
			{
				result = new Font("宋体", 10.5f, FontStyle.Regular);
			}
			return result;
		}
		public static short StrToShort(string[] strArr, string str)
		{
			short result = 0;
			for (int i = 0; i < strArr.Length; i++)
			{
				if (str.Equals(strArr[i]))
				{
					result = (short)i;
					break;
				}
			}
			return result;
		}
		public static string GetEnumtoResourcesString(string type, int val)
		{
			return Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}_{2}_{3}", new object[]
			{
				"PMESStr",
				"ENUM",
				type,
				val
			}));
		}
		public static string toTime(string time)
		{
			string result = string.Empty;
			if (!time.Equals("") && time.Length == 6)
			{
				result = string.Concat(new string[]
				{
					time.Substring(0, 2),
					":",
					time.Substring(2, 2),
					":",
					time.Substring(4, 2)
				});
			}
			else if (!time.Equals("") && time.Length > 8)
			{
				result = time.Substring(time.Length - 8);
			}
			return result;
		}
		public static bool DoubleIsZero(double val)
		{
			return val < 1E-06 && val > -1E-06;
		}
		public static string GetFullPath(string strPath)
		{
			if (strPath.IndexOf(":") > 0)
			{
				return strPath;
			}
			return Environment.CurrentDirectory + "/" + strPath;
		}
	}
}
