using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class HQMDIForm : Form
	{
		private IContainer components;
		private MenuStrip menuMain;
		private ToolStripMenuItem menuItemFile;
		private ToolStripMenuItem menuItemNewForm;
		private ToolStripMenuItem menuItemExit;
		private ToolStripMenuItem menuItemWindow;
		private ToolStripMenuItem menuItemCascade;
		private ToolStripMenuItem menuItemTileH;
		private ToolStripMenuItem menuItemTileV;
		private static int FormCount = 1;
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
			this.menuMain = new MenuStrip();
			this.menuItemFile = new ToolStripMenuItem();
			this.menuItemNewForm = new ToolStripMenuItem();
			this.menuItemExit = new ToolStripMenuItem();
			this.menuItemWindow = new ToolStripMenuItem();
			this.menuItemCascade = new ToolStripMenuItem();
			this.menuItemTileH = new ToolStripMenuItem();
			this.menuItemTileV = new ToolStripMenuItem();
			this.menuMain.SuspendLayout();
			base.SuspendLayout();
			this.menuMain.Items.AddRange(new ToolStripItem[]
			{
				this.menuItemFile,
				this.menuItemWindow
			});
			this.menuMain.Location = new Point(0, 0);
			this.menuMain.MdiWindowListItem = this.menuItemWindow;
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new Size(780, 24);
			this.menuMain.TabIndex = 1;
			this.menuMain.Text = "主菜单";
			this.menuItemFile.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.menuItemNewForm,
				this.menuItemExit
			});
			this.menuItemFile.Name = "menuItemFile";
			this.menuItemFile.Size = new Size(41, 20);
			this.menuItemFile.Text = "文件";
			this.menuItemNewForm.Name = "menuItemNewForm";
			this.menuItemNewForm.Size = new Size(94, 22);
			this.menuItemNewForm.Text = "新建";
			this.menuItemNewForm.Click += new EventHandler(this.menuItemNewForm_Click);
			this.menuItemExit.Name = "menuItemExit";
			this.menuItemExit.Size = new Size(94, 22);
			this.menuItemExit.Text = "退出";
			this.menuItemExit.Click += new EventHandler(this.menuItemExit_Click);
			this.menuItemWindow.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.menuItemCascade,
				this.menuItemTileH,
				this.menuItemTileV
			});
			this.menuItemWindow.Name = "menuItemWindow";
			this.menuItemWindow.Size = new Size(59, 20);
			this.menuItemWindow.Text = "窗口(&W)";
			this.menuItemCascade.Name = "menuItemCascade";
			this.menuItemCascade.Size = new Size(136, 22);
			this.menuItemCascade.Text = "窗体层叠(&C)";
			this.menuItemCascade.Click += new EventHandler(this.menuItemCascade_Click);
			this.menuItemTileH.Name = "menuItemTileH";
			this.menuItemTileH.Size = new Size(136, 22);
			this.menuItemTileH.Text = "水平平铺(&H)";
			this.menuItemTileH.Click += new EventHandler(this.menuItemTileH_Click);
			this.menuItemTileV.Name = "menuItemTileV";
			this.menuItemTileV.Size = new Size(136, 22);
			this.menuItemTileV.Text = "垂直平铺(&V)";
			this.menuItemTileV.Click += new EventHandler(this.menuItemTileV_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(780, 445);
			base.Controls.Add(this.menuMain);
			base.IsMdiContainer = true;
			base.MainMenuStrip = this.menuMain;
			base.Name = "HQMDIForm";
			this.Text = "Form1";
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public HQMDIForm()
		{
			this.InitializeComponent();
		}
		private void Exit_Click(object sender, EventArgs e)
		{
			base.Dispose();
			Application.Exit();
		}
		private void menuItemNewForm_Click(object sender, EventArgs e)
		{
			MainWindow mainWindow = new MainWindow(null);
			mainWindow.MdiParent = this;
			HQMDIForm.FormCount++;
			mainWindow.Show();
		}
		private void menuItemExit_Click(object sender, EventArgs e)
		{
			base.Dispose();
			Application.Exit();
		}
		private void menuItemCascade_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.Cascade);
		}
		private void menuItemTileH_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.TileHorizontal);
		}
		private void menuItemTileV_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.TileVertical);
		}
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
			{
				HQForm hQForm = (HQForm)base.ActiveMdiChild;
				KeyEventArgs e = new KeyEventArgs(keyData);
				hQForm.HQMainForm_KeyDown(this, e);
				return true;
			}
			base.ProcessDialogKey(keyData);
			return false;
		}
	}
}
