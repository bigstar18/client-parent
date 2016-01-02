using FuturesTrade.Gnnt.BLL.Manager;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using ToolsLibrary.util;
using TradeInterface.Gnnt.Interface;
namespace FuturesTrade.Gnnt.Library
{
	public class Global
	{
		public delegate void SetOrderInfoCallBack(string commodityID, double buyPrice, double sellPrice);
		public delegate void SetDoubleClickOrderInfoCallBack(double price, double Lprice, int qty, short buysell, short ordertype);
		public delegate bool SetCommoditySelectIndexCallBack(string marketID, string commodityID);
		public delegate void StatusInfoFillCallBack(string message, Color color, bool display);
		public static Global.SetDoubleClickOrderInfoCallBack SetDoubleClickOrderInfo;
		public static Global.SetCommoditySelectIndexCallBack SetCommoditySelectIndex;
		public static Global.StatusInfoFillCallBack StatusInfoFill;
		public static bool LoadFlag = true;
		public static int orderCount = 0;
		public static string m_order = " Desc ";
		public static bool IsWriteLog = false;
		public static bool isShowOrder = false;
		public static NumberFormatInfo MyNumberFormatInfo = new CultureInfo("zh-CN", false).NumberFormat;
		public static ITradeLibrary TradeLibrary;
		public static Hashtable HTConfig = null;
		public static ResourceManager M_ResourceManager = null;
		public static string UserID = string.Empty;
		public static string Password = string.Empty;
		public static string RegisterWord = string.Empty;
		public static DateTime IdleStartTime = DateTime.Now;
		public static string FirmID = string.Empty;
		public static string CustomerID = "00";
		public static int CustomerCount = 0;
		public static Hashtable MarketHT;
		public static string MarketID = string.Empty;
		public static int MaxCount = 1000;
		public static int PagNum = 300;
		public static string curTradeDay = string.Empty;
		public static string formatMoney = "C";
		public static int DecimalDigits = 2;
		public static string CommCodeXml = "MEBS_CommodityCode.xml";
		public static string TrancCodeXml = "MEBS_TransactionsCode.xml";
		public static string PreDelegateXml = "MEBS_PreDelegate.xml";
		public static string MyCommodityXml = "MyCommodity.xml";
		public static string ConfigPath = string.Empty;
		public static DateTime ServerTime;
		public static Color ErrorColor = Color.Red;
		public static Color RightColor = Color.Black;
		public static Color HighColor = Color.Red;
		public static Color LowColor = Color.Green;
		public static Color EqualsColor = Color.Black;
		public static Color LightColor = Color.LimeGreen;
		public static bool broadcastFlag = false;
		public static int screenWidth = 1024;
		public static int screenHight = 760;
		public static string[] FirmStatusStrArr;
		public static string[] BuySellStrArr;
		public static string[] SettleBasisStrArr;
		public static string[] CloseModeStrArr;
		public static string[] TimeFlagStrArr;
		public static string[] OrderStatusStrArr;
		public static string[] TradeTypeStrArr;
		public static string[] CBasisStrArr;
		public static string[] BillTradeTypeStrArr;
		public static string[] ConditionTypeStrArr;
		public static string[] ConditionOperatorStrArr;
		public static string[] OrderTypeStrArr;
		public static string[] ConditionSignStrArr = new string[]
		{
			"<=",
			"<",
			"=",
			">",
			">="
		};
		public static event Global.SetOrderInfoCallBack SetOrderInfo;
		public static event InterFace.CommodityInfoEventHander MinLineEvent;
		public static event InterFace.CommodityInfoEventHander KLineEvent;
		public static event EventHandler LockFormEvent;
		public static event EventHandler ChangeServerEvent;
		public static event EventHandler FloatFormEvent;
		public static event EventHandler UpdateStyleEvent;
		public static event EventHandler LogoutEvent;
		public static void GetArrItems()
		{
			Global.FirmStatusStrArr = new string[]
			{
				Global.M_ResourceManager.GetString("Global_FirmStatusStrArr0"),
				Global.M_ResourceManager.GetString("Global_FirmStatusStrArr1"),
				Global.M_ResourceManager.GetString("Global_FirmStatusStrArr2")
			};
			Global.BuySellStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_BuySellStrArr1"),
				Global.M_ResourceManager.GetString("Global_BuySellStrArr2")
			};
			Global.SettleBasisStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1"),
				Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2")
			};
			Global.CloseModeStrArr = new string[]
			{
				Global.M_ResourceManager.GetString("Global_CloseModeStrArr0"),
				Global.M_ResourceManager.GetString("Global_CloseModeStrArr1"),
				Global.M_ResourceManager.GetString("Global_CloseModeStrArr2")
			};
			Global.TimeFlagStrArr = new string[]
			{
				Global.M_ResourceManager.GetString("Global_TimeFlagStrArr0"),
				Global.M_ResourceManager.GetString("Global_TimeFlagStrArr1")
			};
			Global.OrderStatusStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr1"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr2"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr3"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr4"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr5"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr6"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr7"),
				Global.M_ResourceManager.GetString("Global_OrderStatusStrArr8")
			};
			Global.TradeTypeStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr1"),
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr2"),
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr3"),
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr4"),
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr5"),
				Global.M_ResourceManager.GetString("Global_TradeTypeStrArr6")
			};
			Global.CBasisStrArr = new string[]
			{
				Global.M_ResourceManager.GetString("Global_CBasisStrArr0"),
				Global.M_ResourceManager.GetString("Global_CBasisStrArr1"),
				Global.M_ResourceManager.GetString("Global_CBasisStrArr2"),
				""
			};
			Global.BillTradeTypeStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_BillTradeTypeStrArr1"),
				Global.M_ResourceManager.GetString("Global_BillTradeTypeStrArr2")
			};
			Global.ConditionTypeStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_ConditionTypeStrArr1"),
				Global.M_ResourceManager.GetString("Global_ConditionTypeStrArr2"),
				Global.M_ResourceManager.GetString("Global_ConditionTypeStrArr3")
			};
			Global.OrderTypeStrArr = new string[]
			{
				"",
				Global.M_ResourceManager.GetString("Global_OrderTypeStrArr1"),
				Global.M_ResourceManager.GetString("Global_OrderTypeStrArr2"),
				Global.M_ResourceManager.GetString("Global_OrderTypeStrArr3"),
				Global.M_ResourceManager.GetString("Global_OrderTypeStrArr4"),
				Global.M_ResourceManager.GetString("Global_OrderTypeStrArr5")
			};
			Global.ConditionOperatorStrArr = new string[]
			{
				"≤",
				"<",
				"=",
				">",
				"≥"
			};
			Global.IsWriteLog = Tools.StrToBool((string)Global.HTConfig["IsWriteLog"], false);
			Global.MaxCount = Tools.StrToInt((string)Global.HTConfig["MaxCount"], 1000);
			Global.PagNum = Tools.StrToInt((string)Global.HTConfig["PageNum"], 300);
			Global.DecimalDigits = Tools.StrToInt((string)Global.HTConfig["DecimalDigits"], 2);
			Global.MyNumberFormatInfo.CurrencyDecimalDigits = Global.DecimalDigits;
			Global.MyNumberFormatInfo.CurrencyNegativePattern = 1;
			if (Global.SetOrderInfo != null)
			{
				Global.SetOrderInfo = null;
			}
		}
		public static void ReSetGlobal()
		{
			Global.SetOrderInfo = null;
			Global.MinLineEvent = null;
			Global.KLineEvent = null;
			Global.LockFormEvent = null;
			Global.LogoutEvent = null;
			Global.ChangeServerEvent = null;
			Global.UpdateStyleEvent = null;
			Global.FloatFormEvent = null;
			Global.StatusInfoFill = null;
			Global.LoadFlag = true;
			Global.orderCount = 0;
			Global.m_order = " Desc ";
			Global.IsWriteLog = false;
			Global.isShowOrder = false;
			Global.HTConfig = null;
			Global.M_ResourceManager = null;
			Global.UserID = string.Empty;
			Global.Password = string.Empty;
			Global.RegisterWord = string.Empty;
			Global.IdleStartTime = DateTime.Now;
			Global.FirmID = string.Empty;
			Global.CustomerID = "00";
			Global.CustomerCount = 0;
			Global.MarketID = string.Empty;
			Global.curTradeDay = string.Empty;
			Global.broadcastFlag = false;
		}
		public static void ReceivedOrderInfo(string commodityID, double buyPrice, double sellPrice)
		{
			if (Global.SetOrderInfo != null)
			{
				Global.SetOrderInfo(commodityID, buyPrice, sellPrice);
			}
		}
		public static void UpdateStyle()
		{
			if (Global.UpdateStyleEvent != null)
			{
				Global.UpdateStyleEvent(null, null);
			}
		}
		public static void TextBoxNumKeypress(KeyPressEventArgs e)
		{
			e.Handled = (e.KeyChar < '0' || e.KeyChar > '9');
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
		}
		public static void AddAscOrDescStr(DataGridView dg, int clickColIndex)
		{
			for (int i = 0; i < dg.Columns.Count; i++)
			{
				if (dg.Columns[i].HeaderText.Contains(" ↑") || dg.Columns[i].HeaderText.Contains(" ↓"))
				{
					dg.Columns[i].FillWeight = dg.Columns[i].FillWeight - 4f;
					dg.Columns[i].HeaderText = dg.Columns[i].HeaderText.Replace(" ↑", "").Replace(" ↓", "");
					break;
				}
			}
			dg.Columns[clickColIndex].FillWeight = dg.Columns[clickColIndex].FillWeight + 4f;
			if (Global.m_order == " ASC ")
			{
				dg.Columns[clickColIndex].HeaderText = dg.Columns[clickColIndex].HeaderText + " ↓";
				return;
			}
			dg.Columns[clickColIndex].HeaderText = dg.Columns[clickColIndex].HeaderText + " ↑";
		}
		public static void BSAlign(DataGridView dg)
		{
			for (int i = 0; i < dg.RowCount; i++)
			{
				if (dg.Rows[i].Cells["B_S"] != null)
				{
					DataGridViewCell dataGridViewCell = dg.Rows[i].Cells["B_S"];
					if (dataGridViewCell.Value != null)
					{
						if (dataGridViewCell.Value.ToString() == Global.BuySellStrArr[1])
						{
							dataGridViewCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
						}
						else
						{
							if (dataGridViewCell.Value.ToString() == Global.BuySellStrArr[2])
							{
								dataGridViewCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
							}
						}
					}
				}
			}
		}
		public static string formatToMoney(double Moneynumber)
		{
			string text = Convert.ToString(Moneynumber);
			return string.Format("{0:N0}", Moneynumber);
		}
		public static string toYear(string year)
		{
			string result = string.Empty;
			if (!year.Equals("") && year.Length == 6)
			{
				result = string.Concat(new string[]
				{
					year.Substring(0, 4),
					"-",
					year.Substring(4, 6),
					"-",
					year.Substring(6, 8)
				});
			}
			else
			{
				if (!year.Equals("") && year.Length > 8)
				{
					result = year.Substring(0, year.Length - 8);
				}
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
			else
			{
				if (!time.Equals("") && time.Length > 8)
				{
					result = time.Substring(time.Length - 8);
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
		public static void displayMinLine(string marketID, string commodityCode)
		{
			if (Global.MarketHT.Count > 1)
			{
				commodityCode = marketID + "_" + commodityCode;
			}
			InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityCode, "MinLineEvent");
			if (Global.MinLineEvent != null)
			{
				Global.MinLineEvent(null, e);
			}
		}
		public static void DisplayKLine(string marketID, string commodityCode)
		{
			if (Global.MarketHT.Count > 1)
			{
				commodityCode = marketID + "_" + commodityCode;
			}
			InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityCode, "KLineEvent");
			if (Global.KLineEvent != null)
			{
				Global.KLineEvent(null, e);
			}
		}
		public static void LockForm(object sender, EventArgs e)
		{
			if (Global.LockFormEvent != null)
			{
				Global.LockFormEvent(sender, e);
			}
		}
		public static void FloatForm(object sender, EventArgs e)
		{
			if (Global.FloatFormEvent != null)
			{
				Global.FloatFormEvent(sender, e);
			}
		}
		public static void Logout(object sender, EventArgs e)
		{
			if (Global.LogoutEvent != null)
			{
				Global.LogoutEvent(sender, e);
			}
		}
		public static Color ColorSet(double value, double PrevClear)
		{
			Color result;
			if (value > PrevClear)
			{
				result = Global.HighColor;
			}
			else
			{
				if (value < PrevClear)
				{
					result = Global.LowColor;
				}
				else
				{
					result = Global.EqualsColor;
				}
			}
			return result;
		}
		public static string ToString(double value)
		{
			string result = string.Empty;
			if (value != 0.0)
			{
				result = value.ToString();
			}
			else
			{
				result = "-";
			}
			return result;
		}
		public static string ToString(double value, int len)
		{
			string result = string.Empty;
			if (value != 0.0)
			{
				result = value.ToString("f" + len);
			}
			else
			{
				result = "-";
			}
			return result;
		}
		public static decimal ToDecimal(string value)
		{
			decimal result;
			if (value.Equals("-"))
			{
				result = 0m;
			}
			else
			{
				try
				{
					result = decimal.Parse(value);
				}
				catch
				{
					result = 0m;
				}
			}
			return result;
		}
		public static void PriceKeyUp(object sender, KeyEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)sender;
			if (numericUpDown.Text.ToString().IndexOf('.') > 6 || (numericUpDown.Text.ToString().IndexOf('.') < 0 && numericUpDown.Text.ToString().Length > 6))
			{
				numericUpDown.Text = numericUpDown.Maximum.ToString();
				e.Handled = true;
			}
			if (numericUpDown.DecimalPlaces == 0)
			{
				if (e.KeyCode == Keys.Decimal)
				{
					numericUpDown.Select(numericUpDown.Value.ToString().Length, 0);
					e.Handled = true;
				}
				if (e.KeyValue % 48 == numericUpDown.Value)
				{
					numericUpDown.Select(numericUpDown.Value.ToString().Length, 0);
				}
			}
		}
		public static void QtyKeyUp(object sender, KeyEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)sender;
			if (numericUpDown.Value.ToString().Length >= 6)
			{
				numericUpDown.Value = numericUpDown.Maximum;
				e.Handled = true;
			}
			if (e.KeyValue % 48 == numericUpDown.Value)
			{
				numericUpDown.Select(numericUpDown.Value.ToString().Length, 0);
			}
		}
		public static void QueryDataLoad()
		{
			OperationManager.GetInstance().queryInitDataOperation.InitData();
			OperationManager.GetInstance().queryUnOrderOperation.QueryUnOrderInfoLoad();
			OperationManager.GetInstance().queryTradeOrderOperation.QueryTradeOrderInfoLoad();
			OperationManager.GetInstance().queryHoldingOperation.QueryHoldingInfoLoad();
		}
	}
}
