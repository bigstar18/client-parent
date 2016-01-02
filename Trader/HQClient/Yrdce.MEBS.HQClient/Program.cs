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
		public class SET
		{
			public PluginInfo pluginInfo = new PluginInfo();
			public string myConfigFileName = "OTC_HQServer.xml";
			public void HQServerConfigInit()
			{
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					this.myConfigFileName = "OTC_HQServer.xml";
				}
				else
				{
					this.myConfigFileName = "OTC_HQPlugin.xml";
				}
				if (!File.Exists("Gnnt.MEBS.ch.resources"))
				{
					MessageBox.Show("资源文件不存在！系统将退出");
					Application.Exit();
					return;
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.myConfigFileName);
				string text = string.Empty;
				string text2 = string.Empty;
				int num = 0;
				XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("ConfigInfo");
				if (xmlElement.SelectSingleNode("CurTelecomServer") != null)
				{
					text = xmlElement.SelectSingleNode("CurTelecomServer").InnerText.Trim();
				}
				if (xmlElement.SelectSingleNode("CurNetcomServer") != null)
				{
					text2 = xmlElement.SelectSingleNode("CurNetcomServer").InnerText.Trim();
				}
				if (xmlElement.SelectSingleNode("ServerType") != null)
				{
					num = Tools.StrToInt(xmlElement.SelectSingleNode("ServerType").InnerText.Trim());
				}
				int num2;
				XmlNode xmlNode;
				if (num == 0)
				{
					num2 = Tools.StrToInt(text);
					xmlNode = xmlElement.SelectSingleNode("AllTelecomServer");
				}
				else
				{
					num2 = Tools.StrToInt(text2);
					xmlNode = xmlElement.SelectSingleNode("AllNetcomServer");
				}
				if (num2 == -1)
				{
					Random random = new Random();
					num2 = random.Next(xmlNode.ChildNodes.Count);
				}
				XmlElement xmlElement2 = (XmlElement)xmlNode.ChildNodes[num2];
				if (xmlElement2.SelectSingleNode("IPAddress") != null)
				{
					this.pluginInfo.IPAddress = xmlElement2.SelectSingleNode("IPAddress").InnerText.Trim();
				}
				if (xmlElement2.SelectSingleNode("Port") != null)
				{
					this.pluginInfo.Port = Tools.StrToInt(xmlElement2.SelectSingleNode("Port").InnerText.Trim());
				}
				if (xmlElement2.SelectSingleNode("HttpPort") != null)
				{
					this.pluginInfo.HttpPort = Tools.StrToInt(xmlElement2.SelectSingleNode("HttpPort").InnerText.Trim());
				}
			}
		}
		[STAThread]
		private static void Main()
		{
			Program.SET sET = new Program.SET();
			sET.pluginInfo.IPAddress = "172.16.2.10";
			sET.pluginInfo.Port = 17922;
			sET.pluginInfo.HttpPort = 17923;
			if (!File.Exists("Set.xml"))
			{
				MessageBox.Show("配置文件不存在！系统将退出");
				Application.Exit();
				return;
			}
			Application.SetCompatibleTextRenderingDefault(false);
			sET.pluginInfo.HQResourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.MEBS.ch", "", null);
			Configuration.Configuration configuration = new Configuration.Configuration();
			sET.pluginInfo.HTConfig = configuration.getSection("HQSystems");
			Application.Run(new MainWindow(sET.pluginInfo));
		}
	}
}
