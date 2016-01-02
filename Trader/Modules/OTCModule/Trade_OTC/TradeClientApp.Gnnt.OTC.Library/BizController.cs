using System;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class BizController
	{
		public static int GetMinSpreadPriceCount(CommodityInfo commodityInfo)
		{
			int result = 0;
			try
			{
				string text = commodityInfo.Spread.ToString();
				if (text.Contains("."))
				{
					string text2 = text.Split(new char[]
					{
						'.'
					})[1];
					if (Convert.ToDouble(text2) > 0.0)
					{
						result = text2.Length;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return 0;
			}
			return result;
		}
		public static double CalculateFloatingProfit(double originPrice, double currentPrice, long qty, double unitQty, string buySell)
		{
			double result = 0.0;
			try
			{
				double num = (currentPrice - originPrice) * (double)qty * unitQty;
				if (buySell == BuySell.Buy.ToString("d"))
				{
					result = num;
				}
				else if (buySell == BuySell.Sell.ToString("d"))
				{
					result = -num;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return 0.0;
			}
			return result;
		}
		public static double CalculateInitRight(double initFund, double yesterdayBail)
		{
			return initFund + yesterdayBail;
		}
		public static double CalculateBalance(double initFund, double inOutFund, double fee, double yesterdayBail, double currentFL)
		{
			return initFund + inOutFund - fee + yesterdayBail + currentFL;
		}
		public static double CalculateInitFund(double Balance, double FloatingProfit)
		{
			return Balance + FloatingProfit;
		}
		public static double CalculateHoldingFund(double currentBail, double orderFrozenFund, double otherFrozenFund)
		{
			return currentBail;
		}
		public static double CalculateFrozenFund(double HoldingQuantity, double OpenPrice, double CtrtSize, double MarginValue)
		{
			return HoldingQuantity * OpenPrice * CtrtSize * MarginValue;
		}
		public static double CalculateRealFund(double InitFund, double CurrentBail, double FrozenFund, double UsingFund)
		{
			return InitFund - CurrentBail - FrozenFund - UsingFund;
		}
		public static double CalculateFundRisk(double InitFund, double CurrentBail)
		{
			double result;
			try
			{
				if (!Global.DoubleIsZero(CurrentBail))
				{
					result = InitFund / CurrentBail;
				}
				else
				{
					result = 0.0;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = 0.0;
			}
			return result;
		}
	}
}
