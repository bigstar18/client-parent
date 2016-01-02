// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.HQThread.MYThread
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.Threading;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
  public abstract class MYThread : IDisposable
  {
    protected Mutex mUnique = new Mutex();
    private Thread thdSubThread;
    protected bool blnIsStopped;
    private bool blnSuspended;
    private bool blnStarted;
    public string threadName;

    public bool IsStopped
    {
      get
      {
        return this.blnIsStopped;
      }
    }

    public bool IsSuspended
    {
      get
      {
        return this.blnSuspended;
      }
    }

    public MYThread()
    {
      this.blnIsStopped = true;
      this.blnSuspended = false;
      this.blnStarted = false;
    }

    public void Start()
    {
      if (this.blnStarted)
        throw new Exception(this.thdSubThread.Name + "线程已经启动！");
      this.thdSubThread = new Thread(new ThreadStart(this.run));
      this.thdSubThread.Name = this.threadName;
      this.thdSubThread.IsBackground = true;
      this.blnIsStopped = false;
      this.blnStarted = true;
      this.thdSubThread.Start();
      Logger.wirte(MsgType.Information, "**********" + this.threadName + "开始！！！**********");
    }

    protected abstract void run();

    protected abstract void disposeThread();

    public void Suspend()
    {
      if (!this.blnStarted || this.blnSuspended)
        return;
      this.blnSuspended = true;
      this.mUnique.WaitOne();
    }

    public void Resume()
    {
      if (!this.blnStarted || !this.blnSuspended)
        return;
      this.blnSuspended = false;
      this.mUnique.ReleaseMutex();
    }

    public void Abort(string stateInfo)
    {
      this.thdSubThread.Abort((object) stateInfo);
    }

    public void Stop()
    {
      if (!this.blnStarted)
        return;
      if (this.blnSuspended)
        this.Resume();
      this.blnStarted = false;
      this.blnIsStopped = true;
      this.disposeThread();
      this.thdSubThread.Join();
    }

    public void Dispose()
    {
      this.Stop();
      GC.SuppressFinalize((object) this);
    }
  }
}
