using ModulesLoader;
using PluginInterface;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation.Manager;
using YrdceClient.UI.Forms.PromptForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using YrdceClient.UI.Forms;
using ToolsLibrary.util;
namespace YrdceClient.Yrdce.Common.Operation.MainOperation
{
	public class StripButtonOperation
	{
		public delegate void RefreshPanelCallBack(PanelLoad panelLoad, FormandPlugin temp);
		public delegate void CreateLoginForm(IPlugin plugin);
		public delegate void LoadClickFormCallBack();
		public StripButtonOperation.RefreshPanelCallBack RefreshPanel;
		public StripButtonOperation.CreateLoginForm createLoginForm;
		private StripButtonOperation.LoadClickFormCallBack loadClickFormCallBack;
    
		private string Nodes = "nodes.xml";
		public bool isReloadPlugin;
		private bool isLogin;
		private bool isAgencyLogin;
		public string curLoginPluginName = string.Empty;
		private List<IPlugin> pluginsList;
		private ToolStrip ToolStripButtons;
		private ToolStripItem curToolStripItem;
		private ThreadStartAddNodes s;
		public Image menuClickBackImage;
		public List<FormandPlugin> FormandPluginList = new List<FormandPlugin>();

        /// <summary>
        /// web页面链接
        /// </summary>
        private string webUrl;
	    public string WebUrl
	    {
		    get { return webUrl;}
		    set { webUrl = value;}
	    }
        private string[] myUrl;

