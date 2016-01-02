using FuturesTrade.Gnnt.DBService.ServiceManager;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Threading;
using ToolsLibrary.util;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class RevokeConOrderOperation
	{
		public delegate void refreshConOrderCallBack(int mark, bool flag);
		public delegate void ShowMessageCallBack();
		public RevokeConOrderOperation.refreshConOrderCallBack refreshConOrder;
		public event RevokeConOrderOperation.ShowMessageCallBack showMessageCallBackEvent;
		public void RevokeConOrderThread(List<string> orderNoList)
		{
			WaitCallback callBack = new WaitCallback(this.RevokeConOrderInfo);
			ThreadPool.QueueUserWorkItem(callBack, orderNoList);
		}
		private void RevokeConOrderInfo(object o)
		{
			if (o == null)
			{
				return;
			}
			bool flag = false;
			List<string> list = (List<string>)o;
			List<ConditionRevokeResponseVO> list2 = new List<ConditionRevokeResponseVO>();
			ConditionRevokeRequestVO conditionRevokeRequestVO = new ConditionRevokeRequestVO();
			conditionRevokeRequestVO.UserID = Global.UserID;
			for (int i = 0; i < list.Count; i++)
			{
				conditionRevokeRequestVO.ConditionOrderNo = Tools.StrToLong(list[i], 0L);
				ConditionRevokeResponseVO conditionRevokeResponseVO = ServiceManage.GetInstance().CreateConditionOrder().WithDrawConditionOrder(conditionRevokeRequestVO);
				if (conditionRevokeResponseVO != null && conditionRevokeResponseVO.RetCode == 0L)
				{
					flag = true;
				}
				list2.Add(conditionRevokeResponseVO);
			}
			if (flag && this.refreshConOrder != null)
			{
				this.refreshConOrder(2, true);
			}
		}
	}
}
