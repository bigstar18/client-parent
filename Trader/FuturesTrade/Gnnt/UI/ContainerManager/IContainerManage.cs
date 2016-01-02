namespace FuturesTrade.Gnnt.UI.ContainerManager
{
    using System;
    using System.Windows.Forms;

    internal interface IContainerManage
    {
        void ControlLayOut();
        void MainFormKeyUp(Keys keyData);
        void SetPanelWidth();

        int FormStaly { get; set; }
    }
}
