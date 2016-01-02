using System;
using System.Collections;
using System.Drawing;
using System.Text;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class ProductDataVO
	{
		public string region;
		public string industry;
		public string marketID;
		public string expStr;
		public DateTime time;
		public string code;
		public string name;
		public float yesterBalancePrice;
		public float closePrice;
		public float openPrice;
		public float highPrice;
		public float lowPrice;
		public float curPrice;
		public float upRate;
		public float shakeRate;
		public int curAmount;
		public int openAmount;
		public int closeAmount;
		public int reserveCount;
		public int reserveChange;
		public float balancePrice;
		public double totalMoney;
		public long totalAmount;
		public float[] buyPrice = new float[5];
		public float[] sellPrice = new float[5];
		public int[] buyAmount = new int[5];
		public int[] sellAmount = new int[5];
		public int outAmount;
		public int inAmount;
		public int tradeCue;
		public int no;
		public long averAmount5;
		public bool bUpdated = true;
		public float amountRate;
		public float consignRate;
		public float upRate5min;
		public bool isDraw;
		public int datahighlightTime;
		public Rectangle curPriceRectangle;
		public ArrayList billList = new ArrayList();
		public object Clone()
		{
			ProductDataVO productDataVO = new ProductDataVO();
			productDataVO.marketID = this.marketID;
			productDataVO.expStr = this.expStr;
			productDataVO.amountRate = this.amountRate;
			productDataVO.averAmount5 = this.averAmount5;
			productDataVO.balancePrice = this.balancePrice;
			productDataVO.billList = this.billList;
			for (int i = 0; i < this.buyAmount.Length; i++)
			{
				productDataVO.buyAmount[i] = this.buyAmount[i];
				productDataVO.buyPrice[i] = this.buyPrice[i];
				productDataVO.sellAmount[i] = this.sellAmount[i];
				productDataVO.sellPrice[i] = this.sellPrice[i];
			}
			productDataVO.closeAmount = this.closeAmount;
			productDataVO.closePrice = (float)this.closeAmount;
			productDataVO.code = this.code;
			productDataVO.consignRate = (float)this.closeAmount;
			productDataVO.curAmount = this.curAmount;
			productDataVO.curPrice = this.curPrice;
			productDataVO.highPrice = this.highPrice;
			productDataVO.inAmount = this.inAmount;
			productDataVO.lowPrice = this.lowPrice;
			productDataVO.name = this.name;
			productDataVO.no = this.no;
			productDataVO.openAmount = this.openAmount;
			productDataVO.openPrice = this.openPrice;
			productDataVO.outAmount = this.outAmount;
			productDataVO.reserveChange = this.reserveChange;
			productDataVO.reserveCount = this.reserveCount;
			productDataVO.time = this.time;
			productDataVO.totalAmount = this.totalAmount;
			productDataVO.totalMoney = this.totalMoney;
			productDataVO.tradeCue = this.tradeCue;
			productDataVO.upRate = (float)this.tradeCue;
			productDataVO.shakeRate = this.shakeRate;
			productDataVO.upRate5min = this.upRate5min;
			productDataVO.yesterBalancePrice = this.yesterBalancePrice;
			productDataVO.isDraw = this.isDraw;
			productDataVO.datahighlightTime = this.datahighlightTime;
			productDataVO.curPriceRectangle = this.curPriceRectangle;
			productDataVO.region = this.region;
			productDataVO.industry = this.industry;
			return productDataVO;
		}
		public bool Equals(ProductDataVO data)
		{
			if (data.balancePrice != this.balancePrice)
			{
				return false;
			}
			for (int i = 0; i < 5; i++)
			{
				if (data.buyAmount[i] != this.buyAmount[i])
				{
					return false;
				}
				if (data.buyPrice[i] != this.buyPrice[i])
				{
					return false;
				}
				if (data.sellAmount[i] != this.sellAmount[i])
				{
					return false;
				}
				if (data.sellPrice[i] != this.sellPrice[i])
				{
					return false;
				}
			}
			return data.closeAmount == this.closeAmount && data.closePrice == this.closePrice && data.code.Equals(this.code) && data.curAmount == this.curAmount && data.curPrice == this.curPrice && data.highPrice == this.highPrice && data.lowPrice == this.lowPrice && data.openAmount == this.openAmount && data.openPrice == this.openPrice && data.reserveCount == this.reserveCount && data.time.Equals(this.time) && data.totalAmount == this.totalAmount && data.totalMoney == this.totalMoney && data.yesterBalancePrice == this.yesterBalancePrice && !(data.industry != this.industry) && !(data.region != this.region);
		}
		public override string ToString()
		{
			string text = "\n";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("**ProductDataVO**" + text);
			stringBuilder.Append("Time:" + this.time + text);
			stringBuilder.Append("Code:" + this.code + text);
			stringBuilder.Append("Name:" + this.name + text);
			stringBuilder.Append("YesterPrice:" + this.yesterBalancePrice + text);
			stringBuilder.Append("ClosePrice:" + this.closePrice + text);
			stringBuilder.Append("OpenPrice:" + this.openPrice + text);
			stringBuilder.Append("HighPrice:" + this.highPrice + text);
			stringBuilder.Append("LowPrice:" + this.lowPrice + text);
			stringBuilder.Append("CurPrice:" + this.curPrice + text);
			stringBuilder.Append("CurAmount:" + this.curAmount + text);
			stringBuilder.Append("OpenAmount:" + this.openAmount + text);
			stringBuilder.Append("CloseAmount:" + this.closeAmount + text);
			stringBuilder.Append("ReserveCount:" + this.reserveCount + text);
			stringBuilder.Append("AverageValue:" + this.balancePrice + text);
			stringBuilder.Append("TotalMoney:" + this.totalMoney + text);
			stringBuilder.Append("TotalAmount:" + this.totalAmount + text);
			for (int i = 0; i < 5; i++)
			{
				stringBuilder.Append(string.Concat(new object[]
				{
					"BuyPrice",
					i + 1,
					":",
					this.buyPrice[i],
					text
				}));
				stringBuilder.Append(string.Concat(new object[]
				{
					"SellPrice",
					i + 1,
					":",
					this.sellPrice[i],
					text
				}));
				stringBuilder.Append(string.Concat(new object[]
				{
					"BuyAmount",
					i + 1,
					":",
					this.buyAmount[i],
					text
				}));
				stringBuilder.Append(string.Concat(new object[]
				{
					"SellAmount",
					i + 1,
					":",
					this.sellAmount[i],
					text
				}));
			}
			stringBuilder.Append("OutAmount:" + this.outAmount + text);
			stringBuilder.Append("InAmount:" + this.inAmount + text);
			stringBuilder.Append("TradeCue:" + this.tradeCue + text);
			stringBuilder.Append("NO:" + this.no + text);
			stringBuilder.Append("AverAmount5:" + this.averAmount5 + text);
			stringBuilder.Append("AmountRate:" + this.amountRate + text);
			stringBuilder.Append("Industry:" + this.industry + text);
			stringBuilder.Append("Region:" + this.region + text);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
	}
}
