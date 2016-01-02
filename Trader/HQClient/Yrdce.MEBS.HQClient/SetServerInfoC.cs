using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
namespace Gnnt.MEBS.HQClient
{
	public class SetServerInfoC : UserControl
	{
		public delegate void dataChangeEventHander(object sender, EventArgs e);
		private int serverType;
		private bool disPlayIp = true;
		private string myXmlPath;
		private XmlDocument myXmlDoc;
		private int myCurServer;
		private string strText = string.Empty;
		private string strCurTelecomServer = string.Empty;
		private string strCurNetcomServer = string.Empty;
		private ArrayList telecomServerList = new ArrayList();
		private ArrayList netcomServerList = new ArrayList();
		private XmlNode xnConfigInfo;
		private bool isUpdateServer;
		private IContainer components;
		public event SetServerInfoC.dataChangeEventHander dataChange;
		public string XmlPath
		{
			get
			{
				return this.myXmlPath;
			}
			set
			{
				this.myXmlPath = value;
			}
		}
		public XmlDocument XmlDoc
		{
			get
			{
				return this.myXmlDoc;
			}
			set
			{
				this.myXmlDoc = value;
			}
		}
		public int CurServer
		{
			get
			{
				return this.myCurServer;
			}
			set
			{
				this.myCurServer = value;
			}
		}
		public bool IsUpdateServer
		{
			get
			{
				return this.isUpdateServer;
			}
		}
		public SetServerInfoC(int serverType)
		{
			this.InitializeComponent();
			this.serverType = serverType;
			this.disPlayIp = Tools.StrToBool("true", true);
		}
		private void LoadData()
		{
			this.xnConfigInfo = this.myXmlDoc.SelectSingleNode("ConfigInfo");
			if (this.xnConfigInfo.SelectSingleNode("Text") != null)
			{
				this.strText = this.xnConfigInfo.SelectSingleNode("Text").InnerText;
			}
			if (this.xnConfigInfo.SelectSingleNode("CurTelecomServer") != null)
			{
				this.strCurTelecomServer = this.xnConfigInfo.SelectSingleNode("CurTelecomServer").InnerText;
			}
			if (this.xnConfigInfo.SelectSingleNode("CurNetcomServer") != null)
			{
				this.strCurNetcomServer = this.xnConfigInfo.SelectSingleNode("CurNetcomServer").InnerText;
			}
			XmlNode xmlNode = this.xnConfigInfo.SelectSingleNode("AllTelecomServer");
			if (xmlNode != null)
			{
				XmlNodeList childNodes = xmlNode.ChildNodes;
				foreach (XmlNode xmlNode2 in childNodes)
				{
					MoudelServerInfo moudelServerInfo = new MoudelServerInfo();
					XmlElement xmlElement = (XmlElement)xmlNode2;
					if (xmlElement.SelectSingleNode("ServerName") != null)
					{
						moudelServerInfo.ServerName = xmlElement.SelectSingleNode("ServerName").InnerText;
					}
					if (xmlElement.SelectSingleNode("IPAddress") != null)
					{
						moudelServerInfo.IP_Address = xmlElement.SelectSingleNode("IPAddress").InnerText;
					}
					if (xmlElement.SelectSingleNode("Port") != null)
					{
						moudelServerInfo.Port = Tools.StrToInt(xmlElement.SelectSingleNode("Port").InnerText);
					}
					if (xmlElement.SelectSingleNode("HttpPort") != null)
					{
						moudelServerInfo.HttpPort = Tools.StrToInt(xmlElement.SelectSingleNode("HttpPort").InnerText);
					}
					this.telecomServerList.Add(moudelServerInfo);
				}
			}
			XmlNode xmlNode3 = this.xnConfigInfo.SelectSingleNode("AllNetcomServer");
			if (xmlNode != null)
			{
				XmlNodeList childNodes2 = xmlNode3.ChildNodes;
				foreach (XmlNode xmlNode4 in childNodes2)
				{
					MoudelServerInfo moudelServerInfo2 = new MoudelServerInfo();
					XmlElement xmlElement2 = (XmlElement)xmlNode4;
					if (xmlElement2.SelectSingleNode("ServerName") != null)
					{
						moudelServerInfo2.ServerName = xmlElement2.SelectSingleNode("ServerName").InnerText;
					}
					if (xmlElement2.SelectSingleNode("IPAddress") != null)
					{
						moudelServerInfo2.IP_Address = xmlElement2.SelectSingleNode("IPAddress").InnerText;
					}
					if (xmlElement2.SelectSingleNode("Port") != null)
					{
						moudelServerInfo2.Port = Tools.StrToInt(xmlElement2.SelectSingleNode("Port").InnerText);
					}
					if (xmlElement2.SelectSingleNode("HttpPort") != null)
					{
						moudelServerInfo2.HttpPort = Tools.StrToInt(xmlElement2.SelectSingleNode("HttpPort").InnerText);
					}
					this.netcomServerList.Add(moudelServerInfo2);
				}
			}
		}
		public void Initialize()
		{
			this.LoadData();
			int num = 20;
			int num2 = 5;
			GroupBox groupBox = new GroupBox();
			groupBox.Text = this.xnConfigInfo.SelectSingleNode("Text").InnerText;
			groupBox.Size = new Size(base.Width - 16, this.telecomServerList.Count * 30 + 80);
			groupBox.Dock = DockStyle.Top;
			groupBox.Name = "CurTelecomServer";
			base.Controls.Add(groupBox);
			if (this.myCurServer != 0)
			{
				groupBox.Enabled = false;
			}
			num2 += 30;
			RadioButton radioButton = new RadioButton();
			radioButton.Text = "系统自动选择服务器";
			radioButton.AutoSize = true;
			radioButton.Location = new Point(num, num2);
			radioButton.Name = "rbServer-1";
			if (Tools.StrToInt(this.strCurTelecomServer) == -1)
			{
				radioButton.Checked = true;
			}
			radioButton.Click += new EventHandler(this.hqControl_Click);
			groupBox.Controls.Add(radioButton);
			for (int i = 0; i < this.telecomServerList.Count; i++)
			{
				num = 20;
				num2 = (i + 2) * 30 + 5;
				MoudelServerInfo moudelServerInfo = (MoudelServerInfo)this.telecomServerList[i];
				radioButton = new RadioButton();
				radioButton.Text = moudelServerInfo.ServerName;
				radioButton.AutoSize = true;
				radioButton.Location = new Point(num, num2);
				radioButton.Name = "rbServer" + i;
				if (Tools.StrToInt(this.strCurTelecomServer) == i)
				{
					radioButton.Checked = true;
				}
				radioButton.Click += new EventHandler(this.hqControl_Click);
				groupBox.Controls.Add(radioButton);
				num += radioButton.Width;
				Label label = new Label();
				label.Location = new Point(num, num2);
				label.Text = "IP地址 ";
				label.Name = "lbIP" + moudelServerInfo.ServerName;
				label.AutoSize = true;
				groupBox.Controls.Add(label);
				num += label.Width;
				TextBox textBox = new TextBox();
				textBox.Location = new Point(num, num2);
				textBox.Text = moudelServerInfo.IP_Address;
				textBox.Name = "tbIP" + moudelServerInfo.ServerName;
				textBox.TextChanged += new EventHandler(this.hqControl_Click);
				groupBox.Controls.Add(textBox);
				num += textBox.Width;
				Label label2 = new Label();
				label2.Location = new Point(num, num2);
				label2.Text = " 端口 ";
				label2.Name = "lbPort" + moudelServerInfo.ServerName;
				label2.AutoSize = true;
				groupBox.Controls.Add(label2);
				num += label2.Width;
				TextBox textBox2 = new TextBox();
				textBox2.Location = new Point(num, num2);
				textBox2.MaxLength = 10;
				textBox2.Text = moudelServerInfo.Port.ToString();
				textBox2.Name = "tbPort" + moudelServerInfo.ServerName;
				textBox2.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
				textBox2.TextChanged += new EventHandler(this.hqControl_Click);
				groupBox.Controls.Add(textBox2);
				num += textBox2.Width;
				if (this.serverType == 1)
				{
					Label label3 = new Label();
					label3.Location = new Point(num, num2);
					label3.Text = " Http端口 ";
					label3.Name = "lbHttpPort" + moudelServerInfo.ServerName;
					label3.AutoSize = true;
					groupBox.Controls.Add(label3);
					num += label3.Width;
					TextBox textBox3 = new TextBox();
					textBox3.Location = new Point(num, num2);
					textBox3.MaxLength = 10;
					textBox3.Text = moudelServerInfo.HttpPort.ToString();
					textBox3.Name = "tbHttpPort" + moudelServerInfo.ServerName;
					textBox3.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
					textBox3.TextChanged += new EventHandler(this.hqControl_Click);
					groupBox.Controls.Add(textBox3);
					num += textBox3.Width;
					if (!this.disPlayIp)
					{
						label3.Visible = false;
						textBox3.Visible = false;
					}
				}
				if (!this.disPlayIp)
				{
					label.Visible = false;
					textBox.Visible = false;
					label2.Visible = false;
					textBox2.Visible = false;
				}
			}
			num = 20;
			num2 = 5;
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = this.xnConfigInfo.SelectSingleNode("Text").InnerText;
			groupBox2.Size = new Size(base.Width - 16, this.netcomServerList.Count * 30 + 80);
			groupBox2.Dock = DockStyle.Top;
			groupBox2.Name = "CurNetcomServer";
			base.Controls.Add(groupBox2);
			if (this.myCurServer != 1)
			{
				groupBox2.Enabled = false;
			}
			num2 += 30;
			radioButton = new RadioButton();
			radioButton.Text = "系统自动选择服务器";
			radioButton.AutoSize = true;
			radioButton.Location = new Point(num, num2);
			radioButton.Name = "rbServer-1";
			if (Tools.StrToInt(this.strCurNetcomServer) == -1)
			{
				radioButton.Checked = true;
			}
			radioButton.Click += new EventHandler(this.hqControl_Click);
			groupBox2.Controls.Add(radioButton);
			for (int j = 0; j < this.netcomServerList.Count; j++)
			{
				num = 20;
				num2 = (j + 2) * 30 + 5;
				MoudelServerInfo moudelServerInfo2 = (MoudelServerInfo)this.netcomServerList[j];
				radioButton = new RadioButton();
				radioButton.Text = moudelServerInfo2.ServerName;
				radioButton.AutoSize = true;
				radioButton.Location = new Point(num, num2);
				radioButton.Name = "rbServer" + j;
				if (Tools.StrToInt(this.strCurNetcomServer) == j)
				{
					radioButton.Checked = true;
				}
				radioButton.Click += new EventHandler(this.hqControl_Click);
				groupBox2.Controls.Add(radioButton);
				num += radioButton.Width;
				Label label4 = new Label();
				label4.Location = new Point(num, num2);
				label4.Text = "IP地址 ";
				label4.Name = "lbIP" + moudelServerInfo2.ServerName;
				label4.AutoSize = true;
				groupBox2.Controls.Add(label4);
				num += label4.Width;
				TextBox textBox4 = new TextBox();
				textBox4.Location = new Point(num, num2);
				textBox4.Text = moudelServerInfo2.IP_Address;
				textBox4.Name = "tbIP" + moudelServerInfo2.ServerName;
				textBox4.TextChanged += new EventHandler(this.hqControl_Click);
				groupBox2.Controls.Add(textBox4);
				num += textBox4.Width;
				Label label5 = new Label();
				label5.Location = new Point(num, num2);
				label5.Text = " 端口 ";
				label5.Name = "lbPort" + moudelServerInfo2.ServerName;
				label5.AutoSize = true;
				groupBox2.Controls.Add(label5);
				num += label5.Width;
				TextBox textBox5 = new TextBox();
				textBox5.Location = new Point(num, num2);
				textBox5.MaxLength = 10;
				textBox5.Text = moudelServerInfo2.Port.ToString();
				textBox5.Name = "tbPort" + moudelServerInfo2.ServerName;
				textBox5.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
				textBox5.TextChanged += new EventHandler(this.hqControl_Click);
				groupBox2.Controls.Add(textBox5);
				num += textBox5.Width;
				if (this.serverType == 1)
				{
					Label label6 = new Label();
					label6.Location = new Point(num, num2);
					label6.Text = " Http端口 ";
					label6.Name = "lbHttpPort" + moudelServerInfo2.ServerName;
					label6.AutoSize = true;
					groupBox2.Controls.Add(label6);
					num += label6.Width;
					TextBox textBox6 = new TextBox();
					textBox6.Location = new Point(num, num2);
					textBox6.MaxLength = 10;
					textBox6.Text = moudelServerInfo2.HttpPort.ToString();
					textBox6.Name = "tbHttpPort" + moudelServerInfo2.ServerName;
					textBox6.KeyPress += new KeyPressEventHandler(this.tbPort_KeyPress);
					textBox6.TextChanged += new EventHandler(this.hqControl_Click);
					groupBox2.Controls.Add(textBox6);
					num += textBox6.Width;
					if (!this.disPlayIp)
					{
						label6.Visible = false;
						textBox6.Visible = false;
					}
				}
				if (!this.disPlayIp)
				{
					label4.Visible = false;
					textBox4.Visible = false;
					label5.Visible = false;
					textBox5.Visible = false;
				}
			}
		}
		private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if ((textBox.Text.Length == 0 || textBox.SelectedText.Length == textBox.Text.Length) && e.KeyChar == '0')
			{
				MessageBox.Show("端口不能以0开始!");
				e.Handled = true;
			}
		}
		private void hqControl_Click(object sender, EventArgs e)
		{
			this.isUpdateServer = true;
			if (this.dataChange != null)
			{
				this.dataChange(this, e);
			}
		}
		public void updateServerInfo()
		{
			foreach (Control control in base.Controls)
			{
				if (control is GroupBox)
				{
					foreach (Control control2 in control.Controls)
					{
						if (control2 is RadioButton)
						{
							RadioButton radioButton = (RadioButton)control2;
							if (radioButton.Checked)
							{
								this.UpdateConfigInfo(control.Name, control2.Name.Substring(8));
							}
						}
						else
						{
							if (control2.Name.StartsWith("tbIP") && control2 is TextBox)
							{
								TextBox textBox = (TextBox)control2;
								if (textBox.Text.Length == 0)
								{
									throw new Exception(string.Concat(new string[]
									{
										"插件:",
										this.strText,
										" 服务器：",
										control2.Name.Substring(4),
										"  IP地址为空"
									}));
								}
								this.UpdateConfigInfo(control2.Name.Substring(4), "IPAddress", textBox.Text);
							}
							else
							{
								if (control2.Name.StartsWith("tbPort") && control2 is TextBox)
								{
									TextBox textBox2 = (TextBox)control2;
									if (textBox2.Text.Length == 0)
									{
										throw new Exception(string.Concat(new string[]
										{
											"插件:",
											this.strText,
											" 服务器：",
											control2.Name.Substring(6),
											"  端口为空"
										}));
									}
									this.UpdateConfigInfo(control2.Name.Substring(6), "Port", textBox2.Text);
								}
								else
								{
									if (control2.Name.StartsWith("tbHttpPort") && control2 is TextBox)
									{
										TextBox textBox3 = (TextBox)control2;
										if (textBox3.Text.Length == 0)
										{
											throw new Exception(string.Concat(new string[]
											{
												"插件:",
												this.strText,
												" 服务器：",
												control2.Name.Substring(10),
												"  Http端口为空"
											}));
										}
										this.UpdateConfigInfo(control2.Name.Substring(10), "HttpPort", textBox3.Text);
									}
								}
							}
						}
					}
				}
			}
		}
		public bool UpdateConfigInfo(string key, string value)
		{
			bool result = false;
			if (key == null || key.Length == 0)
			{
				return false;
			}
			if (value == null || value.Length == 0)
			{
				return false;
			}
			XmlNode xmlNode = this.myXmlDoc.SelectSingleNode("ConfigInfo");
			if (xmlNode.SelectSingleNode(key) != null)
			{
				xmlNode.SelectSingleNode(key).InnerText = value.Trim();
				this.myXmlDoc.Save(this.myXmlPath);
				result = true;
			}
			return result;
		}
		public bool UpdateConfigInfo(string serverName, string key, string value)
		{
			bool flag = false;
			if (key == null || key.Length == 0)
			{
				return false;
			}
			if (value == null || value.Length == 0)
			{
				return false;
			}
			XmlNode xmlNode = this.myXmlDoc.SelectSingleNode("ConfigInfo");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("AllTelecomServer");
			if (xmlNode2 != null)
			{
				XmlNodeList childNodes = xmlNode2.ChildNodes;
				foreach (XmlNode xmlNode3 in childNodes)
				{
					XmlElement xmlElement = (XmlElement)xmlNode3;
					if (xmlElement.SelectSingleNode("ServerName") != null && xmlElement.SelectSingleNode("ServerName").InnerText.Equals(serverName) && xmlElement.SelectSingleNode(key) != null)
					{
						xmlElement.SelectSingleNode(key).InnerText = value.Trim();
						this.myXmlDoc.Save(this.myXmlPath);
						flag = true;
						bool result = flag;
						return result;
					}
				}
			}
			xmlNode2 = xmlNode.SelectSingleNode("AllNetcomServer");
			if (xmlNode2 != null)
			{
				XmlNodeList childNodes2 = xmlNode2.ChildNodes;
				foreach (XmlNode xmlNode4 in childNodes2)
				{
					XmlElement xmlElement2 = (XmlElement)xmlNode4;
					if (xmlElement2.SelectSingleNode("ServerName") != null && xmlElement2.SelectSingleNode("ServerName").InnerText.Equals(serverName) && xmlElement2.SelectSingleNode(key) != null)
					{
						xmlElement2.SelectSingleNode(key).InnerText = value.Trim();
						this.myXmlDoc.Save(this.myXmlPath);
						flag = true;
						bool result = flag;
						return result;
					}
				}
			}
			return flag;
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "SetServerInfoC";
			base.Size = new Size(664, 325);
			base.ResumeLayout(false);
		}
	}
}
