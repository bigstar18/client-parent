// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Draw_MinLine
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class Draw_MinLine : IDisposable
  {
    public int m_iPos = -1;
    private int m_iPosY = -1;
    private int m_minReserveCount = -1;
    private HQForm m_hqForm;
    private HQClientMain m_hqClient;
    private bool bLarge;
    private int m_iTotalMinNum;
    private int m_iMinLineNum;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private int iNum;
    private int m_iPrecision;
    private float m_maxPrice;
    private float m_minPrice;
    private long m_maxVolume;
    private int m_maxReserveCount;
    private ProductData m_product;
    private int iHeight;
    private int iWidth;
    private Rectangle m_rcPrice;
    private Rectangle m_rcVolume;
    private Rectangle m_rcLabel;
    private int m_iProductType;
    private TradeTimeVO[] TimeRange;
    private Rectangle rc_;
    public Bitmap m_YSrcBmp;
    private Rectangle rcYCoordinate;
    private int iPriceGridNum;
    public bool isDrawCursor;

    public Draw_MinLine(HQForm hqForm, bool _bLarge)
    {
      this.m_hqForm = hqForm;
      this.m_hqClient = hqForm.CurHQClient;
      this.pluginInfo = this.m_hqClient.pluginInfo;
      this.setInfo = this.m_hqClient.setInfo;
      this.bLarge = _bLarge;
    }

    internal void Paint(Graphics g, Rectangle rc, ProductData product)
    {
      try
      {
        this.m_product = product;
        this.rc_ = rc;
        if (product != null)
        {
          if (this.m_hqClient.m_htMarketData.Count > 0)
            this.TimeRange = ((MarketDataVO) this.m_hqClient.m_htMarketData[(object) product.commodityInfo.marketID]).m_timeRange;
          this.m_iProductType = this.m_hqClient.getProductType(product.commodityInfo);
          this.GetMaxMinPrice();
          if (product.aMinLine != null)
          {
            this.m_maxVolume = 0L;
            this.m_maxReserveCount = 0;
            this.m_minReserveCount = -1;
            for (int index = 0; index < product.aMinLine.Count; ++index)
            {
              MinDataVO minDataVo = (MinDataVO) product.aMinLine[index];
              float num = index != 0 ? (float) (minDataVo.totalAmount - ((MinDataVO) this.m_product.aMinLine[index - 1]).totalAmount) : (float) minDataVo.totalAmount;
              if ((double) this.m_maxVolume < (double) num)
                this.m_maxVolume = (long) num;
              if (this.m_maxReserveCount < minDataVo.reserveCount)
                this.m_maxReserveCount = minDataVo.reserveCount;
              if (this.m_minReserveCount == -1 || this.m_minReserveCount > minDataVo.reserveCount)
                this.m_minReserveCount = minDataVo.reserveCount;
            }
          }
        }
        this.DrawGrid(g, rc);
        if (this.iPriceGridNum > 0)
          this.DrawTrace(g);
        if (this.m_hqForm.IsMultiCommidity && this.bLarge && product != null)
        {
          CodeTable codeTable = (CodeTable) null;
          if (this.m_hqClient.m_htProduct != null && this.m_hqClient.m_htProduct[(object) (product.commodityInfo.marketID + product.commodityInfo.commodityCode)] != null)
            codeTable = (CodeTable) this.m_hqClient.m_htProduct[(object) (product.commodityInfo.marketID + product.commodityInfo.commodityCode)];
          string s = codeTable == null ? "————" : codeTable.sName;
          g.DrawString(s, new Font("宋体", 10f, FontStyle.Regular), (Brush) new SolidBrush(SetInfo.RHColor.clProductName), (float) (this.m_rcPrice.X + 1), (float) (rc.Y + 1));
        }
        if (this.m_hqForm.IsMultiCommidity || this.m_hqClient.CurrentPage != 1)
          return;
        if (this.m_iPos >= 0 && this.m_iPos < this.iNum)
          this.DrawLabel(g);
        if (this.bLarge)
          this.m_hqForm.EndPaint();
        if (this.m_iPos < 0)
          return;
        if (this.isDrawCursor)
        {
          this.m_YSrcBmp = (Bitmap) null;
          this.DrawCursor(-1);
        }
        else
        {
          this.m_iPos = -1;
          this.m_iPosY = -1;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Draw_MinLine-Paint异常：" + ex.Message);
      }
    }

    public void DrawLabel(Graphics g)
    {
      if (this.m_iPos < 0 || this.m_iPos > this.m_iMinLineNum - 1 || g == null)
        return;
      Rectangle rectangle = new Rectangle(this.m_rcLabel.X - 1, this.m_rcLabel.Y - 1, this.m_rcLabel.Width + 1, this.m_rcLabel.Height + 1);
      Font font = new Font("宋体", 10f, FontStyle.Regular);
      if (((HQClientForm) this.m_hqForm).m_rcBottom.Y < rectangle.Y + font.Height * 12)
        rectangle.Y = this.rc_.Y + this.rc_.Height - font.Height * 12;
      using (Bitmap bitmap = new Bitmap(rectangle.Width, font.Height * 12))
      {
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        {
          graphics.Clear(SetInfo.RHColor.clBackGround);
          SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
          Pen pen = new Pen(SetInfo.RHColor.clNumber);
          graphics.DrawRectangle(pen, 0, 0, this.m_rcLabel.Width, font.Height * 12 - 2);
          int num1 = 1;
          int num2 = 1;
          solidBrush.Color = SetInfo.RHColor.clItem;
          graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Time"), font, (Brush) solidBrush, (float) num1, (float) num2);
          int num3 = num2 + font.Height;
          string str1 = TradeTimeVO.HHMMSSIntToString(M_Common.GetTimeFromMinLineIndex(this.m_iPos, this.TimeRange, this.m_hqClient.m_iMinLineInterval));
          if (this.m_hqClient.m_iMinLineInterval == 60)
            str1 = str1.Substring(0, 5);
          int num4 = rectangle.Width - (int) graphics.MeasureString(str1, font).Width - 1;
          solidBrush.Color = SetInfo.RHColor.clEqual;
          graphics.DrawString(str1, font, (Brush) solidBrush, (float) num4, (float) num3);
          if (this.m_iPos >= 0 && this.m_iPos < this.iNum)
          {
            int num5 = 1;
            int num6 = num3 + font.Height;
            solidBrush.Color = SetInfo.RHColor.clItem;
            graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Price"), font, (Brush) solidBrush, (float) num5, (float) num6);
            int num7 = num6 + font.Height;
            if (this.m_product == null || this.m_product.aMinLine == null || this.m_product.aMinLine.Count < this.m_iPos)
              return;
            MinDataVO minDataVo = (MinDataVO) this.m_product.aMinLine[this.m_iPos];
            if (minDataVo != null)
            {
              string str2 = M_Common.FloatToString((double) minDataVo.curPrice, this.m_iPrecision);
              if (this.m_product.realData == null)
                return;
              solidBrush.Color = (double) minDataVo.curPrice <= (double) this.m_product.realData.yesterBalancePrice ? ((double) minDataVo.curPrice >= (double) this.m_product.realData.yesterBalancePrice ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num8 = rectangle.Width - (int) graphics.MeasureString(str2, font).Width - 1;
              graphics.DrawString(str2, font, (Brush) solidBrush, (float) num8, (float) num7);
            }
            int num9 = 1;
            int num10 = num7 + font.Height;
            solidBrush.Color = SetInfo.RHColor.clItem;
            graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ChangeValue"), font, (Brush) solidBrush, (float) num9, (float) num10);
            int num11 = num10 + font.Height;
            if (minDataVo != null)
            {
              string str2 = M_Common.FloatToString((double) minDataVo.curPrice - (double) this.m_product.realData.yesterBalancePrice, this.m_iPrecision);
              if ((double) minDataVo.curPrice > (double) this.m_product.realData.yesterBalancePrice)
              {
                solidBrush.Color = SetInfo.RHColor.clIncrease;
                str2 = "+" + str2;
              }
              else
                solidBrush.Color = (double) minDataVo.curPrice >= (double) this.m_product.realData.yesterBalancePrice ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease;
              int num8 = rectangle.Width - (int) graphics.MeasureString(str2, font).Width - 1;
              graphics.DrawString(str2, font, (Brush) solidBrush, (float) num8, (float) num11);
            }
            int num12 = 1;
            int num13 = num11 + font.Height;
            solidBrush.Color = SetInfo.RHColor.clItem;
            graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Balance"), font, (Brush) solidBrush, (float) num12, (float) num13);
            int num14 = num13 + font.Height;
            if (minDataVo != null)
            {
              string str2 = M_Common.FloatToString((double) minDataVo.averPrice, this.m_iPrecision);
              solidBrush.Color = (double) minDataVo.averPrice <= (double) this.m_product.realData.yesterBalancePrice ? ((double) minDataVo.averPrice >= (double) this.m_product.realData.yesterBalancePrice ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
              int num8 = rectangle.Width - (int) graphics.MeasureString(str2, font).Width - 1;
              graphics.DrawString(str2, font, (Brush) solidBrush, (float) num8, (float) num14);
            }
            int num15 = 1;
            int num16 = num14 + font.Height;
            solidBrush.Color = SetInfo.RHColor.clItem;
            graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Volume"), font, (Brush) solidBrush, (float) num15, (float) num16);
            int num17 = num16 + font.Height;
            if (minDataVo != null)
            {
              string str2 = Convert.ToString(this.m_iPos != 0 ? (int) (float) (minDataVo.totalAmount - ((MinDataVO) this.m_product.aMinLine[this.m_iPos - 1]).totalAmount) : (int) (float) minDataVo.totalAmount);
              solidBrush.Color = SetInfo.RHColor.clVolume;
              int num8 = rectangle.Width - (int) graphics.MeasureString(str2, font).Width - 1;
              graphics.DrawString(str2, font, (Brush) solidBrush, (float) num8, (float) num17);
            }
            int num18 = 1;
            int num19 = num17 + font.Height;
            solidBrush.Color = SetInfo.RHColor.clItem;
            graphics.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Order"), font, (Brush) solidBrush, (float) num18, (float) num19);
            int num20 = num19 + font.Height;
            if (minDataVo != null)
            {
              string str2 = Convert.ToString(minDataVo.reserveCount);
              solidBrush.Color = SetInfo.RHColor.clReserve;
              int num8 = rectangle.Width - (int) graphics.MeasureString(str2, font).Width - 1;
              graphics.DrawString(str2, font, (Brush) solidBrush, (float) num8, (float) num20);
            }
          }
          font.Dispose();
          pen.Dispose();
          solidBrush.Dispose();
          g.DrawImage((Image) bitmap, rectangle.X, rectangle.Y);
        }
      }
    }

    private void DrawLabel()
    {
      if (!this.m_hqForm.IsEndPaint)
        return;
      using (Graphics mGraphics = this.m_hqForm.M_Graphics)
      {
        this.m_hqForm.TranslateTransform(mGraphics);
        this.DrawLabel(mGraphics);
      }
    }

    private void DrawOriginalCursor(Graphics g)
    {
      int xfromMinLineIndex = this.GetXFromMinLineIndex(this.m_iPos);
      GDIDraw.XorLine(g, xfromMinLineIndex, this.m_rcPrice.Y + 1, xfromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
    }

    public void DrawCursor(int iNewPos)
    {
      int iNewPosY = -1;
      if (iNewPos >= 0 && this.iNum > 0 && (iNewPos < this.iNum && this.m_product != null) && this.m_product.aMinLine != null)
      {
        MinDataVO minDataVo = (MinDataVO) this.m_product.aMinLine[iNewPos];
        if (minDataVo != null)
          iNewPosY = this.GetYFromPrice(minDataVo.curPrice);
      }
      this.DrawCursor(iNewPos, iNewPosY);
    }

    private void DrawCursor(int iNewPosX, int iNewPosY)
    {
      using (Graphics mGraphics = this.m_hqForm.M_Graphics)
      {
        if (!this.m_hqForm.IsEndPaint)
          return;
        if (this.m_iPos >= 0)
        {
          int xfromMinLineIndex = this.GetXFromMinLineIndex(this.m_iPos);
          GDIDraw.XorLine(mGraphics, xfromMinLineIndex, this.m_rcPrice.Y + 1, xfromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
        }
        if (this.m_iPosY >= 0)
        {
          GDIDraw.XorLine(mGraphics, this.m_rcPrice.X, this.m_iPosY, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_iPosY, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
          if (this.m_YSrcBmp != null)
            mGraphics.DrawImage((Image) this.m_YSrcBmp, this.rcYCoordinate.X, this.rcYCoordinate.Y);
        }
        if (iNewPosX >= 0)
        {
          this.m_iPos = iNewPosX;
          int xfromMinLineIndex = this.GetXFromMinLineIndex(this.m_iPos);
          GDIDraw.XorLine(mGraphics, xfromMinLineIndex, this.m_rcPrice.Y + 1, xfromMinLineIndex, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
        }
        if (iNewPosY >= 0)
        {
          if (iNewPosY > this.m_rcPrice.Y + this.m_rcPrice.Height && iNewPosY < this.m_rcVolume.Y)
          {
            this.m_iPosY = -1;
          }
          else
          {
            this.m_iPosY = iNewPosY;
            GDIDraw.XorLine(mGraphics, this.m_rcPrice.X, this.m_iPosY, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_iPosY, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
          }
        }
        if (this.m_iPosY < 0)
          return;
        this.rcYCoordinate = new Rectangle(this.m_rcPrice.Width + this.m_rcPrice.X, this.m_iPosY, 45, 13);
        using (Bitmap bitmap = new Bitmap(this.rcYCoordinate.Width, this.rcYCoordinate.Height))
        {
          using (Graphics graphics1 = Graphics.FromImage((Image) bitmap))
          {
            graphics1.Clear(SetInfo.RHColor.clHighlight);
            SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clHighlight);
            Pen pen = new Pen(SetInfo.RHColor.clGrid);
            graphics1.DrawRectangle(pen, 0, 0, this.rcYCoordinate.Width - 1, this.rcYCoordinate.Height - 1);
            Font font = new Font("宋体", 10f, FontStyle.Regular);
            solidBrush.Color = SetInfo.RHColor.clMinLine;
            string s = "";
            if (this.m_iPosY >= this.m_rcPrice.Y && this.m_iPosY <= this.m_rcPrice.Y + this.m_rcPrice.Height)
              s = string.Concat((object) this.GetPriceFromY(this.m_iPosY));
            else if (this.m_iPosY >= this.m_rcVolume.Y && this.m_iPosY <= this.m_rcVolume.Y + this.m_rcVolume.Height)
              s = string.Concat((object) this.GetReserveFromY(this.m_iPosY));
            graphics1.DrawString(s, font, (Brush) solidBrush, 0.0f, 0.0f);
            font.Dispose();
            pen.Dispose();
            solidBrush.Dispose();
            this.m_YSrcBmp = new Bitmap(this.rcYCoordinate.Width, this.rcYCoordinate.Height);
            Graphics graphics2 = Graphics.FromImage((Image) this.m_YSrcBmp);
            IntPtr hdc1 = mGraphics.GetHdc();
            IntPtr hdc2 = graphics2.GetHdc();
            GDIDraw.BitBlt(hdc2, 0, 0, this.rcYCoordinate.Width, this.rcYCoordinate.Height, hdc1, this.rcYCoordinate.X, this.rcYCoordinate.Y, 13369376);
            mGraphics.ReleaseHdc(hdc1);
            graphics2.ReleaseHdc(hdc2);
            mGraphics.DrawImage((Image) bitmap, this.rcYCoordinate.X, this.rcYCoordinate.Y);
          }
        }
      }
    }

    private bool inTrade(int minLineIndex, int[] tradeSecNo)
    {
      if (this.TimeRange == null || this.TimeRange.Length == 0)
        return false;
      int num1 = M_Common.GetTimeFromMinLineIndex(minLineIndex, this.TimeRange, this.m_hqClient.m_iMinLineInterval) / 100;
      for (int index1 = 0; index1 < tradeSecNo.Length; ++index1)
      {
        if (tradeSecNo[index1] > this.TimeRange.Length)
          return true;
        for (int index2 = 0; index2 < this.TimeRange.Length; ++index2)
        {
          if (this.TimeRange[index2].orderID == tradeSecNo[index1])
          {
            int num2 = this.TimeRange[index2].beginTime;
            int num3 = this.TimeRange[index2].endTime;
            int num4 = num1;
            if (num3 < num2)
            {
              num3 += 2400;
              if (num4 < num2)
                num4 += 2400;
            }
            if (num4 >= num2 && num4 <= num3)
              return true;
          }
        }
      }
      return false;
    }

    private int GetXFromMinLineIndex(int index)
    {
      if (this.m_iMinLineNum == 0)
        return 0;
      if (this.m_iTotalMinNum == 1)
        return this.m_rcPrice.X + 1 + this.m_rcPrice.Width - 2;
      if (index >= this.m_iMinLineNum)
        index = this.m_iMinLineNum - 1;
      return this.m_rcPrice.X + 1 + index * (this.m_rcPrice.Width - 2) / (this.m_iMinLineNum - 1);
    }

    private int GetYFromVolume(long volume)
    {
      if (0L >= this.m_maxVolume)
        return this.m_rcVolume.Y + this.m_rcVolume.Height - 1;
      return this.m_rcVolume.Y + this.m_rcVolume.Height - 1 - (int) ((double) volume * (double) (this.m_rcVolume.Height - 1) / (double) this.m_maxVolume);
    }

    private int GetYFromReserve(int reserveCount)
    {
      if (0 >= this.m_maxReserveCount || reserveCount == 0)
        return this.m_rcVolume.Y + this.m_rcVolume.Height - 1;
      return this.m_rcVolume.Y + this.m_rcVolume.Height - 1 - (int) ((double) (reserveCount - this.m_minReserveCount) * (double) (this.m_rcVolume.Height - 1) / (double) (this.m_maxReserveCount - this.m_minReserveCount));
    }

    private void DrawTrace(Graphics g)
    {
      Pen pen = new Pen(SetInfo.RHColor.clBackGround);
      if (this.m_product == null || this.m_product.realData == null || (this.m_product.aMinLine == null || (double) this.m_product.realData.yesterBalancePrice < 0.00999999977648258))
      {
        if (this.m_product == null || this.m_product.realData != null)
          return;
        Logger.wirte(MsgType.Warning, "为空了！！！！！！！！！！！！！！！！");
      }
      else
      {
        lock (this.m_product)
        {
          int local_1 = this.m_rcPrice.X;
          int local_5;
          int local_3 = local_5 = this.GetYFromPrice(this.m_product.realData.yesterBalancePrice);
          this.iNum = this.m_product.aMinLine.Count;
          MinDataVO local_8;
          if (this.iNum > 0)
          {
            local_8 = (MinDataVO) this.m_product.aMinLine[this.iNum - 1];
          }
          else
          {
            local_8 = new MinDataVO();
            local_8.averPrice = this.m_product.realData.yesterBalancePrice;
            local_8.curPrice = this.m_product.realData.yesterBalancePrice;
            local_8.reserveCount = 0;
            local_8.totalAmount = 0L;
            local_8.totalMoney = 0.0;
          }
          MarketDataVO local_9 = (MarketDataVO) this.m_hqClient.m_htMarketData[(object) this.m_product.commodityInfo.marketID];
          int local_10 = M_Common.GetMinLineIndexFromTime(local_9.date, local_9.time, local_9.m_timeRange, this.m_hqClient.m_iMinLineInterval);
          for (int local_11 = this.iNum; local_11 < local_10 + 1; ++local_11)
            this.m_product.aMinLine.Add((object) new MinDataVO()
            {
              averPrice = local_8.averPrice,
              curPrice = local_8.curPrice,
              reserveCount = local_8.reserveCount,
              totalAmount = local_8.totalAmount,
              totalMoney = local_8.totalMoney
            });
          this.iNum = this.m_product.aMinLine.Count;
          CodeTable local_13 = (CodeTable) this.m_hqClient.m_htProduct[(object) (this.m_product.commodityInfo.marketID + this.m_product.commodityInfo.commodityCode)];
          bool local_14 = this.m_hqClient.isIndex(this.m_product.commodityInfo);
          int local_15 = 0;
          for (int local_17 = 0; local_17 < this.iNum && local_17 < this.m_product.aMinLine.Count; ++local_17)
          {
            MinDataVO local_8_1 = (MinDataVO) this.m_product.aMinLine[local_17];
            bool local_18 = local_14 || local_13 == null || this.inTrade(local_17, local_13.tradeSecNo);
            if ((double) local_8_1.curPrice == 0.0)
              local_8_1.curPrice = local_8_1.averPrice = this.m_product.realData.yesterBalancePrice;
            int local_2 = this.GetXFromMinLineIndex(local_17);
            int local_4 = (double) local_8_1.curPrice < (double) this.m_product.realData.lowPrice || (double) local_8_1.curPrice > (double) this.m_product.realData.highPrice ? local_3 : this.GetYFromPrice(local_8_1.curPrice);
            float local_19_1 = local_8_1.averPrice;
            int local_6 = (double) local_19_1 < (double) this.m_product.realData.lowPrice || (double) local_19_1 > (double) this.m_product.realData.highPrice ? local_5 : this.GetYFromPrice(local_19_1);
            float local_20 = local_17 != 0 ? (float) (local_8_1.totalAmount - ((MinDataVO) this.m_product.aMinLine[local_17 - 1]).totalAmount) : (float) local_8_1.totalAmount;
            if (2 != this.m_iProductType && 3 != this.m_iProductType)
            {
              pen.Color = SetInfo.RHColor.clVolume;
              if (local_18)
                g.DrawLine(pen, local_1, local_5, local_2, local_6);
            }
            if ((double) local_20 > 0.0 && local_18)
            {
              int local_7 = this.GetYFromVolume((long) local_20);
              pen.Color = SetInfo.RHColor.clVolume;
              g.DrawLine(pen, local_2, this.m_rcVolume.Y + this.m_rcVolume.Height - 1, local_2, local_7);
            }
            int local_16_1 = this.GetYFromReserve(local_8_1.reserveCount);
            if (local_17 == 0)
              local_15 = local_16_1;
            if (local_18)
            {
              pen.Color = SetInfo.RHColor.clMinLine;
              g.DrawLine(pen, local_1, local_3, local_2, local_4);
              if (local_8_1.reserveCount > 0 && local_15 > 0)
              {
                pen.Color = SetInfo.RHColor.clReserve;
                g.DrawLine(pen, local_1, local_15, local_2, local_16_1);
              }
            }
            local_1 = local_2;
            local_3 = local_4;
            local_5 = local_6;
            local_15 = local_16_1;
          }
          pen.Dispose();
        }
      }
    }

    private void DrawGrid(Graphics g, Rectangle rc)
    {
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clGrid);
      Pen pen = new Pen(SetInfo.RHColor.clGrid);
      Font font = !this.bLarge ? new Font("宋体", 9f, FontStyle.Regular) : new Font("宋体", 10f, FontStyle.Regular);
      this.iHeight = font.Height;
      this.iWidth = (int) g.MeasureString("AAAAAAAAAA", font).Width / 10;
      int width1;
      int width2;
      int height1;
      int height2;
      if (this.bLarge)
      {
        width1 = this.iWidth * 9 - 1;
        width2 = rc.Width - width1 - (int) g.MeasureString("100.0%", font).Width;
        height1 = rc.Height * 7 / 10 - this.iHeight / 2;
        height2 = rc.Height * 3 / 10 - this.iHeight * 5 / 2;
        if (this.m_hqForm.IsMultiCommidity)
        {
          int num = height1 + height2;
          height1 = num * 3 / 4;
          height2 = num / 4;
        }
      }
      else
      {
        width1 = this.iWidth * 7 - 1;
        width2 = rc.Width - width1 - this.iWidth;
        height1 = rc.Height * 6 / 9 - this.iHeight / 2;
        height2 = rc.Height * 3 / 9 - this.iHeight / 2;
      }
      this.m_rcPrice = new Rectangle(rc.X + width1, rc.Y + this.iHeight, width2, height1);
      this.m_rcVolume = new Rectangle(rc.X + width1, this.m_rcPrice.Y + this.m_rcPrice.Height + this.iHeight, width2, height2);
      this.m_rcLabel = new Rectangle(rc.X + 1, rc.Y + this.iHeight * 2, width1, this.iHeight * 12);
      if (this.m_rcLabel.Y + this.m_rcLabel.Height > rc.Y + rc.Height)
        this.m_rcLabel.Height = rc.Y + rc.Height - this.m_rcLabel.Y;
      this.iPriceGridNum = this.m_rcPrice.Height / this.iHeight * 2 / 3;
      if (this.iPriceGridNum % 2 == 1)
        ++this.iPriceGridNum;
      if (this.iPriceGridNum <= 0)
        return;
      if (!this.bLarge)
        g.DrawLine(pen, rc.X, rc.Y, rc.X, rc.Y + rc.Height);
      for (int index = 0; index <= this.iPriceGridNum; ++index)
        g.DrawLine(pen, this.m_rcPrice.X, this.m_rcPrice.Y + this.m_rcPrice.Height * index / this.iPriceGridNum, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_rcPrice.Y + this.m_rcPrice.Height * index / this.iPriceGridNum);
      int num1 = this.m_rcVolume.Height / this.iHeight * 2 / 3;
      if (num1 <= 0)
        num1 = 1;
      for (int index = 0; index <= num1; ++index)
        g.DrawLine(pen, this.m_rcVolume.X, this.m_rcVolume.Y + this.m_rcVolume.Height * index / num1, this.m_rcVolume.X + this.m_rcVolume.Width, this.m_rcVolume.Y + this.m_rcVolume.Height * index / num1);
      if (this.TimeRange == null)
      {
        if (this.m_hqClient.TimeRange == null)
          return;
        this.TimeRange = this.m_hqClient.TimeRange;
      }
      this.m_iTotalMinNum = TradeTimeVO.GetTotalMinute(this.TimeRange);
      this.m_iMinLineNum = this.m_iTotalMinNum * (60 / this.m_hqClient.m_iMinLineInterval);
      int temp = 0;
      int num2 = 0;
      this.GetXFromTimeIndex(this.m_iTotalMinNum, this.m_iTotalMinNum - 1);
      int[] numArray = new int[this.TimeRange.Length + 1];
      numArray[0] = 0;
      for (int index = 0; index < this.TimeRange.Length; ++index)
        numArray[index + 1] = M_Common.GetTimeIndexFromTime(this.TimeRange[index].endDate, this.TimeRange[index].endTime, this.TimeRange) + 1;
      string text = "09:00";
      int index1 = 0;
      while (temp <= this.m_iTotalMinNum)
      {
        if (temp > 0 && this.m_iTotalMinNum - temp < 10)
          temp = this.m_iTotalMinNum;
        string str1 = "";
        bool flag1 = false;
        if (temp > numArray[index1])
          temp = numArray[index1];
        if (temp == numArray[index1] && index1 < numArray.Length - 1)
        {
          flag1 = true;
          if (index1 > 0 && index1 < this.TimeRange.Length)
          {
            string str2 = string.Concat((object) this.TimeRange[index1].beginTime);
            while (str2.Length < 4)
              str2 = "0" + str2;
            str1 = str2.Substring(0, 2) + ":" + str2.Substring(2, 2);
          }
          ++index1;
        }
        int xfromTimeIndex = this.GetXFromTimeIndex(temp > 0 ? temp - 1 : temp, temp);
        bool flag2 = false;
        if (flag1)
        {
          if (temp == 0)
            --xfromTimeIndex;
          else if (temp == this.m_iTotalMinNum)
            ++xfromTimeIndex;
          g.DrawLine(pen, xfromTimeIndex, this.m_rcPrice.Y, xfromTimeIndex, this.m_rcPrice.Y + this.m_rcPrice.Height);
          g.DrawLine(pen, xfromTimeIndex, this.m_rcVolume.Y, xfromTimeIndex, this.m_rcVolume.Y + this.m_rcVolume.Height);
          flag2 = true;
        }
        else
        {
          M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, xfromTimeIndex, this.m_rcPrice.Y, xfromTimeIndex, this.m_rcPrice.Y + this.m_rcPrice.Height);
          M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, xfromTimeIndex, this.m_rcVolume.Y, xfromTimeIndex, this.m_rcVolume.Y + this.m_rcVolume.Height);
          if (xfromTimeIndex - num2 >= (int) ((double) g.MeasureString(text, font).Width * 1.0))
            flag2 = true;
        }
        if (flag2 && this.bLarge)
        {
          solidBrush.Color = SetInfo.RHColor.clNumber;
          string str2 = Convert.ToString(temp != 0 ? (temp != this.m_iTotalMinNum ? M_Common.GetTimeFromTimeIndex(temp - 1, this.TimeRange) : this.TimeRange[this.TimeRange.Length - 1].endTime) : this.TimeRange[0].beginTime);
          while (str2.Length < 4)
            str2 = "0" + str2;
          string str3 = str2.Substring(0, 2) + ":" + str2.Substring(2, 2);
          if (str1.Length > 0 && !str3.Equals(str1))
            str3 = str3 + "/" + str1;
          int num3 = this.m_rcVolume.Y + this.m_rcVolume.Height;
          if (xfromTimeIndex - num2 < (int) ((double) g.MeasureString(str3, font).Width * 0.800000011920929))
            this.m_hqForm.ClearRect(g, (float) num2 - g.MeasureString(text, font).Width / 2f, (float) (this.m_rcVolume.Y + this.m_rcVolume.Height + 1), g.MeasureString(text, font).Width, (float) this.iHeight);
          g.DrawString(str3, font, (Brush) solidBrush, (float) xfromTimeIndex - g.MeasureString(str3, font).Width / 2f, (float) num3);
          num2 = xfromTimeIndex;
          text = str3;
        }
        if (temp < this.m_iTotalMinNum)
        {
          temp += 30;
          if (temp > this.m_iTotalMinNum)
            temp = this.m_iTotalMinNum;
        }
        else
          break;
      }
      if (this.m_product == null || this.m_product.realData == null)
        return;
      this.m_iPrecision = this.m_hqClient.GetPrecision(this.m_product.commodityInfo);
      float num4 = this.m_maxPrice - this.m_minPrice;
      float num5 = 1f;
      for (int index2 = 0; index2 < this.m_iPrecision; ++index2)
      {
        num4 *= 10f;
        num5 /= 10f;
      }
      int num6 = (int) ((double) num4 + 1.0 - (double) num5);
      if (num6 % this.iPriceGridNum > 0 || num6 == 0)
        num6 = (num6 / this.iPriceGridNum + 1) * this.iPriceGridNum;
      float num7 = (float) num6;
      for (int index2 = 0; index2 < this.m_iPrecision; ++index2)
        num7 /= 10f;
      this.m_maxPrice = this.m_product.realData.yesterBalancePrice + num7 / 2f;
      this.m_minPrice = this.m_product.realData.yesterBalancePrice - num7 / 2f;
      float num8 = this.m_product.realData.yesterBalancePrice;
      for (int index2 = 0; index2 <= this.iPriceGridNum; ++index2)
      {
        float num3 = this.m_maxPrice - (this.m_maxPrice - this.m_minPrice) * (float) index2 / (float) this.iPriceGridNum;
        if ((double) num3 > (double) num8)
          solidBrush.Color = SetInfo.RHColor.clIncrease;
        else if ((double) num8 > (double) num3)
        {
          solidBrush.Color = SetInfo.RHColor.clDecrease;
        }
        else
        {
          if ((double) this.m_maxPrice > (double) this.m_minPrice)
            g.DrawLine(pen, this.m_rcPrice.X, this.m_rcPrice.Y + this.m_rcPrice.Height * index2 / this.iPriceGridNum + 1, this.m_rcPrice.X + this.m_rcPrice.Width, this.m_rcPrice.Y + this.m_rcPrice.Height * index2 / this.iPriceGridNum + 1);
          solidBrush.Color = SetInfo.RHColor.clEqual;
        }
        string str1 = M_Common.FloatToString((double) num3, this.m_iPrecision);
        int num9 = this.m_rcPrice.X - (int) g.MeasureString(str1, font).Width - 1;
        int num10 = this.m_rcPrice.Y + this.m_rcPrice.Height * index2 / this.iPriceGridNum - (int) font.Size;
        g.DrawString(str1, font, (Brush) solidBrush, (float) num9, (float) num10);
        if (this.bLarge)
        {
          float num11 = 0.0 == (double) num8 ? 0.0f : (float) (((double) num3 - (double) num8) * 100.0) / num8;
          if ((double) num11 < 0.0)
            num11 = -num11;
          string str2 = M_Common.FloatToString((double) num11, 2);
          if ((double) num11 >= 100.0)
            str2 = str2.Substring(0, str2.Length - 1);
          string s = str2 + "%";
          int num12 = this.m_rcPrice.X + this.m_rcPrice.Width + 2;
          g.DrawString(s, font, (Brush) solidBrush, (float) num12, (float) num10);
        }
      }
      if (this.m_maxReserveCount == this.m_minReserveCount)
      {
        if (this.m_minReserveCount > 0)
        {
          this.m_maxReserveCount += num1 - 1;
          --this.m_minReserveCount;
        }
      }
      else
      {
        int num3 = (int) ((double) (this.m_maxReserveCount - this.m_minReserveCount) * 0.1);
        if (num3 <= 0)
          num3 = 1;
        this.m_maxReserveCount += num3;
        this.m_minReserveCount -= num3;
      }
      if (this.m_minReserveCount < 0)
        this.m_minReserveCount = 0;
      solidBrush.Color = SetInfo.RHColor.clVolume;
      long num13 = 0L;
      for (int index2 = 0; index2 < num1; ++index2)
      {
        long num3 = this.m_maxVolume - this.m_maxVolume * (long) index2 / (long) num1;
        if (num13 != num3)
        {
          num13 = num3;
          string str = Convert.ToString(num3);
          int num9 = this.m_rcVolume.X - (int) g.MeasureString(str, font).Width;
          int num10 = this.m_rcVolume.Y + this.m_rcVolume.Height * index2 / num1 - (int) font.Size;
          g.DrawString(str, font, (Brush) solidBrush, (float) num9, (float) num10);
        }
      }
      if (this.bLarge)
      {
        solidBrush.Color = SetInfo.RHColor.clReserve;
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          string s = Convert.ToString((long) (this.m_maxReserveCount - (this.m_maxReserveCount - this.m_minReserveCount) * index2 / num1));
          int num3 = this.m_rcVolume.X + this.m_rcVolume.Width + 1;
          int num9 = this.m_rcVolume.Y + this.m_rcVolume.Height * index2 / num1 - (int) font.Size;
          g.DrawString(s, font, (Brush) solidBrush, (float) num3, (float) num9);
        }
      }
      solidBrush.Dispose();
      pen.Dispose();
    }

    private int GetXFromTimeIndex(int index, int temp)
    {
      if (this.m_iTotalMinNum == 0)
        return 0;
      if (this.m_iTotalMinNum == 1)
      {
        if (index == temp)
          return this.m_rcPrice.X + 1;
        return this.m_rcPrice.X + 1 + this.m_rcPrice.Width - 2;
      }
      if (index >= this.m_iTotalMinNum)
        index = this.m_iTotalMinNum - 1;
      return this.m_rcPrice.X + 1 + index * (this.m_rcPrice.Width - 2) / (this.m_iTotalMinNum - 1);
    }

    private float GetPriceFromY(int y)
    {
      return (float) Math.Round((double) (int) ((double) (this.m_rcPrice.Height - (y - this.m_rcPrice.Y)) * ((double) this.m_maxPrice - (double) this.m_minPrice)) / (double) this.m_rcPrice.Height + 0.5, this.m_iPrecision) + this.m_minPrice;
    }

    private float GetReserveFromY(int y)
    {
      return (float) ((int) ((double) ((this.m_rcVolume.Height - (y - this.m_rcVolume.Y)) * (this.m_maxReserveCount - this.m_minReserveCount)) / (double) this.m_rcVolume.Height) + this.m_minReserveCount);
    }

    private int GetYFromPrice(float price)
    {
      if ((double) this.m_maxPrice == (double) this.m_minPrice)
        return this.m_rcPrice.Y + this.m_rcPrice.Height - 1;
      return this.m_rcPrice.Y + this.m_rcPrice.Height - (int) (((double) price - (double) this.m_minPrice) * (double) this.m_rcPrice.Height / ((double) this.m_maxPrice - (double) this.m_minPrice) + 0.5);
    }

    private void GetMaxMinPrice()
    {
      if (this.m_product.realData == null)
        return;
      this.m_maxPrice = this.m_product.realData == null || (double) this.m_product.realData.highPrice >= (double) this.m_product.realData.yesterBalancePrice ? this.m_product.realData.highPrice : this.m_product.realData.yesterBalancePrice;
      if ((double) this.m_maxPrice < 1.0 / 1000.0)
        return;
      this.m_minPrice = (double) this.m_product.realData.lowPrice <= (double) this.m_product.realData.yesterBalancePrice ? this.m_product.realData.lowPrice : this.m_product.realData.yesterBalancePrice;
      float num1 = this.m_hqClient.GetPrecision(this.m_product.commodityInfo) != 3 ? this.m_product.realData.yesterBalancePrice / 1000f : 0.0055f;
      if ((double) this.m_product.realData.highPrice == 0.0 && (double) this.m_product.realData.lowPrice == 0.0)
      {
        this.m_minPrice = 0.0f;
        this.m_maxPrice = 2f * this.m_product.realData.yesterBalancePrice;
      }
      else
      {
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = this.m_product.realData.yesterBalancePrice;
        if ((double) this.m_product.realData.highPrice - (double) num4 >= (double) num1)
        {
          this.m_maxPrice = this.m_product.realData.highPrice;
          num2 = this.m_product.realData.highPrice - num4;
        }
        else
          this.m_maxPrice = num4 + num1;
        if ((double) num4 - (double) this.m_product.realData.lowPrice >= (double) num1)
        {
          this.m_minPrice = this.m_product.realData.lowPrice;
          num3 = num4 - this.m_product.realData.lowPrice;
        }
        else
          this.m_minPrice = num4 - num1;
        if ((double) num2 > (double) num3)
          this.m_minPrice = num4 - num2;
        if ((double) num2 >= (double) num3)
          return;
        this.m_maxPrice = num4 + num3;
      }
    }

    internal bool MouseDoubleClick(int x, int y)
    {
      if (y < this.m_rcPrice.Y || y > this.m_rcVolume.Y + this.m_rcVolume.Height || (x < this.m_rcPrice.X || x > this.m_rcPrice.X + this.m_rcPrice.Width))
        return false;
      if (this.isDrawCursor)
      {
        if (this.m_iPos != -1)
        {
          this.DrawCursor(-1);
          this.m_iPos = -1;
          this.m_iPosY = -1;
          this.isDrawCursor = false;
        }
      }
      else
      {
        this.DrawCursor(this.GetMinLineIndexFromX(x), y);
        this.DrawLabel();
        this.isDrawCursor = true;
      }
      return !this.isDrawCursor;
    }

    public int GetMinLineIndexFromX(int X)
    {
      if (this.m_iMinLineNum == 0)
        return 0;
      return (X - this.m_rcPrice.X - 1) * (this.m_iMinLineNum - 1) / (this.m_rcPrice.Width - 2);
    }

    internal bool MouseDragged(int x, int y)
    {
      if (!this.isDrawCursor)
        return false;
      if (y < this.m_rcPrice.Y || y > this.m_rcVolume.Y + this.m_rcVolume.Height || (x < this.m_rcPrice.X || x > this.m_rcPrice.X + this.m_rcPrice.Width))
      {
        if (this.m_hqForm.M_Cursor == Cursors.Hand)
          this.m_hqForm.M_Cursor = Cursors.Default;
        this.DrawCursor(-1, -1);
        this.m_iPos = -1;
        this.m_iPosY = -1;
        return true;
      }
      this.DrawCursor(this.GetMinLineIndexFromX(x), y);
      this.DrawLabel();
      return false;
    }

    internal bool KeyPressed(KeyEventArgs e)
    {
      bool flag = false;
      switch (e.KeyData)
      {
        case Keys.Escape:
          if (this.m_iPos != -1)
          {
            this.DrawCursor(-1);
            this.m_iPos = -1;
            this.m_iPosY = -1;
            this.isDrawCursor = false;
            flag = true;
            break;
          }
          break;
        case Keys.Left:
          if (this.m_iPos > 0)
          {
            this.isDrawCursor = true;
            if (this.m_iPos > this.iNum)
            {
              this.DrawCursor(this.iNum - 1);
              this.DrawLabel();
              break;
            }
            this.DrawCursor(this.m_iPos - 1);
            this.DrawLabel();
            break;
          }
          if (this.m_iPos == -1 && this.iNum > 0)
          {
            this.isDrawCursor = true;
            this.DrawCursor(this.iNum - 1);
            this.DrawLabel();
            break;
          }
          break;
        case Keys.Right:
          if (this.m_iPos < this.m_iMinLineNum - 1 && this.m_iPos < this.iNum - 1)
          {
            this.isDrawCursor = true;
            this.DrawCursor(this.m_iPos + 1);
            this.DrawLabel();
            break;
          }
          break;
      }
      return flag;
    }

    public void Dispose()
    {
    }
  }
}
