// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.util.CommodityClass
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel.DataVO;
using System.Collections;
using System.IO;
using System.Xml;

namespace Gnnt.MEBS.HQClient.gnnt.util
{
  public class CommodityClass
  {
    public Hashtable htCommodityByClass = new Hashtable();
    public Hashtable htCommodityByMarket = new Hashtable();
    public ArrayList classList = new ArrayList();
    public ArrayList marketList = new ArrayList();

    public CommodityClass(Stream inStream)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(inStream);
      foreach (XmlElement xmlElement in xmlDocument.SelectSingleNode("zxonliveimagemessage").ChildNodes)
      {
        CommodityClassVO commodityClassVo = new CommodityClassVO();
        commodityClassVo.classID = xmlElement.GetAttribute("cc_classid");
        if (xmlElement.SelectSingleNode("cc_commodity_id") != null)
          commodityClassVo.commodityID = xmlElement.SelectSingleNode("cc_commodity_id").InnerText;
        if (xmlElement.SelectSingleNode("cc_name") != null)
          commodityClassVo.name = xmlElement.SelectSingleNode("cc_name").InnerText;
        if (xmlElement.SelectSingleNode("cc_fullname") != null)
          commodityClassVo.fullName = xmlElement.SelectSingleNode("cc_fullname").InnerText;
        if (xmlElement.SelectSingleNode("market") != null)
          commodityClassVo.market = xmlElement.SelectSingleNode("market").InnerText;
        if (xmlElement.SelectSingleNode("market_name") != null)
          commodityClassVo.marketName = xmlElement.SelectSingleNode("market_name").InnerText;
        if (xmlElement.SelectSingleNode("cc_desc") != null)
          commodityClassVo.desc = xmlElement.SelectSingleNode("cc_desc").InnerText;
        if (xmlElement.SelectSingleNode("cc_pricetype") != null)
          commodityClassVo.priceType = xmlElement.SelectSingleNode("cc_pricetype").InnerText;
        if (xmlElement.SelectSingleNode("market") != null)
          commodityClassVo.remark = xmlElement.SelectSingleNode("market").InnerText;
        ClassVO classVo = new ClassVO();
        classVo.classID = commodityClassVo.classID;
        classVo.name = commodityClassVo.name;
        classVo.fullName = commodityClassVo.fullName;
        MarketVO marketVo = new MarketVO();
        marketVo.market = commodityClassVo.market;
        marketVo.marketName = commodityClassVo.marketName;
        ArrayList arrayList1 = (ArrayList) this.htCommodityByClass[(object) classVo.classID];
        ArrayList arrayList2 = (ArrayList) this.htCommodityByMarket[(object) marketVo.market];
        if (arrayList1 == null)
        {
          arrayList1 = new ArrayList();
          this.htCommodityByClass.Add((object) classVo.classID, (object) arrayList1);
          this.classList.Add((object) classVo);
        }
        if (arrayList2 == null)
        {
          arrayList2 = new ArrayList();
          this.htCommodityByMarket.Add((object) marketVo.market, (object) arrayList2);
          this.marketList.Add((object) marketVo);
        }
        arrayList1.Add((object) commodityClassVo);
        arrayList2.Add((object) commodityClassVo);
      }
      inStream.Close();
    }
  }
}
