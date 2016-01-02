using System;
using System.Xml;
namespace AppUpdate
{
	internal class XmlFiles : XmlDocument
	{
		private string _xmlFileName;
		public string XmlFileName
		{
			get
			{
				return this._xmlFileName;
			}
			set
			{
				this._xmlFileName = value;
			}
		}
		public XmlFiles(string xmlFile)
		{
			this.XmlFileName = xmlFile;
			this.Load(xmlFile);
		}
		public XmlNode FindNode(string xPath)
		{
			return base.SelectSingleNode(xPath);
		}
		public string GetNodeValue(string xPath)
		{
			XmlNode xmlNode = base.SelectSingleNode(xPath);
			if (xmlNode == null)
			{
				return string.Empty;
			}
			return xmlNode.InnerText;
		}
		public XmlNodeList GetNodeList(string xPath)
		{
			return base.SelectSingleNode(xPath).ChildNodes;
		}
	}
}
