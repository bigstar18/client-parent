using PluginInterface;
using SoftKeyboard;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary;
using YrdceClient.Yrdce.Common.Library;
using System.ComponentModel;
namespace YrdceClient.UI.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
       // private System.ComponentModel.IContainer components = null;
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
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        

        #region Windows Form Designer generated code
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboxUserProtect = new System.Windows.Forms.CheckBox();
            this.userMemory = new System.Windows.Forms.CheckBox();
            this.labTitle = new System.Windows.Forms.Label();
            this.labUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnLogon = new System.Windows.Forms.Button();
            this.picVerifyCode = new System.Windows.Forms.PictureBox();
            this.txtVerifyCode = new System.Windows.Forms.TextBox();
            this.lbVerifyCode = new System.Windows.Forms.Label();
            this.picSoftKey = new System.Windows.Forms.PictureBox();
            this.buttonNoLogin = new System.Windows.Forms.Button();
            this.btSet = new System.Windows.Forms.Button();
            this.pictureBoxClose = new System.Windows.Forms.PictureBox();
            this.labelLogin = new System.Windows.Forms.Label();
            this.comboBoxLogin = new System.Windows.Forms.ComboBox();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.loadingCircle1 = new ToolsLibrary.LoadingCircle();
            this.password = new ToolsLibrary.PasswordTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picVerifyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSoftKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).BeginInit();
            this.panelLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboxUserProtect
            // 
            this.cboxUserProtect.BackColor = System.Drawing.Color.Transparent;
            this.cboxUserProtect.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cboxUserProtect.ForeColor = System.Drawing.Color.Black;
            this.cboxUserProtect.Location = new System.Drawing.Point(480, 120);
            this.cboxUserProtect.Name = "cboxUserProtect";
            this.cboxUserProtect.Size = new System.Drawing.Size(104, 24);
            this.cboxUserProtect.TabIndex = 11;
            this.cboxUserProtect.UseVisualStyleBackColor = false;
            this.cboxUserProtect.CheckedChanged += new System.EventHandler(this.userProtect_CheckedChanged);
            // 
            // userMemory
            // 
            this.userMemory.BackColor = System.Drawing.Color.Transparent;
            this.userMemory.Checked = true;
            this.userMemory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.userMemory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.userMemory.ForeColor = System.Drawing.Color.Black;
            this.userMemory.Location = new System.Drawing.Point(480, 176);
            this.userMemory.Name = "userMemory";
            this.userMemory.Size = new System.Drawing.Size(104, 24);
            this.userMemory.TabIndex = 7;
            this.userMemory.UseVisualStyleBackColor = false;
            this.userMemory.Click += new System.EventHandler(this.userMemory_Click);
            // 
            // labTitle
            // 
            this.labTitle.BackColor = System.Drawing.Color.Transparent;
            this.labTitle.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labTitle.ForeColor = System.Drawing.Color.Black;
            this.labTitle.Location = new System.Drawing.Point(90, 56);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(100, 23);
            this.labTitle.TabIndex = 3;
            // 
            // labUsername
            // 
            this.labUsername.BackColor = System.Drawing.Color.Transparent;
            this.labUsername.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(178)))), ((int)(((byte)(23)))));
            this.labUsername.Location = new System.Drawing.Point(90, 114);
            this.labUsername.Name = "labUsername";
            this.labUsername.Size = new System.Drawing.Size(100, 23);
            this.labUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(178)))), ((int)(((byte)(23)))));
            this.lblPassword.Location = new System.Drawing.Point(90, 176);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.TabIndex = 8;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Location = new System.Drawing.Point(221, 111);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(180, 28);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Tag = "false";
            this.txtUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.username_KeyPress);
            // 
            // btnLogon
            // 
            this.btnLogon.BackColor = System.Drawing.Color.Transparent;
            this.btnLogon.FlatAppearance.BorderSize = 0;
            this.btnLogon.ForeColor = System.Drawing.Color.White;
            this.btnLogon.Location = new System.Drawing.Point(299, 312);
            this.btnLogon.Name = "btnLogon";
            this.btnLogon.Size = new System.Drawing.Size(75, 23);
            this.btnLogon.TabIndex = 14;
            this.btnLogon.UseVisualStyleBackColor = false;
            this.btnLogon.Click += new System.EventHandler(this.btnLogon_Click);
            // 
            // picVerifyCode
            // 
            this.picVerifyCode.Location = new System.Drawing.Point(506, 14);
            this.picVerifyCode.Name = "picVerifyCode";
            this.picVerifyCode.Size = new System.Drawing.Size(100, 50);
            this.picVerifyCode.TabIndex = 12;
            this.picVerifyCode.TabStop = false;
            this.picVerifyCode.Click += new System.EventHandler(this.picVerifyCode_Click);
            // 
            // txtVerifyCode
            // 
            this.txtVerifyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVerifyCode.Location = new System.Drawing.Point(221, 242);
            this.txtVerifyCode.Name = "txtVerifyCode";
            this.txtVerifyCode.Size = new System.Drawing.Size(180, 28);
            this.txtVerifyCode.TabIndex = 13;
            this.txtVerifyCode.Tag = "false";
            this.txtVerifyCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVerifyCode_KeyPress);
            // 
            // lbVerifyCode
            // 
            this.lbVerifyCode.BackColor = System.Drawing.Color.Transparent;
            this.lbVerifyCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbVerifyCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(178)))), ((int)(((byte)(23)))));
            this.lbVerifyCode.Location = new System.Drawing.Point(90, 242);
            this.lbVerifyCode.Name = "lbVerifyCode";
            this.lbVerifyCode.Size = new System.Drawing.Size(100, 23);
            this.lbVerifyCode.TabIndex = 9;
            // 
            // picSoftKey
            // 
            this.picSoftKey.BackColor = System.Drawing.Color.Transparent;
            this.picSoftKey.Location = new System.Drawing.Point(327, 160);
            this.picSoftKey.Name = "picSoftKey";
            this.picSoftKey.Size = new System.Drawing.Size(100, 50);
            this.picSoftKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picSoftKey.TabIndex = 0;
            this.picSoftKey.TabStop = false;
            this.picSoftKey.Click += new System.EventHandler(this.picSoftKey_Click);
            this.picSoftKey.MouseHover += new System.EventHandler(this.picSoftKey_MouseHover);
            // 
            // buttonNoLogin
            // 
            this.buttonNoLogin.BackColor = System.Drawing.Color.Transparent;
            this.buttonNoLogin.FlatAppearance.BorderSize = 0;
            this.buttonNoLogin.ForeColor = System.Drawing.Color.White;
            this.buttonNoLogin.Location = new System.Drawing.Point(124, 312);
            this.buttonNoLogin.Name = "buttonNoLogin";
            this.buttonNoLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonNoLogin.TabIndex = 10;
            this.buttonNoLogin.UseVisualStyleBackColor = false;
            this.buttonNoLogin.Click += new System.EventHandler(this.buttonNoLogin_Click);
            // 
            // btSet
            // 
            this.btSet.BackColor = System.Drawing.Color.Transparent;
            this.btSet.FlatAppearance.BorderSize = 0;
            this.btSet.ForeColor = System.Drawing.Color.White;
            this.btSet.Location = new System.Drawing.Point(480, 312);
            this.btSet.Name = "btSet";
            this.btSet.Size = new System.Drawing.Size(75, 23);
            this.btSet.TabIndex = 4;
            this.btSet.UseVisualStyleBackColor = false;
            this.btSet.Click += new System.EventHandler(this.labelSet_Click);
            // 
            // pictureBoxClose
            // 
            this.pictureBoxClose.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxClose.Location = new System.Drawing.Point(407, 232);
            this.pictureBoxClose.Name = "pictureBoxClose";
            this.pictureBoxClose.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxClose.TabIndex = 1;
            this.pictureBoxClose.TabStop = false;
            this.pictureBoxClose.Click += new System.EventHandler(this.btnExit_Click);
            this.pictureBoxClose.MouseEnter += new System.EventHandler(this.pictureBoxClose_MouseEnter);
            this.pictureBoxClose.MouseLeave += new System.EventHandler(this.pictureBoxClose_MouseLeave);
            // 
            // labelLogin
            // 
            this.labelLogin.BackColor = System.Drawing.Color.Transparent;
            this.labelLogin.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(178)))), ((int)(((byte)(23)))));
            this.labelLogin.Location = new System.Drawing.Point(289, 18);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(100, 23);
            this.labelLogin.TabIndex = 5;
            // 
            // comboBoxLogin
            // 
            this.comboBoxLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxLogin.FormattingEnabled = true;
            this.comboBoxLogin.Location = new System.Drawing.Point(221, 53);
            this.comboBoxLogin.Name = "comboBoxLogin";
            this.comboBoxLogin.Size = new System.Drawing.Size(180, 26);
            this.comboBoxLogin.TabIndex = 2;
            this.comboBoxLogin.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogin_SelectedIndexChanged);
            this.comboBoxLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxLogin_KeyPress);
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = System.Drawing.Color.Transparent;
            this.panelLogin.Controls.Add(this.lblPassword);
            this.panelLogin.Controls.Add(this.labTitle);
            this.panelLogin.Controls.Add(this.comboBoxLogin);
            this.panelLogin.Controls.Add(this.lbVerifyCode);
            this.panelLogin.Controls.Add(this.labUsername);
            this.panelLogin.Controls.Add(this.userMemory);
            this.panelLogin.Controls.Add(this.buttonNoLogin);
            this.panelLogin.Controls.Add(this.btSet);
            this.panelLogin.Controls.Add(this.cboxUserProtect);
            this.panelLogin.Controls.Add(this.labelLogin);
            this.panelLogin.Controls.Add(this.txtVerifyCode);
            this.panelLogin.Controls.Add(this.picSoftKey);
            this.panelLogin.Controls.Add(this.loadingCircle1);
            this.panelLogin.Controls.Add(this.picVerifyCode);
            this.panelLogin.Controls.Add(this.password);
            this.panelLogin.Controls.Add(this.txtUserName);
            this.panelLogin.Controls.Add(this.btnLogon);
            this.panelLogin.Controls.Add(this.pictureBoxClose);
            this.panelLogin.Location = new System.Drawing.Point(49, 26);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(632, 374);
            this.panelLogin.TabIndex = 0;
            this.panelLogin.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLogin_Paint);
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Active = false;
            this.loadingCircle1.BackColor = System.Drawing.Color.Transparent;
            this.loadingCircle1.Color = System.Drawing.Color.DarkOrange;
            this.loadingCircle1.InnerCircleRadius = 5;
            this.loadingCircle1.Location = new System.Drawing.Point(545, 9);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.NumberSpoke = 12;
            this.loadingCircle1.OuterCircleRadius = 11;
            this.loadingCircle1.RotationSpeed = 100;
            this.loadingCircle1.Size = new System.Drawing.Size(27, 65);
            this.loadingCircle1.SpokeThickness = 2;
            this.loadingCircle1.StylePreset = ToolsLibrary.LoadingCircle.StylePresets.MacOSX;
            this.loadingCircle1.TabIndex = 2;
            // 
            // password
            // 
            this.password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.password.CheckPass = false;
            this.password.Location = new System.Drawing.Point(221, 172);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(180, 28);
            this.password.TabIndex = 6;
            this.password.Tag = "false";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogon;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(710, 469);
            this.Controls.Add(this.panelLogin);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.picVerifyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSoftKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).EndInit();
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}