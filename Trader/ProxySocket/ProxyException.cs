// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.ProxyException
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using System;

namespace Org.Mentalis.Network.ProxySocket
{
  public class ProxyException : Exception
  {
    public ProxyException()
      : this("An error occured while talking to the proxy server.")
    {
    }

    public ProxyException(string message)
      : base(message)
    {
    }

    public ProxyException(int socks5Error)
      : this(ProxyException.Socks5ToString(socks5Error))
    {
    }

    public static string Socks5ToString(int socks5Error)
    {
      switch (socks5Error)
      {
        case 0:
          return "Connection succeeded.";
        case 1:
          return "General SOCKS server failure.";
        case 2:
          return "Connection not allowed by ruleset.";
        case 3:
          return "Network unreachable.";
        case 4:
          return "Host unreachable.";
        case 5:
          return "Connection refused.";
        case 6:
          return "TTL expired.";
        case 7:
          return "Command not supported.";
        case 8:
          return "Address type not supported.";
        default:
          return "Unspecified SOCKS error.";
      }
    }
  }
}
