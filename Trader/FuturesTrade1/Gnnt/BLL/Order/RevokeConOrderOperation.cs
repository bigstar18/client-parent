namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.DBService.ServiceManager;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using ToolsLibrary.util;
    using TradeInterface.Gnnt.DataVO;

    public class RevokeConOrderOperation
    {
        public refreshConOrderCallBack refreshConOrder;

        public event ShowMessageCallBack showMessageCallBackEvent;

        private void RevokeConOrderInfo(object o)
        {
            if (o != null)
            {
                bool flag = false;
                List<string> list = (List<string>)o;
                List<ConditionRevokeResponseVO> list2 = new List<ConditionRevokeResponseVO>();
                ConditionRevokeRequestVO req = new ConditionRevokeRequestVO
                {
                    UserID = Global.UserID
                };
                for (int i = 0; i < list.Count; i++)
                {
                    req.ConditionOrderNo = Tools.StrToLong(list[i], 0L);
                    ConditionRevokeResponseVO item = ServiceManage.GetInstance().CreateConditionOrder().WithDrawConditionOrder(req);
                    if ((item != null) && (item.RetCode == 0L))
                    {
                        flag = true;
                    }
                    list2.Add(item);
                }
                if (flag && (this.refreshConOrder != null))
                {
                    this.refreshConOrder(2, true);
                }
            }
        }

        public void RevokeConOrderThread(List<string> orderNoList)
        {
            WaitCallback callBack = new WaitCallback(this.RevokeConOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, orderNoList);
        }

        public delegate void refreshConOrderCallBack(int mark, bool flag);

        public delegate void ShowMessageCallBack();
    }
}
