using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDQuoteListVO : CMDVO
	{
		public CMDQuoteListVO()
		{
			this.cmd = 5;
		}
		public static ProductDataVO[] getObj(InputStreamConvert input)
		{
			ProductDataVO[] array = new ProductDataVO[input.ReadJavaInt()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ProductDataVO();
				array[i].marketID = input.ReadJavaUTF();
				array[i].code = input.ReadJavaUTF();
				array[i].yesterBalancePrice = input.ReadJavaFloat();
				array[i].closePrice = input.ReadJavaFloat();
				array[i].openPrice = input.ReadJavaFloat();
				array[i].highPrice = input.ReadJavaFloat();
				array[i].lowPrice = input.ReadJavaFloat();
				array[i].curPrice = input.ReadJavaFloat();
				array[i].totalAmount = input.ReadJavaLong();
				array[i].totalMoney = input.ReadJavaDouble();
				array[i].curAmount = input.ReadJavaInt();
				array[i].consignRate = input.ReadJavaFloat();
				array[i].amountRate = input.ReadJavaFloat();
				array[i].balancePrice = input.ReadJavaFloat();
				array[i].reserveCount = input.ReadJavaInt();
				array[i].buyAmount = new int[1];
				array[i].buyAmount[0] = input.ReadJavaInt();
				array[i].sellAmount = new int[1];
				array[i].sellAmount[0] = input.ReadJavaInt();
				array[i].buyPrice = new float[1];
				array[i].buyPrice[0] = input.ReadJavaFloat();
				array[i].sellPrice = new float[1];
				array[i].sellPrice[0] = input.ReadJavaFloat();
				array[i].expStr = input.ReadJavaUTF();
			}
			return array;
		}
	}
}
