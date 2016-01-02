// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.Socks4Handler
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Org.Mentalis.Network.ProxySocket
{
  internal sealed class Socks4Handler : SocksHandler
  {
    public Socks4Handler(Socket server, string user)
      : base(server, user)
    {
    }

    private byte[] GetHostPortBytes(string host, int port)
    {
      if (host == null)
        throw new ArgumentNullException();
      if (port <= 0 || port > (int) ushort.MaxValue)
        throw new ArgumentException();
      byte[] numArray = new byte[10 + this.Username.Length + host.Length];
      numArray[0] = (byte) 4;
      numArray[1] = (byte) 1;
      Array.Copy((Array) this.PortToBytes(port), 0, (Array) numArray, 2, 2);
      numArray[4] = numArray[5] = numArray[6] = (byte) 0;
      numArray[7] = (byte) 1;
      Array.Copy((Array) Encoding.ASCII.GetBytes(this.Username), 0, (Array) numArray, 8, this.Username.Length);
      numArray[8 + this.Username.Length] = (byte) 0;
      Array.Copy((Array) Encoding.ASCII.GetBytes(host), 0, (Array) numArray, 9 + this.Username.Length, host.Length);
      numArray[9 + this.Username.Length + host.Length] = (byte) 0;
      return numArray;
    }

    private byte[] GetEndPointBytes(IPEndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException();
      byte[] numArray = new byte[9 + this.Username.Length];
      numArray[0] = (byte) 4;
      numArray[1] = (byte) 1;
      Array.Copy((Array) this.PortToBytes(remoteEP.Port), 0, (Array) numArray, 2, 2);
      Array.Copy((Array) this.AddressToBytes(remoteEP.Address.Address), 0, (Array) numArray, 4, 4);
      Array.Copy((Array) Encoding.ASCII.GetBytes(this.Username), 0, (Array) numArray, 8, this.Username.Length);
      numArray[8 + this.Username.Length] = (byte) 0;
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
      if (connect == null)
        throw new ArgumentNullException();
      if (connect.Length < 2)
        throw new ArgumentException();
      this.Server.Send(connect);
      if ((int) this.ReadBytes(8)[1] != 90)
      {
        this.Server.Close();
        throw new ProxyException("Negotiation failed.");
      }
    }

    public override IAsyncProxyResult BeginNegotiate(string host, int port, HandShakeComplete callback, IPEndPoint proxyEndPoint)
    {
      this.ProtocolComplete = callback;
      this.Buffer = this.GetHostPortBytes(host, port);
      this.Server.BeginConnect((EndPoint) proxyEndPoint, new AsyncCallback(this.OnConnect), (object) this.Server);
      this.AsyncResult = new IAsyncProxyResult();
      return this.AsyncResult;
    }

    public override IAsyncProxyResult BeginNegotiate(IPEndPoint remoteEP, HandShakeComplete callback, IPEndPoint proxyEndPoint)
    {
      this.ProtocolComplete = callback;
      this.Buffer = this.GetEndPointBytes(remoteEP);
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
        this.Server.BeginSend(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnSent), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    private void OnSent(IAsyncResult ar)
    {
      try
      {
        if (this.Server.EndSend(ar) < this.Buffer.Length)
        {
          this.ProtocolComplete((Exception) new SocketException());
          return;
        }
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
        return;
      }
      try
      {
        this.Buffer = new byte[8];
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
        int num = this.Server.EndReceive(ar);
        if (num <= 0)
        {
          this.ProtocolComplete((Exception) new SocketException());
        }
        else
        {
          this.Received += num;
          if (this.Received == 8)
          {
            if ((int) this.Buffer[1] == 90)
            {
              this.ProtocolComplete((Exception) null);
            }
            else
            {
              this.Server.Close();
              this.ProtocolComplete((Exception) new ProxyException("Negotiation failed."));
            }
          }
          else
            this.Server.BeginReceive(this.Buffer, this.Received, this.Buffer.Length - this.Received, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.Server);
        }
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }
  }
}
