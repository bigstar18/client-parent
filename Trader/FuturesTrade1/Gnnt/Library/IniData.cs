namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Runtime.CompilerServices;
    using TPME.Log;

    public class IniData
    {
        public bool AutoPrice;
        public bool AutoRefresh;
        public bool ClearData;
        private int closeMode;
        public bool FailShowDialog;
        public bool HideHQ;
        private static volatile IniData instance;
        public bool KeyEnterOrder;
        public string Language;
        public bool LimitFocus;
        public bool LockEnable;
        public int LockTime;
        public string Plugin;
        public bool SetDoubleClick;
        public bool ShowDialog;
        public bool SuccessShowDialog;
        public bool UpDownFocus;

        private IniData()
        {
            IniFile file = new IniFile(Global.ConfigPath + "MEBS_Trade.ini");
            try
            {
                this.LockEnable = bool.Parse(file.IniReadValue("LockSet", "LockEnable"));
                this.LockTime = int.Parse(file.IniReadValue("LockSet", "LockTime"));
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "配置文件中程序锁定项数据出错！" + exception.ToString());
                this.LockEnable = true;
                this.LockTime = 12;
            }
            try
            {
                this.ShowDialog = bool.Parse(file.IniReadValue("Set", "ShowDialog"));
            }
            catch (Exception exception2)
            {
                Logger.wirte(MsgType.Error, "配置文件中是否显示确认对话框数据出错！" + exception2.ToString());
                this.ShowDialog = true;
            }
            try
            {
                Console.WriteLine(file.IniReadValue("Set", "FailShowDialog"));
                this.FailShowDialog = bool.Parse(file.IniReadValue("Set", "FailShowDialog"));
            }
            catch (Exception exception3)
            {
                Logger.wirte(MsgType.Error, "配置文件中下单失败后是否使用对话框弹出错误信息出错！" + exception3.ToString());
                this.FailShowDialog = true;
            }
            try
            {
                this.SuccessShowDialog = bool.Parse(file.IniReadValue("Set", "SuccessShowDialog"));
            }
            catch (Exception exception4)
            {
                Logger.wirte(MsgType.Error, "配置文件中下单成功后是否使用对话框弹出提示信息出错！" + exception4.ToString());
                this.SuccessShowDialog = false;
            }
            try
            {
                this.AutoRefresh = bool.Parse(file.IniReadValue("Set", "AutoRefresh"));
            }
            catch (Exception exception5)
            {
                Logger.wirte(MsgType.Error, "配置文件中是否自动刷新交易数据出错！" + exception5.ToString());
                this.AutoRefresh = true;
            }
            try
            {
                this.AutoPrice = bool.Parse(file.IniReadValue("Set", "AutoPrice"));
            }
            catch (Exception exception6)
            {
                Logger.wirte(MsgType.Error, "配置文件中是否自动填入买/卖价格及数量数据出错！" + exception6.ToString());
                this.AutoPrice = true;
            }
            try
            {
                this.closeMode = int.Parse(file.IniReadValue("Set", "CloseMode"));
            }
            catch (Exception exception7)
            {
                Logger.wirte(MsgType.Error, "配置文件中平仓方式项数据出错！" + exception7.ToString());
                this.closeMode = 1;
            }
            try
            {
                this.SetDoubleClick = bool.Parse(file.IniReadValue("Set", "SetDoubleClick"));
            }
            catch (Exception exception8)
            {
                Logger.wirte(MsgType.Error, "配置文件中是否设置双击切换固定价和跟随价数据出错！" + exception8.ToString());
                this.SetDoubleClick = false;
            }
            try
            {
                this.ClearData = bool.Parse(file.IniReadValue("Set", "ClearData"));
            }
            catch (Exception exception9)
            {
                Logger.wirte(MsgType.Error, "配置文件中下单成功后是否清空价格和数量出错！" + exception9.ToString());
                this.ClearData = false;
            }
            try
            {
                this.Language = file.IniReadValue("Set", "Language");
            }
            catch (Exception exception10)
            {
                Logger.wirte(MsgType.Error, "配置文件中语言类型出错！" + exception10.ToString());
                this.Language = "0";
            }
            try
            {
                this.UpDownFocus = bool.Parse(file.IniReadValue("Set", "UpDownFocus"));
            }
            catch (Exception)
            {
                this.UpDownFocus = true;
            }
            try
            {
                this.LimitFocus = bool.Parse(file.IniReadValue("Set", "LimitFocus"));
            }
            catch (Exception)
            {
                this.LimitFocus = false;
            }
            try
            {
                this.KeyEnterOrder = bool.Parse(file.IniReadValue("Set", "KeyEnterOrder"));
            }
            catch (Exception)
            {
                this.KeyEnterOrder = false;
            }
            try
            {
                this.HideHQ = bool.Parse(file.IniReadValue("Set", "HideHQ"));
            }
            catch (Exception)
            {
                this.HideHQ = false;
            }
            try
            {
                this.AutoPrice = bool.Parse(file.IniReadValue("Set", "AutoPrice"));
            }
            catch (Exception)
            {
                this.AutoPrice = true;
            }
        }

        public static IniData GetInstance()
        {
            if (instance == null)
            {
                lock (typeof(IniData))
                {
                    if (instance == null)
                    {
                        try
                        {
                            instance = new IniData();
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }
                    }
                }
            }
            return instance;
        }

        public int CloseMode
        {
            get
            {
                return this.closeMode;
            }
            set
            {
                new IniFile(Global.ConfigPath + "MEBS_Trade.ini").IniWriteValue("Set", "CloseMode", string.Concat(value));
                this.closeMode = value;
            }
        }
    }
}
