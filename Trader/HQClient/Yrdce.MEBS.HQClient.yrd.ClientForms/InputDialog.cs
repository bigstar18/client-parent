using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class InputDialog : Form
	{
		public const int WM_SYSCOMMAND = 274;
		public const int SC_MOVE = 61456;
		public const int HTCAPTION = 2;
		private IContainer components;
		private MenuStrip menuStrip1;
		private PictureBox pictureBox1;
		private TextBox textBoxInput;
		private ListBox listBoxDisplay;
		public string strCmd;
		private HQForm m_hqForm;
		private ArrayList aString = new ArrayList();
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
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
			this.pictureBox1 = new PictureBox();
			this.menuStrip1 = new MenuStrip();
			this.textBoxInput = new TextBox();
			this.listBoxDisplay = new ListBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
			this.pictureBox1.Cursor = Cursors.Hand;
			this.pictureBox1.Location = new Point(148, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(19, 16);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
			this.menuStrip1.Location = new Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new Size(170, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			this.menuStrip1.MouseDown += new MouseEventHandler(this.menuStrip1_MouseDown);
			this.textBoxInput.CharacterCasing = CharacterCasing.Upper;
			this.textBoxInput.Dock = DockStyle.Top;
			this.textBoxInput.Location = new Point(0, 24);
			this.textBoxInput.MaxLength = 8;
			this.textBoxInput.Name = "textBoxInput";
			this.textBoxInput.Size = new Size(170, 21);
			this.textBoxInput.TabIndex = 2;
			this.textBoxInput.TextChanged += new EventHandler(this.textBoxInput_TextChanged);
			this.textBoxInput.KeyDown += new KeyEventHandler(this.textBoxInput_KeyDown);
			this.listBoxDisplay.Dock = DockStyle.Fill;
			this.listBoxDisplay.FormattingEnabled = true;
			this.listBoxDisplay.ItemHeight = 12;
			this.listBoxDisplay.Location = new Point(0, 45);
			this.listBoxDisplay.Name = "listBoxDisplay";
			this.listBoxDisplay.Size = new Size(170, 163);
			this.listBoxDisplay.TabIndex = 3;
			this.listBoxDisplay.DoubleClick += new EventHandler(this.listBoxDisplay_DoubleClick);
			this.listBoxDisplay.KeyDown += new KeyEventHandler(this.textBoxInput_KeyDown);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(170, 208);
			base.ControlBox = false;
			base.Controls.Add(this.listBoxDisplay);
			base.Controls.Add(this.textBoxInput);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.menuStrip1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "InputDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.Manual;
			this.Text = "InputDialog";
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public InputDialog(char ch, HQForm hqForm)
		{
			this.InitializeComponent();
			this.m_hqForm = hqForm;
			this.pluginInfo = hqForm.CurHQClient.pluginInfo;
			this.setInfo = hqForm.CurHQClient.setInfo;
			this.pictureBox1.Image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_closeDlg");
			this.menuStrip1.BackgroundImage = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_jianpanjl");
			this.listBoxDisplay.ForeColor = SetInfo.RHColor.clHighlight;
			this.textBoxInput.Text = ch.ToString().ToUpper();
			this.textBoxInput.Select(this.textBoxInput.Text.Length, 0);
			this.DealString();
		}
		private void DealString()
		{
			this.listBoxDisplay.Items.Clear();
			this.aString.Clear();
			string text = this.textBoxInput.Text;
			if (text.Length == 0)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			if (char.IsDigit(text, 0) && text.Length <= 2)
			{
				this.DealAccelerator(text);
			}
			this.DealProductCode(text);
			this.DealProductName(text);
			if (this.m_hqForm.CurHQClient.CurrentPage == 2)
			{
				this.DealIndicator(text);
			}
			string text2 = "COLOR";
			if (text2.StartsWith(text))
			{
				this.listBoxDisplay.Items.Add("COLOR0 " + this.pluginInfo.HQResourceManager.GetString("HQStr_CLASSICAL"));
				this.listBoxDisplay.Items.Add("COLOR1 " + this.pluginInfo.HQResourceManager.GetString("HQStr_MODERN"));
				this.listBoxDisplay.Items.Add("COLOR2 " + this.pluginInfo.HQResourceManager.GetString("HQStr_ELEGANCE"));
				this.listBoxDisplay.Items.Add("COLOR3 " + this.pluginInfo.HQResourceManager.GetString("HQStr_SOFTNESS"));
				this.listBoxDisplay.Items.Add("COLOR4 " + this.pluginInfo.HQResourceManager.GetString("HQStr_DIGNITY"));
				this.aString.Add("Color" + 0);
				this.aString.Add("Color" + 1);
				this.aString.Add("Color" + 2);
				this.aString.Add("Color" + 3);
				this.aString.Add("Color" + 4);
			}
			if (this.listBoxDisplay.Items.Count > 0)
			{
				this.listBoxDisplay.SelectedIndex = 0;
			}
		}
		private void DealProductCode(string str)
		{
			HQClientMain curHQClient = this.m_hqForm.CurHQClient;
			int count = curHQClient.m_codeList.Count;
			for (int i = 0; i < count; i++)
			{
				CommodityInfo commodityInfo = (CommodityInfo)curHQClient.m_codeList[i];
				if (commodityInfo.commodityCode.ToUpper().IndexOf(str) >= 0)
				{
					CodeTable codeTable = (CodeTable)curHQClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
					if (curHQClient.CurrentPage == 2 || curHQClient.CurrentPage == 6 || (codeTable.status != 1 && codeTable.status != 4))
					{
						this.listBoxDisplay.Items.Add(commodityInfo.commodityCode + " " + codeTable.sName);
						this.aString.Add("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode);
					}
				}
			}
		}
		private void DealProductName(string str)
		{
			HQClientMain curHQClient = this.m_hqForm.CurHQClient;
			int count = curHQClient.m_codeList.Count;
			for (int i = 0; i < count; i++)
			{
				CommodityInfo commodityInfo = (CommodityInfo)curHQClient.m_codeList[i];
				CodeTable codeTable = (CodeTable)curHQClient.m_htProduct[commodityInfo.marketID + commodityInfo.commodityCode];
				if (curHQClient.CurrentPage == 2 || curHQClient.CurrentPage == 6 || (codeTable.status != 1 && codeTable.status != 4))
				{
					for (int j = 0; j < codeTable.sPinyin.Length; j++)
					{
						if (codeTable.sPinyin[j].StartsWith(str))
						{
							bool flag = false;
							for (int k = 0; k < this.aString.Count; k++)
							{
								if (this.aString[k].Equals("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode))
								{
									flag = true;
								}
							}
							if (!flag)
							{
								this.listBoxDisplay.Items.Add(codeTable.sPinyin[j] + " " + codeTable.sName);
								this.aString.Add("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode);
							}
						}
					}
				}
			}
		}
		private void DealAccelerator(string str)
		{
			for (int i = 0; i < 89; i++)
			{
				string text = i.ToString();
				if (text.Length == 1)
				{
					text = "0" + text;
				}
				if ((str.Length == 1 && str[0] == text[0]) || (str[0] == '\u0002' && str.Equals(text)))
				{
					int count = this.listBoxDisplay.Items.Count;
					int num = i;
					if (num <= 6)
					{
						if (num != 1)
						{
							switch (num)
							{
							case 5:
								this.listBoxDisplay.Items.Add("05 分时\\日线");
								break;
							case 6:
								this.listBoxDisplay.Items.Add("06 自选股报价");
								break;
							}
						}
						else
						{
							if (this.m_hqForm.CurHQClient.CurrentPage == 1 || this.m_hqForm.CurHQClient.CurrentPage == 2)
							{
								this.listBoxDisplay.Items.Add("01 成交明细");
							}
						}
					}
					else
					{
						if (num != 60)
						{
							if (num != 70)
							{
								if (num == 80)
								{
									this.listBoxDisplay.Items.Add("80 综合排名");
								}
							}
							else
							{
								this.listBoxDisplay.Items.Add("70 历史商品");
							}
						}
						else
						{
							this.listBoxDisplay.Items.Add("60 涨幅排名");
						}
					}
					if (count < this.listBoxDisplay.Items.Count)
					{
						this.aString.Add("A" + text);
					}
				}
			}
		}
		private void DealIndicator(string str)
		{
			for (int i = 0; i < IndicatorBase.INDICATOR_NAME.GetLength(0); i++)
			{
				if (IndicatorBase.INDICATOR_NAME[i, 0].StartsWith(str))
				{
					string text = this.pluginInfo.HQResourceManager.GetString("HQStr_T_" + IndicatorBase.INDICATOR_NAME[i, 0]);
					if (text == null || text.Length == 0)
					{
						text = IndicatorBase.INDICATOR_NAME[i, 1];
					}
					this.listBoxDisplay.Items.Add(IndicatorBase.INDICATOR_NAME[i, 0] + " " + ((text == null) ? IndicatorBase.INDICATOR_NAME[i, 0] : text.Substring(0, Math.Min(6, text.Length))));
					this.aString.Add("T" + IndicatorBase.INDICATOR_NAME[i, 0]);
				}
			}
		}
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
		private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized && e.Clicks == 1)
			{
				InputDialog.ReleaseCapture();
				InputDialog.SendMessage(base.Handle, 274, 61458, 0);
			}
		}
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}
		private void textBoxInput_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keyData = e.KeyData;
			if (keyData == Keys.Return)
			{
				this.strCmd = "";
				int selectedIndex = this.listBoxDisplay.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < this.aString.Count)
				{
					this.strCmd = (string)this.aString[selectedIndex];
				}
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (keyData != Keys.Escape)
			{
				switch (keyData)
				{
				case Keys.Up:
					if (this.listBoxDisplay.SelectedIndex > 0)
					{
						this.listBoxDisplay.SelectedIndex--;
					}
					e.Handled = true;
					return;
				case Keys.Right:
					break;
				case Keys.Down:
					if (this.listBoxDisplay.SelectedIndex < this.listBoxDisplay.Items.Count - 1)
					{
						this.listBoxDisplay.SelectedIndex++;
					}
					e.Handled = true;
					break;
				default:
					return;
				}
				return;
			}
			base.DialogResult = DialogResult.Cancel;
		}
		private void textBoxInput_TextChanged(object sender, EventArgs e)
		{
			this.DealString();
		}
		private void listBoxDisplay_DoubleClick(object sender, EventArgs e)
		{
			this.strCmd = "";
			int selectedIndex = this.listBoxDisplay.SelectedIndex;
			if (selectedIndex >= 0 && selectedIndex < this.aString.Count)
			{
				this.strCmd = (string)this.aString[selectedIndex];
			}
			base.DialogResult = DialogResult.OK;
		}
	}
}
