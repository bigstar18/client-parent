// Decompiled with JetBrains decompiler
// Type: TestApp
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using Org.Mentalis.Network.ProxySocket;
using System;
using System.Text;

internal class TestApp
{
  private static void Main(string[] args)
  {
    try
    {
      ProxySocket proxySocket = new ProxySocket();
      proxySocket.GetSocket("www.mentalis.org", 80);
      proxySocket.Send(Encoding.ASCII.GetBytes("GET / HTTP/1.0\r\nHost: www.mentalis.org\r\n\r\n"));
      byte[] numArray = new byte[1024];
      for (int count = proxySocket.Receive(numArray); count > 0; count = proxySocket.Receive(numArray))
        Console.Write(Encoding.ASCII.GetString(numArray, 0, count));
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
    }
    Console.WriteLine("Press enter to continue...");
    Console.ReadLine();
  }
}
