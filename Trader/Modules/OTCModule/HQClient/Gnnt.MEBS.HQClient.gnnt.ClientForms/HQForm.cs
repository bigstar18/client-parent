using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public interface HQForm
	{
		event MouseEventHandler MainForm_MouseClick;
		event KeyEventHandler MainForm_KeyDown;
		event MouseEventHandler MainForm_MouseMove;
		event MouseEventHandler MainForm_MouseDoubleClick;
		Page_Main MainGraph
		{
			get;
			set;
		}
		HQClientMain CurHQClient
		{
			get;
			set;
		}
		bool IsNeedRepaint
		{
			get;
			set;
		}
		bool IsMultiCycle
		{
			get;
			set;
		}
		bool IsMultiCommidity
		{
			get;
			set;
		}
		bool IsEndPaint
		{
			get;
			set;
		}
		Point ScrollOffset
		{
			get;
			set;
		}
		Cursor M_Cursor
		{
			get;
			set;
		}
		bool AddMarketName
		{
			get;
			set;
		}
		Graphics M_Graphics
		{
			get;
		}
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
