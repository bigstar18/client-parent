using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Threading;
using ToolsLibrary.util;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class RevokeOrderOperation : QueryOperation
	{
		public delegate void refreshOrderCallBack(int mark, bool flag);
		public delegate void ShowMessageCallBack();
		public RevokeOrderOperation.refreshOrderCallBack refreshOrder;
		public event RevokeOrderOperation.ShowMessageCallBack showMessageCallBackEvent;
		public void RevokeOrderThread(List<string> orderNoList)
		{
			WaitCallback callBack = new WaitCallback(this.RevokeOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, orderNoList);
		}
		private void RevokeOrderInfo(object o)
		{
			if (o == null)
			{
				return;
			}
			bool flag = false;
			List<string> list = (List<string>)o;
			List<ResponseVO> list2 = new List<ResponseVO>();
			WithDrawOrderRequestVO withDrawOrderRequestVO = new WithDrawOrderRequestVO();
			withDrawOrderRequestVO.UserID = Global.UserID;
			for (int i = 0; i < list.Count; i++)
			{
				withDrawOrderRequestVO.OrderNo = Tools.StrToLong(list[i], 0L);
				ResponseVO responseVO = this.serviceManage.CreateRevokeOrder().WithDrawOrder(withDrawOrderRequestVO);
				if (responseVO != null && responseVO.RetCode == 0L)
				{
					flag = true;
				}
				list2.Add(responseVO);
			}
			if (flag && this.refreshOrder != null)
			{
				this.refreshOrder(0, true);
			}
		}
	}
}
