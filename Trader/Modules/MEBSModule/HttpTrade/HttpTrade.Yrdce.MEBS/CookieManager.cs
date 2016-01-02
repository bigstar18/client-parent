using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using TPME.Log;
namespace HttpTrade.Gnnt.MEBS
{
	public class CookieManager
	{
		private static CookieManager cookieManager;
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
		private CookieManager()
		{
		}
		public static CookieManager getInstance()
		{
			if (CookieManager.cookieManager == null)
			{
				lock (typeof(CookieManager))
				{
					if (CookieManager.cookieManager == null)
					{
						CookieManager.cookieManager = new CookieManager();
					}
				}
			}
			return CookieManager.cookieManager;
		}
		public void SetIECookie(string lpszUrlName, string lbszCookieName, string lpszCookieData, bool isConsumerPlugin = false)
		{
			if (isConsumerPlugin)
			{
				string str = DateTime.Now.AddHours(-8.0).AddSeconds(10.0).ToString("dd-MM-yyyy HH:mm:ss");
				lpszCookieData = lpszCookieData + "; expires=Tue, " + str + " GMT";
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeleteCookie), lpszUrlName);
			}
			CookieManager.InternetSetCookie(lpszUrlName, lbszCookieName, lpszCookieData);
		}
		private void DeleteCookie(object o)
		{
			Thread.Sleep(10000);
			try
			{
				string lpszUrlName = (string)o;
				CookieManager.InternetSetCookie(lpszUrlName, "JSESSIONID", "");
			}
			catch
			{
				Logger.wirte(MsgType.Error, "InternetSetCookie Error");
			}
		}
		public void LotSetCookie(Cookie[] cookies)
		{
			for (int i = 0; i < cookies.Length; i++)
			{
				Cookie cookie = cookies[i];
				this.SetIECookie(cookie.Domain, cookie.Name, cookie.Value, false);
			}
		}
		public void LotSetCookie(Dictionary<string, Cookie> cookies, string path, bool isConsumerPlugin = false)
		{
			foreach (KeyValuePair<string, Cookie> current in cookies)
			{
				this.SetIECookie(path, current.Key, current.Value.Value, isConsumerPlugin);
			}
		}
		public CookieContainer setHttpCookieHeader(CookieContainer cookieContainer, Dictionary<string, Cookie> cookies)
		{
			if (cookies != null && cookies.Count > 0)
			{
				foreach (KeyValuePair<string, Cookie> current in cookies)
				{
					cookieContainer.Add(current.Value);
				}
			}
			return cookieContainer;
		}
		public Dictionary<string, bool> setCookiesNameFlag(bool flag, ref Dictionary<string, bool> cookiesName)
		{
			if (cookiesName != null && cookiesName.Count > 0)
			{
				foreach (KeyValuePair<string, bool> current in cookiesName)
				{
					cookiesName[current.Key] = flag;
				}
			}
			return cookiesName;
		}
		public Dictionary<string, bool> setCookiesName(CookieCollection cookieCollection, ref Dictionary<string, bool> cookiesName)
		{
			if (cookieCollection != null && cookieCollection.Count > 0 && cookiesName != null)
			{
				int count = cookieCollection.Count;
				for (int i = 0; i < count; i++)
				{
					string name = cookieCollection[i].Name;
					if (!cookiesName.ContainsKey(name))
					{
						cookiesName.Add(name, true);
					}
					else
					{
						cookiesName[name] = true;
					}
				}
			}
			return cookiesName;
		}
		public Dictionary<string, Cookie> saveCookies(CookieCollection cookieCollection, ref Dictionary<string, Cookie> cookies, Dictionary<string, bool> cookiesName)
		{
			if (cookieCollection != null && cookieCollection.Count > 0)
			{
				int count = cookieCollection.Count;
				for (int i = count - 1; i >= 0; i--)
				{
					Cookie cookie = cookieCollection[i];
					string name = cookie.Name;
					if (cookiesName == null)
					{
						break;
					}
					if (cookiesName.Count == 0)
					{
						this.setCookiesName(cookieCollection, ref cookiesName);
					}
					if (cookiesName.ContainsKey(name) && cookiesName[name])
					{
						cookiesName[name] = false;
						if (cookies.ContainsKey(name))
						{
							cookies[name] = cookie;
						}
						else
						{
							cookies.Add(name, cookie);
						}
					}
				}
			}
			return cookies;
		}
	}
}
