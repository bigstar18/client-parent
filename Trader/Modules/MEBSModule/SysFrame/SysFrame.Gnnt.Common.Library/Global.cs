using ModulesLoader;
using System;
using System.Collections;
namespace SysFrame.Gnnt.Common.Library
{
	public class Global
	{
		public static ModuleService Modules = new ModuleService();
		public static Hashtable htArgs = new Hashtable();
		public static Hashtable htConfig = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems");
		public static string AppUpdaterComName = "AppUpdaterCom.exe";
		public static string ConsumerPluginName = "Consumer";
		public static string MainAccount = string.Empty;
		public static Hashtable ChildAccounts = new Hashtable();
		public static Hashtable ModuleInfos = new Hashtable();
	}
}
