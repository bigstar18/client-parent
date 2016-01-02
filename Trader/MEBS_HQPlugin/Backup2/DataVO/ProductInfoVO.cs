// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.ProductInfoVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;
using System.Text;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class ProductInfoVO
  {
    public string marketID = string.Empty;
    public float fUnit = 1f;
    public int[] tradeSecNo = new int[1];
    public string cclass = "";
    public string ReservedField = string.Empty;
    public DateTime modifyTime = DateTime.Now;
    public string[] pyName = new string[0];
    public const int TYPE_INVALID = -1;
    public const int TYPE_COMMON = 0;
    public const int TYPE_CANCEL = 1;
    public const int TYPE_INDEX = 2;
    public const int TYPE_INDEX_MAIN = 3;
    public const int TYPE_SERIES = 4;
    public const int TYPE_PAUSE = 5;
    public const int TYPE_FINISHIED = 6;
    public string code;
    public string name;
    public int status;
    public string industry;
    public string region;
    public string notice;
    public int type;
    public int openTime;
    public float mPrice;
    public int closeTime;
    public string region_id;
    public string cclass_name;

    public override string ToString()
    {
      string str = "\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("**ProductInfoVO**" + str);
      stringBuilder.Append("Code:" + this.code + str);
      stringBuilder.Append("Name:" + this.name + str);
      stringBuilder.Append("Type:" + (object) this.type + str);
      stringBuilder.Append("Industry:" + this.industry + str);
      stringBuilder.Append("Region:" + this.region + str);
      stringBuilder.Append("CloseTime:" + (object) this.closeTime + str);
      stringBuilder.Append("CreateTime:" + (object) this.openTime + str);
      stringBuilder.Append("ModifyTime:" + (object) this.modifyTime + str);
      stringBuilder.Append("Status:" + (object) this.status + str);
      stringBuilder.Append("pyName.length:" + (object) this.pyName.Length + str);
      for (int index = 0; index < this.pyName.Length; ++index)
        stringBuilder.Append("pyName[" + (object) index + "]:" + this.pyName[index] + str);
      stringBuilder.Append(str);
      return stringBuilder.ToString() + base.ToString();
    }
  }
}
