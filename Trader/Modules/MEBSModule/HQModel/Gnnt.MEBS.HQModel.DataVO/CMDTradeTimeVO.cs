using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.Collections;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDTradeTimeVO : CMDVO
	{
		public CMDTradeTimeVO()
		{
			this.cmd = 8;
		}
		public static Hashtable getObj(InputStreamConvert input)
		{
			Hashtable hashtable = new Hashtable();
			int num = input.ReadJavaInt();
			for (int i = 0; i < num; i++)
			{
				string text = input.ReadJavaUTF();
				MarketDataVO marketDataVO = new MarketDataVO();
				marketDataVO.marketID = text;
				int num2 = input.ReadJavaInt();
				TradeTimeVO[] array = new TradeTimeVO[num2];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = new TradeTimeVO();
					array[j].orderID = input.ReadJavaInt();
					array[j].beginDate = input.ReadJavaInt();
					array[j].beginTime = input.ReadJavaInt();
					array[j].endDate = input.ReadJavaInt();
					array[j].endTime = input.ReadJavaInt();
					array[j].tradeDate = input.ReadJavaInt();
					array[j].status = input.ReadJavaInt();
				}
				marketDataVO.m_timeRange = array;
				hashtable.Add(text, marketDataVO);
			}
			return hashtable;
		}
	}
}
