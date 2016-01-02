using System;
using ToolsLibrary.util;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
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
		public int FloatAlertWindowStayTime;
		private int closeMode;
		public bool LockEnable;
		public int LockTime;
		public string Plugin;
		public int CloseMode
		{
			get
			{
				return this.closeMode;
			}
			set
			{
				try
				{
					IniFile iniFile = new IniFile(Global.ConfigPath + Global.UserID + "Trade.ini");
					iniFile.IniWriteValue("Set", "CloseMode", string.Concat(value));
					this.closeMode = value;
				}
				catch (Exception ex)
				{
					Logger.wirte(3, "Trade.ini写入失败" + ex.ToString());
				}
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
		public static void DisposeInstance()
		{
			if (IniData.instance != null)
			{
				lock (typeof(IniData))
				{
					IniData.instance = null;
				}
			}
		}
		private IniData()
		{
			IniFile iniFile = new IniFile(Global.ConfigPath + Global.UserID + "Trade.ini");
			try
			{
				this.LockEnable = bool.Parse(iniFile.IniReadValue("LockSet", "LockEnable"));
				this.LockTime = int.Parse(iniFile.IniReadValue("LockSet", "LockTime"));
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "配置文件中程序锁定项数据出错！" + ex.ToString());
				this.LockEnable = true;
				this.LockTime = 12;
			}
			try
			{
				this.ShowDialog = bool.Parse(iniFile.IniReadValue("Set", "ShowDialog"));
			}
			catch (Exception ex2)
			{
				Logger.wirte(3, "配置文件中是否显示确认对话框数据出错！" + ex2.ToString());
				this.ShowDialog = true;
			}
			try
			{
				this.FailShowDialog = bool.Parse(iniFile.IniReadValue("Set", "FailShowDialog"));
			}
			catch (Exception ex3)
			{
				Logger.wirte(3, "配置文件中下单失败后是否使用对话框弹出错误信息出错！" + ex3.ToString());
				this.FailShowDialog = true;
			}
			try
			{
				this.SuccessShowDialog = bool.Parse(iniFile.IniReadValue("Set", "SuccessShowDialog"));
			}
			catch (Exception ex4)
			{
				Logger.wirte(3, "配置文件中下单成功后是否使用对话框弹出提示信息出错！" + ex4.ToString());
				this.SuccessShowDialog = true;
			}
			try
			{
				this.AutoRefresh = bool.Parse(iniFile.IniReadValue("Set", "AutoRefresh"));
			}
			catch (Exception)
			{
				this.AutoRefresh = true;
			}
			try
			{
				this.AutoAddBSPQ = bool.Parse(iniFile.IniReadValue("Set", "AutoAddBSPQ"));
			}
			catch (Exception)
			{
				this.AutoAddBSPQ = false;
			}
			try
			{
				this.closeMode = int.Parse(iniFile.IniReadValue("Set", "CloseMode"));
			}
			catch (Exception)
			{
				this.closeMode = 1;
			}
			try
			{
				this.SetDoubleClick = bool.Parse(iniFile.IniReadValue("Set", "SetDoubleClick"));
			}
			catch (Exception)
			{
				this.SetDoubleClick = false;
			}
			try
			{
				this.FloatAlertWindowStayTime = int.Parse(iniFile.IniReadValue("Set", "FloatAlertWindowStayTime"));
			}
			catch (Exception ex5)
			{
				Logger.wirte(3, "配置文件中浮动提醒窗口停留时间数据出错！" + ex5.ToString());
				this.FloatAlertWindowStayTime = 5;
			}
		}
	}
}
