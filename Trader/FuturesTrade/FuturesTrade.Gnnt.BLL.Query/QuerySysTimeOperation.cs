using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.BLL.Query
{
	public class QuerySysTimeOperation : QueryOperation
	{
		public delegate void SetConnectStatusCallBack(int connectionStatus);
		public delegate void ShowNotifierCallBack(string tradeInfo);
		private bool refreshTimeFlag = true;
		private int connectStatus;
		private double QuerySysTimeRefresh = 1300.0;
		private int HQTimerRefreshCount;
		public QuerySysTimeOperation.SetConnectStatusCallBack SetConnectStatus;
		public QuerySysTimeOperation.ShowNotifierCallBack ShowNotifier;
		public event EventHandler ChangeServerEvent;
		public void RefreshSysTime(int interval)
		{
			this.HQTimerRefreshCount++;
			if (this.QuerySysTimeRefresh / (double)interval <= (double)this.HQTimerRefreshCount)
			{
				this.HQTimerRefreshCount = 0;
				this.QuerySysTimeThread();
			}
		}
		public void SetSysTimeRefreshTime(bool type)
		{
			if (!type)
			{
				this.QuerySysTimeRefresh = 20000.0;
				return;
			}
			this.QuerySysTimeRefresh = Tools.StrToDouble((string)Global.HTConfig["HQRefreshTime"], 1.5) * 1000.0;
		}
		public void QuerySysTimeThread()
		{
			WaitCallback callBack = new WaitCallback(this.QuerySysTime);
			ThreadPool.QueueUserWorkItem(callBack, null);
		}
		private void QuerySysTime(object obj)
		{
			if (!this.refreshTimeFlag)
			{
				return;
			}
			this.refreshTimeFlag = false;
			SysTimeQueryRequestVO sysTimeQueryRequestVO = new SysTimeQueryRequestVO();
			sysTimeQueryRequestVO.UserID = Global.UserID;
			SysTimeQueryResponseVO sysTime = Global.TradeLibrary.GetSysTime(sysTimeQueryRequestVO);
			this.CheckConnectStatus(sysTime.RetCode, sysTime.RetMessage);
			if (sysTime.RetCode == 0L)
			{
				if (!sysTime.CurrentDate.Equals("") && !sysTime.CurrentTime.Equals(""))
				{
					this.CheckChangeTradeDay(sysTime.TradeDay);
					Global.curTradeDay = sysTime.TradeDay;
					this.SetCurrentDateTime(sysTime.CurrentDate, sysTime.CurrentTime);
				}
				if (sysTime.RefreshMark != 0 && this.RefreshCurrentTab != null)
				{
					this.RefreshCurrentTab(2, true);
				}
				this.DisplayTradeMessage(sysTime.NewTrade, sysTime.TradeMessageList);
			}
			this.refreshTimeFlag = true;
		}
		private void CheckConnectStatus(long retCode, string retMessage)
		{
			bool flag = false;
			if (retCode == 0L)
			{
				if (this.connectStatus != 0)
				{
					flag = true;
				}
				this.connectStatus = 0;
			}
			else
			{
				Logger.wirte(MsgType.Error, "主窗体获取服务器系统时间错误：" + retMessage);
				if (this.connectStatus == 0)
				{
					flag = true;
					if (retMessage != null && retMessage.Length > 0)
					{
						this.connectStatus = 1;
					}
					else
					{
						this.connectStatus = 2;
						if (this.ChangeServerEvent != null)
						{
							this.ChangeServerEvent(null, null);
						}
					}
				}
			}
			if (this.SetConnectStatus != null && flag)
			{
				this.SetConnectStatus(this.connectStatus);
			}
		}
		private void CheckChangeTradeDay(string tradeDay)
		{
			if (Global.curTradeDay != null && Global.curTradeDay.Length > 0)
			{
				Global.curTradeDay.Equals(tradeDay);
			}
		}
		private void SetCurrentDateTime(string currentDate, string currentTime)
		{
			DateTime serverTime = default(DateTime);
			try
			{
				serverTime = DateTime.Parse(currentDate + " " + currentTime);
			}
			catch
			{
				serverTime = DateTime.Now;
			}
			Global.ServerTime = serverTime;
		}
		private void displayBroadcast(object obj)
		{
			List<Broadcast> list = (List<Broadcast>)obj;
			if (list != null)
			{
				int arg_12_0 = list.Count;
			}
		}
		private void DisplayTradeMessage(short newTrade, List<TradeMessage> tradeMessageList)
		{
			if (newTrade == 1)
			{
				string text = string.Empty;
				if (tradeMessageList != null && tradeMessageList.Count > 0)
				{
					string @string = Global.M_ResourceManager.GetString("TradeStr_MainnForm_TradeMessege1");
					string string2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_TradeMessege2");
					for (int i = 0; i < tradeMessageList.Count; i++)
					{
						if (tradeMessageList[i].CommodityID == "")
						{
							text += string.Format("{0}{1}{2}{3}\n", new object[]
							{
								tradeMessageList[i].OrderNO,
								@string,
								string2,
								tradeMessageList[i].CommodityID,
								tradeMessageList[i].TradeQuatity
							});
						}
						else
						{
							text += string.Format("{0}{1}({2}){3}{4}\n", new object[]
							{
								tradeMessageList[i].OrderNO,
								@string,
								tradeMessageList[i].CommodityID,
								string2,
								tradeMessageList[i].TradeQuatity
							});
						}
					}
					if (this.RefreshCurrentTab != null)
					{
						this.RefreshCurrentTab(1, true);
					}
				}
				this.displayTradeInfo(text);
			}
		}
		private void displayTradeInfo(object obj)
		{
			string text = (string)obj;
			if (!text.Equals(""))
			{
				PlayWav.PlayWavResource("ring.wav", 0);
				if (IniData.GetInstance().SuccessShowDialog)
				{
					if (this.ShowNotifier != null)
					{
						this.ShowNotifier(text);
						return;
					}
				}
				else
				{
					text = text.Replace("\n", "\0");
					if (Global.StatusInfoFill != null)
					{
						Global.StatusInfoFill(text, Global.RightColor, true);
					}
				}
			}
		}
	}
}
