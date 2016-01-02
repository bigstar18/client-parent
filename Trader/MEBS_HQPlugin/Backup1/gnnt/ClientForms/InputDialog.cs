// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.InputDialog
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

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
    private ArrayList aString = new ArrayList();
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
    private PluginInfo pluginInfo;
    private SetInfo setInfo;

    public InputDialog(char ch, HQForm hqForm)
    {
      this.InitializeComponent();
      this.m_hqForm = hqForm;
      this.pluginInfo = hqForm.CurHQClient.pluginInfo;
      this.setInfo = hqForm.CurHQClient.setInfo;
      this.pictureBox1.Image = (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_closeDlg");
      this.menuStrip1.BackgroundImage = (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_jianpanjl");
      this.listBoxDisplay.ForeColor = SetInfo.RHColor.clHighlight;
      this.textBoxInput.Text = ch.ToString().ToUpper();
      this.textBoxInput.Select(this.textBoxInput.Text.Length, 0);
      this.DealString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBox1 = new PictureBox();
      this.menuStrip1 = new MenuStrip();
      this.textBoxInput = new TextBox();
      this.listBoxDisplay = new ListBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
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
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(170, 208);
      this.ControlBox = false;
      this.Controls.Add((Control) this.listBoxDisplay);
      this.Controls.Add((Control) this.textBoxInput);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.menuStrip1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "InputDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "InputDialog";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void DealString()
    {
      this.listBoxDisplay.Items.Clear();
      this.aString.Clear();
      string text = this.textBoxInput.Text;
      if (text.Length == 0)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        if (char.IsDigit(text, 0) && text.Length <= 2)
          this.DealAccelerator(text);
        this.DealProductCode(text);
        this.DealProductName(text);
        if (this.m_hqForm.CurHQClient.CurrentPage == 2)
          this.DealIndicator(text);
        if ("COLOR".StartsWith(text))
        {
          this.listBoxDisplay.Items.Add((object) ("COLOR0 " + this.pluginInfo.HQResourceManager.GetString("HQStr_CLASSICAL")));
          this.listBoxDisplay.Items.Add((object) ("COLOR1 " + this.pluginInfo.HQResourceManager.GetString("HQStr_MODERN")));
          this.listBoxDisplay.Items.Add((object) ("COLOR2 " + this.pluginInfo.HQResourceManager.GetString("HQStr_ELEGANCE")));
          this.listBoxDisplay.Items.Add((object) ("COLOR3 " + this.pluginInfo.HQResourceManager.GetString("HQStr_SOFTNESS")));
          this.listBoxDisplay.Items.Add((object) ("COLOR4 " + this.pluginInfo.HQResourceManager.GetString("HQStr_DIGNITY")));
          this.aString.Add((object) ("Color" + (object) 0));
          this.aString.Add((object) ("Color" + (object) 1));
          this.aString.Add((object) ("Color" + (object) 2));
          this.aString.Add((object) ("Color" + (object) 3));
          this.aString.Add((object) ("Color" + (object) 4));
        }
        if (this.listBoxDisplay.Items.Count <= 0)
          return;
        this.listBoxDisplay.SelectedIndex = 0;
      }
    }

    private void DealProductCode(string str)
    {
      HQClientMain curHqClient = this.m_hqForm.CurHQClient;
      int count = curHqClient.m_codeList.Count;
      for (int index = 0; index < count; ++index)
      {
        CommodityInfo commodityInfo = (CommodityInfo) curHqClient.m_codeList[index];
        if (commodityInfo.commodityCode.ToUpper().IndexOf(str) >= 0)
        {
          CodeTable codeTable = (CodeTable) curHqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
          if (curHqClient.CurrentPage == 2 || curHqClient.CurrentPage == 6 || codeTable.status != 1 && codeTable.status != 4)
          {
            this.listBoxDisplay.Items.Add((object) (commodityInfo.commodityCode + " " + codeTable.sName));
            this.aString.Add((object) ("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode));
          }
        }
      }
    }

    private void DealProductName(string str)
    {
      HQClientMain curHqClient = this.m_hqForm.CurHQClient;
      int count = curHqClient.m_codeList.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        CommodityInfo commodityInfo = (CommodityInfo) curHqClient.m_codeList[index1];
        CodeTable codeTable = (CodeTable) curHqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
        if (curHqClient.CurrentPage == 2 || curHqClient.CurrentPage == 6 || codeTable.status != 1 && codeTable.status != 4)
        {
          for (int index2 = 0; index2 < codeTable.sPinyin.Length; ++index2)
          {
            if (codeTable.sPinyin[index2].StartsWith(str))
            {
              bool flag = false;
              for (int index3 = 0; index3 < this.aString.Count; ++index3)
              {
                if (this.aString[index3].Equals((object) ("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode)))
                  flag = true;
              }
              if (!flag)
              {
                this.listBoxDisplay.Items.Add((object) (codeTable.sPinyin[index2] + " " + codeTable.sName));
                this.aString.Add((object) ("P" + commodityInfo.marketID + "_" + commodityInfo.commodityCode));
              }
            }
          }
        }
      }
    }

    private void DealAccelerator(string str)
    {
      for (int index = 0; index < 89; ++index)
      {
        string str1 = index.ToString();
        if (str1.Length == 1)
          str1 = "0" + str1;
        if (str.Length == 1 && (int) str[0] == (int) str1[0] || (int) str[0] == 2 && str.Equals(str1))
        {
          int count = this.listBoxDisplay.Items.Count;
          switch (index)
          {
            case 60:
              this.listBoxDisplay.Items.Add((object) "60 涨幅排名");
              break;
            case 70:
              this.listBoxDisplay.Items.Add((object) "70 历史商品");
              break;
            case 80:
              this.listBoxDisplay.Items.Add((object) "80 综合排名");
              break;
            case 1:
              if (this.m_hqForm.CurHQClient.CurrentPage == 1 || this.m_hqForm.CurHQClient.CurrentPage == 2)
              {
                this.listBoxDisplay.Items.Add((object) "01 成交明细");
                break;
              }
              break;
            case 5:
              this.listBoxDisplay.Items.Add((object) "05 分时\\日线");
              break;
            case 6:
              this.listBoxDisplay.Items.Add((object) "06 自选股报价");
              break;
          }
          if (count < this.listBoxDisplay.Items.Count)
            this.aString.Add((object) ("A" + str1));
        }
      }
    }

    private void DealIndicator(string str)
    {
      for (int index = 0; index < IndicatorBase.INDICATOR_NAME.GetLength(0); ++index)
      {
        if (IndicatorBase.INDICATOR_NAME[index, 0].StartsWith(str))
        {
          string str1 = this.pluginInfo.HQResourceManager.GetString("HQStr_T_" + IndicatorBase.INDICATOR_NAME[index, 0]);
          if (str1 == null || str1.Length == 0)
            str1 = IndicatorBase.INDICATOR_NAME[index, 1];
          this.listBoxDisplay.Items.Add((object) (IndicatorBase.INDICATOR_NAME[index, 0] + " " + (str1 == null ? IndicatorBase.INDICATOR_NAME[index, 0] : str1.Substring(0, Math.Min(6, str1.Length)))));
          this.aString.Add((object) ("T" + IndicatorBase.INDICATOR_NAME[index, 0]));
        }
      }
    }

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.WindowState == FormWindowState.Maximized || e.Clicks != 1)
        return;
      InputDialog.ReleaseCapture();
      InputDialog.SendMessage(this.Handle, 274, 61458, 0);
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void textBoxInput_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyData)
      {
        case Keys.Return:
          this.strCmd = "";
          int selectedIndex = this.listBoxDisplay.SelectedIndex;
          if (selectedIndex >= 0 && selectedIndex < this.aString.Count)
            this.strCmd = (string) this.aString[selectedIndex];
          this.DialogResult = DialogResult.OK;
          break;
        case Keys.Escape:
          this.DialogResult = DialogResult.Cancel;
          break;
        case Keys.Up:
          if (this.listBoxDisplay.SelectedIndex > 0)
            --this.listBoxDisplay.SelectedIndex;
          e.Handled = true;
          break;
        case Keys.Down:
          if (this.listBoxDisplay.SelectedIndex < this.listBoxDisplay.Items.Count - 1)
            ++this.listBoxDisplay.SelectedIndex;
          e.Handled = true;
          break;
      }
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
        this.strCmd = (string) this.aString[selectedIndex];
      this.DialogResult = DialogResult.OK;
    }
  }
}
