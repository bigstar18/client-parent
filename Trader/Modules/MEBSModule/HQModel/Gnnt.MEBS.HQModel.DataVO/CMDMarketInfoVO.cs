using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDMarketInfoVO : CMDVO
	{
		public int date;
		public int time;
		public CMDMarketInfoVO()
		{
			this.cmd = 17;
		}
		public static MarketInfoListVO getObj(InputStreamConvert input)
		{
			MarketInfoListVO marketInfoListVO = new MarketInfoListVO();
			int num = input.ReadJavaInt();
			MarketInfoVO[] array = new MarketInfoVO[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new MarketInfoVO();
				array[i].marketID = input.ReadJavaUTF();
				array[i].marketName = input.ReadJavaUTF();
			}
			marketInfoListVO.marketInfos = array;
			input = null;
			return marketInfoListVO;
		}
	}
}
