using Gnnt.MEBS.HQModel;
using System;
using System.Collections;
using ToolsLibrary.util;
using TPME.Log;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class ButtonUtils
	{
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		public int selectTemp = -1;
		public string CurButtonName = string.Empty;
		public string CuRightrButtonName = string.Empty;
		public string InitialButtonName = string.Empty;
		public ArrayList ButtonList = new ArrayList();
		public ArrayList RightButtonList = new ArrayList();
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
					for (int i = 0; i < btnList.Count; i++)
					{
						MyButton myButton = (MyButton)btnList[i];
						if (myButton.Selected)
						{
							myButton.Selected = false;
						}
					}
					if (value.Name.EndsWith("_Btn"))
					{
						this.CuRightrButtonName = value.Name;
					}
					else
					{
						this.CurButtonName = value.Name;
					}
				}
				btnList.Insert(index, value);
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
				MyButton myButton = null;
				foreach (MyButton myButton2 in list)
				{
					if (myButton2.Name.Equals("MyCommodity"))
					{
						myButton = myButton2;
						break;
					}
				}
				MyButton value = new MyButton("MyCommodity", this.pluginInfo.HQResourceManager.GetString("HQStr_MyCommodity"), false);
				int index;
				if (Tools.StrToBool(this.pluginInfo.HTConfig["MultiMarket"].ToString(), false))
				{
					if (this.ButtonList.Count > this.setInfo.ShowMarketBtnCount)
					{
						MyButton value2 = new MyButton("MoreMarket", "更多>>", false);
						MyButton value3 = new MyButton("AllMarket", "【所有市场】", false);
						this.InsertButton(this.setInfo.ShowMarketBtnCount, value2, list);
						this.InsertButton(list.Count, value3, list);
						index = this.setInfo.ShowMarketBtnCount - 1;
					}
					else
					{
						index = list.Count - 1;
					}
				}
				else
				{
					if (this.ButtonList.Count > this.setInfo.ShowMarketBtnCount)
					{
						MyButton value4 = new MyButton("MoreCommidity", "更多>>", false);
						this.InsertButton(this.setInfo.ShowMarketBtnCount, value4, list);
						index = this.setInfo.ShowMarketBtnCount - 1;
					}
					else
					{
						index = list.Count - 1;
					}
				}
				if (list.Contains(myButton))
				{
					list.Remove(myButton);
					this.InsertButton(index, value, list);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "TidyButtons异常：" + ex.Message);
			}
		}
	}
}
