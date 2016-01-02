using System;
using System.Xml;
using ToolsLibrary.util;
namespace Gnnt.MEBS.HQModel
{
	public class SetInfo
	{
		public XmlDocument xmlDoc;
		private string SetXml;
		public int iStyle;
		public int ShowBuySellPrice;
		public int ShowMarketBtnCount;
		public int MultiQuoteStaticIndex;
		public string Language;
		public string CurButtonName;
		public string CurTimeMarketId;
		public string StrIndicator;
		public string SetColorStyle;
		public string AmountIncrease;
		public string MultiQuoteName;
		public string MultiQuoteItems;
		public string TransactionBillName;
		public string TransactionBillItems;
		public string StockMarketName;
		public string StockMarketItems;
		public bool ISStandalone;
		public bool ISDebug;
		public static RHColor RHColor;
		public string RightButtonItems;
		public string ABVOLS;
		public void init(string ConfigPath)
		{
			this.SetXml = ConfigPath + "Set.xml";
			this.xmlDoc = new XmlDocument();
			this.xmlDoc.Load(this.SetXml);
			XmlElement xmlElement = (XmlElement)this.xmlDoc.SelectSingleNode("SetInfo");
			if (xmlElement.SelectSingleNode("iStyle") != null)
			{
				this.iStyle = Tools.StrToInt(xmlElement.SelectSingleNode("iStyle").InnerText, 0);
			}
			if (xmlElement.SelectSingleNode("ShowBuySellPrice") != null)
			{
				this.ShowBuySellPrice = Tools.StrToInt(xmlElement.SelectSingleNode("ShowBuySellPrice").InnerText, 0);
			}
			if (xmlElement.SelectSingleNode("MultiQuoteStaticIndex") != null)
			{
				this.MultiQuoteStaticIndex = Tools.StrToInt(xmlElement.SelectSingleNode("MultiQuoteStaticIndex").InnerText, 0);
			}
			if (xmlElement.SelectSingleNode("Language") != null)
			{
				this.Language = xmlElement.SelectSingleNode("Language").InnerText;
			}
			if (xmlElement.SelectSingleNode("CurButtonName") != null)
			{
				this.CurButtonName = xmlElement.SelectSingleNode("CurButtonName").InnerText;
			}
			SetInfo.RHColor = new RHColor(this.iStyle);
			if (xmlElement.SelectSingleNode("MultiQuoteName") != null)
			{
				this.MultiQuoteName = xmlElement.SelectSingleNode("MultiQuoteName").InnerText;
			}
			if (xmlElement.SelectSingleNode("MultiQuoteItems") != null)
			{
				this.MultiQuoteItems = xmlElement.SelectSingleNode("MultiQuoteItems").InnerText;
			}
			if (xmlElement.SelectSingleNode("CurTimeMarketId") != null)
			{
				this.CurTimeMarketId = xmlElement.SelectSingleNode("CurTimeMarketId").InnerText;
			}
			if (xmlElement.SelectSingleNode("TransactionBillName") != null)
			{
				this.TransactionBillName = xmlElement.SelectSingleNode("TransactionBillName").InnerText;
			}
			if (xmlElement.SelectSingleNode("TransactionBillItems") != null)
			{
				this.TransactionBillItems = xmlElement.SelectSingleNode("TransactionBillItems").InnerText;
			}
			if (xmlElement.SelectSingleNode("StockMarketName") != null)
			{
				this.StockMarketName = xmlElement.SelectSingleNode("StockMarketName").InnerText;
			}
			if (xmlElement.SelectSingleNode("StockMarketItems") != null)
			{
				this.StockMarketItems = xmlElement.SelectSingleNode("StockMarketItems").InnerText;
			}
			if (xmlElement.SelectSingleNode("StrIndicator") != null)
			{
				this.StrIndicator = xmlElement.SelectSingleNode("StrIndicator").InnerText;
			}
			if (xmlElement.SelectSingleNode("SetColorStyle") != null)
			{
				this.SetColorStyle = xmlElement.SelectSingleNode("SetColorStyle").InnerText;
			}
			if (xmlElement.SelectSingleNode("AmountIncrease") != null)
			{
				this.AmountIncrease = xmlElement.SelectSingleNode("AmountIncrease").InnerText;
			}
			if (xmlElement.SelectSingleNode("RightButtonItems") != null)
			{
				this.RightButtonItems = xmlElement.SelectSingleNode("RightButtonItems").InnerText;
			}
			if (xmlElement.SelectSingleNode("ABVOLS") != null)
			{
				this.ABVOLS = xmlElement.SelectSingleNode("ABVOLS").InnerText;
			}
			if (xmlElement.SelectSingleNode("ShowMarketBtnCount") != null)
			{
				this.ShowMarketBtnCount = Tools.StrToInt(xmlElement.SelectSingleNode("ShowMarketBtnCount").InnerText, 6);
			}
		}
		public void saveSetInfo(string key, string value)
		{
			XmlElement xmlElement = (XmlElement)this.xmlDoc.SelectSingleNode("SetInfo");
			xmlElement.SelectSingleNode(key).InnerText = value;
		}
		public void lastSave()
		{
			lock (this.xmlDoc)
			{
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(this.SetXml);
					if (this.xmlDoc.InnerText != xmlDocument.InnerText && this.xmlDoc.InnerXml.Contains("SetInfo"))
					{
						this.xmlDoc.Save(this.SetXml);
					}
				}
				catch
				{
				}
			}
		}
	}
}
