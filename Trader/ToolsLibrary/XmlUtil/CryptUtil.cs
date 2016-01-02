using System;
using System.IO;
using System.Text;
using System.Xml;
namespace XmlUtil
{
	public class CryptUtil
	{
		public static bool EntryptFileText(string cPath)
		{
			bool result;
			try
			{
				string text = File.ReadAllText(cPath, Encoding.Default);
				Console.WriteLine(text);
				Console.WriteLine("-----");
				if (CryptUtil.IsBase64(text))
				{
					Console.WriteLine("文件已加密！");
					result = true;
				}
				else
				{
					byte[] bytes = Encoding.Default.GetBytes(text);
					string contents = Convert.ToBase64String(bytes);
					File.WriteAllText(cPath, contents, Encoding.UTF8);
					result = true;
				}
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
				result = false;
			}
			return result;
		}
		public static string EntryptFileToString(string cPath)
		{
			string result;
			try
			{
				string text = File.ReadAllText(cPath, Encoding.Default);
				if (CryptUtil.IsBase64(text))
				{
					Console.WriteLine("文件已加密！");
					result = text;
				}
				else
				{
					byte[] bytes = Encoding.Default.GetBytes(text);
					string text2 = Convert.ToBase64String(bytes);
					result = text2;
				}
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
				result = null;
			}
			return result;
		}
		private static bool IsBase64(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}
			bool result;
			try
			{
				Convert.FromBase64String(s);
				result = true;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}
		public static string DntryptFileToString(string cPath)
		{
			string result;
			try
			{
				string text = File.ReadAllText(cPath, Encoding.Default);
				if (!CryptUtil.IsBase64(text))
				{
					result = text;
				}
				else
				{
					byte[] bytes = Convert.FromBase64String(text);
					string @string = Encoding.Default.GetString(bytes);
					result = @string;
				}
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
				result = null;
			}
			return result;
		}
		public static XmlDocument DntryptStringToXml(string content)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDocument result;
			try
			{
				xmlDocument.LoadXml(content);
				result = xmlDocument;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				result = null;
			}
			return result;
		}
		public static XmlDocument DntryptFileToXml(string cPath)
		{
			return CryptUtil.DntryptStringToXml(CryptUtil.DntryptFileToString(cPath));
		}
		public static bool DntryptFileText(string cPath)
		{
			bool result;
			try
			{
				string text = File.ReadAllText(cPath, Encoding.Default);
				Console.WriteLine(text);
				Console.WriteLine("-----");
				if (!CryptUtil.IsBase64(text))
				{
					Console.WriteLine("该文件未加密，无需解密");
					result = true;
				}
				else
				{
					byte[] bytes = Convert.FromBase64String(text);
					string @string = Encoding.Default.GetString(bytes);
					File.WriteAllText(cPath, @string, Encoding.UTF8);
					result = true;
				}
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
				result = false;
			}
			return result;
		}
	}
}
