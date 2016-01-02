namespace FuturesTrade.Gnnt.UI.Forms
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.DBService.ServiceManager;
    using FuturesTrade.Gnnt.Library;
    using FuturesTrade.Gnnt.UI.ContainerManager;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using ToolsLibrary.util;

    public class MainForm : Form
    {
        private IContainer components;
        private IContainerManage containerManage;

        public MainForm(int formStyle)
        {
            this.InitializeComponent();
            
            base.Activate();
            Global.GetArrItems();
            if (formStyle == 0)
            {
                this.containerManage = new ContainerManage(this);
            }
            else if (formStyle == 1)
            {
                this.containerManage = new ContainerManagerP1(this);
            }
            else if (formStyle == 8)
            {
                this.containerManage = new ContainorManagerT8(this);
            }
            this.containerManage.FormStaly = formStyle;
            this.containerManage.ControlLayOut();
            Global.QueryDataLoad();
            this.Text = "点此恢复交易窗体";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x124, 0x10a);
            base.KeyPreview = true;
            this.BackColor = Color.FromArgb(80,1,0);
            base.Name = "MainForm";
            this.Text = "MainForm";
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
            base.Load += new EventHandler(this.MainForm_Load);
            base.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            base.KeyUp += new KeyEventHandler(this.MainForm_KeyUp);
            base.ResumeLayout(false);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Dispose();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.ReSetGlobal();
            OperationManager.GetInstance().DispostOperationManager();
            ServiceManage.GetInstance().DisposeServiceManage();
            TradeDataInfo.ClearMemoryData();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.containerManage != null)
            {
                this.containerManage.MainFormKeyUp(e.KeyData);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SetFormSize();
            ScaleForm.ScaleForms(this);
            //this.BackColor = Color.Black;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.containerManage != null)
            {
                this.containerManage.SetPanelWidth();
            }
        }

        private void SetFormSize()
        {
            base.Size = new Size(Global.screenWidth, (Global.screenHight / 3) + 60);
            base.Location = new Point(0, (Global.screenHight - (Global.screenHight / 3)) - 60);
        }
    }
}
