// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDBillByVersionVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;
using System.IO;
using TPME.Log;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDBillByVersionVO : CMDVO
  {
    public string marketID = string.Empty;
    public string code = string.Empty;
    public string ReservedField = string.Empty;
    public byte type;
    public long totalAmount;
    public long time;

    public CMDBillByVersionVO()
    {
      this.cmd = (byte) 7;
    }

    public static BillDataVO[] getObj(InputStreamConvert input, ref bool isConnection)
    {
      BillDataVO[] billDataVoArray = new BillDataVO[input.ReadJavaInt()];
      try
      {
        for (int index = 0; index < billDataVoArray.Length; ++index)
        {
          billDataVoArray[index] = new BillDataVO();
          billDataVoArray[index].date = input.ReadJavaInt();
          billDataVoArray[index].time = input.ReadJavaInt();
          billDataVoArray[index].reserveCount = input.ReadJavaInt();
          billDataVoArray[index].buyPrice = input.ReadJavaFloat();
          billDataVoArray[index].curPrice = input.ReadJavaFloat();
          billDataVoArray[index].sellPrice = input.ReadJavaFloat();
          billDataVoArray[index].totalAmount = input.ReadJavaLong();
          billDataVoArray[index].totalMoney = input.ReadJavaDouble();
          billDataVoArray[index].openAmount = input.ReadJavaInt();
          billDataVoArray[index].closeAmount = input.ReadJavaInt();
          billDataVoArray[index].balancePrice = input.ReadJavaFloat();
          billDataVoArray[index].tradeCue = input.ReadJavaUTF();
        }
      }
      catch (EndOfStreamException ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      catch (IOException ex)
      {
        isConnection = true;
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      return billDataVoArray;
    }
  }
}
