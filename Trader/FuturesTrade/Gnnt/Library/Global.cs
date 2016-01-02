namespace FuturesTrade.Gnnt.Library
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ToolsLibrary.util;
    using TradeInterface.Gnnt.Interface;

    public class Global
    {
        public static string[] BillTradeTypeStrArr;
        public static bool broadcastFlag = false;
        public static string[] BuySellStrArr;
        public static string[] CBasisStrArr;
        public static string[] CloseModeStrArr;
        public static string CommCodeXml = "MEBS_CommodityCode.xml";
        public static string[] ConditionOperatorStrArr;
        public static string[] ConditionSignStrArr = new string[] { "<=", "<", "=", ">", ">=" };
        public static string[] ConditionTypeStrArr;
        public static string ConfigPath = string.Empty;
        public static string curTradeDay = string.Empty;
        public static int CustomerCount = 0;
        public static string CustomerID = "00";
        public static int DecimalDigits = 2;
        public static Color EqualsColor = Color.Black;
        public static Color ErrorColor = Color.Red;
        public static string FirmID = string.Empty;
        public static string[] FirmStatusStrArr;
        public static string formatMoney = "C";
        public static Color HighColor = Color.Red;
        public static Hashtable HTConfig = null;
        public static DateTime IdleStartTime = DateTime.Now;
        public static bool isShowOrder = false;
        public static bool IsWriteLog = false;
        public static Color LightColor = Color.LimeGreen;
        public static bool LoadFlag = true;
        public static Color LowColor = Color.Green;
        public static string m_order = " Desc ";
        public static ResourceManager M_ResourceManager = null;
        public static Hashtable MarketHT;
        public static string MarketID = string.Empty;
        public static int MaxCount = 0x3e8;
        public static string MyCommodityXml = "MyCommodity.xml";
        public static NumberFormatInfo MyNumberFormatInfo = new CultureInfo("zh-CN", false).NumberFormat;
        public static int orderCount = 0;
        public static string[] OrderStatusStrArr;
        public static string[] OrderTypeStrArr;
        public static int PagNum = 300;
        public static string Password = string.Empty;
        public static string PreDelegateXml = "MEBS_PreDelegate.xml";
        public static string RegisterWord = string.Empty;
        public static Color RightColor = Color.Black;
        public static int screenHight = 760;
        public static int screenWidth = 0x400;
        public static DateTime ServerTime;
        public static SetCommoditySelectIndexCallBack SetCommoditySelectIndex;
        public static SetDoubleClickOrderInfoCallBack SetDoubleClickOrderInfo;
        public static string[] SettleBasisStrArr;
        public static StatusInfoFillCallBack StatusInfoFill;
        public static string[] TimeFlagStrArr;
        public static ITradeLibrary TradeLibrary;
        public static string[] TradeTypeStrArr;
        public static string TrancCodeXml = "MEBS_TransactionsCode.xml";
        public static string UserID = string.Empty;

        public static event EventHandler ChangeServerEvent;

        public static event EventHandler FloatFormEvent;

        public static event InterFace.CommodityInfoEventHander KLineEvent;

        public static event EventHandler LockFormEvent;

        public static event EventHandler LogoutEvent;

        public static event InterFace.CommodityInfoEventHander MinLineEvent;

        public static event SetOrderInfoCallBack SetOrderInfo;

        public static event EventHandler UpdateStyleEvent;

        public static void AddAscOrDescStr(DataGridView dg, int clickColIndex)
        {
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (dg.Columns[i].HeaderText.Contains(" ↑") || dg.Columns[i].HeaderText.Contains(" ↓"))
                {
                    dg.Columns[i].FillWeight -= 4f;
                    dg.Columns[i].HeaderText = dg.Columns[i].HeaderText.Replace(" ↑", "").Replace(" ↓", "");
                    break;
                }
            }
            dg.Columns[clickColIndex].FillWeight += 4f;
            if (m_order == " ASC ")
            {
                dg.Columns[clickColIndex].HeaderText = dg.Columns[clickColIndex].HeaderText + " ↓";
            }
            else
            {
                dg.Columns[clickColIndex].HeaderText = dg.Columns[clickColIndex].HeaderText + " ↑";
            }
        }

        public static void BSAlign(DataGridView dg)
        {
            for (int i = 0; i < dg.RowCount; i++)
            {
                if (dg.Rows[i].Cells["B_S"] != null)
                {
                    DataGridViewCell cell = dg.Rows[i].Cells["B_S"];
                    if (cell.Value != null)
                    {
                        if (cell.Value.ToString() == BuySellStrArr[1])
                        {
                            cell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        else if (cell.Value.ToString() == BuySellStrArr[2])
                        {
                            cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                }
            }
        }

        public static Color ColorSet(double value, double PrevClear)
        {
            if (value > PrevClear)
            {
                return HighColor;
            }
            if (value < PrevClear)
            {
                return LowColor;
            }
            return EqualsColor;
        }

        public static void DisplayKLine(string marketID, string commodityCode)
        {
            if (MarketHT.Count > 1)
            {
                commodityCode = marketID + "_" + commodityCode;
            }
            InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityCode, "KLineEvent");
            if (KLineEvent != null)
            {
                KLineEvent(null, e);
            }
        }

        public static void displayMinLine(string marketID, string commodityCode)
        {
            if (MarketHT.Count > 1)
            {
                commodityCode = marketID + "_" + commodityCode;
            }
            InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityCode, "MinLineEvent");
            if (MinLineEvent != null)
            {
                MinLineEvent(null, e);
            }
        }

        public static void FloatForm(object sender, EventArgs e)
        {
            if (FloatFormEvent != null)
            {
                FloatFormEvent(sender, e);
            }
        }

        public static string formatToMoney(double Moneynumber)
        {
            string str = Convert.ToString(Moneynumber);
            return string.Format("{0:N0}", Moneynumber);
        }

        public static void GetArrItems()
        {
            FirmStatusStrArr = new string[] { M_ResourceManager.GetString("Global_FirmStatusStrArr0"), M_ResourceManager.GetString("Global_FirmStatusStrArr1"), M_ResourceManager.GetString("Global_FirmStatusStrArr2") };
            BuySellStrArr = new string[] { "", M_ResourceManager.GetString("Global_BuySellStrArr1"), M_ResourceManager.GetString("Global_BuySellStrArr2") };
            SettleBasisStrArr = new string[] { "", M_ResourceManager.GetString("Global_SettleBasisStrArr1"), M_ResourceManager.GetString("Global_SettleBasisStrArr2") };
            CloseModeStrArr = new string[] { M_ResourceManager.GetString("Global_CloseModeStrArr0"), M_ResourceManager.GetString("Global_CloseModeStrArr1"), M_ResourceManager.GetString("Global_CloseModeStrArr2") };
            TimeFlagStrArr = new string[] { M_ResourceManager.GetString("Global_TimeFlagStrArr0"), M_ResourceManager.GetString("Global_TimeFlagStrArr1") };
            OrderStatusStrArr = new string[] { "", M_ResourceManager.GetString("Global_OrderStatusStrArr1"), M_ResourceManager.GetString("Global_OrderStatusStrArr2"), M_ResourceManager.GetString("Global_OrderStatusStrArr3"), M_ResourceManager.GetString("Global_OrderStatusStrArr4"), M_ResourceManager.GetString("Global_OrderStatusStrArr5"), M_ResourceManager.GetString("Global_OrderStatusStrArr6"), M_ResourceManager.GetString("Global_OrderStatusStrArr7"), M_ResourceManager.GetString("Global_OrderStatusStrArr8") };
            TradeTypeStrArr = new string[] { "", M_ResourceManager.GetString("Global_TradeTypeStrArr1"), M_ResourceManager.GetString("Global_TradeTypeStrArr2"), M_ResourceManager.GetString("Global_TradeTypeStrArr3"), M_ResourceManager.GetString("Global_TradeTypeStrArr4"), M_ResourceManager.GetString("Global_TradeTypeStrArr5"), M_ResourceManager.GetString("Global_TradeTypeStrArr6") };
            CBasisStrArr = new string[] { M_ResourceManager.GetString("Global_CBasisStrArr0"), M_ResourceManager.GetString("Global_CBasisStrArr1"), M_ResourceManager.GetString("Global_CBasisStrArr2"), "" };
            BillTradeTypeStrArr = new string[] { "", M_ResourceManager.GetString("Global_BillTradeTypeStrArr1"), M_ResourceManager.GetString("Global_BillTradeTypeStrArr2") };
            ConditionTypeStrArr = new string[] { "", M_ResourceManager.GetString("Global_ConditionTypeStrArr1"), M_ResourceManager.GetString("Global_ConditionTypeStrArr2"), M_ResourceManager.GetString("Global_ConditionTypeStrArr3") };
            OrderTypeStrArr = new string[] { "", M_ResourceManager.GetString("Global_OrderTypeStrArr1"), M_ResourceManager.GetString("Global_OrderTypeStrArr2"), M_ResourceManager.GetString("Global_OrderTypeStrArr3"), M_ResourceManager.GetString("Global_OrderTypeStrArr4"), M_ResourceManager.GetString("Global_OrderTypeStrArr5") };
            ConditionOperatorStrArr = new string[] { "≤", "<", "=", ">", "≥" };
            IsWriteLog = Tools.StrToBool((string)HTConfig["IsWriteLog"], false);
            MaxCount = Tools.StrToInt((string)HTConfig["MaxCount"], 0x3e8);
            PagNum = Tools.StrToInt((string)HTConfig["PageNum"], 300);
            DecimalDigits = Tools.StrToInt((string)HTConfig["DecimalDigits"], 2);
            MyNumberFormatInfo.CurrencyDecimalDigits = DecimalDigits;
            MyNumberFormatInfo.CurrencyNegativePattern = 1;
            if (SetOrderInfo != null)
            {
                SetOrderInfo = null;
            }
        }

        public static string GetFullPath(string strPath)
        {
            if (strPath.IndexOf(":") > 0)
            {
                return strPath;
            }
            return (Environment.CurrentDirectory + "/" + strPath);
        }

        public static void LockForm(object sender, EventArgs e)
        {
            if (LockFormEvent != null)
            {
                LockFormEvent(sender, e);
            }
        }

        public static void Logout(object sender, EventArgs e)
        {
            if (LogoutEvent != null)
            {
                LogoutEvent(sender, e);
            }
        }

        public static void PriceKeyUp(object sender, KeyEventArgs e)
        {
            NumericUpDown down = (NumericUpDown)sender;
            if ((down.Text.ToString().IndexOf('.') > 6) || ((down.Text.ToString().IndexOf('.') < 0) && (down.Text.ToString().Length > 6)))
            {
                down.Text = down.Maximum.ToString();
                e.Handled = true;
            }
            if (down.DecimalPlaces == 0)
            {
                if (e.KeyCode == Keys.Decimal)
                {
                    down.Select(down.Value.ToString().Length, 0);
                    e.Handled = true;
                }
                if ((e.KeyValue % 0x30) == down.Value)
                {
                    down.Select(down.Value.ToString().Length, 0);
                }
            }
        }

        public static void QtyKeyUp(object sender, KeyEventArgs e)
        {
            NumericUpDown down = (NumericUpDown)sender;
            if (down.Value.ToString().Length >= 6)
            {
                down.Value = down.Maximum;
                e.Handled = true;
            }
            if ((e.KeyValue % 0x30) == down.Value)
            {
                down.Select(down.Value.ToString().Length, 0);
            }
        }

        public static void QueryDataLoad()
        {
            OperationManager.GetInstance().queryInitDataOperation.InitData();
            OperationManager.GetInstance().queryUnOrderOperation.QueryUnOrderInfoLoad();
            OperationManager.GetInstance().queryTradeOrderOperation.QueryTradeOrderInfoLoad();
        }

        public static void ReceivedOrderInfo(string commodityID, double buyPrice, double sellPrice)
        {
            if (SetOrderInfo != null)
            {
                SetOrderInfo(commodityID, buyPrice, sellPrice);
            }
        }

        public static void ReSetGlobal()
        {
            SetOrderInfo = null;
            MinLineEvent = null;
            KLineEvent = null;
            LockFormEvent = null;
            LogoutEvent = null;
            ChangeServerEvent = null;
            UpdateStyleEvent = null;
            FloatFormEvent = null;
            StatusInfoFill = null;
            LoadFlag = true;
            orderCount = 0;
            m_order = " Desc ";
            IsWriteLog = false;
            isShowOrder = false;
            HTConfig = null;
            M_ResourceManager = null;
            UserID = string.Empty;
            Password = string.Empty;
            RegisterWord = string.Empty;
            IdleStartTime = DateTime.Now;
            FirmID = string.Empty;
            CustomerID = "00";
            CustomerCount = 0;
            MarketID = string.Empty;
            curTradeDay = string.Empty;
            broadcastFlag = false;
        }

        public static short StrToShort(string[] strArr, string str)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                if (str.Equals(strArr[i]))
                {
                    return (short)i;
                }
            }
            return 0;
        }

        public static void TextBoxNumKeypress(KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar < '0') || (e.KeyChar > '9');
            if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        public static decimal ToDecimal(string value)
        {
            if (value.Equals("-"))
            {
                return 0M;
            }
            try
            {
                return decimal.Parse(value);
            }
            catch
            {
                return 0M;
            }
        }

        public static string ToString(double value)
        {
            if (value != 0.0)
            {
                return value.ToString();
            }
            return "-";
        }

        public static string ToString(double value, int len)
        {
            if (value != 0.0)
            {
                return value.ToString("f" + len);
            }
            return "-";
        }

        public static string toTime(string time)
        {
            string str = string.Empty;
            if (!time.Equals("") && (time.Length == 6))
            {
                return (time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2));
            }
            if (!time.Equals("") && (time.Length > 8))
            {
                str = time.Substring(time.Length - 8);
            }
            return str;
        }

        public static string toYear(string year)
        {
            string str = string.Empty;
            if (!year.Equals("") && (year.Length == 6))
            {
                return (year.Substring(0, 4) + "-" + year.Substring(4, 6) + "-" + year.Substring(6, 8));
            }
            if (!year.Equals("") && (year.Length > 8))
            {
                str = year.Substring(0, year.Length - 8);
            }
            return str;
        }

        public static void UpdateStyle()
        {
            if (UpdateStyleEvent != null)
            {
                UpdateStyleEvent(null, null);
            }
        }

        public delegate bool SetCommoditySelectIndexCallBack(string marketID, string commodityID);

        public delegate void SetDoubleClickOrderInfoCallBack(double price, double Lprice, int qty, short buysell, short ordertype);

        public delegate void SetOrderInfoCallBack(string commodityID, double buyPrice, double sellPrice);

        public delegate void StatusInfoFillCallBack(string message, Color color, bool display);
    }
}
