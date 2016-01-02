using Gnnt.MixAccountPlugin;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.Manager;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame.Gnnt.Common.Operation
{
	public class VerifyCodeOperation
	{
		public delegate void SetFocusCallBack(short flag);
		public delegate void EnableControlsCallBack(bool flag, string info);
		public VerifyCodeOperation.SetFocusCallBack SetFocus;
		public VerifyCodeOperation.EnableControlsCallBack EnableControls;
		public LogonOperationInfo verifyCodeInfo;
		private int passwordType;
		private int codelength = 4;
		public int Codelength
		{
			get
			{
				return this.codelength;
			}
			set
			{
				this.codelength = value;
			}
		}
		public void updatePic()
		{
			this.passwordType = Tools.StrToInt((string)Global.htConfig["PassWordType"], 0);
			RandomIdentifyCode randomIdentifyCode = new RandomIdentifyCode(this.passwordType, this.Codelength);
			this.verifyCodeInfo = new LogonOperationInfo();
			this.verifyCodeInfo.verifyCodeImage = randomIdentifyCode.CreateIdentifyCode();
			this.verifyCodeInfo.verifyCodeString = randomIdentifyCode.RandomCode;
		}
		public void VerifyUserInfo(LogonOperationInfo verifyUserInfo, IntPtr handle)
		{
			string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_ErrorInfo");
			if (verifyUserInfo.username.Trim().Equals(""))
			{
				string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_inputId");
				MessageBox.Show(string2, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.SetFocus(0);
				return;
			}
			if (verifyUserInfo.password.Trim().Equals(""))
			{
				string string3 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_inputPassword");
				MessageBox.Show(string3, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.SetFocus(1);
				return;
			}
			if (verifyUserInfo.verifycode.Trim().Equals(""))
			{
				string string4 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_inputVerifyCode");
				MessageBox.Show(string4, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.SetFocus(2);
				return;
			}
			if (!verifyUserInfo.verifyCodeString.ToLowerInvariant().Equals(verifyUserInfo.verifycode.Trim().ToLowerInvariant()))
			{
				string string5 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_VerifyCodeError");
				MessageBox.Show(string5, @string, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.SetFocus(3);
				return;
			}
			bool flag = WinSendMessage.SendMessageByProcessName("SysFrame", verifyUserInfo.username, handle);
			if (flag)
			{
				string string6 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_Tip");
				string string7 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_RepeatLogin");
				MessageBox.Show(string.Format(string7, verifyUserInfo.username), string6, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.SetFocus(0);
				return;
			}
			RecordUserName recordUserName = new RecordUserName();
			recordUserName.recordUsername(new ReCordUserInfo
			{
				isMemoryChecked = verifyUserInfo.isMemoryChecked,
				isProtectChecked = verifyUserInfo.isProtectChecked,
				username = verifyUserInfo.username
			});
			Logger.wirte(1, "启动登陆线程");
			this.EnableControls(false, "null");
			UserInfo userInfo = new UserInfo();
			userInfo.set_UserID(verifyUserInfo.username);
			userInfo.set_PassWord(verifyUserInfo.password);
			userInfo.set_ModuleID("");
			WaitCallback callBack = new WaitCallback(OperationManager.GetInstance().loginOperation.Connect);
			ThreadPool.QueueUserWorkItem(callBack, verifyUserInfo);
		}
		private static bool IsNumeric(string str)
		{
			if (str == null || str.Length == 0)
			{
				return false;
			}
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			byte[] array = bytes;
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				if (b < 48 || b > 57)
				{
					return false;
				}
			}
			return true;
		}
		private static bool IsAlphabet(string str)
		{
			if (str == null || str.Length == 0)
			{
				return false;
			}
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			byte[] array = bytes;
			int i = 0;
			while (i < array.Length)
			{
				byte b = array[i];
				bool result;
				if (b < 65 || b > 122)
				{
					result = false;
				}
				else
				{
					if (b <= 90 || b >= 97)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}
		public void VerifyPassWord(string password)
		{
			if (password.Length < 8 && this.passwordScore(password) <= 1)
			{
				MessageBox.Show("您的交易密码过于简单，为了您的交易安全，请及时修改！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		private int passwordScore(string password)
		{
			int num = 0;
			Regex regex = new Regex("[a-z]");
			Regex regex2 = new Regex("[A-Z]");
			Regex regex3 = new Regex("\\d+");
			Regex regex4 = new Regex("(\\d\\D*\\d\\D*\\d)");
			Regex regex5 = new Regex("[!,@#$%^&*?_~]");
			Regex regex6 = new Regex("([!,@#$%^&*?_~].*[!,@#$%^&*?_~])");
			Regex regex7 = new Regex("\\d");
			Regex regex8 = new Regex("\\D");
			if (regex.IsMatch(password))
			{
				num++;
			}
			if (regex2.IsMatch(password))
			{
				num += 5;
			}
			if (regex3.IsMatch(password))
			{
				num++;
			}
			if (regex4.IsMatch(password))
			{
				num += 5;
			}
			if (regex5.IsMatch(password))
			{
				num += 5;
			}
			if (regex6.IsMatch(password))
			{
				num += 5;
			}
			if (regex.IsMatch(password) && regex2.IsMatch(password))
			{
				num += 2;
			}
			if (regex7.IsMatch(password) && regex8.IsMatch(password))
			{
				num += 2;
			}
			if (regex.IsMatch(password) && regex2.IsMatch(password) && regex7.IsMatch(password) && regex5.IsMatch(password))
			{
				num += 2;
			}
			return num;
		}
	}
}
