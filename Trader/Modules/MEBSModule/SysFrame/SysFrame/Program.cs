using SysFrame.Gnnt.Common.Operation;
using System;
using System.Windows.Forms;
using TPME.Log;
namespace SysFrame
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			ProgramOperation.ApplicationAddException();
			ProgramOperation.RegistAssemblyResolve();
			Application.SetCompatibleTextRenderingDefault(false);
			Logger.InitLogger(0);
			ProgramOperation.ApplicationRun(args);
			Logger.UnInitLogger();
		}
	}
}
