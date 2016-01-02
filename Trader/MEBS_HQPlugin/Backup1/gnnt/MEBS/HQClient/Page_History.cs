// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_History
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_History : Page_Main
  {
    private readonly int GAP = 3;
    private int iTitleHeight = 30;
    private Font font = new Font("宋体", 12f, FontStyle.Regular);
    private Font fontTitle = new Font("楷体", 14f, FontStyle.Bold);
    private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
    private Pen pen = new Pen(SetInfo.RHColor.clGrid);
    private ArrayList m_aCode;
    private int m_iRows;
    private int m_iCols;
    private int m_iWidth;
    private int m_iTotalPage;
    private int m_iCurPage;
    private int m_iHighlightRow;
    private int m_iHighlightCol;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;

    public Page_History(Rectangle _rc, HQForm m_HQForm)
      : base(_rc, m_HQForm)
    {
      try
      {
        this.pluginInfo = this.m_pluginInfo;
        this.setInfo = this.m_setInfo;
        this.m_aCode = new ArrayList();
        for (int index = 0; index < this.m_hqClient.m_codeList.Count; ++index)
        {
          CommodityInfo commodityInfo = (CommodityInfo) this.m_hqClient.m_codeList[index];
          CodeTable codeTable = (CodeTable) this.m_hqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
          if (codeTable != null && (codeTable.status == 1 || codeTable.status == 6))
            this.m_aCode.Add(this.m_hqClient.m_codeList[index]);
        }
        this.MakeMenus();
        this.m_hqClient.CurrentPage = 6;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_History异常：" + ex.Message);
      }
    }

    protected override void AskForDataOnTimer()
    {
    }

    public override void Paint(Graphics g, int v)
    {
      try
      {
        this.paintTitle(g);
        if (this.m_aCode.Count == 0)
        {
          this.paintPromptMessage(g);
        }
        else
        {
          this.calculateSize(g);
          this.paintProduct(g);
          this.m_hqForm.EndPaint();
          this.paintHighlight(-1, -1);
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_History-Paint异常：" + ex.Message);
      }
    }

    private void paintTitle(Graphics g)
    {
      int x1 = this.m_rc.X;
      int y1 = this.m_rc.Y;
      this.m_Brush.Color = SetInfo.RHColor.clProductName;
      string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_History");
      int num = x1 + (this.m_rc.Width - (int) g.MeasureString(@string, this.fontTitle).Width) / 2;
      if (num < 0)
        num = 0;
      g.DrawString(@string, this.fontTitle, (Brush) this.m_Brush, (float) num, (float) y1);
      int x2 = this.m_rc.X;
      int y2 = this.m_rc.Y + this.fontTitle.Height;
      this.pen.Color = SetInfo.RHColor.clGrid;
      g.DrawRectangle(this.pen, x2, y2, x2 + this.m_rc.Width - 1, this.m_rc.Height - this.fontTitle.Height - 1);
      this.iTitleHeight = this.fontTitle.Height;
    }

    private void calculateSize(Graphics g)
    {
      this.m_iRows = (this.m_rc.Height - this.iTitleHeight) / (this.font.Height + this.GAP);
      this.m_iWidth = (int) g.MeasureString("  大蒜十月  AB0210  ", this.font).Width;
      this.m_iCols = this.m_rc.Width / this.m_iWidth;
      if (this.m_iRows == 0 || this.m_iCols == 0)
        return;
      int num = this.m_aCode.Count / (this.m_iRows * this.m_iCols);
      if (this.m_aCode.Count % (this.m_iRows * this.m_iCols) > 0)
        ++num;
      if (num != this.m_iTotalPage)
      {
        this.m_iCurPage = 0;
        this.m_iHighlightRow = 0;
        this.m_iHighlightCol = 0;
        this.m_iTotalPage = num;
      }
      if (this.m_iHighlightRow >= this.m_iRows)
        this.m_iHighlightRow = 0;
      if (this.m_iHighlightCol >= this.m_iCols)
        this.m_iHighlightCol = 0;
      if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol < this.m_aCode.Count)
        return;
      this.m_iHighlightRow = 0;
      this.m_iHighlightCol = 0;
    }

    private void paintProduct(Graphics g)
    {
      string str = this.pluginInfo.HQResourceManager.GetString("HQStr_PagePrefix") + (object) (this.m_iCurPage + 1) + this.pluginInfo.HQResourceManager.GetString("HQStr_PageSuffix") + " " + this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPagePrefix") + (string) (object) this.m_iTotalPage + this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPageSuffix");
      this.m_Brush.Color = SetInfo.RHColor.clGrid;
      g.DrawString(str, this.font, (Brush) this.m_Brush, (float) (this.m_rc.X + this.m_rc.Width - (int) g.MeasureString(str, this.font).Width), (float) (this.m_rc.Y + this.GAP));
      int index = this.m_iCurPage * (this.m_iRows * this.m_iCols);
      for (int iRow = 0; iRow < this.m_iRows; ++iRow)
      {
        for (int iCol = 0; iCol < this.m_iCols && index < this.m_aCode.Count; ++iCol)
        {
          CommodityInfo commodityInfo = (CommodityInfo) this.m_aCode[index];
          CodeTable codeTable = (CodeTable) this.m_hqClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
          this.paintOneProduct(g, iRow, iCol, codeTable.sName, commodityInfo.commodityCode);
          ++index;
        }
        if (index >= this.m_aCode.Count)
          break;
      }
    }

    private void paintOneProduct(Graphics g, int iRow, int iCol, string name, string code)
    {
      int num1 = this.m_rc.X + iCol * this.m_iWidth;
      int num2 = this.m_rc.Y + this.iTitleHeight + iRow * (this.font.Height + this.GAP);
      this.m_Brush.Color = SetInfo.RHColor.clProductName;
      string str = name + " " + code + "  ";
      if (name.Equals(code))
        str = code;
      Font font = this.font;
      int num3 = (int) this.font.Size;
      for (; (double) g.MeasureString(str, this.font).Width > (double) this.m_iWidth; this.font = new Font("宋体", (float) num3, FontStyle.Regular))
        --num3;
      g.DrawString(str, this.font, (Brush) this.m_Brush, (float) (num1 + this.m_iWidth - (int) g.MeasureString(str, this.font).Width), (float) (num2 + this.GAP / 2));
      this.font = font;
    }

    private void paintHighlight(int newRow, int newCol)
    {
      Graphics mGraphics = this.m_hqForm.M_Graphics;
      if (this.m_iHighlightRow != -1)
        this.paintCurHighlight(mGraphics, this.m_iHighlightRow, this.m_iHighlightCol);
      if (newRow == -1 || this.m_iHighlightRow == newRow && this.m_iHighlightCol == newCol)
        return;
      this.paintCurHighlight(mGraphics, newRow, newCol);
      this.m_iHighlightRow = newRow;
      this.m_iHighlightCol = newCol;
    }

    private void paintCurHighlight(Graphics g, int iRow, int iCol)
    {
      int x = this.m_rc.X + iCol * this.m_iWidth;
      int y = this.m_rc.Y + this.iTitleHeight + iRow * (this.font.Height + this.GAP);
      GDIDraw.XorRectangle(g, new Rectangle(x, y, this.m_iWidth, this.font.Height + this.GAP), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
    }

    private void paintPromptMessage(Graphics g)
    {
      string @string = this.pluginInfo.HQResourceManager.GetString("HQStr_HistoryPrompt");
      int num1 = (int) g.MeasureString(@string, this.font).Width;
      int num2 = num1 / (this.m_rc.Width - 8);
      this.m_Brush.Color = SetInfo.RHColor.clProductName;
      if (num1 % (this.m_rc.Width - 8) > 0)
        ++num2;
      int num3 = (this.m_rc.Height - this.font.Height * num2 - 20) / 2 + 20;
      int startIndex = 0;
      int num4 = (this.m_rc.Width - 8) / 16;
      while (startIndex < @string.Length)
      {
        int length = startIndex + num4;
        string str;
        if (length > @string.Length)
        {
          str = @string.Substring(startIndex);
          startIndex = @string.Length;
        }
        else
        {
          str = @string.Substring(startIndex, length);
          startIndex = length;
        }
        int num5 = (this.m_rc.Width - 8 - (int) g.MeasureString(str, this.font).Width) / 2 + 4;
        g.DrawString(str, this.font, (Brush) this.m_Brush, (float) num5, (float) num3);
        num3 += this.font.Height;
      }
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        this.selectProduct(e.X, e.Y);
      ((HQClientForm) this.m_hqForm).mainWindow.Focus();
    }

    private bool selectProduct(int x, int y)
    {
      if (this.m_iWidth == 0)
        return false;
      int newCol = (x - this.m_rc.X) / this.m_iWidth;
      int newRow = (y - this.m_rc.Y - this.iTitleHeight) / (this.font.Height + this.GAP);
      if (newCol >= this.m_iCols || newRow >= this.m_iRows || this.m_iCurPage * (this.m_iRows * this.m_iCols) + newRow * this.m_iCols + newCol >= this.m_aCode.Count)
        return false;
      if (newRow == this.m_iHighlightRow && newCol == this.m_iHighlightCol)
        return true;
      this.paintHighlight(newRow, newCol);
      return true;
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (!this.selectProduct(e.X, e.Y))
        return;
      int index = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
      if (index >= this.m_aCode.Count)
        return;
      this.m_hqForm.ShowPageKLine((CommodityInfo) this.m_aCode[index]);
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        bool flag = false;
        switch (e.KeyData)
        {
          case Keys.Return:
            int index = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
            if (index < this.m_aCode.Count)
            {
              this.m_hqForm.ShowPageKLine((CommodityInfo) this.m_aCode[index]);
              break;
            }
            break;
          case Keys.Prior:
            if (this.m_iCurPage > 0)
            {
              --this.m_iCurPage;
              this.m_iHighlightRow = this.m_iHighlightCol = 0;
              flag = true;
              break;
            }
            break;
          case Keys.Next:
            if (this.m_iCurPage < this.m_iTotalPage - 1)
            {
              ++this.m_iCurPage;
              this.m_iHighlightRow = this.m_iHighlightCol = 0;
              flag = true;
              break;
            }
            break;
          case Keys.Left:
            if (this.m_iHighlightCol > 0)
            {
              this.paintHighlight(this.m_iHighlightRow, this.m_iHighlightCol - 1);
              break;
            }
            if (this.m_iHighlightRow > 0)
            {
              this.paintHighlight(this.m_iHighlightRow - 1, this.m_iCols - 1);
              break;
            }
            if (this.m_iCurPage > 0)
            {
              --this.m_iCurPage;
              this.m_iHighlightRow = this.m_iRows - 1;
              this.m_iHighlightCol = this.m_iCols - 1;
              flag = true;
              break;
            }
            break;
          case Keys.Up:
            if (this.m_iHighlightRow > 0)
            {
              this.paintHighlight(this.m_iHighlightRow - 1, this.m_iHighlightCol);
              break;
            }
            if (this.m_iCurPage == 0)
            {
              flag = false;
              break;
            }
            --this.m_iCurPage;
            this.m_iHighlightRow = this.m_iRows - 1;
            flag = true;
            break;
          case Keys.Right:
            if (this.m_iHighlightCol < this.m_iCols - 1)
            {
              if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol < this.m_aCode.Count - 1)
              {
                this.paintHighlight(this.m_iHighlightRow, this.m_iHighlightCol + 1);
                break;
              }
              break;
            }
            if (this.m_iHighlightRow < this.m_iRows - 1)
            {
              if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + (this.m_iHighlightRow + 1) * this.m_iCols < this.m_aCode.Count)
              {
                this.paintHighlight(this.m_iHighlightRow + 1, 0);
                break;
              }
              break;
            }
            if (this.m_iCurPage < this.m_iTotalPage - 1)
            {
              ++this.m_iCurPage;
              this.m_iHighlightRow = this.m_iHighlightCol = 0;
              flag = true;
              break;
            }
            break;
          case Keys.Down:
            if (this.m_iHighlightRow == this.m_iRows - 1)
            {
              if (this.m_iCurPage < this.m_iTotalPage - 1)
              {
                ++this.m_iCurPage;
                this.m_iHighlightRow = 0;
                if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightCol > this.m_aCode.Count - 1)
                  this.m_iHighlightCol = this.m_aCode.Count - this.m_iCurPage * (this.m_iRows * this.m_iCols) - 1;
                flag = true;
                break;
              }
              break;
            }
            if (this.m_iCurPage * (this.m_iRows * this.m_iCols) + (this.m_iHighlightRow + 1) * this.m_iCols + this.m_iHighlightCol <= this.m_aCode.Count - 1)
            {
              this.paintHighlight(this.m_iHighlightRow + 1, this.m_iHighlightCol);
              break;
            }
            break;
        }
        if (!flag)
          return;
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_History-Page_KeyDown异常：" + ex.Message);
      }
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
      toolStripMenuItem1.Name = "cmd_60";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
      toolStripMenuItem2.Name = "cmd_80";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
      toolStripMenuItem3.Name = "kline";
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem3);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      this.AddCommonMenu();
    }

    protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        string name = e.ClickedItem.Name;
        if (name.IndexOf("cmd_") >= 0)
          this.m_hqForm.UserCommand(name.Substring(4));
        else if (name.Equals("kline"))
        {
          int index = this.m_iCurPage * (this.m_iRows * this.m_iCols) + this.m_iHighlightRow * this.m_iCols + this.m_iHighlightCol;
          if (index < this.m_aCode.Count)
            this.m_hqForm.ShowPageKLine((CommodityInfo) this.m_aCode[index]);
        }
        else
          this.m_hqForm.UserCommand(name);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_History-contextMenu_ItemClicked异常：" + ex.Message);
      }
    }

    public override void Dispose()
    {
      GC.Collect();
    }
  }
}
