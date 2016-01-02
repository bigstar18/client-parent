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
	public class FormOrder : Form
	{
		private delegate void ButOrderCallback(CommodityInfo commodityInfo);
		private delegate void ResponseVOCallback(ResponseVO resultMessage);
		private FormOrder.ButOrderCallback butOrderComm;
		private static volatile FormOrder instance;
		internal MainForm m_MainForm;
		private string TitleInfo = string.Empty;
		private DataProcess dataProcess = new DataProcess();
		private IContainer components;
		private Label labelMarKet;
		private ComboBox comboMarKet;
		private Button buttonOrder;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private TextBox commodityCode1;
		private TextBox textBoxPrice;
		private TextBox textBoxQty;
		private ComboBox comboB_S;
		private ComboBox comboO_L;
		private Label labTrancCode;
		private TextBox tbTranc;
		private ComboBox comboTranc;
		private ComboBox commodityCode;
		protected FormOrder()
		{
			this.InitializeComponent();
		}
		public static FormOrder Instance()
		{
			if (FormOrder.instance == null)
			{
				lock (typeof(FormOrder))
				{
					if (FormOrder.instance == null)
					{
						FormOrder.instance = new FormOrder();
					}
				}
			}
			return FormOrder.instance;
		}
		private void FormOrder_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.TitleInfo = Global.UserID + "----";
			this.Text = this.TitleInfo + "快速下单";
			this.ComboCommodityLoad();
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
			this.labTrancCode.Visible = false;
			this.comboTranc.Visible = false;
		}
		private void ComboTrancLoad()
		{
			this.comboTranc.Items.Add("00");
			if (this.comboTranc.Items.Count > 0)
			{
				this.comboTranc.SelectedIndex = 0;
			}
		}
		private void ComboCommodityLoad()
		{
			int num = 0;
			foreach (DataRow dataRow in this.m_MainForm.dsCommodity.Tables[0].Rows)
			{
				if (Global.MarketHT.Count > 1)
				{
					if ((bool)dataRow["Flag"])
					{
						if (this.m_MainForm.queryMarketID != null && this.m_MainForm.queryMarketID.Length > 0)
						{
							if (dataRow["MarKet"].ToString().Equals(this.m_MainForm.queryMarketID))
							{
								this.commodityCode.Items.Add(dataRow["commodityCode"].ToString());
								num++;
							}
						}
						else if (dataRow["MarKet"].ToString().Equals(this.m_MainForm.marketID))
						{
							this.commodityCode.Items.Add(dataRow["commodityCode"].ToString());
							num++;
						}
					}
				}
				else if ((bool)dataRow["Flag"])
				{
					this.commodityCode.Items.Add(dataRow["commodityCode"].ToString());
					num++;
				}
			}
			if (num == 0)
			{
				foreach (DataRow dataRow2 in this.m_MainForm.dsCommodity.Tables[0].Rows)
				{
					if (Global.MarketHT.Count > 1)
					{
						if (this.m_MainForm.queryMarketID != null && this.m_MainForm.queryMarketID.Length > 0)
						{
							if (dataRow2["MarKet"].ToString().Equals(this.m_MainForm.queryMarketID))
							{
								this.commodityCode.Items.Add(dataRow2["commodityCode"].ToString());
								num++;
							}
						}
						else if (dataRow2["MarKet"].ToString().Equals(this.m_MainForm.marketID))
						{
							this.commodityCode.Items.Add(dataRow2["commodityCode"].ToString());
							num++;
						}
					}
					else
					{
						this.commodityCode.Items.Add(dataRow2["commodityCode"].ToString());
						num++;
					}
				}
			}
			if (this.commodityCode.Items.Count > 0)
			{
				this.m_MainForm.currentCommodity = this.commodityCode.Items[0].ToString();
				this.commodityCode.SelectedIndex = 0;
			}
		}
		private void SetControlText()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.labTrancCode.Text = Global.M_ResourceManager.GetString("TradeStr_TrancCode");
			this.labComCode.Text = Global.M_ResourceManager.GetString("TradeStr_CommodityCode");
			this.comboB_S.Items.AddRange(new object[]
			{
				Global.M_ResourceManager.GetString("TradeStr_RadioB"),
				Global.M_ResourceManager.GetString("TradeStr_RadioS")
			});
			object[] items = new object[]
			{
				Global.M_ResourceManager.GetString("TradeStr_RadioO"),
				Global.M_ResourceManager.GetString("TradeStr_RadioL")
			};
			if (IniData.GetInstance().CloseMode.ToString() == "2")
			{
				items = new object[]
				{
					Global.M_ResourceManager.GetString("TradeStr_RadioO"),
					Global.M_ResourceManager.GetString("TradeStr_RadioL"),
					"转让今日"
				};
			}
			this.comboO_L.Items.AddRange(items);
			this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
			this.buttonOrder.Text = Global.M_ResourceManager.GetString("TradeStr_FormOrderButtonOrder");
			this.comboB_S.SelectedIndex = 0;
			this.comboO_L.SelectedIndex = 0;
			this.BackgroundImage = ControlLayout.SkinImage;
			if (Global.CustomerCount < 2)
			{
				this.comboTranc.Visible = false;
				this.tbTranc.Size = new Size(100, 20);
			}
		}
		private void comboB_S_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void orderEnable(bool enable)
		{
			this.buttonOrder.Enabled = enable;
			this.m_MainForm.buttonOrder.Enabled = enable;
			this.m_MainForm.buttonOrder6.Enabled = enable;
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			WaitCallback callBack = new WaitCallback(this.ButOrder);
			ThreadPool.QueueUserWorkItem(callBack, this.commodityCode.Text.Trim());
		}
		private void ButOrder(object o)
		{
			CommodityInfo commodityInfo = this.m_MainForm.GetCommodityInfo((string)o);
			this.butOrderComm = new FormOrder.ButOrderCallback(this.ButOrderComm);
			if (commodityInfo != null)
			{
				base.Invoke(this.butOrderComm, new object[]
				{
					commodityInfo
				});
				return;
			}
			base.Invoke(this.butOrderComm, new object[]
			{
				new CommodityInfo()
			});
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
			short buySell;
			short settleBasis;
			if (this.comboB_S.SelectedIndex == 0)
			{
				buySell = 1;
				settleBasis = 1;
			}
			else
			{
				buySell = 2;
				settleBasis = 2;
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
			if (commodityInfo.Spread >= 0.1)
			{
				double arg_FB_0 = commodityInfo.Spread;
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
			if (Convert.ToInt64(num * 100000.0) % Convert.ToInt64((decimal)commodityInfo.Spread * 100000m) != 0L)
			{
				this.Text = string.Concat(new object[]
				{
					this.TitleInfo,
					"价格不符合要求！商品价格最小变动价位为【",
					commodityInfo.Spread,
					"】！"
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
			orderRequestVO.BillType = 0;
			if (this.comboO_L.Text == "转让今日")
			{
				orderRequestVO.CloseMode = 2;
				orderRequestVO.TimeFlag = 1;
			}
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
			FormOrder.ResponseVOCallback method = new FormOrder.ResponseVOCallback(this.OrderMessage);
			base.Invoke(method, new object[]
			{
				responseVO
			});
		}
		private void OrderMessage(ResponseVO responseVO)
		{
			this.commodityCode.Text = "";
			this.comboB_S.SelectedIndex = 0;
			this.comboO_L.SelectedIndex = 0;
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
				if (this.comboB_S.SelectedIndex == 0)
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
				if (this.comboB_S.SelectedIndex == 0)
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
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b' && e.KeyChar != '.');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.m_MainForm.ChangeKeyChar(e.KeyChar);
			}
		}
		private void textBoxQty_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.m_MainForm.ChangeKeyChar(e.KeyChar);
			}
		}
		private void comboTranc_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}
		private void FormOrder_KeyDown(object sender, KeyEventArgs e)
		{
		}
		private void FormOrder_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13)
			{
				if (this.comboTranc.Focused || this.comboMarKet.Focused)
				{
					this.commodityCode.Focus();
					return;
				}
				if (this.commodityCode.Focused)
				{
					this.comboB_S.Focus();
					return;
				}
				if (this.comboB_S.Focused)
				{
					this.textBoxPrice.Focus();
					return;
				}
				if (this.textBoxPrice.Focused)
				{
					this.textBoxQty.Focus();
					return;
				}
				if (this.textBoxQty.Focused)
				{
					this.buttonOrder.Focus();
					return;
				}
				if (this.comboMarKet.Visible)
				{
					this.comboMarKet.Focus();
					return;
				}
				if (this.comboTranc.Visible)
				{
					this.comboTranc.Focus();
					return;
				}
				this.commodityCode.Focus();
			}
		}
		private void FormOrder_KeyPress(object sender, KeyPressEventArgs e)
		{
			bool handled = false;
			if (!this.commodityCode.Focused)
			{
				if (e.KeyChar == 'o' || e.KeyChar == 'O')
				{
					this.comboO_L.SelectedIndex = 0;
					handled = true;
				}
				else if (e.KeyChar == 'l' || e.KeyChar == 'L')
				{
					this.comboO_L.SelectedIndex = 1;
					handled = true;
				}
				else if (e.KeyChar == 'b' || e.KeyChar == 'B')
				{
					this.comboB_S.SelectedIndex = 0;
					handled = true;
				}
				else if (e.KeyChar == 's' || e.KeyChar == 'S')
				{
					this.comboB_S.SelectedIndex = 1;
					handled = true;
				}
				e.Handled = handled;
			}
		}
		private void commodityCode_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < '０' || e.KeyChar > '９') && e.KeyChar != '\b');
			if (e.KeyChar >= '０' && e.KeyChar <= '９')
			{
				e.KeyChar = this.m_MainForm.ChangeKeyChar(e.KeyChar);
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (FormOrder.instance != null)
			{
				FormOrder.instance = null;
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
			this.commodityCode1 = new TextBox();
			this.textBoxPrice = new TextBox();
			this.textBoxQty = new TextBox();
			this.comboB_S = new ComboBox();
			this.comboO_L = new ComboBox();
			this.labTrancCode = new Label();
			this.tbTranc = new TextBox();
			this.comboTranc = new ComboBox();
			this.commodityCode = new ComboBox();
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
			this.comboMarKet.Location = new Point(70, 3);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(100, 20);
			this.comboMarKet.TabIndex = 1;
			this.comboMarKet.Visible = false;
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			this.buttonOrder.FlatStyle = FlatStyle.Popup;
			this.buttonOrder.ImeMode = ImeMode.NoControl;
			this.buttonOrder.Location = new Point(761, 4);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(46, 21);
			this.buttonOrder.TabIndex = 7;
			this.buttonOrder.Text = "提交";
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.labQty.AutoSize = true;
			this.labQty.BackColor = Color.Transparent;
			this.labQty.ImeMode = ImeMode.NoControl;
			this.labQty.Location = new Point(609, 8);
			this.labQty.Name = "labQty";
			this.labQty.Size = new Size(65, 12);
			this.labQty.TabIndex = 9;
			this.labQty.Text = "委托数量：";
			this.labQty.TextAlign = ContentAlignment.BottomLeft;
			this.labPrice.AutoSize = true;
			this.labPrice.BackColor = Color.Transparent;
			this.labPrice.ImeMode = ImeMode.NoControl;
			this.labPrice.Location = new Point(449, 9);
			this.labPrice.Name = "labPrice";
			this.labPrice.Size = new Size(65, 12);
			this.labPrice.TabIndex = 8;
			this.labPrice.Text = "委托价格：";
			this.labPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode.AutoSize = true;
			this.labComCode.BackColor = Color.Transparent;
			this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode.ImeMode = ImeMode.NoControl;
			this.labComCode.Location = new Point(180, 7);
			this.labComCode.Name = "labComCode";
			this.labComCode.Size = new Size(65, 12);
			this.labComCode.TabIndex = 4;
			this.labComCode.Text = "商品代码：";
			this.labComCode.TextAlign = ContentAlignment.BottomLeft;
			this.commodityCode1.CharacterCasing = CharacterCasing.Upper;
			this.commodityCode1.Location = new Point(241, 3);
			this.commodityCode1.Name = "commodityCode1";
			this.commodityCode1.Size = new Size(81, 21);
			this.commodityCode1.TabIndex = 2;
			this.textBoxPrice.Location = new Point(522, 4);
			this.textBoxPrice.Name = "textBoxPrice";
			this.textBoxPrice.Size = new Size(81, 21);
			this.textBoxPrice.TabIndex = 5;
			this.textBoxPrice.KeyPress += new KeyPressEventHandler(this.textBoxPrice_KeyPress);
			this.textBoxQty.Location = new Point(680, 3);
			this.textBoxQty.Name = "textBoxQty";
			this.textBoxQty.Size = new Size(75, 21);
			this.textBoxQty.TabIndex = 6;
			this.textBoxQty.KeyPress += new KeyPressEventHandler(this.textBoxQty_KeyPress);
			this.comboB_S.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboB_S.FormattingEnabled = true;
			this.comboB_S.Location = new Point(349, 4);
			this.comboB_S.Name = "comboB_S";
			this.comboB_S.Size = new Size(81, 20);
			this.comboB_S.TabIndex = 4;
			this.comboB_S.SelectedIndexChanged += new EventHandler(this.comboB_S_SelectedIndexChanged);
			this.comboO_L.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboO_L.FormattingEnabled = true;
			this.comboO_L.Location = new Point(414, 4);
			this.comboO_L.Name = "comboO_L";
			this.comboO_L.Size = new Size(81, 20);
			this.comboO_L.TabIndex = 4;
			this.comboO_L.Visible = false;
			this.labTrancCode.AutoSize = true;
			this.labTrancCode.BackColor = Color.Transparent;
			this.labTrancCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTrancCode.ImeMode = ImeMode.NoControl;
			this.labTrancCode.Location = new Point(3, 6);
			this.labTrancCode.Name = "labTrancCode";
			this.labTrancCode.Size = new Size(65, 12);
			this.labTrancCode.TabIndex = 33;
			this.labTrancCode.Text = "交易代码：";
			this.labTrancCode.TextAlign = ContentAlignment.BottomLeft;
			this.tbTranc.BackColor = Color.White;
			this.tbTranc.Enabled = false;
			this.tbTranc.Location = new Point(70, 3);
			this.tbTranc.Multiline = true;
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.Size = new Size(60, 20);
			this.tbTranc.TabIndex = 1;
			this.comboTranc.Location = new Point(130, 3);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(40, 20);
			this.comboTranc.TabIndex = 2;
			this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
			this.commodityCode.FormattingEnabled = true;
			this.commodityCode.Location = new Point(241, 4);
			this.commodityCode.Name = "commodityCode";
			this.commodityCode.Size = new Size(81, 20);
			this.commodityCode.TabIndex = 3;
			this.commodityCode.KeyPress += new KeyPressEventHandler(this.commodityCode_KeyPress);
			base.AcceptButton = this.buttonOrder;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(816, 30);
			base.Controls.Add(this.commodityCode);
			base.Controls.Add(this.tbTranc);
			base.Controls.Add(this.comboTranc);
			base.Controls.Add(this.comboB_S);
			base.Controls.Add(this.textBoxQty);
			base.Controls.Add(this.textBoxPrice);
			base.Controls.Add(this.commodityCode1);
			base.Controls.Add(this.buttonOrder);
			base.Controls.Add(this.labQty);
			base.Controls.Add(this.labPrice);
			base.Controls.Add(this.labComCode);
			base.Controls.Add(this.comboMarKet);
			base.Controls.Add(this.labTrancCode);
			base.Controls.Add(this.labelMarKet);
			base.Controls.Add(this.comboO_L);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormOrder";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "下单";
			base.Load += new EventHandler(this.FormOrder_Load);
			base.KeyDown += new KeyEventHandler(this.FormOrder_KeyDown);
			base.KeyPress += new KeyPressEventHandler(this.FormOrder_KeyPress);
			base.KeyUp += new KeyEventHandler(this.FormOrder_KeyUp);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
