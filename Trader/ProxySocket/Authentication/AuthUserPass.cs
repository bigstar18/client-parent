// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.Authentication.AuthUserPass
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network.ProxySocket;
using System;
using System.Net.Sockets;
using System.Text;

namespace Org.Mentalis.Network.ProxySocket.Authentication
{
  internal sealed class AuthUserPass : AuthMethod
  {
    private string m_Username;
    private string m_Password;

    private string Username
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

    public AuthUserPass(Socket server, string user, string pass)
      : base(server)
    {
      this.Username = user;
      this.Password = pass;
    }

    private byte[] GetAuthenticationBytes()
    {
      byte[] numArray = new byte[3 + this.Username.Length + this.Password.Length];
      numArray[0] = (byte) 1;
      numArray[1] = (byte) this.Username.Length;
      Array.Copy((Array) Encoding.ASCII.GetBytes(this.Username), 0, (Array) numArray, 2, this.Username.Length);
      numArray[this.Username.Length + 2] = (byte) this.Password.Length;
      Array.Copy((Array) Encoding.ASCII.GetBytes(this.Password), 0, (Array) numArray, this.Username.Length + 3, this.Password.Length);
      return numArray;
    }

    public override void Authenticate()
    {
      this.Server.Send(this.GetAuthenticationBytes());
      byte[] buffer = new byte[2];
      int offset = 0;
      while (offset != 2)
        offset += this.Server.Receive(buffer, offset, 2 - offset, SocketFlags.None);
      if ((int) buffer[1] != 0)
      {
        this.Server.Close();
        throw new ProxyException("Username/password combination rejected.");
      }
    }

    public override void BeginAuthenticate(HandShakeComplete callback)
    {
      this.CallBack = callback;
      this.Server.BeginSend(this.GetAuthenticationBytes(), 0, 3 + this.Username.Length + this.Password.Length, SocketFlags.None, new AsyncCallback(this.OnSent), (object) this.Server);
    }

    private void OnSent(IAsyncResult ar)
    {
      try
      {
        this.Server.EndSend(ar);
        this.Buffer = new byte[2];
        this.Server.BeginReceive(this.Buffer, 0, 2, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.CallBack(ex);
      }
    }

    private void OnReceive(IAsyncResult ar)
    {
      try
      {
        this.Received += this.Server.EndReceive(ar);
        if (this.Received == this.Buffer.Length)
        {
          if ((int) this.Buffer[1] != 0)
            throw new ProxyException("Username/password combination not accepted.");
          this.CallBack((Exception) null);
        }
        else
          this.Server.BeginReceive(this.Buffer, this.Received, this.Buffer.Length - this.Received, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.Server);
      }
      catch (Exception ex)
      {
        this.CallBack(ex);
      }
    }
  }
}
