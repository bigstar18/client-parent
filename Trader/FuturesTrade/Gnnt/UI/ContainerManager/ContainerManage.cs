namespace FuturesTrade.Gnnt.UI.ContainerManager
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.UI.Containers;
    using FuturesTrade.Gnnt.UI.Modules.HQ;
    using FuturesTrade.Gnnt.UI.Modules.Order;
    using FuturesTrade.Gnnt.UI.Modules.Status;
    using FuturesTrade.Gnnt.UI.Modules.Tools;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ContainerManage : IContainerManage
    {
        private Size autoScrollSize = new Size(810, 230);
        private Panel CenterPanel = new Panel();
        private int CenterPanelWidth = 170;
        private int formStaly;
        private Panel LeftPanel = new Panel();
        private LockControl lockControl = new LockControl();
        private Form MainForm;
        private OperationManager operation = OperationManager.GetInstance();
        private QueryTabControl queryTabControl;
        private Panel RightPanel = new Panel();
        private int RightPanelWidth = 170;
        private StatusInfoBar statusInfo = new StatusInfoBar();
        private ToolsBar toolsBar = new ToolsBar(0);

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

        private void FillBottom()
        {
            this.statusInfo.Dock = DockStyle.Bottom;
            this.MainForm.Controls.Add(this.statusInfo);
        }

        private void FillCenterPanel()
        {
            FuturesTrade.Gnnt.UI.Modules.Order.Order order = new FuturesTrade.Gnnt.UI.Modules.Order.Order
            {
                Dock = DockStyle.Fill
            };
            this.CenterPanel.Controls.Add(order);
            this.CenterPanel.Dock = DockStyle.Fill;
            this.MainForm.Controls.Add(this.CenterPanel);
        }

        private void FillLeftPanel()
        {
            this.queryTabControl = new QueryTabControl(this.FormStaly);
            this.queryTabControl.Dock = DockStyle.Fill;
            queryTabControl.BorderStyle = BorderStyle.None;
            //this.queryTabControl.BackColor = Color.Black;
            this.LeftPanel.Controls.Add(this.queryTabControl);

            this.LeftPanel.Dock = DockStyle.Left;
            this.LeftPanel.AutoScroll = true;
            this.LeftPanel.AutoScrollMinSize = this.autoScrollSize;
            this.MainForm.Controls.Add(this.LeftPanel);

            this.MainForm.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            this.toolsBar.Location = new Point(this.LeftPanel.Right - this.toolsBar.ToolsBarWidth, 0);
            this.toolsBar.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            toolsBar.BorderStyle = BorderStyle.None;
            LeftPanel.BorderStyle = BorderStyle.None;
            this.LeftPanel.Controls.Add(this.toolsBar);
            this.toolsBar.BringToFront();
        }

        private void FillLockControl()
        {
            this.lockControl.Dock = DockStyle.Fill;
            this.lockControl.Visible = false;
            this.MainForm.Controls.Add(this.lockControl);
        }

        private void FillRightPanel()
        {
            HQCommodityInfo info = new HQCommodityInfo
            {
                Dock = DockStyle.Fill
            };
            this.RightPanel.Controls.Add(info);
            this.RightPanel.Dock = DockStyle.Right;
            this.MainForm.Controls.Add(this.RightPanel);
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
            }
            else
            {
                this.lockControl.SendToBack();
                this.statusInfo.BringToFront();
                this.LeftPanel.BringToFront();
                this.CenterPanel.BringToFront();
                this.RightPanel.BringToFront();
                this.toolsBar.BringToFront();
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.toolsBar.Location = new Point(this.queryTabControl.Right - this.toolsBar.ToolsBarWidth, 0);
        }

        public void MainFormKeyUp(Keys keyData)
        {
            this.queryTabControl.MainFormKeyUp(keyData);
        }

        public void SetPanelWidth()
        {
            this.RightPanel.Width = this.RightPanelWidth;
            this.LeftPanel.Width = (this.MainForm.Width - this.RightPanelWidth) - this.CenterPanelWidth;
        }

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
    }
}
