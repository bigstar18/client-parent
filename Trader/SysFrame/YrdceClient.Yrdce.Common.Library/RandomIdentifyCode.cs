using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace YrdceClient.Yrdce.Common.Library
{
	public class RandomIdentifyCode
	{
		private int identifyType;
		private int length = 4;
		private string randomCode = string.Empty;
		private Bitmap RandomCodeMap;
		public string RandomCode
		{
			get
			{
				return this.randomCode;
			}
			set
			{
				this.randomCode = value;
			}
		}
		public RandomIdentifyCode(int _identifyType, int _length)
		{
			this.identifyType = _identifyType;
			this.length = _length;
		}
		public Bitmap CreateIdentifyCode()
		{
			switch (this.identifyType)
			{
			case 0:
				this.RandomNumAndLetter(this.length);
				break;
			case 1:
				this.RandomNum(this.length);
				break;
			case 2:
				this.RandomLetter(this.length);
				break;
			default:
				this.RandomNumAndLetter(this.length);
				break;
			}
			this.RandomSortCode();
			this.CreateImage(this.randomCode);
			return this.RandomCodeMap;
		}
		private void RandomNumAndLetter(int length)
		{
			this.randomCode = string.Empty;
			bool flag = false;
			bool flag2 = false;
			char[] array = new char[length];
			Random random = new Random();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = (byte)random.Next(62);
				if (b < 10)
				{
					flag = true;
					array[i] = (char)(b + 48);
				}
				else if (b < 36)
				{
					flag2 = true;
					array[i] = (char)(b + 55);
				}
				else
				{
					flag2 = true;
					array[i] = (char)(61 + b);
				}
				if (i == array.Length - 1 && !flag.ToString().EndsWith(flag2.ToString()))
				{
					i--;
				}
			}
			this.randomCode = new string(array);
		}
		private void RandomNum(int length)
		{
			this.randomCode = string.Empty;
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				this.randomCode += random.Next(10);
			}
		}
		private void RandomLetter(int length)
		{
			this.randomCode = string.Empty;
			char[] array = new char[length];
			Random random = new Random();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = (byte)random.Next(62);
				if (b < 10)
				{
					i--;
				}
				else if (b < 36)
				{
					array[i] = (char)(b + 55);
				}
				else
				{
					array[i] = (char)(61 + b);
				}
			}
			this.randomCode = new string(array);
		}
		private void RandomSortCode()
		{
			string text = this.randomCode;
			this.randomCode = string.Empty;
			while (text.Length > 0)
			{
				Random random = new Random();
				int num = random.Next(text.Length - 1);
				this.randomCode += text[num].ToString();
				text = text.Remove(num, 1);
			}
		}
		private void CreateImage(string randomCode)
		{
			try
			{
				int num = 45;
				int width = randomCode.Length * 17;
				Bitmap bitmap = new Bitmap(width, 28);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.AliceBlue);
				graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Random random = new Random();
				Pen pen = new Pen(Color.LightGray, 0f);
				for (int i = 0; i < 50; i++)
				{
					int x = random.Next(1, bitmap.Width - 2);
					int y = random.Next(1, bitmap.Height - 2);
					graphics.DrawRectangle(pen, x, y, 1, 1);
				}
				char[] array = randomCode.ToCharArray();
				StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip);
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				Color[] array2 = new Color[]
				{
					Color.Black,
					Color.Red,
					Color.DarkBlue,
					Color.Green,
					Color.Orange,
					Color.Brown,
					Color.DarkCyan,
					Color.Purple
				};
				string[] array3 = new string[]
				{
					"Verdana",
					"Microsoft Sans Serif",
					"Comic Sans MS",
					"Arial",
					"宋体"
				};
				for (int j = 0; j < array.Length; j++)
				{
					int num2 = random.Next(7);
					int num3 = random.Next(5);
					Font font = new Font(array3[num3], 13f, FontStyle.Bold);
					Brush brush = new SolidBrush(array2[num2]);
					Point point = new Point(12, 12);
					float num4 = (float)random.Next(-num, num);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(num4);
					graphics.DrawString(array[j].ToString(), font, brush, 1f, 1f, stringFormat);
					graphics.RotateTransform(-num4);
					graphics.TranslateTransform(2f, (float)(-(float)point.Y));
				}
				this.RandomCodeMap = bitmap;
			}
			catch (ArgumentException)
			{
				string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_LoginForm_createImgeError");
				MessageBox.Show(@string);
			}
		}
	}
}
