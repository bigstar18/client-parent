using System;
using System.Data;
using System.IO;
using System.Xml;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	internal class CreateXmlFile
	{
		private static XmlDataSet XmlYJ;
		private static string FileName = "";
		public void CreateFile(string name)
		{
			CreateXmlFile.FileName = name;
			try
			{
				string text = CreateXmlFile.FileName.Substring(0, CreateXmlFile.FileName.IndexOf(".")) + ".xsd";
				string text2;
				if (CreateXmlFile.FileName.Contains("yjmessage"))
				{
					text2 = Global.ConfigPath + "yjmessage.xsd";
				}
				else
				{
					text2 = Global.ConfigPath + "yj.xsd";
				}
				if (!File.Exists(text2))
				{
					Environment.CurrentDirectory = Global.CurrentDirectory;
				}
				if (!File.Exists(text))
				{
					File.Copy(text2, text, true);
				}
				if (!File.Exists(CreateXmlFile.FileName))
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlNode newChild = xmlDocument.CreateNode(XmlNodeType.XmlDeclaration, "", "");
					xmlDocument.AppendChild(newChild);
					XmlElement newChild2 = xmlDocument.CreateElement("Context");
					xmlDocument.AppendChild(newChild2);
					xmlDocument.Save(CreateXmlFile.FileName);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				throw;
			}
		}
		public DataTable GetDataByXml(string strWhere, string strSort)
		{
			DataTable result = new DataTable();
			try
			{
				this.CreateFile(CreateXmlFile.FileName);
				CreateXmlFile.XmlYJ = new XmlDataSet(CreateXmlFile.FileName);
				result = CreateXmlFile.XmlYJ.GetDataViewByXml(strWhere, strSort).ToTable();
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
			return result;
		}
		public string WriteXmlData(string[] Columns, string[] ColumnValue)
		{
			string result = "";
			try
			{
				CreateXmlFile.XmlYJ = new XmlDataSet(CreateXmlFile.FileName);
				result = CreateXmlFile.XmlYJ.WriteXmlByDataSet(Columns, ColumnValue);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
			return result;
		}
		public bool UpdateXmlData(string[] Columns, string[] ColumnValue, string strWhereColumnName, string strWhereColumnValue)
		{
			bool result = false;
			try
			{
				CreateXmlFile.XmlYJ = new XmlDataSet(CreateXmlFile.FileName);
				result = CreateXmlFile.XmlYJ.UpdateXmlRow(Columns, ColumnValue, strWhereColumnName, strWhereColumnValue);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
				return false;
			}
			return result;
		}
	}
}
