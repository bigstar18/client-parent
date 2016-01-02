namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Data;
    using System.IO;

    public class XmlDataSet
    {
        private string strXmlPath;

        public XmlDataSet(string strXmlPath)
        {
            this.strXmlPath = strXmlPath;
        }

        public bool DeleteXmlAllRows()
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables[0].Rows.Count > 0)
                {
                    set.Tables[0].Rows.Clear();
                }
                set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteXmlRowByIndex(int iDeleteRow)
        {
            try
            {
                DataSet set = new DataSet();
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables[0].Rows.Count > 0)
                {
                    set.Tables[0].Rows[iDeleteRow].Delete();
                }
                set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string DeleteXmlRows(string strColumn, string[] ColumnValue)
        {
            try
            {
                DataSet set = new DataSet();
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables[0].Rows.Count > 0)
                {
                    if (ColumnValue.Length > set.Tables[0].Rows.Count)
                    {
                        for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ColumnValue.Length; j++)
                            {
                                if (set.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                                {
                                    set.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < ColumnValue.Length; k++)
                        {
                            for (int m = 0; m < set.Tables[0].Rows.Count; m++)
                            {
                                if (set.Tables[0].Rows[m][strColumn].ToString().Trim().Equals(ColumnValue[k]))
                                {
                                    set.Tables[0].Rows[m].Delete();
                                }
                            }
                        }
                    }
                    set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                }
                return "true";
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        private bool FileCompare(string file1, string file2)
        {
            int num = 0;
            int num2 = 0;
            using (FileStream stream = new FileStream(file1, FileMode.Open))
            {
                using (FileStream stream2 = new FileStream(file2, FileMode.Open))
                {
                    if (stream.Length != stream2.Length)
                    {
                        stream.Close();
                        stream2.Close();
                        return false;
                    }
                    do
                    {
                        num = stream.ReadByte();
                        num2 = stream2.ReadByte();
                    }
                    while ((num == num2) && (num != -1));
                    stream.Close();
                    stream2.Close();
                }
            }
            return ((num - num2) == 0);
        }

        public DataSet GetDataSetByXml()
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables.Count > 0)
                {
                    return set;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataView GetDataViewByXml(string strWhere, string strSort)
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                DataView view = new DataView(set.Tables[0]);
                if (strSort.Trim() != "")
                {
                    view.Sort = strSort;
                }
                if (strWhere.Trim() != "")
                {
                    view.RowFilter = strWhere;
                }
                return view;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetXmlFullPath(string strPath)
        {
            if (strPath.IndexOf(":") > 0)
            {
                return strPath;
            }
            return (Environment.CurrentDirectory + "/" + strPath);
        }

        public bool UpdateXmlCounter(int num)
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables[1].Rows.Count > 0)
                {
                    set.Tables[1].Rows[0][0] = num;
                    set.AcceptChanges();
                    set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateXmlRow(string[] Columns, string[] ColumnValue, string strWhereColumnName, string strWhereColumnValue)
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                if (set.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                    {
                        set.Tables[0].Rows[i][strWhereColumnName].ToString().Trim();
                        if (set.Tables[0].Rows[i][strWhereColumnName].ToString().Trim().Equals(strWhereColumnValue))
                        {
                            for (int j = 0; j < Columns.Length; j++)
                            {
                                set.Tables[0].Rows[i][Columns[j]] = ColumnValue[j];
                            }
                            set.AcceptChanges();
                            set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string WriteBoolXml(string[] Columns, string[] ColumnValue)
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                DataTable table = set.Tables[0];
                DataRow row = table.NewRow();
                if (Columns.Length > 0)
                {
                    row["Flag"] = bool.Parse(ColumnValue[0]);
                }
                for (int i = 1; i < Columns.Length; i++)
                {
                    row[Columns[i]] = ColumnValue[i];
                }
                table.Rows.Add(row);
                table.AcceptChanges();
                set.AcceptChanges();
                set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                return "true";
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        public bool WriteNewXml(string fromFileName, string toFileName)
        {
            string path = fromFileName.Substring(0, fromFileName.IndexOf(".")) + ".xsd";
            if (!File.Exists(fromFileName) || !File.Exists(path))
            {
                throw new Exception("没有发现预埋委托架构文件！");
            }
            try
            {
                string sourceFileName = fromFileName.Substring(0, fromFileName.IndexOf(".")) + ".xsd";
                string str3 = toFileName.Substring(0, toFileName.IndexOf(".")) + ".xsd";
                if (!File.Exists(toFileName) || !File.Exists(str3))
                {
                    File.Copy(fromFileName, toFileName, true);
                    File.Copy(sourceFileName, str3, true);
                }
                else if (!this.FileCompare(sourceFileName, str3))
                {
                    File.Copy(fromFileName, toFileName, true);
                    File.Copy(sourceFileName, str3, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WriteNewXmlSchema(string xmlFileName)
        {
            string strPath = xmlFileName.Substring(0, xmlFileName.IndexOf(".")) + ".xsd";
            DataSet set = new DataSet();
            set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
            set.WriteXmlSchema(this.GetXmlFullPath(strPath));
            for (int i = set.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                set.Tables[0].Rows[i].Delete();
            }
            set.WriteXml(this.GetXmlFullPath(xmlFileName));
        }

        public string WriteXmlByDataSet(string[] Columns, string[] ColumnValue)
        {
            try
            {
                string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
                DataSet set = new DataSet();
                set.ReadXmlSchema(this.GetXmlFullPath(strPath));
                set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
                DataTable table = set.Tables[0];
                DataRow row = table.NewRow();
                for (int i = 0; i < Columns.Length; i++)
                {
                    row[Columns[i]] = ColumnValue[i];
                }
                table.Rows.Add(row);
                table.AcceptChanges();
                set.AcceptChanges();
                set.WriteXml(this.GetXmlFullPath(this.strXmlPath));
                return "true";
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        public void WriteXmlSchema()
        {
            string strPath = this.strXmlPath.Substring(0, this.strXmlPath.IndexOf(".")) + ".xsd";
            DataSet set = new DataSet();
            set.ReadXml(this.GetXmlFullPath(this.strXmlPath));
            set.WriteXmlSchema(this.GetXmlFullPath(strPath));
        }
    }
}
