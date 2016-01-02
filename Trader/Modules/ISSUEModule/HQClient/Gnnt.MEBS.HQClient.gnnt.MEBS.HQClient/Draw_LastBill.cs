using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class Draw_LastBill
	{
		internal static void Paint(Graphics g, Rectangle rc, ProductData product, HQClientMain hqClientMain)
		{
			if (rc.Height <= 0)
			{
				return;
			}
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
			Pen pen = new Pen(SetInfo.RHColor.clGrid);
			Font font = new Font("宋体", 10f, FontStyle.Regular);
			try
			{
				int num = (int)g.MeasureString("tttttttttt", font).Width / 10;
				if (product != null && product.realData != null && product.aBill != null)
				{
					int productType = hqClientMain.getProductType(product.commodityInfo);
					int precision = hqClientMain.GetPrecision(product.commodityInfo);
					int num2 = 0;
					int num3 = rc.Height / font.Height;
					int count = product.lastBill.Count;
					if (count > 0)
					{
						if (count < num3)
						{
							num3 = count;
						}
						int num4 = rc.Y - 2;
						new ArrayList();
						for (int i = count - num3; i < count; i++)
						{
							int num5 = rc.X;
							if (i > product.lastBill.Count)
							{
								break;
							}
							BillDataVO billDataVO = null;
							if (i > 0)
							{
								billDataVO = (BillDataVO)product.lastBill[i - 1];
								if (billDataVO == null)
								{
									break;
								}
							}
							BillDataVO billDataVO2 = (BillDataVO)product.lastBill[i];
							if (billDataVO2 != null)
							{
								if (num2 != billDataVO2.time / 100)
								{
									g.DrawLine(new Pen(SetInfo.RHColor.clGrid)
									{
										DashStyle = DashStyle.Dash
									}, new Point(num5, num4), new Point(num5 + rc.Width, num4));
								}
								string text = TradeTimeVO.HHMMSSIntToString(billDataVO2.time);
								num2 = billDataVO2.time / 100;
								if (text.Length != 8)
								{
									text = "0" + text;
								}
								num5 = rc.X + 1;
								solidBrush.Color = SetInfo.RHColor.clNumber;
								g.DrawString(text, font, solidBrush, (float)num5, (float)num4);
								text = M_Common.FloatToString((double)billDataVO2.curPrice, precision);
								if (billDataVO2.curPrice > product.realData.yesterBalancePrice)
								{
									solidBrush.Color = SetInfo.RHColor.clIncrease;
								}
								else if (billDataVO2.curPrice < product.realData.yesterBalancePrice)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
								num5 += num * 16 - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num5, (float)num4);
								if (billDataVO == null)
								{
									text = Convert.ToString(billDataVO2.totalAmount);
								}
								else
								{
									text = Convert.ToString((int)(billDataVO2.totalAmount - billDataVO.totalAmount));
								}
								solidBrush.Color = SetInfo.RHColor.clVolume;
								num5 = rc.X + rc.Width - num * 2 - (int)g.MeasureString(text, font).Width;
								g.DrawString(text, font, solidBrush, (float)num5, (float)num4);
								if (productType != 2 && productType != 3)
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
										solidBrush.Color = SetInfo.RHColor.clIncrease;
									}
									else if (b == 1)
									{
										text = "↓";
										solidBrush.Color = SetInfo.RHColor.clDecrease;
									}
									else
									{
										text = "–";
										solidBrush.Color = SetInfo.RHColor.clEqual;
									}
									num5 = rc.X + rc.Width - num * 2;
									g.DrawString(text, font, solidBrush, (float)num5, (float)num4);
								}
								num4 += font.Height;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Draw_LastBill-Paint异常：" + ex.Message);
			}
			finally
			{
				pen.Dispose();
				solidBrush.Dispose();
				font.Dispose();
			}
		}
	}
}
