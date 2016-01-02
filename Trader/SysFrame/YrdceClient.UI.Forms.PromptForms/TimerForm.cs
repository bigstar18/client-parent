using DIYForm;
using YrdceClient.Yrdce.Common.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace YrdceClient.UI.Forms.PromptForms
{
	public class TimerForm : MyForm
	{
		private IContainer components;
		private Label lbl_message;
		private Timer timer_close;
		private Button btn_imclose;
		private Button btn_cancelclose;
		private Label lbl_time;
		private bool result;
		private int time = 30;
		public bool Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
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
			this.components = new Container();
			this.lbl_message = new Label();
			this.timer_close = new Timer(this.components);
			this.btn_imclose = new Button();
			this.btn_cancelclose = new Button();
			this.lbl_time = new Label();
			base.SuspendLayout();
			this.lbl_message.AutoSize = true;
			this.lbl_message.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.lbl_message.Location = new Point(25, 35);
			this.lbl_message.Name = "lbl_message";
			this.lbl_message.Size = new Size(101, 12);
			this.lbl_message.TabIndex = 0;
			this.lbl_message.Text = "确定退出系统吗？";
			this.timer_close.Interval = 1000;
			this.timer_close.Tick += new EventHandler(this.timer1_Tick);
			this.btn_imclose.BackColor = SystemColors.Control;
			this.btn_imclose.Location = new Point(20, 75);
			this.btn_imclose.Name = "btn_imclose";
            this.btn_imclose.FlatStyle = FlatStyle.Flat;
			this.btn_imclose.Size = new Size(70, 23);
			this.btn_imclose.TabIndex = 1;
			this.btn_imclose.Text = "确定(&Y)";
			this.btn_imclose.UseVisualStyleBackColor = true;
			this.btn_imclose.Click += new EventHandler(this.btn_imclose_Click);
			this.btn_cancelclose.BackColor = SystemColors.Control;
			this.btn_cancelclose.Location = new Point(110, 75);
			this.btn_cancelclose.Name = "btn_cancelclose";
			this.btn_cancelclose.Size = new Size(70, 23);
            this.btn_cancelclose.FlatStyle = FlatStyle.Flat;
			this.btn_cancelclose.TabIndex = 1;
			this.btn_cancelclose.Text = "取消(&N)";
			this.btn_cancelclose.UseVisualStyleBackColor = true;
			this.btn_cancelclose.Click += new EventHandler(this.btn_cancelclose_Click);
			this.lbl_time.AutoSize = true;
			this.lbl_time.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.lbl_time.Location = new Point(85, 55);
			this.lbl_time.Name = "lbl_time";
			this.lbl_time.Size = new Size(29, 12);
			this.lbl_time.TabIndex = 0;
			this.lbl_time.Text = "0:30";
            this.TextColor = Brushes.White;
            base.AutoScaleMode = AutoScaleMode.None;
			base.ClientSize = new Size(200, 120);
            
            base.Controls.Add(this.btn_cancelclose);
            base.Controls.Add(this.btn_imclose);
            base.Controls.Add(this.lbl_time);
            base.Controls.Add(this.lbl_message);
            //this.BackColor = Color.Black;
            base.DIYMaximizeBox = false;
			base.DIYMinimizeBox = false;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TimerForm";
            base.SetSize = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "退出提示";

			base.TopMost = true;
			base.Load += new EventHandler(this.TimerForm_Load);
			base.Controls.SetChildIndex(this.lbl_message, 0);
			base.Controls.SetChildIndex(this.lbl_time, 0);
			base.Controls.SetChildIndex(this.btn_imclose, 0);
			base.Controls.SetChildIndex(this.btn_cancelclose, 0);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public TimerForm()
		{
			this.InitializeComponent();
		}
		private void TimerForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.SetSkin();
				this.SetControlText();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Information, ex.ToString());
			}
			this.timer_close.Start();
		}
		private void SetControlText()
		{
			this.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_TimerForm_Form");
			this.btn_cancelclose.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_TimerForm_btn_cancelclose");
			this.btn_cancelclose.TextAlign = ContentAlignment.MiddleCenter;
			this.btn_imclose.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_TimerForm_btn_imclose");
			this.btn_imclose.TextAlign = ContentAlignment.MiddleCenter;
			this.lbl_message.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_TimerForm_lbl_message");
			ScaleForm.ScaleForms(this);
		}
		private void SetSkin()
		{
			base.Icon = Global.Modules.Plugins.SystemIcon;
            //this.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_Skin1");
            //this.BackgroundImageLayout = ImageLayout.Stretch;
            //this.btn_imclose.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_Skin1");
            //this.btn_cancelclose.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_Skin1");
            this.lbl_message.BackColor = Color.Transparent;
            this.lbl_time.BackColor = Color.Transparent;
        }
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.time--;
			this.lbl_time.Text = "0:" + this.time.ToString();
			if (this.time == 0)
			{
				this.result = true;
				base.Close();
			}
		}
		private void btn_cancelclose_Click(object sender, EventArgs e)
		{
			this.result = false;
			this.timer_close.Stop();
			base.Close();
		}
		private void btn_imclose_Click(object sender, EventArgs e)
		{
			this.result = true;
			base.Close();
		}
	}
}
