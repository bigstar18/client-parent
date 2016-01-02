// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.Authentication.AuthMethod
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network.ProxySocket;
using System;
using System.Net.Sockets;

namespace Org.Mentalis.Network.ProxySocket.Authentication
{
  internal abstract class AuthMethod
  {
    private byte[] m_Buffer;
    private Socket m_Server;
    protected HandShakeComplete CallBack;
    private int m_Received;

    protected Socket Server
    {
      get
      {
        return this.m_Server;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.m_Server = value;
      }
    }

    protected byte[] Buffer
    {
      get
      {
        return this.m_Buffer;
      }
      set
      {
        this.m_Buffer = value;
      }
    }

    protected int Received
    {
      get
      {
        return this.m_Received;
      }
      set
      {
        this.m_Received = value;
      }
    }

    public AuthMethod(Socket server)
    {
      this.Server = server;
    }

    public abstract void Authenticate();

    public abstract void BeginAuthenticate(HandShakeComplete callback);
  }
}
