using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.UI.Modules.Query;
using System;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.UI.ContainerManager
{
	public class QueryControlLoad
	{
		private SplitContainer splitContainer;
		private SplitContainer splitContainerHC;
		private TabControl tabControlF6;
		private UnTradeOrder unTradeOrder;
		private TradeOrder tradeOrder;
		private AllOrder allOrder;
		private Trade trade;
		private HoldingCollect holdingCollect;
		private HoldingDetail holdingDetail;
		private FundsInfo fundsInfo;
		private CommodityInfo commodityInfo;
		private PreDelegate predelegate;
		private ConditionOrderQuery conditionOrder;
		public QueryControlLoad(int style)
		{
			this.unTradeOrder = new UnTradeOrder();
			this.tradeOrder = new TradeOrder();
			this.allOrder = new AllOrder(style);
			this.trade = new Trade();
			this.holdingCollect = new HoldingCollect(style);
			this.holdingDetail = new HoldingDetail(style);
			this.fundsInfo = new FundsInfo();
			this.commodityInfo = new CommodityInfo();
			this.predelegate = new PreDelegate();
			this.conditionOrder = new ConditionOrderQuery();
		}
		public void FillControl(Control control, ControlLoad controlLoad)
		{
			if (control is TabControl)
			{
				TabControl tabControl = (TabControl)control;
				if (controlLoad == ControlLoad.All_Control)
				{
					this.LoadAllControl(tabControl);
					return;
				}
			}
			else
			{
				if (control is Panel)
				{
					Panel panel = (Panel)control;
					this.LoadPanelControl(panel, controlLoad);
				}
			}
		}
		private void LoadAllControl(TabControl tabControl)
		{
			if (this.splitContainer == null)
			{
				this.splitContainer = new SplitContainer();
				this.splitContainer.Orientation = Orientation.Horizontal;
				this.splitContainer.Dock = DockStyle.Fill;
				this.splitContainer.SplitterDistance = this.splitContainer.Height / 2;
				this.splitContainer.SplitterWidth = 1;
				this.unTradeOrder.Dock = DockStyle.Fill;
				this.tradeOrder.Dock = DockStyle.Fill;
				this.splitContainer.Panel1.Controls.Add(this.unTradeOrder);
				this.splitContainer.Panel2.Controls.Add(this.tradeOrder);
			}
			tabControl.TabPages[0].Controls.Add(this.splitContainer);
			this.allOrder.Dock = DockStyle.Fill;
			tabControl.TabPages[1].Controls.Add(this.allOrder);
			this.trade.Dock = DockStyle.Fill;
			tabControl.TabPages[2].Controls.Add(this.trade);
			this.holdingCollect.Dock = DockStyle.Fill;
			tabControl.TabPages[3].Controls.Add(this.holdingCollect);
			this.holdingDetail.Dock = DockStyle.Fill;
			tabControl.TabPages[3].Controls.Add(this.holdingDetail);
			this.holdingDetail.Visible = false;
			this.fundsInfo.Dock = DockStyle.Fill;
			tabControl.TabPages[4].Controls.Add(this.fundsInfo);
			this.commodityInfo.Dock = DockStyle.Fill;
			tabControl.TabPages[5].Controls.Add(this.commodityInfo);
			this.predelegate.Dock = DockStyle.Fill;
			tabControl.TabPages[6].Controls.Add(this.predelegate);
			this.conditionOrder.Dock = DockStyle.Fill;
			tabControl.TabPages[7].Controls.Add(this.conditionOrder);
		}
		private void LoadPanelControl(Panel panel, ControlLoad controlLoad)
		{
			switch (controlLoad)
			{
			case ControlLoad.F_C_HD_Control:
				if (this.tabControlF6 == null)
				{
					this.tabControlF6 = new TabControl();
					TabPage value = new TabPage("资金信息");
					TabPage value2 = new TabPage("商品信息");
					TabPage value3 = new TabPage("订货明细");
					this.tabControlF6.Controls.Add(value);
					this.tabControlF6.Controls.Add(value2);
					this.tabControlF6.Controls.Add(value3);
					this.tabControlF6.SelectedIndexChanged += new EventHandler(this.tabControlF6_SelectedIndexChanged);
				}
				this.fundsInfo.Dock = DockStyle.Fill;
				this.tabControlF6.TabPages[0].Controls.Add(this.fundsInfo);
				OperationManager.GetInstance().queryInitDataOperation.QueryFirmInfoThread();
				this.commodityInfo.Dock = DockStyle.Fill;
				this.tabControlF6.TabPages[1].Controls.Add(this.commodityInfo);
				this.holdingDetail.Dock = DockStyle.Fill;
				this.tabControlF6.TabPages[2].Controls.Add(this.holdingDetail);
				this.tabControlF6.Dock = DockStyle.Fill;
				panel.Controls.Add(this.tabControlF6);
				return;
			case ControlLoad.HC_AO_Control:
				if (this.splitContainerHC == null)
				{
					this.splitContainerHC = new SplitContainer();
				}
				this.splitContainerHC.Orientation = Orientation.Horizontal;
				this.splitContainerHC.Dock = DockStyle.Fill;
				this.splitContainerHC.SplitterDistance = this.splitContainerHC.Height / 2;
				this.splitContainerHC.SplitterWidth = 1;
				this.holdingCollect.Dock = DockStyle.Fill;
				this.allOrder.Dock = DockStyle.Fill;
				this.splitContainerHC.Panel1.Controls.Add(this.holdingCollect);
				this.splitContainerHC.Panel2.Controls.Add(this.allOrder);
				panel.Controls.Add(this.splitContainerHC);
				OperationManager.GetInstance().queryHoldingOperation.QueryHoldingInfoLoad();
				OperationManager.GetInstance().queryAllOrderOperation.QueryAllOrderInfoLoad();
				return;
			case ControlLoad.AllOrder_Control:
				this.allOrder.Dock = DockStyle.Fill;
				panel.Controls.Add(this.allOrder);
				OperationManager.GetInstance().queryAllOrderOperation.QueryAllOrderInfoLoad();
				return;
			case ControlLoad.Trade_Control:
				this.trade.Dock = DockStyle.Fill;
				panel.Controls.Add(this.trade);
				OperationManager.GetInstance().queryTradeOperation.QueryTradeInfoLoad();
				return;
			case ControlLoad.HC_Control:
				this.holdingCollect.Dock = DockStyle.Fill;
				panel.Controls.Add(this.holdingCollect);
				OperationManager.GetInstance().queryHoldingOperation.QueryHoldingInfoLoad();
				return;
			case ControlLoad.CO_Control:
				this.conditionOrder.Dock = DockStyle.Fill;
				panel.Controls.Add(this.conditionOrder);
				OperationManager.GetInstance().queryConOrderOperation.QueryConOrderInfoLoad();
				return;
			case ControlLoad.PRE_Control:
				this.predelegate.Dock = DockStyle.Fill;
				panel.Controls.Add(this.predelegate);
				OperationManager.GetInstance().queryPredelegateOperation.QueryPreDelegateLoad();
				return;
			default:
				return;
			}
		}
		private void tabControlF6_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.tabControlF6.SelectedIndex)
			{
			case 0:
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.FundsInfo;
				OperationManager.GetInstance().queryInitDataOperation.QueryFirmInfoThread();
				return;
			case 1:
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.CommodityInfo;
				return;
			case 2:
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.HoldingDetail;
				OperationManager.GetInstance().queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
				return;
			default:
				return;
			}
		}
	}
}
