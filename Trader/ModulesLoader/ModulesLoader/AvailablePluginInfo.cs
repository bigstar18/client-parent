using PluginInterface;
using System;
namespace ModulesLoader
{
	public class AvailablePluginInfo
	{
		private IPlugin myInstance;
		private string myAssemblyPath = "";
		public IPlugin Instance
		{
			get
			{
				return this.myInstance;
			}
			set
			{
				this.myInstance = value;
			}
		}
		public string AssemblyPath
		{
			get
			{
				return this.myAssemblyPath;
			}
			set
			{
				this.myAssemblyPath = value;
			}
		}
	}
}
