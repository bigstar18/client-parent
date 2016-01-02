using AppUpdaterCom;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
namespace AppUpdate
{
	public class CheckForUpdate
	{
		private XmlFiles updaterXmlFiles;
		private XmlFiles telNetXmlFile;
		private string updateFileName = string.Empty;
		private string checkUpdateFile = string.Empty;
		private string updateUrl = string.Empty;
		private string tempUpdatePath = string.Empty;
		private int availableUpdate;
		private string VMLoadExe = string.Empty;
		private string updateExe = string.Empty;
		private string updateZip = string.Empty;
		public int UpdateLevel;
		private ResourceManager SysResourceManager;
		public CheckForUpdate(string updateFileName)
		{
			this.updateFileName = updateFileName;
			if (SysLanguage.language == 0)
			{
				this.SysResourceManager = ResourceManager.CreateFileBasedResourceManager("YrdUpdate.ch", "", null);
				return;
			}
            this.SysResourceManager = ResourceManager.CreateFileBasedResourceManager("YrdUpdate.en", "", null);
		}
		public bool StartUpdate()
		{
			if (File.Exists(this.updateExe) && File.Exists(this.tempUpdatePath + "\\" + this.checkUpdateFile))
			{
				if (this.VMLoadExe != null && this.VMLoadExe.Length > 0 && File.Exists(this.VMLoadExe) && Directory.Exists("..\\FW"))
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
					processStartInfo.FileName = Path.GetFileName(this.VMLoadExe);
					processStartInfo.Arguments = this.updateExe + " " + this.updateFileName;
					processStartInfo.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(this.VMLoadExe));
					try
					{
						Process.Start(processStartInfo);
						return true;
					}
					catch (Win32Exception ex)
					{
						string @string = this.SysResourceManager.GetString("UpdateStr_UpdateErrorStr");
						string string2 = this.SysResourceManager.GetString("UpdateStr_NoFoundFile");
						MessageBox.Show(string2 + this.VMLoadExe + "!" + ex.ToString(), @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						bool result = false;
						return result;
					}
				}
				if (!File.Exists(this.updateExe))
				{
					string string3 = this.SysResourceManager.GetString("UpdateStr_UpdateErrorStr");
					string string4 = this.SysResourceManager.GetString("UpdateStr_NoFoundFile");
					MessageBox.Show(string4 + this.updateExe + "!", string3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return false;
				}
				ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
				processStartInfo2.FileName = Path.GetFileName(this.updateExe);
				processStartInfo2.Arguments = this.updateFileName;
				processStartInfo2.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(this.updateExe));
				try
				{
					Process.Start(processStartInfo2);
				}
				catch (Win32Exception ex2)
				{
					string string5 = this.SysResourceManager.GetString("UpdateStr_UpdateErrorStr");
					string string6 = this.SysResourceManager.GetString("UpdateStr_NoFoundFile");
					MessageBox.Show(string6 + this.updateExe + "!" + ex2.ToString(), string5, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					bool result = false;
					return result;
				}
				return true;
			}
			return false;
		}
		public bool StartCheck()
		{
			bool result = false;
			if (this.updateFileName.Equals(""))
			{
				return result;
			}
			string text = AppDomain.CurrentDomain.BaseDirectory + "\\" + this.updateFileName;
			string serverXmlFile = string.Empty;
			string a = string.Empty;
			try
			{
				this.updaterXmlFiles = new XmlFiles(text);
			}
			catch
			{
				string @string = this.SysResourceManager.GetString("UpdateStr_Error");
				string string2 = this.SysResourceManager.GetString("UpdateStr_UpdateConfigerError");
				MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				bool result2 = false;
				return result2;
			}
			string xPath = "//TelUrl";
			try
			{
				this.telNetXmlFile = new XmlFiles(AppDomain.CurrentDomain.BaseDirectory + "\\Yrdce.xml");
				a = this.telNetXmlFile.GetNodeValue("//CurServer");
				if (a != "0")
				{
					xPath = "//NetUrl";
				}
			}
			catch (Exception)
			{
			}
			this.updateUrl = this.updaterXmlFiles.GetNodeValue(xPath);
			this.checkUpdateFile = this.updaterXmlFiles.GetNodeValue("//CheckUpdateFile");
			this.updateExe = this.updaterXmlFiles.GetNodeValue("//UpdateExe");
			this.VMLoadExe = this.updaterXmlFiles.GetNodeValue("//VMLoadExe");
			string extension = Path.GetExtension(this.updateExe);
			if (extension.Equals(".zip"))
			{
				this.updateZip = this.updateExe;
				int length = this.updateExe.LastIndexOf(".");
				this.updateExe = this.updateExe.Substring(0, length) + ".exe";
			}
			this.deleteFiles();
			if (this.updateUrl == null || this.updateUrl.Length == 0)
			{
				return false;
			}
			AppUpdater appUpdater = new AppUpdater();
			appUpdater.UpdaterUrl = this.updateUrl;
			if (this.VMLoadExe != null && this.VMLoadExe.Length > 0 && File.Exists(this.VMLoadExe) && Directory.Exists("..\\FW"))
			{
				appUpdater.UpdaterOutTrader = true;
			}
			string string3 = this.SysResourceManager.GetString("UpdateStr_Tip");
			try
			{
				this.tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + this.updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value;
				string empty = string.Empty;
				string text2 = string.Empty;
				int num = 0;
				do
				{
					try
					{
						num++;
						text2 = appUpdater.DownAutoUpdateFile(this.tempUpdatePath, this.updateFileName, ref empty).ToString();
					}
					catch (Exception)
					{
						if (num == 5)
						{
							throw;
						}
					}
				}
				while (text2.Equals(string.Empty) && num < 5);
			}
			catch
			{
				string string4 = this.SysResourceManager.GetString("UpdateStr_ConnectTimeOut");
                //MessageBox.Show(Form.ActiveForm, string4, string3, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				bool result2 = false;
				return result2;
			}
			Hashtable hashtable = new Hashtable();
			serverXmlFile = this.tempUpdatePath + "\\" + this.updateFileName;
			this.availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, text, out hashtable);
			if (this.availableUpdate == -1)
			{
				string string5 = this.SysResourceManager.GetString("UpdateStr_NoExistUpdateFile");
				MessageBox.Show(Form.ActiveForm, string5, string3, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.availableUpdate == -2)
			{
				string string6 = this.SysResourceManager.GetString("UpdateStr_UpdateFileError");
				MessageBox.Show(Form.ActiveForm, string6, string3, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				this.UpdateLevel = this.availableUpdate;
			}
			if (hashtable.Count > 0)
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(this.tempUpdatePath + "\\" + this.checkUpdateFile, null);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteComment("Need Update FileList");
				xmlTextWriter.WriteStartElement("Update");
				xmlTextWriter.WriteStartElement("Files");
				for (int i = 0; i < hashtable.Count; i++)
				{
					string[] array = (string[])hashtable[i];
					xmlTextWriter.WriteStartElement("File");
					xmlTextWriter.WriteAttributeString("Ver", array[0]);
					xmlTextWriter.WriteAttributeString("Name", array[1]);
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndDocument();
				xmlTextWriter.Flush();
				xmlTextWriter.Close();
				string string7 = this.SysResourceManager.GetString("UpdateStr_UpdateErrorStr");
				string string8 = this.SysResourceManager.GetString("UpdateStr_AdminRun");
				Version version = Environment.OSVersion.Version;
				if (this.updateZip.Equals(""))
				{
					if (!appUpdater.DownAutoUpdateFile(AppDomain.CurrentDomain.BaseDirectory, this.updateExe))
					{
						if (version.Major == 6)
						{
							MessageBox.Show(string.Format(string8, this.availableUpdate), string7, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						return false;
					}
				}
				else
				{
					if (!appUpdater.DownAutoUpdateFile(AppDomain.CurrentDomain.BaseDirectory, this.updateZip))
					{
						if (version.Major == 6)
						{
							MessageBox.Show(string.Format(string8, this.availableUpdate), string7, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						return false;
					}
					UnZipClass unZipClass = new UnZipClass();
					unZipClass.UnZip(AppDomain.CurrentDomain.BaseDirectory, this.updateZip);
					this.deleteFiles(this.updateZip);
				}
				result = true;
			}
			else
			{
				Directory.Delete(this.tempUpdatePath, true);
			}
			return result;
		}
		private void deleteFiles(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		private void deleteFiles()
		{
			try
			{
				if (File.Exists(this.updateExe))
				{
					File.Delete(this.updateExe);
				}
				if (File.Exists(this.checkUpdateFile))
				{
					File.Delete(this.checkUpdateFile);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
