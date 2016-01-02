// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.BillFieldInfo
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class BillFieldInfo
  {
    public string name;
    public bool visible;
    public int width;

    public BillFieldInfo(string n, bool vis, int w)
    {
      this.name = n;
      this.visible = vis;
      this.width = w;
    }
  }
}
