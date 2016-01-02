// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_MarketStatus
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_MarketStatus : Page_Main
  {
    private Font font = new Font("宋体", 12f, FontStyle.Regular);
    private int iCount = 7;
    private const int RANK_RATE = 0;
    private const int RANK_CONSIGNRATE = 6;
    private const int RANK_AMOUNTRATE = 5;
    private const int RANK_TOTALMONEY = 8;
    private SolidBrush m_Brush;
    private Pen pen;
    private string[,] STOCK_RANK_NAME;
    private Pos[,] pos;
    public MarketStatusVO[] statusData;
    private int iHighlightRowIndex;
    private int iHighlightColIndex;
    public Packet_MarketStatus packetInfo;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;

    public Page_MarketStatus(Rectangle _rc, HQForm hqForm)
      : base(_rc, hqForm)
    {
      this.pluginInfo = this.m_pluginInfo;
      this.setInfo = this.m_setInfo;
      this.m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
      this.pen = new Pen(SetInfo.RHColor.clGrid);
      this.AskForDataOnTimer();
      this.MakeMenus();
      this.STOCK_RANK_NAME = new string[3, 3];
      this.STOCK_RANK_NAME[0, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_UpRateList");
      this.STOCK_RANK_NAME[1, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_DownRateList");
      this.STOCK_RANK_NAME[2, 0] = this.pluginInfo.HQResourceManager.GetString("HQStr_SwingList");
      this.STOCK_RANK_NAME[0, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinUpRateList");
      this.STOCK_RANK_NAME[1, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr__5MinDownRateList");
      this.STOCK_RANK_NAME[2, 1] = this.pluginInfo.HQResourceManager.GetString("HQStr_VolRateList");
      this.STOCK_RANK_NAME[0, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRateDesc");
      this.STOCK_RANK_NAME[1, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRateAsce");
      this.STOCK_RANK_NAME[2, 2] = this.pluginInfo.HQResourceManager.GetString("HQStr_MoneyList");
      this.m_hqClient.CurrentPage = 5;
    }

    protected override void AskForDataOnTimer()
    {
      CMDMarketSortVO cmdMarketSortVo = new CMDMarketSortVO();
      this.iCount = this.GetICount();
      cmdMarketSortVo.num = this.iCount;
      this.m_hqClient.sendThread.AskForData((CMDVO) cmdMarketSortVo);
    }

    private int GetICount()
    {
      int num = this.font.Height + 2;
      return (this.m_rc.Height / 3 - (num + 2)) / num;
    }

    public override void Paint(Graphics g, int v)
    {
      try
      {
        if (this.statusData != null)
          Logger.wirte(MsgType.Information, string.Concat(new object[4]
          {
            (object) "this.iCount = ",
            (object) this.iCount,
            (object) "    statusData.length / 9 = ",
            (object) (this.statusData.Length / 9)
          }));
        this.initilizer(g);
        this.paintGrid(g);
        this.paintTitle(g);
        this.paintStockData(g);
        this.m_hqForm.EndPaint();
        this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MarketStatus-Paint异常：" + ex.Message);
      }
    }

    private void initilizer(Graphics g)
    {
      int x = this.m_rc.X;
      int num1 = this.font.Height + 2;
      int num2 = this.m_rc.Y + num1 + 2;
      int num3 = (this.m_rc.Height / 3 - (num1 + 2)) / num1;
      if (this.iCount > num3)
        this.iCount = num3;
      if (this.iCount <= 0)
        return;
      this.pos = new Pos[this.iCount * 3, 3];
      for (int index1 = 0; index1 < this.iCount * 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          this.pos[index1, index2] = new Pos();
          this.pos[index1, index2].x = x;
          this.pos[index1, index2].y = num2;
          x += this.m_rc.Width / 3;
        }
        x = this.m_rc.X;
        if (index1 == this.iCount - 1)
          num2 = this.m_rc.Y + this.m_rc.Height / 3 + num1 + 2;
        else if (index1 == 2 * this.iCount - 1)
          num2 = this.m_rc.Y + this.m_rc.Height / 3 * 2 + num1 + 2;
        else
          num2 += num1;
      }
    }

    private void paintGrid(Graphics g)
    {
      this.pen.Color = SetInfo.RHColor.clEqual;
      g.DrawRectangle(this.pen, this.m_rc.X, this.m_rc.Y, this.m_rc.Width - 1, this.m_rc.Height - 2);
      int num = this.font.Height + 2;
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + num);
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3);
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 + num);
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 * 2, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 * 2);
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height / 3 * 2 + num, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height / 3 * 2 + num);
      g.DrawLine(this.pen, this.m_rc.X + this.m_rc.Width / 3, this.m_rc.Y, this.m_rc.X + this.m_rc.Width / 3, this.m_rc.Y + this.m_rc.Height - 2);
      g.DrawLine(this.pen, this.m_rc.X + this.m_rc.Width / 3 * 2, this.m_rc.Y, this.m_rc.X + this.m_rc.Width / 3 * 2, this.m_rc.Y + this.m_rc.Height - 2);
    }

    private void paintTitle(Graphics g)
    {
      int x = this.m_rc.X;
      int y = this.m_rc.Y;
      int num1 = this.m_rc.Width / 3;
      int num2 = this.m_rc.Height / 3;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          string str = this.STOCK_RANK_NAME[index1, index2];
          int num3 = (int) g.MeasureString(str, this.font).Width;
          int num4 = 0;
          if (num1 > num3)
            num4 = (num1 - num3) / 2;
          this.m_Brush.Color = SetInfo.RHColor.clMultiQuote_TitleBack;
          g.FillRectangle((Brush) this.m_Brush, x + 1, y + 1, this.m_rc.Width / 3 - 1, this.font.Height + 1);
          this.m_Brush.Color = SetInfo.RHColor.clItem;
          g.DrawString(str, this.font, (Brush) this.m_Brush, (float) (x + num4), (float) (y + 2));
          x += this.m_rc.Width / 3;
        }
        x = this.m_rc.X;
        y += this.m_rc.Height / 3;
      }
    }

    private void paintStockData(Graphics g)
    {
      if (this.packetInfo == null || this.statusData == null)
        return;
      for (int i = 0; i < this.iCount * 3; ++i)
      {
        for (int j = 0; j < 3; ++j)
        {
          int ofStatusDataArray = this.getIndexOfStatusDataArray(i, j);
          if (ofStatusDataArray >= 0 && ofStatusDataArray < this.statusData.Length)
          {
            MarketStatusVO marketStatusVo = this.statusData[ofStatusDataArray];
            int x = this.pos[i, j].x;
            int y = this.pos[i, j].y;
            if (j == 0 || j == 1 && i >= 0 && i < 2 * this.iCount)
              this.paintRankData(g, marketStatusVo, 0, x, y);
            else if (i >= 2 * this.iCount && i < 3 * this.iCount && j == 1)
              this.paintRankData(g, marketStatusVo, 5, x, y);
            else if (i >= 2 * this.iCount && i < 3 * this.iCount && j == 2)
              this.paintRankData(g, marketStatusVo, 8, x, y);
            else
              this.paintRankData(g, marketStatusVo, 6, x, y);
          }
        }
      }
    }

    private void paintHighlight(int newRowIndex, int newColIndex)
    {
      if (!this.m_hqForm.IsEndPaint || newRowIndex >= this.iCount * 3 || this.iCount <= 0)
        return;
      Pos pos1 = (Pos) null;
      if (this.iHighlightRowIndex < this.iCount * 3)
        pos1 = this.pos[this.iHighlightRowIndex, this.iHighlightColIndex];
      Pos pos2 = (Pos) null;
      if (newRowIndex < this.iCount * 3)
        pos2 = this.pos[newRowIndex, newColIndex];
      Graphics mGraphics = this.m_hqForm.M_Graphics;
      if (pos1 != null)
        GDIDraw.XorRectangle(mGraphics, new Rectangle(pos1.x, pos1.y, this.m_rc.Width / 3, this.font.Height), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
      if (this.iHighlightRowIndex == newRowIndex && this.iHighlightColIndex == newColIndex)
        return;
      if (pos2 != null)
        GDIDraw.XorRectangle(mGraphics, new Rectangle(pos2.x, pos2.y, this.m_rc.Width / 3, this.font.Height), SetInfo.RHColor.clHighlight, this.m_hqForm.ScrollOffset);
      this.iHighlightRowIndex = newRowIndex;
      this.iHighlightColIndex = newColIndex;
    }

    private void paintRankData(Graphics g, MarketStatusVO value, int rankType, int x, int y)
    {
      string _marketID = value.marketID;
      string str1 = value.code;
      CodeTable codeTable = (CodeTable) this.m_hqClient.m_htProduct[(object) (_marketID + str1)];
      if (codeTable != null)
      {
        string str2 = codeTable.sName;
      }
      int precision = this.m_hqClient.GetPrecision(new CommodityInfo(_marketID, str1));
      bool isPercent = true;
      if (rankType == 8 || rankType == 5)
        isPercent = false;
      string str3 = this.formatRankData(g, value.cur, precision, false);
      string str4 = this.formatRankData(g, value.value, 2, isPercent);
      this.m_Brush.Color = SetInfo.RHColor.clProductName;
      g.DrawString(str1, this.font, (Brush) this.m_Brush, (float) (x + 3), (float) y);
      switch (value.status)
      {
        case (byte) 0:
          this.m_Brush.Color = SetInfo.RHColor.clIncrease;
          break;
        case (byte) 1:
          this.m_Brush.Color = SetInfo.RHColor.clDecrease;
          break;
        case (byte) 2:
          this.m_Brush.Color = SetInfo.RHColor.clEqual;
          break;
      }
      int num1 = this.m_rc.Width / 3 - 128;
      if (num1 > 64)
      {
        int num2 = (num1 - 64) / 2;
        g.DrawString(str3, this.font, (Brush) this.m_Brush, (float) (x + this.m_rc.Width / 3 - 64 - num2 - (int) g.MeasureString(str3, this.font).Width), (float) y);
      }
      else
        g.DrawString(str3, this.font, (Brush) this.m_Brush, (float) (x + 128 - (int) g.MeasureString(str3, this.font).Width), (float) y);
      switch (rankType)
      {
        case 5:
          this.m_Brush.Color = SetInfo.RHColor.clVolume;
          break;
        case 6:
          this.m_Brush.Color = SetInfo.RHColor.clNumber;
          break;
        case 8:
          this.m_Brush.Color = SetInfo.RHColor.clNumber;
          break;
        default:
          this.m_Brush.Color = (double) value.value <= 0.0 ? ((double) value.value != 0.0 ? SetInfo.RHColor.clDecrease : SetInfo.RHColor.clEqual) : SetInfo.RHColor.clIncrease;
          break;
      }
      g.DrawString(str4, this.font, (Brush) this.m_Brush, (float) (x + this.m_rc.Width / 3 - (int) g.MeasureString(str4, this.font).Width - 3), (float) y);
    }

    private int getIndexOfStatusDataArray(int i, int j)
    {
      if (this.packetInfo == null || i % this.iCount >= this.packetInfo.iCount)
        return -1;
      int num = this.statusData.Length / 9;
      if (i >= 0 && i < this.iCount)
        return j * 3 * num + i;
      if (i >= this.iCount && i < 2 * this.iCount)
        return (j * 3 + 1) * num + i - this.iCount;
      if (i >= 2 * this.iCount && i < 3 * this.iCount)
        return (j * 3 + 2) * num + i - 2 * this.iCount;
      return -1;
    }

    private string formatRankData(Graphics g, float num, int iPrecision, bool isPercent)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!isPercent)
      {
        stringBuilder.Append(M_Common.FloatToString((double) num, iPrecision));
      }
      else
      {
        if ((double) num >= 0.0)
          stringBuilder.Append("+");
        stringBuilder.Append(M_Common.FloatToString((double) num, iPrecision));
        stringBuilder.Append("%");
      }
      return stringBuilder.ToString();
    }

    private void selectStock(int x, int y)
    {
      int height = this.font.Height;
      for (int index1 = 0; index1 < this.iCount * 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          int num1 = this.pos[index1, index2].x;
          int num2 = this.pos[index1, index2].y;
          if (x > num1 && x < num1 + this.m_rc.Width / 3 && (y > num2 && y < num2 + height) && ((this.iHighlightRowIndex != index1 || this.iHighlightColIndex != index2) && this.getIndexOfStatusDataArray(index1, index2) >= 0))
            this.paintHighlight(index1, index2);
        }
      }
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        if (this.packetInfo == null || this.statusData == null)
          return;
        this.selectStock(e.X, e.Y);
        ((HQClientForm) this.m_hqForm).mainWindow.Focus();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MarketStatus-Page_MouseClick异常：" + ex.Message);
      }
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      try
      {
        if (this.packetInfo == null || this.statusData == null)
          return;
        int height = this.font.Height;
        for (int i = 0; i < this.iCount * 3; ++i)
        {
          for (int j = 0; j < 3; ++j)
          {
            int num1 = this.pos[i, j].x;
            int num2 = this.pos[i, j].y;
            if (e.X > num1 && e.X < num1 + this.m_rc.Width / 3 && (e.Y > num2 && e.Y < num2 + height))
            {
              int num3 = this.statusData.Length / 9;
              int ofStatusDataArray = this.getIndexOfStatusDataArray(i, j);
              if (ofStatusDataArray >= 0)
                this.m_hqForm.QueryStock(new CommodityInfo(this.statusData[ofStatusDataArray].marketID, this.statusData[ofStatusDataArray].code));
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MarketStatus-Page_MouseDoubleClick异常：" + ex.Message);
      }
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if (this.packetInfo == null || this.statusData == null)
          return;
        switch (e.KeyCode)
        {
          case Keys.Return:
            int ofStatusDataArray = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
            if (ofStatusDataArray < 0)
              break;
            this.m_hqForm.QueryStock(new CommodityInfo(this.statusData[ofStatusDataArray].marketID, this.statusData[ofStatusDataArray].code));
            break;
          case Keys.Left:
            if (this.iHighlightColIndex <= 0)
              break;
            this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex - 1);
            break;
          case Keys.Up:
            if (this.iHighlightRowIndex <= 0)
              break;
            int num1 = 1;
            while (this.iHighlightRowIndex - num1 >= 0 && this.getIndexOfStatusDataArray(this.iHighlightRowIndex - num1, this.iHighlightColIndex) < 0)
              ++num1;
            this.paintHighlight(this.iHighlightRowIndex - num1, this.iHighlightColIndex);
            break;
          case Keys.Right:
            if (this.iHighlightColIndex >= 2)
              break;
            this.paintHighlight(this.iHighlightRowIndex, this.iHighlightColIndex + 1);
            break;
          case Keys.Down:
            if (this.iHighlightRowIndex >= 3 * this.iCount - 1)
              break;
            int num2 = 1;
            while (this.iHighlightRowIndex + num2 <= 3 * this.iCount - 1 && this.getIndexOfStatusDataArray(this.iHighlightRowIndex + num2, this.iHighlightColIndex) < 0)
              ++num2;
            this.paintHighlight(this.iHighlightRowIndex + num2, this.iHighlightColIndex);
            break;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MarketStatus-Page_KeyDown异常：" + ex.Message);
      }
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
      toolStripMenuItem1.Name = "cmd_60";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
      toolStripMenuItem2.Name = "minline";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
      toolStripMenuItem3.Name = "kline";
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem3);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.AddCommonMenu();
    }

    protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        string name = e.ClickedItem.Name;
        if (name.IndexOf("cmd_") >= 0)
          this.m_hqForm.UserCommand(name.Substring(4));
        else if (name.Equals("minline"))
        {
          int ofStatusDataArray = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
          if (ofStatusDataArray >= 0)
            this.m_hqForm.ShowPageMinLine(new CommodityInfo(this.statusData[ofStatusDataArray].marketID, this.statusData[ofStatusDataArray].code));
        }
        else if (name.Equals("kline"))
        {
          int ofStatusDataArray = this.getIndexOfStatusDataArray(this.iHighlightRowIndex, this.iHighlightColIndex);
          if (ofStatusDataArray >= 0)
            this.m_hqForm.ShowPageMinLine(new CommodityInfo(this.statusData[ofStatusDataArray].marketID, this.statusData[ofStatusDataArray].code));
        }
        else
          this.m_hqForm.UserCommand(name);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MarketStatus-contextMenu_ItemClicked异常：" + ex.Message);
      }
    }

    public override void Dispose()
    {
      GC.Collect();
    }
  }
}
