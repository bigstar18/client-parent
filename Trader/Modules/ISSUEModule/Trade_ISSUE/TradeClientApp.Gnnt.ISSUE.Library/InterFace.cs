using System;
using System.Reflection;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class InterFace
	{
		public delegate void CommodityInfoEventHander(object sender, InterFace.CommodityInfoEventArgs e);
		public class CommodityInfoEventArgs : EventArgs
		{
			private string commodityCode;
			private string eventInfo;
			public string CommodityCode
			{
				get
				{
					return this.commodityCode;
				}
			}
			public string EventInfo
			{
				get
				{
					return this.eventInfo;
				}
			}
			public CommodityInfoEventArgs(string commodityCode, string eventInfo)
			{
				this.commodityCode = commodityCode;
				this.eventInfo = eventInfo;
			}
		}
		public static bool TopLevel = true;
		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}
	}
}
