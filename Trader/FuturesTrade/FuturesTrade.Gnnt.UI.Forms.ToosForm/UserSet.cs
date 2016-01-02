using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class UserSet : Form
	{
		private IContainer components;
		private TabControl tabControlSet;
		private TabPage SetCommodity;
		private GroupBox groupBoxSetCom;
		private Button butDelete;
		private Button butAdd;
		private ListBox listMyCommodity;
		private Label labMyCommodity;
		private Label labCommodity;
		private ListBox listCommodity;
		private TabPage SetTrancCode;
		private GroupBox groupBoxSetTranc;
		private Label labMyTranc;
		private ListBox listMyTranc;
		private ListBox listTranc;
		private Button buttonDelete;
		private Button buttonAdd;
		private Label labTranc;
		private TabPage SetOther;
		private GroupBox groupBoxLockSet;
		internal CheckBox checkBoxAutoPrice;
		private Panel panel1;
		private RadioButton rBLanguageE;
		private RadioButton rBLanguageC;
		private Label labelLanguage;
		private CheckBox chbClearData;
		internal CheckBox cbSetDoubleClick;
		internal GroupBox gbCloseMode;
		private RadioButton rbCloseMode2;
		private RadioButton rbCloseMode3;
		private RadioButton rbCloseMode1;
		private CheckBox chbAutoRefresh;
		private CheckBox chbSuccessDialog;
		private CheckBox cbFailDialog;
		private CheckBox checkBoxShowDialog;
		private NumericUpDown numericLockTime;
		private Label labelLockTime;
		private CheckBox checkBoxLock;
		private Button buttonStyle;
		private Button buttonSubmit;
		private Button buttonCancel;
		private int formStyle;
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
			this.checkBoxAutoPrice = new CheckBox();
			this.panel1 = new Panel();
			this.rBLanguageE = new RadioButton();
			this.rBLanguageC = new RadioButton();
			this.labelLanguage = new Label();
			this.chbClearData = new CheckBox();
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
			this.buttonStyle = new Button();
			this.buttonSubmit = new Button();
			this.buttonCancel = new Button();
			this.tabControlSet.SuspendLayout();
			this.SetCommodity.SuspendLayout();
			this.groupBoxSetCom.SuspendLayout();
			this.SetTrancCode.SuspendLayout();
			this.groupBoxSetTranc.SuspendLayout();
			this.SetOther.SuspendLayout();
			this.groupBoxLockSet.SuspendLayout();
			this.panel1.SuspendLayout();
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
			this.tabControlSet.Size = new Size(595, 343);
			this.tabControlSet.TabIndex = 1;
			this.SetCommodity.Controls.Add(this.groupBoxSetCom);
			this.SetCommodity.Location = new Point(4, 22);
			this.SetCommodity.Name = "SetCommodity";
			this.SetCommodity.Padding = new Padding(3);
			this.SetCommodity.Size = new Size(587, 317);
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
			this.groupBoxSetCom.Size = new Size(581, 311);
			this.groupBoxSetCom.TabIndex = 22;
			this.groupBoxSetCom.TabStop = false;
			this.groupBoxSetCom.Text = "商品代码设置";
			this.butDelete.Anchor = AnchorStyles.Bottom;
			this.butDelete.ImeMode = ImeMode.NoControl;
			this.butDelete.Location = new Point(266, 215);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new Size(48, 20);
			this.butDelete.TabIndex = 5;
			this.butDelete.Text = "<<";
			this.butDelete.Click += new EventHandler(this.butDelete_Click);
			this.butAdd.Anchor = AnchorStyles.Top;
			this.butAdd.ImeMode = ImeMode.NoControl;
			this.butAdd.Location = new Point(266, 85);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new Size(48, 20);
			this.butAdd.TabIndex = 4;
			this.butAdd.Text = ">>";
			this.butAdd.Click += new EventHandler(this.butAdd_Click);
			this.listMyCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.listMyCommodity.ItemHeight = 12;
			this.listMyCommodity.Location = new Point(456, 40);
			this.listMyCommodity.Name = "listMyCommodity";
			this.listMyCommodity.Size = new Size(116, 232);
			this.listMyCommodity.TabIndex = 3;
			this.listMyCommodity.TabStop = false;
			this.listMyCommodity.DoubleClick += new EventHandler(this.butDelete_Click);
			this.labMyCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.labMyCommodity.AutoSize = true;
			this.labMyCommodity.ImeMode = ImeMode.NoControl;
			this.labMyCommodity.Location = new Point(454, 20);
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
			this.listCommodity.Size = new Size(116, 232);
			this.listCommodity.TabIndex = 0;
			this.listCommodity.TabStop = false;
			this.listCommodity.DoubleClick += new EventHandler(this.butAdd_Click);
			this.SetTrancCode.Controls.Add(this.groupBoxSetTranc);
			this.SetTrancCode.Location = new Point(4, 22);
			this.SetTrancCode.Name = "SetTrancCode";
			this.SetTrancCode.Padding = new Padding(3);
			this.SetTrancCode.Size = new Size(587, 317);
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
			this.groupBoxSetTranc.Size = new Size(581, 311);
			this.groupBoxSetTranc.TabIndex = 21;
			this.groupBoxSetTranc.TabStop = false;
			this.groupBoxSetTranc.Text = "交易代码设置";
			this.labMyTranc.AutoSize = true;
			this.labMyTranc.ImeMode = ImeMode.NoControl;
			this.labMyTranc.Location = new Point(454, 17);
			this.labMyTranc.Name = "labMyTranc";
			this.labMyTranc.Size = new Size(89, 12);
			this.labMyTranc.TabIndex = 0;
			this.labMyTranc.Text = "自选交易代码：";
			this.listMyTranc.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.listMyTranc.FormattingEnabled = true;
			this.listMyTranc.ItemHeight = 12;
			this.listMyTranc.Location = new Point(456, 40);
			this.listMyTranc.Name = "listMyTranc";
			this.listMyTranc.Size = new Size(116, 232);
			this.listMyTranc.TabIndex = 5;
			this.listMyTranc.TabStop = false;
			this.listMyTranc.Click += new EventHandler(this.buttonDelete_Click);
			this.listTranc.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.listTranc.ItemHeight = 12;
			this.listTranc.Location = new Point(8, 40);
			this.listTranc.Name = "listTranc";
			this.listTranc.Size = new Size(116, 232);
			this.listTranc.TabIndex = 4;
			this.listTranc.TabStop = false;
			this.listTranc.Click += new EventHandler(this.buttonAdd_Click);
			this.buttonDelete.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.buttonDelete.ImeMode = ImeMode.NoControl;
			this.buttonDelete.Location = new Point(270, 211);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new Size(48, 20);
			this.buttonDelete.TabIndex = 3;
			this.buttonDelete.Text = "<<";
			this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
			this.buttonAdd.ImeMode = ImeMode.NoControl;
			this.buttonAdd.Location = new Point(270, 89);
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
			this.SetOther.Size = new Size(587, 317);
			this.SetOther.TabIndex = 3;
			this.SetOther.Text = "其他设置";
			this.SetOther.UseVisualStyleBackColor = true;
			this.groupBoxLockSet.Controls.Add(this.checkBoxAutoPrice);
			this.groupBoxLockSet.Controls.Add(this.panel1);
			this.groupBoxLockSet.Controls.Add(this.labelLanguage);
			this.groupBoxLockSet.Controls.Add(this.chbClearData);
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
			this.groupBoxLockSet.Size = new Size(587, 317);
			this.groupBoxLockSet.TabIndex = 4;
			this.groupBoxLockSet.TabStop = false;
			this.groupBoxLockSet.Text = "其它设置";
			this.checkBoxAutoPrice.AutoSize = true;
			this.checkBoxAutoPrice.Location = new Point(272, 22);
			this.checkBoxAutoPrice.Name = "checkBoxAutoPrice";
			this.checkBoxAutoPrice.Size = new Size(312, 28);
			this.checkBoxAutoPrice.TabIndex = 13;
			this.checkBoxAutoPrice.Text = "正常委托时下单后、切换商品或买卖方式自动填入价格\r\n（优先顺序：对手价、最新价、昨结价）";
			this.checkBoxAutoPrice.UseVisualStyleBackColor = true;
			this.checkBoxAutoPrice.CheckedChanged += new EventHandler(this.checkBoxAutoPrice_CheckedChanged);
			this.panel1.Controls.Add(this.rBLanguageE);
			this.panel1.Controls.Add(this.rBLanguageC);
			this.panel1.Location = new Point(73, 253);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(186, 26);
			this.panel1.TabIndex = 12;
			this.panel1.Visible = false;
			this.rBLanguageE.AutoSize = true;
			this.rBLanguageE.Location = new Point(94, 5);
			this.rBLanguageE.Name = "rBLanguageE";
			this.rBLanguageE.Size = new Size(47, 16);
			this.rBLanguageE.TabIndex = 1;
			this.rBLanguageE.Text = "英文";
			this.rBLanguageE.UseVisualStyleBackColor = true;
			this.rBLanguageC.AutoSize = true;
			this.rBLanguageC.Checked = true;
			this.rBLanguageC.Location = new Point(11, 5);
			this.rBLanguageC.Name = "rBLanguageC";
			this.rBLanguageC.Size = new Size(47, 16);
			this.rBLanguageC.TabIndex = 0;
			this.rBLanguageC.TabStop = true;
			this.rBLanguageC.Text = "中文";
			this.rBLanguageC.UseVisualStyleBackColor = true;
			this.labelLanguage.AutoSize = true;
			this.labelLanguage.Location = new Point(14, 260);
			this.labelLanguage.Name = "labelLanguage";
			this.labelLanguage.Size = new Size(53, 12);
			this.labelLanguage.TabIndex = 11;
			this.labelLanguage.Text = "语言类型";
			this.labelLanguage.Visible = false;
			this.chbClearData.AutoSize = true;
			this.chbClearData.Location = new Point(7, 159);
			this.chbClearData.Name = "chbClearData";
			this.chbClearData.Size = new Size(168, 16);
			this.chbClearData.TabIndex = 10;
			this.chbClearData.Text = "下单成功后清空价格和数量";
			this.chbClearData.UseVisualStyleBackColor = true;
			this.chbClearData.CheckedChanged += new EventHandler(this.chbClearData_CheckedChanged);
			this.cbSetDoubleClick.AutoSize = true;
			this.cbSetDoubleClick.Location = new Point(7, 184);
			this.cbSetDoubleClick.Name = "cbSetDoubleClick";
			this.cbSetDoubleClick.Size = new Size(252, 16);
			this.cbSetDoubleClick.TabIndex = 9;
			this.cbSetDoubleClick.Text = "是否设置双击切换“固定价”与“跟随价”";
			this.cbSetDoubleClick.UseVisualStyleBackColor = true;
			this.gbCloseMode.Controls.Add(this.rbCloseMode2);
			this.gbCloseMode.Controls.Add(this.rbCloseMode3);
			this.gbCloseMode.Controls.Add(this.rbCloseMode1);
			this.gbCloseMode.Location = new Point(8, 204);
			this.gbCloseMode.Name = "gbCloseMode";
			this.gbCloseMode.Size = new Size(287, 43);
			this.gbCloseMode.TabIndex = 8;
			this.gbCloseMode.TabStop = false;
			this.gbCloseMode.Text = "转让方式";
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
			this.chbAutoRefresh.Location = new Point(7, 132);
			this.chbAutoRefresh.Name = "chbAutoRefresh";
			this.chbAutoRefresh.Size = new Size(120, 16);
			this.chbAutoRefresh.TabIndex = 7;
			this.chbAutoRefresh.Text = "自动刷新交易数据";
			this.chbAutoRefresh.UseVisualStyleBackColor = true;
			this.chbSuccessDialog.AutoSize = true;
			this.chbSuccessDialog.Location = new Point(7, 106);
			this.chbSuccessDialog.Name = "chbSuccessDialog";
			this.chbSuccessDialog.Size = new Size(216, 16);
			this.chbSuccessDialog.TabIndex = 6;
			this.chbSuccessDialog.Text = "成交后是否弹出成交回报提示对话框";
			this.chbSuccessDialog.UseVisualStyleBackColor = true;
			this.cbFailDialog.AutoSize = true;
			this.cbFailDialog.Location = new Point(7, 79);
			this.cbFailDialog.Name = "cbFailDialog";
			this.cbFailDialog.Size = new Size(216, 16);
			this.cbFailDialog.TabIndex = 5;
			this.cbFailDialog.Text = "下单失败后使用对话框弹出错误信息";
			this.cbFailDialog.UseVisualStyleBackColor = true;
			this.checkBoxShowDialog.AutoSize = true;
			this.checkBoxShowDialog.Location = new Point(7, 52);
			this.checkBoxShowDialog.Name = "checkBoxShowDialog";
			this.checkBoxShowDialog.Size = new Size(144, 16);
			this.checkBoxShowDialog.TabIndex = 2;
			this.checkBoxShowDialog.Text = "下单后弹出确认对话框";
			this.checkBoxShowDialog.UseVisualStyleBackColor = true;
			this.numericLockTime.Location = new Point(109, 21);
			NumericUpDown arg_1539_0 = this.numericLockTime;
			int[] array = new int[4];
			array[0] = 60;
			arg_1539_0.Maximum = new decimal(array);
			NumericUpDown arg_1555_0 = this.numericLockTime;
			int[] array2 = new int[4];
			array2[0] = 1;
			arg_1555_0.Minimum = new decimal(array2);
			this.numericLockTime.Name = "numericLockTime";
			this.numericLockTime.Size = new Size(49, 21);
			this.numericLockTime.TabIndex = 1;
			NumericUpDown arg_15A1_0 = this.numericLockTime;
			int[] array3 = new int[4];
			array3[0] = 1;
			arg_15A1_0.Value = new decimal(array3);
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
			this.checkBoxLock.Size = new Size(72, 16);
			this.checkBoxLock.TabIndex = 0;
			this.checkBoxLock.Text = "自动锁定";
			this.checkBoxLock.UseVisualStyleBackColor = true;
			this.checkBoxLock.CheckedChanged += new EventHandler(this.checkBoxLock_CheckedChanged);
			this.buttonStyle.Anchor = AnchorStyles.Bottom;
			this.buttonStyle.ImeMode = ImeMode.NoControl;
			this.buttonStyle.Location = new Point(296, 353);
			this.buttonStyle.Name = "buttonStyle";
			this.buttonStyle.Size = new Size(67, 22);
			this.buttonStyle.TabIndex = 8;
			this.buttonStyle.Text = "设置风格";
			this.buttonStyle.Click += new EventHandler(this.buttonStyle_Click);
			this.buttonSubmit.Anchor = AnchorStyles.Bottom;
			this.buttonSubmit.ImeMode = ImeMode.NoControl;
			this.buttonSubmit.Location = new Point(402, 353);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new Size(55, 22);
			this.buttonSubmit.TabIndex = 6;
			this.buttonSubmit.Text = "确  定";
			this.buttonSubmit.Click += new EventHandler(this.buttonSubmit_Click);
			this.buttonCancel.Anchor = AnchorStyles.Bottom;
			this.buttonCancel.ImeMode = ImeMode.NoControl;
			this.buttonCancel.Location = new Point(496, 353);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(55, 22);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "取  消";
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(595, 384);
			base.Controls.Add(this.buttonStyle);
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
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.gbCloseMode.ResumeLayout(false);
			this.gbCloseMode.PerformLayout();
			((ISupportInitialize)this.numericLockTime).EndInit();
			base.ResumeLayout(false);
		}
		public UserSet(int _formStyle)
		{
			this.formStyle = _formStyle;
			this.InitializeComponent();
		}
		private void UserSet_Load(object sender, EventArgs e)
		{
			try
			{
				this.SetControlText();
				if (this.formStyle == 0)
				{
					if (!Tools.StrToBool((string)Global.HTConfig["DisplaySwitch"], false))
					{
						this.cbSetDoubleClick.Visible = false;
					}
					this.checkBoxAutoPrice.Visible = true;
					this.gbCloseMode.Visible = true;
				}
				else
				{
					if (this.formStyle == 1)
					{
						this.cbSetDoubleClick.Visible = false;
						this.checkBoxAutoPrice.Visible = false;
						this.gbCloseMode.Visible = false;
					}
					else
					{
						if (this.formStyle == 8)
						{
							this.cbSetDoubleClick.Visible = false;
							this.checkBoxAutoPrice.Visible = true;
							this.gbCloseMode.Visible = false;
						}
					}
				}
				this.checkBoxLock.Checked = IniData.GetInstance().LockEnable;
				this.numericLockTime.Visible = IniData.GetInstance().LockEnable;
				this.labelLockTime.Visible = IniData.GetInstance().LockEnable;
				this.numericLockTime.Value = IniData.GetInstance().LockTime;
				this.checkBoxShowDialog.Checked = IniData.GetInstance().ShowDialog;
				this.cbFailDialog.Checked = IniData.GetInstance().FailShowDialog;
				this.chbSuccessDialog.Checked = IniData.GetInstance().SuccessShowDialog;
				this.chbAutoRefresh.Checked = IniData.GetInstance().AutoRefresh;
				this.cbSetDoubleClick.Checked = IniData.GetInstance().SetDoubleClick;
				this.chbClearData.Checked = IniData.GetInstance().ClearData;
				this.checkBoxAutoPrice.Checked = IniData.GetInstance().AutoPrice;
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
				string language = IniData.GetInstance().Language;
				if (language == "0" || !File.Exists("Gnnt.en.resources") || string.IsNullOrEmpty(language))
				{
					this.rBLanguageC.Checked = true;
				}
				else
				{
					this.rBLanguageE.Checked = true;
				}
				this.listCommodity.Items.Clear();
				this.listMyCommodity.Items.Clear();
				foreach (string current in OperationManager.GetInstance().commodityList)
				{
					if (current != OperationManager.GetInstance().StrAll)
					{
						this.listCommodity.Items.Add(current);
					}
				}
				foreach (string current2 in OperationManager.GetInstance().myCommodityList)
				{
					if (current2 != OperationManager.GetInstance().StrAll)
					{
						this.listMyCommodity.Items.Add(current2);
					}
				}
				this.listTranc.Items.Clear();
				this.listMyTranc.Items.Clear();
				foreach (string current3 in OperationManager.GetInstance().transactionsList)
				{
					if (current3 != OperationManager.GetInstance().StrAll)
					{
						this.listTranc.Items.Add(current3);
					}
				}
				foreach (string current4 in OperationManager.GetInstance().myTransactionsList)
				{
					if (current4 != OperationManager.GetInstance().StrAll)
					{
						this.listMyTranc.Items.Add(current4);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
			ScaleForm.ScaleForms(this);
		}
		private void butAdd_Click(object sender, EventArgs e)
		{
			if (this.listCommodity.SelectedIndex == -1)
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_UserSet_SelectGoodsId");
				MessageBox.Show(@string);
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
				string @string = Global.M_ResourceManager.GetString("TradeStr_UserSet_SelectGoodsId");
				MessageBox.Show(@string);
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
				string @string = Global.M_ResourceManager.GetString("TradeStr_UserSet_SelectTradeId");
				MessageBox.Show(@string);
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
				string @string = Global.M_ResourceManager.GetString("TradeStr_UserSet_SelectTradeId");
				MessageBox.Show(@string);
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
			string text = string.Empty;
			if (this.commUpdateFlag)
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateGoodsListSuccess");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateGoodsListFail");
				if (this.SubmitCommUpdate())
				{
					text = @string + "\r\n";
				}
				else
				{
					text = string2 + "\r\n";
					flag = false;
				}
				this.commUpdateFlag = false;
			}
			if (this.trancUpdateFlag)
			{
				string string3 = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateTradeListSuccess");
				string string4 = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateTradeListFail");
				if (this.SubmitTrancUpdate())
				{
					text = text + string3 + "\r\n";
				}
				else
				{
					text = text + string4 + "\r\n";
					flag = false;
				}
				this.trancUpdateFlag = false;
			}
			FuturesTrade.Gnnt.Library.IniFile iniFile = new FuturesTrade.Gnnt.Library.IniFile(Global.ConfigPath + "MEBS_Trade.ini");
			if (this.checkBoxLock.Checked != IniData.GetInstance().LockEnable || this.numericLockTime.Value != IniData.GetInstance().LockTime)
			{
				IniData.GetInstance().LockEnable = this.checkBoxLock.Checked;
				IniData.GetInstance().LockTime = (int)this.numericLockTime.Value;
				iniFile.IniWriteValue("LockSet", "LockEnable", this.checkBoxLock.Checked.ToString());
				iniFile.IniWriteValue("LockSet", "LockTime", this.numericLockTime.Value.ToString());
				string string5 = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateLockSuccess");
				text = text + "\r\n" + string5 + "\r\n";
			}
			if (this.checkBoxShowDialog.Checked != IniData.GetInstance().ShowDialog)
			{
				IniData.GetInstance().ShowDialog = this.checkBoxShowDialog.Checked;
				iniFile.IniWriteValue("Set", "ShowDialog", IniData.GetInstance().ShowDialog.ToString());
				string string6 = Global.M_ResourceManager.GetString("TradeStr_UserSet_IsOpenSureBoxSuccess");
				text = text + "\r\n" + string6 + "\r\n";
			}
			if (this.cbFailDialog.Checked != IniData.GetInstance().FailShowDialog)
			{
				IniData.GetInstance().FailShowDialog = this.cbFailDialog.Checked;
				iniFile.IniWriteValue("Set", "FailShowDialog", IniData.GetInstance().FailShowDialog.ToString());
				string string7 = Global.M_ResourceManager.GetString("TradeStr_UserSet_OrderFailAfter");
				text = text + "\r\n" + string7 + "\r\n";
			}
			if (this.chbSuccessDialog.Checked != IniData.GetInstance().SuccessShowDialog)
			{
				IniData.GetInstance().SuccessShowDialog = this.chbSuccessDialog.Checked;
				iniFile.IniWriteValue("Set", "SuccessShowDialog", IniData.GetInstance().SuccessShowDialog.ToString());
				string string8 = Global.M_ResourceManager.GetString("TradeStr_UserSet_IsOpenTransactionReturnInfo");
				text = text + "\r\n" + string8 + "\r\n";
			}
			if (this.chbAutoRefresh.Checked != IniData.GetInstance().AutoRefresh)
			{
				IniData.GetInstance().AutoRefresh = this.chbAutoRefresh.Checked;
				iniFile.IniWriteValue("Set", "AutoRefresh", IniData.GetInstance().AutoRefresh.ToString());
				string string9 = Global.M_ResourceManager.GetString("TradeStr_UserSet_IsAutoRefreshData");
				text = text + "\r\n" + string9 + "\r\n";
			}
			if (this.formStyle == 0 && this.cbSetDoubleClick.Checked != IniData.GetInstance().SetDoubleClick)
			{
				IniData.GetInstance().SetDoubleClick = this.cbSetDoubleClick.Checked;
				iniFile.IniWriteValue("Set", "SetDoubleClick", IniData.GetInstance().SetDoubleClick.ToString());
				string string10 = Global.M_ResourceManager.GetString("TradeStr_UserSet_IsSwitchPrice");
				text = text + "\r\n" + string10 + "\r\n";
			}
			if (this.chbClearData.Checked != IniData.GetInstance().ClearData)
			{
				IniData.GetInstance().ClearData = this.chbClearData.Checked;
				iniFile.IniWriteValue("Set", "ClearData", IniData.GetInstance().ClearData.ToString());
				string string11 = Global.M_ResourceManager.GetString("TradeStr_UserSet_ClearPriceNum");
				text = text + "\r\n" + string11 + "\r\n";
			}
			if (this.formStyle != 1 && this.checkBoxAutoPrice.Checked != IniData.GetInstance().AutoPrice)
			{
				IniData.GetInstance().AutoPrice = this.checkBoxAutoPrice.Checked;
				iniFile.IniWriteValue("Set", "AutoPrice", IniData.GetInstance().AutoPrice.ToString());
				string string12 = Global.M_ResourceManager.GetString("TradeStr_UserSet_AutoPriceInfo");
				text = text + "\r\n" + string12 + "\r\n";
			}
			if (this.formStyle == 0)
			{
				int num;
				if (this.rbCloseMode1.Checked)
				{
					num = 1;
				}
				else
				{
					if (this.rbCloseMode2.Checked)
					{
						num = 2;
					}
					else
					{
						num = 3;
					}
				}
				if (num != IniData.GetInstance().CloseMode)
				{
					IniData.GetInstance().CloseMode = num;
					string string13 = Global.M_ResourceManager.GetString("TradeStr_UserSet_OpenType");
					text = text + "\r\n" + string13 + "\r\n";
				}
			}
			if (this.rBLanguageC.Checked != (IniData.GetInstance().Language == "0"))
			{
				if (this.rBLanguageC.Checked)
				{
					IniData.GetInstance().Language = "0";
					iniFile.IniWriteValue("Set", "Language", IniData.GetInstance().Language.ToString());
					string string14 = Global.M_ResourceManager.GetString("TradeStr_UserSet_SetLanguage");
					text = text + "\r\n" + string14 + "\r\n";
				}
				else
				{
					if (File.Exists("Gnnt.en.resources"))
					{
						IniData.GetInstance().Language = "1";
						iniFile.IniWriteValue("Set", "Language", IniData.GetInstance().Language.ToString());
						string string15 = Global.M_ResourceManager.GetString("TradeStr_UserSet_SetLanguage");
						text = text + "\r\n" + string15 + "\r\n";
					}
					else
					{
						string string16 = Global.M_ResourceManager.GetString("TradeStr_UserSet_SetLanguageFail");
						text = text + "\r\n" + string16 + "\r\n";
					}
				}
			}
			if (text != "")
			{
				string string17 = Global.M_ResourceManager.GetString("TradeStr_UserSet_UpdateResult");
				MessageBox.Show(text, string17, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
			if (this.listCommodity.Items.Count > this.listMyCommodity.Items.Count)
			{
				xmlDataSet.UpdateXmlRow(columns, columnValue2, "Flag", "True");
				for (int i = 0; i < this.listMyCommodity.Items.Count; i++)
				{
					if (!xmlDataSet.UpdateXmlRow(columns, columnValue, "commodityCode", this.listMyCommodity.Items[i].ToString()))
					{
						result = false;
					}
				}
			}
			else
			{
				xmlDataSet.UpdateXmlRow(columns, columnValue, "Flag", "False");
				for (int j = 0; j < this.listCommodity.Items.Count; j++)
				{
					if (!xmlDataSet.UpdateXmlRow(columns, columnValue2, "commodityCode", this.listCommodity.Items[j].ToString()))
					{
						result = false;
					}
				}
			}
			OperationManager.GetInstance().GetCommodityInfoList();
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
			XmlDataSet xmlDataSet = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml);
			if (this.listTranc.Items.Count > this.listMyTranc.Items.Count)
			{
				xmlDataSet.UpdateXmlRow(columns, columnValue2, "Flag", "True");
				for (int i = 0; i < this.listMyTranc.Items.Count; i++)
				{
					if (!xmlDataSet.UpdateXmlRow(columns, columnValue, "TransactionsCode", this.listMyTranc.Items[i].ToString()))
					{
						result = false;
					}
				}
			}
			else
			{
				xmlDataSet.UpdateXmlRow(columns, columnValue, "Flag", "False");
				for (int j = 0; j < this.listTranc.Items.Count; j++)
				{
					if (!xmlDataSet.UpdateXmlRow(columns, columnValue2, "TransactionsCode", this.listTranc.Items[j].ToString()))
					{
						result = false;
					}
				}
			}
			OperationManager.GetInstance().GetTransactionInfoList();
			return result;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void SetControlText()
		{
			this.Text = Global.M_ResourceManager.GetString("TradeStr_toolStripButtonSet");
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
			this.chbClearData.Text = Global.M_ResourceManager.GetString("TradeStr_chbClearData");
			this.buttonSubmit.Text = Global.M_ResourceManager.GetString("TradeStr_buttonSubmit");
			this.chbAutoRefresh.Text = Global.M_ResourceManager.GetString("TradeStr_chbAutoRefresh");
			this.buttonCancel.Text = Global.M_ResourceManager.GetString("TradeStr_buttonCancel");
			this.labelLanguage.Text = Global.M_ResourceManager.GetString("TradeStr_labelLanguage");
			this.rBLanguageC.Text = Global.M_ResourceManager.GetString("TradeStr_rBLanguageC");
			this.rBLanguageE.Text = Global.M_ResourceManager.GetString("TradeStr_rBLanguageE");
			this.cbSetDoubleClick.Text = Global.M_ResourceManager.GetString("TradeStr_cbSetDoubleClick");
			this.gbCloseMode.Text = Global.M_ResourceManager.GetString("TradeStr_TodayPosition");
			this.checkBoxAutoPrice.Text = Global.M_ResourceManager.GetString("TradeStr_AutoPrice") + "\r\n" + Global.M_ResourceManager.GetString("TradeStr_AutoPriceRule");
			this.buttonStyle.Text = Global.M_ResourceManager.GetString("TradeStr_SetStyle");
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
		private void checkBoxAutoPrice_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBoxAutoPrice.Checked && this.chbClearData.Checked && this.checkBoxAutoPrice.Visible)
			{
				this.chbClearData.Checked = false;
			}
		}
		private void chbClearData_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBoxAutoPrice.Checked && this.chbClearData.Checked && this.checkBoxAutoPrice.Visible)
			{
				this.checkBoxAutoPrice.Checked = false;
			}
		}
		private void buttonStyle_Click(object sender, EventArgs e)
		{
			Global.UpdateStyle();
		}
	}
}
