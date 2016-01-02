using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.Manager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace SysFrame.Gnnt.UI.Forms.UserControls
{
	public class StripButtonControl : UserControl
	{
		public delegate void LoadClickFormCallBack();
		private OperationManager operationManager = OperationManager.GetInstance();
		private StripButtonControl.LoadClickFormCallBack loadClickFormCallBack;
		public Image menuBackImage;
		public Image menuClickBackImage;
		private IContainer components;
		private ToolStrip toolStripButtons;
		private ToolStripButton toolStripButton1;
		public StripButtonControl()
		{
			this.InitializeComponent();
			this.operationManager.stripButtonOperation.StripButtonNodeLoad(this.toolStripButtons);
		}
		private void StripButtonControl_Load(object sender, EventArgs e)
		{
			this.SetToolStripButtons();
		}
		private void SetToolStripButtons()
		{
			try
			{
				this.menuBackImage = Image.FromFile("images\\menu.png");
				this.menuClickBackImage = Image.FromFile("images\\menuclick.png");
				this.operationManager.CreateToolStrip();
				this.toolStripButtons.Renderer = new DrawOverflowButtonRenderer();
				this.toolStripButtons.BackgroundImage = this.menuBackImage;
				this.toolStripButtons.OverflowButton.AutoSize = false;
				this.toolStripButtons.OverflowButton.Width = 30;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "初始化StripButtonControl异常：" + ex.Message);
			}
		}
		private void toolStripButtons_MouseEnter(object sender, EventArgs e)
		{
			base.Focus();
		}
		private void toolStripButtons_Paint(object sender, PaintEventArgs e)
		{
			if ((sender as ToolStrip).RenderMode == ToolStripRenderMode.Custom)
			{
				Rectangle clip = new Rectangle(0, 0, this.toolStripButtons.Width, this.toolStripButtons.Height - 1);
				e.Graphics.SetClip(clip);
			}
		}
		private void ChangeItemCheckState(ToolStripItem toolstripitem)
		{
			foreach (ToolStripItem toolStripItem in this.toolStripButtons.Items)
			{
				if (toolstripitem.Name == toolStripItem.Name)
				{
					((ToolStripLabel)toolstripitem).ToolTipText = "true";
					toolstripitem.BackgroundImage = this.menuClickBackImage;
				}
				else
				{
					((ToolStripLabel)toolStripItem).ToolTipText = "false";
					toolStripItem.BackgroundImage = this.menuBackImage;
				}
			}
		}
		private void RemoveLoginChild()
		{
			ToolStripItem value = new ToolStripLabel();
			bool flag = false;
			for (int i = 0; i < this.toolStripButtons.Items.Count; i++)
			{
				if (this.toolStripButtons.Items[i].Name == "login")
				{
					flag = true;
					value = this.toolStripButtons.Items[i];
					break;
				}
			}
			if (flag)
			{
				this.toolStripButtons.Items.Remove(value);
			}
		}
		private void toolStripButtons_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.ChangeItemCheckState(e.ClickedItem);
			this.operationManager.stripButtonOperation.isReloadPlugin = false;
			if (e.ClickedItem.Name == "login" && !this.operationManager.displayFormsOperation.displayCommit())
			{
				return;
			}
			this.operationManager.stripButtonOperation.s_ToolStripButtonClick(e);
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
			this.toolStripButtons = new ToolStrip();
			this.toolStripButton1 = new ToolStripButton();
			this.toolStripButtons.SuspendLayout();
			base.SuspendLayout();
			this.toolStripButtons.BackColor = Color.Transparent;
			this.toolStripButtons.BackgroundImageLayout = ImageLayout.Stretch;
			this.toolStripButtons.Dock = DockStyle.Fill;
			this.toolStripButtons.GripMargin = new Padding(0);
			this.toolStripButtons.GripStyle = ToolStripGripStyle.Hidden;
			this.toolStripButtons.ImageScalingSize = new Size(110, 40);
			this.toolStripButtons.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripButton1
			});
			this.toolStripButtons.Location = new Point(0, 0);
			this.toolStripButtons.Name = "toolStripButtons";
			this.toolStripButtons.Padding = new Padding(0);
			this.toolStripButtons.RenderMode = ToolStripRenderMode.System;
			this.toolStripButtons.ShowItemToolTips = false;
			this.toolStripButtons.Size = new Size(897, 40);
			this.toolStripButtons.TabIndex = 15;
			this.toolStripButtons.Text = "toolStripButtons";
			this.toolStripButtons.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStripButtons_ItemClicked);
			this.toolStripButtons.Paint += new PaintEventHandler(this.toolStripButtons_Paint);
			this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new Size(23, 37);
			this.toolStripButton1.Text = "toolStripButton1";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStripButtons);
			base.Margin = new Padding(0);
			base.Name = "StripButtonControl";
			base.Size = new Size(897, 40);
			base.Load += new EventHandler(this.StripButtonControl_Load);
			this.toolStripButtons.ResumeLayout(false);
			this.toolStripButtons.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
