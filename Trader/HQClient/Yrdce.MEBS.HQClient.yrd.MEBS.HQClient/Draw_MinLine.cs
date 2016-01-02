using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class Draw_MinLine : IDisposable
	{
		private HQForm m_hqForm;
		private HQClientMain m_hqClient;
		private bool bLarge;
		private int m_iTotalMinNum;
		private int m_iMinLineNum;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		public int m_iPos = -1;
		private int m_iPosY = -1;
		private int iNum;
		private int m_iPrecision;
		private float m_maxPrice;
		private float m_minPrice;
		private long m_maxVolume;
		private int m_maxReserveCount;
		private int m_minReserveCount = -1;
		private ProductData m_product;
		private int iHeight;
		private int iWidth;
		private Rectangle m_rcPrice;
		private Rectangle m_rcVolume;
		private Rectangle m_rcLabel;
		private int m_iProductType;
		private TradeTimeVO[] TimeRange;
		private Rectangle rc_;
		public Bitmap m_YSrcBmp;
		private Rectangle rcYCoordinate;
		private int iPriceGridNum;
		public bool isDrawCursor;
		public Draw_MinLine(HQForm hqForm, bool _bLarge)
		{
			this.m_hqForm = hqForm;
			this.m_hqClient = hqForm.CurHQClient;
			this.pluginInfo = this.m_hqClient.pluginInfo;
			this.setInfo = this.m_hqClient.setInfo;
			this.bLarge = _bLarge;
		}
		internal void Paint(Graphics g, Rectangle rc, ProductData product)
		{
			try
			{
				this.m_product = product;
				this.rc_ = rc;
				if (product != null)
				{
					if (this.m_hqClient.m_htMarketData.Count > 0)
					{
						this.TimeRange = ((MarketDataVO)this.m_hqClient.m_htMarketData[product.commodityInfo.marketID]).m_timeRange;
					}
					this.m_iProductType = this.m_hqClient.getProductType(product.commodityInfo);
					this.GetMaxMinPrice();
					if (product.aMinLine != null)
					{
						this.m_maxVolume = 0L;
						this.m_maxReserveCount = 0;
						this.m_minReserveCount = -1;
						for (int i = 0; i < product.aMinLine.Count; i++)
						{
							MinDataVO minDataVO = (MinDataVO)product.aMinLine[i];
							float num;
							if (i == 0)
							{
								num = (float)minDataVO.totalAmount;
							}
							else
							{
								num = (float)(minDataVO.totalAmount - ((MinDataVO)this.m_product.aMinLine[i - 1]).totalAmount);
							}
							if ((float)this.m_maxVolume < num)
							{
								this.m_maxVolume = (long)num;
							}
							if (this.m_maxReserveCount < minDataVO.reserveCount)
							{
								this.m_maxReserveCount = minDataVO.reserveCount;
							}
							if (this.m_minReserveCount == -1 || this.m_minReserveCount > minDataVO.reserveCount)
							{
								this.m_minReserveCount = minDataVO.reserveCount;
							}
						}
					}
				}
				this.DrawGrid(g, rc);
				if (this.iPriceGridNum > 0)
				{
					this.DrawTrace(g);
				}
				if (this.m_hqForm.IsMultiCommidity && this.bLarge && product != null)
				{
					CodeTable codeTable = null;
					if (this.m_hqClient.m_htProduct != null && this.m_hqClient.m_htProduct[product.commodityInfo.marketID + product.commodityInfo.commodityCode] != null)
					{
						codeTable = (CodeTable)this.m_hqClient.m_htProduct[product.commodityInfo.marketID + product.commodityInfo.commodityCode];
					}
					string s;
					if (codeTable != null)
					{
						s = codeTable.sName;
					}
					else
					{
						s = "————";
					}
					g.DrawString(s, new Font("宋体", 10f, FontStyle.Regular), new SolidBrush(SetInfo.RHColor.clProductName), (float)(this.m_rcPrice.X + 1), (float)(rc.Y + 1));
				}
				if (!this.m_hqForm.IsMultiCommidity && this.m_hqClient.CurrentPage == 1)
				{
					if (this.m_iPos >= 0 && this.m_iPos < this.iNum)
					{
						this.DrawLabel(g);
					}
					if (this.bLarge)
					{
						this.m_hqForm.EndPaint();
					}
					if (this.m_iPos >= 0)
					{
						if (this.isDrawCursor)
						{
							this.m_YSrcBmp = null;
							this.DrawCursor(-1);
						}
						else
						{
							this.m_iPos = -1;
							this.m_iPosY = -1;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Draw_MinLine-Paint异常：" + ex.Message);
			}
		}
		public void DrawLabel(Graphics g)
		{
			if (this.m_iPos < 0 || this.m_iPos > this.m_iMinLineNum - 1 || g == null)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(this.m_rcLabel.X - 1, this.m_rcLabel.Y - 1, this.m_rcLabel.Width + 1, this.m_rcLabel.Height + 1);
			Font font = new Font("宋体", 10f, FontStyle.Regular);
			if (((HQClientForm)this.m_hqForm).m_rcBottom.Y < rectangle.Y + font.Height * 12)
			{
				rectangle.Y = this.rc_.Y + this.rc_.Height - font.Height * 12;
			}
			using (Bitmap bitmap = new Bitmap(rectangle.Width, font.Height * 12))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(SetInfo.RHColor.clBackGround);
					SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
					Pen pen = new Pen(SetInfo.RHColor.clNumber);
					graphics.DrawRectangle(pen, 0, 0, this.m_rcLabel.Width, font.Height * 12 - 2);
					int num = 1;
					int num2 = 1;
					solidBrush.Color = SetInfo.RHColor.clItem;
					graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Time"), font, solidBrush, (float)num, (float)num2);
					num2 += font.Height;
					string text = TradeTimeVO.HHMMSSIntToString(M_Common.GetTimeFromMinLineIndex(this.m_iPos, this.TimeRange, this.m_hqClient.m_iMinLineInterval));
					if (this.m_hqClient.m_iMinLineInterval == 60)
					{
						text = text.Substring(0, 5);
					}
					num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
					solidBrush.Color = SetInfo.RHColor.clEqual;
					graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
					if (this.m_iPos >= 0 && this.m_iPos < this.iNum)
					{
						num = 1;
						num2 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Price"), font, solidBrush, (float)num, (float)num2);
						num2 += font.Height;
						if (this.m_product == null || this.m_product.aMinLine == null || this.m_product.aMinLine.Count < this.m_iPos)
						{
							return;
						}
						MinDataVO minDataVO = (MinDataVO)this.m_product.aMinLine[this.m_iPos];
						if (minDataVO != null)
						{
							text = M_Common.FloatToString((double)minDataVO.curPrice, this.m_iPrecision);
							if (this.m_product.realData == null)
							{
								return;
							}
							if (minDataVO.curPrice > this.m_product.realData.yesterBalancePrice)
							{
								solidBrush.Color = SetInfo.RHColor.clIncrease;
							}
							else
							{
								if (minDataVO.curPrice < this.m_product.realData.yesterBalancePrice)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
							num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
						}
						num = 1;
						num2 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"), font, solidBrush, (float)num, (float)num2);
						num2 += font.Height;
						if (minDataVO != null)
						{
							text = M_Common.FloatToString((double)(minDataVO.curPrice - this.m_product.realData.yesterBalancePrice), this.m_iPrecision);
							if (minDataVO.curPrice > this.m_product.realData.yesterBalancePrice)
							{
								solidBrush.Color = SetInfo.RHColor.clIncrease;
								text = "+" + text;
							}
							else
							{
								if (minDataVO.curPrice < this.m_product.realData.yesterBalancePrice)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
							num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
						}
						num = 1;
						num2 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), font, solidBrush, (float)num, (float)num2);
						num2 += font.Height;
						if (minDataVO != null)
						{
							text = M_Common.FloatToString((double)minDataVO.averPrice, this.m_iPrecision);
							if (minDataVO.averPrice > this.m_product.realData.yesterBalancePrice)
							{
								solidBrush.Color = SetInfo.RHColor.clIncrease;
							}
							else
							{
								if (minDataVO.averPrice < this.m_product.realData.yesterBalancePrice)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
							num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
						}
						num = 1;
						num2 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), font, solidBrush, (float)num, (float)num2);
						num2 += font.Height;
						if (minDataVO != null)
						{
							float num3;
							if (this.m_iPos == 0)
							{
								num3 = (float)minDataVO.totalAmount;
							}
							else
							{
								num3 = (float)(minDataVO.totalAmount - ((MinDataVO)this.m_product.aMinLine[this.m_iPos - 1]).totalAmount);
							}
							text = Convert.ToString((int)num3);
							solidBrush.Color = SetInfo.RHColor.clVolume;
							num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
						}
						num = 1;
						num2 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), font, solidBrush, (float)num, (float)num2);
						num2 += font.Height;
						if (minDataVO != null)
						{
							text = Convert.ToString(minDataVO.reserveCount);
							solidBrush.Color = SetInfo.RHColor.clReserve;
							num = rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num, (float)num2);
						}
					}
					font.Dispose();
					pen.Dispose();
					solidBrush.Dispose();
					g.DrawImage(bitmap, rectangle.X, rectangle.Y);
				}
			}
		}
		private void DrawLabel()
		{
			if (!this.m_hqForm.IsEndPaint)
			{
				return;
			}
			using (Graphics m_Graphics = this.m_hqForm.M_Graphics)
			{
				this.m_hqForm.TranslateTransform(m_Graphics);
				this.DrawLabel(m_Graphics);
			}
		}
		private void DrawOriginalCursor(Graphics g)
		{
			int xFromMinLineIndex = this.GetXFromMinLineIndex(this.m_iPos);
			GDIDraw.XorLine(g, xFromMinLineIndex, this.m_rcPrice.Y + 1, xFromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
		}
		public void DrawCursor(int iNewPos)
		{
			int iNewPosY = -1;
			if (iNewPos >= 0 && this.iNum > 0 && iNewPos < this.iNum && this.m_product != null && this.m_product.aMinLine != null)
			{
				MinDataVO minDataVO = (MinDataVO)this.m_product.aMinLine[iNewPos];
				if (minDataVO != null)
				{
					iNewPosY = this.GetYFromPrice(minDataVO.curPrice);
				}
			}
			this.DrawCursor(iNewPos, iNewPosY);
		}
		private void DrawCursor(int iNewPosX, int iNewPosY)
		{
			using (Graphics m_Graphics = this.m_hqForm.M_Graphics)
			{
				if (this.m_hqForm.IsEndPaint)
				{
					if (this.m_iPos >= 0)
					{
						int xFromMinLineIndex = this.GetXFromMinLineIndex(this.m_iPos);
						GDIDraw.XorLine(m_Graphics, xFromMinLineIndex, this.m_rcPrice.Y + 1, xFromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
					}
					if (this.m_iPosY >= 0)
					{
						GDIDraw.XorLine(m_Graphics, this.m_rcPrice.X, this.m_iPosY, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_iPosY, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
						if (this.m_YSrcBmp != null)
						{
							m_Graphics.DrawImage(this.m_YSrcBmp, this.rcYCoordinate.X, this.rcYCoordinate.Y);
						}
					}
					if (iNewPosX >= 0)
					{
						this.m_iPos = iNewPosX;
						int xFromMinLineIndex2 = this.GetXFromMinLineIndex(this.m_iPos);
						GDIDraw.XorLine(m_Graphics, xFromMinLineIndex2, this.m_rcPrice.Y + 1, xFromMinLineIndex2, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
					}
					if (iNewPosY >= 0)
					{
						if (iNewPosY > this.m_rcPrice.Y + this.m_rcPrice.Height && iNewPosY < this.m_rcVolume.Y)
						{
							this.m_iPosY = -1;
						}
						else
						{
							this.m_iPosY = iNewPosY;
							GDIDraw.XorLine(m_Graphics, this.m_rcPrice.X, this.m_iPosY, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_iPosY, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
						}
					}
					if (this.m_iPosY >= 0)
					{
						this.rcYCoordinate = new Rectangle(this.m_rcPrice.Width + this.m_rcPrice.X, this.m_iPosY, 45, 13);
						using (Bitmap bitmap = new Bitmap(this.rcYCoordinate.Width, this.rcYCoordinate.Height))
						{
							using (Graphics graphics = Graphics.FromImage(bitmap))
							{
								graphics.Clear(SetInfo.RHColor.clHighlight);
								SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clHighlight);
								Pen pen = new Pen(SetInfo.RHColor.clGrid);
								graphics.DrawRectangle(pen, 0, 0, this.rcYCoordinate.Width - 1, this.rcYCoordinate.Height - 1);
								Font font = new Font("宋体", 10f, FontStyle.Regular);
								solidBrush.Color = SetInfo.RHColor.clMinLine;
								string s = "";
								if (this.m_iPosY >= this.m_rcPrice.Y && this.m_iPosY <= this.m_rcPrice.Y + this.m_rcPrice.Height)
								{
									s = string.Concat(this.GetPriceFromY(this.m_iPosY));
								}
								else
								{
									if (this.m_iPosY >= this.m_rcVolume.Y && this.m_iPosY <= this.m_rcVolume.Y + this.m_rcVolume.Height)
									{
										s = string.Concat(this.GetReserveFromY(this.m_iPosY));
									}
								}
								graphics.DrawString(s, font, solidBrush, 0f, 0f);
								font.Dispose();
								pen.Dispose();
								solidBrush.Dispose();
								this.m_YSrcBmp = new Bitmap(this.rcYCoordinate.Width, this.rcYCoordinate.Height);
								Graphics graphics2 = Graphics.FromImage(this.m_YSrcBmp);
								IntPtr hdc = m_Graphics.GetHdc();
								IntPtr hdc2 = graphics2.GetHdc();
								GDIDraw.BitBlt(hdc2, 0, 0, this.rcYCoordinate.Width, this.rcYCoordinate.Height, hdc, this.rcYCoordinate.X, this.rcYCoordinate.Y, 13369376);
								m_Graphics.ReleaseHdc(hdc);
								graphics2.ReleaseHdc(hdc2);
								m_Graphics.DrawImage(bitmap, this.rcYCoordinate.X, this.rcYCoordinate.Y);
							}
						}
					}
				}
			}
		}
		private bool inTrade(int minLineIndex, int[] tradeSecNo)
		{
			if (this.TimeRange == null || this.TimeRange.Length == 0)
			{
				return false;
			}
			int timeFromMinLineIndex = M_Common.GetTimeFromMinLineIndex(minLineIndex, this.TimeRange, this.m_hqClient.m_iMinLineInterval);
			int num = timeFromMinLineIndex / 100;
			for (int i = 0; i < tradeSecNo.Length; i++)
			{
				if (tradeSecNo[i] > this.TimeRange.Length)
				{
					return true;
				}
				for (int j = 0; j < this.TimeRange.Length; j++)
				{
					if (this.TimeRange[j].orderID == tradeSecNo[i])
					{
						int beginTime = this.TimeRange[j].beginTime;
						int num2 = this.TimeRange[j].endTime;
						int num3 = num;
						if (num2 < beginTime)
						{
							num2 += 2400;
							if (num3 < beginTime)
							{
								num3 += 2400;
							}
						}
						if (num3 >= beginTime && num3 <= num2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		private int GetXFromMinLineIndex(int index)
		{
			if (this.m_iMinLineNum == 0)
			{
				return 0;
			}
			if (this.m_iTotalMinNum == 1)
			{
				return this.m_rcPrice.X + 1 + this.m_rcPrice.Width - 2;
			}
			if (index >= this.m_iMinLineNum)
			{
				index = this.m_iMinLineNum - 1;
			}
			return this.m_rcPrice.X + 1 + index * (this.m_rcPrice.Width - 2) / (this.m_iMinLineNum - 1);
		}
		private int GetYFromVolume(long volume)
		{
			if (0L >= this.m_maxVolume)
			{
				return this.m_rcVolume.Y + this.m_rcVolume.Height - 1;
			}
			int num = (int)((double)volume * (double)(this.m_rcVolume.Height - 1) / (double)this.m_maxVolume);
			return this.m_rcVolume.Y + this.m_rcVolume.Height - 1 - num;
		}
		private int GetYFromReserve(int reserveCount)
		{
			if (0 >= this.m_maxReserveCount || reserveCount == 0)
			{
				return this.m_rcVolume.Y + this.m_rcVolume.Height - 1;
			}
			int num = (int)((double)(reserveCount - this.m_minReserveCount) * (double)(this.m_rcVolume.Height - 1) / (double)(this.m_maxReserveCount - this.m_minReserveCount));
			return this.m_rcVolume.Y + this.m_rcVolume.Height - 1 - num;
		}
		private void DrawTrace(Graphics g)
		{
			Pen pen = new Pen(SetInfo.RHColor.clBackGround);
			if (this.m_product == null || this.m_product.realData == null || this.m_product.aMinLine == null || this.m_product.realData.yesterBalancePrice < 0.01f)
			{
				if (this.m_product != null && this.m_product.realData == null)
				{
					Logger.wirte(MsgType.Warning, "为空了！！！！！！！！！！！！！！！！");
				}
				return;
			}
			ProductData product;
			Monitor.Enter(product = this.m_product);
			try
			{
				int x = this.m_rcPrice.X;
				int num2;
				int num = num2 = this.GetYFromPrice(this.m_product.realData.yesterBalancePrice);
				this.iNum = this.m_product.aMinLine.Count;
				MinDataVO minDataVO;
				if (this.iNum > 0)
				{
					minDataVO = (MinDataVO)this.m_product.aMinLine[this.iNum - 1];
				}
				else
				{
					minDataVO = new MinDataVO();
					minDataVO.averPrice = this.m_product.realData.yesterBalancePrice;
					minDataVO.curPrice = this.m_product.realData.yesterBalancePrice;
					minDataVO.reserveCount = 0;
					minDataVO.totalAmount = 0L;
					minDataVO.totalMoney = 0.0;
				}
				MarketDataVO marketDataVO = (MarketDataVO)this.m_hqClient.m_htMarketData[this.m_product.commodityInfo.marketID];
				int minLineIndexFromTime = M_Common.GetMinLineIndexFromTime(marketDataVO.date, marketDataVO.time, marketDataVO.m_timeRange, this.m_hqClient.m_iMinLineInterval);
				for (int i = this.iNum; i < minLineIndexFromTime + 1; i++)
				{
					MinDataVO minDataVO2 = new MinDataVO();
					minDataVO2.averPrice = minDataVO.averPrice;
					minDataVO2.curPrice = minDataVO.curPrice;
					minDataVO2.reserveCount = minDataVO.reserveCount;
					minDataVO2.totalAmount = minDataVO.totalAmount;
					minDataVO2.totalMoney = minDataVO.totalMoney;
					this.m_product.aMinLine.Add(minDataVO2);
				}
				this.iNum = this.m_product.aMinLine.Count;
				CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
				bool flag = this.m_hqClient.isIndex(this.m_product.commodityInfo);
				int num3 = 0;
				int num4 = 0;
				while (num4 < this.iNum && num4 < this.m_product.aMinLine.Count)
				{
					minDataVO = (MinDataVO)this.m_product.aMinLine[num4];
					bool flag2 = flag || codeTable == null || this.inTrade(num4, codeTable.tradeSecNo);
					if (minDataVO.curPrice == 0f)
					{
						minDataVO.curPrice = (minDataVO.averPrice = this.m_product.realData.yesterBalancePrice);
					}
					int xFromMinLineIndex = this.GetXFromMinLineIndex(num4);
					int num5;
					if (minDataVO.curPrice < this.m_product.realData.lowPrice || minDataVO.curPrice > this.m_product.realData.highPrice)
					{
						num5 = num2;
					}
					else
					{
						num5 = this.GetYFromPrice(minDataVO.curPrice);
					}
					float averPrice = minDataVO.averPrice;
					int num6;
					if (averPrice < this.m_product.realData.lowPrice || averPrice > this.m_product.realData.highPrice)
					{
						num6 = num;
					}
					else
					{
						num6 = this.GetYFromPrice(averPrice);
					}
					float num7;
					if (num4 == 0)
					{
						num7 = (float)minDataVO.totalAmount;
					}
					else
					{
						num7 = (float)(minDataVO.totalAmount - ((MinDataVO)this.m_product.aMinLine[num4 - 1]).totalAmount);
					}
					if (2 != this.m_iProductType && 3 != this.m_iProductType)
					{
						pen.Color = SetInfo.RHColor.clVolume;
						if (flag2)
						{
							g.DrawLine(pen, x, num, xFromMinLineIndex, num6);
						}
					}
					if (num7 > 0f && flag2)
					{
						int yFromVolume = this.GetYFromVolume((long)num7);
						pen.Color = SetInfo.RHColor.clVolume;
						g.DrawLine(pen, xFromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, xFromMinLineIndex, yFromVolume);
					}
					int yFromReserve = this.GetYFromReserve(minDataVO.reserveCount);
					if (num4 == 0)
					{
						num3 = yFromReserve;
					}
					if (flag2)
					{
						pen.Color = SetInfo.RHColor.clMinLine;
						g.DrawLine(pen, x, num2, xFromMinLineIndex, num5);
						if (minDataVO.reserveCount > 0 && num3 > 0)
						{
							pen.Color = SetInfo.RHColor.clReserve;
							g.DrawLine(pen, x, num3, xFromMinLineIndex, yFromReserve);
						}
					}
					x = xFromMinLineIndex;
					num2 = num5;
					num = num6;
					num3 = yFromReserve;
					num4++;
				}
				pen.Dispose();
			}
			finally
			{
				Monitor.Exit(product);
			}
		}
		private void DrawGrid(Graphics g, Rectangle rc)
		{
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clGrid);
			Pen pen = new Pen(SetInfo.RHColor.clGrid);
			Font font;
			if (this.bLarge)
			{
				font = new Font("宋体", 10f, FontStyle.Regular);
			}
			else
			{
				font = new Font("宋体", 9f, FontStyle.Regular);
			}
			this.iHeight = font.Height;
			this.iWidth = (int)g.MeasureString("AAAAAAAAAA", font).Width / 10;
			int num;
			int width;
			int num2;
			int num3;
			if (this.bLarge)
			{
				num = this.iWidth * 9 - 1;
				width = rc.Width - num - (int)g.MeasureString("100.0%", font).Width;
				num2 = rc.Height * 7 / 10 - this.iHeight / 2;
				num3 = rc.Height * 3 / 10 - this.iHeight * 5 / 2;
				if (this.m_hqForm.IsMultiCommidity)
				{
					int num4 = num2 + num3;
					num2 = num4 * 3 / 4;
					num3 = num4 / 4;
				}
			}
			else
			{
				num = this.iWidth * 7 - 1;
				width = rc.Width - num - this.iWidth;
				num2 = rc.Height * 6 / 9 - this.iHeight / 2;
				num3 = rc.Height * 3 / 9 - this.iHeight / 2;
			}
			this.m_rcPrice = new Rectangle(rc.X + num, rc.Y + this.iHeight, width, num2);
			this.m_rcVolume = new Rectangle(rc.X + num, this.m_rcPrice.Y + this.m_rcPrice.Height + this.iHeight, width, num3);
			this.m_rcLabel = new Rectangle(rc.X + 1, rc.Y + this.iHeight * 2, num, this.iHeight * 12);
			if (this.m_rcLabel.Y + this.m_rcLabel.Height > rc.Y + rc.Height)
			{
				this.m_rcLabel.Height = rc.Y + rc.Height - this.m_rcLabel.Y;
			}
			this.iPriceGridNum = this.m_rcPrice.Height / this.iHeight * 2 / 3;
			if (this.iPriceGridNum % 2 == 1)
			{
				this.iPriceGridNum++;
			}
			if (this.iPriceGridNum <= 0)
			{
				return;
			}
			if (!this.bLarge)
			{
				g.DrawLine(pen, rc.X, rc.Y, rc.X, rc.Y + rc.Height);
			}
			for (int i = 0; i <= this.iPriceGridNum; i++)
			{
				g.DrawLine(pen, this.m_rcPrice.X, this.m_rcPrice.Y + this.m_rcPrice.Height * i / this.iPriceGridNum, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_rcPrice.Y + this.m_rcPrice.Height * i / this.iPriceGridNum);
			}
			int num5 = this.m_rcVolume.Height / this.iHeight * 2 / 3;
			if (num5 <= 0)
			{
				num5 = 1;
			}
			for (int j = 0; j <= num5; j++)
			{
				g.DrawLine(pen, this.m_rcVolume.X, this.m_rcVolume.Y + this.m_rcVolume.Height * j / num5, this.m_rcVolume.X + this.m_rcVolume.Width, this.m_rcVolume.Y + this.m_rcVolume.Height * j / num5);
			}
			if (this.TimeRange == null)
			{
				if (this.m_hqClient.TimeRange == null)
				{
					return;
				}
				this.TimeRange = this.m_hqClient.TimeRange;
			}
			this.m_iTotalMinNum = TradeTimeVO.GetTotalMinute(this.TimeRange);
			this.m_iMinLineNum = this.m_iTotalMinNum * (60 / this.m_hqClient.m_iMinLineInterval);
			int k = 0;
			int num6 = 0;
			this.GetXFromTimeIndex(this.m_iTotalMinNum, this.m_iTotalMinNum - 1);
			int[] array = new int[this.TimeRange.Length + 1];
			array[0] = 0;
			for (int l = 0; l < this.TimeRange.Length; l++)
			{
				array[l + 1] = M_Common.GetTimeIndexFromTime(this.TimeRange[l].endDate, this.TimeRange[l].endTime, this.TimeRange) + 1;
			}
			string text = "09:00";
			int num7 = 0;
			while (k <= this.m_iTotalMinNum)
			{
				if (k > 0 && this.m_iTotalMinNum - k < 10)
				{
					k = this.m_iTotalMinNum;
				}
				string text2 = "";
				bool flag = false;
				if (k > array[num7])
				{
					k = array[num7];
				}
				if (k == array[num7] && num7 < array.Length - 1)
				{
					flag = true;
					if (num7 > 0 && num7 < this.TimeRange.Length)
					{
						text2 = string.Concat(this.TimeRange[num7].beginTime);
						while (text2.Length < 4)
						{
							text2 = "0" + text2;
						}
						text2 = text2.Substring(0, 2) + ":" + text2.Substring(2, 2);
					}
					num7++;
				}
				int num8 = this.GetXFromTimeIndex((k > 0) ? (k - 1) : k, k);
				bool flag2 = false;
				if (flag)
				{
					if (k == 0)
					{
						num8--;
					}
					else
					{
						if (k == this.m_iTotalMinNum)
						{
							num8++;
						}
					}
					g.DrawLine(pen, num8, this.m_rcPrice.Y, num8, this.m_rcPrice.Y + this.m_rcPrice.Height);
					g.DrawLine(pen, num8, this.m_rcVolume.Y, num8, this.m_rcVolume.Y + this.m_rcVolume.Height);
					flag2 = true;
				}
				else
				{
					M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, num8, this.m_rcPrice.Y, num8, this.m_rcPrice.Y + this.m_rcPrice.Height);
					M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, num8, this.m_rcVolume.Y, num8, this.m_rcVolume.Y + this.m_rcVolume.Height);
					if (num8 - num6 >= (int)(g.MeasureString(text, font).Width * 1f))
					{
						flag2 = true;
					}
				}
				if (flag2 && this.bLarge)
				{
					solidBrush.Color = SetInfo.RHColor.clNumber;
					int value;
					if (k == 0)
					{
						value = this.TimeRange[0].beginTime;
					}
					else
					{
						if (k == this.m_iTotalMinNum)
						{
							value = this.TimeRange[this.TimeRange.Length - 1].endTime;
						}
						else
						{
							value = M_Common.GetTimeFromTimeIndex(k - 1, this.TimeRange);
						}
					}
					string text3 = Convert.ToString(value);
					while (text3.Length < 4)
					{
						text3 = "0" + text3;
					}
					text3 = text3.Substring(0, 2) + ":" + text3.Substring(2, 2);
					if (text2.Length > 0 && !text3.Equals(text2))
					{
						text3 = text3 + "/" + text2;
					}
					int num9 = this.m_rcVolume.Y + this.m_rcVolume.Height;
					if (num8 - num6 < (int)(g.MeasureString(text3, font).Width * 0.8f))
					{
						this.m_hqForm.ClearRect(g, (float)num6 - g.MeasureString(text, font).Width / 2f, (float)(this.m_rcVolume.Y + this.m_rcVolume.Height + 1), g.MeasureString(text, font).Width, (float)this.iHeight);
					}
					g.DrawString(text3, font, solidBrush, (float)num8 - g.MeasureString(text3, font).Width / 2f, (float)num9);
					num6 = num8;
					text = text3;
				}
				if (k >= this.m_iTotalMinNum)
				{
					break;
				}
				k += 30;
				if (k > this.m_iTotalMinNum)
				{
					k = this.m_iTotalMinNum;
				}
			}
			if (this.m_product == null || this.m_product.realData == null)
			{
				return;
			}
			this.m_iPrecision = this.m_hqClient.GetPrecision(this.m_product.commodityInfo);
			float num10 = this.m_maxPrice - this.m_minPrice;
			float num11 = 1f;
			for (int m = 0; m < this.m_iPrecision; m++)
			{
				num10 *= 10f;
				num11 /= 10f;
			}
			int num12 = (int)(num10 + 1f - num11);
			if (num12 % this.iPriceGridNum > 0 || num12 == 0)
			{
				num12 = (num12 / this.iPriceGridNum + 1) * this.iPriceGridNum;
			}
			num10 = (float)num12;
			for (int n = 0; n < this.m_iPrecision; n++)
			{
				num10 /= 10f;
			}
			this.m_maxPrice = this.m_product.realData.yesterBalancePrice + num10 / 2f;
			this.m_minPrice = this.m_product.realData.yesterBalancePrice - num10 / 2f;
			float yesterBalancePrice = this.m_product.realData.yesterBalancePrice;
			for (int num13 = 0; num13 <= this.iPriceGridNum; num13++)
			{
				float num14 = this.m_maxPrice - (this.m_maxPrice - this.m_minPrice) * (float)num13 / (float)this.iPriceGridNum;
				if (num14 > yesterBalancePrice)
				{
					solidBrush.Color = SetInfo.RHColor.clIncrease;
				}
				else
				{
					if (yesterBalancePrice > num14)
					{
						solidBrush.Color = SetInfo.RHColor.clDecrease;
					}
					else
					{
						if (this.m_maxPrice > this.m_minPrice)
						{
							g.DrawLine(pen, this.m_rcPrice.X, this.m_rcPrice.Y + this.m_rcPrice.Height * num13 / this.iPriceGridNum + 1, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_rcPrice.Y + this.m_rcPrice.Height * num13 / this.iPriceGridNum + 1);
						}
						solidBrush.Color = SetInfo.RHColor.clEqual;
					}
				}
				string text4 = M_Common.FloatToString((double)num14, this.m_iPrecision);
				int num15 = this.m_rcPrice.X - (int)g.MeasureString(text4, font).Width - 1;
				int num16 = this.m_rcPrice.Y + this.m_rcPrice.Height * num13 / this.iPriceGridNum - (int)font.Size;
				g.DrawString(text4, font, solidBrush, (float)num15, (float)num16);
				if (this.bLarge)
				{
					float num17;
					if (0f != yesterBalancePrice)
					{
						num17 = (num14 - yesterBalancePrice) * 100f / yesterBalancePrice;
					}
					else
					{
						num17 = 0f;
					}
					if (num17 < 0f)
					{
						num17 = -num17;
					}
					text4 = M_Common.FloatToString((double)num17, 2);
					if (num17 >= 100f)
					{
						text4 = text4.Substring(0, text4.Length - 1);
					}
					text4 += "%";
					num15 = this.m_rcPrice.X + this.m_rcPrice.Width + 2;
					g.DrawString(text4, font, solidBrush, (float)num15, (float)num16);
				}
			}
			if (this.m_maxReserveCount == this.m_minReserveCount)
			{
				if (this.m_minReserveCount > 0)
				{
					this.m_maxReserveCount += num5 - 1;
					this.m_minReserveCount--;
				}
			}
			else
			{
				int num18 = (int)((double)(this.m_maxReserveCount - this.m_minReserveCount) * 0.1);
				if (num18 <= 0)
				{
					num18 = 1;
				}
				this.m_maxReserveCount += num18;
				this.m_minReserveCount -= num18;
			}
			if (this.m_minReserveCount < 0)
			{
				this.m_minReserveCount = 0;
			}
			solidBrush.Color = SetInfo.RHColor.clVolume;
			long num19 = 0L;
			for (int num20 = 0; num20 < num5; num20++)
			{
				long num21 = this.m_maxVolume - this.m_maxVolume * (long)num20 / (long)num5;
				if (num19 != num21)
				{
					num19 = num21;
					string text5 = Convert.ToString(num21);
					int num22 = this.m_rcVolume.X - (int)g.MeasureString(text5, font).Width;
					int num23 = this.m_rcVolume.Y + this.m_rcVolume.Height * num20 / num5 - (int)font.Size;
					g.DrawString(text5, font, solidBrush, (float)num22, (float)num23);
				}
			}
			if (this.bLarge)
			{
				solidBrush.Color = SetInfo.RHColor.clReserve;
				for (int num24 = 0; num24 <= num5; num24++)
				{
					long value2 = (long)(this.m_maxReserveCount - (this.m_maxReserveCount - this.m_minReserveCount) * num24 / num5);
					string s = Convert.ToString(value2);
					int num25 = this.m_rcVolume.X + this.m_rcVolume.Width + 1;
					int num26 = this.m_rcVolume.Y + this.m_rcVolume.Height * num24 / num5 - (int)font.Size;
					g.DrawString(s, font, solidBrush, (float)num25, (float)num26);
				}
			}
			solidBrush.Dispose();
			pen.Dispose();
		}
		private int GetXFromTimeIndex(int index, int temp)
		{
			if (this.m_iTotalMinNum == 0)
			{
				return 0;
			}
			if (this.m_iTotalMinNum != 1)
			{
				if (index >= this.m_iTotalMinNum)
				{
					index = this.m_iTotalMinNum - 1;
				}
				return this.m_rcPrice.X + 1 + index * (this.m_rcPrice.Width - 2) / (this.m_iTotalMinNum - 1);
			}
			if (index == temp)
			{
				return this.m_rcPrice.X + 1;
			}
			return this.m_rcPrice.X + 1 + this.m_rcPrice.Width - 2;
		}
		private float GetPriceFromY(int y)
		{
			float num = (float)((int)((float)(this.m_rcPrice.Height - (y - this.m_rcPrice.Y)) * (this.m_maxPrice - this.m_minPrice)));
			return (float)Math.Round((double)(num / (float)this.m_rcPrice.Height) + 0.5, this.m_iPrecision) + this.m_minPrice;
		}
		private float GetReserveFromY(int y)
		{
			float num = (float)((this.m_rcVolume.Height - (y - this.m_rcVolume.Y)) * (this.m_maxReserveCount - this.m_minReserveCount));
			return (float)((int)(num / (float)this.m_rcVolume.Height) + this.m_minReserveCount);
		}
		private int GetYFromPrice(float price)
		{
			if (this.m_maxPrice == this.m_minPrice)
			{
				return this.m_rcPrice.Y + this.m_rcPrice.Height - 1;
			}
			int num = (int)((double)((price - this.m_minPrice) * (float)this.m_rcPrice.Height / (this.m_maxPrice - this.m_minPrice)) + 0.5);
			return this.m_rcPrice.Y + this.m_rcPrice.Height - num;
		}
		private void GetMaxMinPrice()
		{
			if (this.m_product.realData == null)
			{
				return;
			}
			if (this.m_product.realData != null && this.m_product.realData.highPrice < this.m_product.realData.yesterBalancePrice)
			{
				this.m_maxPrice = this.m_product.realData.yesterBalancePrice;
			}
			else
			{
				this.m_maxPrice = this.m_product.realData.highPrice;
			}
			if (this.m_maxPrice < 0.001f)
			{
				return;
			}
			if (this.m_product.realData.lowPrice > this.m_product.realData.yesterBalancePrice)
			{
				this.m_minPrice = this.m_product.realData.yesterBalancePrice;
			}
			else
			{
				this.m_minPrice = this.m_product.realData.lowPrice;
			}
			float num;
			if (this.m_hqClient.GetPrecision(this.m_product.commodityInfo) == 3)
			{
				num = 0.0055f;
			}
			else
			{
				num = this.m_product.realData.yesterBalancePrice / 1000f;
			}
			if (this.m_product.realData.highPrice == 0f && this.m_product.realData.lowPrice == 0f)
			{
				this.m_minPrice = 0f;
				this.m_maxPrice = 2f * this.m_product.realData.yesterBalancePrice;
				return;
			}
			float num2 = 0f;
			float num3 = 0f;
			float yesterBalancePrice = this.m_product.realData.yesterBalancePrice;
			if (this.m_product.realData.highPrice - yesterBalancePrice >= num)
			{
				this.m_maxPrice = this.m_product.realData.highPrice;
				num2 = this.m_product.realData.highPrice - yesterBalancePrice;
			}
			else
			{
				this.m_maxPrice = yesterBalancePrice + num;
			}
			if (yesterBalancePrice - this.m_product.realData.lowPrice >= num)
			{
				this.m_minPrice = this.m_product.realData.lowPrice;
				num3 = yesterBalancePrice - this.m_product.realData.lowPrice;
			}
			else
			{
				this.m_minPrice = yesterBalancePrice - num;
			}
			if (num2 > num3)
			{
				this.m_minPrice = yesterBalancePrice - num2;
			}
			if (num2 < num3)
			{
				this.m_maxPrice = yesterBalancePrice + num3;
			}
		}
		internal bool MouseDoubleClick(int x, int y)
		{
			if (y < this.m_rcPrice.Y || y > this.m_rcVolume.Y + this.m_rcVolume.Height || x < this.m_rcPrice.X || x > this.m_rcPrice.X + this.m_rcPrice.Width)
			{
				return false;
			}
			if (this.isDrawCursor)
			{
				if (this.m_iPos != -1)
				{
					this.DrawCursor(-1);
					this.m_iPos = -1;
					this.m_iPosY = -1;
					this.isDrawCursor = false;
				}
			}
			else
			{
				int minLineIndexFromX = this.GetMinLineIndexFromX(x);
				this.DrawCursor(minLineIndexFromX, y);
				this.DrawLabel();
				this.isDrawCursor = true;
			}
			return !this.isDrawCursor;
		}
		public int GetMinLineIndexFromX(int X)
		{
			if (this.m_iMinLineNum == 0)
			{
				return 0;
			}
			return (X - this.m_rcPrice.X - 1) * (this.m_iMinLineNum - 1) / (this.m_rcPrice.Width - 2);
		}
		internal bool MouseDragged(int x, int y)
		{
			if (!this.isDrawCursor)
			{
				return false;
			}
			if (y < this.m_rcPrice.Y || y > this.m_rcVolume.Y + this.m_rcVolume.Height || x < this.m_rcPrice.X || x > this.m_rcPrice.X + this.m_rcPrice.Width)
			{
				if (this.m_hqForm.M_Cursor == Cursors.Hand)
				{
					this.m_hqForm.M_Cursor = Cursors.Default;
				}
				this.DrawCursor(-1, -1);
				this.m_iPos = -1;
				this.m_iPosY = -1;
				return true;
			}
			int minLineIndexFromX = this.GetMinLineIndexFromX(x);
			this.DrawCursor(minLineIndexFromX, y);
			this.DrawLabel();
			return false;
		}
		internal bool KeyPressed(KeyEventArgs e)
		{
			bool result = false;
			Keys keyData = e.KeyData;
			if (keyData != Keys.Escape)
			{
				switch (keyData)
				{
				case Keys.Left:
					if (this.m_iPos > 0)
					{
						this.isDrawCursor = true;
						if (this.m_iPos > this.iNum)
						{
							this.DrawCursor(this.iNum - 1);
							this.DrawLabel();
						}
						else
						{
							this.DrawCursor(this.m_iPos - 1);
							this.DrawLabel();
						}
					}
					else
					{
						if (this.m_iPos == -1 && this.iNum > 0)
						{
							this.isDrawCursor = true;
							this.DrawCursor(this.iNum - 1);
							this.DrawLabel();
						}
					}
					break;
				case Keys.Right:
					if (this.m_iPos < this.m_iMinLineNum - 1 && this.m_iPos < this.iNum - 1)
					{
						this.isDrawCursor = true;
						this.DrawCursor(this.m_iPos + 1);
						this.DrawLabel();
					}
					break;
				}
			}
			else
			{
				if (this.m_iPos != -1)
				{
					this.DrawCursor(-1);
					this.m_iPos = -1;
					this.m_iPosY = -1;
					this.isDrawCursor = false;
					result = true;
				}
			}
			return result;
		}
		public void Dispose()
		{
		}
	}
}
