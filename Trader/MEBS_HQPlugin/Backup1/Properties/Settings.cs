// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.Properties.Settings
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Gnnt.MEBS.HQClient.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        return Settings.defaultInstance;
      }
    }

    [DefaultSettingValue("0")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public int iStyle
    {
      get
      {
        return (int) this["iStyle"];
      }
      set
      {
        this["iStyle"] = (object) value;
      }
    }

    [DefaultSettingValue("Market")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string CurButtonName
    {
      get
      {
        return (string) this["CurButtonName"];
      }
      set
      {
        this["CurButtonName"] = (object) value;
      }
    }

    [UserScopedSetting]
    [DefaultSettingValue("3")]
    [DebuggerNonUserCode]
    public int MultiQuoteStaticIndex
    {
      get
      {
        return (int) this["MultiQuoteStaticIndex"];
      }
      set
      {
        this["MultiQuoteStaticIndex"] = (object) value;
      }
    }

    [DefaultSettingValue("Chinese")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string Language
    {
      get
      {
        return (string) this["Language"];
      }
      set
      {
        this["Language"] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public int MarketIndex
    {
      get
      {
        return (int) this["MarketIndex"];
      }
      set
      {
        this["MarketIndex"] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public int ServerIndex
    {
      get
      {
        return (int) this["ServerIndex"];
      }
      set
      {
        this["ServerIndex"] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int ShowBuySellPrice
    {
      get
      {
        return (int) this["ShowBuySellPrice"];
      }
      set
      {
        this["ShowBuySellPrice"] = (object) value;
      }
    }

    [DefaultSettingValue("Name:;Code:;CurPrice:;CurAmount:;SellPrice:;SellAmount:;BuyPrice:;BuyAmount:;TotalAmount:;UpValue:;UpRate:;ReverseCount:;Balance:;OpenPrice:;HighPrice:;LowPrice:;YesterBalance:;TotalMoney:;AmountRate:;ConsignRate:;Unit:;Industry:;Region:;")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string MultiQuoteName
    {
      get
      {
        return (string) this["MultiQuoteName"];
      }
      set
      {
        this["MultiQuoteName"] = (object) value;
      }
    }

    [DefaultSettingValue("No;Code;Name;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;Industry;Region;")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string MultiQuoteItems
    {
      get
      {
        return (string) this["MultiQuoteItems"];
      }
      set
      {
        this["MultiQuoteItems"] = (object) value;
      }
    }

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }
  }
}
