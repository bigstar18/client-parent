using System;
using System.Threading;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.HQThread
{
	public abstract class MYThread : IDisposable
	{
		private Thread thdSubThread;
		protected Mutex mUnique = new Mutex();
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
			if (!this.blnStarted)
			{
				this.thdSubThread = new Thread(new ThreadStart(this.run));
				this.thdSubThread.Name = this.threadName;
				this.thdSubThread.IsBackground = true;
				this.blnIsStopped = false;
				this.blnStarted = true;
				this.thdSubThread.Start();
				Logger.wirte(MsgType.Information, "**********" + this.threadName + "开始！！！**********");
				return;
			}
			throw new Exception(this.thdSubThread.Name + "线程已经启动！");
		}
		protected abstract void run();
		protected abstract void disposeThread();
		public void Suspend()
		{
			if (this.blnStarted && !this.blnSuspended)
			{
				this.blnSuspended = true;
				this.mUnique.WaitOne();
			}
		}
		public void Resume()
		{
			if (this.blnStarted && this.blnSuspended)
			{
				this.blnSuspended = false;
				this.mUnique.ReleaseMutex();
			}
		}
		public void Abort(string stateInfo)
		{
			this.thdSubThread.Abort(stateInfo);
		}
		public void Stop()
		{
			if (this.blnStarted)
			{
				if (this.blnSuspended)
				{
					this.Resume();
				}
				this.blnStarted = false;
				this.blnIsStopped = true;
				this.disposeThread();
				this.thdSubThread.Join();
			}
		}
		public void Dispose()
		{
			this.Stop();
			GC.SuppressFinalize(this);
		}
	}
}
