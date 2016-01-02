namespace FuturesTrade.Gnnt.UI.Modules.Query
{
    using FuturesTrade.Gnnt.BLL.Manager;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TabTest;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class CommodityInfo : UserControl
    {
        private MyButton buttonSelF5;
        internal MyCombobox comboCommodityF5;
        private CommodityItemInfo commodityItemInfo = new CommodityItemInfo();
        private IContainer components;
        internal GroupBox groupBoxCommodity;
        private Label labelCommodityF5;
        internal ListView lstVCommodity;
        private OperationManager operationManager = OperationManager.GetInstance();

        public CommodityInfo()
        {
            this.InitializeComponent();
            this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
            this.CreateHandle();
        }

        private void buttonSelF5_Click(object sender, EventArgs e)
        {
            string text = this.comboCommodityF5.Text;
            this.LstVCommodityFill(text);
        }

        private void comboCommodityF5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.comboCommodityF5.Text;
            this.LstVCommodityFill(text);
        }

        private void CommodityInfo_Load(object sender, EventArgs e)
        {
            string text = this.comboCommodityF5.Text;
            this.LstVCommodityFill(text);
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
            this.groupBoxCommodity = new GroupBox();
            this.lstVCommodity = new ListView();
            this.buttonSelF5 = new MyButton();
            this.labelCommodityF5 = new Label();
            this.comboCommodityF5 = new MyCombobox();
            this.groupBoxCommodity.SuspendLayout();
            base.SuspendLayout();
            this.groupBoxCommodity.Controls.Add(this.lstVCommodity);
            this.groupBoxCommodity.Controls.Add(this.buttonSelF5);
            this.groupBoxCommodity.Controls.Add(this.labelCommodityF5);
            this.groupBoxCommodity.Controls.Add(this.comboCommodityF5);
            this.groupBoxCommodity.Dock = DockStyle.Fill;
            this.groupBoxCommodity.Location = new Point(0, 0);
            this.groupBoxCommodity.Name = "groupBoxCommodity";
            this.groupBoxCommodity.Size = new Size(700, 200);
            this.groupBoxCommodity.TabIndex = 0x17;
            this.groupBoxCommodity.TabStop = false;
            this.groupBoxCommodity.Text = "商品查询";
            this.groupBoxCommodity.BackColor = Color.FromArgb(235,235,235);
            //this.groupBoxCommodity.ForeColor = Color.FromArgb(235, 235, 235);
            this.lstVCommodity.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lstVCommodity.GridLines = true;
            this.lstVCommodity.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lstVCommodity.Location = new Point(5, 0x27);
            this.lstVCommodity.Name = "lstVCommodity";
            this.lstVCommodity.Size = new Size(690, 0x9f);
            this.lstVCommodity.TabIndex = 0x19;
            this.lstVCommodity.TabStop = false;
            this.lstVCommodity.UseCompatibleStateImageBehavior = false;
            this.lstVCommodity.View = View.Details;
            this.buttonSelF5.ImeMode = ImeMode.NoControl;
            this.buttonSelF5.Location = new Point(0xc3, 13);
            this.buttonSelF5.Name = "buttonSelF5";
            this.buttonSelF5.Size = new Size(0x39, 20);
            this.buttonSelF5.TabIndex = 0x18;
            this.buttonSelF5.Text = "刷新";
            this.buttonSelF5.UseVisualStyleBackColor = true;
            this.buttonSelF5.Click += new EventHandler(this.buttonSelF5_Click);
            this.labelCommodityF5.ImeMode = ImeMode.NoControl;
            this.labelCommodityF5.Location = new Point(20, 15);
            this.labelCommodityF5.Name = "labelCommodityF5";
            this.labelCommodityF5.Size = new Size(0x48, 0x10);
            this.labelCommodityF5.TabIndex = 0x17;
            this.labelCommodityF5.Text = "商品代码：";
            this.labelCommodityF5.TextAlign = ContentAlignment.MiddleCenter;
            this.comboCommodityF5.Location = new Point(0x62, 14);
            this.comboCommodityF5.Name = "comboCommodityF5";
            this.comboCommodityF5.Size = new Size(80, 20);
            this.comboCommodityF5.TabIndex = 0x16;
            this.comboCommodityF5.SelectedIndexChanged += new EventHandler(this.comboCommodityF5_SelectedIndexChanged);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBoxCommodity);
            base.Name = "CommodityInfo";
            base.Size = new Size(700, 200);
            base.Load += new EventHandler(this.CommodityInfo_Load);
            this.groupBoxCommodity.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LstVCommodityFill(string commodityID)
        {
            if (TradeDataInfo.CommodityHashtable.Contains(commodityID))
            {
                TradeInterface.Gnnt.DataVO.CommodityInfo info = (TradeInterface.Gnnt.DataVO.CommodityInfo)TradeDataInfo.CommodityHashtable[commodityID];
                ImageList list = new ImageList
                {
                    ImageSize = new Size(1, 14)
                };
                this.lstVCommodity.SmallImageList = list;
                try
                {
                    if (info != null)
                    {
                        string text = string.Empty;
                        string str2 = string.Empty;
                        string str3 = string.Empty;
                        string str4 = string.Empty;
                        string str5 = Global.M_ResourceManager.GetString("TradeStr_MainFormF7_Full");
                        if (info.MarginType == 1)
                        {
                            if (info.BMargin == -1.0)
                            {
                                str3 = str5;
                            }
                            else
                            {
                                str3 = info.BMargin.ToString() + "%";
                            }
                            if (info.SMargin == -1.0)
                            {
                                str4 = str5;
                            }
                            else
                            {
                                str4 = info.SMargin.ToString() + "%";
                            }
                        }
                        else
                        {
                            if (info.BMargin == -1.0)
                            {
                                str3 = str5;
                            }
                            else
                            {
                                str3 = info.BMargin.ToString(Global.formatMoney);
                            }
                            if (info.SMargin == -1.0)
                            {
                                str4 = str5;
                            }
                            else
                            {
                                str4 = info.SMargin.ToString(Global.formatMoney);
                            }
                        }
                        if (info.CommType == 1)
                        {
                            text = info.BOpenComm.ToString().Trim() + "%";
                        }
                        else if (info.CommType == 2)
                        {
                            text = info.BOpenComm.ToString(Global.formatMoney);
                        }
                        if (info.DeliveryCommType == 1)
                        {
                            str2 = info.DeliveryBComm.ToString().Trim() + "%";
                        }
                        else if (info.DeliveryCommType == 2)
                        {
                            str2 = info.DeliveryBComm.ToString(Global.formatMoney);
                        }
                        this.lstVCommodity.Clear();
                        string str6 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_Project");
                        string str7 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_ProjectValue");
                        string[] strArray = this.commodityItemInfo.m_strItems.Split(new char[] { '|' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            this.lstVCommodity.Columns.Add(str6, 0x91, HorizontalAlignment.Left);
                            this.lstVCommodity.Columns.Add(str7, 190, HorizontalAlignment.Left);
                            this.groupBoxCommodity.Width = 0x162 * (i + 1);
                            string[] strArray2 = strArray[i].Split(new char[] { ';' });
                            for (int j = 0; j < strArray2.Length; j++)
                            {
                                ListViewItem item = null;
                                if (j < this.lstVCommodity.Items.Count)
                                {
                                    item = this.lstVCommodity.Items[j];
                                }
                                if (strArray2[j].Equals("CommodityID"))
                                {
                                    ColItemInfo info2 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CommodityID"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info2.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info2.name);
                                    }
                                    item.SubItems.Add(info.CommodityID);
                                }
                                if (strArray2[j].Equals("CommodityName"))
                                {
                                    ColItemInfo info3 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CommodityName"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info3.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info3.name);
                                    }
                                    item.SubItems.Add(info.CommodityName);
                                }
                                if (strArray2[j].Equals("SpreadUp"))
                                {
                                    ColItemInfo info4 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SpreadUp"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info4.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info4.name);
                                    }
                                    item.SubItems.Add(info.SpreadUp.ToString(info4.format));
                                }
                                if (strArray2[j].Equals("SpreadDown"))
                                {
                                    ColItemInfo info5 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SpreadDown"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info5.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info5.name);
                                    }
                                    item.SubItems.Add(info.SpreadDown.ToString(info5.format));
                                }
                                if (strArray2[j].Equals("BMargin"))
                                {
                                    ColItemInfo info6 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["BMargin"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info6.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info6.name);
                                    }
                                    item.SubItems.Add(str3);
                                }
                                if (strArray2[j].Equals("SMargin"))
                                {
                                    ColItemInfo info7 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SMargin"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info7.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info7.name);
                                    }
                                    item.SubItems.Add(str4);
                                }
                                if (strArray2[j].Equals("LSettledate"))
                                {
                                    ColItemInfo info8 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["LSettledate"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info8.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info8.name);
                                    }
                                    item.SubItems.Add(info.DeliveryDate);
                                }
                                if (strArray2[j].Equals("TmpTradecomm"))
                                {
                                    ColItemInfo info9 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["TmpTradecomm"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info9.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info9.name);
                                    }
                                    item.SubItems.Add(text);
                                }
                                if (strArray2[j].Equals("TmpSettlecomm"))
                                {
                                    ColItemInfo info10 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["TmpSettlecomm"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info10.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info10.name);
                                    }
                                    item.SubItems.Add(str2);
                                }
                                if (strArray2[j].Equals("BHold_Max"))
                                {
                                    ColItemInfo info11 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["BHold_Max"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info11.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info11.name);
                                    }
                                    item.SubItems.Add("");
                                }
                                if (strArray2[j].Equals("SHold_Max"))
                                {
                                    ColItemInfo info12 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SHold_Max"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info12.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info12.name);
                                    }
                                    item.SubItems.Add("");
                                }
                                if (strArray2[j].Equals("CtrtSize"))
                                {
                                    ColItemInfo info13 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CtrtSize"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info13.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info13.name);
                                    }
                                    item.SubItems.Add(info.CtrtSize.ToString());
                                }
                                if (strArray2[j].Equals("MaxHoldDays"))
                                {
                                    ColItemInfo info14 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["MaxHoldDays"];
                                    if (item == null)
                                    {
                                        item = new ListViewItem(info14.name);
                                        this.lstVCommodity.Items.Add(item);
                                    }
                                    else
                                    {
                                        item.SubItems.Add(info14.name);
                                    }
                                    item.SubItems.Add(info.MaxHoldDays);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
                }
            }
        }

        public void SetComboCommodityIDList(List<string> commodityIDList)
        {
            this.comboCommodityF5.Items.Clear();
            foreach (string str in commodityIDList)
            {
                if (str != this.operationManager.StrAll)
                {
                    this.comboCommodityF5.Items.Add(str);
                }
            }
            this.comboCommodityF5.SelectedIndex = 0;
        }
    }
}
