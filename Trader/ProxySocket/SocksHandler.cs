// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.SocksHandler
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using System;
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Network.ProxySocket
{
  internal abstract class SocksHandler
  {
    private Socket m_Server;
    private string m_Username;
    private IAsyncProxyResult m_AsyncResult;
    private byte[] m_Buffer;
    private int m_Received;
    protected HandShakeComplete ProtocolComplete;

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

    protected string Username
    {
      get
      {
        return this.m_Username;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.m_Username = value;
      }
    }

    protected IAsyncProxyResult AsyncResult
    {
      get
      {
        return this.m_AsyncResult;
      }
      set
      {
        this.m_AsyncResult = value;
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

    public SocksHandler(Socket server, string user)
    {
      this.Server = server;
      this.Username = user;
    }

    protected byte[] PortToBytes(int port)
    {
      return new byte[2]
      {
        (byte) (port / 256),
        (byte) (port % 256)
      };
    }

    protected byte[] AddressToBytes(long address)
    {
      return new byte[4]
      {
        (byte) ((ulong) address % 256UL),
        (byte) ((ulong) (address / 256L) % 256UL),
        (byte) ((ulong) (address / 65536L) % 256UL),
        (byte) ((ulong) address / 16777216UL)
      };
    }

    protected byte[] ReadBytes(int count)
    {
      if (count <= 0)
        throw new ArgumentException();
      byte[] buffer = new byte[count];
      int offset = 0;
      while (offset != count)
        offset += this.Server.Receive(buffer, offset, count - offset, SocketFlags.None);
      return buffer;
    }

    public abstract void Negotiate(string host, int port);

    public abstract void Negotiate(IPEndPoint remoteEP);

    public abstract IAsyncProxyResult BeginNegotiate(IPEndPoint remoteEP, HandShakeComplete callback, IPEndPoint proxyEndPoint);

    public abstract IAsyncProxyResult BeginNegotiate(string host, int port, HandShakeComplete callback, IPEndPoint proxyEndPoint);
  }
}
