using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class KLineData
	{
		public long date;
		public float openPrice;
		public float closePrice;
		public float highPrice;
		public float lowPrice;
		public float balancePrice;
		public long totalAmount;
		public double totalMoney;
		public int reserveCount;
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"\r\ndate:",
				this.date,
				"\r\nopenPrice:",
				this.openPrice,
				"\r\nhighPrice:",
				this.highPrice,
				"\r\nlowPrice:",
				this.lowPrice,
				"\r\nclosePrice:",
				this.closePrice,
				"\r\ntotalAmount:",
				this.totalAmount,
				"\r\ntotalMoney:",
				this.totalMoney
			});
		}
	}
}
