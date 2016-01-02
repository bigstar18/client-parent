using FuturesTrade.Gnnt.BLL.Order;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation;
using FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using TPME.Log;
namespace FuturesTrade.Gnnt.BLL.Manager
{
	public class OperationManager
	{
		public delegate void SetComboCommodityCallBack(List<string> commodityIDList);
		public delegate void SetTransactionCallBack(List<string> transactionLis);
		public delegate void ShowHoldingControlCallBack();
		public delegate void TransferInfoCallBack(string info, byte priceQtyFlag);
		public delegate void SetHQTimerEnableCallBack(bool enable);
		public string InputRightPageNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPageNum");
		public string PageNumError = Global.M_ResourceManager.GetString("TradeStr_MainForm_PageNumError");
		public string InputPageNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPageNum");
		public string Prompt = Global.M_ResourceManager.GetString("TradeStr_MainForm_Prompt");
		public string RevokeOrders = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrders");
		public string DoubleClickCancellation = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_DoubleClickCancellation");
		public string RevokeOrdersMessge = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrdersMessge");
		public string Total = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
		public string StrAll = Global.M_ResourceManager.GetString("TradeStr_RadioAllF3");
		public string StrBuy = Global.M_ResourceManager.GetString("TradeStr_RadioB");
		public string StrSale = Global.M_ResourceManager.GetString("TradeStr_RadioS");
		public string AllCheck = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_AllCheck");
		public string AllNotCheck = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_AllNotCheck");
		public string SussceOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SussceOrder");
		public string ErrorTitle = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorTitle");
		public ConOrderOperation conOrderOperation;
		public OrderOperation orderOperation;
		public T8OrderOperation t8OrderOperation;
		public SubmitOrderOperation submitOrderOperation;
		public SubmitConOrderOperation submitConOrderOperation;
		public RevokeOrderOperation revokeOrderOperation;
		public RevokeConOrderOperation revokeConOrderOperation;
		public QueryAllOrderOperation queryAllOrderOperation;
		public QueryUnOrderOperation queryUnOrderOperation;
		public QueryTradeOperation queryTradeOperation;
		public QueryTradeOrderOperation queryTradeOrderOperation;
		public QueryCommDataOperation queryCommDataOperation;
		public QueryConOrderOperation queryConOrderOperation;
		public QueryHoldingDetailOperation queryHoldingDatailOperation;
		public QueryHoldingOperation queryHoldingOperation;
		public QueryInitDataOperation queryInitDataOperation;
		public QueryPredelegateOperation queryPredelegateOperation;
		public QuerySysTimeOperation querySysTimeOperation;
		public OperationManager.ShowHoldingControlCallBack ShowHoldingCollect;
		public OperationManager.ShowHoldingControlCallBack ShowHoldingDetail;
		public OperationManager.TransferInfoCallBack TransferInfo;
		public OperationManager.SetHQTimerEnableCallBack SetHQTimerEnable;
		private static volatile OperationManager operationManager;
		public List<string> commodityList = new List<string>();
		public List<string> myCommodityList = new List<string>();
		public List<string> transactionsList = new List<string>();
		public List<string> myTransactionsList = new List<string>();
		public RefreshQueryInfo CurrentSelectIndex;
		public int IdleOnMoudel;
		public int IdleRefreshButton;
		public event OperationManager.SetComboCommodityCallBack SetComboCommodityEvent;
		public event OperationManager.SetTransactionCallBack SetTransactionEvent;
		public static OperationManager GetInstance()
		{
			if (OperationManager.operationManager == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(OperationManager));
				try
				{
					if (OperationManager.operationManager == null)
					{
						try
						{
							OperationManager.operationManager = new OperationManager();
						}
						catch (Exception ex)
						{
							Logger.wirte(ex);
						}
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return OperationManager.operationManager;
		}
		public void GetCommodityInfoList()
		{
			try
			{
				this.commodityList.Clear();
				this.myCommodityList.Clear();
				XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
				DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
				this.commodityList.Add(this.StrAll);
				this.myCommodityList.Add(this.StrAll);
				foreach (DataRow dataRow in dataSetByXml.Tables[0].Rows)
				{
					if ((bool)dataRow["Flag"])
					{
						this.myCommodityList.Add(dataRow["commodityCode"].ToString());
					}
					else
					{
						this.commodityList.Add(dataRow["commodityCode"].ToString());
					}
				}
				if (this.SetComboCommodityEvent != null)
				{
					if (this.myCommodityList.Count <= 1)
					{
						this.SetComboCommodityEvent(this.commodityList);
					}
					else
					{
						this.SetComboCommodityEvent(this.myCommodityList);
					}
				}
				Global.LoadFlag = false;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "获取商品信息错误：" + ex.Message);
			}
		}
		public void GetTransactionInfoList()
		{
			try
			{
				XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml);
				DataSet dataSetByXml = xmlDataSet.GetDataSetByXml();
				this.transactionsList.Add(OperationManager.operationManager.StrAll);
				int num = 0;
				foreach (DataRow dataRow in dataSetByXml.Tables[0].Rows)
				{
					if ((bool)dataRow["Flag"])
					{
						string text = dataRow["TransactionsCode"].ToString();
						if (!Global.FirmID.Equals(text) && text.Length > 2)
						{
							this.myTransactionsList.Add(text);
						}
						num++;
					}
					else
					{
						string text2 = dataRow["TransactionsCode"].ToString();
						if (!Global.FirmID.Equals(text2) && text2.Length > 2)
						{
							this.transactionsList.Add(text2);
						}
					}
				}
				if (this.SetTransactionEvent != null)
				{
					this.SetTransactionEvent(this.transactionsList);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		public OperationManager()
		{
			this.conOrderOperation = new ConOrderOperation();
			this.orderOperation = new OrderOperation();
			this.t8OrderOperation = new T8OrderOperation();
			this.submitOrderOperation = new SubmitOrderOperation();
			this.submitConOrderOperation = new SubmitConOrderOperation();
			this.revokeOrderOperation = new RevokeOrderOperation();
			this.revokeConOrderOperation = new RevokeConOrderOperation();
			this.queryAllOrderOperation = new QueryAllOrderOperation();
			this.queryUnOrderOperation = new QueryUnOrderOperation();
			this.queryTradeOperation = new QueryTradeOperation();
			this.queryTradeOrderOperation = new QueryTradeOrderOperation();
			this.queryCommDataOperation = new QueryCommDataOperation();
			this.queryConOrderOperation = new QueryConOrderOperation();
			this.queryHoldingDatailOperation = new QueryHoldingDetailOperation();
			this.queryHoldingOperation = new QueryHoldingOperation();
			this.queryInitDataOperation = new QueryInitDataOperation();
			this.queryPredelegateOperation = new QueryPredelegateOperation();
			this.querySysTimeOperation = new QuerySysTimeOperation();
			this.SetOperationDelegate();
		}
		private void SetOperationDelegate()
		{
			this.queryInitDataOperation.isPagingQueryCallBack = new QueryInitDataOperation.IsPagingQueryCallBack(this.SetPagingInfo);
			this.queryInitDataOperation.ComboCommodityLoad = new QueryInitDataOperation.ComboCommodityLoadCallBack(this.GetCommodityInfoList);
			this.queryInitDataOperation.TransactionLoad = new QueryInitDataOperation.TransactionLoadCallBack(this.GetTransactionInfoList);
			this.revokeOrderOperation.refreshOrder = new RevokeOrderOperation.refreshOrderCallBack(this.SetQueryFlag);
			this.revokeConOrderOperation.refreshConOrder = new RevokeConOrderOperation.refreshConOrderCallBack(this.SetQueryFlag);
			this.orderOperation.refreshHQ = new OrderOperation.RefreshHQCallBack(this.queryCommDataOperation.RefreshGN);
			this.submitOrderOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
			this.querySysTimeOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
			this.queryConOrderOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
			this.queryCommDataOperation.UpdatePrice = new QueryCommDataOperation.UpdatePriceCallBack(this.orderOperation.UpdatePrice);
			this.submitOrderOperation.SubmitPredelegateInfo = new SubmitOrderOperation.SubmitPredelegateCallBack(this.queryPredelegateOperation.SubmitPredelegateInfo);
			this.queryPredelegateOperation.setMaxID = new QueryPredelegateOperation.SetMaxIDCallBack(this.submitOrderOperation.SetMaxID);
		}
		private void SetPagingInfo(bool isOrderNew, bool isTradeNew)
		{
			this.queryUnOrderOperation.SetUnOrderIsPaging(isOrderNew);
			this.queryAllOrderOperation.SetAllOrderIsPaging(isOrderNew);
			this.queryTradeOperation.SetTradeIsPaging(isTradeNew);
			this.queryTradeOrderOperation.SetTradeOrderIsPaging(isTradeNew);
		}
		private void SetQueryFlag(int mark, bool flag)
		{
			if (mark == 0)
			{
				this.queryUnOrderOperation.SetQueryUnOrderFlag(flag);
				this.queryAllOrderOperation.SetQueryUnOrderFlag(flag);
				this.queryInitDataOperation.SetQueryUnOrderFlag(flag);
				this.RefreshOrderInfo();
				return;
			}
			if (mark == 1)
			{
				this.SetAllQueryFlag(flag);
				this.TabMainSelectIndexChanged();
				return;
			}
			if (mark == 2)
			{
				this.queryConOrderOperation.SetQueryUnOrderFlag(flag);
				if (this.CurrentSelectIndex == RefreshQueryInfo.ConditionOrder)
				{
					this.queryConOrderOperation.QueryConOrderInfoLoad();
					return;
				}
			}
			else
			{
				if (mark == 3)
				{
					OperationManager.operationManager.queryPredelegateOperation.QueryPreDelegateLoad();
				}
			}
		}
		public void SetAllQueryFlag(bool flag)
		{
			this.queryUnOrderOperation.SetQueryUnOrderFlag(flag);
			this.queryTradeOrderOperation.SetQueryUnOrderFlag(flag);
			this.queryAllOrderOperation.SetQueryUnOrderFlag(flag);
			this.queryTradeOperation.SetQueryUnOrderFlag(flag);
			this.queryHoldingOperation.SetQueryUnOrderFlag(flag);
			this.queryHoldingDatailOperation.SetQueryUnOrderFlag(flag);
			this.queryInitDataOperation.SetQueryUnOrderFlag(flag);
			this.queryConOrderOperation.SetQueryUnOrderFlag(flag);
		}
		public void TabMainSelectIndexChanged()
		{
			switch (this.CurrentSelectIndex)
			{
			case RefreshQueryInfo.UnTrade_TradeOrder:
				this.queryUnOrderOperation.QueryUnOrderInfoLoad();
				this.queryTradeOrderOperation.QueryTradeOrderInfoLoad();
				break;
			case RefreshQueryInfo.AllOrder:
				this.queryAllOrderOperation.QueryAllOrderInfoLoad();
				break;
			case RefreshQueryInfo.AllTrade:
				this.queryTradeOperation.QueryTradeInfoLoad();
				break;
			case RefreshQueryInfo.HoldingDetail:
				this.queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
				break;
			case RefreshQueryInfo.FundsInfo:
				this.queryInitDataOperation.QueryFirmInfoThread();
				break;
			case RefreshQueryInfo.PreDelegates:
				this.queryPredelegateOperation.QueryPreDelegateLoad();
				break;
			case RefreshQueryInfo.ConditionOrder:
				this.queryConOrderOperation.QueryConOrderInfoLoad();
				break;
			case RefreshQueryInfo.AllOrder_HoldingCollect:
				this.queryAllOrderOperation.QueryAllOrderInfoLoad();
				this.queryHoldingOperation.QueryHoldingInfoLoad();
				break;
			case RefreshQueryInfo.Holding_HoldingDetail:
				this.queryHoldingOperation.QueryHoldingInfoLoad();
				this.queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
				break;
			}
			this.IdleOnMoudel = 0;
		}
		private void RefreshOrderInfo()
		{
			RefreshQueryInfo currentSelectIndex = this.CurrentSelectIndex;
			switch (currentSelectIndex)
			{
			case RefreshQueryInfo.UnTrade_TradeOrder:
				this.queryUnOrderOperation.QueryUnOrderInfo();
				return;
			case RefreshQueryInfo.AllOrder:
				break;
			default:
				if (currentSelectIndex == RefreshQueryInfo.FundsInfo)
				{
					this.queryInitDataOperation.QueryFirmInfoThread();
					return;
				}
				if (currentSelectIndex != RefreshQueryInfo.AllOrder_HoldingCollect)
				{
					return;
				}
				break;
			}
			this.queryAllOrderOperation.QueryAllOrderInfo();
		}
		public void SetRefreshTime(bool type)
		{
			OperationManager.operationManager.queryCommDataOperation.SetRefreshGNTime(type);
			OperationManager.operationManager.querySysTimeOperation.SetSysTimeRefreshTime(type);
		}
		public void ShowHolding(int flag)
		{
			if (flag == 0)
			{
				if (this.ShowHoldingCollect != null)
				{
					this.ShowHoldingCollect();
					return;
				}
			}
			else
			{
				if (flag == 1 && this.ShowHoldingDetail != null)
				{
					this.ShowHoldingDetail();
				}
			}
		}
		public void SetIdleOnMoudel()
		{
			this.queryUnOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryUnOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryTradeOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryTradeOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryAllOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryAllOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryTradeOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryTradeOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryHoldingOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryHoldingOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryHoldingDatailOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryHoldingDatailOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryInitDataOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryInitDataOperation.IdleRefreshButton = this.IdleRefreshButton;
			this.queryConOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
			this.queryConOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
		}
		public void DispostOperationManager()
		{
			if (OperationManager.operationManager != null)
			{
				OperationManager.operationManager = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
