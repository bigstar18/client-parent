using System;
using ToolsLibrary.XmlUtil;
namespace ToolsLibrary
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			XmlParse.ParseXml(Environment.CurrentDirectory + "/OTC_HQSystems.xml");
		}
	}
}
