using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeClientApp.Gnnt.ISSUE.Library;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class BillOrder : Form
	{
		private delegate void ButOrderCallback(CommodityInfo commodityInfo);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private BillOrder.ButOrderCallback butOrderComm;
		private static volatile BillOrder instance;
		internal MainForm m_MainForm;
		private string TitleInfo = string.Empty;
		private DataProcess dataProcess = new DataProcess();
		private object[] comBillType = new object[]
		{
			"卖仓单",
			"抵顶转让"
		};
		private IContainer components;
		private Label labelMarKet;
		private ComboBox comboMarKet;
		private Button buttonOrder;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private TextBox commodityCode;
		private TextBox textBoxPrice;
		private TextBox textBoxQty;
		private ComboBox comb_BillType;
		private Label labTCode;
		private TextBox tbTranc;
		private ComboBox comboTranc;
		protected BillOrder()
		{
			this.InitializeComponent();
		}
		public static BillOrder Instance()
		{
			if (BillOrder.instance == null)
			{
				lock (typeof(BillOrder))
				{
					if (BillOrder.instance == null)
					{
						BillOrder.instance = new BillOrder();
					}
				}
			}
			return BillOrder.instance;
		}
		private void BillOrder_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.TitleInfo = Global.UserID + "----";
			this.Text = this.TitleInfo + "仓单交易";
			if (Global.MarketHT.Count > 1)
			{
				this.ComboMarKetLoad();
			}
			else
			{
				this.tbTranc.Text = Global.FirmID;
				this.ComboTrancLoad();
			}
			ScaleForm.ScaleForms(this);
		}
		private void ComboMarKetLoad()
		{
			ArrayList arrayList = new ArrayList();
			if (Global.MarketHT != null)
			{
				foreach (DictionaryEntry dictionaryEntry in Global.MarketHT)
				{
					MarkeInfo markeInfo = (MarkeInfo)dictionaryEntry.Value;
					if (markeInfo != null)
					{
						arrayList.Add(new AddValue(markeInfo.ShortName, markeInfo.MarketID));
					}
				}
				this.comboMarKet.DisplayMember = "Display";
				this.comboMarKet.ValueMember = "Value";
				this.comboMarKet.DataSource = null;
				this.comboMarKet.DataSource = arrayList;
			}
			this.comboMarKet.SelectedIndex = 0;
			this.labelMarKet.Visible = true;
			this.comboMarKet.Visible = true;
			this.tbTranc.Visible = false;
			this.labTCode.Visible = false;
			this.comboTranc.Visible = false;
		}
		private void ComboTrancLoad()
		{
			int num = 0;
			foreach (DataRow dataRow in this.m_MainForm.dsTransactions.Tables[0].Rows)
			{
				if ((bool)dataRow["Flag"])
				{
					string text = dataRow["TransactionsCode"].ToString();
					if (!Global.FirmID.Equals(text) && text.Length > 2)
					{
						this.comboTranc.Items.Add(text.Substring(text.Length - 2));
					}
					num++;
				}
			}
			if (num == 0)
			{
				foreach (DataRow dataRow2 in this.m_MainForm.dsTransactions.Tables[0].Rows)
				{
					string text2 = dataRow2["TransactionsCode"].ToString();
					if (!Global.FirmID.Equals(text2) && text2.Length > 2)
					{
						this.comboTranc.Items.Add(text2.Substring(text2.Length - 2));
					}
					num++;
				}
			}
			if (this.comboTranc.Items.Count > 0)
			{
				this.comboTranc.SelectedIndex = 0;
			}
		}
		private void SetControlText()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.labTCode.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			string a = (string)Global.HTConfig["BillType"];
			if (a == "1")
			{
				this.comb_BillType.Items.Add(this.comBillType[0]);
			}
			else if (a == "2")
			{
				this.comb_BillType.Items.Add(this.comBillType[1]);
			}
			else if (a == "3")
			{
				this.comb_BillType.Items.AddRange(this.comBillType);
			}
			this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
			this.buttonOrder.Text = Global.M_ResourceManager.GetString("TradeStr_FormOrderButtonOrder");
			this.comb_BillType.SelectedIndex = 0;
			this.BackgroundImage = ControlLayout.SkinImage;
			if (Global.CustomerCount < 2)
			{
				this.comboTranc.Visible = false;
				this.tbTranc.Size = new Size(100, 20);
			}
		}
		private void orderEnable(bool enable)
		{
			this.buttonOrder.Enabled = enable;
			this.m_MainForm.buttonOrder.Enabled = enable;
			this.m_MainForm.buttonOrder6.Enabled = enable;
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			this.orderEnable(false);
			WaitCallback callBack = new WaitCallback(this.ButOrder);
			ThreadPool.QueueUserWorkItem(callBack, this.commodityCode.Text.Trim());
		}
		private void ButOrder(object o)
		{
			CommodityInfo commodityInfo = this.m_MainForm.GetCommodityInfo((string)o);
			this.butOrderComm = new BillOrder.ButOrderCallback(this.ButOrderComm);
			if (commodityInfo != null)
			{
				base.Invoke(this.butOrderComm, new object[]
				{
					commodityInfo
				});
			}
		}
		private void ButOrderComm(CommodityInfo commodityInfo)
		{
			string marketID = string.Empty;
			if (!this.comboMarKet.Text.Equals("全部") && Global.MarketHT.Count > 1)
			{
				marketID = this.comboMarKet.SelectedValue.ToString();
			}
			string commodityID = this.commodityCode.Text.Trim();
			string customerID = this.tbTranc.Text + this.comboTranc.Text;
			short buySell = 2;
			short settleBasis = 1;
			short billType = 1;
			if (this.comb_BillType.Text == this.comBillType[0].ToString())
			{
				buySell = 2;
				settleBasis = 1;
				billType = 1;
			}
			else if (this.comb_BillType.Text == this.comBillType[1].ToString())
			{
				buySell = 1;
				settleBasis = 2;
				billType = 2;
			}
			double num = 0.0;
			if (this.textBoxPrice.Text.Length > 0)
			{
				num = Tools.StrToDouble(this.textBoxPrice.Text);
			}
			long num2 = 0L;
			if (this.textBoxQty.Text.Length > 0)
			{
				num2 = Tools.StrToLong(this.textBoxQty.Text);
			}
			if (commodityInfo == null || commodityInfo.CommodityID == null || commodityInfo.CommodityID.Length <= 0)
			{
				this.Text = this.TitleInfo + "输入的商品不存在!";
				this.orderEnable(true);
				return;
			}
			if (num > commodityInfo.SpreadUp || num < commodityInfo.SpreadDown)
			{
				this.Text = string.Concat(new object[]
				{
					this.TitleInfo,
					"价格不符合要求！应在",
					commodityInfo.SpreadDown,
					"与",
					commodityInfo.SpreadUp,
					"之间！"
				});
				this.orderEnable(true);
				this.textBoxPrice.Focus();
				return;
			}
			if (num2 <= 0L)
			{
				this.Text = this.TitleInfo + "数量不能为０！";
				this.orderEnable(true);
				this.textBoxQty.Focus();
				return;
			}
			OrderRequestVO orderRequestVO = new OrderRequestVO();
			orderRequestVO.UserID = Global.UserID;
			orderRequestVO.CustomerID = customerID;
			orderRequestVO.MarketID = marketID;
			orderRequestVO.CommodityID = commodityID;
			orderRequestVO.BuySell = buySell;
			orderRequestVO.SettleBasis = settleBasis;
			orderRequestVO.Price = num;
			orderRequestVO.Quantity = num2;
			orderRequestVO.CloseMode = 1;
			orderRequestVO.BillType = billType;
			string text = string.Empty;
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"商品代码：",
				orderRequestVO.CommodityID,
				"\r\n商品价格：",
				orderRequestVO.Price,
				"   商品数量:",
				orderRequestVO.Quantity,
				"\r\n买卖方式：",
				Global.BuySellStrArr[(int)orderRequestVO.BuySell],
				"   \r\n\u3000\u3000\u3000确定下委托单吗？"
			});
			if (!IniData.GetInstance().ShowDialog)
			{
				Logger.wirte(1, "下单线程提交，等待程序处理");
				WaitCallback callBack = new WaitCallback(this.Order);
				ThreadPool.QueueUserWorkItem(callBack, orderRequestVO);
				return;
			}
			MessageForm messageForm = new MessageForm("委托单信息", text, 0);
			messageForm.Owner = this;
			messageForm.ShowDialog();
			messageForm.Dispose();
			if (messageForm.isOK)
			{
				Logger.wirte(1, "下单线程提交，等待程序处理");
				WaitCallback callBack2 = new WaitCallback(this.Order);
				ThreadPool.QueueUserWorkItem(callBack2, orderRequestVO);
				return;
			}
			this.orderEnable(true);
		}
		private void Order(object _orderRequestVO)
		{
			OrderRequestVO req = (OrderRequestVO)_orderRequestVO;
			ResponseVO responseVO = this.dataProcess.Order(req);
			BillOrder.ResponseVOCallback method = new BillOrder.ResponseVOCallback(this.OrderMessage);
			base.Invoke(method, new object[]
			{
				responseVO
			});
		}
		private void OrderMessage(ResponseVO responseVO)
		{
			this.commodityCode.Text = "";
			this.comb_BillType.SelectedIndex = 0;
			this.textBoxPrice.Text = "";
			this.textBoxQty.Text = "";
			if (responseVO.RetCode == 0L)
			{
				this.Text = this.TitleInfo + "委托成功!";
				this.m_MainForm.refreshFlag = true;
			}
			else
			{
				this.Text = this.TitleInfo + responseVO.RetMessage;
			}
			this.orderEnable(true);
		}
		public void SetOrderInfo(string marketID, string CommodityCode, float BuyPrice, float SellPrice)
		{
			if (Global.MarketHT.Count == 1)
			{
				this.commodityCode.Text = CommodityCode;
				if (this.comb_BillType.SelectedIndex == 0)
				{
					this.textBoxPrice.Text = SellPrice.ToString();
				}
				else
				{
					this.textBoxPrice.Text = BuyPrice.ToString();
				}
			}
			else
			{
				for (int i = 0; i < this.comboMarKet.Items.Count; i++)
				{
					AddValue addValue = (AddValue)this.comboMarKet.Items[i];
					if (marketID.Equals(addValue.Value))
					{
						this.comboMarKet.SelectedIndex = i;
						break;
					}
				}
				this.commodityCode.Text = CommodityCode;
				if (this.comb_BillType.SelectedIndex == 0)
				{
					this.textBoxPrice.Text = SellPrice.ToString();
				}
				else
				{
					this.textBoxPrice.Text = BuyPrice.ToString();
				}
			}
			this.textBoxQty.Focus();
		}
		private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = (e.KeyChar < '0' || e.KeyChar > '9');
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
		}
		private void textBoxQty_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = (e.KeyChar < '0' || e.KeyChar > '9');
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
		}
		private void comboTranc_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (BillOrder.instance != null)
			{
				BillOrder.instance = null;
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.labelMarKet = new Label();
			this.comboMarKet = new ComboBox();
			this.buttonOrder = new Button();
			this.labQty = new Label();
			this.labPrice = new Label();
			this.labComCode = new Label();
			this.commodityCode = new TextBox();
			this.textBoxPrice = new TextBox();
			this.textBoxQty = new TextBox();
			this.comb_BillType = new ComboBox();
			this.labTCode = new Label();
			this.tbTranc = new TextBox();
			this.comboTranc = new ComboBox();
			base.SuspendLayout();
			this.labelMarKet.AutoSize = true;
			this.labelMarKet.ImeMode = ImeMode.NoControl;
			this.labelMarKet.Location = new Point(3, 7);
			this.labelMarKet.Name = "labelMarKet";
			this.labelMarKet.Size = new Size(65, 12);
			this.labelMarKet.TabIndex = 31;
			this.labelMarKet.Text = "市场标志：";
			this.labelMarKet.Visible = false;
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet.Location = new Point(64, 3);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(81, 20);
			this.comboMarKet.TabIndex = 1;
			this.comboMarKet.Visible = false;
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			this.buttonOrder.FlatStyle = FlatStyle.Popup;
			this.buttonOrder.ImeMode = ImeMode.NoControl;
			this.buttonOrder.Location = new Point(728, 4);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(46, 21);
			this.buttonOrder.TabIndex = 7;
			this.buttonOrder.Text = "提交";
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.labQty.AutoSize = true;
			this.labQty.BackColor = Color.FromArgb(215, 215, 215);
			this.labQty.ImeMode = ImeMode.NoControl;
			this.labQty.Location = new Point(600, 8);
			this.labQty.Name = "labQty";
			this.labQty.Size = new Size(65, 12);
			this.labQty.TabIndex = 9;
			this.labQty.Text = "委托数量：";
			this.labQty.TextAlign = ContentAlignment.BottomLeft;
			this.labPrice.AutoSize = true;
			this.labPrice.BackColor = Color.FromArgb(215, 215, 215);
			this.labPrice.ImeMode = ImeMode.NoControl;
			this.labPrice.Location = new Point(448, 9);
			this.labPrice.Name = "labPrice";
			this.labPrice.Size = new Size(65, 12);
			this.labPrice.TabIndex = 8;
			this.labPrice.Text = "委托价格：";
			this.labPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode.AutoSize = true;
			this.labComCode.BackColor = Color.FromArgb(215, 215, 215);
			this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode.ImeMode = ImeMode.NoControl;
			this.labComCode.Location = new Point(169, 7);
			this.labComCode.Name = "labComCode";
			this.labComCode.Size = new Size(65, 12);
			this.labComCode.TabIndex = 4;
			this.labComCode.Text = "商品代码：";
			this.labComCode.TextAlign = ContentAlignment.BottomLeft;
			this.commodityCode.CharacterCasing = CharacterCasing.Upper;
			this.commodityCode.Location = new Point(230, 3);
			this.commodityCode.Name = "commodityCode";
			this.commodityCode.Size = new Size(81, 21);
			this.commodityCode.TabIndex = 2;
			this.textBoxPrice.Location = new Point(509, 4);
			this.textBoxPrice.Name = "textBoxPrice";
			this.textBoxPrice.Size = new Size(81, 21);
			this.textBoxPrice.TabIndex = 5;
			this.textBoxPrice.KeyPress += new KeyPressEventHandler(this.textBoxPrice_KeyPress);
			this.textBoxQty.Location = new Point(662, 4);
			this.textBoxQty.Name = "textBoxQty";
			this.textBoxQty.Size = new Size(56, 21);
			this.textBoxQty.TabIndex = 6;
			this.textBoxQty.KeyPress += new KeyPressEventHandler(this.textBoxQty_KeyPress);
			this.comb_BillType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comb_BillType.FormattingEnabled = true;
			this.comb_BillType.Location = new Point(341, 4);
			this.comb_BillType.Name = "comb_BillType";
			this.comb_BillType.Size = new Size(81, 20);
			this.comb_BillType.TabIndex = 3;
			this.labTCode.AutoSize = true;
			this.labTCode.BackColor = Color.FromArgb(215, 215, 215);
			this.labTCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTCode.ImeMode = ImeMode.NoControl;
			this.labTCode.Location = new Point(3, 8);
			this.labTCode.Name = "labTCode";
			this.labTCode.Size = new Size(65, 12);
			this.labTCode.TabIndex = 33;
			this.labTCode.Text = "交易代码：";
			this.labTCode.TextAlign = ContentAlignment.BottomLeft;
			this.tbTranc.BackColor = Color.White;
			this.tbTranc.Enabled = false;
			this.tbTranc.Location = new Point(66, 3);
			this.tbTranc.Multiline = true;
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.Size = new Size(43, 20);
			this.tbTranc.TabIndex = 36;
			this.comboTranc.Location = new Point(107, 3);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(37, 20);
			this.comboTranc.TabIndex = 35;
			this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
			base.AcceptButton = this.buttonOrder;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(784, 30);
			base.Controls.Add(this.tbTranc);
			base.Controls.Add(this.comboTranc);
			base.Controls.Add(this.comb_BillType);
			base.Controls.Add(this.textBoxQty);
			base.Controls.Add(this.textBoxPrice);
			base.Controls.Add(this.commodityCode);
			base.Controls.Add(this.buttonOrder);
			base.Controls.Add(this.labQty);
			base.Controls.Add(this.labPrice);
			base.Controls.Add(this.labComCode);
			base.Controls.Add(this.comboMarKet);
			base.Controls.Add(this.labTCode);
			base.Controls.Add(this.labelMarKet);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "BillOrder";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "仓单交易";
			base.Load += new EventHandler(this.BillOrder_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
