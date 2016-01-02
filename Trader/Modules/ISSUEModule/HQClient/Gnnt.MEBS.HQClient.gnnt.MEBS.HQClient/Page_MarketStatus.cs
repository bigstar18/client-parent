using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_MarketStatus : Page_Main
	{
		private const int RANK_RATE = 0;
		private const int RANK_CONSIGNRATE = 6;
		private const int RANK_AMOUNTRATE = 5;
		private const int RANK_TOTALMONEY = 8;
		private Font font = new Font("宋体", 12f, FontStyle.Regular);
		private SolidBrush m_Brush;
		private Pen pen;
		private string[,] STOCK_RANK_NAME;
		private Pos[,] pos;
		public MarketStatusVO[] statusData;
		private int iCount = 7;
		private int iHighlightRowIndex;
		private int iHighlightColIndex;
		public Packet_MarketStatus packetInfo;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		public Page_MarketStatus(Rectangle _rc, HQForm hqForm) : base(_rc, hqForm)
		{
			this.pluginInfo = this.m_pluginInfo;
			this.setInfo = this.m_setInfo;
			this.m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
			this.pen = new Pen(SetInfo.RHColor.clGrid);
			this.AskForDataOnTimer();
			this.MakeMenus();
			this.STOCK_RANK_NAME = new string[3, 3];
			this.STOCK_RANK_NAME[0, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_UpRateList");
			this.STOCK_RANK_NAME[1, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_DownRateList");
			this.STOCK_RANK_NAME[2, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_SwingList");
			this.STOCK_RANK_NAME[0, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinUpRateList");
			this.STOCK_RANK_NAME[1, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinDownRateList");
			this.STOCK_RANK_NAME[2, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr_VolRateList");
			this.STOCK_RANK_NAME[0, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRateDesc");
			this.STOCK_RANK_NAME[1, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRateAsce");
			this.STOCK_RANK_NAME[2, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_MoneyList");
			this.m_hqClient.CurrentPage = 5;
		}
		protected override void AskForDataOnTimer()
		{
			CMDMarketSortVO cMDMarketSortVO = new CMDMarketSortVO();
			this.iCount = this.GetICount();
			cMDMarketSortVO.num = this.iCount;
			this.m_hqClient.sendThread.AskForData(cMDMarketSortVO);
		}
		private int GetICount()
		{
			int num = this.font.Height + 2;
			return (this.m_rc.Height / 3 - (num + 2)) / num;
		}
		public override void Paint(Graphics g, int v)
		{
			try
			{
				if (this.statusData != null)
				{
					Logger.wirte(1, string.Concat(new object[]
					{
						"this.iCount = ",
						this.iCount,
						"    statusData.length / 9 = ",
						this.statusData.Length / 9
					}));
				}
				this.initilizer(g);
				this.paintGrid(g);
				this.paintTitle(g);
				this.paintStockData(g);
				this.m_hqForm.EndPaint();
				this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MarketStatus-Paint异常：" + ex.Message);
			}
		}
		private void initilizer(Graphics g)
		{
			int num = this.m_rc.X;
			int num2 = this.font.Height + 2;
			int num3 = this.m_rc.Y + num2 + 2;
			int num4 = (this.m_rc.Height / 3 - (num2 + 2)) / num2;
			if (this.iCount > num4)
			{
				this.iCount = num4;
			}
			if (this.iCount <= 0)
			{
				return;
			}
			this.pos = new Pos[this.iCount * 3, 3];
			for (int i = 0; i < this.iCount * 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					this.pos[i, j] = new Pos();
					this.pos[i, j].x = num;
					this.pos[i, j].y = num3;
					num += this.m_rc.Width / 3;
				}
				num = this.m_rc.X;
				if (i == this.iCount - 1)
				{
					num3 = this.m_rc.Y + this.m_rc.Height / 3 + num2 + 2;
				}
				else if (i == 2 * this.iCount - 1)
				{
					num3 = this.m_rc.Y + this.m_rc.Height / 3 * 2 + num2 + 2;
				}
				else
				{
					num3 += num2;
				}
			}
		}
		private void paintGrid(Graphics g)
		{
			this.pen.Color = SetInfo.RHColor.clEqual;
			g.DrawRectangle(this.pen, this.m_rc.X, this.m_rc.Y, this.m_rc.Width - 1, this.m_rc.Height - 2);
			int num = this.font.Height + 2;
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + num);
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3);
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 + num);
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 * 2, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 * 2);
			g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 * 2 + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 * 2 + num);
			g.DrawLine(this.pen, this.m_rc.X + this.m_rc.Width / 3, this.m_rc.Y, this.m_rc.X + this.m_rc.Width / 3, this.m_rc.Y + this.m_rc.Height - 2);
			g.DrawLine(this.pen, this.m_rc.X + this.m_rc.Width / 3 * 2, this.m_rc.Y, this.m_rc.X + this.m_rc.Width / 3 * 2, this.m_rc.Y + this.m_rc.Height - 2);
		}
		private void paintTitle(Graphics g)
		{
			int num = this.m_rc.X;
			int num2 = this.m_rc.Y;
			int num3 = this.m_rc.Width / 3;
			int arg_33_0 = this.m_rc.Height / 3;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					string text = this.STOCK_RANK_NAME[i, j];
					int num4 = (int)g.MeasureString(text, this.font).Width;
					int num5 = 0;
					if (num3 > num4)
					{
						num5 = (num3 - num4) / 2;
					}
					this.m_Brush.Color = SetInfo.RHColor.clMultiQuote_TitleBack;
					g.FillRectangle(this.m_Brush, num + 1, num2 + 1, this.m_rc.Width / 3 - 1, this.font.Height + 1);
					this.m_Brush.Color = SetInfo.RHColor.clItem;
					g.DrawString(text, this.font, this.m_Brush, (float)(num + num5), (float)(num2 + 2));
					num += this.m_rc.Width / 3;
				}
				num = this.m_rc.X;
				num2 += this.m_rc.Height / 3;
			}
		}
		private void paintStockData(Graphics g)
		{
			if (this.packetInfo != null && this.statusData != null)
			{
				for (int i = 0; i < this.iCount * 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						int indexOfStatusDataArray = this.getIndexOfStatusDataArray(i, j);
						if (indexOfStatusDataArray >= 0 && indexOfStatusDataArray < this.statusData.Length)
						{
							MarketStatusVO value = this.statusData[indexOfStatusDataArray];
							int x = this.pos[i, j].x;
							int y = this.pos[i, j].y;
							if (j == 0 || (j == 1 && i >= 0 && i < 2 * this.iCount))
							{
								this.paintRankData(g, value, 0, x, y);
							}
							else if (i >= 2 * this.iCount && i < 3 * this.iCount && j == 1)
							{
								this.paintRankData(g, value, 5, x, y);
							}
							else if (i >= 2 * this.iCount && i < 3 * this.iCount && j == 2)
							{
								this.paintRankData(g, value, 8, x, y);
							}
							else
							{
								this.paintRankData(g, value, 6, x, y);
							}
						}
					}
				}
			}
		}
		private void paintHighlight(int newRowIndex, int newColIndex)
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			if (newRowIndex >= this.iCount * 3)
			{
				return;
			}
			if (this.iCount <= 0)
			{
				return;
			}
			Pos pos = null;
			if (this.iHighlightRowIndex < this.iCount * 3)
			{
				pos = this.pos[this.iHighlightRowIndex, this.iHighlightColIndex];
			}
			Pos pos2 = null;
			if (newRowIndex < this.iCount * 3)
			{
				pos2 = this.pos[newRowIndex, newColIndex];
			}
			Graphics m_Graphics = this.m_hqForm.M_Graphics;
			if (pos != null)
			{
				GDIDraw.XorRectangle(m_Graphics, new Rectangle(pos.x, pos.y, this.m_rc.Width / 3, this.font.Height), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
			}
			if (this.iHighlightRowIndex != newRowIndex || this.iHighlightColIndex != newColIndex)
			{
				if (pos2 != null)
				{
					GDIDraw.XorRectangle(m_Graphics, new Rectangle(pos2.x, pos2.y, this.m_rc.Width / 3, this.font.Height), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
				}
				this.iHighlightRowIndex = newRowIndex;
				this.iHighlightColIndex = newColIndex;
			}
		}
		private void paintRankData(Graphics g, MarketStatusVO value, int rankType, int x, int y)
		{
			string marketID = value.marketID;
			string code = value.code;
			CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[marketID + code];
			if (codeTable != null)
			{
				string arg_34_0 = codeTable.sName;
			}
			CommodityInfo commodityInfo = new CommodityInfo(marketID, code);
			int precision = this.m_hqClient.GetPrecision(commodityInfo);
			bool isPercent = true;
			if (rankType == 8 || rankType == 5)
			{
				isPercent = false;
			}
			string text = this.formatRankData(g, value.cur, precision, false);
			string text2 = this.formatRankData(g, value.value, 2, isPercent);
			this.m_Brush.Color = SetInfo.RHColor.clProductName;
			g.DrawString(code, this.font, this.m_Brush, (float)(x + 3), (float)y);
			switch (value.status)
			{
			case 0:
				this.m_Brush.Color = SetInfo.RHColor.clIncrease;
				break;
			case 1:
				this.m_Brush.Color = SetInfo.RHColor.clDecrease;
				break;
			case 2:
				this.m_Brush.Color = SetInfo.RHColor.clEqual;
				break;
			}
			int num = this.m_rc.Width / 3 - 128;
			if (num > 64)
			{
				int num2 = (num - 64) / 2;
				g.DrawString(text, this.font, this.m_Brush, (float)(x + this.m_rc.Width / 3 - 64 - num2 - (int)g.MeasureString(text, this.font).Width), (float)y);
			}
			else
			{
				g.DrawString(text, this.font, this.m_Brush, (float)(x + 128 - (int)g.MeasureString(text, this.font).Width), (float)y);
			}
			switch (rankType)
			{
			case 5:
				this.m_Brush.Color = SetInfo.RHColor.clVolume;
				goto IL_276;
			case 6:
				this.m_Brush.Color = SetInfo.RHColor.clNumber;
				goto IL_276;
			case 8:
				this.m_Brush.Color = SetInfo.RHColor.clNumber;
				goto IL_276;
			}
			if (value.value > 0f)
			{
				this.m_Brush.Color = SetInfo.RHColor.clIncrease;
			}
			else if (value.value == 0f)
			{
				this.m_Brush.Color = SetInfo.RHColor.clEqual;
			}
			else
			{
				this.m_Brush.Color = SetInfo.RHColor.clDecrease;
			}
			IL_276:
			g.DrawString(text2, this.font, this.m_Brush, (float)(x + this.m_rc.Width / 3 - (int)g.MeasureString(text2, this.font).Width - 3), (float)y);
		}
		private int getIndexOfStatusDataArray(int i, int j)
		{
			if (this.packetInfo == null)
			{
				return -1;
			}
			if (i % this.iCount >= this.packetInfo.iCount)
			{
				return -1;
			}
			int num = this.statusData.Length / 9;
			if (i >= 0 && i < this.iCount)
			{
				return j * 3 * num + i;
			}
			if (i >= this.iCount && i < 2 * this.iCount)
			{
				return (j * 3 + 1) * num + i - this.iCount;
			}
			if (i >= 2 * this.iCount && i < 3 * this.iCount)
			{
				return (j * 3 + 2) * num + i - 2 * this.iCount;
			}
			return -1;
		}
		private string formatRankData(Graphics g, float num, int iPrecision, bool isPercent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!isPercent)
			{
				stringBuilder.Append(M_Common.FloatToString((double)num, iPrecision));
			}
			else
			{
				if (num >= 0f)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(M_Common.FloatToString((double)num, iPrecision));
				stringBuilder.Append("%");
			}
			return stringBuilder.ToString();
		}
		private void selectStock(int x, int y)
		{
			int height = this.font.Height;
			for (int i = 0; i < this.iCount * 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int x2 = this.pos[i, j].x;
					int y2 = this.pos[i, j].y;
					if (x > x2 && x < x2 + this.m_rc.Width / 3 && y > y2 && y < y2 + height && (this.iHighlightRowIndex != i || this.iHighlightColIndex != j) && this.getIndexOfStatusDataArray(i, j) >= 0)
					{
						this.paintHighlight(i, j);
					}
				}
			}
		}
		protected override void Page_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.packetInfo != null && this.statusData != null)
				{
					this.selectStock(e.X, e.Y);
					((HQClientForm)this.m_hqForm).mainWindow.Focus();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MarketStatus-Page_MouseClick异常：" + ex.Message);
			}
		}
		protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.packetInfo != null && this.statusData != null)
				{
					int height = this.font.Height;
					for (int i = 0; i < this.iCount * 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							int x = this.pos[i, j].x;
							int y = this.pos[i, j].y;
							if (e.X > x && e.X < x + this.m_rc.Width / 3 && e.Y > y && e.Y < y + height)
							{
								int arg_97_0 = this.statusData.Length / 9;
								int indexOfStatusDataArray = this.getIndexOfStatusDataArray(i, j);
								if (indexOfStatusDataArray >= 0)
								{
									CommodityInfo commodityInfo = new CommodityInfo(this.statusData[indexOfStatusDataArray].marketID, this.statusData[indexOfStatusDataArray].code);
									this.m_hqForm.QueryStock(commodityInfo);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MarketStatus-Page_MouseDoubleClick异常：" + ex.Message);
			}
		}
		protected override void Page_MouseMove(object sender, MouseEventArgs e)
		{
		}
		protected override void Page_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (this.packetInfo != null && this.statusData != null)
				{
					Keys keyCode = e.KeyCode;
					if (keyCode != Keys.Return)
					{
						switch (keyCode)
						{
						case Keys.Left:
							if (this.iHighlightColIndex > 0)
							{
								this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex - 1);
							}
							break;
						case Keys.Up:
							if (this.iHighlightRowIndex > 0)
							{
								int num = 1;
								while (this.iHighlightRowIndex - num >= 0 && this.getIndexOfStatusDataArray(this.iHighlightRowIndex - num, this.iHighlightColIndex) < 0)
								{
									num++;
								}
								this.paintHighlight(this.iHighlightRowIndex - num, this.iHighlightColIndex);
							}
							break;
						case Keys.Right:
							if (this.iHighlightColIndex < 2)
							{
								this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex + 1);
							}
							break;
						case Keys.Down:
							if (this.iHighlightRowIndex < 3 * this.iCount - 1)
							{
								int num2 = 1;
								while (this.iHighlightRowIndex + num2 <= 3 * this.iCount - 1 && this.getIndexOfStatusDataArray(this.iHighlightRowIndex + num2, this.iHighlightColIndex) < 0)
								{
									num2++;
								}
								this.paintHighlight(this.iHighlightRowIndex + num2, this.iHighlightColIndex);
							}
							break;
						}
					}
					else
					{
						int indexOfStatusDataArray = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
						if (indexOfStatusDataArray >= 0)
						{
							CommodityInfo commodityInfo = new CommodityInfo(this.statusData[indexOfStatusDataArray].marketID, this.statusData[indexOfStatusDataArray].code);
							this.m_hqForm.QueryStock(commodityInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MarketStatus-Page_KeyDown异常：" + ex.Message);
			}
		}
		private void MakeMenus()
		{
			this.contextMenuStrip.Items.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
			toolStripMenuItem.Name = "cmd_60";
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
			toolStripMenuItem2.Name = "minline";
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
			toolStripMenuItem3.Name = "kline";
			this.contextMenuStrip.Items.Add(toolStripMenuItem2);
			this.contextMenuStrip.Items.Add(toolStripMenuItem3);
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			base.AddCommonMenu();
		}
		protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				string name = e.ClickedItem.Name;
				if (name.IndexOf("cmd_") >= 0)
				{
					this.m_hqForm.UserCommand(name.Substring(4));
				}
				else if (name.Equals("minline"))
				{
					int indexOfStatusDataArray = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
					if (indexOfStatusDataArray >= 0)
					{
						CommodityInfo commodityInfo = new CommodityInfo(this.statusData[indexOfStatusDataArray].marketID, this.statusData[indexOfStatusDataArray].code);
						this.m_hqForm.ShowPageMinLine(commodityInfo);
					}
				}
				else if (name.Equals("kline"))
				{
					int indexOfStatusDataArray2 = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
					if (indexOfStatusDataArray2 >= 0)
					{
						CommodityInfo commodityInfo2 = new CommodityInfo(this.statusData[indexOfStatusDataArray2].marketID, this.statusData[indexOfStatusDataArray2].code);
						this.m_hqForm.ShowPageMinLine(commodityInfo2);
					}
				}
				else
				{
					this.m_hqForm.UserCommand(name);
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MarketStatus-contextMenu_ItemClicked异常：" + ex.Message);
			}
		}
		public override void Dispose()
		{
			GC.Collect();
		}
	}
}
