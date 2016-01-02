// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.PluginInfo
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.Collections;
using System.Resources;

namespace Gnnt.MEBS.HQModel
{
  public class PluginInfo
  {
    public string IPAddress = string.Empty;
    public Hashtable HTConfig = new Hashtable();
    public string ConfigPath;
    public ResourceManager HQResourceManager;
    public int Port;
    public int HttpPort;
  }
}
