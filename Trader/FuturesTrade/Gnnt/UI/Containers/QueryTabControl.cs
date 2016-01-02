namespace FuturesTrade.Gnnt.UI.Containers
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.UI.ContainerManager;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TabTest;

    public class QueryTabControl : UserControl
    {
        private IContainer components;
        private OperationManager operationManager = OperationManager.GetInstance();
        private MyTabControl tabMain;
        private TabPage TabPageF2;
        private TabPage TabPageF3;
        private TabPage TabPageF4;
        private TabPage TabPageF5;
        private TabPage TabPageF6;
        private TabPage TabPageF7;
        private TabPage TabPageF8;
        private TabPage TabPageF9;

        public QueryTabControl(int formStaly)
        {
            this.InitializeComponent();
            new QueryControlLoad(formStaly).FillControl(this.tabMain, ControlLoad.All_Control);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle rec = tabMain.ClientRectangle;
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            Font font = new Font("微软雅黑", 8,FontStyle.Regular);
            SolidBrush bruFont = new SolidBrush(Color.FromArgb(0, 0, 0));
            SolidBrush bru = new SolidBrush(Color.FromArgb(72, 181, 250));
            for (int i = 0; i < tabMain.TabPages.Count; i++)

            {

                //获取标签头的工作区域
                Rectangle recChild = tabMain.GetTabRect(i);
                //绘制标签头背景颜色
                //e.Graphics.FillRectangle(bru, recChild);
                //绘制标签头的文字
                e.Graphics.DrawString(tabMain.TabPages[i].Text, font, bruFont, recChild, StrFormat);
            }


        }
        private void InitializeComponent()
        {
            this.BackColor = Color.WhiteSmoke;
            this.BorderStyle = BorderStyle.None;
            this.tabMain = new MyTabControl();
            
            this.TabPageF2 = new TabPage();
            this.TabPageF3 = new TabPage();
            this.TabPageF4 = new TabPage();
            this.TabPageF5 = new TabPage();
            this.TabPageF6 = new TabPage();
            this.TabPageF7 = new TabPage();
            this.TabPageF8 = new TabPage();
            this.TabPageF9 = new TabPage();
            this.tabMain.SuspendLayout();
            base.SuspendLayout();
            this.tabMain.Controls.Add(this.TabPageF2);
            this.tabMain.Controls.Add(this.TabPageF3);
            this.tabMain.Controls.Add(this.TabPageF4);
            this.tabMain.Controls.Add(this.TabPageF5);
            this.tabMain.Controls.Add(this.TabPageF6);
            this.tabMain.Controls.Add(this.TabPageF7);
            this.tabMain.Controls.Add(this.TabPageF8);
            this.tabMain.Controls.Add(this.TabPageF9);
            this.tabMain.Dock = DockStyle.Fill;
          
            this.tabMain.Location = new Point(0, 0);
            this.tabMain.Multiline = true;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new Size(900, 0x12a);
            this.tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabMain.DrawItem += new DrawItemEventHandler(tabMain_DrawItem);
            this.tabMain.TabIndex = 3;
            this.tabMain.TabStop = false;
            
           
            this.tabMain.SelectedIndexChanged += new EventHandler(this.tabMain_SelectedIndexChanged);
            this.TabPageF2.AutoScroll = true;
            this.TabPageF2.BorderStyle = BorderStyle.None;
            this.TabPageF2.Location = new Point(4, 0x15);
            this.TabPageF2.Name = "TabPageF2";
            this.TabPageF2.Size = new Size(0x37c, 0x111);
     
            this.TabPageF2.TabIndex = 0;
            this.TabPageF2.Text = "F2 委托";
            this.TabPageF2.BackColor = Color.WhiteSmoke;
            this.TabPageF2.UseVisualStyleBackColor = true;
            this.TabPageF3.Location = new Point(4, 0x15);
            this.TabPageF3.Name = "TabPageF3";
            this.TabPageF3.BorderStyle = BorderStyle.None;
            this.TabPageF3.Size = new Size(0x37c, 0x111);
            this.TabPageF3.TabIndex = 1;
            this.TabPageF3.Text = "F3 查委托";
            this.TabPageF3.UseVisualStyleBackColor = true;
            this.TabPageF3.Visible = false;
            this.TabPageF4.Location = new Point(4, 0x15);
            this.TabPageF4.Name = "TabPageF4";
            this.TabPageF4.Size = new Size(0x37c, 0x111);
            this.TabPageF4.TabIndex = 2;
            this.TabPageF4.Text = "F4 查成交";
            this.TabPageF4.BorderStyle = BorderStyle.None;
            this.TabPageF4.UseVisualStyleBackColor = true;
            this.TabPageF4.Visible = false;
            this.TabPageF5.Location = new Point(4, 0x15);
            this.TabPageF5.Name = "TabPageF5";
            this.TabPageF5.Size = new Size(0x37c, 0x111);
            this.TabPageF5.TabIndex = 3;
            this.TabPageF5.BorderStyle = BorderStyle.None;
            this.TabPageF5.Text = "F5 查订货";
            this.TabPageF5.UseVisualStyleBackColor = true;
            this.TabPageF5.Visible = false;
            this.TabPageF6.Location = new Point(4, 0x15);
            this.TabPageF6.Name = "TabPageF6";
            this.TabPageF6.Size = new Size(0x37c, 0x111);
            this.TabPageF6.TabIndex = 4;
            this.TabPageF6.BorderStyle = BorderStyle.None;
            this.TabPageF6.Text = "F6 资金";
            this.TabPageF6.UseVisualStyleBackColor = true;
            this.TabPageF6.Visible = false;
            this.TabPageF7.Location = new Point(4, 0x15);
            this.TabPageF7.Name = "TabPageF7";
            this.TabPageF7.Size = new Size(0x37c, 0x111);
            this.TabPageF7.TabIndex = 7;
            this.TabPageF7.BorderStyle = BorderStyle.None;
            this.TabPageF7.Text = "F7商品";
            this.TabPageF7.UseVisualStyleBackColor = true;
            this.TabPageF8.Location = new Point(4, 0x15);
            this.TabPageF8.Name = "TabPageF8";
            this.TabPageF8.Size = new Size(0x37c, 0x111);
            this.TabPageF8.TabIndex = 5;
            this.TabPageF8.BorderStyle = BorderStyle.None;
            this.TabPageF8.Text = "F8 预埋委托";
            this.TabPageF8.UseVisualStyleBackColor = true;
            this.TabPageF8.Visible = false;
            this.TabPageF9.Location = new Point(4, 0x15);
            this.TabPageF9.Name = "TabPageF9";
            this.TabPageF9.BorderStyle = BorderStyle.None;
            this.TabPageF9.Size = new Size(0x37c, 0x111);
            this.TabPageF9.TabIndex = 8;
            this.TabPageF9.Text = "F9 查条件下单";
            this.TabPageF9.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tabMain);
            base.Name = "QueryTabControl";
            base.Size = new Size(900, 0x12a);
            base.Load += new EventHandler(this.QueryTabControl_Load);
            

            this.tabMain.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void MainFormKeyUp(Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                this.tabMain.SelectedIndex = 0;
            }
            else if (keyData == Keys.F3)
            {
                this.tabMain.SelectedIndex = 1;
            }
            else if (keyData == Keys.F4)
            {
                this.tabMain.SelectedIndex = 2;
            }
            else if (keyData == Keys.F5)
            {
                this.tabMain.SelectedIndex = 3;
            }
            else if (keyData == Keys.F6)
            {
                this.tabMain.SelectedIndex = 4;
            }
            else if (keyData == Keys.F7)
            {
                this.tabMain.SelectedIndex = 5;
            }
            else if (keyData == Keys.F8)
            {
                this.tabMain.SelectedIndex = 6;
            }
            else if (keyData == Keys.F9)
            {
                this.tabMain.SelectedIndex = 7;
            }
        }

        private void QueryTabControl_Load(object sender, EventArgs e)
        {
            //this.TabPageF2.BackColor = Color;
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabMain.SelectedIndex)
            {
                case 0:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.UnTrade_TradeOrder;
                    break;

                case 1:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.AllOrder;
                    break;

                case 2:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.AllTrade;
                    break;

                case 3:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.Holding_HoldingDetail;
                    break;

                case 4:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.FundsInfo;
                    break;

                case 5:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.CommodityInfo;
                    break;

                case 6:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.PreDelegates;
                    break;

                case 7:
                    this.operationManager.CurrentSelectIndex = RefreshQueryInfo.ConditionOrder;
                    break;
            }
            this.operationManager.TabMainSelectIndexChanged();
        }
    }
}
