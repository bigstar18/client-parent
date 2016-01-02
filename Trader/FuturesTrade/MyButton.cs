using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TabTest
{
    public class MyButton:Button
    {
        public MyButton() 
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.Popup;

            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ForeColor = Color.Black;
            
        } 
    }
}
