using Gnnt.MEBS.HQModel.DataVO;
using System;
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
			XmlNode xmlNode = xmlDocument.SelectSingleNode("zxonliveimagemessage");
			XmlNodeList childNodes = xmlNode.ChildNodes;
			foreach (XmlNode xmlNode2 in childNodes)
			{
				XmlElement xmlElement = (XmlElement)xmlNode2;
				CommodityClassVO commodityClassVO = new CommodityClassVO();
				commodityClassVO.classID = xmlElement.GetAttribute("cc_classid");
				if (xmlElement.SelectSingleNode("cc_commodity_id") != null)
				{
					commodityClassVO.commodityID = xmlElement.SelectSingleNode("cc_commodity_id").InnerText;
				}
				if (xmlElement.SelectSingleNode("cc_name") != null)
				{
					commodityClassVO.name = xmlElement.SelectSingleNode("cc_name").InnerText;
				}
				if (xmlElement.SelectSingleNode("cc_fullname") != null)
				{
					commodityClassVO.fullName = xmlElement.SelectSingleNode("cc_fullname").InnerText;
				}
				if (xmlElement.SelectSingleNode("market") != null)
				{
					commodityClassVO.market = xmlElement.SelectSingleNode("market").InnerText;
				}
				if (xmlElement.SelectSingleNode("market_name") != null)
				{
					commodityClassVO.marketName = xmlElement.SelectSingleNode("market_name").InnerText;
				}
				if (xmlElement.SelectSingleNode("cc_desc") != null)
				{
					commodityClassVO.desc = xmlElement.SelectSingleNode("cc_desc").InnerText;
				}
				if (xmlElement.SelectSingleNode("cc_pricetype") != null)
				{
					commodityClassVO.priceType = xmlElement.SelectSingleNode("cc_pricetype").InnerText;
				}
				if (xmlElement.SelectSingleNode("market") != null)
				{
					commodityClassVO.remark = xmlElement.SelectSingleNode("market").InnerText;
				}
				ClassVO classVO = new ClassVO();
				classVO.classID = commodityClassVO.classID;
				classVO.name = commodityClassVO.name;
				classVO.fullName = commodityClassVO.fullName;
				MarketVO marketVO = new MarketVO();
				marketVO.market = commodityClassVO.market;
				marketVO.marketName = commodityClassVO.marketName;
				ArrayList arrayList = (ArrayList)this.htCommodityByClass[classVO.classID];
				ArrayList arrayList2 = (ArrayList)this.htCommodityByMarket[marketVO.market];
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					this.htCommodityByClass.Add(classVO.classID, arrayList);
					this.classList.Add(classVO);
				}
				if (arrayList2 == null)
				{
					arrayList2 = new ArrayList();
					this.htCommodityByMarket.Add(marketVO.market, arrayList2);
					this.marketList.Add(marketVO);
				}
				arrayList.Add(commodityClassVO);
				arrayList2.Add(commodityClassVO);
			}
			inStream.Close();
		}
	}
}
