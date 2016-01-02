using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using System.Reflection;
using System.Xml;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public abstract class ReqVO
	{
		private ProtocolName name;
		public ProtocolName Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public ReqVO()
		{
		}
		public string toXmlString()
		{
			string xml = "<?xml version='1.0' encoding='gb2312'?><GNNT></GNNT>";
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("GNNT");
			XmlElement xmlElement = xmlDocument.CreateElement("REQ");
			xmlElement.SetAttribute("name", this.name.ToString());
			this.setObjToXmlStr(this, xmlDocument, xmlElement);
			xmlNode.AppendChild(xmlElement);
			return xmlDocument.InnerXml;
		}
		public void setObjToXmlStr(object sourceObj, XmlDocument xmlDoc, XmlElement xe1)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			FieldInfo[] fields = sourceObj.GetType().GetFields(bindingAttr);
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (fieldInfo.FieldType.Name.Equals("String"))
				{
					XmlElement xmlElement = xmlDoc.CreateElement(fieldInfo.Name);
					object value = fieldInfo.GetValue(sourceObj);
					if (value != null)
					{
						xmlElement.InnerText = value.ToString();
					}
					xe1.AppendChild(xmlElement);
				}
				else if (fieldInfo.FieldType.IsGenericType)
				{
					Type fieldType = fieldInfo.FieldType;
					Type arg_94_0 = fieldInfo.FieldType.GetGenericArguments()[0];
					object value2 = fieldInfo.GetValue(sourceObj);
					PropertyInfo property = fieldType.GetProperty("Count");
					int num = Convert.ToInt32(property.GetValue(value2, null));
					for (int j = 0; j < num; j++)
					{
						XmlElement xmlElement2 = xmlDoc.CreateElement(fieldInfo.Name);
						MethodInfo method = fieldType.GetMethod("get_Item");
						object obj = method.Invoke(value2, new object[]
						{
							j
						});
						if (obj != null)
						{
							this.setObjToXmlStr(obj, xmlDoc, xmlElement2);
							xe1.AppendChild(xmlElement2);
						}
					}
				}
				else if (fieldInfo.FieldType.IsClass)
				{
					XmlElement xmlElement3 = xmlDoc.CreateElement(fieldInfo.Name);
					object value3 = fieldInfo.GetValue(sourceObj);
					if (value3 != null)
					{
						this.setObjToXmlStr(value3, xmlDoc, xmlElement3);
						xe1.AppendChild(xmlElement3);
					}
				}
			}
		}
	}
}
