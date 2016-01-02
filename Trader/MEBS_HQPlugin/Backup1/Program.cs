// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.Program
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Configuration;
using Gnnt.MEBS.HQModel;
using System;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;

namespace Gnnt.MEBS.HQClient
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Program.SET set = new Program.SET();
      set.pluginInfo.IPAddress = "172.16.2.10";
      set.pluginInfo.Port = 17922;
      set.pluginInfo.HttpPort = 17923;
      if (!File.Exists("Set.xml"))
      {
        int num = (int) MessageBox.Show("配置文件不存在！系统将退出");
        Application.Exit();
      }
      else
      {
        Application.SetCompatibleTextRenderingDefault(false);
        set.pluginInfo.HQResourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", (System.Type) null);
        Configuration configuration = new Configuration();
        set.pluginInfo.HTConfig = configuration.getSection("HQSystems");
        Application.Run((Form) new MainWindow(set.pluginInfo));
      }
    }

    public class SET
    {
      public PluginInfo pluginInfo = new PluginInfo();
      public string myConfigFileName = "OTC_HQServer.xml";

      public void HQServerConfigInit()
      {
        this.myConfigFileName = !Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false) ? "OTC_HQPlugin.xml" : "OTC_HQServer.xml";
        if (!File.Exists("Gnnt.MEBS.ch.resources"))
        {
          int num = (int) MessageBox.Show("资源文件不存在！系统将退出");
          Application.Exit();
        }
        else
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(this.myConfigFileName);
          string s1 = string.Empty;
          string s2 = string.Empty;
          int num = 0;
          XmlElement xmlElement1 = (XmlElement) xmlDocument.SelectSingleNode("ConfigInfo");
          if (xmlElement1.SelectSingleNode("CurTelecomServer") != null)
            s1 = xmlElement1.SelectSingleNode("CurTelecomServer").InnerText.Trim();
          if (xmlElement1.SelectSingleNode("CurNetcomServer") != null)
            s2 = xmlElement1.SelectSingleNode("CurNetcomServer").InnerText.Trim();
          if (xmlElement1.SelectSingleNode("ServerType") != null)
            num = Tools.StrToInt(xmlElement1.SelectSingleNode("ServerType").InnerText.Trim());
          int index;
          XmlNode xmlNode;
          if (num == 0)
          {
            index = Tools.StrToInt(s1);
            xmlNode = xmlElement1.SelectSingleNode("AllTelecomServer");
          }
          else
          {
            index = Tools.StrToInt(s2);
            xmlNode = xmlElement1.SelectSingleNode("AllNetcomServer");
          }
          if (index == -1)
            index = new Random().Next(xmlNode.ChildNodes.Count);
          XmlElement xmlElement2 = (XmlElement) xmlNode.ChildNodes[index];
          if (xmlElement2.SelectSingleNode("IPAddress") != null)
            this.pluginInfo.IPAddress = xmlElement2.SelectSingleNode("IPAddress").InnerText.Trim();
          if (xmlElement2.SelectSingleNode("Port") != null)
            this.pluginInfo.Port = Tools.StrToInt(xmlElement2.SelectSingleNode("Port").InnerText.Trim());
          if (xmlElement2.SelectSingleNode("HttpPort") == null)
            return;
          this.pluginInfo.HttpPort = Tools.StrToInt(xmlElement2.SelectSingleNode("HttpPort").InnerText.Trim());
        }
      }
    }
  }
}
