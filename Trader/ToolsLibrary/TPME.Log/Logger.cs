using System;
using System.Diagnostics;
namespace TPME.Log
{
	public class Logger
	{
		private static bool Init = false;
		private static Log log = null;
		private static object LockInit = new object();
		public static void InitLogger(LogType t)
		{
			lock (Logger.LockInit)
			{
				if (!Logger.Init)
				{
					Logger.log = new Log(t);
					Logger.Init = true;
				}
			}
		}
		public static void UnInitLogger()
		{
			Logger.log.Dispose();
			Logger.log = null;
			lock (Logger.LockInit)
			{
				Logger.Init = true;
			}
		}
		public static void wirte(MsgType msgT, string strinfo)
		{
			if (Logger.log != null)
			{
				if (strinfo.Length == 0)
				{
					return;
				}
				Msg msg = new Msg();
				msg.Type = msgT;
				msg.Text = strinfo;
                StackTrace stackTrace = new StackTrace(new StackFrame(1, true));
                StackFrame Client = stackTrace.GetFrame(0);
				if (msgT == MsgType.Error || msgT == MsgType.Warning)
				{
					msg.Info = string.Format("[文件名:]{0}\r\n[函数:]{1}\r\n[行数:]{2}", Client.GetFileName(), Client.GetMethod(), Client.GetFileLineNumber());
				}
				Logger.log.Write(msg);
			}
		}
		public static void wirte(Exception ex)
		{
			if (Logger.log != null)
			{
				if (ex == null)
				{
					return;
				}
				Msg msg = new Msg();
				msg.Type = MsgType.Error;
				msg.Text = ex.Message;
				msg.Info = ex.StackTrace;
				Logger.log.Write(msg);
			}
		}
	}
}
