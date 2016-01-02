using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDQuoteVO : CMDVO
	{
		public string[,] codeList = new string[0, 0];
		public byte isAll;
		public int count;
		public CMDQuoteVO()
		{
			this.cmd = 4;
		}
		public static ProductDataVO[] getObj(InputStreamConvert input)
		{
			byte b = input.ReadJavaByte();
			int num = input.ReadJavaInt();
			ProductDataVO[] array = new ProductDataVO[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ProductDataVO();
				array[i].marketID = input.ReadJavaUTF();
				array[i].code = input.ReadJavaUTF();
				int yyyyMMdd = input.ReadJavaInt();
				int hHmmss = input.ReadJavaInt();
				array[i].time = TradeTimeVO.HHmmssToDateTime(yyyyMMdd, hHmmss);
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
				array[i].yesterBalancePrice = input.ReadJavaFloat();
				array[i].reserveChange = input.ReadJavaInt();
				if (b == 1)
				{
					array[i].inAmount = input.ReadJavaInt();
					array[i].outAmount = input.ReadJavaInt();
					array[i].buyAmount = new int[input.ReadJavaInt()];
					for (int j = 0; j < array[i].buyAmount.Length; j++)
					{
						array[i].buyAmount[j] = input.ReadJavaInt();
					}
					array[i].sellAmount = new int[input.ReadJavaInt()];
					for (int k = 0; k < array[i].sellAmount.Length; k++)
					{
						array[i].sellAmount[k] = input.ReadJavaInt();
					}
					array[i].buyPrice = new float[input.ReadJavaInt()];
					for (int l = 0; l < array[i].buyPrice.Length; l++)
					{
						array[i].buyPrice[l] = input.ReadJavaFloat();
					}
					array[i].sellPrice = new float[input.ReadJavaInt()];
					for (int m = 0; m < array[i].sellPrice.Length; m++)
					{
						array[i].sellPrice[m] = input.ReadJavaFloat();
					}
				}
				array[i].expStr = input.ReadJavaUTF();
			}
			return array;
		}
	}
}
