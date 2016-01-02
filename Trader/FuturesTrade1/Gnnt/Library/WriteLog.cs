namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class WriteLog
    {
        private string filename = string.Empty;
        private static volatile WriteLog instance;
        private bool isDelOld;
        private string strPath = string.Empty;

        protected WriteLog(bool isDelOld)
        {
            this.strPath = AppDomain.CurrentDomain.BaseDirectory + @"\log\";
            this.filename = "TradeClientAppLog.txt";
            this.isDelOld = isDelOld;
        }

        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] strArray = path.Split(new char[] { '\\' });
                string str = string.Empty;
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    str = str + strArray[i].Trim() + @"\";
                    if (!Directory.Exists(str))
                    {
                        Directory.CreateDirectory(str).Attributes = FileAttributes.Archive;
                    }
                }
            }
        }

        public static void Dispose()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        private static WriteLog Instance(bool isDelOld)
        {
            if (instance == null)
            {
                lock (typeof(WriteLog))
                {
                    if (instance == null)
                    {
                        instance = new WriteLog(isDelOld);
                    }
                }
            }
            return instance;
        }

        public void Write(string sMsg)
        {
            if (sMsg != "")
            {
                try
                {
                    FileInfo info = new FileInfo(this.strPath + this.filename);
                    if (!info.Exists)
                    {
                        this.CreateDirtory(info.FullName);
                        using (StreamWriter writer = info.CreateText())
                        {
                            writer.WriteLine(string.Concat(new object[] { DateTime.Now, "\n", sMsg, "\n" }));
                            writer.Close();
                            return;
                        }
                    }
                    info.Attributes = FileAttributes.Archive;
                    if (info.LastWriteTime.ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")) || !this.isDelOld)
                    {
                        using (StreamWriter writer2 = info.AppendText())
                        {
                            writer2.WriteLine(string.Concat(new object[] { DateTime.Now, "\n", sMsg, "\n" }));
                            writer2.Close();
                            return;
                        }
                    }
                    using (StreamWriter writer3 = info.CreateText())
                    {
                        writer3.WriteLine(string.Concat(new object[] { DateTime.Now, "\n", sMsg, "\n" }));
                        writer3.Close();
                    }
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
                EventLog log = null;
                string source = "gnnt";
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, source + "Log");
                }
                if (log == null)
                {
                    log = new EventLog(source + "Log")
                    {
                        Source = source
                    };
                }
                EventLogEntryType error = EventLogEntryType.Error;
                switch (errorType)
                {
                    case 0:
                        error = EventLogEntryType.Error;
                        break;

                    case 1:
                        error = EventLogEntryType.FailureAudit;
                        break;

                    case 2:
                        error = EventLogEntryType.Information;
                        break;

                    case 3:
                        error = EventLogEntryType.SuccessAudit;
                        break;

                    case 4:
                        error = EventLogEntryType.Warning;
                        break;

                    default:
                        error = EventLogEntryType.Error;
                        break;
                }
                log.WriteEntry(sMsg, error);
                log.Dispose();
                log.Close();
            }
        }

        public static void WriteMsg(string sMsg)
        {
            if (Global.IsWriteLog)
            {
                Instance(true).Write(sMsg);
                Console.WriteLine(sMsg);
            }
        }
    }
}
