using PluginInterface;
using System;
namespace ModulesLoader
{
	public class PluginCommunicateInfoEventArgs : EventArgs
	{
		private PluginCommunicateInfo pluginCommunicateInfo;
		public PluginCommunicateInfo PluginCommunicateInfo
		{
			get
			{
				return this.pluginCommunicateInfo;
			}
		}
		public PluginCommunicateInfoEventArgs(PluginCommunicateInfo pluginCommunicateInfo)
		{
			this.pluginCommunicateInfo = pluginCommunicateInfo;
		}
	}
}
