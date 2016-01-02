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

    public class ContainorManagerT8 : IContainerManage
    {
        private Size autoScrollSize = new Size(850, 240);
        private int formStaly;
        private Panel LeftPanel = new Panel();
        private int LeftPanelWidth = 170;
        private LockControl lockControl = new LockControl();
        private Form MainForm;
        private OperationManager operation = OperationManager.GetInstance();
        private QueryTabControl queryTabControl;
        private Panel RightPanel = new Panel();
        private StatusInfoBar statusInfo = new StatusInfoBar();
        private ToolsBar toolsBar = new ToolsBar(8);
        private Panel TopPanel = new Panel();
        private int TopPanelHeight = 0x55;

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

        private void FillBottom()
        {
            this.statusInfo.CreateControl();
            this.statusInfo.Dock = DockStyle.Bottom;
            this.MainForm.Controls.Add(this.statusInfo);
        }

        private void FillLeftPanel()
        {
            this.queryTabControl = new QueryTabControl(this.FormStaly);
            this.queryTabControl.Dock = DockStyle.Fill;
            this.queryTabControl.CreateControl();
            //this.queryTabControl.BackColor = Color.Black;
            this.RightPanel.Controls.Add(this.queryTabControl);
            this.RightPanel.BorderStyle = BorderStyle.None;
            this.RightPanel.Dock = DockStyle.Fill;
            this.RightPanel.AutoScroll = true;
            this.RightPanel.AutoScrollMinSize = this.autoScrollSize;
            this.MainForm.Controls.Add(this.RightPanel);
            this.MainForm.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            this.toolsBar.Location = new Point(this.RightPanel.Right - this.toolsBar.ToolsBarWidth, 0);
            this.toolsBar.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            //this.toolsBar.Left -= 30; 
            this.RightPanel.Controls.Add(this.toolsBar);
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
            this.LeftPanel.Controls.Add(info);
            this.LeftPanel.Dock = DockStyle.Right;
            this.MainForm.Controls.Add(this.LeftPanel);
        }

        private void FillTopPanel()
        {
            T8Order order = new T8Order();
            order.CreateControl();
            order.Dock = DockStyle.Fill;
            this.TopPanel.Controls.Add(order);
            this.TopPanel.Dock = DockStyle.Top;
            this.MainForm.Controls.Add(this.TopPanel);
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
            }
            else
            {
                this.lockControl.SendToBack();
                this.statusInfo.BringToFront();
                this.TopPanel.BringToFront();
                this.LeftPanel.BringToFront();
                this.RightPanel.BringToFront();
                this.toolsBar.BringToFront();
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.toolsBar.Location = new Point(this.RightPanel.Right - this.toolsBar.ToolsBarWidth-20, 0);

        }

        public void MainFormKeyUp(Keys keyData)
        {
            this.queryTabControl.MainFormKeyUp(keyData);
        }

        public void SetPanelWidth()
        {
            this.TopPanel.Height = this.TopPanelHeight;
            this.LeftPanel.Width = this.LeftPanelWidth;
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
