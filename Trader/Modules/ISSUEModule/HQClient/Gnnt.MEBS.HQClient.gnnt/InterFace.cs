using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Reflection;
namespace Gnnt.MEBS.HQClient.gnnt
{
	public class InterFace
	{
		public delegate void CommodityInfoEventHander(object sender, InterFace.CommodityInfoEventArgs e);
		public class CommodityInfoEventArgs : EventArgs
		{
			private ProductDataVO commodityInfo;
			public ProductDataVO CommodityInfo
			{
				get
				{
					return this.commodityInfo;
				}
			}
			public CommodityInfoEventArgs(ProductDataVO commodityInfo)
			{
				this.commodityInfo = commodityInfo;
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
