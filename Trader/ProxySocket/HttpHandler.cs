// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.HttpHandler
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Org.Mentalis.Network.ProxySocket
{
  internal sealed class HttpHandler : SocksHandler
  {
    private string m_Password;

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

    public HttpHandler(Socket server)
      : this(server, "")
    {
    }

    public HttpHandler(Socket server, string user)
      : this(server, user, "")
    {
    }

    public HttpHandler(Socket server, string user, string pass)
      : base(server, user)
    {
      this.Password = pass;
    }

    private byte[] GetHostPortBytes(string host, int port)
    {
      if (host == null)
        throw new ArgumentNullException();
      if (port <= 0 || port > (int) ushort.MaxValue || host.Length > (int) byte.MaxValue)
        throw new ArgumentException();
      string str = "CONNECT " + (object) host + ":" + (string) (object) port + " HTTP/1.1\r\nHost: " + host + ":" + (string) (object) port + "\r\n";
      return Encoding.ASCII.GetBytes(this.Username == null || this.Username.Length <= 0 ? str + "\r\n" : str + "Authorization: Basic " + this.EncodeBase64("ASCII", this.Username + ":" + this.Password) + "\r\nProxy-Authorization: Basic " + this.EncodeBase64("ASCII", this.Username + ":" + this.Password) + "\r\n\r\n");
    }

    private byte[] GetEndPointBytes(IPEndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException();
      string str = "CONNECT " + (object) remoteEP.Address + ":" + (string) (object) remoteEP.Port + " HTTP/1.1\r\nHost: " + (string) (object) remoteEP.Address + ":" + (string) (object) remoteEP.Port + "\r\n";
      return Encoding.ASCII.GetBytes(this.Username == null || this.Username.Length <= 0 ? str + "\r\n" : str + "Authorization: Basic " + this.EncodeBase64("ASCII", this.Username + ":" + this.Password) + "\r\nProxy-Authorization: Basic " + this.EncodeBase64("ASCII", this.Username + ":" + this.Password) + "\r\n\r\n");
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
      this.Server.Send(connect);
      byte[] numArray = new byte[1024];
      this.Server.Receive(numArray, 0, numArray.Length, SocketFlags.None);
      string @string = Encoding.ASCII.GetString(numArray);
      if (!@string.StartsWith("HTTP/1.1 200", true, (CultureInfo) null) && !@string.StartsWith("HTTP/1.0 200", true, (CultureInfo) null))
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
        this.Buffer = new byte[1024];
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
          if (!Encoding.ASCII.GetString(this.Buffer).StartsWith("HTTP/1.1 200", true, (CultureInfo) null))
          {
            this.Server.Close();
            this.ProtocolComplete((Exception) new ProxyException("Negotiation failed."));
          }
          else
            this.ProtocolComplete((Exception) null);
        }
      }
      catch (Exception ex)
      {
        this.ProtocolComplete(ex);
      }
    }

    public string EncodeBase64(string code_type, string code)
    {
      byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
      try
      {
        return Convert.ToBase64String(bytes);
      }
      catch
      {
        return code;
      }
    }

    public string DecodeBase64(string code_type, string code)
    {
      byte[] bytes = Convert.FromBase64String(code);
      try
      {
        return Encoding.GetEncoding(code_type).GetString(bytes);
      }
      catch
      {
        return code;
      }
    }
  }
}
