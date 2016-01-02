using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DIYForm;

namespace YrdceClient
{
    public partial class StateForm : Form
    {
        public StateForm()
        {
            InitializeComponent();
            //textbox模仿label
            this.ControlBox = false;
            this.lblHQAddress.BackColor = System.Drawing.SystemColors.Control;
            this.lblHQAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblHQAddress.ReadOnly = true;
            this.lblCounmerAddress.BackColor = System.Drawing.SystemColors.Control;
            this.lblCounmerAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblCounmerAddress.ReadOnly = true;
            this.lblTradeAddress.BackColor = System.Drawing.SystemColors.Control;
            this.lblTradeAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblTradeAddress.ReadOnly = true;
            //this.label1.BackColor = System.Drawing.SystemColors.Control;
            //this.label1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.label1.ReadOnly = true;
            //this.label2.BackColor = System.Drawing.SystemColors.Control;
            //this.label2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.label2.ReadOnly = true;
            //this.label5.BackColor = System.Drawing.SystemColors.Control;
            //this.label5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.label5.ReadOnly = true; 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
