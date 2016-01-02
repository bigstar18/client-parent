using System;
using System.Text;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class ProductInfoListVO
	{
		public int date;
		public int time;
		public int count;
		public ProductInfoVO[] productInfos = new ProductInfoVO[0];
		public override string ToString()
		{
			string text = "\n";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("**ProductInfoListVO**" + text);
			stringBuilder.Append("date:" + this.date + text);
			stringBuilder.Append("time:" + this.time + text);
			stringBuilder.Append("count:" + this.count + text);
			for (int i = 0; i < this.productInfos.Length; i++)
			{
				stringBuilder.Append(this.productInfos[i].ToString());
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
	}
}
