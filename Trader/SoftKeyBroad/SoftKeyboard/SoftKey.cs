using SoftKeyboard.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToolsLibrary;
namespace SoftKeyboard
{
	public class SoftKey : Form
	{
		private IContainer components;
		private Button btnShift;
		private Button btnCaps;
		private Panel palLetter;
		private Panel palNumber;
		private Button btnDel;
		private Panel palSign;
		private PictureBox picClose;
		private PasswordTextBox password;
		private List<Button> btnNumbers;
		private List<Button> btnSigns;
		private List<Button> btnLetters;
		private string[] strSigns;
		private string[] strShiftSigns;
		private string[] strNumbers;
		private string[] strShiftNumbers;
		private string[] strLetters;
		private int rNumber;
		private int rSign;
		private int rLetter;
		private bool isShift;
		private bool isCaps;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SoftKey));
			this.btnShift = new Button();
			this.btnCaps = new Button();
			this.palLetter = new Panel();
			this.palSign = new Panel();
			this.palNumber = new Panel();
			this.btnDel = new Button();
			this.picClose = new PictureBox();
			this.palSign.SuspendLayout();
			this.palNumber.SuspendLayout();
			((ISupportInitialize)this.picClose).BeginInit();
			base.SuspendLayout();
			this.btnShift.BackgroundImage = (Image)componentResourceManager.GetObject("btnShift.BackgroundImage");
			this.btnShift.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.btnShift.ForeColor = Color.Maroon;
			this.btnShift.Location = new Point(3, 0);
			this.btnShift.Name = "btnShift";
			this.btnShift.Size = new Size(48, 23);
			this.btnShift.TabIndex = 0;
			this.btnShift.Text = "Shift";
			this.btnShift.UseVisualStyleBackColor = true;
			this.btnShift.Click += new EventHandler(this.btnShift_Click);
			this.btnCaps.BackColor = Color.Transparent;
			this.btnCaps.BackgroundImage = (Image)componentResourceManager.GetObject("btnCaps.BackgroundImage");
			this.btnCaps.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.btnCaps.ForeColor = Color.Maroon;
			this.btnCaps.Location = new Point(293, 0);
			this.btnCaps.Name = "btnCaps";
			this.btnCaps.Size = new Size(43, 23);
			this.btnCaps.TabIndex = 1;
			this.btnCaps.Text = "Caps";
			this.btnCaps.UseVisualStyleBackColor = false;
			this.btnCaps.Click += new EventHandler(this.btnCaps_Click);
			this.palLetter.BackColor = Color.Transparent;
			this.palLetter.Location = new Point(1, 59);
			this.palLetter.Name = "palLetter";
			this.palLetter.Size = new Size(335, 50);
			this.palLetter.TabIndex = 3;
			this.palSign.BackColor = Color.Transparent;
			this.palSign.Controls.Add(this.btnShift);
			this.palSign.Controls.Add(this.btnCaps);
			this.palSign.Location = new Point(1, 33);
			this.palSign.Name = "palSign";
			this.palSign.Size = new Size(335, 25);
			this.palSign.TabIndex = 3;
			this.palNumber.BackColor = Color.Transparent;
			this.palNumber.Controls.Add(this.btnDel);
			this.palNumber.Location = new Point(1, 8);
			this.palNumber.Name = "palNumber";
			this.palNumber.Size = new Size(328, 25);
			this.palNumber.TabIndex = 3;
			this.btnDel.BackgroundImage = (Image)componentResourceManager.GetObject("btnDel.BackgroundImage");
			this.btnDel.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.btnDel.ForeColor = Color.Maroon;
			this.btnDel.Location = new Point(282, 0);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new Size(43, 23);
			this.btnDel.TabIndex = 0;
			this.btnDel.Text = "←";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new EventHandler(this.btnDel_Click);
			this.picClose.BackColor = Color.Transparent;
			this.picClose.Image = Resources.close2;
			this.picClose.Location = new Point(326, 4);
			this.picClose.Name = "picClose";
			this.picClose.Size = new Size(11, 15);
			this.picClose.TabIndex = 1;
			this.picClose.TabStop = false;
			this.picClose.MouseLeave += new EventHandler(this.picClose_MouseLeave);
			this.picClose.Click += new EventHandler(this.picClose_Click);
			this.picClose.MouseHover += new EventHandler(this.picClose_MouseHover);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackgroundImage = Resources.bgimage;
			base.ClientSize = new Size(341, 114);
			base.Controls.Add(this.picClose);
			base.Controls.Add(this.palNumber);
			base.Controls.Add(this.palSign);
			base.Controls.Add(this.palLetter);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "SoftKey";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.Deactivate += new EventHandler(this.SoftKey_Deactivate);
			base.Load += new EventHandler(this.SoftKey_Load);
			this.palSign.ResumeLayout(false);
			this.palNumber.ResumeLayout(false);
			((ISupportInitialize)this.picClose).EndInit();
			base.ResumeLayout(false);
		}
		public SoftKey(PasswordTextBox password)
		{
			this.InitializeComponent();
			this.password = password;
			this.Inlit();
		}
		private void SoftKey_Load(object sender, EventArgs e)
		{
			this.SoftKeyLocation();
			this.InitButtonNumbers(e);
			this.InitButtonSigns(e);
			this.InitButtonLetters(e);
		}
		public void SoftKeyLocation()
		{
			int num = this.password.Location.X;
			int num2 = this.password.Location.Y + this.password.Height;
			for (Control parent = this.password.Parent; parent != null; parent = parent.Parent)
			{
				num += parent.Location.X;
				num2 += parent.Location.Y;
				if (parent.Parent == null)
				{
					num += parent.Width - parent.ClientSize.Width;
					num2 += parent.Height - parent.ClientSize.Height;
				}
			}
			base.Location = new Point(num, num2);
		}
		private void Inlit()
		{
			int num = 0;
			this.strNumbers = new string[]
			{
				"`",
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"0"
			};
			this.strShiftNumbers = new string[]
			{
				"~",
				"!",
				"@",
				"#",
				"$",
				"%",
				"^",
				"&&",
				"*",
				"(",
				")"
			};
			this.strSigns = new string[]
			{
				"'",
				",",
				".",
				"/",
				"-",
				"=",
				"\\",
				"[",
				"]",
				";"
			};
			this.strShiftSigns = new string[]
			{
				" \" ",
				"<",
				">",
				"?",
				"_",
				"+",
				"|",
				"{",
				"}",
				":"
			};
			this.strLetters = new string[26];
			char c = 'a';
			while (c <= 'z')
			{
				this.strLetters[num] = c.ToString();
				c += '\u0001';
				num++;
			}
		}
		public void InitButtonNumbers(EventArgs e)
		{
			Random random = new Random();
			this.rNumber = random.Next(10);
			int num = this.rNumber;
			this.btnNumbers = new List<Button>(11);
			for (int i = 0; i < 11; i++)
			{
				this.btnNumbers.Add(new Button());
				this.btnNumbers[i].Width = 24;
				this.btnNumbers[i].Height = this.btnDel.Height;
				this.btnNumbers[i].Top = this.btnDel.Top;
				this.btnNumbers[i].Text = this.strNumbers[++num];
				this.btnNumbers[i].BackgroundImage = Resources.button;
				this.btnNumbers[i].BackColor = Color.Transparent;
				this.btnNumbers[i].Click += new EventHandler(this.btns_Click);
				if (num == this.strNumbers.Length - 1)
				{
					num = -1;
				}
				if (i == 0)
				{
					this.btnNumbers[i].Left = this.palNumber.Left + 15;
				}
				else
				{
					this.btnNumbers[i].Left = this.btnNumbers[i - 1].Right;
				}
				this.palNumber.Controls.Add(this.btnNumbers[i]);
			}
			this.ChaneColorOfButton(this);
		}
		public void InitButtonSigns(EventArgs e)
		{
			Random random = new Random();
			this.rSign = random.Next(9);
			int num = this.rSign;
			this.btnSigns = new List<Button>(10);
			for (int i = 0; i < 10; i++)
			{
				this.btnSigns.Add(new Button());
				this.btnSigns[i].Width = 24;
				this.btnSigns[i].Height = this.btnShift.Height;
				this.btnSigns[i].Text = this.strSigns[++num];
				this.btnSigns[i].BackgroundImage = Resources.button;
				this.btnSigns[i].BackColor = Color.Transparent;
				this.btnSigns[i].Click += new EventHandler(this.btns_Click);
				if (num == this.strSigns.Length - 1)
				{
					num = -1;
				}
				if (i == 0)
				{
					this.btnSigns[i].Left = this.btnShift.Right;
				}
				else
				{
					this.btnSigns[i].Left = this.btnSigns[i - 1].Right;
				}
				this.palSign.Controls.Add(this.btnSigns[i]);
			}
			this.ChaneColorOfButton(this);
		}
		public void InitButtonLetters(EventArgs e)
		{
			Random random = new Random();
			this.rLetter = random.Next(25);
			int num = this.rLetter;
			this.btnLetters = new List<Button>(26);
			for (int i = 0; i < 26; i++)
			{
				this.btnLetters.Add(new Button());
				this.btnLetters[i].Width = 24;
				this.btnLetters[i].Text = this.strLetters[++num];
				this.btnLetters[i].BackgroundImage = Resources.button;
				this.btnLetters[i].BackColor = Color.Transparent;
				this.btnLetters[i].Click += new EventHandler(this.btns_Click);
				if (num == this.strLetters.Length - 1)
				{
					num = -1;
				}
				if (i == 0)
				{
					this.btnLetters[i].Left = this.palLetter.Left + 15;
				}
				else
				{
					if (i == 13)
					{
						this.btnLetters[i].Top = this.btnLetters[0].Top + this.btnShift.Height;
						this.btnLetters[i].Left = this.palLetter.Left + 15;
					}
					else
					{
						if (i > 13)
						{
							this.btnLetters[i].Top = this.btnLetters[0].Top + this.btnShift.Height;
							this.btnLetters[i].Left = this.btnLetters[i - 1].Right;
						}
						else
						{
							this.btnLetters[i].Left = this.btnLetters[i - 1].Right;
						}
					}
				}
				this.palLetter.Controls.Add(this.btnLetters[i]);
			}
			this.ChaneColorOfButton(this);
		}
		private void changeNumberText()
		{
			int num = this.rNumber;
			string[] array;
			if (this.isShift)
			{
				array = this.strNumbers;
			}
			else
			{
				array = this.strShiftNumbers;
			}
			for (int i = 0; i < 11; i++)
			{
				this.btnNumbers[i].Text = array[++num];
				if (num == array.Length - 1)
				{
					num = -1;
				}
			}
		}
		private void changeSignText()
		{
			int num = this.rSign;
			string[] array;
			if (this.isShift)
			{
				array = this.strSigns;
			}
			else
			{
				array = this.strShiftSigns;
			}
			for (int i = 0; i < 10; i++)
			{
				this.btnSigns[i].Text = array[++num];
				if (num == array.Length - 1)
				{
					num = -1;
				}
			}
		}
		private void changeLetterText()
		{
			if ((this.isCaps & !this.isShift) || (this.isShift & !this.isCaps))
			{
				using (List<Button>.Enumerator enumerator = this.btnLetters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Button current = enumerator.Current;
						current.Text = current.Text.ToString().ToLower();
					}
					return;
				}
			}
			if ((this.isCaps & this.isShift) || (!this.isCaps & !this.isShift))
			{
				foreach (Button current2 in this.btnLetters)
				{
					current2.Text = current2.Text.ToString().ToUpper();
				}
			}
		}
		private void btnShift_Click(object sender, EventArgs e)
		{
			this.changeNumberText();
			this.changeSignText();
			this.changeLetterText();
			if (this.isShift)
			{
				this.isShift = false;
				this.btnShift.FlatStyle = FlatStyle.Standard;
				return;
			}
			this.isShift = true;
			this.btnShift.FlatStyle = FlatStyle.Popup;
		}
		private void btnCaps_Click(object sender, EventArgs e)
		{
			this.changeLetterText();
			if (this.isCaps)
			{
				this.isCaps = false;
				this.btnCaps.FlatStyle = FlatStyle.Standard;
				return;
			}
			this.isCaps = true;
			this.btnCaps.FlatStyle = FlatStyle.Popup;
		}
		private void btnDel_Click(object sender, EventArgs e)
		{
			this.password.CheckPass=true;
			if (this.password.Text.Length > 0)
			{
				this.password.Text = this.password.Text.ToString().Substring(0, this.password.Text.ToString().Length - 1);
			}
			this.password.CheckPass=false;
		}
		private void btns_Click(object sender, EventArgs e)
		{
			this.password.CheckPass=true;
			PasswordTextBox expr_12 = this.password;
			expr_12.Text += ((Button)sender).Text[0].ToString();
			if (this.isShift)
			{
				this.isShift = false;
				this.btnShift.FlatStyle = FlatStyle.Standard;
			}
			this.password.CheckPass=false;
		}
		public void ChaneColorOfButton(Control control)
		{
			foreach (Control control2 in control.Controls)
			{
				if (control2.HasChildren)
				{
					this.ChaneColorOfButton(control2);
				}
				if (control2.GetType() == typeof(Button))
				{
					control2.MouseEnter += new EventHandler(this.Son_MouseEnter);
					control2.MouseLeave += new EventHandler(this.con_MouseLeave);
				}
			}
		}
		public void con_MouseLeave(object sender, EventArgs e)
		{
			((Button)sender).BackgroundImage = Resources.button;
		}
		public void Son_MouseEnter(object sender, EventArgs e)
		{
			((Button)sender).BackgroundImage = Resources.onfocus;
		}
		private void SoftKey_Deactivate(object sender, EventArgs e)
		{
			base.Hide();
		}
		private void picClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}
		private void picClose_MouseHover(object sender, EventArgs e)
		{
			this.picClose.Image = Resources.close1;
		}
		private void picClose_MouseLeave(object sender, EventArgs e)
		{
			this.picClose.Image = Resources.close2;
		}
	}
}
