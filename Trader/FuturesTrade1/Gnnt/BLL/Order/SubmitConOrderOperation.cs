namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using TradeInterface.Gnnt.DataVO;

    public class SubmitConOrderOperation : QueryOperation
    {
        private string CommodityID = Global.M_ResourceManager.GetString("TradeStr_ConditionCommodityID");
        private string ConditionPrice = Global.M_ResourceManager.GetString("TradeStr_ConditionPrice");
        private string ConditionSign = Global.M_ResourceManager.GetString("TradeStr_ConditionSign");
        private string ConditionType = Global.M_ResourceManager.GetString("TradeStr_ConditionType");
        public ConOrderMessageCallBack ConOrderMessage;
        private string MatureTime = Global.M_ResourceManager.GetString("TradeStr_MatureTime");
        private string OrderInfo = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_TrustMessage");
        public SetFocusCallBack SetFocus;
        private string SureOrder = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_SureSubmitTrust");
        private string TrustNum = Global.M_ResourceManager.GetString("TradeStr_TrustNum");
        private string TrustPrice = Global.M_ResourceManager.GetString("TradeStr_TrustPrice");
        private string TrustType = Global.M_ResourceManager.GetString("TradeStr_TrustType");
        private string TrustTypeId = Global.M_ResourceManager.GetString("TradeStr_TrustTypeId");

        public void ButtonConOrderComm(FuturesTrade.Gnnt.BLL.Order.SubmitConOrderInfo orderInfo)
        {
            if (orderInfo != null)
            {
                if (!TradeDataInfo.CommodityHashtable.ContainsKey(orderInfo.commodityID))
                {
                    string text = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_NoExistGoodsId");
                    if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                    {
                        OperationManager.GetInstance().conOrderOperation.setLargestTN(text, 1);
                    }
                    if (this.SetFocus != null)
                    {
                        this.SetFocus(0);
                    }
                }
                else
                {
                    CommodityInfo info = (CommodityInfo)TradeDataInfo.CommodityHashtable[orderInfo.commodityID];
                    if (orderInfo.price <= 0.0)
                    {
                        string format = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_InputTrustPrice");
                        if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Format(format, info.SpreadUp, info.SpreadDown), 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(1);
                        }
                    }
                    else if (orderInfo.conprice <= 0.0)
                    {
                        string str3 = Global.M_ResourceManager.GetString("TradeStr_ConditionOrder_InputConditionPrice");
                        if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Format(str3, info.SpreadUp, info.SpreadDown), 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(1);
                        }
                    }
                    else if ((Convert.ToInt64((double)(orderInfo.price * 100000.0)) % Convert.ToInt64((decimal)(((decimal)info.Spread) * 100000M))) != 0L)
                    {
                        string str4 = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorPrice");
                        if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Concat(new object[] { str4, "【", info.Spread, "】" }), 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(1);
                        }
                    }
                    else if (orderInfo.qty <= 0)
                    {
                        string str5 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
                        if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().conOrderOperation.setLargestTN(str5, 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(2);
                        }
                    }
                    else if ((Convert.ToDouble(orderInfo.qty) % Convert.ToDouble(info.MinQty)) != 0.0)
                    {
                        string str6 = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsError");
                        if (OperationManager.GetInstance().conOrderOperation.setLargestTN != null)
                        {
                            OperationManager.GetInstance().conOrderOperation.setLargestTN(string.Concat(new object[] { str6, "【", info.MinQty, "】" }), 1);
                        }
                        if (this.SetFocus != null)
                        {
                            this.SetFocus(2);
                        }
                    }
                    else
                    {
                        this.SubmitConOrderInfo(orderInfo);
                    }
                }
            }
        }

        public void SubmitConOrder(object _orderRequestVO)
        {
            ConditionOrderRequestVO req = (ConditionOrderRequestVO)_orderRequestVO;
            ConditionOrderResponseVO evo = base.serviceManage.CreateConditionOrder().SetConditionOrder(req);
            if ((base.RefreshCurrentTab != null) && (evo.RetCode == 0L))
            {
                base.RefreshCurrentTab(2, true);
            }
            if ((this.ConOrderMessage != null) && (evo != null))
            {
                this.ConOrderMessage(evo.RetCode, evo.RetMessage);
            }
        }

        private void SubmitConOrderInfo(FuturesTrade.Gnnt.BLL.Order.SubmitConOrderInfo orderInfo)
        {
            ConditionOrderRequestVO orderRequertVO = new ConditionOrderRequestVO
            {
                UserID = Global.UserID,
                FirmID = Global.FirmID,
                Concommodity = orderInfo.commodityID,
                CommodityID = orderInfo.commodityID,
                BuySell = orderInfo.B_SType,
                SettleBasis = orderInfo.O_LType,
                Price = Convert.ToDouble(orderInfo.price),
                Quantity = Convert.ToInt32(orderInfo.qty),
                TraderID = Global.UserID,
                ConPrice = Convert.ToDouble(orderInfo.conprice),
                ConExpire = orderInfo.datetime,
                Conoperator = orderInfo.conoperator,
                Contype = orderInfo.contype
            };
            if (IniData.GetInstance().ShowDialog)
            {
                object obj2 = string.Empty;
                if (MessageBox.Show(string.Concat(new object[] {
                    obj2, this.CommodityID, ":", orderRequertVO.Concommodity, "\r\n", this.ConditionType, ":", Global.ConditionTypeStrArr[orderRequertVO.Contype], "\r\n", this.ConditionSign, ":", Global.ConditionOperatorStrArr[orderRequertVO.Conoperator + 2], "\r\n", this.ConditionPrice, ":", orderRequertVO.ConPrice,
                    "\r\n", this.MatureTime, ":", orderRequertVO.ConExpire, "\r\n\n********************\r\n", this.TrustTypeId, ":", orderRequertVO.CommodityID, "\r\n", this.TrustType, ":", Global.BuySellStrArr[orderRequertVO.BuySell], Global.SettleBasisStrArr[orderRequertVO.SettleBasis], "\r\n", this.TrustPrice, ":",
                    orderRequertVO.Price, "\r\n", this.TrustNum, ":", orderRequertVO.Quantity, "\r\n********************\r\n", this.SureOrder
                 }), this.OrderInfo, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.SubmitConOrderThread(orderRequertVO);
                }
            }
            else
            {
                this.SubmitConOrderThread(orderRequertVO);
            }
        }

        public void SubmitConOrderThread(object orderRequertVO)
        {
            WaitCallback callBack = new WaitCallback(this.SubmitConOrder);
            ThreadPool.QueueUserWorkItem(callBack, orderRequertVO);
        }

        public delegate void ConOrderMessageCallBack(long retCode, string retMessage);

        public delegate void SetFocusCallBack(short flag);
    }
}
