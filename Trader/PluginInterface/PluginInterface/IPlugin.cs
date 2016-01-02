using System;
using System.Collections;
using System.Windows.Forms;
namespace PluginInterface
{
	public interface IPlugin
	{
		IPluginHost Host
		{
			get;
			set;
		}
		int PluginNO
		{
			get;
		}
		string Name
		{
			get;
		}
		string ConfigFileName
		{
			get;
			set;
		}
		string SettingConfigName
		{
			get;
			set;
		}
		Hashtable HashConfigSettings
		{
			get;
			set;
		}
		string Description
		{
			get;
		}
		string Author
		{
			get;
		}
		string Version
		{
			get;
		}
		string Text
		{
			get;
		}
		bool IsEnable
		{
			get;
		}
		bool IsNeedLoad
		{
			get;
		}
		DisplayTypes DisplayType
		{
			get;
		}
		string IpAddress
		{
			get;
		}
		int Port
		{
			get;
		}
		string CommunicationUrl
		{
			get;
		}
		void Initialize();
		bool Logon(ref string info);
		bool AgencyLogon(ref string info);
		bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info);
		void Dispose();
		Form GetForm(bool isLoad, ref string info);
		void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin);
		void CloseForm();
		void SetProgressEvent(EventInitData _initDataMainForm);
		void SetMessageEvent(EventHandler _messageEvent);
		void SetLogoutEvent(EventLogOut _LogoutEvent);
		void SetAgencyLogoutEvent(EventAgencyLogOut _AgencyLogoutEvent);
		void SetLockTree(EventLockTree _LogoutEvent);
		void SetReLoad(EventReLoad _ReLoad);
		void SetPlayMessage(EventPlayMessage _PlayMessage);
		void SetUnLock(bool _UnLock);
	}
}
