using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class SortListVO
	{
		public const int SORT_KEY_MAX = 10;
		public const int SORT_BY_CODE = 0;
		public const int SORT_BY_PRICE = 1;
		public const int SORT_BY_UP = 2;
		public const int SORT_BY_UPRATE = 3;
		public const int SORT_BY_SHAKERATE = 4;
		public const int SORT_BY_AMOUNTRATE = 5;
		public const int SORT_BY_MONEY = 6;
		public const int SORT_BY_CONSIGNRATE = 7;
		public const int SORT_BY_MINRATE = 8;
		public const int SORT_BY_TOTALAMOUNT = 9;
		public int sortKey;
		public string[] codeList = new string[0];
		public object clone()
		{
			SortListVO sortListVO = new SortListVO();
			sortListVO.sortKey = this.sortKey;
			if (this.codeList != null)
			{
				sortListVO.codeList = new string[this.codeList.Length];
				for (int i = 0; i < sortListVO.codeList.Length; i++)
				{
					sortListVO.codeList[i] = this.codeList[i];
				}
			}
			return sortListVO;
		}
	}
}
