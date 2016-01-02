using HttpTrade.Gnnt.OTC.Lib;
using HttpTrade.Gnnt.OTC.VO;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC
{
	public class HTTPCommunicate
	{
		private string url;
		private string safeUrl;
		private IWebProxy proxy;
		private object lockA = new object();
		private string outcookie = "";
		private bool isSetCookie;
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value.Replace("\\", "/");
			}
		}
		public string SafeUrl
		{
			get
			{
				return this.safeUrl;
			}
			set
			{
				this.safeUrl = value.Replace("\\", "/");
			}
		}
		public HTTPCommunicate() : this(null, null)
		{
		}
		public HTTPCommunicate(string url, string safeUrl)
		{
			try
			{
				this.url = url.Trim();
				this.safeUrl = safeUrl.Trim();
				if (this.safeUrl.Length == 0)
				{
					this.safeUrl = this.url;
				}
				this.GetWebProxy();
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
		private IWebProxy GetWebProxy()
		{
			IWebProxy webProxy = null;
			IWebProxy result;
			try
			{
				IniFile iniFile = new IniFile("Proxy.ini");
				bool flag = bool.Parse(iniFile.IniReadValue("ProxyServer", "Enable"));
				bool flag2 = Tools.StrToBool(iniFile.IniReadValue("ProxyServer", "IeEnable"), false);
				int num = int.Parse(iniFile.IniReadValue("ProxyServer", "Type"));
				string arg = iniFile.IniReadValue("ProxyServer", "ProxyIP");
				int num2 = int.Parse(iniFile.IniReadValue("ProxyServer", "ProxyPort"));
				if (flag)
				{
					if (2 == num)
					{
						webProxy = new WebProxy(string.Format("{0}:{1}/", arg, num2), true);
					}
				}
				else if (flag2)
				{
					webProxy = WebRequest.GetSystemWebProxy();
				}
				result = webProxy;
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
		public RepVO commuteBYVO(ReqVO requestVO)
		{
			bool type = this.GetType(requestVO.Name);
			RepVO repVO = null;
			string xml = string.Empty;
			try
			{
				if (type)
				{
					short iD = requestVO.ID;
					BinaryReader binaryReader = this.commute(requestVO.DeSerializationobj(), this.url);
					if (binaryReader != null)
					{
						repVO = RepVO.GetResponseVO(requestVO.Name, binaryReader);
						if (iD != repVO.ID)
						{
							throw new Exception("请求包与返回包名称不一致！");
						}
					}
				}
				else
				{
					repVO = RepVO.GetResponseVO(requestVO.Name, null);
					if (repVO != null)
					{
						xml = this.commute(requestVO.toXmlString(), this.safeUrl);
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(xml);
						XmlNode xmlNode = xmlDocument.SelectSingleNode("GNNT").SelectSingleNode("REP");
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (!xmlElement.GetAttribute("name").Equals(requestVO.Name.ToString()))
						{
							throw new Exception("请求包与返回包名称不一致！");
						}
						this.setValue(repVO, xmlNode);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				return null;
			}
			return repVO;
		}
		private bool GetType(ProtocolName name)
		{
			bool result = false;
			switch (name)
			{
			case ProtocolName.logon:
			case ProtocolName.logoff:
			case ProtocolName.check_user:
			case ProtocolName.change_password:
			case ProtocolName.firm_info:
			case ProtocolName.order_s:
			case ProtocolName.order_x:
			case ProtocolName.order_wd:
			case ProtocolName.my_order_query:
			case ProtocolName.tradequery:
			case ProtocolName.holding_query:
			case ProtocolName.holding_detail_query:
			case ProtocolName.commodity_query:
			case ProtocolName.set_loss_profit:
			case ProtocolName.withdraw_loss_profit:
			case ProtocolName.firm_funds_info:
			case ProtocolName.customer_order_query:
			case ProtocolName.other_firm_query:
				break;
			case ProtocolName.c_d_q:
			case ProtocolName.c_d_q_m:
				result = true;
				break;
			case ProtocolName.sys_time_query:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}
		private void setValue(object sourceObj, XmlNode xn)
		{
			if (xn == null)
			{
				return;
			}
			try
			{
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
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		private BinaryReader HttpWebResponsePost(string url, MemoryStream postData, string encodeType, string cookie, out string outcookie)
		{
			BinaryReader result;
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Proxy = this.proxy;
				if (httpWebRequest.Proxy != null)
				{
					httpWebRequest.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
				}
				CookieContainer cookieContainer = new CookieContainer();
				httpWebRequest.CookieContainer = cookieContainer;
				if (cookie != "")
				{
					httpWebRequest.CookieContainer.SetCookies(new Uri(url), cookie);
				}
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "binary";
				httpWebRequest.ContentLength = postData.Length;
				httpWebRequest.ServicePoint.Expect100Continue = false;
				Stream requestStream = httpWebRequest.GetRequestStream();
				byte[] buffer = postData.GetBuffer();
				requestStream.Write(buffer, 0, Convert.ToInt32(postData.Length));
				requestStream.Close();
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				httpWebResponse.GetResponseStream();
				outcookie = cookie;
				CookieCollection cookies = httpWebRequest.CookieContainer.GetCookies(new Uri(url));
				if (cookies != null && cookies.Count > 0)
				{
					int count = cookies.Count;
					for (int i = count - 1; i >= 0; i--)
					{
						if (cookies[i].Name.ToUpper().Equals("JSESSIONID"))
						{
							outcookie = string.Format("JSESSIONID={0}", cookies[i].Value);
							break;
						}
					}
				}
				if (cookie.Equals(outcookie))
				{
					Logger.wirte(1, cookie);
				}
				else
				{
					Logger.wirte(2, string.Format("cookie不一致，协议二进制 in={0},out={1}", cookie, outcookie));
				}
				HttpWebResponse httpWebResponse2 = (HttpWebResponse)httpWebRequest.GetResponse();
				BinaryReader binaryReader = new BinaryReader(httpWebResponse2.GetResponseStream());
				result = binaryReader;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				outcookie = cookie;
				result = null;
			}
			return result;
		}
		private string HttpWebResponsePost(string url, string postData, string encodeType, string cookie, out string outcookie)
		{
			Encoding encoding = Encoding.Default;
			if (encodeType != null && encodeType.Length > 0)
			{
				encoding = Encoding.GetEncoding(encodeType);
			}
			byte[] bytes = encoding.GetBytes(postData);
			string result;
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Proxy = this.proxy;
				if (httpWebRequest.Proxy != null)
				{
					httpWebRequest.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
				}
				CookieContainer cookieContainer = new CookieContainer();
				httpWebRequest.CookieContainer = cookieContainer;
				if (cookie != "")
				{
					httpWebRequest.CookieContainer.SetCookies(new Uri(url), cookie);
				}
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				outcookie = cookie;
				CookieCollection cookies = httpWebRequest.CookieContainer.GetCookies(new Uri(url));
				if (cookies != null && cookies.Count > 0)
				{
					int count = cookies.Count;
					for (int i = count - 1; i >= 0; i--)
					{
						if (cookies[i].Name.ToUpper().Equals("JSESSIONID"))
						{
							outcookie = string.Format("JSESSIONID={0}", cookies[i].Value);
							break;
						}
					}
				}
				if (cookie.Equals(outcookie))
				{
					Logger.wirte(1, cookie);
				}
				else
				{
					Logger.wirte(2, string.Format("cookie不一致 in={0},out={1}", cookie, outcookie));
					Logger.wirte(1, string.Format("cookie不一致 postdata={0}", postData));
				}
				StreamReader streamReader = new StreamReader(responseStream, encoding);
				string text = streamReader.ReadToEnd();
				result = text;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				outcookie = cookie;
				result = string.Empty;
			}
			return result;
		}
		private string commute(string sendStr, string commUrl)
		{
			string result;
			try
			{
				if (commUrl == null || commUrl.Length == 0)
				{
					throw new Exception("通讯地址为空！");
				}
				Logger.wirte(1, sendStr);
				string text = this.HttpWebResponsePost(commUrl, sendStr, "", this.outcookie, out this.outcookie);
				if (!this.isSetCookie && this.outcookie.Length > 11)
				{
					Uri uri = new Uri(commUrl);
					if (uri.Segments.Length >= 1)
					{
						string text2 = uri.GetLeftPart(UriPartial.Authority) + uri.Segments[0];
						for (int i = 1; i < uri.Segments.Length; i++)
						{
							text2 += uri.Segments[i];
							HTTPCommunicate.InternetSetCookie(text2, "JSESSIONID", this.outcookie.Substring(11));
						}
					}
					else
					{
						HTTPCommunicate.InternetSetCookie(commUrl, "JSESSIONID", this.outcookie.Substring(11));
					}
					this.isSetCookie = true;
				}
				Logger.wirte(1, text);
				result = text;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = string.Empty;
			}
			return result;
		}
		private BinaryReader commute(MemoryStream sendbin, string commUrl)
		{
			if (commUrl == null || commUrl.Length == 0)
			{
				throw new Exception("通讯地址为空！");
			}
			BinaryReader result;
			try
			{
				BinaryReader binaryReader = this.HttpWebResponsePost(commUrl, sendbin, "", this.outcookie, out this.outcookie);
				if (!this.isSetCookie && this.outcookie.Length > 11)
				{
					Uri uri = new Uri(commUrl);
					if (uri.Segments.Length >= 1)
					{
						string text = uri.GetLeftPart(UriPartial.Authority) + uri.Segments[0];
						for (int i = 1; i < uri.Segments.Length; i++)
						{
							text += uri.Segments[i];
							HTTPCommunicate.InternetSetCookie(text, "JSESSIONID", this.outcookie.Substring(11));
						}
					}
					else
					{
						HTTPCommunicate.InternetSetCookie(commUrl, "JSESSIONID", this.outcookie.Substring(11));
					}
					this.isSetCookie = true;
				}
				result = binaryReader;
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				result = null;
			}
			return result;
		}
		private int charIndexOfString(char c, string str, int count)
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < str.Length; i++)
			{
				char c2 = str[i];
				num2++;
				if (c2 == c)
				{
					num++;
					if (num == count)
					{
						break;
					}
				}
			}
			if (num == count)
			{
				return num2;
			}
			return -1;
		}
	}
}
