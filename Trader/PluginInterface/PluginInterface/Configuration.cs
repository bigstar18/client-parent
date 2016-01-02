using System;
using System.Collections;
using System.IO;
using System.Xml;
namespace PluginInterface
{
	public class Configuration
	{
		private static bool initFlag;
		private static Hashtable configInfo = new Hashtable();
		private string cPath;
		private static object xmlConfigA = new object();
		private static object xmlConfigB = new object();
		private static object xmlConfigC = new object();
		public Configuration() : this(Configuration.getDefaultPath())
		{
		}
		public Configuration(string path)
		{
			lock (Configuration.xmlConfigA)
			{
				this.cPath = path;
				if (!Configuration.initFlag && this.resetConfigInfo() == 0)
				{
					Configuration.initFlag = true;
				}
			}
		}
		public Hashtable getSection(string sectionPath)
		{
			return (Hashtable)Configuration.configInfo[sectionPath];
		}
		public int resetConfigInfo()
		{
			int result;
			lock (Configuration.xmlConfigB)
			{
				int num = this.parseXML();
				if (num != 0)
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
		public bool updateValue(string sectionPath, string key, string value)
		{
			bool result;
			lock (Configuration.xmlConfigC)
			{
				bool flag = false;
				if (sectionPath == null || sectionPath.Length == 0)
				{
					result = false;
				}
				else if (key == null || key.Length == 0)
				{
					result = false;
				}
				else if (value == null || value.Length == 0)
				{
					result = false;
				}
				else
				{
					Hashtable section = this.getSection(sectionPath);
					if (section != null && section[key] != null)
					{
						try
						{
							XmlDocument xmlDocument = new XmlDocument();
							xmlDocument.Load(this.cPath);
							XmlNodeList childNodes = xmlDocument.SelectSingleNode("CONFIG").ChildNodes;
							foreach (XmlNode xmlNode in childNodes)
							{
								if (xmlNode.NodeType == XmlNodeType.Element)
								{
									XmlElement xmlElement = (XmlElement)xmlNode;
									if (string.Compare(xmlElement.GetAttribute("NAME"), sectionPath, true) == 0)
									{
										XmlNodeList childNodes2 = xmlElement.ChildNodes;
										IEnumerator enumerator2 = childNodes2.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												XmlNode xmlNode2 = (XmlNode)enumerator2.Current;
												if (xmlNode2.NodeType == XmlNodeType.Element)
												{
													XmlElement xmlElement2 = (XmlElement)xmlNode2;
													if (string.Compare(xmlElement2.Name, key, true) == 0)
													{
														xmlElement2.InnerText = value;
														section[key] = value;
														flag = true;
														xmlDocument.Save(this.cPath);
														break;
													}
												}
											}
											break;
										}
										finally
										{
											IDisposable disposable = enumerator2 as IDisposable;
											if (disposable != null)
											{
												disposable.Dispose();
											}
										}
									}
								}
							}
						}
						catch (FileNotFoundException ex)
						{
							Console.WriteLine(ex.ToString());
						}
					}
					result = flag;
				}
			}
			return result;
		}
		protected int parseXML()
		{
			int result = 0;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.cPath);
				XmlNodeList childNodes = xmlDocument.SelectSingleNode("CONFIG").ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (string.Compare(xmlElement.Name, "COMPONENT", true) == 0)
						{
							Hashtable hashtable = new Hashtable();
							XmlNodeList childNodes2 = xmlElement.ChildNodes;
							foreach (XmlNode xmlNode2 in childNodes2)
							{
								if (xmlNode2.NodeType == XmlNodeType.Element)
								{
									XmlElement xmlElement2 = (XmlElement)xmlNode2;
									if (hashtable.ContainsKey(xmlElement2.Name))
									{
										Console.WriteLine(xmlElement2.Name + "节点已经出现过一次");
									}
									else
									{
										hashtable.Add(xmlElement2.Name, xmlElement2.InnerText);
									}
								}
							}
							Configuration.configInfo.Add(xmlElement.GetAttribute("NAME"), hashtable);
						}
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				result = 1;
				Console.WriteLine(ex.ToString());
			}
			catch (Exception ex2)
			{
				result = 1;
				Console.WriteLine(ex2.ToString());
			}
			return result;
		}
		private static string getDefaultPath()
		{
			string text = "Yrdce.xml";
			if (text.IndexOf(":") > 0)
			{
				return text;
			}
			return Environment.CurrentDirectory + "/" + text;
		}
	}
}
