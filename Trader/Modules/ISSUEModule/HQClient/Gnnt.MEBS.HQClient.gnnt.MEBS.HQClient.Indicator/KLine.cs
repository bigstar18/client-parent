using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class KLine : IndicatorBase
	{
		public const int LineType_K = 0;
		public const int LineType_USA = 1;
		public const int LineType_POLY = 2;
		private int m_iLineType;
		private HQForm m_hqForm;
		public KLine(IndicatorPos pos, int iLineType, int Precision, HQForm hqForm) : base(pos, Precision)
		{
			this.m_strIndicatorName = "KLine";
			this.m_iLineType = iLineType;
			this.m_hqForm = hqForm;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.GetMaxMin();
			base.DrawCoordinate(g, this.m_iPrecision);
			switch (this.m_iLineType)
			{
			case 1:
				this.drawUSA(g);
				return;
			case 2:
				this.DrawPolyLine(g);
				return;
			default:
				this.DrawKLine(g);
				return;
			}
		}
		public override void Calculate()
		{
		}
		protected virtual void GetMaxMin()
		{
			if (this.m_pos.m_Begin > this.m_pos.m_End)
			{
				this.m_max = 0f;
				this.m_min = 0f;
				return;
			}
			this.m_max = 0f;
			this.m_min = 1E+08f;
			for (int i = this.m_pos.m_Begin; i <= this.m_pos.m_End; i++)
			{
				if (this.m_iLineType == 0 || this.m_iLineType == 1)
				{
					if (this.m_kData[i].highPrice > 0f)
					{
						if (this.m_kData[i].highPrice > this.m_max)
						{
							this.m_max = this.m_kData[i].highPrice;
						}
						if (this.m_kData[i].lowPrice < this.m_min)
						{
							this.m_min = this.m_kData[i].lowPrice;
						}
					}
				}
				else if (this.m_kData[i].closePrice > 0f)
				{
					if (this.m_kData[i].closePrice > this.m_max)
					{
						this.m_max = this.m_kData[i].closePrice;
					}
					if (this.m_kData[i].closePrice < this.m_min)
					{
						this.m_min = this.m_kData[i].closePrice;
					}
				}
			}
		}
		private void DrawKLine(Graphics g)
		{
			int begin = this.m_pos.m_Begin;
			int end = this.m_pos.m_End;
			if (this.m_max - this.m_min == 0f || this.m_rc.Height - this.m_iTextH <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Ratio < 3f) ? 0 : ((int)((this.m_pos.m_Ratio + 1f) / 3f));
			if (num % 2 == 0 && num > 0)
			{
				num--;
			}
			float num2 = (float)this.m_rc.X + this.m_pos.m_Ratio / 2f;
			float num3 = (this.m_max - this.m_min) / (float)(this.m_rc.Height - this.m_iTextH);
			int num4 = 0;
			int num5 = 0;
			for (int i = begin; i <= end; i++)
			{
				int num6 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].openPrice) / num3);
				int num7 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].highPrice) / num3);
				int num8 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].lowPrice) / num3);
				int num9 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].closePrice) / num3);
				if (this.m_kData[i].openPrice == this.m_kData[i].closePrice)
				{
					this.pen.Color = SetInfo.RHColor.clKLineEqual;
					g.DrawLine(this.pen, (int)num2 - num, num6, (int)num2 + num, num9);
					g.DrawLine(this.pen, (int)num2, num7, (int)num2, num8);
				}
				else if (this.m_kData[i].openPrice > this.m_kData[i].closePrice)
				{
					this.pen.Color = SetInfo.RHColor.clKLineDown;
					this.m_Brush.Color = SetInfo.RHColor.clKLineDown;
					g.DrawLine(this.pen, (int)num2, num7, (int)num2, num8);
					int num10 = num9 - num6;
					if (num10 == 0)
					{
						num10 = 1;
					}
					g.FillRectangle(this.m_Brush, (int)num2 - num, num6, 2 * num + 1, num10);
				}
				else
				{
					this.pen.Color = SetInfo.RHColor.clKLineUp;
					g.DrawLine(this.pen, (int)num2, num7, (int)num2, num9);
					g.DrawLine(this.pen, (int)num2, num6, (int)num2, num8);
					int num11 = num6 - num9;
					if (num11 == 0)
					{
						num11 = 1;
					}
					g.DrawRectangle(this.pen, (int)num2 - num, num9, 2 * num, num11);
				}
				if (this.m_max == this.m_kData[i].highPrice)
				{
					if (num4 == 0)
					{
						this.pen.Color = SetInfo.RHColor.clKLineEqual;
						g.DrawLine(this.pen, (int)num2, num7, (int)num2 + 15, num7 + 5);
						g.DrawString(this.m_max.ToString(), new Font("宋体", 10f, FontStyle.Bold), new SolidBrush(Color.White), new PointF(num2 + 15f, (float)num7));
					}
					num4++;
				}
				if (this.m_min == this.m_kData[i].lowPrice)
				{
					if (num5 == 0)
					{
						this.pen.Color = SetInfo.RHColor.clKLineEqual;
						g.DrawLine(this.pen, (int)num2, num8, (int)num2 + 15, num8 - 5);
						g.DrawString(this.m_min.ToString(), new Font("宋体", 10f, FontStyle.Bold), new SolidBrush(Color.White), new PointF(num2 + 15f, (float)(num8 - 10)));
					}
					num5++;
				}
				num2 += this.m_pos.m_Ratio;
			}
		}
		private void drawUSA(Graphics g)
		{
			int begin = this.m_pos.m_Begin;
			int end = this.m_pos.m_End;
			if (this.m_max - this.m_min == 0f || this.m_rc.Height - this.m_iTextH <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Ratio < 3f) ? 0 : ((int)((this.m_pos.m_Ratio + 1f) / 3f));
			if (num % 2 == 0 && num > 0)
			{
				num--;
			}
			float num2 = (float)this.m_rc.X + this.m_pos.m_Ratio / 2f;
			float num3 = (this.m_max - this.m_min) / (float)(this.m_rc.Height - this.m_iTextH);
			for (int i = begin; i <= end; i++)
			{
				int num4 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].openPrice) / num3);
				int y = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].highPrice) / num3);
				int y2 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].lowPrice) / num3);
				int num5 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].closePrice) / num3);
				this.pen.Color = SetInfo.RHColor.clUSALine;
				g.DrawLine(this.pen, (int)num2, y, (int)num2, y2);
				g.DrawLine(this.pen, (int)num2 - num, num4, (int)num2, num4);
				g.DrawLine(this.pen, (int)num2 + num + 1, num5, (int)num2, num5);
				num2 += this.m_pos.m_Ratio;
			}
		}
		private void DrawPolyLine(Graphics g)
		{
			int begin = this.m_pos.m_Begin;
			int end = this.m_pos.m_End;
			if (this.m_max - this.m_min == 0f || this.m_rc.Height - this.m_iTextH <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Ratio < 3f) ? 0 : ((int)((this.m_pos.m_Ratio + 1f) / 3f));
			if (num % 2 == 0 && num > 0)
			{
				num--;
			}
			float num2 = (float)this.m_rc.X + this.m_pos.m_Ratio / 2f;
			float num3 = (this.m_max - this.m_min) / (float)(this.m_rc.Height - this.m_iTextH);
			this.pen.Color = SetInfo.RHColor.clPolyLine;
			int num4 = -1;
			int num5 = -1;
			for (int i = begin; i <= end; i++)
			{
				int num6 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - this.m_kData[i].closePrice) / num3);
				if (num4 != -1 && num5 != -1)
				{
					g.DrawLine(this.pen, num4, num5, (int)num2, num6);
					if ((float)(num5 - num6) > num2 - (float)num4)
					{
						g.DrawLine(this.pen, num4 - 1, num5, (int)num2 - 1, num6);
						g.DrawLine(this.pen, num4 + 1, num5, (int)num2 + 1, num6);
					}
					else if ((float)(num6 - num5) > num2 - (float)num4)
					{
						g.DrawLine(this.pen, num4 - 1, num5, (int)num2 - 1, num6);
						g.DrawLine(this.pen, num4 + 1, num5, (int)num2 + 1, num6);
					}
					else
					{
						g.DrawLine(this.pen, num4, num5 - 1, (int)num2, num6 - 1);
						g.DrawLine(this.pen, num4, num5 + 1, (int)num2, num6 + 1);
					}
				}
				num4 = (int)num2;
				num5 = num6;
				num2 += this.m_pos.m_Ratio;
			}
		}
		public override void DrawCursor(Graphics g, int iPos)
		{
			int num = this.m_pos.m_Begin + iPos;
			if (num >= this.m_kData.Length)
			{
				return;
			}
			int num2 = (int)((float)(this.m_rc.Y + this.m_iTextH) + (this.m_max - this.m_kData[num].closePrice) * (float)(this.m_rc.Height - this.m_iTextH) / (this.m_max - this.m_min));
			GDIDraw.XorLine(g, this.m_rc.X, num2, this.m_rc.X + this.m_rc.Width, num2, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
		}
	}
}
