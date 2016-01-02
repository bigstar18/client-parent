using HttpTrade.Gnnt.OTC.Lib;
using HttpTrade.Gnnt.OTC.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using TradeInterface.Gnnt.OTC.DataVO;
namespace HttpTrade
{
	public class Form1 : Form
	{
		private const string CommunicationUrl = "http://localhost/PMESHost/DoRequest.aspx";
		private IContainer components;
		private Button button1;
		private Button button2;
		private Button button3;
		private TextBox textBox1;
		private Button button4;
		private Button button5;
		private Button button6;
		private DataGridView dataGridView1;
		private Button button7;
		public Form1()
		{
			this.InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			FirmInfoRepVO sourceObj = new FirmInfoRepVO();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load("c:/gnnt.xml");
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
			for (int i = 0; i < 500; i++)
			{
				OrderRequestVO orderRequestVO = new OrderRequestVO();
				orderRequestVO.UserID = logonRequestVO.UserID;
				new Random((int)DateTime.Now.Ticks + i);
				orderRequestVO.BuySell = 2;
				orderRequestVO.CommodityID = "LGC11";
				new Random((int)DateTime.Now.Ticks + i);
				int num = 2600;
				orderRequestVO.Price = (double)num;
				tradeLibrary.Order(orderRequestVO);
			}
		}
		private void button3_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			MessageBox.Show(string.Concat(tradeLibrary.passwordScore(this.textBox1.Text)));
		}
		private void button4_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			tradeLibrary.CommunicationUrl = "http://localhost/PMESHost/DoRequest.aspx";
			tradeLibrary.Initialize();
			tradeLibrary.Logon(new LogonRequestVO
			{
				UserID = "2000",
				Password = "111111"
			});
		}
		private void button5_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			tradeLibrary.CommunicationUrl = "http://localhost/PMESHost/DoRequest.aspx";
			tradeLibrary.Initialize();
			tradeLibrary.CommDataQuery(new CommDataQueryRequestVO
			{
				CommodityID = string.Empty,
				MarketID = "1",
				UserID = "2000"
			});
		}
		private void button6_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			tradeLibrary.CommunicationUrl = "http://localhost/PMESHost/DoRequest.aspx";
			tradeLibrary.Initialize();
			HoldingDetailResponseVO holdingDetailResponseVO = tradeLibrary.HoldPtByPrice(new HoldingDetailRequestVO
			{
				CommodityID = string.Empty,
				MarketID = "1",
				UserID = "2000",
				IsDesc = false,
				RecordCount = 100,
				SortField = "OR_T",
				StartNum = 0
			});
			DataTable propertiDataTable = this.GetPropertiDataTable<HoldingDetailInfo>(holdingDetailResponseVO.HoldingDetailInfoList);
			this.dataGridView1.DataSource = propertiDataTable.DefaultView;
		}
		private ArrayList GetPropertieName(object t)
		{
			ArrayList arrayList = new ArrayList();
			if (t == null)
			{
				return arrayList;
			}
			FieldInfo[] fields = t.GetType().GetFields();
			if (fields.Length <= 0)
			{
				return arrayList;
			}
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				string name = fieldInfo.Name;
				arrayList.Add(name);
			}
			return arrayList;
		}
		private Hashtable GetPropertieNameHash(object t)
		{
			Hashtable hashtable = new Hashtable();
			string arg_0B_0 = string.Empty;
			if (t == null)
			{
				return hashtable;
			}
			FieldInfo[] fields = t.GetType().GetFields();
			if (fields.Length <= 0)
			{
				return hashtable;
			}
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				string name = fieldInfo.Name;
				object value = fieldInfo.GetValue(t);
				hashtable.Add(name, value);
			}
			return hashtable;
		}
		private DataTable GetPropertieTable(ArrayList propertieList)
		{
			DataTable dataTable = new DataTable();
			foreach (string columnName in propertieList)
			{
				dataTable.Columns.Add(columnName, typeof(string));
			}
			return dataTable;
		}
		private void FillPropertieData(DataTable dt, Hashtable ht, ArrayList al)
		{
			DataRow dataRow = dt.NewRow();
			foreach (string text in al)
			{
				dataRow[text] = ht[text].ToString();
			}
			dt.Rows.Add(dataRow);
		}
		private DataTable GetPropertiDataTable<T>(List<T> tList)
		{
			if (tList.Count == 0)
			{
				return new DataTable();
			}
			T t = tList[0];
			ArrayList propertieName = this.GetPropertieName(t);
			DataTable propertieTable = this.GetPropertieTable(propertieName);
			foreach (T current in tList)
			{
				Hashtable propertieNameHash = this.GetPropertieNameHash(current);
				this.FillPropertieData(propertieTable, propertieNameHash, propertieName);
			}
			return propertieTable;
		}
		private void button7_Click(object sender, EventArgs e)
		{
			TradeLibrary tradeLibrary = new TradeLibrary();
			tradeLibrary.CommunicationUrl = "http://localhost/PMESHost/DoRequest.aspx";
			tradeLibrary.Initialize();
			tradeLibrary.CommodityQuery(new CommodityQueryRequestVO
			{
				CommodityID = string.Empty,
				UserID = "2000",
				MarketID = string.Empty
			});
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
			this.button4 = new Button();
			this.button5 = new Button();
			this.button6 = new Button();
			this.dataGridView1 = new DataGridView();
			this.button7 = new Button();
			((ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			this.button1.Location = new Point(22, 39);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(135, 39);
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
			this.button4.Location = new Point(22, 114);
			this.button4.Name = "button4";
			this.button4.Size = new Size(75, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Logon";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.button5.Location = new Point(22, 143);
			this.button5.Name = "button5";
			this.button5.Size = new Size(75, 23);
			this.button5.TabIndex = 5;
			this.button5.Text = "HQ";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new EventHandler(this.button5_Click);
			this.button6.Location = new Point(22, 173);
			this.button6.Name = "button6";
			this.button6.Size = new Size(100, 23);
			this.button6.TabIndex = 6;
			this.button6.Text = "HoldingDetail";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new EventHandler(this.button6_Click);
			this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new Point(12, 202);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Size = new Size(752, 255);
			this.dataGridView1.TabIndex = 7;
			this.button7.Location = new Point(140, 114);
			this.button7.Name = "button7";
			this.button7.Size = new Size(75, 23);
			this.button7.TabIndex = 8;
			this.button7.Text = "Commodity";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new EventHandler(this.button7_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(777, 467);
			base.Controls.Add(this.button7);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.button6);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Name = "Form1";
			this.Text = "Form1";
			((ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
