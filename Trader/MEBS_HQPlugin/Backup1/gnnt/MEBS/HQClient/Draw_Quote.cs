// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Draw_Quote
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Draw_Quote
  {
    public int buttonHight = 25;
    private Rectangle codeRectangle = new Rectangle();
    public int fontGap = 2;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private ButtonUtils buttonUtils;
    public Rectangle rcRightButton;
    public Page_Button rightbuttonGraph;
    private Hashtable stockM_htItemInfo;
    private string[] m_strItems;

    public Draw_Quote(HQClientMain m_hqClient)
    {
      this.pluginInfo = m_hqClient.pluginInfo;
      this.setInfo = m_hqClient.setInfo;
      this.buttonUtils = m_hqClient.buttonUtils;
    }

    private void paintStockNumber(Graphics g, SolidBrush m_Brush, Rectangle rc, ProductData product, string m_strItem, string preStr, Font font, int iPrecision, bool isLeft, int y)
    {
      string str = string.Empty;
      int num1 = (int) font.Size;
      int height = font.Height;
      if ("Newly".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        if (product.realData != null && (double) product.realData.curPrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.curPrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("ChangeValue".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        if (product.realData != null && (double) product.realData.curPrice > 0.0 && (double) product.realData.yesterBalancePrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.curPrice - (double) product.realData.yesterBalancePrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("Open".Equals(m_strItem))
      {
        if (product.realData != null && (double) product.realData.openPrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.openPrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.openPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("ChangeRate".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        if (product.realData != null && (double) product.realData.curPrice > 0.0 && (double) product.realData.yesterBalancePrice > 0.0)
        {
          str = M_Common.FloatToString(((double) product.realData.curPrice - (double) product.realData.yesterBalancePrice) / (double) product.realData.yesterBalancePrice * 100.0, 2) + "%";
          m_Brush.Color = this.GetPriceColor(product.realData.curPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("High".Equals(m_strItem))
      {
        if (product.realData != null && (double) product.realData.highPrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.highPrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.highPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("CurVol".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        str = product.realData == null || product.realData.curAmount <= 0 ? "—" : Convert.ToString(product.realData.curAmount);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      else if ("Low".Equals(m_strItem))
      {
        if (product.realData != null && (double) product.realData.lowPrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.lowPrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.lowPrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("TotalVolume".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        str = product.realData == null || product.realData.totalAmount <= 0L ? "—" : Convert.ToString((int) product.realData.totalAmount);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      else if ("VolRate".Equals(m_strItem))
      {
        str = product.realData == null || (double) product.realData.amountRate <= 0.0 ? "—" : M_Common.FloatToString((double) product.realData.amountRate, 2);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      else if ("Balance".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        if (product.realData != null && (double) product.realData.balancePrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.balancePrice, iPrecision);
          m_Brush.Color = this.GetPriceColor(product.realData.balancePrice, product.realData.yesterBalancePrice);
        }
        else
        {
          str = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("PreBalance".Equals(m_strItem))
      {
        str = product.realData == null || (double) product.realData.yesterBalancePrice <= 0.0 ? "—" : M_Common.FloatToString((double) product.realData.yesterBalancePrice, iPrecision);
        m_Brush.Color = SetInfo.RHColor.clEqual;
      }
      else if ("Order".Equals(m_strItem))
      {
        if (y + height > rc.Y + rc.Height)
          return;
        str = product.realData == null || product.realData.reserveCount <= 0 ? "—" : Convert.ToString(product.realData.reserveCount);
        m_Brush.Color = SetInfo.RHColor.clReserve;
      }
      else if ("OrderChange".Equals(m_strItem))
      {
        str = product.realData == null ? "—" : Convert.ToString(product.realData.reserveChange);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      else if ("cje".Equals(m_strItem))
      {
        if (product.realData != null && product.realData.totalMoney > 0.0)
        {
          str = M_Common.FloatToString(product.realData.totalMoney, iPrecision);
          m_Brush.Color = SetInfo.RHColor.clVolume;
        }
        else
        {
          str = "0";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("hsl".Equals(m_strItem))
      {
        if (product.realData != null && product.realData.totalAmount > 0L && product.realData.reserveCount > 0)
        {
          str = M_Common.FloatToString((double) product.realData.totalAmount / (double) product.realData.reserveCount * 100.0, 2) + "%";
          m_Brush.Color = SetInfo.RHColor.clVolume;
        }
        else
        {
          str = "0";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("zs".Equals(m_strItem))
      {
        if (product.realData != null && (double) product.realData.closePrice > 0.0)
        {
          str = M_Common.FloatToString((double) product.realData.yesterBalancePrice, iPrecision);
          m_Brush.Color = SetInfo.RHColor.clNumber;
        }
        else
        {
          str = "0";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
      }
      else if ("AskVolume".Equals(m_strItem))
      {
        str = product == null || product.realData == null || product.realData.outAmount <= 0 ? "—" : Convert.ToString(product.realData.outAmount);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      else if ("BidVolume".Equals(m_strItem))
      {
        str = product == null || product.realData == null || product.realData.inAmount <= 0 ? "—" : Convert.ToString(product.realData.inAmount);
        m_Brush.Color = SetInfo.RHColor.clVolume;
      }
      for (float width = g.MeasureString(preStr, font).Width; (double) g.MeasureString(str, font).Width > (double) (rc.Width / 2) - (double) width; font = new Font("宋体", (float) num1, FontStyle.Regular))
        --num1;
      int num2 = !isLeft ? rc.X + rc.Width - (int) g.MeasureString(str, font).Width : rc.X + rc.Width / 2 - (int) g.MeasureString(str, font).Width;
      g.DrawString(str, font, (Brush) m_Brush, (float) num2, (float) y);
    }

    internal void Paint(Graphics g, Rectangle rc, ProductData product, CommodityInfo commodityInfo, int iShowBuySellNum, HQClientMain hqClientMain)
    {
      HQForm hqForm = hqClientMain.m_hqForm;
      this.stockM_htItemInfo = hqClientMain.stockM_htItemInfo;
      this.m_strItems = hqClientMain.stockM_strItems;
      string str1 = hqClientMain.ABVOLS;
      Font font1 = new Font("楷体_GB2312", 20f, FontStyle.Bold);
      SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clProductName);
      Pen pen = new Pen(SetInfo.RHColor.clGrid);
      this.rightbuttonGraph = new Page_Button(this.rcRightButton, hqForm, this.buttonUtils);
      try
      {
        int num1 = (int) font1.Size;
        int height1 = font1.Height;
        hqClientMain.getProductType(product.commodityInfo);
        int precision = hqClientMain.GetPrecision(product.commodityInfo);
        int x = rc.X;
        int y1 = rc.Y;
        CodeTable codeTable = (CodeTable) null;
        if (hqClientMain.m_htProduct != null && hqClientMain.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)] != null)
          codeTable = (CodeTable) hqClientMain.m_htProduct[(object) (commodityInfo.marketID + commodityInfo.commodityCode)];
        string s1 = codeTable == null ? "————" : codeTable.sName;
        string str2 = commodityInfo.commodityCode;
        if (s1.Equals(str2))
          str2 = "";
        Font font2 = font1;
        for (; (double) g.MeasureString(s1 + str2, font1).Width > (double) (rc.Width - 20); font1 = new Font("楷体_GB2312", (float) num1, FontStyle.Bold))
          --num1;
        int height2 = font2.Height;
        this.codeRectangle.X = x;
        this.codeRectangle.Y = y1;
        this.codeRectangle.Width = rc.Width;
        this.codeRectangle.Height = height2;
        if (y1 + height2 < rc.Y + rc.Height)
          g.DrawString(s1, font1, (Brush) m_Brush, (float) (x + 10), (float) y1);
        Point[] points1 = new Point[3]
        {
          new Point(this.codeRectangle.X, this.codeRectangle.Y + this.codeRectangle.Height / 2),
          new Point(this.codeRectangle.X + 8, this.codeRectangle.Y + 3),
          new Point(this.codeRectangle.X + 8, this.codeRectangle.Y + this.codeRectangle.Height - 3)
        };
        int num2 = this.codeRectangle.X + this.codeRectangle.Width - 10;
        Point[] points2 = new Point[3]
        {
          new Point(num2 + 2, this.codeRectangle.Y + 3),
          new Point(num2 + 2, this.codeRectangle.Y + this.codeRectangle.Height - 3),
          new Point(num2 + 10, this.codeRectangle.Y + this.codeRectangle.Height / 2)
        };
        g.FillPolygon(Brushes.White, points1);
        g.FillPolygon(Brushes.White, points2);
        m_Brush.Color = SetInfo.RHColor.clItem;
        string str3 = str2;
        int num3 = rc.X + rc.Width - (int) g.MeasureString(str3, font1).Width - 10;
        if (y1 + height2 < rc.Y + rc.Height)
          g.DrawString(str3, font1, (Brush) m_Brush, (float) num3, (float) y1);
        int num4 = y1 + height2;
        font1 = font2;
        pen.Color = SetInfo.RHColor.clGrid;
        if (height2 < rc.Height)
          g.DrawRectangle(pen, rc.X, rc.Y + height2, rc.Width - 1, rc.Height - height2);
        font1 = new Font("宋体", 12f, FontStyle.Regular);
        int num5 = (int) font1.Size;
        int height3 = font1.Height;
        int num6 = rc.X + 1;
        int num7 = num4;
        int num8 = num4 + this.fontGap;
        if (num8 + height3 < rc.Y + rc.Height - this.buttonHight + 5)
          g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Sell"), font1, (Brush) m_Brush, (float) num6, (float) num8);
        int num9 = num8 + height3;
        g.DrawLine(pen, rc.X, num9, rc.X + rc.Width, num9);
        int num10 = num9 + this.fontGap;
        if (num10 + height3 < rc.Y + rc.Height - this.buttonHight + 5)
          g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Buy"), font1, (Brush) m_Brush, (float) num6, (float) num10);
        int num11 = num10 + height3;
        g.DrawLine(pen, rc.X, num11, rc.X + rc.Width, num11);
        int num12 = num11 + this.fontGap;
        if (num12 + height3 < rc.Y + rc.Height)
        {
          m_Brush.Color = SetInfo.RHColor.clItem;
          g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), font1, (Brush) m_Brush, (float) num6, (float) num12);
          g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignDiff"), font1, (Brush) m_Brush, (float) (num6 + rc.Width / 2), (float) num12);
          num12 += height3;
          g.DrawLine(pen, rc.X, num12, rc.X + rc.Width, num12);
        }
        string str4 = string.Empty;
        for (int index = 0; index < this.m_strItems.Length - 1; ++index)
        {
          if (index % 2 == 0)
          {
            if (index == 0)
              num12 += this.fontGap;
            else
              num12 += height3;
          }
          if (num12 + height3 < rc.Y + rc.Height)
          {
            if (this.stockM_htItemInfo[(object) this.m_strItems[index]] == null)
            {
              string s2 = "";
              g.DrawString(s2, font1, (Brush) m_Brush, (float) (num6 + rc.Width / 2), (float) num12);
            }
            else
            {
              m_Brush.Color = SetInfo.RHColor.clItem;
              string str5 = this.stockM_htItemInfo[(object) this.m_strItems[index]].ToString();
              if (index % 2 == 0)
              {
                g.DrawString(str5, font1, (Brush) m_Brush, (float) num6, (float) num12);
                this.paintStockNumber(g, m_Brush, rc, product, this.m_strItems[index], str5, font1, precision, true, num12);
              }
              else
              {
                g.DrawString(str5, font1, (Brush) m_Brush, (float) (num6 + rc.Width / 2), (float) num12);
                this.paintStockNumber(g, m_Brush, rc, product, this.m_strItems[index], str5, font1, precision, false, num12);
              }
            }
          }
        }
        m_Brush.Color = SetInfo.RHColor.clItem;
        int num13 = num12 + height3;
        if (num13 < rc.Y + rc.Height)
          g.DrawLine(pen, rc.X, num13, rc.X + rc.Width, num13);
        if (str1 != null && str1.Length > 0)
        {
          int num14 = num13 + this.fontGap;
          if (num14 + height3 < rc.Y + rc.Height + 1)
          {
            g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_AskVolume"), font1, (Brush) m_Brush, (float) num6, (float) num14);
            g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_BidVolume"), font1, (Brush) m_Brush, (float) (num6 + rc.Width / 2), (float) num14);
          }
          num13 = num14 + height3;
          if (num13 < rc.Y + rc.Height)
            g.DrawLine(pen, rc.X, num13, rc.X + rc.Width, num13);
        }
        g.DrawLine(pen, rc.X, rc.Y + rc.Height - 1, rc.X + rc.Width, rc.Y + rc.Height - 1);
        if (this.buttonUtils.CuRightrButtonName == "P_Btn")
        {
          int num14 = num13 + this.fontGap;
          for (int index = iShowBuySellNum - 1; index >= 0; --index)
          {
            if (num14 + height3 < rc.Y + rc.Height - this.buttonHight + 5)
              g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Sell") + this.pluginInfo.HQResourceManager.GetString("HQStr_Num" + (object) (index + 1)), font1, (Brush) m_Brush, (float) num6, (float) num14);
            num14 += height3;
          }
          if (num14 < rc.Y + rc.Height - this.buttonHight)
            g.DrawLine(pen, rc.X, num14, rc.X + rc.Width, num14);
          int num15 = num14 + this.fontGap;
          for (int index = 0; index < iShowBuySellNum; ++index)
          {
            if (num15 + height3 < rc.Y + rc.Height - this.buttonHight + 5)
              g.DrawString(this.pluginInfo.HQResourceManager.GetString("HQStr_Buy") + this.pluginInfo.HQResourceManager.GetString("HQStr_Num" + (object) (index + 1)), font1, (Brush) m_Brush, (float) num6, (float) num15);
            num15 += height3;
          }
          if (num15 < rc.Y + rc.Height - this.buttonHight)
            g.DrawLine(pen, rc.X, num15, rc.X + rc.Width, num15);
        }
        if (product == null)
          return;
        int num16 = num7;
        if (num16 + height3 > rc.Y + rc.Height)
          return;
        float[] numArray1 = new float[5];
        float[] numArray2 = new float[5];
        float[] numArray3 = new float[5];
        float[] numArray4 = new float[5];
        string str6;
        string str7;
        if (product != null)
        {
          if (product.realData != null)
          {
            try
            {
              numArray1[0] = product.realData.buyPrice[0];
              numArray1[1] = product.realData.buyPrice[1];
              numArray1[2] = product.realData.buyPrice[2];
              numArray1[3] = product.realData.buyPrice[3];
              numArray1[4] = product.realData.buyPrice[4];
              numArray2[0] = (float) product.realData.buyAmount[0];
              numArray2[1] = (float) product.realData.buyAmount[1];
              numArray2[2] = (float) product.realData.buyAmount[2];
              numArray2[3] = (float) product.realData.buyAmount[3];
              numArray2[4] = (float) product.realData.buyAmount[4];
              numArray3[0] = product.realData.sellPrice[0];
              numArray3[1] = product.realData.sellPrice[1];
              numArray3[2] = product.realData.sellPrice[2];
              numArray3[3] = product.realData.sellPrice[3];
              numArray3[4] = product.realData.sellPrice[4];
              numArray4[0] = (float) product.realData.sellAmount[0];
              numArray4[1] = (float) product.realData.sellAmount[1];
              numArray4[2] = (float) product.realData.sellAmount[2];
              numArray4[3] = (float) product.realData.sellAmount[3];
              numArray4[4] = (float) product.realData.sellAmount[4];
            }
            catch (Exception ex)
            {
              Logger.wirte(MsgType.Error, ex.ToString());
            }
            float num14 = 0.0f;
            float num15 = 0.0f;
            for (int index = 0; index < iShowBuySellNum; ++index)
            {
              num14 += numArray4[index];
              num15 += numArray2[index];
            }
            float num17 = num15 - num14;
            str6 = (double) num15 + (double) num14 >= 0.001 ? M_Common.FloatToString((double) num17 / ((double) num15 + (double) num14) * 100.0, 2) + "%" : "—";
            if ((double) num17 > 0.0)
            {
              str6 = "+" + str6;
              str7 = Convert.ToString((int) num17);
              m_Brush.Color = SetInfo.RHColor.clIncrease;
              goto label_71;
            }
            else if ((double) num17 < 0.0)
            {
              str7 = Convert.ToString(-(int) num17);
              m_Brush.Color = SetInfo.RHColor.clDecrease;
              goto label_71;
            }
            else
            {
              str7 = "0";
              m_Brush.Color = SetInfo.RHColor.clEqual;
              goto label_71;
            }
          }
        }
        str6 = "—";
        str7 = "—";
        m_Brush.Color = SetInfo.RHColor.clEqual;
label_71:
        string str8;
        if (product != null && (double) numArray3[0] > 0.0)
        {
          m_Brush.Color = this.GetPriceColor(numArray3[0], product.realData.yesterBalancePrice);
          str8 = M_Common.FloatToString((double) numArray3[0], precision);
        }
        else
        {
          str8 = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
        int num18 = rc.X + rc.Width / 2 - (int) g.MeasureString(str8, font1).Width;
        g.DrawString(str8, font1, (Brush) m_Brush, (float) num18, (float) num16);
        string str9 = product == null || (double) numArray4[0] <= 0.0 ? "—" : Convert.ToString((int) numArray4[0]);
        m_Brush.Color = SetInfo.RHColor.clVolume;
        int num19 = rc.X + rc.Width - (int) g.MeasureString(str9, font1).Width;
        g.DrawString(str9, font1, (Brush) m_Brush, (float) num19, (float) num16);
        int num20 = num16 + this.fontGap + height3;
        string str10;
        if (product != null && (double) numArray1[0] > 0.0)
        {
          m_Brush.Color = this.GetPriceColor(numArray1[0], product.realData.yesterBalancePrice);
          str10 = M_Common.FloatToString((double) numArray1[0], precision);
        }
        else
        {
          str10 = "—";
          m_Brush.Color = SetInfo.RHColor.clEqual;
        }
        int num21 = rc.X + rc.Width / 2 - (int) g.MeasureString(str10, font1).Width;
        g.DrawString(str10, font1, (Brush) m_Brush, (float) num21, (float) num20);
        string str11 = product == null || (double) numArray2[0] <= 0.0 ? "—" : Convert.ToString((int) numArray2[0]);
        m_Brush.Color = SetInfo.RHColor.clVolume;
        int num22 = rc.X + rc.Width - (int) g.MeasureString(str11, font1).Width;
        g.DrawString(str11, font1, (Brush) m_Brush, (float) num22, (float) num20);
        int num23 = num20 + this.fontGap + this.fontGap + height3;
        int num24 = rc.X + rc.Width / 2 - (int) g.MeasureString(str6, font1).Width;
        int num25 = (int) g.MeasureString(this.pluginInfo.HQResourceManager.GetString("HQStr_ConsignRate"), font1).Width;
        if (rc.X + num25 > rc.X + rc.Width / 2 - (int) g.MeasureString(str6, font1).Width)
          num24 = rc.X + num25;
        g.DrawString(str6, font1, (Brush) m_Brush, (float) num24, (float) num23);
        int num26 = rc.X + rc.Width - (int) g.MeasureString(str7, font1).Width;
        g.DrawString(str7, font1, (Brush) m_Brush, (float) num26, (float) num23);
        int num27 = num23 + this.fontGap;
        int num28 = this.m_strItems.Length % 2 != 0 ? (this.m_strItems.Length - 1) / 2 : this.m_strItems.Length / 2;
        int num29 = num27 + (height3 * num28 + this.fontGap);
        if (str1 != null && str1.Length > 0)
        {
          num29 = num29 + this.fontGap + height3;
          if (num29 + height3 > rc.Y + rc.Height)
            return;
          string str5 = product == null || product.realData == null || product.realData.outAmount <= 0 ? "—" : Convert.ToString(product.realData.outAmount);
          m_Brush.Color = SetInfo.RHColor.clVolume;
          int num14 = rc.X + rc.Width / 2 - (int) g.MeasureString(str5, font1).Width;
          g.DrawString(str5, font1, (Brush) m_Brush, (float) num14, (float) num29);
          string str12 = product == null || product.realData == null || product.realData.inAmount <= 0 ? "—" : Convert.ToString(product.realData.inAmount);
          m_Brush.Color = SetInfo.RHColor.clVolume;
          int num15 = rc.X + rc.Width - (int) g.MeasureString(str12, font1).Width;
          g.DrawString(str12, font1, (Brush) m_Brush, (float) num15, (float) num29);
        }
        if (this.buttonUtils.CuRightrButtonName == "P_Btn")
        {
          for (int index = iShowBuySellNum - 1; index >= 0; --index)
          {
            num29 += height3;
            if (num29 + height3 <= rc.Y + rc.Height - this.buttonHight + 4)
            {
              string str5;
              if (product != null && (double) numArray3[index] > 0.0)
              {
                m_Brush.Color = this.GetPriceColor(numArray3[index], product.realData.yesterBalancePrice);
                str5 = M_Common.FloatToString((double) numArray3[index], precision);
              }
              else
              {
                str5 = "—";
                m_Brush.Color = SetInfo.RHColor.clEqual;
              }
              int num14 = rc.X + rc.Width / 2 - (int) g.MeasureString(str5, font1).Width;
              g.DrawString(str5, font1, (Brush) m_Brush, (float) num14, (float) num29);
              string str12 = product == null || (double) numArray4[index] <= 0.0 ? "—" : Convert.ToString((int) numArray4[index]);
              m_Brush.Color = SetInfo.RHColor.clVolume;
              int num15 = rc.X + rc.Width - (int) g.MeasureString(str12, font1).Width;
              g.DrawString(str12, font1, (Brush) m_Brush, (float) num15, (float) num29);
            }
            else
              goto label_102;
          }
          num29 += this.fontGap;
          for (int index = 0; index < iShowBuySellNum; ++index)
          {
            num29 += height3;
            if (num29 + height3 <= rc.Y + rc.Height - this.buttonHight + 4)
            {
              string str5;
              if (product != null && (double) numArray1[index] > 0.0)
              {
                m_Brush.Color = this.GetPriceColor(numArray1[index], product.realData.yesterBalancePrice);
                str5 = M_Common.FloatToString((double) numArray1[index], precision);
              }
              else
              {
                str5 = "—";
                m_Brush.Color = SetInfo.RHColor.clEqual;
              }
              int num14 = rc.X + rc.Width / 2 - (int) g.MeasureString(str5, font1).Width;
              g.DrawString(str5, font1, (Brush) m_Brush, (float) num14, (float) num29);
              string str12 = product == null || (double) numArray2[index] <= 0.0 ? "—" : Convert.ToString((int) numArray2[index]);
              m_Brush.Color = SetInfo.RHColor.clVolume;
              int num15 = rc.X + rc.Width - (int) g.MeasureString(str12, font1).Width;
              g.DrawString(str12, font1, (Brush) m_Brush, (float) num15, (float) num29);
            }
            else
              goto label_102;
          }
        }
        int y2 = num29 + height3;
        ProductData productData = hqForm.CurHQClient.GetProductData(hqForm.CurHQClient.curCommodityInfo);
        if (this.buttonUtils.CuRightrButtonName == "X_Btn")
        {
          Rectangle rc1 = new Rectangle(rc.X, y2, rc.Width, rc.Height - y2 - this.buttonHight);
          Draw_LastBill.Paint(g, rc1, productData, hqForm.CurHQClient);
        }
        if (this.buttonUtils.CuRightrButtonName == "T_Btn")
        {
          Draw_MinLine drawMinLine = new Draw_MinLine(hqForm, false);
          Rectangle rc1 = new Rectangle(rc.X, y2, rc.Width, rc.Height - y2 - 2 * this.buttonHight);
          drawMinLine.Paint(g, rc1, productData);
        }
label_102:
        g.DrawLine(pen, rc.X, rc.Y + rc.Height - this.buttonHight - 1, rc.X + rc.Width, rc.Y + rc.Height - this.buttonHight - 1);
        this.rcRightButton = rc;
        this.rcRightButton.X = rc.X;
        this.rcRightButton.Y = rc.Y + rc.Height - this.buttonHight;
        this.rcRightButton.Height = this.buttonHight;
        this.rightbuttonGraph.rc = this.rcRightButton;
        if (this.buttonUtils.RightButtonList == null || this.buttonUtils.RightButtonList.Count <= 0)
          return;
        this.rightbuttonGraph.Paint(g, this.buttonUtils.RightButtonList, false);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Draw_Quote-Paint异常：" + ex.Message);
      }
      finally
      {
        pen.Dispose();
        m_Brush.Dispose();
        font1.Dispose();
      }
    }

    public Color GetPriceColor(float fPrice, float fBenchMark)
    {
      if ((double) fPrice > (double) fBenchMark)
        return SetInfo.RHColor.clIncrease;
      if ((double) fPrice < (double) fBenchMark)
        return SetInfo.RHColor.clDecrease;
      return SetInfo.RHColor.clEqual;
    }

    public void MouseMove(int x, int y, HQForm m_hqForm)
    {
      if (this.codeRectangle.Contains(x, y))
        m_hqForm.M_Cursor = Cursors.Hand;
      else
        m_hqForm.M_Cursor = Cursors.Default;
    }

    public void MouseLeftClick(int x, int y, HQForm m_hqForm, HQClientMain m_hqClient)
    {
      if (!this.codeRectangle.Contains(x, y))
        return;
      if (x < this.codeRectangle.X + 8)
      {
        m_hqForm.ChangeStock(true);
        m_hqForm.Repaint();
      }
      else
      {
        if (x <= this.codeRectangle.X + this.codeRectangle.Width - 8)
          return;
        m_hqForm.ChangeStock(false);
        m_hqForm.Repaint();
      }
    }

    public void MouseDoubleClick(int x, int y, ProductData stock, HQForm m_hqForm, HQClientMain m_hqClient)
    {
      if (!this.codeRectangle.Contains(x, y) || !Tools.StrToBool((string) this.pluginInfo.HTConfig[(object) "QuoteMouseDoubleClick"], true) || (x <= this.codeRectangle.X + 10 || x >= this.codeRectangle.X + this.codeRectangle.Width - 10) || stock.realData == null)
        return;
      InterFace.CommodityInfoEventArgs e = new InterFace.CommodityInfoEventArgs((ProductDataVO) stock.realData.Clone());
      m_hqForm.MultiQuoteMouseLeftClick((object) this, e);
    }
  }
}
