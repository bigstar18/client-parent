using Gnnt.MEBS.HQModel.Service.IO;
using System;
using System.IO;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDBillByVersionVO : CMDVO
	{
		public string marketID = string.Empty;
		public string code = string.Empty;
		public byte type;
		public long totalAmount;
		public long time;
		public string ReservedField = string.Empty;
		public CMDBillByVersionVO()
		{
			this.cmd = 7;
		}
		public static BillDataVO[] getObj(InputStreamConvert input, ref bool isConnection)
		{
			int num = input.ReadJavaInt();
			BillDataVO[] array = new BillDataVO[num];
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new BillDataVO();
					array[i].date = input.ReadJavaInt();
					array[i].time = input.ReadJavaInt();
					array[i].reserveCount = input.ReadJavaInt();
					array[i].buyPrice = input.ReadJavaFloat();
					array[i].curPrice = input.ReadJavaFloat();
					array[i].sellPrice = input.ReadJavaFloat();
					array[i].totalAmount = input.ReadJavaLong();
					array[i].totalMoney = input.ReadJavaDouble();
					array[i].openAmount = input.ReadJavaInt();
					array[i].closeAmount = input.ReadJavaInt();
					array[i].balancePrice = input.ReadJavaFloat();
					array[i].tradeCue = input.ReadJavaUTF();
				}
			}
			catch (EndOfStreamException ex)
			{
				Logger.wirte(3, ex.ToString());
			}
			catch (IOException ex2)
			{
				isConnection = true;
				Logger.wirte(3, ex2.ToString());
			}
			return array;
		}
	}
}
