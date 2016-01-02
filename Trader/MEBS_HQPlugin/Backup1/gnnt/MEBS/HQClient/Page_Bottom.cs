// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_Bottom
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_Bottom
  {
    private int gap = 2;
    private HQForm m_hqForm;
    public Rectangle rc;
    private Rectangle m_rcIndex;
    private Rectangle m_rcConnectState;
    private Rectangle m_rcTime;
    public PluginInfo pluginInfo;
    public SetInfo setInfo;

    public Page_Bottom(Rectangle _rc, HQForm hqForm)
    {
      this.rc = _rc;
      this.m_hqForm = hqForm;
      this.pluginInfo = hqForm.CurHQClient.pluginInfo;
      this.setInfo = hqForm.CurHQClient.setInfo;
    }

    internal void Paint(Graphics g)
    {
      try
      {
        this.rc.X = 0;
        this.rc.Y = 0;
        Font font = new Font("宋体", 12f);
        SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clBackGround);
        Pen pen = new Pen(SetInfo.RHColor.clGrid);
        int width1 = (int) g.MeasureString("2005-12-24 09:30", font).Width;
        int width2 = (int) g.MeasureString(" " + this.pluginInfo.HQResourceManager.GetString("HQStr_DisConnected") + " ", font).Width;
        this.m_rcIndex = new Rectangle(this.rc.X, this.rc.Y, this.rc.Width - width1 - width2, this.rc.Height);
        this.m_rcConnectState = new Rectangle(this.rc.X + this.rc.Width - width1 - width2, this.rc.Y, width2, this.rc.Height);
        this.m_rcTime = new Rectangle(this.rc.X + this.rc.Width - width1, this.rc.Y, width1, this.rc.Height);
        g.FillRectangle((Brush) solidBrush, this.rc.X, this.rc.Y, this.rc.Width, this.rc.Height);
        g.DrawLine(pen, this.rc.X, this.rc.Y, this.rc.Width, this.rc.Y);
        g.DrawLine(pen, this.m_rcTime.X - 1, this.rc.Y, this.m_rcTime.X - 1, this.rc.Y + this.rc.Height);
        if (this.rc.Height < font.Height / 2)
          return;
        this.PaintIndex(g);
        g.DrawLine(pen, this.m_rcConnectState.X - 1, this.rc.Y, this.m_rcConnectState.X - 1, this.rc.Y + this.rc.Height);
        this.PaintConnectState(g);
        this.PaintCurTime(g);
        pen.Dispose();
        solidBrush.Dispose();
        font.Dispose();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Bottom-Paint异常：" + ex.Message);
      }
    }

    private void ComputeAndPaintIndex(Graphics g)
    {
      try
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 2;
        int num4 = this.m_rcIndex.X + 2;
        Font font = new Font("宋体", 12f);
        SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clProductName);
        for (int index = 0; index < this.m_hqForm.CurHQClient.m_quoteList.Length; ++index)
        {
          ProductDataVO productDataVo = this.m_hqForm.CurHQClient.m_quoteList[index];
          if (!this.m_hqForm.CurHQClient.isIndex(new CommodityInfo(productDataVo.marketID, productDataVo.code)))
          {
            num1 += productDataVo.reserveCount;
            num2 += (int) productDataVo.totalAmount;
          }
        }
        string string1 = this.pluginInfo.HQResourceManager.GetString("HQStr_Volume");
        string str1 = num2 <= 0 ? string1 + "——" : string1 + (object) num2;
        if (this.m_rcIndex.X + this.m_rcIndex.Width - num4 < (int) g.MeasureString(str1, font).Width)
          return;
        solidBrush.Color = SetInfo.RHColor.clVolume;
        g.DrawString(str1, font, (Brush) solidBrush, (float) this.m_rcIndex.X, (float) (this.m_rcIndex.Y + num3));
        int num5 = num4 + ((int) g.MeasureString(str1, font).Width + 10);
        string string2 = this.pluginInfo.HQResourceManager.GetString("HQStr_Order");
        string str2 = num1 <= 0 ? string2 + "——" : string2 + (object) num1;
        if (this.m_rcIndex.X + this.m_rcIndex.Width - num5 < (int) g.MeasureString(str2, font).Width)
          return;
        solidBrush.Color = SetInfo.RHColor.clReserve;
        g.DrawString(str2, font, (Brush) solidBrush, (float) num5, (float) (this.m_rcIndex.Y + num3));
        solidBrush.Dispose();
        font.Dispose();
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "ComputeAndPaintIndex异常：" + ex.Message);
      }
    }

    private void PaintIndex(Graphics g)
    {
      try
      {
        if (this.m_hqForm.CurHQClient.m_bShowIndexAtBottom == 0)
          return;
        if (this.m_hqForm.CurHQClient.indexMainCode.Length == 0 || string.Compare((string) this.pluginInfo.HTConfig[(object) "ShowIndex"], "false", true) == 0)
        {
          if (this.m_hqForm.CurHQClient.CurrentPage != 0)
            return;
          this.ComputeAndPaintIndex(g);
        }
        else
        {
          CodeTable codeTable = (CodeTable) null;
          if (this.m_hqForm.CurHQClient.m_htProduct != null)
          {
            CommodityInfo commodityInfo = CommodityInfo.DealCode(this.m_hqForm.CurHQClient.indexMainCode);
            if (this.m_hqForm.CurHQClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)] != null)
              codeTable = (CodeTable) this.m_hqForm.CurHQClient.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
          }
          string str1 = codeTable == null ? "  指数  " : codeTable.sName;
          ProductData productData = this.m_hqForm.CurHQClient.GetProductData(CommodityInfo.DealCode(this.m_hqForm.CurHQClient.indexMainCode));
          if (productData == null || productData.realData == null)
          {
            this.ComputeAndPaintIndex(g);
          }
          else
          {
            Font font = new Font("宋体", 12f);
            SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clProductName);
            int num1 = this.m_rcIndex.X + 2;
            if (this.m_rcIndex.X + this.m_rcIndex.Width - num1 < (int) g.MeasureString(str1, font).Width)
              return;
            g.DrawString(str1, font, (Brush) solidBrush, (float) num1, (float) (this.m_rcIndex.Y + this.gap));
            int num2 = num1 + ((int) g.MeasureString(str1, font).Width + 10);
            string str2 = (double) productData.realData.curPrice <= 0.0 ? "——" : M_Common.FloatToString((double) productData.realData.curPrice, this.m_hqForm.CurHQClient.m_iPrecisionIndex);
            if (this.m_rcIndex.X + this.m_rcIndex.Width - num2 < (int) g.MeasureString(str2, font).Width)
              return;
            solidBrush.Color = this.GetPriceColor(productData.realData.curPrice, productData.realData.yesterBalancePrice);
            g.DrawString(str2, font, (Brush) solidBrush, (float) num2, (float) (this.m_rcIndex.Y + this.gap));
            int num3 = num2 + ((int) g.MeasureString(str2, font).Width + 10);
            string str3 = (double) productData.realData.curPrice <= 0.0 || (double) productData.realData.yesterBalancePrice <= 0.0 ? "——" : M_Common.FloatToString((double) productData.realData.curPrice - (double) productData.realData.yesterBalancePrice, this.m_hqForm.CurHQClient.m_iPrecisionIndex);
            if ((double) productData.realData.curPrice > (double) productData.realData.yesterBalancePrice)
              str3 = "+" + str3;
            else if ((double) productData.realData.curPrice * 100.0 == (double) productData.realData.yesterBalancePrice * 100.0)
              str3 = "——";
            if (this.m_rcIndex.X + this.m_rcIndex.Width - num3 < (int) g.MeasureString(str3, font).Width)
              return;
            solidBrush.Color = this.GetPriceColor(productData.realData.curPrice, productData.realData.yesterBalancePrice);
            g.DrawString(str3, font, (Brush) solidBrush, (float) num3, (float) (this.m_rcIndex.Y + this.gap));
            int num4 = num3 + ((int) g.MeasureString(str3, font).Width + 10);
            string string1 = this.pluginInfo.HQResourceManager.GetString("HQStr_Volume");
            string str4 = productData.realData.totalAmount <= 0L ? string1 + "——" : string1 + Convert.ToString((int) productData.realData.totalAmount);
            if (this.m_rcIndex.X + this.m_rcIndex.Width - num4 < (int) g.MeasureString(str4, font).Width)
              return;
            solidBrush.Color = SetInfo.RHColor.clVolume;
            g.DrawString(str4, font, (Brush) solidBrush, (float) num4, (float) (this.m_rcIndex.Y + this.gap));
            int num5 = num4 + ((int) g.MeasureString(str4, font).Width + 10);
            string string2 = this.pluginInfo.HQResourceManager.GetString("HQStr_Order");
            string str5 = productData.realData.reserveCount <= 0 ? string2 + "——" : string2 + Convert.ToString(productData.realData.reserveCount);
            if (this.m_rcIndex.X + this.m_rcIndex.Width - num5 < (int) g.MeasureString(str5, font).Width)
              return;
            solidBrush.Color = SetInfo.RHColor.clReserve;
            g.DrawString(str5, font, (Brush) solidBrush, (float) num5, (float) (this.m_rcIndex.Y + this.gap));
            solidBrush.Dispose();
            font.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Bottom-PaintIndex异常：" + ex.Message);
      }
    }

    private Color GetPriceColor(float fPrice, float fBenchMark)
    {
      if ((double) fPrice > (double) fBenchMark)
        return SetInfo.RHColor.clIncrease;
      if ((double) fPrice < (double) fBenchMark)
        return SetInfo.RHColor.clDecrease;
      return SetInfo.RHColor.clEqual;
    }

    private void PaintConnectState(Graphics g)
    {
      Font font = new Font("宋体", 12f);
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clDecrease);
      string @string;
      if (this.m_hqForm.CurHQClient.Connected)
      {
        @string = this.pluginInfo.HQResourceManager.GetString("HQStr_Connected");
      }
      else
      {
        @string = this.pluginInfo.HQResourceManager.GetString("HQStr_DisConnected");
        solidBrush.Color = SetInfo.RHColor.clGrid;
      }
      g.DrawString(@string, font, (Brush) solidBrush, (float) this.m_rcConnectState.X, (float) (this.m_rcConnectState.Y + this.gap));
      solidBrush.Dispose();
      font.Dispose();
    }

    private void PaintCurTime(Graphics g)
    {
      if (this.m_hqForm.CurHQClient.m_iDate == 0 || this.m_hqForm.CurHQClient.m_iTime == 0)
        return;
      Font font = new Font("宋体", 12f);
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
      string str = TradeTimeVO.HHMMIntToString(this.m_hqForm.CurHQClient.m_iTime / 100);
      if (str.EndsWith(":"))
        str = str.Substring(0, str.Length - 1);
      string s = this.m_hqForm.CurHQClient.m_iDate.ToString("####-##-##") + " " + str;
      g.DrawString(s, font, (Brush) solidBrush, (float) this.m_rcTime.X, (float) (this.m_rcTime.Y + this.gap));
      solidBrush.Dispose();
      font.Dispose();
    }

    private void PaintLocalCurTime(Graphics g)
    {
      Font font = new Font("宋体", 12f);
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
      DateTime now = DateTime.Now;
      string str1 = now.Hour.ToString();
      string str2 = now.Minute.ToString();
      if (str1.Length == 1)
        str1 = "0" + (object) now.Hour;
      if (str2.Length == 1)
        str2 = "0" + (object) now.Minute;
      string s = (string) (object) now.Year + (object) "-" + (string) (object) now.Month + "-" + (string) (object) now.Day + "  " + str1 + ":" + str2;
      g.DrawString(s, font, (Brush) solidBrush, (float) this.m_rcTime.X, (float) (this.m_rcTime.Y + this.gap));
      solidBrush.Dispose();
      font.Dispose();
    }
  }
}
