// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.MA
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class MA : KLine
  {
    private readonly int[] m_iParam = new int[5]
    {
      5,
      10,
      20,
      30,
      60
    };

    public MA(IndicatorPos pos, int iLineType, int iPrecision, HQForm hqForm)
      : base(pos, iLineType, iPrecision, hqForm)
    {
      this.m_strIndicatorName = "MA";
      this.m_strParamName = new string[this.m_iParam.Length];
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.m_strParamName[index] = "MA" + (object) this.m_iParam[index];
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      this.m_kData = data;
      this.Calculate();
      base.Paint(g, rc, data);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index] - 1, SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        this.m_data[index] = new float[this.m_kData.Length];
        this.AverageClose(this.m_iParam[index], this.m_data[index]);
      }
    }

    protected override void GetMaxMin()
    {
      base.GetMaxMin();
    }
  }
}
