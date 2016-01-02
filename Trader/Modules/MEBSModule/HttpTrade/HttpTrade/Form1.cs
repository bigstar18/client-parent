using HttpTrade.Gnnt.MEBS.Lib;
using HttpTrade.Gnnt.MEBS.VO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using TradeInterface.Gnnt.DataVO;
namespace HttpTrade
{
	public class Form1 : Form
	{
		private IContainer components;
		private Button button1;
		private Button button2;
		private Button button3;
		private TextBox textBox1;
		public Form1()
		{
			this.InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			FirmInfoRepVO sourceObj = new FirmInfoRepVO();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load("c:/Yrdce.xml");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("GNNT").SelectSingleNode("REP");
			XmlElement arg_33_0 = (XmlElement)xmlNode;
			this.setValue(sourceObj, xmlNode);
		}
		private void setValue(object sourceObj, XmlNode xn)
		{
			if (xn == null)
			{
				return;
			}
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			FieldInfo[] fields = sourceObj.GetType().GetFields(bindingAttr);
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (fieldInfo.FieldType.Name.Equals("String"))
				{
					string name = fieldInfo.Name;
					if (xn.SelectSingleNode(name) != null)
					{
						fieldInfo.SetValue(sourceObj, xn.SelectSingleNode(name).InnerText);
					}
				}
				else if (fieldInfo.FieldType.IsArray)
				{
					XmlNodeList xmlNodeList = xn.SelectNodes(fieldInfo.Name);
					if (xmlNodeList == null)
					{
						break;
					}
					Type elementType = fieldInfo.FieldType.GetElementType();
					ArrayList arrayList = new ArrayList();
					for (int j = 0; j < xmlNodeList.Count; j++)
					{
						object obj = Activator.CreateInstance(elementType);
						this.setValue(obj, xmlNodeList.Item(j));
						arrayList.Add(obj);
					}
					fieldInfo.SetValue(sourceObj, arrayList.ToArray(elementType));
				}
				else if (fieldInfo.FieldType.IsGenericType)
				{
					XmlNodeList xmlNodeList2 = xn.SelectNodes(fieldInfo.Name);
					if (xmlNodeList2 == null)
					{
						break;
					}
					Type fieldType = fieldInfo.FieldType;
					Type type = fieldInfo.FieldType.GetGenericArguments()[0];
					object obj2 = Activator.CreateInstance(fieldInfo.FieldType);
					for (int k = 0; k < xmlNodeList2.Count; k++)
					{
						object obj3 = Activator.CreateInstance(type);
						this.setValue(obj3, xmlNodeList2.Item(k));
						MethodInfo method = fieldType.GetMethod("Add", new Type[]
						{
							type
						});
						method.Invoke(obj2, new object[]
						{
							obj3
						});
					}
					fieldInfo.SetValue(sourceObj, obj2);
				}
				else if (fieldInfo.FieldType.IsClass)
				{
					XmlNodeList xmlNodeList3 = xn.SelectNodes(fieldInfo.Name);
					if (xmlNodeList3 == null)
					{
						break;
					}
					object obj4 = Activator.CreateInstance(fieldInfo.FieldType);
					this.setValue(obj4, xmlNodeList3.Item(0));
					fieldInfo.SetValue(sourceObj, obj4);
				}
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			tradeLibrary.CommunicationUrl = "http://222.178.120.42:16953/tradeweb/httpXmlServlet";
			tradeLibrary.Initialize();
			LogonRequestVO logonRequestVO = new LogonRequestVO();
			logonRequestVO.UserID = "2000";
			logonRequestVO.Password = "111111";
			Console.WriteLine("StartTime=" + DateTime.Now);
			for (int i = 0; i < 500; i++)
			{
				OrderRequestVO orderRequestVO = new OrderRequestVO();
				orderRequestVO.UserID = logonRequestVO.UserID;
				orderRequestVO.CustomerID = logonRequestVO.UserID + "00";
				new Random((int)DateTime.Now.Ticks + i);
				orderRequestVO.BuySell = 2;
				orderRequestVO.CommodityID = "LGC11";
				new Random((int)DateTime.Now.Ticks + i);
				int num = 2600;
				orderRequestVO.Price = (double)num;
				orderRequestVO.Quantity = 1L;
				orderRequestVO.SettleBasis = 1;
				orderRequestVO.CloseMode = 1;
				tradeLibrary.Order(orderRequestVO);
			}
			Console.WriteLine("EndTime=" + DateTime.Now);
		}
		private void button3_Click(object sender, EventArgs e)
		{
			new TradeLibrary();
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
			this.button1 = new Button();
			this.button2 = new Button();
			this.button3 = new Button();
			this.textBox1 = new TextBox();
			base.SuspendLayout();
			this.button1.Location = new Point(104, 144);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(90, 188);
			this.button2.Name = "button2";
			this.button2.Size = new Size(109, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "下单性能测试";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.button3.Location = new Point(140, 10);
			this.button3.Name = "button3";
			this.button3.Size = new Size(104, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "密码强度测试";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.textBox1.Location = new Point(22, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(100, 21);
			this.textBox1.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(292, 273);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Name = "Form1";
			this.Text = "Form1";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
