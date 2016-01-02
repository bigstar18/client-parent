// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.util.Arrays
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel.DataVO;

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
          int num1 = -1;
          int num2 = -1;
          string[] strArray = sortRules.Split(';');
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (product.code.StartsWith(strArray[index]))
              num1 = index;
            if (other.code.StartsWith(strArray[index]))
              num2 = index;
            if (num1 > -1 && num2 > -1)
              break;
          }
          if (num1 > num2)
            return true;
          if (num1 < num2)
            return false;
        }
        return product.code.CompareTo(other.code) < 0;
      }
      if (strSortItem.Equals("CurPrice"))
        return (double) product.curPrice < (double) other.curPrice;
      if (strSortItem.Equals("TotalAmount"))
        return product.totalAmount < other.totalAmount;
      if (strSortItem.Equals("UpValue"))
        return ((double) product.yesterBalancePrice <= 0.0 || (double) product.curPrice <= 0.0 ? 0.0 : (double) (product.curPrice - product.yesterBalancePrice)) < ((double) other.yesterBalancePrice <= 0.0 || (double) other.curPrice <= 0.0 ? 0.0 : (double) (other.curPrice - other.yesterBalancePrice));
      if (strSortItem.Equals("UpRate"))
        return ((double) product.yesterBalancePrice == 0.0 ? 0.0 : ((double) product.curPrice - (double) product.yesterBalancePrice) / (double) product.yesterBalancePrice * 100.0) < ((double) other.yesterBalancePrice == 0.0 ? 0.0 : ((double) other.curPrice - (double) other.yesterBalancePrice) / (double) other.yesterBalancePrice * 100.0);
      if (strSortItem.Equals("TotalMoney"))
        return product.totalMoney < other.totalMoney;
      if (strSortItem.Equals("AmountRate"))
        return (double) product.amountRate < (double) other.amountRate;
      if (strSortItem.Equals("ConsignRate"))
        return (double) product.consignRate < (double) other.consignRate;
      return true;
    }

    private static void QuickSort(ProductDataVO[] proDataVO, int left, int right, string strSortItem, string sortRules)
    {
      if (left >= right)
        return;
      ProductDataVO other = proDataVO[left];
      int index1 = left;
      int index2 = right;
      while (index1 < index2)
      {
        while (index1 < index2 && Arrays.LessThan(proDataVO[index2], other, strSortItem, sortRules))
          --index2;
        if (index1 < index2)
          proDataVO[index1++] = proDataVO[index2];
        while (index1 < index2 && !Arrays.LessThan(proDataVO[index1], other, strSortItem, sortRules))
          ++index1;
        if (index1 < index2)
          proDataVO[index2--] = proDataVO[index1];
      }
      proDataVO[index1] = other;
      Arrays.QuickSort(proDataVO, left, index1 - 1, strSortItem, sortRules);
      Arrays.QuickSort(proDataVO, index1 + 1, right, strSortItem, sortRules);
    }

    private static void QuickSort(int[] a, int left, int right)
    {
      if (left >= right)
        return;
      int num = a[left];
      int index1 = left;
      int index2 = right;
      while (index1 < index2)
      {
        while (index1 < index2 && a[index2] < num)
          --index2;
        if (index1 < index2)
          a[index1++] = a[index2];
        while (index1 < index2 && a[index1] >= num)
          ++index1;
        if (index1 < index2)
          a[index2--] = a[index1];
      }
      a[index1] = num;
      Arrays.QuickSort(a, left, index1 - 1);
      Arrays.QuickSort(a, index1 + 1, right);
    }

    private static void Reverse(int[] a)
    {
      int length = a.Length;
      int num1 = length / 2;
      for (int index = 0; index < num1; ++index)
      {
        int num2 = a[index];
        a[index] = a[length - index - 1];
        a[length - index - 1] = num2;
      }
    }
  }
}
