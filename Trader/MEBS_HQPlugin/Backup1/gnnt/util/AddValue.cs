// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.util.AddValue
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

namespace Gnnt.MEBS.HQClient.gnnt.util
{
  internal class AddValue
  {
    private string m_Display;
    private string m_Value;

    public string Display
    {
      get
      {
        return this.m_Display;
      }
    }

    public string Value
    {
      get
      {
        return this.m_Value;
      }
    }

    public AddValue(string Display, string Value)
    {
      this.m_Display = Display;
      this.m_Value = Value;
    }
  }
}
