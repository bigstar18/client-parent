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
		private HQForm m_hqForm;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private MultiQuoteData multiQuoteData;
		public Rectangle rc;
		private int gap = 4;
		private int buttonRepeat = 12;
		public string CurButtonName = string.Empty;
		public string CuRightrButtonName = string.Empty;
		public int ShowMarketBtnCount;
		private int selectedIndex = -1;
		private bool isAddMoreBtn;
		public int selectTemp = -1;
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
				if (btnList != null)
				{
					Font font = new Font("宋体", 10f, FontStyle.Regular);
					int count = btnList.Count;
					for (int i = 0; i < count; i++)
					{
						MyButton myButton = (MyButton)btnList[i];
						int num = (int)g.MeasureString(myButton.Text, font).Width + this.buttonRepeat;
						Point point = new Point(this.rc.X, this.rc.Y);
						Point point2 = new Point(this.rc.X, this.rc.Y + this.rc.Height);
						Point point3 = new Point(this.rc.X + num + this.buttonRepeat, this.rc.Y + this.rc.Height);
						Point point4 = new Point(this.rc.X + num + this.buttonRepeat, this.rc.Y);
						Point[] points = new Point[]
						{
							point,
							point2,
							point3,
							point4
						};
						myButton.Points = points;
						myButton.font = font;
						this.rc.X = this.rc.X + num + this.buttonRepeat;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "initButtonInfo异常：" + ex.Message);
			}
		}
		internal void Paint(Graphics g, ArrayList btnList, bool isBottomButton)
		{
			try
			{
				if (btnList != null)
				{
					this.initButtonInfo(g, btnList, isBottomButton);
					MyButton myButton = null;
					int num = btnList.Count;
					if (isBottomButton && num > this.ShowMarketBtnCount)
					{
						num = this.ShowMarketBtnCount + 1;
					}
					for (int i = 0; i < num; i++)
					{
						MyButton myButton2 = (MyButton)btnList[i];
						if (!myButton2.Selected)
						{
							this.PaintButton(g, myButton2);
						}
						else
						{
							if (myButton != null)
							{
								myButton.Selected = false;
								this.PaintButton(g, myButton);
							}
							this.selectedIndex = i;
							myButton = myButton2;
						}
					}
					if (myButton == null)
					{
						foreach (MyButton myButton3 in btnList)
						{
							if (myButton3.Selected)
							{
								myButton = (MyButton)btnList[this.ShowMarketBtnCount];
								myButton.Selected = true;
								goto IL_124;
							}
						}
						myButton = (MyButton)btnList[0];
						myButton.Selected = true;
						if (this.m_hqForm.MainGraph != null)
						{
							if (myButton.Name.EndsWith("_Btn"))
							{
								this.CuRightrButtonName = myButton.Name;
							}
							else
							{
								this.CurButtonName = myButton.Name;
							}
						}
					}
					IL_124:
					this.PaintButton(g, myButton);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Button-Paint异常：" + ex.Message);
			}
		}
		private void PaintButton(Graphics g, MyButton button)
		{
			try
			{
				SolidBrush solidBrush = new SolidBrush(SetInfo.RHColor.clItem);
				GraphicsPath graphicsPath = new GraphicsPath();
				if (button.Points != null)
				{
					graphicsPath.AddPolygon(button.Points);
					PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
					Image image;
					if (!button.Selected)
					{
						image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_hqback");
						pathGradientBrush.CenterColor = SetInfo.RHColor.clMultiQuote_TitleBack;
						pathGradientBrush.SurroundColors = new Color[]
						{
							SetInfo.RHColor.clMultiQuote_TitleBack,
							SetInfo.RHColor.clGrid,
							SetInfo.RHColor.clHighlight,
							SetInfo.RHColor.clMultiQuote_TitleBack
						};
					}
					else
					{
						image = (Image)this.pluginInfo.HQResourceManager.GetObject("HQImg_hqselectback");
						pathGradientBrush.CenterColor = SetInfo.RHColor.clGrid;
						pathGradientBrush.SurroundColors = new Color[]
						{
							SetInfo.RHColor.clGrid,
							SetInfo.RHColor.clHighlight,
							SetInfo.RHColor.clMultiQuote_TitleBack,
							SetInfo.RHColor.clGrid
						};
					}
					float width = (float)(button.Points[2].X - button.Points[0].X - 1);
					float height = (float)(button.Points[2].Y - button.Points[0].Y);
					g.DrawImage(image, (float)button.Points[0].X, (float)button.Points[0].Y, width, height);
					g.DrawString(button.Text, button.font, solidBrush, (float)(button.Points[0].X + this.buttonRepeat), (float)(button.Points[0].Y + this.gap));
					pathGradientBrush.Dispose();
					graphicsPath.Dispose();
					solidBrush.Dispose();
				}
			}
			catch (Exception ex)
			{
				WriteLog.WriteMsg("Page_Button-PaintButton异常：" + ex.Message);
			}
		}
		public MyButton MouseLeftClicked(int x, int y, ArrayList btnList, bool isBottomButton)
		{
			MyButton result;
			try
			{
				if (btnList == null || this.selectedIndex == -1)
				{
					result = null;
				}
				else
				{
					MyButton myButton = (MyButton)btnList[this.selectedIndex];
					if (isBottomButton)
					{
						int num = btnList.Count;
						if (this.ShowMarketBtnCount < btnList.Count)
						{
							num = this.ShowMarketBtnCount + 1;
						}
						for (int i = 0; i < num; i++)
						{
							MyButton myButton2 = (MyButton)btnList[i];
							if (!myButton2.Selected && x > myButton2.Points[0].X && x < myButton2.Points[3].X)
							{
								myButton.Selected = false;
								myButton2.Selected = true;
								this.multiQuoteData.iStart = 0;
								this.multiQuoteData.yChange = 0;
								result = myButton2;
								return result;
							}
						}
					}
					else
					{
						for (int j = 0; j < btnList.Count; j++)
						{
							MyButton myButton2 = (MyButton)btnList[j];
							if (!myButton2.Selected && x > myButton2.Points[0].X && x < myButton2.Points[3].X)
							{
								myButton.Selected = false;
								myButton2.Selected = true;
								result = myButton2;
								return result;
							}
						}
					}
					result = null;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Button-MouseLeftClicked异常：" + ex.Message);
				result = null;
			}
			return result;
		}
		public MyButton MouseRightClicked(int x, int y, ArrayList btnList)
		{
			MyButton result;
			try
			{
				if (btnList == null || this.selectedIndex == -1)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < btnList.Count; i++)
					{
						MyButton myButton = (MyButton)btnList[i];
						if (x > myButton.Points[0].X && x < myButton.Points[3].X)
						{
							this.selectTemp = i;
							result = myButton;
							return result;
						}
					}
					result = null;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Button-MouseRightClicked异常：" + ex.Message);
				result = null;
			}
			return result;
		}
		public void ResetSelButton(ArrayList btnList)
		{
			try
			{
				if (btnList != null)
				{
					MyButton myButton = (MyButton)btnList[this.selectTemp];
					MyButton myButton2 = (MyButton)btnList[this.selectedIndex];
					myButton2.Selected = false;
					myButton.Selected = true;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, "Page_Button-ResetSelButton异常：" + ex.Message);
			}
		}
	}
}
