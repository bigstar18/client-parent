using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public abstract class Page_Main : IDisposable
	{
		public bool stopFlag;
		internal HQForm m_hqForm;
		internal HQClientMain m_hqClient;
		internal Rectangle m_rc;
		internal PluginInfo m_pluginInfo;
		internal SetInfo m_setInfo;
		internal MultiQuoteData m_multiQuoteData;
		internal GlobalData m_globalData;
		private Thread timerThread;
		public ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
		public Page_Main(Rectangle rc, HQForm hqForm)
		{
			try
			{
				this.m_hqForm = hqForm;
				this.m_hqClient = hqForm.CurHQClient;
				if (this.m_hqClient == null)
				{
					throw new Exception("hqClient IS NULL 请实例化m_hqClient对象！！！");
				}
				if (this.m_hqClient.sendThread == null)
				{
					throw new Exception("hqClient.sendThread IS NULL 请实例化m_hqClient sendThread对象(通过hqClient.init方法)！！！");
				}
				if (this.m_hqClient.receiveThread == null)
				{
					throw new Exception("hqClient.receiveThread IS NULL 请实例化m_hqClient receiveThread对象(通过hqClient.init方法)！！！");
				}
				if (this.m_hqClient.httpThread == null)
				{
					throw new Exception("hqClient.httpThread IS NULL 请实例化m_hqClient httpThread对象(通过hqClient.init方法)！！！");
				}
				this.m_rc = rc;
				this.m_pluginInfo = this.m_hqClient.pluginInfo;
				this.m_setInfo = this.m_hqClient.setInfo;
				this.m_multiQuoteData = this.m_hqClient.multiQuoteData;
				this.m_globalData = this.m_hqClient.globalData;
				if (this.m_hqForm.MainGraph != null)
				{
					this.m_hqForm.MainGraph.stopFlag = true;
					this.m_hqForm.MainForm_MouseClick -= new MouseEventHandler(this.m_hqForm.MainGraph.Page_MouseClick);
					this.m_hqForm.MainForm_MouseDoubleClick -= new MouseEventHandler(this.m_hqForm.MainGraph.Page_MouseDoubleClick);
					this.m_hqForm.MainForm_MouseMove -= new MouseEventHandler(this.m_hqForm.MainGraph.Page_MouseMove);
					this.m_hqForm.MainForm_KeyDown -= new KeyEventHandler(this.m_hqForm.MainGraph.Page_KeyDown);
					this.m_hqForm.MainGraph.Dispose();
				}
				this.m_hqForm.MainForm_MouseClick += new MouseEventHandler(this.Page_MouseClick);
				this.m_hqForm.MainForm_MouseDoubleClick += new MouseEventHandler(this.Page_MouseDoubleClick);
				this.m_hqForm.MainForm_MouseMove += new MouseEventHandler(this.Page_MouseMove);
				this.m_hqForm.MainForm_KeyDown += new KeyEventHandler(this.Page_KeyDown);
				this.contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
				this.timerThread = new Thread(new ThreadStart(this.run));
				this.timerThread.Start();
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "Page_Main异常：" + ex.Message);
			}
		}
		public void run()
		{
			try
			{
				while (!this.stopFlag)
				{
					Thread.Sleep(5000);
					if (this.m_hqForm.MainGraph == null)
					{
						this.stopFlag = true;
					}
					if (!this.stopFlag)
					{
						this.AskForDataOnTimer();
					}
				}
			}
			catch (ThreadAbortException ex)
			{
				Logger.wirte(MsgType.Error, (string)ex.ExceptionState);
			}
			catch (Exception ex2)
			{
				Logger.wirte(MsgType.Error, ex2.ToString());
			}
		}
		protected abstract void AskForDataOnTimer();
		public abstract void Paint(Graphics g, int v);
		protected abstract void Page_MouseClick(object sender, MouseEventArgs e);
		protected abstract void Page_MouseDoubleClick(object sender, MouseEventArgs e);
		protected abstract void Page_MouseMove(object sender, MouseEventArgs e);
		protected abstract void Page_KeyDown(object sender, KeyEventArgs e);
		protected abstract void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e);
		public abstract void Dispose();
		protected void AddCommonMenu()
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("TradeStr_ButtonSelF3"), (Image)this.m_pluginInfo.HQResourceManager.GetObject("HQImg_refresh"));
			toolStripMenuItem.Name = "refreshBt";
			this.contextMenuStrip.Items.Add(toolStripMenuItem);
			this.m_hqForm.RepaintBottom();
			if (this.m_hqClient.m_codeList.Count > 0)
			{
				this.AddIndexMenu();
			}
			this.contextMenuStrip.Items.Add("-");
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_SetColorStyle"));
			toolStripMenuItem2.Name = "SetColorStyle";
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_CLASSICAL"));
			toolStripMenuItem3.Name = "Color" + 0;
			ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_MODERN"));
			toolStripMenuItem4.Name = "Color" + 1;
			ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_ELEGANCE"));
			toolStripMenuItem5.Name = "Color" + 2;
			ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_SOFTNESS"));
			toolStripMenuItem6.Name = "Color" + 3;
			ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_DIGNITY"));
			toolStripMenuItem7.Name = "Color" + 4;
			ToolStripMenuItem toolStripMenuItem8 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_TRADITION"));
			toolStripMenuItem8.Name = "Color" + 5;
			contextMenuStrip.Items.Add(toolStripMenuItem3);
			contextMenuStrip.Items.Add(toolStripMenuItem4);
			contextMenuStrip.Items.Add(toolStripMenuItem5);
			contextMenuStrip.Items.Add(toolStripMenuItem6);
			contextMenuStrip.Items.Add(toolStripMenuItem7);
			contextMenuStrip.Items.Add(toolStripMenuItem8);
			contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
			toolStripMenuItem2.DropDown = contextMenuStrip;
			if (Tools.StrToBool(this.m_setInfo.SetColorStyle, false))
			{
				this.contextMenuStrip.Items.Add(toolStripMenuItem2);
			}
			ToolStripMenuItem toolStripMenuItem9 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_History") + "  F7", (Image)this.m_pluginInfo.HQResourceManager.GetObject("HQImg_History"));
			toolStripMenuItem9.Name = "page_history";
			this.contextMenuStrip.Items.Add(toolStripMenuItem9);
			this.contextMenuStrip.Items.Add("-");
			ToolStripMenuItem toolStripMenuItem10 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_UserSet"), (Image)this.m_pluginInfo.HQResourceManager.GetObject("HQImg_UserSet"));
			toolStripMenuItem10.Name = "userSet";
			this.contextMenuStrip.Items.Add(toolStripMenuItem10);
		}
		public void AddIndexMenu()
		{
			if (this.contextMenuStrip.Items.ContainsKey("ClassIndexMenu") || this.contextMenuStrip.Items.ContainsKey("MenuSeriesSub"))
			{
				return;
			}
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_ClassIndex"));
			toolStripMenuItem.Name = "ClassIndexMenu";
			int num = 0;
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			for (int i = 0; i < this.m_hqClient.m_codeList.Count; i++)
			{
				CommodityInfo commodityInfo = (CommodityInfo)this.m_hqClient.m_codeList[i];
				CodeTable codeTable = (CodeTable)this.m_hqClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
				if (codeTable.status == 2 || codeTable.status == 3)
				{
					ToolStripMenuItem toolStripMenuItem2;
					if (codeTable.status == 3)
					{
						toolStripMenuItem2 = new ToolStripMenuItem(codeTable.sName + "  F3");
					}
					else
					{
						toolStripMenuItem2 = new ToolStripMenuItem(codeTable.sName);
					}
					toolStripMenuItem2.Name = "INDEX_" + commodityInfo.marketID + commodityInfo.commodityCode;
					if (codeTable.status == 3)
					{
						this.contextMenuStrip.Items.Insert(0, toolStripMenuItem2);
					}
					else
					{
						contextMenuStrip.Items.Add(toolStripMenuItem2);
					}
					num++;
				}
			}
			if (contextMenuStrip.Items.Count > 0)
			{
				contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
				toolStripMenuItem.DropDown = contextMenuStrip;
				this.contextMenuStrip.Items.Insert(0, toolStripMenuItem);
			}
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.m_pluginInfo.HQResourceManager.GetString("HQStr_SeriesPrice"));
			toolStripMenuItem3.Name = "MenuSeriesSub";
			ContextMenuStrip contextMenuStrip2 = new ContextMenuStrip();
			for (int j = 0; j < this.m_hqClient.m_codeList.Count; j++)
			{
				CommodityInfo commodityInfo2 = (CommodityInfo)this.m_hqClient.m_codeList[j];
				CodeTable codeTable2 = (CodeTable)this.m_hqClient.m_htProduct[commodityInfo2.marketID + commodityInfo2.commodityCode];
				if (codeTable2.status == 4)
				{
					ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(codeTable2.sName);
					toolStripMenuItem4.Name = "SERIES_" + commodityInfo2.marketID + "_" + commodityInfo2.commodityCode;
					contextMenuStrip2.Items.Add(toolStripMenuItem4);
				}
			}
			if (contextMenuStrip2.Items.Count > 0)
			{
				contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
				toolStripMenuItem3.DropDown = contextMenuStrip2;
				this.contextMenuStrip.Items.Insert(0, toolStripMenuItem3);
			}
		}
		public void PageDispose()
		{
			this.Dispose();
			this.stopFlag = true;
			if (this.timerThread != null)
			{
				this.timerThread.Abort("系统退出，放弃 Page_Main中的线程");
			}
			this.timerThread.Join();
		}
	}
}
