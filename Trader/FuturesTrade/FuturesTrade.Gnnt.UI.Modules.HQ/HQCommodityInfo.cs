using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Modules.HQ
{
	public class HQCommodityInfo : UserControl
	{
		private delegate void FillCommDataInfo(CommData commData);
		private IContainer components;
		private GroupBox groupBoxGNCommodit;
		private Button butMinLine;
		private Button butKLine;
		private Label labelSpread;
		private Label labelGNInfo;
		private Label labelBidV;
		private Label labelVolTodayV;
		private Label labelOfferVolV;
		private Label labelChangeV;
		private Label labelOfferVol;
		private Label labelChange;
		private Label labelOfferV;
		private Label labelPrevClearV;
		private Label labelOffer;
		private Label labelPrevClear;
		private Label labelLowV;
		private Label labelAvgV;
		private Label labelLow;
		private Label labelAvg;
		private Label labelHighV;
		private Label labelLastV;
		private Label labelHigh;
		private Label labelBidVolV;
		private Label labelLast;
		private Label labelBidVol;
		private Label labelLiquidTodayV;
		private Label labelBid;
		private Label labelLiquidToday;
		private Label labelVolToday;
		private System.Windows.Forms.Timer HQtimer;
		private OperationManager operationManager = OperationManager.GetInstance();
		private HQCommodityInfo.FillCommDataInfo fillCommDataInfo;
		private string commodityID = string.Empty;
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.groupBoxGNCommodit = new GroupBox();
			this.butMinLine = new Button();
			this.butKLine = new Button();
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
			this.groupBoxGNCommodit.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxGNCommodit.BackColor = Color.Transparent;
			this.groupBoxGNCommodit.BackgroundImageLayout = ImageLayout.Stretch;
			this.groupBoxGNCommodit.CausesValidation = false;
			this.groupBoxGNCommodit.Controls.Add(this.butMinLine);
			this.groupBoxGNCommodit.Controls.Add(this.butKLine);
			this.groupBoxGNCommodit.Controls.Add(this.labelSpread);
			this.groupBoxGNCommodit.Controls.Add(this.labelGNInfo);
			this.groupBoxGNCommodit.Controls.Add(this.labelBidV);
			this.groupBoxGNCommodit.Controls.Add(this.labelVolTodayV);
			this.groupBoxGNCommodit.Controls.Add(this.labelOfferVolV);
			this.groupBoxGNCommodit.Controls.Add(this.labelChangeV);
			this.groupBoxGNCommodit.Controls.Add(this.labelOfferVol);
			this.groupBoxGNCommodit.Controls.Add(this.labelChange);
			this.groupBoxGNCommodit.Controls.Add(this.labelOfferV);
			this.groupBoxGNCommodit.Controls.Add(this.labelPrevClearV);
			this.groupBoxGNCommodit.Controls.Add(this.labelOffer);
			this.groupBoxGNCommodit.Controls.Add(this.labelPrevClear);
			this.groupBoxGNCommodit.Controls.Add(this.labelLowV);
			this.groupBoxGNCommodit.Controls.Add(this.labelAvgV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLow);
			this.groupBoxGNCommodit.Controls.Add(this.labelAvg);
			this.groupBoxGNCommodit.Controls.Add(this.labelHighV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLastV);
			this.groupBoxGNCommodit.Controls.Add(this.labelHigh);
			this.groupBoxGNCommodit.Controls.Add(this.labelBidVolV);
			this.groupBoxGNCommodit.Controls.Add(this.labelLast);
			this.groupBoxGNCommodit.Controls.Add(this.labelBidVol);
			this.groupBoxGNCommodit.Controls.Add(this.labelLiquidTodayV);
			this.groupBoxGNCommodit.Controls.Add(this.labelBid);
			this.groupBoxGNCommodit.Controls.Add(this.labelLiquidToday);
			this.groupBoxGNCommodit.Controls.Add(this.labelVolToday);
			this.groupBoxGNCommodit.Dock = DockStyle.Fill;
			this.groupBoxGNCommodit.Location = new Point(0, 0);
			this.groupBoxGNCommodit.Name = "groupBoxGNCommodit";
			this.groupBoxGNCommodit.Size = new Size(180, 240);
			this.groupBoxGNCommodit.TabIndex = 0;
			this.groupBoxGNCommodit.TabStop = false;
			this.groupBoxGNCommodit.Text = "行情";
			this.butMinLine.BackColor = Color.LightSteelBlue;
			this.butMinLine.FlatStyle = FlatStyle.Popup;
			this.butMinLine.ImeMode = ImeMode.NoControl;
			this.butMinLine.Location = new Point(100, 210);
			this.butMinLine.Name = "butMinLine";
			this.butMinLine.Size = new Size(45, 21);
			this.butMinLine.TabIndex = 33;
			this.butMinLine.TabStop = false;
			this.butMinLine.Text = "分时";
			this.butMinLine.UseVisualStyleBackColor = false;
			this.butMinLine.Click += new EventHandler(this.butMinLine_Click);
			this.butKLine.BackColor = Color.LightSteelBlue;
			this.butKLine.FlatStyle = FlatStyle.Popup;
			this.butKLine.ImeMode = ImeMode.NoControl;
			this.butKLine.Location = new Point(30, 210);
			this.butKLine.Name = "butKLine";
			this.butKLine.Size = new Size(45, 21);
			this.butKLine.TabIndex = 32;
			this.butKLine.TabStop = false;
			this.butKLine.Text = "k线";
			this.butKLine.UseVisualStyleBackColor = false;
			this.butKLine.Click += new EventHandler(this.butKLine_Click);
			this.labelSpread.AutoSize = true;
			this.labelSpread.ForeColor = Color.DodgerBlue;
			this.labelSpread.ImeMode = ImeMode.NoControl;
			this.labelSpread.Location = new Point(5, 185);
			this.labelSpread.Name = "labelSpread";
			this.labelSpread.Size = new Size(143, 12);
			this.labelSpread.TabIndex = 31;
			this.labelSpread.Text = "价格区间：111111-111111";
			this.labelGNInfo.AutoSize = true;
			this.labelGNInfo.Font = new Font("宋体", 9f, FontStyle.Bold);
			this.labelGNInfo.ForeColor = Color.Blue;
			this.labelGNInfo.ImeMode = ImeMode.NoControl;
			this.labelGNInfo.Location = new Point(7, 19);
			this.labelGNInfo.Name = "labelGNInfo";
			this.labelGNInfo.Size = new Size(57, 12);
			this.labelGNInfo.TabIndex = 29;
			this.labelGNInfo.Text = "商品信息";
			this.labelBidV.AutoSize = true;
			this.labelBidV.Cursor = Cursors.Hand;
			this.labelBidV.ForeColor = SystemColors.ControlText;
			this.labelBidV.ImeMode = ImeMode.NoControl;
			this.labelBidV.Location = new Point(35, 165);
			this.labelBidV.Name = "labelBidV";
			this.labelBidV.Size = new Size(17, 12);
			this.labelBidV.TabIndex = 26;
			this.labelBidV.Text = "--";
			this.labelBidV.Click += new EventHandler(this.labelBidV_Click);
			this.labelVolTodayV.AutoSize = true;
			this.labelVolTodayV.ImeMode = ImeMode.NoControl;
			this.labelVolTodayV.Location = new Point(35, 65);
			this.labelVolTodayV.Name = "labelVolTodayV";
			this.labelVolTodayV.Size = new Size(17, 12);
			this.labelVolTodayV.TabIndex = 27;
			this.labelVolTodayV.Text = "--";
			this.labelOfferVolV.AutoSize = true;
			this.labelOfferVolV.Cursor = Cursors.Hand;
			this.labelOfferVolV.ImeMode = ImeMode.NoControl;
			this.labelOfferVolV.Location = new Point(120, 140);
			this.labelOfferVolV.Name = "labelOfferVolV";
			this.labelOfferVolV.Size = new Size(17, 12);
			this.labelOfferVolV.TabIndex = 19;
			this.labelOfferVolV.Text = "--";
			this.labelOfferVolV.Click += new EventHandler(this.labelOfferVolV_Click);
			this.labelChangeV.AutoSize = true;
			this.labelChangeV.ImeMode = ImeMode.NoControl;
			this.labelChangeV.Location = new Point(120, 90);
			this.labelChangeV.Name = "labelChangeV";
			this.labelChangeV.Size = new Size(17, 12);
			this.labelChangeV.TabIndex = 20;
			this.labelChangeV.Text = "--";
			this.labelChangeV.TextAlign = ContentAlignment.MiddleCenter;
			this.labelOfferVol.AutoSize = true;
			this.labelOfferVol.ImeMode = ImeMode.NoControl;
			this.labelOfferVol.Location = new Point(90, 140);
			this.labelOfferVol.Name = "labelOfferVol";
			this.labelOfferVol.Size = new Size(29, 12);
			this.labelOfferVol.TabIndex = 17;
			this.labelOfferVol.Text = "卖量";
			this.labelChange.AutoSize = true;
			this.labelChange.ImeMode = ImeMode.NoControl;
			this.labelChange.Location = new Point(90, 90);
			this.labelChange.Name = "labelChange";
			this.labelChange.Size = new Size(29, 12);
			this.labelChange.TabIndex = 18;
			this.labelChange.Text = "涨跌";
			this.labelOfferV.AutoSize = true;
			this.labelOfferV.Cursor = Cursors.Hand;
			this.labelOfferV.ImeMode = ImeMode.NoControl;
			this.labelOfferV.Location = new Point(35, 140);
			this.labelOfferV.Name = "labelOfferV";
			this.labelOfferV.Size = new Size(17, 12);
			this.labelOfferV.TabIndex = 21;
			this.labelOfferV.Text = "--";
			this.labelOfferV.Click += new EventHandler(this.labelOfferV_Click);
			this.labelPrevClearV.AutoSize = true;
			this.labelPrevClearV.ImeMode = ImeMode.NoControl;
			this.labelPrevClearV.Location = new Point(35, 90);
			this.labelPrevClearV.Name = "labelPrevClearV";
			this.labelPrevClearV.Size = new Size(17, 12);
			this.labelPrevClearV.TabIndex = 24;
			this.labelPrevClearV.Text = "--";
			this.labelOffer.AutoSize = true;
			this.labelOffer.ImeMode = ImeMode.NoControl;
			this.labelOffer.Location = new Point(5, 140);
			this.labelOffer.Name = "labelOffer";
			this.labelOffer.Size = new Size(29, 12);
			this.labelOffer.TabIndex = 25;
			this.labelOffer.Text = "卖价";
			this.labelPrevClear.AutoSize = true;
			this.labelPrevClear.ImeMode = ImeMode.NoControl;
			this.labelPrevClear.Location = new Point(5, 90);
			this.labelPrevClear.Name = "labelPrevClear";
			this.labelPrevClear.Size = new Size(29, 12);
			this.labelPrevClear.TabIndex = 22;
			this.labelPrevClear.Text = "昨结";
			this.labelLowV.AutoSize = true;
			this.labelLowV.ImeMode = ImeMode.NoControl;
			this.labelLowV.Location = new Point(120, 115);
			this.labelLowV.Name = "labelLowV";
			this.labelLowV.Size = new Size(17, 12);
			this.labelLowV.TabIndex = 23;
			this.labelLowV.Text = "--";
			this.labelAvgV.AutoSize = true;
			this.labelAvgV.ImeMode = ImeMode.NoControl;
			this.labelAvgV.Location = new Point(120, 40);
			this.labelAvgV.Name = "labelAvgV";
			this.labelAvgV.Size = new Size(17, 12);
			this.labelAvgV.TabIndex = 7;
			this.labelAvgV.Text = "--";
			this.labelLow.AutoSize = true;
			this.labelLow.ImeMode = ImeMode.NoControl;
			this.labelLow.Location = new Point(90, 115);
			this.labelLow.Name = "labelLow";
			this.labelLow.Size = new Size(29, 12);
			this.labelLow.TabIndex = 8;
			this.labelLow.Text = "最低";
			this.labelAvg.AutoSize = true;
			this.labelAvg.ImeMode = ImeMode.NoControl;
			this.labelAvg.Location = new Point(90, 40);
			this.labelAvg.Name = "labelAvg";
			this.labelAvg.Size = new Size(29, 12);
			this.labelAvg.TabIndex = 9;
			this.labelAvg.Text = "平均";
			this.labelHighV.AutoSize = true;
			this.labelHighV.ImeMode = ImeMode.NoControl;
			this.labelHighV.Location = new Point(35, 115);
			this.labelHighV.Name = "labelHighV";
			this.labelHighV.Size = new Size(17, 12);
			this.labelHighV.TabIndex = 4;
			this.labelHighV.Text = "--";
			this.labelLastV.AutoSize = true;
			this.labelLastV.Cursor = Cursors.Hand;
			this.labelLastV.ImeMode = ImeMode.NoControl;
			this.labelLastV.Location = new Point(35, 40);
			this.labelLastV.Name = "labelLastV";
			this.labelLastV.Size = new Size(17, 12);
			this.labelLastV.TabIndex = 5;
			this.labelLastV.Text = "--";
			this.labelLastV.Click += new EventHandler(this.labelLastV_Click);
			this.labelHigh.AutoSize = true;
			this.labelHigh.ImeMode = ImeMode.NoControl;
			this.labelHigh.Location = new Point(5, 115);
			this.labelHigh.Name = "labelHigh";
			this.labelHigh.Size = new Size(29, 12);
			this.labelHigh.TabIndex = 6;
			this.labelHigh.Text = "最高";
			this.labelBidVolV.AutoSize = true;
			this.labelBidVolV.Cursor = Cursors.Hand;
			this.labelBidVolV.ImeMode = ImeMode.NoControl;
			this.labelBidVolV.Location = new Point(120, 165);
			this.labelBidVolV.Name = "labelBidVolV";
			this.labelBidVolV.Size = new Size(17, 12);
			this.labelBidVolV.TabIndex = 10;
			this.labelBidVolV.Text = "--";
			this.labelBidVolV.Click += new EventHandler(this.labelBidVolV_Click);
			this.labelLast.AutoSize = true;
			this.labelLast.ImeMode = ImeMode.NoControl;
			this.labelLast.Location = new Point(5, 40);
			this.labelLast.Name = "labelLast";
			this.labelLast.Size = new Size(29, 12);
			this.labelLast.TabIndex = 14;
			this.labelLast.Text = "最新";
			this.labelBidVol.AutoSize = true;
			this.labelBidVol.ImeMode = ImeMode.NoControl;
			this.labelBidVol.Location = new Point(90, 165);
			this.labelBidVol.Name = "labelBidVol";
			this.labelBidVol.Size = new Size(29, 12);
			this.labelBidVol.TabIndex = 15;
			this.labelBidVol.Text = "买量";
			this.labelLiquidTodayV.AutoSize = true;
			this.labelLiquidTodayV.ImeMode = ImeMode.NoControl;
			this.labelLiquidTodayV.Location = new Point(120, 65);
			this.labelLiquidTodayV.Name = "labelLiquidTodayV";
			this.labelLiquidTodayV.Size = new Size(17, 12);
			this.labelLiquidTodayV.TabIndex = 16;
			this.labelLiquidTodayV.Text = "--";
			this.labelBid.AutoSize = true;
			this.labelBid.ImeMode = ImeMode.NoControl;
			this.labelBid.Location = new Point(5, 165);
			this.labelBid.Name = "labelBid";
			this.labelBid.Size = new Size(29, 12);
			this.labelBid.TabIndex = 11;
			this.labelBid.Text = "买价";
			this.labelLiquidToday.AutoSize = true;
			this.labelLiquidToday.ImeMode = ImeMode.NoControl;
			this.labelLiquidToday.Location = new Point(90, 65);
			this.labelLiquidToday.Name = "labelLiquidToday";
			this.labelLiquidToday.Size = new Size(29, 12);
			this.labelLiquidToday.TabIndex = 12;
			this.labelLiquidToday.Text = "订货";
			this.labelVolToday.AutoSize = true;
			this.labelVolToday.ImeMode = ImeMode.NoControl;
			this.labelVolToday.Location = new Point(5, 65);
			this.labelVolToday.Name = "labelVolToday";
			this.labelVolToday.Size = new Size(29, 12);
			this.labelVolToday.TabIndex = 13;
			this.labelVolToday.Text = "成交";
			this.HQtimer.Tick += new EventHandler(this.HQtimer_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxGNCommodit);
			base.Name = "HQCommodityInfo";
			base.Size = new Size(180, 240);
			base.Load += new EventHandler(this.HQCommodityInfo_Load);
			this.groupBoxGNCommodit.ResumeLayout(false);
			this.groupBoxGNCommodit.PerformLayout();
			base.ResumeLayout(false);
		}
		public HQCommodityInfo()
		{
			this.InitializeComponent();
			this.operationManager.queryCommDataOperation.UpdateCommData = new QueryCommDataOperation.UpdateCommDataCallBack(this.FillCommData);
			this.operationManager.SetHQTimerEnable = new OperationManager.SetHQTimerEnableCallBack(this.SetHQTimerEnable);
			this.CreateHandle();
		}
		private void FillCommData(CommData cData)
		{
			try
			{
				if (cData != null)
				{
					this.fillCommDataInfo = new HQCommodityInfo.FillCommDataInfo(this.CommDataFill);
					this.HandleCreated();
					base.Invoke(this.fillCommDataInfo, new object[]
					{
						cData
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void CommDataFill(CommData commData)
		{
			string text = commData.CommodityName.Trim();
			this.commodityID = commData.CommodityID;
			CommodityInfo commodityInfo = (CommodityInfo)TradeDataInfo.CommodityHashtable[commData.CommodityID];
			if (commodityInfo == null)
			{
				return;
			}
			if (text.Length < 2)
			{
				text = commodityInfo.CommodityName;
			}
			this.labelGNInfo.Text = commData.CommodityID.Trim() + "(" + text + ")";
			if (this.labelGNInfo.Width > this.groupBoxGNCommodit.Width)
			{
				this.labelGNInfo.Text = commData.CommodityID.Trim();
			}
			int len = 0;
			if (commodityInfo.Spread < 1.0)
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
			string @string = Global.M_ResourceManager.GetString("TradeStr_MainForm_Price");
			this.labelSpread.Text = string.Concat(new object[]
			{
				" ",
				@string,
				commodityInfo.SpreadDown,
				"–",
				commodityInfo.SpreadUp
			});
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
            if (!ToolsLibrary.util.Tools.StrToBool((string)Global.HTConfig["DispalyBSV"]))
			{
				this.labelBidVol.Visible = false;
				this.labelBidVolV.Visible = false;
				this.labelOfferVol.Visible = false;
				this.labelOfferVolV.Visible = false;
			}
		}
		private void HQCommodityInfo_Load(object sender, EventArgs e)
		{
			this.labelSpread.Text = "";
			this.SetHQTimerEnable(true);
		}
		private void SetHQTimerEnable(bool enable)
		{
			this.HQtimer.Enabled = enable;
		}
		private void HQtimer_Tick(object sender, EventArgs e)
		{
			this.operationManager.queryCommDataOperation.RefreshGNTime(this.HQtimer.Interval);
		}
		private void butKLine_Click(object sender, EventArgs e)
		{
			Global.DisplayKLine("", this.commodityID);
		}
		private void butMinLine_Click(object sender, EventArgs e)
		{
			Global.displayMinLine("", this.commodityID);
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
	}
}
