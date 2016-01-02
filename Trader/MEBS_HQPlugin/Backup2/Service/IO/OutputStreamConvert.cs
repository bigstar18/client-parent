// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.Service.IO.OutputStreamConvert
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

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
      Array.Reverse((Array) bytes);
      this.outer.Write(BitConverter.ToUInt32(bytes, 0));
    }

    public void WriteJavaLong(long n)
    {
      byte[] bytes = BitConverter.GetBytes(n);
      Array.Reverse((Array) bytes);
      this.outer.Write(BitConverter.ToUInt64(bytes, 0));
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
        Logger.wirte(MsgType.Error, ex.StackTrace);
      }
    }

    private static void WriteJavaUTF(string str, BinaryWriter outer)
    {
      try
      {
        int length = str.Length;
        int num1 = 0;
        char[] destination = new char[length];
        int num2 = 0;
        str.CopyTo(0, destination, 0, length);
        for (int index = 0; index < length; ++index)
        {
          int num3 = (int) destination[index];
          if (num3 >= 1 && num3 <= (int) sbyte.MaxValue)
            ++num1;
          else if (num3 > 2047)
            num1 += 3;
          else
            num1 += 2;
        }
        if (num1 > (int) ushort.MaxValue)
          throw new Exception();
        byte[] buffer = new byte[num1 + 2];
        byte[] numArray1 = buffer;
        int index1 = num2;
        int num4 = 1;
        int num5 = index1 + num4;
        int num6 = (int) (byte) (num1 >> 8 & (int) byte.MaxValue);
        numArray1[index1] = (byte) num6;
        byte[] numArray2 = buffer;
        int index2 = num5;
        int num7 = 1;
        int num8 = index2 + num7;
        int num9 = (int) (byte) (num1 & (int) byte.MaxValue);
        numArray2[index2] = (byte) num9;
        for (int index3 = 0; index3 < length; ++index3)
        {
          int num3 = (int) destination[index3];
          if (num3 >= 1 && num3 <= (int) sbyte.MaxValue)
            buffer[num8++] = (byte) num3;
          else if (num3 > 2047)
          {
            byte[] numArray3 = buffer;
            int index4 = num8;
            int num10 = 1;
            int num11 = index4 + num10;
            int num12 = (int) (byte) (224 | num3 >> 12 & 15);
            numArray3[index4] = (byte) num12;
            byte[] numArray4 = buffer;
            int index5 = num11;
            int num13 = 1;
            int num14 = index5 + num13;
            int num15 = (int) (byte) (128 | num3 >> 6 & 63);
            numArray4[index5] = (byte) num15;
            byte[] numArray5 = buffer;
            int index6 = num14;
            int num16 = 1;
            num8 = index6 + num16;
            int num17 = (int) (byte) (128 | num3 & 63);
            numArray5[index6] = (byte) num17;
          }
          else
          {
            byte[] numArray3 = buffer;
            int index4 = num8;
            int num10 = 1;
            int num11 = index4 + num10;
            int num12 = (int) (byte) (192 | num3 >> 6 & 31);
            numArray3[index4] = (byte) num12;
            byte[] numArray4 = buffer;
            int index5 = num11;
            int num13 = 1;
            num8 = index5 + num13;
            int num14 = (int) (byte) (128 | num3 & 63);
            numArray4[index5] = (byte) num14;
          }
        }
        outer.Write(buffer);
      }
      catch (IOException ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace);
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
