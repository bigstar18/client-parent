using ModulesLoader;
using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
namespace SysFrame.Gnnt.Common.Operation
{
	public class SpeedTestOperation
	{
		public delegate void repaceListBoxLineCallBack(string lineStr, int indexId);
		public delegate void RefreshLoginComboBox(string itemname);
		private string TelecomStr = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_Telecom");
		private string UnicomStr = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_Unicom");
		public IPlugin myPlugin;
		public ListBox listBoxServerSpeed;
		public int fristSelectIndex;
		private Socket socket;
		private IPEndPoint hostEP;
		private ManualResetEvent connectDone = new ManualResetEvent(false);
		public SpeedTestOperation.repaceListBoxLineCallBack ListBoxOneLineCallBack;
		public SpeedTestOperation.RefreshLoginComboBox refreshLoginComboBox;
		private bool isRunServerLoad = true;
		private DataTable xmlDt;
		public void serverSpeedDefault()
		{
			this.listBoxServerSpeed.Items.Clear();
			DataSet dataSet = new DataSet();
			try
			{
				dataSet.ReadXml(this.myPlugin.get_ConfigFileName());
			}
			catch (Exception)
			{
				string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_getServerListError");
				this.listBoxServerSpeed.Items.Add(@string);
				return;
			}
			if (dataSet != null)
			{
				this.xmlDt = dataSet.Tables["ServerInfo"];
				int count = this.xmlDt.Rows.Count;
				for (int i = 0; i < count; i++)
				{
					string text = "  ";
					string str = this.xmlDt.Rows[i]["ServerName"].ToString();
					text += str;
					for (int j = text.Length; j < 20; j++)
					{
						text += "  ";
					}
					string text2 = "---";
					for (int k = text2.Length; k < 23; k++)
					{
						text2 += " ";
					}
					text += text2;
					string str2 = "------";
					text += str2;
					this.listBoxServerSpeed.Items.Add(text);
				}
				this.setServerInfoAfterIndex();
				return;
			}
		}
		private void setServerInfoAfterIndex()
		{
			PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)this.myPlugin.get_Host().get_HtConfigInfo()[this.myPlugin.get_Name()];
			XmlElement xmlElement;
			if (this.myPlugin.get_Name().Contains("OTC"))
			{
				xmlElement = (XmlElement)pluginConfigInfo.XmlDoc.SelectSingleNode("Root");
			}
			else
			{
				xmlElement = (XmlElement)pluginConfigInfo.XmlDoc.SelectSingleNode("ConfigInfo");
			}
			int count = this.listBoxServerSpeed.Items.Count;
			int num = Tools.StrToInt((string)Global.htConfig["CurServer"]);
			if (num == 0)
			{
				int num2 = Tools.StrToInt(xmlElement.SelectSingleNode("CurTelecomServer").InnerText.Trim());
				for (int i = 0; i < count; i++)
				{
					string text = this.listBoxServerSpeed.Items[i].ToString();
					if (text.IndexOf(this.TelecomStr) != -1)
					{
						if (num2 == -1)
						{
							string a = this.xmlDt.Rows[i]["IPAddress"].ToString();
							int num3 = Tools.StrToInt(this.xmlDt.Rows[i]["Port"].ToString());
							if (a == this.myPlugin.get_IpAddress() && num3 == this.myPlugin.get_Port())
							{
								this.listBoxServerSpeed.SelectedIndex = i;
								this.fristSelectIndex = i;
								return;
							}
						}
						else
						{
							if (num2 == 0)
							{
								this.listBoxServerSpeed.SelectedIndex = i;
								this.fristSelectIndex = i;
								return;
							}
							num2--;
						}
					}
				}
			}
			if (num == 1)
			{
				int num2 = Tools.StrToInt(xmlElement.SelectSingleNode("CurNetcomServer").InnerText.Trim());
				for (int j = 0; j < count; j++)
				{
					string text2 = this.listBoxServerSpeed.Items[j].ToString();
					if (text2.IndexOf(this.UnicomStr) != -1)
					{
						if (num2 == -1)
						{
							string a2 = this.xmlDt.Rows[j]["IPAddress"].ToString();
							int num4 = Tools.StrToInt(this.xmlDt.Rows[j]["Port"].ToString());
							if (a2 == this.myPlugin.get_IpAddress() && num4 == this.myPlugin.get_Port())
							{
								this.listBoxServerSpeed.SelectedIndex = j;
								this.fristSelectIndex = j;
								return;
							}
						}
						else
						{
							if (num2 == 0)
							{
								this.listBoxServerSpeed.SelectedIndex = j;
								this.fristSelectIndex = j;
								return;
							}
							num2--;
						}
					}
				}
			}
		}
		public void serverLoad()
		{
			if (this.isRunServerLoad)
			{
				WaitCallback callBack = new WaitCallback(this.ServerSpeed);
				ThreadPool.QueueUserWorkItem(callBack, null);
			}
		}
		private void ServerSpeed(object obj)
		{
			this.isRunServerLoad = false;
			if (this.xmlDt == null)
			{
				return;
			}
			int count = this.xmlDt.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				string text = "  ";
				if (i >= this.xmlDt.Rows.Count)
				{
					return;
				}
				string str = this.xmlDt.Rows[i]["ServerName"].ToString();
				string myIpAddress = this.xmlDt.Rows[i]["IPAddress"].ToString();
				text += str;
				for (int j = text.Length; j < 19; j++)
				{
					text += "  ";
				}
				int myPort = Tools.StrToInt(this.xmlDt.Rows[i]["Port"].ToString());
				int num = this.TextConnServer(myIpAddress, myPort);
				if (num == -1)
				{
					text += "N/A                  ";
				}
				else
				{
					string text2 = num.ToString() + "ms";
					for (int k = text2.Length; k < 24; k++)
					{
						text2 += " ";
					}
					text += text2;
				}
				if (num >= 0 && num <= 50)
				{
					string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_faster");
					text += @string;
				}
				else if (num > 50 && num <= 200)
				{
					string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_fast");
					text += string2;
				}
				else if (num > 200 && num <= 500)
				{
					string string3 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_slow");
					text += string3;
				}
				else if (num > 500 && num <= 2000)
				{
					string string4 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_slower");
					text += string4;
				}
				else if (num > 2000)
				{
					string string5 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_Slowest");
					text += string5;
				}
				else
				{
					string string6 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_UnableConnect");
					text += string6;
				}
				this.ListBoxOneLineCallBack(text, i);
				this.isRunServerLoad = true;
			}
		}
		private int TextConnServer(string myIpAddress, int myPort)
		{
			int result;
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(myIpAddress);
				IPAddress address = hostAddresses[0];
				IPEndPoint iPEndPoint = new IPEndPoint(address, myPort);
				Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				DateTime now = DateTime.Now;
				this.connectDone.Reset();
				socket.BeginConnect(iPEndPoint, new AsyncCallback(this.ConnectCallback), socket);
				this.connectDone.WaitOne(2000, false);
				if (socket.Connected)
				{
					socket.Close();
					DateTime now2 = DateTime.Now;
					TimeSpan timeSpan = now2 - now;
					result = timeSpan.Seconds * 1000 + timeSpan.Milliseconds;
				}
				else
				{
					result = -1;
				}
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				Socket socket = (Socket)ar.AsyncState;
				socket.EndConnect(ar);
			}
			catch (Exception)
			{
			}
			finally
			{
				this.connectDone.Set();
			}
		}
		public void setServerInfo()
		{
			int num = 0;
			string text = this.listBoxServerSpeed.Items[this.listBoxServerSpeed.SelectedIndex].ToString();
			if (text.IndexOf(this.UnicomStr) != -1)
			{
				num = 1;
			}
			this.myPlugin.get_Host().get_ConfigurationInfo().updateValue("Systems", "CurServer", string.Concat(num));
			int count = this.listBoxServerSpeed.Items.Count;
			int num2 = -1;
			int num3 = -1;
			for (int i = 0; i < count; i++)
			{
				string text2 = this.listBoxServerSpeed.Items[i].ToString();
				if (text2.IndexOf(this.UnicomStr) != -1)
				{
					num3++;
				}
				else if (text2.IndexOf(this.TelecomStr) != -1)
				{
					num2++;
				}
				if (i == this.listBoxServerSpeed.SelectedIndex)
				{
					break;
				}
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.myPlugin.get_ConfigFileName());
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("ConfigInfo").ChildNodes;
			foreach (XmlNode xmlNode in childNodes)
			{
				if (num == 0)
				{
					if (xmlNode.Name == "CurTelecomServer")
					{
						xmlNode.InnerText = num2.ToString();
						break;
					}
				}
				else if (xmlNode.Name == "CurNetcomServer")
				{
					xmlNode.InnerText = num3.ToString();
					break;
				}
			}
			xmlDocument.Save(this.myPlugin.get_ConfigFileName());
			PluginConfigInfo pluginConfigInfo = (PluginConfigInfo)this.myPlugin.get_Host().get_HtConfigInfo()[this.myPlugin.get_Name()];
			XmlElement xmlElement = (XmlElement)pluginConfigInfo.XmlDoc.SelectSingleNode("ConfigInfo");
			if (num == 0)
			{
				xmlElement.SelectSingleNode("CurTelecomServer").InnerText = num2.ToString();
			}
			else
			{
				xmlElement.SelectSingleNode("CurNetcomServer").InnerText = num3.ToString();
			}
			if (this.listBoxServerSpeed.Items[this.listBoxServerSpeed.SelectedIndex].ToString().IndexOf(this.UnicomStr) != this.listBoxServerSpeed.Items[this.fristSelectIndex].ToString().IndexOf(this.UnicomStr))
			{
				WaitCallback callBack = new WaitCallback(this.InitialPlugin);
				ThreadPool.QueueUserWorkItem(callBack, null);
			}
			this.myPlugin.Initialize();
		}
		private void InitialPlugin(object obj)
		{
			foreach (AvailablePluginInfo availablePluginInfo in Global.Modules.get_Plugins().get_AvailablePlugins())
			{
				if (availablePluginInfo.get_Instance() != this.myPlugin)
				{
					availablePluginInfo.get_Instance().Initialize();
				}
			}
		}
	}
}
