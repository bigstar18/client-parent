using System;
using System.IO;
using System.Text;
namespace YrdceClient.Yrdce.Common.Library
{
	public class RecordUserName
	{
		private string userNameFile = "user.memory";
		public void recordUsername(ReCordUserInfo recordUserinfo)
		{
			FileStream fileStream = new FileStream(this.userNameFile, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.BigEndianUnicode);
			binaryWriter.Write(recordUserinfo.isMemoryChecked);
			binaryWriter.Write(recordUserinfo.isProtectChecked);
			if (recordUserinfo.isMemoryChecked)
			{
				binaryWriter.Write(Cryptographic.encode(recordUserinfo.username));
			}
			binaryWriter.Close();
			fileStream.Close();
		}
		public ReCordUserInfo readUsername()
		{
			if (File.Exists(this.userNameFile))
			{
				FileStream fileStream = new FileStream("user.memory", FileMode.Open);
				BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.BigEndianUnicode);
				ReCordUserInfo reCordUserInfo = new ReCordUserInfo();
				try
				{
					reCordUserInfo.isMemoryChecked = binaryReader.ReadBoolean();
					reCordUserInfo.isProtectChecked = binaryReader.ReadBoolean();
					reCordUserInfo.username = Cryptographic.decode(binaryReader.ReadString());
				}
				catch
				{
					reCordUserInfo.username = "";
				}
				binaryReader.Close();
				fileStream.Close();
				return reCordUserInfo;
			}
			return null;
		}
	}
}
