using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Page_Bottom
	{
		private HQForm m_hqForm;
		public Rectangle rc;
		private Rectangle m_rcIndex;
		private Rectangle m_rcConnectState;
		private Rectangle m_rcTime;
		private int gap = 2;
		public PluginInfo pluginInfo;
		public SetInfo setInfo;
		public Page_Bottom(Rectangle _rc, HQForm hqForm)
		{
			this.rc = _rc;
			this.m_hqForm = hqForm;
			this.pluginInfo = hqForm.CurHQClient.pluginInfo;
			this.setInfo = hqForm.CurHQClient.setInfo;
		}
		internal void Paint(Graphics g)
		{
			try
			{
				this.rc.X = 0;
				this.rc.Y = 0;
				Font font = new Font("宋体", 12f);
				SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clBackGround);
				Pen pen = new Pen(SetInfo.RHColor.clGrid);
				int num = (int)g.MeasureString("2005-12-24 09:30", font).Width;
				int num2 = (int)g.MeasureString(" " + this.pluginInfo.HQResourceManager.GetString("HQStr_DisConnected") + " ", font).Width;
				this.m_rcIndex = new Rectangle(this.rc.X, this.rc.Y, this.rc.Width - num - num2, this.rc.Height);
				this.m_rcConnectState = new Rectangle(this.rc.X + this.rc.Width - num - num2, this.rc.Y, num2, this.rc.Height);
				this.m_rcTime = new Rectangle(this.rc.X + this.rc.Width - num, this.rc.Y, num, this.rc.Height);
				g.FillRectangle(solidBrush, this.rc.X, this.rc.Y, this.rc.Width, this.rc.Height);
				g.DrawLine(pen, this.rc.X, this.rc.Y, this.rc.Width, this.rc.Y);
				g.DrawLine(pen, this.m_rcTime.X - 1, this.rc.Y, this.m_rcTime.X - 1, this.rc.Y + this.rc.Height);
				if (this.rc.Height >= font.Height / 2)
				{
					this.PaintIndex(g);
					g.DrawLine(pen, this.m_rcConnectState.X - 1, this.rc.Y, this.m_rcConnectState.X - 1, this.rc.Y + this.rc.Height);
					this.PaintConnectState(g);
					this.PaintCurTime(g);
					pen.Dispose();
					solidBrush.Dispose();
					font.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Bottom-Paint异常：" + ex.Message);
			}
		}
		private void ComputeAndPaintIndex(Graphics g)
		{
			try
			{
				int num = 0;
				int num2 = 0;
				int num3 = 2;
				int num4 = this.m_rcIndex.X + 2;
				Font font = new Font("宋体", 12f);
				SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clProductName);
				for (int i = 0; i < this.m_hqForm.CurHQClient.m_quoteList.Length; i++)
				{
					ProductDataVO productDataVO = this.m_hqForm.CurHQClient.m_quoteList[i];
					CommodityInfo commodityInfo = new CommodityInfo(productDataVO.marketID, productDataVO.code);
					if (!this.m_hqForm.CurHQClient.isIndex(commodityInfo))
					{
						num += productDataVO.reserveCount;
						num2 += (int)productDataVO.totalAmount;
					}
				}
				string text = this.pluginInfo.HQResourceManager.GetString("HQStr_Volume");
				if (num2 > 0)
				{
					text += num2;
				}
				else
				{
					text += "——";
				}
				if (this.m_rcIndex.X + this.m_rcIndex.Width - num4 >= (int)g.MeasureString(text, font).Width)
				{
					solidBrush.Color = SetInfo.RHColor.clVolume;
					g.DrawString(text, font, solidBrush, (float)this.m_rcIndex.X, (float)(this.m_rcIndex.Y + num3));
					num4 += (int)g.MeasureString(text, font).Width + 10;
					text = this.pluginInfo.HQResourceManager.GetString("HQStr_Order");
					if (num > 0)
					{
						text += num;
					}
					else
					{
						text += "——";
					}
					if (this.m_rcIndex.X + this.m_rcIndex.Width - num4 >= (int)g.MeasureString(text, font).Width)
					{
						solidBrush.Color = SetInfo.RHColor.clReserve;
						g.DrawString(text, font, solidBrush, (float)num4, (float)(this.m_rcIndex.Y + num3));
						solidBrush.Dispose();
						font.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "ComputeAndPaintIndex异常：" + ex.Message);
			}
		}
		private void PaintIndex(Graphics g)
		{
			try
			{
				if (this.m_hqForm.CurHQClient.m_bShowIndexAtBottom != 0)
				{
					if (this.m_hqForm.CurHQClient.indexMainCode.Length == 0 || string.Compare((string)this.pluginInfo.HTConfig["ShowIndex"], "false", true) == 0)
					{
						if (this.m_hqForm.CurHQClient.CurrentPage == 0)
						{
							this.ComputeAndPaintIndex(g);
						}
					}
					else
					{
						CodeTable codeTable = null;
						if (this.m_hqForm.CurHQClient.m_htProduct != null)
						{
							CommodityInfo commodityInfo = CommodityInfo.DealCode(this.m_hqForm.CurHQClient.indexMainCode);
							if (this.m_hqForm.CurHQClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode] != null)
							{
								codeTable = (CodeTable)this.m_hqForm.CurHQClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
							}
						}
						string text;
						if (codeTable != null)
						{
							text = codeTable.sName;
						}
						else
						{
							text = "  指数  ";
						}
						ProductData productData = this.m_hqForm.CurHQClient.GetProductData(CommodityInfo.DealCode(this.m_hqForm.CurHQClient.indexMainCode));
						if (productData == null || productData.realData == null)
						{
							this.ComputeAndPaintIndex(g);
						}
						else
						{
							Font font = new Font("宋体", 12f);
							SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clProductName);
							int num = this.m_rcIndex.X + 2;
							if (this.m_rcIndex.X + this.m_rcIndex.Width - num >= (int)g.MeasureString(text, font).Width)
							{
								g.DrawString(text, font, solidBrush, (float)num, (float)(this.m_rcIndex.Y + this.gap));
								num += (int)g.MeasureString(text, font).Width + 10;
								string text2;
								if (productData.realData.curPrice > 0f)
								{
									text2 = M_Common.FloatToString((double)productData.realData.curPrice, this.m_hqForm.CurHQClient.m_iPrecisionIndex);
								}
								else
								{
									text2 = "——";
								}
								if (this.m_rcIndex.X + this.m_rcIndex.Width - num >= (int)g.MeasureString(text2, font).Width)
								{
									solidBrush.Color = this.GetPriceColor(productData.realData.curPrice, productData.realData.yesterBalancePrice);
									g.DrawString(text2, font, solidBrush, (float)num, (float)(this.m_rcIndex.Y + this.gap));
									num += (int)g.MeasureString(text2, font).Width + 10;
									if (productData.realData.curPrice > 0f && productData.realData.yesterBalancePrice > 0f)
									{
										text2 = M_Common.FloatToString((double)(productData.realData.curPrice - productData.realData.yesterBalancePrice), this.m_hqForm.CurHQClient.m_iPrecisionIndex);
									}
									else
									{
										text2 = "——";
									}
									if (productData.realData.curPrice > productData.realData.yesterBalancePrice)
									{
										text2 = "+" + text2;
									}
									else if (productData.realData.curPrice * 100f == productData.realData.yesterBalancePrice * 100f)
									{
										text2 = "——";
									}
									if (this.m_rcIndex.X + this.m_rcIndex.Width - num >= (int)g.MeasureString(text2, font).Width)
									{
										solidBrush.Color = this.GetPriceColor(productData.realData.curPrice, productData.realData.yesterBalancePrice);
										g.DrawString(text2, font, solidBrush, (float)num, (float)(this.m_rcIndex.Y + this.gap));
										num += (int)g.MeasureString(text2, font).Width + 10;
										text2 = this.pluginInfo.HQResourceManager.GetString("HQStr_Volume");
										if (productData.realData.totalAmount > 0L)
										{
											text2 += Convert.ToString((int)productData.realData.totalAmount);
										}
										else
										{
											text2 += "——";
										}
										if (this.m_rcIndex.X + this.m_rcIndex.Width - num >= (int)g.MeasureString(text2, font).Width)
										{
											solidBrush.Color = SetInfo.RHColor.clVolume;
											g.DrawString(text2, font, solidBrush, (float)num, (float)(this.m_rcIndex.Y + this.gap));
											num += (int)g.MeasureString(text2, font).Width + 10;
											text2 = this.pluginInfo.HQResourceManager.GetString("HQStr_Order");
											if (productData.realData.reserveCount > 0)
											{
												text2 += Convert.ToString(productData.realData.reserveCount);
											}
											else
											{
												text2 += "——";
											}
											if (this.m_rcIndex.X + this.m_rcIndex.Width - num >= (int)g.MeasureString(text2, font).Width)
											{
												solidBrush.Color = SetInfo.RHColor.clReserve;
												g.DrawString(text2, font, solidBrush, (float)num, (float)(this.m_rcIndex.Y + this.gap));
												solidBrush.Dispose();
												font.Dispose();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Bottom-PaintIndex异常：" + ex.Message);
			}
		}
		private Color GetPriceColor(float fPrice, float fBenchMark)
		{
			if (fPrice > fBenchMark)
			{
				return SetInfo.RHColor.clIncrease;
			}
			if (fPrice < fBenchMark)
			{
				return SetInfo.RHColor.clDecrease;
			}
			return SetInfo.RHColor.clEqual;
		}
		private void PaintConnectState(Graphics g)
		{
			Font font = new Font("宋体", 12f);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clDecrease);
			string @string;
			if (this.m_hqForm.CurHQClient.Connected)
			{
				@string = this.pluginInfo.HQResourceManager.GetString("HQStr_Connected");
			}
			else
			{
				@string = this.pluginInfo.HQResourceManager.GetString("HQStr_DisConnected");
				solidBrush.Color = SetInfo.RHColor.clGrid;
			}
			g.DrawString(@string, font, solidBrush, (float)this.m_rcConnectState.X, (float)(this.m_rcConnectState.Y + this.gap));
			solidBrush.Dispose();
			font.Dispose();
		}
		private void PaintCurTime(Graphics g)
		{
			if (this.m_hqForm.CurHQClient.m_iDate == 0 || this.m_hqForm.CurHQClient.m_iTime == 0)
			{
				return;
			}
			Font font = new Font("宋体", 12f);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
			string text = TradeTimeVO.HHMMIntToString(this.m_hqForm.CurHQClient.m_iTime / 100);
			if (text.EndsWith(":"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			string s = this.m_hqForm.CurHQClient.m_iDate.ToString("####-##-##") + " " + text;
			g.DrawString(s, font, solidBrush, (float)this.m_rcTime.X, (float)(this.m_rcTime.Y + this.gap));
			solidBrush.Dispose();
			font.Dispose();
		}
		private void PaintLocalCurTime(Graphics g)
		{
			Font font = new Font("宋体", 12f);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
			DateTime now = DateTime.Now;
			string text = now.Hour.ToString();
			string text2 = now.Minute.ToString();
			if (text.Length == 1)
			{
				text = "0" + now.Hour;
			}
			if (text2.Length == 1)
			{
				text2 = "0" + now.Minute;
			}
			string s = string.Concat(new object[]
			{
				now.Year,
				"-",
				now.Month,
				"-",
				now.Day,
				"  ",
				text,
				":",
				text2
			});
			g.DrawString(s, font, solidBrush, (float)this.m_rcTime.X, (float)(this.m_rcTime.Y + this.gap));
			solidBrush.Dispose();
			font.Dispose();
		}
	}
}
