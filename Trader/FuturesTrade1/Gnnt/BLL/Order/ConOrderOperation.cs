namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.DBService.ServiceManager;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using ToolsLibrary.util;
    using TradeInterface.Gnnt.DataVO;

    public class ConOrderOperation : QueryOperation
    {
        private double bPrice;
        private string GoodsName = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsName");
        private string InfoGoods = Global.M_ResourceManager.GetString("TradeStr_MainForm_InfoGoods");
        private ListBox lbmain = new ListBox();
        private string MessegeConclude = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeConclude");
        private string MessegeTransfer = Global.M_ResourceManager.GetString("TradeStr_MainForm_MessegeTransfer");
        private string NoBuyPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoBuyPositions");
        private string NoSellPositions = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoSellPositions");
        private string PriceIn = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceIn");
        public ServiceManage serviceManage = ServiceManage.GetInstance();
        public SetCommodityIDCallBack setCommodityID;
        public SetLargestTNCallBack setLargestTN;
        private double sPrice;

        public ConOrderOperation()
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
                FirmInfoResponseVO evo = this.serviceManage.CreateQueryInitData().QueryFundsInfo();
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
                HoldingQueryRequestVO req = new HoldingQueryRequestVO
                {
                    UserID = Global.UserID,
                    CommodityID = commodityInfo.CommodityID
                };
                HoldingQueryResponseVO evo2 = Global.TradeLibrary.HoldingQuery(req);
                if (evo2.RetCode == 0L)
                {
                    List<HoldingInfo> holdingInfoList = evo2.HoldingInfoList;
                    for (int i = 0; i < holdingInfoList.Count; i++)
                    {
                        HoldingInfo info = holdingInfoList[i];
                        if (info.CustomerID.Equals(CustomerID))
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

        public decimal GetBSPrice(int buysell)
        {
            decimal num = 0M;
            if (!IniData.GetInstance().AutoPrice)
            {
                return num;
            }
            if (buysell == 0)
            {
                return (decimal)this.sPrice;
            }
            return (decimal)this.bPrice;
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
            }
            if (this.setLargestTN != null)
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

        public delegate void SetCommodityIDCallBack(string commodityID);

        public delegate void SetLargestTNCallBack(string text, int colorFlag);
    }
}
