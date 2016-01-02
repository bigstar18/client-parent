using HttpTrade.Gnnt.ISSUE.Lib;
using PluginInterface;
using System;
using System.Resources;
using System.Windows.Forms;
using TradeClientApp.Gnnt.ISSUE;
using TradeClientApp.Gnnt.ISSUE.Library;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeClientApp
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Global.IsWriteLog = true;
			Global.M_ResourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.ch", "", null);
			Configuration configuration = new Configuration();
			Global.HTConfig = configuration.getSection("Trade");
			Global.TradeLibrary = new TradeLibrary();
			Global.TradeLibrary.IsWriteLog = true;
			Global.TradeLibrary.CommunicationUrl = " http://172.16.2.55:17081/issue_tradeweb/httpXmlServlet";
			Global.TradeLibrary.Initialize();
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.UserID = "lqy003";
			logonRequestVO.Password = "111111";
			logonRequestVO.UserID = "lh302";
			logonRequestVO.Password = "123123";
			Global.UserID = logonRequestVO.UserID;
			Global.Password = logonRequestVO.Password;
			Global.RegisterWord = logonRequestVO.RegisterWord;
			Global.TradeLibrary.Logon(logonRequestVO);
			Global.TradeLibrary.CheckUser(logonRequestVO.UserID);
			Application.Run(new MainForm());
		}
	}
}
