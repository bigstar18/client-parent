using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Modules.Query
{
	public class CommodityInfo : UserControl
	{
		private CommodityItemInfo commodityItemInfo = new CommodityItemInfo();
		private OperationManager operationManager = OperationManager.GetInstance();
		private IContainer components;
		internal GroupBox groupBoxCommodity;
		internal ListView lstVCommodity;
		private Button buttonSelF5;
		private Label labelCommodityF5;
		internal ComboBox comboCommodityF5;
		public CommodityInfo()
		{
			this.InitializeComponent();
			this.operationManager.SetComboCommodityEvent += new OperationManager.SetComboCommodityCallBack(this.SetComboCommodityIDList);
			this.CreateHandle();
		}
		private void LstVCommodityFill(string commodityID)
		{
			if (TradeDataInfo.CommodityHashtable.Contains(commodityID))
			{
				TradeInterface.Gnnt.DataVO.CommodityInfo commodityInfo = (TradeInterface.Gnnt.DataVO.CommodityInfo)TradeDataInfo.CommodityHashtable[commodityID];
				ImageList imageList = new ImageList();
				imageList.ImageSize = new Size(1, 14);
				this.lstVCommodity.SmallImageList = imageList;
				try
				{
					if (commodityInfo != null)
					{
						string text = string.Empty;
						string text2 = string.Empty;
						string text3 = string.Empty;
						string text4 = string.Empty;
						string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF7_Full");
						if (commodityInfo.MarginType == 1)
						{
							if (commodityInfo.BMargin == -1.0)
							{
								text3 = @string;
							}
							else
							{
								text3 = commodityInfo.BMargin.ToString() + "%";
							}
							if (commodityInfo.SMargin == -1.0)
							{
								text4 = @string;
							}
							else
							{
								text4 = commodityInfo.SMargin.ToString() + "%";
							}
						}
						else
						{
							if (commodityInfo.BMargin == -1.0)
							{
								text3 = @string;
							}
							else
							{
								text3 = commodityInfo.BMargin.ToString(Global.formatMoney);
							}
							if (commodityInfo.SMargin == -1.0)
							{
								text4 = @string;
							}
							else
							{
								text4 = commodityInfo.SMargin.ToString(Global.formatMoney);
							}
						}
						if (commodityInfo.CommType == 1)
						{
							text = commodityInfo.BOpenComm.ToString().Trim() + "%";
						}
						else
						{
							if (commodityInfo.CommType == 2)
							{
								text = commodityInfo.BOpenComm.ToString(Global.formatMoney);
							}
						}
						if (commodityInfo.DeliveryCommType == 1)
						{
							text2 = commodityInfo.DeliveryBComm.ToString().Trim() + "%";
						}
						else
						{
							if (commodityInfo.DeliveryCommType == 2)
							{
								text2 = commodityInfo.DeliveryBComm.ToString(Global.formatMoney);
							}
						}
						this.lstVCommodity.Clear();
						string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_Project");
						string string3 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_ProjectValue");
						string[] array = this.commodityItemInfo.m_strItems.Split(new char[]
						{
							'|'
						});
						for (int i = 0; i < array.Length; i++)
						{
							this.lstVCommodity.Columns.Add(string2, 145, HorizontalAlignment.Left);
							this.lstVCommodity.Columns.Add(string3, 190, HorizontalAlignment.Left);
							this.groupBoxCommodity.Width = 354 * (i + 1);
							string[] array2 = array[i].Split(new char[]
							{
								';'
							});
							for (int j = 0; j < array2.Length; j++)
							{
								ListViewItem listViewItem = null;
								if (j < this.lstVCommodity.Items.Count)
								{
									listViewItem = this.lstVCommodity.Items[j];
								}
								if (array2[j].Equals("CommodityID"))
								{
									ColItemInfo colItemInfo = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CommodityID"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo.name);
									}
									listViewItem.SubItems.Add(commodityInfo.CommodityID);
								}
								if (array2[j].Equals("CommodityName"))
								{
									ColItemInfo colItemInfo2 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CommodityName"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo2.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo2.name);
									}
									listViewItem.SubItems.Add(commodityInfo.CommodityName);
								}
								if (array2[j].Equals("SpreadUp"))
								{
									ColItemInfo colItemInfo3 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SpreadUp"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo3.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo3.name);
									}
									listViewItem.SubItems.Add(commodityInfo.SpreadUp.ToString(colItemInfo3.format));
								}
								if (array2[j].Equals("SpreadDown"))
								{
									ColItemInfo colItemInfo4 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SpreadDown"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo4.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo4.name);
									}
									listViewItem.SubItems.Add(commodityInfo.SpreadDown.ToString(colItemInfo4.format));
								}
								if (array2[j].Equals("BMargin"))
								{
									ColItemInfo colItemInfo5 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["BMargin"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo5.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo5.name);
									}
									listViewItem.SubItems.Add(text3);
								}
								if (array2[j].Equals("SMargin"))
								{
									ColItemInfo colItemInfo6 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SMargin"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo6.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo6.name);
									}
									listViewItem.SubItems.Add(text4);
								}
								if (array2[j].Equals("LSettledate"))
								{
									ColItemInfo colItemInfo7 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["LSettledate"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo7.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo7.name);
									}
									listViewItem.SubItems.Add(commodityInfo.DeliveryDate);
								}
								if (array2[j].Equals("TmpTradecomm"))
								{
									ColItemInfo colItemInfo8 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["TmpTradecomm"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo8.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo8.name);
									}
									listViewItem.SubItems.Add(text);
								}
								if (array2[j].Equals("TmpSettlecomm"))
								{
									ColItemInfo colItemInfo9 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["TmpSettlecomm"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo9.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo9.name);
									}
									listViewItem.SubItems.Add(text2);
								}
								if (array2[j].Equals("BHold_Max"))
								{
									ColItemInfo colItemInfo10 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["BHold_Max"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo10.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo10.name);
									}
									listViewItem.SubItems.Add("");
								}
								if (array2[j].Equals("SHold_Max"))
								{
									ColItemInfo colItemInfo11 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["SHold_Max"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo11.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo11.name);
									}
									listViewItem.SubItems.Add("");
								}
								if (array2[j].Equals("CtrtSize"))
								{
									ColItemInfo colItemInfo12 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["CtrtSize"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo12.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo12.name);
									}
									listViewItem.SubItems.Add(commodityInfo.CtrtSize.ToString());
								}
								if (array2[j].Equals("MaxHoldDays"))
								{
									ColItemInfo colItemInfo13 = (ColItemInfo)this.commodityItemInfo.m_htItemInfo["MaxHoldDays"];
									if (listViewItem == null)
									{
										listViewItem = new ListViewItem(colItemInfo13.name);
										this.lstVCommodity.Items.Add(listViewItem);
									}
									else
									{
										listViewItem.SubItems.Add(colItemInfo13.name);
									}
									listViewItem.SubItems.Add(commodityInfo.MaxHoldDays);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
				}
			}
		}
		private void comboCommodityF5_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = this.comboCommodityF5.Text;
			this.LstVCommodityFill(text);
		}
		private void buttonSelF5_Click(object sender, EventArgs e)
		{
			string text = this.comboCommodityF5.Text;
			this.LstVCommodityFill(text);
		}
		private void CommodityInfo_Load(object sender, EventArgs e)
		{
			string text = this.comboCommodityF5.Text;
			this.LstVCommodityFill(text);
		}
		public void SetComboCommodityIDList(List<string> commodityIDList)
		{
			this.comboCommodityF5.Items.Clear();
			foreach (string current in commodityIDList)
			{
				if (current != this.operationManager.StrAll)
				{
					this.comboCommodityF5.Items.Add(current);
				}
			}
			this.comboCommodityF5.SelectedIndex = 0;
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
			this.groupBoxCommodity = new GroupBox();
			this.lstVCommodity = new ListView();
			this.buttonSelF5 = new Button();
			this.labelCommodityF5 = new Label();
			this.comboCommodityF5 = new ComboBox();
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
			this.groupBoxCommodity.TabIndex = 23;
			this.groupBoxCommodity.TabStop = false;
			this.groupBoxCommodity.Text = "商品查询";
			this.lstVCommodity.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lstVCommodity.GridLines = true;
			this.lstVCommodity.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.lstVCommodity.Location = new Point(5, 39);
			this.lstVCommodity.Name = "lstVCommodity";
			this.lstVCommodity.Size = new Size(690, 159);
			this.lstVCommodity.TabIndex = 25;
			this.lstVCommodity.TabStop = false;
			this.lstVCommodity.UseCompatibleStateImageBehavior = false;
			this.lstVCommodity.View = View.Details;
			this.buttonSelF5.ImeMode = ImeMode.NoControl;
			this.buttonSelF5.Location = new Point(195, 13);
			this.buttonSelF5.Name = "buttonSelF5";
			this.buttonSelF5.Size = new Size(57, 20);
			this.buttonSelF5.TabIndex = 24;
			this.buttonSelF5.Text = "刷新";
			this.buttonSelF5.UseVisualStyleBackColor = true;
			this.buttonSelF5.Click += new EventHandler(this.buttonSelF5_Click);
			this.labelCommodityF5.ImeMode = ImeMode.NoControl;
			this.labelCommodityF5.Location = new Point(20, 15);
			this.labelCommodityF5.Name = "labelCommodityF5";
			this.labelCommodityF5.Size = new Size(72, 16);
			this.labelCommodityF5.TabIndex = 23;
			this.labelCommodityF5.Text = "商品代码：";
			this.labelCommodityF5.TextAlign = ContentAlignment.MiddleCenter;
			this.comboCommodityF5.Location = new Point(98, 14);
			this.comboCommodityF5.Name = "comboCommodityF5";
			this.comboCommodityF5.Size = new Size(80, 20);
			this.comboCommodityF5.TabIndex = 22;
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
	}
}
