using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.UI.Containers;
using FuturesTrade.Gnnt.UI.Modules.Order;
using FuturesTrade.Gnnt.UI.Modules.Status;
using FuturesTrade.Gnnt.UI.Modules.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.UI.ContainerManager
{
	public class ContainerManagerP1 : IContainerManage
	{
		private Form MainForm;
		private int formStaly;
		private TreeViewControl treeView;
		private OperationManager operation = OperationManager.GetInstance();
		private Panel LeftPanel = new Panel();
		private Panel CenterPanel = new Panel();
		private Panel RightPanel = new Panel();
		private int CenterPanelWidth = 270;
		private int LeftPanelWidth = 110;
		private StatusInfoBar statusInfo = new StatusInfoBar();
		private LockControl lockControl = new LockControl();
		private ToolsBar toolsBar = new ToolsBar(1);
		private Size autoScrollSize = new Size(850, 240);
		private QueryControlLoad queryControlLoad;
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
		public QueryControlLoad QueryControlLoad
		{
			get
			{
				return this.queryControlLoad;
			}
			set
			{
				this.queryControlLoad = value;
			}
		}
		public ContainerManagerP1(Form _mainForm)
		{
			this.MainForm = _mainForm;
			this.operation.CurrentSelectIndex = RefreshQueryInfo.AllOrder_HoldingCollect;
			this.toolsBar.lockFormCallBack = new ToolsBar.LockFormCallBack(this.LockForm);
			this.lockControl.lockFormCallBack = new LockControl.LockFormCallBack(this.LockForm);
		}
		public void ControlLayOut()
		{
			this.queryControlLoad = new QueryControlLoad(1);
			this.FillCenterPanel();
			this.FillLockControl();
			this.FillLeftPanel();
			this.FillRightPanel(ControlLoad.HC_AO_Control);
			this.FillBottom();
			this.SetPanelWidth();
		}
		private void FillLeftPanel()
		{
			this.treeView = new TreeViewControl();
			this.treeView.TreeviewNodeClick = new TreeViewControl.TreeviewNodeClickCallBack(this.FillRightPanel);
			this.treeView.Dock = DockStyle.Fill;
			this.LeftPanel.Controls.Add(this.treeView);
			this.LeftPanel.Dock = DockStyle.Left;
			this.MainForm.Controls.Add(this.LeftPanel);
		}
		private void FillRightPanel(ControlLoad controlLoad)
		{
			this.RightPanel.Controls.Clear();
			this.queryControlLoad.FillControl(this.RightPanel, controlLoad);
			this.RightPanel.Dock = DockStyle.Right;
			this.RightPanel.AutoScroll = true;
			this.RightPanel.AutoScrollMinSize = this.autoScrollSize;
			if (!this.MainForm.Controls.Contains(this.RightPanel))
			{
				this.MainForm.Controls.Add(this.RightPanel);
			}
			this.MainForm.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
			this.toolsBar.Location = new Point(this.MainForm.Right - this.toolsBar.ToolsBarWidth, 0);
			this.toolsBar.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			if (!this.MainForm.Controls.Contains(this.toolsBar))
			{
				this.MainForm.Controls.Add(this.toolsBar);
			}
			this.toolsBar.BringToFront();
		}
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			this.toolsBar.Location = new Point(this.MainForm.Right - this.toolsBar.ToolsBarWidth, 0);
		}
		private void FillCenterPanel()
		{
			P1Order p1Order = new P1Order();
			p1Order.Dock = DockStyle.Fill;
			this.CenterPanel.Controls.Add(p1Order);
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
			this.LeftPanel.Width = this.LeftPanelWidth;
			this.RightPanel.Width = this.MainForm.Width - this.LeftPanelWidth - this.CenterPanelWidth - 16;
		}
		public void MainFormKeyUp(Keys keyData)
		{
			this.treeView.MainFormKeyUp(keyData);
		}
	}
}
