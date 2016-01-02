using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.IO;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDMarketSortVO : CMDVO
	{
		public int num;
		public CMDMarketSortVO()
		{
			this.cmd = 10;
		}
		public static MarketStatusVO[] getObj(InputStreamConvert input)
		{
			MarketStatusVO[] array = new MarketStatusVO[input.ReadJavaInt()];
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new MarketStatusVO();
					array[i].marketID = input.ReadJavaUTF();
					array[i].code = input.ReadJavaUTF();
					array[i].cur = input.ReadJavaFloat();
					array[i].status = input.ReadJavaByte();
					array[i].value = input.ReadJavaFloat();
				}
			}
			catch (IOException ex)
			{
				Logger.wirte(3, ex.StackTrace);
			}
			return array;
		}
	}
}
