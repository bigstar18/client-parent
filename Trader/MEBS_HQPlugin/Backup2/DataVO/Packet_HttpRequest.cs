// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.Packet_HttpRequest
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class Packet_HttpRequest
  {
    public const byte TYPE_DAYLINE = (byte) 0;
    public const byte TYPE_5MINLINE = (byte) 1;
    public const byte TYPE_1MINLINE = (byte) 2;
    public const byte TYPE_F10 = (byte) 2;
    public string marketID;
    public string sCode;
    public byte type;
  }
}
