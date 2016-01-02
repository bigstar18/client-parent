// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.Authentication.AuthNone
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network.ProxySocket;
using System;
using System.Net.Sockets;

namespace Org.Mentalis.Network.ProxySocket.Authentication
{
  internal sealed class AuthNone : AuthMethod
  {
    public AuthNone(Socket server)
      : base(server)
    {
    }

    public override void Authenticate()
    {
    }

    public override void BeginAuthenticate(HandShakeComplete callback)
    {
      callback((Exception) null);
    }
  }
}
