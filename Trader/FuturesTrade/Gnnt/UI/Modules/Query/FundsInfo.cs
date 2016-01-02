namespace FuturesTrade.Gnnt.UI.Modules.Query
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using TabTest;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class FundsInfo : UserControl
    {
        private MyButton buttonFundsTransfer;
        private MyButton buttonSelFundsF4;
        private IContainer components;
        private FillHolding fillHolding;
        private FundsItemInfo fundsItemInfo = new FundsItemInfo();
        private GroupBox groupBoxFunds;
        private ListView lstVFunds;
        private OperationManager operationManager = OperationManager.GetInstance();

        public FundsInfo()
        {
            this.InitializeComponent();
            this.operationManager.queryInitDataOperation.FundsFill = new QueryInitDataOperation.FundsFillCallBack(this.HoldingInfoFill);
            this.CreateHandle();
        }

        private void buttonSelFundsF4_Click(object sender, EventArgs e)
        {
            this.operationManager.queryInitDataOperation.ButtonRefreshFlag = 1;
            this.operationManager.queryInitDataOperation.QueryFirmInfoThread();
            this.operationManager.IdleRefreshButton = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FundsInfo_Load(object sender, EventArgs e)
        {
            this.buttonFundsTransfer.Visible = false;
        }

        public void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void HoldingInfoFill(FirmInfoResponseVO firmInfoResponseVO)
        {
            try
            {
                this.fillHolding = new FillHolding(this.LstVFundsFill);
                this.HandleCreated();
                base.Invoke(this.fillHolding, new object[] { firmInfoResponseVO });
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void InitializeComponent()
        {
            this.groupBoxFunds = new GroupBox();
            this.buttonFundsTransfer = new MyButton();
            this.buttonSelFundsF4 = new MyButton();
            this.lstVFunds = new ListView();
            this.groupBoxFunds.SuspendLayout();
            base.SuspendLayout();
            this.groupBoxFunds.AutoSize = true;
            this.groupBoxFunds.Controls.Add(this.buttonFundsTransfer);
            this.groupBoxFunds.Controls.Add(this.buttonSelFundsF4);
            this.groupBoxFunds.Controls.Add(this.lstVFunds);
            this.groupBoxFunds.Dock = DockStyle.Fill;
            this.groupBoxFunds.Location = new Point(0, 0);
            this.groupBoxFunds.Name = "groupBoxFunds";
            this.groupBoxFunds.Size = new Size(700, 200);
            this.groupBoxFunds.TabIndex = 4;
            this.groupBoxFunds.TabStop = false;
            this.groupBoxFunds.Text = "资金查询";
            this.groupBoxFunds.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxFunds.ForeColor = Color.FromArgb(235, 235, 235);
            this.buttonFundsTransfer.BackColor = SystemColors.ControlDark;
            this.buttonFundsTransfer.Font = new Font("宋体", 9f, FontStyle.Bold);
            this.buttonFundsTransfer.ForeColor = Color.Maroon;
            this.buttonFundsTransfer.ImeMode = ImeMode.NoControl;
            this.buttonFundsTransfer.Location = new Point(0xb0, 8);
            this.buttonFundsTransfer.Name = "buttonFundsTransfer";
            this.buttonFundsTransfer.Size = new Size(100, 20);
            this.buttonFundsTransfer.TabIndex = 2;
            this.buttonFundsTransfer.Text = "资金划转";
            this.buttonFundsTransfer.UseVisualStyleBackColor = false;
            this.buttonSelFundsF4.ImeMode = ImeMode.NoControl;
            this.buttonSelFundsF4.Location = new Point(0x184, 8);
            this.buttonSelFundsF4.Name = "buttonSelFundsF4";
            this.buttonSelFundsF4.Size = new Size(0x39, 20);
            this.buttonSelFundsF4.TabIndex = 1;
            this.buttonSelFundsF4.Text = "刷新";
            this.buttonSelFundsF4.UseVisualStyleBackColor = true;
            this.buttonSelFundsF4.Click += new EventHandler(this.buttonSelFundsF4_Click);
            this.lstVFunds.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lstVFunds.FullRowSelect = true;
            this.lstVFunds.GridLines = true;
            this.lstVFunds.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lstVFunds.Location = new Point(4, 0x1c);
            this.lstVFunds.Name = "lstVFunds";
            this.lstVFunds.Size = new Size(0x2b3, 0xa9);
            this.lstVFunds.TabIndex = 0;
            this.lstVFunds.TabStop = false;
            this.lstVFunds.UseCompatibleStateImageBehavior = false;
            this.lstVFunds.View = View.Details;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxFunds);
            base.Name = "FundsInfo";
            base.Size = new Size(700, 200);
            base.Load += new EventHandler(this.FundsInfo_Load);
            this.groupBoxFunds.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LstVFundsFill(object _firmInfoResponseVO)
        {
            ImageList list = new ImageList
            {
                ImageSize = new Size(1, 15)
            };
            this.lstVFunds.SmallImageList = list;
            FirmInfoResponseVO evo = (FirmInfoResponseVO)_firmInfoResponseVO;
            string text = string.Empty;
            for (int i = 0; i < evo.CDS.Count; i++)
            {
                Code code = evo.CDS[i];
                text = text + code.CD + "/";
            }
            string str2 = evo.FirmID + "(" + evo.FirmName.Trim() + ")";
            this.lstVFunds.Clear();
            try
            {
                string str3 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_Project");
                string str4 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_ProjectValue");
                string[] strArray = this.fundsItemInfo.m_strItems.Split(new char[] { '|' });
                for (int j = 0; j < strArray.Length; j++)
                {
                    this.lstVFunds.Columns.Add(str3, 200, HorizontalAlignment.Left);
                    this.lstVFunds.Columns.Add(str4, 140, HorizontalAlignment.Right);
                    this.groupBoxFunds.Width = 0x162 * (j + 1);
                    string[] strArray2 = strArray[j].Split(new char[] { ';' });
                    for (int k = 0; k < strArray2.Length; k++)
                    {
                        ListViewItem item = null;
                        if (k < this.lstVFunds.Items.Count)
                        {
                            item = this.lstVFunds.Items[k];
                        }
                        if (strArray2[k].Equals("FirmID"))
                        {
                            ColItemInfo info = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FirmID"];
                            if (item == null)
                            {
                                item = new ListViewItem(info.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info.name);
                            }
                            item.SubItems.Add(str2);
                        }
                        if (strArray2[k].Equals("Jysqy"))
                        {
                            ColItemInfo info2 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Jysqy"];
                            if (item == null)
                            {
                                item = new ListViewItem(info2.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info2.name);
                            }
                            item.SubItems.Add(evo.InitFund.ToString(info2.format));
                        }
                        if (strArray2[k].Equals("HoldGain"))
                        {
                            ColItemInfo info3 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["HoldGain"];
                            if (item == null)
                            {
                                item = new ListViewItem(info3.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info3.name);
                            }
                            item.SubItems.Add(evo.YesterdayBail.ToString(info3.format));
                        }
                        if (strArray2[k].Equals("DynRight"))
                        {
                            ColItemInfo info4 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["DynRight"];
                            if (item == null)
                            {
                                item = new ListViewItem(info4.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info4.name);
                            }
                            item.SubItems.Add(evo.YesterdayFL.ToString(info4.format));
                        }
                        if (strArray2[k].Equals("Margin"))
                        {
                            ColItemInfo info5 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Margin"];
                            if (item == null)
                            {
                                item = new ListViewItem(info5.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info5.name);
                            }
                            item.SubItems.Add(evo.CurrentBail.ToString(info5.format));
                        }
                        if (strArray2[k].Equals("Lfpl"))
                        {
                            ColItemInfo info6 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Lfpl"];
                            if (item == null)
                            {
                                item = new ListViewItem(info6.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info6.name);
                            }
                            item.SubItems.Add(evo.CurrentFL.ToString(info6.format));
                        }
                        if (strArray2[k].Equals("FreezFund"))
                        {
                            ColItemInfo info7 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FreezFund"];
                            if (item == null)
                            {
                                item = new ListViewItem(info7.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info7.name);
                            }
                            item.SubItems.Add(evo.OrderFrozenFund.ToString(info7.format));
                        }
                        if (strArray2[k].Equals("FreezComm"))
                        {
                            ColItemInfo info8 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FreezComm"];
                            if (item == null)
                            {
                                item = new ListViewItem(info8.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info8.name);
                            }
                            item.SubItems.Add(evo.OtherFrozenFund.ToString(info8.format));
                        }
                        if (strArray2[k].Equals("RealFunds"))
                        {
                            ColItemInfo info9 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["RealFunds"];
                            if (item == null)
                            {
                                item = new ListViewItem(info9.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info9.name);
                            }
                            item.ForeColor = Color.Blue;
                            item.SubItems.Add(evo.RealFund.ToString(info9.format));
                        }
                        if (strArray2[k].Equals("Comm"))
                        {
                            ColItemInfo info10 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Comm"];
                            if (item == null)
                            {
                                item = new ListViewItem(info10.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info10.name);
                            }
                            item.SubItems.Add(evo.Fee.ToString(info10.format));
                        }
                        if (strArray2[k].Equals("TransLiqpl"))
                        {
                            ColItemInfo info11 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["TransLiqpl"];
                            if (item == null)
                            {
                                item = new ListViewItem(info11.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info11.name);
                            }
                            item.SubItems.Add(evo.TransferPL.ToString(info11.format));
                        }
                        if (strArray2[k].Equals("AddFund"))
                        {
                            ColItemInfo info12 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["AddFund"];
                            if (item == null)
                            {
                                item = new ListViewItem(info12.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info12.name);
                            }
                            item.SubItems.Add(evo.CurrentRight.ToString(info12.format));
                        }
                        if (strArray2[k].Equals("Ioamt"))
                        {
                            ColItemInfo info13 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Ioamt"];
                            if (item == null)
                            {
                                item = new ListViewItem(info13.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info13.name);
                            }
                            item.SubItems.Add(evo.InOutFund.ToString(info13.format));
                        }
                        if (strArray2[k].Equals("MarginAmt"))
                        {
                            ColItemInfo info14 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["MarginAmt"];
                            if (item == null)
                            {
                                item = new ListViewItem(info14.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info14.name);
                            }
                            item.SubItems.Add(evo.HoldingPL.ToString(info14.format));
                        }
                        if (strArray2[k].Equals("ChkFund"))
                        {
                            ColItemInfo info15 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["ChkFund"];
                            if (item == null)
                            {
                                item = new ListViewItem(info15.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info15.name);
                            }
                            item.SubItems.Add(evo.OtherChange.ToString(info15.format));
                        }
                        if (strArray2[k].Equals("TTlMargin"))
                        {
                            ColItemInfo info16 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["TTlMargin"];
                            if (item == null)
                            {
                                item = new ListViewItem(info16.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info16.name);
                            }
                            item.SubItems.Add(evo.ImpawnFund.ToString(info16.format));
                        }
                        if (strArray2[k].Equals("Status"))
                        {
                            ColItemInfo info17 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Status"];
                            if (item == null)
                            {
                                item = new ListViewItem(info17.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info17.name);
                            }
                            item.SubItems.Add(Global.FirmStatusStrArr[evo.Status]);
                        }
                        if (strArray2[k].Equals("Cur_Open"))
                        {
                            ColItemInfo info18 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Cur_Open"];
                            if (item == null)
                            {
                                item = new ListViewItem(info18.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info18.name);
                            }
                            item.SubItems.Add(evo.CurrentOpen.ToString());
                        }
                        if (strArray2[k].Equals("Virtual_Open"))
                        {
                            ColItemInfo info19 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Virtual_Open"];
                            if (item == null)
                            {
                                item = new ListViewItem(info19.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info19.name);
                            }
                            item.SubItems.Add(evo.CurMHolding.ToString());
                        }
                        if (strArray2[k].Equals("Zjaql"))
                        {
                            ColItemInfo info20 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Zjaql"];
                            if (item == null)
                            {
                                item = new ListViewItem(info20.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info20.name);
                            }
                            double num4 = 0.0;
                            if (evo.CurrentBail <= 0.0)
                            {
                                num4 = 999.99;
                            }
                            else
                            {
                                num4 = ((evo.RealFund + evo.CurrentBail) * 100.0) / evo.CurrentBail;
                            }
                            if (num4 > 999.999)
                            {
                                num4 = 999.99;
                            }
                            else if ((num4 > 99.99) && (num4 < 100.0))
                            {
                                num4 = 99.99;
                            }
                            item.SubItems.Add(num4.ToString(info20.format) + "%");
                        }
                        if (strArray2[k].Equals("Code") && (text.Length > 0))
                        {
                            ColItemInfo info21 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Code"];
                            if (item == null)
                            {
                                item = new ListViewItem(info21.name);
                                this.lstVFunds.Items.Add(item);
                            }
                            else
                            {
                                item.SubItems.Add(info21.name);
                            }
                            item.SubItems.Add(text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private delegate void FillHolding(FirmInfoResponseVO dt);
    }
}
