// Decompiled with JetBrains decompiler
// Type: HQPlugin.Form1
// Assembly: MEBS_HQPlugin, Version=3.0.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 44B89E14-6101-43B0-AA7B-677C3C2AF0B8
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\Plugins\MEBS_HQPlugin.dll

using System.ComponentModel;
using System.Windows.Forms;

namespace HQPlugin
{
  public class Form1 : Form
  {
    private IContainer components;

    public Form1()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Text = "Form1";
    }
  }
}
