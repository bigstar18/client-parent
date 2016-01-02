namespace FuturesTrade
{
    using FuturesTrade.Gnnt.Library;
    using FuturesTrade.Gnnt.UI.Forms;
    using HttpTrade.Gnnt.MEBS.Lib;
    using PluginInterface;
    using System;
    using System.Resources;
    using System.Windows.Forms;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Global.IsWriteLog = true;
            Global.M_ResourceManager = ResourceManager.CreateFileBasedResourceManager("Gnnt.ch", "", null);
            Global.HTConfig = new Configuration().getSection("Trade");
            Global.TradeLibrary = new TradeLibrary();
            Global.TradeLibrary.IsWriteLog = true;
            Global.TradeLibrary.CommunicationUrl = "http://172.16.2.10:19924/tradeweb/httpXmlServlet";
            Global.TradeLibrary.CommunicationUrl = "http://59.45.103.239:10065/tradeweb/httpXmlServlet";
            Global.TradeLibrary.Initialize();
            LogonRequestVO req = new LogonRequestVO
            {
                UserID = "gnnttest",
                Password = "11111111"
            };
            Global.UserID = req.UserID;
            Global.Password = req.Password;
            Global.RegisterWord = req.RegisterWord;
            Global.TradeLibrary.Logon(req);
            //Global.TradeLibrary.CheckUser(req.UserID);
            Logger.InitLogger(LogType.Daily);
            Application.Run(new MainForm(0));
            Logger.UnInitLogger();
        }
    }
}
