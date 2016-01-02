using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TabTest
{
    public class MyTabControl:TabControl
    {
        /// <summary>
        /// tab页背景颜色
        /// </summary>
        private Color myBackColor = Color.FromArgb(240,240,240);
        public Color MyBackColor
        {
            get { return myBackColor; }
            set { myBackColor = value; }
        }
        /// <summary>
        /// 选项卡的字体颜色
        /// </summary>
        private Color fontColor = Color.Black;

        public Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }
        /// <summary>
        /// 选项卡选中后的颜色
        /// </summary>
        private Color focusedColor = Color.White;
     
        public Color FocusedColor
        {
            get { return focusedColor ; }
            set { focusedColor = value; }
        }
        
        
        public MyTabControl()
        {
 
            this.SizeMode = TabSizeMode.Fixed;  // 大小模式为固定
            this.ItemSize = new Size(80,25);   // 设定每个标签的尺寸
            //backImage = new Bitmap(this.GetType(), "bgi.bmp");
            base.SetStyle(
             ControlStyles.UserPaint |                      // 控件将自行绘制，而不是通过操作系统来绘制
             ControlStyles.OptimizedDoubleBuffer |          // 该控件首先在缓冲区中绘制，而不是直接绘制到屏幕上，这样可以减少闪烁
             ControlStyles.AllPaintingInWmPaint |           // 控件将忽略 WM_ERASEBKGND 窗口消息以减少闪烁
             ControlStyles.ResizeRedraw |                   // 在调整控件大小时重绘控件
             ControlStyles.SupportsTransparentBackColor,    // 控件接受 alpha 组件小于 255 的 BackColor 以模拟透明
             true);                                         // 设置以上值为 true
            base.UpdateStyles();
        }
  
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

           // e.Graphics.DrawLine(Pens.Gray, this.Left, this.TabPages[0].Top-2, this.Right, this.TabPages[0].Top-2);
            e.Graphics.FillRectangle(new SolidBrush(myBackColor), this.Left, this.Top, this.Width, this.Height);
            for (int i = 0; i < this.TabCount; i++)
            {
                if (this.SelectedIndex == i)
                {
                    e.Graphics.FillRectangle(new SolidBrush(focusedColor), this.GetTabRect(i));
                }
                e.Graphics.DrawLine(Pens.Gray, new Point(this.GetTabRect(i).Location.X + this.GetTabRect(i).Width, this.GetTabRect(i).Y), new Point(this.GetTabRect(i).Location.X + this.GetTabRect(i).Width, this.GetTabRect(i).Y+this.GetTabRect(i).Height));//画选项卡边框
                Rectangle bounds = this.GetTabRect(i);
                PointF textPoint = new PointF();
                SizeF textSize = TextRenderer.MeasureText(this.TabPages[i].Text, this.Font);
                // 注意要加上每个标签的左偏移量X
                textPoint.X
                    = bounds.X + (bounds.Width - textSize.Width) / 2;
                textPoint.Y
                    = (bounds.Height - textSize.Height) / 2 + this.Padding.Y;

                // Draw highlights
                //e.Graphics.DrawString(
                //    this.TabPages[i].Text,
                //    this.Font,
                //    SystemBrushes.ControlLightLight,    // 高光颜色
                //    textPoint.X,
                    //textPoint.Y);

                // 绘制正常文字
             
                textPoint.Y--;
                e.Graphics.DrawString(
                    this.TabPages[i].Text,
                    this.Font,
                    new SolidBrush(fontColor),    // 正常颜色
                    textPoint.X,
                    textPoint.Y);
               
                //if (this.ImageList != null)
                //{
                //    int index = this.TabPages[i].ImageIndex;
                //    string key = this.TabPages[i].ImageKey;
                //    Image icon = new Bitmap(1, 1);

                //    if (index > -1)
                //    {
                //        icon = this.ImageList.Images[index];
                //    }
                //    if (!string.IsNullOrEmpty(key))
                //    {
                //        icon = this.ImageList.Images[key];
                //    }
                //    e.Graphics.DrawImage(
                //        icon,
                //        bounds.X + (bounds.Width - icon.Width) / 2,
                //        bounds.Top + this.Padding.Y);
                //}

            }
          


        }

    }

}
