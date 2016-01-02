using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient
{
	public class MyButton
	{
		public string Name;
		public string Text;
		public Font font;
		public bool Selected;
		public Point[] Points;
		public MyButton(string Name, string Text, bool Selected)
		{
			this.Name = Name;
			this.Text = Text;
			this.Selected = Selected;
		}
	}
}
