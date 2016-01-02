// Decompiled with JetBrains decompiler
// Type: SetServerInfoPlugin.ServerSetPlugin
// Assembly: SetServerInfoPlugin, Version=3.0.8.0, Culture=neutral, PublicKeyToken=null
// MVID: E04F003E-2DD5-4E4F-8F62-E41AF4AB517D
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\SetServerInfoPlugin.dll

using PluginInterface;
using System;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;

namespace SetServerInfoPlugin
{
  public class ServerSetPlugin : IPlugin
  {
    public static bool displayIP = false;
    public static string strServerNameFir = string.Empty;
    public static string strServerNameSec = string.Empty;
    public static ResourceManager SysResourceManager = (ResourceManager) null;
    private int myPluginNO = 1;
    private string myName = "ServerSet";
    private string myConfigFileName = "ServerSet.xml";
    private string myDescription = "服务器设置插件";
    private string myAuthor = " ";
    private string myText = string.Empty;
    private string mysettingconfig = string.Empty;
    private bool myIsEnable = true;
    private DisplayTypes myDisplayType = (DisplayTypes) 2;
    private string myIpAddress = string.Empty;
    private string myCommunicationUrl = string.Empty;
    private bool isServerSetE = true;
    private Hashtable myhashconfig;
    private bool myIsNeedLoad;
    private Form myPreLoginForm;
    private Form myLoginedForm;
    private int myPort;
    private IPluginHost myHost;

    public string Description
    {
      get
      {
        return this.myDescription;
      }
    }

    public string Author
    {
      get
      {
        return this.myAuthor;
      }
    }

    public IPluginHost Host
    {
      get
      {
        return this.myHost;
      }
      set
      {
        this.myHost = value;
      }
    }

    public int PluginNO
    {
      get
      {
        return this.myPluginNO;
      }
    }

    public string Name
    {
      get
      {
        return this.myName;
      }
    }

    public string ConfigFileName
    {
      get
      {
        return this.myConfigFileName;
      }
      set
      {
        this.myConfigFileName = value;
      }
    }

    public string Version
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
      }
    }

    public string Text
    {
      get
      {
        return this.myText;
      }
    }

    public bool IsEnable
    {
      get
      {
        return this.myIsEnable;
      }
    }

    public bool IsNeedLoad
    {
      get
      {
        return this.myIsNeedLoad;
      }
    }

    public DisplayTypes DisplayType
    {
      get
      {
        return this.myDisplayType;
      }
    }

    public string IpAddress
    {
      get
      {
        return this.myIpAddress;
      }
    }

    public int Port
    {
      get
      {
        return this.myPort;
      }
    }

    public string CommunicationUrl
    {
      get
      {
        return this.myCommunicationUrl;
      }
    }

    public Hashtable HashConfigSettings
    {
      get
      {
        return this.myhashconfig;
      }
      set
      {
        this.myhashconfig = value;
      }
    }

    public string SettingConfigName
    {
      get
      {
        return this.mysettingconfig;
      }
      set
      {
        this.mysettingconfig = value;
      }
    }

    public void Initialize()
    {
      try
      {
        if (this.myHost != null)
          ServerSetPlugin.SysResourceManager = this.myHost.MEBS_ResourceManager;
        XmlDocument xmlDocument = (XmlDocument) null;
        if (this.myHost != null && this.myHost.HtConfigInfo != null)
          xmlDocument = (XmlDocument) ((PluginConfigInfo) this.myHost.HtConfigInfo[(object) this.myName]).XmlDoc;
        if (xmlDocument == null)
        {
          this.myIsEnable = false;
        }
        else
        {
          Hashtable section = this.myHost.ConfigurationInfo.getSection("Systems");
          ServerSetPlugin.displayIP = Tools.StrToBool((string) section[(object) "DisplayIP"], true);
          this.isServerSetE = Tools.StrToBool((string) section[(object) "ServerSet"], true);
          string s = string.Empty;
          string str = string.Empty;
          XmlElement xmlElement = (XmlElement) xmlDocument.SelectSingleNode("ConfigInfo");
          if (xmlElement.SelectSingleNode("Enable") != null)
            s = xmlElement.SelectSingleNode("Enable").InnerText.Trim();
          if (xmlElement.SelectSingleNode("Text") != null)
            str = xmlElement.SelectSingleNode("Text").InnerText.Trim();
          if (xmlElement.SelectSingleNode("DisplayTypes") != null)
            this.myDisplayType = (DisplayTypes) int.Parse(xmlElement.SelectSingleNode("DisplayTypes").InnerText.Trim());
          if (xmlElement.SelectSingleNode("ServerNameFir") != null)
            ServerSetPlugin.strServerNameFir = xmlElement.SelectSingleNode("ServerNameFir").InnerText;
          if (xmlElement.SelectSingleNode("ServerNameSec") != null)
            ServerSetPlugin.strServerNameSec = xmlElement.SelectSingleNode("ServerNameSec").InnerText;
          this.myIsEnable = Tools.StrToBool(s);
          this.myText = str;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(ex);
      }
    }

    public bool Logon(ref string info)
    {
      string @string = ServerSetPlugin.SysResourceManager.GetString("PluginStr_LoginSuccess");
      info = @string;
      return true;
    }

    public bool chgPWD(string newPWD, string oldPWD, ref string info)
    {
      string @string = ServerSetPlugin.SysResourceManager.GetString("PluginStr_NoChangePWD");
      info = @string;
      return false;
    }

    public Form GetForm(bool isLoad, ref string info)
    {
      if (!isLoad)
      {
        if (this.myPreLoginForm == null || this.myPreLoginForm.IsDisposed)
          this.myPreLoginForm = !this.isServerSetE ? (Form) new ServerSet(this.myHost) : (Form) new ServerSetE(this.myHost);
        return this.myPreLoginForm;
      }
      if (this.myPreLoginForm == null || this.myPreLoginForm.IsDisposed)
        this.myPreLoginForm = !this.isServerSetE ? (Form) new ServerSet(this.myHost) : (Form) new ServerSetE(this.myHost);
      this.myLoginedForm = this.myPreLoginForm;
      return this.myLoginedForm;
    }

    public void AcceptInfo(PluginCommunicateInfo pluginCommunicateInfo, IPlugin Plugin)
    {
    }

    public void Dispose()
    {
      if (this.myPreLoginForm != null && !this.myPreLoginForm.IsDisposed)
        this.myPreLoginForm.Close();
      if (this.myLoginedForm == null || this.myLoginedForm.IsDisposed)
        return;
      this.myLoginedForm.Close();
    }

    public bool AgencyLogon(ref string info)
    {
      return true;
    }

    public void CloseForm()
    {
    }

    public void SetAgencyLogoutEvent(EventAgencyLogOut _AgencyLogoutEvent)
    {
    }

    public void SetLockTree(EventLockTree _LogoutEvent)
    {
    }

    public void SetLogoutEvent(EventLogOut _LogoutEvent)
    {
    }

    public void SetMessageEvent(EventHandler _messageEvent)
    {
    }

    public void SetPlayMessage(EventPlayMessage _PlayMessage)
    {
    }

    public void SetProgressEvent(EventInitData _initDataMainForm)
    {
    }

    public void SetReLoad(EventReLoad _ReLoad)
    {
    }

    public void SetUnLock(bool _UnLock)
    {
    }

    public bool chgPWD(ChgPWD chgpwd, string newPWD, string oldPWD, ref string info)
    {
      return true;
    }
  }
}
