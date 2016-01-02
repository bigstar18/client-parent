using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDByTimeVO : CMDVO
	{
		public int time;
		public static ProductDataVO[] getObj(InputStreamConvert input)
		{
			return CMDSortVO.getObj(input);
		}
	}
}
