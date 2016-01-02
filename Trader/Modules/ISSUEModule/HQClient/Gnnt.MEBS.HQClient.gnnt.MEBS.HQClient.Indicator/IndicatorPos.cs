using System;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class IndicatorPos
	{
		public int m_VirtualRatio = 15;
		public float m_Ratio;
		public int m_ScreenCount;
		public int m_EndPos;
		public int m_MaxPos;
		public int m_End;
		public int m_Begin;
		public void GetScreen(int width, int kDataLen)
		{
			this.m_Ratio = ((this.m_VirtualRatio <= 10) ? (1f / (float)(12 - this.m_VirtualRatio)) : ((float)(this.m_VirtualRatio - 10)));
			if (this.m_EndPos < 0)
			{
				this.m_EndPos = 0;
			}
			if (this.m_EndPos >= kDataLen)
			{
				this.m_EndPos = kDataLen - 1;
			}
			this.m_ScreenCount = (int)((float)width / this.m_Ratio);
			if (this.m_ScreenCount < 0)
			{
				this.m_ScreenCount = 0;
			}
			if (kDataLen > this.m_ScreenCount)
			{
				this.m_MaxPos = kDataLen - this.m_ScreenCount;
				this.m_End = kDataLen - this.m_EndPos - 1;
				this.m_Begin = this.m_End + 1 - this.m_ScreenCount;
				if (this.m_Begin < 0)
				{
					this.m_Begin = 0;
					this.m_End = this.m_ScreenCount - 1;
					this.m_EndPos = kDataLen - this.m_ScreenCount;
					return;
				}
			}
			else
			{
				this.m_Begin = (this.m_MaxPos = (this.m_EndPos = 0));
				this.m_End = kDataLen - 1;
			}
		}
	}
}
