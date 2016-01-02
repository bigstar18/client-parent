using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_MultiQuote : Page_Main
	{
		private const int TITLE_GAP = 2;
		private Font fontTitle = new Font("宋体", 12f, FontStyle.Regular);
		private Font font = new Font("宋体", 12f, FontStyle.Regular);
		private string strSortItem = "Code";
		private int iStockRows;
		private int iStockCols;
		private int fontHeight;
		private bool bCanMove = true;
		private byte sortBy;
		private byte isDescend;
		private Rectangle rcButton;
		private Rectangle rcData;
		private Page_Button buttonGraph;
		private int iEnd = 30;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private ButtonUtils buttonUtils;
		public string[] m_strItems;
		private int iDynamicIndex;
		private int m_iStaticIndex;
		private int xOnceMove = 3;
		public int xScrollWidth = 100;
		private int scrollButtonSize = 15;
		private Rectangle xScrollRect;
		private Rectangle xScrollBarRect;
		private int xScrollBarWidth = 50;
		private int xChange;
		private int xMaxChaneg;
		private double yOnceMove = 10.0;
		private Rectangle yScrollRect;
		private Rectangle yScrollBarRect;
		private int yScrollBarHeight;
		private int yMaxChaneg;
		private Color colorBlack = Color.FromArgb(80, 80, 80);
		private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
		private int i;
		private bool m_bShowSortTag;
		private int dataGap = 20;
		private int needShowCol = 4;
		private int scrolXOrY;
		private ToolStripMenuItem DelUserCommodity;
		private ToolStripMenuItem AddUserCommodity;
		private int lbLocationY = 35;
		private int lbHeight = 25;
		private void setSortStockRequestPacket()
		{
			CMDSortVO cMDSortVO = new CMDSortVO();
			cMDSortVO.isDescend = this.isDescend;
			cMDSortVO.sortBy = this.sortBy;
			if (this.m_multiQuoteData.iStart == this.iEnd)
			{
				this.iEnd = this.m_multiQuoteData.iStart + 1;
			}
			cMDSortVO.start = this.m_multiQuoteData.iStart;
			cMDSortVO.end = this.iEnd;
			Logger.wirte(1, string.Concat(new object[]
			{
				"取报价排名Start = ",
				this.m_multiQuoteData.iStart,
				"  End = ",
				this.iEnd
			}));
			this.m_hqClient.sendThread.AskForData(cMDSortVO);
		}
		protected override void AskForDataOnTimer()
		{
			int arg_0D_0 = this.m_hqClient.m_bShowIndexAtBottom;
		}
		private void AskForQuoteList()
		{
		}
		public void TransferCommodityInfo()
		{
			try
			{
				if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
				{
					ProductDataVO commodityInfo = (ProductDataVO)this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].Clone();
					InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityInfo);
					this.m_hqForm.MultiQuoteMouseLeftClick(this, e);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace);
			}
		}
		public Page_MultiQuote(Rectangle _rc, HQForm hqForm) : base(_rc, hqForm)
		{
			try
			{
				this.pluginInfo = this.m_pluginInfo;
				this.setInfo = this.m_setInfo;
				this.buttonUtils = hqForm.CurHQClient.buttonUtils;
				bool flag = Tools.StrToBool((string)this.m_pluginInfo.HTConfig["FontB"], false);
				if (flag)
				{
					this.font = this.fontTitle;
				}
				this.fontHeight = this.font.Height;
				this.AskForQuoteList();
				this.m_hqClient.CurrentPage = 0;
				this.initStockFieldInfo();
				this.buttonGraph = new Page_Button(this.rcButton, this.m_hqForm, this.buttonUtils);
				this.MakeMenus();
				Thread thread = new Thread(new ThreadStart(this.checkCommCurprice));
				thread.Start();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote异常：" + ex.Message);
			}
		}
		private void checkCommCurprice()
		{
			while (!this.stopFlag)
			{
				Thread.Sleep(1000);
				if (this.m_hqClient.CurrentPage != 0)
				{
					return;
				}
				lock (this.m_multiQuoteData.m_curQuoteList)
				{
					if (this.m_multiQuoteData.m_curQuoteList.Length != 0)
					{
						int num = this.iStockRows;
						if (this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart < this.iStockRows)
						{
							num = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
						}
						for (int i = 0; i < num; i++)
						{
							int num2 = this.m_multiQuoteData.iStart + i;
							if (num2 >= this.m_multiQuoteData.m_curQuoteList.Length)
							{
								num2 = this.m_multiQuoteData.m_curQuoteList.Length - 1;
							}
							ProductDataVO productDataVO = this.m_multiQuoteData.m_curQuoteList[num2];
							if (productDataVO.datahighlightTime == 0 && productDataVO.isDraw)
							{
								GDIDraw.XorRectangle(this.m_hqForm.M_Graphics, productDataVO.curPriceRectangle, SetInfo.RHColor.clPriceChange, this.m_hqForm.ScrollOffset);
								productDataVO.isDraw = false;
							}
							else if (productDataVO.datahighlightTime == this.m_multiQuoteData.HighlightTime && !productDataVO.isDraw)
							{
								GDIDraw.XorRectangle(this.m_hqForm.M_Graphics, productDataVO.curPriceRectangle, SetInfo.RHColor.clPriceChange, this.m_hqForm.ScrollOffset);
								productDataVO.isDraw = true;
							}
							if (productDataVO.datahighlightTime > 0)
							{
								productDataVO.datahighlightTime--;
							}
						}
					}
				}
			}
		}
		private void initStockFieldInfo()
		{
			string multiQuoteName = this.setInfo.MultiQuoteName;
			string[] array = multiQuoteName.Split(new char[]
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
					MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[array2[0]];
					if (multiQuoteItemInfo != null)
					{
						multiQuoteItemInfo.name = array2[1];
					}
				}
			}
			if (!Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
			{
				this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Industry;", "");
				this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Region;", "");
				this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("MarketName;", "");
			}
			this.setInfo.MultiQuoteItems = this.setInfo.MultiQuoteItems.Replace("Unit;", "");
			string text = this.setInfo.MultiQuoteItems;
			if (text.Length == 0)
			{
				text = this.m_hqClient.strAllItemName;
			}
			this.m_strItems = text.Split(new char[]
			{
				';'
			});
			this.m_iStaticIndex = this.setInfo.MultiQuoteStaticIndex;
			this.iDynamicIndex = this.m_iStaticIndex + 1;
		}
		private void calculateRowsOfPage()
		{
			this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
			int num = 2;
			this.iStockRows = (this.rcData.Height - this.fontTitle.Height) / (this.fontHeight + num);
			if (this.iStockRows < 1)
			{
				this.iStockRows = 25;
			}
			this.iEnd = this.m_multiQuoteData.iStart + this.iStockRows - 1;
			if (this.m_multiQuoteData.iHighlightIndex > this.iStockRows)
			{
				this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
			}
		}
		private void calculateColsOfPage()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.m_strItems.Length; i++)
			{
				MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[i]];
				if ((i <= this.m_iStaticIndex || i >= this.iDynamicIndex) && multiQuoteItemInfo != null)
				{
					num2 += multiQuoteItemInfo.width;
					if (this.rcData.Width <= num2)
					{
						break;
					}
					num++;
				}
			}
			this.iStockCols = num - 1;
		}
		private void paintXScroll(Graphics g)
		{
			this.xMaxChaneg = this.xOnceMove * (this.m_strItems.Length - this.needShowCol - 1);
			this.xScrollWidth = this.xOnceMove * (this.m_strItems.Length - this.needShowCol - 1) + this.xScrollBarWidth + 2 * this.scrollButtonSize;
			this.xScrollRect = new Rectangle(this.rcButton.X + this.rcButton.Width - this.xScrollWidth, this.rcButton.Y - 1, this.xScrollWidth - 1, this.scrollButtonSize);
			this.xScrollBarRect = new Rectangle(this.xScrollRect.X + this.scrollButtonSize + this.xChange, this.xScrollRect.Y, this.xScrollBarWidth, this.xScrollRect.Height);
			g.FillRectangle(this.m_Brush, this.xScrollRect);
			g.FillRectangle(new SolidBrush(this.colorBlack), this.xScrollBarRect);
		}
		private void paintYScroll(Graphics g)
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length <= this.iStockRows)
			{
				Rectangle rectangle = default(Rectangle);
				this.yScrollBarRect = rectangle;
				this.yScrollRect = (this.yScrollBarRect = rectangle);
				return;
			}
			int num = (int)(this.yOnceMove * (double)this.m_multiQuoteData.iStart);
			if (this.m_multiQuoteData.yChange > num + 20 || this.m_multiQuoteData.yChange < num - 20)
			{
				this.m_multiQuoteData.yChange = num;
			}
			this.yScrollRect = new Rectangle(this.rcData.X + this.rcData.Width, this.rcData.Y + this.fontHeight + 1, this.scrollButtonSize - 1, this.rcData.Height - this.fontHeight - 3);
			this.yScrollBarHeight = (this.yScrollRect.Height - 2 * this.scrollButtonSize) * this.iStockRows / this.m_multiQuoteData.m_curQuoteList.Length;
			this.yScrollBarRect = new Rectangle(this.yScrollRect.X, this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange, this.scrollButtonSize, this.yScrollBarHeight);
			this.yOnceMove = (double)(Convert.ToSingle(this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarRect.Height) / (float)(this.m_multiQuoteData.m_curQuoteList.Length - this.iStockRows));
			this.yMaxChaneg = this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarRect.Height;
			if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
			{
				this.m_multiQuoteData.yChange = this.yMaxChaneg;
				this.yScrollBarRect = new Rectangle(this.yScrollRect.X, this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange, this.scrollButtonSize, this.yScrollBarHeight);
			}
			g.FillRectangle(this.m_Brush, this.yScrollRect);
			g.FillRectangle(new SolidBrush(this.colorBlack), this.yScrollBarRect);
		}
		private void rePaintXScrollBar()
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			if (this.xChange < 0)
			{
				this.xChange = 0;
			}
			if (this.xChange > this.xMaxChaneg)
			{
				this.xChange = this.xMaxChaneg;
			}
			this.xScrollBarRect.X = this.xScrollRect.X + this.scrollButtonSize + this.xChange;
			using (Graphics m_Graphics = this.m_hqForm.M_Graphics)
			{
				this.m_hqForm.TranslateTransform(m_Graphics);
				using (Bitmap bitmap = new Bitmap(this.xScrollRect.Width, this.xScrollRect.Height))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillRectangle(new SolidBrush(this.colorBlack), this.xScrollBarRect);
						m_Graphics.DrawImage(bitmap, this.xScrollRect.X, this.xScrollRect.Y);
					}
				}
			}
		}
		private void rePaintYScrollBar()
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			if (this.m_multiQuoteData.yChange < 0)
			{
				this.m_multiQuoteData.yChange = 0;
			}
			if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
			{
				this.m_multiQuoteData.yChange = this.yMaxChaneg;
			}
			this.yScrollBarRect.Y = this.yScrollRect.Y + this.scrollButtonSize + this.m_multiQuoteData.yChange;
			using (Graphics m_Graphics = this.m_hqForm.M_Graphics)
			{
				this.m_hqForm.TranslateTransform(m_Graphics);
				using (Bitmap bitmap = new Bitmap(this.yScrollRect.Width, this.yScrollRect.Height))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillRectangle(new SolidBrush(this.colorBlack), this.yScrollBarRect);
						m_Graphics.DrawImage(bitmap, this.yScrollRect.X, this.yScrollRect.Y);
					}
				}
			}
		}
		private void paintScrollBack(Graphics g)
		{
			this.m_Brush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
			if (this.xScrollRect.Location.X + this.xScrollRect.Width != this.m_rc.Width)
			{
				this.paintXScroll(g);
			}
			g.DrawString("<", this.font, new SolidBrush(Color.Yellow), this.xScrollRect.Location);
			g.DrawString(">", this.font, new SolidBrush(Color.Yellow), new Point(this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize, this.xScrollRect.Y));
			if (this.m_multiQuoteData.m_curQuoteList.Length > this.iStockRows)
			{
				Font font = new Font("宋体", 8f, FontStyle.Regular);
				this.rcData.Width = this.rcData.Width - this.scrollButtonSize;
				this.paintYScroll(g);
				g.DrawString("∧", font, new SolidBrush(Color.Yellow), this.yScrollRect.Location);
				g.DrawString("∨", font, new SolidBrush(Color.Yellow), new Point(this.yScrollRect.X, this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize));
			}
		}
		public override void Paint(Graphics g, int v)
		{
			try
			{
				this.rcData = this.m_rc;
				this.rcButton = this.m_rc;
				this.rcData.Height = this.rcData.Height - this.m_multiQuoteData.buttonHight;
				this.rcButton.Y = this.rcData.Y + this.rcData.Height;
				this.rcButton.Height = this.m_multiQuoteData.buttonHight;
				this.buttonGraph.rc = this.rcButton;
				this.i++;
				if (this.buttonUtils.ButtonList != null && this.buttonUtils.ButtonList.Count > 0)
				{
					this.buttonGraph.Paint(g, this.buttonUtils.ButtonList, true);
				}
				this.calculateRowsOfPage();
				this.calculateColsOfPage();
				this.paintTitleItems(g);
				this.paintQuoteData(g);
				this.paintGrid(g);
				this.paintScrollBack(g);
				this.m_hqForm.EndPaint();
				this.paintHighlightBar();
				this.paintXScroll(g);
				this.paintYScroll(g);
				this.bCanMove = true;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
			}
		}
		private void paintGrid(Graphics g)
		{
			Pen pen = new Pen(new SolidBrush(SetInfo.RHColor.clGrid), 1f);
			g.DrawRectangle(pen, this.m_rc.X, this.m_rc.Y, this.rcData.Width - 1, this.m_rc.Height - 1);
			g.DrawLine(pen, this.rcData.X, this.rcData.Y + this.fontTitle.Height, this.rcData.X + this.rcData.Width, this.rcData.Y + this.fontTitle.Height);
		}
		private void paintTitleItems(Graphics g)
		{
			int num = this.rcData.X;
			int num2 = this.rcData.Y + 1;
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clMultiQuote_TitleBack);
			g.FillRectangle(solidBrush, new Rectangle(this.rcData.X, this.rcData.Y, this.rcData.Width, this.fontTitle.Height));
			for (int i = 0; i < this.m_strItems.Length; i++)
			{
				if (i <= this.m_iStaticIndex || i >= this.iDynamicIndex)
				{
					MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[i]];
					if (multiQuoteItemInfo != null)
					{
						if (i > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1) && i != this.m_strItems.Length)
						{
							g.DrawString(">>", this.font, solidBrush, (float)num, (float)num2);
							break;
						}
						string text = multiQuoteItemInfo.name;
						if (this.m_bShowSortTag && this.strSortItem.Equals(this.m_strItems[i]))
						{
							if (this.isDescend == 1)
							{
								solidBrush.Color = SetInfo.RHColor.clIncrease;
								text += "↓";
							}
							else
							{
								solidBrush.Color = SetInfo.RHColor.clDecrease;
								text += "↑";
							}
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clItem;
						}
						int num3 = (int)g.MeasureString(text, this.fontTitle).Width;
						if (multiQuoteItemInfo.width < num3)
						{
							multiQuoteItemInfo.width = num3 + 10;
						}
						num += multiQuoteItemInfo.width;
						g.DrawString(text, this.fontTitle, solidBrush, (float)(num - num3), (float)num2);
						if (num > this.rcData.X + this.rcData.Width)
						{
							break;
						}
					}
				}
			}
			solidBrush.Dispose();
		}
		public bool zoom(string strText, Graphics g, MultiQuoteItemInfo info, int y)
		{
			int num = (int)g.MeasureString(strText, this.font).Width;
			if (info.width - num < this.dataGap)
			{
				info.width += num - info.width + this.dataGap;
				this.m_Brush.Color = SetInfo.RHColor.clBackGround;
				g.FillRectangle(this.m_Brush, this.rcData.X, this.rcData.Y, this.rcData.Width, y - this.rcData.Y);
				this.paintTitleItems(g);
				this.paintQuoteData(g);
				return true;
			}
			return false;
		}
		private void paintQuoteData(Graphics g)
		{
			lock (this.m_multiQuoteData.m_curQuoteList)
			{
				if (this.buttonUtils.CurButtonName == "AllMarket")
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < this.m_hqClient.m_quoteList.Length; i++)
					{
						ProductDataVO value = this.m_hqClient.m_quoteList[i];
						arrayList.Add(value);
					}
					this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList.ToArray(typeof(ProductDataVO));
					this.m_multiQuoteData.MultiQuotePage = 0;
				}
				else if (this.buttonUtils.CurButtonName.StartsWith("Market"))
				{
					string value2 = this.buttonUtils.CurButtonName.Substring(6);
					ArrayList arrayList2 = new ArrayList();
					for (int j = 0; j < this.m_hqClient.m_quoteList.Length; j++)
					{
						if (this.m_hqClient.m_quoteList[j].marketID.Equals(value2))
						{
							ProductDataVO value3 = this.m_hqClient.m_quoteList[j];
							arrayList2.Add(value3);
						}
					}
					this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList2.ToArray(typeof(ProductDataVO));
					this.m_multiQuoteData.MultiQuotePage = 0;
				}
				else if (this.buttonUtils.CurButtonName.Equals("MyCommodity"))
				{
					ArrayList arrayList3 = new ArrayList();
					this.m_multiQuoteData.MyCommodityList.Clear();
					ArrayList myCommodity = this.m_hqClient.myCommodity.GetMyCommodity();
					for (int k = 0; k < myCommodity.Count; k++)
					{
						CommodityInfo commodityInfo = CommodityInfo.DealCode(myCommodity[k].ToString());
						for (int l = 0; l < this.m_hqClient.m_quoteList.Length; l++)
						{
							if (this.m_hqClient.m_quoteList[l].marketID.Equals(commodityInfo.marketID) && this.m_hqClient.m_quoteList[l].code.Equals(commodityInfo.commodityCode))
							{
								ProductDataVO value4 = this.m_hqClient.m_quoteList[l];
								arrayList3.Add(value4);
								this.m_multiQuoteData.MyCommodityList.Add(myCommodity[k].ToString());
								break;
							}
						}
					}
					this.m_multiQuoteData.MultiQuotePage = 1;
					this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList3.ToArray(typeof(ProductDataVO));
				}
				else if (this.buttonUtils.CurButtonName.StartsWith("MAC"))
				{
					if (this.m_hqClient.commodityClass == null)
					{
						return;
					}
					string text = this.buttonUtils.CurButtonName.Substring(3);
					string text2 = string.Empty;
					string text3 = string.Empty;
					int num = text.IndexOf("_");
					if (num != -1)
					{
						text2 = text.Substring(0, num);
						text3 = text.Substring(num + 1);
					}
					else
					{
						text2 = text;
					}
					if (text2 != null && text2.Length > 0)
					{
						ArrayList arrayList4 = new ArrayList();
						for (int m = 0; m < this.m_hqClient.m_quoteList.Length; m++)
						{
							if (this.m_hqClient.m_quoteList[m].marketID.Equals(text2))
							{
								ProductDataVO value5 = this.m_hqClient.m_quoteList[m];
								arrayList4.Add(value5);
							}
						}
						this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList4.ToArray(typeof(ProductDataVO));
					}
					if (this.m_multiQuoteData.m_curQuoteList.Length > 0 && text3 != null && text3.Length > 0)
					{
						if (this.m_hqClient.commodityClass == null)
						{
							return;
						}
						ArrayList arrayList5 = (ArrayList)this.m_hqClient.commodityClass.htCommodityByClass[text3];
						if (arrayList5 == null)
						{
							return;
						}
						ArrayList arrayList6 = new ArrayList();
						for (int n = 0; n < this.m_multiQuoteData.m_curQuoteList.Length; n++)
						{
							for (int num2 = 0; num2 < arrayList5.Count; num2++)
							{
								CommodityClassVO commodityClassVO = (CommodityClassVO)arrayList5[num2];
								if (this.m_multiQuoteData.m_curQuoteList[n].code.Equals(commodityClassVO.commodityID))
								{
									arrayList6.Add(this.m_multiQuoteData.m_curQuoteList[n]);
									break;
								}
							}
						}
						this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList6.ToArray(typeof(ProductDataVO));
					}
					this.m_multiQuoteData.MultiQuotePage = 0;
				}
				else if (this.buttonUtils.CurButtonName.StartsWith("C"))
				{
					string text4 = this.buttonUtils.CurButtonName.Substring(1);
					if (text4 != null && text4.Length > 0)
					{
						if (this.m_hqClient.commodityClass == null)
						{
							return;
						}
						ArrayList arrayList7 = (ArrayList)this.m_hqClient.commodityClass.htCommodityByClass[text4];
						if (arrayList7 == null)
						{
							return;
						}
						ArrayList arrayList8 = new ArrayList();
						for (int num3 = 0; num3 < this.m_hqClient.m_quoteList.Length; num3++)
						{
							for (int num4 = 0; num4 < arrayList7.Count; num4++)
							{
								CommodityClassVO commodityClassVO2 = (CommodityClassVO)arrayList7[num4];
								if (this.m_hqClient.m_quoteList[num3].marketID.Equals(commodityClassVO2.market) && this.m_hqClient.m_quoteList[num3].code.Equals(commodityClassVO2.commodityID))
								{
									arrayList8.Add(this.m_hqClient.m_quoteList[num3]);
									break;
								}
							}
						}
						this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList8.ToArray(typeof(ProductDataVO));
						this.m_multiQuoteData.MultiQuotePage = 0;
					}
				}
				else if (this.buttonUtils.CurButtonName.StartsWith("Select"))
				{
					ArrayList arrayList9 = new ArrayList();
					for (int num5 = 0; num5 < this.m_hqClient.m_quoteList.Length; num5++)
					{
						ProductDataVO productDataVO = this.m_hqClient.m_quoteList[num5];
						if (MainWindow.selectIndexHY != -1 && MainWindow.selectIndexDQ != -1)
						{
							if (MainWindow.hangYeStrings[MainWindow.selectIndexHY] == productDataVO.industry && MainWindow.diQuStrings[MainWindow.selectIndexDQ] == productDataVO.region)
							{
								arrayList9.Add(productDataVO);
							}
						}
						else if (MainWindow.selectIndexHY == -1 && MainWindow.selectIndexDQ == -1)
						{
							arrayList9.Add(productDataVO);
						}
						else if (MainWindow.selectIndexHY == -1)
						{
							if (MainWindow.diQuStrings[MainWindow.selectIndexDQ] == productDataVO.region)
							{
								arrayList9.Add(productDataVO);
							}
						}
						else if (MainWindow.selectIndexDQ == -1 && MainWindow.hangYeStrings[MainWindow.selectIndexHY] == productDataVO.industry)
						{
							arrayList9.Add(productDataVO);
						}
					}
					this.m_multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList9.ToArray(typeof(ProductDataVO));
				}
				if (this.m_multiQuoteData.m_curQuoteList.Length != 0)
				{
					this.sortItems();
					int num6 = 2;
					int num7 = this.rcData.X;
					int num8 = this.rcData.Y + this.fontHeight + 2;
					int num9 = this.iStockRows;
					if (this.m_multiQuoteData.m_curQuoteList.Length < this.iStockRows)
					{
						this.m_multiQuoteData.iStart = 0;
					}
					if (this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart < this.iStockRows)
					{
						num9 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
					}
					SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
					for (int num10 = 0; num10 < num9; num10++)
					{
						int num11 = this.m_multiQuoteData.iStart + num10;
						ProductDataVO productDataVO2 = this.m_multiQuoteData.m_curQuoteList[num11];
						if (productDataVO2.code == null)
						{
							Logger.wirte(3, "Code = null");
						}
						else
						{
							CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[productDataVO2.marketID + productDataVO2.code];
							string text5 = "-";
							if (codeTable != null)
							{
								if (codeTable.sName != null)
								{
									text5 = codeTable.sName;
								}
								else
								{
									Logger.wirte(1, "stockTable.sName = null");
								}
							}
							else
							{
								Logger.wirte(1, " stockTable = null ");
							}
							CommodityInfo commodityInfo2 = new CommodityInfo(productDataVO2.marketID, productDataVO2.code);
							int precision = this.m_hqClient.GetPrecision(commodityInfo2);
							float yesterBalancePrice = productDataVO2.yesterBalancePrice;
							int num12 = 0;
							for (int num13 = 0; num13 < this.m_strItems.Length; num13++)
							{
								if (num13 <= this.m_iStaticIndex || num13 >= this.iDynamicIndex)
								{
									if (num13 > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
									{
										break;
									}
									MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[num13]];
									if (multiQuoteItemInfo != null)
									{
										num7 += multiQuoteItemInfo.width;
										if (this.m_strItems[num13].Equals("No"))
										{
											string text6 = Convert.ToString(num11 + 1);
											solidBrush.Color = SetInfo.RHColor.clNumber;
											num12 = (int)g.MeasureString(text6, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text6, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										else if (this.m_strItems[num13].Equals("Name"))
										{
											string text6 = text5;
											solidBrush.Color = SetInfo.RHColor.clProductName;
											num12 = (int)g.MeasureString(text6, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text6, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										else if (this.m_strItems[num13].Equals("Code"))
										{
											string text6 = productDataVO2.code;
											solidBrush.Color = SetInfo.RHColor.clProductName;
											num12 = (int)g.MeasureString(text6, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text6, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										else if (this.m_strItems[num13].Equals("CurPrice"))
										{
											Rectangle curPriceRectangle = new Rectangle(num7 - multiQuoteItemInfo.width, num8, multiQuoteItemInfo.width, this.fontHeight);
											productDataVO2.curPriceRectangle = curPriceRectangle;
											productDataVO2.isDraw = false;
											if (this.zoom(productDataVO2.curPrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.curPrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("Balance"))
										{
											if (this.zoom(productDataVO2.balancePrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.balancePrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("UpValue"))
										{
											float num14 = (yesterBalancePrice != 0f && productDataVO2.curPrice != 0f) ? (productDataVO2.curPrice - yesterBalancePrice) : 0f;
											if (this.zoom(num14.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)num14, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("UpRate"))
										{
											float num15 = (yesterBalancePrice > 0f && productDataVO2.curPrice > 0f) ? ((productDataVO2.curPrice - yesterBalancePrice) / yesterBalancePrice * 100f) : 0f;
											if (this.zoom(num15.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)num15, "", this.m_strItems[num13], 2, 0f, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("YesterBalance"))
										{
											if (this.zoom(productDataVO2.yesterBalancePrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.yesterBalancePrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("OpenPrice"))
										{
											if (this.zoom(productDataVO2.openPrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.openPrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("BuyPrice"))
										{
											if (this.zoom(productDataVO2.buyPrice[0].ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.buyPrice[0], "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("SellPrice"))
										{
											if (this.zoom(productDataVO2.sellPrice[0].ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.sellPrice[0], "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("BuyAmount"))
										{
											if (this.zoom(productDataVO2.buyAmount[0].ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.buyAmount[0], "", this.m_strItems[num13], 0, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("SellAmount"))
										{
											if (this.zoom(productDataVO2.sellAmount[0].ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.sellAmount[0], "", this.m_strItems[num13], 0, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("HighPrice"))
										{
											if (this.zoom(productDataVO2.highPrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.highPrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("LowPrice"))
										{
											if (this.zoom(productDataVO2.lowPrice.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.lowPrice, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("TotalAmount"))
										{
											if (this.zoom(productDataVO2.totalAmount.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.totalAmount, "", this.m_strItems[num13], 0, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("TotalMoney"))
										{
											if (this.zoom(productDataVO2.totalMoney.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, productDataVO2.totalMoney, "", this.m_strItems[num13], precision, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("ReverseCount"))
										{
											if (this.zoom(productDataVO2.reserveCount.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.reserveCount, "", this.m_strItems[num13], 0, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("CurAmount"))
										{
											if (this.zoom(productDataVO2.curAmount.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.curAmount, "", this.m_strItems[num13], 0, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("AmountRate"))
										{
											if (this.zoom(productDataVO2.amountRate.ToString("0.00"), g, multiQuoteItemInfo, num8))
											{
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.amountRate, "", this.m_strItems[num13], 2, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("ConsignRate"))
										{
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											this.paintNumber(g, solidBrush, (double)productDataVO2.consignRate, "", this.m_strItems[num13], 2, yesterBalancePrice, num7, num8);
										}
										else if (this.m_strItems[num13].Equals("Region"))
										{
											solidBrush.Color = Color.White;
											string text7 = "—";
											if (this.m_hqClient.m_htRegion.Count != 0 && productDataVO2.region != null && this.m_hqClient.m_htRegion[productDataVO2.region] != null)
											{
												text7 = this.m_hqClient.m_htRegion[productDataVO2.region].ToString();
											}
											else if (productDataVO2.region != null)
											{
												text7 = productDataVO2.region;
											}
											num12 = (int)g.MeasureString(text7, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text7, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										else if (this.m_strItems[num13].Equals("Industry"))
										{
											solidBrush.Color = Color.White;
											string text8 = "—";
											if (this.m_hqClient.m_htIndustry.Count != 0 && productDataVO2.industry != null && this.m_hqClient.m_htIndustry[productDataVO2.industry] != null)
											{
												text8 = this.m_hqClient.m_htIndustry[productDataVO2.industry].ToString();
											}
											else if (productDataVO2.industry != null)
											{
												text8 = productDataVO2.industry;
											}
											num12 = (int)g.MeasureString(text8, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text8, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										else if (this.m_strItems[num13].Equals("MarketName"))
										{
											string text9 = "--";
											if (this.m_hqClient.m_htMarketData != null)
											{
												foreach (DictionaryEntry dictionaryEntry in this.m_hqClient.m_htMarketData)
												{
													MarketDataVO marketDataVO = (MarketDataVO)dictionaryEntry.Value;
													if (marketDataVO.marketID == productDataVO2.marketID)
													{
														text9 = marketDataVO.marketName;
													}
												}
											}
											solidBrush.Color = SetInfo.RHColor.clProductName;
											num12 = (int)g.MeasureString(text9, this.font).Width;
											if (multiQuoteItemInfo.width - num12 < this.dataGap)
											{
												multiQuoteItemInfo.width += num12 - multiQuoteItemInfo.width + this.dataGap;
												solidBrush.Color = SetInfo.RHColor.clBackGround;
												g.FillRectangle(solidBrush, this.rcData.X, this.rcData.Y, this.rcData.Width, num8 - this.rcData.Y);
												this.paintTitleItems(g);
												this.paintQuoteData(g);
												return;
											}
											g.DrawString(text9, this.font, solidBrush, (float)(num7 - num12), (float)num8);
										}
										if (num7 > this.rcData.X + this.rcData.Width)
										{
											break;
										}
									}
								}
							}
							num7 = this.rcData.X;
							num8 += this.fontHeight + num6;
						}
					}
					solidBrush.Dispose();
				}
			}
		}
		private void paintHighlightBar()
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0 && this.m_multiQuoteData.iHighlightIndex > this.m_multiQuoteData.m_curQuoteList.Length)
			{
				this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length;
				if (this.m_multiQuoteData.iHighlightIndex < 0)
				{
					this.m_multiQuoteData.iHighlightIndex = 1;
				}
			}
			else if (this.m_multiQuoteData.m_curQuoteList.Length == 0)
			{
				this.m_multiQuoteData.iHighlightIndex = 1;
			}
			Graphics m_Graphics = this.m_hqForm.M_Graphics;
			int y = this.rcData.Y + (this.m_multiQuoteData.iHighlightIndex - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1;
			GDIDraw.XorRectangle(m_Graphics, new Rectangle(this.rcData.X, y, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
			try
			{
				if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
				{
					CommodityInfo curCommodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].code);
					this.m_hqClient.curCommodityInfo = curCommodityInfo;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
		}
		private void repaintHighlightBar(int iNewPos)
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			Graphics m_Graphics = this.m_hqForm.M_Graphics;
			int y = this.rcData.Y + (this.m_multiQuoteData.iHighlightIndex - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1;
			int y2 = this.rcData.Y + (iNewPos - 1) * (this.fontHeight + 2) + this.fontTitle.Height + 2 - 1;
			GDIDraw.XorRectangle(m_Graphics, new Rectangle(this.rcData.X, y, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
			GDIDraw.XorRectangle(m_Graphics, new Rectangle(this.rcData.X, y2, this.rcData.Width, this.fontHeight), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
			this.m_multiQuoteData.iHighlightIndex = iNewPos;
			try
			{
				if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
				{
					CommodityInfo curCommodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1].code);
					this.m_hqClient.curCommodityInfo = curCommodityInfo;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
		}
		private void paintNumber(Graphics g, SolidBrush m_Brush, double num, string strSuffix, string itemName, int iPrecision, float close, int x, int y)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (itemName.Equals("TotalAmount") || itemName.Equals("CurAmount") || itemName.Equals("BuyAmount") || itemName.Equals("SellAmount") || itemName.Equals("AmountRate"))
			{
				m_Brush.Color = SetInfo.RHColor.clVolume;
			}
			else if (itemName.Equals("ReverseCount"))
			{
				m_Brush.Color = SetInfo.RHColor.clReserve;
			}
			else if (itemName.Equals("TotalMoney"))
			{
				m_Brush.Color = SetInfo.RHColor.clNumber;
			}
			else if (itemName.Equals("ConsignRate"))
			{
				m_Brush.Color = SetInfo.RHColor.clNumber;
			}
			else if (itemName.Equals("YesterBalance"))
			{
				m_Brush.Color = SetInfo.RHColor.clEqual;
			}
			else if (itemName.Equals("UpValue"))
			{
				if (num > 0.0)
				{
					stringBuilder.Append("+");
					m_Brush.Color = SetInfo.RHColor.clIncrease;
				}
				else if (num == 0.0)
				{
					m_Brush.Color = SetInfo.RHColor.clEqual;
				}
				else
				{
					m_Brush.Color = SetInfo.RHColor.clDecrease;
				}
			}
			else if (num > (double)close)
			{
				m_Brush.Color = SetInfo.RHColor.clIncrease;
			}
			else if (num == (double)close || num == 0.0)
			{
				m_Brush.Color = SetInfo.RHColor.clEqual;
			}
			else
			{
				m_Brush.Color = SetInfo.RHColor.clDecrease;
			}
			if (itemName.Equals("UpRate"))
			{
				if (num == -100.0 || num == 0.0)
				{
					stringBuilder.Append("—");
				}
				else
				{
					stringBuilder.Append(M_Common.FloatToString(num, iPrecision));
					stringBuilder.Append("%");
				}
			}
			else if (num == 0.0)
			{
				stringBuilder.Append("—");
			}
			else
			{
				stringBuilder.Append(M_Common.FloatToString(num, iPrecision));
			}
			stringBuilder.Append(strSuffix);
			int num2 = (int)g.MeasureString(stringBuilder.ToString(), this.font).Width;
			float arg_259_0 = g.MeasureString("代码", this.font).Width;
			g.DrawString(stringBuilder.ToString(), this.font, m_Brush, (float)(x - num2), (float)y);
		}
		public void sortItems()
		{
			string sortRules = (string)this.pluginInfo.HTConfig["SortRules00"];
			Arrays.sort(this.m_multiQuoteData.m_curQuoteList, this.strSortItem, sortRules);
			if (this.isDescend == 0)
			{
				int num = this.m_multiQuoteData.m_curQuoteList.Length;
				int num2 = num / 2;
				for (int i = 0; i < num2; i++)
				{
					ProductDataVO productDataVO = this.m_multiQuoteData.m_curQuoteList[i];
					this.m_multiQuoteData.m_curQuoteList[i] = this.m_multiQuoteData.m_curQuoteList[num - i - 1];
					this.m_multiQuoteData.m_curQuoteList[num - i - 1] = productDataVO;
				}
			}
		}
		protected override void Page_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				int num = e.X - this.m_hqForm.ScrollOffset.X;
				int num2 = e.Y - this.m_hqForm.ScrollOffset.Y;
				if (e.Button == MouseButtons.Left)
				{
					bool flag = false;
					if (this.xScrollRect.Contains(num, num2) && !this.xScrollBarRect.Contains(num, num2))
					{
						if (num < this.xScrollRect.X + this.scrollButtonSize)
						{
							flag = this.Key_LEFT_Pressed(1);
						}
						else if (num > this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize)
						{
							flag = this.Key_RIGHT_Pressed(1);
						}
						else
						{
							int num3 = num - this.xScrollRect.X - this.scrollButtonSize;
							if (num3 < this.xScrollBarWidth / 2)
							{
								this.xChange = 0;
							}
							else if (num3 > this.xScrollRect.Width - 2 * this.scrollButtonSize - this.xScrollBarWidth / 2)
							{
								this.xChange = this.xMaxChaneg;
							}
							else
							{
								this.xChange = num3 - this.xScrollBarWidth / 2;
							}
							flag = true;
							this.iDynamicIndex = this.xChange / this.xOnceMove + this.needShowCol;
						}
					}
					else if (this.yScrollRect.Contains(num, num2) && !this.yScrollBarRect.Contains(num, num2))
					{
						if (num2 < this.yScrollRect.Y + this.scrollButtonSize)
						{
							flag = this.Key_UP_Pressed();
						}
						else if (num2 > this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize)
						{
							flag = this.Key_DOWN_Pressed();
						}
						else
						{
							int num4 = num2 - this.yScrollRect.Y - this.scrollButtonSize;
							if (num4 < this.yScrollBarHeight / 2)
							{
								this.m_multiQuoteData.yChange = 0;
							}
							else if (num4 > this.yScrollRect.Height - 2 * this.scrollButtonSize - this.yScrollBarHeight / 2)
							{
								this.m_multiQuoteData.yChange = this.yMaxChaneg;
							}
							else
							{
								this.m_multiQuoteData.yChange = num4 - this.yScrollBarHeight / 2;
							}
							flag = true;
							this.m_multiQuoteData.iStart = (int)((double)this.m_multiQuoteData.yChange / this.yOnceMove);
						}
					}
					if (flag)
					{
						this.m_hqForm.Repaint();
					}
					if (num2 > this.fontHeight && num2 < this.rcData.Y + this.rcData.Height)
					{
						this.selectProduct(num, num2);
						this.TransferCommodityInfo();
					}
					else if (num2 > this.rcData.Y + this.rcData.Height && num2 < this.rcButton.Y + this.rcButton.Height)
					{
						this.ClickButton(num, num2);
					}
					else
					{
						int num5 = this.rcData.X;
						for (int i = 0; i < this.m_strItems.Length; i++)
						{
							if (i <= this.m_iStaticIndex || i >= this.iDynamicIndex)
							{
								MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[i]];
								if (multiQuoteItemInfo != null)
								{
									if (i > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
									{
										return;
									}
									if (num > num5 && num < num5 + multiQuoteItemInfo.width)
									{
										this.changeSortField(this.m_strItems[i]);
										break;
									}
									num5 += multiQuoteItemInfo.width;
									if (num5 > this.rcData.X + this.rcData.Width)
									{
										break;
									}
								}
							}
						}
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (num2 > this.rcData.Y + this.rcData.Height && num2 < this.rcButton.Y + this.rcButton.Height)
					{
						this.ClickButtonRight(num, num2);
					}
					if (num2 > this.fontHeight && num2 < this.rcData.Y + this.rcData.Height)
					{
						this.selectProduct(num, num2);
					}
				}
				((HQClientForm)this.m_hqForm).mainWindow.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-Page_MouseClick异常：" + ex.Message);
			}
		}
		protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				int num = e.X - this.m_hqForm.ScrollOffset.X;
				int num2 = e.Y - this.m_hqForm.ScrollOffset.Y;
				if (e.Button == MouseButtons.Left && this.m_multiQuoteData.m_curQuoteList.Length > 0)
				{
					int x = this.rcData.X;
					int num3 = this.rcData.Y + this.fontTitle.Height;
					int num4 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
					if (num4 > this.iStockRows)
					{
						num4 = this.iStockRows;
					}
					for (int i = 0; i < num4; i++)
					{
						if (num > x && num < x + this.m_rc.Width - this.yScrollRect.Width && num2 > num3 && num2 < num3 + this.fontHeight)
						{
							CommodityInfo commodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + i].marketID, this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + i].code);
							this.m_hqForm.QueryStock(commodityInfo);
							this.m_hqForm.Repaint();
							break;
						}
						num3 += this.fontHeight + 2;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-Page_MouseDoubleClick异常：" + ex.Message);
			}
		}
		protected override void Page_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				int num = e.X - this.m_hqForm.ScrollOffset.X;
				int num2 = e.Y - this.m_hqForm.ScrollOffset.Y;
				HQClientForm hQClientForm = (HQClientForm)this.m_hqForm;
				if (hQClientForm.m_isMouseLeftButtonDown)
				{
					if (this.xScrollBarRect.Contains(hQClientForm.m_mouseBeforeMove))
					{
						this.scrolXOrY = 1;
						if (this.xScrollBarRect.X + this.xScrollBarRect.Width > this.xScrollRect.X + this.xScrollRect.Width - this.scrollButtonSize)
						{
							this.xChange = this.xMaxChaneg;
						}
						else if (this.xScrollBarRect.X < this.xScrollRect.X + this.scrollButtonSize)
						{
							this.xChange = 0;
						}
						if (e.Location.X - hQClientForm.m_mouseBeforeMove.X != 0)
						{
							this.xChange += e.Location.X - hQClientForm.m_mouseBeforeMove.X;
							if (this.xChange < 0)
							{
								this.xChange = 0;
							}
							this.rePaintXScrollBar();
						}
						if (this.iDynamicIndex != Math.Ceiling(this.xChange / this.xOnceMove + this.needShowCol))
						{
							if (Math.Ceiling(this.xChange / this.xOnceMove + this.needShowCol) <= this.m_iStaticIndex + 1)
							{
								this.iDynamicIndex = this.m_iStaticIndex + 1;
							}
							else
							{
								this.iDynamicIndex = this.xChange / this.xOnceMove + this.needShowCol;
							}
							this.m_hqForm.Repaint();
						}
					}
					else if (this.yScrollBarRect.Contains(hQClientForm.m_mouseBeforeMove))
					{
						this.scrolXOrY = 2;
						if (this.yScrollBarRect.Y + this.yScrollBarRect.Height > this.yScrollRect.Y + this.yScrollRect.Height - this.scrollButtonSize)
						{
							this.m_multiQuoteData.yChange = this.yMaxChaneg;
						}
						else if (this.yScrollBarRect.Y < this.yScrollRect.Y + this.scrollButtonSize)
						{
							this.m_multiQuoteData.yChange = 0;
						}
						if (e.Location.Y - hQClientForm.m_mouseBeforeMove.Y != 0)
						{
							this.m_multiQuoteData.yChange += e.Location.Y - hQClientForm.m_mouseBeforeMove.Y;
							hQClientForm.m_mouseBeforeMove.Y = e.Location.Y;
							this.rePaintYScrollBar();
						}
						if ((double)this.m_multiQuoteData.iStart != (double)this.m_multiQuoteData.yChange / this.yOnceMove)
						{
							this.m_multiQuoteData.iStart = (int)((double)this.m_multiQuoteData.yChange / this.yOnceMove);
							if (this.m_multiQuoteData.iStart <= 0)
							{
								this.m_multiQuoteData.iStart = 0;
							}
							this.m_hqForm.Repaint();
						}
					}
					if (this.scrolXOrY == 1)
					{
						hQClientForm.m_mouseBeforeMove.X = e.Location.X;
					}
					else if (this.scrolXOrY == 2)
					{
						hQClientForm.m_mouseBeforeMove.Y = e.Location.Y;
					}
				}
				if (num2 <= 0 || num2 >= this.fontHeight)
				{
					this.m_hqForm.M_Cursor = Cursors.Default;
				}
				else
				{
					int num3 = this.rcData.X;
					for (int i = 0; i < this.m_strItems.Length; i++)
					{
						if (i <= this.m_iStaticIndex || i >= this.iDynamicIndex)
						{
							MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[i]];
							if (multiQuoteItemInfo != null)
							{
								if (i > this.iStockCols + (this.iDynamicIndex - this.m_iStaticIndex - 1))
								{
									return;
								}
								if (num > num3 && num < num3 + multiQuoteItemInfo.width && multiQuoteItemInfo.sortID == -1)
								{
									this.m_hqForm.M_Cursor = Cursors.Default;
									return;
								}
								if (num > num3 && num < num3 + multiQuoteItemInfo.width && multiQuoteItemInfo.sortID != -1)
								{
									this.m_hqForm.M_Cursor = Cursors.Hand;
									return;
								}
								num3 += multiQuoteItemInfo.width;
								if (num3 > this.rcData.X + this.rcData.Width)
								{
									break;
								}
							}
						}
					}
					this.m_hqForm.M_Cursor = Cursors.Default;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-Page_MouseMove异常：" + ex.Message);
			}
		}
		protected override void Page_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				bool isNeedRepaint = false;
				Keys keyData = e.KeyData;
				if (keyData != Keys.Return)
				{
					switch (keyData)
					{
					case Keys.Prior:
						isNeedRepaint = this.Key_PAGEUP_Pressed();
						break;
					case Keys.Next:
						isNeedRepaint = this.Key_PAGEDOWN_Pressed();
						break;
					case Keys.End:
					case Keys.Home:
						break;
					case Keys.Left:
						isNeedRepaint = this.Key_LEFT_Pressed(1);
						break;
					case Keys.Up:
						isNeedRepaint = this.Key_UP_Pressed();
						break;
					case Keys.Right:
						isNeedRepaint = this.Key_RIGHT_Pressed(1);
						break;
					case Keys.Down:
						isNeedRepaint = this.Key_DOWN_Pressed();
						break;
					default:
						if (keyData == Keys.F10)
						{
							isNeedRepaint = this.Key_F10_Pressed();
						}
						break;
					}
				}
				else
				{
					isNeedRepaint = this.Key_ENTER_Pressed();
				}
				this.m_hqForm.IsNeedRepaint = isNeedRepaint;
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-Page_KeyDown异常：" + ex.Message);
			}
		}
		private void MakeMenus()
		{
			this.contextMenuStrip.Items.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_SortBy"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_StockRank"));
			toolStripMenuItem.Name = "SortBy";
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
			toolStripMenuItem2.Name = "cmd_80";
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
			toolStripMenuItem3.Name = "minline";
			ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
			toolStripMenuItem4.Name = "kline";
			ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_CommodityInfo") + "  F10");
			toolStripMenuItem5.Name = "commodityInfo";
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			for (int i = 0; i < this.m_strItems.Length; i++)
			{
				MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[this.m_strItems[i]];
				if (multiQuoteItemInfo != null && multiQuoteItemInfo.sortID != -1)
				{
					ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(multiQuoteItemInfo.name);
					toolStripMenuItem6.Name = "Sort_" + this.m_strItems[i];
					contextMenuStrip.Items.Add(toolStripMenuItem6);
				}
			}
			contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
			toolStripMenuItem.DropDown = contextMenuStrip;
			this.AddUserCommodity = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_AddUserCommodity"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_AddCustom"));
			this.AddUserCommodity.Name = "AddUserCommodity";
			this.AddUserCommodity.Visible = !this.buttonUtils.CurButtonName.Equals("MyCommodity");
			this.DelUserCommodity = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_DelUserCommodity"), (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_DelCustom"));
			this.DelUserCommodity.Name = "DelUserCommodity";
			this.DelUserCommodity.Visible = this.buttonUtils.CurButtonName.Equals("MyCommodity");
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			this.contextMenuStrip.Items.Add("-");
			this.contextMenuStrip.Items.Add(toolStripMenuItem2);
			this.contextMenuStrip.Items.Add(toolStripMenuItem3);
			this.contextMenuStrip.Items.Add(toolStripMenuItem4);
			if (this.m_hqForm.isDisplayF10Menu())
			{
				this.contextMenuStrip.Items.Add(toolStripMenuItem5);
			}
			this.contextMenuStrip.Items.Add(this.AddUserCommodity);
			this.contextMenuStrip.Items.Add(this.DelUserCommodity);
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
				else if (name.IndexOf("Sort_") >= 0)
				{
					this.changeSortField(name.Substring(5));
				}
				else if (name.Equals("minline"))
				{
					if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						ProductDataVO productDataVO = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
						CommodityInfo commodityInfo = new CommodityInfo(productDataVO.marketID, productDataVO.code);
						this.m_hqForm.ShowPageMinLine(commodityInfo);
					}
				}
				else if (name.Equals("kline"))
				{
					if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						ProductDataVO productDataVO2 = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
						CommodityInfo commodityInfo2 = new CommodityInfo(productDataVO2.marketID, productDataVO2.code);
						this.m_hqForm.ShowPageKLine(commodityInfo2);
					}
				}
				else if (name.Equals("AddUserCommodity"))
				{
					if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						ProductDataVO productDataVO3 = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
						string commodityCode = productDataVO3.marketID + "_" + productDataVO3.code;
						this.m_hqClient.myCommodity.AddMyCommodity(commodityCode);
					}
				}
				else if (name.Equals("DelUserCommodity"))
				{
					if (this.m_multiQuoteData.iHighlightIndex > 0 && this.m_multiQuoteData.iHighlightIndex <= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						ProductDataVO productDataVO4 = this.m_multiQuoteData.m_curQuoteList[this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1];
						string commodityCode2 = productDataVO4.marketID + "_" + productDataVO4.code;
						if (this.m_hqClient != null)
						{
							this.m_hqClient.myCommodity.DelMyCommodity(commodityCode2);
						}
					}
				}
				else if (name.Equals("commodityInfo"))
				{
					this.Key_F10_Pressed();
				}
				else
				{
					this.m_hqForm.UserCommand(name);
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-contextMenu_ItemClicked异常：" + ex.Message);
			}
		}
		private void selectProduct(int x, int y)
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				int x2 = this.rcData.X;
				int num = this.rcData.Y + this.fontTitle.Height + 2;
				int num2 = this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart;
				if (num2 > this.iStockRows)
				{
					num2 = this.iStockRows;
				}
				int i = 1;
				while (i < num2 + 1)
				{
					if (x > x2 && x < x2 + this.rcData.Width && y > num && y < num + this.fontHeight + 2)
					{
						if (i != this.m_multiQuoteData.iHighlightIndex)
						{
							this.repaintHighlightBar(i);
							return;
						}
						break;
					}
					else
					{
						num += this.fontHeight + 2;
						i++;
					}
				}
			}
		}
		private void ClickButton(int x, int y)
		{
			try
			{
				MyButton myButton = this.buttonGraph.MouseLeftClicked(x, y, this.buttonUtils.ButtonList, true);
				if (myButton != null && !myButton.Name.StartsWith("More"))
				{
					this.buttonUtils.CurButtonName = myButton.Name;
					this.m_multiQuoteData.iStart = 0;
					this.AddUserCommodity.Visible = !this.buttonUtils.CurButtonName.Equals("MyCommodity");
					this.DelUserCommodity.Visible = this.buttonUtils.CurButtonName.Equals("MyCommodity");
					this.m_hqForm.Repaint();
				}
				MyButton myButton2 = this.buttonGraph.MouseRightClicked(x, y, this.buttonUtils.ButtonList);
				if (myButton2 != null && myButton2.Name.StartsWith("More") && this.m_hqClient != null && this.m_hqClient.m_htMarketData != null && this.m_hqClient.m_htMarketData.Count > 0)
				{
					this.CreatMarketListMenu(this.buttonUtils.ButtonList, x, y);
					this.m_hqForm.Repaint();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-ClickButton异常：" + ex.Message);
			}
		}
		private void ClickButtonRight(int x, int y)
		{
			try
			{
				MyButton myButton = this.buttonGraph.MouseRightClicked(x, y, this.buttonUtils.ButtonList);
				if (myButton != null)
				{
					string name = myButton.Name;
					if (this.m_hqClient.m_htMarketData.Count > 1 && name.StartsWith("Market"))
					{
						string marketID = name.Substring(6);
						if (this.m_hqClient != null && this.m_hqClient.commodityClass != null && this.m_hqClient.commodityClass.classList.Count > 0)
						{
							this.CreatClassListMenu(marketID, this.m_hqClient.commodityClass.classList, x, y);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-ClickButtonRight异常：" + ex.Message);
			}
		}
		private void changeSortField(string strSortItem)
		{
			MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[strSortItem];
			if (multiQuoteItemInfo == null)
			{
				return;
			}
			if (multiQuoteItemInfo.sortID == -1)
			{
				return;
			}
			this.m_bShowSortTag = true;
			if (this.strSortItem.Equals(strSortItem))
			{
				this.isDescend = ((this.isDescend == 1) ? 0 : 1);
				this.m_multiQuoteData.iStart = 0;
			}
			else
			{
				this.isDescend = 0;
				this.strSortItem = strSortItem;
			}
			this.sortItems();
			this.m_hqForm.Repaint();
			this.AskForDataOnTimer();
		}
		private bool Key_LEFT_Pressed(int leftColNum = 1)
		{
			if (this.iDynamicIndex == this.m_iStaticIndex + 1)
			{
				return false;
			}
			this.iDynamicIndex -= leftColNum;
			this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
			return true;
		}
		private bool Key_RIGHT_Pressed(int rightColNum = 1)
		{
			bool result = false;
			if (this.iDynamicIndex < this.m_strItems.Length - 1)
			{
				this.iDynamicIndex += rightColNum;
				this.xChange = this.xOnceMove * (this.iDynamicIndex - this.needShowCol);
				result = true;
			}
			return result;
		}
		private bool Key_DOWN_Pressed()
		{
			if (!this.bCanMove)
			{
				return false;
			}
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0 && this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex < this.m_multiQuoteData.m_curQuoteList.Length)
			{
				if (this.m_multiQuoteData.iHighlightIndex >= this.iStockRows)
				{
					this.m_multiQuoteData.iStart += ((this.iStockRows - 1 > 0) ? this.iStockRows : 1);
					this.m_multiQuoteData.iHighlightIndex = 1;
					this.m_multiQuoteData.yChange = (int)(this.yOnceMove * (double)this.m_multiQuoteData.iStart);
					if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
					{
						this.m_multiQuoteData.yChange = this.yMaxChaneg;
					}
					return true;
				}
				this.repaintHighlightBar(this.m_multiQuoteData.iHighlightIndex + 1);
			}
			return false;
		}
		private bool Key_UP_Pressed()
		{
			if (!this.bCanMove)
			{
				return false;
			}
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				if (this.m_multiQuoteData.iHighlightIndex > 1)
				{
					this.repaintHighlightBar(this.m_multiQuoteData.iHighlightIndex - 1);
				}
				else if (this.m_multiQuoteData.iStart > 0)
				{
					this.m_multiQuoteData.iStart -= ((this.iStockRows - 1 > 0) ? this.iStockRows : 1);
					if (this.m_multiQuoteData.iStart < 0)
					{
						this.m_multiQuoteData.iStart = 0;
					}
					this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
					if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length;
					}
					this.m_multiQuoteData.yChange = (int)(this.yOnceMove * (double)this.m_multiQuoteData.iStart);
					return true;
				}
			}
			return false;
		}
		private bool Key_PAGEUP_Pressed()
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				if (this.m_multiQuoteData.iStart > 0)
				{
					this.m_multiQuoteData.iStart -= ((this.iStockRows - 1 > 0) ? (this.iStockRows - 1) : 1);
					if (this.m_multiQuoteData.iStart < 0)
					{
						this.m_multiQuoteData.iStart = 0;
					}
					this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
					if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length - 1;
					}
					this.m_multiQuoteData.yChange = (int)(this.yOnceMove * (double)this.m_multiQuoteData.iStart);
					return true;
				}
				this.repaintHighlightBar(1);
			}
			return false;
		}
		public bool YScrollUp(int UpDataRowNum = 1)
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				if (this.m_multiQuoteData.iStart > 0)
				{
					this.m_multiQuoteData.iStart -= ((this.iStockRows - 1 > 0) ? (this.iStockRows - 1) : 1);
					if (this.m_multiQuoteData.iStart < 0)
					{
						this.m_multiQuoteData.iStart = 0;
					}
					this.m_multiQuoteData.iHighlightIndex = this.iStockRows;
					if (this.m_multiQuoteData.iHighlightIndex >= this.m_multiQuoteData.m_curQuoteList.Length)
					{
						this.m_multiQuoteData.iHighlightIndex = this.m_multiQuoteData.m_curQuoteList.Length - 1;
					}
					return true;
				}
				this.repaintHighlightBar(1);
			}
			return false;
		}
		private bool Key_PAGEDOWN_Pressed()
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				if (this.m_multiQuoteData.iStart + this.iStockRows < this.m_multiQuoteData.m_curQuoteList.Length)
				{
					this.m_multiQuoteData.iStart += ((this.iStockRows - 1 > 0) ? (this.iStockRows - 1) : 1);
					this.m_multiQuoteData.iHighlightIndex = 1;
					this.m_multiQuoteData.yChange = (int)(this.yOnceMove * (double)this.m_multiQuoteData.iStart);
					if (this.m_multiQuoteData.yChange > this.yMaxChaneg)
					{
						this.m_multiQuoteData.yChange = this.yMaxChaneg;
					}
					return true;
				}
				this.repaintHighlightBar(this.m_multiQuoteData.m_curQuoteList.Length - this.m_multiQuoteData.iStart);
			}
			return false;
		}
		private bool Key_F10_Pressed()
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				int num = this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1;
				if (num >= 0 && num <= this.m_multiQuoteData.m_curQuoteList.Length - 1)
				{
					string code = this.m_multiQuoteData.m_curQuoteList[num].code;
					this.m_hqForm.DisplayCommodityInfo(code);
				}
			}
			return false;
		}
		private bool Key_ENTER_Pressed()
		{
			if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
			{
				int num = this.m_multiQuoteData.iStart + this.m_multiQuoteData.iHighlightIndex - 1;
				if (num >= 0 && num <= this.m_multiQuoteData.m_curQuoteList.Length - 1)
				{
					CommodityInfo commodityInfo = new CommodityInfo(this.m_multiQuoteData.m_curQuoteList[num].marketID, this.m_multiQuoteData.m_curQuoteList[num].code);
					this.m_hqForm.QueryStock(commodityInfo);
					return true;
				}
			}
			return false;
		}
		private void CreatClassListMenu(string marketID, ArrayList classList, int x, int y)
		{
			try
			{
				ContextMenuStrip contextMenuStrip = new ContextMenuStrip
				{
					BackColor = Color.Black,
					ForeColor = Color.White,
					ShowImageMargin = false,
					ShowCheckMargin = false
				};
				for (int i = 0; i < classList.Count; i++)
				{
					ClassVO classVO = (ClassVO)classList[i];
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(classVO.name);
					toolStripMenuItem.Name = "MAC" + marketID + "_" + classVO.classID;
					contextMenuStrip.Items.Add(toolStripMenuItem);
				}
				if (contextMenuStrip.Items.Count > 0)
				{
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_AllCommodity"));
					toolStripMenuItem2.Name = "MAC" + marketID;
					contextMenuStrip.Items.Insert(0, toolStripMenuItem2);
				}
				contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.classMenuStrip_ItemClicked);
				contextMenuStrip.Show((Form)this.m_hqForm, new Point(x, y), ToolStripDropDownDirection.AboveRight);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-CreatClassListMenu异常：" + ex.Message);
			}
		}
		private void CreatMarketListMenu(ArrayList marketList, int x, int y)
		{
			try
			{
				ContextMenuStrip contextMenuStrip = new ContextMenuStrip
				{
					BackColor = Color.Black,
					ForeColor = Color.White,
					ShowImageMargin = false,
					ShowCheckMargin = false
				};
				for (int i = this.setInfo.ShowMarketBtnCount + 1; i < marketList.Count; i++)
				{
					MyButton myButton = (MyButton)marketList[i];
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(myButton.Text);
					toolStripMenuItem.Name = myButton.Name;
					contextMenuStrip.Items.Add(toolStripMenuItem);
				}
				contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.marketMenuStrip_ItemClicked);
				contextMenuStrip.Show((Form)this.m_hqForm, new Point(x, y), ToolStripDropDownDirection.AboveRight);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-CreatMarketListMenu异常：" + ex.Message);
			}
		}
		private void marketMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				string name = e.ClickedItem.Name;
				if (name.StartsWith("All"))
				{
					MarketForm marketForm = new MarketForm();
					int num = 0;
					foreach (DictionaryEntry dictionaryEntry in this.m_hqClient.m_htMarketData)
					{
						MarketDataVO marketDataVO = (MarketDataVO)dictionaryEntry.Value;
						Label label = new Label();
						label.Parent = marketForm.MainPanel;
						label.ForeColor = Color.White;
						label.Location = new Point(10, 35 + num * 25);
						label.Font = new Font("宋体", 12f, FontStyle.Regular);
						label.TextAlign = ContentAlignment.MiddleLeft;
						label.Text = marketDataVO.marketID;
						Label label2 = new Label
						{
							Tag = "Market" + marketDataVO.marketID,
							Parent = marketForm.MainPanel,
							ForeColor = Color.White,
							Location = new Point(110, this.lbLocationY + num * this.lbHeight),
							Font = new Font("宋体", 12f, FontStyle.Underline),
							Cursor = Cursors.Hand,
							TextAlign = ContentAlignment.MiddleCenter,
							Text = marketDataVO.marketName
						};
						label2.Click += new EventHandler(this.lbValue_Click);
						num++;
					}
					marketForm.ShowDialog();
				}
				else
				{
					this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
					this.buttonUtils.CurButtonName = name;
					this.m_multiQuoteData.iStart = 0;
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-marketMenuStrip_ItemClicked异常：" + ex.Message);
			}
		}
		private void lbValue_Click(object sender, EventArgs e)
		{
			try
			{
				Label label = sender as Label;
				string arg_0D_0 = label.Text;
				MarketForm marketForm = label.Parent.Parent as MarketForm;
				string text = label.Tag.ToString();
				if (text.StartsWith("Market"))
				{
					this.buttonUtils.selectTemp = (label.Location.Y - this.lbLocationY) / this.lbHeight + 1;
					if (this.buttonUtils.selectTemp > this.setInfo.ShowMarketBtnCount - 2)
					{
						this.buttonUtils.selectTemp = this.setInfo.ShowMarketBtnCount;
					}
					this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
					this.buttonUtils.CurButtonName = text;
					this.m_multiQuoteData.iStart = 0;
				}
				this.m_hqForm.Repaint();
				marketForm.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-lbValue_Click异常：" + ex.Message);
			}
		}
		private void classMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				string name = e.ClickedItem.Name;
				if (name.StartsWith("MAC"))
				{
					this.buttonGraph.ResetSelButton(this.buttonUtils.ButtonList);
					this.buttonUtils.CurButtonName = name;
					this.m_multiQuoteData.iStart = 0;
				}
				this.m_hqForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_MultiQuote-classMenuStrip_ItemClicked异常：" + ex.Message);
			}
		}
		public override void Dispose()
		{
			this.setInfo.saveSetInfo("CurButtonName", this.buttonUtils.CurButtonName);
			this.setInfo.lastSave();
			GC.Collect();
		}
	}
}
