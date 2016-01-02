namespace FuturesTrade.Gnnt.UI.Modules.HQ
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
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class HQCommodityInfo : UserControl
    {
        private MyButton butKLine;
        private MyButton butMinLine;
        private string commodityID = string.Empty;
        private IContainer components;
        private FillCommDataInfo fillCommDataInfo;
        private Panel pGNCommodit;
        private System.Windows.Forms.Timer HQtimer;
        private Label lbltitle;
        private Label labelAvg;
        private Label labelAvgV;
        private Label labelBid;
        private Label labelBidV;
        private Label labelBidVol;
        private Label labelBidVolV;
        private Label labelChange;
        private Label labelChangeV;
        private Label labelGNInfo;
        private Label labelHigh;
        private Label labelHighV;
        private Label labelLast;
        private Label labelLastV;
        private Label labelLiquidToday;
        private Label labelLiquidTodayV;
        private Label labelLow;
        private Label labelLowV;
        private Label labelOffer;
        private Label labelOfferV;
        private Label labelOfferVol;
        private Label labelOfferVolV;
        private Label labelPrevClear;
        private Label labelPrevClearV;
        private Label labelSpread;
        private Label labelVolToday;
        private Label labelVolTodayV;
        private OperationManager operationManager = OperationManager.GetInstance();

        public HQCommodityInfo()
        {
            this.InitializeComponent();
            this.operationManager.queryCommDataOperation.UpdateCommData = new QueryCommDataOperation.UpdateCommDataCallBack(this.FillCommData);
            this.operationManager.SetHQTimerEnable = new OperationManager.SetHQTimerEnableCallBack(this.SetHQTimerEnable);
            this.CreateHandle();
        }

        private void butKLine_Click(object sender, EventArgs e)
        {
            Global.DisplayKLine("", this.commodityID);
        }

        private void butMinLine_Click(object sender, EventArgs e)
        {
            Global.displayMinLine("", this.commodityID);
        }

        private void CommDataFill(CommData commData)
        {
            string commodityName = commData.CommodityName.Trim();
            this.commodityID = commData.CommodityID;
            CommodityInfo info = (CommodityInfo)TradeDataInfo.CommodityHashtable[commData.CommodityID];
            if (info != null)
            {
                if (commodityName.Length < 2)
                {
                    commodityName = info.CommodityName;
                }
                this.labelGNInfo.Text = commData.CommodityID.Trim() + "(" + commodityName + ")";
                if (this.labelGNInfo.Width > this.pGNCommodit.Width)
                {
                    this.labelGNInfo.Text = commData.CommodityID.Trim();
                }
                int len = 0;
                if (info.Spread < 1.0)
                {
                    len = 2;
                }
                this.labelLastV.ForeColor = Global.ColorSet(commData.Last, commData.PrevClear);
                this.labelLastV.Text = Global.ToString(commData.Last, len);
                this.labelAvgV.ForeColor = Global.ColorSet(commData.Avg, commData.PrevClear);
                this.labelAvgV.Text = Global.ToString(commData.Avg, len);
                this.labelVolTodayV.Text = Global.ToString(commData.VolToday);
                this.labelLiquidTodayV.Text = Global.ToString(commData.TTOpen);
                this.labelPrevClearV.Text = Global.ToString(commData.PrevClear, len);
                string str2 = Global.M_ResourceManager.GetString("TradeStr_MainForm_Price");
                this.labelSpread.Text = string.Concat(new object[] { " ", str2, info.SpreadDown, "–", info.SpreadUp });
                if (commData.Last > 0.0)
                {
                    this.labelChangeV.ForeColor = Global.ColorSet(commData.Last - commData.PrevClear, 0.0);
                    this.labelChangeV.Text = Global.ToString(commData.Last - commData.PrevClear, len);
                }
                else
                {
                    this.labelChangeV.ForeColor = Global.ColorSet(commData.Change, 0.0);
                    this.labelChangeV.Text = Global.ToString(commData.Change, len);
                }
                this.labelHighV.ForeColor = Global.ColorSet(commData.High, commData.PrevClear);
                this.labelHighV.Text = Global.ToString(commData.High, len);
                this.labelLowV.ForeColor = Global.ColorSet(commData.Low, commData.PrevClear);
                this.labelLowV.Text = Global.ToString(commData.Low, len);
                this.labelBidV.ForeColor = Global.ColorSet(commData.Bid, commData.PrevClear);
                this.labelBidV.Text = Global.ToString(commData.Bid, len);
                this.labelBidVolV.Text = Global.ToString((double)commData.BidVol);
                this.labelOfferV.ForeColor = Global.ColorSet(commData.Offer, commData.PrevClear);
                this.labelOfferV.Text = Global.ToString(commData.Offer, len);
                this.labelOfferVolV.Text = Global.ToString((double)commData.OfferVol);
                if (!Tools.StrToBool((string)Global.HTConfig["DispalyBSV"]))
                {
                    this.labelBidVol.Visible = false;
                    this.labelBidVolV.Visible = false;
                    this.labelOfferVol.Visible = false;
                    this.labelOfferVolV.Visible = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillCommData(CommData cData)
        {
            try
            {
                if (cData != null)
                {
                    this.fillCommDataInfo = new FillCommDataInfo(this.CommDataFill);
                    this.HandleCreated();
                    base.Invoke(this.fillCommDataInfo, new object[] { cData });
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void HandleCreated()
        {
            while (!base.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

        private void HQCommodityInfo_Load(object sender, EventArgs e)
        {
            this.labelSpread.Text = "";
            this.SetHQTimerEnable(true);
        }

        private void HQtimer_Tick(object sender, EventArgs e)
        {
            this.operationManager.queryCommDataOperation.RefreshGNTime(this.HQtimer.Interval);
        }

        private void InitializeComponent()
        {
            this.lbltitle = new Label();
            this.components = new Container();
            this.pGNCommodit = new Panel();
            this.butMinLine = new MyButton();
            this.butKLine = new MyButton();
            this.labelSpread = new Label();
            this.labelGNInfo = new Label();
            this.labelBidV = new Label();
            this.labelVolTodayV = new Label();
            this.labelOfferVolV = new Label();
            this.labelChangeV = new Label();
            this.labelOfferVol = new Label();
            this.labelChange = new Label();
            this.labelOfferV = new Label();
            this.labelPrevClearV = new Label();
            this.labelOffer = new Label();
            this.labelPrevClear = new Label();
            this.labelLowV = new Label();
            this.labelAvgV = new Label();
            this.labelLow = new Label();
            this.labelAvg = new Label();
            this.labelHighV = new Label();
            this.labelLastV = new Label();
            this.labelHigh = new Label();
            this.labelBidVolV = new Label();
            this.labelLast = new Label();
            this.labelBidVol = new Label();
            this.labelLiquidTodayV = new Label();
            this.labelBid = new Label();
            this.labelLiquidToday = new Label();
            this.labelVolToday = new Label();
            this.HQtimer = new System.Windows.Forms.Timer(this.components);
            this.pGNCommodit.SuspendLayout();
            base.SuspendLayout();
            this.pGNCommodit.BackColor = Color.Transparent;
            this.pGNCommodit.BackgroundImageLayout = ImageLayout.Stretch;
            this.pGNCommodit.CausesValidation = false;
            this.pGNCommodit.Controls.Add(this.lbltitle);
            this.pGNCommodit.Controls.Add(this.butMinLine);
            this.pGNCommodit.Controls.Add(this.butKLine);
            this.pGNCommodit.Controls.Add(this.labelSpread);
            this.pGNCommodit.Controls.Add(this.labelGNInfo);
            this.pGNCommodit.Controls.Add(this.labelBidV);
            this.pGNCommodit.Controls.Add(this.labelVolTodayV);
            this.pGNCommodit.Controls.Add(this.labelOfferVolV);
            this.pGNCommodit.Controls.Add(this.labelChangeV);
            this.pGNCommodit.Controls.Add(this.labelOfferVol);
            this.pGNCommodit.Controls.Add(this.labelChange);
            this.pGNCommodit.Controls.Add(this.labelOfferV);
            this.pGNCommodit.Controls.Add(this.labelPrevClearV);
            this.pGNCommodit.Controls.Add(this.labelOffer);
            this.pGNCommodit.Controls.Add(this.labelPrevClear);
            this.pGNCommodit.Controls.Add(this.labelLowV);
            this.pGNCommodit.Controls.Add(this.labelAvgV);
            this.pGNCommodit.Controls.Add(this.labelLow);
            this.pGNCommodit.Controls.Add(this.labelAvg);
            this.pGNCommodit.Controls.Add(this.labelHighV);
            this.pGNCommodit.Controls.Add(this.labelLastV);
            this.pGNCommodit.Controls.Add(this.labelHigh);
            this.pGNCommodit.Controls.Add(this.labelBidVolV);
            this.pGNCommodit.Controls.Add(this.labelLast);
            this.pGNCommodit.Controls.Add(this.labelBidVol);
            this.pGNCommodit.Controls.Add(this.labelLiquidTodayV);
            this.pGNCommodit.Controls.Add(this.labelBid);
            this.pGNCommodit.Controls.Add(this.labelLiquidToday);
            this.pGNCommodit.Controls.Add(this.labelVolToday);
            this.pGNCommodit.Dock = DockStyle.Fill;
            this.pGNCommodit.Location = new Point(0, 0);
            this.pGNCommodit.Name = "pGNCommodit";
            this.pGNCommodit.Size = new Size(180,240);
            this.pGNCommodit.TabIndex = 0;
            this.pGNCommodit.TabStop = false;
            this.pGNCommodit.BackColor = Color.FromArgb(235, 235, 235);
            //this.groupBoxGNCommodit.Text = "行情";
            ///this.pGNCommodit.BackColor = Color.FromArgb(235, 235, 235);
            this.lbltitle.Location = new Point(this.pGNCommodit.Left, this.pGNCommodit.Top);
            this.lbltitle.AutoSize = false;
            this.lbltitle.Height = 25;
            this.lbltitle.Width = this.pGNCommodit.Width;
            this.lbltitle.Text = "  行情";
            this.lbltitle.ForeColor = Color.Black;
            this.lbltitle.Font = new Font("微软雅黑",12);
            this.lbltitle.BackColor = Color.FromArgb(239, 227, 199);
            //this.butMinLine.BackColor = Color.LightSteelBlue;
            this.butMinLine.FlatStyle = FlatStyle.Popup;
            this.butMinLine.ImeMode = ImeMode.NoControl;
            this.butMinLine.Location = new Point(100, 210+10);
            this.butMinLine.Name = "butMinLine";
            this.butMinLine.Size = new Size(0x2d, 0x15);
            this.butMinLine.TabIndex = 0x21;
            this.butMinLine.TabStop = false;
            this.butMinLine.Text = "分时";
            //this.butMinLine.ForeColor = Color.FromArgb(235, 235, 235);
            this.butMinLine.UseVisualStyleBackColor = false;
            this.butMinLine.Click += new EventHandler(this.butMinLine_Click);
            //this.butKLine.BackColor = Color.LightSteelBlue;
            this.butKLine.FlatStyle = FlatStyle.Popup;
            this.butKLine.ImeMode = ImeMode.NoControl;
            this.butKLine.Location = new Point(30, 210+10);
            this.butKLine.Name = "butKLine";
            this.butKLine.Size = new Size(0x2d, 0x15);
            this.butKLine.TabIndex = 0x20;
            this.butKLine.TabStop = false;
            //this.butKLine.ForeColor = Color.FromArgb(235, 235, 235);
            this.butKLine.Text = "k线";//标记按钮
            this.butKLine.UseVisualStyleBackColor = false;
            this.butKLine.Click += new EventHandler(this.butKLine_Click);
            this.labelSpread.AutoSize = true;
            this.labelSpread.ForeColor = Color.DodgerBlue;
            this.labelSpread.ImeMode = ImeMode.NoControl;
            this.labelSpread.Location = new Point(5, 0xb9+10);
            this.labelSpread.Name = "labelSpread";
            this.labelSpread.Size = new Size(0x8f, 12);
            this.labelSpread.TabIndex = 0x1f;
            this.labelSpread.Text = "价格区间：111111-111111";
            this.labelGNInfo.AutoSize = true;
            this.labelGNInfo.Font = new Font("宋体", 9f, FontStyle.Bold);
            this.labelGNInfo.ForeColor = Color.Blue;
            this.labelGNInfo.ImeMode = ImeMode.NoControl;
            this.labelGNInfo.Location = new Point(7, 0x13+10);
            this.labelGNInfo.Name = "labelGNInfo";
            this.labelGNInfo.Size = new Size(0x39, 12);
            this.labelGNInfo.TabIndex = 0x1d;
            this.labelGNInfo.Text = "商品信息";
            this.labelBidV.AutoSize = true;
            this.labelBidV.Cursor = Cursors.Hand;
            this.labelBidV.ForeColor = SystemColors.ControlText;
            this.labelBidV.ImeMode = ImeMode.NoControl;
            this.labelBidV.Location = new Point(0x23, 0xa5+10);
            this.labelBidV.Name = "labelBidV";
            this.labelBidV.Size = new Size(0x11, 12);
            this.labelBidV.TabIndex = 0x1a;
            this.labelBidV.Text = "--";
            this.labelBidV.Click += new EventHandler(this.labelBidV_Click);
            this.labelVolTodayV.AutoSize = true;
            this.labelVolTodayV.ImeMode = ImeMode.NoControl;
            this.labelVolTodayV.Location = new Point(0x23, 0x41+10);
            this.labelVolTodayV.Name = "labelVolTodayV";
            this.labelVolTodayV.Size = new Size(0x11, 12);
            this.labelVolTodayV.TabIndex = 0x1b;
            this.labelVolTodayV.Text = "--";
            this.labelOfferVolV.AutoSize = true;
            this.labelOfferVolV.Cursor = Cursors.Hand;
            this.labelOfferVolV.ImeMode = ImeMode.NoControl;
            this.labelOfferVolV.Location = new Point(120, 140+10);
            this.labelOfferVolV.Name = "labelOfferVolV";
            this.labelOfferVolV.Size = new Size(0x11, 12);
            this.labelOfferVolV.TabIndex = 0x13;
            this.labelOfferVolV.Text = "--";
            this.labelOfferVolV.Click += new EventHandler(this.labelOfferVolV_Click);
            this.labelChangeV.AutoSize = true;
            this.labelChangeV.ImeMode = ImeMode.NoControl;
            this.labelChangeV.Location = new Point(120, 90+10);
            this.labelChangeV.Name = "labelChangeV";
            this.labelChangeV.Size = new Size(0x11, 12);
            this.labelChangeV.TabIndex = 20;
            this.labelChangeV.Text = "--";
            this.labelChangeV.TextAlign = ContentAlignment.MiddleCenter;
            this.labelOfferVol.AutoSize = true;
            this.labelOfferVol.ImeMode = ImeMode.NoControl;
            this.labelOfferVol.Location = new Point(90, 140+10);
            this.labelOfferVol.Name = "labelOfferVol";
            this.labelOfferVol.Size = new Size(0x1d, 12);
            this.labelOfferVol.TabIndex = 0x11;
            this.labelOfferVol.Text = "卖量";
            this.labelChange.AutoSize = true;
            this.labelChange.ImeMode = ImeMode.NoControl;
            this.labelChange.Location = new Point(90, 90+10);
            this.labelChange.Name = "labelChange";
            this.labelChange.Size = new Size(0x1d, 12);
            this.labelChange.TabIndex = 0x12;
            this.labelChange.Text = "涨跌";
            this.labelOfferV.AutoSize = true;
            this.labelOfferV.Cursor = Cursors.Hand;
            this.labelOfferV.ImeMode = ImeMode.NoControl;
            this.labelOfferV.Location = new Point(0x23, 140+10);
            this.labelOfferV.Name = "labelOfferV";
            this.labelOfferV.Size = new Size(0x11, 12);
            this.labelOfferV.TabIndex = 0x15;
            this.labelOfferV.Text = "--";
            this.labelOfferV.Click += new EventHandler(this.labelOfferV_Click);
            this.labelPrevClearV.AutoSize = true;
            this.labelPrevClearV.ImeMode = ImeMode.NoControl;
            this.labelPrevClearV.Location = new Point(0x23, 90+10);
            this.labelPrevClearV.Name = "labelPrevClearV";
            this.labelPrevClearV.Size = new Size(0x11, 12);
            this.labelPrevClearV.TabIndex = 0x18;
            this.labelPrevClearV.Text = "--";
            this.labelOffer.AutoSize = true;
            this.labelOffer.ImeMode = ImeMode.NoControl;
            this.labelOffer.Location = new Point(5, 140+10);
            this.labelOffer.Name = "labelOffer";
            this.labelOffer.Size = new Size(0x1d, 12);
            this.labelOffer.TabIndex = 0x19;
            this.labelOffer.Text = "卖价";
            this.labelPrevClear.AutoSize = true;
            this.labelPrevClear.ImeMode = ImeMode.NoControl;
            this.labelPrevClear.Location = new Point(5, 90+10);
            this.labelPrevClear.Name = "labelPrevClear";
            this.labelPrevClear.Size = new Size(0x1d, 12);
            this.labelPrevClear.TabIndex = 0x16;
            this.labelPrevClear.Text = "昨结";
            this.labelLowV.AutoSize = true;
            this.labelLowV.ImeMode = ImeMode.NoControl;
            this.labelLowV.Location = new Point(120, 0x73+10);
            this.labelLowV.Name = "labelLowV";
            this.labelLowV.Size = new Size(0x11, 12);
            this.labelLowV.TabIndex = 0x17;
            this.labelLowV.Text = "--";
            this.labelAvgV.AutoSize = true;
            this.labelAvgV.ImeMode = ImeMode.NoControl;
            this.labelAvgV.Location = new Point(120, 40+10);
            this.labelAvgV.Name = "labelAvgV";
            this.labelAvgV.Size = new Size(0x11, 12);
            this.labelAvgV.TabIndex = 7;
            this.labelAvgV.Text = "--";
            this.labelLow.AutoSize = true;
            this.labelLow.ImeMode = ImeMode.NoControl;
            this.labelLow.Location = new Point(90, 0x73+10);
            this.labelLow.Name = "labelLow";
            this.labelLow.Size = new Size(0x1d, 12);
            this.labelLow.TabIndex = 8;
            this.labelLow.Text = "最低";
            this.labelAvg.AutoSize = true;
            this.labelAvg.ImeMode = ImeMode.NoControl;
            this.labelAvg.Location = new Point(90, 40+10);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new Size(0x1d, 12);
            this.labelAvg.TabIndex = 9;
            this.labelAvg.Text = "平均";
            this.labelHighV.AutoSize = true;
            this.labelHighV.ImeMode = ImeMode.NoControl;
            this.labelHighV.Location = new Point(0x23, 0x73+10);
            this.labelHighV.Name = "labelHighV";
            this.labelHighV.Size = new Size(0x11, 12);
            this.labelHighV.TabIndex = 4;
            this.labelHighV.Text = "--";
            this.labelLastV.AutoSize = true;
            this.labelLastV.Cursor = Cursors.Hand;
            this.labelLastV.ImeMode = ImeMode.NoControl;
            this.labelLastV.Location = new Point(0x23, 40+10);
            this.labelLastV.Name = "labelLastV";
            this.labelLastV.Size = new Size(0x11, 12);
            this.labelLastV.TabIndex = 5;
            this.labelLastV.Text = "--";
            this.labelLastV.Click += new EventHandler(this.labelLastV_Click);
            this.labelHigh.AutoSize = true;
            this.labelHigh.ImeMode = ImeMode.NoControl;
            this.labelHigh.Location = new Point(5, 0x73+10);
            this.labelHigh.Name = "labelHigh";
            this.labelHigh.Size = new Size(0x1d, 12);
            this.labelHigh.TabIndex = 6;
            this.labelHigh.Text = "最高";
            this.labelBidVolV.AutoSize = true;
            this.labelBidVolV.Cursor = Cursors.Hand;
            this.labelBidVolV.ImeMode = ImeMode.NoControl;
            this.labelBidVolV.Location = new Point(120, 0xa5+10);
            this.labelBidVolV.Name = "labelBidVolV";
            this.labelBidVolV.Size = new Size(0x11, 12);
            this.labelBidVolV.TabIndex = 10;
            this.labelBidVolV.Text = "--";
            this.labelBidVolV.Click += new EventHandler(this.labelBidVolV_Click);
            this.labelLast.AutoSize = true;
            this.labelLast.ImeMode = ImeMode.NoControl;
            this.labelLast.Location = new Point(5, 40+10);
            this.labelLast.Name = "labelLast";
            this.labelLast.Size = new Size(0x1d, 12);
            this.labelLast.TabIndex = 14;
            this.labelLast.Text = "最新";
            this.labelBidVol.AutoSize = true;
            this.labelBidVol.ImeMode = ImeMode.NoControl;
            this.labelBidVol.Location = new Point(90, 0xa5+10);
            this.labelBidVol.Name = "labelBidVol";
            this.labelBidVol.Size = new Size(0x1d, 12);
            this.labelBidVol.TabIndex = 15;
            this.labelBidVol.Text = "买量";
            this.labelLiquidTodayV.AutoSize = true;
            this.labelLiquidTodayV.ImeMode = ImeMode.NoControl;
            this.labelLiquidTodayV.Location = new Point(120, 0x41+10);
            this.labelLiquidTodayV.Name = "labelLiquidTodayV";
            this.labelLiquidTodayV.Size = new Size(0x11, 12);
            this.labelLiquidTodayV.TabIndex = 0x10;
            this.labelLiquidTodayV.Text = "--";
            this.labelBid.AutoSize = true;
            this.labelBid.ImeMode = ImeMode.NoControl;
            this.labelBid.Location = new Point(5, 0xa5+10);
            this.labelBid.Name = "labelBid";
            this.labelBid.Size = new Size(0x1d, 12);
            this.labelBid.TabIndex = 11;
            this.labelBid.Text = "买价";
            this.labelLiquidToday.AutoSize = true;
            this.labelLiquidToday.ImeMode = ImeMode.NoControl;
            this.labelLiquidToday.Location = new Point(90, 0x41+10);
            this.labelLiquidToday.Name = "labelLiquidToday";
            this.labelLiquidToday.Size = new Size(0x1d, 12);
            this.labelLiquidToday.TabIndex = 12;
            this.labelLiquidToday.Text = "订货";
            this.labelVolToday.AutoSize = true;
            this.labelVolToday.ImeMode = ImeMode.NoControl;
            this.labelVolToday.Location = new Point(5, 0x41+10);
            this.labelVolToday.Name = "labelVolToday";
            this.labelVolToday.Size = new Size(0x1d, 12);
            this.labelVolToday.TabIndex = 13;
            this.labelVolToday.Text = "成交";
            this.HQtimer.Tick += new EventHandler(this.HQtimer_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pGNCommodit);
            base.Name = "HQCommodityInfo";
            base.Size = new Size(180, 240);
            base.Load += new EventHandler(this.HQCommodityInfo_Load);
            this.pGNCommodit.ResumeLayout(false);
            this.pGNCommodit.PerformLayout();
           
            base.ResumeLayout(false);
        }

        private void labelBidV_Click(object sender, EventArgs e)
        {
            if (this.operationManager.TransferInfo != null)
            {
                this.operationManager.TransferInfo(this.labelBidV.Text, 0);
            }
        }

        private void labelBidVolV_Click(object sender, EventArgs e)
        {
            if (this.operationManager.TransferInfo != null)
            {
                this.operationManager.TransferInfo(this.labelBidVolV.Text, 1);
            }
        }

        private void labelLastV_Click(object sender, EventArgs e)
        {
            if (this.operationManager.TransferInfo != null)
            {
                this.operationManager.TransferInfo(this.labelLastV.Text, 0);
            }
        }

        private void labelOfferV_Click(object sender, EventArgs e)
        {
            if (this.operationManager.TransferInfo != null)
            {
                this.operationManager.TransferInfo(this.labelOfferV.Text, 0);
            }
        }

        private void labelOfferVolV_Click(object sender, EventArgs e)
        {
            if (this.operationManager.TransferInfo != null)
            {
                this.operationManager.TransferInfo(this.labelOfferVolV.Text, 1);
            }
        }

        private void SetHQTimerEnable(bool enable)
        {
            this.HQtimer.Enabled = enable;
        }

        private delegate void FillCommDataInfo(CommData commData);
    }
}
