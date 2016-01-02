using System;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	internal class BillFieldInfo
	{
		public string name;
		public bool visible;
		public int width;
		public BillFieldInfo(string n, bool vis, int w)
		{
			this.name = n;
			this.visible = vis;
			this.width = w;
		}
	}
}
