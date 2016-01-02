namespace FuturesTrade.Gnnt.UI.Modules.Tools
{
    using FuturesTrade.Gnnt.Library;
    using FuturesTrade.Gnnt.UI.Forms.ToosForm;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using TPME.Log;

    public class ToolsBar : UserControl
    {
        private IContainer components;
        private ConditionOrder conditionOrder;
        private CrazyOrder crazyOrder;
        private FormOrder formOrder;
        private int formStyle;
        public LockFormCallBack lockFormCallBack;
        public int ToolsBarWidth = 150;
        public ToolStrip toolStrip;
        private ToolStripButton toolStripButtonAbout;
        private ToolStripButton toolStripButtonBill;
        private ToolStripButton toolStripButtonCO;
        private ToolStripButton toolStripButtonCon;
        private ToolStripButton toolStripButtonExit;
        private ToolStripButton toolStripButtonFloat;
        private ToolStripButton toolStripButtonHelp;
        private ToolStripButton toolStripButtonLock;
        private ToolStripButton toolStripButtonMsg;
        private ToolStripButton toolStripButtonOrder;
        private ToolStripButton toolStripButtonSet;
        private UserSet userSet;

        public ToolsBar(int _formStyle)
        {
            this.InitializeComponent();
            this.formStyle = _formStyle;
        }

        private void CloseForms()
        {
            if (this.formOrder != null)
            {
                this.formOrder.Close();
            }
            if (this.crazyOrder != null)
            {
                this.crazyOrder.Close();
            }
            if (this.conditionOrder != null)
            {
                this.conditionOrder.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.CloseForms();
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HideToolsBarButton()
        {
            try
            {
                this.toolStrip.Items.Remove(this.toolStripButtonMsg);
                this.toolStrip.Items.Remove(this.toolStripButtonBill);
                this.toolStrip.Items.Remove(this.toolStripButtonHelp);
                this.toolStrip.Items.Remove(this.toolStripButtonAbout);
                this.toolStrip.Items.Remove(this.toolStripButtonExit);
                base.Width = this.ToolsBarWidth;
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ToolsBar));
            this.toolStrip = new ToolStrip();
            
            this.toolStripButtonBill = new ToolStripButton();
            this.toolStripButtonCon = new ToolStripButton();
            this.toolStripButtonOrder = new ToolStripButton();
            this.toolStripButtonCO = new ToolStripButton();
            this.toolStripButtonSet = new ToolStripButton();
            this.toolStripButtonMsg = new ToolStripButton();
            this.toolStripButtonLock = new ToolStripButton();
            this.toolStripButtonHelp = new ToolStripButton();
            this.toolStripButtonAbout = new ToolStripButton();
            this.toolStripButtonExit = new ToolStripButton();
            this.toolStripButtonFloat = new ToolStripButton();
            this.toolStrip.SuspendLayout();
            base.SuspendLayout();
            this.toolStrip.BackColor = SystemColors.Control;
            this.toolStrip.Dock = DockStyle.Fill;
            this.toolStrip.GripMargin = new Padding(0);
            this.toolStrip.Items.AddRange(new ToolStripItem[] { this.toolStripButtonBill, this.toolStripButtonCon, this.toolStripButtonOrder, this.toolStripButtonCO, this.toolStripButtonSet, this.toolStripButtonMsg, this.toolStripButtonLock, this.toolStripButtonHelp, this.toolStripButtonAbout, this.toolStripButtonExit, this.toolStripButtonFloat });
            this.toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new Padding(0);
            this.toolStrip.RenderMode = ToolStripRenderMode.System;
            this.toolStrip.Size = new Size(0x11c, 0x17);
            this.toolStrip.TabIndex = 13;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.BackColor = Color.FromArgb(235,235,235);
            this.toolStripButtonBill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBill.Image = (Image)manager.GetObject("toolStripButtonBill.Image");
            this.toolStripButtonBill.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonBill.Name = "toolStripButtonBill";
            this.toolStripButtonBill.Size = new Size(25, 25);
            this.toolStripButtonBill.AutoSize = false;
            this.toolStripButtonBill.Text = "仓单交易";
            this.toolStripButtonCon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCon.Image = (Image)manager.GetObject("toolStripButtonCon.Image");
            this.toolStripButtonCon.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonCon.Name = "toolStripButtonCon";
            this.toolStripButtonCon.Size = new Size(25, 25);
            this.toolStripButtonCon.AutoSize = false;
            this.toolStripButtonCon.Text = "条件下单";
            this.toolStripButtonCon.Click += new EventHandler(this.toolStripButtonCon_Click);
            this.toolStripButtonOrder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOrder.Image = (Image)manager.GetObject("toolStripButtonOrder.Image");
            this.toolStripButtonOrder.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonOrder.Name = "toolStripButtonOrder";
            this.toolStripButtonOrder.Size = new Size(25, 25);
            this.toolStripButtonOrder.AutoSize = false;
            this.toolStripButtonOrder.Text = "快速下单";
            this.toolStripButtonOrder.Click += new EventHandler(this.toolStripButtonOrder_Click);
            this.toolStripButtonCO.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCO.Image = (Image)manager.GetObject("toolStripButtonCO.Image");
            
            this.toolStripButtonCO.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonCO.Name = "toolStripButtonCO";
            this.toolStripButtonCO.Size = new Size(25,25 );
            this.toolStripButtonCO.AutoSize = false;
            this.toolStripButtonCO.Text = "下单助手";
            this.toolStripButtonCO.Click += new EventHandler(this.toolStripButtonCO_Click);
            this.toolStripButtonCO.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            this.toolStripButtonSet.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSet.Image = (Image)manager.GetObject("toolStripButtonSet.Image");
            this.toolStripButtonSet.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonSet.Name = "toolStripButtonSet";
            this.toolStripButtonSet.Size = new Size(25, 25);
            this.toolStripButtonSet.AutoSize = false;
            this.toolStripButtonSet.Text = "toolStripButton1";
            this.toolStripButtonSet.ToolTipText = "设置";
            this.toolStripButtonSet.Click += new EventHandler(this.toolStripButtonSet_Click);
            this.toolStripButtonMsg.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMsg.Image = (Image)manager.GetObject("toolStripButtonMsg.Image");
            this.toolStripButtonMsg.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonMsg.Name = "toolStripButtonMsg";
            this.toolStripButtonMsg.Size = new Size(25, 25);
            this.toolStripButtonMsg.AutoSize = false;
            this.toolStripButtonMsg.Text = "toolStripButton1";
            this.toolStripButtonMsg.ToolTipText = "广播消息";
            this.toolStripButtonMsg.Click += new EventHandler(this.toolStripButtonMsg_Click);
            this.toolStripButtonLock.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLock.Image = (Image)manager.GetObject("toolStripButtonLock.Image");
            this.toolStripButtonLock.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonLock.Name = "toolStripButtonLock";
            this.toolStripButtonLock.Size = new Size(25, 25);
            this.toolStripButtonLock.AutoSize = false;
            this.toolStripButtonLock.Text = "toolStripButton1";
            this.toolStripButtonLock.ToolTipText = "锁定";
            this.toolStripButtonLock.Click += new EventHandler(this.toolStripButtonLock_Click);
            this.toolStripButtonHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = (Image)manager.GetObject("toolStripButtonHelp.Image");
            this.toolStripButtonHelp.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new Size(25, 25);
            this.toolStripButtonHelp.AutoSize = false;
            this.toolStripButtonHelp.Text = "toolStripButton2";
            this.toolStripButtonHelp.ToolTipText = "帮助";
            this.toolStripButtonAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAbout.Image = (Image)manager.GetObject("toolStripButtonAbout.Image");
            this.toolStripButtonAbout.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonAbout.Name = "toolStripButtonAbout";
            this.toolStripButtonAbout.Size = new Size(25, 25);
            this.toolStripButtonAbout.AutoSize = false;
            this.toolStripButtonAbout.Text = "关于";
            this.toolStripButtonExit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExit.Image = (Image)manager.GetObject("toolStripButtonExit.Image");
            this.toolStripButtonExit.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Size = new Size(25, 25);
            this.toolStripButtonExit.AutoSize = false;
            this.toolStripButtonExit.Text = "退出";
            this.toolStripButtonFloat.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFloat.Image = (Image)manager.GetObject("toolStripButtonFloat.Image");
            this.toolStripButtonFloat.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonFloat.Name = "toolStripButtonFloat";
            this.toolStripButtonFloat.Size = new Size(25, 25);
            this.toolStripButtonFloat.AutoSize = false;
            this.toolStripButtonFloat.Text = "浮动";
            this.toolStripButtonFloat.Click += new EventHandler(this.toolStripButtonFloat_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            this.AutoSize = true;
            base.Controls.Add(this.toolStrip);
            base.Margin = new Padding(0);

            base.Name = "ToolsBar";
            base.Size = new Size(0x11c+10, 0x17+3);
            this.Left -= 10;
            //base.BorderStyle = System.Windows.Forms.BorderStyle.None;
            base.Load += new EventHandler(this.ToolsBar_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ToolsBar_Load(object sender, EventArgs e)
        {
            this.HideToolsBarButton();
            //this.BackColor = Color.FromArgb(80,0,1);

        }

        private void toolStripButtonCO_Click(object sender, EventArgs e)
        {
            this.crazyOrder = CrazyOrder.Instance();
            this.crazyOrder.TopMost = true;
            this.crazyOrder.StartPosition = FormStartPosition.CenterScreen;
            this.crazyOrder.Show();
        }

        private void toolStripButtonCon_Click(object sender, EventArgs e)
        {
            if ((this.conditionOrder != null) && (this.conditionOrder.WindowState == FormWindowState.Minimized))
            {
                this.conditionOrder.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.conditionOrder = ConditionOrder.Instance();
                this.conditionOrder.TopMost = true;
                this.conditionOrder.StartPosition = FormStartPosition.CenterScreen;
                this.conditionOrder.Show();
            }
        }

        private void toolStripButtonFloat_Click(object sender, EventArgs e)
        {
            Global.FloatForm(sender, e);
        }

        private void toolStripButtonLock_Click(object sender, EventArgs e)
        {
            Global.LockForm(sender, e);
            if (this.lockFormCallBack != null)
            {
                this.lockFormCallBack(false);
            }
            this.CloseForms();
        }

        private void toolStripButtonMsg_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonOrder_Click(object sender, EventArgs e)
        {
            this.formOrder = FormOrder.Instance();
            this.formOrder.TopMost = true;
            this.formOrder.StartPosition = FormStartPosition.CenterScreen;
            this.formOrder.Show();
        }

        private void toolStripButtonSet_Click(object sender, EventArgs e)
        {
            if (this.userSet == null)
            {
                this.userSet = new UserSet(this.formStyle);
            }
            this.userSet.TopMost = true;
            this.userSet.StartPosition = FormStartPosition.CenterScreen;
            this.userSet.ShowDialog();
        }

        public delegate void LockFormCallBack(bool type);
    }
}
