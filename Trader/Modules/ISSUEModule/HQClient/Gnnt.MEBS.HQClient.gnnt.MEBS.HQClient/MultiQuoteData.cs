using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class MultiQuoteData
	{
		public int iHighlightIndex = 1;
		public int MultiQuotePage;
		public int buttonHight = 25;
		public ArrayList MyCommodityList = new ArrayList();
		public int iStart;
		public int HighlightTime = 2;
		public int yChange;
		public ProductDataVO[] m_curQuoteList = new ProductDataVO[0];
	}
}
