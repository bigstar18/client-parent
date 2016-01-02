using System;
using TPME.Log;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class IniData
	{
		private static volatile IniData instance;
		public bool ShowDialog;
		public bool FailShowDialog;
		public bool SuccessShowDialog;
		public bool AutoRefresh;
		public bool AutoAddBSPQ;
		public bool SetDoubleClick;
		private int closeMode;
		public bool LockEnable;
		public int LockTime;
		public string Plugin;
		private IniFile ini;
		public int CloseMode
		{
			get
			{
				return this.closeMode;
			}
			set
			{
				this.ini.IniWriteValue("Set", "CloseMode", string.Concat(value));
				this.closeMode = value;
			}
		}
		public static IniData GetInstance()
		{
			if (IniData.instance == null)
			{
				lock (typeof(IniData))
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
			}
			return IniData.instance;
		}
		private IniData()
		{
			this.ini = new IniFile(Global.ConfigPath + "ISSUE_Trade.ini");
			try
			{
				this.LockEnable = bool.Parse(this.ini.IniReadValue("LockSet", "LockEnable"));
				this.LockTime = int.Parse(this.ini.IniReadValue("LockSet", "LockTime"));
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "配置文件中程序锁定项数据出错！" + ex.ToString());
				this.LockEnable = true;
				this.LockTime = 12;
			}
			try
			{
				this.ShowDialog = bool.Parse(this.ini.IniReadValue("Set", "ShowDialog"));
			}
			catch (Exception ex2)
			{
				Logger.wirte(3, "配置文件中是否显示确认对话框数据出错！" + ex2.ToString());
				this.ShowDialog = true;
			}
			try
			{
				Console.WriteLine(this.ini.IniReadValue("Set", "FailShowDialog"));
				this.FailShowDialog = bool.Parse(this.ini.IniReadValue("Set", "FailShowDialog"));
			}
			catch (Exception ex3)
			{
				Logger.wirte(3, "配置文件中下单失败后是否使用对话框弹出错误信息出错！" + ex3.ToString());
				this.FailShowDialog = true;
			}
			try
			{
				this.SuccessShowDialog = bool.Parse(this.ini.IniReadValue("Set", "SuccessShowDialog"));
			}
			catch (Exception ex4)
			{
				Logger.wirte(3, "配置文件中下单成功后是否使用对话框弹出提示信息出错！" + ex4.ToString());
				this.SuccessShowDialog = true;
			}
			try
			{
				this.AutoRefresh = bool.Parse(this.ini.IniReadValue("Set", "AutoRefresh"));
			}
			catch (Exception ex5)
			{
				Logger.wirte(3, "配置文件中是否自动刷新交易数据出错！" + ex5.ToString());
				this.AutoRefresh = true;
			}
			try
			{
				this.AutoAddBSPQ = bool.Parse(this.ini.IniReadValue("Set", "AutoAddBSPQ"));
			}
			catch (Exception ex6)
			{
				Logger.wirte(3, "配置文件中是否自动填入买/卖价格及数量数据出错！" + ex6.ToString());
				this.AutoAddBSPQ = false;
			}
			try
			{
				this.closeMode = int.Parse(this.ini.IniReadValue("Set", "CloseMode"));
			}
			catch (Exception ex7)
			{
				Logger.wirte(3, "配置文件中平仓方式项数据出错！" + ex7.ToString());
				this.closeMode = 1;
			}
			try
			{
				this.SetDoubleClick = bool.Parse(this.ini.IniReadValue("Set", "SetDoubleClick"));
			}
			catch (Exception ex8)
			{
				Logger.wirte(3, "配置文件中是否设置双击切换固定价和跟随价数据出错！" + ex8.ToString());
				this.SetDoubleClick = false;
			}
		}
	}
}
