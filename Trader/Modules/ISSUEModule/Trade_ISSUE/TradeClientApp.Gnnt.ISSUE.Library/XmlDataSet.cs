using System;
using System.Data;
using System.IO;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class XmlDataSet
	{
		private string strXmlPath;
		public XmlDataSet(string strXmlPath)
		{
			this.strXmlPath = strXmlPath;
		}
		public DataSet GetDataSetByXml()
		{
			DataSet result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables.Count > 0)
				{
					result = dataSet;
				}
				else
				{
					result = null;
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
		public DataView GetDataViewByXml(string strWhere, string strSort)
		{
			DataView result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				DataView dataView = new DataView(dataSet.Tables[0]);
				if (strSort.Trim() != "")
				{
					dataView.Sort = strSort;
				}
				if (strWhere.Trim() != "")
				{
					dataView.RowFilter = strWhere;
				}
				result = dataView;
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
		public string WriteXmlByDataSet(string[] Columns, string[] ColumnValue)
		{
			string result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				DataTable dataTable = dataSet.Tables[0];
				DataRow dataRow = dataTable.NewRow();
				for (int i = 0; i < Columns.Length; i++)
				{
					dataRow[Columns[i]] = ColumnValue[i];
				}
				dataTable.Rows.Add(dataRow);
				dataTable.AcceptChanges();
				dataSet.AcceptChanges();
				dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				result = "true";
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				result = text;
			}
			return result;
		}
		public string WriteBoolXml(string[] Columns, string[] ColumnValue)
		{
			string result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				DataTable dataTable = dataSet.Tables[0];
				DataRow dataRow = dataTable.NewRow();
				if (Columns.Length > 0)
				{
					dataRow["Flag"] = bool.Parse(ColumnValue[0]);
				}
				for (int i = 1; i < Columns.Length; i++)
				{
					dataRow[Columns[i]] = ColumnValue[i];
				}
				dataTable.Rows.Add(dataRow);
				dataTable.AcceptChanges();
				dataSet.AcceptChanges();
				dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				result = "true";
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				result = text;
			}
			return result;
		}
		public bool UpdateXmlRow(string[] Columns, string[] ColumnValue, string strWhereColumnName, string strWhereColumnValue)
		{
			bool result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
					{
						dataSet.Tables[0].Rows[i][strWhereColumnName].ToString().Trim();
						if (dataSet.Tables[0].Rows[i][strWhereColumnName].ToString().Trim().Equals(strWhereColumnValue))
						{
							for (int j = 0; j < Columns.Length; j++)
							{
								dataSet.Tables[0].Rows[i][Columns[j]] = ColumnValue[j];
							}
							dataSet.AcceptChanges();
							dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
						}
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public bool UpdateXmlCounter(int num)
		{
			bool result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables[1].Rows.Count > 0)
				{
					dataSet.Tables[1].Rows[0][0] = num;
					dataSet.AcceptChanges();
					dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public bool DeleteXmlRowByIndex(int iDeleteRow)
		{
			bool result;
			try
			{
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					dataSet.Tables[0].Rows[iDeleteRow].Delete();
				}
				dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public string DeleteXmlRows(string strColumn, string[] ColumnValue)
		{
			string result;
			try
			{
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					if (ColumnValue.Length > dataSet.Tables[0].Rows.Count)
					{
						for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
						{
							for (int j = 0; j < ColumnValue.Length; j++)
							{
								if (dataSet.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
								{
									dataSet.Tables[0].Rows[i].Delete();
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < ColumnValue.Length; k++)
						{
							for (int l = 0; l < dataSet.Tables[0].Rows.Count; l++)
							{
								if (dataSet.Tables[0].Rows[l][strColumn].ToString().Trim().Equals(ColumnValue[k]))
								{
									dataSet.Tables[0].Rows[l].Delete();
								}
							}
						}
					}
					dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				}
				result = "true";
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				result = text;
			}
			return result;
		}
		public bool DeleteXmlAllRows()
		{
			bool result;
			try
			{
				string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
				DataSet dataSet = new DataSet();
				dataSet.ReadXmlSchema(this.GetXmlFullPath(strPath));
				dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					dataSet.Tables[0].Rows.Clear();
				}
				dataSet.WriteXml(this.GetXmlFullPath(this.strXmlPath));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public string GetXmlFullPath(string strPath)
		{
			if (strPath.IndexOf(":") > 0)
			{
				return strPath;
			}
			return Environment.CurrentDirectory + "/" + strPath;
		}
		public void WriteXmlSchema()
		{
			string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
			dataSet.WriteXmlSchema(this.GetXmlFullPath(strPath));
		}
		public void WriteNewXmlSchema(string xmlFileName)
		{
			string strPath = xmlFileName.Substring(0, xmlFileName.IndexOf(".")) + ".xsd";
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(this.GetXmlFullPath(this.strXmlPath));
			dataSet.WriteXmlSchema(this.GetXmlFullPath(strPath));
			for (int i = dataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
			{
				dataSet.Tables[0].Rows[i].Delete();
			}
			dataSet.WriteXml(this.GetXmlFullPath(xmlFileName));
		}
		public bool WriteNewXml(string fromFileName, string toFileName)
		{
			string path = fromFileName.Substring(0, fromFileName.IndexOf(".")) + ".xsd";
			if (!File.Exists(fromFileName) || !File.Exists(path))
			{
				throw new Exception("没有发现预埋委托架构文件！");
			}
			bool result;
			try
			{
				string text = fromFileName.Substring(0, fromFileName.IndexOf(".")) + ".xsd";
				string text2 = toFileName.Substring(0, toFileName.IndexOf(".")) + ".xsd";
				if (!File.Exists(toFileName) || !File.Exists(text2))
				{
					File.Copy(fromFileName, toFileName, true);
					File.Copy(text, text2, true);
				}
				else if (!this.FileCompare(text, text2))
				{
					File.Copy(fromFileName, toFileName, true);
					File.Copy(text, text2, true);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		private bool FileCompare(string file1, string file2)
		{
			int num = 0;
			int num2 = 0;
			using (FileStream fileStream = new FileStream(file1, FileMode.Open))
			{
				using (FileStream fileStream2 = new FileStream(file2, FileMode.Open))
				{
					if (fileStream.Length != fileStream2.Length)
					{
						fileStream.Close();
						fileStream2.Close();
						return false;
					}
					do
					{
						num = fileStream.ReadByte();
						num2 = fileStream2.ReadByte();
					}
					while (num == num2 && num != -1);
					fileStream.Close();
					fileStream2.Close();
				}
			}
			return num - num2 == 0;
		}
	}
}
