// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.ProxySocket
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network;
using System;
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Network.ProxySocket
{
  public class ProxySocket : Socket
  {
    private object m_State;
    private IPEndPoint m_ProxyEndPoint;
    private ProxyTypes m_ProxyType;
    private string m_ProxyUser;
    private string m_ProxyPass;
    private AsyncCallback CallBack;
    private IAsyncProxyResult m_AsyncResult;
    private Exception m_ToThrow;
    private int m_RemotePort;

    public IPEndPoint ProxyEndPoint
    {
      get
      {
        return this.m_ProxyEndPoint;
      }
      set
      {
        this.m_ProxyEndPoint = value;
      }
    }

    public ProxyTypes ProxyType
    {
      get
      {
        return this.m_ProxyType;
      }
      set
      {
        this.m_ProxyType = value;
      }
    }

    private object State
    {
      get
      {
        return this.m_State;
      }
      set
      {
        this.m_State = value;
      }
    }

    public string ProxyUser
    {
      get
      {
        return this.m_ProxyUser;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.m_ProxyUser = value;
      }
    }

    public string ProxyPass
    {
      get
      {
        return this.m_ProxyPass;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.m_ProxyPass = value;
      }
    }

    private IAsyncProxyResult AsyncResult
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

    private Exception ToThrow
    {
      get
      {
        return this.m_ToThrow;
      }
      set
      {
        this.m_ToThrow = value;
      }
    }

    private int RemotePort
    {
      get
      {
        return this.m_RemotePort;
      }
      set
      {
        this.m_RemotePort = value;
      }
    }

    public ProxySocket()
      : this(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    {
    }

    public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
      : this(addressFamily, socketType, protocolType, "")
    {
    }

    public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername)
      : this(addressFamily, socketType, protocolType, proxyUsername, "")
    {
    }

    public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername, string proxyPassword)
      : base(addressFamily, socketType, protocolType)
    {
      this.ProxyUser = proxyUsername;
      this.ProxyPass = proxyPassword;
      this.ToThrow = (Exception) new InvalidOperationException();
    }

    public Socket GetSocket(string host, int port)
    {
      try
      {
        if (System.IO.File.Exists("Proxy.ini"))
        {
          IniFile iniFile = new IniFile("Proxy.ini");
          if (bool.Parse(iniFile.IniReadValue("ProxyServer", "Enable")))
          {
            int num = int.Parse(iniFile.IniReadValue("ProxyServer", "Type"));
            string ipString = iniFile.IniReadValue("ProxyServer", "ProxyIP");
            int port1 = int.Parse(iniFile.IniReadValue("ProxyServer", "ProxyPort"));
            string str1 = iniFile.IniReadValue("ProxyServer", "UserName");
            string str2 = iniFile.IniReadValue("ProxyServer", "Password");
            if (ipString.Length > 0)
            {
              if (port1 > 0)
              {
                this.ProxyEndPoint = new IPEndPoint(IPAddress.Parse(ipString), port1);
                if (num == 0)
                {
                  this.ProxyUser = str1;
                  this.ProxyType = ProxyTypes.Socks4;
                }
                else if (num == 1)
                {
                  this.ProxyUser = str1;
                  this.ProxyPass = str2;
                  this.ProxyType = ProxyTypes.Socks5;
                }
                else if (num == 2)
                {
                  this.ProxyUser = str1;
                  this.ProxyPass = str2;
                  this.ProxyType = ProxyTypes.Http;
                }
              }
            }
          }
        }
        else
          this.ProxyType = ProxyTypes.None;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      this.Connect(host, port);
      return (Socket) this;
    }

    public Socket GetSocket(EndPoint remoteEP)
    {
      try
      {
        if (System.IO.File.Exists("Proxy.ini"))
        {
          IniFile iniFile = new IniFile("Proxy.ini");
          if (bool.Parse(iniFile.IniReadValue("ProxyServer", "Enable")))
          {
            int num = int.Parse(iniFile.IniReadValue("ProxyServer", "Type"));
            string ipString = iniFile.IniReadValue("ProxyServer", "ProxyIP");
            int port = int.Parse(iniFile.IniReadValue("ProxyServer", "ProxyPort"));
            string str1 = iniFile.IniReadValue("ProxyServer", "UserName");
            string str2 = iniFile.IniReadValue("ProxyServer", "Password");
            if (ipString.Length > 0)
            {
              if (port > 0)
              {
                this.ProxyEndPoint = new IPEndPoint(IPAddress.Parse(ipString), port);
                if (num == 0)
                {
                  this.ProxyUser = str1;
                  this.ProxyType = ProxyTypes.Socks4;
                }
                else if (num == 1)
                {
                  this.ProxyUser = str1;
                  this.ProxyPass = str2;
                  this.ProxyType = ProxyTypes.Socks5;
                }
                else if (num == 2)
                {
                  this.ProxyUser = str1;
                  this.ProxyPass = str2;
                  this.ProxyType = ProxyTypes.Http;
                }
              }
            }
          }
        }
        else
          this.ProxyType = ProxyTypes.None;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      this.Connect(remoteEP);
      return (Socket) this;
    }

    public new void Connect(EndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException("<remoteEP> cannot be null.");
      if (this.ProtocolType != ProtocolType.Tcp || this.ProxyType == ProxyTypes.None || this.ProxyEndPoint == null)
      {
        base.Connect(remoteEP);
      }
      else
      {
        base.Connect((EndPoint) this.ProxyEndPoint);
        if (this.ProxyType == ProxyTypes.Socks4)
          new Socks4Handler((Socket) this, this.ProxyUser).Negotiate((IPEndPoint) remoteEP);
        else if (this.ProxyType == ProxyTypes.Socks5)
        {
          new Socks5Handler((Socket) this, this.ProxyUser, this.ProxyPass).Negotiate((IPEndPoint) remoteEP);
        }
        else
        {
          if (this.ProxyType != ProxyTypes.Http)
            return;
          new HttpHandler((Socket) this, this.ProxyUser, this.ProxyPass).Negotiate((IPEndPoint) remoteEP);
        }
      }
    }

    public new void Connect(string host, int port)
    {
      if (host == null)
        throw new ArgumentNullException("<host> cannot be null.");
      if (port <= 0 || port > (int) ushort.MaxValue)
        throw new ArgumentException("Invalid port.");
      if (this.ProtocolType != ProtocolType.Tcp || this.ProxyType == ProxyTypes.None || this.ProxyEndPoint == null)
      {
        base.Connect((EndPoint) new IPEndPoint(Dns.Resolve(host).AddressList[0], port));
      }
      else
      {
        base.Connect((EndPoint) this.ProxyEndPoint);
        if (this.ProxyType == ProxyTypes.Socks4)
          new Socks4Handler((Socket) this, this.ProxyUser).Negotiate(host, port);
        else if (this.ProxyType == ProxyTypes.Socks5)
        {
          new Socks5Handler((Socket) this, this.ProxyUser, this.ProxyPass).Negotiate(host, port);
        }
        else
        {
          if (this.ProxyType != ProxyTypes.Http)
            return;
          new HttpHandler((Socket) this, this.ProxyUser, this.ProxyPass).Negotiate(host, port);
        }
      }
    }

    public new IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (remoteEP == null || callback == null)
        throw new ArgumentNullException();
      if (this.ProtocolType != ProtocolType.Tcp || this.ProxyType == ProxyTypes.None || this.ProxyEndPoint == null)
        return base.BeginConnect(remoteEP, callback, state);
      this.CallBack = callback;
      if (this.ProxyType == ProxyTypes.Socks4)
      {
        this.AsyncResult = new Socks4Handler((Socket) this, this.ProxyUser).BeginNegotiate((IPEndPoint) remoteEP, new HandShakeComplete(this.OnHandShakeComplete), this.ProxyEndPoint);
        return (IAsyncResult) this.AsyncResult;
      }
      if (this.ProxyType != ProxyTypes.Socks5)
        return (IAsyncResult) null;
      this.AsyncResult = new Socks5Handler((Socket) this, this.ProxyUser, this.ProxyPass).BeginNegotiate((IPEndPoint) remoteEP, new HandShakeComplete(this.OnHandShakeComplete), this.ProxyEndPoint);
      return (IAsyncResult) this.AsyncResult;
    }

    public new IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
    {
      if (host == null || callback == null)
        throw new ArgumentNullException();
      if (port <= 0 || port > (int) ushort.MaxValue)
        throw new ArgumentException();
      this.CallBack = callback;
      if (this.ProtocolType != ProtocolType.Tcp || this.ProxyType == ProxyTypes.None || this.ProxyEndPoint == null)
      {
        this.RemotePort = port;
        this.AsyncResult = this.BeginDns(host, new HandShakeComplete(this.OnHandShakeComplete));
        return (IAsyncResult) this.AsyncResult;
      }
      if (this.ProxyType == ProxyTypes.Socks4)
      {
        this.AsyncResult = new Socks4Handler((Socket) this, this.ProxyUser).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), this.ProxyEndPoint);
        return (IAsyncResult) this.AsyncResult;
      }
      if (this.ProxyType != ProxyTypes.Socks5)
        return (IAsyncResult) null;
      this.AsyncResult = new Socks5Handler((Socket) this, this.ProxyUser, this.ProxyPass).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), this.ProxyEndPoint);
      return (IAsyncResult) this.AsyncResult;
    }

    public new void EndConnect(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException();
      if (!asyncResult.IsCompleted)
        throw new ArgumentException();
      if (this.ToThrow != null)
        throw this.ToThrow;
    }

    internal IAsyncProxyResult BeginDns(string host, HandShakeComplete callback)
    {
      try
      {
        Dns.BeginResolve(host, new AsyncCallback(this.OnResolved), (object) this);
        return new IAsyncProxyResult();
      }
      catch
      {
        throw new SocketException();
      }
    }

    private void OnResolved(IAsyncResult asyncResult)
    {
      try
      {
        base.BeginConnect((EndPoint) new IPEndPoint(Dns.EndResolve(asyncResult).AddressList[0], this.RemotePort), new AsyncCallback(this.OnConnect), this.State);
      }
      catch (Exception ex)
      {
        this.OnHandShakeComplete(ex);
      }
    }

    private void OnConnect(IAsyncResult asyncResult)
    {
      try
      {
        base.EndConnect(asyncResult);
        this.OnHandShakeComplete((Exception) null);
      }
      catch (Exception ex)
      {
        this.OnHandShakeComplete(ex);
      }
    }

    private void OnHandShakeComplete(Exception error)
    {
      if (error != null)
        this.Close();
      this.ToThrow = error;
      this.AsyncResult.Reset();
      if (this.CallBack == null)
        return;
      this.CallBack((IAsyncResult) this.AsyncResult);
    }
  }
}
