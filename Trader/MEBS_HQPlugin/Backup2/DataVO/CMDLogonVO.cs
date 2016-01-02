// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDLogonVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDLogonVO : CMDVO
  {
    public string name = string.Empty;
    public string password = string.Empty;
    public string key = string.Empty;

    public CMDLogonVO()
    {
      this.cmd = (byte) 1;
    }

    public static LogonVO getObj(InputStreamConvert input)
    {
      return new LogonVO()
      {
        sessionID = input.ReadJavaInt(),
        message = input.ReadJavaUTF(),
        gmlevel = input.ReadJavaInt(),
        lastIP = input.ReadJavaUTF(),
        lastTime = input.ReadJavaUTF()
      };
    }
  }
}
