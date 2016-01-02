using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDByConditionVO : CMDVO
	{
		public int type;
		public long value;
		public CMDByConditionVO()
		{
			this.cmd = 11;
		}
		public static ProductDataVO[] getObj(InputStreamConvert input)
		{
			return CMDSortVO.getObj(input);
		}
	}
}
