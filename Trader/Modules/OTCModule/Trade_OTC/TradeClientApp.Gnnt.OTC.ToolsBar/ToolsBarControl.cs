using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
using TradeClientApp.Gnnt.OTC.Library;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC.ToolsBar
{
	public class ToolsBarControl : UserControl
	{
		private delegate void FillFirmInfoDataGrid(DataSet firmDataSet);
		public delegate void ToolsBarButtonCallBack(ToolsBarButton toolsBarButton);
		private delegate void CallbackUpDataFirmInfo(double dataTable);
		private Identity identity;
		private bool IsAgency;
		private ToolsBarControl.FillFirmInfoDataGrid FillFirmInfo;
		public ToolsBarControl.ToolsBarButtonCallBack toolsBarButtonClick;
		private ToolsBarControl.CallbackUpDataFirmInfo callbackUpDataFirmInfo;
		private object floatingPriceTotalLock = new object();
		private bool FirmInfoflag;
		private IContainer components;
		private ToolStrip m_tsTMFToolBar;
		private ToolStripButton tbCreatWareHouse;
		private ToolStripButton tbPWareHouse;
		private ToolStripButton tbAlarm;
		private ToolStripButton tbSetUp;
		private ToolStripButton tbLock;
		private ToolStripButton tbCalc;
		private ToolStripButton tbHelp;
		private ToolStripButton tblogOut;
		private ToolStripLabel toolStripLabelAgency;
		private Panel panelToolsBar;
		private Panel panel1;
		private DataGridView dgvFirmInfo;
		public ToolsBarControl()
		{
			this.InitializeComponent();
		}
		public ToolsBarControl(bool agency, string sIdentity)
		{
			this.IsAgency = agency;
			this.identity = (Identity)Enum.Parse(typeof(Identity), sIdentity);
			this.InitializeComponent();
		}
		private void ToolsBarControl_Load(object sender, EventArgs e)
		{
			this.InitToolBarText();
			this.SetToolsBarVisible();
			this.SetFirmInfoGridVisible();
		}
		private void InitToolBarText()
		{
			this.tbCreatWareHouse.Image = (Image)Global.m_PMESResourceManager.GetObject("CreatWarehouse.png");
			this.tbCreatWareHouse.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_CREATWAREHOUSE");
			this.tbPWareHouse.Image = (Image)Global.m_PMESResourceManager.GetObject("PWarehouse.png");
			this.tbPWareHouse.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_PWAREHOUSE");
			this.tbAlarm.Image = (Image)Global.m_PMESResourceManager.GetObject("Alarm.png");
			this.tbAlarm.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_ALARM");
			this.tbSetUp.Image = (Image)Global.m_PMESResourceManager.GetObject("Setup.png");
			this.tbSetUp.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_SETUP");
			this.tbLock.Image = (Image)Global.m_PMESResourceManager.GetObject("Lock.png");
			this.tbLock.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_LOCK");
			this.tbCalc.Image = (Image)Global.m_PMESResourceManager.GetObject("Calc.png");
			this.tbCalc.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TXET_CALC");
			this.tbHelp.Image = (Image)Global.m_PMESResourceManager.GetObject("Help.png");
			this.tbHelp.Text = Global.m_PMESResourceManager.GetString("PMESStr_TS_TEXT_HELP");
			this.tblogOut.Image = (Image)Global.m_PMESResourceManager.GetObject("AgencyLogoff");
			this.tblogOut.Visible = false;
			this.m_tsTMFToolBar.Font = Global.GetIniFont();
		}
		private void SetToolsBarVisible()
		{
			this.tblogOut.Visible = this.IsAgency;
			this.tbAlarm.Visible = !this.IsAgency;
			this.tbSetUp.Visible = !this.IsAgency;
			this.tbLock.Visible = !this.IsAgency;
			this.toolStripLabelAgency.Visible = this.IsAgency;
			if (this.IsAgency)
			{
				this.m_tsTMFToolBar.BackColor = Color.FromArgb(215, 229, 242);
			}
		}
		private void SetFirmInfoGridVisible()
		{
			if (this.identity == Identity.Member)
			{
				this.panel1.Visible = false;
			}
		}
		public void SetToolsBarEnable(bool enable)
		{
			this.m_tsTMFToolBar.Enabled = enable;
		}
		public void LoadFirmInfoDataGrid(DataSet FirmInfo)
		{
			try
			{
				this.FillFirmInfo = new ToolsBarControl.FillFirmInfoDataGrid(this.FillFirmInfoData);
				this.HandleCreated();
				base.BeginInvoke(this.FillFirmInfo, new object[]
				{
					FirmInfo
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.StackTrace + ex.Message);
			}
		}
		private void FillFirmInfoData(DataSet FirmInfo)
		{
			Logger.wirte(1, "FillFirmInfoDataGrid线程启动1");
			if (FirmInfo != null)
			{
				DataView dataView = new DataView(FirmInfo.Tables["tFirmInfo"]);
				this.dgvFirmInfo.DataSource = dataView.Table;
			}
			Logger.wirte(1, "FillFirmInfoDataGrid线程启动2");
			this.dgvFirmInfo.ClearSelection();
			this.SetFIDataGridColText();
		}
		private void SetFIDataGridColText()
		{
			try
			{
				DataGridViewCellStyle defaultCellStyle = this.dgvFirmInfo.Columns["FirmName"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvFirmInfo.Columns["FirmName"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["FirmName"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_ACCFUND"));
				this.dgvFirmInfo.Columns["FirmName"].Width = 90;
				this.dgvFirmInfo.Columns["FirmName"].Visible = false;
				defaultCellStyle = this.dgvFirmInfo.Columns["InitFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["InitFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["InitFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_TOTALFUNDS"));
				this.dgvFirmInfo.Columns["InitFund"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentRight"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentRight"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentRight"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_BALANCE"));
				this.dgvFirmInfo.Columns["CurrentRight"].Width = 90;
				this.dgvFirmInfo.Columns["CurrentRight"].Visible = false;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentFL"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentFL"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentFL"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FLOATINGLP"));
				this.dgvFirmInfo.Columns["CurrentFL"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["RealFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["RealFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["RealFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_CURRENTBAIL"));
				this.dgvFirmInfo.Columns["RealFund"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["CurrentBail"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["CurrentBail"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["CurrentBail"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_USEDMARGIN"));
				this.dgvFirmInfo.Columns["CurrentBail"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["OrderFrozenFund"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["OrderFrozenFund"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["OrderFrozenFund"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_FROZENFUND"));
				this.dgvFirmInfo.Columns["OrderFrozenFund"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["OrderFrozenFee"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				defaultCellStyle.Format = "n2";
				this.dgvFirmInfo.Columns["OrderFrozenFee"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["OrderFrozenFee"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "AI_ORDERFROZEN"));
				this.dgvFirmInfo.Columns["OrderFrozenFee"].Width = 90;
				defaultCellStyle = this.dgvFirmInfo.Columns["FundRisk"].DefaultCellStyle;
				defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				this.dgvFirmInfo.Columns["FundRisk"].DefaultCellStyle = defaultCellStyle;
				this.dgvFirmInfo.Columns["FundRisk"].HeaderText = Global.m_PMESResourceManager.GetString(string.Format("{0}_{1}", "PMESStr", "FI_RISK"));
				this.dgvFirmInfo.Columns["FundRisk"].Width = 90;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void CalculateFirmInfo(double floatingPT)
		{
			try
			{
				lock (this.floatingPriceTotalLock)
				{
				}
				this.callbackUpDataFirmInfo = new ToolsBarControl.CallbackUpDataFirmInfo(this.UpDataFirmInfo);
				this.HandleCreated();
				base.BeginInvoke(this.callbackUpDataFirmInfo, new object[]
				{
					floatingPT
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void UpDataFirmInfo(double floatingPriceTotal)
		{
			try
			{
				FirmInfoResponseVO firmInfoResponseVO = null;
				if (!this.FirmInfoflag)
				{
					this.FirmInfoflag = true;
					if (this.dgvFirmInfo == null || this.dgvFirmInfo.Rows.Count == 0)
					{
						this.FirmInfoflag = false;
					}
					else
					{
						int rowCount = this.dgvFirmInfo.RowCount;
						if (this.IsAgency)
						{
							lock (Global.AgencyFirmInfoDataLock)
							{
								if (Global.AgencyFirmInfoData == null)
								{
									return;
								}
								firmInfoResponseVO = (FirmInfoResponseVO)Global.AgencyFirmInfoData.Clone();
								goto IL_C0;
							}
						}
						lock (Global.FirmInfoDataLock)
						{
							if (Global.FirmInfoData == null)
							{
								return;
							}
							firmInfoResponseVO = (FirmInfoResponseVO)Global.FirmInfoData.Clone();
						}
						IL_C0:
						for (int i = 0; i < rowCount; i++)
						{
							if (firmInfoResponseVO != null)
							{
								double balance = BizController.CalculateBalance(firmInfoResponseVO.InitFund, firmInfoResponseVO.InOutFund, firmInfoResponseVO.Fee, firmInfoResponseVO.YesterdayBail, firmInfoResponseVO.TransferPL);
								this.dgvFirmInfo.Rows[i].Cells["CurrentRight"].Value = balance.ToString("f2");
								DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
								if (Convert.ToDouble(floatingPriceTotal) > 0.0)
								{
									dataGridViewCellStyle.ForeColor = Color.Red;
								}
								else if (Convert.ToDouble(floatingPriceTotal) == 0.0)
								{
									dataGridViewCellStyle.ForeColor = Color.White;
								}
								else
								{
									dataGridViewCellStyle.ForeColor = Color.Green;
								}
								this.dgvFirmInfo.Rows[i].Cells["CurrentFL"].Style = dataGridViewCellStyle;
								this.dgvFirmInfo.Rows[i].Cells["CurrentFL"].Value = floatingPriceTotal.ToString("f2");
								double initFund = BizController.CalculateInitFund(balance, floatingPriceTotal);
								this.dgvFirmInfo.Rows[i].Cells["InitFund"].Value = initFund.ToString("f2");
								double currentBail = BizController.CalculateHoldingFund(firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.OtherFrozenFund);
								this.dgvFirmInfo.Rows[i].Cells["CurrentBail"].Value = currentBail.ToString("f2");
								double orderFrozenMargin = firmInfoResponseVO.OrderFrozenMargin;
								this.dgvFirmInfo.Rows[i].Cells["OrderFrozenFund"].Value = orderFrozenMargin.ToString("f2");
								this.dgvFirmInfo.Rows[i].Cells["RealFund"].Value = BizController.CalculateRealFund(initFund, firmInfoResponseVO.CurrentBail, firmInfoResponseVO.OrderFrozenFund, firmInfoResponseVO.UsingFund).ToString("f2");
								this.dgvFirmInfo.Rows[i].Cells["FundRisk"].Value = BizController.CalculateFundRisk(initFund, currentBail).ToString("p2");
							}
						}
						this.FirmInfoflag = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void tbCreatWareHouse_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.CreateWareHoust);
			}
		}
		private void tbPWareHouse_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.PWareHouse);
			}
		}
		private void tbAlarm_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.Alarm);
			}
		}
		private void tbSetUp_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.SetUp);
			}
		}
		private void tbLock_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.Lock);
			}
		}
		private void tbCalc_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.Calc);
			}
		}
		private void tblogOut_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.logOut);
			}
		}
		private void tbHelp_Click(object sender, EventArgs e)
		{
			if (this.toolsBarButtonClick != null)
			{
				this.toolsBarButtonClick(ToolsBarButton.Help);
			}
		}
		private void m_tsTMFToolBar_Paint(object sender, PaintEventArgs e)
		{
			Rectangle clip = new Rectangle(0, 0, this.m_tsTMFToolBar.Width - 1, this.m_tsTMFToolBar.Height - 1);
			e.Graphics.SetClip(clip);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ToolsBarControl));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.m_tsTMFToolBar = new ToolStrip();
			this.tbCreatWareHouse = new ToolStripButton();
			this.tbPWareHouse = new ToolStripButton();
			this.tbAlarm = new ToolStripButton();
			this.tbSetUp = new ToolStripButton();
			this.tbLock = new ToolStripButton();
			this.tbCalc = new ToolStripButton();
			this.tblogOut = new ToolStripButton();
			this.tbHelp = new ToolStripButton();
			this.toolStripLabelAgency = new ToolStripLabel();
			this.panelToolsBar = new Panel();
			this.panel1 = new Panel();
			this.dgvFirmInfo = new DataGridView();
			this.m_tsTMFToolBar.SuspendLayout();
			this.panelToolsBar.SuspendLayout();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.dgvFirmInfo).BeginInit();
			base.SuspendLayout();
			this.m_tsTMFToolBar.BackColor = Color.FromArgb(100, 200, 220);
			this.m_tsTMFToolBar.Dock = DockStyle.Fill;
			this.m_tsTMFToolBar.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.m_tsTMFToolBar.GripMargin = new Padding(0);
			this.m_tsTMFToolBar.GripStyle = ToolStripGripStyle.Hidden;
			this.m_tsTMFToolBar.Items.AddRange(new ToolStripItem[]
			{
				this.tbCreatWareHouse,
				this.tbPWareHouse,
				this.tbAlarm,
				this.tbSetUp,
				this.tbLock,
				this.tbCalc,
				this.tblogOut,
				this.tbHelp,
				this.toolStripLabelAgency
			});
			this.m_tsTMFToolBar.Location = new Point(0, 0);
			this.m_tsTMFToolBar.Name = "m_tsTMFToolBar";
			this.m_tsTMFToolBar.Padding = new Padding(0);
			this.m_tsTMFToolBar.Size = new Size(320, 35);
			this.m_tsTMFToolBar.TabIndex = 2;
			this.m_tsTMFToolBar.Text = "toolStrip1";
			this.m_tsTMFToolBar.Paint += new PaintEventHandler(this.m_tsTMFToolBar_Paint);
			this.tbCreatWareHouse.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbCreatWareHouse.ImageTransparentColor = Color.Magenta;
			this.tbCreatWareHouse.Margin = new Padding(0);
			this.tbCreatWareHouse.MergeIndex = 1;
			this.tbCreatWareHouse.Name = "tbCreatWareHouse";
			this.tbCreatWareHouse.Size = new Size(32, 35);
			this.tbCreatWareHouse.Tag = "sssToolStripMenuItem";
			this.tbCreatWareHouse.Text = "xxx";
			this.tbCreatWareHouse.TextAlign = ContentAlignment.MiddleRight;
			this.tbCreatWareHouse.Click += new EventHandler(this.tbCreatWareHouse_Click);
			this.tbPWareHouse.Image = (Image)componentResourceManager.GetObject("tbPWareHouse.Image");
			this.tbPWareHouse.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbPWareHouse.ImageTransparentColor = Color.Magenta;
			this.tbPWareHouse.Margin = new Padding(0);
			this.tbPWareHouse.Name = "tbPWareHouse";
			this.tbPWareHouse.Size = new Size(55, 35);
			this.tbPWareHouse.Text = "toqq";
			this.tbPWareHouse.TextAlign = ContentAlignment.MiddleRight;
			this.tbPWareHouse.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbPWareHouse.Click += new EventHandler(this.tbPWareHouse_Click);
			this.tbAlarm.Image = (Image)componentResourceManager.GetObject("tbAlarm.Image");
			this.tbAlarm.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbAlarm.ImageTransparentColor = Color.Magenta;
			this.tbAlarm.Margin = new Padding(0);
			this.tbAlarm.Name = "tbAlarm";
			this.tbAlarm.Size = new Size(55, 35);
			this.tbAlarm.Text = "toqq";
			this.tbAlarm.TextAlign = ContentAlignment.MiddleRight;
			this.tbAlarm.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbAlarm.Click += new EventHandler(this.tbAlarm_Click);
			this.tbSetUp.Image = (Image)componentResourceManager.GetObject("tbSetUp.Image");
			this.tbSetUp.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbSetUp.ImageTransparentColor = Color.Magenta;
			this.tbSetUp.Margin = new Padding(0);
			this.tbSetUp.Name = "tbSetUp";
			this.tbSetUp.Size = new Size(55, 35);
			this.tbSetUp.Text = "toqq";
			this.tbSetUp.TextAlign = ContentAlignment.MiddleRight;
			this.tbSetUp.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbSetUp.Click += new EventHandler(this.tbSetUp_Click);
			this.tbLock.Image = (Image)componentResourceManager.GetObject("tbLock.Image");
			this.tbLock.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbLock.ImageTransparentColor = Color.Magenta;
			this.tbLock.Margin = new Padding(0);
			this.tbLock.Name = "tbLock";
			this.tbLock.Size = new Size(55, 35);
			this.tbLock.Text = "toqq";
			this.tbLock.TextAlign = ContentAlignment.MiddleRight;
			this.tbLock.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbLock.Click += new EventHandler(this.tbLock_Click);
			this.tbCalc.Image = (Image)componentResourceManager.GetObject("tbCalc.Image");
			this.tbCalc.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbCalc.ImageTransparentColor = Color.Magenta;
			this.tbCalc.Margin = new Padding(0);
			this.tbCalc.Name = "tbCalc";
			this.tbCalc.Size = new Size(55, 20);
			this.tbCalc.Text = "toqq";
			this.tbCalc.TextAlign = ContentAlignment.MiddleRight;
			this.tbCalc.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbCalc.Click += new EventHandler(this.tbCalc_Click);
			this.tblogOut.ImageAlign = ContentAlignment.TopCenter;
			this.tblogOut.ImageScaling = ToolStripItemImageScaling.None;
			this.tblogOut.ImageTransparentColor = Color.Magenta;
			this.tblogOut.Name = "tblogOut";
			this.tblogOut.Size = new Size(67, 18);
			this.tblogOut.Text = "注销客户";
			this.tblogOut.TextAlign = ContentAlignment.BottomLeft;
			this.tblogOut.TextImageRelation = TextImageRelation.ImageAboveText;
			this.tblogOut.Click += new EventHandler(this.tblogOut_Click);
			this.tbHelp.BackColor = Color.FromArgb(100, 200, 220);
			this.tbHelp.Image = (Image)componentResourceManager.GetObject("tbHelp.Image");
			this.tbHelp.ImageAlign = ContentAlignment.MiddleLeft;
			this.tbHelp.ImageTransparentColor = Color.Magenta;
			this.tbHelp.Name = "tbHelp";
			this.tbHelp.Size = new Size(39, 34);
			this.tbHelp.Text = "toqq";
			this.tbHelp.TextAlign = ContentAlignment.MiddleRight;
			this.tbHelp.TextDirection = ToolStripTextDirection.Horizontal;
			this.tbHelp.TextImageRelation = TextImageRelation.ImageAboveText;
			this.tbHelp.Visible = false;
			this.tbHelp.Click += new EventHandler(this.tbHelp_Click);
			this.toolStripLabelAgency.Alignment = ToolStripItemAlignment.Right;
			this.toolStripLabelAgency.BackColor = Color.FromArgb(100, 200, 220);
			this.toolStripLabelAgency.Font = new Font("宋体", 12f, FontStyle.Bold);
			this.toolStripLabelAgency.ForeColor = Color.Red;
			this.toolStripLabelAgency.ImageAlign = ContentAlignment.MiddleLeft;
			this.toolStripLabelAgency.Name = "toolStripLabelAgency";
			this.toolStripLabelAgency.Size = new Size(76, 16);
			this.toolStripLabelAgency.Text = "电话下单";
			this.toolStripLabelAgency.TextAlign = ContentAlignment.MiddleRight;
			this.panelToolsBar.Controls.Add(this.m_tsTMFToolBar);
			this.panelToolsBar.Dock = DockStyle.Fill;
			this.panelToolsBar.Location = new Point(0, 0);
			this.panelToolsBar.Margin = new Padding(0);
			this.panelToolsBar.Name = "panelToolsBar";
			this.panelToolsBar.Size = new Size(320, 35);
			this.panelToolsBar.TabIndex = 3;
			this.panel1.BackColor = Color.FromArgb(100, 200, 220);
			this.panel1.Controls.Add(this.dgvFirmInfo);
			this.panel1.Dock = DockStyle.Right;
			this.panel1.Location = new Point(320, 0);
			this.panel1.Margin = new Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(700, 35);
			this.panel1.TabIndex = 4;
			this.dgvFirmInfo.AllowUserToAddRows = false;
			this.dgvFirmInfo.AllowUserToDeleteRows = false;
			this.dgvFirmInfo.AllowUserToResizeColumns = false;
			this.dgvFirmInfo.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = Color.White;
			this.dgvFirmInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dgvFirmInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dgvFirmInfo.BackgroundColor = Color.FromArgb(100, 200, 220);
			this.dgvFirmInfo.BorderStyle = BorderStyle.None;
			this.dgvFirmInfo.CellBorderStyle = DataGridViewCellBorderStyle.None;
			this.dgvFirmInfo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 200, 220);
			dataGridViewCellStyle2.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dgvFirmInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvFirmInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = Color.FromArgb(100, 200, 220);
			dataGridViewCellStyle3.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = Color.White;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
			this.dgvFirmInfo.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvFirmInfo.Dock = DockStyle.Fill;
			this.dgvFirmInfo.Enabled = false;
			this.dgvFirmInfo.Location = new Point(0, 0);
			this.dgvFirmInfo.Margin = new Padding(0);
			this.dgvFirmInfo.MultiSelect = false;
			this.dgvFirmInfo.Name = "dgvFirmInfo";
			this.dgvFirmInfo.ReadOnly = true;
			this.dgvFirmInfo.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = Color.FromArgb(100, 200, 220);
			dataGridViewCellStyle4.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = Color.White;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
			this.dgvFirmInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvFirmInfo.RowHeadersVisible = false;
			this.dgvFirmInfo.RowHeadersWidth = 23;
			this.dgvFirmInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle5.BackColor = Color.FromArgb(100, 200, 220);
			dataGridViewCellStyle5.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = Color.White;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			this.dgvFirmInfo.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvFirmInfo.RowTemplate.Height = 16;
			this.dgvFirmInfo.ScrollBars = ScrollBars.None;
			this.dgvFirmInfo.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvFirmInfo.Size = new Size(700, 35);
			this.dgvFirmInfo.TabIndex = 4;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.FromArgb(100, 200, 220);
			base.Controls.Add(this.panelToolsBar);
			base.Controls.Add(this.panel1);
			this.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			base.Name = "ToolsBarControl";
			base.Size = new Size(1020, 35);
			base.Load += new EventHandler(this.ToolsBarControl_Load);
			this.m_tsTMFToolBar.ResumeLayout(false);
			this.m_tsTMFToolBar.PerformLayout();
			this.panelToolsBar.ResumeLayout(false);
			this.panelToolsBar.PerformLayout();
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.dgvFirmInfo).EndInit();
			base.ResumeLayout(false);
		}
	}
}
