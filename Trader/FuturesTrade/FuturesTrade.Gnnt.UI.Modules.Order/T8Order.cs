using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Order;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using FuturesTrade.Gnnt.UI.Modules.Tools;
namespace FuturesTrade.Gnnt.UI.Modules.Order
{
	public class T8Order : UserControl
	{
		private delegate void UpdateNumericPriceCallBack(double bPrice, double sPrice);
		private delegate void PromptLargestTradeNumCallBack(string text, int colorFlag);
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private IContainer components;
		private Panel panelTop;
		private TextBox textBoxInfo;
		private Label labelAnswer;
		private Panel panelOrder;
		private TextBox textBoxCanNum;
		private Label labelCanTransfer;
		private ComboBox comboCommodity;
		private NumericUpDown numericUpDownNum;
		private NumericUpDown numericUpDownPrice;
		private Label label_OG;
		private Label label_BS;
		private ComboBox comboBoxTransfer;
		private ComboBox comboBoxBuyOrSall;
		private Button buttonSubmit;
		private Label label_Qty;
		private Label label_Price;
		private Label labelPingZhong;
		private Panel panelAnswer;
		private Panel panelPicBt;
		private NumericUpDown numericLPrice;
		private Label labelLPrice;
		private PictureBox pictureBox3;
		private PictureBox pictureBox2;
		private PictureBox pictureBox1;
		private bool isFirstLoad = true;
		private bool buttonClick;
		private int startKeyNum = 1;
		private OperationManager operationManager = OperationManager.GetInstance();
		private T8Order.UpdateNumericPriceCallBack UpdatePrice;
		private T8Order.PromptLargestTradeNumCallBack PromptLargestTradeNum;
		private T8Order.OrderMessageInfoCallBack OrderMessageInfo;
		private byte BtnFlag;
		private int clickNum = 1;
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
			this.panelTop = new Panel();
			this.panelPicBt = new Panel();
			this.numericLPrice = new NumericUpDown();
			this.labelLPrice = new Label();
			this.pictureBox3 = new PictureBox();
			this.pictureBox2 = new PictureBox();
			this.pictureBox1 = new PictureBox();
			this.panelOrder = new Panel();
			this.textBoxCanNum = new TextBox();
			this.labelCanTransfer = new Label();
			this.comboCommodity = new ComboBox();
			this.numericUpDownNum = new NumericUpDown();
			this.numericUpDownPrice = new NumericUpDown();
			this.label_OG = new Label();
			this.label_BS = new Label();
			this.comboBoxTransfer = new ComboBox();
			this.comboBoxBuyOrSall = new ComboBox();
			this.buttonSubmit = new Button();
			this.label_Qty = new Label();
			this.label_Price = new Label();
			this.labelPingZhong = new Label();
			this.panelAnswer = new Panel();
			this.labelAnswer = new Label();
			this.textBoxInfo = new TextBox();
			this.panelTop.SuspendLayout();
			this.panelPicBt.SuspendLayout();
			((ISupportInitialize)this.numericLPrice).BeginInit();
			((ISupportInitialize)this.pictureBox3).BeginInit();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			this.panelOrder.SuspendLayout();
			((ISupportInitialize)this.numericUpDownNum).BeginInit();
			((ISupportInitialize)this.numericUpDownPrice).BeginInit();
			this.panelAnswer.SuspendLayout();
			base.SuspendLayout();
			this.panelTop.BorderStyle = BorderStyle.FixedSingle;
			this.panelTop.Controls.Add(this.panelPicBt);
			this.panelTop.Controls.Add(this.panelOrder);
			this.panelTop.Controls.Add(this.panelAnswer);
			this.panelTop.Dock = DockStyle.Fill;
			this.panelTop.Location = new Point(0, 0);
			this.panelTop.Margin = new Padding(0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new Size(950, 84);
			this.panelTop.TabIndex = 16;
			this.panelPicBt.BorderStyle = BorderStyle.Fixed3D;
			this.panelPicBt.Controls.Add(this.numericLPrice);
			this.panelPicBt.Controls.Add(this.labelLPrice);
			this.panelPicBt.Controls.Add(this.pictureBox3);
			this.panelPicBt.Controls.Add(this.pictureBox2);
			this.panelPicBt.Controls.Add(this.pictureBox1);
			this.panelPicBt.Dock = DockStyle.Fill;
			this.panelPicBt.ForeColor = SystemColors.ControlText;
			this.panelPicBt.Location = new Point(0, 56);
			this.panelPicBt.Name = "panelPicBt";
			this.panelPicBt.Size = new Size(948, 26);
			this.panelPicBt.TabIndex = 4;
			this.numericLPrice.Font = new Font("宋体", 9f);
			this.numericLPrice.Location = new Point(464, 1);
			NumericUpDown arg_354_0 = this.numericLPrice;
			int[] array = new int[4];
			array[0] = 999999;
			arg_354_0.Maximum = new decimal(array);
			this.numericLPrice.Name = "numericLPrice";
			this.numericLPrice.Size = new Size(81, 21);
			this.numericLPrice.TabIndex = 38;
			this.numericLPrice.Visible = false;
			this.labelLPrice.AutoSize = true;
			this.labelLPrice.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.labelLPrice.ImeMode = ImeMode.NoControl;
			this.labelLPrice.Location = new Point(383, 4);
			this.labelLPrice.Name = "labelLPrice";
			this.labelLPrice.Size = new Size(77, 14);
			this.labelLPrice.TabIndex = 39;
			this.labelLPrice.Text = "指定价格：";
			this.labelLPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labelLPrice.Visible = false;
			this.pictureBox3.ImeMode = ImeMode.NoControl;
			this.pictureBox3.Location = new Point(199, 0);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new Size(100, 22);
			this.pictureBox3.TabIndex = 37;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Tag = "市价委托";
			this.pictureBox3.Click += new EventHandler(this.pictureBox1_Click);
			this.pictureBox3.Paint += new PaintEventHandler(this.pictureBox_Paint);
			this.pictureBox2.ImeMode = ImeMode.NoControl;
			this.pictureBox2.Location = new Point(101, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(100, 22);
			this.pictureBox2.TabIndex = 35;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Tag = "预备指令";
			this.pictureBox2.Click += new EventHandler(this.pictureBox1_Click);
			this.pictureBox2.Paint += new PaintEventHandler(this.pictureBox_Paint);
			this.pictureBox1.ImeMode = ImeMode.NoControl;
			this.pictureBox1.Location = new Point(3, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(100, 22);
			this.pictureBox1.TabIndex = 34;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Tag = "正常委托";
			this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
			this.pictureBox1.Paint += new PaintEventHandler(this.pictureBox_Paint);
			this.panelOrder.BorderStyle = BorderStyle.Fixed3D;
			this.panelOrder.Controls.Add(this.textBoxCanNum);
			this.panelOrder.Controls.Add(this.labelCanTransfer);
			this.panelOrder.Controls.Add(this.comboCommodity);
			this.panelOrder.Controls.Add(this.numericUpDownNum);
			this.panelOrder.Controls.Add(this.numericUpDownPrice);
			this.panelOrder.Controls.Add(this.label_OG);
			this.panelOrder.Controls.Add(this.label_BS);
			this.panelOrder.Controls.Add(this.comboBoxTransfer);
			this.panelOrder.Controls.Add(this.comboBoxBuyOrSall);
			this.panelOrder.Controls.Add(this.buttonSubmit);
			this.panelOrder.Controls.Add(this.label_Qty);
			this.panelOrder.Controls.Add(this.label_Price);
			this.panelOrder.Controls.Add(this.labelPingZhong);
			this.panelOrder.Dock = DockStyle.Top;
			this.panelOrder.Location = new Point(0, 26);
			this.panelOrder.Margin = new Padding(0);
			this.panelOrder.Name = "panelOrder";
			this.panelOrder.Size = new Size(948, 30);
			this.panelOrder.TabIndex = 0;
			this.textBoxCanNum.Enabled = false;
			this.textBoxCanNum.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBoxCanNum.ForeColor = SystemColors.ControlText;
			this.textBoxCanNum.Location = new Point(621, 2);
			this.textBoxCanNum.Name = "textBoxCanNum";
			this.textBoxCanNum.ReadOnly = true;
			this.textBoxCanNum.Size = new Size(90, 23);
			this.textBoxCanNum.TabIndex = 24;
			this.textBoxCanNum.Text = "0";
			this.labelCanTransfer.AutoSize = true;
			this.labelCanTransfer.BackColor = Color.Transparent;
			this.labelCanTransfer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.labelCanTransfer.ImeMode = ImeMode.NoControl;
			this.labelCanTransfer.Location = new Point(550, 6);
			this.labelCanTransfer.Name = "labelCanTransfer";
			this.labelCanTransfer.Size = new Size(63, 14);
			this.labelCanTransfer.TabIndex = 23;
			this.labelCanTransfer.Text = "可转让量";
			this.labelCanTransfer.TextAlign = ContentAlignment.MiddleLeft;
			this.comboCommodity.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.comboCommodity.ItemHeight = 14;
			this.comboCommodity.Location = new Point(41, 2);
			this.comboCommodity.MaxLength = 6;
			this.comboCommodity.Name = "comboCommodity";
			this.comboCommodity.Size = new Size(90, 22);
			this.comboCommodity.TabIndex = 1;
			this.comboCommodity.DropDown += new EventHandler(this.comboCommodity_DropDown);
			this.comboCommodity.SelectedIndexChanged += new EventHandler(this.comboCommodity_SelectedIndexChanged);
			this.comboCommodity.TextChanged += new EventHandler(this.comboCommodity_TextChanged);
			this.comboCommodity.KeyDown += new KeyEventHandler(this.comboCommodity_KeyDown);
			this.numericUpDownNum.BackColor = SystemColors.Window;
			this.numericUpDownNum.Font = new Font("宋体", 10.5f);
			this.numericUpDownNum.Location = new Point(756, 2);
			this.numericUpDownNum.Margin = new Padding(0);
			NumericUpDown arg_A31_0 = this.numericUpDownNum;
			int[] array2 = new int[4];
			array2[0] = 999999;
			arg_A31_0.Maximum = new decimal(array2);
			this.numericUpDownNum.Name = "numericUpDownNum";
			this.numericUpDownNum.Size = new Size(90, 23);
			this.numericUpDownNum.TabIndex = 14;
			this.numericUpDownNum.Enter += new EventHandler(this.numericUpDownNum_Enter);
			this.numericUpDownNum.KeyUp += new KeyEventHandler(this.numericUpDownNum_KeyUp);
			this.numericUpDownPrice.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			NumericUpDown arg_ACE_0 = this.numericUpDownPrice;
			int[] array3 = new int[4];
			array3[0] = 10;
			arg_ACE_0.Increment = new decimal(array3);
			this.numericUpDownPrice.Location = new Point(463, 2);
			NumericUpDown arg_B04_0 = this.numericUpDownPrice;
			int[] array4 = new int[4];
			array4[0] = 999999;
			arg_B04_0.Maximum = new decimal(array4);
			this.numericUpDownPrice.Name = "numericUpDownPrice";
			this.numericUpDownPrice.Size = new Size(81, 23);
			this.numericUpDownPrice.TabIndex = 13;
			this.numericUpDownPrice.ValueChanged += new EventHandler(this.numericUpDownPrice_ValueChanged);
			this.numericUpDownPrice.Enter += new EventHandler(this.numericUpDownPrice_Enter);
			this.numericUpDownPrice.KeyUp += new KeyEventHandler(this.numericUpDownPrice_KeyUp);
			this.label_OG.AutoSize = true;
			this.label_OG.BackColor = Color.Transparent;
			this.label_OG.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label_OG.ImageAlign = ContentAlignment.MiddleLeft;
			this.label_OG.ImeMode = ImeMode.NoControl;
			this.label_OG.Location = new Point(264, 6);
			this.label_OG.Name = "label_OG";
			this.label_OG.Size = new Size(35, 14);
			this.label_OG.TabIndex = 20;
			this.label_OG.Text = "订转";
			this.label_OG.TextAlign = ContentAlignment.MiddleLeft;
			this.label_BS.AutoSize = true;
			this.label_BS.BackColor = Color.Transparent;
			this.label_BS.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label_BS.ImageAlign = ContentAlignment.MiddleLeft;
			this.label_BS.ImeMode = ImeMode.NoControl;
			this.label_BS.Location = new Point(136, 6);
			this.label_BS.Margin = new Padding(0);
			this.label_BS.Name = "label_BS";
			this.label_BS.Size = new Size(35, 14);
			this.label_BS.TabIndex = 19;
			this.label_BS.Text = "买卖";
			this.label_BS.TextAlign = ContentAlignment.MiddleLeft;
			this.comboBoxTransfer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.comboBoxTransfer.FormattingEnabled = true;
			this.comboBoxTransfer.Items.AddRange(new object[]
			{
				"1.订立",
				"2.转让",
				"3.转今",
				"4.按价格转让"
			});
			this.comboBoxTransfer.Location = new Point(305, 2);
			this.comboBoxTransfer.Margin = new Padding(0);
			this.comboBoxTransfer.Name = "comboBoxTransfer";
			this.comboBoxTransfer.Size = new Size(110, 22);
			this.comboBoxTransfer.TabIndex = 12;
			this.comboBoxTransfer.SelectedIndexChanged += new EventHandler(this.comboBoxTransfer_SelectedIndexChanged);
			this.comboBoxTransfer.Enter += new EventHandler(this.comboBoxTransfer_Enter);
			this.comboBoxBuyOrSall.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.comboBoxBuyOrSall.FormattingEnabled = true;
			this.comboBoxBuyOrSall.Items.AddRange(new object[]
			{
				"1.卖出",
				"2.买入"
			});
			this.comboBoxBuyOrSall.Location = new Point(179, 2);
			this.comboBoxBuyOrSall.Name = "comboBoxBuyOrSall";
			this.comboBoxBuyOrSall.Size = new Size(81, 22);
			this.comboBoxBuyOrSall.TabIndex = 11;
			this.comboBoxBuyOrSall.DrawItem += new DrawItemEventHandler(this.comboBoxBuyOrSall_DrawItem);
			this.comboBoxBuyOrSall.SelectedIndexChanged += new EventHandler(this.comboBoxBuyOrSall_SelectedIndexChanged);
			this.comboBoxBuyOrSall.TextChanged += new EventHandler(this.comboBoxBuyOrSall_TextChanged);
			this.comboBoxBuyOrSall.Enter += new EventHandler(this.comboBoxBuyOrSall_Enter);
			this.buttonSubmit.BackColor = Color.LightSteelBlue;
			this.buttonSubmit.FlatStyle = FlatStyle.Popup;
			this.buttonSubmit.Font = new Font("宋体", 10.5f);
			this.buttonSubmit.ImeMode = ImeMode.NoControl;
			this.buttonSubmit.Location = new Point(851, 1);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new Size(80, 24);
			this.buttonSubmit.TabIndex = 20;
			this.buttonSubmit.Text = "正常委托";
			this.buttonSubmit.UseVisualStyleBackColor = false;
			this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
			this.label_Qty.AutoSize = true;
			this.label_Qty.BackColor = Color.Transparent;
			this.label_Qty.Font = new Font("宋体", 10.5f);
			this.label_Qty.ImeMode = ImeMode.NoControl;
			this.label_Qty.Location = new Point(716, 6);
			this.label_Qty.Name = "label_Qty";
			this.label_Qty.Size = new Size(35, 14);
			this.label_Qty.TabIndex = 17;
			this.label_Qty.Text = "数量";
			this.label_Qty.TextAlign = ContentAlignment.MiddleLeft;
			this.label_Price.AutoSize = true;
			this.label_Price.BackColor = Color.Transparent;
			this.label_Price.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label_Price.ImeMode = ImeMode.NoControl;
			this.label_Price.Location = new Point(420, 6);
			this.label_Price.Name = "label_Price";
			this.label_Price.Size = new Size(35, 14);
			this.label_Price.TabIndex = 16;
			this.label_Price.Text = "价格";
			this.label_Price.TextAlign = ContentAlignment.MiddleLeft;
			this.labelPingZhong.AutoSize = true;
			this.labelPingZhong.BackColor = Color.Transparent;
			this.labelPingZhong.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.labelPingZhong.ImageAlign = ContentAlignment.MiddleLeft;
			this.labelPingZhong.ImeMode = ImeMode.NoControl;
			this.labelPingZhong.Location = new Point(3, 6);
			this.labelPingZhong.Name = "labelPingZhong";
			this.labelPingZhong.Size = new Size(35, 14);
			this.labelPingZhong.TabIndex = 12;
			this.labelPingZhong.Text = "品种";
			this.labelPingZhong.TextAlign = ContentAlignment.MiddleLeft;
			this.panelAnswer.BorderStyle = BorderStyle.Fixed3D;
			this.panelAnswer.Controls.Add(this.labelAnswer);
			this.panelAnswer.Controls.Add(this.textBoxInfo);
			this.panelAnswer.Dock = DockStyle.Top;
			this.panelAnswer.Location = new Point(0, 0);
			this.panelAnswer.Name = "panelAnswer";
			this.panelAnswer.Size = new Size(948, 26);
			this.panelAnswer.TabIndex = 5;
			this.labelAnswer.AutoSize = true;
			this.labelAnswer.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.labelAnswer.ImeMode = ImeMode.NoControl;
			this.labelAnswer.Location = new Point(3, 5);
			this.labelAnswer.Name = "labelAnswer";
			this.labelAnswer.Size = new Size(35, 14);
			this.labelAnswer.TabIndex = 1;
			this.labelAnswer.Text = "应答";
			this.textBoxInfo.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.textBoxInfo.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBoxInfo.ForeColor = Color.Red;
			this.textBoxInfo.Location = new Point(40, 1);
			this.textBoxInfo.Margin = new Padding(0);
			this.textBoxInfo.Name = "textBoxInfo";
			this.textBoxInfo.ReadOnly = true;
			this.textBoxInfo.Size = new Size(908, 23);
			this.textBoxInfo.TabIndex = 2;
			this.textBoxInfo.TabStop = false;
			this.textBoxInfo.Text = "10:57:02 当前系统正常!";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panelTop);
			base.Margin = new Padding(0);
			base.Name = "T8Order";
			base.Size = new Size(950, 84);
			base.Load += new EventHandler(this.T8Order_Load);
			this.panelTop.ResumeLayout(false);
			this.panelPicBt.ResumeLayout(false);
			this.panelPicBt.PerformLayout();
			((ISupportInitialize)this.numericLPrice).EndInit();
			((ISupportInitialize)this.pictureBox3).EndInit();
			((ISupportInitialize)this.pictureBox2).EndInit();
			((ISupportInitialize)this.pictureBox1).EndInit();
			this.panelOrder.ResumeLayout(false);
			this.panelOrder.PerformLayout();
			((ISupportInitialize)this.numericUpDownNum).EndInit();
			((ISupportInitialize)this.numericUpDownPrice).EndInit();
			this.panelAnswer.ResumeLayout(false);
			this.panelAnswer.PerformLayout();
			base.ResumeLayout(false);
		}
		public T8Order()
		{
			this.InitializeComponent();
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.operationManager.orderOperation.UpdateNumericPrice = new OrderOperation.UpdateNumericPriceCallBack(this.UpdateNumericPrice);
			this.operationManager.orderOperation.setLargestTN = new OrderOperation.SetLargestTNCallBack(this.SetLargestTNInfo);
			this.operationManager.submitOrderOperation.SetFocus = new SubmitOrderOperation.SetFocusCallBack(this.SetFouce);
			this.operationManager.submitOrderOperation.OrderMessage = new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage);
			this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
			this.operationManager.TransferInfo = new OperationManager.TransferInfoCallBack(this.SetPriceQty);
			Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			Global.SetCommoditySelectIndex = new Global.SetCommoditySelectIndexCallBack(this.SetCommoditySelectIndex);
			Global.SetDoubleClickOrderInfo = new Global.SetDoubleClickOrderInfoCallBack(this.SetDoubleClickOrderInfo);
			base.CreateControl();
		}
		private void SetRadioEnable(int currentTradeMode)
		{
			switch (currentTradeMode)
			{
			case 0:
				this.comboBoxBuyOrSall.Enabled = true;
				this.comboBoxTransfer.Enabled = true;
				return;
			case 1:
				this.comboBoxBuyOrSall.Enabled = true;
				this.comboBoxTransfer.Enabled = false;
				if (this.comboBoxBuyOrSall.SelectedIndex == 0)
				{
					int closeMode = IniData.GetInstance().CloseMode;
					if (closeMode == 1)
					{
						this.comboBoxTransfer.SelectedIndex = 1;
						return;
					}
					if (closeMode == 2)
					{
						this.comboBoxTransfer.SelectedIndex = 2;
						return;
					}
					if (closeMode == 3)
					{
						this.comboBoxTransfer.SelectedIndex = 3;
						return;
					}
				}
				else
				{
					if (this.comboBoxBuyOrSall.SelectedIndex == 1)
					{
						this.comboBoxTransfer.SelectedIndex = 0;
						return;
					}
				}
				break;
			case 2:
				this.comboBoxBuyOrSall.Enabled = true;
				this.comboBoxTransfer.Enabled = false;
				if (this.comboBoxBuyOrSall.SelectedIndex == 0)
				{
					this.comboBoxTransfer.SelectedIndex = 0;
					return;
				}
				if (this.comboBoxBuyOrSall.SelectedIndex == 1)
				{
					int closeMode2 = IniData.GetInstance().CloseMode;
					if (closeMode2 == 1)
					{
						this.comboBoxTransfer.SelectedIndex = 1;
						return;
					}
					if (closeMode2 == 2)
					{
						this.comboBoxTransfer.SelectedIndex = 2;
						return;
					}
					if (closeMode2 == 3)
					{
						this.comboBoxTransfer.SelectedIndex = 3;
						return;
					}
				}
				break;
			case 3:
				this.comboBoxBuyOrSall.Enabled = false;
				this.comboBoxBuyOrSall.SelectedIndex = 0;
				this.comboBoxTransfer.Enabled = false;
				this.comboBoxTransfer.SelectedIndex = 0;
				return;
			case 4:
				this.comboBoxBuyOrSall.Enabled = false;
				this.comboBoxBuyOrSall.SelectedIndex = 1;
				this.comboBoxTransfer.Enabled = false;
				this.comboBoxTransfer.SelectedIndex = 0;
				break;
			default:
				return;
			}
		}
		private void comboCommodity_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IniData.GetInstance().AutoPrice)
			{
				this.numericUpDownPrice.Value = 0m;
			}
			this.numericUpDownNum.Value = 0m;
			this.numericLPrice.Value = 0m;
			this.operationManager.orderOperation.SetListBoxVisible(false);
			this.operationManager.orderOperation.ShowMinLine(this.comboCommodity.Text);
			int currentTradeMode = this.operationManager.orderOperation.GetCurrentTradeMode(this.comboCommodity.Text);
			this.SetRadioEnable(currentTradeMode);
		}
		private void comboCommodity_TextChanged(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.IsChangePrice = false;
			this.operationManager.orderOperation.ComboxTextChanged(this.comboCommodity);
			decimal commoditySpread = this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
			this.numericUpDownPrice.Increment = commoditySpread;
			this.numericLPrice.Increment = commoditySpread;
			int decimalPlaces = this.operationManager.orderOperation.GetDecimalPlaces(commoditySpread);
			this.numericUpDownPrice.DecimalPlaces = decimalPlaces;
			this.numericLPrice.DecimalPlaces = decimalPlaces;
		}
		private void comboCommodity_KeyDown(object sender, KeyEventArgs e)
		{
			this.operationManager.orderOperation.ComboxKeyDown(e);
		}
		private void comboCommodity_DropDown(object sender, EventArgs e)
		{
			this.operationManager.orderOperation.SetListBoxVisible(false);
		}
		private void comboBoxBuyOrSall_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.isFirstLoad)
			{
				this.operationManager.orderOperation.IsChangePrice = false;
				int selectedIndex = this.comboBoxBuyOrSall.SelectedIndex;
				decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(selectedIndex);
				this.numericUpDownPrice.Value = bSPrice;
				this.numericUpDownNum_Enter(null, null);
			}
		}
		private void comboBoxBuyOrSall_TextChanged(object sender, EventArgs e)
		{
			this.operationManager.t8OrderOperation.ChangeComboForColor(this.comboBoxBuyOrSall);
		}
		private void comboBoxBuyOrSall_DrawItem(object sender, DrawItemEventArgs e)
		{
			this.operationManager.t8OrderOperation.ComboDrawItem(this.comboBoxBuyOrSall, e);
		}
		private void comboBoxBuyOrSall_Enter(object sender, EventArgs e)
		{
			this.operationManager.t8OrderOperation.KeyTip(1, this.startKeyNum);
		}
		private void comboBoxTransfer_Enter(object sender, EventArgs e)
		{
			this.operationManager.t8OrderOperation.KeyTip(2, this.startKeyNum);
		}
		private void comboBoxTransfer_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.isFirstLoad)
			{
				string @string = Global.M_ResourceManager.GetString("HQStr_Dingli");
				if (this.comboBoxTransfer.SelectedItem != null && this.comboBoxTransfer.SelectedItem.ToString().Contains(@string))
				{
					string string2 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanAmount");
					this.labelCanTransfer.Text = string2;
					this.comboBoxTransfer.SelectedIndex = 0;
					this.labelLPrice.Visible = false;
					this.numericLPrice.Visible = false;
				}
				else
				{
					string string3 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanGrantAmount");
					this.labelCanTransfer.Text = string3;
					if (this.comboBoxTransfer.SelectedIndex == 1)
					{
						IniData.GetInstance().CloseMode = 1;
					}
					else
					{
						if (this.comboBoxTransfer.SelectedIndex == 2)
						{
							IniData.GetInstance().CloseMode = 2;
						}
						else
						{
							IniData.GetInstance().CloseMode = 3;
						}
					}
					switch (IniData.GetInstance().CloseMode)
					{
					case 1:
						this.labelLPrice.Visible = false;
						this.numericLPrice.Visible = false;
						break;
					case 2:
						this.labelLPrice.Visible = false;
						this.numericLPrice.Visible = false;
						break;
					case 3:
						this.labelLPrice.Visible = true;
						this.numericLPrice.Visible = true;
						break;
					default:
						this.labelLPrice.Visible = false;
						this.numericLPrice.Visible = false;
						break;
					}
				}
				this.numericUpDownNum_Enter(null, null);
			}
		}
		private void numericUpDownPrice_Enter(object sender, EventArgs e)
		{
			if (this.numericUpDownPrice.Value == 0m)
			{
				this.numericUpDownPrice.Select(0, this.numericUpDownPrice.Text.Length);
			}
			this.operationManager.orderOperation.GetCommoditySpread(this.comboCommodity.Text);
		}
		private void numericUpDownPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericUpDownNum_Enter(object sender, EventArgs e)
		{
			this.numericUpDownNum.Select(0, this.numericUpDownNum.Value.ToString().Length);
			short num = 2;
			if (this.comboBoxBuyOrSall.SelectedIndex == 0)
			{
				num = 1;
			}
			short num2 = 1;
			if (this.comboBoxTransfer.SelectedIndex != 0)
			{
				num2 = 2;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Commodity", this.comboCommodity.Text);
			hashtable.Add("B_SType", num);
			hashtable.Add("O_LType", num2);
			hashtable.Add("numericPrice", Convert.ToDouble(this.numericUpDownPrice.Value));
			hashtable.Add("tbTranc_comboTranc", Global.FirmID + Global.CustomerID);
			this.operationManager.orderOperation.GetNumericQtyThread(hashtable);
		}
		private void numericUpDownNum_KeyUp(object sender, KeyEventArgs e)
		{
			Global.QtyKeyUp(sender, e);
		}
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;
			this.clickNum = this.operationManager.t8OrderOperation.PictureBoxClick(pictureBox, this.panelPicBt, this.clickNum);
			if (this.clickNum == 2)
			{
				this.BtnFlag = 1;
			}
			else
			{
				this.BtnFlag = 0;
			}
			this.buttonSubmit.Text = pictureBox.Tag.ToString();
			if (this.clickNum == 4)
			{
				this.numericUpDownPrice.Enabled = false;
				return;
			}
			this.numericUpDownPrice.Enabled = true;
		}
		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			PictureBox label = sender as PictureBox;
			this.operationManager.t8OrderOperation.ChangeBorder(e, label, this.clickNum);
		}
		private void buttonSubmit_Click(object sender, EventArgs e)
		{
			this.buttonClick = true;
			SubmitOrderInfo submitOrderInfo = new SubmitOrderInfo();
			submitOrderInfo.customerID = Global.FirmID + Global.CustomerID;
			submitOrderInfo.commodityID = this.comboCommodity.Text;
            submitOrderInfo.B_SType = ToolsLibrary.util.Tools.StrToShort((this.comboBoxBuyOrSall.SelectedIndex + 1).ToString());
			if (this.comboBoxTransfer.SelectedIndex != 0)
			{
				submitOrderInfo.O_LType = 2;
			}
            submitOrderInfo.price = ToolsLibrary.util.Tools.StrToDouble(this.numericUpDownPrice.Value.ToString(), 0.0);
            submitOrderInfo.qty = ToolsLibrary.util.Tools.StrToInt(this.numericUpDownNum.Value.ToString(), 0);
			if (submitOrderInfo.O_LType == 2)
			{
				if (IniData.GetInstance().CloseMode == 2)
				{
					submitOrderInfo.closeMode = 2;
					if (this.comboBoxTransfer.SelectedIndex == 2)
					{
						submitOrderInfo.timeFlag = 1;
					}
					else
					{
						submitOrderInfo.closeMode = 1;
					}
				}
				else
				{
					if (IniData.GetInstance().CloseMode == 3)
					{
						submitOrderInfo.closeMode = 3;
                        submitOrderInfo.lPrice = ToolsLibrary.util.Tools.StrToDouble(this.numericLPrice.Value.ToString(), 0.0);
					}
					else
					{
						submitOrderInfo.closeMode = 1;
					}
				}
			}
			OperationManager.GetInstance().orderOperation.orderType = OrderType.Order;
			this.operationManager.submitOrderOperation.ButtonOrderComm(submitOrderInfo, this.BtnFlag);
		}
		private void numericLPrice_Enter(object sender, EventArgs e)
		{
			this.numericLPrice.Select(0, this.numericLPrice.Text.Length);
		}
		private void numericLPrice_KeyUp(object sender, KeyEventArgs e)
		{
			Global.PriceKeyUp(sender, e);
		}
		private void numericLPrice_MouseDown(object sender, MouseEventArgs e)
		{
			this.numericLPrice.Select(0, this.numericLPrice.Value.ToString().Length);
		}
		private void T8Order_Load(object sender, EventArgs e)
		{
            if (ToolsLibrary.util.Tools.StrToBool((string)Global.HTConfig["UseZeroKey"], false))
			{
				this.startKeyNum = 0;
			}
			this.SetControlText();
			this.ComboLoad();
			this.ComboTrans();
			this.operationManager.orderOperation.SetButtonOrderEnable = new OrderOperation.SetButtonOrderEnableCallBack(this.SetButtonOrderEnable);
			this.isFirstLoad = false;
		}
		private void SetControlText()
		{
			this.numericUpDownNum.Text = "";
			this.comboBoxBuyOrSall.DrawMode = DrawMode.OwnerDrawVariable;
			this.labelAnswer.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Answer");
			this.labelPingZhong.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Variety");
			this.label_BS.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_BS");
			this.label_OG.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_OG");
			this.label_Price.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Price");
			this.labelCanTransfer.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_CanGrantAmount");
			this.label_Qty.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Qty");
			this.buttonSubmit.Text = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-Normal");
			this.labelLPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabelLPrice");
			this.pictureBox1.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-Normal");
			this.pictureBox2.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_embed");
			this.pictureBox3.Tag = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Order-market");
		}
		private void ComboLoad()
		{
			this.comboBoxBuyOrSall.Items.Clear();
			this.comboBoxBuyOrSall.Items.Add(this.startKeyNum + "." + this.operationManager.StrBuy);
			this.comboBoxBuyOrSall.Items.Add(this.startKeyNum + 1 + "." + this.operationManager.StrSale);
			this.comboBoxBuyOrSall.SelectedIndex = 0;
		}
		private void ComboTrans()
		{
			this.comboBoxTransfer.Items.Clear();
			string @string = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1");
			string string2 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2");
			this.comboBoxTransfer.Items.Add(this.startKeyNum + "." + @string);
			this.comboBoxTransfer.Items.Add(this.startKeyNum + 1 + "." + string2);
			string string3 = Global.M_ResourceManager.GetString("TradeStr_TransferToday");
			this.comboBoxTransfer.Items.Add(this.startKeyNum + 2 + "." + string3);
			string string4 = Global.M_ResourceManager.GetString("Global_CloseModeStrArr2");
			this.comboBoxTransfer.Items.Add(this.startKeyNum + 3 + "." + string4);
			this.comboBoxTransfer.SelectedIndex = 0;
		}
		private void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboCommodity.Items.Clear();
			foreach (string current in commodityIDList)
			{
				if (current != this.operationManager.StrAll)
				{
					this.comboCommodity.Items.Add(current);
				}
			}
			this.comboCommodity.SelectedIndex = 0;
			this.comboCommodity.Focus();
		}
		private void SetButtonOrderEnable(bool enable)
		{
			this.buttonSubmit.Enabled = enable;
		}
		private void keyNum(int num)
		{
            if (ToolsLibrary.util.Tools.StrToBool((string)Global.HTConfig["UseZeroKey"], false))
			{
				num++;
			}
			switch (num)
			{
			case 1:
				if (this.comboBoxBuyOrSall.Focused)
				{
					this.comboBoxBuyOrSall.SelectedIndex = 0;
				}
				if (this.comboBoxTransfer.Focused)
				{
					this.comboBoxTransfer.SelectedIndex = 0;
					return;
				}
				break;
			case 2:
				if (this.comboBoxBuyOrSall.Focused)
				{
					this.comboBoxBuyOrSall.SelectedIndex = 1;
				}
				if (this.comboBoxTransfer.Focused)
				{
					this.comboBoxTransfer.SelectedIndex = 1;
					return;
				}
				break;
			case 3:
				if (this.comboBoxTransfer.Focused && this.comboBoxTransfer.Items.Count > 2)
				{
					this.comboBoxTransfer.SelectedIndex = 2;
					return;
				}
				break;
			case 4:
				if (this.comboBoxTransfer.Focused && this.comboBoxTransfer.Items.Count > 2)
				{
					this.comboBoxTransfer.SelectedIndex = 3;
				}
				break;
			default:
				return;
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (this.buttonClick)
			{
				this.buttonClick = false;
				return false;
			}
			if (!IniData.GetInstance().UpDownFocus)
			{
				return false;
			}
			if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
			{
				this.SetFouces(keyData);
				return true;
			}
			if (this.comboBoxBuyOrSall.Focused || this.comboBoxTransfer.Focused)
			{
				if (keyData == Keys.D0 || keyData == Keys.NumPad0)
				{
					this.keyNum(0);
				}
				else
				{
					if (keyData == Keys.D1 || keyData == Keys.NumPad1)
					{
						this.keyNum(1);
					}
					else
					{
						if (keyData == Keys.D2 || keyData == Keys.NumPad2)
						{
							this.keyNum(2);
						}
						else
						{
							if (keyData == Keys.D3 || keyData == Keys.NumPad3)
							{
								this.keyNum(3);
							}
							else
							{
								if (keyData == Keys.D4 || keyData == Keys.NumPad4)
								{
									this.keyNum(4);
								}
								else
								{
									if (keyData == Keys.D5 || keyData == Keys.NumPad5)
									{
										this.keyNum(5);
									}
								}
							}
						}
					}
				}
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void SetFouces(Keys keyData)
		{
			if (keyData == Keys.Left)
			{
				if (this.comboCommodity.Focused)
				{
					this.buttonSubmit.Focus();
					return;
				}
				if (this.comboBoxBuyOrSall.Focused)
				{
					if (IniData.GetInstance().LimitFocus && (this.clickNum == 1 || this.clickNum == 4))
					{
						this.buttonSubmit.Focus();
						return;
					}
					this.comboCommodity.Focus();
					return;
				}
				else
				{
					if (this.comboBoxTransfer.Focused)
					{
						this.comboBoxBuyOrSall.Focus();
						return;
					}
					if (this.numericUpDownPrice.Focused)
					{
						this.comboBoxTransfer.Focus();
						return;
					}
					if (this.numericUpDownNum.Focused)
					{
						if (this.numericUpDownPrice.Enabled)
						{
							this.numericUpDownPrice.Focus();
							return;
						}
						this.comboBoxTransfer.Focus();
						return;
					}
					else
					{
						if (this.buttonSubmit.Focused)
						{
							if (this.numericLPrice.Visible)
							{
								this.numericLPrice.Focus();
								return;
							}
							this.numericUpDownNum.Focus();
							return;
						}
						else
						{
							if (this.numericLPrice.Focused)
							{
								this.numericUpDownNum.Focus();
								return;
							}
						}
					}
				}
			}
			else
			{
				if (keyData == Keys.Right)
				{
					if (this.comboCommodity.Focused)
					{
						this.comboBoxBuyOrSall.Focus();
						return;
					}
					if (this.comboBoxBuyOrSall.Focused)
					{
						this.comboBoxTransfer.Focus();
						return;
					}
					if (this.comboBoxTransfer.Focused)
					{
						if (this.numericUpDownPrice.Enabled)
						{
							this.numericUpDownPrice.Focus();
							return;
						}
						this.numericUpDownNum.Focus();
						return;
					}
					else
					{
						if (this.numericUpDownPrice.Focused)
						{
							this.numericUpDownNum.Focus();
							return;
						}
						if (this.numericUpDownNum.Focused)
						{
							if (this.numericLPrice.Visible)
							{
								this.numericLPrice.Focus();
								return;
							}
							this.buttonSubmit.Focus();
							return;
						}
						else
						{
							if (this.numericLPrice.Focused)
							{
								this.buttonSubmit.Focus();
								return;
							}
							if (this.buttonSubmit.Focused)
							{
								if (IniData.GetInstance().LimitFocus && (this.clickNum == 1 || this.clickNum == 4))
								{
									this.comboBoxBuyOrSall.Focus();
									return;
								}
								this.comboCommodity.Focus();
								return;
							}
						}
					}
				}
				else
				{
					if (this.comboCommodity.Focused)
					{
						this.ChangeComboSelectIndex(keyData, this.comboCommodity);
						return;
					}
					if (this.comboBoxBuyOrSall.Focused)
					{
						this.ChangeComboSelectIndex(keyData, this.comboBoxBuyOrSall);
						return;
					}
					if (this.comboBoxTransfer.Focused)
					{
						this.ChangeComboSelectIndex(keyData, this.comboBoxTransfer);
					}
				}
			}
		}
		private void ChangeComboSelectIndex(Keys keyData, ComboBox combo)
		{
			int num = combo.SelectedIndex;
			if (keyData == Keys.Up)
			{
				num--;
				if (num < 0)
				{
					num = 0;
				}
			}
			else
			{
				if (keyData == Keys.Down)
				{
					num++;
					if (num > combo.Items.Count - 1)
					{
						num = combo.Items.Count - 1;
					}
				}
			}
			combo.SelectedIndex = num;
		}
		private void UpdateNumericPrice(double bPrice, double sPrice)
		{
			try
			{
				this.UpdatePrice = new T8Order.UpdateNumericPriceCallBack(this.SetNumericPrice);
				this.HandleCreated();
				base.Invoke(this.UpdatePrice, new object[]
				{
					bPrice,
					sPrice
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void SetNumericPrice(double bPrice, double sPrice)
		{
			if (this.comboBoxBuyOrSall.SelectedIndex == 0)
			{
				this.numericUpDownPrice.Value = (decimal)sPrice;
			}
			else
			{
				if (this.comboBoxBuyOrSall.SelectedIndex == 1)
				{
					this.numericUpDownPrice.Value = (decimal)bPrice;
				}
			}
			this.numericUpDownNum_Enter(null, null);
		}
		private void SetPriceQty(string info, byte priceQtyFlag)
		{
			if (!string.IsNullOrEmpty(info))
			{
				switch (priceQtyFlag)
				{
				case 0:
					try
					{
						this.numericUpDownPrice.Value = decimal.Parse(info);
						return;
					}
					catch (Exception)
					{
						this.numericUpDownPrice.Value = 0m;
						return;
					}
					break;
				case 1:
					break;
				default:
					return;
				}
				try
				{
					this.numericUpDownNum.Value = decimal.Parse(info);
				}
				catch (Exception)
				{
					this.numericUpDownNum.Value = 0m;
				}
			}
		}
		private void SetLargestTNInfo(string text, int colorFlag)
		{
			try
			{
				this.PromptLargestTradeNum = new T8Order.PromptLargestTradeNumCallBack(this.LargestTNInfo);
				this.HandleCreated();
				base.Invoke(this.PromptLargestTradeNum, new object[]
				{
					text,
					colorFlag
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void LargestTNInfo(string text, int colorFlag)
		{
			if (colorFlag == 0)
			{
				this.textBoxInfo.ForeColor = Global.LightColor;
			}
			else
			{
				if (colorFlag == 1)
				{
					this.textBoxInfo.ForeColor = Global.ErrorColor;
				}
			}
			this.textBoxInfo.Text = Global.ServerTime.ToLongTimeString() + "  " + text;
			int num = text.IndexOf("：");
			if (num > 0)
			{
				this.textBoxCanNum.Text = text.Substring(num + 1);
				return;
			}
			this.textBoxCanNum.Text = "0";
		}
		private void OrderMessage(long retCode, string retMessage)
		{
			try
			{
				if (OperationManager.GetInstance().orderOperation.orderType == OrderType.Order)
				{
					this.OrderMessageInfo = new T8Order.OrderMessageInfoCallBack(this.OrderInfoMessage);
					this.HandleCreated();
					base.Invoke(this.OrderMessageInfo, new object[]
					{
						retCode,
						retMessage
					});
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void OrderInfoMessage(long retCode, string retMessage)
		{
			this.comboCommodity.Focus();
			if (IniData.GetInstance().ClearData)
			{
				if (this.numericUpDownPrice.Enabled)
				{
					this.numericUpDownPrice.Value = 0m;
				}
				this.numericUpDownNum.Text = "";
				this.numericLPrice.Value = 0m;
			}
			if (retCode == 0L)
			{
				this.operationManager.orderOperation.IsChangePrice = false;
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(this.operationManager.SussceOrder, Global.RightColor, true);
					return;
				}
			}
			else
			{
				if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
				{
					MessageBox.Show(retMessage, this.operationManager.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (this.operationManager.orderOperation.setLargestTN != null)
				{
					this.operationManager.orderOperation.setLargestTN(retMessage, 1);
				}
			}
		}
		private void SetFouce(short flag)
		{
			if (flag == 0)
			{
				this.comboCommodity.Focus();
				return;
			}
			if (flag == 1)
			{
				this.numericUpDownPrice.Focus();
				return;
			}
			if (flag == 2)
			{
				this.numericUpDownNum.Focus();
			}
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void numericUpDownPrice_ValueChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.comboBoxBuyOrSall.SelectedIndex;
			decimal bSPrice = this.operationManager.orderOperation.GetBSPrice(selectedIndex);
			if (this.numericUpDownPrice.Value != bSPrice)
			{
				this.operationManager.orderOperation.IsChangePrice = true;
				return;
			}
			this.operationManager.orderOperation.IsChangePrice = false;
		}
		private void SetOrderInfo(string commodityID, double buyPrice, double sellPrice)
		{
			string arg_05_0 = string.Empty;
			string text = string.Empty;
			int num = commodityID.IndexOf("_");
			if (num != -1)
			{
				commodityID.Substring(0, num);
				text = commodityID.Substring(num + 1);
			}
			else
			{
				text = commodityID;
			}
			if (text != this.comboCommodity.Text)
			{
				this.operationManager.orderOperation.ConnectHQ = true;
			}
			if (Global.MarketHT.Count == 1)
			{
				int i = 0;
				while (i < this.comboCommodity.Items.Count)
				{
					if (text.Equals(this.comboCommodity.Items[i].ToString()))
					{
						this.comboCommodity.SelectedIndex = i;
						if (this.comboBoxBuyOrSall.SelectedIndex == 0)
						{
							this.numericUpDownPrice.Value = (decimal)sellPrice;
							return;
						}
						this.numericUpDownPrice.Value = (decimal)buyPrice;
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			int j = 0;
			while (j < this.comboCommodity.Items.Count)
			{
				if (text.Equals(this.comboCommodity.Items[j].ToString()))
				{
					this.comboCommodity.SelectedIndex = j;
					if (this.comboBoxBuyOrSall.SelectedIndex == 0)
					{
						this.numericUpDownPrice.Value = (decimal)sellPrice;
						return;
					}
					this.numericUpDownPrice.Value = (decimal)buyPrice;
					return;
				}
				else
				{
					j++;
				}
			}
		}
		private bool SetCommoditySelectIndex(string marketID, string commodityID)
		{
			bool result = false;
			for (int i = 0; i < this.comboCommodity.Items.Count; i++)
			{
				if (this.comboCommodity.Items[i].ToString() == commodityID)
				{
					this.comboCommodity.SelectedIndex = i;
					result = true;
					break;
				}
			}
			return result;
		}
		private void SetDoubleClickOrderInfo(double price, double Lprice, int qty, short buysell, short ordertype)
		{
			try
			{
				if (buysell == 0)
				{
					this.comboBoxBuyOrSall.SelectedIndex = 0;
				}
				else
				{
					this.comboBoxBuyOrSall.SelectedIndex = 1;
				}
				if (ordertype == 0)
				{
					this.comboBoxTransfer.SelectedIndex = 0;
				}
				else
				{
					this.comboBoxTransfer.SelectedIndex = 1;
				}
				if (this.numericLPrice.Visible)
				{
					this.numericLPrice.Value = decimal.Parse(Lprice.ToString());
				}
				if (price != 0.0)
				{
					this.numericUpDownPrice.Value = decimal.Parse(price.ToString());
				}
				this.numericUpDownNum.Value = qty;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "SetDoubleClickOrderInfo异常：" + ex.Message);
			}
		}
	}
}
