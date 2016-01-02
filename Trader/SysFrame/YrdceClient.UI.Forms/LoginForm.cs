using AxKEYLib;
using CSharpWin;
using Gnnt.MixAccountPlugin;
using ModulesLoader;
using PluginInterface;
using SoftKeyboard;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation;
using YrdceClient.Yrdce.Common.Operation.Manager;
using YrdceClient.Yrdce.Common.Operation.MainOperation;
using YrdceClient.UI.Forms.PromptForms;
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
using System.Net.NetworkInformation;
using System.IO;
using YrdceClient.Yrdce.Common.Operation;
using YrdceClient.Properties;
namespace YrdceClient.UI.Forms
{
	public class LoginForm : Form
	{
		public delegate bool ShowBalanceFormCallBack();
		public delegate bool ShowCommitFormCallBack();
		private delegate void ShowClientCallBack();
		private delegate void BooleanCallback(bool enable, string info);
		private IContainer components;
		private Button btnLogon;
		private PasswordTextBox password;
		private TextBox txtUserName;
		private Label lblPassword;
		private Label labUsername;
		private Label labTitle;
		private CheckBox cboxUserProtect;
		private CheckBox userMemory;
		private PictureBox picVerifyCode;
		private TextBox txtVerifyCode;
		private Label lbVerifyCode;
		private LoadingCircle loadingCircle1;
		private PictureBox picSoftKey;
		private Button buttonNoLogin;
		private Button btSet;
		private PictureBox pictureBoxClose;
		private Label labelLogin;
		private ComboBox comboBoxLogin;
		private Panel panelLogin;
		public LoginForm.ShowBalanceFormCallBack ShowBalanceForm;
		public LoginForm.ShowCommitFormCallBack ShowCommitForm;
		private SoftKey softKeyBoard;
		private bool isClientCreated;
		private RecordUserName recordUserName;
		private string[] Args;
		private byte LoginMark;
		private int top = 20;
		private bool ishideNoLogin;
		private bool isHideLoginModule;
		private bool isEnableUserNameLogin;
		private LoginForm.ShowClientCallBack showClientCallBack;
		private LoginForm.BooleanCallback enableControls;
		private IPlugin currentLoginPlugin;
        private ProgressBar progressBarLogin;
        private System.Windows.Forms.Timer timer1;
        private Label labelNumber;

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
            //NewPing();
            this.ClientSize = new System.Drawing.Size(710,469);
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoginForm));
			this.cboxUserProtect = new CheckBox();
			this.userMemory = new CheckBox();
			this.labTitle = new Label();
			this.labUsername = new Label();
			this.lblPassword = new Label();
			this.txtUserName = new TextBox();
			this.btnLogon = new Button();
			this.picVerifyCode = new PictureBox();
			this.txtVerifyCode = new TextBox();
			this.lbVerifyCode = new Label();
			this.loadingCircle1 = new LoadingCircle();
			this.picSoftKey = new PictureBox();
			this.buttonNoLogin = new Button();
			this.btSet = new Button();
			this.pictureBoxClose = new PictureBox();
			this.labelLogin = new Label();
			this.comboBoxLogin = new ComboBox();
			this.password = new PasswordTextBox();
			this.panelLogin = new Panel();
            this.progressBarLogin = new ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer();
            this.labelNumber = new Label();

			((ISupportInitialize)this.picVerifyCode).BeginInit();
			((ISupportInitialize)this.picSoftKey).BeginInit();
			((ISupportInitialize)this.pictureBoxClose).BeginInit();
            this.panelLogin.SuspendLayout();
            base.SuspendLayout();
    
			componentResourceManager.ApplyResources(this.cboxUserProtect, "userProtect");
			this.cboxUserProtect.BackColor = Color.Transparent;
            this.cboxUserProtect.ForeColor = Color.Black;
            this.cboxUserProtect.Left += 5;
            this.cboxUserProtect.Font = new Font("微软雅黑", 10);
            this.cboxUserProtect.Name = "userProtect";
			this.cboxUserProtect.UseVisualStyleBackColor = false;
			this.cboxUserProtect.CheckedChanged += new EventHandler(this.userProtect_CheckedChanged);
			componentResourceManager.ApplyResources(this.userMemory, "userMemory");
			this.userMemory.BackColor = Color.Transparent;
			this.userMemory.Checked = true;
			this.userMemory.CheckState = CheckState.Checked;
			this.userMemory.ForeColor = Color.Black;
			this.userMemory.Name = "userMemory";

            this.userMemory.Left += 5;
            this.userMemory.Font = new Font("微软雅黑", 10);
			this.userMemory.UseVisualStyleBackColor = false;
			this.userMemory.Click += new EventHandler(this.userMemory_Click);
			this.labTitle.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.labTitle, "labTitle");
			this.labTitle.Name = "labTitle";
            this.labTitle.Font = new Font("微软雅黑", 12);
            this.labTitle.ForeColor = Color.Black;
            componentResourceManager.ApplyResources(this.labUsername, "labUsername");
			this.labUsername.BackColor = Color.Transparent;
			this.labUsername.ForeColor = Color.Black;
            this.labUsername.Font = new Font("微软雅黑", 12);
            this.labUsername.Left += 30;
            this.labUsername.ForeColor = Color.FromArgb(251, 178, 23);
            this.labUsername.Name = "labUsername";
			componentResourceManager.ApplyResources(this.lblPassword, "labPassword");
			this.lblPassword.BackColor = Color.Transparent;
			this.lblPassword.ForeColor = Color.Black;
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Font = new Font("微软雅黑", 12);
            this.lblPassword.Left += 35;
            this.lblPassword.ForeColor = Color.FromArgb(251, 178, 23);
            componentResourceManager.ApplyResources(this.txtUserName, "username");
			this.txtUserName.Name = "username";
			this.txtUserName.Tag = "false";
            this.txtUserName.BorderStyle = BorderStyle.FixedSingle;
            this.txtUserName.Width =180;
            this.txtUserName.KeyPress += new KeyPressEventHandler(this.username_KeyPress);
			this.btnLogon.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.btnLogon, "buttonLogin");
			this.btnLogon.FlatAppearance.BorderSize = 0;
			this.btnLogon.ForeColor = Color.White;
			this.btnLogon.Name = "btnLogon";
            this.btnLogon.Left += 50;
			this.btnLogon.UseVisualStyleBackColor = false;
			this.btnLogon.Click += new EventHandler(this.btnLogon_Click);
			componentResourceManager.ApplyResources(this.picVerifyCode, "pbVerifyCode");
			this.picVerifyCode.Name = "pbVerifyCode";
			this.picVerifyCode.TabStop = false;
            this.picVerifyCode.Left += 5;
            this.picVerifyCode.Click += new EventHandler(this.picVerifyCode_Click);
			componentResourceManager.ApplyResources(this.txtVerifyCode, "tbVerifyCode");
			this.txtVerifyCode.Name = "tbVerifyCode";
			this.txtVerifyCode.Tag = "false";
            this.txtVerifyCode.BorderStyle = BorderStyle.FixedSingle;
            this.txtVerifyCode.Width = 180;
			this.txtVerifyCode.KeyPress += new KeyPressEventHandler(this.txtVerifyCode_KeyPress);
			componentResourceManager.ApplyResources(this.lbVerifyCode, "lbVerifyCode");
			this.lbVerifyCode.BackColor = Color.Transparent;
			this.lbVerifyCode.ForeColor = Color.Black;
            this.lbVerifyCode.Font = new Font("微软雅黑", 12);
            this.lbVerifyCode.Left += 30;
            this.lbVerifyCode.ForeColor = Color.FromArgb(251, 178, 23);
            this.lbVerifyCode.Name = "lbVerifyCode";
			this.loadingCircle1.Active = false;
			this.loadingCircle1.BackColor = Color.Transparent;
			this.loadingCircle1.Color = Color.DarkOrange;
			this.loadingCircle1.InnerCircleRadius = 5;
			componentResourceManager.ApplyResources(this.loadingCircle1, "loadingCircle1");
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.NumberSpoke = 12;
			this.loadingCircle1.OuterCircleRadius = 11;
			this.loadingCircle1.RotationSpeed = 100;
			this.loadingCircle1.SpokeThickness = 2;
            this.loadingCircle1.Left += 190;
            this.loadingCircle1.Top -= 25;
            this.loadingCircle1.StylePreset = LoadingCircle.StylePresets.MacOSX;
			this.picSoftKey.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.picSoftKey, "picSoftKey");
			this.picSoftKey.Name = "picSoftKey";
			this.picSoftKey.TabStop = false;
            //this.picSoftKey.Left += 25;
			this.picSoftKey.Click += new EventHandler(this.picSoftKey_Click);
			this.picSoftKey.MouseHover += new EventHandler(this.picSoftKey_MouseHover);
			this.buttonNoLogin.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.buttonNoLogin, "buttonNoLogin");
			this.buttonNoLogin.DialogResult = DialogResult.Cancel;
			this.buttonNoLogin.FlatAppearance.BorderSize = 0;
			this.buttonNoLogin.ForeColor = Color.White;
			this.buttonNoLogin.Name = "buttonNoLogin";
            this.buttonNoLogin.Left += 50;
            this.buttonNoLogin.UseVisualStyleBackColor = false;
			this.buttonNoLogin.Click += new EventHandler(this.buttonNoLogin_Click);
			this.btSet.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.btSet, "btSet");
			this.btSet.FlatAppearance.BorderSize = 0;
			this.btSet.ForeColor = Color.White;
			this.btSet.Name = "btSet";
            this.btSet.Left += 50;
			this.btSet.UseVisualStyleBackColor = false;
			this.btSet.Click += new EventHandler(this.labelSet_Click);
			this.pictureBoxClose.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxClose, "pictureBoxClose");
			this.pictureBoxClose.Name = "pictureBoxClose";
			this.pictureBoxClose.TabStop = false;
            this.pictureBoxClose.Left += 190;
            this.pictureBoxClose.Top -= 25;
			this.pictureBoxClose.Click += new EventHandler(this.btnExit_Click);
			this.pictureBoxClose.MouseEnter += new EventHandler(this.pictureBoxClose_MouseEnter);
			this.pictureBoxClose.MouseLeave += new EventHandler(this.pictureBoxClose_MouseLeave);
			componentResourceManager.ApplyResources(this.labelLogin, "labelLogin");
			this.labelLogin.BackColor = Color.Transparent;
			this.labelLogin.ForeColor = Color.Black;
            this.labelLogin.Font = new Font("微软雅黑", 12);
            this.labelLogin.Left += 30;
            this.labelLogin.ForeColor = Color.FromArgb(251, 178, 23);
            this.labelLogin.Name = "labelLogin";
			componentResourceManager.ApplyResources(this.comboBoxLogin, "comboBoxLogin");
			this.comboBoxLogin.FormattingEnabled = true;
            this.comboBoxLogin.FlatStyle = FlatStyle.Flat;
            this.comboBoxLogin.Width = 180;
			this.comboBoxLogin.Name = "comboBoxLogin";
            this.comboBoxLogin.SelectedIndexChanged += new EventHandler(this.comboBoxLogin_SelectedIndexChanged);
			this.comboBoxLogin.KeyPress += new KeyPressEventHandler(this.comboBoxLogin_KeyPress);
            this.password.BorderStyle = BorderStyle.FixedSingle;
            //
            //progressBarLogin
            //
            this.progressBarLogin.Location = new Point(this.Left,this.Bottom-20);
            this.progressBarLogin.Height = 10;
            this.progressBarLogin.Width = this.Width;

            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            //
            //labelNumber
            //
            this.labelNumber.Location = new Point(515, 420);
            this.labelNumber.Text = "0%";
            //this.labelNumber.ForeColor = Color.White;
            this.labelNumber.Font = new System.Drawing.Font("微软雅黑",12);
            this.labelNumber.BackColor = Color.Transparent;

			componentResourceManager.ApplyResources(this.password, "password");
            this.password.Width = 180;
            this.password.Name = "password";
            this.password.Tag = "false";
            this.panelLogin.BackColor = Color.Transparent;
            this.panelLogin.Controls.Add(this.picSoftKey);
            this.panelLogin.Controls.Add(this.labUsername);
            this.panelLogin.Controls.Add(this.comboBoxLogin);
            this.panelLogin.Controls.Add(this.txtUserName);
            this.panelLogin.Controls.Add(this.btSet);
            this.panelLogin.Controls.Add(this.labelLogin);
            this.panelLogin.Controls.Add(this.password);
            this.panelLogin.Controls.Add(this.userMemory);
            this.panelLogin.Controls.Add(this.lblPassword);
            this.panelLogin.Controls.Add(this.lbVerifyCode);
            this.panelLogin.Controls.Add(this.buttonNoLogin);
            this.panelLogin.Controls.Add(this.cboxUserProtect);
            this.panelLogin.Controls.Add(this.picVerifyCode);
            this.panelLogin.Controls.Add(this.txtVerifyCode);
            //this.Controls.Add(this.progressBarLogin);
            //this.Controls.Add(this.labelNumber);

            this.panelLogin.Controls.Add(this.btnLogon);
            componentResourceManager.ApplyResources(this.panelLogin, "panelLogin");
			this.panelLogin.Name = "panelLogin";
            this.panelLogin.Left += 220;
            this.panelLogin.Top += 50;
            this.panelLogin.Paint += new PaintEventHandler(this.panelLogin_Paint);
			base.AcceptButton = this.btnLogon;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.panelLogin);
            this.Controls.Add(this.pictureBoxClose);
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
			((ISupportInitialize)this.picVerifyCode).EndInit();
			((ISupportInitialize)this.picSoftKey).EndInit();
			((ISupportInitialize)this.pictureBoxClose).EndInit();
			this.panelLogin.ResumeLayout(false);
			this.panelLogin.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

        //private void NewPing()
        //{
        //    Ping p = new Ping();
        //    PingReply pr;
        //    pr = p.Send("119.75.217.109");//IP
        //    if (pr.Status != IPStatus.Success)//如果连接不成功
        //    {
        //        DialogResult r = MessageBox.Show("无法连接到网络，请检查网络后重新登录。", "长三角商品交易所", MessageBoxButtons.OK, MessageBoxIcon.Hand);

        //        if (r == System.Windows.Forms.DialogResult.OK)
        //        {
        //            this.Close();

        //        }


        //    }
        //}
        private void NewPingLogin()
        {
            Ping p = new Ping();
            PingReply pr;
            pr = p.Send("119.75.217.109");//IP
            if (pr.Status != IPStatus.Success)//如果连接不成功
            {
                DialogResult r = MessageBox.Show("无法连接到网络，请检查网络后重新登录。", "长三角商品交易所", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                if (r == System.Windows.Forms.DialogResult.OK)
                {
                    Application.ExitThread();

                }


            }


        }





		public LoginForm(string[] args, IPlugin plugin, bool isClient)
		{
			this.Args = args;
			this.InitializeComponent();
			if (plugin != null)
			{
				this.isHideLoginModule = true;
				this.currentLoginPlugin = plugin;
			}
			this.isClientCreated = isClient;
			if (this.isClientCreated)
			{
				this.ishideNoLogin = true;
			}
		}
		private void SetFouce(short flag)
		{
			switch (flag)
			{
			case 0:
				this.txtUserName.Focus();
				return;
			case 1:
				this.password.Focus();
				return;
			case 2:
				this.txtVerifyCode.Focus();
				return;
			case 3:
				this.txtVerifyCode.Text = "";
				this.txtVerifyCode.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
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
			this.btnLogon.Left -= 60;
			this.btSet.Left -= 40;
			this.buttonNoLogin.Hide();
		}
		private void HideLoginModule()
		{
			this.labelLogin.Visible = false;
			this.comboBoxLogin.Visible = false;
			this.labUsername.Top -= this.top;
			this.txtUserName.Top -= this.top;
			this.userMemory.Top -= this.top;
			this.lblPassword.Top -= this.top - 5;
			this.password.Top -= this.top - 5;
			this.picSoftKey.Top -= this.top - 5;
			this.cboxUserProtect.Top -= this.top - 5;
			this.lbVerifyCode.Top -= this.top - 10;
			this.txtVerifyCode.Top -= this.top - 10;
			this.picVerifyCode.Top -= this.top - 10;
		}

		private void picVerifyCode_Click(object sender, EventArgs e)
		{
			OperationManager.GetInstance().verifyCodeOparation.updatePic();
			this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
		}
		private void LoginForm_Load(object sender, EventArgs e)
		{
			this.ReadUserName();
			this.SetControlText();
			this.RegistMethod();
            if (this.cboxUserProtect.Checked == true&& this.userMemory.Checked==true)
            {

                this.password.Text = Properties.Settings.Default.userPwd;

            }
            if (this.userMemory.Checked==false)
            {
                this.cboxUserProtect.Checked = false;
                this.password.Text = null;
            }
            
			string pluginNameOrPath = "MixAccount";
			try
			{
				OperationManager.GetInstance().mixAccountPlugin = (MixAccountPlugin)Global.Modules.Plugins.AvailablePlugins.Find(pluginNameOrPath).Instance;
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
			foreach (DictionaryEntry dictionaryEntry in Global.Modules.Plugins.LoginPluginsHashtable)
			{
				if (dictionaryEntry.Key != null)
				{
					this.comboBoxLogin.Items.Add(dictionaryEntry.Key);
				}
				if (this.currentLoginPlugin != null && dictionaryEntry.Value.ToString() == this.currentLoginPlugin.Name)
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
            OperationManager.GetInstance().loginOperation.showClient = new LoginOperation.ShowClient(this.ShowClient);
			OperationManager.GetInstance().updateOperation.StartCheckUpdate(this);
			OperationManager.GetInstance().loginOperation.ShowCommitForm = new LoginOperation.ShowCommitFormCallBack(this.DisPlayCommitForm);
			OperationManager.GetInstance().loginOperation.ShowBalanceForm = new LoginOperation.ShowBalanceFormCallBack(this.DisPlayBalance);
		}



        
        
        //登录的ip
		private void btnLogon_Click(object sender, EventArgs e)
		{

			LogonOperationInfo logonOperationInfo = new LogonOperationInfo();
			logonOperationInfo.username = this.txtUserName.Text;
			this.password.CheckPass = true;
			logonOperationInfo.password = this.password.Text;
            //MessageBox.Show(this.password.Text);
            if (this.cboxUserProtect.Checked == true && this.userMemory.Checked==true)
            {

                Properties.Settings.Default.userPwd = this.password.Text;
                Properties.Settings.Default.Save();

            }
			this.password.CheckPass = false;
			logonOperationInfo.loginmark = (short)this.LoginMark;
			logonOperationInfo.isMemoryChecked = this.userMemory.Checked;
			logonOperationInfo.isProtectChecked = this.cboxUserProtect.Checked;
			logonOperationInfo.verifycode = this.txtVerifyCode.Text;
			logonOperationInfo.verifyCodeString = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeString;
            
			logonOperationInfo.myPlugin = this.currentLoginPlugin;
            OperationManager.GetInstance().stripButtonOperation.curLoginPluginName = this.currentLoginPlugin.Name;
            
            
            OperationManager.GetInstance().verifyCodeOparation.VerifyUserInfo(logonOperationInfo, base.Handle);
            //MessageBox.Show(base.Handle.ToString());
            
            
            
		}
		private void btnExit_Click(object sender, EventArgs e)
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
            //Image image = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_TradeLoginBG");
            Image image = TimeImage.loginBG1;
            if (image != null)
			{
				Bitmap bitmap = new Bitmap((Bitmap)image);
				BitmapRegion.CreateControlRegion(this, bitmap);
			}
			this.Text = string.Format("{0}", (string)Global.htConfig["Title"]);
			this.labTitle.Text = (string)Global.htConfig["CompanyName"];
			this.labUsername.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LAB_USERNAME");
			this.lblPassword.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LAB_PASSWORD");
			this.btnLogon.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_BUTTON_LOGIN");
			this.buttonNoLogin.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_BUTTON_NOLOGIN");
			this.btSet.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_btSet");
			this.lbVerifyCode.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_lbVerifyCode");
			this.userMemory.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_userMemory");
            this.cboxUserProtect.Text = "记住密码";
			this.btnLogon.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_btn");
			this.buttonNoLogin.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_TestSpeed");
			this.btSet.BackgroundImage = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_TestSpeed");
			this.pictureBoxClose.Image = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_Close");
			this.picSoftKey.Image = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_defalutKeyBoard");
			base.Icon = Global.Modules.Plugins.SystemIcon;
			this.txtUserName.MaxLength = Tools.ObjToInt(Global.htConfig["UsernameMaxLength"]);
			this.loadingCircle1.Visible = false;
			this.txtVerifyCode.MaxLength = OperationManager.GetInstance().verifyCodeOparation.Codelength;
			this.labelLogin.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_loginModule");
			this.comboBoxLogin.Items.Clear();
			this.comboBoxLogin.Text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_moduleLoading");
			this.isEnableUserNameLogin = Tools.StrToBool((string)Global.htConfig["IsEnableUserNamLogin"], false);
			this.HideControl();
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
			this.txtUserName.Enabled = enable;
			this.password.Enabled = enable;
			this.txtVerifyCode.Enabled = enable;
			this.userMemory.Enabled = enable;
			this.cboxUserProtect.Enabled = enable;
			this.picVerifyCode.Enabled = enable;
			this.btnLogon.Enabled = enable;
			this.picSoftKey.Enabled = enable;
			this.buttonNoLogin.Enabled = enable;
			this.loadingCircle1.Visible = !enable;
			this.loadingCircle1.Active = !enable;
			this.pictureBoxClose.Visible = enable;
			string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_PasswordError");
			string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_NoExistTradeCode");
			Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_LoginFail");
			if (info.Equals(@string))
			{
				this.password.Text = "";
				this.txtVerifyCode.Text = "";
				this.password.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
				return;
			}
			if (info.Equals(string2))
			{
				this.txtUserName.Text = "";
				this.txtVerifyCode.Text = "";
				this.txtUserName.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
				return;
			}
			if (enable)
			{
				this.txtVerifyCode.Text = "";
				this.txtVerifyCode.Focus();
				OperationManager.GetInstance().verifyCodeOparation.updatePic();
				this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
			}
		}
		private void userProtect_CheckedChanged(object sender, EventArgs e)
		{
            //if (this.cboxUserProtect.Checked==true)
            //{

            //    Properties.Settings.Default.userPwd = this.password.Text;
            //    MessageBox.Show( this.password.Text);
            //        Properties.Settings.Default.Save();
              
            //}
			
			this.userMemory_Click(sender, e);
		}
		private void userMemory_Click(object sender, EventArgs e)
		{
			if (this.txtUserName.Text != "")
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
				this.txtUserName.Text = reCordUserInfo.username;
				this.userMemory.Checked = reCordUserInfo.isMemoryChecked;
				this.cboxUserProtect.Checked = reCordUserInfo.isProtectChecked;
			}
		}
		private void RecordUserName()
		{
			if (this.recordUserName == null)
			{
				this.recordUserName = new RecordUserName();
			}
			ReCordUserInfo reCordUserInfo = new ReCordUserInfo();
			reCordUserInfo.username = this.txtUserName.Text;
			reCordUserInfo.isMemoryChecked = this.userMemory.Checked;
			reCordUserInfo.isProtectChecked = this.cboxUserProtect.Checked;
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
		private void txtVerifyCode_KeyPress(object sender, KeyPressEventArgs e)
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
			string pluginNameOrPath = "ServerSet";
			try
			{
				IPlugin instance = Global.Modules.Plugins.AvailablePlugins.Find(pluginNameOrPath).Instance;
				string empty = string.Empty;
				Form form = instance.GetForm(false, ref empty);
     
				if (form.ShowDialog() == DialogResult.OK)
				{
					string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_serverSet");
					string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_serverSetContext");
					if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
					{
						Global.Modules.Plugins.ClosePlugins();
						Global.Modules.LoadPlugins();
						Global.ModuleInfos.Clear();
						foreach (ModuleInfo current in Global.Modules.Modules)
						{
							Global.ModuleInfos.Add(current.ModuleNo, current.ModuleName);
						}
					}
				}
               
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}


      public   Client client;
       


		private void buttonNoLogin_Click(object sender, EventArgs e)
		{
			if (this.Args != null)
			{
				 client = new Client(this.Args);
                 client.Show();
                 
            }
                base.Hide();
		}





        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!OperationManager.GetInstance().updateOperation.updateColse && !this.isClientCreated)
			{
				TimerForm timerForm = new TimerForm();
				timerForm.ShowDialog();
				if (timerForm.Result)
				{
					Global.Modules.Plugins.ClosePlugins();
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
			this.pictureBoxClose.Image = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_Close1");
		}
		private void pictureBoxClose_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBoxClose.Image = (Image)Global.Modules.Plugins.MEBS_ResourceManager.GetObject("TradeImg_dl_Close");
		}
		private void comboBoxLogin_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxLogin.Visible)
			{
				this.currentLoginPlugin = Global.Modules.Plugins.AvailablePlugins.Find(Global.Modules.Plugins.LoginPluginsHashtable[this.comboBoxLogin.Text].ToString()).Instance;
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
			this.picVerifyCode.Image = OperationManager.GetInstance().verifyCodeOparation.verifyCodeInfo.verifyCodeImage;
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

		public void ShowClient()
		{
			try
			{
                OperationManager.GetInstance().isLogin = true;
				this.showClientCallBack = new LoginForm.ShowClientCallBack(this.CreateClient);
				this.HandleCreated();
				base.Invoke(this.showClientCallBack);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "创建框架异常：" + ex.Message);
			}
		}
		public void CreateClient()
		{
			OperationManager.GetInstance().stripButtonOperation.SetIsLogin(true, this.currentLoginPlugin.Name);
			if (!this.isClientCreated)
			{
				ProgramOperation.CreateYrdceClient(this.Args);
				base.Hide();
				return;
			}
			base.Close();
		}
		public void comboBoxLoginRefresh(string name)
		{
		}




        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = 0;

            progressBarLogin.Value = progressBarLogin.Value + 1;
            i = progressBarLogin.Value;
            labelNumber.Text = i.ToString()+"%";
            if (i == 99)
            {
                timer1.Enabled = false;
            }
        }

        private void change()
        {
            for (int i = 1; i <= 99; i++)
            {
                progressBarLogin.PerformStep();
                Application.DoEvents();//让系统在百忙之中来响应其他事件
            
            }
        }
	}
}
