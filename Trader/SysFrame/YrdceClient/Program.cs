using YrdceClient.Yrdce.Common.Operation;
using System;
using System.Windows.Forms;
using TPME.Log;
namespace YrdceClient
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			ProgramOperation.ApplicationAddException();
			ProgramOperation.RegistAssemblyResolve();
			Application.SetCompatibleTextRenderingDefault(false);
			Logger.InitLogger(LogType.Daily);
			ProgramOperation.ApplicationRun(args);
			Logger.UnInitLogger();
		}
	}
}
