namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using TPME.Log;

    public class PlayWav
    {
        public const uint SND_ASYNC = 1;
        public const uint SND_MEMORY = 4;

        [DllImport("Winmm.dll")]
        public static extern bool PlaySound(byte[] data, IntPtr hMod, uint dwFlags);
        public static void PlayWavResource(string wav, int type)
        {
            string str = Assembly.GetExecutingAssembly().GetName().Name.ToString();
            Stream manifestResourceStream = null;
            if (type == 0)
            {
                manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(str + "." + wav);
            }
            else if (type == 1)
            {
                try
                {
                    manifestResourceStream = File.OpenRead(Global.GetFullPath(wav));
                }
                catch
                {
                    Logger.wirte(MsgType.Error, "没有找到文件！");
                }
            }
            if (manifestResourceStream != null)
            {
                byte[] buffer = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(buffer, 0, (int)manifestResourceStream.Length);
                PlaySound(buffer, IntPtr.Zero, 5);
            }
        }
    }
}
