﻿// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.MarketStatusVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class MarketStatusVO
  {
    public string marketID = string.Empty;
    public string code = string.Empty;
    public float cur;
    public byte status;
    public float value;

    public override string ToString()
    {
      return "\r\ncode:" + (object) this.code + "\r\ncur:" + (string) (object) this.cur + "\r\nstatus:" + (string) (object) this.status + "\r\nvalue:" + (string) (object) this.value;
    }
  }
}
