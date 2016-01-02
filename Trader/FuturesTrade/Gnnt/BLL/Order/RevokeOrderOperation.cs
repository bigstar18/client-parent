namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using ToolsLibrary.util;
    using TradeInterface.Gnnt.DataVO;

    public class RevokeOrderOperation : QueryOperation
    {
        public refreshOrderCallBack refreshOrder;

        public event ShowMessageCallBack showMessageCallBackEvent;

        private void RevokeOrderInfo(object o)
        {
            if (o != null)
            {
                bool flag = false;
                List<string> list = (List<string>)o;
                List<ResponseVO> list2 = new List<ResponseVO>();
                WithDrawOrderRequestVO req = new WithDrawOrderRequestVO
                {
                    UserID = Global.UserID
                };
                for (int i = 0; i < list.Count; i++)
                {
                    req.OrderNo = Tools.StrToLong(list[i], 0L);
                    ResponseVO item = base.serviceManage.CreateRevokeOrder().WithDrawOrder(req);
                    if ((item != null) && (item.RetCode == 0L))
                    {
                        flag = true;
                    }
                    list2.Add(item);
                }
                if (flag && (this.refreshOrder != null))
                {
                    this.refreshOrder(0, true);
                }
            }
        }

        public void RevokeOrderThread(List<string> orderNoList)
        {
            WaitCallback callBack = new WaitCallback(this.RevokeOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, orderNoList);
        }

        public delegate void refreshOrderCallBack(int mark, bool flag);

        public delegate void ShowMessageCallBack();
    }
}
