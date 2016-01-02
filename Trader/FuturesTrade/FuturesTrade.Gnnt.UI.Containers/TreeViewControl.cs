using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.UI.ContainerManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.UI.Containers
{
	public class TreeViewControl : UserControl
	{
		public delegate void TreeviewNodeClickCallBack(ControlLoad controlLoad);
		public TreeViewControl.TreeviewNodeClickCallBack TreeviewNodeClick;
		private IContainer components;
		private TreeView treeViewPages;
		private ImageList imageListF;
		public TreeViewControl()
		{
			this.InitializeComponent();
		}
		private void treeViewPages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			if (this.TreeviewNodeClick == null && e.Node.Name != "PageF8")
			{
				return;
			}
			if (e.Node.Name == "PageF1")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllOrder_HoldingCollect;
				this.TreeviewNodeClick(ControlLoad.HC_AO_Control);
				return;
			}
			if (e.Node.Name == "PageF2")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllOrder;
				this.TreeviewNodeClick(ControlLoad.AllOrder_Control);
				return;
			}
			if (e.Node.Name == "PageF3")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllTrade;
				this.TreeviewNodeClick(ControlLoad.Trade_Control);
				return;
			}
			if (e.Node.Name == "PageF4")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.HoldingCollect;
				this.TreeviewNodeClick(ControlLoad.HC_Control);
				return;
			}
			if (e.Node.Name == "PageF5")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.ConditionOrder;
				this.TreeviewNodeClick(ControlLoad.CO_Control);
				return;
			}
			if (e.Node.Name == "PageF6")
			{
				this.TreeviewNodeClick(ControlLoad.F_C_HD_Control);
				return;
			}
			if (e.Node.Name == "PageF7")
			{
				OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.PreDelegates;
				this.TreeviewNodeClick(ControlLoad.PRE_Control);
			}
		}
		public void MainFormKeyUp(Keys keyData)
		{
			TreeNode treeNode = new TreeNode();
			if (keyData == Keys.F1)
			{
				treeNode.Name = "PageF1";
			}
			else
			{
				if (keyData == Keys.F2)
				{
					treeNode.Name = "PageF2";
				}
				else
				{
					if (keyData == Keys.F3)
					{
						treeNode.Name = "PageF3";
					}
					else
					{
						if (keyData == Keys.F4)
						{
							treeNode.Name = "PageF4";
						}
						else
						{
							if (keyData == Keys.F5)
							{
								treeNode.Name = "PageF5";
							}
							else
							{
								if (keyData == Keys.F6)
								{
									treeNode.Name = "PageF6";
								}
								else
								{
									if (keyData == Keys.F7)
									{
										treeNode.Name = "PageF7";
									}
								}
							}
						}
					}
				}
			}
			TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(treeNode, MouseButtons.Left, 1, 0, 0);
			this.treeViewPages_NodeMouseClick(null, e);
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
			TreeNode treeNode = new TreeNode("交易", 0, 0);
			TreeNode treeNode2 = new TreeNode("当日委托", 1, 1);
			TreeNode treeNode3 = new TreeNode("当日成交", 2, 2);
			TreeNode treeNode4 = new TreeNode("持仓", 3, 3);
			TreeNode treeNode5 = new TreeNode("条件单", 4, 4);
			TreeNode treeNode6 = new TreeNode("查询", 5, 5);
			TreeNode treeNode7 = new TreeNode("预埋", 6, 6);
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TreeViewControl));
			this.treeViewPages = new TreeView();
			this.imageListF = new ImageList(this.components);
			base.SuspendLayout();
			this.treeViewPages.BorderStyle = BorderStyle.FixedSingle;
			this.treeViewPages.Dock = DockStyle.Fill;
			this.treeViewPages.Font = new Font("宋体", 10f);
			this.treeViewPages.ImageIndex = 0;
			this.treeViewPages.ImageList = this.imageListF;
			this.treeViewPages.ItemHeight = 20;
			this.treeViewPages.Location = new Point(0, 0);
			this.treeViewPages.Margin = new Padding(1);
			this.treeViewPages.Name = "treeViewPages";
			treeNode.ImageIndex = 0;
			treeNode.Name = "PageF1";
			treeNode.SelectedImageIndex = 0;
			treeNode.Text = "交易";
			treeNode2.ImageIndex = 1;
			treeNode2.Name = "PageF2";
			treeNode2.SelectedImageIndex = 1;
			treeNode2.Text = "当日委托";
			treeNode3.ImageIndex = 2;
			treeNode3.Name = "PageF3";
			treeNode3.SelectedImageIndex = 2;
			treeNode3.Text = "当日成交";
			treeNode4.ImageIndex = 3;
			treeNode4.Name = "PageF4";
			treeNode4.SelectedImageIndex = 3;
			treeNode4.Text = "持仓";
			treeNode5.ImageIndex = 4;
			treeNode5.Name = "PageF5";
			treeNode5.SelectedImageIndex = 4;
			treeNode5.Text = "条件单";
			treeNode6.ImageIndex = 5;
			treeNode6.Name = "PageF6";
			treeNode6.SelectedImageIndex = 5;
			treeNode6.Text = "查询";
			treeNode7.ImageIndex = 6;
			treeNode7.Name = "PageF7";
			treeNode7.SelectedImageIndex = 6;
			treeNode7.Text = "预埋";
			this.treeViewPages.Nodes.AddRange(new TreeNode[]
			{
				treeNode,
				treeNode2,
				treeNode3,
				treeNode4,
				treeNode5,
				treeNode6,
				treeNode7
			});
			this.treeViewPages.SelectedImageIndex = 0;
			this.treeViewPages.Size = new Size(108, 191);
			this.treeViewPages.TabIndex = 1;
			this.treeViewPages.TabStop = false;
			this.treeViewPages.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeViewPages_NodeMouseClick);
			this.imageListF.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListF.ImageStream");
			this.imageListF.TransparentColor = Color.Transparent;
			this.imageListF.Images.SetKeyName(0, "f1.png");
			this.imageListF.Images.SetKeyName(1, "f2.png");
			this.imageListF.Images.SetKeyName(2, "f3.png");
			this.imageListF.Images.SetKeyName(3, "f4.png");
			this.imageListF.Images.SetKeyName(4, "f5.png");
			this.imageListF.Images.SetKeyName(5, "f6.png");
			this.imageListF.Images.SetKeyName(6, "f7.png");
			this.imageListF.Images.SetKeyName(7, "f8.png");
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeViewPages);
			base.Name = "TreeViewControl";
			base.Size = new Size(108, 191);
			base.ResumeLayout(false);
		}
	}
}
