using System;
using System.Diagnostics;
using System.IO;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class WriteLog
	{
		private string filename = string.Empty;
		private string strPath = string.Empty;
		private bool isDelOld;
		private static volatile WriteLog instance;
		protected WriteLog(bool isDelOld)
		{
			this.strPath = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
			this.filename = "TradeClientAppLog.txt";
			this.isDelOld = isDelOld;
		}
		private static WriteLog Instance(bool isDelOld)
		{
			if (WriteLog.instance == null)
			{
				lock (typeof(WriteLog))
				{
					if (WriteLog.instance == null)
					{
						WriteLog.instance = new WriteLog(isDelOld);
					}
				}
			}
			return WriteLog.instance;
		}
		public static void WriteMsg(string sMsg)
		{
			if (Global.IsWriteLog)
			{
				WriteLog writeLog = WriteLog.Instance(true);
				writeLog.Write(sMsg);
				Console.WriteLine(sMsg);
			}
		}
		public static void Dispose()
		{
			if (WriteLog.instance != null)
			{
				WriteLog.instance = null;
			}
		}
		public void Write(string sMsg)
		{
			if (sMsg != "")
			{
				try
				{
					FileInfo fileInfo = new FileInfo(this.strPath + this.filename);
					if (!fileInfo.Exists)
					{
						this.CreateDirtory(fileInfo.FullName);
						using (StreamWriter streamWriter = fileInfo.CreateText())
						{
							streamWriter.WriteLine(string.Concat(new object[]
							{
								DateTime.Now,
								"\n",
								sMsg,
								"\n"
							}));
							streamWriter.Close();
							goto IL_175;
						}
					}
					fileInfo.Attributes = FileAttributes.Archive;
					if (fileInfo.LastWriteTime.ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")) || !this.isDelOld)
					{
						using (StreamWriter streamWriter2 = fileInfo.AppendText())
						{
							streamWriter2.WriteLine(string.Concat(new object[]
							{
								DateTime.Now,
								"\n",
								sMsg,
								"\n"
							}));
							streamWriter2.Close();
							goto IL_175;
						}
					}
					using (StreamWriter streamWriter3 = fileInfo.CreateText())
					{
						streamWriter3.WriteLine(string.Concat(new object[]
						{
							DateTime.Now,
							"\n",
							sMsg,
							"\n"
						}));
						streamWriter3.Close();
					}
					IL_175:;
				}
				catch (Exception)
				{
				}
			}
		}
		public void WriteEventLog(string sMsg, int errorType)
		{
			if (sMsg != "")
			{
				EventLog eventLog = null;
				string text = "gnnt";
				if (!EventLog.SourceExists(text))
				{
					EventLog.CreateEventSource(text, text + "Log");
				}
				if (eventLog == null)
				{
					eventLog = new EventLog(text + "Log");
					eventLog.Source = text;
				}
				EventLogEntryType type;
				switch (errorType)
				{
				case 0:
					type = EventLogEntryType.Error;
					break;
				case 1:
					type = EventLogEntryType.FailureAudit;
					break;
				case 2:
					type = EventLogEntryType.Information;
					break;
				case 3:
					type = EventLogEntryType.SuccessAudit;
					break;
				case 4:
					type = EventLogEntryType.Warning;
					break;
				default:
					type = EventLogEntryType.Error;
					break;
				}
				eventLog.WriteEntry(sMsg, type);
				eventLog.Dispose();
				eventLog.Close();
			}
		}
		private void CreateDirtory(string path)
		{
			if (!File.Exists(path))
			{
				string[] array = path.Split(new char[]
				{
					'\\'
				});
				string text = string.Empty;
				for (int i = 0; i < array.Length - 1; i++)
				{
					text = text + array[i].Trim() + "\\";
					if (!Directory.Exists(text))
					{
						DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
						directoryInfo.Attributes = FileAttributes.Archive;
					}
				}
			}
		}
	}
}
