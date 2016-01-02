// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_Bill
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.HQThread;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_Bill : Page_Main
  {
    private float zoomRate = 1f;
    private int totalPages = 1;
    private Font fntTitle = new Font("楷体", 12f, FontStyle.Bold);
    private Font fntText = new Font("宋体", 10f, FontStyle.Regular);
    private SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clGrid);
    private Pen pen = new Pen(SetInfo.RHColor.clGrid);
    private const int ROW_NUM = 3;
    private BillFieldInfo[] fieldInfo;
    private int curPageNo;
    private int iRows;
    private int iProductType;
    private ProductData stock;
    private int rowHeight;
    private int startY;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private string[] m_strItems;
    private Hashtable b_htItemInfo;
    private Rectangle leftRect;
    private Rectangle rightRect;
    private int lastMinTime;

    public Page_Bill(Rectangle _rc, HQForm m_HQForm)
      : base(_rc, m_HQForm)
    {
      try
      {
        Logger.wirte(MsgType.Information, this.fntTitle.Height.ToString());
        this.pluginInfo = this.m_pluginInfo;
        this.setInfo = this.m_setInfo;
        this.AskForDataOnce();
        this.m_hqClient.CurrentPage = 4;
        this.MakeMenus();
        this.iProductType = this.m_hqClient.getProductType(this.m_hqClient.curCommodityInfo);
        this.initBillFieldInfo();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    private void initBillFieldInfo()
    {
      this.b_htItemInfo = new Hashtable();
      this.b_htItemInfo.Add((object) "Time", (object) new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Time"), true, 64));
      this.b_htItemInfo.Add((object) "Price", (object) new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Price"), true, 70));
      this.b_htItemInfo.Add((object) "CurVol", (object) new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_CurVol"), true, 60));
      this.b_htItemInfo.Add((object) "Dingli", (object) new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_Dingli"), true, 50));
      this.b_htItemInfo.Add((object) "ZhuanRang", (object) new BillFieldInfo(this.pluginInfo.HQResourceManager.GetString("HQStr_ZhuanRang"), true, 50));
      string str1 = this.setInfo.TransactionBillName;
      char[] chArray1 = new char[1]
      {
        ';'
      };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]
        {
          ':'
        };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 2 && strArray[1].Length > 0)
        {
          BillFieldInfo billFieldInfo = (BillFieldInfo) this.b_htItemInfo[(object) strArray[0]];
          if (billFieldInfo != null)
            billFieldInfo.name = strArray[1];
        }
      }
      this.m_strItems = this.setInfo.TransactionBillItems.Split(';');
      this.fieldInfo = new BillFieldInfo[this.m_strItems.Length - 1];
      for (int index = 0; index < this.m_strItems.Length - 1; ++index)
        this.fieldInfo[index] = (BillFieldInfo) this.b_htItemInfo[(object) this.m_strItems[index]];
    }

    private void AskForDataOnce()
    {
      this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
      if (this.stock == null)
      {
        if (this.m_hqClient.aProductData.Count > 50)
          this.m_hqClient.aProductData.RemoveAt(50);
        this.stock = new ProductData();
        this.stock.commodityInfo = this.m_hqClient.curCommodityInfo;
        if (this.stock.commodityInfo == null)
          return;
        this.m_hqClient.aProductData.Insert(0, (object) this.stock);
        DateTime time = new DateTime();
        if (this.stock != null && this.stock.realData != null)
          time = this.stock.realData.time;
        SendThread.AskForRealQuote(this.m_hqClient.curCommodityInfo.marketID, this.m_hqClient.curCommodityInfo.commodityCode, time, this.m_hqClient.sendThread);
      }
      CMDBillByVersionVO cmdBillByVersionVo = new CMDBillByVersionVO();
      cmdBillByVersionVo.marketID = this.m_hqClient.curCommodityInfo.marketID;
      cmdBillByVersionVo.code = this.m_hqClient.curCommodityInfo.commodityCode;
      cmdBillByVersionVo.type = (byte) 2;
      cmdBillByVersionVo.time = 0L;
      cmdBillByVersionVo.ReservedField = string.Empty;
      if (this.stock == null || this.stock.aBill.Count == 0)
      {
        cmdBillByVersionVo.totalAmount = 0L;
      }
      else
      {
        BillDataVO billDataVo = (BillDataVO) this.stock.aBill[this.stock.aBill.Count - 1];
        cmdBillByVersionVo.totalAmount = billDataVo.totalAmount;
      }
      this.m_hqClient.sendThread.AskForData((CMDVO) cmdBillByVersionVo);
    }

    protected override void AskForDataOnTimer()
    {
    }

    public override void Paint(Graphics g, int v)
    {
      try
      {
        this.initVisibleField();
        this.initPageInfo(g);
        this.paintTitle(g);
        this.paintBillData(g);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
      }
    }

    private void initVisibleField()
    {
      int num1 = this.m_rc.Width / 3 - 4;
      if (num1 < 0)
        num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int length = this.fieldInfo.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        if (num2 + this.fieldInfo[index1].width < num1)
        {
          this.fieldInfo[index1].visible = true;
          ++num3;
          num2 += this.fieldInfo[index1].width;
        }
        else
        {
          for (int index2 = index1; index2 < length; ++index2)
            this.fieldInfo[index2].visible = false;
          break;
        }
      }
      if (num3 <= 0)
        num3 = 1;
      if (num3 != length)
        return;
      this.zoomRate = (float) num1 / (float) num2;
    }

    private void initPageInfo(Graphics g)
    {
      this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
      if (this.stock == null || this.stock.realData == null || this.stock.aBill == null)
      {
        this.totalPages = 1;
        this.curPageNo = 0;
      }
      else
      {
        int count = this.stock.aBill.Count;
        if (count <= 0)
        {
          this.totalPages = 1;
          this.curPageNo = 0;
        }
        else
        {
          int num1;
          try
          {
            num1 = this.fntTitle.Height;
          }
          catch (Exception ex)
          {
            Logger.wirte(MsgType.Error, ex.Message);
            num1 = 19;
          }
          int num2;
          try
          {
            num2 = this.fntText.Height;
          }
          catch (Exception ex)
          {
            Logger.wirte(MsgType.Error, ex.Message);
            num2 = 16;
          }
          this.rowHeight = num2 + 2;
          this.iRows = (this.m_rc.Height - num1 - num2 - 6) / this.rowHeight;
          int num3 = (count - 1) / (this.iRows * 3);
          if ((count - 1) % (this.iRows * 3) != 0)
            ++num3;
          if (num3 == this.totalPages)
            return;
          if (num3 == 0)
            num3 = 1;
          this.totalPages = num3;
          this.curPageNo = 0;
        }
      }
    }

    private void paintTitle(Graphics g)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if (this.stock != null)
      {
        str1 = this.stock.commodityInfo.commodityCode;
        CodeTable codeTable = (CodeTable) this.m_hqClient.m_htProduct[(object) (this.stock.commodityInfo.marketID + this.stock.commodityInfo.commodityCode)];
        if (codeTable != null)
          str3 = codeTable.sName;
      }
      if (str3.Equals(str1))
        str3 = "";
      string str4 = str2 + str3 + " " + str1 + " " + this.pluginInfo.HQResourceManager.GetString("HQStr_TradeList");
      int x1 = this.m_rc.X;
      int y = this.m_rc.Y;
      try
      {
        this.m_Brush.Color = SetInfo.RHColor.clProductName;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, ex.Message);
      }
      int x2 = x1 + (this.m_rc.Width - (int) g.MeasureString(str4, this.fntTitle).Width) / 2;
      if (x2 < 0)
        x2 = 0;
      g.DrawString(str4, this.fntTitle, (Brush) this.m_Brush, (float) x2, (float) y);
      Point point = new Point(x2, y);
      Point[] points1 = new Point[7]
      {
        new Point(point.X - 30, point.Y + 10),
        new Point(point.X - 20, point.Y + 4),
        new Point(point.X - 20, point.Y + 7),
        new Point(point.X - 5, point.Y + 7),
        new Point(point.X - 5, point.Y + 11),
        new Point(point.X - 20, point.Y + 11),
        new Point(point.X - 20, point.Y + 14)
      };
      this.leftRect = new Rectangle(point.X - 30, point.Y - 7, 30, (int) g.MeasureString(str4, this.fntTitle).Height);
      point = new Point(point.X + (int) g.MeasureString(str4, this.fntTitle).Width + 5, point.Y + 9);
      Point[] points2 = new Point[7]
      {
        new Point(point.X, point.Y - 2),
        new Point(point.X + 15, point.Y - 2),
        new Point(point.X + 15, point.Y - 5),
        new Point(point.X + 25, point.Y),
        new Point(point.X + 15, point.Y + 5),
        new Point(point.X + 15, point.Y + 2),
        new Point(point.X, point.Y + 2)
      };
      this.rightRect = new Rectangle(point.X, point.Y - 7, 30, (int) g.MeasureString(str4, this.fntTitle).Height);
      g.DrawPolygon(new Pen(Brushes.White, 1f), points1);
      g.DrawPolygon(new Pen(Brushes.White, 1f), points2);
      int x3 = this.m_rc.X;
      int num1 = this.m_rc.Y + this.fntTitle.Height;
      this.pen.Color = SetInfo.RHColor.clGrid;
      g.DrawRectangle(this.pen, x3, num1, x3 + this.m_rc.Width - 1, this.m_rc.Height - this.fntTitle.Height);
      g.DrawLine(this.pen, this.m_rc.X, this.m_rc.Y + this.m_rc.Height - 1, this.m_rc.X + this.m_rc.Width, this.m_rc.Y + this.m_rc.Height - 1);
      for (int index = 1; index < 3; ++index)
        g.DrawLine(this.pen, x3 + this.m_rc.Width / 3 * index, num1, x3 + this.m_rc.Width / 3 * index, num1 + this.m_rc.Height - this.fntTitle.Height);
      g.DrawLine(this.pen, x3, num1 + this.fntText.Height + 2, x3 + this.m_rc.Width - 1, num1 + this.fntText.Height + 2);
      this.startY = num1 + this.fntText.Height + 4;
      this.m_Brush.Color = SetInfo.RHColor.clItem;
      int length = this.fieldInfo.Length;
      int num2 = num1 + 1;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int num3 = this.m_rc.X + this.m_rc.Width / 3 * index1;
        for (int index2 = 0; index2 < length && this.fieldInfo[index2].visible; ++index2)
        {
          num3 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
          string str5 = this.fieldInfo[index2].name;
          int num4 = (int) g.MeasureString(str5, this.fntText).Width;
          g.DrawString(str5, this.fntText, (Brush) this.m_Brush, (float) (num3 - num4), (float) num2);
        }
      }
      string str6 = this.pluginInfo.HQResourceManager.GetString("HQStr_PagePrefix") + (object) (this.totalPages - this.curPageNo) + this.pluginInfo.HQResourceManager.GetString("HQStr_PageSuffix") + " " + this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPagePrefix") + (string) (object) this.totalPages + this.pluginInfo.HQResourceManager.GetString("HQStr_TotalPageSuffix");
      this.m_Brush.Color = SetInfo.RHColor.clGrid;
      g.DrawString(str6, this.fntText, (Brush) this.m_Brush, (float) (this.m_rc.X + this.m_rc.Width - (int) g.MeasureString(str6, this.fntText).Width), (float) (this.m_rc.Y + num2 - this.fntText.Height));
    }

    private void paintBillData(Graphics g)
    {
      this.stock = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
      if (this.stock == null || this.stock.realData == null || this.stock.aBill == null)
        return;
      int precision = this.m_hqClient.GetPrecision(this.stock.commodityInfo);
      int num1 = this.iRows * 3;
      int count = this.stock.aBill.Count;
      if (count <= 0)
        return;
      if (count < num1)
        num1 = count;
      int x1 = this.m_rc.X;
      int y = this.startY;
      int num2 = count - num1 * (this.curPageNo + 1);
      if (num2 < 0)
        num2 = 0;
      int num3 = num2 + num1;
      if (num3 > count)
      {
        num3 = count;
        num2 = num3 - num1;
        if (num2 <= 0)
          num2 = 1;
      }
      for (int index1 = num2; index1 < num3; ++index1)
      {
        int x2 = this.m_rc.X + this.m_rc.Width / 3 * ((index1 - num2) / this.iRows);
        if ((index1 - num2) % this.iRows == 0)
          y = this.startY;
        BillDataVO billDataVo1 = (BillDataVO) null;
        if (index1 > 0)
        {
          billDataVo1 = (BillDataVO) this.stock.aBill[index1 - 1];
          if (billDataVo1 == null)
            break;
        }
        BillDataVO billDataVo2 = (BillDataVO) this.stock.aBill[index1];
        if (billDataVo2 == null)
          break;
        if (this.lastMinTime != billDataVo2.time / 100 && y != this.startY)
          g.DrawLine(new Pen(SetInfo.RHColor.clGrid)
          {
            DashStyle = DashStyle.Dash
          }, new Point(x2, y), new Point(x2 + this.m_rc.Width / 3, y));
        for (int index2 = 0; index2 < this.fieldInfo.Length; ++index2)
        {
          if ("Time".Equals(this.m_strItems[index2]))
          {
            if (this.fieldInfo[index2].visible)
            {
              string str = TradeTimeVO.HHMMSSIntToString(billDataVo2.time);
              this.lastMinTime = billDataVo2.time / 100;
              this.m_Brush.Color = SetInfo.RHColor.clNumber;
              x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
              g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
            }
          }
          else if ("Price".Equals(this.m_strItems[index2]))
          {
            if (this.fieldInfo[index2].visible)
            {
              string str = M_Common.FloatToString((double) billDataVo2.curPrice, precision);
              this.m_Brush.Color = (double) billDataVo2.curPrice <= (double) this.stock.realData.yesterBalancePrice ? ((double) billDataVo2.curPrice >= (double) this.stock.realData.yesterBalancePrice ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
              g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
            }
          }
          else if ("CurVol".Equals(this.m_strItems[index2]))
          {
            if (this.fieldInfo[index2].visible)
            {
              string str = billDataVo1 != null ? Convert.ToString((int) (billDataVo2.totalAmount - billDataVo1.totalAmount)) : Convert.ToString(billDataVo2.totalAmount);
              this.m_Brush.Color = SetInfo.RHColor.clVolume;
              x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate - 16.0);
              g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
              if (this.iProductType != 2 && this.iProductType != 3)
              {
                byte num4;
                if (billDataVo1 == null)
                  num4 = (byte) 2;
                else if ((double) billDataVo1.buyPrice <= 1.0 / 1000.0)
                  num4 = (byte) 1;
                else if ((double) billDataVo2.curPrice >= (double) billDataVo1.sellPrice)
                  num4 = (byte) 0;
                else if ((double) billDataVo2.curPrice <= (double) billDataVo1.buyPrice)
                {
                  num4 = (byte) 1;
                }
                else
                {
                  int num5 = (int) (((double) billDataVo1.sellPrice - (double) billDataVo2.curPrice) * 1000.0);
                  float num6 = (float) (int) (((double) billDataVo2.curPrice - (double) billDataVo1.buyPrice) * 1000.0);
                  num4 = (double) num5 >= (double) num6 ? ((double) num5 <= (double) num6 ? (byte) 2 : (byte) 1) : (byte) 0;
                }
                string s;
                if ((int) num4 == 0)
                {
                  s = "↑";
                  this.m_Brush.Color = SetInfo.RHColor.clIncrease;
                }
                else if ((int) num4 == 1)
                {
                  s = "↓";
                  this.m_Brush.Color = SetInfo.RHColor.clDecrease;
                }
                else
                {
                  s = "–";
                  this.m_Brush.Color = SetInfo.RHColor.clEqual;
                }
                g.DrawString(s, this.fntText, (Brush) this.m_Brush, (float) x2, (float) y);
              }
            }
          }
          else if ("Dingli".Equals(this.m_strItems[index2]))
          {
            if (this.fieldInfo[index2].visible)
            {
              string str = Convert.ToString(billDataVo2.openAmount);
              x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
              this.m_Brush.Color = SetInfo.RHColor.clNumber;
              g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
            }
          }
          else if ("ZhuanRang".Equals(this.m_strItems[index2]))
          {
            if (this.fieldInfo[index2].visible)
            {
              string str = Convert.ToString(billDataVo2.closeAmount);
              x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
              this.m_Brush.Color = SetInfo.RHColor.clNumber;
              g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
            }
          }
          else if ("sng".Equals(this.m_strItems[index2]) && this.fieldInfo[index2].visible)
          {
            string str = Convert.ToString(billDataVo2.closeAmount);
            x2 += (int) ((double) this.fieldInfo[index2].width * (double) this.zoomRate);
            this.m_Brush.Color = SetInfo.RHColor.clNumber;
            g.DrawString(str, this.fntText, (Brush) this.m_Brush, (float) (x2 - (int) g.MeasureString(str, this.fntText).Width), (float) y);
          }
        }
        y += this.rowHeight;
      }
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      if (this.leftRect.Contains(e.Location))
      {
        this.m_hqForm.ChangeStock(true);
        this.AskForDataOnce();
        this.m_hqForm.Repaint();
      }
      else if (this.rightRect.Contains(e.Location))
      {
        this.m_hqForm.ChangeStock(false);
        this.AskForDataOnce();
        this.m_hqForm.Repaint();
      }
      ((HQClientForm) this.m_hqForm).mainWindow.Focus();
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
      Point location = e.Location;
      if (this.leftRect.Contains(location) || this.rightRect.Contains(location))
        this.m_hqForm.M_Cursor = Cursors.Hand;
      else
        this.m_hqForm.M_Cursor = Cursors.Default;
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      bool flag = false;
      switch (e.KeyCode)
      {
        case Keys.Prior:
          if (this.curPageNo < this.totalPages - 1)
          {
            ++this.curPageNo;
            flag = true;
            break;
          }
          break;
        case Keys.Next:
          if (this.curPageNo > 0)
          {
            --this.curPageNo;
            flag = true;
            break;
          }
          break;
      }
      if (!flag)
        return;
      this.m_hqForm.Repaint();
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
      toolStripMenuItem1.Name = "cmd_80";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MinLine") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MinLine"));
      toolStripMenuItem2.Name = "minline";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
      toolStripMenuItem3.Name = "kline";
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
      toolStripMenuItem4.Name = "cmd_60";
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem3);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem4);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.AddCommonMenu();
    }

    protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      string name = e.ClickedItem.Name;
      if (name.IndexOf("cmd_") >= 0)
        this.m_hqForm.UserCommand(name.Substring(4));
      else if (name.Equals("minline"))
        this.m_hqForm.ShowPageMinLine();
      else if (name.Equals("kline"))
        this.m_hqForm.ShowPageKLine();
      else
        this.m_hqForm.UserCommand(name);
      this.m_hqForm.Repaint();
    }

    public override void Dispose()
    {
      this.fntTitle.Dispose();
      this.fntText.Dispose();
      this.m_Brush.Dispose();
      this.pen.Dispose();
    }
  }
}
