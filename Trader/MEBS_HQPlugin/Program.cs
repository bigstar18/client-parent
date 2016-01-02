// Decompiled with JetBrains decompiler
// Type: HQPlugin.Program
// Assembly: MEBS_HQPlugin, Version=3.0.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 44B89E14-6101-43B0-AA7B-677C3C2AF0B8
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\MEBS_HQPlugin.dll

using System;
using System.Windows.Forms;

namespace HQPlugin
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}
