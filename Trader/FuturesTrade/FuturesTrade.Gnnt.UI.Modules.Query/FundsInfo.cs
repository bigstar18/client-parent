using FuturesTrade.Gnnt.BLL.Manager;
using FuturesTrade.Gnnt.BLL.Query;
using FuturesTrade.Gnnt.Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.UI.Modules.Query
{
	public class FundsInfo : UserControl
	{
		private delegate void FillHolding(FirmInfoResponseVO dt);
		private IContainer components;
		private GroupBox groupBoxFunds;
		private Button buttonFundsTransfer;
		private Button buttonSelFundsF4;
		private ListView lstVFunds;
		private OperationManager operationManager = OperationManager.GetInstance();
		private FundsInfo.FillHolding fillHolding;
		private FundsItemInfo fundsItemInfo = new FundsItemInfo();
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
			this.groupBoxFunds = new GroupBox();
			this.buttonFundsTransfer = new Button();
			this.buttonSelFundsF4 = new Button();
			this.lstVFunds = new ListView();
			this.groupBoxFunds.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxFunds.AutoSize = true;
			this.groupBoxFunds.Controls.Add(this.buttonFundsTransfer);
			this.groupBoxFunds.Controls.Add(this.buttonSelFundsF4);
			this.groupBoxFunds.Controls.Add(this.lstVFunds);
			this.groupBoxFunds.Dock = DockStyle.Fill;
			this.groupBoxFunds.Location = new Point(0, 0);
			this.groupBoxFunds.Name = "groupBoxFunds";
			this.groupBoxFunds.Size = new Size(700, 200);
			this.groupBoxFunds.TabIndex = 4;
			this.groupBoxFunds.TabStop = false;
			this.groupBoxFunds.Text = "资金查询";
			this.buttonFundsTransfer.BackColor = SystemColors.ControlDark;
			this.buttonFundsTransfer.Font = new Font("宋体", 9f, FontStyle.Bold);
			this.buttonFundsTransfer.ForeColor = Color.Maroon;
			this.buttonFundsTransfer.ImeMode = ImeMode.NoControl;
			this.buttonFundsTransfer.Location = new Point(176, 8);
			this.buttonFundsTransfer.Name = "buttonFundsTransfer";
			this.buttonFundsTransfer.Size = new Size(100, 20);
			this.buttonFundsTransfer.TabIndex = 2;
			this.buttonFundsTransfer.Text = "资金划转";
			this.buttonFundsTransfer.UseVisualStyleBackColor = false;
			this.buttonSelFundsF4.ImeMode = ImeMode.NoControl;
			this.buttonSelFundsF4.Location = new Point(388, 8);
			this.buttonSelFundsF4.Name = "buttonSelFundsF4";
			this.buttonSelFundsF4.Size = new Size(57, 20);
			this.buttonSelFundsF4.TabIndex = 1;
			this.buttonSelFundsF4.Text = "刷新";
			this.buttonSelFundsF4.UseVisualStyleBackColor = true;
			this.buttonSelFundsF4.Click += new EventHandler(this.buttonSelFundsF4_Click);
			this.lstVFunds.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lstVFunds.FullRowSelect = true;
			this.lstVFunds.GridLines = true;
			this.lstVFunds.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.lstVFunds.Location = new Point(4, 28);
			this.lstVFunds.Name = "lstVFunds";
			this.lstVFunds.Size = new Size(691, 169);
			this.lstVFunds.TabIndex = 0;
			this.lstVFunds.TabStop = false;
			this.lstVFunds.UseCompatibleStateImageBehavior = false;
			this.lstVFunds.View = View.Details;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxFunds);
			base.Name = "FundsInfo";
			base.Size = new Size(700, 200);
			base.Load += new EventHandler(this.FundsInfo_Load);
			this.groupBoxFunds.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public FundsInfo()
		{
			this.InitializeComponent();
			this.operationManager.queryInitDataOperation.FundsFill = new QueryInitDataOperation.FundsFillCallBack(this.HoldingInfoFill);
			this.CreateHandle();
		}
		private void HoldingInfoFill(FirmInfoResponseVO firmInfoResponseVO)
		{
			try
			{
				this.fillHolding = new FundsInfo.FillHolding(this.LstVFundsFill);
				this.HandleCreated();
				base.Invoke(this.fillHolding, new object[]
				{
					firmInfoResponseVO
				});
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		public new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void LstVFundsFill(object _firmInfoResponseVO)
		{
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(1, 15);
			this.lstVFunds.SmallImageList = imageList;
			FirmInfoResponseVO firmInfoResponseVO = (FirmInfoResponseVO)_firmInfoResponseVO;
			string text = string.Empty;
			for (int i = 0; i < firmInfoResponseVO.CDS.Count; i++)
			{
				Code code = firmInfoResponseVO.CDS[i];
				text = text + code.CD + "/";
			}
			string text2 = firmInfoResponseVO.FirmID + "(" + firmInfoResponseVO.FirmName.Trim() + ")";
			this.lstVFunds.Clear();
			try
			{
				string @string = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_Project");
				string string2 = Global.M_ResourceManager.GetString("TradeStr_MainFormF6_ProjectValue");
				string[] array = this.fundsItemInfo.m_strItems.Split(new char[]
				{
					'|'
				});
				for (int j = 0; j < array.Length; j++)
				{
					this.lstVFunds.Columns.Add(@string, 200, HorizontalAlignment.Left);
					this.lstVFunds.Columns.Add(string2, 140, HorizontalAlignment.Right);
					this.groupBoxFunds.Width = 354 * (j + 1);
					string[] array2 = array[j].Split(new char[]
					{
						';'
					});
					for (int k = 0; k < array2.Length; k++)
					{
						ListViewItem listViewItem = null;
						if (k < this.lstVFunds.Items.Count)
						{
							listViewItem = this.lstVFunds.Items[k];
						}
						if (array2[k].Equals("FirmID"))
						{
							ColItemInfo colItemInfo = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FirmID"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo.name);
							}
							listViewItem.SubItems.Add(text2);
						}
						if (array2[k].Equals("Jysqy"))
						{
							ColItemInfo colItemInfo2 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Jysqy"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo2.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo2.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.InitFund.ToString(colItemInfo2.format));
						}
						if (array2[k].Equals("HoldGain"))
						{
							ColItemInfo colItemInfo3 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["HoldGain"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo3.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo3.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.YesterdayBail.ToString(colItemInfo3.format));
						}
						if (array2[k].Equals("DynRight"))
						{
							ColItemInfo colItemInfo4 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["DynRight"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo4.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo4.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.YesterdayFL.ToString(colItemInfo4.format));
						}
						if (array2[k].Equals("Margin"))
						{
							ColItemInfo colItemInfo5 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Margin"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo5.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo5.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurrentBail.ToString(colItemInfo5.format));
						}
						if (array2[k].Equals("Lfpl"))
						{
							ColItemInfo colItemInfo6 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Lfpl"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo6.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo6.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurrentFL.ToString(colItemInfo6.format));
						}
						if (array2[k].Equals("FreezFund"))
						{
							ColItemInfo colItemInfo7 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FreezFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo7.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo7.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OrderFrozenFund.ToString(colItemInfo7.format));
						}
						if (array2[k].Equals("FreezComm"))
						{
							ColItemInfo colItemInfo8 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["FreezComm"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo8.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo8.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OtherFrozenFund.ToString(colItemInfo8.format));
						}
						if (array2[k].Equals("RealFunds"))
						{
							ColItemInfo colItemInfo9 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["RealFunds"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo9.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo9.name);
							}
							listViewItem.ForeColor = Color.Blue;
							listViewItem.SubItems.Add(firmInfoResponseVO.RealFund.ToString(colItemInfo9.format));
						}
						if (array2[k].Equals("Comm"))
						{
							ColItemInfo colItemInfo10 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Comm"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo10.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo10.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.Fee.ToString(colItemInfo10.format));
						}
						if (array2[k].Equals("TransLiqpl"))
						{
							ColItemInfo colItemInfo11 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["TransLiqpl"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo11.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo11.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.TransferPL.ToString(colItemInfo11.format));
						}
						if (array2[k].Equals("AddFund"))
						{
							ColItemInfo colItemInfo12 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["AddFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo12.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo12.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurrentRight.ToString(colItemInfo12.format));
						}
						if (array2[k].Equals("Ioamt"))
						{
							ColItemInfo colItemInfo13 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Ioamt"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo13.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo13.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.InOutFund.ToString(colItemInfo13.format));
						}
						if (array2[k].Equals("MarginAmt"))
						{
							ColItemInfo colItemInfo14 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["MarginAmt"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo14.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo14.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.HoldingPL.ToString(colItemInfo14.format));
						}
						if (array2[k].Equals("ChkFund"))
						{
							ColItemInfo colItemInfo15 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["ChkFund"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo15.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo15.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.OtherChange.ToString(colItemInfo15.format));
						}
						if (array2[k].Equals("TTlMargin"))
						{
							ColItemInfo colItemInfo16 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["TTlMargin"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo16.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo16.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.ImpawnFund.ToString(colItemInfo16.format));
						}
						if (array2[k].Equals("Status"))
						{
							ColItemInfo colItemInfo17 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Status"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo17.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo17.name);
							}
							listViewItem.SubItems.Add(Global.FirmStatusStrArr[(int)firmInfoResponseVO.Status]);
						}
						if (array2[k].Equals("Cur_Open"))
						{
							ColItemInfo colItemInfo18 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Cur_Open"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo18.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo18.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurrentOpen.ToString());
						}
						if (array2[k].Equals("Virtual_Open"))
						{
							ColItemInfo colItemInfo19 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Virtual_Open"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo19.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo19.name);
							}
							listViewItem.SubItems.Add(firmInfoResponseVO.CurMHolding.ToString());
						}
						if (array2[k].Equals("Zjaql"))
						{
							ColItemInfo colItemInfo20 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Zjaql"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo20.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo20.name);
							}
							double num;
							if (firmInfoResponseVO.CurrentBail <= 0.0)
							{
								num = 999.99;
							}
							else
							{
								num = (firmInfoResponseVO.RealFund + firmInfoResponseVO.CurrentBail) * 100.0 / firmInfoResponseVO.CurrentBail;
							}
							if (num > 999.999)
							{
								num = 999.99;
							}
							else
							{
								if (num > 99.99 && num < 100.0)
								{
									num = 99.99;
								}
							}
							listViewItem.SubItems.Add(num.ToString(colItemInfo20.format) + "%");
						}
						if (array2[k].Equals("Code") && text.Length > 0)
						{
							ColItemInfo colItemInfo21 = (ColItemInfo)this.fundsItemInfo.m_htItemInfo["Code"];
							if (listViewItem == null)
							{
								listViewItem = new ListViewItem(colItemInfo21.name);
								this.lstVFunds.Items.Add(listViewItem);
							}
							else
							{
								listViewItem.SubItems.Add(colItemInfo21.name);
							}
							listViewItem.SubItems.Add(text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
		}
		private void FundsInfo_Load(object sender, EventArgs e)
		{
			this.buttonFundsTransfer.Visible = false;
		}
		private void buttonSelFundsF4_Click(object sender, EventArgs e)
		{
			this.operationManager.queryInitDataOperation.ButtonRefreshFlag = 1;
			this.operationManager.queryInitDataOperation.QueryFirmInfoThread();
			this.operationManager.IdleRefreshButton = 0;
		}
	}
}
