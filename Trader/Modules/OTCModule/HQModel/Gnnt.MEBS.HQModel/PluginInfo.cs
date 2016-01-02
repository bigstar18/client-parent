using System;
using System.Collections;
using System.Resources;
namespace Gnnt.MEBS.HQModel
{
	public class PluginInfo
	{
		public string ConfigPath;
		public ResourceManager HQResourceManager;
		public string IPAddress = string.Empty;
		public int Port;
		public int HttpPort;
		public Hashtable HTConfig = new Hashtable();
	}
}
