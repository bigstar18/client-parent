// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.MultiQuoteItemInfo
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class MultiQuoteItemInfo
  {
    public string name;
    public int width;
    public int sortID;

    public MultiQuoteItemInfo(string name, int width, int sortID)
    {
      this.name = name;
      this.width = width;
      this.sortID = sortID;
    }
  }
}
