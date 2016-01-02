using System;
using System.Collections;
using System.Drawing;
using System.Resources;
using System.Text;
using TradeInterface.Gnnt.ISSUE.Interface;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class Global
	{
		public static bool IsWriteLog = false;
		public static ITradeLibrary TradeLibrary;
		public static Hashtable HTConfig = null;
		public static ResourceManager M_ResourceManager = null;
		public static string UserID = string.Empty;
		public static string Password = string.Empty;
		public static string RegisterWord = string.Empty;
		public static string FirmID = string.Empty;
		public static int CustomerCount = 0;
		public static Hashtable MarketHT;
		public static string CommCodeXml = "ISSUE_CommodityCode.xml";
		public static string TrancCodeXml = "ISSUE_TransactionsCode.xml";
		public static string PreDelegateXml = "ISSUE_PreDelegate.xml";
		public static string ConfigPath;
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
			"订立",
			"转让"
		};
		public static string[] CloseModeStrArr = new string[]
		{
			"不指定卖出",
			"指定时间卖出",
			"指定价格卖出"
		};
		public static string[] TimeFlagStrArr = new string[]
		{
			"当日卖出",
			"隔日卖出"
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
		public static string formatMoney = "C";
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
		public static string BuilderString(string money)
		{
			if (money.Length <= 0)
			{
				return null;
			}
			if (money.StartsWith("-"))
			{
				StringBuilder stringBuilder = new StringBuilder(money);
				return stringBuilder.Remove(1, 1).ToString();
			}
			StringBuilder stringBuilder2 = new StringBuilder(money);
			return stringBuilder2.Remove(0, 1).ToString();
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
		public static string ToPercent(double radio)
		{
			string result = string.Empty;
			string text = Math.Round(radio * 100.0, 2).ToString();
			string[] array = text.Split(new char[]
			{
				'.'
			});
			if (array.Length == 1)
			{
				result = text + ".00%";
			}
			else if (array.Length == 2)
			{
				if (array[1].Length == 1)
				{
					result = text + "0%";
				}
				else if (array[1].Length == 2)
				{
					result = text + "%";
				}
			}
			return result;
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
