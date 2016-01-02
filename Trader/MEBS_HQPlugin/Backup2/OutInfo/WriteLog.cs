// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.OutInfo.WriteLog
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System;
using System.Diagnostics;
using System.IO;

namespace Gnnt.MEBS.HQModel.OutInfo
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
      this.filename = "HQLog.txt";
      this.isDelOld = isDelOld;
    }

    private static WriteLog Instance(bool isDelOld)
    {
      if (WriteLog.instance == null)
      {
        lock (typeof (WriteLog))
        {
          if (WriteLog.instance == null)
            WriteLog.instance = new WriteLog(isDelOld);
        }
      }
      return WriteLog.instance;
    }

    public static void WriteMsg(string sMsg)
    {
      WriteLog writeLog = WriteLog.Instance(true);
      Console.WriteLine(sMsg);
      writeLog.Write(sMsg);
    }

    public static void Dispose()
    {
      if (WriteLog.instance == null)
        return;
      WriteLog.instance = (WriteLog) null;
    }

    public void Write(string sMsg)
    {
      if (!(sMsg != ""))
        return;
      try
      {
        FileInfo fileInfo = new FileInfo(this.strPath + this.filename);
        if (!fileInfo.Exists)
        {
          this.CreateDirtory(fileInfo.FullName);
          using (StreamWriter text = fileInfo.CreateText())
          {
            text.WriteLine(string.Concat(new object[4]
            {
              (object) DateTime.Now,
              (object) "\n",
              (object) sMsg,
              (object) "\n"
            }));
            text.Close();
          }
        }
        else
        {
          fileInfo.Attributes = FileAttributes.Archive;
          if (fileInfo.LastWriteTime.ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")) || !this.isDelOld)
          {
            using (StreamWriter streamWriter = fileInfo.AppendText())
            {
              streamWriter.WriteLine(string.Concat(new object[4]
              {
                (object) DateTime.Now,
                (object) "\n",
                (object) sMsg,
                (object) "\n"
              }));
              streamWriter.Close();
            }
          }
          else
          {
            using (StreamWriter text = fileInfo.CreateText())
            {
              text.WriteLine(string.Concat(new object[4]
              {
                (object) DateTime.Now,
                (object) "\n",
                (object) sMsg,
                (object) "\n"
              }));
              text.Close();
            }
          }
        }
      }
      catch
      {
      }
    }

    public void WriteEventLog(string sMsg, int errorType)
    {
      if (!(sMsg != ""))
        return;
      EventLog eventLog = (EventLog) null;
      string source = "gnnt";
      if (!EventLog.SourceExists(source))
        EventLog.CreateEventSource(source, source + "Log");
      if (eventLog == null)
      {
        eventLog = new EventLog(source + "Log");
        eventLog.Source = source;
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

    private void CreateDirtory(string path)
    {
      if (File.Exists(path))
        return;
      string[] strArray = path.Split('\\');
      string path1 = string.Empty;
      for (int index = 0; index < strArray.Length - 1; ++index)
      {
        path1 = path1 + strArray[index].Trim() + "\\";
        if (!Directory.Exists(path1))
          Directory.CreateDirectory(path1).Attributes = FileAttributes.Archive;
      }
    }
  }
}
