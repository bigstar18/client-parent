// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.ClientForms.HQForm
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
  public interface HQForm
  {
    Page_Main MainGraph { get; set; }

    HQClientMain CurHQClient { get; set; }

    bool IsNeedRepaint { get; set; }

    bool IsMultiCycle { get; set; }

    bool IsMultiCommidity { get; set; }

    bool IsEndPaint { get; set; }

    Point ScrollOffset { get; set; }

    Cursor M_Cursor { get; set; }

    bool AddMarketName { get; set; }

    Graphics M_Graphics { get; }

    event MouseEventHandler MainForm_MouseClick;

    event KeyEventHandler MainForm_KeyDown;

    event MouseEventHandler MainForm_MouseMove;

    event MouseEventHandler MainForm_MouseDoubleClick;

    void ClearRect(Graphics g, float x, float y, float width, float height);

    void Repaint();

    void EndPaint();

    void RepaintMin();

    void ReQueryCurClient();

    void RepaintBottom();

    void EndBottomPaint();

    Graphics TranslateTransform(Graphics g);

    void ChangeStock(bool bUp);

    void ShowPageMinLine(CommodityInfo commodityInfo);

    void ShowPageMinLine();

    void ShowPageKLine(CommodityInfo commodityInfo);

    void ShowPageKLine();

    void QueryStock(CommodityInfo commodityInfo);

    void UserCommand(string sCmd);

    bool isDisplayF10Menu();

    void DisplayCommodityInfo(string sCode);

    void ReMakeIndexMenu();

    void MultiQuoteMouseLeftClick(object sender, InterFace.CommodityInfoEventArgs e);

    void HQMainForm_KeyDown(object sender, KeyEventArgs e);
  }
}
