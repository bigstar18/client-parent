using System;
namespace ToolsLibrary.util
{
	public class Tools
	{
		public static long StrToLong(string s)
		{
			return Tools.StrToLong(s, -1L);
		}
		public static long StrToLong(string s, long defaultValue)
		{
			long result;
			try
			{
				result = long.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static char StrToChar(string s)
		{
			return Tools.StrToChar(s, ' ');
		}
		public static char StrToChar(string s, char defaultValue)
		{
			char result;
			try
			{
				result = char.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static short StrToShort(string s)
		{
			return Tools.StrToShort(s, -1);
		}
		public static short StrToShort(string s, short defaultValue)
		{
			short result;
			try
			{
				result = short.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static int StrToInt(string s, int defaultValue)
		{
			int result;
			try
			{
				result = int.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static int StrToInt(string s)
		{
			return Tools.StrToInt(s, -1);
		}
		public static float StrToFloat(string s, float defaultValue)
		{
			float result;
			try
			{
				result = float.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static float StrToFloat(string s)
		{
			return Tools.StrToFloat(s, -1f);
		}
		public static double StrToDouble(string s, double defaultValue)
		{
			double result;
			try
			{
				result = double.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static double StrToDouble(string s)
		{
			return Tools.StrToDouble(s, 0.0);
		}
		public static int ObjToInt(object o, int defaultValue)
		{
			int result;
			try
			{
				result = int.Parse(o.ToString());
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static int ObjToInt(object o)
		{
			return Tools.ObjToInt(0, -1);
		}
		public static bool StrToBool(string s, bool defaultValue)
		{
			bool result;
			try
			{
				result = bool.Parse(s);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}
		public static bool StrToBool(string s)
		{
			return Tools.StrToBool(s, false);
		}
	}
}
