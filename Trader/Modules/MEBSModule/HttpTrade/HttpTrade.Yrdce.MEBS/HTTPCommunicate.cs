using HttpTrade.Gnnt.MEBS.Lib;
using HttpTrade.Gnnt.MEBS.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using ToolsLibrary.util;
using TPME.Log;
namespace HttpTrade.Gnnt.MEBS
{
	public class HTTPCommunicate
	{
		private string url;
		private IWebProxy proxy;
		private CookieManager cookieManager;
		private object lockA = new object();
		public Dictionary<string, Cookie> cookies = new Dictionary<string, Cookie>();
		private Dictionary<string, bool> cookiesName = new Dictionary<string, bool>();
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
		public HTTPCommunicate() : this(null)
		{
		}
		public HTTPCommunicate(string url)
		{
			this.url = url;
            Lib.IniFile iniFile = new Lib.IniFile("Proxy.ini");
			if (Tools.StrToBool(iniFile.IniReadValue("ProxyServer", "Enable"), false))
			{
				this.proxy = WebRequest.GetSystemWebProxy();
			}
			else
			{
				this.proxy = null;
			}
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
			this.cookieManager = CookieManager.getInstance();
		}
		public RepVO commuteBYVO(ReqVO requestVO, bool isConsumerPlugin = false)
		{
			RepVO responseVO = RepVO.GetResponseVO(requestVO.Name);
			if (responseVO != null)
			{
				string xml = string.Empty;
				try
				{
					xml = this.commute(requestVO.toXmlString(), isConsumerPlugin);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("GNNT").SelectSingleNode("REP");
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (!xmlElement.GetAttribute("name").Equals(requestVO.Name.ToString()))
					{
						throw new Exception("请求包与返回包名称不一致！");
					}
					this.setValue(responseVO, xmlNode);
				}
				catch (Exception ex)
				{
					Logger.wirte(MsgType.Error, "通讯异常：" + ex.ToString());
                    
					return null;
				}
				return responseVO;
			}
			return responseVO;
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
		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		private string HttpWebResponsePost(string url, string postData, string encodeType, bool isConsumerPluginRes = false)
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
				httpWebRequest.CookieContainer = this.cookieManager.setHttpCookieHeader(new CookieContainer(), this.cookies);
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "text/html";
				httpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				this.cookieManager.setCookiesName(httpWebRequest.CookieContainer.GetCookies(new Uri(url)), ref this.cookiesName);
				this.cookieManager.saveCookies(httpWebRequest.CookieContainer.GetCookies(new Uri(url)), ref this.cookies, this.cookiesName);
				StreamReader streamReader = new StreamReader(responseStream, encoding);
				string text = streamReader.ReadToEnd();
				result = text;
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}
		private string commute(string sendStr, bool isConsumerPlugin = false)
		{
			if (this.url == null || this.url.Length == 0)
			{
				throw new Exception("通讯地址为空！");
			}
			string result;
			try
			{
				Logger.wirte(MsgType.Information, "请求内容：");
				Logger.wirte(MsgType.Information, sendStr);
				string text = this.HttpWebResponsePost(this.url, sendStr, "GBK", isConsumerPlugin);
				if ((!this.isSetCookie && this.cookies.Count > 0) || isConsumerPlugin)
				{
					Uri uri = new Uri(this.url);
					if (uri.Segments.Length >= 1)
					{
						string text2 = uri.GetLeftPart(UriPartial.Authority) + uri.Segments[0];
						for (int i = 1; i < uri.Segments.Length; i++)
						{
							text2 += uri.Segments[i];
							this.cookieManager.LotSetCookie(this.cookies, text2, isConsumerPlugin);
						}
					}
					else
					{
						this.cookieManager.LotSetCookie(this.cookies, this.url, isConsumerPlugin);
					}
					this.isSetCookie = true;
				}
				Logger.wirte(MsgType.Information, "返回内容：");
				Logger.wirte(MsgType.Information, text);
				result = text;
			}
			catch (Exception ex)
			{
				throw ex;
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
