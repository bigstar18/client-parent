using System;
using System.Windows.Forms;
namespace FuturesTrade.Gnnt.UI.ContainerManager
{
	internal interface IContainerManage
	{
		int FormStaly
		{
			get;
			set;
		}
		void ControlLayOut();
		void SetPanelWidth();
		void MainFormKeyUp(Keys keyData);
	}
}
