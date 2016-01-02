using System;
using System.Collections;
using TPME.Log;
namespace ModulesLoader
{
	public class AvailablePlugins : CollectionBase
	{
		public void Add(AvailablePluginInfo pluginToAdd)
		{
			if (!base.List.Contains(pluginToAdd))
			{
				base.List.Add(pluginToAdd);
			}
		}
		public void Remove(AvailablePluginInfo pluginToRemove)
		{
			base.List.Remove(pluginToRemove);
		}
		public AvailablePluginInfo Find(string pluginNameOrPath)
		{
			AvailablePluginInfo result = null;
			try
			{
				foreach (AvailablePluginInfo availablePluginInfo in base.List)
				{
					if (availablePluginInfo.Instance != null && (availablePluginInfo.Instance.Name.Equals(pluginNameOrPath) || availablePluginInfo.AssemblyPath.Equals(pluginNameOrPath)))
					{
						result = availablePluginInfo;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "查找插件异常" + ex.Message);
			}
			return result;
		}
	}
}
