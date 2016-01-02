using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace Gnnt.MEBS.HQModel.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int iStyle
		{
			get
			{
				return (int)this["iStyle"];
			}
			set
			{
				this["iStyle"] = value;
			}
		}
		[DefaultSettingValue("Market"), UserScopedSetting, DebuggerNonUserCode]
		public string CurButtonName
		{
			get
			{
				return (string)this["CurButtonName"];
			}
			set
			{
				this["CurButtonName"] = value;
			}
		}
		[DefaultSettingValue("3"), UserScopedSetting, DebuggerNonUserCode]
		public int MultiQuoteStaticIndex
		{
			get
			{
				return (int)this["MultiQuoteStaticIndex"];
			}
			set
			{
				this["MultiQuoteStaticIndex"] = value;
			}
		}
		[DefaultSettingValue("Chinese"), UserScopedSetting, DebuggerNonUserCode]
		public string Language
		{
			get
			{
				return (string)this["Language"];
			}
			set
			{
				this["Language"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int MarketIndex
		{
			get
			{
				return (int)this["MarketIndex"];
			}
			set
			{
				this["MarketIndex"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int ServerIndex
		{
			get
			{
				return (int)this["ServerIndex"];
			}
			set
			{
				this["ServerIndex"] = value;
			}
		}
		[DefaultSettingValue("Name:;MarketName:;Code:;CurPrice:;CurAmount:;SellPrice:;SellAmount:;BuyPrice:;BuyAmount:;TotalAmount:;UpValue:;UpRate:;ReverseCount:;Balance:;OpenPrice:;HighPrice:;LowPrice:;YesterBalance:;TotalMoney:;AmountRate:;ConsignRate:;Unit:;Industry:;Region:;"), UserScopedSetting, DebuggerNonUserCode]
		public string MultiQuoteName
		{
			get
			{
				return (string)this["MultiQuoteName"];
			}
			set
			{
				this["MultiQuoteName"] = value;
			}
		}
		[DefaultSettingValue("No;Code;MarketName;Name;CurPrice;CurAmount;SellPrice;SellAmount;BuyPrice;BuyAmount;TotalAmount;UpValue;ReverseCount;Balance;OpenPrice;HighPrice;LowPrice;YesterBalance;Industry;Region;"), UserScopedSetting, DebuggerNonUserCode]
		public string MultiQuoteItems
		{
			get
			{
				return (string)this["MultiQuoteItems"];
			}
			set
			{
				this["MultiQuoteItems"] = value;
			}
		}
		[DefaultSettingValue("5"), UserScopedSetting, DebuggerNonUserCode]
		public int ShowBuySellPrice
		{
			get
			{
				return (int)this["ShowBuySellPrice"];
			}
			set
			{
				this["ShowBuySellPrice"] = value;
			}
		}
	}
}
