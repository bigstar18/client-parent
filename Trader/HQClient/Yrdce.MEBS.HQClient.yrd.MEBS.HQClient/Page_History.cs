using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_History : Page_Main
	{
		private ArrayList m_aCode;
		private readonly int GAP = 3;
		private int iTitleHeight = 30;
		private int m_iRows;
		private int m_iCols;
		private int m_iWidth;
		private int m_iTotalPage;
		private int m_iCurPage;
		private int m_iHighlightRow;
		private int m_iHighlightCol;
		private Font font = new Font("宋体", 12f, FontStyle.Regular);
		private Font fontTitle = new Font("楷体", 14f, FontStyle.Bold);
		private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
		private Pen pen = new Pen(SetInfo.RHColor.clGrid);
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		public Page_History(Rectangle _rc, HQForm m_HQForm) : base(_rc, m_HQForm)
		{
			try
			{
				this.pluginInfo = this.m_pluginInfo;
				this.setInfo = this.m_setInfo;
				this.m_aCode = new ArrayList();
				for (int i = 0; i < this.m_hqClient.m_codeList.Count; i++)
				{
					CommodityInfo commodityInfo = (CommodityInfo)this.m_hqClient.m_codeList[i];
					CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
					if (codeTable != null && (codeTable.status == 1 || codeTable.status == 6))
					{
						this.m_aCode.Add(this.m_hqClient.m_codeList[i]);
					}
				}
				this.MakeMenus();
				this.m_hqClient.CurrentPage = 6;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_History异常：" + ex.Message);
			}
		}
		protected override void AskForDataOnTimer()
		{
		}
		public override void Paint(Graphics g, int v)
		{
			try
			{
				this.paintTitle(g);
				if (this.m_aCode.Count == 0)
				{
					this.paintPromptMessage(g);
				}
				else
				{
					this.calculateSize(g);
					this.paintProduct(g);
					this.m_hqForm.EndPaint();
					this.paintHighlight(-1, -1);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_History-Paint异常：" + ex.Message);
			}
		}
		private void paintTitle(Graphics g)
		{
			int num = this.m_rc.X;
			int num2 = this.m_rc.Y;
			this.m_Brush.Color = SetInfo.RHColor.clProductName;
			string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_History");
			num += (this.m_rc.Width - (int)g.MeasureString(@string, this.fontTitle).Width) / 2;
			if (num < 0)
			{
				num = 0;
			}
			g.DrawString(@string, this.fontTitle, this.m_Brush, (float)num, (float)num2);
			num = this.m_rc.X;
			num2 = this.m_rc.Y + this.fontTitle.Height;
			this.pen.Color = SetInfo.RHColor.clGrid;
			g.DrawRectangle(this.pen, num, num2, num + this.m_rc.Width - 1, this.m_rc.Height - this.fontTitle.Height - 1);
			this.iTitleHeight = this.fontTitle.Height;
		}
		private void calculateSize(Graphics g)
		{
			this.m_iRows = (this.m_rc.Height - this.iTitleHeight) / (this.font.Height + this.GAP);
			this.m_iWidth = (int)g.MeasureString("  大蒜十月  AB0210  ", this.font).Width;
			this.m_iCols = this.m_rc.Width / this.m_iWidth;
			if (this.m_iRows == 0 || this.m_iCols == 0)
			{
				return;
			}
			int num = this.m_aCode.Count / (this.m_iRows * this.m_iCols);
			if (this.m_aCode.Count % (this.m_iRows * this.m_iCols) > 0)
			{
				num++;
			}
			if (num != this.m_iTotalPage)
			{
				this.m_iCurPage = 0;
				this.m_iHighlightRow = 0;
				this.m_iHighlightCol = 0;
				this.m_iTotalPage = num;
			}
			if (this.m_iHighlightRow >= this.m_iRows)
			{
				this.m_iHighlightRow = 0;
			}
			if (this.m_iHighlightCol >= this.m_iCols)
			{
				this.m_iHighlightCol = 0;
			}
			if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol >= this.m_aCode.Count)
			{
				this.m_iHighlightRow = 0;
				this.m_iHighlightCol = 0;
			}
		}
		private void paintProduct(Graphics g)
		{
			string text = string.Concat(new object[]
			{
				this.pluginInfo.HQResourceManager.GetString("HQStr_PagePrefix"),
				this.m_iCurPage + 1,
				this.pluginInfo.HQResourceManager.GetString("HQStr_PageSuffix"),
				" ",
				this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPagePrefix"),
				this.m_iTotalPage,
				this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPageSuffix")
			});
			this.m_Brush.Color = SetInfo.RHColor.clGrid;
			g.DrawString(text, this.font, this.m_Brush, (float)(this.m_rc.X + this.m_rc.Width - (int)g.MeasureString(text, this.font).Width), (float)(this.m_rc.Y + this.GAP));
			int num = this.m_iCurPage * (this.m_iRows * this.m_iCols);
			for (int i = 0; i < this.m_iRows; i++)
			{
				int num2 = 0;
				while (num2 < this.m_iCols && num < this.m_aCode.Count)
				{
					CommodityInfo commodityInfo = (CommodityInfo)this.m_aCode[num];
					CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
					this.paintOneProduct(g, i, num2, codeTable.sName, commodityInfo.commodityCode);
					num++;
					num2++;
				}
				if (num >= this.m_aCode.Count)
				{
					return;
				}
			}
		}
		private void paintOneProduct(Graphics g, int iRow, int iCol, string name, string code)
		{
			int num = this.m_rc.X + iCol * this.m_iWidth;
			int num2 = this.m_rc.Y + this.iTitleHeight + iRow * (this.font.Height + this.GAP);
			this.m_Brush.Color = SetInfo.RHColor.clProductName;
			string text = name + " " + code + "  ";
			if (name.Equals(code))
			{
				text = code;
			}
			Font font = this.font;
			int num3 = (int)this.font.Size;
			while (g.MeasureString(text, this.font).Width > (float)this.m_iWidth)
			{
				num3--;
				this.font = new Font("宋体", (float)num3, FontStyle.Regular);
			}
			g.DrawString(text, this.font, this.m_Brush, (float)(num + this.m_iWidth - (int)g.MeasureString(text, this.font).Width), (float)(num2 + this.GAP / 2));
			this.font = font;
		}
		private void paintHighlight(int newRow, int newCol)
		{
			Graphics m_Graphics = this.m_hqForm.M_Graphics;
			if (this.m_iHighlightRow != -1)
			{
				this.paintCurHighlight(m_Graphics, this.m_iHighlightRow, this.m_iHighlightCol);
			}
			if (newRow != -1 && (this.m_iHighlightRow != newRow || this.m_iHighlightCol != newCol))
			{
				this.paintCurHighlight(m_Graphics, newRow, newCol);
				this.m_iHighlightRow = newRow;
				this.m_iHighlightCol = newCol;
			}
		}
		private void paintCurHighlight(Graphics g, int iRow, int iCol)
		{
			int x = this.m_rc.X + iCol * this.m_iWidth;
			int y = this.m_rc.Y + this.iTitleHeight + iRow * (this.font.Height + this.GAP);
			GDIDraw.XorRectangle(g, new Rectangle(x, y, this.m_iWidth, this.font.Height + this.GAP), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
		}
		private void paintPromptMessage(Graphics g)
		{
			string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_HistoryPrompt");
			int num = (int)g.MeasureString(@string, this.font).Width;
			int num2 = num / (this.m_rc.Width - 8);
			this.m_Brush.Color = SetInfo.RHColor.clProductName;
			if (num % (this.m_rc.Width - 8) > 0)
			{
				num2++;
			}
			int num3 = (this.m_rc.Height - this.font.Height * num2 - 20) / 2 + 20;
			int i = 0;
			int num4 = (this.m_rc.Width - 8) / 16;
			while (i < @string.Length)
			{
				int num5 = i + num4;
				string text;
				if (num5 > @string.Length)
				{
					text = @string.Substring(i);
					i = @string.Length;
				}
				else
				{
					text = @string.Substring(i, num5);
					i = num5;
				}
				int num6 = (this.m_rc.Width - 8 - (int)g.MeasureString(text, this.font).Width) / 2 + 4;
				g.DrawString(text, this.font, this.m_Brush, (float)num6, (float)num3);
				num3 += this.font.Height;
			}
		}
		protected override void Page_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.selectProduct(e.X, e.Y);
			}
			((HQClientForm)this.m_hqForm).mainWindow.Focus();
		}
		private bool selectProduct(int x, int y)
		{
			if (this.m_iWidth == 0)
			{
				return false;
			}
			int num = (x - this.m_rc.X) / this.m_iWidth;
			int num2 = (y - this.m_rc.Y - this.iTitleHeight) / (this.font.Height + this.GAP);
			if (num >= this.m_iCols || num2 >= this.m_iRows)
			{
				return false;
			}
			if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + num2 * this.m_iCols + num >= this.m_aCode.Count)
			{
				return false;
			}
			if (num2 == this.m_iHighlightRow && num == this.m_iHighlightCol)
			{
				return true;
			}
			this.paintHighlight(num2, num);
			return true;
		}
		protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!this.selectProduct(e.X, e.Y))
			{
				return;
			}
			int num = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
			if (num < this.m_aCode.Count)
			{
				this.m_hqForm.ShowPageKLine((CommodityInfo)this.m_aCode[num]);
			}
		}
		protected override void Page_MouseMove(object sender, MouseEventArgs e)
		{
		}
		protected override void Page_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				bool flag = false;
				Keys keyData = e.KeyData;
				if (keyData != Keys.Return)
				{
					switch (keyData)
					{
					case Keys.Prior:
						if (this.m_iCurPage > 0)
						{
							this.m_iCurPage--;
							this.m_iHighlightRow = (this.m_iHighlightCol = 0);
							flag = true;
						}
						break;
					case Keys.Next:
						if (this.m_iCurPage < this.m_iTotalPage - 1)
						{
							this.m_iCurPage++;
							this.m_iHighlightRow = (this.m_iHighlightCol = 0);
							flag = true;
						}
						break;
					case Keys.Left:
						if (this.m_iHighlightCol > 0)
						{
							this.paintHighlight(this.m_iHighlightRow, this.m_iHighlightCol - 1);
						}
						else
						{
							if (this.m_iHighlightRow > 0)
							{
								this.paintHighlight(this.m_iHighlightRow - 1, this.m_iCols - 1);
							}
							else
							{
								if (this.m_iCurPage > 0)
								{
									this.m_iCurPage--;
									this.m_iHighlightRow = this.m_iRows - 1;
									this.m_iHighlightCol = this.m_iCols - 1;
									flag = true;
								}
							}
						}
						break;
					case Keys.Up:
						if (this.m_iHighlightRow > 0)
						{
							this.paintHighlight(this.m_iHighlightRow - 1, this.m_iHighlightCol);
						}
						else
						{
							if (this.m_iCurPage == 0)
							{
								flag = false;
							}
							else
							{
								this.m_iCurPage--;
								this.m_iHighlightRow = this.m_iRows - 1;
								flag = true;
							}
						}
						break;
					case Keys.Right:
						if (this.m_iHighlightCol < this.m_iCols - 1)
						{
							if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol < this.m_aCode.Count - 1)
							{
								this.paintHighlight(this.m_iHighlightRow, this.m_iHighlightCol + 1);
							}
						}
						else
						{
							if (this.m_iHighlightRow < this.m_iRows - 1)
							{
								if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + (this.m_iHighlightRow + 1) * this.m_iCols < this.m_aCode.Count)
								{
									this.paintHighlight(this.m_iHighlightRow + 1, 0);
								}
							}
							else
							{
								if (this.m_iCurPage < this.m_iTotalPage - 1)
								{
									this.m_iCurPage++;
									this.m_iHighlightRow = (this.m_iHighlightCol = 0);
									flag = true;
								}
							}
						}
						break;
					case Keys.Down:
						if (this.m_iHighlightRow == this.m_iRows - 1)
						{
							if (this.m_iCurPage < this.m_iTotalPage - 1)
							{
								this.m_iCurPage++;
								this.m_iHighlightRow = 0;
								if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightCol > this.m_aCode.Count - 1)
								{
									this.m_iHighlightCol = this.m_aCode.Count - this.m_iCurPage * (this.m_iRows * this.m_iCols) - 1;
								}
								flag = true;
							}
						}
						else
						{
							if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + (this.m_iHighlightRow + 1) * this.m_iCols + this.m_iHighlightCol <= this.m_aCode.Count - 1)
							{
								this.paintHighlight(this.m_iHighlightRow + 1, this.m_iHighlightCol);
							}
						}
						break;
					}
				}
				else
				{
					int num = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
					if (num < this.m_aCode.Count)
					{
						this.m_hqForm.ShowPageKLine((CommodityInfo)this.m_aCode[num]);
					}
				}
				if (flag)
				{
					this.m_hqForm.Repaint();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_History-Page_KeyDown异常：" + ex.Message);
			}
		}
		private void MakeMenus()
		{
			this.contextMenuStrip.Items.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
			toolStripMenuItem.Name = "cmd_60";
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
			toolStripMenuItem2.Name = "cmd_80";
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis") + "  F5", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
			toolStripMenuItem3.Name = "kline";
			this.contextMenuStrip.Items.Add(toolStripMenuItem3);
			this.contextMenuStrip.Items.Add("-");
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			this.contextMenuStrip.Items.Add(toolStripMenuItem2);
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
				else
				{
					if (name.Equals("kline"))
					{
						int num = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
						if (num < this.m_aCode.Count)
						{
							this.m_hqForm.ShowPageKLine((CommodityInfo)this.m_aCode[num]);
						}
					}
					else
					{
						this.m_hqForm.UserCommand(name);
					}
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_History-contextMenu_ItemClicked异常：" + ex.Message);
			}
		}
		public override void Dispose()
		{
			GC.Collect();
		}
	}
}
