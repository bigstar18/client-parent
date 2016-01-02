// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_MinLine
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_MinLine : Page_Main
  {
    private int rcQuoteWidth = 220;
    private int m_iQuoteH = 380;
    public int iProductType;
    private Draw_MinLine draw_MinLine;
    private Draw_Quote draw_Quote;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ButtonUtils buttonUtils;
    private Rectangle rcMinLine;
    private Rectangle rcQuote;

    public Page_MinLine(Rectangle _rc, HQForm hqForm)
      : base(_rc, hqForm)
    {
      try
      {
        this.pluginInfo = this.m_pluginInfo;
        this.setInfo = this.m_setInfo;
        this.buttonUtils = hqForm.CurHQClient.buttonUtils;
        if (this.m_hqClient.isNeedAskData)
          this.AskForDataOnce();
        this.m_hqClient.CurrentPage = 1;
        this.draw_MinLine = new Draw_MinLine(hqForm, true);
        this.MakeMenus();
        this.iProductType = this.m_hqClient.getProductType(this.m_hqClient.curCommodityInfo);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine异常：" + ex.Message);
      }
    }

    protected override void AskForDataOnTimer()
    {
    }

    private void AskForDataOnce()
    {
      if (this.m_hqClient.listData.Count != this.m_hqClient.rectCol * this.m_hqClient.rectRol || !this.m_hqForm.IsMultiCommidity)
      {
        this.m_hqClient.listData = this.getAfterGoodsInfo(this.m_hqClient.rectCol * this.m_hqClient.rectRol);
      }
      else
      {
        ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
        if (productData == null)
        {
          productData = new ProductData();
          productData.commodityInfo = this.m_hqClient.curCommodityInfo;
          if (productData.commodityInfo == null)
            return;
          this.m_hqClient.aProductData.Insert(0, (object) productData);
        }
        this.m_hqClient.listData[this.m_globalData.SelectNumMin - 1] = productData;
      }
      CMDMinVO cmdMinVo = new CMDMinVO();
      List<CommidityVO> list = new List<CommidityVO>();
      for (int index = 0; index < this.m_hqClient.listData.Count; ++index)
        list.Add(new CommidityVO()
        {
          code = this.m_hqClient.listData[index].commodityInfo.commodityCode,
          marketID = this.m_hqClient.listData[index].commodityInfo.marketID,
          location = (byte) (index + 1)
        });
      cmdMinVo.mark = !this.m_hqForm.IsMultiCommidity ? (byte) 1 : (byte) (this.m_hqClient.rectCol * this.m_hqClient.rectRol);
      cmdMinVo.type = (byte) 0;
      cmdMinVo.date = 0;
      cmdMinVo.time = 0;
      cmdMinVo.commidityList = list;
      this.m_hqClient.sendThread.AskForData((CMDVO) cmdMinVo);
      CMDBillByVersionVO cmdBillByVersionVo = new CMDBillByVersionVO();
      cmdBillByVersionVo.marketID = this.m_hqClient.curCommodityInfo.marketID;
      cmdBillByVersionVo.type = (byte) 1;
      cmdBillByVersionVo.code = this.m_hqClient.curCommodityInfo.commodityCode;
      cmdMinVo.date = 0;
      cmdBillByVersionVo.time = (long) (this.m_rc.Height / 16);
      this.m_hqClient.sendThread.AskForData((CMDVO) cmdBillByVersionVo);
    }

    public override void Paint(Graphics g, int v)
    {
      try
      {
        ProductData productData = this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo);
        Font font1 = new Font("楷体_GB2312", 26f, FontStyle.Bold);
        this.m_iQuoteH = this.m_rc.Height;
        Font font2 = new Font("宋体", 12f, FontStyle.Regular);
        font1.Dispose();
        font2.Dispose();
        this.rcQuote = new Rectangle(this.m_rc.X + this.m_rc.Width - this.rcQuoteWidth, this.m_rc.Y, this.rcQuoteWidth, this.m_iQuoteH);
        this.draw_Quote = new Draw_Quote(this.m_hqClient);
        this.draw_Quote.Paint(g, this.rcQuote, productData, this.m_hqClient.curCommodityInfo, this.m_hqClient.iShowBuySellPrice, this.m_hqClient);
        this.rcMinLine = new Rectangle(this.m_rc.X, this.m_rc.Y, this.m_rc.Width - this.rcQuoteWidth - 10, this.m_rc.Height);
        if (this.m_hqForm.IsMultiCommidity)
        {
          if (this.m_hqClient.listRectInfo == null)
            this.m_hqClient.listRectInfo = new List<clickRect>();
          if (this.m_hqClient.listRectInfo.Count != this.m_hqClient.rectCol * this.m_hqClient.rectRol)
          {
            this.m_hqClient.listRectInfo.Clear();
            List<ProductData> afterGoodsInfo = this.getAfterGoodsInfo(this.m_hqClient.rectCol * this.m_hqClient.rectRol);
            int num1 = this.rcMinLine.Width / this.m_hqClient.rectCol;
            int num2 = this.rcMinLine.Height / this.m_hqClient.rectRol;
            int num3 = 0;
            for (int index1 = 0; index1 < this.m_hqClient.rectRol; ++index1)
            {
              for (int index2 = 0; index2 < this.m_hqClient.rectCol; ++index2)
              {
                int index3 = num3;
                while (index3 >= afterGoodsInfo.Count)
                  index3 -= afterGoodsInfo.Count;
                clickRect clickRect = new clickRect();
                Rectangle rc = new Rectangle();
                Draw_MinLine drawMinLine = new Draw_MinLine(this.m_hqForm, true);
                rc.X = this.rcMinLine.X + index2 * num1;
                rc.Y = this.rcMinLine.Y + index1 * num2;
                rc.Height = num2;
                rc.Width = num1;
                clickRect.OwnRect = rc;
                clickRect.OwnMinKLine = drawMinLine;
                clickRect.MinData = this.m_hqClient.GetProductData(afterGoodsInfo[index3].commodityInfo);
                if (clickRect.MinData != null)
                  this.m_hqClient.listRectInfo.Add(clickRect);
                drawMinLine.Paint(g, rc, clickRect.MinData);
                Pen pen = new Pen(SetInfo.RHColor.clGrid, 1f);
                if (clickRect.MinData == productData)
                  pen.Color = Color.BurlyWood;
                g.DrawRectangle(pen, new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2));
                ++num3;
              }
            }
          }
          else
          {
            int num1 = this.rcMinLine.Width / this.m_hqClient.rectCol;
            int num2 = this.rcMinLine.Height / this.m_hqClient.rectRol;
            int index1 = 0;
            for (int index2 = 0; index2 < this.m_hqClient.rectRol; ++index2)
            {
              for (int index3 = 0; index3 < this.m_hqClient.rectCol; ++index3)
              {
                Rectangle rc = new Rectangle();
                rc.X = this.rcMinLine.X + index3 * num1;
                rc.Y = this.rcMinLine.Y + index2 * num2;
                rc.Height = num2;
                rc.Width = num1;
                this.m_hqClient.listRectInfo[index1].OwnRect = rc;
                if (this.m_globalData.SelectNumMin - 1 != index1)
                {
                  this.m_hqClient.listRectInfo[index1].MinData = this.m_hqClient.GetProductData(this.m_hqClient.listData[index1].commodityInfo);
                  this.m_hqClient.listRectInfo[index1].OwnMinKLine.Paint(g, rc, this.m_hqClient.listRectInfo[index1].MinData);
                  g.DrawRectangle(new Pen(SetInfo.RHColor.clGrid, 1f), new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2));
                }
                else
                {
                  this.m_hqClient.listRectInfo[index1].MinData = productData;
                  if (productData != null)
                    this.m_hqClient.listData[index1] = productData;
                  this.m_hqClient.listRectInfo[index1].OwnMinKLine.Paint(g, rc, productData);
                  g.DrawRectangle(new Pen(Color.BurlyWood, 1f), new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2));
                }
                ++index1;
              }
            }
          }
          if (this.draw_MinLine.isDrawCursor)
          {
            for (int index = 0; index < this.m_hqClient.listRectInfo.Count; ++index)
            {
              if (this.m_hqClient.listRectInfo[index].OwnMinKLine.m_iPos >= 0)
                this.m_hqClient.listRectInfo[index].OwnMinKLine.DrawLabel(g);
            }
          }
          this.m_hqForm.EndPaint();
          for (int index = 0; index < this.m_hqClient.listRectInfo.Count; ++index)
          {
            if (this.m_hqClient.listRectInfo[index].OwnMinKLine.m_iPos >= 0)
            {
              if (this.draw_MinLine.isDrawCursor)
              {
                this.m_hqClient.listRectInfo[index].OwnMinKLine.m_YSrcBmp = (Bitmap) null;
                this.m_hqClient.listRectInfo[index].OwnMinKLine.DrawCursor(-1);
              }
              else
                this.m_hqClient.listRectInfo[index].OwnMinKLine.m_iPos = -1;
            }
          }
        }
        else
          this.draw_MinLine.Paint(g, this.rcMinLine, productData);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-Paint异常：" + ex.Message);
      }
    }

    private List<ProductData> getAfterGoodsInfo(int afterNum)
    {
      List<ProductData> list = new List<ProductData>();
      int num = -1;
      for (int index = 0; index < this.m_multiQuoteData.m_curQuoteList.Length; ++index)
      {
        if (this.m_hqClient.curCommodityInfo.commodityCode.Equals(this.m_multiQuoteData.m_curQuoteList[index].code) && this.m_hqClient.curCommodityInfo.marketID.Equals(this.m_multiQuoteData.m_curQuoteList[index].marketID))
        {
          num = index;
          break;
        }
      }
      if (num == -1 && this.m_multiQuoteData.m_curQuoteList.Length > 0)
        num = 0;
      if (this.m_multiQuoteData.m_curQuoteList.Length > 0)
      {
        for (int index1 = num; index1 < num + afterNum; ++index1)
        {
          ProductData productData = new ProductData();
          int index2 = index1;
          while (index2 >= this.m_multiQuoteData.m_curQuoteList.Length)
            index2 -= this.m_multiQuoteData.m_curQuoteList.Length;
          string _marketID = this.m_multiQuoteData.m_curQuoteList[index2].marketID;
          string _commodityCode = this.m_multiQuoteData.m_curQuoteList[index2].code;
          productData.commodityInfo = new CommodityInfo(_marketID, _commodityCode);
          list.Add(productData);
        }
      }
      return list;
    }

    protected override void Page_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        int x = e.X - this.m_hqForm.ScrollOffset.X;
        int y = e.Y - this.m_hqForm.ScrollOffset.Y;
        if (e.Button == MouseButtons.Left)
        {
          if (this.draw_Quote == null)
            return;
          if (y > this.rcQuote.Y + this.rcQuote.Height - this.draw_Quote.rcRightButton.Height && y < this.draw_Quote.rcRightButton.Y + this.draw_Quote.rcRightButton.Height)
            this.ClickButton(x, y);
          if (this.rcQuote.Contains(e.X, e.Y))
            this.draw_Quote.MouseLeftClick(e.X, e.Y, this.m_hqForm, this.m_hqClient);
          else if (this.m_hqForm.IsMultiCommidity)
          {
            for (int index1 = 0; index1 < this.m_hqClient.listRectInfo.Count; ++index1)
            {
              if (this.m_hqClient.listRectInfo[index1].OwnRect.Contains(e.X, e.Y))
              {
                if (this.m_globalData.SelectNumMin != index1 + 1)
                {
                  this.m_globalData.SelectNumMin = index1 + 1;
                  for (int index2 = 0; index2 < this.m_hqClient.listRectInfo.Count; ++index2)
                  {
                    if (index2 != this.m_globalData.SelectNumMin - 1)
                      this.m_hqClient.listRectInfo[index2].OwnMinKLine.m_iPos = -1;
                  }
                  if (this.m_hqClient.listRectInfo[index1].MinData != null)
                  {
                    this.m_hqClient.curCommodityInfo = this.m_hqClient.listRectInfo[index1].MinData.commodityInfo;
                    this.m_hqForm.Repaint();
                  }
                }
                if (index1 >= this.m_hqClient.listRectInfo.Count)
                  return;
                if (this.m_hqClient.listRectInfo[index1].OwnMinKLine.MouseDoubleClick(e.X, e.Y))
                {
                  this.m_hqForm.Repaint();
                  break;
                }
                break;
              }
            }
          }
          else if (this.draw_MinLine.MouseDoubleClick(e.X, e.Y))
            this.m_hqForm.Repaint();
        }
        ((HQClientForm) this.m_hqForm).mainWindow.Focus();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseClick异常：" + ex.Message);
      }
    }

    private void ClickButton(int x, int y)
    {
      MyButton myButton = this.draw_Quote.rightbuttonGraph.MouseLeftClicked(x, y, this.buttonUtils.RightButtonList, false);
      if (myButton == null)
        return;
      this.buttonUtils.CuRightrButtonName = myButton.Name;
      this.m_hqForm.Repaint();
    }

    protected override void Page_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      try
      {
        if (this.rcQuote.Contains(e.X, e.Y))
        {
          this.draw_Quote.MouseDoubleClick(e.X, e.Y, this.m_hqClient.GetProductData(this.m_hqClient.curCommodityInfo), this.m_hqForm, this.m_hqClient);
        }
        else
        {
          if (!this.rcMinLine.Contains(e.X, e.Y) || this.draw_MinLine == null || !this.m_hqClient.isClickMultiMarket)
            return;
          if (this.m_hqForm.IsMultiCommidity)
          {
            for (int index = 0; index < this.m_hqClient.listRectInfo.Count; ++index)
            {
              if (this.m_hqClient.listRectInfo[index].OwnRect.Contains(e.X, e.Y))
              {
                this.m_globalData.SelectNumMin = index + 1;
                if (this.m_hqClient.listRectInfo[index].MinData == null)
                  return;
                this.m_hqClient.curCommodityInfo = this.m_hqClient.listRectInfo[index].MinData.commodityInfo;
              }
            }
          }
          this.m_hqForm.IsMultiCommidity = !this.m_hqForm.IsMultiCommidity;
          this.m_hqForm.Repaint();
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseDoubleClick异常：" + ex.Message);
      }
    }

    protected override void Page_MouseMove(object sender, MouseEventArgs e)
    {
      try
      {
        if (this.rcMinLine.Contains(e.X, e.Y) && this.draw_MinLine != null)
        {
          if (this.m_hqForm.IsMultiCommidity)
          {
            for (int index = 0; index < this.m_hqClient.listRectInfo.Count; ++index)
            {
              if (this.m_hqClient.listRectInfo[index].OwnRect.Contains(e.X, e.Y))
              {
                if (this.m_hqClient.listRectInfo[index].OwnMinKLine.MouseDragged(e.X, e.Y))
                  this.m_hqForm.Repaint();
              }
              else if (this.m_hqClient.listRectInfo[index].OwnMinKLine.m_iPos != -1)
              {
                this.m_hqClient.listRectInfo[index].OwnMinKLine.m_iPos = -1;
                this.m_hqForm.Repaint();
              }
            }
          }
          else
          {
            if (!this.draw_MinLine.MouseDragged(e.X, e.Y))
              return;
            this.m_hqForm.Repaint();
          }
        }
        else if (this.rcQuote.Contains(e.X, e.Y))
        {
          this.draw_Quote.MouseMove(e.X, e.Y, this.m_hqForm);
        }
        else
        {
          if (!(this.m_hqForm.M_Cursor == Cursors.Hand))
            return;
          this.m_hqForm.M_Cursor = Cursors.Default;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-Page_MouseMove异常：" + ex.Message);
      }
    }

    protected override void Page_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        bool flag = false;
        switch (e.KeyData)
        {
          case Keys.Prior:
            this.m_hqForm.ChangeStock(true);
            flag = true;
            break;
          case Keys.Next:
            this.m_hqForm.ChangeStock(false);
            flag = true;
            break;
          case Keys.F10:
            this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
            break;
          default:
            flag = !this.m_hqForm.IsMultiCommidity ? this.draw_MinLine.KeyPressed(e) : this.m_hqClient.listRectInfo[this.m_globalData.SelectNumMin - 1].OwnMinKLine.KeyPressed(e);
            break;
        }
        this.m_hqForm.IsNeedRepaint = flag;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-Page_KeyDown异常：" + ex.Message);
      }
    }

    private void MakeMenus()
    {
      this.contextMenuStrip.Items.Clear();
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_PrevCommodity") + "  PageUp", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_PrevStock"));
      toolStripMenuItem1.Name = "prevstock";
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_NextCommodity") + "  PageDown", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_PostStock"));
      toolStripMenuItem2.Name = "poststock";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_Analysis") + "  F5", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_KLine"));
      toolStripMenuItem3.Name = "kline";
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_TradeList") + "  F1", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Bill"));
      toolStripMenuItem4.Name = "bill";
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_MultiQuote") + "  F2", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Quote"));
      toolStripMenuItem5.Name = "cmd_60";
      ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_ClassedList") + "  F4", (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_MarketStatus"));
      toolStripMenuItem6.Name = "cmd_80";
      ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_multiMarket"), (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_Cycle"));
      toolStripMenuItem7.Name = "multiMarket";
      ToolStripMenuItem toolStripMenuItem8 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_oneMarket"));
      toolStripMenuItem8.Name = "oneMarket";
      ToolStripMenuItem toolStripMenuItem9 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_twoMarket"));
      toolStripMenuItem9.Name = "twoMarket";
      ToolStripMenuItem toolStripMenuItem10 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_threeMarket"));
      toolStripMenuItem10.Name = "threeMarket";
      ToolStripMenuItem toolStripMenuItem11 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_fourMarket"));
      toolStripMenuItem11.Name = "fourMarket";
      ToolStripMenuItem toolStripMenuItem12 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_sixMarket"));
      toolStripMenuItem12.Name = "sixMarket";
      ToolStripMenuItem toolStripMenuItem13 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_nineMarket"));
      toolStripMenuItem13.Name = "nineMarket";
      ToolStripMenuItem toolStripMenuItem14 = new ToolStripMenuItem(this.pluginInfo.HQResourceManager.GetString("HQStr_CommodityInfo") + "  F10");
      toolStripMenuItem14.Name = "commodityInfo";
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem8);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem9);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem10);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem11);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem12);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem13);
      contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(((Page_Main) this).contextMenu_ItemClicked);
      toolStripMenuItem7.DropDown = (ToolStripDropDown) contextMenuStrip;
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem7);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem3);
      if (this.iProductType != 2 && this.iProductType != 3)
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem4);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      this.contextMenuStrip.Items.Add("-");
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem5);
      this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem6);
      if (this.m_hqForm.isDisplayF10Menu())
        this.contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem14);
      this.AddCommonMenu();
    }

    protected override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      try
      {
        CommodityInfoF.CommodityInfoClose();
        string name = e.ClickedItem.Name;
        if (name.IndexOf("cmd_") >= 0)
          this.m_hqForm.UserCommand(name.Substring(4));
        else if (name.Equals("kline"))
          this.m_hqForm.ShowPageKLine(this.m_hqClient.curCommodityInfo);
        else if (name.Equals("bill"))
          this.m_hqForm.UserCommand("01");
        else if (name.Equals("prevstock"))
          this.m_hqForm.ChangeStock(true);
        else if (name.Equals("poststock"))
          this.m_hqForm.ChangeStock(false);
        else if (name.Equals("commodityInfo"))
          this.m_hqForm.DisplayCommodityInfo(this.m_hqClient.curCommodityInfo.commodityCode);
        else if (name.Equals("oneMarket"))
        {
          this.m_hqClient.isClickMultiMarket = false;
          this.m_hqForm.IsMultiCommidity = false;
        }
        else if (name.Equals("twoMarket"))
        {
          this.m_hqClient.isClickMultiMarket = true;
          this.m_hqForm.IsMultiCommidity = true;
          this.m_hqClient.rectCol = 2;
          this.m_hqClient.rectRol = 1;
          this.m_hqForm.ShowPageMinLine();
        }
        else if (name.Equals("threeMarket"))
        {
          this.m_hqClient.isClickMultiMarket = true;
          this.m_hqForm.IsMultiCommidity = true;
          this.m_hqClient.rectCol = 3;
          this.m_hqClient.rectRol = 1;
          this.m_hqForm.ShowPageMinLine();
        }
        else if (name.Equals("fourMarket"))
        {
          this.m_hqClient.isClickMultiMarket = true;
          this.m_hqForm.IsMultiCommidity = true;
          this.m_hqClient.rectCol = 2;
          this.m_hqClient.rectRol = 2;
          this.m_hqForm.ShowPageMinLine();
        }
        else if (name.Equals("sixMarket"))
        {
          this.m_hqClient.isClickMultiMarket = true;
          this.m_hqForm.IsMultiCommidity = true;
          this.m_hqClient.rectCol = 3;
          this.m_hqClient.rectRol = 2;
          this.m_hqForm.ShowPageMinLine();
        }
        else if (name.Equals("nineMarket"))
        {
          this.m_hqClient.isClickMultiMarket = true;
          this.m_hqForm.IsMultiCommidity = true;
          this.m_hqClient.rectCol = 3;
          this.m_hqClient.rectRol = 3;
          this.m_hqForm.ShowPageMinLine();
        }
        else
          this.m_hqForm.UserCommand(name);
        this.m_hqForm.Repaint();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_MinLine-contextMenu_ItemClicked异常：" + ex.Message);
      }
    }

    public override void Dispose()
    {
      GC.Collect();
    }
  }
}
