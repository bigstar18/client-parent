// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.ButtonUtils
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using System;
using System.Collections;
using ToolsLibrary.util;
using TPME.Log;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
  public class ButtonUtils
  {
    public int selectTemp = -1;
    public string CurButtonName = string.Empty;
    public string CuRightrButtonName = string.Empty;
    public string InitialButtonName = string.Empty;
    public ArrayList ButtonList = new ArrayList();
    public ArrayList RightButtonList = new ArrayList();
    private PluginInfo pluginInfo;
    private SetInfo setInfo;
    public int isTidyBtnFlag;

    public ButtonUtils(PluginInfo _pluginInfo, SetInfo _setInfo)
    {
      this.pluginInfo = _pluginInfo;
      this.setInfo = _setInfo;
      this.InitialButtonName = this.setInfo.CurButtonName;
    }

    public void InsertButton(int index, MyButton value, ArrayList btnList)
    {
      try
      {
        if (value.Selected)
        {
          for (int index1 = 0; index1 < btnList.Count; ++index1)
          {
            MyButton myButton = (MyButton) btnList[index1];
            if (myButton.Selected)
              myButton.Selected = false;
          }
          if (value.Name.EndsWith("_Btn"))
            this.CuRightrButtonName = value.Name;
          else
            this.CurButtonName = value.Name;
        }
        btnList.Insert(index, (object) value);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "InsertButton异常：" + ex.Message);
      }
    }

    public void TidyButtons(ArrayList list)
    {
      try
      {
        MyButton myButton1 = (MyButton) null;
        foreach (MyButton myButton2 in list)
        {
          if (myButton2.Name.Equals("MyCommodity"))
          {
            myButton1 = myButton2;
            break;
          }
        }
        MyButton myButton3 = new MyButton("MyCommodity", this.pluginInfo.HQResourceManager.GetString("HQStr_MyCommodity"), false);
        int index;
        if (Tools.StrToBool(this.pluginInfo.HTConfig[(object) "MultiMarket"].ToString(), false))
        {
          if (this.ButtonList.Count > this.setInfo.ShowMarketBtnCount)
          {
            MyButton myButton2 = new MyButton("MoreMarket", "更多>>", false);
            MyButton myButton4 = new MyButton("AllMarket", "【所有市场】", false);
            this.InsertButton(this.setInfo.ShowMarketBtnCount, myButton2, list);
            this.InsertButton(list.Count, myButton4, list);
            index = this.setInfo.ShowMarketBtnCount - 1;
          }
          else
            index = list.Count - 1;
        }
        else if (this.ButtonList.Count > this.setInfo.ShowMarketBtnCount)
        {
          this.InsertButton(this.setInfo.ShowMarketBtnCount, new MyButton("MoreCommidity", "更多>>", false), list);
          index = this.setInfo.ShowMarketBtnCount - 1;
        }
        else
          index = list.Count - 1;
        if (!list.Contains((object) myButton1))
          return;
        list.Remove((object) myButton1);
        this.InsertButton(index, myButton3, list);
      }
      catch (Exception ex)
      {
        Logger.wirte(MsgType.Error, "TidyButtons异常：" + ex.Message);
      }
    }
  }
}
