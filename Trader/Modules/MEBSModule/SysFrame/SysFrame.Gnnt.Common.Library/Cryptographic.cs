using System;
namespace SysFrame.Gnnt.Common.Library
{
	public class Cryptographic
	{
		public static string encode(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += (char)((int)(str[i] + '\f') - i * 2);
			}
			return text;
		}
		public static string decode(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += (char)((int)(str[i] - '\f') + i * 2);
			}
			return text;
		}
	}
}
