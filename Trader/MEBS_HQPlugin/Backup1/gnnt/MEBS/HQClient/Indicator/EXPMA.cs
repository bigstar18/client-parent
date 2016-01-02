// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.EXPMA
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class EXPMA : KLine
  {
    private readonly int[] m_iParam = new int[3]
    {
      5,
      20,
      50
    };

    public EXPMA(IndicatorPos pos, int iPrecision, HQForm hqForm)
      : base(pos, 0, iPrecision, hqForm)
    {
      this.m_strIndicatorName = "EXPMA";
      this.m_strParamName = new string[this.m_iParam.Length];
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.m_strParamName[index] = "MA" + (object) this.m_iParam[index];
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      if (data == null || data.Length == 0)
        return;
      this.m_kData = data;
      this.Calculate();
      base.Paint(g, rc, data);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], 0, SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        this.m_data[index] = new float[this.m_kData.Length];
        this.GetEXPMA(this.m_iParam[index], this.m_data[index]);
      }
    }

    private void GetEXPMA(int n, float[] expma)
    {
      float num = 2f / (float) (n + 1);
      expma[0] = this.m_kData[0].closePrice;
      for (int index = 1; index < this.m_kData.Length; ++index)
        expma[index] = (this.m_kData[index].closePrice - expma[index - 1]) * num + expma[index - 1];
    }

    protected override void GetMaxMin()
    {
      base.GetMaxMin();
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index] - 1);
    }
  }
}
