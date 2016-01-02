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
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class FormOrder : Form
	{
		private delegate void ButOrderCallback(CommodityInfo commodityInfo);
		private delegate void OrderMessageInfoCallBack(long retCode, string retMessage);
		private delegate void StringObjCallback(object _commodityItem);
		private IContainer components;
		private ComboBox commodityCode;
		private TextBox tbTranc;
		private ComboBox comboTranc;
		private ComboBox comboO_L;
		private ComboBox comboB_S;
		private TextBox textBoxQty;
		private TextBox textBoxPrice;
		private Button buttonOrder;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private ComboBox comboMarKet;
		private Label labTrancCode;
		private Label labelMarKet;
		private string NumIsNotZero = Global.M_ResourceManager.GetString("TradeStr_MainForm_NumIsNotZero");
		private string ErrorPriceMassage = "价格不能为0！！！";
		private FormOrder.ButOrderCallback butOrderComm;
		private SubmitOrderInfo submitOrderInfo;
		private string TitleInfo = string.Empty;
		private FormOrder.OrderMessageInfoCallBack OrderMessageInfo;
		private static volatile FormOrder instance;
		protected override void Dispose(bool disposing)
		{
			if (FormOrder.instance != null)
			{
				FormOrder.instance = null;
			}
			Global.SetOrderInfo -= new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			SubmitOrderOperation expr_2C = OperationManager.GetInstance().submitOrderOperation;
			expr_2C.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Remove(expr_2C.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.commodityCode = new ComboBox();
			this.tbTranc = new TextBox();
			this.comboTranc = new ComboBox();
			this.comboO_L = new ComboBox();
			this.comboB_S = new ComboBox();
			this.textBoxQty = new TextBox();
			this.textBoxPrice = new TextBox();
			this.buttonOrder = new Button();
			this.labQty = new Label();
			this.labPrice = new Label();
			this.labComCode = new Label();
			this.comboMarKet = new ComboBox();
			this.labTrancCode = new Label();
			this.labelMarKet = new Label();
			base.SuspendLayout();
			this.commodityCode.FlatStyle = FlatStyle.Flat;
			this.commodityCode.FormattingEnabled = true;
			this.commodityCode.Location = new Point(221, 5);
			this.commodityCode.Name = "commodityCode";
			this.commodityCode.Size = new Size(81, 20);
			this.commodityCode.TabIndex = 2;
			this.commodityCode.TextChanged += new EventHandler(this.commodityCode_TextChanged);
			this.commodityCode.KeyDown += new KeyEventHandler(this.commodityCode_KeyDown);
			this.tbTranc.BackColor = Color.White;
			this.tbTranc.Enabled = false;
			this.tbTranc.Location = new Point(69, 5);
			this.tbTranc.Multiline = true;
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.Size = new Size(43, 20);
			this.tbTranc.TabIndex = 51;
			this.tbTranc.TabStop = false;
			this.comboTranc.FlatStyle = FlatStyle.Flat;
			this.comboTranc.Location = new Point(113, 5);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(37, 20);
			this.comboTranc.TabIndex = 50;
			this.comboTranc.TabStop = false;
			this.comboTranc.KeyPress += new KeyPressEventHandler(this.comboTranc_KeyPress);
			this.comboO_L.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboO_L.FlatStyle = FlatStyle.Flat;
			this.comboO_L.FormattingEnabled = true;
			this.comboO_L.Location = new Point(397, 5);
			this.comboO_L.Name = "comboO_L";
			this.comboO_L.Size = new Size(81, 20);
			this.comboO_L.TabIndex = 4;
			this.comboO_L.KeyDown += new KeyEventHandler(this.comboO_L_KeyDown);
			this.comboO_L.KeyUp += new KeyEventHandler(this.comboO_L_KeyUp);
			this.comboB_S.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboB_S.FlatStyle = FlatStyle.Flat;
			this.comboB_S.FormattingEnabled = true;
			this.comboB_S.Location = new Point(309, 5);
			this.comboB_S.Name = "comboB_S";
			this.comboB_S.Size = new Size(81, 20);
			this.comboB_S.TabIndex = 3;
			this.comboB_S.KeyDown += new KeyEventHandler(this.comboB_S_KeyDown);
			this.comboB_S.KeyUp += new KeyEventHandler(this.comboB_S_KeyUp);
			this.textBoxQty.Location = new Point(700, 5);
			this.textBoxQty.MaxLength = 99999;
			this.textBoxQty.Name = "textBoxQty";
			this.textBoxQty.Size = new Size(56, 21);
			this.textBoxQty.TabIndex = 6;
			this.textBoxQty.Enter += new EventHandler(this.textBoxQty_Enter);
			this.textBoxQty.KeyPress += new KeyPressEventHandler(this.textBoxQty_KeyPress);
			this.textBoxQty.KeyUp += new KeyEventHandler(this.textBoxQty_KeyUp);
			this.textBoxPrice.Location = new Point(548, 5);
			this.textBoxPrice.MaxLength = 999999;
			this.textBoxPrice.Name = "textBoxPrice";
			this.textBoxPrice.Size = new Size(81, 21);
			this.textBoxPrice.TabIndex = 5;
			this.textBoxPrice.Enter += new EventHandler(this.textBoxPrice_Enter);
			this.textBoxPrice.KeyPress += new KeyPressEventHandler(this.textBoxPrice_KeyPress);
			this.textBoxPrice.KeyUp += new KeyEventHandler(this.textBoxPrice_KeyUp);
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			this.buttonOrder.FlatStyle = FlatStyle.Popup;
			this.buttonOrder.ImeMode = ImeMode.NoControl;
			this.buttonOrder.Location = new Point(766, 5);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(50, 21);
			this.buttonOrder.TabIndex = 7;
			this.buttonOrder.Text = "提交";
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.buttonOrder.Click += new EventHandler(this.buttonOrder_Click);
			this.labQty.AutoSize = true;
			this.labQty.BackColor = Color.Transparent;
			this.labQty.ImeMode = ImeMode.NoControl;
			this.labQty.Location = new Point(635, 9);
			this.labQty.Name = "labQty";
			this.labQty.Size = new Size(65, 12);
			this.labQty.TabIndex = 47;
			this.labQty.Text = "委托数量：";
			this.labQty.TextAlign = ContentAlignment.BottomLeft;
			this.labPrice.AutoSize = true;
			this.labPrice.BackColor = Color.Transparent;
			this.labPrice.ImeMode = ImeMode.NoControl;
			this.labPrice.Location = new Point(484, 9);
			this.labPrice.Name = "labPrice";
			this.labPrice.Size = new Size(65, 12);
			this.labPrice.TabIndex = 46;
			this.labPrice.Text = "委托价格：";
			this.labPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode.AutoSize = true;
			this.labComCode.BackColor = Color.Transparent;
			this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode.ImeMode = ImeMode.NoControl;
			this.labComCode.Location = new Point(156, 9);
			this.labComCode.Name = "labComCode";
			this.labComCode.Size = new Size(65, 12);
			this.labComCode.TabIndex = 41;
			this.labComCode.Text = "商品代码：";
			this.labComCode.TextAlign = ContentAlignment.BottomLeft;
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet.Location = new Point(69, 5);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(81, 20);
			this.comboMarKet.TabIndex = 37;
			this.comboMarKet.TabStop = false;
			this.comboMarKet.Visible = false;
			this.labTrancCode.AutoSize = true;
			this.labTrancCode.BackColor = Color.Transparent;
			this.labTrancCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTrancCode.ImeMode = ImeMode.NoControl;
			this.labTrancCode.Location = new Point(5, 9);
			this.labTrancCode.Name = "labTrancCode";
			this.labTrancCode.Size = new Size(65, 12);
			this.labTrancCode.TabIndex = 49;
			this.labTrancCode.Text = "交易代码：";
			this.labTrancCode.TextAlign = ContentAlignment.BottomLeft;
			this.labelMarKet.AutoSize = true;
			this.labelMarKet.ImeMode = ImeMode.NoControl;
			this.labelMarKet.Location = new Point(5, 9);
			this.labelMarKet.Name = "labelMarKet";
			this.labelMarKet.Size = new Size(65, 12);
			this.labelMarKet.TabIndex = 48;
			this.labelMarKet.Text = "市场标志：";
			this.labelMarKet.Visible = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(839, 34);
			base.Controls.Add(this.commodityCode);
			base.Controls.Add(this.tbTranc);
			base.Controls.Add(this.comboTranc);
			base.Controls.Add(this.comboO_L);
			base.Controls.Add(this.comboB_S);
			base.Controls.Add(this.textBoxQty);
			base.Controls.Add(this.textBoxPrice);
			base.Controls.Add(this.buttonOrder);
			base.Controls.Add(this.labQty);
			base.Controls.Add(this.labPrice);
			base.Controls.Add(this.labComCode);
			base.Controls.Add(this.comboMarKet);
			base.Controls.Add(this.labTrancCode);
			base.Controls.Add(this.labelMarKet);
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
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		protected FormOrder()
		{
			this.InitializeComponent();
			Global.SetOrderInfo += new Global.SetOrderInfoCallBack(this.SetOrderInfo);
			SubmitOrderOperation expr_52 = OperationManager.GetInstance().submitOrderOperation;
			expr_52.OrderMessage = (SubmitOrderOperation.OrderMessageCallBack)Delegate.Combine(expr_52.OrderMessage, new SubmitOrderOperation.OrderMessageCallBack(this.OrderMessage));
		}
		public static FormOrder Instance()
		{
			if (FormOrder.instance == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(FormOrder));
				try
				{
					if (FormOrder.instance == null)
					{
						FormOrder.instance = new FormOrder();
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return FormOrder.instance;
		}
		private void FormOrder_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			this.TitleInfo = Global.UserID + "----";
			string @string = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonOrder");
			this.Text = this.TitleInfo + @string;
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
			this.commodityCode.Focus();
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
			this.comboTranc.Items.Clear();
			try
			{
				if (OperationManager.GetInstance().myTransactionsList.Count > 1)
				{
					using (List<string>.Enumerator enumerator = OperationManager.GetInstance().myTransactionsList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (current != OperationManager.GetInstance().StrAll)
							{
								this.comboTranc.Items.Add(current.Substring(current.Length - 2));
							}
						}
						goto IL_EB;
					}
				}
				foreach (string current2 in OperationManager.GetInstance().transactionsList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.comboTranc.Items.Add(current2.Substring(current2.Length - 2));
					}
				}
				IL_EB:;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
			if (this.comboTranc.Items.Count > 0)
			{
				this.comboTranc.SelectedIndex = 0;
			}
		}
		private void ComboCommodityLoad()
		{
			this.commodityCode.Items.Clear();
			try
			{
				if (OperationManager.GetInstance().myCommodityList.Count > 1)
				{
					using (List<string>.Enumerator enumerator = OperationManager.GetInstance().myCommodityList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (current != OperationManager.GetInstance().StrAll)
							{
								this.commodityCode.Items.Add(current);
							}
						}
						goto IL_D1;
					}
				}
				foreach (string current2 in OperationManager.GetInstance().commodityList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.commodityCode.Items.Add(current2);
					}
				}
				IL_D1:;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "获取商品信息错误：" + ex.Message);
			}
			if (this.commodityCode.Items.Count > 0)
			{
				this.commodityCode.SelectedIndex = 0;
			}
		}
		private void SetControlText()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.labPrice.Text = Global.M_ResourceManager.GetString("TradeStr_LabPrice");
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
					Global.M_ResourceManager.GetString("TradeStr_FormOrder_TransferToday")
				};
			}
			this.comboO_L.Items.AddRange(items);
			this.buttonOrder.TextAlign = ContentAlignment.TopCenter;
			this.labQty.Text = Global.M_ResourceManager.GetString("TradeStr_LabQty");
			this.buttonOrder.Text = Global.M_ResourceManager.GetString("TradeStr_FormOrderButtonOrder");
			this.comboB_S.SelectedIndex = 0;
			this.comboO_L.SelectedIndex = 0;
			this.BackgroundImage = ControlLayout.SkinImage;
			if (Global.CustomerCount < 2)
			{
				this.comboTranc.Visible = false;
				this.tbTranc.Size = new Size(70, 20);
			}
		}
		private void orderEnable(bool enable)
		{
		}
		private void buttonOrder_Click(object sender, EventArgs e)
		{
			if (this.submitOrderInfo == null)
			{
				this.submitOrderInfo = new SubmitOrderInfo();
			}
			this.submitOrderInfo.commodityID = this.commodityCode.Text.Trim();
			this.submitOrderInfo.customerID = this.tbTranc.Text + this.comboTranc.Text;
			this.submitOrderInfo.closeMode = 1;
			this.submitOrderInfo.B_SType = 1;
			if (this.comboB_S.SelectedIndex == 1)
			{
				this.submitOrderInfo.B_SType = 2;
			}
			this.submitOrderInfo.O_LType = 1;
			if (this.comboO_L.SelectedIndex == 1)
			{
				this.submitOrderInfo.O_LType = 2;
			}
			double num = Tools.StrToDouble(this.textBoxPrice.Text, -1.0);
			if (num <= 0.0)
			{
				this.textBoxPrice.Focus();
				this.Text = this.TitleInfo + this.ErrorPriceMassage;
				return;
			}
			this.submitOrderInfo.price = num;
			int num2 = Tools.StrToInt(this.textBoxQty.Text, 0);
			if (num2 > 0)
			{
				this.submitOrderInfo.qty = num2;
				OperationManager.GetInstance().orderOperation.orderType = OrderType.FormOrder;
				OperationManager.GetInstance().submitOrderOperation.SubmitOrderInfo(this.submitOrderInfo, 0);
				return;
			}
			this.textBoxQty.Focus();
			this.Text = this.TitleInfo + this.NumIsNotZero;
		}
		public void SetOrderInfo(string CommodityCode, double BuyPrice, double SellPrice)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = CommodityCode.IndexOf("_");
			if (num != -1)
			{
				text = CommodityCode.Substring(0, num);
				text2 = CommodityCode.Substring(num + 1);
			}
			else
			{
				text2 = CommodityCode;
			}
			if (Global.MarketHT.Count == 1)
			{
				int i = 0;
				while (i < this.commodityCode.Items.Count)
				{
					if (text2.Equals(this.commodityCode.Items[i].ToString()))
					{
						this.commodityCode.SelectedIndex = i;
						if (this.comboB_S.SelectedIndex == 0)
						{
							this.textBoxPrice.Text = SellPrice.ToString();
							break;
						}
						this.textBoxPrice.Text = BuyPrice.ToString();
						break;
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.comboMarKet.Items.Count; j++)
				{
					AddValue addValue = (AddValue)this.comboMarKet.Items[j];
					if (text.Equals(addValue.Value))
					{
						this.comboMarKet.SelectedIndex = j;
						break;
					}
				}
				int k = 0;
				while (k < this.commodityCode.Items.Count)
				{
					if (text2.Equals(this.commodityCode.Items[k].ToString()))
					{
						this.commodityCode.SelectedIndex = k;
						if (this.comboB_S.SelectedIndex == 0)
						{
							this.textBoxPrice.Text = SellPrice.ToString();
							break;
						}
						this.textBoxPrice.Text = BuyPrice.ToString();
						break;
					}
					else
					{
						k++;
					}
				}
			}
			this.textBoxQty.Focus();
		}
		private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\r' && e.KeyChar != '.')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf(".") >= 0)
			{
				e.Handled = true;
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
		private void FormOrder_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				if (!this.buttonOrder.Focused)
				{
					this.buttonOrder.Focus();
				}
				return;
			}
			if (e.KeyCode == Keys.Right)
			{
				if (this.commodityCode.Focused)
				{
					this.comboB_S.Focus();
					return;
				}
				if (this.comboB_S.Focused)
				{
					this.comboO_L.Focus();
					return;
				}
				if (this.comboO_L.Focused)
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
				if (this.buttonOrder.Focused)
				{
					this.commodityCode.Focus();
					return;
				}
			}
			else
			{
				if (e.KeyCode == Keys.Left)
				{
					if (this.commodityCode.Focused)
					{
						this.buttonOrder.Focus();
						return;
					}
					if (this.comboB_S.Focused)
					{
						this.commodityCode.Focus();
						return;
					}
					if (this.comboO_L.Focused)
					{
						this.comboB_S.Focus();
						return;
					}
					if (this.textBoxPrice.Focused)
					{
						this.comboO_L.Focus();
						return;
					}
					if (this.textBoxQty.Focused)
					{
						this.textBoxPrice.Focus();
						return;
					}
					if (this.buttonOrder.Focused)
					{
						this.textBoxQty.Focus();
					}
				}
			}
		}
		private void comboB_S_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
			{
				this.comboB_S.SelectedIndex = 0;
				return;
			}
			if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
			{
				this.comboB_S.SelectedIndex = 1;
			}
		}
		private void comboO_L_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
			{
				this.comboO_L.SelectedIndex = 0;
				return;
			}
			if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
			{
				this.comboO_L.SelectedIndex = 1;
			}
		}
		private void textBoxPrice_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.textBoxPrice.Text.ToString().Length == 0)
			{
				return;
			}
			if (this.textBoxPrice.Text.ToString().Length > 6)
			{
				this.textBoxPrice.Text = this.textBoxPrice.MaxLength.ToString();
				e.Handled = true;
			}
			if (e.KeyValue % 48 == Convert.ToDecimal(this.textBoxPrice.Text))
			{
				this.textBoxPrice.Select(this.textBoxPrice.Text.ToString().Length, 0);
			}
		}
		private void textBoxQty_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.textBoxQty.Text.Length == 0)
			{
				return;
			}
			if (this.textBoxQty.Text.Length > 6)
			{
				this.textBoxQty.Text = this.textBoxQty.MaxLength.ToString();
				e.Handled = true;
			}
			if (e.KeyValue % 48 == Convert.ToInt32(this.textBoxQty.Text.ToString()))
			{
				this.textBoxQty.Select(this.textBoxQty.Text.Length, 0);
			}
		}
		private void commodityCode_TextChanged(object sender, EventArgs e)
		{
			this.commodityCode.SelectAll();
		}
		private void NewPrice(string commodityName)
		{
		}
		private void comboB_S_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				if (this.comboB_S.SelectedIndex == 0)
				{
					this.comboB_S.SelectedIndex = 1;
				}
				else
				{
					this.comboB_S.SelectedIndex = 0;
				}
			}
			else
			{
				if (e.KeyCode == Keys.Down)
				{
					if (this.comboB_S.SelectedIndex == 0)
					{
						this.comboB_S.SelectedIndex = 1;
					}
					else
					{
						this.comboB_S.SelectedIndex = 0;
					}
				}
			}
			e.Handled = true;
		}
		private void comboO_L_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				if (this.comboO_L.SelectedIndex == 0)
				{
					this.comboO_L.SelectedIndex = 1;
				}
				else
				{
					this.comboO_L.SelectedIndex = 0;
				}
			}
			else
			{
				if (e.KeyCode == Keys.Down)
				{
					if (this.comboO_L.SelectedIndex == 0)
					{
						this.comboO_L.SelectedIndex = 1;
					}
					else
					{
						this.comboO_L.SelectedIndex = 0;
					}
				}
			}
			e.Handled = true;
		}
		private void commodityCode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				try
				{
					this.commodityCode.SelectedIndex--;
					if (this.commodityCode.SelectedIndex == -1)
					{
						this.commodityCode.SelectedIndex = this.commodityCode.Items.Count - 1;
					}
					goto IL_98;
				}
				catch (Exception)
				{
					this.commodityCode.SelectedIndex = this.commodityCode.Items.Count - 1;
					goto IL_98;
				}
			}
			if (e.KeyCode == Keys.Down)
			{
				try
				{
					this.commodityCode.SelectedIndex++;
				}
				catch (Exception)
				{
					this.commodityCode.SelectedIndex = 0;
				}
			}
			IL_98:
			e.Handled = true;
		}
		private void textBoxPrice_Enter(object sender, EventArgs e)
		{
			this.textBoxPrice.Select(this.textBoxPrice.Text.Length, 0);
		}
		private void textBoxQty_Enter(object sender, EventArgs e)
		{
			this.textBoxQty.Select(this.textBoxQty.Text.Length, 0);
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void OrderMessage(long retCode, string retMessage)
		{
			try
			{
				if (OperationManager.GetInstance().orderOperation.orderType == OrderType.FormOrder)
				{
					this.OrderMessageInfo = new FormOrder.OrderMessageInfoCallBack(this.OrderInfoMessage);
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
			this.commodityCode.Focus();
			if (IniData.GetInstance().ClearData)
			{
				if (this.textBoxPrice.Enabled)
				{
					this.textBoxPrice.Text = "";
				}
				this.textBoxQty.Text = "";
			}
			if (retCode == 0L)
			{
				OperationManager.GetInstance().orderOperation.IsChangePrice = false;
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(OperationManager.GetInstance().SussceOrder, Global.RightColor, true);
					return;
				}
			}
			else
			{
				if (IniData.GetInstance().FailShowDialog && !string.IsNullOrEmpty(retMessage))
				{
					MessageBox.Show(retMessage, OperationManager.GetInstance().ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (Global.StatusInfoFill != null)
				{
					Global.StatusInfoFill(retMessage, Global.ErrorColor, true);
				}
			}
		}
	}
}
