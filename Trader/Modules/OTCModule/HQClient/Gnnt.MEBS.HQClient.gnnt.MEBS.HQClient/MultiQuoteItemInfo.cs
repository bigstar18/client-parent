using System;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class MultiQuoteItemInfo
	{
		public string name;
		public int width;
		public int sortID;
		public MultiQuoteItemInfo(string name, int width, int sortID)
		{
			this.name = name;
			this.width = width;
			this.sortID = sortID;
		}
	}
}
