// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.IndicatorPos
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

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
      this.m_Ratio = this.m_VirtualRatio <= 10 ? 1f / (float) (12 - this.m_VirtualRatio) : (float) (this.m_VirtualRatio - 10);
      if (this.m_EndPos < 0)
        this.m_EndPos = 0;
      if (this.m_EndPos >= kDataLen)
        this.m_EndPos = kDataLen - 1;
      this.m_ScreenCount = (int) ((double) width / (double) this.m_Ratio);
      if (this.m_ScreenCount < 0)
        this.m_ScreenCount = 0;
      if (kDataLen > this.m_ScreenCount)
      {
        this.m_MaxPos = kDataLen - this.m_ScreenCount;
        this.m_End = kDataLen - this.m_EndPos - 1;
        this.m_Begin = this.m_End + 1 - this.m_ScreenCount;
        if (this.m_Begin >= 0)
          return;
        this.m_Begin = 0;
        this.m_End = this.m_ScreenCount - 1;
        this.m_EndPos = kDataLen - this.m_ScreenCount;
      }
      else
      {
        this.m_Begin = this.m_MaxPos = this.m_EndPos = 0;
        this.m_End = kDataLen - 1;
      }
    }
  }
}
