// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.MultiQuoteData
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel.DataVO;
using System.Collections;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class MultiQuoteData
  {
    public int iHighlightIndex = 1;
    public int buttonHight = 25;
    public ArrayList MyCommodityList = new ArrayList();
    public int HighlightTime = 2;
    public ProductDataVO[] m_curQuoteList = new ProductDataVO[0];
    public int MultiQuotePage;
    public int iStart;
    public int yChange;
  }
}
