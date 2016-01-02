using System;
using System.Collections;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class ProductData
	{
		public CommodityInfo commodityInfo;
		public ProductDataVO realData;
		public ArrayList aMinLine;
		public ArrayList aBill = ArrayList.Synchronized(new ArrayList());
		public ArrayList lastBill = ArrayList.Synchronized(new ArrayList());
		public KLineData[] dayKLine;
		public KLineData[] min5KLine;
		public KLineData[] min1KLine;
		public KLineData[] hrKLine;
	}
}
