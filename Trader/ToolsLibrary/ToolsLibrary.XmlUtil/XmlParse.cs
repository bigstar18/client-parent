using System;
using System.Collections;
using System.IO;
using System.Xml;
using XmlUtil;
namespace ToolsLibrary.XmlUtil
{
	public class XmlParse
	{
		private static object XmlConfigA = new object();
		private static string elementName = string.Empty;
		private static Stack stack = null;
		private static string root = string.Empty;
		public static Hashtable getSection(Hashtable configInfo, string sectionPath)
		{
			return (Hashtable)configInfo[sectionPath];
		}
		public static Hashtable ParseXml(string cPath)
		{
			Hashtable result;
			lock (XmlParse.XmlConfigA)
			{
				Hashtable hashtable = new Hashtable();
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument = CryptUtil.DntryptFileToXml(cPath);
					XmlParse.stack = new Stack(2048);
					XmlNodeList childNodes = xmlDocument.ChildNodes;
					foreach (XmlNode xmlNode in childNodes)
					{
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							XmlParse.root = xmlNode.Name;
						}
					}
					XmlParse.parseXml(childNodes, ref hashtable);
				}
				catch (FileNotFoundException ex)
				{
					Console.WriteLine(ex.ToString());
				}
				catch (Exception ex2)
				{
					Console.WriteLine(ex2.ToString());
				}
				result = hashtable;
			}
			return result;
		}
		private static void parseXml(XmlNodeList nodeList, ref Hashtable configInfo)
		{
			Hashtable hashtable = new Hashtable();
			foreach (XmlNode xmlNode in nodeList)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.HasChildNodes && !(xmlElement.ChildNodes[0] is XmlText))
					{
						if (xmlElement.HasAttributes)
						{
							XmlParse.elementName = xmlElement.Attributes[0].Value;
						}
						else
						{
							XmlParse.elementName = xmlElement.Name;
						}
						XmlParse.stack.Push(XmlParse.elementName);
						XmlParse.parseXml(xmlNode.ChildNodes, ref configInfo);
						if (hashtable.ContainsKey(XmlParse.elementName))
						{
							Console.WriteLine("重复添加相同的key");
						}
						Hashtable hashtable2 = new Hashtable();
						if (XmlParse.stack.Peek() is string)
						{
							XmlParse.elementName = (string)XmlParse.stack.Pop();
						}
						else if (xmlNode.Name == XmlParse.root)
						{
							hashtable2 = null;
						}
						else
						{
							hashtable2 = (Hashtable)XmlParse.stack.Pop();
							XmlParse.elementName = (string)XmlParse.stack.Pop();
						}
						if (hashtable2 != null && hashtable2.Count != 0)
						{
							hashtable.Add(XmlParse.elementName, hashtable2);
							if (xmlNode.NextSibling == null)
							{
								XmlParse.stack.Push(hashtable);
							}
						}
						else if (XmlParse.stack.Peek() is Hashtable)
						{
							configInfo = (Hashtable)XmlParse.stack.Pop();
						}
					}
					else
					{
						if (xmlElement.ChildNodes[0] == null)
						{
							hashtable.Add(xmlElement.Name, "");
						}
						if (xmlElement.ChildNodes[0] is XmlText)
						{
							hashtable.Add(xmlElement.Name, xmlElement.InnerText);
						}
						if (xmlElement.NextSibling == null)
						{
							XmlParse.stack.Push(hashtable);
						}
					}
				}
			}
		}
		public static string GetXmlFullPath(string strPath)
		{
			if (strPath.IndexOf(":") > 0)
			{
				return strPath;
			}
			return Environment.CurrentDirectory + "/" + strPath;
		}
	}
}
