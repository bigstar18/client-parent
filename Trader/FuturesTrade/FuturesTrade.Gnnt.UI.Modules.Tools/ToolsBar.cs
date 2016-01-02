using FuturesTrade.Gnnt.Library;
using FuturesTrade.Gnnt.UI.Forms.ToosForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace FuturesTrade.Gnnt.UI.Modules.Tools
{
	public class ToolsBar : UserControl
	{
		public delegate void LockFormCallBack(bool type);
		private IContainer components;
		public ToolStrip toolStrip;
		private ToolStripButton toolStripButtonCon;
		private ToolStripButton toolStripButtonOrder;
		private ToolStripButton toolStripButtonCO;
		private ToolStripButton toolStripButtonSet;
		private ToolStripButton toolStripButtonMsg;
		private ToolStripButton toolStripButtonLock;
		private ToolStripButton toolStripButtonHelp;
		private ToolStripButton toolStripButtonAbout;
		private ToolStripButton toolStripButtonExit;
		private ToolStripButton toolStripButtonBill;
		private ToolStripButton toolStripButtonFloat;
		private int formStyle;
		private ConditionOrder conditionOrder;
		private FormOrder formOrder;
		private CrazyOrder crazyOrder;
		private UserSet userSet;
		public int ToolsBarWidth = 150;
		public ToolsBar.LockFormCallBack lockFormCallBack;
		protected override void Dispose(bool disposing)
		{
			this.CloseForms();
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ToolsBar));
			this.toolStrip = new ToolStrip();
			this.toolStripButtonBill = new ToolStripButton();
			this.toolStripButtonCon = new ToolStripButton();
			this.toolStripButtonOrder = new ToolStripButton();
			this.toolStripButtonCO = new ToolStripButton();
			this.toolStripButtonSet = new ToolStripButton();
			this.toolStripButtonMsg = new ToolStripButton();
			this.toolStripButtonLock = new ToolStripButton();
			this.toolStripButtonHelp = new ToolStripButton();
			this.toolStripButtonAbout = new ToolStripButton();
			this.toolStripButtonExit = new ToolStripButton();
			this.toolStripButtonFloat = new ToolStripButton();
			this.toolStrip.SuspendLayout();
			base.SuspendLayout();
			this.toolStrip.BackColor = SystemColors.Control;
			this.toolStrip.Dock = DockStyle.Fill;
			this.toolStrip.GripMargin = new Padding(0);
			this.toolStrip.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripButtonBill,
				this.toolStripButtonCon,
				this.toolStripButtonOrder,
				this.toolStripButtonCO,
				this.toolStripButtonSet,
				this.toolStripButtonMsg,
				this.toolStripButtonLock,
				this.toolStripButtonHelp,
				this.toolStripButtonAbout,
				this.toolStripButtonExit,
				this.toolStripButtonFloat
			});
			this.toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
			this.toolStrip.Location = new Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new Padding(0);
			this.toolStrip.RenderMode = ToolStripRenderMode.System;
			this.toolStrip.Size = new Size(284, 23);
			this.toolStrip.TabIndex = 13;
			this.toolStrip.Text = "toolStrip1";
			this.toolStripButtonBill.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonBill.Image = (Image)componentResourceManager.GetObject("toolStripButtonBill.Image");
			this.toolStripButtonBill.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonBill.Name = "toolStripButtonBill";
			this.toolStripButtonBill.Size = new Size(23, 20);
			this.toolStripButtonBill.Text = "仓单交易";
			this.toolStripButtonCon.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCon.Image = (Image)componentResourceManager.GetObject("toolStripButtonCon.Image");
			this.toolStripButtonCon.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonCon.Name = "toolStripButtonCon";
			this.toolStripButtonCon.Size = new Size(23, 20);
			this.toolStripButtonCon.Text = "条件下单";
			this.toolStripButtonCon.Click += new EventHandler(this.toolStripButtonCon_Click);
			this.toolStripButtonOrder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOrder.Image = (Image)componentResourceManager.GetObject("toolStripButtonOrder.Image");
			this.toolStripButtonOrder.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonOrder.Name = "toolStripButtonOrder";
			this.toolStripButtonOrder.Size = new Size(23, 20);
			this.toolStripButtonOrder.Text = "快速下单";
			this.toolStripButtonOrder.Click += new EventHandler(this.toolStripButtonOrder_Click);
			this.toolStripButtonCO.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCO.Image = (Image)componentResourceManager.GetObject("toolStripButtonCO.Image");
			this.toolStripButtonCO.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonCO.Name = "toolStripButtonCO";
			this.toolStripButtonCO.Size = new Size(23, 20);
			this.toolStripButtonCO.Text = "疯狂下单";
			this.toolStripButtonCO.Click += new EventHandler(this.toolStripButtonCO_Click);
			this.toolStripButtonSet.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSet.Image = (Image)componentResourceManager.GetObject("toolStripButtonSet.Image");
			this.toolStripButtonSet.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonSet.Name = "toolStripButtonSet";
			this.toolStripButtonSet.Size = new Size(23, 20);
			this.toolStripButtonSet.Text = "toolStripButton1";
			this.toolStripButtonSet.ToolTipText = "设置";
			this.toolStripButtonSet.Click += new EventHandler(this.toolStripButtonSet_Click);
			this.toolStripButtonMsg.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonMsg.Image = (Image)componentResourceManager.GetObject("toolStripButtonMsg.Image");
			this.toolStripButtonMsg.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonMsg.Name = "toolStripButtonMsg";
			this.toolStripButtonMsg.Size = new Size(23, 20);
			this.toolStripButtonMsg.Text = "toolStripButton1";
			this.toolStripButtonMsg.ToolTipText = "广播消息";
			this.toolStripButtonMsg.Click += new EventHandler(this.toolStripButtonMsg_Click);
			this.toolStripButtonLock.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonLock.Image = (Image)componentResourceManager.GetObject("toolStripButtonLock.Image");
			this.toolStripButtonLock.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonLock.Name = "toolStripButtonLock";
			this.toolStripButtonLock.Size = new Size(23, 20);
			this.toolStripButtonLock.Text = "toolStripButton1";
			this.toolStripButtonLock.ToolTipText = "锁定";
			this.toolStripButtonLock.Click += new EventHandler(this.toolStripButtonLock_Click);
			this.toolStripButtonHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonHelp.Image = (Image)componentResourceManager.GetObject("toolStripButtonHelp.Image");
			this.toolStripButtonHelp.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonHelp.Name = "toolStripButtonHelp";
			this.toolStripButtonHelp.Size = new Size(23, 20);
			this.toolStripButtonHelp.Text = "toolStripButton2";
			this.toolStripButtonHelp.ToolTipText = "帮助";
			this.toolStripButtonAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAbout.Image = (Image)componentResourceManager.GetObject("toolStripButtonAbout.Image");
			this.toolStripButtonAbout.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonAbout.Name = "toolStripButtonAbout";
			this.toolStripButtonAbout.Size = new Size(23, 20);
			this.toolStripButtonAbout.Text = "关于";
			this.toolStripButtonExit.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonExit.Image = (Image)componentResourceManager.GetObject("toolStripButtonExit.Image");
			this.toolStripButtonExit.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonExit.Name = "toolStripButtonExit";
			this.toolStripButtonExit.Size = new Size(23, 20);
			this.toolStripButtonExit.Text = "退出";
			this.toolStripButtonFloat.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFloat.Image = (Image)componentResourceManager.GetObject("toolStripButtonFloat.Image");
			this.toolStripButtonFloat.ImageTransparentColor = Color.Magenta;
			this.toolStripButtonFloat.Name = "toolStripButtonFloat";
			this.toolStripButtonFloat.Size = new Size(23, 20);
			this.toolStripButtonFloat.Text = "浮动";
			this.toolStripButtonFloat.Click += new EventHandler(this.toolStripButtonFloat_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.AutoSize = true;
			base.Controls.Add(this.toolStrip);
			base.Margin = new Padding(0);
			base.Name = "ToolsBar";
			base.Size = new Size(284, 23);
			base.Load += new EventHandler(this.ToolsBar_Load);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ToolsBar(int _formStyle)
		{
			this.InitializeComponent();
			this.formStyle = _formStyle;
		}
		private void ToolsBar_Load(object sender, EventArgs e)
		{
			this.HideToolsBarButton();
		}
		private void HideToolsBarButton()
		{
			try
			{
				this.toolStrip.Items.Remove(this.toolStripButtonMsg);
				this.toolStrip.Items.Remove(this.toolStripButtonBill);
				this.toolStrip.Items.Remove(this.toolStripButtonHelp);
				this.toolStrip.Items.Remove(this.toolStripButtonAbout);
				this.toolStrip.Items.Remove(this.toolStripButtonExit);
				base.Width = this.ToolsBarWidth;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void toolStripButtonCon_Click(object sender, EventArgs e)
		{
			if (this.conditionOrder != null && this.conditionOrder.WindowState == FormWindowState.Minimized)
			{
				this.conditionOrder.WindowState = FormWindowState.Normal;
				return;
			}
			this.conditionOrder = ConditionOrder.Instance();
			this.conditionOrder.TopMost = true;
			this.conditionOrder.StartPosition = FormStartPosition.CenterScreen;
			this.conditionOrder.Show();
		}
		private void toolStripButtonOrder_Click(object sender, EventArgs e)
		{
			this.formOrder = FormOrder.Instance();
			this.formOrder.TopMost = true;
			this.formOrder.StartPosition = FormStartPosition.CenterScreen;
			this.formOrder.Show();
		}
		private void toolStripButtonCO_Click(object sender, EventArgs e)
		{
			this.crazyOrder = CrazyOrder.Instance();
			this.crazyOrder.TopMost = true;
			this.crazyOrder.StartPosition = FormStartPosition.CenterScreen;
			this.crazyOrder.Show();
		}
		private void toolStripButtonSet_Click(object sender, EventArgs e)
		{
			if (this.userSet == null)
			{
				this.userSet = new UserSet(this.formStyle);
			}
			this.userSet.TopMost = true;
			this.userSet.StartPosition = FormStartPosition.CenterScreen;
			this.userSet.ShowDialog();
		}
		private void toolStripButtonMsg_Click(object sender, EventArgs e)
		{
		}
		private void toolStripButtonLock_Click(object sender, EventArgs e)
		{
			Global.LockForm(sender, e);
			if (this.lockFormCallBack != null)
			{
				this.lockFormCallBack(false);
			}
			this.CloseForms();
		}
		private void toolStripButtonFloat_Click(object sender, EventArgs e)
		{
			Global.FloatForm(sender, e);
		}
		private void CloseForms()
		{
			if (this.formOrder != null)
			{
				this.formOrder.Close();
			}
			if (this.crazyOrder != null)
			{
				this.crazyOrder.Close();
			}
			if (this.conditionOrder != null)
			{
				this.conditionOrder.Close();
			}
		}
	}
}
