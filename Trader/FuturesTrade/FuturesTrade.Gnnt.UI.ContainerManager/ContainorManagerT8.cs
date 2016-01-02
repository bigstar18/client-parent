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
	public class ContainorManagerT8 : IContainerManage
	{
		private Form MainForm;
		private int formStaly;
		private QueryTabControl queryTabControl;
		private OperationManager operation = OperationManager.GetInstance();
		private Panel LeftPanel = new Panel();
		private Panel TopPanel = new Panel();
		private Panel RightPanel = new Panel();
		private int TopPanelHeight = 85;
		private int LeftPanelWidth = 170;
		private StatusInfoBar statusInfo = new StatusInfoBar();
		private LockControl lockControl = new LockControl();
		private ToolsBar toolsBar = new ToolsBar(8);
		private Size autoScrollSize = new Size(850, 240);
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
		public ContainorManagerT8(Form _mainForm)
		{
			this.MainForm = _mainForm;
			this.operation.CurrentSelectIndex = RefreshQueryInfo.UnTrade_TradeOrder;
			this.toolsBar.lockFormCallBack = new ToolsBar.LockFormCallBack(this.LockForm);
			this.lockControl.lockFormCallBack = new LockControl.LockFormCallBack(this.LockForm);
		}
		public void ControlLayOut()
		{
			this.FillLeftPanel();
			this.FillRightPanel();
			this.FillTopPanel();
			this.FillBottom();
			this.SetPanelWidth();
			this.FillLockControl();
		}
		private void FillRightPanel()
		{
			HQCommodityInfo hQCommodityInfo = new HQCommodityInfo();
			hQCommodityInfo.Dock = DockStyle.Fill;
			this.LeftPanel.Controls.Add(hQCommodityInfo);
			this.LeftPanel.Dock = DockStyle.Right;
			this.MainForm.Controls.Add(this.LeftPanel);
		}
		private void FillLeftPanel()
		{
			this.queryTabControl = new QueryTabControl(this.FormStaly);
			this.queryTabControl.Dock = DockStyle.Fill;
			this.queryTabControl.CreateControl();
			this.RightPanel.Controls.Add(this.queryTabControl);
			this.RightPanel.Dock = DockStyle.Fill;
			this.RightPanel.AutoScroll = true;
			this.RightPanel.AutoScrollMinSize = this.autoScrollSize;
			this.MainForm.Controls.Add(this.RightPanel);
			this.MainForm.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
			this.toolsBar.Location = new Point(this.RightPanel.Right - this.toolsBar.ToolsBarWidth, 0);
			this.toolsBar.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.RightPanel.Controls.Add(this.toolsBar);
			this.toolsBar.BringToFront();
		}
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			this.toolsBar.Location = new Point(this.RightPanel.Right - this.toolsBar.ToolsBarWidth, 0);
		}
		private void FillTopPanel()
		{
			T8Order t8Order = new T8Order();
			t8Order.CreateControl();
			t8Order.Dock = DockStyle.Fill;
			this.TopPanel.Controls.Add(t8Order);
			this.TopPanel.Dock = DockStyle.Top;
			this.MainForm.Controls.Add(this.TopPanel);
		}
		private void FillBottom()
		{
			this.statusInfo.CreateControl();
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
			this.LeftPanel.Visible = type;
			this.TopPanel.Visible = type;
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
			this.TopPanel.BringToFront();
			this.LeftPanel.BringToFront();
			this.RightPanel.BringToFront();
			this.toolsBar.BringToFront();
		}
		public void SetPanelWidth()
		{
			this.TopPanel.Height = this.TopPanelHeight;
			this.LeftPanel.Width = this.LeftPanelWidth;
		}
		public void MainFormKeyUp(Keys keyData)
		{
			this.queryTabControl.MainFormKeyUp(keyData);
		}
	}
}
