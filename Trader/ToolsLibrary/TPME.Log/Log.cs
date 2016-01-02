using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
namespace TPME.Log
{
	public class Log : IDisposable
	{
		private static Queue<Msg> msgs;
		private static string path;
		private static bool state;
		private static LogType type;
		private static DateTime TimeSign;
		private static StreamWriter writer;
		private Thread thread;
		public Log() : this(".\\", LogType.Daily)
		{
		}
		public Log(LogType t) : this(".\\log\\", t)
		{
		}
		public Log(string p, LogType t)
		{
			if (Log.msgs == null)
			{
				Log.state = true;
				Log.path = p;
				Log.type = t;
				Log.msgs = new Queue<Msg>();
				this.thread = new Thread(new ThreadStart(this.work));
				this.thread.Start();
			}
		}
		private void work()
		{
			while (true)
			{
				if (Log.msgs.Count > 0)
				{
					Msg msg = null;
					lock (Log.msgs)
					{
						msg = Log.msgs.Dequeue();
					}
					if (msg != null)
					{
						this.FileWrite(msg);
					}
				}
				else
				{
					if (!Log.state)
					{
						break;
					}
					Thread.Sleep(1);
				}
			}
			this.FileClose();
		}
		private string GetFilename()
		{
			DateTime now = DateTime.Now;
			string format = "";
			int value = 0;
			switch (Log.type)
			{
			case LogType.Daily:
				Log.TimeSign = new DateTime(now.Year, now.Month, now.Day);
				Log.TimeSign = Log.TimeSign.AddDays(1.0);
				format = "yyyyMMdd'.log'";
				value = 1;
				break;
			case LogType.Weekly:
				Log.TimeSign = new DateTime(now.Year, now.Month, now.Day);
				Log.TimeSign = Log.TimeSign.AddDays(7.0);
				format = "yyyyMMdd'.log'";
				value = 7;
				break;
			case LogType.Monthly:
				Log.TimeSign = new DateTime(now.Year, now.Month, 1);
				Log.TimeSign = Log.TimeSign.AddMonths(1);
				format = "yyyyMM'.log'";
				value = 30;
				break;
			case LogType.Annually:
				Log.TimeSign = new DateTime(now.Year, 1, 1);
				Log.TimeSign = Log.TimeSign.AddYears(1);
				format = "yyyy'.log'";
				value = 365;
				break;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Log.path);
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				FileInfo fileInfo = files[i];
				DateTime creationTime = fileInfo.CreationTime;
				if ((now - creationTime).TotalDays >= Convert.ToDouble(value))
				{
					fileInfo.Delete();
				}
			}
			return now.ToString(format);
		}
		private void FileWrite(Msg msg)
		{
			try
			{
				if (Log.writer == null)
				{
					this.FileOpen();
					this.FileWrite(msg);
				}
				else
				{
					if (DateTime.Now >= Log.TimeSign)
					{
						this.FileClose();
						this.FileOpen();
					}
					if (msg.tostring().Trim().Length > 0)
					{
						Log.writer.WriteLine(msg.tostring());
					}
					Log.writer.Flush();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}
		private void FileOpen()
		{
			Directory.CreateDirectory(Log.path);
			Log.writer = new StreamWriter(Log.path + this.GetFilename(), true, Encoding.UTF8);
		}
		private void FileClose()
		{
			if (Log.writer != null)
			{
				Log.writer.Flush();
				Log.writer.Close();
				Log.writer.Dispose();
				Log.writer = null;
			}
		}
		public void Write(Msg msg)
		{
			if (msg != null)
			{
				lock (Log.msgs)
				{
					Log.msgs.Enqueue(msg);
				}
			}
		}
		public void Write(string text, MsgType type)
		{
			this.Write(new Msg(text, type));
		}
		public void Write(DateTime dt, string text, MsgType type)
		{
			this.Write(new Msg(dt, text, type));
		}
		public void Write(Exception e, MsgType type)
		{
			this.Write(new Msg(e.Message, type));
		}
		public void Dispose()
		{
			Log.state = false;
			this.thread.Join();
		}
	}
}
