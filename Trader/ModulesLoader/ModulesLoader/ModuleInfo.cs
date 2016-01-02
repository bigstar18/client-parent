using System;
namespace ModulesLoader
{
	public class ModuleInfo
	{
		private string moduleName;
		private string modulePath;
		private string configPath;
		private string moduleNo;
		public string ModuleName
		{
			get
			{
				return this.moduleName;
			}
			set
			{
				this.moduleName = value;
			}
		}
		public string ModulePath
		{
			get
			{
				return this.modulePath;
			}
			set
			{
				this.modulePath = value;
			}
		}
		public string ConfigPath
		{
			get
			{
				return this.configPath;
			}
			set
			{
				this.configPath = value;
			}
		}
		public string ModuleNo
		{
			get
			{
				return this.moduleNo;
			}
			set
			{
				this.moduleNo = value;
			}
		}
	}
}
