using System;
using System.Text;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class ProductInfoVO
	{
		public const int TYPE_INVALID = -1;
		public const int TYPE_COMMON = 0;
		public const int TYPE_CANCEL = 1;
		public const int TYPE_INDEX = 2;
		public const int TYPE_INDEX_MAIN = 3;
		public const int TYPE_SERIES = 4;
		public const int TYPE_PAUSE = 5;
		public const int TYPE_FINISHIED = 6;
		public string marketID = string.Empty;
		public string code;
		public float fUnit = 1f;
		public string name;
		public int status;
		public string industry;
		public string region;
		public string notice;
		public int type;
		public int openTime;
		public int[] tradeSecNo = new int[1];
		public float mPrice;
		public int closeTime;
		public string region_id;
		public string cclass = "";
		public string cclass_name;
		public string ReservedField = string.Empty;
		public DateTime modifyTime = DateTime.Now;
		public string[] pyName = new string[0];
		public override string ToString()
		{
			string text = "\n";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("**ProductInfoVO**" + text);
			stringBuilder.Append("Code:" + this.code + text);
			stringBuilder.Append("Name:" + this.name + text);
			stringBuilder.Append("Type:" + this.type + text);
			stringBuilder.Append("Industry:" + this.industry + text);
			stringBuilder.Append("Region:" + this.region + text);
			stringBuilder.Append("CloseTime:" + this.closeTime + text);
			stringBuilder.Append("CreateTime:" + this.openTime + text);
			stringBuilder.Append("ModifyTime:" + this.modifyTime + text);
			stringBuilder.Append("Status:" + this.status + text);
			stringBuilder.Append("pyName.length:" + this.pyName.Length + text);
			for (int i = 0; i < this.pyName.Length; i++)
			{
				stringBuilder.Append(string.Concat(new object[]
				{
					"pyName[",
					i,
					"]:",
					this.pyName[i],
					text
				}));
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString() + base.ToString();
		}
	}
}
