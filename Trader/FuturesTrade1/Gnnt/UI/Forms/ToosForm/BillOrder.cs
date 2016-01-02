namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;

    public class BillOrder : Form
    {
        private MyButton buttonOrder;
        private MyCombobox comb_BillType;
        private MyCombobox comboMarKet;
        private MyCombobox comboTranc;
        private TextBox commodityCode;
        private IContainer components;
        private Label labComCode;
        private Label labelMarKet;
        private Label labPrice;
        private Label labQty;
        private Label labTCode;
        private TextBox tbTranc;
        private TextBox textBoxPrice;
        private TextBox textBoxQty;

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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tbTranc = new TextBox();
            this.comboTranc = new MyCombobox();
            this.comb_BillType = new MyCombobox();
            this.textBoxQty = new TextBox();
            this.textBoxPrice = new TextBox();
            this.commodityCode = new TextBox();
            this.buttonOrder = new MyButton();
            this.labQty = new Label();
            this.labPrice = new Label();
            this.labComCode = new Label();
            this.comboMarKet = new MyCombobox();
            this.labTCode = new Label();
            this.labelMarKet = new Label();
            base.SuspendLayout();
            this.tbTranc.BackColor = Color.White;
            this.tbTranc.Enabled = false;
            this.tbTranc.Location = new Point(0x44, 7);
            this.tbTranc.Multiline = true;
            this.tbTranc.Name = "tbTranc";
            this.tbTranc.ReadOnly = true;
            this.tbTranc.Size = new Size(0x2b, 20);
            this.tbTranc.TabIndex = 0x31;
            this.comboTranc.Location = new Point(0x6f, 7);
            this.comboTranc.MaxLength = 2;
            this.comboTranc.Name = "comboTranc";
            this.comboTranc.Size = new Size(0x25, 20);
            this.comboTranc.TabIndex = 0x30;
            this.comb_BillType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comb_BillType.FormattingEnabled = true;
            this.comb_BillType.Location = new Point(0x13c, 7);
            this.comb_BillType.Name = "comb_BillType";
            this.comb_BillType.Size = new Size(0x51, 20);
            this.comb_BillType.TabIndex = 0x27;
            this.textBoxQty.Location = new Point(0x285, 6);
            this.textBoxQty.Name = "textBoxQty";
            this.textBoxQty.Size = new Size(0x51, 0x15);
            this.textBoxQty.TabIndex = 0x2a;
            this.textBoxPrice.Location = new Point(480, 7);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new Size(0x58, 0x15);
            this.textBoxPrice.TabIndex = 0x29;
            this.commodityCode.CharacterCasing = CharacterCasing.Upper;
            this.commodityCode.Location = new Point(220, 7);
            this.commodityCode.Name = "commodityCode";
            this.commodityCode.Size = new Size(0x58, 0x15);
            this.commodityCode.TabIndex = 0x26;
           // this.buttonOrder.BackColor = Color.LightSteelBlue;
            this.buttonOrder.FlatStyle = FlatStyle.Popup;
            this.buttonOrder.ImeMode = ImeMode.NoControl;
            this.buttonOrder.Location = new Point(0x2dc, 7);
            this.buttonOrder.Name = "buttonOrder";
            this.buttonOrder.Size = new Size(0x2e, 0x15);
            this.buttonOrder.TabIndex = 0x2b;
            this.buttonOrder.Text = "提交";
            this.buttonOrder.UseVisualStyleBackColor = false;
            this.labQty.AutoSize = true;
            this.labQty.BackColor = Color.Transparent;
            this.labQty.ImeMode = ImeMode.NoControl;
            this.labQty.Location = new Point(0x241, 11);
            this.labQty.Name = "labQty";
            this.labQty.Size = new Size(0x41, 12);
            this.labQty.TabIndex = 0x2d;
            this.labQty.Text = "委托数量：";
            this.labQty.TextAlign = ContentAlignment.BottomLeft;
            this.labPrice.AutoSize = true;
            this.labPrice.BackColor = Color.Transparent;
            this.labPrice.ImeMode = ImeMode.NoControl;
            this.labPrice.Location = new Point(0x197, 11);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(0x41, 12);
            this.labPrice.TabIndex = 0x2c;
            this.labPrice.Text = "委托价格：";
            this.labPrice.TextAlign = ContentAlignment.BottomLeft;
            this.labComCode.AutoSize = true;
            this.labComCode.BackColor = Color.Transparent;
            this.labComCode.ImageAlign = ContentAlignment.MiddleLeft;
            this.labComCode.ImeMode = ImeMode.NoControl;
            this.labComCode.Location = new Point(0x9a, 11);
            this.labComCode.Name = "labComCode";
            this.labComCode.Size = new Size(0x41, 12);
            this.labComCode.TabIndex = 40;
            this.labComCode.Text = "商品代码：";
            this.labComCode.TextAlign = ContentAlignment.BottomLeft;
            this.comboMarKet.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboMarKet.Location = new Point(0x44, 7);
            this.comboMarKet.Name = "comboMarKet";
            this.comboMarKet.Size = new Size(0x51, 20);
            this.comboMarKet.TabIndex = 0x25;
            this.comboMarKet.Visible = false;
            this.labTCode.AutoSize = true;
            this.labTCode.BackColor = Color.Transparent;
            this.labTCode.ImageAlign = ContentAlignment.MiddleLeft;
            this.labTCode.ImeMode = ImeMode.NoControl;
            this.labTCode.Location = new Point(2, 11);
            this.labTCode.Name = "labTCode";
            this.labTCode.Size = new Size(0x41, 12);
            this.labTCode.TabIndex = 0x2f;
            this.labTCode.Text = "交易代码：";
            this.labTCode.TextAlign = ContentAlignment.BottomLeft;
            this.labelMarKet.AutoSize = true;
            this.labelMarKet.ImeMode = ImeMode.NoControl;
            this.labelMarKet.Location = new Point(7, 11);
            this.labelMarKet.Name = "labelMarKet";
            this.labelMarKet.Size = new Size(0x41, 12);
            this.labelMarKet.TabIndex = 0x2e;
            this.labelMarKet.Text = "市场标志：";
            this.labelMarKet.Visible = false;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x310, 0x20);
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
