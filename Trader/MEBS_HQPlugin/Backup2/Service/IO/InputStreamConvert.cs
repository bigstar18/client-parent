// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.Service.IO.InputStreamConvert
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

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
      byte[] bytes = BitConverter.GetBytes(this.input.ReadInt32());
      Array.Reverse((Array) bytes);
      return BitConverter.ToInt32(bytes, 0);
    }

    public static int ReadJavaUnsignedShort(BinaryReader input)
    {
      byte[] bytes = BitConverter.GetBytes(input.ReadUInt16());
      Array.Reverse((Array) bytes);
      return (int) BitConverter.ToUInt16(bytes, 0);
    }

    public float ReadJavaFloat()
    {
      return BitConverter.ToSingle(BitConverter.GetBytes(this.ReadJavaInt()), 0);
    }

    public long ReadJavaLong()
    {
      byte[] bytes = BitConverter.GetBytes(this.input.ReadInt64());
      Array.Reverse((Array) bytes);
      return BitConverter.ToInt64(bytes, 0);
    }

    public double ReadJavaDouble()
    {
      return BitConverter.ToDouble(BitConverter.GetBytes(this.ReadJavaLong()), 0);
    }

    public static void ReadJavaFully(BinaryReader input, byte[] b, int off, int len)
    {
      if (len < 0)
        throw new Exception();
      int num1 = 0;
      while (num1 < len)
      {
        int num2 = input.Read(b, off + num1, len - num1);
        if (num2 <= 0)
          throw new Exception();
        num1 += num2;
      }
    }

    public string ReadJavaUTF()
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      try
      {
        int length = InputStreamConvert.ReadJavaUnsignedShort(this.input);
        stringBuilder = new StringBuilder(length);
        byte[] b = new byte[length];
        int index = 0;
        InputStreamConvert.ReadJavaFully(this.input, b, 0, length);
        while (index < length)
        {
          int num1 = (int) b[index];
          int num2 = (int) b[index] & (int) byte.MaxValue;
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
              ++index;
              stringBuilder.Append((char) num2);
              continue;
            case 12:
            case 13:
              index += 2;
              if (index > length)
                throw new Exception();
              int num3 = (int) b[index - 1];
              if ((num3 & 192) != 128)
                throw new Exception();
              stringBuilder.Append((char) ((num2 & 31) << 6 | num3 & 63));
              continue;
            case 14:
              index += 3;
              if (index <= length)
              {
                int num4 = (int) b[index - 2];
                int num5 = (int) b[index - 1];
                if ((num4 & 192) == 128)
                {
                  if ((num5 & 192) == 128)
                  {
                    try
                    {
                      stringBuilder.Append((char) ((num2 & 15) << 12 | (num4 & 63) << 6 | num5 & 63));
                      continue;
                    }
                    catch (Exception ex)
                    {
                      Logger.wirte(MsgType.Error, ex.Message);
                      Logger.wirte(MsgType.Error, ex.Source);
                      Logger.wirte(MsgType.Error, ex.StackTrace);
                      continue;
                    }
                  }
                  else
                    continue;
                }
                else
                  continue;
              }
              else
                continue;
            default:
              throw new InvalidDataException("UTF Data Format Exception");
          }
        }
      }
      catch (OutOfMemoryException ex)
      {
        Logger.wirte(MsgType.Error, ex.ToString());
      }
      return stringBuilder.ToString();
    }
  }
}
