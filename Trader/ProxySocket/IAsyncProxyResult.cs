// Decompiled with JetBrains decompiler
// Type: Org.Mentalis.Network.ProxySocket.IAsyncProxyResult
// Assembly: ProxySocket, Version=3.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: C99B9CA8-2A8B-46F9-BFEC-566D35DF8146
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\ProxySocket.dll

using System;
using System.Threading;

namespace Org.Mentalis.Network.ProxySocket
{
  internal class IAsyncProxyResult : IAsyncResult
  {
    internal bool m_Completed = true;
    private object m_StateObject;
    private ManualResetEvent m_WaitHandle;

    public bool IsCompleted
    {
      get
      {
        return this.m_Completed;
      }
    }

    public bool CompletedSynchronously
    {
      get
      {
        return false;
      }
    }

    public object AsyncState
    {
      get
      {
        return this.m_StateObject;
      }
    }

    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this.m_WaitHandle == null)
          this.m_WaitHandle = new ManualResetEvent(false);
        return (WaitHandle) this.m_WaitHandle;
      }
    }

    internal void Init(object stateObject)
    {
      this.m_StateObject = stateObject;
      this.m_Completed = false;
      if (this.m_WaitHandle == null)
        return;
      this.m_WaitHandle.Reset();
    }

    internal void Reset()
    {
      this.m_StateObject = (object) null;
      this.m_Completed = true;
      if (this.m_WaitHandle == null)
        return;
      this.m_WaitHandle.Set();
    }
  }
}
