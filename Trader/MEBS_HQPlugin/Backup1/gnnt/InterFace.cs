// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.InterFace
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Reflection;

namespace Gnnt.MEBS.HQClient.gnnt
{
  public class InterFace
  {
    public static bool TopLevel = true;

    public static string AssemblyVersion
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
      }
    }

    public delegate void CommodityInfoEventHander(object sender, InterFace.CommodityInfoEventArgs e);

    public class CommodityInfoEventArgs : EventArgs
    {
      private ProductDataVO commodityInfo;

      public ProductDataVO CommodityInfo
      {
        get
        {
          return this.commodityInfo;
        }
      }

      public CommodityInfoEventArgs(ProductDataVO commodityInfo)
      {
        this.commodityInfo = commodityInfo;
      }
    }
  }
}
