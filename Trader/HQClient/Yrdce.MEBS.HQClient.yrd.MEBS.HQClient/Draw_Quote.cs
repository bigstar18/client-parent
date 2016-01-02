using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Draw_Quote
	{
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private ButtonUtils buttonUtils;
		public Rectangle rcRightButton;
		public Page_Button rightbuttonGraph;
		public int buttonHight = 25;
		private Rectangle codeRectangle = default(Rectangle);
		public int fontGap = 2;
		private Hashtable stockM_htItemInfo;
		private string[] m_strItems;
		public Draw_Quote(HQClientMain m_hqClient)
		{
			this.pluginInfo = m_hqClient.pluginInfo;
			this.setInfo = m_hqClient.setInfo;
			this.buttonUtils = m_hqClient.buttonUtils;
		}
		private void paintStockNumber(Graphics g, SolidBrush m_Brush, Rectangle rc, ProductData product, string m_strItem, string preStr, Font font, int iPrecision, bool isLeft, int y)
		{
			string text = string.Empty;
			int num = (int)font.Size;
			int height = font.Height;
			if ("Newly".Equals(m_strItem))
			{
				if (y + height > rc.Y + rc.Height)
				{
					return;
				}
				if (product.realData != null && product.realData.curPrice > 0f)
				{
					text = M_Common.FloatToString((double)product.realData.curPrice, iPrecision);
					m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
				}
				else
				{
					text = "—";
					m_Brush.Color = SetInfo.RHColor.clEqual;
				}
			}
			else
			{
				if ("ChangeValue".Equals(m_strItem))
				{
					if (y + height > rc.Y + rc.Height)
					{
						return;
					}
					if (product.realData != null && product.realData.curPrice > 0f && product.realData.yesterBalancePrice > 0f)
					{
						text = M_Common.FloatToString((double)(product.realData.curPrice - product.realData.yesterBalancePrice), iPrecision);
						m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
					}
					else
					{
						text = "—";
						m_Brush.Color = SetInfo.RHColor.clEqual;
					}
				}
				else
				{
					if ("Open".Equals(m_strItem))
					{
						if (product.realData != null && product.realData.openPrice > 0f)
						{
							text = M_Common.FloatToString((double)product.realData.openPrice, iPrecision);
							m_Brush.Color = this.GetPriceColor(product.realData.openPrice, product.realData.yesterBalancePrice);
						}
						else
						{
							text = "—";
							m_Brush.Color = SetInfo.RHColor.clEqual;
						}
					}
					else
					{
						if ("ChangeRate".Equals(m_strItem))
						{
							if (y + height > rc.Y + rc.Height)
							{
								return;
							}
							if (product.realData != null && product.realData.curPrice > 0f && product.realData.yesterBalancePrice > 0f)
							{
								text = M_Common.FloatToString((double)((product.realData.curPrice - product.realData.yesterBalancePrice) / product.realData.yesterBalancePrice * 100f), 2) + "%";
								m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
							}
							else
							{
								text = "—";
								m_Brush.Color = SetInfo.RHColor.clEqual;
							}
						}
						else
						{
							if ("High".Equals(m_strItem))
							{
								if (product.realData != null && product.realData.highPrice > 0f)
								{
									text = M_Common.FloatToString((double)product.realData.highPrice, iPrecision);
									m_Brush.Color = this.GetPriceColor(product.realData.highPrice, product.realData.yesterBalancePrice);
								}
								else
								{
									text = "—";
									m_Brush.Color = SetInfo.RHColor.clEqual;
								}
							}
							else
							{
								if ("CurVol".Equals(m_strItem))
								{
									if (y + height > rc.Y + rc.Height)
									{
										return;
									}
									if (product.realData != null && product.realData.curAmount > 0)
									{
										text = Convert.ToString(product.realData.curAmount);
									}
									else
									{
										text = "—";
									}
									m_Brush.Color = SetInfo.RHColor.clVolume;
								}
								else
								{
									if ("Low".Equals(m_strItem))
									{
										if (product.realData != null && product.realData.lowPrice > 0f)
										{
											text = M_Common.FloatToString((double)product.realData.lowPrice, iPrecision);
											m_Brush.Color = this.GetPriceColor(product.realData.lowPrice, product.realData.yesterBalancePrice);
										}
										else
										{
											text = "—";
											m_Brush.Color = SetInfo.RHColor.clEqual;
										}
									}
									else
									{
										if ("TotalVolume".Equals(m_strItem))
										{
											if (y + height > rc.Y + rc.Height)
											{
												return;
											}
											if (product.realData != null && product.realData.totalAmount > 0L)
											{
												text = Convert.ToString((int)product.realData.totalAmount);
											}
											else
											{
												text = "—";
											}
											m_Brush.Color = SetInfo.RHColor.clVolume;
										}
										else
										{
											if ("VolRate".Equals(m_strItem))
											{
												if (product.realData != null && product.realData.amountRate > 0f)
												{
													text = M_Common.FloatToString((double)product.realData.amountRate, 2);
												}
												else
												{
													text = "—";
												}
												m_Brush.Color = SetInfo.RHColor.clVolume;
											}
											else
											{
												if ("Balance".Equals(m_strItem))
												{
													if (y + height > rc.Y + rc.Height)
													{
														return;
													}
													if (product.realData != null && product.realData.balancePrice > 0f)
													{
														text = M_Common.FloatToString((double)product.realData.balancePrice, iPrecision);
														m_Brush.Color = this.GetPriceColor(product.realData.balancePrice, product.realData.yesterBalancePrice);
													}
													else
													{
														text = "—";
														m_Brush.Color = SetInfo.RHColor.clEqual;
													}
												}
												else
												{
													if ("PreBalance".Equals(m_strItem))
													{
														if (product.realData != null && product.realData.yesterBalancePrice > 0f)
														{
															text = M_Common.FloatToString((double)product.realData.yesterBalancePrice, iPrecision);
														}
														else
														{
															text = "—";
														}
														m_Brush.Color = SetInfo.RHColor.clEqual;
													}
													else
													{
														if ("Order".Equals(m_strItem))
														{
															if (y + height > rc.Y + rc.Height)
															{
																return;
															}
															if (product.realData != null && product.realData.reserveCount > 0)
															{
																text = Convert.ToString(product.realData.reserveCount);
															}
															else
															{
																text = "—";
															}
															m_Brush.Color = SetInfo.RHColor.clReserve;
														}
														else
														{
															if ("OrderChange".Equals(m_strItem))
															{
																if (product.realData != null)
																{
																	text = Convert.ToString(product.realData.reserveChange);
																}
																else
																{
																	text = "—";
																}
																m_Brush.Color = SetInfo.RHColor.clVolume;
															}
															else
															{
																if ("cje".Equals(m_strItem))
																{
																	if (product.realData != null && product.realData.totalMoney > 0.0)
																	{
																		text = M_Common.FloatToString(product.realData.totalMoney, iPrecision);
																		m_Brush.Color = SetInfo.RHColor.clVolume;
																	}
																	else
																	{
																		text = "0";
																		m_Brush.Color = SetInfo.RHColor.clEqual;
																	}
																}
																else
																{
																	if ("hsl".Equals(m_strItem))
																	{
																		if (product.realData != null && product.realData.totalAmount > 0L && product.realData.reserveCount > 0)
																		{
																			double num2 = (double)product.realData.totalAmount / (double)product.realData.reserveCount;
																			text = M_Common.FloatToString(num2 * 100.0, 2) + "%";
																			m_Brush.Color = SetInfo.RHColor.clVolume;
																		}
																		else
																		{
																			text = "0";
																			m_Brush.Color = SetInfo.RHColor.clEqual;
																		}
																	}
																	else
																	{
																		if ("zs".Equals(m_strItem))
																		{
																			if (product.realData != null && product.realData.closePrice > 0f)
																			{
																				text = M_Common.FloatToString((double)product.realData.yesterBalancePrice, iPrecision);
																				m_Brush.Color = SetInfo.RHColor.clNumber;
																			}
																			else
																			{
																				text = "0";
																				m_Brush.Color = SetInfo.RHColor.clEqual;
																			}
																		}
																		else
																		{
																			if ("AskVolume".Equals(m_strItem))
																			{
																				if (product != null && product.realData != null && product.realData.outAmount > 0)
																				{
																					text = Convert.ToString(product.realData.outAmount);
																				}
																				else
																				{
																					text = "—";
																				}
																				m_Brush.Color = SetInfo.RHColor.clVolume;
																			}
																			else
																			{
																				if ("BidVolume".Equals(m_strItem))
																				{
																					if (product != null && product.realData != null && product.realData.inAmount > 0)
																					{
																						text = Convert.ToString(product.realData.inAmount);
																					}
																					else
																					{
																						text = "—";
																					}
																					m_Brush.Color = SetInfo.RHColor.clVolume;
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
								}
							}
						}
					}
				}
			}
			float width = g.MeasureString(preStr, font).Width;
			while (g.MeasureString(text, font).Width > (float)(rc.Width / 2) - width)
			{
				num--;
				font = new Font("宋体", (float)num, FontStyle.Regular);
			}
			int num3;
			if (isLeft)
			{
				num3 = rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width;
			}
			else
			{
				num3 = rc.X + rc.Width - (int)g.MeasureString(text, font).Width;
			}
			g.DrawString(text, font, m_Brush, (float)num3, (float)y);
		}
		internal void Paint(Graphics g, Rectangle rc, ProductData product, CommodityInfo commodityInfo, int iShowBuySellNum, HQClientMain hqClientMain)
		{
			HQForm hqForm = hqClientMain.m_hqForm;
			this.stockM_htItemInfo = hqClientMain.stockM_htItemInfo;
			this.m_strItems = hqClientMain.stockM_strItems;
			string aBVOLS = hqClientMain.ABVOLS;
			Font font = new Font("楷体_GB2312", 20f, FontStyle.Bold);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clProductName);
			Pen pen = new Pen(SetInfo.RHColor.clGrid);
			this.rightbuttonGraph = new Page_Button(this.rcRightButton, hqForm, this.buttonUtils);
			try
			{
				int num = (int)font.Size;
				int height = font.Height;
				hqClientMain.getProductType(product.commodityInfo);
				int precision = hqClientMain.GetPrecision(product.commodityInfo);
				int num2 = rc.X;
				int num3 = rc.Y;
				CodeTable codeTable = null;
				if (hqClientMain.m_htProduct != null && hqClientMain.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode] != null)
				{
					codeTable = (CodeTable)hqClientMain.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
				}
				string text;
				if (codeTable != null)
				{
					text = codeTable.sName;
				}
				else
				{
					text = "————";
				}
				string text2 = commodityInfo.commodityCode;
				if (text.Equals(text2))
				{
					text2 = "";
				}
				Font font2 = font;
				while (g.MeasureString(text + text2, font).Width > (float)(rc.Width - 20))
				{
					num--;
					font = new Font("楷体_GB2312", (float)num, FontStyle.Bold);
				}
				height = font2.Height;
				this.codeRectangle.X = num2;
				this.codeRectangle.Y = num3;
				this.codeRectangle.Width = rc.Width;
				this.codeRectangle.Height = height;
				if (num3 + height < rc.Y + rc.Height)
				{
					g.DrawString(text, font, solidBrush, (float)(num2 + 10), (float)num3);
				}
				Point[] points = new Point[]
				{
					new Point(this.codeRectangle.X, this.codeRectangle.Y + this.codeRectangle.Height / 2),
					new Point(this.codeRectangle.X + 8, this.codeRectangle.Y + 3),
					new Point(this.codeRectangle.X + 8, this.codeRectangle.Y + this.codeRectangle.Height - 3)
				};
				int num4 = this.codeRectangle.X + this.codeRectangle.Width - 10;
				Point[] points2 = new Point[]
				{
					new Point(num4 + 2, this.codeRectangle.Y + 3),
					new Point(num4 + 2, this.codeRectangle.Y + this.codeRectangle.Height - 3),
					new Point(num4 + 10, this.codeRectangle.Y + this.codeRectangle.Height / 2)
				};
				g.FillPolygon(Brushes.White, points);
				g.FillPolygon(Brushes.White, points2);
				solidBrush.Color = SetInfo.RHColor.clItem;
				text = text2;
				num2 = rc.X + rc.Width - (int)g.MeasureString(text, font).Width - 10;
				if (num3 + height < rc.Y + rc.Height)
				{
					g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
				}
				num3 += height;
				font = font2;
				pen.Color = SetInfo.RHColor.clGrid;
				if (height < rc.Height)
				{
					g.DrawRectangle(pen, rc.X, rc.Y + height, rc.Width - 1, rc.Height - height);
				}
				font = new Font("宋体", 12f, FontStyle.Regular);
				num = (int)font.Size;
				height = font.Height;
				num2 = rc.X + 1;
				int num5 = num3;
				num3 += this.fontGap;
				if (num3 + height < rc.Y + rc.Height - this.buttonHight + 5)
				{
					g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Sell"), font, solidBrush, (float)num2, (float)num3);
				}
				num3 += height;
				g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
				num3 += this.fontGap;
				if (num3 + height < rc.Y + rc.Height - this.buttonHight + 5)
				{
					g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Buy"), font, solidBrush, (float)num2, (float)num3);
				}
				num3 += height;
				g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
				num3 += this.fontGap;
				if (num3 + height < rc.Y + rc.Height)
				{
					solidBrush.Color = SetInfo.RHColor.clItem;
					g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), font, solidBrush, (float)num2, (float)num3);
					g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignDiff"), font, solidBrush, (float)(num2 + rc.Width / 2), (float)num3);
					num3 += height;
					g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
				}
				string text3 = string.Empty;
				for (int i = 0; i < this.m_strItems.Length - 1; i++)
				{
					if (i % 2 == 0)
					{
						if (i == 0)
						{
							num3 += this.fontGap;
						}
						else
						{
							num3 += height;
						}
					}
					if (num3 + height < rc.Y + rc.Height)
					{
						if (this.stockM_htItemInfo[this.m_strItems[i]] == null)
						{
							text3 = "";
							g.DrawString(text3, font, solidBrush, (float)(num2 + rc.Width / 2), (float)num3);
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clItem;
							text3 = this.stockM_htItemInfo[this.m_strItems[i]].ToString();
							if (i % 2 == 0)
							{
								g.DrawString(text3, font, solidBrush, (float)num2, (float)num3);
								this.paintStockNumber(g, solidBrush, rc, product, this.m_strItems[i], text3, font, precision, true, num3);
							}
							else
							{
								g.DrawString(text3, font, solidBrush, (float)(num2 + rc.Width / 2), (float)num3);
								this.paintStockNumber(g, solidBrush, rc, product, this.m_strItems[i], text3, font, precision, false, num3);
							}
						}
					}
				}
				solidBrush.Color = SetInfo.RHColor.clItem;
				num3 += height;
				if (num3 < rc.Y + rc.Height)
				{
					g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
				}
				if (aBVOLS != null && aBVOLS.Length > 0)
				{
					num3 += this.fontGap;
					if (num3 + height < rc.Y + rc.Height + 1)
					{
						g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_AskVolume"), font, solidBrush, (float)num2, (float)num3);
						g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_BidVolume"), font, solidBrush, (float)(num2 + rc.Width / 2), (float)num3);
					}
					num3 += height;
					if (num3 < rc.Y + rc.Height)
					{
						g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
					}
				}
				g.DrawLine(pen, rc.X, rc.Y + rc.Height - 1, rc.X + rc.Width, rc.Y + rc.Height - 1);
				if (this.buttonUtils.CuRightrButtonName == "P_Btn")
				{
					num3 += this.fontGap;
					for (int j = iShowBuySellNum - 1; j >= 0; j--)
					{
						if (num3 + height < rc.Y + rc.Height - this.buttonHight + 5)
						{
							g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Sell") + this.pluginInfo.HQResourceManager.GetString("HQStr_Num" + (j + 1)), font, solidBrush, (float)num2, (float)num3);
						}
						num3 += height;
					}
					if (num3 < rc.Y + rc.Height - this.buttonHight)
					{
						g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
					}
					num3 += this.fontGap;
					for (int k = 0; k < iShowBuySellNum; k++)
					{
						if (num3 + height < rc.Y + rc.Height - this.buttonHight + 5)
						{
							g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Buy") + this.pluginInfo.HQResourceManager.GetString("HQStr_Num" + (k + 1)), font, solidBrush, (float)num2, (float)num3);
						}
						num3 += height;
					}
					if (num3 < rc.Y + rc.Height - this.buttonHight)
					{
						g.DrawLine(pen, rc.X, num3, rc.X + rc.Width, num3);
					}
				}
				if (product != null)
				{
					num3 = num5;
					if (num3 + height <= rc.Y + rc.Height)
					{
						float[] array = new float[5];
						float[] array2 = new float[5];
						float[] array3 = new float[5];
						float[] array4 = new float[5];
						string text4;
						if (product != null && product.realData != null)
						{
							try
							{
								array[0] = product.realData.buyPrice[0];
								array[1] = product.realData.buyPrice[1];
								array[2] = product.realData.buyPrice[2];
								array[3] = product.realData.buyPrice[3];
								array[4] = product.realData.buyPrice[4];
								array2[0] = (float)product.realData.buyAmount[0];
								array2[1] = (float)product.realData.buyAmount[1];
								array2[2] = (float)product.realData.buyAmount[2];
								array2[3] = (float)product.realData.buyAmount[3];
								array2[4] = (float)product.realData.buyAmount[4];
								array3[0] = product.realData.sellPrice[0];
								array3[1] = product.realData.sellPrice[1];
								array3[2] = product.realData.sellPrice[2];
								array3[3] = product.realData.sellPrice[3];
								array3[4] = product.realData.sellPrice[4];
								array4[0] = (float)product.realData.sellAmount[0];
								array4[1] = (float)product.realData.sellAmount[1];
								array4[2] = (float)product.realData.sellAmount[2];
								array4[3] = (float)product.realData.sellAmount[3];
								array4[4] = (float)product.realData.sellAmount[4];
							}
							catch (Exception ex)
							{
								Logger.wirte(MsgType.Error, ex.ToString());
							}
							float num6 = 0f;
							float num7 = 0f;
							for (int l = 0; l < iShowBuySellNum; l++)
							{
								num6 += array4[l];
								num7 += array2[l];
							}
							float num8 = num7 - num6;
							if ((double)(num7 + num6) < 0.001)
							{
								text = "—";
							}
							else
							{
								text = M_Common.FloatToString((double)(num8 / (num7 + num6) * 100f), 2) + "%";
							}
							if (num8 > 0f)
							{
								text = "+" + text;
								text4 = Convert.ToString((int)num8);
								solidBrush.Color = SetInfo.RHColor.clIncrease;
							}
							else
							{
								if (num8 < 0f)
								{
									text4 = Convert.ToString(-(int)num8);
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									text4 = "0";
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
						}
						else
						{
							text = "—";
							text4 = "—";
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						string text5;
						if (product != null && array3[0] > 0f)
						{
							solidBrush.Color = this.GetPriceColor(array3[0], product.realData.yesterBalancePrice);
							text5 = M_Common.FloatToString((double)array3[0], precision);
						}
						else
						{
							text5 = "—";
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text5, font).Width;
						g.DrawString(text5, font, solidBrush, (float)num2, (float)num3);
						if (product != null && array4[0] > 0f)
						{
							text5 = Convert.ToString((int)array4[0]);
						}
						else
						{
							text5 = "—";
						}
						solidBrush.Color = SetInfo.RHColor.clVolume;
						num2 = rc.X + rc.Width - (int)g.MeasureString(text5, font).Width;
						g.DrawString(text5, font, solidBrush, (float)num2, (float)num3);
						num3 += this.fontGap;
						num3 += height;
						string text6;
						if (product != null && array[0] > 0f)
						{
							solidBrush.Color = this.GetPriceColor(array[0], product.realData.yesterBalancePrice);
							text6 = M_Common.FloatToString((double)array[0], precision);
						}
						else
						{
							text6 = "—";
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text6, font).Width;
						g.DrawString(text6, font, solidBrush, (float)num2, (float)num3);
						if (product != null && array2[0] > 0f)
						{
							text6 = Convert.ToString((int)array2[0]);
						}
						else
						{
							text6 = "—";
						}
						solidBrush.Color = SetInfo.RHColor.clVolume;
						num2 = rc.X + rc.Width - (int)g.MeasureString(text6, font).Width;
						g.DrawString(text6, font, solidBrush, (float)num2, (float)num3);
						num3 += this.fontGap;
						num3 += this.fontGap;
						num3 += height;
						num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width;
						int num9 = (int)g.MeasureString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), font).Width;
						if (rc.X + num9 > rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width)
						{
							num2 = rc.X + num9;
						}
						g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						num2 = rc.X + rc.Width - (int)g.MeasureString(text4, font).Width;
						g.DrawString(text4, font, solidBrush, (float)num2, (float)num3);
						num3 += this.fontGap;
						int num10;
						if (this.m_strItems.Length % 2 == 0)
						{
							num10 = this.m_strItems.Length / 2;
						}
						else
						{
							num10 = (this.m_strItems.Length - 1) / 2;
						}
						num3 += height * num10 + this.fontGap;
						if (aBVOLS != null && aBVOLS.Length > 0)
						{
							num3 += this.fontGap;
							num3 += height;
							if (num3 + height > rc.Y + rc.Height)
							{
								return;
							}
							if (product != null && product.realData != null && product.realData.outAmount > 0)
							{
								text = Convert.ToString(product.realData.outAmount);
							}
							else
							{
								text = "—";
							}
							solidBrush.Color = SetInfo.RHColor.clVolume;
							num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width;
							g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
							if (product != null && product.realData != null && product.realData.inAmount > 0)
							{
								text = Convert.ToString(product.realData.inAmount);
							}
							else
							{
								text = "—";
							}
							solidBrush.Color = SetInfo.RHColor.clVolume;
							num2 = rc.X + rc.Width - (int)g.MeasureString(text, font).Width;
							g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						}
						if (this.buttonUtils.CuRightrButtonName == "P_Btn")
						{
							for (int m = iShowBuySellNum - 1; m >= 0; m--)
							{
								num3 += height;
								if (num3 + height > rc.Y + rc.Height - this.buttonHight + 4)
								{
									goto IL_146B;
								}
								if (product != null && array3[m] > 0f)
								{
									solidBrush.Color = this.GetPriceColor(array3[m], product.realData.yesterBalancePrice);
									text = M_Common.FloatToString((double)array3[m], precision);
								}
								else
								{
									text = "—";
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
								num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
								if (product != null && array4[m] > 0f)
								{
									text = Convert.ToString((int)array4[m]);
								}
								else
								{
									text = "—";
								}
								solidBrush.Color = SetInfo.RHColor.clVolume;
								num2 = rc.X + rc.Width - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
							}
							num3 += this.fontGap;
							for (int n = 0; n < iShowBuySellNum; n++)
							{
								num3 += height;
								if (num3 + height > rc.Y + rc.Height - this.buttonHight + 4)
								{
									goto IL_146B;
								}
								if (product != null && array[n] > 0f)
								{
									solidBrush.Color = this.GetPriceColor(array[n], product.realData.yesterBalancePrice);
									text = M_Common.FloatToString((double)array[n], precision);
								}
								else
								{
									text = "—";
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
								num2 = rc.X + rc.Width / 2 - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
								if (product != null && array2[n] > 0f)
								{
									text = Convert.ToString((int)array2[n]);
								}
								else
								{
									text = "—";
								}
								solidBrush.Color = SetInfo.RHColor.clVolume;
								num2 = rc.X + rc.Width - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num2, (float)num3);
							}
						}
						num3 += height;
						ProductData productData = hqForm.CurHQClient.GetProductData(hqForm.CurHQClient.curCommodityInfo);
						if (this.buttonUtils.CuRightrButtonName == "X_Btn")
						{
							Rectangle rc2 = new Rectangle(rc.X, num3, rc.Width, rc.Height - num3 - this.buttonHight);
							Draw_LastBill.Paint(g, rc2, productData, hqForm.CurHQClient);
						}
						if (this.buttonUtils.CuRightrButtonName == "T_Btn")
						{
							Draw_MinLine draw_MinLine = new Draw_MinLine(hqForm, false);
							Rectangle rc3 = new Rectangle(rc.X, num3, rc.Width, rc.Height - num3 - 2 * this.buttonHight);
							draw_MinLine.Paint(g, rc3, productData);
						}
						IL_146B:
						g.DrawLine(pen, rc.X, rc.Y + rc.Height - this.buttonHight - 1, rc.X + rc.Width, rc.Y + rc.Height - this.buttonHight - 1);
						this.rcRightButton = rc;
						this.rcRightButton.X = rc.X;
						this.rcRightButton.Y = rc.Y + rc.Height - this.buttonHight;
						this.rcRightButton.Height = this.buttonHight;
						this.rightbuttonGraph.rc = this.rcRightButton;
						if (this.buttonUtils.RightButtonList != null && this.buttonUtils.RightButtonList.Count > 0)
						{
							this.rightbuttonGraph.Paint(g, this.buttonUtils.RightButtonList, false);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.wirte(MsgType.Error, "Draw_Quote-Paint异常：" + ex2.Message);
			}
			finally
			{
				pen.Dispose();
				solidBrush.Dispose();
				font.Dispose();
			}
		}
		public Color GetPriceColor(float fPrice, float fBenchMark)
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
		public void MouseMove(int x, int y, HQForm m_hqForm)
		{
			if (this.codeRectangle.Contains(x, y))
			{
				m_hqForm.M_Cursor = Cursors.Hand;
				return;
			}
			m_hqForm.M_Cursor = Cursors.Default;
		}
		public void MouseLeftClick(int x, int y, HQForm m_hqForm, HQClientMain m_hqClient)
		{
			if (this.codeRectangle.Contains(x, y))
			{
				if (x < this.codeRectangle.X + 8)
				{
					m_hqForm.ChangeStock(true);
					m_hqForm.Repaint();
					return;
				}
				if (x > this.codeRectangle.X + this.codeRectangle.Width - 8)
				{
					m_hqForm.ChangeStock(false);
					m_hqForm.Repaint();
				}
			}
		}
		public void MouseDoubleClick(int x, int y, ProductData stock, HQForm m_hqForm, HQClientMain m_hqClient)
		{
			if (this.codeRectangle.Contains(x, y) && Tools.StrToBool((string)this.pluginInfo.HTConfig["QuoteMouseDoubleClick"], true) && x > this.codeRectangle.X + 10 && x < this.codeRectangle.X + this.codeRectangle.Width - 10 && stock.realData != null)
			{
				ProductDataVO commodityInfo = (ProductDataVO)stock.realData.Clone();
				InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs(commodityInfo);
				m_hqForm.MultiQuoteMouseLeftClick(this, e);
			}
		}
	}
}
