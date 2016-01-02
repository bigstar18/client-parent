using FuturesTrade.Gnnt.Library;
using FuturesTrade.Gnnt.UI.Forms;
using HttpTrade.Gnnt.MEBS.Lib;
using PluginInterface;
using System;
using System.Resources;
using System.Windows.Forms;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.SetCompatibleTextRenderingDefault(false);
			Global.IsWriteLog = true;
			Global.M_ResourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.ch", "", null);
			Configuration configuration = new Configuration();
			Global.HTConfig = configuration.getSection("Trade");
			Global.TradeLibrary = new TradeLibrary();
			Global.TradeLibrary.IsWriteLog = true;
			Global.TradeLibrary.CommunicationUrl = "http://172.16.2.10:19924/tradeweb/httpXmlServlet";
			Global.TradeLibrary.CommunicationUrl = "http://59.45.103.239:10065/tradeweb/httpXmlServlet";
			Global.TradeLibrary.Initialize();
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.UserID = "gnnttest";
			logonRequestVO.Password = "11111111";
			Global.UserID = logonRequestVO.UserID;
			Global.Password = logonRequestVO.Password;
			Global.RegisterWord = logonRequestVO.RegisterWord;
			Global.TradeLibrary.Logon(logonRequestVO);
			Logger.InitLogger(LogType.Daily);
			Application.Run(new MainForm(0));
			Logger.UnInitLogger();
		}
	}
}
