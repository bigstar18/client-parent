namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class OrderOperation : QueryOperation
    {
        private double bPrice;
        public bool ConnectHQ;
        private string GoodsName = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
        private string InfoGoods = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoGoods");
        private string InputRightPrice = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPrice");
        public bool IsChangePrice;
        private ListBox lbmain = new ListBox();
        private string MessegeConclude = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeConclude");
        private string MessegeTransfer = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeTransfer");
        private string NoBuyPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoBuyPositions");
        private string NoSellPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoSellPositions");
        public OrderType orderType;
        private string PriceIn = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
        public RefreshHQCallBack refreshHQ;
        public SetButtonOrderEnableCallBack SetButtonOrderEnable;
        public SetCommodityIDCallBack setCommodityID;
        public SetLargestTNCallBack setLargestTN;
        public SetLargestTNCallBack setLargestTN_S;
        private double sPrice;
        public UpdateNumericPriceCallBack UpdateNumericPrice;

        public OrderOperation()
        {
            this.lbmain.Click += new EventHandler(this.lbmain_click);
            this.lbmain.KeyDown += new KeyEventHandler(this.lbmain_keydown);
            this.lbmain.Visible = false;
        }

        private long CalculatLargestTradeNum(CommodityInfo commodityInfo, double price, short B_SType, short O_LType, string CustomerID)
        {
            double sellVHolding = 0.0;
            if (O_LType == 1)
            {
                double num2 = 0.0;
                double num3 = price;
                short marginType = 1;
                double bMargin = 0.0;
                double sMargin = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                short commType = 1;
                double bOpenComm = 0.0;
                double sOpenComm = 0.0;
                double ctrtSize = 0.0;
                FirmInfoResponseVO evo = base.serviceManage.CreateQueryInitData().QueryFundsInfo();
                num2 = evo.RealFund + evo.ImpawnFund;
                marginType = commodityInfo.MarginType;
                bMargin = commodityInfo.BMargin;
                sMargin = commodityInfo.SMargin;
                num7 = commodityInfo.BMargin_g;
                num8 = commodityInfo.SMargin_g;
                commType = commodityInfo.CommType;
                bOpenComm = commodityInfo.BOpenComm;
                sOpenComm = commodityInfo.SOpenComm;
                ctrtSize = commodityInfo.CtrtSize;
                if (num2 <= 0.0)
                {
                    return 0L;
                }
                double num13 = 0.0;
                double num14 = 0.0;
                if (B_SType == 1)
                {
                    if (marginType == 2)
                    {
                        num13 = bMargin - num7;
                    }
                    else if ((num3 > 0.0) && (ctrtSize > 0.0))
                    {
                        num13 = ((num3 * ctrtSize) * (bMargin - num7)) / 100.0;
                    }
                    if (commType == 2)
                    {
                        num14 = bOpenComm;
                    }
                    else
                    {
                        num14 = ((bOpenComm * num3) * ctrtSize) / 100.0;
                    }
                }
                else if (B_SType == 2)
                {
                    if (marginType == 2)
                    {
                        num13 = sMargin - num8;
                    }
                    else if ((num3 > 0.0) && (ctrtSize > 0.0))
                    {
                        num13 = ((num3 * ctrtSize) * (sMargin - num8)) / 100.0;
                    }
                    if (commType == 2)
                    {
                        num14 = sOpenComm;
                    }
                    else
                    {
                        num14 = ((sOpenComm * num3) * ctrtSize) / 100.0;
                    }
                }
                if ((num13 + num14) > 0.0)
                {
                    sellVHolding = num2 / (num13 + num14);
                }
                else
                {
                    sellVHolding = 99999.0;
                }
            }
            else
            {
                List<HoldingInfo> holdingInfoList = TradeDataInfo.holdingInfoList;
                for (int i = 0; i < holdingInfoList.Count; i++)
                {
                    HoldingInfo info = holdingInfoList[i];
                    if (info.CustomerID.Equals(CustomerID) && info.CommodityID.Equals(commodityInfo.CommodityID))
                    {
                        if (B_SType == 1)
                        {
                            sellVHolding = info.SellVHolding;
                        }
                        else
                        {
                            sellVHolding = info.BuyVHolding;
                        }
                        break;
                    }
                }
            }
            return (long)sellVHolding;
        }

        public void ComboxKeyDown(KeyEventArgs e)
        {
            if (this.lbmain.Visible)
            {
                if ((((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)) || ((e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right))) || ((e.KeyCode == Keys.Next) || (e.KeyCode == Keys.PageUp)))
                {
                    this.lbmain_keydown(this.lbmain, e);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    this.lbmain_click(this.lbmain, e);
                    e.Handled = true;
                }
            }
        }

        public void ComboxTextChanged(ComboBox comboCommodity)
        {
            if (this.refreshHQ != null)
            {
                this.refreshHQ(comboCommodity.Text);
            }
            if (comboCommodity.Text == "")
            {
                this.lbmain.Visible = false;
            }
            else if (!comboCommodity.Focused)
            {
                this.lbmain.Visible = false;
            }
            else
            {
                if (!comboCommodity.Parent.Controls.Contains(this.lbmain))
                {
                    this.lbmain.Width = comboCommodity.Width;
                    this.lbmain.Height = 80;
                    this.lbmain.Parent = comboCommodity.Parent;
                    this.lbmain.Top = (comboCommodity.Top + comboCommodity.Height) + 1;
                    this.lbmain.Left = comboCommodity.Left;
                    comboCommodity.Parent.Controls.Add(this.lbmain);
                    this.lbmain.BringToFront();
                }
                int selectionStart = comboCommodity.SelectionStart;
                if (selectionStart <= comboCommodity.Text.Length)
                {
                    List<string> commodityIDList = new List<string>();
                    for (int i = 0; i < comboCommodity.Items.Count; i++)
                    {
                        commodityIDList.Add(comboCommodity.Items[i].ToString());
                    }
                    ArrayList list2 = this.getfilllist(comboCommodity.Text.Substring(0, selectionStart), commodityIDList);
                    this.lbmain.Items.Clear();
                    this.lbmain.Items.AddRange(list2.ToArray());
                    if (this.lbmain.Items.Count > 0)
                    {
                        this.lbmain.SelectedIndex = 0;
                    }
                    if (!this.lbmain.Visible)
                    {
                        this.lbmain.Visible = true;
                    }
                }
            }
        }

        public decimal GetBSPrice(int buysell)
        {
            decimal num = 0M;
            if (!IniData.GetInstance().AutoPrice || this.IsChangePrice)
            {
                return num;
            }
            if (buysell == 0)
            {
                return (decimal)this.sPrice;
            }
            return (decimal)this.bPrice;
        }

        public decimal GetCommoditySpread(string commodityID)
        {
            decimal num = 0M;
            try
            {
                CommodityInfo info = (CommodityInfo)TradeDataInfo.CommodityHashtable[commodityID];
                if (info == null)
                {
                    return num;
                }
                string message = string.Concat(new object[] { this.GoodsName, info.CommodityName, "  ", this.PriceIn, info.SpreadDown, " – ", info.SpreadUp });
                if (Global.StatusInfoFill != null)
                {
                    Global.StatusInfoFill(message, Global.RightColor, true);
                }
                num = decimal.Parse(info.Spread.ToString());
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + "    " + exception.Message);
            }
            return num;
        }

        public int GetCurrentTradeMode(string commodityesID)
        {
            int num = 0;
            if (TradeDataInfo.ht_TradeMode.Contains(commodityesID))
            {
                string str = TradeDataInfo.ht_TradeMode[commodityesID].ToString();
                bool flag = false;
                for (int i = 0; i < TradeDataInfo.FirmBreedInfoList.Count; i++)
                {
                    if (TradeDataInfo.FirmBreedInfoList[i].VarietyID == TradeDataInfo.ht_Variety[commodityesID].ToString())
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
                if (((str == "3") && flag) || ((str == "4") && !flag))
                {
                    return 3;
                }
                if ((!flag && (str == "3")) || ((str == "4") && flag))
                {
                    return 4;
                }
                switch (str)
                {
                    case "1":
                        return 1;

                    case "2":
                        num = 2;
                        break;
                }
            }
            return num;
        }

        public int GetDecimalPlaces(decimal spread)
        {
            int num = 0;
            try
            {
                string[] strArray = spread.ToString().Split(new char[] { '.' });
                if (strArray.Length == 1)
                {
                    return 0;
                }
                if (strArray.Length != 2)
                {
                    return num;
                }
                if (strArray[1].Length == 1)
                {
                    return 1;
                }
                if (strArray[1].Length == 2)
                {
                    num = 2;
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + "    " + exception.Message);
            }
            return num;
        }

        private ArrayList getfilllist(string strvalue, List<string> commodityIDList)
        {
            ArrayList list = new ArrayList();
            int count = commodityIDList.Count;
            int length = strvalue.Length;
            for (int i = 0; i < count; i++)
            {
                string str = commodityIDList[i];
                if ((str.Length >= length) && (string.Compare(str.Substring(0, length), strvalue, true) == 0))
                {
                    list.Add(str);
                }
            }
            return list;
        }

        public long GetLargestTradeNum(string largestInfo)
        {
            long num = 0L;
            if ((largestInfo == null) || (largestInfo.Length <= 0))
            {
                return num;
            }
            int index = largestInfo.IndexOf("：");
            if (index == -1)
            {
                return num;
            }
            try
            {
                return Tools.StrToLong(largestInfo.Substring(index + 1), 0L);
            }
            catch
            {
                return 0L;
            }
        }

        public void GetNumericQtyThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.Qty);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void lbmain_click(object sender, EventArgs e)
        {
            if (this.lbmain.SelectedItems.Count != 0)
            {
                string commodityID = this.lbmain.SelectedItem.ToString();
                if (this.setCommodityID != null)
                {
                    this.setCommodityID(commodityID);
                }
                this.lbmain.Visible = false;
            }
        }

        private void lbmain_keydown(object sender, KeyEventArgs e)
        {
            if (((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left)) || (e.KeyCode == Keys.PageUp))
            {
                if (this.lbmain.SelectedIndex > 0)
                {
                    this.lbmain.SelectedIndex--;
                }
            }
            else if ((((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Right)) || (e.KeyCode == Keys.Next)) && (this.lbmain.SelectedIndex < (this.lbmain.Items.Count - 1)))
            {
                this.lbmain.SelectedIndex++;
            }
        }

        private void NumericQtyInfo(long TradeNum, CommodityInfo commodityInfo, double price, short B_SType, short O_LType)
        {
            string text = string.Empty;
            int colorFlag = 0;
            if (O_LType == 2)
            {
                if (((price > 0.0) && (price <= commodityInfo.SpreadUp)) && (price >= commodityInfo.SpreadDown))
                {
                    if (B_SType == 1)
                    {
                        if (TradeNum == 0L)
                        {
                            colorFlag = 1;
                            text = this.NoSellPositions;
                        }
                        else
                        {
                            colorFlag = 0;
                            text = this.MessegeTransfer + TradeNum;
                        }
                    }
                    else if (TradeNum == 0L)
                    {
                        colorFlag = 1;
                        text = this.NoBuyPositions;
                    }
                    else
                    {
                        colorFlag = 0;
                        text = this.MessegeTransfer + TradeNum;
                    }
                }
                else
                {
                    colorFlag = 1;
                    text = this.InputRightPrice;
                }
            }
            else if (((price > 0.0) && (price <= commodityInfo.SpreadUp)) && (price >= commodityInfo.SpreadDown))
            {
                colorFlag = 0;
                text = Global.M_ResourceManager.GetString("TradeStr_Ckkdll") + "：" + TradeNum;
            }
            else
            {
                colorFlag = 1;
                text = this.InputRightPrice;
            }
            if (this.setLargestTN_S != null)
            {
                if ((this.setLargestTN != null) && (B_SType == 1))
                {
                    this.setLargestTN(text, colorFlag);
                }
                if (B_SType == 2)
                {
                    this.setLargestTN_S(text, colorFlag);
                }
            }
            else if (this.setLargestTN != null)
            {
                this.setLargestTN(text, colorFlag);
            }
            if (commodityInfo.MinQty != 0.0)
            {
                string message = string.Format(this.InfoGoods, commodityInfo.CommodityName, commodityInfo.MinQty);
                if (Global.StatusInfoFill != null)
                {
                    Global.StatusInfoFill(message, Global.RightColor, true);
                }
            }
        }

        private void Qty(object o)
        {
            try
            {
                long tradeNum = 0L;
                Hashtable hashtable = (Hashtable)o;
                CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[(string)hashtable["Commodity"]];
                double price = (double)hashtable["numericPrice"];
                short num3 = (short)hashtable["B_SType"];
                short num4 = (short)hashtable["O_LType"];
                string customerID = (string)hashtable["tbTranc_comboTranc"];
                if (commodityInfo != null)
                {
                    tradeNum = this.CalculatLargestTradeNum(commodityInfo, price, num3, num4, customerID);
                    this.NumericQtyInfo(tradeNum, commodityInfo, price, num3, num4);
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "最大可订立量查询错误：" + exception.Message);
            }
        }

        public void SetListBoxVisible(bool visible)
        {
            this.lbmain.Visible = visible;
        }

        public void ShowMinLine(string commodityID)
        {
            if ((Tools.StrToBool((string)Global.HTConfig["AutoDisplayMinLine"]) && !Global.LoadFlag) && !this.ConnectHQ)
            {
                Global.displayMinLine("", commodityID);
            }
            this.ConnectHQ = false;
        }

        public void UpdatePrice(double _bPrice, double _sPrice)
        {
            this.bPrice = _bPrice;
            this.sPrice = _sPrice;
            if (!this.IsChangePrice && (this.UpdateNumericPrice != null))
            {
                this.UpdateNumericPrice(this.bPrice, this.sPrice);
            }
        }

        public delegate void RefreshHQCallBack(string commodityID);

        public delegate void SetButtonOrderEnableCallBack(bool enable);

        public delegate void SetCommodityIDCallBack(string commodityID);

        public delegate void SetLargestTNCallBack(string text, int colorFlag);

        public delegate void UpdateNumericPriceCallBack(double bPrice, double sPrice);
    }
}
