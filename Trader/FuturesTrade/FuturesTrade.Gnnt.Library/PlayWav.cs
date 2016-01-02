using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using TPME.Log;
namespace FuturesTrade.Gnnt.Library
{
	public class PlayWav
	{
		public const uint SND_ASYNC = 1u;
		public const uint SND_MEMORY = 4u;
		[DllImport("Winmm.dll")]
		public static extern bool PlaySound(byte[] data, IntPtr hMod, uint dwFlags);
		public static void PlayWavResource(string wav, int type)
		{
			string str = Assembly.GetExecutingAssembly().GetName().Name.ToString();
			Stream stream = null;
			if (type == 0)
			{
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(str + "." + wav);
			}
			else
			{
				if (type == 1)
				{
					try
					{
						stream = File.OpenRead(Global.GetFullPath(wav));
					}
					catch
					{
						Logger.wirte(MsgType.Error, "没有找到文件！");
					}
				}
			}
			if (stream == null)
			{
				return;
			}
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, (int)stream.Length);
			PlayWav.PlaySound(array, IntPtr.Zero, 5u);
		}
	}
}
