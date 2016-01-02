using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
namespace AppUpdaterCom
{
	public class UnZipClass
	{
		public void UnZip(string zipPath, string zipFile)
		{
			ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFile));
			ZipEntry nextEntry;
			while ((nextEntry = zipInputStream.GetNextEntry()) != null)
			{
				string directoryName = Path.GetDirectoryName(zipPath);
				string fileName = Path.GetFileName(nextEntry.Name);
				Directory.CreateDirectory(directoryName);
				if (fileName != string.Empty)
				{
					string path = zipPath + "\\" + nextEntry.Name;
					FileStream fileStream = File.Create(path);
					byte[] array = new byte[2048];
					while (true)
					{
						int num = zipInputStream.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						fileStream.Write(array, 0, num);
					}
					fileStream.Close();
				}
			}
			zipInputStream.Close();
		}
	}
}
