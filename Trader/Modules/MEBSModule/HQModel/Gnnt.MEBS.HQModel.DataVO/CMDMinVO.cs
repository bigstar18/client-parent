using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.Collections.Generic;
using System.IO;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDMinVO : CMDVO
	{
		public byte type;
		public byte mark;
		public int date;
		public int time;
		public List<CommidityVO> commidityList;
		public CMDMinVO()
		{
			this.cmd = 6;
		}
		public static MinDataVO[] getObj(InputStreamConvert input)
		{
			int num = input.ReadJavaInt();
			MinDataVO[] array = new MinDataVO[num];
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new MinDataVO();
					array[i].date = input.ReadJavaInt();
					array[i].time = input.ReadJavaInt();
					array[i].curPrice = input.ReadJavaFloat();
					array[i].totalAmount = input.ReadJavaLong();
					array[i].averPrice = input.ReadJavaFloat();
					array[i].reserveCount = input.ReadJavaInt();
				}
			}
			catch (IOException ex)
			{
				Logger.wirte(3, ex.ToString());
			}
			return array;
		}
	}
}
