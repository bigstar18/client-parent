// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.ProductDataVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;
using System.Collections;
using System.Drawing;
using System.Text;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class ProductDataVO
  {
    public float[] buyPrice = new float[5];
    public float[] sellPrice = new float[5];
    public int[] buyAmount = new int[5];
    public int[] sellAmount = new int[5];
    public bool bUpdated = true;
    public ArrayList billList = new ArrayList();
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
    public int outAmount;
    public int inAmount;
    public int tradeCue;
    public int no;
    public long averAmount5;
    public float amountRate;
    public float consignRate;
    public float upRate5min;
    public bool isDraw;
    public int datahighlightTime;
    public Rectangle curPriceRectangle;

    public object Clone()
    {
      ProductDataVO productDataVo = new ProductDataVO();
      productDataVo.marketID = this.marketID;
      productDataVo.expStr = this.expStr;
      productDataVo.amountRate = this.amountRate;
      productDataVo.averAmount5 = this.averAmount5;
      productDataVo.balancePrice = this.balancePrice;
      productDataVo.billList = this.billList;
      for (int index = 0; index < this.buyAmount.Length; ++index)
      {
        productDataVo.buyAmount[index] = this.buyAmount[index];
        productDataVo.buyPrice[index] = this.buyPrice[index];
        productDataVo.sellAmount[index] = this.sellAmount[index];
        productDataVo.sellPrice[index] = this.sellPrice[index];
      }
      productDataVo.closeAmount = this.closeAmount;
      productDataVo.closePrice = (float) this.closeAmount;
      productDataVo.code = this.code;
      productDataVo.consignRate = (float) this.closeAmount;
      productDataVo.curAmount = this.curAmount;
      productDataVo.curPrice = this.curPrice;
      productDataVo.highPrice = this.highPrice;
      productDataVo.inAmount = this.inAmount;
      productDataVo.lowPrice = this.lowPrice;
      productDataVo.name = this.name;
      productDataVo.no = this.no;
      productDataVo.openAmount = this.openAmount;
      productDataVo.openPrice = this.openPrice;
      productDataVo.outAmount = this.outAmount;
      productDataVo.reserveChange = this.reserveChange;
      productDataVo.reserveCount = this.reserveCount;
      productDataVo.time = this.time;
      productDataVo.totalAmount = this.totalAmount;
      productDataVo.totalMoney = this.totalMoney;
      productDataVo.tradeCue = this.tradeCue;
      productDataVo.upRate = (float) this.tradeCue;
      productDataVo.shakeRate = this.shakeRate;
      productDataVo.upRate5min = this.upRate5min;
      productDataVo.yesterBalancePrice = this.yesterBalancePrice;
      productDataVo.isDraw = this.isDraw;
      productDataVo.datahighlightTime = this.datahighlightTime;
      productDataVo.curPriceRectangle = this.curPriceRectangle;
      productDataVo.region = this.region;
      productDataVo.industry = this.industry;
      return (object) productDataVo;
    }

    public bool Equals(ProductDataVO data)
    {
      if ((double) data.balancePrice != (double) this.balancePrice)
        return false;
      for (int index = 0; index < 5; ++index)
      {
        if (data.buyAmount[index] != this.buyAmount[index] || (double) data.buyPrice[index] != (double) this.buyPrice[index] || (data.sellAmount[index] != this.sellAmount[index] || (double) data.sellPrice[index] != (double) this.sellPrice[index]))
          return false;
      }
      return data.closeAmount == this.closeAmount && (double) data.closePrice == (double) this.closePrice && (data.code.Equals(this.code) && data.curAmount == this.curAmount) && ((double) data.curPrice == (double) this.curPrice && (double) data.highPrice == (double) this.highPrice && ((double) data.lowPrice == (double) this.lowPrice && data.openAmount == this.openAmount)) && ((double) data.openPrice == (double) this.openPrice && data.reserveCount == this.reserveCount && (data.time.Equals(this.time) && data.totalAmount == this.totalAmount) && (data.totalMoney == this.totalMoney && (double) data.yesterBalancePrice == (double) this.yesterBalancePrice && (!(data.industry != this.industry) && !(data.region != this.region))));
    }

    public override string ToString()
    {
      string str = "\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("**ProductDataVO**" + str);
      stringBuilder.Append("Time:" + (object) this.time + str);
      stringBuilder.Append("Code:" + this.code + str);
      stringBuilder.Append("Name:" + this.name + str);
      stringBuilder.Append("YesterPrice:" + (object) this.yesterBalancePrice + str);
      stringBuilder.Append("ClosePrice:" + (object) this.closePrice + str);
      stringBuilder.Append("OpenPrice:" + (object) this.openPrice + str);
      stringBuilder.Append("HighPrice:" + (object) this.highPrice + str);
      stringBuilder.Append("LowPrice:" + (object) this.lowPrice + str);
      stringBuilder.Append("CurPrice:" + (object) this.curPrice + str);
      stringBuilder.Append("CurAmount:" + (object) this.curAmount + str);
      stringBuilder.Append("OpenAmount:" + (object) this.openAmount + str);
      stringBuilder.Append("CloseAmount:" + (object) this.closeAmount + str);
      stringBuilder.Append("ReserveCount:" + (object) this.reserveCount + str);
      stringBuilder.Append("AverageValue:" + (object) this.balancePrice + str);
      stringBuilder.Append("TotalMoney:" + (object) this.totalMoney + str);
      stringBuilder.Append("TotalAmount:" + (object) this.totalAmount + str);
      for (int index = 0; index < 5; ++index)
      {
        stringBuilder.Append("BuyPrice" + (object) (index + 1) + ":" + (string) (object) this.buyPrice[index] + str);
        stringBuilder.Append("SellPrice" + (object) (index + 1) + ":" + (string) (object) this.sellPrice[index] + str);
        stringBuilder.Append("BuyAmount" + (object) (index + 1) + ":" + (string) (object) this.buyAmount[index] + str);
        stringBuilder.Append("SellAmount" + (object) (index + 1) + ":" + (string) (object) this.sellAmount[index] + str);
      }
      stringBuilder.Append("OutAmount:" + (object) this.outAmount + str);
      stringBuilder.Append("InAmount:" + (object) this.inAmount + str);
      stringBuilder.Append("TradeCue:" + (object) this.tradeCue + str);
      stringBuilder.Append("NO:" + (object) this.no + str);
      stringBuilder.Append("AverAmount5:" + (object) this.averAmount5 + str);
      stringBuilder.Append("AmountRate:" + (object) this.amountRate + str);
      stringBuilder.Append("Industry:" + this.industry + str);
      stringBuilder.Append("Region:" + this.region + str);
      stringBuilder.Append(str);
      return stringBuilder.ToString();
    }
  }
}
