using System;
using System.IO;
using System.Text;
using TPME.Log;
namespace Gnnt.MEBS.HQModel.Service.IO
{
	public class InputStreamConvert
	{
		private byte[] readBuffer = new byte[8];
		private BinaryReader input;
		public InputStreamConvert(BinaryReader input)
		{
			this.input = input;
		}
		public byte ReadJavaByte()
		{
			return this.input.ReadByte();
		}
		public int ReadJavaInt()
		{
			int value = this.input.ReadInt32();
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return BitConverter.ToInt32(bytes, 0);
		}
		public static int ReadJavaUnsignedShort(BinaryReader input)
		{
			ushort value = input.ReadUInt16();
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return (int)BitConverter.ToUInt16(bytes, 0);
		}
		public float ReadJavaFloat()
		{
			int value = this.ReadJavaInt();
			byte[] bytes = BitConverter.GetBytes(value);
			return BitConverter.ToSingle(bytes, 0);
		}
		public long ReadJavaLong()
		{
			long value = this.input.ReadInt64();
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return BitConverter.ToInt64(bytes, 0);
		}
		public double ReadJavaDouble()
		{
			long value = this.ReadJavaLong();
			byte[] bytes = BitConverter.GetBytes(value);
			return BitConverter.ToDouble(bytes, 0);
		}
		public static void ReadJavaFully(BinaryReader input, byte[] b, int off, int len)
		{
			if (len < 0)
			{
				throw new Exception();
			}
			int num;
			for (int i = 0; i < len; i += num)
			{
				num = input.Read(b, off + i, len - i);
				if (num <= 0)
				{
					throw new Exception();
				}
			}
		}
		public string ReadJavaUTF()
		{
			StringBuilder stringBuilder = null;
			try
			{
				int num = InputStreamConvert.ReadJavaUnsignedShort(this.input);
				stringBuilder = new StringBuilder(num);
				byte[] array = new byte[num];
				int i = 0;
				InputStreamConvert.ReadJavaFully(this.input, array, 0, num);
				while (i < num)
				{
					byte arg_36_0 = array[i];
					int num2 = (int)(array[i] & 255);
					switch (num2 >> 4)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
						i++;
						stringBuilder.Append((char)num2);
						continue;
					case 12:
					case 13:
					{
						i += 2;
						if (i > num)
						{
							throw new Exception();
						}
						int num3 = (int)array[i - 1];
						if ((num3 & 192) != 128)
						{
							throw new Exception();
						}
						stringBuilder.Append((char)((num2 & 31) << 6 | (num3 & 63)));
						continue;
					}
					case 14:
					{
						i += 3;
						if (i > num)
						{
							continue;
						}
						int num3 = (int)array[i - 2];
						int num4 = (int)array[i - 1];
						if ((num3 & 192) != 128 || (num4 & 192) != 128)
						{
							continue;
						}
						try
						{
							stringBuilder.Append((char)((num2 & 15) << 12 | (num3 & 63) << 6 | (num4 & 63)));
							continue;
						}
						catch (Exception ex)
						{
							Logger.wirte(3, ex.Message);
							Logger.wirte(3, ex.Source);
							Logger.wirte(3, ex.StackTrace);
							continue;
						}
						break;
					}
					}
					throw new InvalidDataException("UTF Data Format Exception");
				}
			}
			catch (OutOfMemoryException ex2)
			{
				Logger.wirte(3, ex2.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
