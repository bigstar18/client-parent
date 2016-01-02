// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Draw_LastBill
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Draw_LastBill
  {
    internal static void Paint(Graphics g, Rectangle rc, ProductData product, HQClientMain hqClientMain)
    {
      if (rc.Height <= 0)
        return;
      SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clNumber);
      Pen pen = new Pen(SetInfo.RHColor.clGrid);
      Font font = new Font("宋体", 10f, FontStyle.Regular);
      try
      {
        int num1 = (int) g.MeasureString("tttttttttt", font).Width / 10;
        if (product == null || product.realData == null || product.aBill == null)
          return;
        int productType = hqClientMain.getProductType(product.commodityInfo);
        int precision = hqClientMain.GetPrecision(product.commodityInfo);
        int num2 = 0;
        int num3 = rc.Height / font.Height;
        int count = product.lastBill.Count;
        if (count <= 0)
          return;
        if (count < num3)
          num3 = count;
        int y = rc.Y - 2;
        ArrayList arrayList = new ArrayList();
        for (int index = count - num3; index < count; ++index)
        {
          int x = rc.X;
          if (index > product.lastBill.Count)
            break;
          BillDataVO billDataVo1 = (BillDataVO) null;
          if (index > 0)
          {
            billDataVo1 = (BillDataVO) product.lastBill[index - 1];
            if (billDataVo1 == null)
              break;
          }
          BillDataVO billDataVo2 = (BillDataVO) product.lastBill[index];
          if (billDataVo2 != null)
          {
            if (num2 != billDataVo2.time / 100)
              g.DrawLine(new Pen(SetInfo.RHColor.clGrid)
              {
                DashStyle = DashStyle.Dash
              }, new Point(x, y), new Point(x + rc.Width, y));
            string s1 = TradeTimeVO.HHMMSSIntToString(billDataVo2.time);
            num2 = billDataVo2.time / 100;
            if (s1.Length != 8)
              s1 = "0" + s1;
            int num4 = rc.X + 1;
            solidBrush.Color = SetInfo.RHColor.clNumber;
            g.DrawString(s1, font, (Brush) solidBrush, (float) num4, (float) y);
            string str1 = M_Common.FloatToString((double) billDataVo2.curPrice, precision);
            solidBrush.Color = (double) billDataVo2.curPrice <= (double) product.realData.yesterBalancePrice ? ((double) billDataVo2.curPrice >= (double) product.realData.yesterBalancePrice ? SetInfo.RHColor.clEqual : SetInfo.RHColor.clDecrease) : SetInfo.RHColor.clIncrease;
            int num5 = num4 + (num1 * 16 - (int) g.MeasureString(str1, font).Width);
            g.DrawString(str1, font, (Brush) solidBrush, (float) num5, (float) y);
            string str2 = billDataVo1 != null ? Convert.ToString((int) (billDataVo2.totalAmount - billDataVo1.totalAmount)) : Convert.ToString(billDataVo2.totalAmount);
            solidBrush.Color = SetInfo.RHColor.clVolume;
            int num6 = rc.X + rc.Width - num1 * 2 - (int) g.MeasureString(str2, font).Width;
            g.DrawString(str2, font, (Brush) solidBrush, (float) num6, (float) y);
            if (productType != 2 && productType != 3)
            {
              byte num7;
              if (billDataVo1 == null)
                num7 = (byte) 2;
              else if ((double) billDataVo1.buyPrice <= 1.0 / 1000.0)
                num7 = (byte) 1;
              else if ((double) billDataVo2.curPrice >= (double) billDataVo1.sellPrice)
                num7 = (byte) 0;
              else if ((double) billDataVo2.curPrice <= (double) billDataVo1.buyPrice)
              {
                num7 = (byte) 1;
              }
              else
              {
                int num8 = (int) (((double) billDataVo1.sellPrice - (double) billDataVo2.curPrice) * 1000.0);
                float num9 = (float) (int) (((double) billDataVo2.curPrice - (double) billDataVo1.buyPrice) * 1000.0);
                num7 = (double) num8 >= (double) num9 ? ((double) num8 <= (double) num9 ? (byte) 2 : (byte) 1) : (byte) 0;
              }
              string s2;
              if ((int) num7 == 0)
              {
                s2 = "↑";
                solidBrush.Color = SetInfo.RHColor.clIncrease;
              }
              else if ((int) num7 == 1)
              {
                s2 = "↓";
                solidBrush.Color = SetInfo.RHColor.clDecrease;
              }
              else
              {
                s2 = "–";
                solidBrush.Color = SetInfo.RHColor.clEqual;
              }
              int num10 = rc.X + rc.Width - num1 * 2;
              g.DrawString(s2, font, (Brush) solidBrush, (float) num10, (float) y);
            }
            y += font.Height;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Draw_LastBill-Paint异常：" + ex.Message);
      }
      finally
      {
        pen.Dispose();
        solidBrush.Dispose();
        font.Dispose();
      }
    }
  }
}
