// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.MyButton
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

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
