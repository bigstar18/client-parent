using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class PlayWav
	{
		public const uint SND_ASYNC = 1u;
		public const uint SND_MEMORY = 4u;
		[DllImport("Winmm.dll")]
		public static extern bool PlaySound(byte[] data, IntPtr hMod, uint dwFlags);
		public static void PlayWavResource(string wav, int type)
		{
			try
			{
				string str = Assembly.GetExecutingAssembly().GetName().Name.ToString();
				Stream stream = null;
				if (type == 0)
				{
					stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(str + "." + wav);
				}
				else if (type == 1)
				{
					try
					{
						stream = File.OpenRead(Global.GetFullPath(wav));
					}
					catch (Exception ex)
					{
						Logger.wirte(2, ex.Message);
					}
				}
				if (stream != null)
				{
					byte[] array = new byte[stream.Length];
					stream.Read(array, 0, (int)stream.Length);
					PlayWav.PlaySound(array, IntPtr.Zero, 5u);
				}
			}
			catch (Exception ex2)
			{
				Logger.wirte(ex2);
			}
		}
	}
}
