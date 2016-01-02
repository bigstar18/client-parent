using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDSetCurPage : CMDVO
	{
		public int curPage = -1;
		public CMDSetCurPage()
		{
			this.cmd = 13;
		}
	}
}
