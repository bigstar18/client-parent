// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDByConditionVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDByConditionVO : CMDVO
  {
    public int type;
    public long value;

    public CMDByConditionVO()
    {
      this.cmd = (byte) 11;
    }

    public static ProductDataVO[] getObj(InputStreamConvert input)
    {
      return CMDSortVO.getObj(input);
    }
  }
}
