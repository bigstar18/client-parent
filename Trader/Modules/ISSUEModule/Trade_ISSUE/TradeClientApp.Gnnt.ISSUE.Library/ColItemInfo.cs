using System;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class ColItemInfo
	{
		public string name;
		public int width;
		public string format;
		public int sortID;
		public ColItemInfo(string name, int width, string format, int sortID)
		{
			this.name = name;
			this.width = width;
			this.format = format;
			this.sortID = sortID;
		}
	}
}
