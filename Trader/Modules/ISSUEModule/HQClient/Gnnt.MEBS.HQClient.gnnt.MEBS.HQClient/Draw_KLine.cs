using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using Gnnt.MEBS.HQModel.OutInfo;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class Draw_KLine
	{
		public const int CYCLE_DAY = 1;
		public const int CYCLE_WEEK = 2;
		public const int CYCLE_MONTH = 3;
		public const int CYCLE_QUARTER = 4;
		public const int CYCLE_MIN1 = 5;
		public const int CYCLE_MIN3 = 6;
		public const int CYCLE_MIN5 = 7;
		public const int CYCLE_MIN15 = 8;
		public const int CYCLE_MIN30 = 9;
		public const int CYCLE_MIN60 = 10;
		public const int CYCLE_HOUR2 = 11;
		public const int CYCLE_HOUR4 = 12;
		public const int CYCLE_ANYDAY = 13;
		public const int CYCLE_ANYMIN = 14;
		private int cache_m_VirtualRatio = 15;
		private Page_KLine parent;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private ProductData m_product;
		private KLineData[] m_kData;
		private Rectangle[] m_rcPane = new Rectangle[3];
		private IndicatorBase[] m_indicator = new IndicatorBase[3];
		private IndicatorPos m_pos = new IndicatorPos();
		public int m_iPos = -1;
		private Rectangle m_rcLabel;
		private int m_iPrecision;
		private DateTime dateTimeRange;
		public Draw_KLine(Page_KLine _parent)
		{
			this.m_pos.m_VirtualRatio = this.cache_m_VirtualRatio;
			this.parent = _parent;
			this.pluginInfo = this.parent.pluginInfo;
			this.setInfo = this.parent.setInfo;
			int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
			if (this.parent.m_hqClient.isIndex(this.parent.m_hqClient.curCommodityInfo) && this.parent.m_hqClient.m_bShowIndexKLine == 0)
			{
				this.m_indicator[0] = new MA(this.m_pos, 2, precision, this.parent.m_hqForm);
			}
			else
			{
				this.m_indicator[0] = new MA(this.m_pos, _parent.m_globalData.m_iCurKLineType, precision, this.parent.m_hqForm);
			}
			this.m_indicator[1] = new VOL(this.m_pos, 0);
			this.CreateIndicator();
		}
		internal void Paint(Graphics g, Rectangle rc, ProductData product, int value)
		{
			try
			{
				this.m_product = product;
				if (product != null)
				{
					this.m_iPrecision = this.parent.m_hqClient.GetPrecision(this.m_product.commodityInfo);
				}
				this.MakeCycleData(value);
				this.GetScreen(g, rc);
				this.DrawCycle(g);
				if (this.m_rcPane[0].Width >= 0)
				{
					this.DrawTimeCoordinate(g);
					for (int i = 0; i < 3; i++)
					{
						if (this.m_indicator[i] != null)
						{
							this.m_indicator[i].Paint(g, this.m_rcPane[i], this.m_kData);
						}
						else
						{
							Logger.wirte(2, "m_indicator[" + i + "]为空！");
						}
					}
					if (!this.parent.m_hqForm.IsMultiCycle)
					{
						if (this.parent.isDrawCursor)
						{
							this.DrawLabel(g);
						}
						this.parent.m_hqForm.EndPaint();
						if (this.parent.isDrawCursor)
						{
							this.DrawCursor(-1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Draw_KLine-Paint异常：" + ex.StackTrace + ex.Message);
			}
		}
		private void GetScreen(Graphics g, Rectangle rc)
		{
			Font font = new Font("宋体", 10f, FontStyle.Regular);
			int height = font.Height;
			int num = rc.X + 4 * height;
			int num2 = rc.Width - 4 * height - 2;
			this.m_rcPane[0] = new Rectangle(num, rc.Y, num2, (rc.Height - height) / 2);
			this.m_rcPane[1] = new Rectangle(num, rc.Y + this.m_rcPane[0].Height, num2, (rc.Height - height) / 4);
			this.m_rcPane[2] = new Rectangle(num, this.m_rcPane[1].Y + this.m_rcPane[1].Height, num2, (rc.Height - height) / 4);
			Pen pen = new Pen(SetInfo.RHColor.clGrid);
			g.DrawRectangle(pen, this.m_rcPane[0].X, this.m_rcPane[0].Y, num2, rc.Height - height);
			g.DrawLine(pen, this.m_rcPane[1].X, this.m_rcPane[1].Y, this.m_rcPane[1].X + num2, this.m_rcPane[1].Y);
			g.DrawLine(pen, this.m_rcPane[2].X, this.m_rcPane[2].Y, this.m_rcPane[1].X + num2, this.m_rcPane[2].Y);
			pen.Dispose();
			int num3 = -1;
			if (this.m_iPos != -1)
			{
				num3 = this.m_pos.m_Begin + this.m_iPos;
			}
			if (this.m_kData != null)
			{
				this.m_pos.GetScreen(this.m_rcPane[0].Width, this.m_kData.Length);
			}
			else
			{
				this.m_pos.GetScreen(this.m_rcPane[0].Width, 0);
			}
			if (this.m_iPos != -1)
			{
				if (num3 >= this.m_pos.m_Begin && num3 <= this.m_pos.m_End)
				{
					this.m_iPos = num3 - this.m_pos.m_Begin;
				}
				else
				{
					this.m_iPos = -1;
				}
			}
			if ((this.parent.m_hqClient.m_iKLineCycle >= 5 && this.parent.m_hqClient.m_iKLineCycle <= 12) || this.parent.m_hqClient.m_iKLineCycle == 14)
			{
				this.m_rcLabel = new Rectangle(1, height, num - 1 - rc.X, height * 22);
			}
			else
			{
				this.m_rcLabel = new Rectangle(1, height, num - 1 - rc.X, height * 21);
			}
			font.Dispose();
		}
		private void DrawTimeCoordinate(Graphics g)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			Font font = new Font("宋体", 10f, FontStyle.Regular);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
			Pen pen = new Pen(SetInfo.RHColor.clGrid);
			int height = font.Height;
			Rectangle rectangle = new Rectangle(this.m_rcPane[2].X, this.m_rcPane[2].Y + this.m_rcPane[2].Height, this.m_rcPane[2].Width, height);
			int iKLineCycle = this.parent.m_hqClient.m_iKLineCycle;
			int num;
			switch (iKLineCycle)
			{
			case 1:
			case 2:
				break;
			case 3:
			case 4:
				num = (int)g.MeasureString("2004-10", font).Width;
				goto IL_117;
			default:
				if (iKLineCycle != 13)
				{
					num = (int)g.MeasureString("10-30 09:40", font).Width;
					goto IL_117;
				}
				break;
			}
			num = (int)g.MeasureString("2004-10-10", font).Width;
			IL_117:
			int num2 = (int)((double)num * 1.5 / (double)this.m_pos.m_Ratio) + 1;
			int num3 = this.m_pos.m_End - this.m_pos.m_Begin + 1;
			int num4 = (this.m_pos.m_End - this.m_pos.m_Begin) / num2;
			int y = rectangle.Y;
			int num5 = (int)((float)rectangle.X + this.m_pos.m_Ratio / 2f);
			g.DrawLine(pen, num5, rectangle.Y, num5, rectangle.Y + 5);
			string s = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[this.m_pos.m_Begin].date);
			g.DrawString(s, font, solidBrush, (float)num5, (float)y);
			for (int i = 1; i < num4; i++)
			{
				num5 = (int)((float)rectangle.X + (float)(i * num2) * this.m_pos.m_Ratio + this.m_pos.m_Ratio / 2f);
				g.DrawLine(pen, num5, rectangle.Y, num5, rectangle.Y + 5);
				s = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[i * num2 + this.m_pos.m_Begin].date);
				num5 -= num / 2;
				g.DrawString(s, font, solidBrush, (float)num5, (float)y);
			}
			if (num4 > 0)
			{
				num5 = rectangle.X + (int)((float)num3 * this.m_pos.m_Ratio - this.m_pos.m_Ratio / 2f);
				g.DrawLine(pen, num5, rectangle.Y, num5, rectangle.Y + 5);
				s = this.TimeToString(this.parent.m_hqClient.m_iKLineCycle, this.m_kData[this.m_pos.m_End].date);
				if (num4 > 1 || num5 + num > rectangle.X + rectangle.Width)
				{
					num5 -= num;
				}
				g.DrawString(s, font, solidBrush, (float)num5, (float)y);
			}
			pen.Dispose();
			solidBrush.Dispose();
			font.Dispose();
		}
		private string TimeToString(int iCycle, long date)
		{
			string text;
			switch (iCycle)
			{
			case 1:
			case 2:
				break;
			case 3:
			case 4:
				text = Convert.ToString(date / 100L);
				if (text.Length >= 6)
				{
					text = text.Substring(0, 4) + "-" + text.Substring(4, 2);
					return text;
				}
				return text;
			default:
				if (iCycle != 13)
				{
					text = Convert.ToString(date);
					if (text.Length >= 12)
					{
						text = text.Substring(4);
						text = string.Concat(new string[]
						{
							text.Substring(0, 2),
							"-",
							text.Substring(2, 2),
							" ",
							text.Substring(4, 2),
							":",
							text.Substring(6, 2)
						});
					}
					return text;
				}
				break;
			}
			text = Convert.ToString(date);
			if (text.Length >= 8)
			{
				text = string.Concat(new string[]
				{
					text.Substring(0, 4),
					"-",
					text.Substring(4, 2),
					"-",
					text.Substring(6, 2)
				});
			}
			return text;
		}
		private void MakeCycleData(int value)
		{
			if (this.m_product == null)
			{
				return;
			}
			if (1 == this.parent.m_hqClient.m_iKLineCycle || 2 == this.parent.m_hqClient.m_iKLineCycle || 3 == this.parent.m_hqClient.m_iKLineCycle || 4 == this.parent.m_hqClient.m_iKLineCycle || 13 == this.parent.m_hqClient.m_iKLineCycle)
			{
				this.MakeTodayDayLine();
			}
			else if (7 == this.parent.m_hqClient.m_iKLineCycle)
			{
				this.MakeToday5MinLine();
			}
			else
			{
				this.MakeToday1MinLine();
			}
			if (this.m_kData == null)
			{
				return;
			}
			switch (this.parent.m_hqClient.m_iKLineCycle)
			{
			case 1:
			case 5:
			case 7:
				break;
			case 2:
				this.MakeWeek();
				return;
			case 3:
				this.MakeMonth();
				return;
			case 4:
				this.MakeQUARTER();
				return;
			case 6:
				this.MakeMinCycle(3);
				return;
			case 8:
				this.MakeMinCycle(15);
				return;
			case 9:
				this.MakeMinCycle(30);
				return;
			case 10:
				this.MakeMinCycle(60);
				return;
			case 11:
				this.MakeMinCycle(120);
				return;
			case 12:
				this.MakeMinCycle(240);
				return;
			case 13:
				if (value == 0)
				{
					value = 1;
				}
				this.MakeDay(value);
				return;
			case 14:
				if (value == 0)
				{
					value = 1;
				}
				this.MakeMinCycle(value);
				break;
			default:
				return;
			}
		}
		private void MakeTodayDayLine()
		{
			if (this.m_product.realData == null || this.m_product.realData.curPrice < 0.001f)
			{
				this.m_kData = this.m_product.dayKLine;
				return;
			}
			int num;
			if (this.m_product.dayKLine == null)
			{
				num = 0;
			}
			else
			{
				num = this.m_product.dayKLine.Length;
			}
			if (this.parent.m_hqClient.TimeRange == null)
			{
				return;
			}
			if (num > 0 && this.m_product.dayKLine[num - 1].date > (long)this.parent.m_hqClient.TimeRange[0].tradeDate)
			{
				this.m_kData = this.m_product.dayKLine;
				return;
			}
			if (num > 0 && this.m_product.dayKLine[num - 1].date == (long)this.parent.m_hqClient.TimeRange[0].tradeDate)
			{
				if (this.m_product.realData.totalAmount <= 0L)
				{
					this.m_kData = this.m_product.dayKLine;
					return;
				}
				num--;
			}
			this.m_kData = new KLineData[num + 1];
			for (int i = 0; i < num; i++)
			{
				this.m_kData[i] = this.m_product.dayKLine[i];
			}
			this.m_kData[num] = new KLineData();
			this.m_kData[num].date = (long)this.parent.m_hqClient.TimeRange[0].tradeDate;
			this.m_kData[num].openPrice = this.m_product.realData.openPrice;
			this.m_kData[num].highPrice = this.m_product.realData.highPrice;
			this.m_kData[num].lowPrice = this.m_product.realData.lowPrice;
			this.m_kData[num].closePrice = this.m_product.realData.curPrice;
			this.m_kData[num].balancePrice = this.m_product.realData.balancePrice;
			this.m_kData[num].totalAmount = this.m_product.realData.totalAmount;
			this.m_kData[num].totalMoney = this.m_product.realData.totalMoney;
			this.m_kData[num].reserveCount = this.m_product.realData.reserveCount;
		}
		private void MakeToday1MinLine()
		{
			if (this.m_product.realData == null)
			{
				CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
				if (codeTable != null && (codeTable.status == 1 || codeTable.status == 6))
				{
					this.m_kData = this.m_product.min1KLine;
				}
				return;
			}
			KLineData[] array = this.get1MinKLine(this.m_product.commodityInfo, this.m_product.aBill, this.m_product.realData.yesterBalancePrice);
			if (this.m_product.min1KLine == null)
			{
				this.m_kData = array;
				return;
			}
			if (array.Length == 0)
			{
				this.m_kData = this.m_product.min1KLine;
				return;
			}
			int i;
			for (i = this.m_product.min1KLine.Length - 1; i >= 0; i--)
			{
				if (this.parent.m_hqClient.TimeRange.Length > 0)
				{
					long num = (long)this.parent.m_hqClient.TimeRange[0].beginDate * 10000L;
					num += (long)this.parent.m_hqClient.TimeRange[0].beginTime;
					if (this.m_product.min1KLine[i].date < num)
					{
						break;
					}
				}
			}
			i++;
			this.m_kData = new KLineData[i + array.Length];
			for (int j = 0; j < i; j++)
			{
				this.m_kData[j] = this.m_product.min1KLine[j];
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].reserveCount == 0 && array[k].totalAmount == 0L)
				{
					if (k == 0)
					{
						if (i > 0)
						{
							array[k].reserveCount = this.m_kData[i - 1].reserveCount;
						}
					}
					else
					{
						array[k].reserveCount = array[k - 1].reserveCount;
					}
				}
				this.m_kData[k + i] = array[k];
			}
		}
		private KLineData[] get1MinKLine(CommodityInfo commodityInfo, ArrayList aBillData, float fPreClosePrice)
		{
			if (aBillData == null)
			{
				return new KLineData[0];
			}
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
			TradeTimeVO[] timeRange = M_Common.getTimeRange(commodityInfo, this.parent.m_hqClient);
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			DateTime d = default(DateTime);
			float num = 0f;
			long num2 = 0L;
			for (int i = 0; i < aBillData.Count; i++)
			{
				BillDataVO billDataVO = (BillDataVO)aBillData[i];
				if (billDataVO == null)
				{
					break;
				}
				if (billDataVO.curPrice > 0f)
				{
					long date = (long)billDataVO.date * 10000L + (long)(billDataVO.time / 100);
					DateTime dateTimePlus = this.GetDateTimePlus(date, 1, false);
					if (dateTimePlus != d)
					{
						if (kLineData != null)
						{
							if (kLineData.totalAmount > 0L)
							{
								if (codeTable != null)
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
								}
								else
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
								}
							}
							arrayList.Add(kLineData);
						}
						kLineData = new KLineData();
						int num3 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("yyyy-MM-dd"));
						int num4 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("HH:mm:ss"));
						kLineData.date = (long)num3 * 10000L + (long)(num4 / 100);
						kLineData.openPrice = billDataVO.curPrice;
						kLineData.highPrice = billDataVO.curPrice;
						kLineData.lowPrice = billDataVO.curPrice;
						kLineData.closePrice = billDataVO.curPrice;
						kLineData.balancePrice = billDataVO.balancePrice;
						kLineData.reserveCount = billDataVO.reserveCount;
						kLineData.totalAmount += billDataVO.totalAmount - num2;
						kLineData.totalMoney += billDataVO.totalMoney - (double)num;
						d = dateTimePlus;
					}
					else
					{
						if (billDataVO.curPrice > kLineData.highPrice)
						{
							kLineData.highPrice = billDataVO.curPrice;
						}
						if (billDataVO.curPrice < kLineData.lowPrice)
						{
							kLineData.lowPrice = billDataVO.curPrice;
						}
						kLineData.closePrice = billDataVO.curPrice;
						kLineData.balancePrice = billDataVO.balancePrice;
						kLineData.reserveCount = billDataVO.reserveCount;
						kLineData.totalAmount += billDataVO.totalAmount - num2;
						kLineData.totalMoney += billDataVO.totalMoney - (double)num;
					}
					num2 = billDataVO.totalAmount;
					num = (float)billDataVO.totalMoney;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			KLineData[] array = new KLineData[0];
			if (arrayList.Count > 0)
			{
				int num5 = this.parent.m_hqClient.m_iTime / 100;
				int num6 = this.parent.m_hqClient.m_iTime % 100;
				if (num6 > 0)
				{
					int num7 = num5 / 100 * 60 + num5 % 100;
					num7++;
					num5 = num7 / 60 * 100 + num7 % 60;
					num5 %= 2400;
				}
				int num8 = TradeTimeVO.GetIndexFromTime(this.parent.m_hqClient.m_iDate, num5, timeRange) + 1;
				if (num8 == -1)
				{
					num8 = 0;
				}
				array = new KLineData[num8];
				int num9 = 0;
				int j = 0;
				while (j < num8)
				{
					DateTime dateTimeFromIndex = TradeTimeVO.GetDateTimeFromIndex(j, timeRange);
					num5 = TradeTimeVO.TimeStringToInt(dateTimeFromIndex.ToString("HH:mm"));
					long num10 = (long)(dateTimeFromIndex.Year * 10000 + dateTimeFromIndex.Month * 100 + dateTimeFromIndex.Day) * 10000L + (long)num5;
					if (num9 >= arrayList.Count)
					{
						goto IL_407;
					}
					kLineData = (KLineData)arrayList[num9];
					if (num10 != kLineData.date)
					{
						goto IL_407;
					}
					array[j] = kLineData;
					num9++;
					IL_521:
					j++;
					continue;
					IL_407:
					array[j] = new KLineData();
					array[j].date = num10;
					if (j == 0)
					{
						array[j].balancePrice = fPreClosePrice;
						KLineData arg_45F_0 = array[j];
						KLineData arg_458_0 = array[j];
						KLineData arg_44E_0 = array[j];
						array[j].closePrice = fPreClosePrice;
						arg_44E_0.lowPrice = fPreClosePrice;
						arg_458_0.highPrice = fPreClosePrice;
						arg_45F_0.openPrice = fPreClosePrice;
						array[j].reserveCount = 0;
						array[j].totalAmount = 0L;
						array[j].totalMoney = 0.0;
						goto IL_521;
					}
					array[j].balancePrice = array[j - 1].balancePrice;
					array[j].openPrice = (array[j].highPrice = (array[j].lowPrice = (array[j].closePrice = array[j - 1].closePrice)));
					array[j].reserveCount = array[j - 1].reserveCount;
					array[j].totalAmount = 0L;
					array[j].totalMoney = 0.0;
					goto IL_521;
				}
			}
			return array;
		}
		private void MakeToday5MinLine()
		{
			if (this.m_product.realData == null)
			{
				CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
				if (codeTable != null && (codeTable.status == 1 || codeTable.status == 6))
				{
					this.m_kData = this.m_product.min5KLine;
				}
				return;
			}
			KLineData[] array = this.get5MinKLine(this.m_product.commodityInfo, this.m_product.aBill, this.m_product.realData.yesterBalancePrice);
			if (this.m_product.min5KLine == null)
			{
				this.m_kData = array;
				return;
			}
			if (array.Length == 0)
			{
				this.m_kData = this.m_product.min5KLine;
				return;
			}
			int i;
			for (i = this.m_product.min5KLine.Length - 1; i >= 0; i--)
			{
				if (this.parent.m_hqClient.TimeRange.Length > 0)
				{
					long num = (long)this.parent.m_hqClient.TimeRange[0].beginDate * 10000L;
					num += (long)this.parent.m_hqClient.TimeRange[0].beginTime;
					if (this.m_product.min5KLine[i].date < num)
					{
						break;
					}
				}
			}
			i++;
			this.m_kData = new KLineData[i + array.Length];
			for (int j = 0; j < i; j++)
			{
				this.m_kData[j] = this.m_product.min5KLine[j];
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].reserveCount == 0 && array[k].totalAmount == 0L)
				{
					if (k == 0)
					{
						if (i > 0)
						{
							array[k].reserveCount = this.m_kData[i - 1].reserveCount;
						}
					}
					else
					{
						array[k].reserveCount = array[k - 1].reserveCount;
					}
				}
				this.m_kData[k + i] = array[k];
			}
		}
		private KLineData[] get5MinKLine(CommodityInfo commodityInfo, ArrayList aBillData, float fPreClosePrice)
		{
			if (aBillData == null)
			{
				return new KLineData[0];
			}
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
			TradeTimeVO[] timeRange = M_Common.getTimeRange(commodityInfo, this.parent.m_hqClient);
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			DateTime d = default(DateTime);
			float num = 0f;
			long num2 = 0L;
			for (int i = 0; i < aBillData.Count; i++)
			{
				BillDataVO billDataVO = (BillDataVO)aBillData[i];
				if (billDataVO == null)
				{
					break;
				}
				if (billDataVO.curPrice > 0f)
				{
					long date = (long)billDataVO.date * 10000L + (long)(billDataVO.time / 100);
					DateTime dateTimePlus = this.GetDateTimePlus(date, 5, false);
					if (dateTimePlus != d)
					{
						if (kLineData != null)
						{
							if (kLineData.totalAmount > 0L)
							{
								if (codeTable != null)
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
								}
								else
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
								}
							}
							arrayList.Add(kLineData);
						}
						kLineData = new KLineData();
						int num3 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("yyyy-MM-dd"));
						int num4 = TradeTimeVO.TimeStringToInt(dateTimePlus.ToString("HH:mm:ss"));
						kLineData.date = (long)num3 * 10000L + (long)(num4 / 100);
						kLineData.openPrice = billDataVO.curPrice;
						kLineData.highPrice = billDataVO.curPrice;
						kLineData.lowPrice = billDataVO.curPrice;
						kLineData.closePrice = billDataVO.curPrice;
						kLineData.balancePrice = billDataVO.balancePrice;
						kLineData.reserveCount = billDataVO.reserveCount;
						kLineData.totalAmount += billDataVO.totalAmount - num2;
						kLineData.totalMoney += billDataVO.totalMoney - (double)num;
						d = dateTimePlus;
					}
					else
					{
						if (billDataVO.curPrice > kLineData.highPrice)
						{
							kLineData.highPrice = billDataVO.curPrice;
						}
						if (billDataVO.curPrice < kLineData.lowPrice)
						{
							kLineData.lowPrice = billDataVO.curPrice;
						}
						kLineData.closePrice = billDataVO.curPrice;
						kLineData.balancePrice = billDataVO.balancePrice;
						kLineData.reserveCount = billDataVO.reserveCount;
						kLineData.totalAmount += billDataVO.totalAmount - num2;
						kLineData.totalMoney += billDataVO.totalMoney - (double)num;
					}
					num2 = billDataVO.totalAmount;
					num = (float)billDataVO.totalMoney;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			KLineData[] array = new KLineData[0];
			if (arrayList.Count > 0)
			{
				int num5 = this.parent.m_hqClient.m_iTime / 100;
				int num6 = this.parent.m_hqClient.m_iTime % 100;
				if (num6 > 0)
				{
					int num7 = num5 / 100 * 60 + num5 % 100;
					num7++;
					num5 = num7 / 60 * 100 + num7 % 60;
					num5 %= 2400;
				}
				int indexFromTime = TradeTimeVO.GetIndexFromTime(this.parent.m_hqClient.m_iDate, num5, timeRange);
				int num8 = indexFromTime / 5;
				if (indexFromTime % 5 >= 0)
				{
					num8++;
				}
				array = new KLineData[num8];
				int num9 = 0;
				int j = 0;
				while (j < num8)
				{
					DateTime dateTimeFromIndex = TradeTimeVO.GetDateTimeFromIndex(j * 5 + 4, timeRange);
					num5 = TradeTimeVO.TimeStringToInt(dateTimeFromIndex.ToString("HH:mm"));
					long num10 = (long)(dateTimeFromIndex.Year * 10000 + dateTimeFromIndex.Month * 100 + dateTimeFromIndex.Day) * 10000L + (long)num5;
					if (num9 >= arrayList.Count)
					{
						goto IL_414;
					}
					kLineData = (KLineData)arrayList[num9];
					if (num10 != kLineData.date)
					{
						goto IL_414;
					}
					array[j] = kLineData;
					num9++;
					IL_52E:
					j++;
					continue;
					IL_414:
					array[j] = new KLineData();
					array[j].date = num10;
					if (j == 0)
					{
						array[j].balancePrice = fPreClosePrice;
						KLineData arg_46C_0 = array[j];
						KLineData arg_465_0 = array[j];
						KLineData arg_45B_0 = array[j];
						array[j].closePrice = fPreClosePrice;
						arg_45B_0.lowPrice = fPreClosePrice;
						arg_465_0.highPrice = fPreClosePrice;
						arg_46C_0.openPrice = fPreClosePrice;
						array[j].reserveCount = 0;
						array[j].totalAmount = 0L;
						array[j].totalMoney = 0.0;
						goto IL_52E;
					}
					array[j].balancePrice = array[j - 1].balancePrice;
					array[j].openPrice = (array[j].highPrice = (array[j].lowPrice = (array[j].closePrice = array[j - 1].closePrice)));
					array[j].reserveCount = array[j - 1].reserveCount;
					array[j].totalAmount = 0L;
					array[j].totalMoney = 0.0;
					goto IL_52E;
				}
			}
			return array;
		}
		private void MakeWeek()
		{
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			for (int i = 0; i < this.m_kData.Length; i++)
			{
				bool flag;
				if (kLineData != null)
				{
					DateTime dateTime = new DateTime((int)kLineData.date / 10000, (int)kLineData.date / 100 % 100, (int)kLineData.date % 100);
					int num = 0;
					if (dateTime.DayOfWeek == DayOfWeek.Monday)
					{
						num = 2;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Tuesday)
					{
						num = 3;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Wednesday)
					{
						num = 4;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Thursday)
					{
						num = 5;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Friday)
					{
						num = 6;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Saturday)
					{
						num = 7;
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
					{
						num = 1;
					}
					DateTime value = new DateTime((int)this.m_kData[i].date / 10000, (int)this.m_kData[i].date / 100 % 100, (int)this.m_kData[i].date % 100);
					int num2 = 0;
					if (value.DayOfWeek == DayOfWeek.Sunday)
					{
						num2 = 1;
					}
					else if (value.DayOfWeek == DayOfWeek.Monday)
					{
						num2 = 2;
					}
					else if (value.DayOfWeek == DayOfWeek.Tuesday)
					{
						num2 = 3;
					}
					else if (value.DayOfWeek == DayOfWeek.Wednesday)
					{
						num2 = 4;
					}
					else if (value.DayOfWeek == DayOfWeek.Thursday)
					{
						num2 = 5;
					}
					else if (value.DayOfWeek == DayOfWeek.Friday)
					{
						num2 = 6;
					}
					else if (value.DayOfWeek == DayOfWeek.Saturday)
					{
						num2 = 7;
					}
					if (num >= num2)
					{
						flag = true;
					}
					else
					{
						dateTime = dateTime.AddDays(7.0);
						dateTime.CompareTo(value);
						flag = (dateTime.CompareTo(value) < 0);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					if (kLineData != null)
					{
						if (kLineData.totalAmount > 0L)
						{
							if (codeTable != null)
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
							}
							else
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
							}
						}
						arrayList.Add(kLineData);
					}
					kLineData = new KLineData();
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.date = this.m_kData[i].date;
					kLineData.highPrice = this.m_kData[i].highPrice;
					kLineData.lowPrice = this.m_kData[i].lowPrice;
					kLineData.openPrice = this.m_kData[i].openPrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount = this.m_kData[i].totalAmount;
					kLineData.totalMoney = this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
				}
				else
				{
					kLineData.date = this.m_kData[i].date;
					if (this.m_kData[i].highPrice > kLineData.highPrice)
					{
						kLineData.highPrice = this.m_kData[i].highPrice;
					}
					if (this.m_kData[i].lowPrice < kLineData.lowPrice)
					{
						kLineData.lowPrice = this.m_kData[i].lowPrice;
					}
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount += this.m_kData[i].totalAmount;
					kLineData.totalMoney += this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			this.m_kData = new KLineData[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.m_kData[j] = (KLineData)arrayList[j];
			}
		}
		private void MakeMonth()
		{
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			int num = -1;
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
			for (int i = 0; i < this.m_kData.Length; i++)
			{
				int num2 = (int)this.m_kData[i].date / 100;
				if (num2 != num)
				{
					if (kLineData != null)
					{
						if (kLineData.totalAmount > 0L)
						{
							if (codeTable != null)
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
							}
							else
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
							}
						}
						arrayList.Add(kLineData);
					}
					kLineData = new KLineData();
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.highPrice = this.m_kData[i].highPrice;
					kLineData.lowPrice = this.m_kData[i].lowPrice;
					kLineData.openPrice = this.m_kData[i].openPrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount = this.m_kData[i].totalAmount;
					kLineData.totalMoney = this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
					kLineData.date = (long)(num2 * 100);
					num = num2;
				}
				else
				{
					if (this.m_kData[i].highPrice > kLineData.highPrice)
					{
						kLineData.highPrice = this.m_kData[i].highPrice;
					}
					if (this.m_kData[i].lowPrice < kLineData.lowPrice)
					{
						kLineData.lowPrice = this.m_kData[i].lowPrice;
					}
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount += this.m_kData[i].totalAmount;
					kLineData.totalMoney += this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			this.m_kData = new KLineData[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.m_kData[j] = (KLineData)arrayList[j];
			}
		}
		private void MakeQUARTER()
		{
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			int num = -1;
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
			for (int i = 0; i < this.m_kData.Length; i++)
			{
				int num2 = (int)this.m_kData[i].date / 100;
				if (num2 > num)
				{
					if (kLineData != null)
					{
						if (kLineData.totalAmount > 0L)
						{
							if (codeTable != null)
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
							}
							else
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
							}
						}
						arrayList.Add(kLineData);
					}
					kLineData = new KLineData();
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.highPrice = this.m_kData[i].highPrice;
					kLineData.lowPrice = this.m_kData[i].lowPrice;
					kLineData.openPrice = this.m_kData[i].openPrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount = this.m_kData[i].totalAmount;
					kLineData.totalMoney = this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
					kLineData.date = (long)(num2 * 100);
					num = num2 + 3;
				}
				else
				{
					if (this.m_kData[i].highPrice > kLineData.highPrice)
					{
						kLineData.highPrice = this.m_kData[i].highPrice;
					}
					if (this.m_kData[i].lowPrice < kLineData.lowPrice)
					{
						kLineData.lowPrice = this.m_kData[i].lowPrice;
					}
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount += this.m_kData[i].totalAmount;
					kLineData.totalMoney += this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			this.m_kData = new KLineData[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.m_kData[j] = (KLineData)arrayList[j];
			}
		}
		private void MakeMinCycle(int iMin)
		{
			try
			{
				int num = iMin - 1;
				this.dateTimeRange = default(DateTime);
				ArrayList arrayList = new ArrayList();
				KLineData kLineData = null;
				long num2 = -1L;
				CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
				for (int i = 0; i < this.m_kData.Length; i++)
				{
					long num3;
					if (this.m_kData.Length <= iMin)
					{
						num3 = this.m_kData[this.m_kData.Length - 1].date;
					}
					else if (num > this.m_kData.Length - 1)
					{
						num3 = this.m_kData[num - (iMin - 1)].date;
						DateTime dateTime = TradeTimeVO.HHmmToDateTime((int)(num3 / 10000L), (int)(num3 % 10000L));
						dateTime = dateTime.AddMinutes((double)iMin);
						dateTime = dateTime.AddMinutes(-1.0);
						dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
						int num4 = TradeTimeVO.TimeStringToInt(dateTime.ToString("yyyy-MM-dd"));
						int num5 = TradeTimeVO.TimeStringToInt(dateTime.ToString("HH:mm:ss"));
						num3 = (long)num4 * 10000L + (long)(num5 / 100);
					}
					else
					{
						num3 = this.m_kData[num].date;
					}
					if (num3 > num2)
					{
						if (kLineData != null)
						{
							if (kLineData.totalAmount > 0L)
							{
								if (codeTable != null)
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
								}
								else
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
								}
							}
							arrayList.Add(kLineData);
						}
						kLineData = new KLineData();
						kLineData.closePrice = this.m_kData[i].closePrice;
						kLineData.highPrice = this.m_kData[i].highPrice;
						kLineData.lowPrice = this.m_kData[i].lowPrice;
						kLineData.openPrice = this.m_kData[i].openPrice;
						kLineData.balancePrice = this.m_kData[i].balancePrice;
						kLineData.totalAmount = this.m_kData[i].totalAmount;
						kLineData.totalMoney = this.m_kData[i].totalMoney;
						kLineData.reserveCount = this.m_kData[i].reserveCount;
						kLineData.date = num3;
						num2 = num3;
					}
					else
					{
						if (this.m_kData[i].highPrice > kLineData.highPrice)
						{
							kLineData.highPrice = this.m_kData[i].highPrice;
						}
						if (this.m_kData[i].lowPrice < kLineData.lowPrice)
						{
							kLineData.lowPrice = this.m_kData[i].lowPrice;
						}
						kLineData.closePrice = this.m_kData[i].closePrice;
						kLineData.balancePrice = this.m_kData[i].balancePrice;
						kLineData.totalAmount += this.m_kData[i].totalAmount;
						kLineData.totalMoney += this.m_kData[i].totalMoney;
						kLineData.reserveCount = this.m_kData[i].reserveCount;
					}
					if (num - i == 0)
					{
						num += iMin;
					}
				}
				if (kLineData != null)
				{
					if (kLineData.totalAmount > 0L)
					{
						if (codeTable != null)
						{
							kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
						}
						else
						{
							kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
						}
					}
					arrayList.Add(kLineData);
				}
				this.m_kData = new KLineData[arrayList.Count];
				for (int j = 0; j < arrayList.Count; j++)
				{
					this.m_kData[j] = (KLineData)arrayList[j];
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("MakeMinCycle异常" + ex.Message);
			}
		}
		private void Make5MinCycle(int iMin)
		{
			this.dateTimeRange = default(DateTime);
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			long num = -1L;
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
			for (int i = 0; i < this.m_kData.Length; i++)
			{
				long currentDateTime = this.GetCurrentDateTime(this.m_kData[i].date, iMin);
				if (currentDateTime != num)
				{
					if (kLineData != null)
					{
						if (kLineData.totalAmount > 0L)
						{
							if (codeTable != null)
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
							}
							else
							{
								kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
							}
						}
						arrayList.Add(kLineData);
					}
					kLineData = new KLineData();
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.highPrice = this.m_kData[i].highPrice;
					kLineData.lowPrice = this.m_kData[i].lowPrice;
					kLineData.openPrice = this.m_kData[i].openPrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount = this.m_kData[i].totalAmount;
					kLineData.totalMoney = this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
					kLineData.date = currentDateTime;
					num = currentDateTime;
				}
				else
				{
					if (this.m_kData[i].highPrice > kLineData.highPrice)
					{
						kLineData.highPrice = this.m_kData[i].highPrice;
					}
					if (this.m_kData[i].lowPrice < kLineData.lowPrice)
					{
						kLineData.lowPrice = this.m_kData[i].lowPrice;
					}
					kLineData.closePrice = this.m_kData[i].closePrice;
					kLineData.balancePrice = this.m_kData[i].balancePrice;
					kLineData.totalAmount += this.m_kData[i].totalAmount;
					kLineData.totalMoney += this.m_kData[i].totalMoney;
					kLineData.reserveCount = this.m_kData[i].reserveCount;
				}
			}
			if (kLineData != null)
			{
				if (kLineData.totalAmount > 0L)
				{
					if (codeTable != null)
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
					}
					else
					{
						kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
					}
				}
				arrayList.Add(kLineData);
			}
			this.m_kData = new KLineData[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.m_kData[j] = (KLineData)arrayList[j];
			}
			if (iMin == 60)
			{
				this.m_product.hrKLine = this.m_kData;
			}
		}
		private void MakeDay(int iday)
		{
			int num = iday - 1;
			ArrayList arrayList = new ArrayList();
			KLineData kLineData = null;
			long num2 = -1L;
			CodeTable codeTable = (CodeTable)this.parent.m_hqClient.m_htProduct[this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode];
			try
			{
				for (int i = 0; i < this.m_kData.Length; i++)
				{
					long date;
					if (this.m_kData.Length <= iday)
					{
						date = this.m_kData[this.m_kData.Length - 1].date;
					}
					else if (num > this.m_kData.Length - 1)
					{
						date = this.m_kData[this.m_kData.Length - 1].date;
					}
					else
					{
						date = this.m_kData[num].date;
					}
					if (date > num2)
					{
						if (kLineData != null)
						{
							if (kLineData.totalAmount > 0L)
							{
								if (codeTable != null)
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
								}
								else
								{
									kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
								}
							}
							arrayList.Add(kLineData);
						}
						kLineData = new KLineData();
						kLineData.closePrice = this.m_kData[i].closePrice;
						kLineData.highPrice = this.m_kData[i].highPrice;
						kLineData.lowPrice = this.m_kData[i].lowPrice;
						kLineData.openPrice = this.m_kData[i].openPrice;
						kLineData.balancePrice = this.m_kData[i].balancePrice;
						kLineData.totalAmount = this.m_kData[i].totalAmount;
						kLineData.totalMoney = this.m_kData[i].totalMoney;
						kLineData.reserveCount = this.m_kData[i].reserveCount;
						kLineData.date = date;
						num2 = date;
					}
					else
					{
						if (this.m_kData[i].highPrice > kLineData.highPrice)
						{
							kLineData.highPrice = this.m_kData[i].highPrice;
						}
						if (this.m_kData[i].lowPrice < kLineData.lowPrice)
						{
							kLineData.lowPrice = this.m_kData[i].lowPrice;
						}
						kLineData.closePrice = this.m_kData[i].closePrice;
						kLineData.balancePrice = this.m_kData[i].balancePrice;
						kLineData.totalAmount += this.m_kData[i].totalAmount;
						kLineData.totalMoney += this.m_kData[i].totalMoney;
						kLineData.reserveCount = this.m_kData[i].reserveCount;
					}
					if (num - i == 0)
					{
						num += iday;
					}
				}
				if (kLineData != null)
				{
					if (kLineData.totalAmount > 0L)
					{
						if (codeTable != null)
						{
							kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount / (double)codeTable.fUnit);
						}
						else
						{
							kLineData.balancePrice = (float)(kLineData.totalMoney / (double)kLineData.totalAmount);
						}
					}
					arrayList.Add(kLineData);
				}
				this.m_kData = new KLineData[arrayList.Count];
				for (int j = 0; j < arrayList.Count; j++)
				{
					this.m_kData[j] = (KLineData)arrayList[j];
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "MakeDay异常" + ex.Message);
			}
		}
		private long GetCurrentDateTime(long date, int iMin)
		{
			return Convert.ToInt64(this.GetDateTimePlus(date, iMin, true).ToString("yyyyMMddHHmm"));
		}
		private DateTime GetDateTime(long date, int iMin, bool isHistoryData)
		{
			DateTime result = TradeTimeVO.HHmmToDateTime((int)(date / 10000L), (int)(date % 10000L));
			int num = 0;
			TradeTimeVO[] timeRange = M_Common.getTimeRange(this.m_product.commodityInfo, this.parent.m_hqClient);
			if (timeRange.Length > 0)
			{
				num = timeRange[0].beginTime / 100;
			}
			int num2 = -1;
			int i = 0;
			while (i < timeRange.Length)
			{
				DateTime dateTime = TradeTimeVO.HHmmssToDateTime(timeRange[i].beginDate, timeRange[i].beginTime * 100);
				DateTime value = TradeTimeVO.HHmmssToDateTime(timeRange[i].endDate, timeRange[i].endTime * 100);
				if (i == 0 && result.CompareTo(dateTime) == -1)
				{
					int arg_A2_0 = (int)date / 10000;
					if (!isHistoryData)
					{
						num2 = 4;
						result = dateTime;
						break;
					}
					num2 = 3;
					break;
				}
				else if (result.CompareTo(dateTime) >= 0 && result.CompareTo(value) <= 0)
				{
					if (result.CompareTo(dateTime) == 0)
					{
						num2 = 1;
						break;
					}
					if (result.CompareTo(value) == 0)
					{
						num2 = 2;
						break;
					}
					num2 = 0;
					break;
				}
				else
				{
					i++;
				}
			}
			if (num2 == -1)
			{
				for (int j = timeRange.Length - 1; j >= 0; j--)
				{
					DateTime dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[j].endDate, timeRange[j].endTime * 100);
					if (result.CompareTo(dateTime2) > 0)
					{
						num2 = 2;
						result = dateTime2;
						break;
					}
				}
			}
			if (num2 == 0 || num2 == 1)
			{
				if (iMin == 5)
				{
					result = result.AddMinutes((double)iMin);
					result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
					result = result.AddSeconds((double)(-(double)result.Second));
				}
				else if (iMin > 60)
				{
					int num3 = iMin / 60;
					int num4 = result.Hour;
					if (num4 == 22)
					{
						num4 = 22;
					}
					if (result.Minute == 0)
					{
						if ((num4 - num) % num3 != 0)
						{
							result = result.AddMinutes((double)iMin);
							result = result.AddHours((double)(-(double)((num4 - num) % num3)));
							result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
							result = result.AddSeconds((double)(-(double)result.Second));
						}
					}
					else
					{
						result = result.AddMinutes((double)iMin);
						result = result.AddHours((double)(-(double)((num4 - num) % num3)));
						result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
						result = result.AddSeconds((double)(-(double)result.Second));
					}
				}
				else if (result.Minute % iMin > 0)
				{
					result = result.AddMinutes((double)iMin);
					result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
					result = result.AddSeconds((double)(-(double)result.Second));
				}
			}
			else if (num2 == 2)
			{
				if (result.Minute % iMin > 0)
				{
					result = result.AddMinutes((double)iMin);
					result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
					result = result.AddSeconds((double)(-(double)result.Second));
				}
			}
			else if (num2 == 3)
			{
				if (result.Minute % iMin > 0)
				{
					result = result.AddMinutes((double)iMin);
					result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
					result = result.AddSeconds((double)(-(double)result.Second));
				}
			}
			else if (num2 == 4)
			{
				result = result.AddMinutes((double)iMin);
				result = result.AddMinutes((double)(-(double)(result.Minute % iMin)));
				result = result.AddSeconds((double)(-(double)result.Second));
			}
			return result;
		}
		private void DrawCycle(Graphics g)
		{
			string text = "";
			switch (this.parent.m_hqClient.m_iKLineCycle)
			{
			case 1:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr_DayLine");
				break;
			case 2:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr_WeekLine");
				break;
			case 3:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr_MonthLine");
				break;
			case 4:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr_QuarterLine");
				break;
			case 5:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__1MinLine");
				break;
			case 6:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__3MinLine");
				break;
			case 7:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinLine");
				break;
			case 8:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__15MinLine");
				break;
			case 9:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__30MinLine");
				break;
			case 10:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__60MinLine");
				break;
			case 11:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__2HrLine");
				break;
			case 12:
				text = this.pluginInfo.HQResourceManager.GetString("HQStr__4HrLine");
				break;
			case 13:
				text = this.parent.m_hqClient.KLineValue.ToString() + this.pluginInfo.HQResourceManager.GetString("HQStr_AnyDayLine");
				break;
			case 14:
				text = this.parent.m_hqClient.KLineValue.ToString() + this.pluginInfo.HQResourceManager.GetString("HQStr_AnyMinLine");
				break;
			}
			Font font = new Font("宋体", 10f, FontStyle.Regular);
			SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
			int num = this.m_rcPane[0].X - (int)g.MeasureString(text, font).Width - 1;
			int num2 = this.m_rcPane[0].Y + 2;
			g.DrawString(text, font, solidBrush, (float)num, (float)num2);
			solidBrush.Dispose();
			font.Dispose();
		}
		private void DrawLabel()
		{
			using (Graphics m_Graphics = this.parent.m_hqForm.M_Graphics)
			{
				this.parent.m_hqForm.TranslateTransform(m_Graphics);
				this.DrawLabel(m_Graphics);
			}
		}
		public void DrawCursor(int iNewPos)
		{
			using (Graphics m_Graphics = this.parent.m_hqForm.M_Graphics)
			{
				if (this.parent.m_hqForm.IsEndPaint)
				{
					if (this.m_iPos != -1)
					{
						int num = this.m_rcPane[0].X + (int)(this.m_pos.m_Ratio / 2f + (float)this.m_iPos * this.m_pos.m_Ratio);
						GDIDraw.XorLine(m_Graphics, num, this.m_rcPane[0].Y + 1, num, this.m_rcPane[2].Y + this.m_rcPane[2].Height - 1, SetInfo.RHColor.clCursor, this.parent.m_hqForm.ScrollOffset);
						this.m_indicator[0].DrawCursor(m_Graphics, this.m_iPos);
					}
					if (iNewPos != -1)
					{
						this.m_iPos = iNewPos;
						int num2 = this.m_rcPane[0].X + (int)(this.m_pos.m_Ratio / 2f + (float)this.m_iPos * this.m_pos.m_Ratio);
						GDIDraw.XorLine(m_Graphics, num2, this.m_rcPane[0].Y + 1, num2, this.m_rcPane[2].Y + this.m_rcPane[2].Height - 1, SetInfo.RHColor.clCursor, this.parent.m_hqForm.ScrollOffset);
						this.m_indicator[0].DrawCursor(m_Graphics, this.m_iPos);
					}
				}
			}
		}
		public void DrawLabel(Graphics g)
		{
			if (this.m_kData == null || this.m_kData.Length == 0 || g == null)
			{
				return;
			}
			int num;
			if (this.m_iPos < 0)
			{
				num = this.m_kData.Length - 1;
			}
			else
			{
				num = this.m_pos.m_Begin + this.m_iPos;
			}
			for (int i = 0; i < 3; i++)
			{
				if (this.m_indicator[i] != null)
				{
					this.m_indicator[i].DrawTitle(g, num);
				}
			}
			if (this.m_iPos < 0)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(this.m_rcLabel.X - 1, this.m_rcLabel.Y - 1, this.m_rcLabel.Width + 1, this.m_rcLabel.Height + 1);
			using (Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(SetInfo.RHColor.clBackGround);
					SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
					Pen pen = new Pen(SetInfo.RHColor.clNumber);
					graphics.DrawRectangle(pen, 0, 0, this.m_rcLabel.Width, this.m_rcLabel.Height);
					Font font = new Font("宋体", 10f, FontStyle.Regular);
					int num2 = 1;
					int num3 = 1;
					solidBrush.Color = SetInfo.RHColor.clItem;
					graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Date"), font, solidBrush, (float)num2, (float)num3);
					num3 += font.Height;
					if (num < this.m_kData.Length)
					{
						string text = Convert.ToString(this.m_kData[num].date);
						int iKLineCycle = this.parent.m_hqClient.m_iKLineCycle;
						switch (iKLineCycle)
						{
						case 1:
						case 2:
							break;
						case 3:
						case 4:
							if (text.Length >= 6)
							{
								text = text.Substring(0, 6);
							}
							break;
						default:
							if (iKLineCycle != 13 && text.Length >= 4)
							{
								text = text.Substring(0, 8);
							}
							break;
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						solidBrush.Color = SetInfo.RHColor.clEqual;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						if ((this.parent.m_hqClient.m_iKLineCycle >= 5 && this.parent.m_hqClient.m_iKLineCycle <= 12) || this.parent.m_hqClient.m_iKLineCycle == 14)
						{
							text = Convert.ToString(this.m_kData[num].date);
							if (text.Length > 8)
							{
								text = text.Substring(8, 2) + ":" + text.Substring(10);
								num3 += font.Height;
								num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							}
							else
							{
								num3 += font.Height;
								num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							}
							graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						}
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Open"), font, solidBrush, (float)num2, (float)num3);
						float num4;
						if (num > 0)
						{
							num4 = this.m_kData[num - 1].balancePrice;
						}
						else
						{
							num4 = this.m_kData[num].openPrice;
						}
						num3 += font.Height;
						text = M_Common.FloatToString((double)this.m_kData[num].openPrice, this.m_iPrecision);
						if (this.m_kData[num].openPrice > num4)
						{
							solidBrush.Color = SetInfo.RHColor.clIncrease;
						}
						else if (this.m_kData[num].openPrice < num4)
						{
							solidBrush.Color = SetInfo.RHColor.clDecrease;
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_High"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = M_Common.FloatToString((double)this.m_kData[num].highPrice, this.m_iPrecision);
						if (this.m_kData[num].highPrice > num4)
						{
							solidBrush.Color = SetInfo.RHColor.clIncrease;
						}
						else if (this.m_kData[num].highPrice < num4)
						{
							solidBrush.Color = SetInfo.RHColor.clDecrease;
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Low"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = M_Common.FloatToString((double)this.m_kData[num].lowPrice, this.m_iPrecision);
						if (this.m_kData[num].lowPrice > num4)
						{
							solidBrush.Color = SetInfo.RHColor.clIncrease;
						}
						else if (this.m_kData[num].lowPrice < num4)
						{
							solidBrush.Color = SetInfo.RHColor.clDecrease;
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Close"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = M_Common.FloatToString((double)this.m_kData[num].closePrice, this.m_iPrecision);
						if (this.m_kData[num].closePrice > num4)
						{
							solidBrush.Color = SetInfo.RHColor.clIncrease;
						}
						else if (this.m_kData[num].closePrice < num4)
						{
							solidBrush.Color = SetInfo.RHColor.clDecrease;
						}
						else
						{
							solidBrush.Color = SetInfo.RHColor.clEqual;
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						bool flag = true;
						if (this.parent.m_hqClient.m_iKLineCycle != 1 && (this.parent.m_hqClient.getProductType(this.m_product.commodityInfo) == 2 || this.parent.m_hqClient.getProductType(this.m_product.commodityInfo) == 3))
						{
							flag = false;
						}
						num2 = rectangle.X + 1;
						num3 += font.Height;
						if (flag)
						{
							solidBrush.Color = SetInfo.RHColor.clItem;
							graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), font, solidBrush, (float)num2, (float)num3);
						}
						num3 += font.Height;
						if (flag)
						{
							text = M_Common.FloatToString((double)this.m_kData[num].balancePrice, this.m_iPrecision);
							if (this.m_kData[num].balancePrice > num4)
							{
								solidBrush.Color = SetInfo.RHColor.clIncrease;
							}
							else if (this.m_kData[num].balancePrice < num4)
							{
								solidBrush.Color = SetInfo.RHColor.clDecrease;
							}
							else
							{
								solidBrush.Color = SetInfo.RHColor.clEqual;
							}
							num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						}
						if (num > 0 && this.m_kData[num - 1] != null)
						{
							num2 = rectangle.X + 1;
							num3 += font.Height;
							solidBrush.Color = SetInfo.RHColor.clItem;
							graphics.DrawString("涨幅", font, solidBrush, (float)num2, (float)num3);
							string text2 = "";
							if (this.setInfo.AmountIncrease == "0" || this.setInfo.AmountIncrease == "2")
							{
								num3 += font.Height;
								text = M_Common.FloatToString((double)(this.m_kData[num].closePrice - this.m_kData[num - 1].balancePrice), this.m_iPrecision);
								if (this.m_kData[num - 1].balancePrice > 0f)
								{
									text2 = ((this.m_kData[num].closePrice - this.m_kData[num - 1].balancePrice) / this.m_kData[num - 1].balancePrice).ToString("P2");
								}
								if (this.m_kData[num].balancePrice - this.m_kData[num - 1].balancePrice > 0f)
								{
									solidBrush.Color = SetInfo.RHColor.clIncrease;
								}
								else if (this.m_kData[num].balancePrice - this.m_kData[num - 1].balancePrice < 0f)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
							else if (this.setInfo.AmountIncrease == "1")
							{
								num3 += font.Height;
								text = M_Common.FloatToString((double)(this.m_kData[num].closePrice - this.m_kData[num - 1].closePrice), this.m_iPrecision);
								if (this.m_kData[num - 1].balancePrice > 0f)
								{
									text2 = ((this.m_kData[num].closePrice - this.m_kData[num - 1].closePrice) / this.m_kData[num - 1].closePrice).ToString("P2");
								}
								if (this.m_kData[num].closePrice - this.m_kData[num - 1].closePrice > 0f)
								{
									solidBrush.Color = SetInfo.RHColor.clIncrease;
								}
								else if (this.m_kData[num].closePrice - this.m_kData[num - 1].closePrice < 0f)
								{
									solidBrush.Color = SetInfo.RHColor.clDecrease;
								}
								else
								{
									solidBrush.Color = SetInfo.RHColor.clEqual;
								}
							}
							num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
							graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
							num3 += font.Height;
							num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text2, font).Width - 1;
							graphics.DrawString(text2, font, solidBrush, (float)num2, (float)num3);
						}
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = Convert.ToString(this.m_kData[num].totalAmount);
						solidBrush.Color = SetInfo.RHColor.clVolume;
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Money"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = Convert.ToString((long)this.m_kData[num].totalMoney);
						solidBrush.Color = SetInfo.RHColor.clVolume;
						Font font2 = font;
						int num5 = (int)font.Size;
						while (g.MeasureString(text, font).Width > (float)rectangle.Width)
						{
							num5--;
							font = new Font("宋体", (float)num5, FontStyle.Regular);
						}
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						font = font2;
						num2 = rectangle.X + 1;
						num3 += font.Height;
						solidBrush.Color = SetInfo.RHColor.clItem;
						graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), font, solidBrush, (float)num2, (float)num3);
						num3 += font.Height;
						text = Convert.ToString(this.m_kData[num].reserveCount);
						solidBrush.Color = SetInfo.RHColor.clReserve;
						num2 = rectangle.X + rectangle.Width - (int)graphics.MeasureString(text, font).Width - 1;
						graphics.DrawString(text, font, solidBrush, (float)num2, (float)num3);
						font.Dispose();
						pen.Dispose();
						solidBrush.Dispose();
						g.DrawImage(bitmap, rectangle.X, rectangle.Y);
					}
				}
			}
		}
		public void CreateIndicator()
		{
			int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
			if (this.parent.m_hqClient.m_strIndicator.Equals("ASI"))
			{
				this.m_indicator[2] = new ASI(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("BIAS"))
			{
				this.m_indicator[2] = new BIAS(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("BRAR"))
			{
				this.m_indicator[2] = new BRAR(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("BOLL"))
			{
				this.m_indicator[2] = new BOLL(this.m_pos, precision, this.parent.m_hqForm);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("CCI"))
			{
				this.m_indicator[2] = new CCI(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("CR"))
			{
				this.m_indicator[2] = new CR(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("DMA"))
			{
				this.m_indicator[2] = new DMA(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("DMI"))
			{
				this.m_indicator[2] = new DMI(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("EMV"))
			{
				this.m_indicator[2] = new EMV(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("EXPMA"))
			{
				this.m_indicator[2] = new EXPMA(this.m_pos, precision, this.parent.m_hqForm);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("KDJ"))
			{
				this.m_indicator[2] = new KDJ(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("MACD"))
			{
				this.m_indicator[2] = new MACD(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("MIKE"))
			{
				this.m_indicator[2] = new MIKE(this.m_pos, precision, this.parent.m_hqForm);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("OBV"))
			{
				this.m_indicator[2] = new OBV(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("ORDER"))
			{
				this.m_indicator[2] = new Reserve(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("PSY"))
			{
				this.m_indicator[2] = new PSY(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("ROC"))
			{
				this.m_indicator[2] = new ROC(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("RSI"))
			{
				this.m_indicator[2] = new RSI(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("SAR"))
			{
				this.m_indicator[2] = new SAR(this.m_pos, precision, this.parent.m_hqForm);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("TRIX"))
			{
				this.m_indicator[2] = new TRIX(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("VR"))
			{
				this.m_indicator[2] = new VR(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("W%R"))
			{
				this.m_indicator[2] = new W_R(this.m_pos, precision);
				return;
			}
			if (this.parent.m_hqClient.m_strIndicator.Equals("WVAD"))
			{
				this.m_indicator[2] = new WVAD(this.m_pos, precision);
			}
		}
		internal bool MouseLeftClicked(int x, int y)
		{
			if (!this.parent.m_hqForm.IsEndPaint)
			{
				return false;
			}
			if (x < this.m_rcPane[0].X || x > this.m_rcPane[0].X + this.m_rcPane[0].Width)
			{
				return false;
			}
			int num = (int)((float)(x - this.m_rcPane[0].X) / this.m_pos.m_Ratio);
			if (num != this.m_iPos && num >= 0 && num <= this.m_pos.m_End - this.m_pos.m_Begin)
			{
				this.DrawCursor(num);
				this.DrawLabel();
			}
			if (this.parent.m_hqForm.IsMultiCycle)
			{
				num = this.getClickPointX(x, y);
				if (num != this.m_iPos)
				{
					this.DrawCursor(num);
					this.DrawLabel();
				}
			}
			return false;
		}
		public int getClickPointX(int x, int y)
		{
			if (!this.parent.m_hqForm.IsEndPaint)
			{
				return -1;
			}
			if (x < this.m_rcPane[0].X || x > this.m_rcPane[0].X + this.m_rcPane[0].Width)
			{
				return -1;
			}
			int num = (int)((float)(x - this.m_rcPane[0].X) / this.m_pos.m_Ratio);
			if (num <= this.m_pos.m_End - this.m_pos.m_Begin)
			{
				return num;
			}
			return this.m_pos.m_End - this.m_pos.m_Begin;
		}
		internal bool MouseDragged(int x, int y)
		{
			return this.MouseLeftClicked(x, y);
		}
		internal bool KeyPressed(KeyEventArgs e)
		{
			bool flag = false;
			if (!this.parent.isDrawCursor)
			{
				this.parent.isDrawCursor = true;
				this.m_iPos = -1;
			}
			if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Left)
			{
				this.m_pos.m_EndPos += this.m_pos.m_ScreenCount;
				this.m_iPos -= this.m_pos.m_ScreenCount;
				flag = true;
			}
			else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Left)
			{
				if (this.m_pos.m_Begin > 0)
				{
					this.m_pos.m_EndPos++;
					this.m_iPos--;
					flag = true;
				}
			}
			else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Right)
			{
				if (this.m_pos.m_Begin < this.m_pos.m_MaxPos)
				{
					this.m_pos.m_EndPos--;
					this.m_iPos++;
					flag = true;
				}
			}
			else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Right)
			{
				this.m_pos.m_EndPos -= this.m_pos.m_ScreenCount;
				this.m_iPos += this.m_pos.m_ScreenCount;
				flag = true;
			}
			if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
			{
				this.parent.isDrawCursor = false;
				this.m_iPos = -1;
			}
			Keys keyData = e.KeyData;
			if (keyData != Keys.Escape)
			{
				switch (keyData)
				{
				case Keys.End:
					this.ChangeIndicator(true);
					flag = true;
					break;
				case Keys.Home:
					this.ChangeIndicator(false);
					flag = true;
					break;
				case Keys.Left:
					flag = this.ChangePos(true);
					break;
				case Keys.Up:
					flag = this.ChangeRatio(true);
					if (flag)
					{
						if (!this.ChangeRatio(true))
						{
							this.parent.setMenuEnable("zoomin", false);
						}
						else
						{
							this.ChangeRatio(false);
						}
						this.parent.setMenuEnable("zoomout", true);
					}
					break;
				case Keys.Right:
					flag = this.ChangePos(false);
					break;
				case Keys.Down:
					flag = this.ChangeRatio(false);
					if (flag)
					{
						if (!this.ChangeRatio(false))
						{
							this.parent.setMenuEnable("zoomout", false);
						}
						else
						{
							this.ChangeRatio(true);
						}
						this.parent.setMenuEnable("zoomin", true);
					}
					break;
				default:
					if (keyData == Keys.F8)
					{
						this.ChangeCycle();
						flag = true;
					}
					break;
				}
			}
			else if (this.m_iPos != -1)
			{
				this.DrawCursor(-1);
				this.m_iPos = -1;
				flag = true;
			}
			return flag;
		}
		private void ChangeCycle()
		{
			if (this.parent.m_hqClient.m_iKLineCycle >= 12)
			{
				this.parent.m_hqClient.m_iKLineCycle = 1;
			}
			else
			{
				this.parent.m_hqClient.m_iKLineCycle++;
			}
			this.m_iPos = -1;
			this.m_pos.m_EndPos = 0;
			this.m_pos.m_MaxPos = 0;
			this.m_pos.m_End = 0;
			this.m_pos.m_Begin = 0;
			this.parent.AskForKLine();
		}
		public void ChangeKLineType(int iType)
		{
			int precision = this.parent.m_hqClient.GetPrecision(this.parent.m_hqClient.curCommodityInfo);
			this.m_indicator[0] = new MA(this.m_pos, iType, precision, this.parent.m_hqForm);
		}
		private bool ChangePos(bool bIsLeft)
		{
			bool result = false;
			if (!this.parent.m_hqForm.IsEndPaint)
			{
				return result;
			}
			if (this.m_kData == null)
			{
				return result;
			}
			if (this.m_kData.Length == 0)
			{
				return result;
			}
			if (this.m_iPos == -1)
			{
				if (bIsLeft)
				{
					this.DrawCursor(this.m_pos.m_End - this.m_pos.m_Begin);
				}
				else
				{
					this.DrawCursor(0);
				}
				this.DrawLabel();
			}
			else if (bIsLeft)
			{
				if (this.m_iPos == 0)
				{
					if (this.m_pos.m_Begin <= 0)
					{
						return false;
					}
					this.m_pos.m_EndPos++;
					result = true;
				}
				else
				{
					this.DrawCursor(this.m_iPos - 1);
					this.DrawLabel();
				}
			}
			else if (this.m_iPos == this.m_pos.m_End - this.m_pos.m_Begin)
			{
				if (this.m_pos.m_Begin >= this.m_pos.m_MaxPos)
				{
					return false;
				}
				result = true;
				this.m_pos.m_EndPos--;
			}
			else
			{
				this.DrawCursor(this.m_iPos + 1);
				this.DrawLabel();
			}
			return result;
		}
		public bool ChangeRatio(bool b)
		{
			if (b)
			{
				if (this.m_pos.m_VirtualRatio >= 60)
				{
					return false;
				}
				this.m_pos.m_VirtualRatio = this.m_pos.m_VirtualRatio + 1;
			}
			else
			{
				if (this.m_pos.m_VirtualRatio <= 10)
				{
					return false;
				}
				this.m_pos.m_VirtualRatio = this.m_pos.m_VirtualRatio - 1;
			}
			this.cache_m_VirtualRatio = this.m_pos.m_VirtualRatio;
			return true;
		}
		private void ChangeIndicator(bool bDown)
		{
			int num = -1;
			for (int i = 0; i < IndicatorBase.INDICATOR_NAME.GetLength(0); i++)
			{
				if (IndicatorBase.INDICATOR_NAME[i, 0].Equals(this.parent.m_hqClient.m_strIndicator))
				{
					num = i;
					break;
				}
			}
			if (bDown)
			{
				num = (num + 1) % IndicatorBase.INDICATOR_NAME.GetLength(0);
			}
			else if (num < 1)
			{
				num = IndicatorBase.INDICATOR_NAME.GetLength(0) - 1;
			}
			else
			{
				num = (num - 1) % IndicatorBase.INDICATOR_NAME.Length;
			}
			this.parent.m_hqClient.m_strIndicator = IndicatorBase.INDICATOR_NAME[num, 0];
			this.CreateIndicator();
		}
		private DateTime GetDateTimePlus(long date, int iMin, bool isHistoryData)
		{
			DateTime dateTime = TradeTimeVO.HHmmToDateTime((int)(date / 10000L), (int)(date % 10000L));
			int num = 0;
			TradeTimeVO[] timeRange = M_Common.getTimeRange(this.m_product.commodityInfo, this.parent.m_hqClient);
			if (timeRange.Length > 0)
			{
				num = timeRange[0].beginTime / 100;
			}
			int num2 = 1;
			int num3 = -1;
			DateTime dateTime2 = default(DateTime);
			DateTime d = default(DateTime);
			TimeSpan t = TimeSpan.Zero;
			int i = 0;
			while (i < timeRange.Length)
			{
				DateTime dateTime3 = TradeTimeVO.HHmmssToDateTime(timeRange[i].beginDate, timeRange[i].beginTime * 100);
				DateTime value = TradeTimeVO.HHmmssToDateTime(timeRange[i].endDate, timeRange[i].endTime * 100);
				if (i == 0 && dateTime.CompareTo(dateTime3) == -1)
				{
					int arg_BC_0 = (int)date / 10000;
					if (!isHistoryData)
					{
						num3 = 4;
						dateTime = dateTime3;
						break;
					}
					num3 = 3;
					break;
				}
				else if (dateTime.CompareTo(dateTime3) >= 0 && dateTime.CompareTo(value) <= 0)
				{
					num2 = i;
					if (dateTime.CompareTo(dateTime3) == 0)
					{
						num3 = 1;
						break;
					}
					if (dateTime.CompareTo(value) == 0)
					{
						num3 = 2;
						break;
					}
					num3 = 0;
					break;
				}
				else
				{
					i++;
				}
			}
			if (num3 == -1)
			{
				for (int j = timeRange.Length - 1; j >= 0; j--)
				{
					DateTime dateTime4 = TradeTimeVO.HHmmssToDateTime(timeRange[j].endDate, timeRange[j].endTime * 100);
					if (dateTime.CompareTo(dateTime4) > 0)
					{
						num3 = 2;
						dateTime = dateTime4;
						break;
					}
				}
			}
			if (num3 == 0 || num3 == 1)
			{
				if (iMin == 5 || iMin == 1 || iMin == 3)
				{
					dateTime = dateTime.AddMinutes((double)iMin);
					dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % iMin)));
					dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
				}
				else
				{
					if (iMin > 60)
					{
						if (dateTime.CompareTo(this.dateTimeRange) < 0)
						{
							dateTime = this.dateTimeRange;
							return dateTime;
						}
						int num4 = iMin / 60;
						int num5 = dateTime.Hour + num4;
						if (dateTime.Minute == 0)
						{
							if ((num5 - num) % num4 != 0)
							{
								dateTime = dateTime.AddMinutes((double)iMin);
								dateTime = dateTime.AddHours((double)(-(double)((num5 - num) % num4)));
								if (iMin % 60 == 0)
								{
									dateTime = dateTime.AddMinutes(0.0);
								}
								else if (iMin % 60 == 1)
								{
									dateTime = dateTime.AddMinutes(-1.0);
								}
								else
								{
									dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % (iMin % 60))));
								}
								dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
							}
						}
						else
						{
							dateTime = dateTime.AddMinutes((double)iMin);
							dateTime = dateTime.AddHours((double)(-(double)((num5 - num) % num4)));
							if (iMin % 60 == 0)
							{
								dateTime = dateTime.AddMinutes((double)(-(double)dateTime.Minute));
							}
							else if (iMin % 60 == 1)
							{
								dateTime = dateTime.AddMinutes(-1.0);
							}
							else
							{
								dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % (iMin % 60))));
							}
							dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
						}
					}
					else if (dateTime.Minute % iMin > 0)
					{
						dateTime = dateTime.AddMinutes((double)iMin);
						dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % iMin)));
						dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
					}
					TradeTimeVO.HHmmssToDateTime(timeRange[num2].beginDate, timeRange[num2].beginTime * 100);
					dateTime2 = TradeTimeVO.HHmmssToDateTime(timeRange[num2].endDate, timeRange[num2].endTime * 100);
					if (dateTime.CompareTo(dateTime2) > 0)
					{
						if (num2 + 1 >= timeRange.Length)
						{
							dateTime = dateTime2;
							return dateTime;
						}
						d = TradeTimeVO.HHmmssToDateTime(timeRange[num2 + 1].beginDate, timeRange[num2 + 1].beginTime * 100);
						TradeTimeVO.HHmmssToDateTime(timeRange[num2 + 1].endDate, timeRange[num2 + 1].endTime * 100);
						t = dateTime - dateTime2;
						dateTime = d + t;
						this.dateTimeRange = dateTime;
					}
				}
			}
			else if (num3 == 2)
			{
				if (dateTime.CompareTo(this.dateTimeRange) < 0)
				{
					dateTime = this.dateTimeRange;
					return dateTime;
				}
				if (dateTime.Minute % iMin > 0)
				{
					dateTime = dateTime.AddMinutes((double)iMin);
					dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % iMin)));
					dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
				}
			}
			else if (num3 == 3)
			{
				if (dateTime.Minute % iMin > 0)
				{
					dateTime = dateTime.AddMinutes((double)iMin);
					dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % iMin)));
					dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
				}
			}
			else if (num3 == 4)
			{
				dateTime = dateTime.AddMinutes((double)iMin);
				dateTime = dateTime.AddMinutes((double)(-(double)(dateTime.Minute % iMin)));
				dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
			}
			return dateTime;
		}
	}
}
