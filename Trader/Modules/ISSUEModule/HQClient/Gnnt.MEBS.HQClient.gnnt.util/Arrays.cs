using Gnnt.MEBS.HQModel.DataVO;
using System;
namespace Gnnt.MEBS.HQClient.gnnt.util
{
	internal class Arrays
	{
		public static void sort(ProductDataVO[] a, string strSortItem, string sortRules)
		{
			Arrays.QuickSort(a, 0, a.Length - 1, strSortItem, sortRules);
		}
		private static bool LessThan(ProductDataVO product, ProductDataVO other, string strSortItem, string sortRules)
		{
			if (strSortItem.Equals("Code"))
			{
				if (sortRules != null && sortRules.Length > 0)
				{
					int num = -1;
					int num2 = -1;
					string[] array = sortRules.Split(new char[]
					{
						';'
					});
					for (int i = 0; i < array.Length; i++)
					{
						if (product.code.StartsWith(array[i]))
						{
							num = i;
						}
						if (other.code.StartsWith(array[i]))
						{
							num2 = i;
						}
						if (num > -1 && num2 > -1)
						{
							break;
						}
					}
					if (num > num2)
					{
						return true;
					}
					if (num < num2)
					{
						return false;
					}
				}
				return product.code.CompareTo(other.code) < 0;
			}
			if (strSortItem.Equals("CurPrice"))
			{
				return product.curPrice < other.curPrice;
			}
			if (strSortItem.Equals("TotalAmount"))
			{
				return product.totalAmount < other.totalAmount;
			}
			if (strSortItem.Equals("UpValue"))
			{
				float num3 = (product.yesterBalancePrice > 0f && product.curPrice > 0f) ? (product.curPrice - product.yesterBalancePrice) : 0f;
				float num4 = (other.yesterBalancePrice > 0f && other.curPrice > 0f) ? (other.curPrice - other.yesterBalancePrice) : 0f;
				return num3 < num4;
			}
			if (strSortItem.Equals("UpRate"))
			{
				float num5 = (product.yesterBalancePrice == 0f) ? 0f : ((product.curPrice - product.yesterBalancePrice) / product.yesterBalancePrice * 100f);
				float num6 = (other.yesterBalancePrice == 0f) ? 0f : ((other.curPrice - other.yesterBalancePrice) / other.yesterBalancePrice * 100f);
				return num5 < num6;
			}
			if (strSortItem.Equals("TotalMoney"))
			{
				return product.totalMoney < other.totalMoney;
			}
			if (strSortItem.Equals("AmountRate"))
			{
				return product.amountRate < other.amountRate;
			}
			return !strSortItem.Equals("ConsignRate") || product.consignRate < other.consignRate;
		}
		private static void QuickSort(ProductDataVO[] proDataVO, int left, int right, string strSortItem, string sortRules)
		{
			if (left < right)
			{
				ProductDataVO productDataVO = proDataVO[left];
				int i = left;
				int num = right;
				while (i < num)
				{
					while (i < num && Arrays.LessThan(proDataVO[num], productDataVO, strSortItem, sortRules))
					{
						num--;
					}
					if (i < num)
					{
						proDataVO[i++] = proDataVO[num];
					}
					while (i < num && !Arrays.LessThan(proDataVO[i], productDataVO, strSortItem, sortRules))
					{
						i++;
					}
					if (i < num)
					{
						proDataVO[num--] = proDataVO[i];
					}
				}
				proDataVO[i] = productDataVO;
				Arrays.QuickSort(proDataVO, left, i - 1, strSortItem, sortRules);
				Arrays.QuickSort(proDataVO, i + 1, right, strSortItem, sortRules);
			}
		}
		private static void QuickSort(int[] a, int left, int right)
		{
			if (left < right)
			{
				int num = a[left];
				int i = left;
				int num2 = right;
				while (i < num2)
				{
					while (i < num2 && a[num2] < num)
					{
						num2--;
					}
					if (i < num2)
					{
						a[i++] = a[num2];
					}
					while (i < num2 && a[i] >= num)
					{
						i++;
					}
					if (i < num2)
					{
						a[num2--] = a[i];
					}
				}
				a[i] = num;
				Arrays.QuickSort(a, left, i - 1);
				Arrays.QuickSort(a, i + 1, right);
			}
		}
		private static void Reverse(int[] a)
		{
			int num = a.Length;
			int num2 = num / 2;
			for (int i = 0; i < num2; i++)
			{
				int num3 = a[i];
				a[i] = a[num - i - 1];
				a[num - i - 1] = num3;
			}
		}
	}
}
