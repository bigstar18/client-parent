using ModulesLoader;
using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.UI.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame.Gnnt.Common.Operation
{
	public class ProgramOperation
	{
		public static void ApplicationAddException()
		{
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += new ThreadExceptionEventHandler(ProgramOperation.Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ProgramOperation.CurrentDomain_UnhandledException);
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			string str = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
			Exception exception = e.Exception;
			string str2;
			if (exception != null)
			{
				str2 = string.Format(str + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n", exception.GetType().Name, exception.Message, exception.StackTrace);
			}
			else
			{
				str2 = string.Format("应用程序线程错误:{0}", e);
			}
			ProgramOperation.writeLog(str2);
		}
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			string str = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
			string str2;
			if (ex != null)
			{
				str2 = string.Format(str + "Application UnhandledException:{0};\n\r堆栈信息:{1}", ex.Message, ex.StackTrace);
			}
			else
			{
				str2 = string.Format("Application UnhandledError:{0}", e);
			}
			ProgramOperation.writeLog(str2);
		}
		private static void writeLog(string str)
		{
			if (!Directory.Exists("ErrLog"))
			{
				Directory.CreateDirectory("ErrLog");
			}
			using (StreamWriter streamWriter = new StreamWriter("ErrLog\\ErrLog.txt", true))
			{
				streamWriter.WriteLine(str);
				streamWriter.WriteLine("---------------------------------------------------------");
				streamWriter.Close();
			}
		}
		public static void RegistAssemblyResolve()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += new ResolveEventHandler(ProgramOperation.MyResolveEventHandler);
		}
		private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
		{
			Assembly assembly = null;
			try
			{
				string needAddAssembly = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
				string currentDirectory = Environment.CurrentDirectory;
				assembly = ProgramOperation.LoadAssembly(assembly, needAddAssembly, currentDirectory);
				if (assembly == null)
				{
					foreach (ModuleInfo current in Global.Modules.get_Modules())
					{
						string currentDirectory2 = current.get_ModulePath().Substring(0, current.get_ModulePath().LastIndexOf("/"));
						assembly = ProgramOperation.LoadAssembly(assembly, needAddAssembly, currentDirectory2);
						if (assembly != null)
						{
							break;
						}
						assembly = ProgramOperation.LoadAssembly(assembly, needAddAssembly, current.get_ModulePath());
						if (assembly != null)
						{
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message);
			}
			return assembly;
		}
		private static Assembly LoadAssembly(Assembly MyAssembly, string needAddAssembly, string currentDirectory)
		{
			FileSystemInfo[] fileSystemInfos = Directory.CreateDirectory(currentDirectory).GetFileSystemInfos();
			if (fileSystemInfos != null)
			{
				FileSystemInfo[] array = fileSystemInfos;
				for (int i = 0; i < array.Length; i++)
				{
					FileSystemInfo fileSystemInfo = array[i];
					if (fileSystemInfo.Name.Contains(needAddAssembly))
					{
						MyAssembly = Assembly.LoadFrom(currentDirectory + "\\" + needAddAssembly);
						break;
					}
				}
			}
			return MyAssembly;
		}
		public static bool IsAdministrator()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}
		public static void AdministratorRun(string[] args)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = Application.ExecutablePath,
				Arguments = string.Join(" ", args),
				Verb = "runas"
			});
			Application.Exit();
			Environment.Exit(0);
		}
		public static void SetThreadPoolSize()
		{
			int num = Tools.StrToInt((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["MinThreads"], 5);
			int num2 = Tools.StrToInt((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["MaxThreads"], 25);
			ThreadPool.SetMinThreads(num, num);
			ThreadPool.SetMaxThreads(num2, num2);
		}
		public static void ApplicationRun(string[] args)
		{
			if (!ProgramOperation.IsAdministrator())
			{
				ProgramOperation.AdministratorRun(args);
				return;
			}
			ProgramOperation.SetThreadPoolSize();
			Global.Modules.LoadPlugins();
			Global.ModuleInfos.Clear();
			foreach (ModuleInfo current in Global.Modules.get_Modules())
			{
				Global.ModuleInfos.Add(current.get_ModuleNo(), current.get_ModuleName());
			}
			if (Tools.StrToInt((string)Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems")["ShowLoginPage"], 0) == 0)
			{
				Application.Run(new LoginForm(args, null, false));
				return;
			}
			Application.Run(new Frame(args));
		}
		public static void CreateSysFrame(string[] args)
		{
			Frame frame = new Frame(args);
			frame.Show();
		}
		public static void CreateLogin(IPlugin plugin, bool isFrame)
		{
			LoginForm loginForm = new LoginForm(null, plugin, isFrame);
			loginForm.ShowDialog();
		}
	}
}
