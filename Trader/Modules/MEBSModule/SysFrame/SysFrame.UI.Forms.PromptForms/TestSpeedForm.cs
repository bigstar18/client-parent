using SysFrame.Gnnt.Common.Library;
using SysFrame.Gnnt.Common.Operation;
using SysFrame.Gnnt.Common.Operation.Manager;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ToolsLibrary.util;
using ToolStripRender;
namespace SysFrame.UI.Forms.PromptForms
{
	public class TestSpeedForm : Form
	{
		private delegate void UpdateLine(string lineSrr, int indexId);
		private string toolSelect = string.Empty;
		private TestSpeedForm.UpdateLine updateLine;
		private IContainer components;
		private PictureBox picBoxHead;
		private Panel pnlMain;
		private Button btnTest;
		private ListBox listBoxServerSpeed;
		private Button buttonExit;
		private Button button;
		private Label label1;
		private Label labelTitleSpeed;
		private Label labelNetInfo;
		private ToolStrip toolStripButtons;
		public TestSpeedForm()
		{
			this.InitializeComponent();
		}
		public void UpdateListLine(string lineSrr, int indexId)
		{
			if (!base.IsDisposed)
			{
				this.updateLine = new TestSpeedForm.UpdateLine(this.UpdateListBoxLine);
				this.HandleCreated();
				base.Invoke(this.updateLine, new object[]
				{
					lineSrr,
					indexId
				});
			}
		}
		public void UpdateListBoxLine(string lineSrr, int indexId)
		{
			if (indexId < this.listBoxServerSpeed.Items.Count)
			{
				this.listBoxServerSpeed.Items[indexId] = lineSrr;
			}
		}
		private new void HandleCreated()
		{
			while (!base.IsHandleCreated)
			{
				Thread.Sleep(100);
			}
		}
		private void TestSpeedForm_Load(object sender, EventArgs e)
		{
			this.SetControlText();
			OperationManager.GetInstance().speedTestOperation.listBoxServerSpeed = this.listBoxServerSpeed;
			OperationManager.GetInstance().speedTestOperation.ListBoxOneLineCallBack = new SpeedTestOperation.repaceListBoxLineCallBack(this.UpdateListLine);
			OperationManager.GetInstance().speedTestOperation.serverSpeedDefault();
		}
		private void SetControlText()
		{
			this.pnlMain.BackgroundImage = (Image)Global.Modules.get_Plugins().get_MEBS_ResourceManager().GetObject("TradeImg_Skin1");
			base.Icon = Global.Modules.get_Plugins().get_SystemIcon();
			ToolStripColorTable toolStripColorTable = new ToolStripColorTable();
			int num = Tools.StrToInt((string)Global.htConfig["ColorR"], 300);
			int num2 = Tools.StrToInt((string)Global.htConfig["ColorG"], 300);
			int num3 = Tools.StrToInt((string)Global.htConfig["ColorB"], 300);
			if (num3 < 256 && num < 256 && num2 < 256)
			{
				toolStripColorTable.set_Base(Color.FromArgb((int)((byte)num), (int)((byte)num2), (int)((byte)num3)));
				toolStripColorTable.set_Fore(Color.White);
				toolStripColorTable.set_Border(Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30))));
				toolStripColorTable.set_BackHover(Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30))));
				toolStripColorTable.set_BackPressed(Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30))));
			}
			this.toolStripButtons.Renderer = new ProfessionalToolStripRendererEx(toolStripColorTable);
			this.AddNetInfoNode();
			ScaleForm.ScaleForms(this);
		}
		public void AddNetInfoNode()
		{
			foreach (DictionaryEntry dictionaryEntry in Global.Modules.get_Plugins().get_LoginPluginsHashtable())
			{
				string text = dictionaryEntry.Key.ToString();
				ToolStripItem toolStripItem = new ToolStripButton();
				toolStripItem.Name = text;
				toolStripItem.AutoSize = false;
				toolStripItem.Size = new Size(100, 30);
				toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
				toolStripItem.Margin = new Padding(1, 0, 1, 0);
				toolStripItem.TextAlign = ContentAlignment.MiddleCenter;
				toolStripItem.Text = text;
				toolStripItem.Font = new Font("宋体", 10f, FontStyle.Regular);
				toolStripItem.ForeColor = Color.White;
				toolStripItem.MouseEnter += new EventHandler(this.new_child_MouseEnter);
				toolStripItem.MouseLeave += new EventHandler(this.new_child_MouseLeave);
				if (OperationManager.GetInstance().stripButtonOperation.curLoginPluginName == Global.Modules.get_Plugins().get_LoginPluginsHashtable()[toolStripItem.Name].ToString())
				{
					((ToolStripButton)toolStripItem).CheckState = CheckState.Checked;
					this.labelNetInfo.Text = text + "服务器列表";
				}
				else
				{
					((ToolStripButton)toolStripItem).CheckState = CheckState.Unchecked;
				}
				this.toolStripButtons.Items.Add(toolStripItem);
			}
		}
		private void listBoxServerSpeed_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight = 20;
		}
		private void listBoxServerSpeed_DrawItem(object sender, DrawItemEventArgs e)
		{
			try
			{
				e.DrawBackground();
				e.DrawFocusRectangle();
				float num = (float)((e.Bounds.Height - e.Font.Height) / 2);
				RectangleF layoutRectangle = new RectangleF((float)e.Bounds.X, (float)e.Bounds.Y + num, (float)e.Bounds.Width, (float)e.Font.Height);
				e.Graphics.DrawString(this.listBoxServerSpeed.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), layoutRectangle);
			}
			catch (Exception)
			{
			}
		}
		private void btnTest_Click(object sender, EventArgs e)
		{
			OperationManager.GetInstance().speedTestOperation.serverLoad();
		}
		private void buttonExit_Click(object sender, EventArgs e)
		{
			base.Close();
			base.Dispose();
		}
		private void new_child_MouseLeave(object sender, EventArgs e)
		{
			Hashtable section = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems");
			int num = Tools.StrToInt((string)section["ColorR"], 300);
			int num2 = Tools.StrToInt((string)section["ColorG"], 300);
			int num3 = Tools.StrToInt((string)section["ColorB"], 300);
			if (num3 < 256 && num < 256 && num2 < 256)
			{
				ToolStripButton toolStripButton = sender as ToolStripButton;
				if (toolStripButton != null)
				{
					toolStripButton.BackColor = Color.FromArgb((int)((byte)num), (int)((byte)num2), (int)((byte)num3));
				}
			}
		}
		private void new_child_MouseEnter(object sender, EventArgs e)
		{
			Hashtable section = Global.Modules.get_Plugins().get_ConfigurationInfo().getSection("Systems");
			int num = Tools.StrToInt((string)section["ColorR"], 300);
			int num2 = Tools.StrToInt((string)section["ColorG"], 300);
			int num3 = Tools.StrToInt((string)section["ColorB"], 300);
			if (num3 < 256 && num < 256 && num2 < 256)
			{
				ToolStripButton toolStripButton = sender as ToolStripButton;
				if (toolStripButton != null)
				{
					toolStripButton.BackColor = Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30)));
				}
			}
		}
		private void button_Click(object sender, EventArgs e)
		{
			if (OperationManager.GetInstance().speedTestOperation.fristSelectIndex != this.listBoxServerSpeed.SelectedIndex && this.listBoxServerSpeed.SelectedIndex != -1)
			{
				OperationManager.GetInstance().speedTestOperation.setServerInfo();
			}
			OperationManager.GetInstance().speedTestOperation.refreshLoginComboBox(this.toolSelect);
			base.Close();
		}
		private void toolStripButtons_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.toolSelect = e.ClickedItem.Name;
			this.ChangeItemCheckState(e.ClickedItem);
			this.DisplayNetInfo(e.ClickedItem.Name);
		}
		private void DisplayNetInfo(string name)
		{
			OperationManager.GetInstance().stripButtonOperation.curLoginPluginName = Global.Modules.get_Plugins().get_AvailablePlugins().Find(Global.Modules.get_Plugins().get_LoginPluginsHashtable()[name].ToString()).get_Instance().get_Name();
			if (OperationManager.GetInstance().stripButtonOperation.curLoginPluginName != "")
			{
				this.labelNetInfo.Text = name + "服务器列表";
				OperationManager.GetInstance().speedTestOperation.serverSpeedDefault();
			}
		}
		private void ChangeItemCheckState(ToolStripItem toolstripitem)
		{
			foreach (ToolStripItem toolStripItem in this.toolStripButtons.Items)
			{
				if (toolstripitem.Name == toolStripItem.Name)
				{
					((ToolStripButton)toolstripitem).CheckState = CheckState.Checked;
				}
				else
				{
					((ToolStripButton)toolStripItem).CheckState = CheckState.Unchecked;
				}
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TestSpeedForm));
			this.pnlMain = new Panel();
			this.toolStripButtons = new ToolStrip();
			this.label1 = new Label();
			this.labelTitleSpeed = new Label();
			this.labelNetInfo = new Label();
			this.listBoxServerSpeed = new ListBox();
			this.btnTest = new Button();
			this.picBoxHead = new PictureBox();
			this.buttonExit = new Button();
			this.button = new Button();
			this.pnlMain.SuspendLayout();
			((ISupportInitialize)this.picBoxHead).BeginInit();
			base.SuspendLayout();
			this.pnlMain.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pnlMain, "pnlMain");
			this.pnlMain.Controls.Add(this.toolStripButtons);
			this.pnlMain.Controls.Add(this.label1);
			this.pnlMain.Controls.Add(this.labelTitleSpeed);
			this.pnlMain.Controls.Add(this.labelNetInfo);
			this.pnlMain.Controls.Add(this.listBoxServerSpeed);
			this.pnlMain.Name = "pnlMain";
			componentResourceManager.ApplyResources(this.toolStripButtons, "toolStripButtons");
			this.toolStripButtons.LayoutStyle = ToolStripLayoutStyle.Table;
			this.toolStripButtons.Name = "toolStripButtons";
			this.toolStripButtons.Stretch = true;
			this.toolStripButtons.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStripButtons_ItemClicked);
			this.toolStripButtons.MouseEnter += new EventHandler(this.new_child_MouseEnter);
			this.toolStripButtons.MouseLeave += new EventHandler(this.new_child_MouseLeave);
			this.label1.BackColor = Color.Transparent;
			this.label1.ForeColor = SystemColors.Highlight;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.labelTitleSpeed.BackColor = Color.Transparent;
			this.labelTitleSpeed.ForeColor = SystemColors.Highlight;
			componentResourceManager.ApplyResources(this.labelTitleSpeed, "labelTitleSpeed");
			this.labelTitleSpeed.Name = "labelTitleSpeed";
			this.labelNetInfo.BackColor = Color.Transparent;
			this.labelNetInfo.ForeColor = SystemColors.Highlight;
			componentResourceManager.ApplyResources(this.labelNetInfo, "labelNetInfo");
			this.labelNetInfo.Name = "labelNetInfo";
			this.listBoxServerSpeed.BackColor = SystemColors.InactiveBorder;
			this.listBoxServerSpeed.BorderStyle = BorderStyle.None;
			this.listBoxServerSpeed.DrawMode = DrawMode.OwnerDrawVariable;
			this.listBoxServerSpeed.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.listBoxServerSpeed, "listBoxServerSpeed");
			this.listBoxServerSpeed.Name = "listBoxServerSpeed";
			this.listBoxServerSpeed.DrawItem += new DrawItemEventHandler(this.listBoxServerSpeed_DrawItem);
			this.listBoxServerSpeed.MeasureItem += new MeasureItemEventHandler(this.listBoxServerSpeed_MeasureItem);
			componentResourceManager.ApplyResources(this.btnTest, "btnTest");
			this.btnTest.Name = "btnTest";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new EventHandler(this.btnTest_Click);
			componentResourceManager.ApplyResources(this.picBoxHead, "picBoxHead");
			this.picBoxHead.Name = "picBoxHead";
			this.picBoxHead.TabStop = false;
			this.buttonExit.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonExit, "buttonExit");
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
			componentResourceManager.ApplyResources(this.button, "button");
			this.button.Name = "button";
			this.button.UseVisualStyleBackColor = true;
			this.button.Click += new EventHandler(this.button_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.White;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.buttonExit;
			base.Controls.Add(this.button);
			base.Controls.Add(this.btnTest);
			base.Controls.Add(this.buttonExit);
			base.Controls.Add(this.pnlMain);
			base.Controls.Add(this.picBoxHead);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TestSpeedForm";
			base.Load += new EventHandler(this.TestSpeedForm_Load);
			this.pnlMain.ResumeLayout(false);
			((ISupportInitialize)this.picBoxHead).EndInit();
			base.ResumeLayout(false);
		}
	}
}
