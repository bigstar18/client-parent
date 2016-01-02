using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
namespace AppUpdate
{
	internal class AppUpdater
	{
		private string _updaterUrl;
		private bool _updaterOutTrader;
		public string UpdaterUrl
		{
			get
			{
				return this._updaterUrl;
			}
			set
			{
				this._updaterUrl = value;
			}
		}
		public bool UpdaterOutTrader
		{
			get
			{
				return this._updaterOutTrader;
			}
			set
			{
				this._updaterOutTrader = value;
			}
		}
		public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
		{
			updateFileList = new Hashtable();
			int result;
			try
			{
				if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
				{
					result = -1;
				}
				else
				{
					XmlFiles xmlFiles = new XmlFiles(serverXmlFile);
					XmlFiles xmlFiles2 = new XmlFiles(localXmlFile);
					XmlNodeList nodeList = xmlFiles.GetNodeList("AutoUpdater/Files");
					int num = 0;
					try
					{
						num = int.Parse(xmlFiles.GetNodeValue("//Level"));
					}
					catch
					{
						num = 0;
					}
					XmlNodeList nodeList2 = xmlFiles2.GetNodeList("AutoUpdater/Files");
					int num2 = 0;
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < nodeList2.Count; i++)
					{
						string value = nodeList2.Item(i).Attributes["Name"].Value.Trim();
						string value2 = nodeList2.Item(i).Attributes["Ver"].Value.Trim();
						arrayList.Add(value);
						arrayList.Add(value2);
					}
					for (int j = 0; j < nodeList.Count; j++)
					{
						string[] array = new string[3];
						string text = nodeList.Item(j).Attributes["Name"].Value.Trim();
						string text2 = nodeList.Item(j).Attributes["Ver"].Value.Trim();
						if (this._updaterOutTrader || text.Contains("\\"))
						{
							int num3 = arrayList.IndexOf(text);
							if (num3 == -1)
							{
								array[0] = text2;
								array[1] = text;
								updateFileList.Add(num2, array);
								num2++;
							}
							else if (num3 > -1 && text2.CompareTo(arrayList[num3 + 1].ToString()) > 0)
							{
								array[0] = text2;
								array[1] = text;
								updateFileList.Add(num2, array);
								num2++;
							}
						}
					}
					result = num;
				}
			}
			catch
			{
				result = -2;
			}
			return result;
		}
		public bool DownAutoUpdateFile(string downpath, string fileName, ref string error)
		{
			if (!Directory.Exists(downpath))
			{
				Directory.CreateDirectory(downpath);
			}
			string text = downpath + "\\" + fileName;
			string requestUriString = this.UpdaterUrl + "/" + fileName;
			bool result;
			try
			{
				WebRequest webRequest = WebRequest.Create(requestUriString);
				webRequest.Timeout = 30000;
				WebResponse response = webRequest.GetResponse();
				if (response.ContentLength > 0L)
				{
					long contentLength = response.ContentLength;
					Stream responseStream = response.GetResponseStream();
					byte[] array = new byte[contentLength];
					int num = array.Length;
					int num2 = 0;
					while (contentLength > 0L)
					{
						int num3 = responseStream.Read(array, num2, num);
						if (num3 == 0)
						{
							break;
						}
						num2 += num3;
						num -= num3;
					}
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					string text2 = downpath + "\\" + fileName.Replace(".exe", "_exe");
					FileStream fileStream = new FileStream(text2, FileMode.OpenOrCreate, FileAccess.Write);
					fileStream.Write(array, 0, array.Length);
					responseStream.Close();
					fileStream.Close();
					if (!text2.Equals(text))
					{
						File.Move(text2, text);
					}
				}
				result = true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				throw;
			}
			return result;
		}
		public bool DownAutoUpdateFile(string downpath, string fileName)
		{
			string empty = string.Empty;
			return this.DownAutoUpdateFile(downpath, fileName, ref empty);
		}
	}
}
