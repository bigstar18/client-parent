// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDMinVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;
using System.Collections.Generic;
using System.IO;
using TPME.Log;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDMinVO : CMDVO
  {
    public byte type;
    public byte mark;
    public int date;
    public int time;
    public List<CommidityVO> commidityList;

    public CMDMinVO()
    {
      this.cmd = (byte) 6;
    }

    public static MinDataVO[] getObj(InputStreamConvert input)
    {
      MinDataVO[] minDataVoArray = new MinDataVO[input.ReadJavaInt()];
      try
      {
        for (int index = 0; index < minDataVoArray.Length; ++index)
        {
          minDataVoArray[index] = new MinDataVO();
          minDataVoArray[index].date = input.ReadJavaInt();
          minDataVoArray[index].time = input.ReadJavaInt();
          minDataVoArray[index].curPrice = input.ReadJavaFloat();
          minDataVoArray[index].totalAmount = input.ReadJavaLong();
          minDataVoArray[index].averPrice = input.ReadJavaFloat();
          minDataVoArray[index].reserveCount = input.ReadJavaInt();
        }
      }
      catch (IOException ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      return minDataVoArray;
    }
  }
}
