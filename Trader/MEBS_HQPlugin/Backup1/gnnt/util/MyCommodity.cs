// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.util.MyCommodity
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

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
      if (File.Exists(this.xmlName))
        return;
      this.WriteMyCommodity();
    }

    private void WriteMyCommodity()
    {
      XmlWriter xmlWriter = XmlWriter.Create(this.xmlName, new XmlWriterSettings()
      {
        Indent = true
      });
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
      for (int index = 0; index < childNodes.Count; ++index)
      {
        if (childNodes.Item(index).NodeType != XmlNodeType.Comment)
        {
          string innerText = childNodes.Item(index).InnerText;
          if (!arrayList.Contains((object) innerText))
            arrayList.Add((object) innerText);
        }
      }
      return arrayList;
    }

    public void AddMyCommodity(string commodityCode)
    {
      if (this.GetMyCommodity().Contains((object) commodityCode))
        return;
      this.doc.Load(this.xmlName);
      XmlElement element = this.doc.CreateElement("CommodityCode");
      element.InnerText = commodityCode;
      this.doc.DocumentElement.AppendChild((XmlNode) element);
      this.doc.Save(this.xmlName);
    }

    public void DelMyCommodity(string commodityCode)
    {
      this.doc.Load(this.xmlName);
      XmlNode xmlNode = this.doc.SelectSingleNode("CommodityList");
      XmlNodeList childNodes = xmlNode.ChildNodes;
      int index = 0;
      while (index < childNodes.Count)
      {
        if (childNodes.Item(index).InnerText.Equals(commodityCode) && childNodes.Item(index).NodeType != XmlNodeType.Comment)
          xmlNode.RemoveChild(childNodes.Item(index));
        else
          ++index;
      }
      this.doc.Save(this.xmlName);
    }
  }
}
