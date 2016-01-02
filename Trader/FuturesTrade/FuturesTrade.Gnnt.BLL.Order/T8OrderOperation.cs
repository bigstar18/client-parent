using FuturesTrade.Gnnt.Library;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class T8OrderOperation
	{
		private Font font;
		private Brush PicClickBrush = Brushes.Black;
		private bool isRunOnceDraw = true;
		public void ChangeComboForColor(ComboBox comboBoxBuyOrSall)
		{
			switch (comboBoxBuyOrSall.SelectedIndex)
			{
			case 0:
				comboBoxBuyOrSall.ForeColor = Color.Red;
				break;
			case 1:
				comboBoxBuyOrSall.ForeColor = Color.Green;
				break;
			}
			if (comboBoxBuyOrSall.Focused)
			{
				comboBoxBuyOrSall.Select(0, 0);
			}
		}
		public void ComboDrawItem(ComboBox comboBoxBuyOrSall, DrawItemEventArgs e)
		{
			Pen pen = new Pen(Color.Black);
			Pen pen2 = new Pen(Color.White);
			switch (e.Index)
			{
			case 0:
				pen = new Pen(Color.Red);
				break;
			case 1:
				pen = new Pen(Color.Green);
				break;
			}
			if (e.State == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(pen.Brush, e.Bounds);
				e.Graphics.DrawString((string)comboBoxBuyOrSall.Items[e.Index], comboBoxBuyOrSall.Font, pen2.Brush, e.Bounds);
				return;
			}
			e.Graphics.FillRectangle(pen2.Brush, e.Bounds);
			e.Graphics.DrawString((string)comboBoxBuyOrSall.Items[e.Index], comboBoxBuyOrSall.Font, pen.Brush, e.Bounds);
		}
		public void KeyTip(int comboNum, int startKeyNum)
		{
			string @string = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Press");
			string string2 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_meaning");
			string message = string.Empty;
			switch (comboNum)
			{
			case 1:
			{
				string string3 = Global.M_ResourceManager.GetString("Global_BuySellStrArr2");
				string string4 = Global.M_ResourceManager.GetString("Global_BuySellStrArr1");
				message = string.Concat(new object[]
				{
					@string,
					startKeyNum,
					string2,
					string4,
					@string,
					startKeyNum + 1,
					string2,
					string3
				});
				break;
			}
			case 2:
			{
				string string5 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1");
				string string6 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2");
				string string7 = Global.M_ResourceManager.GetString("TradeStr_TransferToday");
				string string8 = Global.M_ResourceManager.GetString("Global_CloseModeStrArr2");
				message = string.Concat(new object[]
				{
					@string,
					startKeyNum,
					string2,
					string5,
					@string,
					startKeyNum + 1,
					string2,
					string6,
					@string,
					startKeyNum + 2,
					string2,
					string7,
					@string,
					startKeyNum + 3,
					string2,
					string8
				});
				break;
			}
			case 3:
			{
				string string9 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Bid");
				string string10 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_AskPrice");
				string string11 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_last");
				message = string.Concat(new object[]
				{
					@string,
					startKeyNum,
					string2,
					string9,
					@string,
					startKeyNum + 1,
					string2,
					string10,
					@string,
					startKeyNum + 2,
					string2,
					string11
				});
				break;
			}
			case 4:
				message = string.Concat(new object[]
				{
					@string,
					startKeyNum,
					string2,
					"<",
					@string,
					startKeyNum + 1,
					string2,
					">",
					@string,
					startKeyNum + 2,
					string2,
					"=",
					@string,
					startKeyNum + 3,
					string2,
					"≤",
					@string,
					startKeyNum + 4,
					string2,
					"≥"
				});
				break;
			}
			if (Global.StatusInfoFill != null)
			{
				Global.StatusInfoFill(message, Global.RightColor, true);
			}
		}
		public void ChangeBorder(PaintEventArgs e, PictureBox label, int clickNum)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Pen black = Pens.Black;
			Point[] points = new Point[]
			{
				new Point(0, 0),
				new Point(8, label.Height - 1),
				new Point(label.Width - 8, label.Height - 1),
				new Point(label.Width, 0)
			};
			if (label.Name.Contains(clickNum.ToString()))
			{
				e.Graphics.FillPolygon(Brushes.White, points);
				this.font = new Font("宋体", 10.5f, FontStyle.Bold);
				this.PicClickBrush = Brushes.Red;
			}
			else
			{
				this.font = new Font("宋体", 10.5f, FontStyle.Regular);
				this.PicClickBrush = Brushes.Black;
			}
			Image image;
			try
			{
				image = Image.FromFile("images\\imageBt" + label.Name.Substring(10, 1) + ".gif");
			}
			catch (Exception)
			{
				image = null;
			}
			if (image != null)
			{
				e.Graphics.DrawImage(image, 12, 2);
			}
			if (label.Tag != null)
			{
				int num = 0;
				if (image != null)
				{
					num = image.Width;
				}
				if (this.isRunOnceDraw)
				{
					int num2 = 9;
					float width = e.Graphics.MeasureString(label.Tag.ToString(), label.Font).Width;
					while (width + 15f + (float)num > (float)(label.Width + 2))
					{
						this.font = new Font(this.font.FontFamily, (float)num2--, this.font.Style);
						width = e.Graphics.MeasureString(label.Tag.ToString(), this.font).Width;
					}
					this.isRunOnceDraw = false;
				}
				e.Graphics.DrawString(label.Tag.ToString(), this.font, this.PicClickBrush, (float)(num + 12), 3f);
			}
			e.Graphics.DrawLine(black, 0, 0, 8, label.Height - 1);
			e.Graphics.DrawLine(black, 8, label.Height - 1, label.Width - 8, label.Height - 1);
			e.Graphics.DrawLine(black, label.Width - 8, label.Height - 1, label.Width - 1, 0);
		}
		public int PictureBoxClick(PictureBox pb, Panel panelPicBt, int clickNum)
		{
			foreach (Control control in panelPicBt.Controls)
			{
				if (control.Name == "pictureBox" + clickNum)
				{
					control.Invalidate();
					break;
				}
			}
			clickNum = Convert.ToInt32(pb.Name.Substring(10, 1));
			pb.Invalidate();
			foreach (Control control2 in panelPicBt.Controls)
			{
				if (control2 is PictureBox)
				{
					control2.BringToFront();
				}
			}
			pb.BringToFront();
			if (clickNum == 4)
			{
				IniData.GetInstance().SetDoubleClick = true;
				IniData.GetInstance().AutoPrice = true;
			}
			else
			{
				IniData.GetInstance().SetDoubleClick = false;
				IniData.GetInstance().AutoPrice = false;
			}
			return clickNum;
		}
	}
}
