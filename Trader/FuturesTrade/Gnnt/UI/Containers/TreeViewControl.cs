namespace FuturesTrade.Gnnt.UI.Containers
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.UI.ContainerManager;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TreeViewControl : UserControl
    {
        private IContainer components;
        private ImageList imageListF;
        public TreeviewNodeClickCallBack TreeviewNodeClick;
        private TreeView treeViewPages;

        public TreeViewControl()
        {
            this.InitializeComponent();
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
            this.components = new Container();
            TreeNode node = new TreeNode("交易", 0, 0);
            TreeNode node2 = new TreeNode("当日委托", 1, 1);
            TreeNode node3 = new TreeNode("当日成交", 2, 2);
            TreeNode node4 = new TreeNode("持仓", 3, 3);
            TreeNode node5 = new TreeNode("条件单", 4, 4);
            TreeNode node6 = new TreeNode("查询", 5, 5);
            TreeNode node7 = new TreeNode("预埋", 6, 6);
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TreeViewControl));
            this.treeViewPages = new TreeView();
            this.imageListF = new ImageList(this.components);
            base.SuspendLayout();
            this.treeViewPages.BorderStyle = BorderStyle.None;
            this.treeViewPages.Dock = DockStyle.Fill;
            this.treeViewPages.Font = new Font("宋体", 10f);
            this.treeViewPages.ImageIndex = 0;
            this.treeViewPages.ImageList = this.imageListF;
            this.treeViewPages.ItemHeight = 20;
            this.treeViewPages.Location = new Point(0, 0);
            this.treeViewPages.Margin = new Padding(1);
            this.treeViewPages.Name = "treeViewPages";
            node.ImageIndex = 0;
            node.Name = "PageF1";
            node.SelectedImageIndex = 0;
            node.Text = "交易";
            node2.ImageIndex = 1;
            node2.Name = "PageF2";
            node2.SelectedImageIndex = 1;
            node2.Text = "当日委托";
            node3.ImageIndex = 2;
            node3.Name = "PageF3";
            node3.SelectedImageIndex = 2;
            node3.Text = "当日成交";
            node4.ImageIndex = 3;
            node4.Name = "PageF4";
            node4.SelectedImageIndex = 3;
            node4.Text = "持仓";
            node5.ImageIndex = 4;
            node5.Name = "PageF5";
            node5.SelectedImageIndex = 4;
            node5.Text = "条件单";
            node6.ImageIndex = 5;
            node6.Name = "PageF6";
            node6.SelectedImageIndex = 5;
            node6.Text = "查询";
            node7.ImageIndex = 6;
            node7.Name = "PageF7";
            node7.SelectedImageIndex = 6;
            node7.Text = "预埋";
            this.treeViewPages.Nodes.AddRange(new TreeNode[] { node, node2, node3, node4, node5, node6, node7 });
            this.treeViewPages.SelectedImageIndex = 0;
            this.treeViewPages.Size = new Size(0x6c, 0xbf);
            this.treeViewPages.TabIndex = 1;
            this.treeViewPages.TabStop = false;
            this.treeViewPages.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeViewPages_NodeMouseClick);
            this.imageListF.ImageStream = (ImageListStreamer)manager.GetObject("imageListF.ImageStream");
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
            base.Size = new Size(0x6c, 0xbf);
            base.ResumeLayout(false);
        }

        public void MainFormKeyUp(Keys keyData)
        {
            TreeNode node = new TreeNode();
            if (keyData == Keys.F1)
            {
                node.Name = "PageF1";
            }
            else if (keyData == Keys.F2)
            {
                node.Name = "PageF2";
            }
            else if (keyData == Keys.F3)
            {
                node.Name = "PageF3";
            }
            else if (keyData == Keys.F4)
            {
                node.Name = "PageF4";
            }
            else if (keyData == Keys.F5)
            {
                node.Name = "PageF5";
            }
            else if (keyData == Keys.F6)
            {
                node.Name = "PageF6";
            }
            else if (keyData == Keys.F7)
            {
                node.Name = "PageF7";
            }
            TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0);
            this.treeViewPages_NodeMouseClick(null, e);
        }

        private void treeViewPages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e != null) && ((this.TreeviewNodeClick != null) || (e.Node.Name == "PageF8")))
            {
                if (e.Node.Name == "PageF1")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllOrder_HoldingCollect;
                    this.TreeviewNodeClick(ControlLoad.HC_AO_Control);
                }
                else if (e.Node.Name == "PageF2")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllOrder;
                    this.TreeviewNodeClick(ControlLoad.AllOrder_Control);
                }
                else if (e.Node.Name == "PageF3")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.AllTrade;
                    this.TreeviewNodeClick(ControlLoad.Trade_Control);
                }
                else if (e.Node.Name == "PageF4")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.HoldingCollect;
                    this.TreeviewNodeClick(ControlLoad.HC_Control);
                }
                else if (e.Node.Name == "PageF5")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.ConditionOrder;
                    this.TreeviewNodeClick(ControlLoad.CO_Control);
                }
                else if (e.Node.Name == "PageF6")
                {
                    this.TreeviewNodeClick(ControlLoad.F_C_HD_Control);
                }
                else if (e.Node.Name == "PageF7")
                {
                    OperationManager.GetInstance().CurrentSelectIndex = RefreshQueryInfo.PreDelegates;
                    this.TreeviewNodeClick(ControlLoad.PRE_Control);
                }
            }
        }

        public delegate void TreeviewNodeClickCallBack(ControlLoad controlLoad);
    }
}
