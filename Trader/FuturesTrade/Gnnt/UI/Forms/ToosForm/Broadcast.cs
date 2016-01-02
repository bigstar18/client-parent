namespace FuturesTrade.Gnnt.UI.Forms.ToosForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TabTest;
    using ToolsLibrary.util;

    public class Broadcast : Form
    {
        private RichTextBox BroadcastText;
        private MyButton butClose;
        private MyButton butModel;
        private IContainer components;
        private Label labInfo;
        private Panel panelCover;
        private SplitContainer splitContainer1;

        public Broadcast()
        {
            this.InitializeComponent();
        }

        private void Broadcast_Load(object sender, EventArgs e)
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
            this.splitContainer1 = new SplitContainer();
            this.butClose = new MyButton();
            this.panelCover = new Panel();
            this.labInfo = new Label();
            this.butModel = new MyButton();
            this.BroadcastText = new RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelCover.SuspendLayout();
            base.SuspendLayout();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.BackColor = SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.butClose);
            this.splitContainer1.Panel1.Controls.Add(this.panelCover);
            this.splitContainer1.Panel1.Controls.Add(this.butModel);
            this.splitContainer1.Panel1.MouseDown += new MouseEventHandler(this.panelCover_MouseDown);
            this.splitContainer1.Panel2.Controls.Add(this.BroadcastText);
            this.splitContainer1.Size = new Size(540, 170);
            this.splitContainer1.SplitterDistance = 0x1b;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 6;
            this.butClose.Location = new Point(0x201, 4);
            this.butClose.Name = "butClose";
            this.butClose.Size = new Size(0x19, 0x12);
            this.butClose.TabIndex = 3;
            this.butClose.UseVisualStyleBackColor = true;
            this.panelCover.BackColor = Color.Black;
            this.panelCover.BorderStyle = BorderStyle.Fixed3D;
            this.panelCover.Controls.Add(this.labInfo);
            this.panelCover.Location = new Point(2, 2);
            this.panelCover.Name = "panelCover";
            this.panelCover.Size = new Size(0x1df, 0x17);
            this.panelCover.TabIndex = 3;
            this.labInfo.AutoSize = true;
            this.labInfo.BackColor = Color.Black;
            this.labInfo.ForeColor = Color.White;
            this.labInfo.Location = new Point(3, 7);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new Size(0x35, 12);
            this.labInfo.TabIndex = 2;
            this.labInfo.Text = "系统消息";
            this.labInfo.TextAlign = ContentAlignment.MiddleLeft;
            this.butModel.Location = new Point(0x1e7, 4);
            this.butModel.Name = "butModel";
            this.butModel.Size = new Size(0x19, 0x12);
            this.butModel.TabIndex = 4;
            this.butModel.UseVisualStyleBackColor = true;
            this.BroadcastText.BackColor = Color.Gainsboro;
            this.BroadcastText.Dock = DockStyle.Fill;
            this.BroadcastText.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.BroadcastText.Location = new Point(0, 0);
            this.BroadcastText.Name = "BroadcastText";
            this.BroadcastText.Size = new Size(540, 0x8e);
            this.BroadcastText.TabIndex = 1;
            this.BroadcastText.Text = "";
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(540, 170);
            base.Controls.Add(this.splitContainer1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "Broadcast";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Broadcast";
            base.TopMost = true;
            base.Load += new EventHandler(this.Broadcast_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelCover.ResumeLayout(false);
            this.panelCover.PerformLayout();
            base.ResumeLayout(false);
        }

        private void panelCover_MouseDown(object sender, MouseEventArgs e)
        {
        }
    }
}
