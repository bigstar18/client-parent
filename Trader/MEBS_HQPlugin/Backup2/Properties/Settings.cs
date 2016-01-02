// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.Properties.Settings
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Gnnt.MEBS.HQModel.Properties
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

    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    [UserScopedSetting]
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
    [DebuggerNonUserCode]
    [UserScopedSetting]
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

    [DefaultSettingValue("3")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
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
    [DebuggerNonUserCode]
    [UserScopedSetting]
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
    [UserScopedSetting]
    [DebuggerNonUserCode]
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

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
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
    [DefaultSettingValue("Name:;MarketName:;Code:;CurPrice:;CurAmount:;SellPrice:;SellAmount:;BuyPrice:;BuyAmount:;TotalAmount:;UpValue:;UpRate:;ReverseCount:;Balance:;OpenPrice:;HighPrice:;LowPrice:;YesterBalance:;TotalMoney:;AmountRate:;ConsignRate:;Unit:;Industry:;Region:;")]
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

    [DefaultSettingValue("No;Code;MarketName;Name;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;Industry;Region;")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
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

    [UserScopedSetting]
    [DefaultSettingValue("5")]
    [DebuggerNonUserCode]
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
  }
}
