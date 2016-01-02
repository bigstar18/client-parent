using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary.util;
namespace Gnnt.MEBS.HQClient.gnnt.ClientForms
{
	public class UserSet : Form
	{
		private IContainer components;
		private TabControl tabControlSet;
		private TabPage setItems;
		private TabPage setOther;
		private Button buttonConfirm;
		private Button buttonCancel;
		private Button buttonApply;
		private ComboBox cbMultiQuoteStaticIndex;
		private ComboBox cbBuySell;
		private Label labelMultiQuoteStaticIndex;
		private Label labelBuySell;
		private GroupBox gbInfo;
		private Label lbInfo;
		private Button butDefault;
		private Button butDown;
		private Button butUp;
		private Label lbCur;
		private Label lUseable;
		private Button butDelItem;
		private Button butAddItem;
		private ListBox lbCurrentItems;
		private ListBox lbUseable;
		private HQForm m_hqForm;
		private HQClientMain m_hqClient;
		private PluginInfo pluginInfo;
		private SetInfo setInfo;
		private UserSetFlag userSetFlag = new UserSetFlag();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserSet));
			this.tabControlSet = new TabControl();
			this.setItems = new TabPage();
			this.butDefault = new Button();
			this.butDown = new Button();
			this.butUp = new Button();
			this.lbCur = new Label();
			this.lUseable = new Label();
			this.butDelItem = new Button();
			this.butAddItem = new Button();
			this.lbCurrentItems = new ListBox();
			this.lbUseable = new ListBox();
			this.setOther = new TabPage();
			this.gbInfo = new GroupBox();
			this.lbInfo = new Label();
			this.cbMultiQuoteStaticIndex = new ComboBox();
			this.cbBuySell = new ComboBox();
			this.labelMultiQuoteStaticIndex = new Label();
			this.labelBuySell = new Label();
			this.buttonConfirm = new Button();
			this.buttonCancel = new Button();
			this.buttonApply = new Button();
			this.tabControlSet.SuspendLayout();
			this.setItems.SuspendLayout();
			this.setOther.SuspendLayout();
			this.gbInfo.SuspendLayout();
			base.SuspendLayout();
			this.tabControlSet.Controls.Add(this.setItems);
			this.tabControlSet.Controls.Add(this.setOther);
			this.tabControlSet.Location = new Point(6, 4);
			this.tabControlSet.Name = "tabControlSet";
			this.tabControlSet.SelectedIndex = 0;
			this.tabControlSet.Size = new Size(306, 275);
			this.tabControlSet.TabIndex = 0;
			this.tabControlSet.SelectedIndexChanged += new EventHandler(this.tabControlSet_SelectedIndexChanged);
			this.setItems.Controls.Add(this.butDefault);
			this.setItems.Controls.Add(this.butDown);
			this.setItems.Controls.Add(this.butUp);
			this.setItems.Controls.Add(this.lbCur);
			this.setItems.Controls.Add(this.lUseable);
			this.setItems.Controls.Add(this.butDelItem);
			this.setItems.Controls.Add(this.butAddItem);
			this.setItems.Controls.Add(this.lbCurrentItems);
			this.setItems.Controls.Add(this.lbUseable);
			this.setItems.Location = new Point(4, 21);
			this.setItems.Name = "setItems";
			this.setItems.Padding = new Padding(3);
			this.setItems.Size = new Size(298, 250);
			this.setItems.TabIndex = 0;
			this.setItems.Text = "设置列标题";
			this.setItems.UseVisualStyleBackColor = true;
			this.butDefault.Location = new Point(8, 223);
			this.butDefault.Name = "butDefault";
			this.butDefault.Size = new Size(65, 23);
			this.butDefault.TabIndex = 20;
			this.butDefault.Text = "恢复默认";
			this.butDefault.UseVisualStyleBackColor = true;
			this.butDefault.Click += new EventHandler(this.butDefault_Click);
			this.butDown.Location = new Point(242, 221);
			this.butDown.Name = "butDown";
			this.butDown.Size = new Size(46, 23);
			this.butDown.TabIndex = 19;
			this.butDown.Text = "下移";
			this.butDown.UseVisualStyleBackColor = true;
			this.butDown.Click += new EventHandler(this.butDown_Click);
			this.butUp.Location = new Point(187, 221);
			this.butUp.Name = "butUp";
			this.butUp.Size = new Size(46, 23);
			this.butUp.TabIndex = 18;
			this.butUp.Text = "上移";
			this.butUp.UseVisualStyleBackColor = true;
			this.butUp.Click += new EventHandler(this.butUp_Click);
			this.lbCur.AutoSize = true;
			this.lbCur.Location = new Point(179, 12);
			this.lbCur.Name = "lbCur";
			this.lbCur.Size = new Size(65, 12);
			this.lbCur.TabIndex = 17;
			this.lbCur.Text = "当前的标题";
			this.lUseable.AutoSize = true;
			this.lUseable.Location = new Point(6, 12);
			this.lUseable.Name = "lUseable";
			this.lUseable.Size = new Size(65, 12);
			this.lUseable.TabIndex = 16;
			this.lUseable.Text = "可用的标题";
			this.butDelItem.Location = new Point(120, 145);
			this.butDelItem.Name = "butDelItem";
			this.butDelItem.Size = new Size(54, 23);
			this.butDelItem.TabIndex = 15;
			this.butDelItem.Text = "<-删除";
			this.butDelItem.UseVisualStyleBackColor = true;
			this.butDelItem.Click += new EventHandler(this.butDelItem_Click);
			this.butAddItem.Location = new Point(120, 74);
			this.butAddItem.Name = "butAddItem";
			this.butAddItem.Size = new Size(54, 23);
			this.butAddItem.TabIndex = 14;
			this.butAddItem.Text = "添加->";
			this.butAddItem.UseVisualStyleBackColor = true;
			this.butAddItem.Click += new EventHandler(this.butAddItem_Click);
			this.lbCurrentItems.FormattingEnabled = true;
			this.lbCurrentItems.ItemHeight = 12;
			this.lbCurrentItems.Location = new Point(180, 34);
			this.lbCurrentItems.Name = "lbCurrentItems";
			this.lbCurrentItems.Size = new Size(112, 184);
			this.lbCurrentItems.TabIndex = 13;
			this.lbCurrentItems.DoubleClick += new EventHandler(this.butDelItem_Click);
			this.lbUseable.FormattingEnabled = true;
			this.lbUseable.ItemHeight = 12;
			this.lbUseable.Location = new Point(6, 34);
			this.lbUseable.Name = "lbUseable";
			this.lbUseable.Size = new Size(108, 184);
			this.lbUseable.TabIndex = 12;
			this.lbUseable.DoubleClick += new EventHandler(this.butAddItem_Click);
			this.setOther.Controls.Add(this.gbInfo);
			this.setOther.Controls.Add(this.cbMultiQuoteStaticIndex);
			this.setOther.Controls.Add(this.cbBuySell);
			this.setOther.Controls.Add(this.labelMultiQuoteStaticIndex);
			this.setOther.Controls.Add(this.labelBuySell);
			this.setOther.Location = new Point(4, 21);
			this.setOther.Name = "setOther";
			this.setOther.Padding = new Padding(3);
			this.setOther.Size = new Size(298, 250);
			this.setOther.TabIndex = 1;
			this.setOther.Text = "设置杂项";
			this.setOther.UseVisualStyleBackColor = true;
			this.gbInfo.Controls.Add(this.lbInfo);
			this.gbInfo.Location = new Point(4, 73);
			this.gbInfo.Name = "gbInfo";
			this.gbInfo.Size = new Size(288, 172);
			this.gbInfo.TabIndex = 2;
			this.gbInfo.TabStop = false;
			this.gbInfo.Text = "修改说明";
			this.lbInfo.ForeColor = Color.RoyalBlue;
			this.lbInfo.Location = new Point(20, 27);
			this.lbInfo.Name = "lbInfo";
			this.lbInfo.Size = new Size(246, 132);
			this.lbInfo.TabIndex = 0;
			this.lbInfo.Text = "说明";
			this.cbMultiQuoteStaticIndex.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbMultiQuoteStaticIndex.FormattingEnabled = true;
			this.cbMultiQuoteStaticIndex.Location = new Point(103, 47);
			this.cbMultiQuoteStaticIndex.Name = "cbMultiQuoteStaticIndex";
			this.cbMultiQuoteStaticIndex.Size = new Size(71, 20);
			this.cbMultiQuoteStaticIndex.TabIndex = 1;
			this.cbMultiQuoteStaticIndex.SelectedIndexChanged += new EventHandler(this.cbMultiQuoteStaticIndex_SelectedIndexChanged);
			this.cbMultiQuoteStaticIndex.Enter += new EventHandler(this.cbMultiQuoteStaticIndex_Enter);
			this.cbBuySell.FormattingEnabled = true;
			this.cbBuySell.Location = new Point(103, 16);
			this.cbBuySell.Name = "cbBuySell";
			this.cbBuySell.Size = new Size(71, 20);
			this.cbBuySell.TabIndex = 1;
			this.cbBuySell.SelectedIndexChanged += new EventHandler(this.cbBuySell_SelectedIndexChanged);
			this.cbBuySell.Enter += new EventHandler(this.cbBuySell_Enter);
			this.labelMultiQuoteStaticIndex.AutoSize = true;
			this.labelMultiQuoteStaticIndex.Location = new Point(22, 50);
			this.labelMultiQuoteStaticIndex.Name = "labelMultiQuoteStaticIndex";
			this.labelMultiQuoteStaticIndex.Size = new Size(65, 12);
			this.labelMultiQuoteStaticIndex.TabIndex = 0;
			this.labelMultiQuoteStaticIndex.Text = "保留列数：";
			this.labelBuySell.AutoSize = true;
			this.labelBuySell.Location = new Point(22, 19);
			this.labelBuySell.Name = "labelBuySell";
			this.labelBuySell.Size = new Size(77, 12);
			this.labelBuySell.TabIndex = 0;
			this.labelBuySell.Text = "设置买卖盘：";
			this.buttonConfirm.Location = new Point(124, 285);
			this.buttonConfirm.Name = "buttonConfirm";
			this.buttonConfirm.Size = new Size(60, 23);
			this.buttonConfirm.TabIndex = 1;
			this.buttonConfirm.Text = "确定";
			this.buttonConfirm.UseVisualStyleBackColor = true;
			this.buttonConfirm.Click += new EventHandler(this.buttonConfirm_Click);
			this.buttonCancel.Location = new Point(188, 285);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(60, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.buttonApply.Enabled = false;
			this.buttonApply.Location = new Point(252, 285);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new Size(60, 23);
			this.buttonApply.TabIndex = 1;
			this.buttonApply.Text = "应用&A";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new EventHandler(this.buttonApply_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(317, 312);
			base.Controls.Add(this.buttonApply);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonConfirm);
			base.Controls.Add(this.tabControlSet);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UserSet";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "UserSet";
			base.Load += new EventHandler(this.UserSet_Load);
			this.tabControlSet.ResumeLayout(false);
			this.setItems.ResumeLayout(false);
			this.setItems.PerformLayout();
			this.setOther.ResumeLayout(false);
			this.setOther.PerformLayout();
			this.gbInfo.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public UserSet(HQForm hqForm)
		{
			this.InitializeComponent();
			this.m_hqForm = hqForm;
		}
		private void UserSet_Load(object sender, EventArgs e)
		{
			this.m_hqClient = this.m_hqForm.CurHQClient;
			this.pluginInfo = this.m_hqClient.pluginInfo;
			this.setInfo = this.m_hqClient.setInfo;
			this.SetControlText();
			this.initSetItems(this.setInfo.MultiQuoteItems);
			this.initSetOther();
			this.buttonApply.Enabled = false;
		}
		private void SetControlText()
		{
			this.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_UserSetTitle");
			this.tabControlSet.TabPages[0].Text = this.pluginInfo.HQResourceManager.GetString("HQStr_SetColsTitle");
			this.tabControlSet.TabPages[1].Text = this.pluginInfo.HQResourceManager.GetString("HQStr_SetOther");
			this.lUseable.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_UsableColsTitle");
			this.lbCur.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_CurrentColsTitle");
			this.butAddItem.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Add");
			this.butDelItem.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Del");
			this.butDefault.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Default");
			this.butUp.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_GoUp");
			this.butDown.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_GoDown");
			this.buttonConfirm.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Confirm");
			this.buttonCancel.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Cancel");
			this.buttonApply.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_Apply");
			this.labelBuySell.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_SetB_SNum");
			this.labelMultiQuoteStaticIndex.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_HoldCols");
			this.gbInfo.Text = this.pluginInfo.HQResourceManager.GetString("HQStr_UpdateExplanation");
		}
		private void initSetItems(string multiQuoteItems)
		{
			string[] array = multiQuoteItems.Split(new char[]
			{
				';'
			});
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				MultiQuoteItemInfo multiQuoteItemInfo = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[array[i]];
				if (multiQuoteItemInfo != null && multiQuoteItemInfo.name.Length != 0)
				{
					AddValue value = new AddValue(multiQuoteItemInfo.name, array[i]);
					arrayList.Add(value);
				}
			}
			if (arrayList.Count != 0)
			{
				this.lbCurrentItems.DataSource = arrayList;
				this.lbCurrentItems.DisplayMember = "Display";
				this.lbCurrentItems.ValueMember = "Value";
			}
			else
			{
				this.lbCurrentItems.DisplayMember = null;
				this.lbCurrentItems.ValueMember = null;
			}
			ArrayList arrayList2 = new ArrayList();
			string[] array2 = this.m_hqClient.strAllItemName.Split(new char[]
			{
				';'
			});
			for (int j = 0; j < array2.Length; j++)
			{
				MultiQuoteItemInfo multiQuoteItemInfo2 = (MultiQuoteItemInfo)this.m_hqClient.m_htItemInfo[array2[j]];
				if (multiQuoteItemInfo2 != null && multiQuoteItemInfo2.name.Length != 0 && !multiQuoteItems.Contains(array2[j]))
				{
					AddValue value2 = new AddValue(multiQuoteItemInfo2.name, array2[j]);
					arrayList2.Add(value2);
				}
			}
			if (arrayList2.Count != 0)
			{
				this.lbUseable.DataSource = arrayList2;
				this.lbUseable.DisplayMember = "Display";
				this.lbUseable.ValueMember = "Value";
			}
			else
			{
				this.lbUseable.DisplayMember = null;
				this.lbUseable.ValueMember = null;
			}
			this.lbUseable.DataSource = arrayList2;
		}
		private void initSetOther()
		{
			for (int i = 1; i <= 5; i++)
			{
				this.cbBuySell.Items.Add(i);
			}
			this.cbBuySell.SelectedIndex = this.m_hqClient.iShowBuySellPrice - 1;
			this.labelBuySell.Visible = false;
			this.cbBuySell.Visible = false;
			this.labelMultiQuoteStaticIndex.Top -= 28;
			this.cbMultiQuoteStaticIndex.Top -= 28;
			string[] array = this.setInfo.MultiQuoteItems.Split(new char[]
			{
				';'
			});
			for (int j = 3; j < array.Length; j++)
			{
				this.cbMultiQuoteStaticIndex.Items.Add(j);
			}
			this.cbMultiQuoteStaticIndex.SelectedIndex = this.setInfo.MultiQuoteStaticIndex - 3;
		}
		private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tabControlSet.SelectedIndex == 1)
			{
				this.cbMultiQuoteStaticIndex.Focus();
			}
		}
		private void butAddItem_Click(object sender, EventArgs e)
		{
			if (this.lbUseable.SelectedIndex == -1)
			{
				MessageBox.Show("请选择可用列！");
				return;
			}
			int selectedIndex = this.lbUseable.SelectedIndex;
			if (selectedIndex > -1)
			{
				ArrayList arrayList = (ArrayList)this.lbUseable.DataSource;
				AddValue value = (AddValue)arrayList[selectedIndex];
				ArrayList arrayList2 = (ArrayList)this.lbCurrentItems.DataSource;
				arrayList2.Add(value);
				arrayList.RemoveAt(selectedIndex);
				this.lbUseable.DataSource = null;
				this.lbCurrentItems.DataSource = null;
				if (arrayList.Count != 0)
				{
					this.lbUseable.DataSource = arrayList;
					this.lbUseable.DisplayMember = "Display";
					this.lbUseable.ValueMember = "Value";
				}
				else
				{
					this.lbUseable.DisplayMember = null;
					this.lbUseable.ValueMember = null;
				}
				if (arrayList2.Count != 0)
				{
					this.lbCurrentItems.DataSource = arrayList2;
					this.lbCurrentItems.DisplayMember = "Display";
					this.lbCurrentItems.ValueMember = "Value";
				}
				else
				{
					this.lbCurrentItems.DisplayMember = null;
					this.lbCurrentItems.ValueMember = null;
				}
				if (this.lbUseable.Items.Count == 0)
				{
					this.butAddItem.Enabled = false;
				}
				else
				{
					if (selectedIndex >= this.lbUseable.Items.Count)
					{
						this.lbUseable.SelectedIndex = selectedIndex - 1;
					}
					else
					{
						this.lbUseable.SelectedIndex = selectedIndex;
					}
				}
				if (!this.butDelItem.Enabled)
				{
					this.butDelItem.Enabled = true;
				}
				this.userSetFlag.itemsFlag = true;
				this.buttonApply.Enabled = true;
			}
		}
		private void butDelItem_Click(object sender, EventArgs e)
		{
			if (this.lbCurrentItems.SelectedIndex == -1)
			{
				MessageBox.Show("请选择当前标题！");
				return;
			}
			if (this.lbCurrentItems.Items.Count < 4)
			{
				MessageBox.Show("最少保留三列可用");
				return;
			}
			int selectedIndex = this.lbCurrentItems.SelectedIndex;
			if (selectedIndex > -1)
			{
				ArrayList arrayList = (ArrayList)this.lbUseable.DataSource;
				ArrayList arrayList2 = (ArrayList)this.lbCurrentItems.DataSource;
				AddValue value = (AddValue)arrayList2[selectedIndex];
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				arrayList.Add(value);
				arrayList2.RemoveAt(selectedIndex);
				this.lbUseable.DataSource = null;
				this.lbCurrentItems.DataSource = null;
				if (arrayList.Count != 0)
				{
					this.lbUseable.DataSource = arrayList;
					this.lbUseable.DisplayMember = "Display";
					this.lbUseable.ValueMember = "Value";
				}
				else
				{
					this.lbUseable.DisplayMember = null;
					this.lbUseable.ValueMember = null;
				}
				if (arrayList2.Count != 0)
				{
					this.lbCurrentItems.DataSource = arrayList2;
					this.lbCurrentItems.DisplayMember = "Display";
					this.lbCurrentItems.ValueMember = "Value";
				}
				else
				{
					this.lbCurrentItems.DisplayMember = null;
					this.lbCurrentItems.ValueMember = null;
				}
				if (this.lbCurrentItems.Items.Count == 3)
				{
					this.butDelItem.Enabled = false;
				}
				else
				{
					if (selectedIndex >= this.lbCurrentItems.Items.Count)
					{
						this.lbCurrentItems.SelectedIndex = selectedIndex - 1;
					}
					else
					{
						this.lbCurrentItems.SelectedIndex = selectedIndex;
					}
				}
				if (!this.butAddItem.Enabled)
				{
					this.butAddItem.Enabled = true;
				}
				this.userSetFlag.itemsFlag = true;
				this.buttonApply.Enabled = true;
			}
		}
		private void butDefault_Click(object sender, EventArgs e)
		{
			this.initSetItems(this.m_hqClient.strDefaultItem);
			this.userSetFlag.itemsFlag = true;
			this.buttonApply.Enabled = true;
		}
		private void butUp_Click(object sender, EventArgs e)
		{
			if (this.lbCurrentItems.SelectedIndex == -1)
			{
				MessageBox.Show("请选择当前标题！");
				return;
			}
			int selectedIndex = this.lbCurrentItems.SelectedIndex;
			if (selectedIndex > 0)
			{
				ArrayList arrayList = (ArrayList)this.lbCurrentItems.DataSource;
				AddValue value = (AddValue)arrayList[selectedIndex];
				arrayList[selectedIndex] = arrayList[selectedIndex - 1];
				arrayList[selectedIndex - 1] = value;
				this.lbCurrentItems.DataSource = null;
				if (arrayList.Count != 0)
				{
					this.lbCurrentItems.DataSource = arrayList;
					this.lbCurrentItems.DisplayMember = "Display";
					this.lbCurrentItems.ValueMember = "Value";
				}
				else
				{
					this.lbCurrentItems.DisplayMember = null;
					this.lbCurrentItems.ValueMember = null;
				}
				this.lbCurrentItems.SelectedIndex = selectedIndex - 1;
				this.userSetFlag.itemsFlag = true;
				this.buttonApply.Enabled = true;
			}
		}
		private void butDown_Click(object sender, EventArgs e)
		{
			if (this.lbCurrentItems.SelectedIndex == -1)
			{
				MessageBox.Show("请选择当前标题！");
				return;
			}
			int selectedIndex = this.lbCurrentItems.SelectedIndex;
			if (selectedIndex < this.lbCurrentItems.Items.Count - 1)
			{
				ArrayList arrayList = (ArrayList)this.lbCurrentItems.DataSource;
				AddValue value = (AddValue)arrayList[selectedIndex];
				arrayList[selectedIndex] = arrayList[selectedIndex + 1];
				arrayList[selectedIndex + 1] = value;
				this.lbCurrentItems.DataSource = null;
				if (arrayList.Count != 0)
				{
					this.lbCurrentItems.DataSource = arrayList;
					this.lbCurrentItems.DisplayMember = "Display";
					this.lbCurrentItems.ValueMember = "Value";
				}
				else
				{
					this.lbCurrentItems.DisplayMember = null;
					this.lbCurrentItems.ValueMember = null;
				}
				this.lbCurrentItems.SelectedIndex = selectedIndex + 1;
				this.userSetFlag.itemsFlag = true;
				this.buttonApply.Enabled = true;
			}
		}
		private void cbBuySell_Enter(object sender, EventArgs e)
		{
			this.gbInfo.Text = "买卖盘" + this.pluginInfo.HQResourceManager.GetString("HQStr_UpdateExplanation");
			this.lbInfo.Text = "设置在k线图/分时图界面中显示的买卖盘数量";
		}
		private void cbMultiQuoteStaticIndex_Enter(object sender, EventArgs e)
		{
			this.gbInfo.Text = "保留列数" + this.pluginInfo.HQResourceManager.GetString("HQStr_UpdateExplanation");
			this.lbInfo.Text = "设置在报价排名界面中显示的最少列数";
		}
		private void cbBuySell_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.userSetFlag.buySellFalg = true;
			this.buttonApply.Enabled = true;
		}
		private void cbMultiQuoteStaticIndex_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.userSetFlag.multiQuoteStaticIndex = true;
			this.buttonApply.Enabled = true;
		}
		private void buttonApply_Click(object sender, EventArgs e)
		{
			if (this.userSetFlag.buySellFalg && this.m_hqClient.iShowBuySellPrice != this.cbBuySell.SelectedIndex + 1)
			{
				this.setInfo.saveSetInfo("ShowBuySellPrice", Tools.StrToInt(this.cbBuySell.Text, 3).ToString());
				this.m_hqClient.iShowBuySellPrice = this.cbBuySell.SelectedIndex + 1;
				if (this.m_hqClient.CurrentPage == 2 || this.m_hqClient.CurrentPage == 1)
				{
					this.m_hqForm.Repaint();
				}
			}
			if (this.userSetFlag.multiQuoteStaticIndex && this.cbMultiQuoteStaticIndex.SelectedIndex != this.setInfo.MultiQuoteStaticIndex - 3)
			{
				this.setInfo.saveSetInfo("MultiQuoteStaticIndex", (this.cbMultiQuoteStaticIndex.SelectedIndex + 3).ToString());
				this.setInfo.MultiQuoteStaticIndex = this.cbMultiQuoteStaticIndex.SelectedIndex + 3;
				if (this.m_hqClient.CurrentPage == 0)
				{
					this.m_hqForm.UserCommand("60");
					this.m_hqForm.Repaint();
				}
			}
			if (this.userSetFlag.itemsFlag)
			{
				string text = "No;";
				ArrayList arrayList = (ArrayList)this.lbCurrentItems.DataSource;
				for (int i = 0; i < arrayList.Count; i++)
				{
					AddValue addValue = (AddValue)arrayList[i];
					text = text + addValue.Value + ";";
				}
				this.setInfo.saveSetInfo("MultiQuoteItems", text);
				this.setInfo.MultiQuoteItems = text;
				if (this.m_hqClient.CurrentPage == 0)
				{
					this.m_hqForm.UserCommand("60");
					this.m_hqForm.Repaint();
				}
			}
			this.userSetFlag = new UserSetFlag();
			this.buttonApply.Enabled = false;
		}
		private void buttonConfirm_Click(object sender, EventArgs e)
		{
			if (this.buttonApply.Enabled)
			{
				this.buttonApply_Click(null, null);
			}
			base.Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
	}
}
