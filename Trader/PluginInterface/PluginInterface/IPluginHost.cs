using System;
using System.Collections;
using System.Drawing;
using System.Resources;
namespace PluginInterface
{
	public interface IPluginHost
	{
		string IdentityType
		{
			get;
			set;
		}
		string TradeUrl
		{
			get;
			set;
		}
		string TradeSafeUrl
		{
			get;
			set;
		}
		Hashtable HtConfigInfo
		{
			get;
		}
		LogonInfo SysLogonInfo
		{
			get;
			set;
		}
		Configuration ConfigurationInfo
		{
			get;
		}
		ResourceManager MEBS_ResourceManager
		{
			get;
		}
		ResourceManager ISSUE_ResourceManager
		{
			get;
		}
		ResourceManager OTC_ResourceManager
		{
			get;
		}
		Image SystemImage
		{
			get;
		}
		Icon SystemIcon
		{
			get;
		}
		string SystemTitle
		{
			get;
		}
		string SystemVersion
		{
			get;
		}
		void Feedback(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin);
	}
}
