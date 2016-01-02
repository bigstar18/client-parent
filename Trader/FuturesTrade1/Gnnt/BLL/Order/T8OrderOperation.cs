namespace FuturesTrade.Gnnt.BLL.Order
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class T8OrderOperation
    {
        private Font font;
        private bool isRunOnceDraw = true;
        private Brush PicClickBrush = Brushes.Black;

        public void ChangeBorder(PaintEventArgs e, PictureBox label, int clickNum)
        {
            Image image;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen black = Pens.Black;
            Point[] points = new Point[] { new Point(0, 0), new Point(8, label.Height - 1), new Point(label.Width - 8, label.Height - 1), new Point(label.Width, 0) };
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
            try
            {
                image = Image.FromFile(@"images\imageBt" + label.Name.Substring(10, 1) + ".gif");
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
                int width = 0;
                if (image != null)
                {
                    width = image.Width;
                }
                if (this.isRunOnceDraw)
                {
                    int num2 = 9;
                    for (float i = e.Graphics.MeasureString(label.Tag.ToString(), label.Font).Width; ((i + 15f) + width) > (label.Width + 2); i = e.Graphics.MeasureString(label.Tag.ToString(), this.font).Width)
                    {
                        this.font = new Font(this.font.FontFamily, (float)num2--, this.font.Style);
                    }
                    this.isRunOnceDraw = false;
                }
                e.Graphics.DrawString(label.Tag.ToString(), this.font, this.PicClickBrush, (float)(width + 12), 3f);
            }
            e.Graphics.DrawLine(black, 0, 0, 8, label.Height - 1);
            e.Graphics.DrawLine(black, 8, label.Height - 1, label.Width - 8, label.Height - 1);
            e.Graphics.DrawLine(black, label.Width - 8, label.Height - 1, label.Width - 1, 0);
        }

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
            }
            else
            {
                e.Graphics.FillRectangle(pen2.Brush, e.Bounds);
                e.Graphics.DrawString((string)comboBoxBuyOrSall.Items[e.Index], comboBoxBuyOrSall.Font, pen.Brush, e.Bounds);
            }
        }

        public void KeyTip(int comboNum, int startKeyNum)
        {
            string str = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Press");
            string str2 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_meaning");
            string message = string.Empty;
            switch (comboNum)
            {
                case 1:
                    {
                        string str4 = Global.M_ResourceManager.GetString("Global_BuySellStrArr2");
                        string str5 = Global.M_ResourceManager.GetString("Global_BuySellStrArr1");
                        message = string.Concat(new object[] { str, startKeyNum, str2, str5, str, startKeyNum + 1, str2, str4 });
                        break;
                    }
                case 2:
                    {
                        string str6 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr1");
                        string str7 = Global.M_ResourceManager.GetString("Global_SettleBasisStrArr2");
                        string str8 = Global.M_ResourceManager.GetString("TradeStr_TransferToday");
                        string str9 = Global.M_ResourceManager.GetString("Global_CloseModeStrArr2");
                        message = string.Concat(new object[] { str, startKeyNum, str2, str6, str, startKeyNum + 1, str2, str7, str, startKeyNum + 2, str2, str8, str, startKeyNum + 3, str2, str9 });
                        break;
                    }
                case 3:
                    {
                        string str10 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_Bid");
                        string str11 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_AskPrice");
                        string str12 = Global.M_ResourceManager.GetString("TradeStr_T8MainForm_last");
                        message = string.Concat(new object[] { str, startKeyNum, str2, str10, str, startKeyNum + 1, str2, str11, str, startKeyNum + 2, str2, str12 });
                        break;
                    }
                case 4:
                    message = string.Concat(new object[] {
                        str, startKeyNum, str2, "<", str, startKeyNum + 1, str2, ">", str, startKeyNum + 2, str2, "=", str, startKeyNum + 3, str2, "≤",
                        str, startKeyNum + 4, str2, "≥"
                     });
                    break;
            }
            if (Global.StatusInfoFill != null)
            {
                Global.StatusInfoFill(message, Global.RightColor, true);
            }
        }

        public int PictureBoxClick(PictureBox pb, Panel panelPicBt, int clickNum)
        {
            foreach (Control control in panelPicBt.Controls)
            {
                if (control.Name == ("pictureBox" + clickNum))
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
                return clickNum;
            }
            IniData.GetInstance().SetDoubleClick = false;
            IniData.GetInstance().AutoPrice = false;
            return clickNum;
        }
    }
}
