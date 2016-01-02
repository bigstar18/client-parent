// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Page_Button
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.OutInfo;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  internal class Page_Button
  {
    private int gap = 4;
    private int buttonRepeat = 12;
    public string CurButtonName = string.Empty;
    public string CuRightrButtonName = string.Empty;
    private int selectedIndex = -1;
    public int selectTemp = -1;
    private HQForm m_hqForm;
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    private MultiQuoteData multiQuoteData;
    public Rectangle rc;
    public int ShowMarketBtnCount;
    private bool isAddMoreBtn;

    public Page_Button(Rectangle _rc, HQForm hqForm, ButtonUtils buttonUtils)
    {
      this.rc = _rc;
      this.m_hqForm = hqForm;
      this.pluginInfo = this.m_hqForm.CurHQClient.pluginInfo;
      this.setInfo = this.m_hqForm.CurHQClient.setInfo;
      this.multiQuoteData = this.m_hqForm.CurHQClient.multiQuoteData;
      this.ShowMarketBtnCount = this.setInfo.ShowMarketBtnCount;
      this.CurButtonName = buttonUtils.CurButtonName;
      this.CuRightrButtonName = buttonUtils.CuRightrButtonName;
    }

    private void initButtonInfo(Graphics g, ArrayList btnList, bool isBottomButton)
    {
      try
      {
        if (btnList == null)
          return;
        Font font = new Font("宋体", 10f, FontStyle.Regular);
        int count = btnList.Count;
        for (int index = 0; index < count; ++index)
        {
          MyButton myButton = (MyButton) btnList[index];
          int num = (int) g.MeasureString(myButton.Text, font).Width + this.buttonRepeat;
          Point[] pointArray = new Point[4]
          {
            new Point(this.rc.X, this.rc.Y),
            new Point(this.rc.X, this.rc.Y + this.rc.Height),
            new Point(this.rc.X + num + this.buttonRepeat, this.rc.Y + this.rc.Height),
            new Point(this.rc.X + num + this.buttonRepeat, this.rc.Y)
          };
          myButton.Points = pointArray;
          myButton.font = font;
          this.rc.X = this.rc.X + num + this.buttonRepeat;
        }
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "initButtonInfo异常：" + ex.Message);
      }
    }

    internal void Paint(Graphics g, ArrayList btnList, bool isBottomButton)
    {
      try
      {
        if (btnList == null)
          return;
        this.initButtonInfo(g, btnList, isBottomButton);
        MyButton button1 = (MyButton) null;
        int num = btnList.Count;
        if (isBottomButton && num > this.ShowMarketBtnCount)
          num = this.ShowMarketBtnCount + 1;
        for (int index = 0; index < num; ++index)
        {
          MyButton button2 = (MyButton) btnList[index];
          if (!button2.Selected)
          {
            this.PaintButton(g, button2);
          }
          else
          {
            if (button1 != null)
            {
              button1.Selected = false;
              this.PaintButton(g, button1);
            }
            this.selectedIndex = index;
            button1 = button2;
          }
        }
        if (button1 == null)
        {
          foreach (MyButton myButton in btnList)
          {
            if (myButton.Selected)
            {
              button1 = (MyButton) btnList[this.ShowMarketBtnCount];
              button1.Selected = true;
              goto label_24;
            }
          }
          button1 = (MyButton) btnList[0];
          button1.Selected = true;
          if (this.m_hqForm.MainGraph != null)
          {
            if (button1.Name.EndsWith("_Btn"))
              this.CuRightrButtonName = button1.Name;
            else
              this.CurButtonName = button1.Name;
          }
        }
label_24:
        this.PaintButton(g, button1);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Button-Paint异常：" + ex.Message);
      }
    }

    private void PaintButton(Graphics g, MyButton button)
    {
      try
      {
        SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
        GraphicsPath path = new GraphicsPath();
        if (button.Points == null)
          return;
        path.AddPolygon(button.Points);
        PathGradientBrush pathGradientBrush = new PathGradientBrush(path);
        Image image;
        if (!button.Selected)
        {
          image = (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_hqback");
          pathGradientBrush.CenterColor = SetInfo.RHColor.clMultiQuote_TitleBack;
          pathGradientBrush.SurroundColors = new Color[4]
          {
            SetInfo.RHColor.clMultiQuote_TitleBack,
            SetInfo.RHColor.clGrid,
            SetInfo.RHColor.clHighlight,
            SetInfo.RHColor.clMultiQuote_TitleBack
          };
        }
        else
        {
          image = (Image) this.pluginInfo.HQResourceManager.GetObject("HQImg_hqselectback");
          pathGradientBrush.CenterColor = SetInfo.RHColor.clGrid;
          pathGradientBrush.SurroundColors = new Color[4]
          {
            SetInfo.RHColor.clGrid,
            SetInfo.RHColor.clHighlight,
            SetInfo.RHColor.clMultiQuote_TitleBack,
            SetInfo.RHColor.clGrid
          };
        }
        float width = (float) (button.Points[2].X - button.Points[0].X - 1);
        float height = (float) (button.Points[2].Y - button.Points[0].Y);
        g.DrawImage(image, (float) button.Points[0].X, (float) button.Points[0].Y, width, height);
        g.DrawString(button.Text, button.font, (Brush) solidBrush, (float) (button.Points[0].X + this.buttonRepeat), (float) (button.Points[0].Y + this.gap));
        pathGradientBrush.Dispose();
        path.Dispose();
        solidBrush.Dispose();
      }
      catch (Exception ex)
      {
        WriteLog.WriteMsg("Page_Button-PaintButton异常：" + ex.Message);
      }
    }

    public MyButton MouseLeftClicked(int x, int y, ArrayList btnList, bool isBottomButton)
    {
      try
      {
        if (btnList == null || this.selectedIndex == -1)
          return (MyButton) null;
        MyButton myButton1 = (MyButton) btnList[this.selectedIndex];
        if (isBottomButton)
        {
          int num = btnList.Count;
          if (this.ShowMarketBtnCount < btnList.Count)
            num = this.ShowMarketBtnCount + 1;
          for (int index = 0; index < num; ++index)
          {
            MyButton myButton2 = (MyButton) btnList[index];
            if (!myButton2.Selected && x > myButton2.Points[0].X && x < myButton2.Points[3].X)
            {
              myButton1.Selected = false;
              myButton2.Selected = true;
              this.multiQuoteData.iStart = 0;
              this.multiQuoteData.yChange = 0;
              return myButton2;
            }
          }
        }
        else
        {
          for (int index = 0; index < btnList.Count; ++index)
          {
            MyButton myButton2 = (MyButton) btnList[index];
            if (!myButton2.Selected && x > myButton2.Points[0].X && x < myButton2.Points[3].X)
            {
              myButton1.Selected = false;
              myButton2.Selected = true;
              return myButton2;
            }
          }
        }
        return (MyButton) null;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Button-MouseLeftClicked异常：" + ex.Message);
        return (MyButton) null;
      }
    }

    public MyButton MouseRightClicked(int x, int y, ArrayList btnList)
    {
      try
      {
        if (btnList == null || this.selectedIndex == -1)
          return (MyButton) null;
        for (int index = 0; index < btnList.Count; ++index)
        {
          MyButton myButton = (MyButton) btnList[index];
          if (x > myButton.Points[0].X && x < myButton.Points[3].X)
          {
            this.selectTemp = index;
            return myButton;
          }
        }
        return (MyButton) null;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Button-MouseRightClicked异常：" + ex.Message);
        return (MyButton) null;
      }
    }

    public void ResetSelButton(ArrayList btnList)
    {
      try
      {
        if (btnList == null)
          return;
        MyButton myButton = (MyButton) btnList[this.selectTemp];
        ((MyButton) btnList[this.selectedIndex]).Selected = false;
        myButton.Selected = true;
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "Page_Button-ResetSelButton异常：" + ex.Message);
      }
    }
  }
}
