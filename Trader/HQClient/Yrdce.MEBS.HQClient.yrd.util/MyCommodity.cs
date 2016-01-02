using System;
using System.Collections;
using System.IO;
using System.Xml;
namespace Gnnt.MEBS.HQClient.gnnt.util
{
	public class MyCommodity
	{
		private string xmlName = string.Empty;
		private XmlDocument doc = new XmlDocument();
		public MyCommodity(string _xmlName)
		{
			this.xmlName = _xmlName;
			if (!File.Exists(this.xmlName))
			{
				this.WriteMyCommodity();
			}
		}
		private void WriteMyCommodity()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			XmlWriter xmlWriter = XmlWriter.Create(this.xmlName, xmlWriterSettings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("CommodityList");
			xmlWriter.WriteComment("商品列表");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Flush();
			xmlWriter.Close();
		}
		public ArrayList GetMyCommodity()
		{
			ArrayList arrayList = new ArrayList();
			this.doc.Load(this.xmlName);
			XmlNodeList childNodes = this.doc.SelectSingleNode("CommodityList").ChildNodes;
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes.Item(i).NodeType != XmlNodeType.Comment)
				{
					string innerText = childNodes.Item(i).InnerText;
					if (!arrayList.Contains(innerText))
					{
						arrayList.Add(innerText);
					}
				}
			}
			return arrayList;
		}
		public void AddMyCommodity(string commodityCode)
		{
			ArrayList myCommodity = this.GetMyCommodity();
			if (myCommodity.Contains(commodityCode))
			{
				return;
			}
			this.doc.Load(this.xmlName);
			XmlElement xmlElement = this.doc.CreateElement("CommodityCode");
			xmlElement.InnerText = commodityCode;
			this.doc.DocumentElement.AppendChild(xmlElement);
			this.doc.Save(this.xmlName);
		}
		public void DelMyCommodity(string commodityCode)
		{
			this.doc.Load(this.xmlName);
			XmlNode xmlNode = this.doc.SelectSingleNode("CommodityList");
			XmlNodeList childNodes = xmlNode.ChildNodes;
			int i = 0;
			while (i < childNodes.Count)
			{
				if (childNodes.Item(i).InnerText.Equals(commodityCode) && childNodes.Item(i).NodeType != XmlNodeType.Comment)
				{
					xmlNode.RemoveChild(childNodes.Item(i));
				}
				else
				{
					i++;
				}
			}
			this.doc.Save(this.xmlName);
		}
	}
}
