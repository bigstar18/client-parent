using AppUpdate;
using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator;
using Gnnt.MEBS.HQClient.Properties;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using SplitButtonDemo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient
{
	public class MainWindow : Form
	{
		private delegate void SetPictureEnableCallBack(bool enable);
		private IContainer components;
		private ToolTip toolTipTopImage;
		private ContextMenuStrip contextMenuHY;
		private ContextMenuStrip contextMenuDiQu;
		private Panel panelHQ;
		private PictureBox pictureBoxConnect;
		private Label lbTime;
		private Label lbConnect;
		private Panel topPanel;
		private Label labelSet;
		private Label lbLogin;
		private SplitButton btDiQu;
		private SplitButton btHangYe;
		private Button btSysMessage;
		private Label lbAbout;
		private Label label1;
		private Button btNews;
		private Button btOwnGoods;
		private Panel panelLeftBtn;
		private Button btRanking;
		private Button btMultiRanking;
		private Button btLine;
		private Button btBill;
		private Panel panelMain;
		private Panel imagePanel;
		private PictureBox pictureSet;
		private PictureBox pictureF10;
		private PictureBox pictureDown;
		private PictureBox pictureUp;
		private PictureBox Indecator;
		private PictureBox pictureBoxBill;
		private PictureBox refreshBt;
		private PictureBox AnyMin;
		private PictureBox BackBtn;
		private PictureBox AnyDay;
		private PictureBox FifteenMin;
		private PictureBox KLineBtn;
		private PictureBox FourHr;
		private PictureBox Day;
		private PictureBox SearchBtn;
		private PictureBox FiveMin;
		private PictureBox Quarter;
		private PictureBox ThirtyMin;
		private PictureBox StartButton;
		private PictureBox FSZSBtn;
		private PictureBox TwoHr;
		private PictureBox Week;
		private PictureBox OneMin;
		private PictureBox ThreeMin;
		private PictureBox Hour;
		private PictureBox Month;
		private PictureBox BJPMBtn;
		private PictureBox Split;
		public ButtonUtils buttonUtils;
		public PluginInfo pluginInfo;
		public SetInfo setInfo;
		public MultiQuoteData multiQuoteData;
		public HQClientForm mainForm;
		private Image backgroundImage;
		private Image backgroundImageleave;
		private SysMessage sys;
		private bool updateColse;
		private bool bUpdate;
		private Thread startCheckUpdate;
		public static List<string> diQuStrings = new List<string>();
		public static List<string> hangYeStrings = new List<string>();
		public static bool isLoadItemButtom = false;
		public static int selectIndexHY = -1;
		public static int selectIndexDQ = -1;
		private Color topBtMouseColor = Color.FromArgb(255, 255, 0);
		private Color topBtColor = Color.FromArgb(255, 138, 0);
		private About about;
		private int lbLocationY = 35;
		private int lbHeight = 25;
		private MainWindow.SetPictureEnableCallBack setEnable;
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
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1182, 423);
            this.Name = "MainWindow";
            this.ResumeLayout(false);

		}
		public MainWindow(PluginInfo _pluginInfo)
		{
			this.InitializeComponent();
			this.pluginInfo = _pluginInfo;
			this.setInfo = new SetInfo();
			this.setInfo.init(this.pluginInfo.ConfigPath);
			this.mainForm = new HQClientForm(this);
			base.Activated += new EventHandler(this.MainWindow_Activated);
			base.GotFocus += new EventHandler(this.MainWindow_GotFocus);

		}
		private void MainWindow_Load(object sender, EventArgs e)
		{
			try
			{
				this.BackColor = Color.Black;
				this.SetControl(true);
				this.SetControlText();
				this.iniSizeAndStyle();
				this.SetPicture();
				this.SetPictureEnable(false);
				if (this.mainForm != null)
				{
					this.mainForm.Size = this.panelHQ.Size;
					this.mainForm.TopLevel = false;
					this.mainForm.Parent = this.panelHQ;
					this.mainForm.Dock = DockStyle.Fill;
					this.mainForm.Show();
				}
				else
				{
					Logger.wirte(MsgType.Information, "设置窗体停靠时mainForm = null");
				}
				if (this.mainForm.CurHQClient != null)
				{
					this.mainForm.CurHQClient.setPictureEnable = new HQClientMain.SetPictureEnableCallback(this.SetPictureEnable);
				}
				this.buttonUtils = this.mainForm.buttonUtils;
				this.multiQuoteData = this.mainForm.multiQuoteData;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "MainWindow_Load异常：" + ex.Message);
			}
		}
		private void SetPicture()
		{
			this.Indecator.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Indicator");
			this.pictureUp.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_ZoomIn");
			this.pictureDown.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_ZoomOut");
			this.pictureF10.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_F10Btn");
			this.pictureSet.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_UserSet");
			this.BackBtn.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_back");
			this.StartButton.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_start");
			this.refreshBt.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_refresh");
			this.BJPMBtn.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_bjpm");
			this.FSZSBtn.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_fszs");
			this.KLineBtn.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine");
			this.pictureBoxBill.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_Bill");
			this.Day.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_day");
			this.Week.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_week");
			this.Month.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_month");
			this.Quarter.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_quarter");
			this.AnyDay.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_anyday");
			this.Split.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_split");
			this.OneMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_onemin");
			this.ThreeMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_threemin");
			this.FiveMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_fivemin");
			this.FifteenMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_fifteenmin");
			this.ThirtyMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_thirtymin");
			this.Hour.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_hour");
			this.TwoHr.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_twohr");
			this.FourHr.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_fourhr");
			this.AnyMin.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_anymin");
			this.backgroundImage = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_select");
			this.backgroundImageleave = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_leave");
		}
		private void ShowMessage()
		{
			this.sys = new SysMessage(this);
			this.sys.ShowDialog();
		}
		private void MainWindow_Activated(object sender, EventArgs e)
		{
			this.mainForm.Focus();
		}
		private void MainWindow_GotFocus(object sender, EventArgs e)
		{
			this.mainForm.Focus();
		}
		private void StartCheckUpdate()
		{
			if (!this.bUpdate)
			{
				return;
			}
			this.startCheckUpdate = new Thread(new ParameterizedThreadStart(this.CheckUpdate));
			this.startCheckUpdate.Start();
		}
		private void CheckUpdate(object nul)
		{
			CheckForUpdate checkForUpdate = new CheckForUpdate("UpdateList.xml");
			if (checkForUpdate.StartCheck())
			{
				if (checkForUpdate.UpdateLevel == 1)
				{
					checkForUpdate.StartUpdate();
					this.updateColse = true;
					MethodInvoker method = new MethodInvoker(base.Close);
					base.BeginInvoke(method);
					return;
				}
				string @string = this.pluginInfo.HQResourceManager.GetString("TradeStr_LoginForm_UpdateTitle");
				string string2 = this.pluginInfo.HQResourceManager.GetString("TradeStr_LoginForm_UpdateContext");
				if (MessageBox.Show(string2, @string, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
				{
					checkForUpdate.StartUpdate();
					this.updateColse = true;
					MethodInvoker method2 = new MethodInvoker(base.Close);
					base.BeginInvoke(method2);
				}
			}
		}
		public void SetControl(bool isFristOnce = true)
		{
			if (!Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false) && isFristOnce)
			{
				this.topPanel.Visible = false;
				this.panelHQ.Top -= this.topPanel.Height;
				this.panelHQ.Height += this.topPanel.Height;
				return;
			}
			if ((this.contextMenuDiQu.Items.Count == 0 || this.contextMenuHY.Items.Count == 0) && this.mainForm.CurHQClient.m_codeList.Count != 0 && this.mainForm.CurHQClient.m_htRegion.Count != 0)
			{
				for (int i = 0; i < this.mainForm.CurHQClient.m_codeList.Count; i++)
				{
					CommodityInfo commodityInfo = (CommodityInfo)this.mainForm.CurHQClient.m_codeList[i];
					bool flag = true;
					bool flag2 = true;
					if (MainWindow.diQuStrings.Count == 0)
					{
						MainWindow.diQuStrings.Add(commodityInfo.region);
					}
					if (MainWindow.hangYeStrings.Count == 0)
					{
						MainWindow.hangYeStrings.Add(commodityInfo.industry);
					}
					for (int j = 0; j < MainWindow.diQuStrings.Count; j++)
					{
						if (MainWindow.diQuStrings[j] == commodityInfo.region)
						{
							flag = false;
						}
					}
					for (int k = 0; k < MainWindow.hangYeStrings.Count; k++)
					{
						if (MainWindow.hangYeStrings[k] == commodityInfo.industry)
						{
							flag2 = false;
						}
					}
					if (flag)
					{
						MainWindow.diQuStrings.Add(commodityInfo.region);
					}
					if (flag2)
					{
						MainWindow.hangYeStrings.Add(commodityInfo.industry);
					}
				}
				ToolStripMenuItem value = new ToolStripMenuItem("全部");
				ToolStripMenuItem value2 = new ToolStripMenuItem("全部");
				this.contextMenuHY.Items.Add(value2);
				this.contextMenuDiQu.Items.Add(value);
				for (int l = 0; l < MainWindow.diQuStrings.Count; l++)
				{
					string text = "";
					if (this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[l]] != null)
					{
						text = this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[l]].ToString();
					}
					else
					{
						if (MainWindow.diQuStrings[l] != null)
						{
							if (l != MainWindow.selectIndexDQ)
							{
								text = MainWindow.diQuStrings[l].ToString();
							}
							else
							{
								text = MainWindow.diQuStrings[l].ToString() + " √";
							}
						}
					}
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
					toolStripMenuItem.Tag = MainWindow.diQuStrings[l];
					this.contextMenuDiQu.Items.Add(toolStripMenuItem);
					this.contextMenuDiQu.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuDiQu_ItemClicked);
				}
				for (int m = 0; m < MainWindow.hangYeStrings.Count; m++)
				{
					string text2 = "";
					if (this.mainForm.CurHQClient.m_htIndustry.Count != 0)
					{
						text2 = this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[m]].ToString();
					}
					else
					{
						if (MainWindow.hangYeStrings[m] != null)
						{
							if (m != MainWindow.selectIndexHY)
							{
								text2 = MainWindow.hangYeStrings[m].ToString();
							}
							else
							{
								text2 = MainWindow.hangYeStrings[m].ToString() + " √";
							}
						}
					}
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(text2);
					toolStripMenuItem2.Tag = MainWindow.hangYeStrings[m];
					this.contextMenuHY.Items.Add(toolStripMenuItem2);
					this.contextMenuHY.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuHY_ItemClicked);
				}
				MainWindow.isLoadItemButtom = true;
			}
		}
		public void clearMenuItem()
		{
			if (this.buttonUtils.CurButtonName != "Select")
			{
				if (MainWindow.selectIndexDQ != -1)
				{
					foreach (ToolStripMenuItem toolStripMenuItem in this.contextMenuDiQu.Items)
					{
						if (toolStripMenuItem.Text.Contains(" √"))
						{
							toolStripMenuItem.Text = toolStripMenuItem.Text.Replace(" √", "");
							MainWindow.selectIndexDQ = -1;
							break;
						}
					}
				}
				if (MainWindow.selectIndexHY != -1)
				{
					foreach (ToolStripMenuItem toolStripMenuItem2 in this.contextMenuHY.Items)
					{
						if (toolStripMenuItem2.Text.Contains(" √"))
						{
							toolStripMenuItem2.Text = toolStripMenuItem2.Text.Replace(" √", "");
							MainWindow.selectIndexHY = -1;
							break;
						}
					}
				}
			}
		}
		private void contextMenuDiQu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				ToolStripItem clickedItem = e.ClickedItem;
				if (clickedItem != null)
				{
					if (clickedItem.Text != "全部")
					{
						for (int i = 0; i < MainWindow.diQuStrings.Count; i++)
						{
							if (clickedItem != this.contextMenuDiQu.Items[i + 1])
							{
								if (this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[i]] != null)
								{
									this.contextMenuDiQu.Items[i + 1].Text = this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[i]].ToString();
								}
							}
							else
							{
								if (MainWindow.diQuStrings[i] != null && this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[i]] != null)
								{
									this.contextMenuDiQu.Items[i + 1].Text = this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[i]].ToString() + " √";
								}
								MainWindow.selectIndexDQ = i;
							}
						}
					}
					else
					{
						if (MainWindow.selectIndexDQ != -1 && this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[MainWindow.selectIndexDQ]] != null)
						{
							this.contextMenuDiQu.Items[MainWindow.selectIndexDQ + 1].Text = this.mainForm.CurHQClient.m_htRegion[MainWindow.diQuStrings[MainWindow.selectIndexDQ]].ToString();
						}
						MainWindow.selectIndexDQ = -1;
					}
					this.selectCurQuoteList();
				}
				this.multiQuoteData.iStart = 0;
				this.multiQuoteData.yChange = 0;
				this.buttonUtils.CurButtonName = "Select";
				foreach (MyButton myButton in this.buttonUtils.ButtonList)
				{
					if (myButton.Selected)
					{
						myButton.Selected = false;
					}
				}
				if (this.buttonUtils.ButtonList.Count > 0)
				{
					MyButton myButton2 = (MyButton)this.buttonUtils.ButtonList[0];
					myButton2.Selected = true;
				}
				this.multiQuoteData.MultiQuotePage = 0;
				this.mainForm.UserCommand("60");
				this.mainForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "contextMenuDiQu_ItemClicked异常：" + ex.Message);
			}
		}
		private void contextMenuHY_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				ToolStripItem clickedItem = e.ClickedItem;
				if (clickedItem != null)
				{
					if (clickedItem.Text != "全部")
					{
						for (int i = 0; i < MainWindow.hangYeStrings.Count; i++)
						{
							if (clickedItem != this.contextMenuHY.Items[i + 1])
							{
								if (this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[i]] != null)
								{
									this.contextMenuHY.Items[i + 1].Text = this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[i]].ToString();
								}
							}
							else
							{
								if (MainWindow.hangYeStrings[i] != null && this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[i]] != null)
								{
									this.contextMenuHY.Items[i + 1].Text = this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[i]].ToString() + " √";
								}
								MainWindow.selectIndexHY = i;
							}
						}
					}
					else
					{
						if (MainWindow.selectIndexHY != -1 && this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[MainWindow.selectIndexHY]] != null)
						{
							this.contextMenuHY.Items[MainWindow.selectIndexHY + 1].Text = this.mainForm.CurHQClient.m_htIndustry[MainWindow.hangYeStrings[MainWindow.selectIndexHY]].ToString();
						}
						MainWindow.selectIndexHY = -1;
					}
					this.selectCurQuoteList();
				}
				this.multiQuoteData.iStart = 0;
				this.multiQuoteData.yChange = 0;
				this.buttonUtils.CurButtonName = "Select";
				foreach (MyButton myButton in this.buttonUtils.ButtonList)
				{
					if (myButton.Selected)
					{
						myButton.Selected = false;
					}
				}
				if (this.buttonUtils.ButtonList.Count > 0)
				{
					MyButton myButton2 = (MyButton)this.buttonUtils.ButtonList[0];
					myButton2.Selected = true;
				}
				this.multiQuoteData.MultiQuotePage = 0;
				this.mainForm.UserCommand("60");
				this.mainForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "contextMenuHY_ItemClicked异常：" + ex.Message);
			}
		}
		private void selectCurQuoteList()
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this.mainForm.CurHQClient.m_quoteList.Length; i++)
			{
				ProductDataVO productDataVO = this.mainForm.CurHQClient.m_quoteList[i];
				if (MainWindow.selectIndexHY != -1 && MainWindow.selectIndexDQ != -1)
				{
					if (MainWindow.hangYeStrings[MainWindow.selectIndexHY] == productDataVO.industry && MainWindow.diQuStrings[MainWindow.selectIndexDQ] == productDataVO.region)
					{
						arrayList.Add(productDataVO);
					}
				}
				else
				{
					if (MainWindow.selectIndexHY == -1 && MainWindow.selectIndexDQ == -1)
					{
						arrayList.Add(productDataVO);
					}
					else
					{
						if (MainWindow.selectIndexHY == -1)
						{
							if (MainWindow.diQuStrings[MainWindow.selectIndexDQ] == productDataVO.region)
							{
								arrayList.Add(productDataVO);
							}
						}
						else
						{
							if (MainWindow.selectIndexDQ == -1 && MainWindow.hangYeStrings[MainWindow.selectIndexHY] == productDataVO.industry)
							{
								arrayList.Add(productDataVO);
							}
						}
					}
				}
			}
			this.multiQuoteData.m_curQuoteList = (ProductDataVO[])arrayList.ToArray(typeof(ProductDataVO));
		}
		private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.mainForm.HQMainForm_KeyPress(this.mainForm, e);
		}
		private void iniSizeAndStyle()
		{
			this.mainForm.Height = this.panelHQ.Height - 1;
			this.mainForm.Width = this.panelHQ.Width - 1;
			this.setLeftBtBackcolor(this.btRanking);
			this.topPanel.BackColor = Color.FromArgb(37, 37, 37);
			foreach (object current in this.topPanel.Controls)
			{
				if (current is Button)
				{
					Button button = current as Button;
					button.BackColor = this.topBtColor;
					button.MouseEnter += new EventHandler(this.button_MouseEnter);
					button.MouseLeave += new EventHandler(this.button_MouseLeave);
				}
			}
			foreach (object current2 in this.imagePanel.Controls)
			{
				if (current2 is PictureBox)
				{
					PictureBox pictureBox = current2 as PictureBox;
					pictureBox.MouseEnter += new EventHandler(this.pic_MouseEnter);
					pictureBox.MouseLeave += new EventHandler(this.pic_MouseLeave);
				}
			}
		}
		private void button_MouseLeave(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackColor = this.topBtColor;
		}
		private void button_MouseEnter(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackColor = this.topBtMouseColor;
		}
		private void setLeftBtBackcolor(Button clickBt)
		{
			Image lbtn = Resources.lbtn3;
			this.btRanking.BackgroundImage = lbtn;
			this.btMultiRanking.BackgroundImage = lbtn;
			this.btBill.BackgroundImage = lbtn;
			this.btLine.BackgroundImage = lbtn;
			if (clickBt != null)
			{
				clickBt.BackgroundImage = Resources.lbtn;
			}
		}
		public void changeBtColor()
		{
			switch (this.mainForm.CurHQClient.CurrentPage)
			{
			case 0:
				this.setLeftBtBackcolor(this.btRanking);
				return;
			case 1:
				this.setLeftBtBackcolor(this.btLine);
				return;
			case 2:
				this.setLeftBtBackcolor(this.btLine);
				return;
			case 4:
				this.setLeftBtBackcolor(this.btBill);
				return;
			case 5:
				this.setLeftBtBackcolor(this.btMultiRanking);
				return;
			}
			this.setLeftBtBackcolor(null);
		}
		public void CloseMainForm()
		{
			try
			{
				if (this.mainForm.CurHQClient.CurrentPage != 0)
				{
					this.setInfo.lastSave();
				}
				this.setInfo.saveSetInfo("iStyle", this.mainForm.iStyle.ToString());
				if (this.mainForm.CurHQClient != null)
				{
					this.mainForm.CurHQClient.Dispose();
				}
				if (this.mainForm.MainGraph != null)
				{
					this.mainForm.MainGraph.PageDispose();
				}
				this.buttonUtils.ButtonList.Clear();
				this.buttonUtils.RightButtonList.Clear();
				this.buttonUtils.isTidyBtnFlag = 0;
				SelectServer.GetInstance().Close();
				this.mainForm.Dispose();
				this.mainForm.Close();
				base.Dispose();
				base.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "CloseMainForm异常：" + ex.Message);
			}
		}
		private void SetControlText()
		{
			this.btRanking.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btRanking");
			this.btMultiRanking.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btMultiRanking");
			this.btLine.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btLine");
			this.btBill.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btBill");
			base.Icon = (Icon)this.pluginInfo.HQResourceManager.GetObject("Logo.ico");
			this.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_title");
			this.btHangYe.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btHangYe");
			this.btDiQu.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btDiQu");
			this.btNews.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btNews");
			this.btOwnGoods.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btOwnGoods");
			this.btSysMessage.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_btSysMessage");
			this.labelSet.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_labelSet");
			this.lbAbout.Text = this.pluginInfo.HQResourceManager.GetString("VHQStr_lbAbout");
			if (!Tools.StrToBool(this.pluginInfo.HTConfig["LeftButton"].ToString(), false))
			{
				this.panelLeftBtn.Visible = false;
				this.panelHQ.Left -= this.panelLeftBtn.Width - 5;
				this.panelHQ.Width += this.panelLeftBtn.Width - 5;
			}
			this.bUpdate = Tools.StrToBool((string)this.pluginInfo.HTConfig["AutoUpdate"], false);
		}
		private void pic_MouseEnter(object sender, EventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;
			try
			{
				if (!Tools.StrToBool(pictureBox.Tag.ToString(), false))
				{
					pictureBox.BackgroundImage = this.backgroundImage;
				}
			}
			catch (Exception)
			{
			}
		}
		private void pic_MouseLeave(object sender, EventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;
			this.toolTipTopImage.Hide(pictureBox);
			if (!Tools.StrToBool(pictureBox.Tag.ToString(), false))
			{
				pictureBox.BackgroundImage = this.backgroundImageleave;
			}
		}
		public void ChangeKLineBtnColor()
		{
			foreach (Control control in this.imagePanel.Controls)
			{
				if (control is PictureBox)
				{
					if (this.mainForm.CurHQClient.CurrentPage == 2)
					{
						if (control.Name == "StartButton" || control.Name == "BJPMBtn" || control.Name == "FSZSBtn" || control.Name == "KLineBtn")
						{
							control.BackgroundImage = this.backgroundImageleave;
						}
						else
						{
							control.BackgroundImage = this.backgroundImageleave;
							if (this.mainForm.CurHQClient.m_iKLineCycle == 1 && control.Name == "Day")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 13 && control.Name == "AnyDay")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 14 && control.Name == "AnyMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 11 && control.Name == "TwoHr")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 12 && control.Name == "FourHr")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 5 && control.Name == "OneMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 8 && control.Name == "FifteenMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 6 && control.Name == "ThreeMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 9 && control.Name == "ThirtyMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 7 && control.Name == "FiveMin")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 10 && control.Name == "Hour")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 3 && control.Name == "Month")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 4 && control.Name == "Quarter")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							if (this.mainForm.CurHQClient.m_iKLineCycle == 2 && control.Name == "Week")
							{
								control.BackgroundImage = this.backgroundImage;
							}
						}
					}
					else
					{
						if (this.mainForm.CurHQClient.CurrentPage == 0 && control.Name == "StartButton")
						{
							control.BackgroundImage = this.backgroundImage;
						}
						if (this.mainForm.CurHQClient.CurrentPage == 0 && control.Name == "BJPMBtn")
						{
							control.BackgroundImage = this.backgroundImage;
						}
						else
						{
							if (this.mainForm.CurHQClient.CurrentPage == 1 && control.Name == "FSZSBtn")
							{
								control.BackgroundImage = this.backgroundImage;
							}
							else
							{
								if (this.mainForm.CurHQClient.CurrentPage == 4 && control.Name == "Bill")
								{
									control.BackgroundImage = this.backgroundImage;
								}
								else
								{
									control.BackgroundImage = this.backgroundImageleave;
								}
							}
						}
					}
				}
			}
		}
		private void image_Click(object sender, EventArgs e)
		{
			try
			{
				if (sender is PictureBox)
				{
					this.ChangeBtnBackGround((PictureBox)sender);
				}
				bool flag = true;
				this.mainForm.IsMultiCycle = false;
				PictureBox pictureBox = sender as PictureBox;
				if (pictureBox.Name == "Day")
				{
					this.mainForm.CurHQClient.m_iKLineCycle = 1;
					flag = true;
				}
				else
				{
					if (pictureBox.Name == "Week")
					{
						this.mainForm.CurHQClient.m_iKLineCycle = 2;
						flag = true;
					}
					else
					{
						if (pictureBox.Name == "Month")
						{
							this.mainForm.CurHQClient.m_iKLineCycle = 3;
							flag = true;
						}
						else
						{
							if (pictureBox.Name == "Quarter")
							{
								this.mainForm.CurHQClient.m_iKLineCycle = 4;
								flag = true;
							}
							else
							{
								if (pictureBox.Name == "AnyDay")
								{
									InputWindow inputWindow = new InputWindow(1);
									inputWindow.ShowDialog();
									if (inputWindow.DialogResult == DialogResult.Yes)
									{
										this.mainForm.CurHQClient.m_iKLineCycle = 13;
										this.mainForm.CurHQClient.KLineValue = inputWindow.KValue;
										flag = true;
									}
									else
									{
										flag = false;
									}
								}
								else
								{
									if (pictureBox.Name == "OneMin")
									{
										this.mainForm.CurHQClient.m_iKLineCycle = 5;
										flag = true;
									}
									else
									{
										if (pictureBox.Name == "ThreeMin")
										{
											this.mainForm.CurHQClient.m_iKLineCycle = 6;
											flag = true;
										}
										else
										{
											if (pictureBox.Name == "FiveMin")
											{
												this.mainForm.CurHQClient.m_iKLineCycle = 7;
												flag = true;
											}
											else
											{
												if (pictureBox.Name == "FifteenMin")
												{
													this.mainForm.CurHQClient.m_iKLineCycle = 8;
													flag = true;
												}
												else
												{
													if (pictureBox.Name == "ThirtyMin")
													{
														this.mainForm.CurHQClient.m_iKLineCycle = 9;
														flag = true;
													}
													else
													{
														if (pictureBox.Name == "Hour")
														{
															this.mainForm.CurHQClient.m_iKLineCycle = 10;
															flag = true;
														}
														else
														{
															if (pictureBox.Name == "TwoHr")
															{
																this.mainForm.CurHQClient.m_iKLineCycle = 11;
																flag = true;
															}
															else
															{
																if (pictureBox.Name == "FourHr")
																{
																	this.mainForm.CurHQClient.m_iKLineCycle = 12;
																	flag = true;
																}
																else
																{
																	if (pictureBox.Name == "AnyMin")
																	{
																		InputWindow inputWindow2 = new InputWindow(2);
																		inputWindow2.ShowDialog();
																		if (inputWindow2.DialogResult == DialogResult.Yes)
																		{
																			this.mainForm.CurHQClient.m_iKLineCycle = 14;
																			this.mainForm.CurHQClient.KLineValue = inputWindow2.KValue;
																			flag = true;
																		}
																		else
																		{
																			flag = false;
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (flag)
				{
					if (this.mainForm.CurHQClient.CurrentPage != 1)
					{
						this.mainForm.CurHQClient.CurrentPage = 1;
					}
					this.mainForm.UserCommand("05");
					this.mainForm.CurHQClient.globalData.PrePage = 2;
					this.mainForm.Repaint();
				}
				this.mainForm.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "image_Click异常：" + ex.Message);
			}
		}
		private void ChangeBtnBackGround(PictureBox pic)
		{
			foreach (Control control in this.imagePanel.Controls)
			{
				if (control is PictureBox)
				{
					if (((PictureBox)control).Equals(pic))
					{
						control.Tag = true;
					}
					else
					{
						control.Tag = false;
					}
				}
			}
		}
		private void btRanking_Click(object sender, EventArgs e)
		{
			try
			{
				if (sender is PictureBox)
				{
					this.ChangeBtnBackGround((PictureBox)sender);
				}
				if ((this.buttonUtils.CurButtonName == "MyCommodity" || this.buttonUtils.CurButtonName == "Select") && this.mainForm.CurHQClient.CurrentPage == 0)
				{
					this.multiQuoteData.iStart = 0;
					this.multiQuoteData.yChange = 0;
					this.multiQuoteData.MultiQuotePage = 0;
					this.buttonUtils.CurButtonName = "AllMarket";
					if (this.buttonUtils.ButtonList.Count > 0)
					{
						MyButton myButton = (MyButton)this.buttonUtils.ButtonList[0];
						myButton.Selected = true;
						foreach (MyButton myButton2 in this.buttonUtils.ButtonList)
						{
							if (myButton2 != myButton && myButton2.Selected)
							{
								myButton2.Selected = false;
							}
						}
					}
				}
				this.mainForm.UserCommand("60");
				this.mainForm.Repaint();
				this.mainForm.Focus();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "btRanking_Click异常：" + ex.Message);
			}
		}
		private void btMultiRanking_Click(object sender, EventArgs e)
		{
			this.mainForm.UserCommand("80");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void btLine_Click(object sender, EventArgs e)
		{
			if (this.mainForm.CurHQClient.CurrentPage != 1 && this.mainForm.CurHQClient.CurrentPage != 2)
			{
				this.mainForm.UserCommand("05");
			}
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void btBill_Click(object sender, EventArgs e)
		{
			this.mainForm.UserCommand("01");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void FSZSBtn_Click(object sender, EventArgs e)
		{
			if (sender is PictureBox)
			{
				this.ChangeBtnBackGround((PictureBox)sender);
			}
			if (this.mainForm.CurHQClient.curCommodityInfo == null)
			{
				return;
			}
			this.mainForm.CurHQClient.CurrentPage = 2;
			this.mainForm.UserCommand("05");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void KLineBtn_Click(object sender, EventArgs e)
		{
			if (sender is PictureBox)
			{
				this.ChangeBtnBackGround((PictureBox)sender);
			}
			if (this.mainForm.CurHQClient.curCommodityInfo == null)
			{
				return;
			}
			this.mainForm.CurHQClient.CurrentPage = 1;
			this.mainForm.UserCommand("05");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void btNews_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.gnnt.com.cn/");
			this.mainForm.Focus();
		}
		private void lbAbout_Click(object sender, EventArgs e)
		{
			if (this.about == null || this.about.IsDisposed)
			{
				this.about = new About(this);
				this.about.Show();
			}
			else
			{
				this.about.TopMost = true;
			}
			this.mainForm.Focus();
		}
		private void btSysMessage_Click(object sender, EventArgs e)
		{
			if (this.sys == null || this.sys.IsDisposed)
			{
				this.sys = new SysMessage(this);
			}
			this.sys.Show();
			this.sys.TopMost = true;
		}
		private void panelHQ_SizeChanged(object sender, EventArgs e)
		{
			try
			{
				this.mainForm.Size = this.panelHQ.Size;
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "panelHQ_SizeChanged窗体出错：" + ex.Message + ex.StackTrace);
			}
		}
		private void btOwnGoods_Click(object sender, EventArgs e)
		{
			try
			{
				this.buttonUtils.CurButtonName = "MyCommodity";
				if (this.buttonUtils.ButtonList.Count > 0)
				{
					MyButton myButton = (MyButton)this.buttonUtils.ButtonList[this.buttonUtils.ButtonList.Count - 1];
					myButton.Selected = true;
				}
				this.multiQuoteData.MultiQuotePage = 1;
				this.mainForm.UserCommand("60");
				this.mainForm.Repaint();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "btOwnGoods_Click异常：" + ex.Message);
			}
		}
		private void timerStatusTime_Tick(object sender, EventArgs e)
		{
		}
		private void setStatusAndTime()
		{
			try
			{
				if (this.mainForm == null)
				{
					Logger.wirte(MsgType.Information, "setStatusAndTime:mainForm == null");
				}
				else
				{
					if (this.mainForm.CurHQClient == null)
					{
						Logger.wirte(MsgType.Information, "setStatusAndTime:mainForm.CurHQClient == null");
					}
					else
					{
						string text = TradeTimeVO.HHMMIntToString(this.mainForm.CurHQClient.m_iTime / 100);
						if (text.EndsWith(":"))
						{
							text = text.Substring(0, text.Length - 1);
						}
						string text2 = this.mainForm.CurHQClient.m_iDate.ToString("####-##-##") + " " + text;
						this.lbTime.Text = text2;
						if (this.mainForm.CurHQClient.Connected)
						{
							this.lbConnect.ForeColor = Color.LimeGreen;
							this.lbConnect.Text = "连接正常";
						}
						else
						{
							this.lbConnect.ForeColor = Color.Red;
							this.lbConnect.Text = "连接失败";
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "设置系统时间错误：" + ex.Message + ex.StackTrace);
			}
		}
		private void BackBtn_Click(object sender, EventArgs e)
		{
			int currentPage = this.mainForm.CurHQClient.CurrentPage;
			if (this.mainForm.CurHQClient.oldPage == 1 || this.mainForm.CurHQClient.oldPage == 2)
			{
				if (currentPage != 1 && currentPage != 2)
				{
					this.mainForm.UserCommand("05");
				}
				else
				{
					this.mainForm.OnF5();
				}
			}
			else
			{
				if (this.mainForm.CurHQClient.oldPage == 0)
				{
					this.mainForm.UserCommand("60");
				}
				else
				{
					if (this.mainForm.CurHQClient.oldPage == 5)
					{
						this.mainForm.UserCommand("80");
					}
					else
					{
						if (this.mainForm.CurHQClient.oldPage == 4)
						{
							this.mainForm.UserCommand("01");
						}
						else
						{
							if (this.mainForm.CurHQClient.oldPage == 6)
							{
								this.mainForm.UserCommand("70");
							}
						}
					}
				}
			}
			if (this.mainForm.CurHQClient.CurrentPage != -1)
			{
				this.mainForm.CurHQClient.oldPage = currentPage;
			}
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void btNews_MouseLeave(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn;
		}
		private void btNews_MouseEnter(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn1;
		}
		private void btSysMessage_MouseEnter(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn1;
		}
		private void btSysMessage_MouseLeave(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn;
		}
		private void btOwnGoods_MouseEnter(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn1;
		}
		private void btOwnGoods_MouseLeave(object sender, EventArgs e)
		{
			Button button = sender as Button;
			button.BackgroundImage = Resources.btn;
		}
		private void btHangYe_MouseEnter(object sender, EventArgs e)
		{
			SplitButton splitButton = sender as SplitButton;
			splitButton.BackgroundImage = Resources.cbox1;
		}
		private void btHangYe_MouseLeave(object sender, EventArgs e)
		{
			SplitButton splitButton = sender as SplitButton;
			splitButton.BackgroundImage = Resources.cbox;
		}
		private void btDiQu_MouseEnter(object sender, EventArgs e)
		{
			SplitButton splitButton = sender as SplitButton;
			splitButton.BackgroundImage = Resources.cbox1;
		}
		private void btDiQu_MouseLeave(object sender, EventArgs e)
		{
			SplitButton splitButton = sender as SplitButton;
			splitButton.BackgroundImage = Resources.cbox;
		}
		private void MainWindow_Shown(object sender, EventArgs e)
		{
			if (Tools.StrToBool(this.pluginInfo.HTConfig["WelcomeInfo"].ToString(), false))
			{
				this.ShowMessage();
			}
		}
		private void labelSet_Click(object sender, EventArgs e)
		{
			try
			{
				ServerSet serverSet = new ServerSet(this);
				if (DialogResult.OK == serverSet.ShowDialog())
				{
					Process.Start(Assembly.GetExecutingAssembly().Location);
					base.Close();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "labelSet_Click异常：" + ex.Message);
			}
		}
		private void setTimeMarket_Click(object sender, EventArgs e)
		{
			try
			{
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					Label label = sender as Label;
					string arg_34_0 = label.Text;
					MarketForm marketForm = label.Parent.Parent as MarketForm;
					string text = label.Tag.ToString();
					this.setInfo.saveSetInfo("CurTimeMarketId", text);
					this.setInfo.CurTimeMarketId = text;
					marketForm.Close();
					MarketDataVO marketDataVO = (MarketDataVO)this.mainForm.CurHQClient.m_htMarketData[text];
					this.mainForm.CurHQClient.m_iTime = marketDataVO.time;
					this.mainForm.CurHQClient.m_iDate = marketDataVO.date;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "setTimeMarket_Click异常：" + ex.Message);
			}
		}
		private void lbTime_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					if (this.mainForm.CurHQClient.m_htMarketData.Count != 0)
					{
						MarketForm marketForm = new MarketForm();
						marketForm.Text = "选择对应的市场为当前系统时间";
						int num = 0;
						foreach (DictionaryEntry dictionaryEntry in this.mainForm.CurHQClient.m_htMarketData)
						{
							MarketDataVO marketDataVO = (MarketDataVO)dictionaryEntry.Value;
							Label label = new Label
							{
								Tag = marketDataVO.marketID,
								Parent = marketForm.MainPanel,
								ForeColor = Color.White,
								Location = new Point(20, this.lbLocationY + num * this.lbHeight),
								Font = new Font("宋体", 12f, (this.setInfo.CurTimeMarketId == marketDataVO.marketID) ? FontStyle.Regular : FontStyle.Underline),
								TextAlign = ContentAlignment.MiddleRight,
								AutoSize = true,
								Text = marketDataVO.marketName
							};
							if (this.setInfo.CurTimeMarketId != marketDataVO.marketID)
							{
								label.Cursor = Cursors.Hand;
								label.Click += new EventHandler(this.setTimeMarket_Click);
							}
							num++;
						}
						marketForm.labelId.Visible = false;
						marketForm.labelName.Left = 20;
						marketForm.ShowDialog();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "lbTime_DoubleClick异常：" + ex.Message);
			}
		}
		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.CloseMainForm();
		}
		private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (this.mainForm.CurHQClient.CurrentPage != 0)
			{
				this.setInfo.lastSave();
			}
		}
		private void refreshBt_Click(object sender, EventArgs e)
		{
			this.mainForm.UserCommand("refreshBt");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void pictureBoxBill_Click(object sender, EventArgs e)
		{
			if (sender is PictureBox)
			{
				this.ChangeBtnBackGround((PictureBox)sender);
			}
			if (this.mainForm.CurHQClient.curCommodityInfo == null)
			{
				return;
			}
			this.mainForm.CurHQClient.CurrentPage = 4;
			this.mainForm.UserCommand("01");
			this.mainForm.Repaint();
			this.mainForm.Focus();
		}
		private void pictureUp_Click(object sender, EventArgs e)
		{
			this.KLineUpDown(1);
		}
		private void pictureDown_Click(object sender, EventArgs e)
		{
			this.KLineUpDown(2);
		}
		private void KLineUpDown(int udFlag)
		{
			if (this.mainForm.CurHQClient.kLineUpDown != null && this.mainForm.CurHQClient.CurrentPage == 2)
			{
				this.mainForm.CurHQClient.kLineUpDown(udFlag);
				this.mainForm.Repaint();
			}
		}
		private void pictureF10_Click(object sender, EventArgs e)
		{
			CommodityInfo curCommodityInfo = this.mainForm.CurHQClient.curCommodityInfo;
			if (curCommodityInfo != null)
			{
				string commodityCode = curCommodityInfo.commodityCode;
				this.mainForm.CurHQClient.m_hqForm.DisplayCommodityInfo(commodityCode);
			}
		}
		private void pictureSet_Click(object sender, EventArgs e)
		{
			UserSet userSet = new UserSet(this.mainForm.CurHQClient.m_hqForm);
			userSet.ShowDialog();
		}
		private void Indecator_Click(object sender, EventArgs e)
		{
			int length = IndicatorBase.INDICATOR_NAME.GetLength(0);
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			for (int i = 0; i < length; i++)
			{
				string text = IndicatorBase.INDICATOR_NAME[i, 0];
				string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_T_" + IndicatorBase.INDICATOR_NAME[i, 0]);
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text + "  " + @string);
				toolStripMenuItem.Name = "Indicator_" + text;
				contextMenuStrip.Items.Add(toolStripMenuItem);
			}
			contextMenuStrip.Show(this, this.Indecator.Location.X, this.Indecator.Location.Y + this.Indecator.Height);
			contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
		}
		private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string name = e.ClickedItem.Name;
			this.mainForm.CurHQClient.m_strIndicator = name.Substring(10);
			this.setInfo.StrIndicator = name.Substring(10);
			this.setInfo.saveSetInfo("StrIndicator", name.Substring(10));
			this.mainForm.CurHQClient.createIndicator();
			this.mainForm.Repaint();
		}
		private void SetPictureEnable(bool enable)
		{
			this.setEnable = new MainWindow.SetPictureEnableCallBack(this.SetPictureInfo);
			base.Invoke(this.setEnable, new object[]
			{
				enable
			});
		}
		private void SetPictureInfo(bool enable)
		{
			this.Indecator.Visible = enable;
			this.pictureUp.Visible = enable;
			this.pictureDown.Visible = enable;
			string text = (string)this.pluginInfo.HTConfig["CommodityInfoAddress"];
			if (text != null && text.Length > 0)
			{
				this.pictureF10.Visible = true;
			}
			else
			{
				this.pictureF10.Visible = false;
			}
			if (enable)
			{
				if (this.pictureF10.Visible)
				{
					this.pictureF10.Location = new Point(855, 0);
					this.pictureSet.Location = new Point(890, 0);
					return;
				}
				this.pictureSet.Location = new Point(855, 0);
				return;
			}
			else
			{
				if (this.pictureF10.Visible)
				{
					this.pictureF10.Location = new Point(750, 0);
					this.pictureSet.Location = new Point(785, 0);
					return;
				}
				this.pictureSet.Location = new Point(750, 0);
				return;
			}
		}
	}
}
