using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
using TradeClientApp.Gnnt.ISSUE.Library;
namespace TradeClientApp.Gnnt.ISSUE
{
	public class UserSet : Form
	{
		private IContainer components;
		private TabControl tabControlSet;
		private TabPage SetCommodity;
		private TabPage SetTrancCode;
		private TabPage SetOther;
		private GroupBox groupBoxSetTranc;
		private Label labMyTranc;
		private ListBox listMyTranc;
		private ListBox listTranc;
		private Button buttonDelete;
		private Button buttonAdd;
		private Label labTranc;
		private GroupBox groupBoxLockSet;
		private NumericUpDown numericLockTime;
		private Label labelLockTime;
		private CheckBox checkBoxLock;
		private Button buttonSubmit;
		private Button buttonCancel;
		private GroupBox groupBoxSetCom;
		private Button butDelete;
		private Button butAdd;
		private ListBox listMyCommodity;
		private Label labMyCommodity;
		private Label labCommodity;
		private ListBox listCommodity;
		private CheckBox checkBoxShowDialog;
		private CheckBox cbFailDialog;
		private CheckBox chbSuccessDialog;
		private CheckBox chbAutoRefresh;
		private GroupBox gbCloseMode;
		private RadioButton rbCloseMode2;
		private RadioButton rbCloseMode3;
		private RadioButton rbCloseMode1;
		private CheckBox cbSetDoubleClick;
		private MainForm m_MainForm;
		private IniFile ini;
		private bool commUpdateFlag;
		private bool trancUpdateFlag;
		public int tabIndex
		{
			set
			{
				if (value > 0 && value < this.tabControlSet.TabPages.Count)
				{
					this.tabControlSet.SelectedIndex = value;
				}
			}
		}
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
			this.tabControlSet = new TabControl();
			this.SetCommodity = new TabPage();
			this.groupBoxSetCom = new GroupBox();
			this.butDelete = new Button();
			this.butAdd = new Button();
			this.listMyCommodity = new ListBox();
			this.labMyCommodity = new Label();
			this.labCommodity = new Label();
			this.listCommodity = new ListBox();
			this.SetTrancCode = new TabPage();
			this.groupBoxSetTranc = new GroupBox();
			this.labMyTranc = new Label();
			this.listMyTranc = new ListBox();
			this.listTranc = new ListBox();
			this.buttonDelete = new Button();
			this.buttonAdd = new Button();
			this.labTranc = new Label();
			this.SetOther = new TabPage();
			this.groupBoxLockSet = new GroupBox();
			this.cbSetDoubleClick = new CheckBox();
			this.gbCloseMode = new GroupBox();
			this.rbCloseMode2 = new RadioButton();
			this.rbCloseMode3 = new RadioButton();
			this.rbCloseMode1 = new RadioButton();
			this.chbAutoRefresh = new CheckBox();
			this.chbSuccessDialog = new CheckBox();
			this.cbFailDialog = new CheckBox();
			this.checkBoxShowDialog = new CheckBox();
			this.numericLockTime = new NumericUpDown();
			this.labelLockTime = new Label();
			this.checkBoxLock = new CheckBox();
			this.buttonSubmit = new Button();
			this.buttonCancel = new Button();
			this.tabControlSet.SuspendLayout();
			this.SetCommodity.SuspendLayout();
			this.groupBoxSetCom.SuspendLayout();
			this.SetTrancCode.SuspendLayout();
			this.groupBoxSetTranc.SuspendLayout();
			this.SetOther.SuspendLayout();
			this.groupBoxLockSet.SuspendLayout();
			this.gbCloseMode.SuspendLayout();
			((ISupportInitialize)this.numericLockTime).BeginInit();
			base.SuspendLayout();
			this.tabControlSet.Controls.Add(this.SetCommodity);
			this.tabControlSet.Controls.Add(this.SetTrancCode);
			this.tabControlSet.Controls.Add(this.SetOther);
			this.tabControlSet.Dock = DockStyle.Top;
			this.tabControlSet.Location = new Point(0, 0);
			this.tabControlSet.Name = "tabControlSet";
			this.tabControlSet.SelectedIndex = 0;
			this.tabControlSet.Size = new Size(333, 278);
			this.tabControlSet.TabIndex = 0;
			this.tabControlSet.DrawItem += new DrawItemEventHandler(this.tabControlSet_DrawItem);
			this.SetCommodity.Controls.Add(this.groupBoxSetCom);
			this.SetCommodity.Location = new Point(4, 22);
			this.SetCommodity.Name = "SetCommodity";
			this.SetCommodity.Padding = new Padding(3);
			this.SetCommodity.Size = new Size(325, 252);
			this.SetCommodity.TabIndex = 0;
			this.SetCommodity.Text = "设置自选商品";
			this.SetCommodity.UseVisualStyleBackColor = true;
			this.groupBoxSetCom.BackColor = SystemColors.Control;
			this.groupBoxSetCom.Controls.Add(this.butDelete);
			this.groupBoxSetCom.Controls.Add(this.butAdd);
			this.groupBoxSetCom.Controls.Add(this.listMyCommodity);
			this.groupBoxSetCom.Controls.Add(this.labMyCommodity);
			this.groupBoxSetCom.Controls.Add(this.labCommodity);
			this.groupBoxSetCom.Controls.Add(this.listCommodity);
			this.groupBoxSetCom.Dock = DockStyle.Fill;
			this.groupBoxSetCom.Location = new Point(3, 3);
			this.groupBoxSetCom.Name = "groupBoxSetCom";
			this.groupBoxSetCom.Size = new Size(319, 246);
			this.groupBoxSetCom.TabIndex = 22;
			this.groupBoxSetCom.TabStop = false;
			this.groupBoxSetCom.Text = "商品代码设置";
			this.butDelete.Anchor = AnchorStyles.Bottom;
			this.butDelete.ImeMode = ImeMode.NoControl;
			this.butDelete.Location = new Point(135, 150);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new Size(48, 20);
			this.butDelete.TabIndex = 5;
			this.butDelete.Text = "<<";
			this.butDelete.Click += new EventHandler(this.butDelete_Click);
			this.butAdd.Anchor = AnchorStyles.Top;
			this.butAdd.ImeMode = ImeMode.NoControl;
			this.butAdd.Location = new Point(135, 85);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new Size(48, 20);
			this.butAdd.TabIndex = 4;
			this.butAdd.Text = ">>";
			this.butAdd.Click += new EventHandler(this.butAdd_Click);
			this.listMyCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.listMyCommodity.ItemHeight = 12;
			this.listMyCommodity.Location = new Point(194, 40);
			this.listMyCommodity.Name = "listMyCommodity";
			this.listMyCommodity.Size = new Size(116, 184);
			this.listMyCommodity.TabIndex = 3;
			this.listMyCommodity.TabStop = false;
			this.listMyCommodity.DoubleClick += new EventHandler(this.butDelete_Click);
			this.labMyCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.labMyCommodity.AutoSize = true;
			this.labMyCommodity.ImeMode = ImeMode.NoControl;
			this.labMyCommodity.Location = new Point(192, 20);
			this.labMyCommodity.Name = "labMyCommodity";
			this.labMyCommodity.Size = new Size(89, 12);
			this.labMyCommodity.TabIndex = 2;
			this.labMyCommodity.Text = "自选商品代码：";
			this.labCommodity.AutoSize = true;
			this.labCommodity.ImeMode = ImeMode.NoControl;
			this.labCommodity.Location = new Point(8, 20);
			this.labCommodity.Name = "labCommodity";
			this.labCommodity.Size = new Size(89, 12);
			this.labCommodity.TabIndex = 1;
			this.labCommodity.Text = "商品代码列表：";
			this.listCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.listCommodity.ItemHeight = 12;
			this.listCommodity.Location = new Point(8, 40);
			this.listCommodity.Name = "listCommodity";
			this.listCommodity.Size = new Size(116, 184);
			this.listCommodity.TabIndex = 0;
			this.listCommodity.TabStop = false;
			this.listCommodity.DoubleClick += new EventHandler(this.butAdd_Click);
			this.SetTrancCode.Controls.Add(this.groupBoxSetTranc);
			this.SetTrancCode.Location = new Point(4, 22);
			this.SetTrancCode.Name = "SetTrancCode";
			this.SetTrancCode.Padding = new Padding(3);
			this.SetTrancCode.Size = new Size(325, 252);
			this.SetTrancCode.TabIndex = 1;
			this.SetTrancCode.Text = "设置自选交易员代码";
			this.SetTrancCode.UseVisualStyleBackColor = true;
			this.groupBoxSetTranc.BackColor = SystemColors.Control;
			this.groupBoxSetTranc.Controls.Add(this.labMyTranc);
			this.groupBoxSetTranc.Controls.Add(this.listMyTranc);
			this.groupBoxSetTranc.Controls.Add(this.listTranc);
			this.groupBoxSetTranc.Controls.Add(this.buttonDelete);
			this.groupBoxSetTranc.Controls.Add(this.buttonAdd);
			this.groupBoxSetTranc.Controls.Add(this.labTranc);
			this.groupBoxSetTranc.Dock = DockStyle.Fill;
			this.groupBoxSetTranc.Location = new Point(3, 3);
			this.groupBoxSetTranc.Name = "groupBoxSetTranc";
			this.groupBoxSetTranc.Size = new Size(319, 246);
			this.groupBoxSetTranc.TabIndex = 21;
			this.groupBoxSetTranc.TabStop = false;
			this.groupBoxSetTranc.Text = "交易代码设置";
			this.labMyTranc.AutoSize = true;
			this.labMyTranc.ImeMode = ImeMode.NoControl;
			this.labMyTranc.Location = new Point(192, 20);
			this.labMyTranc.Name = "labMyTranc";
			this.labMyTranc.Size = new Size(89, 12);
			this.labMyTranc.TabIndex = 0;
			this.labMyTranc.Text = "自选交易代码：";
			this.listMyTranc.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.listMyTranc.FormattingEnabled = true;
			this.listMyTranc.ItemHeight = 12;
			this.listMyTranc.Location = new Point(194, 40);
			this.listMyTranc.Name = "listMyTranc";
			this.listMyTranc.Size = new Size(116, 184);
			this.listMyTranc.TabIndex = 5;
			this.listMyTranc.TabStop = false;
			this.listMyTranc.DoubleClick += new EventHandler(this.buttonDelete_Click);
			this.listTranc.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.listTranc.ItemHeight = 12;
			this.listTranc.Location = new Point(8, 40);
			this.listTranc.Name = "listTranc";
			this.listTranc.Size = new Size(116, 184);
			this.listTranc.TabIndex = 4;
			this.listTranc.TabStop = false;
			this.listTranc.DoubleClick += new EventHandler(this.buttonAdd_Click);
			this.buttonDelete.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.buttonDelete.ImeMode = ImeMode.NoControl;
			this.buttonDelete.Location = new Point(135, 150);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new Size(48, 20);
			this.buttonDelete.TabIndex = 3;
			this.buttonDelete.Text = "<<";
			this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
			this.buttonAdd.ImeMode = ImeMode.NoControl;
			this.buttonAdd.Location = new Point(135, 85);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new Size(48, 20);
			this.buttonAdd.TabIndex = 2;
			this.buttonAdd.Text = ">>";
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			this.labTranc.AutoSize = true;
			this.labTranc.ImeMode = ImeMode.NoControl;
			this.labTranc.Location = new Point(8, 20);
			this.labTranc.Name = "labTranc";
			this.labTranc.Size = new Size(89, 12);
			this.labTranc.TabIndex = 0;
			this.labTranc.Text = "交易代码列表：";
			this.SetOther.Controls.Add(this.groupBoxLockSet);
			this.SetOther.Location = new Point(4, 22);
			this.SetOther.Name = "SetOther";
			this.SetOther.Size = new Size(325, 252);
			this.SetOther.TabIndex = 3;
			this.SetOther.Text = "其他设置";
			this.SetOther.UseVisualStyleBackColor = true;
			this.groupBoxLockSet.Controls.Add(this.cbSetDoubleClick);
			this.groupBoxLockSet.Controls.Add(this.gbCloseMode);
			this.groupBoxLockSet.Controls.Add(this.chbAutoRefresh);
			this.groupBoxLockSet.Controls.Add(this.chbSuccessDialog);
			this.groupBoxLockSet.Controls.Add(this.cbFailDialog);
			this.groupBoxLockSet.Controls.Add(this.checkBoxShowDialog);
			this.groupBoxLockSet.Controls.Add(this.numericLockTime);
			this.groupBoxLockSet.Controls.Add(this.labelLockTime);
			this.groupBoxLockSet.Controls.Add(this.checkBoxLock);
			this.groupBoxLockSet.Dock = DockStyle.Fill;
			this.groupBoxLockSet.Location = new Point(0, 0);
			this.groupBoxLockSet.Name = "groupBoxLockSet";
			this.groupBoxLockSet.Size = new Size(325, 252);
			this.groupBoxLockSet.TabIndex = 4;
			this.groupBoxLockSet.TabStop = false;
			this.groupBoxLockSet.Text = "其它设置";
			this.cbSetDoubleClick.AutoSize = true;
			this.cbSetDoubleClick.Location = new Point(7, 168);
			this.cbSetDoubleClick.Name = "cbSetDoubleClick";
			this.cbSetDoubleClick.Size = new Size(252, 16);
			this.cbSetDoubleClick.TabIndex = 9;
			this.cbSetDoubleClick.Text = "是否设置双击切换“固定价”与“跟随价”";
			this.cbSetDoubleClick.UseVisualStyleBackColor = true;
			this.gbCloseMode.Controls.Add(this.rbCloseMode2);
			this.gbCloseMode.Controls.Add(this.rbCloseMode3);
			this.gbCloseMode.Controls.Add(this.rbCloseMode1);
			this.gbCloseMode.Location = new Point(8, 191);
			this.gbCloseMode.Name = "gbCloseMode";
			this.gbCloseMode.Size = new Size(287, 43);
			this.gbCloseMode.TabIndex = 8;
			this.gbCloseMode.TabStop = false;
			this.gbCloseMode.Text = "卖出方式";
			this.rbCloseMode2.AutoSize = true;
			this.rbCloseMode2.Location = new Point(90, 17);
			this.rbCloseMode2.Name = "rbCloseMode2";
			this.rbCloseMode2.Size = new Size(95, 16);
			this.rbCloseMode2.TabIndex = 6;
			this.rbCloseMode2.Text = "指定时间平仓";
			this.rbCloseMode2.UseVisualStyleBackColor = true;
			this.rbCloseMode3.AutoSize = true;
			this.rbCloseMode3.Location = new Point(186, 17);
			this.rbCloseMode3.Name = "rbCloseMode3";
			this.rbCloseMode3.Size = new Size(95, 16);
			this.rbCloseMode3.TabIndex = 5;
			this.rbCloseMode3.Text = "指定价格平仓";
			this.rbCloseMode3.UseVisualStyleBackColor = true;
			this.rbCloseMode1.AutoSize = true;
			this.rbCloseMode1.Checked = true;
			this.rbCloseMode1.Location = new Point(8, 17);
			this.rbCloseMode1.Name = "rbCloseMode1";
			this.rbCloseMode1.Size = new Size(83, 16);
			this.rbCloseMode1.TabIndex = 4;
			this.rbCloseMode1.TabStop = true;
			this.rbCloseMode1.Text = "不指定平仓";
			this.rbCloseMode1.UseVisualStyleBackColor = true;
			this.chbAutoRefresh.AutoSize = true;
			this.chbAutoRefresh.Location = new Point(7, 139);
			this.chbAutoRefresh.Name = "chbAutoRefresh";
			this.chbAutoRefresh.Size = new Size(144, 16);
			this.chbAutoRefresh.TabIndex = 7;
			this.chbAutoRefresh.Text = "是否自动刷新交易数据";
			this.chbAutoRefresh.UseVisualStyleBackColor = true;
			this.chbSuccessDialog.AutoSize = true;
			this.chbSuccessDialog.Location = new Point(7, 110);
			this.chbSuccessDialog.Name = "chbSuccessDialog";
			this.chbSuccessDialog.Size = new Size(216, 16);
			this.chbSuccessDialog.TabIndex = 6;
			this.chbSuccessDialog.Text = "成交后是否弹出成交回报提示对话框";
			this.chbSuccessDialog.UseVisualStyleBackColor = true;
			this.cbFailDialog.AutoSize = true;
			this.cbFailDialog.Location = new Point(7, 81);
			this.cbFailDialog.Name = "cbFailDialog";
			this.cbFailDialog.Size = new Size(240, 16);
			this.cbFailDialog.TabIndex = 5;
			this.cbFailDialog.Text = "下单失败后是否使用对话框弹出错误信息";
			this.cbFailDialog.UseVisualStyleBackColor = true;
			this.checkBoxShowDialog.AutoSize = true;
			this.checkBoxShowDialog.Location = new Point(7, 52);
			this.checkBoxShowDialog.Name = "checkBoxShowDialog";
			this.checkBoxShowDialog.Size = new Size(168, 16);
			this.checkBoxShowDialog.TabIndex = 2;
			this.checkBoxShowDialog.Text = "下单后是否弹出确认对话框";
			this.checkBoxShowDialog.UseVisualStyleBackColor = true;
			this.numericLockTime.Location = new Point(109, 21);
			NumericUpDown arg_11AF_0 = this.numericLockTime;
			int[] array = new int[4];
			array[0] = 60;
			arg_11AF_0.Maximum = new decimal(array);
			NumericUpDown arg_11CB_0 = this.numericLockTime;
			int[] array2 = new int[4];
			array2[0] = 1;
			arg_11CB_0.Minimum = new decimal(array2);
			this.numericLockTime.Name = "numericLockTime";
			this.numericLockTime.Size = new Size(49, 21);
			this.numericLockTime.TabIndex = 1;
			NumericUpDown arg_1217_0 = this.numericLockTime;
			int[] array3 = new int[4];
			array3[0] = 1;
			arg_1217_0.Value = new decimal(array3);
			this.numericLockTime.KeyUp += new KeyEventHandler(this.numericLockTime_KeyUp);
			this.labelLockTime.AutoSize = true;
			this.labelLockTime.ImeMode = ImeMode.NoControl;
			this.labelLockTime.Location = new Point(165, 24);
			this.labelLockTime.Name = "labelLockTime";
			this.labelLockTime.Size = new Size(29, 12);
			this.labelLockTime.TabIndex = 2;
			this.labelLockTime.Text = "分钟";
			this.checkBoxLock.AutoSize = true;
			this.checkBoxLock.Checked = true;
			this.checkBoxLock.CheckState = CheckState.Checked;
			this.checkBoxLock.ImeMode = ImeMode.NoControl;
			this.checkBoxLock.Location = new Point(7, 23);
			this.checkBoxLock.Name = "checkBoxLock";
			this.checkBoxLock.Size = new Size(96, 16);
			this.checkBoxLock.TabIndex = 0;
			this.checkBoxLock.Text = "是否自动锁定";
			this.checkBoxLock.UseVisualStyleBackColor = true;
			this.checkBoxLock.CheckedChanged += new EventHandler(this.checkBoxLock_CheckedChanged);
			this.buttonSubmit.Anchor = AnchorStyles.Bottom;
			this.buttonSubmit.ImeMode = ImeMode.NoControl;
			this.buttonSubmit.Location = new Point(206, 285);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new Size(55, 20);
			this.buttonSubmit.TabIndex = 3;
			this.buttonSubmit.Text = "确  定";
			this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
			this.buttonCancel.Anchor = AnchorStyles.Bottom;
			this.buttonCancel.ImeMode = ImeMode.NoControl;
			this.buttonCancel.Location = new Point(269, 285);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(55, 20);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "取  消";
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(333, 311);
			base.Controls.Add(this.buttonSubmit);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.tabControlSet);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UserSet";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "设置";
			base.Load += new EventHandler(this.UserSet_Load);
			this.tabControlSet.ResumeLayout(false);
			this.SetCommodity.ResumeLayout(false);
			this.groupBoxSetCom.ResumeLayout(false);
			this.groupBoxSetCom.PerformLayout();
			this.SetTrancCode.ResumeLayout(false);
			this.groupBoxSetTranc.ResumeLayout(false);
			this.groupBoxSetTranc.PerformLayout();
			this.SetOther.ResumeLayout(false);
			this.groupBoxLockSet.ResumeLayout(false);
			this.groupBoxLockSet.PerformLayout();
			this.gbCloseMode.ResumeLayout(false);
			this.gbCloseMode.PerformLayout();
			((ISupportInitialize)this.numericLockTime).EndInit();
			base.ResumeLayout(false);
		}
		public UserSet(MainForm mainForm)
		{
			this.InitializeComponent();
			this.m_MainForm = mainForm;
			this.ini = new IniFile(Global.ConfigPath + "ISSUE_Trade.ini");
		}
		private void UserSet_Load(object sender, EventArgs e)
		{
			try
			{
				this.SetControlText();
				this.checkBoxLock.Checked = IniData.GetInstance().LockEnable;
				this.numericLockTime.Visible = IniData.GetInstance().LockEnable;
				this.labelLockTime.Visible = IniData.GetInstance().LockEnable;
				this.numericLockTime.Value = IniData.GetInstance().LockTime;
				this.checkBoxShowDialog.Checked = IniData.GetInstance().ShowDialog;
				this.cbFailDialog.Checked = IniData.GetInstance().FailShowDialog;
				this.chbSuccessDialog.Checked = IniData.GetInstance().SuccessShowDialog;
				this.chbAutoRefresh.Checked = IniData.GetInstance().AutoRefresh;
				this.cbSetDoubleClick.Checked = IniData.GetInstance().SetDoubleClick;
				switch (IniData.GetInstance().CloseMode)
				{
				case 1:
					this.rbCloseMode1.Checked = true;
					break;
				case 2:
					this.rbCloseMode2.Checked = true;
					break;
				case 3:
					this.rbCloseMode3.Checked = true;
					break;
				default:
					this.rbCloseMode1.Checked = true;
					break;
				}
				this.listCommodity.Items.Clear();
				this.listMyCommodity.Items.Clear();
				foreach (DataRow dataRow in this.m_MainForm.dsCommodity.Tables[0].Rows)
				{
					if (Global.MarketHT.Count > 1)
					{
						if ((bool)dataRow["Flag"] && dataRow["MarKet"].ToString().Equals(this.m_MainForm.marketID))
						{
							this.listMyCommodity.Items.Add(dataRow["commodityCode"].ToString());
						}
						else if (dataRow["MarKet"].ToString().Equals(this.m_MainForm.marketID))
						{
							this.listCommodity.Items.Add(dataRow["commodityCode"].ToString());
						}
					}
					else if ((bool)dataRow["Flag"])
					{
						this.listMyCommodity.Items.Add(dataRow["commodityCode"].ToString());
					}
					else
					{
						this.listCommodity.Items.Add(dataRow["commodityCode"].ToString());
					}
				}
				this.listTranc.Items.Clear();
				this.listMyTranc.Items.Clear();
				foreach (DataRow dataRow2 in this.m_MainForm.dsTransactions.Tables[0].Rows)
				{
					if ((bool)dataRow2["Flag"])
					{
						this.listMyTranc.Items.Add(dataRow2["TransactionsCode"].ToString());
					}
					else
					{
						this.listTranc.Items.Add(dataRow2["TransactionsCode"].ToString());
					}
				}
				ScaleForm.ScaleForms(this);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.ToString());
			}
		}
		private void butAdd_Click(object sender, EventArgs e)
		{
			if (this.listCommodity.SelectedIndex == -1)
			{
				MessageBox.Show("请选择商品代码！");
				return;
			}
			this.commUpdateFlag = true;
			this.listMyCommodity.Items.Add(this.listCommodity.Text);
			int selectedIndex = this.listCommodity.SelectedIndex;
			this.listCommodity.Items.RemoveAt(this.listCommodity.SelectedIndex);
			if (selectedIndex > -1)
			{
				if (selectedIndex >= this.listCommodity.Items.Count)
				{
					this.listCommodity.SelectedIndex = selectedIndex - 1;
					return;
				}
				this.listCommodity.SelectedIndex = selectedIndex;
			}
		}
		private void butDelete_Click(object sender, EventArgs e)
		{
			if (this.listMyCommodity.SelectedIndex == -1)
			{
				MessageBox.Show("请选择商品代码！");
				return;
			}
			this.commUpdateFlag = true;
			this.listCommodity.Items.Add(this.listMyCommodity.Text);
			int selectedIndex = this.listMyCommodity.SelectedIndex;
			this.listMyCommodity.Items.RemoveAt(this.listMyCommodity.SelectedIndex);
			if (selectedIndex > -1)
			{
				if (selectedIndex >= this.listMyCommodity.Items.Count)
				{
					this.listMyCommodity.SelectedIndex = selectedIndex - 1;
					return;
				}
				this.listMyCommodity.SelectedIndex = selectedIndex;
			}
		}
		private void buttonAdd_Click(object sender, EventArgs e)
		{
			if (this.listTranc.SelectedIndex == -1)
			{
				MessageBox.Show("请选择交易代码！");
				return;
			}
			this.trancUpdateFlag = true;
			this.listMyTranc.Items.Add(this.listTranc.Text);
			int selectedIndex = this.listTranc.SelectedIndex;
			this.listTranc.Items.RemoveAt(this.listTranc.SelectedIndex);
			if (selectedIndex > -1)
			{
				if (selectedIndex >= this.listTranc.Items.Count)
				{
					this.listTranc.SelectedIndex = selectedIndex - 1;
					return;
				}
				this.listTranc.SelectedIndex = selectedIndex;
			}
		}
		private void buttonDelete_Click(object sender, EventArgs e)
		{
			if (this.listMyTranc.SelectedIndex == -1)
			{
				MessageBox.Show("请选择交易代码！");
				return;
			}
			this.trancUpdateFlag = true;
			this.listTranc.Items.Add(this.listMyTranc.Text);
			int selectedIndex = this.listMyTranc.SelectedIndex;
			this.listMyTranc.Items.RemoveAt(this.listMyTranc.SelectedIndex);
			if (selectedIndex > -1)
			{
				if (selectedIndex >= this.listMyTranc.Items.Count)
				{
					this.listMyTranc.SelectedIndex = selectedIndex - 1;
					return;
				}
				this.listMyTranc.SelectedIndex = selectedIndex;
			}
		}
		private void checkBoxLock_CheckedChanged(object sender, EventArgs e)
		{
			this.numericLockTime.Visible = this.checkBoxLock.Checked;
			this.labelLockTime.Visible = this.checkBoxLock.Checked;
		}
		private void numericLockTime_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numericLockTime.Value > this.numericLockTime.Maximum)
			{
				this.numericLockTime.Value = this.numericLockTime.Maximum;
				e.Handled = true;
			}
		}
		private void buttonSubmit_Click(object sender, EventArgs e)
		{
			bool flag = true;
			this.m_MainForm.dsCommodity = this.m_MainForm.XmlCommodity.GetDataSetByXml();
			this.m_MainForm.dsTransactions = this.m_MainForm.XmlTransactions.GetDataSetByXml();
			string text = string.Empty;
			if (this.commUpdateFlag)
			{
				if (this.SubmitCommUpdate())
				{
					text = "商品列表修改信息：成功！\r\n";
				}
				else
				{
					text = "商品列表修改信息：失败！\r\n";
					flag = false;
				}
				this.commUpdateFlag = false;
			}
			if (this.trancUpdateFlag)
			{
				if (this.SubmitTrancUpdate())
				{
					text += "交易代码列表修改信息：成功！\r\n";
				}
				else
				{
					text += "交易代码列表修改信息：失败！\r\n";
					flag = false;
				}
				this.trancUpdateFlag = false;
			}
			if (this.checkBoxLock.Checked != IniData.GetInstance().LockEnable || this.numericLockTime.Value != IniData.GetInstance().LockTime)
			{
				IniData.GetInstance().LockEnable = this.checkBoxLock.Checked;
				IniData.GetInstance().LockTime = (int)this.numericLockTime.Value;
				this.ini.IniWriteValue("LockSet", "LockEnable", this.checkBoxLock.Checked.ToString());
				this.ini.IniWriteValue("LockSet", "LockTime", this.numericLockTime.Value.ToString());
				text += "\r\n锁定设置修改信息：成功！\r\n";
			}
			if (this.checkBoxShowDialog.Checked != IniData.GetInstance().ShowDialog)
			{
				IniData.GetInstance().ShowDialog = this.checkBoxShowDialog.Checked;
				this.ini.IniWriteValue("Set", "ShowDialog", IniData.GetInstance().ShowDialog.ToString());
				text += "\r\n是否弹出下单确认框修改信息：成功！\r\n";
			}
			if (this.cbFailDialog.Checked != IniData.GetInstance().FailShowDialog)
			{
				IniData.GetInstance().FailShowDialog = this.cbFailDialog.Checked;
				this.ini.IniWriteValue("Set", "FailShowDialog", IniData.GetInstance().FailShowDialog.ToString());
				text += "\r\n下单失败后是否使用对话框弹出错误修改信息：成功！\r\n";
			}
			if (this.chbSuccessDialog.Checked != IniData.GetInstance().SuccessShowDialog)
			{
				IniData.GetInstance().SuccessShowDialog = this.chbSuccessDialog.Checked;
				this.ini.IniWriteValue("Set", "SuccessShowDialog", IniData.GetInstance().SuccessShowDialog.ToString());
				text += "\r\n成交后是否弹出成交回报提示对话框修改信息：成功！\r\n";
			}
			if (this.chbAutoRefresh.Checked != IniData.GetInstance().AutoRefresh)
			{
				IniData.GetInstance().AutoRefresh = this.chbAutoRefresh.Checked;
				text += "\r\n是否自动刷新交易数据修改信息：成功！\r\n";
			}
			if (this.cbSetDoubleClick.Checked != IniData.GetInstance().SetDoubleClick)
			{
				IniData.GetInstance().SetDoubleClick = this.cbSetDoubleClick.Checked;
				this.ini.IniWriteValue("Set", "SetDoubleClick", IniData.GetInstance().SetDoubleClick.ToString());
				text += "\r\n是否设置双击切换“固定价”与“跟随价”修改信息：成功！\r\n";
			}
			int num;
			if (this.rbCloseMode1.Checked)
			{
				num = 1;
			}
			else if (this.rbCloseMode2.Checked)
			{
				num = 2;
			}
			else
			{
				num = 3;
			}
			if (num != IniData.GetInstance().CloseMode)
			{
				IniData.GetInstance().CloseMode = num;
				this.m_MainForm.radioL_CheckedChanged(this, null);
				text += "\r\n设置平仓方式修改信息：成功！\r\n";
			}
			if (text != "")
			{
				MessageBox.Show(text, "修改结果", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			if (flag)
			{
				base.Close();
			}
		}
		private bool SubmitCommUpdate()
		{
			string[] columns = new string[]
			{
				"Flag"
			};
			string[] columnValue = new string[]
			{
				"true"
			};
			string[] columnValue2 = new string[]
			{
				"false"
			};
			bool result = true;
			if (this.listCommodity.Items.Count > this.listMyCommodity.Items.Count)
			{
				this.m_MainForm.XmlCommodity.UpdateXmlRow(columns, columnValue2, "Flag", "True");
				for (int i = 0; i < this.listMyCommodity.Items.Count; i++)
				{
					if (!this.m_MainForm.XmlCommodity.UpdateXmlRow(columns, columnValue, "commodityCode", this.listMyCommodity.Items[i].ToString()))
					{
						result = false;
					}
				}
			}
			else
			{
				this.m_MainForm.XmlCommodity.UpdateXmlRow(columns, columnValue, "Flag", "False");
				for (int j = 0; j < this.listCommodity.Items.Count; j++)
				{
					if (!this.m_MainForm.XmlCommodity.UpdateXmlRow(columns, columnValue2, "commodityCode", this.listCommodity.Items[j].ToString()))
					{
						result = false;
					}
				}
			}
			this.m_MainForm.comboCommodityRefresh();
			return result;
		}
		private bool SubmitTrancUpdate()
		{
			string[] columns = new string[]
			{
				"Flag"
			};
			string[] columnValue = new string[]
			{
				"true"
			};
			string[] columnValue2 = new string[]
			{
				"false"
			};
			bool result = true;
			if (this.listTranc.Items.Count > this.listMyTranc.Items.Count)
			{
				this.m_MainForm.XmlTransactions.UpdateXmlRow(columns, columnValue2, "Flag", "True");
				for (int i = 0; i < this.listMyTranc.Items.Count; i++)
				{
					if (!this.m_MainForm.XmlTransactions.UpdateXmlRow(columns, columnValue, "TransactionsCode", this.listMyTranc.Items[i].ToString()))
					{
						result = false;
					}
				}
			}
			else
			{
				this.m_MainForm.XmlTransactions.UpdateXmlRow(columns, columnValue, "Flag", "False");
				for (int j = 0; j < this.listTranc.Items.Count; j++)
				{
					if (!this.m_MainForm.XmlTransactions.UpdateXmlRow(columns, columnValue2, "TransactionsCode", this.listTranc.Items[j].ToString()))
					{
						result = false;
					}
				}
			}
			this.m_MainForm.comboTrancRefresh();
			return result;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void SetControlText()
		{
			this.SetCommodity.Text = Global.M_ResourceManager.GetString("TradeStr_SetCommodity");
			this.SetTrancCode.Text = Global.M_ResourceManager.GetString("TradeStr_SetTrancCode");
			this.SetOther.Text = Global.M_ResourceManager.GetString("TradeStr_SetOther");
			this.groupBoxSetCom.Text = Global.M_ResourceManager.GetString("TradeStr_groupBoxSetCom");
			this.labCommodity.Text = Global.M_ResourceManager.GetString("TradeStr_labCommodity");
			this.labMyCommodity.Text = Global.M_ResourceManager.GetString("TradeStr_labMyCommodity");
			this.groupBoxSetTranc.Text = Global.M_ResourceManager.GetString("TradeStr_groupBoxSetTranc");
			this.labTranc.Text = Global.M_ResourceManager.GetString("TradeStr_labTranc");
			this.labMyTranc.Text = Global.M_ResourceManager.GetString("TradeStr_labMyTranc");
			this.groupBoxLockSet.Text = Global.M_ResourceManager.GetString("TradeStr_groupBoxLockSet");
			this.checkBoxLock.Text = Global.M_ResourceManager.GetString("TradeStr_checkBoxLock");
			this.labelLockTime.Text = Global.M_ResourceManager.GetString("TradeStr_labelLockTime");
			this.checkBoxShowDialog.Text = Global.M_ResourceManager.GetString("TradeStr_checkBoxShowDialog");
			this.cbFailDialog.Text = Global.M_ResourceManager.GetString("TradeStr_cbFailDialog");
			this.chbSuccessDialog.Text = Global.M_ResourceManager.GetString("TradeStr_chbSuccessDialog");
			this.buttonSubmit.Text = Global.M_ResourceManager.GetString("TradeStr_buttonSubmit");
			this.buttonCancel.Text = Global.M_ResourceManager.GetString("TradeStr_buttonCancel");
			if (!Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
			{
				this.cbSetDoubleClick.Visible = false;
			}
			this.rbCloseMode1.Text = Global.CloseModeStrArr[0];
			this.rbCloseMode2.Text = Global.CloseModeStrArr[1];
			this.rbCloseMode3.Text = Global.CloseModeStrArr[2];
			if (Global.CustomerCount < 2)
			{
				this.tabControlSet.Controls.Remove(this.SetTrancCode);
			}
			if (!Global.HTConfig.Contains("CloseMode"))
			{
				return;
			}
			if (Global.HTConfig["CloseMode"].ToString().Equals("1"))
			{
				this.gbCloseMode.Visible = false;
				return;
			}
			if (Global.HTConfig["CloseMode"].ToString().Equals("2"))
			{
				this.rbCloseMode3.Visible = false;
				this.rbCloseMode1.Location = new Point(30, 17);
				this.rbCloseMode2.Location = new Point(164, 17);
				return;
			}
			if (Global.HTConfig["CloseMode"].ToString().Equals("3"))
			{
				this.rbCloseMode2.Visible = false;
				this.rbCloseMode1.Location = new Point(30, 17);
				this.rbCloseMode3.Location = new Point(164, 17);
			}
		}
		private void SetSkin()
		{
			base.Icon = (Icon)Global.M_ResourceManager.GetObject("Logo.ico");
			this.BackgroundImage = ControlLayout.SkinImage;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			if (this.tabControlSet.DrawMode == TabDrawMode.Normal)
			{
				this.tabControlSet.DrawMode = TabDrawMode.OwnerDrawFixed;
			}
			for (int i = 0; i < this.tabControlSet.TabCount; i++)
			{
				foreach (Control control in this.tabControlSet.TabPages[i].Controls)
				{
					this.tabControlSet.TabPages[i].BackColor = Color.Transparent;
					this.tabControlSet.TabPages[i].BackgroundImage = ControlLayout.SkinImage;
					this.tabControlSet.TabPages[i].BackgroundImageLayout = ImageLayout.Stretch;
					if (control is GroupBox)
					{
						control.BackColor = Color.Transparent;
					}
				}
			}
			this.buttonSubmit.BackgroundImage = ControlLayout.SkinImage;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			this.buttonCancel.BackgroundImage = ControlLayout.SkinImage;
			this.BackgroundImageLayout = ImageLayout.Stretch;
		}
		private void tabControlSet_DrawItem(object sender, DrawItemEventArgs e)
		{
			for (int i = 0; i < this.tabControlSet.TabCount; i++)
			{
				this.DrawTab(e.Graphics, this.tabControlSet.TabPages[i], i);
			}
		}
		internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
		{
			Rectangle tabRect = this.tabControlSet.GetTabRect(nIndex);
			RectangleF layoutRectangle = this.tabControlSet.GetTabRect(nIndex);
			bool flag = this.tabControlSet.SelectedIndex == nIndex;
			Point[] array = new Point[7];
			if (this.tabControlSet.Alignment == TabAlignment.Top)
			{
				array[0] = new Point(tabRect.Left, tabRect.Bottom);
				array[1] = new Point(tabRect.Left, tabRect.Top + 3);
				array[2] = new Point(tabRect.Left + 3, tabRect.Top);
				array[3] = new Point(tabRect.Right - 3, tabRect.Top);
				array[4] = new Point(tabRect.Right, tabRect.Top + 3);
				array[5] = new Point(tabRect.Right, tabRect.Bottom);
				array[6] = new Point(tabRect.Left, tabRect.Bottom);
			}
			else
			{
				array[0] = new Point(tabRect.Left, tabRect.Top);
				array[1] = new Point(tabRect.Right, tabRect.Top);
				array[2] = new Point(tabRect.Right, tabRect.Bottom - 3);
				array[3] = new Point(tabRect.Right - 3, tabRect.Bottom);
				array[4] = new Point(tabRect.Left + 3, tabRect.Bottom);
				array[5] = new Point(tabRect.Left, tabRect.Bottom - 3);
				array[6] = new Point(tabRect.Left, tabRect.Top);
			}
			Brush brush = new TextureBrush(ControlLayout.SkinImage);
			g.FillPolygon(brush, array);
			g.DrawPolygon(SystemPens.ButtonHighlight, array);
			if (flag)
			{
				Pen pen = new Pen(tabPage.BackColor);
				switch (this.tabControlSet.Alignment)
				{
				case TabAlignment.Top:
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Bottom, tabRect.Right - 1, tabRect.Bottom);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Bottom + 1, tabRect.Right - 1, tabRect.Bottom + 1);
					break;
				case TabAlignment.Bottom:
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top, tabRect.Right - 1, tabRect.Top);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top - 1, tabRect.Right - 1, tabRect.Top - 1);
					g.DrawLine(pen, tabRect.Left + 1, tabRect.Top - 2, tabRect.Right - 1, tabRect.Top - 2);
					break;
				}
				pen.Dispose();
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			brush = new SolidBrush(tabPage.ForeColor);
			g.DrawString(tabPage.Text, this.Font, brush, layoutRectangle, stringFormat);
			brush.Dispose();
		}
		private void textAfmPwd_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '\b')
			{
				MessageBox.Show("只能输入数字和字母!");
				e.Handled = true;
			}
		}
	}
}
