using AxGNNTKEYLib;
using CSharpWin;
using Gnnt.MixAccountPlugin;
using ModulesLoader;
using PluginInterface;
using SoftKeyboard;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.UI.Forms.PromptForms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame.UI.Forms
{
	public class LoginForm : Form
	{
		public delegate bool ShowBalanceFormCallBack();
		public delegate bool ShowCommitFormCallBack();
		private delegate void ShowFrameCallBack();
		private delegate void BooleanCallback(bool enable, string info);
		public delegate bool ChangePwdCallBack(IPlugin myPlugin);
		public LoginForm.ShowBalanceFormCallBack ShowBalanceForm;
		public LoginForm.ShowCommitFormCallBack ShowCommitForm;
		private SoftKey softKeyBoard;
		private bool isFrameCreated;
		private RecordUserName recordUserName;
		private string[] Args;
		private byte LoginMark;
		private int top = 20;
		private bool ishideNoLogin;
		private bool isHideLoginModule;
		private bool isEnableUserNameLogin;
		private LoginForm.ShowFrameCallBack showFrameCallBack;
		private LoginForm.BooleanCallback enableControls;
		public LoginForm.ChangePwdCallBack ChangePwd;
		private IPlugin currentLoginPlugin;
		private IContainer components;
		private Button buttonExit;
		private Button buttonLogin;
		private PasswordTextBox password;
		private TextBox username;
		private Label labPassword;
		private Label labUsername;
		private Label labTitle;
		private CheckBox userProtect;
		private CheckBox userMemory;
		private PictureBox pbVerifyCode;
		private TextBox tbVerifyCode;
		private Label lbVerifyCode;
		private LoadingCircle loadingCircle1;
		private AxGnntKey axGnntKey1;
		private PictureBox picSoftKey;
		private Button buttonNoLogin;
		private Button btTestSpeed;
		private Button btSet;
		private PictureBox pictureBoxClose;
		private Label labelLogin;
		private ComboBox comboBoxLogin;
		private Panel panelLogin;
		private ButtonEx buttonExCode;
		private ButtonEx buttonExName;
		public LoginForm(string[] args, IPlugin plugin, bool isFrame)
		{
			this.Args = args;
			this.InitializeComponent();
			if (plugin != null)
			{
				this.isHideLoginModule = true;
				this.currentLoginPlugin = plugin;
			}
			this.isFrameCreated = isFrame;
			if (this.isFrameCreated)
			{
				this.ishideNoLogin = true;
			}
		}
		private void SetFouce(short flag)
		{
			switch (flag)
			{
			case 0:
				this.username.Focus();
				return;
			case 1:
				this.password.Focus();
				return;
			case 2:
				this.tbVerifyCode.Focus();
				return;
			case 3:
				this.tbVerifyCode.Text = "";
				this.tbVerifyCode.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
				return;
			default:
				return;
			}
		}
		private bool DisPlayCommitForm()
		{
			bool flag = (bool)base.Invoke(this.ShowCommitForm);
			if (!flag)
			{
				this.EnableControls(true, "null");
			}
			return flag;
		}
		private bool DisPlayBalance()
		{
			bool flag = (bool)base.Invoke(this.ShowBalanceForm);
			if (!flag)
			{
				this.EnableControls(true, "null");
			}
			return flag;
		}
		private bool DisPlayChangePwd(IPlugin myPlugin)
		{
			this.ChangePwd = new LoginForm.ChangePwdCallBack(this.ShowChangePwd);
			return (bool)base.Invoke(this.ChangePwd, new object[]
			{
				myPlugin
			});
		}
		private bool ShowChangePwd(IPlugin myPlugin)
		{
			bool flag = OperationManager.GetInstance().displayFormsOperation.displayChangePwd(myPlugin);
			if (!flag)
			{
				this.EnableControls(true, "null");
			}
			return flag;
		}
		private void SetEnable(bool flag, string info)
		{
			this.HandleCreated();
			if (this.enableControls != null)
			{
				base.Invoke(this.enableControls, new object[]
				{
					flag,
					info
				});
			}
		}
		private void HideControl()
		{
			if (!this.isEnableUserNameLogin)
			{
				this.HideUserNameLogin();
			}
			if (this.isHideLoginModule)
			{
				this.HideLoginModule();
			}
			if (this.ishideNoLogin)
			{
				this.hideNologinButton();
			}
		}
		public void hideNologinButton()
		{
			this.buttonLogin.Left -= 60;
			this.btSet.Left -= 40;
			this.buttonExit.Left -= 20;
			this.buttonNoLogin.Hide();
		}
		private void HideLoginModule()
		{
			this.labelLogin.Visible = false;
			this.comboBoxLogin.Visible = false;
			this.labUsername.Top -= this.top;
			this.username.Top -= this.top;
			this.userMemory.Top -= this.top;
			this.labPassword.Top -= this.top - 5;
			this.password.Top -= this.top - 5;
			this.picSoftKey.Top -= this.top - 5;
			this.userProtect.Top -= this.top - 5;
			this.lbVerifyCode.Top -= this.top - 10;
			this.tbVerifyCode.Top -= this.top - 10;
			this.pbVerifyCode.Top -= this.top - 10;
		}
		private void HideUserNameLogin()
		{
			this.buttonExCode.Visible = false;
			this.buttonExName.Visible = false;
		}
		private void pbVerifyCode_Click(object sender, EventArgs e)
		{
			OperationManager.GetInstance().verifyCodeOparation.updatePic();
			this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
		}
		private void LoginForm_Load(object sender, EventArgs e)
		{
			this.ReadUserName();
			this.SetControlText();
			this.RegistMethod();
			string text = "MixAccount";
			try
			{
				OperationManager.GetInstance().mixAccountPlugin = (MixAccountPlugin)Global.Modules.get_Plugins().get_AvailablePlugins().Find(text).get_Instance();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void LoadComboxLogin()
		{
			this.comboBoxLogin.Items.Clear();
			string text = string.Empty;
			foreach (DictionaryEntry dictionaryEntry in Global.Modules.get_Plugins().get_LoginPluginsHashtable())
			{
				if (dictionaryEntry.Key != null)
				{
					this.comboBoxLogin.Items.Add(dictionaryEntry.Key);
				}
				if (this.currentLoginPlugin != null && dictionaryEntry.Value.ToString() == this.currentLoginPlugin.get_Name())
				{
					text = dictionaryEntry.Key.ToString();
				}
			}
			if (text != "")
			{
				this.comboBoxLogin.Text = text;
				return;
			}
			this.comboBoxLogin.SelectedIndex = 0;
		}
		private void RegistMethod()
		{
			this.ShowCommitForm = new LoginForm.ShowCommitFormCallBack(OperationManager.GetInstance().displayFormsOperation.displayCommit);
			this.ShowBalanceForm = new LoginForm.ShowBalanceFormCallBack(OperationManager.GetInstance().displayFormsOperation.displayBalance);
			this.enableControls = new LoginForm.BooleanCallback(this.EnableControls);
			OperationManager.GetInstance().verifyCodeOparation.EnableControls = new VerifyCodeOperation.EnableControlsCallBack(this.EnableControls);
			OperationManager.GetInstance().loginOperation.enableControls = new LoginOperation.BooleanCallback(this.SetEnable);
			OperationManager.GetInstance().verifyCodeOparation.SetFocus = new VerifyCodeOperation.SetFocusCallBack(this.SetFouce);
			OperationManager.GetInstance().speedTestOperation.refreshLoginComboBox = new SpeedTestOperation.RefreshLoginComboBox(this.comboBoxLoginRefresh);
			OperationManager.GetInstance().updateOperation.CloseForm = new UpdateOperation.CloseFormCallBack(this.CloseForm);
			OperationManager.GetInstance().loginOperation.showFrame = new LoginOperation.ShowFrame(this.ShowFrame);
			OperationManager.GetInstance().updateOperation.StartCheckUpdate(this);
			OperationManager.GetInstance().loginOperation.ShowCommitForm = new LoginOperation.ShowCommitFormCallBack(this.DisPlayCommitForm);
			OperationManager.GetInstance().loginOperation.ShowBalanceForm = new LoginOperation.ShowBalanceFormCallBack(this.DisPlayBalance);
			OperationManager.GetInstance().loginOperation.ChangePwd = new LoginOperation.ChangePwdCallBack(this.DisPlayChangePwd);
		}
		private void buttonLogin_Click(object sender, EventArgs e)
		{
			LogonOperationInfo logonOperationInfo = new LogonOperationInfo();
			logonOperationInfo.username = this.username.Text;
			this.password.set_CheckPass(true);
			logonOperationInfo.password = this.password.Text;
			this.password.set_CheckPass(false);
			logonOperationInfo.axgnntkey = this.axGnntKey1;
			logonOperationInfo.loginmark = (short)this.LoginMark;
			logonOperationInfo.isMemoryChecked = this.userMemory.Checked;
			logonOperationInfo.isProtectChecked = this.userProtect.Checked;
			logonOperationInfo.verifycode = this.tbVerifyCode.Text;
			logonOperationInfo.verifyCodeString = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeString;
			logonOperationInfo.myPlugin = this.currentLoginPlugin;
			OperationManager.GetInstance().stripButtonOperation.curLoginPluginName = this.currentLoginPlugin.get_Name();
			OperationManager.GetInstance().verifyCodeOparation.VerifyUserInfo(logonOperationInfo, base.Handle);
		}
		private void buttonExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void CloseForm()
		{
			base.Close();
		}
		private void SetControlText()
		{
			new BitmapRegion();
			Image image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_TradeLoginBG");
			if (image != null)
			{
				Bitmap bitmap = new Bitmap((Bitmap)image);
				BitmapRegion.CreateControlRegion(this, bitmap);
			}
			this.Text = string.Format("{0}", (string)Global.htConfig["Title"]);
			this.labTitle.Text = (string)Global.htConfig["CompanyName"];
			this.labUsername.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LAB_USERNAME");
			this.labPassword.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LAB_PASSWORD");
			this.buttonLogin.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_LOGIN");
			this.buttonExit.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_EXIT");
			this.buttonNoLogin.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_BUTTON_NOLOGIN");
			this.btTestSpeed.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_btTestSpeed");
			this.btSet.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_btSet");
			this.lbVerifyCode.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_lbVerifyCode");
			this.userMemory.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_userMemory");
			this.userProtect.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_userProtect");
			this.buttonLogin.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_btn");
			this.buttonNoLogin.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_TestSpeed");
			this.buttonExit.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_TestSpeed");
			this.btTestSpeed.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_TestSpeed");
			this.btSet.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_TestSpeed");
			this.pictureBoxClose.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_Close");
			this.picSoftKey.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_defalutKeyBoard");
			base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
			this.username.MaxLength = Tools.ObjToInt(Global.htConfig["UsernameMaxLength"]);
			this.loadingCircle1.Visible = false;
			this.tbVerifyCode.MaxLength = OperationManager.GetInstance().verifyCodeOparation.Codelength;
			this.labelLogin.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_loginModule");
			this.comboBoxLogin.Items.Clear();
			this.comboBoxLogin.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_moduleLoading");
			this.isEnableUserNameLogin = Tools.StrToBool((string)Global.htConfig["IsEnableUserNamLogin"], false);
			this.HideControl();
			if (this.isEnableUserNameLogin)
			{
				this.SetButtonExForColor();
			}
			ScaleForm.ScaleForms(this);
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void EnableControls(bool enable, string info)
		{
			this.comboBoxLogin.Enabled = enable;
			this.username.Enabled = enable;
			this.password.Enabled = enable;
			this.tbVerifyCode.Enabled = enable;
			this.userMemory.Enabled = enable;
			this.userProtect.Enabled = enable;
			this.pbVerifyCode.Enabled = enable;
			this.buttonLogin.Enabled = enable;
			this.picSoftKey.Enabled = enable;
			this.buttonNoLogin.Enabled = enable;
			this.loadingCircle1.Visible = !enable;
			this.loadingCircle1.set_Active(!enable);
			this.pictureBoxClose.Visible = enable;
			string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_PasswordError");
			string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_NoExistTradeCode");
			Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_LoginFail");
			if (info.Equals(@string))
			{
				this.password.Text = "";
				this.tbVerifyCode.Text = "";
				this.password.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
				return;
			}
			if (info.Equals(string2))
			{
				this.username.Text = "";
				this.tbVerifyCode.Text = "";
				this.username.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
				return;
			}
			if (enable)
			{
				this.tbVerifyCode.Text = "";
				this.tbVerifyCode.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
			}
		}
		private void userProtect_CheckedChanged(object sender, EventArgs e)
		{
			if (this.userProtect.Checked)
			{
				this.username.PasswordChar = '#';
			}
			else
			{
				this.username.PasswordChar = '\0';
			}
			this.userMemory_Click(sender, e);
		}
		private void userMemory_Click(object sender, EventArgs e)
		{
			if (this.username.Text != "")
			{
				this.RecordUserName();
			}
		}
		private void ReadUserName()
		{
			if (this.recordUserName == null)
			{
				this.recordUserName = new RecordUserName();
			}
			ReCordUserInfo reCordUserInfo = this.recordUserName.readUsername();
			if (reCordUserInfo != null)
			{
				this.username.Text = reCordUserInfo.username;
				this.userMemory.Checked = reCordUserInfo.isMemoryChecked;
				this.userProtect.Checked = reCordUserInfo.isProtectChecked;
			}
		}
		private void RecordUserName()
		{
			if (this.recordUserName == null)
			{
				this.recordUserName = new RecordUserName();
			}
			ReCordUserInfo reCordUserInfo = new ReCordUserInfo();
			reCordUserInfo.username = this.username.Text;
			reCordUserInfo.isMemoryChecked = this.userMemory.Checked;
			reCordUserInfo.isProtectChecked = this.userProtect.Checked;
			this.recordUserName.recordUsername(reCordUserInfo);
		}
		private void LoginForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized && e.Clicks == 1)
			{
				WinSendMessage.SendMessageToMoveForm(base.Handle);
			}
		}
		private void username_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar >= '！' && e.KeyChar <= '｝')
			{
				e.KeyChar -= 'ﻠ';
			}
		}
		private void tbVerifyCode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar >= '！' && e.KeyChar <= '｝')
			{
				e.KeyChar -= 'ﻠ';
			}
		}
		private void picSoftKey_Click(object sender, EventArgs e)
		{
			if (this.softKeyBoard == null)
			{
				this.softKeyBoard = new SoftKey(this.password);
			}
			if (this.softKeyBoard.Visible)
			{
				this.softKeyBoard.Hide();
			}
			else
			{
				this.softKeyBoard.Show();
			}
			this.softKeyBoard.SoftKeyLocation();
		}
		private void picSoftKey_MouseHover(object sender, EventArgs e)
		{
			this.picSoftKey.Cursor = Cursors.Hand;
		}
		private void labelTest_Click(object sender, EventArgs e)
		{
			TestSpeedForm testSpeedForm = new TestSpeedForm();
			testSpeedForm.ShowDialog();
		}
		private void labelSet_Click(object sender, EventArgs e)
		{
			string text = "ServerSet";
			try
			{
				IPlugin instance = Global.Modules.get_Plugins().get_AvailablePlugins().Find(text).get_Instance();
				string empty = string.Empty;
				Form form = instance.GetForm(false, ref empty);
				if (form.ShowDialog() == DialogResult.OK)
				{
					string @string = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_serverSet");
					string string2 = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LoginForm_serverSetContext");
					if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
					{
						Global.Modules.get_Plugins().ClosePlugins();
						Global.Modules.LoadPlugins();
						Global.ModuleInfos.Clear();
						foreach (ModuleInfo current in Global.Modules.get_Modules())
						{
							Global.ModuleInfos.Add(current.get_ModuleNo(), current.get_ModuleName());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void buttonNoLogin_Click(object sender, EventArgs e)
		{
			if (this.Args != null)
			{
				Frame frame = new Frame(this.Args);
				frame.Show();
			}
			base.Hide();
		}
		private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!OperationManager.GetInstance().updateOperation.updateColse && !this.isFrameCreated)
			{
				TimerForm timerForm = new TimerForm();
				timerForm.ShowDialog();
				if (timerForm.Result)
				{
					Global.Modules.get_Plugins().ClosePlugins();
					base.Dispose();
					base.Close();
					Application.ExitThread();
					Application.Exit();
					Process.GetCurrentProcess().Kill();
					return;
				}
				e.Cancel = true;
			}
		}
		private void pictureBoxClose_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBoxClose.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_Close1");
		}
		private void pictureBoxClose_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBoxClose.Image = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_dl_Close");
		}
		private void comboBoxLogin_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxLogin.Visible)
			{
				this.currentLoginPlugin = Global.Modules.get_Plugins().get_AvailablePlugins().Find(Global.Modules.get_Plugins().get_LoginPluginsHashtable()[this.comboBoxLogin.Text].ToString()).get_Instance();
				OperationManager.GetInstance().speedTestOperation.myPlugin = this.currentLoginPlugin;
			}
		}
		private void LoginForm_Shown(object sender, EventArgs e)
		{
			if (this.comboBoxLogin.Visible)
			{
				this.LoadComboxLogin();
			}
			OperationManager.GetInstance().verifyCodeOparation.updatePic();
			this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
		}
		private void comboBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}
		private void panelLogin_Paint(object sender, PaintEventArgs e)
		{
			if (this.isEnableUserNameLogin)
			{
				ControlPaint.DrawBorder(e.Graphics, this.panelLogin.ClientRectangle, Color.DodgerBlue, 1, ButtonBorderStyle.Solid, Color.DodgerBlue, 1, ButtonBorderStyle.Solid, Color.DodgerBlue, 1, ButtonBorderStyle.Solid, Color.DodgerBlue, 1, ButtonBorderStyle.Solid);
			}
		}
		private void SetButtonExForColor()
		{
			switch (this.LoginMark)
			{
			case 0:
				this.buttonExCode.ForeColor = Color.DarkSlateGray;
				this.buttonExName.ForeColor = Color.White;
				this.labUsername.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LAB_Code");
				return;
			case 1:
				this.buttonExCode.ForeColor = Color.White;
				this.buttonExName.ForeColor = Color.DarkSlateGray;
				this.labUsername.Text = Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetString("TradeStr_LAB_USERNAME");
				return;
			default:
				return;
			}
		}
		private void buttonExCode_Click(object sender, EventArgs e)
		{
			this.LoginMark = 0;
			this.SetButtonExForColor();
			OperationManager.GetInstance().verifyCodeOparation.updatePic();
			this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
		}
		private void buttonExName_Click(object sender, EventArgs e)
		{
			this.LoginMark = 1;
			this.SetButtonExForColor();
			OperationManager.GetInstance().verifyCodeOparation.updatePic();
			this.pbVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
		}
		public void ShowFrame()
		{
			try
			{
				OperationManager.GetInstance().isLogin = true;
				this.showFrameCallBack = new LoginForm.ShowFrameCallBack(this.CreateFrame);
				this.HandleCreated();
				base.Invoke(this.showFrameCallBack);
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "创建框架异常：" + ex.Message);
			}
		}
		public void CreateFrame()
		{
			OperationManager.GetInstance().stripButtonOperation.SetIsLogin(true, this.currentLoginPlugin.get_Name());
			if (!this.isFrameCreated)
			{
				ProgramOperation.CreateSysFrame(this.Args);
				base.Hide();
				return;
			}
			base.Close();
		}
		public void comboBoxLoginRefresh(string name)
		{
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoginForm));
			this.userProtect = new CheckBox();
			this.userMemory = new CheckBox();
			this.labTitle = new Label();
			this.labUsername = new Label();
			this.labPassword = new Label();
			this.buttonExit = new Button();
			this.username = new TextBox();
			this.buttonLogin = new Button();
			this.pbVerifyCode = new PictureBox();
			this.tbVerifyCode = new TextBox();
			this.lbVerifyCode = new Label();
			this.loadingCircle1 = new LoadingCircle();
			this.picSoftKey = new PictureBox();
			this.buttonNoLogin = new Button();
			this.btTestSpeed = new Button();
			this.btSet = new Button();
			this.pictureBoxClose = new PictureBox();
			this.labelLogin = new Label();
			this.comboBoxLogin = new ComboBox();
			this.password = new PasswordTextBox();
			this.panelLogin = new Panel();
			this.axGnntKey1 = new AxGnntKey();
			this.buttonExCode = new ButtonEx();
			this.buttonExName = new ButtonEx();
			((ISupportInitialize)this.pbVerifyCode).BeginInit();
			((ISupportInitialize)this.picSoftKey).BeginInit();
			((ISupportInitialize)this.pictureBoxClose).BeginInit();
			this.panelLogin.SuspendLayout();
			this.axGnntKey1.BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.userProtect, "userProtect");
			this.userProtect.BackColor = Color.Transparent;
			this.userProtect.ForeColor = Color.White;
			this.userProtect.Name = "userProtect";
			this.userProtect.UseVisualStyleBackColor = false;
			this.userProtect.CheckedChanged += new EventHandler(this.userProtect_CheckedChanged);
			componentResourceManager.ApplyResources(this.userMemory, "userMemory");
			this.userMemory.BackColor = Color.Transparent;
			this.userMemory.Checked = true;
			this.userMemory.CheckState = CheckState.Checked;
			this.userMemory.ForeColor = Color.White;
			this.userMemory.Name = "userMemory";
			this.userMemory.UseVisualStyleBackColor = false;
			this.userMemory.Click += new EventHandler(this.userMemory_Click);
			this.labTitle.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.labTitle, "labTitle");
			this.labTitle.Name = "labTitle";
			componentResourceManager.ApplyResources(this.labUsername, "labUsername");
			this.labUsername.BackColor = Color.Transparent;
			this.labUsername.ForeColor = Color.White;
			this.labUsername.Name = "labUsername";
			componentResourceManager.ApplyResources(this.labPassword, "labPassword");
			this.labPassword.BackColor = Color.Transparent;
			this.labPassword.ForeColor = Color.White;
			this.labPassword.Name = "labPassword";
			this.buttonExit.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.buttonExit, "buttonExit");
			this.buttonExit.DialogResult = DialogResult.Cancel;
			this.buttonExit.FlatAppearance.BorderSize = 0;
			this.buttonExit.ForeColor = Color.White;
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.UseVisualStyleBackColor = false;
			this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
			componentResourceManager.ApplyResources(this.username, "username");
			this.username.Name = "username";
			this.username.Tag = "false";
			this.username.KeyPress += new KeyPressEventHandler(this.username_KeyPress);
			this.buttonLogin.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.buttonLogin, "buttonLogin");
			this.buttonLogin.FlatAppearance.BorderSize = 0;
			this.buttonLogin.ForeColor = Color.White;
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.UseVisualStyleBackColor = false;
			this.buttonLogin.Click += new EventHandler(this.buttonLogin_Click);
			componentResourceManager.ApplyResources(this.pbVerifyCode, "pbVerifyCode");
			this.pbVerifyCode.Name = "pbVerifyCode";
			this.pbVerifyCode.TabStop = false;
			this.pbVerifyCode.Click += new EventHandler(this.pbVerifyCode_Click);
			componentResourceManager.ApplyResources(this.tbVerifyCode, "tbVerifyCode");
			this.tbVerifyCode.Name = "tbVerifyCode";
			this.tbVerifyCode.Tag = "false";
			this.tbVerifyCode.KeyPress += new KeyPressEventHandler(this.tbVerifyCode_KeyPress);
			componentResourceManager.ApplyResources(this.lbVerifyCode, "lbVerifyCode");
			this.lbVerifyCode.BackColor = Color.Transparent;
			this.lbVerifyCode.ForeColor = Color.White;
			this.lbVerifyCode.Name = "lbVerifyCode";
			this.loadingCircle1.set_Active(false);
			this.loadingCircle1.BackColor = Color.Transparent;
			this.loadingCircle1.set_Color(Color.DarkOrange);
			this.loadingCircle1.set_InnerCircleRadius(5);
			componentResourceManager.ApplyResources(this.loadingCircle1, "loadingCircle1");
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.set_NumberSpoke(12);
			this.loadingCircle1.set_OuterCircleRadius(11);
			this.loadingCircle1.set_RotationSpeed(100);
			this.loadingCircle1.set_SpokeThickness(2);
			this.loadingCircle1.set_StylePreset(0);
			this.picSoftKey.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.picSoftKey, "picSoftKey");
			this.picSoftKey.Name = "picSoftKey";
			this.picSoftKey.TabStop = false;
			this.picSoftKey.Click += new EventHandler(this.picSoftKey_Click);
			this.picSoftKey.MouseHover += new EventHandler(this.picSoftKey_MouseHover);
			this.buttonNoLogin.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.buttonNoLogin, "buttonNoLogin");
			this.buttonNoLogin.DialogResult = DialogResult.Cancel;
			this.buttonNoLogin.FlatAppearance.BorderSize = 0;
			this.buttonNoLogin.ForeColor = Color.White;
			this.buttonNoLogin.Name = "buttonNoLogin";
			this.buttonNoLogin.UseVisualStyleBackColor = false;
			this.buttonNoLogin.Click += new EventHandler(this.buttonNoLogin_Click);
			this.btTestSpeed.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.btTestSpeed, "btTestSpeed");
			this.btTestSpeed.FlatAppearance.BorderSize = 0;
			this.btTestSpeed.ForeColor = Color.White;
			this.btTestSpeed.Name = "btTestSpeed";
			this.btTestSpeed.UseVisualStyleBackColor = false;
			this.btTestSpeed.Click += new EventHandler(this.labelTest_Click);
			this.btSet.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.btSet, "btSet");
			this.btSet.FlatAppearance.BorderSize = 0;
			this.btSet.ForeColor = Color.White;
			this.btSet.Name = "btSet";
			this.btSet.UseVisualStyleBackColor = false;
			this.btSet.Click += new EventHandler(this.labelSet_Click);
			this.pictureBoxClose.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxClose, "pictureBoxClose");
			this.pictureBoxClose.Name = "pictureBoxClose";
			this.pictureBoxClose.TabStop = false;
			this.pictureBoxClose.Click += new EventHandler(this.buttonExit_Click);
			this.pictureBoxClose.MouseEnter += new EventHandler(this.pictureBoxClose_MouseEnter);
			this.pictureBoxClose.MouseLeave += new EventHandler(this.pictureBoxClose_MouseLeave);
			componentResourceManager.ApplyResources(this.labelLogin, "labelLogin");
			this.labelLogin.BackColor = Color.Transparent;
			this.labelLogin.ForeColor = Color.White;
			this.labelLogin.Name = "labelLogin";
			componentResourceManager.ApplyResources(this.comboBoxLogin, "comboBoxLogin");
			this.comboBoxLogin.FormattingEnabled = true;
			this.comboBoxLogin.Name = "comboBoxLogin";
			this.comboBoxLogin.SelectedIndexChanged += new EventHandler(this.comboBoxLogin_SelectedIndexChanged);
			this.comboBoxLogin.KeyPress += new KeyPressEventHandler(this.comboBoxLogin_KeyPress);
			this.password.set_CheckPass(false);
			componentResourceManager.ApplyResources(this.password, "password");
			this.password.Name = "password";
			this.password.Tag = "false";
			this.panelLogin.BackColor = Color.Transparent;
			this.panelLogin.Controls.Add(this.picSoftKey);
			this.panelLogin.Controls.Add(this.labUsername);
			this.panelLogin.Controls.Add(this.comboBoxLogin);
			this.panelLogin.Controls.Add(this.username);
			this.panelLogin.Controls.Add(this.btSet);
			this.panelLogin.Controls.Add(this.labelLogin);
			this.panelLogin.Controls.Add(this.password);
			this.panelLogin.Controls.Add(this.userMemory);
			this.panelLogin.Controls.Add(this.labPassword);
			this.panelLogin.Controls.Add(this.lbVerifyCode);
			this.panelLogin.Controls.Add(this.buttonNoLogin);
			this.panelLogin.Controls.Add(this.userProtect);
			this.panelLogin.Controls.Add(this.pbVerifyCode);
			this.panelLogin.Controls.Add(this.tbVerifyCode);
			this.panelLogin.Controls.Add(this.buttonExit);
			this.panelLogin.Controls.Add(this.buttonLogin);
			componentResourceManager.ApplyResources(this.panelLogin, "panelLogin");
			this.panelLogin.Name = "panelLogin";
			this.panelLogin.Paint += new PaintEventHandler(this.panelLogin_Paint);
			componentResourceManager.ApplyResources(this.axGnntKey1, "axGnntKey1");
			this.axGnntKey1.Name = "axGnntKey1";
			this.axGnntKey1.OcxState = (AxHost.State)componentResourceManager.GetObject("axGnntKey1.OcxState");
			componentResourceManager.ApplyResources(this.buttonExCode, "buttonExCode");
			this.buttonExCode.set_BaseColor(Color.DodgerBlue);
			this.buttonExCode.ForeColor = Color.DarkSlateGray;
			this.buttonExCode.Name = "buttonExCode";
			this.buttonExCode.set_RoundStyle(4);
			this.buttonExCode.UseVisualStyleBackColor = true;
			this.buttonExCode.Click += new EventHandler(this.buttonExCode_Click);
			componentResourceManager.ApplyResources(this.buttonExName, "buttonExName");
			this.buttonExName.set_BaseColor(Color.DodgerBlue);
			this.buttonExName.ForeColor = Color.White;
			this.buttonExName.Name = "buttonExName";
			this.buttonExName.set_RoundStyle(4);
			this.buttonExName.UseVisualStyleBackColor = true;
			this.buttonExName.Click += new EventHandler(this.buttonExName_Click);
			base.AcceptButton = this.buttonLogin;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.buttonExName);
			base.Controls.Add(this.buttonExCode);
			base.Controls.Add(this.panelLogin);
			base.Controls.Add(this.pictureBoxClose);
			base.Controls.Add(this.btTestSpeed);
			base.Controls.Add(this.axGnntKey1);
			base.Controls.Add(this.loadingCircle1);
			base.Controls.Add(this.labTitle);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LoginForm";
			base.FormClosing += new FormClosingEventHandler(this.LoginForm_FormClosing);
			base.Load += new EventHandler(this.LoginForm_Load);
			base.Shown += new EventHandler(this.LoginForm_Shown);
			base.MouseDown += new MouseEventHandler(this.LoginForm_MouseDown);
			((ISupportInitialize)this.pbVerifyCode).EndInit();
			((ISupportInitialize)this.picSoftKey).EndInit();
			((ISupportInitialize)this.pictureBoxClose).EndInit();
			this.panelLogin.ResumeLayout(false);
			this.panelLogin.PerformLayout();
			this.axGnntKey1.EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
