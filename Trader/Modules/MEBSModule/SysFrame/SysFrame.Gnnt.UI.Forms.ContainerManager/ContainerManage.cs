using PluginInterface;
using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation.MainOperation;
using SysFrame.Gnnt.Common.Operation.Manager;
using SysFrame.Gnnt.UI.Forms.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace SysFrame.Gnnt.UI.Forms.ContainerManager
{
	public class ContainerManage
	{
		private Control MainControl;
		private ProgressControl ProgressCtrl = new ProgressControl();
		private BackgroundWorker LoginProgressWorker = new BackgroundWorker();
		private Panel TopPanel = new Panel();
		private Panel HQPanel = new Panel();
		private Panel TradePanel = new Panel();
		private Splitter splitterHQTrade = new Splitter();
		private int TopControlHeight = 40;
		private int PanelTradeHeight = 304;
		private int TopToolsBarHeight = 40;
		private int height = 70;
		private int BJheight = 20;
		private int splitterHQTradeHeight = 3;
		private Form curForm;
		private Form hqForm;
		private Control hqGrid;
		private Control toolsBar;
		private bool addHQGridFlag;
		private OperationManager operationManager = OperationManager.GetInstance();
		private Point[] pt = new Point[7];
		public ContainerManage(Control control)
		{
			this.MainControl = control;
			this.operationManager.FormStyle = Tools.StrToInt(Global.htConfig["FormStyle"].ToString(), 0);
			this.operationManager.stripButtonOperation.RefreshPanel = new StripButtonOperation.RefreshPanelCallBack(this.FillHQTrade);
			this.splitterHQTrade.SplitterMoved += new SplitterEventHandler(this.splitterHQTrade_SplitterMoved);
			this.TradePanel.Paint += new PaintEventHandler(this.TradePanel_Paint);
			this.TradePanel.MouseClick += new MouseEventHandler(this.TradePanel_MouseClick);
			this.HQPanel.Paint += new PaintEventHandler(this.HQPanel_Paint);
			this.LoginProgressWorker.WorkerReportsProgress = true;
			this.LoginProgressWorker.DoWork += new DoWorkEventHandler(this.LoginProgressWorker_DoWork);
			this.LoginProgressWorker.ProgressChanged += new ProgressChangedEventHandler(this.LoginProgressWorker_ProgressChanged);
			this.LoginProgressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoginProgressWorker_RunWorkerCompleted);
			this.operationManager.sysFrameOperation.lockTradeFormEventCallBack = new SysFrameOperation.LockTradeFormEventCallBack(this.LockTradeForm);
			this.operationManager.sysFrameOperation.floatTradeFormEventCallBack = new SysFrameOperation.FloatTradeFormEventCallBack(this.FloatTradeForm);
			this.operationManager.sysFrameOperation.logOutTradeFormEventCallBack = new SysFrameOperation.LogOutTradeFormEventCallBack(this.operationManager.stripButtonOperation.Logout);
		}
		public void ControlLayOut()
		{
			this.FillHQPanel(null);
			this.FillTopControl();
		}
		private void FillProcessControl(FormandPlugin temp)
		{
			if (temp != null)
			{
				this.ProgressCtrl.Dock = DockStyle.Bottom;
				this.HQPanel.Controls.Add(this.ProgressCtrl);
				this.ProgressCtrl.InitProgressCtrl();
				this.ProgressCtrl.Visible = true;
				this.LoginProgressWorker.RunWorkerAsync(temp);
			}
		}
		private void FillTopControl()
		{
			StripButtonControl stripButtonControl = new StripButtonControl();
			stripButtonControl.Dock = DockStyle.Top;
			stripButtonControl.Height = this.TopControlHeight;
			this.TopPanel.Controls.Add(stripButtonControl);
			this.TopPanel.Dock = DockStyle.Top;
			this.TopPanel.Height = this.TopControlHeight;
			this.MainControl.Controls.Add(this.TopPanel);
		}
		private void FillTopToolsBar(object obj)
		{
			Control control = (Control)obj;
			control.Dock = DockStyle.Bottom;
			control.Height = this.TopToolsBarHeight;
			this.TopPanel.Controls.Add(control);
		}
		private void FillHQGrid(object obj)
		{
			Control control = (Control)obj;
			control.Dock = DockStyle.Left;
			control.Width = 260;
			this.HQPanel.Controls.Add(control);
		}
		private void FillHQTrade(PanelLoad panel, FormandPlugin temp)
		{
			switch (panel)
			{
			case PanelLoad.HQPanelLoad:
				if (!temp.plugin.get_Name().EndsWith("HQSystem"))
				{
					this.FillHQPanel(temp);
					return;
				}
				if (this.operationManager.PluginNameList.Contains(temp.plugin.get_Name()) || !this.operationManager.isLogin)
				{
					this.FillHQPanel(temp);
					return;
				}
				this.HQPanel.Controls.Clear();
				this.hqForm = temp.form;
				this.operationManager.PluginNameList.Add(temp.plugin.get_Name());
				return;
			case PanelLoad.TradePanelLoad:
				if (this.operationManager.PluginNameList.Contains(temp.plugin.get_Name()))
				{
					if (temp.plugin.get_Name() == "OTC_Trade")
					{
						this.addHQGridFlag = true;
					}
					else
					{
						this.addHQGridFlag = false;
					}
					this.FillTradePanel(temp.form);
					return;
				}
				if (temp.plugin.get_Name() == "OTC_Trade")
				{
					this.addHQGridFlag = true;
				}
				else
				{
					this.addHQGridFlag = false;
				}
				this.FillProcessControl(temp);
				this.operationManager.PluginNameList.Add(temp.plugin.get_Name());
				return;
			case PanelLoad.RemoveHQTrade:
				this.RemoveHQTrade();
				return;
			default:
				return;
			}
		}
		private void FillHQPanel(FormandPlugin formAndPlugin)
		{
			Form form = null;
			if (formAndPlugin != null)
			{
				form = formAndPlugin.form;
			}
			this.HQPanel.Controls.Clear();
			if (form == null)
			{
				this.HQPanel.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("HQImg_main");
				this.HQPanel.BackgroundImageLayout = ImageLayout.Stretch;
				this.HQPanel.Dock = DockStyle.Fill;
				this.HQPanel.BringToFront();
				this.TradePanel.Visible = false;
				if (!this.MainControl.Controls.Contains(this.HQPanel))
				{
					this.MainControl.Controls.Add(this.HQPanel);
					return;
				}
			}
			else
			{
				this.HQPanel.BackgroundImage = null;
				if (this.TradePanel.Controls.Count < 1)
				{
					this.TradePanel.Visible = false;
				}
				form.TopLevel = false;
				form.Dock = DockStyle.Fill;
				form.FormBorderStyle = FormBorderStyle.None;
				this.HQPanel.Visible = true;
				this.HQPanel.Dock = DockStyle.Fill;
				if (this.hqGrid != null && formAndPlugin.plugin != null && this.operationManager.isLogin)
				{
					this.HQPanel.Controls.Add(form);
					if (formAndPlugin.plugin.get_Name().Equals("OTC_HQSystem") || formAndPlugin.plugin.get_Name().Equals("OTC_Trade"))
					{
						this.FillHQGrid(this.hqGrid);
					}
				}
				else
				{
					this.HQPanel.Controls.Add(form);
				}
				this.HQPanel.BringToFront();
				if (!this.MainControl.Controls.Contains(this.HQPanel))
				{
					this.MainControl.Controls.Add(this.HQPanel);
				}
				form.Show();
			}
		}
		private void FillTradePanel(Form tradeForm)
		{
			this.MainControl.Controls.Remove(this.TradePanel);
			this.TradePanel.Controls.Clear();
			if (!this.operationManager.sysFrameOperation.isLockTrade)
			{
				if (this.operationManager.FormStyle == 8 && this.operationManager.stripButtonOperation.curLoginPluginName.Contains("MEBS"))
				{
					this.TradePanel.Height = this.PanelTradeHeight + this.height;
				}
				else if (this.hqGrid != null && this.addHQGridFlag)
				{
					this.TradePanel.Height = this.PanelTradeHeight - this.BJheight;
				}
				else
				{
					this.TradePanel.Height = this.PanelTradeHeight;
				}
			}
			this.curForm = tradeForm;
			if (this.curForm == null || this.curForm.IsDisposed)
			{
				return;
			}
			this.curForm.TopLevel = false;
			this.curForm.Dock = DockStyle.Fill;
			this.curForm.WindowState = FormWindowState.Normal;
			this.curForm.FormBorderStyle = FormBorderStyle.None;
			this.TradePanel.Dock = DockStyle.Bottom;
			this.splitterHQTrade.Dock = DockStyle.Bottom;
			this.splitterHQTrade.Height = 2;
			this.splitterHQTrade.BackColor = Color.White;
			this.splitterHQTrade.Visible = true;
			if (!this.MainControl.Controls.Contains(this.splitterHQTrade))
			{
				this.MainControl.Controls.Add(this.splitterHQTrade);
			}
			this.TradePanel.Controls.Add(this.curForm);
			this.curForm.Tag = this;
			this.TradePanel.Visible = true;
			this.TradePanel.BringToFront();
			if (!this.MainControl.Controls.Contains(this.TradePanel))
			{
				this.MainControl.Controls.Add(this.TradePanel);
			}
			this.curForm.Show();
			if (this.toolsBar != null)
			{
				this.TopPanel.Height = this.TopControlHeight + this.TopToolsBarHeight;
				this.FillTopToolsBar(this.toolsBar);
			}
		}
		private void RemoveHQTrade()
		{
			this.HQPanel.Controls.Clear();
			this.HQPanel.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("HQImg_main");
			this.HQPanel.BackgroundImageLayout = ImageLayout.Stretch;
			this.TradePanel.Controls.Clear();
			this.TopPanel.Height = this.TopControlHeight;
			this.TradePanel.Visible = false;
		}
		private void splitterHQTrade_SplitterMoved(object sender, SplitterEventArgs e)
		{
			this.PanelTradeHeight = this.TradePanel.Height;
		}
		private void TradePanel_Paint(object sender, PaintEventArgs e)
		{
			if (this.curForm != null && this.operationManager.sysFrameOperation.isLockTrade)
			{
				Graphics graphics = e.Graphics;
				Font font = new Font("宋体", 13f, FontStyle.Bold);
				SolidBrush solidBrush = new SolidBrush(Color.Brown);
				Icon icon = this.curForm.Icon;
				string text = this.curForm.Text;
				int y = this.TradePanel.Height;
				int num = (int)graphics.MeasureString(text, font).Width + 16 + 16;
				int width = this.TradePanel.Width;
				int num2 = (this.TradePanel.Width - num) / 2;
				this.pt[0] = new Point(0, y);
				this.pt[1] = new Point(0, 3);
				this.pt[2] = new Point(3, 0);
				this.pt[3] = new Point(width - 3, 0);
				this.pt[4] = new Point(width, 3);
				this.pt[5] = new Point(width, y);
				this.pt[6] = new Point(0, y);
				Brush buttonHighlight = SystemBrushes.ButtonHighlight;
				graphics.FillPolygon(buttonHighlight, this.pt);
				graphics.DrawIcon(icon, new Rectangle(num2 + 4, 2, 20, 20));
				graphics.DrawString(text, font, solidBrush, (float)(num2 + 24 + 4), 3f);
				solidBrush.Dispose();
			}
		}
		private void TradePanel_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.PointInFences(new Point(e.X, e.Y), this.pt))
			{
				this.splitterHQTrade.Visible = true;
				this.renewPanel2();
			}
		}
		private void LockTradeForm(bool isLockTrade)
		{
			if (!isLockTrade)
			{
				this.curForm = (Form)this.TradePanel.Controls[0];
				this.TradePanel.Controls.Clear();
				if (!this.curForm.TopLevel)
				{
					this.operationManager.sysFrameOperation.isLockTrade = true;
					if (this.operationManager.FormStyle == 8 && this.operationManager.stripButtonOperation.curLoginPluginName.Contains("MEBS"))
					{
						this.HQPanel.Height += this.PanelTradeHeight + this.height - 25;
					}
					else if (this.hqGrid != null && this.addHQGridFlag)
					{
						this.HQPanel.Height += this.PanelTradeHeight - this.BJheight - 25;
					}
					else
					{
						this.HQPanel.Height += this.PanelTradeHeight - 25;
					}
					this.TradePanel.Height = 25;
				}
			}
		}
		private void FloatTradeForm(bool isFloat)
		{
			if (!isFloat)
			{
				if (this.TradePanel.Controls.Count > 0)
				{
					this.curForm = (Form)this.TradePanel.Controls[0];
				}
				if (this.curForm != null)
				{
					Size size = new Size(this.TradePanel.ClientSize.Width, this.TradePanel.ClientSize.Height + 30);
					this.TradePanel.Controls.Clear();
					this.curForm.Left = 0;
					this.curForm.Top = Screen.PrimaryScreen.WorkingArea.Height - this.curForm.Height - 30;
					this.curForm.Dock = DockStyle.None;
					this.curForm.FormBorderStyle = FormBorderStyle.Sizable;
					this.curForm.ControlBox = false;
					this.curForm.Size = size;
					if (this.curForm.Location.X < 0)
					{
						this.curForm.Location = new Point(0, this.curForm.Location.Y);
					}
					this.curForm.TopLevel = true;
					this.curForm.Show();
					this.curForm.TopMost = true;
					if (this.operationManager.FormStyle == 8 && this.operationManager.stripButtonOperation.curLoginPluginName.Contains("MEBS"))
					{
						this.HQPanel.Height += this.PanelTradeHeight + this.height;
					}
					else if (this.hqGrid != null && this.addHQGridFlag)
					{
						this.HQPanel.Height += this.PanelTradeHeight - this.BJheight;
					}
					else
					{
						this.HQPanel.Height += this.PanelTradeHeight;
					}
					this.TradePanel.Height = 0;
					this.splitterHQTrade.Visible = false;
				}
				this.operationManager.sysFrameOperation.isFloatTrade = true;
				return;
			}
			if (this.curForm != null)
			{
				this.curForm.TopMost = false;
				this.curForm.TopLevel = false;
				this.curForm.FormBorderStyle = FormBorderStyle.None;
				this.curForm.Dock = DockStyle.Fill;
				this.TradePanel.Controls.Add(this.curForm);
				if (this.operationManager.FormStyle == 8 && this.operationManager.stripButtonOperation.curLoginPluginName.Contains("MEBS"))
				{
					this.HQPanel.Height -= this.PanelTradeHeight + this.height;
					this.TradePanel.Height = this.PanelTradeHeight + this.height;
				}
				else if (this.hqGrid != null && this.addHQGridFlag)
				{
					this.HQPanel.Height -= this.PanelTradeHeight - this.BJheight;
					this.TradePanel.Height = this.PanelTradeHeight - this.BJheight;
				}
				else
				{
					this.HQPanel.Height += this.PanelTradeHeight;
					this.TradePanel.Height = this.PanelTradeHeight;
				}
				this.splitterHQTrade.Visible = true;
			}
			this.operationManager.sysFrameOperation.isFloatTrade = false;
		}
		private void splitContainerForm_Panel2_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.PointInFences(new Point(e.X, e.Y), this.pt))
			{
				this.renewPanel2();
			}
		}
		private void renewPanel2()
		{
			if (this.operationManager.sysFrameOperation.isLockTrade)
			{
				this.operationManager.sysFrameOperation.isLockTrade = false;
				if (this.operationManager.FormStyle == 8 && this.operationManager.stripButtonOperation.curLoginPluginName.Contains("MEBS"))
				{
					this.HQPanel.Height -= this.PanelTradeHeight + this.height - 25;
					this.TradePanel.Height = this.PanelTradeHeight + this.height;
				}
				else if (this.hqGrid != null && this.addHQGridFlag)
				{
					this.HQPanel.Height -= this.PanelTradeHeight - this.BJheight - 25;
					this.TradePanel.Height = this.PanelTradeHeight - this.BJheight;
				}
				else
				{
					this.HQPanel.Height -= this.PanelTradeHeight - 25;
					this.TradePanel.Height = this.PanelTradeHeight;
				}
				if (this.curForm != null)
				{
					this.curForm.TopLevel = false;
					this.TradePanel.Controls.Add(this.curForm);
					this.curForm.BringToFront();
				}
			}
		}
		private bool PointInFences(Point pnt1, Point[] fencePnts)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < fencePnts.Length; i++)
			{
				num = ((i == fencePnts.Length - 1) ? 0 : (num + 1));
				if (fencePnts[i].Y != fencePnts[num].Y && ((pnt1.Y >= fencePnts[i].Y && pnt1.Y < fencePnts[num].Y) || (pnt1.Y >= fencePnts[num].Y && pnt1.Y < fencePnts[i].Y)) && pnt1.X < (fencePnts[num].X - fencePnts[i].X) * (pnt1.Y - fencePnts[i].Y) / (fencePnts[num].Y - fencePnts[i].Y) + fencePnts[i].X)
				{
					num2++;
				}
			}
			return num2 % 2 > 0;
		}
		private void HQPanel_Paint(object sender, PaintEventArgs e)
		{
			if (this.curForm != null && this.operationManager.sysFrameOperation.isLockTrade)
			{
				Graphics graphics = e.Graphics;
				Font font = new Font("宋体", 13f, FontStyle.Bold);
				SolidBrush solidBrush = new SolidBrush(Color.Brown);
				Icon icon = this.curForm.Icon;
				string text = this.curForm.Text;
				int y = this.TradePanel.Height;
				int num = (int)graphics.MeasureString(text, font).Width + 16 + 16;
				int width = this.TradePanel.Width;
				int num2 = (this.TradePanel.Width - num) / 2;
				this.pt[0] = new Point(0, y);
				this.pt[1] = new Point(0, 3);
				this.pt[2] = new Point(3, 0);
				this.pt[3] = new Point(width - 3, 0);
				this.pt[4] = new Point(width, 3);
				this.pt[5] = new Point(width, y);
				this.pt[6] = new Point(0, y);
				Brush buttonHighlight = SystemBrushes.ButtonHighlight;
				graphics.FillPolygon(buttonHighlight, this.pt);
				graphics.DrawIcon(icon, new Rectangle(num2 + 4, 2, 20, 20));
				graphics.DrawString(text, font, solidBrush, (float)(num2 + 24 + 4), 3f);
				solidBrush.Dispose();
			}
		}
		private void ShowProgress(string strInfo, int val)
		{
			try
			{
				if (!this.LoginProgressWorker.CancellationPending)
				{
					Thread.Sleep(100);
					this.LoginProgressWorker.ReportProgress(val, strInfo);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Logger.wirte(3, ex.Message);
			}
		}
		private void LoginProgressWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				FormandPlugin formandPlugin = (FormandPlugin)e.Argument;
				e.Result = formandPlugin;
				formandPlugin.plugin.SetProgressEvent(new EventInitData(this.ShowProgress));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Logger.wirte(3, ex.Message);
			}
		}
		private void LoginProgressWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.ProgressCtrl.SetProgressCtrlInfo(e.UserState.ToString(), e.ProgressPercentage);
		}
		private void LoginProgressWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				Form form = ((FormandPlugin)e.Result).form;
				IPlugin plugin = ((FormandPlugin)e.Result).plugin;
				this.ProgressCtrl.SetProgressCtrlInfo("加载完成", 100);
				this.ProgressCtrl.Visible = false;
				if (form != null && plugin.get_DisplayType() == 4)
				{
					if (form.Tag != null)
					{
						List<Control> list = (List<Control>)form.Tag;
						if (list != null && list.Count == 2)
						{
							this.toolsBar = list[0];
							this.hqGrid = list[1];
						}
					}
					else
					{
						this.TopPanel.Height = this.TopControlHeight;
					}
					this.FillTradePanel(form);
					this.FillHQPanel(new FormandPlugin
					{
						form = this.hqForm,
						plugin = plugin
					});
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Logger.wirte(3, ex.Message);
			}
		}
	}
}
