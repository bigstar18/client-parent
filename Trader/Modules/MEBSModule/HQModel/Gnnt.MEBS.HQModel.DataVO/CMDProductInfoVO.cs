using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDProductInfoVO : CMDVO
	{
		public int date;
		public int time;
		public CMDProductInfoVO()
		{
			this.cmd = 3;
		}
		public static ProductInfoListVO getObj(InputStreamConvert input)
		{
			ProductInfoListVO productInfoListVO = new ProductInfoListVO();
			productInfoListVO.date = input.ReadJavaInt();
			productInfoListVO.time = input.ReadJavaInt();
			int num = input.ReadJavaInt();
			ProductInfoVO[] array = new ProductInfoVO[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ProductInfoVO();
				array[i].code = input.ReadJavaUTF();
				array[i].marketID = input.ReadJavaUTF();
				array[i].fUnit = input.ReadJavaFloat();
				array[i].name = input.ReadJavaUTF();
				array[i].status = input.ReadJavaInt();
				array[i].industry = input.ReadJavaUTF();
				array[i].region = input.ReadJavaUTF();
				array[i].pyName = new string[input.ReadJavaInt()];
				for (int j = 0; j < array[i].pyName.Length; j++)
				{
					array[i].pyName[j] = input.ReadJavaUTF();
				}
				array[i].tradeSecNo = new int[input.ReadJavaInt()];
				for (int k = 0; k < array[i].tradeSecNo.Length; k++)
				{
					array[i].tradeSecNo[k] = input.ReadJavaInt();
				}
				array[i].mPrice = input.ReadJavaFloat();
				if (array[i].fUnit <= 0f)
				{
					array[i].fUnit = 1f;
				}
			}
			productInfoListVO.productInfos = array;
			input = null;
			return productInfoListVO;
		}
	}
}
