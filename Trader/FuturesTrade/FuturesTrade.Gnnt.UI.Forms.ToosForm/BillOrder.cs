using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
	public class BillOrder : Form
	{
		private IContainer components;
		private TextBox tbTranc;
		private ComboBox comboTranc;
		private ComboBox comb_BillType;
		private TextBox textBoxQty;
		private TextBox textBoxPrice;
		private TextBox commodityCode;
		private Button buttonOrder;
		private Label labQty;
		private Label labPrice;
		private Label labComCode;
		private ComboBox comboMarKet;
		private Label labTCode;
		private Label labelMarKet;
		public BillOrder()
		{
			this.InitializeComponent();
		}
		private void BillOrder_Load(object sender, EventArgs e)
		{
			ScaleForm.ScaleForms(this);
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
			this.tbTranc = new TextBox();
			this.comboTranc = new ComboBox();
			this.comb_BillType = new ComboBox();
			this.textBoxQty = new TextBox();
			this.textBoxPrice = new TextBox();
			this.commodityCode = new TextBox();
			this.buttonOrder = new Button();
			this.labQty = new Label();
			this.labPrice = new Label();
			this.labComCode = new Label();
			this.comboMarKet = new ComboBox();
			this.labTCode = new Label();
			this.labelMarKet = new Label();
			base.SuspendLayout();
			this.tbTranc.BackColor = Color.White;
			this.tbTranc.Enabled = false;
			this.tbTranc.Location = new Point(68, 7);
			this.tbTranc.Multiline = true;
			this.tbTranc.Name = "tbTranc";
			this.tbTranc.ReadOnly = true;
			this.tbTranc.Size = new Size(43, 20);
			this.tbTranc.TabIndex = 49;
			this.comboTranc.Location = new Point(111, 7);
			this.comboTranc.MaxLength = 2;
			this.comboTranc.Name = "comboTranc";
			this.comboTranc.Size = new Size(37, 20);
			this.comboTranc.TabIndex = 48;
			this.comb_BillType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comb_BillType.FormattingEnabled = true;
			this.comb_BillType.Location = new Point(316, 7);
			this.comb_BillType.Name = "comb_BillType";
			this.comb_BillType.Size = new Size(81, 20);
			this.comb_BillType.TabIndex = 39;
			this.textBoxQty.Location = new Point(645, 6);
			this.textBoxQty.Name = "textBoxQty";
			this.textBoxQty.Size = new Size(81, 21);
			this.textBoxQty.TabIndex = 42;
			this.textBoxPrice.Location = new Point(480, 7);
			this.textBoxPrice.Name = "textBoxPrice";
			this.textBoxPrice.Size = new Size(88, 21);
			this.textBoxPrice.TabIndex = 41;
			this.commodityCode.CharacterCasing = CharacterCasing.Upper;
			this.commodityCode.Location = new Point(220, 7);
			this.commodityCode.Name = "commodityCode";
			this.commodityCode.Size = new Size(88, 21);
			this.commodityCode.TabIndex = 38;
			this.buttonOrder.BackColor = Color.LightSteelBlue;
			this.buttonOrder.FlatStyle = FlatStyle.Popup;
			this.buttonOrder.ImeMode = ImeMode.NoControl;
			this.buttonOrder.Location = new Point(732, 7);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new Size(46, 21);
			this.buttonOrder.TabIndex = 43;
			this.buttonOrder.Text = "提交";
			this.buttonOrder.UseVisualStyleBackColor = false;
			this.labQty.AutoSize = true;
			this.labQty.BackColor = Color.Transparent;
			this.labQty.ImeMode = ImeMode.NoControl;
			this.labQty.Location = new Point(577, 11);
			this.labQty.Name = "labQty";
			this.labQty.Size = new Size(65, 12);
			this.labQty.TabIndex = 45;
			this.labQty.Text = "委托数量：";
			this.labQty.TextAlign = ContentAlignment.BottomLeft;
			this.labPrice.AutoSize = true;
			this.labPrice.BackColor = Color.Transparent;
			this.labPrice.ImeMode = ImeMode.NoControl;
			this.labPrice.Location = new Point(407, 11);
			this.labPrice.Name = "labPrice";
			this.labPrice.Size = new Size(65, 12);
			this.labPrice.TabIndex = 44;
			this.labPrice.Text = "委托价格：";
			this.labPrice.TextAlign = ContentAlignment.BottomLeft;
			this.labComCode.AutoSize = true;
			this.labComCode.BackColor = Color.Transparent;
			this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labComCode.ImeMode = ImeMode.NoControl;
			this.labComCode.Location = new Point(154, 11);
			this.labComCode.Name = "labComCode";
			this.labComCode.Size = new Size(65, 12);
			this.labComCode.TabIndex = 40;
			this.labComCode.Text = "商品代码：";
			this.labComCode.TextAlign = ContentAlignment.BottomLeft;
			this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboMarKet.Location = new Point(68, 7);
			this.comboMarKet.Name = "comboMarKet";
			this.comboMarKet.Size = new Size(81, 20);
			this.comboMarKet.TabIndex = 37;
			this.comboMarKet.Visible = false;
			this.labTCode.AutoSize = true;
			this.labTCode.BackColor = Color.Transparent;
			this.labTCode.ImageAlign = ContentAlignment.MiddleLeft;
			this.labTCode.ImeMode = ImeMode.NoControl;
			this.labTCode.Location = new Point(2, 11);
			this.labTCode.Name = "labTCode";
			this.labTCode.Size = new Size(65, 12);
			this.labTCode.TabIndex = 47;
			this.labTCode.Text = "交易代码：";
			this.labTCode.TextAlign = ContentAlignment.BottomLeft;
			this.labelMarKet.AutoSize = true;
			this.labelMarKet.ImeMode = ImeMode.NoControl;
			this.labelMarKet.Location = new Point(7, 11);
			this.labelMarKet.Name = "labelMarKet";
			this.labelMarKet.Size = new Size(65, 12);
			this.labelMarKet.TabIndex = 46;
			this.labelMarKet.Text = "市场标志：";
			this.labelMarKet.Visible = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(784, 32);
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
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "BillOrder";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "BillOrder";
			base.Load += new EventHandler(this.BillOrder_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
