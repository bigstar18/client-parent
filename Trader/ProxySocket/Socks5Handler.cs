// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.Socks5Handler
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network.ProxySocket.Authentication;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Org.Mentalis.Network.ProxySocket
{
  internal sealed class Socks5Handler : SocksHandler
  {
    private string m_Password;
    private byte[] m_HandShake;

    private string Password
    {
      get
      {
        return this.m_Password;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.m_Password = value;
      }
    }

    private byte[] HandShake
    {
      get
      {
        return this.m_HandShake;
      }
      set
      {
        this.m_HandShake = value;
      }
    }

    public Socks5Handler(Socket server)
      : this(server, "")
    {
    }

    public Socks5Handler(Socket server, string user)
      : this(server, user, "")
    {
    }

    public Socks5Handler(Socket server, string user, string pass)
      : base(server, user)
    {
      this.Password = pass;
    }

    private void Authenticate()
    {
      this.Server.Send(new byte[4]
      {
        (byte) 5,
        (byte) 2,
        (byte) 0,
        (byte) 2
      });
      byte[] numArray = this.ReadBytes(2);
      if ((int) numArray[1] == (int) byte.MaxValue)
        throw new ProxyException("No authentication method accepted.");
      AuthMethod authMethod;
      switch (numArray[1])
      {
        case (byte) 0:
          authMethod = (AuthMethod) new AuthNone(this.Server);
          break;
        case (byte) 2:
          authMethod = (AuthMethod) new AuthUserPass(this.Server, this.Username, this.Password);
          break;
        default:
          throw new ProtocolViolationException();
      }
      authMethod.Authenticate();
    }

    private byte[] GetHostPortBytes(string host, int port)
    {
      if (host == null)
        throw new ArgumentNullException();
      if (port <= 0 || port > (int) ushort.MaxValue || host.Length > (int) byte.MaxValue)
        throw new ArgumentException();
      byte[] numArray = new byte[7 + host.Length];
      numArray[0] = (byte) 5;
      numArray[1] = (byte) 1;
      numArray[2] = (byte) 0;
      numArray[3] = (byte) 3;
      numArray[4] = (byte) host.Length;
      Array.Copy((Array) Encoding.ASCII.GetBytes(host), 0, (Array) numArray, 5, host.Length);
      Array.Copy((Array) this.PortToBytes(port), 0, (Array) numArray, host.Length + 5, 2);
      return numArray;
    }

    private byte[] GetEndPointBytes(IPEndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException();
      byte[] numArray = new byte[10];
      numArray[0] = (byte) 5;
      numArray[1] = (byte) 1;
      numArray[2] = (byte) 0;
      numArray[3] = (byte) 1;
      Array.Copy((Array) this.AddressToBytes(remoteEP.Address.Address), 0, (Array) numArray, 4, 4);
      Array.Copy((Array) this.PortToBytes(remoteEP.Port), 0, (Array) numArray, 8, 2);
      return numArray;
    }

    public override void Negotiate(string host, int port)
    {
      this.Negotiate(this.GetHostPortBytes(host, port));
    }

    public override void Negotiate(IPEndPoint remoteEP)
    {
      this.Negotiate(this.GetEndPointBytes(remoteEP));
    }

    private void Negotiate(byte[] connect)
    {
      this.Authenticate();
      this.Server.Send(connect);
      byte[] numArray1 = this.ReadBytes(4);
      if ((int) numArray1[1] != 0)
      {
        this.Server.Close();
        throw new ProxyException((int) numArray1[1]);
      }
      byte[] numArray2;
      switch (numArray1[3])
      {
        case (byte) 1:
          numArray2 = this.ReadBytes(6);
          break;
        case (byte) 3:
          numArray2 = this.ReadBytes((int) this.ReadBytes(1)[0] + 2);
          break;
        case (byte) 4:
          numArray2 = this.ReadBytes(18);
          break;
        default:
          this.Server.Close();
          throw new ProtocolViolationException();
      }
    }

    public override IAsyncProxyResult BeginNegotiate(string host, int port, HandShakeComplete callback, IPEndPoint proxyEndPoint)
    {
      this.ProtocolComplete = callback;
      this.HandShake = this.GetHostPortBytes(host, port);
      this.Server.BeginConnect((EndPoint) proxyEndPoint, new AsyncCallback(this.OnConnect), (object) this.Server);
      this.AsyncResult = new IAsyncProxyResult();
      return this.AsyncResult;
    }

    public override IAsyncProxyResult BeginNegotiate(IPEndPoint remoteEP, HandShakeComplete callback, IPEndPoint proxyEndPoint)
    {
      this.ProtocolComplete = callback;
      this.HandShake = this.GetEndPointBytes(remoteEP);
      this.Server.BeginConnect((EndPoint) proxyEndPoint, new AsyncCallback(this.OnConnect), (object) this.Server);
      this.AsyncResult = new IAsyncProxyResult();
      return this.AsyncResult;
    }

    private void OnConnect(IAsyncResult ar)
    {
      try
      {
        this.Server.EndConnect(ar);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        this.Server.BeginSend(new byte[4]
        {
          (byte) 5,
          (byte) 2,
          (byte) 0,
          (byte) 2
        }, 0, 4, SocketFlags.None, new AsyncCallback(this.OnAuthSent), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void OnAuthSent(IAsyncResult ar)
    {
      try
      {
        this.Server.EndSend(ar);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        this.Buffer = new byte[1024];
        this.Received = 0;
        this.Server.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnAuthReceive), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void OnAuthReceive(IAsyncResult ar)
    {
      try
      {
        this.Received += this.Server.EndReceive(ar);
        if (this.Received <= 0)
          throw new SocketException();
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        if (this.Received < 2)
        {
          this.Server.BeginReceive(this.Buffer, this.Received, this.Buffer.Length - this.Received, SocketFlags.None, new AsyncCallback(this.OnAuthReceive), (object) this.Server);
        }
        else
        {
          AuthMethod authMethod;
          switch (this.Buffer[1])
          {
            case (byte) 0:
              authMethod = (AuthMethod) new AuthNone(this.Server);
              break;
            case (byte) 2:
              authMethod = (AuthMethod) new AuthUserPass(this.Server, this.Username, this.Password);
              break;
            default:
              this.ProtocolComplete((Exception) new SocketException());
              return;
          }
          authMethod.BeginAuthenticate(new HandShakeComplete(this.OnAuthenticated));
        }
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void OnAuthenticated(Exception e)
    {
      if (e != null)
      {
        this.ProtocolComplete(e);
      }
      else
      {
        try
        {
          this.Server.BeginSend(this.HandShake, 0, this.HandShake.Length, SocketFlags.None, new AsyncCallback(this.OnSent), (object) this.Server);
        }
        catch (Exception ex)
        {
          this.ProtocolComplete(ex);
        }
      }
    }

    private void OnSent(IAsyncResult ar)
    {
      try
      {
        this.Server.EndSend(ar);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        this.Buffer = new byte[5];
        this.Received = 0;
        this.Server.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void OnReceive(IAsyncResult ar)
    {
      try
      {
        this.Received += this.Server.EndReceive(ar);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        if (this.Received == this.Buffer.Length)
          this.ProcessReply(this.Buffer);
        else
          this.Server.BeginReceive(this.Buffer, this.Received, this.Buffer.Length - this.Received, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void ProcessReply(byte[] buffer)
    {
      switch (buffer[3])
      {
        case (byte) 1:
          this.Buffer = new byte[5];
          break;
        case (byte) 3:
          this.Buffer = new byte[(int) buffer[4] + 2];
          break;
        case (byte) 4:
          buffer = new byte[17];
          break;
        default:
          throw new ProtocolViolationException();
      }
      this.Received = 0;
      this.Server.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReadLast), (object) this.Server);
    }

    private void OnReadLast(IAsyncResult ar)
    {
      try
      {
        this.Received += this.Server.EndReceive(ar);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        if (this.Received == this.Buffer.Length)
          this.ProtocolComplete((Exception) null);
        else
          this.Server.BeginReceive(this.Buffer, this.Received, this.Buffer.Length - this.Received, SocketFlags.None, new AsyncCallback(this.OnReadLast), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }
  }
}
