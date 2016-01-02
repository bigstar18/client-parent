using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TPME.Log;
using TradeClientApp.Gnnt.OTC.Library;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC
{
	public class TMainYJSZ : Form
	{
		private IContainer components;
		private Label label1;
		private ComboBox cbyjlx;
		private Label label2;
		private DateTimePicker dtsjqd;
		private Label label3;
		private DateTimePicker dtsjzd;
		private GroupBox groupBox1;
		private ComboBox cbyjtj;
		private Label label6;
		private Label label5;
		private ComboBox cbyjx;
		private Label label4;
		private GroupBox groupBox2;
		private CheckBox chbfcsy;
		private CheckBox chbtcck;
		private ComboBox cbxdsj;
		private Label label7;
		private Label label8;
		private Button button1;
		private Button button2;
		private TextBox tbfz;
		private TextBox tbsy;
		private Button bsy;
		private OpenFileDialog OFDYJSY;
		private Label fxlabel;
		private CheckBox chbsfqy;
		private NumericUpDown nudcfcs;
		private static CreateXmlFile _CreateXml;
		public int TYJSZHH;
		public bool TYJSZFLAG;
		private TMainForm _ParentForm;
		private string YJFileName = Global.ConfigPath + "yj" + Global.UserID + ".xml";
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
			this.label1 = new Label();
			this.cbyjlx = new ComboBox();
			this.label2 = new Label();
			this.dtsjqd = new DateTimePicker();
			this.label3 = new Label();
			this.dtsjzd = new DateTimePicker();
			this.groupBox1 = new GroupBox();
			this.fxlabel = new Label();
			this.tbfz = new TextBox();
			this.cbyjtj = new ComboBox();
			this.label6 = new Label();
			this.label5 = new Label();
			this.cbyjx = new ComboBox();
			this.label4 = new Label();
			this.groupBox2 = new GroupBox();
			this.tbsy = new TextBox();
			this.bsy = new Button();
			this.chbfcsy = new CheckBox();
			this.chbtcck = new CheckBox();
			this.cbxdsj = new ComboBox();
			this.label7 = new Label();
			this.label8 = new Label();
			this.button1 = new Button();
			this.button2 = new Button();
			this.OFDYJSY = new OpenFileDialog();
			this.chbsfqy = new CheckBox();
			this.nudcfcs = new NumericUpDown();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.nudcfcs).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(13, 29);
			this.label1.Name = "label1";
			this.label1.Size = new Size(59, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "预警类型:";
			this.cbyjlx.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbyjlx.FormattingEnabled = true;
			this.cbyjlx.Location = new Point(78, 27);
			this.cbyjlx.Name = "cbyjlx";
			this.cbyjlx.Size = new Size(121, 20);
			this.cbyjlx.TabIndex = 1;
			this.cbyjlx.SelectedIndexChanged += new EventHandler(this.cbyjlx_SelectedIndexChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(223, 29);
			this.label2.Name = "label2";
			this.label2.Size = new Size(59, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "有效时段:";
			this.dtsjqd.CustomFormat = "HH:mm:ss";
			this.dtsjqd.Format = DateTimePickerFormat.Custom;
			this.dtsjqd.Location = new Point(287, 27);
			this.dtsjqd.Name = "dtsjqd";
			this.dtsjqd.ShowUpDown = true;
			this.dtsjqd.Size = new Size(78, 21);
			this.dtsjqd.TabIndex = 2;
			this.dtsjqd.Value = new DateTime(2011, 5, 10, 9, 0, 0, 0);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(371, 29);
			this.label3.Name = "label3";
			this.label3.Size = new Size(17, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "至";
			this.dtsjzd.CustomFormat = "HH:mm:ss";
			this.dtsjzd.Format = DateTimePickerFormat.Custom;
			this.dtsjzd.Location = new Point(394, 27);
			this.dtsjzd.Name = "dtsjzd";
			this.dtsjzd.ShowUpDown = true;
			this.dtsjzd.Size = new Size(78, 21);
			this.dtsjzd.TabIndex = 3;
			this.dtsjzd.Value = new DateTime(2011, 5, 10, 18, 0, 0, 0);
			this.groupBox1.Controls.Add(this.fxlabel);
			this.groupBox1.Controls.Add(this.tbfz);
			this.groupBox1.Controls.Add(this.cbyjtj);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.cbyjx);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = Color.Black;
			this.groupBox1.Location = new Point(15, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(465, 104);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "预警条件设置";
			this.fxlabel.AutoSize = true;
			this.fxlabel.Location = new Point(430, 64);
			this.fxlabel.Name = "fxlabel";
			this.fxlabel.Size = new Size(11, 12);
			this.fxlabel.TabIndex = 13;
			this.fxlabel.Text = "%";
			this.tbfz.Location = new Point(287, 60);
			this.tbfz.Name = "tbfz";
			this.tbfz.Size = new Size(141, 21);
			this.tbfz.TabIndex = 12;
			this.tbfz.TextChanged += new EventHandler(this.tbfz_TextChanged);
			this.tbfz.KeyPress += new KeyPressEventHandler(this.textBox1_KeyPress);
			this.cbyjtj.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbyjtj.FormattingEnabled = true;
			this.cbyjtj.Location = new Point(91, 61);
			this.cbyjtj.Name = "cbyjtj";
			this.cbyjtj.Size = new Size(121, 20);
			this.cbyjtj.TabIndex = 4;
			this.cbyjtj.SelectedIndexChanged += new EventHandler(this.cbyjtj_SelectedIndexChanged);
			this.label6.AutoSize = true;
			this.label6.Location = new Point(29, 64);
			this.label6.Name = "label6";
			this.label6.Size = new Size(59, 12);
			this.label6.TabIndex = 11;
			this.label6.Text = "预警条件:";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(246, 65);
			this.label5.Name = "label5";
			this.label5.Size = new Size(35, 12);
			this.label5.TabIndex = 8;
			this.label5.Text = "阈值:";
			this.cbyjx.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbyjx.FormattingEnabled = true;
			this.cbyjx.Location = new Point(82, 26);
			this.cbyjx.Name = "cbyjx";
			this.cbyjx.Size = new Size(185, 20);
			this.cbyjx.TabIndex = 2;
			this.cbyjx.SelectedIndexChanged += new EventHandler(this.cbyjx_SelectedIndexChanged);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(29, 29);
			this.label4.Name = "label4";
			this.label4.Size = new Size(47, 12);
			this.label4.TabIndex = 7;
			this.label4.Text = "预警项:";
			this.groupBox2.Controls.Add(this.tbsy);
			this.groupBox2.Controls.Add(this.bsy);
			this.groupBox2.Controls.Add(this.chbfcsy);
			this.groupBox2.Controls.Add(this.chbtcck);
			this.groupBox2.ForeColor = Color.Black;
			this.groupBox2.Location = new Point(15, 174);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(465, 94);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "预警方式";
			this.tbsy.Enabled = false;
			this.tbsy.Location = new Point(104, 59);
			this.tbsy.Name = "tbsy";
			this.tbsy.Size = new Size(266, 21);
			this.tbsy.TabIndex = 3;
			this.bsy.Enabled = false;
			this.bsy.Location = new Point(379, 58);
			this.bsy.Name = "bsy";
			this.bsy.Size = new Size(39, 23);
			this.bsy.TabIndex = 2;
			this.bsy.Text = "...";
			this.bsy.UseVisualStyleBackColor = true;
			this.bsy.Click += new EventHandler(this.bsy_Click);
			this.chbfcsy.AutoSize = true;
			this.chbfcsy.Location = new Point(31, 63);
			this.chbfcsy.Name = "chbfcsy";
			this.chbfcsy.Size = new Size(72, 16);
			this.chbfcsy.TabIndex = 1;
			this.chbfcsy.Text = "发出声音";
			this.chbfcsy.UseVisualStyleBackColor = true;
			this.chbfcsy.CheckedChanged += new EventHandler(this.chbfcsy_CheckedChanged);
			this.chbtcck.AutoSize = true;
			this.chbtcck.Location = new Point(31, 30);
			this.chbtcck.Name = "chbtcck";
			this.chbtcck.Size = new Size(72, 16);
			this.chbtcck.TabIndex = 0;
			this.chbtcck.Text = "弹出窗口";
			this.chbtcck.UseVisualStyleBackColor = true;
			this.cbxdsj.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbxdsj.FormattingEnabled = true;
			this.cbxdsj.Location = new Point(57, 289);
			this.cbxdsj.Name = "cbxdsj";
			this.cbxdsj.Size = new Size(96, 20);
			this.cbxdsj.TabIndex = 7;
			this.cbxdsj.SelectedIndexChanged += new EventHandler(this.cbxdsj_SelectedIndexChanged);
			this.label7.AutoSize = true;
			this.label7.Location = new Point(32, 292);
			this.label7.Name = "label7";
			this.label7.Size = new Size(17, 12);
			this.label7.TabIndex = 6;
			this.label7.Text = "在";
			this.label8.AutoSize = true;
			this.label8.Location = new Point(172, 292);
			this.label8.Name = "label8";
			this.label8.Size = new Size(95, 12);
			this.label8.TabIndex = 8;
			this.label8.Text = "内最多重复次数:";
			this.button1.Location = new Point(164, 329);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 10;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(248, 329);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 23);
			this.button2.TabIndex = 11;
			this.button2.Text = "取消";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.OFDYJSY.FileName = "openFileDialogYJSY";
			this.OFDYJSY.Filter = "波形文件(*.wav)|*.wav";
			this.chbsfqy.AutoSize = true;
			this.chbsfqy.Checked = true;
			this.chbsfqy.CheckState = CheckState.Checked;
			this.chbsfqy.Location = new Point(386, 292);
			this.chbsfqy.Name = "chbsfqy";
			this.chbsfqy.Size = new Size(72, 16);
			this.chbsfqy.TabIndex = 13;
			this.chbsfqy.Text = "是否有效";
			this.chbsfqy.UseVisualStyleBackColor = true;
			this.nudcfcs.Location = new Point(268, 289);
			NumericUpDown arg_DA5_0 = this.nudcfcs;
			int[] array = new int[4];
			array[0] = 1;
			arg_DA5_0.Minimum = new decimal(array);
			this.nudcfcs.Name = "nudcfcs";
			this.nudcfcs.Size = new Size(102, 21);
			this.nudcfcs.TabIndex = 15;
			NumericUpDown arg_DF2_0 = this.nudcfcs;
			int[] array2 = new int[4];
			array2[0] = 1;
			arg_DF2_0.Value = new decimal(array2);
			this.nudcfcs.TextChanged += new EventHandler(this.numericUpDownQty_TextChanged);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(497, 368);
			base.Controls.Add(this.nudcfcs);
			base.Controls.Add(this.chbsfqy);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.cbxdsj);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.dtsjzd);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.dtsjqd);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cbyjlx);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			this.MaximumSize = new Size(507, 400);
			base.MinimizeBox = false;
			this.MinimumSize = new Size(507, 400);
			base.Name = "TMainYJSZ";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "设置预警";
			base.Load += new EventHandler(this.TMainYJSZ_Load);
			base.KeyDown += new KeyEventHandler(this.TMainYJSZ_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.nudcfcs).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public TMainYJSZ(TMainForm ParentForm)
		{
			try
			{
				this._ParentForm = ParentForm;
				this.InitializeComponent();
				base.Icon = Global.SystamIcon;
				this.Font = ParentForm.SysFont;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainYJSZ_Load(object sender, EventArgs e)
		{
			try
			{
				string[] array;
				if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
				{
					array = new string[]
					{
						"行情预警",
						"风险预警",
						"可用保证金预警",
						"会员持有净浮动盈亏"
					};
				}
				else
				{
					array = new string[]
					{
						"行情预警",
						"风险预警",
						"当前权益预警",
						"可用保证金预警",
						"总浮动盈亏预警"
					};
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.cbyjlx.Items.Add(array[i]);
				}
				this.cbyjlx.SelectedIndex = 0;
				string[] array2 = new string[]
				{
					"10sec",
					"30sec",
					"60sec",
					"3min",
					"5min",
					"15min",
					"30min",
					"1hour"
				};
				for (int j = 0; j < array2.Length; j++)
				{
					this.cbxdsj.Items.Add(array2[j]);
				}
				this.cbxdsj.SelectedIndex = 0;
				this.chbtcck.Checked = true;
				if (this.TYJSZFLAG)
				{
					this.TmainYJSZ_UpData();
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void cbyjlx_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.cbyjx.Items.Clear();
				this.cbyjtj.Items.Clear();
				string[] array = new string[0];
				switch (this.cbyjlx.SelectedIndex)
				{
				case 0:
					if (this._ParentForm.dataProcess.IsAgency)
					{
						using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, CommodityInfo> current = enumerator.Current;
								string commodityName = current.Value.CommodityName;
								string commodityID = current.Value.CommodityID;
								CBListItem item = new CBListItem(commodityID, commodityName);
								this.cbyjx.Items.Add(item);
							}
							goto IL_12E;
						}
					}
					foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
					{
						string commodityName = current2.Value.CommodityName;
						string commodityID = current2.Value.CommodityID;
						CBListItem item = new CBListItem(commodityID, commodityName);
						this.cbyjx.Items.Add(item);
					}
					IL_12E:
					array = new string[]
					{
						"买价>",
						"买价=",
						"买价<",
						"卖价>",
						"卖价=",
						"卖价<"
					};
					this.fxlabel.Visible = false;
					break;
				case 1:
				{
					array = new string[]
					{
						">",
						"=",
						"<"
					};
					CBListItem item = new CBListItem("0", "风险值");
					this.cbyjx.Items.Add(item);
					this.fxlabel.Visible = true;
					break;
				}
				case 2:
				{
					array = new string[]
					{
						">",
						"=",
						"<"
					};
					CBListItem item;
					if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
					{
						item = new CBListItem("1", "可用保证金预警");
					}
					else
					{
						item = new CBListItem("1", "当前权益");
					}
					this.cbyjx.Items.Add(item);
					this.fxlabel.Visible = false;
					break;
				}
				case 3:
				{
					array = new string[]
					{
						">",
						"=",
						"<"
					};
					CBListItem item;
					if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
					{
						item = new CBListItem("2", "会员持有净浮动盈亏");
					}
					else
					{
						item = new CBListItem("2", "可用保证金");
					}
					this.cbyjx.Items.Add(item);
					this.fxlabel.Visible = false;
					break;
				}
				case 4:
				{
					array = new string[]
					{
						">",
						"=",
						"<"
					};
					CBListItem item = new CBListItem("3", "总浮动盈亏");
					this.cbyjx.Items.Add(item);
					this.fxlabel.Visible = false;
					break;
				}
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.cbyjtj.Items.Add(array[i]);
				}
				this.cbyjx.SelectedIndex = 0;
				this.cbyjtj.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.YJDataWrite();
		}
		private void YJDataWrite()
		{
			try
			{
				if (Convert.ToDateTime(this.dtsjqd.Text.Trim()) > Convert.ToDateTime(this.dtsjzd.Text.Trim()))
				{
					MessageForm messageForm = new MessageForm("提示信息", "有效时间段设置有误,请确保开始时间小于终止", 1, StatusBarType.Error);
					messageForm.ShowDialog();
					messageForm.Dispose();
					return;
				}
				if (Convert.ToDateTime(this.dtsjqd.Text.Trim()) == Convert.ToDateTime(this.dtsjzd.Text.Trim()))
				{
					MessageForm messageForm2 = new MessageForm("提示信息", "有效时间段设置有误,请确保开始时间不等于终止时间", 1, StatusBarType.Error);
					messageForm2.ShowDialog();
					messageForm2.Dispose();
					return;
				}
				if (this.tbfz.Text.Trim() == "")
				{
					MessageForm messageForm3 = new MessageForm("提示信息", "请确保您的阈值不为空!", 1, StatusBarType.Error);
					messageForm3.ShowDialog();
					messageForm3.Dispose();
					return;
				}
				if (this.tbfz.Text.Trim() == "-" || this.tbfz.Text.Trim() == ".")
				{
					MessageForm messageForm4 = new MessageForm("提示信息", "请正确填写阈值!", 1, StatusBarType.Error);
					messageForm4.ShowDialog();
					messageForm4.Dispose();
					return;
				}
				if (this._ParentForm.dataProcess.sIdentity == Identity.Member)
				{
					if (this.cbyjlx.SelectedIndex != 2 && this.cbyjlx.SelectedIndex != 3 && Convert.ToDecimal(this.tbfz.Text.Trim()) < 0m)
					{
						MessageForm messageForm5 = new MessageForm("提示信息", "请确保您的阈值不为负数!", 1, StatusBarType.Error);
						messageForm5.ShowDialog();
						messageForm5.Dispose();
						return;
					}
				}
				else if (this.cbyjlx.SelectedIndex != 4 && this.cbyjlx.SelectedIndex != 3 && Convert.ToDecimal(this.tbfz.Text.Trim()) < 0m)
				{
					MessageForm messageForm6 = new MessageForm("提示信息", "请确保您的阈值不为负数!", 1, StatusBarType.Error);
					messageForm6.ShowDialog();
					messageForm6.Dispose();
					return;
				}
				if (!this.chbtcck.Checked && !this.chbfcsy.Checked)
				{
					MessageForm messageForm7 = new MessageForm("提示信息", "请选择一种预警方式!", 1, StatusBarType.Error);
					messageForm7.ShowDialog();
					messageForm7.Dispose();
					return;
				}
				if (this.chbfcsy.Checked && this.tbsy.Text.Trim() == "")
				{
					MessageForm messageForm8 = new MessageForm("提示信息", "请选择声音文件!", 1, StatusBarType.Error);
					messageForm8.ShowDialog();
					messageForm8.Dispose();
					return;
				}
				if (this.tbsy.Text.Trim() != "" && !File.Exists(this.tbsy.Text.Trim()))
				{
					MessageForm messageForm9 = new MessageForm("提示信息", "请选择有效的声音文件!", 1, StatusBarType.Error);
					messageForm9.ShowDialog();
					messageForm9.Dispose();
					return;
				}
				if (this.nudcfcs.Value > this.nudcfcs.Maximum)
				{
					MessageForm messageForm10 = new MessageForm("提示信息", "重复次数大于最大值,请您重新选择!", 1, StatusBarType.Error);
					messageForm10.ShowDialog();
					messageForm10.Dispose();
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return;
			}
			try
			{
				DateTime.Parse(this.dtsjqd.Text.ToString());
			}
			catch
			{
				MessageForm messageForm11 = new MessageForm("提示信息", "请输入正确的有效时段起始时间!", 1, StatusBarType.Error);
				messageForm11.ShowDialog();
				messageForm11.Dispose();
				return;
			}
			try
			{
				DateTime.Parse(this.dtsjzd.Text.ToString());
			}
			catch
			{
				MessageForm messageForm12 = new MessageForm("提示信息", "请输入正确的有效时段终止时间!", 1, StatusBarType.Error);
				messageForm12.ShowDialog();
				messageForm12.Dispose();
				return;
			}
			try
			{
				string text = this.dtsjqd.Text.Trim();
				string text2 = this.dtsjzd.Text.Trim();
				string[] columns = new string[]
				{
					"YJLX",
					"YJX",
					"YJTJ",
					"YJFZ",
					"YJFS",
					"SYDZ",
					"ZJCFSJ",
					"YJYXSD",
					"SFYX",
					"XDSJD",
					"CFCS"
				};
				string text3;
				if (this.chbtcck.Checked)
				{
					text3 = "0";
				}
				else
				{
					text3 = "1";
				}
				if (this.chbfcsy.Checked)
				{
					text3 += "0";
				}
				else
				{
					text3 += "1";
				}
				if (text.Length == 7)
				{
					text = '0' + text;
				}
				if (text2.Length == 7)
				{
					text2 = '0' + text2;
				}
				string text4;
				if (this.chbsfqy.Checked)
				{
					text4 = "Y";
				}
				else
				{
					text4 = "N";
				}
				CBListItem cBListItem = (CBListItem)this.cbyjx.Items[this.cbyjx.SelectedIndex];
				Environment.CurrentDirectory = Global.CurrentDirectory;
				string[] columnValue = new string[]
				{
					this.cbyjlx.SelectedIndex.ToString(),
					cBListItem.Key.ToString(),
					this.cbyjtj.SelectedIndex.ToString(),
					Convert.ToString(Convert.ToDecimal(this.tbfz.Text.Trim())),
					text3,
					this.tbsy.Text.Trim(),
					"",
					text + "--" + text2,
					text4,
					this.cbxdsj.SelectedIndex.ToString(),
					this.nudcfcs.Value.ToString()
				};
				TMainYJSZ._CreateXml = new CreateXmlFile();
				TMainYJSZ._CreateXml.CreateFile(this.YJFileName);
				if (!this.TYJSZFLAG)
				{
					string text5 = TMainYJSZ._CreateXml.WriteXmlData(columns, columnValue);
					if (text5.Equals("true"))
					{
						base.Close();
					}
					else
					{
						MessageForm messageForm13 = new MessageForm("提示信息", "新建预警信息失败!", 1, StatusBarType.Error);
						messageForm13.ShowDialog();
						messageForm13.Dispose();
					}
				}
				else if (TMainYJSZ._CreateXml.UpdateXmlData(columns, columnValue, "ID", this.TYJSZHH.ToString()))
				{
					base.Close();
				}
				else
				{
					MessageForm messageForm14 = new MessageForm("提示信息", "更新预警信息失败!", 1, StatusBarType.Error);
					messageForm14.ShowDialog();
					messageForm14.Dispose();
				}
			}
			catch (Exception ex2)
			{
				Logger.wirte(ex2);
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				e.Handled = (e.KeyChar < '0' || e.KeyChar > '9');
				if (e.KeyChar == '\b')
				{
					e.Handled = false;
				}
				else if (e.KeyChar == '-')
				{
					if (((TextBox)sender).SelectionStart == 0 && (((TextBox)sender).Text.IndexOf('-') < 0 || ((TextBox)sender).SelectedText.IndexOf("-") >= 0))
					{
						e.Handled = false;
					}
					else
					{
						e.Handled = true;
					}
				}
				else if (e.KeyChar == '.')
				{
					if (this.tbfz.Text.Trim() == "")
					{
						e.Handled = true;
					}
					else
					{
						string text = this.tbfz.Text.Trim();
						for (int i = 0; i < text.Length; i++)
						{
							char c = text[i];
							if (c != '-' && char.IsPunctuation(c))
							{
								e.Handled = true;
								return;
							}
						}
						e.Handled = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void chbfcsy_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chbfcsy.Checked)
			{
				this.tbsy.Enabled = true;
				this.bsy.Enabled = true;
				return;
			}
			this.tbsy.Enabled = false;
			this.bsy.Enabled = false;
			this.tbsy.Text = "";
		}
		private void bsy_Click(object sender, EventArgs e)
		{
			if (this.OFDYJSY.ShowDialog() == DialogResult.OK)
			{
				this.tbsy.Text = this.OFDYJSY.FileName;
			}
		}
		private void TmainYJSZ_UpData()
		{
			try
			{
				TMainYJSZ._CreateXml = new CreateXmlFile();
				TMainYJSZ._CreateXml.CreateFile(this.YJFileName);
				DataTable dataByXml = TMainYJSZ._CreateXml.GetDataByXml("ID= '" + this.TYJSZHH + "'", "");
				this.cbyjlx.SelectedIndex = (int)Convert.ToInt16(dataByXml.Rows[0]["YJLX"]);
				this.dtsjqd.Value = Convert.ToDateTime(dataByXml.Rows[0]["YJYXSD"].ToString().Substring(0, 8));
				this.dtsjzd.Value = Convert.ToDateTime(dataByXml.Rows[0]["YJYXSD"].ToString().Substring(10, 8));
				int num = 0;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					using (Dictionary<string, CommodityInfo>.Enumerator enumerator = Global.AgencyCommodityData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, CommodityInfo> current = enumerator.Current;
							if (current.Value.CommodityID == dataByXml.Rows[0]["YJX"].ToString())
							{
								this.cbyjx.SelectedIndex = num;
								break;
							}
							num++;
						}
						goto IL_1C1;
					}
				}
				foreach (KeyValuePair<string, CommodityInfo> current2 in Global.CommodityData)
				{
					if (current2.Value.CommodityID == dataByXml.Rows[0]["YJX"].ToString())
					{
						this.cbyjx.SelectedIndex = num;
						break;
					}
					num++;
				}
				IL_1C1:
				this.cbyjtj.SelectedIndex = (int)Convert.ToInt16(dataByXml.Rows[0]["YJTJ"]);
				this.tbfz.Text = dataByXml.Rows[0]["YJFZ"].ToString();
				if (dataByXml.Rows[0]["YJFS"].ToString().Substring(0, 1) == "0")
				{
					this.chbtcck.Checked = true;
				}
				else
				{
					this.chbtcck.Checked = false;
				}
				if (dataByXml.Rows[0]["YJFS"].ToString().Substring(1, 1) == "0")
				{
					this.chbfcsy.Checked = true;
					this.tbsy.Text = dataByXml.Rows[0]["SYDZ"].ToString();
				}
				if (dataByXml.Rows[0]["SFYX"].ToString() == "Y")
				{
					this.chbsfqy.Checked = true;
				}
				else
				{
					this.chbsfqy.Checked = false;
				}
				this.cbxdsj.SelectedIndex = (int)Convert.ToInt16(dataByXml.Rows[0]["XDSJD"]);
				this.nudcfcs.Value = Convert.ToInt16(dataByXml.Rows[0]["CFCS"]);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void cbyjx_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData == null)
						{
							return;
						}
						goto IL_56;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData == null)
					{
						return;
					}
				}
				IL_56:
				Dictionary<string, CommData> dictionary = null;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData != null)
						{
							dictionary = new Dictionary<string, CommData>(Global.AgencyHQCommData);
						}
						goto IL_BC;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData != null)
					{
						dictionary = new Dictionary<string, CommData>(Global.HQCommData);
					}
				}
				IL_BC:
				if (this.cbyjlx.SelectedIndex == 0)
				{
					CBListItem cBListItem = (CBListItem)this.cbyjx.Items[this.cbyjx.SelectedIndex];
					this.tbfz.Text = dictionary[cBListItem.Key.ToString()].BuyPrice.ToString();
				}
				else
				{
					this.tbfz.Text = "";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void cbyjtj_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._ParentForm.dataProcess.IsAgency)
				{
					if (Global.AgencyHQCommData == null)
					{
						return;
					}
				}
				else if (Global.HQCommData == null)
				{
					return;
				}
				Dictionary<string, CommData> dictionary = null;
				if (this._ParentForm.dataProcess.IsAgency)
				{
					lock (Global.AgencyHQCommDataLock)
					{
						if (Global.AgencyHQCommData != null)
						{
							dictionary = Global.gAgencyHQCommData;
						}
						goto IL_86;
					}
				}
				lock (Global.HQCommDataLock)
				{
					if (Global.HQCommData != null)
					{
						dictionary = Global.gHQCommData;
					}
				}
				IL_86:
				if (this.cbyjlx.SelectedIndex == 0)
				{
					if (this.cbyjtj.SelectedIndex < 3)
					{
						CBListItem cBListItem = (CBListItem)this.cbyjx.Items[this.cbyjx.SelectedIndex];
						this.tbfz.Text = dictionary[cBListItem.Key.ToString()].BuyPrice.ToString();
					}
					else
					{
						CBListItem cBListItem2 = (CBListItem)this.cbyjx.Items[this.cbyjx.SelectedIndex];
						this.tbfz.Text = dictionary[cBListItem2.Key.ToString()].SellPrice.ToString();
					}
				}
				else
				{
					this.tbfz.Text = "";
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void tbfz_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.cbyjlx.SelectedIndex == 0 && this.cbyjx.SelectedIndex == 1 && this.tbfz.Text.IndexOf('.') >= 0)
				{
					MessageForm messageForm = new MessageForm("错误", "只能填入整数!", 1, StatusBarType.Message);
					messageForm.Owner = this;
					messageForm.ShowDialog();
					messageForm.Dispose();
					this.tbfz.Text = this.tbfz.Text.Substring(0, this.tbfz.Text.IndexOf("."));
				}
				if (this.tbfz.Text.Contains("."))
				{
					int num = this.tbfz.Text.IndexOf(".") + 1;
					if (this.tbfz.Text.Substring(num, this.tbfz.Text.Trim().Length - num).Length > 2)
					{
						MessageForm messageForm2 = new MessageForm("错误", "最多只能填入两位小数!", 1, StatusBarType.Message);
						messageForm2.Owner = this;
						messageForm2.ShowDialog();
						messageForm2.Dispose();
						this.tbfz.Text = this.tbfz.Text.Substring(0, this.tbfz.Text.IndexOf('.') + 3);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private void TMainYJSZ_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode != Keys.Escape)
			{
				return;
			}
			base.Close();
		}
		private void cbxdsj_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.cbxdsj.SelectedIndex)
			{
			case 0:
				this.nudcfcs.Maximum = 10m;
				return;
			case 1:
				this.nudcfcs.Maximum = 30m;
				return;
			case 2:
				this.nudcfcs.Maximum = 60m;
				return;
			case 3:
				this.nudcfcs.Maximum = 180m;
				return;
			case 4:
				this.nudcfcs.Maximum = 300m;
				return;
			case 5:
				this.nudcfcs.Maximum = 900m;
				return;
			case 6:
				this.nudcfcs.Maximum = 1800m;
				return;
			case 7:
				this.nudcfcs.Maximum = 3600m;
				return;
			default:
				return;
			}
		}
		private void numericUpDownQty_TextChanged(object sender, EventArgs e)
		{
			int value = 0;
			try
			{
				if (this.nudcfcs.Text.Trim().Length == 0)
				{
					return;
				}
				value = Convert.ToInt32(this.nudcfcs.Text);
			}
			catch (Exception)
			{
				MessageForm messageForm = new MessageForm("提示", "请输入数值类型", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
				this.nudcfcs.Text = "";
				return;
			}
			if (value > this.nudcfcs.Maximum || value < this.nudcfcs.Minimum)
			{
				MessageForm messageForm = new MessageForm("提示", "数值超出范围！", 1, StatusBarType.Warning);
				messageForm.ShowDialog();
			}
		}
	}
}
