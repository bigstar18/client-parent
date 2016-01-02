using ModulesLoader;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
namespace YrdceClient.Yrdce.Common.Library
{
	public class ThreadStartAddNodes
	{
		public delegate void ToolsStripButtonClickCallBack();
		private delegate void dPopulateTreeControl(XmlNode document, ToolStripItemCollection nodes);
		private ImageList TreeviewIL = new ImageList();
		private ToolStripItemCollection toolStripItem;
		private string xmlPath = string.Empty;
		private bool isLogin;
		public Image menuBackImage;
		public Image menuClickBackImage;
		public Image menuSplitImage;
		public List<string> NodeNameList = new List<string>();
		public ThreadStartAddNodes.ToolsStripButtonClickCallBack ToolStripButtonClick;
		public static Size nodeSize = new Size(40, 40);
		public static Size new_child_size = new Size(120, 40);
		public static Color new_childForeColor = Color.White;
		public static Font font = new Font("微软雅黑", 12f, FontStyle.Regular);
		public static SolidBrush brush = new SolidBrush(ThreadStartAddNodes.new_childForeColor);
		public ThreadStartAddNodes(ToolStripItemCollection toolStripItem, string xmlPath)
		{
			try
			{
				this.toolStripItem = toolStripItem;
				this.xmlPath = xmlPath;
				this.TreeviewIL.ImageSize = new Size(40, 40);
				//this.menuBackImage = Image.FromFile("images\\menu.png");
				this.menuClickBackImage = Image.FromFile("images\\menuclick.png");
				this.menuSplitImage = Image.FromFile("images\\menusplit.png");
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "初始化资源文件异常：" + ex.Message);
			}
		}
		public void LoadNode(object Login)
		{
			try
			{
				this.isLogin = (bool)Login;
				XmlDocument xmlDocument = new XmlDataDocument();
				xmlDocument.Load(this.xmlPath);
				this.populateTreeControl(xmlDocument.DocumentElement, this.toolStripItem);
				if (this.isLogin)
				{
					this.addNodeChild(null, this.toolStripItem);
				}
				else
				{
					this.addLoginChild(null, this.toolStripItem);
				}
				if (this.ToolStripButtonClick != null)
				{
					this.ToolStripButtonClick();
				}
			}
			catch (FileNotFoundException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void populateTreeControl(XmlNode document, ToolStripItemCollection nodes)
		{
			this.toolStripItem[0].Owner.ImageList = this.TreeviewIL;
			nodes.Clear();
			foreach (XmlNode xmlNode in document.ChildNodes)
			{
				string text = string.Empty;
				if (xmlNode.Attributes != null && xmlNode.Attributes["Name"] != null)
				{
					text = xmlNode.Attributes["Name"].Value;
				}
				if (text.Length == 0)
				{
					Console.WriteLine("节点名称不能为空");
				}
				else
				{
					string text2 = (xmlNode.Value != null) ? xmlNode.Value : ((xmlNode.Attributes != null && xmlNode.Attributes["Text"] != null) ? xmlNode.Attributes["Text"].Value : xmlNode.Name);
					string imagePath = "";
					if (xmlNode.Attributes != null)
					{
						if (xmlNode.Attributes["ImageIco"] != null)
						{
							imagePath = xmlNode.Attributes["ImageIco"].Value;
						}
						if (xmlNode.Attributes["SelectImageIco"] != null)
						{
							string arg_133_0 = xmlNode.Attributes["SelectImageIco"].Value;
						}
					}
					if (xmlNode.Attributes == null || xmlNode.Attributes["ToolTip"] == null)
					{
						string arg_154_0 = xmlNode.Name;
					}
					else
					{
						string arg_16C_0 = xmlNode.Attributes["ToolTip"].Value;
					}
					ToolStripItem toolStripItem = new ToolStripLabel();
					toolStripItem.Name = text;
					toolStripItem.AutoSize = false;
					toolStripItem.Size = ThreadStartAddNodes.new_child_size;
					toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					toolStripItem.Margin = new Padding(0, 0, 0, 0);
					toolStripItem.Image = ThreadStartAddNodes.DrawImageSize(imagePath, text2, toolStripItem.Width, toolStripItem.Height);
					toolStripItem.ImageScaling = ToolStripItemImageScaling.None;
					toolStripItem.ImageAlign = ContentAlignment.MiddleLeft;
					toolStripItem.TextImageRelation = TextImageRelation.ImageBeforeText;
					toolStripItem.TextAlign = ContentAlignment.MiddleRight;
					toolStripItem.Text = text2;
					toolStripItem.Font = ThreadStartAddNodes.font;
					toolStripItem.ForeColor = ThreadStartAddNodes.new_childForeColor;
                    toolStripItem.BackgroundImage = this.menuBackImage;
                    toolStripItem.BackColor = Color.Black;
                    toolStripItem.MouseEnter += new EventHandler(this.new_child_MouseEnter);
					toolStripItem.MouseLeave += new EventHandler(this.new_child_MouseLeave);
					if (xmlNode.Attributes != null && xmlNode.Attributes["Plugins"] != null)
					{
						string value = xmlNode.Attributes["Plugins"].Value;
						if (value.Length > 0)
						{
							string[] array = value.Split(new char[]
							{
								';'
							});
							List<IPlugin> list = new List<IPlugin>();
							for (int i = 0; i < array.Length; i++)
							{
								AvailablePluginInfo availablePluginInfo = Global.Modules.Plugins.AvailablePlugins.Find(array[i]);
								if (availablePluginInfo == null)
								{
									Logger.wirte(MsgType.Error, "没有找到插件" + array[i]);
								}
								else if (!availablePluginInfo.Instance.IsEnable)
								{
									Logger.wirte(MsgType.Error, "插件不可用" + array[i]);
								}
								else
								{
									list.Add(availablePluginInfo.Instance);
								}
							}
							toolStripItem.Tag = list;
						}
					}
					List<IPlugin> list2 = (List<IPlugin>)toolStripItem.Tag;
					if (list2.Count > 0 && list2[0].Name != "ServerSet" && list2[0].Name != "MixAccount")
					{
						bool flag = false;
						if (xmlNode.Attributes["IsLoginedNode"] != null)
						{
							flag = Tools.StrToBool(xmlNode.Attributes["IsLoginedNode"].Value, false);
						}
						if (flag)
						{
							toolStripItem.Visible = false;
							this.NodeNameList.Add(toolStripItem.Name);
						}
                        nodes.Add(toolStripItem);
                    }
				}
			}
		}
		private void new_child_MouseLeave(object sender, EventArgs e)
		{
			ToolStripLabel toolStripLabel = sender as ToolStripLabel;
			if (toolStripLabel != null && toolStripLabel.ToolTipText != "true")
			{
				toolStripLabel.BackgroundImage = this.menuBackImage;
			}
		}
		private void new_child_MouseEnter(object sender, EventArgs e)
		{
			ToolStripLabel toolStripLabel = sender as ToolStripLabel;
			if (toolStripLabel != null)
			{
				toolStripLabel.BackgroundImage = this.menuClickBackImage;
			}
		}
		public static Image DrawImageSize(string imagePath, string text, int width, int height)
		{
			Image image = Image.FromFile(imagePath);
			Image image2 = new Bitmap(ThreadStartAddNodes.nodeSize.Width, ThreadStartAddNodes.nodeSize.Height);
			Image image3 = Image.FromFile("images\\menusplit.png");
			Graphics graphics = Graphics.FromImage(image2);
			graphics.InterpolationMode = InterpolationMode.High;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(Color.Transparent);
			float arg_61_0 = graphics.MeasureString(text, ThreadStartAddNodes.font).Width;
			int num = (int)graphics.MeasureString(text, ThreadStartAddNodes.font).Height;
			new PointF(40f, (float)((height - num) / 2 + 2));
			if (text != null && text.Length > 0)
			{
				graphics.DrawImage(image3, new Rectangle(0, 0, 2, 40), new Rectangle(0, 0, 2, 40), GraphicsUnit.Pixel);
				graphics.DrawImage(image, new Rectangle(15, 8, 25, 25), new Rectangle(0, 0, 25, 25), GraphicsUnit.Pixel);
			}
			else
			{
				graphics.DrawImage(image3, new Rectangle(0, 0, 2, 40), new Rectangle(0, 0, 2, 40), GraphicsUnit.Pixel);
				graphics.DrawImage(image, new Rectangle(15, 8, 25, 25), new Rectangle(0, 0, 25, 25), GraphicsUnit.Pixel);
			}
			return image2;
		}
		public void addNodeChild(XmlNode document, ToolStripItemCollection nodes)
		{
			try
			{
				ToolStripItem toolStripItem = new ToolStripLabel();
				toolStripItem.Name = "logout";
				toolStripItem.AutoSize = false;
				string @string = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_Logout");
				Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_LogoutSys");
				toolStripItem.Size = ThreadStartAddNodes.new_child_size;
				toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				toolStripItem.Margin = new Padding(0, 0, 0, 0);
				toolStripItem.Image = ThreadStartAddNodes.DrawImageSize("images/logout.png", @string, toolStripItem.Width, toolStripItem.Height);
				toolStripItem.ImageScaling = ToolStripItemImageScaling.None;
				toolStripItem.ImageAlign = ContentAlignment.MiddleLeft;
				toolStripItem.TextImageRelation = TextImageRelation.ImageBeforeText;
				toolStripItem.TextAlign = ContentAlignment.MiddleRight;
				toolStripItem.Text = @string;
				toolStripItem.Font = ThreadStartAddNodes.font;
				toolStripItem.ForeColor = ThreadStartAddNodes.new_childForeColor;
				toolStripItem.BackgroundImage = this.menuBackImage;
				toolStripItem.MouseEnter += new EventHandler(this.new_child_MouseEnter);
				toolStripItem.MouseLeave += new EventHandler(this.new_child_MouseLeave);
				ToolStripItem toolStripItem2 = new ToolStripLabel();
				toolStripItem2.Name = "ChangePW";
				toolStripItem2.AutoSize = false;
				string string2 = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_ChangePws");
				toolStripItem2.Size = ThreadStartAddNodes.new_child_size;
				toolStripItem2.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				toolStripItem2.Margin = new Padding(0, 0, 0, 0);
				toolStripItem2.Image = ThreadStartAddNodes.DrawImageSize("images/ChangePW.png", string2, toolStripItem2.Width, toolStripItem2.Height);
				toolStripItem2.ImageScaling = ToolStripItemImageScaling.None;
				toolStripItem2.ImageAlign = ContentAlignment.MiddleLeft;
				toolStripItem2.TextImageRelation = TextImageRelation.ImageBeforeText;
				toolStripItem2.TextAlign = ContentAlignment.MiddleRight;
				toolStripItem2.Text = string2;
				toolStripItem2.Font = ThreadStartAddNodes.font;
				toolStripItem2.ForeColor = ThreadStartAddNodes.new_childForeColor;
				toolStripItem2.BackgroundImage = this.menuBackImage;
				toolStripItem2.MouseEnter += new EventHandler(this.new_child_MouseEnter);
				toolStripItem2.MouseLeave += new EventHandler(this.new_child_MouseLeave);
				nodes.Add(toolStripItem2);
				nodes.Add(toolStripItem);
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, "addNodeChild异常：" + ex.Message);
			}
		}
		public void addLoginChild(XmlNode document, ToolStripItemCollection nodes)
		{
			ToolStripItem toolStripItem = new ToolStripLabel();
			toolStripItem.Name = "login";
			toolStripItem.AutoSize = false;
			toolStripItem.Size = ThreadStartAddNodes.new_child_size;
			toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			toolStripItem.Margin = new Padding(0, 0, 0, 0);
			string text = "";
			if (Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_login") != null)
			{
				text = Global.Modules.Plugins.MEBS_ResourceManager.GetString("TradeStr_SysForm_login");
			}
			if (text == "")
			{
				text = "登  录";
			}
			toolStripItem.Image = ThreadStartAddNodes.DrawImageSize("images/login.png", text, toolStripItem.Width, toolStripItem.Height);
			toolStripItem.ImageScaling = ToolStripItemImageScaling.None;
			toolStripItem.ImageAlign = ContentAlignment.MiddleLeft;
			toolStripItem.TextImageRelation = TextImageRelation.ImageBeforeText;
			toolStripItem.TextAlign = ContentAlignment.MiddleRight;
			toolStripItem.Text = text;
			toolStripItem.Font = ThreadStartAddNodes.font;
			toolStripItem.ForeColor = ThreadStartAddNodes.new_childForeColor;
			toolStripItem.BackgroundImage = this.menuBackImage;
			toolStripItem.MouseEnter += new EventHandler(this.new_child_MouseEnter);
			toolStripItem.MouseLeave += new EventHandler(this.new_child_MouseLeave);
			nodes.Add(toolStripItem);
		}
		public static int CreateDeptImageList(ImageList treeviewIL, string path)
		{
			int result = -1;
			try
			{
				if (treeviewIL != null && path != null && path.Length > 0)
				{
					if (treeviewIL.Images.ContainsKey(path))
					{
						result = treeviewIL.Images.IndexOfKey(path);
					}
					else
					{
						Image image = Image.FromFile(path);
						treeviewIL.Images.Add(path, image);
						result = treeviewIL.Images.Count - 1;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
	}
}
