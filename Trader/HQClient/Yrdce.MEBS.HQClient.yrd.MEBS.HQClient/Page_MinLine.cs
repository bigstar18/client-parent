using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_MinLine : Page_Main
	{
		public int iProductType;
		private Draw_MinLine draw_MinLine;
		private Draw_Quote draw_Quote;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private ButtonUtils buttonUtils;
		private Rectangle rcMinLine;
		private Rectangle rcQuote;
		private int rcQuoteWidth = 220;
		private int m_iQuoteH = 380;
		protected override void AskForDataOnTimer()
		{
		}
		private void AskForDataOnce()
		{
			if (this.m_hqClient.listData.Count != this.m_hqClient.rectCol * this.m_hqClient.rectRol || !this.m_hqForm.IsMultiCommidity)
			{
				this.m_hqClient.listData = this.getAfterGoodsInfo(this.m_hqClient.rectCol * this.m_hqClient.rectRol);
			}
			else
			{
				ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
				if (productData == null)
				{
					productData = new ProductData();
					productData.commodityInfo = this.m_hqClient.curCommodityInfo;
					if (productData.commodityInfo == null)
					{
						return;
					}
					this.m_hqClient.aProductData.Insert(0, productData);
				}
				this.m_hqClient.listData[this.m_globalData.SelectNumMin - 1] = productData;
			}
			CMDMinVO cMDMinVO = new CMDMinVO();
			List<CommidityVO> list = new List<CommidityVO>();
			for (int i = 0; i < this.m_hqClient.listData.Count; i++)
			{
				list.Add(new CommidityVO
				{
					code = this.m_hqClient.listData[i].commodityInfo.commodityCode,
					marketID = this.m_hqClient.listData[i].commodityInfo.marketID,
					location = (byte)(i + 1)
				});
			}
			if (this.m_hqForm.IsMultiCommidity)
			{
				cMDMinVO.mark = (byte)(this.m_hqClient.rectCol * this.m_hqClient.rectRol);
			}
			else
			{
				cMDMinVO.mark = 1;
			}
			cMDMinVO.type = 0;
			cMDMinVO.date = 0;
			cMDMinVO.time = 0;
			cMDMinVO.commidityList = list;
			this.m_hqClient.sendThread.AskForData(cMDMinVO);
			CMDBillByVersionVO cMDBillByVersionVO = new CMDBillByVersionVO();
			cMDBillByVersionVO.marketID = this.m_hqClient.curCommodityInfo.marketID;
			cMDBillByVersionVO.type = 1;
			cMDBillByVersionVO.code = this.m_hqClient.curCommodityInfo.commodityCode;
			cMDMinVO.date = 0;
			cMDBillByVersionVO.time = (long)(this.m_rc.Height / 16);
			this.m_hqClient.sendThread.AskForData(cMDBillByVersionVO);
		}
		public Page_MinLine(Rectangle _rc, HQForm hqForm) : base(_rc, hqForm)
		{
			try
			{
				this.pluginInfo = this.m_pluginInfo;
				this.setInfo = this.m_setInfo;
				this.buttonUtils = hqForm.CurHQClient.buttonUtils;
				if (this.m_hqClient.isNeedAskData)
				{
					this.AskForDataOnce();
				}
				this.m_hqClient.CurrentPage = 1;
				this.draw_MinLine = new Draw_MinLine(hqForm, true);
				this.MakeMenus();
				this.iProductType = this.m_hqClient.getProductType(this.m_hqClient.curCommodityInfo);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine异常：" + ex.Message);
			}
		}
		public override void Paint(Graphics g, int v)
		{
			try
			{
				ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
				Font font = new Font("楷体_GB2312", 26f, FontStyle.Bold);
				this.m_iQuoteH = this.m_rc.Height;
				Font font2 = new Font("宋体", 12f, FontStyle.Regular);
				font.Dispose();
				font2.Dispose();
				this.rcQuote = new Rectangle(this.m_rc.X + this.m_rc.Width - this.rcQuoteWidth, this.m_rc.Y, this.rcQuoteWidth, this.m_iQuoteH);
				this.draw_Quote = new Draw_Quote(this.m_hqClient);
				this.draw_Quote.Paint(g, this.rcQuote, productData, this.m_hqClient.curCommodityInfo, this.m_hqClient.iShowBuySellPrice, this.m_hqClient);
				this.rcMinLine = new Rectangle(this.m_rc.X, this.m_rc.Y, this.m_rc.Width - this.rcQuoteWidth - 10, this.m_rc.Height);
				if (this.m_hqForm.IsMultiCommidity)
				{
					if (this.m_hqClient.listRectInfo == null)
					{
						this.m_hqClient.listRectInfo = new List<clickRect>();
					}
					if (this.m_hqClient.listRectInfo.Count != this.m_hqClient.rectCol * this.m_hqClient.rectRol)
					{
						this.m_hqClient.listRectInfo.Clear();
						List<ProductData> afterGoodsInfo = this.getAfterGoodsInfo(this.m_hqClient.rectCol * this.m_hqClient.rectRol);
						int num = this.rcMinLine.Width / this.m_hqClient.rectCol;
						int num2 = this.rcMinLine.Height / this.m_hqClient.rectRol;
						int num3 = 0;
						for (int i = 0; i < this.m_hqClient.rectRol; i++)
						{
							for (int j = 0; j < this.m_hqClient.rectCol; j++)
							{
								int k;
								for (k = num3; k >= afterGoodsInfo.Count; k -= afterGoodsInfo.Count)
								{
								}
								clickRect clickRect = new clickRect();
								Rectangle rectangle = default(Rectangle);
								Draw_MinLine draw_MinLine = new Draw_MinLine(this.m_hqForm, true);
								rectangle.X = this.rcMinLine.X + j * num;
								rectangle.Y = this.rcMinLine.Y + i * num2;
								rectangle.Height = num2;
								rectangle.Width = num;
								clickRect.OwnRect = rectangle;
								clickRect.OwnMinKLine = draw_MinLine;
								clickRect.MinData = this.m_hqClient.GetProductData(afterGoodsInfo[k].commodityInfo);
								if (clickRect.MinData != null)
								{
									this.m_hqClient.listRectInfo.Add(clickRect);
								}
								draw_MinLine.Paint(g, rectangle, clickRect.MinData);
								Pen pen = new Pen(SetInfo.RHColor.clGrid, 1f);
								if (clickRect.MinData == productData)
								{
									pen.Color = Color.BurlyWood;
								}
								g.DrawRectangle(pen, new Rectangle(rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 2, rectangle.Height - 2));
								num3++;
							}
						}
					}
					else
					{
						int num4 = this.rcMinLine.Width / this.m_hqClient.rectCol;
						int num5 = this.rcMinLine.Height / this.m_hqClient.rectRol;
						int num6 = 0;
						for (int l = 0; l < this.m_hqClient.rectRol; l++)
						{
							for (int m = 0; m < this.m_hqClient.rectCol; m++)
							{
								Rectangle rectangle2 = default(Rectangle);
								rectangle2.X = this.rcMinLine.X + m * num4;
								rectangle2.Y = this.rcMinLine.Y + l * num5;
								rectangle2.Height = num5;
								rectangle2.Width = num4;
								this.m_hqClient.listRectInfo[num6].OwnRect = rectangle2;
								if (this.m_globalData.SelectNumMin - 1 != num6)
								{
									this.m_hqClient.listRectInfo[num6].MinData = this.m_hqClient.GetProductData(this.m_hqClient.listData[num6].commodityInfo);
									this.m_hqClient.listRectInfo[num6].OwnMinKLine.Paint(g, rectangle2, this.m_hqClient.listRectInfo[num6].MinData);
									g.DrawRectangle(new Pen(SetInfo.RHColor.clGrid, 1f), new Rectangle(rectangle2.X + 1, rectangle2.Y + 1, rectangle2.Width - 2, rectangle2.Height - 2));
								}
								else
								{
									this.m_hqClient.listRectInfo[num6].MinData = productData;
									if (productData != null)
									{
										this.m_hqClient.listData[num6] = productData;
									}
									this.m_hqClient.listRectInfo[num6].OwnMinKLine.Paint(g, rectangle2, productData);
									g.DrawRectangle(new Pen(Color.BurlyWood, 1f), new Rectangle(rectangle2.X + 1, rectangle2.Y + 1, rectangle2.Width - 2, rectangle2.Height - 2));
								}
								num6++;
							}
						}
					}
					if (this.draw_MinLine.isDrawCursor)
					{
						for (int n = 0; n < this.m_hqClient.listRectInfo.Count; n++)
						{
							if (this.m_hqClient.listRectInfo[n].OwnMinKLine.m_iPos >= 0)
							{
								this.m_hqClient.listRectInfo[n].OwnMinKLine.DrawLabel(g);
							}
						}
					}
					this.m_hqForm.EndPaint();
					for (int num7 = 0; num7 < this.m_hqClient.listRectInfo.Count; num7++)
					{
						if (this.m_hqClient.listRectInfo[num7].OwnMinKLine.m_iPos >= 0)
						{
							if (this.draw_MinLine.isDrawCursor)
							{
								this.m_hqClient.listRectInfo[num7].OwnMinKLine.m_YSrcBmp = null;
								this.m_hqClient.listRectInfo[num7].OwnMinKLine.DrawCursor(-1);
							}
							else
							{
								this.m_hqClient.listRectInfo[num7].OwnMinKLine.m_iPos = -1;
							}
						}
					}
				}
				else
				{
					this.draw_MinLine.Paint(g, this.rcMinLine, productData);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-Paint异常：" + ex.Message);
			}
		}
		private List<ProductData> getAfterGoodsInfo(int afterNum)
		{
			List<ProductData> list = new List<ProductData>();
			int num = -1;
			for (int i = 0; i < this.m_multiQuoteData.m_curQuoteList.Length; i++)
			{
				if (this.m_hqClient.curCommodityInfo.commodityCode.Equals(this.m_multiQuoteData.m_curQuoteList[i].code) && this.m_hqClient.curCommodityInfo.marketID.Equals(this.m_multiQuoteData.m_curQuoteList[i].marketID))
				{
					num = i;
					break;
				}
			}
			if (num == -1 && this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				num = 0;
			}
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				for (int j = num; j < num + afterNum; j++)
				{
					ProductData productData = new ProductData();
					int k;
					for (k = j; k >= this.m_multiQuoteData.m_curQuoteList.Length; k -= this.m_multiQuoteData.m_curQuoteList.Length)
					{
					}
					string marketID = this.m_multiQuoteData.m_curQuoteList[k].marketID;
					string code = this.m_multiQuoteData.m_curQuoteList[k].code;
					productData.commodityInfo = new CommodityInfo(marketID, code);
					list.Add(productData);
				}
			}
			return list;
		}
		protected override void Page_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				int x = e.X - this.m_hqForm.ScrollOffset.X;
				int num = e.Y - this.m_hqForm.ScrollOffset.Y;
				if (e.Button == MouseButtons.Left)
				{
					if (this.draw_Quote == null)
					{
						return;
					}
					if (num > this.rcQuote.Y + this.rcQuote.Height - this.draw_Quote.rcRightButton.Height && num < this.draw_Quote.rcRightButton.Y + this.draw_Quote.rcRightButton.Height)
					{
						this.ClickButton(x, num);
					}
					if (this.rcQuote.Contains(e.X, e.Y))
					{
						this.draw_Quote.MouseLeftClick(e.X, e.Y, this.m_hqForm, this.m_hqClient);
					}
					else
					{
						if (this.m_hqForm.IsMultiCommidity)
						{
							int i = 0;
							while (i < this.m_hqClient.listRectInfo.Count)
							{
								if (this.m_hqClient.listRectInfo[i].OwnRect.Contains(e.X, e.Y))
								{
									if (this.m_globalData.SelectNumMin != i + 1)
									{
										this.m_globalData.SelectNumMin = i + 1;
										for (int j = 0; j < this.m_hqClient.listRectInfo.Count; j++)
										{
											if (j != this.m_globalData.SelectNumMin - 1)
											{
												this.m_hqClient.listRectInfo[j].OwnMinKLine.m_iPos = -1;
											}
										}
										if (this.m_hqClient.listRectInfo[i].MinData != null)
										{
											this.m_hqClient.curCommodityInfo = this.m_hqClient.listRectInfo[i].MinData.commodityInfo;
											this.m_hqForm.Repaint();
										}
									}
									if (i >= this.m_hqClient.listRectInfo.Count)
									{
										return;
									}
									bool flag = this.m_hqClient.listRectInfo[i].OwnMinKLine.MouseDoubleClick(e.X, e.Y);
									if (flag)
									{
										this.m_hqForm.Repaint();
										break;
									}
									break;
								}
								else
								{
									i++;
								}
							}
						}
						else
						{
							bool flag2 = this.draw_MinLine.MouseDoubleClick(e.X, e.Y);
							if (flag2)
							{
								this.m_hqForm.Repaint();
							}
						}
					}
				}
				((HQClientForm)this.m_hqForm).mainWindow.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseClick异常：" + ex.Message);
			}
		}
		private void ClickButton(int x, int y)
		{
			MyButton myButton = this.draw_Quote.rightbuttonGraph.MouseLeftClicked(x, y, this.buttonUtils.RightButtonList, false);
			if (myButton != null)
			{
				this.buttonUtils.CuRightrButtonName = myButton.Name;
				this.m_hqForm.Repaint();
			}
		}
		protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.rcQuote.Contains(e.X, e.Y))
				{
					this.draw_Quote.MouseDoubleClick(e.X, e.Y, this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo), this.m_hqForm, this.m_hqClient);
				}
				else
				{
					if (this.rcMinLine.Contains(e.X, e.Y) && this.draw_MinLine != null && this.m_hqClient.isClickMultiMarket)
					{
						if (this.m_hqForm.IsMultiCommidity)
						{
							for (int i = 0; i < this.m_hqClient.listRectInfo.Count; i++)
							{
								if (this.m_hqClient.listRectInfo[i].OwnRect.Contains(e.X, e.Y))
								{
									this.m_globalData.SelectNumMin = i + 1;
									if (this.m_hqClient.listRectInfo[i].MinData == null)
									{
										return;
									}
									this.m_hqClient.curCommodityInfo = this.m_hqClient.listRectInfo[i].MinData.commodityInfo;
								}
							}
						}
						this.m_hqForm.IsMultiCommidity = !this.m_hqForm.IsMultiCommidity;
						this.m_hqForm.Repaint();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseDoubleClick异常：" + ex.Message);
			}
		}
		protected override void Page_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.rcMinLine.Contains(e.X, e.Y) && this.draw_MinLine != null)
				{
					if (this.m_hqForm.IsMultiCommidity)
					{
						for (int i = 0; i < this.m_hqClient.listRectInfo.Count; i++)
						{
							if (this.m_hqClient.listRectInfo[i].OwnRect.Contains(e.X, e.Y))
							{
								bool flag = this.m_hqClient.listRectInfo[i].OwnMinKLine.MouseDragged(e.X, e.Y);
								if (flag)
								{
									this.m_hqForm.Repaint();
								}
							}
							else
							{
								if (this.m_hqClient.listRectInfo[i].OwnMinKLine.m_iPos != -1)
								{
									this.m_hqClient.listRectInfo[i].OwnMinKLine.m_iPos = -1;
									this.m_hqForm.Repaint();
								}
							}
						}
					}
					else
					{
						bool flag2 = this.draw_MinLine.MouseDragged(e.X, e.Y);
						if (flag2)
						{
							this.m_hqForm.Repaint();
						}
					}
				}
				else
				{
					if (this.rcQuote.Contains(e.X, e.Y))
					{
						this.draw_Quote.MouseMove(e.X, e.Y, this.m_hqForm);
					}
					else
					{
						if (this.m_hqForm.M_Cursor == Cursors.Hand)
						{
							this.m_hqForm.M_Cursor = Cursors.Default;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseMove异常：" + ex.Message);
			}
		}
		protected override void Page_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				bool isNeedRepaint = false;
				Keys keyData = e.KeyData;
				switch (keyData)
				{
				case Keys.Prior:
					this.m_hqForm.ChangeStock(true);
					isNeedRepaint = true;
					break;
				case Keys.Next:
					this.m_hqForm.ChangeStock(false);
					isNeedRepaint = true;
					break;
				default:
					if (keyData != Keys.F10)
					{
						if (this.m_hqForm.IsMultiCommidity)
						{
							isNeedRepaint = this.m_hqClient.listRectInfo[this.m_globalData.SelectNumMin - 1].OwnMinKLine.KeyPressed(e);
						}
						else
						{
							isNeedRepaint = this.draw_MinLine.KeyPressed(e);
						}
					}
					else
					{
						this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
					}
					break;
				}
				this.m_hqForm.IsNeedRepaint = isNeedRepaint;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-Page_KeyDown异常：" + ex.Message);
			}
		}
		private void MakeMenus()
		{
			this.contextMenuStrip.Items.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_PrevCommodity") + "  PageUp", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_PrevStock"));
			toolStripMenuItem.Name = "prevstock";
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_NextCommodity") + "  PageDown", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_PostStock"));
			toolStripMenuItem2.Name = "poststock";
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis") + "  F5", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
			toolStripMenuItem3.Name = "kline";
			ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_TradeList") + "  F1", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Bill"));
			toolStripMenuItem4.Name = "bill";
			ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
			toolStripMenuItem5.Name = "cmd_60";
			ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
			toolStripMenuItem6.Name = "cmd_80";
			ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_multiMarket"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Cycle"));
			toolStripMenuItem7.Name = "multiMarket";
			ToolStripMenuItem toolStripMenuItem8 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_oneMarket"));
			toolStripMenuItem8.Name = "oneMarket";
			ToolStripMenuItem toolStripMenuItem9 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_twoMarket"));
			toolStripMenuItem9.Name = "twoMarket";
			ToolStripMenuItem toolStripMenuItem10 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_threeMarket"));
			toolStripMenuItem10.Name = "threeMarket";
			ToolStripMenuItem toolStripMenuItem11 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_fourMarket"));
			toolStripMenuItem11.Name = "fourMarket";
			ToolStripMenuItem toolStripMenuItem12 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_sixMarket"));
			toolStripMenuItem12.Name = "sixMarket";
			ToolStripMenuItem toolStripMenuItem13 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_nineMarket"));
			toolStripMenuItem13.Name = "nineMarket";
			ToolStripMenuItem toolStripMenuItem14 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_CommodityInfo") + "  F10");
			toolStripMenuItem14.Name = "commodityInfo";
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Items.Add(toolStripMenuItem8);
			contextMenuStrip.Items.Add(toolStripMenuItem9);
			contextMenuStrip.Items.Add(toolStripMenuItem10);
			contextMenuStrip.Items.Add(toolStripMenuItem11);
			contextMenuStrip.Items.Add(toolStripMenuItem12);
			contextMenuStrip.Items.Add(toolStripMenuItem13);
			contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
			toolStripMenuItem7.DropDown = contextMenuStrip;
			this.contextMenuStrip.Items.Add(toolStripMenuItem7);
			this.contextMenuStrip.Items.Add(toolStripMenuItem3);
			if (this.iProductType != 2 && this.iProductType != 3)
			{
				this.contextMenuStrip.Items.Add(toolStripMenuItem4);
			}
			this.contextMenuStrip.Items.Add("-");
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			this.contextMenuStrip.Items.Add(toolStripMenuItem2);
			this.contextMenuStrip.Items.Add("-");
			this.contextMenuStrip.Items.Add(toolStripMenuItem5);
			this.contextMenuStrip.Items.Add(toolStripMenuItem6);
			if (this.m_hqForm.isDisplayF10Menu())
			{
				this.contextMenuStrip.Items.Add(toolStripMenuItem14);
			}
			base.AddCommonMenu();
		}
		protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				CommodityInfoF.CommodityInfoClose();
				string name = e.ClickedItem.Name;
				if (name.IndexOf("cmd_") >= 0)
				{
					this.m_hqForm.UserCommand(name.Substring(4));
				}
				else
				{
					if (name.Equals("kline"))
					{
						this.m_hqForm.ShowPageKLine(this.m_hqClient.curCommodityInfo);
					}
					else
					{
						if (name.Equals("bill"))
						{
							this.m_hqForm.UserCommand("01");
						}
						else
						{
							if (name.Equals("prevstock"))
							{
								this.m_hqForm.ChangeStock(true);
							}
							else
							{
								if (name.Equals("poststock"))
								{
									this.m_hqForm.ChangeStock(false);
								}
								else
								{
									if (name.Equals("commodityInfo"))
									{
										this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
									}
									else
									{
										if (name.Equals("oneMarket"))
										{
											this.m_hqClient.isClickMultiMarket = false;
											this.m_hqForm.IsMultiCommidity = false;
										}
										else
										{
											if (name.Equals("twoMarket"))
											{
												this.m_hqClient.isClickMultiMarket = true;
												this.m_hqForm.IsMultiCommidity = true;
												this.m_hqClient.rectCol = 2;
												this.m_hqClient.rectRol = 1;
												this.m_hqForm.ShowPageMinLine();
											}
											else
											{
												if (name.Equals("threeMarket"))
												{
													this.m_hqClient.isClickMultiMarket = true;
													this.m_hqForm.IsMultiCommidity = true;
													this.m_hqClient.rectCol = 3;
													this.m_hqClient.rectRol = 1;
													this.m_hqForm.ShowPageMinLine();
												}
												else
												{
													if (name.Equals("fourMarket"))
													{
														this.m_hqClient.isClickMultiMarket = true;
														this.m_hqForm.IsMultiCommidity = true;
														this.m_hqClient.rectCol = 2;
														this.m_hqClient.rectRol = 2;
														this.m_hqForm.ShowPageMinLine();
													}
													else
													{
														if (name.Equals("sixMarket"))
														{
															this.m_hqClient.isClickMultiMarket = true;
															this.m_hqForm.IsMultiCommidity = true;
															this.m_hqClient.rectCol = 3;
															this.m_hqClient.rectRol = 2;
															this.m_hqForm.ShowPageMinLine();
														}
														else
														{
															if (name.Equals("nineMarket"))
															{
																this.m_hqClient.isClickMultiMarket = true;
																this.m_hqForm.IsMultiCommidity = true;
																this.m_hqClient.rectCol = 3;
																this.m_hqClient.rectRol = 3;
																this.m_hqForm.ShowPageMinLine();
															}
															else
															{
																this.m_hqForm.UserCommand(name);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_MinLine-contextMenu_ItemClicked异常：" + ex.Message);
			}
		}
		public override void Dispose()
		{
			GC.Collect();
		}
	}
}
