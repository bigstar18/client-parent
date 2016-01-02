using System;
using System.Threading;
using TPME.Log;
namespace FuturesTrade.Gnnt.Library
{
	public class IniData
	{
		private static volatile IniData instance;
		public bool ShowDialog;
		public bool FailShowDialog;
		public bool SuccessShowDialog;
		public bool AutoRefresh;
		public bool SetDoubleClick;
		public bool ClearData;
		public string Language;
		private int closeMode;
		public bool LockEnable;
		public int LockTime;
		public string Plugin;
		public bool UpDownFocus;
		public bool LimitFocus;
		public bool KeyEnterOrder;
		public bool HideHQ;
		public bool AutoPrice;
		public int CloseMode
		{
			get
			{
				return this.closeMode;
			}
			set
			{
				IniFile iniFile = new IniFile(Global.ConfigPath + "MEBS_Trade.ini");
				iniFile.IniWriteValue("Set", "CloseMode", string.Concat(value));
				this.closeMode = value;
			}
		}
		public static IniData GetInstance()
		{
			if (IniData.instance == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(IniData));
				try
				{
					if (IniData.instance == null)
					{
						try
						{
							IniData.instance = new IniData();
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return IniData.instance;
		}
		private IniData()
		{
			IniFile iniFile = new IniFile(Global.ConfigPath + "MEBS_Trade.ini");
			try
			{
				this.LockEnable = bool.Parse(iniFile.IniReadValue("LockSet", "LockEnable"));
				this.LockTime = int.Parse(iniFile.IniReadValue("LockSet", "LockTime"));
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "配置文件中程序锁定项数据出错！" + ex.ToString());
				this.LockEnable = true;
				this.LockTime = 12;
			}
			try
			{
				this.ShowDialog = bool.Parse(iniFile.IniReadValue("Set", "ShowDialog"));
			}
			catch (Exception ex2)
			{
				Logger.wirte(MsgType.Error, "配置文件中是否显示确认对话框数据出错！" + ex2.ToString());
				this.ShowDialog = true;
			}
			try
			{
				Console.WriteLine(iniFile.IniReadValue("Set", "FailShowDialog"));
				this.FailShowDialog = bool.Parse(iniFile.IniReadValue("Set", "FailShowDialog"));
			}
			catch (Exception ex3)
			{
				Logger.wirte(MsgType.Error, "配置文件中下单失败后是否使用对话框弹出错误信息出错！" + ex3.ToString());
				this.FailShowDialog = true;
			}
			try
			{
				this.SuccessShowDialog = bool.Parse(iniFile.IniReadValue("Set", "SuccessShowDialog"));
			}
			catch (Exception ex4)
			{
				Logger.wirte(MsgType.Error, "配置文件中下单成功后是否使用对话框弹出提示信息出错！" + ex4.ToString());
				this.SuccessShowDialog = false;
			}
			try
			{
				this.AutoRefresh = bool.Parse(iniFile.IniReadValue("Set", "AutoRefresh"));
			}
			catch (Exception ex5)
			{
				Logger.wirte(MsgType.Error, "配置文件中是否自动刷新交易数据出错！" + ex5.ToString());
				this.AutoRefresh = true;
			}
			try
			{
				this.AutoPrice = bool.Parse(iniFile.IniReadValue("Set", "AutoPrice"));
			}
			catch (Exception ex6)
			{
				Logger.wirte(MsgType.Error, "配置文件中是否自动填入买/卖价格及数量数据出错！" + ex6.ToString());
				this.AutoPrice = true;
			}
			try
			{
				this.closeMode = int.Parse(iniFile.IniReadValue("Set", "CloseMode"));
			}
			catch (Exception ex7)
			{
				Logger.wirte(MsgType.Error, "配置文件中平仓方式项数据出错！" + ex7.ToString());
				this.closeMode = 1;
			}
			try
			{
				this.SetDoubleClick = bool.Parse(iniFile.IniReadValue("Set", "SetDoubleClick"));
			}
			catch (Exception ex8)
			{
				Logger.wirte(MsgType.Error, "配置文件中是否设置双击切换固定价和跟随价数据出错！" + ex8.ToString());
				this.SetDoubleClick = false;
			}
			try
			{
				this.ClearData = bool.Parse(iniFile.IniReadValue("Set", "ClearData"));
			}
			catch (Exception ex9)
			{
				Logger.wirte(MsgType.Error, "配置文件中下单成功后是否清空价格和数量出错！" + ex9.ToString());
				this.ClearData = false;
			}
			try
			{
				this.Language = iniFile.IniReadValue("Set", "Language");
			}
			catch (Exception ex10)
			{
				Logger.wirte(MsgType.Error, "配置文件中语言类型出错！" + ex10.ToString());
				this.Language = "0";
			}
			try
			{
				this.UpDownFocus = bool.Parse(iniFile.IniReadValue("Set", "UpDownFocus"));
			}
			catch (Exception)
			{
				this.UpDownFocus = true;
			}
			try
			{
				this.LimitFocus = bool.Parse(iniFile.IniReadValue("Set", "LimitFocus"));
			}
			catch (Exception)
			{
				this.LimitFocus = false;
			}
			try
			{
				this.KeyEnterOrder = bool.Parse(iniFile.IniReadValue("Set", "KeyEnterOrder"));
			}
			catch (Exception)
			{
				this.KeyEnterOrder = false;
			}
			try
			{
				this.HideHQ = bool.Parse(iniFile.IniReadValue("Set", "HideHQ"));
			}
			catch (Exception)
			{
				this.HideHQ = false;
			}
			try
			{
				this.AutoPrice = bool.Parse(iniFile.IniReadValue("Set", "AutoPrice"));
			}
			catch (Exception)
			{
				this.AutoPrice = true;
			}
		}
	}
}
