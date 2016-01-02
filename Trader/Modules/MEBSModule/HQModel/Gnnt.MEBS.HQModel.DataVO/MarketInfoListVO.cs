using System;
using System.Text;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class MarketInfoListVO
	{
		public int count;
		public MarketInfoVO[] marketInfos = new MarketInfoVO[0];
		public override string ToString()
		{
			string text = "\n";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("**MarketInfoListVO**" + text);
			stringBuilder.Append("count:" + this.count + text);
			for (int i = 0; i < this.marketInfos.Length; i++)
			{
				stringBuilder.Append(this.marketInfos[i].ToString());
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
	}
}
