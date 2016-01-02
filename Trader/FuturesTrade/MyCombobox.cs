using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace FuturesTrade
{
    class MyCombobox:ComboBox
    {
        public MyCombobox() 
        {
        this.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            

        
        }
        //[System.Runtime.InteropServices.DllImport("user32.dll ")]
        //static extern IntPtr GetWindowDC(IntPtr hWnd);//返回hWnd参数所指定的窗口的设备环境。
        //[System.Runtime.InteropServices.DllImport("user32.dll ")]
        //static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC); 
        //protected override void WndProc(ref   Message  e)
        //{
        //    if (e.Msg == 0xf || e.Msg == 0x133)
        //    {
        //        IntPtr hDC = GetWindowDC(e.HWnd);
        //        if (hDC.ToInt32() == 0) //如果取设备上下文失败则返回
        //        {
        //            return;
        //        }
        //        base.WndProc(ref e);
        //        ControlPaint.DrawBorder(Graphics.FromHdc(hDC), new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height), Color.Black, ButtonBorderStyle.Solid);
        //        ReleaseDC(e.HWnd, hDC);  
        //    }
           
        //}
    }
}
