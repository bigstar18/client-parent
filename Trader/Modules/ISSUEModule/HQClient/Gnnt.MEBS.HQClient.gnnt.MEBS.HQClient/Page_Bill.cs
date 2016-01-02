using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.HQThread;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_Bill : Page_Main
	{
		private const int ROW_NUM = 3;
		private BillFieldInfo[] fieldInfo;
		private float zoomRate = 1f;
		private int totalPages = 1;
		private int curPageNo;
		private int iRows;
		private int iProductType;
		private ProductData stock;
		private Font fntTitle = new Font("楷体", 12f, FontStyle.Bold);
		private Font fntText = new Font("宋体", 10f, FontStyle.Regular);
		private int rowHeight;
		private int startY;
		private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
		private Pen pen = new Pen(SetInfo.RHColor.clGrid);
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private string[] m_strItems;
		private Hashtable b_htItemInfo;
		private Rectangle leftRect;
		private Rectangle rightRect;
		private int lastMinTime;
		public Page_Bill(Rectangle _rc, HQForm m_HQForm) : base(_rc, m_HQForm)
		{
			try
			{
				Logger.wirte(1, this.fntTitle.Height.ToString());
				this.pluginInfo = this.m_pluginInfo;
				this.setInfo = this.m_setInfo;
				this.AskForDataOnce();
				this.m_hqClient.CurrentPage = 4;
				this.MakeMenus();
				this.iProductType = this.m_hqClient.getProductType(this.m_hqClient.curCommodityInfo);
				this.initBillFieldInfo();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
			}
		}
		private void initBillFieldInfo()
		{
			this.b_htItemInfo = new Hashtable();
			this.b_htItemInfo.Add("Time", new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Time"), true, 64));
			this.b_htItemInfo.Add("Price", new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Price"), true, 70));
			this.b_htItemInfo.Add("CurVol", new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"), true, 60));
			this.b_htItemInfo.Add("Dingli", new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Dingli"), true, 50));
			this.b_htItemInfo.Add("ZhuanRang", new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ZhuanRang"), true, 50));
			string transactionBillName = this.setInfo.TransactionBillName;
			string[] array = transactionBillName.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				if (array2.Length == 2 && array2[1].Length > 0)
				{
					BillFieldInfo billFieldInfo = (BillFieldInfo)this.b_htItemInfo[array2[0]];
					if (billFieldInfo != null)
					{
						billFieldInfo.name = array2[1];
					}
				}
			}
			string transactionBillItems = this.setInfo.TransactionBillItems;
			this.m_strItems = transactionBillItems.Split(new char[]
			{
				';'
			});
			this.fieldInfo = new BillFieldInfo[this.m_strItems.Length - 1];
			for (int j = 0; j < this.m_strItems.Length - 1; j++)
			{
				this.fieldInfo[j] = (BillFieldInfo)this.b_htItemInfo[this.m_strItems[j]];
			}
		}
		private void AskForDataOnce()
		{
			this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
			if (this.stock == null)
			{
				if (this.m_hqClient.aProductData.Count > 50)
				{
					this.m_hqClient.aProductData.RemoveAt(50);
				}
				this.stock = new ProductData();
				this.stock.commodityInfo = this.m_hqClient.curCommodityInfo;
				if (this.stock.commodityInfo == null)
				{
					return;
				}
				this.m_hqClient.aProductData.Insert(0, this.stock);
				DateTime time = default(DateTime);
				if (this.stock != null && this.stock.realData != null)
				{
					time = this.stock.realData.time;
				}
				SendThread.AskForRealQuote(this.m_hqClient.curCommodityInfo.marketID, this.m_hqClient.curCommodityInfo.commodityCode, time, this.m_hqClient.sendThread);
			}
			CMDBillByVersionVO cMDBillByVersionVO = new CMDBillByVersionVO();
			cMDBillByVersionVO.marketID = this.m_hqClient.curCommodityInfo.marketID;
			cMDBillByVersionVO.code = this.m_hqClient.curCommodityInfo.commodityCode;
			cMDBillByVersionVO.type = 2;
			cMDBillByVersionVO.time = 0L;
			cMDBillByVersionVO.ReservedField = string.Empty;
			if (this.stock == null || this.stock.aBill.Count == 0)
			{
				cMDBillByVersionVO.totalAmount = 0L;
			}
			else
			{
				BillDataVO billDataVO = (BillDataVO)this.stock.aBill[this.stock.aBill.Count - 1];
				cMDBillByVersionVO.totalAmount = billDataVO.totalAmount;
			}
			this.m_hqClient.sendThread.AskForData(cMDBillByVersionVO);
		}
		protected override void AskForDataOnTimer()
		{
		}
		public override void Paint(Graphics g, int v)
		{
			try
			{
				this.initVisibleField();
				this.initPageInfo(g);
				this.paintTitle(g);
				this.paintBillData(g);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
			}
		}
		private void initVisibleField()
		{
			int num = this.m_rc.Width / 3 - 4;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = this.fieldInfo.Length;
			for (int i = 0; i < num4; i++)
			{
				if (num2 + this.fieldInfo[i].width >= num)
				{
					for (int j = i; j < num4; j++)
					{
						this.fieldInfo[j].visible = false;
					}
					break;
				}
				this.fieldInfo[i].visible = true;
				num3++;
				num2 += this.fieldInfo[i].width;
			}
			if (num3 <= 0)
			{
				num3 = 1;
			}
			if (num3 == num4)
			{
				this.zoomRate = (float)num / (float)num2;
			}
		}
		private void initPageInfo(Graphics g)
		{
			this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
			if (this.stock == null || this.stock.realData == null || this.stock.aBill == null)
			{
				this.totalPages = 1;
				this.curPageNo = 0;
				return;
			}
			int count = this.stock.aBill.Count;
			if (count <= 0)
			{
				this.totalPages = 1;
				this.curPageNo = 0;
				return;
			}
			int num = 0;
			try
			{
				num = this.fntTitle.Height;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				num = 19;
			}
			int num2 = 0;
			try
			{
				num2 = this.fntText.Height;
			}
			catch (Exception ex2)
			{
				Logger.wirte(3, ex2.Message);
				num2 = 16;
			}
			this.rowHeight = num2 + 2;
			this.iRows = (this.m_rc.Height - num - num2 - 6) / this.rowHeight;
			int num3 = (count - 1) / (this.iRows * 3);
			if ((count - 1) % (this.iRows * 3) != 0)
			{
				num3++;
			}
			if (num3 != this.totalPages)
			{
				if (num3 == 0)
				{
					num3 = 1;
				}
				this.totalPages = num3;
				this.curPageNo = 0;
			}
		}
		private void paintTitle(Graphics g)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			if (this.stock != null)
			{
				text = this.stock.commodityInfo.commodityCode;
				CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[this.stock.commodityInfo.marketID + this.stock.commodityInfo.commodityCode];
				if (codeTable != null)
				{
					text3 = codeTable.sName;
				}
			}
			if (text3.Equals(text))
			{
				text3 = "";
			}
			string text4 = text2;
			text2 = string.Concat(new string[]
			{
				text4,
				text3,
				" ",
				text,
				" ",
				this.pluginInfo.HQResourceManager.GetString("HQStr_TradeList")
			});
			int num = this.m_rc.X;
			int num2 = this.m_rc.Y;
			try
			{
				this.m_Brush.Color = SetInfo.RHColor.clProductName;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
			num += (this.m_rc.Width - (int)g.MeasureString(text2, this.fntTitle).Width) / 2;
			if (num < 0)
			{
				num = 0;
			}
			g.DrawString(text2, this.fntTitle, this.m_Brush, (float)num, (float)num2);
			Point point = new Point(num, num2);
			Point[] points = new Point[]
			{
				new Point(point.X - 30, point.Y + 10),
				new Point(point.X - 20, point.Y + 4),
				new Point(point.X - 20, point.Y + 7),
				new Point(point.X - 5, point.Y + 7),
				new Point(point.X - 5, point.Y + 11),
				new Point(point.X - 20, point.Y + 11),
				new Point(point.X - 20, point.Y + 14)
			};
			this.leftRect = new Rectangle(point.X - 30, point.Y - 7, 30, (int)g.MeasureString(text2, this.fntTitle).Height);
			point = new Point(point.X + (int)g.MeasureString(text2, this.fntTitle).Width + 5, point.Y + 9);
			Point[] points2 = new Point[]
			{
				new Point(point.X, point.Y - 2),
				new Point(point.X + 15, point.Y - 2),
				new Point(point.X + 15, point.Y - 5),
				new Point(point.X + 25, point.Y),
				new Point(point.X + 15, point.Y + 5),
				new Point(point.X + 15, point.Y + 2),
				new Point(point.X, point.Y + 2)
			};
			this.rightRect = new Rectangle(point.X, point.Y - 7, 30, (int)g.MeasureString(text2, this.fntTitle).Height);
			g.DrawPolygon(new Pen(Brushes.White, 1f), points);
			g.DrawPolygon(new Pen(Brushes.White, 1f), points2);
			num = this.m_rc.X;
			num2 = this.m_rc.Y + this.fntTitle.Height;
			this.pen.Color = SetInfo.RHColor.clGrid;
			g.DrawRectangle(this.pen, num, num2, num + this.m_rc.Width - 1, this.m_rc.Height - this.fntTitle.Height);
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height - 1, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height - 1);
			for (int i = 1; i < 3; i++)
			{
				g.DrawLine(this.pen, num + this.m_rc.Width / 3 * i, num2, num + this.m_rc.Width / 3 * i, num2 + this.m_rc.Height - this.fntTitle.Height);
			}
			g.DrawLine(this.pen, num, num2 + this.fntText.Height + 2, num + this.m_rc.Width - 1, num2 + this.fntText.Height + 2);
			this.startY = num2 + this.fntText.Height + 4;
			this.m_Brush.Color = SetInfo.RHColor.clItem;
			int num3 = this.fieldInfo.Length;
			num2++;
			for (int j = 0; j < 3; j++)
			{
				num = this.m_rc.X + this.m_rc.Width / 3 * j;
				int num4 = 0;
				while (num4 < num3 && this.fieldInfo[num4].visible)
				{
					num += (int)((float)this.fieldInfo[num4].width * this.zoomRate);
					string name = this.fieldInfo[num4].name;
					int num5 = (int)g.MeasureString(name, this.fntText).Width;
					g.DrawString(name, this.fntText, this.m_Brush, (float)(num - num5), (float)num2);
					num4++;
				}
			}
			string text5 = string.Concat(new object[]
			{
				this.pluginInfo.HQResourceManager.GetString("HQStr_PagePrefix"),
				this.totalPages - this.curPageNo,
				this.pluginInfo.HQResourceManager.GetString("HQStr_PageSuffix"),
				" ",
				this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPagePrefix"),
				this.totalPages,
				this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPageSuffix")
			});
			this.m_Brush.Color = SetInfo.RHColor.clGrid;
			g.DrawString(text5, this.fntText, this.m_Brush, (float)(this.m_rc.X + this.m_rc.Width - (int)g.MeasureString(text5, this.fntText).Width), (float)(this.m_rc.Y + num2 - this.fntText.Height));
		}
		private void paintBillData(Graphics g)
		{
			this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
			if (this.stock == null || this.stock.realData == null || this.stock.aBill == null)
			{
				return;
			}
			int precision = this.m_hqClient.GetPrecision(this.stock.commodityInfo);
			int num = this.iRows * 3;
			int count = this.stock.aBill.Count;
			if (count <= 0)
			{
				return;
			}
			if (count < num)
			{
				num = count;
			}
			int num2 = this.m_rc.X;
			int num3 = this.startY;
			int num4 = count - num * (this.curPageNo + 1);
			if (num4 < 0)
			{
				num4 = 0;
			}
			int num5 = num4 + num;
			if (num5 > count)
			{
				num5 = count;
				num4 = num5 - num;
				if (num4 <= 0)
				{
					num4 = 1;
				}
			}
			for (int i = num4; i < num5; i++)
			{
				num2 = this.m_rc.X;
				num2 += this.m_rc.Width / 3 * ((i - num4) / this.iRows);
				if ((i - num4) % this.iRows == 0)
				{
					num3 = this.startY;
				}
				BillDataVO billDataVO = null;
				if (i > 0)
				{
					billDataVO = (BillDataVO)this.stock.aBill[i - 1];
					if (billDataVO == null)
					{
						return;
					}
				}
				BillDataVO billDataVO2 = (BillDataVO)this.stock.aBill[i];
				if (billDataVO2 == null)
				{
					return;
				}
				if (this.lastMinTime != billDataVO2.time / 100 && num3 != this.startY)
				{
					g.DrawLine(new Pen(SetInfo.RHColor.clGrid)
					{
						DashStyle = DashStyle.Dash
					}, new Point(num2, num3), new Point(num2 + this.m_rc.Width / 3, num3));
				}
				for (int j = 0; j < this.fieldInfo.Length; j++)
				{
					if ("Time".Equals(this.m_strItems[j]))
					{
						if (this.fieldInfo[j].visible)
						{
							string text = TradeTimeVO.HHMMSSIntToString(billDataVO2.time);
							this.lastMinTime = billDataVO2.time / 100;
							this.m_Brush.Color = SetInfo.RHColor.clNumber;
							num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate);
							g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
						}
					}
					else if ("Price".Equals(this.m_strItems[j]))
					{
						if (this.fieldInfo[j].visible)
						{
							string text = M_Common.FloatToString((double)billDataVO2.curPrice, precision);
							if (billDataVO2.curPrice > this.stock.realData.yesterBalancePrice)
							{
								this.m_Brush.Color = SetInfo.RHColor.clIncrease;
							}
							else if (billDataVO2.curPrice < this.stock.realData.yesterBalancePrice)
							{
								this.m_Brush.Color = SetInfo.RHColor.clDecrease;
							}
							else
							{
								this.m_Brush.Color = SetInfo.RHColor.clEqual;
							}
							num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate);
							g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
						}
					}
					else if ("CurVol".Equals(this.m_strItems[j]))
					{
						if (this.fieldInfo[j].visible)
						{
							string text;
							if (billDataVO == null)
							{
								text = Convert.ToString(billDataVO2.totalAmount);
							}
							else
							{
								text = Convert.ToString((int)(billDataVO2.totalAmount - billDataVO.totalAmount));
							}
							this.m_Brush.Color = SetInfo.RHColor.clVolume;
							num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate - 16f);
							g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
							if (this.iProductType != 2 && this.iProductType != 3)
							{
								byte b;
								if (billDataVO == null)
								{
									b = 2;
								}
								else if (billDataVO.buyPrice <= 0.001f)
								{
									b = 1;
								}
								else if (billDataVO2.curPrice >= billDataVO.sellPrice)
								{
									b = 0;
								}
								else if (billDataVO2.curPrice <= billDataVO.buyPrice)
								{
									b = 1;
								}
								else
								{
									int num6 = (int)((billDataVO.sellPrice - billDataVO2.curPrice) * 1000f);
									float num7 = (float)((int)((billDataVO2.curPrice - billDataVO.buyPrice) * 1000f));
									if ((float)num6 < num7)
									{
										b = 0;
									}
									else if ((float)num6 > num7)
									{
										b = 1;
									}
									else
									{
										b = 2;
									}
								}
								if (b == 0)
								{
									text = "↑";
									this.m_Brush.Color = SetInfo.RHColor.clIncrease;
								}
								else if (b == 1)
								{
									text = "↓";
									this.m_Brush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									text = "–";
									this.m_Brush.Color = SetInfo.RHColor.clEqual;
								}
								g.DrawString(text, this.fntText, this.m_Brush, (float)num2, (float)num3);
							}
						}
					}
					else if ("Dingli".Equals(this.m_strItems[j]))
					{
						if (this.fieldInfo[j].visible)
						{
							string text = Convert.ToString(billDataVO2.openAmount);
							num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate);
							this.m_Brush.Color = SetInfo.RHColor.clNumber;
							g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
						}
					}
					else if ("ZhuanRang".Equals(this.m_strItems[j]))
					{
						if (this.fieldInfo[j].visible)
						{
							string text = Convert.ToString(billDataVO2.closeAmount);
							num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate);
							this.m_Brush.Color = SetInfo.RHColor.clNumber;
							g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
						}
					}
					else if ("sng".Equals(this.m_strItems[j]) && this.fieldInfo[j].visible)
					{
						string text = Convert.ToString(billDataVO2.closeAmount);
						num2 += (int)((float)this.fieldInfo[j].width * this.zoomRate);
						this.m_Brush.Color = SetInfo.RHColor.clNumber;
						g.DrawString(text, this.fntText, this.m_Brush, (float)(num2 - (int)g.MeasureString(text, this.fntText).Width), (float)num3);
					}
				}
				num3 += this.rowHeight;
			}
		}
		protected override void Page_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.leftRect.Contains(e.Location))
			{
				this.m_hqForm.ChangeStock(true);
				this.AskForDataOnce();
				this.m_hqForm.Repaint();
			}
			else if (this.rightRect.Contains(e.Location))
			{
				this.m_hqForm.ChangeStock(false);
				this.AskForDataOnce();
				this.m_hqForm.Repaint();
			}
			((HQClientForm)this.m_hqForm).mainWindow.Focus();
		}
		protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
		{
		}
		protected override void Page_MouseMove(object sender, MouseEventArgs e)
		{
			Point location = e.Location;
			if (this.leftRect.Contains(location) || this.rightRect.Contains(location))
			{
				this.m_hqForm.M_Cursor = Cursors.Hand;
				return;
			}
			this.m_hqForm.M_Cursor = Cursors.Default;
		}
		protected override void Page_KeyDown(object sender, KeyEventArgs e)
		{
			bool flag = false;
			switch (e.KeyCode)
			{
			case Keys.Prior:
				if (this.curPageNo < this.totalPages - 1)
				{
					this.curPageNo++;
					flag = true;
				}
				break;
			case Keys.Next:
				if (this.curPageNo > 0)
				{
					this.curPageNo--;
					flag = true;
				}
				break;
			}
			if (flag)
			{
				this.m_hqForm.Repaint();
			}
		}
		private void MakeMenus()
		{
			this.contextMenuStrip.Items.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
			toolStripMenuItem.Name = "cmd_80";
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
			toolStripMenuItem2.Name = "minline";
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
			toolStripMenuItem3.Name = "kline";
			ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
			toolStripMenuItem4.Name = "cmd_60";
			this.contextMenuStrip.Items.Add(toolStripMenuItem2);
			this.contextMenuStrip.Items.Add(toolStripMenuItem3);
			this.contextMenuStrip.Items.Add(toolStripMenuItem4);
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			base.AddCommonMenu();
		}
		protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string name = e.ClickedItem.Name;
			if (name.IndexOf("cmd_") >= 0)
			{
				this.m_hqForm.UserCommand(name.Substring(4));
			}
			else if (name.Equals("minline"))
			{
				this.m_hqForm.ShowPageMinLine();
			}
			else if (name.Equals("kline"))
			{
				this.m_hqForm.ShowPageKLine();
			}
			else
			{
				this.m_hqForm.UserCommand(name);
			}
			this.m_hqForm.Repaint();
		}
		public override void Dispose()
		{
			this.fntTitle.Dispose();
			this.fntText.Dispose();
			this.m_Brush.Dispose();
			this.pen.Dispose();
		}
	}
}