        public string[] MyUrl
        {
            get { return myUrl; }
            set { myUrl = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toolStripButtons"></param>
        private string homeUrl;

        public string HomeUrl
        {
            get { return homeUrl; }
            set { homeUrl = value; }
        }

        private int myPerformStep;

        public int MyPerformStep
        {
            get { return myPerformStep; }
            set { myPerformStep = value; }
        }

        


		public void StripButtonNodeLoad(ToolStrip toolStripButtons)
		{
			this.ToolStripButtons = toolStripButtons;
			this.s = new ThreadStartAddNodes(toolStripButtons.Items, this.Nodes);
			this.s.ToolStripButtonClick = new ThreadStartAddNodes.ToolsStripButtonClickCallBack(this.ToolStripButtonClickForm);
			new Thread(new ParameterizedThreadStart(this.s.LoadNode)).Start(this.isLogin);
		}
		private void ToolStripButtonClickForm()
		{
			this.loadClickFormCallBack = new StripButtonOperation.LoadClickFormCallBack(this.LoadCheckForm);
			this.ToolStripButtons.Invoke(this.loadClickFormCallBack);
		}
		private void LoadCheckForm()
		{
			this.ShowLoginedNodes();
			ToolStripItem toolStripItem = this.ToolStripButtons.Items[this.curLoginPluginName];
			if (toolStripItem != null)
			{
				this.menuClickBackImage = Image.FromFile("images\\menuclick.png");
				((ToolStripLabel)toolStripItem).ToolTipText = "true";
				toolStripItem.BackgroundImage = this.menuClickBackImage;
				ToolStripItemClickedEventArgs e = new ToolStripItemClickedEventArgs(toolStripItem);
				this.isReloadPlugin = true;
				this.s_ToolStripButtonClick(e);
			}
		}
        public void s_ToolStripButtonClick(ToolStripItemClickedEventArgs e)
		{
            
			if (e == null || e.ClickedItem == null )
			{
				return;
			}
			List<IPlugin> list = (List<IPlugin>)e.ClickedItem.Tag;
           
			if (list != null)
			{
				this.pluginsList = list;
			}
            if (e.ClickedItem.Name == "login" || (e.ClickedItem.Name == "OTC_AgencyTrade" && !this.isAgencyLogin))
            {
                if (this.pluginsList != null)
                {
                    if (this.createLoginForm != null)
                    {
                        if (this.pluginsList.Count > 1)
                        {
                            this.createLoginForm(this.pluginsList[1]);
                        }
                        if (this.pluginsList.Count == 1)
                        {
                            this.createLoginForm(this.pluginsList[0]);
                            return;
                        }
                    }
                }
                if (this.createLoginForm != null)
                {
                    this.createLoginForm(null);
                    return;
                }
            }
            else
            {
                if (e.ClickedItem.Name.Equals("ChangePW"))
                {

                    return;
                }
                if (e.ClickedItem.Name.Equals("logout"))
                {
                    string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_canceled");
                    string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_canceledContext");
                    if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                    {
                        this.Logout();
                        return;
                    }
                }
                else
                {
                    this.FormandPluginList.Clear();
                    string empty = string.Empty;
                    for (int i = 0; i < this.pluginsList.Count; i++)
                    {
                        if (this.curToolStripItem == null || this.curToolStripItem != e.ClickedItem || this.isReloadPlugin)
                        {
                            IPlugin arg_19A_0 = this.pluginsList[i];
                            this.displayPluginForm(this.pluginsList[i], ref empty);
                        }
                    }
                    //
                    if (!this.isLogin || this.FormandPluginList.Count == this.pluginsList.Count)
                    {
                        
                        //this.showPanel(e.ClickedItem.Name);
                        if (this.FormandPluginList.Count <= 0)
                        {
                            return;
                        }
                        if (this.FormandPluginList[0].plugin.DisplayType != DisplayTypes.IEDialog)
                        {
                            this.RefreshPanel(PanelLoad.RemoveHQTrade, null);
                            myPerformStep = 100;
                        }
                        using (List<FormandPlugin>.Enumerator enumerator = this.FormandPluginList.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                FormandPlugin current = enumerator.Current;
                                if (current.plugin.Description == "连续现货行情系统插件")
                                {
                                    hqIPaddress = current.plugin.IpAddress + "[" + current.plugin.Port + "]";
                                }
                                if (current.plugin.Description == "远期交易系统插件")
                                {
                                    tradeIPaddress = current.plugin.IpAddress + "[" + current.plugin.Port + "]";
                                    
                                }
                                if (current.plugin.DisplayType == DisplayTypes.Normal)
                                {
                                    if (this.RefreshPanel != null)
                                    {
                                        this.RefreshPanel(PanelLoad.HQPanelLoad, current);
                                    }
                                }
                                else if (current.plugin.DisplayType == DisplayTypes.panel2)
                                {
                                    if (this.RefreshPanel != null)
                                    {

                                        this.RefreshPanel(PanelLoad.TradePanelLoad, current);
                                        
                                        
                                    }
                                }
                                else if (current.plugin.DisplayType == DisplayTypes.IEDialog && current.form.Tag != null)
                                {
                                    Process.Start(current.form.Tag.ToString());
                                   // myURL = current.form.Tag.ToString();
                                    //MessageBox.Show(current.form.Tag.ToString());
                                }
                            }
                            goto IL_316;
                          
                        }
                    }
                    if (this.curToolStripItem != e.ClickedItem)
                    {
                        string string3 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_Error");
                        if (empty.Length > 0)
                        {
                            MessageBox.Show(empty, string3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        return;
                    }
                
                IL_316:
                    this.curLoginPluginName = e.ClickedItem.Name;
                    IPlugin plugin = this.pluginsList[0];
                    if (!plugin.Name.Contains("HQ"))
                    {
                        OperationManager.GetInstance().changePassWordOperation.myPlugin = plugin;
                    }
                    else if (this.pluginsList.Count > 1)
                    {
                        OperationManager.GetInstance().changePassWordOperation.myPlugin = this.pluginsList[1];
                    }
                    if (plugin.DisplayType == DisplayTypes.IEDialog)
                    {
                        SysShareInfo.FromLogonType = "web";
                    }
                    else
                    {
                        SysShareInfo.FromLogonType = "pc";
                    }
                    this.curToolStripItem = e.ClickedItem;
                }
            
			}
		}

        public void button_Click(string name)
        {
            //Button tempBtn = (Button)sender;
            List<IPlugin> list = (List<IPlugin>)this.ToolStripButtons.Items[name].Tag;
            if (list != null)
            {
                this.pluginsList = list;
            }
            this.FormandPluginList.Clear();
            if (name.Equals("ChangePW"))
            {
                this.ChangePassWord();
                return;
            }
            if (name.Equals("logout"))
            {
                string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_canceled");
                string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_canceledContext");
                if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    this.Logout();
                    
                    return;
                }
            }else{
                List<IPlugin> myListPlugin = (List<IPlugin>)this.ToolStripButtons.Items[name].Tag;
               
                if (myListPlugin.Count == 0)
                {
                    return;
                }
                string empty = string.Empty;
                foreach (IPlugin item in myListPlugin)
                {
                    this.displayPluginForm(item, ref empty);//加载窗体
                }

                if (!this.isLogin || this.FormandPluginList.Count == this.pluginsList.Count)
                {
                    if (this.FormandPluginList.Count <= 0)
                    {
                        return;
                    }
                    if (this.FormandPluginList[0].plugin.DisplayType != DisplayTypes.IEDialog)
                    {
                        this.RefreshPanel(PanelLoad.RemoveHQTrade, null);
                    }
                    using (List<FormandPlugin>.Enumerator enumerator = this.FormandPluginList.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            FormandPlugin current = enumerator.Current;

                            //取地址
                            if (current.plugin.Description == "连续现货行情系统插件")
                            {
                                hqIPaddress = current.plugin.IpAddress + "[" + current.plugin.Port + "]";
                            }
                            if (current.plugin.Description == "客户平台插件")
                            {
                                counmerIPadress = current.plugin.IpAddress + "[" + current.plugin.Port + "]";
                            }
                            if (current.plugin.Description == "远期交易系统插件")
                            {
                              tradeIPaddress = current.plugin.IpAddress + "[" + current.plugin.Port + "]";
                            }
                            
                            if (current.plugin.DisplayType == DisplayTypes.Normal)
                            {
                                if (this.RefreshPanel != null)
                                {
                                    this.RefreshPanel(PanelLoad.HQPanelLoad, current);
                                }
                            }
                            else if (current.plugin.DisplayType == DisplayTypes.panel2)
                            {
                                if (this.RefreshPanel != null)
                                {
                                    this.RefreshPanel(PanelLoad.TradePanelLoad, current);
                                }
                            }
                            else if (current.plugin.DisplayType == DisplayTypes.IEDialog && current.form.Tag != null)
                            {
                                this.webUrl = current.form.Tag.ToString();
                                
                                 myUrl = webUrl.Split ('?');
                                 
                                 
                                SysShareInfo.FromLogonType = "pc";
                            }
                        }
                    }
                }
            }



        }

        private string counmerIPadress;

        public string CounmerIPadress
        {
            get { return counmerIPadress; }
            set { counmerIPadress = value; }
        }

        private string tradeIPaddress;

        public string TradeIPaddress
        {
            get { return tradeIPaddress; }
            set { tradeIPaddress = value; }
        }

        private string hqIPaddress;

        public string HQIPaddress
        {
            get { return hqIPaddress; }
            set { hqIPaddress = value; }
        }
        
       

        public void displayPluginForm(IPlugin plugin, ref string info)
		{
			if (plugin == null)
			{
				return;
			}
			Form form = null;
			try
			{
				form = plugin.GetForm(this.isLogin, ref info);
			}
			catch (Exception)
			{
				info = "窗体初始化失败！！！";
			}
			if (form != null)
			{
				FormandPlugin formandPlugin = new FormandPlugin();
				formandPlugin.form = form;
				formandPlugin.plugin = plugin;
                
				this.FormandPluginList.Add(formandPlugin);
			}
		}
		private void ChangePassWord()
		{
			ChangePW changePW = new ChangePW(false);
			changePW.ShowDialog();
		}
        //注销
        private void CreateLoginForm1(IPlugin plugin)
        {
            ProgramOperation.CreateLogin(plugin, true);

        }


       
		public void Logout()
		{

            
			if (this.RefreshPanel != null)
			{
				this.RefreshPanel(PanelLoad.RemoveHQTrade, null);
			}
			Global.Modules.Plugins.ClosePlugins();
			Global.Modules.LoadPlugins();
			Global.ModuleInfos.Clear();
			foreach (ModuleInfo current in Global.Modules.Modules)
			{
				Global.ModuleInfos.Add(current.ModuleNo, current.ModuleName);
			}
			this.isLogin = false;
			this.isAgencyLogin = false;
			Global.Modules.Plugins.SysLogonInfo.RegisterWord = string.Empty;
			Global.Modules.Plugins.SysLogonInfo.Password = string.Empty;
			Global.Modules.Plugins.SysLogonInfo.TraderID = string.Empty;
			Global.Modules.Plugins.SysLogonInfo.VersionInfo = string.Empty;
			Global.htConfig = Global.Modules.Plugins.ConfigurationInfo.getSection("Systems");
            
			OperationManager.GetInstance().YrdceClientOperation.isFloatTrade = false;
			OperationManager.GetInstance().YrdceClientOperation.isLockTrade = false;

			if (this.curLoginPluginName != "")
			{
				this.curLoginPluginName = "";
			}
			if (this.pluginsList != null)
			{
				this.pluginsList = null;
			}
			if (this.curToolStripItem != null)
			{
				this.curToolStripItem = null;
			}
			OperationManager.GetInstance().isLogin = false;
			OperationManager.GetInstance().PluginNameList.Clear();


            //Application.ExitThread();
            //Thread thtmp = new Thread(new ParameterizedThreadStart(run));
            //object appName = Application.ExecutablePath;
            //Thread.Sleep(1);
            //thtmp.Start(appName);



            if (ProgramOperation.myIsLoad == true)
            {
                //普通登录
                ProgramOperation.client.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogin;
                ProgramOperation.client.btnChangePWD.Visible = false;
                ProgramOperation.client.panelTopInfo.Location = new System.Drawing.Point(ProgramOperation.client.btnLoginAndLogoff.Left - ProgramOperation.client.panelTopInfo.Width - 5, 0);
                ProgramOperation.client.TopToRight.Location = new Point(ProgramOperation.client.btnLoginAndLogoff.Left - ProgramOperation.client.panelTopInfo.Width - 15, 0);
                ProgramOperation.client.tabControl1.SelectTab(0);

                ProgramOperation.client.tempButton.GlowColor = System.Drawing.Color.White;
                ProgramOperation.client.tempButton2.GlowColor = System.Drawing.Color.White;
                ProgramOperation.client.tempButton3.GlowColor = System.Drawing.Color.White;
                ProgramOperation.client.tempButton4.GlowColor = System.Drawing.Color.White;
                ProgramOperation.client.tempButtonQuotation.GlowColor = System.Drawing.Color.White;
                ProgramOperation.client.tempButton.Enabled = true;
                ProgramOperation.client.tempButton2.Enabled = true;
                ProgramOperation.client.tempButton3.Enabled = true;
                ProgramOperation.client.tempButton4.Enabled = true;
                ProgramOperation.client.tempButtonQuotation.Enabled = true;
                this.button_Click("MEBS_Trade");
            }
            if (ProgramOperation.myIsLoad == false)
            {
                //游客登录后登录
                ProgramOperation.loginForm2.client.btnLoginAndLogoff.BackgroundImage = TimeImage.picLogin;
                ProgramOperation.loginForm2.client.btnChangePWD.Visible = false;
               
                
                ProgramOperation.loginForm2.client.panelTopInfo.Location = new System.Drawing.Point(ProgramOperation.loginForm2.client.btnLoginAndLogoff.Left - ProgramOperation.loginForm2.client.panelTopInfo.Width - 5, 0);
                ProgramOperation.loginForm2.client.TopToRight.Location = new Point(ProgramOperation.loginForm2.client.btnLoginAndLogoff.Left - ProgramOperation.loginForm2.client.panelTopInfo.Width - 15, 0);
                ProgramOperation.loginForm2.client.tabControl1.SelectTab(0);

                ProgramOperation.loginForm2.client.tempButton.GlowColor = System.Drawing.Color.White;
                ProgramOperation.loginForm2.client.tempButton2.GlowColor = System.Drawing.Color.White;
                ProgramOperation.loginForm2.client.tempButton3.GlowColor = System.Drawing.Color.White;
                ProgramOperation.loginForm2.client.tempButton4.GlowColor = System.Drawing.Color.White;
                ProgramOperation.loginForm2.client.tempButtonQuotation.GlowColor = System.Drawing.Color.White;
                ProgramOperation.loginForm2.client.tempButton.Enabled = true;
                ProgramOperation.loginForm2.client.tempButton2.Enabled = true;
                ProgramOperation.loginForm2.client.tempButton3.Enabled = true;
                ProgramOperation.loginForm2.client.tempButton4.Enabled = true;
                ProgramOperation.loginForm2.client.tempButtonQuotation.Enabled = true;
                this.button_Click("MEBS_Trade");
            }
            ////this.panelTopInfo.Location =  new System.Drawing.Point(1020, 0);
            //ProgramOperation.loginForm.client.panelTopInfo.Location = new System.Drawing.Point(ProgramOperation.loginForm.client.btnLoginAndLogoff.Left - ProgramOperation.loginForm.client.panelTopInfo.Width - 5, 0);
			this.StripButtonNodeLoad(this.ToolStripButtons);



           
		}
        private void run(Object obj)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }
       
		public void SetIsLogin(bool _isLogin, string _curLoginPluginName)
		{
			if (this.curLoginPluginName == "OTC_AgencyTrade")
			{
				this.isAgencyLogin = true;
			}
			else
			{
				this.isLogin = _isLogin;
			}
			if (this.s != null)
			{
				this.ToolStripButtonClickForm();
				if (!this.isAgencyLogin)
				{
					this.s.addNodeChild(null, this.ToolStripButtons.Items);
				}
				this.RemoveLoginButton();
			}
		}
		private void RemoveLoginButton()
		{
			ToolStripItem value = this.ToolStripButtons.Items["login"];
			this.ToolStripButtons.Items.Remove(value);
		}
		private void ShowLoginedNodes()
		{
			if (this.isLogin)
			{
				foreach (string current in this.s.NodeNameList)
				{
					ToolStripItem toolStripItem = this.ToolStripButtons.Items[current];
					if (current == "OTC_AgencyTrade")
					{
						if (Global.Modules.Plugins.IdentityType == "1")
						{
							toolStripItem.Visible = true;
						}
					}
					else
					{
						toolStripItem.Visible = true;
					}
				}
                
			}
		}

	}
}
