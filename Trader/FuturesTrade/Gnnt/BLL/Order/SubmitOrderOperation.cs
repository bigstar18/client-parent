namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class SubmitOrderOperation : QueryOperation
    {
        private string GoodsId = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsId");
        private string GoodsNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsNum");
        private string GoodsPrice = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsPrice");
        private string GoodsSaleType = Global.M_ResourceManager.GetString("TradeStr_MainForm_GoodsSaleType");
        private string idMax = "1";
        private string OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OrderInfo");
        public OrderMessageCallBack OrderMessage;
        public SetFocusCallBack SetFocus;
        public SubmitPredelegateCallBack SubmitPredelegateInfo;
        private string SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureOrder");

        public void ButtonOrderComm(FuturesTrade.Gnnt.BLL.Order.SubmitOrderInfo orderInfo, byte btnFlag)
        {
            if (TradeDataInfo.CommodityHashtable.ContainsKey(orderInfo.commodityID))
            {
                CommodityInfo info = (CommodityInfo)TradeDataInfo.CommodityHashtable[orderInfo.commodityID];
                if ((orderInfo.price <= info.SpreadUp) && (orderInfo.price >= info.SpreadDown))
                {
                    if ((Convert.ToInt64((double)(orderInfo.price * 100000.0)) % Convert.ToInt64((decimal)(((decimal)info.Spread) * 100000M))) == 0L)
                    {
                        if (orderInfo.qty > 0)
                        {
                            if ((Convert.ToDouble(orderInfo.qty) % Convert.ToDouble(info.MinQty)) == 0.0)
                            {
                                this.SubmitOrderInfo(orderInfo, btnFlag);
                            }
                            else
                            {
                                string str4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsError");
                                if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
                                {
                                    OperationManager.GetInstance().orderOperation.setLargestTN(string.Concat(new object[] { str4, "【", info.MinQty, "】" }), 1);
                                }
                                if (this.SetFocus != null)
                                {
                                    this.SetFocus(2);
                                }
                            }
                        }
                        else
                        {
                            string text = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
                            if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
                            {
                                OperationManager.GetInstance().orderOperation.setLargestTN(text, 1);
                            }
                            if (this.SetFocus != null)
                            {
                                this.SetFocus(2);
                            }
                        }
                    }
                    else
                    {
                        string str2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorPrice");
                        if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().orderOperation.setLargestTN(string.Concat(new object[] { str2, "【", info.Spread, "】" }), 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(1);
                        }
                    }
                }
                else
                {
                    string format = Global.M_ResourceManager.GetString("TradeStr_MainForm_PriceErrorMessege");
                    if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
                    {
                        OperationManager.GetInstance().orderOperation.setLargestTN(string.Format(format, info.SpreadUp, info.SpreadDown), 1);
                    }
                    if (this.SetFocus != null)
                    {
                        this.SetFocus(1);
                    }
                }
            }
            else
            {
                string str5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NoExistInputGoods");
                if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
                {
                    OperationManager.GetInstance().orderOperation.setLargestTN(str5, 1);
                }
                if (this.SetFocus != null)
                {
                    this.SetFocus(0);
                }
            }
        }

        public void SetMaxID(int maxID)
        {
            this.idMax = maxID.ToString();
        }

        public void SubmitOrder(object _orderRequestVO)
        {
            OrderRequestVO req = (OrderRequestVO)_orderRequestVO;
            ResponseVO evo = base.serviceManage.CreateEntrustOrder().Order(req);
            if ((base.RefreshCurrentTab != null) && (evo.RetCode == 0L))
            {
                base.RefreshCurrentTab(0, true);
            }
            if ((this.OrderMessage != null) && (evo != null))
            {
                this.OrderMessage(evo.RetCode, evo.RetMessage);
            }
        }

        public void SubmitOrderInfo(FuturesTrade.Gnnt.BLL.Order.SubmitOrderInfo orderInfo, byte btnFlag)
        {
            OrderRequestVO orderRequertVO = new OrderRequestVO
            {
                UserID = Global.UserID,
                CustomerID = orderInfo.customerID,
                MarketID = Global.MarketID,
                BuySell = orderInfo.B_SType,
                CommodityID = orderInfo.commodityID,
                Price = orderInfo.price,
                Quantity = orderInfo.qty,
                SettleBasis = orderInfo.O_LType,
                CloseMode = orderInfo.closeMode,
                TimeFlag = orderInfo.timeFlag,
                LPrice = orderInfo.lPrice,
                BillType = orderInfo.billType
            };
            if (IniData.GetInstance().ShowDialog)
            {
                string message = string.Empty;
                if (btnFlag == 0)
                {
                    this.SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureOrder");
                    this.OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_OrderInfo");
                }
                else if (btnFlag == 1)
                {
                    this.SureOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SureEmbedOrder");
                    this.OrderInfo = Global.M_ResourceManager.GetString("TradeStr_MainForm_EmbedOrderInfo");
                }
                object obj2 = message;
                message = string.Concat(new object[] { obj2, this.GoodsId, orderRequertVO.CommodityID, "\r\n", this.GoodsPrice, orderRequertVO.Price, "   ", this.GoodsNum, orderRequertVO.Quantity, "\r\n", this.GoodsSaleType, Global.BuySellStrArr[orderRequertVO.BuySell], "   ", Global.SettleBasisStrArr[orderRequertVO.SettleBasis], "\r\n　　　", this.SureOrder });
                MessageForm form = new MessageForm(this.OrderInfo, message, 0);
                if (orderInfo.B_SType == 1)
                {
                    form.ForeColor = Color.Red;
                }
                else
                {
                    form.ForeColor = Color.Green;
                }
                form.ShowDialog();
                form.Dispose();
                if (form.isOK)
                {
                    if (btnFlag == 0)
                    {
                        this.SubmitOrderThread(orderRequertVO);
                    }
                    else if (btnFlag == 1)
                    {
                        this.SubmitPredelegate(orderRequertVO);
                    }
                }
            }
            else if (btnFlag == 0)
            {
                this.SubmitOrderThread(orderRequertVO);
            }
            else if (btnFlag == 1)
            {
                this.SubmitPredelegate(orderRequertVO);
            }
        }

        public void SubmitOrderThread(object orderRequertVO)
        {
            WaitCallback callBack = new WaitCallback(this.SubmitOrder);
            ThreadPool.QueueUserWorkItem(callBack, orderRequertVO);
        }

        private void SubmitPredelegate(OrderRequestVO orderRequertVO)
        {
            string[] columns = new string[] { "ID", "TransactionsCode", "commodityCode", "B_S", "O_L", "price", "qty", "MarKet", "LPrice", "TodayPosition", "CloseMode", "TimeFlag" };
            string str = string.Empty;
            if (orderRequertVO.SettleBasis == 2)
            {
                if (orderRequertVO.CloseMode == 2)
                {
                    if (orderRequertVO.TimeFlag == 1)
                    {
                        str = Global.TimeFlagStrArr[0];
                    }
                    else
                    {
                        str = Global.TimeFlagStrArr[1];
                    }
                }
                else if (orderRequertVO.CloseMode == 3)
                {
                    str = Global.CloseModeStrArr[2];
                }
                else
                {
                    str = Global.CloseModeStrArr[0];
                }
            }
            string[] columnValue = new string[] { this.idMax, orderRequertVO.CustomerID, orderRequertVO.CommodityID, Global.BuySellStrArr[orderRequertVO.BuySell], Global.SettleBasisStrArr[orderRequertVO.SettleBasis], orderRequertVO.Price.ToString(), orderRequertVO.Quantity.ToString(), orderRequertVO.MarketID, orderRequertVO.LPrice.ToString(), str, orderRequertVO.CloseMode.ToString(), orderRequertVO.TimeFlag.ToString() };
            string text = string.Empty;
            if (this.SubmitPredelegateInfo != null)
            {
                text = this.SubmitPredelegateInfo(columns, columnValue);
            }
            if ((base.RefreshCurrentTab != null) && text.Equals("true"))
            {
                base.RefreshCurrentTab(3, true);
            }
            if (this.SetFocus != null)
            {
                this.SetFocus(0);
            }
            string message = Global.M_ResourceManager.GetString("TradeStr_MainForm_AddEmbeddedOrderSuccess");
            if (text.Equals("true"))
            {
                if (Global.StatusInfoFill != null)
                {
                    Global.StatusInfoFill(message, Global.RightColor, true);
                }
            }
            else if (OperationManager.GetInstance().orderOperation.setLargestTN != null)
            {
                OperationManager.GetInstance().orderOperation.setLargestTN(text, 1);
            }
        }

        public delegate void OrderMessageCallBack(long retCode, string retMessage);

        public delegate void SetFocusCallBack(short flag);

        public delegate string SubmitPredelegateCallBack(string[] Columns, string[] ColumnValue);
    }
}
