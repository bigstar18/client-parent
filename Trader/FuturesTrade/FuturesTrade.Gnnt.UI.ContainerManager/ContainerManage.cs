using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.UI.Containers;
using FuturesTrade.Gnnt.UI.Modules.HQ;
using FuturesTrade.Gnnt.UI.Modules.Order;
using FuturesTrade.Gnnt.UI.Modules.Status;
using FuturesTrade.Gnnt.UI.Modules.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.UI.ContainerManager
{
	public class ContainerManage : IContainerManage
	{
		private Form MainForm;
		private int formStaly;
		private QueryTabControl queryTabControl;
		private OperationManager operation = OperationManager.GetInstance();
		private Panel LeftPanel = new Panel();
		private Panel CenterPanel = new Panel();
		private Panel RightPanel = new Panel();
		private StatusInfoBar statusInfo = new StatusInfoBar();
		private LockControl lockControl = new LockControl();
		private ToolsBar toolsBar = new ToolsBar(0);
		private int CenterPanelWidth = 170;
		private int RightPanelWidth = 170;
		private Size autoScrollSize = new Size(810, 230);
		public int FormStaly
		{
			get
			{
				return this.formStaly;
			}
			set
			{
				this.formStaly = value;
			}
		}
		public ContainerManage(Form _mainForm)
		{
			this.MainForm = _mainForm;
			this.operation.CurrentSelectIndex = RefreshQueryInfo.UnTrade_TradeOrder;
			this.toolsBar.lockFormCallBack = new ToolsBar.LockFormCallBack(this.LockForm);
			this.lockControl.lockFormCallBack = new LockControl.LockFormCallBack(this.LockForm);
		}
		public void ControlLayOut()
		{
			this.FillCenterPanel();
			this.FillLeftPanel();
			this.FillRightPanel();
			this.FillBottom();
			this.SetPanelWidth();
			this.FillLockControl();
		}
		private void FillLeftPanel()
		{
			this.queryTabControl = new QueryTabControl(this.FormStaly);
			this.queryTabControl.Dock = DockStyle.Fill;
			this.LeftPanel.Controls.Add(this.queryTabControl);
			this.LeftPanel.Dock = DockStyle.Left;
			this.LeftPanel.AutoScroll = true;
			this.LeftPanel.AutoScrollMinSize = this.autoScrollSize;
			this.MainForm.Controls.Add(this.LeftPanel);
			this.MainForm.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
			this.toolsBar.Location = new Point(this.LeftPanel.Right - this.toolsBar.ToolsBarWidth, 0);
			this.toolsBar.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.LeftPanel.Controls.Add(this.toolsBar);
			this.toolsBar.BringToFront();
		}
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			this.toolsBar.Location = new Point(this.LeftPanel.Right - this.toolsBar.ToolsBarWidth, 0);
		}
		private void FillRightPanel()
		{
			HQCommodityInfo hQCommodityInfo = new HQCommodityInfo();
			hQCommodityInfo.Dock = DockStyle.Fill;
			this.RightPanel.Controls.Add(hQCommodityInfo);
			this.RightPanel.Dock = DockStyle.Right;
			this.MainForm.Controls.Add(this.RightPanel);
		}
		private void FillCenterPanel()
		{
			Order order = new Order();
			order.Dock = DockStyle.Fill;
			this.CenterPanel.Controls.Add(order);
			this.CenterPanel.Dock = DockStyle.Fill;
			this.MainForm.Controls.Add(this.CenterPanel);
		}
		private void FillBottom()
		{
			this.statusInfo.Dock = DockStyle.Bottom;
			this.MainForm.Controls.Add(this.statusInfo);
		}
		private void FillLockControl()
		{
			this.lockControl.Dock = DockStyle.Fill;
			this.lockControl.Visible = false;
			this.MainForm.Controls.Add(this.lockControl);
		}
		public void LockForm(bool type)
		{
			this.lockControl.LockSet(type);
			this.lockControl.Visible = !type;
			this.CenterPanel.Visible = type;
			this.LeftPanel.Visible = type;
			this.RightPanel.Visible = type;
			this.statusInfo.Visible = type;
			this.toolsBar.Visible = type;
			if (!type)
			{
				this.lockControl.BringToFront();
				return;
			}
			this.lockControl.SendToBack();
			this.statusInfo.BringToFront();
			this.LeftPanel.BringToFront();
			this.CenterPanel.BringToFront();
			this.RightPanel.BringToFront();
			this.toolsBar.BringToFront();
		}
		public void SetPanelWidth()
		{
			this.RightPanel.Width = this.RightPanelWidth;
			this.LeftPanel.Width = this.MainForm.Width - this.RightPanelWidth - this.CenterPanelWidth;
		}
		public void MainFormKeyUp(Keys keyData)
		{
			this.queryTabControl.MainFormKeyUp(keyData);
		}
	}
}
