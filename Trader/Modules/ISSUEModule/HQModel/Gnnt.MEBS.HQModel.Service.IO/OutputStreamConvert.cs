using System;
using System.IO;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.Service.IO
{
	internal class OutputStreamConvert
	{
		private BinaryWriter outer;
		public OutputStreamConvert(BinaryWriter outer)
		{
			this.outer = outer;
		}
		public void WriteJavaInt(int n)
		{
			byte[] bytes = BitConverter.GetBytes(n);
			Array.Reverse(bytes);
			uint value = BitConverter.ToUInt32(bytes, 0);
			this.outer.Write(value);
		}
		public void WriteJavaLong(long n)
		{
			byte[] bytes = BitConverter.GetBytes(n);
			Array.Reverse(bytes);
			ulong value = BitConverter.ToUInt64(bytes, 0);
			this.outer.Write(value);
		}
		public void WriteJavaByte(byte v)
		{
			this.outer.Write(v);
		}
		public void WriteJavaUTF(string str)
		{
			try
			{
				OutputStreamConvert.WriteJavaUTF(str, this.outer);
			}
			catch (IOException ex)
			{
				Logger.wirte(3, ex.StackTrace);
			}
		}
		private static void WriteJavaUTF(string str, BinaryWriter outer)
		{
			try
			{
				int length = str.Length;
				int num = 0;
				char[] array = new char[length];
				int num2 = 0;
				str.CopyTo(0, array, 0, length);
				for (int i = 0; i < length; i++)
				{
					int num3 = (int)array[i];
					if (num3 >= 1 && num3 <= 127)
					{
						num++;
					}
					else if (num3 > 2047)
					{
						num += 3;
					}
					else
					{
						num += 2;
					}
				}
				if (num > 65535)
				{
					throw new Exception();
				}
				byte[] array2 = new byte[num + 2];
				array2[num2++] = (byte)(num >> 8 & 255);
				array2[num2++] = (byte)(num & 255);
				for (int j = 0; j < length; j++)
				{
					int num3 = (int)array[j];
					if (num3 >= 1 && num3 <= 127)
					{
						array2[num2++] = (byte)num3;
					}
					else if (num3 > 2047)
					{
						array2[num2++] = (byte)(224 | (num3 >> 12 & 15));
						array2[num2++] = (byte)(128 | (num3 >> 6 & 63));
						array2[num2++] = (byte)(128 | (num3 & 63));
					}
					else
					{
						array2[num2++] = (byte)(192 | (num3 >> 6 & 31));
						array2[num2++] = (byte)(128 | (num3 & 63));
					}
				}
				outer.Write(array2);
			}
			catch (IOException ex)
			{
				Logger.wirte(3, ex.StackTrace);
			}
		}
		public void Flush()
		{
			this.outer.Flush();
		}
		public void Close()
		{
			this.outer.Close();
		}
	}
}
